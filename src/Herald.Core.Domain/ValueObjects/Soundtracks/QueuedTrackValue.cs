namespace Herald.Core.Domain.ValueObjects.Soundtracks;

public class QueuedTrackValue : ValueObject
{
    public string? TrackId { get; set; }
    
    public string? Author { get; set; }
    
    public string? Title { get; set; }
    
    public string? TrackString { get; set; }
    
    public Uri? Uri { get; set; }
    
    public ulong NotifyChannelId { get; set; }
    
    public ulong RequestUserId { get; set; }

    public bool Playing { get; set; } = false;

    public bool Paused { get; set; } = false;

    public bool Played { get; set; } = false;
    
    public QueuedTrackValue() { }

    private QueuedTrackValue(string? trackId, string author, string title, string trackString, Uri uri,
        ulong notifyChannelId,ulong requestUserId, bool playing = false)
    {
        TrackId = trackId;
        Author = author;
        Title = title;
        TrackString = trackString;
        Uri = uri;
        NotifyChannelId = notifyChannelId;
        RequestUserId = requestUserId;
        Playing = playing;
    }

    public static QueuedTrackValue Create(string? identifier, string author, string title, string trackString, Uri uri,
        ulong notifyChannelId, ulong requestUserId, bool playing = false) =>
        new(identifier, author, title, trackString, uri, notifyChannelId, requestUserId, playing);

    public void Pause()
    {
        Playing = false;
        Paused = true;
    }

    public void Play()
    {
        Playing = true;
        Paused = false;
    }

    public void Stop()
    {
        Playing = false;
        Paused = false;
    }

    public void Ended()
    {
        Played = true;
        Paused = false;
        Stop();
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return TrackId;
        yield return Author;
        yield return Title;
        yield return TrackString;
        yield return Uri;
    }
}