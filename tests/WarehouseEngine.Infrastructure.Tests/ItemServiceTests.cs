using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Infrastructure.Tests;

[ExcludeFromCodeCoverage]
public class ItemServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    public ItemServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetItemByIdAsync_SingleItem_AddSingleItem()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = context.Database.BeginTransaction();

        const string newSku = "TestAddAsync";

        var item = new Item
        {
            Description = "Test",
            Sku = newSku
        };

        await context.AddAsync(item);
        await context.SaveChangesAsync();

        int expectedId = item.Id;
        context.ChangeTracker.Clear();

        var sut = new ItemService(context);
        Item result = await sut.GetByIdAsync(expectedId);

        context.ChangeTracker.Clear();
        Assert.Equal(expectedId, result.Id);
    }

    [Fact]
    public async Task AddAsync_SingleItem_AddSingleItem()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = context.Database.BeginTransaction();

        const string newSku = "TestAddAsync";

        var item = new Item
        {
            Description = "Test",
            Sku = newSku
        };

        var sut = new ItemService(context);
        await sut.AddAsync(item);

        context.ChangeTracker.Clear();
        Assert.Single(context.Item.Where(i => i.Sku == newSku));
    }

    [Fact]
    public async Task UpdateAsync_SingleItem_Fields()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = context.Database.BeginTransaction();

        const string newSku = "TestSku";

        Item item = await context.Item.SingleAsync(i => i.Id == 1);

        item.Sku = newSku;
        item.Description = "TestDescriptionUpdateAsync";
        item.IsActive = false;

        var sut = new ItemService(context);
        await sut.UpdateAsync(1, item);

        context.ChangeTracker.Clear();
        Assert.Equal("TestSku", item.Sku);
        Assert.Equal("TestDescriptionUpdateAsync", item.Description);
        Assert.False(item.IsActive);
    }
}
