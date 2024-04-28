using OneOf;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Interfaces;
public interface ICustomerService
{
    Task<OneOf<CustomerResponseDto, InvalidOperationException, EntityAlreadyExistsResult, InvalidShippingResult>> AddAsync(PostCustomerDto customer, string username);
    Task DeleteAsync(Guid id);
    Task<OneOf<CustomerResponseDto, EntityDoesNotExistResult>> GetByIdAsync(Guid id);
    Task<int> GetCount();
    Task<int> GetCountByDate(DateOnly date);
    Task<OneOf<CustomerResponseDto, EntityDoesNotExistResult>> UpdateAsync(Guid id, Customer entity);
}
