using OneOf;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Interfaces;
public interface IItemService
{
    Task<OneOf<ItemResponseDto, EntityDoesNotExistResult>> GetByIdAsync(Guid id);
    Task<OneOf<ItemResponseDto, EntityAlreadyExistsResult>> AddAsync(PostItemDto item);
    Task<OneOf<ItemResponseDto, EntityDoesNotExistResult>> UpdateAsync(Guid id, PostItemDto item);
}
