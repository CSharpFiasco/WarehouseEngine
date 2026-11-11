using System.ComponentModel.DataAnnotations;
using WarehouseEngine.Domain.Entities;

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
