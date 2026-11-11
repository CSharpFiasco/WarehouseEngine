using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Tests;

public class EntityTests
{
    private static Address Address => new()
    {
        Address1 = "123 Main St",
        City = "Tulsa",
        State = "OK",
        ZipCode = "74101"
    };

    public static TheoryData<Type> EntityTypes => new()
    {
        typeof(Company), typeof(Contact), typeof(Customer), typeof(Employee),
        typeof(Item), typeof(Position), typeof(Vendor), typeof(Order),
        typeof(PurchaseOrder), typeof(WarehouseItem), typeof(OrderWarehouseItem),
        typeof(OrderWarehouseItemOutOfStock), typeof(PurchaseOrderWarehouseItem)
    };

    private static object Create(Type type, Guid id)
    {
        var relatedId = Guid.NewGuid();
        return type.Name switch
        {
            nameof(Company) => Company.Create(id, "Company"),
            nameof(Contact) => Contact.Create(id),
            nameof(Customer) => Customer.Create(id, "Customer", Address, DateTime.MinValue, "Creator"),
            nameof(Employee) => Employee.Create(id, "First", "Last", "User", relatedId, relatedId),
            nameof(Item) => Item.Create(id, "Sku", "Description"),
            nameof(Position) => Position.Create(id, "Position"),
            nameof(Vendor) => Vendor.Create(id),
            nameof(Order) => Order.Create(id, relatedId, Address),
            nameof(PurchaseOrder) => PurchaseOrder.Create(id, DateTime.MinValue, "PO-1"),
            nameof(WarehouseItem) => WarehouseItem.Create(id, relatedId, relatedId),
            nameof(OrderWarehouseItem) => OrderWarehouseItem.Create(id, relatedId, 1),
            nameof(OrderWarehouseItemOutOfStock) => OrderWarehouseItemOutOfStock.Create(id, relatedId, 1),
            nameof(PurchaseOrderWarehouseItem) => PurchaseOrderWarehouseItem.Create(id, relatedId, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    [Theory]
    [MemberData(nameof(EntityTypes))]
    public void Create_ValidInputs_ConstructsEntity(Type type)
    {
        var id = Guid.NewGuid();
        var entity = Create(type, id);

        Assert.IsType(type, entity);
        var key = type.GetProperty("Id") ?? type.GetProperty("OrderId") ?? type.GetProperty("PurchaseOrderId");
        Assert.NotNull(key);
        Assert.Equal(id, key.GetValue(entity));
    }

    [Theory]
    [MemberData(nameof(EntityTypes))]
    public void Create_EmptyIdentity_RejectsInput(Type type)
    {
        Assert.ThrowsAny<ArgumentException>(() => Create(type, Guid.Empty));
    }

    [Theory]
    [MemberData(nameof(EntityTypes))]
    public void Entity_CannotBeConstructedOrAssignedDirectly(Type type)
    {
        Assert.Empty(type.GetConstructors());
        Assert.All(type.GetProperties(), property =>
            Assert.False(property.SetMethod?.IsPublic ?? false, $"{type.Name}.{property.Name} exposes a public setter."));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_MissingRequiredText_RejectsInput(string? text)
    {
        var id = Guid.NewGuid();
        Assert.ThrowsAny<ArgumentException>(() => Company.Create(id, text!));
        Assert.ThrowsAny<ArgumentException>(() => Position.Create(id, text!));
        Assert.ThrowsAny<ArgumentException>(() => Item.Create(id, text!, "Description"));
        Assert.ThrowsAny<ArgumentException>(() => Item.Create(id, "Sku", text!));
        Assert.ThrowsAny<ArgumentException>(() => PurchaseOrder.Create(id, DateTime.MinValue, text!));
        Assert.ThrowsAny<ArgumentException>(() => Employee.Create(id, text!, "Last", "User", id, id));
        Assert.ThrowsAny<ArgumentException>(() => Employee.Create(id, "First", text!, "User", id, id));
        Assert.ThrowsAny<ArgumentException>(() => Employee.Create(id, "First", "Last", text!, id, id));
    }

    [Fact]
    public void Create_TextAtSchemaLimits_SucceedsWithoutNormalizing()
    {
        var id = Guid.NewGuid();
        Assert.Equal(80, Company.Create(id, new string('N', 80)).Name.Length);
        Assert.Equal(80, Position.Create(id, new string('N', 80)).Name.Length);
        Assert.Equal(80, Vendor.Create(id, new string('N', 80)).Name!.Length);
        var contact = Contact.Create(id, new string('F', 80), new string('L', 80), new string('E', 60));
        Assert.Equal(80, contact.FirstName!.Length);
        Assert.Equal(80, contact.LastName!.Length);
        Assert.Equal(60, contact.Email!.Length);
        var item = Item.Create(id, new string('S', 12), new string('D', 120));
        Assert.Equal(12, item.Sku.Length);
        Assert.Equal(120, item.Description.Length);
        Assert.Equal(255, PurchaseOrder.Create(id, DateTime.MinValue, new string('P', 255)).OrderNumber.Length);
        var employee = Employee.Create(id, new string('F', 30), new string('L', 30), new string('U', 32), id, id,
            new string('M', 30), new byte[32], "1234");
        Assert.Equal(30, employee.FirstName.Length);
        Assert.Equal(30, employee.MiddleName!.Length);
        Assert.Equal(30, employee.LastName.Length);
        Assert.Equal(32, employee.UserName.Length);
        Assert.Equal(32, employee.SocialSecurityNumberHash!.Length);
        Assert.Equal("1234", employee.SocialSecuritySerialNumber);
        Assert.Equal(" Name ", Company.Create(id, " Name ").Name);
    }

    [Fact]
    public void Create_TextBeyondSchemaLimits_RejectsInput()
    {
        var id = Guid.NewGuid();
        Assert.Throws<ArgumentOutOfRangeException>(() => Company.Create(id, new string('N', 81)));
        Assert.Throws<ArgumentOutOfRangeException>(() => Position.Create(id, new string('N', 81)));
        Assert.Throws<ArgumentOutOfRangeException>(() => Vendor.Create(id, new string('N', 81)));
        Assert.Throws<ArgumentOutOfRangeException>(() => Contact.Create(id, firstName: new string('F', 81)));
        Assert.Throws<ArgumentOutOfRangeException>(() => Contact.Create(id, lastName: new string('L', 81)));
        Assert.Throws<ArgumentOutOfRangeException>(() => Contact.Create(id, email: new string('E', 61)));
        Assert.Throws<ArgumentOutOfRangeException>(() => Item.Create(id, new string('S', 13), "Description"));
        Assert.Throws<ArgumentOutOfRangeException>(() => Item.Create(id, "Sku", new string('D', 121)));
        Assert.Throws<ArgumentOutOfRangeException>(() => PurchaseOrder.Create(id, DateTime.MinValue, new string('P', 256)));
        Assert.Throws<ArgumentOutOfRangeException>(() => Employee.Create(id, new string('F', 31), "Last", "User", id, id));
        Assert.Throws<ArgumentOutOfRangeException>(() => Employee.Create(id, "First", new string('L', 31), "User", id, id));
        Assert.Throws<ArgumentOutOfRangeException>(() => Employee.Create(id, "First", "Last", new string('U', 33), id, id));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Employee.Create(id, "First", "Last", "User", id, id, middleName: new string('M', 31)));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Employee.Create(id, "First", "Last", "User", id, id, socialSecurityNumberHash: new byte[33]));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Employee.Create(id, "First", "Last", "User", id, id, socialSecuritySerialNumber: "12345"));
    }

