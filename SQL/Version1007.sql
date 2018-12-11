/*
Run this script on:

        Server2012R2\SQLServer2012.AbidosMestre    -  This database will be modified

to synchronize it with:

        Server2012R2\SQLServer2012.AbidosDomingo

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 15/04/2015 16:41:44

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
PRINT N'Dropping foreign keys from [dbo].[ActividadCRM_Accion_Archivo]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Archivo] DROP CONSTRAINT [FK_ActividadCRM_Accion_Archivo_ActividadCRM_Accion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[ActividadCRM_Accion_Aux]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Aux] DROP CONSTRAINT [FK_ActividadCRM_Accion_Aux_ActividadCRM_Accion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[ActividadCRM_Accion_Personal]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Personal] DROP CONSTRAINT [FK_ActividadCRM_Accion_Personal_ActividadCRM_Accion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Aviso]'
GO
ALTER TABLE [dbo].[Aviso] DROP CONSTRAINT [FK_Aviso_ActividadCRM_Accion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[ActividadCRM_Accion]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion] DROP CONSTRAINT [FK_ActividadCRM_Accion_ActividadCRM]
ALTER TABLE [dbo].[ActividadCRM_Accion] DROP CONSTRAINT [FK_ActividadCRM_Accion_ActividadCRM_Accion_Tipo]
ALTER TABLE [dbo].[ActividadCRM_Accion] DROP CONSTRAINT [FK_ActividadCRM_Accion_Personal]
ALTER TABLE [dbo].[ActividadCRM_Accion] DROP CONSTRAINT [FK_ActividadCRM_Accion_Automatismo_Accion]
ALTER TABLE [dbo].[ActividadCRM_Accion] DROP CONSTRAINT [FK_ActividadCRM_Accion_Prioridad]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[ActividadCRM_Accion]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion] DROP CONSTRAINT [PK_ActividadCRM_Accion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[ActividadCRM_Accion]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ActividadCRM_Accion]
(
[ID_ActividadCRM_Accion] [int] NOT NULL IDENTITY(1, 1),
[ID_ActividadCRM] [int] NOT NULL,
[ID_ActividadCRM_Accion_Tipo] [int] NOT NULL,
[ID_Personal] [int] NOT NULL,
[ID_Automatismo_Accion] [int] NULL,
[ID_Prioridad] [int] NOT NULL,
[Descripcion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[Solucion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[FechaAlta] [smalldatetime] NULL,
[AvisoFecha] [smalldatetime] NULL,
[AvisoHora] [smalldatetime] NULL,
[Finalizada] [bit] NULL,
[NotificadoAviso] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Accion_NotificadoAviso] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ActividadCRM_Accion] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_ActividadCRM_Accion]([ID_ActividadCRM_Accion], [ID_ActividadCRM], [ID_ActividadCRM_Accion_Tipo], [ID_Personal], [ID_Automatismo_Accion], [ID_Prioridad], [Descripcion], [Solucion], [FechaAlta], [AvisoFecha], [AvisoHora], [Finalizada]) SELECT [ID_ActividadCRM_Accion], [ID_ActividadCRM], [ID_ActividadCRM_Accion_Tipo], [ID_Personal], [ID_Automatismo_Accion], [ID_Prioridad], [Descripcion], [Solucion], [FechaAlta], [FechaAviso], [HoraAviso], [Finalizada] FROM [dbo].[ActividadCRM_Accion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ActividadCRM_Accion] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[ActividadCRM_Accion]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_ActividadCRM_Accion]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[ActividadCRM_Accion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ActividadCRM_Accion]', N'ActividadCRM_Accion'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Accion] on [dbo].[ActividadCRM_Accion]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [PK_ActividadCRM_Accion] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[ActividadCRM]'
GO
ALTER TABLE [dbo].[ActividadCRM] ADD
[AvisoFecha] [smalldatetime] NULL,
[AvisoHora] [smalldatetime] NULL,
[Solucion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[ALaEsperaRespuesta] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_ALaEsperaRespuesta] DEFAULT ((0)),
[SoloSeguimiento] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_SoloSeguimiento] DEFAULT ((0)),
[NotificadoVencimiento] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_AvisadoVencimiento] DEFAULT ((0)),
[NotificadoAviso] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_NotificadoAviso] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[ActividadCRM] ALTER COLUMN [Asunto] [nvarchar] (4000) COLLATE Modern_Spanish_CI_AS NOT NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_ActividadCRM_Acciones]'
GO
ALTER VIEW dbo.C_ActividadCRM_Acciones
AS
SELECT        dbo.ActividadCRM_Accion.ID_ActividadCRM_Accion, dbo.ActividadCRM_Accion.ID_ActividadCRM, dbo.ActividadCRM_Accion.ID_ActividadCRM_Accion_Tipo, 
                         dbo.ActividadCRM_Accion.ID_Personal, dbo.ActividadCRM_Accion.Descripcion, dbo.ActividadCRM_Accion.FechaAlta, dbo.ActividadCRM_Accion.AvisoFecha, 
                         dbo.ActividadCRM_Accion.AvisoHora, dbo.ActividadCRM_Accion.Finalizada, dbo.ActividadCRM_Accion_Tipo.Descripcion AS TipoAccion, 
                         dbo.Personal.Nombre AS Propietario, dbo.Prioridad.Descripcion AS Prioridad, dbo.ActividadCRM_Accion.ID_Prioridad, 
                         dbo.ActividadCRM_Accion.ID_Automatismo_Accion, dbo.Automatismo_Accion.Descripcion AS [Acción del automatismo], dbo.ActividadCRM_Accion.Solucion
FROM            dbo.ActividadCRM_Accion INNER JOIN
                         dbo.ActividadCRM_Accion_Tipo ON 
                         dbo.ActividadCRM_Accion.ID_ActividadCRM_Accion_Tipo = dbo.ActividadCRM_Accion_Tipo.ID_ActividadCRM_Accion_Tipo INNER JOIN
                         dbo.ActividadCRM ON dbo.ActividadCRM_Accion.ID_ActividadCRM = dbo.ActividadCRM.ID_ActividadCRM INNER JOIN
                         dbo.ActividadCRM_Tipo ON dbo.ActividadCRM.ID_ActividadCRM_Tipo = dbo.ActividadCRM_Tipo.ID_ActividadCRM_Tipo INNER JOIN
                         dbo.Personal ON dbo.ActividadCRM_Accion.ID_Personal = dbo.Personal.ID_Personal INNER JOIN
                         dbo.Prioridad ON dbo.ActividadCRM_Accion.ID_Prioridad = dbo.Prioridad.ID_Prioridad LEFT OUTER JOIN
                         dbo.Automatismo_Accion ON dbo.ActividadCRM_Accion.ID_Automatismo_Accion = dbo.Automatismo_Accion.ID_Automatismo_Accion
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Cliente]'
GO
ALTER VIEW dbo.C_Cliente
AS
SELECT        dbo.Cliente.ID_Cliente, dbo.Cliente.ID_Cliente_Origen, dbo.Cliente.Codigo, dbo.Cliente.Nombre, dbo.Cliente.NombreComercial, dbo.Cliente.NIF, 
                         dbo.Cliente.PersonaContacto, dbo.Cliente.Email, dbo.Cliente.Telefono, dbo.Cliente.Fax, dbo.Cliente.Direccion, dbo.Cliente.Poblacion, dbo.Cliente.Provincia, 
                         dbo.Cliente.FechaAlta, dbo.Cliente.FechaBaja, dbo.Cliente.Observaciones, dbo.Cliente.Activo, dbo.Cliente_Origen.Descripcion AS ClienteOrigen, 
                         dbo.Cliente.ID_Cliente_Tipo, dbo.Cliente.ID_FormaPago, dbo.Cliente_Tipo.Descripcion AS [Tipo de cliente]
FROM            dbo.Cliente INNER JOIN
                         dbo.Cliente_Tipo ON dbo.Cliente.ID_Cliente_Tipo = dbo.Cliente_Tipo.ID_Cliente_Tipo LEFT OUTER JOIN
                         dbo.Cliente_Origen ON dbo.Cliente.ID_Cliente_Origen = dbo.Cliente_Origen.ID_Cliente_Origen
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_ActividadCRM]'
GO
ALTER VIEW dbo.C_ActividadCRM
AS
SELECT        dbo.ActividadCRM.ID_ActividadCRM, dbo.Cliente.ID_Cliente, dbo.ActividadCRM.ID_Instalacion, dbo.ActividadCRM.ID_Propuesta, 
                         dbo.ActividadCRM.ID_ActividadCRM_Tipo, dbo.ActividadCRM.FechaAlta, dbo.ActividadCRM.FechaVencimiento, dbo.ActividadCRM.Finalizada, 
                         dbo.ActividadCRM.Asunto, dbo.ActividadCRM.Activo, dbo.Propuesta.Codigo AS PropuestaCodigo, dbo.Propuesta.Version AS PropuestaVersion, 
                         dbo.Propuesta.Descripcion AS PropuestaDescripcion, dbo.ActividadCRM_Tipo.Descripcion AS ActividadTipo, dbo.Cliente.Codigo AS ClienteCodigo, 
                         dbo.Cliente.Nombre AS ClienteNombre, dbo.Cliente.NombreComercial AS ClienteNombreComercial, dbo.Personal.Nombre AS Propietario, 
                         dbo.ActividadCRM.ID_Personal, dbo.ActividadCRM.Descripcion, dbo.ActividadCRM.PorcentajeFinalizada, dbo.ActividadCRM.ID_Automatismo, 
                         dbo.ActividadCRM.ID_Prioridad, dbo.Automatismo.Descripcion AS Automatismo, dbo.Prioridad.Descripcion AS Prioridad, CASE
                             (SELECT        Count(*)
                               FROM            ActividadCRM_Archivo
                               WHERE        ID_ActividadCRM_Archivo = ActividadCRM.ID_ActividadCRM) WHEN 0 THEN 0 ELSE 1 END AS TieneFichero, ActividadCRM.Solucion, ActividadCRM.ALaEsperaRespuesta, ActividadCRM.SoloSeguimiento
FROM            dbo.Cliente RIGHT OUTER JOIN
                         dbo.ActividadCRM INNER JOIN
                         dbo.ActividadCRM_Tipo ON dbo.ActividadCRM.ID_ActividadCRM_Tipo = dbo.ActividadCRM_Tipo.ID_ActividadCRM_Tipo INNER JOIN
                         dbo.Personal ON dbo.ActividadCRM.ID_Personal = dbo.Personal.ID_Personal INNER JOIN
                         dbo.Prioridad ON dbo.ActividadCRM.ID_Prioridad = dbo.Prioridad.ID_Prioridad ON dbo.Cliente.ID_Cliente = dbo.ActividadCRM.ID_Cliente LEFT OUTER JOIN
                         dbo.Automatismo ON dbo.ActividadCRM.ID_Automatismo = dbo.Automatismo.ID_Automatismo LEFT OUTER JOIN
                         dbo.Instalacion ON dbo.ActividadCRM.ID_Instalacion = dbo.Instalacion.ID_Instalacion LEFT OUTER JOIN
                         dbo.Propuesta ON dbo.ActividadCRM.ID_Propuesta = dbo.Propuesta.ID_Propuesta
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_ActividadCRM_PersonalAsignadoOPropietario2]'
GO
CREATE VIEW [dbo].[C_ActividadCRM_PersonalAsignadoOPropietario2]
AS
SELECT        TOP (100) PERCENT ID_ActividadCRM, ID_Personal, ISNULL
                             ((SELECT        Finalizado
                                 FROM            dbo.ActividadCRM_Personal AS A
                                 WHERE        (ID_Personal = DondeParticipo.ID_Personal) AND (ID_ActividadCRM = DondeParticipo.ID_ActividadCRM)), 0) AS Finalizado
FROM            (SELECT        ID_ActividadCRM, ID_Personal
                          FROM            dbo.ActividadCRM
                          WHERE        (Activo = 1)
                          UNION ALL
                          SELECT        ID_ActividadCRM, ID_Personal
                          FROM            dbo.ActividadCRM_Accion
                          UNION ALL
                          SELECT        ID_ActividadCRM, ID_Personal
                          FROM            dbo.ActividadCRM_Personal
                          UNION ALL
                          SELECT        ActividadCRM_Accion_1.ID_ActividadCRM, dbo.ActividadCRM_Accion_Personal.ID_Personal
                          FROM            dbo.ActividadCRM_Accion_Personal INNER JOIN
                                                   dbo.ActividadCRM_Accion AS ActividadCRM_Accion_1 ON 
                                                   dbo.ActividadCRM_Accion_Personal.ID_ActividadCRM_Accion = ActividadCRM_Accion_1.ID_ActividadCRM_Accion) AS DondeParticipo
GROUP BY ID_ActividadCRM, ID_Personal
ORDER BY ID_Personal
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[ActividadCRM_Chat]'
GO
ALTER TABLE [dbo].[ActividadCRM_Chat] ADD
[Leido] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Chat_Leido] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[ActividadCRM_Chat] ALTER COLUMN [Mensaje] [nvarchar] (4000) COLLATE Modern_Spanish_CI_AS NOT NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_ActividadCRM_Chat]'
GO
ALTER VIEW dbo.C_ActividadCRM_Chat
AS
SELECT        dbo.ActividadCRM_Chat.ID_ActividadCRM_Chat, dbo.ActividadCRM.ID_ActividadCRM, dbo.Personal.Nombre AS De, Personal_1.Nombre AS Para, 
                         dbo.ActividadCRM_Chat.FechaAlta, dbo.ActividadCRM_Chat.Mensaje, dbo.ActividadCRM.Asunto, dbo.ActividadCRM.Finalizada, 
                         dbo.ActividadCRM_Chat.ID_Personal_Origen, dbo.ActividadCRM_Chat.ID_Personal_Destino, dbo.ActividadCRM.Activo, dbo.ActividadCRM_Chat.Leido, 
                         dbo.Cliente.Nombre AS ClienteNombre
FROM            dbo.ActividadCRM_Chat INNER JOIN
                         dbo.ActividadCRM ON dbo.ActividadCRM_Chat.ID_ActividadCRM = dbo.ActividadCRM.ID_ActividadCRM INNER JOIN
                         dbo.Personal ON dbo.ActividadCRM_Chat.ID_Personal_Origen = dbo.Personal.ID_Personal INNER JOIN
                         dbo.Personal AS Personal_1 ON dbo.ActividadCRM_Chat.ID_Personal_Destino = Personal_1.ID_Personal LEFT OUTER JOIN
                         dbo.Cliente ON dbo.ActividadCRM.ID_Cliente = dbo.Cliente.ID_Cliente
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_ActividadCRM_PersonalAsignadoOPropietario]'
GO
ALTER VIEW dbo.C_ActividadCRM_PersonalAsignadoOPropietario
AS
SELECT        dbo.C_ActividadCRM_PersonalAsignadoOPropietario2.ID_ActividadCRM, dbo.C_ActividadCRM_PersonalAsignadoOPropietario2.ID_Personal, 
                         dbo.C_ActividadCRM_PersonalAsignadoOPropietario2.Finalizado, dbo.ActividadCRM.SoloSeguimiento, dbo.ActividadCRM.ALaEsperaRespuesta, 
                         CASE WHEN ActividadCRM.ID_PErsonal = C_ActividadCRM_PersonalAsignadoOPropietario2.ID_PErsonal AND 
                         SoloSeguimiento = 1 THEN 1 ELSE 0 END AS EsSoloSeguimiento, 
                         CASE WHEN ActividadCRM.ID_PErsonal = C_ActividadCRM_PersonalAsignadoOPropietario2.ID_PErsonal AND 
                         ALaEsperaRespuesta = 1 THEN 1 ELSE 0 END AS EsALaEsperaRespuesta
FROM            dbo.C_ActividadCRM_PersonalAsignadoOPropietario2 INNER JOIN
                         dbo.ActividadCRM ON dbo.C_ActividadCRM_PersonalAsignadoOPropietario2.ID_ActividadCRM = dbo.ActividadCRM.ID_ActividadCRM
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[ActividadCRM_Aux]'
GO
ALTER TABLE [dbo].[ActividadCRM_Aux] ADD
[SolucionRTF] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Accion_Archivo]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Archivo] ADD CONSTRAINT [FK_ActividadCRM_Accion_Archivo_ActividadCRM_Accion] FOREIGN KEY ([ID_ActividadCRM_Accion_Archivo]) REFERENCES [dbo].[ActividadCRM_Accion] ([ID_ActividadCRM_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Accion_Aux]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Aux] ADD CONSTRAINT [FK_ActividadCRM_Accion_Aux_ActividadCRM_Accion] FOREIGN KEY ([ID_ActividadCRM_Accion_Aux]) REFERENCES [dbo].[ActividadCRM_Accion] ([ID_ActividadCRM_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Accion_Personal]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Personal] ADD CONSTRAINT [FK_ActividadCRM_Accion_Personal_ActividadCRM_Accion] FOREIGN KEY ([ID_ActividadCRM_Accion]) REFERENCES [dbo].[ActividadCRM_Accion] ([ID_ActividadCRM_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Aviso]'
GO
ALTER TABLE [dbo].[Aviso] ADD CONSTRAINT [FK_Aviso_ActividadCRM_Accion] FOREIGN KEY ([ID_ActividadCRM_Accion]) REFERENCES [dbo].[ActividadCRM_Accion] ([ID_ActividadCRM_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Accion]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_ActividadCRM] FOREIGN KEY ([ID_ActividadCRM]) REFERENCES [dbo].[ActividadCRM] ([ID_ActividadCRM])
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_ActividadCRM_Accion_Tipo] FOREIGN KEY ([ID_ActividadCRM_Accion_Tipo]) REFERENCES [dbo].[ActividadCRM_Accion_Tipo] ([ID_ActividadCRM_Accion_Tipo])
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_Personal] FOREIGN KEY ([ID_Personal]) REFERENCES [dbo].[Personal] ([ID_Personal])
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_Automatismo_Accion] FOREIGN KEY ([ID_Automatismo_Accion]) REFERENCES [dbo].[Automatismo_Accion] ([ID_Automatismo_Accion])
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_Prioridad] FOREIGN KEY ([ID_Prioridad]) REFERENCES [dbo].[Prioridad] ([ID_Prioridad])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering extended properties'
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[52] 4[19] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ActividadCRM"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 392
               Right = 248
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "C_ActividadCRM_PersonalAsignadoOPropietario2"
            Begin Extent = 
               Top = 6
               Left = 286
               Bottom = 118
               Right = 495
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 3945
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 5775
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_PersonalAsignadoOPropietario', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[36] 4[29] 2[22] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 303
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "ActividadCRM"
            Begin Extent = 
               Top = 6
               Left = 341
               Bottom = 135
               Right = 551
            End
            DisplayFlags = 280
            TopColumn = 14
         End
         Begin Table = "ActividadCRM_Tipo"
            Begin Extent = 
               Top = 6
               Left = 589
               Bottom = 135
               Right = 799
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 6
               Left = 837
               Bottom = 135
               Right = 1071
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Prioridad"
            Begin Extent = 
               Top = 6
               Left = 1109
               Bottom = 101
               Right = 1318
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Automatismo"
            Begin Extent = 
               Top = 102
               Left = 1109
               Bottom = 231
               Right = 1319
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 267
               Right = 249
            End
    ', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_updateextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[27] 4[34] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ActividadCRM_Accion"
            Begin Extent = 
               Top = 33
               Left = 568
               Bottom = 304
               Right = 820
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM_Accion_Tipo"
            Begin Extent = 
               Top = 22
               Left = 979
               Bottom = 124
               Right = 1231
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM"
            Begin Extent = 
               Top = 36
               Left = 268
               Bottom = 363
               Right = 478
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM_Tipo"
            Begin Extent = 
               Top = 88
               Left = 21
               Bottom = 217
               Right = 231
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 175
               Left = 951
               Bottom = 304
               Right = 1185
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Prioridad"
            Begin Extent = 
               Top = 348
               Left = 955
               Bottom = 443
               Right = 1164
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Automatismo_Accion"
            Begin Extent = 
               Top = 328
               Left = 119
               Bottom = 500
             ', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Acciones', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'  Right = 371
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 15
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 3645
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Acciones', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 35
               Left = 292
               Bottom = 357
               Right = 490
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente_Origen"
            Begin Extent = 
               Top = 27
               Left = 559
               Bottom = 166
               Right = 757
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente_Tipo"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 115
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4740
         Alias = 2220
         Table = 2385
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_Cliente', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[49] 4[13] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ActividadCRM_Chat"
            Begin Extent = 
               Top = 0
               Left = 495
               Bottom = 205
               Right = 706
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM"
            Begin Extent = 
               Top = 35
               Left = 852
               Bottom = 371
               Right = 1062
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 17
               Left = 90
               Bottom = 179
               Right = 324
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal_1"
            Begin Extent = 
               Top = 197
               Left = 74
               Bottom = 326
               Right = 308
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 103
               Left = 1151
               Bottom = 374
               Right = 1416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 3645
        ', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Chat', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N' Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Chat', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "DondeParticipo"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 118
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 2640
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_PersonalAsignadoOPropietario2', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_PersonalAsignadoOPropietario2', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N'        DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta"
            Begin Extent = 
               Top = 138
               Left = 287
               Bottom = 267
               Right = 574
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 31
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 3960
         Alias = 2430
         Table = 2850
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM', NULL, NULL
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
