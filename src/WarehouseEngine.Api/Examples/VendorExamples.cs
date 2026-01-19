using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Api.Examples;

public static class VendorExamples
{
    public static readonly VendorResponseDto VendorResponseDto = new(
        Id: Guid.NewGuid(),
        Name: "ACME Corporation"
    );
}
