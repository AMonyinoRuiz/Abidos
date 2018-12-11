<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntrada_Linea_Parte
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
        Dim StateEditorButton1 As Infragistics.Win.UltraWinEditors.StateEditorButton = New Infragistics.Win.UltraWinEditors.StateEditorButton("SoloAsignados")
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab3 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Horas = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Material = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl4 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Gastos = New M_UltraGrid.m_UltraGrid()
        Me.C_Parte = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.UltraLabel55 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.Tab_Principal = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage2 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.T_ImporteGastos = New M_MaskEditor.m_MaskEditor()
        Me.T_NumHoras = New M_MaskEditor.m_MaskEditor()
        Me.T_NumHorasExtras = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel12 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel67 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel23 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl1.SuspendLayout()
        Me.UltraTabPageControl2.SuspendLayout()
        Me.UltraTabPageControl4.SuspendLayout()
        CType(Me.C_Parte, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tab_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tab_Principal.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(988, 24)
        Me.ToolForm.TabIndex = 29
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Horas)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(980, 390)
        '
        'GRD_Horas
        '
        Me.GRD_Horas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Horas.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Horas.Name = "GRD_Horas"
        Me.GRD_Horas.pAccessibleName = Nothing
        Me.GRD_Horas.pActivarBotonFiltro = False
        Me.GRD_Horas.pText = " "
        Me.GRD_Horas.Size = New System.Drawing.Size(980, 390)
        Me.GRD_Horas.TabIndex = 136
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.GRD_Material)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(980, 390)
        '
        'GRD_Material
        '
        Me.GRD_Material.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Material.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Material.Name = "GRD_Material"
        Me.GRD_Material.pAccessibleName = Nothing
        Me.GRD_Material.pActivarBotonFiltro = False
        Me.GRD_Material.pText = " "
        Me.GRD_Material.Size = New System.Drawing.Size(980, 390)
        Me.GRD_Material.TabIndex = 137
        '
        'UltraTabPageControl4
        '
        Me.UltraTabPageControl4.Controls.Add(Me.GRD_Gastos)
        Me.UltraTabPageControl4.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl4.Name = "UltraTabPageControl4"
        Me.UltraTabPageControl4.Size = New System.Drawing.Size(980, 390)
        '
        'GRD_Gastos
        '
        Me.GRD_Gastos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Gastos.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Gastos.Name = "GRD_Gastos"
        Me.GRD_Gastos.pAccessibleName = Nothing
        Me.GRD_Gastos.pActivarBotonFiltro = False
        Me.GRD_Gastos.pText = " "
        Me.GRD_Gastos.Size = New System.Drawing.Size(980, 390)
        Me.GRD_Gastos.TabIndex = 137
        '
        'C_Parte
        '
        StateEditorButton1.DisplayStyle = Infragistics.Win.UltraWinEditors.StateButtonDisplayStyle.StateButton
        StateEditorButton1.Key = "SoloAsignados"
        StateEditorButton1.Text = "Ver sólo los partes asignados al documento"
        Me.C_Parte.ButtonsRight.Add(StateEditorButton1)
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.C_Parte.DisplayLayout.Appearance = Appearance1
        Me.C_Parte.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.C_Parte.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.C_Parte.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.C_Parte.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.C_Parte.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.C_Parte.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.C_Parte.DisplayLayout.MaxColScrollRegions = 1
        Me.C_Parte.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.C_Parte.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.C_Parte.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.C_Parte.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.C_Parte.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.C_Parte.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.C_Parte.DisplayLayout.Override.CellAppearance = Appearance8
        Me.C_Parte.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.C_Parte.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.C_Parte.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlignAsString = "Left"
        Me.C_Parte.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.C_Parte.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.C_Parte.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.C_Parte.DisplayLayout.Override.RowAppearance = Appearance11
        Me.C_Parte.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.C_Parte.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.C_Parte.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.C_Parte.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.C_Parte.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.C_Parte.Location = New System.Drawing.Point(12, 59)
        Me.C_Parte.Name = "C_Parte"
        Me.C_Parte.Size = New System.Drawing.Size(597, 22)
        Me.C_Parte.TabIndex = 0
        Me.C_Parte.Text = "C_Parte"
        '
        'UltraLabel55
        '
        Me.UltraLabel55.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel55.Name = "UltraLabel55"
        Me.UltraLabel55.Size = New System.Drawing.Size(96, 16)
        Me.UltraLabel55.TabIndex = 137
        Me.UltraLabel55.Text = "Partes:"
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(959, 328)
        '
        'UltraTabPageControl10
        '
        Me.UltraTabPageControl10.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl10.Name = "UltraTabPageControl10"
        Me.UltraTabPageControl10.Size = New System.Drawing.Size(955, 344)
        '
        'UltraTabPageControl11
        '
        Me.UltraTabPageControl11.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl11.Name = "UltraTabPageControl11"
        Me.UltraTabPageControl11.Size = New System.Drawing.Size(955, 344)
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(1, 20)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(694, 303)
        '
        'Tab_Principal
        '
        Me.Tab_Principal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tab_Principal.Controls.Add(Me.UltraTabSharedControlsPage2)
        Me.Tab_Principal.Controls.Add(Me.UltraTabPageControl1)
        Me.Tab_Principal.Controls.Add(Me.UltraTabPageControl2)
        Me.Tab_Principal.Controls.Add(Me.UltraTabPageControl4)
        Me.Tab_Principal.Location = New System.Drawing.Point(12, 91)
        Me.Tab_Principal.Name = "Tab_Principal"
        Me.Tab_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage2
        Me.Tab_Principal.Size = New System.Drawing.Size(984, 416)
        Me.Tab_Principal.TabIndex = 1
        UltraTab1.Key = "Horas"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Horas"
        UltraTab2.Key = "Material"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Material"
        UltraTab2.Visible = False
        UltraTab3.Key = "Gastos"
        UltraTab3.TabPage = Me.UltraTabPageControl4
        UltraTab3.Text = "Gastos"
        Me.Tab_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2, UltraTab3})
        '
        'UltraTabSharedControlsPage2
        '
        Me.UltraTabSharedControlsPage2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage2.Name = "UltraTabSharedControlsPage2"
        Me.UltraTabSharedControlsPage2.Size = New System.Drawing.Size(980, 390)
        '
        'T_ImporteGastos
        '
        Me.T_ImporteGastos.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_ImporteGastos.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Currency
        Me.T_ImporteGastos.Location = New System.Drawing.Point(882, 61)
        Me.T_ImporteGastos.Name = "T_ImporteGastos"
        Me.T_ImporteGastos.NonAutoSizeHeight = 20
        Me.T_ImporteGastos.ReadOnly = True
        Me.T_ImporteGastos.Size = New System.Drawing.Size(114, 20)
        Me.T_ImporteGastos.TabIndex = 140
        Me.T_ImporteGastos.Text = "T_ImporteGastos"
        '
        'T_NumHoras
        '
        Me.T_NumHoras.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_NumHoras.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Double]
        Me.T_NumHoras.InputMask = "{double:-9.2}"
        Me.T_NumHoras.Location = New System.Drawing.Point(624, 61)
        Me.T_NumHoras.Name = "T_NumHoras"
        Me.T_NumHoras.NonAutoSizeHeight = 20
        Me.T_NumHoras.ReadOnly = True
        Me.T_NumHoras.Size = New System.Drawing.Size(114, 20)
        Me.T_NumHoras.TabIndex = 138
        Me.T_NumHoras.Text = "T_Num_Horas"
        '
        'T_NumHorasExtras
        '
        Me.T_NumHorasExtras.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_NumHorasExtras.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Double]
        Me.T_NumHorasExtras.InputMask = "{double:-9.2}"
        Me.T_NumHorasExtras.Location = New System.Drawing.Point(752, 61)
        Me.T_NumHorasExtras.Name = "T_NumHorasExtras"
        Me.T_NumHorasExtras.NonAutoSizeHeight = 20
        Me.T_NumHorasExtras.ReadOnly = True
        Me.T_NumHorasExtras.Size = New System.Drawing.Size(114, 20)
        Me.T_NumHorasExtras.TabIndex = 139
        Me.T_NumHorasExtras.Text = "Num_Horas_Extras"
        '
        'UltraLabel12
        '
        Me.UltraLabel12.Location = New System.Drawing.Point(624, 45)
        Me.UltraLabel12.Name = "UltraLabel12"
        Me.UltraLabel12.Size = New System.Drawing.Size(105, 16)
        Me.UltraLabel12.TabIndex = 141
        Me.UltraLabel12.Text = "Nº horas:"
        '
        'UltraLabel67
        '
        Me.UltraLabel67.Location = New System.Drawing.Point(882, 45)
        Me.UltraLabel67.Name = "UltraLabel67"
        Me.UltraLabel67.Size = New System.Drawing.Size(105, 16)
        Me.UltraLabel67.TabIndex = 143
        Me.UltraLabel67.Text = "Importe gastos:"
        '
        'UltraLabel23
        '
        Me.UltraLabel23.Location = New System.Drawing.Point(752, 45)
        Me.UltraLabel23.Name = "UltraLabel23"
        Me.UltraLabel23.Size = New System.Drawing.Size(105, 16)
        Me.UltraLabel23.TabIndex = 142
        Me.UltraLabel23.Text = "Nº horas extras:"
        '
        'frmEntrada_Linea_Parte
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1012, 523)
        Me.Controls.Add(Me.T_ImporteGastos)
        Me.Controls.Add(Me.T_NumHoras)
        Me.Controls.Add(Me.T_NumHorasExtras)
        Me.Controls.Add(Me.UltraLabel12)
        Me.Controls.Add(Me.UltraLabel67)
        Me.Controls.Add(Me.UltraLabel23)
        Me.Controls.Add(Me.C_Parte)
        Me.Controls.Add(Me.UltraLabel55)
        Me.Controls.Add(Me.Tab_Principal)
        Me.KeyPreview = True
        Me.Name = "frmEntrada_Linea_Parte"
        Me.Text = "Relación con los partes"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.Tab_Principal, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel55, 0)
        Me.Controls.SetChildIndex(Me.C_Parte, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel23, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel67, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel12, 0)
        Me.Controls.SetChildIndex(Me.T_NumHorasExtras, 0)
        Me.Controls.SetChildIndex(Me.T_NumHoras, 0)
        Me.Controls.SetChildIndex(Me.T_ImporteGastos, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl2.ResumeLayout(False)
        Me.UltraTabPageControl4.ResumeLayout(False)
        CType(Me.C_Parte, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tab_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tab_Principal.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents GRD_Horas As M_UltraGrid.m_UltraGrid
    Friend WithEvents Tab_Principal As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage2 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents C_Parte As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents UltraLabel55 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl4 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Material As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Gastos As M_UltraGrid.m_UltraGrid
    Friend WithEvents T_ImporteGastos As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_NumHoras As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_NumHorasExtras As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel12 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel67 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel23 As Infragistics.Win.Misc.UltraLabel
End Class
