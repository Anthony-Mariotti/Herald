using System.Reflection;
using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Soundtracks;
using Herald.Core.Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Infrastructure.Persistence;

public class HeraldDbContext : DbContext, IHeraldDbContext
{
    private readonly ILogger<HeraldDbContext> _logger;
    private readonly IMediator _mediator;

    public HeraldDbContext(ILogger<HeraldDbContext> logger, DbContextOptions<HeraldDbContext> options,
        IMediator mediator) : base(options)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public DbSet<GuildEntity> Guilds => Set<GuildEntity>();

    public DbSet<QueueEntity> Queues => Set<QueueEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Dispatching Domain Events and Saving Changes");
        await _mediator.DispatchDomainEvents(this);
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        SaveChangesAsync(true, cancellationToken);
}