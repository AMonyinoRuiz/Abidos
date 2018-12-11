/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 12/02/2015 19:17:53

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
PRINT N'Dropping foreign keys from [dbo].[ListadoADV]'
GO
ALTER TABLE [dbo].[ListadoADV] DROP CONSTRAINT [FK_ListadoADV_Formulario]
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
PRINT N'Dropping foreign keys from [dbo].[Producto_Proveedor]'
GO
ALTER TABLE [dbo].[Producto_Proveedor] DROP CONSTRAINT [FK_Producto_Proveedor_Producto]
ALTER TABLE [dbo].[Producto_Proveedor] DROP CONSTRAINT [FK_Producto_Proveedor_Proveedor]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Producto_Proveedor]'
GO
ALTER TABLE [dbo].[Producto_Proveedor] DROP CONSTRAINT [PK_Producto_Proveedor]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Producto_Proveedor]'
GO
ALTER TABLE [dbo].[Producto_Proveedor] DROP CONSTRAINT [DF_Producto_Proveedor_Predeterminado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Entrada]'
GO
ALTER VIEW dbo.C_Entrada
AS
SELECT        dbo.Entrada.ID_Entrada, dbo.Entrada.Codigo, dbo.Entrada.Descripcion, dbo.Entrada.FechaEntrada, dbo.Entrada_Estado.ID_Entrada_Estado, 
                         dbo.Entrada_Estado.Descripcion AS Estado, dbo.Entrada_Tipo.ID_Entrada_Tipo, dbo.Entrada_Tipo.Descripcion AS [Tiop de documento], dbo.Almacen.ID_Almacen, 
                         dbo.Almacen.Descripcion AS [Almacén de destino], Almacen_1.Descripcion AS Almacén, dbo.Cliente.ID_Cliente, dbo.Cliente.Codigo AS [Código de Cliente], 
                         dbo.Cliente.Nombre AS [Nombre del cliente], dbo.Proveedor.ID_Proveedor, dbo.Proveedor.Codigo AS [Código del proveedor], 
                         dbo.Proveedor.Nombre AS [Nombre del proveedor], dbo.Entrada.Cliente_Direccion AS [Dirección del cliente], 
                         dbo.Entrada.Cliente_Poblacion AS [Población del cliente], dbo.Entrada.Cliente_Provincia AS [Povincia del cliente], 
                         dbo.Entrada.Cliente_PersonaContacto AS [Persona de contacto], dbo.Entrada.Cliente_Telefono AS Télefono, dbo.Entrada.Cliente_Nif AS [N.I.F], 
                         dbo.Entrada.Proveedor_Direccion AS [Dirección del proveedor], dbo.Entrada.Proveedor_Provincia AS [Provincia del proveedor], 
                         dbo.Entrada.Proveedor_Poblacion AS [Población del proveedor], dbo.Entrada.Proveedor_PersonaContacto AS [Comercial asignado], 
                         dbo.Entrada.Proveedor_Telefono, dbo.Entrada.Cliente_CP, dbo.Entrada.NumPedidoCliente AS [Número de pedido del cliente], 
                         dbo.Entrada.NumReferencia AS [Número de referencia], dbo.Entrada.Proyecto, dbo.Entrada.NombreObra AS [Nombre de la obra], 
                         dbo.Entrada.NumeroSeguimiento AS [Número de seguimiento], dbo.CompañiaTransporte.Descripcion AS [Compañía de transporte], 
                         dbo.Personal.Nombre AS Responsable, dbo.Entrada.Base, dbo.Entrada.IVA, dbo.Entrada.Descuento, dbo.Entrada.Total, 
                         dbo.Entrada.NumeroDocumentoProveedor AS [Número de documento proveedor], Almacen_1.ID_Almacen_Tipo AS IDTipoAlmacenOrigen, 
                         dbo.Almacen.ID_Almacen_Tipo AS IDTipoAlmacenDestino
