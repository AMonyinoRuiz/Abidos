Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEntrada_Linea_Parte
    Dim oDTC As DTCDataContext
    Dim oclsEntradaLinea As clsEntradaLinea
    Dim RowsSeleccionadasHoras As New ArrayList
    Dim RowsSeleccionadasMaterial As New ArrayList
    Dim RowsSeleccionadasGastos As New ArrayList

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Try

            Util.WaitFormObrir()
            Call AssignarHoras()
            Call AssignarMaterial()
            Call AssignarGastos()
            Util.WaitFormTancar()

            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE, "")


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

    Public Sub Entrada(ByRef pclsEntradaLinea As clsEntradaLinea, ByRef pDTC As DTCDataContext, ByVal pNomTab As String)
        Try
            Me.AplicarDisseny()

            oclsEntradaLinea = pclsEntradaLinea

            oDTC = pDTC
            'oDTC = New DTCDataContext(BD.Conexion)
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Asignar"

            Me.GRD_Gastos.M.clsToolBar.Boto_Afegir("SeleccionarTodo", "Seleccionar todos", True)
            Me.GRD_Horas.M.clsToolBar.Boto_Afegir("SeleccionarTodo", "Seleccionar todos", True)
            Me.GRD_Material.M.clsToolBar.Boto_Afegir("SeleccionarTodo", "Seleccionar todos", True)


            Dim _Boto As Infragistics.Win.UltraWinEditors.StateEditorButton = Me.C_Parte.ButtonsRight("SoloAsignados")
            _Boto.Checked = True   'Fem això pq volem que per defecte només es vegin els assignats al document
            Call CargarCombo_Parte(Me.C_Parte)

            'Cargar en rowsseleccionadas els id propuesta ja assignats
            Dim _Relacio As Parte_Horas
            For Each _Relacio In oclsEntradaLinea.oLinqLinea.Parte_Horas
                RowsSeleccionadasHoras.Add(_Relacio.ID_Parte_Horas)
            Next

            'Cargar en rowsseleccionadas els id propuesta ja assignats
            Dim _Relacio2 As Parte_Material
            For Each _Relacio2 In oclsEntradaLinea.oLinqLinea.Parte_Material
                RowsSeleccionadasMaterial.Add(_Relacio2.ID_Parte_Material)
            Next

            'Cargar en rowsseleccionadas els id propuesta ja assignats
            Dim _Relacio3 As Parte_Gastos
            For Each _Relacio3 In oclsEntradaLinea.oLinqLinea.Parte_Gastos
                RowsSeleccionadasGastos.Add(_Relacio3.ID_Parte_Gastos)
            Next

            Me.Tab_Principal.SelectedTab = Me.Tab_Principal.Tabs(pNomTab)

            Me.GRD_Horas.GRID.ActiveRow = Nothing
            Me.GRD_Horas.GRID.Selected.Rows.Clear()

            Call Calculs()


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AssignarHoras()

        Dim _IDParte_Horas As Integer
        'Aquest bucle es per no traspasar les línies que no estan seleccionades

        Dim _LlistatParteHoras As IList(Of Parte_Horas) = oclsEntradaLinea.oLinqLinea.Parte_Horas.ToList

        'oDTC.Parte_Horas.Where(Function(F) F.ID_Entrada_Linea = oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea)
        For Each _ParteHoras In _LlistatParteHoras
            _ParteHoras.Entrada_Linea = Nothing
            _ParteHoras.ID_Entrada_Linea = Nothing
        Next

        oDTC.SubmitChanges()

        For Each _IDParte_Horas In RowsSeleccionadasHoras
            'If oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(pRow.Cells("ID_Propuesta_Linea").Value)).Count = 0 Then
            Dim _Horas As Parte_Horas = oDTC.Parte_Horas.Where(Function(F) F.ID_Parte_Horas = _IDParte_Horas).FirstOrDefault
            _Horas.Entrada_Linea = oclsEntradaLinea.oLinqLinea
            'oDTC.Entrada_Linea_Propuesta_Linea.InsertOnSubmit(_Relacio)
            'End If
        Next


        oDTC.SubmitChanges()

        Call Calculs()
    End Sub

    Private Sub AssignarMaterial()

        Dim _IDParte_Material As Integer
        'Aquest bucle es per no traspasar les línies que no estan seleccionades

        Dim _LlistatParteMaterial As IList(Of Parte_Material) = oclsEntradaLinea.oLinqLinea.Parte_Material.ToList

        'oDTC.Parte_Horas.Where(Function(F) F.ID_Entrada_Linea = oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea)
        For Each _ParteMaterial In _LlistatParteMaterial
            _ParteMaterial.Entrada_Linea = Nothing
            _ParteMaterial.ID_Entrada_Linea = Nothing
        Next

        oDTC.SubmitChanges()

        For Each _IDParte_Material In RowsSeleccionadasMaterial
            'If oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(pRow.Cells("ID_Propuesta_Linea").Value)).Count = 0 Then
            Dim _Material As Parte_Material = oDTC.Parte_Material.Where(Function(F) F.ID_Parte_Material = _IDParte_Material).FirstOrDefault
            _Material.Entrada_Linea = oclsEntradaLinea.oLinqLinea
            'oDTC.Entrada_Linea_Propuesta_Linea.InsertOnSubmit(_Relacio)
            'End If
        Next


        oDTC.SubmitChanges()

        Call Calculs()
    End Sub

    Private Sub AssignarGastos()

        Dim _IDParte_Gastos As Integer
        'Aquest bucle es per no traspasar les línies que no estan seleccionades

        Dim _LlistatParteGastos As IList(Of Parte_Gastos) = oclsEntradaLinea.oLinqLinea.Parte_Gastos.ToList

        'oDTC.Parte_Horas.Where(Function(F) F.ID_Entrada_Linea = oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea)
        For Each _ParteGastos In _LlistatParteGastos
            _ParteGastos.Entrada_Linea = Nothing
            _ParteGastos.ID_Entrada_Linea = Nothing
        Next

        oDTC.SubmitChanges()

        For Each _IDParte_Gastos In RowsSeleccionadasGastos
            'If oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(pRow.Cells("ID_Propuesta_Linea").Value)).Count = 0 Then
            Dim _Gastos As Parte_Gastos = oDTC.Parte_Gastos.Where(Function(F) F.ID_Parte_Gastos = _IDParte_Gastos).FirstOrDefault
            _Gastos.Entrada_Linea = oclsEntradaLinea.oLinqLinea
            'oDTC.Entrada_Linea_Propuesta_Linea.InsertOnSubmit(_Relacio)
            'End If
        Next


        oDTC.SubmitChanges()

        Call Calculs()
    End Sub

    Private Sub Calculs()
        Me.T_NumHoras.Value = 0
        Me.T_NumHorasExtras.Value = 0
        Me.T_ImporteGastos.Value = 0

        Dim _IDParte_Horas As Integer
        For Each _IDParte_Horas In RowsSeleccionadasHoras
            Dim _Horas As Parte_Horas = oDTC.Parte_Horas.Where(Function(F) F.ID_Parte_Horas = _IDParte_Horas).FirstOrDefault
            Me.T_NumHoras.Value = Me.T_NumHoras.Value + _Horas.Horas
            Me.T_NumHorasExtras.Value = Me.T_NumHorasExtras.Value + _Horas.HorasExtras
        Next

        Dim _IDParte_Gastos As Integer
        For Each _IDParte_Gastos In RowsSeleccionadasGastos
            Dim _Gastos As Parte_Gastos = oDTC.Parte_Gastos.Where(Function(F) F.ID_Parte_Gastos = _IDParte_Horas).FirstOrDefault
            Me.T_ImporteGastos.Value = Me.T_ImporteGastos.Value + _Gastos.Gasto
        Next
    End Sub

