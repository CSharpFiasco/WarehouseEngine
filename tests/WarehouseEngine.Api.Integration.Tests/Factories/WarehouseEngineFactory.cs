using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValueObjects;
using WarehouseEngine.Infrastructure.DataContext;
using Xunit;

namespace WarehouseEngine.Api.Integration.Tests.Factories;

[CollectionDefinition(nameof(DatabaseCollection))]
public sealed class DatabaseCollection : ICollectionFixture<WarehouseEngineFactory>
{
    // Per https://xunit.net/docs/shared-context#collection-fixture

    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

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
            new Item { Id = ItemId1, Description = "Test", Sku = "Sku1" },
            new Item { Id = ItemId2, Description = "Test", Sku = "Sku2" }
        );

        context.Customer.AddRange(
            new Customer
            {
                Id = CustomerId1,
                Name = string.Empty,
                DateCreated = DateTime.MinValue,
                CreatedBy = string.Empty,
                ShippingAddress = new Address
                {
                    Address1 = string.Empty,
                    City = string.Empty,
                    State = "OK",
                    ZipCode = string.Empty
                }
            },
            new Customer
            {
                Id = CustomerId2,
                Name = string.Empty,
                DateCreated = DateTime.MinValue,
                CreatedBy = string.Empty,
                ShippingAddress = new Address
                {
                    Address1 = string.Empty,
                    City = string.Empty,
                    State = "OK",
                    ZipCode = string.Empty
                }
            }
        );

        context.Vendor.AddRange(
            new Vendor { Id = VendorId1, Name = "Test Vendor 1" },
            new Vendor { Id = VendorId2, Name = "Test Vendor 2" }
        );
    }
}
