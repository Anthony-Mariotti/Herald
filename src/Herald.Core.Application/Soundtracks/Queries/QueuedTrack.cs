using AutoMapper;
using Herald.Core.Application.Common.Mappings;
using Herald.Core.Domain.ValueObjects.Soundtracks;

namespace Herald.Core.Application.Soundtracks.Queries;

public class QueuedTrack : IMapFrom<QueuedTrackValue>
{
    public string? Identifier { get; set; }
    
    public string? Author { get; set; }
    
    public string? Title { get; set; }
    
    public string? TrackString { get; set; }
    
    public Uri? Uri { get; set; }
    
    public ulong NotifyChannelId { get; set; }
    
    public ulong RequestUserId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<QueuedTrackValue, QueuedTrack>();
    }
}