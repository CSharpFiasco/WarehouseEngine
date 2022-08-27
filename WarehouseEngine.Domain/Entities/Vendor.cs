
namespace WarehouseEngine.Domain.Entities;

public partial class Vendor
{
    public Vendor()
    {
        Item = new HashSet<Item>();
    }

    public int Id { get; set; }
    public string? Name { get; set; }

    public virtual ICollection<Item> Item { get; set; }
}
