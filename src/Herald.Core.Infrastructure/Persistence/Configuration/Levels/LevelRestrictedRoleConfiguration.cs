using Herald.Core.Domain.Entities.Leveling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Levels;

public class LevelRestrictedRoleConfiguration : IEntityTypeConfiguration<RestrictedLevelRole>
{
    public void Configure(EntityTypeBuilder<RestrictedLevelRole> builder)
    {
        builder.ToTable("LevelRestrictedRoles");
        builder.HasKey(e => e.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.LevelId)
            .IsRequired();

        builder.Property(x => x.RoleId)
            .IsRequired();

        builder.HasIndex(x => new { x.LevelId, x.RoleId });
    }
}
