/*
Run this script on:

        SERVER2012R2\SQLSERVER2012.AbidosDomingo    -  This database will be modified

to synchronize it with:

        SERVER2012R2\SQLSERVER2012.AbidosDomingoReal

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.0 from Red Gate Software Ltd at 07/12/2016 10:26:56

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
PRINT N'Dropping foreign keys from [dbo].[Empresa_CuentaBancaria]'
GO
ALTER TABLE [dbo].[Empresa_CuentaBancaria] DROP CONSTRAINT [FK_Empresa_CuentaBancaria_Empresa]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping foreign keys from [dbo].[Empresa_FechasNoLaborables]'
GO
ALTER TABLE [dbo].[Empresa_FechasNoLaborables] DROP CONSTRAINT [FK_Empresa_FechasNoLaborables_Empresa]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping constraints from [dbo].[Empresa]'
GO
ALTER TABLE [dbo].[Empresa] DROP CONSTRAINT [PK_Empresa]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Dropping constraints from [dbo].[Empresa]'
GO
ALTER TABLE [dbo].[Empresa] DROP CONSTRAINT [DF_Empresa_Activo]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[Entrada]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Entrada] ADD
[ID_Empresa] [int] NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Rebuilding [dbo].[Empresa]'
GO
CREATE TABLE [dbo].[RG_Recovery_1_Empresa]
(
[ID_Empresa] [int] NOT NULL IDENTITY(1, 1),
[Codigo] [int] NOT NULL,
[Nombre] [nvarchar] (250) COLLATE Modern_Spanish_CI_AS NOT NULL,
[NombreComercial] [nvarchar] (250) COLLATE Modern_Spanish_CI_AS NULL,
[NIF] [nvarchar] (20) COLLATE Modern_Spanish_CI_AS NULL,
[PersonaContacto] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[Email] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[Telefono] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
[Fax] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
[Direccion] [nvarchar] (200) COLLATE Modern_Spanish_CI_AS NULL,
[Poblacion] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[Provincia] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[FechaAlta] [smalldatetime] NOT NULL,
[FechaBaja] [smalldatetime] NULL,
[Observaciones] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL,
[CP] [nvarchar] (20) COLLATE Modern_Spanish_CI_AS NULL,
[URLAcceso] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[Usuario] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
[Contraseña] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
[Logo] [varbinary] (max) NULL,
[Activo] [bit] NOT NULL CONSTRAINT [DF_Empresa_Activo] DEFAULT ((1)),
[Predeterminada] [bit] NOT NULL CONSTRAINT [DF_Empresa_Predeterminada] DEFAULT ((0)),
[NumeracionFacturaVenta] [int] NOT NULL CONSTRAINT [DF_Empresa_NumeracionFactura] DEFAULT ((0)),
[NumeracionFacturaCompra] [int] NOT NULL CONSTRAINT [DF_Empresa_NumeracionFacturaVenta1] DEFAULT ((0)),
[NumeracionFacturaVentaRectificativa] [int] NOT NULL CONSTRAINT [DF_Empresa_NumeracionFacturaVenta2] DEFAULT ((0)),
[NumeracionFacturaCompraRectificativa] [int] NOT NULL CONSTRAINT [DF_Empresa_NumeracionFacturaVenta3] DEFAULT ((0))
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_Empresa] ON
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
INSERT INTO [dbo].[RG_Recovery_1_Empresa]([ID_Empresa], [Codigo], [Nombre], [NombreComercial], [NIF], [PersonaContacto], [Email], [Telefono], [Fax], [Direccion], [Poblacion], [Provincia], [FechaAlta], [FechaBaja], [Observaciones], [CP], [URLAcceso], [Usuario], [Contraseña], [Logo], [Activo]) SELECT [ID_Empresa], [Codigo], [Nombre], [NombreComercial], [NIF], [PersonaContacto], [Email], [Telefono], [Fax], [Direccion], [Poblacion], [Provincia], [FechaAlta], [FechaBaja], [Observaciones], [CP], [URLAcceso], [Usuario], [Contraseña], [Logo], [Activo] FROM [dbo].[Empresa]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
SET IDENTITY_INSERT [dbo].[RG_Recovery_1_Empresa] OFF
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DROP TABLE [dbo].[Empresa]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_rename N'[dbo].[RG_Recovery_1_Empresa]', N'Empresa', N'OBJECT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Empresa] on [dbo].[Empresa]'
GO
ALTER TABLE [dbo].[Empresa] ADD CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED  ([ID_Empresa])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[Producto]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Producto] ADD
[Comercial] [bit] NOT NULL CONSTRAINT [DF_Producto_Comercial] DEFAULT ((0)),
[Produccion] [bit] NOT NULL CONSTRAINT [DF_Producto_Produccion] DEFAULT ((1))
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[C_Entrada]'
GO
ALTER VIEW [dbo].[C_Entrada]
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
                         dbo.Almacen.ID_Almacen_Tipo AS IDTipoAlmacenDestino, dbo.Empresa.ID_Empresa, dbo.Empresa.NombreComercial AS Empresa
FROM            dbo.Entrada INNER JOIN
                         dbo.Entrada_Estado ON dbo.Entrada.ID_Entrada_Estado = dbo.Entrada_Estado.ID_Entrada_Estado INNER JOIN
                         dbo.Entrada_Tipo ON dbo.Entrada.ID_Entrada_Tipo = dbo.Entrada_Tipo.ID_Entrada_Tipo LEFT OUTER JOIN
                         dbo.Empresa ON dbo.Entrada.ID_Empresa = dbo.Empresa.ID_Empresa LEFT OUTER JOIN
                         dbo.Empresa AS Empresa_1 ON dbo.Entrada.ID_Empresa = Empresa_1.ID_Empresa LEFT OUTER JOIN
                         dbo.Almacen ON dbo.Entrada.ID_Almacen_Destino = dbo.Almacen.ID_Almacen LEFT OUTER JOIN
                         dbo.Personal ON dbo.Entrada.ID_Personal = dbo.Personal.ID_Personal LEFT OUTER JOIN
                         dbo.Almacen AS Almacen_1 ON dbo.Entrada.ID_Almacen = Almacen_1.ID_Almacen LEFT OUTER JOIN
                         dbo.Cliente ON dbo.Entrada.ID_Cliente = dbo.Cliente.ID_Cliente LEFT OUTER JOIN
                         dbo.CompañiaTransporte ON dbo.Entrada.ID_CompañiaTransporte = dbo.CompañiaTransporte.ID_CompañiaTransporte LEFT OUTER JOIN
                         dbo.Proveedor ON dbo.Entrada.ID_Proveedor = dbo.Proveedor.ID_Proveedor LEFT OUTER JOIN
                         dbo.Entrada_Origen ON dbo.Entrada.ID_Entrada_Origen = dbo.Entrada_Origen.ID_Entrada_Origen
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[Propuesta]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[Propuesta] ADD
[ID_Empresa] [int] NULL
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
                         dbo.Producto.EsBono, dbo.Producto.Bono_Cantidad, dbo.Archivo.CampoBinario AS Foto, dbo.Producto.Comercial, dbo.Producto.Produccion
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
PRINT N'Altering [dbo].[C_Propuesta]'
GO
ALTER VIEW [dbo].[C_Propuesta]
AS
SELECT        dbo.Propuesta.ID_Propuesta, dbo.Propuesta.ID_Instalacion, dbo.Propuesta.ID_Propuesta_Estado, dbo.Propuesta.ID_Propuesta_Tipo, 
                         dbo.Propuesta.ID_Producto_Grado, dbo.Propuesta.ID_Propuesta_Relacion, dbo.Propuesta.Codigo, dbo.Propuesta.Version, dbo.Propuesta.Descripcion, 
                         dbo.Propuesta.Fecha, dbo.Propuesta.Persona, dbo.Propuesta.SegunNormativa, dbo.Propuesta.Base, dbo.Propuesta.IVA, dbo.Propuesta.Descuento, 
                         dbo.Propuesta.Total, dbo.Propuesta.Activo, dbo.Propuesta_Estado.Descripcion AS Estado, dbo.Propuesta_Tipo.Descripcion AS Tipo, 
                         Propuesta_1.Codigo AS Relacionado, dbo.Producto_Grado.Descripcion AS Grado, dbo.Propuesta.SeInstalo, ISNULL
                             ((SELECT        TOP (1) ID_Parte
                                 FROM            dbo.Parte
                                 WHERE        (Activo = 1) AND (ID_Propuesta = dbo.Propuesta.ID_Propuesta) AND (ID_Parte_Tipo = 3)), NULL) AS ID_Parte, dbo.Cliente.ID_Cliente, 
                         dbo.Cliente.Nombre, dbo.Instalacion_Emplazamiento_Planta.ID_Instalacion_Emplazamiento_Planta, dbo.Instalacion_Emplazamiento.Descripcion AS Ubicación, 
                         dbo.Instalacion_Emplazamiento_Planta.Descripcion AS Planta, dbo.Instalacion_Emplazamiento_Zona.Descripcion AS Zona, 
                         dbo.Entrada.ID_Entrada AS PedidoDeVenta, dbo.Entrada.Codigo AS PedidoDeVentaCodigo, dbo.FormaPago.Descripcion AS FormaDePago, 
                         dbo.Personal.Nombre AS Comercial, dbo.Empresa.ID_Empresa, dbo.Empresa.NombreComercial AS Empresa
FROM            dbo.Propuesta INNER JOIN
                         dbo.Propuesta_Estado ON dbo.Propuesta.ID_Propuesta_Estado = dbo.Propuesta_Estado.ID_Propuesta_Estado INNER JOIN
                         dbo.Propuesta_Tipo ON dbo.Propuesta.ID_Propuesta_Tipo = dbo.Propuesta_Tipo.ID_Propuesta_Tipo INNER JOIN
                         dbo.Instalacion ON dbo.Propuesta.ID_Instalacion = dbo.Instalacion.ID_Instalacion INNER JOIN
                         dbo.Cliente ON dbo.Instalacion.ID_Cliente = dbo.Cliente.ID_Cliente LEFT OUTER JOIN
                         dbo.Empresa ON dbo.Propuesta.ID_Empresa = dbo.Empresa.ID_Empresa LEFT OUTER JOIN
                         dbo.Propuesta AS Propuesta_1 ON dbo.Propuesta.ID_Propuesta_Relacion = Propuesta_1.ID_Propuesta LEFT OUTER JOIN
                         dbo.Entrada ON dbo.Propuesta.ID_Propuesta = dbo.Entrada.ID_Propuesta LEFT OUTER JOIN
                         dbo.Personal ON dbo.Propuesta.ID_Personal = dbo.Personal.ID_Personal LEFT OUTER JOIN
                         dbo.FormaPago ON dbo.Propuesta.ID_FormaPago = dbo.FormaPago.ID_FormaPago LEFT OUTER JOIN
                         dbo.Instalacion_Emplazamiento_Zona ON 
                         dbo.Propuesta.ID_Instalacion_Emplazamiento_Zona = dbo.Instalacion_Emplazamiento_Zona.ID_Instalacion_Emplazamiento_Zona LEFT OUTER JOIN
                         dbo.Instalacion_Emplazamiento ON 
                         dbo.Propuesta.ID_Instalacion_Emplazamiento = dbo.Instalacion_Emplazamiento.ID_Instalacion_Emplazamiento LEFT OUTER JOIN
                         dbo.Instalacion_Emplazamiento_Planta ON 
                         dbo.Propuesta.ID_Instalacion_Emplazamiento_Planta = dbo.Instalacion_Emplazamiento_Planta.ID_Instalacion_Emplazamiento_Planta LEFT OUTER JOIN
                         dbo.Producto_Grado ON dbo.Propuesta.ID_Producto_Grado = dbo.Producto_Grado.ID_Producto_Grado
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Cliente_Empresa]'
GO
CREATE TABLE [dbo].[Cliente_Empresa]
(
[ID_Cliente_Empresa] [int] NOT NULL IDENTITY(1, 1),
[ID_Cliente] [int] NOT NULL,
[ID_Empresa] [int] NOT NULL,
[Predeterminada] [bit] NOT NULL CONSTRAINT [DF_Cliente_Empresa_Predeterminada] DEFAULT ((0))
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Cliente_Empresa] on [dbo].[Cliente_Empresa]'
GO
ALTER TABLE [dbo].[Cliente_Empresa] ADD CONSTRAINT [PK_Cliente_Empresa] PRIMARY KEY CLUSTERED  ([ID_Cliente_Empresa])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Proveedor_Empresa]'
GO
CREATE TABLE [dbo].[Proveedor_Empresa]
(
[ID_Proveedor_Empresa] [int] NOT NULL IDENTITY(1, 1),
[ID_Proveedor] [int] NOT NULL,
[ID_Empresa] [int] NOT NULL,
[Predeterminada] [bit] NOT NULL CONSTRAINT [DF_Proveedor_Empresa_Predeterminada] DEFAULT ((0))
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Proveedor_Empresa] on [dbo].[Proveedor_Empresa]'
GO
ALTER TABLE [dbo].[Proveedor_Empresa] ADD CONSTRAINT [PK_Proveedor_Empresa] PRIMARY KEY CLUSTERED  ([ID_Proveedor_Empresa])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Personal_Empresa]'
GO
CREATE TABLE [dbo].[Personal_Empresa]
(
[ID_Personal_Empresa] [int] NOT NULL IDENTITY(1, 1),
[ID_Personal] [int] NOT NULL,
[ID_Empresa] [int] NOT NULL,
[Predeterminada] [bit] NOT NULL CONSTRAINT [DF_Personal_Empresa_Predeterminada] DEFAULT ((0))
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Personal_Empresa] on [dbo].[Personal_Empresa]'
GO
ALTER TABLE [dbo].[Personal_Empresa] ADD CONSTRAINT [PK_Personal_Empresa] PRIMARY KEY CLUSTERED  ([ID_Personal_Empresa])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Cliente_Empresa]'
GO
ALTER TABLE [dbo].[Cliente_Empresa] ADD CONSTRAINT [FK_Cliente_Empresa_Cliente] FOREIGN KEY ([ID_Cliente]) REFERENCES [dbo].[Cliente] ([ID_Cliente])
ALTER TABLE [dbo].[Cliente_Empresa] ADD CONSTRAINT [FK_Cliente_Empresa_Empresa] FOREIGN KEY ([ID_Empresa]) REFERENCES [dbo].[Empresa] ([ID_Empresa])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Entrada]'
GO
ALTER TABLE [dbo].[Entrada] ADD CONSTRAINT [FK_Entrada_Empresa] FOREIGN KEY ([ID_Empresa]) REFERENCES [dbo].[Empresa] ([ID_Empresa])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Personal_Empresa]'
GO
ALTER TABLE [dbo].[Personal_Empresa] ADD CONSTRAINT [FK_Personal_Empresa_Empresa] FOREIGN KEY ([ID_Empresa]) REFERENCES [dbo].[Empresa] ([ID_Empresa])
ALTER TABLE [dbo].[Personal_Empresa] ADD CONSTRAINT [FK_Personal_Empresa_Personal] FOREIGN KEY ([ID_Personal]) REFERENCES [dbo].[Personal] ([ID_Personal])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta]'
GO
ALTER TABLE [dbo].[Propuesta] ADD CONSTRAINT [FK_Propuesta_Empresa] FOREIGN KEY ([ID_Empresa]) REFERENCES [dbo].[Empresa] ([ID_Empresa])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Proveedor_Empresa]'
GO
ALTER TABLE [dbo].[Proveedor_Empresa] ADD CONSTRAINT [FK_Proveedor_Empresa_Empresa] FOREIGN KEY ([ID_Empresa]) REFERENCES [dbo].[Empresa] ([ID_Empresa])
ALTER TABLE [dbo].[Proveedor_Empresa] ADD CONSTRAINT [FK_Proveedor_Empresa_Proveedor] FOREIGN KEY ([ID_Proveedor]) REFERENCES [dbo].[Proveedor] ([ID_Proveedor])
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
         Configuration = "(H (1[50] 4[34] 2[5] 3) )"
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
         Top = -1344
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
         Begin Table = "Empresa"
            Begin Extent = 
               Top = 612
               Left = 31
               Bottom = 763
               Right = 240
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Empresa_1"
            Begin Extent = 
               Top = 604
               Left = 365
               Bottom = 733
               Right = 574
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
            ', 'SCHEMA', N'dbo', 'VIEW', N'C_Entrada', NULL, NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'DisplayFlags = 280
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
            DisplayFlags = 280
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
IF @@ERROR <> 0 SET NOEXEC ON
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
         Top = -192
         Left = -114
      End
      Begin Tables = 
         Begin Table = "Producto"
            Begin Extent = 
               Top = 39
               Left = 1190
               Bottom = 529
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
         Begin Table = "TempStockRealPorProducto"
            Begin Extent = 
               Top = 414
               Left = 827
               Bottom = 510
     ', 'SCHEMA', N'dbo', 'VIEW', N'C_Producto', NULL, NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'          Right = 1036
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_TipoSirena"
            Begin Extent = 
               Top = 455
               Left = 150
               Bottom = 574
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
EXEC sp_updateextendedproperty N'MS_DiagramPane3', N'    Begin ColumnWidths = 11
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
EXEC sp_updateextendedproperty N'MS_DiagramPane1', N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[54] 4[24] 2[8] 3) )"
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
         Begin Table = "Propuesta"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 610
               Right = 325
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta_Estado"
            Begin Extent = 
               Top = 4
               Left = 483
               Bottom = 123
               Right = 681
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta_Tipo"
            Begin Extent = 
               Top = 96
               Left = 538
               Bottom = 215
               Right = 736
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion"
            Begin Extent = 
               Top = 288
               Left = 701
               Bottom = 396
               Right = 897
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 288
               Left = 935
               Bottom = 396
               Right = 1124
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Empresa"
            Begin Extent = 
               Top = 713
               Left = 470
               Bottom = 842
               Right = 679
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Propuesta_1"
            Begin Extent = 
               Top = 81
               Left = 885
               Bottom = 286
               Right = 1086
            End
 ', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta', NULL, NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'           DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Entrada"
            Begin Extent = 
               Top = 0
               Left = 951
               Bottom = 130
               Right = 1201
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 761
               Left = 99
               Bottom = 891
               Right = 333
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FormaPago"
            Begin Extent = 
               Top = 470
               Left = 726
               Bottom = 600
               Right = 935
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Emplazamiento_Zona"
            Begin Extent = 
               Top = 542
               Left = 1000
               Bottom = 671
               Right = 1305
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "Instalacion_Emplazamiento"
            Begin Extent = 
               Top = 347
               Left = 429
               Bottom = 476
               Right = 678
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion_Emplazamiento_Planta"
            Begin Extent = 
               Top = 566
               Left = 420
               Bottom = 695
               Right = 707
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Grado"
            Begin Extent = 
               Top = 213
               Left = 465
               Bottom = 332
               Right = 663
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
      Begin ColumnWidths = 36
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
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2310
         Alias = 2340
         Table = 3315
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta', NULL, NULL
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
