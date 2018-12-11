<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMenu
    'Inherits System.Windows.Forms.Form
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
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.TreeMenu = New DevExpress.XtraTreeList.TreeList()
        Me.IDMenu = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.Descripcio = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.IDTipusMenu = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.Ordre = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.RepositoryItemDateEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.RepositoryItemDateEdit2 = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Observaciones = New M_TextEditor.m_TextEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NivellSeguretat = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.L_Estado = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.C_Tipo = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Codigo = New M_TextEditor.m_TextEditor()
        Me.C_Objeto = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.T_Orden = New M_MaskEditor.m_MaskEditor()
        Me.ImageComboBoxEdit1 = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.M_Images1 = New M_Global.M_Images(Me.components)
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.UltraTabControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage2 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.TreeMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Observaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Tipo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TE_Codigo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Objeto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageComboBoxEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Size = New System.Drawing.Size(980, 24)
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.TreeMenu)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel4)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel7)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Observaciones)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel3)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel6)
        Me.UltraTabPageControl1.Controls.Add(Me.T_NivellSeguretat)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel2)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel1)
        Me.UltraTabPageControl1.Controls.Add(Me.L_Estado)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Descripcion)
        Me.UltraTabPageControl1.Controls.Add(Me.C_Tipo)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel5)
        Me.UltraTabPageControl1.Controls.Add(Me.TE_Codigo)
        Me.UltraTabPageControl1.Controls.Add(Me.C_Objeto)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Orden)
        Me.UltraTabPageControl1.Controls.Add(Me.ImageComboBoxEdit1)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(976, 562)
        '
        'TreeMenu
        '
        Me.TreeMenu.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TreeMenu.Appearance.Empty.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TreeMenu.Appearance.Empty.Options.UseBackColor = True
        Me.TreeMenu.Appearance.Row.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TreeMenu.Appearance.Row.Options.UseBackColor = True
        Me.TreeMenu.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.IDMenu, Me.Descripcio, Me.IDTipusMenu, Me.Ordre})
        Me.TreeMenu.CustomizationFormBounds = New System.Drawing.Rectangle(1155, 585, 234, 209)
        Me.TreeMenu.Location = New System.Drawing.Point(14, 12)
        Me.TreeMenu.LookAndFeel.SkinName = "Office 2010 Black"
        Me.TreeMenu.LookAndFeel.UseDefaultLookAndFeel = False
        Me.TreeMenu.Name = "TreeMenu"
        Me.TreeMenu.OptionsBehavior.DragNodes = True
        Me.TreeMenu.OptionsBehavior.EnableFiltering = True
        Me.TreeMenu.OptionsBehavior.ExpandNodesOnIncrementalSearch = True
        Me.TreeMenu.OptionsBehavior.PopulateServiceColumns = True
        Me.TreeMenu.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Smart
        Me.TreeMenu.OptionsFilter.ShowAllValuesInCheckedFilterPopup = False
        Me.TreeMenu.OptionsFilter.ShowAllValuesInFilterPopup = True
        Me.TreeMenu.OptionsFind.AllowFindPanel = True
        Me.TreeMenu.OptionsFind.AlwaysVisible = True
        Me.TreeMenu.OptionsFind.FindMode = DevExpress.XtraTreeList.FindMode.Always
        Me.TreeMenu.OptionsFind.ShowClearButton = False
        Me.TreeMenu.OptionsFind.ShowCloseButton = False
        Me.TreeMenu.OptionsFind.ShowFindButton = False
        Me.TreeMenu.OptionsPrint.PrintHorzLines = False
        Me.TreeMenu.OptionsPrint.PrintPageHeader = False
        Me.TreeMenu.OptionsPrint.PrintReportFooter = False
        Me.TreeMenu.OptionsPrint.PrintTree = False
        Me.TreeMenu.OptionsPrint.PrintVertLines = False
        Me.TreeMenu.OptionsView.ShowColumns = False
        Me.TreeMenu.OptionsView.ShowHorzLines = False
        Me.TreeMenu.OptionsView.ShowIndicator = False
        Me.TreeMenu.OptionsView.ShowVertLines = False
        Me.TreeMenu.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemDateEdit1, Me.RepositoryItemDateEdit2})
        Me.TreeMenu.Size = New System.Drawing.Size(278, 536)
        Me.TreeMenu.TabIndex = 226
        Me.TreeMenu.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None
        '
        'IDMenu
        '
        Me.IDMenu.Caption = "IDMenu"
        Me.IDMenu.FieldName = "IDMenu"
        Me.IDMenu.Name = "IDMenu"
        Me.IDMenu.OptionsColumn.AllowEdit = False
        Me.IDMenu.OptionsColumn.ReadOnly = True
        Me.IDMenu.Width = 69
        '
        'Descripcio
        '
        Me.Descripcio.Caption = "Descripcio"
        Me.Descripcio.FieldName = "Descripcio"
        Me.Descripcio.MinWidth = 33
        Me.Descripcio.Name = "Descripcio"
        Me.Descripcio.OptionsColumn.AllowEdit = False
        Me.Descripcio.OptionsColumn.ReadOnly = True
        Me.Descripcio.Visible = True
        Me.Descripcio.VisibleIndex = 0
        Me.Descripcio.Width = 184
        '
        'IDTipusMenu
        '
        Me.IDTipusMenu.Caption = "IDTipusMenu"
        Me.IDTipusMenu.FieldName = "IDTipusMenu"
        Me.IDTipusMenu.Name = "IDTipusMenu"
        Me.IDTipusMenu.OptionsColumn.AllowEdit = False
        Me.IDTipusMenu.OptionsColumn.ReadOnly = True
        Me.IDTipusMenu.Width = 137
        '
        'Ordre
        '
        Me.Ordre.Caption = "Ordre"
        Me.Ordre.FieldName = "Ordre"
        Me.Ordre.Name = "Ordre"
        Me.Ordre.OptionsColumn.AllowEdit = False
        Me.Ordre.OptionsColumn.ReadOnly = True
        Me.Ordre.SortOrder = System.Windows.Forms.SortOrder.Ascending
        '
        'RepositoryItemDateEdit1
        '
        Me.RepositoryItemDateEdit1.AutoHeight = False
        Me.RepositoryItemDateEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit1.Name = "RepositoryItemDateEdit1"
        '
        'RepositoryItemDateEdit2
        '
        Me.RepositoryItemDateEdit2.AutoHeight = False
        Me.RepositoryItemDateEdit2.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.RepositoryItemDateEdit2.Name = "RepositoryItemDateEdit2"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(803, 328)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(105, 16)
        Me.UltraLabel4.TabIndex = 233
        Me.UltraLabel4.Text = "*Nivel de seguridad:"
        Me.UltraLabel4.Visible = False
        '
        'UltraLabel7
        '
        Me.UltraLabel7.Location = New System.Drawing.Point(308, 117)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.Size = New System.Drawing.Size(112, 16)
        Me.UltraLabel7.TabIndex = 242
        Me.UltraLabel7.Text = "Foto:"
        '
        'T_Observaciones
        '
        Me.T_Observaciones.Location = New System.Drawing.Point(634, 339)
        Me.T_Observaciones.Multiline = True
        Me.T_Observaciones.Name = "T_Observaciones"
        Me.T_Observaciones.Size = New System.Drawing.Size(131, 101)
        Me.T_Observaciones.TabIndex = 230
        Me.T_Observaciones.Text = "T_Observaciones"
        Me.T_Observaciones.Visible = False
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(634, 323)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel3.TabIndex = 231
        Me.UltraLabel3.Text = "Observaciones"
        Me.UltraLabel3.Visible = False
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(770, 67)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(105, 16)
        Me.UltraLabel6.TabIndex = 235
        Me.UltraLabel6.Text = "*Orden:"
        '
        'T_NivellSeguretat
        '
        Me.T_NivellSeguretat.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Integer]
        Me.T_NivellSeguretat.InputMask = "nnn"
        Me.T_NivellSeguretat.Location = New System.Drawing.Point(803, 345)
        Me.T_NivellSeguretat.Name = "T_NivellSeguretat"
        Me.T_NivellSeguretat.Size = New System.Drawing.Size(131, 20)
        Me.T_NivellSeguretat.TabIndex = 232
        Me.T_NivellSeguretat.Visible = False
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(308, 67)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(112, 16)
        Me.UltraLabel2.TabIndex = 225
        Me.UltraLabel2.Text = "*Objeto:"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(308, 12)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel1.TabIndex = 155
        Me.UltraLabel1.Text = "*Código:"
        '
        'L_Estado
        '
        Me.L_Estado.Location = New System.Drawing.Point(770, 12)
        Me.L_Estado.Name = "L_Estado"
        Me.L_Estado.Size = New System.Drawing.Size(112, 16)
        Me.L_Estado.TabIndex = 221
        Me.L_Estado.Text = "*Tipo:"
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(411, 29)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(344, 21)
        Me.T_Descripcion.TabIndex = 0
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'C_Tipo
        '
        Me.C_Tipo.Location = New System.Drawing.Point(770, 29)
        Me.C_Tipo.Name = "C_Tipo"
        Me.C_Tipo.Size = New System.Drawing.Size(194, 21)
        Me.C_Tipo.TabIndex = 1
        Me.C_Tipo.Text = "C_Tipo"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(408, 12)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 156
        Me.UltraLabel5.Text = "*Descripción"
        '
        'TE_Codigo
        '
        Me.TE_Codigo.Location = New System.Drawing.Point(308, 29)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.ReadOnly = True
        Me.TE_Codigo.Size = New System.Drawing.Size(93, 21)
        Me.TE_Codigo.TabIndex = 222
        Me.TE_Codigo.Text = "TE_Codigo"
        '
        'C_Objeto
        '
        Me.C_Objeto.Location = New System.Drawing.Point(308, 84)
        Me.C_Objeto.Name = "C_Objeto"
        Me.C_Objeto.Size = New System.Drawing.Size(447, 21)
        Me.C_Objeto.TabIndex = 2
        Me.C_Objeto.Text = "C_Objeto"
        '
        'T_Orden
        '
        Me.T_Orden.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Integer]
        Me.T_Orden.InputMask = "nnnnn"
        Me.T_Orden.Location = New System.Drawing.Point(770, 84)
        Me.T_Orden.Name = "T_Orden"
        Me.T_Orden.Size = New System.Drawing.Size(194, 20)
        Me.T_Orden.TabIndex = 3
        '
        'ImageComboBoxEdit1
        '
        Me.ImageComboBoxEdit1.Location = New System.Drawing.Point(308, 134)
        Me.ImageComboBoxEdit1.Name = "ImageComboBoxEdit1"
        Me.ImageComboBoxEdit1.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ImageComboBoxEdit1.Properties.DropDownRows = 20
        Me.ImageComboBoxEdit1.Size = New System.Drawing.Size(447, 20)
        Me.ImageComboBoxEdit1.TabIndex = 4
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(1, 20)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(552, 262)
        '
        'UltraTabControl1
        '
        Me.UltraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabSharedControlsPage2)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabPageControl1)
        Me.UltraTabControl1.Location = New System.Drawing.Point(12, 43)
        Me.UltraTabControl1.Name = "UltraTabControl1"
        Me.UltraTabControl1.SharedControlsPage = Me.UltraTabSharedControlsPage2
        Me.UltraTabControl1.Size = New System.Drawing.Size(980, 588)
        Me.UltraTabControl1.TabIndex = 243
        UltraTab1.Key = "General"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "General"
        Me.UltraTabControl1.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1})
        '
        'UltraTabSharedControlsPage2
        '
        Me.UltraTabSharedControlsPage2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage2.Name = "UltraTabSharedControlsPage2"
        Me.UltraTabSharedControlsPage2.Size = New System.Drawing.Size(976, 562)
        '
        'frmMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1004, 643)
        Me.Controls.Add(Me.UltraTabControl1)
        Me.KeyPreview = True
        Me.Name = "frmMenu"
        Me.Text = "Mantenimiento de menús"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraTabControl1, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl1.PerformLayout()
        CType(Me.TreeMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemDateEdit2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Observaciones, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Tipo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TE_Codigo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Objeto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageComboBoxEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_Tipo As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents L_Estado As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Codigo As M_TextEditor.m_TextEditor
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents C_Objeto As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TreeMenu As DevExpress.XtraTreeList.TreeList
    Private WithEvents IDMenu As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents Descripcio As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents IDTipusMenu As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents RepositoryItemDateEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents RepositoryItemDateEdit2 As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents T_Observaciones As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Orden As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NivellSeguretat As M_MaskEditor.m_MaskEditor
    Friend WithEvents Ordre As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents ImageComboBoxEdit1 As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents M_Images1 As M_Global.M_Images
    Friend WithEvents UltraTabControl1 As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage2 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
End Class
