using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("OrderId", Name = "IX_OrderWarehouseItemOutOfStock01")]
[Index("WarehouseItemId", Name = "IX_OrderWarehouseItemOutOfStock02")]
public partial class OrderWarehouseItemOutOfStock
{
    private OrderWarehouseItemOutOfStock() { }

    [Key]
    public Guid OrderId { get; private set; }

    [Key]
    public Guid WarehouseItemId { get; private set; }

    public int Quantity { get; private set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderWarehouseItemOutOfStock")]
    public virtual Order? Order { get; private set; }

    [ForeignKey("WarehouseItemId")]
    [InverseProperty("OrderWarehouseItemOutOfStock")]
    public virtual WarehouseItem? WarehouseItem { get; private set; }
}
