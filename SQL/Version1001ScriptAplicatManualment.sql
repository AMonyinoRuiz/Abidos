/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 30/01/2015 21:49:01

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
PRINT N'Dropping foreign keys from [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_ListadoADV]
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
PRINT N'Altering [dbo].[Entrada_Linea]'
GO
ALTER TABLE [dbo].[Entrada_Linea] ADD
[Coste] [decimal] (14, 4) NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Usuario]'
GO
ALTER TABLE [dbo].[Usuario] ADD
[Idioma] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[RetornaCalculos_Entrada_Linea]'
GO
CREATE Function [dbo].[RetornaCalculos_Entrada_Linea](@pIDLinea int)
Returns @Taula Table
(
ID_Entrada_Linea int, Unidades decimal(10, 2), Precio decimal(10, 4), Descuento1 decimal(10, 2), Descuento2 decimal(10, 2), IVA decimal(4, 2), Base Decimal(14,4), TotalBase Decimal(14,4), TotalIva Decimal(14,4), TotalLinea Decimal(14,4), PVP Decimal(14,4), PVD Decimal(14,4), PartesAsignados_NumeroHorasNormales Decimal (14,4), PartesAsignados_NumeroHorasExtras Decimal (14,4), PartesAsignados_ImporteHorasNormales Decimal (14,4), PartesAsignados_ImporteHorasExtras Decimal (14,4), PartesAsignados_ImporteGastos Decimal (14,4), CompuestoPor_TotalCoste Decimal (14,4), CompuestoPor_NumConceptos int, Linea_TotalCostes Decimal(14,4), Linea_Margen Decimal(14,4), Linea_MargenPorcentaje Decimal(10,4)

)
as 
Begin
DECLARE @Cantidad Decimal(10,2)
DECLARE @Precio Decimal(10,4)
DECLARE @Descuento1 Decimal(10,2)
DECLARE @Descuento2 Decimal(10,2)
DECLARE @IVA Decimal(4,2)
DECLARE @Base Decimal(14,2)
DECLARE @TotalBase Decimal(14,2)
DECLARE @TotalIva Decimal(14,2)
DECLARE @TotalLinea Decimal(14,2)

DECLARE @IDProducto int
DECLARE @PVP_Predeterminado bit  -- Variable per saber si s'usa el preu predeterminat del article o es posa un preu per a cada proveïdor
DECLARE @PVP Decimal(14,4)
DECLARE @PVD Decimal(14,4)
DECLARE @PartesAsignados_NumeroHorasNormales Decimal (14,4)
DECLARE @PartesAsignados_NumeroHorasExtras Decimal (14,4)
DECLARE @PartesAsignados_ImporteHorasNormales Decimal (14,4)
DECLARE @PartesAsignados_ImporteHorasExtras Decimal (14,4)
DECLARE @PartesAsignados_ImporteGastos Decimal (14,4)
DECLARE @CompuestoPor_TotalCoste Decimal (14,4)
DECLARE @CompuestoPor_NumConceptos int

DECLARE @Linea_TotalCostes Decimal(14,4)
DECLARE @NoRestarStock as bit

DECLARE @Linea_Margen Decimal(14,4)
DECLARE @Linea_MargenPorcentaje Decimal(10,4)


Select @Cantidad=Unidad, @Precio=Precio, @Descuento1=Descuento1, @Descuento2=Descuento2, @IVA=Iva, @TotalBase=TotalBase, @TotalIVA=TotalIva, @TotalLinea=TotalLinea, @IDProducto=ID_Producto, @NoRestarStock=NoRestarStock, @PVD=Coste From Entrada_Linea Where ID_Entrada_Linea=@pIDLinea

/*
Select @PVP_Predeterminado=PVP_Proveedor_Predeterminado, @PVP=PVP, @PVD=PVD From Producto Where ID_Producto=@IDProducto

if @PVP_Predeterminado=1
begin
	if exists (Select * From Producto_Proveedor Where ID_Producto=@IDProducto and Predeterminado=1)
	begin
		Select  @PVP=PVP, @PVD=PVD From Producto_Proveedor Where ID_Producto=@IDProducto and Predeterminado=1
	end
	else
	Begin
		set @PVP=0
		set @PVD=0
	End
end
*/

