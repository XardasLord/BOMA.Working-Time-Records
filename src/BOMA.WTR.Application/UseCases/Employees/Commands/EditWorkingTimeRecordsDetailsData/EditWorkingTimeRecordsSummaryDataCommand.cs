using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsDetailsData;

public record EditWorkingTimeRecordsDetailsDataCommand(
    int EmployeeId,
    int Year, 
    int Month,
    IDictionary<int, double?> DayHours,
    IDictionary<int, double?> SaturdayHours,
    IDictionary<int, double?> NightHours) : ICommand;