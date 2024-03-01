using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Infrastructure.Tests.Fixtures;

public sealed class TestDatabaseFixture
{
    /*
     * TODO: Pull from environment variables against an instance of SQL Server 2022
     * VS 2022 uses SQL Serveer 2019
     */
    private readonly string ConnectionString = @$"Server=(localdb)\mssqllocaldb;Database=WarehouseTestDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;Application Name=""Warehouse Engine Integration Tests""";

    private static readonly object _lock = new();
    private static bool _databaseInitialized = false;

    public static readonly Guid ItemId1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
    public static readonly Guid ItemId2 = Guid.Parse("00000000-0000-0000-0000-000000000002");

    public TestDatabaseFixture()
    {
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
    }

    /// <summary>
    ///     When we test infrastructure, we test against Visual Studio's SQL Server 2019 instance.
    /// </summary>
    public WarehouseEngineContext CreateContext()
        => new WarehouseEngineContext(
            new DbContextOptionsBuilder<WarehouseEngineContext>()
                .UseSqlServer(ConnectionString)
                .Options);

}