FROM            dbo.Entrada INNER JOIN
                         dbo.Entrada_Estado ON dbo.Entrada.ID_Entrada_Estado = dbo.Entrada_Estado.ID_Entrada_Estado INNER JOIN
                         dbo.Entrada_Tipo ON dbo.Entrada.ID_Entrada_Tipo = dbo.Entrada_Tipo.ID_Entrada_Tipo LEFT OUTER JOIN
                         dbo.Almacen ON dbo.Entrada.ID_Almacen_Destino = dbo.Almacen.ID_Almacen LEFT OUTER JOIN
                         dbo.Personal ON dbo.Entrada.ID_Personal = dbo.Personal.ID_Personal LEFT OUTER JOIN
                         dbo.Almacen AS Almacen_1 ON dbo.Entrada.ID_Almacen = Almacen_1.ID_Almacen LEFT OUTER JOIN
                         dbo.Cliente ON dbo.Entrada.ID_Cliente = dbo.Cliente.ID_Cliente LEFT OUTER JOIN
                         dbo.CompañiaTransporte ON dbo.Entrada.ID_CompañiaTransporte = dbo.CompañiaTransporte.ID_CompañiaTransporte LEFT OUTER JOIN
                         dbo.Proveedor ON dbo.Entrada.ID_Proveedor = dbo.Proveedor.ID_Proveedor LEFT OUTER JOIN
                         dbo.Entrada_Origen ON dbo.Entrada.ID_Entrada_Origen = dbo.Entrada_Origen.ID_Entrada_Origen
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Producto]'
GO
ALTER TABLE [dbo].[Producto] ADD
[EsBono] [bit] NOT NULL CONSTRAINT [DF_Producto_Bono] DEFAULT ((0)),
[Bono_Cantidad] [decimal] (12, 2) NULL,
[ID_Producto_TipoCalculoPrecio] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Parte]'
GO
ALTER TABLE [dbo].[Parte] ADD
[ID_Bono] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
ALTER TABLE [dbo].[Parte] DROP
COLUMN [TrabajosRealizados],
COLUMN [ObservacionesTecnico],
COLUMN [ExplicacionHorasTecnico]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Parte_Aux]'
GO
CREATE TABLE [dbo].[Parte_Aux]
(
[ID_Parte] [int] NOT NULL,
[TrabajosRealizados] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[ObservacionesTecnico] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[ExplicacionHorasTecnico] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Parte_Aux] on [dbo].[Parte_Aux]'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Parte_Reparacion]'
GO
ALTER VIEW dbo.C_Parte_Reparacion
AS
SELECT        dbo.Parte_Reparacion.ID_Parte_Reparacion, dbo.Parte_Reparacion.ID_Parte, dbo.Parte_Reparacion.ID_Personal, dbo.Parte_Reparacion.Fecha, 
                         dbo.Parte_Reparacion.Descripcion AS Detalle, dbo.Parte_Reparacion.ID_Producto AS ID_Producto_Anterior, dbo.Personal.Nombre, 
                         dbo.Parte_Reparacion.MotivoAsignacion, dbo.C_Propuesta_Linea.ID_Propuesta_Linea, dbo.C_Propuesta_Linea.ID_Propuesta, dbo.C_Propuesta_Linea.Identificador, 
                         dbo.C_Propuesta_Linea.Uso, dbo.C_Propuesta_Linea.Unidad, dbo.C_Propuesta_Linea.Precio, dbo.C_Propuesta_Linea.Descuento, dbo.C_Propuesta_Linea.IVA, 
                         dbo.C_Propuesta_Linea.TotalBase, dbo.C_Propuesta_Linea.Identificador_LineaVinculada, dbo.C_Propuesta_Linea.Emplazamiento, dbo.C_Propuesta_Linea.Planta, 
                         dbo.C_Propuesta_Linea.Zona, dbo.C_Propuesta_Linea.CodigoProducto, dbo.C_Propuesta_Linea.Abertura, dbo.C_Propuesta_Linea.ElementoAProteger, 
                         dbo.C_Propuesta_Linea.Descripcion, dbo.C_Propuesta_Linea.ID_Instalacion_Emplazamiento_Zona, dbo.C_Propuesta_Linea.ID_Propuesta_Linea_Vinculado, 
                         dbo.C_Propuesta_Linea.Division, dbo.C_Propuesta_Linea.PropuestaCodigo, dbo.C_Propuesta_Linea.PropuestaVersion, 
                         dbo.C_Propuesta_Linea.ID_Producto_Division, dbo.C_Propuesta_Linea.NumZona, dbo.C_Propuesta_Linea.DetalleInstalacion, 
                         dbo.C_Propuesta_Linea.IdentificadorDelProducto, dbo.C_Propuesta_Linea.Familia, dbo.C_Propuesta_Linea.ID_Producto_Familia_Simbolo, 
                         dbo.C_Propuesta_Linea.ID_Producto_Familia, dbo.C_Propuesta_Linea.ID_Instalacion_Emplazamiento_Planta, 
                         dbo.C_Propuesta_Linea.ID_Instalacion_Emplazamiento, dbo.C_Propuesta_Linea.ID_Producto_SubFamilia_Traspaso, dbo.C_Propuesta_Linea.Activo, 
                         dbo.C_Propuesta_Linea.MotivoEliminacion, dbo.C_Propuesta_Linea.ID_Propuesta_Linea_Estado, dbo.C_Propuesta_Linea.ID_Producto, dbo.Parte.FechaAlta, 
                         dbo.Parte.FechaInicio, dbo.Parte.FechaFin, dbo.Parte.FechaLimiteFinalizacion, dbo.Parte.TrabajoARealizar, dbo.Parte_Aux.TrabajosRealizados, 
                         dbo.Parte_Aux.ObservacionesTecnico, dbo.Parte_Reparacion_Tipo.Descripcion AS TipoReparacion, dbo.Proveedor.Nombre AS Proveedor, 
                         dbo.Parte_Reparacion.ID_Proveedor, dbo.Parte_Reparacion.Fecha_Reparacion, dbo.Parte.Activo AS ParteActivo, dbo.Parte_Reparacion.Finalizado
