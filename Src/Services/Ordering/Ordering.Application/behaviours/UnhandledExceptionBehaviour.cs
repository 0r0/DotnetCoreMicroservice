using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.behaviours;

public class UnhandledExceptionBehaviour<TRequest,TResponse>:IPipelineBehavior<TRequest,TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = nameof(TRequest);
            _logger.LogError(e,"application Request:unhandled exception of request-{Name} {@Request}",requestName,request);
           
            throw;
        }
    }
}