using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Exceptions;

namespace WarehouseEngine.Application.Implementations;
public class ItemService : IItemService
{
    private readonly IWarehouseEngineContext _context;
    public ItemService(IWarehouseEngineContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Item item)
    {
        if (_context.Item.SingleOrDefault(i => item.Id == i.Id) is not null)
            throw new EntityAlreadyExistsException();
        await _context.Item.AddAsync(item);
        await _context.SaveChangesAsync();
    }
}
