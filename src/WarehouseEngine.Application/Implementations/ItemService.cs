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

    public async Task<Item> GetByIdAsync(Guid id)
    {
        Item? item = await _context.Item.AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == id);

        return item is not null
            ? item
            : throw new EntityDoesNotExistException<Item>();
    }

    public async Task AddAsync(Item item)
    {
        if (await _context.Item.AnyAsync(i => item.Id == i.Id))
            throw new EntityAlreadyExistsException<Item>();
        await _context.Item.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, Item item)
    {
        Item? entityToUpdate = await _context.Item.SingleOrDefaultAsync(i => id == i.Id);
        if (entityToUpdate is null)
            throw new EntityDoesNotExistException<Item>();

        entityToUpdate.IsActive = item.IsActive;
        entityToUpdate.Sku = item.Sku;
        entityToUpdate.Description = item.Description;

        _context.Item.Update(entityToUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Item.Where(e => e.Id == id).ExecuteDeleteAsync();
    }
}
