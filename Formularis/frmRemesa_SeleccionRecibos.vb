Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmRemesa_SeleccionRecibos
    Dim oDTC As DTCDataContext
    Dim olinqRemesa As Remesa
    Dim oVencimentsSeleccionats As New ArrayList
    Public Event AlTancarFormulariSeleccio(ByVal pCorrecte As Boolean, ByRef pVencimentsSeleccionats As ArrayList)


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar



        Dim _Venciment As Entrada_Vencimiento


        Dim _IDVenciment As Integer
        For Each _IDVenciment In oVencimentsSeleccionats
            '  If olinqRemesa.Entrada_Vencimiento.Where(Function(F) F.ID_NS = _IDVenciment).Count = 0 Then
            _Venciment = oDTC.Entrada_Vencimiento.Where(Function(F) F.ID_Entrada_Vencimiento = _IDVenciment).FirstOrDefault
            _Venciment.Entrada_Vencimiento_Estado = oDTC.Entrada_Vencimiento_Estado.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.PendienteGenerarArchivo).FirstOrDefault
            olinqRemesa.Entrada_Vencimiento.Add(_Venciment)
            'End If
        Next

        oDTC.SubmitChanges()



        RaiseEvent AlTancarFormulariSeleccio(True, oVencimentsSeleccionats)

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
        RaiseEvent AlTancarFormulariSeleccio(False, Nothing)
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pRemesa As Remesa, ByRef pDTC As DTCDataContext)

        Try

            Me.AplicarDisseny()

            olinqRemesa = pRemesa

            oDTC = pDTC

            If olinqRemesa.Entrada_Vencimiento.Count = 0 Then  'si no hi han rebuts en la remesa llavors carregarem tots els rebuts que hi ha i si ni han llavors agafarem la fecha del primer rebut i filtrarem per la data
                Me.DT_Remesa.Value = Now.Date
                Call CargaGrid_VencimentsPendentsAssignar(True)
            Else
                Dim _Rebut As Entrada_Vencimiento = olinqRemesa.Entrada_Vencimiento.FirstOrDefault
                Me.DT_Remesa.Value = _Rebut.Fecha
                Call CargaGrid_VencimentsPendentsAssignar(False)
            End If





            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Asignar"

            ' Util.Cargar_Combo(Me.C_Almacen, "Select ID_Almacen, Descripcion From Almacen Where Activo=1 Order By Descripcion", False, False)



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

#End Region

#Region "Grid Lineas"

    Public Sub CargaGrid_VencimentsPendentsAssignar(ByVal pVisualitzarTot As Boolean)
        ' Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        'If _Propuesta Is Nothing = False Then

        Dim _SqlFecha As String
        If pVisualitzarTot = True Then
            _SqlFecha = ""
        Else
            _SqlFecha = " and Fecha = '" & Me.DT_Remesa.Value & "'"
        End If


        Dim DT As New DataTable
        DT = BD.RetornaDataTable("Select *, cast(0 as bit) as Seleccion From C_Remesa_Vencimientos Where  ID_Entrada_Vencimiento_Estado=" & EnumVencimientoEstado.PendienteAsignarARemesa & " and ID_Empresa_CuentaBancaria=" & olinqRemesa.ID_Empresa_CuentaBancaria & _SqlFecha & " Order by Fecha", True)

        'If DT Is Nothing Then
        '    'El missatge ja l'ensenyem abans
        '    Exit Sub
        'End If

        Me.GRD_Lineas.M.clsUltraGrid.Cargar(DT)

        If pVisualitzarTot = True Then
            Me.GRD_Lineas.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False
            Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellActivation = Activation.Disabled
        Else
            Me.GRD_Lineas.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
            Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellActivation = Activation.AllowEdit
        End If


        Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellClickAction = CellClickAction.CellSelect


        Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("Fecha").CellClickAction = CellClickAction.CellSelect
        Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("Fecha").CellActivation = Activation.NoEdit

        Dim pRow As UltraGridRow


        'For Each pRow In Me.GRD_Lineas.GRID.Rows
        '    If oclsEntradaLinea.oLinqLinea.Entrada_Linea_NS.Where(Function(F) F.ID_NS = CInt(pRow.Cells("ID_NS").Value)).Count = 1 Then
        '        pRow.Cells("Seleccion").Value = True
        '        'pRow.Cells("Seleccion").Activation = Activation.Disabled
        '        oNSSeleccionadas.Add(pRow.Cells("ID_NS").Value)
        '        pRow.Update()
        '    End If
        'Next


    End Sub

    Private Sub GRD_Lineas_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Lineas.M_Grid_InitializeRow

        If e.Row.Cells("Seleccion").Value = True Then
            e.Row.CellAppearance.BackColor = Color.LightGreen
        Else
            e.Row.CellAppearance.BackColor = Color.LightCoral
        End If

        Call ControlSeleccio(Me.GRD_Lineas.GRID.ActiveCell, e.Row)

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

        End With
    End Sub

    Private Sub ControlSeleccio(ByRef pActiveCell As UltraGridCell, ByRef pActiveRow As UltraGridRow)
        If pActiveRow.Cells("Seleccion").Value = True Then

            If oVencimentsSeleccionats.Contains(pActiveRow.Cells("ID_Entrada_Vencimiento").Value) = False Then
                oVencimentsSeleccionats.Add(pActiveRow.Cells("ID_Entrada_Vencimiento").Value)
            End If

        Else
            If oVencimentsSeleccionats.Contains(pActiveRow.Cells("ID_Entrada_Vencimiento").Value) Then
                oVencimentsSeleccionats.Remove(pActiveRow.Cells("ID_Entrada_Vencimiento").Value)
            End If
        End If
    End Sub

#End Region
   

    Private Sub B_Buscar_Click(sender As Object, e As EventArgs) Handles B_Buscar.Click
        Call CargaGrid_VencimentsPendentsAssignar(False)
    End Sub
End Class