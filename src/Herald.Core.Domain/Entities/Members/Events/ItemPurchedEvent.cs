namespace Herald.Core.Domain.Entities.Members.Events;

internal class ItemPurchedEvent : BaseEvent
{
    public long MemberId { get; }
    public long ItemId { get; }
    public int? Quantity { get; }

    public ItemPurchedEvent(long memberId, long itemId, int? quantity)
    {
        MemberId = memberId;
        ItemId = itemId;
        Quantity = quantity;
    }
}