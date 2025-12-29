using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;

namespace WarehouseEngine.Api.Endpoints;

internal class CustomerEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v{version:apiVersion}/customer/{id}", [Authorize] async (ILogger<CustomerEndpoints> logger, ICustomerService customerService, Guid id) =>
        {
            var customer = await customerService.GetByIdAsync(id);
            return customer.Match<Results<Ok<CustomerResponseDto>, ProblemHttpResult>>(
                    customer => TypedResults.Ok(customer),
                    invalidResult =>
                    {
                        logger.LogError("Record not found. {message}", invalidResult.GetMessage());
                        return TypedResults.Problem(statusCode: 404, detail: invalidResult.GetMessage());
                    });
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

        app.MapPost("/api/v{version:apiVersion}/customer", [Authorize] async (ILogger < CustomerEndpoints > logger, ICustomerService customerService, PostCustomerDto customerDto, HttpContext httpContext) =>
        {
            customerDto.DateCreated = DateTime.UtcNow;
            customerDto.CreatedBy = httpContext.User.Identity?.Name ?? "Unknown";

            var customer = await customerService.AddAsync(customerDto, httpContext.User.Identity?.Name ?? "Unknown");

            return customer.Match<Results<Created, ProblemHttpResult>>(
               customer => TypedResults.Created(),
               entityExists =>
               {
                   logger.LogWarning("Record not found. {message}", entityExists.GetMessage());

                   return TypedResults.Problem(statusCode: 404, extensions: [new("Record not found", entityExists.GetMessage())]);
               },
               invalidShippingResult =>
               {
                   logger.LogWarning(invalidShippingResult.ErrorMessage!);

                   return TypedResults.Problem(invalidShippingResult.ErrorMessage, statusCode: 404);
               });
        })
        .Produces<CustomerResponseDto>(200)
        .ProducesProblem(400)
        .WithName("CreateCustomer")
        .WithTags("Customer");
    }
}
