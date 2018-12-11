<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInstalacionFusionarPropuestas
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
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.T_Persona = New M_TextEditor.m_TextEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.L_klkl = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.CH_TraspasarFicheros = New DevExpress.XtraEditors.CheckEdit()
        Me.CH_TraspasarPlanos = New DevExpress.XtraEditors.CheckEdit()
        CType(Me.T_Persona,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.T_Descripcion,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.CH_TraspasarFicheros.Properties,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.CH_TraspasarPlanos.Properties,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.TabIndex = 29
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(959, 328)
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
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(1, 20)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(694, 303)
        '
        'T_Persona
        '
        Me.T_Persona.Location = New System.Drawing.Point(776, 63)
        Me.T_Persona.Name = "T_Persona"
        Me.T_Persona.Size = New System.Drawing.Size(176, 21)
        Me.T_Persona.TabIndex = 31
        Me.T_Persona.Text = "T_Persona"
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(12, 63)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(749, 21)
        Me.T_Descripcion.TabIndex = 30
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'L_klkl
        '
        Me.L_klkl.Location = New System.Drawing.Point(776, 47)
        Me.L_klkl.Name = "L_klkl"
        Me.L_klkl.Size = New System.Drawing.Size(176, 16)
        Me.L_klkl.TabIndex = 33
        Me.L_klkl.Text = "Persona a la que va dirigido:"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(13, 49)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel4.TabIndex = 32
        Me.UltraLabel4.Text = "*Descripción de la propuesta"
        '
        'CH_TraspasarFicheros
        '
        Me.CH_TraspasarFicheros.EditValue = True
        Me.CH_TraspasarFicheros.Location = New System.Drawing.Point(13, 96)
        Me.CH_TraspasarFicheros.Name = "CH_TraspasarFicheros"
        Me.CH_TraspasarFicheros.Properties.Caption = "Traspasar ficheros"
        Me.CH_TraspasarFicheros.Size = New System.Drawing.Size(143, 19)
        Me.CH_TraspasarFicheros.TabIndex = 34
        '
        'CH_TraspasarPlanos
        '
        Me.CH_TraspasarPlanos.EditValue = True
        Me.CH_TraspasarPlanos.Location = New System.Drawing.Point(13, 121)
        Me.CH_TraspasarPlanos.Name = "CH_TraspasarPlanos"
        Me.CH_TraspasarPlanos.Properties.Caption = "Traspasar planos"
        Me.CH_TraspasarPlanos.Size = New System.Drawing.Size(143, 19)
        Me.CH_TraspasarPlanos.TabIndex = 35
        '
        'frmInstalacionFusionarPropuestas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 523)
        Me.Controls.Add(Me.CH_TraspasarPlanos)
        Me.Controls.Add(Me.CH_TraspasarFicheros)
        Me.Controls.Add(Me.T_Persona)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.L_klkl)
        Me.Controls.Add(Me.UltraLabel4)
        Me.KeyPreview = true
        Me.Name = "frmInstalacionFusionarPropuestas"
        Me.Text = "Fusionar propuestas"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel4, 0)
        Me.Controls.SetChildIndex(Me.L_klkl, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.T_Persona, 0)
        Me.Controls.SetChildIndex(Me.CH_TraspasarFicheros, 0)
        Me.Controls.SetChildIndex(Me.CH_TraspasarPlanos, 0)
        CType(Me.T_Persona,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.T_Descripcion,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.CH_TraspasarFicheros.Properties,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.CH_TraspasarPlanos.Properties,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents T_Persona As M_TextEditor.m_TextEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents L_klkl As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents CH_TraspasarFicheros As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents CH_TraspasarPlanos As DevExpress.XtraEditors.CheckEdit
End Class