    [Fact]
    public void Create_EmptyRelatedIdentity_RejectsInput()
    {
        var id = Guid.NewGuid();
        Assert.Throws<ArgumentException>(() => Employee.Create(id, "First", "Last", "User", Guid.Empty, id));
        Assert.Throws<ArgumentException>(() => Employee.Create(id, "First", "Last", "User", id, Guid.Empty));
        Assert.Throws<ArgumentException>(() => Order.Create(id, Guid.Empty, Address));
        Assert.Throws<ArgumentException>(() => WarehouseItem.Create(id, Guid.Empty, id));
        Assert.Throws<ArgumentException>(() => WarehouseItem.Create(id, id, Guid.Empty));
        Assert.Throws<ArgumentException>(() => OrderWarehouseItem.Create(id, Guid.Empty, 1));
        Assert.Throws<ArgumentException>(() => OrderWarehouseItemOutOfStock.Create(id, Guid.Empty, 1));
        Assert.Throws<ArgumentException>(() => PurchaseOrderWarehouseItem.Create(id, Guid.Empty, 1));
    }

    [Fact]
    public void OptionalFields_RemainOptional()
    {
        var id = Guid.NewGuid();
        var contact = Contact.Create(id);
        Assert.Null(contact.FirstName);
        Assert.Null(contact.LastName);
        Assert.Null(contact.Email);
        Assert.Null(contact.Address);
        Assert.Null(Vendor.Create(id).Name);
        Assert.Equal("", Vendor.Create(id, "").Name);
        var item = Item.Create(id, "Sku", "Description");
        Assert.Null(item.DateCreated);
        Assert.Null(item.CreatedBy);
        Assert.False(item.IsActive);
        var employee = Employee.Create(id, "First", "Last", "User", id, id);
        Assert.Null(employee.MiddleName);
        Assert.Null(employee.SocialSecurityNumberHash);
        Assert.Null(employee.SocialSecuritySerialNumber);
    }

