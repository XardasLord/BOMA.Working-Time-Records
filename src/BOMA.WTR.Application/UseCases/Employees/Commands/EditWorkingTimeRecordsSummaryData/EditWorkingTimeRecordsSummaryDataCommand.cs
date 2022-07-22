using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsSummaryData;

public record EditWorkingTimeRecordsSummaryDataCommand(int EmployeeId, decimal HolidaySalary, int Year, int Month) : ICommand;