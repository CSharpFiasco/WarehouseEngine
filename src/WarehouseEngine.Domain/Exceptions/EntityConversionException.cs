using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseEngine.Domain.Exceptions;

[Serializable]
[SuppressMessage("ISerializable should be implemented correctly", "S3925: Api is Obsolete and should not be implemented https://github.com/dotnet/docs/issues/34893.")]
public class EntityConversionException<T, U>: Exception
{
    [NonSerialized]
    private const string DefaultMessage = $"Failed to convert entity from type {nameof(T)} to type {nameof(U)}.";

    public EntityConversionException() : base(DefaultMessage) { }

    public EntityConversionException(string? message) : base($"{DefaultMessage} {message}") { }

    public EntityConversionException(string? message, Exception? innerException) : base($"{DefaultMessage} {message}", innerException) { }
}
