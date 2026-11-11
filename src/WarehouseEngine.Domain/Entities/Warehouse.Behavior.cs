namespace WarehouseEngine.Domain.Entities;

public partial class Warehouse
{
    public const int MaxNameLength = 32;

    public static Warehouse Create(Guid id, string name)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Warehouse ID must not be empty.", nameof(id));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (name.Length > MaxNameLength)
        {
            throw new ArgumentOutOfRangeException(nameof(name), "Warehouse name must not exceed 32 characters.");
        }

        return new Warehouse { Id = id, Name = name };
    }
}
