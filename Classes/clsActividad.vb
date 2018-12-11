Public Class clsActividad
    Dim oLinqActividad As ActividadCRM
    Dim oDTC As DTCDataContext
    Public oEsPropietari As Boolean = False
    Public oIDPersonalActual As Integer

    Public Sub New(ByRef pDTC As DTCDataContext, ByVal pActividad As ActividadCRM, ByVal pIDPersonalActual As Integer)
        oDTC = pDTC
        oLinqActividad = pActividad
        oIDPersonalActual = pIDPersonalActual
        If oLinqActividad.ID_Personal = pIDPersonalActual Then
            oEsPropietari = True
        Else
            oEsPropietari = False
        End If
    End Sub


    Public Function FinalizarActividad() As Boolean
        FinalizarActividad = False

        If oLinqActividad.ID_ActividadCRM = 0 Then
            Exit Function
        End If

        If oEsPropietari = True Then 'si ets el propietari de l'activitat finalitzarem tota la activitat
            oLinqActividad.Finalizada = True
            oLinqActividad.PorcentajeFinalizada = 100
        Else

            If oLinqActividad.ActividadCRM_Accion.Where(Function(F) F.ID_Personal = oIDPersonalActual And F.Finalizada = False).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible dar por finalizado mientras hayan acciones propias pendientes de finalizar", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Return False
            End If

            Dim _Destinatari As ActividadCRM_Personal = oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
            If _Destinatari Is Nothing Then
                Exit Function
            End If

            _Destinatari.Finalizado = True

            ' 
            ' Dim _PercentatgeFinalitzat As Integer = Math.Round(1 / oLinqActividad.ActividadCRM_Personal.Count, 2) * 100
            oLinqActividad.PorcentajeFinalizada = CalcularPercentatgeFinalitzat()

            'Enviem email
            Dim _PersonalActual As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
            Dim _Missatge As String = "El usuario:<b> " & _PersonalActual.Nombre & "</b> ha dado por finalizada parcialmente la actividad nº <b>" & oLinqActividad.ID_ActividadCRM & "</b>con asunto:<b> " & oLinqActividad.Asunto
            Dim _Asunto As String = "Finalización parcial de la actividad"
            clsActividad.EnviarCorreuAlaBustia(oLinqActividad.Personal.Personal_Emails.FirstOrDefault.Email, "abidos@westpoint.es", _Asunto, _Missatge, Seguretat.oUser.ID_Personal, oLinqActividad.ID_Personal, oLinqActividad.ID_ActividadCRM)


        End If

        oDTC.SubmitChanges()

 

        Return True

    End Function

    Public Function DesFinalizarActividad(Optional ByVal pSocPropietariIVullDesfinalitzarLaAccioDalguAltre_IDActividadCRM_Personal As Integer = 0) As Boolean
        DesFinalizarActividad = False

        If oLinqActividad.ID_ActividadCRM = 0 Then
            Exit Function
        End If

        If oEsPropietari = True And pSocPropietariIVullDesfinalitzarLaAccioDalguAltre_IDActividadCRM_Personal = 0 Then 'si ets el propietari de l'activitat des finalitzarem tota la activitat a menys que m'hagin passat un Numéro de activitat personal que voldrà dir que estic finalitzat una acció d'un altre
            oLinqActividad.Finalizada = False
            oLinqActividad.PorcentajeFinalizada = CalcularPercentatgeFinalitzat()
        Else

            'If oLinqActividad.ActividadCRM_Accion.Where(Function(F) F.ID_Personal = oIDPersonalActual And F.Finalizada = False).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible dar por finalizado mientras hayan acciones propias pendientes de finalizar", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Return False
            'End If

            Dim _Destinatari As ActividadCRM_Personal
            'aquest if es una xapuceta que diu: Si em pases Un Id vol dir que ets el propietari i que vols desfinalitzar a unaltre
            If pSocPropietariIVullDesfinalitzarLaAccioDalguAltre_IDActividadCRM_Personal = 0 Then
                _Destinatari = oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
            Else
                _Destinatari = oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.ID_ActividadCRM_Personal = pSocPropietariIVullDesfinalitzarLaAccioDalguAltre_IDActividadCRM_Personal).FirstOrDefault
            End If

            If _Destinatari Is Nothing Then
                Exit Function
            End If

            If _Destinatari.Finalizado = True Then
                _Destinatari.Finalizado = False
                oLinqActividad.PorcentajeFinalizada = CalcularPercentatgeFinalitzat()  '_PercentatgeFinalitzat
            End If
        End If

        oDTC.SubmitChanges()

        Return True

    End Function

    Private Function CalcularPercentatgeFinalitzat() As Integer
        Dim _NumDestinatarisFinalitzats As Integer = oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.Finalizado = True).Count
        If _NumDestinatarisFinalitzats = 0 Then
            Return 0
        Else
            Return Math.Round(_NumDestinatarisFinalitzats / oLinqActividad.ActividadCRM_Personal.Count, 2) * 100
        End If

    End Function

    Public Sub DarPorLeidaLaActividad()
        If oLinqActividad.ID_ActividadCRM = 0 Then
            Exit Sub
        End If

        If oEsPropietari = False Then 'si no ets el propietari de l'activitat donarem per llegida l'activitat
            Dim _Destinatari As ActividadCRM_Personal = oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = oIDPersonalActual).FirstOrDefault
            If _Destinatari Is Nothing Then
                Exit Sub
            End If

            _Destinatari.Leido = True
            oDTC.SubmitChanges()
        End If
    End Sub

    Public Function TienePersmisoElUsuarioActualAEntrarALaAccion(ByVal pIDAccion As Integer) As Boolean
        Try
            'Si ets el propietari de l'activitat, o ets el propietari de l'acció o ets un dels destintaris de l'acció llavors podràs entrar dins

            Dim _Accio As ActividadCRM_Accion = oLinqActividad.ActividadCRM_Accion.Where(Function(F) F.ID_ActividadCRM_Accion = pIDAccion).FirstOrDefault
            If _Accio Is Nothing Then
                Return False
            End If

            If oEsPropietari = True Then 'Si ets el propietari de l'activitat
                Return True
            End If

            If _Accio.ID_Personal = oIDPersonalActual OrElse (_Accio.ActividadCRM_Accion_Personal Is Nothing = False AndAlso _Accio.ActividadCRM_Accion_Personal.Where(Function(F) F.ID_Personal = oIDPersonalActual).Count > 0) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function PuedeFinalizarUnaActivitat() As Boolean
        Try
            'Si no és el propietari de la activitat
            'Si es propietari d'alguna acció o es destinatari d'alguna acció sense estar acabada
            If oEsPropietari = True Then 'si ets el propietari
                Return True
            End If

            'Si no es propietari de cap acció i no està assignat a cap acció llavors no podrà tancar l'activitat
            If oLinqActividad.ActividadCRM_Accion.Where(Function(F) F.ID_Personal = oIDPersonalActual).Count = 0 AndAlso oLinqActividad.ActividadCRM_Accion.Where(Function(F) F.ActividadCRM_Accion_Personal.Where(Function(Y) Y.ID_Personal = oIDPersonalActual).Count > 0).Count = 0 Then
                Return False
            End If

            'no es pot tancar una activitat si tens accions on ets propietari pendents de acabar
            If oLinqActividad.ActividadCRM_Accion.Where(Function(F) F.ID_Personal = oIDPersonalActual And F.Finalizada = False).Count = 0 Then
                Return False
            End If

            'no es pot tancar una activitat on estàs de destinatari si encara no l'has donat per finalitzada
            If oLinqActividad.ActividadCRM_Accion.Where(Function(F) F.Finalizada = False And F.ActividadCRM_Accion_Personal.Where(Function(Y) Y.ID_Personal = oIDPersonalActual).Count > 0).Count = 0 Then
                Return False
            End If


            PuedeFinalizarUnaActivitat = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function HabilitarElBotonFinalizarUnaActividad() As Boolean
        Try
            'Si ja esta finalitzada no es podrà tornar a finalitzar
            If oLinqActividad.Finalizada = True Then
                Return False
            End If

            'Si ets el propietari sempre podràs tancar l'activitat (a menys que ja estigui tancada)
            If oEsPropietari = True Then
                Return True
            Else
                Dim _Personal As ActividadCRM_Personal = oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = oIDPersonalActual).FirstOrDefault
                If _Personal Is Nothing Then
                    Return False
                End If

                If _Personal.Finalizado = True Then 'Si ja està finalitzat no direm que es pot tornar a finalitzar
                    Return False
                Else
                    Return True
                End If
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function HabilitarElBotonDesFinalizarUnaActividad() As Boolean
        Try
            'Si ets el propietari sempre podràs desfinalitzar l'activitat (a menys que ja estigui desfinalitzada)
            If oEsPropietari = True Then
                'Si no esta finalitzada  no es podrà des finalitzar
                If oLinqActividad.Finalizada = False Then
                    Return False
                Else
                    Return True
                End If

            Else
                Dim _Personal As ActividadCRM_Personal = oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = oIDPersonalActual).FirstOrDefault
                If _Personal Is Nothing Then
                    Return False
                End If

                If _Personal.Finalizado = False Then 'Si ja està des finalitzada no direm que es pot tornar a des finalitzar
                    Return False
                Else
                    Return True
                End If
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Sub AfegirDestinatarilALaActivitat(ByVal pIDPersonal As Integer, ByVal pLeido As Boolean, ByVal pFinalizado As Boolean)
        Dim _NewPersonal As New ActividadCRM_Personal
        _NewPersonal.Finalizado = pFinalizado
        _NewPersonal.Leido = pLeido
        _NewPersonal.Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = pIDPersonal).FirstOrDefault
        oLinqActividad.ActividadCRM_Personal.Add(_NewPersonal)
        oDTC.SubmitChanges()

    End Sub

    Public Sub DonarPerLlegitTotElChat()
        Dim _Chat As ActividadCRM_Chat
        For Each _Chat In oDTC.ActividadCRM_Chat.Where(Function(F) F.ID_ActividadCRM = oLinqActividad.ID_ActividadCRM And F.ID_Personal_Destino = oIDPersonalActual)
            _Chat.Leido = True
        Next
        oDTC.SubmitChanges()
    End Sub

    Public Function TincAccesALaActivitat() As Boolean
        If oLinqActividad.ID_Personal = oIDPersonalActual Then
            Return True
        End If

        If oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = oIDPersonalActual).Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub EnviarChatADemasDestinatarios(ByRef pChat As ActividadCRM_Chat)
        Try
            Dim _Destinatari As ActividadCRM_Personal
            For Each _Destinatari In oLinqActividad.ActividadCRM_Personal
                If _Destinatari.ID_Personal <> pChat.ID_Personal_Origen And _Destinatari.ID_Personal <> pChat.ID_Personal_Destino Then
                    Dim _Chat As New ActividadCRM_Chat
                    _Chat.Leido = False
                    _Chat.Mensaje = pChat.Mensaje
                    _Chat.FechaAlta = Date.Now
                    _Chat.Personal_Origen = pChat.Personal_Origen
                    _Chat.Personal_Destino = _Destinatari.Personal
                    oLinqActividad.ActividadCRM_Chat.Add(_Chat)
                End If
            Next
            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Shared Function RetornaSelect(ByVal pIDPersonal As Integer, ByVal pVisualizarPendientes As Boolean, ByVal pVisualizarFinalizados As Boolean, ByVal pVisualizarALaEsperaRespuesta As Boolean, ByVal pVisualizarSeguimimento As Boolean) As String
        Dim _Select As String
        _Select = "Select ID_ActividadCRM,TieneFichero, Cast(isnull((Select Leido From ActividadCRM_Personal Where ID_ActividadCRM=C_ActividadCRM.ID_ActividadCRM and ID_Personal=" & pIDPersonal & "),1) as int) as Leido,  Asunto, Propietario, FechaAlta as FechaAviso, cast(Finalizada as int) as Finalizada, ActividadTipo, FechaVencimiento, ID_Prioridad, ClienteNombre, Solucion, CASE WHEN C_ActividadCRM.ID_Personal = " & Seguretat.oUser.ID_Personal & " and SoloSeguimiento=1 then 1 else 0 end as EsSoloSeguimiento, CASE WHEN C_ActividadCRM.ID_PErsonal = " & Seguretat.oUser.ID_Personal & " and ALaEsperaRespuesta =1 then 1 else 0 end as EsALaEsperaRespuesta, (Select Finalizado From ActividadCRM_Personal Where ActividadCRM_Personal.ID_ActividadCRM=C_ActividadCRM.ID_ActividadCRM and ID_Personal=" & Seguretat.oUser.ID_Personal & ") as EstaFinalizada,PorcentajeFinalizada, Descripcion  From C_ActividadCRM "
        If pVisualizarFinalizados = False Then
            _Select = _Select & "where Activo=1 and Finalizada=" & Util.Bool_To_Int(pVisualizarFinalizados) & " and ID_ActividadCRM in (Select ID_ActividadCRM From C_ActividadCRM_PersonalAsignadoOPropietario Where Finalizado= " & Util.Bool_To_Int(pVisualizarFinalizados) & " and EsSoloSeguimiento=" & Util.Bool_To_Int(pVisualizarSeguimimento) & " and EsALaEsperaRespuesta=" & Util.Bool_To_Int(pVisualizarALaEsperaRespuesta) & " and ID_Personal =" & pIDPersonal & ") "
        Else
            _Select = _Select & "where Activo=1 and (Finalizada=1 and ID_Personal =" & pIDPersonal & ")  or ID_ActividadCRM in (Select ID_ActividadCRM From C_ActividadCRM_PersonalAsignadoOPropietario Where Finalizado= 1 and ID_Personal =" & pIDPersonal & ") "
        End If

        Return _Select

    End Function

    Public Shared Function RetornaDTActivitatsAVisualitzarSegonsPersonal(ByVal pIDPersonal As Integer, ByVal pVisualizarPendientes As Boolean, ByVal pVisualizarFinalizados As Boolean, ByVal pVisualizarALaEsperaRespuesta As Boolean, ByVal pVisualizarSeguimimento As Boolean) As DataTable
        Try

            Dim _Select As String = ""
            If pVisualizarPendientes = True Then
                _Select = "(" & RetornaSelect(pIDPersonal, True, False, False, False) & ")"
            End If
            If pVisualizarALaEsperaRespuesta = True Then
                If _Select <> "" Then
                    _Select = _Select & " Union All "
                End If
                _Select = _Select & "(" & RetornaSelect(pIDPersonal, True, False, True, False) & ")"
            End If
            If pVisualizarSeguimimento = True Then
                If _Select <> "" Then
                    _Select = _Select & " Union All "
                End If
                _Select = _Select & "(" & RetornaSelect(pIDPersonal, True, False, False, True) & ")"
            End If
            If pVisualizarFinalizados = True Then
                If _Select <> "" Then
                    _Select = _Select & " Union All "
                End If
                _Select = _Select & "(" & RetornaSelect(pIDPersonal, False, True, False, False) & ")"
            End If

            Dim _DT As DataTable
            If _Select = "" Then
                _DT = Nothing
            Else
                _DT = BD.RetornaDataTable(_Select, True)
            End If

            'Dim _StrVisualizarFinalizados As String = ""
            'Dim _StrVisualizarRealizados As String = ""
            'If pVisualizarFinalizados = True And pVisualizarPendientes = True Then

            'Else
            '    If pVisualizarFinalizados = True And pVisualizarPendientes = False Then
            '        _StrVisualizarFinalizados = " Finalizado=1 Or  "
            '        _StrVisualizarRealizados = " Finalizada=1 and "
            '    Else
            '        If pVisualizarFinalizados = False And pVisualizarPendientes = True Then
            '            _StrVisualizarFinalizados = " Finalizado=0 and  "
            '            _StrVisualizarRealizados = " Finalizada=0 and "
            '        Else
            '            _StrVisualizarFinalizados = " 1=0 or  "  'fem aixo pq mai es compleixi
            '            _StrVisualizarRealizados = " 1=0 and "
            '        End If
            '    End If
            'End If

            ''If pVisualizarTodos = False Then
            ''    _StrVisualizarFinalizados = " Finalizado=0 and  "
            ''    _StrVisualizarRealizados = " Finalizada=0 and "
            ''End If

            'Dim _strVisualizarSeguimiento As String = ""
            'If pVisualizarSeguimimento = False Then
            '    _strVisualizarSeguimiento = " EsSoloSeguimiento=0 and "
            'End If


            'Dim _strVisualizarALaEsperaRespuesta As String = ""
            'If pVisualizarALaEsperaRespuesta = False Then
            '    _strVisualizarALaEsperaRespuesta = " EsALaEsperaRespuesta=0 and "
            'End If


            'Dim _DT As DataTable
            '_DT = BD.RetornaDataTable("Select ID_ActividadCRM,TieneFichero, Cast(isnull((Select Leido From ActividadCRM_Personal Where ID_ActividadCRM=C_ActividadCRM.ID_ActividadCRM and ID_Personal=" & pIDPersonal & "),1) as int) as Leido,  Asunto, Propietario, FechaAlta as FechaAviso, cast(Finalizada as int) as Finalizada, ActividadTipo, FechaVencimiento, ID_Prioridad, ClienteNombre, Solucion, CASE WHEN C_ActividadCRM.ID_Personal = " & Seguretat.oUser.ID_Personal & " and SoloSeguimiento=1 then 1 else 0 end as EsSoloSeguimiento, CASE WHEN C_ActividadCRM.ID_PErsonal = " & Seguretat.oUser.ID_Personal & " and ALaEsperaRespuesta =1 then 1 else 0 end as EsALaEsperaRespuesta, (Select Finalizado From ActividadCRM_Personal Where ActividadCRM_Personal.ID_ActividadCRM=C_ActividadCRM.ID_ActividadCRM and ID_Personal=" & Seguretat.oUser.ID_Personal & ") as EstaFinalizada,PorcentajeFinalizada  From C_ActividadCRM where Activo=1 and " & _StrVisualizarRealizados & " ID_ActividadCRM in (Select ID_ActividadCRM From C_ActividadCRM_PersonalAsignadoOPropietario Where " & _StrVisualizarFinalizados & _strVisualizarSeguimiento & _strVisualizarALaEsperaRespuesta & " ID_Personal =" & pIDPersonal & ")  Order by FechaAlta Desc", True)
            Return _DT
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function RetornaNumActivitatsPendentsLlegir(ByVal pIDPersonal As Integer) As Integer
        Try
            Dim _StrVisualizarFinalizados As String = ""
            Dim _StrVisualizarRealizados As String = ""

            Return BD.RetornaValorSQL("Select Count(*)  From C_ActividadCRM where  Activo=1 and Finalizada=0 and  ID_ActividadCRM in (Select ID_ActividadCRM From ActividadCRM_Personal Where Leido=0 and ID_Personal =" & pIDPersonal & ")")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Sub DarPorNoLeidoALosDestinatariosDeUnaAccionDeUnaActividad(ByRef pDTC As DTCDataContext, ByRef pAccion As ActividadCRM_Accion)
        Try
            Dim _Personal As ActividadCRM_Accion_Personal

            For Each _Personal In pAccion.ActividadCRM_Accion_Personal
                Dim _PersonalAssignat As ActividadCRM_Personal = _Personal.ActividadCRM_Accion.ActividadCRM.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = _Personal.ID_Personal).FirstOrDefault
                If _PersonalAssignat Is Nothing = False Then
                    _PersonalAssignat.Finalizado = False
                End If
            Next
            pDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Shared Function RetornaDTChatsActivitatsSegonsPersonal(ByVal pIDPersonal As Integer, ByVal pVisualizarTodos As Boolean) As DataTable
        Try
            Dim _Todas As String = ""
            If pVisualizarTodos = False Then
                _Todas = " Leido=0 and "
            Else

            End If


            Dim _DT As DataTable
            _DT = BD.RetornaDataTable("Select *  From C_ActividadCRM_Chat where  Activo=1 and " & _Todas & " ID_Personal_Destino=" & pIDPersonal & " Order by FechaAlta Desc", True)
            Return _DT
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function AñadirDestinatarioALaActividadSiHaceFalta(ByRef pDTC As DTCDataContext, ByRef pAccion As ActividadCRM_Accion, ByVal pIDPersonal As Integer)
        'Aquesta funció es pq al afegir un destinatari a una acció, si el destinatari no existeix a la activitat l'afegirem automáticament.

        If pAccion.ActividadCRM.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = pIDPersonal).Count = 0 Then 'Si la persona que anem a afegir no existeix a l'activitat
            Dim _Personal As New ActividadCRM_Personal
            '_Personal.ActividadCRM = pAccion.ActividadCRM
            _Personal.Finalizado = False
            _Personal.Leido = False
            _Personal.Personal = pDTC.Personal.Where(Function(F) F.ID_Personal = pIDPersonal).FirstOrDefault
            pAccion.ActividadCRM.ActividadCRM_Personal.Add(_Personal)
            pDTC.SubmitChanges()

        End If
    End Function

    Public Shared Function EnviarCorreuAlaBustia(ByVal pPara As String, ByVal pDe As String, ByVal pAsunto As String, ByVal pMensaje As String, ByVal pIDPersonalOrigen As Integer, ByVal pIDPersonalDestino As Integer, Optional ByVal pIDActividad As Integer = 0, Optional ByVal pIDActividad_Accion As Integer = 0)
        Try

            Dim oDTC As New DTCDataContext(BD.Conexion)
            Dim _Pou As New MailPool
            With _Pou

                _Pou.De = pDe
                _Pou.Para = pPara
                _Pou.Asunto = pAsunto
                _Pou.Mensaje = pMensaje

                If pIDActividad <> 0 Then
                    _Pou.ID_ActividadCRM = pIDActividad
                End If

                If pIDActividad_Accion <> 0 Then
                    _Pou.ID_ActividadCRM_Accion = pIDActividad_Accion
                End If

                _Pou.ID_Personal_Destino = pIDPersonalDestino
                _Pou.ID_Personal_Origen = pIDPersonalOrigen

                _Pou.Fecha = Now
                _Pou.Enviado = False

                oDTC.MailPool.InsertOnSubmit(_Pou)
                oDTC.SubmitChanges()
            End With



            'Quan tu enviis un correu i el programa el llegeixi ha d'enviar un coreu al propietari i unaltre a cada destinatari.
            '----Quan s'assigni un destinatari a una activitat s'enviara un correu al destinatari
            '-----Quan s'assigni un destinatari a una acció s'enviarà un correu al destinatari
            'Quan els destinataris d'una acció finalitzin la seva part de la activitat s'envii un email al propietari
        Catch ex As Exception

        End Try


    End Function
End Class
