CREATE TABLE [VendorItem](
    [VendorId]         UNIQUEIDENTIFIER      NOT NULL,
    [ItemId]         UNIQUEIDENTIFIER      NOT NULL,
    [DateCreated] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] NVARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] NVARCHAR(80) NOT NULL,
    CONSTRAINT [PK_VendorItem] PRIMARY KEY CLUSTERED ([VendorId] ASC, [ItemId] ASC),
    CONSTRAINT [FK_VendorItem_Vendor] FOREIGN KEY ([VendorId]) REFERENCES [Vendor]([Id]),
    CONSTRAINT [FK_VendorItem_Item] FOREIGN KEY ([ItemId]) REFERENCES [Item]([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_VendorItem01] ON [VendorItem]([VendorId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_VendorItem02] ON [VendorItem]([ItemId] ASC);