#End Region

#Region "Grid Horas"

    Public Sub CargaGrid_Horas(ByVal pIDParte As Integer)

        Dim DT As New DataTable
        DT = BD.RetornaDataTable("Select *, cast(0 as bit) as Seleccion From C_Parte_Horas Where  (ID_Entrada_Linea is null or ID_Entrada_Linea=" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea & ") and  ID_Parte=" & pIDParte)
        Me.GRD_Horas.M.clsUltraGrid.Cargar(DT)
        Me.GRD_Horas.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True

        Dim pRow As UltraGridRow
        For Each pRow In Me.GRD_Horas.GRID.Rows

            'pRow.Cells("Seleccion").Value = True
            'pRow.CellAppearance.BackColor = Color.White
            'RowsSeleccionadas.Add(pRow)
            pRow.Update()
        Next

    End Sub

    Private Sub GRD_Horas_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Horas.M_Grid_InitializeRow
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

        ' Dim _Linea As Propuesta_Linea
        '_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
        '  If oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).Count = 1 Then
        'If _Linea.Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True And F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea).Count > 0 Then
        If RowsSeleccionadasHoras.Contains(e.Row.Cells("ID_Parte_Horas").Value) Then
            e.Row.Cells("Seleccion").Value = True
            '    e.Row.Cells("Seleccion").Activation = Activation.Disabled
        End If

    End Sub

    Private Sub GRD_Horas_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Horas.M_GRID_AfterCellActivate
        With GRD_Horas.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                'Dim _Linea As Propuesta_Linea = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(.ActiveRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault()
                If .ActiveCell.Value = True Then
                    RowsSeleccionadasHoras.Remove(.ActiveCell.Row.Cells("ID_Parte_Horas").Value)
                    .ActiveCell.Value = False
                Else
                    RowsSeleccionadasHoras.Add(.ActiveCell.Row.Cells("ID_Parte_Horas").Value)
                    .ActiveCell.Value = True
                End If
                oDTC.SubmitChanges()
                Call Calculs()
            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
        End With
    End Sub

    Private Sub GRD_Horas_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Horas.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "SeleccionarTodo" Then
            Dim pRow As UltraGridRow
            For Each pRow In Me.GRD_Horas.GRID.Rows
                If pRow.Cells("Seleccion").Value = False Then
                    pRow.Cells("Seleccion").Value = True
                    RowsSeleccionadasHoras.Add(pRow.Cells("ID_Parte_Horas").Value)
                    pRow.Update()
                    Call Calculs()
                End If
            Next
        End If
    End Sub

