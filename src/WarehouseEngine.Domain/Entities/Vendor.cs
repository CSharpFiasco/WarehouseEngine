using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseEngine.Domain.Entities;

public partial class Vendor
{
    private Vendor()
    {
        Item = new HashSet<Item>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    [StringLength(MaxNameLength)]
    public string? Name { get; private set; }

    [ForeignKey("VendorId")]
    [InverseProperty("Vendor")]
    public virtual ICollection<Item> Item { get; private set; }
}
