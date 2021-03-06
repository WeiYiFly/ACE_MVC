Create database ACE_MVC
delete from SYS_UserSYS_Program
select * from SYS_UserSYS_Program
create table SYS_UserSYS_Program
(
Id int primary key identity(1,1),
SYS_UserID varchar(30),
SYS_ProgramID int
)
drop table SYS_Module
drop table
CREATE TABLE SYS_Program(
	Id int IDENTITY(1,1) NOT NULL,
	ChinaName varchar(30) NULL,
	EnglishName varchar(30) NULL,
	CreatorID varchar(30) NULL,
	CreatedTime datetime NULL,
	Del char(1) NULL)
	
	CREATE TABLE [dbo].[SYS_Module](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](30) NULL,
	[Lv] [char](1) NULL,
	[Controller] [varchar](30) NULL,
	[View_] [varchar](30) NULL,
	[Url] [varchar](50) NULL,
	[Icon] [varchar](30) NULL,
	[UpId] [int] NULL,
	[ProgramId] [int] NULL,
	[SYS_ProgramId] [int] NOT NULL)
	
	
create table SYS_Program
(
Id int primary key identity(1,1),
Name varchar(30),
CreatorID varchar(30),
CreatedTime datetime,
Del char(1)
)
delete from SYS_Program
where Id<>5080
select * from SYS_Program
drop table SYS_User
create table SYS_User
(
Id varchar(30) primary key ,
Name varchar(30),
Bu varchar(30),
Email varchar(40),
Tel varchar(20),
Passw varchar(20),
Del char(1)
)

create table SYS_Role
(
Id int primary key identity(1,1), 
Name varchar(30), --名字
ProgramId int ,  --所屬系統,0 為共用角色
Remark varchar(100),--備註
CreatorID varchar(30),
CreatedTime datetime,
Del char(1)
)
drop table SYS_RoleSYS_Module

select * from SYS_Module
create table SYS_Module
(
Id int primary key identity(1,1),--
Name varchar(30),--系統名字
Lv   char(1),--目錄等級 1 模塊 2 菜單 3 子菜單
Controller varchar(30),
View_ varchar(30),
Url   varchar(50),--絕對路徑
Icon  varchar(30),
UpId  int,--上級菜單 0，代表無
ProgramId int--所屬系統
)
delete from SYS_Module
where Id=5