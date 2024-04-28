using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid id)
    {
        var item = await _itemService.GetByIdAsync(id);

        return item.Match<IActionResult>(
            item => Ok(item),
            error => Problem(error.ErrorMessage, statusCode: 400)
            );
    }

    [HttpPost]
    public async Task<ActionResult<Item>> Create(PostItemDto item)
    {
        var response = await _itemService.AddAsync(item);

        return Ok(response);
    }
}
