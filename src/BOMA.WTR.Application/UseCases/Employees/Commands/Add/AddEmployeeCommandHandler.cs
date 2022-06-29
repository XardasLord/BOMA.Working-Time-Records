using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Add;

public sealed class AddEmployeeCommandHandler : ICommandHandler<AddEmployeeCommand, AddEmployeeResponse>
{
    public AddEmployeeCommandHandler()
    {
        
    }
    
    public Task<AddEmployeeResponse> Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}