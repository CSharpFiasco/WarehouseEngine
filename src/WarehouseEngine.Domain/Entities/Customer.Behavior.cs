using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Customer
{
    public const int MaxNameLength = 80;
    public const int MaxUserNameLength = 80;

    public static Customer Create(Guid id, string name, Address shippingAddress,
        DateTime dateCreated, string createdBy, Address? billingAddress = null)
    {
        EntityRules.Identity(id, nameof(id));
        EntityRules.RequiredText(createdBy, MaxUserNameLength, nameof(createdBy));
        var customer = new Customer { Id = id, DateCreated = dateCreated, CreatedBy = createdBy };
        customer.UpdateDetails(name, billingAddress, shippingAddress);
        return customer;
    }

    public void UpdateDetails(string name, Address? billingAddress, Address shippingAddress)
    {
        EntityRules.RequiredText(name, MaxNameLength, nameof(name));
        EntityRules.Address(shippingAddress, nameof(shippingAddress));
        if (billingAddress is not null)
            EntityRules.Address(billingAddress, nameof(billingAddress));

        Name = name;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;
    }

    public void RecordModification(DateTime dateModified, string modifiedBy)
    {
        EntityRules.RequiredText(modifiedBy, MaxUserNameLength, nameof(modifiedBy));
        DateModified = dateModified;
        ModifiedBy = modifiedBy;
    }
}
