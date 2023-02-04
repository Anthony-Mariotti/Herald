using Herald.Core.Domain.Entities.Catalog;
using Herald.Core.Domain.Entities.Inventories;
using Herald.Core.Domain.Entities.Members.Events;

namespace Herald.Core.Domain.Entities.Members;

public class Member : BaseDomainEntity
{
    public ulong GuildId { get; set; }

    public ulong MemberId { get; set; }

    public double Balance { get; set; }

    private readonly List<InventoryItem> _items = new List<InventoryItem>();

    public IReadOnlyCollection<InventoryItem> Items => _items.AsReadOnly();

    public void PurchaseItem(CatalogItem item, int? quantity = null)
    {
        if (quantity != null)
        {
            if (Balance < item.Price * quantity)
            {
                throw new Exception("Insufficient balance");
            }

            Balance -= item.Price * (int)quantity;

            if (item.Quantity != null)
            {
                item.Quantity -= quantity;
            }
        }
        else
        {
            if (Balance < item.Price)
            {
                throw new Exception("Insufficient balance");
            }

            Balance -= item.Price;
        }

        var inventoryItem = Items.SingleOrDefault(x => x.ItemId == item.Id);

        if (inventoryItem != null)
        {
            if (quantity == null)
            {
                inventoryItem.Quantity += 1;
            }
            else
            {
                inventoryItem.Quantity += (int)quantity;
            }
        }
        else
        {
            _items.Add(new InventoryItem(Id, item.Id, quantity));
        }

        AddDomainEvent(new ItemPurchedEvent(Id, item.Id, quantity));
    }
}