Select @PartesAsignados_NumeroHorasNormales=Sum(Horas) From Parte_Horas Where ID_Entrada_Linea=@PIDLinea
Select @PartesAsignados_NumeroHorasExtras=Sum(HorasExtras) From Parte_Horas Where ID_Entrada_Linea=@PIDLinea

Set @PartesAsignados_NumeroHorasNormales= isnull(@PartesAsignados_NumeroHorasNormales,0)
Set @PartesAsignados_NumeroHorasExtras= isnull(@PartesAsignados_NumeroHorasExtras,0)

Select @PartesAsignados_ImporteHorasNormales=Sum(Horas*PrecioCoste) From Parte_Horas, Personal Where ID_Entrada_Linea=@PIDLinea and Parte_Horas.ID_Personal=Personal.ID_Personal
Select @PartesAsignados_ImporteHorasExtras=Sum(Horas*PrecioCosteHoraExtra) From Parte_Horas, Personal Where ID_Entrada_Linea=@PIDLinea and Parte_Horas.ID_Personal=Personal.ID_Personal

Set @PartesAsignados_ImporteHorasNormales= isnull(@PartesAsignados_ImporteHorasNormales,0)
Set @PartesAsignados_ImporteHorasExtras= isnull(@PartesAsignados_ImporteHorasExtras,0)

Select @PartesAsignados_ImporteGastos=Sum(Gasto) From Parte_Gastos Where ID_Entrada_Linea=@PIDLinea 
Set @PartesAsignados_ImporteGastos= isnull(@PartesAsignados_ImporteGastos,0)


Select @CompuestoPor_TotalCoste= Sum(Coste*Unidad) From Entrada_Linea Where ID_Entrada_Linea_Padre=@PIDLinea 
Set @CompuestoPor_TotalCoste= isnull(@CompuestoPor_TotalCoste,0)

Select @CompuestoPor_NumConceptos= Count(*) From Entrada_Linea Where ID_Entrada_Linea_Padre=@PIDLinea 
Set @CompuestoPor_NumConceptos= isnull(@CompuestoPor_NumConceptos,0)

set @PVD=isnull(@PVD,0)

Set @Linea_TotalCostes=(@PVD*@Cantidad* ~@NoRestarStock) + @PartesAsignados_ImporteHorasNormales + @PartesAsignados_ImporteHorasExtras + @PartesAsignados_ImporteGastos + @CompuestoPor_TotalCoste
Set @Linea_TotalCostes= isnull(@Linea_TotalCostes,0)

Set @Linea_Margen=@TotalBase - @Linea_TotalCostes 
Set @Linea_Margen=isnull(@Linea_Margen,0)

if @TotalBase=0 
begin
	Set @Linea_MargenPorcentaje=0
end
else
begin
	Set @Linea_MargenPorcentaje=(@Linea_Margen*100) / @TotalBase
end

Set @Linea_MargenPorcentaje=isnull(@Linea_MargenPorcentaje,0)

