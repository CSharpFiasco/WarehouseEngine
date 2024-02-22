CREATE TABLE [WarehouseItem](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [WarehouseId]         INT      NOT NULL,
    [ItemId]         INT      NOT NULL,
    [Quantity] INT NOT NULL,
    [Price] DECIMAL(15, 3),
    [DateCreated] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] VARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] VARCHAR(80) NOT NULL,
    CONSTRAINT [PK_WarehouseItem] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WarehouseItem_Warehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouse]([Id]),
    CONSTRAINT [CHK_WarehouseItem_QuantityIsNotNegative] CHECK ([Quantity] >= 0),
    CONSTRAINT [CHK_WarehouseItem_PriceIsNotNegative] CHECK ([Price] > 0),
    CONSTRAINT [FK_WarehouseItem_Item] FOREIGN KEY ([ItemId]) REFERENCES [Item]([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_WarehouseItem01] ON [WarehouseItem]([WarehouseId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_WarehouseItem02] ON [WarehouseItem]([ItemId] ASC);
