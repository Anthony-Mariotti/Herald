using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Soundtracks;
using Herald.Core.Domain.Events.Soundtracks;
using Herald.Core.Domain.ValueObjects.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.Soundtracks.Commands.PlayTrack;

public record PlayTrackCommand : IRequest
{
    public ulong GuildId { get; init; }
    
    public ulong NotifyChannelId { get; init; }
    
    public ulong RequestUserId { get; init; }
    
    // TODO: This is a hard dependency on Lavalink, change to be the actual soundtrack for more clear separation.
    public QueuedTrackValue Track { get; init; } = default!;
}

public class PlayTrackCommandHandler : IRequestHandler<PlayTrackCommand>
{
    private readonly IHeraldDbContext _context;
    private readonly ILogger<PlayTrackCommandHandler> _logger;

    public PlayTrackCommandHandler(IHeraldDbContext context, ILogger<PlayTrackCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(PlayTrackCommand request, CancellationToken cancellationToken)
    {
        // var filter = Builders<QueueEntity>.Filter
        //     .Eq(x => x.GuildId, request.GuildId);

        var queue = await _context.Queues
            .Include(x => x.Tracks)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);

        if (queue is null)
        {
            queue = QueueEntity.Create(request.GuildId);
            
            queue.AddTrack(request.Track);
            queue.AddDomainEvent(new TrackPlayingEvent(request.GuildId, request.Track));

            _context.Queues.Add(queue);
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        
        queue.AddTrack(request.Track);
        queue.AddDomainEvent(new TrackPlayingEvent(request.GuildId, request.Track));

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}