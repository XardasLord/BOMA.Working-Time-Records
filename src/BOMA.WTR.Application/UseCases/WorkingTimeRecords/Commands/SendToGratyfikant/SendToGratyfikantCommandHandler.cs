using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.InsERT.Gratyfikant;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using MediatR;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands.SendToGratyfikant;

public class SendToGratyfikantCommandHandler(
    IMediator mediator,
    IGratyfikantService gratyfikantService) : ICommandHandler<SendToGratyfikantCommand, List<string>>
{
    public async Task<List<string>> Handle(SendToGratyfikantCommand command, CancellationToken cancellationToken)
    {
        var departmentType = (DepartmentType)command.QueryModel.DepartmentId!;
        if (departmentType is DepartmentType.Zlecenia or DepartmentType.Agencja or DepartmentType.Wszyscy)
        {
            throw new InvalidOperationException($"Nie można eksportować godzin do Gratyfikanta dla działu '{departmentType.ToString()}'");
        }
        
        var records = await mediator.Send(new GetAllWorkingTimeRecordsQuery(command.QueryModel), cancellationToken);
        
        var start = command.DateRangeQueryModel.StartDate;
        var endExclusive = command.DateRangeQueryModel.EndDate.Date.AddDays(1);
        
        records = records
            .Where(r => r.WorkingTimeRecordsAggregated
                .Where(wtr => wtr.DayWorkTimePeriodNormalized is not null || wtr.NightWorkTimePeriodNormalized is not null && wtr.WorkedHoursRounded > 0)
                .Any(wtr => wtr.Date >= start && wtr.Date < endExclusive))
            .Select(r => 
            {
                r.WorkingTimeRecordsAggregated = r.WorkingTimeRecordsAggregated
                    .Where(wtr => wtr.Date >= start && wtr.Date < endExclusive)
                    .Where(wtr => wtr.DayWorkTimePeriodNormalized is not null || wtr.NightWorkTimePeriodNormalized is not null && wtr.WorkedHoursRounded > 0)
                    .ToList();
                return r;
            })
            .ToList();
        
        var response = await gratyfikantService.SendHours(records, departmentType);
        
        return response;
    }
}