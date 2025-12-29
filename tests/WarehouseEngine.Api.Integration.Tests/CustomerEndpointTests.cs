using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using WarehouseEngine.Api.Controllers;
using WarehouseEngine.Api.Integration.Tests.Factories;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Domain.Models.Auth;
using WarehouseEngine.Domain.ValueObjects;
using Xunit;

namespace WarehouseEngine.Api.Integration.Tests;

public class CustomerEndpointTests
{
    private readonly WarehouseEngineFactory _factory;

    public CustomerEndpointTests(WarehouseEngineFactory factory)
    {
        _factory = factory;
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to retrieve a customer
        When the request is unauthorized
        Then the response should have a 401 status code
        """)]
    public async Task CustomerController_GET_NoAuth_Unauthorized()
    {
        using var client = _factory.CreateClient();

        using var response = await client.GetAsync($"api/v1/Customer/{WarehouseEngineFactory.CustomerId1}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to retrieve a customer
        When the request is authorized
        And when the requested resource is not found
        Then the response should have a 404 status code
        """)]
    public async Task CustomerController_GET_Auth_NotFound()
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
        using var response = await client.GetAsync($"api/v1/Customer/{guid}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to retrieve a customer
        When the request is authorized
        And when the requested resource is found
        Then the response should have a 200 status code
        """)]
    public async Task CustomerController_GET_Auth_Found()
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

        using var response = await client.GetAsync($"api/v1/Customer/{WarehouseEngineFactory.CustomerId1}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var contentString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var deserializedCustomerDto = JsonSerializer.Deserialize<CustomerResponseDto>(contentString, JsonSerializerOptions.Web);
        Assert.NotNull(deserializedCustomerDto);

        Assert.Equal("Customer1", deserializedCustomerDto.Name);

        Assert.NotNull(deserializedCustomerDto.ShippingAddress);
        Assert.Equal("OK", deserializedCustomerDto.ShippingAddress.State);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to create a customer
        When the request is unauthorized
        Then the response should have a 401 status code
        """)]
    public async Task CustomerController_POST_NoAuth_Unauthorized()
    {
        using var client = _factory.CreateClient();

        var customerPayload = new PostCustomerDto
        {
            Name = "New Customer",
            ShippingAddress = new Address
            {
                Address1 = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "12345",
            }
        };

        using var customerContent = JsonContent.Create(customerPayload);

        using var response = await client.PostAsync($"api/v1/Customer", customerContent, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to create a customer
        When the request is authorized
        Then the response should have a 201 status code
        """)]
    public async Task CustomerController_POST_Auth_NotFound()
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

        var customerPayload = new PostCustomerDto
        {
            Name = "New Customer",
            ShippingAddress = new Address
            {
                Address1 = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "12345",
            }
        };

        using var customerContent = JsonContent.Create(customerPayload);
        using var response = await client.PostAsync($"api/v1/Customer", customerContent, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to create a customer
        When the request is authorized
        And the request has an id
        Then the response should have a 201 status code
        And the customer should be created with a different id
        """)]
    public async Task CustomerController_POST_Auth_WithId ()
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

        var customerPayload = new PostCustomerDto
        {
            Id = id,
            Name = "New Customer",
            ShippingAddress = new Address
            {
                Address1 = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "12345",
            }
        };

        using var customerContent = JsonContent.Create(customerPayload);
        using var response = await client.PostAsync($"api/v1/Customer", customerContent, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var getResponse = await client.GetAsync($"api/v1/Customer/{id}", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact(DisplayName = $"""
        Given the client sends a request to create a customer
        When the request is authorized
        And when the customer has no shipping address
        Then the response should have a 400 status code
        """)]
    public async Task CustomerController_POST_NoShipping()
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

        var customerPayload = new PostCustomerDto
        {
            Name = "New Customer",
            ShippingAddress = null!
        };

        using var customerContent = JsonContent.Create(customerPayload);
        using var response = await client.PostAsync($"api/v1/Customer", customerContent, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
