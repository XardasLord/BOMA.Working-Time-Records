namespace BOMA.WTR.Infrastructure.ErrorHandling.Exceptions;

public abstract class InfrastructureException : Exception
{
    public abstract string Code { get; }

    protected InfrastructureException(string message) : base(message)
    {
    }
}