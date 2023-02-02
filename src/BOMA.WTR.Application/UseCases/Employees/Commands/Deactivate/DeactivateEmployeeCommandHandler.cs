using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.SharedKernel;
using MediatR;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Deactivate;

public class DeactivateEmployeeCommandHandler : ICommandHandler<DeactivateEmployeeCommand>
{
    private readonly IAggregateRepository<Employee> _employeeRepository;

    public DeactivateEmployeeCommandHandler(IAggregateRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<Unit> Handle(DeactivateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(command.Id, cancellationToken) 
                       ?? throw new NotFoundException($"Employee with ID = {command.Id} was not found");
        
        employee.Deactivate();

        await _employeeRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}