/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 03/11/2014 12:35:53

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
PRINT N'Dropping foreign keys from [dbo].[ActividadCRM]'
GO
ALTER TABLE [dbo].[ActividadCRM] DROP CONSTRAINT [FK_ActividadCRM_Usuario]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Entrada_Vencimiento]'
GO
ALTER TABLE [dbo].[Entrada_Vencimiento] DROP CONSTRAINT [FK_Entrada_Vencimiento_UbicacionPagosYCobros]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[UbicacionPagosYCobros]'
GO
ALTER TABLE [dbo].[UbicacionPagosYCobros] DROP CONSTRAINT [PK_UbicacionPagosYCobros]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping [dbo].[UbicacionPagosYCobros]'
GO
DROP TABLE [dbo].[UbicacionPagosYCobros]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FormaPago]'
GO
ALTER TABLE [dbo].[FormaPago] ADD
[ID_Empresa_CuentaBancaria] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[EliminarMaestro]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[EliminarMaestro](@pIDMaestro int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
Delete From Maestro Where ID_Maestro=@pIDMaestro
DELETE FROM Formulario_Usuario_Grupo
WHERE        (ID_Formulario =
                             (SELECT        ID_Formulario
                               FROM            Formulario
                               WHERE        (ParametroEntrada = @pIDMaestro)))
Delete From Menus 
WHERE        (ID_Formulario =
                             (SELECT        ID_Formulario
                               FROM            Formulario
                               WHERE        (ParametroEntrada = @pIDMaestro)))
Delete From Formulario WHERE (ParametroEntrada = @pIDMaestro)
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Entrada_Vencimiento]'
GO
ALTER TABLE [dbo].[Entrada_Vencimiento] ADD
[Domiciliacion] [bit] NOT NULL CONSTRAINT [DF_Entrada_Vencimiento_Domiciliacion_1] DEFAULT ((0)),
[ID_Cliente_CuentaBancaria] [int] NULL,
[ID_Proveedor_CuentaBancaria] [int] NULL,
[ID_Remesa] [int] NULL,
[ID_FormaPago] [int] NULL,
[ID_Entrada_Vencimiento_Estado] [int] NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[Entrada_Vencimiento].[ID_UbicacionPagosYCobros]', N'ID_Empresa_CuentaBancaria', 'COLUMN'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Remesa]'
GO
CREATE TABLE [dbo].[Remesa]
(
[ID_Remesa] [int] NOT NULL IDENTITY(1, 1),
[FechaAlta] [smalldatetime] NOT NULL,
[FechaRemesa] [smalldatetime] NOT NULL,
[ID_Empresa_CuentaBancaria] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Remesa] on [dbo].[Remesa]'
GO
ALTER TABLE [dbo].[Remesa] ADD CONSTRAINT [PK_Remesa] PRIMARY KEY CLUSTERED  ([ID_Remesa])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Empresa_CuentaBancaria]'
GO
CREATE TABLE [dbo].[Empresa_CuentaBancaria]
(
[ID_Empresa_CuentaBancaria] [int] NOT NULL IDENTITY(1, 1),
[ID_Empresa] [int] NOT NULL,
[NombreBanco] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[NumeroCuenta] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Observaciones] [nvarchar] (500) COLLATE Modern_Spanish_CI_AS NULL,
[Domiciliacion] [bit] NOT NULL CONSTRAINT [DF_Empresa_CuentaBancaria_Domiciliacion] DEFAULT ((0)),
[BIC] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Empresa_CuentaBancaria] on [dbo].[Empresa_CuentaBancaria]'
GO
ALTER TABLE [dbo].[Empresa_CuentaBancaria] ADD CONSTRAINT [PK_Empresa_CuentaBancaria] PRIMARY KEY CLUSTERED  ([ID_Empresa_CuentaBancaria])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Remesa]'
GO

CREATE VIEW [dbo].[C_Remesa]
AS
SELECT        dbo.Remesa.ID_Remesa, dbo.Remesa.FechaAlta AS [Fecha Alta], dbo.Remesa.FechaRemesa AS [Fecha remesa], dbo.Remesa.ID_Empresa_CuentaBancaria, 
                         dbo.Empresa_CuentaBancaria.NombreBanco + '-' + dbo.Empresa_CuentaBancaria.NumeroCuenta AS [Cuenta bancaria asociada],
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.Entrada_Vencimiento AS a
                               WHERE        (dbo.Remesa.ID_Remesa = ID_Remesa)) AS [Número de recibos]
