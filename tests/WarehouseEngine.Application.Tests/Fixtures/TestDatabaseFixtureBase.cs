using System.Runtime.InteropServices;
using WarehouseEngine.Domain.ValueObjects;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Application.Tests.Fixtures;

public sealed class TestDatabaseFixture
{
    /*
     * TODO: Pull from environment variables against an instance of SQL Server 2022
     * VS 2022 uses SQL Serveer 2019
     */
    private readonly string ConnectionString = """Server=(localdb)\mssqllocaldb;Database=WarehouseTestDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;Application Name="Warehouse Engine Integration Tests";""";

    private static readonly Lock _lock = new();
    private static bool _databaseInitialized = false;

    public static readonly Guid ItemId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public static readonly Guid ItemId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");

    public static readonly Guid CustomerId1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid CustomerId2 = Guid.Parse("10000000-0000-0000-0000-000000000002");

    public TestDatabaseFixture()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            ConnectionString = "Data Source=.\\warehousetest.db";
        }

        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using WarehouseEngineContext context = CreateContext();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                SeedDatabase(context);
                context.SaveChanges();

                _databaseInitialized = true;
            }
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
    ///     When we test infrastructure, we test against Visual Studio's SQL Server 2019 instance.
    /// </summary>
    public WarehouseEngineContext CreateContext()
    {
        var builder = new DbContextOptionsBuilder<WarehouseEngineContext>();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            builder.UseSqlite(ConnectionString);
        }
        else
        {
            builder.UseSqlServer(ConnectionString);
        }

        return new WarehouseEngineContext(builder.Options);
    }

}