Insert into @Taula Values (@pIDLinea, @Cantidad, @Precio, @Descuento1, @Descuento2, @IVA, @Base, @TotalBase, @TotalIva, @TotalLinea, @PVP, @PVD, @PartesAsignados_NumeroHorasNormales, @PartesAsignados_NumeroHorasExtras, @PartesAsignados_ImporteHorasNormales,@PartesAsignados_ImporteHorasExtras, @PartesAsignados_ImporteGastos, @CompuestoPor_TotalCoste, @CompuestoPor_NumConceptos, @Linea_TotalCostes, @Linea_Margen, @Linea_MargenPorcentaje)
Return
End
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Entrada_Linea]'
GO
ALTER VIEW dbo.C_Entrada_Linea
AS
SELECT        dbo.Entrada_Linea.ID_Entrada_Linea, dbo.Entrada_Linea.ID_Entrada, dbo.Entrada_Linea.ID_Producto, dbo.Entrada_Linea.ID_Almacen, 
                         dbo.Entrada_Linea.ID_Entrada_Linea_Pedido, dbo.Entrada_Linea.ID_Entrada_Factura, dbo.Entrada_Linea.FechaEntrada, dbo.Entrada_Linea.Unidad, 
                         dbo.Entrada_Linea.Precio, dbo.Entrada_Linea.Descuento1, dbo.Entrada_Linea.Descuento2, dbo.Entrada_Linea.IVA, dbo.Entrada_Linea.TotalBase, 
                         dbo.Entrada_Linea.TotalIVA, dbo.Entrada_Linea.TotalLinea, dbo.Entrada_Linea.CantidadTraspasada, dbo.Entrada_Linea.StockActivo, 
                         dbo.Entrada_Linea.ID_DocumentoInicializacionStocks, dbo.Entrada_Linea.FechaEntrega, dbo.Entrada_Linea.ID_Producto_Garantia, 
                         dbo.Entrada_Linea.FechaFinGarantia, dbo.Entrada_Linea.Descripcion, dbo.Entrada_Linea.Uso, dbo.Entrada_Linea.PeriodoInicio, dbo.Entrada_Linea.PeriodoFin, 
                         dbo.Entrada_Linea.ID_Instalacion, dbo.Entrada_Linea.ID_Instalacion_Contrato, dbo.Entrada_Linea.DescripcionAmpliada, dbo.Entrada_Linea.Observaciones, 
                         dbo.Entrada_Linea.ReferenciaNumeroPedidoDeCompra, dbo.Entrada_Linea.ReferenciaLinea, dbo.Entrada_Linea.ReferenciaProyecto, 
                         dbo.Entrada_Linea.ReferenciaNombreObra, dbo.Entrada_Linea.ReferenciaNum, dbo.Entrada_Linea.AmidamientosFase, 
                         dbo.Entrada_Linea.AmidamientosReferenciaEnObra, dbo.Producto.Codigo AS Producto_Codigo, dbo.Almacen.Descripcion AS Almacen_Descripcion, 
                         dbo.Entrada_Linea.ID_Instalacion_Emplazamiento, dbo.Entrada_Linea.ID_Instalacion_Emplazamiento_Planta, 
                         dbo.Entrada_Linea.ID_Instalacion_Emplazamiento_Zona, dbo.Entrada_Linea.ID_Instalacion_Emplazamiento_Abertura, 
                         dbo.Entrada_Linea.ID_Instalacion_ElementosAProteger, dbo.Entrada_Linea.ID_Entrada_Linea_Padre, dbo.Entrada.ID_Entrada_Tipo, dbo.Entrada.ID_Entrada_Estado,
                          dbo.Producto.RequiereNumeroSerie, '' AS Foto, dbo.Entrada.NumeroDocumentoProveedor, dbo.Entrada_Linea.NoRestarStock, 
                         [Factura(Entrada)].Codigo AS NumFactura, dbo.Entrada_Linea.Coste
FROM            dbo.Entrada_Linea INNER JOIN
                         dbo.Entrada ON dbo.Entrada_Linea.ID_Entrada = dbo.Entrada.ID_Entrada INNER JOIN
                         dbo.Producto ON dbo.Entrada_Linea.ID_Producto = dbo.Producto.ID_Producto INNER JOIN
                         dbo.Almacen ON dbo.Entrada_Linea.ID_Almacen = dbo.Almacen.ID_Almacen LEFT OUTER JOIN
                         dbo.Entrada AS [Factura(Entrada)] ON dbo.Entrada_Linea.ID_Entrada_Factura = [Factura(Entrada)].ID_Entrada
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Entrada_Linea_ConCalculos]'
GO
CREATE VIEW dbo.C_Entrada_Linea_ConCalculos
AS
SELECT        C_Entrada_Linea.*, b.PartesAsignados_NumeroHorasNormales, b.PartesAsignados_NumeroHorasExtras, b.PartesAsignados_ImporteHorasNormales, 
                         b.PartesAsignados_ImporteHorasExtras, b.PartesAsignados_ImporteGastos, b.CompuestoPor_TotalCoste, b.CompuestoPor_NumConceptos, 
                         b.Linea_TotalCostes, b.Linea_Margen, b.Linea_MargenPorcentaje
FROM            dbo.C_Entrada_Linea CROSS Apply dbo.RetornaCalculos_Entrada_Linea(C_Entrada_Linea.ID_Entrada_Linea) AS B
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
                         dbo.Parte.TrabajoARealizar, dbo.Parte_Horas_TipoActuacion.Descripcion, dbo.Entrada.Codigo AS CódigoDocumento
