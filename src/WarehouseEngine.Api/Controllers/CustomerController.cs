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
    public async Task<ActionResult<CustomerResponseDto>> Get(Guid id)
    {
        CustomerResponseDto? customer = await _customerService.GetByIdAsync(id);

        return Ok(customer);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> Count()
    {
        return Ok(await _customerService.GetCount());
    }

    [HttpGet("countByDate")]
    public async Task<ActionResult<int>> CountByDate(DateOnly date)
    {
        return Ok(await _customerService.GetCount());
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Create(PostCustomerDto customer)
    {
        customer.DateCreated = DateTime.UtcNow;
        customer.CreatedBy = User.Identity?.Name ?? "Unknown";

        var created = await _customerService.AddAsync(customer, User.Identity?.Name ?? "Unknown");

        return Ok(created);
    }
}
