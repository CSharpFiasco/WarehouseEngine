using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WarehouseEngine.Api.Controllers;
using WarehouseEngine.Api.Integration.Tests.Factories;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Models.Auth;
using Xunit;

namespace WarehouseEngine.Api.Integration.Tests;

[Collection(nameof(DatabaseCollection))]
public class VendorEndpointTests
{
    private readonly WarehouseEngineFactory _factory;

    public VendorEndpointTests(WarehouseEngineFactory factory)
    {
        _factory = factory;
    }

    [Fact(DisplayName = $"""
        Given a request to the {nameof(VendorController)}
        When the request is unauthorized
        Then the response should have a 401 status code
        """)]
    public async Task VendorController_NoAuth_Unauthorized()
    {
        // Arrange
        using var client = _factory.CreateClient();
        // Act
        using var response = await client.GetAsync("api/v1/Vendor", TestContext.Current.CancellationToken);
        // Assert
        // response should have 401 status code
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given a request to the {nameof(VendorController)}
        When the request is authorized
        And when the requested resource is not found
        Then the response should have a 404 status code
        """)]
    public async Task VendorController_Auth_NotFound()
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
        using var response = await client.GetAsync($"api/v1/Vendor?id={guid}", TestContext.Current.CancellationToken);
        // Assert
        // response should have 404 status code
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given a request to the {nameof(VendorController)}
        When the request is authorized
        And when the requested resource is found
        Then the response should have a 200 status code
        """)]
    public async Task VendorController_Auth_Found()
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
        using var response = await client.GetAsync($"api/v1/Vendor?id={WarehouseEngineFactory.VendorId1}", TestContext.Current.CancellationToken);
        // Assert
        // response should have 200 status code
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact(DisplayName = """
        Given a request to create a vendor
        When the request is authorized
        And when the vendor data is valid
        Then the response should have a 200 status code
        And the vendor should be created
        """)]
    public async Task VendorController_Create_Success()
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

        var newVendor = new PostVendorDto { Name = "New Test Vendor" };
        var json = JsonSerializer.Serialize(newVendor);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PostAsync("api/v1/Vendor", content, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var createdVendor = JsonSerializer.Deserialize<VendorResponseDto>(responseContent, JsonSerializerOptions.Web);

        Assert.NotNull(createdVendor);
        Assert.Equal("New Test Vendor", createdVendor.Name);
        Assert.NotEqual(Guid.Empty, createdVendor.Id);
    }

    [Fact(DisplayName = """
        Given a request to delete a vendor
        When the request is authorized
        And when the vendor exists
        Then the response should have a 204 status code
        """)]
    public async Task VendorController_Delete_Success()
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
        using var response = await client.DeleteAsync($"api/v1/Vendor?id={WarehouseEngineFactory.VendorId2}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact(DisplayName = """
        Given a request to get vendor count
        When the request is authorized
        Then the response should have a 200 status code
        And return the count
        """)]
    public async Task VendorController_Count_Success()
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
        using var response = await client.GetAsync("api/v1/Vendor/count", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var count = JsonSerializer.Deserialize<int>(responseContent);

        Assert.True(count >= 0);
    }

    [Fact(DisplayName = """
        Given a request to get all vendors
        When the request is authorized
        Then the response should have a 200 status code
        And return the vendor list
        """)]
    public async Task VendorController_GetAll_Success()
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
        using var response = await client.GetAsync("api/v1/Vendor/list", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var vendors = JsonSerializer.Deserialize<VendorResponseDto[]>(responseContent, JsonSerializerOptions.Web);

        Assert.NotNull(vendors);
        Assert.True(vendors.Length >= 0);
    }

    [Fact(DisplayName = """
        Given a request to update a vendor
        When the request is authorized
        And when the vendor exists
        Then the response should have a 200 status code
        And the vendor should be updated
        """)]
    public async Task VendorController_Update_Success()
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

        var updatedVendor = new Vendor { Id = WarehouseEngineFactory.VendorId1, Name = "Updated Vendor Name" };
        var json = JsonSerializer.Serialize(updatedVendor);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        using var response = await client.PutAsync($"api/v1/Vendor/{WarehouseEngineFactory.VendorId1}", content, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var result = JsonSerializer.Deserialize<VendorResponseDto>(responseContent, JsonSerializerOptions.Web);

        Assert.NotNull(result);
        Assert.Equal("Updated Vendor Name", result.Name);
        Assert.Equal(WarehouseEngineFactory.VendorId1, result.Id);
    }

    [Fact(DisplayName = """
        Given a request to get OpenAPI specification
        When the request is made to openapi/v1.json
        Then the response should have a 200 status code
        And return the OpenAPI JSON document
        """)]
    public async Task OpenApi_V1Json_Success()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        using var response = await client.GetAsync("openapi/v1.json", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.NotNull(responseContent);
        Assert.NotEmpty(responseContent);
        
        // Verify it's valid JSON by deserializing
        var openApiDoc = JsonSerializer.Deserialize<JsonElement>(responseContent);
        Assert.True(openApiDoc.ValueKind == JsonValueKind.Object);
    }
}
