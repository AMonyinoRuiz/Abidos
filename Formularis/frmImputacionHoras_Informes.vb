Public Class frmImputacionHoras_Informes
    Dim oDTC As DTCDataContext
    Dim olinqParte As Parte

#Region "Toolform"

    Private Sub ToolForm_m_ToolForm_Seleccionar() Handles ToolForm.m_ToolForm_Seleccionar
        Try
            Call Guardar()

            Me.FormTancar()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub
#End Region

    Public Sub Entrada(ByVal pParte As Parte, ByRef pDTC As DTCDataContext)
        oDTC = pDTC
        olinqParte = pParte

        Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Caption = "Guardar"

        Me.AplicarDisseny()

        Me.R_ExplicacionHoras.RichText.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.PrintLayout
        Me.R_Informe.RichText.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.PrintLayout
        Me.R_Informe.pText = pParte.Parte_Aux.ObservacionesTecnico
        Me.R_ExplicacionHoras.pText = pParte.Parte_Aux.ExplicacionHorasTecnico

    End Sub

    Private Sub Guardar()
        'Dim _DTC As New DTCDataContext(BD.Conexion)

        Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = olinqParte.ID_Parte).FirstOrDefault
        If Me.R_Informe.RichText.Text = "" Then
            _Parte.Parte_Aux.ObservacionesTecnico = Nothing
        Else
            _Parte.Parte_Aux.ObservacionesTecnico = Me.R_Informe.pText
        End If

        If Me.R_ExplicacionHoras.RichText.Text = "" Then
            _Parte.Parte_Aux.ExplicacionHorasTecnico = Nothing
        Else
            _Parte.Parte_Aux.ExplicacionHorasTecnico = Me.R_ExplicacionHoras.pText
        End If

        oDTC.SubmitChanges()
        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE, "")
    End Sub

End Class