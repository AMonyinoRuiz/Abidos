/*
Run this script on:

        (local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

        (local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Compare version 10.4.8 from Red Gate Software Ltd at 30/04/2014 15:55:51

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
PRINT N'Dropping foreign keys from [dbo].[Producto]'
GO
ALTER TABLE [dbo].[Producto] DROP CONSTRAINT [FK_Producto_Producto_SubFamilia]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping foreign keys from [dbo].[Producto_SubFamilia]'
GO
ALTER TABLE [dbo].[Producto_SubFamilia] DROP CONSTRAINT [FK_Producto_SubFamilia_Producto_Familia]
ALTER TABLE [dbo].[Producto_SubFamilia] DROP CONSTRAINT [FK_Producto_SubFamilia_Producto_SubFamilia_Traspaso]
ALTER TABLE [dbo].[Producto_SubFamilia] DROP CONSTRAINT [FK_Producto_SubFamilia_Producto_Subfamilia_Tipo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Producto_SubFamilia]'
GO
ALTER TABLE [dbo].[Producto_SubFamilia] DROP CONSTRAINT [PK_Producto_SubFamilia]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Producto_SubFamilia]'
GO
ALTER TABLE [dbo].[Producto_SubFamilia] DROP CONSTRAINT [DF_Producto_SubFamilia_ID_Producto_Familia_Traspaso]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Producto_SubFamilia]'
GO
ALTER TABLE [dbo].[Producto_SubFamilia] DROP CONSTRAINT [DF_Producto_SubFamilia_ID_Producto_Subfamilia_Tipo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Dropping constraints from [dbo].[Producto_SubFamilia]'
GO
ALTER TABLE [dbo].[Producto_SubFamilia] DROP CONSTRAINT [DF_Producto_SubFamilia_Activo]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Entrada]'
GO
ALTER TABLE [dbo].[Entrada] ADD
[Anulado] [bit] NOT NULL CONSTRAINT [DF_Entrada_Anulado] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Propuesta]'
GO
ALTER TABLE [dbo].[Propuesta] ADD
[HorasPrevistas] [decimal] (10, 2) NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Rebuilding [dbo].[Producto_SubFamilia]'
GO
CREATE TABLE [dbo].[tmp_rg_xx_Producto_SubFamilia]
(
[ID_Producto_SubFamilia] [int] NOT NULL IDENTITY(1, 1),
[ID_Producto_Familia] [int] NOT NULL,
[ID_Producto_SubFamilia_Traspaso] [int] NOT NULL CONSTRAINT [DF_Producto_SubFamilia_ID_Producto_Familia_Traspaso] DEFAULT ((0)),
[ID_Producto_Subfamilia_Tipo] [int] NOT NULL CONSTRAINT [DF_Producto_SubFamilia_ID_Producto_Subfamilia_Tipo] DEFAULT ((1)),
[Codigo] [nvarchar] (10) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Descripcion] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL,
[Activo] [bit] NOT NULL CONSTRAINT [DF_Producto_SubFamilia_Activo] DEFAULT ((1))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Producto_SubFamilia] ON
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
INSERT INTO [dbo].[tmp_rg_xx_Producto_SubFamilia]([ID_Producto_SubFamilia], [ID_Producto_Familia], [ID_Producto_SubFamilia_Traspaso], [ID_Producto_Subfamilia_Tipo], [Codigo], [Descripcion], [Activo]) SELECT [ID_Producto_SubFamilia], [ID_Producto_Familia], [ID_Producto_SubFamilia_Traspaso], [ID_Producto_Subfamilia_Tipo], [Codigo], [Descripcion], [Activo] FROM [dbo].[Producto_SubFamilia]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
SET IDENTITY_INSERT [dbo].[tmp_rg_xx_Producto_SubFamilia] OFF
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
DROP TABLE [dbo].[Producto_SubFamilia]
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
EXEC sp_rename N'[dbo].[tmp_rg_xx_Producto_SubFamilia]', N'Producto_SubFamilia'
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Producto_SubFamilia] on [dbo].[Producto_SubFamilia]'
GO
ALTER TABLE [dbo].[Producto_SubFamilia] ADD CONSTRAINT [PK_Producto_SubFamilia] PRIMARY KEY CLUSTERED  ([ID_Producto_SubFamilia])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[Producto_SubFamilia_ChangeTracking] on [dbo].[Producto_SubFamilia]'
GO
create trigger [dbo].[Producto_SubFamilia_ChangeTracking] on [dbo].[Producto_SubFamilia] for insert, update, delete
as

declare @bit int ,
@field int ,
@maxfield int ,
@char int ,
@fieldname varchar(128) ,
@TableName varchar(128) ,
@NombreCampoIDPadre varchar(128) ,	
@PKCols varchar(1000) ,
@sql varchar(2000), 
@UpdateDate varchar(21) ,
@UserName varchar(128) ,
@Type char(1) ,
@PKSELECT varchar(1000)

select @TableName = 'Producto_SubFamilia'
SELECT @NombreCampoIDPadre = 'i.' + '' --<-- poner el nombre del campo relacional del Padre

-- date and user
select @UserName = system_user ,
@UpdateDate = convert(varchar(8), getdate(), 112) + ' ' + convert(varchar(12), getdate(), 114)

-- Action
if exists (select * from inserted)
if exists (select * from deleted)
select @Type = 'U'
else
select @Type = 'I'
else
select @Type = 'D'

-- get list of columns
select * into #ins from inserted
select * into #del from deleted

-- Get primary key columns for full outer join
select @PKCols = coalesce(@PKCols + ' and', ' on') + ' i.' + c.COLUMN_NAME + ' = d.' + c.COLUMN_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

-- Get primary key fields select for insert
select @PKSelect = coalesce(@PKSelect+'+','') + 'convert(varchar(100), coalesce(i.' + COLUMN_NAME + ',d.' + COLUMN_NAME + '))'
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS pk ,
INFORMATION_SCHEMA.KEY_COLUMN_USAGE c
where pk.TABLE_NAME = @TableName
and CONSTRAINT_TYPE = 'PRIMARY KEY'
and c.TABLE_NAME = pk.TABLE_NAME
and c.CONSTRAINT_NAME = pk.CONSTRAINT_NAME

if @PKCols is null
begin
raiserror('no PK on table %s', 16, -1, @TableName)
return
end

select @field = 0, @maxfield = max(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName
while @field < @maxfield
begin
select @field = min(ORDINAL_POSITION) from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION > @field
select @bit = (@field - 1 )% 8 + 1
select @bit = power(2,@bit - 1)
select @char = ((@field - 1) / 8) + 1
if substring(COLUMNS_UPDATED(),@char, 1) & @bit > 0 or @Type in ('I','D')
begin
select @fieldname = COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName and ORDINAL_POSITION = @field
select @sql = 'insert logTransacciones (TipoTrn, Tabla, PK, PKPadre, Campo, ValorOriginal, ValorNuevo, FechaTrn, Usuario)'
select @sql = @sql + ' select ''' + @Type + ''''
select @sql = @sql + ',''' + @TableName + ''''
select @sql = @sql + ',' + @PKSelect
if @NombreCampoIDPadre='i.'
begin
	SELECT @sql = @sql + ', null'
end
else
SELECT @sql = @sql + ',''' + @NombreCampoIDPadre;
select @sql = @sql + ',''' + @fieldname + ''''
select @sql = @sql + ',convert(varchar(Max),d.' + @fieldname + ')'
select @sql = @sql + ',convert(varchar(Max),i.' + @fieldname + ')'
select @sql = @sql + ',''' + @UpdateDate + ''''
select @sql = @sql + ',''' + @UserName + ''''
select @sql = @sql + ' from #ins i full outer join #del d'
select @sql = @sql + @PKCols
select @sql = @sql + ' where i.' + @fieldname + ' <> d.' + @fieldname 
select @sql = @sql + ' or (i.' + @fieldname + ' is null and  d.' + @fieldname + ' is not null)' 
select @sql = @sql + ' or (i.' + @fieldname + ' is not null and  d.' + @fieldname + ' is null)' 
Print(@SQL)

exec (@sql)
end
end
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Producto]'
GO
ALTER TABLE [dbo].[Producto] ADD CONSTRAINT [FK_Producto_Producto_SubFamilia] FOREIGN KEY ([ID_Producto_SubFamilia]) REFERENCES [dbo].[Producto_SubFamilia] ([ID_Producto_SubFamilia])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Producto_SubFamilia]'
GO
ALTER TABLE [dbo].[Producto_SubFamilia] ADD CONSTRAINT [FK_Producto_SubFamilia_Producto_Familia] FOREIGN KEY ([ID_Producto_Familia]) REFERENCES [dbo].[Producto_Familia] ([ID_Producto_Familia])
ALTER TABLE [dbo].[Producto_SubFamilia] ADD CONSTRAINT [FK_Producto_SubFamilia_Producto_SubFamilia_Traspaso] FOREIGN KEY ([ID_Producto_SubFamilia_Traspaso]) REFERENCES [dbo].[Producto_SubFamilia_Traspaso] ([ID_Producto_SubFamilia_Traspaso])
ALTER TABLE [dbo].[Producto_SubFamilia] ADD CONSTRAINT [FK_Producto_SubFamilia_Producto_Subfamilia_Tipo] FOREIGN KEY ([ID_Producto_Subfamilia_Tipo]) REFERENCES [dbo].[Producto_Subfamilia_Tipo] ([ID_Producto_Subfamilia_Tipo])
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
