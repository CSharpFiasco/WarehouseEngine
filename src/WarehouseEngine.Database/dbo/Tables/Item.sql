CREATE TABLE [Item](
    [Id]          INT         NOT NULL IDENTITY(1, 1),
    [Sku]         VARCHAR(12) NOT NULL,
    [Description] VARCHAR(120) NOT NULL,
    [IsActive]    BIT         NOT NULL,
    [DateCreated] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] VARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] VARCHAR(80) NOT NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC)
);
