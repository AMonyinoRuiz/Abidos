/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 08/05/2014 12:35:23

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
PRINT N'Altering [dbo].[Producto_Garantia]'
GO
ALTER TABLE [dbo].[Producto_Garantia] ALTER COLUMN [Tiempo] [decimal] (6, 2) NOT NULL
GO
Delete From GRID_ToolGrid_Tools Where Grid_Name='GRD_DIVISION' and Formulari_Name='FRMMANTENIMENT_FAMILIAS' and ToolKey='Eliminar'
GO
PRINT(N'Update rows in [dbo].[Idioma]')
GO
UPDATE [dbo].[Idioma] SET [Descripcion]=N'Catalán' WHERE [ID_Idioma]=2
UPDATE [dbo].[Idioma] SET [Descripcion]=N'Francés' WHERE [ID_Idioma]=3
UPDATE [dbo].[Idioma] SET [Descripcion]=N'Alemán' WHERE [ID_Idioma]=4
PRINT(N'Operation applied to 3 rows out of 3')
GO
UPDATE       GRID_Columna
SET                Columna_Caption = 'Emplazamiento Construcción'
WHERE     Grid_Name='GRD' and    (Formulari_Name = 'FRMMANTENIMENT') AND (Formulari_AccessibleName = 'Contadores') AND (Columna_Key = 'Instalacion_Emplazamiento_Construccion')
GO
INSERT INTO [dbo].[Producto_Garantia] ([ID_Producto_Garantia], [Codigo], [Tiempo], [Activo]) VALUES (12, N'1011', 0.50, 1)
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
