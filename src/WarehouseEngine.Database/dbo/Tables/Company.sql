﻿CREATE TABLE [Company](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [Name]       VARCHAR(80) NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([Id] ASC)
);