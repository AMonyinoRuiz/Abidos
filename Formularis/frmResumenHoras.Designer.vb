<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmResumenHoras
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
        Me.GRD_Resumen = New M_UltraGrid.m_UltraGrid()
        Me.C_Año = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.B_Buscar = New Infragistics.Win.Misc.UltraButton()
        Me.C_Personal = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel16 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.C_Año, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Personal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(980, 24)
        '
        'GRD_Resumen
        '
        Me.GRD_Resumen.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Resumen.Location = New System.Drawing.Point(12, 76)
        Me.GRD_Resumen.Name = "GRD_Resumen"
        Me.GRD_Resumen.pAccessibleName = Nothing
        Me.GRD_Resumen.pActivarBotonFiltro = False
        Me.GRD_Resumen.pText = " "
        Me.GRD_Resumen.Size = New System.Drawing.Size(980, 534)
        Me.GRD_Resumen.TabIndex = 10
        '
        'C_Año
        '
        Me.C_Año.Location = New System.Drawing.Point(45, 43)
        Me.C_Año.Name = "C_Año"
        Me.C_Año.Size = New System.Drawing.Size(86, 21)
        Me.C_Año.TabIndex = 0
        Me.C_Año.Text = "C_Año"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(12, 47)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(38, 16)
        Me.UltraLabel2.TabIndex = 153
        Me.UltraLabel2.Text = "Año:"
        '
        'B_Buscar
        '
        Me.B_Buscar.Location = New System.Drawing.Point(438, 43)
        Me.B_Buscar.Name = "B_Buscar"
        Me.B_Buscar.Size = New System.Drawing.Size(96, 21)
        Me.B_Buscar.TabIndex = 2
        Me.B_Buscar.Text = "Buscar"
        '
        'C_Personal
        '
        Me.C_Personal.Location = New System.Drawing.Point(204, 42)
        Me.C_Personal.Name = "C_Personal"
        Me.C_Personal.Size = New System.Drawing.Size(214, 21)
        Me.C_Personal.TabIndex = 1
        Me.C_Personal.Text = "C_Personal"
        '
        'UltraLabel16
        '
        Me.UltraLabel16.Location = New System.Drawing.Point(149, 47)
        Me.UltraLabel16.Name = "UltraLabel16"
        Me.UltraLabel16.Size = New System.Drawing.Size(63, 16)
        Me.UltraLabel16.TabIndex = 187
        Me.UltraLabel16.Text = "Personal:"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 614)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(564, 16)
        Me.UltraLabel1.TabIndex = 188
        Me.UltraLabel1.Text = "El personal que aparece en el desplegable son las personas que el usuario actual " & _
    "tiene a cargo"
        '
        'frmResumenHoras
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1004, 641)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.C_Personal)
        Me.Controls.Add(Me.UltraLabel16)
        Me.Controls.Add(Me.B_Buscar)
        Me.Controls.Add(Me.C_Año)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.GRD_Resumen)
        Me.KeyPreview = True
        Me.Name = "frmResumenHoras"
        Me.Text = "Resumen de horas"
        Me.Controls.SetChildIndex(Me.GRD_Resumen, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel2, 0)
        Me.Controls.SetChildIndex(Me.C_Año, 0)
        Me.Controls.SetChildIndex(Me.B_Buscar, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel16, 0)
        Me.Controls.SetChildIndex(Me.C_Personal, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        CType(Me.C_Año, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Personal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD_Resumen As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_Año As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents B_Buscar As Infragistics.Win.Misc.UltraButton
    Friend WithEvents C_Personal As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel16 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
End Class
