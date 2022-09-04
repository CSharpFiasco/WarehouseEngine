using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

public partial class Address
{
    public Address()
    {
        Contact = new HashSet<Contact>();
        Customer = new HashSet<Customer>();
        Order = new HashSet<Order>();
    }

    [Key]
    public int Id { get; set; }
    [Column("Address")]
    [StringLength(80)]
    [Unicode(false)]
    public required string Address1 { get; set; }
    [StringLength(80)]
    [Unicode(false)]
    public required string Address2 { get; set; }
    [StringLength(32)]
    [Unicode(false)]
    public required string City { get; set; }
    [StringLength(2)]
    [Unicode(false)]
    public required string State { get; set; }
    [StringLength(11)]
    [Unicode(false)]
    public required string Zip { get; set; }

    [InverseProperty("Address")]
    public virtual ICollection<Contact> Contact { get; set; }
    [InverseProperty("Address")]
    public virtual ICollection<Customer> Customer { get; set; }
    [InverseProperty("Address")]
    public virtual ICollection<Order> Order { get; set; }
}

