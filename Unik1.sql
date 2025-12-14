CREATE TABLE [dbo].[Cicle] (
[Cicle] nvarchar(255),
[Id_cicle] int NOT NULL
)
CREATE TABLE [dbo].[Predmet] (
[PredmetName] nvarchar(255),
[Id_cycle] int NOT NULL,
[Id_predmet] int NOT NULL
)
CREATE TABLE [dbo].[Category] (
[CategoryName] nvarchar(255),
[Salary] int,
[Id_category] int NOT NULL
)
CREATE TABLE [dbo].[Load] (
[Id_predmet] int NOT NULL,
[Clocks] int NOT NULL,
[Id_teacher] int NOT NULL,
[Id_load] int NOT NULL
)
CREATE TABLE [dbo].[Teachers] (
[Fio] [nvarchar](255) NOT NULL,
[Id_category] [int] NOT NULL,
[Street] [nvarchar](255) NULL,
[House] [nvarchar](255) NULL,
[Apartment] [nvarchar](255) NULL,
[Id_teacher] [int] NOT NULL,
[Id_user] [int] NULL
)
CREATE TABLE [dbo].[Users](
	[Id_user] [int] NOT NULL,
	[Login] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Id_role] [int] NOT NULL
)
CREATE TABLE [dbo].[Role](
	[RoleName] [nvarchar](255) NOT NULL,
	[Id_role] [int] NOT NULL
)