FROM            dbo.Remesa INNER JOIN
                         dbo.Empresa_CuentaBancaria ON dbo.Remesa.ID_Empresa_CuentaBancaria = dbo.Empresa_CuentaBancaria.ID_Empresa_CuentaBancaria

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Entrada_Vencimiento_Estado]'
GO
CREATE TABLE [dbo].[Entrada_Vencimiento_Estado]
(
[ID_Entrada_Vencimiento_Estado] [int] NOT NULL IDENTITY(1, 1),
[Descripcion] [nvarchar] (75) COLLATE Modern_Spanish_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Entrada_Vencimiento_Estado] on [dbo].[Entrada_Vencimiento_Estado]'
GO
ALTER TABLE [dbo].[Entrada_Vencimiento_Estado] ADD CONSTRAINT [PK_Entrada_Vencimiento_Estado] PRIMARY KEY CLUSTERED  ([ID_Entrada_Vencimiento_Estado])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Cliente_CuentaBancaria]'
GO
ALTER TABLE [dbo].[Cliente_CuentaBancaria] ADD
[Domiciliacion] [bit] NOT NULL CONSTRAINT [DF_Cliente_CuentaBancaria_Domiciliacion_1] DEFAULT ((0)),
[FechaAceptacionDomiciliacion] [smalldatetime] NULL,
[Predeterminada] [bit] NOT NULL CONSTRAINT [DF_Cliente_CuentaBancaria_Predeterminada_1] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Remesa_Vencimientos]'
GO

CREATE VIEW [dbo].[C_Remesa_Vencimientos]
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
                         dbo.Cliente.Poblacion AS Cliente_Poblacion, dbo.Empresa_CuentaBancaria.NumeroCuenta AS Cliente_NumeroCuentaBancaria
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
PRINT N'Creating [dbo].[RetornaNumerosDeFacturaDeUnaRemesaParaUnaCuentaCliente]'
GO

CREATE FUNCTION [dbo].[RetornaNumerosDeFacturaDeUnaRemesaParaUnaCuentaCliente]

(

  @pIDRemesa int,

  @pIDClienteCuentaBancaria int

)

RETURNS VARCHAR(MAX)

AS 

BEGIN

   DECLARE @MVtextList varchar(max)

   /*Set @MVTextList='Facturas: ' */

   SELECT @MVtextList = COALESCE(@MVtextList + '', '') + Cast([NumFactura] as nvarchar(50)) + ' - '

   FROM C_Remesa_Vencimientos Where ID_Remesa=@pIDRemesa and ID_Cliente_CuentaBancaria=@pIDClienteCuentaBancaria 

   
   RETURN @MVtextList

END


GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_Remesa_ExportacionXML]'
GO

CREATE VIEW [dbo].[C_Remesa_ExportacionXML]
AS
SELECT        ID_Remesa, ID_Empresa_CuentaBancaria, ID_Cliente_CuentaBancaria, SUM(Importe) AS Importe, COUNT(*) AS NumRecibos, 
                         dbo.RetornaNumerosDeFacturaDeUnaRemesaParaUnaCuentaCliente(ID_Remesa, ID_Cliente_CuentaBancaria) AS NumFacturas, Cliente_Direccion, 
                         Cliente_Poblacion, Cliente_Nombre, MAX(ID_Entrada_Vencimiento) AS ID_Entrada_Vencimiento, Cliente_NumeroCuentaBancaria
FROM            dbo.C_Remesa_Vencimientos
GROUP BY ID_Remesa, ID_Empresa_CuentaBancaria, ID_Cliente_CuentaBancaria, dbo.RetornaNumerosDeFacturaDeUnaRemesaParaUnaCuentaCliente(ID_Remesa, 
                         ID_Cliente_CuentaBancaria), Cliente_Direccion, Cliente_Poblacion, Cliente_Nombre, Cliente_NumeroCuentaBancaria

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_LADV_Personal_ResumenHorasPorMes_Aux2]'
GO

