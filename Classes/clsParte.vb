Public Class clsParte
    'Public oParte As Parte

    'Public Sub New()

    'End Sub

    Public Shared Sub CrearMagatzem(ByRef pDTC As DTCDataContext, ByRef pParte As Parte)
        'Crear almacen
        Dim _NewAlmacen As New Almacen
        With _NewAlmacen
            .Activo = True
            .Parte = pParte
            .Almacen_Tipo = pDTC.Almacen_Tipo.Where(Function(F) F.ID_Almacen_Tipo = EnumAlmacenTipo.Parte).FirstOrDefault
            .Descripcion = "Almacén parte: " & pParte.ID_Parte
            .FechaAlta = Now.Date
        End With
        pDTC.Almacen.InsertOnSubmit(_NewAlmacen)
    End Sub


    Public Shared Function CargarDadesParte(ByVal pIDParte As Integer) As Parte
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            Return oDTC.Parte.Where(Function(F) F.ID_Parte = pIDParte).FirstOrDefault()

            oDTC.Dispose()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Function GuardarParte(ByRef pParte As Parte) As Boolean
        Try
            GuardarParte = False
            Dim oDTC As New DTCDataContext(BD.Conexion)

            oDTC.Parte.Attach(pParte)
            oDTC.SubmitChanges()

            oDTC.Dispose()
            GuardarParte = True
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Sub CambiarEstadoParte(ByVal pIDParte As Integer, ByVal pEstado As EnumParteEstado)
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)
            Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte).FirstOrDefault

            _Parte.Parte_Estado = oDTC.Parte_Estado.Where(Function(F) F.ID_Parte_Estado = CInt(pEstado)).FirstOrDefault

            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Shared Function RetornaHoresRealitzades(ByVal pIDParte As Integer) As Decimal
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)
            Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte).FirstOrDefault

            Return _Parte.RetornaHoresRealizadas

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Sub GuardarTotals(ByVal pIDParte As Integer)
        If pIDParte <> 0 Then
            Dim oDTC As New DTCDataContext(BD.Conexion)
            Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte).FirstOrDefault
            _Parte.HorasRealizadas = _Parte.RetornaHoresRealizadas
            _Parte.CosteImputadoMO = _Parte.RetornaCosteImputadoMO
            _Parte.MargenMO = _Parte.RetornaMargenMO
            _Parte.CosteGastos = _Parte.RetornaGastos
            _Parte.MargenGastos = _Parte.RetornaMargenGastos
            oDTC.SubmitChanges()
        End If
    End Sub



End Class
