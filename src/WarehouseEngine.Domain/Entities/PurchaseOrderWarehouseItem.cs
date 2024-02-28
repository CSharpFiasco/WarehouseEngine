using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("PurchaseOrderId", Name = "IX_PurchaseOrderWarehouseItem01")]
[Index("WarehouseItemId", Name = "IX_PurchaseOrderWarehouseItem02")]
public partial class PurchaseOrderWarehouseItem
{
    [Key]
    public Guid PurchaseOrderId { get; set; }

    [Key]
    public Guid WarehouseItemId { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("PurchaseOrderWarehouseItem")]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    [ForeignKey("WarehouseItemId")]
    [InverseProperty("PurchaseOrderWarehouseItem")]
    public virtual WarehouseItem? WarehouseItem { get; set; }
}
