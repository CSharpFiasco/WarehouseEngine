using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Core.Entities;

namespace WarehouseEngine.Application.Implementations;
public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task AddAsync(Item item)
    {
        await _itemRepository.AddAsync(item);
    }
}
