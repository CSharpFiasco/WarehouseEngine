using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Api.Examples;
public class PostCustomerDtoExample : IExamplesProvider<PostCustomerDto>
{
    public PostCustomerDto GetExamples()
    {
        return new PostCustomerDto
        {
            Name = "John Doe",
            BillingAddress = new Address { Address1 = "123 Main St", City = "Anytown", State = "OK", ZipCode = "12345" },
            ShippingAddress = new Address { Address1 = "123 Main St", City = "Anytown", State = "NY", ZipCode = "12345" }
        };
    }
}