FROM            dbo.Parte_Reparacion INNER JOIN
                         dbo.C_Propuesta_Linea ON dbo.Parte_Reparacion.ID_Propuesta_Linea = dbo.C_Propuesta_Linea.ID_Propuesta_Linea INNER JOIN
                         dbo.Parte ON dbo.Parte_Reparacion.ID_Parte = dbo.Parte.ID_Parte INNER JOIN
                         dbo.Parte_Reparacion_Tipo ON dbo.Parte_Reparacion.ID_Parte_Reparacion_Tipo = dbo.Parte_Reparacion_Tipo.ID_Parte_Reparacion_Tipo LEFT OUTER JOIN
                         dbo.Parte_Aux ON dbo.Parte.ID_Parte = dbo.Parte_Aux.ID_Parte LEFT OUTER JOIN
                         dbo.Proveedor ON dbo.Parte_Reparacion.ID_Proveedor = dbo.Proveedor.ID_Proveedor LEFT OUTER JOIN
                         dbo.Personal ON dbo.Parte_Reparacion.ID_Personal = dbo.Personal.ID_Personal
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Parte]'
GO
ALTER VIEW dbo.C_Parte
AS
SELECT        dbo.Cliente.Nombre AS NombreCliente, dbo.Parte_TipoFacturacion.Descripcion AS TipoFacturacion, dbo.Parte_Estado.Descripcion AS Estado, 
                         dbo.Parte_Tipo.Descripcion AS Tipo, dbo.Parte.ID_Parte, dbo.Parte.ID_Parte_Vinculado, dbo.Parte.ID_Instalacion, dbo.Parte.ID_Propuesta, dbo.Parte.ID_Cliente, 
                         dbo.Parte.ID_Parte_Tipo, dbo.Parte.Direccion, dbo.Parte.Poblacion, dbo.Parte.Provincia, dbo.Parte.PersonaContacto, dbo.Parte.Telefono, 
                         dbo.Parte.QuienDetectoIncidencia, dbo.Parte.FechaAlta, dbo.Parte.FechaInicio, dbo.Parte.FechaFin, dbo.Parte.FechaLimiteFinalizacion, dbo.Parte.HoraInicio, 
                         dbo.Parte.ParteFirmado, dbo.Parte.PersonaQueLoFirmo, dbo.Parte.HorasRealizadas, dbo.Parte.HorasPrevistas, dbo.Parte.ID_Parte_TipoFacturacion, 
                         dbo.Parte.ID_Parte_Estado, dbo.Parte.CostePrevisto, dbo.Parte.CostePrevistoMaterial, dbo.Parte.CostePrevistoGastos, dbo.Parte.CosteImputadoMO, 
                         dbo.Parte.CosteMaterial, dbo.Parte.CosteGastos, dbo.Parte.MargenMO, dbo.Parte.MargenMaterial, dbo.Parte.MargenGastos, dbo.Parte.TrabajoARealizar, 
                         dbo.Parte_Aux.TrabajosRealizados, dbo.Parte.Factura, dbo.Parte.Activo, dbo.Parte.Punteo, dbo.Parte.ID_Personal, dbo.Parte.BloquearImputacionHoras, 
                         dbo.Parte.NoPermitirModificarInformeTecnico, dbo.Parte.BloquearImputacionMaterial
