using WarehouseEngine.Application.Dtos;

namespace WarehouseEngine.Api.Examples;

public static class WarehouseExamples
{
    public static readonly WarehouseResponseDto WarehouseResponseDto = new()
    {
        Id = Guid.NewGuid(),
        Name = "Main Warehouse"
    };
}
