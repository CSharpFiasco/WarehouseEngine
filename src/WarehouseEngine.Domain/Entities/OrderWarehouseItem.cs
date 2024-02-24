using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("OrderId", Name = "IX_OrderWarehouseItem01")]
[Index("WarehouseItemId", Name = "IX_OrderWarehouseItem02")]
public partial class OrderWarehouseItem
{
    [Key]
    public Guid OrderId { get; set; }
    [Key]
    public Guid WarehouseItemId { get; set; }
    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderWarehouseItem")]
    public virtual Order? Order { get; set; }

    [ForeignKey("WarehouseItemId")]
    [InverseProperty("OrderWarehouseItem")]
    public virtual WarehouseItem? WarehouseItem { get; set; }
}
