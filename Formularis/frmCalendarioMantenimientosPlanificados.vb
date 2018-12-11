Imports DevExpress.XtraScheduler.Drawing
Imports DevExpress.XtraScheduler

Public Class frmCalendarioMantenimientosPlanificados
    Public Sub Entrada()
        If Seguretat.oUser.ID_Personal.HasValue = False Then
            Mensaje.Mostrar_Mensaje("Imposible entrar en la pantalla, el usuario actual no tiene asociado ningún persona 'Personal'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Me.Visible = False
            Exit Sub
        End If

        Me.AplicarDisseny()

        Me.Calendari.ResourceNavigator.Visibility = DevExpress.XtraScheduler.ResourceNavigatorVisibility.Always

        Me.C_Calendario_MantenimientosPlanificadosTableAdapter1.Connection = BD.Conexion
        Me.C_Calendario_MantenimientosPlanificadosTableAdapter1.Fill(Me.AbidosDataSetMantenimientosPlanificados.C_Calendario_MantenimientosPlanificados)

        Me.Calendari.GoToToday()

        Me.Calendari.DayView.TopRowTime = New TimeSpan(7, 0, 0) 'DateTime.Now.TimeOfDay  

        Me.GRD.M.clsToolBar.Boto_Afegir("IrAlParte", "Ir al parte", True)

        'Me.DateNavigator1.ShowTodayButton = True
        'Me.DateNavigator1.ShowWeekNumbers = True
    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Me.FormTancar()
    End Sub

    Private Sub Calendari_CustomDrawAppointment(sender As Object, e As DevExpress.XtraScheduler.CustomDrawObjectEventArgs) Handles Calendari.CustomDrawAppointment
        'MsgBox("a")
    End Sub

    Private Sub Calendari_EditAppointmentFormShowing(sender As Object, e As DevExpress.XtraScheduler.AppointmentFormEventArgs) Handles Calendari.EditAppointmentFormShowing
        Dim _Fecha As Date = CDate(e.Appointment.Start.Day & "/" & e.Appointment.Start.Month & "/" & e.Appointment.Start.Year)
        Dim _IDInstalacion As Integer = e.Appointment.CustomFields(0).ToString

        Me.GRD.M.clsUltraGrid.Cargar("Select * From C_Calendario_MantenimientosPlanificados_Listado Where ID_Instalacion=" & _IDInstalacion & " and Fecha='" & _Fecha & "'", BD)

        e.Handled = True

    End Sub

    Private Sub Calendari_MouseDown(sender As Object, e As MouseEventArgs) Handles Calendari.MouseDown
        Dim pos As New Point(e.X, e.Y)
        Dim viewInfo As SchedulerViewInfoBase = Calendari.ActiveView.ViewInfo
        Dim hitInfo As SchedulerHitInfo = viewInfo.CalcHitInfo(pos, False)

        If hitInfo.HitTest = SchedulerHitTest.AppointmentContent Then
            Dim apt As Appointment = (CType(hitInfo.ViewInfo, AppointmentViewInfo)).Appointment

            Dim _Fecha As Date = CDate(apt.Start.Day & "/" & apt.Start.Month & "/" & apt.Start.Year)
            Dim _IDInstalacion As Integer = apt.CustomFields(0).ToString

            Me.GRD.M.clsUltraGrid.Cargar("Select * From C_Calendario_MantenimientosPlanificados_Listado Where ID_Instalacion=" & _IDInstalacion & " and Fecha='" & _Fecha & "'", BD)

            Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
            For Each pRow In Me.GRD.GRID.Rows
                If pRow.Cells("ID_Parte").Value.ToString.Length <> 0 Then
                    pRow.CellAppearance.BackColor = Color.LightGreen
                End If
            Next

        Else
            Me.GRD.M.clsUltraGrid.Cargar("Select * From C_Calendario_MantenimientosPlanificados_Listado Where ID_Instalacion=0", BD)
        End If


    End Sub

    Private Sub GRD_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD.M_ToolGrid_ToolClickBotonsExtras2
        Select Case e.Tool.Key
            Case "IrAlParte"
                If Me.GRD.GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If

                If Me.GRD.GRID.Selected.Rows(0).Cells("ID_Parte").Value.ToString.Length <> 0 Then
                    Dim _IDParte As Integer = Me.GRD.GRID.Selected.Rows(0).Cells("ID_Parte").Value

                    Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                    frm.Entrada(_IDParte)
                    frm.FormObrir(frmPrincipal)
                End If
        End Select
    End Sub

    Private Sub frmCalendarioMantenimientosPlanificados_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: esta línea de código carga datos en la tabla 'AbidosDataSetMantenimientosPlanificados.C_Calendario_MantenimientosPlanificados' Puede moverla o quitarla según sea necesario.
        Me.C_Calendario_MantenimientosPlanificadosTableAdapter1.Fill(Me.AbidosDataSetMantenimientosPlanificados.C_Calendario_MantenimientosPlanificados)

    End Sub
End Class