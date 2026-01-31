using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Application.Tests.Implementations;

[ExcludeFromCodeCoverage]
public class PositionServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    private readonly Mock<IIdGenerator> _idGenerator = new Mock<IIdGenerator>();

    public PositionServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdAsync_ExistingPosition_ReturnsPosition()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var positionId = Guid.NewGuid();
        var position = new Position
        {
            Id = positionId,
            Name = "Test Position"
        };

        await context.AddAsync(position, TestContext.Current.CancellationToken);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        context.ChangeTracker.Clear();

        var sut = new PositionService(context, _idGenerator.Object);
        var result = await sut.GetByIdAsync(positionId);

        context.ChangeTracker.Clear();
        var positionResult = Assert.IsType<PositionResponseDto>(result.Value);
        Assert.Equal(positionId, positionResult.Id);
        Assert.Equal("Test Position", positionResult.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingPosition_ReturnsEntityDoesNotExistResult()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var nonExistentId = Guid.NewGuid();

        var sut = new PositionService(context, _idGenerator.Object);
        var result = await sut.GetByIdAsync(nonExistentId);

        Assert.False(result.IsT0); // Not a PositionResponseDto
        Assert.True(result.IsT1);  // Is EntityDoesNotExistResult
    }

    [Fact]
    public async Task AddAsync_ValidPosition_AddsPosition()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        const string positionName = "New Test Position";
        var newId = Guid.NewGuid();

        var position = new PostPositionDto
        {
            Name = positionName
        };

        _idGenerator.Setup(g => g.NewId()).Returns(newId);

        var sut = new PositionService(context, _idGenerator.Object);
        var result = await sut.AddAsync(position);

        context.ChangeTracker.Clear();
        var positionResult = Assert.IsType<PositionResponseDto>(result.Value);
        Assert.Equal(newId, positionResult.Id);
        Assert.Equal(positionName, positionResult.Name);
        Assert.Single(context.Position.Where(p => p.Name == positionName));
    }

    [Fact]
    public async Task AddAsync_PositionWithExistingId_ThrowsInvalidOperationException()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var position = new PostPositionDto
        {
            Id = Guid.NewGuid(), // Pre-set ID should throw
            Name = "Test Position"
        };

        var sut = new PositionService(context, _idGenerator.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.AddAsync(position));
    }

    [Fact]
    public async Task GetCount_ReturnsCorrectCount()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        // Add some positions
        context.Position.AddRange(
            new Position { Id = Guid.NewGuid(), Name = "Position 1" },
            new Position { Id = Guid.NewGuid(), Name = "Position 2" },
            new Position { Id = Guid.NewGuid(), Name = "Position 3" }
        );
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        context.ChangeTracker.Clear();

        var sut = new PositionService(context, _idGenerator.Object);
        var count = await sut.GetCount();

        Assert.Equal(3, count);
    }
}
