using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.Common.Behavior;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly ILogger<TRequest> _requestLogger;

    private readonly Stopwatch _timer;
    
    public PerformanceBehavior(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger<PerformanceBehavior<TRequest, TResponse>>();
        _requestLogger = logger.CreateLogger<TRequest>();

        _timer = new Stopwatch();
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Running {Behavior} on {Request}", typeof(PerformanceBehavior<,>).Name,
            typeof(TRequest).Name);
        
        _timer.Start();

        var response = await next();
        
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            _logger.LogWarning("Herald Long Running Request: {Name} ({Elapsed} ms) {@Request}",
                typeof(TRequest).Name, elapsedMilliseconds, request);
        }

        return response;
    }
}