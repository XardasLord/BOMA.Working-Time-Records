using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Hangfire;
using Hangfire;

namespace BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands.CloseMonth;

public class CloseMonthCommandHandler(IBackgroundJobClient backgroundJobClient)
    : ICommandHandler<CloseMonthCommand>
{
    public Task Handle(CloseMonthCommand command, CancellationToken cancellationToken)
    {
        backgroundJobClient.Enqueue<AggregateWorkingTimeRecordHistoriesJob>(job => job.Execute(null, cancellationToken));
        
        return Task.CompletedTask;
    }
}