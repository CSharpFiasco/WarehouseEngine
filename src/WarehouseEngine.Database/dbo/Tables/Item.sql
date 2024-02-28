CREATE TABLE [Item](
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Sku]         NVARCHAR(12) NOT NULL,
    [Description] NVARCHAR(120) NOT NULL,
    [IsActive]    BIT         NOT NULL,
    [DateCreated] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] NVARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] NVARCHAR(80) NOT NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC)
);
