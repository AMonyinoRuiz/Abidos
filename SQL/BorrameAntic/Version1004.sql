/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 16/05/2014 13:44:29

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
ALTER TABLE [dbo].[Entrada_Linea] DROP CONSTRAINT [FK_Entrada_Linea_Almacen]
ALTER TABLE [dbo].[Entrada_Linea] DROP CONSTRAINT [FK_Entrada_Linea_Producto_Garantia]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Entrada]'
GO
ALTER TABLE [dbo].[Entrada] DROP CONSTRAINT [FK_Entrada_Almacen]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[NS]'
GO
ALTER TABLE [dbo].[NS] DROP CONSTRAINT [FK_NS_Almacen]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Personal_Idioma]'
GO
ALTER TABLE [dbo].[Personal_Idioma] DROP CONSTRAINT [FK_Personal_Idioma_Idioma]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Producto_DescripcionIdioma]'
GO
ALTER TABLE [dbo].[Producto_DescripcionIdioma] DROP CONSTRAINT [FK_Producto_DescripcionIdioma_Idioma]
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
PRINT N'Dropping trigger [dbo].[TempStockRealPorProducto_ChangeTracking] from [dbo].[TempStockRealPorProducto]'
GO
DROP TRIGGER [dbo].[TempStockRealPorProducto_ChangeTracking]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Producto_Garantia]'
GO
ALTER TABLE [dbo].[Producto_Garantia] ALTER COLUMN [Tiempo] [decimal] (6, 2) NOT NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Producto_Proveedor]'
GO
ALTER TABLE [dbo].[Producto_Proveedor] ADD
[CodigoProductoProveedor] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[TempStockRealPorProductoYPorAlmacen]'
GO
CREATE TABLE [dbo].[TempStockRealPorProductoYPorAlmacen]
(
[ID_Producto] [int] NOT NULL,
[ID_Almacen] [int] NOT NULL,
[StockReal] [decimal] (12, 2) NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Entrada_Linea]'
GO
ALTER TABLE [dbo].[Entrada_Linea] ADD CONSTRAINT [FK_Entrada_Linea_Almacen] FOREIGN KEY ([ID_Almacen]) REFERENCES [dbo].[Almacen] ([ID_Almacen])
ALTER TABLE [dbo].[Entrada_Linea] ADD CONSTRAINT [FK_Entrada_Linea_Producto_Garantia] FOREIGN KEY ([ID_Producto_Garantia]) REFERENCES [dbo].[Producto_Garantia] ([ID_Producto_Garantia])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Entrada]'
GO
ALTER TABLE [dbo].[Entrada] ADD CONSTRAINT [FK_Entrada_Almacen] FOREIGN KEY ([ID_Almacen]) REFERENCES [dbo].[Almacen] ([ID_Almacen])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[NS]'
GO
ALTER TABLE [dbo].[NS] ADD CONSTRAINT [FK_NS_Almacen] FOREIGN KEY ([ID_Almacen]) REFERENCES [dbo].[Almacen] ([ID_Almacen])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Personal_Idioma]'
GO
ALTER TABLE [dbo].[Personal_Idioma] ADD CONSTRAINT [FK_Personal_Idioma_Idioma] FOREIGN KEY ([ID_Idioma]) REFERENCES [dbo].[Idioma] ([ID_Idioma])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Producto_DescripcionIdioma]'
GO
ALTER TABLE [dbo].[Producto_DescripcionIdioma] ADD CONSTRAINT [FK_Producto_DescripcionIdioma_Idioma] FOREIGN KEY ([ID_Idioma]) REFERENCES [dbo].[Idioma] ([ID_Idioma])
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
