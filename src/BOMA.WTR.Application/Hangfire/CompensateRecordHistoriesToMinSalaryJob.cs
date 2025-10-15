using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Application.Salary;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Setting;
using BOMA.WTR.Domain.AggregateModels.Setting.Specifications;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Domain.Extensions;
using BOMA.WTR.Domain.SharedKernel;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace BOMA.WTR.Application.Hangfire;

public class CompensateRecordHistoriesToMinSalaryJob(
    IAggregateRepository<Employee> employeeRepository,
    IAggregateReadRepository<Setting> settingsRepository)
{
    public async Task Execute(PerformContext? context, CancellationToken cancellationToken)
    {
        // We could base here on the last year & month history in DB
        var previousMonthDate = DateTime.Now.AddMonths(-1).Date;
        
        context.SetTextColor(ConsoleTextColor.DarkYellow);
        context.WriteLine($"Month: {previousMonthDate:MM.yyyy}");
        context.ResetTextColor();
        
        var workingHoursInMonth = (decimal)previousMonthDate.WorkingHoursInMonthExcludingBankHolidays();
        if (workingHoursInMonth >= 168)
        {
            context.SetTextColor(ConsoleTextColor.DarkYellow);
            context.WriteLine($"Number of working hours in {previousMonthDate:MM.yyyy} equals {workingHoursInMonth} and does not require salary compensation factor to be included.");
            return;
        }
        
        context.SetTextColor(ConsoleTextColor.Green);
        context.WriteLine($"Number of working hours in {previousMonthDate:MM.yyyy} equals {workingHoursInMonth} so it requires a salary compensation factor to be included.");
        context.ResetTextColor();
        
        // Otherwise we should compensate to the minimum salary based on the employee with the lowest salary
        var lowestSalaryEmployeeSpec = new ActiveEmployeeWithTheLowestSalarySpec(previousMonthDate.Year, previousMonthDate.Month);
        var employeeWithLowestSalary = await employeeRepository.FirstOrDefaultAsync(lowestSalaryEmployeeSpec, cancellationToken) 
                                       ?? throw new NotFoundException("No employee found for the given date with lowest salary");
        
        // Get min wage (salary)
        var minimumWageSetting = await settingsRepository.SingleOrDefaultAsync(new SettingByKeySpec("MinimumWage"), cancellationToken);
        var minWage = int.Parse(minimumWageSetting!.Value);
        
        // Get the employee with the lowest salary
        var lowestSalary = employeeWithLowestSalary.Salary.Amount;
        var monthlySalary = lowestSalary * workingHoursInMonth;
        var differenceToMinSalary = minWage - monthlySalary;
        
        
        context.SetTextColor(ConsoleTextColor.Green);
        context.WriteLine($"Lowest salary: {lowestSalary}");
        context.WriteLine($"Monthly lowest salary: {monthlySalary}");
        context.WriteLine($"Difference to min salary: {differenceToMinSalary}");

        if (differenceToMinSalary <= 0)
        {
            context.SetTextColor(ConsoleTextColor.DarkBlue);
            context.WriteLine("Difference to min salary is not greater than zero and does not require salary compensation factor to be included.");
            return;
        }
        
        // Calculate the percentage bonus
        var minSalaryCompensationFactor = Math.Round(differenceToMinSalary / monthlySalary, 4, MidpointRounding.ToPositiveInfinity); // e.g. 0.0416
        
        context.SetTextColor(ConsoleTextColor.Green);
        context.WriteLine($"Salary compensation factor: {minSalaryCompensationFactor}");

        var spec = new ActiveEmployeesWithHistoryRecordsByDateSpec(previousMonthDate.Year, previousMonthDate.Month);
        var employees = await employeeRepository.ListAsync(spec, cancellationToken)
                        ?? throw new NotFoundException("No employees found for the given date");

        employees.ForEach(employee =>
        {
            foreach (var historyEntry in employee.WorkingTimeRecordAggregatedHistories)
            {
                // Remove previous compensation if exists
                historyEntry.SalaryInformation.FinalSumSalary -= historyEntry.SalaryInformation.MinSalaryCompensationAmount;
                
                // Add new compensation
                historyEntry.SalaryInformation.MinSalaryCompensationFactor = minSalaryCompensationFactor;
                historyEntry.SalaryInformation.MinSalaryCompensationAmount = Math.Round(historyEntry.SalaryInformation.GrossBaseSalary * minSalaryCompensationFactor, 2);
                historyEntry.SalaryInformation.FinalSumSalary += historyEntry.SalaryInformation.MinSalaryCompensationAmount;
            }
        });
        
        await employeeRepository.SaveChangesAsync(cancellationToken);
    }
}