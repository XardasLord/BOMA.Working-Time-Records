using BOMA.WRT.Application.RogerFiles;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace BOMA.WRT.Application.Hangfire;

public class ParseWorkingTimeRecordsFileJob
{
    private readonly RogerFileConfiguration _rogerFileOptions;

    public ParseWorkingTimeRecordsFileJob(IOptions<RogerFileConfiguration> options)
    {
        _rogerFileOptions = options.Value;
    }
    
    public async Task Execute(PerformContext? context, CancellationToken cancellationToken)
    {
        if (Directory.GetFiles(_rogerFileOptions.FileLocation, "*.csv").Length < 1)
        {
            context.WriteLine("There is no CSV files to get");
            return;
        }
        
        

        return;
    }
}