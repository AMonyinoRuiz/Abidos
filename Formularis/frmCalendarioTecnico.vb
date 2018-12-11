Imports DevExpress.XtraScheduler
Public Class frmCalendarioTecnico
    Dim oDTC As DTCDataContext
    Enum Accio
        Afegir = 1
        Modificar = 2
        Eliminar = 3
    End Enum

    Public Sub Entrada(Optional ByVal pFecha As Date = Nothing)
        If Seguretat.oUser.ID_Personal.HasValue = False Then
            Mensaje.Mostrar_Mensaje("Imposible entrar en la pantalla, el usuario actual no tiene asociado ningún persona 'Personal'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Me.Visible = False
            Exit Sub
        End If


        Me.AplicarDisseny()
        oDTC = New DTCDataContext(BD.Conexion)

        Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Guardar configuración"

        ' Call CargarLlistat()
        'Me.Calendari.ActiveVie()
        'pepe.SameDay
        'pepe.LongerThanADay




        Me.Calendari.ResourceNavigator.Visibility = DevExpress.XtraScheduler.ResourceNavigatorVisibility.Always
        Me.SchedulerStorage1.Resources.DataSource = BD.RetornaDataTable("Select ID_Personal, Nombre From Personal Where Activo=1 and ActivarCalendario=1 and ID_Personal in (Select ID_PersonalACargo From Personal_PersonalACargo Where ID_Personal=" & Seguretat.oUser.ID_Personal & ") Order by Nombre")
        Me.SchedulerStorage1.Resources.Mappings.Caption = "Nombre"
        Me.SchedulerStorage1.Resources.Mappings.Id = "ID_Personal"

        'Me.SchedulerStorage1.Appointments.DataSource = "Select * From Calendario_Operarios"
        'Me.SchedulerStorage1.Appointments.Mappings.AllDay = "TodoElDia"
        'Me.SchedulerStorage1.Appointments.Mappings.AppointmentId = "ID_Calendario_Operarios"
        'Me.SchedulerStorage1.Appointments.Mappings.Description = "Descripcion"
        'Me.SchedulerStorage1.Appointments.Mappings.End = "FechaFin"
        'Me.SchedulerStorage1.Appointments.Mappings.Label = "Etiqueta"
        'Me.SchedulerStorage1.Appointments.Mappings.Location = "Localizacion"
        'Me.SchedulerStorage1.Appointments.Mappings.RecurrenceInfo = "RecurrenceInfo"
        'Me.SchedulerStorage1.Appointments.Mappings.ReminderInfo = "ReminderInfo"
        'Me.SchedulerStorage1.Appointments.Mappings.ResourceId = "ID_Personal"
        'Me.SchedulerStorage1.Appointments.Mappings.Start = "FechaInicio"
        'Me.SchedulerStorage1.Appointments.Mappings.Status = "Estado"
        'Me.SchedulerStorage1.Appointments.Mappings.Subject = "Asunto"
        'Me.SchedulerStorage1.Appointments.Mappings.Type = "Tipo"

        Me.Calendario_OperariosTableAdapter.Connection = BD.Conexion
        Me.Calendario_OperariosTableAdapter.Fill(Me.AbidosDataSet11.Calendario_Operarios)

        If pFecha.Year = 1 Then
            Me.Calendari.GoToToday()
        Else
            Me.Calendari.GoToDate(pFecha)
        End If
        Me.Calendari.DayView.TopRowTime = New TimeSpan(7, 0, 0) 'DateTime.Now.TimeOfDay  
        Me.Calendari.GroupType = SchedulerGroupType.Resource
        Me.DateNavigator1.ShowTodayButton = True
        Me.DateNavigator1.ShowWeekNumbers = True


        'Me.SchedulerControl1.DayView.TimeScale = TimeSpan.FromMinutes(15)

        'Me.SchedulerStorage1.Resources.CustomFieldMapping
    End Sub

    'Private Sub CargarLlistat()
    '    'Me.GridView1.ShowPrintPreview

    '    Dim DT As DataTable
    '    DT = BD.RetornaDataTable("Select * From C_CalendarioTecnico_Partes_Pendientes  Order by FechaInicio desc")
    '    Me.GRID.DataSource = DT

    '    With Me.GridView1
    '        .OptionsView.ShowFooter = True
    '        .ShowFindPanel()

    '        .OptionsView.ShowAutoFilterRow = False
    '        .OptionsView.ShowDetailButtons = False
    '        .OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
    '        .OptionsView.ShowGroupedColumns = False
    '        .OptionsView.ShowGroupPanel = False
    '        .OptionsView.ShowIndicator = False
    '        'Me.GridView1.OptionsView.ShowViewCaption = True
    '        ' Me.GridView1.ShowFilterEditor(Me.GridView1.Columns(0))
    '        '.FormatConditions.View.ShowEditor()

    '        .OptionsView.ShowFooter = False
    '        .OptionsBehavior.Editable = False
    '        .OptionsFind.AlwaysVisible = True
    '        .OptionsFind.FindDelay = 500

    '        '.OptionsBehavior.Editable = False
    '        '.OptionsSelection.EnableAppearanceFocusedCell = False
    '        '.OptionsSelection.EnableAppearanceFocusedRow = False
    '        '.OptionsSelection.EnableAppearanceHideSelection = False



    '        'Me.GridView1.FocusRectStyle = DrawFocusRectStyle.RowFocus
    '        'Me.GridView1.HideEditor()
    '        'Me.GridView1.ShowButtonMode = Views.Base.ShowButtonModeEnum.ShowForFocusedRow
    '        'Me.GridView1.OptionsBehavior.ReadOnly = True

    '        'Me.GridView1.Columns(0).OptionsColumn.AllowEdit = False
    '        'Me.GridView1.ShowLoadingPanel()
    '        'Me.GridView1.ShowUnboundExpressionEditor(Me.GridView1.Columns(0))

    '        Call Aplicar_ConfiguracionsAlNouGrid()
    '    End With
    'End Sub

    'Private Sub Aplicar_ConfiguracionsAlNouGrid()
    '    Try
    '        With Me.GridView1
    '            'Dim _LlistatConfiguracions As IEnumerable(Of GRID_Columna) = oDTC.GRID_Columna.Where(Function(F) F.Formulari_name = "frmCreadorListados" And F.Formulari_AccessibleName = CStr(oLinqLlistat.ID_ListadoADV))
    '            'Dim _Configuracio As GRID_Columna
    '            '.OptionsView.ColumnAutoWidth = False

    '            'For Each _Configuracio In _LlistatConfiguracions
    '            '    If .Columns(_Configuracio.Columna_Key) Is Nothing = False Then
    '            '        .Columns(_Configuracio.Columna_Key).Caption = _Configuracio.Columna_Caption
    '            '        .Columns(_Configuracio.Columna_Key).VisibleIndex = _Configuracio.Columna_Position
    '            '        .Columns(_Configuracio.Columna_Key).Width = _Configuracio.Columna_Width
    '            '        .Columns(_Configuracio.Columna_Key).Width = _Configuracio.Columna_Width
    '            '        .Columns(_Configuracio.Columna_Key).Visible = _Configuracio.Columna_Visible
    '            '        Select Case _Configuracio.Columna_Ordenacion
    '            '            Case 1
    '            '                .Columns(_Configuracio.Columna_Key).SortOrder = DevExpress.Data.ColumnSortOrder.None
    '            '            Case 2
    '            '                .Columns(_Configuracio.Columna_Key).SortOrder = DevExpress.Data.ColumnSortOrder.Ascending
    '            '            Case 3
    '            '                .Columns(_Configuracio.Columna_Key).SortOrder = DevExpress.Data.ColumnSortOrder.Descending
    '            '        End Select

    '            '        .Columns(_Configuracio.Columna_Key).DisplayFormat.FormatType = FormatType.Custom
    '            '        If _Configuracio.GRID_Columna_Format Is Nothing = False Then
    '            '            .Columns(_Configuracio.Columna_Key).DisplayFormat.FormatString = _Configuracio.GRID_Columna_Format.Format
    '            '        End If
    '            '    End If
    '            'Next
    '        End With

    '    Catch ex As Exception
    '        MsgBox("Error en Aplicar_ConfiguracionsAlNouGrid", MsgBoxStyle.Critical)
    '    End Try
    'End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub Calendari_EditAppointmentFormShowing(sender As Object, e As DevExpress.XtraScheduler.AppointmentFormEventArgs) Handles Calendari.EditAppointmentFormShowing
        Dim scheduler As DevExpress.XtraScheduler.SchedulerControl = CType(sender, DevExpress.XtraScheduler.SchedulerControl)
        Dim form As Abidos.Scheduler13._1_TestDesignTime.AppointmentFormOutlook2007Style = New Abidos.Scheduler13._1_TestDesignTime.AppointmentFormOutlook2007Style(scheduler, e.Appointment, e.OpenRecurrenceForm)
        Try
            e.DialogResult = form.ShowDialog
            e.Handled = True
        Finally
            form.Dispose()
        End Try

    End Sub

    Private Sub SchedulerStorage1_AppointmentsChanged(sender As Object, e As DevExpress.XtraScheduler.PersistentObjectsEventArgs) Handles SchedulerStorage1.AppointmentsChanged
        Dim _Appointment As DevExpress.XtraScheduler.Appointment
        For Each _Appointment In e.Objects
            Call AlFerAlgoAmbUnAppointment(_Appointment, Accio.Modificar)
        Next
    End Sub

    Private Sub SchedulerStorage1_AppointmentsDeleted(sender As Object, e As DevExpress.XtraScheduler.PersistentObjectsEventArgs) Handles SchedulerStorage1.AppointmentsDeleted
        Dim _Appointment As DevExpress.XtraScheduler.Appointment
        For Each _Appointment In e.Objects
            Call AlFerAlgoAmbUnAppointment(_Appointment, Accio.Eliminar)
        Next

    End Sub

    Private Sub SchedulerStorage1_AppointmentsInserted(sender As Object, e As DevExpress.XtraScheduler.PersistentObjectsEventArgs) Handles SchedulerStorage1.AppointmentsInserted
        Dim _Appointment As DevExpress.XtraScheduler.Appointment
        For Each _Appointment In e.Objects
            Call AlFerAlgoAmbUnAppointment(_Appointment, Accio.Afegir)
        Next

    End Sub

    Private Sub AlFerAlgoAmbUnAppointment(ByRef pAppointment As Appointment, ByVal pAccio As Accio)
        Dim _IDAppointment As Integer
        'Dim _IDParte As Integer
        Dim _pepe As CustomFieldCollection
        _pepe = pAppointment.CustomFields
        _IDAppointment = _pepe("IDCalendarioOperarios")
        'If IsDBNull(_pepe("IDParte")) = True Then
        '    _IDParte = 0
        'Else
        '    _IDParte = _pepe("IDParte")
        'End If

        Dim _Appointment As Calendario_Operarios

        Select Case pAccio
            Case Accio.Afegir
                _Appointment = New Calendario_Operarios



            Case Accio.Modificar
                _Appointment = oDTC.Calendario_Operarios.Where(Function(F) F.ID_Calendario_Operarios = _IDAppointment).FirstOrDefault

            Case Accio.Eliminar
                oDTC.Calendario_Operarios.DeleteOnSubmit(oDTC.Calendario_Operarios.Where(Function(F) F.ID_Calendario_Operarios = _IDAppointment).FirstOrDefault)
                oDTC.SubmitChanges()
                Exit Sub
        End Select

        _Appointment.Asunto = pAppointment.Subject
        _Appointment.Descripcion = pAppointment.Description
        _Appointment.Etiqueta = pAppointment.LabelId
        _Appointment.FechaFin = pAppointment.End
        _Appointment.FechaInicio = pAppointment.Start
        _Appointment.Localizacion = pAppointment.Location
        If IsNumeric(pAppointment.ResourceId) Then
            Dim _IDPersonal As Integer = pAppointment.ResourceId
            _Appointment.Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = _IDPersonal).FirstOrDefault
        Else
            _Appointment.Personal = Nothing
        End If

        If pAppointment.PercentComplete = 0 Then
            _Appointment.ID_Parte = Nothing
        Else
            Dim _IDParte As Integer = pAppointment.PercentComplete
            _Appointment.Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = _IDParte).FirstOrDefault
        End If

        _Appointment.TodoElDia = pAppointment.AllDay

        If pAccio = Accio.Afegir Then
            oDTC.Calendario_Operarios.InsertOnSubmit(_Appointment)
        End If

        oDTC.SubmitChanges()

        If pAccio = Accio.Afegir Then 'Li pasem al appointment el ID del registre creat
            _pepe("IDCalendarioOperarios") = _Appointment.ID_Calendario_Operarios
        End If


    End Sub

    Private Sub frmCalendarioTecnico_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: esta línea de código carga datos en la tabla 'AbidosDataSet11.Calendario_Operarios' Puede moverla o quitarla según sea necesario.
        Me.SchedulerStorage1.Appointments.Mappings.PercentComplete = "ID_Parte"
        Me.Calendario_OperariosTableAdapter.Fill(Me.AbidosDataSet11.Calendario_Operarios)

    End Sub

    Private Sub BarButtonItem1_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem1.ItemClick
        Me.FormTancar()
    End Sub


End Class