namespace WarehouseEngine.Domain.Entities;

public partial class Company
{
    public const int MaxNameLength = 80;

    public static Company Create(Guid id, string name)
    {
        EntityRules.Identity(id, nameof(id));
        EntityRules.RequiredText(name, MaxNameLength, nameof(name));
        return new Company { Id = id, Name = name };
    }

    public void Rename(string name)
    {
        EntityRules.RequiredText(name, MaxNameLength, nameof(name));
        Name = name;
    }
}
