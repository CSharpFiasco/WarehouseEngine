namespace WarehouseEngine.Domain.Entities;

public partial class Vendor
{
    public const int MaxNameLength = 80;

    public static Vendor Create(Guid id, string? name = null)
    {
        EntityRules.Identity(id, nameof(id));
        EntityRules.OptionalText(name, MaxNameLength, nameof(name));
        return new Vendor { Id = id, Name = name };
    }

    public void Rename(string? name)
    {
        EntityRules.OptionalText(name, MaxNameLength, nameof(name));
        Name = name;
    }
}
