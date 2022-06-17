using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Guilds.Commands.GuildDeleted;

public record GuildDeletedCommand(ulong GuildId) : IRequest;

public class GuildDeletedCommandHandler : IRequestHandler<GuildDeletedCommand>
{
    private readonly IHeraldDbContext _context;
    private readonly IDateTime _dateTime;

    public GuildDeletedCommandHandler(IHeraldDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }
    
    public async Task<Unit> Handle(GuildDeletedCommand request, CancellationToken cancellationToken)
    {
        // var filter = Builders<GuildEntity>.Filter
        //     .Where(x => x.GuildId.Equals(request.guildId));

        var guild = await _context.Guilds.SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId),
            cancellationToken);

        if (guild is null)
            throw new NotFoundException(nameof(GuildEntity), request.GuildId);

        guild.LeftServer(_dateTime.UtcNow);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}