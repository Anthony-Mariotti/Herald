using Herald.Core.Domain.Entities.Soundtracks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configurations.Soundtracks;

public class QueueConfiguration : IEntityTypeConfiguration<QueueEntity>
{
    public void Configure(EntityTypeBuilder<QueueEntity> builder)
    {
        builder.ToTable("Queues");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .UseHiLo("QueuesSeq");

        builder.Property("GuildId")
            .IsRequired();
        
        builder.HasMany(x => x.Tracks)
            .WithOne()
            .HasForeignKey("QueueId");

        builder.Ignore(p => p.DomainEvents);
    }
}