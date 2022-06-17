// using AutoMapper;
// using Herald.Core.Application.Common.Mappings;
// using Herald.Core.Domain.Entities.Soundtracks;
//
// namespace Herald.Core.Application.Soundtracks.Queries;
//
// public class TrackQueue : IMapFrom<QueueEntity>
// {
//     public IEnumerable<QueuedTrack> Tracks { get; set; } = new List<QueuedTrack>();
//
//     public void Mapping(Profile profile)
//     {
//         profile.CreateMap<QueueEntity, TrackQueue>()
//             .ForMember(d => d.Tracks, opt => opt.MapFrom(s => s.Tracks));
//     }
// }