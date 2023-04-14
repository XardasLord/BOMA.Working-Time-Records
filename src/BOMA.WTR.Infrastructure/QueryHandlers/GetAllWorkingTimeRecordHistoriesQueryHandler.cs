using AutoMapper;
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
        var nextMonth = new DateTime(query.QueryModel.Year, query.QueryModel.Month, 1).AddMonths(1).Month;
        var nextMonthYear = new DateTime(query.QueryModel.Year, query.QueryModel.Month, 1).AddMonths(1).Year;

        var databaseQuery = _dbContext.Employees
            .Include(x => x.Department)
            .Include(x => x.WorkingTimeRecordAggregatedHistories
                .Where(w => w.Date.Year == query.QueryModel.Year && w.Date.Month == query.QueryModel.Month)) // We need to include also the first day of new month for last day of month calculations
            .Where(x => x.IsActive)
            .Where(x => x.WorkingTimeRecordAggregatedHistories
                .Any(w => w.Date.Year == query.QueryModel.Year && w.Date.Month == query.QueryModel.Month));
        
        if (!string.IsNullOrWhiteSpace(query.QueryModel.SearchText))
        {
            databaseQuery = databaseQuery.Where(e => e.Name.FirstName.Contains(query.QueryModel.SearchText) || e.Name.LastName.Contains(query.QueryModel.SearchText));
        }

        if (query.QueryModel.DepartmentId is > 0)
        {
            databaseQuery = databaseQuery.Where(x => x.DepartmentId == query.QueryModel.DepartmentId);
        }

        if (query.QueryModel.ShiftId is > 0)
        {
            databaseQuery = databaseQuery.Where(x => x.JobInformation.ShiftType == (ShiftType)query.QueryModel.ShiftId);
        }

        var employeesWithWorkingTimeRecords = await databaseQuery
            .OrderBy(x => x.Name.LastName)
            .ToListAsync(cancellationToken);

        // We delete all records when there is no records for querying period of time
        foreach (var employee in employeesWithWorkingTimeRecords.Where(employee => employee.WorkingTimeRecordAggregatedHistories.All(x => x.Date.Month != query.QueryModel.Month)))
        {
            employee.ClearAllWorkingTimeRecords();
        }

        var result = employeesWithWorkingTimeRecords.Select(employee => new EmployeeWorkingTimeRecordViewModel
        {
            Employee = _mapper.Map<EmployeeViewModel>(employee), 
            WorkingTimeRecordsAggregated = _mapper.Map<List<WorkingTimeRecordAggregatedViewModel>>(employee.WorkingTimeRecordAggregatedHistories)
        }).ToList();
        
        // Clear all records without any entry
        result = result.Where(x => x.WorkingTimeRecordsAggregated.Any()).ToList();

        result.ForEach(x =>
        {
            x.SalaryInformation = _mapper.Map<EmployeeSalaryViewModel>(x);
        });
        
        // Extend AutoMapper logic
        result.ForEach(entry =>
        {
            var baseEmployeeHistoryRecord = employeesWithWorkingTimeRecords
                .Single(employee => employee.Id == entry.Employee.Id)
                .WorkingTimeRecordAggregatedHistories
                .First();
            
            entry.SalaryInformation.HolidaySalary = baseEmployeeHistoryRecord.SalaryInformation.HolidaySalary;
            entry.SalaryInformation.SicknessSalary = baseEmployeeHistoryRecord.SalaryInformation.SicknessSalary;
            entry.SalaryInformation.AdditionalSalary = baseEmployeeHistoryRecord.SalaryInformation.AdditionalSalary;
            entry.SalaryInformation.FinalSumSalary += entry.SalaryInformation.HolidaySalary + entry.SalaryInformation.SicknessSalary + entry.SalaryInformation.AdditionalSalary;
            
            entry.WorkingTimeRecordsAggregated.RemoveAll(x => x.Date.Month != query.QueryModel.Month); // Remove all records from different months (the first day of the next month)
            entry.WorkingTimeRecordsAggregated.ForEach(x => x.IsWeekendDay = x.Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday);
            
            entry.WorkingTimeRecordsAggregated = entry.WorkingTimeRecordsAggregated.OrderBy(x => x.Date).ToList();
        });
        
        return result;
    }
}