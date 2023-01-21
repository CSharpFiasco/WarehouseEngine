namespace WarehouseEngine.Infrastructure.Tests;

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
        using var context = _fixture.CreateContext();
        context.Database.BeginTransaction();

        const string newSku = "TestAddAsync";

        var item = new Item
        {
            Description = "Test",
            Sku = newSku
        };

        await context.AddAsync(item);
        await context.SaveChangesAsync();

        var expectedId = item.Id;
        context.ChangeTracker.Clear();

        var sut = new ItemService(context);
        var result = await sut.GetByIdAsync(expectedId);

        context.ChangeTracker.Clear();
        Assert.Equal(expectedId, result.Id);
    }

    [Fact]
    public async Task AddAsync_SingleItem_AddSingleItem()
    {
        using var context = _fixture.CreateContext();
        context.Database.BeginTransaction();

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
}
