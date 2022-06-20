using Herald.Core.Domain.Common.Extensions;
using Herald.Core.Domain.ValueObjects.Soundtracks;

namespace Herald.Core.Domain.Entities.Soundtracks;

public sealed class QueueEntity : BaseEntity, IAggregateRoot
{
    public ulong GuildId { get; set; }
    
    public ICollection<QueuedTrackValue> Tracks { get; set; } = new List<QueuedTrackValue>();

    public QueueEntity() { }

    private QueueEntity(ulong guildId)
    {
        GuildId = guildId;
    }

    public static QueueEntity Create(ulong guildId) => new(guildId);

    public QueuedTrackValue? GetPlayingTrack()
    {
        var track = Tracks.SingleOrDefault(x => x.Playing);

        return track;
    }

    public QueuedTrackValue? GetNextTrack()
    {
        var track = Tracks.FirstOrDefault(x => !x.Played && !x.Playing);

        return track;
    }

    public QueuedTrackValue? GetPausedTrack()
    {
        var track = Tracks.SingleOrDefault(x => x.Paused);

        return track;
    }

    public void AddTrack(QueuedTrackValue track)
    {
        if (track is null) throw new ArgumentNullException(nameof(track));

        var isExistingTrack = Tracks.Any(x => x.Identifier?.Equals(track.Identifier) ?? false);

        if (isExistingTrack && track.Playing)
        {
            Parallel.ForEach(Tracks, x => x.Stop());
            var existingTrack = Tracks.SingleOrDefault(x => x.Identifier?.Equals(track.Identifier) ?? false);
            existingTrack?.Play();
            return;
        }

        if (track.Playing)
        {
            Parallel.ForEach(Tracks, x => x.Stop());
        }
        
        Tracks.Add(track);

        AuditHistory();
    }

    public void RemoveTrack(string trackId)
    {
        if (trackId is null) throw new ArgumentNullException(nameof(trackId));
        if (string.IsNullOrWhiteSpace(trackId))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(trackId));

        var track = Tracks.SingleOrDefault(x => x.Identifier?.Equals(trackId) ?? false);

        if (track is null) return;
        
        Tracks.Remove(track);
    }

    public void TrackEnded(string trackId)
    {
        if (string.IsNullOrWhiteSpace(trackId))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(trackId));

        var track = Tracks.SingleOrDefault(x => x.Identifier?.Equals(trackId) ?? false);

        track?.Ended();
        AuditHistory();
    }

    private void AuditHistory()
    {
        var tracks = Tracks.Where(x => x.Played).ToList();

        if (!tracks.Any()) return;

        if (tracks.Count > 15)
        {
            tracks.RemoveLast(tracks.Count - 15);
        }
    }
}