/*
Run this script on:

(local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

(local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Data Compare version 10.4.8 from Red Gate Software Ltd at 23/05/2014 14:07:25

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

PRINT(N'Drop constraint FK_Entrada_Linea_Producto_Garantia from [dbo].[Entrada_Linea]')
GO
ALTER TABLE [dbo].[Entrada_Linea] DROP CONSTRAINT [FK_Entrada_Linea_Producto_Garantia]

PRINT(N'Drop constraint FK_Producto_Producto_Garantia from [dbo].[Producto]')
GO
ALTER TABLE [dbo].[Producto] DROP CONSTRAINT [FK_Producto_Producto_Garantia]

PRINT(N'Drop constraint FK_ListadoADV_Formulario from [dbo].[ListadoADV]')
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [FK_ListadoADV_Formulario]

PRINT(N'Drop constraint FK_Menu_Formulario from [dbo].[Menus]')
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_Formulario]

PRINT(N'Drop constraint FK_Notificacion_Formulario from [dbo].[Notificacion]')
GO
ALTER TABLE [dbo].[Notificacion] DROP CONSTRAINT [FK_Notificacion_Formulario]

PRINT(N'Update row in [dbo].[Producto_Garantia]')
GO
ALTER TABLE [dbo].[Formulario] ADD
[AperturaExterna] [bit] NOT NULL CONSTRAINT [DF_Formulario_AperturaExterna] DEFAULT ((1))
GO

UPDATE [dbo].[Producto_Garantia] SET [Tiempo]=0.50 WHERE [ID_Producto_Garantia]=12

PRINT(N'Update rows in [dbo].[Maestro]')
GO
UPDATE [dbo].[Maestro] SET [Descripcion]=N'Propuesta linea - Tipo de Zona ' WHERE [ID_Maestro]=78
UPDATE [dbo].[Maestro] SET [Descripcion]=N'Campaña - Estados de los seguimientos' WHERE [ID_Maestro]=97
UPDATE [dbo].[Maestro] SET [Descripcion]=N'Clientes - Sectores' WHERE [ID_Maestro]=99
UPDATE [dbo].[Maestro] SET [Descripcion]=N'Campaña - Respuestas según división' WHERE [ID_Maestro]=101
UPDATE [dbo].[Maestro] SET [Descripcion]=N'Parte -  cuestionario de preguntas' WHERE [ID_Maestro]=102
UPDATE [dbo].[Maestro] SET [Descripcion]=N'Parte horas - Tipos de actuación ' WHERE [ID_Maestro]=103
UPDATE [dbo].[Maestro] SET [Descripcion]=N'Ubicación de los pagos y cobros' WHERE [ID_Maestro]=106
UPDATE [dbo].[Maestro] SET [Descripcion]=N'Listados avanzados agrupaciones' WHERE [ID_Maestro]=107
PRINT(N'Operation applied to 8 rows out of 8')
GO

PRINT(N'Update rows in [dbo].[Formulario]')
GO
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Planificación técnica' WHERE [ID_Formulario]=0
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Mantenimiento de tipos de cables' WHERE [ID_Formulario]=1
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Clientes' WHERE [ID_Formulario]=2
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Cableado de la instalación' WHERE [ID_Formulario]=5
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Partes de trabajo de la instalación' WHERE [ID_Formulario]=6
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Instalación - Traspaso' WHERE [ID_Formulario]=7
UPDATE [dbo].[Formulario] SET [NombreReal]=N'frmManteniment', [Descripcion]=N'Borrar' WHERE [ID_Formulario]=19
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Divisiones - Familias - Subfamilias' WHERE [ID_Formulario]=20
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Familias automatismo' WHERE [ID_Formulario]=21
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Mantenimiento de los grados' WHERE [ID_Formulario]=22
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Partes de trabajo - Reparación' WHERE [ID_Formulario]=24
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Borrar' WHERE [ID_Formulario]=25
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Planos' WHERE [ID_Formulario]=27
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Propuesta' WHERE [ID_Formulario]=31
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Propuesta línea' WHERE [ID_Formulario]=32
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Borrar' WHERE [ID_Formulario]=33
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Proveedor' WHERE [ID_Formulario]=34
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Mantenimiento del Script de los informes' WHERE [ID_Formulario]=44
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Personal - Incidencia' WHERE [ID_Formulario]=46
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Informes - Diseñador' WHERE [ID_Formulario]=52
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Informes - Plantillas' WHERE [ID_Formulario]=53
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Línea de documento' WHERE [ID_Formulario]=55
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Línea de documento - Compuesto por...' WHERE [ID_Formulario]=56
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Línea de documento - Partes' WHERE [ID_Formulario]=57
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Línea de documento - Tal y como se instaló' WHERE [ID_Formulario]=58
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Instalación -  Emplazmiento -  Historia de robos' WHERE [ID_Formulario]=102
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Propuesta linea - Tipo de Zona ' WHERE [ID_Formulario]=150
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Campaña - Estados de los seguimientos' WHERE [ID_Formulario]=169
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Borrar', [ParametroEntrada]=NULL WHERE [ID_Formulario]=170
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Clientes - Sectores' WHERE [ID_Formulario]=171
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Campaña - Respuestas según división' WHERE [ID_Formulario]=172
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Parte -  cuestionario de preguntas' WHERE [ID_Formulario]=173
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Parte horas - Tipos de actuación ' WHERE [ID_Formulario]=174
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Ubicación de los pagos y cobros' WHERE [ID_Formulario]=177
UPDATE [dbo].[Formulario] SET [Descripcion]=N'Listados avanzados agrupaciones' WHERE [ID_Formulario]=178
PRINT(N'Operation applied to 35 rows out of 35')
GO

PRINT(N'Add rows to [dbo].[Formulario]')
GO
INSERT INTO [dbo].[Formulario] ([ID_Formulario], [NombreReal], [Descripcion], [ParametroEntrada]) VALUES (204, N'frmManteniment', N'Mantenimiento de delegaciones', 124)
INSERT INTO [dbo].[Formulario] ([ID_Formulario], [NombreReal], [Descripcion], [ParametroEntrada]) VALUES (205, N'frmManteniment', N'Mantenimiento de contadores de los documentos de gestión', 125)
PRINT(N'Operation applied to 2 rows out of 2')
GO

PRINT(N'Add rows to [dbo].[Maestro]')
GO
INSERT INTO [dbo].[Maestro] ([ID_Maestro], [Tabla], [Descripcion], [Interna]) VALUES (124, N'Delegacion', N'Mantenimiento de delegaciones', 0)
INSERT INTO [dbo].[Maestro] ([ID_Maestro], [Tabla], [Descripcion], [Interna]) VALUES (125, N'Entrada_Tipo', N'Mantenimiento de los contadores de los documentos de gestión', 0)
PRINT(N'Operation applied to 2 rows out of 2')
GO

PRINT(N'Add constraint FK_Entrada_Linea_Producto_Garantia to [dbo].[Entrada_Linea]')
GO
ALTER TABLE [dbo].[Entrada_Linea] WITH NOCHECK ADD CONSTRAINT [FK_Entrada_Linea_Producto_Garantia] FOREIGN KEY ([ID_Producto_Garantia]) REFERENCES [dbo].[Producto_Garantia] ([ID_Producto_Garantia])

PRINT(N'Add constraint FK_Producto_Producto_Garantia to [dbo].[Producto]')
GO
ALTER TABLE [dbo].[Producto] WITH NOCHECK ADD CONSTRAINT [FK_Producto_Producto_Garantia] FOREIGN KEY ([ID_Producto_Garantia]) REFERENCES [dbo].[Producto_Garantia] ([ID_Producto_Garantia])

PRINT(N'Add constraint FK_ListadoADV_Formulario to [dbo].[ListadoADV]')
GO
ALTER TABLE [dbo].[ListadoADV] WITH NOCHECK ADD CONSTRAINT [FK_ListadoADV_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])

PRINT(N'Add constraint FK_Menu_Formulario to [dbo].[Menus]')
GO
ALTER TABLE [dbo].[Menus] WITH NOCHECK ADD CONSTRAINT [FK_Menu_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])

PRINT(N'Add constraint FK_Notificacion_Formulario to [dbo].[Notificacion]')
GO
ALTER TABLE [dbo].[Notificacion] WITH NOCHECK ADD CONSTRAINT [FK_Notificacion_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])
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
GO
COMMIT TRANSACTION
GO

