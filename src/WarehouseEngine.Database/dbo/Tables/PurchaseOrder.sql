CREATE TABLE [PurchaseOrder](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [OrderDate] DATE NOT NULL,
    [OrderNumber] VARCHAR(255) NOT NULL,
    [Status] TINYINT NOT NULL,
    CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED ([Id] ASC)
);
