﻿using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace BOMA.WTR.Application.MediatorRequestPipelines;

public class PostRequestLogger<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : MediatR.IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public PostRequestLogger(ILogger<TRequest> logger)
    {
        _logger = logger;
    }
    
    public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Request completed: {requestName} with response: {@response}", requestName, response);

        return Task.CompletedTask;
    }
}