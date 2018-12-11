/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 30/09/2014 12:15:40

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
PRINT N'Creating [dbo].[Monitor_Colas]'
GO
CREATE TABLE [dbo].[Monitor_Colas]
(
[Procedimiento] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Fecha] [datetime] NOT NULL CONSTRAINT [DF__monitor_b__fecha__64054DAC] DEFAULT (getdate())
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
Create Queue cola_ActualizarStocks
GO
PRINT N'Creating services'
GO
CREATE SERVICE [Service_InicioCola_ActualizarStocks]
AUTHORIZATION [dbo]
ON QUEUE [dbo].[cola_ActualizarStocks]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
CREATE SERVICE [Service_ContinuacionCola_ActualizarStocks]
AUTHORIZATION [dbo]
ON QUEUE [dbo].[cola_ActualizarStocks]
(
[DEFAULT]
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[prActualizarStocks]'
GO
Create procedure [dbo].[prActualizarStocks]  as  
declare @conversationhandle uniqueidentifier      
declare @message_type_name sysname     
declare @dialog uniqueidentifier       
waitfor (receive top(1)  @message_type_name = message_type_name,  @dialog = conversation_handle from cola_ActualizarStocks ), timeout 500  if (@message_type_name ='http://schemas.microsoft.com/sql/servicebroker/dialogtimer')    
 begin            
  -- segundos en que se lanzará el siguiente procedure    
  begin conversation timer (@dialog) timeout = 10;                 
    
 begin try            
    --backup database AbidosDomingo to disk = 'c:\temp\accountsdb.bak'  with init, format             
	Delete From TempStockRealPorProducto
	Insert into TempStockRealPorProducto  Select ID_Producto, Sum(StockReal) as StockReal From RetornaStock(0,0) Group by ID_Producto
	Delete From TempStockRealPorProductoYPorAlmacen
	Insert into TempStockRealPorProductoYPorAlmacen  Select ID_Producto, ID_Almacen, Sum(StockReal) as StockReal From RetornaStock(0,0) Group by ID_Producto, ID_Almacen
   -- registro de seguimiento             
	 insert into Monitor_Colas (Procedimiento) select '[prActualizarStocks]'         
 end try       
    begin catch                 
	 declare @s sysname = ERROR_MESSAGE()                
	  raiserror ( @s, 10, 1) with log;                     
end catch    
end 
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating queues'
GO
ALTER QUEUE [dbo].[cola_ActualizarStocks] 
WITH STATUS=ON, 
RETENTION=OFF, 
ACTIVATION (
STATUS=ON, 
PROCEDURE_NAME=[dbo].[prActualizarStocks], 
MAX_QUEUE_READERS=1, 
EXECUTE AS N'dbo'
), POISON_MESSAGE_HANDLING (STATUS = ON) 
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
declare @conversationhandle uniqueidentifier  
begin dialog conversation @conversationhandle   from service Service_InicioCola_ActualizarStocks   to service 'Service_ContinuacionCola_ActualizarStocks';
begin conversation timer (@conversationhandle)    timeout = 10;
go
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
