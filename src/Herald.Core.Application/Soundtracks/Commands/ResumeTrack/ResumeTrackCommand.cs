using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Soundtracks.Commands.ResumeTrack;

public record ResumeTrackCommand(ulong GuildId) : IRequest;

public class ResumeTrackCommandHandler : IRequestHandler<ResumeTrackCommand>
{
    private readonly IHeraldDbContext _context;

    public ResumeTrackCommandHandler(IHeraldDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(ResumeTrackCommand request, CancellationToken cancellationToken)
    {
        var queue = await _context.Queues
            .Include(x => x.Tracks)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId), cancellationToken);

        if (queue is null)
            throw new NotFoundException(nameof(QueueEntity), request.GuildId);
        
        queue.GetPausedTrack()?.Play();

        // TODO: Add Track Resumed Domain Event
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}