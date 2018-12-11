Public Class frmProvaMemoria

    'Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
    '    Me.FormTancar()
    'End Sub

    Public Sub Entrada()
        Me.AplicarDisseny()
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub UltraButton1_Click(sender As Object, e As EventArgs) Handles UltraButton1.Click
        Me.Close()
    End Sub
End Class