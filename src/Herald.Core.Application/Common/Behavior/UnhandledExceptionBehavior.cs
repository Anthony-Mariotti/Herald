using MediatR;
using Microsoft.Extensions.Logging;
// ReSharper disable ContextualLoggerProblem

namespace Herald.Core.Application.Common.Behavior;

public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;
    private readonly ILogger<TRequest> _requestLogger;
    
    public UnhandledExceptionBehavior(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger<UnhandledExceptionBehavior<TRequest, TResponse>>();
        _requestLogger = logger.CreateLogger<TRequest>();
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.LogTrace("Running {Behavior} on {Request}", typeof(UnhandledExceptionBehavior<,>).Name,
            typeof(TRequest).Name);
        
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            
            _requestLogger.LogError(ex, "An Unhandled Exception Occurred: {Name} {@Request}",requestName, request);

            throw;
        }
    }
}