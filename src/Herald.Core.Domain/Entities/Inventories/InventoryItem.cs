namespace Herald.Core.Domain.Entities.Inventories;

public class InventoryItem : BaseDomainEntity, IAggregateRoot
{
    public long MemberId { get; set; }

    public long ItemId { get; set; }

    public int? Quantity { get; set; }

    public InventoryItem(long memberId, long itemId, int? quantity = null)
    {
        MemberId = memberId;
        ItemId = itemId;

        Quantity = quantity;
    }
}