FROM            dbo.Entrada INNER JOIN
                         dbo.Entrada_Linea ON dbo.Entrada.ID_Entrada = dbo.Entrada_Linea.ID_Entrada RIGHT OUTER JOIN
                         dbo.Parte_Horas INNER JOIN
                         dbo.Personal ON dbo.Parte_Horas.ID_Personal = dbo.Personal.ID_Personal INNER JOIN
                         dbo.Parte_Horas_Estado ON dbo.Parte_Horas.ID_Parte_Horas_Estado = dbo.Parte_Horas_Estado.ID_Parte_Horas_Estado INNER JOIN
                         dbo.Parte ON dbo.Parte_Horas.ID_Parte = dbo.Parte.ID_Parte ON dbo.Entrada_Linea.ID_Entrada_Linea = dbo.Parte_Horas.ID_Entrada_Linea LEFT OUTER JOIN
                         dbo.Parte_Horas_TipoActuacion ON dbo.Parte_Horas.ID_Parte_Horas_TipoActuacion = dbo.Parte_Horas_TipoActuacion.ID_Parte_Horas_TipoActuacion
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FORM_Controls]'
GO
ALTER TABLE [dbo].[FORM_Controls] ADD
[Idioma1] [nvarchar] (75) COLLATE Modern_Spanish_CI_AS NULL,
[Idioma2] [nvarchar] (75) COLLATE Modern_Spanish_CI_AS NULL,
[Idioma3] [nvarchar] (75) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[GRID_Columna]'
GO
ALTER TABLE [dbo].[GRID_Columna] ADD
[Idioma1] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[Idioma2] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[Idioma3] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] ADD CONSTRAINT [FK_Menu_ListadoADV] FOREIGN KEY ([ID_ListadoADV]) REFERENCES [dbo].[ListadoADV] ([ID_ListadoADV])
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
PRINT N'Adding foreign keys to [dbo].[Parte]'
GO
ALTER TABLE [dbo].[Parte] ADD CONSTRAINT [FK_Parte_Instalacion_Contrato] FOREIGN KEY ([ID_Instalacion_Contrato]) REFERENCES [dbo].[Instalacion_Contrato] ([ID_Instalacion_Contrato])
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
         Configuration = "(H (1[49] 4[24] 2[11] 3) )"
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
         Begin Table = "Entrada_Linea"
            Begin Extent = 
               Top = 25
               Left = 359
               Bottom = 347
               Right = 629
            End
            DisplayFlags = 280
            TopColumn = 38
         End
         Begin Table = "Entrada"
            Begin Extent = 
               Top = 1
               Left = 30
               Bottom = 250
               Right = 239
            End
            DisplayFlags = 280
            TopColumn = 40
         End
         Begin Table = "Producto"
            Begin Extent = 
               Top = 0
               Left = 731
               Bottom = 247
               Right = 1060
            End
            DisplayFlags = 280
            TopColumn = 108
         End
         Begin Table = "Almacen"
            Begin Extent = 
               Top = 254
               Left = 31
               Bottom = 383
               Right = 240
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Factura(Entrada)"
            Begin Extent = 
               Top = 384
               Left = 38
               Bottom = 513
               Right = 288
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
         Column = 3825
         Al', 'SCHEMA', N'dbo', 'VIEW', N'C_Entrada_Linea', NULL, NULL
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
         Configuration = "(H (1[54] 4[14] 2[11] 3) )"
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
               Top = 156
               Left = 1135
               Bottom = 379
               Right = 1385
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Entrada_Linea"
            Begin Extent = 
               Top = 255
               Left = 718
               Bottom = 490
               Right = 1018
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Horas"
            Begin Extent = 
               Top = 0
               Left = 305
               Bottom = 413
               Right = 605
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 9
               Left = 656
               Bottom = 138
               Right = 890
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte_Horas_Estado"
            Begin Extent = 
               Top = 150
               Left = 723
               Bottom = 245
               Right = 932
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Parte"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 326
               Right = 266
            End
            DisplayFlags = 280
            TopColumn = 27
         End
         Begin Table = "Parte_Horas_TipoActuacion"
            Begin Extent = 
               Top = 330
               Left = 38
               Bottom = 426
               Right = 289
     ', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_Horas', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'       End
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Parte_Horas', NULL, NULL
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
         Configuration = "(H (1[42] 4[21] 2[9] 3) )"
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
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 60
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
         SortOrder = 1', 'SCHEMA', N'dbo', 'VIEW', N'C_Entrada_Linea_ConCalculos', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N'410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', 'SCHEMA', N'dbo', 'VIEW', N'C_Entrada_Linea_ConCalculos', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Entrada_Linea_ConCalculos', NULL, NULL
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