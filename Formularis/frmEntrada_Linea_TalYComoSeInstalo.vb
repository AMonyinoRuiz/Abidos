Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEntrada_Linea_TalYComoSeInstalo
    Dim oDTC As DTCDataContext
    Dim oclsEntradaLinea As clsEntradaLinea
    Dim RowsSeleccionadas As New ArrayList



#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Try

            Util.WaitFormObrir()
            Call Assignar()
            Util.WaitFormTancar()

            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)


            Call M_ToolForm1_m_ToolForm_Sortir()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pclsEntradaLinea As clsEntradaLinea, ByRef pDTC As DTCDataContext)

        Try

            Me.AplicarDisseny()

            oclsEntradaLinea = pclsEntradaLinea

            oDTC = pDTC
            'oDTC = New DTCDataContext(BD.Conexion)
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Asignar"

            Call CargarCombo_Instalacion(Me.C_Instalacion)

            'Cargar en rowsseleccionadas els id propuesta ja assignats
            Dim _Relacio As Entrada_Linea_Propuesta_Linea
            For Each _Relacio In oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea
                RowsSeleccionadas.Add(_Relacio.ID_Propuesta_Linea)
            Next


            Me.GRD_Lineas.M.clsToolBar.Boto_Afegir("SeleccionarTodo", "Seleccionar todos", True)


            'Call CargaGrid_Lineas(oLinqPropuesta.ID_Propuesta)

            ' Me.GRD_Lineas.GRID.ActiveRow = Nothing
            ' Me.GRD_Lineas.GRID.Selected.Rows.Clear()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Private Sub Assignar()


        Dim _IDLineaPropuesta As Integer
        'Aquest bucle es per no traspasar les línies que no estan seleccionades

        oDTC.Entrada_Linea_Propuesta_Linea.DeleteAllOnSubmit(oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea)

        For Each _IDLineaPropuesta In RowsSeleccionadas
            'If oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(pRow.Cells("ID_Propuesta_Linea").Value)).Count = 0 Then
            Dim _Relacio As New Entrada_Linea_Propuesta_Linea
            _Relacio.Entrada_Linea = oclsEntradaLinea.oLinqLinea
            _Relacio.Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _IDLineaPropuesta).FirstOrDefault
            oDTC.Entrada_Linea_Propuesta_Linea.InsertOnSubmit(_Relacio)
            'End If
        Next



        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Lineas"

    Public Sub CargaGrid_Lineas(ByVal pIDInstalacion As Integer)
        Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Instalacion = pIDInstalacion And F.SeInstalo = True And F.Activo = True).FirstOrDefault

        If _Propuesta Is Nothing Then
            Mensaje.Mostrar_Mensaje("Esta instalación no tiene un 'Tal y como se instaló'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Me.GRD_Lineas.GRID.DataSource = Nothing
            Exit Sub
        End If


        Dim _ID_Propuesta As Integer = _Propuesta.ID_Propuesta

        Dim DTS As New DataSet
        BD.CargarDataSet(DTS, "Select *, cast(0 as bit) as Seleccion From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & _ID_Propuesta & " and ID_Propuesta_Linea Not In (Select ID_Propuesta_Linea From Entrada_Linea_Propuesta_Linea Where ID_Entrada_Linea<>" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea & ")")
        Me.GRD_Lineas.M.clsUltraGrid.Cargar(DTS)
        Me.GRD_Lineas.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True

        Dim pRow As UltraGridRow
        For Each pRow In Me.GRD_Lineas.GRID.Rows
            If RowsSeleccionadas.Contains(pRow.Cells("ID_Propuesta_Linea").Value) Then
                ' pRow.Cells("Seleccion").Value = True
                'pRow.CellAppearance.BackColor = Color.LightGreen
            End If
            '
            'pRow.CellAppearance.BackColor = Color.White
            'RowsSeleccionadas.Add(pRow)
            pRow.Update()
        Next

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

        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_Lineas.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
        e.Row.Cells("Seleccion").Column.CellClickAction = CellClickAction.CellSelect
        e.Row.Cells("Seleccion").Column.CellActivation = Activation.AllowEdit

        ' Dim _Linea As Propuesta_Linea
        '_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
        '  If oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).Count = 1 Then
        'If _Linea.Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True And F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea).Count > 0 Then
        If RowsSeleccionadas.Contains(e.Row.Cells("ID_Propuesta_Linea").Value) Then
            e.Row.Cells("Seleccion").Value = True
            e.Row.CellAppearance.BackColor = Color.LightGreen
        Else
            e.Row.CellAppearance.BackColor = Color.White
            '    e.Row.Cells("Seleccion").Activation = Activation.Disabled
        End If

    End Sub

    Private Sub GRD_Lineas_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Lineas.M_GRID_AfterCellActivate
        With GRD_Lineas.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                'Dim _Linea As Propuesta_Linea = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(.ActiveRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault()
                If .ActiveCell.Value = True Then
                    RowsSeleccionadas.Remove(.ActiveCell.Row.Cells("ID_Propuesta_Linea").Value)
                    .ActiveCell.Value = False
                Else
                    RowsSeleccionadas.Add(.ActiveCell.Row.Cells("ID_Propuesta_Linea").Value)
                    .ActiveCell.Value = True
                End If
                oDTC.SubmitChanges()
            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
        End With
    End Sub

    Private Sub GRD_Lineas_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Lineas.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "SeleccionarTodo" Then
            Dim pRow As UltraGridRow
            For Each pRow In Me.GRD_Lineas.GRID.Rows
                If pRow.Cells("Seleccion").Value = False Then
                    pRow.Cells("Seleccion").Value = True
                    RowsSeleccionadas.Add(pRow.Cells("ID_Propuesta_Linea").Value)
                    pRow.Update()
                End If
            Next
        End If
    End Sub
#End Region

    Private Sub CargarCombo_Instalacion(ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo)
        Try

            Dim _LlistatInstalacionsSeleccionades As IEnumerable(Of Instalacion) = From Taula In oDTC.Entrada_Instalacion Where Taula.ID_Entrada = oclsEntradaLinea.oLinqEntrada.ID_Entrada Select Taula.Instalacion

            Dim _LlistatInstalacions As IEnumerable

            '_LlistatInstalacions = From Taula In oDTC.Entrada_InstalacionPropuesta Order By Taula.ID_Instalacion Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.Instalacion.OtrosDetalles, Taula.ID_Instalacion, Taula.Instalacion.OtrosDetalles, Taula.Instalacion.FechaInstalacion, Cliente = Taula.Instalacion.Cliente.NombreComercial, Poblacion = Taula.Instalacion.Poblacion

            _LlistatInstalacions = From Taula In oDTC.Entrada_Instalacion Where _LlistatInstalacionsSeleccionades.Contains(Taula.Instalacion) Order By Taula.ID_Instalacion Descending Group By ID_Instalacion = Taula.ID_Instalacion, Visualitzar = Taula.ID_Instalacion & " - " & Taula.Instalacion.OtrosDetalles, OtrosDetalles = Taula.Instalacion.OtrosDetalles, Poblacion = Taula.Instalacion.Poblacion Into Group Select ID_Instalacion, Visualitzar, OtrosDetalles, Poblacion




            pCombo.DataSource = _LlistatInstalacions

            If _LlistatInstalacions Is Nothing Then
                Exit Sub
            End If

            With pCombo
                .AutoCompleteMode = AutoCompleteMode.None
                .AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
                .AlwaysInEditMode = False
                .DropDownStyle = UltraComboStyle.DropDownList

                .MaxDropDownItems = 16
                .DisplayMember = "Visualitzar"
                .ValueMember = "ID_Instalacion"
                .DisplayLayout.Bands(0).Columns("ID_Instalacion").Width = 50
                .DisplayLayout.Bands(0).Columns("ID_Instalacion").Header.Caption = "Código"
                .DisplayLayout.Bands(0).Columns("ID_Instalacion").Header.Appearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("OtrosDetalles").Width = 200
                .DisplayLayout.Bands(0).Columns("OtrosDetalles").Header.Caption = "Descripción"
                '.DisplayLayout.Bands(0).Columns("FechaInstalacion").Width = 75
                '.DisplayLayout.Bands(0).Columns("FechaInstalacion").Header.Caption = "Fecha"
                '.DisplayLayout.Bands(0).Columns("Cliente").Width = 300
                '.DisplayLayout.Bands(0).Columns("Cliente").Header.Caption = "Cliente"
                .DisplayLayout.Bands(0).Columns("Poblacion").Width = 200
                .DisplayLayout.Bands(0).Columns("Poblacion").Header.Caption = "Población"
                .DisplayLayout.Bands(0).Columns("Visualitzar").Hidden = True
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub C_Instalacion_RowSelected(sender As Object, e As RowSelectedEventArgs) Handles C_Instalacion.RowSelected
        Call CargaGrid_Lineas(Me.C_Instalacion.Value)
    End Sub


End Class