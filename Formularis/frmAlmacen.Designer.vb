<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAlmacen
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
        Dim ValueListItem4 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Dim ValueListItem5 As Infragistics.Win.ValueListItem = New Infragistics.Win.ValueListItem()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAlmacen))
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab5 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab6 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.Pan_Parte = New Infragistics.Win.Misc.UltraPanel()
        Me.TE_Parte = New M_TextEditor.m_TextEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.Pan_Proveedor = New Infragistics.Win.Misc.UltraPanel()
        Me.TE_Proveedor = New M_TextEditor.m_TextEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.Pan_Cliente = New Infragistics.Win.Misc.UltraPanel()
        Me.TE_Cliente = New M_TextEditor.m_TextEditor()
        Me.UltraLabel16 = New Infragistics.Win.Misc.UltraLabel()
        Me.Pan_Personal = New Infragistics.Win.Misc.UltraPanel()
        Me.C_Personal = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.L_Personal = New Infragistics.Win.Misc.UltraLabel()
        Me.OP_TipusMagatzem = New Infragistics.Win.UltraWinEditors.UltraOptionSet()
        Me.CH_Predeterminado = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.T_CP = New M_TextEditor.m_TextEditor()
        Me.UltraLabel14 = New Infragistics.Win.Misc.UltraLabel()
        Me.DT_Baja = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel13 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Fax = New M_TextEditor.m_TextEditor()
        Me.UltraLabel12 = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Codigo = New M_MaskEditor.m_MaskEditor()
        Me.DT_Alta = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.T_Telefono = New M_TextEditor.m_TextEditor()
        Me.T_Direccion = New M_TextEditor.m_TextEditor()
        Me.T_Email = New M_TextEditor.m_TextEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.T_Persona_Contacto = New M_TextEditor.m_TextEditor()
        Me.T_Poblacion = New M_TextEditor.m_TextEditor()
        Me.T_Provincia = New M_TextEditor.m_TextEditor()
        Me.UltraLabel28 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel9 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel10 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel15 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Stock = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl9 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.R_Observaciones = New M_RichText.M_RichText()
        Me.UltraTabPageControl4 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Ficheros = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.TAB_Principal = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl1.SuspendLayout()
        Me.Pan_Parte.ClientArea.SuspendLayout()
        Me.Pan_Parte.SuspendLayout()
        CType(Me.TE_Parte, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Pan_Proveedor.ClientArea.SuspendLayout()
        Me.Pan_Proveedor.SuspendLayout()
        CType(Me.TE_Proveedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Pan_Cliente.ClientArea.SuspendLayout()
        Me.Pan_Cliente.SuspendLayout()
        CType(Me.TE_Cliente, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Pan_Personal.ClientArea.SuspendLayout()
        Me.Pan_Personal.SuspendLayout()
        CType(Me.C_Personal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OP_TipusMagatzem, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CH_Predeterminado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_CP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Baja, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Fax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Telefono, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Direccion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Email, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Persona_Contacto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Poblacion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Provincia, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl2.SuspendLayout()
        Me.UltraTabPageControl9.SuspendLayout()
        Me.UltraTabPageControl4.SuspendLayout()
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_Principal.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Margin = New System.Windows.Forms.Padding(4)
        Me.ToolForm.Size = New System.Drawing.Size(985, 24)
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.Pan_Parte)
        Me.UltraTabPageControl1.Controls.Add(Me.Pan_Proveedor)
        Me.UltraTabPageControl1.Controls.Add(Me.Pan_Cliente)
        Me.UltraTabPageControl1.Controls.Add(Me.Pan_Personal)
        Me.UltraTabPageControl1.Controls.Add(Me.OP_TipusMagatzem)
        Me.UltraTabPageControl1.Controls.Add(Me.CH_Predeterminado)
        Me.UltraTabPageControl1.Controls.Add(Me.T_CP)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel14)
        Me.UltraTabPageControl1.Controls.Add(Me.DT_Baja)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel13)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Fax)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel12)
        Me.UltraTabPageControl1.Controls.Add(Me.TE_Codigo)
        Me.UltraTabPageControl1.Controls.Add(Me.DT_Alta)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Telefono)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Direccion)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Email)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Descripcion)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Persona_Contacto)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Poblacion)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Provincia)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel28)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel1)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel5)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel4)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel9)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel10)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel6)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel15)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel7)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(983, 560)
        '
        'Pan_Parte
        '
        '
        'Pan_Parte.ClientArea
        '
        Me.Pan_Parte.ClientArea.Controls.Add(Me.TE_Parte)
        Me.Pan_Parte.ClientArea.Controls.Add(Me.UltraLabel3)
        Me.Pan_Parte.Location = New System.Drawing.Point(163, 231)
        Me.Pan_Parte.Name = "Pan_Parte"
        Me.Pan_Parte.Size = New System.Drawing.Size(542, 61)
        Me.Pan_Parte.TabIndex = 216
        Me.Pan_Parte.Visible = False
        '
        'TE_Parte
        '
        Me.TE_Parte.Location = New System.Drawing.Point(14, 26)
        Me.TE_Parte.Name = "TE_Parte"
        Me.TE_Parte.ReadOnly = True
        Me.TE_Parte.Size = New System.Drawing.Size(496, 21)
        Me.TE_Parte.TabIndex = 0
        Me.TE_Parte.Text = "TE_Parte"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(14, 11)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(152, 16)
        Me.UltraLabel3.TabIndex = 210
        Me.UltraLabel3.Text = "*Parte relacionado:"
        '
        'Pan_Proveedor
        '
        '
        'Pan_Proveedor.ClientArea
        '
        Me.Pan_Proveedor.ClientArea.Controls.Add(Me.TE_Proveedor)
        Me.Pan_Proveedor.ClientArea.Controls.Add(Me.UltraLabel2)
        Me.Pan_Proveedor.Location = New System.Drawing.Point(163, 228)
        Me.Pan_Proveedor.Name = "Pan_Proveedor"
        Me.Pan_Proveedor.Size = New System.Drawing.Size(542, 66)
        Me.Pan_Proveedor.TabIndex = 215
        Me.Pan_Proveedor.Visible = False
        '
        'TE_Proveedor
        '
        Me.TE_Proveedor.Location = New System.Drawing.Point(14, 26)
        Me.TE_Proveedor.Name = "TE_Proveedor"
        Me.TE_Proveedor.ReadOnly = True
        Me.TE_Proveedor.Size = New System.Drawing.Size(496, 21)
        Me.TE_Proveedor.TabIndex = 153
        Me.TE_Proveedor.Text = "TE_Proveedor"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(13, 10)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel2.TabIndex = 154
        Me.UltraLabel2.Text = "*Proveedor:"
        '
        'Pan_Cliente
        '
        '
        'Pan_Cliente.ClientArea
        '
        Me.Pan_Cliente.ClientArea.Controls.Add(Me.TE_Cliente)
        Me.Pan_Cliente.ClientArea.Controls.Add(Me.UltraLabel16)
        Me.Pan_Cliente.Location = New System.Drawing.Point(163, 225)
        Me.Pan_Cliente.Name = "Pan_Cliente"
        Me.Pan_Cliente.Size = New System.Drawing.Size(542, 60)
        Me.Pan_Cliente.TabIndex = 214
        Me.Pan_Cliente.Visible = False
        '
        'TE_Cliente
        '
        Me.TE_Cliente.Location = New System.Drawing.Point(14, 25)
        Me.TE_Cliente.Name = "TE_Cliente"
        Me.TE_Cliente.ReadOnly = True
        Me.TE_Cliente.Size = New System.Drawing.Size(496, 21)
        Me.TE_Cliente.TabIndex = 151
        Me.TE_Cliente.Text = "TE_Cliente"
        '
        'UltraLabel16
        '
        Me.UltraLabel16.Location = New System.Drawing.Point(13, 9)
        Me.UltraLabel16.Name = "UltraLabel16"
        Me.UltraLabel16.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel16.TabIndex = 152
        Me.UltraLabel16.Text = "*Cliente:"
        '
        'Pan_Personal
        '
        '
        'Pan_Personal.ClientArea
        '
        Me.Pan_Personal.ClientArea.Controls.Add(Me.C_Personal)
        Me.Pan_Personal.ClientArea.Controls.Add(Me.L_Personal)
        Me.Pan_Personal.Location = New System.Drawing.Point(172, 221)
        Me.Pan_Personal.Name = "Pan_Personal"
        Me.Pan_Personal.Size = New System.Drawing.Size(302, 61)
        Me.Pan_Personal.TabIndex = 213
        Me.Pan_Personal.Visible = False
        '
        'C_Personal
        '
        Me.C_Personal.Location = New System.Drawing.Point(14, 28)
        Me.C_Personal.Name = "C_Personal"
        Me.C_Personal.Size = New System.Drawing.Size(273, 21)
        Me.C_Personal.TabIndex = 209
        Me.C_Personal.Text = "C_Personal"
        '
        'L_Personal
        '
        Me.L_Personal.Location = New System.Drawing.Point(14, 12)
        Me.L_Personal.Name = "L_Personal"
        Me.L_Personal.Size = New System.Drawing.Size(152, 16)
        Me.L_Personal.TabIndex = 210
        Me.L_Personal.Text = "*Personal relacionado:"
        '
        'OP_TipusMagatzem
        '
        ValueListItem1.CheckState = System.Windows.Forms.CheckState.Checked
        ValueListItem1.DataValue = CType(1, Byte)
        ValueListItem1.DisplayText = "Almacén interno"
        ValueListItem2.DataValue = CType(2, Byte)
        ValueListItem2.DisplayText = "Personal interno"
        ValueListItem3.DataValue = CType(3, Byte)
        ValueListItem3.DisplayText = "Cliente"
        ValueListItem4.DataValue = CType(4, Byte)
        ValueListItem4.DisplayText = "Proveedor"
        ValueListItem5.DataValue = CType(5, Byte)
        ValueListItem5.DisplayText = "Parte"
        Me.OP_TipusMagatzem.Items.AddRange(New Infragistics.Win.ValueListItem() {ValueListItem1, ValueListItem2, ValueListItem3, ValueListItem4, ValueListItem5})
        Me.OP_TipusMagatzem.ItemSpacingVertical = 2
        Me.OP_TipusMagatzem.Location = New System.Drawing.Point(13, 221)
        Me.OP_TipusMagatzem.Name = "OP_TipusMagatzem"
        Me.OP_TipusMagatzem.Size = New System.Drawing.Size(140, 99)
        Me.OP_TipusMagatzem.TabIndex = 13
        '
        'CH_Predeterminado
        '
        Me.CH_Predeterminado.Location = New System.Drawing.Point(270, 174)
        Me.CH_Predeterminado.Name = "CH_Predeterminado"
        Me.CH_Predeterminado.Size = New System.Drawing.Size(244, 28)
        Me.CH_Predeterminado.TabIndex = 12
        Me.CH_Predeterminado.Text = "Almacén predeterminado"
        '
        'T_CP
        '
        Me.T_CP.Location = New System.Drawing.Point(798, 127)
        Me.T_CP.Name = "T_CP"
        Me.T_CP.Size = New System.Drawing.Size(165, 21)
        Me.T_CP.TabIndex = 9
        Me.T_CP.Text = "T_CP"
        '
        'UltraLabel14
        '
        Me.UltraLabel14.Location = New System.Drawing.Point(799, 112)
        Me.UltraLabel14.Name = "UltraLabel14"
        Me.UltraLabel14.Size = New System.Drawing.Size(151, 16)
        Me.UltraLabel14.TabIndex = 153
        Me.UltraLabel14.Text = "Código postal:"
        '
        'DT_Baja
        '
        Me.DT_Baja.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Baja.Location = New System.Drawing.Point(122, 177)
        Me.DT_Baja.Name = "DT_Baja"
        Me.DT_Baja.ReadOnly = True
        Me.DT_Baja.Size = New System.Drawing.Size(90, 21)
        Me.DT_Baja.TabIndex = 11
        Me.DT_Baja.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel13
        '
        Me.UltraLabel13.Location = New System.Drawing.Point(122, 163)
        Me.UltraLabel13.Name = "UltraLabel13"
        Me.UltraLabel13.Size = New System.Drawing.Size(90, 16)
        Me.UltraLabel13.TabIndex = 149
        Me.UltraLabel13.Text = "Fecha de baja:"
        '
        'T_Fax
        '
        Me.T_Fax.Location = New System.Drawing.Point(798, 78)
        Me.T_Fax.Name = "T_Fax"
        Me.T_Fax.Size = New System.Drawing.Size(165, 21)
        Me.T_Fax.TabIndex = 5
        Me.T_Fax.Text = "T_Fax"
        '
        'UltraLabel12
        '
        Me.UltraLabel12.Location = New System.Drawing.Point(799, 63)
        Me.UltraLabel12.Name = "UltraLabel12"
        Me.UltraLabel12.Size = New System.Drawing.Size(151, 16)
        Me.UltraLabel12.TabIndex = 147
        Me.UltraLabel12.Text = "Fax:"
        '
        'TE_Codigo
        '
        Me.TE_Codigo.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.TE_Codigo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.TE_Codigo.InputMask = "nnnnnnnnnn"
        Me.TE_Codigo.Location = New System.Drawing.Point(13, 31)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.NonAutoSizeHeight = 20
        Me.TE_Codigo.Size = New System.Drawing.Size(120, 20)
        Me.TE_Codigo.TabIndex = 0
        '
        'DT_Alta
        '
        Me.DT_Alta.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Alta.Location = New System.Drawing.Point(13, 177)
        Me.DT_Alta.Name = "DT_Alta"
        Me.DT_Alta.ReadOnly = True
        Me.DT_Alta.Size = New System.Drawing.Size(90, 21)
        Me.DT_Alta.TabIndex = 10
        Me.DT_Alta.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'T_Telefono
        '
        Me.T_Telefono.Location = New System.Drawing.Point(635, 78)
        Me.T_Telefono.Name = "T_Telefono"
        Me.T_Telefono.Size = New System.Drawing.Size(152, 21)
        Me.T_Telefono.TabIndex = 4
        Me.T_Telefono.Text = "T_Telefono"
        '
        'T_Direccion
        '
        Me.T_Direccion.Location = New System.Drawing.Point(13, 127)
        Me.T_Direccion.Name = "T_Direccion"
        Me.T_Direccion.Size = New System.Drawing.Size(240, 21)
        Me.T_Direccion.TabIndex = 6
        Me.T_Direccion.Text = "T_Direccion"
        '
        'T_Email
        '
        Me.T_Email.Location = New System.Drawing.Point(388, 78)
        Me.T_Email.Name = "T_Email"
        Me.T_Email.Size = New System.Drawing.Size(230, 21)
        Me.T_Email.TabIndex = 3
        Me.T_Email.Text = "T_Email"
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(149, 30)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(814, 21)
        Me.T_Descripcion.TabIndex = 1
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'T_Persona_Contacto
        '
        Me.T_Persona_Contacto.Location = New System.Drawing.Point(13, 78)
        Me.T_Persona_Contacto.Name = "T_Persona_Contacto"
        Me.T_Persona_Contacto.Size = New System.Drawing.Size(362, 21)
        Me.T_Persona_Contacto.TabIndex = 2
        Me.T_Persona_Contacto.Text = "T_Persona_Contacto"
        '
        'T_Poblacion
        '
        Me.T_Poblacion.Location = New System.Drawing.Point(270, 127)
        Me.T_Poblacion.Name = "T_Poblacion"
        Me.T_Poblacion.Size = New System.Drawing.Size(244, 21)
        Me.T_Poblacion.TabIndex = 7
        Me.T_Poblacion.Text = "T_Poblacion"
        '
        'T_Provincia
        '
        Me.T_Provincia.Location = New System.Drawing.Point(532, 127)
        Me.T_Provincia.Name = "T_Provincia"
        Me.T_Provincia.Size = New System.Drawing.Size(255, 21)
        Me.T_Provincia.TabIndex = 8
        Me.T_Provincia.Text = "T_Provincia"
        '
        'UltraLabel28
        '
        Me.UltraLabel28.Location = New System.Drawing.Point(13, 163)
        Me.UltraLabel28.Name = "UltraLabel28"
        Me.UltraLabel28.Size = New System.Drawing.Size(90, 16)
        Me.UltraLabel28.TabIndex = 140
        Me.UltraLabel28.Text = "*Fecha de alta:"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(13, 15)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel1.TabIndex = 63
        Me.UltraLabel1.Text = "*Código:"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(148, 14)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 126
        Me.UltraLabel5.Text = "*Descripción"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(13, 112)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(240, 16)
        Me.UltraLabel4.TabIndex = 76
        Me.UltraLabel4.Text = "Dirección:"
        '
        'UltraLabel9
        '
        Me.UltraLabel9.Location = New System.Drawing.Point(636, 63)
        Me.UltraLabel9.Name = "UltraLabel9"
        Me.UltraLabel9.Size = New System.Drawing.Size(151, 16)
        Me.UltraLabel9.TabIndex = 136
        Me.UltraLabel9.Text = "Teléfono:"
        '
        'UltraLabel10
        '
        Me.UltraLabel10.Location = New System.Drawing.Point(389, 63)
        Me.UltraLabel10.Name = "UltraLabel10"
        Me.UltraLabel10.Size = New System.Drawing.Size(229, 16)
        Me.UltraLabel10.TabIndex = 134
        Me.UltraLabel10.Text = "Email:"
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(271, 112)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(244, 16)
        Me.UltraLabel6.TabIndex = 128
        Me.UltraLabel6.Text = "Población:"
        '
        'UltraLabel15
        '
        Me.UltraLabel15.Location = New System.Drawing.Point(13, 63)
        Me.UltraLabel15.Name = "UltraLabel15"
        Me.UltraLabel15.Size = New System.Drawing.Size(222, 16)
        Me.UltraLabel15.TabIndex = 132
        Me.UltraLabel15.Text = "Persona de contacto:"
        '
        'UltraLabel7
        '
        Me.UltraLabel7.Location = New System.Drawing.Point(533, 112)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.Size = New System.Drawing.Size(187, 16)
        Me.UltraLabel7.TabIndex = 130
        Me.UltraLabel7.Text = "Provincia:"
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.GRD_Stock)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(983, 560)
        '
        'GRD_Stock
        '
        Me.GRD_Stock.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Stock.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Stock.Name = "GRD_Stock"
        Me.GRD_Stock.pAccessibleName = Nothing
        Me.GRD_Stock.pActivarBotonFiltro = False
        Me.GRD_Stock.pText = " "
        Me.GRD_Stock.Size = New System.Drawing.Size(983, 560)
        Me.GRD_Stock.TabIndex = 0
        '
        'UltraTabPageControl9
        '
        Me.UltraTabPageControl9.Controls.Add(Me.R_Observaciones)
        Me.UltraTabPageControl9.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl9.Name = "UltraTabPageControl9"
        Me.UltraTabPageControl9.Size = New System.Drawing.Size(983, 560)
        '
        'R_Observaciones
        '
        Me.R_Observaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.R_Observaciones.Location = New System.Drawing.Point(0, 0)
        Me.R_Observaciones.Margin = New System.Windows.Forms.Padding(4)
        Me.R_Observaciones.Name = "R_Observaciones"
        Me.R_Observaciones.pEnable = True
        Me.R_Observaciones.pText = resources.GetString("R_Observaciones.pText")
        Me.R_Observaciones.pTextEspecial = ""
        Me.R_Observaciones.Size = New System.Drawing.Size(983, 560)
        Me.R_Observaciones.TabIndex = 2
        '
        'UltraTabPageControl4
        '
        Me.UltraTabPageControl4.Controls.Add(Me.GRD_Ficheros)
        Me.UltraTabPageControl4.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl4.Name = "UltraTabPageControl4"
        Me.UltraTabPageControl4.Size = New System.Drawing.Size(983, 560)
        '
        'GRD_Ficheros
        '
        Me.GRD_Ficheros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Ficheros.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Ficheros.Name = "GRD_Ficheros"
        Me.GRD_Ficheros.pAccessibleName = Nothing
        Me.GRD_Ficheros.pActivarBotonFiltro = False
        Me.GRD_Ficheros.pText = " "
        Me.GRD_Ficheros.Size = New System.Drawing.Size(983, 560)
        Me.GRD_Ficheros.TabIndex = 134
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(959, 328)
        '
        'TAB_Principal
        '
        Me.TAB_Principal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TAB_Principal.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl9)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl1)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl4)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl2)
        Me.TAB_Principal.Location = New System.Drawing.Point(12, 43)
        Me.TAB_Principal.Name = "TAB_Principal"
        Me.TAB_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.TAB_Principal.Size = New System.Drawing.Size(987, 586)
        Me.TAB_Principal.TabIndex = 0
        UltraTab1.Key = "General"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "General"
        UltraTab2.Key = "Stocks"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Stocks"
        UltraTab5.Key = "Observaciones"
        UltraTab5.TabPage = Me.UltraTabPageControl9
        UltraTab5.Text = "Observaciones"
        UltraTab6.Key = "Ficheros"
        UltraTab6.TabPage = Me.UltraTabPageControl4
        UltraTab6.Text = "Ficheros"
        Me.TAB_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2, UltraTab5, UltraTab6})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(983, 560)
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
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
        'frmAlmacen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 641)
        Me.Controls.Add(Me.TAB_Principal)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmAlmacen"
        Me.Text = "Almacén"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.TAB_Principal, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl1.PerformLayout()
        Me.Pan_Parte.ClientArea.ResumeLayout(False)
        Me.Pan_Parte.ClientArea.PerformLayout()
        Me.Pan_Parte.ResumeLayout(False)
        CType(Me.TE_Parte, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Pan_Proveedor.ClientArea.ResumeLayout(False)
        Me.Pan_Proveedor.ClientArea.PerformLayout()
        Me.Pan_Proveedor.ResumeLayout(False)
        CType(Me.TE_Proveedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Pan_Cliente.ClientArea.ResumeLayout(False)
        Me.Pan_Cliente.ClientArea.PerformLayout()
        Me.Pan_Cliente.ResumeLayout(False)
        CType(Me.TE_Cliente, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Pan_Personal.ClientArea.ResumeLayout(False)
        Me.Pan_Personal.ClientArea.PerformLayout()
        Me.Pan_Personal.ResumeLayout(False)
        CType(Me.C_Personal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OP_TipusMagatzem, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CH_Predeterminado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_CP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Baja, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Fax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Telefono, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Direccion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Email, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Persona_Contacto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Poblacion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Provincia, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl2.ResumeLayout(False)
        Me.UltraTabPageControl9.ResumeLayout(False)
        Me.UltraTabPageControl4.ResumeLayout(False)
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_Principal.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TAB_Principal As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UltraTabPageControl9 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl4 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents DT_Baja As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel13 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Fax As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel12 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Codigo As M_MaskEditor.m_MaskEditor
    Friend WithEvents DT_Alta As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents T_Telefono As M_TextEditor.m_TextEditor
    Friend WithEvents T_Direccion As M_TextEditor.m_TextEditor
    Friend WithEvents T_Email As M_TextEditor.m_TextEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents T_Persona_Contacto As M_TextEditor.m_TextEditor
    Friend WithEvents T_Poblacion As M_TextEditor.m_TextEditor
    Friend WithEvents T_Provincia As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel28 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel9 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel10 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel15 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_CP As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel14 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents R_Observaciones As M_RichText.M_RichText
    Friend WithEvents GRD_Ficheros As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_Personal As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents L_Personal As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Stock As M_UltraGrid.m_UltraGrid
    Friend WithEvents CH_Predeterminado As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents Pan_Cliente As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents Pan_Personal As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents OP_TipusMagatzem As Infragistics.Win.UltraWinEditors.UltraOptionSet
    Friend WithEvents Pan_Proveedor As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents TE_Proveedor As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Cliente As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel16 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Pan_Parte As Infragistics.Win.Misc.UltraPanel
    Friend WithEvents TE_Parte As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
End Class
