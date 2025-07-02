using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsDetailsData;

public class EditWorkingTimeRecordsDetailsDataCommandHandler : ICommandHandler<EditWorkingTimeRecordsDetailsDataCommand>
{
    private readonly IAggregateRepository<Employee> _employeeRepository;
    private readonly IEmployeeWorkingTimeRecordCalculationDomainService _calculationDomainService;
    private readonly ISalaryCalculationDomainService _salaryCalculationDomainService;

    public EditWorkingTimeRecordsDetailsDataCommandHandler(
        IAggregateRepository<Employee> employeeRepository,
        IEmployeeWorkingTimeRecordCalculationDomainService calculationDomainService,
        ISalaryCalculationDomainService salaryCalculationDomainService)
    {
        _employeeRepository = employeeRepository;
        _calculationDomainService = calculationDomainService;
        _salaryCalculationDomainService = salaryCalculationDomainService;
    }
    
    public async Task Handle(EditWorkingTimeRecordsDetailsDataCommand command, CancellationToken cancellationToken)
    {
        var spec = new EmployeeWithHistoryRecordsByDateSpec(command.EmployeeId, command.Year, command.Month);
        var employee = await _employeeRepository.SingleOrDefaultAsync(spec, cancellationToken)
                       ?? throw new NotFoundException($"Employee with ID = {command.EmployeeId} was not found");

        await employee.UpdateDetailsData(command.Year, command.Month, command.DayHours, command.SaturdayHours, command.NightHours, _calculationDomainService, _salaryCalculationDomainService);

        await _employeeRepository.SaveChangesAsync(cancellationToken);
    }
};