using DSharpPlus.Lavalink;
using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Soundtracks;
using MediatR;
using MongoDB.Driver;

namespace Herald.Core.Application.Soundtracks.Commands.AddTrackToQueue;

public record AddTrackToQueueCommand : IRequest
{
    public ulong GuildId { get; init; }
    
    public ulong NotifyChannelId { get; init; }

    public LavalinkTrack Track { get; init; } = default!;
}

public class AddTrackToQueueCommandHandler : IRequestHandler<AddTrackToQueueCommand>
{
    private readonly IHeraldDbContext _context;

    public AddTrackToQueueCommandHandler(IHeraldDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(AddTrackToQueueCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<SoundtrackQueueEntity>.Filter
            .Eq(x => x.GuildId, request.GuildId);

        var queue = await _context.SoundtrackQueues.Find(filter)
            .Limit(1)
            .SingleOrDefaultAsync(cancellationToken);

        queue.AddToQueue(request.NotifyChannelId, request.Track);

        await _context.SoundtrackQueues.ReplaceOneAsync(filter, queue, cancellationToken: cancellationToken);
        
        return Unit.Value;
    }
}
