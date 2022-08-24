namespace BOMA.WTR.Domain.AggregateModels.ValueObjects;

public record Position(string Name)
{
    public static Position Empty => new Position(string.Empty);
}