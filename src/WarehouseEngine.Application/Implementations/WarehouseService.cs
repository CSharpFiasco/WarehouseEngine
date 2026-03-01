using Microsoft.EntityFrameworkCore;
using OneOf;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Application.Implementations;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseEngineContext _context;
    private readonly IIdGenerator _idGenerator;

    public WarehouseService(IWarehouseEngineContext context, IIdGenerator idGenerator)
    {
        _context = context;
        _idGenerator = idGenerator;
    }

    public async Task<OneOf<WarehouseResponseDto, EntityErrorType>> GetByIdAsync(Guid id)
    {
        Warehouse? entity = await _context.Warehouse
            .AsNoTracking()
            .SingleOrDefaultAsync(w => w.Id == id);

        return entity is not null
            ? (WarehouseResponseDto)entity
            : new EntityDoesNotExist();
    }

    public async Task<int> GetCount()
    {
        return await _context.Warehouse.CountAsync();
    }

    public async Task<OneOf<WarehouseResponseDto, EntityAlreadyExists>> AddAsync(PostWarehouseDto warehouse)
    {
        if (warehouse.Id is not null)
        {
            // this is exceptional because this is internal logic
            throw new InvalidOperationException("Warehouse should not have new id when created");
        }
        warehouse.Id = _idGenerator.NewId();

        var entity = (Warehouse)warehouse;

        if (await _context.Warehouse.AnyAsync(w => entity.Id == w.Id))
            return new EntityAlreadyExists();

        await _context.Warehouse.AddAsync(entity);
        await _context.SaveChangesAsync();

        return (WarehouseResponseDto)entity;
    }
}
