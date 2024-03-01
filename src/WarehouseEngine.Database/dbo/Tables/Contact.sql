CREATE TABLE [Contact](
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [FirstName]    NVARCHAR(80) NULL,
    [LastName]     NVARCHAR(80) NULL,
    [Email]        NVARCHAR(60) NULL,
    [Address1]     NVARCHAR(80) NOT NULL,
    [Address2]     NVARCHAR(80) NULL,
    [City]         NVARCHAR(32) NOT NULL,
    [State]        NVARCHAR(2)  NOT NULL,
    [ZipCode]      NVARCHAR(11) NOT NULL,
    [DateCreated]  DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy]    NVARCHAR(80) NOT NULL,
    [DateModified] DATETIME2(7) NULL,
    [ModifiedBy]   NVARCHAR(80) NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