#End Region

#Region "Grid Material"

    Public Sub CargaGrid_Material(ByVal pIDParte As Integer)

        Dim DT As New DataTable
        DT = BD.RetornaDataTable("Select *, cast(0 as bit) as Seleccion From C_Parte_Material Where  (ID_Entrada_Linea is null or ID_Entrada_Linea=" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea & ") and  ID_Parte=" & pIDParte)

        With Me.GRD_Material
            .M.clsUltraGrid.Cargar(DT)
            .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
        End With
        Dim pRow As UltraGridRow
        For Each pRow In Me.GRD_Horas.GRID.Rows

            'pRow.Cells("Seleccion").Value = True
            'pRow.CellAppearance.BackColor = Color.White
            'RowsSeleccionadas.Add(pRow)
            pRow.Update()
        Next

    End Sub

    Private Sub GRD_Material_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Material.M_Grid_InitializeRow

        e.Row.Cells("Seleccion").Column.CellClickAction = CellClickAction.CellSelect
        e.Row.Cells("Seleccion").Column.CellActivation = Activation.AllowEdit

        If RowsSeleccionadasMaterial.Contains(e.Row.Cells("ID_Parte_Material").Value) Then
            e.Row.Cells("Seleccion").Value = True
        End If

    End Sub

    Private Sub GRD_Material_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Material.M_GRID_AfterCellActivate
        With GRD_Material.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                If .ActiveCell.Value = True Then
                    RowsSeleccionadasMaterial.Remove(.ActiveCell.Row.Cells("ID_Parte_Material").Value)
                    .ActiveCell.Value = False
                Else
                    RowsSeleccionadasMaterial.Add(.ActiveCell.Row.Cells("ID_Parte_Material").Value)
                    .ActiveCell.Value = True
                End If
                oDTC.SubmitChanges()
                Call Calculs()
            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
        End With
    End Sub

    Private Sub GRD_Material_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Material.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "SeleccionarTodo" Then
            Dim pRow As UltraGridRow
            For Each pRow In Me.GRD_Material.GRID.Rows
                If pRow.Cells("Seleccion").Value = False Then
                    pRow.Cells("Seleccion").Value = True
                    RowsSeleccionadasMaterial.Add(pRow.Cells("ID_Parte_Material").Value)
                    pRow.Update()

                End If
            Next
            Call Calculs()
        End If
    End Sub
#End Region

