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
    public int Id { get; set; }
    [StringLength(80)]
    [Unicode(false)]
    public string? Name { get; set; }

    [ForeignKey("VendorId")]
    [InverseProperty("Vendor")]
    public virtual ICollection<Item> Item { get; set; }
}
