/*
Run this script on:

(local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

(local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Data Compare version 10.4.8 from Red Gate Software Ltd at 01/08/2014 16:27:46

*/
		
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)

PRINT(N'Drop constraint FK_Propuesta_Opcion_Propuesta_Opcion_Accion from [dbo].[Propuesta_Opcion]')
GO
ALTER TABLE [dbo].[Propuesta_Opcion] DROP CONSTRAINT [FK_Propuesta_Opcion_Propuesta_Opcion_Accion]

PRINT(N'Drop constraint FK_ListadoADV_Formulario from [dbo].[ListadoADV]')
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [FK_ListadoADV_Formulario]

PRINT(N'Drop constraint FK_Menu_Formulario from [dbo].[Menus]')
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_Formulario]

PRINT(N'Drop constraint FK_Notificacion_Formulario from [dbo].[Notificacion]')
GO
ALTER TABLE [dbo].[Notificacion] DROP CONSTRAINT [FK_Notificacion_Formulario]

PRINT(N'Drop constraint FK_Propuesta_FinanciacionMeses_FinanciacionMeses from [dbo].[Propuesta_Financiacion]')
GO
ALTER TABLE [dbo].[Propuesta_Financiacion] DROP CONSTRAINT [FK_Propuesta_FinanciacionMeses_FinanciacionMeses]

PRINT(N'Add rows to [dbo].[FinanciacionMeses]')
GO
SET IDENTITY_INSERT [dbo].[FinanciacionMeses] ON
INSERT INTO [dbo].[FinanciacionMeses] ([ID_FinanciacionMeses], [Descripcion], [Meses]) VALUES (1, N'1 Año', 12)
INSERT INTO [dbo].[FinanciacionMeses] ([ID_FinanciacionMeses], [Descripcion], [Meses]) VALUES (2, N'2 Años', 24)
INSERT INTO [dbo].[FinanciacionMeses] ([ID_FinanciacionMeses], [Descripcion], [Meses]) VALUES (3, N'3 Años', 36)
SET IDENTITY_INSERT [dbo].[FinanciacionMeses] OFF
PRINT(N'Operation applied to 3 rows out of 3')
GO

PRINT(N'Add row to [dbo].[Formulario]')
GO
INSERT INTO [dbo].[Formulario] ([ID_Formulario], [NombreReal], [Descripcion], [ParametroEntrada], [AperturaExterna]) VALUES (207, N'frmManteniment', N'Mantenimiento de meses de financiación', 126, 1)

PRINT(N'Add row to [dbo].[Maestro]')
GO
INSERT INTO [dbo].[Maestro] ([ID_Maestro], [Tabla], [Descripcion], [Interna]) VALUES (126, N'FinanciacionMeses', N'Mantenimiento de meses de financiación', 0)

PRINT(N'Add rows to [dbo].[Propuesta_Opcion_Accion]')
GO
SET IDENTITY_INSERT [dbo].[Propuesta_Opcion_Accion] ON
INSERT INTO [dbo].[Propuesta_Opcion_Accion] ([ID_Propuesta_Opcion_Accion], [Descripcion]) VALUES (1, N'Incrementar')
INSERT INTO [dbo].[Propuesta_Opcion_Accion] ([ID_Propuesta_Opcion_Accion], [Descripcion]) VALUES (2, N'Disminuir')
SET IDENTITY_INSERT [dbo].[Propuesta_Opcion_Accion] OFF
PRINT(N'Operation applied to 2 rows out of 2')
GO

PRINT(N'Add constraint FK_Propuesta_Opcion_Propuesta_Opcion_Accion to [dbo].[Propuesta_Opcion]')
GO
ALTER TABLE [dbo].[Propuesta_Opcion] WITH NOCHECK ADD CONSTRAINT [FK_Propuesta_Opcion_Propuesta_Opcion_Accion] FOREIGN KEY ([ID_Propuesta_Opcion_Accion]) REFERENCES [dbo].[Propuesta_Opcion_Accion] ([ID_Propuesta_Opcion_Accion])

PRINT(N'Add constraint FK_ListadoADV_Formulario to [dbo].[ListadoADV]')
GO
ALTER TABLE [dbo].[ListadoADV] WITH NOCHECK ADD CONSTRAINT [FK_ListadoADV_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])

PRINT(N'Add constraint FK_Menu_Formulario to [dbo].[Menus]')
GO
ALTER TABLE [dbo].[Menus] WITH NOCHECK ADD CONSTRAINT [FK_Menu_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])

PRINT(N'Add constraint FK_Notificacion_Formulario to [dbo].[Notificacion]')
GO
ALTER TABLE [dbo].[Notificacion] WITH NOCHECK ADD CONSTRAINT [FK_Notificacion_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])

PRINT(N'Add constraint FK_Propuesta_FinanciacionMeses_FinanciacionMeses to [dbo].[Propuesta_Financiacion]')
GO
ALTER TABLE [dbo].[Propuesta_Financiacion] WITH NOCHECK ADD CONSTRAINT [FK_Propuesta_FinanciacionMeses_FinanciacionMeses] FOREIGN KEY ([ID_FinanciacionMeses]) REFERENCES [dbo].[FinanciacionMeses] ([ID_FinanciacionMeses])
COMMIT TRANSACTION
GO