    [Fact]
    public void Rename_ValidAndInvalidChanges_PreserveIdentityAndLastValidName()
    {
        var id = Guid.NewGuid();
        var company = Company.Create(id, "Original");
        var position = Position.Create(id, "Original");
        var vendor = Vendor.Create(id, "Original");
        company.Rename("Changed");
        position.Rename("Changed");
        vendor.Rename(null);
        Assert.Throws<ArgumentException>(() => company.Rename(" "));
        Assert.Throws<ArgumentException>(() => position.Rename(""));
        Assert.Throws<ArgumentOutOfRangeException>(() => vendor.Rename(new string('N', 81)));
        Assert.Equal("Changed", company.Name);
        Assert.Equal("Changed", position.Name);
        Assert.Null(vendor.Name);
        Assert.Equal(id, company.Id);
        Assert.Equal(id, position.Id);
        Assert.Equal(id, vendor.Id);
    }

    [Fact]
    public void UpdateDetails_InvalidLaterField_DoesNotPartiallyMutate()
    {
        var id = Guid.NewGuid();
        var item = Item.Create(id, "Original", "Description", true);
        Assert.Throws<ArgumentOutOfRangeException>(() => item.UpdateDetails("Changed", new string('D', 121), false));
        Assert.Equal("Original", item.Sku);
        Assert.Equal("Description", item.Description);
        Assert.True(item.IsActive);
        item.UpdateDetails("Changed", "New description", false);
        Assert.Equal("Changed", item.Sku);
        Assert.False(item.IsActive);

        var contact = Contact.Create(id, "First", "Last", "Email", Address);
        Assert.Throws<ArgumentException>(() =>
            contact.UpdateDetails("Changed", null, null, Address with { City = "" }));
        Assert.Equal("First", contact.FirstName);
        Assert.Equal("Last", contact.LastName);
        Assert.Equal("Email", contact.Email);
        Assert.NotNull(contact.Address);
        contact.UpdateDetails(null, null, null, null);
        Assert.Null(contact.FirstName);
        Assert.Null(contact.Address);
    }

