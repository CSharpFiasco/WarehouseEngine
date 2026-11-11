using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Results;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Application.Interfaces;
public interface ICustomerService
{
    Task<CustomerCreationResult> AddAsync(PostCustomerDto customer, string username);
    Task DeleteAsync(Guid id);
    Task<ServiceResult<CustomerResponseDto, EntityErrorType>> GetByIdAsync(Guid id);
    Task<int> GetCount();
    Task<int> GetCountByDate(DateOnly date);
    Task<ServiceResult<CustomerResponseDto, EntityErrorType>> UpdateAsync(Guid id, Customer entity);
}
