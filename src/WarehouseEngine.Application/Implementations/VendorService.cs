using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Application.Results;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Application.Implementations;

public class VendorService : IVendorService
{
    private readonly IWarehouseEngineContext _context;
    private readonly IIdGenerator _idGenerator;
    
    public VendorService(IWarehouseEngineContext context, IIdGenerator idGenerator)
    {
        _context = context;
        _idGenerator = idGenerator;
    }

    public async Task<ServiceResult<VendorResponseDto, EntityErrorType>> GetByIdAsync(Guid id)
    {
        Vendor? entity = await _context.Vendor
            .AsNoTracking()
            .SingleOrDefaultAsync(v => v.Id == id);

        return entity is not null
            ? (VendorResponseDto)entity
            : new EntityDoesNotExist();
    }

    public async Task<IEnumerable<VendorResponseDto>> GetAllAsync()
    {
        var vendors = await _context.Vendor
            .AsNoTracking()
            .ToListAsync();

        return vendors.Select(v => (VendorResponseDto)v);
    }

    public async Task<int> GetCount()
    {
        return await _context.Vendor.CountAsync();
    }

    public async Task<ServiceResult<VendorResponseDto, EntityAlreadyExists>> AddAsync(PostVendorDto vendor, string username)
    {
        if (vendor.Id is not null)
        {
            // this is exceptional because this is internal logic
            throw new InvalidOperationException("Vendor should not have new id when created");
        }
        vendor.Id = _idGenerator.NewId();

        var entity = (Vendor)vendor;

        if (await _context.Vendor.AnyAsync(v => entity.Id == v.Id))
            return new EntityAlreadyExists();

        await _context.Vendor.AddAsync(entity);
        await _context.SaveChangesAsync();

        return (VendorResponseDto)entity;
    }

    public async Task<ServiceResult<VendorResponseDto, EntityErrorType>> UpdateAsync(Guid id, UpdateVendorDto vendor)
    {
        Vendor? entityToUpdate = await _context.Vendor
            .AsTracking()
            .SingleOrDefaultAsync(v => id == v.Id);
        
        if (entityToUpdate is null)
        {
            return new EntityDoesNotExist();
        }

        entityToUpdate.Rename(vendor.Name);

        await _context.SaveChangesAsync();

        return (VendorResponseDto)entityToUpdate;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Vendor
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();
    }
}
