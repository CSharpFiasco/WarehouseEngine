using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Infrastructure.Data;

namespace WarehouseEngine.Infrastructure.Tests.Helpers;
public class TestDatabaseFixture
{
    private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

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
                    context.AddRange(
                    new Item { Description = "Test", Sku = "Sku1" },
                        new Item { Description = "Test", Sku = "Sku2" }

                        );
                    context.SaveChanges();
                }

                _databaseInitialized = true;
            }
        }
    }

    public WarehouseEngineContext CreateContext()
        => new WarehouseEngineContext(
            new DbContextOptionsBuilder<WarehouseEngineContext>()
                .UseSqlServer(ConnectionString)
                .Options);
}
