using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using Herald.Core.Domain.Events.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.Soundtracks.Commands.TrackEnded;

public record TrackEndedCommand : IRequest
{
    public ulong GuildId { get; init; }

    public string TrackId { get; init; } = default!;
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
        _logger.LogDebug("Handling {Command}: {@Request}", nameof(TrackEndedCommand), new
        {
            request.GuildId, request.TrackId
        });

        // var filter = Builders<QueueEntity>.Filter
        //     .Eq(x => x.GuildId, request.GuildId);

        var queue = await _context.Queues.SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);

        if (queue is null)
            throw new NotFoundException(nameof(QueueEntity), request.GuildId);

        queue.TrackEnded(request.TrackId);
        queue.AddDomainEvent(new TrackEndedEvent(request.GuildId, request.TrackId));

        return Unit.Value;
    }
}