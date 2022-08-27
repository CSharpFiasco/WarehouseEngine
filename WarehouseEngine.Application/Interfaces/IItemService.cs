using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Application.Interfaces;
public interface IItemService
{
    Task AddAsync(Item item);
}
