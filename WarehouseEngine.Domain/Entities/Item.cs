

namespace WarehouseEngine.Domain.Entities;

public partial class Item
{
    public Item()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
        Vendor = new HashSet<Vendor>();
    }

    public int Id { get; set; }
    public required string Sku { get; set; }
    public required string Description { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<WarehouseItem> WarehouseItem { get; set; }

    public virtual ICollection<Vendor> Vendor { get; set; }
}
