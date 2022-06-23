using Hangfire.Console;
using Hangfire.Server;

namespace BOMA.WRT.Application.Hangfire;

public class ParseWorkingTimeRecordsFileJob
{
    public Task Execute(PerformContext? context, CancellationToken cancellationToken)
    {
        // Look for a CSV file
        // If file exists try to parse it to the model class
        // 
        return Task.CompletedTask;
    }
}