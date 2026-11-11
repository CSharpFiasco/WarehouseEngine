using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Domain.Tests;
public class WarehouseTests
{
    [Fact]
    public void Create_ValidInputs_PreservesIdentityAndName()
    {
        var id = Guid.NewGuid();
        var warehouse = Warehouse.Create(id, " Test Warehouse ");

        Assert.Equal(id, warehouse.Id);
        Assert.Equal(" Test Warehouse ", warehouse.Name);
        Assert.Empty(warehouse.WarehouseItem);
    }

    [Fact]
    public void Create_NameAtMaximumLength_Succeeds()
    {
        var name = new string('W', 32);

        var warehouse = Warehouse.Create(Guid.NewGuid(), name);

        Assert.Equal(name, warehouse.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_MissingName_ThrowsArgumentException(string? name)
    {
        var exception = Assert.ThrowsAny<ArgumentException>(() => Warehouse.Create(Guid.NewGuid(), name!));

        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Create_NameExceedsMaximumLength_ThrowsArgumentOutOfRangeException()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => Warehouse.Create(Guid.NewGuid(), new string('W', 33)));

        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Create_EmptyId_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => Warehouse.Create(Guid.Empty, "Test Warehouse"));

        Assert.Equal("id", exception.ParamName);
    }
}
