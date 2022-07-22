using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;
using MediatR;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsSummaryData;

public class EditWorkingTimeRecordsSummaryDataCommandHandler : ICommandHandler<EditWorkingTimeRecordsSummaryDataCommand>
{
    private readonly IAggregateRepository<Employee> _employeeRepository;

    public EditWorkingTimeRecordsSummaryDataCommandHandler(IAggregateRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Unit> Handle(EditWorkingTimeRecordsSummaryDataCommand command, CancellationToken cancellationToken)
    {
        var spec = new EmployeeWithHistoryRecordsByDateSpec(command.EmployeeId, command.Year, command.Month);
        var employee = await _employeeRepository.SingleOrDefaultAsync(spec, cancellationToken)
                       ?? throw new NotFoundException($"Employee with ID = {command.EmployeeId} was not found");

        var holidaySalary = new Money(command.HolidaySalary);
        
        employee.UpdateSummaryData(command.Year, command.Month, holidaySalary);

        await _employeeRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}