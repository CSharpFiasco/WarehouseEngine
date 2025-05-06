using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValueObjects;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Api.Integration.Tests.Factories;

[CollectionDefinition("Database collection")]
public sealed class DatabaseCollection : ICollectionFixture<WarehouseEngineFactory<Program>>
{
    // Per https://xunit.net/docs/shared-context#collection-fixture

    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class WarehouseEngineFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    // https://dotnet.testcontainers.org/modules/mssql/
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        // Pinning image to a specific version to avoid breaking changes
        .WithImage("mcr.microsoft.com/mssql/server:2019-CU28-ubuntu-20.04")
        // Adding wait strategy due to recent runner issues: https://github.com/actions/runner-images/issues/10649
        .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd", "-C", "-Q", "SELECT 1;"))
        .WithPortBinding(54965, true)
        .Build();

    // We add the database when the connection string is being consumed
    // https://github.com/testcontainers/testcontainers-dotnet/issues/986#issuecomment-1698807027
    public string ConnectionString => _msSqlContainer.GetConnectionString() + ";Initial Catalog=WarehouseEngineTest";
    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    private static bool _databaseInitialized = false;

    public static readonly Guid ItemId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public static readonly Guid ItemId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");

    public static readonly Guid CustomerId1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid CustomerId2 = Guid.Parse("10000000-0000-0000-0000-000000000002");

    public WarehouseEngineFactory()
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // set EnvironmentName as integration rather than development
        var i = 1;

        var configurationValues = new Dictionary<string, string>
        {
            { "ConnectionStrings:WarehouseEngine", ConnectionString }
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationValues)
            .Build();

        builder
        .UseConfiguration(configuration)
        .ConfigureAppConfiguration((context, config) =>
        {
            context.Configuration["ConnectionStrings:WarehouseEngine"] = ConnectionString;

            builder.ConfigureServices(services =>
            {
                //// Remove the existing WarehouseEngineContext registration
                //var dbContextDescriptor = services.SingleOrDefault(
                //    d => d.ServiceType ==
                //        typeof(DbContextOptions<WarehouseEngineContext>));

                //ArgumentNullException.ThrowIfNull(dbContextDescriptor);

                //services.Remove(dbContextDescriptor);
            });
        });

        builder.UseEnvironment("Integration");
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        await _lock.WaitAsync(TimeSpan.FromMicroseconds(1_000));
        try
        {
            if (!_databaseInitialized)
            {
                using WarehouseEngineContext context = CreateContext();

                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                SeedDatabase(context);
                await context.SaveChangesAsync();

                _databaseInitialized = true;
            }
        }
        finally
        {
            _lock.Release();
        }
    }

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
    }

    /// <summary>
    ///     When we test infrastructure, we test against the database spun up by the testcontainer
    /// </summary>
    public WarehouseEngineContext CreateContext()
    {
        var builder = new DbContextOptionsBuilder<WarehouseEngineContext>();

        builder.UseSqlServer(ConnectionString);

        return new WarehouseEngineContext(builder.Options);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }
}
