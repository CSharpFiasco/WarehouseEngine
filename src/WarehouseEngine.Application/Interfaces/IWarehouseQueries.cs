using OneOf;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Application.Interfaces;

public interface IWarehouseQueries
{
    Task<OneOf<WarehouseResponseDto, EntityErrorType>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<WarehouseResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> GetCount(CancellationToken cancellationToken = default);
}
