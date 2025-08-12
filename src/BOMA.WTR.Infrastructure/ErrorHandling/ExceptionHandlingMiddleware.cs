using System.Text.Json;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Infrastructure.ErrorHandling.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ApplicationException = BOMA.WTR.Application.Exceptions.ApplicationException;

namespace BOMA.WTR.Infrastructure.ErrorHandling;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new
        {
            title = GetTitle(exception),
            status = statusCode,
            details = exception.Message,
            errors = GetErrors(exception)
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            ApplicationException => StatusCodes.Status400BadRequest,
            InfrastructureException => StatusCodes.Status424FailedDependency,
            _ => StatusCodes.Status500InternalServerError
        };
    private static string GetTitle(Exception exception) =>
        exception switch
        {
            ApplicationException applicationException => applicationException.Code,
            _ => "Server Error"
        };
    private static IReadOnlyDictionary<string, string>? GetErrors(Exception exception)
    {
        IReadOnlyDictionary<string, string>? errors = null;
        
        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
        }
        
        return errors;
    }
}