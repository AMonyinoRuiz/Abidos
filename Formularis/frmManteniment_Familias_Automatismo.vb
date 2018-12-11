Imports Infragistics.Win.UltraWinGrid

Public Class frmManteniment_Familias_Automatismo
    Dim oDTC As New DTCDataContext(BD.Conexion)
    Dim RowsSeleccionadas As New ArrayList
    Dim oIDCaracteristicaInstalacion As Integer

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If RowsSeleccionadas.Count = 0 Then
            Mensaje.Mostrar_Mensaje("Error. Tiene que haber al menos una línea seleccionada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If
        If Mensaje.Mostrar_Mensaje("¿Desea agregar la característica a todos los artículos seleccionados?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then

            Dim pRow As UltraGridRow
            For Each pRow In RowsSeleccionadas
                Dim _Linea As New Producto_Producto_Caracteristica_Instalacion
                _Linea.ID_Producto = pRow.Cells("ID_Producto").Value
                _Linea.ID_Producto_Caracteristica_Instalacion = oIDCaracteristicaInstalacion
                _Linea.ID_Producto_Caracteristica_Vision = Me.C_Vision.Value
                _Linea.Imprimible = Me.CH_Imprimible.Checked
                _Linea.Verificable = Me.CH_Verificable.Checked
                _Linea.Valor = Me.T_Descripcion.Text
                oDTC.Producto_Producto_Caracteristica_Instalacion.InsertOnSubmit(_Linea)
            Next

            oDTC.SubmitChanges()

            Mensaje.Mostrar_Mensaje("Característica añadida correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
        Else
            Exit Sub
        End If

        Call M_ToolForm1_m_ToolForm_Sortir()
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"
    Public Sub Entrada(ByVal pIDFamilia As Integer, ByVal pIDCaracteristica As Integer)
        Me.AplicarDisseny()
        oIDCaracteristicaInstalacion = pIDCaracteristica
        Util.Cargar_Combo(Me.C_Vision, "Select ID_Producto_Caracteristica_Vision, Descripcion From Producto_Caracteristica_Vision Where Activo=1 Order by Codigo", True)
        Call CargaGrid_Lineas(pIDFamilia)
        Me.GRD.M.clsToolBar.Boto_Afegir("SeleccionarTodo", "Seleccionar todo", True)
        Me.GRD.M.clsToolBar.Boto_Afegir("DeseleccionarTodo", "Deseleccionar todo", True)

    End Sub

    Private Sub GRD_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD.M_Grid_InitializeRow
        'If e.Row.Cells("ID_Producto_Subfamilia_Traspaso").Value = 0 Then
        '    e.Row.Cells("Seleccion").Value = False
        '    e.Row.CellAppearance.BackColor = Color.LightCoral
        'Else
        '    e.Row.Cells("Seleccion").Value = True
        '    e.Row.CellAppearance.BackColor = Color.White
        '    e.Row.Update()
        '    RowsSeleccionadas.Add(e.Row)
        'End If

        e.Row.Cells("Seleccion").Column.CellClickAction = CellClickAction.CellSelect
        e.Row.Cells("Seleccion").Column.CellActivation = Activation.AllowEdit
    End Sub

    Private Sub GRD_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD.M_GRID_AfterCellActivate
        With GRD.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                'Dim _Linea As Propuesta_Linea = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(.ActiveRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault()
                If .ActiveCell.Value = True Then
                    .ActiveCell.Value = False
                    '.ActiveCell.Row.Cells("ID_Producto_SubFamilia_Traspaso").Value = 0
                    '_Linea.Producto_SubFamilia_Traspaso = oDTC.Producto_SubFamilia_Traspaso.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso = 0).FirstOrDefault
                    RowsSeleccionadas.Remove(.ActiveCell.Row)
                Else
                    .ActiveCell.Value = True
                    '.ActiveCell.Row.Cells("ID_Producto_SubFamilia_Traspaso").Value = 1
                    '_Linea.Producto_SubFamilia_Traspaso = oDTC.Producto_SubFamilia_Traspaso.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso = 1).FirstOrDefault
                    RowsSeleccionadas.Add(.ActiveCell.Row)
                End If
                'oDTC.SubmitChanges()
            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
        End With
    End Sub

#End Region


#Region "Grid"
    Public Sub CargaGrid_Lineas(ByVal pIDFamilia As Integer)
        ' Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        'If _Propuesta Is Nothing = False Then
        Dim DT As DataTable = BD.RetornaDataTable("Select *, cast(0 as bit) as Seleccion From C_Producto Where Activo=1 And ID_Producto_Familia=" & pIDFamilia & " and (Select Count(*) From Producto_Producto_Caracteristica_Instalacion as PPCI Where PPCI.ID_Producto=c_Producto.ID_Producto and PPCI.ID_Producto_Caracteristica_Instalacion=" & oIDCaracteristicaInstalacion & ")=0 Order by Descripcion")
        Me.GRD.M.clsUltraGrid.Cargar(DT)
        Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

        'End If
    End Sub

    Private Sub GRD_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD.M_ToolGrid_ToolClickBotonsExtras
        Try
            Dim Valor As Boolean

            Select Case e.Tool.Key
                Case "SeleccionarTodo"
                    Valor = True
                Case "DeseleccionarTodo"
                    Valor = False
            End Select

            RowsSeleccionadas.Clear() 'eliminem totes les rows seleccionades
            For Each pRow In Me.GRD.GRID.Rows
                pRow.Cells("Seleccion").Value = Valor
                If Valor = True Then
                    RowsSeleccionadas.Add(pRow) 'afegim totes les rows
                End If
                pRow.Update()
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

End Class