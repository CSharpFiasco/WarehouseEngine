﻿CREATE TABLE [Vendor](
    [Id]         UNIQUEIDENTIFIER      NOT NULL,
    [Name]       NVARCHAR(80),
    CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED ([Id] ASC)
);