    [Fact]
    public void Employee_ChangesValidateBeforeMutation_AndDoNotExposeHashBuffer()
    {
        var id = Guid.NewGuid();
        byte[] hash = [1, 2, 3];
        var employee = Employee.Create(id, "First", "Last", "User", id, id, socialSecurityNumberHash: hash);
        hash[0] = 9;
        Assert.Equal((byte)1, employee.SocialSecurityNumberHash![0]);
        var copy = employee.SocialSecurityNumberHash;
        copy![0] = 9;
        Assert.Equal((byte)1, employee.SocialSecurityNumberHash![0]);

        Assert.Throws<ArgumentException>(() => employee.UpdatePersonalDetails("Changed", null, "Last", ""));
        Assert.Equal("First", employee.FirstName);
        Assert.Throws<ArgumentOutOfRangeException>(() => employee.UpdateSocialSecurityDetails([9], "12345"));
        Assert.Equal((byte)1, employee.SocialSecurityNumberHash![0]);
        Assert.Null(employee.SocialSecuritySerialNumber);
        Assert.Throws<ArgumentException>(() => employee.AssignPosition(Guid.Empty));
        Assert.Throws<ArgumentException>(() => employee.AssignSupervisor(Guid.Empty));
        Assert.Equal(id, employee.PositionId);
        Assert.Equal(id, employee.SupervisorEmployeeId);

        var relatedId = Guid.NewGuid();
        employee.AssignPosition(relatedId);
        employee.AssignSupervisor(relatedId);
        employee.UpdatePersonalDetails("Changed", "Middle", "New last", "New user");
        employee.UpdateSocialSecurityDetails([4], "1234");
        Assert.Equal(relatedId, employee.PositionId);
        Assert.Equal(relatedId, employee.SupervisorEmployeeId);
        Assert.Equal("Changed", employee.FirstName);
        Assert.Equal("Middle", employee.MiddleName);
        Assert.Equal("New last", employee.LastName);
        Assert.Equal("New user", employee.UserName);
        Assert.Equal((byte)4, employee.SocialSecurityNumberHash![0]);
        Assert.Equal("1234", employee.SocialSecuritySerialNumber);
    }

    [Fact]
    public void OrderChanges_PreserveExistingStatusVocabularyAndValidateOtherFields()
    {
        var id = Guid.NewGuid();
        var shipping = Address;
        var order = Order.Create(id, id, shipping, byte.MaxValue);
        Assert.Equal(byte.MaxValue, order.Status);
        Assert.Throws<ArgumentException>(() => order.ChangeShippingAddress(shipping with { Address1 = "" }));
        Assert.Same(shipping, order.ShippingAddress);
        Assert.Throws<ArgumentNullException>(() => Order.Create(id, id, null!));
        Assert.Throws<ArgumentException>(() => Order.Create(id, id, shipping with { State = "Invalid" }));
        order.ChangeShippingAddress(shipping with { City = "Dallas" });
        order.ChangeStatus(1);
        Assert.Equal("Dallas", order.ShippingAddress.City);
        Assert.Equal((byte)1, order.Status);

        var purchaseOrder = PurchaseOrder.Create(id, DateTime.MinValue, "PO-1", byte.MaxValue);
        Assert.Equal(byte.MaxValue, purchaseOrder.Status);
        Assert.Throws<ArgumentException>(() => purchaseOrder.ReviseDetails(DateTime.UnixEpoch, " "));
        Assert.Equal(DateTime.MinValue, purchaseOrder.OrderDate);
        Assert.Equal("PO-1", purchaseOrder.OrderNumber);
        purchaseOrder.ReviseDetails(DateTime.UnixEpoch, "PO-2");
        purchaseOrder.ChangeStatus(1);
        Assert.Equal(DateTime.UnixEpoch, purchaseOrder.OrderDate);
        Assert.Equal("PO-2", purchaseOrder.OrderNumber);
        Assert.Equal((byte)1, purchaseOrder.Status);
    }
}
