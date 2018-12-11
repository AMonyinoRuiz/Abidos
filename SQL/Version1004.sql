/*
Run this script on:

        Server2012R2\SQLServer2012.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        Server2012R2\SQLServer2012.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 16/03/2015 14:44:27

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
PRINT N'Dropping foreign keys from [dbo].[ActividadCRM_Archivo]'
GO
ALTER TABLE [dbo].[ActividadCRM_Archivo] DROP CONSTRAINT [FK_ActividadCRM_Archivo_ActividadCRM]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[ActividadCRM]'
GO
ALTER TABLE [dbo].[ActividadCRM] DROP CONSTRAINT [FK_ActividadCRM_Cliente]
ALTER TABLE [dbo].[ActividadCRM] DROP CONSTRAINT [FK_ActividadCRM_Instalacion]
ALTER TABLE [dbo].[ActividadCRM] DROP CONSTRAINT [FK_ActividadCRM_Propuesta]
ALTER TABLE [dbo].[ActividadCRM] DROP CONSTRAINT [FK_ActividadCRM_Personal]
ALTER TABLE [dbo].[ActividadCRM] DROP CONSTRAINT [FK_ActividadCRM_ActividadCRM_Tipo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
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
PRINT N'Dropping constraints from [dbo].[ActividadCRM]'
GO
ALTER TABLE [dbo].[ActividadCRM] DROP CONSTRAINT [PK_ActividadCRM]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[ActividadCRM]'
GO
ALTER TABLE [dbo].[ActividadCRM] DROP CONSTRAINT [DF_ActividadCRM_Realizado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[ActividadCRM]'
GO
ALTER TABLE [dbo].[ActividadCRM] DROP CONSTRAINT [DF_ActividadCRM_Activo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Albaranes_de_Venta]'
GO
ALTER VIEW dbo.C_LADV_Albaranes_de_Venta
AS
SELECT        Codigo, Descripcion, FechaEntrada, Estado, [Nombre del cliente], Base, IVA, Descuento, Total, [Tiop de documento]
FROM            dbo.C_Entrada
WHERE        ([Tiop de documento] = N'Albaran de venta')
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Propuesta_Linea]'
GO
ALTER TABLE [dbo].[Propuesta_Linea] ADD
[PlazoEntrega] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Producto]'
GO
ALTER TABLE [dbo].[Producto] ADD
[PlazoEntrega] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Entrada_Linea]'
GO
ALTER TABLE [dbo].[Entrada_Linea] ALTER COLUMN [Uso] [nvarchar] (1000) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Propuesta_Linea]'
GO
ALTER VIEW dbo.C_Propuesta_Linea
AS
SELECT        dbo.Propuesta_Linea.ID_Propuesta_Linea, dbo.Propuesta_Linea.ID_Propuesta, dbo.Propuesta_Linea.Identificador, dbo.Propuesta_Linea.Uso, 
                         dbo.Propuesta_Linea.Unidad, dbo.Propuesta_Linea.Precio, dbo.Propuesta_Linea.Descuento, dbo.Propuesta_Linea.IVA, dbo.Propuesta_Linea.TotalBase, 
                         Propuesta_Linea_1.Identificador AS Identificador_LineaVinculada, dbo.Instalacion_Emplazamiento.Descripcion AS Emplazamiento, 
                         dbo.Instalacion_Emplazamiento_Planta.Descripcion AS Planta, dbo.Instalacion_Emplazamiento_Zona.Descripcion AS Zona, dbo.Producto.Codigo AS CodigoProducto,
                          dbo.Instalacion_Emplazamiento_Abertura.Descripcion_Detallada AS Abertura, dbo.Instalacion_ElementosAProteger_Tipo.Descripcion AS ElementoAProteger, 
                         dbo.Propuesta_Linea.Descripcion, dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento_Zona, dbo.Propuesta_Linea.ID_Propuesta_Linea_Vinculado, 
                         dbo.Producto_Division.Descripcion AS Division, Propuesta_1.Codigo AS PropuestaCodigo, Propuesta_1.Version AS PropuestaVersion, 
                         dbo.Producto.ID_Producto_Division, dbo.Propuesta_Linea.NumZona, dbo.Propuesta_Linea.DetalleInstalacion, dbo.Propuesta_Linea.IdentificadorDelProducto, 
                         dbo.Producto_Familia.Descripcion AS Familia, dbo.Producto_Familia_Simbolo.ID_Producto_Familia_Simbolo, dbo.Producto_Familia.ID_Producto_Familia, 
                         dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento_Planta, dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento, 
                         dbo.Propuesta_Linea.ID_Producto_SubFamilia_Traspaso, dbo.Propuesta_Linea.Activo, dbo.Propuesta_Linea.MotivoEliminacion, 
                         dbo.Propuesta_Linea.ID_Propuesta_Linea_Estado, dbo.Propuesta_Linea.ID_Producto, dbo.Producto_Subfamilia_Tipo.ID_Producto_Subfamilia_Tipo,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.Parte_Reparacion AS PR INNER JOIN
                                                         dbo.Parte ON PR.ID_Parte = dbo.Parte.ID_Parte
                               WHERE        (PR.ID_Propuesta_Linea = dbo.Propuesta_Linea.ID_Propuesta_Linea) AND (dbo.Parte.Activo = 1)) AS NumPartes, dbo.Propuesta_Linea.BocaConexion, 
                         dbo.Propuesta_Linea.NickZona, dbo.Propuesta.SeInstalo, dbo.Propuesta.ID_Instalacion, dbo.Instalacion.Poblacion AS InstalacionPoblacion, 
                         dbo.Cliente.Nombre AS ClienteNombre, dbo.Instalacion_Estado.Descripcion AS InstalacionEstado, dbo.Propuesta_Linea.NumSerie, 
                         dbo.Propuesta_Linea.ID_Proveedor, dbo.Propuesta_Linea.DescripcionAmpliada, dbo.Propuesta_Linea.TotalIVA, dbo.Propuesta_Linea.TotalLinea, 
                         dbo.Producto_Familia.Conectable, dbo.Propuesta_Linea.RutaOrden, dbo.Propuesta_Linea.RutaParametros, '' AS Foto, dbo.Propuesta_Linea.PrecioCoste, 
                         dbo.Propuesta_Linea.PrecioCoste * dbo.Propuesta_Linea.Unidad AS TotalPrecioCoste, 
                         dbo.Propuesta_Linea.TotalBase - dbo.Propuesta_Linea.PrecioCoste * dbo.Propuesta_Linea.Unidad AS Margen, dbo.Propuesta_Linea.Particion, 
                         dbo.Propuesta_Linea.ReferenciaMemoria, dbo.Producto_SubFamilia_Traspaso.Descripcion AS Traspasable, 
                         dbo.Instalacion_InstaladoEn.Descripcion AS InstaladoEn, dbo.Instalacion_ElementosAProteger.ID_Instalacion_ElementosAProteger, 
                         dbo.Instalacion_Emplazamiento_Abertura.ID_Instalacion_Emplazamiento_Abertura, dbo.Instalacion_InstaladoEn.ID_Instalacion_InstaladoEn, 
                         dbo.Propuesta_Linea.VLAN, dbo.Propuesta_Linea.IP, dbo.Propuesta_Linea.MascaraSubred, dbo.Propuesta_Linea.PuertaEnlace, dbo.Propuesta_Linea.DNSPrimaria, 
                         dbo.Propuesta_Linea.DNSSecundaria, dbo.Propuesta_Linea.IPPublica, dbo.Propuesta_Linea.ServidorWINS, dbo.Propuesta_Linea.Dominio, 
                         dbo.Propuesta_Linea.NombreEquipo, dbo.Propuesta_Linea.NetBios, dbo.Propuesta_Linea.ID_SistemaOperativo, dbo.Propuesta_Linea.MemoriaRam, 
                         dbo.Propuesta_Linea.Procesador, dbo.SistemaOperativo.Descripcion AS SistemaOperativo, dbo.Propuesta_Linea.MacAdress, dbo.Instalacion.OtrosDetalles, 
                         dbo.C_Propuesta_Linea_Albaranada.ID_Entrada, dbo.C_Propuesta_Linea_Albaranada.Codigo AS CodigoDocumentoRelacionado, 
                         dbo.Propuesta_Linea.ID_Entrada_Linea, dbo.Instalacion.ID_Cliente, dbo.Entrada.Codigo, dbo.Propuesta_Opcion.Nombre AS Opción, 
                         dbo.Propuesta_Opcion_Accion.Descripcion AS [Opción acción], dbo.Propuesta_Linea.ImporteOpcion AS [Opción importe], dbo.Propuesta_Linea.PlazoEntrega
FROM            dbo.Producto_Subfamilia_Tipo INNER JOIN
                         dbo.Producto_SubFamilia ON 
                         dbo.Producto_Subfamilia_Tipo.ID_Producto_Subfamilia_Tipo = dbo.Producto_SubFamilia.ID_Producto_Subfamilia_Tipo RIGHT OUTER JOIN
                         dbo.Instalacion_Emplazamiento_Abertura_Elemento INNER JOIN
                         dbo.Instalacion_Emplazamiento_Abertura ON 
                         dbo.Instalacion_Emplazamiento_Abertura_Elemento.ID_Instalacion_Emplazamiento_Abertura_Elemento = dbo.Instalacion_Emplazamiento_Abertura.ID_Instalacion_Emplazamiento_Abertura_Elemento
                          RIGHT OUTER JOIN
                         dbo.Propuesta INNER JOIN
                         dbo.Propuesta_Linea INNER JOIN
                         dbo.Producto ON dbo.Propuesta_Linea.ID_Producto = dbo.Producto.ID_Producto INNER JOIN
                         dbo.Producto_Division ON dbo.Producto.ID_Producto_Division = dbo.Producto_Division.ID_Producto_Division ON 
                         dbo.Propuesta.ID_Propuesta = dbo.Propuesta_Linea.ID_Propuesta INNER JOIN
                         dbo.Instalacion ON dbo.Propuesta.ID_Instalacion = dbo.Instalacion.ID_Instalacion INNER JOIN
                         dbo.Cliente ON dbo.Instalacion.ID_Cliente = dbo.Cliente.ID_Cliente INNER JOIN
                         dbo.Instalacion_Estado ON dbo.Instalacion.ID_Instalacion_Estado = dbo.Instalacion_Estado.ID_Instalacion_Estado INNER JOIN
                         dbo.Propuesta_Linea_Estado ON dbo.Propuesta_Linea.ID_Propuesta_Linea_Estado = dbo.Propuesta_Linea_Estado.ID_Propuesta_Linea_Estado INNER JOIN
                         dbo.Producto_SubFamilia_Traspaso ON 
                         dbo.Propuesta_Linea.ID_Producto_SubFamilia_Traspaso = dbo.Producto_SubFamilia_Traspaso.ID_Producto_SubFamilia_Traspaso LEFT OUTER JOIN
                         dbo.Propuesta_Opcion_Accion INNER JOIN
                         dbo.Propuesta_Opcion ON dbo.Propuesta_Opcion_Accion.ID_Propuesta_Opcion_Accion = dbo.Propuesta_Opcion.ID_Propuesta_Opcion_Accion ON 
                         dbo.Propuesta_Linea.ID_Propuesta_Opcion = dbo.Propuesta_Opcion.ID_Propuesta_Opcion LEFT OUTER JOIN
                         dbo.Propuesta AS Propuesta_1 ON dbo.Propuesta_Linea.ID_Propuesta_Antigua = Propuesta_1.ID_Propuesta LEFT OUTER JOIN
                         dbo.Propuesta_Linea AS Propuesta_Linea_1 ON dbo.Propuesta_Linea.ID_Propuesta_Linea_Vinculado = Propuesta_Linea_1.ID_Propuesta_Linea LEFT OUTER JOIN
                         dbo.Entrada INNER JOIN
                         dbo.Entrada_Linea ON dbo.Entrada.ID_Entrada = dbo.Entrada_Linea.ID_Entrada ON 
                         dbo.Propuesta_Linea.ID_Entrada_Linea = dbo.Entrada_Linea.ID_Entrada_Linea ON 
                         dbo.Instalacion_Emplazamiento_Abertura.ID_Instalacion_Emplazamiento_Abertura = dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento_Abertura LEFT OUTER JOIN
                         dbo.Instalacion_ElementosAProteger_Tipo INNER JOIN
                         dbo.Instalacion_ElementosAProteger ON 
                         dbo.Instalacion_ElementosAProteger_Tipo.ID_Instalacion_ElementosAProteger_Tipo = dbo.Instalacion_ElementosAProteger.ID_Instalacion_ElementosAProteger_Tipo
                          ON dbo.Propuesta_Linea.ID_Instalacion_ElementosAProteger = dbo.Instalacion_ElementosAProteger.ID_Instalacion_ElementosAProteger LEFT OUTER JOIN
                         dbo.Instalacion_Emplazamiento ON 
                         dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento = dbo.Instalacion_Emplazamiento.ID_Instalacion_Emplazamiento LEFT OUTER JOIN
                         dbo.Instalacion_Emplazamiento_Zona ON 
                         dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento_Zona = dbo.Instalacion_Emplazamiento_Zona.ID_Instalacion_Emplazamiento_Zona LEFT OUTER JOIN
                         dbo.Instalacion_Emplazamiento_Planta ON 
                         dbo.Propuesta_Linea.ID_Instalacion_Emplazamiento_Planta = dbo.Instalacion_Emplazamiento_Planta.ID_Instalacion_Emplazamiento_Planta LEFT OUTER JOIN
                         dbo.C_Propuesta_Linea_Albaranada ON dbo.Propuesta_Linea.ID_Propuesta_Linea = dbo.C_Propuesta_Linea_Albaranada.ID_Propuesta_Linea LEFT OUTER JOIN
                         dbo.SistemaOperativo ON dbo.Propuesta_Linea.ID_SistemaOperativo = dbo.SistemaOperativo.ID_SistemaOperativo LEFT OUTER JOIN
                         dbo.Instalacion_InstaladoEn ON dbo.Propuesta_Linea.ID_Instalacion_InstaladoEn = dbo.Instalacion_InstaladoEn.ID_Instalacion_InstaladoEn ON 
                         dbo.Producto_SubFamilia.ID_Producto_SubFamilia = dbo.Producto.ID_Producto_SubFamilia LEFT OUTER JOIN
                         dbo.Producto_Familia_Simbolo RIGHT OUTER JOIN
                         dbo.Producto_Familia ON dbo.Producto_Familia_Simbolo.ID_Producto_Familia_Simbolo = dbo.Producto_Familia.ID_Producto_Familia_Simbolo ON 
                         dbo.Producto.ID_Producto_Familia = dbo.Producto_Familia.ID_Producto_Familia
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[RetornaStock_Soporte]'
GO
ALTER Function [dbo].[RetornaStock_Soporte](@pIDProducto int, @pIDAlmacen int)
Returns @Taula Table
(
ID_Producto int, ID_Almacen int, StockReal Decimal(10,2), StockTeorico decimal(10,2), ProductoCodigo  nvarchar(100), ProductoDescripcion nvarchar(250), AlmacenDescripcion nvarchar(250), PVD Decimal(10,2), PVP Decimal(10,2)
)
as 
Begin
/*
Declare @_FiltreIDProducto nvarchar(50)
if @pIDProducto = 0
begin
	set	@_FiltreIDProducto= ''
end
else
	set	@_FiltreIDProducto= ' and ID_Producto=' & @pIDArticulo
*/


