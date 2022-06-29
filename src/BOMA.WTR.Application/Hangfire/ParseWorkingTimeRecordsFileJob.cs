using System.Globalization;
using BOMA.WRT.Application.RogerFiles;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using CsvHelper;
using CsvHelper.Configuration;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace BOMA.WRT.Application.Hangfire;

public class ParseWorkingTimeRecordsFileJob
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly RogerFileConfiguration _rogerFileOptions;

    public ParseWorkingTimeRecordsFileJob(IOptions<RogerFileConfiguration> options, IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
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

            var employeesCache = new List<Employee>();

            foreach (var rogerFileModel in rogerFileModels)
            {
                var currentEmployee = employeesCache.SingleOrDefault(x => x.RcpId == rogerFileModel.UserRcpId);
                
                if (currentEmployee is null)
                {
                    currentEmployee = await _employeeRepository.GetByRcpIdAsync(rogerFileModel.UserRcpId.Value);
                    
                    if (currentEmployee is null)
                    {
                        currentEmployee = Employee.Add(new Name(rogerFileModel.Name, rogerFileModel.LastName), rogerFileModel.UserRcpId.Value);
                        await _employeeRepository.AddAsync(currentEmployee);
                    }
                    
                    employeesCache.Add(currentEmployee);
                }
                
                currentEmployee.AddWorkingTimeRecord(WorkingTimeRecord.Create(
                    rogerFileModel.RogerEventType.Value,
                    new DateTime(
                        rogerFileModel.Date.Value.Year,
                        rogerFileModel.Date.Value.Month,
                        rogerFileModel.Date.Value.Day, 
                        rogerFileModel.Time.Value.Hours, 
                        rogerFileModel.Time.Value.Minutes, 
                        rogerFileModel.Time.Value.Seconds),
                    rogerFileModel.GroupId.Value));
            }

            await _employeeRepository.SaveChangesAsync();
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
}