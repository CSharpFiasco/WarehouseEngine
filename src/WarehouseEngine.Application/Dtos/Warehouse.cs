using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Exceptions;

namespace WarehouseEngine.Application.Dtos;

public class WarehouseResponseDto
{
    public required Guid Id { get; init; }

    [Required]
    public required string Name { get; init; }

    public static explicit operator WarehouseResponseDto(Warehouse warehouse)
    {
        return new WarehouseResponseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name
        };
    }
}

public class PostWarehouseDto
{
    /// <summary>
    /// This should be null when deserialized from a request
    /// </summary>
    [JsonIgnore]
    public Guid? Id { get; set; }

    [Required]
    public required string Name { get; init; }

    public static explicit operator Warehouse(PostWarehouseDto dto)
    {
        if (!dto.Id.HasValue)
        {
            throw new EntityConversionException<Warehouse, PostWarehouseDto>("Id is null");
        }
        if (dto.Id.Value == Guid.Empty)
        {
            throw new EntityConversionException<Warehouse, PostWarehouseDto>("Id is empty");
        }

        return new Warehouse
        {
            Id = dto.Id.Value,
            Name = dto.Name
        };
    }
}
