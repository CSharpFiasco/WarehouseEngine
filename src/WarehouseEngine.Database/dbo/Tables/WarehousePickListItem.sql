CREATE TABLE [dbo].[WarehousePickListItem] (
    [PickListId]        UNIQUEIDENTIFIER NOT NULL,
    [WarehouseItemId]   UNIQUEIDENTIFIER NOT NULL,
    [OrderId]           UNIQUEIDENTIFIER NOT NULL,
    [QuantityToPick]    INT   NOT NULL,
    [QuantityPicked]    INT   NOT NULL DEFAULT(0),
    [BinLocation]       NVARCHAR(50)     NULL,
    [PickSequence]      INT              NOT NULL DEFAULT(0),
    [Status]            TINYINT          NOT NULL DEFAULT(0), -- 0=Pending, 1=Partial, 2=Completed, 3=Skipped
    [Notes]             NVARCHAR(255)    NULL,
    CONSTRAINT [PK_WarehousePickListItem] PRIMARY KEY CLUSTERED ([PickListId] ASC, [WarehouseItemId] ASC),
    CONSTRAINT [FK_WarehousePickListItem_PickList] FOREIGN KEY ([PickListId]) REFERENCES [dbo].[WarehousePickList] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_WarehousePickListItem_WarehouseItem] FOREIGN KEY ([WarehouseItemId]) REFERENCES [dbo].[WarehouseItem] ([Id]),
    CONSTRAINT [FK_WarehousePickListItem_OrderWarehouseItem] FOREIGN KEY ([OrderId], [WarehouseItemId]) REFERENCES [dbo].[OrderWarehouseItem] ([OrderId], [WarehouseItemId])
);

GO
CREATE NONCLUSTERED INDEX [IX_WarehousePickListItem_WarehouseItemId] ON [dbo].[WarehousePickListItem] ([WarehouseItemId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_WarehousePickListItem_Status] ON [dbo].[WarehousePickListItem] ([Status] ASC);