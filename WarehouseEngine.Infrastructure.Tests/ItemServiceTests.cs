using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Infrastructure.Implementations;
using WarehouseEngine.Infrastructure.Tests.Helpers;

namespace WarehouseEngine.Infrastructure.Tests;

public class ItemServiceTests : IClassFixture<TestDatabaseFixture>
{
    public ItemServiceTests(TestDatabaseFixture fixture)
    => Fixture = fixture;
    public TestDatabaseFixture Fixture { get; }

    [Fact]
    public async Task AddAsync_SingleItem_AddsSingleItem()
    {
        using var context = Fixture.CreateContext();
        context.Database.EnsureCreated();

        var newSku = "TestAddAsync";

        var item = new Item
        {
            Description = "Test",
            Sku = newSku
        };

        var sut = new ItemService(context);
        await sut.AddAsync(item);

        Assert.NotEmpty(context.Item.Where(i => i.Sku == newSku));
    }
}
