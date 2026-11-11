namespace WarehouseEngine.Domain.Entities;

public partial class PurchaseOrder
{
    public const int MaxOrderNumberLength = 255;

    public static PurchaseOrder Create(Guid id, DateTime orderDate, string orderNumber, byte status = 0)
    {
        EntityRules.Identity(id, nameof(id));
        EntityRules.RequiredText(orderNumber, MaxOrderNumberLength, nameof(orderNumber));
        return new PurchaseOrder { Id = id, OrderDate = orderDate, OrderNumber = orderNumber, Status = status };
    }

    public void ReviseDetails(DateTime orderDate, string orderNumber)
    {
        EntityRules.RequiredText(orderNumber, MaxOrderNumberLength, nameof(orderNumber));
        OrderDate = orderDate;
        OrderNumber = orderNumber;
    }

    public void ChangeStatus(byte status) => Status = status;
}
