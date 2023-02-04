using Herald.Core.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Herald.Core.Infrastructure.Persistence.Configuration.Catalogs;

public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("GuildCatalog");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Price)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired(false);

        builder.Property(x => x.GuildId)
            .IsRequired();
    }
}
