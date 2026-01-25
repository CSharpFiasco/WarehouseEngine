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

public class ItemEndpointTests
{
    private readonly WarehouseEngineFactory _factory;

    public ItemEndpointTests(WarehouseEngineFactory factory)
    {
        _factory = factory;
    }

    [Fact(DisplayName = """
        Given a request to the ItemEndpoints
        When the request is unauthorized
        Then the response should have a 401 status code
        """)]
    public async Task ItemEndpoints_NoAuth_Unauthorized()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var guid = Guid.NewGuid();
        using var response = await client.GetAsync($"api/v1/item/{guid}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = """
        Given a request to the ItemEndpoints
        When the request is authorized
        And when the requested resource is not found
        Then the response should have a 400 status code
        """)]
    public async Task ItemEndpoints_Auth_NotFound()
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
        using var response = await client.GetAsync($"api/v1/item/{guid}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = """
        Given a request to the ItemEndpoints
        When the request is authorized
        And when the requested resource is found
        Then the response should have a 200 status code
        """)]
    public async Task ItemEndpoints_Auth_Found()
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
        using var response = await client.GetAsync($"api/v1/item/{WarehouseEngineFactory.ItemId1}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var item = JsonSerializer.Deserialize<ItemResponseDto>(responseContent, JsonSerializerOptions.Web);

        Assert.NotNull(item);
        Assert.Equal(WarehouseEngineFactory.ItemId1, item.Id);
    }

    [Fact(DisplayName = """
        Given a request to create an item
        When the request is authorized
        And when the item data is valid
        Then the response should have a 200 status code
        And the item should be created
        """)]
    public async Task ItemEndpoints_Create_Success()
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

        var newItem = new PostItemDto { Sku = "NewSku123", Description = "New Test Item", IsActive = true };
        var json = JsonSerializer.Serialize(newItem);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PostAsync("api/v1/item", content, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var createdItem = JsonSerializer.Deserialize<ItemResponseDto>(responseContent, JsonSerializerOptions.Web);

        Assert.NotNull(createdItem);
        Assert.Equal("NewSku123", createdItem.Sku);
        Assert.Equal("New Test Item", createdItem.Description);
        Assert.NotEqual(Guid.Empty, createdItem.Id);
    }

    [Fact(DisplayName = """
        Given a request to get item count
        When the request is authorized
        Then the response should have a 200 status code
        And return the count
        """)]
    public async Task ItemEndpoints_Count_Success()
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
        using var response = await client.GetAsync("api/v1/item/count", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var count = JsonSerializer.Deserialize<int>(responseContent);

        Assert.True(count >= 0);
    }
}
