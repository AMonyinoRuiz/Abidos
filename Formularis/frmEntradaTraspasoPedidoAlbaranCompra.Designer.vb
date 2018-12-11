<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntradaTraspasoPedidoAlbaranCompra
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
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.TE_Codigo = New M_MaskEditor.m_MaskEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NumDocumentoProveedor = New M_TextEditor.m_TextEditor()
        Me.L_NumDocumentoProveedor = New Infragistics.Win.Misc.UltraLabel()
        Me.GRD_Lineas = New M_UltraGrid.m_UltraGrid()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.OP_TipusDeTraspas = New Infragistics.Win.UltraWinEditors.UltraOptionSet()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Ficheros = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.Tab_Principal = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage2 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_NumDocumentoProveedor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OP_TipusDeTraspas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl2.SuspendLayout()
        CType(Me.Tab_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tab_Principal.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(987, 24)
        Me.ToolForm.TabIndex = 29
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.TE_Codigo)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Descripcion)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel2)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel5)
        Me.UltraTabPageControl1.Controls.Add(Me.T_NumDocumentoProveedor)
        Me.UltraTabPageControl1.Controls.Add(Me.L_NumDocumentoProveedor)
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Lineas)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel1)
        Me.UltraTabPageControl1.Controls.Add(Me.OP_TipusDeTraspas)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(983, 442)
        '
        'TE_Codigo
        '
        Me.TE_Codigo.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.TE_Codigo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.TE_Codigo.InputMask = "nnnnnnnnnn"
        Me.TE_Codigo.Location = New System.Drawing.Point(405, 23)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.ReadOnly = True
        Me.TE_Codigo.Size = New System.Drawing.Size(120, 20)
        Me.TE_Codigo.TabIndex = 2
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(536, 22)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.ReadOnly = True
        Me.T_Descripcion.Size = New System.Drawing.Size(312, 21)
        Me.T_Descripcion.TabIndex = 3
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(405, 7)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel2.TabIndex = 181
        Me.UltraLabel2.Text = "*Código:"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(535, 6)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 182
        Me.UltraLabel5.Text = "Descripción"
        '
        'T_NumDocumentoProveedor
        '
        Me.T_NumDocumentoProveedor.Location = New System.Drawing.Point(3, 22)
        Me.T_NumDocumentoProveedor.Name = "T_NumDocumentoProveedor"
        Me.T_NumDocumentoProveedor.Size = New System.Drawing.Size(152, 21)
        Me.T_NumDocumentoProveedor.TabIndex = 0
        Me.T_NumDocumentoProveedor.Text = "T_NumDocumentoProveedor"
        '
        'L_NumDocumentoProveedor
        '
        Me.L_NumDocumentoProveedor.Location = New System.Drawing.Point(4, 7)
        Me.L_NumDocumentoProveedor.Name = "L_NumDocumentoProveedor"
        Me.L_NumDocumentoProveedor.Size = New System.Drawing.Size(151, 16)
        Me.L_NumDocumentoProveedor.TabIndex = 178
        Me.L_NumDocumentoProveedor.Text = "*Nº documento proveedor:"
        '
        'GRD_Lineas
        '
        Me.GRD_Lineas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Lineas.Location = New System.Drawing.Point(3, 49)
        Me.GRD_Lineas.Name = "GRD_Lineas"
        Me.GRD_Lineas.pAccessibleName = Nothing
        Me.GRD_Lineas.pActivarBotonFiltro = False
        Me.GRD_Lineas.pText = " "
        Me.GRD_Lineas.Size = New System.Drawing.Size(977, 369)
        Me.GRD_Lineas.TabIndex = 136
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.UltraLabel1.Location = New System.Drawing.Point(3, 421)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(584, 22)
        Me.UltraLabel1.TabIndex = 137
        Me.UltraLabel1.Text = "*Las líneas que no se pueden deseleccionar son líneas que tienen otros artículos " & _
    "relacionados"
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
        Me.OP_TipusDeTraspas.Location = New System.Drawing.Point(175, 7)
        Me.OP_TipusDeTraspas.Name = "OP_TipusDeTraspas"
        Me.OP_TipusDeTraspas.Size = New System.Drawing.Size(224, 44)
        Me.OP_TipusDeTraspas.TabIndex = 1
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.GRD_Ficheros)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(983, 442)
        '
        'GRD_Ficheros
        '
        Me.GRD_Ficheros.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Ficheros.Location = New System.Drawing.Point(3, 14)
        Me.GRD_Ficheros.Name = "GRD_Ficheros"
        Me.GRD_Ficheros.pAccessibleName = Nothing
        Me.GRD_Ficheros.pActivarBotonFiltro = False
        Me.GRD_Ficheros.pText = " "
        Me.GRD_Ficheros.Size = New System.Drawing.Size(974, 403)
        Me.GRD_Ficheros.TabIndex = 137
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
        'Tab_Principal
        '
        Me.Tab_Principal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tab_Principal.Controls.Add(Me.UltraTabSharedControlsPage2)
        Me.Tab_Principal.Controls.Add(Me.UltraTabPageControl1)
        Me.Tab_Principal.Controls.Add(Me.UltraTabPageControl2)
        Me.Tab_Principal.Location = New System.Drawing.Point(12, 43)
        Me.Tab_Principal.Name = "Tab_Principal"
        Me.Tab_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage2
        Me.Tab_Principal.Size = New System.Drawing.Size(987, 468)
        Me.Tab_Principal.TabIndex = 138
        UltraTab1.Key = "General"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Lineas del pedido pendientes de traspasar"
        UltraTab2.Key = "Ficheros"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Ficheros de la propuesta"
        Me.Tab_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2})
        '
        'UltraTabSharedControlsPage2
        '
        Me.UltraTabSharedControlsPage2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage2.Name = "UltraTabSharedControlsPage2"
        Me.UltraTabSharedControlsPage2.Size = New System.Drawing.Size(983, 442)
        '
        'frmEntradaTraspasoPedidoAlbaranCompra
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 523)
        Me.Controls.Add(Me.Tab_Principal)
        Me.KeyPreview = True
        Me.Name = "frmEntradaTraspasoPedidoAlbaranCompra"
        Me.Text = "Traspaso"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.Tab_Principal, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl1.PerformLayout()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_NumDocumentoProveedor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OP_TipusDeTraspas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl2.ResumeLayout(False)
        CType(Me.Tab_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tab_Principal.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents GRD_Lineas As M_UltraGrid.m_UltraGrid
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Tab_Principal As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage2 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Ficheros As M_UltraGrid.m_UltraGrid
    Friend WithEvents OP_TipusDeTraspas As Infragistics.Win.UltraWinEditors.UltraOptionSet
    Friend WithEvents T_NumDocumentoProveedor As M_TextEditor.m_TextEditor
    Friend WithEvents L_NumDocumentoProveedor As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Codigo As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
End Class
