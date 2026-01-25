using Microsoft.EntityFrameworkCore;
using OneOf;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Implementations;
public class ItemService : IItemService
{
    private readonly IWarehouseEngineContext _context;
    private readonly IIdGenerator _idGenerator;

    public ItemService(IWarehouseEngineContext context, IIdGenerator idGenerator)
    {
        _context = context;
        _idGenerator = idGenerator;
    }

    public async Task<OneOf<ItemResponseDto, EntityDoesNotExistResult>> GetByIdAsync(Guid id)
    {
        Item? item = await _context.Item.AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == id);

        return item is not null
            ? (ItemResponseDto)item
            : new EntityDoesNotExistResult(typeof(Item));
    }

    public async Task<OneOf<ItemResponseDto, EntityAlreadyExistsResult>> AddAsync(PostItemDto item)
    {
        if (item.Id is not null)
        {
            throw new InvalidOperationException("Item should not have an id when created");
        }
        item.Id = _idGenerator.NewId();

        if (await _context.Item.AnyAsync(i => item.Id == i.Id))
            return new EntityAlreadyExistsResult(typeof(Item));

        var itemToAdd = (Item)item;
        await _context.Item.AddAsync(itemToAdd);
        await _context.SaveChangesAsync();

        return (ItemResponseDto)itemToAdd;
    }

    public async Task<OneOf<ItemResponseDto, EntityDoesNotExistResult>> UpdateAsync(Guid id, PostItemDto item)
    {
        Item? entityToUpdate = await _context.Item.SingleOrDefaultAsync(i => id == i.Id);
        if (entityToUpdate is null)
            return new EntityDoesNotExistResult(typeof(Item));

        entityToUpdate.IsActive = item.IsActive;
        entityToUpdate.Sku = item.Sku;
        entityToUpdate.Description = item.Description;

        _context.Item.Update(entityToUpdate);
        await _context.SaveChangesAsync();

        return (ItemResponseDto)entityToUpdate;
    }

    public async Task<int> GetCount()
    {
        return await _context.Item.CountAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Item.Where(e => e.Id == id).ExecuteDeleteAsync();
    }
}
