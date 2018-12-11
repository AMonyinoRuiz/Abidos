<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmActividadCRM_Principal
    ' Inherits System.Windows.Forms.Form
    Inherits M_GenericForm.frmBase

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmActividadCRM_Principal))
        Dim GridFormatRule1 As DevExpress.XtraGrid.GridFormatRule = New DevExpress.XtraGrid.GridFormatRule()
        Dim FormatConditionRuleValue1 As DevExpress.XtraEditors.FormatConditionRuleValue = New DevExpress.XtraEditors.FormatConditionRuleValue()
        Dim GridFormatRule2 As DevExpress.XtraGrid.GridFormatRule = New DevExpress.XtraGrid.GridFormatRule()
        Dim FormatConditionRuleValue2 As DevExpress.XtraEditors.FormatConditionRuleValue = New DevExpress.XtraEditors.FormatConditionRuleValue()
        Dim GridFormatRule3 As DevExpress.XtraGrid.GridFormatRule = New DevExpress.XtraGrid.GridFormatRule()
        Dim FormatConditionRuleValue3 As DevExpress.XtraEditors.FormatConditionRuleValue = New DevExpress.XtraEditors.FormatConditionRuleValue()
        Dim GridFormatRule4 As DevExpress.XtraGrid.GridFormatRule = New DevExpress.XtraGrid.GridFormatRule()
        Dim FormatConditionRuleValue4 As DevExpress.XtraEditors.FormatConditionRuleValue = New DevExpress.XtraEditors.FormatConditionRuleValue()
        Dim GridFormatRule5 As DevExpress.XtraGrid.GridFormatRule = New DevExpress.XtraGrid.GridFormatRule()
        Dim FormatConditionRuleValue5 As DevExpress.XtraEditors.FormatConditionRuleValue = New DevExpress.XtraEditors.FormatConditionRuleValue()
        Me.Col_Finalizada = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Col_EstaFinalizada = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Col_EsSoloSeguimiento = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Col_ALaEsperaRespuesta = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Col_Leido = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn16 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repositoryItemImageComboBox2 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.imageCollection1 = New DevExpress.Utils.ImageCollection(Me.components)
        Me.repositoryItemImageComboBox1 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.repositoryItemImageComboBox3 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.repositoryItemImageComboBox4 = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.gridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.gridColumn2 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.gridColumn3 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.gridColumn4 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.gridColumn5 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.gridColumn6 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.gridColumn7 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GRD_Actividades = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.GridColumn15 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn17 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn23 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn18 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn19 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn20 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn22 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn21 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn24 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Col_Cliente = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Col_Solucion = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Col_PorcentajeFinalizada = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepositoryItemProgressBar1 = New DevExpress.XtraEditors.Repository.RepositoryItemProgressBar()
        Me.ilColumns = New System.Windows.Forms.ImageList(Me.components)
        Me.GridColumn8 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn9 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn10 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn11 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn12 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn13 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn14 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.B_VisualizarTodos = New DevExpress.XtraEditors.CheckButton()
        Me.GRD_Chat = New DevExpress.XtraGrid.GridControl()
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.ID_ActividadCRM_Chat = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ID_ActividadCRM = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.FechaAlta = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.De = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Para = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Mensaje = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Asunto = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn25 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.B_VisualizarSeguimiento = New DevExpress.XtraEditors.CheckButton()
        Me.B_ALaEsperaDeRespuesta = New DevExpress.XtraEditors.CheckButton()
        Me.B_CrearActividad = New DevExpress.XtraEditors.SimpleButton()
        Me.B_Actualizar = New DevExpress.XtraEditors.SimpleButton()
        Me.B_VisualizarPendientes = New DevExpress.XtraEditors.CheckButton()
        Me.B_Chat_Todos = New DevExpress.XtraEditors.CheckButton()
        Me.L_NumRows = New DevExpress.XtraEditors.LabelControl()
        CType(Me.repositoryItemImageComboBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imageCollection1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repositoryItemImageComboBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repositoryItemImageComboBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GRD_Actividades, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemProgressBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GRD_Chat, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Location = New System.Drawing.Point(24, -3)
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.Sortir
        Me.ToolForm.Size = New System.Drawing.Size(1192, 24)
        Me.ToolForm.Visible = False
        '
        'Col_Finalizada
        '
        Me.Col_Finalizada.Caption = "Finalizada"
        Me.Col_Finalizada.FieldName = "Finalizada"
        Me.Col_Finalizada.Name = "Col_Finalizada"
        '
        'Col_EstaFinalizada
        '
        Me.Col_EstaFinalizada.Caption = "EstaFinalizada"
        Me.Col_EstaFinalizada.FieldName = "EstaFinalizada"
        Me.Col_EstaFinalizada.Name = "Col_EstaFinalizada"
        '
        'Col_EsSoloSeguimiento
        '
        Me.Col_EsSoloSeguimiento.Caption = "EsSoloSeguimiento"
        Me.Col_EsSoloSeguimiento.FieldName = "EsSoloSeguimiento"
        Me.Col_EsSoloSeguimiento.Name = "Col_EsSoloSeguimiento"
        '
        'Col_ALaEsperaRespuesta
        '
        Me.Col_ALaEsperaRespuesta.Caption = "EsALaEsperaRespuesta"
        Me.Col_ALaEsperaRespuesta.FieldName = "EsALaEsperaRespuesta"
        Me.Col_ALaEsperaRespuesta.Name = "Col_ALaEsperaRespuesta"
        '
        'Col_Leido
        '
        Me.Col_Leido.Caption = "Leído"
        Me.Col_Leido.FieldName = "Leido"
        Me.Col_Leido.Name = "Col_Leido"
        Me.Col_Leido.Width = 99
        '
        'GridColumn16
        '
        Me.GridColumn16.Caption = "Read"
        Me.GridColumn16.ColumnEdit = Me.repositoryItemImageComboBox2
        Me.GridColumn16.FieldName = "Leido"
        Me.GridColumn16.ImageIndex = 1
        Me.GridColumn16.Name = "GridColumn16"
        Me.GridColumn16.OptionsColumn.AllowEdit = False
        Me.GridColumn16.OptionsColumn.AllowFocus = False
        Me.GridColumn16.OptionsColumn.AllowSize = False
        Me.GridColumn16.OptionsColumn.FixedWidth = True
        Me.GridColumn16.OptionsColumn.ShowCaption = False
        Me.GridColumn16.ToolTip = "Icon"
        Me.GridColumn16.Visible = True
        Me.GridColumn16.VisibleIndex = 1
        Me.GridColumn16.Width = 25
        '
        'repositoryItemImageComboBox2
        '
        Me.repositoryItemImageComboBox2.AutoHeight = False
        Me.repositoryItemImageComboBox2.GlyphAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.repositoryItemImageComboBox2.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Unread", 0, 3), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Read", 1, 2)})
        Me.repositoryItemImageComboBox2.Name = "repositoryItemImageComboBox2"
        Me.repositoryItemImageComboBox2.SmallImages = Me.imageCollection1
        '
        'imageCollection1
        '
        Me.imageCollection1.ImageSize = New System.Drawing.Size(13, 13)
        Me.imageCollection1.ImageStream = CType(resources.GetObject("imageCollection1.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        Me.imageCollection1.Images.SetKeyName(7, "NoDate_Flag.png")
        Me.imageCollection1.Images.SetKeyName(8, "ThisWeek_Flag.png")
        Me.imageCollection1.Images.SetKeyName(9, "Today_Flag.png")
        '
        'repositoryItemImageComboBox1
        '
        Me.repositoryItemImageComboBox1.AutoHeight = False
        Me.repositoryItemImageComboBox1.GlyphAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.repositoryItemImageComboBox1.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Alta", 1, 9), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Media", 2, 8), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Baja", 3, 7)})
        Me.repositoryItemImageComboBox1.Name = "repositoryItemImageComboBox1"
        Me.repositoryItemImageComboBox1.SmallImages = Me.imageCollection1
        '
        'repositoryItemImageComboBox3
        '
        Me.repositoryItemImageComboBox3.AutoHeight = False
        Me.repositoryItemImageComboBox3.GlyphAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.repositoryItemImageComboBox3.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Attachment", 1, 4), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Empty", 0, -1)})
        Me.repositoryItemImageComboBox3.Name = "repositoryItemImageComboBox3"
        Me.repositoryItemImageComboBox3.SmallImages = Me.imageCollection1
        '
        'repositoryItemImageComboBox4
        '
        Me.repositoryItemImageComboBox4.AutoHeight = False
        Me.repositoryItemImageComboBox4.GlyphAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.repositoryItemImageComboBox4.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Read", 1, 6), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Unread", 0, 5)})
        Me.repositoryItemImageComboBox4.Name = "repositoryItemImageComboBox4"
        Me.repositoryItemImageComboBox4.SmallImages = Me.imageCollection1
        '
        'gridColumn1
        '
        Me.gridColumn1.Caption = "Priority"
        Me.gridColumn1.ColumnEdit = Me.repositoryItemImageComboBox1
        Me.gridColumn1.FieldName = "Priority"
        Me.gridColumn1.ImageIndex = 0
        Me.gridColumn1.Name = "gridColumn1"
        Me.gridColumn1.OptionsColumn.AllowFocus = False
        Me.gridColumn1.OptionsColumn.AllowSize = False
        Me.gridColumn1.OptionsColumn.FixedWidth = True
        Me.gridColumn1.OptionsColumn.ShowCaption = False
        Me.gridColumn1.ToolTip = "Importance"
        Me.gridColumn1.Visible = True
        Me.gridColumn1.VisibleIndex = 0
        Me.gridColumn1.Width = 25
        '
        'gridColumn2
        '
        Me.gridColumn2.Caption = "Read"
        Me.gridColumn2.ColumnEdit = Me.repositoryItemImageComboBox2
        Me.gridColumn2.FieldName = "Read"
        Me.gridColumn2.ImageIndex = 1
        Me.gridColumn2.Name = "gridColumn2"
        Me.gridColumn2.OptionsColumn.AllowEdit = False
        Me.gridColumn2.OptionsColumn.AllowFocus = False
        Me.gridColumn2.OptionsColumn.AllowSize = False
        Me.gridColumn2.OptionsColumn.FixedWidth = True
        Me.gridColumn2.OptionsColumn.ShowCaption = False
        Me.gridColumn2.ToolTip = "Icon"
        Me.gridColumn2.Visible = True
        Me.gridColumn2.VisibleIndex = 1
        Me.gridColumn2.Width = 25
        '
        'gridColumn3
        '
        Me.gridColumn3.Caption = "Attachment"
        Me.gridColumn3.ColumnEdit = Me.repositoryItemImageComboBox3
        Me.gridColumn3.FieldName = "Attachment"
        Me.gridColumn3.ImageIndex = 2
        Me.gridColumn3.Name = "gridColumn3"
        Me.gridColumn3.OptionsColumn.AllowEdit = False
        Me.gridColumn3.OptionsColumn.AllowFocus = False
        Me.gridColumn3.OptionsColumn.AllowSize = False
        Me.gridColumn3.OptionsColumn.FixedWidth = True
        Me.gridColumn3.OptionsColumn.ShowCaption = False
        Me.gridColumn3.ToolTip = "Attachment"
        Me.gridColumn3.Visible = True
        Me.gridColumn3.VisibleIndex = 2
        Me.gridColumn3.Width = 25
        '
        'gridColumn4
        '
        Me.gridColumn4.Caption = "Subject"
        Me.gridColumn4.FieldName = "Subject"
        Me.gridColumn4.Name = "gridColumn4"
        Me.gridColumn4.OptionsColumn.AllowFocus = False
        Me.gridColumn4.Visible = True
        Me.gridColumn4.VisibleIndex = 3
        Me.gridColumn4.Width = 326
        '
        'gridColumn5
        '
        Me.gridColumn5.Caption = "From"
        Me.gridColumn5.FieldName = "From"
        Me.gridColumn5.Name = "gridColumn5"
        Me.gridColumn5.OptionsColumn.AllowFocus = False
        Me.gridColumn5.Visible = True
        Me.gridColumn5.VisibleIndex = 4
        Me.gridColumn5.Width = 113
        '
        'gridColumn6
        '
        Me.gridColumn6.Caption = "Received"
        Me.gridColumn6.FieldName = "Date"
        Me.gridColumn6.GroupInterval = DevExpress.XtraGrid.ColumnGroupInterval.DateRange
        Me.gridColumn6.Name = "gridColumn6"
        Me.gridColumn6.OptionsColumn.AllowFocus = False
        Me.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.DateAlt
        Me.gridColumn6.Visible = True
        Me.gridColumn6.VisibleIndex = 5
        Me.gridColumn6.Width = 92
        '
        'gridColumn7
        '
        Me.gridColumn7.ColumnEdit = Me.repositoryItemImageComboBox4
        Me.gridColumn7.FieldName = "Read"
        Me.gridColumn7.ImageAlignment = System.Drawing.StringAlignment.Center
        Me.gridColumn7.ImageIndex = 3
        Me.gridColumn7.Name = "gridColumn7"
        Me.gridColumn7.OptionsColumn.AllowEdit = False
        Me.gridColumn7.OptionsColumn.AllowFocus = False
        Me.gridColumn7.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.[False]
        Me.gridColumn7.OptionsColumn.AllowShowHide = False
        Me.gridColumn7.OptionsColumn.AllowSize = False
        Me.gridColumn7.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
        Me.gridColumn7.OptionsColumn.FixedWidth = True
        Me.gridColumn7.OptionsColumn.ShowCaption = False
        Me.gridColumn7.OptionsFilter.AllowAutoFilter = False
        Me.gridColumn7.OptionsFilter.AllowFilter = False
        Me.gridColumn7.Visible = True
        Me.gridColumn7.VisibleIndex = 6
        Me.gridColumn7.Width = 25
        '
        'GRD_Actividades
        '
        Me.GRD_Actividades.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Actividades.Location = New System.Drawing.Point(0, 27)
        Me.GRD_Actividades.MainView = Me.GridView1
        Me.GRD_Actividades.Name = "GRD_Actividades"
        Me.GRD_Actividades.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.repositoryItemImageComboBox1, Me.repositoryItemImageComboBox2, Me.repositoryItemImageComboBox3, Me.repositoryItemImageComboBox4, Me.RepositoryItemProgressBar1})
        Me.GRD_Actividades.Size = New System.Drawing.Size(1216, 279)
        Me.GRD_Actividades.TabIndex = 5
        Me.GRD_Actividades.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn15, Me.GridColumn16, Me.GridColumn17, Me.GridColumn23, Me.GridColumn18, Me.GridColumn19, Me.GridColumn20, Me.GridColumn22, Me.GridColumn21, Me.GridColumn24, Me.Col_EsSoloSeguimiento, Me.Col_ALaEsperaRespuesta, Me.Col_EstaFinalizada, Me.Col_Cliente, Me.Col_Solucion, Me.Col_Finalizada, Me.Col_PorcentajeFinalizada})
        Me.GridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus
        GridFormatRule1.ApplyToRow = True
        GridFormatRule1.Column = Me.Col_Finalizada
        GridFormatRule1.Name = "Finalizada"
        FormatConditionRuleValue1.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        FormatConditionRuleValue1.Appearance.Options.UseBackColor = True
        FormatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal
        FormatConditionRuleValue1.Value1 = "1"
        GridFormatRule1.Rule = FormatConditionRuleValue1
        GridFormatRule2.ApplyToRow = True
        GridFormatRule2.Column = Me.Col_EstaFinalizada
        GridFormatRule2.Name = "EstaFinalizada"
        FormatConditionRuleValue2.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        FormatConditionRuleValue2.Appearance.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold)
        FormatConditionRuleValue2.Appearance.Options.UseBackColor = True
        FormatConditionRuleValue2.Appearance.Options.UseFont = True
        FormatConditionRuleValue2.Condition = DevExpress.XtraEditors.FormatCondition.Equal
        FormatConditionRuleValue2.Value1 = 1
        GridFormatRule2.Rule = FormatConditionRuleValue2
        GridFormatRule3.ApplyToRow = True
        GridFormatRule3.Column = Me.Col_EsSoloSeguimiento
        GridFormatRule3.Name = "Seguimiento"
        FormatConditionRuleValue3.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        FormatConditionRuleValue3.Appearance.Options.UseBackColor = True
        FormatConditionRuleValue3.Condition = DevExpress.XtraEditors.FormatCondition.Equal
        FormatConditionRuleValue3.Value1 = CType(1, Short)
        GridFormatRule3.Rule = FormatConditionRuleValue3
        GridFormatRule4.ApplyToRow = True
        GridFormatRule4.Column = Me.Col_ALaEsperaRespuesta
        GridFormatRule4.Name = "ALaEsperaRespuesta"
        FormatConditionRuleValue4.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        FormatConditionRuleValue4.Appearance.Options.UseBackColor = True
        FormatConditionRuleValue4.Condition = DevExpress.XtraEditors.FormatCondition.Equal
        FormatConditionRuleValue4.Value1 = "1"
        GridFormatRule4.Rule = FormatConditionRuleValue4
        Me.GridView1.FormatRules.Add(GridFormatRule1)
        Me.GridView1.FormatRules.Add(GridFormatRule2)
        Me.GridView1.FormatRules.Add(GridFormatRule3)
        Me.GridView1.FormatRules.Add(GridFormatRule4)
        Me.GridView1.GridControl = Me.GRD_Actividades
        Me.GridView1.GroupCount = 1
        Me.GridView1.GroupFormat = "[#image]{1} {2}"
        Me.GridView1.GroupSummary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "Subject", Nothing, "({0} items)")})
        Me.GridView1.Images = Me.ilColumns
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.[True]
        Me.GridView1.OptionsBehavior.AllowPixelScrolling = DevExpress.Utils.DefaultBoolean.[True]
        Me.GridView1.OptionsBehavior.AutoExpandAllGroups = True
        Me.GridView1.OptionsDetail.EnableMasterViewMode = False
        Me.GridView1.OptionsFind.AlwaysVisible = True
        Me.GridView1.OptionsFind.SearchInPreview = True
        Me.GridView1.OptionsPrint.PrintHorzLines = False
        Me.GridView1.OptionsPrint.PrintPreview = True
        Me.GridView1.OptionsPrint.PrintVertLines = False
        Me.GridView1.OptionsView.AutoCalcPreviewLineCount = True
        Me.GridView1.OptionsView.GroupDrawMode = DevExpress.XtraGrid.Views.Grid.GroupDrawMode.Office
        Me.GridView1.OptionsView.ShowGroupedColumns = True
        Me.GridView1.OptionsView.ShowGroupPanel = False
        Me.GridView1.OptionsView.ShowIndicator = False
        Me.GridView1.OptionsView.ShowPreview = True
        Me.GridView1.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.[True]
        Me.GridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridView1.PreviewFieldName = "Descripcion"
        Me.GridView1.PreviewIndent = 1
        Me.GridView1.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.GridColumn20, DevExpress.Data.ColumnSortOrder.Descending)})
        '
        'GridColumn15
        '
        Me.GridColumn15.Caption = "Priority"
        Me.GridColumn15.ColumnEdit = Me.repositoryItemImageComboBox1
        Me.GridColumn15.FieldName = "ID_Prioridad"
        Me.GridColumn15.ImageIndex = 0
        Me.GridColumn15.Name = "GridColumn15"
        Me.GridColumn15.OptionsColumn.AllowFocus = False
        Me.GridColumn15.OptionsColumn.AllowSize = False
        Me.GridColumn15.OptionsColumn.FixedWidth = True
        Me.GridColumn15.OptionsColumn.ShowCaption = False
        Me.GridColumn15.ToolTip = "Importance"
        Me.GridColumn15.Visible = True
        Me.GridColumn15.VisibleIndex = 0
        Me.GridColumn15.Width = 25
        '
        'GridColumn17
        '
        Me.GridColumn17.Caption = "Fichero"
        Me.GridColumn17.ColumnEdit = Me.repositoryItemImageComboBox3
        Me.GridColumn17.FieldName = "TieneFichero"
        Me.GridColumn17.ImageIndex = 2
        Me.GridColumn17.Name = "GridColumn17"
        Me.GridColumn17.OptionsColumn.AllowEdit = False
        Me.GridColumn17.OptionsColumn.AllowFocus = False
        Me.GridColumn17.OptionsColumn.AllowSize = False
        Me.GridColumn17.OptionsColumn.FixedWidth = True
        Me.GridColumn17.OptionsColumn.ShowCaption = False
        Me.GridColumn17.ToolTip = "Fichero"
        Me.GridColumn17.Visible = True
        Me.GridColumn17.VisibleIndex = 2
        Me.GridColumn17.Width = 25
        '
        'GridColumn23
        '
        Me.GridColumn23.Caption = "Actividad"
        Me.GridColumn23.FieldName = "ActividadTipo"
        Me.GridColumn23.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText
        Me.GridColumn23.Name = "GridColumn23"
        Me.GridColumn23.OptionsColumn.AllowFocus = False
        Me.GridColumn23.Visible = True
        Me.GridColumn23.VisibleIndex = 3
        Me.GridColumn23.Width = 120
        '
        'GridColumn18
        '
        Me.GridColumn18.Caption = "Asunto"
        Me.GridColumn18.FieldName = "Asunto"
        Me.GridColumn18.Name = "GridColumn18"
        Me.GridColumn18.OptionsColumn.AllowEdit = False
        Me.GridColumn18.OptionsColumn.AllowFocus = False
        Me.GridColumn18.Visible = True
        Me.GridColumn18.VisibleIndex = 4
        Me.GridColumn18.Width = 451
        '
        'GridColumn19
        '
        Me.GridColumn19.Caption = "Propietario"
        Me.GridColumn19.FieldName = "Propietario"
        Me.GridColumn19.Name = "GridColumn19"
        Me.GridColumn19.OptionsColumn.AllowFocus = False
        Me.GridColumn19.ToolTip = "Propietario"
        Me.GridColumn19.Visible = True
        Me.GridColumn19.VisibleIndex = 7
        Me.GridColumn19.Width = 171
        '
        'GridColumn20
        '
        Me.GridColumn20.AppearanceCell.Options.UseTextOptions = True
        Me.GridColumn20.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridColumn20.Caption = "Fecha"
        Me.GridColumn20.DisplayFormat.FormatString = "dd/MM/yyyy"
        Me.GridColumn20.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.GridColumn20.FieldName = "FechaAviso"
        Me.GridColumn20.GroupInterval = DevExpress.XtraGrid.ColumnGroupInterval.DateRange
        Me.GridColumn20.Name = "GridColumn20"
        Me.GridColumn20.OptionsColumn.AllowFocus = False
        Me.GridColumn20.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.DateAlt
        Me.GridColumn20.Visible = True
        Me.GridColumn20.VisibleIndex = 10
        Me.GridColumn20.Width = 62
        '
        'GridColumn22
        '
        Me.GridColumn22.AppearanceCell.Options.UseTextOptions = True
        Me.GridColumn22.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridColumn22.Caption = "Vencimiento"
        Me.GridColumn22.FieldName = "FechaVencimiento"
        Me.GridColumn22.Name = "GridColumn22"
        Me.GridColumn22.OptionsColumn.AllowFocus = False
        Me.GridColumn22.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.DateAlt
        Me.GridColumn22.Visible = True
        Me.GridColumn22.VisibleIndex = 9
        Me.GridColumn22.Width = 70
        '
        'GridColumn21
        '
        Me.GridColumn21.ColumnEdit = Me.repositoryItemImageComboBox4
        Me.GridColumn21.FieldName = "Leido"
        Me.GridColumn21.ImageAlignment = System.Drawing.StringAlignment.Center
        Me.GridColumn21.ImageIndex = 3
        Me.GridColumn21.Name = "GridColumn21"
        Me.GridColumn21.OptionsColumn.AllowEdit = False
        Me.GridColumn21.OptionsColumn.AllowFocus = False
        Me.GridColumn21.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridColumn21.OptionsColumn.AllowShowHide = False
        Me.GridColumn21.OptionsColumn.AllowSize = False
        Me.GridColumn21.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridColumn21.OptionsColumn.FixedWidth = True
        Me.GridColumn21.OptionsColumn.ShowCaption = False
        Me.GridColumn21.OptionsFilter.AllowAutoFilter = False
        Me.GridColumn21.OptionsFilter.AllowFilter = False
        Me.GridColumn21.Width = 25
        '
        'GridColumn24
        '
        Me.GridColumn24.AppearanceCell.Options.UseTextOptions = True
        Me.GridColumn24.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.GridColumn24.Caption = "Nº Actividad"
        Me.GridColumn24.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.GridColumn24.FieldName = "ID_ActividadCRM"
        Me.GridColumn24.Name = "GridColumn24"
        Me.GridColumn24.OptionsColumn.AllowEdit = False
        Me.GridColumn24.Visible = True
        Me.GridColumn24.VisibleIndex = 11
        Me.GridColumn24.Width = 72
        '
        'Col_Cliente
        '
        Me.Col_Cliente.Caption = "Cliente"
        Me.Col_Cliente.FieldName = "ClienteNombre"
        Me.Col_Cliente.Name = "Col_Cliente"
        Me.Col_Cliente.OptionsColumn.AllowEdit = False
        Me.Col_Cliente.Visible = True
        Me.Col_Cliente.VisibleIndex = 6
        Me.Col_Cliente.Width = 178
        '
        'Col_Solucion
        '
        Me.Col_Solucion.Caption = "Solución"
        Me.Col_Solucion.FieldName = "Solucion"
        Me.Col_Solucion.Name = "Col_Solucion"
        Me.Col_Solucion.OptionsColumn.AllowEdit = False
        Me.Col_Solucion.Visible = True
        Me.Col_Solucion.VisibleIndex = 5
        Me.Col_Solucion.Width = 324
        '
        'Col_PorcentajeFinalizada
        '
        Me.Col_PorcentajeFinalizada.Caption = "% Finalizada"
        Me.Col_PorcentajeFinalizada.ColumnEdit = Me.RepositoryItemProgressBar1
        Me.Col_PorcentajeFinalizada.FieldName = "PorcentajeFinalizada"
        Me.Col_PorcentajeFinalizada.Name = "Col_PorcentajeFinalizada"
        Me.Col_PorcentajeFinalizada.Visible = True
        Me.Col_PorcentajeFinalizada.VisibleIndex = 8
        Me.Col_PorcentajeFinalizada.Width = 69
        '
        'RepositoryItemProgressBar1
        '
        Me.RepositoryItemProgressBar1.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.RepositoryItemProgressBar1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.RepositoryItemProgressBar1.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        Me.RepositoryItemProgressBar1.EndColor = System.Drawing.SystemColors.ControlDarkDark
        Me.RepositoryItemProgressBar1.LookAndFeel.SkinName = "Office 2013 Light Gray"
        Me.RepositoryItemProgressBar1.Name = "RepositoryItemProgressBar1"
        Me.RepositoryItemProgressBar1.ShowTitle = True
        Me.RepositoryItemProgressBar1.StartColor = System.Drawing.SystemColors.ControlDarkDark
        '
        'ilColumns
        '
        Me.ilColumns.ImageStream = CType(resources.GetObject("ilColumns.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilColumns.TransparentColor = System.Drawing.Color.Transparent
        Me.ilColumns.Images.SetKeyName(0, "Importance.png")
        Me.ilColumns.Images.SetKeyName(1, "Icon.png")
        Me.ilColumns.Images.SetKeyName(2, "Attach.png")
        Me.ilColumns.Images.SetKeyName(3, "Whatched.png")
        '
        'GridColumn8
        '
        Me.GridColumn8.Caption = "Priority"
        Me.GridColumn8.ColumnEdit = Me.repositoryItemImageComboBox1
        Me.GridColumn8.FieldName = "Priority"
        Me.GridColumn8.ImageIndex = 0
        Me.GridColumn8.Name = "GridColumn8"
        Me.GridColumn8.OptionsColumn.AllowFocus = False
        Me.GridColumn8.OptionsColumn.AllowSize = False
        Me.GridColumn8.OptionsColumn.FixedWidth = True
        Me.GridColumn8.OptionsColumn.ShowCaption = False
        Me.GridColumn8.ToolTip = "Importance"
        Me.GridColumn8.Visible = True
        Me.GridColumn8.VisibleIndex = 0
        Me.GridColumn8.Width = 25
        '
        'GridColumn9
        '
        Me.GridColumn9.Caption = "Read"
        Me.GridColumn9.ColumnEdit = Me.repositoryItemImageComboBox2
        Me.GridColumn9.FieldName = "Read"
        Me.GridColumn9.ImageIndex = 1
        Me.GridColumn9.Name = "GridColumn9"
        Me.GridColumn9.OptionsColumn.AllowEdit = False
        Me.GridColumn9.OptionsColumn.AllowFocus = False
        Me.GridColumn9.OptionsColumn.AllowSize = False
        Me.GridColumn9.OptionsColumn.FixedWidth = True
        Me.GridColumn9.OptionsColumn.ShowCaption = False
        Me.GridColumn9.ToolTip = "Icon"
        Me.GridColumn9.Visible = True
        Me.GridColumn9.VisibleIndex = 1
        Me.GridColumn9.Width = 25
        '
        'GridColumn10
        '
        Me.GridColumn10.Caption = "Attachment"
        Me.GridColumn10.ColumnEdit = Me.repositoryItemImageComboBox3
        Me.GridColumn10.FieldName = "Attachment"
        Me.GridColumn10.ImageIndex = 2
        Me.GridColumn10.Name = "GridColumn10"
        Me.GridColumn10.OptionsColumn.AllowEdit = False
        Me.GridColumn10.OptionsColumn.AllowFocus = False
        Me.GridColumn10.OptionsColumn.AllowSize = False
        Me.GridColumn10.OptionsColumn.FixedWidth = True
        Me.GridColumn10.OptionsColumn.ShowCaption = False
        Me.GridColumn10.ToolTip = "Attachment"
        Me.GridColumn10.Visible = True
        Me.GridColumn10.VisibleIndex = 2
        Me.GridColumn10.Width = 25
        '
        'GridColumn11
        '
        Me.GridColumn11.Caption = "Subject"
        Me.GridColumn11.FieldName = "Subject"
        Me.GridColumn11.Name = "GridColumn11"
        Me.GridColumn11.OptionsColumn.AllowFocus = False
        Me.GridColumn11.Visible = True
        Me.GridColumn11.VisibleIndex = 3
        Me.GridColumn11.Width = 326
        '
        'GridColumn12
        '
        Me.GridColumn12.Caption = "From"
        Me.GridColumn12.FieldName = "From"
        Me.GridColumn12.Name = "GridColumn12"
        Me.GridColumn12.OptionsColumn.AllowFocus = False
        Me.GridColumn12.Visible = True
        Me.GridColumn12.VisibleIndex = 4
        Me.GridColumn12.Width = 113
        '
        'GridColumn13
        '
        Me.GridColumn13.Caption = "Received"
        Me.GridColumn13.FieldName = "Date"
        Me.GridColumn13.GroupInterval = DevExpress.XtraGrid.ColumnGroupInterval.DateRange
        Me.GridColumn13.Name = "GridColumn13"
        Me.GridColumn13.OptionsColumn.AllowFocus = False
        Me.GridColumn13.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.DateAlt
        Me.GridColumn13.Visible = True
        Me.GridColumn13.VisibleIndex = 5
        Me.GridColumn13.Width = 92
        '
        'GridColumn14
        '
        Me.GridColumn14.ColumnEdit = Me.repositoryItemImageComboBox4
        Me.GridColumn14.FieldName = "Read"
        Me.GridColumn14.ImageAlignment = System.Drawing.StringAlignment.Center
        Me.GridColumn14.ImageIndex = 3
        Me.GridColumn14.Name = "GridColumn14"
        Me.GridColumn14.OptionsColumn.AllowEdit = False
        Me.GridColumn14.OptionsColumn.AllowFocus = False
        Me.GridColumn14.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridColumn14.OptionsColumn.AllowShowHide = False
        Me.GridColumn14.OptionsColumn.AllowSize = False
        Me.GridColumn14.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.[False]
        Me.GridColumn14.OptionsColumn.FixedWidth = True
        Me.GridColumn14.OptionsColumn.ShowCaption = False
        Me.GridColumn14.OptionsFilter.AllowAutoFilter = False
        Me.GridColumn14.OptionsFilter.AllowFilter = False
        Me.GridColumn14.Visible = True
        Me.GridColumn14.VisibleIndex = 6
        Me.GridColumn14.Width = 25
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 300000
        '
        'B_VisualizarTodos
        '
        Me.B_VisualizarTodos.Location = New System.Drawing.Point(1031, 39)
        Me.B_VisualizarTodos.Name = "B_VisualizarTodos"
        Me.B_VisualizarTodos.Size = New System.Drawing.Size(91, 23)
        Me.B_VisualizarTodos.TabIndex = 7
        Me.B_VisualizarTodos.Text = "Finalizados"
        '
        'GRD_Chat
        '
        Me.GRD_Chat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Chat.Location = New System.Drawing.Point(0, 331)
        Me.GRD_Chat.MainView = Me.GridView2
        Me.GRD_Chat.Name = "GRD_Chat"
        Me.GRD_Chat.Size = New System.Drawing.Size(1215, 286)
        Me.GRD_Chat.TabIndex = 8
        Me.GRD_Chat.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView2})
        '
        'GridView2
        '
        Me.GridView2.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.ID_ActividadCRM_Chat, Me.ID_ActividadCRM, Me.FechaAlta, Me.De, Me.Para, Me.Mensaje, Me.Asunto, Me.Col_Leido, Me.GridColumn25})
        GridFormatRule5.ApplyToRow = True
        GridFormatRule5.Column = Me.Col_Leido
        GridFormatRule5.Name = "Format0"
        FormatConditionRuleValue5.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        FormatConditionRuleValue5.Appearance.Options.UseFont = True
        FormatConditionRuleValue5.Condition = DevExpress.XtraEditors.FormatCondition.Equal
        FormatConditionRuleValue5.Value1 = CType(0, Short)
        GridFormatRule5.Rule = FormatConditionRuleValue5
        Me.GridView2.FormatRules.Add(GridFormatRule5)
        Me.GridView2.GridControl = Me.GRD_Chat
        Me.GridView2.Name = "GridView2"
        Me.GridView2.OptionsBehavior.Editable = False
        Me.GridView2.OptionsFind.AlwaysVisible = True
        Me.GridView2.OptionsView.ShowGroupPanel = False
        '
        'ID_ActividadCRM_Chat
        '
        Me.ID_ActividadCRM_Chat.Caption = "ID_ActividadCRM_Chat"
        Me.ID_ActividadCRM_Chat.FieldName = "ID_ActividadCRM_Chat"
        Me.ID_ActividadCRM_Chat.Name = "ID_ActividadCRM_Chat"
        '
        'ID_ActividadCRM
        '
        Me.ID_ActividadCRM.AppearanceCell.Options.UseTextOptions = True
        Me.ID_ActividadCRM.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.ID_ActividadCRM.Caption = "Nº Actividad"
        Me.ID_ActividadCRM.FieldName = "ID_ActividadCRM"
        Me.ID_ActividadCRM.Name = "ID_ActividadCRM"
        Me.ID_ActividadCRM.Visible = True
        Me.ID_ActividadCRM.VisibleIndex = 3
        Me.ID_ActividadCRM.Width = 68
        '
        'FechaAlta
        '
        Me.FechaAlta.Caption = "Fecha de alta"
        Me.FechaAlta.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm"
        Me.FechaAlta.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.FechaAlta.FieldName = "FechaAlta"
        Me.FechaAlta.Name = "FechaAlta"
        Me.FechaAlta.Visible = True
        Me.FechaAlta.VisibleIndex = 1
        Me.FechaAlta.Width = 92
        '
        'De
        '
        Me.De.Caption = "De"
        Me.De.FieldName = "De"
        Me.De.Name = "De"
        Me.De.Visible = True
        Me.De.VisibleIndex = 0
        Me.De.Width = 110
        '
        'Para
        '
        Me.Para.Caption = "Para"
        Me.Para.FieldName = "Para"
        Me.Para.Name = "Para"
        Me.Para.Width = 208
        '
        'Mensaje
        '
        Me.Mensaje.Caption = "Mensaje"
        Me.Mensaje.FieldName = "Mensaje"
        Me.Mensaje.Name = "Mensaje"
        Me.Mensaje.Visible = True
        Me.Mensaje.VisibleIndex = 2
        Me.Mensaje.Width = 884
        '
        'Asunto
        '
        Me.Asunto.Caption = "Asunto de la actividad"
        Me.Asunto.FieldName = "Asunto"
        Me.Asunto.Name = "Asunto"
        Me.Asunto.Visible = True
        Me.Asunto.VisibleIndex = 4
        Me.Asunto.Width = 215
        '
        'GridColumn25
        '
        Me.GridColumn25.Caption = "Nombre del cliente"
        Me.GridColumn25.FieldName = "ClienteNombre"
        Me.GridColumn25.Name = "GridColumn25"
        Me.GridColumn25.Visible = True
        Me.GridColumn25.VisibleIndex = 5
        Me.GridColumn25.Width = 207
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(5, 9)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(106, 13)
        Me.LabelControl1.TabIndex = 9
        Me.LabelControl1.Text = "Listado de actividades"
        '
        'LabelControl2
        '
        Me.LabelControl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl2.Location = New System.Drawing.Point(6, 313)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(136, 13)
        Me.LabelControl2.TabIndex = 10
        Me.LabelControl2.Text = "Listado de mensajes de chat"
        '
        'B_VisualizarSeguimiento
        '
        Me.B_VisualizarSeguimiento.Location = New System.Drawing.Point(934, 39)
        Me.B_VisualizarSeguimiento.Name = "B_VisualizarSeguimiento"
        Me.B_VisualizarSeguimiento.Size = New System.Drawing.Size(91, 23)
        Me.B_VisualizarSeguimiento.TabIndex = 11
        Me.B_VisualizarSeguimiento.Text = "Seguimiento"
        '
        'B_ALaEsperaDeRespuesta
        '
        Me.B_ALaEsperaDeRespuesta.Location = New System.Drawing.Point(837, 39)
        Me.B_ALaEsperaDeRespuesta.Name = "B_ALaEsperaDeRespuesta"
        Me.B_ALaEsperaDeRespuesta.Size = New System.Drawing.Size(91, 23)
        Me.B_ALaEsperaDeRespuesta.TabIndex = 12
        Me.B_ALaEsperaDeRespuesta.Text = "A la espera"
        '
        'B_CrearActividad
        '
        Me.B_CrearActividad.Location = New System.Drawing.Point(615, 39)
        Me.B_CrearActividad.Name = "B_CrearActividad"
        Me.B_CrearActividad.Size = New System.Drawing.Size(84, 23)
        Me.B_CrearActividad.TabIndex = 13
        Me.B_CrearActividad.Text = "Crear actividad"
        '
        'B_Actualizar
        '
        Me.B_Actualizar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.B_Actualizar.Location = New System.Drawing.Point(1134, 39)
        Me.B_Actualizar.Name = "B_Actualizar"
        Me.B_Actualizar.Size = New System.Drawing.Size(70, 23)
        Me.B_Actualizar.TabIndex = 14
        Me.B_Actualizar.Text = "Actualizar"
        '
        'B_VisualizarPendientes
        '
        Me.B_VisualizarPendientes.Checked = True
        Me.B_VisualizarPendientes.Location = New System.Drawing.Point(740, 39)
        Me.B_VisualizarPendientes.Name = "B_VisualizarPendientes"
        Me.B_VisualizarPendientes.Size = New System.Drawing.Size(91, 23)
        Me.B_VisualizarPendientes.TabIndex = 15
        Me.B_VisualizarPendientes.Text = "Pendientes"
        '
        'B_Chat_Todos
        '
        Me.B_Chat_Todos.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.B_Chat_Todos.Location = New System.Drawing.Point(502, 343)
        Me.B_Chat_Todos.Name = "B_Chat_Todos"
        Me.B_Chat_Todos.Size = New System.Drawing.Size(116, 22)
        Me.B_Chat_Todos.TabIndex = 16
        Me.B_Chat_Todos.Text = "Todos"
        '
        'L_NumRows
        '
        Me.L_NumRows.Location = New System.Drawing.Point(502, 44)
        Me.L_NumRows.Name = "L_NumRows"
        Me.L_NumRows.Size = New System.Drawing.Size(6, 13)
        Me.L_NumRows.TabIndex = 18
        Me.L_NumRows.Text = "0"
        '
        'frmActividadCRM_Principal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1216, 618)
        Me.Controls.Add(Me.L_NumRows)
        Me.Controls.Add(Me.B_Chat_Todos)
        Me.Controls.Add(Me.B_VisualizarPendientes)
        Me.Controls.Add(Me.B_Actualizar)
        Me.Controls.Add(Me.B_CrearActividad)
        Me.Controls.Add(Me.B_ALaEsperaDeRespuesta)
        Me.Controls.Add(Me.B_VisualizarSeguimiento)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.GRD_Chat)
        Me.Controls.Add(Me.B_VisualizarTodos)
        Me.Controls.Add(Me.GRD_Actividades)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.KeyPreview = True
        Me.Name = "frmActividadCRM_Principal"
        Me.Text = "Actividades pendientes"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.GRD_Actividades, 0)
        Me.Controls.SetChildIndex(Me.B_VisualizarTodos, 0)
        Me.Controls.SetChildIndex(Me.GRD_Chat, 0)
        Me.Controls.SetChildIndex(Me.LabelControl1, 0)
        Me.Controls.SetChildIndex(Me.LabelControl2, 0)
        Me.Controls.SetChildIndex(Me.B_VisualizarSeguimiento, 0)
        Me.Controls.SetChildIndex(Me.B_ALaEsperaDeRespuesta, 0)
        Me.Controls.SetChildIndex(Me.B_CrearActividad, 0)
        Me.Controls.SetChildIndex(Me.B_Actualizar, 0)
        Me.Controls.SetChildIndex(Me.B_VisualizarPendientes, 0)
        Me.Controls.SetChildIndex(Me.B_Chat_Todos, 0)
        Me.Controls.SetChildIndex(Me.L_NumRows, 0)
        CType(Me.repositoryItemImageComboBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imageCollection1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repositoryItemImageComboBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repositoryItemImageComboBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repositoryItemImageComboBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GRD_Actividades, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemProgressBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GRD_Chat, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents gridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents gridColumn2 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents gridColumn3 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents gridColumn4 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents gridColumn5 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents gridColumn6 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents gridColumn7 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents GRD_Actividades As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridColumn15 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn16 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn17 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn18 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn19 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn20 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn21 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents GridColumn8 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents GridColumn9 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents GridColumn10 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents GridColumn11 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents GridColumn12 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents GridColumn13 As DevExpress.XtraGrid.Columns.GridColumn
    Private WithEvents GridColumn14 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repositoryItemImageComboBox1 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents repositoryItemImageComboBox2 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents repositoryItemImageComboBox3 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents repositoryItemImageComboBox4 As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Private WithEvents ilColumns As System.Windows.Forms.ImageList
    Private WithEvents imageCollection1 As DevExpress.Utils.ImageCollection
    Friend WithEvents GridColumn23 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn22 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn24 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents B_VisualizarTodos As DevExpress.XtraEditors.CheckButton
    Friend WithEvents GRD_Chat As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ID_ActividadCRM_Chat As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents ID_ActividadCRM As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents De As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents FechaAlta As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Para As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Mensaje As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Asunto As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents B_VisualizarSeguimiento As DevExpress.XtraEditors.CheckButton
    Friend WithEvents B_ALaEsperaDeRespuesta As DevExpress.XtraEditors.CheckButton
    Friend WithEvents Col_EsSoloSeguimiento As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Col_ALaEsperaRespuesta As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Col_EstaFinalizada As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Col_Cliente As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Col_Solucion As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Col_Leido As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn25 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Col_Finalizada As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Col_PorcentajeFinalizada As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RepositoryItemProgressBar1 As DevExpress.XtraEditors.Repository.RepositoryItemProgressBar
    Friend WithEvents B_CrearActividad As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents B_Actualizar As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents B_VisualizarPendientes As DevExpress.XtraEditors.CheckButton
    Friend WithEvents B_Chat_Todos As DevExpress.XtraEditors.CheckButton
    Friend WithEvents L_NumRows As DevExpress.XtraEditors.LabelControl
End Class
