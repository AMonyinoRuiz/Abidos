/*
Run this script on:

        192.168.1.225.AbidosMestre    -  This database will be modified

to synchronize it with:

        192.168.1.225.AbidosDomingo

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.0 from Red Gate Software Ltd at 10/02/2016 2:10:08

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[Cliente_Contacto]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Cliente_Contacto] ADD
[ID_Idioma_Escrito] [int] NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Refreshing [dbo].[C_Campaña_Exportacion]'
GO
EXEC sp_refreshview N'[dbo].[C_Campaña_Exportacion]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Refreshing [dbo].[C_Cliente_Contacto]'
GO
EXEC sp_refreshview N'[dbo].[C_Cliente_Contacto]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Cliente_Direccion]'
GO
CREATE TABLE [dbo].[Cliente_Direccion]
(
[ID_Cliente_Direccion] [int] NOT NULL IDENTITY(1, 1),
[ID_Cliente] [int] NOT NULL,
[ID_Cliente_DireccionTipo] [int] NOT NULL,
[Direccion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Poblacion] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[Provincia] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[CP] [nvarchar] (20) COLLATE Modern_Spanish_CI_AS NULL,
[Observaciones] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Cliente_Direccion] on [dbo].[Cliente_Direccion]'
GO
ALTER TABLE [dbo].[Cliente_Direccion] ADD CONSTRAINT [PK_Cliente_Direccion] PRIMARY KEY CLUSTERED  ([ID_Cliente_Direccion])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Cliente_DireccionTipo]'
GO
CREATE TABLE [dbo].[Cliente_DireccionTipo]
(
[ID_Cliente_DireccionTipo] [int] NOT NULL IDENTITY(1, 1),
[Descripcion] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Cliente_DireccionTipo] on [dbo].[Cliente_DireccionTipo]'
GO
ALTER TABLE [dbo].[Cliente_DireccionTipo] ADD CONSTRAINT [PK_Cliente_DireccionTipo] PRIMARY KEY CLUSTERED  ([ID_Cliente_DireccionTipo])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Cliente_ProductosInteres]'
GO
CREATE TABLE [dbo].[Cliente_ProductosInteres]
(
[ID_Cliente_ProductosInteres] [int] NOT NULL IDENTITY(1, 1),
[ID_Cliente] [int] NOT NULL,
[ID_Producto_Division] [int] NOT NULL,
[ID_Producto_Familia] [int] NULL,
[FechaAlta] [smalldatetime] NOT NULL,
[Observacions] [nvarchar] (500) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Cliente_ProductosInteres] on [dbo].[Cliente_ProductosInteres]'
GO
ALTER TABLE [dbo].[Cliente_ProductosInteres] ADD CONSTRAINT [PK_Cliente_ProductosInteres] PRIMARY KEY CLUSTERED  ([ID_Cliente_ProductosInteres])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Cliente_Contacto]'
GO
ALTER TABLE [dbo].[Cliente_Contacto] ADD CONSTRAINT [FK_Cliente_Contacto_Idioma1] FOREIGN KEY ([ID_Idioma_Escrito]) REFERENCES [dbo].[Idioma] ([ID_Idioma])
ALTER TABLE [dbo].[Cliente_Contacto] ADD CONSTRAINT [FK_Cliente_Contacto_Idioma] FOREIGN KEY ([ID_Idioma]) REFERENCES [dbo].[Idioma] ([ID_Idioma])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Cliente_Direccion]'
GO
ALTER TABLE [dbo].[Cliente_Direccion] ADD CONSTRAINT [FK_Cliente_Direccion_Cliente_DireccionTipo] FOREIGN KEY ([ID_Cliente_DireccionTipo]) REFERENCES [dbo].[Cliente_DireccionTipo] ([ID_Cliente_DireccionTipo])
ALTER TABLE [dbo].[Cliente_Direccion] ADD CONSTRAINT [FK_Cliente_Direccion_Cliente] FOREIGN KEY ([ID_Cliente]) REFERENCES [dbo].[Cliente] ([ID_Cliente])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Cliente_ProductosInteres]'
GO
ALTER TABLE [dbo].[Cliente_ProductosInteres] ADD CONSTRAINT [FK_Cliente_ProductosInteres_Cliente] FOREIGN KEY ([ID_Cliente]) REFERENCES [dbo].[Cliente] ([ID_Cliente])
ALTER TABLE [dbo].[Cliente_ProductosInteres] ADD CONSTRAINT [FK_Cliente_ProductosInteres_Producto_Division] FOREIGN KEY ([ID_Producto_Division]) REFERENCES [dbo].[Producto_Division] ([ID_Producto_Division])
ALTER TABLE [dbo].[Cliente_ProductosInteres] ADD CONSTRAINT [FK_Cliente_ProductosInteres_Producto_Familia] FOREIGN KEY ([ID_Producto_Familia]) REFERENCES [dbo].[Producto_Familia] ([ID_Producto_Familia])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'The database update failed'
END
GO