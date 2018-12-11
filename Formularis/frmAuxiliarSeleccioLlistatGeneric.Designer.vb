<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAuxiliarSeleccioLlistatGeneric
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
        Me.TE_Seleccio = New M_TextEditor.m_TextEditor()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.TE_Seleccio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(161, 24)
        '
        'TE_Seleccio
        '
        Me.TE_Seleccio.Location = New System.Drawing.Point(12, 59)
        Me.TE_Seleccio.Name = "TE_Seleccio"
        Me.TE_Seleccio.ReadOnly = True
        Me.TE_Seleccio.Size = New System.Drawing.Size(619, 21)
        Me.TE_Seleccio.TabIndex = 0
        Me.TE_Seleccio.Text = "TE_Seleccio"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 142
        Me.UltraLabel5.Text = "*Descripción:"
        '
        'frmAuxiliarSeleccioLlistatGeneric
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(660, 113)
        Me.Controls.Add(Me.TE_Seleccio)
        Me.Controls.Add(Me.UltraLabel5)
        Me.KeyPreview = True
        Me.Name = "frmAuxiliarSeleccioLlistatGeneric"
        Me.Text = "Seleccione"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.TE_Seleccio, 0)
        CType(Me.TE_Seleccio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TE_Seleccio As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
End Class
