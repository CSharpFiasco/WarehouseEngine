using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    [StringLength(80)]
    public required string Name { get; set; }

    public Address? BillingAddress { get; set; }

    public required Address ShippingAddress { get; set; }

    public required DateTime DateCreated { get; set; }

    [StringLength(80)]
    public required string CreatedBy { get; set; }

    public DateTime? DateModified { get; set; }

    [StringLength(80)]
    public string? ModifiedBy { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Order { get; init; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Customer")]
    public virtual ICollection<Contact> Contact { get; init; }
}

public class PostCustomerDto {
    public required string Name { get; init; }
    public Address? BillingAddress { get; set; }
    public required Address ShippingAddress { get; set; }
}
