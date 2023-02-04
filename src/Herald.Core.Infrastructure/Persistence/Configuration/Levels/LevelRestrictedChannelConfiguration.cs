using Herald.Core.Domain.Entities.Leveling;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Levels;
public class LevelRestrictedChannelConfiguration : IEntityTypeConfiguration<RestrictedLevelChannel>
{
    public void Configure(EntityTypeBuilder<RestrictedLevelChannel> builder)
    {
        builder.ToTable("LevelRestrictedChannels");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.LevelId)
            .IsRequired();

        builder.Property(x => x.ChannelId)
            .IsRequired();

        builder.HasIndex(x => new { x.LevelId, x.ChannelId });
    }
}
