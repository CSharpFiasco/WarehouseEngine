using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WarehouseEngine.Domain.Exceptions;

namespace WarehouseEngine.Domain.Entities;

public partial class Item
{
    public Item()
    {
        WarehouseItem = new HashSet<WarehouseItem>();
        Vendor = new HashSet<Vendor>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    [Required]
    [StringLength(12)]
    public required string Sku { get; set; }

    [Required]
    [StringLength(120)]
    public required string Description { get; set; }

    public bool IsActive { get; set; }

    [JsonIgnore]
    public DateTime? DateCreated { get; set; }

    [JsonIgnore]
    public string? CreatedBy { get; set; }

    [InverseProperty("Item")]
    public virtual ICollection<WarehouseItem> WarehouseItem { get; init; }

    [ForeignKey("ItemId")]
    [InverseProperty("Item")]
    public virtual ICollection<Vendor> Vendor { get; init; }
}
