using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("WarehouseId", Name = "IX_WarehouseItem01")]
[Index("ItemId", Name = "IX_WarehouseItem02")]
public partial class WarehouseItem
{
    private WarehouseItem()
    {
        OrderWarehouseItem = new HashSet<OrderWarehouseItem>();
        OrderWarehouseItemOutOfStock = new HashSet<OrderWarehouseItemOutOfStock>();
        PurchaseOrderWarehouseItem = new HashSet<PurchaseOrderWarehouseItem>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    public Guid WarehouseId { get; private set; }

    public Guid ItemId { get; private set; }

    public int Quantity { get; private set; }

    [Column(TypeName = "decimal(15, 3)")]
    public decimal? Price { get; private set; }

    [ForeignKey("ItemId")]
    [InverseProperty("WarehouseItem")]
    public virtual Item? Item { get; private set; }

    [ForeignKey("WarehouseId")]
    [InverseProperty("WarehouseItem")]
    public virtual Warehouse? Warehouse { get; private set; }

    [InverseProperty("WarehouseItem")]
    public virtual ICollection<OrderWarehouseItem> OrderWarehouseItem { get; private set; }

    [InverseProperty("WarehouseItem")]
    public virtual ICollection<OrderWarehouseItemOutOfStock> OrderWarehouseItemOutOfStock { get; private set; }

    [InverseProperty("WarehouseItem")]
    public virtual ICollection<PurchaseOrderWarehouseItem> PurchaseOrderWarehouseItem { get; private set; }
}
