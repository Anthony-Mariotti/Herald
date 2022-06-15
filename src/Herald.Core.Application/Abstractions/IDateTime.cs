namespace Herald.Core.Application.Abstractions;

public interface IDateTime
{
    public DateTime Now { get; }
    
    public DateTime UtcNow { get; }
    
    public DateTimeOffset NowOffset { get; }
    
    public DateTimeOffset UtcNowOffset { get; }
}