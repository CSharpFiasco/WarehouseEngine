using System.ComponentModel.DataAnnotations;

namespace WarehouseEngine.Domain.ValidationResults;

[Serializable]
public sealed class EntityDoesNotExistResult(Type type) : ValidationResult(GetDefaultMessage(type))
{
    public static string GetDefaultMessage(Type type)
    {
        return $"This is {type} entity does not exist";
    }
}
