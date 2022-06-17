using Herald.Core.Domain.ValueObjects.Soundtracks;

namespace Herald.Core.Domain.Events.Soundtracks;

public class TrackQueuedEvent : BaseEvent
{
    public ulong GuildId { get; }
    
    public QueuedTrackValue Track { get; }

    public TrackQueuedEvent(ulong guildId, QueuedTrackValue track)
    {
        GuildId = guildId;
        Track = track;
    }
}