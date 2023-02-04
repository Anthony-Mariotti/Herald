using Herald.Core.Domain.Entities.Leveling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Levels;
public class LevelConfiguration : IEntityTypeConfiguration<Level>
{
    public void Configure(EntityTypeBuilder<Level> builder)
    {
        builder.ToTable("Levels");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.GuildId)
            .IsRequired();

        builder.Property(x => x.ChanncelRestriction)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.RoleRestriction)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasMany(x => x.Members)
            .WithOne()
            .HasForeignKey(x => x.LevelId);

        builder.HasMany(x => x.ResitrctedChannels)
            .WithOne()
            .HasForeignKey(x => x.LevelId);

        builder.HasMany(x => x.RestrictedRoles)
            .WithOne()
            .HasForeignKey(x => x.LevelId);
    }
}
