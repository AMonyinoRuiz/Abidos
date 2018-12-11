<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManteniment_Script
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
        Me.RT1 = New DevExpress.XtraRichEdit.RichEditControl()
        Me.RichEditBarController1 = New DevExpress.XtraRichEdit.UI.RichEditBarController()
        Me.InsertPageBreakItem2 = New DevExpress.XtraRichEdit.UI.InsertPageBreakItem()
        CType(Me.RichEditBarController1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Location = New System.Drawing.Point(502, 502)
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        '
        'RT1
        '
        Me.RT1.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Draft
        Me.RT1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RT1.Location = New System.Drawing.Point(12, 12)
        Me.RT1.Name = "RT1"
        Me.RT1.Options.Fields.UseCurrentCultureDateTimeFormat = False
        Me.RT1.Options.MailMerge.KeepLastParagraph = False
        Me.RT1.Size = New System.Drawing.Size(812, 484)
        Me.RT1.TabIndex = 1
        Me.RT1.Text = "RT1"
        '
        'RichEditBarController1
        '
        Me.RichEditBarController1.BarItems.Add(Me.InsertPageBreakItem2)
        Me.RichEditBarController1.Control = Me.RT1
        '
        'InsertPageBreakItem2
        '
        Me.InsertPageBreakItem2.Id = -1
        Me.InsertPageBreakItem2.Name = "InsertPageBreakItem2"
        '
        'frmManteniment_Script
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(845, 538)
        Me.Controls.Add(Me.RT1)
        Me.KeyPreview = True
        Me.Name = "frmManteniment_Script"
        Me.Text = "frmManteniment_Script"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.RT1, 0)
        CType(Me.RichEditBarController1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RT1 As DevExpress.XtraRichEdit.RichEditControl
    Friend WithEvents RichEditBarController1 As DevExpress.XtraRichEdit.UI.RichEditBarController
    Friend WithEvents InsertPageBreakItem2 As DevExpress.XtraRichEdit.UI.InsertPageBreakItem
End Class
