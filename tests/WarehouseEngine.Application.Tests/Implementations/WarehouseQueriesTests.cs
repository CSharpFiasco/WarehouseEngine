using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Application.Tests.Implementations;

[ExcludeFromCodeCoverage]
public class WarehouseQueriesTests(TestDatabaseFixture fixture) : IClassFixture<TestDatabaseFixture>
{
    [Fact]
    public async Task GetByIdAsync_ExistingWarehouse_ReturnsProjectionWithoutTracking()
    {
        await using var context = fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);
        var id = Guid.NewGuid();
        context.Warehouse.Add(Warehouse.Create(id, "Test Warehouse"));
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        context.ChangeTracker.Clear();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

        var result = await new WarehouseQueries(context).GetByIdAsync(id, TestContext.Current.CancellationToken);

        var response = Assert.IsType<WarehouseResponseDto>(result.Value);
        Assert.Equal(id, response.Id);
        Assert.Equal("Test Warehouse", response.Name);
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task GetByIdAsync_MissingWarehouse_ReturnsNotFound()
    {
        await using var context = fixture.CreateContext();

        var result = await new WarehouseQueries(context).GetByIdAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        Assert.IsType<EntityDoesNotExist>(result.Value);
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task GetAllAsync_AndGetCount_ReturnPersistedWarehousesWithoutTracking()
    {
        await using var context = fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);
        var queries = new WarehouseQueries(context);
        var originalWarehouses = (await queries.GetAllAsync(TestContext.Current.CancellationToken)).ToList();
        var firstId = Guid.NewGuid();
        var secondId = Guid.NewGuid();
        context.Warehouse.AddRange(Warehouse.Create(firstId, "First Warehouse"), Warehouse.Create(secondId, "Second Warehouse"));
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);
        context.ChangeTracker.Clear();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

        var warehouses = (await queries.GetAllAsync(TestContext.Current.CancellationToken)).ToList();
        var count = await queries.GetCount(TestContext.Current.CancellationToken);

        Assert.Equal(originalWarehouses.Count + 2, count);
        Assert.Equal(count, warehouses.Count);
        Assert.Equal("First Warehouse", Assert.Single(warehouses, w => w.Id == firstId).Name);
        Assert.Equal("Second Warehouse", Assert.Single(warehouses, w => w.Id == secondId).Name);
        Assert.All(originalWarehouses, original => Assert.Equal(original.Name, Assert.Single(warehouses, w => w.Id == original.Id).Name));
        Assert.Empty(context.ChangeTracker.Entries());
    }

    [Fact]
    public async Task Queries_CanceledToken_PropagateCancellation()
    {
        await using var context = fixture.CreateContext();
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();
        var queries = new WarehouseQueries(context);

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => queries.GetByIdAsync(Guid.NewGuid(), cancellation.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => queries.GetAllAsync(cancellation.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => queries.GetCount(cancellation.Token));
        Assert.Empty(context.ChangeTracker.Entries());
    }
}
