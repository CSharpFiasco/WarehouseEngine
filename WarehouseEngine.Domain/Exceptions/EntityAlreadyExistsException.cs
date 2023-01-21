using System.Runtime.Serialization;

namespace WarehouseEngine.Domain.Exceptions;

[Serializable]
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

    private EntityAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EntityAlreadyExistsException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    {
    }
}
