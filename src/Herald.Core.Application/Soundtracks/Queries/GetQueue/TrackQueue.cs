using AutoMapper;
using Herald.Core.Application.Common.Mappings;
using Herald.Core.Domain.Entities.Soundtracks;

namespace Herald.Core.Application.Soundtracks.Queries.GetQueue;

public class TrackQueue : IMapFrom<SoundtrackQueueEntity>
{
    public ulong NotifyChannelId { get; set; }

    public IEnumerable<QueuedTrack> Tracks { get; set; } = new List<QueuedTrack>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SoundtrackQueueEntity, TrackQueue>()
            .ForMember(d => d.NotifyChannelId, opt => opt.MapFrom(s => s.NotifyChannelId))
            .ForMember(d => d.Tracks, opt => opt.MapFrom(s => s.Tracks));
    }
}