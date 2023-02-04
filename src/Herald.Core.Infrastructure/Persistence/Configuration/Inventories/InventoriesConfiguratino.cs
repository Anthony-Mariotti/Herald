using Herald.Core.Domain.Entities.Inventories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Inventories;
public class InventoriesConfiguratino : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("MemberInventories");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.MemberId)
            .IsRequired();

        builder.Property(x => x.ItemId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired(false);
    }
}
