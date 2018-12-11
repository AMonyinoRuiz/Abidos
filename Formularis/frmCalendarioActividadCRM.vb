Imports DevExpress.XtraScheduler
Public Class frmCalendarioActividadCRM
    Dim oDTC As DTCDataContext

    Public Sub Entrada(Optional ByVal pFecha As Date = Nothing)
        Me.AplicarDisseny()
        oDTC = New DTCDataContext(BD.Conexion)

        ' Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Guardar configuración"

        Me.Calendari.ResourceNavigator.Visibility = DevExpress.XtraScheduler.ResourceNavigatorVisibility.Always
        Me.SchedulerStorage1.Resources.Mappings.Caption = "Nombre"
        Me.SchedulerStorage1.Resources.Mappings.Id = "ID_Personal"
        Me.SchedulerStorage1.Resources.DataSource = BD.RetornaDataTable("Select ID_Personal, Nombre From Personal Where Activo=1 and ActivarCalendario=1 and ID_Personal in (Select ID_PersonalACargo From Personal_PersonalACargo Where ID_Personal=" & Seguretat.oUser.ID_Personal & ") Order by Nombre")



        Me.C_Calendario_ActividadCRMTableAdapter.Connection = BD.Conexion
        Me.C_Calendario_ActividadCRMTableAdapter.Fill(Me.AbidosDataSet11.C_Calendario_ActividadCRM)

        If pFecha.Year = 1 Then
            Me.Calendari.GoToToday()
        Else
            Me.Calendari.GoToDate(pFecha)
        End If
        Me.Calendari.DayView.TopRowTime = New TimeSpan(7, 0, 0) 'DateTime.Now.TimeOfDay  
        Me.Calendari.GroupType = SchedulerGroupType.Resource
        Me.DateNavigator1.ShowTodayButton = True
        Me.DateNavigator1.ShowWeekNumbers = True



        Me.ResourcesCheckedListBoxControl1.UnCheckAll()
        'Me.ResourcesCheckedListBoxControl1.SetItemCheckState(0, CheckState.Checked)
        '  Me.ResourcesCheckedListBoxControl1.SelectedValue = "{" & oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Nombre & "}"
        'Me.SchedulerControl1.DayView.TimeScale = TimeSpan.FromMinutes(15)

        'Me.SchedulerStorage1.Resources.CustomFieldMapping
    End Sub

  
    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub frmCalendarioTecnico_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: esta línea de código carga datos en la tabla 'AbidosDataSet11.C_Calendario_ActividadCRM' Puede moverla o quitarla según sea necesario.
        Me.C_Calendario_ActividadCRMTableAdapter.Fill(Me.AbidosDataSet11.C_Calendario_ActividadCRM)
        'TODO: esta línea de código carga datos en la tabla 'AbidosDataSet11.Calendario_Operarios' Puede moverla o quitarla según sea necesario.
        'Me.SchedulerStorage1.Appointments.Mappings.PercentComplete = "ID_Parte"
    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Me.FormTancar()
    End Sub

    Private Sub Calendari_EditAppointmentFormShowing(sender As Object, e As DevExpress.XtraScheduler.AppointmentFormEventArgs) Handles Calendari.EditAppointmentFormShowing
        ' Dim _Fecha As Date = CDate(e.Appointment.Start.Day & "/" & e.Appointment.Start.Month & "/" & e.Appointment.Start.Year)
        ' Dim _IDInstalacion As Integer = e.Appointment.CustomFields(0).ToString

        ' Me.GRD.M.clsUltraGrid.Cargar("Select * From C_Calendario_MantenimientosPlanificados_Listado Where ID_Instalacion=" & _IDInstalacion & " and Fecha='" & _Fecha & "'", BD)
        Dim frm As New frmActividadCRM_Mantenimiento
        frm.Entrada(e.Appointment.CustomFields(0).ToString)
        frm.FormObrir(Me, True)

        e.Handled = True

    End Sub

End Class