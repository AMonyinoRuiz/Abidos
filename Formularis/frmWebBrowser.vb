Public Class frmWebBrowser
    'Dim oDTC As New DTCDataContext(BD.Conexion)
    Dim oURL As String
    Public FormCarregat As Boolean

    Public Sub Entrada(ByVal pURL As String)
        'MsgBox(oDTC.Configuracion.Where(Function(F) F.ID_Configuracion = 1).FirstOrDefault.URLPantallaPrincipal)
        'Me.WebBrowser1.Navigate(oDTC.Configuracion.Where(Function(F) F.ID_Configuracion = 1).FirstOrDefault.URLPantallaPrincipal)
        oURL = pURL
        'Me.WebBrowser1.Navigate(pURL)
        FormCarregat = True
        'Me.WebBrowser1.Url = pepe
    End Sub

    Private Sub frmWebBrowser_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        Me.WebBrowser1.Navigate(oURL)
        'Mensaje.Mostrar_Mensaje("aaa", M_Mensaje.Missatge_Modo.INFORMACIO, "")
    End Sub



    Private Sub frmWebBrowser_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        'If TancadaForçada = False Then
        '    e.Cancel = True
        'End If
    End Sub
End Class