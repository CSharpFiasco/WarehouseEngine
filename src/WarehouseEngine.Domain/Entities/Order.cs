using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("AddressId", Name = "IX_Order01")]
public partial class Order
{
    public Order()
    {
        OrderWarehouseItem = new HashSet<OrderWarehouseItem>();
        OrderWarehouseItemOutOfStock = new HashSet<OrderWarehouseItemOutOfStock>();
    }

    [Key]
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int AddressId { get; set; }
    public byte Status { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Order")]
    public virtual Address? Address { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Order")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderWarehouseItem> OrderWarehouseItem { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderWarehouseItemOutOfStock> OrderWarehouseItemOutOfStock { get; set; }
}
