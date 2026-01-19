using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Exceptions;

namespace WarehouseEngine.Application.Dtos;

public record ItemResponseDto(Guid Id, string Sku, string Description, bool IsActive)
{
    public static explicit operator ItemResponseDto(Item item)
    {
        return new ItemResponseDto(
            item.Id,
            item.Sku,
            item.Description,
            item.IsActive
        );
    }

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

public record PostItemDto
{
    [JsonIgnore]
    public Guid? Id { get; set; }
    public required string Sku { get; init; }
    public required string Description { get; init; }
    public bool IsActive { get; init; }

    /// <summary>
    /// Converts PostItemDto to Item
    /// </summary>
    /// <param name="dto"></param>
    public static explicit operator Item(PostItemDto dto)
    {
        if (!dto.Id.HasValue)
        {
            throw new EntityConversionException<Item, PostItemDto>("Item should have Id when created");
        }

        if (dto.Id.Value == Guid.Empty)
        {
            throw new EntityConversionException<Item, PostItemDto>("Item should have Id when created");
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
