using System.Runtime.InteropServices;
using WarehouseEngine.Domain.ValueObjects;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Application.Tests.Fixtures;

public sealed class TestDatabaseFixture
{
    private readonly DbContextOptions<WarehouseEngineContext> _options;

    private static readonly Lock _lock = new();
    private static bool _databaseInitialized = false;

    public static readonly Guid ItemId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public static readonly Guid ItemId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");

    public static readonly Guid CustomerId1 = Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static readonly Guid CustomerId2 = Guid.Parse("10000000-0000-0000-0000-000000000002");

    public TestDatabaseFixture()
    {
        _options = CreateOptions(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));

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
    }

    internal static DbContextOptions<WarehouseEngineContext> CreateOptions(string? connectionString)
    {
        var builder = new DbContextOptionsBuilder<WarehouseEngineContext>();

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            builder.UseSqlServer(connectionString);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            builder.UseSqlServer("""Server=(localdb)\mssqllocaldb;Database=WarehouseTestDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;Application Name="Warehouse Engine Integration Tests";""");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            builder.UseSqlite("Data Source=.\\warehousetest.db");
        }
        else
        {
            throw new InvalidOperationException(
                "Set ConnectionStrings__DefaultConnection to a SQL Server connection string for a dedicated test database on this platform.");
        }

        return builder.Options;
    }

    public WarehouseEngineContext CreateContext()
    {
        return new WarehouseEngineContext(_options);
    }

}