Insert into @Taula 
Select ID_Producto, ID_Almacen, 0, SUM(Unidad) - ISNULL(SUM(CantidadTraspasada), 0) AS StockReal, codigo, Descripcion, Almacen, PVD, PVP From C_Stock_Soporte_TodasLasLineas WHERE ID_Entrada_Tipo = 1  AND ID_Entrada_Estado IN (1, 2) and (@pIDProducto=0 or ID_Producto=@pIDProducto) and (@pIDAlmacen=0 or ID_Almacen=@pIDAlmacen)  Group by ID_Producto, ID_Almacen, codigo, Descripcion, Almacen, PVD, PVP
Union All
SELECT ID_Producto, ID_Almacen, SUM(Unidad) AS StockReal, 0, codigo, Descripcion, Almacen, PVD, PVP FROM C_Stock_Soporte_TodasLasLineas WHERE (ID_Entrada_Tipo = 2) and (@pIDProducto=0 or ID_Producto=@pIDProducto) and (@pIDAlmacen=0 or ID_Almacen=@pIDAlmacen)  GROUP BY ID_Producto, ID_Almacen, codigo, Descripcion, Almacen, PVD, PVP
Union All
Select ID_Producto, ID_Almacen, 0, (SUM(Unidad) - ISNULL(SUM(CantidadTraspasada), 0))*-1 AS StockReal, codigo, Descripcion, Almacen, PVD, PVP From C_Stock_Soporte_TodasLasLineas WHERE ID_Entrada_Tipo = 7  AND ID_Entrada_Estado IN (1, 2) and (@pIDProducto=0 or ID_Producto=@pIDProducto) and (@pIDAlmacen=0 or ID_Almacen=@pIDAlmacen)  Group by ID_Producto, ID_Almacen, codigo, Descripcion, Almacen, PVD, PVP
Union All
SELECT ID_Producto, ID_Almacen, SUM(Unidad)*-1  AS StockReal, 0, codigo, Descripcion, Almacen, PVD, PVP FROM C_Stock_Soporte_TodasLasLineas WHERE (ID_Entrada_Tipo = 8)  and (@pIDProducto=0 or ID_Producto=@pIDProducto) and (@pIDAlmacen=0 or ID_Almacen=@pIDAlmacen)  GROUP BY ID_Producto, ID_Almacen, codigo, Descripcion, Almacen, PVD, PVP
Union All
SELECT ID_Producto, ID_Almacen, (SUM(Unidad)) AS StockReal, 0, codigo, Descripcion, Almacen, PVD, PVP FROM C_Stock_Soporte_TodasLasLineas WHERE (ID_Entrada_Tipo = 5)  and (@pIDProducto=0 or ID_Producto=@pIDProducto) and (@pIDAlmacen=0 or ID_Almacen=@pIDAlmacen)  GROUP BY ID_Producto, ID_Almacen, codigo, Descripcion, Almacen, PVD, PVP
Union All
SELECT ID_Producto, ID_Almacen, (SUM(Unidad)) AS StockReal, 0, codigo, Descripcion, Almacen, PVD, PVP FROM C_Stock_Soporte_TodasLasLineas WHERE (ID_Entrada_Tipo = 10)  and (@pIDProducto=0 or ID_Producto=@pIDProducto) and (@pIDAlmacen=0 or ID_Almacen=@pIDAlmacen)  GROUP BY ID_Producto, ID_Almacen, codigo, Descripcion, Almacen, PVD, PVP
Union All
SELECT ID_Producto, ID_Almacen, (SUM(Unidad))*-1 AS StockReal, 0, codigo, Descripcion, Almacen, PVD, PVP FROM C_Stock_Soporte_TodasLasLineas WHERE (ID_Entrada_Tipo = 4)  and (@pIDProducto=0 or ID_Producto=@pIDProducto) and (@pIDAlmacen=0 or ID_Almacen=@pIDAlmacen)  GROUP BY ID_Producto, ID_Almacen, codigo, Descripcion, Almacen, PVD, PVP
Union All
SELECT ID_Producto, ID_Almacen, (SUM(Unidad)) AS StockReal, 0, codigo, Descripcion, Almacen, PVD, PVP FROM C_Stock_Soporte_TodasLasLineas WHERE (ID_Entrada_Tipo = 11)  and (@pIDProducto=0 or ID_Producto=@pIDProducto) and (@pIDAlmacen=0 or ID_Almacen=@pIDAlmacen)  GROUP BY ID_Producto, ID_Almacen, codigo, Descripcion, Almacen, PVD, PVP
Return
End
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[RetornaStock]'
GO
ALTER Function [dbo].[RetornaStock](@pIDArticulo int, @pIDAlmacen int)
Returns @Taula Table
(
ID_Producto int, ID_Almacen int, StockReal Decimal(10,2), StockTeorico decimal(10,2), ProductoCodigo  nvarchar(100), ProductoDescripcion nvarchar(250), AlmacenDescripcion nvarchar(250), ImporteStockReal Decimal (12,2), ImporteStockTeorico Decimal (12,2), ImportePVPStockReal Decimal (12,2)
)
as 
Begin
Insert into @Taula 
Select ID_Producto, ID_Almacen,  Sum(StockReal), Sum(StockTeorico), ProductoCodigo, ProductoDescripcion,AlmacenDescripcion, Sum(StockReal)*PVD as ImporteStockReal, Sum(StockTeorico)*PVD*-1 as ImporteStockTeorico, Sum(StockReal)*PVP as ImportePVPStockReal  From RetornaStock_Soporte(@pIDArticulo,@pIDAlmacen) Group by ID_Producto, ID_Almacen, Productocodigo, ProductoDescripcion, AlmacenDescripcion, PVD, PVP Having  (Sum(StockReal)<>0 or Sum(StockTeorico)<>0)
Return
End
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Almacen_ConStock]'
GO
CREATE VIEW dbo.C_Almacen_ConStock
AS
SELECT        AlmacenDescripcion, ID_Almacen
FROM            dbo.RetornaStock(0, 0) AS RetornaStock_1
WHERE        (StockReal > 0)
GROUP BY AlmacenDescripcion, ID_Almacen
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Prioridad]'
GO
CREATE TABLE [dbo].[Prioridad]
(
[ID_Prioridad] [int] NOT NULL IDENTITY(1, 1),
[Descripcion] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Automatismo_Prioridad] on [dbo].[Prioridad]'
GO
ALTER TABLE [dbo].[Prioridad] ADD CONSTRAINT [PK_Automatismo_Prioridad] PRIMARY KEY CLUSTERED  ([ID_Prioridad])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Automatismo]'
GO
CREATE TABLE [dbo].[Automatismo]
(
[ID_Automatismo] [int] NOT NULL,
[ID_Prioridad] [int] NOT NULL,
[ID_ActividadCRM_Tipo] [int] NOT NULL,
[Descripcion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Explicacion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[Activo] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Automatismo_Tipo_Activo] DEFAULT ((1))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Automatismo_Tipo] on [dbo].[Automatismo]'
GO
ALTER TABLE [dbo].[Automatismo] ADD CONSTRAINT [PK_ActividadCRM_Automatismo_Tipo] PRIMARY KEY CLUSTERED  ([ID_Automatismo])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[ActividadCRM]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_ActividadCRM]
(
[ID_ActividadCRM] [int] NOT NULL IDENTITY(1, 1),
[ID_Cliente] [int] NULL,
[ID_Instalacion] [int] NULL,
[ID_Propuesta] [int] NULL,
[ID_Personal] [int] NOT NULL,
[ID_ActividadCRM_Tipo] [int] NOT NULL,
[ID_Automatismo] [int] NULL,
[ID_Prioridad] [int] NOT NULL,
[FechaAlta] [smalldatetime] NOT NULL,
[FechaVencimiento] [smalldatetime] NULL,
[Realizado] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Realizado] DEFAULT ((0)),
[PorcentajeRealizado] [decimal] (5, 2) NULL,
[Asunto] [nvarchar] (500) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Descripcion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[Activo] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Activo] DEFAULT ((1))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ActividadCRM] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_ActividadCRM]([ID_ActividadCRM], [ID_Cliente], [ID_Instalacion], [ID_Propuesta], [ID_Personal], [ID_ActividadCRM_Tipo], [ID_Automatismo], [FechaAlta], [FechaVencimiento], [Realizado], [Asunto], [Descripcion], [Activo]) SELECT [ID_ActividadCRM], [ID_Cliente], [ID_Instalacion], [ID_Propuesta], [ID_Personal], [ID_ActividadCRM_Tipo], [Prioridad], [FechaAlta], [FechaVencimiento], [Realizado], [Asunto], [Observaciones], [Activo] FROM [dbo].[ActividadCRM]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_ActividadCRM] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[ActividadCRM]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_ActividadCRM]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[ActividadCRM]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_ActividadCRM]', N'ActividadCRM'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM] on [dbo].[ActividadCRM]'
GO
ALTER TABLE [dbo].[ActividadCRM] ADD CONSTRAINT [PK_ActividadCRM] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM])
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
                         dbo.ActividadCRM.ID_ActividadCRM_Tipo, dbo.ActividadCRM.FechaAlta, dbo.ActividadCRM.FechaVencimiento, dbo.ActividadCRM.Realizado, 
                         dbo.ActividadCRM.Asunto, dbo.ActividadCRM.Activo, dbo.Propuesta.Codigo AS PropuestaCodigo, dbo.Propuesta.Version AS PropuestaVersion, 
                         dbo.Propuesta.Descripcion AS PropuestaDescripcion, dbo.ActividadCRM_Tipo.Descripcion AS ActividadTipo, dbo.Cliente.Codigo AS ClienteCodigo, 
                         dbo.Cliente.Nombre AS ClienteNombre, dbo.Cliente.NombreComercial AS ClienteNombreComercial, dbo.Personal.Nombre AS Propietario, 
                         dbo.ActividadCRM.ID_Personal, dbo.ActividadCRM.Descripcion, dbo.ActividadCRM.PorcentajeRealizado, dbo.ActividadCRM.ID_Automatismo, 
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
PRINT N'Creating [dbo].[Automatismo_Accion]'
GO
CREATE TABLE [dbo].[Automatismo_Accion]
(
[ID_Automatismo_Accion] [int] NOT NULL IDENTITY(1, 1),
[ID_Automatismo] [int] NOT NULL,
[ID_Prioridad] [int] NOT NULL,
[ID_ActividadCRM_Accion_Tipo] [int] NOT NULL,
[Descripcion] [nvarchar] (500) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Explicacion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Automatismo_Accion] on [dbo].[Automatismo_Accion]'
GO
ALTER TABLE [dbo].[Automatismo_Accion] ADD CONSTRAINT [PK_Automatismo_Accion] PRIMARY KEY CLUSTERED  ([ID_Automatismo_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ActividadCRM_Accion_Tipo]'
GO
CREATE TABLE [dbo].[ActividadCRM_Accion_Tipo]
(
[ID_ActividadCRM_Accion_Tipo] [int] NOT NULL IDENTITY(1, 1),
[Codigo] [int] NOT NULL,
[Descripcion] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Activo] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Accion_Tipo_Activo] DEFAULT ((1)),
[RO] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Accion_Tipo_RO] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Accion_Tipo] on [dbo].[ActividadCRM_Accion_Tipo]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Tipo] ADD CONSTRAINT [PK_ActividadCRM_Accion_Tipo] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM_Accion_Tipo])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ActividadCRM_Accion]'
GO
CREATE TABLE [dbo].[ActividadCRM_Accion]
(
[ID_ActividadCRM_Accion] [int] NOT NULL IDENTITY(1, 1),
[ID_ActividadCRM] [int] NOT NULL,
[ID_ActividadCRM_Accion_Tipo] [int] NOT NULL,
[ID_Personal] [int] NOT NULL,
[ID_Automatismo_Accion] [int] NULL,
[ID_Prioridad] [int] NOT NULL,
[Descripcion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[FechaAlta] [smalldatetime] NULL,
[FechaAviso] [smalldatetime] NULL,
[HoraAviso] [smalldatetime] NULL,
[Finalizada] [bit] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Accion] on [dbo].[ActividadCRM_Accion]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [PK_ActividadCRM_Accion] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_ActividadCRM_Acciones]'
GO
CREATE VIEW dbo.C_ActividadCRM_Acciones
AS
SELECT        dbo.ActividadCRM_Accion.ID_ActividadCRM_Accion, dbo.ActividadCRM_Accion.ID_ActividadCRM, dbo.ActividadCRM_Accion.ID_ActividadCRM_Accion_Tipo, 
                         dbo.ActividadCRM_Accion.ID_Personal, dbo.ActividadCRM_Accion.Descripcion, dbo.ActividadCRM_Accion.FechaAlta, dbo.ActividadCRM_Accion.FechaAviso, 
                         dbo.ActividadCRM_Accion.HoraAviso, dbo.ActividadCRM_Accion.Finalizada, dbo.ActividadCRM_Accion_Tipo.Descripcion AS TipoAccion, 
                         dbo.Personal.Nombre AS Propietario, dbo.Prioridad.Descripcion AS Prioridad, dbo.ActividadCRM_Accion.ID_Prioridad, 
                         dbo.ActividadCRM_Accion.ID_Automatismo_Accion, dbo.Automatismo_Accion.Descripcion AS [Acción del automatismo]
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
PRINT N'Altering [dbo].[C_LADV_Documentos_AlbaranesDeCompra_Parciales]'
GO
ALTER VIEW dbo.C_LADV_Documentos_AlbaranesDeCompra_Parciales
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 2) AND (ID_Entrada_Tipo = 2)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Automatismo]'
GO
CREATE VIEW dbo.C_Automatismo
AS
SELECT        dbo.Automatismo.ID_Automatismo, dbo.Automatismo.ID_ActividadCRM_Tipo, dbo.Automatismo.ID_Prioridad, dbo.Automatismo.Descripcion, dbo.Automatismo.Activo, 
                         dbo.Prioridad.Descripcion AS Prioridad, dbo.ActividadCRM_Tipo.Descripcion AS [Tipo de actividad]
