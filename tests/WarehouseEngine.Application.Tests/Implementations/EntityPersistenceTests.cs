using WarehouseEngine.Domain.ValueObjects;
using WarehouseEngine.Infrastructure.DataContext;

namespace WarehouseEngine.Application.Tests.Implementations;

public class EntityPersistenceTests(TestDatabaseFixture fixture) : IClassFixture<TestDatabaseFixture>
{
    private static Address Address => new()
    {
        Address1 = "123 Main St",
        City = "Tulsa",
        State = "OK",
        ZipCode = "74101"
    };

    [Fact]
    public async Task GuardedEntities_RoundTripAndPersistChanges_WithExistingMappings()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var company = Company.Create(Guid.NewGuid(), "Company");
        var contact = Contact.Create(Guid.NewGuid(), "First", "Last", "email@example.test", Address);
        var customer = Customer.Create(Guid.NewGuid(), "Customer", Address, DateTime.UnixEpoch, "Creator", Address);
        customer.Contact.Add(contact);
        var item = Item.Create(Guid.NewGuid(), "ROUNDTRIP", "Item", true, DateTime.UnixEpoch, "Creator");
        var vendor = Vendor.Create(Guid.NewGuid(), "Vendor");
        item.Vendor.Add(vendor);
        var position = Position.Create(Guid.NewGuid(), "Position");
        var employeeId = Guid.NewGuid();
        var employee = Employee.Create(employeeId, "First", "Last", "User", position.Id, employeeId,
            socialSecurityNumberHash: [1, 2, 3], socialSecuritySerialNumber: "1234");
        var warehouse = Warehouse.Create(Guid.NewGuid(), "Warehouse");
        var stock = WarehouseItem.Create(Guid.NewGuid(), warehouse.Id, item.Id, 10, 5m);
        var order = Order.Create(Guid.NewGuid(), customer.Id, Address, 255);
        var purchaseOrder = PurchaseOrder.Create(Guid.NewGuid(), DateTime.UnixEpoch, "PO-1", 255);
        var orderLine = OrderWarehouseItem.Create(order.Id, stock.Id, 2);
        var outOfStockLine = OrderWarehouseItemOutOfStock.Create(order.Id, stock.Id, 3);
        var purchaseLine = PurchaseOrderWarehouseItem.Create(purchaseOrder.Id, stock.Id, 4);

        context.AddRange(company, customer, item, position, employee, warehouse, stock, order, purchaseOrder,
            orderLine, outOfStockLine, purchaseLine);
        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();

        var restoredCompany = await context.Company.AsTracking().SingleAsync(e => e.Id == company.Id, cancellationToken);
        var restoredCustomer = await context.Customer.AsTracking().Include(e => e.Contact)
            .SingleAsync(e => e.Id == customer.Id, cancellationToken);
        var restoredContact = Assert.Single(restoredCustomer.Contact);
        var restoredItem = await context.Item.AsTracking().Include(e => e.Vendor)
            .SingleAsync(e => e.Id == item.Id, cancellationToken);
        var restoredVendor = Assert.Single(restoredItem.Vendor);
        var restoredEmployee = await context.Employee.AsTracking().Include(e => e.Position).Include(e => e.SupervisorEmployee)
            .SingleAsync(e => e.Id == employee.Id, cancellationToken);
        var restoredPosition = Assert.IsType<Position>(restoredEmployee.Position);
        var restoredStock = await context.WarehouseItem.AsTracking().Include(e => e.Warehouse)
            .SingleAsync(e => e.Id == stock.Id, cancellationToken);
        var restoredOrder = await context.Order.AsTracking().Include(e => e.OrderWarehouseItem)
            .Include(e => e.OrderWarehouseItemOutOfStock).SingleAsync(e => e.Id == order.Id, cancellationToken);
        var restoredPurchaseOrder = await context.PurchaseOrder.AsTracking().Include(e => e.PurchaseOrderWarehouseItem)
            .SingleAsync(e => e.Id == purchaseOrder.Id, cancellationToken);

        Assert.Equal("Company", restoredCompany.Name);
        Assert.Equal(contact.Id, restoredContact.Id);
        Assert.Equal(Address, restoredContact.Address);
        Assert.Equal(Address, restoredCustomer.BillingAddress);
        Assert.Equal(Address, restoredCustomer.ShippingAddress);
        Assert.Equal(vendor.Id, restoredVendor.Id);
        Assert.Equal("ROUNDTRIP", restoredItem.Sku);
        Assert.Equal(DateTime.UnixEpoch, restoredItem.DateCreated);
        Assert.Equal("Creator", restoredItem.CreatedBy);
        Assert.Equal(new byte[] { 1, 2, 3 }, restoredEmployee.SocialSecurityNumberHash);
        Assert.Equal("1234", restoredEmployee.SocialSecuritySerialNumber);
        Assert.Same(restoredEmployee, restoredEmployee.SupervisorEmployee);
        Assert.Equal(position.Id, restoredPosition.Id);
        Assert.Equal(warehouse.Id, restoredStock.Warehouse!.Id);
        Assert.Equal(10, restoredStock.Quantity);
        Assert.Equal(5m, restoredStock.Price);
        Assert.Equal((byte)255, restoredOrder.Status);
        Assert.Equal(Address, restoredOrder.ShippingAddress);
        Assert.Equal(2, Assert.Single(restoredOrder.OrderWarehouseItem).Quantity);
        Assert.Equal(3, Assert.Single(restoredOrder.OrderWarehouseItemOutOfStock).Quantity);
        Assert.Equal((byte)255, restoredPurchaseOrder.Status);
        Assert.Equal(4, Assert.Single(restoredPurchaseOrder.PurchaseOrderWarehouseItem).Quantity);

        restoredCompany.Rename("Changed company");
        restoredContact.UpdateDetails("Changed", null, null, null);
        restoredCustomer.UpdateDetails("Changed customer", null, Address with { City = "Dallas" });
        restoredCustomer.RecordModification(DateTime.UnixEpoch.AddDays(1), "Modifier");
        restoredItem.UpdateDetails("CHANGED", "Changed item", false);
        restoredVendor.Rename(null);
        restoredPosition.Rename("Changed position");
        restoredEmployee.UpdatePersonalDetails("Changed", "Middle", "Last", "Changed user");
        restoredEmployee.UpdateSocialSecurityDetails([4, 5], null);
        restoredStock.UpdateStock(0, null);
        restoredOrder.ChangeStatus(1);
        restoredOrder.ChangeShippingAddress(Address with { City = "Dallas" });
        restoredPurchaseOrder.ChangeStatus(1);
        restoredPurchaseOrder.ReviseDetails(DateTime.UnixEpoch.AddDays(1), "PO-2");
        Assert.Single(restoredOrder.OrderWarehouseItem).ChangeQuantity(5);
        Assert.Single(restoredOrder.OrderWarehouseItemOutOfStock).ChangeQuantity(6);
        Assert.Single(restoredPurchaseOrder.PurchaseOrderWarehouseItem).ChangeQuantity(7);
        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();

