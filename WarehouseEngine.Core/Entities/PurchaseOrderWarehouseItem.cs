﻿namespace WarehouseEngine.Core.Entities;

public partial class PurchaseOrderWarehouseItem
{
    public int PurchaseOrderId { get; set; }
    public int WarehouseItemId { get; set; }
    public int Quantity { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
    public virtual WarehouseItem WarehouseItem { get; set; } = null!;
}
