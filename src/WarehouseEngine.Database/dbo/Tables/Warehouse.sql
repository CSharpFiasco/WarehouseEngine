CREATE TABLE [Warehouse](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [Name]       VARCHAR(32) NOT NULL,
    CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED ([Id] ASC)
);
