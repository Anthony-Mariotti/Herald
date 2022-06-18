using DSharpPlus.Lavalink;
using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Soundtracks;
using Herald.Core.Domain.Events.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.Soundtracks.Commands.QueueTrack;

public record QueueTrackCommand : IRequest
{
    public ulong GuildId { get; init; }
    
    public ulong NotifyChannelId { get; init; }
    
    public ulong RequestUserId { get; init; }

    // TODO: This is a hard dependency on Lavalink, change to be the actual soundtrack for more clear separation.
    public LavalinkTrack Track { get; init; } = default!;
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
        _logger.LogDebug("Handling {Command}: {@Request}", nameof(QueueTrackCommand), new
        {
            request.GuildId,
            request.NotifyChannelId,
            TrackId = request.Track.Identifier
        });
        
        // var filter = Builders<QueueEntity>.Filter
        //     .Eq(x => x.GuildId, request.GuildId);

        var queue = await _context.Queues
            .Include(x => x.Tracks)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);

        var track = Domain.ValueObjects.Soundtracks.QueuedTrackValue.Create(request.Track.Identifier, request.Track.Author, request.Track.Title,
            request.Track.TrackString, request.Track.Uri, request.NotifyChannelId, request.RequestUserId);
        
        if (queue is null)
        {
            queue = QueueEntity.Create(request.GuildId);
            
            queue.AddTrack(track);
            queue.AddDomainEvent(new TrackQueuedEvent(request.GuildId, track));

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        
        queue.AddTrack(track);
        queue.AddDomainEvent(new TrackQueuedEvent(request.GuildId, track));

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}
