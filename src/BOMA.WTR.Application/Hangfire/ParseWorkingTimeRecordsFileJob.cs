using System.Globalization;
using BOMA.WRT.Application.RogerFiles;
using BOMA.WTR.Domain.Entities;
using BOMA.WTR.Domain.Entities.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace BOMA.WRT.Application.Hangfire;

public class ParseWorkingTimeRecordsFileJob
{
    private readonly IWorkingTimeRecordRepository _workingTimeRecordRepository;
    private readonly RogerFileConfiguration _rogerFileOptions;

    public ParseWorkingTimeRecordsFileJob(IOptions<RogerFileConfiguration> options, IWorkingTimeRecordRepository workingTimeRecordRepository)
    {
        _workingTimeRecordRepository = workingTimeRecordRepository;
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

            var workingTimeRecords = rogerFileModels.Select(x =>
                WorkingTimeRecord.Create(
                    x.RogerEventType.Value,
                    new DateTime(x.Date.Value.Year, x.Date.Value.Month, x.Date.Value.Day, x.Time.Value.Hours, x.Time.Value.Minutes, x.Time.Value.Seconds),
                    x.UserRcpId.Value,
                    x.GroupId.Value));

            
            var recordsToAdd = await GetNonExistingOnly(workingTimeRecords);

            if (recordsToAdd.Any())
            {
                await _workingTimeRecordRepository.AddAsync(recordsToAdd.ToArray());
                await _workingTimeRecordRepository.SaveChangesAsync();
            }
            
            context.WriteLine($"Number of entries saved in DB - {recordsToAdd.Count}.");
        }
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

    private async Task<List<WorkingTimeRecord>> GetNonExistingOnly(IEnumerable<WorkingTimeRecord> workingTimeRecords)
    {
        var recordsToAdd = new List<WorkingTimeRecord>();
        foreach (var record in workingTimeRecords)
        {
            if (!await _workingTimeRecordRepository.ExistsAsync(record))
                recordsToAdd.Add(record);
        }

        return recordsToAdd;
    }
}