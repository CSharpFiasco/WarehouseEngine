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
        Contact = new HashSet<Contact>();
    }

    [Key]
    public int Id { get; set; }
    [StringLength(80)]
    [Unicode(false)]
    public required string Name { get; set; }

    public Address? BillingAddress { get; set; }
    public required Address ShippingAddress { get; set; }
    [InverseProperty("Customer")]
    public virtual ICollection<Order> Order { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Customer")]
    public virtual ICollection<Contact> Contact { get; set; }
}
