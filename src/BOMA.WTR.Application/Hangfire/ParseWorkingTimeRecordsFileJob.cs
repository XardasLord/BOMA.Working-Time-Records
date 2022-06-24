using System.Globalization;
using BOMA.WRT.Application.RogerFiles;
using CsvHelper;
using CsvHelper.Configuration;
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
        var files = Directory.GetFiles(_rogerFileOptions.FileLocation, "*.csv");
        
        if (files.Length < 1)
        {
            context.WriteLine("There is no CSV files to get");
            return;
        }

        foreach (var file in files)
        {
            var rogerFileModels = ParseCsvToRogerFile(file);
            rogerFileModels = rogerFileModels.Where(x => x.IsValid()).ToList();
            
            context.WriteLine($"There are {rogerFileModels.Count()} valid entries in the file - {file}");

            // TODO: add entries to DB
        }

        return;
    }

    private static IEnumerable<RogerFileModel> ParseCsvToRogerFile(string fileLocation)
    {
        using var reader = new StreamReader(fileLocation);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = true,
            MissingFieldFound = null,
            TrimOptions = TrimOptions.Trim
        });
        
        return csv.GetRecords<RogerFileModel>().ToList();
    }
}