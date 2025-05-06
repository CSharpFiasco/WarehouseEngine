using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseEngine.Domain.Entities;

public partial class Warehouse
{
    public Warehouse()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    [StringLength(32)]
    public required string Name { get; set; }

    [InverseProperty("Warehouse")]
    public virtual ICollection<WarehouseItem> WarehouseItem { get; init; }
}
