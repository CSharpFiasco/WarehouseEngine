using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Results;
using WarehouseEngine.Domain.ErrorTypes;
using WarehouseEngine.Domain.ValidationResults;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Application.Tests.Results;

[ExcludeFromCodeCoverage]
public class ServiceResultTests
{
    [Fact]
    public void ServiceResult_Success_PreservesResponse()
    {
        var response = new WarehouseResponseDto { Id = Guid.NewGuid(), Name = "Warehouse" };
        ServiceResult<WarehouseResponseDto, EntityErrorType> result = response;

        Assert.True(result is WarehouseResponseDto);
        Assert.Same(response, result.Value);
    }

    [Fact]
    public void ServiceResult_DerivedError_PreservesConcreteType()
    {
        var error = new EntityDoesNotExist();
        ServiceResult<WarehouseResponseDto, EntityErrorType> result = error;

        Assert.True(result is EntityDoesNotExist);
        Assert.Same(error, result.Value);
    }

    [Fact]
    public void ServiceResult_ValidationError_PreservesMessage()
    {
        var error = new EntityAlreadyExistsResult(typeof(Item));
        ServiceResult<ItemResponseDto, EntityAlreadyExistsResult> result = error;

        Assert.True(result is EntityAlreadyExistsResult);
        Assert.Same(error, result.Value);
        Assert.Equal(error.ErrorMessage, Assert.IsType<EntityAlreadyExistsResult>(result.Value).ErrorMessage);
    }

    [Fact]
    public void CustomerCreationResult_Success_PreservesResponse()
    {
        var response = (CustomerResponseDto)Customer.Create(
            Guid.NewGuid(), "Customer",
            new Address { Address1 = "123 Main St", City = "Tulsa", State = "OK", ZipCode = "74101" },
            DateTime.UnixEpoch, "Creator");
        CustomerCreationResult result = response;

        Assert.True(result is CustomerResponseDto);
        Assert.Same(response, result.Value);
    }

    [Fact]
    public void CustomerCreationResult_AlreadyExists_PreservesError()
    {
        var error = new EntityAlreadyExists();
        CustomerCreationResult result = error;

        Assert.True(result is EntityAlreadyExists);
        Assert.Same(error, result.Value);
    }

    [Fact]
    public void CustomerCreationResult_InvalidShipping_PreservesError()
    {
        var error = new InvalidShippingResult();
        CustomerCreationResult result = error;

        Assert.True(result is InvalidShippingResult);
        Assert.Same(error, result.Value);
    }

    [Fact]
    public void DefaultResults_MatchNull()
    {
        ServiceResult<WarehouseResponseDto, EntityErrorType> serviceResult = default;
        CustomerCreationResult customerResult = default;

        Assert.True(serviceResult is null);
        Assert.True(customerResult is null);
    }
}
