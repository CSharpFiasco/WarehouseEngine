using Microsoft.EntityFrameworkCore;
using OneOf;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Application.Implementations;

public class WarehouseQueries(IWarehouseEngineContext context) : IWarehouseQueries
{
    public async Task<OneOf<WarehouseResponseDto, EntityErrorType>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var warehouse = await context.Warehouse
            .AsNoTracking()
            .Where(w => w.Id == id)
            .Select(w => new WarehouseResponseDto { Id = w.Id, Name = w.Name })
            .SingleOrDefaultAsync(cancellationToken);

        return warehouse is not null ? warehouse : new EntityDoesNotExist();
    }

    public async Task<IEnumerable<WarehouseResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Warehouse
            .AsNoTracking()
            .Select(w => new WarehouseResponseDto { Id = w.Id, Name = w.Name })
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetCount(CancellationToken cancellationToken = default) =>
        context.Warehouse.CountAsync(cancellationToken);
}
