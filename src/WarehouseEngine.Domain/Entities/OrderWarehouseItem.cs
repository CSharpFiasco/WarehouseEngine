using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("OrderId", Name = "IX_OrderWarehouseItem01")]
[Index("WarehouseItemId", Name = "IX_OrderWarehouseItem02")]
public partial class OrderWarehouseItem
{
    private OrderWarehouseItem() { }

    [Key]
    public Guid OrderId { get; private set; }

    [Key]
    public Guid WarehouseItemId { get; private set; }

    public int Quantity { get; private set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderWarehouseItem")]
    public virtual Order? Order { get; private set; }

    [ForeignKey("WarehouseItemId")]
    [InverseProperty("OrderWarehouseItem")]
    public virtual WarehouseItem? WarehouseItem { get; private set; }
}
