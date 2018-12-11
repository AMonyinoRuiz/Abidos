<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAuxiliarDesdeHasta
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
        Me.T_Desde = New M_MaskEditor.m_MaskEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Hasta = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(628, 24)
        '
        'T_Desde
        '
        Me.T_Desde.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_Desde.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.T_Desde.InputMask = "nnnnnnnnnn"
        Me.T_Desde.Location = New System.Drawing.Point(379, 60)
        Me.T_Desde.Name = "T_Desde"
        Me.T_Desde.Size = New System.Drawing.Size(120, 20)
        Me.T_Desde.TabIndex = 1
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(12, 59)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(355, 21)
        Me.T_Descripcion.TabIndex = 0
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(379, 43)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel1.TabIndex = 141
        Me.UltraLabel1.Text = "*Desde:"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 142
        Me.UltraLabel5.Text = "*Descripción"
        '
        'T_Hasta
        '
        Me.T_Hasta.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_Hasta.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.T_Hasta.InputMask = "nnnnnnnnnn"
        Me.T_Hasta.Location = New System.Drawing.Point(511, 60)
        Me.T_Hasta.Name = "T_Hasta"
        Me.T_Hasta.Size = New System.Drawing.Size(120, 20)
        Me.T_Hasta.TabIndex = 2
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(511, 43)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel2.TabIndex = 144
        Me.UltraLabel2.Text = "*Hasta:"
        '
        'frmAuxiliarDesdeHasta
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(652, 118)
        Me.Controls.Add(Me.T_Hasta)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.T_Desde)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel5)
        Me.KeyPreview = True
        Me.Name = "frmAuxiliarDesdeHasta"
        Me.Text = "Multiplicador"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.T_Desde, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel2, 0)
        Me.Controls.SetChildIndex(Me.T_Hasta, 0)
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents T_Desde As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Hasta As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
End Class
