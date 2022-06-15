namespace Herald.Core.Domain.ValueObjects.Soundtracks;

public class Soundtrack : ValueObject
{
    public string? Identifier { get; set; }
    
    public string? Author { get; set; }
    
    public string? Title { get; set; }
    
    public string? TrackString { get; set; }
    
    public Uri? Uri { get; set; }
    
    public Soundtrack() {}

    public Soundtrack(string? identifier, string author, string title, string trackString, Uri uri)
    {
        Identifier = identifier;
        Author = author;
        Title = title;
        TrackString = trackString;
        Uri = uri;
    }

    public static Soundtrack Create(string? identifier, string author, string title, string trackString, Uri uri)
        => new Soundtrack(identifier, author, title, trackString, uri);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Identifier;
        yield return Author;
        yield return Title;
        yield return TrackString;
        yield return Uri;
    }
}