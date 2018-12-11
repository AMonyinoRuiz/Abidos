<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNotificacionesUsuarios
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
        Me.GRD_TipoNotificacion = New M_UltraGrid.m_UltraGrid()
        Me.C_Usuario = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.C_Usuario, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.Sortir
        Me.ToolForm.Size = New System.Drawing.Size(987, 24)
        '
        'GRD_TipoNotificacion
        '
        Me.GRD_TipoNotificacion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_TipoNotificacion.Location = New System.Drawing.Point(12, 84)
        Me.GRD_TipoNotificacion.Name = "GRD_TipoNotificacion"
        Me.GRD_TipoNotificacion.pAccessibleName = Nothing
        Me.GRD_TipoNotificacion.pActivarBotonFiltro = False
        Me.GRD_TipoNotificacion.pText = " "
        Me.GRD_TipoNotificacion.Size = New System.Drawing.Size(987, 545)
        Me.GRD_TipoNotificacion.TabIndex = 131
        '
        'C_Usuario
        '
        Me.C_Usuario.Location = New System.Drawing.Point(12, 57)
        Me.C_Usuario.Name = "C_Usuario"
        Me.C_Usuario.Size = New System.Drawing.Size(251, 21)
        Me.C_Usuario.TabIndex = 132
        Me.C_Usuario.Text = "C_Usuario"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(96, 16)
        Me.UltraLabel3.TabIndex = 133
        Me.UltraLabel3.Text = "Usuario:"
        '
        'frmNotificacionesUsuarios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 641)
        Me.Controls.Add(Me.C_Usuario)
        Me.Controls.Add(Me.UltraLabel3)
        Me.Controls.Add(Me.GRD_TipoNotificacion)
        Me.KeyPreview = True
        Me.Name = "frmNotificacionesUsuarios"
        Me.Text = "Asignación de notificaciones a usuarios"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.GRD_TipoNotificacion, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel3, 0)
        Me.Controls.SetChildIndex(Me.C_Usuario, 0)
        CType(Me.C_Usuario, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD_TipoNotificacion As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_Usuario As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
End Class
