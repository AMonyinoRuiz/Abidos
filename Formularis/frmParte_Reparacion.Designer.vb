<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmParte_Reparacion
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
        Dim ValueListItem3 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Me.T_Detalle = New M_TextEditor.m_TextEditor()
        Me.C_Personal = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.TE_Producto = New M_TextEditor.m_TextEditor()
        Me.UltraLabel13 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel16 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.UltraLabel22 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.DT_Fecha = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel17 = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Producto_Anterior = New M_TextEditor.m_TextEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.Panel_SubstitucionArticulo = New System.Windows.Forms.Panel()
        Me.Panel_ReparacionExterna = New Infragistics.Win.Misc.UltraPanel()
        Me.DT_FechaPrevisionRecepcion = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Proveedor = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.OP_Tipo_Reparacion = New Infragistics.Win.UltraWinEditors.UltraOptionSet()
        CType(Me.T_Detalle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Personal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TE_Producto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Fecha, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TE_Producto_Anterior, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_SubstitucionArticulo.SuspendLayout()
        Me.Panel_ReparacionExterna.ClientArea.SuspendLayout()
        Me.Panel_ReparacionExterna.SuspendLayout()
        CType(Me.DT_FechaPrevisionRecepcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TE_Proveedor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OP_Tipo_Reparacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(987, 24)
        Me.ToolForm.TabIndex = 29
        '
        'T_Detalle
        '
        Me.T_Detalle.Location = New System.Drawing.Point(16, 110)
        Me.T_Detalle.Multiline = True
        Me.T_Detalle.Name = "T_Detalle"
        Me.T_Detalle.Size = New System.Drawing.Size(983, 151)
        Me.T_Detalle.TabIndex = 2
        Me.T_Detalle.Text = "T_Detalle"
        '
        'C_Personal
        '
        Me.C_Personal.Location = New System.Drawing.Point(16, 58)
        Me.C_Personal.Name = "C_Personal"
        Me.C_Personal.Size = New System.Drawing.Size(195, 21)
        Me.C_Personal.TabIndex = 0
        Me.C_Personal.Text = "C_Personal"
        '
        'TE_Producto
        '
        Me.TE_Producto.Location = New System.Drawing.Point(14, 24)
        Me.TE_Producto.Name = "TE_Producto"
        Me.TE_Producto.ReadOnly = True
        Me.TE_Producto.Size = New System.Drawing.Size(777, 21)
        Me.TE_Producto.TabIndex = 1
        Me.TE_Producto.Text = "TE_Producto"
        '
        'UltraLabel13
        '
        Me.UltraLabel13.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel13.Name = "UltraLabel13"
        Me.UltraLabel13.Size = New System.Drawing.Size(96, 16)
        Me.UltraLabel13.TabIndex = 22
        Me.UltraLabel13.Text = "*Personal:"
        '
        'UltraLabel16
        '
        Me.UltraLabel16.Location = New System.Drawing.Point(14, 94)
        Me.UltraLabel16.Name = "UltraLabel16"
        Me.UltraLabel16.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel16.TabIndex = 21
        Me.UltraLabel16.Text = "*Detalle:"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(14, 9)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel4.TabIndex = 16
        Me.UltraLabel4.Text = "*Descripción del producto:"
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(959, 328)
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'UltraLabel22
        '
        Me.UltraLabel22.Location = New System.Drawing.Point(16, 392)
        Me.UltraLabel22.Name = "UltraLabel22"
        Me.UltraLabel22.Size = New System.Drawing.Size(112, 16)
        Me.UltraLabel22.TabIndex = 28
        Me.UltraLabel22.Text = "* Campo obligatorio"
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
        'DT_Fecha
        '
        Me.DT_Fecha.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Fecha.Location = New System.Drawing.Point(227, 58)
        Me.DT_Fecha.Name = "DT_Fecha"
        Me.DT_Fecha.Size = New System.Drawing.Size(83, 21)
        Me.DT_Fecha.TabIndex = 1
        Me.DT_Fecha.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel17
        '
        Me.UltraLabel17.Location = New System.Drawing.Point(224, 43)
        Me.UltraLabel17.Name = "UltraLabel17"
        Me.UltraLabel17.Size = New System.Drawing.Size(86, 16)
        Me.UltraLabel17.TabIndex = 34
        Me.UltraLabel17.Text = "*Fecha:"
        '
        'TE_Producto_Anterior
        '
        Me.TE_Producto_Anterior.Location = New System.Drawing.Point(14, 72)
        Me.TE_Producto_Anterior.Name = "TE_Producto_Anterior"
        Me.TE_Producto_Anterior.ReadOnly = True
        Me.TE_Producto_Anterior.Size = New System.Drawing.Size(777, 21)
        Me.TE_Producto_Anterior.TabIndex = 38
        Me.TE_Producto_Anterior.Text = "TE_Producto_Anterior"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(14, 57)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel3.TabIndex = 40
        Me.UltraLabel3.Text = "Descripción del producto substituido:"
        '
        'Panel_SubstitucionArticulo
        '
        Me.Panel_SubstitucionArticulo.Controls.Add(Me.TE_Producto_Anterior)
        Me.Panel_SubstitucionArticulo.Controls.Add(Me.TE_Producto)
        Me.Panel_SubstitucionArticulo.Controls.Add(Me.UltraLabel4)
        Me.Panel_SubstitucionArticulo.Controls.Add(Me.UltraLabel3)
        Me.Panel_SubstitucionArticulo.Location = New System.Drawing.Point(202, 267)
        Me.Panel_SubstitucionArticulo.Name = "Panel_SubstitucionArticulo"
        Me.Panel_SubstitucionArticulo.Size = New System.Drawing.Size(801, 110)
        Me.Panel_SubstitucionArticulo.TabIndex = 41
        '
        'Panel_ReparacionExterna
        '
        '
        'Panel_ReparacionExterna.ClientArea
        '
        Me.Panel_ReparacionExterna.ClientArea.Controls.Add(Me.DT_FechaPrevisionRecepcion)
        Me.Panel_ReparacionExterna.ClientArea.Controls.Add(Me.UltraLabel2)
        Me.Panel_ReparacionExterna.ClientArea.Controls.Add(Me.TE_Proveedor)
        Me.Panel_ReparacionExterna.ClientArea.Controls.Add(Me.UltraLabel1)
        Me.Panel_ReparacionExterna.Location = New System.Drawing.Point(202, 267)
        Me.Panel_ReparacionExterna.Name = "Panel_ReparacionExterna"
        Me.Panel_ReparacionExterna.Size = New System.Drawing.Size(795, 80)
        Me.Panel_ReparacionExterna.TabIndex = 43
        '
        'DT_FechaPrevisionRecepcion
        '
        Me.DT_FechaPrevisionRecepcion.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_FechaPrevisionRecepcion.Location = New System.Drawing.Point(663, 25)
        Me.DT_FechaPrevisionRecepcion.Name = "DT_FechaPrevisionRecepcion"
        Me.DT_FechaPrevisionRecepcion.Size = New System.Drawing.Size(123, 21)
        Me.DT_FechaPrevisionRecepcion.TabIndex = 1
        Me.DT_FechaPrevisionRecepcion.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(663, 9)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(125, 16)
        Me.UltraLabel2.TabIndex = 36
        Me.UltraLabel2.Text = "Fecha prev. recepción:"
        '
        'TE_Proveedor
        '
        Me.TE_Proveedor.Location = New System.Drawing.Point(9, 25)
        Me.TE_Proveedor.Name = "TE_Proveedor"
        Me.TE_Proveedor.ReadOnly = True
        Me.TE_Proveedor.Size = New System.Drawing.Size(636, 21)
        Me.TE_Proveedor.TabIndex = 0
        Me.TE_Proveedor.Text = "TE_Proveedor"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(9, 10)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel1.TabIndex = 18
        Me.UltraLabel1.Text = "*Proveedor:"
        '
        'OP_Tipo_Reparacion
        '
        Me.OP_Tipo_Reparacion.ItemOrigin = New System.Drawing.Point(2, 2)
        ValueListItem1.CheckState = System.Windows.Forms.CheckState.Checked
        ValueListItem1.DataValue = CType(1, Short)
        ValueListItem1.DisplayText = "Reparación interna"
        ValueListItem2.DataValue = CType(2, Short)
        ValueListItem2.DisplayText = "Reparación externa"
        ValueListItem3.DataValue = CType(3, Short)
        ValueListItem3.DisplayText = "Substitución por otro artículo"
        Me.OP_Tipo_Reparacion.Items.AddRange(New Infragistics.Win.ValueListItem() {ValueListItem1, ValueListItem2, ValueListItem3})
        Me.OP_Tipo_Reparacion.Location = New System.Drawing.Point(16, 277)
        Me.OP_Tipo_Reparacion.Name = "OP_Tipo_Reparacion"
        Me.OP_Tipo_Reparacion.Size = New System.Drawing.Size(217, 83)
        Me.OP_Tipo_Reparacion.TabIndex = 3
        '
        'frmParte_Reparacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 419)
        Me.Controls.Add(Me.Panel_ReparacionExterna)
        Me.Controls.Add(Me.Panel_SubstitucionArticulo)
        Me.Controls.Add(Me.DT_Fecha)
        Me.Controls.Add(Me.UltraLabel17)
        Me.Controls.Add(Me.T_Detalle)
        Me.Controls.Add(Me.UltraLabel22)
        Me.Controls.Add(Me.C_Personal)
        Me.Controls.Add(Me.UltraLabel13)
        Me.Controls.Add(Me.UltraLabel16)
        Me.Controls.Add(Me.OP_Tipo_Reparacion)
        Me.KeyPreview = True
        Me.Name = "frmParte_Reparacion"
        Me.Text = "Reparación de un producto"
        Me.Controls.SetChildIndex(Me.OP_Tipo_Reparacion, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel16, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel13, 0)
        Me.Controls.SetChildIndex(Me.C_Personal, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel22, 0)
        Me.Controls.SetChildIndex(Me.T_Detalle, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel17, 0)
        Me.Controls.SetChildIndex(Me.DT_Fecha, 0)
        Me.Controls.SetChildIndex(Me.Panel_SubstitucionArticulo, 0)
        Me.Controls.SetChildIndex(Me.Panel_ReparacionExterna, 0)
        CType(Me.T_Detalle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Personal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TE_Producto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Fecha, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TE_Producto_Anterior, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_SubstitucionArticulo.ResumeLayout(False)
        Me.Panel_SubstitucionArticulo.PerformLayout()
        Me.Panel_ReparacionExterna.ClientArea.ResumeLayout(False)
        Me.Panel_ReparacionExterna.ClientArea.PerformLayout()
        Me.Panel_ReparacionExterna.ResumeLayout(False)
        CType(Me.DT_FechaPrevisionRecepcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TE_Proveedor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OP_Tipo_Reparacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TE_Producto As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UltraLabel22 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_Personal As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel13 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents T_Detalle As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel16 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents DT_Fecha As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel17 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Panel_ReparacionExterna As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents OP_Tipo_Reparacion As Infragistics.Win.UltraWinEditors.UltraOptionSet
    Friend WithEvents Panel_SubstitucionArticulo As System.Windows.Forms.Panel
    Friend WithEvents TE_Producto_Anterior As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents DT_FechaPrevisionRecepcion As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Proveedor As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
End Class
