using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Results;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Interfaces;
public interface IItemService
{
    Task<ServiceResult<ItemResponseDto, EntityDoesNotExistResult>> GetByIdAsync(Guid id);
    Task<ServiceResult<ItemResponseDto, EntityAlreadyExistsResult>> AddAsync(PostItemDto item);
    Task<ServiceResult<ItemResponseDto, EntityDoesNotExistResult>> UpdateAsync(Guid id, PostItemDto item);
    Task<int> GetCount();
}
