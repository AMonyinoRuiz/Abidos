Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo

Public Class frmActividadCRM_Principal
    Dim oTemporitzadorSeguimiento As Integer = 0
    Dim _UltimRecompteHistorialCanvis As Integer = 0

    Public Sub Entrada()
        Me.AplicarDisseny()

        Call CargarGrids()

    End Sub

    Private Sub CargarGrids()

        GridView1.RefreshData()
        Me.GRD_Actividades.DataSource = Nothing
        Me.GRD_Actividades.DataSource = clsActividad.RetornaDTActivitatsAVisualitzarSegonsPersonal(Seguretat.oUser.ID_Personal, Me.B_VisualizarPendientes.Checked, Me.B_VisualizarTodos.Checked, Me.B_ALaEsperaDeRespuesta.Checked, Me.B_VisualizarSeguimiento.Checked)
        Me.Text = "Actividades (" & clsActividad.RetornaNumActivitatsPendentsLlegir(Seguretat.oUser.ID_Personal) & ")"
        Me.GRD_Chat.DataSource = clsActividad.RetornaDTChatsActivitatsSegonsPersonal(Seguretat.oUser.ID_Personal, Me.B_Chat_Todos.Checked)

        Dim _Pendientes As Integer = clsActividad.RetornaDTActivitatsAVisualitzarSegonsPersonal(Seguretat.oUser.ID_Personal, True, False, False, False).Rows.Count
        Dim _Finalizados As Integer = clsActividad.RetornaDTActivitatsAVisualitzarSegonsPersonal(Seguretat.oUser.ID_Personal, True, True, False, False).Rows.Count
        Dim _ALaEspera As Integer = clsActividad.RetornaDTActivitatsAVisualitzarSegonsPersonal(Seguretat.oUser.ID_Personal, True, False, True, False).Rows.Count
        Dim _Seguimiento As Integer = clsActividad.RetornaDTActivitatsAVisualitzarSegonsPersonal(Seguretat.oUser.ID_Personal, True, False, False, True).Rows.Count
        Me.B_VisualizarPendientes.Text = "Pendientes (" & _Pendientes & ")"
        Me.B_VisualizarTodos.Text = "Finalizados (" & _Finalizados - _Pendientes & ")"
        Me.B_ALaEsperaDeRespuesta.Text = "A la espera (" & _ALaEspera - _Pendientes & ")"
        Me.B_VisualizarSeguimiento.Text = "Seguimiento (" & _Seguimiento - _Pendientes & ")"
        GridView1.PreviewIndent = 25
        ' GridView1.OptionsView.ShowPreview = True
        GridView1.PreviewLineCount = 0
        ' Me.L_NumRows.Text = GridView1.DataRowCount
    End Sub

    Private Sub GridView1_CustomRowFilter(sender As Object, e As Views.Base.RowFilterEventArgs) Handles GridView1.CustomRowFilter
        '   Exit Sub
        'Dim _SearchText As String = GridView1.FindFilterText
        'If Not String.IsNullOrEmpty(_SearchText) Then
        '    'e.Handled = True
        '    Dim _RowHandle As Integer = GridView1.GetRow(e.ListSourceRow)
        '    Dim _PreviewText As String = GridView1.GetRowPreviewDisplayText(_RowHandle)
        '    If _PreviewText.ToUpper().Contains(_SearchText.ToUpper) Then
        '        e.Visible = True
        '    Else
        '        e.Visible = False
        '    End If
        'End If
        ''  pre GridView1.GetRowPreviewDisplayText(GridView1.GetRowHandle(e.ListSourceRow))
        Dim Grid As GridView = sender
        Dim searchtext As String = Grid.FindFilterText


        If Not String.IsNullOrEmpty(searchtext) Then

            Dim Row As DataRowView = Me.GridView1.GetRow(Me.GridView1.GetRowHandle(e.ListSourceRow))
            ' searchtext = Grid.GetRowPreviewDisplayText(Grid.GetRowHandle(e.ListSourceRow))


            Dim rowhandle As Integer = GridView1.GetRowHandle(e.ListSourceRow)
            Dim previewtext As String = GridView1.GetRowPreviewDisplayText(rowhandle)

            If previewtext.ToUpper().Contains(searchtext.ToUpper()) Then
                e.Handled = True
                e.Visible = True
                If GridView1.OptionsView.ShowPreview = False Then
                    GridView1.PreviewLineCount = -1

                End If
            Else
                ' e.Handled = True
                e.Visible = False
            End If
        Else
            GridView1.OptionsView.ShowPreview = False
            GridView1.PreviewLineCount = 0
        End If


    End Sub

    Private Sub GridViewActividades_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Dim view As GridView = CType(sender, GridView)

        Dim pt As Point = view.GridControl.PointToClient(Control.MousePosition)

        DoRowDoubleClickActividades(view, pt)
    End Sub

    Private Sub DoRowDoubleClickActividades(ByVal view As GridView, ByVal pt As Point)

        Dim info As GridHitInfo = view.CalcHitInfo(pt)

        If info.InRow OrElse info.InRowCell Then

            If info.Column Is Nothing Then
                Exit Sub
            End If

            If GridView1.GetRowCellValue(info.RowHandle, info.Column) Is Nothing Then
                Exit Sub
            End If

            Dim _IDActividad As Integer = Me.GridView1.GetRowCellValue(info.RowHandle, "ID_ActividadCRM")

            Dim _frmActividadCRM As New frmActividadCRM_Mantenimiento
            _frmActividadCRM.Entrada(_IDActividad)
            _frmActividadCRM.FormObrir(frmPrincipal, True)

            'Dim view As GridView = CType(sender, GridView)
            'Dim pt As Point = view.GridControl.PointToClient(Control.MousePosition)
            'Dim info As GridHitInfo = view.CalcHitInfo(pt)

            If info.InRow Then
                If info.Column Is Nothing Then
                    Exit Sub
                End If
                If GridView1.GetRowCellValue(info.RowHandle, "Leido") = 0 Then
                    GridView1.SetRowCellValue(info.RowHandle, "Leido", 1)
                Else
                    ' GridView1.SetRowCellValue(info.RowHandle, "Leido", 0)
                End If

                ' MsgBox(GridView1.GetRowCellValue(info.RowHandle, "Leido"))

                'Exit Sub
            End If


            'Dim info As GridHitInfo = view.CalcHitInfo(pt)

            'GridView1.SetRowCellValue(info)

            GridView1.LayoutChanged()
            GridView1.MakeRowVisible(GridView1.FocusedRowHandle)
        End If

    End Sub

    Private Sub GridView1_Click(sender As Object, e As EventArgs) Handles GridView1.Click
        Dim view As GridView = CType(sender, GridView)
        Dim pt As Point = view.GridControl.PointToClient(Control.MousePosition)
        Dim info As GridHitInfo = view.CalcHitInfo(pt)

        If info.InRow Then
            If info.Column Is Nothing Then
                Exit Sub
            End If
            If GridView1.GetRowCellValue(info.RowHandle, "Leido") = 0 Then
                GridView1.SetRowCellValue(info.RowHandle, "Leido", 1)
            Else
                ' GridView1.SetRowCellValue(info.RowHandle, "Leido", 0)
            End If

            ' MsgBox(GridView1.GetRowCellValue(info.RowHandle, "Leido"))

            'Exit Sub
            GridView1.LayoutChanged()
            GridView1.MakeRowVisible(GridView1.FocusedRowHandle)
        End If


        'Dim info As GridHitInfo = view.CalcHitInfo(pt)

        'GridView1.SetRowCellValue(info)


    End Sub

    'Private Sub GridView1_RowStyle(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles GridView1.RowStyle
    '    'Dim _GridView As DevExpress.XtraGrid.Views.Grid.GridView = sender
    '    'Dim _Llegit As Boolean = Convert.ToBoolean(_GridView.GetRowCellValue(e.RowHandle, ""))
    '    'If _Llegit = True Then
    '    '    e.Appearance.Font = New Font(e.Appearance.Font, FontStyle.Bold)
    '    'Else
    '    '    e.Appearance.Font = New Font(e.Appearance.Font, FontStyle.Regular)
    '    'End If
    'End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Dim _ActualRecompteHistorialCanvis As Integer = BD.RetornaValorSQL("Select Count(*) From ActividadCRM_RegistroCambios")
        If _UltimRecompteHistorialCanvis = _ActualRecompteHistorialCanvis Then
            'Exit Sub
        Else
            _UltimRecompteHistorialCanvis = _ActualRecompteHistorialCanvis
            Call CargarGrids()
        End If



        If Me.B_VisualizarSeguimiento.Checked = False Then
            If oTemporitzadorSeguimiento = 2 Then
                Me.B_VisualizarSeguimiento.Checked = False
                oTemporitzadorSeguimiento = 0
            Else
                oTemporitzadorSeguimiento = oTemporitzadorSeguimiento + 1
            End If
        End If

    End Sub

    Private Sub B_VisualizarTodos_CheckedChanged(sender As Object, e As EventArgs) Handles B_VisualizarTodos.CheckedChanged
        Call CargarGrids()
    End Sub

    Private Sub GridViewChat_DoubleClick(sender As Object, e As EventArgs) Handles GridView2.DoubleClick
        Dim view As GridView = CType(sender, GridView)

        Dim pt As Point = view.GridControl.PointToClient(Control.MousePosition)

        DoRowDoubleClickChat(view, pt)
    End Sub

    Private Sub DoRowDoubleClickChat(ByVal view As GridView, ByVal pt As Point)

        Dim info As GridHitInfo = view.CalcHitInfo(pt)

        If info.InRow OrElse info.InRowCell Then

            If info.Column Is Nothing Then
                Exit Sub
            End If

            If GridView2.GetRowCellValue(info.RowHandle, info.Column) Is Nothing Then
                Exit Sub
            End If

            Dim _IDActividad As Integer = Me.GridView2.GetRowCellValue(info.RowHandle, "ID_ActividadCRM")

            Dim _frmActividadCRM As New frmActividadCRM_Mantenimiento
            _frmActividadCRM.Entrada(_IDActividad, "Chat")
            _frmActividadCRM.FormObrir(frmPrincipal, True)

            'Dim view As GridView = CType(sender, GridView)
            'Dim pt As Point = view.GridControl.PointToClient(Control.MousePosition)
            'Dim info As GridHitInfo = view.CalcHitInfo(pt)

            'If info.InRow Then
            '    If info.Column Is Nothing Then
            '        Exit Sub
            '    End If
            '    If GridView1.GetRowCellValue(info.RowHandle, "Leido") = 0 Then
            '        GridView1.SetRowCellValue(info.RowHandle, "Leido", 1)
            '    Else
            '        ' GridView1.SetRowCellValue(info.RowHandle, "Leido", 0)
            '    End If

            '    ' MsgBox(GridView1.GetRowCellValue(info.RowHandle, "Leido"))

            '    'Exit Sub
            'End If


            ''Dim info As GridHitInfo = view.CalcHitInfo(pt)

            ''GridView1.SetRowCellValue(info)

            'GridView1.LayoutChanged()
            'GridView1.MakeRowVisible(GridView1.FocusedRowHandle)
        End If

    End Sub

    Private Sub B_ALaEsperaDeRespuesta_CheckedChanged(sender As Object, e As EventArgs) Handles B_ALaEsperaDeRespuesta.CheckedChanged
        Call CargarGrids()

    End Sub

    Private Sub B_VisualizarSeguimiento_CheckedChanged(sender As Object, e As EventArgs) Handles B_VisualizarSeguimiento.CheckedChanged
        Call CargarGrids()
        If Me.B_VisualizarSeguimiento.Checked = True Then
            oTemporitzadorSeguimiento = 0 'posem a 0 per a que començi a contar
        End If
    End Sub

    Private Sub B_CrearActividad_Click(sender As Object, e As EventArgs) Handles B_CrearActividad.Click
        Dim _frmActividadCRM As New frmActividadCRM_Mantenimiento
        _frmActividadCRM.Entrada()
        _frmActividadCRM.FormObrir(frmPrincipal, True)
    End Sub

    Private Sub B_Actualizar_Click(sender As Object, e As EventArgs) Handles B_Actualizar.Click
        Call CargarGrids()
    End Sub

    Private Sub CheckButton1_CheckedChanged(sender As Object, e As EventArgs) Handles B_VisualizarPendientes.CheckedChanged
        Call CargarGrids()
    End Sub

    Private Sub B_Chat_Finalizados_CheckedChanged(sender As Object, e As EventArgs) Handles B_Chat_Todos.CheckedChanged
        Call CargarGrids()
    End Sub

    Private Sub GridView1_MeasurePreviewHeight(sender As Object, e As RowHeightEventArgs) Handles GridView1.MeasurePreviewHeight
        '  e.RowHeight = 20
    End Sub

    Private Sub GridView1_RowCountChanged(sender As Object, e As EventArgs) Handles GridView1.RowCountChanged
        Me.L_NumRows.Text = GridView1.DataRowCount
    End Sub

    Private Sub GridView1_SubstituteFilter(sender As Object, e As DevExpress.Data.SubstituteFilterEventArgs) Handles GridView1.SubstituteFilter
        'Posem això aquí pq si ho posem al event customdrawfilter peta incomprensiblement
        If GridView1.OptionsView.ShowPreview = False Then
            GridView1.OptionsView.ShowPreview = True
        End If
    End Sub
End Class