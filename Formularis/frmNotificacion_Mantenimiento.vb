Public Class frmNotificacion_Mantenimiento
    Dim oDTC As DTCDataContext
    Dim oLinqNotificacion As Notificacion

#Region "Toolform"
    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If Guardar() = True Then
            Call ToolForm_m_ToolForm_Salir()
        End If
    End Sub
#End Region

#Region "Métodes"
    Public Sub Entrada(ByRef pDTC As DTCDataContext, Optional ByVal pID As Integer = 0)
        Me.AplicarDisseny()

        oDTC = pDTC

        Util.Cargar_Combo(Me.C_Usuario, "Select ID_Usuario, NombreCompleto From Usuario Where Activo=1 and ID_Usuario <> " & Seguretat.oUser.ID_Usuario & " Order by Nombre", False)


        If pID <> 0 Then
            Call Cargar_Form(pID)
        Else
            Call Netejar_Pantalla()
        End If

    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqNotificacion = (From taula In oDTC.Notificacion Where taula.ID_Notificacion = pID Select taula).FirstOrDefault
            Call SetToForm()

            Me.EstableixCaptionForm("Notificación: " & (oLinqNotificacion.ID_Notificacion))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqNotificacion = New Notificacion
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.T_Descripcion.Focus()
        Me.C_Usuario.Value = Nothing

        Me.DT_Alta.Value = Now

        ErrorProvider1.Clear()

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.T_Descripcion)
                oClsControls.ControlBuit(.DT_Alta)
                oClsControls.ControlBuit(.C_Usuario)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqNotificacion
                Me.T_CodigoNotificacion.Value = .ID_Notificacion
                Me.T_Descripcion.Text = .Descripcion
                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Limite.Value = .FechaLimite
                Me.T_Observaciones.Text = .Observaciones
                Me.C_Usuario.Value = .ID_Usuario_Destino
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pNotificacion As Notificacion)
        Try
            With pNotificacion
                .Usuario_Origen = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault
                .Usuario_Destino = oDTC.Usuario.Where(Function(F) F.ID_Usuario = CInt(Me.C_Usuario.Value)).FirstOrDefault

                .FechaAlta = Me.DT_Alta.Value


                .FechaLimite = Me.DT_Limite.Value
                .Descripcion = Me.T_Descripcion.Text
                .Observaciones = Me.T_Observaciones.Text
                .Automatica = False
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.T_Descripcion.Focus()

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqNotificacion)

            If oLinqNotificacion.ID_Notificacion = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oLinqNotificacion.Leido = False
                oLinqNotificacion.Realizado = False
                oDTC.Notificacion.InsertOnSubmit(oLinqNotificacion)
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)

            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If

            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

#End Region

End Class