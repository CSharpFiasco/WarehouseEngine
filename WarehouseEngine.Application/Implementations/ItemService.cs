using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Core.Entities;

namespace WarehouseEngine.Infrastructure.Implementations;
public class ItemService : IItemService
{
    private readonly IWarehouseEngineContext _context;
    public ItemService(IWarehouseEngineContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Item item)
    {
        await _context.Item.AddAsync(item);
        await _context.SaveChangesAsync();
    }
}
