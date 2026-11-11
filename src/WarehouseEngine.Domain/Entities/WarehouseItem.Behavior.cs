namespace WarehouseEngine.Domain.Entities;

public partial class WarehouseItem
{
    public static WarehouseItem Create(Guid id, Guid warehouseId, Guid itemId, int quantity = 0, decimal? price = null)
    {
        EntityRules.Identity(id, nameof(id));
        EntityRules.Identity(warehouseId, nameof(warehouseId));
        EntityRules.Identity(itemId, nameof(itemId));
        EntityRules.Stock(quantity, price);
        return new WarehouseItem { Id = id, WarehouseId = warehouseId, ItemId = itemId, Quantity = quantity, Price = price };
    }

    public void UpdateStock(int quantity, decimal? price)
    {
        EntityRules.Stock(quantity, price);
        Quantity = quantity;
        Price = price;
    }
}
