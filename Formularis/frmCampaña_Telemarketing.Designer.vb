<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCampaña_Telemarketing
    Inherits M_GenericForm.frmBase ' System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim UltraTab6 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab4 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab5 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab7 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim TimeRuler1 As DevExpress.XtraScheduler.TimeRuler = New DevExpress.XtraScheduler.TimeRuler()
        Dim TimeRuler2 As DevExpress.XtraScheduler.TimeRuler = New DevExpress.XtraScheduler.TimeRuler()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCampaña_Telemarketing))
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab8 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab3 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl6 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Contactos = New M_UltraGrid.m_UltraGrid()
        Me.L_Nombre_Contacto = New Infragistics.Win.Misc.UltraLabel()
        Me.L_Telefono_Contacto = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl4 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Seguimiento = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl5 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Seguimiento_Anterior = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl7 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Division = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Clientes_A_Llamar = New M_UltraGrid.m_UltraGrid()
        Me.TAB_Inferior = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage2 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.L_Usuario_Actual = New Infragistics.Win.Misc.UltraLabel()
        Me.B_Refrescar = New Infragistics.Win.Misc.UltraButton()
        Me.C_Estado_Seguimiento = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.L_Telefono_Empresa = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.SchedulerControl1 = New DevExpress.XtraScheduler.SchedulerControl()
        Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
        Me.ActiveViewBar1 = New DevExpress.XtraScheduler.UI.ActiveViewBar()
        Me.SwitchToDayViewItem1 = New DevExpress.XtraScheduler.UI.SwitchToDayViewItem()
        Me.SwitchToWorkWeekViewItem1 = New DevExpress.XtraScheduler.UI.SwitchToWorkWeekViewItem()
        Me.SwitchToWeekViewItem1 = New DevExpress.XtraScheduler.UI.SwitchToWeekViewItem()
        Me.SwitchToMonthViewItem1 = New DevExpress.XtraScheduler.UI.SwitchToMonthViewItem()
        Me.SwitchToTimelineViewItem1 = New DevExpress.XtraScheduler.UI.SwitchToTimelineViewItem()
        Me.SwitchToGanttViewItem1 = New DevExpress.XtraScheduler.UI.SwitchToGanttViewItem()
        Me.StandaloneBarDockControl1 = New DevExpress.XtraBars.StandaloneBarDockControl()
        Me.TimeScaleBar1 = New DevExpress.XtraScheduler.UI.TimeScaleBar()
        Me.SwitchTimeScalesItem1 = New DevExpress.XtraScheduler.UI.SwitchTimeScalesItem()
        Me.ChangeScaleWidthItem1 = New DevExpress.XtraScheduler.UI.ChangeScaleWidthItem()
        Me.RepositoryItemSpinEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
        Me.SwitchTimeScalesCaptionItem1 = New DevExpress.XtraScheduler.UI.SwitchTimeScalesCaptionItem()
        Me.LayoutBar1 = New DevExpress.XtraScheduler.UI.LayoutBar()
        Me.SwitchCompressWeekendItem1 = New DevExpress.XtraScheduler.UI.SwitchCompressWeekendItem()
        Me.SwitchShowWorkTimeOnlyItem1 = New DevExpress.XtraScheduler.UI.SwitchShowWorkTimeOnlyItem()
        Me.SwitchCellsAutoHeightItem1 = New DevExpress.XtraScheduler.UI.SwitchCellsAutoHeightItem()
        Me.ChangeSnapToCellsUIItem1 = New DevExpress.XtraScheduler.UI.ChangeSnapToCellsUIItem()
        Me.PrintBar1 = New DevExpress.XtraScheduler.UI.PrintBar()
        Me.PrintPreviewItem1 = New DevExpress.XtraScheduler.UI.PrintPreviewItem()
        Me.PrintItem1 = New DevExpress.XtraScheduler.UI.PrintItem()
        Me.PrintPageSetupItem1 = New DevExpress.XtraScheduler.UI.PrintPageSetupItem()
        Me.NavigatorBar1 = New DevExpress.XtraScheduler.UI.NavigatorBar()
        Me.NavigateViewBackwardItem1 = New DevExpress.XtraScheduler.UI.NavigateViewBackwardItem()
        Me.NavigateViewForwardItem1 = New DevExpress.XtraScheduler.UI.NavigateViewForwardItem()
        Me.GotoTodayItem1 = New DevExpress.XtraScheduler.UI.GotoTodayItem()
        Me.ViewZoomInItem1 = New DevExpress.XtraScheduler.UI.ViewZoomInItem()
        Me.ViewZoomOutItem1 = New DevExpress.XtraScheduler.UI.ViewZoomOutItem()
        Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
        Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
        Me.OpenScheduleItem1 = New DevExpress.XtraScheduler.UI.OpenScheduleItem()
        Me.SaveScheduleItem1 = New DevExpress.XtraScheduler.UI.SaveScheduleItem()
        Me.NewAppointmentItem1 = New DevExpress.XtraScheduler.UI.NewAppointmentItem()
        Me.NewRecurringAppointmentItem1 = New DevExpress.XtraScheduler.UI.NewRecurringAppointmentItem()
        Me.GroupByNoneItem1 = New DevExpress.XtraScheduler.UI.GroupByNoneItem()
        Me.GroupByDateItem1 = New DevExpress.XtraScheduler.UI.GroupByDateItem()
        Me.GroupByResourceItem1 = New DevExpress.XtraScheduler.UI.GroupByResourceItem()
        Me.SchedulerStorage1 = New DevExpress.XtraScheduler.SchedulerStorage(Me.components)
        Me.CCampañaSeguimientoBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.AbidosDataSet1 = New Abidos.AbidosDataSet1()
        Me.UltraTabPageControl8 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Exportacion = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.R_Observaciones = New M_RichText.M_RichText()
        Me.C_Campaña = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.L_Personal = New Infragistics.Win.Misc.UltraLabel()
        Me.TAB_General = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.SchedulerBarController1 = New DevExpress.XtraScheduler.UI.SchedulerBarController()
        Me.C_Campaña_SeguimientoTableAdapter = New Abidos.AbidosDataSet1TableAdapters.C_Campaña_SeguimientoTableAdapter()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.UltraTabPageControl6.SuspendLayout()
        Me.UltraTabPageControl4.SuspendLayout()
        Me.UltraTabPageControl5.SuspendLayout()
        Me.UltraTabPageControl7.SuspendLayout()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.TAB_Inferior, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_Inferior.SuspendLayout()
        CType(Me.C_Estado_Seguimiento, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl2.SuspendLayout()
        CType(Me.SchedulerControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemSpinEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SchedulerStorage1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CCampañaSeguimientoBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AbidosDataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl8.SuspendLayout()
        Me.UltraTabPageControl3.SuspendLayout()
        CType(Me.C_Campaña, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TAB_General, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_General.SuspendLayout()
        CType(Me.SchedulerBarController1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.Sortir
        Me.ToolForm.Size = New System.Drawing.Size(1207, 24)
        '
        'UltraTabPageControl6
        '
        Me.UltraTabPageControl6.Controls.Add(Me.GRD_Contactos)
        Me.UltraTabPageControl6.Controls.Add(Me.L_Nombre_Contacto)
        Me.UltraTabPageControl6.Controls.Add(Me.L_Telefono_Contacto)
        Me.UltraTabPageControl6.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl6.Name = "UltraTabPageControl6"
        Me.UltraTabPageControl6.Size = New System.Drawing.Size(1190, 467)
        '
        'GRD_Contactos
        '
        Me.GRD_Contactos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Contactos.Location = New System.Drawing.Point(0, 50)
        Me.GRD_Contactos.Name = "GRD_Contactos"
        Me.GRD_Contactos.pAccessibleName = Nothing
        Me.GRD_Contactos.pActivarBotonFiltro = False
        Me.GRD_Contactos.pText = " "
        Me.GRD_Contactos.Size = New System.Drawing.Size(1190, 417)
        Me.GRD_Contactos.TabIndex = 1
        '
        'L_Nombre_Contacto
        '
        Me.L_Nombre_Contacto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.L_Nombre_Contacto.Font = New System.Drawing.Font("Microsoft Sans Serif", 30.0!, System.Drawing.FontStyle.Bold)
        Me.L_Nombre_Contacto.Location = New System.Drawing.Point(14, -7)
        Me.L_Nombre_Contacto.Name = "L_Nombre_Contacto"
        Me.L_Nombre_Contacto.Size = New System.Drawing.Size(928, 42)
        Me.L_Nombre_Contacto.TabIndex = 220
        Me.L_Nombre_Contacto.Text = "Nombre"
        '
        'L_Telefono_Contacto
        '
        Me.L_Telefono_Contacto.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.L_Telefono_Contacto.Font = New System.Drawing.Font("Microsoft Sans Serif", 30.0!, System.Drawing.FontStyle.Bold)
        Me.L_Telefono_Contacto.Location = New System.Drawing.Point(948, -7)
        Me.L_Telefono_Contacto.Name = "L_Telefono_Contacto"
        Me.L_Telefono_Contacto.Size = New System.Drawing.Size(230, 42)
        Me.L_Telefono_Contacto.TabIndex = 219
        Me.L_Telefono_Contacto.Text = "934641939"
        '
        'UltraTabPageControl4
        '
        Me.UltraTabPageControl4.Controls.Add(Me.GRD_Seguimiento)
        Me.UltraTabPageControl4.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl4.Name = "UltraTabPageControl4"
        Me.UltraTabPageControl4.Size = New System.Drawing.Size(1190, 467)
        '
        'GRD_Seguimiento
        '
        Me.GRD_Seguimiento.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Seguimiento.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Seguimiento.Name = "GRD_Seguimiento"
        Me.GRD_Seguimiento.pAccessibleName = Nothing
        Me.GRD_Seguimiento.pActivarBotonFiltro = False
        Me.GRD_Seguimiento.pText = " "
        Me.GRD_Seguimiento.Size = New System.Drawing.Size(1190, 467)
        Me.GRD_Seguimiento.TabIndex = 2
        '
        'UltraTabPageControl5
        '
        Me.UltraTabPageControl5.Controls.Add(Me.GRD_Seguimiento_Anterior)
        Me.UltraTabPageControl5.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl5.Name = "UltraTabPageControl5"
        Me.UltraTabPageControl5.Size = New System.Drawing.Size(1190, 467)
        '
        'GRD_Seguimiento_Anterior
        '
        Me.GRD_Seguimiento_Anterior.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Seguimiento_Anterior.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Seguimiento_Anterior.Name = "GRD_Seguimiento_Anterior"
        Me.GRD_Seguimiento_Anterior.pAccessibleName = Nothing
        Me.GRD_Seguimiento_Anterior.pActivarBotonFiltro = False
        Me.GRD_Seguimiento_Anterior.pText = " "
        Me.GRD_Seguimiento_Anterior.Size = New System.Drawing.Size(1190, 467)
        Me.GRD_Seguimiento_Anterior.TabIndex = 3
        '
        'UltraTabPageControl7
        '
        Me.UltraTabPageControl7.Controls.Add(Me.GRD_Division)
        Me.UltraTabPageControl7.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl7.Name = "UltraTabPageControl7"
        Me.UltraTabPageControl7.Size = New System.Drawing.Size(1190, 467)
        '
        'GRD_Division
        '
        Me.GRD_Division.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Division.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Division.Name = "GRD_Division"
        Me.GRD_Division.pAccessibleName = Nothing
        Me.GRD_Division.pActivarBotonFiltro = False
        Me.GRD_Division.pText = " "
        Me.GRD_Division.Size = New System.Drawing.Size(1190, 467)
        Me.GRD_Division.TabIndex = 4
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Clientes_A_Llamar)
        Me.UltraTabPageControl1.Controls.Add(Me.TAB_Inferior)
        Me.UltraTabPageControl1.Controls.Add(Me.L_Usuario_Actual)
        Me.UltraTabPageControl1.Controls.Add(Me.B_Refrescar)
        Me.UltraTabPageControl1.Controls.Add(Me.C_Estado_Seguimiento)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel1)
        Me.UltraTabPageControl1.Controls.Add(Me.L_Telefono_Empresa)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(1206, 759)
        '
        'GRD_Clientes_A_Llamar
        '
        Me.GRD_Clientes_A_Llamar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Clientes_A_Llamar.Location = New System.Drawing.Point(6, 51)
        Me.GRD_Clientes_A_Llamar.Name = "GRD_Clientes_A_Llamar"
        Me.GRD_Clientes_A_Llamar.pAccessibleName = Nothing
        Me.GRD_Clientes_A_Llamar.pActivarBotonFiltro = False
        Me.GRD_Clientes_A_Llamar.pText = " "
        Me.GRD_Clientes_A_Llamar.Size = New System.Drawing.Size(1194, 197)
        Me.GRD_Clientes_A_Llamar.TabIndex = 0
        '
        'TAB_Inferior
        '
        Me.TAB_Inferior.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TAB_Inferior.Controls.Add(Me.UltraTabSharedControlsPage2)
        Me.TAB_Inferior.Controls.Add(Me.UltraTabPageControl4)
        Me.TAB_Inferior.Controls.Add(Me.UltraTabPageControl5)
        Me.TAB_Inferior.Controls.Add(Me.UltraTabPageControl6)
        Me.TAB_Inferior.Controls.Add(Me.UltraTabPageControl7)
        Me.TAB_Inferior.Location = New System.Drawing.Point(6, 254)
        Me.TAB_Inferior.Name = "TAB_Inferior"
        Me.TAB_Inferior.SharedControlsPage = Me.UltraTabSharedControlsPage2
        Me.TAB_Inferior.Size = New System.Drawing.Size(1194, 493)
        Me.TAB_Inferior.TabIndex = 2
        UltraTab6.Key = "Contactos"
        UltraTab6.TabPage = Me.UltraTabPageControl6
        UltraTab6.Text = "Contactos"
        UltraTab4.Key = "Actual"
        UltraTab4.TabPage = Me.UltraTabPageControl4
        UltraTab4.Text = "Seguimiento de la campaña actual"
        UltraTab5.Key = "Anterior"
        UltraTab5.TabPage = Me.UltraTabPageControl5
        UltraTab5.Text = "Seguimiento de campañas anteriores"
        UltraTab7.Key = "Division"
        UltraTab7.TabPage = Me.UltraTabPageControl7
        UltraTab7.Text = "División de interés"
        Me.TAB_Inferior.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab6, UltraTab4, UltraTab5, UltraTab7})
        '
        'UltraTabSharedControlsPage2
        '
        Me.UltraTabSharedControlsPage2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage2.Name = "UltraTabSharedControlsPage2"
        Me.UltraTabSharedControlsPage2.Size = New System.Drawing.Size(1190, 467)
        '
        'L_Usuario_Actual
        '
        Me.L_Usuario_Actual.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.L_Usuario_Actual.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.L_Usuario_Actual.Location = New System.Drawing.Point(434, 12)
        Me.L_Usuario_Actual.Name = "L_Usuario_Actual"
        Me.L_Usuario_Actual.Size = New System.Drawing.Size(511, 20)
        Me.L_Usuario_Actual.TabIndex = 216
        Me.L_Usuario_Actual.Text = "Última empresa contactada:"
        '
        'B_Refrescar
        '
        Me.B_Refrescar.Location = New System.Drawing.Point(345, 11)
        Me.B_Refrescar.Name = "B_Refrescar"
        Me.B_Refrescar.Size = New System.Drawing.Size(83, 21)
        Me.B_Refrescar.TabIndex = 1
        Me.B_Refrescar.Text = "Refrescar"
        '
        'C_Estado_Seguimiento
        '
        Me.C_Estado_Seguimiento.Location = New System.Drawing.Point(148, 11)
        Me.C_Estado_Seguimiento.Name = "C_Estado_Seguimiento"
        Me.C_Estado_Seguimiento.Size = New System.Drawing.Size(191, 21)
        Me.C_Estado_Seguimiento.TabIndex = 0
        Me.C_Estado_Seguimiento.Text = "C_Estado_Seguimiento"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(10, 15)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(152, 16)
        Me.UltraLabel1.TabIndex = 214
        Me.UltraLabel1.Text = "Estado de las empresas:"
        '
        'L_Telefono_Empresa
        '
        Me.L_Telefono_Empresa.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.L_Telefono_Empresa.Font = New System.Drawing.Font("Microsoft Sans Serif", 30.0!, System.Drawing.FontStyle.Bold)
        Me.L_Telefono_Empresa.Location = New System.Drawing.Point(967, -11)
        Me.L_Telefono_Empresa.Name = "L_Telefono_Empresa"
        Me.L_Telefono_Empresa.Size = New System.Drawing.Size(230, 42)
        Me.L_Telefono_Empresa.TabIndex = 218
        Me.L_Telefono_Empresa.Text = "934641939"
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.SchedulerControl1)
        Me.UltraTabPageControl2.Controls.Add(Me.StandaloneBarDockControl1)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(1206, 759)
        '
        'SchedulerControl1
        '
        Me.SchedulerControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SchedulerControl1.Location = New System.Drawing.Point(3, 34)
        Me.SchedulerControl1.MenuManager = Me.BarManager1
        Me.SchedulerControl1.Name = "SchedulerControl1"
        Me.SchedulerControl1.Size = New System.Drawing.Size(1191, 717)
        Me.SchedulerControl1.Start = New Date(2013, 5, 13, 0, 0, 0, 0)
        Me.SchedulerControl1.Storage = Me.SchedulerStorage1
        Me.SchedulerControl1.TabIndex = 1
        Me.SchedulerControl1.Text = "SchedulerControl1"
        TimeRuler1.TimeZoneId = "Romance Standard Time"
        TimeRuler1.UseClientTimeZone = False
        Me.SchedulerControl1.Views.DayView.TimeRulers.Add(TimeRuler1)
        TimeRuler2.TimeZoneId = "Romance Standard Time"
        TimeRuler2.UseClientTimeZone = False
        Me.SchedulerControl1.Views.WorkWeekView.TimeRulers.Add(TimeRuler2)
        '
        'BarManager1
        '
        Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.ActiveViewBar1, Me.TimeScaleBar1, Me.LayoutBar1, Me.PrintBar1, Me.NavigatorBar1})
        Me.BarManager1.DockControls.Add(Me.barDockControlTop)
        Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
        Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
        Me.BarManager1.DockControls.Add(Me.barDockControlRight)
        Me.BarManager1.DockControls.Add(Me.StandaloneBarDockControl1)
        Me.BarManager1.Form = Me
        Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.SwitchToDayViewItem1, Me.SwitchToWorkWeekViewItem1, Me.SwitchToWeekViewItem1, Me.SwitchToMonthViewItem1, Me.SwitchToTimelineViewItem1, Me.SwitchToGanttViewItem1, Me.SwitchTimeScalesItem1, Me.ChangeScaleWidthItem1, Me.SwitchTimeScalesCaptionItem1, Me.SwitchCompressWeekendItem1, Me.SwitchShowWorkTimeOnlyItem1, Me.SwitchCellsAutoHeightItem1, Me.ChangeSnapToCellsUIItem1, Me.OpenScheduleItem1, Me.SaveScheduleItem1, Me.PrintPreviewItem1, Me.PrintItem1, Me.PrintPageSetupItem1, Me.NewAppointmentItem1, Me.NewRecurringAppointmentItem1, Me.NavigateViewBackwardItem1, Me.NavigateViewForwardItem1, Me.GotoTodayItem1, Me.ViewZoomInItem1, Me.ViewZoomOutItem1, Me.GroupByNoneItem1, Me.GroupByDateItem1, Me.GroupByResourceItem1})
        Me.BarManager1.MaxItemId = 28
        Me.BarManager1.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemSpinEdit1})
        '
        'ActiveViewBar1
        '
        Me.ActiveViewBar1.Control = Me.SchedulerControl1
        Me.ActiveViewBar1.DockCol = 2
        Me.ActiveViewBar1.DockRow = 0
        Me.ActiveViewBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone
        Me.ActiveViewBar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchToDayViewItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchToWorkWeekViewItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchToWeekViewItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchToMonthViewItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchToTimelineViewItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchToGanttViewItem1)})
        Me.ActiveViewBar1.Offset = 291
        Me.ActiveViewBar1.OptionsBar.AllowQuickCustomization = False
        Me.ActiveViewBar1.StandaloneBarDockControl = Me.StandaloneBarDockControl1
        '
        'SwitchToDayViewItem1
        '
        Me.SwitchToDayViewItem1.Id = 0
        Me.SwitchToDayViewItem1.Name = "SwitchToDayViewItem1"
        '
        'SwitchToWorkWeekViewItem1
        '
        Me.SwitchToWorkWeekViewItem1.Id = 1
        Me.SwitchToWorkWeekViewItem1.Name = "SwitchToWorkWeekViewItem1"
        '
        'SwitchToWeekViewItem1
        '
        Me.SwitchToWeekViewItem1.Id = 2
        Me.SwitchToWeekViewItem1.Name = "SwitchToWeekViewItem1"
        '
        'SwitchToMonthViewItem1
        '
        Me.SwitchToMonthViewItem1.Id = 3
        Me.SwitchToMonthViewItem1.Name = "SwitchToMonthViewItem1"
        '
        'SwitchToTimelineViewItem1
        '
        Me.SwitchToTimelineViewItem1.Id = 4
        Me.SwitchToTimelineViewItem1.Name = "SwitchToTimelineViewItem1"
        '
        'SwitchToGanttViewItem1
        '
        Me.SwitchToGanttViewItem1.Id = 5
        Me.SwitchToGanttViewItem1.Name = "SwitchToGanttViewItem1"
        '
        'StandaloneBarDockControl1
        '
        Me.StandaloneBarDockControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StandaloneBarDockControl1.CausesValidation = False
        Me.StandaloneBarDockControl1.Location = New System.Drawing.Point(3, 3)
        Me.StandaloneBarDockControl1.Name = "StandaloneBarDockControl1"
        Me.StandaloneBarDockControl1.Size = New System.Drawing.Size(1191, 32)
        Me.StandaloneBarDockControl1.Text = "StandaloneBarDockControl1"
        '
        'TimeScaleBar1
        '
        Me.TimeScaleBar1.CanDockStyle = CType(((((DevExpress.XtraBars.BarCanDockStyle.Left Or DevExpress.XtraBars.BarCanDockStyle.Top) _
            Or DevExpress.XtraBars.BarCanDockStyle.Right) _
            Or DevExpress.XtraBars.BarCanDockStyle.Bottom) _
            Or DevExpress.XtraBars.BarCanDockStyle.Standalone), DevExpress.XtraBars.BarCanDockStyle)
        Me.TimeScaleBar1.Control = Me.SchedulerControl1
        Me.TimeScaleBar1.DockCol = 4
        Me.TimeScaleBar1.DockRow = 0
        Me.TimeScaleBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone
        Me.TimeScaleBar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchTimeScalesItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.ChangeScaleWidthItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchTimeScalesCaptionItem1)})
        Me.TimeScaleBar1.Offset = 2
        Me.TimeScaleBar1.OptionsBar.AllowQuickCustomization = False
        Me.TimeScaleBar1.OptionsBar.DisableClose = True
        Me.TimeScaleBar1.OptionsBar.DisableCustomization = True
        Me.TimeScaleBar1.StandaloneBarDockControl = Me.StandaloneBarDockControl1
        '
        'SwitchTimeScalesItem1
        '
        Me.SwitchTimeScalesItem1.Id = 6
        Me.SwitchTimeScalesItem1.Name = "SwitchTimeScalesItem1"
        '
        'ChangeScaleWidthItem1
        '
        Me.ChangeScaleWidthItem1.Edit = Me.RepositoryItemSpinEdit1
        Me.ChangeScaleWidthItem1.Id = 7
        Me.ChangeScaleWidthItem1.Name = "ChangeScaleWidthItem1"
        Me.ChangeScaleWidthItem1.UseCommandCaption = True
        '
        'RepositoryItemSpinEdit1
        '
        Me.RepositoryItemSpinEdit1.AutoHeight = False
        Me.RepositoryItemSpinEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.RepositoryItemSpinEdit1.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.[Default]
        Me.RepositoryItemSpinEdit1.MaxValue = New Decimal(New Integer() {200, 0, 0, 0})
        Me.RepositoryItemSpinEdit1.MinValue = New Decimal(New Integer() {10, 0, 0, 0})
        Me.RepositoryItemSpinEdit1.Name = "RepositoryItemSpinEdit1"
        '
        'SwitchTimeScalesCaptionItem1
        '
        Me.SwitchTimeScalesCaptionItem1.Id = 8
        Me.SwitchTimeScalesCaptionItem1.Name = "SwitchTimeScalesCaptionItem1"
        '
        'LayoutBar1
        '
        Me.LayoutBar1.Control = Me.SchedulerControl1
        Me.LayoutBar1.DockCol = 3
        Me.LayoutBar1.DockRow = 0
        Me.LayoutBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone
        Me.LayoutBar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchCompressWeekendItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchShowWorkTimeOnlyItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.SwitchCellsAutoHeightItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.ChangeSnapToCellsUIItem1)})
        Me.LayoutBar1.Offset = 478
        Me.LayoutBar1.OptionsBar.AllowQuickCustomization = False
        Me.LayoutBar1.StandaloneBarDockControl = Me.StandaloneBarDockControl1
        '
        'SwitchCompressWeekendItem1
        '
        Me.SwitchCompressWeekendItem1.Id = 9
        Me.SwitchCompressWeekendItem1.Name = "SwitchCompressWeekendItem1"
        '
        'SwitchShowWorkTimeOnlyItem1
        '
        Me.SwitchShowWorkTimeOnlyItem1.Id = 10
        Me.SwitchShowWorkTimeOnlyItem1.Name = "SwitchShowWorkTimeOnlyItem1"
        '
        'SwitchCellsAutoHeightItem1
        '
        Me.SwitchCellsAutoHeightItem1.Id = 11
        Me.SwitchCellsAutoHeightItem1.Name = "SwitchCellsAutoHeightItem1"
        '
        'ChangeSnapToCellsUIItem1
        '
        Me.ChangeSnapToCellsUIItem1.Id = 12
        Me.ChangeSnapToCellsUIItem1.Name = "ChangeSnapToCellsUIItem1"
        '
        'PrintBar1
        '
        Me.PrintBar1.Control = Me.SchedulerControl1
        Me.PrintBar1.DockCol = 0
        Me.PrintBar1.DockRow = 0
        Me.PrintBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone
        Me.PrintBar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.PrintPreviewItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PrintItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.PrintPageSetupItem1)})
        Me.PrintBar1.OptionsBar.AllowQuickCustomization = False
        Me.PrintBar1.StandaloneBarDockControl = Me.StandaloneBarDockControl1
        '
        'PrintPreviewItem1
        '
        Me.PrintPreviewItem1.Id = 13
        Me.PrintPreviewItem1.Name = "PrintPreviewItem1"
        '
        'PrintItem1
        '
        Me.PrintItem1.Id = 14
        Me.PrintItem1.Name = "PrintItem1"
        '
        'PrintPageSetupItem1
        '
        Me.PrintPageSetupItem1.Id = 15
        Me.PrintPageSetupItem1.Name = "PrintPageSetupItem1"
        '
        'NavigatorBar1
        '
        Me.NavigatorBar1.Control = Me.SchedulerControl1
        Me.NavigatorBar1.DockCol = 1
        Me.NavigatorBar1.DockRow = 0
        Me.NavigatorBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone
        Me.NavigatorBar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.NavigateViewBackwardItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.NavigateViewForwardItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.GotoTodayItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.ViewZoomInItem1), New DevExpress.XtraBars.LinkPersistInfo(Me.ViewZoomOutItem1)})
        Me.NavigatorBar1.Offset = 115
        Me.NavigatorBar1.StandaloneBarDockControl = Me.StandaloneBarDockControl1
        '
        'NavigateViewBackwardItem1
        '
        Me.NavigateViewBackwardItem1.Id = 18
        Me.NavigateViewBackwardItem1.Name = "NavigateViewBackwardItem1"
        '
        'NavigateViewForwardItem1
        '
        Me.NavigateViewForwardItem1.Id = 19
        Me.NavigateViewForwardItem1.Name = "NavigateViewForwardItem1"
        '
        'GotoTodayItem1
        '
        Me.GotoTodayItem1.Id = 20
        Me.GotoTodayItem1.Name = "GotoTodayItem1"
        '
        'ViewZoomInItem1
        '
        Me.ViewZoomInItem1.Id = 21
        Me.ViewZoomInItem1.Name = "ViewZoomInItem1"
        '
        'ViewZoomOutItem1
        '
        Me.ViewZoomOutItem1.Id = 22
        Me.ViewZoomOutItem1.Name = "ViewZoomOutItem1"
        '
        'barDockControlTop
        '
        Me.barDockControlTop.CausesValidation = False
        Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlTop.Size = New System.Drawing.Size(1234, 0)
        '
        'barDockControlBottom
        '
        Me.barDockControlBottom.CausesValidation = False
        Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.barDockControlBottom.Location = New System.Drawing.Point(0, 874)
        Me.barDockControlBottom.Size = New System.Drawing.Size(1234, 0)
        '
        'barDockControlLeft
        '
        Me.barDockControlLeft.CausesValidation = False
        Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
        Me.barDockControlLeft.Size = New System.Drawing.Size(0, 874)
        '
        'barDockControlRight
        '
        Me.barDockControlRight.CausesValidation = False
        Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.barDockControlRight.Location = New System.Drawing.Point(1234, 0)
        Me.barDockControlRight.Size = New System.Drawing.Size(0, 874)
        '
        'OpenScheduleItem1
        '
        Me.OpenScheduleItem1.Id = 16
        Me.OpenScheduleItem1.Name = "OpenScheduleItem1"
        '
        'SaveScheduleItem1
        '
        Me.SaveScheduleItem1.Id = 17
        Me.SaveScheduleItem1.Name = "SaveScheduleItem1"
        '
        'NewAppointmentItem1
        '
        Me.NewAppointmentItem1.Id = 23
        Me.NewAppointmentItem1.Name = "NewAppointmentItem1"
        '
        'NewRecurringAppointmentItem1
        '
        Me.NewRecurringAppointmentItem1.Id = 24
        Me.NewRecurringAppointmentItem1.Name = "NewRecurringAppointmentItem1"
        '
        'GroupByNoneItem1
        '
        Me.GroupByNoneItem1.Id = 25
        Me.GroupByNoneItem1.Name = "GroupByNoneItem1"
        '
        'GroupByDateItem1
        '
        Me.GroupByDateItem1.Id = 26
        Me.GroupByDateItem1.Name = "GroupByDateItem1"
        '
        'GroupByResourceItem1
        '
        Me.GroupByResourceItem1.Id = 27
        Me.GroupByResourceItem1.Name = "GroupByResourceItem1"
        '
        'SchedulerStorage1
        '
        Me.SchedulerStorage1.Appointments.CustomFieldMappings.Add(New DevExpress.XtraScheduler.AppointmentCustomFieldMapping("ColorB", "ColorB"))
        Me.SchedulerStorage1.Appointments.CustomFieldMappings.Add(New DevExpress.XtraScheduler.AppointmentCustomFieldMapping("ColorG", "ColorG"))
        Me.SchedulerStorage1.Appointments.CustomFieldMappings.Add(New DevExpress.XtraScheduler.AppointmentCustomFieldMapping("ColorR", "ColorR"))
        Me.SchedulerStorage1.Appointments.CustomFieldMappings.Add(New DevExpress.XtraScheduler.AppointmentCustomFieldMapping("Realizado", "Realizado"))
        Me.SchedulerStorage1.Appointments.DataSource = Me.CCampañaSeguimientoBindingSource
        Me.SchedulerStorage1.Appointments.Mappings.AppointmentId = "ID_Campaña_Cliente_Seguimiento"
        Me.SchedulerStorage1.Appointments.Mappings.Description = "Description"
        Me.SchedulerStorage1.Appointments.Mappings.End = "DataFi"
        Me.SchedulerStorage1.Appointments.Mappings.Start = "DataInici"
        Me.SchedulerStorage1.Appointments.Mappings.Subject = "Subject"
        '
        'CCampañaSeguimientoBindingSource
        '
        Me.CCampañaSeguimientoBindingSource.DataMember = "C_Campaña_Seguimiento"
        Me.CCampañaSeguimientoBindingSource.DataSource = Me.AbidosDataSet1
        '
        'AbidosDataSet1
        '
        Me.AbidosDataSet1.DataSetName = "AbidosDataSet1"
        Me.AbidosDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'UltraTabPageControl8
        '
        Me.UltraTabPageControl8.Controls.Add(Me.GRD_Exportacion)
        Me.UltraTabPageControl8.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl8.Name = "UltraTabPageControl8"
        Me.UltraTabPageControl8.Size = New System.Drawing.Size(1206, 759)
        '
        'GRD_Exportacion
        '
        Me.GRD_Exportacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Exportacion.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Exportacion.Name = "GRD_Exportacion"
        Me.GRD_Exportacion.pAccessibleName = Nothing
        Me.GRD_Exportacion.pActivarBotonFiltro = False
        Me.GRD_Exportacion.pText = " "
        Me.GRD_Exportacion.Size = New System.Drawing.Size(1206, 759)
        Me.GRD_Exportacion.TabIndex = 147
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Controls.Add(Me.R_Observaciones)
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(1206, 759)
        '
        'R_Observaciones
        '
        Me.R_Observaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.R_Observaciones.Location = New System.Drawing.Point(0, 0)
        Me.R_Observaciones.Name = "R_Observaciones"
        Me.R_Observaciones.pEnable = True
        Me.R_Observaciones.pText = resources.GetString("R_Observaciones.pText")
        Me.R_Observaciones.pTextEspecial = ""
        Me.R_Observaciones.Size = New System.Drawing.Size(1206, 759)
        Me.R_Observaciones.TabIndex = 4
        '
        'C_Campaña
        '
        Me.C_Campaña.Location = New System.Drawing.Point(131, 38)
        Me.C_Campaña.Name = "C_Campaña"
        Me.C_Campaña.Size = New System.Drawing.Size(745, 21)
        Me.C_Campaña.TabIndex = 0
        Me.C_Campaña.Text = "C_Campaña"
        '
        'L_Personal
        '
        Me.L_Personal.Location = New System.Drawing.Point(12, 43)
        Me.L_Personal.Name = "L_Personal"
        Me.L_Personal.Size = New System.Drawing.Size(152, 16)
        Me.L_Personal.TabIndex = 212
        Me.L_Personal.Text = "Campaña disponibles:"
        '
        'TAB_General
        '
        Me.TAB_General.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TAB_General.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl1)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl2)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl3)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl8)
        Me.TAB_General.Location = New System.Drawing.Point(12, 77)
        Me.TAB_General.Name = "TAB_General"
        Me.TAB_General.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.TAB_General.Size = New System.Drawing.Size(1210, 785)
        Me.TAB_General.TabIndex = 213
        UltraTab1.Key = "EmpresasALlamar"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Empresas a llamar"
        UltraTab2.Key = "Calendario"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Calendario"
        UltraTab8.Key = "Exportacion"
        UltraTab8.TabPage = Me.UltraTabPageControl8
        UltraTab8.Text = "Exportación"
        UltraTab3.Key = "Observaciones"
        UltraTab3.TabPage = Me.UltraTabPageControl3
        UltraTab3.Text = "Observaciones"
        Me.TAB_General.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2, UltraTab8, UltraTab3})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(1206, 759)
        '
        'SchedulerBarController1
        '
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchToDayViewItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchToWorkWeekViewItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchToWeekViewItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchToMonthViewItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchToTimelineViewItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchToGanttViewItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchTimeScalesItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.ChangeScaleWidthItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchTimeScalesCaptionItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchCompressWeekendItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchShowWorkTimeOnlyItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SwitchCellsAutoHeightItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.ChangeSnapToCellsUIItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.OpenScheduleItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.SaveScheduleItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.PrintPreviewItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.PrintItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.PrintPageSetupItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.NewAppointmentItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.NewRecurringAppointmentItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.NavigateViewBackwardItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.NavigateViewForwardItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.GotoTodayItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.ViewZoomInItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.ViewZoomOutItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.GroupByNoneItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.GroupByDateItem1)
        Me.SchedulerBarController1.BarItems.Add(Me.GroupByResourceItem1)
        Me.SchedulerBarController1.Control = Me.SchedulerControl1
        '
        'C_Campaña_SeguimientoTableAdapter
        '
        Me.C_Campaña_SeguimientoTableAdapter.ClearBeforeFill = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 60000
        '
        'frmCampaña_Telemarketing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1234, 874)
        Me.Controls.Add(Me.TAB_General)
        Me.Controls.Add(Me.C_Campaña)
        Me.Controls.Add(Me.L_Personal)
        Me.Controls.Add(Me.barDockControlLeft)
        Me.Controls.Add(Me.barDockControlRight)
        Me.Controls.Add(Me.barDockControlBottom)
        Me.Controls.Add(Me.barDockControlTop)
        Me.KeyPreview = True
        Me.Name = "frmCampaña_Telemarketing"
        Me.Text = "Telemarketing"
        Me.Controls.SetChildIndex(Me.barDockControlTop, 0)
        Me.Controls.SetChildIndex(Me.barDockControlBottom, 0)
        Me.Controls.SetChildIndex(Me.barDockControlRight, 0)
        Me.Controls.SetChildIndex(Me.barDockControlLeft, 0)
        Me.Controls.SetChildIndex(Me.L_Personal, 0)
        Me.Controls.SetChildIndex(Me.C_Campaña, 0)
        Me.Controls.SetChildIndex(Me.TAB_General, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.UltraTabPageControl6.ResumeLayout(False)
        Me.UltraTabPageControl4.ResumeLayout(False)
        Me.UltraTabPageControl5.ResumeLayout(False)
        Me.UltraTabPageControl7.ResumeLayout(False)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl1.PerformLayout()
        CType(Me.TAB_Inferior, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_Inferior.ResumeLayout(False)
        CType(Me.C_Estado_Seguimiento, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl2.ResumeLayout(False)
        CType(Me.SchedulerControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemSpinEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SchedulerStorage1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CCampañaSeguimientoBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AbidosDataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl8.ResumeLayout(False)
        Me.UltraTabPageControl3.ResumeLayout(False)
        CType(Me.C_Campaña, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TAB_General, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_General.ResumeLayout(False)
        CType(Me.SchedulerBarController1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents C_Campaña As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents L_Personal As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TAB_General As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents C_Estado_Seguimiento As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents GRD_Seguimiento As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Contactos As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Clientes_A_Llamar As M_UltraGrid.m_UltraGrid
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents R_Observaciones As M_RichText.M_RichText
    Friend WithEvents B_Refrescar As Infragistics.Win.Misc.UltraButton
    Friend WithEvents L_Usuario_Actual As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TAB_Inferior As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage2 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl4 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl5 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Seguimiento_Anterior As M_UltraGrid.m_UltraGrid
    Friend WithEvents SchedulerControl1 As DevExpress.XtraScheduler.SchedulerControl
    Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
    Friend WithEvents ActiveViewBar1 As DevExpress.XtraScheduler.UI.ActiveViewBar
    Friend WithEvents SwitchToDayViewItem1 As DevExpress.XtraScheduler.UI.SwitchToDayViewItem
    Friend WithEvents SwitchToWorkWeekViewItem1 As DevExpress.XtraScheduler.UI.SwitchToWorkWeekViewItem
    Friend WithEvents SwitchToWeekViewItem1 As DevExpress.XtraScheduler.UI.SwitchToWeekViewItem
    Friend WithEvents SwitchToMonthViewItem1 As DevExpress.XtraScheduler.UI.SwitchToMonthViewItem
    Friend WithEvents SwitchToTimelineViewItem1 As DevExpress.XtraScheduler.UI.SwitchToTimelineViewItem
    Friend WithEvents SwitchToGanttViewItem1 As DevExpress.XtraScheduler.UI.SwitchToGanttViewItem
    Friend WithEvents StandaloneBarDockControl1 As DevExpress.XtraBars.StandaloneBarDockControl
    Friend WithEvents TimeScaleBar1 As DevExpress.XtraScheduler.UI.TimeScaleBar
    Friend WithEvents SwitchTimeScalesItem1 As DevExpress.XtraScheduler.UI.SwitchTimeScalesItem
    Friend WithEvents ChangeScaleWidthItem1 As DevExpress.XtraScheduler.UI.ChangeScaleWidthItem
    Friend WithEvents RepositoryItemSpinEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit
    Friend WithEvents SwitchTimeScalesCaptionItem1 As DevExpress.XtraScheduler.UI.SwitchTimeScalesCaptionItem
    Friend WithEvents LayoutBar1 As DevExpress.XtraScheduler.UI.LayoutBar
    Friend WithEvents SwitchCompressWeekendItem1 As DevExpress.XtraScheduler.UI.SwitchCompressWeekendItem
    Friend WithEvents SwitchShowWorkTimeOnlyItem1 As DevExpress.XtraScheduler.UI.SwitchShowWorkTimeOnlyItem
    Friend WithEvents SwitchCellsAutoHeightItem1 As DevExpress.XtraScheduler.UI.SwitchCellsAutoHeightItem
    Friend WithEvents ChangeSnapToCellsUIItem1 As DevExpress.XtraScheduler.UI.ChangeSnapToCellsUIItem
    Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
    Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
    Friend WithEvents SchedulerStorage1 As DevExpress.XtraScheduler.SchedulerStorage
    Friend WithEvents SchedulerBarController1 As DevExpress.XtraScheduler.UI.SchedulerBarController
    Friend WithEvents PrintBar1 As DevExpress.XtraScheduler.UI.PrintBar
    Friend WithEvents PrintPreviewItem1 As DevExpress.XtraScheduler.UI.PrintPreviewItem
    Friend WithEvents PrintItem1 As DevExpress.XtraScheduler.UI.PrintItem
    Friend WithEvents PrintPageSetupItem1 As DevExpress.XtraScheduler.UI.PrintPageSetupItem
    Friend WithEvents OpenScheduleItem1 As DevExpress.XtraScheduler.UI.OpenScheduleItem
    Friend WithEvents SaveScheduleItem1 As DevExpress.XtraScheduler.UI.SaveScheduleItem
    Friend WithEvents UltraTabPageControl6 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl7 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents AbidosDataSet1 As Abidos.AbidosDataSet1
    Friend WithEvents CCampañaSeguimientoBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents C_Campaña_SeguimientoTableAdapter As Abidos.AbidosDataSet1TableAdapters.C_Campaña_SeguimientoTableAdapter
    Friend WithEvents GRD_Division As M_UltraGrid.m_UltraGrid
    Friend WithEvents L_Nombre_Contacto As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents L_Telefono_Contacto As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents L_Telefono_Empresa As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents NavigatorBar1 As DevExpress.XtraScheduler.UI.NavigatorBar
    Friend WithEvents NavigateViewBackwardItem1 As DevExpress.XtraScheduler.UI.NavigateViewBackwardItem
    Friend WithEvents NavigateViewForwardItem1 As DevExpress.XtraScheduler.UI.NavigateViewForwardItem
    Friend WithEvents GotoTodayItem1 As DevExpress.XtraScheduler.UI.GotoTodayItem
    Friend WithEvents ViewZoomInItem1 As DevExpress.XtraScheduler.UI.ViewZoomInItem
    Friend WithEvents ViewZoomOutItem1 As DevExpress.XtraScheduler.UI.ViewZoomOutItem
    Friend WithEvents NewAppointmentItem1 As DevExpress.XtraScheduler.UI.NewAppointmentItem
    Friend WithEvents NewRecurringAppointmentItem1 As DevExpress.XtraScheduler.UI.NewRecurringAppointmentItem
    Friend WithEvents GroupByNoneItem1 As DevExpress.XtraScheduler.UI.GroupByNoneItem
    Friend WithEvents GroupByDateItem1 As DevExpress.XtraScheduler.UI.GroupByDateItem
    Friend WithEvents GroupByResourceItem1 As DevExpress.XtraScheduler.UI.GroupByResourceItem
    Friend WithEvents UltraTabPageControl8 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Exportacion As M_UltraGrid.m_UltraGrid
End Class
