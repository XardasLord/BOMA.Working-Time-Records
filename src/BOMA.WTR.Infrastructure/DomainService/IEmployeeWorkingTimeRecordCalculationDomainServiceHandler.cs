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
        DateTime previousDate = default;
        RecordEventType previousEventType = RecordEventType.None;
        
        foreach (var timeRecord in workingTimeRecords)
        {
            var currentDay = timeRecord.OccuredAt.Day;

            if (currentDay > previousDay && previousDay != 0) // Is a next day
            {
                if (previousEventType == RecordEventType.Exit && timeRecord.EventType == RecordEventType.Entry)
                {
                    result.Add(new WorkingTimeRecordAggregatedViewModel
                    {
                        WorkedMinutes = aggregatedMinutesForDay,
                        WorkedHoursRounded = Math.Round(TimeSpan.FromMinutes(aggregatedMinutesForDay).TotalHours * 2, MidpointRounding.AwayFromZero) / 2,
                        Date = previousDate.Date,
                        EventType = previousEventType
                    });
                    
                    aggregatedMinutesForDay = 0;
                } else if (previousEventType == RecordEventType.Entry && timeRecord.EventType == RecordEventType.Exit)
                {
                    // TODO: Add that difference between previous date and current date
                    aggregatedMinutesForDay += (int)(timeRecord.OccuredAt - previousDate).TotalMinutes;
                    
                    result.Add(new WorkingTimeRecordAggregatedViewModel
                    {
                        WorkedMinutes = aggregatedMinutesForDay,
                        WorkedHoursRounded = Math.Round(TimeSpan.FromMinutes(aggregatedMinutesForDay).TotalHours * 2, MidpointRounding.AwayFromZero) / 2,
                        Date = previousDate.Date,
                        EventType = previousEventType
                    });
                    
                    aggregatedMinutesForDay = 0;
                    previousEventType = timeRecord.EventType;
                    previousDay = timeRecord.OccuredAt.Day;
                    previousDate = timeRecord.OccuredAt;
                    
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
                    aggregatedMinutesForDay += (int)(timeRecord.OccuredAt - previousDate).TotalMinutes;
                }
            }

            
            previousEventType = timeRecord.EventType;
            previousDay = timeRecord.OccuredAt.Day;
            previousDate = timeRecord.OccuredAt;
        }

        if (aggregatedMinutesForDay > 0)
        {
            result.Add(new WorkingTimeRecordAggregatedViewModel
            {
                WorkedMinutes = aggregatedMinutesForDay,
                WorkedHoursRounded = Math.Round(TimeSpan.FromMinutes(aggregatedMinutesForDay).TotalHours * 2, MidpointRounding.AwayFromZero) / 2,
                Date = previousDate.Date,
                EventType = previousEventType
            });
        }
        
        return result;
    }
}