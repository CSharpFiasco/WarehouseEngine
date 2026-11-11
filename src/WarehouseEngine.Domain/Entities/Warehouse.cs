using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseEngine.Domain.Entities;

public partial class Warehouse
{
    private Warehouse()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    [StringLength(MaxNameLength)]
    public string Name { get; private set; } = null!;

    [InverseProperty("Warehouse")]
    public virtual ICollection<WarehouseItem> WarehouseItem { get; init; }
}
