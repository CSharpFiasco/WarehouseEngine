using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Domain.Tests;
public class WarehouseTests
{
    private readonly ITestOutputHelper _output;
    public WarehouseTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void GivenIdIsGiven_WhenDeserialized_ThenSerializationIsSuccessful()
    {
        //SETUP
        var json = """
            {
                "Id": "d3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b",
                "Name": "Test Warehouse"
            }
            """;
        _output.WriteLine(json);

        //ATTEMPT
        var warehouse = JsonSerializer.Deserialize<Warehouse>(json);
        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(warehouse!, new ValidationContext(warehouse!), errors, true);


        //VERIFY
        Assert.NotNull(warehouse);
        Assert.Empty(errors);
        Assert.True(isValid);
        Assert.Equal("d3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b", warehouse.Id.ToString());
    }

    [Fact]
    public void GivenNameExceedsMaxLength_WhenDeserialized_ThenValidationFails()
    {
        //SETUP
        var json = """
            {
                "Id": "d3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b",
                "Name": "This name is way too long and exceeds the thirty two character limit"
            }
            """;
        _output.WriteLine(json);

        //ATTEMPT
        var warehouse = JsonSerializer.Deserialize<Warehouse>(json);
        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(warehouse!, new ValidationContext(warehouse!), errors, true);


        //VERIFY
        Assert.NotNull(warehouse);
        Assert.False(isValid);
        var error = Assert.Single(errors);
        Assert.Contains("Name", error.ErrorMessage!);
    }
}
