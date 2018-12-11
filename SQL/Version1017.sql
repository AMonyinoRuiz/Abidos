/*
Run this script on:

        SERVER2012R2\SQLSERVER2012.AbidosDomingo    -  This database will be modified

to synchronize it with:

        SERVER2012R2\SQLSERVER2012.AbidosDomingoReal

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.0 from Red Gate Software Ltd at 04/10/2016 1:00:17

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
PRINT N'Altering [dbo].[Propuesta_Linea]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Propuesta_Linea] ALTER COLUMN [DetalleInstalacion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
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
                         dbo.Producto_Familia.ID_Producto_Familia = dbo.Producto_SubFamilia.ID_Producto_Familia LEFT OUTER JOIN
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
