/*
Run this script on:

Server2012R2\SQLServer2012.AbidosMestre    -  This database will be modified

to synchronize it with:

Server2012R2\SQLServer2012.AbidosDomingo

You are recommended to back up your database before running this script

Script created by SQL Data Compare version 10.4.8 from Red Gate Software Ltd at 16/03/2015 14:40:00

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

PRINT(N'Add rows to [dbo].[ActividadCRM_Accion_Tipo]')
GO
SET IDENTITY_INSERT [dbo].[ActividadCRM_Accion_Tipo] ON
INSERT INTO [dbo].[ActividadCRM_Accion_Tipo] ([ID_ActividadCRM_Accion_Tipo], [Codigo], [Descripcion], [Activo], [RO]) VALUES (1, 1, N'Tipo 1', 1, 0)
INSERT INTO [dbo].[ActividadCRM_Accion_Tipo] ([ID_ActividadCRM_Accion_Tipo], [Codigo], [Descripcion], [Activo], [RO]) VALUES (2, 2, N'Tipo 2', 1, 0)
SET IDENTITY_INSERT [dbo].[ActividadCRM_Accion_Tipo] OFF
PRINT(N'Operation applied to 2 rows out of 2')
GO

PRINT(N'Add rows to [dbo].[Formulario]')
GO
INSERT INTO [dbo].[Formulario] ([ID_Formulario], [NombreReal], [Descripcion], [ParametroEntrada], [AperturaExterna]) VALUES (211, N'frmNotificacionesUsuarios', N'Notificaciones - Asignación de usuarios', NULL, 1)
INSERT INTO [dbo].[Formulario] ([ID_Formulario], [NombreReal], [Descripcion], [ParametroEntrada], [AperturaExterna]) VALUES (212, N'frmAutomatismo', N'Mantenimiento de automatismos', NULL, 1)
PRINT(N'Operation applied to 2 rows out of 40')
GO

PRINT(N'Add rows to [dbo].[Maestro]')
GO
INSERT INTO [dbo].[Maestro] ([ID_Maestro], [Tabla], [Descripcion], [Interna]) VALUES (127, N'ActividadCRM_Accion_Tipo', N'Mantenimiento de tipos de acciones', 0)
PRINT(N'Operation applied to 1 rows out of 37')
GO
COMMIT TRANSACTION
GO