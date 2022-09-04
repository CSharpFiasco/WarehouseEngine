using Microsoft.AspNetCore.Mvc;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Api.Controllers;
[ApiController]
[Route("Item")]
public class ItemController : Controller
{
    private readonly IItemService _itemService;
    public ItemController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Item item)
    {
        await _itemService.AddAsync(item);

        return Ok(item);
    }
}
