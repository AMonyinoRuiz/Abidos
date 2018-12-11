<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRemesa
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRemesa))
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab5 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab6 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Venciments = New M_UltraGrid.m_UltraGrid()
        Me.C_CuentaBancaria = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel37 = New Infragistics.Win.Misc.UltraLabel()
        Me.DT_Remesa = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel13 = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Codigo = New M_MaskEditor.m_MaskEditor()
        Me.DT_Alta = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel28 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl9 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.R_Observaciones = New M_RichText.M_RichText()
        Me.UltraTabPageControl4 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Ficheros = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.TAB_Principal = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.C_CuentaBancaria, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Remesa, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl9.SuspendLayout()
        Me.UltraTabPageControl4.SuspendLayout()
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_Principal.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Margin = New System.Windows.Forms.Padding(4)
        Me.ToolForm.Size = New System.Drawing.Size(985, 24)
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Venciments)
        Me.UltraTabPageControl1.Controls.Add(Me.C_CuentaBancaria)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel37)
        Me.UltraTabPageControl1.Controls.Add(Me.DT_Remesa)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel13)
        Me.UltraTabPageControl1.Controls.Add(Me.TE_Codigo)
        Me.UltraTabPageControl1.Controls.Add(Me.DT_Alta)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel28)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel1)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(983, 560)
        '
        'GRD_Venciments
        '
        Me.GRD_Venciments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Venciments.Location = New System.Drawing.Point(13, 71)
        Me.GRD_Venciments.Name = "GRD_Venciments"
        Me.GRD_Venciments.pAccessibleName = Nothing
        Me.GRD_Venciments.pActivarBotonFiltro = False
        Me.GRD_Venciments.pText = " "
        Me.GRD_Venciments.Size = New System.Drawing.Size(958, 475)
        Me.GRD_Venciments.TabIndex = 198
        '
        'C_CuentaBancaria
        '
        Me.C_CuentaBancaria.Location = New System.Drawing.Point(352, 29)
        Me.C_CuentaBancaria.Name = "C_CuentaBancaria"
        Me.C_CuentaBancaria.Size = New System.Drawing.Size(356, 21)
        Me.C_CuentaBancaria.TabIndex = 196
        Me.C_CuentaBancaria.Text = "C_CuentaBancaria"
        '
        'UltraLabel37
        '
        Me.UltraLabel37.Location = New System.Drawing.Point(352, 13)
        Me.UltraLabel37.Name = "UltraLabel37"
        Me.UltraLabel37.Size = New System.Drawing.Size(114, 16)
        Me.UltraLabel37.TabIndex = 197
        Me.UltraLabel37.Text = "*Cuenta bancaria:"
        '
        'DT_Remesa
        '
        Me.DT_Remesa.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Remesa.Location = New System.Drawing.Point(247, 30)
        Me.DT_Remesa.Name = "DT_Remesa"
        Me.DT_Remesa.Size = New System.Drawing.Size(90, 21)
        Me.DT_Remesa.TabIndex = 11
        Me.DT_Remesa.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel13
        '
        Me.UltraLabel13.Location = New System.Drawing.Point(247, 16)
        Me.UltraLabel13.Name = "UltraLabel13"
        Me.UltraLabel13.Size = New System.Drawing.Size(90, 16)
        Me.UltraLabel13.TabIndex = 149
        Me.UltraLabel13.Text = "*Fecha remesa:"
        '
        'TE_Codigo
        '
        Me.TE_Codigo.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.TE_Codigo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.TE_Codigo.InputMask = "nnnnnnnnnn"
        Me.TE_Codigo.Location = New System.Drawing.Point(13, 31)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.ReadOnly = True
        Me.TE_Codigo.Size = New System.Drawing.Size(115, 20)
        Me.TE_Codigo.TabIndex = 0
        '
        'DT_Alta
        '
        Me.DT_Alta.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Alta.Location = New System.Drawing.Point(143, 30)
        Me.DT_Alta.Name = "DT_Alta"
        Me.DT_Alta.ReadOnly = True
        Me.DT_Alta.Size = New System.Drawing.Size(90, 21)
        Me.DT_Alta.TabIndex = 10
        Me.DT_Alta.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel28
        '
        Me.UltraLabel28.Location = New System.Drawing.Point(143, 16)
        Me.UltraLabel28.Name = "UltraLabel28"
        Me.UltraLabel28.Size = New System.Drawing.Size(90, 16)
        Me.UltraLabel28.TabIndex = 140
        Me.UltraLabel28.Text = "*Fecha de alta:"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(13, 15)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel1.TabIndex = 63
        Me.UltraLabel1.Text = "*Código:"
        '
        'UltraTabPageControl9
        '
        Me.UltraTabPageControl9.Controls.Add(Me.R_Observaciones)
        Me.UltraTabPageControl9.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl9.Name = "UltraTabPageControl9"
        Me.UltraTabPageControl9.Size = New System.Drawing.Size(983, 560)
        '
        'R_Observaciones
        '
        Me.R_Observaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.R_Observaciones.Location = New System.Drawing.Point(0, 0)
        Me.R_Observaciones.Margin = New System.Windows.Forms.Padding(4)
        Me.R_Observaciones.Name = "R_Observaciones"
        Me.R_Observaciones.pEnable = True
        Me.R_Observaciones.pText = resources.GetString("R_Observaciones.pText")
        Me.R_Observaciones.pTextEspecial = ""
        Me.R_Observaciones.Size = New System.Drawing.Size(983, 560)
        Me.R_Observaciones.TabIndex = 2
        '
        'UltraTabPageControl4
        '
        Me.UltraTabPageControl4.Controls.Add(Me.GRD_Ficheros)
        Me.UltraTabPageControl4.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl4.Name = "UltraTabPageControl4"
        Me.UltraTabPageControl4.Size = New System.Drawing.Size(983, 560)
        '
        'GRD_Ficheros
        '
        Me.GRD_Ficheros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Ficheros.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Ficheros.Name = "GRD_Ficheros"
        Me.GRD_Ficheros.pAccessibleName = Nothing
        Me.GRD_Ficheros.pActivarBotonFiltro = False
        Me.GRD_Ficheros.pText = " "
        Me.GRD_Ficheros.Size = New System.Drawing.Size(983, 560)
        Me.GRD_Ficheros.TabIndex = 134
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(959, 328)
        '
        'TAB_Principal
        '
        Me.TAB_Principal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TAB_Principal.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl9)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl1)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl4)
        Me.TAB_Principal.Location = New System.Drawing.Point(12, 43)
        Me.TAB_Principal.Name = "TAB_Principal"
        Me.TAB_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.TAB_Principal.Size = New System.Drawing.Size(987, 586)
        Me.TAB_Principal.TabIndex = 0
        UltraTab1.Key = "General"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "General"
        UltraTab5.Key = "Observaciones"
        UltraTab5.TabPage = Me.UltraTabPageControl9
        UltraTab5.Text = "Observaciones"
        UltraTab6.Key = "Ficheros"
        UltraTab6.TabPage = Me.UltraTabPageControl4
        UltraTab6.Text = "Ficheros"
        Me.TAB_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab5, UltraTab6})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(983, 560)
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
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
        'frmRemesa
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 641)
        Me.Controls.Add(Me.TAB_Principal)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmRemesa"
        Me.Text = "Remesa"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.TAB_Principal, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl1.PerformLayout()
        CType(Me.C_CuentaBancaria, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Remesa, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl9.ResumeLayout(False)
        Me.UltraTabPageControl4.ResumeLayout(False)
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_Principal.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TAB_Principal As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UltraTabPageControl9 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl4 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents DT_Remesa As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel13 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Codigo As M_MaskEditor.m_MaskEditor
    Friend WithEvents DT_Alta As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel28 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents R_Observaciones As M_RichText.M_RichText
    Friend WithEvents GRD_Ficheros As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Venciments As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_CuentaBancaria As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel37 As Infragistics.Win.Misc.UltraLabel
End Class
