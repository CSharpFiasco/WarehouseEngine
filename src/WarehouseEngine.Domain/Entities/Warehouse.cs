using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

public partial class Warehouse
{
    public Warehouse()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
    }

    [Key]
    public Guid Id { get; set; }
    [StringLength(32)]
    [Unicode(false)]
    public required string Name { get; set; }

    [InverseProperty("Warehouse")]
    public virtual ICollection<WarehouseItem> WarehouseItem { get; init; }
}
