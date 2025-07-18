﻿using AutoMapper;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.QueryHandlers;

public class GetAllWorkingTimeRecordsQueryHandler : IQueryHandler<GetAllWorkingTimeRecordsQuery, IEnumerable<EmployeeWorkingTimeRecordViewModel>>
{
    private readonly BomaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IEmployeeWorkingTimeRecordCalculationDomainService _employeeWorkingTimeRecordCalculationDomainService;
    private readonly ISalaryCalculationDomainService _salaryCalculationDomainService;

    public GetAllWorkingTimeRecordsQueryHandler(
        BomaDbContext dbContext,
        IMapper mapper,
        IMediator mediator,
        IEmployeeWorkingTimeRecordCalculationDomainService employeeWorkingTimeRecordCalculationDomainService,
        ISalaryCalculationDomainService salaryCalculationDomainService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _mediator = mediator;
        _employeeWorkingTimeRecordCalculationDomainService = employeeWorkingTimeRecordCalculationDomainService;
        _salaryCalculationDomainService = salaryCalculationDomainService;
    }
    
    public async Task<IEnumerable<EmployeeWorkingTimeRecordViewModel>> Handle(GetAllWorkingTimeRecordsQuery query, CancellationToken cancellationToken)
    {
        var historyEntries = (await _mediator.Send(new GetAllWorkingTimeRecordHistoriesQuery(query.QueryModel), cancellationToken)).ToList();
        if (historyEntries.Any())
        {
            historyEntries.ForEach(x => x.IsEditable = true);
            return historyEntries;
        }
        
        var monthStart = new DateTime(query.QueryModel.Year, query.QueryModel.Month, 1);
        var monthEnd = monthStart.AddMonths(1);
        var nextMonthFirstDay = monthEnd;
        var includeEnd = nextMonthFirstDay.AddDays(1); // tylko 1. dzień kolejnego miesiąca

        var databaseQuery = _dbContext.Employees
            .Include(x => x.Department)
            .Include(x => x.WorkingTimeRecords
                .Where(w => w.OccuredAt >= monthStart && w.OccuredAt < includeEnd)) // miesiąc + 1 dzień
            .Where(x => x.IsActive)
            .Where(x => x.WorkingTimeRecords.Any(w => w.OccuredAt >= monthStart && w.OccuredAt < includeEnd));

        if (!string.IsNullOrWhiteSpace(query.QueryModel.SearchText))
        {
            databaseQuery = databaseQuery.Where(e =>
                e.Name.FirstName.Contains(query.QueryModel.SearchText) ||
                e.Name.LastName.Contains(query.QueryModel.SearchText));
        }

        if (query.QueryModel.DepartmentId is > 0)
        {
            databaseQuery = databaseQuery.Where(x => x.DepartmentId == query.QueryModel.DepartmentId);
        }

        if (query.QueryModel.ShiftId is > 0)
        {
            databaseQuery = databaseQuery.Where(x => x.JobInformation.ShiftType == (ShiftType)query.QueryModel.ShiftId!);
        }

        var employeesWithWorkingTimeRecords = await databaseQuery
            .OrderBy(x => x.Name.LastName)
            .ToListAsync(cancellationToken);


        // We delete all records when there is no records for querying period of time
        foreach (var employee in employeesWithWorkingTimeRecords.Where(employee => employee.WorkingTimeRecords.All(x => x.OccuredAt.Month != query.QueryModel.Month)))
        {
            employee.ClearAllWorkingTimeRecords();
        }

        var result = employeesWithWorkingTimeRecords.Select(employee => new EmployeeWorkingTimeRecordViewModel
        {
            Employee = _mapper.Map<EmployeeViewModel>(employee), 
            WorkingTimeRecordsAggregated = _employeeWorkingTimeRecordCalculationDomainService.CalculateAggregatedWorkingTimeRecords(employee.WorkingTimeRecords)
        }).ToList();
        
        // Clear all records without any entry
        result = result.Where(x => x.WorkingTimeRecordsAggregated.Any()).ToList();
        
        foreach (var x in result)
        {
            var salary = await _salaryCalculationDomainService.GetRecalculatedCurrentMonthSalary(
                x.Employee.BaseSalary,
                x.Employee.SalaryBonusPercentage, 
                0, 0, 0, 0,
                x.WorkingTimeRecordsAggregated
                    .Where(record => record.Date.Month == query.QueryModel.Month)
                    .ToList());

            x.SalaryInformation = _mapper.Map<EmployeeSalaryViewModel>(salary);
        }
        
        result.ForEach(entry =>
        {
            for (var day = 1; day <= DateTime.DaysInMonth(query.QueryModel.Year, query.QueryModel.Month); day++)
            {
                if (entry.WorkingTimeRecordsAggregated.Any(x => x.Date.Day == day && x.Date.Month == query.QueryModel.Month))
                    continue;
                
                // Entry for this day does not exist so we need to add empty one
                entry.WorkingTimeRecordsAggregated.Add(
                    WorkingTimeRecordAggregatedViewModel.EmptyForDay(
                        new DateTime(query.QueryModel.Year, query.QueryModel.Month, day)));
            }

            entry.WorkingTimeRecordsAggregated.RemoveAll(x => x.Date.Month != query.QueryModel.Month); // Remove all records from different months (the first day of the next month)
            entry.WorkingTimeRecordsAggregated.Sort((x, y) => x.Date.CompareTo(y.Date));
        });

        return result;
    }
}