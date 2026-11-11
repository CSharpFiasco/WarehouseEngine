using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.ErrorTypes;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Api.Endpoints;

internal class CustomerEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v{version:apiVersion}/customer/{id}", [Authorize] async Task<Results<Ok<CustomerResponseDto>, ProblemHttpResult>> (ILogger<CustomerEndpoints> logger, ICustomerService customerService, Guid id) =>
        {
            var customer = await customerService.GetByIdAsync(id);
            return customer switch
            {
                CustomerResponseDto response => TypedResults.Ok(response),
                EntityErrorType invalidResult => NotFoundProblem(logger, invalidResult),
            };
        })
        .Produces<CustomerResponseDto>(200)
        .WithName("GetCustomerById")
        .WithTags("Customer");

        app.MapGet("/api/v{version:apiVersion}/customer/count", [Authorize] async (ICustomerService customerService) =>
        {
            var count = await customerService.GetCount();
            return TypedResults.Ok(count);
        })
        .Produces<int>(200)
        .WithName("GetCustomerCount")
        .WithTags("Customer");

        app.MapPost("/api/v{version:apiVersion}/customer", [Authorize] async Task<Results<Created, ProblemHttpResult>> (ILogger < CustomerEndpoints > logger, ICustomerService customerService, PostCustomerDto customerDto, HttpContext httpContext) =>
        {
            customerDto.DateCreated = DateTime.UtcNow;
            customerDto.CreatedBy = httpContext.User.Identity?.Name ?? "Unknown";

            var customer = await customerService.AddAsync(customerDto, httpContext.User.Identity?.Name ?? "Unknown");

            return customer switch
            {
                CustomerResponseDto => TypedResults.Created(),
                EntityAlreadyExists entityExists => AlreadyExistsProblem(logger, entityExists),
                InvalidShippingResult invalidShippingResult => InvalidShippingProblem(logger, invalidShippingResult),
            };
        })
        .Produces<CustomerResponseDto>(200)
        .ProducesProblem(400)
        .ProducesValidationProblem()
        .WithName("CreateCustomer")
        .WithTags("Customer");
    }

    private static ProblemHttpResult NotFoundProblem(ILogger<CustomerEndpoints> logger, EntityErrorType error)
    {
        logger.LogError("Record not found. {message}", error.GetMessage());
        return TypedResults.Problem(statusCode: 404, detail: error.GetMessage());
    }

    private static ProblemHttpResult AlreadyExistsProblem(ILogger<CustomerEndpoints> logger, EntityAlreadyExists error)
    {
        logger.LogWarning("Record not found. {message}", error.GetMessage());
        return TypedResults.Problem(statusCode: 404, extensions: [new("Record not found", error.GetMessage())]);
    }

    private static ProblemHttpResult InvalidShippingProblem(ILogger<CustomerEndpoints> logger, InvalidShippingResult error)
    {
        logger.LogWarning(error.ErrorMessage!);
        return TypedResults.Problem(error.ErrorMessage, statusCode: 404);
    }
}
