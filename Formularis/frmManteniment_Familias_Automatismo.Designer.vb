<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManteniment_Familias_Automatismo
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
        Me.GRD = New M_UltraGrid.m_UltraGrid()
        Me.CH_Imprimible = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.CH_Verificable = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.C_Vision = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.CH_Imprimible, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CH_Verificable, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Vision, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Location = New System.Drawing.Point(475, 512)
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(288, 24)
        '
        'GRD
        '
        Me.GRD.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD.Location = New System.Drawing.Point(12, 72)
        Me.GRD.Name = "GRD"
        Me.GRD.pAccessibleName = Nothing
        Me.GRD.pActivarBotonFiltro = False
        Me.GRD.pText = " "
        Me.GRD.Size = New System.Drawing.Size(751, 421)
        Me.GRD.TabIndex = 1
        '
        'CH_Imprimible
        '
        Me.CH_Imprimible.Location = New System.Drawing.Point(18, 30)
        Me.CH_Imprimible.Name = "CH_Imprimible"
        Me.CH_Imprimible.Size = New System.Drawing.Size(88, 25)
        Me.CH_Imprimible.TabIndex = 0
        Me.CH_Imprimible.Text = "Imprimible"
        '
        'CH_Verificable
        '
        Me.CH_Verificable.Location = New System.Drawing.Point(117, 30)
        Me.CH_Verificable.Name = "CH_Verificable"
        Me.CH_Verificable.Size = New System.Drawing.Size(81, 25)
        Me.CH_Verificable.TabIndex = 1
        Me.CH_Verificable.Text = "Verificable"
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(210, 31)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(346, 21)
        Me.T_Descripcion.TabIndex = 2
        Me.T_Descripcion.Text = "T_Valor"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(211, 17)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel4.TabIndex = 78
        Me.UltraLabel4.Text = "Valor:"
        '
        'C_Vision
        '
        Me.C_Vision.Location = New System.Drawing.Point(577, 31)
        Me.C_Vision.Name = "C_Vision"
        Me.C_Vision.Size = New System.Drawing.Size(186, 21)
        Me.C_Vision.TabIndex = 3
        Me.C_Vision.Text = "C_Vision"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(577, 15)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(96, 16)
        Me.UltraLabel3.TabIndex = 120
        Me.UltraLabel3.Text = "Visión:"
        '
        'frmManteniment_Familias_Automatismo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(775, 548)
        Me.Controls.Add(Me.C_Vision)
        Me.Controls.Add(Me.UltraLabel3)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel4)
        Me.Controls.Add(Me.CH_Verificable)
        Me.Controls.Add(Me.CH_Imprimible)
        Me.Controls.Add(Me.GRD)
        Me.KeyPreview = True
        Me.Name = "frmManteniment_Familias_Automatismo"
        Me.Text = "Añadir característica a los artículos seleccionados"
        Me.Controls.SetChildIndex(Me.GRD, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.CH_Imprimible, 0)
        Me.Controls.SetChildIndex(Me.CH_Verificable, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel4, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel3, 0)
        Me.Controls.SetChildIndex(Me.C_Vision, 0)
        CType(Me.CH_Imprimible, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CH_Verificable, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Vision, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD As M_UltraGrid.m_UltraGrid
    Friend WithEvents CH_Imprimible As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents CH_Verificable As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_Vision As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
End Class
