namespace BOMA.WTR.Application.Exceptions;

public class NotFoundException : ApplicationException
{
    public override string Code => "not_found_exception";

    public NotFoundException(string message) : base(message)
    {
    }
}