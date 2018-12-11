/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 01/08/2014 16:21:22

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping foreign keys from [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [FK_ListadoADV_Formulario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_Formulario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Notificacion]'
GO
ALTER TABLE [dbo].[Notificacion] DROP CONSTRAINT [FK_Notificacion_Formulario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Propuesta_Linea]'
GO
ALTER TABLE [dbo].[Propuesta_Linea] ADD
[ID_Propuesta_Opcion] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Propuesta_Opcion]'
GO
CREATE TABLE [dbo].[Propuesta_Opcion]
(
[ID_Propuesta_Opcion] [int] NOT NULL IDENTITY(1, 1),
[ID_Propuesta] [int] NOT NULL,
[ID_Propuesta_Opcion_Accion] [int] NULL,
[Nombre] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NULL,
[Importe] [decimal] (12, 4) NULL,
[ImportePropuesta] [decimal] (12, 4) NULL,
[Requerido] [bit] NOT NULL CONSTRAINT [DF_Propuesta_Opcion_Requerido_1] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Propuesta_Opcion] on [dbo].[Propuesta_Opcion]'
GO
ALTER TABLE [dbo].[Propuesta_Opcion] ADD CONSTRAINT [PK_Propuesta_Opcion] PRIMARY KEY CLUSTERED  ([ID_Propuesta_Opcion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[FinanciacionMeses]'
GO
CREATE TABLE [dbo].[FinanciacionMeses]
(
[ID_FinanciacionMeses] [int] NOT NULL IDENTITY(1, 1),
[Descripcion] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Meses] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_FinanciacionMeses] on [dbo].[FinanciacionMeses]'
GO
ALTER TABLE [dbo].[FinanciacionMeses] ADD CONSTRAINT [PK_FinanciacionMeses] PRIMARY KEY CLUSTERED  ([ID_FinanciacionMeses])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Propuesta_Financiacion]'
GO
CREATE TABLE [dbo].[Propuesta_Financiacion]
(
[ID_Propuesta_Financiacion] [int] NOT NULL IDENTITY(1, 1),
[ID_Propuesta] [int] NOT NULL,
[ID_FinanciacionMeses] [int] NOT NULL,
[Descripcion] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Importe] [decimal] (12, 4) NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Propuesta_FinanciacionMeses] on [dbo].[Propuesta_Financiacion]'
GO
ALTER TABLE [dbo].[Propuesta_Financiacion] ADD CONSTRAINT [PK_Propuesta_FinanciacionMeses] PRIMARY KEY CLUSTERED  ([ID_Propuesta_Financiacion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Propuesta_Opcion_Accion]'
GO
CREATE TABLE [dbo].[Propuesta_Opcion_Accion]
(
[ID_Propuesta_Opcion_Accion] [int] NOT NULL IDENTITY(1, 1),
[Descripcion] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Propuesta_Opcion_Accion] on [dbo].[Propuesta_Opcion_Accion]'
GO
ALTER TABLE [dbo].[Propuesta_Opcion_Accion] ADD CONSTRAINT [PK_Propuesta_Opcion_Accion] PRIMARY KEY CLUSTERED  ([ID_Propuesta_Opcion_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_Financiacion]'
GO
ALTER TABLE [dbo].[Propuesta_Financiacion] ADD CONSTRAINT [FK_Propuesta_FinanciacionMeses_FinanciacionMeses] FOREIGN KEY ([ID_FinanciacionMeses]) REFERENCES [dbo].[FinanciacionMeses] ([ID_FinanciacionMeses])
ALTER TABLE [dbo].[Propuesta_Financiacion] ADD CONSTRAINT [FK_Propuesta_FinanciacionMeses_Propuesta] FOREIGN KEY ([ID_Propuesta]) REFERENCES [dbo].[Propuesta] ([ID_Propuesta])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] ADD CONSTRAINT [FK_ListadoADV_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] ADD CONSTRAINT [FK_Menu_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Notificacion]'
GO
ALTER TABLE [dbo].[Notificacion] ADD CONSTRAINT [FK_Notificacion_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_Linea]'
GO
ALTER TABLE [dbo].[Propuesta_Linea] ADD CONSTRAINT [FK_Propuesta_Linea_Propuesta_Opcion] FOREIGN KEY ([ID_Propuesta_Opcion]) REFERENCES [dbo].[Propuesta_Opcion] ([ID_Propuesta_Opcion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_Opcion]'
GO
ALTER TABLE [dbo].[Propuesta_Opcion] ADD CONSTRAINT [FK_Propuesta_Opcion_Propuesta] FOREIGN KEY ([ID_Propuesta]) REFERENCES [dbo].[Propuesta] ([ID_Propuesta])
ALTER TABLE [dbo].[Propuesta_Opcion] ADD CONSTRAINT [FK_Propuesta_Opcion_Propuesta_Opcion_Accion] FOREIGN KEY ([ID_Propuesta_Opcion_Accion]) REFERENCES [dbo].[Propuesta_Opcion_Accion] ([ID_Propuesta_Opcion_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO