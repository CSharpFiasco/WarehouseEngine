using Castle.Core.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(ICustomerService customer, ILogger<CustomerController> logger)
    {
        _customerService = customer;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponseDto>> Get(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        return customer.Match(
               customer => Ok(customer),
               invalidResult =>
               {
                   _logger.LogError("Record not found. {message}", invalidResult.GetMessage());
                   ModelState.AddModelError("Record not found", invalidResult.GetMessage());

                   return Problem(statusCode: 404);
               });
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> Count()
    {
        return Ok(await _customerService.GetCount());
    }

    /// <summary>
    /// <para>
    ///     This endpoint creates a new customer
    /// </para>
    /// <example>
    ///     <see cref="Examples.PostCustomerDtoExample">Swagger examples</see>
    /// </example>
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CustomerResponseDto>> Create(PostCustomerDto customerDto)
    {
        customerDto.DateCreated = DateTime.UtcNow;
        customerDto.CreatedBy = User.Identity?.Name ?? "Unknown";

        var customer = await _customerService.AddAsync(customerDto, User.Identity?.Name ?? "Unknown");

        return customer.Match(
               customer => Ok(customer),
               entityExists =>
               {
                   _logger.LogError("Record not found. {message}", entityExists.GetMessage());
                   ModelState.AddModelError("Record not found", entityExists.GetMessage());

                   return Problem(statusCode: 404);
               },
               invalidShippingResult =>
               {
                   _logger.LogError(invalidShippingResult.ErrorMessage!);

                   return Problem(invalidShippingResult.ErrorMessage, statusCode: 404);
               });
    }
}
