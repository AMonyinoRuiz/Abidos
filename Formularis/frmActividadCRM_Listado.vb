Public Class frmActividadCRM_Listado

#Region "Toolform"
    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"
    Public Sub Entrada()
        Me.AplicarDisseny()

        Util.Cargar_Combo(Me.C_Comercial, "Select ID_Personal, Nombre From Personal Where Activo=1 and ActivarCalendario=1 and ID_Personal in (Select ID_PersonalACargo From Personal_PersonalACargo Where ID_Personal=" & Seguretat.oUser.ID_Personal & ") Order by Nombre", False)
        Me.C_Comercial.Value = Seguretat.oUser.ID_Personal

    End Sub

    Public Sub AlTancarFormulariMantenimentActivitatsCRM()
        Dim _Realizado As Boolean
        Select Case Me.Tab_Principal.ActiveTab.Key
            Case "Pendiente"
                _Realizado = False
            Case "Realizadas"
                _Realizado = True
        End Select

        Call CargaGrid_Activitats(Me.C_Comercial.Value, True)
    End Sub

#End Region

#Region "Grid Activitats Pendients"
    Private Sub CargaGrid_Activitats(ByVal pIDPersonal As Integer, ByVal pRealizado As Boolean)
        Try

            Dim oDTC As New DTCDataContext(BD.Conexion)
            Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = pIDPersonal).FirstOrDefault

            Dim _Grid As M_UltraGrid.m_UltraGrid
            If pRealizado = True Then
                _Grid = Me.GRD_Realizadas
            Else
                _Grid = Me.GRD_Pendientes
            End If


            With _Grid
                Dim DT As DataTable = BD.RetornaDataTable("Select * From C_ActividadCRM Where Activo=1 and Realizado=" & Util.Bool_To_Int(pRealizado) & " and ID_Personal=" & _Personal.Usuario.FirstOrDefault.ID_Personal, True)
                .M.clsUltraGrid.Cargar(DT)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Pendientes.M_ToolGrid_ToolAfegir, GRD_Realizadas.M_ToolGrid_ToolAfegir
        Try
            Dim frm As New frmActividadCRM_Mantenimiento
            frm.Entrada()
            frm.FormObrir(Me, True)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormulariMantenimentActivitatsCRM
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Pendientes.M_ToolGrid_ToolEditar
        Try
            Call GRD_M_ToolGrid_ToolVisualitzarDobleClickRow()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Pendientes.M_ToolGrid_ToolVisualitzarDobleClickRow
        If Me.GRD_Pendientes.GRID.Selected.Rows.Count = 1 Then
            Dim frm As New frmActividadCRM_Mantenimiento
            frm.Entrada(Me.GRD_Pendientes.GRID.Selected.Rows(0).Cells("ID_ActividadCRM").Value)
            frm.FormObrir(Me, True)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormulariMantenimentActivitatsCRM
        End If
    End Sub

#End Region

#Region "Grid Activitats Realitzades"
    Private Sub GRD_Realizadas_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Realizadas.M_ToolGrid_ToolEditar
        Try
            Call GRD_Realizadas_M_ToolGrid_ToolVisualitzarDobleClickRow()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Realizadas_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Realizadas.M_ToolGrid_ToolVisualitzarDobleClickRow
        If Me.GRD_Realizadas.GRID.Selected.Rows.Count = 1 Then
            Dim frm As New frmActividadCRM_Mantenimiento
            frm.Entrada(Me.GRD_Realizadas.GRID.Selected.Rows(0).Cells("ID_ActividadCRM").Value)
            frm.FormObrir(Me, True)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormulariMantenimentActivitatsCRM
        End If
    End Sub

#End Region

    Private Sub C_Comercial_ValueChanged(sender As Object, e As EventArgs) Handles C_Comercial.ValueChanged
        Try

            If Me.Tab_Principal.ActiveTab Is Nothing Then
                Exit Sub
            End If

            Dim _Realizado As Boolean
            Select Case Me.Tab_Principal.ActiveTab.Key
                Case "Pendiente"
                    _Realizado = False
                Case "Realizadas"
                    _Realizado = True
            End Select

            Call CargaGrid_Activitats(Me.C_Comercial.Value, _Realizado)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Tab_Principal_SelectedTabChanged(sender As Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles Tab_Principal.SelectedTabChanged
        Dim _Realizado As Boolean
        Select Case Me.Tab_Principal.ActiveTab.Key
            Case "Pendiente"
                _Realizado = False
            Case "Realizadas"
                _Realizado = True
        End Select
        Call CargaGrid_Activitats(Me.C_Comercial.Value, _Realizado)
    End Sub

End Class