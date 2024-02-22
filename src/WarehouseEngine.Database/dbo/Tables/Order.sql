CREATE TABLE [Order](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [CustomerId] INT      NOT NULL,
    [ShippingAddress1]    NVARCHAR(80) NOT NULL,
    [ShippingAddress2]    NVARCHAR(80) NULL,
    [ShippingCity]    NVARCHAR(32) NOT NULL,
    [ShippingState]    NVARCHAR(2) NOT NULL,
    [ShippingZipCode]    NVARCHAR(11) NOT NULL,
    [OrderDate]  DATE     NOT NULL,
    [Status]     TINYINT  NOT NULL,
    [DateCreated] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] VARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] VARCHAR(80) NOT NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id])
);
GO
