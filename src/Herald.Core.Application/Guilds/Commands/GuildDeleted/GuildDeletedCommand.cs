using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using MediatR;
using MongoDB.Driver;

namespace Herald.Core.Application.Guilds.Commands.GuildDeleted;

public record GuildDeletedCommand(ulong guildId) : IRequest;

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
        var filter = Builders<GuildEntity>.Filter
            .Where(x => x.GuildId.Equals(request.guildId));

        var entity = await _context.Guilds.Find(filter)
            .Limit(1)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            throw new NotFoundException(nameof(GuildEntity), request.guildId);
        }

        var update = Builders<GuildEntity>.Update
            .Set(x => x.Joined, false)
            .Set(x => x.LeftOn, _dateTime.UtcNow);

        await _context.Guilds.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        
        return Unit.Value;
    }
}