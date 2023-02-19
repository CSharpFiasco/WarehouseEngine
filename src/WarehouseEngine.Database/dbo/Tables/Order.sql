CREATE TABLE [Order](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [CustomerId] INT      NOT NULL,
    [ShippingAddress1]    VARCHAR(80) NOT NULL,
    [ShippingAddress2]    VARCHAR(80) NULL,
    [ShippingCity]    VARCHAR(32) NOT NULL,
    [ShippingState]    VARCHAR(2) NOT NULL,
    [ShippingZipCode]    VARCHAR(11) NOT NULL,
    [OrderDate]  DATE     NOT NULL,
    [Status]     TINYINT  NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id])
);
GO
