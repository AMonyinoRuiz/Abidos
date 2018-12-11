/*
Run this script on:

(local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

(local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Data Compare version 10.4.8 from Red Gate Software Ltd at 18/07/2014 21:10:25

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

PRINT(N'Update row in [dbo].[GRID_Columna]')
GO
UPDATE [dbo].[GRID_Columna] SET [Columna_Caption]=N'Cólor' WHERE [ID_Grid_Columna]=233546

PRINT(N'Update rows in [dbo].[Formulario]')
GO
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=4
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=5
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=6
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=7
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=19
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=21
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=25
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=27
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=29
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=30
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=32
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=43
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=54
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=55
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=56
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=57
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=58
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=59
UPDATE [dbo].[Formulario] SET [AperturaExterna]=0 WHERE [ID_Formulario]=60
PRINT(N'Operation applied to 19 rows out of 19')
GO

PRINT(N'Add row to [dbo].[Formulario]')
GO
INSERT INTO [dbo].[Formulario] ([ID_Formulario], [NombreReal], [Descripcion], [ParametroEntrada], [AperturaExterna]) VALUES (206, N'frmPropuesta_Especificacion', N'Mantenimiento de las especificaciones de los presupuestos', NULL, 1)

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
