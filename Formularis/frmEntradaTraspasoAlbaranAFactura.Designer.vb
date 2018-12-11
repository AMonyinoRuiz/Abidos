<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntradaTraspasoAlbaranAFactura
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
        Me.TE_Codigo = New M_MaskEditor.m_MaskEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.GRD_Lineas = New M_UltraGrid.m_UltraGrid()
        Me.OP_TipusDeTraspas = New Infragistics.Win.UltraWinEditors.UltraOptionSet()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.T_NumDocumentoProveedor = New M_TextEditor.m_TextEditor()
        Me.L_NumDocumentoProveedor = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NumFactura = New M_MaskEditor.m_MaskEditor()
        Me.L_NumFactura = New Infragistics.Win.Misc.UltraLabel()
        Me.C_Empresa = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel55 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OP_TipusDeTraspas, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_NumDocumentoProveedor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Empresa, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(984, 24)
        Me.ToolForm.TabIndex = 29
        '
        'TE_Codigo
        '
        Me.TE_Codigo.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.TE_Codigo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.TE_Codigo.InputMask = "nnnnnnnnnn"
        Me.TE_Codigo.Location = New System.Drawing.Point(550, 59)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.NonAutoSizeHeight = 20
        Me.TE_Codigo.ReadOnly = True
        Me.TE_Codigo.Size = New System.Drawing.Size(120, 20)
        Me.TE_Codigo.TabIndex = 2
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(686, 59)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.ReadOnly = True
        Me.T_Descripcion.Size = New System.Drawing.Size(307, 21)
        Me.T_Descripcion.TabIndex = 3
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(550, 43)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel2.TabIndex = 181
        Me.UltraLabel2.Text = "*Código:"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(686, 44)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 182
        Me.UltraLabel5.Text = "Descripción"
        '
        'GRD_Lineas
        '
        Me.GRD_Lineas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Lineas.Location = New System.Drawing.Point(12, 150)
        Me.GRD_Lineas.Name = "GRD_Lineas"
        Me.GRD_Lineas.pAccessibleName = Nothing
        Me.GRD_Lineas.pActivarBotonFiltro = False
        Me.GRD_Lineas.pText = " "
        Me.GRD_Lineas.Size = New System.Drawing.Size(984, 361)
        Me.GRD_Lineas.TabIndex = 136
        '
        'OP_TipusDeTraspas
        '
        ValueListItem1.CheckState = System.Windows.Forms.CheckState.Checked
        ValueListItem1.DataValue = True
        ValueListItem1.DisplayText = "Traspasar a una nueva factura"
        ValueListItem2.DataValue = False
        ValueListItem2.DisplayText = "Traspasar a una factura existente"
        Me.OP_TipusDeTraspas.Items.AddRange(New Infragistics.Win.ValueListItem() {ValueListItem1, ValueListItem2})
        Me.OP_TipusDeTraspas.ItemSpacingVertical = 2
        Me.OP_TipusDeTraspas.Location = New System.Drawing.Point(187, 44)
        Me.OP_TipusDeTraspas.Name = "OP_TipusDeTraspas"
        Me.OP_TipusDeTraspas.Size = New System.Drawing.Size(224, 44)
        Me.OP_TipusDeTraspas.TabIndex = 1
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
        'T_NumDocumentoProveedor
        '
        Me.T_NumDocumentoProveedor.Location = New System.Drawing.Point(12, 59)
        Me.T_NumDocumentoProveedor.Name = "T_NumDocumentoProveedor"
        Me.T_NumDocumentoProveedor.Size = New System.Drawing.Size(152, 21)
        Me.T_NumDocumentoProveedor.TabIndex = 0
        Me.T_NumDocumentoProveedor.Text = "T_NumDocumentoProveedor"
        '
        'L_NumDocumentoProveedor
        '
        Me.L_NumDocumentoProveedor.Location = New System.Drawing.Point(12, 43)
        Me.L_NumDocumentoProveedor.Name = "L_NumDocumentoProveedor"
        Me.L_NumDocumentoProveedor.Size = New System.Drawing.Size(151, 16)
        Me.L_NumDocumentoProveedor.TabIndex = 184
        Me.L_NumDocumentoProveedor.Text = "*Nº documento proveedor:"
        '
        'T_NumFactura
        '
        Me.T_NumFactura.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_NumFactura.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.T_NumFactura.InputMask = "nnnnnnnnnn"
        Me.T_NumFactura.Location = New System.Drawing.Point(424, 59)
        Me.T_NumFactura.Name = "T_NumFactura"
        Me.T_NumFactura.NonAutoSizeHeight = 20
        Me.T_NumFactura.Size = New System.Drawing.Size(109, 20)
        Me.T_NumFactura.TabIndex = 185
        '
        'L_NumFactura
        '
        Me.L_NumFactura.Location = New System.Drawing.Point(424, 43)
        Me.L_NumFactura.Name = "L_NumFactura"
        Me.L_NumFactura.Size = New System.Drawing.Size(109, 16)
        Me.L_NumFactura.TabIndex = 186
        Me.L_NumFactura.Text = "Número de factura:"
        '
        'C_Empresa
        '
        Me.C_Empresa.Location = New System.Drawing.Point(12, 106)
        Me.C_Empresa.Name = "C_Empresa"
        Me.C_Empresa.Size = New System.Drawing.Size(152, 21)
        Me.C_Empresa.TabIndex = 228
        Me.C_Empresa.Text = "C_Empresa"
        '
        'UltraLabel55
        '
        Me.UltraLabel55.Location = New System.Drawing.Point(12, 91)
        Me.UltraLabel55.Name = "UltraLabel55"
        Me.UltraLabel55.Size = New System.Drawing.Size(96, 16)
        Me.UltraLabel55.TabIndex = 229
        Me.UltraLabel55.Text = "Empresa:"
        '
        'frmEntradaTraspasoAlbaranAFactura
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 523)
        Me.Controls.Add(Me.C_Empresa)
        Me.Controls.Add(Me.UltraLabel55)
        Me.Controls.Add(Me.T_NumFactura)
        Me.Controls.Add(Me.L_NumFactura)
        Me.Controls.Add(Me.T_NumDocumentoProveedor)
        Me.Controls.Add(Me.L_NumDocumentoProveedor)
        Me.Controls.Add(Me.TE_Codigo)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.GRD_Lineas)
        Me.Controls.Add(Me.UltraLabel5)
        Me.Controls.Add(Me.OP_TipusDeTraspas)
        Me.KeyPreview = True
        Me.Name = "frmEntradaTraspasoAlbaranAFactura"
        Me.Text = "Facturar..."
        Me.Controls.SetChildIndex(Me.OP_TipusDeTraspas, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.GRD_Lineas, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel2, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.TE_Codigo, 0)
        Me.Controls.SetChildIndex(Me.L_NumDocumentoProveedor, 0)
        Me.Controls.SetChildIndex(Me.T_NumDocumentoProveedor, 0)
        Me.Controls.SetChildIndex(Me.L_NumFactura, 0)
        Me.Controls.SetChildIndex(Me.T_NumFactura, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel55, 0)
        Me.Controls.SetChildIndex(Me.C_Empresa, 0)
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OP_TipusDeTraspas, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_NumDocumentoProveedor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Empresa, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents GRD_Lineas As M_UltraGrid.m_UltraGrid
    Friend WithEvents OP_TipusDeTraspas As Infragistics.Win.UltraWinEditors.UltraOptionSet
    Friend WithEvents TE_Codigo As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NumDocumentoProveedor As M_TextEditor.m_TextEditor
    Friend WithEvents L_NumDocumentoProveedor As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NumFactura As M_MaskEditor.m_MaskEditor
    Friend WithEvents L_NumFactura As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_Empresa As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel55 As Infragistics.Win.Misc.UltraLabel
End Class
