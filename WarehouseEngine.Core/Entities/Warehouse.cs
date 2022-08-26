namespace WarehouseEngine.Core.Entities;

public partial class Warehouse
{
    public Warehouse()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
    }

    public int Id { get; set; }
    public required string Name { get; set; }

    public virtual ICollection<WarehouseItem> WarehouseItem { get; set; }
}
