using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Entities;

public class WorkingTimeRecordAggregatedViewModel
{
    public static WorkingTimeRecordAggregatedViewModel EmptyForDay(DateTime date) => new()
    {
        Date = date,
        WorkTimePeriodNormalized = new WorkTimePeriod(date, date),
        WorkTimePeriodOriginal = new WorkTimePeriod(date, date),
        WorkedMinutes = 0,
        WorkedHoursRounded = 0,
        BaseNormativeHours = 0,
        FiftyPercentageBonusHours = 0,
        HundredPercentageBonusHours = 0,
        SaturdayHours = 0,
        NightHours = 0,
        IsWeekendDay = date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
    };
    
    public RecordEventType EventType { get; set; }
    public DateTime Date { get; set; }
    public WorkTimePeriod WorkTimePeriodNormalized { get; set; }
    public WorkTimePeriod WorkTimePeriodOriginal { get; set; }
    public double WorkedMinutes { get; set; }
    public double WorkedHoursRounded { get; set; }
    public double BaseNormativeHours { get; set; }
    public double FiftyPercentageBonusHours { get; set; }
    public double HundredPercentageBonusHours { get; set; }
    public double SaturdayHours { get; set; }
    public double NightHours { get; set; }
    public bool IsWeekendDay { get; set; }
    // public DateTime StartDayWorkNormalizedAt { get; set; }
    // public DateTime FinishDayWorkNormalizedAt { get; set; }
    // public DateTime StartNightWorkNormalizedAt { get; set; }
    // public DateTime FinishNightWorkNormalizedAt { get; set; }
    public MissingRecordEventType? MissingRecordEventType { get; set; }
}