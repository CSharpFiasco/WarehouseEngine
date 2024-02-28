CREATE TABLE [Employee](
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [FirstName]   NVARCHAR(30) NOT NULL,
    [MiddleName]  NVARCHAR(30) NULL,
    [LastName]   NVARCHAR(30) NOT NULL,
    [UserName]   NVARCHAR(32) NOT NULL,
    [PositionId] UNIQUEIDENTIFIER NOT NULL,
    [SupervisorEmployeeId] UNIQUEIDENTIFIER NOT NULL,
    [SocialSecurityNumberHash] VARBINARY(32) NULL,
    [SocialSecuritySerialNumber] NCHAR(4) NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employee_Position] FOREIGN KEY ([PositionId]) REFERENCES [Position]([Id]),
    CONSTRAINT [FK_Employee_SupervisorEmployee] FOREIGN KEY ([SupervisorEmployeeId]) REFERENCES [Employee]([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_Employee01] ON [Employee]([PositionId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Employee02] ON [Employee]([SupervisorEmployeeId] ASC);
