using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Add;

public sealed class AddEmployeeCommandHandler : ICommandHandler<AddEmployeeCommand, AddEmployeeResponse>
{
    private readonly IEmployeeRepository _employeeRepository;

    public AddEmployeeCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<AddEmployeeResponse> Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = Employee.Add(new Name(command.FirstName, command.LastName), command.RcpId);

        await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveChangesAsync();

        return new AddEmployeeResponse(employee.Id);
    }
}