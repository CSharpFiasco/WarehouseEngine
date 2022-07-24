using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Core.Entities;
using WarehouseEngine.Infrastructure.Data;

namespace WarehouseEngine.Infrastructure.Implementations;
public class ItemRepository : IItemRepository
{
    private readonly WarehouseEngineContext _context;
    public ItemRepository(WarehouseEngineContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Item item)
    {
        await _context.Item.AddAsync(item);
    }
}
