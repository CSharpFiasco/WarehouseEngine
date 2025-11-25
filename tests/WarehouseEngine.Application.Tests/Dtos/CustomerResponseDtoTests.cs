using System.ComponentModel.DataAnnotations;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Domain.Exceptions;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Application.Tests;
public class CustomerResponseDtoTests
{
    private readonly ITestOutputHelper _output;
    public CustomerResponseDtoTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void GivenIdIsGiven_WhenDeserialized_ThenSerializationIsSuccessful()
    {
        //SETUP
        var json = """
            {
                "id": "d3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b",
                "name": "Test Customer",
                "shippingAddress": {
                    "address1": "123 Main St",
                    "city": "Anytown",
                    "state": "NY",
                    "zipCode": "12345"
                },
                "dateCreated": "2021-01-01T00:00:00",
                "createdBy": "Test User"
            }
            """;
        _output.WriteLine(json);

        //ATTEMPT
        var serializerOptions = JsonSerializerOptions.Web;
        var customer = JsonSerializer.Deserialize<CustomerResponseDto>(json, serializerOptions);
        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(customer!, new ValidationContext(customer!), errors, true);

        //VERIFY
        Assert.True(isValid);
        Assert.Empty(errors);
    }

    [Fact]
    public void GivenNameIsNotGiven_WhenDeserialized_ThenSerializationIsSuccessful()
    {
        //SETUP
        var json = """
            {
                "id": "d3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b",
                "name": null,
                "shippingAddress": {
                    "address1": "123 Main St",
                    "city": "Anytown",
                    "state": "NY",
                    "zipCode": "12345"
                },
                "dateCreated": "2021-01-01T00:00:00",
                "createdBy": "Test User"
            }
            """;
        _output.WriteLine(json);

        //ATTEMPT
        var serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var customer = JsonSerializer.Deserialize<CustomerResponseDto>(json, serializerOptions);

        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(customer!, new ValidationContext(customer!), errors, true);

        //VERIFY
        Assert.False(isValid);
        var error = Assert.Single(errors);
        Assert.Single(
            collection: error.MemberNames,
            predicate: member =>
            {
                Assert.Equal(nameof(CustomerResponseDto.Name), member); return true;
            }
        );
    }

    [Fact]
    public void GivenPostCustomerDto_WhenConvertedToCustomer_ThenConversionIsSuccessful()
    {
        //SETUP
        var dto = new PostCustomerDto
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            ShippingAddress = new Address
            {
                Address1 = "123 Main St",
                City = "Anytown",
                State = "NY",
                ZipCode = "12345"
            },
            DateCreated = DateTime.MinValue,
            CreatedBy = "a"
        };

        //ATTEMPT
        var customer = (Customer)dto;

        //VERIFY
        Assert.NotNull(customer);
        Assert.Equal(dto.Name, customer.Name);
        Assert.Equal(dto.ShippingAddress, customer.ShippingAddress);
    }

    [Fact]
    public void GivenPostCustomerDtoWithNullShippingAddress_WhenConvertedToCustomer_ThenThrowEntityConversionException()
    {
        //SETUP
        var dto = new PostCustomerDto
        {
            Name = "Test Customer",
            ShippingAddress = null!,
            DateCreated = DateTime.MinValue,
            CreatedBy = "a"
        };

        //ATTEMPT
        var ex = Assert.Throws<EntityConversionException<Customer, PostCustomerDto>>(() => (Customer)dto);


    }
}
