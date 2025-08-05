using BOMA.WTR.Domain.AggregateModels.ValueObjects;

namespace BOMA.WTR.Infrastructure.InsERT.Gratyfikant.PayloadModels;

public class WorkingTimeRecordPayloadModel
{
    public DateTime Date { get; set; }
    public WorkTimePeriod? DayWorkTimePeriod { get; set; }
    public WorkTimePeriod? NightWorkTimePeriod { get; set; }
    public double WorkedMinutes { get; set; }
    public double WorkedHoursRounded { get; set; }
    public double BaseNormativeHours { get; set; }
    public double FiftyPercentageBonusHours { get; set; }
    public double HundredPercentageBonusHours { get; set; }
    public double SaturdayHours { get; set; }
    public double NightHours { get; set; }
}