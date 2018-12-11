/*
Run this script on:

(local)\SQLEXPRESS.AbidosMestreVersioAnterior    -  This database will be modified

to synchronize it with:

(local)\SQLEXPRESS.AbidosMestre

You are recommended to back up your database before running this script

Script created by SQL Data Compare version 10.4.8 from Red Gate Software Ltd at 21/11/2014 11:50:07

*/
		
GO
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)

PRINT(N'Drop constraint FK_FormaPago_FormaPago_Tipo from [dbo].[FormaPago]')
GO
ALTER TABLE [dbo].[FormaPago] DROP CONSTRAINT [FK_FormaPago_FormaPago_Tipo]

PRINT(N'Drop constraint FK_Entrada_Vencimiento_Entrada_Vencimiento_Estado from [dbo].[Entrada_Vencimiento]')
GO
ALTER TABLE [dbo].[Entrada_Vencimiento] DROP CONSTRAINT [FK_Entrada_Vencimiento_Entrada_Vencimiento_Estado]

PRINT(N'Add rows to [dbo].[Entrada_Vencimiento_Estado]')
GO
SET IDENTITY_INSERT [dbo].[Entrada_Vencimiento_Estado] ON
INSERT INTO [dbo].[Entrada_Vencimiento_Estado] ([ID_Entrada_Vencimiento_Estado], [Descripcion]) VALUES (1, N'Pendiente de asignar a una remesa')
INSERT INTO [dbo].[Entrada_Vencimiento_Estado] ([ID_Entrada_Vencimiento_Estado], [Descripcion]) VALUES (2, N'Pendiente de generar el archivo')
INSERT INTO [dbo].[Entrada_Vencimiento_Estado] ([ID_Entrada_Vencimiento_Estado], [Descripcion]) VALUES (3, N'Recibo exportado')
INSERT INTO [dbo].[Entrada_Vencimiento_Estado] ([ID_Entrada_Vencimiento_Estado], [Descripcion]) VALUES (4, N'Recibo enviado al banco')
INSERT INTO [dbo].[Entrada_Vencimiento_Estado] ([ID_Entrada_Vencimiento_Estado], [Descripcion]) VALUES (5, N'Cobrado')
INSERT INTO [dbo].[Entrada_Vencimiento_Estado] ([ID_Entrada_Vencimiento_Estado], [Descripcion]) VALUES (6, N'Devuelto')
SET IDENTITY_INSERT [dbo].[Entrada_Vencimiento_Estado] OFF
PRINT(N'Operation applied to 6 rows out of 6')
GO

PRINT(N'Add row to [dbo].[FormaPago_Tipo]')
GO
INSERT INTO [dbo].[FormaPago_Tipo] ([ID_FormaPago_Tipo], [Codigo], [Descripcion], [Activo], [GenerarDomiciliacion]) VALUES (2, N'02', N'Domiciliado', 1, 1)

PRINT(N'Add constraint FK_FormaPago_FormaPago_Tipo to [dbo].[FormaPago]')
GO
ALTER TABLE [dbo].[FormaPago] WITH NOCHECK ADD CONSTRAINT [FK_FormaPago_FormaPago_Tipo] FOREIGN KEY ([ID_FormaPago_Tipo]) REFERENCES [dbo].[FormaPago_Tipo] ([ID_FormaPago_Tipo])

PRINT(N'Add constraint FK_Entrada_Vencimiento_Entrada_Vencimiento_Estado to [dbo].[Entrada_Vencimiento]')
GO
ALTER TABLE [dbo].[Entrada_Vencimiento] WITH NOCHECK ADD CONSTRAINT [FK_Entrada_Vencimiento_Entrada_Vencimiento_Estado] FOREIGN KEY ([ID_Entrada_Vencimiento_Estado]) REFERENCES [dbo].[Entrada_Vencimiento_Estado] ([ID_Entrada_Vencimiento_Estado])
COMMIT TRANSACTION
GO
