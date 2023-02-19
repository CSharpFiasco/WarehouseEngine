using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

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

    public Address? Address { get; set; }

    [ForeignKey("ContactId")]
    [InverseProperty("Contact")]
    public virtual ICollection<Customer> Customer { get; set; }
}
