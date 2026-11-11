using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Contact
{
    private Contact()
    {
        Customer = new HashSet<Customer>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    [StringLength(MaxNameLength)]
    public string? FirstName { get; private set; }

    [StringLength(MaxNameLength)]
    public string? LastName { get; private set; }

    [StringLength(MaxEmailLength)]
    public string? Email { get; private set; }

    public Address? Address { get; private set; }

    [ForeignKey("ContactId")]
    [InverseProperty("Contact")]
    public virtual ICollection<Customer> Customer { get; private set; }
}
