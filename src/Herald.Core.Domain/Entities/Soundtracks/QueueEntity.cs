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

        bool ExistingTrack(QueuedTrackValue input) =>
            input.Identifier.Equals(track.Identifier);
        
        if (Tracks.Any(ExistingTrack))
        {
            Tracks.Single(ExistingTrack).Play();
            AuditHistory();
            return;
        }

        Tracks.Add(track);
        AuditHistory();
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

    public void TrackEnded(string trackId)
    {
        if (string.IsNullOrWhiteSpace(trackId))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(trackId));

        var track = Tracks.SingleOrDefault(x =>
            x.Identifier.Equals(trackId) && 
            x.Status.Equals(TrackStatus.Playing));

        track?.Ended();
        AuditHistory();
    }

    private void AuditHistory()
    {
        bool PlayedFunc(QueuedTrackValue x) => x.Status.Equals(TrackStatus.Played);

        var playedCount = Tracks.Count(PlayedFunc);
        if (playedCount <= 15) return;
        
        var tracks = Tracks.Where(PlayedFunc).Take(15).ToList();
        foreach (var track in tracks)
        {
            Tracks.Remove(track);
        }
    }
}