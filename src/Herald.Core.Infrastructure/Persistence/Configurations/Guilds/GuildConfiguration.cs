using Herald.Core.Domain.Entities.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configurations.Guilds;

public class GuildConfiguration : IEntityTypeConfiguration<GuildEntity>
{
    public void Configure(EntityTypeBuilder<GuildEntity> builder)
    {
        builder.ToTable("Guilds");
        builder.HasKey(p => p.GuildId);
        builder.Property(p => p.GuildId)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(p => p.OwnerId)
            .IsRequired();

        builder.Property(p => p.Joined)
            .IsRequired();

        builder.HasMany(p => p.Modules)
            .WithOne()
            .HasForeignKey("GuildId")
            .IsRequired();
        
        builder.Property(p => p.JoinedOn)
            .IsRequired();
        
        builder.Property(p => p.LeftOn);

        builder.Ignore(p => p.DomainEvents);
    }
}