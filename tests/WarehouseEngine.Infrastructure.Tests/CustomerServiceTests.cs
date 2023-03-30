using System.Diagnostics.CodeAnalysis;
using WarehouseEngine.Domain.ValueObjects;

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
        await using var context = _fixture.CreateContext();
        await using var _ = context.Database.BeginTransaction();

        const string newSku = "TestName";

        var customer = new Customer
        {
            Name = newSku,
            ShippingAddress = new Address("test1", "test2", "test3", "test4", "test5")
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
        await using var context = _fixture.CreateContext();
        await using var _ = context.Database.BeginTransaction();

        const string newSku = "TestName";

        var customer = new Customer
        {
            Id = 0,
            Name = newSku,
            BillingAddress = null,
            ShippingAddress = new Address("test1", "test2", "test3", "test4", "test5")
        };

        var sut = new CustomerService(context);
        await sut.AddAsync(customer);

        context.ChangeTracker.Clear();
        Assert.NotEqual(0, customer.Id);
    }

    [Fact]
    public async Task AddAsync_SingleCustomer_ShippingAddressIsMissing_ThrowsException()
    {
        await using var context = _fixture.CreateContext();
        await using var _ = context.Database.BeginTransaction();

        const string newSku = "TestName";

        var customer = new Customer
        {
            Name = newSku,
            BillingAddress = new Address("test1", "test2", "test3", "test4", "test5"),
            ShippingAddress = null!
        };

        var sut = new CustomerService(context);
        await Assert.ThrowsAnyAsync<Exception>(() => sut.AddAsync(customer));
    }
}
