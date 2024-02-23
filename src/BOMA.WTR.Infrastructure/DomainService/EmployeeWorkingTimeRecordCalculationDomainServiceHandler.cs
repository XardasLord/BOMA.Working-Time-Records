using BOMA.WTR.Application.RogerFiles;
using BOMA.WTR.Application.Salary;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.Extensions;
using BOMA.WTR.Domain.SharedKernel;
using Microsoft.Extensions.Options;

namespace BOMA.WTR.Infrastructure.DomainService;

public class EmployeeWorkingTimeRecordCalculationDomainService : IEmployeeWorkingTimeRecordCalculationDomainService
{
    private readonly SalaryConfiguration _salaryOptions;

    public EmployeeWorkingTimeRecordCalculationDomainService(IOptions<SalaryConfiguration> options)
    {
        _salaryOptions = options.Value;
    }
    
    public List<WorkingTimeRecordAggregatedViewModel> CalculateAggregatedWorkingTimeRecords(IEnumerable<WorkingTimeRecord> workingTimeRecords)
    {
        var results = new List<WorkingTimeRecordAggregatedViewModel>();
        var resultWithMissingRecords = new List<WorkingTimeRecordAggregatedViewModel>();

        var previousDay = 0;
        var aggregatedMinutesForDayNormalized = 0d;
        var aggregatedMinutesForDayOriginal = 0d;
        var previousDateNormalized = default(DateTime);
        var previousDateOriginal = default(DateTime);
        var previousEventType = RecordEventType.None;

        workingTimeRecords = workingTimeRecords.OrderBy(x => x.OccuredAt).ToList();
        
        // TODO: Need a refactor to make it much simplier to understand
        foreach (var timeRecord in workingTimeRecords)
        {
            var normalizedOccuredAt = NormalizeDateTime(timeRecord.EventType, timeRecord.OccuredAt);
            var currentDay = normalizedOccuredAt.Day;

            if (currentDay != previousDay && previousDay != 0) // Is a next day
            {
                if (previousEventType == RecordEventType.Exit && timeRecord.EventType == RecordEventType.Entry)
                {
                    results.Add(CreateWorkingTimeRecord(aggregatedMinutesForDayNormalized, previousDateNormalized, aggregatedMinutesForDayOriginal, previousDateOriginal));
                    
                    aggregatedMinutesForDayNormalized = 0;
                    aggregatedMinutesForDayOriginal = 0;
                } else if (previousEventType == RecordEventType.Entry && timeRecord.EventType == RecordEventType.Exit)
                {
                    aggregatedMinutesForDayNormalized += (int)(normalizedOccuredAt - previousDateNormalized).TotalMinutes;
                    aggregatedMinutesForDayOriginal += (int)(timeRecord.OccuredAt - previousDateOriginal).TotalMinutes;
                    
                    results.Add(CreateWorkingTimeRecord(aggregatedMinutesForDayNormalized, normalizedOccuredAt, aggregatedMinutesForDayOriginal, timeRecord.OccuredAt));
                    
                    aggregatedMinutesForDayNormalized = 0;
                    aggregatedMinutesForDayOriginal = 0;
                    previousEventType = timeRecord.EventType;
                    previousDay = normalizedOccuredAt.Day;
                    previousDateNormalized = normalizedOccuredAt;
                    previousDateOriginal = timeRecord.OccuredAt;
                    
                    continue;
                }
            }

            if (previousEventType is not RecordEventType.None)
            {
                if (previousEventType == timeRecord.EventType)
                {
                    // This should never happen for valid entries but we have to handle such scenario anyway
                    if (timeRecord.EventType == RecordEventType.Exit)
                    {
                        continue;
                    }
                    
                    aggregatedMinutesForDayNormalized = 0;
                    aggregatedMinutesForDayOriginal = 0;
                    var missingRecordType = timeRecord.EventType == RecordEventType.Entry ? MissingRecordEventType.MissingExit : MissingRecordEventType.MissingEntry;

                    resultWithMissingRecords.Add(CreateWorkingTimeRecord(aggregatedMinutesForDayNormalized, previousDateNormalized, aggregatedMinutesForDayOriginal, previousDateOriginal, missingRecordType));
                }
                
                if (timeRecord.EventType == RecordEventType.Exit)
                {
                    var minutesNormalized = (int)(NormalizeDateTime(timeRecord.EventType, normalizedOccuredAt) - previousDateNormalized).TotalMinutes;
                    var minutesOriginal = (timeRecord.OccuredAt - previousDateOriginal).TotalMinutes;
                    aggregatedMinutesForDayNormalized += minutesNormalized > 0 ? minutesNormalized : 0;
                    aggregatedMinutesForDayOriginal += minutesOriginal > 0 ? minutesOriginal : 0;
                }
            }

            
            previousEventType = timeRecord.EventType;
            previousDay = normalizedOccuredAt.Day;
            previousDateNormalized = normalizedOccuredAt;
            previousDateOriginal = timeRecord.OccuredAt;
        }

        if (aggregatedMinutesForDayNormalized > 0)
        {
            results.Add(CreateWorkingTimeRecord(aggregatedMinutesForDayNormalized, previousDateNormalized, aggregatedMinutesForDayOriginal, previousDateOriginal));
        }

        if (previousEventType == RecordEventType.Entry)
        {
            // Employee entries the work but did not finish yet
            results.Add(new WorkingTimeRecordAggregatedViewModel
            {
                Date = previousDateOriginal.Date,
                WorkTimePeriodNormalized = new WorkTimePeriod(previousDateNormalized, null),
                WorkTimePeriodOriginal = new WorkTimePeriod(previousDateOriginal, null),
                WorkedMinutes = 0,
                WorkedHoursRounded = 0,
                BaseNormativeHours = 0,
                FiftyPercentageBonusHours = 0,
                HundredPercentageBonusHours = 0,
                SaturdayHours = 0,
                NightHours = 0,
                IsWeekendDay = previousDateOriginal.Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
                MissingRecordEventType = null
            });
        }
        
        // TODO: Merge entries with missing record type data
        foreach (var entryWithMissingRecord in resultWithMissingRecords)
        {
            var existingEntry = results.FirstOrDefault(x => x.Date == entryWithMissingRecord.Date);
            if (existingEntry is null)
            {
                results.Add(entryWithMissingRecord);
            }
            else
            {
                existingEntry.MissingRecordEventType = entryWithMissingRecord.MissingRecordEventType;
            }
        }
        
        // Sort to keep dates in order
        results.Sort((x, y) => x.Date.CompareTo(y.Date));
        return results;
    }

