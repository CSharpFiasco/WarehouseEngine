using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Customer
{
    private Customer()
    {
        Order = new HashSet<Order>();
        Contact = new HashSet<Contact>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    [StringLength(MaxNameLength)]
    [Required]
    public string Name { get; private set; } = null!;

    public Address? BillingAddress { get; private set; }

    [DisallowNull]
    public Address ShippingAddress { get; private set; } = null!;

    public DateTime DateCreated { get; private set; }

    [StringLength(MaxUserNameLength)]
    public string CreatedBy { get; private set; } = null!;

    public DateTime? DateModified { get; private set; }

    [StringLength(MaxUserNameLength)]
    public string? ModifiedBy { get; private set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Order { get; private set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Customer")]
    public virtual ICollection<Contact> Contact { get; private set; }
}

