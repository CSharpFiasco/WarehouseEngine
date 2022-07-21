namespace WarehouseEngine.Core.Entities;

public partial class WarehouseItem
{
    public WarehouseItem()
    {
        OrderWarehouseItem = new HashSet<OrderWarehouseItem>();
        PurchaseOrderWarehouseItem = new HashSet<PurchaseOrderWarehouseItem>();
        Order = new HashSet<Order>();
    }

    public int Id { get; set; }
    public int WarehouseId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal? Price { get; set; }

    public virtual Item Item { get; set; } = null!;
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual ICollection<OrderWarehouseItem> OrderWarehouseItem { get; set; }
    public virtual ICollection<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; set; }

    public virtual ICollection<Order> Order { get; set; }
}
