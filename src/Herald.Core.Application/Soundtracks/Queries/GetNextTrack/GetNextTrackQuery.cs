using AutoMapper;
using Herald.Core.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Herald.Core.Application.Soundtracks.Queries.GetNextTrack;

public record GetNextTrackQuery(ulong GuildId) : IRequest<QueuedTrack?>;

public class GetNextTrackQueryHandler : IRequestHandler<GetNextTrackQuery, QueuedTrack?>
{
    private readonly IHeraldDbContext _context;
    private readonly IMapper _mapper;

    public GetNextTrackQueryHandler(IHeraldDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<QueuedTrack?> Handle(GetNextTrackQuery request, CancellationToken cancellationToken)
    {
        // var filter = Builders<QueueEntity>.Filter
        //     .Eq(x => x.GuildId, request.GuildId);

        var queue = await _context.Queues.SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);

        if (queue is null || !queue.Tracks.Any()) return null;

        return _mapper.Map<QueuedTrack?>(queue.GetNextTrack());
    }
}

