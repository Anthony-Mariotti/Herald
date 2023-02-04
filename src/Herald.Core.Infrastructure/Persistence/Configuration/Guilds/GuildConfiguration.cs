using Herald.Core.Domain.Entities.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Guilds;

public class GuildConfiguration : IEntityTypeConfiguration<Guild>
{
    public void Configure(EntityTypeBuilder<Guild> builder)
    {
        builder.ToTable("Guilds");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(p => p.OwnerId)
            .IsRequired();

        builder.Property(p => p.Joined)
            .IsRequired();

        builder.HasMany(x => x.Modules)
            .WithOne()
            .HasForeignKey(x => x.GuildId);

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.GuildId);

        builder.HasMany(x => x.Members)
            .WithOne()
            .HasForeignKey(x => x.GuildId);

        builder.Property(p => p.JoinedOn)
            .IsRequired();

        builder.Property(p => p.LeftOn);
    }
}