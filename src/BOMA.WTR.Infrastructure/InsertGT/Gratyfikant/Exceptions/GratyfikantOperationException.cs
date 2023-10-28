using BOMA.WTR.Infrastructure.ErrorHandling.Exceptions;

namespace BOMA.WTR.Infrastructure.InsertGT.Gratyfikant.Exceptions;

public class GratyfikantOperationException : InfrastructureException
{
    public override string Code => "gratyfikant_operation_exception";

    public GratyfikantOperationException(string message) : base(message)
    {
    }
}