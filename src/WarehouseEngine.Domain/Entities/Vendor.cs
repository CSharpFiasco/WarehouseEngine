using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

public partial class Vendor
{
    public Vendor()
    {
        Item = new HashSet<Item>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    [StringLength(80)]
    public string? Name { get; set; }

    [ForeignKey("VendorId")]
    [InverseProperty("Vendor")]
    public virtual ICollection<Item> Item { get; init; }
}
