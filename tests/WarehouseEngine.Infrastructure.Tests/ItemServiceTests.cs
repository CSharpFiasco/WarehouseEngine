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
        await using var _ = await context.Database.BeginTransactionAsync();

        const string newSku = "TestAddAsync";

        Guid newGuidId = Guid.NewGuid();

        var item = new Item
        {
            Id = newGuidId,
            Description = "Test",
            Sku = newSku
        };

        await context.AddAsync(item);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var sut = new ItemService(context);
        Item result = await sut.GetByIdAsync(newGuidId);

        context.ChangeTracker.Clear();
        Assert.Equal(newGuidId, result.Id);
    }

    [Fact]
    public async Task AddAsync_SingleItem_AddSingleItem()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync();

        const string newSku = "TestAddAsync";

        var item = new Item
        {
            Id = Guid.Empty,
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
        await using var _ = await context.Database.BeginTransactionAsync();

        const string newSku = "TestSku";

        Item item = await context.Item.SingleAsync(i => i.Id == TestDatabaseFixture.ItemId1);

        item.Sku = newSku;
        item.Description = "TestDescriptionUpdateAsync";
        item.IsActive = false;

        var sut = new ItemService(context);
        await sut.UpdateAsync(TestDatabaseFixture.ItemId1, item);

        context.ChangeTracker.Clear();
        Assert.Equal("TestSku", item.Sku);
        Assert.Equal("TestDescriptionUpdateAsync", item.Description);
        Assert.False(item.IsActive);
    }
}
