using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class VendorController : ControllerBase
{
    private readonly IVendorService _vendorService;
    private readonly ILogger<VendorController> _logger;

    public VendorController(IVendorService vendorService, ILogger<VendorController> logger)
    {
        _vendorService = vendorService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(VendorResponseDto), 200)]
    public async Task<ActionResult<VendorResponseDto>> Get(Guid id)
    {
        var vendor = await _vendorService.GetByIdAsync(id);

        return vendor.Match(
               vendor => Ok(vendor),
               invalidResult =>
               {
                   _logger.LogError("Record not found. {message}", invalidResult.GetMessage());
                   ModelState.AddModelError("Record not found", invalidResult.GetMessage());

                   return Problem(statusCode: 404);
               });
    }

    [HttpGet("list")]
    [ProducesResponseType(typeof(IEnumerable<VendorResponseDto>), 200)]
    public async Task<ActionResult<IEnumerable<VendorResponseDto>>> GetAll()
    {
        var vendors = await _vendorService.GetAllAsync();
        return Ok(vendors);
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> Count()
    {
        return Ok(await _vendorService.GetCount());
    }

    [HttpPost]
    public async Task<ActionResult<VendorResponseDto>> Create(PostVendorDto vendorDto)
    {
        var vendor = await _vendorService.AddAsync(vendorDto, User.Identity?.Name ?? "Unknown");

        return vendor.Match(
               vendor => Ok(vendor),
               entityExists =>
               {
                   _logger.LogError("Entity already exists. {message}", entityExists.GetMessage());
                   ModelState.AddModelError("Entity already exists", entityExists.GetMessage());

                   return Problem(statusCode: 409);
               });
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _vendorService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VendorResponseDto>> Update(Guid id, UpdateVendorDto vendorDto)
    {
        vendorDto.Id = id;
        var vendor = (Vendor)vendorDto;
        var result = await _vendorService.UpdateAsync(id, vendor);

        return result.Match(
               vendor => Ok(vendor),
               invalidResult =>
               {
                   _logger.LogError("Record not found. {message}", invalidResult.GetMessage());
                   ModelState.AddModelError("Record not found", invalidResult.GetMessage());

                   return Problem(statusCode: 404);
               });
    }
}
