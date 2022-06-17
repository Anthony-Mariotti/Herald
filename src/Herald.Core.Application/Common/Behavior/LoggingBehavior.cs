using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.Common.Behavior;

public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest>> _logger;
    private readonly ILogger<TRequest> _requestLogger;
    
    public LoggingBehavior(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger<LoggingBehavior<TRequest>>();
        _requestLogger = logger.CreateLogger<TRequest>();
    }
    
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Running {Behavior} on {Request}", typeof(LoggingBehavior<>).Name,
            typeof(TRequest).Name);
        
        _logger.LogDebug("Herald Handling Request: {Name} {@Request}", typeof(TRequest).Name, request);

        return Task.CompletedTask;
    }
}