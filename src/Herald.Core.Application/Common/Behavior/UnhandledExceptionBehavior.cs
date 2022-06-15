using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.Common.Behavior;

public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    
    public UnhandledExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            
            _logger.LogError(ex, "An Unhandled Exception Occurred: {Name} {@Request}",requestName, request);

            throw;
        }
    }
}