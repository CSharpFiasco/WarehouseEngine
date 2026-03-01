using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WarehouseEngine.Api.Integration.Tests.Factories;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Domain.Models.Auth;
using Xunit;

namespace WarehouseEngine.Api.Integration.Tests;

public class WarehouseEndpointTests
{
    private readonly WarehouseEngineFactory _factory;

    public WarehouseEndpointTests(WarehouseEngineFactory factory)
    {
        _factory = factory;
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to retrieve a warehouse
        When the request is unauthorized
        Then the response should have a 401 status code
        """)]
    public async Task WarehouseEndpoints_GET_NoAuth_Unauthorized()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync($"api/v1/Warehouse/{WarehouseEngineFactory.WarehouseId1}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to retrieve a warehouse
        When the request is authorized
        And when the requested resource is not found
        Then the response should have a 404 status code
        """)]
    public async Task WarehouseEndpoints_GET_Auth_NotFound()
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

        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = authHeader;


        var guid = Guid.NewGuid();
        using var response = await client.GetAsync($"api/v1/Warehouse/{guid}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to retrieve a warehouse
        When the request is authorized
        And when the requested resource is found
        Then the response should have a 200 status code
        """)]
    public async Task WarehouseEndpoints_GET_Auth_Found()
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

        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = authHeader;

        using var response = await client.GetAsync($"api/v1/Warehouse/{WarehouseEngineFactory.WarehouseId1}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var contentString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var deserializedWarehouseDto = JsonSerializer.Deserialize<WarehouseResponseDto>(contentString, JsonSerializerOptions.Web);
        Assert.NotNull(deserializedWarehouseDto);

        Assert.Equal("Warehouse 1", deserializedWarehouseDto.Name);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to get warehouse count
        When the request is unauthorized
        Then the response should have a 401 status code
        """)]
    public async Task WarehouseEndpoints_Count_NoAuth_Unauthorized()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync("api/v1/Warehouse/count", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to get warehouse count
        When the request is authorized
        Then the response should have a 200 status code
        """)]
    public async Task WarehouseEndpoints_Count_Auth_Success()
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

        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = authHeader;

        using var response = await client.GetAsync("api/v1/Warehouse/count", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var contentString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var count = JsonSerializer.Deserialize<int>(contentString);
        Assert.True(count >= 0);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to create a warehouse
        When the request is unauthorized
        Then the response should have a 401 status code
        """)]
    public async Task WarehouseEndpoints_POST_NoAuth_Unauthorized()
    {
        using var client = _factory.CreateClient();

        var warehousePayload = new PostWarehouseDto
        {
            Name = "New Warehouse"
        };

        using var warehouseContent = JsonContent.Create(warehousePayload);

        using var response = await client.PostAsync("api/v1/Warehouse", warehouseContent, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to create a warehouse
        When the request is authorized
        Then the response should have a 201 status code
        """)]
    public async Task WarehouseEndpoints_POST_Auth_Created()
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

        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = authHeader;

        var warehousePayload = new PostWarehouseDto
        {
            Name = "New Warehouse"
        };

        using var warehouseContent = JsonContent.Create(warehousePayload);
        using var response = await client.PostAsync("api/v1/Warehouse", warehouseContent, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to create a warehouse
        When the request is authorized
        And the request has an id
        Then the response should have a 201 status code
        And the warehouse should be created with a different id
        """)]
    public async Task WarehouseEndpoints_POST_Auth_WithId()
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

        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = authHeader;

        var id = Guid.NewGuid();

        var warehousePayload = new PostWarehouseDto
        {
            Id = id,
            Name = "New Warehouse"
        };

        using var warehouseContent = JsonContent.Create(warehousePayload);
        using var response = await client.PostAsync("api/v1/Warehouse", warehouseContent, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var getResponse = await client.GetAsync($"api/v1/Warehouse/{id}", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
