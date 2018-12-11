<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProducto_Alternativo
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
        Me.TE_Producto_Codigo = New M_TextEditor.m_TextEditor()
        Me.T_Producto_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Unidades = New M_MaskEditor.m_MaskEditor()
        Me.L_klkl = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.TE_Producto_Codigo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Producto_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.SeleccionarSortir
        Me.ToolForm.Size = New System.Drawing.Size(826, 24)
        '
        'TE_Producto_Codigo
        '
        Me.TE_Producto_Codigo.Location = New System.Drawing.Point(14, 59)
        Me.TE_Producto_Codigo.Name = "TE_Producto_Codigo"
        Me.TE_Producto_Codigo.Size = New System.Drawing.Size(173, 21)
        Me.TE_Producto_Codigo.TabIndex = 0
        Me.TE_Producto_Codigo.Text = "TE_Producto_Codigo"
        '
        'T_Producto_Descripcion
        '
        Me.T_Producto_Descripcion.Location = New System.Drawing.Point(204, 59)
        Me.T_Producto_Descripcion.Name = "T_Producto_Descripcion"
        Me.T_Producto_Descripcion.ReadOnly = True
        Me.T_Producto_Descripcion.Size = New System.Drawing.Size(517, 21)
        Me.T_Producto_Descripcion.TabIndex = 1
        Me.T_Producto_Descripcion.Text = "T_Producto_Descripcion"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(116, 16)
        Me.UltraLabel1.TabIndex = 19
        Me.UltraLabel1.Text = "*Código Producto:"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(204, 44)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel4.TabIndex = 20
        Me.UltraLabel4.Text = "*Descripción del producto"
        '
        'T_Unidades
        '
        Me.T_Unidades.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_Unidades.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Integer]
        Me.T_Unidades.InputMask = "nnnn"
        Me.T_Unidades.Location = New System.Drawing.Point(737, 60)
        Me.T_Unidades.Name = "T_Unidades"
        Me.T_Unidades.Size = New System.Drawing.Size(101, 20)
        Me.T_Unidades.TabIndex = 2
        '
        'L_klkl
        '
        Me.L_klkl.Location = New System.Drawing.Point(737, 44)
        Me.L_klkl.Name = "L_klkl"
        Me.L_klkl.Size = New System.Drawing.Size(66, 16)
        Me.L_klkl.TabIndex = 22
        Me.L_klkl.Text = "*Unidades:"
        '
        'frmProducto_Alternativo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(850, 93)
        Me.Controls.Add(Me.T_Unidades)
        Me.Controls.Add(Me.L_klkl)
        Me.Controls.Add(Me.TE_Producto_Codigo)
        Me.Controls.Add(Me.T_Producto_Descripcion)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel4)
        Me.KeyPreview = True
        Me.Name = "frmProducto_Alternativo"
        Me.Text = "Assignación de producto alternativo"
        Me.Controls.SetChildIndex(Me.UltraLabel4, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Producto_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.TE_Producto_Codigo, 0)
        Me.Controls.SetChildIndex(Me.L_klkl, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.T_Unidades, 0)
        CType(Me.TE_Producto_Codigo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Producto_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TE_Producto_Codigo As M_TextEditor.m_TextEditor
    Friend WithEvents T_Producto_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Unidades As M_MaskEditor.m_MaskEditor
    Friend WithEvents L_klkl As Infragistics.Win.Misc.UltraLabel
End Class
