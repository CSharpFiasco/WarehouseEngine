CREATE TABLE [PurchaseOrderWarehouseItem](
    [PurchaseOrderId]         INT      NOT NULL,
    [WarehouseItemId]         INT      NOT NULL,
    [Quantity]                INT      NOT NULL,
    CONSTRAINT [PK_PurchaseOrderWarehouseItem] PRIMARY KEY CLUSTERED ([PurchaseOrderId] ASC, [WarehouseItemId] ASC),
    CONSTRAINT [FK_PurchaseOrderWarehouseItem_PurchaseOrder] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [PurchaseOrder]([Id]),
    CONSTRAINT [FK_PurchaseOrderWarehouseItem_WarehouseItem] FOREIGN KEY ([WarehouseItemId]) REFERENCES [WarehouseItem]([Id]),
    CONSTRAINT [CHK_PurchaseOrderWarehouseItem_QuantityIsNotNegative] CHECK ([Quantity] >= 0)
);
GO

CREATE NONCLUSTERED INDEX [IX_PurchaseOrderWarehouseItem01] ON [PurchaseOrderWarehouseItem]([PurchaseOrderId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_PurchaseOrderWarehouseItem02] ON [PurchaseOrderWarehouseItem]([WarehouseItemId] ASC);

