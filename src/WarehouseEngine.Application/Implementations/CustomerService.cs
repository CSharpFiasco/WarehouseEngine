using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Exceptions;

namespace WarehouseEngine.Application.Implementations;

public class CustomerService : ICustomerService
{
    private readonly IWarehouseEngineContext _context;
    public CustomerService(IWarehouseEngineContext context)
    {
        _context = context;
    }

    public async Task<Customer> GetByIdAsync(int id)
    {
        var entity = await _context.Customer.AsNoTracking()
            .SingleOrDefaultAsync(i => i.Id == id);

        return entity is not null
            ? entity
            : throw new EntityDoesNotExistException<Customer>();
    }

    public async Task AddAsync(Customer entity)
    {
        if (await _context.Customer.AnyAsync(e => entity.Id == e.Id))
            throw new EntityAlreadyExistsException<Customer>();

        if (entity.ShippingAddress is null)
        {
            throw new InvalidOperationException("Shipping Address is required");
        }

        await _context.Customer.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Customer entity)
    {
        var entityToUpdate = await _context.Customer.SingleOrDefaultAsync(e => id == e.Id);
        if (entityToUpdate is null)
            throw new EntityDoesNotExistException<Customer>();

        entityToUpdate.BillingAddress = entity.BillingAddress;
        entityToUpdate.ShippingAddress = entity.ShippingAddress;
        entityToUpdate.Name = entity.Name;

        _context.Customer.Update(entityToUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Customer.Where(e => e.Id == id).ExecuteDeleteAsync();
    }
}
