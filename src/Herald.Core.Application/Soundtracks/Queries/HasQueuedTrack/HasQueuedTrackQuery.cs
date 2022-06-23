using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Soundtracks.Queries.HasQueuedTrack;

public record HasQueuedTrackQuery(ulong GuildId) : IRequest<bool>;

public class HasQueuedTrackQueryHandler : IRequestHandler<HasQueuedTrackQuery, bool>
{
    private readonly IHeraldDbContext _context;

    public HasQueuedTrackQueryHandler(IHeraldDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(HasQueuedTrackQuery request, CancellationToken cancellationToken)
    {
        var queue = await _context.Queues
            .Include(x => x.Tracks)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId), cancellationToken);

        return queue is not null && 
               queue.Tracks.Any(x =>
                   x.Status.Equals(TrackStatus.Queued));
    }
}