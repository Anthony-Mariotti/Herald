using Herald.Core.Domain.ValueObjects.Soundtracks;

namespace Herald.Core.Domain.Events.Soundtracks;

public class TrackPlayingEvent : BaseEvent
{
    public ulong GuildId { get; }
    
    public QueuedTrackValue Track { get; }

    public TrackPlayingEvent(ulong guildId, QueuedTrackValue track)
    {
        GuildId = guildId;
        Track = track;
    }
}