using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Entities;

public partial class Order
{
    public static Order Create(Guid id, Guid customerId, Address shippingAddress, byte status = 0)
    {
        EntityRules.Identity(id, nameof(id));
        EntityRules.Identity(customerId, nameof(customerId));
        EntityRules.Address(shippingAddress, nameof(shippingAddress));
        return new Order { Id = id, CustomerId = customerId, ShippingAddress = shippingAddress, Status = status };
    }

    public void ChangeShippingAddress(Address shippingAddress)
    {
        EntityRules.Address(shippingAddress, nameof(shippingAddress));
        ShippingAddress = shippingAddress;
    }

    public void ChangeStatus(byte status) => Status = status;
}
