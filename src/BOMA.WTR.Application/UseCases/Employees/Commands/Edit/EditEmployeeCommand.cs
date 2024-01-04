using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Edit;

public sealed record EditEmployeeCommand(
    int Id,
    string FirstName,
    string LastName,
    decimal BaseSalary,
    double SalaryBonusPercentage,
    int RcpId,
    int DepartmentId,
    int ShiftTypeId,
    string Position) : ICommand;