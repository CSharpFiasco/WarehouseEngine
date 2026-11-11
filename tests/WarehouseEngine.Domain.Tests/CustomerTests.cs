using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Tests;

public class CustomerTests
{
    private static Address ShippingAddress => new()
    {
        Address1 = "123 Main St",
        City = "Tulsa",
        State = "OK",
        ZipCode = "74101"
    };

    [Fact]
    public void Create_ValidInputs_PreservesIdentityAddressesAndAudit()
    {
        var id = Guid.NewGuid();
        var created = DateTime.UtcNow;
        var shipping = ShippingAddress;
        var billing = shipping with { Address1 = "456 Main St" };

        var customer = Customer.Create(id, " Customer ", shipping, created, "Creator", billing);

        Assert.Equal(id, customer.Id);
        Assert.Equal(" Customer ", customer.Name);
        Assert.Equal(created, customer.DateCreated);
        Assert.Equal("Creator", customer.CreatedBy);
        Assert.Same(shipping, customer.ShippingAddress);
        Assert.Same(billing, customer.BillingAddress);
        Assert.Null(customer.DateModified);
        Assert.Null(customer.ModifiedBy);
        Assert.Empty(customer.Contact);
        Assert.Empty(customer.Order);
    }

    [Fact]
    public void Create_OptionalBillingAddress_RemainsNull()
    {
        var customer = Customer.Create(Guid.NewGuid(), "Customer", ShippingAddress, DateTime.MinValue, "Creator");

        Assert.Null(customer.BillingAddress);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_MissingNameOrCreator_RejectsInput(string? text)
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            Customer.Create(Guid.NewGuid(), text!, ShippingAddress, DateTime.MinValue, "Creator"));
        Assert.ThrowsAny<ArgumentException>(() =>
            Customer.Create(Guid.NewGuid(), "Customer", ShippingAddress, DateTime.MinValue, text!));
    }

    [Fact]
    public void Create_NameAndCreatorLimits_AreEnforced()
    {
        var customer = Customer.Create(Guid.NewGuid(), new string('N', 80), ShippingAddress,
            DateTime.MinValue, new string('U', 80));
        Assert.Equal(80, customer.Name.Length);
        Assert.Equal(80, customer.CreatedBy.Length);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Customer.Create(Guid.NewGuid(), new string('N', 81), ShippingAddress, DateTime.MinValue, "Creator"));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Customer.Create(Guid.NewGuid(), "Customer", ShippingAddress, DateTime.MinValue, new string('U', 81)));
    }

    [Fact]
    public void Create_InvalidAddresses_RejectsInput()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Customer.Create(Guid.NewGuid(), "Customer", null!, DateTime.MinValue, "Creator"));
        Assert.Throws<ArgumentException>(() =>
            Customer.Create(Guid.NewGuid(), "Customer", ShippingAddress with { City = " " }, DateTime.MinValue, "Creator"));
        Assert.Throws<ArgumentException>(() =>
            Customer.Create(Guid.NewGuid(), "Customer", ShippingAddress, DateTime.MinValue, "Creator",
                ShippingAddress with { State = "Invalid" }));
    }

    [Fact]
    public void UpdateDetails_InvalidProposedAddress_LeavesAllFieldsUnchanged()
    {
        var shipping = ShippingAddress;
        var customer = Customer.Create(Guid.NewGuid(), "Original", shipping, DateTime.MinValue, "Creator", shipping);

        Assert.Throws<ArgumentException>(() =>
            customer.UpdateDetails("Changed", shipping with { ZipCode = "" }, shipping with { City = "Changed" }));

        Assert.Equal("Original", customer.Name);
        Assert.Same(shipping, customer.BillingAddress);
        Assert.Same(shipping, customer.ShippingAddress);
    }

    [Fact]
    public void UpdateDetails_ValidChange_PreservesIdentityAndCreationAudit()
    {
        var customer = Customer.Create(Guid.NewGuid(), "Original", ShippingAddress, DateTime.MinValue, "Creator");
        var id = customer.Id;
        var shipping = ShippingAddress with { City = "Dallas" };

        customer.UpdateDetails("Changed", null, shipping);
        customer.RecordModification(DateTime.UnixEpoch, "Modifier");

        Assert.Equal(id, customer.Id);
        Assert.Equal("Changed", customer.Name);
        Assert.Same(shipping, customer.ShippingAddress);
        Assert.Null(customer.BillingAddress);
        Assert.Equal(DateTime.MinValue, customer.DateCreated);
        Assert.Equal("Creator", customer.CreatedBy);
        Assert.Equal(DateTime.UnixEpoch, customer.DateModified);
        Assert.Equal("Modifier", customer.ModifiedBy);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            customer.RecordModification(DateTime.UtcNow, new string('U', 81)));
        Assert.Equal(DateTime.UnixEpoch, customer.DateModified);
        Assert.Equal("Modifier", customer.ModifiedBy);
    }
}
