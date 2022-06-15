using AutoMapper;
using Herald.Core.Application.Common.Mappings;
using Herald.Core.Domain.ValueObjects.Soundtracks;

namespace Herald.Core.Application.Soundtracks.Queries.GetQueue;

public class QueuedTrack : IMapFrom<Soundtrack>
{
    public string? Identifier { get; set; }
    
    public string? Author { get; set; }
    
    public string? Title { get; set; }
    
    public string? TrackString { get; set; }
    
    public Uri? Uri { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Soundtrack, QueuedTrack>();
    }
}