#Region "Grid Gastos"

    Public Sub CargaGrid_Gastos(ByVal pIDParte As Integer)

        Dim DT As New DataTable
        DT = BD.RetornaDataTable("Select *, cast(0 as bit) as Seleccion From C_Parte_Gastos Where  (ID_Entrada_Linea is null or ID_Entrada_Linea=" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea & ") and  ID_Parte=" & pIDParte)

        With Me.GRD_Gastos
            .M.clsUltraGrid.Cargar(DT)
            .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True

            Dim pRow As UltraGridRow
            For Each pRow In .GRID.Rows
                'pRow.Cells("Seleccion").Value = True
                'pRow.CellAppearance.BackColor = Color.White
                'RowsSeleccionadas.Add(pRow)
                pRow.Update()
            Next

        End With

    End Sub

    Private Sub GRD_Gastos_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Gastos.M_Grid_InitializeRow
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

        ' Dim _Linea As Propuesta_Linea
        '_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
        '  If oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).Count = 1 Then
        'If _Linea.Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True And F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea).Count > 0 Then
        If RowsSeleccionadasGastos.Contains(e.Row.Cells("ID_Parte_Gastos").Value) Then
            e.Row.Cells("Seleccion").Value = True
            '    e.Row.Cells("Seleccion").Activation = Activation.Disabled
        End If

    End Sub

    Private Sub GRD_Gastos_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Gastos.M_GRID_AfterCellActivate
        With GRD_Gastos.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                'Dim _Linea As Propuesta_Linea = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(.ActiveRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault()
                If .ActiveCell.Value = True Then
                    RowsSeleccionadasGastos.Remove(.ActiveCell.Row.Cells("ID_Parte_Gastos").Value)
                    .ActiveCell.Value = False
                Else
                    RowsSeleccionadasGastos.Add(.ActiveCell.Row.Cells("ID_Parte_Gastos").Value)
                    .ActiveCell.Value = True
                End If
                oDTC.SubmitChanges()
                Call Calculs()
            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
        End With
    End Sub

    Private Sub GRD_Gastos_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Gastos.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "SeleccionarTodo" Then
            Dim pRow As UltraGridRow
            For Each pRow In Me.GRD_Gastos.GRID.Rows
                If pRow.Cells("Seleccion").Value = False Then
                    pRow.Cells("Seleccion").Value = True
                    RowsSeleccionadasGastos.Add(pRow.Cells("ID_Parte_Gastos").Value)
                    pRow.Update()
                End If
            Next
            Call Calculs()
        End If
    End Sub
