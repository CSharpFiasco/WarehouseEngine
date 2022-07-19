CREATE TABLE [Vendor](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [Name]       VARCHAR(80),
    CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED ([Id] ASC)
);