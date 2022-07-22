using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using MediatR;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsSummaryData;

public class EditWorkingTimeRecordsSummaryDataCommandHandler : ICommandHandler<EditWorkingTimeRecordsSummaryDataCommand>
{
    private readonly IEmployeeRepository _employeeRepository;

    public EditWorkingTimeRecordsSummaryDataCommandHandler(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Unit> Handle(EditWorkingTimeRecordsSummaryDataCommand command, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetWithHistoryRecordsAsync(command.EmployeeId)
                       ?? throw new NotFoundException($"Employee with ID = {command.EmployeeId} was not found");

        var holidaySalary = new Money(command.HolidaySalary);
        
        employee.UpdateSummaryData(command.Year, command.Month, holidaySalary);

        await _employeeRepository.SaveChangesAsync();
        
        return Unit.Value;
    }
}