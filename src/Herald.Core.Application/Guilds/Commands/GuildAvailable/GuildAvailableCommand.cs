using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Events.Guilds;
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
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId), cancellationToken);

        if (guild is not null)
        {
            if (guild.Joined) return Unit.Value;
            
            guild.JoinedServer(request.OwnerId);

            guild.AddDomainEvent(new GuildRejoinedEvent(request.GuildId, request.OwnerId));
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        
        guild = GuildEntity.Create(request.GuildId, request.OwnerId, _dateTime.UtcNow);

        _context.Guilds.Add(guild);
            
        guild.AddDomainEvent(new GuildCreatedEvent(request.GuildId, request.OwnerId));
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}