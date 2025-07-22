using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Add;

public sealed class AddEmployeeCommandHandler(IAggregateRepository<Employee> employeeRepository)
    : ICommandHandler<AddEmployeeCommand, AddEmployeeResponse>
{
    public async Task<AddEmployeeResponse> Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        var name = new Name(command.FirstName, command.LastName);
        var salary = new Money(command.BaseSalary);
        var bonus = new PercentageBonus(command.PercentageSalaryBonus);
        var jobInformation = new JobInformation(new Position(command.Position), (ShiftType)command.ShiftTypeId);
        var personalIdentityNumber = new PersonalIdentityNumber(command.PersonalIdentityNumber);
        
        var employee = Employee.Add(name, salary, bonus, jobInformation, personalIdentityNumber, command.RcpId, command.DepartmentId);

        await employeeRepository.AddAsync(employee, cancellationToken);
        await employeeRepository.SaveChangesAsync(cancellationToken);

        return new AddEmployeeResponse(employee.Id);
    }
}