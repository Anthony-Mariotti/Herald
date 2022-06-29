using Herald.Core.Domain.Enums;
using Lavalink4NET.Player;

namespace Herald.Core.Domain.ValueObjects.Soundtracks;

public class QueuedTrackValue : ValueObject
{
    public string Identifier { get; set; }
    
    public TrackStatus Status { get; set; }
    
    public string Author { get; set; }
    
    public string Title { get; set; }
    
    public TimeSpan Duration { get; set; }

    public bool Livestream { get; set; }
    
    public bool Seekable { get; set; }
    
    public string Provider { get; set; }

    public ulong NotifyChannelId { get; set; }
    
    public ulong RequestUserId { get; set; }
    
    public string Source { get; set; }
    
    public string Encoded { get; set; }

    private QueuedTrackValue(string identifier, string author, string title, TimeSpan duration, bool livestream,
        bool seekable, string provider, ulong notifyChannelId, ulong requestUserId, string source, string encoded,
        TrackStatus status)
    {
        Identifier = identifier;
        Author = author;
        Title = title;
        Duration = duration;
        Livestream = livestream;
        Seekable = seekable;
        Provider = provider;
        NotifyChannelId = notifyChannelId;
        RequestUserId = requestUserId;
        Source = source;
        Encoded = encoded;
        Status = status;
    }
    
    public static QueuedTrackValue Create(string identifier, string author, string title, TimeSpan duration,
        bool livestream, bool seekable, string provider, ulong notifyChannelId, ulong requestUserId, string source,
        string encoded, TrackStatus status) =>
        new(identifier, author, title, duration, livestream, seekable, provider, notifyChannelId, requestUserId,
            source, encoded, status);

    /// <summary>
    /// Conversion method from <see cref="LavalinkTrack"/> to <see cref="QueuedTrackValue"/>
    /// </summary>
    /// <param name="track">The <see cref="LavalinkTrack"/> to be converted.</param>
    /// <param name="notifyChannelId">The channel where to reply when the track is played.</param>
    /// <param name="requestUserId">The user who requested the track.</param>
    /// <param name="status">The <see cref="TrackStatus"/> of the track that is being created.</param>
    /// <returns>Populated <see cref="QueuedTrackValue"/></returns>
    public static QueuedTrackValue Create(LavalinkTrack track, ulong notifyChannelId, ulong requestUserId,
        TrackStatus status)
        => Create(track.TrackIdentifier, track.Author, track.Title, track.Duration, track.IsLiveStream,
            track.IsSeekable, track.Provider.ToString(), notifyChannelId, requestUserId, track.Source!,
            track.Identifier, status);

    public void Pause() => Status = TrackStatus.Paused;

    public void Play() => Status = TrackStatus.Playing;

    public void Ended() => Status = TrackStatus.Played;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Identifier;
        yield return Author;
        yield return Title;
        yield return Duration;
        yield return Livestream;
        yield return Seekable;
        yield return Provider;
        yield return Source;
        yield return Encoded;
        yield return Status;
    }
}