FROM            dbo.Parte INNER JOIN
                         dbo.Parte_Estado ON dbo.Parte.ID_Parte_Estado = dbo.Parte_Estado.ID_Parte_Estado INNER JOIN
                         dbo.Parte_Tipo ON dbo.Parte.ID_Parte_Tipo = dbo.Parte_Tipo.ID_Parte_Tipo LEFT OUTER JOIN
                         dbo.Parte_Aux ON dbo.Parte.ID_Parte = dbo.Parte_Aux.ID_Parte LEFT OUTER JOIN
                         dbo.Parte_TipoFacturacion ON dbo.Parte.ID_Parte_TipoFacturacion = dbo.Parte_TipoFacturacion.ID_Parte_TipoFacturacion LEFT OUTER JOIN
                         dbo.Cliente ON dbo.Parte.ID_Cliente = dbo.Cliente.ID_Cliente
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Producto]'
GO
ALTER VIEW dbo.C_Producto
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
                         dbo.Producto.EsBono, dbo.Producto.Bono_Cantidad
FROM            dbo.Producto INNER JOIN
                         dbo.Producto_Division ON dbo.Producto.ID_Producto_Division = dbo.Producto_Division.ID_Producto_Division INNER JOIN
                         dbo.Producto_Familia ON dbo.Producto.ID_Producto_Familia = dbo.Producto_Familia.ID_Producto_Familia AND 
                         dbo.Producto_Division.ID_Producto_Division = dbo.Producto_Familia.ID_Producto_Division INNER JOIN
                         dbo.Producto_Marca ON dbo.Producto.ID_Producto_Marca = dbo.Producto_Marca.ID_Producto_Marca AND 
                         dbo.Producto_Division.ID_Producto_Division = dbo.Producto_Marca.ID_Producto_Division INNER JOIN
                         dbo.Producto_SubFamilia ON dbo.Producto.ID_Producto_SubFamilia = dbo.Producto_SubFamilia.ID_Producto_SubFamilia AND 
                         dbo.Producto_Familia.ID_Producto_Familia = dbo.Producto_SubFamilia.ID_Producto_Familia LEFT OUTER JOIN
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
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Listado_Parte]'
GO
ALTER VIEW dbo.C_Listado_Parte
AS
SELECT        dbo.Parte.ID_Parte, dbo.Parte.ID_Parte_Vinculado, dbo.Parte.ID_Instalacion, dbo.Parte.ID_Propuesta, dbo.Parte.ID_Cliente, dbo.Parte.ID_Parte_Tipo, 
                         dbo.Parte.ID_Producto_Division, dbo.Parte.Direccion, dbo.Parte.Poblacion, dbo.Parte.Provincia, dbo.Parte.PersonaContacto, dbo.Parte.Telefono, 
                         dbo.Parte.QuienDetectoIncidencia, dbo.Parte.FechaAlta, dbo.Parte.FechaInicio, dbo.Parte.FechaFin, dbo.Parte.FechaLimiteFinalizacion, dbo.Parte.HoraInicio, 
                         dbo.Parte.ParteFirmado, dbo.Parte.PersonaQueLoFirmo, dbo.Parte.HorasRealizadas, dbo.Parte.HorasPrevistas, dbo.Parte.ID_Parte_TipoFacturacion, 
                         dbo.Parte.ID_Parte_Estado, dbo.Parte.CostePrevisto, dbo.Parte.CostePrevistoMaterial, dbo.Parte.CostePrevistoGastos, dbo.Parte.CosteImputadoMO, 
                         dbo.Parte.CosteMaterial, dbo.Parte.CosteGastos, dbo.Parte.MargenMO, dbo.Parte.MargenMaterial, dbo.Parte.MargenGastos, dbo.Parte.TrabajoARealizar, 
                         dbo.Parte_Aux.TrabajosRealizados, dbo.Parte_Aux.ObservacionesTecnico, dbo.Parte.Factura, dbo.Parte.Activo, dbo.Parte_Estado.Descripcion AS ParteEstado, 
                         dbo.Parte_Tipo.Descripcion AS ParteTipo, dbo.Cliente.Nombre AS ClienteNombre, Personal_1.Nombre AS InstalacionResponsable, 
                         dbo.Personal.Nombre AS ClienteComercial, dbo.Producto_Division.Descripcion AS ParteRevisionDivision, 
                         dbo.Parte_TipoFacturacion.Descripcion AS ParteTipoFacturacion
