using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsSummaryData;

public record EditWorkingTimeRecordsSummaryDataCommand(
    int EmployeeId,
    int Year, 
    int Month,
    decimal BaseSalary,
    decimal PercentageBonusSalary,
    decimal HolidaySalary,
    decimal SicknessSalary,
    decimal AdditionalSalary,
    decimal MinSalaryCompensationAmount) : ICommand;