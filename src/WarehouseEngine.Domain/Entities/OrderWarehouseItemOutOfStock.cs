using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("OrderId", Name = "IX_OrderWarehouseItemOutOfStock01")]
[Index("WarehouseItemId", Name = "IX_OrderWarehouseItemOutOfStock02")]
public partial class OrderWarehouseItemOutOfStock
{
    [Key]
    public Guid OrderId { get; set; }

    [Key]
    public Guid WarehouseItemId { get; set; }

    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderWarehouseItemOutOfStock")]
    public virtual Order? Order { get; set; }

    [ForeignKey("WarehouseItemId")]
    [InverseProperty("OrderWarehouseItemOutOfStock")]
    public virtual WarehouseItem? WarehouseItem { get; set; }
}
