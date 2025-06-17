using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Order
{
    [Key]
    public required Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public byte Status { get; set; }

    public required Address ShippingAddress { get; set; }

    public DateTime DateCreated { get; set; }

    [StringLength(80)]
    public string CreatedBy { get; set; } = null!;

    public DateTime? DateModified { get; set; }

    [StringLength(80)]
    public string? ModifiedBy { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Order")]
    public virtual Customer? Customer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderWarehouseItem> OrderWarehouseItem { get; init; } = new List<OrderWarehouseItem>();

    [InverseProperty("Order")]
    public virtual ICollection<OrderWarehouseItemOutOfStock> OrderWarehouseItemOutOfStock { get; init; } = new List<OrderWarehouseItemOutOfStock>();

    [InverseProperty("Order")]
    public virtual ICollection<WarehousePickList> WarehousePickList { get; init; } = new List<WarehousePickList>();
}
