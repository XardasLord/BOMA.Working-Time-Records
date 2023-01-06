using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Entities;

public class WorkingTimeRecordAggregatedViewModel
{
    public static WorkingTimeRecordAggregatedViewModel EmptyForDay(DateTime date) => new()
    {
        Date = date,
        StartNormalizedAt = date,
        FinishNormalizedAt = date,
        StartOriginalAt = date,
        FinishOriginalAt = date,
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
    public DateTime StartNormalizedAt { get; set; }
    public DateTime? FinishNormalizedAt { get; set; }
    public DateTime StartOriginalAt { get; set; }
    public DateTime? FinishOriginalAt { get; set; }
    public double WorkedMinutes { get; set; }
    public double WorkedHoursRounded { get; set; }
    public double BaseNormativeHours { get; set; }
    public double FiftyPercentageBonusHours { get; set; }
    public double HundredPercentageBonusHours { get; set; }
    public double SaturdayHours { get; set; }
    public double NightHours { get; set; }
    public bool IsWeekendDay { get; set; }
    public MissingRecordEventType? MissingRecordEventType { get; set; }
}