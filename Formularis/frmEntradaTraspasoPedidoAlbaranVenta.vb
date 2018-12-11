Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win

Public Class frmEntradaTraspasoPedidoAlbaranVenta
    Dim oclsEntrada As clsEntrada
    Dim oDTC As DTCDataContext
    Dim TaulaNSSeleccionats(10000, 1) As Integer
    Public Event AlTancarFormulariTraspas(ByVal pTraspasatCorrectament As Boolean)

    Private Sub ToolForm_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar


        'If Mensaje.Mostrar_Mensaje("¿Desea crear un nuevo albarán con las lineas seleccionadas?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then


        Dim _RowsSeleccionadas As New ArrayList

        For Each pRow In Me.GRD_Linea.GRID.Rows
            If pRow.Cells("CantidadATraspasar").Value > 0 Then
                _RowsSeleccionadas.Add(pRow)
            End If
        Next

        If Me.OP_TipusDeTraspas.Value = False AndAlso Me.TE_Codigo.Tag Is Nothing = True Then
            Mensaje.Mostrar_Mensaje("Imposible traspasar, es obligatorio introducir el albarán de compra para vincular las líneas seleccionadas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        Dim _clsEntrada As New clsEntrada(oDTC, oclsEntrada.oEntrada)
        Dim _IDNouAlbara As Integer = _clsEntrada.TraspasarDeComandaAAlbaraVenta(_RowsSeleccionadas, TaulaNSSeleccionats, IIf(Me.TE_Codigo.Tag Is Nothing, 0, Me.TE_Codigo.Tag))
        If _IDNouAlbara <> 0 Then
            RaiseEvent AlTancarFormulariTraspas(True)
            Mensaje.Mostrar_Mensaje("Traspaso realizado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
            Me.FormTancar()

            Dim frm As New frmEntrada
            frm.Entrada(_IDNouAlbara, EnumEntradaTipo.AlbaranVenta)
            frm.FormObrir(Principal, False)  'posem el principal, pq com es destrueix el formulari traspas queda orfa

            Exit Sub
        End If

        'Else
        'Exit Sub
        'End If
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Public Sub Entrada(ByRef pclsEntrada As clsEntrada, ByRef pDTC As DTCDataContext)
        Me.AplicarDisseny()
        oclsEntrada = pclsEntrada
        oDTC = pDTC

        Me.GRD_NS.M.clsToolBar.Boto_Afegir("SeleccionarNS", "Seleccionar Nº de serie", True)
        Me.GRD_Linea.M.clsToolBar.Boto_Afegir("VisualizarNS", "Visualizar números de serie", True)
        ' Me.GRD_NS.M.clsToolBar.Boto_Afegir("GenerarNSVirtuales", "Generar Números de serie virtuales", True)
        Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Traspasar"



        Call CargarGrid_Lineas()
        Me.GRD_NS.Enabled = False
        ' Call CargarGrid_NS(Nothing)
        ' Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        'Dim i As Integer = 0
        'For Each pRow In Me.GRD_Linea.GRID.Rows
        'TaulaNSSeleccionats(i, 0) = pRow.Cells("ID_Entrada_Linea").Value
        ' i = i + 1
        'Next
        Me.OP_TipusDeTraspas.Value = True

    End Sub

#Region "Grid Lineas"

    Private Sub CargarGrid_Lineas()
        Me.GRD_Linea.M.clsUltraGrid.Cargar(oclsEntrada.RetornaLineasTraspasPedidoAlbaranVenta)
        Me.GRD_Linea.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
        Me.GRD_Linea.GRID.DisplayLayout.Bands(0).Columns("CantidadATraspasar").CellClickAction = CellClickAction.EditAndSelectText
        Me.GRD_Linea.GRID.DisplayLayout.Bands(0).Columns("NoRestarStock").CellClickAction = CellClickAction.Edit
    End Sub

    Private Sub GRD_Linea_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Linea.M_GRID_ClickRow2
 

        If Me.GRD_Linea.GRID.ActiveCell Is Nothing = False AndAlso Me.GRD_Linea.GRID.ActiveCell.Column.Key = "NoRestarStock" Then
            Dim _Valor As Boolean = Me.GRD_Linea.GRID.ActiveCell.Value
            If _Valor = True Then
                Me.GRD_Linea.GRID.ActiveCell.Value = False
                e.Cells("CantidadATraspasar").Value = 0
            Else
                Me.GRD_Linea.GRID.ActiveCell.Value = True
                e.Cells("CantidadATraspasar").Activation = Activation.AllowEdit
            End If
        End If

    End Sub

    Private Sub GRD_Linea_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Linea.M_Grid_InitializeRow
        If e.Row.Cells("RequiereNS").Value = True Then
            If e.Row.Cells("NoRestarStock").Value = False Then  'Si esta la marca de NoRestarStock llavors no deshabilitarem la cantidad a traspasar
                e.Row.Cells("CantidadATraspasar").Activation = Activation.Disabled
            End If
            e.Row.Cells("NoRestarStock").Activation = Activation.NoEdit
        Else
            e.Row.Cells("CantidadATraspasar").Activation = Activation.AllowEdit
            e.Row.Cells("NoRestarStock").Activation = Activation.Disabled
        End If
    End Sub

    Private Sub GRD_Linea_M_GRID_BeforeCellUpdate(sender As Object, e As BeforeCellUpdateEventArgs) Handles GRD_Linea.M_GRID_BeforeCellUpdate
        If e.Cell.Column.Key = "CantidadATraspasar" Then
            If e.Cell.IsActiveCell = False Then 'Aquest if es pq això no s'apliqui la primera vegada k es carrega el grid
                Exit Sub
            End If
            If IsNumeric(e.NewValue) Then
                Dim QuantitatTraspasada As Decimal
                If IsDBNull(e.Cell.Row.Cells("CantidadTraspasada").Value) = True Then
                    QuantitatTraspasada = 0
                Else
                    QuantitatTraspasada = e.Cell.Row.Cells("CantidadTraspasada").Value
                End If
                If e.NewValue > e.Cell.Row.Cells("Cantidad").Value - QuantitatTraspasada Then
                    Mensaje.Mostrar_Mensaje("Imposible traspasar más cantidad de la que hay en el origen.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    e.Cancel = True
                    Exit Sub
                End If
            End If
        Else
            If e.Cell.Column.Key = "NoRestarStock" Then
                If e.Cell.Row.Cells("CantidadATraspasar").Value > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible marcar esta opción mientras hayan asignados números de serie no virtuales a esta línea de pedido", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
            End If
        End If
    End Sub

#End Region

#Region "Grid NS"
    Private Sub CargarGrid_NS(ByRef pLinea As Entrada_Linea)
        If GRD_Linea.GRID.ActiveRow Is Nothing Or pLinea Is Nothing Then
            Exit Sub
        End If


        Dim _clsEntradaLinea As New clsEntradaLinea(oclsEntrada.oEntrada, pLinea, oDTC)
        If _clsEntradaLinea.pRequiereNS = True Then 'Si requereix números de serie cargarem el grid
            Dim _DT As New DataTable
            _DT.Columns.Add("ID_NS", GetType(Integer))
            _DT.Columns.Add("Descripcion", GetType(String))
            Dim i As Integer
            'Bucle que va pels 1000 registres buscant tots els registres que siguin de la linea seleccionada, 
            For i = 0 To 1000
                If TaulaNSSeleccionats(i, 0) = pLinea.ID_Entrada_Linea Then
                    Dim _DTRow As DataRow
                    _DTRow = _DT.NewRow
                    _DTRow("ID_NS") = TaulaNSSeleccionats(i, 1)
                    Dim _NS As NS = oDTC.NS.Where(Function(F) F.ID_NS = TaulaNSSeleccionats(i, 1)).FirstOrDefault
                    _DTRow("Descripcion") = _NS.Descripcion
                    _DT.Rows.Add(_DTRow)
                End If
            Next

            Me.GRD_NS.M.clsUltraGrid.Cargar(_DT)

        Else
            Me.GRD_NS.GRID.DataSource = Nothing
        End If

        'Me.GRD_Linea.M.clsUltraGrid.Cargar(oclsEntradaLinea.RetornaNSDeLaLinea)

    End Sub

    Private Sub GRD_NS_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_NS.M_ToolGrid_ToolClickBotonsExtras
        If GRD_Linea.GRID.ActiveRow Is Nothing Then
            Exit Sub
        End If

        Select e.Tool.Key
            Case "SeleccionarNS"
                Dim _ID_Linea As Integer
                _ID_Linea = Me.GRD_Linea.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value
                Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = _ID_Linea).FirstOrDefault
                Dim oclsEntradaLinea As New clsEntradaLinea(oclsEntrada.oEntrada, _Linea, oDTC)
                'If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea = 0 Then
                'If Guardar() = False Then
                '    Mensaje.Mostrar_Mensaje("Los datos de la linea no son correctos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                '    Exit Sub
                'End If
                'End If

                Dim frm As New frmEntrada_SeleccionNS
                frm.Entrada(oclsEntradaLinea, oDTC, oclsEntradaLinea.oLinqLinea.ID_Almacen, frmEntrada_SeleccionNS.EnumTipusEntradaAlFormulari.DesdeTraspasDeComandaAAlbaraDeVenta, RetornaArrayListAmbElsNumerosDeSerieDeUnaLinea(_ID_Linea))
                If frm.HiHaNumerosDeSerieDisponibles = True Then
                    frm.FormObrir(Me, True)
                Else
                    Mensaje.Mostrar_Mensaje("No hay números de serie disponibles", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    frm.FormTancar()
                End If

                AddHandler frm.AlTancarFormulariSeleccioNS, AddressOf AlTancarFormulariSeleccioNS


        End Select
    End Sub

    Private Sub AlTancarFormulariSeleccioNS(ByVal pRecarregarDades As Boolean, ByRef pNSSeleccionats As ArrayList)
        If pRecarregarDades = True Then

            'Blanquejem els registres de la taula que tenen el id linea seleccionat. Perque després la tornarem a reomplir amb les dades bones
            Dim i As Integer = 0
            Dim _IDLinea As Integer = Me.GRD_Linea.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value
            For i = 0 To 10000
                If _IDLinea = TaulaNSSeleccionats(i, 0) Then
                    TaulaNSSeleccionats(i, 0) = 0
                    TaulaNSSeleccionats(i, 1) = 0
                End If
            Next

            Dim _IDNS As Integer
            For Each _IDNS In pNSSeleccionats
                For i = 0 To 10000
                    If TaulaNSSeleccionats(i, 0) = 0 Then  'Si no hi ha re ho podrem omplir
                        TaulaNSSeleccionats(i, 0) = _IDLinea
                        TaulaNSSeleccionats(i, 1) = _IDNS
                        Exit For
                    End If
                Next
            Next

            Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = _IDLinea).FirstOrDefault
            Call CargarGrid_NS(_Linea)

            'Canviar les quantitats a Traspasar de les linies
            Dim pRow As UltraGridRow
            For Each pRow In Me.GRD_Linea.GRID.Rows
                If pRow.Cells("RequiereNS").Value = True Then
                    pRow.Cells("CantidadATraspasar").Value = RetornaQuantsNSHiHaSelecciontasEnUnaLinea(pRow.Cells("ID_Entrada_Linea").Value)
                End If
            Next
        End If
    End Sub

    Private Function RetornaQuantsNSHiHaSelecciontasEnUnaLinea(ByVal pIDLinea As Integer) As Integer
        Dim i As Integer
        Dim ContaNS As Integer = 0
        For i = 0 To 10000
            If TaulaNSSeleccionats(i, 0) = pIDLinea Then
                ContaNS = ContaNS + 1
            End If
        Next
        Return ContaNS
    End Function

    Private Function RetornaArrayListAmbElsNumerosDeSerieDeUnaLinea(ByVal pIDLinea As Integer) As ArrayList
        Dim _ArrayListNSSeleccionats As New ArrayList
        Dim i As Integer
        For i = 0 To 10000
            If TaulaNSSeleccionats(i, 0) = pIDLinea Then
                _ArrayListNSSeleccionats.Add(TaulaNSSeleccionats(i, 1))
            End If
        Next
        Return _ArrayListNSSeleccionats
    End Function
#End Region

#Region "Events varis"
    Private Sub TE_Codigo_EditorButtonClick(sender As Object, e As UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Entrada Where  ID_Entrada_Tipo=" & EnumEntradaTipo.AlbaranVenta & " and ID_Cliente=" & oclsEntrada.oEntrada.ID_Cliente & "  ORDER BY FechaEntrada", Me.TE_Codigo, "ID_Entrada", "Codigo", Me.T_Descripcion, "Descripcion")
            'AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat
        End If
    End Sub

    Private Sub OP_TipusDeTraspas_ValueChanged(sender As Object, e As EventArgs) Handles OP_TipusDeTraspas.ValueChanged
        If OP_TipusDeTraspas.Value = True Then
            Me.T_Descripcion.Text = ""
            Me.TE_Codigo.Value = Nothing
            Me.TE_Codigo.Tag = Nothing
            Me.TE_Codigo.Enabled = False
        Else
            Me.TE_Codigo.Enabled = True
        End If
    End Sub
#End Region

    Private Sub GRD_Linea_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Linea.M_ToolGrid_ToolClickBotonsExtras2
        If e.Tool.Key = "VisualizarNS" Then
            If Me.GRD_Linea.GRID.ActiveRow Is Nothing Then
                Exit Sub
            End If
            Dim _IDLinea As Integer
            _IDLinea = Me.GRD_Linea.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value
            Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = _IDLinea).FirstOrDefault

            If _Linea.Producto.RequiereNumeroSerie = True Then
                Call CargarGrid_NS(_Linea)
                Me.GRD_NS.Enabled = True
            Else
                Me.GRD_NS.Enabled = False
            End If

        End If
    End Sub


    Private Sub GRD_Linea_Load(sender As Object, e As EventArgs) Handles GRD_Linea.Load

    End Sub
End Class