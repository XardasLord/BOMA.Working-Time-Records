using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Application.Salary;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Domain.Extensions;
using BOMA.WTR.Domain.SharedKernel;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace BOMA.WTR.Application.Hangfire;

public class CompensateRecordHistoriesToMinSalaryJob(
    IAggregateRepository<Employee> employeeRepository,
    IOptions<SalaryConfiguration> salaryConfigurationOptions)
{
    public async Task Execute(PerformContext? context, CancellationToken cancellationToken)
    {
        // We could base here on the last year & month history in DB
        var previousMonthDate = DateTime.Now.AddMonths(-1).Date;
        
        var workingHoursInMonth = (decimal)previousMonthDate.WorkingHoursInMonthExcludingBankHolidays();
        if (workingHoursInMonth >= 168)
            return;
        
        // Otherwise we should compensate to the minimum salary based on the employee with the lowest salary
        var lowestSalaryEmployeeSpec = new ActiveEmployeeWithTheLowestSalarySpec(previousMonthDate.Year, previousMonthDate.Month);
        var employeeWithLowestSalary = await employeeRepository.FirstOrDefaultAsync(lowestSalaryEmployeeSpec, cancellationToken) 
                                       ?? throw new NotFoundException("No employee found for the given date with lowest salary");
        // Get the employee with the lowest salary
        var lowestSalary = employeeWithLowestSalary.Salary.Amount;
        var monthlySalary = lowestSalary * workingHoursInMonth;
        var differenceToMinSalary = salaryConfigurationOptions.Value.MinSalary - monthlySalary;
        
        if (differenceToMinSalary <= 0)
            return;
        
        // Calculate the percentage bonus
        var minSalaryCompensationFactor = Math.Round(differenceToMinSalary / monthlySalary, 4, MidpointRounding.ToPositiveInfinity); // e.g. 0.0416

        var spec = new ActiveEmployeesWithHistoryRecordsByDateSpec(previousMonthDate.Year, previousMonthDate.Month);
        var employees = await employeeRepository.ListAsync(spec, cancellationToken)
                        ?? throw new NotFoundException("No employees found for the given date");

        employees.ForEach(employee =>
        {
            foreach (var historyEntry in employee.WorkingTimeRecordAggregatedHistories)
            {
                historyEntry.SalaryInformation.MinSalaryCompensationFactor = minSalaryCompensationFactor;
                historyEntry.SalaryInformation.MinSalaryCompensationAmount = historyEntry.SalaryInformation.BaseSalary * workingHoursInMonth * minSalaryCompensationFactor;
                historyEntry.SalaryInformation.FinalSumSalary += historyEntry.SalaryInformation.MinSalaryCompensationAmount;
            }
        });
        
        await employeeRepository.SaveChangesAsync(cancellationToken);
    }
}