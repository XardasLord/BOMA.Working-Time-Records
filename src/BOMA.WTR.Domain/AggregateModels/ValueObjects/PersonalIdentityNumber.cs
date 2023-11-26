namespace BOMA.WTR.Domain.AggregateModels.ValueObjects;

public class PersonalIdentityNumber
{
    public string Number { get; init; }
    
    public static PersonalIdentityNumber Empty => new();
    
    private PersonalIdentityNumber() { }

    public PersonalIdentityNumber(string number)
    {
        Number = number;
    }
}