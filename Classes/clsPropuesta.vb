Public Class clsPropuesta


    Public Shared Function PasarLaPropuestaANegativaoPositiva(ByRef pDTC As DTCDataContext, ByVal pIDPropuesta As Integer, Optional ByVal pCridaDesdePantallaPedidoVenta As Boolean = False) As Boolean
        Try

            PasarLaPropuestaANegativaoPositiva = False

            Dim _Propuesta As Propuesta = pDTC.Propuesta.Where(Function(F) F.ID_Propuesta = pIDPropuesta).FirstOrDefault

            If pCridaDesdePantallaPedidoVenta = False Then
                If _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Traspasado Then
                    Mensaje.Mostrar_Mensaje("Imposible cambiar el estado de una propuesta en estado 'Traspasada'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If

                If _Propuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then 'Les instalacions anteriors si que es poden traspasar sense estar aprobades
                    Mensaje.Mostrar_Mensaje("Este tipo de propuesta no se puede dar por negativa", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
            End If

            Select Case DirectCast(_Propuesta.ID_Propuesta_Estado, EnumPropuestaEstado)
                Case EnumPropuestaEstado.Pendiente
                    If Mensaje.Mostrar_Mensaje("¿Desea dar por negativa la propuesta?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                        _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Negativo
                        Call CambiarEstadoPropuestaCRM(pDTC, _Propuesta, EnumPropuestaEstado.Negativo)
                    End If

                Case EnumPropuestaEstado.Negativo
                    If Mensaje.Mostrar_Mensaje("¿Desea quitar el estado 'negativa' a la propuesta?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                        _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Pendiente
                        Call CambiarEstadoPropuestaCRM(pDTC, _Propuesta, EnumPropuestaEstado.Pendiente)
                    End If

                Case EnumPropuestaEstado.Aprobado
                    'Això es només quan pCridaDesdePantallaPedidoVenta = a true  <--- NO APLICA

                    _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Negativo
                    Call CambiarEstadoPropuestaCRM(pDTC, _Propuesta, EnumPropuestaEstado.Negativo)
            End Select

            pDTC.SubmitChanges()
            PasarLaPropuestaANegativaoPositiva = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Sub CambiarEstadoPropuestaCRM(ByRef pDTC As DTCDataContext, ByRef pPropuesta As Propuesta, ByVal pIDDestinoEstadoPropuesta As EnumPropuestaEstado)

        Dim _EstatCRM As Propuesta_EstadoCRM = pDTC.Propuesta_EstadoCRM.Where(Function(F) F.ID_Propuesta_Estado = pIDDestinoEstadoPropuesta).FirstOrDefault()
        If _EstatCRM Is Nothing = False Then 'Només si hem trobat un EstatCRM equivalment al ESTAT de la proposta cambiada llavors cambiarem l'estat CRM
            pPropuesta.Propuesta_EstadoCRM = _EstatCRM
        End If

    End Sub

    Public Shared Sub AsignarEspecificaciones(ByRef pDTC As DTCDataContext, ByRef pPropuesta As Propuesta, ByRef pPropuestaLinea As Propuesta_Linea)
        Try
            Dim _Especificacion As PropuestaEspecificacion
            If pPropuesta.Propuesta_PropuestaEspecificacion.Count = 0 Then
                'si encara no te cap especificació lo primer que farem es agregar totes les espeficacions que no tinguin ni divisio ni familia

                For Each _Especificacion In pDTC.PropuestaEspecificacion.Where(Function(F) F.ID_Producto_Division.HasValue = False AndAlso F.ID_Producto_Familia.HasValue = False)
                    Dim _NewEspecificacion As New Propuesta_PropuestaEspecificacion
                    _NewEspecificacion.PropuestaEspecificacion = _Especificacion
                    pPropuesta.Propuesta_PropuestaEspecificacion.Add(_NewEspecificacion)
                Next

            End If

            Dim _Linea As Propuesta_Linea = pPropuestaLinea

            For Each _Especificacion In pDTC.PropuestaEspecificacion.Where(Function(F) F.ID_Producto_Division = _Linea.Producto.ID_Producto_Division And (F.ID_Producto_Familia = _Linea.Producto.ID_Producto_Familia OrElse F.ID_Producto_Familia.HasValue = False))
                If pPropuesta.Propuesta_PropuestaEspecificacion.Where(Function(F) F.ID_PropuestaEspecificacion = _Especificacion.ID_PropuestaEspecificacion).Count = 0 Then
                    Dim _NewEspecificacion As New Propuesta_PropuestaEspecificacion
                    _NewEspecificacion.PropuestaEspecificacion = _Especificacion
                    pPropuesta.Propuesta_PropuestaEspecificacion.Add(_NewEspecificacion)
                End If
            Next


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

End Class
