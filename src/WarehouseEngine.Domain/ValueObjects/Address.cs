using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEngine.Domain.ValueObjects;

[Owned]
public record Address
{
    [Required]
    [StringLength(80)]
    public required string Address1 { get; init; }

    [StringLength(80)]
    public string? Address2 { get; init; }

    [Required]
    [StringLength(32)]
    public required string City { get; init; }
    [Required]
    [StringLength(2)]
    public required string State { get; init; }
    [Required]
    [StringLength(11)]
    public required string ZipCode { get; init; }
}
