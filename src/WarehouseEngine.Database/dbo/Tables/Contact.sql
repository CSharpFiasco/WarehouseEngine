CREATE TABLE [Contact](
    [Id]         INT         NOT NULL IDENTITY(1, 1),
    [FirstName]  VARCHAR(80) NULL,
    [LastName]   VARCHAR(80) NULL,
    [Email]      VARCHAR(60) NULL,
    [Address1]    VARCHAR(80) NOT NULL,
    [Address2]    VARCHAR(80) NULL,
    [City]    VARCHAR(32) NOT NULL,
    [State]    VARCHAR(2) NOT NULL,
    [ZipCode]    VARCHAR(11) NOT NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