        Assert.Equal("Changed company", (await context.Company.SingleAsync(e => e.Id == company.Id, cancellationToken)).Name);
        var savedContact = await context.Contact.SingleAsync(e => e.Id == contact.Id, cancellationToken);
        Assert.Equal("Changed", savedContact.FirstName);
        Assert.Null(savedContact.Address);
        var savedCustomer = await context.Customer.SingleAsync(e => e.Id == customer.Id, cancellationToken);
        Assert.Equal("Changed customer", savedCustomer.Name);
        Assert.Equal("Dallas", savedCustomer.ShippingAddress.City);
        Assert.Null(savedCustomer.BillingAddress);
        Assert.Equal(DateTime.UnixEpoch, savedCustomer.DateCreated);
        Assert.Equal("Creator", savedCustomer.CreatedBy);
        Assert.Equal("Modifier", savedCustomer.ModifiedBy);
        Assert.Equal(DateTime.UnixEpoch.AddDays(1), savedCustomer.DateModified);
        var savedItem = await context.Item.SingleAsync(e => e.Id == item.Id, cancellationToken);
        Assert.Equal("CHANGED", savedItem.Sku);
        Assert.Equal("Changed item", savedItem.Description);
        Assert.False(savedItem.IsActive);
        Assert.Null((await context.Vendor.SingleAsync(e => e.Id == vendor.Id, cancellationToken)).Name);
        Assert.Equal("Changed position", (await context.Position.SingleAsync(e => e.Id == position.Id, cancellationToken)).Name);
        var savedEmployee = await context.Employee.SingleAsync(e => e.Id == employee.Id, cancellationToken);
        Assert.Equal("Changed", savedEmployee.FirstName);
        Assert.Equal("Middle", savedEmployee.MiddleName);
        Assert.Equal("Changed user", savedEmployee.UserName);
        Assert.Equal(new byte[] { 4, 5 }, savedEmployee.SocialSecurityNumberHash);
        Assert.Null(savedEmployee.SocialSecuritySerialNumber);
        var savedStock = await context.WarehouseItem.SingleAsync(e => e.Id == stock.Id, cancellationToken);
        Assert.Equal(0, savedStock.Quantity);
        Assert.Null(savedStock.Price);
        var savedOrder = await context.Order.SingleAsync(e => e.Id == order.Id, cancellationToken);
        Assert.Equal((byte)1, savedOrder.Status);
        Assert.Equal("Dallas", savedOrder.ShippingAddress.City);
        var savedPurchaseOrder = await context.PurchaseOrder.SingleAsync(e => e.Id == purchaseOrder.Id, cancellationToken);
        Assert.Equal((byte)1, savedPurchaseOrder.Status);
        Assert.Equal("PO-2", savedPurchaseOrder.OrderNumber);
        Assert.Equal(DateTime.UnixEpoch.AddDays(1), savedPurchaseOrder.OrderDate);
        Assert.Equal(5, (await context.OrderWarehouseItem.SingleAsync(e => e.OrderId == order.Id, cancellationToken)).Quantity);
        Assert.Equal(6, (await context.OrderWarehouseItemOutOfStock.SingleAsync(e => e.OrderId == order.Id, cancellationToken)).Quantity);
        Assert.Equal(7, (await context.PurchaseOrderWarehouseItem.SingleAsync(e => e.PurchaseOrderId == purchaseOrder.Id, cancellationToken)).Quantity);
    }

    [Fact]
    public async Task Employee_Reassignment_PersistsForeignKeysAndNavigationFixup()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        var originalPosition = Position.Create(Guid.NewGuid(), "Original");
        var newPosition = Position.Create(Guid.NewGuid(), "New");
        var id = Guid.NewGuid();
        var employee = Employee.Create(id, "First", "Last", "User", originalPosition.Id, id);
        context.AddRange(originalPosition, newPosition, employee);
        await context.SaveChangesAsync(cancellationToken);
        var supervisorId = Guid.NewGuid();
        var supervisor = Employee.Create(supervisorId, "Supervisor", "Last", "Supervisor", newPosition.Id, supervisorId);
        context.Add(supervisor);
        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();

        var restored = await context.Employee.AsTracking().Include(e => e.Position).Include(e => e.SupervisorEmployee)
            .SingleAsync(e => e.Id == id, cancellationToken);
        restored.AssignPosition(newPosition.Id);
        restored.AssignSupervisor(supervisorId);
        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();

        var saved = await context.Employee.Include(e => e.Position).Include(e => e.SupervisorEmployee)
            .SingleAsync(e => e.Id == id, cancellationToken);
        Assert.Equal(newPosition.Id, saved.PositionId);
        Assert.Equal(newPosition.Id, saved.Position!.Id);
        Assert.Equal(supervisorId, saved.SupervisorEmployeeId);
        Assert.Equal(supervisorId, saved.SupervisorEmployee.Id);
    }
}
