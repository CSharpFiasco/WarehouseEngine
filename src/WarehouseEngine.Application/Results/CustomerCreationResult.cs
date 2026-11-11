using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Domain.ErrorTypes;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Application.Results;

public union CustomerCreationResult(CustomerResponseDto, EntityAlreadyExists, InvalidShippingResult);
