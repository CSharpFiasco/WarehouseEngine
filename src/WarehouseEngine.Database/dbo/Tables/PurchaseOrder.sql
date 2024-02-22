CREATE TABLE [PurchaseOrder](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [OrderDate] DATE NOT NULL,
    [OrderNumber] VARCHAR(255) NOT NULL,
    [Status] TINYINT NOT NULL,
    [DateCreated] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] VARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] VARCHAR(80) NOT NULL,
    CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED ([Id] ASC)
);
