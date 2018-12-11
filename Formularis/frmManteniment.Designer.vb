<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManteniment
    'Inherits System.Windows.Forms.Form
    Inherits M_GenericForm.frmBase



    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD = New M_UltraGrid.m_UltraGrid()
        Me.C_Taula = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.B_EliminarPropuesta = New Infragistics.Win.Misc.UltraButton()
        Me.B_Recalcular_Estados_Parte = New Infragistics.Win.Misc.UltraButton()
        Me.Tap_Principal = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.C_Taula, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tap_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tap_Principal.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(819, 24)
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(815, 393)
        '
        'GRD
        '
        Me.GRD.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD.Location = New System.Drawing.Point(0, 0)
        Me.GRD.Name = "GRD"
        Me.GRD.pAccessibleName = Nothing
        Me.GRD.pActivarBotonFiltro = False
        Me.GRD.pText = " "
        Me.GRD.Size = New System.Drawing.Size(815, 393)
        Me.GRD.TabIndex = 0
        '
        'C_Taula
        '
        Me.C_Taula.Location = New System.Drawing.Point(12, 59)
        Me.C_Taula.Name = "C_Taula"
        Me.C_Taula.Size = New System.Drawing.Size(535, 21)
        Me.C_Taula.TabIndex = 2
        Me.C_Taula.Text = "C_Taula"
        Me.C_Taula.Visible = False
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(124, 19)
        Me.UltraLabel1.TabIndex = 3
        Me.UltraLabel1.Text = "Maestros:"
        '
        'B_EliminarPropuesta
        '
        Me.B_EliminarPropuesta.Location = New System.Drawing.Point(563, 55)
        Me.B_EliminarPropuesta.Name = "B_EliminarPropuesta"
        Me.B_EliminarPropuesta.Size = New System.Drawing.Size(109, 29)
        Me.B_EliminarPropuesta.TabIndex = 4
        Me.B_EliminarPropuesta.Text = "Eliminar propuesta"
        Me.B_EliminarPropuesta.Visible = False
        '
        'B_Recalcular_Estados_Parte
        '
        Me.B_Recalcular_Estados_Parte.Location = New System.Drawing.Point(678, 55)
        Me.B_Recalcular_Estados_Parte.Name = "B_Recalcular_Estados_Parte"
        Me.B_Recalcular_Estados_Parte.Size = New System.Drawing.Size(153, 29)
        Me.B_Recalcular_Estados_Parte.TabIndex = 6
        Me.B_Recalcular_Estados_Parte.Text = "Recalcular estados parte"
        Me.B_Recalcular_Estados_Parte.Visible = False
        '
        'Tap_Principal
        '
        Me.Tap_Principal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tap_Principal.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.Tap_Principal.Controls.Add(Me.UltraTabPageControl1)
        Me.Tap_Principal.Location = New System.Drawing.Point(12, 43)
        Me.Tap_Principal.Name = "Tap_Principal"
        Me.Tap_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.Tap_Principal.Size = New System.Drawing.Size(819, 419)
        Me.Tap_Principal.TabIndex = 7
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "General"
        Me.Tap_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(815, 393)
        '
        'frmManteniment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(843, 474)
        Me.Controls.Add(Me.Tap_Principal)
        Me.Controls.Add(Me.B_Recalcular_Estados_Parte)
        Me.Controls.Add(Me.B_EliminarPropuesta)
        Me.Controls.Add(Me.C_Taula)
        Me.Controls.Add(Me.UltraLabel1)
        Me.KeyPreview = True
        Me.Name = "frmManteniment"
        Me.Text = "Mantenimiento de maestros"
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.C_Taula, 0)
        Me.Controls.SetChildIndex(Me.B_EliminarPropuesta, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.B_Recalcular_Estados_Parte, 0)
        Me.Controls.SetChildIndex(Me.Tap_Principal, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        CType(Me.C_Taula, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tap_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tap_Principal.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_Taula As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents B_EliminarPropuesta As Infragistics.Win.Misc.UltraButton
    Friend WithEvents B_Recalcular_Estados_Parte As Infragistics.Win.Misc.UltraButton
    Friend WithEvents Tap_Principal As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
End Class
