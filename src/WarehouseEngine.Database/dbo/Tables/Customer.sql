CREATE TABLE [Customer](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [Name]       VARCHAR(80) NOT NULL,
    [AddressId]  INT NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Customer_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_Customer01] ON [Customer]([AddressId] ASC);
