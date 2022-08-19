namespace BOMA.WTR.Domain.AggregateModels.ValueObjects;

public record JobInformation
{
    public Position Position { get; init; }
    public ShiftType? ShiftType { get; init; }
    
    public static JobInformation Empty => new(Position.Empty, null);
    
    private JobInformation() { } // EF Core needs

    public JobInformation(Position position, ShiftType? shiftType)
    {
        Position = position;
        ShiftType = shiftType;
    }
}