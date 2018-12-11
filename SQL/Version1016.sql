/*
Run this script on:

        SERVER2012R2\SQLSERVER2012.AbidosMestre    -  This database will be modified

to synchronize it with:

        SERVER2012R2\SQLSERVER2012.AbidosDomingoReal

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.0 from Red Gate Software Ltd at 03/10/2016 17:04:38

*/
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[Producto]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Producto] ADD
[ID_Archivo_FotoPredeterminadaMini] [int] NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[C_Producto]'
GO
ALTER VIEW [dbo].[C_Producto]
AS
SELECT        TOP (100) PERCENT dbo.Producto.ID_Producto, dbo.Producto.Codigo, dbo.Producto.Referencia_Fabricante, dbo.Producto.Descripcion, dbo.Producto.Fecha_Alta, 
                         dbo.Producto.Fecha_Baja, dbo.Producto.Fuente_Alimentacion, dbo.Producto.Central, dbo.Producto.Central_Num_Zonas, 
                         dbo.Producto.Central_Num_Zonas_Inalambricas, dbo.Producto.Inalambrico, dbo.Producto.Elemento_arme_desarme, dbo.Producto.Sirena, 
                         dbo.Producto.Sistema_Transmision, dbo.Producto.Baterias, dbo.Producto.Elemento_Deteccion, dbo.Producto.Expansor, dbo.Producto.Expansor_Num_Elementos, 
                         dbo.Producto.Modulo_Rele, dbo.Producto.Modulo_Rele_Num_Elementos, dbo.Producto.Elemento_Verificación, dbo.Producto.Pulsador, dbo.Producto.Bidirecciona, 
                         dbo.Producto.Numero_Aberturas, dbo.Producto.PVP_Proveedor_Predeterminado, dbo.Producto.PVP, dbo.Producto.Supervisado, dbo.Producto.Activo, 
                         dbo.Producto_ATS.Descripcion AS ATS, dbo.Producto_ClaseAmbiental.Descripcion AS ClaseAmbiental, dbo.Producto_Division.Descripcion AS Division, 
                         dbo.Producto_Familia.Descripcion AS Familia, dbo.Producto_FrecuenciaInalambrica.Descripcion AS FrecuenciaInalambrica, 
                         dbo.Producto_Garantia.Tiempo AS Garantia, dbo.Producto_Grado.Descripcion AS Grado, dbo.Producto_Marca.Descripcion AS Marca, 
                         dbo.Producto_SistemaTransmision.Descripcion AS SistemaTransmision, dbo.Producto_SubFamilia.Descripcion AS Subfamilia, 
                         dbo.Producto_Tipo_Fuente_Alimentacion.Descripcion AS TipoFuenteAlimentacion, dbo.Producto_TipoSirena.Descripcion AS TipoSirena, 
                         dbo.Producto.ID_Producto_Familia, dbo.Producto.Obsoleto, dbo.Producto.RequiereNumeroSerie, dbo.Producto.DescripcionAmpliada, 
                         ISNULL(dbo.TempStockRealPorProducto.StockReal, 0) AS StockReal, dbo.Producto.Peso, dbo.Producto.StockMinimo, dbo.Producto.StockMaximo, 
                         dbo.Producto.EsBono, dbo.Producto.Bono_Cantidad, dbo.Archivo.CampoBinario AS Foto
