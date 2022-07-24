using WarehouseEngine.Core.Entities;

namespace WarehouseEngine.Application.Interfaces;
public interface IItemRepository
{
    Task AddAsync(Item item);
}
