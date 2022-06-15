using DSharpPlus.Lavalink;
using Herald.Core.Domain.ValueObjects.Soundtracks;
using MongoDB.Bson.Serialization.Attributes;

namespace Herald.Core.Domain.Entities.Soundtracks;

public sealed class SoundtrackQueueEntity : BaseEntity, IAggregateRoot
{
    [BsonRequired]
    public ulong GuildId { get; set; }
    
    public ulong NotifyChannelId { get; set; }

    public bool Playing { get; set; } = false;

    public IList<Soundtrack> Tracks { get; set; } = new List<Soundtrack>();
    
    public SoundtrackQueueEntity() { }

    public SoundtrackQueueEntity(ulong guildId, ulong notifyChannelId, bool playing = false, IList<Soundtrack>? tracks = null)
    {
        GuildId = guildId;
        NotifyChannelId = notifyChannelId;
        Playing = playing;

        if (tracks is not null)
        {
            Tracks = tracks;
        }
    }

    public void AddToQueue(ulong notifyChannelId, LavalinkTrack track)
    {
        NotifyChannelId = notifyChannelId;
        Tracks.Add(Soundtrack.Create(track.Identifier, track.Author, track.Title, track.TrackString, track.Uri));
    }

    public void RemoveFromQueue(string trackIdentifier)
    {
        if (trackIdentifier == null) throw new ArgumentNullException(nameof(trackIdentifier));

        var track = Tracks.FirstOrDefault(x => x.Identifier!.Equals(trackIdentifier));

        if (track is not null)
        {
            Tracks.Remove(track);
        }
    }
}