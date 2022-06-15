using Herald.Core.Domain.ValueObjects.Soundtracks;
using MongoDB.Bson.Serialization.Attributes;

namespace Herald.Core.Domain.Entities.Soundtracks;

public sealed class SoundtrackQueueEntity : BaseEntity, IAggregateRoot
{
    [BsonRequired]
    public ulong GuildId { get; set; }
    
    public ulong NotifyChannelId { get; set; }

    public bool Playing { get; set; } = false;

    public IEnumerable<Soundtrack> Tracks { get; set; } = new List<Soundtrack>();
    
    public SoundtrackQueueEntity() { }

    public SoundtrackQueueEntity(ulong guildId, ulong notifyChannelId, bool playing = false, IEnumerable<Soundtrack>? tracks = null)
    {
        GuildId = guildId;
        NotifyChannelId = notifyChannelId;
        Playing = playing;

        if (tracks is not null)
        {
            Tracks = tracks;
        }
    }
}