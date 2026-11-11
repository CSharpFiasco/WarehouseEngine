using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Exceptions;

namespace WarehouseEngine.Application.Dtos;

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

    public static explicit operator Item(ItemResponseDto v)
    {
        return Item.Create(v.Id, v.Sku, v.Description, v.IsActive);
    }
}

public class PostItemDto
{
    [JsonIgnore]
    public Guid? Id { get; set; }
    [Required]
    [StringLength(Item.MaxSkuLength)]
    public required string Sku { get; set; }
    [Required]
    [StringLength(Item.MaxDescriptionLength)]
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

        return Item.Create(dto.Id.Value, dto.Sku, dto.Description, dto.IsActive);
    }
}
