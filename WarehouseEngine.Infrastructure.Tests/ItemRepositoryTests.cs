using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Core.Entities;
using WarehouseEngine.Infrastructure.Data;
using WarehouseEngine.Infrastructure.Implementations;

namespace WarehouseEngine.Infrastructure.Tests;

public class ItemRepositoryTests
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<WarehouseEngineContext> _contextOptions;
    public ItemRepositoryTests()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        _contextOptions = new DbContextOptionsBuilder<WarehouseEngineContext>()
            .UseSqlite(_connection)
            .Options;
    }
    [Fact]
    public async Task AddAsync()
    {
        using var context = CreateContext();
        context.Database.EnsureCreated();
        var item = new Item
        {
            Description = "Test",
            Sku = "Sku1"
        };

        var sut = new ItemRepository(context);

        await sut.AddAsync(item);
        await context.SaveChangesAsync();

        Assert.NotEmpty(context.Item);

    }

    WarehouseEngineContext CreateContext() => new WarehouseEngineContext(_contextOptions);

    private void Dispose() => _connection.Dispose();
}
