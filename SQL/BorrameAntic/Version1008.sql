/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 14/07/2014 0:19:30

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
PRINT N'Dropping foreign keys from [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [FK_ListadoADV_Formulario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Producto]'
GO
ALTER TABLE [dbo].[Producto] DROP CONSTRAINT [FK_Producto_Producto_Garantia]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Propuesta_Linea]'
GO
ALTER TABLE [dbo].[Propuesta_Linea] ADD
[ID_Propuesta_Linea_Vinculado_Energetico] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Entrada_Linea]'
GO
ALTER TABLE [dbo].[Entrada_Linea] ADD
[FechaInicio] [smalldatetime] NULL,
[FechaFin] [smalldatetime] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Propuesta_Linea_Vinculacion_Energetica]'
GO

CREATE VIEW [dbo].[C_Propuesta_Linea_Vinculacion_Energetica]
AS
SELECT        dbo.Propuesta_Linea.ID_Propuesta_Linea, dbo.Propuesta_Linea.ID_Propuesta, dbo.Propuesta.ID_Instalacion, dbo.Propuesta_Linea.Identificador, 
                         dbo.Propuesta_Linea.IdentificadorDelProducto, dbo.Propuesta_Linea.Descripcion, dbo.Instalacion_Emplazamiento.Descripcion AS Emplazamiento, 
                         dbo.Instalacion_Emplazamiento_Planta.Descripcion AS Planta, dbo.Instalacion_Emplazamiento_Zona.Descripcion AS Zona, dbo.Propuesta_Linea.NickZona, 
                         dbo.Producto.PotenciaSalida * dbo.Propuesta_Linea.Unidad AS Expr1, dbo.Producto.PotenciaEntrada * dbo.Propuesta_Linea.Unidad AS Expr2, 
                         ISNULL(dbo.Producto.PotenciaSalida, 0) * dbo.Propuesta_Linea.Unidad - ISNULL
                             ((SELECT        SUM(ISNULL(Prod.PotenciaEntrada, 0) * Linea.Unidad) AS Expr1
                                 FROM            dbo.Propuesta_Linea AS Linea INNER JOIN
                                                          dbo.Producto AS Prod ON Linea.ID_Producto = Prod.ID_Producto
                                 WHERE        (Linea.ID_Propuesta_Linea_Vinculado_Energetico = dbo.Propuesta_Linea.ID_Propuesta_Linea)), 0) AS TotalSalidaRestante,
                             (SELECT        SUM(Unidad) AS Expr1
                               FROM            dbo.Propuesta_Linea AS B
                               WHERE        (ID_Propuesta_Linea_Vinculado_Energetico = dbo.Propuesta_Linea.ID_Propuesta_Linea)) AS NumVinculados, dbo.Propuesta_Linea.Activo, 
                         dbo.Propuesta_Linea.Unidad, dbo.Producto_Familia.Conectable, dbo.Propuesta_Linea.ID_Propuesta_Linea_Vinculado_Energetico
FROM            dbo.Propuesta INNER JOIN
                         dbo.Propuesta_Linea INNER JOIN
                         dbo.Producto ON dbo.Propuesta_Linea.ID_Producto = dbo.Producto.ID_Producto ON dbo.Propuesta.ID_Propuesta = dbo.Propuesta_Linea.ID_Propuesta INNER JOIN
                         dbo.Instalacion ON dbo.Propuesta.ID_Instalacion = dbo.Instalacion.ID_Instalacion INNER JOIN
                         dbo.Producto_Familia ON dbo.Producto.ID_Producto_Familia = dbo.Producto_Familia.ID_Producto_Familia LEFT OUTER JOIN
                         dbo.Instalacion_Emplazamiento ON 
                         dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento = dbo.Instalacion_Emplazamiento.ID_Instalacion_Emplazamiento LEFT OUTER JOIN
                         dbo.Instalacion_Emplazamiento_Zona ON 
                         dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento_Zona = dbo.Instalacion_Emplazamiento_Zona.ID_Instalacion_Emplazamiento_Zona LEFT OUTER JOIN
                         dbo.Instalacion_Emplazamiento_Planta ON 
                         dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento_Planta = dbo.Instalacion_Emplazamiento_Planta.ID_Instalacion_Emplazamiento_Planta

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
PRINT N'Adding foreign keys to [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] ADD CONSTRAINT [FK_ListadoADV_Formulario] FOREIGN KEY ([ID_Formulario]) REFERENCES [dbo].[Formulario] ([ID_Formulario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Producto]'
GO
ALTER TABLE [dbo].[Producto] ADD CONSTRAINT [FK_Producto_Producto_Garantia] FOREIGN KEY ([ID_Producto_Garantia]) REFERENCES [dbo].[Producto_Garantia] ([ID_Producto_Garantia])
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
         Configuration = "(H (1[40] 4[21] 2[20] 3) )"
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
         Begin Table = "Propuesta"
            Begin Extent = 
               Top = 15
               Left = 805
               Bottom = 144
               Right = 1092
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta_Linea"
            Begin Extent = 
               Top = 204
               Left = 446
               Bottom = 494
               Right = 746
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Producto"
            Begin Extent = 
               Top = 238
               Left = 848
               Bottom = 554
               Right = 1177
            End
            DisplayFlags = 280
            TopColumn = 108
         End
         Begin Table = "Instalacion"
            Begin Extent = 
               Top = 17
               Left = 1200
               Bottom = 146
               Right = 1411
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Familia"
            Begin Extent = 
               Top = 390
               Left = 1215
               Bottom = 588
               Right = 1461
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Emplazamiento"
            Begin Extent = 
               Top = 109
               Left = 34
               Bottom = 238
               Right = 283
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Emplazamiento_Zona"
            Begin Extent = 
               Top = 427
               Left = 86
               Bottom = 556', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea_Vinculacion_Energetica', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N'
               Right = 391
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Emplazamiento_Planta"
            Begin Extent = 
               Top = 285
               Left = 9
               Bottom = 414
               Right = 296
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
      Begin ColumnWidths = 89
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2415
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2490
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
         Column = 8805
         Alias = 3375
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea_Vinculacion_Energetica', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea_Vinculacion_Energetica', NULL, NULL
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
