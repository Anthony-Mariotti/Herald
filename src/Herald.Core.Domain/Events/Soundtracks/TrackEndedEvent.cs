using Herald.Core.Domain.Enums;

namespace Herald.Core.Domain.Events.Soundtracks;

public class TrackEndedEvent : BaseEvent
{
    public ulong GuildId { get; }
    
    public string Identifier { get; }
    
    public TrackStatusReason Reason { get; }

    public TrackEndedEvent(ulong guildId, string identifier, TrackStatusReason reason)
    {
        GuildId = guildId;
        Identifier = identifier;
        Reason = reason;
    }
}