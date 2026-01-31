using System.Text.Json.Serialization;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Exceptions;

namespace WarehouseEngine.Application.Dtos;

public class PositionResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public static explicit operator PositionResponseDto(Position position)
    {
        return new PositionResponseDto
        {
            Id = position.Id,
            Name = position.Name
        };
    }

    public static explicit operator Position(PositionResponseDto dto)
    {
        return new Position
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }
}

public class PostPositionDto
{
    [JsonIgnore]
    public Guid? Id { get; set; }
    public required string Name { get; set; }

    /// <summary>
    /// Converts PostPositionDto to Position
    /// </summary>
    /// <param name="dto"></param>
    public static explicit operator Position(PostPositionDto dto)
    {
        if (!dto.Id.HasValue)
        {
            throw new EntityConversionException<Position, PostPositionDto>("Position should have Id when created");
        }

        if (dto.Id.Value == Guid.Empty)
        {
            throw new EntityConversionException<Position, PostPositionDto>("Position should have Id when created");
        }

        return new Position
        {
            Id = dto.Id.Value,
            Name = dto.Name
        };
    }
}
