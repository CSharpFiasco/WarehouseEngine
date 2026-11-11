using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("PurchaseOrderId", Name = "IX_PurchaseOrderWarehouseItem01")]
[Index("WarehouseItemId", Name = "IX_PurchaseOrderWarehouseItem02")]
public partial class PurchaseOrderWarehouseItem
{
    private PurchaseOrderWarehouseItem() { }

    [Key]
    public Guid PurchaseOrderId { get; private set; }

    [Key]
    public Guid WarehouseItemId { get; private set; }

    public int Quantity { get; private set; }

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("PurchaseOrderWarehouseItem")]
    public virtual PurchaseOrder? PurchaseOrder { get; private set; }

    [ForeignKey("WarehouseItemId")]
    [InverseProperty("PurchaseOrderWarehouseItem")]
    public virtual WarehouseItem? WarehouseItem { get; private set; }
}
