using OneOf;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Interfaces;

public interface IPositionService
{
    Task<OneOf<PositionResponseDto, EntityDoesNotExistResult>> GetByIdAsync(Guid id);
    Task<OneOf<PositionResponseDto, EntityAlreadyExistsResult>> AddAsync(PostPositionDto position);
    Task<int> GetCount();
}
