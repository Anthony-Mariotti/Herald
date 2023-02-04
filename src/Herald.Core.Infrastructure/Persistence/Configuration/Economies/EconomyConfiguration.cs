using Herald.Core.Domain.Entities.Economies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Economies;
public class EconomyConfiguration : IEntityTypeConfiguration<Economy>
{
    public void Configure(EntityTypeBuilder<Economy> builder)
    {
        builder.ToTable("Economy");
        builder.HasKey(e => e.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.GuildId);

        builder.Property(x => x.Name)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ChannelRestriction)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasMany(x => x.Channels)
            .WithOne()
            .HasForeignKey(x => x.EconomyId);

    }
}
