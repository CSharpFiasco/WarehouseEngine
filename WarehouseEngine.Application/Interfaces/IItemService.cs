using WarehouseEngine.Core.Entities;

namespace WarehouseEngine.Application.Interfaces;
public interface IItemService
{
    Task AddAsync(Item item);
}
