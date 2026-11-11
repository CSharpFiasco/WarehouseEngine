using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Results;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Interfaces;

public interface IPositionService
{
    Task<ServiceResult<PositionResponseDto, EntityDoesNotExistResult>> GetByIdAsync(Guid id);
    Task<ServiceResult<PositionResponseDto, EntityAlreadyExistsResult>> AddAsync(PostPositionDto position);
    Task<int> GetCount();
}
