using System.Diagnostics.CodeAnalysis;

namespace WarehouseEngine.Domain.Exceptions;

[Serializable]
[SuppressMessage("ISerializable should be implemented correctly", "S3925: Api is Obsolete and should not be implemented https://github.com/dotnet/docs/issues/34893")]
public sealed class EntityAlreadyExistsException<T> : Exception
{
    [NonSerialized]
    private const string DefaultMessage = $"This is {nameof(T)} entity does not exist";
    public EntityAlreadyExistsException()
    {
    }

    public EntityAlreadyExistsException(string? message) : base(message ?? DefaultMessage)
    {
    }

    public EntityAlreadyExistsException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    {
    }
}
