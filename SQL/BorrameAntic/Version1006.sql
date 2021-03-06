/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 06/06/2014 18:12:15

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
PRINT N'Dropping foreign keys from [dbo].[Entrada_Linea]'
GO
ALTER TABLE [dbo].[Entrada_Linea] DROP CONSTRAINT [FK_Entrada_Linea_Producto_Garantia]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Entrada_Linea]'
GO
ALTER TABLE [dbo].[Entrada_Linea] DROP
COLUMN [TotalBase],
COLUMN [TotalIVA],
COLUMN [TotalLinea]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[Entrada_Linea] ALTER COLUMN [Precio] [decimal] (10, 4) NOT NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[Entrada_Linea] ADD
[TotalBase] AS (([Unidad]*[Precio]-(([unidad]*[Precio])*[Descuento1])/(100))-(([Unidad]*[Precio]-(([unidad]*[Precio])*[Descuento1])/(100))*[Descuento2])/(100)),
[TotalIVA] AS (((([Unidad]*[Precio]-(([unidad]*[Precio])*[Descuento1])/(100))-(([Unidad]*[Precio]-(([unidad]*[Precio])*[Descuento1])/(100))*[Descuento2])/(100))*[IVA])/(100)),
[TotalLinea] AS ((([Unidad]*[Precio]-(([unidad]*[Precio])*[Descuento1])/(100))-(([Unidad]*[Precio]-(([unidad]*[Precio])*[Descuento1])/(100))*[Descuento2])/(100))+((([Unidad]*[Precio]-(([unidad]*[Precio])*[Descuento1])/(100))-(([Unidad]*[Precio]-(([unidad]*[Precio])*[Descuento1])/(100))*[Descuento2])/(100))*[IVA])/(100))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[FORM_Controls_BD]'
GO
CREATE TABLE [dbo].[FORM_Controls_BD]
(
[ID_FORM_Controls_BD] [int] NOT NULL IDENTITY(1, 1),
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
PRINT N'Creating primary key [PK_FORM_Controls_BD] on [dbo].[FORM_Controls_BD]'
GO
ALTER TABLE [dbo].[FORM_Controls_BD] ADD CONSTRAINT [PK_FORM_Controls_BD] PRIMARY KEY CLUSTERED  ([ID_FORM_Controls_BD])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_SYS_EstructuraBD]'
GO

CREATE VIEW [dbo].[C_SYS_EstructuraBD]
AS
SELECT        TOP (100) PERCENT TABLE_NAME AS Tabla, COLUMN_NAME AS Campo, CASE IS_Nullable WHEN 'Yes' THEN 0 ELSE 1 END AS Requerido, 
                         DATA_TYPE AS TipoDato, CHARACTER_MAXIMUM_LENGTH AS TextoLongitudMaxima, NUMERIC_PRECISION AS NumericoLongitud, 
                         NUMERIC_SCALE AS NumericoDecimales
FROM            INFORMATION_SCHEMA.COLUMNS
WHERE        (TABLE_NAME IN
                             (SELECT        CAST(TABLE_NAME AS varchar(100)) AS NombreTabla
                               FROM            INFORMATION_SCHEMA.TABLES
                               WHERE        (TABLE_TYPE = 'Base Table')))
ORDER BY TABLE_SCHEMA

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Entrada_Linea]'
GO
ALTER TABLE [dbo].[Entrada_Linea] ADD CONSTRAINT [FK_Entrada_Linea_Producto_Garantia] FOREIGN KEY ([ID_Producto_Garantia]) REFERENCES [dbo].[Producto_Garantia] ([ID_Producto_Garantia])
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
         Begin Table = "COLUMNS (INFORMATION_SCHEMA)"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 309
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_SYS_EstructuraBD', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_SYS_EstructuraBD', NULL, NULL
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


