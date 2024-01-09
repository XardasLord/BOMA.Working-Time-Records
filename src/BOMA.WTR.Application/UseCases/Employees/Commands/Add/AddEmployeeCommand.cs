using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Add;

public sealed record AddEmployeeCommand(
    string FirstName,
    string LastName,
    decimal BaseSalary,
    double PercentageSalaryBonus,
    int RcpId,
    int DepartmentId,
    int ShiftTypeId,
    string Position) : ICommand<AddEmployeeResponse>;