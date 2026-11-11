using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WarehouseEngine.Api.Integration.Tests.Factories;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValueObjects;
using Xunit;

namespace WarehouseEngine.Api.Integration.Tests;

public class EntityValidationEndpointTests(WarehouseEngineFactory factory)
{
    [Theory]
    [InlineData("customer", "name", 81)]
    [InlineData("customer", "name", -1)]
    [InlineData("customer", "name", -2)]
    [InlineData("item", "sku", 13)]
    [InlineData("item", "sku", -1)]
    [InlineData("item", "sku", -2)]
    [InlineData("item", "description", 121)]
    [InlineData("item", "description", -1)]
    [InlineData("item", "description", -2)]
    [InlineData("position", "name", 81)]
    [InlineData("position", "name", -1)]
    [InlineData("position", "name", -2)]
    [InlineData("vendor", "name", 81)]
    public async Task Create_InvalidText_ReturnsValidationProblemWithoutSaving(string resource, string field, int length)
    {
        var id = Guid.NewGuid();
        using var configuredFactory = factory.WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IIdGenerator>();
            services.AddSingleton<IIdGenerator>(new FixedIdGenerator(id));
        }));
        using var client = configuredFactory.CreateClient();
        var jwtService = configuredFactory.Services.GetRequiredService<IJwtService>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtService.GetNewToken([]));
        var payload = new Dictionary<string, object?>
        {
            ["name"] = "Validation test",
            ["sku"] = "VALID",
            ["description"] = "Valid description",
            ["shippingAddress"] = new Address { Address1 = "123 Main St", City = "Tulsa", State = "OK", ZipCode = "74101" }
        };
        payload[field] = length switch { -1 => null, -2 => " ", _ => new string('X', length) };

        using var response = await client.PostAsJsonAsync($"api/v1/{resource}", payload, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.Contains(problem.Errors.Keys, key => key.EndsWith(field, StringComparison.OrdinalIgnoreCase));
        var entityType = resource switch
        {
            "customer" => typeof(Customer),
            "item" => typeof(Item),
            "position" => typeof(Position),
            "vendor" => typeof(Vendor),
            _ => throw new ArgumentOutOfRangeException(nameof(resource))
        };
        await using var context = factory.CreateContext();
        Assert.Null(await context.FindAsync(entityType, [id], TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task UpdateVendor_EmptyRouteIdentity_PreservesNotFoundResponse()
    {
        using var client = factory.CreateClient();
        var jwtService = factory.Services.GetRequiredService<IJwtService>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtService.GetNewToken([]));

        using var response = await client.PutAsJsonAsync($"api/v1/vendor/{Guid.Empty}",
            new { Name = "Changed" }, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateVendor_OverlongName_ReturnsValidationProblemWithoutChangingVendor()
    {
        using var client = factory.CreateClient();
        var jwtService = factory.Services.GetRequiredService<IJwtService>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtService.GetNewToken([]));
        await using var context = factory.CreateContext();
        var id = Guid.NewGuid();
        context.Vendor.Add(Vendor.Create(id, "Original"));
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        try
        {
            using var response = await client.PutAsJsonAsync($"api/v1/vendor/{id}",
                new { Name = new string('V', 81) }, TestContext.Current.CancellationToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(TestContext.Current.CancellationToken);
            Assert.NotNull(problem);
            Assert.Contains(problem.Errors.Keys, key => key.EndsWith("Name", StringComparison.OrdinalIgnoreCase));
            context.ChangeTracker.Clear();
            Assert.Equal("Original", (await context.Vendor.FindAsync([id], TestContext.Current.CancellationToken))!.Name);
        }
        finally
        {
            context.Vendor.Remove((await context.Vendor.FindAsync([id], TestContext.Current.CancellationToken))!);
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        }
    }

    private sealed class FixedIdGenerator(Guid id) : IIdGenerator
    {
        public Guid NewId() => id;
    }
}
