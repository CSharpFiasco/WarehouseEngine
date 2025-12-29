using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
public static class EntityErrorTypeExtensions
{
    private static readonly string EntityAlreadyExistsMessage = "Entity already exists";
    private static readonly string EntityDoesNotExistMessage = "Entity does not exist";
    private static readonly string UnknownErrorMessage = "Unknown error";
    public static string GetMessage(this EntityErrorType errorType)
    {
        if (errorType is EntityAlreadyExists)
        {
            return EntityAlreadyExistsMessage;
        }
        else if (errorType is EntityDoesNotExist)
        {
            return EntityDoesNotExistMessage;
        }
        else
        {
            return UnknownErrorMessage;
        }
    }
}
