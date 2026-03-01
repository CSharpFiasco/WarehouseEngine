using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Application.Tests.Implementations;

[ExcludeFromCodeCoverage]
public class WarehouseServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    private readonly Mock<IIdGenerator> _idGenerator = new Mock<IIdGenerator>();
    public WarehouseServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdAsync_SingleWarehouse_ReturnsSingleWarehouse()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        Guid testId = Guid.NewGuid();

        Warehouse warehouse = new()
        {
            Id = testId,
            Name = "Test Warehouse"
        };

        await context.AddAsync(warehouse, TestContext.Current.CancellationToken);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        context.ChangeTracker.Clear();

        var sut = new WarehouseService(context, _idGenerator.Object);
        var result = await sut.GetByIdAsync(testId);

        context.ChangeTracker.Clear();

        var response = Assert.IsType<WarehouseResponseDto>(result.Value);
        Assert.Equal(testId, response.Id);
    }

    [Fact]
    public async Task AddAsync_SingleWarehouse_ReturnsWarehouse()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        Guid testId = Guid.NewGuid();
        _idGenerator.Setup(e => e.NewId()).Returns(testId);

        PostWarehouseDto warehouse = new()
        {
            Name = "Test Warehouse"
        };

        var sut = new WarehouseService(context, _idGenerator.Object);
        await sut.AddAsync(warehouse);

        context.ChangeTracker.Clear();
        var result = await context.Warehouse.FirstOrDefaultAsync(e => e.Id == testId, TestContext.Current.CancellationToken);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddAsync_SingleWarehouse_IdAlreadySet_ThrowsException()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        Guid testId = Guid.NewGuid();

        PostWarehouseDto warehouse = new()
        {
            Id = testId,
            Name = "Test Warehouse"
        };

        var sut = new WarehouseService(context, _idGenerator.Object);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.AddAsync(warehouse));
    }
}
