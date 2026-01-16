using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Exceptions;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Application.Dtos;

public record CustomerResponseDto(
    Guid Id,
    [property: Required] string Name,
    Address? BillingAddress,
    Address ShippingAddress,
    DateTime DateCreated,
    string CreatedBy,
    DateTime? DateModified,
    string? ModifiedBy)
{
    public static explicit operator CustomerResponseDto(Customer customer)
    {
        return new CustomerResponseDto(
            customer.Id,
            customer.Name,
            customer.BillingAddress,
            customer.ShippingAddress,
            customer.DateCreated,
            customer.CreatedBy,
            customer.DateModified,
            customer.ModifiedBy
        );
    }
}

public record PostCustomerDto
{
    /// <summary>
    /// This should be null when deserialized from a request
    /// </summary>
    [JsonIgnore]
    public Guid? Id { get; set; }

    [Required]
    public required string Name { get; init; }

    public Address? BillingAddress { get; init; }

    [Required]
    public required Address ShippingAddress { get; init; }

    [JsonIgnore]
    public DateTime? DateCreated { get; set; }

    [JsonIgnore]
    public string? CreatedBy { get; set; }

    [JsonIgnore]
    public DateTime? DateModified { get; set; }

    [JsonIgnore]
    public string? ModifiedBy { get; set; }

    public static explicit operator Customer(PostCustomerDto dto)
    {
        if (!dto.Id.HasValue)
        {
            throw new EntityConversionException<Customer, PostCustomerDto>("Id is null");
        }
        if (dto.Id.Value == Guid.Empty)
        {
            throw new EntityConversionException<Customer, PostCustomerDto>("Id is empty");
        }

        if (!dto.DateCreated.HasValue || dto.CreatedBy is null)
        {
            throw new EntityConversionException<Customer, PostCustomerDto>("Date created is null");
        }

        if (dto.ShippingAddress is null)
        {
            throw new EntityConversionException<Customer, PostCustomerDto>("Shipping address is null");
        }

        return new Customer
        {
            Id = dto.Id.Value,
            Name = dto.Name,
            BillingAddress = dto.BillingAddress,
            ShippingAddress = dto.ShippingAddress,
            DateCreated = dto.DateCreated.Value,
            CreatedBy = dto.CreatedBy
        };
    }
}