FROM            dbo.Parte INNER JOIN
                         dbo.Parte_Estado ON dbo.Parte.ID_Parte_Estado = dbo.Parte_Estado.ID_Parte_Estado INNER JOIN
                         dbo.Parte_Tipo ON dbo.Parte.ID_Parte_Tipo = dbo.Parte_Tipo.ID_Parte_Tipo INNER JOIN
                         dbo.Parte_TipoFacturacion ON dbo.Parte.ID_Parte_TipoFacturacion = dbo.Parte_TipoFacturacion.ID_Parte_TipoFacturacion INNER JOIN
                         dbo.Instalacion ON dbo.Parte.ID_Instalacion = dbo.Instalacion.ID_Instalacion INNER JOIN
                         dbo.Cliente ON dbo.Parte.ID_Cliente = dbo.Cliente.ID_Cliente INNER JOIN
                         dbo.Parte_Aux ON dbo.Parte.ID_Parte = dbo.Parte_Aux.ID_Parte LEFT OUTER JOIN
                         dbo.Producto_Division ON dbo.Parte.ID_Producto_Division = dbo.Producto_Division.ID_Producto_Division LEFT OUTER JOIN
                         dbo.Personal AS Personal_1 ON dbo.Instalacion.ID_Personal = Personal_1.ID_Personal LEFT OUTER JOIN
                         dbo.Personal ON dbo.Cliente.ID_Personal = dbo.Personal.ID_Personal
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Bono]'
GO
CREATE TABLE [dbo].[Bono]
(
[ID_Bono] [int] NOT NULL IDENTITY(1, 1),
[ID_Producto] [int] NOT NULL,
[ID_Cliente] [int] NOT NULL,
[Codigo] [int] NOT NULL,
[DescripcionProducto] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Cantidad] [decimal] (12, 2) NOT NULL,
[HorasConsumidas] [decimal] (12, 2) NULL,
[FechaAlta] [smalldatetime] NULL,
[FechaCaducidad] [smalldatetime] NULL,
[Cerrado] [bit] NOT NULL CONSTRAINT [DF_Bono_Cerrado] DEFAULT ((0)),
[Observaciones] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[CondicionesComerciales] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Bono] on [dbo].[Bono]'
GO
ALTER TABLE [dbo].[Bono] ADD CONSTRAINT [PK_Bono] PRIMARY KEY CLUSTERED  ([ID_Bono])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Bono]'
GO
CREATE VIEW dbo.C_Bono
AS
SELECT        dbo.Bono.ID_Bono, dbo.Bono.ID_Producto, dbo.Bono.ID_Cliente, dbo.Bono.Codigo, dbo.Bono.DescripcionProducto, dbo.Bono.Cantidad, dbo.Bono.FechaAlta, 
                         dbo.Bono.FechaCaducidad, dbo.Bono.Cerrado, dbo.Producto.Codigo AS CodigoProducto, dbo.Cliente.Codigo AS CodigoCliente, ISNULL
                             ((SELECT        SUM(dbo.Parte_Horas.Horas + dbo.Parte_Horas.HorasExtras) AS HorasUsadas
                                 FROM            dbo.Parte INNER JOIN
                                                          dbo.Parte_Horas ON dbo.Parte.ID_Parte = dbo.Parte_Horas.ID_Parte
                                 WHERE        (dbo.Parte.ID_Bono = dbo.Bono.ID_Bono) AND (dbo.Parte_Horas.ErrorDelTecnico = 0) AND (dbo.Parte_Horas.ErrorDeOtroTecnico = 0) AND 
                                                          (dbo.Parte.ID_Bono IS NOT NULL)), 0) AS HorasConsumidas, dbo.Bono.Cantidad - ISNULL
                             ((SELECT        SUM(Parte_Horas_1.Horas + Parte_Horas_1.HorasExtras) AS HorasUsadas
                                 FROM            dbo.Parte AS Parte_1 INNER JOIN
                                                          dbo.Parte_Horas AS Parte_Horas_1 ON Parte_1.ID_Parte = Parte_Horas_1.ID_Parte
                                 WHERE        (Parte_1.ID_Bono = dbo.Bono.ID_Bono) AND (Parte_Horas_1.ErrorDelTecnico = 0) AND (Parte_Horas_1.ErrorDeOtroTecnico = 0) AND 
                                                          (Parte_1.ID_Bono IS NOT NULL)), 0) AS HorasRestantes, dbo.Cliente.Nombre AS ClienteNombre
