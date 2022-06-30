using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Edit;

public sealed record EditEmployeeCommand(int Id, string FirstName, string LastName, int RcpId) : ICommand;