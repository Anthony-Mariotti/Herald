using Herald.Core.Domain.Entities.Economies;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Leveling;
using Herald.Core.Domain.Entities.Members;
using Herald.Core.Domain.Entities.Modules;
using Herald.Core.Domain.Entities.Soundtracks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Herald.Core.Application.Abstractions;

public interface IHeraldDbContext
{
    public DbSet<Guild> Guilds { get; }
    
    public DbSet<QueueEntity> Queues { get; }

    public DbSet<Level> Levels { get; }

    public DbSet<Member> Members { get; }

    public DbSet<Economy> Economies { get; }

    public DbSet<Module> Modules { get; }

    public DatabaseFacade Database { get; }

    public EntityEntry Attach(object entity);

    public EntityEntry Entry(object entity);

    public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}