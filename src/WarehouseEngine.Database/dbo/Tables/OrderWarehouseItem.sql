CREATE TABLE [OrderWarehouseItem](
    [OrderId]         UNIQUEIDENTIFIER NOT NULL,
    [WarehouseItemId]         UNIQUEIDENTIFIER NOT NULL,
    [Quantity] INT NOT NULL,
    CONSTRAINT [PK_OrderWarehouseItem] PRIMARY KEY CLUSTERED ([OrderId] ASC, [WarehouseItemId] ASC),
    CONSTRAINT [FK_OrderWarehouseItem_Order] FOREIGN KEY ([OrderId]) REFERENCES [Order]([Id]),
    CONSTRAINT [FK_OrderWarehouseItem_Warehouse] FOREIGN KEY ([WarehouseItemId]) REFERENCES [WarehouseItem]([Id]),
    CONSTRAINT [CHK_OrderWarehouseItem_QuantityIsNotNegative] CHECK ([Quantity] >= 0)
);
GO

CREATE NONCLUSTERED INDEX [IX_OrderWarehouseItem01] ON [OrderWarehouseItem]([OrderId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_OrderWarehouseItem02] ON [OrderWarehouseItem]([WarehouseItemId] ASC);
