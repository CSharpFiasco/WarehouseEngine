using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Application.Interfaces;
public interface ICustomerService
{
    Task AddAsync(Customer entity);
    Task DeleteAsync(Guid id);
    Task<Customer> GetByIdAsync(Guid id);
    Task<int> GetCount();
    Task<int> GetCountByDate(DateOnly date);
    Task UpdateAsync(Guid id, Customer entity);
}
