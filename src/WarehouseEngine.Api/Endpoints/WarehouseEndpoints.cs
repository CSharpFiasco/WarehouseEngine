using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;

namespace WarehouseEngine.Api.Endpoints;

internal class WarehouseEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v{version:apiVersion}/warehouse/{id}", [Authorize] async (ILogger<WarehouseEndpoints> logger, IWarehouseService warehouseService, Guid id) =>
        {
            var warehouse = await warehouseService.GetByIdAsync(id);
            return warehouse.Match<Results<Ok<WarehouseResponseDto>, ProblemHttpResult>>(
                    warehouse => TypedResults.Ok(warehouse),
                    invalidResult =>
                    {
                        logger.LogError("Record not found. {message}", invalidResult.GetMessage());
                        return TypedResults.Problem(statusCode: 404, detail: invalidResult.GetMessage());
                    });
        })
        .Produces<WarehouseResponseDto>(200)
        .WithName("GetWarehouseById")
        .WithTags("Warehouse");

        app.MapGet("/api/v{version:apiVersion}/warehouse/count", [Authorize] async (IWarehouseService warehouseService) =>
        {
            var count = await warehouseService.GetCount();
            return TypedResults.Ok(count);
        })
        .Produces<int>(200)
        .WithName("GetWarehouseCount")
        .WithTags("Warehouse");

        app.MapPost("/api/v{version:apiVersion}/warehouse", [Authorize] async (ILogger<WarehouseEndpoints> logger, IWarehouseService warehouseService, PostWarehouseDto warehouseDto) =>
        {
            var warehouse = await warehouseService.AddAsync(warehouseDto);

            return warehouse.Match<Results<Created, ProblemHttpResult>>(
               warehouse => TypedResults.Created(),
               entityExists =>
               {
                   logger.LogWarning("Entity already exists. {message}", entityExists.GetMessage());

                   return TypedResults.Problem(statusCode: 409, detail: entityExists.GetMessage());
               });
        })
        .Produces(201)
        .ProducesProblem(409)
        .WithName("CreateWarehouse")
        .WithTags("Warehouse");
    }
}
