using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Soundtracks;
using Herald.Core.Domain.Events.Soundtracks;
using Herald.Core.Domain.ValueObjects.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.Soundtracks.Commands.QueueTrack;

public record QueueTrackCommand : IRequest
{
    public ulong GuildId { get; init; }

    // TODO: This is a hard dependency on Lavalink, change to be the actual soundtrack for more clear separation.
    public QueuedTrackValue Track { get; init; } = default!;
}

public class QueueTrackCommandHandler : IRequestHandler<QueueTrackCommand>
{
    private readonly IHeraldDbContext _context;
    private readonly ILogger<QueueTrackCommandHandler> _logger;

    public QueueTrackCommandHandler(IHeraldDbContext context, ILogger<QueueTrackCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(QueueTrackCommand request, CancellationToken cancellationToken)
    {
        var queue = await _context.Queues
            .Include(x => x.Tracks)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);
        
        if (queue is null)
        {
            _logger.LogTrace("Creating queue for {Guild}", request.GuildId);
            queue = QueueEntity.Create(request.GuildId);
            
            _logger.LogTrace("Adding track to queue for {GuildId}", request.GuildId);
            queue.AddTrack(request.Track);
            queue.AddDomainEvent(new TrackQueuedEvent(request.GuildId, request.Track));

            _ = await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        
        _logger.LogTrace("Adding track to queue for {GuildId}", request.GuildId);
        
        queue.AddTrack(request.Track);
        queue.AddDomainEvent(new TrackQueuedEvent(request.GuildId, request.Track));

        _ = await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
