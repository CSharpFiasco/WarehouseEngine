using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WarehouseEngine.Api.Integration.Tests.Factories;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Domain.Models.Auth;
using Xunit;

namespace WarehouseEngine.Api.Integration.Tests;

public class PositionEndpointTests
{
    private readonly WarehouseEngineFactory _factory;

    public PositionEndpointTests(WarehouseEngineFactory factory)
    {
        _factory = factory;
    }

    [Fact(DisplayName = """
        Given a request to the PositionEndpoints
        When the request is unauthorized
        Then the response should have a 401 status code
        """)]
    public async Task PositionEndpoints_NoAuth_Unauthorized()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var guid = Guid.NewGuid();
        using var response = await client.GetAsync($"api/v1/position/{guid}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = """
        Given a request to the PositionEndpoints
        When the request is authorized
        And when the requested resource is not found
        Then the response should have a 404 status code
        """)]
    public async Task PositionEndpoints_Auth_NotFound()
    {
        var options = Options.Create(new JwtConfiguration
        {
            Secret = "MyIntegrationTestSecr3!tIsSoSecr3t",
            ValidIssuer = "http://localhost",
            ValidAudience = "http://warehouse-api"
        });
        var jwtService = new JwtService(options);

        var tokenString = jwtService.GetNewToken([]);
        var authHeader = new AuthenticationHeaderValue("Bearer", tokenString);

        // Arrange
        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = authHeader;

        // Act
        var guid = Guid.NewGuid();
        using var response = await client.GetAsync($"api/v1/position/{guid}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = """
        Given a request to the PositionEndpoints
        When the request is authorized
        And when the requested resource is found
        Then the response should have a 200 status code
        """)]
    public async Task PositionEndpoints_Auth_Found()
    {
        var options = Options.Create(new JwtConfiguration
        {
            Secret = "MyIntegrationTestSecr3!tIsSoSecr3t",
            ValidIssuer = "http://localhost",
            ValidAudience = "http://warehouse-api"
        });
        var jwtService = new JwtService(options);

        var tokenString = jwtService.GetNewToken([]);
        var authHeader = new AuthenticationHeaderValue("Bearer", tokenString);

        // Arrange
        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = authHeader;

        // Act
        using var response = await client.GetAsync($"api/v1/position/{WarehouseEngineFactory.PositionId1}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var position = JsonSerializer.Deserialize<PositionResponseDto>(responseContent, JsonSerializerOptions.Web);

        Assert.NotNull(position);
        Assert.Equal(WarehouseEngineFactory.PositionId1, position.Id);
    }

    [Fact(DisplayName = """
        Given a request to create a position
        When the request is authorized
        And when the position data is valid
        Then the response should have a 200 status code
        And the position should be created
        """)]
    public async Task PositionEndpoints_Create_Success()
    {
        var options = Options.Create(new JwtConfiguration
        {
            Secret = "MyIntegrationTestSecr3!tIsSoSecr3t",
            ValidIssuer = "http://localhost",
            ValidAudience = "http://warehouse-api"
        });
        var jwtService = new JwtService(options);

        var tokenString = jwtService.GetNewToken([]);
        var authHeader = new AuthenticationHeaderValue("Bearer", tokenString);

        // Arrange
        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = authHeader;

        var newPosition = new PostPositionDto { Name = "New Test Position" };
        var json = JsonSerializer.Serialize(newPosition);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PostAsync("api/v1/position", content, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var createdPosition = JsonSerializer.Deserialize<PositionResponseDto>(responseContent, JsonSerializerOptions.Web);

        Assert.NotNull(createdPosition);
        Assert.Equal("New Test Position", createdPosition.Name);
        Assert.NotEqual(Guid.Empty, createdPosition.Id);
    }

    [Fact(DisplayName = """
        Given a request to get position count
        When the request is authorized
        Then the response should have a 200 status code
        And return the count
        """)]
    public async Task PositionEndpoints_Count_Success()
    {
        var options = Options.Create(new JwtConfiguration
        {
            Secret = "MyIntegrationTestSecr3!tIsSoSecr3t",
            ValidIssuer = "http://localhost",
            ValidAudience = "http://warehouse-api"
        });
        var jwtService = new JwtService(options);

        var tokenString = jwtService.GetNewToken([]);
        var authHeader = new AuthenticationHeaderValue("Bearer", tokenString);

        // Arrange
        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = authHeader;

        // Act
        using var response = await client.GetAsync("api/v1/position/count", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var count = JsonSerializer.Deserialize<int>(responseContent);

        Assert.True(count >= 0);
    }
}
