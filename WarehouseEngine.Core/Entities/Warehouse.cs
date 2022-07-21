namespace WarehouseEngine.Core.Entities;

public partial class Warehouse
{
    public Warehouse()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<WarehouseItem> WarehouseItem { get; set; }
}
