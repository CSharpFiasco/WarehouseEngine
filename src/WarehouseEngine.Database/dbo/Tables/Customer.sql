CREATE TABLE [Customer](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [Name]       VARCHAR(80) NOT NULL,
    [BillingAddress1]    NVARCHAR(80) NOT NULL,
    [BillingAddress2]    NVARCHAR(80) NULL,
    [BillingCity]    NVARCHAR(32) NOT NULL,
    [BillingState]    NVARCHAR(2) NOT NULL,
    [BillingZipCode]    NVARCHAR(11) NOT NULL,
    [ShippingAddress1]    NVARCHAR(80) NOT NULL,
    [ShippingAddress2]    NVARCHAR(80) NULL,
    [ShippingCity]    NVARCHAR(32) NOT NULL,
    [ShippingState]    NVARCHAR(2) NOT NULL,
    [ShippingZipCode]    NVARCHAR(11) NOT NULL,
    [DateCreated] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] VARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] VARCHAR(80) NOT NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
