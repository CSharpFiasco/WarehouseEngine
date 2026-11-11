using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;
using WarehouseEngine.Domain.ValidationResults;

namespace WarehouseEngine.Api.Endpoints;

internal class ItemEndpoints
{
    protected ItemEndpoints() { }

    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v{version:apiVersion}/item/{id}", [Authorize] async Task<Results<Ok<ItemResponseDto>, ProblemHttpResult>> (ILogger<ItemEndpoints> logger, IItemService itemService, Guid id) =>
        {
            var item = await itemService.GetByIdAsync(id);

            return item switch
            {
                ItemResponseDto response => TypedResults.Ok(response),
                EntityDoesNotExistResult error => RetrievalProblem(logger, error),
            };
        })
        .Produces<ItemResponseDto>(200)
        .ProducesProblem(400)
        .WithName("GetItemById")
        .WithTags("Item");

        app.MapGet("/api/v{version:apiVersion}/item/count", [Authorize] async (IItemService itemService) =>
        {
            var count = await itemService.GetCount();
            return TypedResults.Ok(count);
        })
        .Produces<int>(200)
        .WithName("GetItemCount")
        .WithTags("Item");

        app.MapPost("/api/v{version:apiVersion}/item", [Authorize] async Task<Results<Ok<ItemResponseDto>, ProblemHttpResult>> (ILogger<ItemEndpoints> logger, IItemService itemService, PostItemDto itemDto) =>
        {
            var item = await itemService.AddAsync(itemDto);

            return item switch
            {
                ItemResponseDto response => TypedResults.Ok(response),
                EntityAlreadyExistsResult error => CreationProblem(logger, error),
            };
        })
        .Produces<ItemResponseDto>(200)
        .ProducesProblem(400)
        .ProducesValidationProblem()
        .WithName("CreateItem")
        .WithTags("Item");
    }

    private static ProblemHttpResult RetrievalProblem(ILogger<ItemEndpoints> logger, EntityDoesNotExistResult error)
    {
        logger.LogError("Error retrieving item. {message}", error.ErrorMessage);
        return TypedResults.Problem(error.ErrorMessage, statusCode: 400);
    }

    private static ProblemHttpResult CreationProblem(ILogger<ItemEndpoints> logger, EntityAlreadyExistsResult error)
    {
        logger.LogError("Error creating item. {message}", error.ErrorMessage);
        return TypedResults.Problem(error.ErrorMessage, statusCode: 400);
    }
}
