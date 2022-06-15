using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using MediatR;
using MongoDB.Driver;

namespace Herald.Core.Application.Soundtracks.Commands.PlayNextTrack;

public class PlayNextTrackCommand : IRequest
{
    public ulong GuildId { get; init; }

    public string TrackIdentifier { get; init; } = default!;
}

public class PlayNextTrackCommandHandler : IRequestHandler<PlayNextTrackCommand>
{
    private readonly IHeraldDbContext _context;

    public PlayNextTrackCommandHandler(IHeraldDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(PlayNextTrackCommand request, CancellationToken cancellationToken)
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

        queue.RemoveFromQueue(request.TrackIdentifier);

        await _context.SoundtrackQueues.ReplaceOneAsync(filter, queue, cancellationToken: cancellationToken);
        
        return Unit.Value;
    }
}