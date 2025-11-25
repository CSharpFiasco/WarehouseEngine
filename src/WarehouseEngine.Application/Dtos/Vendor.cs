using System.Text.Json.Serialization;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Exceptions;

namespace WarehouseEngine.Application.Dtos;

public class VendorResponseDto
{
    public required Guid Id { get; init; }

    public string? Name { get; init; }

    public static explicit operator VendorResponseDto(Vendor vendor)
    {
        return new VendorResponseDto
        {
            Id = vendor.Id,
            Name = vendor.Name
        };
    }
}

public class PostVendorDto
{
    /// <summary>
    /// This should be null when deserialized from a request
    /// </summary>
    [JsonIgnore]
    public Guid? Id { get; set; }

    public string? Name { get; init; }

    public static explicit operator Vendor(PostVendorDto dto)
    {
        if (!dto.Id.HasValue)
        {
            throw new EntityConversionException<Vendor, PostVendorDto>("Id is null");
        }
        if (dto.Id.Value == Guid.Empty)
        {
            throw new EntityConversionException<Vendor, PostVendorDto>("Id is empty");
        }

        return new Vendor
        {
            Id = dto.Id.Value,
            Name = dto.Name
        };
    }
}

