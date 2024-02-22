using System.Diagnostics.CodeAnalysis;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Tests;

[ExcludeFromCodeCoverage]
public class ValueObjectTests
{
    [Fact]
    public void Address_AllFieldMatch_Equality()
    {
        var address1 = new Address { Address1 = "testAddress1", Address2 = "testAddress2", City = "testCity3", State = "testState2", ZipCode = "testZip" };
        var address2 = new Address { Address1 = "testAddress1", Address2 = "testAddress2", City = "testCity3", State = "testState2", ZipCode = "testZip" };

        Assert.True(EqualityComparer<Address>.Default.Equals(address1, address2));
        Assert.True(address1.Equals(address2));
        Assert.True(address2.Equals(address1));
        Assert.True(address1 == address2);
    }

    [Fact]
    public void Address_Address1FieldDoesNotMatch_Inequality()
    {
        var address1 = new Address { Address1 = "testAddress1", Address2 = "testAddress2", City = "testCity3", State = "testState2", ZipCode = "testZip" };
        var address2 = new Address { Address1 = "testAddress2", Address2 = "testAddress2", City = "testCity3", State = "testState2", ZipCode = "testZip" };

        Assert.False(EqualityComparer<Address>.Default.Equals(address1, address2));
        Assert.False(address1.Equals(address2));
        Assert.False(address2.Equals(address1));
        Assert.True(address1 != address2);
    }

    [Fact]
    public void Address_Address2FieldDoesNotMatchWithNull_Inequality()
    {
        var address1 = new Address { Address1 = "testAddress1", Address2 = "testAddress2", City = "testCity3", State = "testState2", ZipCode = "testZip" };
        var address2 = new Address { Address1 = "testAddress1", Address2 = null, City = "testCity3", State = "testState2", ZipCode = "testZip" };

        Assert.False(EqualityComparer<Address>.Default.Equals(address1, address2));
        Assert.False(address1.Equals(address2));
        Assert.False(address2.Equals(address1));
        Assert.True(address1 != address2);
    }
}
