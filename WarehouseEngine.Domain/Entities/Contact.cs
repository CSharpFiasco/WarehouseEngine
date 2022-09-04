using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

[Index("AddressId", Name = "IX_Contact01")]
public partial class Contact
{
    public Contact()
    {
        Customer = new HashSet<Customer>();
    }

    [Key]
    public int Id { get; set; }
    [StringLength(80)]
    [Unicode(false)]
    public string? FirstName { get; set; }
    [StringLength(80)]
    [Unicode(false)]
    public string? LastName { get; set; }
    [StringLength(60)]
    [Unicode(false)]
    public string? Email { get; set; }
    public int? AddressId { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("Contact")]
    public virtual Address? Address { get; set; }

    [ForeignKey("ContactId")]
    [InverseProperty("Contact")]
    public virtual ICollection<Customer> Customer { get; set; }
}
