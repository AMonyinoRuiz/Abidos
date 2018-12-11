/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 25/11/2014 16:08:34

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
PRINT N'Dropping foreign keys from [dbo].[BI_Usuario]'
GO
ALTER TABLE [dbo].[BI_Usuario] DROP CONSTRAINT [FK_BI_Usuario_BI]
ALTER TABLE [dbo].[BI_Usuario] DROP CONSTRAINT [FK_BI_Usuario_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menus_BI]
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_Gauge]
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_Listado]
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_ListadoADV]
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_Menu_Tipo]
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_Formulario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[FormaPago]'
GO
ALTER TABLE [dbo].[FormaPago] DROP CONSTRAINT [FK_FormaPago_FormaPago_Tipo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Formulario_Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] DROP CONSTRAINT [FK_Formulario_Usuario_Grupo_Formulario]
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] DROP CONSTRAINT [FK_Formulario_Usuario_Grupo_Usuario_Grupo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[GaugeAgrupacion_Gauge]'
GO
ALTER TABLE [dbo].[GaugeAgrupacion_Gauge] DROP CONSTRAINT [FK_GaugeAgrupacion_Gauge_Gauge]
ALTER TABLE [dbo].[GaugeAgrupacion_Gauge] DROP CONSTRAINT [FK_GaugeAgrupacion_Gauge_GaugeAgrupacion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Gauge]'
GO
ALTER TABLE [dbo].[Gauge] DROP CONSTRAINT [FK_Gauge_ListadoADV]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Entrada_Vencimiento]'
GO
ALTER TABLE [dbo].[Entrada_Vencimiento] DROP CONSTRAINT [FK_Entrada_Vencimiento_Entrada_Vencimiento_Estado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Informe_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Apartado_Version] DROP CONSTRAINT [FK_Informe_Apartado_Version_Informe_Apartado1]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Informe_Plantilla_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] DROP CONSTRAINT [FK_Informe_Plantilla_Apartado_Version_Informe_Apartado]
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] DROP CONSTRAINT [FK_Informe_Plantilla_Apartado_Version_Informe_Apartado_Version]
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] DROP CONSTRAINT [FK_Informe_Plantilla_Apartado_Version_Informe_Plantilla]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Informe_Apartado]'
GO
ALTER TABLE [dbo].[Informe_Apartado] DROP CONSTRAINT [FK_Informe_Apartado_Informe]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Informe_Plantilla]'
GO
ALTER TABLE [dbo].[Informe_Plantilla] DROP CONSTRAINT [FK_Informe_Plantilla_Informe]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Listado]'
GO
ALTER TABLE [dbo].[Listado] DROP CONSTRAINT [FK_Listado_Listado_Entidad]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [FK_ListadoADV_ListadoADV_Agrupacion]
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [FK_ListadoADV_Formulario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Campaña_Cliente_Seguimiento]'
GO
ALTER TABLE [dbo].[Campaña_Cliente_Seguimiento] DROP CONSTRAINT [FK_Campaña_Cliente_Seguimiento_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Campaña_Usuario]'
GO
ALTER TABLE [dbo].[Campaña_Usuario] DROP CONSTRAINT [FK_Campaña_Usuario_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Cliente_Seguridad]'
GO
ALTER TABLE [dbo].[Cliente_Seguridad] DROP CONSTRAINT [FK_Cliente_Seguridad_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Instalacion_Seguridad]'
GO
ALTER TABLE [dbo].[Instalacion_Seguridad] DROP CONSTRAINT [FK_Instalacion_Seguridad_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Log_Sesiones]'
GO
ALTER TABLE [dbo].[Log_Sesiones] DROP CONSTRAINT [FK_Log_Sesiones_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Notificacion]'
GO
ALTER TABLE [dbo].[Notificacion] DROP CONSTRAINT [FK_Notificacion_Usuario2]
ALTER TABLE [dbo].[Notificacion] DROP CONSTRAINT [FK_Notificacion_Usuario3]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Notificacion_Automatica_Usuario]'
GO
ALTER TABLE [dbo].[Notificacion_Automatica_Usuario] DROP CONSTRAINT [FK_Notififcacion_Automatica_Usuario_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Parte_ToDo]'
GO
ALTER TABLE [dbo].[Parte_ToDo] DROP CONSTRAINT [FK_Parte_ToDo_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Personal_Seguridad]'
GO
ALTER TABLE [dbo].[Personal_Seguridad] DROP CONSTRAINT [FK_Personal_Seguridad_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Propuesta_PropuestaEspecificacion]'
GO
ALTER TABLE [dbo].[Propuesta_PropuestaEspecificacion] DROP CONSTRAINT [FK_Propuesta_PropuestaCuestionario_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Propuesta_Seguridad]'
GO
ALTER TABLE [dbo].[Propuesta_Seguridad] DROP CONSTRAINT [FK_Propuesta_Seguridad_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Proveedor_Seguridad]'
GO
ALTER TABLE [dbo].[Proveedor_Seguridad] DROP CONSTRAINT [FK_Proveedor_Seguridad_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Usuario]'
GO
ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT [FK_Usuario_Usuario_Grupo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[BI_Usuario]'
GO
ALTER TABLE [dbo].[BI_Usuario] DROP CONSTRAINT [PK_BI_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[BI_Usuario]'
GO
ALTER TABLE [dbo].[BI_Usuario] DROP CONSTRAINT [DF_BI_Usuario_CargarAlIniciarPrograma]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Conexiones]'
GO
ALTER TABLE [dbo].[Conexiones] DROP CONSTRAINT [PK_Table_1_1]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Conexiones]'
GO
ALTER TABLE [dbo].[Conexiones] DROP CONSTRAINT [DF_Conexiones_Activo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[BI]'
GO
ALTER TABLE [dbo].[BI] DROP CONSTRAINT [PK_BI]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[BI]'
GO
ALTER TABLE [dbo].[BI] DROP CONSTRAINT [DF_BI_Refresco]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[BI]'
GO
ALTER TABLE [dbo].[BI] DROP CONSTRAINT [DF_BI_Activo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[FORM_Controls_BD]'
GO
ALTER TABLE [dbo].[FORM_Controls_BD] DROP CONSTRAINT [PK_FORM_Controls_BD]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[FORM_Controls]'
GO
ALTER TABLE [dbo].[FORM_Controls] DROP CONSTRAINT [PK_MEMPHIS_OBJETO]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[FORM_Controls]'
GO
ALTER TABLE [dbo].[FORM_Controls] DROP CONSTRAINT [DF_MEMPHIS_OBJETO_Visible]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[FORM_Controls]'
GO
ALTER TABLE [dbo].[FORM_Controls] DROP CONSTRAINT [DF_MEMPHIS_OBJETO_ToolTip]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[FORM_Controls]'
GO
ALTER TABLE [dbo].[FORM_Controls] DROP CONSTRAINT [DF_MEMPHIS_OBJETO_Observacion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Formulario_Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] DROP CONSTRAINT [PK_Formulario_Usuario_Grupo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Formulario_Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] DROP CONSTRAINT [DF_Formulario_Usuario_Grupo_Visualizar]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Formulario_Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] DROP CONSTRAINT [DF_Table_1_Edicion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Formulario_Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] DROP CONSTRAINT [DF_Table_1_Borrado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Formulario_Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] DROP CONSTRAINT [DF_Formulario_Usuario_Grupo_Todo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Formulario_Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] DROP CONSTRAINT [DF_Formulario_Usuario_Grupo_NumEnCancheAlIniciar]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Gauge]'
GO
ALTER TABLE [dbo].[Gauge] DROP CONSTRAINT [PK_Table_1_2]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Gauge]'
GO
ALTER TABLE [dbo].[Gauge] DROP CONSTRAINT [DF_Gauge_NivelSeguridad]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[GaugeAgrupacion]'
GO
ALTER TABLE [dbo].[GaugeAgrupacion] DROP CONSTRAINT [PK_GaugeAgrupacion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[GaugeAgrupacion]'
GO
ALTER TABLE [dbo].[GaugeAgrupacion] DROP CONSTRAINT [DF_GaugeAgrupacion_NivelSeguridad]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[GaugeAgrupacion]'
GO
ALTER TABLE [dbo].[GaugeAgrupacion] DROP CONSTRAINT [DF_GaugeAgrupacion_TiempoActualizacion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[GaugeAgrupacion_Gauge]'
GO
ALTER TABLE [dbo].[GaugeAgrupacion_Gauge] DROP CONSTRAINT [PK_GaugeAgrupacion_Gaute]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Apartado]'
GO
ALTER TABLE [dbo].[Informe_Apartado] DROP CONSTRAINT [PK_Informe_Apartado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Apartado]'
GO
ALTER TABLE [dbo].[Informe_Apartado] DROP CONSTRAINT [DF_Informe_Apartado_RO]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Apartado_Version] DROP CONSTRAINT [PK_Informe_Apartado_Version]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Apartado_Version] DROP CONSTRAINT [DF_Informe_Apartado_Version_Activo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Apartado_Version] DROP CONSTRAINT [DF_Informe_Apartado_Version_RO]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Plantilla]'
GO
ALTER TABLE [dbo].[Informe_Plantilla] DROP CONSTRAINT [PK_Informe_Plantilla2]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Plantilla]'
GO
ALTER TABLE [dbo].[Informe_Plantilla] DROP CONSTRAINT [DF_Informe_Plantilla_Predeterminada]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Plantilla]'
GO
ALTER TABLE [dbo].[Informe_Plantilla] DROP CONSTRAINT [DF_Informe_Plantilla_NivelSeguridad]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Plantilla_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] DROP CONSTRAINT [PK_Informe_Agrupacion_Version]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Plantilla_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] DROP CONSTRAINT [DF_Informe_Plantilla_Apartado_Version_Orden]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Plantilla_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] DROP CONSTRAINT [DF_Informe_Plantilla_Apartado_Version_Seleccionado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Informe_Plantilla_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] DROP CONSTRAINT [DF_Informe_Plantilla_Apartado_Version_NumCopias]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Listado]'
GO
ALTER TABLE [dbo].[Listado] DROP CONSTRAINT [PK_Listado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Listado]'
GO
ALTER TABLE [dbo].[Listado] DROP CONSTRAINT [DF_Listado_Nivell_Seguretat]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Listado]'
GO
ALTER TABLE [dbo].[Listado] DROP CONSTRAINT [DF_Listado_Activo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Listado]'
GO
ALTER TABLE [dbo].[Listado] DROP CONSTRAINT [DF_Listado_RO]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Listado_Entidad]'
GO
ALTER TABLE [dbo].[Listado_Entidad] DROP CONSTRAINT [PK_Listado_Entidad]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [PK_ListadoADV]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [DF_ListadoADV_NivelSeguridad]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [DF_ListadoADV_InvisibleCampoApertura]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [DF_ListadoADV_RO]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[ListadoADV_Agrupacion]'
GO
ALTER TABLE [dbo].[ListadoADV_Agrupacion] DROP CONSTRAINT [PK_Listados_Agrupacion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [PK_Menu]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [DF_Menus_NivellSeguretat]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [DF_Menus_Ordre]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Usuario]'
GO
ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT [PK_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Usuario]'
GO
ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT [DF_Usuario_TiempoInactividadPantallaLogin]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Usuario]'
GO
ALTER TABLE [dbo].[Usuario] DROP CONSTRAINT [DF_Usuario_Activo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Usuario_Grupo] DROP CONSTRAINT [PK_Usuario_Grupo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Usuario_Grupo] DROP CONSTRAINT [DF_Usuario_Grupo_Activo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping index [IX_Menus] from [dbo].[Menus]'
GO
DROP INDEX [IX_Menus] ON [dbo].[Menus]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Usuario]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Usuario]
(
[ID_Usuario] [int] NOT NULL IDENTITY(50000, 1),
[ID_Usuario_Grupo] [int] NOT NULL,
[ID_Personal] [int] NULL,
[NivelSeguridad] [int] NOT NULL,
[Nombre] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[NombreCompleto] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Contraseña] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
[TiempoInactividadPantallaLogin] [int] NOT NULL CONSTRAINT [DF_Usuario_TiempoInactividadPantallaLogin] DEFAULT ((0)),
[Activo] [bit] NOT NULL CONSTRAINT [DF_Usuario_Activo] DEFAULT ((1)),
[CampoAux1] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAux2] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAux3] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAux4] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Usuario] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Usuario]([ID_Usuario], [ID_Usuario_Grupo], [ID_Personal], [NivelSeguridad], [Nombre], [NombreCompleto], [Contraseña], [TiempoInactividadPantallaLogin], [Activo], [CampoAux1], [CampoAux2], [CampoAux3], [CampoAux4]) SELECT [ID_Usuario], [ID_Usuario_Grupo], [ID_Personal], [NivelSeguridad], [Nombre], [NombreCompleto], [Contraseña], [TiempoInactividadPantallaLogin], [Activo], [CampoAux1], [CampoAux2], [CampoAux3], [CampoAux4] FROM [dbo].[Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Usuario] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Usuario]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Usuario]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Usuario]', N'Usuario'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Usuario] on [dbo].[Usuario]'
GO
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED  ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Menus]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Menus]
(
[ID_Menus] [int] NOT NULL IDENTITY(50000, 1),
[ID_Menus_Tipo] [int] NOT NULL,
[ID_Menus_Padre] [int] NULL,
[Descripcion] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
[ID_Formulario] [int] NULL,
[ID_GaugeAgrupacion] [int] NULL,
[ID_Listado] [int] NULL,
[ID_ListadoADV] [int] NULL,
[ID_BI] [int] NULL,
[Observaciones] [nvarchar] (300) COLLATE Modern_Spanish_CI_AS NULL,
[NivellSeguretat] [int] NOT NULL CONSTRAINT [DF_Menus_NivellSeguretat] DEFAULT ((10)),
[NomFoto] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
[Ordre] [int] NOT NULL CONSTRAINT [DF_Menus_Ordre] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Menus] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Menus]([ID_Menus], [ID_Menus_Tipo], [ID_Menus_Padre], [Descripcion], [ID_Formulario], [ID_GaugeAgrupacion], [ID_Listado], [ID_ListadoADV], [ID_BI], [Observaciones], [NivellSeguretat], [NomFoto], [Ordre]) SELECT [ID_Menus], [ID_Menus_Tipo], [ID_Menus_Padre], [Descripcion], [ID_Formulario], [ID_GaugeAgrupacion], [ID_Listado], [ID_ListadoADV], [ID_BI], [Observaciones], [NivellSeguretat], [NomFoto], [Ordre] FROM [dbo].[Menus]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Menus] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Menus]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Menus]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Menus]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Menus]', N'Menus'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Menu] on [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] ADD CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED  ([ID_Menus])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_Menus] on [dbo].[Menus]'
GO
CREATE NONCLUSTERED INDEX [IX_Menus] ON [dbo].[Menus] ([ID_Menus_Padre])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Formulario_Usuario_Grupo]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Formulario_Usuario_Grupo]
(
[ID_Formulario_Usuario_Grupo] [int] NOT NULL IDENTITY(50000, 1),
[ID_Formulario] [int] NOT NULL,
[ID_Usuario_Grupo] [int] NOT NULL,
[Visualizar] [bit] NOT NULL CONSTRAINT [DF_Formulario_Usuario_Grupo_Visualizar] DEFAULT ((1)),
[Editar] [bit] NOT NULL CONSTRAINT [DF_Table_1_Edicion] DEFAULT ((1)),
[Eliminar] [bit] NOT NULL CONSTRAINT [DF_Table_1_Borrado] DEFAULT ((1)),
[Todo] [bit] NOT NULL CONSTRAINT [DF_Formulario_Usuario_Grupo_Todo] DEFAULT ((1)),
[NumEnCancheAlIniciar] [int] NOT NULL CONSTRAINT [DF_Formulario_Usuario_Grupo_NumEnCancheAlIniciar] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Formulario_Usuario_Grupo] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Formulario_Usuario_Grupo]([ID_Formulario_Usuario_Grupo], [ID_Formulario], [ID_Usuario_Grupo], [Visualizar], [Editar], [Eliminar], [Todo], [NumEnCancheAlIniciar]) SELECT [ID_Formulario_Usuario_Grupo], [ID_Formulario], [ID_Usuario_Grupo], [Visualizar], [Editar], [Eliminar], [Todo], [NumEnCancheAlIniciar] FROM [dbo].[Formulario_Usuario_Grupo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Formulario_Usuario_Grupo] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Formulario_Usuario_Grupo]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Formulario_Usuario_Grupo]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Formulario_Usuario_Grupo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Formulario_Usuario_Grupo]', N'Formulario_Usuario_Grupo'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Formulario_Usuario_Grupo] on [dbo].[Formulario_Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] ADD CONSTRAINT [PK_Formulario_Usuario_Grupo] PRIMARY KEY CLUSTERED  ([ID_Formulario_Usuario_Grupo])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Informe_Plantilla]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Informe_Plantilla]
(
[ID_Informe_Plantilla] [int] NOT NULL IDENTITY(50000, 1),
[ID_Informe] [int] NOT NULL,
[Descripcion] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Predeterminada] [bit] NOT NULL CONSTRAINT [DF_Informe_Plantilla_Predeterminada] DEFAULT ((0)),
[NivelSeguridad] [int] NOT NULL CONSTRAINT [DF_Informe_Plantilla_NivelSeguridad] DEFAULT ((100))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Informe_Plantilla] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Informe_Plantilla]([ID_Informe_Plantilla], [ID_Informe], [Descripcion], [Predeterminada], [NivelSeguridad]) SELECT [ID_Informe_Plantilla], [ID_Informe], [Descripcion], [Predeterminada], [NivelSeguridad] FROM [dbo].[Informe_Plantilla]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Informe_Plantilla] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Informe_Plantilla]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Informe_Plantilla]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Informe_Plantilla]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Informe_Plantilla]', N'Informe_Plantilla'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Informe_Plantilla2] on [dbo].[Informe_Plantilla]'
GO
ALTER TABLE [dbo].[Informe_Plantilla] ADD CONSTRAINT [PK_Informe_Plantilla2] PRIMARY KEY CLUSTERED  ([ID_Informe_Plantilla])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[FORM_Controls]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FORM_Controls]
(
[ID_Form_Controls] [int] NOT NULL IDENTITY(50000, 1) NOT FOR REPLICATION,
[Formulario] [nvarchar] (75) COLLATE Modern_Spanish_CI_AS NULL,
[Objeto] [nvarchar] (75) COLLATE Modern_Spanish_CI_AS NULL,
[Texto] [nvarchar] (75) COLLATE Modern_Spanish_CI_AS NULL,
[Width] [int] NULL,
[Height] [int] NULL,
[Visible] [bit] NULL CONSTRAINT [DF_MEMPHIS_OBJETO_Visible] DEFAULT ((1)),
[ForeColor] [bigint] NULL,
[BackColor] [bigint] NULL,
[X] [int] NULL,
[Y] [int] NULL,
[TabIndex] [int] NULL,
[ToolTip] [nvarchar] (1000) COLLATE Modern_Spanish_CI_AS NULL CONSTRAINT [DF_MEMPHIS_OBJETO_ToolTip] DEFAULT (''),
[Observacion] [nvarchar] (1000) COLLATE Modern_Spanish_CI_AS NULL CONSTRAINT [DF_MEMPHIS_OBJETO_Observacion] DEFAULT (''),
[Nivell_Seguretat] [int] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FORM_Controls] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_FORM_Controls]([ID_Form_Controls], [Formulario], [Objeto], [Texto], [Width], [Height], [Visible], [ForeColor], [BackColor], [X], [Y], [TabIndex], [ToolTip], [Observacion], [Nivell_Seguretat]) SELECT [ID_Form_Controls], [Formulario], [Objeto], [Texto], [Width], [Height], [Visible], [ForeColor], [BackColor], [X], [Y], [TabIndex], [ToolTip], [Observacion], [Nivell_Seguretat] FROM [dbo].[FORM_Controls]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FORM_Controls] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[FORM_Controls]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_FORM_Controls]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[FORM_Controls]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FORM_Controls]', N'FORM_Controls'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_MEMPHIS_OBJETO] on [dbo].[FORM_Controls]'
GO
ALTER TABLE [dbo].[FORM_Controls] ADD CONSTRAINT [PK_MEMPHIS_OBJETO] PRIMARY KEY CLUSTERED  ([ID_Form_Controls])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Informe_Apartado_Version]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Informe_Apartado_Version]
(
[ID_Informe_Apartado_Version] [int] NOT NULL IDENTITY(50000, 1),
[ID_Informe_Apartado] [int] NOT NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Fecha] [smalldatetime] NOT NULL,
[Fichero] [varbinary] (max) NULL,
[Activo] [bit] NOT NULL CONSTRAINT [DF_Informe_Apartado_Version_Activo] DEFAULT ((1)),
[RO] [bit] NOT NULL CONSTRAINT [DF_Informe_Apartado_Version_RO] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Informe_Apartado_Version] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Informe_Apartado_Version]([ID_Informe_Apartado_Version], [ID_Informe_Apartado], [Descripcion], [Fecha], [Fichero], [Activo], [RO]) SELECT [ID_Informe_Apartado_Version], [ID_Informe_Apartado], [Descripcion], [Fecha], [Fichero], [Activo], [RO] FROM [dbo].[Informe_Apartado_Version]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Informe_Apartado_Version] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Informe_Apartado_Version]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Informe_Apartado_Version]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Informe_Apartado_Version]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Informe_Apartado_Version]', N'Informe_Apartado_Version'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Informe_Apartado_Version] on [dbo].[Informe_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Apartado_Version] ADD CONSTRAINT [PK_Informe_Apartado_Version] PRIMARY KEY CLUSTERED  ([ID_Informe_Apartado_Version])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Informe_Plantilla_Apartado_Version]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Informe_Plantilla_Apartado_Version]
(
[ID_Informe_Plantilla_Apartado_Version] [int] NOT NULL IDENTITY(50000, 1),
[ID_Informe_Plantilla] [int] NOT NULL,
[ID_Informe_Apartado] [int] NOT NULL,
[ID_Informe_Apartado_Version] [int] NULL,
[Orden] [int] NOT NULL CONSTRAINT [DF_Informe_Plantilla_Apartado_Version_Orden] DEFAULT ((0)),
[Seleccionado] [bit] NOT NULL CONSTRAINT [DF_Informe_Plantilla_Apartado_Version_Seleccionado] DEFAULT ((1)),
[NumCopias] [int] NOT NULL CONSTRAINT [DF_Informe_Plantilla_Apartado_Version_NumCopias] DEFAULT ((1))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Informe_Plantilla_Apartado_Version] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Informe_Plantilla_Apartado_Version]([ID_Informe_Plantilla_Apartado_Version], [ID_Informe_Plantilla], [ID_Informe_Apartado], [ID_Informe_Apartado_Version], [Orden], [Seleccionado], [NumCopias]) SELECT [ID_Informe_Plantilla_Apartado_Version], [ID_Informe_Plantilla], [ID_Informe_Apartado], [ID_Informe_Apartado_Version], [Orden], [Seleccionado], [NumCopias] FROM [dbo].[Informe_Plantilla_Apartado_Version]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Informe_Plantilla_Apartado_Version] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Informe_Plantilla_Apartado_Version]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Informe_Plantilla_Apartado_Version]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Informe_Plantilla_Apartado_Version]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Informe_Plantilla_Apartado_Version]', N'Informe_Plantilla_Apartado_Version'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Informe_Agrupacion_Version] on [dbo].[Informe_Plantilla_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] ADD CONSTRAINT [PK_Informe_Agrupacion_Version] PRIMARY KEY CLUSTERED  ([ID_Informe_Plantilla_Apartado_Version])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Gauge]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Gauge]
(
[ID_Gauge] [int] NOT NULL IDENTITY(50000, 1),
[ID_ListadoADV] [int] NULL,
[NivelSeguridad] [int] NOT NULL CONSTRAINT [DF_Gauge_NivelSeguridad] DEFAULT ((10)),
[FechaAlta] [smalldatetime] NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Observaciones] [nvarchar] (500) COLLATE Modern_Spanish_CI_AS NULL,
[SQL] [nvarchar] (4000) COLLATE Modern_Spanish_CI_AS NOT NULL,
[RangoMinimo1] [decimal] (10, 2) NULL,
[RangoMaximo1] [decimal] (10, 2) NULL,
[RangoMinimo2] [decimal] (10, 2) NULL,
[RangoMaximo2] [decimal] (10, 2) NULL,
[RangoMinimo3] [decimal] (10, 2) NULL,
[RangoMaximo3] [decimal] (10, 2) NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Gauge] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Gauge]([ID_Gauge], [ID_ListadoADV], [NivelSeguridad], [FechaAlta], [Descripcion], [Observaciones], [SQL], [RangoMinimo1], [RangoMaximo1], [RangoMinimo2], [RangoMaximo2], [RangoMinimo3], [RangoMaximo3]) SELECT [ID_Gauge], [ID_ListadoADV], [NivelSeguridad], [FechaAlta], [Descripcion], [Observaciones], [SQL], [RangoMinimo1], [RangoMaximo1], [RangoMinimo2], [RangoMaximo2], [RangoMinimo3], [RangoMaximo3] FROM [dbo].[Gauge]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Gauge] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Gauge]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Gauge]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Gauge]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Gauge]', N'Gauge'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Table_1_2] on [dbo].[Gauge]'
GO
ALTER TABLE [dbo].[Gauge] ADD CONSTRAINT [PK_Table_1_2] PRIMARY KEY CLUSTERED  ([ID_Gauge])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Listado]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Listado]
(
[ID_Listado] [int] NOT NULL IDENTITY(50000, 1),
[ID_Listado_Entidad] [int] NOT NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Fecha] [smalldatetime] NOT NULL,
[Fichero] [varbinary] (max) NULL,
[NivelSeguridad] [int] NOT NULL CONSTRAINT [DF_Listado_Nivell_Seguretat] DEFAULT ((10)),
[Activo] [bit] NOT NULL CONSTRAINT [DF_Listado_Activo] DEFAULT ((1)),
[RO] [bit] NOT NULL CONSTRAINT [DF_Listado_RO] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Listado] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Listado]([ID_Listado], [ID_Listado_Entidad], [Descripcion], [Fecha], [Fichero], [NivelSeguridad], [Activo], [RO]) SELECT [ID_Listado], [ID_Listado_Entidad], [Descripcion], [Fecha], [Fichero], [NivelSeguridad], [Activo], [RO] FROM [dbo].[Listado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Listado] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Listado]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Listado]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Listado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Listado]', N'Listado'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Listado] on [dbo].[Listado]'
GO
ALTER TABLE [dbo].[Listado] ADD CONSTRAINT [PK_Listado] PRIMARY KEY CLUSTERED  ([ID_Listado])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Usuario_Grupo]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Usuario_Grupo]
(
[ID_Usuario_Grupo] [int] NOT NULL IDENTITY(50000, 1),
[Descripcion] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Activo] [bit] NOT NULL CONSTRAINT [DF_Usuario_Grupo_Activo] DEFAULT ((1))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Usuario_Grupo] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Usuario_Grupo]([ID_Usuario_Grupo], [Descripcion], [Activo]) SELECT [ID_Usuario_Grupo], [Descripcion], [Activo] FROM [dbo].[Usuario_Grupo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Usuario_Grupo] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Usuario_Grupo]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Usuario_Grupo]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Usuario_Grupo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Usuario_Grupo]', N'Usuario_Grupo'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Usuario_Grupo] on [dbo].[Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Usuario_Grupo] ADD CONSTRAINT [PK_Usuario_Grupo] PRIMARY KEY CLUSTERED  ([ID_Usuario_Grupo])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[GaugeAgrupacion]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_GaugeAgrupacion]
(
[ID_GaugeAgrupacion] [int] NOT NULL IDENTITY(50000, 1),
[Codigo] [int] NOT NULL,
[Descripcion] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[FechaAlta] [smalldatetime] NOT NULL,
[NivelSeguridad] [int] NOT NULL CONSTRAINT [DF_GaugeAgrupacion_NivelSeguridad] DEFAULT ((10)),
[Observaciones] [nvarchar] (400) COLLATE Modern_Spanish_CI_AS NULL,
[TiempoActualizacion] [int] NOT NULL CONSTRAINT [DF_GaugeAgrupacion_TiempoActualizacion] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_GaugeAgrupacion] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_GaugeAgrupacion]([ID_GaugeAgrupacion], [Codigo], [Descripcion], [FechaAlta], [NivelSeguridad], [Observaciones], [TiempoActualizacion]) SELECT [ID_GaugeAgrupacion], [Codigo], [Descripcion], [FechaAlta], [NivelSeguridad], [Observaciones], [TiempoActualizacion] FROM [dbo].[GaugeAgrupacion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_GaugeAgrupacion] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[GaugeAgrupacion]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_GaugeAgrupacion]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[GaugeAgrupacion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_GaugeAgrupacion]', N'GaugeAgrupacion'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_GaugeAgrupacion] on [dbo].[GaugeAgrupacion]'
GO
ALTER TABLE [dbo].[GaugeAgrupacion] ADD CONSTRAINT [PK_GaugeAgrupacion] PRIMARY KEY CLUSTERED  ([ID_GaugeAgrupacion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[BI]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_BI]
(
[ID_BI] [int] NOT NULL IDENTITY(50000, 1),
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Refresco] [int] NOT NULL CONSTRAINT [DF_BI_Refresco] DEFAULT ((0)),
[Activo] [bit] NOT NULL CONSTRAINT [DF_BI_Activo] DEFAULT ((1)),
[Archivo] [varbinary] (max) NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_BI] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_BI]([ID_BI], [Descripcion], [Refresco], [Activo], [Archivo]) SELECT [ID_BI], [Descripcion], [Refresco], [Activo], [Archivo] FROM [dbo].[BI]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_BI] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[BI]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_BI]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[BI]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_BI]', N'BI'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_BI] on [dbo].[BI]'
GO
ALTER TABLE [dbo].[BI] ADD CONSTRAINT [PK_BI] PRIMARY KEY CLUSTERED  ([ID_BI])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[BI_Usuario]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_BI_Usuario]
(
[ID_BI_Usuario] [int] NOT NULL IDENTITY(50000, 1),
[ID_BI] [int] NOT NULL,
[ID_Usuario] [int] NOT NULL,
[CargarAlIniciarPrograma] [bit] NOT NULL CONSTRAINT [DF_BI_Usuario_CargarAlIniciarPrograma] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_BI_Usuario] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_BI_Usuario]([ID_BI_Usuario], [ID_BI], [ID_Usuario], [CargarAlIniciarPrograma]) SELECT [ID_BI_Usuario], [ID_BI], [ID_Usuario], [CargarAlIniciarPrograma] FROM [dbo].[BI_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_BI_Usuario] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[BI_Usuario]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_BI_Usuario]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[BI_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_BI_Usuario]', N'BI_Usuario'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_BI_Usuario] on [dbo].[BI_Usuario]'
GO
ALTER TABLE [dbo].[BI_Usuario] ADD CONSTRAINT [PK_BI_Usuario] PRIMARY KEY CLUSTERED  ([ID_BI_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[ListadoADV]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ListadoADV]
(
[ID_ListadoADV] [int] NOT NULL IDENTITY(50000, 1),
[ID_ListadoADV_Agrupacion] [int] NOT NULL,
[ID_Formulario] [int] NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[FechaAlta] [smalldatetime] NOT NULL,
[NivelSeguridad] [int] NOT NULL CONSTRAINT [DF_ListadoADV_NivelSeguridad] DEFAULT ((10)),
[ConsultaSQL] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[FiltrosPredeterminados] [nvarchar] (4000) COLLATE Modern_Spanish_CI_AS NULL,
[NombreCampoAperturaFormulario] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[CodigoApertura] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
[AlternarColorFilas] [bit] NOT NULL CONSTRAINT [DF_ListadoADV_InvisibleCampoApertura] DEFAULT ((0)),
[RO] [bit] NOT NULL CONSTRAINT [DF_ListadoADV_RO] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ListadoADV] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_ListadoADV]([ID_ListadoADV], [ID_ListadoADV_Agrupacion], [ID_Formulario], [Descripcion], [FechaAlta], [NivelSeguridad], [ConsultaSQL], [FiltrosPredeterminados], [NombreCampoAperturaFormulario], [CodigoApertura], [AlternarColorFilas], [RO]) SELECT [ID_ListadoADV], [ID_ListadoADV_Agrupacion], [ID_Formulario], [Descripcion], [FechaAlta], [NivelSeguridad], [ConsultaSQL], [FiltrosPredeterminados], [NombreCampoAperturaFormulario], [CodigoApertura], [AlternarColorFilas], [RO] FROM [dbo].[ListadoADV]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ListadoADV] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[ListadoADV]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_ListadoADV]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[ListadoADV]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ListadoADV]', N'ListadoADV'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ListadoADV] on [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] ADD CONSTRAINT [PK_ListadoADV] PRIMARY KEY CLUSTERED  ([ID_ListadoADV])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Conexiones]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Conexiones]
(
[ID_Conexiones] [int] NOT NULL IDENTITY(50000, 1),
[Codigo] [int] NOT NULL,
[Descripcion] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Servidor] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[BaseDeDatos] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Activo] [bit] NOT NULL CONSTRAINT [DF_Conexiones_Activo] DEFAULT ((1))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Conexiones] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Conexiones]([ID_Conexiones], [Codigo], [Descripcion], [Servidor], [BaseDeDatos], [Activo]) SELECT [ID_Conexiones], [Codigo], [Descripcion], [Servidor], [BaseDeDatos], [Activo] FROM [dbo].[Conexiones]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Conexiones] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Conexiones]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Conexiones]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Conexiones]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Conexiones]', N'Conexiones'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Table_1_1] on [dbo].[Conexiones]'
GO
ALTER TABLE [dbo].[Conexiones] ADD CONSTRAINT [PK_Table_1_1] PRIMARY KEY CLUSTERED  ([ID_Conexiones])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Informe_Apartado]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Informe_Apartado]
(
[ID_Informe_Apartado] [int] NOT NULL IDENTITY(50000, 1),
[ID_Informe] [int] NOT NULL,
[Codigo] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Observaciones] [nvarchar] (500) COLLATE Modern_Spanish_CI_AS NULL,
[RO] [bit] NOT NULL CONSTRAINT [DF_Informe_Apartado_RO] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Informe_Apartado] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Informe_Apartado]([ID_Informe_Apartado], [ID_Informe], [Codigo], [Descripcion], [Observaciones], [RO]) SELECT [ID_Informe_Apartado], [ID_Informe], [Codigo], [Descripcion], [Observaciones], [RO] FROM [dbo].[Informe_Apartado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Informe_Apartado] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Informe_Apartado]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Informe_Apartado]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Informe_Apartado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Informe_Apartado]', N'Informe_Apartado'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Informe_Apartado] on [dbo].[Informe_Apartado]'
GO
ALTER TABLE [dbo].[Informe_Apartado] ADD CONSTRAINT [PK_Informe_Apartado] PRIMARY KEY CLUSTERED  ([ID_Informe_Apartado])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[GaugeAgrupacion_Gauge]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_GaugeAgrupacion_Gauge]
(
[ID_GaugeAgrupacion_Gauge] [int] NOT NULL IDENTITY(50000, 1),
[ID_Gauge] [int] NOT NULL,
[ID_GaugeAgrupacion] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_GaugeAgrupacion_Gauge] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_GaugeAgrupacion_Gauge]([ID_GaugeAgrupacion_Gauge], [ID_Gauge], [ID_GaugeAgrupacion]) SELECT [ID_GaugeAgrupacion_Gauge], [ID_Gauge], [ID_GaugeAgrupacion] FROM [dbo].[GaugeAgrupacion_Gauge]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_GaugeAgrupacion_Gauge] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[GaugeAgrupacion_Gauge]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_GaugeAgrupacion_Gauge]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[GaugeAgrupacion_Gauge]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_GaugeAgrupacion_Gauge]', N'GaugeAgrupacion_Gauge'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_GaugeAgrupacion_Gaute] on [dbo].[GaugeAgrupacion_Gauge]'
GO
ALTER TABLE [dbo].[GaugeAgrupacion_Gauge] ADD CONSTRAINT [PK_GaugeAgrupacion_Gaute] PRIMARY KEY CLUSTERED  ([ID_GaugeAgrupacion_Gauge])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Listado_Entidad]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Listado_Entidad]
(
[ID_Listado_Entidad] [int] NOT NULL IDENTITY(50000, 1),
[Descripcion] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
[NombreTabla] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
[LlavePrimariaParaAperturaFormulario] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Listado_Entidad] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Listado_Entidad]([ID_Listado_Entidad], [Descripcion], [NombreTabla], [LlavePrimariaParaAperturaFormulario]) SELECT [ID_Listado_Entidad], [Descripcion], [NombreTabla], [LlavePrimariaParaAperturaFormulario] FROM [dbo].[Listado_Entidad]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Listado_Entidad] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Listado_Entidad]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Listado_Entidad]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Listado_Entidad]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Listado_Entidad]', N'Listado_Entidad'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Listado_Entidad] on [dbo].[Listado_Entidad]'
GO
ALTER TABLE [dbo].[Listado_Entidad] ADD CONSTRAINT [PK_Listado_Entidad] PRIMARY KEY CLUSTERED  ([ID_Listado_Entidad])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[ListadoADV_Agrupacion]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ListadoADV_Agrupacion]
(
[ID_ListadoADV_Agrupacion] [int] NOT NULL IDENTITY(50000, 1),
[Codigo] [int] NOT NULL,
[Descripcion] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ListadoADV_Agrupacion] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_ListadoADV_Agrupacion]([ID_ListadoADV_Agrupacion], [Codigo], [Descripcion]) SELECT [ID_ListadoADV_Agrupacion], [Codigo], [Descripcion] FROM [dbo].[ListadoADV_Agrupacion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ListadoADV_Agrupacion] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[ListadoADV_Agrupacion]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_ListadoADV_Agrupacion]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[ListadoADV_Agrupacion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ListadoADV_Agrupacion]', N'ListadoADV_Agrupacion'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Listados_Agrupacion] on [dbo].[ListadoADV_Agrupacion]'
GO
ALTER TABLE [dbo].[ListadoADV_Agrupacion] ADD CONSTRAINT [PK_Listados_Agrupacion] PRIMARY KEY CLUSTERED  ([ID_ListadoADV_Agrupacion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[BI_Usuario_ChangeTracking] on [dbo].[BI_Usuario]'
GO
create trigger [dbo].[BI_Usuario_ChangeTracking] on [dbo].[BI_Usuario] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'BI_Usuario'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Listado_Entidad_ChangeTracking] on [dbo].[Listado_Entidad]'
GO
create trigger [dbo].[Listado_Entidad_ChangeTracking] on [dbo].[Listado_Entidad] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Listado_Entidad'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[FormaPago_Tipo_ChangeTracking] on [dbo].[FormaPago_Tipo]'
GO
create trigger [dbo].[FormaPago_Tipo_ChangeTracking] on [dbo].[FormaPago_Tipo] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'FormaPago_Tipo'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Informe_Plantilla_Apartado_Version_ChangeTracking] on [dbo].[Informe_Plantilla_Apartado_Version]'
GO
create trigger [dbo].[Informe_Plantilla_Apartado_Version_ChangeTracking] on [dbo].[Informe_Plantilla_Apartado_Version] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Informe_Plantilla_Apartado_Version'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Informe_Plantilla_ChangeTracking] on [dbo].[Informe_Plantilla]'
GO
create trigger [dbo].[Informe_Plantilla_ChangeTracking] on [dbo].[Informe_Plantilla] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Informe_Plantilla'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Gauge_ChangeTracking] on [dbo].[Gauge]'
GO
create trigger [dbo].[Gauge_ChangeTracking] on [dbo].[Gauge] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Gauge'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Informe_Apartado_ChangeTracking] on [dbo].[Informe_Apartado]'
GO
create trigger [dbo].[Informe_Apartado_ChangeTracking] on [dbo].[Informe_Apartado] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Informe_Apartado'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Formulario_Usuario_Grupo_ChangeTracking] on [dbo].[Formulario_Usuario_Grupo]'
GO
create trigger [dbo].[Formulario_Usuario_Grupo_ChangeTracking] on [dbo].[Formulario_Usuario_Grupo] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Formulario_Usuario_Grupo'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Conexiones_ChangeTracking] on [dbo].[Conexiones]'
GO
create trigger [dbo].[Conexiones_ChangeTracking] on [dbo].[Conexiones] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Conexiones'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[ListadoADV_ChangeTracking] on [dbo].[ListadoADV]'
GO
create trigger [dbo].[ListadoADV_ChangeTracking] on [dbo].[ListadoADV] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'ListadoADV'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Informe_Apartado_Version_ChangeTracking] on [dbo].[Informe_Apartado_Version]'
GO
create trigger [dbo].[Informe_Apartado_Version_ChangeTracking] on [dbo].[Informe_Apartado_Version] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Informe_Apartado_Version'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Usuario_ChangeTracking] on [dbo].[Usuario]'
GO
create trigger [dbo].[Usuario_ChangeTracking] on [dbo].[Usuario] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Usuario'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[GaugeAgrupacion_Gauge_ChangeTracking] on [dbo].[GaugeAgrupacion_Gauge]'
GO
create trigger [dbo].[GaugeAgrupacion_Gauge_ChangeTracking] on [dbo].[GaugeAgrupacion_Gauge] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'GaugeAgrupacion_Gauge'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Listado_ChangeTracking] on [dbo].[Listado]'
GO
create trigger [dbo].[Listado_ChangeTracking] on [dbo].[Listado] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Listado'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Usuario_Grupo_ChangeTracking] on [dbo].[Usuario_Grupo]'
GO
create trigger [dbo].[Usuario_Grupo_ChangeTracking] on [dbo].[Usuario_Grupo] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Usuario_Grupo'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[GaugeAgrupacion_ChangeTracking] on [dbo].[GaugeAgrupacion]'
GO
create trigger [dbo].[GaugeAgrupacion_ChangeTracking] on [dbo].[GaugeAgrupacion] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'GaugeAgrupacion'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[BI_ChangeTracking] on [dbo].[BI]'
GO
create trigger [dbo].[BI_ChangeTracking] on [dbo].[BI] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'BI'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[ListadoADV_Agrupacion_ChangeTracking] on [dbo].[ListadoADV_Agrupacion]'
GO
create trigger [dbo].[ListadoADV_Agrupacion_ChangeTracking] on [dbo].[ListadoADV_Agrupacion] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'ListadoADV_Agrupacion'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[FORM_Controls_BD]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_FORM_Controls_BD]
(
[ID_FORM_Controls_BD] [int] NOT NULL IDENTITY(50000, 1),
[Formulario] [nvarchar] (75) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Objeto] [nvarchar] (75) COLLATE Modern_Spanish_CI_AS NOT NULL,
[NombreTabla] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[NombreCampo] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[TipoDeDato] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FORM_Controls_BD] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_FORM_Controls_BD]([ID_FORM_Controls_BD], [Formulario], [Objeto], [NombreTabla], [NombreCampo], [TipoDeDato]) SELECT [ID_FORM_Controls_BD], [Formulario], [Objeto], [NombreTabla], [NombreCampo], [TipoDeDato] FROM [dbo].[FORM_Controls_BD]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_FORM_Controls_BD] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[FORM_Controls_BD]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_FORM_Controls_BD]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[FORM_Controls_BD]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_FORM_Controls_BD]', N'FORM_Controls_BD'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_FORM_Controls_BD] on [dbo].[FORM_Controls_BD]'
GO
ALTER TABLE [dbo].[FORM_Controls_BD] ADD CONSTRAINT [PK_FORM_Controls_BD] PRIMARY KEY CLUSTERED  ([ID_FORM_Controls_BD])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] WITH NOCHECK  ADD CONSTRAINT [FK_Menu_ListadoADV] FOREIGN KEY ([ID_ListadoADV]) REFERENCES [dbo].[ListadoADV] ([ID_ListadoADV])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[BI_Usuario]'
GO
ALTER TABLE [dbo].[BI_Usuario] ADD CONSTRAINT [FK_BI_Usuario_BI] FOREIGN KEY ([ID_BI]) REFERENCES [dbo].[BI] ([ID_BI])
ALTER TABLE [dbo].[BI_Usuario] ADD CONSTRAINT [FK_BI_Usuario_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] ADD CONSTRAINT [FK_Menus_BI] FOREIGN KEY ([ID_BI]) REFERENCES [dbo].[BI] ([ID_BI])
ALTER TABLE [dbo].[Menus] ADD CONSTRAINT [FK_Menu_Gauge] FOREIGN KEY ([ID_GaugeAgrupacion]) REFERENCES [dbo].[Gauge] ([ID_Gauge])
ALTER TABLE [dbo].[Menus] ADD CONSTRAINT [FK_Menu_Listado] FOREIGN KEY ([ID_Listado]) REFERENCES [dbo].[Listado] ([ID_Listado])
ALTER TABLE [dbo].[Menus] ADD CONSTRAINT [FK_Menu_Menu_Tipo] FOREIGN KEY ([ID_Menus_Tipo]) REFERENCES [dbo].[Menus_Tipo] ([ID_Menus_Tipo])
ALTER TABLE [dbo].[Menus] ADD CONSTRAINT [FK_Menu_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[FormaPago]'
GO
ALTER TABLE [dbo].[FormaPago] ADD CONSTRAINT [FK_FormaPago_FormaPago_Tipo] FOREIGN KEY ([ID_FormaPago_Tipo]) REFERENCES [dbo].[FormaPago_Tipo] ([ID_FormaPago_Tipo])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Formulario_Usuario_Grupo]'
GO
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] ADD CONSTRAINT [FK_Formulario_Usuario_Grupo_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])
ALTER TABLE [dbo].[Formulario_Usuario_Grupo] ADD CONSTRAINT [FK_Formulario_Usuario_Grupo_Usuario_Grupo] FOREIGN KEY ([ID_Usuario_Grupo]) REFERENCES [dbo].[Usuario_Grupo] ([ID_Usuario_Grupo])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[GaugeAgrupacion_Gauge]'
GO
ALTER TABLE [dbo].[GaugeAgrupacion_Gauge] ADD CONSTRAINT [FK_GaugeAgrupacion_Gauge_Gauge] FOREIGN KEY ([ID_Gauge]) REFERENCES [dbo].[Gauge] ([ID_Gauge])
ALTER TABLE [dbo].[GaugeAgrupacion_Gauge] ADD CONSTRAINT [FK_GaugeAgrupacion_Gauge_GaugeAgrupacion] FOREIGN KEY ([ID_GaugeAgrupacion]) REFERENCES [dbo].[GaugeAgrupacion] ([ID_GaugeAgrupacion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Gauge]'
GO
ALTER TABLE [dbo].[Gauge] ADD CONSTRAINT [FK_Gauge_ListadoADV] FOREIGN KEY ([ID_ListadoADV]) REFERENCES [dbo].[ListadoADV] ([ID_ListadoADV])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Entrada_Vencimiento]'
GO
ALTER TABLE [dbo].[Entrada_Vencimiento] ADD CONSTRAINT [FK_Entrada_Vencimiento_Entrada_Vencimiento_Estado] FOREIGN KEY ([ID_Entrada_Vencimiento_Estado]) REFERENCES [dbo].[Entrada_Vencimiento_Estado] ([ID_Entrada_Vencimiento_Estado])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Informe_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Apartado_Version] ADD CONSTRAINT [FK_Informe_Apartado_Version_Informe_Apartado1] FOREIGN KEY ([ID_Informe_Apartado]) REFERENCES [dbo].[Informe_Apartado] ([ID_Informe_Apartado])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Informe_Plantilla_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] ADD CONSTRAINT [FK_Informe_Plantilla_Apartado_Version_Informe_Apartado] FOREIGN KEY ([ID_Informe_Apartado]) REFERENCES [dbo].[Informe_Apartado] ([ID_Informe_Apartado])
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] ADD CONSTRAINT [FK_Informe_Plantilla_Apartado_Version_Informe_Apartado_Version] FOREIGN KEY ([ID_Informe_Apartado_Version]) REFERENCES [dbo].[Informe_Apartado_Version] ([ID_Informe_Apartado_Version])
ALTER TABLE [dbo].[Informe_Plantilla_Apartado_Version] ADD CONSTRAINT [FK_Informe_Plantilla_Apartado_Version_Informe_Plantilla] FOREIGN KEY ([ID_Informe_Plantilla]) REFERENCES [dbo].[Informe_Plantilla] ([ID_Informe_Plantilla])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Informe_Apartado]'
GO
ALTER TABLE [dbo].[Informe_Apartado] ADD CONSTRAINT [FK_Informe_Apartado_Informe] FOREIGN KEY ([ID_Informe]) REFERENCES [dbo].[Informe] ([ID_Informe])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Informe_Plantilla]'
GO
ALTER TABLE [dbo].[Informe_Plantilla] ADD CONSTRAINT [FK_Informe_Plantilla_Informe] FOREIGN KEY ([ID_Informe]) REFERENCES [dbo].[Informe] ([ID_Informe])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Listado]'
GO
ALTER TABLE [dbo].[Listado] ADD CONSTRAINT [FK_Listado_Listado_Entidad] FOREIGN KEY ([ID_Listado_Entidad]) REFERENCES [dbo].[Listado_Entidad] ([ID_Listado_Entidad])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] ADD CONSTRAINT [FK_ListadoADV_ListadoADV_Agrupacion] FOREIGN KEY ([ID_ListadoADV_Agrupacion]) REFERENCES [dbo].[ListadoADV_Agrupacion] ([ID_ListadoADV_Agrupacion])
ALTER TABLE [dbo].[ListadoADV] ADD CONSTRAINT [FK_ListadoADV_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Campaña_Cliente_Seguimiento]'
GO
ALTER TABLE [dbo].[Campaña_Cliente_Seguimiento] ADD CONSTRAINT [FK_Campaña_Cliente_Seguimiento_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Campaña_Usuario]'
GO
ALTER TABLE [dbo].[Campaña_Usuario] ADD CONSTRAINT [FK_Campaña_Usuario_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Cliente_Seguridad]'
GO
ALTER TABLE [dbo].[Cliente_Seguridad] ADD CONSTRAINT [FK_Cliente_Seguridad_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Instalacion_Seguridad]'
GO
ALTER TABLE [dbo].[Instalacion_Seguridad] ADD CONSTRAINT [FK_Instalacion_Seguridad_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Log_Sesiones]'
GO
ALTER TABLE [dbo].[Log_Sesiones] ADD CONSTRAINT [FK_Log_Sesiones_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Notificacion]'
GO
ALTER TABLE [dbo].[Notificacion] ADD CONSTRAINT [FK_Notificacion_Usuario2] FOREIGN KEY ([ID_Usuario_Origen]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
ALTER TABLE [dbo].[Notificacion] ADD CONSTRAINT [FK_Notificacion_Usuario3] FOREIGN KEY ([ID_Usuario_Destino]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Notificacion_Automatica_Usuario]'
GO
ALTER TABLE [dbo].[Notificacion_Automatica_Usuario] ADD CONSTRAINT [FK_Notififcacion_Automatica_Usuario_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Parte_ToDo]'
GO
ALTER TABLE [dbo].[Parte_ToDo] ADD CONSTRAINT [FK_Parte_ToDo_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Personal_Seguridad]'
GO
ALTER TABLE [dbo].[Personal_Seguridad] ADD CONSTRAINT [FK_Personal_Seguridad_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_PropuestaEspecificacion]'
GO
ALTER TABLE [dbo].[Propuesta_PropuestaEspecificacion] ADD CONSTRAINT [FK_Propuesta_PropuestaCuestionario_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_Seguridad]'
GO
ALTER TABLE [dbo].[Propuesta_Seguridad] ADD CONSTRAINT [FK_Propuesta_Seguridad_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Proveedor_Seguridad]'
GO
ALTER TABLE [dbo].[Proveedor_Seguridad] ADD CONSTRAINT [FK_Proveedor_Seguridad_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Usuario]'
GO
ALTER TABLE [dbo].[Usuario] ADD CONSTRAINT [FK_Usuario_Usuario_Grupo] FOREIGN KEY ([ID_Usuario_Grupo]) REFERENCES [dbo].[Usuario_Grupo] ([ID_Usuario_Grupo])
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
