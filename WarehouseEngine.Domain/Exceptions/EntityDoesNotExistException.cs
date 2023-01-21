using System.Runtime.Serialization;

namespace WarehouseEngine.Domain.Exceptions;

[Serializable]
public sealed class EntityDoesNotExistException<T> : Exception
{
    [NonSerialized]
    private const string DefaultMessage = $"This is {nameof(T)} entity does not exist";

    public EntityDoesNotExistException()
    {
    }

    public EntityDoesNotExistException(string? message) : base(message ?? DefaultMessage)
    {

    }

    private EntityDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EntityDoesNotExistException(string? message, Exception? innerException) : base(message ?? DefaultMessage, innerException)
    {
    }
}
