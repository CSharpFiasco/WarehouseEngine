using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Moq;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.ValueObjects;
using WarehouseEngine.Infrastructure.DataContext;
using Xunit;

namespace WarehouseEngine.Infrastructure.Tests;

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
        await using var _ = await context.Database.BeginTransactionAsync();

        const string newSku = "TestName";
        Guid testId = Guid.NewGuid();

        Customer customer = new()
        {
            Id = testId,
            Name = newSku,
            ShippingAddress = new Address { Address1 = "test1", Address2 = "test2", City = "test3", State = "TE", ZipCode = "test5" },
            DateCreated = DateTime.UtcNow,
            CreatedBy = "TestUser"
        };

        await context.AddAsync(customer);
        await context.SaveChangesAsync();

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
        await using var _ = await context.Database.BeginTransactionAsync();

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
        var result = await context.Customer.FirstOrDefaultAsync(e => e.Id == testId);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddAsync_SingleCustomer_ShippingAddressIsMissing_ThrowsException()
    {
        await using WarehouseEngineContext context = _fixture.CreateContext();
        await using var _ = await context.Database.BeginTransactionAsync();

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
