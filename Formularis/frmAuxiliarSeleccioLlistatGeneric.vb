Public Class frmAuxiliarSeleccioLlistatGeneric
    Public Shadows Event AlTancarForm(ByVal pID As Integer)
    Dim oTipusEntrada As EnumTipusEntrada
    Dim oValorAuxiliar1 As String
    Public Enum EnumTipusEntrada
        SeleccionarProducto = 0
        SeleccionInstalacionAlDuplicarPropuestaAOtraInstalacion = 1
        SeleccionarAlmacen = 2
        SeleccionInstalacionAlMoverPropuestaAOtraInstalacion = 3
    End Enum

    Public Sub Entrada(ByVal pTipusEntrada As EnumTipusEntrada, Optional ByVal pValorAuxiliar1 As String = Nothing)
        Me.AplicarDisseny()

        Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Seleccionar"

        oValorAuxiliar1 = pValorAuxiliar1
        oTipusEntrada = pTipusEntrada

        Me.TE_Seleccio.ButtonsRight("Lupeta").Enabled = True

        Select Case pTipusEntrada
            Case EnumTipusEntrada.SeleccionarProducto
                Me.Text = "Seleccione un produco"
                Me.UltraLabel5.Text = "Producto:"
            Case EnumTipusEntrada.SeleccionInstalacionAlDuplicarPropuestaAOtraInstalacion
                Me.Text = "Seleccione una instalación"
                Me.UltraLabel5.Text = "Código de instalación:"
            Case EnumTipusEntrada.SeleccionInstalacionAlDuplicarPropuestaAOtraInstalacion
                Me.Text = "Seleccione un almacén"
                Me.UltraLabel5.Text = "Código de almacén:"
            Case EnumTipusEntrada.SeleccionInstalacionAlMoverPropuestaAOtraInstalacion
                Me.Text = "Seleccione una instalación"
                Me.UltraLabel5.Text = "Código de instalación:"
        End Select

    End Sub

    Private Sub ToolForm_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If Me.TE_Seleccio.Tag Is Nothing = True Then
            Mensaje.Mostrar_Mensaje("El campo descripcion es obligatorio", M_Mensaje.Missatge_Modo.INFORMACIO)
            Exit Sub
        End If

        RaiseEvent AlTancarForm(CInt(Me.TE_Seleccio.Tag))
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub TE_Seleccio_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Seleccio.EditorButtonClick
        Try
            If e.Button.Key = "Lupeta" Then
                Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
                Select Case oTipusEntrada
                    Case EnumTipusEntrada.SeleccionarProducto
                        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto ORDER BY Descripcion", Me.TE_Seleccio, "ID_Producto", "Descripcion")
                    Case EnumTipusEntrada.SeleccionInstalacionAlDuplicarPropuestaAOtraInstalacion, EnumTipusEntrada.SeleccionInstalacionAlMoverPropuestaAOtraInstalacion
                        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Instalacion Where Activo=1 and ID_Cliente=" & oValorAuxiliar1 & " ORDER BY ID_Instalacion Desc", Me.TE_Seleccio, "ID_Instalacion", "ID_Instalacion")
                        AddHandler LlistatGeneric.AlApretarElBotoAuxiliar, AddressOf AlApretarElBotoAuxiliarDelLlistatGeneric
                        LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = True
                        LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Caption = "Visualizar todas las instalaciones"
                    Case EnumTipusEntrada.SeleccionarAlmacen
                        LlistatGeneric.Mostrar_Llistat("SELECT ID_Almacen, Descripcion FROM Almacen Where Activo=1 ORDER BY Descripcion", Me.TE_Seleccio, "ID_Almacen", "Descripcion")

                End Select

                'AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGeneric
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarLlistatGeneric(ByVal pID As String)
        'If pID Is Nothing Then
        'Else
        '    Select Case oTipusEntrada
        '        Case EnumTipusEntrada.SeleccionarProducto

        '    End Select


        'End If

    End Sub

    Private Sub AlApretarElBotoAuxiliarDelLlistatGeneric(ByRef pInstanciaLlistatGeneric As M_LlistatGeneric.clsLlistatGeneric)
        Select Case oTipusEntrada
            Case EnumTipusEntrada.SeleccionarProducto

            Case EnumTipusEntrada.SeleccionInstalacionAlDuplicarPropuestaAOtraInstalacion, EnumTipusEntrada.SeleccionInstalacionAlMoverPropuestaAOtraInstalacion
                pInstanciaLlistatGeneric.pGrid.M.clsUltraGrid.Cargar("SELECT * FROM C_Instalacion Where  activo=1 ORDER BY ID_Instalacion desc", BD)
                pInstanciaLlistatGeneric.AplicarCanvisBotoAuxiliarAlNouGrid()
                pInstanciaLlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = False
        End Select


    End Sub

End Class