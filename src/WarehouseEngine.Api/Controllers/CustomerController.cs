using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customer)
    {
        _customerService = customer;
    }

    [HttpGet]
    public async Task<ActionResult<Customer>> Get(int id)
    {
        Customer? customer = await _customerService.GetByIdAsync(id);

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Create(Customer customer)
    {
        await _customerService.AddAsync(customer);

        return Ok(customer);
    }
}
