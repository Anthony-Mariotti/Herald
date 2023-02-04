namespace Herald.Core.Domain.Entities.Guilds.Events;

internal class GuildCatalogItemAddedEvent : BaseEvent
{
    public ulong GuildId { get; }
    public string Name { get; }
    public double Price { get; }
    public int? Quantity { get; }

    public GuildCatalogItemAddedEvent(ulong guildId, string name, double price, int? quantity = null)
    {
        GuildId = guildId;
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}