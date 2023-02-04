using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Economies;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Leveling;
using Herald.Core.Domain.Entities.Members;
using Herald.Core.Domain.Entities.Modules;
using Herald.Core.Domain.Entities.Soundtracks;
using Herald.Core.Infrastructure.Common.Extensions;
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

    public DbSet<Guild> Guilds => Set<Guild>();

    public DbSet<QueueEntity> Queues => Set<QueueEntity>();

    public DbSet<Economy> Economies => Set<Economy>();

    public DbSet<Level> Levels => Set<Level>();

    public DbSet<Member> Members => Set<Member>();

    public DbSet<Module> Modules => Set<Module>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        _ = builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Dispatching Domain Events");
        await _mediator.DispatchDomainEvents(this);
        
        _logger.LogTrace("Saving Database Changes");
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        SaveChangesAsync(true, cancellationToken);
}