#End Region

    Private Sub CargarCombo_Parte(ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo)
        Try
            Dim _Boto As Infragistics.Win.UltraWinEditors.StateEditorButton = Me.C_Parte.ButtonsRight("SoloAsignados")

            Dim _LlistatPartes As IEnumerable

            Dim _LlistatInstalacionsSeleccionades As IEnumerable(Of Instalacion) = From Taula In oDTC.Entrada_Instalacion Where Taula.ID_Entrada = oclsEntradaLinea.oLinqEntrada.ID_Entrada Select Taula.Instalacion

            '_LlistatInstalacions = From Taula In oDTC.Entrada_InstalacionPropuesta Order By Taula.ID_Instalacion Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.Instalacion.OtrosDetalles, Taula.ID_Instalacion, Taula.Instalacion.OtrosDetalles, Taula.Instalacion.FechaInstalacion, Cliente = Taula.Instalacion.Cliente.NombreComercial, Poblacion = Taula.Instalacion.Poblacion
            '_LlistatPartes = From Taula In oDTC.Parte Where _LlistatInstalacionsSeleccionades.Contains(Taula.Instalacion) Order By Taula.ID_Parte Descending Group By ID_Parte = Taula.ID_Parte, Visualitzar = Taula.ID_Parte & " - " & Taula.Parte.FechaAlta, TrabajoARealizar = Taula.Parte.TrabajoARealizar, FechaParte = Taula.Parte.FechaAlta Into Group Select ID_Parte, Visualitzar, TrabajoARealizar, FechaParte
            '_LlistatPartes = From Taula In oDTC.Parte Where _LlistatInstalacionsSeleccionades.Contains(Taula.Instalacion) Order By Taula.ID_Parte Descending Select ID_Parte = Taula.ID_Parte, Visualitzar = Taula.ID_Parte & " - " & Taula.Parte.FechaAlta, TrabajoARealizar = Taula.Parte.TrabajoARealizar, FechaParte = Taula.Parte.FechaAlta 'Into Group Select ID_Parte, Visualitzar, TrabajoARealizar, FechaParte
            ' _LlistatPartes = From Taula In oDTC.Entrada_Parte Where _LlistatInstalacionsSeleccionades.Contains(Taula.Parte.Instalacion) Order By Taula.ID_Parte Descending Group By ID_Parte = Taula.ID_Parte, Visualitzar = Taula.ID_Parte & " - " & Taula.Parte.FechaAlta, TrabajoARealizar = Taula.Parte.TrabajoARealizar, FechaParte = Taula.Parte.FechaAlta Into Group Select ID_Parte, Visualitzar, TrabajoARealizar, FechaParte

            If _Boto.Checked = True Then
                '_LlistatPartes = From Taula In oDTC.Parte Where Taula.Entrada_Parte.Where(Function(F) F.ID_Entrada = oclsEntradaLinea.oLinqEntrada.ID_Entrada).Contains(Taula.Entrada_Parte.FirstOrDefault) And Taula.ID_Parte_Estado <> CInt(EnumParteEstado.Finalizado) Order By Taula.ID_Parte Descending Group By ID_Parte = Taula.ID_Parte, Visualitzar = Taula.ID_Parte & " - " & Taula.TrabajoARealizar, TrabajoARealizar = Taula.TrabajoARealizar, FechaParte = Taula.FechaAlta Into Group Select ID_Parte, Visualitzar, TrabajoARealizar, FechaParte Order By ID_Parte Descending
                _LlistatPartes = From Taula In oDTC.Parte Where Taula.Entrada_Parte.Where(Function(F) F.ID_Entrada = oclsEntradaLinea.oLinqEntrada.ID_Entrada).Contains(Taula.Entrada_Parte.FirstOrDefault) Order By Taula.ID_Parte Descending Select ID_Parte = Taula.ID_Parte, Visualitzar = Taula.ID_Parte & " - " & Taula.TrabajoARealizar, TrabajoARealizar = Taula.TrabajoARealizar, RetornaHoresPendentsAssignarAUnAlbara = Taula.RetornaHoresPendentsAssignarAUnAlbara, FechaParte = Taula.FechaAlta
            Else  'And Taula.ID_Parte_Estado <> CInt(EnumParteEstado.Finalizado)
                _LlistatPartes = From Taula In oDTC.Parte Where _LlistatInstalacionsSeleccionades.Contains(Taula.Instalacion) Order By Taula.ID_Parte Descending Select ID_Parte = Taula.ID_Parte, Visualitzar = Taula.ID_Parte & " - " & Taula.TrabajoARealizar, TrabajoARealizar = Taula.TrabajoARealizar, RetornaHoresPendentsAssignarAUnAlbara = Taula.RetornaHoresPendentsAssignarAUnAlbara, FechaParte = Taula.FechaAlta
            End If



            pCombo.DataSource = _LlistatPartes

            If _LlistatPartes Is Nothing Then
                Exit Sub
            End If

            With pCombo
                .AutoCompleteMode = AutoCompleteMode.None
                .AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
                .AlwaysInEditMode = False
                .DropDownStyle = UltraComboStyle.DropDownList

                .MaxDropDownItems = 16
                .DisplayMember = "Visualitzar"
                .ValueMember = "ID_Parte"
                .DisplayLayout.Bands(0).Columns("ID_Parte").Width = 50
                .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Caption = "Código"
                .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Appearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Width = 600
                .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Header.Caption = "Trabajo a realizar"
                .DisplayLayout.Bands(0).Columns("FechaParte").Width = 75
                .DisplayLayout.Bands(0).Columns("FechaParte").Header.Caption = "Fecha"
                .DisplayLayout.Bands(0).Columns("RetornaHoresPendentsAssignarAUnAlbara").Width = 110
                .DisplayLayout.Bands(0).Columns("RetornaHoresPendentsAssignarAUnAlbara").CellAppearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("RetornaHoresPendentsAssignarAUnAlbara").Header.Caption = "Horas Pendientes"

                .DisplayLayout.Bands(0).Columns("Visualitzar").Hidden = True
                If .Rows.Count > 0 Then
                    '.SelectedRow = .Rows(0)
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub C_Parte_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Parte.BeforeDropDown
        Call CargarCombo_Parte(Me.C_Parte)

    End Sub

    Private Sub C_Instalacion_RowSelected(sender As Object, e As RowSelectedEventArgs) Handles C_Parte.RowSelected
        Call CargaGrid_Horas(Me.C_Parte.Value)
        Call CargaGrid_Material(Me.C_Parte.Value)
        Call CargaGrid_Gastos(Me.C_Parte.Value)
    End Sub

    Private Sub C_Parte_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles C_Parte.InitializeLayout

    End Sub

    Private Sub C_Parte_BeforeEditorButtonCheckStateChanged(sender As Object, e As UltraWinEditors.BeforeCheckStateChangedEventArgs) Handles C_Parte.BeforeEditorButtonCheckStateChanged
        Dim _Boto As Infragistics.Win.UltraWinEditors.StateEditorButton = Me.C_Parte.ButtonsRight("SoloAsignados")
        If e.NewCheckState = CheckState.Checked Then
            _Boto.Text = "Visualizar los partes de las instalaciones asignadas"
        Else
            _Boto.Text = "Visualizar sólo los partes asignados"
        End If
    End Sub


End Class