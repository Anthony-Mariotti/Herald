using Herald.Core.Application.Abstractions;

namespace Herald.Core.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;
    
    public DateTimeOffset NowOffset => DateTimeOffset.Now;
    
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;
}