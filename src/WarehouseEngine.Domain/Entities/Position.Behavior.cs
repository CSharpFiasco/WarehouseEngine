namespace WarehouseEngine.Domain.Entities;

public partial class Position
{
    public const int MaxNameLength = 80;

    public static Position Create(Guid id, string name)
    {
        EntityRules.Identity(id, nameof(id));
        EntityRules.RequiredText(name, MaxNameLength, nameof(name));
        return new Position { Id = id, Name = name };
    }

    public void Rename(string name)
    {
        EntityRules.RequiredText(name, MaxNameLength, nameof(name));
        Name = name;
    }
}
