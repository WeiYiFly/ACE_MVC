Create database ACE_MVC

create table SYS_UserSYS_Program
(
Id int primary key identity(1,1),
SYS_UserID varchar(30),
SYS_ProgramID int,

)

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
Name varchar(30), --�W�r
ProgramId int ,  --���ݨt��,0 ���@�Ψ���
Remark varchar(100),--�Ƶ�
CreatorID varchar(30),
CreatedTime datetime,
Del char(1)
)
drop table SYS_RoleSYS_Module

select * from SYS_Module
create table SYS_Module
(
Id int primary key identity(1,1),--
Name varchar(30),--�t�ΦW�r
Lv   char(1),--�ؿ����� 1 �Ҷ� 2 ��� 3 �l���
Controller varchar(30),
View_ varchar(30),
Url   varchar(50),--������|
Icon  varchar(30),
UpId  int,--�W�ŵ�� 0�A�N��L
ProgramId int--���ݨt��
)
delete from SYS_Module
where Id=5