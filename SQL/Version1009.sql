/*
Run this script on:

        192.168.1.225.AbidosMestre    -  This database will be modified

to synchronize it with:

        192.168.1.225.AbidosDomingo

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.0 from Red Gate Software Ltd at 03/12/2015 1:05:40

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
PRINT N'Altering [dbo].[Parte_TrabajosARealizar]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Parte_TrabajosARealizar] ADD
[FechaPrevision] [smalldatetime] NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Parte_TrabajosARealizar] ALTER COLUMN [DuracionAproximada] [decimal] (10, 2) NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[C_Parte_TrabajosARealizar]'
GO
ALTER VIEW [dbo].[C_Parte_TrabajosARealizar]
AS
SELECT        dbo.Parte_TrabajosARealizar.ID_Parte_TrabajosARealizar, dbo.Parte_TrabajosARealizar.ID_Parte, dbo.Parte_TrabajosARealizar.FechaAlta, 
                         dbo.Parte_TrabajosARealizar.Titulo, dbo.Parte_TrabajosARealizar.Descripcion, dbo.Parte_TrabajosARealizar.NumDia, 
                         dbo.Parte_TrabajosARealizar.DuracionAproximada, dbo.Parte_TrabajosARealizar.Participantes, dbo.Parte_TrabajosARealizar.Orden, 
                         dbo.Parte_TrabajosARealizar.ID_Partes_TrabajosARealizar_Obligatorio, dbo.Parte_TrabajosARealizar.Realizada, 
                         Parte_TrabajosARealizar_1.Titulo AS Obligatorio_Titulo, dbo.Parte_TrabajosARealizar.FechaPrevision
FROM            dbo.Parte_TrabajosARealizar LEFT OUTER JOIN
                         dbo.Parte_TrabajosARealizar AS Parte_TrabajosARealizar_1 ON 
                         dbo.Parte_TrabajosARealizar.ID_Partes_TrabajosARealizar_Obligatorio = Parte_TrabajosARealizar_1.ID_Parte_TrabajosARealizar
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[Parte_TrabajosARealizar_Personal]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Parte_TrabajosARealizar_Personal] ADD
[FechaFinalizacion] [smalldatetime] NULL,
[Finalizada] [bit] NOT NULL CONSTRAINT [DF_Parte_TrabajosARealizar_Personal_Finalizado] DEFAULT ((0))
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
         Configuration = "(H (1[42] 4[17] 2[25] 3) )"
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
         Begin Table = "Parte_TrabajosARealizar"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 324
               Right = 338
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_TrabajosARealizar_1"
            Begin Extent = 
               Top = 6
               Left = 376
               Bottom = 275
               Right = 676
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
         Column = 3300
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_TrabajosARealizar', NULL, NULL
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