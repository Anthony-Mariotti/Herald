using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Events.Guilds;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Guilds.Commands.GuildCreated;

public record GuildCreatedCommand : IRequest<ulong>
{
    public ulong GuildId { get; init; }
    
    public ulong OwnerId { get; init; }
}

public class GuildCreatedCommandHandler : IRequestHandler<GuildCreatedCommand, ulong>
{
    private readonly IHeraldDbContext _context;
    private readonly IDateTime _dateTime;

    public GuildCreatedCommandHandler(IHeraldDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }
    
    public async Task<ulong> Handle(GuildCreatedCommand request, CancellationToken cancellationToken)
    {
        var guild = await _context.Guilds.SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);

        if (guild is not null)
        {
            guild.JoinedServer(request.OwnerId);
            guild.AddDomainEvent(new GuildRejoinedEvent(request.GuildId, request.OwnerId));
            
            await _context.SaveChangesAsync(cancellationToken);
            return guild.GuildId;
        }
        
        guild = GuildEntity.Create(request.GuildId, request.OwnerId, _dateTime.UtcNow);
        guild.AddDomainEvent(new GuildCreatedEvent(request.GuildId, request.OwnerId));
        
        await _context.Guilds.AddAsync(guild, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return guild.GuildId;
    }
}