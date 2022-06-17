using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Soundtracks;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Abstractions;

public interface IHeraldDbContext
{
    public DbSet<GuildEntity> Guilds { get; }
    
    public DbSet<QueueEntity> Queues { get; }

    public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}