FROM            dbo.Producto INNER JOIN
                         dbo.Producto_Division ON dbo.Producto.ID_Producto_Division = dbo.Producto_Division.ID_Producto_Division INNER JOIN
                         dbo.Producto_Familia ON dbo.Producto.ID_Producto_Familia = dbo.Producto_Familia.ID_Producto_Familia AND 
                         dbo.Producto_Division.ID_Producto_Division = dbo.Producto_Familia.ID_Producto_Division INNER JOIN
                         dbo.Producto_Marca ON dbo.Producto.ID_Producto_Marca = dbo.Producto_Marca.ID_Producto_Marca AND 
                         dbo.Producto_Division.ID_Producto_Division = dbo.Producto_Marca.ID_Producto_Division INNER JOIN
                         dbo.Producto_SubFamilia ON dbo.Producto.ID_Producto_SubFamilia = dbo.Producto_SubFamilia.ID_Producto_SubFamilia AND 
                         dbo.Producto_Familia.ID_Producto_Familia = dbo.Producto_SubFamilia.ID_Producto_Familia INNER JOIN
                         dbo.Archivo ON dbo.Producto.ID_Archivo_FotoPredeterminadaMini = dbo.Archivo.ID_Archivo LEFT OUTER JOIN
                         dbo.TempStockRealPorProducto ON dbo.Producto.ID_Producto = dbo.TempStockRealPorProducto.ID_Producto LEFT OUTER JOIN
                         dbo.Producto_TipoSirena ON dbo.Producto.ID_Producto_TipoSirena = dbo.Producto_TipoSirena.ID_Producto_TipoSirena LEFT OUTER JOIN
                         dbo.Producto_Tipo_Fuente_Alimentacion ON 
                         dbo.Producto.ID_Producto_Tipo_Fuente_Alimentacion = dbo.Producto_Tipo_Fuente_Alimentacion.ID_Producto_Tipo_Fuente_Alimentacion LEFT OUTER JOIN
                         dbo.Producto_SistemaTransmision ON 
                         dbo.Producto.ID_Producto_SistemaTransmision = dbo.Producto_SistemaTransmision.ID_Producto_SistemaTransmision LEFT OUTER JOIN
                         dbo.Producto_Grado ON dbo.Producto.ID_Producto_Grado = dbo.Producto_Grado.ID_Producto_Grado LEFT OUTER JOIN
                         dbo.Producto_Garantia ON dbo.Producto.ID_Producto_Garantia = dbo.Producto_Garantia.ID_Producto_Garantia LEFT OUTER JOIN
                         dbo.Producto_FrecuenciaInalambrica ON 
                         dbo.Producto.ID_Producto_FrecuenciaInalambrica = dbo.Producto_FrecuenciaInalambrica.ID_Producto_FrecuenciaInalambrica LEFT OUTER JOIN
                         dbo.Producto_ClaseAmbiental ON dbo.Producto.ID_Producto_Clase_Ambiental = dbo.Producto_ClaseAmbiental.ID_Producto_ClaseAmbiental LEFT OUTER JOIN
                         dbo.Producto_ATS ON dbo.Producto.ID_Producto_ATS = dbo.Producto_ATS.ID_Producto_ATS
