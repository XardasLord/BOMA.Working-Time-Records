using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsReportHoursData;

public record EditWorkingTimeRecordsReportHoursDataCommand(
    int EmployeeId,
    int Year, 
    int Month,
    IDictionary<int, string> ReportEntryHours,
    IDictionary<int, string> ReportExitHours) : ICommand;