using Microsoft.EntityFrameworkCore;
using OneOf;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ErrorTypes;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Implementations;

public class CustomerService : ICustomerService
{
    private readonly IWarehouseEngineContext _context;
    private readonly IIdGenerator _idGenerator;
    public CustomerService(IWarehouseEngineContext context, IIdGenerator idGenerator)
    {
        _context = context;
        _idGenerator = idGenerator;
    }

    public async Task<OneOf<CustomerResponseDto, EntityErrorType>> GetByIdAsync(Guid id)
    {
        Customer? entity = await _context.Customer
            .AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == id);

        return entity is not null
            ? (CustomerResponseDto)entity
            : new EntityDoesNotExist();
    }

    public async Task<int> GetCount()
    {
        return await _context.Customer.CountAsync();
    }

    public async Task<int> GetCountByDate(DateOnly date)
    {
        return await _context.Customer
            .Where(e => e.DateCreated == date.ToDateTime(TimeOnly.MinValue))
            .CountAsync();
    }

    public async Task<OneOf<CustomerResponseDto, InvalidOperationException, EntityAlreadyExists, InvalidShippingResult>>
        AddAsync(PostCustomerDto customer, string username)
    {
        if (customer.Id is not null)
        {
            return new InvalidOperationException("Customer should not have new id when created");
        }
        customer.Id = _idGenerator.NewId();
        customer.CreatedBy = username;
        customer.DateCreated = DateTime.UtcNow;

        var entity = (Customer)customer;

        if (await _context.Customer.AnyAsync(e => entity.Id == e.Id))
            return new EntityAlreadyExists();

        if (entity.ShippingAddress is null)
        {
            return new InvalidShippingResult();
        }

        await _context.Customer.AddAsync(entity);
        await _context.SaveChangesAsync();

        return (CustomerResponseDto)entity;
    }

    public async Task<OneOf<CustomerResponseDto, EntityErrorType>> UpdateAsync(Guid id, Customer entity)
    {
        Customer? entityToUpdate = await _context.Customer
            .SingleOrDefaultAsync(e => id == e.Id);
        if (entityToUpdate is null)
        {

            return new EntityDoesNotExist();
        }

        entityToUpdate.BillingAddress = entity.BillingAddress;
        entityToUpdate.ShippingAddress = entity.ShippingAddress;
        entityToUpdate.Name = entity.Name;

        _context.Customer.Update(entityToUpdate);
        await _context.SaveChangesAsync();

        return (CustomerResponseDto)entityToUpdate;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Customer
            .Where(e => e.Id == id)
            .ExecuteDeleteAsync();
    }
}
