﻿CREATE TABLE [Company](
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR(80)      NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([Id] ASC)
);