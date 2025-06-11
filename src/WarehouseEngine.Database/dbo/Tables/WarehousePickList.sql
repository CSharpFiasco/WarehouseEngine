CREATE TABLE [dbo].[WarehousePickList] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
    [OrderId]              UNIQUEIDENTIFIER NOT NULL,
    [WarehouseId]          UNIQUEIDENTIFIER NOT NULL,
    [PickListNumber]       NVARCHAR(25)     NOT NULL,
    [Status]               TINYINT          NOT NULL, -- 0=Created, 1=In Progress, 2=Completed, 3=Cancelled
    [CreatedDate]          DATETIME2(7)     NOT NULL,
    [CreatedByEmployeeId]  UNIQUEIDENTIFIER NOT NULL,
    [AssignedToEmployeeId] UNIQUEIDENTIFIER NULL,
    [StartedDate]          DATETIME2(7)     NULL,
    [CompletedDate]        DATETIME2(7)     NULL,
    [Priority]             TINYINT          NOT NULL DEFAULT(1), -- 1=Normal, 2=Rush, 3=Emergency
    [Notes]                NVARCHAR(500)    NULL,
    CONSTRAINT [PK_WarehousePickList] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WarehousePickList_Order] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]),
    CONSTRAINT [FK_WarehousePickList_Warehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouse] ([Id]),
    CONSTRAINT [FK_WarehousePickList_CreatedByEmployee] FOREIGN KEY ([CreatedByEmployeeId]) REFERENCES [dbo].[Employee] ([Id]),
    CONSTRAINT [FK_WarehousePickList_AssignedToEmployee] FOREIGN KEY ([AssignedToEmployeeId]) REFERENCES [dbo].[Employee] ([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_WarehousePickList_OrderId] ON [dbo].[WarehousePickList] ([OrderId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_WarehousePickList_WarehouseId] ON [dbo].[WarehousePickList] ([WarehouseId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_WarehousePickList_Status] ON [dbo].[WarehousePickList] ([Status] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_WarehousePickList_AssignedToEmployeeId] ON [dbo].[WarehousePickList] ([AssignedToEmployeeId] ASC);