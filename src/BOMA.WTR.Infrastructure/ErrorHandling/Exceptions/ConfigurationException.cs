namespace BOMA.WTR.Infrastructure.ErrorHandling.Exceptions;

public class ConfigurationException : InfrastructureException
{
    public override string Code => "infrastructure_configuration_exception";

    public ConfigurationException(string message) : base(message)
    {
    }
}