using Microsoft.EntityFrameworkCore;
using OneOf;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Implementations;

public class PositionService : IPositionService
{
    private readonly IWarehouseEngineContext _context;
    private readonly IIdGenerator _idGenerator;

    public PositionService(IWarehouseEngineContext context, IIdGenerator idGenerator)
    {
        _context = context;
        _idGenerator = idGenerator;
    }

    public async Task<OneOf<PositionResponseDto, EntityDoesNotExistResult>> GetByIdAsync(Guid id)
    {
        Position? position = await _context.Position.AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == id);

        return position is not null
            ? (PositionResponseDto)position
            : new EntityDoesNotExistResult(typeof(Position));
    }

    public async Task<OneOf<PositionResponseDto, EntityAlreadyExistsResult>> AddAsync(PostPositionDto position)
    {
        if (position.Id is not null)
        {
            throw new InvalidOperationException("Position should not have an id when created");
        }
        position.Id = _idGenerator.NewId();

        if (await _context.Position.AnyAsync(p => position.Id == p.Id))
            return new EntityAlreadyExistsResult(typeof(Position));

        var positionToAdd = (Position)position;
        await _context.Position.AddAsync(positionToAdd);
        await _context.SaveChangesAsync();

        return (PositionResponseDto)positionToAdd;
    }

    public async Task<int> GetCount()
    {
        return await _context.Position.CountAsync();
    }
}
