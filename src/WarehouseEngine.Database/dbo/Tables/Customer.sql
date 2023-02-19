CREATE TABLE [Customer](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [Name]       VARCHAR(80) NOT NULL,
    [BillingAddress1]    VARCHAR(80) NOT NULL,
    [BillingAddress2]    VARCHAR(80) NULL,
    [BillingCity]    VARCHAR(32) NOT NULL,
    [BillingState]    VARCHAR(2) NOT NULL,
    [BillingZipCode]    VARCHAR(11) NOT NULL,
    [ShippingAddress1]    VARCHAR(80) NOT NULL,
    [ShippingAddress2]    VARCHAR(80) NULL,
    [ShippingCity]    VARCHAR(32) NOT NULL,
    [ShippingState]    VARCHAR(2) NOT NULL,
    [ShippingZipCode]    VARCHAR(11) NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
