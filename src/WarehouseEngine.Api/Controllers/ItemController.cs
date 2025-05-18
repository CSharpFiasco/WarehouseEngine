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
    [ProducesResponseType(typeof(ItemResponseDto), 200)]
    public async Task<ActionResult<ItemResponseDto>> Get(Guid id)
    {
        var item = await _itemService.GetByIdAsync(id);

        return item.Match(
            item => Ok(item),
            error => Problem(error.ErrorMessage, statusCode: 400)
            );
    }

    [HttpPost]
    public async Task<ActionResult<ItemResponseDto>> Create(PostItemDto itemDto)
    {
        var item = await _itemService.AddAsync(itemDto);

        return item.Match(
            item => Ok(item),
            error => Problem(error.ErrorMessage, statusCode: 400)
            );
    }
}
