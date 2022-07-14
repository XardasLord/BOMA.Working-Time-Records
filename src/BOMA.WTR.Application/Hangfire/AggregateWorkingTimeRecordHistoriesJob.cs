using AutoMapper;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using Hangfire.Server;
using MediatR;

namespace BOMA.WTR.Application.Hangfire;

public class AggregateWorkingTimeRecordHistoriesJob
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;

    public AggregateWorkingTimeRecordHistoriesJob(IMediator mediator, IMapper mapper, IEmployeeRepository employeeRepository)
    {
        _mediator = mediator;
        _mapper = mapper;
        _employeeRepository = employeeRepository;
    }
    
    public async Task Execute(PerformContext? context, CancellationToken cancellationToken)
    {
        var previousMonthDate = DateTime.Now.AddMonths(-1).Date;

        var queryRecordModel = new GetRecordsQueryModel(previousMonthDate.Month, previousMonthDate.Year);
        var workingTimeRecordViewModels = await _mediator.Send(new GetAllWorkingTimeRecordsQuery(queryRecordModel), cancellationToken);
        
        // TODO: Need to recalculate worked hours for the month based on business rules 'adding to normative 8 hours'

        var employeesCache = new List<Employee>();
        
        foreach (var model in workingTimeRecordViewModels)
        {
            var currentEmployee = employeesCache.SingleOrDefault(x => x.RcpId == model.Employee.RcpId);
            
            if (currentEmployee is null)
            {
                currentEmployee = await _employeeRepository.GetByRcpIdAsync(model.Employee.RcpId);
                    
                employeesCache.Add(currentEmployee);
            }
            
            foreach (var wtr in model.WorkingTimeRecordsAggregated)
            {
                currentEmployee.AddWorkingTimeRecordAggregatedHistory(new WorkingTimeRecordAggregatedHistory
                {
                    Date = wtr.Date,
                    WorkedHoursRounded = wtr.WorkedHoursRounded,
                    WorkedMinutes = wtr.WorkedMinutes,
                    BaseNormativeHours = wtr.BaseNormativeHours,
                    FiftyPercentageBonusHours = wtr.FiftyPercentageBonusHours,
                    HundredPercentageBonusHours = wtr.HundredPercentageBonusHours,
                    SaturdayHours = wtr.SaturdayHours,
                    NightHours = wtr.NightHours,
                    SalaryInformation = _mapper.Map<EmployeeSalaryAggregatedHistory>(model.SalaryInformation)
                });
            }
        }
        
        await _employeeRepository.SaveChangesAsync();
    }
}