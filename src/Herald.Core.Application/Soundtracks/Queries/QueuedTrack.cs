using AutoMapper;
using Herald.Core.Application.Common.Mappings;
using Herald.Core.Domain.ValueObjects.Soundtracks;
using Lavalink4NET.Player;

namespace Herald.Core.Application.Soundtracks.Queries;

public class QueuedTrack : IMapFrom<QueuedTrackValue>
{
    public string Identifier { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public TimeSpan Duration { get; set; }

    public bool Livestream { get; set; }
    
    public bool Seekable { get; set; }
    
    public string Provider { get; set; } = string.Empty;

    public ulong NotifyChannelId { get; set; }
    
    public ulong RequestUserId { get; set; }
    
    public string Source { get; set; } = string.Empty;
    
    public string Encoded { get; set; } = string.Empty;

    public void Mapping(Profile profile) =>
        profile.CreateMap<QueuedTrackValue, QueuedTrack>();

    public LavalinkTrack GetLavalinkTrack() =>
        new LavalinkTrack(Encoded, Author, Duration, Livestream, Seekable, Source, Title, Identifier,
            (StreamProvider)Enum.Parse(typeof(StreamProvider), Provider, true));
}