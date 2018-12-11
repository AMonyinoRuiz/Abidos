Public Class clsAvisos
    Dim oDTC As DTCDataContext
    Dim oLinqAviso As Aviso


    Enum EnumTipoAviso
        Manual = 1
        AlAsignarUnDestinatarioAUnaActividad = 2
        AlEliminarUnDestinatarioDeUnaActividad = 3
        AlAsignarUnDestinatarioAUnaAccion = 4
        AlEliminarUnDestinatarioDeUnaAccion = 5
        AlCrearUnPedidoVenta = 4
        AlCrearUnParte = 5
        AlAsignarUnMaterialAUnAlmacenDesdeImputacionHoras = 6
        AlAñadirOModificarUnInformeTecnicoDesdeImputacionHoras = 7
        AlCrearUnCliente = 8
        AlMarcarUnProductoComoObsoleto = 9
        AlCrearUnPresupuesto = 10
        AlModificarOAñadirUnaLineaDeTalYComoSeInstalo = 11
        AlAñadirUnToDoDesdeLaPantallaImputacionHoras = 12
        AlAñadirUnToDoDesdeLaPantallaParte = 13
    End Enum

    Public Sub New(ByRef pDTC As DTCDataContext)
        oDTC = pDTC
    End Sub

    Private Sub CrearAviso(ByRef pActividad As ActividadCRM, ByVal pTipoAviso As EnumTipoAviso, ByVal pMensaje As String, Optional ByRef pPersonalDestino As Personal = Nothing, Optional ByRef pAccion As ActividadCRM_Accion = Nothing)
        Try
            ' Dim _Rel As ActividadCRM_Personal

            'Dim _GentQueTeAquestaNotificacioActivada As IEnumerable(Of Notificacion_Automatica_Usuario) = oDTC.Notificacion_Automatica_Usuario.Where(Function(F) F.ID_Notificacion_Automatica_Tipo = pTipoNotificacion And F.ID_Usuario <> Seguretat.oUser.ID_Usuario)

            ' For Each _Rel In pActividad.ActividadCRM_Personal
            Dim _NouAvis As New Aviso
            With _NouAvis
                .ActividadCRM = pActividad
                .ActividadCRM_Accion = pAccion
                '.Aviso_AutomatismoTipo = oDTC.Aviso_AutomatismoTipo.Where(Function(F) F.ID_Aviso_AutomatismoTipo = CInt(pTipoAviso)).FirstOrDefault
                .FechaAviso = Date.Now
                .Asunto = pMensaje
                .Leido = False
                .Personal_Destino = pPersonalDestino
                .Personal_Origen = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
                .Prioridad = pActividad.Prioridad

                oDTC.Aviso.InsertOnSubmit(_NouAvis)
            End With
            '  Next
            '  oLinqAviso = New ActividadCRM_Aviso
            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub


    Private Sub CrearActividad(ByRef pActividad As ActividadCRM, ByVal pTipoAviso As EnumTipoAviso, ByVal pMensaje As String)
        Try
            ' Dim _Rel As ActividadCRM_Personal

            'Dim _GentQueTeAquestaNotificacioActivada As IEnumerable(Of Notificacion_Automatica_Usuario) = oDTC.Notificacion_Automatica_Usuario.Where(Function(F) F.ID_Notificacion_Automatica_Tipo = pTipoNotificacion And F.ID_Usuario <> Seguretat.oUser.ID_Usuario)

            ' For Each _Rel In pActividad.ActividadCRM_Personal
            Dim _NuevaActividad As New ActividadCRM
            With _NuevaActividad

                .Activo = True
                .Asunto = pMensaje
                .FechaAlta = Date.Now
                '.FechaAviso = Date.Now
                '.HoraAviso = Date.Now
                .PorcentajeFinalizada = 0
                .Finalizada = False
                oDTC.ActividadCRM.InsertOnSubmit(_NuevaActividad)
            End With
            '  Next
            '  oLinqAviso = New ActividadCRM_Aviso
            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub


    'Private Sub CrearActividad()
    '    Try
    '        Dim _NuevaActividad As New ActividadCRM
    '        With _NuevaActividad
    '            .
    '            .ActividadCRM = pActividad
    '            .ActividadCRM_Accion = Nothing
    '            .ActividadCRM_Aviso_Automatismo_Tipo = oDTC.ActividadCRM_Aviso_Automatismo_Tipo.Where(Function(F) F.ID_ActividadCRM_Aviso_Automatismo_Tipo = CInt(pTipoAviso)).FirstOrDefault
    '            .FechaAviso = Date.Now
    '            .Leido = False
    '            .Personal = _Rel.Personal
    '            oDTC.ActividadCRM_Aviso.InsertOnSubmit(_NouAvis)
    '        End With
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Public Sub AlAsignarUnPersonalAUnaActividad(ByRef pActividad As ActividadCRM, ByRef pPersonalDestino As Personal)
        Try
            Dim _Mensaje As String = ""
            _Mensaje = "El usuario: " & NomPersonalUsuariActual() & " te ha asignado a la actividad: " & pActividad.ID_ActividadCRM

            Call CrearAviso(pActividad, EnumTipoAviso.AlAsignarUnDestinatarioAUnaActividad, _Mensaje, pPersonalDestino)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub AlEliminarUnPersonalDeUnaActividad(ByRef pActividad As ActividadCRM, ByRef pPersonalDestino As Personal)
        Try
            Dim _Mensaje As String = ""
            _Mensaje = "El usuario: " & NomPersonalUsuariActual() & " te ha desasignado de la actividad: " & pActividad.ID_ActividadCRM

            Call CrearAviso(pActividad, EnumTipoAviso.AlEliminarUnDestinatarioDeUnaActividad, _Mensaje, pPersonalDestino)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub AlAsignarUnPersonalAUnaAccion(ByRef pAccion As ActividadCRM_Accion, ByRef pPersonalDestino As Personal)
        Try
            Dim _Mensaje As String = ""
            _Mensaje = "El usuario: " & NomPersonalUsuariActual() & " te ha asignado a la acción: " & pAccion.Descripcion & " de la actividad: " & pAccion.ActividadCRM.ID_ActividadCRM

            Call CrearAviso(pAccion.ActividadCRM, EnumTipoAviso.AlAsignarUnDestinatarioAUnaActividad, _Mensaje, pPersonalDestino, pAccion)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub AlEliminarUnPersonalDeUnaAccion(ByRef pAccion As ActividadCRM_Accion, ByRef pPersonalDestino As Personal)
        Try
            Dim _Mensaje As String = ""
            _Mensaje = "El usuario: " & NomPersonalUsuariActual() & " te ha desasignado a la acción: " & pAccion.Descripcion & " de la actividad: " & pAccion.ActividadCRM.ID_ActividadCRM

            Call CrearAviso(pAccion.ActividadCRM, EnumTipoAviso.AlEliminarUnDestinatarioDeUnaAccion, _Mensaje, pPersonalDestino, pAccion)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function NomPersonalUsuariActual() As String
        Return oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Nombre
    End Function

End Class
