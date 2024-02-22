CREATE TABLE [Contact](
    [Id]         INT         NOT NULL IDENTITY(1, 1),
    [FirstName]  VARCHAR(80) NULL,
    [LastName]   VARCHAR(80) NULL,
    [Email]      VARCHAR(60) NULL,
    [Address1]    NVARCHAR(80) NOT NULL,
    [Address2]    NVARCHAR(80) NULL,
    [City]    NVARCHAR(32) NOT NULL,
    [State]    NVARCHAR(2) NOT NULL,
    [ZipCode]    NVARCHAR(11) NOT NULL,
    [DateCreated] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] VARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy] VARCHAR(80) NOT NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
