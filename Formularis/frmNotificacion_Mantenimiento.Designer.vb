<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNotificacion_Mantenimiento
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
        Me.T_CodigoNotificacion = New M_TextEditor.m_TextEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.DT_Limite = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.DT_Alta = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel13 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel28 = New Infragistics.Win.Misc.UltraLabel()
        Me.C_Usuario = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Observaciones = New M_TextEditor.m_TextEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.T_CodigoNotificacion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Limite, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Usuario, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Observaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(905, 24)
        '
        'T_CodigoNotificacion
        '
        Me.T_CodigoNotificacion.Location = New System.Drawing.Point(14, 59)
        Me.T_CodigoNotificacion.Name = "T_CodigoNotificacion"
        Me.T_CodigoNotificacion.ReadOnly = True
        Me.T_CodigoNotificacion.Size = New System.Drawing.Size(114, 21)
        Me.T_CodigoNotificacion.TabIndex = 17
        Me.T_CodigoNotificacion.Text = "T_CodigoNotificacion"
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(145, 59)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(547, 21)
        Me.T_Descripcion.TabIndex = 0
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(116, 16)
        Me.UltraLabel1.TabIndex = 19
        Me.UltraLabel1.Text = "*Código notificación:"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(145, 44)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel4.TabIndex = 20
        Me.UltraLabel4.Text = "*Descripción"
        '
        'DT_Limite
        '
        Me.DT_Limite.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Limite.Location = New System.Drawing.Point(817, 59)
        Me.DT_Limite.Name = "DT_Limite"
        Me.DT_Limite.Size = New System.Drawing.Size(100, 21)
        Me.DT_Limite.TabIndex = 2
        Me.DT_Limite.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'DT_Alta
        '
        Me.DT_Alta.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Alta.Location = New System.Drawing.Point(708, 59)
        Me.DT_Alta.Name = "DT_Alta"
        Me.DT_Alta.ReadOnly = True
        Me.DT_Alta.Size = New System.Drawing.Size(90, 21)
        Me.DT_Alta.TabIndex = 1
        Me.DT_Alta.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel13
        '
        Me.UltraLabel13.Location = New System.Drawing.Point(817, 43)
        Me.UltraLabel13.Name = "UltraLabel13"
        Me.UltraLabel13.Size = New System.Drawing.Size(90, 16)
        Me.UltraLabel13.TabIndex = 153
        Me.UltraLabel13.Text = "Fecha límite:"
        '
        'UltraLabel28
        '
        Me.UltraLabel28.Location = New System.Drawing.Point(708, 43)
        Me.UltraLabel28.Name = "UltraLabel28"
        Me.UltraLabel28.Size = New System.Drawing.Size(90, 16)
        Me.UltraLabel28.TabIndex = 152
        Me.UltraLabel28.Text = "*Fecha de alta:"
        '
        'C_Usuario
        '
        Me.C_Usuario.Location = New System.Drawing.Point(14, 111)
        Me.C_Usuario.Name = "C_Usuario"
        Me.C_Usuario.Size = New System.Drawing.Size(435, 21)
        Me.C_Usuario.TabIndex = 3
        Me.C_Usuario.Text = "C_Usuario"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(14, 94)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(114, 16)
        Me.UltraLabel2.TabIndex = 155
        Me.UltraLabel2.Text = "*Destinatario:"
        '
        'T_Observaciones
        '
        Me.T_Observaciones.Location = New System.Drawing.Point(14, 161)
        Me.T_Observaciones.Multiline = True
        Me.T_Observaciones.Name = "T_Observaciones"
        Me.T_Observaciones.Size = New System.Drawing.Size(903, 165)
        Me.T_Observaciones.TabIndex = 4
        Me.T_Observaciones.Text = "T_Observaciones"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(14, 146)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel3.TabIndex = 157
        Me.UltraLabel3.Text = "Observaciones:"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'frmNotificacion_Mantenimiento
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(929, 338)
        Me.Controls.Add(Me.T_Observaciones)
        Me.Controls.Add(Me.UltraLabel3)
        Me.Controls.Add(Me.C_Usuario)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.DT_Limite)
        Me.Controls.Add(Me.DT_Alta)
        Me.Controls.Add(Me.UltraLabel13)
        Me.Controls.Add(Me.UltraLabel28)
        Me.Controls.Add(Me.T_CodigoNotificacion)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel4)
        Me.KeyPreview = True
        Me.Name = "frmNotificacion_Mantenimiento"
        Me.Text = "Mantenimiento de notificaciones"
        Me.Controls.SetChildIndex(Me.UltraLabel4, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.T_CodigoNotificacion, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel28, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel13, 0)
        Me.Controls.SetChildIndex(Me.DT_Alta, 0)
        Me.Controls.SetChildIndex(Me.DT_Limite, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel2, 0)
        Me.Controls.SetChildIndex(Me.C_Usuario, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel3, 0)
        Me.Controls.SetChildIndex(Me.T_Observaciones, 0)
        CType(Me.T_CodigoNotificacion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Limite, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Usuario, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Observaciones, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents T_CodigoNotificacion As M_TextEditor.m_TextEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents DT_Limite As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents DT_Alta As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel13 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel28 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_Usuario As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Observaciones As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
End Class