    // TODO: Need a refactor to make it much simpler to understand
    public DateTime NormalizeDateTime(RecordEventType recordEventType, DateTime dateTime)
    {
        switch (recordEventType)
        {
            case RecordEventType.Entry:
                
                // First shift (06:00 - 16:00)
                if (dateTime.Hour < 6 ||
                    dateTime.Hour == 6 && dateTime.Minute <= 5)
                {
                    // Entries to work at X:40-6:05 then system records it as 6:00
                    return new DateTime(
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
                        6,
                        0,
                        0);
                }

                if (dateTime.Hour is >= 6 and <= 12)
                {
                    if (dateTime.Minute is >= 40 or <= 5)
                    {
                        return new DateTime(
                            dateTime.Year,
                            dateTime.Month,
                            dateTime.Day,
                            dateTime.Minute is >= 0 and <= 5 ? dateTime.Hour : dateTime.AddHours(1).Hour,
                            0,
                            0);
                    }

                    // Entries to work at X:06-X:39 then system records it as X:30
                    return new DateTime(
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
                        dateTime.Hour,
                        30,
                        0);
                }
                
                // Third shift (14:00 - 22:00)
                if (dateTime.Hour == 13 ||
                    dateTime.Hour == 14 && dateTime.Minute <= 5)
                {
                    return new DateTime(
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
                        14,
                        0,
                        0);
                }

                if (dateTime.Hour == 14)
                {
                    if (dateTime.Minute is > 5 and <= 39)
                    {
                        return new DateTime(
                            dateTime.Year,
                            dateTime.Month,
                            dateTime.Day,
                            14,
                            30,
                            0);
                    }

                    return new DateTime(
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
                        15,
                        0,
                        0);
                }

                // Second shift (16:00 - 02:00)
                if (dateTime.Hour < 16 ||
                    dateTime.Hour == 16 && dateTime.Minute <= 5)
                {
                    // Entries to work at X:40-16:05 then system records it as 16:00
                    return new DateTime(
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
                        16,
                        0,
                        0);
                }

                // Hours > 16
                if (dateTime.Minute is >= 40 or <= 5)
                {
                    return new DateTime(
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
                        dateTime.Minute is >= 0 and <= 5 ? dateTime.Hour : dateTime.AddHours(1).Hour,
                        0,
                        0);
                }

                // Entries to work at X:06-X:39 then system records it as X:30
                return new DateTime(
                    dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    dateTime.Hour,
                    30,
                    0);

            case RecordEventType.Exit:
                // Exists work at 13:55-14:24 then system records it as 14:00
                if (dateTime.Minute is >= 55 or <= 24)
                {
                    var day = dateTime.Minute is >= 55 and <= 59 && dateTime.Hour is 23 ? dateTime.AddDays(1).Day : dateTime.Day;
                        
                    return new DateTime(
                        dateTime.Year,
                        dateTime.Month,
                        day,
                        dateTime.Minute is >= 55 and <= 59 ? dateTime.AddHours(1).Hour : dateTime.Hour,
                        0,
                        0);
                }
                
                // Exists work at 14:25-14:54 then system records it as 14:30
                return new DateTime(
                    dateTime.Year,
                    dateTime.Month,
                    dateTime.Day,
                    dateTime.Hour,
                    30,
                    0);
            case RecordEventType.None:
                return dateTime;
            default:
                throw new ArgumentOutOfRangeException(nameof(recordEventType), recordEventType, null);
        }
    }

