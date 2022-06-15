using AutoMapper;
using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using MediatR;
using MongoDB.Driver;

namespace Herald.Core.Application.Soundtracks.Queries.GetQueue;

public record GetQueueQuery(ulong GuildId) : IRequest<TrackQueue>;

public class GetQueueQueryHandler : IRequestHandler<GetQueueQuery, TrackQueue>
{
    private readonly IHeraldDbContext _context;
    private readonly IMapper _mapper;

    public GetQueueQueryHandler(IHeraldDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<TrackQueue> Handle(GetQueueQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<SoundtrackQueueEntity>.Filter
            .Eq(x => x.GuildId, request.GuildId);

        var queue = await _context.SoundtrackQueues.Find(filter)
            .Limit(1)
            .SingleOrDefaultAsync(cancellationToken);

        if (queue is null)
        {
            throw new NotFoundException(nameof(SoundtrackQueueEntity), request.GuildId);
        }

        return _mapper.Map<TrackQueue>(queue);
    }
}