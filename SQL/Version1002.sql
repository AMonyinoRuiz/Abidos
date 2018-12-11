/*
Run this script on:

(local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

(local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Data Compare version 10.4.8 from Red Gate Software Ltd at 12/02/2015 16:57:41

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

PRINT(N'Drop constraint FK_ListadoADV_Formulario from [dbo].[ListadoADV]')
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [FK_ListadoADV_Formulario]

PRINT(N'Drop constraint FK_Menu_Formulario from [dbo].[Menus]')
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_Formulario]

PRINT(N'Drop constraint FK_Notificacion_Formulario from [dbo].[Notificacion]')
GO
ALTER TABLE [dbo].[Notificacion] DROP CONSTRAINT [FK_Notificacion_Formulario]

PRINT(N'Add row to [dbo].[Formulario]')
GO
INSERT INTO [dbo].[Formulario] ([ID_Formulario], [NombreReal], [Descripcion], [ParametroEntrada], [AperturaExterna]) VALUES (210, N'frmBono', N'Bonos', NULL, 1)

PRINT(N'Add constraint FK_ListadoADV_Formulario to [dbo].[ListadoADV]')
GO
ALTER TABLE [dbo].[ListadoADV] WITH NOCHECK ADD CONSTRAINT [FK_ListadoADV_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])

PRINT(N'Add constraint FK_Menu_Formulario to [dbo].[Menus]')
GO
ALTER TABLE [dbo].[Menus] WITH NOCHECK ADD CONSTRAINT [FK_Menu_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])

PRINT(N'Add constraint FK_Notificacion_Formulario to [dbo].[Notificacion]')
GO
ALTER TABLE [dbo].[Notificacion] WITH NOCHECK ADD CONSTRAINT [FK_Notificacion_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])
COMMIT TRANSACTION
GO
