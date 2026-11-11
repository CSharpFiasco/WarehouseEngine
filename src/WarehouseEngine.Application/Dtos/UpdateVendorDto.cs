using System.ComponentModel.DataAnnotations;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Application.Dtos;

/// <summary>Changes the name of the vendor identified by the request route.</summary>
public sealed record UpdateVendorDto
{
    [StringLength(Vendor.MaxNameLength)]
    public string? Name { get; init; }
}
