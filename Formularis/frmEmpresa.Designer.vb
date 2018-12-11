<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEmpresa
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEmpresa))
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab4 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab3 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab5 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab6 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.T_NumeracionFacturaCompraRectificativa = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel22 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NumeracionFacturaVentaRectificativa = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel21 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NumeracionFacturaCompra = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel20 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NumeracionFacturaVenta = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel19 = New Infragistics.Win.Misc.UltraLabel()
        Me.CH_Predeterminada = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
        Me.T_Contraseña = New M_TextEditor.m_TextEditor()
        Me.UltraLabel16 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_URLAcceso = New M_TextEditor.m_TextEditor()
        Me.T_Usuario = New M_TextEditor.m_TextEditor()
        Me.UltraLabel17 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel18 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_CP = New M_TextEditor.m_TextEditor()
        Me.UltraLabel14 = New Infragistics.Win.Misc.UltraLabel()
        Me.DT_Baja = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel13 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Fax = New M_TextEditor.m_TextEditor()
        Me.UltraLabel12 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NIF = New M_TextEditor.m_TextEditor()
        Me.UltraLabel11 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NombreComercial = New M_TextEditor.m_TextEditor()
        Me.UltraLabel8 = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Codigo = New M_MaskEditor.m_MaskEditor()
        Me.DT_Alta = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.T_Telefono = New M_TextEditor.m_TextEditor()
        Me.T_Direccion = New M_TextEditor.m_TextEditor()
        Me.T_Email = New M_TextEditor.m_TextEditor()
        Me.T_Nombre = New M_TextEditor.m_TextEditor()
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
        Me.GRD_Delegaciones = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl5 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_CuentasBancarias = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl6 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.C_Año = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.GRD_Festivos = New M_UltraGrid.m_UltraGrid()
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
        Me.UltraColorPicker1 = New Infragistics.Win.UltraWinEditors.UltraColorPicker()
        Me.UltraLabel23 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.CH_Predeterminada, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Contraseña, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_URLAcceso, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Usuario, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_CP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Baja, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Fax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_NIF, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_NombreComercial, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Telefono, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Direccion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Email, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Nombre, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Persona_Contacto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Poblacion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Provincia, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl2.SuspendLayout()
        Me.UltraTabPageControl5.SuspendLayout()
        Me.UltraTabPageControl6.SuspendLayout()
        CType(Me.C_Año, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl9.SuspendLayout()
        Me.UltraTabPageControl4.SuspendLayout()
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_Principal.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraColorPicker1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Size = New System.Drawing.Size(984, 24)
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.UltraColorPicker1)
        Me.UltraTabPageControl1.Controls.Add(Me.T_NumeracionFacturaCompraRectificativa)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel22)
        Me.UltraTabPageControl1.Controls.Add(Me.T_NumeracionFacturaVentaRectificativa)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel21)
        Me.UltraTabPageControl1.Controls.Add(Me.T_NumeracionFacturaCompra)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel20)
        Me.UltraTabPageControl1.Controls.Add(Me.T_NumeracionFacturaVenta)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel19)
        Me.UltraTabPageControl1.Controls.Add(Me.CH_Predeterminada)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel3)
        Me.UltraTabPageControl1.Controls.Add(Me.PictureEdit1)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Contraseña)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel16)
        Me.UltraTabPageControl1.Controls.Add(Me.T_URLAcceso)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Usuario)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel17)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel18)
        Me.UltraTabPageControl1.Controls.Add(Me.T_CP)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel14)
        Me.UltraTabPageControl1.Controls.Add(Me.DT_Baja)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel13)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Fax)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel12)
        Me.UltraTabPageControl1.Controls.Add(Me.T_NIF)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel11)
        Me.UltraTabPageControl1.Controls.Add(Me.T_NombreComercial)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel8)
        Me.UltraTabPageControl1.Controls.Add(Me.TE_Codigo)
        Me.UltraTabPageControl1.Controls.Add(Me.DT_Alta)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Telefono)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Direccion)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Email)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Nombre)
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
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel23)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(980, 560)
        '
        'T_NumeracionFacturaCompraRectificativa
        '
        Me.T_NumeracionFacturaCompraRectificativa.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_NumeracionFacturaCompraRectificativa.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.T_NumeracionFacturaCompraRectificativa.InputMask = "nnnnnnnnnn"
        Me.T_NumeracionFacturaCompraRectificativa.Location = New System.Drawing.Point(504, 392)
        Me.T_NumeracionFacturaCompraRectificativa.Name = "T_NumeracionFacturaCompraRectificativa"
        Me.T_NumeracionFacturaCompraRectificativa.NonAutoSizeHeight = 20
        Me.T_NumeracionFacturaCompraRectificativa.Size = New System.Drawing.Size(97, 20)
        Me.T_NumeracionFacturaCompraRectificativa.TabIndex = 169
        '
        'UltraLabel22
        '
        Me.UltraLabel22.Location = New System.Drawing.Point(276, 395)
        Me.UltraLabel22.Name = "UltraLabel22"
        Me.UltraLabel22.Size = New System.Drawing.Size(222, 16)
        Me.UltraLabel22.TabIndex = 170
        Me.UltraLabel22.Text = "*Numeración factura rectificativa compra:"
        '
        'T_NumeracionFacturaVentaRectificativa
        '
        Me.T_NumeracionFacturaVentaRectificativa.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_NumeracionFacturaVentaRectificativa.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.T_NumeracionFacturaVentaRectificativa.InputMask = "nnnnnnnnnn"
        Me.T_NumeracionFacturaVentaRectificativa.Location = New System.Drawing.Point(504, 358)
        Me.T_NumeracionFacturaVentaRectificativa.Name = "T_NumeracionFacturaVentaRectificativa"
        Me.T_NumeracionFacturaVentaRectificativa.NonAutoSizeHeight = 20
        Me.T_NumeracionFacturaVentaRectificativa.Size = New System.Drawing.Size(97, 20)
        Me.T_NumeracionFacturaVentaRectificativa.TabIndex = 167
        '
        'UltraLabel21
        '
        Me.UltraLabel21.Location = New System.Drawing.Point(276, 361)
        Me.UltraLabel21.Name = "UltraLabel21"
        Me.UltraLabel21.Size = New System.Drawing.Size(222, 16)
        Me.UltraLabel21.TabIndex = 168
        Me.UltraLabel21.Text = "*Numeración factura rectificativa venta:"
        '
        'T_NumeracionFacturaCompra
        '
        Me.T_NumeracionFacturaCompra.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_NumeracionFacturaCompra.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.T_NumeracionFacturaCompra.InputMask = "nnnnnnnnnn"
        Me.T_NumeracionFacturaCompra.Location = New System.Drawing.Point(504, 325)
        Me.T_NumeracionFacturaCompra.Name = "T_NumeracionFacturaCompra"
        Me.T_NumeracionFacturaCompra.NonAutoSizeHeight = 20
        Me.T_NumeracionFacturaCompra.Size = New System.Drawing.Size(97, 20)
        Me.T_NumeracionFacturaCompra.TabIndex = 165
        '
        'UltraLabel20
        '
        Me.UltraLabel20.Location = New System.Drawing.Point(276, 328)
        Me.UltraLabel20.Name = "UltraLabel20"
        Me.UltraLabel20.Size = New System.Drawing.Size(222, 16)
        Me.UltraLabel20.TabIndex = 166
        Me.UltraLabel20.Text = "*Numeración factura compra:"
        '
        'T_NumeracionFacturaVenta
        '
        Me.T_NumeracionFacturaVenta.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_NumeracionFacturaVenta.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.T_NumeracionFacturaVenta.InputMask = "nnnnnnnnnn"
        Me.T_NumeracionFacturaVenta.Location = New System.Drawing.Point(504, 291)
        Me.T_NumeracionFacturaVenta.Name = "T_NumeracionFacturaVenta"
        Me.T_NumeracionFacturaVenta.NonAutoSizeHeight = 20
        Me.T_NumeracionFacturaVenta.Size = New System.Drawing.Size(97, 20)
        Me.T_NumeracionFacturaVenta.TabIndex = 163
        '
        'UltraLabel19
        '
        Me.UltraLabel19.Location = New System.Drawing.Point(276, 294)
        Me.UltraLabel19.Name = "UltraLabel19"
        Me.UltraLabel19.Size = New System.Drawing.Size(222, 16)
        Me.UltraLabel19.TabIndex = 164
        Me.UltraLabel19.Text = "*Numeración factura venta:"
        '
        'CH_Predeterminada
        '
        Me.CH_Predeterminada.Location = New System.Drawing.Point(270, 177)
        Me.CH_Predeterminada.Name = "CH_Predeterminada"
        Me.CH_Predeterminada.Size = New System.Drawing.Size(178, 21)
        Me.CH_Predeterminada.TabIndex = 162
        Me.CH_Predeterminada.Text = "Empresa predeterminada"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(14, 276)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel3.TabIndex = 161
        Me.UltraLabel3.Text = "Logo:"
        '
        'PictureEdit1
        '
        Me.PictureEdit1.Location = New System.Drawing.Point(13, 293)
        Me.PictureEdit1.Name = "PictureEdit1"
        Me.PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        Me.PictureEdit1.Properties.ZoomAccelerationFactor = 1.0R
        Me.PictureEdit1.Size = New System.Drawing.Size(240, 235)
        Me.PictureEdit1.TabIndex = 160
        '
        'T_Contraseña
        '
        Me.T_Contraseña.Location = New System.Drawing.Point(541, 233)
        Me.T_Contraseña.Name = "T_Contraseña"
        Me.T_Contraseña.Size = New System.Drawing.Size(246, 21)
        Me.T_Contraseña.TabIndex = 16
        Me.T_Contraseña.Text = "T_Contraseña"
        '
        'UltraLabel16
        '
        Me.UltraLabel16.Location = New System.Drawing.Point(542, 218)
        Me.UltraLabel16.Name = "UltraLabel16"
        Me.UltraLabel16.Size = New System.Drawing.Size(151, 16)
        Me.UltraLabel16.TabIndex = 159
        Me.UltraLabel16.Text = "Contraseña:"
        '
        'T_URLAcceso
        '
        Me.T_URLAcceso.Location = New System.Drawing.Point(13, 233)
        Me.T_URLAcceso.Name = "T_URLAcceso"
        Me.T_URLAcceso.Size = New System.Drawing.Size(240, 21)
        Me.T_URLAcceso.TabIndex = 14
        Me.T_URLAcceso.Text = "T_URLAcceso"
        '
        'T_Usuario
        '
        Me.T_Usuario.Location = New System.Drawing.Point(275, 233)
        Me.T_Usuario.Name = "T_Usuario"
        Me.T_Usuario.Size = New System.Drawing.Size(255, 21)
        Me.T_Usuario.TabIndex = 15
        Me.T_Usuario.Text = "T_Usuario"
        '
        'UltraLabel17
        '
        Me.UltraLabel17.Location = New System.Drawing.Point(14, 218)
        Me.UltraLabel17.Name = "UltraLabel17"
        Me.UltraLabel17.Size = New System.Drawing.Size(244, 16)
        Me.UltraLabel17.TabIndex = 156
        Me.UltraLabel17.Text = "URL acceso:"
        '
        'UltraLabel18
        '
        Me.UltraLabel18.Location = New System.Drawing.Point(276, 218)
        Me.UltraLabel18.Name = "UltraLabel18"
        Me.UltraLabel18.Size = New System.Drawing.Size(187, 16)
        Me.UltraLabel18.TabIndex = 157
        Me.UltraLabel18.Text = "Usuario:"
        '
        'T_CP
        '
        Me.T_CP.Location = New System.Drawing.Point(798, 127)
        Me.T_CP.Name = "T_CP"
        Me.T_CP.Size = New System.Drawing.Size(165, 21)
        Me.T_CP.TabIndex = 11
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
        Me.DT_Baja.TabIndex = 13
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
        Me.T_Fax.TabIndex = 7
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
        'T_NIF
        '
        Me.T_NIF.Location = New System.Drawing.Point(13, 78)
        Me.T_NIF.Name = "T_NIF"
        Me.T_NIF.Size = New System.Drawing.Size(120, 21)
        Me.T_NIF.TabIndex = 3
        Me.T_NIF.Text = "T_NIF"
        '
        'UltraLabel11
        '
        Me.UltraLabel11.Location = New System.Drawing.Point(13, 63)
        Me.UltraLabel11.Name = "UltraLabel11"
        Me.UltraLabel11.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel11.TabIndex = 145
        Me.UltraLabel11.Text = "NIF / CIF:"
        '
        'T_NombreComercial
        '
        Me.T_NombreComercial.Location = New System.Drawing.Point(573, 30)
        Me.T_NombreComercial.Name = "T_NombreComercial"
        Me.T_NombreComercial.Size = New System.Drawing.Size(390, 21)
        Me.T_NombreComercial.TabIndex = 2
        Me.T_NombreComercial.Text = "T_NombreComercial"
        '
        'UltraLabel8
        '
        Me.UltraLabel8.Location = New System.Drawing.Point(573, 15)
        Me.UltraLabel8.Name = "UltraLabel8"
        Me.UltraLabel8.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel8.TabIndex = 143
        Me.UltraLabel8.Text = "Nombre comercial"
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
        Me.DT_Alta.TabIndex = 12
        Me.DT_Alta.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'T_Telefono
        '
        Me.T_Telefono.Location = New System.Drawing.Point(635, 78)
        Me.T_Telefono.Name = "T_Telefono"
        Me.T_Telefono.Size = New System.Drawing.Size(152, 21)
        Me.T_Telefono.TabIndex = 6
        Me.T_Telefono.Text = "T_Telefono"
        '
        'T_Direccion
        '
        Me.T_Direccion.Location = New System.Drawing.Point(13, 127)
        Me.T_Direccion.Name = "T_Direccion"
        Me.T_Direccion.Size = New System.Drawing.Size(240, 21)
        Me.T_Direccion.TabIndex = 8
        Me.T_Direccion.Text = "T_Direccion"
        '
        'T_Email
        '
        Me.T_Email.Location = New System.Drawing.Point(388, 78)
        Me.T_Email.Name = "T_Email"
        Me.T_Email.Size = New System.Drawing.Size(230, 21)
        Me.T_Email.TabIndex = 5
        Me.T_Email.Text = "T_Email"
        '
        'T_Nombre
        '
        Me.T_Nombre.Location = New System.Drawing.Point(149, 30)
        Me.T_Nombre.Name = "T_Nombre"
        Me.T_Nombre.Size = New System.Drawing.Size(407, 21)
        Me.T_Nombre.TabIndex = 1
        Me.T_Nombre.Text = "T_Nombre"
        '
        'T_Persona_Contacto
        '
        Me.T_Persona_Contacto.Location = New System.Drawing.Point(149, 78)
        Me.T_Persona_Contacto.Name = "T_Persona_Contacto"
        Me.T_Persona_Contacto.Size = New System.Drawing.Size(222, 21)
        Me.T_Persona_Contacto.TabIndex = 4
        Me.T_Persona_Contacto.Text = "T_Persona_Contacto"
        '
        'T_Poblacion
        '
        Me.T_Poblacion.Location = New System.Drawing.Point(270, 127)
        Me.T_Poblacion.Name = "T_Poblacion"
        Me.T_Poblacion.Size = New System.Drawing.Size(244, 21)
        Me.T_Poblacion.TabIndex = 9
        Me.T_Poblacion.Text = "T_Poblacion"
        '
        'T_Provincia
        '
        Me.T_Provincia.Location = New System.Drawing.Point(532, 127)
        Me.T_Provincia.Name = "T_Provincia"
        Me.T_Provincia.Size = New System.Drawing.Size(255, 21)
        Me.T_Provincia.TabIndex = 10
        Me.T_Provincia.Text = "T_Provincia"
        '
        'UltraLabel28
        '
        Me.UltraLabel28.Location = New System.Drawing.Point(13, 163)
        Me.UltraLabel28.Name = "UltraLabel28"
        Me.UltraLabel28.Size = New System.Drawing.Size(90, 16)
        Me.UltraLabel28.TabIndex = 140
        Me.UltraLabel28.Text = "Fecha de alta:"
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
        Me.UltraLabel5.Text = "* Nombre o razón social"
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
        Me.UltraLabel15.Location = New System.Drawing.Point(149, 63)
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
        Me.UltraTabPageControl2.Controls.Add(Me.GRD_Delegaciones)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(980, 560)
        '
        'GRD_Delegaciones
        '
        Me.GRD_Delegaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Delegaciones.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Delegaciones.Name = "GRD_Delegaciones"
        Me.GRD_Delegaciones.pAccessibleName = Nothing
        Me.GRD_Delegaciones.pActivarBotonFiltro = False
        Me.GRD_Delegaciones.pText = " "
        Me.GRD_Delegaciones.Size = New System.Drawing.Size(980, 560)
        Me.GRD_Delegaciones.TabIndex = 134
        '
        'UltraTabPageControl5
        '
        Me.UltraTabPageControl5.Controls.Add(Me.GRD_CuentasBancarias)
        Me.UltraTabPageControl5.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl5.Name = "UltraTabPageControl5"
        Me.UltraTabPageControl5.Size = New System.Drawing.Size(980, 560)
        '
        'GRD_CuentasBancarias
        '
        Me.GRD_CuentasBancarias.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_CuentasBancarias.Location = New System.Drawing.Point(0, 0)
        Me.GRD_CuentasBancarias.Name = "GRD_CuentasBancarias"
        Me.GRD_CuentasBancarias.pAccessibleName = Nothing
        Me.GRD_CuentasBancarias.pActivarBotonFiltro = False
        Me.GRD_CuentasBancarias.pText = " "
        Me.GRD_CuentasBancarias.Size = New System.Drawing.Size(980, 560)
        Me.GRD_CuentasBancarias.TabIndex = 135
        '
        'UltraTabPageControl6
        '
        Me.UltraTabPageControl6.Controls.Add(Me.C_Año)
        Me.UltraTabPageControl6.Controls.Add(Me.UltraLabel2)
        Me.UltraTabPageControl6.Controls.Add(Me.GRD_Festivos)
        Me.UltraTabPageControl6.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl6.Name = "UltraTabPageControl6"
        Me.UltraTabPageControl6.Size = New System.Drawing.Size(980, 560)
        '
        'C_Año
        '
        Me.C_Año.Location = New System.Drawing.Point(3, 22)
        Me.C_Año.Name = "C_Año"
        Me.C_Año.Size = New System.Drawing.Size(95, 21)
        Me.C_Año.TabIndex = 0
        Me.C_Año.Text = "C_Año"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(3, 7)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(114, 16)
        Me.UltraLabel2.TabIndex = 153
        Me.UltraLabel2.Text = "Año:"
        '
        'GRD_Festivos
        '
        Me.GRD_Festivos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Festivos.Location = New System.Drawing.Point(0, 53)
        Me.GRD_Festivos.Name = "GRD_Festivos"
        Me.GRD_Festivos.pAccessibleName = Nothing
        Me.GRD_Festivos.pActivarBotonFiltro = False
        Me.GRD_Festivos.pText = " "
        Me.GRD_Festivos.Size = New System.Drawing.Size(983, 507)
        Me.GRD_Festivos.TabIndex = 133
        '
        'UltraTabPageControl9
        '
        Me.UltraTabPageControl9.Controls.Add(Me.R_Observaciones)
        Me.UltraTabPageControl9.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl9.Name = "UltraTabPageControl9"
        Me.UltraTabPageControl9.Size = New System.Drawing.Size(980, 560)
        '
        'R_Observaciones
        '
        Me.R_Observaciones.BotoGuardar = Nothing
        Me.R_Observaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.R_Observaciones.Location = New System.Drawing.Point(0, 0)
        Me.R_Observaciones.Name = "R_Observaciones"
        Me.R_Observaciones.pBotoGuardarVisible = False
        Me.R_Observaciones.pEnable = True
        Me.R_Observaciones.pText = resources.GetString("R_Observaciones.pText")
        Me.R_Observaciones.pTextEspecial = ""
        Me.R_Observaciones.Size = New System.Drawing.Size(980, 560)
        Me.R_Observaciones.TabIndex = 2
        '
        'UltraTabPageControl4
        '
        Me.UltraTabPageControl4.Controls.Add(Me.GRD_Ficheros)
        Me.UltraTabPageControl4.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl4.Name = "UltraTabPageControl4"
        Me.UltraTabPageControl4.Size = New System.Drawing.Size(980, 560)
        '
        'GRD_Ficheros
        '
        Me.GRD_Ficheros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Ficheros.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Ficheros.Name = "GRD_Ficheros"
        Me.GRD_Ficheros.pAccessibleName = Nothing
        Me.GRD_Ficheros.pActivarBotonFiltro = False
        Me.GRD_Ficheros.pText = " "
        Me.GRD_Ficheros.Size = New System.Drawing.Size(980, 560)
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
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl6)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl2)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl5)
        Me.TAB_Principal.Location = New System.Drawing.Point(12, 43)
        Me.TAB_Principal.Name = "TAB_Principal"
        Me.TAB_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.TAB_Principal.Size = New System.Drawing.Size(984, 586)
        Me.TAB_Principal.TabIndex = 107
        UltraTab1.Key = "General"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "General"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Delegaciones"
        UltraTab4.Key = "CuentasBancarias"
        UltraTab4.TabPage = Me.UltraTabPageControl5
        UltraTab4.Text = "Cuentas bancarias"
        UltraTab3.Key = "Festivos"
        UltraTab3.TabPage = Me.UltraTabPageControl6
        UltraTab3.Text = "Festivos"
        UltraTab5.Key = "Observaciones"
        UltraTab5.TabPage = Me.UltraTabPageControl9
        UltraTab5.Text = "Observaciones"
        UltraTab6.Key = "Ficheros"
        UltraTab6.TabPage = Me.UltraTabPageControl4
        UltraTab6.Text = "Ficheros"
        Me.TAB_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2, UltraTab4, UltraTab3, UltraTab5, UltraTab6})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(980, 560)
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
        'UltraColorPicker1
        '
        Me.UltraColorPicker1.Location = New System.Drawing.Point(798, 233)
        Me.UltraColorPicker1.Name = "UltraColorPicker1"
        Me.UltraColorPicker1.Size = New System.Drawing.Size(165, 21)
        Me.UltraColorPicker1.TabIndex = 171
        Me.UltraColorPicker1.Text = "Control"
        '
        'UltraLabel23
        '
        Me.UltraLabel23.Location = New System.Drawing.Point(798, 218)
        Me.UltraLabel23.Name = "UltraLabel23"
        Me.UltraLabel23.Size = New System.Drawing.Size(151, 16)
        Me.UltraLabel23.TabIndex = 172
        Me.UltraLabel23.Text = "Color:"
        '
        'frmEmpresa
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 641)
        Me.Controls.Add(Me.TAB_Principal)
        Me.KeyPreview = True
        Me.Name = "frmEmpresa"
        Me.Text = "Empresa"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.TAB_Principal, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl1.PerformLayout()
        CType(Me.CH_Predeterminada, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Contraseña, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_URLAcceso, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Usuario, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_CP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Baja, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Fax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_NIF, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_NombreComercial, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Telefono, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Direccion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Email, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Nombre, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Persona_Contacto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Poblacion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Provincia, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl2.ResumeLayout(False)
        Me.UltraTabPageControl5.ResumeLayout(False)
        Me.UltraTabPageControl6.ResumeLayout(False)
        Me.UltraTabPageControl6.PerformLayout()
        CType(Me.C_Año, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl9.ResumeLayout(False)
        Me.UltraTabPageControl4.ResumeLayout(False)
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_Principal.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraColorPicker1, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents GRD_Ficheros As M_UltraGrid.m_UltraGrid
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents DT_Baja As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel13 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Fax As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel12 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NIF As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel11 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NombreComercial As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel8 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Codigo As M_MaskEditor.m_MaskEditor
    Friend WithEvents DT_Alta As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents T_Telefono As M_TextEditor.m_TextEditor
    Friend WithEvents T_Direccion As M_TextEditor.m_TextEditor
    Friend WithEvents T_Email As M_TextEditor.m_TextEditor
    Friend WithEvents T_Nombre As M_TextEditor.m_TextEditor
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
    Friend WithEvents UltraTabPageControl6 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Festivos As M_UltraGrid.m_UltraGrid
    Friend WithEvents T_Contraseña As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel16 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_URLAcceso As M_TextEditor.m_TextEditor
    Friend WithEvents T_Usuario As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel17 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel18 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents R_Observaciones As M_RichText.M_RichText
    Friend WithEvents C_Año As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Delegaciones As M_UltraGrid.m_UltraGrid
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents UltraTabPageControl5 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_CuentasBancarias As M_UltraGrid.m_UltraGrid
    Friend WithEvents CH_Predeterminada As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents T_NumeracionFacturaVenta As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel19 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NumeracionFacturaCompraRectificativa As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel22 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NumeracionFacturaVentaRectificativa As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel21 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NumeracionFacturaCompra As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel20 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraColorPicker1 As Infragistics.Win.UltraWinEditors.UltraColorPicker
    Friend WithEvents UltraLabel23 As Infragistics.Win.Misc.UltraLabel
End Class
