namespace BOMA.WTR.Application.Exceptions;

public class BadRequestException : ApplicationException
{
    public override string Code => "bad_request_exception";

    public BadRequestException(string message) : base(message)
    {
    }
}