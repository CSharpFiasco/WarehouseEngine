using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Results;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Application.Interfaces;

public interface IVendorService
{
    Task<ServiceResult<VendorResponseDto, EntityAlreadyExists>> AddAsync(PostVendorDto vendor, string username);
    Task DeleteAsync(Guid id);
    Task<ServiceResult<VendorResponseDto, EntityErrorType>> GetByIdAsync(Guid id);
    Task<IEnumerable<VendorResponseDto>> GetAllAsync();
    Task<int> GetCount();
    Task<ServiceResult<VendorResponseDto, EntityErrorType>> UpdateAsync(Guid id, UpdateVendorDto vendor);
}
