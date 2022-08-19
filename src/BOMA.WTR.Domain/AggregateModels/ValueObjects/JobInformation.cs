namespace BOMA.WTR.Domain.AggregateModels.ValueObjects;

public record JobInformation
{
    public Position Position { get; init; }
    public ShiftType? ShiftType { get; init; }
}