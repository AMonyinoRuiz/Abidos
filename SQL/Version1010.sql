/*
Run this script on:

        192.168.1.225.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        192.168.1.225.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.0 from Red Gate Software Ltd at 10/12/2015 18:53:14

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
PRINT N'Creating [dbo].[Parte_TrabajosARealizar_Producto]'
GO
CREATE TABLE [dbo].[Parte_TrabajosARealizar_Producto]
(
[ID_Parte_TrabajosARealizar_Producto] [int] NOT NULL IDENTITY(1, 1),
[ID_Parte_TrabajosARealizar] [int] NOT NULL,
[ID_Producto] [int] NOT NULL,
[Cantidad] [decimal] (10, 2) NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Parte_TrabajosARealizar_Producto] on [dbo].[Parte_TrabajosARealizar_Producto]'
GO
ALTER TABLE [dbo].[Parte_TrabajosARealizar_Producto] ADD CONSTRAINT [PK_Parte_TrabajosARealizar_Producto] PRIMARY KEY CLUSTERED  ([ID_Parte_TrabajosARealizar_Producto])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[C_Parte_TrabajosARealizar_Productos]'
GO
CREATE VIEW [dbo].[C_Parte_TrabajosARealizar_Productos]
AS
SELECT        dbo.Parte_TrabajosARealizar_Producto.ID_Parte_TrabajosARealizar_Producto, dbo.Parte_TrabajosARealizar_Producto.ID_Parte_TrabajosARealizar, 
                         dbo.Parte_TrabajosARealizar_Producto.ID_Producto, dbo.Parte_TrabajosARealizar_Producto.Cantidad, dbo.Producto.Codigo, dbo.Producto.Referencia_Fabricante, 
                         dbo.Producto.Descripcion, dbo.Parte_TrabajosARealizar.ID_Parte
FROM            dbo.Producto INNER JOIN
                         dbo.Parte_TrabajosARealizar_Producto ON dbo.Producto.ID_Producto = dbo.Parte_TrabajosARealizar_Producto.ID_Producto INNER JOIN
                         dbo.Parte_TrabajosARealizar ON dbo.Parte_TrabajosARealizar_Producto.ID_Parte_TrabajosARealizar = dbo.Parte_TrabajosARealizar.ID_Parte_TrabajosARealizar
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Parte_TrabajosARealizar_Producto]'
GO
ALTER TABLE [dbo].[Parte_TrabajosARealizar_Producto] ADD CONSTRAINT [FK_Parte_TrabajosARealizar_Producto_Parte_TrabajosARealizar] FOREIGN KEY ([ID_Parte_TrabajosARealizar]) REFERENCES [dbo].[Parte_TrabajosARealizar] ([ID_Parte_TrabajosARealizar])
ALTER TABLE [dbo].[Parte_TrabajosARealizar_Producto] ADD CONSTRAINT [FK_Parte_TrabajosARealizar_Producto_Producto] FOREIGN KEY ([ID_Producto]) REFERENCES [dbo].[Producto] ([ID_Producto])
GO
IF @@ERROR <> 0 SET NOEXEC ON
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
         Begin Table = "Producto"
            Begin Extent = 
               Top = 13
               Left = 912
               Bottom = 288
               Right = 1241
            End
            DisplayFlags = 280
            TopColumn = 21
         End
         Begin Table = "Parte_TrabajosARealizar_Producto"
            Begin Extent = 
               Top = 67
               Left = 492
               Bottom = 254
               Right = 776
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_TrabajosARealizar"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 194
               Right = 338
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
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_TrabajosARealizar_Productos', NULL, NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_TrabajosARealizar_Productos', NULL, NULL
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