using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using WarehouseEngine.Api.Integration.Tests.Factories;
using WarehouseEngine.Application.Commands;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Application.Interfaces;
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

        var warehousePayload = new CreateWarehouseCommand
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

        var warehousePayload = new CreateWarehouseCommand
        {
            Name = Guid.NewGuid().ToString("N")
        };

        using var warehouseContent = JsonContent.Create(warehousePayload);
        using var response = await client.PostAsync("api/v1/Warehouse", warehouseContent, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(string.Empty, await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
        await using var context = _factory.CreateContext();
        var persisted = await context.Warehouse.SingleAsync(w => w.Name == warehousePayload.Name, TestContext.Current.CancellationToken);
        Assert.NotEqual(Guid.Empty, persisted.Id);
        using var getResponse = await client.GetAsync($"api/v1/Warehouse/{persisted.Id}", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var retrieved = await getResponse.Content.ReadFromJsonAsync<WarehouseResponseDto>(TestContext.Current.CancellationToken);
        Assert.NotNull(retrieved);
        Assert.Equal(persisted.Id, retrieved.Id);
        Assert.Equal(warehousePayload.Name, retrieved.Name);
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

        var warehousePayload = new
        {
            Id = id,
            Name = Guid.NewGuid().ToString("N")
        };

        using var warehouseContent = JsonContent.Create(warehousePayload);
        using var response = await client.PostAsync("api/v1/Warehouse", warehouseContent, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var getResponse = await client.GetAsync($"api/v1/Warehouse/{id}", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        await using var context = _factory.CreateContext();
        var persisted = await context.Warehouse.SingleAsync(w => w.Name == warehousePayload.Name, TestContext.Current.CancellationToken);
        Assert.NotEqual(id, persisted.Id);
        Assert.NotEqual(Guid.Empty, persisted.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123456789012345678901234567890123")]
    public async Task WarehouseEndpoints_POST_InvalidName_ReturnsValidationProblemWithoutSaving(string? name)
    {
        using var client = CreateAuthorizedClient();
        await using var context = _factory.CreateContext();
        var countBefore = await context.Warehouse.CountAsync(TestContext.Current.CancellationToken);

        using var response = await client.PostAsJsonAsync("api/v1/Warehouse", new { Name = name }, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.Contains(problem.Errors.Keys, key => key.EndsWith("Name", StringComparison.OrdinalIgnoreCase));
        Assert.Equal(countBefore, await context.Warehouse.CountAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task WarehouseEndpoints_POST_DuplicateGeneratedId_ReturnsConflictWithoutChangingWarehouse()
    {
        using var factory = _factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IIdGenerator>();
            services.AddSingleton<IIdGenerator>(new DuplicateWarehouseIdGenerator());
        }));
        using var client = CreateAuthorizedClient(factory.CreateClient());
        var command = new CreateWarehouseCommand { Name = "Replacement Warehouse" };

        using var response = await client.PostAsJsonAsync("api/v1/Warehouse", command, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);
        Assert.NotNull(problem);
        Assert.Equal(409, problem.Status);
        await using var context = _factory.CreateContext();
        var warehouse = await context.Warehouse.SingleAsync(w => w.Id == WarehouseEngineFactory.WarehouseId1, TestContext.Current.CancellationToken);
        Assert.Equal("Warehouse 1", warehouse.Name);
        Assert.False(await context.Warehouse.AnyAsync(w => w.Name == command.Name, TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task WarehouseEndpoints_List_Auth_ReturnsSeededWarehouses()
    {
        using var client = CreateAuthorizedClient();

        using var response = await client.GetAsync("api/v1/Warehouse/list", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var warehouses = await response.Content.ReadFromJsonAsync<List<WarehouseResponseDto>>(TestContext.Current.CancellationToken);
        Assert.NotNull(warehouses);
        Assert.Equal("Warehouse 1", Assert.Single(warehouses, w => w.Id == WarehouseEngineFactory.WarehouseId1).Name);
        Assert.Equal("Warehouse 2", Assert.Single(warehouses, w => w.Id == WarehouseEngineFactory.WarehouseId2).Name);
    }

    private HttpClient CreateAuthorizedClient(HttpClient? client = null)
    {
        var jwtService = new JwtService(Options.Create(new JwtConfiguration
        {
            Secret = "MyIntegrationTestSecr3!tIsSoSecr3t",
            ValidIssuer = "http://localhost",
            ValidAudience = "http://warehouse-api"
        }));
        client ??= _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtService.GetNewToken([]));
        return client;
    }

    private sealed class DuplicateWarehouseIdGenerator : IIdGenerator
    {
        public Guid NewId() => WarehouseEngineFactory.WarehouseId1;
    }
}
