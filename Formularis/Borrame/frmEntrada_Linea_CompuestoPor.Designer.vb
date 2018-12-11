<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntrada_Linea_CompuestoPor
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
        Dim EditorButton1 As Infragistics.Win.UltraWinEditors.EditorButton = New Infragistics.Win.UltraWinEditors.EditorButton("Todos")
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.GRD_NS = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.C_Almacen = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel29 = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Producto_Codigo = New M_TextEditor.m_TextEditor()
        Me.T_Producto_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.B_CarregarNS = New Infragistics.Win.Misc.UltraButton()
        Me.L_klkl = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Unidades = New M_MaskEditor.m_MaskEditor()
        Me.GRD_NS_Seleccionats = New M_UltraGrid.m_UltraGrid()
        Me.B_Asignar = New Infragistics.Win.Misc.UltraButton()
        Me.B_Desasignar = New Infragistics.Win.Misc.UltraButton()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.L_Producto_ConNS = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.C_Almacen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TE_Producto_Codigo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Producto_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(986, 24)
        Me.ToolForm.TabIndex = 29
        '
        'GRD_NS
        '
        Me.GRD_NS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GRD_NS.Location = New System.Drawing.Point(12, 94)
        Me.GRD_NS.Name = "GRD_NS"
        Me.GRD_NS.pAccessibleName = Nothing
        Me.GRD_NS.pActivarBotonFiltro = False
        Me.GRD_NS.pText = " "
        Me.GRD_NS.Size = New System.Drawing.Size(482, 417)
        Me.GRD_NS.TabIndex = 136
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
        'C_Almacen
        '
        EditorButton1.Key = "Todos"
        EditorButton1.Text = "Todos"
        Me.C_Almacen.ButtonsRight.Add(EditorButton1)
        Me.C_Almacen.Location = New System.Drawing.Point(600, 59)
        Me.C_Almacen.Name = "C_Almacen"
        Me.C_Almacen.Size = New System.Drawing.Size(209, 21)
        Me.C_Almacen.TabIndex = 2
        Me.C_Almacen.Text = "C_Almacen"
        '
        'UltraLabel29
        '
        Me.UltraLabel29.Location = New System.Drawing.Point(600, 43)
        Me.UltraLabel29.Name = "UltraLabel29"
        Me.UltraLabel29.Size = New System.Drawing.Size(96, 16)
        Me.UltraLabel29.TabIndex = 141
        Me.UltraLabel29.Text = "Almacén:"
        '
        'TE_Producto_Codigo
        '
        Me.TE_Producto_Codigo.Location = New System.Drawing.Point(12, 59)
        Me.TE_Producto_Codigo.Name = "TE_Producto_Codigo"
        Me.TE_Producto_Codigo.Size = New System.Drawing.Size(173, 21)
        Me.TE_Producto_Codigo.TabIndex = 0
        Me.TE_Producto_Codigo.Text = "TE_Producto_Codigo"
        '
        'T_Producto_Descripcion
        '
        Me.T_Producto_Descripcion.Location = New System.Drawing.Point(197, 59)
        Me.T_Producto_Descripcion.Name = "T_Producto_Descripcion"
        Me.T_Producto_Descripcion.ReadOnly = True
        Me.T_Producto_Descripcion.Size = New System.Drawing.Size(391, 21)
        Me.T_Producto_Descripcion.TabIndex = 1
        Me.T_Producto_Descripcion.Text = "T_Producto_Descripcion"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(116, 16)
        Me.UltraLabel1.TabIndex = 139
        Me.UltraLabel1.Text = "*Código Producto:"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(197, 43)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel4.TabIndex = 140
        Me.UltraLabel4.Text = "Descripción del producto"
        '
        'B_CarregarNS
        '
        Me.B_CarregarNS.Location = New System.Drawing.Point(898, 57)
        Me.B_CarregarNS.Name = "B_CarregarNS"
        Me.B_CarregarNS.Size = New System.Drawing.Size(79, 21)
        Me.B_CarregarNS.TabIndex = 4
        Me.B_CarregarNS.Text = "Buscar"
        '
        'L_klkl
        '
        Me.L_klkl.Location = New System.Drawing.Point(820, 43)
        Me.L_klkl.Name = "L_klkl"
        Me.L_klkl.Size = New System.Drawing.Size(66, 16)
        Me.L_klkl.TabIndex = 145
        Me.L_klkl.Text = "*Unidades:"
        '
        'T_Unidades
        '
        Me.T_Unidades.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_Unidades.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Integer]
        Me.T_Unidades.InputMask = "nnnn"
        Me.T_Unidades.Location = New System.Drawing.Point(820, 59)
        Me.T_Unidades.Name = "T_Unidades"
        Me.T_Unidades.Size = New System.Drawing.Size(66, 20)
        Me.T_Unidades.TabIndex = 3
        '
        'GRD_NS_Seleccionats
        '
        Me.GRD_NS_Seleccionats.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_NS_Seleccionats.Location = New System.Drawing.Point(536, 94)
        Me.GRD_NS_Seleccionats.Name = "GRD_NS_Seleccionats"
        Me.GRD_NS_Seleccionats.pAccessibleName = Nothing
        Me.GRD_NS_Seleccionats.pActivarBotonFiltro = False
        Me.GRD_NS_Seleccionats.pText = " "
        Me.GRD_NS_Seleccionats.Size = New System.Drawing.Size(462, 417)
        Me.GRD_NS_Seleccionats.TabIndex = 146
        '
        'B_Asignar
        '
        Me.B_Asignar.Location = New System.Drawing.Point(502, 242)
        Me.B_Asignar.Name = "B_Asignar"
        Me.B_Asignar.Size = New System.Drawing.Size(28, 28)
        Me.B_Asignar.TabIndex = 5
        Me.B_Asignar.Text = ">"
        '
        'B_Desasignar
        '
        Me.B_Desasignar.Location = New System.Drawing.Point(502, 276)
        Me.B_Desasignar.Name = "B_Desasignar"
        Me.B_Desasignar.Size = New System.Drawing.Size(28, 26)
        Me.B_Desasignar.TabIndex = 6
        Me.B_Desasignar.Text = "<"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'L_Producto_ConNS
        '
        Appearance1.ForeColor = System.Drawing.Color.Red
        Me.L_Producto_ConNS.Appearance = Appearance1
        Me.L_Producto_ConNS.Location = New System.Drawing.Point(985, 59)
        Me.L_Producto_ConNS.Name = "L_Producto_ConNS"
        Me.L_Producto_ConNS.Size = New System.Drawing.Size(211, 16)
        Me.L_Producto_ConNS.TabIndex = 147
        Me.L_Producto_ConNS.Text = "Producto con nº de serie"
        Me.L_Producto_ConNS.Visible = False
        '
        'frmEntrada_Linea_CompuestoPor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1013, 523)
        Me.Controls.Add(Me.L_Producto_ConNS)
        Me.Controls.Add(Me.B_Desasignar)
        Me.Controls.Add(Me.B_Asignar)
        Me.Controls.Add(Me.GRD_NS_Seleccionats)
        Me.Controls.Add(Me.T_Unidades)
        Me.Controls.Add(Me.B_CarregarNS)
        Me.Controls.Add(Me.C_Almacen)
        Me.Controls.Add(Me.UltraLabel29)
        Me.Controls.Add(Me.TE_Producto_Codigo)
        Me.Controls.Add(Me.T_Producto_Descripcion)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel4)
        Me.Controls.Add(Me.GRD_NS)
        Me.Controls.Add(Me.L_klkl)
        Me.KeyPreview = True
        Me.Name = "frmEntrada_Linea_CompuestoPor"
        Me.Text = "Compuesto por..."
        Me.Controls.SetChildIndex(Me.L_klkl, 0)
        Me.Controls.SetChildIndex(Me.GRD_NS, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel4, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Producto_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.TE_Producto_Codigo, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel29, 0)
        Me.Controls.SetChildIndex(Me.C_Almacen, 0)
        Me.Controls.SetChildIndex(Me.B_CarregarNS, 0)
        Me.Controls.SetChildIndex(Me.T_Unidades, 0)
        Me.Controls.SetChildIndex(Me.GRD_NS_Seleccionats, 0)
        Me.Controls.SetChildIndex(Me.B_Asignar, 0)
        Me.Controls.SetChildIndex(Me.B_Desasignar, 0)
        Me.Controls.SetChildIndex(Me.L_Producto_ConNS, 0)
        CType(Me.C_Almacen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TE_Producto_Codigo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Producto_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents GRD_NS As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_Almacen As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel29 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Producto_Codigo As M_TextEditor.m_TextEditor
    Friend WithEvents T_Producto_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents B_CarregarNS As Infragistics.Win.Misc.UltraButton
    Friend WithEvents L_klkl As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Unidades As M_MaskEditor.m_MaskEditor
    Friend WithEvents GRD_NS_Seleccionats As M_UltraGrid.m_UltraGrid
    Friend WithEvents B_Asignar As Infragistics.Win.Misc.UltraButton
    Friend WithEvents B_Desasignar As Infragistics.Win.Misc.UltraButton
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents L_Producto_ConNS As Infragistics.Win.Misc.UltraLabel
End Class
