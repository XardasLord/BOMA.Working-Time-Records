namespace BOMA.WTR.Domain.AggregateModels.ValueObjects;

public record PercentageBonus(double Value)
{
    public static PercentageBonus Empty => new(0);
}