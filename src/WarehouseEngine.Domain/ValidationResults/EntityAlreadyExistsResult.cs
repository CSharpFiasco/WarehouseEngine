using System.ComponentModel.DataAnnotations;

namespace WarehouseEngine.Domain.ValidationResults;

[Serializable]
public sealed class EntityAlreadyExistsResult(Type type) : ValidationResult(GetDefaultMessage(type))
{
    public static string GetDefaultMessage(Type type)
    {
        return $"This is {type} entity already exists";
    }
}