WHERE        (dbo.Producto.Activo = 1)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Propuesta_Diagrama]'
GO
CREATE TABLE [dbo].[Propuesta_Diagrama]
(
[ID_Propuesta_Diagrama] [int] NOT NULL IDENTITY(1, 1),
[ID_Propuesta] [int] NOT NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[FechaCreacion] [smalldatetime] NOT NULL,
[Validado] [bit] NOT NULL CONSTRAINT [DF_Propuesta_Diagrama_Validado] DEFAULT ((0)),
[ID_Instalacion_Emplazamiento] [int] NULL,
[ID_Instalacion_Emplazamiento_Zona] [int] NULL,
[ID_Instalacion_Emplazamiento_Planta] [int] NULL,
[ID_DiagramaBinario] [int] NULL,
[ID_Propuesta_Antigua] [int] NULL,
[Propuesta_Version_Antigua] [nvarchar] (1) COLLATE Modern_Spanish_CI_AS NULL,
[ID_Producto_Division] [int] NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Propuesta_Diagrama] on [dbo].[Propuesta_Diagrama]'
GO
ALTER TABLE [dbo].[Propuesta_Diagrama] ADD CONSTRAINT [PK_Propuesta_Diagrama] PRIMARY KEY CLUSTERED  ([ID_Propuesta_Diagrama])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[DiagramaBinario]'
GO
CREATE TABLE [dbo].[DiagramaBinario]
(
[ID_DiagramaBinario] [int] NOT NULL IDENTITY(1, 1),
[Fichero] [varbinary] (max) NOT NULL,
[Foto] [varbinary] (max) NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_DiagramaBinario] on [dbo].[DiagramaBinario]'
GO
ALTER TABLE [dbo].[DiagramaBinario] ADD CONSTRAINT [PK_DiagramaBinario] PRIMARY KEY CLUSTERED  ([ID_DiagramaBinario])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_Diagrama]'
GO
ALTER TABLE [dbo].[Propuesta_Diagrama] ADD CONSTRAINT [FK_Propuesta_Diagrama_DiagramaBinario] FOREIGN KEY ([ID_DiagramaBinario]) REFERENCES [dbo].[DiagramaBinario] ([ID_DiagramaBinario])
ALTER TABLE [dbo].[Propuesta_Diagrama] ADD CONSTRAINT [FK_Propuesta_Diagrama_Instalacion_Emplazamiento] FOREIGN KEY ([ID_Instalacion_Emplazamiento]) REFERENCES [dbo].[Instalacion_Emplazamiento] ([ID_Instalacion_Emplazamiento])
ALTER TABLE [dbo].[Propuesta_Diagrama] ADD CONSTRAINT [FK_Propuesta_Diagrama_Instalacion_Emplazamiento_Planta] FOREIGN KEY ([ID_Instalacion_Emplazamiento_Planta]) REFERENCES [dbo].[Instalacion_Emplazamiento_Planta] ([ID_Instalacion_Emplazamiento_Planta])
ALTER TABLE [dbo].[Propuesta_Diagrama] ADD CONSTRAINT [FK_Propuesta_Diagrama_Instalacion_Emplazamiento_Zona] FOREIGN KEY ([ID_Instalacion_Emplazamiento_Zona]) REFERENCES [dbo].[Instalacion_Emplazamiento_Zona] ([ID_Instalacion_Emplazamiento_Zona])
ALTER TABLE [dbo].[Propuesta_Diagrama] ADD CONSTRAINT [FK_Propuesta_Diagrama_Producto_Division] FOREIGN KEY ([ID_Producto_Division]) REFERENCES [dbo].[Producto_Division] ([ID_Producto_Division])
ALTER TABLE [dbo].[Propuesta_Diagrama] ADD CONSTRAINT [FK_Propuesta_Diagrama_Propuesta] FOREIGN KEY ([ID_Propuesta]) REFERENCES [dbo].[Propuesta] ([ID_Propuesta])
ALTER TABLE [dbo].[Propuesta_Diagrama] ADD CONSTRAINT [FK_Propuesta_Diagrama_Propuesta1] FOREIGN KEY ([ID_Propuesta_Antigua]) REFERENCES [dbo].[Propuesta] ([ID_Propuesta])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering extended properties'
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[35] 4[13] 2[28] 3) )"
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
         Left = -947
      End
      Begin Tables = 
         Begin Table = "Producto"
            Begin Extent = 
               Top = 39
               Left = 1190
               Bottom = 347
               Right = 1514
            End
            DisplayFlags = 280
            TopColumn = 110
         End
         Begin Table = "Producto_Division"
            Begin Extent = 
               Top = 3
               Left = 864
               Bottom = 122
               Right = 1062
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Familia"
            Begin Extent = 
               Top = 126
               Left = 38
               Bottom = 245
               Right = 236
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Marca"
            Begin Extent = 
               Top = 258
               Left = 56
               Bottom = 377
               Right = 254
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_SubFamilia"
            Begin Extent = 
               Top = 263
               Left = 572
               Bottom = 382
               Right = 777
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TempStockRealPorProducto"
            Begin Extent = 
               Top = 414
               Left = 827
               Bottom = 510
               Right = 1036
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_TipoSirena"
            Begin Extent = 
               Top = 455
               Left = 150
               Bottom = 57', 'SCHEMA', N'dbo', 'VIEW', N'C_Producto', NULL, NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'4
               Right = 355
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Tipo_Fuente_Alimentacion"
            Begin Extent = 
               Top = 253
               Left = 750
               Bottom = 372
               Right = 1031
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_SistemaTransmision"
            Begin Extent = 
               Top = 275
               Left = 288
               Bottom = 394
               Right = 536
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Grado"
            Begin Extent = 
               Top = 121
               Left = 844
               Bottom = 240
               Right = 1042
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Garantia"
            Begin Extent = 
               Top = 131
               Left = 600
               Bottom = 250
               Right = 798
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_FrecuenciaInalambrica"
            Begin Extent = 
               Top = 137
               Left = 289
               Bottom = 256
               Right = 551
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_ClaseAmbiental"
            Begin Extent = 
               Top = 6
               Left = 570
               Bottom = 125
               Right = 798
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_ATS"
            Begin Extent = 
               Top = 54
               Left = 469
               Bottom = 173
               Right = 667
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Archivo"
            Begin Extent = 
               Top = 130
               Left = 1690
               Bottom = 259
               Right = 1899
            End
            DisplayFlags = 280
            TopColumn = 4
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 53
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
     ', 'SCHEMA', N'dbo', 'VIEW', N'C_Producto', NULL, NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @xp int
SELECT @xp=3
EXEC sp_updateextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Producto', NULL, NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating extended properties'
GO
EXEC sp_addextendedproperty N'MS_DiagramPane3', N' Begin ColumnWidths = 11
         Column = 5355
         Alias = 1800
         Table = 2910
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1965
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_Producto', NULL, NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	PRINT 'The database update failed'
END
GO