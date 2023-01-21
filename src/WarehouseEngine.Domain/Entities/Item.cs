using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

public partial class Item
{
    public Item()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
        Vendor = new HashSet<Vendor>();
    }

    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(12)]
    [Unicode(false)]
    public required string Sku { get; set; }
    [Required]
    [StringLength(120)]
    [Unicode(false)]
    public required string Description { get; set; }
    public bool IsActive { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<WarehouseItem> WarehouseItem { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("Item")]
    public virtual ICollection<Vendor> Vendor { get; set; }
}
