using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Infrastructure.DomainService;

public class EmployeeWorkingTimeRecordCalculationDomainService : IEmployeeWorkingTimeRecordCalculationDomainService
{
    public IEnumerable<WorkingTimeRecordAggregatedViewModel> CalculateAggregatedWorkingTimeRecords(IEnumerable<WorkingTimeRecord> workingTimeRecords)
    {
        var result = new List<WorkingTimeRecordAggregatedViewModel>();

        var previousDay = 0;
        var aggregatedMinutesForDay = 0d;
        var previousDate = default(DateTime);
        var previousEventType = RecordEventType.None;
        
        foreach (var timeRecord in workingTimeRecords)
        {
            var currentDay = timeRecord.OccuredAt.Day;
            var normalizedOccuredAt = NormalizeDateTime(timeRecord.EventType, timeRecord.OccuredAt);

            if (currentDay > previousDay && previousDay != 0) // Is a next day
            {
                if (previousEventType == RecordEventType.Exit && timeRecord.EventType == RecordEventType.Entry)
                {
                    result.Add(CreateWorkingTimeRecord(aggregatedMinutesForDay, previousDate, previousEventType));
                    
                    aggregatedMinutesForDay = 0;
                } else if (previousEventType == RecordEventType.Entry && timeRecord.EventType == RecordEventType.Exit)
                {
                    aggregatedMinutesForDay += (int)(normalizedOccuredAt - previousDate).TotalMinutes;
                    
                    result.Add(CreateWorkingTimeRecord(aggregatedMinutesForDay, previousDate, previousEventType));
                    
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
                    // This should never happen for valid entries
                    continue;
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
            result.Add(CreateWorkingTimeRecord(aggregatedMinutesForDay, previousDate, previousEventType));
        }
        
        return result;
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

    private static WorkingTimeRecordAggregatedViewModel CreateWorkingTimeRecord(double aggregatedMinutesForDay, DateTime previousDate, RecordEventType previousEventType)
    {
        var allWorkedHoursRounded = Math.Round(TimeSpan.FromMinutes(aggregatedMinutesForDay).TotalHours * 2, MidpointRounding.AwayFromZero) / 2;
        
        return new WorkingTimeRecordAggregatedViewModel
        {
            Date = previousDate.Date,
            EventType = previousEventType,
            WorkedMinutes = aggregatedMinutesForDay,
            WorkedHoursRounded = allWorkedHoursRounded,
            BaseNormativeHours = previousDate.Date.DayOfWeek != DayOfWeek.Saturday ? allWorkedHoursRounded > 8 ? allWorkedHoursRounded + 8 - allWorkedHoursRounded : allWorkedHoursRounded : 0,
            FiftyPercentageBonusHours = previousDate.Date.DayOfWeek != DayOfWeek.Saturday ? allWorkedHoursRounded > 8 ? (allWorkedHoursRounded - 8 > 2 ? 2 : allWorkedHoursRounded - 8) : 0 : 0,
            HundredPercentageBonusHours = previousDate.Date.DayOfWeek != DayOfWeek.Saturday ? allWorkedHoursRounded > 10 ? allWorkedHoursRounded - 10 : 0 : 0,
            SaturdayHours = previousDate.Date.DayOfWeek == DayOfWeek.Saturday ? allWorkedHoursRounded : 0,
            NightHours = 0 // TODO
        };
    }
}