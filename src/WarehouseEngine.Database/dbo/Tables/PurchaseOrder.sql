CREATE TABLE [PurchaseOrder](
    [Id]         UNIQUEIDENTIFIER      NOT NULL,
    [OrderDate] DATE NOT NULL,
    [OrderNumber] NVARCHAR(255) NOT NULL,
    [Status] TINYINT NOT NULL,
    [DateCreated] DATETIME2(7) NOT NULL,
    [CreatedBy] NVARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] NVARCHAR(80) NULL,
    CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED ([Id] ASC)
);
