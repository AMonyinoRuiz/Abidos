/*
Run this script on:

192.168.1.225.AbidosMestre    -  This database will be modified

to synchronize it with:

192.168.1.225.AbidosDomingo

You are recommended to back up your database before running this script

Script created by SQL Data Compare version 11.1.0 from Red Gate Software Ltd at 10/02/2016 2:21:54

*/
		
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

PRINT(N'Add 3 rows to [dbo].[Cliente_DireccionTipo]')
SET IDENTITY_INSERT [dbo].[Cliente_DireccionTipo] ON
INSERT INTO [dbo].[Cliente_DireccionTipo] ([ID_Cliente_DireccionTipo], [Descripcion]) VALUES (1, N'Principal')
INSERT INTO [dbo].[Cliente_DireccionTipo] ([ID_Cliente_DireccionTipo], [Descripcion]) VALUES (2, N'Oficinas')
INSERT INTO [dbo].[Cliente_DireccionTipo] ([ID_Cliente_DireccionTipo], [Descripcion]) VALUES (3, N'Almacen')
SET IDENTITY_INSERT [dbo].[Cliente_DireccionTipo] OFF
COMMIT TRANSACTION
GO