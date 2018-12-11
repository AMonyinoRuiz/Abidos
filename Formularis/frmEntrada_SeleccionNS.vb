Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEntrada_SeleccionNS
    Dim oDTC As DTCDataContext
    Dim oclsEntradaLinea As clsEntradaLinea
    Dim oNSSeleccionadas As New ArrayList
    Public Event AlTancarFormulariSeleccioNS(ByVal pTraspasCorrecte As Boolean, ByRef pNSSeleccionadas As ArrayList)
    Enum EnumTipusEntradaAlFormulari As Integer
        Normal = 1
        DesdeTraspasDeComandaAAlbaraDeVenta = 2
    End Enum
    Dim oTipusEntradaAlFormulari As EnumTipusEntradaAlFormulari
    Dim oMaximNumeroDeNSaSeleccionar As Integer


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.AlbaranVenta And oclsEntradaLinea.oLinqLinea.ID_Entrada_Factura.HasValue Then 'Si es un albara de venta i ja està facturada la línea llavors els números de serie que es seleccionin hauran de ser els mateixos que els de la linea
            If oclsEntradaLinea.oLinqLinea.Unidad <> oNSSeleccionadas.Count Then
                Mensaje.Mostrar_Mensaje("Imposible asignar un número de 'números de serie' diferente al que contiene la línea de albarán", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If
        End If

        If oTipusEntradaAlFormulari = EnumTipusEntradaAlFormulari.Normal Then 'Si es normal ho guardarem a la base de dades si no simplement passarem el arraylist en l'event
            Dim _LineaNS As Entrada_Linea_NS

            'Bucle per eliminar les liniesNS que ja estaven assignades abans d'entrar a aquesta pantalla i ara ja no ho estan. Per tant les que han deseleccionat i abans hi eren.
            For Each _LineaNS In oclsEntradaLinea.oLinqLinea.Entrada_Linea_NS
                If oNSSeleccionadas.Contains(CInt(_LineaNS.ID_NS)) = False Then
                    oclsEntradaLinea.LineaNSEliminar(_LineaNS)
                End If
            Next

            Dim _NS As Integer
            For Each _NS In oNSSeleccionadas
                If oclsEntradaLinea.oLinqLinea.Entrada_Linea_NS.Where(Function(F) F.ID_NS = _NS).Count = 0 Then
                    oclsEntradaLinea.LineaNSCrear(_NS)
                End If
            Next

            oDTC.SubmitChanges()
        End If

        If oTipusEntradaAlFormulari = EnumTipusEntradaAlFormulari.DesdeTraspasDeComandaAAlbaraDeVenta And oNSSeleccionadas.Count > oclsEntradaLinea.oLinqLinea.Unidad Then
            Mensaje.Mostrar_Mensaje("Imposible seleccionar más números de serie que la cantidad introducida en la línea del pedido", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If




        RaiseEvent AlTancarFormulariSeleccioNS(True, oNSSeleccionadas)

        Me.FormTancar()

        'Dim _clsEntrada As New clsEntrada(oDTC, oLinqEntrada)
        'Dim _IDNouAlbara As Integer = _clsEntrada.TraspasarDeComandaAAlbara(oRowsSeleccionadas, oNSSeleccionadas)
        'If _IDNouAlbara <> 0 Then
        '    RaiseEvent AlTancarFormulariTraspas(True)
        '    Mensaje.Mostrar_Mensaje("Traspaso realizado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
        '    Me.FormTancar()

        '    Dim frm As New frmEntrada
        '    frm.Entrada(_IDNouAlbara, EnumEntradaTipo.AlbaranCompra)
        '    frm.FormObrir(Principal, False)  'posem el principal, pq com es destrueix el formulari traspas queda orfa

        '    Exit Sub
        'End If


    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        RaiseEvent AlTancarFormulariSeleccioNS(False, Nothing)
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pEntradaLinea As clsEntradaLinea, ByRef pDTC As DTCDataContext, ByVal pIDAlmacen As Integer, Optional ByVal pTipusEntradaAlFormulari As EnumTipusEntradaAlFormulari = EnumTipusEntradaAlFormulari.Normal, Optional ByVal pArrayListNS As ArrayList = Nothing)

        Try

            Me.AplicarDisseny()

            oclsEntradaLinea = pEntradaLinea

            oTipusEntradaAlFormulari = pTipusEntradaAlFormulari
            'Només en els traspasos visualitzarem la quantitat màxima a traspasar de números de serie
            If oTipusEntradaAlFormulari = EnumTipusEntradaAlFormulari.DesdeTraspasDeComandaAAlbaraDeVenta Then
                Me.L_NS.Visible = True
                oMaximNumeroDeNSaSeleccionar = oclsEntradaLinea.oLinqLinea.Unidad - Util.Comprobar_NULL_Per_0_Decimal(oclsEntradaLinea.oLinqLinea.CantidadTraspasada)
            Else
                Me.L_NS.Visible = False

            End If

            oDTC = pDTC

            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Asignar"

            Util.Cargar_Combo(Me.C_Almacen, "Select ID_Almacen, Descripcion From Almacen Where Activo=1 Order By Descripcion", False, False)
            Me.C_Almacen.Value = pIDAlmacen


            Call CargaGrid_LineasNS()


            If pArrayListNS Is Nothing = False Then 'Si ens han passat una llistat de números de serie per a que els seleccionem, ho farem
                oNSSeleccionadas = pArrayListNS
                For Each pRow In Me.GRD_Lineas.GRID.Rows
                    If oNSSeleccionadas.Contains(pRow.Cells("ID_NS").Value) Then
                        pRow.Cells("Seleccion").Value = True
                        'oNSSeleccionadas.Add(pRow.Cells("ID_NS").Value)
                        pRow.Update()
                    End If
                Next
            End If

            Call ActualitzarContador()
            ' Me.GRD_Lineas.GRID.ActiveRow = Nothing
            ' Me.GRD_Lineas.GRID.Selected.Rows.Clear()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub ActualitzarContador()
        'Només en els traspasos visualitzarem la quantitat màxima a traspasar de números de serie
        Me.L_NS.Text = "Nº de serie: (" & oNSSeleccionadas.Count & " / " & oMaximNumeroDeNSaSeleccionar & ")"
        If oNSSeleccionadas.Count > oMaximNumeroDeNSaSeleccionar Then
            Me.L_NS.Appearance.ForeColor = Color.Red
        Else
            Me.L_NS.Appearance.ForeColor = Color.Black
        End If

    End Sub

#End Region

#Region "Grid Lineas"

    Public Sub CargaGrid_LineasNS()
        ' Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        'If _Propuesta Is Nothing = False Then




        Dim DT As New DataTable
        DT = RetornaNumerosDeSerieDisponibles(oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea, oclsEntradaLinea.oLinqLinea.ID_Producto, Me.C_Almacen.Value)

        If DT Is Nothing Then
            'El missatge ja l'ensenyem abans
            'Mensaje.Mostrar_Mensaje("No hay números de serie disponibles para este artículo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        Me.GRD_Lineas.M.clsUltraGrid.Cargar(DT)
        Me.GRD_Lineas.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True

        Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellClickAction = CellClickAction.CellSelect
        Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellActivation = Activation.AllowEdit

        Dim pRow As UltraGridRow

        Select Case oTipusEntradaAlFormulari
            Case EnumTipusEntradaAlFormulari.Normal

                For Each pRow In Me.GRD_Lineas.GRID.Rows
                    If oclsEntradaLinea.oLinqLinea.Entrada_Linea_NS.Where(Function(F) F.ID_NS = CInt(pRow.Cells("ID_NS").Value)).Count = 1 Then
                        pRow.Cells("Seleccion").Value = True
                        'pRow.Cells("Seleccion").Activation = Activation.Disabled
                        oNSSeleccionadas.Add(pRow.Cells("ID_NS").Value)
                        pRow.Update()
                    End If
                Next
            Case EnumTipusEntradaAlFormulari.DesdeTraspasDeComandaAAlbaraDeVenta


        End Select

    End Sub

    Private Sub GRD_Lineas_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Lineas.M_Grid_InitializeRow
        'If e.Row.Cells("ID_Producto_Subfamilia_Traspaso").Value = 0 Then
        '    e.Row.Cells("Seleccion").Value = False
        '    e.Row.CellAppearance.BackColor = Color.LightCoral
        'Else
        '    e.Row.Cells("Seleccion").Value = True
        '    e.Row.CellAppearance.BackColor = Color.White
        '    e.Row.Update()
        '    RowsSeleccionadas.Add(e.Row)
        'End If

        'If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
        '    Me.GRD_Lineas.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        'End If
        If e.Row.Cells("Seleccion").Value = True Then
            e.Row.CellAppearance.BackColor = Color.LightGreen
        Else
            e.Row.CellAppearance.BackColor = Color.LightCoral
        End If

        Call ControlSeleccio(Me.GRD_Lineas.GRID.ActiveCell, e.Row)
        Call ActualitzarContador()

        'If e.Row.Band.Index = 0 Then
        '    If e.Row.Cells("NumNS").Value > 0 Then
        '        e.Row.Cells("CantidadATraspasar").Activation = Activation.Disabled
        '        e.Row.Cells("Seleccion").Activation = Activation.Disabled
        '        '  e.Row.Cells("CantidadATraspasar").Activation = Activation.AllowEdit
        '    Else
        '        e.Row.Cells("CantidadATraspasar").Activation = Activation.AllowEdit
        '        e.Row.Cells("Seleccion").Activation = Activation.AllowEdit

        '        '  e.Row.Cells("CantidadATraspasar").Activation = Activation.NoEdit
        '        'e.Row.Cells("CantidadATraspasar").ClickAction = CellClickAction.CellSelect
        '    End If

        'End If


        'Dim _Linea As Propuesta_Linea
        '_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
        'If _Linea.Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True And F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea).Count > 0 Then
        '    e.Row.Cells("Seleccion").Value = True
        '    e.Row.Cells("Seleccion").Activation = Activation.Disabled
        'End If


    End Sub

    Private Sub GRD_Lineas_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Lineas.M_GRID_AfterCellActivate
        With GRD_Lineas.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                .ActiveCell.Value = Not .ActiveCell.Value
                .ActiveCell.Row.Update()
                .ActiveRow = Nothing
                .Selected.Rows.Clear()
                .ActiveCell = Nothing
            End If

            'If .ActiveRow.ParentRow Is Nothing = False Then
            '    .ActiveRow.ParentRow.Update()
            'End If


        End With
    End Sub

    Private Sub ControlSeleccio(ByRef pActiveCell As UltraGridCell, ByRef pActiveRow As UltraGridRow)
        If pActiveRow.Cells("Seleccion").Value = True Then

            If oNSSeleccionadas.Contains(pActiveRow.Cells("ID_NS").Value) = False Then
                oNSSeleccionadas.Add(pActiveRow.Cells("ID_NS").Value)
            End If

        Else
            If oNSSeleccionadas.Contains(pActiveRow.Cells("ID_NS").Value) Then
                oNSSeleccionadas.Remove(pActiveRow.Cells("ID_NS").Value)
            End If
        End If
    End Sub

    Private Function RetornaNumerosDeSerieDisponibles(ByVal pIDEntradaLinea As Integer, ByVal pIDProducto As Integer, ByVal pIDAlmacen As Integer, Optional ByVal pIDCliente As Integer = 0, Optional ByVal pIDProveedor As Integer = 0) As DataTable
        Dim _IDProveedor As Integer = 0
        Dim _IDClient As Integer = 0
        Dim _STRMagatzem As String = ""

        If oclsEntradaLinea.oLinqEntrada.ID_Entrada_Tipo <> EnumEntradaTipo.DevolucionCompra And oclsEntradaLinea.oLinqEntrada.ID_Entrada_Tipo <> EnumEntradaTipo.DevolucionVenta Then
            _STRMagatzem = " and ID_Almacen=" & pIDAlmacen
        End If

        If oclsEntradaLinea.oLinqEntrada.ID_Entrada_Tipo = EnumEntradaTipo.DevolucionCompra Then
            _IDProveedor = oclsEntradaLinea.oLinqEntrada.ID_Proveedor
        End If

        If oclsEntradaLinea.oLinqEntrada.ID_Entrada_Tipo = EnumEntradaTipo.DevolucionVenta Then
            _IDClient = oclsEntradaLinea.oLinqEntrada.ID_Cliente
        End If

        Dim _STRCliente As String = ""
        Dim _STRProveedor As String = ""
        Dim _STRDisponible As String = "1"
        If _IDClient <> 0 Then
            _STRCliente = " and ID_Cliente=" & _IDClient
            _STRDisponible = "2" 'El posem a 2 pq les devolucions de venta els articles no estan disponibles
        End If
        If _IDProveedor <> 0 Then
            _STRProveedor = " and ID_Proveedor=" & _IDProveedor
        End If

        Return BD.RetornaDataTable("Select *, cast(0 as bit) as Seleccion From C_Entrada_Seleccion_NS Where (ID_NS_Estado=" & _STRDisponible & " or ID_NS in (Select ID_NS From Entrada_Linea_NS, Entrada_Linea Where Entrada_Linea_NS.ID_Entrada_Linea=Entrada_Linea.ID_Entrada_Linea and Entrada_Linea.ID_Entrada_Linea=" & pIDEntradaLinea & ")) and ID_Producto=" & pIDProducto & _strMagatzem & _STRCliente & _STRProveedor)
    End Function

    Public Function HiHaNumerosDeSerieDisponibles() As Boolean
        If oNSSeleccionadas.Count > 0 Then
            Return True 'Si hi han seleccionades es que hi ha stock (si més no per treure'n)
        Else
            If RetornaNumerosDeSerieDisponibles(oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea, oclsEntradaLinea.oLinqLinea.ID_Producto, Me.C_Almacen.Value) Is Nothing Then
                Return False
            Else
                Return True
            End If
        End If

    End Function

#End Region

End Class