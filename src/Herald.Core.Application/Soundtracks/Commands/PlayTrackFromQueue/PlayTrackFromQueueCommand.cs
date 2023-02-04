using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Soundtracks.Commands.PlayTrackFromQueue;

public record PlayTrackFromQueueCommand : IRequest
{
    public ulong GuildId { get; init; }
    
    public string TrackIdentifier { get; init; } = default!;
}

public class PlayTrackFromQueueCommandHandler : IRequestHandler<PlayTrackFromQueueCommand>
{
    private readonly IHeraldDbContext _context;

    public PlayTrackFromQueueCommandHandler(IHeraldDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(PlayTrackFromQueueCommand request, CancellationToken cancellationToken)
    {
        var queue = await _context.Queues
            .Include(x => x.Tracks)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
                cancellationToken);

        if (queue is null)
        {
            throw new NotFoundException(nameof(QueueEntity), request.GuildId);
        }

        queue.PlayTrack(request.TrackIdentifier);

        _ = await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}