FROM            dbo.Automatismo INNER JOIN
                         dbo.ActividadCRM_Tipo ON dbo.Automatismo.ID_ActividadCRM_Tipo = dbo.ActividadCRM_Tipo.ID_ActividadCRM_Tipo INNER JOIN
                         dbo.Prioridad ON dbo.Automatismo.ID_Prioridad = dbo.Prioridad.ID_Prioridad
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_AlbaranesDeCompra_Cerrados]'
GO
ALTER VIEW dbo.C_LADV_Documentos_AlbaranesDeCompra_Cerrados
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 3) AND (ID_Entrada_Tipo = 2)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Automatismo_Accion]'
GO
CREATE VIEW dbo.C_Automatismo_Accion
AS
SELECT        dbo.Automatismo_Accion.ID_Automatismo_Accion, dbo.Automatismo_Accion.ID_Automatismo, dbo.Automatismo_Accion.ID_Prioridad, 
                         dbo.Automatismo_Accion.ID_ActividadCRM_Accion_Tipo, dbo.Automatismo_Accion.Descripcion, dbo.Prioridad.Descripcion AS Prioridad, 
                         dbo.ActividadCRM_Accion_Tipo.Descripcion AS [Tipo de acción]
FROM            dbo.Automatismo_Accion INNER JOIN
                         dbo.Prioridad ON dbo.Automatismo_Accion.ID_Prioridad = dbo.Prioridad.ID_Prioridad INNER JOIN
                         dbo.ActividadCRM_Accion_Tipo ON dbo.Automatismo_Accion.ID_ActividadCRM_Accion_Tipo = dbo.ActividadCRM_Accion_Tipo.ID_ActividadCRM_Accion_Tipo AND 
                         dbo.Automatismo_Accion.ID_ActividadCRM_Accion_Tipo = dbo.ActividadCRM_Accion_Tipo.ID_ActividadCRM_Accion_Tipo
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_FacturasDeCompra_Cerradas]'
GO
ALTER VIEW dbo.C_LADV_Documentos_FacturasDeCompra_Cerradas
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 3) AND (ID_Entrada_Tipo = 3)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_AlbaranesDeVenta_Pendientes]'
GO
ALTER VIEW dbo.C_LADV_Documentos_AlbaranesDeVenta_Pendientes
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 1) AND (ID_Entrada_Tipo = 8)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_DeVentaYCompra_ConSeguimientosPendientesDeRealizar]'
GO
ALTER VIEW dbo.C_LADV_Documentos_DeVentaYCompra_ConSeguimientosPendientesDeRealizar
AS
SELECT        TOP (100) PERCENT dbo.C_Entrada.ID_Entrada, dbo.C_Entrada.Codigo, dbo.C_Entrada.Descripcion, dbo.C_Entrada.FechaEntrada AS [Fecha documento], 
                         dbo.C_Entrada.[Código del proveedor], dbo.C_Entrada.[Nombre del proveedor], dbo.C_Entrada.[Número de documento proveedor], dbo.C_Entrada.Base, 
                         dbo.C_Entrada.IVA, dbo.C_Entrada.Descuento, dbo.C_Entrada.Total, dbo.Entrada.DocumentoImpreso
