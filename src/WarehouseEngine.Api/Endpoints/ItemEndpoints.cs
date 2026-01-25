using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;

namespace WarehouseEngine.Api.Endpoints;

internal class ItemEndpoints
{
    protected ItemEndpoints() { }

    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v{version:apiVersion}/item/{id}", [Authorize] async (ILogger<ItemEndpoints> logger, IItemService itemService, Guid id) =>
        {
            var item = await itemService.GetByIdAsync(id);

            return item.Match<Results<Ok<ItemResponseDto>, ProblemHttpResult>>(
                item => TypedResults.Ok(item),
                error =>
                {
                    logger.LogError("Error retrieving item. {message}", error.ErrorMessage);
                    return TypedResults.Problem(error.ErrorMessage, statusCode: 400);
                });
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

        app.MapPost("/api/v{version:apiVersion}/item", [Authorize] async (ILogger<ItemEndpoints> logger, IItemService itemService, PostItemDto itemDto) =>
        {
            var item = await itemService.AddAsync(itemDto);

            return item.Match<Results<Ok<ItemResponseDto>, ProblemHttpResult>>(
                item => TypedResults.Ok(item),
                error =>
                {
                    logger.LogError("Error creating item. {message}", error.ErrorMessage);
                    return TypedResults.Problem(error.ErrorMessage, statusCode: 400);
                });
        })
        .Produces<ItemResponseDto>(200)
        .ProducesProblem(400)
        .WithName("CreateItem")
        .WithTags("Item");
    }
}
