Imports Infragistics.Win.UltraWinGrid

Public Class frmListado_ContratosPendientes
    Dim oDTC As DTCDataContext
    Dim RowsSeleccionadas As New ArrayList
    Public Sub Entrada()
        Me.AplicarDisseny()

        Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Visible = False

        Me.ToolForm.M.clsToolBar.Afegir_Boto("Facturar", "Facturar líneas seleccionadas", True)

        oDTC = New DTCDataContext(BD.Conexion)

        Call CarregarDades()

    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub GRD_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD.M_GRID_DoubleClickRow
        'If Me.GRD.GRID.Selected.Rows.Count <> 1 Then
        '    Exit Sub
        'End If

        'Dim ID As Integer
        'ID = Me.GRD.GRID.Selected.Rows(0).Cells("ID_Instalacion").Value

        'Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
        'frm.Entrada(ID)
        'frm.FormObrir(Me, True)
    End Sub

    Private Sub GRD_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD.M_GRID_AfterCellActivate
        With GRD.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                If .ActiveCell.Value = True Then
                    RowsSeleccionadas.Remove(.ActiveCell.Row.Cells("ID_Instalacion_Contrato").Value)
                    .ActiveCell.Value = False
                Else
                    RowsSeleccionadas.Add(.ActiveCell.Row.Cells("ID_Instalacion_Contrato").Value)
                    .ActiveCell.Value = True
                End If
                oDTC.SubmitChanges()
            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
        End With
    End Sub

    Private Sub GRD_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD.M_Grid_InitializeRow


        e.Row.Cells("Seleccion").Column.CellClickAction = CellClickAction.CellSelect
        e.Row.Cells("Seleccion").Column.CellActivation = Activation.AllowEdit

        'If RowsSeleccionadasHoras.Contains(e.Row.Cells("ID_Parte_Horas").Value) Then
        '    e.Row.Cells("Seleccion").Value = True
        '    '    e.Row.Cells("Seleccion").Activation = Activation.Disabled
        'End If

    End Sub

    Private Sub CarregarDades()
        Try

            With Me.GRD

                'Tots els contractes que no estiguin facturats (Tots els contractes que no estiguin en cap línea d'albara facturada
                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select  cast(0 as bit) as Seleccion, * From C_Instalacion_Contrato Where ID_Instalacion_Contrato Not IN (Select ID_Instalacion_Contrato From Entrada_Linea, Entrada Where Entrada_Linea.ID_Entrada=Entrada.ID_Entrada and ID_Entrada_Tipo=" & EnumEntradaTipo.AlbaranVenta & " and ID_Entrada_Factura is not null and ID_Instalacion_Contrato is not null) ", True))
                .M_NoEditable()

                RowsSeleccionadas.Clear() 'Cada cop que recarreguem el grid eliminarem tot lo seleccionat

            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ToolForm_m_ToolForm_ToolClick_Botones_Extras(Sender As Object, e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        Select Case e.Tool.Key
            Case "Facturar"
                If RowsSeleccionadas.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("No hay ningún contrato", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                oDTC = New DTCDataContext(BD.Conexion)
                Dim i As Integer
                For i = 0 To RowsSeleccionadas.Count - 1
                    Dim _Contrato As Instalacion_Contrato = oDTC.Instalacion_Contrato.Where(Function(F) F.ID_Instalacion_Contrato = CInt(RowsSeleccionadas(i))).FirstOrDefault
                    Dim _Factura As Entrada
                    Dim _clsEntrada As New clsEntrada(oDTC)
                    _Factura = _clsEntrada.CrearAlbaraDeVentaApartirDeContrato(RowsSeleccionadas(i))

                    Dim _LineaContrato As Instalacion_Contrato_Producto
                    For Each _LineaContrato In _Contrato.Instalacion_Contrato_Producto
                        _Factura.Entrada_Linea.Add(_clsEntrada.CrearLineaAlbaraDeVentaApartirDeLineaContrato(_LineaContrato.ID_Instalacion_Contrato_Producto))
                    Next



                    oDTC.SubmitChanges()

                    _Factura.Base = _Factura.RetornaImporteBase
                    _Factura.IVA = _Factura.RetornaImporteIVA
                    _Factura.Total = _Factura.RetornaImporteTotal

                    oDTC.SubmitChanges()


                    _clsEntrada = New clsEntrada(oDTC, _Factura)

                    'Fem això pq la funció de traspasar de albarà a factura demana un arraylist de id's de línea d'albarà
                    Dim _LiniesEntrada As New ArrayList
                    _LiniesEntrada.Add(_Factura.ID_Entrada)
                    'Dim _LineaEntrada As Entrada_Linea
                    'For Each _LineaEntrada In _Factura.Entrada_Linea
                    '    _LiniesEntrada.Add(_LineaEntrada.ID_Entrada_Linea)
                    'Next

                    _clsEntrada.FacturarAlbaraVentaMultiple(_LiniesEntrada, _Factura.ID_FormaPago, _Factura.DiaDePago)
                Next

                Call CarregarDades()
                Mensaje.Mostrar_Mensaje("Contratos facturados correctamente", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)
        End Select
    End Sub
End Class