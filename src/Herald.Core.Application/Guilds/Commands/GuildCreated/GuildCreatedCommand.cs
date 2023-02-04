using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Guilds;
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
        var guild = await _context.Guilds
            .SingleOrDefaultAsync(x => x.Id.Equals(request.GuildId), cancellationToken);

        if (guild is null)
        {
            guild = new Guild(request.GuildId, request.OwnerId, _dateTime.UtcNow);
            _ = _context.Guilds.Add(guild);
        }

        guild.JoinedServer(request.OwnerId);

        _ = await _context.SaveChangesAsync(cancellationToken);

        return guild.Id;
    }
}