using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Application.Interfaces;
public interface ICustomerService
{
    Task AddAsync(Customer entity);
    Task DeleteAsync(int id);
    Task<Customer> GetByIdAsync(int id);
    Task<int> GetCount();
    Task<int> GetCountByDate(DateOnly date);
    Task UpdateAsync(int id, Customer entity);
}
