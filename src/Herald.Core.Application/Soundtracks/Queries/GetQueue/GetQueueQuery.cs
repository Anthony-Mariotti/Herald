// using AutoMapper;
// using Herald.Core.Application.Abstractions;
// using Herald.Core.Application.Exceptions;
// using Herald.Core.Domain.Entities.Soundtracks;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
//
// namespace Herald.Core.Application.Soundtracks.Queries;
//
// public record GetQueueQuery(ulong GuildId) : IRequest<TrackQueue>;
//
// public class GetQueueQueryHandler : IRequestHandler<GetQueueQuery, TrackQueue>
// {
//     private readonly IHeraldDbContext _context;
//     private readonly IMapper _mapper;
//
//     public GetQueueQueryHandler(IHeraldDbContext context, IMapper mapper)
//     {
//         _context = context;
//         _mapper = mapper;
//     }
//     
//     public async Task<TrackQueue> Handle(GetQueueQuery request, CancellationToken cancellationToken)
//     {
//         // var filter = Builders<QueueEntity>.Filter
//         //     .Eq(x => x.GuildId, request.GuildId);
//
//         var queue = await _context.Queues.SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
//             cancellationToken);
//
//         if (queue is null)
//             throw new NotFoundException(nameof(QueueEntity), request.GuildId);
//
//         return _mapper.Map<TrackQueue>(queue);
//     }
// }