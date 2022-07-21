

namespace WarehouseEngine.Core.Entities;

public partial class Item
{
    public Item()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
        Vendor = new HashSet<Vendor>();
    }

    public int Id { get; set; }
    public string Sku { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool IsActive { get; set; }

    public virtual ICollection<WarehouseItem> WarehouseItem { get; set; }

    public virtual ICollection<Vendor> Vendor { get; set; }
}
