Imports Infragistics.Win.UltraWinGrid

Public Class frmFacturacionAlbaranes
    Enum EnumTipoDeEntrada
        Ventas = 1
        Compras = 2
    End Enum
    Dim oEntradaTipo As EnumTipoDeEntrada
    Dim oRowsSeleccionadas As ArrayList

#Region "ToolForm"
    Private Sub ToolForm_m_ToolForm_Nuevo() Handles ToolForm.m_ToolForm_Nuevo
        Call NetejarPantalla()

    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

    Public Sub Entrada(ByVal pParametreQueNoServeixPerARe As Integer, ByVal pTipoEntrada As EnumEntradaTipo)
        Try
            Me.AplicarDisseny()

            oEntradaTipo = pTipoEntrada

            Me.ToolForm.M.Botons.tGuardar.SharedProps.Visible = True
            Me.ToolForm.M.Botons.tEliminar.SharedProps.Visible = False
            Me.ToolForm.M.Botons.tBuscar.SharedProps.Visible = False

            Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Generar factura"

            Util.Cargar_Combo(Me.C_FormaPago, "SELECT ID_FormaPago, Descripcion FROM FormaPago Where Activo=1 ORDER BY Codigo", False)
            'Carregarem 31 dias de la setmana al combo
            Dim i As Integer
            For i = 1 To 31
                Me.C_DiaPago.Items.Add(i, i)
            Next

            Util.Cargar_Combo(Me.C_Empresa, "Select ID_Empresa, NombreComercial From Empresa Where Activo=1  Order by NombreComercial ", False)

            Select Case oEntradaTipo
                Case EnumTipoDeEntrada.Compras
                    Me.L_Codigo.Text = "Código de proveedor:"
                    'La última factura només es vol visualitzar per les ventes
                    Me.L_UltimaFactura.Visible = False
                    Me.T_UltimaFactura.Visible = False
                Case EnumTipoDeEntrada.Ventas
                    Me.L_Codigo.Text = "Código de cliente:"
                    Dim oDTC As New DTCDataContext(BD.Conexion)
                    Me.T_UltimaFactura.Value = clsEntrada.RetornaCodiSeguent(EnumEntradaTipo.FacturaVenta, Me.C_Empresa.Value)
            End Select


            Call NetejarPantalla()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub TE_Codigo_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Select Case oEntradaTipo
                Case EnumTipoDeEntrada.Compras
                    Call Cridar_Llistat_Generic_Proveedores()
                Case EnumTipoDeEntrada.Ventas
                    Call Cridar_Llistat_Generic_Clientes()
            End Select

        End If
        Me.C_Empresa.Enabled = False
    End Sub

    Private Sub Cridar_Llistat_Generic_Clientes()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Cliente Where ID_Cliente_Tipo=" & EnumClienteTipo.Cliente & "  and Activo=1 and ID_Cliente in (Select ID_Cliente From Cliente_Empresa Where ID_Empresa=" & Me.C_Empresa.Value & ") ORDER BY Nombre", Me.TE_Codigo, "ID_Cliente", "Codigo", Me.T_Nombre, "Nombre")
        'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
        '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
        '        pRow.Appearance.BackColor = Color.LightCoral
        '    End If
        'Next
    End Sub

    Private Sub Cridar_Llistat_Generic_Proveedores()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM c_Proveedor Where Activo=1 and ID_Proveedor in (Select ID_Proveedor From Proveedor_Empresa Where ID_Empresa=" & Me.C_Empresa.Value & ") ORDER BY Nombre", Me.TE_Codigo, "ID_Proveedor", "Codigo", Me.T_Nombre, "Nombre")
        'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
        '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
        '        pRow.Appearance.BackColor = Color.LightCoral
        '    End If
        'Next
    End Sub

    Private Sub CargarGrid(ByVal pIDEntidad As Integer)
        Try
            Dim DT As DataTable
            Dim _TipoDocumento As Integer
            Dim _ClienteOProveedor As String = ""
            Select Case oEntradaTipo
                Case EnumTipoDeEntrada.Compras
                    _TipoDocumento = EnumEntradaTipo.AlbaranCompra
                    _ClienteOProveedor = " And ID_Proveedor=" & pIDEntidad
                Case EnumTipoDeEntrada.Ventas
                    _TipoDocumento = EnumEntradaTipo.AlbaranVenta
                    _ClienteOProveedor = " And ID_Cliente=" & pIDEntidad
            End Select


            With Me.GRD_Albaranes

                DT = BD.RetornaDataTable("Select cast(0 as bit) as Seleccion, * From C_Entrada Where ID_Entrada_Tipo=" & _TipoDocumento & _ClienteOProveedor & " and ID_Entrada_Estado<>" & EnumEntradaEstado.Cerrado, True)

                .M_Editable()
                .M.clsUltraGrid.Cargar(DT)



            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Albaranes_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Albaranes.M_Grid_InitializeRow

        e.Row.Cells("Seleccion").Column.CellClickAction = CellClickAction.CellSelect
        e.Row.Cells("Seleccion").Column.CellActivation = Activation.AllowEdit

    End Sub

    Private Sub GRD_Albaranes_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Albaranes.M_GRID_AfterCellActivate
        With GRD_Albaranes.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                If .ActiveCell.Value = True Then
                    oRowsSeleccionadas.Remove(.ActiveRow.Cells("ID_Entrada").Value)
                    .ActiveCell.Value = False
                Else
                    oRowsSeleccionadas.Add(.ActiveRow.Cells("ID_Entrada").Value)
                    .ActiveCell.Value = True
                End If
            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
            Call CalcularTotals()
        End With
    End Sub

    Private Sub B_CargarAlbaranes_Click(sender As Object, e As EventArgs) Handles B_CargarAlbaranes.Click
        Try
            If Me.TE_Codigo.Tag Is Nothing Then
                Select Case oEntradaTipo
                    Case EnumTipoDeEntrada.Ventas
                        Mensaje.Mostrar_Mensaje("Seleccione primero el cliente", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                    Case EnumTipoDeEntrada.Compras
                        Mensaje.Mostrar_Mensaje("Seleccione primero el proveedor", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                End Select
                Exit Sub
            End If

            Call CargarGrid(Me.TE_Codigo.Tag)

            Dim oDTC As New DTCDataContext(BD.Conexion)

            'Carreguem la forma de pago del client o del proveidor seleccionat
            Select Case oEntradaTipo
                Case EnumTipoDeEntrada.Ventas
                    Dim _Cliente As Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(Me.TE_Codigo.Tag)).FirstOrDefault
                    If _Cliente.ID_FormaPago.HasValue Then
                        Me.C_FormaPago.Value = _Cliente.ID_FormaPago
                    Else
                        Dim _FormaPago As FormaPago = oDTC.FormaPago.Where(Function(F) F.Predeterminada).FirstOrDefault
                        If _FormaPago Is Nothing = False Then
                            Me.C_FormaPago.Value = _FormaPago.ID_FormaPago
                        End If
                    End If
                    If _Cliente.DiaDePago.HasValue Then
                        Me.C_DiaPago.Value = _Cliente.DiaDePago
                    End If
                Case EnumTipoDeEntrada.Compras
                    Dim _Proveidor As Proveedor = oDTC.Proveedor.Where(Function(F) F.ID_Proveedor = CInt(Me.TE_Codigo.Tag)).FirstOrDefault
                    If _Proveidor.ID_FormaPago.HasValue Then
                        Me.C_FormaPago.Value = _Proveidor.ID_FormaPago
                    Else
                        Dim _FormaPago As FormaPago = oDTC.FormaPago.Where(Function(F) F.Predeterminada).FirstOrDefault
                        If _FormaPago Is Nothing = False Then
                            Me.C_FormaPago.Value = _FormaPago.ID_FormaPago
                        End If
                    End If
                    If _Proveidor.DiaDePago.HasValue Then
                        Me.C_DiaPago.Value = _Proveidor.DiaDePago
                    End If
            End Select




        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub NetejarPantalla()
        oRowsSeleccionadas = New ArrayList
        Call CargarGrid(0)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.C_Empresa.Value = oEmpresa.ID_Empresa
        Me.C_Empresa.Enabled = True

    End Sub

    Private Sub CalcularTotals()
        Try
            Dim _IDsAlbarans As Integer
            Dim _TotalAlbarans As Decimal = 0
            Dim _TotalIva As Decimal = 0

            For Each _IDsAlbarans In oRowsSeleccionadas
                Dim oDTC As New DTCDataContext(BD.Conexion)
                Dim _Albara As Entrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = _IDsAlbarans).FirstOrDefault
                Dim _LineaAlbara As Entrada_Linea
                For Each _LineaAlbara In _Albara.Entrada_Linea
                    If _LineaAlbara.ID_Entrada_Factura.HasValue = False Then
                        Dim _TotalLinea As Decimal
                        _TotalLinea = _LineaAlbara.Unidad * _LineaAlbara.Precio - ((_LineaAlbara.Unidad * _LineaAlbara.Precio) * _LineaAlbara.Descuento1 / 100)
                        _TotalLinea = _TotalLinea - ((_TotalLinea) * _LineaAlbara.Descuento2 / 100)
                        _TotalAlbarans = _TotalAlbarans + _TotalLinea
                        _TotalIva = _TotalIva + (_TotalLinea * _LineaAlbara.IVA) / 100
                    End If
                Next
            Next

            Me.T_TotalBase.Value = _TotalAlbarans
            Me.T_TotalIVA.Value = _TotalIva
            Me.T_Total.Value = _TotalAlbarans + _TotalIva
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ToolForm_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Dim oDTC As New DTCDataContext(BD.Conexion)

        Select Case oEntradaTipo
            Case EnumTipoDeEntrada.Ventas
                If Me.T_NumeroFactura.Text.Length > 0 Then
                    If oDTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = EnumEntradaTipo.FacturaVenta And F.Codigo = Me.T_NumeroFactura.Text).Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible generar la factura, el número de factura introducido ya existe", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If
                End If
            Case EnumTipoDeEntrada.Compras
                If Me.T_NumeroFactura.Text.Length = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible generar la factura, el número de factura és obligatorio", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                    Exit Sub
                End If


        End Select


        If oRowsSeleccionadas.Count = 0 Then
            Mensaje.Mostrar_Mensaje("Imposible crear el albarán. Tiene que haber al menos una línea seleccionada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If


        'If Me.OP_TipusDeTraspas.Value = False AndAlso Me.TE_Codigo.Tag Is Nothing = True Then
        '    Mensaje.Mostrar_Mensaje("Imposible traspasar, es obligatorio introducir la factura para vincular las líneas seleccionadas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
        '    Exit Sub
        'End If

        Dim _EntradaPrimerAlbaraSeleccionat As Entrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = CInt(oRowsSeleccionadas(0))).FirstOrDefault
        Select Case oEntradaTipo
            Case EnumTipoDeEntrada.Ventas
                Dim _clsEntrada As New clsEntrada(oDTC, _EntradaPrimerAlbaraSeleccionat)
                Dim _IDNovaFactura As Integer = _clsEntrada.FacturarAlbaraVentaMultiple(oRowsSeleccionadas, Me.C_FormaPago.Value, Me.C_DiaPago.Value, IIf(Me.T_NumeroFactura.Text.Length = 0, 0, Me.T_NumeroFactura.Text), Me.C_Empresa.Value)
                If _IDNovaFactura <> 0 Then
                    Mensaje.Mostrar_Mensaje("Factura realizada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Me.FormTancar()
                    Dim frm As New frmEntrada
                    frm.Entrada(_IDNovaFactura, EnumEntradaTipo.FacturaVenta)
                    frm.FormObrir(Principal, False)  'posem el principal, pq com es destrueix el formulari traspas queda orfa
                End If

            Case EnumTipoDeEntrada.Compras
                If Me.T_NumeroFactura.Text Is Nothing = True OrElse Me.T_NumeroFactura.TextLength = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible traspasar, es obligatorio introducir el número de albarán del proveedor", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If


                Dim _clsEntrada As New clsEntrada(oDTC, _EntradaPrimerAlbaraSeleccionat)
                Dim _IDNovaFactura As Integer = _clsEntrada.FacturarAlbaraCompraMultiple(oRowsSeleccionadas, Me.C_FormaPago.Value, Me.C_DiaPago.Value, Me.T_NumeroFactura.Text, Me.C_Empresa.Value)
                If _IDNovaFactura <> 0 Then
                    Mensaje.Mostrar_Mensaje("Factura realizada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Me.FormTancar()
                    Dim frm As New frmEntrada
                    frm.Entrada(_IDNovaFactura, EnumEntradaTipo.FacturaCompra)
                    frm.FormObrir(Principal, False)  'posem el principal, pq com es destrueix el formulari traspas queda orfa
                End If


        End Select



    End Sub

    Private Sub TE_Codigo_ValueChanged(sender As Object, e As EventArgs) Handles TE_Codigo.ValueChanged

    End Sub
End Class