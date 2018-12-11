/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 18/07/2014 19:42:16

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
PRINT N'Creating [dbo].[PropuestaEspecificacion_Respuesta]'
GO
CREATE TABLE [dbo].[PropuestaEspecificacion_Respuesta]
(
[ID_PropuestaEspecificacion_Respuesta] [int] NOT NULL IDENTITY(1, 1),
[ID_PropuestaEspecificacion] [int] NOT NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_PropuestaCuestionario_Respuesta] on [dbo].[PropuestaEspecificacion_Respuesta]'
GO
ALTER TABLE [dbo].[PropuestaEspecificacion_Respuesta] ADD CONSTRAINT [PK_PropuestaCuestionario_Respuesta] PRIMARY KEY CLUSTERED  ([ID_PropuestaEspecificacion_Respuesta])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[PropuestaEspecificacion]'
GO
CREATE TABLE [dbo].[PropuestaEspecificacion]
(
[ID_PropuestaEspecificacion] [int] NOT NULL IDENTITY(1, 1),
[ID_Producto_Division] [int] NULL,
[ID_Producto_Familia] [int] NULL,
[Descripcion] [nvarchar] (500) COLLATE Modern_Spanish_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_PropuestaCuestionario] on [dbo].[PropuestaEspecificacion]'
GO
ALTER TABLE [dbo].[PropuestaEspecificacion] ADD CONSTRAINT [PK_PropuestaCuestionario] PRIMARY KEY CLUSTERED  ([ID_PropuestaEspecificacion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Propuesta_PropuestaEspecificacion]'
GO
CREATE TABLE [dbo].[Propuesta_PropuestaEspecificacion]
(
[ID_Propuesta_PropuestaEspecificacion] [int] NOT NULL IDENTITY(1, 1),
[ID_Propuesta] [int] NOT NULL,
[ID_PropuestaEspecificacion] [int] NOT NULL,
[ID_PropuestaEspecificacion_Respuesta] [int] NULL,
[Observaciones] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NULL,
[FechaRespuesta] [smalldatetime] NULL,
[ID_Usuario] [int] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Propuesta_PropuestaCuestionario] on [dbo].[Propuesta_PropuestaEspecificacion]'
GO
ALTER TABLE [dbo].[Propuesta_PropuestaEspecificacion] ADD CONSTRAINT [PK_Propuesta_PropuestaCuestionario] PRIMARY KEY CLUSTERED  ([ID_Propuesta_PropuestaEspecificacion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Propuesta_Especificacion]'
GO

CREATE VIEW [dbo].[C_Propuesta_Especificacion]
AS
SELECT        dbo.Propuesta_PropuestaEspecificacion.ID_Propuesta_PropuestaEspecificacion, dbo.Propuesta_PropuestaEspecificacion.ID_Propuesta, 
                         dbo.Producto_Division.Descripcion AS División, dbo.Producto_Familia.Descripcion AS Familia, dbo.PropuestaEspecificacion.Descripcion AS Especificación, 
                         dbo.PropuestaEspecificacion_Respuesta.Descripcion AS Respuesta, dbo.Propuesta_PropuestaEspecificacion.Observaciones, 
                         dbo.Propuesta_PropuestaEspecificacion.FechaRespuesta AS Fecha, dbo.Usuario.Nombre
FROM            dbo.PropuestaEspecificacion INNER JOIN
                         dbo.Propuesta_PropuestaEspecificacion ON 
                         dbo.PropuestaEspecificacion.ID_PropuestaEspecificacion = dbo.Propuesta_PropuestaEspecificacion.ID_PropuestaEspecificacion INNER JOIN
                         dbo.PropuestaEspecificacion_Respuesta ON 
                         dbo.Propuesta_PropuestaEspecificacion.ID_PropuestaEspecificacion_Respuesta = dbo.PropuestaEspecificacion_Respuesta.ID_PropuestaEspecificacion_Respuesta
                          INNER JOIN
                         dbo.Usuario ON dbo.Propuesta_PropuestaEspecificacion.ID_Usuario = dbo.Usuario.ID_Usuario LEFT OUTER JOIN
                         dbo.Producto_Familia ON dbo.PropuestaEspecificacion.ID_Producto_Familia = dbo.Producto_Familia.ID_Producto_Familia LEFT OUTER JOIN
                         dbo.Producto_Division ON dbo.PropuestaEspecificacion.ID_Producto_Division = dbo.Producto_Division.ID_Producto_Division

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Propuesta_Linea_Vinculacion_Energetica]'
GO
ALTER VIEW dbo.C_Propuesta_Linea_Vinculacion_Energetica
AS
SELECT        dbo.Propuesta_Linea.ID_Propuesta_Linea, dbo.Propuesta_Linea.ID_Propuesta, dbo.Propuesta.ID_Instalacion, dbo.Propuesta_Linea.Identificador, 
                         dbo.Propuesta_Linea.IdentificadorDelProducto, dbo.Propuesta_Linea.Descripcion, dbo.Instalacion_Emplazamiento.Descripcion AS Emplazamiento, 
                         dbo.Instalacion_Emplazamiento_Planta.Descripcion AS Planta, dbo.Instalacion_Emplazamiento_Zona.Descripcion AS Zona, dbo.Propuesta_Linea.NickZona, 
                         dbo.Producto.PotenciaSalida * dbo.Propuesta_Linea.Unidad AS [Potencia de salida], 
                         dbo.Producto.PotenciaEntrada * dbo.Propuesta_Linea.Unidad AS [Potencia de entrada], ISNULL(dbo.Producto.PotenciaSalida, 0) 
                         * dbo.Propuesta_Linea.Unidad - ISNULL
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
PRINT N'Creating [dbo].[C_Parte_Estadistica]'
GO
CREATE VIEW dbo.C_Parte_Estadistica
AS
SELECT        TOP (100) PERCENT dbo.Parte.ID_Parte AS [Número de parte], dbo.Parte_Tipo.Descripcion AS [Tipo de parte], dbo.Parte.ID_Instalacion AS [Número de instalación], 
                         dbo.Cliente.Nombre AS [Nombre del cliente], dbo.Parte.FechaAlta AS [Fecha alta del parte], dbo.Parte_Estado.Descripcion AS [Estado del parte],
                             (SELECT        MIN(Fecha) AS Expr1
                               FROM            dbo.Parte_Horas
                               WHERE        (ID_Parte = dbo.Parte.ID_Parte)) AS [Primera imputación de horas], ISNULL(DATEDIFF(day, dbo.Parte.FechaAlta, GETDATE()), 0) 
                         AS [Número de días desde que se creo el parte], ISNULL(DATEDIFF(day, dbo.Parte.FechaInicio, GETDATE()), 0) AS [Número de días des de que se empezó el parte], 
                         ISNULL(DATEDIFF(day,
                             (SELECT        MIN(Fecha) AS Expr1
                               FROM            dbo.Parte_Horas AS Parte_Horas_1
                               WHERE        (ID_Parte = dbo.Parte.ID_Parte)), GETDATE()), 0) AS [Número de días des de que se imputó la primera hora], ISNULL(dbo.Parte.HorasRealizadas, 0) 
                         AS [Horas realizadas], ISNULL(dbo.Parte.HorasPrevistas, 0) AS [Horas Previstas], ISNULL(dbo.Parte.HorasPrevistas, 0) - ISNULL(dbo.Parte.HorasRealizadas, 0) 
                         AS Diferencia, dbo.Parte.FechaInicio AS [Fecha de inicio del parte],
                             (SELECT        COUNT(*) AS Expr1
                               FROM            (SELECT DISTINCT ID_Personal
                                                         FROM            dbo.Parte_Horas AS Parte_Horas_2
                                                         WHERE        (ID_Parte = dbo.Parte.ID_Parte)) AS a) AS [Número de personas que han participado]
FROM            dbo.Parte INNER JOIN
                         dbo.Cliente ON dbo.Parte.ID_Cliente = dbo.Cliente.ID_Cliente INNER JOIN
                         dbo.Parte_Tipo ON dbo.Parte.ID_Parte_Tipo = dbo.Parte_Tipo.ID_Parte_Tipo INNER JOIN
                         dbo.Parte_Estado ON dbo.Parte.ID_Parte_Estado = dbo.Parte_Estado.ID_Parte_Estado
WHERE        (dbo.Parte.Activo = 1)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_PropuestaEspecificacion]'
GO
ALTER TABLE [dbo].[Propuesta_PropuestaEspecificacion] ADD CONSTRAINT [FK_Propuesta_PropuestaCuestionario_Propuesta] FOREIGN KEY ([ID_Propuesta]) REFERENCES [dbo].[Propuesta] ([ID_Propuesta])
ALTER TABLE [dbo].[Propuesta_PropuestaEspecificacion] ADD CONSTRAINT [FK_Propuesta_PropuestaEspecificacion_PropuestaEspecificacion] FOREIGN KEY ([ID_PropuestaEspecificacion]) REFERENCES [dbo].[PropuestaEspecificacion] ([ID_PropuestaEspecificacion])
ALTER TABLE [dbo].[Propuesta_PropuestaEspecificacion] ADD CONSTRAINT [FK_Propuesta_PropuestaCuestionario_PropuestaCuestionario_Respuesta] FOREIGN KEY ([ID_PropuestaEspecificacion_Respuesta]) REFERENCES [dbo].[PropuestaEspecificacion_Respuesta] ([ID_PropuestaEspecificacion_Respuesta])
ALTER TABLE [dbo].[Propuesta_PropuestaEspecificacion] ADD CONSTRAINT [FK_Propuesta_PropuestaCuestionario_Usuario] FOREIGN KEY ([ID_Usuario]) REFERENCES [dbo].[Usuario] ([ID_Usuario])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[PropuestaEspecificacion_Respuesta]'
GO
ALTER TABLE [dbo].[PropuestaEspecificacion_Respuesta] ADD CONSTRAINT [FK_PropuestaEspecificacion_Respuesta_PropuestaEspecificacion] FOREIGN KEY ([ID_PropuestaEspecificacion]) REFERENCES [dbo].[PropuestaEspecificacion] ([ID_PropuestaEspecificacion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[PropuestaEspecificacion]'
GO
ALTER TABLE [dbo].[PropuestaEspecificacion] ADD CONSTRAINT [FK_PropuestaEspecificacion_Producto_Division] FOREIGN KEY ([ID_Producto_Division]) REFERENCES [dbo].[Producto_Division] ([ID_Producto_Division])
ALTER TABLE [dbo].[PropuestaEspecificacion] ADD CONSTRAINT [FK_PropuestaEspecificacion_Producto_Familia] FOREIGN KEY ([ID_Producto_Familia]) REFERENCES [dbo].[Producto_Familia] ([ID_Producto_Familia])
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
         Begin Table = "PropuestaEspecificacion"
            Begin Extent = 
               Top = 54
               Left = 341
               Bottom = 193
               Right = 574
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PropuestaEspecificacion_Respuesta"
            Begin Extent = 
               Top = 252
               Left = 505
               Bottom = 364
               Right = 796
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta_PropuestaEspecificacion"
            Begin Extent = 
               Top = 119
               Left = 895
               Bottom = 333
               Right = 1186
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Division"
            Begin Extent = 
               Top = 0
               Left = 48
               Bottom = 224
               Right = 257
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Familia"
            Begin Extent = 
               Top = 236
               Left = 34
               Bottom = 365
               Right = 280
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Usuario"
            Begin Extent = 
               Top = 47
               Left = 1236
               Bottom = 249
               Right = 1496
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
   End
   Be', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Especificacion', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N'gin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2235
         Alias = 2205
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Especificacion', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Especificacion', NULL, NULL
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
         Begin Table = "Parte"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 320
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 6
               Left = 358
               Bottom = 135
               Right = 623
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Tipo"
            Begin Extent = 
               Top = 6
               Left = 661
               Bottom = 135
               Right = 870
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Estado"
            Begin Extent = 
               Top = 6
               Left = 908
               Bottom = 135
               Right = 1117
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
         Column = 1440
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
      End', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_Estadistica', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N'
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_Estadistica', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_Estadistica', NULL, NULL
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