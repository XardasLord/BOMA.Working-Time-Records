using AutoMapper;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
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

    public GetAllWorkingTimeRecordsQueryHandler(
        BomaDbContext dbContext,
        IMapper mapper,
        IMediator mediator,
        IEmployeeWorkingTimeRecordCalculationDomainService employeeWorkingTimeRecordCalculationDomainService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _mediator = mediator;
        _employeeWorkingTimeRecordCalculationDomainService = employeeWorkingTimeRecordCalculationDomainService;
    }
    
    public async Task<IEnumerable<EmployeeWorkingTimeRecordViewModel>> Handle(GetAllWorkingTimeRecordsQuery query, CancellationToken cancellationToken)
    {
        var historyEntries = (await _mediator.Send(new GetAllWorkingTimeRecordHistoriesQuery(query.QueryModel), cancellationToken)).ToList();
        if (historyEntries.Any())
        {
            historyEntries.ForEach(x => x.IsEditable = true);
            return historyEntries;
        }
        
        IQueryable<Employee> databaseQuery;

        if (query.QueryModel.DepartmentId is not null)
        {
            databaseQuery = _dbContext.Employees
                .Include(x => x.Department)
                .Include(x => x.WorkingTimeRecords
                    .Where(w => w.OccuredAt.Year == query.QueryModel.Year)
                    .Where(w => w.OccuredAt.Month == query.QueryModel.Month))
                .Where(x => x.WorkingTimeRecords
                    .Where(w => w.OccuredAt.Year == query.QueryModel.Year)
                    .Any(w => w.OccuredAt.Month == query.QueryModel.Month))
                .Where(x => x.DepartmentId == query.QueryModel.DepartmentId);
        }
        else
        {
            databaseQuery = _dbContext.Employees
                .Include(x => x.Department)
                .Include(x => x.WorkingTimeRecords
                    .Where(w => w.OccuredAt.Year == query.QueryModel.Year)
                    .Where(w => w.OccuredAt.Month == query.QueryModel.Month))
                .Where(x => x.WorkingTimeRecords
                    .Where(w => w.OccuredAt.Year == query.QueryModel.Year)
                    .Any(w => w.OccuredAt.Month == query.QueryModel.Month));
        }

        if (!string.IsNullOrWhiteSpace(query.QueryModel.SearchText))
        {
            databaseQuery = databaseQuery.Where(e => e.Name.FirstName.Contains(query.QueryModel.SearchText) || e.Name.LastName.Contains(query.QueryModel.SearchText));
        }

        var employeesWithWorkingTimeRecords = await databaseQuery.ToListAsync(cancellationToken);

        var result = employeesWithWorkingTimeRecords.Select(employee => new EmployeeWorkingTimeRecordViewModel
        {
            Employee = _mapper.Map<EmployeeViewModel>(employee), 
            WorkingTimeRecordsAggregated = _employeeWorkingTimeRecordCalculationDomainService.CalculateAggregatedWorkingTimeRecords(employee.WorkingTimeRecords)
        }).ToList();

        result.ForEach(x =>
        {
            x.SalaryInformation = _mapper.Map<EmployeeSalaryViewModel>(x);
        });
        
        result.ForEach(entry =>
        {
            for (var day = 1; day <= DateTime.DaysInMonth(query.QueryModel.Year, query.QueryModel.Month); day++)
            {
                if (entry.WorkingTimeRecordsAggregated.Any(x => x.Date.Day == day))
                    continue;
                
                // Entry for this day does not exist so we need to add empty one
                entry.WorkingTimeRecordsAggregated.Add(
                    WorkingTimeRecordAggregatedViewModel.EmptyForDay(
                        new DateTime(query.QueryModel.Year, query.QueryModel.Month, day)));
            }

            entry.WorkingTimeRecordsAggregated.Sort((x, y) => x.Date.CompareTo(y.Date));
        });
        
        return result;
    }
}