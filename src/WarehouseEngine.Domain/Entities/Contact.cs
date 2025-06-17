using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Contact
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(80)]
    public string? FirstName { get; set; }

    [StringLength(80)]
    public string? LastName { get; set; }

    [StringLength(60)]
    public string? Email { get; set; }

    public Address? Address { get; set; }
    public DateTime DateCreated { get; set; }

    [StringLength(80)]
    public string CreatedBy { get; set; } = null!;

    public DateTime? DateModified { get; set; }

    [StringLength(80)]
    public string? ModifiedBy { get; set; }

    [ForeignKey("ContactId")]
    [InverseProperty("Contact")]
    public virtual ICollection<Customer> Customer { get; init; } = new List<Customer>();
}
