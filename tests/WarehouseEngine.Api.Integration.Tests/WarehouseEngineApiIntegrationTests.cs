﻿using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using WarehouseEngine.Api.Integration.Tests.Factories;
using WarehouseEngine.Application.Implementations;
using WarehouseEngine.Domain.Models.Auth;
using Xunit;

namespace WarehouseEngine.Api.Integration.Tests;

[Collection(nameof(DatabaseCollection))]
public class WarehouseEngineApiIntegrationTests
{
    private readonly WarehouseEngineFactory _factory;

    public WarehouseEngineApiIntegrationTests(WarehouseEngineFactory factory)
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
        using var client = _factory.CreateClient();
        // Act
        using var response = await client.GetAsync("api/v1/Customer");
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
    public async Task CustomerController_Auth_NotFound()
    {
        var options = Options.Create<JwtConfiguration>(new JwtConfiguration
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
        using var response = await client.GetAsync($"api/v1/Customer?id={guid}");
        // Assert
        // response should have 200 status code
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = """
        Given a request to the CustomerController
        When the request is authorized
        And when the requested resource is found
        Then the response should have a 200 status code
        """)]
    public async Task CustomerController_Auth_Found()
    {
        var options = Options.Create<JwtConfiguration>(new JwtConfiguration
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
        using var response = await client.GetAsync($"api/v1/Customer?id={WarehouseEngineFactory.CustomerId1}");
        // Assert
        // response should have 200 status code
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}
