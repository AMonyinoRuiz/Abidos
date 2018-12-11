/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestre    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosDomingo

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 18/11/2014 16:04:10

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
PRINT N'Dropping foreign keys from [dbo].[BI]'
GO
ALTER TABLE [dbo].[BI] DROP CONSTRAINT [FK_BI_Archivo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Informe_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Apartado_Version] DROP CONSTRAINT [FK__Informe_A__ID_In__02091B31]
ALTER TABLE [dbo].[Informe_Apartado_Version] DROP CONSTRAINT [FK__Informe_A__ID_In__5DCBBABB]
ALTER TABLE [dbo].[Informe_Apartado_Version] DROP CONSTRAINT [FK__Informe_A__ID_In__6B25B5D9]
ALTER TABLE [dbo].[Informe_Apartado_Version] DROP CONSTRAINT [FK_Informe_Apartado_Version_Informe_Apartado]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] DROP CONSTRAINT [FK_Menu_Menu]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Propuesta_Linea]'
GO
ALTER TABLE [dbo].[Propuesta_Linea] ADD
[DescripcionAmpliada_Tecnica] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Entrada_Linea]'
GO
ALTER TABLE [dbo].[Entrada_Linea] ADD
[CodigoProductoProveedor] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NULL,
[DescripcionAmpliada_Tecnica] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Producto]'
GO
ALTER TABLE [dbo].[Producto] ADD
[DescripcionAmpliada_Tecnica] [nvarchar] (max) COLLATE Modern_Spanish_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[RetornaPersonalDiasTrabajados]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER FUNCTION [dbo].[RetornaPersonalDiasTrabajados]()
Returns @Taula Table
(
IDPersonal int, NombrePersonal nvarchar(200), Fecha datetime, Año int, Mes int, EsFinDeSemana bit, EsFestivoEmpresa bit, SeAusento bit, EstabaDeBaja bit, HorasDiaPorContrato Decimal(4,2), HorasTrabajadas Decimal(8,2), HorasNoImputadas Decimal(8,2), HorasFacturadas Decimal(8,2),  HorasNoFacturadasEnFuncionDeLasImputadas Decimal(8,2), HorasNoFacturadasEnFuncionDeLasTrabajables Decimal(8,2), PrecioCoste Decimal(8,2), ImporteFacturado Decimal(8,2), HorasImputadasAlbaranCerradoManualmente Decimal(8,2)
)
as 
Begin

    -- Tabla temporal con las fechas de inicio y cierre

DECLARE @Año int
DECLARE @Mes int
DECLARE	@IDPersonal int
DECLARE	@NombrePersonal nvarchar(200)
DECLARE @FechaAlta Smalldatetime
DECLARE @FechaBaja Smalldatetime
DECLARE @EsFinDeSemana bit
DECLARE @EsFestivoEmpresa bit
DECLARE @SeAusento bit
DECLARE @EstabaDeBaja bit
DECLARE @HorasDiaPorContrato Decimal(4,2)
DECLARE @HorasTrabajadas Decimal(8,2)
DECLARE @HorasNoImputadas Decimal(8,2)
DECLARE @HorasFacturadas Decimal(8,2)
DECLARE @HorasNoFacturadasEnFuncionDeLasImputadas Decimal(8,2)
DECLARE @HorasNoFacturadasEnFuncionDeLasTrabajables Decimal(8,2)
DECLARE @PrecioCoste Decimal(8,2)
DECLARE @ImporteFacturado Decimal(8,2)
DECLARE @HorasImputadasAlbaranCerradoManualmente Decimal(8,2)

