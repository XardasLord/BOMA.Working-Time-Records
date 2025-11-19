using AutoMapper;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Application.UseCases.Reports.Models;
using BOMA.WTR.Application.UseCases.Reports.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.QueryHandlers;

public class GetDataQueryHandler(BomaDbContext dbContext, IMapper mapper)
    : IQueryHandler<GetDataQuery, ReportViewModel>
{
    public async Task<ReportViewModel> Handle(GetDataQuery query, CancellationToken cancellationToken)
    {
        var monthStart = query.QueryModel.StartDate;
        var monthEnd = query.QueryModel.EndDate.AddMonths(1);

        var databaseQuery = dbContext.Employees
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
        
        var employeesWithWorkingTimeRecords = await databaseQuery.ToListAsync(cancellationToken);
        
        if (!employeesWithWorkingTimeRecords.Any())
        {
            return new ReportViewModel();
        }

        // We delete all records when there is no records for querying period of time
        foreach (var employee in employeesWithWorkingTimeRecords.Where(employee => employee.WorkingTimeRecordAggregatedHistories.All(x => x.Date.Month != query.QueryModel.EndDate.Month)))
        {
            employee.ClearAllWorkingTimeRecords();
        }

        var result = employeesWithWorkingTimeRecords.Select(employee =>
        {
            var workingTimeRecord = employee.WorkingTimeRecordAggregatedHistories.FirstOrDefault();

            var employeeViewModel = mapper.Map<EmployeeViewModel>(employee);
            
            if (workingTimeRecord != null)
            {
                employeeViewModel.IsActive = workingTimeRecord.IsActive;
                employeeViewModel.DepartmentId = workingTimeRecord.DepartmentId;
                employeeViewModel.ShiftTypeId = (int?)workingTimeRecord.ShiftType;
            }

            return new EmployeeWorkingTimeRecordViewModel
            {
                Employee = employeeViewModel,
                WorkingTimeRecordsAggregated = mapper.Map<List<WorkingTimeRecordAggregatedViewModel>>(employee.WorkingTimeRecordAggregatedHistories),
                SalaryInformation = mapper.Map<EmployeeSalaryViewModel>(workingTimeRecord?.SalaryInformation)
            };
        }).ToList();
        
        var distinctWorkingTimeRecords = employeesWithWorkingTimeRecords
            .Select(x => x.WorkingTimeRecordAggregatedHistories
                .DistinctBy(w => w.Date.Month))
            .ToList();

        return new ReportViewModel
        {
            WorkingHoursReport = new WorkingHoursReportViewModel
            {
                EmployeesCount = result.Count,
                NormativeHours = result.Sum(x => x.WorkingTimeRecordsAggregated.Sum(w => w.BaseNormativeHours)),
                FiftyPercentageBonusHours = result.Sum(x => x.WorkingTimeRecordsAggregated.Sum(w => w.FiftyPercentageBonusHours)),
                HundredPercentageBonusHours = result.Sum(x => x.WorkingTimeRecordsAggregated.Sum(w => w.HundredPercentageBonusHours)),
                SaturdayHours = result.Sum(x => x.WorkingTimeRecordsAggregated.Sum(w => w.SaturdayHours))
            },
            SalaryReport = new SalaryReportViewModel
            {
                EmployeesCount = result.Count,
                GrossBaseSalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.GrossBaseSalary)),
                BonusSalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.BonusBaseSalary)) + 
                              distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.BonusBase50PercentageSalary)) +
                              distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.BonusBase100PercentageSalary)) +
                              distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.BonusBaseSaturdaySalary)),
                OvertimeSalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.GrossBase50PercentageSalary)) + 
                                 distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.GrossBase100PercentageSalary)),
                SaturdaySalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.GrossBaseSaturdaySalary)),
                NightSalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.NightBaseSalary)),
                HolidaySalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.HolidaySalary)),
                SicknessSalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.SicknessSalary)),
                AdditionalSalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.AdditionalSalary)),
                CompensationSalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.MinSalaryCompensationAmount)),
                FinalSalary = distinctWorkingTimeRecords.Sum(x => x.Sum(s => s.SalaryInformation.FinalSumSalary))
                
            }
        };
    }
}