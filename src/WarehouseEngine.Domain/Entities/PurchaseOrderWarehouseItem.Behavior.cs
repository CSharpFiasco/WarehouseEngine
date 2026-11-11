namespace WarehouseEngine.Domain.Entities;

public partial class PurchaseOrderWarehouseItem
{
    public static PurchaseOrderWarehouseItem Create(Guid purchaseOrderId, Guid warehouseItemId, int quantity)
    {
        EntityRules.Identity(purchaseOrderId, nameof(purchaseOrderId));
        EntityRules.Identity(warehouseItemId, nameof(warehouseItemId));
        EntityRules.LineQuantity(quantity);
        return new PurchaseOrderWarehouseItem { PurchaseOrderId = purchaseOrderId, WarehouseItemId = warehouseItemId, Quantity = quantity };
    }

    public void ChangeQuantity(int quantity)
    {
        EntityRules.LineQuantity(quantity);
        Quantity = quantity;
    }
}
