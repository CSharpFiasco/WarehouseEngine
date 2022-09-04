using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("AddressId", Name = "IX_Customer01")]
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
    public int AddressId { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Customer")]
    public virtual Address? Address { get; set; }
    [InverseProperty("Customer")]
    public virtual ICollection<Order> Order { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Customer")]
    public virtual ICollection<Contact> Contact { get; set; }
}
