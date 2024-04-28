using System.Diagnostics.CodeAnalysis;

namespace WarehouseEngine.Domain.Exceptions;

[Serializable]
[SuppressMessage("ISerializable should be implemented correctly", "S3925: Api is Obsolete and should not be implemented https://github.com/dotnet/docs/issues/34893")]
[Obsolete("Exception is obsolete. Use EntityAlreadyExistsResult")]
public sealed class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException(Type type): base(GetDefaultMessage(type, null))
    {
    }

    public EntityAlreadyExistsException(Type type, string? message) : base(GetDefaultMessage(type, message))
    {
    }

    public EntityAlreadyExistsException(Type type,string? message, Exception? innerException) : base(GetDefaultMessage(type, message), innerException)
    {
    }

    private static string GetDefaultMessage(Type type, string? message)
    {
        return message ?? $"This is {type} entity does not exist";
    }
}
