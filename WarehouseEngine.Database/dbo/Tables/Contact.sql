CREATE TABLE [Contact](
    [Id]         INT         NOT NULL IDENTITY(1, 1),
    [FirstName]  VARCHAR(80) NULL,
    [LastName]   VARCHAR(80) NULL,
    [Email]      VARCHAR(60) NULL,
    [AddressId]  INT         NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Contact_Address] FOREIGN KEY ([AddressId]) REFERENCES [Address]([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_Contact01] ON [Contact]([AddressId] ASC);
