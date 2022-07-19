CREATE TABLE [Order](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [CustomerId] INT      NOT NULL,
    [AddressId]  INT      NOT NULL,
    [Status]     TINYINT  NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]),
    CONSTRAINT [FK_Order_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_Order01] ON [Order]([AddressId] ASC);
