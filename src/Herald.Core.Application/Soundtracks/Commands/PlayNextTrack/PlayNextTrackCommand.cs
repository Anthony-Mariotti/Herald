using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Soundtracks.Commands.PlayNextTrack;

public record PlayNextTrackCommand : IRequest
{
    public ulong GuildId { get; init; }
    
    public long TrackId { get; init; }
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
        var queue = await _context.Queues.SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);

        if (queue is null)
            throw new NotFoundException(nameof(QueueEntity), request.GuildId);

        var track = queue.GetNextTrack();
        track?.Play();

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}