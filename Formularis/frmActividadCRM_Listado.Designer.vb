<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmActividadCRM_Listado
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
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Pendientes = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Realizadas = New M_UltraGrid.m_UltraGrid()
        Me.Tab_Principal = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.C_Comercial = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl1.SuspendLayout()
        Me.UltraTabPageControl2.SuspendLayout()
        CType(Me.Tab_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tab_Principal.SuspendLayout()
        CType(Me.C_Comercial, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.Sortir
        Me.ToolForm.Size = New System.Drawing.Size(892, 24)
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Pendientes)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(889, 496)
        '
        'GRD_Pendientes
        '
        Me.GRD_Pendientes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Pendientes.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Pendientes.Name = "GRD_Pendientes"
        Me.GRD_Pendientes.pAccessibleName = Nothing
        Me.GRD_Pendientes.pActivarBotonFiltro = False
        Me.GRD_Pendientes.pText = " "
        Me.GRD_Pendientes.Size = New System.Drawing.Size(889, 496)
        Me.GRD_Pendientes.TabIndex = 1
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.GRD_Realizadas)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(889, 496)
        '
        'GRD_Realizadas
        '
        Me.GRD_Realizadas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Realizadas.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Realizadas.Name = "GRD_Realizadas"
        Me.GRD_Realizadas.pAccessibleName = Nothing
        Me.GRD_Realizadas.pActivarBotonFiltro = False
        Me.GRD_Realizadas.pText = " "
        Me.GRD_Realizadas.Size = New System.Drawing.Size(889, 496)
        Me.GRD_Realizadas.TabIndex = 2
        '
        'Tab_Principal
        '
        Me.Tab_Principal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tab_Principal.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.Tab_Principal.Controls.Add(Me.UltraTabPageControl1)
        Me.Tab_Principal.Controls.Add(Me.UltraTabPageControl2)
        Me.Tab_Principal.Location = New System.Drawing.Point(12, 94)
        Me.Tab_Principal.Name = "Tab_Principal"
        Me.Tab_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.Tab_Principal.Size = New System.Drawing.Size(893, 522)
        Me.Tab_Principal.TabIndex = 2
        UltraTab1.Key = "Pendientes"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Pendientes"
        UltraTab2.Key = "Realizadas"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Realizadas"
        Me.Tab_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(889, 496)
        '
        'C_Comercial
        '
        Me.C_Comercial.Location = New System.Drawing.Point(12, 58)
        Me.C_Comercial.Name = "C_Comercial"
        Me.C_Comercial.Size = New System.Drawing.Size(242, 21)
        Me.C_Comercial.TabIndex = 222
        Me.C_Comercial.Text = "C_Comercial"
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(176, 16)
        Me.UltraLabel6.TabIndex = 221
        Me.UltraLabel6.Text = "Comercial:"
        '
        'frmActividadCRM_Listado
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(917, 628)
        Me.Controls.Add(Me.C_Comercial)
        Me.Controls.Add(Me.UltraLabel6)
        Me.Controls.Add(Me.Tab_Principal)
        Me.KeyPreview = True
        Me.Name = "frmActividadCRM_Listado"
        Me.Text = "Actividad CRM"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.Tab_Principal, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel6, 0)
        Me.Controls.SetChildIndex(Me.C_Comercial, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl2.ResumeLayout(False)
        CType(Me.Tab_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tab_Principal.ResumeLayout(False)
        CType(Me.C_Comercial, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD_Pendientes As M_UltraGrid.m_UltraGrid
    Friend WithEvents Tab_Principal As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Realizadas As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_Comercial As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
End Class
