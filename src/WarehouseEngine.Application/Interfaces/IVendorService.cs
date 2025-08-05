using OneOf;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Application.Interfaces;

public interface IVendorService
{
    Task<OneOf<VendorResponseDto, EntityAlreadyExists>> AddAsync(PostVendorDto vendor, string username);
    Task DeleteAsync(Guid id);
    Task<OneOf<VendorResponseDto, EntityErrorType>> GetByIdAsync(Guid id);
    Task<IEnumerable<VendorResponseDto>> GetAllAsync();
    Task<int> GetCount();
    Task<OneOf<VendorResponseDto, EntityErrorType>> UpdateAsync(Guid id, Vendor entity);
}