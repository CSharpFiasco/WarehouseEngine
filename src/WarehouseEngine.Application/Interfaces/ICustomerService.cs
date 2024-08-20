using OneOf;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ErrorTypes;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Interfaces;
public interface ICustomerService
{
    Task<OneOf<CustomerResponseDto, InvalidOperationException, EntityAlreadyExists, InvalidShippingResult>> AddAsync(PostCustomerDto customer, string username);
    Task DeleteAsync(Guid id);
    Task<OneOf<CustomerResponseDto, EntityErrorType>> GetByIdAsync(Guid id);
    Task<int> GetCount();
    Task<int> GetCountByDate(DateOnly date);
    Task<OneOf<CustomerResponseDto, EntityErrorType>> UpdateAsync(Guid id, Customer entity);
}
