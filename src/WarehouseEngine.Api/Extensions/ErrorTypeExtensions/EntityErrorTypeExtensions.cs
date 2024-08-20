using WarehouseEngine.Domain.ErrorTypes;

namespace WarehouseEngine.Api.Extensions.ErrorTypeExtensions;
public static class EntityErrorTypeExtensions
{
    public static string GetMessage(this EntityErrorType errorType)
    {
        if (errorType is EntityAlreadyExists)
        {
            return "Entity already exists";
        }
        else if (errorType is EntityDoesNotExist)
        {
            return "Entity does not exist";
        }
        else
        {
            return "Unknown error";
        }
    }
}
