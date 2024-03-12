using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Application.Interfaces;
public interface ICustomerService
{
    Task<CustomerResponseDto> AddAsync(PostCustomerDto customer, string username);
    Task DeleteAsync(Guid id);
    Task<CustomerResponseDto> GetByIdAsync(Guid id);
    Task<int> GetCount();
    Task<int> GetCountByDate(DateOnly date);
    Task UpdateAsync(Guid id, Customer entity);
}
