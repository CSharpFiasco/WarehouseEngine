using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.ValueObjects;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Application.Tests.Implementations;

[ExcludeFromCodeCoverage]
public class CustomerServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    private readonly Mock<IIdGenerator> _idGenerator = new Mock<IIdGenerator>();
    public CustomerServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdAsync_SingleCustomer_ReturnsSingleCustomer()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        const string newSku = "TestName";
        Guid testId = Guid.NewGuid();

        var customer = Customer.Create(testId, newSku,
            new Address { Address1 = "test1", Address2 = "test2", City = "test3", State = "TE", ZipCode = "test5" },
            DateTime.UtcNow, "TestUser");

        await context.AddAsync(customer, TestContext.Current.CancellationToken);
        await context.SaveChangesAsync(TestContext.Current.CancellationToken);

        context.ChangeTracker.Clear();

        var sut = new CustomerService(context, _idGenerator.Object);
        var result = await sut.GetByIdAsync(testId);

        context.ChangeTracker.Clear();

        var response = Assert.IsType<CustomerResponseDto>(result.Value);
        Assert.Equal(testId, response.Id);
    }

    [Fact]
    public async Task AddAsync_SingleCustomer_BillingAddressIsMissing_DoesNotThrow()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        const string newSku = "TestName";
        Guid testId = Guid.NewGuid();
        _idGenerator.Setup(e => e.NewId()).Returns(testId);

        PostCustomerDto customer = new()
        {
            Name = newSku,
            BillingAddress = null,
            ShippingAddress = new Address { Address1 = "test1", Address2 = "test2", City = "test3", State = "TE", ZipCode = "test5" }
        };

        var sut = new CustomerService(context, _idGenerator.Object);
        await sut.AddAsync(customer, "TestUser");

        context.ChangeTracker.Clear();
        var result = await context.Customer.FirstOrDefaultAsync(e => e.Id == testId, TestContext.Current.CancellationToken);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateAsync_ValidDetails_PersistsAddressesAndPreservesCreationAudit()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var context = _fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var id = Guid.NewGuid();
        var shipping = new Address { Address1 = "123 Main St", City = "Tulsa", State = "OK", ZipCode = "74101" };
        context.Customer.Add(Customer.Create(id, "Original", shipping, DateTime.UnixEpoch, "Creator", shipping with { }));
        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
        var changes = Customer.Create(id, "Changed", shipping with { City = "Dallas" }, DateTime.UtcNow, "Other");

        var result = await new CustomerService(context, _idGenerator.Object).UpdateAsync(id, changes);

        Assert.IsType<CustomerResponseDto>(result.Value);
        context.ChangeTracker.Clear();
        var saved = await context.Customer.SingleAsync(e => e.Id == id, cancellationToken);
        Assert.Equal("Changed", saved.Name);
        Assert.Equal("Dallas", saved.ShippingAddress.City);
        Assert.Null(saved.BillingAddress);
        Assert.Equal(DateTime.UnixEpoch, saved.DateCreated);
        Assert.Equal("Creator", saved.CreatedBy);
    }

    [Fact]
    public async Task AddAsync_SingleCustomer_ShippingAddressIsMissing_ThrowsException()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync(TestContext.Current.CancellationToken);

        const string newSku = "TestName";
        Guid testId = Guid.NewGuid();

        PostCustomerDto customer = new()
        {
            Id = testId,
            Name = newSku,
            BillingAddress = new Address { Address1 = "test1", Address2 = "test2", City = "test3", State = "test4", ZipCode = "test5" },
            ShippingAddress = null!,
            DateCreated = DateTime.UtcNow,
            CreatedBy = "TestUser"
        };

        var sut = new CustomerService(context, _idGenerator.Object);
        await Assert.ThrowsAsync<InvalidOperationException>(async () => await sut.AddAsync(customer, string.Empty));
    }
}
