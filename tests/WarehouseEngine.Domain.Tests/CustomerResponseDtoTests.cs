using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using WarehouseEngine.Domain.Entities;
using Xunit.Abstractions;

namespace WarehouseEngine.Domain.Tests;
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
        var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web) { DefaultIgnoreCondition = JsonIgnoreCondition.Never };
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
}
