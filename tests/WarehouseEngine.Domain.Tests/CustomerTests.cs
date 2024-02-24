using WarehouseEngine.Domain.Entities;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace WarehouseEngine.Domain.Tests;
public class CustomerTests
{
    private readonly ITestOutputHelper _output;
    public CustomerTests(ITestOutputHelper output)
    {
        _output = output;
    }
    [Fact]
    public void GivenIdIsGiven_WhenDeserialized_ThenSerializationIsSuccessful()
    {
        //SETUP
        var customer = new Customer { Id = Guid.Empty, Name = "", ShippingAddress = null!, DateCreated = DateTime.MinValue, CreatedBy = string.Empty };
        var json = JsonSerializer.Serialize(customer);
        _output.WriteLine(json);

        //ATTEMPT
        var customer2 = JsonSerializer.Deserialize<Customer>(json);
        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(customer2!, new ValidationContext(customer2!), errors, true);


        //VERIFY
        Assert.NotNull(customer2);
        //Assert.Null(customer2.Id);
        Assert.True(isValid);
        Assert.Equal(customer.Id, customer2.Id);
    }
}
