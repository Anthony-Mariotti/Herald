using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using Herald.Core.Domain.Enums;
using Herald.Core.Domain.Events.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.Soundtracks.Commands.TrackEnded;

public record TrackEndedCommand : IRequest
{
    public ulong GuildId { get; init; }

    /// <summary>
    /// Lavalink4NET TrackIdentifier
    /// </summary>
    public string Identifier { get; init; } = default!;

    public TrackStatusReason Reason { get; init; } = default!;
}

public class TrackEndedCommandHandler : IRequestHandler<TrackEndedCommand>
{
    private readonly IHeraldDbContext _context;
    private readonly ILogger<TrackEndedCommandHandler> _logger;

    public TrackEndedCommandHandler(IHeraldDbContext context, ILogger<TrackEndedCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(TrackEndedCommand request, CancellationToken cancellationToken)
    {
        var queue = await _context.Queues
            .Include(x => x.Tracks)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);

        if (queue is null)
        {
            throw new NotFoundException(nameof(QueueEntity), request.GuildId);
        }

        queue.TrackEnded(request.Identifier, request.Reason);
        queue.AddDomainEvent(new TrackEndedEvent(request.GuildId, request.Identifier, request.Reason));

        _ = _context.Queues.Update(queue);
        _ = await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}