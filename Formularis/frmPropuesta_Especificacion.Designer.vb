<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPropuesta_Especificacion
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GRD_Respuestas = New M_UltraGrid.m_UltraGrid()
        Me.GRD_Preguntas = New M_UltraGrid.m_UltraGrid()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.Sortir
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(674, 339)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(19, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = ">>"
        '
        'GRD_Respuestas
        '
        Me.GRD_Respuestas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Respuestas.Location = New System.Drawing.Point(702, 55)
        Me.GRD_Respuestas.Name = "GRD_Respuestas"
        Me.GRD_Respuestas.pAccessibleName = Nothing
        Me.GRD_Respuestas.pActivarBotonFiltro = False
        Me.GRD_Respuestas.pText = " "
        Me.GRD_Respuestas.Size = New System.Drawing.Size(303, 590)
        Me.GRD_Respuestas.TabIndex = 11
        '
        'GRD_Preguntas
        '
        Me.GRD_Preguntas.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GRD_Preguntas.Location = New System.Drawing.Point(12, 55)
        Me.GRD_Preguntas.Name = "GRD_Preguntas"
        Me.GRD_Preguntas.pAccessibleName = Nothing
        Me.GRD_Preguntas.pActivarBotonFiltro = False
        Me.GRD_Preguntas.pText = " "
        Me.GRD_Preguntas.Size = New System.Drawing.Size(648, 590)
        Me.GRD_Preguntas.TabIndex = 13
        '
        'frmPropuesta_Cuestionario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1017, 657)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GRD_Respuestas)
        Me.Controls.Add(Me.GRD_Preguntas)
        Me.KeyPreview = True
        Me.Name = "frmPropuesta_Especificacion"
        Me.Text = "Especificaciones de las propuestas"
        Me.Controls.SetChildIndex(Me.GRD_Preguntas, 0)
        Me.Controls.SetChildIndex(Me.GRD_Respuestas, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GRD_Respuestas As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Preguntas As M_UltraGrid.m_UltraGrid
End Class
