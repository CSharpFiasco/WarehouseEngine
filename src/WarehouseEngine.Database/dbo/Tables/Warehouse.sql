CREATE TABLE [Warehouse](
    [Id]         UNIQUEIDENTIFIER      NOT NULL,
    [Name]       NVARCHAR(32) NOT NULL,
    CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED ([Id] ASC)
);
