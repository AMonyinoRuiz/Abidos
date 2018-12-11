Public Class frmDiagrama
    Dim oDTC As DTCDataContext
    Dim oLinQDiagrama As Propuesta_Diagrama

    Public Sub Entrada(ByVal pDTC As DTCDataContext, ByRef pDiagrama As Propuesta_Diagrama)
        Me.AplicarDisseny()
        oDTC = pDTC
        oLinQDiagrama = pDiagrama

        If pDiagrama.ID_DiagramaBinario.HasValue = True Then
            Dim dStream As System.IO.MemoryStream = New System.IO.MemoryStream(oLinQDiagrama.DiagramaBinario.Fichero.ToArray, 0, oLinQDiagrama.DiagramaBinario.Fichero.Length)
            dStream.Position = 0
            DiagramControl1.LoadDocument(dStream)
            dStream.Close()
        End If

        'DiagramControl1.Toolbox.ShowSearchPanel = True
        ' DiagramControl1.Toolbox.State = DevExpress.XtraToolbox.ToolboxState.Normal

    End Sub

    Private Sub B_Salir_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles B_Salir.ItemClick

        If Mensaje.Mostrar_Mensaje("¿Desea guardar el diagrama antes de salir?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
            Call B_Guardar_ItemClick(Nothing, Nothing)
        End If
        Me.FormTancar()

    End Sub

    Private Sub B_Guardar_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles B_Guardar.ItemClick
        Dim _Archivo As DiagramaBinario

        If oLinQDiagrama.ID_DiagramaBinario.HasValue = False Then 'Si entrem al diseñador per primera vegada no s'haurà creat el ID de arhivo per tant farem un insert
            _Archivo = New DiagramaBinario
        Else
            _Archivo = oDTC.DiagramaBinario.Where(Function(F) F.ID_DiagramaBinario = oLinQDiagrama.ID_DiagramaBinario).FirstOrDefault
        End If

        Dim dStream As System.IO.MemoryStream = New System.IO.MemoryStream()
        DiagramControl1.SaveDocument(dStream)

        Dim dStreamFoto As System.IO.MemoryStream = New System.IO.MemoryStream()
        DirectCast(Me.DiagramControl1, DevExpress.Diagram.Core.IDiagramControl).ExportDiagram(dStreamFoto, DevExpress.Diagram.Core.DiagramExportFormat.JPEG)


        _Archivo.Fichero = dStream.ToArray
        _Archivo.Foto = dStreamFoto.ToArray
        oLinQDiagrama.DiagramaBinario = _Archivo


        If oLinQDiagrama.ID_DiagramaBinario.HasValue = False Then
            oDTC.DiagramaBinario.InsertOnSubmit(_Archivo)
        End If
        oDTC.SubmitChanges()
    End Sub
End Class