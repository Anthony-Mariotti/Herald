namespace Herald.Core.Domain.Events.Soundtracks;

public class TrackEndedEvent : BaseEvent
{
    public ulong GuildId { get; }
    
    public string TrackId { get; }

    public TrackEndedEvent(ulong guildId, string trackId)
    {
        GuildId = guildId;
        TrackId = trackId;
    }
}