FROM            dbo.C_Entrada INNER JOIN
                         dbo.Entrada ON dbo.C_Entrada.ID_Entrada = dbo.Entrada.ID_Entrada
WHERE        (dbo.C_Entrada.ID_Entrada_Tipo IN (1, 2, 3, 7, 8, 9)) AND
                             ((SELECT        COUNT(*) AS Expr1
                                 FROM            dbo.Entrada_Seguimiento
                                 WHERE        (ID_Entrada = dbo.C_Entrada.ID_Entrada) AND (Realizado = 0)) > 0)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_AlbaranesDeCompra_Pendientes]'
GO
ALTER VIEW dbo.C_LADV_Documentos_AlbaranesDeCompra_Pendientes
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 1) AND (ID_Entrada_Tipo = 2)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_AlbaranesDeVenta_Parciales]'
GO
ALTER VIEW dbo.C_LADV_Documentos_AlbaranesDeVenta_Parciales
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 2) AND (ID_Entrada_Tipo = 8)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_AlbaranesDeVenta_Cerrados]'
GO
ALTER VIEW dbo.C_LADV_Documentos_AlbaranesDeVenta_Cerrados
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 3) AND (ID_Entrada_Tipo = 8)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_PedidosDeVenta_Parciales]'
GO
ALTER VIEW dbo.C_LADV_Documentos_PedidosDeVenta_Parciales
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 2) AND (ID_Entrada_Tipo = 7)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_FacturasDeVenta_Pendientes]'
GO
ALTER VIEW dbo.C_LADV_Documentos_FacturasDeVenta_Pendientes
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 1) AND (ID_Entrada_Tipo = 9)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_FacturasDeVenta_Cerradas]'
GO
ALTER VIEW dbo.C_LADV_Documentos_FacturasDeVenta_Cerradas
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 3) AND (ID_Entrada_Tipo = 9)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_FacturasDeVenta_PendientesDeCobrar]'
GO
ALTER VIEW dbo.C_LADV_Documentos_FacturasDeVenta_PendientesDeCobrar
AS
SELECT        ID_Entrada, Codigo AS Código, Descripcion, FechaEntrada AS [Fecha documento], [Código de Cliente], [Nombre del cliente], [Dirección del cliente], 
                         [Población del cliente], [Povincia del cliente], [Persona de contacto], Télefono, [N.I.F], [Comercial asignado], [Número de pedido del cliente], [Número de referencia], 
                         Responsable, Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Tipo = 9) AND (ID_Entrada_Estado = 1) AND
                             ((SELECT        COUNT(*) AS Expr1
                                 FROM            dbo.Entrada_Vencimiento
                                 WHERE        (ID_Entrada = dbo.C_Entrada.ID_Entrada) AND (Pagado = 0)) > 0)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_FacturasDeVenta_NoImpresas]'
