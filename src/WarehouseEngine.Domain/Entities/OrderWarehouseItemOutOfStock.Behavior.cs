namespace WarehouseEngine.Domain.Entities;

public partial class OrderWarehouseItemOutOfStock
{
    public static OrderWarehouseItemOutOfStock Create(Guid orderId, Guid warehouseItemId, int quantity)
    {
        EntityRules.Identity(orderId, nameof(orderId));
        EntityRules.Identity(warehouseItemId, nameof(warehouseItemId));
        EntityRules.LineQuantity(quantity);
        return new OrderWarehouseItemOutOfStock { OrderId = orderId, WarehouseItemId = warehouseItemId, Quantity = quantity };
    }

    public void ChangeQuantity(int quantity)
    {
        EntityRules.LineQuantity(quantity);
        Quantity = quantity;
    }
}
