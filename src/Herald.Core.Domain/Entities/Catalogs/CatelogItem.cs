namespace Herald.Core.Domain.Entities.Catalog;

public class CatalogItem : BaseDomainEntity
{
    public ulong GuildId { get; set; }

    public string Name { get; set; }

    public double Price { get; set; }

    public int? Quantity { get; set; }

    public CatalogItem()
    {
        Name = string.Empty;
    }

    public CatalogItem(ulong guildId, string name, double price, int? quantity)
    {
        GuildId = guildId;
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}
