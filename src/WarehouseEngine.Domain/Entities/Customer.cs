using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Customer
{
    public Customer()
    {
        Order = new HashSet<Order>();
    }

    [Key]
    public int Id { get; set; }
    [StringLength(80)]
    [Unicode(false)]
    public required string Name { get; set; }

    public Address? BillingAddress { get; set; }
    public required Address ShippingAddress { get; set; }
    public required DateTime DateCreated { get; set; }
    [InverseProperty("Customer")]
    public virtual ICollection<Order> Order { get; set; }
}
