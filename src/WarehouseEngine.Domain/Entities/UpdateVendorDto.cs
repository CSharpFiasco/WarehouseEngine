using System.Text.Json.Serialization;

namespace WarehouseEngine.Domain.Entities;

public class UpdateVendorDto
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public static explicit operator Vendor(UpdateVendorDto dto)
    {
        return new Vendor
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }
}
