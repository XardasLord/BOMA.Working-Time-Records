using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Domain.SharedKernel;
using MediatR;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsReportHoursData;

public class EditWorkingTimeRecordsReportHoursDataCommandHandler : ICommandHandler<EditWorkingTimeRecordsReportHoursDataCommand>
{
    private readonly IAggregateRepository<Employee> _employeeRepository;

    public EditWorkingTimeRecordsReportHoursDataCommandHandler(IAggregateRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    public async Task<Unit> Handle(EditWorkingTimeRecordsReportHoursDataCommand command, CancellationToken cancellationToken)
    {
        var spec = new EmployeeWithCurrentAndHistoryRecordsByDateSpec(command.EmployeeId, command.Year, command.Month);
        var employee = await _employeeRepository.SingleOrDefaultAsync(spec, cancellationToken)
                       ?? throw new NotFoundException($"Employee with ID = {command.EmployeeId} was not found");

        employee.UpdateWorkingHours(command.Year, command.Month, command.ReportEntryHours, command.ReportExitHours);

        await _employeeRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
};