GO
ALTER VIEW dbo.C_LADV_Documentos_FacturasDeVenta_NoImpresas
AS
SELECT        TOP (100) PERCENT dbo.C_Entrada.ID_Entrada, dbo.C_Entrada.Codigo, dbo.C_Entrada.Descripcion, dbo.C_Entrada.FechaEntrada AS [Fecha documento], 
                         dbo.C_Entrada.[Código del proveedor], dbo.C_Entrada.[Nombre del proveedor], dbo.C_Entrada.[Número de documento proveedor], dbo.C_Entrada.Base, 
                         dbo.C_Entrada.IVA, dbo.C_Entrada.Descuento, dbo.C_Entrada.Total, dbo.Entrada.DocumentoImpreso
FROM            dbo.C_Entrada INNER JOIN
                         dbo.Entrada ON dbo.C_Entrada.ID_Entrada = dbo.Entrada.ID_Entrada
WHERE        (dbo.C_Entrada.ID_Entrada_Tipo = 9) AND (dbo.Entrada.DocumentoImpreso = 0)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_PedidosDeCompra_Cerrados]'
GO
ALTER VIEW dbo.C_LADV_Documentos_PedidosDeCompra_Cerrados
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 3) AND (ID_Entrada_Tipo = 1)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[logTransacciones]'
GO
ALTER TABLE [dbo].[logTransacciones] ALTER COLUMN [PK] [bigint] NULL
ALTER TABLE [dbo].[logTransacciones] ALTER COLUMN [PKPadre] [bigint] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_FacturasDeCompra_Pendientes]'
GO
ALTER VIEW dbo.C_LADV_Documentos_FacturasDeCompra_Pendientes
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 1) AND (ID_Entrada_Tipo = 3)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_PedidosDeVenta_Pendientes]'
GO
ALTER VIEW dbo.C_LADV_Documentos_PedidosDeVenta_Pendientes
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 1) AND (ID_Entrada_Tipo = 7)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_PedidosDeCompra_Pendientes]'
GO
ALTER VIEW dbo.C_LADV_Documentos_PedidosDeCompra_Pendientes
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 1) AND (ID_Entrada_Tipo = 1)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_PedidosDeCompra_Parciales]'
GO
ALTER VIEW dbo.C_LADV_Documentos_PedidosDeCompra_Parciales
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 2) AND (ID_Entrada_Tipo = 1)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Documentos_PedidosDeVenta_Cerrados]'
GO
ALTER VIEW dbo.C_LADV_Documentos_PedidosDeVenta_Cerrados
AS
SELECT        TOP (100) PERCENT ID_Entrada, Codigo, Descripcion, FechaEntrada AS [Fecha documento], [Código del proveedor], [Nombre del proveedor], 
                         [Número de documento proveedor], Base, IVA, Descuento, Total
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Estado = 3) AND (ID_Entrada_Tipo = 7)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Bono]'
GO
ALTER TABLE [dbo].[Bono] ADD
[ID_Entrada] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Facturas_de_venta]'
GO
ALTER VIEW dbo.C_LADV_Facturas_de_venta
AS
SELECT        ID_Entrada, Codigo, Descripcion, FechaEntrada, ID_Entrada_Estado, Estado, ID_Entrada_Tipo, [Almacén de destino], ID_Almacen, [Tiop de documento], Almacén, 
                         [Código de Cliente], [Nombre del cliente], ID_Cliente, Télefono, [Povincia del cliente], [Persona de contacto], [Dirección del cliente], [Población del cliente], 
                         CASE ID_Entrada_Tipo WHEN 9 THEN Base WHEN 12 THEN Base * - 1 END AS Base, IVA, Descuento
FROM            dbo.C_Entrada
WHERE        (ID_Entrada_Tipo IN (9, 12))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Parte_Horas]'
GO
ALTER VIEW dbo.C_Parte_Horas
AS
SELECT        dbo.Parte_Horas.ID_Parte_Horas, dbo.Parte_Horas.ID_Parte, dbo.Parte_Horas.ID_Personal, dbo.Parte_Horas.Fecha, dbo.Parte_Horas.Horas, 
                         dbo.Parte_Horas.HorasExtras, dbo.Parte_Horas.ParteFirmado, dbo.Parte_Horas.DescripcionTrabajo, dbo.Parte_Horas.ErrorDelTecnico, 
                         dbo.Parte_Horas.ErrorDeOtroTecnico, dbo.Parte_Horas.ID_Parte_Horas_Estado, dbo.Parte_Horas.ObservacionesIncorrecto, 
                         dbo.Parte_Horas.ID_Instalacion_Emplazamiento, dbo.Parte_Horas.ID_Instalacion_Emplazamiento_Planta, dbo.Parte_Horas.ID_Instalacion_Emplazamiento_Zona, 
                         dbo.Parte_Horas.ID_Instalacion_Emplazamiento_Abertura, dbo.Parte_Horas.ID_Instalacion_ElementosAProteger, dbo.Parte_Horas.Pendiente, 
                         dbo.Parte_Horas.ID_Entrada_Linea, dbo.Personal.Nombre AS Personal_Nombre, dbo.Parte_Horas_Estado.Descripcion AS Parte_Estado, 
                         dbo.Parte.TrabajoARealizar, dbo.Parte_Horas_TipoActuacion.Descripcion, dbo.Entrada.Codigo AS [Código de documento], dbo.Cliente.Codigo AS [Código cliente], 
                         dbo.Cliente.Nombre AS [Nombre del cliente], dbo.Instalacion.ID_Instalacion, dbo.Instalacion.OtrosDetalles AS [Descripción de la instalación], 
                         dbo.Parte.Poblacion AS Población, dbo.Parte.QuienDetectoIncidencia AS [Quién detecto la incidéncia]