CREATE VIEW [dbo].[C_LADV_Personal_ResumenHorasPorMes_Aux2]
AS
SELECT        ID_Entrada_Linea, CASE TotalBase WHEN 0 THEN 0 ELSE Round((TotalBase / TotalHoras), 2) END AS Importehora
FROM            (SELECT        ID_Entrada_Linea, TotalBase, ISNULL
                                                        ((SELECT        SUM(Horas + HorasExtras) AS Expr1
                                                            FROM            dbo.Parte_Horas
                                                            WHERE        (ID_Entrada_Linea = dbo.Entrada_Linea.ID_Entrada_Linea)), 0) AS TotalHoras
                          FROM            dbo.Entrada_Linea
                          WHERE        (ID_Entrada_Factura IS NOT NULL)) AS pepe
WHERE        (TotalHoras > 0)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[RetornaPersonalDiasTrabajados]'
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[RetornaPersonalDiasTrabajados]()
Returns @Taula Table
(
IDPersonal int, NombrePersonal nvarchar(200), Fecha datetime, Año int, Mes int, EsFinDeSemana bit, EsFestivoEmpresa bit, SeAusento bit, EstabaDeBaja bit, HorasDiaPorContrato Decimal(4,2), HorasTrabajadas Decimal(8,2), HorasNoImputadas Decimal(8,2), HorasFacturadas Decimal(8,2),  HorasNoFacturadasEnFuncionDeLasImputadas Decimal(8,2), HorasNoFacturadasEnFuncionDeLasTrabajables Decimal(8,2), PrecioCoste Decimal(8,2), ImporteFacturado Decimal(8,2)
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



		
		
		SET @Año = Year(@FechaAlta)
		SET @Mes = Month(@FechaAlta)
				
		INSERT INTO @Taula VALUES(@IDPersonal, @NombrePersonal, @FechaAlta, @Año, @Mes, @EsFinDeSemana, @EsFestivoEmpresa, @SeAusento, @EstabaDeBaja, @HorasDiaPorContrato, @HorasTrabajadas,@HorasNoImputadas, @HorasFacturadas, @HorasNoFacturadasEnFuncionDeLasImputadas,@HorasNoFacturadasEnFuncionDeLasTrabajables, @PrecioCoste,@ImporteFacturado)
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
PRINT N'Creating [dbo].[C_LADV_Personal_ResumenHorasPorMes_Aux]'
GO

CREATE VIEW [dbo].[C_LADV_Personal_ResumenHorasPorMes_Aux]
AS
SELECT        RetornaPersonalDiasTrabajados_1.IDPersonal, RetornaPersonalDiasTrabajados_1.NombrePersonal, dbo.Personal_Tipo.Descripcion, 
                         RetornaPersonalDiasTrabajados_1.Año, RetornaPersonalDiasTrabajados_1.Mes, RetornaPersonalDiasTrabajados_1.HorasDiaPorContrato, 
                         SUM(RetornaPersonalDiasTrabajados_1.ImporteFacturado) AS ImporteFacturado
FROM            dbo.Personal_Tipo INNER JOIN
                         dbo.Personal ON dbo.Personal_Tipo.ID_Personal_Tipo = dbo.Personal.ID_Personal_Tipo INNER JOIN
                         dbo.RetornaPersonalDiasTrabajados() AS RetornaPersonalDiasTrabajados_1 ON dbo.Personal.ID_Personal = RetornaPersonalDiasTrabajados_1.IDPersonal
GROUP BY RetornaPersonalDiasTrabajados_1.IDPersonal, RetornaPersonalDiasTrabajados_1.NombrePersonal, dbo.Personal_Tipo.Descripcion, 
                         RetornaPersonalDiasTrabajados_1.Año, RetornaPersonalDiasTrabajados_1.Mes, RetornaPersonalDiasTrabajados_1.HorasDiaPorContrato

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[C_LADV_Personal_ResumenHorasPorMes]'
GO

CREATE VIEW [dbo].[C_LADV_Personal_ResumenHorasPorMes]
AS
SELECT        TOP (100) PERCENT IDPersonal, NombrePersonal, Descripcion, Año, Mes,
                             (SELECT        COUNT(*) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (EsFinDeSemana = 0) AND (EsFestivoEmpresa = 0) AND (EstabaDeBaja = 0) AND 
                                                         (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) * HorasDiaPorContrato AS HorasTrabajables,
                             (SELECT        SUM(HorasTrabajadas) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) AS HorasImputadas,
                             (SELECT        SUM(HorasNoImputadas) AS Expr1
                               FROM            dbo.RetornaPersonalDiasTrabajados() AS a
                               WHERE        (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año) AND (EsFinDeSemana = 0) AND (EsFestivoEmpresa = 0) AND (EstabaDeBaja = 0))
                          AS HorasNoImputadas,
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
                               WHERE        (EsFinDeSemana = 0) AND (EsFestivoEmpresa = 0) AND (EstabaDeBaja = 0) AND 
                                                         (dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.IDPersonal = IDPersonal) AND (Mes = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Mes) 
                                                         AND (Año = dbo.C_LADV_Personal_ResumenHorasPorMes_Aux.Año)) AS HorasNoFacturadasEnFuncionDeLasTrabajables, ImporteFacturado
FROM            dbo.C_LADV_Personal_ResumenHorasPorMes_Aux
ORDER BY NombrePersonal, Año, Mes

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FormaPago_Tipo]'
GO
ALTER TABLE [dbo].[FormaPago_Tipo] ADD
[GenerarDomiciliacion] [bit] NOT NULL CONSTRAINT [DF_FormaPago_Tipo_GenerarDomiciliacion] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Proveedor_CuentaBancaria]'
GO
ALTER TABLE [dbo].[Proveedor_CuentaBancaria] ADD
[Domiciliacion] [bit] NOT NULL CONSTRAINT [DF_Proveedor_CuentaBancaria_Domiciliacion_1] DEFAULT ((0)),
[FechaAceptacionDomiciliacion] [smalldatetime] NULL,
[Predeterminada] [bit] NOT NULL CONSTRAINT [DF_Proveedor_CuentaBancaria_Predeterminada_1] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Remesa_Archivo]'
GO
CREATE TABLE [dbo].[Remesa_Archivo]
(
[ID_Remesa_Archivo] [int] NOT NULL,
[ID_Archivo] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_FECHA] on [dbo].[Parte_Horas]'
GO
CREATE NONCLUSTERED INDEX [IX_FECHA] ON [dbo].[Parte_Horas] ([Fecha])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Entrada_Vencimiento]'
GO
ALTER TABLE [dbo].[Entrada_Vencimiento] ADD CONSTRAINT [FK_Entrada_Vencimiento_Empresa_CuentaBancaria] FOREIGN KEY ([ID_Empresa_CuentaBancaria]) REFERENCES [dbo].[Empresa_CuentaBancaria] ([ID_Empresa_CuentaBancaria])
ALTER TABLE [dbo].[Entrada_Vencimiento] ADD CONSTRAINT [FK_Entrada_Vencimiento_Entrada_Vencimiento_Estado] FOREIGN KEY ([ID_Entrada_Vencimiento_Estado]) REFERENCES [dbo].[Entrada_Vencimiento_Estado] ([ID_Entrada_Vencimiento_Estado])
ALTER TABLE [dbo].[Entrada_Vencimiento] ADD CONSTRAINT [FK_Entrada_Vencimiento_Cliente_CuentaBancaria] FOREIGN KEY ([ID_Cliente_CuentaBancaria]) REFERENCES [dbo].[Cliente_CuentaBancaria] ([ID_Cliente_CuentaBancaria])
ALTER TABLE [dbo].[Entrada_Vencimiento] ADD CONSTRAINT [FK_Entrada_Vencimiento_Proveedor_CuentaBancaria] FOREIGN KEY ([ID_Proveedor_CuentaBancaria]) REFERENCES [dbo].[Proveedor_CuentaBancaria] ([ID_Proveedor_CuentaBancaria])
ALTER TABLE [dbo].[Entrada_Vencimiento] ADD CONSTRAINT [FK_Entrada_Vencimiento_Remesa] FOREIGN KEY ([ID_Remesa]) REFERENCES [dbo].[Remesa] ([ID_Remesa])
ALTER TABLE [dbo].[Entrada_Vencimiento] ADD CONSTRAINT [FK_Entrada_Vencimiento_FormaPago] FOREIGN KEY ([ID_FormaPago]) REFERENCES [dbo].[FormaPago] ([ID_FormaPago])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[FormaPago]'
GO
ALTER TABLE [dbo].[FormaPago] ADD CONSTRAINT [FK_FormaPago_Empresa_CuentaBancaria] FOREIGN KEY ([ID_Empresa_CuentaBancaria]) REFERENCES [dbo].[Empresa_CuentaBancaria] ([ID_Empresa_CuentaBancaria])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Remesa]'
GO
ALTER TABLE [dbo].[Remesa] ADD CONSTRAINT [FK_Remesa_Empresa_CuentaBancaria] FOREIGN KEY ([ID_Empresa_CuentaBancaria]) REFERENCES [dbo].[Empresa_CuentaBancaria] ([ID_Empresa_CuentaBancaria])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Empresa_CuentaBancaria]'
GO
ALTER TABLE [dbo].[Empresa_CuentaBancaria] ADD CONSTRAINT [FK_Empresa_CuentaBancaria_Empresa] FOREIGN KEY ([ID_Empresa]) REFERENCES [dbo].[Empresa] ([ID_Empresa])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Entrada]'
GO
ALTER TABLE [dbo].[Entrada] ADD CONSTRAINT [FK_Entrada_FormaPago] FOREIGN KEY ([ID_FormaPago]) REFERENCES [dbo].[FormaPago] ([ID_FormaPago])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Remesa_Archivo]'
GO
ALTER TABLE [dbo].[Remesa_Archivo] ADD CONSTRAINT [FK_Remesa_Archivo_Remesa] FOREIGN KEY ([ID_Remesa_Archivo]) REFERENCES [dbo].[Remesa] ([ID_Remesa])
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
         Configuration = "(H (1[45] 4[15] 2[20] 3) )"
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
         Begin Table = "Personal_Tipo"
            Begin Extent = 
               Top = 6
               Left = 1172
               Bottom = 135
               Right = 1381
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Personal"
            Begin Extent = 
               Top = 6
               Left = 900
               Bottom = 360
               Right = 1134
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RetornaPersonalDiasTrabajados_1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 361
               Right = 862
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
         Column = 2115
         Alias = 900
         Table = 4860
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_LADV_Personal_ResumenHorasPorMes_Aux', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_LADV_Personal_ResumenHorasPorMes_Aux', NULL, NULL
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
         Begin Table = "pepe"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 118
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_LADV_Personal_ResumenHorasPorMes_Aux2', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_LADV_Personal_ResumenHorasPorMes_Aux2', NULL, NULL
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
         Column = 11145
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
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_LADV_Personal_ResumenHorasPorMes', NULL, NULL
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
         Configuration = "(H (1[38] 4[23] 2[20] 3) )"
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
         Begin Table = "Remesa"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 177
               Right = 276
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Empresa_CuentaBancaria"
            Begin Extent = 
               Top = 6
               Left = 314
               Bottom = 193
               Right = 552
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
         Column = 4230
         Alias = 3960
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Remesa', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Remesa', NULL, NULL
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
         Begin Table = "C_Remesa_Vencimientos"
            Begin Extent = 
               Top = 13
               Left = 321
               Bottom = 387
               Right = 667
            End
            DisplayFlags = 280
            TopColumn = 5
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 3360
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2265
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 3555
         Alias = 6285
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Remesa_ExportacionXML', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=1
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Remesa_ExportacionXML', NULL, NULL
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
         Configuration = "(H (1[37] 4[24] 2[17] 3) )"
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
         Begin Table = "Entrada_Vencimiento"
            Begin Extent = 
               Top = 138
               Left = 607
               Bottom = 433
               Right = 864
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Entrada"
            Begin Extent = 
               Top = 18
               Left = 303
               Bottom = 286
               Right = 553
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 0
               Left = 7
               Bottom = 281
               Right = 272
            End
            DisplayFlags = 280
            TopColumn = 9
         End
         Begin Table = "FormaPago"
            Begin Extent = 
               Top = 311
               Left = 1023
               Bottom = 479
               Right = 1261
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Empresa_CuentaBancaria"
            Begin Extent = 
               Top = 6
               Left = 916
               Bottom = 135
               Right = 1154
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Cliente_CuentaBancaria"
            Begin Extent = 
               Top = 154
               Left = 995
               Bottom = 283
               Right = 1248
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "Entrada_Vencimiento_Estado"
            Begin Extent = 
               Top = 348
               Left = 74
               Bottom = 443
          ', 'SCHEMA', N'dbo', 'VIEW', N'C_Remesa_Vencimientos', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_addextendedproperty N'MS_DiagramPane2', N'     Right = 331
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
', 'SCHEMA', N'dbo', 'VIEW', N'C_Remesa_Vencimientos', NULL, NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DECLARE @xp int
SELECT @xp=2
EXEC sp_addextendedproperty N'MS_DiagramPaneCount', @xp, 'SCHEMA', N'dbo', 'VIEW', N'C_Remesa_Vencimientos', NULL, NULL
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
