using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Soundtracks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.Soundtracks.Commands.PauseTrack;

public record PauseTrackCommand(ulong GuildId) : IRequest;

public class PauseTrackCommandHandler : IRequestHandler<PauseTrackCommand>
{
    private readonly IHeraldDbContext _context;
    private readonly ILogger<PauseTrackCommandHandler> _logger;

    public PauseTrackCommandHandler(IHeraldDbContext context, ILogger<PauseTrackCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(PauseTrackCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {Command}: {@Request}", nameof(PauseTrackCommand), new
        {
            request.GuildId,
        });
        
        var queue = await _context.Queues
            .Include(x => x.Tracks)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);

        if (queue is null) throw new NotFoundException(nameof(QueueEntity), request.GuildId);

        queue.GetPlayingTrack()?.Pause();

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}