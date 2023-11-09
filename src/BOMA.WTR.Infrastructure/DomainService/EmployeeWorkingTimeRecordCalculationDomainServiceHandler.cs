using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.Extensions;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Infrastructure.DomainService;

public class EmployeeWorkingTimeRecordCalculationDomainService : IEmployeeWorkingTimeRecordCalculationDomainService
{
    private const int MinSalary = 3600;
    
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
            var currentDay = timeRecord.OccuredAt.Day;
            var normalizedOccuredAt = NormalizeDateTime(timeRecord.EventType, timeRecord.OccuredAt);

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
                StartNormalizedAt = previousDateNormalized,
                FinishNormalizedAt = null,
                StartOriginalAt = previousDateOriginal,
                FinishOriginalAt = null,
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

    public double GetBaseNormativeHours(DateTime startWorkDate, DateTime endWorkDate, double workedHoursRounded)
    {
        if (startWorkDate.DayOfWeek is DayOfWeek.Saturday && endWorkDate.DayOfWeek is DayOfWeek.Saturday)
            return 0;
            
        return workedHoursRounded > 8 ? workedHoursRounded + 8 - workedHoursRounded : workedHoursRounded;
    }

    public double GetFiftyPercentageBonusHours(DateTime startWorkDate, DateTime endWorkDate, double workedHoursRounded)
    {
        if (startWorkDate.DayOfWeek is DayOfWeek.Saturday || endWorkDate.DayOfWeek is DayOfWeek.Saturday)
            return 0;
            
        return workedHoursRounded > 8 ? (workedHoursRounded - 8 > 2 ? 2 : workedHoursRounded - 8) : 0;
    }

    public double GetHundredPercentageBonusHours(DateTime startWorkDate, DateTime endWorkDate, double workedHoursRounded)
    {
        if (startWorkDate.DayOfWeek is DayOfWeek.Saturday && endWorkDate.DayOfWeek is DayOfWeek.Saturday)
            return 0;

        if (startWorkDate.DayOfWeek is DayOfWeek.Friday && endWorkDate.DayOfWeek is DayOfWeek.Saturday)
        {
            var saturdayStart = startWorkDate.Date.AddDays(1);
            
            var saturdayWorkDuration = endWorkDate.Subtract(saturdayStart);
            
            return (int)saturdayWorkDuration.TotalHours;
        }

        return workedHoursRounded > 10 ? workedHoursRounded - 10 : 0;
    }

    public double GetSaturdayHours(DateTime startWorkDate, DateTime endWorkDate, double workedHoursRounded)
    {
        return startWorkDate.DayOfWeek is DayOfWeek.Saturday && endWorkDate.DayOfWeek is DayOfWeek.Saturday ? workedHoursRounded : 0;
    }

    public double GetNightFactorBonus(int year, int month)
    {
        var workedHoursInMonth = new DateTime(year, month, 1).WorkingHoursInMonthExcludingBankHolidays();
        var nightFactor = MinSalary / workedHoursInMonth * 0.2;

        return Math.Round(nightFactor, 2);
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
            StartNormalizedAt = startWorkDateNormalized,
            FinishNormalizedAt = endWorkDateNormalized,
            StartOriginalAt = startWorkDateOriginal,
            FinishOriginalAt = endWorkDateOriginal,
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