using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Herald.Core.Application.Modules.Commands.ModuleEnable;

public record ModuleEnableCommand : IRequest
{
    public ObjectId ModuleId { get; init; }
    
    public ulong GuildId { get; init; }
}

public class ModuleEnableCommandHandler : IRequestHandler<ModuleEnableCommand>
{
    private readonly IHeraldDbContext _context;

    public ModuleEnableCommandHandler(IHeraldDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(ModuleEnableCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<GuildEntity>.Filter
            .Where(x => x.GuildId.Equals(request.GuildId));

        var guild = await _context.Guilds.Find(filter)
            .Limit(1)
            .SingleOrDefaultAsync(cancellationToken);

        if (guild is null)
        {
            throw new NotFoundException(nameof(GuildEntity), request.GuildId);
        }

        guild.EnableModule(request.ModuleId);

        await _context.Guilds.ReplaceOneAsync(filter, guild, cancellationToken: cancellationToken);
        
        return Unit.Value;
    }
}