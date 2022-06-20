using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Soundtracks.Queries.HasQueuedTrackQuery;

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
            .SingleOrDefaultAsync(cancellationToken);

        if (queue is null)
            throw new NotFoundException(nameof(QueueEntity), request.GuildId);

        return queue.Tracks.Any(x => !x.Played && !x.Playing && !x.Paused);
    }
}