<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProvaMemoria
    Inherits M_GenericForm.frmBase 'System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProvaMemoria))
        Me.M_RichText1 = New M_RichText.M_RichText()
        Me.M_RichText2 = New M_RichText.M_RichText()
        Me.M_RichText3 = New M_RichText.M_RichText()
        Me.UltraButton1 = New Infragistics.Win.Misc.UltraButton()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        '
        'M_RichText1
        '
        Me.M_RichText1.Location = New System.Drawing.Point(49, 118)
        Me.M_RichText1.Name = "M_RichText1"
        Me.M_RichText1.pEnable = True
        Me.M_RichText1.pText = resources.GetString("M_RichText1.pText")
        Me.M_RichText1.pTextEspecial = "RichEditControl1"
        Me.M_RichText1.Size = New System.Drawing.Size(221, 234)
        Me.M_RichText1.TabIndex = 1
        '
        'M_RichText2
        '
        Me.M_RichText2.Location = New System.Drawing.Point(318, 118)
        Me.M_RichText2.Name = "M_RichText2"
        Me.M_RichText2.pEnable = True
        Me.M_RichText2.pText = resources.GetString("M_RichText2.pText")
        Me.M_RichText2.pTextEspecial = "RichEditControl1"
        Me.M_RichText2.Size = New System.Drawing.Size(221, 234)
        Me.M_RichText2.TabIndex = 2
        '
        'M_RichText3
        '
        Me.M_RichText3.Location = New System.Drawing.Point(571, 118)
        Me.M_RichText3.Name = "M_RichText3"
        Me.M_RichText3.pEnable = True
        Me.M_RichText3.pText = resources.GetString("M_RichText3.pText")
        Me.M_RichText3.pTextEspecial = "RichEditControl1"
        Me.M_RichText3.Size = New System.Drawing.Size(221, 234)
        Me.M_RichText3.TabIndex = 3
        '
        'UltraButton1
        '
        Me.UltraButton1.Location = New System.Drawing.Point(232, 440)
        Me.UltraButton1.Name = "UltraButton1"
        Me.UltraButton1.Size = New System.Drawing.Size(174, 39)
        Me.UltraButton1.TabIndex = 4
        Me.UltraButton1.Text = "UltraButton1"
        '
        'frmProvaMemoria
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(888, 590)
        Me.Controls.Add(Me.UltraButton1)
        Me.Controls.Add(Me.M_RichText3)
        Me.Controls.Add(Me.M_RichText2)
        Me.Controls.Add(Me.M_RichText1)
        Me.KeyPreview = True
        Me.Name = "frmProvaMemoria"
        Me.Text = "frmProvaMemoria"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.M_RichText1, 0)
        Me.Controls.SetChildIndex(Me.M_RichText2, 0)
        Me.Controls.SetChildIndex(Me.M_RichText3, 0)
        Me.Controls.SetChildIndex(Me.UltraButton1, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents M_RichText1 As M_RichText.M_RichText
    Friend WithEvents M_RichText2 As M_RichText.M_RichText
    Friend WithEvents M_RichText3 As M_RichText.M_RichText
    Friend WithEvents UltraButton1 As Infragistics.Win.Misc.UltraButton
End Class
