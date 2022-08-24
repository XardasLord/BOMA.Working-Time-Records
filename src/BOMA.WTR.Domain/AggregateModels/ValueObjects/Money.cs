namespace BOMA.WTR.Domain.AggregateModels.ValueObjects;

public record Money(decimal Amount, string Currency = "PLN")
{
    public static Money Empty => new(0);
}