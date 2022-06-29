using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Add;

public sealed record AddEmployeeCommand(string FirstName, string LastName, int RcpId) : ICommand<AddEmployeeResponse>;