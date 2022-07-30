using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.Extensions;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Infrastructure.DomainService;

public class EmployeeWorkingTimeRecordCalculationDomainService : IEmployeeWorkingTimeRecordCalculationDomainService
{
    public List<WorkingTimeRecordAggregatedViewModel> CalculateAggregatedWorkingTimeRecords(IEnumerable<WorkingTimeRecord> workingTimeRecords)
    {
        var results = new List<WorkingTimeRecordAggregatedViewModel>();
        var resultWithMissingRecords = new List<WorkingTimeRecordAggregatedViewModel>();

        var previousDay = 0;
        var aggregatedMinutesForDay = 0d;
        var previousDate = default(DateTime);
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
                    results.Add(CreateWorkingTimeRecord(aggregatedMinutesForDay, previousDate));
                    
                    aggregatedMinutesForDay = 0;
                } else if (previousEventType == RecordEventType.Entry && timeRecord.EventType == RecordEventType.Exit)
                {
                    aggregatedMinutesForDay += (int)(normalizedOccuredAt - previousDate).TotalMinutes;
                    
                    results.Add(CreateWorkingTimeRecord(aggregatedMinutesForDay, normalizedOccuredAt));
                    
                    aggregatedMinutesForDay = 0;
                    previousEventType = timeRecord.EventType;
                    previousDay = normalizedOccuredAt.Day;
                    previousDate = normalizedOccuredAt;
                    
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
                    
                    aggregatedMinutesForDay = 0;
                    var missingRecordType = timeRecord.EventType == RecordEventType.Entry ? MissingRecordEventType.MissingExit : MissingRecordEventType.MissingEntry;

                    resultWithMissingRecords.Add(CreateWorkingTimeRecord(aggregatedMinutesForDay, previousDate, missingRecordType));
                }
                
                if (timeRecord.EventType == RecordEventType.Exit)
                {
                    aggregatedMinutesForDay += (int)(NormalizeDateTime(timeRecord.EventType, normalizedOccuredAt) - previousDate).TotalMinutes;
                }
            }

            
            previousEventType = timeRecord.EventType;
            previousDay = normalizedOccuredAt.Day;
            previousDate = normalizedOccuredAt;
        }

        if (aggregatedMinutesForDay > 0)
        {
            results.Add(CreateWorkingTimeRecord(aggregatedMinutesForDay, previousDate));
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

    public double GetBaseNormativeHours(DateTime date, double workedHoursRounded)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday)
            return 0;
            
        return workedHoursRounded > 8 ? workedHoursRounded + 8 - workedHoursRounded : workedHoursRounded;
    }

    public double GetFiftyPercentageBonusHours(DateTime date, double workedHoursRounded)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday)
            return 0;
            
        return workedHoursRounded > 8 ? (workedHoursRounded - 8 > 2 ? 2 : workedHoursRounded - 8) : 0;
    }

    public double GetHundredPercentageBonusHours(DateTime date, double workedHoursRounded)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday)
            return 0;
            
        return workedHoursRounded > 10 ? workedHoursRounded - 10 : 0;
    }

    public double GetSaturdayHours(DateTime date, double workedHoursRounded)
    {
        return date.DayOfWeek == DayOfWeek.Saturday ? workedHoursRounded : 0;
    }

    public double GetNightFactorBonus(int year, int month)
    {
        var workedHoursInMonth = new DateTime(year, month, 1).WorkingHoursInMonthExcludingBankHolidays();
        var nightFactor = 3010 / workedHoursInMonth * 0.2;

        return nightFactor;
    }

    private static DateTime NormalizeDateTime(RecordEventType recordEventType, DateTime dateTime)
    {
        switch (recordEventType)
        {
            case RecordEventType.Entry:
                // Entries to work at 5:40-6:05 then system records it as 6:00
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
                
                // Entries to work at 6:06-6:39 then system records it as 6:30
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
                    return new DateTime(
                        dateTime.Year,
                        dateTime.Month,
                        dateTime.Day,
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

    private WorkingTimeRecordAggregatedViewModel CreateWorkingTimeRecord(double aggregatedMinutesForDay, DateTime endWorkDate, MissingRecordEventType? incorrectRecordEventTypeOrder = null)
    {
        var allWorkedHoursRounded = Math.Round(TimeSpan.FromMinutes(aggregatedMinutesForDay).TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
        var startWorkDate = endWorkDate.AddHours(-allWorkedHoursRounded);
        
        return new WorkingTimeRecordAggregatedViewModel
        {
            Date = startWorkDate.Date,
            WorkedMinutes = aggregatedMinutesForDay,
            WorkedHoursRounded = endWorkDate.DayOfWeek == DayOfWeek.Saturday ? 0 : allWorkedHoursRounded,
            BaseNormativeHours = GetBaseNormativeHours(endWorkDate, allWorkedHoursRounded),
            FiftyPercentageBonusHours = GetFiftyPercentageBonusHours(endWorkDate, allWorkedHoursRounded),
            HundredPercentageBonusHours = GetHundredPercentageBonusHours(endWorkDate, allWorkedHoursRounded),
            SaturdayHours = GetSaturdayHours(endWorkDate, allWorkedHoursRounded),
            NightHours = GetNightHours(),
            IsWeekendDay = startWorkDate.Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday,
            MissingRecordEventType = incorrectRecordEventTypeOrder
        };

        // TODO: Move this logic to domain service
        double GetNightHours()
        {
            if (endWorkDate.Date.DayOfWeek == DayOfWeek.Saturday)
                return 0;

            if (startWorkDate.Hour < 22 && endWorkDate.Hour > 6 && endWorkDate.Day > startWorkDate.Day)
            {
                // Worked all night
                return 8;
            }
            
            if (startWorkDate.Hour is < 22 and > 6 && endWorkDate.Hour is > 22 or <= 6)
            {
                // Started before 10pm but finished in night hours
                var nightWorkTimeSpan = endWorkDate.Subtract(new DateTime(startWorkDate.Year, startWorkDate.Month, startWorkDate.Day, 22, 0, 0));
                var result = Math.Round(nightWorkTimeSpan.TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
                return result > 0 ? result : 0;
            }
            
            if (startWorkDate.Hour is >= 22 or <= 6 && endWorkDate.Hour <= 6)
            {
                // Started work in night hours and finished in night hours
                var nightWorkTimeSpan = endWorkDate.Subtract(startWorkDate);
                var result = Math.Round(nightWorkTimeSpan.TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
                return result > 0 ? result : 0;
            }
            
            if (startWorkDate.Hour is >= 22 or < 6 && endWorkDate.Hour > 6)
            {
                // Started work in night hours and finished after 6am
                var nightWorkTimeSpan =  new DateTime(endWorkDate.Year, endWorkDate.Month, endWorkDate.Day, 6, 0, 0).Subtract(startWorkDate);
                var result = Math.Round(nightWorkTimeSpan.TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
                return result > 0 ? result : 0;
            }

            return 0;
        }
    }
}