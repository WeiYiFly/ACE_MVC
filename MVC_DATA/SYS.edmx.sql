
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/27/2018 13:46:07
-- Generated from EDMX file: F:\ACE_MVC\MVC_DATA\SYS.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ACE_MVC];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SYS_UserSYS_Program_SYS_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SYS_UserSYS_Program] DROP CONSTRAINT [FK_SYS_UserSYS_Program_SYS_User];
GO
IF OBJECT_ID(N'[dbo].[FK_SYS_UserSYS_Program_SYS_Program]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SYS_UserSYS_Program] DROP CONSTRAINT [FK_SYS_UserSYS_Program_SYS_Program];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SYS_Program]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SYS_Program];
GO
IF OBJECT_ID(N'[dbo].[SYS_Module]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SYS_Module];
GO
IF OBJECT_ID(N'[dbo].[SYS_Role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SYS_Role];
GO
IF OBJECT_ID(N'[dbo].[SYS_User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SYS_User];
GO
IF OBJECT_ID(N'[dbo].[SYS_UserSYS_Program]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SYS_UserSYS_Program];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SYS_Program'
CREATE TABLE [dbo].[SYS_Program] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ChinaName] varchar(30)  NULL,
    [EnglishName] varchar(30)  NULL,
    [CreatorID] varchar(30)  NULL,
    [CreatedTime] datetime  NULL,
    [Del] char(1)  NULL
);
GO

-- Creating table 'SYS_Module'
CREATE TABLE [dbo].[SYS_Module] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(30)  NULL,
    [Lv] char(1)  NULL,
    [Controller] varchar(30)  NULL,
    [View_] varchar(30)  NULL,
    [Url] varchar(50)  NULL,
    [Icon] varchar(30)  NULL,
    [UpId] int  NULL,
    [ProgramId] int  NULL,
    [SYS_ProgramId] int  NOT NULL
);
GO

-- Creating table 'SYS_Role'
CREATE TABLE [dbo].[SYS_Role] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(30)  NULL,
    [ProgramId] int  NULL,
    [Remark] varchar(100)  NULL,
    [CreatorID] varchar(30)  NULL,
    [CreatedTime] datetime  NULL,
    [Del] char(1)  NULL
);
GO

-- Creating table 'SYS_User'
CREATE TABLE [dbo].[SYS_User] (
    [Id] varchar(30)  NOT NULL,
    [Name] varchar(30)  NULL,
    [Bu] varchar(30)  NULL,
    [Email] varchar(40)  NULL,
    [Tel] varchar(20)  NULL,
    [Passw] varchar(20)  NULL,
    [Del] char(1)  NULL
);
GO

-- Creating table 'SYS_UserSYS_Program'
CREATE TABLE [dbo].[SYS_UserSYS_Program] (
    [SYS_User_Id] varchar(30)  NOT NULL,
    [SYS_Program_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'SYS_Program'
ALTER TABLE [dbo].[SYS_Program]
ADD CONSTRAINT [PK_SYS_Program]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SYS_Module'
ALTER TABLE [dbo].[SYS_Module]
ADD CONSTRAINT [PK_SYS_Module]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SYS_Role'
ALTER TABLE [dbo].[SYS_Role]
ADD CONSTRAINT [PK_SYS_Role]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SYS_User'
ALTER TABLE [dbo].[SYS_User]
ADD CONSTRAINT [PK_SYS_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [SYS_User_Id], [SYS_Program_Id] in table 'SYS_UserSYS_Program'
ALTER TABLE [dbo].[SYS_UserSYS_Program]
ADD CONSTRAINT [PK_SYS_UserSYS_Program]
    PRIMARY KEY CLUSTERED ([SYS_User_Id], [SYS_Program_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SYS_User_Id] in table 'SYS_UserSYS_Program'
ALTER TABLE [dbo].[SYS_UserSYS_Program]
ADD CONSTRAINT [FK_SYS_UserSYS_Program_SYS_User]
    FOREIGN KEY ([SYS_User_Id])
    REFERENCES [dbo].[SYS_User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SYS_Program_Id] in table 'SYS_UserSYS_Program'
ALTER TABLE [dbo].[SYS_UserSYS_Program]
ADD CONSTRAINT [FK_SYS_UserSYS_Program_SYS_Program]
    FOREIGN KEY ([SYS_Program_Id])
    REFERENCES [dbo].[SYS_Program]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SYS_UserSYS_Program_SYS_Program'
CREATE INDEX [IX_FK_SYS_UserSYS_Program_SYS_Program]
ON [dbo].[SYS_UserSYS_Program]
    ([SYS_Program_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------