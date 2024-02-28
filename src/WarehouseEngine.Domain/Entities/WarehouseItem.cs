using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("WarehouseId", Name = "IX_WarehouseItem01")]
[Index("ItemId", Name = "IX_WarehouseItem02")]
public partial class WarehouseItem
{
    public WarehouseItem()
    {
        OrderWarehouseItem = new HashSet<OrderWarehouseItem>();
        OrderWarehouseItemOutOfStock = new HashSet<OrderWarehouseItemOutOfStock>();
        PurchaseOrderWarehouseItem = new HashSet<PurchaseOrderWarehouseItem>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    public required Guid WarehouseId { get; set; }

    public required Guid ItemId { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(15, 3)")]
    public decimal? Price { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("WarehouseItem")]
    public virtual Item? Item { get; set; }

    [ForeignKey("WarehouseId")]
    [InverseProperty("WarehouseItem")]
    public virtual Warehouse? Warehouse { get; set; }

    [InverseProperty("WarehouseItem")]
    public virtual ICollection<OrderWarehouseItem> OrderWarehouseItem { get; init; }
    [InverseProperty("WarehouseItem")]
    public virtual ICollection<OrderWarehouseItemOutOfStock> OrderWarehouseItemOutOfStock { get; init; }
    [InverseProperty("WarehouseItem")]
    public virtual ICollection<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; init; }
}
