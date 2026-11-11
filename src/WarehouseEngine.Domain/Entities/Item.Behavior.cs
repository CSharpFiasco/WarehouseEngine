namespace WarehouseEngine.Domain.Entities;

public partial class Item
{
    public const int MaxSkuLength = 12;
    public const int MaxDescriptionLength = 120;

    public static Item Create(Guid id, string sku, string description, bool isActive = false,
        DateTime? dateCreated = null, string? createdBy = null)
    {
        EntityRules.Identity(id, nameof(id));
        var item = new Item { Id = id, DateCreated = dateCreated, CreatedBy = createdBy };
        item.UpdateDetails(sku, description, isActive);
        return item;
    }

    public void UpdateDetails(string sku, string description, bool isActive)
    {
        EntityRules.RequiredText(sku, MaxSkuLength, nameof(sku));
        EntityRules.RequiredText(description, MaxDescriptionLength, nameof(description));
        Sku = sku;
        Description = description;
        IsActive = isActive;
    }
}
