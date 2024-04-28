using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using WarehouseEngine.Domain.Exceptions;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Customer
{
    public Customer()
    {
        Order = new HashSet<Order>();
        Contact = new HashSet<Contact>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required Guid Id { get; set; }

    [StringLength(80)]
    public required string Name { get; set; }

    public Address? BillingAddress { get; set; }

    [DisallowNull]
    public required Address ShippingAddress { get; set; }

    public required DateTime DateCreated { get; set; }

    [StringLength(80)]
    public required string CreatedBy { get; set; }

    public DateTime? DateModified { get; set; }

    [StringLength(80)]
    public string? ModifiedBy { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Order { get; init; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Customer")]
    public virtual ICollection<Contact> Contact { get; init; }
}

public class CustomerResponseDto
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public Address? BillingAddress { get; init; }

    public required Address ShippingAddress { get; init; }

    public required DateTime DateCreated { get; init; }

    public required string CreatedBy { get; init; }

    public DateTime? DateModified { get; init; }

    public string? ModifiedBy { get; init; }

    public static explicit operator CustomerResponseDto(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            Name = customer.Name,
            BillingAddress = customer.BillingAddress,
            ShippingAddress = customer.ShippingAddress,
            DateCreated = customer.DateCreated,
            CreatedBy = customer.CreatedBy,
            DateModified = customer.DateModified,
            ModifiedBy = customer.ModifiedBy
        };
    }
}

public class PostCustomerDto
{
    /// <summary>
    /// This should be null when deserialized from a request
    /// </summary>
    [JsonIgnore]
    public Guid? Id { get; set; }
    public required string Name { get; init; }

    public Address? BillingAddress { get; init; }

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
