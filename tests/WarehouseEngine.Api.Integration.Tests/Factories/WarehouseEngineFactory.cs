using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WarehouseEngine.Api.Integration.Tests.Factories;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValueObjects;
using WarehouseEngine.Infrastructure.DataContext;
using Xunit;

[assembly: AssemblyFixture(typeof(WarehouseEngineFactory))]

namespace WarehouseEngine.Api.Integration.Tests.Factories;

public sealed class WarehouseEngineFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestDatabaseManager _databaseManager = new(SeedDatabase);

    public string ConnectionString => _databaseManager.ConnectionString;

    public static readonly Guid ItemId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public static readonly Guid ItemId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
    public static readonly Guid CustomerId1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid CustomerId2 = Guid.Parse("10000000-0000-0000-0000-000000000002");
    public static readonly Guid VendorId1 = Guid.Parse("20000000-0000-0000-0000-000000000001");
    public static readonly Guid VendorId2 = Guid.Parse("20000000-0000-0000-0000-000000000002");
    public static readonly Guid PositionId1 = Guid.Parse("30000000-0000-0000-0000-000000000001");
    public static readonly Guid PositionId2 = Guid.Parse("30000000-0000-0000-0000-000000000002");
    public static readonly Guid WarehouseId1 = Guid.Parse("40000000-0000-0000-0000-000000000001");
    public static readonly Guid WarehouseId2 = Guid.Parse("40000000-0000-0000-0000-000000000002");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configurationValues = new Dictionary<string, string?>
        {
            { "ConnectionStrings:WarehouseEngine", ConnectionString },
            { "JwtConfiguration:ValidAudience", "http://warehouse-api"},
            { "JwtConfiguration:ValidIssuer", "http://localhost" },
            { "JwtConfiguration:Secret", "MyIntegrationTestSecr3!tIsSoSecr3t" }
    };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationValues)
            .Build();

        builder
            .UseConfiguration(configuration)
            .ConfigureAppConfiguration((context, config) => { })
            .UseEnvironment("Integration");
    }

    public async ValueTask InitializeAsync() => await _databaseManager.InitializeAsync();

    /// <inheritdoc/>
    async ValueTask IAsyncDisposable.DisposeAsync() => await _databaseManager.DisposeAsync();

    public WarehouseEngineContext CreateContext() => _databaseManager.CreateContext();

    private static void SeedDatabase(WarehouseEngineContext context)
    {
        context.Item.AddRange(
            Item.Create(ItemId1, "Sku1", "Test"),
            Item.Create(ItemId2, "Sku2", "Test")
        );

        context.Customer.AddRange(
            Customer.Create(CustomerId1, "Customer1",
                new Address
                {
                    Address1 = "123 Main St",
                    City = "Tulsa",
                    State = "OK",
                    ZipCode = "74101"
                }, DateTime.MinValue, "Seed"),
            Customer.Create(CustomerId2, "Customer2",
                new Address
                {
                    Address1 = "456 Main St",
                    City = "Tulsa",
                    State = "OK",
                    ZipCode = "74101"
                }, DateTime.MinValue, "Seed")
        );

        context.Vendor.AddRange(
            Vendor.Create(VendorId1, "Test Vendor 1"),
            Vendor.Create(VendorId2, "Test Vendor 2")
        );

        context.Position.AddRange(
            Position.Create(PositionId1, "Test Position 1"),
            Position.Create(PositionId2, "Test Position 2")
        );

        context.Warehouse.AddRange(
            Warehouse.Create(WarehouseId1, "Warehouse 1"),
            Warehouse.Create(WarehouseId2, "Warehouse 2")
        );
    }
}
