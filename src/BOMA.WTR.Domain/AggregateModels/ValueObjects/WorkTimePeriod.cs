namespace BOMA.WTR.Domain.AggregateModels.ValueObjects;

public class WorkTimePeriod
{
    public WorkTimePeriod(DateTime from, DateTime? to)
    {
        From = from;
        To = to;
    }

    public DateTime From { get; }
    public DateTime? To { get; }
    public TimeSpan? Duration => To - From;
}