Declare CrPersonal Cursor Fast_Forward For Select ID_Personal, Nombre, FechaAltaEmpresa,  isnull(FechaBajaEmpresa,getdate()), HorasMinimas, PrecioCoste From Personal Where OcultarEnListados=0 Order by nombre
Open CrPersonal
Fetch CrPersonal Into @IDPersonal, @NombrePersonal, @FechaAlta, @FechaBaja, @HorasDiaPorContrato, @PrecioCoste
While @@Fetch_Status = 0
Begin
	Fetch CrPersonal Into @IDPersonal, @NombrePersonal, @FechaAlta, @FechaBaja, @HorasDiaPorContrato, @PrecioCoste
	
	WHILE @FechaAlta < @FechaBaja
    BEGIN
		if DatePart(WEEKDAY,@FechaAlta)=6 or DatePart(WEEKDAY,@FechaAlta)=7 
			set @EsFinDeSemana=1
		else
			set @EsFinDeSemana=0
	
		if (Select Count(*) From Empresa_FechasNoLaborables Where Fecha=@FechaAlta)=1
			set @EsFestivoEmpresa=1
		else
			set @EsFestivoEmpresa=0

		if (Select Count(*) From Personal_Ausencia Where Fecha=@FechaAlta and ID_Personal=@IDPersonal)=1
			set @SeAusento=1
		else
			set @SeAusento=0

		if (Select Count(*) From Personal_Baja Where @FechaAlta Between FechaInicio and FechaFin and ID_Personal=@IDPersonal)=1
			set @EstabaDeBaja=1
		else
			set @EstabaDeBaja=0

		set @HorasTrabajadas= (Select isnull(Sum(Horas+HorasExtras),0) From Parte_Horas Where Fecha=@FechaAlta and ID_Personal=@IDPersonal)
		set @HorasNoImputadas=@HorasDiaPorContrato-@HorasTrabajadas
		set @HorasFacturadas= (Select isnull(Sum(Horas+HorasExtras),0) From Parte_Horas, Entrada_Linea  Where Parte_Horas.ID_Entrada_Linea=Entrada_Linea.ID_Entrada_Linea and Fecha=@FechaAlta and ID_Personal=@IDPersonal and Entrada_Linea.ID_Entrada_Factura is not null)
		set @HorasNoFacturadasEnFuncionDeLasImputadas= @HorasTrabajadas - @HorasFacturadas 
		set @HorasNoFacturadasEnFuncionDeLasTrabajables=@HorasDiaPorContrato - @HorasFacturadas 

		set @ImporteFacturado= (Select isnull(Sum((Horas+HorasExtras)*ImporteHora),0) From Parte_Horas, Entrada_Linea, C_LADV_Personal_ResumenHorasPorMes_Aux2  Where C_LADV_Personal_ResumenHorasPorMes_Aux2.ID_Entrada_Linea=Parte_Horas.ID_Entrada_Linea and Parte_Horas.ID_Entrada_Linea=Entrada_Linea.ID_Entrada_Linea and Fecha=@FechaAlta and ID_Personal=@IDPersonal and Entrada_Linea.ID_Entrada_Factura is not null)

		set @HorasImputadasAlbaranCerradoManualmente=(Select isnull(Sum(Horas+HorasExtras),0) From Parte_Horas, Entrada_Linea, Entrada  Where Entrada.ID_Entrada=Entrada_Linea.ID_Entrada and Entrada.ID_Entrada_Estado=3 and Parte_Horas.ID_Entrada_Linea=Entrada_Linea.ID_Entrada_Linea and Fecha=@FechaAlta and Parte_Horas.ID_Personal=@IDPersonal and Entrada_Linea.ID_Entrada_Factura is null)

		
		
		SET @Año = Year(@FechaAlta)
		SET @Mes = Month(@FechaAlta)
				
		INSERT INTO @Taula VALUES(@IDPersonal, @NombrePersonal, @FechaAlta, @Año, @Mes, @EsFinDeSemana, @EsFestivoEmpresa, @SeAusento, @EstabaDeBaja, @HorasDiaPorContrato, @HorasTrabajadas,@HorasNoImputadas, @HorasFacturadas, @HorasNoFacturadasEnFuncionDeLasImputadas,@HorasNoFacturadasEnFuncionDeLasTrabajables, @PrecioCoste,@ImporteFacturado, @HorasImputadasAlbaranCerradoManualmente)
		SET @FechaAlta = dateadd(day, 1, @FechaAlta)
	END

	end
