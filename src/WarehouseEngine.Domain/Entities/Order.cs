using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Order
{
    private Order()
    {
        OrderWarehouseItem = new HashSet<OrderWarehouseItem>();
        OrderWarehouseItemOutOfStock = new HashSet<OrderWarehouseItemOutOfStock>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public byte Status { get; private set; }

    public Address ShippingAddress { get; private set; } = null!;

    [ForeignKey("CustomerId")]
    [InverseProperty("Order")]
    public virtual Customer? Customer { get; private set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderWarehouseItem> OrderWarehouseItem { get; private set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderWarehouseItemOutOfStock> OrderWarehouseItemOutOfStock { get; private set; }
}
