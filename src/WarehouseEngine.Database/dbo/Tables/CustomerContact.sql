CREATE TABLE [CustomerContact](
    [CustomerId]        UNIQUEIDENTIFIER      NOT NULL,
    [ContactId]         UNIQUEIDENTIFIER      NOT NULL,
    CONSTRAINT [PK_CustomerContact] PRIMARY KEY CLUSTERED ([CustomerId] ASC, [ContactId] ASC),
    CONSTRAINT [FK_CustomerContact_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]),
    CONSTRAINT [FK_CustomerContact_Contact] FOREIGN KEY ([ContactId]) REFERENCES [Contact]([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_CustomerContact01] ON [CustomerContact]([CustomerId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CustomerContact02] ON [CustomerContact]([ContactId] ASC);

