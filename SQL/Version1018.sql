/*
Run this script on:

        SERVER2012R2\SQLSERVER2012.AbidosDomingo    -  This database will be modified

to synchronize it with:

        SERVER2012R2\SQLSERVER2012.AbidosDomingoReal

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.0 from Red Gate Software Ltd at 17/10/2016 14:25:58

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
PRINT N'Creating trigger [dbo].[ActividadCRM_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM]'
GO
Create trigger [dbo].[ActividadCRM_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM] for insert, update, delete
as
Declare @sql as varchar(128)
Select @sql = 'Insert ActividadCRM_RegistroCambios (FechaCambio) Values (Getdate())'
exec (@sql)


GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating trigger [dbo].[ActividadCRM_Accion_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Accion]'
GO
Create trigger [dbo].[ActividadCRM_Accion_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Accion] for insert, update, delete
as
Declare @sql as varchar(128)
Select @sql = 'Insert ActividadCRM_RegistroCambios (FechaCambio) Values (Getdate())'
exec (@sql)


GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating trigger [dbo].[ActividadCRM_Personal_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Personal]'
GO
Create trigger [dbo].[ActividadCRM_Personal_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Personal] for insert, update, delete
as
Declare @sql as varchar(128)
Select @sql = 'Insert ActividadCRM_RegistroCambios (FechaCambio) Values (Getdate())'
exec (@sql)


GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating trigger [dbo].[ActividadCRM_Accion_Personal_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Accion_Personal]'
GO
Create trigger [dbo].[ActividadCRM_Accion_Personal_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Accion_Personal] for insert, update, delete
as
Declare @sql as varchar(128)
Select @sql = 'Insert ActividadCRM_RegistroCambios (FechaCambio) Values (Getdate())'
exec (@sql)


GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating trigger [dbo].[ActividadCRM_Aux_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Aux]'
GO
Create trigger [dbo].[ActividadCRM_Aux_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Aux] for insert, update, delete
as
Declare @sql as varchar(128)
Select @sql = 'Insert ActividadCRM_RegistroCambios (FechaCambio) Values (Getdate())'
exec (@sql)


GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating trigger [dbo].[ActividadCRM_Chat_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Chat]'
GO
Create trigger [dbo].[ActividadCRM_Chat_ChangeTrackingPerActualitzarLesActividades] on [dbo].[ActividadCRM_Chat] for insert, update, delete
as
Declare @sql as varchar(128)
Select @sql = 'Insert ActividadCRM_RegistroCambios (FechaCambio) Values (Getdate())'
exec (@sql)


GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[ActividadCRM_RegistroCambios]'
GO
CREATE TABLE [dbo].[ActividadCRM_RegistroCambios]
(
[ID_ActividadCRM_RegistroCambios] [int] NOT NULL IDENTITY(1, 1),
[FechaCambio] [smalldatetime] NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_ActividadCRM_RegistroCambios] on [dbo].[ActividadCRM_RegistroCambios]'
GO
ALTER TABLE [dbo].[ActividadCRM_RegistroCambios] ADD CONSTRAINT [PK_ActividadCRM_RegistroCambios] PRIMARY KEY CLUSTERED  ([ID_ActividadCRM_RegistroCambios])
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