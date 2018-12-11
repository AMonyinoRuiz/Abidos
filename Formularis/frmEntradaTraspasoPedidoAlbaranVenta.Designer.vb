<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntradaTraspasoPedidoAlbaranVenta
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
        Dim ValueListItem1 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Dim ValueListItem2 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Me.GRD_NS = New M_UltraGrid.m_UltraGrid()
        Me.GRD_Linea = New M_UltraGrid.m_UltraGrid()
        Me.TE_Codigo = New M_MaskEditor.m_MaskEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.OP_TipusDeTraspas = New Infragistics.Win.UltraWinEditors.UltraOptionSet()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OP_TipusDeTraspas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(966, 24)
        '
        'GRD_NS
        '
        Me.GRD_NS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_NS.Location = New System.Drawing.Point(547, 100)
        Me.GRD_NS.Name = "GRD_NS"
        Me.GRD_NS.pAccessibleName = Nothing
        Me.GRD_NS.pActivarBotonFiltro = False
        Me.GRD_NS.pText = " "
        Me.GRD_NS.Size = New System.Drawing.Size(431, 449)
        Me.GRD_NS.TabIndex = 217
        '
        'GRD_Linea
        '
        Me.GRD_Linea.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Linea.Location = New System.Drawing.Point(12, 100)
        Me.GRD_Linea.Name = "GRD_Linea"
        Me.GRD_Linea.pAccessibleName = Nothing
        Me.GRD_Linea.pActivarBotonFiltro = False
        Me.GRD_Linea.pText = " "
        Me.GRD_Linea.Size = New System.Drawing.Size(529, 449)
        Me.GRD_Linea.TabIndex = 216
        '
        'TE_Codigo
        '
        Me.TE_Codigo.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.TE_Codigo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.TE_Codigo.InputMask = "nnnnnnnnnn"
        Me.TE_Codigo.Location = New System.Drawing.Point(243, 60)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.NonAutoSizeHeight = 20
        Me.TE_Codigo.ReadOnly = True
        Me.TE_Codigo.Size = New System.Drawing.Size(120, 20)
        Me.TE_Codigo.TabIndex = 1
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(374, 59)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.ReadOnly = True
        Me.T_Descripcion.Size = New System.Drawing.Size(312, 21)
        Me.T_Descripcion.TabIndex = 2
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(243, 44)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel2.TabIndex = 221
        Me.UltraLabel2.Text = "*Código:"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(373, 43)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 222
        Me.UltraLabel5.Text = "Descripción"
        '
        'OP_TipusDeTraspas
        '
        ValueListItem1.CheckState = System.Windows.Forms.CheckState.Checked
        ValueListItem1.DataValue = True
        ValueListItem1.DisplayText = "Traspasar a un nuevo albarán"
        ValueListItem2.DataValue = False
        ValueListItem2.DisplayText = "Traspasar a un albarán existente"
        Me.OP_TipusDeTraspas.Items.AddRange(New Infragistics.Win.ValueListItem() {ValueListItem1, ValueListItem2})
        Me.OP_TipusDeTraspas.ItemSpacingVertical = 2
        Me.OP_TipusDeTraspas.Location = New System.Drawing.Point(12, 43)
        Me.OP_TipusDeTraspas.Name = "OP_TipusDeTraspas"
        Me.OP_TipusDeTraspas.Size = New System.Drawing.Size(224, 44)
        Me.OP_TipusDeTraspas.TabIndex = 0
        '
        'frmEntradaTraspasoPedidoAlbaranVenta
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(990, 561)
        Me.Controls.Add(Me.TE_Codigo)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.UltraLabel5)
        Me.Controls.Add(Me.OP_TipusDeTraspas)
        Me.Controls.Add(Me.GRD_NS)
        Me.Controls.Add(Me.GRD_Linea)
        Me.KeyPreview = True
        Me.Name = "frmEntradaTraspasoPedidoAlbaranVenta"
        Me.Text = "Traspaso de pedido a albarán de venta"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.GRD_Linea, 0)
        Me.Controls.SetChildIndex(Me.GRD_NS, 0)
        Me.Controls.SetChildIndex(Me.OP_TipusDeTraspas, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel2, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.TE_Codigo, 0)
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OP_TipusDeTraspas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD_NS As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Linea As M_UltraGrid.m_UltraGrid
    Friend WithEvents TE_Codigo As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents OP_TipusDeTraspas As Infragistics.Win.UltraWinEditors.UltraOptionSet
End Class
