using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Application.Interfaces;
public interface IItemService
{
    Task<Item> GetByIdAsync(int id);
    Task AddAsync(Item item);
}
