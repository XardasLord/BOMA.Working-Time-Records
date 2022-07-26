using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Domain.SharedKernel;
using MediatR;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsDetailsData;

public class EditWorkingTimeRecordsDetailsDataCommandHandler : ICommandHandler<EditWorkingTimeRecordsDetailsDataCommand>
{
    private readonly IAggregateRepository<Employee> _employeeRepository;
    private readonly IEmployeeWorkingTimeRecordCalculationDomainService _calculationDomainService;

    public EditWorkingTimeRecordsDetailsDataCommandHandler(IAggregateRepository<Employee> employeeRepository, IEmployeeWorkingTimeRecordCalculationDomainService calculationDomainService)
    {
        _employeeRepository = employeeRepository;
        _calculationDomainService = calculationDomainService;
    }
    
    public async Task<Unit> Handle(EditWorkingTimeRecordsDetailsDataCommand command, CancellationToken cancellationToken)
    {
        var spec = new EmployeeWithHistoryRecordsByDateSpec(command.EmployeeId, command.Year, command.Month);
        var employee = await _employeeRepository.SingleOrDefaultAsync(spec, cancellationToken)
                       ?? throw new NotFoundException($"Employee with ID = {command.EmployeeId} was not found");

        var dayHoursDictionary = new Dictionary<int, double?>
        {
            { 1, command.Day1 },
            { 2, command.Day2 },
            { 3, command.Day3 },
            { 4, command.Day4 },
            { 5, command.Day5 },
            { 6, command.Day6 },
            { 7, command.Day7 },
            { 8, command.Day8 },
            { 9, command.Day9 },
            { 10, command.Day10 },
            { 11, command.Day11 },
            { 12, command.Day12 },
            { 13, command.Day13 },
            { 14, command.Day14 },
            { 15, command.Day15 },
            { 16, command.Day16 },
            { 17, command.Day17 },
            { 18, command.Day18 },
            { 19, command.Day19 },
            { 20, command.Day20 },
            { 21, command.Day21 },
            { 22, command.Day22 },
            { 23, command.Day23 },
            { 24, command.Day24 },
            { 25, command.Day25 },
            { 26, command.Day26 },
            { 27, command.Day27 },
            { 28, command.Day28 },
            { 29, command.Day29 },
            { 30, command.Day30 },
            { 31, command.Day31 },
        };

        employee.UpdateDetailsData(command.Year, command.Month, dayHoursDictionary, _calculationDomainService);

        await _employeeRepository.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
};