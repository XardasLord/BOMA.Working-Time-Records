using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;
using MediatR;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Edit;

public sealed class EditEmployeeCommandHandler : ICommandHandler<EditEmployeeCommand>
{
    private readonly IAggregateRepository<Employee> _employeeRepository;

    public EditEmployeeCommandHandler(IAggregateRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<Unit> Handle(EditEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(command.Id, cancellationToken) 
            ?? throw new NotFoundException($"Employee with ID = {command.Id} was not found");

        var name = new Name(command.FirstName, command.LastName);
        var salary = employee.Salary with
        {
            Amount = command.BaseSalary
        };
        var salaryBonusPercentage = new PercentageBonus(command.SalaryBonusPercentage);
        
        employee.UpdateData(name, salary, salaryBonusPercentage, command.RcpId, command.DepartmentId);

        await _employeeRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}