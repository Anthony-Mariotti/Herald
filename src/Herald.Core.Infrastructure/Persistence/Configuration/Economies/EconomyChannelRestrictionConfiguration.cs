using Herald.Core.Domain.Entities.Economies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Economies;
public class EconomyChannelRestrictionConfiguration : IEntityTypeConfiguration<EconomyChannel>
{
    public void Configure(EntityTypeBuilder<EconomyChannel> builder)
    {
        builder.ToTable("EconomyChannels");
        builder.HasKey(x => new { x.EconomyId, x.ChannelId });

        builder.Property(x => x.EconomyId)
            .IsRequired();

        builder.Property(x => x.ChannelId)
            .IsRequired();
    }
}
