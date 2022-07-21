namespace WarehouseEngine.Core.Entities;

public partial class OrderWarehouseItem
{
    public int OrderId { get; set; }
    public int WarehouseItemId { get; set; }
    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;
    public virtual WarehouseItem WarehouseItem { get; set; } = null!;
}
