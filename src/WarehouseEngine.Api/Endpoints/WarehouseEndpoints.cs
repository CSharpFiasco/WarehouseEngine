using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
using WarehouseEngine.Application.Commands;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Api.Endpoints;

internal class WarehouseEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v{version:apiVersion}/warehouse/{id}", [Authorize] async Task<Results<Ok<WarehouseResponseDto>, ProblemHttpResult>> (ILogger<WarehouseEndpoints> logger, IWarehouseQueries warehouseQueries, Guid id, CancellationToken cancellationToken) =>
        {
            var warehouse = await warehouseQueries.GetByIdAsync(id, cancellationToken);
            return warehouse switch
            {
                WarehouseResponseDto response => TypedResults.Ok(response),
                EntityErrorType invalidResult => NotFoundProblem(logger, invalidResult),
            };
        })
        .Produces<WarehouseResponseDto>(200)
        .WithName("GetWarehouseById")
        .WithTags("Warehouse");

        app.MapGet("/api/v{version:apiVersion}/warehouse/count", [Authorize] async (IWarehouseQueries warehouseQueries, CancellationToken cancellationToken) =>
        {
            var count = await warehouseQueries.GetCount(cancellationToken);
            return TypedResults.Ok(count);
        })
        .Produces<int>(200)
        .WithName("GetWarehouseCount")
        .WithTags("Warehouse");

        app.MapGet("/api/v{version:apiVersion}/warehouse/list", [Authorize] async (IWarehouseQueries warehouseQueries, CancellationToken cancellationToken) =>
        {
            var warehouses = await warehouseQueries.GetAllAsync(cancellationToken);
            return TypedResults.Ok(warehouses);
        })
        .Produces<IEnumerable<WarehouseResponseDto>>(200)
        .WithName("GetAllWarehouses")
        .WithTags("Warehouse");

        app.MapPost("/api/v{version:apiVersion}/warehouse", [Authorize] async Task<Results<Created, ProblemHttpResult>> (ILogger<WarehouseEndpoints> logger, IWarehouseEngineContext context, IIdGenerator idGenerator, CreateWarehouseCommand command, CancellationToken cancellationToken) =>
        {
            var warehouse = Warehouse.Create(idGenerator.NewId(), command.Name);
            if (await context.Warehouse.AnyAsync(w => w.Id == warehouse.Id, cancellationToken))
            {
                var entityExists = new EntityAlreadyExists();
                logger.LogWarning("Entity already exists. {message}", entityExists.GetMessage());
                return TypedResults.Problem(statusCode: 409, detail: entityExists.GetMessage());
            }

            context.Warehouse.Add(warehouse);
            await context.SaveChangesAsync(cancellationToken);
            return TypedResults.Created();
        })
        .Produces(201)
        .ProducesValidationProblem()
        .ProducesProblem(409)
        .WithName("CreateWarehouse")
        .WithTags("Warehouse");
    }

    private static ProblemHttpResult NotFoundProblem(ILogger<WarehouseEndpoints> logger, EntityErrorType error)
    {
        logger.LogError("Record not found. {message}", error.GetMessage());
        return TypedResults.Problem(statusCode: 404, detail: error.GetMessage());
    }
}
