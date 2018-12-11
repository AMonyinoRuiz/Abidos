/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 02/08/2014 22:48:41

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
PRINT N'Dropping foreign keys from [dbo].[Propuesta_Financiacion]'
GO
ALTER TABLE [dbo].[Propuesta_Financiacion] DROP CONSTRAINT [FK_Propuesta_FinanciacionMeses_FinanciacionMeses]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Propuesta_Opcion]'
GO
ALTER TABLE [dbo].[Propuesta_Opcion] DROP CONSTRAINT [FK_Propuesta_Opcion_Propuesta_Opcion_Accion]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Propuesta_Linea]'
GO
ALTER TABLE [dbo].[Propuesta_Linea] ADD
[ImporteOpcion] [decimal] (12, 4) NULL
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
                         dbo.Propuesta_Opcion_Accion.Descripcion AS [Opción acción], dbo.Propuesta_Linea.ImporteOpcion AS [Opción importe]
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
PRINT N'Adding foreign keys to [dbo].[Propuesta_Financiacion]'
GO
ALTER TABLE [dbo].[Propuesta_Financiacion] ADD CONSTRAINT [FK_Propuesta_FinanciacionMeses_FinanciacionMeses] FOREIGN KEY ([ID_FinanciacionMeses]) REFERENCES [dbo].[FinanciacionMeses] ([ID_FinanciacionMeses])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_Opcion]'
GO
ALTER TABLE [dbo].[Propuesta_Opcion] ADD CONSTRAINT [FK_Propuesta_Opcion_Propuesta_Opcion_Accion] FOREIGN KEY ([ID_Propuesta_Opcion_Accion]) REFERENCES [dbo].[Propuesta_Opcion_Accion] ([ID_Propuesta_Opcion_Accion])
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
               Top = 148
               Left = 468
               Bottom = 1210
               Right = 752
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "Producto"
            Begin Extent = 
               Top = 0
               Left = 870
               Bottom = 228
               Right = 1153
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Producto_Division"
            Begin Extent = 
               Top = 6
               Left = 516
               Bottom = 125
               Right = 714
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Instalacion"
            Begin Extent = 
               Top = 934
               Left = 127
               Bottom = 1061
              ', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N' Right = 368
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
         Begin Table = "Instalacion_ElementosAProteger"
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
               Top = 13
               Left = 311
               Bottom = 143
               Right = 520
            End
            DisplayFlags = 280
            TopColumn = 0', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane3', N'
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
         Wi', 'SCHEMA', N'dbo', 'VIEW', N'C_Propuesta_Linea', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane4', N'dth = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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