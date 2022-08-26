namespace WarehouseEngine.Core.Entities;

public partial class PurchaseOrder
{
    public PurchaseOrder()
    {
        PurchaseOrderWarehouseItem = new HashSet<PurchaseOrderWarehouseItem>();
    }

    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public required string OrderNumber { get; set; }
    public byte Status { get; set; }

    public virtual ICollection<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; set; }
}
