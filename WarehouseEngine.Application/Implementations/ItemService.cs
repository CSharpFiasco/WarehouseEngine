using Microsoft.EntityFrameworkCore;
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

    public async Task<Item> GetByIdAsync(int id)
    {
        var item = await _context.Item.AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == id);

        return item is not null
            ? item
            : throw new EntityDoesNotExistException<Item>();
    }

    public async Task AddAsync(Item item)
    {
        if (_context.Item.SingleOrDefault(i => item.Id == i.Id) is not null)
            throw new EntityAlreadyExistsException<Item>();
        await _context.Item.AddAsync(item);
        await _context.SaveChangesAsync();
    }
}