Close CrPersonal
Deallocate CrPersonal
RETURN 
end


GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_LADV_Personal_ResumenHorasPorMes]'
GO
ALTER VIEW dbo.C_LADV_Personal_ResumenHorasPorMes
AS
SELECT        TOP (100) PERCENT IDPersonal, NombrePersonal, Descripcion, Año, Mes,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (EsFinDeSemana = 0) AND (EsFestivoEmpresa = 0) AND (EstabaDeBaja = 0) AND (SeAusento = 0) AND 
                                                         (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) * HorasDiaPorContrato AS HorasTrabajables,
                             (SELECT        SUM(HorasTrabajadas) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) AS HorasImputadas,
                             (SELECT        SUM(HorasNoImputadas) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año) AND (EsFinDeSemana = 0) AND (EsFestivoEmpresa = 0) AND (SeAusento = 0) 
                                                         AND (EstabaDeBaja = 0)) AS HorasNoImputadas,
                             (SELECT        SUM(HorasFacturadas) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) AS HorasFacturadas,
                             (SELECT        SUM(HorasNoFacturadasEnFuncionDeLasImputadas) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) AS HorasNoFacturadasEnFuncionDeLasImputadas,
                             (SELECT        SUM(HorasNoFacturadasEnFuncionDeLasTrabajables) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (EsFinDeSemana = 0) AND (EsFestivoEmpresa = 0) AND (EstabaDeBaja = 0) AND (SeAusento = 0) AND 
                                                         (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) AS HorasNoFacturadasEnFuncionDeLasTrabajables, ImporteFacturado,
                             (SELECT        SUM(HorasImputadasAlbaranCerradoManualmente) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) AS HorasImputadasAlbaranCerradoManualmente,
                             (SELECT        SUM(HorasNoFacturadasEnFuncionDeLasImputadas) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) -
                             (SELECT        SUM(HorasImputadasAlbaranCerradoManualmente) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) 
                         AS HorasDiferenciaEntreNoFacturadoImputadasYAlbaranadasCerradasManualmente
FROM            dbo.C_LADV_Personal_ResumenHorasPorMes_Aux
ORDER BY NombrePersonal, Año, Mes
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[C_Remesa_Vencimientos]'
GO
ALTER VIEW dbo.C_Remesa_Vencimientos
AS
SELECT        dbo.Entrada_Vencimiento.ID_Entrada_Vencimiento, dbo.Cliente.Codigo AS Cliente_Codigo, dbo.Cliente.Nombre AS Cliente_Nombre, 
                         dbo.Cliente.NombreComercial AS Cliente_NombreComercial, dbo.Entrada.Codigo AS NumFactura, 
                         dbo.Cliente_CuentaBancaria.NombreBanco + N'-' + dbo.Cliente_CuentaBancaria.NumeroCuenta AS Cliente_CuentaBancaria, 
                         dbo.Empresa_CuentaBancaria.NombreBanco + N'-' + dbo.Empresa_CuentaBancaria.NumeroCuenta AS Empresa_CuentaBancaria, 
                         dbo.FormaPago.Descripcion AS FormaPago, dbo.Entrada_Vencimiento_Estado.Descripcion AS Vencimiento_Estado, dbo.Entrada_Vencimiento.Fecha, 
                         CASE dbo.Entrada.ID_Entrada_Tipo WHEN 12 THEN dbo.Entrada_Vencimiento.Importe * - 1 ELSE dbo.Entrada_Vencimiento.Importe END AS Importe, 
                         dbo.Entrada_Vencimiento.Pagado, dbo.Entrada_Vencimiento.Observaciones, dbo.Entrada_Vencimiento.Domiciliacion, dbo.Entrada_Vencimiento.ID_Remesa, 
                         dbo.Entrada_Vencimiento.ID_Entrada_Vencimiento_Estado, dbo.Entrada_Vencimiento.ID_Empresa_CuentaBancaria, dbo.Entrada_Vencimiento.ID_Entrada, 
                         dbo.Entrada.ID_Entrada_Tipo, dbo.Entrada_Vencimiento.ID_Cliente_CuentaBancaria, dbo.Cliente.Direccion AS Cliente_Direccion, 
                         dbo.Cliente.Poblacion AS Cliente_Poblacion, dbo.Cliente_CuentaBancaria.NumeroCuenta AS Cliente_NumeroCuentaBancaria
FROM            dbo.Entrada_Vencimiento INNER JOIN
                         dbo.Entrada ON dbo.Entrada_Vencimiento.ID_Entrada = dbo.Entrada.ID_Entrada INNER JOIN
                         dbo.Cliente ON dbo.Entrada.ID_Cliente = dbo.Cliente.ID_Cliente INNER JOIN
                         dbo.FormaPago ON dbo.Entrada_Vencimiento.ID_FormaPago = dbo.FormaPago.ID_FormaPago INNER JOIN
                         dbo.Empresa_CuentaBancaria ON dbo.Entrada_Vencimiento.ID_Empresa_CuentaBancaria = dbo.Empresa_CuentaBancaria.ID_Empresa_CuentaBancaria INNER JOIN
                         dbo.Cliente_CuentaBancaria ON dbo.Entrada_Vencimiento.ID_Cliente_CuentaBancaria = dbo.Cliente_CuentaBancaria.ID_Cliente_CuentaBancaria LEFT OUTER JOIN
                         dbo.Entrada_Vencimiento_Estado ON dbo.Entrada_Vencimiento.ID_Entrada_Vencimiento_Estado = dbo.Entrada_Vencimiento_Estado.ID_Entrada_Vencimiento_Estado
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Menus]'
GO
ALTER TABLE [dbo].[Menus] DROP
COLUMN [ID_Archivo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[BI]'
GO
ALTER TABLE [dbo].[BI] DROP
COLUMN [ID_Archivo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Informe_Apartado_Version]'
GO
ALTER TABLE [dbo].[Informe_Apartado_Version] ADD CONSTRAINT [FK_Informe_Apartado_Version_Informe_Apartado1] FOREIGN KEY ([ID_Informe_Apartado]) REFERENCES [dbo].[Informe_Apartado] ([ID_Informe_Apartado])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Listado]'
GO
ALTER TABLE [dbo].[Listado] ADD CONSTRAINT [FK_Listado_Listado_Entidad] FOREIGN KEY ([ID_Listado_Entidad]) REFERENCES [dbo].[Listado_Entidad] ([ID_Listado_Entidad])
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
         Configuration = "(H (1[31] 4[30] 2[21] 3) )"
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
         Begin Table = "C_LADV_Personal_ResumenHorasPorMes_Aux"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 193
               Right = 247
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
      Begin ColumnWidths = 12
         Width = 284
         Width = 1500
         Width = 3510
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2610
         Width = 1500
         Width = 2040
         Width = 1500
         Width = 2220
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 18510
         Alias = 5985
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_LADV_Personal_ResumenHorasPorMes', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_updateextendedproperty N'MS_DiagramPane2', N'     Right = 331
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
      Begin ColumnWidths = 24
         Width = 284
         Width = 2220
         Width = 1500
         Width = 2505
         Width = 1500
         Width = 1500
         Width = 3195
         Width = 3015
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2700
         Width = 3135
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
         Column = 7005
         Alias = 2745
         Table = 2685
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Remesa_Vencimientos', NULL, NULL
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
