﻿using AutoMapper;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.QueryHandlers;

public class GetAllWorkingTimeRecordHistoriesQueryHandler : IQueryHandler<GetAllWorkingTimeRecordHistoriesQuery, IEnumerable<EmployeeWorkingTimeRecordViewModel>>
{
    private readonly BomaDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllWorkingTimeRecordHistoriesQueryHandler(BomaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<EmployeeWorkingTimeRecordViewModel>> Handle(GetAllWorkingTimeRecordHistoriesQuery query, CancellationToken cancellationToken)
    {
        var databaseQuery = _dbContext.Employees
            .Include(x => x.Department)
            .Include(x => x.WorkingTimeRecordAggregatedHistories
                .Where(w => w.Date.Year == query.QueryModel.Year && w.Date.Month == query.QueryModel.Month)) // We need to include also the first day of new month for last day of month calculations
            .Where(x => x.WorkingTimeRecordAggregatedHistories
                .Any(w => w.Date.Year == query.QueryModel.Year && w.Date.Month == query.QueryModel.Month))
            .Where(x => x.WorkingTimeRecordAggregatedHistories
                .Any(w => w.IsActive));
        
        if (!string.IsNullOrWhiteSpace(query.QueryModel.SearchText))
        {
            databaseQuery = databaseQuery.Where(e => e.Name.FirstName.Contains(query.QueryModel.SearchText) || e.Name.LastName.Contains(query.QueryModel.SearchText));
        }

        if (query.QueryModel.DepartmentId is > 0)
        {
            databaseQuery = databaseQuery.Where(x => x.WorkingTimeRecordAggregatedHistories.Any(w => w.DepartmentId == query.QueryModel.DepartmentId));
        }

        if (query.QueryModel.ShiftId is > 0)
        {
            databaseQuery = databaseQuery.Where(x => x.WorkingTimeRecordAggregatedHistories.Any(w => w.ShiftType == (ShiftType)query.QueryModel.DepartmentId!));
        }

        var employeesWithWorkingTimeRecords = await databaseQuery
            .OrderBy(x => x.Name.LastName)
            .ToListAsync(cancellationToken);

        if (!employeesWithWorkingTimeRecords.Any())
        {
            // No historical data
            return new List<EmployeeWorkingTimeRecordViewModel>();
        }

        // We delete all records when there is no records for querying period of time
        foreach (var employee in employeesWithWorkingTimeRecords.Where(employee => employee.WorkingTimeRecordAggregatedHistories.All(x => x.Date.Month != query.QueryModel.Month)))
        {
            employee.ClearAllWorkingTimeRecords();
        }

        var result = employeesWithWorkingTimeRecords.Select(employee =>
        {
            var workingTimeRecord = employee.WorkingTimeRecordAggregatedHistories.FirstOrDefault();

            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);
            
            if (workingTimeRecord != null)
            {
                // Nadpisujemy danymi historycznymi za ten miesiąc
                employeeViewModel.IsActive = workingTimeRecord.IsActive;
                employeeViewModel.DepartmentId = workingTimeRecord.DepartmentId;
                employeeViewModel.ShiftTypeId = (int?)workingTimeRecord.ShiftType;
            }

            return new EmployeeWorkingTimeRecordViewModel
            {
                Employee = employeeViewModel,
                WorkingTimeRecordsAggregated = _mapper.Map<List<WorkingTimeRecordAggregatedViewModel>>(employee.WorkingTimeRecordAggregatedHistories),
                SalaryInformation = _mapper.Map<EmployeeSalaryViewModel>(workingTimeRecord?.SalaryInformation)
            };
        }).ToList();

        // var result = employeesWithWorkingTimeRecords.Select(employee => new EmployeeWorkingTimeRecordViewModel
        // {
        //     Employee = _mapper.Map<EmployeeViewModel>(employee), // TODO: BOMA-13 tutaj dla tego obiektu trzeba zmienić wartości IsActive, DepartmentId oraz ShiftId. A te wartości możemy wziąć z employee.WorkingTimeRecordAggregatedHistories.First().XYZ
        //     WorkingTimeRecordsAggregated = _mapper.Map<List<WorkingTimeRecordAggregatedViewModel>>(employee.WorkingTimeRecordAggregatedHistories),
        //     SalaryInformation = _mapper.Map<EmployeeSalaryViewModel>(employee.WorkingTimeRecordAggregatedHistories.First().SalaryInformation) // TODO: BOMA-13: Tak jest zwracane wynagrodzenie historyczne
        // }).ToList();

        // Order by date
        result.ForEach(entry =>
        {
            entry.WorkingTimeRecordsAggregated = entry.WorkingTimeRecordsAggregated.OrderBy(x => x.Date).ToList();
        });
        
        return result;
    }
}