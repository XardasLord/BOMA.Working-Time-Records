using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using MediatR;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Edit;

public sealed class EditEmployeeCommandHandler : ICommandHandler<EditEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;

    public EditEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<Unit> Handle(EditEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetAsync(command.Id) 
            ?? throw new NotFoundException($"Employee with ID = {command.Id} was not found");

        employee.UpdateData(new Name(command.FirstName, command.LastName), command.RcpId);

        await _employeeRepository.SaveChangesAsync();
        
        return Unit.Value;
    }
}