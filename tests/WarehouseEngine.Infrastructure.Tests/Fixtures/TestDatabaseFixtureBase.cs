using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Infrastructure.Tests.Fixtures;

public sealed class TestDatabaseFixture
{
    private readonly string ConnectionString = @$"Server=(localdb)\mssqllocaldb;Database=WarehouseTestDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

    private static readonly object _lock = new();
    private static bool _databaseInitialized = false;

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = CreateContext())
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    SeedDatabase(context);
                    context.SaveChanges();
                }

                _databaseInitialized = true;
            }
        }
    }

    private static void SeedDatabase(WarehouseEngineContext context)
    {
        context.Item.AddRange(
            new Item { Description = "Test", Sku = "Sku1" },
            new Item { Description = "Test", Sku = "Sku2" }
            );
    }

    public WarehouseEngineContext CreateContext()
        => new WarehouseEngineContext(
            new DbContextOptionsBuilder<WarehouseEngineContext>()
                .UseSqlServer(ConnectionString)
                .Options);

}
