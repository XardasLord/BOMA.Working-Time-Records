using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace BOMA.WTR.Application.MediatorRequestPipelines;

public class PreRequestLogger<TRequest> : IRequestPreProcessor<TRequest> 
    where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public PreRequestLogger(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken = default)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Request started: {requestName}", requestName);

        return Task.CompletedTask;
    }
}