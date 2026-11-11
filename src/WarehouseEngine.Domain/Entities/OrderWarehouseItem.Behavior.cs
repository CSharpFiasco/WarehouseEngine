namespace WarehouseEngine.Domain.Entities;

public partial class OrderWarehouseItem
{
    public static OrderWarehouseItem Create(Guid orderId, Guid warehouseItemId, int quantity)
    {
        EntityRules.Identity(orderId, nameof(orderId));
        EntityRules.Identity(warehouseItemId, nameof(warehouseItemId));
        EntityRules.LineQuantity(quantity);
        return new OrderWarehouseItem { OrderId = orderId, WarehouseItemId = warehouseItemId, Quantity = quantity };
    }

    public void ChangeQuantity(int quantity)
    {
        EntityRules.LineQuantity(quantity);
        Quantity = quantity;
    }
}
