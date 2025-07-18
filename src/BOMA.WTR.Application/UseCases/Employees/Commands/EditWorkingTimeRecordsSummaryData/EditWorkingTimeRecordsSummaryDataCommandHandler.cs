﻿using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsSummaryData;

public class EditWorkingTimeRecordsSummaryDataCommandHandler : ICommandHandler<EditWorkingTimeRecordsSummaryDataCommand>
{
    private readonly IAggregateRepository<Employee> _employeeRepository;
    private readonly ISalaryCalculationDomainService _salaryCalculationDomainService;

    public EditWorkingTimeRecordsSummaryDataCommandHandler(
        IAggregateRepository<Employee> employeeRepository,
        ISalaryCalculationDomainService salaryCalculationDomainService)
    {
        _employeeRepository = employeeRepository;
        _salaryCalculationDomainService = salaryCalculationDomainService;
    }

    public async Task Handle(EditWorkingTimeRecordsSummaryDataCommand command, CancellationToken cancellationToken)
    {
        var spec = new EmployeeWithHistoryRecordsByDateSpec(command.EmployeeId, command.Year, command.Month);
        var employee = await _employeeRepository.SingleOrDefaultAsync(spec, cancellationToken)
                       ?? throw new NotFoundException($"Employee with ID = {command.EmployeeId} was not found");

        var holidaySalary = new Money(command.HolidaySalary);
        var sicknessSalary = new Money(command.SicknessSalary);
        var additionalSalary = new Money(command.AdditionalSalary);
        var minSalaryCompensationAmount = new Money(command.MinSalaryCompensationAmount);
        
        await employee.UpdateSummaryData(
            command.Year, command.Month,
            command.BaseSalary, command.PercentageBonusSalary,
            holidaySalary, sicknessSalary, additionalSalary, minSalaryCompensationAmount,
            _salaryCalculationDomainService);

        await _employeeRepository.SaveChangesAsync(cancellationToken);
    }
}