using OneOf;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Application.Interfaces;

public interface IWarehouseService
{
    Task<OneOf<WarehouseResponseDto, EntityAlreadyExists>> AddAsync(PostWarehouseDto warehouse);
    Task<OneOf<WarehouseResponseDto, EntityErrorType>> GetByIdAsync(Guid id);
    Task<int> GetCount();
}
