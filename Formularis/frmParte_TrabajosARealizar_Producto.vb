Public Class frmParte_TrabajosARealizar_Producto
    Dim oDTC As DTCDataContext
    Dim olinqTrabajo As Parte_TrabajosARealizar
    Dim oLinqProducto As Parte_TrabajosARealizar_Producto


#Region "Toolform"
    Private Sub ToolForm_m_ToolForm_ToolClick_Botones_Extras(Sender As Object, e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        Select Case e.Tool.Key
            Case "GuardarYContinuar"
                Call Guardar()
                Me.T_Producto_Descripcion.Text = ""
                Me.TE_Producto_Codigo.Tag = Nothing
                Me.TE_Producto_Codigo.Text = ""
                Me.T_Unidades.Value = 0
                'Case "GuardarYMantener"
                '    Call ToolForm_m_ToolForm_Seleccionar()

                '    If Guardar() = True Then
                '        oLinqTrabajosARealizar = New Parte_TrabajosARealizar
                '    End If

        End Select
    End Sub

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

    Public Sub Entrada(ByRef pTrabajo As Parte_TrabajosARealizar, ByRef pDTC As DTCDataContext, Optional ByRef pIDAlternativo As Integer = 0)
        oDTC = pDTC
        Dim IDAlternativo As Integer = pIDAlternativo
        olinqTrabajo = pTrabajo

        Me.ToolForm.M.clsToolBar.Afegir_Boto("GuardarYContinuar", "Guardar y continuar", True)
        Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Caption = "Guardar"
        ' Me.ToolForm.M.clsToolBar.Afegir_Boto("GuardarYMantener", "Guardar y mantener", True)

        Me.AplicarDisseny()
        Me.T_Unidades.Value = 1

        If IDAlternativo <> 0 Then
            oLinqProducto = oDTC.Parte_TrabajosARealizar_Producto.Where(Function(F) F.ID_Parte_TrabajosARealizar_Producto = IDAlternativo).FirstOrDefault
            Me.T_Producto_Descripcion.Text = oLinqProducto.Producto.Descripcion
            Me.T_Unidades.Value = oLinqProducto.Cantidad
            Me.TE_Producto_Codigo.Text = oLinqProducto.Producto.Codigo
            Me.TE_Producto_Codigo.Tag = oLinqProducto.ID_Producto
        End If
    End Sub

    Private Sub Guardar()
        If Me.TE_Producto_Codigo.Tag Is Nothing OrElse Me.T_Unidades.Value Is Nothing OrElse IsDBNull(Me.T_Unidades.Value) = True OrElse Me.T_Unidades.Value = 0 Then
            Mensaje.Mostrar_Mensaje("Imposible guardar, el producto y las unidades son campos requeridos.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        If oLinqProducto Is Nothing Then

            Dim _NewProducto As New Parte_TrabajosARealizar_Producto
            _NewProducto.Cantidad = Me.T_Unidades.Value
            _NewProducto.Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
            olinqTrabajo.Parte_TrabajosARealizar_Producto.Add(_NewProducto)
            'oDTC.Producto_Alternativo.InsertOnSubmit(_NewAlternativo)

        Else
            oLinqProducto.Cantidad = Me.T_Unidades.Value
            oLinqProducto.Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
        End If

        oDTC.SubmitChanges()
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

   
End Class