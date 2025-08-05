using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Domain.Exceptions;

namespace WarehouseEngine.Domain.Entities;

public partial class Vendor
{
    public Vendor()
    {
        Item = new HashSet<Item>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    [StringLength(80)]
    public string? Name { get; set; }

    [ForeignKey("VendorId")]
    [InverseProperty("Vendor")]
    public virtual ICollection<Item> Item { get; init; }
}

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
