using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Entities;

public class WorkingTimeRecordAggregatedViewModel
{
    public RecordEventType EventType { get; set; }
    public DateTime Date { get; set; }
    public double WorkedMinutes { get; set; }
    public double WorkedHoursRounded { get; set; }
}