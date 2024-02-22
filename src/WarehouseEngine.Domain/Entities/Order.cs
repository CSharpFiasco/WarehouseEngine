using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Order
{
    public Order() { }

    [Key]
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public byte Status { get; set; }

    public required Address ShippingAddress { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Order")]
    public virtual Customer? Customer { get; set; }

}
