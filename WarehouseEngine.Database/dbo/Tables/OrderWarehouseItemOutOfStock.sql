CREATE TABLE [OrderWarehouseItemOutOfStock](
    [OrderId]         INT      NOT NULL,
    [WarehouseItemId]         INT      NOT NULL,
    CONSTRAINT [PK_OrderWarehouseItemOutOfStock] PRIMARY KEY CLUSTERED ([OrderId] ASC, [WarehouseItemId] ASC),
    CONSTRAINT [FK_OrderWarehouseItemOutOfStock_Order] FOREIGN KEY ([OrderId]) REFERENCES [Order]([Id]),
    CONSTRAINT [FK_OrderWarehouseItemOutOfStock_WarehouseItemOutOfStock] FOREIGN KEY ([WarehouseItemId]) REFERENCES [WarehouseItem]([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_OrderWarehouseItemOutOfStock01] ON [OrderWarehouseItemOutOfStock]([OrderId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_OrderWarehouseItemOutOfStock02] ON [OrderWarehouseItemOutOfStock]([WarehouseItemId] ASC);
