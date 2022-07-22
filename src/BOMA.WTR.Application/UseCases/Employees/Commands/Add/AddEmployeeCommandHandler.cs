using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Add;

public sealed class AddEmployeeCommandHandler : ICommandHandler<AddEmployeeCommand, AddEmployeeResponse>
{
    private readonly IAggregateRepository<Employee> _employeeRepository;

    public AddEmployeeCommandHandler(IAggregateRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<AddEmployeeResponse> Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        var name = new Name(command.FirstName, command.LastName);
        var salary = new Money(command.BaseSalary);
        var bonus = new PercentageBonus(command.PercentageSalaryBonus);
        var employee = Employee.Add(name, salary, bonus, command.RcpId, command.DepartmentId);

        await _employeeRepository.AddAsync(employee, cancellationToken);
        await _employeeRepository.SaveChangesAsync(cancellationToken);

        return new AddEmployeeResponse(employee.Id);
    }
}