Public Class frmManteniment_Script
    Dim oDTC As New DTCDataContext(BD.Conexion)
    Dim oLinqConfiguracio As Configuracion

    Public Sub Entrada()
        Me.AplicarDisseny()
        oLinqConfiguracio = oDTC.Configuracion.FirstOrDefault
        Me.RT1.Text = oLinqConfiguracio.ListadoScriptAccesoAFormularios
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        oLinqConfiguracio.ListadoScriptAccesoAFormularios = Me.RT1.Text
        oDTC.SubmitChanges()
    End Sub

End Class