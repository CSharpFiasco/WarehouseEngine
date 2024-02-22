using System.Diagnostics.CodeAnalysis;
using WarehouseEngine.Domain.ValueObjects;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Infrastructure.Tests;

[ExcludeFromCodeCoverage]
public class CustomerServiceTests : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture _fixture;
    public CustomerServiceTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetByIdAsync_SingleCustomer_ReturnsSingleCustomer()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = context.Database.BeginTransaction();

        const string newSku = "TestName";

        Customer customer = new()
        {
            Name = newSku,
            ShippingAddress = new Address { Address1 = "test1", Address2 = "test2", City = "test3", State = "TE", ZipCode = "test5" },
            DateCreated = DateTime.UtcNow
        };

        await context.AddAsync(customer);
        await context.SaveChangesAsync();

        var expectedId = customer.Id;
        context.ChangeTracker.Clear();

        var sut = new CustomerService(context);
        var result = await sut.GetByIdAsync(expectedId);

        context.ChangeTracker.Clear();
        Assert.Equal(expectedId, result.Id);
    }

    [Fact]
    public async Task AddAsync_SingleCustomer_BillingAddressIsMissing_DoesNotThrow()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = context.Database.BeginTransaction();

        const string newSku = "TestName";

        Customer customer = new()
        {
            Id = 0,
            Name = newSku,
            BillingAddress = null,
            ShippingAddress = new Address { Address1 = "test1", Address2 = "test2", City = "test3", State = "TE", ZipCode = "test5" },
            DateCreated = DateTime.UtcNow
        };

        var sut = new CustomerService(context);
        await sut.AddAsync(customer);

        context.ChangeTracker.Clear();
        Assert.NotEqual(0, customer.Id);
    }

    [Fact]
    public async Task AddAsync_SingleCustomer_ShippingAddressIsMissing_ThrowsException()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = context.Database.BeginTransaction();

        const string newSku = "TestName";

        Customer customer = new()
        {
            Name = newSku,
            BillingAddress = new Address { Address1 = "test1", Address2 = "test2", City = "test3", State = "test4", ZipCode = "test5" },
            ShippingAddress = null!,
            DateCreated = DateTime.UtcNow
        };

        var sut = new CustomerService(context);
        await Assert.ThrowsAnyAsync<Exception>(() => sut.AddAsync(customer));
    }
}
