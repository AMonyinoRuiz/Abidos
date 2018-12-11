/*
Run this script on:

        Server2012R2\SQLServer2012.AbidosMestre    -  This database will be modified

to synchronize it with:

        Server2012R2\SQLServer2012.AbidosDomingo

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 11/04/2015 1:00:49

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
PRINT N'Dropping extended properties'
GO
EXEC sp_dropextendedproperty N'MS_DiagramPane2', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[ActividadCRM_Accion]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD
[Solucion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[ActividadCRM]'
GO
EXEC sp_rename N'[dbo].[ActividadCRM].[Realizado]', N'Finalizada', 'COLUMN'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[ActividadCRM].[PorcentajeRealizado]', N'PorcentajeFinalizada', 'COLUMN'
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
                         dbo.ActividadCRM_Accion.ID_Personal, dbo.ActividadCRM_Accion.Descripcion, dbo.ActividadCRM_Accion.FechaAlta, dbo.ActividadCRM_Accion.FechaAviso, 
                         dbo.ActividadCRM_Accion.HoraAviso, dbo.ActividadCRM_Accion.Finalizada, dbo.ActividadCRM_Accion_Tipo.Descripcion AS TipoAccion, 
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
                               WHERE        ID_ActividadCRM_Archivo = ActividadCRM.ID_ActividadCRM) WHEN 0 THEN 0 ELSE 1 END AS TieneFichero
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
PRINT N'Altering [dbo].[ActividadCRM_Personal]'
GO
ALTER TABLE [dbo].[ActividadCRM_Personal] ADD
[Finalizado] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Personal_Finalizado] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_ActividadCRM_PersonalAsignadoOPropietario]'
GO
CREATE VIEW dbo.C_ActividadCRM_PersonalAsignadoOPropietario
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
PRINT N'Creating [dbo].[C_ActividadCRM_Chat]'
GO
CREATE VIEW dbo.C_ActividadCRM_Chat
AS
SELECT        dbo.ActividadCRM_Chat.ID_ActividadCRM_Chat, dbo.ActividadCRM.ID_ActividadCRM, dbo.Personal.Nombre AS De, Personal_1.Nombre AS Para, 
                         dbo.ActividadCRM_Chat.FechaAlta, dbo.ActividadCRM_Chat.Mensaje, dbo.ActividadCRM.Asunto, dbo.ActividadCRM.Finalizada, 
                         dbo.ActividadCRM_Chat.ID_Personal_Origen, dbo.ActividadCRM_Chat.ID_Personal_Destino
FROM            dbo.ActividadCRM_Chat INNER JOIN
                         dbo.ActividadCRM ON dbo.ActividadCRM_Chat.ID_ActividadCRM = dbo.ActividadCRM.ID_ActividadCRM INNER JOIN
                         dbo.Personal ON dbo.ActividadCRM_Chat.ID_Personal_Origen = dbo.Personal.ID_Personal INNER JOIN
                         dbo.Personal AS Personal_1 ON dbo.ActividadCRM_Chat.ID_Personal_Destino = Personal_1.ID_Personal
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[MailPool]'
GO
CREATE TABLE [dbo].[MailPool]
(
[ID_MailPool] [int] NOT NULL IDENTITY(1, 1),
[ID_Personal_Origen] [int] NOT NULL,
[ID_Personal_Destino] [int] NOT NULL,
[ID_ActividadCRM] [int] NULL,
[ID_ActividadCRM_Accion] [int] NULL,
[De] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Para] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Asunto] [nvarchar] (500) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Mensaje] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Fecha] [smalldatetime] NOT NULL,
[Enviado] [bit] NOT NULL CONSTRAINT [DF_MailPool_Enviado] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_MailPool] on [dbo].[MailPool]'
GO
ALTER TABLE [dbo].[MailPool] ADD CONSTRAINT [PK_MailPool] PRIMARY KEY CLUSTERED  ([ID_MailPool])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[ActividadCRM_Accion_Aux]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Aux] ADD
[SolucionRTF] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Personal_Emails]'
GO
CREATE TABLE [dbo].[Personal_Emails]
(
[ID_Personal_Emails] [int] NOT NULL IDENTITY(1, 1),
[ID_Personal] [int] NOT NULL,
[Email] [nvarchar] (150) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Personal_Emails] on [dbo].[Personal_Emails]'
GO
ALTER TABLE [dbo].[Personal_Emails] ADD CONSTRAINT [PK_Personal_Emails] PRIMARY KEY CLUSTERED  ([ID_Personal_Emails])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Personal_Emails]'
GO
ALTER TABLE [dbo].[Personal_Emails] ADD CONSTRAINT [FK_Personal_Emails_Personal] FOREIGN KEY ([ID_Personal]) REFERENCES [dbo].[Personal] ([ID_Personal])
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
         Configuration = "(H (1[35] 4[29] 2[20] 3) )"
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
         Top = -96
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 29
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
DECLARE @xp int
SELECT @xp=1
EXEC sp_updateextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM', NULL, NULL
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_PersonalAsignadoOPropietario', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_PersonalAsignadoOPropietario', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[38] 4[23] 2[20] 3) )"
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
               Left = 969
               Bottom = 371
               Right = 1179
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
         Or =', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Chat', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N' 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Chat', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Chat', NULL, NULL
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