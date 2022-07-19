﻿/*
Deployment script for WarehouseEngine

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "WarehouseEngine"
:setvar DefaultFilePrefix "WarehouseEngine"
:setvar DefaultDataPath "C:\Users\carlo\AppData\Local\Microsoft\VisualStudio\SSDT\"
:setvar DefaultLogPath "C:\Users\carlo\AppData\Local\Microsoft\VisualStudio\SSDT\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE,
                DISABLE_BROKER 
            WITH ROLLBACK IMMEDIATE;
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (QUERY_CAPTURE_MODE = ALL, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367), MAX_STORAGE_SIZE_MB = 100) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
PRINT N'Creating Table [dbo].[Address]...';


GO
CREATE TABLE [dbo].[Address] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME     NOT NULL,
    [CreateUser] VARCHAR (32) NOT NULL,
    [ModifyDate] DATETIME     NULL,
    [ModifyUser] VARCHAR (32) NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[Company]...';


GO
CREATE TABLE [dbo].[Company] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME     NOT NULL,
    [CreateUser] VARCHAR (32) NOT NULL,
    [ModifyDate] DATETIME     NULL,
    [ModifyUser] VARCHAR (32) NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[Customer]...';


GO
CREATE TABLE [dbo].[Customer] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (80) NOT NULL,
    [Address]    VARCHAR (80) NOT NULL,
    [Address2]   VARCHAR (80) NOT NULL,
    [City]       VARCHAR (32) NOT NULL,
    [State]      VARCHAR (2)  NOT NULL,
    [Zip]        VARCHAR (11) NOT NULL,
    [CreateDate] DATETIME     NOT NULL,
    [CreateUser] VARCHAR (32) NOT NULL,
    [ModifyDate] DATETIME     NULL,
    [ModifyUser] VARCHAR (32) NULL,
    CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[Employee]...';


GO
CREATE TABLE [dbo].[Employee] (
    [Id]                         INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]                  VARCHAR (30)   NOT NULL,
    [MiddleName]                 VARCHAR (30)   NULL,
    [LastName]                   VARCHAR (30)   NOT NULL,
    [UserName]                   VARCHAR (32)   NOT NULL,
    [PositionId]                 INT            NOT NULL,
    [SupervisorEmployeeId]       INT            NOT NULL,
    [SocialSecurityNumberHash]   VARBINARY (32) NULL,
    [SocialSecuritySerialNumber] CHAR (4)       NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Index [dbo].[Employee].[IX_Employee01]...';


GO
CREATE NONCLUSTERED INDEX [IX_Employee01]
    ON [dbo].[Employee]([PositionId] ASC);


GO
PRINT N'Creating Index [dbo].[Employee].[IX_Employee02]...';


GO
CREATE NONCLUSTERED INDEX [IX_Employee02]
    ON [dbo].[Employee]([SupervisorEmployeeId] ASC);


GO
PRINT N'Creating Table [dbo].[Item]...';


GO
CREATE TABLE [dbo].[Item] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Sku]         VARCHAR (12) NOT NULL,
    [Description] VARCHAR (80) NOT NULL,
    [IsActive]    BIT          NOT NULL,
    [CreateDate]  DATETIME     NOT NULL,
    [CreateUser]  VARCHAR (32) NOT NULL,
    [ModifyDate]  DATETIME     NULL,
    [ModifyUser]  VARCHAR (32) NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[Order]...';


GO
CREATE TABLE [dbo].[Order] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CustomerId] INT          NOT NULL,
    [CreateDate] DATETIME     NOT NULL,
    [CreateUser] VARCHAR (32) NOT NULL,
    [ModifyDate] DATETIME     NULL,
    [ModifyUser] VARCHAR (32) NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[OrderWarehouseItem]...';


GO
CREATE TABLE [dbo].[OrderWarehouseItem] (
    [OrderId]         INT          NOT NULL,
    [WarehouseItemId] INT          NOT NULL,
    [Quantity]        INT          NOT NULL,
    [CreateDate]      DATETIME     NOT NULL,
    [CreateUser]      VARCHAR (32) NOT NULL,
    [ModifyDate]      DATETIME     NULL,
    [ModifyUser]      VARCHAR (32) NULL,
    CONSTRAINT [PK_OrderWarehouseItem] PRIMARY KEY CLUSTERED ([OrderId] ASC, [WarehouseItemId] ASC)
);


GO
PRINT N'Creating Index [dbo].[OrderWarehouseItem].[IX_OrderWarehouseItem01]...';


GO
CREATE NONCLUSTERED INDEX [IX_OrderWarehouseItem01]
    ON [dbo].[OrderWarehouseItem]([OrderId] ASC);


GO
PRINT N'Creating Index [dbo].[OrderWarehouseItem].[IX_OrderWarehouseItem02]...';


GO
CREATE NONCLUSTERED INDEX [IX_OrderWarehouseItem02]
    ON [dbo].[OrderWarehouseItem]([WarehouseItemId] ASC);


GO
PRINT N'Creating Table [dbo].[OrderWarehouseItemOutOfStock]...';


GO
CREATE TABLE [dbo].[OrderWarehouseItemOutOfStock] (
    [OrderId]         INT          NOT NULL,
    [WarehouseItemId] INT          NOT NULL,
    [CreateDate]      DATETIME     NOT NULL,
    [CreateUser]      VARCHAR (32) NOT NULL,
    [ModifyDate]      DATETIME     NULL,
    [ModifyUser]      VARCHAR (32) NULL,
    CONSTRAINT [PK_OrderWarehouseItemOutOfStock] PRIMARY KEY CLUSTERED ([OrderId] ASC, [WarehouseItemId] ASC)
);


GO
PRINT N'Creating Index [dbo].[OrderWarehouseItemOutOfStock].[IX_OrderWarehouseItemOutOfStock01]...';


GO
CREATE NONCLUSTERED INDEX [IX_OrderWarehouseItemOutOfStock01]
    ON [dbo].[OrderWarehouseItemOutOfStock]([OrderId] ASC);


GO
PRINT N'Creating Index [dbo].[OrderWarehouseItemOutOfStock].[IX_OrderWarehouseItemOutOfStock02]...';


GO
CREATE NONCLUSTERED INDEX [IX_OrderWarehouseItemOutOfStock02]
    ON [dbo].[OrderWarehouseItemOutOfStock]([WarehouseItemId] ASC);


GO
PRINT N'Creating Table [dbo].[Position]...';


GO
CREATE TABLE [dbo].[Position] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME     NOT NULL,
    [CreateUser] VARCHAR (32) NOT NULL,
    [ModifyDate] DATETIME     NULL,
    [ModifyUser] VARCHAR (32) NULL,
    CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[PurchaseOrder]...';


GO
CREATE TABLE [dbo].[PurchaseOrder] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME     NOT NULL,
    [CreateUser] VARCHAR (32) NOT NULL,
    [ModifyDate] DATETIME     NULL,
    [ModifyUser] VARCHAR (32) NULL,
    CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[PurchaseOrderWarehouseItem]...';


GO
CREATE TABLE [dbo].[PurchaseOrderWarehouseItem] (
    [PurchaseOrderId] INT NOT NULL,
    [WarehouseItemId] INT NOT NULL,
    CONSTRAINT [PK_PurchaseOrderWarehouseItem] PRIMARY KEY CLUSTERED ([PurchaseOrderId] ASC, [WarehouseItemId] ASC)
);


GO
PRINT N'Creating Index [dbo].[PurchaseOrderWarehouseItem].[IX_PurchaseOrderWarehouseItem01]...';


GO
CREATE NONCLUSTERED INDEX [IX_PurchaseOrderWarehouseItem01]
    ON [dbo].[PurchaseOrderWarehouseItem]([PurchaseOrderId] ASC);


GO
PRINT N'Creating Index [dbo].[PurchaseOrderWarehouseItem].[IX_PurchaseOrderWarehouseItem02]...';


GO
CREATE NONCLUSTERED INDEX [IX_PurchaseOrderWarehouseItem02]
    ON [dbo].[PurchaseOrderWarehouseItem]([WarehouseItemId] ASC);


GO
PRINT N'Creating Table [dbo].[Vendor]...';


GO
CREATE TABLE [dbo].[Vendor] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME     NOT NULL,
    [CreateUser] VARCHAR (32) NOT NULL,
    [ModifyDate] DATETIME     NULL,
    [ModifyUser] VARCHAR (32) NULL,
    CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[VendorItem]...';


GO
CREATE TABLE [dbo].[VendorItem] (
    [VendorId] INT NOT NULL,
    [ItemId]   INT NOT NULL,
    CONSTRAINT [PK_VendorItem] PRIMARY KEY CLUSTERED ([VendorId] ASC, [ItemId] ASC)
);


GO
PRINT N'Creating Index [dbo].[VendorItem].[IX_VendorItem01]...';


GO
CREATE NONCLUSTERED INDEX [IX_VendorItem01]
    ON [dbo].[VendorItem]([VendorId] ASC);


GO
PRINT N'Creating Index [dbo].[VendorItem].[IX_VendorItem02]...';


GO
CREATE NONCLUSTERED INDEX [IX_VendorItem02]
    ON [dbo].[VendorItem]([ItemId] ASC);


GO
PRINT N'Creating Table [dbo].[Warehouse]...';


GO
CREATE TABLE [dbo].[Warehouse] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (32) NOT NULL,
    [CreateDate] DATETIME     NOT NULL,
    [CreateUser] VARCHAR (32) NOT NULL,
    [ModifyDate] DATETIME     NULL,
    [ModifyUser] VARCHAR (32) NULL,
    CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Table [dbo].[WarehouseItem]...';


GO
CREATE TABLE [dbo].[WarehouseItem] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [WarehouseId] INT          NOT NULL,
    [ItemId]      INT          NOT NULL,
    [Quantity]    INT          NOT NULL,
    [CreateDate]  DATETIME     NOT NULL,
    [CreateUser]  VARCHAR (32) NOT NULL,
    [ModifyDate]  DATETIME     NULL,
    [ModifyUser]  VARCHAR (32) NULL,
    CONSTRAINT [PK_WarehouseItem] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating Index [dbo].[WarehouseItem].[IX_WarehouseItem01]...';


GO
CREATE NONCLUSTERED INDEX [IX_WarehouseItem01]
    ON [dbo].[WarehouseItem]([WarehouseId] ASC);


GO
PRINT N'Creating Index [dbo].[WarehouseItem].[IX_WarehouseItem02]...';


GO
CREATE NONCLUSTERED INDEX [IX_WarehouseItem02]
    ON [dbo].[WarehouseItem]([ItemId] ASC);


GO
PRINT N'Creating Default Constraint [dbo].[DF_Address_CreateDate]...';


GO
ALTER TABLE [dbo].[Address]
    ADD CONSTRAINT [DF_Address_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_Company_CreateDate]...';


GO
ALTER TABLE [dbo].[Company]
    ADD CONSTRAINT [DF_Company_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_Customer_CreateDate]...';


GO
ALTER TABLE [dbo].[Customer]
    ADD CONSTRAINT [DF_Customer_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_Item_CreateDate]...';


GO
ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [DF_Item_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_Order_CreateDate]...';


GO
ALTER TABLE [dbo].[Order]
    ADD CONSTRAINT [DF_Order_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_OrderWarehouseItem_CreateDate]...';


GO
ALTER TABLE [dbo].[OrderWarehouseItem]
    ADD CONSTRAINT [DF_OrderWarehouseItem_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_OrderWarehouseItemOutOfStock_CreateDate]...';


GO
ALTER TABLE [dbo].[OrderWarehouseItemOutOfStock]
    ADD CONSTRAINT [DF_OrderWarehouseItemOutOfStock_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_Position_CreateDate]...';


GO
ALTER TABLE [dbo].[Position]
    ADD CONSTRAINT [DF_Position_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_PurchaseOrder_CreateDate]...';


GO
ALTER TABLE [dbo].[PurchaseOrder]
    ADD CONSTRAINT [DF_PurchaseOrder_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_Vendor_CreateDate]...';


GO
ALTER TABLE [dbo].[Vendor]
    ADD CONSTRAINT [DF_Vendor_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_Warehouse_CreateDate]...';


GO
ALTER TABLE [dbo].[Warehouse]
    ADD CONSTRAINT [DF_Warehouse_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Default Constraint [dbo].[DF_WarehouseItem_CreateDate]...';


GO
ALTER TABLE [dbo].[WarehouseItem]
    ADD CONSTRAINT [DF_WarehouseItem_CreateDate] DEFAULT GETUTCDATE() FOR [CreateDate];


GO
PRINT N'Creating Foreign Key [dbo].[FK_Employee_Position]...';


GO
ALTER TABLE [dbo].[Employee] WITH NOCHECK
    ADD CONSTRAINT [FK_Employee_Position] FOREIGN KEY ([PositionId]) REFERENCES [dbo].[Position] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_Employee_SupervisorEmployee]...';


GO
ALTER TABLE [dbo].[Employee] WITH NOCHECK
    ADD CONSTRAINT [FK_Employee_SupervisorEmployee] FOREIGN KEY ([SupervisorEmployeeId]) REFERENCES [dbo].[Employee] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_Order_Customer]...';


GO
ALTER TABLE [dbo].[Order] WITH NOCHECK
    ADD CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_OrderWarehouseItem_Order]...';


GO
ALTER TABLE [dbo].[OrderWarehouseItem] WITH NOCHECK
    ADD CONSTRAINT [FK_OrderWarehouseItem_Order] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_OrderWarehouseItem_Warehouse]...';


GO
ALTER TABLE [dbo].[OrderWarehouseItem] WITH NOCHECK
    ADD CONSTRAINT [FK_OrderWarehouseItem_Warehouse] FOREIGN KEY ([WarehouseItemId]) REFERENCES [dbo].[WarehouseItem] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_OrderWarehouseItemOutOfStock_Order]...';


GO
ALTER TABLE [dbo].[OrderWarehouseItemOutOfStock] WITH NOCHECK
    ADD CONSTRAINT [FK_OrderWarehouseItemOutOfStock_Order] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_OrderWarehouseItemOutOfStock_WarehouseItemOutOfStock]...';


GO
ALTER TABLE [dbo].[OrderWarehouseItemOutOfStock] WITH NOCHECK
    ADD CONSTRAINT [FK_OrderWarehouseItemOutOfStock_WarehouseItemOutOfStock] FOREIGN KEY ([WarehouseItemId]) REFERENCES [dbo].[WarehouseItem] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_PurchaseOrderWarehouseItem_PurchaseOrder]...';


GO
ALTER TABLE [dbo].[PurchaseOrderWarehouseItem] WITH NOCHECK
    ADD CONSTRAINT [FK_PurchaseOrderWarehouseItem_PurchaseOrder] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [dbo].[PurchaseOrder] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_PurchaseOrderWarehouseItem_WarehouseItem]...';


GO
ALTER TABLE [dbo].[PurchaseOrderWarehouseItem] WITH NOCHECK
    ADD CONSTRAINT [FK_PurchaseOrderWarehouseItem_WarehouseItem] FOREIGN KEY ([WarehouseItemId]) REFERENCES [dbo].[WarehouseItem] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_VendorItem_Vendor]...';


GO
ALTER TABLE [dbo].[VendorItem] WITH NOCHECK
    ADD CONSTRAINT [FK_VendorItem_Vendor] FOREIGN KEY ([VendorId]) REFERENCES [dbo].[Vendor] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_VendorItem_Item]...';


GO
ALTER TABLE [dbo].[VendorItem] WITH NOCHECK
    ADD CONSTRAINT [FK_VendorItem_Item] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Item] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_WarehouseItem_Warehouse]...';


GO
ALTER TABLE [dbo].[WarehouseItem] WITH NOCHECK
    ADD CONSTRAINT [FK_WarehouseItem_Warehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[Warehouse] ([Id]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_WarehouseItem_Item]...';


GO
ALTER TABLE [dbo].[WarehouseItem] WITH NOCHECK
    ADD CONSTRAINT [FK_WarehouseItem_Item] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Item] ([Id]);


GO
PRINT N'Creating Check Constraint [dbo].[CHK_WarehouseItem_QuantityIsNotNegative]...';


GO
ALTER TABLE [dbo].[WarehouseItem] WITH NOCHECK
    ADD CONSTRAINT [CHK_WarehouseItem_QuantityIsNotNegative] CHECK ([Quantity] >= 0);


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Employee] WITH CHECK CHECK CONSTRAINT [FK_Employee_Position];

ALTER TABLE [dbo].[Employee] WITH CHECK CHECK CONSTRAINT [FK_Employee_SupervisorEmployee];

ALTER TABLE [dbo].[Order] WITH CHECK CHECK CONSTRAINT [FK_Order_Customer];

ALTER TABLE [dbo].[OrderWarehouseItem] WITH CHECK CHECK CONSTRAINT [FK_OrderWarehouseItem_Order];

ALTER TABLE [dbo].[OrderWarehouseItem] WITH CHECK CHECK CONSTRAINT [FK_OrderWarehouseItem_Warehouse];

ALTER TABLE [dbo].[OrderWarehouseItemOutOfStock] WITH CHECK CHECK CONSTRAINT [FK_OrderWarehouseItemOutOfStock_Order];

ALTER TABLE [dbo].[OrderWarehouseItemOutOfStock] WITH CHECK CHECK CONSTRAINT [FK_OrderWarehouseItemOutOfStock_WarehouseItemOutOfStock];

ALTER TABLE [dbo].[PurchaseOrderWarehouseItem] WITH CHECK CHECK CONSTRAINT [FK_PurchaseOrderWarehouseItem_PurchaseOrder];

ALTER TABLE [dbo].[PurchaseOrderWarehouseItem] WITH CHECK CHECK CONSTRAINT [FK_PurchaseOrderWarehouseItem_WarehouseItem];

ALTER TABLE [dbo].[VendorItem] WITH CHECK CHECK CONSTRAINT [FK_VendorItem_Vendor];

ALTER TABLE [dbo].[VendorItem] WITH CHECK CHECK CONSTRAINT [FK_VendorItem_Item];

ALTER TABLE [dbo].[WarehouseItem] WITH CHECK CHECK CONSTRAINT [FK_WarehouseItem_Warehouse];

ALTER TABLE [dbo].[WarehouseItem] WITH CHECK CHECK CONSTRAINT [FK_WarehouseItem_Item];

ALTER TABLE [dbo].[WarehouseItem] WITH CHECK CHECK CONSTRAINT [CHK_WarehouseItem_QuantityIsNotNegative];


GO
PRINT N'Update complete.';


GO
