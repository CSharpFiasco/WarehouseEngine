using System.Net;
using WarehouseEngine.Api.Integration.Tests.Factories;

namespace WarehouseEngine.Api.Integration.Tests;

public class WarehouseEngineApiIntegrationTests
    : IClassFixture<WarehouseEngineFactory<Program>>
{
    private readonly WarehouseEngineFactory<Program> _factory;

    public WarehouseEngineApiIntegrationTests(WarehouseEngineFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact(DisplayName = """
        Given a request to the CustomerController
        When the request is unauthorized
        Then the response should have a 401 status code
        """)]
    public async Task CustomerController_NoAuth_Unauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync("api/v1/Customer");
        // Assert
        // response should have 401 status code
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = """
        Given a request to the CustomerController
        When the request is authorized
        And when the requesed resource is not found
        Then the response should have a 404 status code
        """)]
    public async Task CustomerController_Auth_Ok()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var guid = Guid.NewGuid();
        var response = await client.GetAsync($"api/v1/Customer?id={guid}");
        // Assert
        // response should have 200 status code
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
