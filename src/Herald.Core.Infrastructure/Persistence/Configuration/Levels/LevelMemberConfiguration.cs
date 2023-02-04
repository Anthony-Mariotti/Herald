using Herald.Core.Domain.Entities.Leveling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Levels;
public class LevelMemberConfiguration : IEntityTypeConfiguration<LevelMember>
{
    public void Configure(EntityTypeBuilder<LevelMember> builder)
    {
        builder.ToTable("LevelMembers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.LevelId)
            .IsRequired();

        builder.Property(x => x.MemberId)
            .IsRequired();

        builder.Property(x => x.Level)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(x => x.Xp)
            .HasDefaultValue(0.0)
            .IsRequired();

        builder.HasIndex(x => new { x.LevelId, x.MemberId });
    }
}
