using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Api.Examples;
public static class CustomerExamples
{
    public static readonly CustomerResponseDto CustomerResponseDto = new()
    {
        Id = Guid.NewGuid(),
        Name = "John Doe",
        BillingAddress = new Address { Address1 = "123 Main St", City = "Anytown", State = "OK", ZipCode = "12345" },
        ShippingAddress = new Address { Address1 = "123 Main St", City = "Anytown", State = "NY", ZipCode = "12345" },
        DateCreated = DateTime.UtcNow,
        CreatedBy = "System"
    };
}
