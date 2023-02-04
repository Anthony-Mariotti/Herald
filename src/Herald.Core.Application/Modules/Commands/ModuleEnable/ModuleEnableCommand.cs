using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Modules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Modules.Commands.ModuleEnable;

public record ModuleEnableCommand : IRequest
{
    public ulong GuildId { get; init; }

    public Module Module { get; init; } = default!;
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
        // var filter = Builders<GuildEntity>.Filter
        //     .Where(x => x.GuildId.Equals(request.GuildId));

        var guild = await _context.Guilds
            .Include(x => x.Modules)
            .SingleOrDefaultAsync(x => x.Id.Equals(request.GuildId),
            cancellationToken);

        if (guild is null)
        {
            throw new NotFoundException(nameof(Guild), request.GuildId);
        }

        guild.EnableModule(request.Module);

        _ = await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}