    public double GetBaseNormativeHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized, double workedHoursRounded)
    {
        if (startWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday && endWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday)
            return 0;
            
        return workedHoursRounded > 8 ? workedHoursRounded + 8 - workedHoursRounded : workedHoursRounded;
    }

    public double GetFiftyPercentageBonusHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized, double workedHoursRounded)
    {
        if (startWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday || (startWorkDateNormalized.DayOfWeek is DayOfWeek.Friday && endWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday && endWorkDateNormalized.Hour > 0))
            return 0;
            
        return workedHoursRounded > 8 ? (workedHoursRounded - 8 > 2 ? 2 : workedHoursRounded - 8) : 0;
    }

    public double GetHundredPercentageBonusHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized, double workedHoursRounded)
    {
        if (startWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday && endWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday)
            return 0;

        if (startWorkDateNormalized.DayOfWeek is DayOfWeek.Friday && endWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday)
        {
            var saturdayStart = startWorkDateNormalized.Date.AddDays(1);
            
            var saturdayWorkDuration = endWorkDateNormalized.Subtract(saturdayStart);
            
            return (int)saturdayWorkDuration.TotalHours;
        }

        return workedHoursRounded > 10 ? workedHoursRounded - 10 : 0;
    }

    public double GetSaturdayHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized, double workedHoursRounded)
    {
        return startWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday && endWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday ? workedHoursRounded : 0;
    }
    
    public double GetNightHours(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized)
    {
        if (startWorkDateNormalized.Date.DayOfWeek is DayOfWeek.Saturday && endWorkDateNormalized.Date.DayOfWeek is DayOfWeek.Saturday)
            return 0;

        if (startWorkDateNormalized.Hour < 22 && endWorkDateNormalized.Hour > 6 && endWorkDateNormalized.Day > startWorkDateNormalized.Day)
        {
            // Worked all night
            return 8;
        }
            
        if (startWorkDateNormalized.Hour is < 22 and > 6 && endWorkDateNormalized.Hour is > 22 or <= 6)
        {
            // Started before 10pm but finished in night hours
            var nightWorkTimeSpan = endWorkDateNormalized.Subtract(new DateTime(startWorkDateNormalized.Year, startWorkDateNormalized.Month, startWorkDateNormalized.Day, 22, 0, 0));
            var result = Math.Round(nightWorkTimeSpan.TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
            return result > 0 ? result : 0;
        }
            
        if (startWorkDateNormalized.Hour is >= 22 or <= 6 && endWorkDateNormalized.Hour <= 6)
        {
            // Started work in night hours and finished in night hours
            var nightWorkTimeSpan = endWorkDateNormalized.Subtract(startWorkDateNormalized);
            var result = Math.Round(nightWorkTimeSpan.TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
            return result > 0 ? result : 0;
        }
            
        if (startWorkDateNormalized.Hour is >= 22 or < 6 && endWorkDateNormalized.Hour > 6)
        {
            // Started work in night hours and finished after 6am
            var nightWorkTimeSpan =  new DateTime(endWorkDateNormalized.Year, endWorkDateNormalized.Month, endWorkDateNormalized.Day, 6, 0, 0).Subtract(startWorkDateNormalized);
            var result = Math.Round(nightWorkTimeSpan.TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
            return result > 0 ? result : 0;
        }

        return 0;
    }

    public double GetNightFactorBonus(int year, int month)
    {
        var workedHoursInMonth = new DateTime(year, month, 1).WorkingHoursInMonthExcludingBankHolidays();
        var nightFactor = _salaryOptions.MinSalary / workedHoursInMonth * 0.2;

        return Math.Round(nightFactor, 2);
    }

    public WorkTimePeriod? GetDayWorkTimePeriod(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized)
    {
        if (startWorkDateNormalized > endWorkDateNormalized)
            throw new ArgumentException("Czas rozpoczęcia pracy nie może być późniejszy niż czas zakończenia pracy.");
        
        var dayStart = new DateTime(startWorkDateNormalized.Year, startWorkDateNormalized.Month, startWorkDateNormalized.Day, 6, 0, 0);
        var dayEnd = new DateTime(dayStart.Year, dayStart.Month, dayStart.Day, 22, 0, 0);
        
        // Praca zaczyna się i kończy w ciągu dnia.
        if (WorkStartsAtDay() && endWorkDateNormalized <= dayEnd && endWorkDateNormalized < dayStart.AddDays(1))
            return new WorkTimePeriod(startWorkDateNormalized, endWorkDateNormalized);
        
        // Praca rozpoczyna się po godzinie 22:00 i kończy przed 6:00 następnego dnia.
        if (startWorkDateNormalized >= dayEnd && endWorkDateNormalized <= dayStart.AddDays(1))
            return null;

        // Praca zaczyna się i kończy poza standardowymi godzinami pracy (np. nocą).
        if ((startWorkDateNormalized < dayStart && endWorkDateNormalized <= dayStart) || (startWorkDateNormalized >= dayEnd && endWorkDateNormalized >= dayEnd))
            return null;

        // Praca zaczyna się w ciągu dnia i kończy przed 6:00 następnego dnia.
        if (WorkStartsAtDay() && endWorkDateNormalized > dayEnd && endWorkDateNormalized < dayStart.AddDays(1))
            return new WorkTimePeriod(startWorkDateNormalized, dayEnd);
        
        // Praca zaczyna się w ciągu dnia i kończy po 22:00 tego samego dnia.
        if (WorkStartsAtDay() && endWorkDateNormalized > dayEnd)
            return new WorkTimePeriod(startWorkDateNormalized, dayEnd);
        
        // Praca zaczyna się przed godziną 6:00 i kończy po 22:00 tego samego dnia.
        if (startWorkDateNormalized < dayStart && endWorkDateNormalized > dayEnd)
            return new WorkTimePeriod(dayStart, dayEnd);
        
        // Praca zaczyna się przed godziną 6:00, ale kończy później tego samego dnia.
        if (startWorkDateNormalized < dayStart && endWorkDateNormalized > dayStart && endWorkDateNormalized <= dayEnd)
            return new WorkTimePeriod(dayStart, endWorkDateNormalized);

        // Praca rozpoczyna się po godzinie 22:00, ale kończy następnego dnia po godzinie 6:00.
        if (startWorkDateNormalized >= dayEnd && endWorkDateNormalized > dayStart.AddDays(1))
            return new WorkTimePeriod(dayStart.AddDays(1), endWorkDateNormalized);
        
        // Przypadek domyślny - powinien być obsłużony przez powyższe warunki.
        return new WorkTimePeriod(startWorkDateNormalized, endWorkDateNormalized);

        bool WorkStartsAtDay()
        {
            return startWorkDateNormalized >= dayStart && startWorkDateNormalized < dayEnd;
        }
    }

    public WorkTimePeriod? GetNightWorkTimePeriod(DateTime startWorkDateNormalized, DateTime endWorkDateNormalized)
    {
        if (startWorkDateNormalized > endWorkDateNormalized)
            throw new ArgumentException("Czas rozpoczęcia pracy nie może być późniejszy niż czas zakończenia pracy.");
        
        var nightStart = new DateTime(startWorkDateNormalized.Year, startWorkDateNormalized.Month, startWorkDateNormalized.Day, 22, 0, 0);

        if (startWorkDateNormalized.Hour >= 22)
            nightStart = new DateTime(startWorkDateNormalized.Year, startWorkDateNormalized.Month, startWorkDateNormalized.Day, 22, 0, 0);
        else if (startWorkDateNormalized.Hour < 6)
            nightStart = new DateTime(startWorkDateNormalized.Year, startWorkDateNormalized.Month, startWorkDateNormalized.Day, 22, 0, 0).AddDays(-1);
        else if (startWorkDateNormalized.Hour >= 6)
            nightStart = new DateTime(startWorkDateNormalized.Year, startWorkDateNormalized.Month, startWorkDateNormalized.Day, 22, 0, 0);
        
        var nightEnd = new DateTime(nightStart.Year, nightStart.Month, nightStart.Day, 6, 0, 0).AddDays(1);

        // Praca zaczyna się i kończy w ciągu nocy.
        if (WorkStartsAtNight() && WorkEndsAtNight())
            return new WorkTimePeriod(startWorkDateNormalized, endWorkDateNormalized);

        // Praca zaczyna się w ciągu nocy i kończy po 6:00 następnego dnia.
        if (WorkStartsAtNight() && endWorkDateNormalized > nightEnd)
            return new WorkTimePeriod(startWorkDateNormalized, nightEnd);
        
        // Praca zaczyna się w ciągu dnia i kończy w nocy.
        if (WorkStartsAtDay() && endWorkDateNormalized > nightStart && endWorkDateNormalized <= nightEnd)
            return new WorkTimePeriod(nightStart, endWorkDateNormalized);
        
        // Praca zaczyna się w ciągu dnia i kończy następnego dnia w ciągu dnia.
        if (WorkStartsAtDay() && endWorkDateNormalized > nightStart && endWorkDateNormalized > nightEnd)
            return new WorkTimePeriod(nightStart, nightEnd);
        
        // Praca zaczyna się i kończy w ciągu dnia.
        if (WorkStartsAtDay() && WorkEndsAtDay())
            return null;
        
        // Przypadek domyślny - powinien być obsłużony przez powyższe warunki.
        return new WorkTimePeriod(startWorkDateNormalized, endWorkDateNormalized);

        bool WorkStartsAtNight()
        {
            return startWorkDateNormalized >= nightStart && startWorkDateNormalized < nightEnd;
        }

        bool WorkEndsAtNight()
        {
            return endWorkDateNormalized <= nightEnd && endWorkDateNormalized > nightStart;
        }

        bool WorkStartsAtDay()
        {
            return startWorkDateNormalized >= nightEnd.AddDays(-1);
        }

        bool WorkEndsAtDay()
        {
            return endWorkDateNormalized <= nightStart;
        }
    }

    
    // TODO: Move to separate domain service
    public void RecalculateSalary(List<WorkingTimeRecordAggregatedHistory> aggregatedHistoryRecordsForMonth)
    {
        var baseSalary = aggregatedHistoryRecordsForMonth.First().SalaryInformation.BaseSalary;
        var percentageBonusSalary = aggregatedHistoryRecordsForMonth.First().SalaryInformation.PercentageBonusSalary;

        foreach (var historyRecord in aggregatedHistoryRecordsForMonth)
        {
            historyRecord.SalaryInformation.Base50PercentageSalary = Math.Round(baseSalary * 1.5m, 2);
            historyRecord.SalaryInformation.Base100PercentageSalary = Math.Round(baseSalary * 2m, 2);
            historyRecord.SalaryInformation.BaseSaturdaySalary = Math.Round(baseSalary * 2m, 2);
            
            historyRecord.SalaryInformation.GrossBaseSalary = CalculateGrossBaseSalary(baseSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossBase50PercentageSalary = CalculateGrossBase50PercentageSalary(baseSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossBase100PercentageSalary = CalculateGrossBase100PercentageSalary(baseSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossBaseSaturdaySalary = CalculateGrossBaseSaturdaySalary(baseSalary, aggregatedHistoryRecordsForMonth);
            
            historyRecord.SalaryInformation.BonusBaseSalary = CalculateBonusBaseSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.BonusBase50PercentageSalary = CalculateBonusBase50PercentageSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.BonusBase100PercentageSalary = CalculateBonusBase100PercentageSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.BonusBaseSaturdaySalary = CalculateBonusBaseSaturdaySalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            
            historyRecord.SalaryInformation.GrossSumBaseSalary = CalculateGrossSumBaseSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossSumBase50PercentageSalary = CalculateGrossSumBase50PercentageSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossSumBase100PercentageSalary = CalculateGrossSumBase100PercentageSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.GrossSumBaseSaturdaySalary = CalculateGrossSumBaseSaturdaySalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            
            historyRecord.SalaryInformation.BonusSumSalary = CalculateBonusSumSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.NightBaseSalary = CalculateNightSumSalary(aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.NightWorkedHours = CalculateAllNightWorkedHours(aggregatedHistoryRecordsForMonth);
            historyRecord.SalaryInformation.FinalSumSalary = CalculateFinalSumSalary(baseSalary, percentageBonusSalary, aggregatedHistoryRecordsForMonth);
        }
    }

    private static decimal CalculateGrossBaseSalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(baseSalary * (decimal)records.Sum(x => x.BaseNormativeHours), 2);
    }

    private static decimal CalculateGrossBase50PercentageSalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(baseSalary * 1.5m * (decimal)records.Sum(x => x.FiftyPercentageBonusHours), 2);
    }

    private static decimal CalculateGrossBase100PercentageSalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(baseSalary * 2m * (decimal)records.Sum(x => x.HundredPercentageBonusHours), 2);
    }

    private static decimal CalculateGrossBaseSaturdaySalary(decimal baseSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(baseSalary * 2m * (decimal)records.Sum(x => x.SaturdayHours), 2);
    }

    private static decimal CalculateBonusBaseSalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(CalculateGrossBaseSalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateBonusBase50PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(CalculateGrossBase50PercentageSalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateBonusBase100PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(CalculateGrossBase100PercentageSalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateBonusBaseSaturdaySalary(decimal baseSalary, decimal bonusPercentageSalary, IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return Math.Round(CalculateGrossBaseSaturdaySalary(baseSalary, records) * bonusPercentageSalary / 100, 2);
    }

    private static decimal CalculateGrossSumBaseSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBaseSalary(baseSalary, records) + 
               CalculateBonusBaseSalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateGrossSumBase50PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBase50PercentageSalary(baseSalary, records) + 
               CalculateBonusBase50PercentageSalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateGrossSumBase100PercentageSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBase100PercentageSalary(baseSalary, records) + 
               CalculateBonusBase100PercentageSalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateGrossSumBaseSaturdaySalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBaseSaturdaySalary(baseSalary, records) + 
               CalculateBonusBaseSaturdaySalary(baseSalary, bonusPercentageSalary, records);
    }

    private static decimal CalculateBonusSumSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        return CalculateGrossBase50PercentageSalary(baseSalary, records) + 
               CalculateGrossBase100PercentageSalary(baseSalary, records) + 
               CalculateGrossBaseSaturdaySalary(baseSalary, records);
    }

    private decimal CalculateNightSumSalary(IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        var period = records.FirstOrDefault();
        if (period is null)
            return 0;
        
        var nightFactor = GetNightFactorBonus(period.Date.Year, period.Date.Month);
        var nightWorkedHours = (decimal)CalculateAllNightWorkedHours(records);
        var nightSumSalary =  (decimal)nightFactor * nightWorkedHours;

        return Math.Round(nightSumSalary, 2);
    }

    private decimal CalculateFinalSumSalary(decimal baseSalary, decimal bonusPercentageSalary, IReadOnlyCollection<WorkingTimeRecordAggregatedHistory> records)
    {
        var firstRecord = records.First();
        
        return CalculateGrossSumBaseSalary(baseSalary, bonusPercentageSalary, records) +
               CalculateGrossSumBase50PercentageSalary(baseSalary, bonusPercentageSalary, records) +
               CalculateGrossSumBase100PercentageSalary(baseSalary, bonusPercentageSalary, records) +
               CalculateGrossSumBaseSaturdaySalary(baseSalary, bonusPercentageSalary, records) +
               CalculateNightSumSalary(records) +
               firstRecord.SalaryInformation.HolidaySalary +
               firstRecord.SalaryInformation.SicknessSalary +
               firstRecord.SalaryInformation.AdditionalSalary;
    }

    private static double CalculateAllNightWorkedHours(IEnumerable<WorkingTimeRecordAggregatedHistory> records)
    {
        return records.Sum(x => x.NightHours);
    }
    
    

    private WorkingTimeRecordAggregatedViewModel CreateWorkingTimeRecord(
        double aggregatedMinutesForDayNormalized,
        DateTime endWorkDateNormalized,
        double aggregatedMinutesForDayOriginal,
        DateTime endWorkDateOriginal,
        MissingRecordEventType? incorrectRecordEventTypeOrder = null)
    {
        var allWorkedHoursRounded = Math.Round(TimeSpan.FromMinutes(aggregatedMinutesForDayNormalized).TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
        var startWorkDateNormalized = endWorkDateNormalized.AddHours(-allWorkedHoursRounded);
        var startWorkDateOriginal = endWorkDateOriginal.AddMinutes(-aggregatedMinutesForDayOriginal);
        
        return new WorkingTimeRecordAggregatedViewModel
        {
            Date = startWorkDateNormalized.Date,
            WorkTimePeriodOriginal = new WorkTimePeriod(startWorkDateOriginal, endWorkDateOriginal),
            WorkTimePeriodNormalized = new WorkTimePeriod(startWorkDateNormalized, endWorkDateNormalized),
            DayWorkTimePeriodNormalized = GetDayWorkTimePeriod(startWorkDateNormalized, endWorkDateNormalized),
            NightWorkTimePeriodNormalized = GetNightWorkTimePeriod(startWorkDateNormalized, endWorkDateNormalized),
            WorkedMinutes = aggregatedMinutesForDayNormalized,
            WorkedHoursRounded = startWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday && endWorkDateNormalized.DayOfWeek is DayOfWeek.Saturday ? 0 : allWorkedHoursRounded,
            BaseNormativeHours = GetBaseNormativeHours(startWorkDateNormalized, endWorkDateNormalized, allWorkedHoursRounded),
            FiftyPercentageBonusHours = GetFiftyPercentageBonusHours(startWorkDateNormalized, endWorkDateNormalized, allWorkedHoursRounded),
            HundredPercentageBonusHours = GetHundredPercentageBonusHours(startWorkDateNormalized, endWorkDateNormalized, allWorkedHoursRounded),
            SaturdayHours = GetSaturdayHours(startWorkDateNormalized, endWorkDateNormalized, allWorkedHoursRounded),
            NightHours = GetNightHours(startWorkDateNormalized, endWorkDateNormalized),
            IsWeekendDay = startWorkDateNormalized.Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
            MissingRecordEventType = incorrectRecordEventTypeOrder
        };
    }
}