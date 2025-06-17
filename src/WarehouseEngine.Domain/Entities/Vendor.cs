using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.Entities;

public partial class Vendor
{
    [Key]
    public required Guid Id { get; set; }

    [StringLength(80)]
    public string? Name { get; set; }

    [InverseProperty("Vendor")]
    public virtual ICollection<VendorItem> VendorItem { get; init; } = new List<VendorItem>();
}
