Public Class frmAuxiliarDesdeHasta
    Public Shadows Event AlTancarForm(ByVal pDescripcio As String, ByVal pDesde As Integer, ByVal pHasta As Integer)

    Public Sub Entrada(ByVal pDescripcio As String)
        Me.AplicarDisseny()
        Me.T_Descripcion.Text = pDescripcio

    End Sub

    Private Sub ToolForm_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If Me.T_Descripcion.Text Is Nothing = True OrElse Me.T_Descripcion.Text.Length = 0 Then
            Mensaje.Mostrar_Mensaje("El campo descripcion es obligatorio", M_Mensaje.Missatge_Modo.INFORMACIO)
            Exit Sub
        End If

        If Me.T_Desde.Value Is Nothing OrElse Me.T_Hasta.Value Is Nothing OrElse Me.T_Hasta.Value < Me.T_Desde.Value OrElse Me.T_Hasta.Value = 0 Then
            Mensaje.Mostrar_Mensaje("Datos incorrectos", M_Mensaje.Missatge_Modo.INFORMACIO)
            Exit Sub
        End If

        RaiseEvent AlTancarForm(Me.T_Descripcion.Text, Me.T_Desde.Value, Me.T_Hasta.Value)
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub
End Class