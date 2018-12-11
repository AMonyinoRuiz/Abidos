Public Class frmPersonal_Incidencia
    Dim oDTC As DTCDataContext
    Dim oLinqIncidencia As Personal_Incidencia
    Dim oLinqPersonal As Personal

#Region "Métodes"

    Public Sub Entrada(ByRef pPersonal As Personal, ByRef pDTC As DTCDataContext, Optional ByRef pIDIncidencia As Integer = 0)

        Me.AplicarDisseny()
        oDTC = pDTC
        oLinqPersonal = pPersonal
        Me.DT_Alta.Value = Now.Date
        Me.TE_Cliente.ButtonsRight("Lupeta").Enabled = True


        If pIDIncidencia <> 0 Then
            Call CargarForm(pIDIncidencia)
        Else
            oLinqIncidencia = New Personal_Incidencia
        End If
    End Sub

    Private Sub CargarForm(ByVal pID As Integer)
        oLinqIncidencia = (From taula In oDTC.Personal_Incidencia Where taula.ID_Personal_Incidencia = pID Select taula).First
        Call SetToForm()
    End Sub

    Private Sub SetToForm()
        Try

            With oLinqIncidencia
                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Incidencia.Value = .FechaIncidencia
                Me.T_Lugar.Text = .Lugar
                Me.T_MaterialAfectado.Text = .MaterialAfectado
                Me.T_Testigos.Text = .Testigos
                If .Cliente Is Nothing = False Then
                    Me.TE_Cliente.Tag = .ID_Cliente
                    Me.TE_Cliente.Text = .Cliente.Nombre
                End If
                Me.R_Descripcion.pText = .Descripcion
                Me.R_Sancion.pText = .Sancion
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pIncidencia As Personal_Incidencia)
        Try
            With pIncidencia
                .FechaAlta = Me.DT_Alta.Value
                .FechaIncidencia = Me.DT_Incidencia.Value
                .Lugar = Me.T_Lugar.Text
                .MaterialAfectado = Me.T_MaterialAfectado.Text
                .Testigos = Me.T_Testigos.Text
                If Me.TE_Cliente.Tag Is Nothing Then
                    .Cliente = Nothing
                Else
                    .Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(Me.TE_Cliente.Tag)).FirstOrDefault
                End If
                .Descripcion = Me.R_Descripcion.pText
                .Sancion = Me.R_Sancion.pText
                .Personal = oLinqPersonal
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqIncidencia)

            If oLinqIncidencia.ID_Cliente = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Personal_Incidencia.InsertOnSubmit(oLinqIncidencia)
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

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.DT_Alta)
                oClsControls.ControlBuit(.DT_Incidencia)
                oClsControls.ControlBuit(.T_Lugar)
                oClsControls.ControlBuit(.R_Descripcion)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function
#End Region

    Private Sub TE_Cliente_Codigo_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Cliente.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Cliente ORDER BY Nombre", Me.TE_Cliente, "ID_Cliente", "Nombre")
        End If
    End Sub

    Private Sub ToolForm_m_ToolForm_Seleccionar() Handles ToolForm.m_ToolForm_Seleccionar
        Try

            If Guardar() = True Then
                Me.FormTancar()
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

End Class