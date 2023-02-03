using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Deactivate;

public record DeactivateEmployeeCommand(int Id) : ICommand;