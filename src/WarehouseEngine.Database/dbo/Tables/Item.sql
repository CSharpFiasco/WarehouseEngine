CREATE TABLE [Item](
    [Id]          INT         NOT NULL IDENTITY(1, 1),
    [Sku]         VARCHAR(12) NOT NULL,
    [Description] VARCHAR(120) NOT NULL,
    [IsActive]    BIT         NOT NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC)
);
