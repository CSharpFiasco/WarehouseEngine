using WarehouseEngine.Application.Dtos;

namespace WarehouseEngine.Api.Examples;

public static class PositionExamples
{
    public static readonly PositionResponseDto PositionResponseDto = new()
    {
        Id = Guid.NewGuid(),
        Name = "Software Engineer"
    };
}
