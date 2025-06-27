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
        var monthStart = new DateTime(query.QueryModel.Year, query.QueryModel.Month, 1);
        var monthEnd = monthStart.AddMonths(1);

        var databaseQuery = _dbContext.Employees
            .Include(x => x.Department)
            .Include(x => x.WorkingTimeRecordAggregatedHistories
                .Where(w => w.Date >= monthStart && w.Date < monthEnd))
            .Where(x => x.WorkingTimeRecordAggregatedHistories.Any(w =>
                w.Date >= monthStart && w.Date < monthEnd &&
                w.IsActive &&
                (query.QueryModel.DepartmentId == null || query.QueryModel.DepartmentId <= 0 || w.DepartmentId == query.QueryModel.DepartmentId) &&
                (query.QueryModel.ShiftId == null || query.QueryModel.ShiftId <= 0 || w.ShiftType == (ShiftType)query.QueryModel.ShiftId!)
            ));

        if (!string.IsNullOrWhiteSpace(query.QueryModel.SearchText))
        {
            databaseQuery = databaseQuery.Where(e =>
                e.Name.FirstName.Contains(query.QueryModel.SearchText) ||
                e.Name.LastName.Contains(query.QueryModel.SearchText));
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

        // Order by date
        result.ForEach(entry =>
        {
            entry.WorkingTimeRecordsAggregated = entry.WorkingTimeRecordsAggregated.OrderBy(x => x.Date).ToList();
        });
        
        return result;
    }
}