FROM            dbo.Bono INNER JOIN
                         dbo.Cliente ON dbo.Bono.ID_Cliente = dbo.Cliente.ID_Cliente INNER JOIN
                         dbo.Producto ON dbo.Bono.ID_Producto = dbo.Producto.ID_Producto
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Contadores]'
GO
ALTER TABLE [dbo].[Contadores] ADD
[Bono] [int] NOT NULL CONSTRAINT [DF_Contadores_Bono] DEFAULT ((1))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Producto_Proveedor]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Producto_Proveedor]
(
[ID_Producto_Proveedor] [int] NOT NULL IDENTITY(1, 1),
[ID_Producto] [int] NOT NULL,
[ID_Proveedor] [int] NOT NULL,
[Descuento] [decimal] (5, 2) NULL,
[PVP] [decimal] (10, 2) NOT NULL,
[PVD] [decimal] (10, 2) NULL,
[Predeterminado] [bit] NOT NULL CONSTRAINT [DF_Producto_Proveedor_Predeterminado] DEFAULT ((0)),
[CodigoProductoProveedor] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Producto_Proveedor] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Producto_Proveedor]([ID_Producto_Proveedor], [ID_Producto], [ID_Proveedor], [Descuento], [PVP], [Predeterminado], [CodigoProductoProveedor]) SELECT [ID_Producto_Proveedor], [ID_Producto], [ID_Proveedor], [Descuento], [PVP], [Predeterminado], [CodigoProductoProveedor] FROM [dbo].[Producto_Proveedor]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Producto_Proveedor] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @idVal BIGINT
SELECT @idVal = IDENT_CURRENT(N'[dbo].[Producto_Proveedor]')
IF @idVal IS NOT NULL
    DBCC CHECKIDENT(N'[dbo].[tmp_rg_xx_Producto_Proveedor]', RESEED, @idVal)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Producto_Proveedor]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Producto_Proveedor]', N'Producto_Proveedor'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Producto_Proveedor] on [dbo].[Producto_Proveedor]'
