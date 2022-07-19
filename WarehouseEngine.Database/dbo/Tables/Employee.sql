CREATE TABLE [Employee](
    [Id]         INT      NOT NULL IDENTITY(1, 1),
    [FirstName]   VARCHAR(30) NOT NULL,
    [MiddleName]  VARCHAR(30) NULL,
    [LastName]   VARCHAR(30) NOT NULL,
    [UserName]   VARCHAR(32) NOT NULL,
    [PositionId] INT NOT NULL,
    [SupervisorEmployeeId] INT NOT NULL,
    [SocialSecurityNumberHash] VARBINARY(32) NULL,
    [SocialSecuritySerialNumber] CHAR(4) NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employee_Position] FOREIGN KEY ([PositionId]) REFERENCES [Position]([Id]),
    CONSTRAINT [FK_Employee_SupervisorEmployee] FOREIGN KEY ([SupervisorEmployeeId]) REFERENCES [Employee]([Id])
);
GO

CREATE NONCLUSTERED INDEX [IX_Employee01] ON [Employee]([PositionId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Employee02] ON [Employee]([SupervisorEmployeeId] ASC);
