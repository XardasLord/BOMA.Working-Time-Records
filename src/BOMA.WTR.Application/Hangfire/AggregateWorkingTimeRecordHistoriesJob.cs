using AutoMapper;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using MediatR;

namespace BOMA.WTR.Application.Hangfire;

public class AggregateWorkingTimeRecordHistoriesJob(
    IMediator mediator,
    IMapper mapper,
    IAggregateRepository<Employee> employeeRepository,
    IBackgroundJobClient backgroundJobClient)
{
    public async Task Execute(PerformContext? context, CancellationToken cancellationToken)
    {
        // We could base here on the last year & month history in DB
        var previousMonthDate = DateTime.Now.AddMonths(-1).Date;
        
        context.SetTextColor(ConsoleTextColor.DarkYellow);
        context.WriteLine($"Month: {previousMonthDate:MM.yyyy}");
        context.ResetTextColor();

        var queryRecordModel = new GetRecordsQueryModel(previousMonthDate.Month, previousMonthDate.Year);
        var workingTimeRecordViewModels = await mediator.Send(new GetAllWorkingTimeRecordsQuery(queryRecordModel), cancellationToken);

        var employeeWorkingTimeRecordViewModels = workingTimeRecordViewModels.ToList();
        
        if (employeeWorkingTimeRecordViewModels.FirstOrDefault()?.IsEditable == true)
        {
            context.SetTextColor(ConsoleTextColor.DarkYellow);
            context.WriteLine("Previous month has been already closed.");
            return;
        }
        
        // TODO: Need to recalculate worked hours for the month based on business rules 'adding to normative 8 hours'

        var employeesCache = new List<Employee>();
        
        foreach (var model in employeeWorkingTimeRecordViewModels)
        {
            var currentEmployee = employeesCache.SingleOrDefault(x => x.RcpId == model.Employee.RcpId);
            
            if (currentEmployee is null)
            {
                var spec = new EmployeeWithCurrentAndFullHistoricalEntriesByRcpIdSpec(model.Employee.RcpId);

                if (await employeeRepository.CountAsync(spec, cancellationToken) > 1)
                {
                    throw new InvalidOperationException($"Employee with RCP ID = {model.Employee.RcpId} is not unique. There are more than one entry with this RCP ID in database.");
                }
                
                currentEmployee = await employeeRepository.SingleOrDefaultAsync(spec, cancellationToken)
                    ?? throw new NotFoundException($"Employee with RCP ID = {model.Employee.RcpId} was not found");
                    
                employeesCache.Add(currentEmployee);
            }
            
            foreach (var wtr in model.WorkingTimeRecordsAggregated)
            {
                var historyEntry = new WorkingTimeRecordAggregatedHistory
                {
                    Date = wtr.Date,
                    StartNormalizedAt = wtr.WorkTimePeriodNormalized.From,
                    StartOriginalAt = wtr.WorkTimePeriodOriginal.From,
                    FinishNormalizedAt = wtr.WorkTimePeriodNormalized.To ?? wtr.WorkTimePeriodNormalized.From,
                    FinishOriginalAt = wtr.WorkTimePeriodOriginal.To ?? wtr.WorkTimePeriodOriginal.From,
                    WorkedHoursRounded = wtr.WorkedHoursRounded,
                    WorkedMinutes = wtr.WorkedMinutes,
                    BaseNormativeHours = wtr.BaseNormativeHours,
                    FiftyPercentageBonusHours = wtr.FiftyPercentageBonusHours,
                    HundredPercentageBonusHours = wtr.HundredPercentageBonusHours,
                    SaturdayHours = wtr.SaturdayHours,
                    NightHours = wtr.NightHours,
                    SalaryInformation = mapper.Map<EmployeeSalaryAggregatedHistory>(model.SalaryInformation),
                    IsActive = model.Employee.IsActive,
                    DepartmentId = model.Employee.DepartmentId,
                    ShiftType = (ShiftType?)model.Employee.ShiftTypeId
                };

                historyEntry.SalaryInformation.PercentageBonusSalary = model.Employee.SalaryBonusPercentage;
                
                currentEmployee.AddWorkingTimeRecordAggregatedHistory(historyEntry);
            }
        }
        
        await employeeRepository.SaveChangesAsync(cancellationToken);
        
        context.SetTextColor(ConsoleTextColor.DarkGreen);
        context.WriteLine("Previous month has been closed successfully.");

        backgroundJobClient.Enqueue<CompensateRecordHistoriesToMinSalaryJob>(job => job.Execute(null, cancellationToken));
    }
}