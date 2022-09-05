CREATE TABLE [Address](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [Address1]    VARCHAR(80) NOT NULL,
    [Address2]    VARCHAR(80) NULL,
    [City]    VARCHAR(32) NOT NULL,
    [State]    VARCHAR(2) NOT NULL,
    [Zip]    VARCHAR(11) NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id] ASC)
);