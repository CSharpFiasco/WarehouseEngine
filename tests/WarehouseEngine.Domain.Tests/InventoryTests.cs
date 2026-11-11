using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Domain.Tests;

public class InventoryTests
{
    [Theory]
    [InlineData(0, null)]
    [InlineData(0, 0d)]
    [InlineData(1, 12.5)]
    [InlineData(int.MaxValue, 1d)]
    public void Stock_NonnegativeQuantityAndOptionalPrice_Succeed(int quantity, double? price)
    {
        var id = Guid.NewGuid();
        var warehouseItem = WarehouseItem.Create(id, id, id, quantity, (decimal?)price);

        Assert.Equal(quantity, warehouseItem.Quantity);
        Assert.Equal((decimal?)price, warehouseItem.Price);
        warehouseItem.UpdateStock(0, null);
        Assert.Equal(0, warehouseItem.Quantity);
        Assert.Null(warehouseItem.Price);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -0.001)]
    public void Stock_NegativeQuantityOrPrice_RejectsWithoutPartialMutation(int quantity, double price)
    {
        var id = Guid.NewGuid();
        Assert.Throws<ArgumentOutOfRangeException>(() => WarehouseItem.Create(id, id, id, quantity, (decimal)price));
        var warehouseItem = WarehouseItem.Create(id, id, id, 10, 5m);

        Assert.Throws<ArgumentOutOfRangeException>(() => warehouseItem.UpdateStock(quantity, (decimal)price));
        Assert.Equal(10, warehouseItem.Quantity);
        Assert.Equal(5m, warehouseItem.Price);
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(0)]
    public void Lines_NonpositiveQuantity_RejectsCreationAndChange(int quantity)
    {
        var id = Guid.NewGuid();
        Assert.Throws<ArgumentOutOfRangeException>(() => OrderWarehouseItem.Create(id, id, quantity));
        Assert.Throws<ArgumentOutOfRangeException>(() => OrderWarehouseItemOutOfStock.Create(id, id, quantity));
        Assert.Throws<ArgumentOutOfRangeException>(() => PurchaseOrderWarehouseItem.Create(id, id, quantity));
        var orderLine = OrderWarehouseItem.Create(id, id, 1);
        var outOfStockLine = OrderWarehouseItemOutOfStock.Create(id, id, 1);
        var purchaseLine = PurchaseOrderWarehouseItem.Create(id, id, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => orderLine.ChangeQuantity(quantity));
        Assert.Throws<ArgumentOutOfRangeException>(() => outOfStockLine.ChangeQuantity(quantity));
        Assert.Throws<ArgumentOutOfRangeException>(() => purchaseLine.ChangeQuantity(quantity));
        Assert.Equal(1, orderLine.Quantity);
        Assert.Equal(1, outOfStockLine.Quantity);
        Assert.Equal(1, purchaseLine.Quantity);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void Lines_PositiveQuantity_PreservesKeysOnChange(int quantity)
    {
        var id = Guid.NewGuid();
        var stockId = Guid.NewGuid();
        var orderLine = OrderWarehouseItem.Create(id, stockId, 1);
        var outOfStockLine = OrderWarehouseItemOutOfStock.Create(id, stockId, 1);
        var purchaseLine = PurchaseOrderWarehouseItem.Create(id, stockId, 1);
        orderLine.ChangeQuantity(quantity);
        outOfStockLine.ChangeQuantity(quantity);
        purchaseLine.ChangeQuantity(quantity);

        Assert.Equal(quantity, orderLine.Quantity);
        Assert.Equal(quantity, outOfStockLine.Quantity);
        Assert.Equal(quantity, purchaseLine.Quantity);
        Assert.Equal(id, orderLine.OrderId);
        Assert.Equal(id, outOfStockLine.OrderId);
        Assert.Equal(id, purchaseLine.PurchaseOrderId);
        Assert.Equal(stockId, orderLine.WarehouseItemId);
        Assert.Equal(stockId, outOfStockLine.WarehouseItemId);
        Assert.Equal(stockId, purchaseLine.WarehouseItemId);
    }
}
