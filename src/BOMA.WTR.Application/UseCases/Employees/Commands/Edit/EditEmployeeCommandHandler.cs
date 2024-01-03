using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Edit;

public sealed class EditEmployeeCommandHandler : ICommandHandler<EditEmployeeCommand>
{
    private readonly IAggregateRepository<Employee> _employeeRepository;

    public EditEmployeeCommandHandler(IAggregateRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task Handle(EditEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(command.Id, cancellationToken) 
            ?? throw new NotFoundException($"Employee with ID = {command.Id} was not found");

        var name = new Name(command.FirstName, command.LastName);
        var jobInformation = new JobInformation(new Position(command.Position), (ShiftType)command.ShiftTypeId);
        var personalIdentityNumber = new PersonalIdentityNumber(command.PersonalIdentityNumber);
        var salaryBonusPercentage = new PercentageBonus(command.SalaryBonusPercentage);
        
        var salary = employee.Salary with
        {
            Amount = command.BaseSalary
        };
        
        employee.UpdateData(name, salary, salaryBonusPercentage, jobInformation, personalIdentityNumber, command.RcpId, command.DepartmentId);

        await _employeeRepository.SaveChangesAsync(cancellationToken);
    }
}