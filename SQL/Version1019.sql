/*
Run this script on:

        SERVER2012R2\SQLSERVER2012.AbidosDomingo    -  This database will be modified

to synchronize it with:

        SERVER2012R2\SQLSERVER2012.AbidosDomingoReal

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.0 from Red Gate Software Ltd at 21/10/2016 17:35:52

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
PRINT N'Altering [dbo].[TempStockRealPorProducto]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[TempStockRealPorProducto] ADD
[StockTeorico] [decimal] (12, 2) NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[TempStockRealPorProductoYPorAlmacen]'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[TempStockRealPorProductoYPorAlmacen] ADD
[StockTeorico] [decimal] (12, 2) NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Altering [dbo].[prActualizarStocks]'
GO
ALTER procedure [dbo].[prActualizarStocks]  as  
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
	Insert into TempStockRealPorProducto  Select ID_Producto, Sum(StockReal) as StockReal, Sum(StockTeorico) as StockTeorico From RetornaStock(0,0) Group by ID_Producto
	Delete From TempStockRealPorProductoYPorAlmacen
	Insert into TempStockRealPorProductoYPorAlmacen  Select ID_Producto, ID_Almacen, Sum(StockReal) as StockReal, Sum(StockTeorico) as StockTeorico From RetornaStock(0,0) Group by ID_Producto, ID_Almacen
   -- registro de seguimiento             
	 insert into Monitor_Colas (Procedimiento) select '[prActualizarStocks]'         
 end try       
    begin catch                 
	 declare @s sysname = ERROR_MESSAGE()                
	  raiserror ( @s, 10, 1) with log;                     
end catch    
end 
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