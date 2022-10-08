using System.Runtime.Serialization;

namespace WarehouseEngine.Domain.Exceptions;

[Serializable]
public sealed class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException()
    {
    }

    public EntityAlreadyExistsException(string? message) : base(message)
    {
    }

    private EntityAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public EntityAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
