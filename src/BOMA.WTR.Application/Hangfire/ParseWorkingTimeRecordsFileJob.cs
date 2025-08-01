﻿using System.Globalization;
using BOMA.WTR.Application.RogerFiles;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Specifications;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;
using CsvHelper;
using CsvHelper.Configuration;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Options;

namespace BOMA.WTR.Application.Hangfire;

public class ParseWorkingTimeRecordsFileJob
{
    private readonly IAggregateRepository<Employee> _employeeRepository;
    private readonly RogerFileConfiguration _rogerFileOptions;

    public ParseWorkingTimeRecordsFileJob(IOptions<RogerFileConfiguration> options, IAggregateRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
        _rogerFileOptions = options.Value;
    }
    
    public async Task Execute(PerformContext? context, CancellationToken cancellationToken)
    {
        var files = Directory.GetFiles(_rogerFileOptions.FileLocation, "*.csv");
        
        if (files.Length < 1)
        {
            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine("There is no CSV files to get");
            return;
        }

        var addedNewEmployees = 0;
        var addedNewRecords = 0;

        foreach (var file in files)
        {
            var rogerFileModels = ParseCsvToRogerFile(file).ToList();

            context.WriteLine($"There are {rogerFileModels.Count} of TOTAL entries in the file - {file}");

            rogerFileModels = rogerFileModels.Where(x => x.IsValid()).ToList();

            context.WriteLine($"There are {rogerFileModels.Count} of VALID entries in the file - {file}");

            var employeesCache = new List<Employee>();

            foreach (var rogerFileModel in rogerFileModels)
            {
                var currentEmployee = employeesCache.SingleOrDefault(x => x.RcpId == rogerFileModel.UserRcpId);

                if (currentEmployee is null)
                {
                    if (!rogerFileModel.UserRcpId.HasValue)
                    {
                        continue;
                    }

                    if (rogerFileModel.DepartmentId!.Value == 0)
                    {
                        rogerFileModel.DepartmentId = 9; // Bez grupy
                    }

                    var spec = new EmployeeWithCurrentEntriesByRcpIdSpec(rogerFileModel.UserRcpId!.Value);
                    currentEmployee = await _employeeRepository.FirstOrDefaultAsync(spec, cancellationToken);

                    if (currentEmployee is null)
                    {
                        var name = new Name(rogerFileModel.Name!, rogerFileModel.LastName!);
                        var salary = Money.Empty;
                        var bonus = PercentageBonus.Empty;
                        var jobInformation = JobInformation.Empty;
                        var personalIdentityNumber = PersonalIdentityNumber.Empty;

                        currentEmployee = Employee.Add(name, salary, bonus, jobInformation, personalIdentityNumber, rogerFileModel.UserRcpId.Value, rogerFileModel.DepartmentId!.Value);
                        await _employeeRepository.AddAsync(currentEmployee, cancellationToken);

                        addedNewEmployees++;
                    }

                    employeesCache.Add(currentEmployee);
                }

                var addedRecord = currentEmployee.AddWorkingTimeRecord(WorkingTimeRecord.Create(
                    rogerFileModel.RogerEventType!.Value,
                    new DateTime(
                        rogerFileModel.Date!.Value.Year,
                        rogerFileModel.Date.Value.Month,
                        rogerFileModel.Date.Value.Day,
                        rogerFileModel.Time!.Value.Hours,
                        rogerFileModel.Time.Value.Minutes,
                        rogerFileModel.Time.Value.Seconds),
                    currentEmployee.DepartmentId));

                if (addedRecord)
                {
                    addedNewRecords++;
                }
            }

            await _employeeRepository.SaveChangesAsync(cancellationToken);

            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine($"There are {addedNewEmployees} added new employees from the file - {file}");
            
            context.SetTextColor(ConsoleTextColor.Green);
            context.WriteLine($"There are {addedNewRecords} added new records for employees from the file - {file}");

            File.Move(file, Path.Combine(_rogerFileOptions.ProcessedFileLocation, Path.GetFileName(file)), true);
        }
    }

    private static IEnumerable<RogerFileModel> ParseCsvToRogerFile(string fileLocation)
    {
        using var reader = new StreamReader(fileLocation, System.Text.Encoding.GetEncoding(1250));
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