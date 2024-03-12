CREATE TABLE [WarehouseItem](
    [Id]         UNIQUEIDENTIFIER      NOT NULL,
    [WarehouseId]         UNIQUEIDENTIFIER      NOT NULL,
    [ItemId]         UNIQUEIDENTIFIER      NOT NULL,
    [Quantity] INT NOT NULL,
    [Price] DECIMAL(15, 3),
    [DateCreated] DATETIME2(7) NOT NULL,
    [CreatedBy] NVARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] NVARCHAR(80) NULL,
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
