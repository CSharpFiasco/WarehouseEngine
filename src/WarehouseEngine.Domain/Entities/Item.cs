using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WarehouseEngine.Domain.Entities;

public partial class Item
{
    private Item()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
        Vendor = new HashSet<Vendor>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; private set; }

    [Required]
    [StringLength(MaxSkuLength)]
    public string Sku { get; private set; } = null!;

    [Required]
    [StringLength(MaxDescriptionLength)]
    public string Description { get; private set; } = null!;

    public bool IsActive { get; private set; }

    [JsonIgnore]
    public DateTime? DateCreated { get; private set; }

    [JsonIgnore]
    public string? CreatedBy { get; private set; }

    [InverseProperty("Item")]
    public virtual ICollection<WarehouseItem> WarehouseItem { get; private set; }

    [ForeignKey("ItemId")]
    [InverseProperty("Item")]
    public virtual ICollection<Vendor> Vendor { get; private set; }
}
