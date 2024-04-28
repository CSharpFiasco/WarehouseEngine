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

    public static explicit operator Item(ItemResponseDto v)
    {
        return new Item
        {
            Id = v.Id,
            Sku = v.Sku,
            Description = v.Description,
            IsActive = v.IsActive
        };
    }
}

public class ItemResponseDto
{
    public Guid Id { get; set; }
    public required string Sku { get; set; }
    public required string Description { get; set; }
    public bool IsActive { get; set; }

    public static explicit operator ItemResponseDto(Item item)
    {
        return new ItemResponseDto
        {
            Id = item.Id,
            Sku = item.Sku,
            Description = item.Description,
            IsActive = item.IsActive
        };
    }
}

public class PostItemDto
{
    [JsonIgnore]
    public Guid? Id { get; set; }
    public required string Sku { get; set; }
    public required string Description { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    /// Converts PostItemDto to Item
    /// </summary>
    /// <param name="dto"></param>
    public static explicit operator Item(PostItemDto dto)
    {
        if (!dto.Id.HasValue)
        {
            throw new EntityConversionException<Item, PostItemDto>("Item should should have Id when created");
        }

        if (dto.Id.Value == Guid.Empty)
        {
            throw new EntityConversionException<Item, PostItemDto>("Item should should have Id when created");
        }

        return new Item
        {
            Id = dto.Id.Value,
            Sku = dto.Sku,
            Description = dto.Description,
            IsActive = dto.IsActive
        };
    }
}
