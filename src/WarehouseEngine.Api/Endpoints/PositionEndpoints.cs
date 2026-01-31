using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using WarehouseEngine.Application.Dtos;
using WarehouseEngine.Application.Interfaces;

namespace WarehouseEngine.Api.Endpoints;

internal class PositionEndpoints
{
    protected PositionEndpoints() { }

    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v{version:apiVersion}/position/{id}", [Authorize] async (ILogger<PositionEndpoints> logger, IPositionService positionService, Guid id) =>
        {
            var position = await positionService.GetByIdAsync(id);

            return position.Match<Results<Ok<PositionResponseDto>, ProblemHttpResult>>(
                position => TypedResults.Ok(position),
                error =>
                {
                    logger.LogError("Error retrieving position. {message}", error.ErrorMessage);
                    return TypedResults.Problem(error.ErrorMessage, statusCode: 404);
                });
        })
        .Produces<PositionResponseDto>(200)
        .ProducesProblem(404)
        .WithName("GetPositionById")
        .WithTags("Position");

        app.MapGet("/api/v{version:apiVersion}/position/count", [Authorize] async (IPositionService positionService) =>
        {
            var count = await positionService.GetCount();
            return TypedResults.Ok(count);
        })
        .Produces<int>(200)
        .WithName("GetPositionCount")
        .WithTags("Position");

        app.MapPost("/api/v{version:apiVersion}/position", [Authorize] async (ILogger<PositionEndpoints> logger, IPositionService positionService, PostPositionDto positionDto) =>
        {
            var position = await positionService.AddAsync(positionDto);

            return position.Match<Results<Ok<PositionResponseDto>, ProblemHttpResult>>(
                position => TypedResults.Ok(position),
                error =>
                {
                    logger.LogError("Error creating position. {message}", error.ErrorMessage);
                    return TypedResults.Problem(error.ErrorMessage, statusCode: 400);
                });
        })
        .Produces<PositionResponseDto>(200)
        .ProducesProblem(400)
        .WithName("CreatePosition")
        .WithTags("Position");
    }
}
