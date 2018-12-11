Public Class frmProducto_Alternativo
    Dim oDTC As DTCDataContext
    Dim olinqAlternativo As Producto_Alternativo
    Dim oLinqProducto As Producto

    Public Sub Entrada(ByRef pProducto As Producto, ByRef pDTC As DTCDataContext, Optional ByRef pIDAlternativo As Integer = 0)
        oDTC = pDTC
        Dim IDAlternativo As Integer = pIDAlternativo
        oLinqProducto = pProducto

        Me.AplicarDisseny()
        Me.T_Unidades.Value = 1

        If IDAlternativo <> 0 Then
            olinqAlternativo = oDTC.Producto_Alternativo.Where(Function(F) F.ID_Producto_Alternativo = IDAlternativo).FirstOrDefault
            Me.T_Producto_Descripcion.Text = olinqAlternativo.Producto1.Descripcion
            Me.T_Unidades.Value = olinqAlternativo.Cantidad
            Me.TE_Producto_Codigo.Text = olinqAlternativo.Producto1.Codigo
            Me.TE_Producto_Codigo.Tag = olinqAlternativo.ID_Producto_Necesario
        End If
    End Sub

    Private Sub TE_Producto_Codigo_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Producto_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto ORDER BY Descripcion", Me.TE_Producto_Codigo, "ID_Producto", "Codigo", Me.T_Producto_Descripcion, "Descripcion")

            'AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGeneric

        End If
    End Sub

    Private Sub TE_Producto_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Producto_Codigo.KeyDown
        If Me.TE_Producto_Codigo.Text Is Nothing = False Then
            If e.KeyCode = Keys.Enter Then
                Dim ooLinqProducto As Producto = oDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Producto_Codigo.Text).FirstOrDefault()
                If ooLinqProducto Is Nothing = False Then
                    Me.TE_Producto_Codigo.Tag = ooLinqProducto.ID_Producto
                    Me.T_Producto_Descripcion.Text = ooLinqProducto.Descripcion
                    If Me.T_Unidades.Visible = True Then
                        Me.T_Unidades.Focus()
                    Else
                        Me.T_Producto_Descripcion.Focus()
                    End If
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Me.TE_Producto_Codigo.Value = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub ToolForm_m_ToolForm_Seleccionar() Handles ToolForm.m_ToolForm_Seleccionar
        Try

            If Me.TE_Producto_Codigo.Tag Is Nothing OrElse Me.T_Unidades.Value Is Nothing OrElse IsDBNull(Me.T_Unidades.Value) = True OrElse Me.T_Unidades.Value = 0 Then
                Mensaje.Mostrar_Mensaje("Imposible guardar, el producto y las unidades son campos requeridos.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If olinqAlternativo Is Nothing Then

                Dim _NewAlternativo As New Producto_Alternativo
                _NewAlternativo.Cantidad = Me.T_Unidades.Value
                _NewAlternativo.Producto1 = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
                oLinqProducto.Producto_Alternativo.Add(_NewAlternativo)
                'oDTC.Producto_Alternativo.InsertOnSubmit(_NewAlternativo)

            Else
                olinqAlternativo.Cantidad = Me.T_Unidades.Value
                olinqAlternativo.Producto1 = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
            End If

            oDTC.SubmitChanges()

            Me.FormTancar()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub
End Class