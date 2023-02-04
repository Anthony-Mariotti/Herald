using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Guilds;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Guilds.Commands.GuildAvailable;

public record GuildAvailableCommand(ulong GuildId, ulong OwnerId) : IRequest;

public class GuildAvailableCommandHandler : IRequestHandler<GuildAvailableCommand>
{
    private readonly IHeraldDbContext _context;
    private readonly IDateTime _dateTime;

    public GuildAvailableCommandHandler(IHeraldDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }
    
    public async Task<Unit> Handle(GuildAvailableCommand request, CancellationToken cancellationToken)
    {
        var guild = await _context.Guilds
            .SingleOrDefaultAsync(x => x.Id.Equals(request.GuildId), cancellationToken);

        if (guild is null)
        {
            guild = new Guild(request.GuildId, request.OwnerId, _dateTime.UtcNow);
            _ = _context.Guilds.Add(guild);
        }

        guild.JoinedServer(request.OwnerId);
            
        _ = await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}