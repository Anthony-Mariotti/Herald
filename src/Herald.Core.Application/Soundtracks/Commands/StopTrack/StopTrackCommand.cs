using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Soundtracks.Commands.StopTrack;

public record StopTrackCommand(ulong GuildId) : IRequest;

public class StopTrackCommandHandler : IRequestHandler<StopTrackCommand>
{
    private readonly IHeraldDbContext _context;

    public StopTrackCommandHandler(IHeraldDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(StopTrackCommand request, CancellationToken cancellationToken)
    {
        var queue = await _context.Queues
            .Include(x => x.Tracks)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId), cancellationToken);

        if (queue is null)
            throw new NotFoundException(nameof(QueueEntity), request.GuildId);

        queue.GetPlayingTrack()?.Ended();

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}