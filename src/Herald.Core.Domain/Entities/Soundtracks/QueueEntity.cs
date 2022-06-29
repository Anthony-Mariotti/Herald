using Herald.Core.Domain.Enums;
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
        var track = Tracks.SingleOrDefault(x => x.Status.Equals(TrackStatus.Playing));
        return track;
    }

    public QueuedTrackValue? GetNextTrack()
    {
        var track = Tracks.FirstOrDefault(x => x.Status.Equals(TrackStatus.Queued));
        return track;
    }

    public QueuedTrackValue? GetPausedTrack()
    {
        var track = Tracks.SingleOrDefault(x =>
            x.Status.Equals(TrackStatus.Paused));
        return track;
    }

    public void PlayTrack(string trackIdentifier)
    {
        if (string.IsNullOrWhiteSpace(trackIdentifier))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(trackIdentifier));

        bool IsQueuedTrack(QueuedTrackValue track, string identifier)
            => track.Status.Equals(TrackStatus.Queued) && track.Identifier.Equals(identifier);

        if (!Tracks.Any(x => IsQueuedTrack(x, trackIdentifier))) return;
        
        var track = Tracks.FirstOrDefault(x => IsQueuedTrack(x, trackIdentifier));
        track?.Play();
    }
    
    public void AddTrack(QueuedTrackValue track)
    {
        if (track is null) throw new ArgumentNullException(nameof(track));

        if (track.Status.Equals(TrackStatus.Playing))
        {
            foreach (var oldTrack in Tracks.Where(x => !x.Status.Equals(TrackStatus.Queued)))
            {
                oldTrack.Ended();
            }
        }
        
        Tracks.Add(track);
    }

    public void RemoveTrack(string trackId)
    {
        if (trackId is null) throw new ArgumentNullException(nameof(trackId));
        if (string.IsNullOrWhiteSpace(trackId))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(trackId));

        var track = Tracks.SingleOrDefault(x => x.Identifier.Equals(trackId));

        if (track is null) return;
        
        Tracks.Remove(track);
    }

    public void TrackEnded(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(identifier));

        var track = Tracks.SingleOrDefault(x =>
            x.Identifier.Equals(identifier) && 
            x.Status.Equals(TrackStatus.Playing));

        track?.Ended();
        AuditHistory();
    }

    private void AuditHistory()
    {
        bool IsPlayed(QueuedTrackValue x) => x.Status.Equals(TrackStatus.Played);

        var playedCount = Tracks.Count(IsPlayed);
        if (playedCount <= 15) return;
        
        var tracks = Tracks.Where(IsPlayed).Take(15).ToList();
        foreach (var track in tracks)
        {
            Tracks.Remove(track);
        }
    }
}