﻿using Herald.Core.Domain.Enums;
using Herald.Core.Domain.ValueObjects.Soundtracks;

namespace Herald.Core.Domain.Entities.Soundtracks;

public sealed class QueueEntity : BaseDomainEntity, IAggregateRoot
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
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(trackIdentifier));
        }

        static bool IsQueuedTrack(QueuedTrackValue track, string identifier)
        {
            return track.Status.Equals(TrackStatus.Queued) && track.Identifier.Equals(identifier);
        }

        if (!Tracks.Any(x => IsQueuedTrack(x, trackIdentifier)))
        {
            return;
        }

        var track = Tracks.FirstOrDefault(x => IsQueuedTrack(x, trackIdentifier));
        track?.Play(TrackStatusReason.FromQueue);
    }
    
    public void AddTrack(QueuedTrackValue track)
    {
        if (track is null)
        {
            throw new ArgumentNullException(nameof(track));
        }

        if (track.Status.Equals(TrackStatus.Playing))
        {
            foreach (var oldTrack in Tracks.Where(x => x.Status.Equals(TrackStatus.Playing)))
            {
                oldTrack.Ended(TrackStatusReason.TrackCleanUp);
            }
        }
        
        Tracks.Add(track);
    }

    public void RemoveTrack(string trackId)
    {
        if (trackId is null)
        {
            throw new ArgumentNullException(nameof(trackId));
        }

        if (string.IsNullOrWhiteSpace(trackId))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(trackId));
        }

        var track = Tracks.SingleOrDefault(x => x.Identifier.Equals(trackId));

        if (track is null)
        {
            return;
        }

        _ = Tracks.Remove(track);
    }

    public void TrackEnded(string identifier, TrackStatusReason reason)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(identifier));
        }

        var track = Tracks.SingleOrDefault(x =>
            x.Identifier.Equals(identifier) && 
            x.Status.Equals(TrackStatus.Playing));

        track?.Ended(reason);
        AuditHistory();
    }

    private void AuditHistory()
    {
        const int maxCount = 15;

        static bool IsPlayed(QueuedTrackValue x)
        {
            return x.Status.Equals(TrackStatus.Played) ||
            x.Status.Equals(TrackStatus.Failed) ||
            x.Status.Equals(TrackStatus.Skipped);
        }

        var playedCount = Tracks.Count(IsPlayed);
        if (playedCount <= maxCount)
        {
            return;
        }

        var removeCount = playedCount - maxCount;
        var tracks = Tracks.Where(IsPlayed).Take(removeCount).ToList();
        foreach (var track in tracks)
        {
            _ = Tracks.Remove(track);
        }
    }
}