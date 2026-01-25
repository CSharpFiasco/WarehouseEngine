using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;

namespace WarehouseEngine.Api.Endpoints;

public class VendorEndpoints
{
    protected VendorEndpoints() { }

    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v{version:apiVersion}/vendor/{id}", [Authorize] async (ILogger<VendorEndpoints> logger, IVendorService vendorService, Guid id) =>
        {
            var vendor = await vendorService.GetByIdAsync(id);

            return vendor.Match<Results<Ok<VendorResponseDto>, ProblemHttpResult>>(
                vendor => TypedResults.Ok(vendor),
                invalidResult =>
                {
                    logger.LogWarning("Record not found. {message}", invalidResult.GetMessage());
                    return TypedResults.Problem(statusCode: 404, detail: invalidResult.GetMessage());
                });
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

        app.MapPost("/api/v{version:apiVersion}/vendor", [Authorize] async (ILogger<VendorEndpoints> logger, IVendorService vendorService, PostVendorDto vendorDto, HttpContext httpContext) =>
        {
            var vendor = await vendorService.AddAsync(vendorDto, httpContext.User.Identity?.Name ?? "Unknown");

            return vendor.Match<Results<Ok<VendorResponseDto>, ProblemHttpResult>>(
                vendor => TypedResults.Ok(vendor),
                entityExists =>
                {
                    logger.LogWarning("Entity already exists. {message}", entityExists.GetMessage());
                    return TypedResults.Problem(statusCode: 409, detail: entityExists.GetMessage());
                });
        })
        .Produces<VendorResponseDto>(200)
        .ProducesProblem(409)
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

        app.MapPut("/api/v{version:apiVersion}/vendor/{id}", [Authorize] async (ILogger<VendorEndpoints> logger, IVendorService vendorService, Guid id, UpdateVendorDto vendorDto) =>
        {
            vendorDto.Id = id;
            var vendor = (Vendor)vendorDto;
            var result = await vendorService.UpdateAsync(id, vendor);

            return result.Match<Results<Ok<VendorResponseDto>, ProblemHttpResult>>(
                vendor => TypedResults.Ok(vendor),
                invalidResult =>
                {
                    logger.LogError("Record not found. {message}", invalidResult.GetMessage());
                    return TypedResults.Problem(statusCode: 404, detail: invalidResult.GetMessage());
                });
        })
        .Produces<VendorResponseDto>(200)
        .ProducesProblem(404)
        .WithName("UpdateVendor")
        .WithTags("Vendor");
    }
}