GO
ALTER TABLE [dbo].[Producto_Proveedor] ADD CONSTRAINT [PK_Producto_Proveedor] PRIMARY KEY CLUSTERED  ([ID_Producto_Proveedor])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Bono_Archivo]'
GO
CREATE TABLE [dbo].[Bono_Archivo]
(
[ID_Bono_Archivo] [int] NOT NULL,
[ID_Archivo] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Bono_Archivo] on [dbo].[Bono_Archivo]'
GO
ALTER TABLE [dbo].[Bono_Archivo] ADD CONSTRAINT [PK_Bono_Archivo] PRIMARY KEY CLUSTERED  ([ID_Bono_Archivo], [ID_Archivo])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Bono_Instalacion]'
GO
CREATE TABLE [dbo].[Bono_Instalacion]
(
[ID_Bono_Instalacion] [int] NOT NULL IDENTITY(1, 1),
[ID_Bono] [int] NOT NULL,
[ID_Instalacion] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Bono_Instalacion] on [dbo].[Bono_Instalacion]'
GO
ALTER TABLE [dbo].[Bono_Instalacion] ADD CONSTRAINT [PK_Bono_Instalacion] PRIMARY KEY CLUSTERED  ([ID_Bono_Instalacion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Bono_Archivo]'
GO
ALTER TABLE [dbo].[Bono_Archivo] ADD CONSTRAINT [FK_Bono_Archivo_Bono1] FOREIGN KEY ([ID_Bono_Archivo]) REFERENCES [dbo].[Bono] ([ID_Bono])
ALTER TABLE [dbo].[Bono_Archivo] ADD CONSTRAINT [FK_Bono_Archivo_Archivo1] FOREIGN KEY ([ID_Archivo]) REFERENCES [dbo].[Archivo] ([ID_Archivo])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Bono_Instalacion]'
GO
ALTER TABLE [dbo].[Bono_Instalacion] ADD CONSTRAINT [FK_Bono_Instalacion_Bono] FOREIGN KEY ([ID_Bono]) REFERENCES [dbo].[Bono] ([ID_Bono])
ALTER TABLE [dbo].[Bono_Instalacion] ADD CONSTRAINT [FK_Bono_Instalacion_Instalacion] FOREIGN KEY ([ID_Instalacion]) REFERENCES [dbo].[Instalacion] ([ID_Instalacion])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Parte]'
GO
ALTER TABLE [dbo].[Parte] ADD CONSTRAINT [FK_Parte_Bono] FOREIGN KEY ([ID_Bono]) REFERENCES [dbo].[Bono] ([ID_Bono])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Bono]'
GO
ALTER TABLE [dbo].[Bono] ADD CONSTRAINT [FK_Bono_Producto] FOREIGN KEY ([ID_Producto]) REFERENCES [dbo].[Producto] ([ID_Producto])
ALTER TABLE [dbo].[Bono] ADD CONSTRAINT [FK_Bono_Cliente] FOREIGN KEY ([ID_Cliente]) REFERENCES [dbo].[Cliente] ([ID_Cliente])
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
PRINT N'Adding foreign keys to [dbo].[Producto_Proveedor]'
GO
ALTER TABLE [dbo].[Producto_Proveedor] ADD CONSTRAINT [FK_Producto_Proveedor_Producto] FOREIGN KEY ([ID_Producto]) REFERENCES [dbo].[Producto] ([ID_Producto])
ALTER TABLE [dbo].[Producto_Proveedor] ADD CONSTRAINT [FK_Producto_Proveedor_Proveedor] FOREIGN KEY ([ID_Proveedor]) REFERENCES [dbo].[Proveedor] ([ID_Proveedor])
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
         Configuration = "(H (1[31] 4[47] 2[8] 3) )"
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
         Begin Table = "Entrada"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 522
               Right = 277
            End
            DisplayFlags = 280
            TopColumn = 25
         End
         Begin Table = "Entrada_Estado"
            Begin Extent = 
               Top = 0
               Left = 361
               Bottom = 95
               Right = 570
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Entrada_Tipo"
            Begin Extent = 
               Top = 52
               Left = 610
               Bottom = 165
               Right = 819
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Almacen"
            Begin Extent = 
               Top = 524
               Left = 911
               Bottom = 653
               Right = 1120
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 443
               Left = 357
               Bottom = 573
               Right = 591
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Almacen_1"
            Begin Extent = 
               Top = 28
               Left = 958
               Bottom = 157
               Right = 1167
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 245
               Left = 927
               Bottom = 456
               Right = 1136
            End
            ', 'SCHEMA', N'dbo', 'VIEW', N'C_Entrada', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CompañiaTransporte"
            Begin Extent = 
               Top = 102
               Left = 315
               Bottom = 198
               Right = 532
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Proveedor"
            Begin Extent = 
               Top = 305
               Left = 374
               Bottom = 434
               Right = 583
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Entrada_Origen"
            Begin Extent = 
               Top = 171
               Left = 562
               Bottom = 266
               Right = 771
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
      Begin ColumnWidths = 44
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 3480
         Alias = 3390
         Table = 3105
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Entrada', NULL, NULL
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
         Configuration = "(H (1[23] 4[49] 2[12] 3) )"
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
               Top = 13
               Left = 261
               Bottom = 488
               Right = 475
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Estado"
            Begin Extent = 
               Top = 0
               Left = 4
               Bottom = 119
               Right = 202
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Tipo"
            Begin Extent = 
               Top = 0
               Left = 673
               Bottom = 119
               Right = 871
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_TipoFacturacion"
            Begin Extent = 
               Top = 46
               Left = 909
               Bottom = 165
               Right = 1123
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion"
            Begin Extent = 
               Top = 330
               Left = 555
               Bottom = 533
               Right = 760
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 231
               Left = 846
               Bottom = 381
               Right = 1044
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Division"
            Begin Extent = 
               Top = 178
               Left = 0
               Bottom = 297
               Right = 198
            End', 'SCHEMA', N'dbo', 'VIEW', N'C_Listado_Parte', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal_1"
            Begin Extent = 
               Top = 407
               Left = 881
               Bottom = 526
               Right = 1079
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 321
               Left = 1101
               Bottom = 440
               Right = 1299
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Aux"
            Begin Extent = 
               Top = 321
               Left = 0
               Bottom = 450
               Right = 221
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
      Begin ColumnWidths = 46
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2625
         Alias = 2175
         Table = 2460
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Listado_Parte', NULL, NULL
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
         Configuration = "(H (1[41] 4[29] 2[14] 3) )"
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
               Top = 6
               Left = 38
               Bottom = 125
               Right = 319
            End
            DisplayFlags = 280
            TopColumn = 17
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
               Bottom = 574
     ', 'SCHEMA', N'dbo', 'VIEW', N'C_Producto', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'          Right = 355
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
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 47
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
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
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[32] 4[39] 2[11] 3) )"
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
               Top = 0
               Left = 324
               Bottom = 402
               Right = 538
            End
            DisplayFlags = 280
            TopColumn = 36
         End
         Begin Table = "Parte_Estado"
            Begin Extent = 
               Top = 284
               Left = 761
               Bottom = 403
               Right = 959
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Tipo"
            Begin Extent = 
               Top = 6
               Left = 762
               Bottom = 125
               Right = 960
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_TipoFacturacion"
            Begin Extent = 
               Top = 147
               Left = 747
               Bottom = 266
               Right = 961
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 16
               Left = 26
               Bottom = 359
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Aux"
            Begin Extent = 
               Top = 384
               Left = 602
               Bottom = 513
               Right = 823
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
      Begin ColumnWidths = 42
         Width = 284
         Width = 1500
    ', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'     Width = 1500
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
         Column = 2835
         Alias = 2565
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte', NULL, NULL
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
         Configuration = "(H (1[24] 4[35] 2[15] 3) )"
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
         Begin Table = "Parte_Reparacion"
            Begin Extent = 
               Top = 13
               Left = 564
               Bottom = 293
               Right = 762
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "C_Propuesta_Linea"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 336
               Right = 309
            End
            DisplayFlags = 280
            TopColumn = 20
         End
         Begin Table = "Parte"
            Begin Extent = 
               Top = 5
               Left = 913
               Bottom = 229
               Right = 1127
            End
            DisplayFlags = 280
            TopColumn = 26
         End
         Begin Table = "Parte_Reparacion_Tipo"
            Begin Extent = 
               Top = 150
               Left = 271
               Bottom = 269
               Right = 489
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Proveedor"
            Begin Extent = 
               Top = 173
               Left = 1239
               Bottom = 292
               Right = 1437
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 0
               Left = 1194
               Bottom = 119
               Right = 1392
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Aux"
            Begin Extent = 
               Top = 300
               Left = 618
               Bottom = 429
               Right = 839
    ', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_Reparacion', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'        End
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
      Begin ColumnWidths = 46
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4470
         Alias = 2565
         Table = 2310
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_Reparacion', NULL, NULL
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
         Begin Table = "Bono"
            Begin Extent = 
               Top = 36
               Left = 557
               Bottom = 313
               Right = 766
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 30
               Left = 849
               Bottom = 265
               Right = 1114
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Producto"
            Begin Extent = 
               Top = 39
               Left = 81
               Bottom = 276
               Right = 410
            End
            DisplayFlags = 280
            TopColumn = 14
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 17
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1755
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
         Column = 3570
         Alias = 3000
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Bono', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Bono', NULL, NULL
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
