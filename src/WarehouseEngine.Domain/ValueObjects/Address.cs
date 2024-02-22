using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Domain.Helpers;

namespace WarehouseEngine.Domain.ValueObjects;

[Owned]
public class Address : ValueObject
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

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        // Using a yield return statement to return each element one at a time
        yield return Address1;
        yield return Address2;
        yield return City;
        yield return State;
        yield return ZipCode;
    }
}
