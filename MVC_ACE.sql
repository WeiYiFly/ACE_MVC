Create database ACE_MVC

create table SYS_Program
(
Id int primary key identity(1,1),
Name varchar(30),
CreatorID varchar(30),
CreatedTime datetime,
Del char(1)
)
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
Id int primary key identity(1,1) ,
Name varchar(30),
CreatorID varchar(30),
CreatedTime datetime,
Del char(1)
)

create table SYS_Module
(
Id int primary key identity(1,1),
Name varchar(30),
Lv   char(1),--¥Ø¿ýµ¥¯Å
Controller varchar(30),
View_ varchar(30),
Url   varchar(50),
Icon  varchar(30),
UpId  int,
ProgramId int
)
