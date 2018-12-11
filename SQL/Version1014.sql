/*
Run this script on:

        SERVER2012R2.AbidosMestre    -  This database will be modified

to synchronize it with:

        SERVER2012R2.AbidosDomingo

You are recommended to back up your database before running this script

Script created by SQL Compare version 11.1.0 from Red Gate Software Ltd at 18/06/2016 0:06:06

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
PRINT N'Creating [dbo].[Propuesta_Linea_Software]'
GO
CREATE TABLE [dbo].[Propuesta_Linea_Software]
(
[ID_Propuesta_Linea_Software] [int] NOT NULL IDENTITY(1, 1),
[ID_Propuesta_Linea] [int] NOT NULL,
[ID_Software] [int] NOT NULL,
[NumSerie] [nvarchar] (50) COLLATE Modern_Spanish_CI_AS NULL,
[Usuario] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Contraseña] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Version] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Legal] [bit] NOT NULL CONSTRAINT [DF_Propuesta_Linea_Software_Legal] DEFAULT ((0)),
[CampoAuxiliar1] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAuxiliar2] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAuxiliar3] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAuxiliar4] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAuxiliar5] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAuxiliar6] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAuxiliar7] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAuxiliar8] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[CampoAuxiliar9] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Propuesta_Linea_Software] on [dbo].[Propuesta_Linea_Software]'
GO
ALTER TABLE [dbo].[Propuesta_Linea_Software] ADD CONSTRAINT [PK_Propuesta_Linea_Software] PRIMARY KEY CLUSTERED  ([ID_Propuesta_Linea_Software])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Propuesta_Linea_Informatica]'
GO
CREATE TABLE [dbo].[Propuesta_Linea_Informatica]
(
[ID_Propuesta_Linea_Informatica] [int] NOT NULL IDENTITY(1, 1),
[ID_Propuesta_Linea] [int] NOT NULL,
[ID_TipoDato] [int] NOT NULL,
[Valor] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Valor1] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Valor2] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Valor3] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Valor4] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Valor5] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Valor6] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Valor7] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Valor8] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Valor9] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Usuario] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Contraseña] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL,
[Observaciones] [nvarchar] (2000) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Propuesta_Linea_DatosGenericos] on [dbo].[Propuesta_Linea_Informatica]'
GO
ALTER TABLE [dbo].[Propuesta_Linea_Informatica] ADD CONSTRAINT [PK_Propuesta_Linea_DatosGenericos] PRIMARY KEY CLUSTERED  ([ID_Propuesta_Linea_Informatica])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[TipoDato]'
GO
CREATE TABLE [dbo].[TipoDato]
(
[ID_TipoDato] [int] NOT NULL IDENTITY(1, 1),
[Descripcion] [nvarchar] (100) COLLATE Modern_Spanish_CI_AS NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_TipoDato] on [dbo].[TipoDato]'
GO
ALTER TABLE [dbo].[TipoDato] ADD CONSTRAINT [PK_TipoDato] PRIMARY KEY CLUSTERED  ([ID_TipoDato])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating [dbo].[Software]'
GO
CREATE TABLE [dbo].[Software]
(
[ID_Software] [int] NOT NULL IDENTITY(1, 1),
[Descripcion] [nvarchar] (255) COLLATE Modern_Spanish_CI_AS NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Creating primary key [PK_Software] on [dbo].[Software]'
GO
ALTER TABLE [dbo].[Software] ADD CONSTRAINT [PK_Software] PRIMARY KEY CLUSTERED  ([ID_Software])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_Linea_Informatica]'
GO
ALTER TABLE [dbo].[Propuesta_Linea_Informatica] ADD CONSTRAINT [FK_Propuesta_Linea_Informatica_Propuesta_Linea] FOREIGN KEY ([ID_Propuesta_Linea]) REFERENCES [dbo].[Propuesta_Linea] ([ID_Propuesta_Linea])
ALTER TABLE [dbo].[Propuesta_Linea_Informatica] ADD CONSTRAINT [FK_Propuesta_Linea_Informatica_TipoDato] FOREIGN KEY ([ID_TipoDato]) REFERENCES [dbo].[TipoDato] ([ID_TipoDato])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
PRINT N'Adding foreign keys to [dbo].[Propuesta_Linea_Software]'
GO
ALTER TABLE [dbo].[Propuesta_Linea_Software] ADD CONSTRAINT [FK_Propuesta_Linea_Software_Propuesta_Linea] FOREIGN KEY ([ID_Propuesta_Linea]) REFERENCES [dbo].[Propuesta_Linea] ([ID_Propuesta_Linea])
ALTER TABLE [dbo].[Propuesta_Linea_Software] ADD CONSTRAINT [FK_Propuesta_Linea_Software_Software] FOREIGN KEY ([ID_Software]) REFERENCES [dbo].[Software] ([ID_Software])
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