FROM            dbo.Parte_Horas INNER JOIN
                         dbo.Personal ON dbo.Parte_Horas.ID_Personal = dbo.Personal.ID_Personal INNER JOIN
                         dbo.Parte_Horas_Estado ON dbo.Parte_Horas.ID_Parte_Horas_Estado = dbo.Parte_Horas_Estado.ID_Parte_Horas_Estado INNER JOIN
                         dbo.Parte ON dbo.Parte_Horas.ID_Parte = dbo.Parte.ID_Parte INNER JOIN
                         dbo.Cliente INNER JOIN
                         dbo.Instalacion ON dbo.Cliente.ID_Cliente = dbo.Instalacion.ID_Cliente ON dbo.Parte.ID_Instalacion = dbo.Instalacion.ID_Instalacion LEFT OUTER JOIN
                         dbo.Entrada INNER JOIN
                         dbo.Entrada_Linea ON dbo.Entrada.ID_Entrada = dbo.Entrada_Linea.ID_Entrada ON 
                         dbo.Parte_Horas.ID_Entrada_Linea = dbo.Entrada_Linea.ID_Entrada_Linea LEFT OUTER JOIN
                         dbo.Parte_Horas_TipoActuacion ON dbo.Parte_Horas.ID_Parte_Horas_TipoActuacion = dbo.Parte_Horas_TipoActuacion.ID_Parte_Horas_TipoActuacion
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Aviso]'
GO
CREATE TABLE [dbo].[Aviso]
(
[ID_Aviso] [int] NOT NULL IDENTITY(1, 1),
[ID_ActividadCRM] [int] NOT NULL,
[ID_ActividadCRM_Accion] [int] NULL,
[ID_Personal_Origen] [int] NULL,
[ID_Personal_Destino] [int] NOT NULL,
[ID_Prioridad] [int] NOT NULL,
[FechaAviso] [smalldatetime] NOT NULL,
[Asunto] [nvarchar] (400) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Leido] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Aviso_Leido] DEFAULT ((0)),
[FechaLeido] [smalldatetime] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Aviso] on [dbo].[Aviso]'
GO
ALTER TABLE [dbo].[Aviso] ADD CONSTRAINT [PK_ActividadCRM_Aviso] PRIMARY KEY CLUSTERED  ([ID_Aviso])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ActividadCRM_Personal]'
GO
CREATE TABLE [dbo].[ActividadCRM_Personal]
(
[ID_ActividadCRM_Personal] [int] NOT NULL IDENTITY(1, 1),
[ID_ActividadCRM] [int] NOT NULL,
[ID_Personal] [int] NOT NULL,
[Leido] [bit] NOT NULL CONSTRAINT [DF_ActividadCRM_Personal_Leido] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Personal] on [dbo].[ActividadCRM_Personal]'
GO
ALTER TABLE [dbo].[ActividadCRM_Personal] ADD CONSTRAINT [PK_ActividadCRM_Personal] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM_Personal])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Producto_Proveedor]'
GO
ALTER TABLE [dbo].[Producto_Proveedor] ADD
[PlazoEntrega] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ActividadCRM_Aux]'
GO
CREATE TABLE [dbo].[ActividadCRM_Aux]
(
[ID_ActividadCRM_Aux] [int] NOT NULL,
[DescripcionRTF] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[Explicacion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[Observaciones] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[Hoja] [varbinary] (max) NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Actividad_CRM_Aux] on [dbo].[ActividadCRM_Aux]'
GO
ALTER TABLE [dbo].[ActividadCRM_Aux] ADD CONSTRAINT [PK_Actividad_CRM_Aux] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM_Aux])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ActividadCRM_Accion_Archivo]'
GO
CREATE TABLE [dbo].[ActividadCRM_Accion_Archivo]
(
[ID_ActividadCRM_Accion_Archivo] [int] NOT NULL,
[ID_Archivo] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ActividadCRM_Accion_Aux]'
GO
CREATE TABLE [dbo].[ActividadCRM_Accion_Aux]
(
[ID_ActividadCRM_Accion_Aux] [int] NOT NULL,
[DescripcionRTF] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[Explicacion] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[Observaciones] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[Hoja] [varbinary] (max) NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Accion_Aux] on [dbo].[ActividadCRM_Accion_Aux]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Aux] ADD CONSTRAINT [PK_ActividadCRM_Accion_Aux] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM_Accion_Aux])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ActividadCRM_Accion_Personal]'
GO
CREATE TABLE [dbo].[ActividadCRM_Accion_Personal]
(
[ID_ActividadCRM_Accion_Personal] [int] NOT NULL IDENTITY(1, 1),
[ID_ActividadCRM_Accion] [int] NOT NULL,
[ID_Personal] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Accion_Personal] on [dbo].[ActividadCRM_Accion_Personal]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Personal] ADD CONSTRAINT [PK_ActividadCRM_Accion_Personal] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM_Accion_Personal])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ActividadCRM_Chat]'
GO
CREATE TABLE [dbo].[ActividadCRM_Chat]
(
[ID_ActividadCRM_Chat] [int] NOT NULL IDENTITY(1, 1),
[ID_ActividadCRM] [int] NOT NULL,
[ID_Personal_Origen] [int] NOT NULL,
[ID_Personal_Destino] [int] NOT NULL,
[Mensaje] [nvarchar] (500) COLLATE Modern_Spanish_CI_AS NOT NULL,
[FechaAlta] [smalldatetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Chat] on [dbo].[ActividadCRM_Chat]'
GO
ALTER TABLE [dbo].[ActividadCRM_Chat] ADD CONSTRAINT [PK_ActividadCRM_Chat] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM_Chat])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Automatismo_Accion_Personal]'
GO
CREATE TABLE [dbo].[Automatismo_Accion_Personal]
(
[ID_Automatismo_Accion_Personal] [int] NOT NULL IDENTITY(1, 1),
[ID_Automatismo_Accion] [int] NOT NULL,
[ID_Personal] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Automatismo_Accion_Personal] on [dbo].[Automatismo_Accion_Personal]'
GO
ALTER TABLE [dbo].[Automatismo_Accion_Personal] ADD CONSTRAINT [PK_Automatismo_Accion_Personal] PRIMARY KEY CLUSTERED  ([ID_Automatismo_Accion_Personal])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Automatismo_Personal]'
GO
CREATE TABLE [dbo].[Automatismo_Personal]
(
[ID_Automatismo_Personal] [int] NOT NULL IDENTITY(1, 1),
[ID_Automatismo] [int] NOT NULL,
[ID_Personal] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ActividadCRM_Aviso_Automatismo_Personal] on [dbo].[Automatismo_Personal]'
GO
ALTER TABLE [dbo].[Automatismo_Personal] ADD CONSTRAINT [PK_ActividadCRM_Aviso_Automatismo_Personal] PRIMARY KEY CLUSTERED  ([ID_Automatismo_Personal])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Parte_MaterialOperarios]'
GO
ALTER TABLE [dbo].[Parte_MaterialOperarios] ALTER COLUMN [Cantidad] [decimal] (10, 2) NOT NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Accion_Archivo]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Archivo] ADD CONSTRAINT [FK_ActividadCRM_Accion_Archivo_ActividadCRM_Accion] FOREIGN KEY ([ID_ActividadCRM_Accion_Archivo]) REFERENCES [dbo].[ActividadCRM_Accion] ([ID_ActividadCRM_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Accion_Aux]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Aux] ADD CONSTRAINT [FK_ActividadCRM_Accion_Aux_ActividadCRM_Accion] FOREIGN KEY ([ID_ActividadCRM_Accion_Aux]) REFERENCES [dbo].[ActividadCRM_Accion] ([ID_ActividadCRM_Accion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Accion_Personal]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion_Personal] ADD CONSTRAINT [FK_ActividadCRM_Accion_Personal_ActividadCRM_Accion] FOREIGN KEY ([ID_ActividadCRM_Accion]) REFERENCES [dbo].[ActividadCRM_Accion] ([ID_ActividadCRM_Accion])
ALTER TABLE [dbo].[ActividadCRM_Accion_Personal] ADD CONSTRAINT [FK_ActividadCRM_Accion_Personal_Personal] FOREIGN KEY ([ID_Personal]) REFERENCES [dbo].[Personal] ([ID_Personal])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Aviso]'
GO
ALTER TABLE [dbo].[Aviso] ADD CONSTRAINT [FK_Aviso_ActividadCRM_Accion] FOREIGN KEY ([ID_ActividadCRM_Accion]) REFERENCES [dbo].[ActividadCRM_Accion] ([ID_ActividadCRM_Accion])
ALTER TABLE [dbo].[Aviso] ADD CONSTRAINT [FK_Aviso_ActividadCRM] FOREIGN KEY ([ID_ActividadCRM]) REFERENCES [dbo].[ActividadCRM] ([ID_ActividadCRM])
ALTER TABLE [dbo].[Aviso] ADD CONSTRAINT [FK_Aviso_Personal] FOREIGN KEY ([ID_Personal_Origen]) REFERENCES [dbo].[Personal] ([ID_Personal])
ALTER TABLE [dbo].[Aviso] ADD CONSTRAINT [FK_Aviso_Personal1] FOREIGN KEY ([ID_Personal_Destino]) REFERENCES [dbo].[Personal] ([ID_Personal])
ALTER TABLE [dbo].[Aviso] ADD CONSTRAINT [FK_Aviso_Prioridad] FOREIGN KEY ([ID_Prioridad]) REFERENCES [dbo].[Prioridad] ([ID_Prioridad])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Accion]'
GO
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_ActividadCRM] FOREIGN KEY ([ID_ActividadCRM]) REFERENCES [dbo].[ActividadCRM] ([ID_ActividadCRM])
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_ActividadCRM_Accion_Tipo] FOREIGN KEY ([ID_ActividadCRM_Accion_Tipo]) REFERENCES [dbo].[ActividadCRM_Accion_Tipo] ([ID_ActividadCRM_Accion_Tipo])
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_Personal] FOREIGN KEY ([ID_Personal]) REFERENCES [dbo].[Personal] ([ID_Personal])
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_Automatismo_Accion] FOREIGN KEY ([ID_Automatismo_Accion]) REFERENCES [dbo].[Automatismo_Accion] ([ID_Automatismo_Accion])
ALTER TABLE [dbo].[ActividadCRM_Accion] ADD CONSTRAINT [FK_ActividadCRM_Accion_Prioridad] FOREIGN KEY ([ID_Prioridad]) REFERENCES [dbo].[Prioridad] ([ID_Prioridad])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Personal]'
GO
ALTER TABLE [dbo].[ActividadCRM_Personal] ADD CONSTRAINT [FK_ActividadCRM_Personal_ActividadCRM] FOREIGN KEY ([ID_ActividadCRM]) REFERENCES [dbo].[ActividadCRM] ([ID_ActividadCRM])
ALTER TABLE [dbo].[ActividadCRM_Personal] ADD CONSTRAINT [FK_ActividadCRM_Personal_Personal] FOREIGN KEY ([ID_Personal]) REFERENCES [dbo].[Personal] ([ID_Personal])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Automatismo_Accion]'
GO
ALTER TABLE [dbo].[Automatismo_Accion] ADD CONSTRAINT [FK_Automatismo_Accion_ActividadCRM_Accion_Tipo] FOREIGN KEY ([ID_ActividadCRM_Accion_Tipo]) REFERENCES [dbo].[ActividadCRM_Accion_Tipo] ([ID_ActividadCRM_Accion_Tipo])
ALTER TABLE [dbo].[Automatismo_Accion] ADD CONSTRAINT [FK_Automatismo_Accion_Automatismo] FOREIGN KEY ([ID_Automatismo]) REFERENCES [dbo].[Automatismo] ([ID_Automatismo])
ALTER TABLE [dbo].[Automatismo_Accion] ADD CONSTRAINT [FK_Automatismo_Accion_Prioridad] FOREIGN KEY ([ID_Prioridad]) REFERENCES [dbo].[Prioridad] ([ID_Prioridad])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Aux]'
GO
ALTER TABLE [dbo].[ActividadCRM_Aux] ADD CONSTRAINT [FK_Actividad_CRM_Aux_ActividadCRM] FOREIGN KEY ([ID_ActividadCRM_Aux]) REFERENCES [dbo].[ActividadCRM] ([ID_ActividadCRM])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Archivo]'
GO
ALTER TABLE [dbo].[ActividadCRM_Archivo] ADD CONSTRAINT [FK_ActividadCRM_Archivo_ActividadCRM] FOREIGN KEY ([ID_ActividadCRM_Archivo]) REFERENCES [dbo].[ActividadCRM] ([ID_ActividadCRM])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM_Chat]'
GO
ALTER TABLE [dbo].[ActividadCRM_Chat] ADD CONSTRAINT [FK_ActividadCRM_Chat_ActividadCRM] FOREIGN KEY ([ID_ActividadCRM]) REFERENCES [dbo].[ActividadCRM] ([ID_ActividadCRM])
ALTER TABLE [dbo].[ActividadCRM_Chat] ADD CONSTRAINT [FK_ActividadCRM_Chat_Personal] FOREIGN KEY ([ID_Personal_Origen]) REFERENCES [dbo].[Personal] ([ID_Personal])
ALTER TABLE [dbo].[ActividadCRM_Chat] ADD CONSTRAINT [FK_ActividadCRM_Chat_Personal1] FOREIGN KEY ([ID_Personal_Destino]) REFERENCES [dbo].[Personal] ([ID_Personal])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[ActividadCRM]'
GO
ALTER TABLE [dbo].[ActividadCRM] ADD CONSTRAINT [FK_ActividadCRM_Cliente] FOREIGN KEY ([ID_Cliente]) REFERENCES [dbo].[Cliente] ([ID_Cliente])
ALTER TABLE [dbo].[ActividadCRM] ADD CONSTRAINT [FK_ActividadCRM_Instalacion] FOREIGN KEY ([ID_Instalacion]) REFERENCES [dbo].[Instalacion] ([ID_Instalacion])
ALTER TABLE [dbo].[ActividadCRM] ADD CONSTRAINT [FK_ActividadCRM_Propuesta] FOREIGN KEY ([ID_Propuesta]) REFERENCES [dbo].[Propuesta] ([ID_Propuesta])
ALTER TABLE [dbo].[ActividadCRM] ADD CONSTRAINT [FK_ActividadCRM_Personal] FOREIGN KEY ([ID_Personal]) REFERENCES [dbo].[Personal] ([ID_Personal])
ALTER TABLE [dbo].[ActividadCRM] ADD CONSTRAINT [FK_ActividadCRM_ActividadCRM_Tipo] FOREIGN KEY ([ID_ActividadCRM_Tipo]) REFERENCES [dbo].[ActividadCRM_Tipo] ([ID_ActividadCRM_Tipo])
ALTER TABLE [dbo].[ActividadCRM] ADD CONSTRAINT [FK_ActividadCRM_Automatismo] FOREIGN KEY ([ID_Automatismo]) REFERENCES [dbo].[Automatismo] ([ID_Automatismo])
ALTER TABLE [dbo].[ActividadCRM] ADD CONSTRAINT [FK_ActividadCRM_Prioridad] FOREIGN KEY ([ID_Prioridad]) REFERENCES [dbo].[Prioridad] ([ID_Prioridad])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Automatismo_Personal]'
GO
ALTER TABLE [dbo].[Automatismo_Personal] ADD CONSTRAINT [FK_Automatismo_Personal_Automatismo] FOREIGN KEY ([ID_Automatismo]) REFERENCES [dbo].[Automatismo] ([ID_Automatismo])
ALTER TABLE [dbo].[Automatismo_Personal] ADD CONSTRAINT [FK_Automatismo_Personal_Personal] FOREIGN KEY ([ID_Personal]) REFERENCES [dbo].[Personal] ([ID_Personal])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Automatismo]'
GO
ALTER TABLE [dbo].[Automatismo] ADD CONSTRAINT [FK_Automatismo_Prioridad] FOREIGN KEY ([ID_Prioridad]) REFERENCES [dbo].[Prioridad] ([ID_Prioridad])
ALTER TABLE [dbo].[Automatismo] ADD CONSTRAINT [FK_Automatismo_ActividadCRM_Tipo] FOREIGN KEY ([ID_ActividadCRM_Tipo]) REFERENCES [dbo].[ActividadCRM_Tipo] ([ID_ActividadCRM_Tipo])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Automatismo_Accion_Personal]'
GO
ALTER TABLE [dbo].[Automatismo_Accion_Personal] ADD CONSTRAINT [FK_Automatismo_Accion_Personal_Automatismo_Accion] FOREIGN KEY ([ID_Automatismo_Accion]) REFERENCES [dbo].[Automatismo_Accion] ([ID_Automatismo_Accion])
ALTER TABLE [dbo].[Automatismo_Accion_Personal] ADD CONSTRAINT [FK_Automatismo_Accion_Personal_Personal] FOREIGN KEY ([ID_Personal]) REFERENCES [dbo].[Personal] ([ID_Personal])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Bono]'
GO
ALTER TABLE [dbo].[Bono] ADD CONSTRAINT [FK_Bono_Entrada] FOREIGN KEY ([ID_Entrada]) REFERENCES [dbo].[Entrada] ([ID_Entrada])
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
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 137
               Left = 1021
               Bottom = 422
               Right = 1286
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM"
            Begin Extent = 
               Top = 11
               Left = 474
               Bottom = 375
               Right = 684
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM_Tipo"
            Begin Extent = 
               Top = 169
               Left = 55
               Bottom = 298
               Right = 265
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 6
               Left = 722
               Bottom = 135
               Right = 956
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Prioridad"
            Begin Extent = 
               Top = 433
               Left = 81
               Bottom = 528
               Right = 290
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Automatismo"
            Begin Extent = 
               Top = 301
               Left = 25
               Bottom = 430
               Right = 235
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Instalacion"
            Begin Extent = 
               Top = 213
               Left = 774
               Bottom = 458
               Right = 985
            End
', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Propuesta"
            Begin Extent = 
               Top = 10
               Left = 49
               Bottom = 139
               Right = 336
            End
            DisplayFlags = 280
            TopColumn = 8
         End
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
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
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
         Begin Table = "C_Entrada"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
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
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4455
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_LADV_Documentos_AlbaranesDeCompra_Cerrados', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[30] 4[26] 2[8] 3) )"
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
         Begin Table = "C_Entrada"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 383
               Right = 309
            End
            DisplayFlags = 280
            TopColumn = 18
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 24
         Width = 284
         Width = 1500
         Width = 1500
         Width = 3375
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
         Column = 2475
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_LADV_Facturas_de_venta', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[34] 4[42] 2[7] 3) )"
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
         Top = -288
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Parte_Horas"
            Begin Extent = 
               Top = 4
               Left = 587
               Bottom = 417
               Right = 887
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 9
               Left = 1038
               Bottom = 138
               Right = 1272
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Horas_Estado"
            Begin Extent = 
               Top = 173
               Left = 1002
               Bottom = 268
               Right = 1211
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 326
               Right = 371
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 484
               Left = 9
               Bottom = 613
               Right = 274
            End
            DisplayFlags = 280
            TopColumn = 9
         End
         Begin Table = "Instalacion"
            Begin Extent = 
               Top = 377
               Left = 344
               Bottom = 588
               Right = 555
            End
            DisplayFlags = 280
            TopColumn = 13
         End
         Begin Table = "Entrada"
            Begin Extent = 
               Top = 156
               Left = 1335
               Bottom = 379
               Right = 1585
            End
      ', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_Horas', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'      DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Entrada_Linea"
            Begin Extent = 
               Top = 438
               Left = 957
               Bottom = 673
               Right = 1257
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Horas_TipoActuacion"
            Begin Extent = 
               Top = 330
               Left = 38
               Bottom = 426
               Right = 289
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
      Begin ColumnWidths = 30
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2685
         Alias = 4275
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_Horas', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[34] 2[12] 3) )"
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
         Top = -1056
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Producto_Subfamilia_Tipo"
            Begin Extent = 
               Top = 13
               Left = 1655
               Bottom = 145
               Right = 1884
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_SubFamilia"
            Begin Extent = 
               Top = 6
               Left = 1262
               Bottom = 125
               Right = 1517
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Emplazamiento_Abertura_Elemento"
            Begin Extent = 
               Top = 296
               Left = 1543
               Bottom = 415
               Right = 1877
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Emplazamiento_Abertura"
            Begin Extent = 
               Top = 241
               Left = 820
               Bottom = 398
               Right = 1154
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta"
            Begin Extent = 
               Top = 564
               Left = 832
               Bottom = 751
               Right = 1033
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta_Linea"
            Begin Extent = 
               Top = 167
               Left = 467
               Bottom = 1272
               Right = 751
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "Producto"
            Begin Extent = 
               Top = 0
             ', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'  Left = 870
               Bottom = 228
               Right = 1153
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Division"
            Begin Extent = 
               Top = 0
               Left = 579
               Bottom = 119
               Right = 777
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion"
            Begin Extent = 
               Top = 934
               Left = 127
               Bottom = 1061
               Right = 368
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 965
               Left = 822
               Bottom = 1073
               Right = 1011
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Estado"
            Begin Extent = 
               Top = 834
               Left = 0
               Bottom = 942
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta_Linea_Estado"
            Begin Extent = 
               Top = 425
               Left = 1155
               Bottom = 533
               Right = 1371
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_SubFamilia_Traspaso"
            Begin Extent = 
               Top = 534
               Left = 1155
               Bottom = 642
               Right = 1401
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta_Opcion_Accion"
            Begin Extent = 
               Top = 1219
               Left = 1178
               Bottom = 1314
               Right = 1422
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta_Opcion"
            Begin Extent = 
               Top = 1266
               Left = 799
               Bottom = 1395
               Right = 1043
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Propuesta_1"
            Begin Extent = 
               Top = 653
               Left = 134
               Bottom = 925
               Right = 335
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Propuesta_Linea_1"
            Begin Extent = 
               Top = 41
               Left = 0
               Bottom = 182
               Right = 284
            End
            DisplayFlags = 280
            TopColumn = 8
         End
         Begin Table = "Entrada"
            Begin Extent = 
               Top = 1402
               Left = 374
               Bottom = 1532
               Right = 624
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Entrada_Linea"
            Begin Extent = 
               Top = 1097
               Left = 840
               Bottom = 1227
               Right = 1140
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_ElementosAProteger_Tipo"
            Begin Extent = 
               Top = 305
               Left = 1230
               Bottom = 424
               Right = 1518
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane3', N' = "Instalacion_ElementosAProteger"
            Begin Extent = 
               Top = 414
               Left = 829
               Bottom = 553
               Right = 1117
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Emplazamiento"
            Begin Extent = 
               Top = 187
               Left = 3
               Bottom = 306
               Right = 238
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Emplazamiento_Zona"
            Begin Extent = 
               Top = 486
               Left = 0
               Bottom = 620
               Right = 271
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Instalacion_Emplazamiento_Planta"
            Begin Extent = 
               Top = 327
               Left = 16
               Bottom = 446
               Right = 287
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "C_Propuesta_Linea_Albaranada"
            Begin Extent = 
               Top = 0
               Left = 358
               Bottom = 130
               Right = 567
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "SistemaOperativo"
            Begin Extent = 
               Top = 1243
               Left = 163
               Bottom = 1339
               Right = 372
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_InstaladoEn"
            Begin Extent = 
               Top = 781
               Left = 869
               Bottom = 947
               Right = 1131
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Familia_Simbolo"
            Begin Extent = 
               Top = 162
               Left = 1715
               Bottom = 281
               Right = 1944
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Familia"
            Begin Extent = 
               Top = 132
               Left = 1386
               Bottom = 284
               Right = 1615
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
      Begin ColumnWidths = 87
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
         Width = 2115
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
         Wid', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane4', N'th = 1500
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
         Column = 5565
         Alias = 2625
         Table = 4035
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea', NULL, NULL
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
         Begin Table = "RetornaStock_1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 279
               Right = 263
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Almacen_ConStock', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Almacen_ConStock', NULL, NULL
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
         Configuration = "(H (1[50] 4[11] 2[20] 3) )"
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
         Begin Table = "ActividadCRM_Accion"
            Begin Extent = 
               Top = 33
               Left = 568
               Bottom = 304
               Right = 820
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM_Accion_Tipo"
            Begin Extent = 
               Top = 22
               Left = 979
               Bottom = 124
               Right = 1231
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM"
            Begin Extent = 
               Top = 36
               Left = 268
               Bottom = 363
               Right = 478
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM_Tipo"
            Begin Extent = 
               Top = 88
               Left = 21
               Bottom = 217
               Right = 231
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 175
               Left = 951
               Bottom = 304
               Right = 1185
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Prioridad"
            Begin Extent = 
               Top = 348
               Left = 955
               Bottom = 443
               Right = 1164
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Automatismo_Accion"
            Begin Extent = 
               Top = 328
               Left = 119
               Bottom = 500
             ', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Acciones', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N'  Right = 371
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
      Begin ColumnWidths = 15
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2445
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Acciones', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_ActividadCRM_Acciones', NULL, NULL
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
         Begin Table = "Prioridad"
            Begin Extent = 
               Top = 6
               Left = 618
               Bottom = 121
               Right = 827
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM_Accion_Tipo"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 290
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Automatismo_Accion"
            Begin Extent = 
               Top = 147
               Left = 339
               Bottom = 344
               Right = 591
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
         Alias = 2850
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Automatismo_Accion', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Automatismo_Accion', NULL, NULL
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
         Begin Table = "Automatismo"
            Begin Extent = 
               Top = 84
               Left = 503
               Bottom = 264
               Right = 713
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ActividadCRM_Tipo"
            Begin Extent = 
               Top = 61
               Left = 961
               Bottom = 207
               Right = 1171
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Prioridad"
            Begin Extent = 
               Top = 90
               Left = 114
               Bottom = 206
               Right = 323
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Automatismo', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Automatismo', NULL, NULL
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
