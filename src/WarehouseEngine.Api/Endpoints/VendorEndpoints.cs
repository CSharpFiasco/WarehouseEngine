using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Api.Endpoints;

public class VendorEndpoints
{
    protected VendorEndpoints() { }

    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v{version:apiVersion}/vendor/{id}", [Authorize] async Task<Results<Ok<VendorResponseDto>, ProblemHttpResult>> (ILogger<VendorEndpoints> logger, IVendorService vendorService, Guid id) =>
        {
            var vendor = await vendorService.GetByIdAsync(id);

            return vendor switch
            {
                VendorResponseDto response => TypedResults.Ok(response),
                EntityErrorType invalidResult => NotFoundProblem(logger, invalidResult, LogLevel.Warning),
            };
        })
        .Produces<VendorResponseDto>(200)
        .ProducesProblem(404)
        .WithName("GetVendorById")
        .WithTags("Vendor");

        app.MapGet("/api/v{version:apiVersion}/vendor/list", [Authorize] async (IVendorService vendorService) =>
        {
            var vendors = await vendorService.GetAllAsync();
            return TypedResults.Ok(vendors);
        })
        .Produces<IEnumerable<VendorResponseDto>>(200)
        .WithName("GetAllVendors")
        .WithTags("Vendor");

        app.MapGet("/api/v{version:apiVersion}/vendor/count", [Authorize] async (IVendorService vendorService) =>
        {
            var count = await vendorService.GetCount();
            return TypedResults.Ok(count);
        })
        .Produces<int>(200)
        .WithName("GetVendorCount")
        .WithTags("Vendor");

        app.MapPost("/api/v{version:apiVersion}/vendor", [Authorize] async Task<Results<Ok<VendorResponseDto>, ProblemHttpResult>> (ILogger<VendorEndpoints> logger, IVendorService vendorService, PostVendorDto vendorDto, HttpContext httpContext) =>
        {
            var vendor = await vendorService.AddAsync(vendorDto, httpContext.User.Identity?.Name ?? "Unknown");

            return vendor switch
            {
                VendorResponseDto response => TypedResults.Ok(response),
                EntityAlreadyExists entityExists => ConflictProblem(logger, entityExists),
            };
        })
        .Produces<VendorResponseDto>(200)
        .ProducesProblem(409)
        .ProducesValidationProblem()
        .WithName("CreateVendor")
        .WithTags("Vendor");

        app.MapDelete("/api/v{version:apiVersion}/vendor/{id}", [Authorize] async (IVendorService vendorService, Guid id) =>
        {
            await vendorService.DeleteAsync(id);
            return TypedResults.NoContent();
        })
        .Produces(204)
        .WithName("DeleteVendor")
        .WithTags("Vendor");

        app.MapPut("/api/v{version:apiVersion}/vendor/{id}", [Authorize] async Task<Results<Ok<VendorResponseDto>, ProblemHttpResult>> (ILogger<VendorEndpoints> logger, IVendorService vendorService, Guid id, UpdateVendorDto vendorDto) =>
        {
            var result = await vendorService.UpdateAsync(id, vendorDto);

            return result switch
            {
                VendorResponseDto response => TypedResults.Ok(response),
                EntityErrorType invalidResult => NotFoundProblem(logger, invalidResult, LogLevel.Error),
            };
        })
        .Produces<VendorResponseDto>(200)
        .ProducesProblem(404)
        .ProducesValidationProblem()
        .WithName("UpdateVendor")
        .WithTags("Vendor");
    }

    private static ProblemHttpResult NotFoundProblem(ILogger<VendorEndpoints> logger, EntityErrorType error, LogLevel logLevel)
    {
        logger.Log(logLevel, "Record not found. {message}", error.GetMessage());
        return TypedResults.Problem(statusCode: 404, detail: error.GetMessage());
    }

    private static ProblemHttpResult ConflictProblem(ILogger<VendorEndpoints> logger, EntityAlreadyExists error)
    {
        logger.LogWarning("Entity already exists. {message}", error.GetMessage());
        return TypedResults.Problem(statusCode: 409, detail: error.GetMessage());
    }
}
