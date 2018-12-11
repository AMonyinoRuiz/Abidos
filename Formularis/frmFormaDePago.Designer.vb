<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFormaDePago
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFormaDePago))
        Dim UltraTab4 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab5 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab3 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl4 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Uso_Cliente = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl5 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Uso_Proveedor = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.R_Condiciones = New M_RichText.M_RichText()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Giros = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage2 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.TAB_General = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.C_Tipo = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.L_Estado = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Codigo = New M_TextEditor.m_TextEditor()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.CH_Predeterminada = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.C_CuentaBancaria = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl4.SuspendLayout()
        Me.UltraTabPageControl5.SuspendLayout()
        Me.UltraTabPageControl3.SuspendLayout()
        Me.UltraTabPageControl2.SuspendLayout()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.UltraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabControl1.SuspendLayout()
        CType(Me.TAB_General, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_General.SuspendLayout()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Tipo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TE_Codigo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CH_Predeterminada, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_CuentaBancaria, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Size = New System.Drawing.Size(986, 24)
        '
        'UltraTabPageControl4
        '
        Me.UltraTabPageControl4.Controls.Add(Me.GRD_Uso_Cliente)
        Me.UltraTabPageControl4.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl4.Name = "UltraTabPageControl4"
        Me.UltraTabPageControl4.Size = New System.Drawing.Size(978, 482)
        '
        'GRD_Uso_Cliente
        '
        Me.GRD_Uso_Cliente.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Uso_Cliente.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Uso_Cliente.Name = "GRD_Uso_Cliente"
        Me.GRD_Uso_Cliente.pAccessibleName = Nothing
        Me.GRD_Uso_Cliente.pActivarBotonFiltro = False
        Me.GRD_Uso_Cliente.pText = " "
        Me.GRD_Uso_Cliente.Size = New System.Drawing.Size(978, 482)
        Me.GRD_Uso_Cliente.TabIndex = 144
        '
        'UltraTabPageControl5
        '
        Me.UltraTabPageControl5.Controls.Add(Me.GRD_Uso_Proveedor)
        Me.UltraTabPageControl5.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl5.Name = "UltraTabPageControl5"
        Me.UltraTabPageControl5.Size = New System.Drawing.Size(978, 482)
        '
        'GRD_Uso_Proveedor
        '
        Me.GRD_Uso_Proveedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Uso_Proveedor.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Uso_Proveedor.Name = "GRD_Uso_Proveedor"
        Me.GRD_Uso_Proveedor.pAccessibleName = Nothing
        Me.GRD_Uso_Proveedor.pActivarBotonFiltro = False
        Me.GRD_Uso_Proveedor.pText = " "
        Me.GRD_Uso_Proveedor.Size = New System.Drawing.Size(978, 482)
        Me.GRD_Uso_Proveedor.TabIndex = 145
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Controls.Add(Me.R_Condiciones)
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(982, 508)
        '
        'R_Condiciones
        '
        Me.R_Condiciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.R_Condiciones.Location = New System.Drawing.Point(0, 0)
        Me.R_Condiciones.Name = "R_Condiciones"
        Me.R_Condiciones.pEnable = True
        Me.R_Condiciones.pText = resources.GetString("R_Condiciones.pText")
        Me.R_Condiciones.pTextEspecial = ""
        Me.R_Condiciones.Size = New System.Drawing.Size(982, 508)
        Me.R_Condiciones.TabIndex = 0
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.GRD_Giros)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(982, 508)
        '
        'GRD_Giros
        '
        Me.GRD_Giros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Giros.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Giros.Name = "GRD_Giros"
        Me.GRD_Giros.pAccessibleName = Nothing
        Me.GRD_Giros.pActivarBotonFiltro = False
        Me.GRD_Giros.pText = " "
        Me.GRD_Giros.Size = New System.Drawing.Size(982, 508)
        Me.GRD_Giros.TabIndex = 145
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.UltraTabControl1)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(982, 508)
        '
        'UltraTabControl1
        '
        Me.UltraTabControl1.Controls.Add(Me.UltraTabSharedControlsPage2)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabPageControl4)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabPageControl5)
        Me.UltraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UltraTabControl1.Location = New System.Drawing.Point(0, 0)
        Me.UltraTabControl1.Name = "UltraTabControl1"
        Me.UltraTabControl1.SharedControlsPage = Me.UltraTabSharedControlsPage2
        Me.UltraTabControl1.Size = New System.Drawing.Size(982, 508)
        Me.UltraTabControl1.TabIndex = 145
        UltraTab4.TabPage = Me.UltraTabPageControl4
        UltraTab4.Text = "Clientes"
        UltraTab5.TabPage = Me.UltraTabPageControl5
        UltraTab5.Text = "Proveedores"
        Me.UltraTabControl1.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab4, UltraTab5})
        '
        'UltraTabSharedControlsPage2
        '
        Me.UltraTabSharedControlsPage2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage2.Name = "UltraTabSharedControlsPage2"
        Me.UltraTabSharedControlsPage2.Size = New System.Drawing.Size(978, 482)
        '
        'TAB_General
        '
        Me.TAB_General.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TAB_General.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl1)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl3)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl2)
        Me.TAB_General.Location = New System.Drawing.Point(12, 95)
        Me.TAB_General.Name = "TAB_General"
        Me.TAB_General.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.TAB_General.Size = New System.Drawing.Size(986, 534)
        Me.TAB_General.TabIndex = 157
        UltraTab3.Key = "Condiciones"
        UltraTab3.TabPage = Me.UltraTabPageControl3
        UltraTab3.Text = "Condiciones"
        UltraTab2.Key = "Giros"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Giros"
        UltraTab1.Key = "Uso"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Uso"
        Me.TAB_General.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab3, UltraTab2, UltraTab1})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(982, 508)
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(148, 58)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(344, 21)
        Me.T_Descripcion.TabIndex = 1
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel1.TabIndex = 155
        Me.UltraLabel1.Text = "*Código:"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(147, 42)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 156
        Me.UltraLabel5.Text = "*Descripción"
        '
        'C_Tipo
        '
        Me.C_Tipo.Location = New System.Drawing.Point(504, 58)
        Me.C_Tipo.Name = "C_Tipo"
        Me.C_Tipo.Size = New System.Drawing.Size(172, 21)
        Me.C_Tipo.TabIndex = 2
        Me.C_Tipo.Text = "C_Tipo"
        '
        'L_Estado
        '
        Me.L_Estado.Location = New System.Drawing.Point(504, 42)
        Me.L_Estado.Name = "L_Estado"
        Me.L_Estado.Size = New System.Drawing.Size(112, 16)
        Me.L_Estado.TabIndex = 221
        Me.L_Estado.Text = "*Tipo:"
        '
        'TE_Codigo
        '
        Me.TE_Codigo.Location = New System.Drawing.Point(12, 58)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.Size = New System.Drawing.Size(125, 21)
        Me.TE_Codigo.TabIndex = 0
        Me.TE_Codigo.Text = "TE_Codigo"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'CH_Predeterminada
        '
        Me.CH_Predeterminada.Location = New System.Drawing.Point(891, 58)
        Me.CH_Predeterminada.Name = "CH_Predeterminada"
        Me.CH_Predeterminada.Size = New System.Drawing.Size(104, 19)
        Me.CH_Predeterminada.TabIndex = 3
        Me.CH_Predeterminada.Text = "Predeterminada"
        '
        'C_CuentaBancaria
        '
        Me.C_CuentaBancaria.Location = New System.Drawing.Point(690, 56)
        Me.C_CuentaBancaria.Name = "C_CuentaBancaria"
        Me.C_CuentaBancaria.Size = New System.Drawing.Size(184, 21)
        Me.C_CuentaBancaria.TabIndex = 222
        Me.C_CuentaBancaria.Text = "C_CuentaBancaria"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(690, 39)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(112, 16)
        Me.UltraLabel2.TabIndex = 223
        Me.UltraLabel2.Text = "*Cuenta bancaria:"
        '
        'frmFormaDePago
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 641)
        Me.Controls.Add(Me.C_CuentaBancaria)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.CH_Predeterminada)
        Me.Controls.Add(Me.TE_Codigo)
        Me.Controls.Add(Me.C_Tipo)
        Me.Controls.Add(Me.L_Estado)
        Me.Controls.Add(Me.TAB_General)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel5)
        Me.KeyPreview = True
        Me.Name = "frmFormaDePago"
        Me.Text = "Formas de pago"
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.TAB_General, 0)
        Me.Controls.SetChildIndex(Me.L_Estado, 0)
        Me.Controls.SetChildIndex(Me.C_Tipo, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.TE_Codigo, 0)
        Me.Controls.SetChildIndex(Me.CH_Predeterminada, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel2, 0)
        Me.Controls.SetChildIndex(Me.C_CuentaBancaria, 0)
        Me.UltraTabPageControl4.ResumeLayout(False)
        Me.UltraTabPageControl5.ResumeLayout(False)
        Me.UltraTabPageControl3.ResumeLayout(False)
        Me.UltraTabPageControl2.ResumeLayout(False)
        Me.UltraTabPageControl1.ResumeLayout(False)
        CType(Me.UltraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabControl1.ResumeLayout(False)
        CType(Me.TAB_General, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_General.ResumeLayout(False)
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Tipo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TE_Codigo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CH_Predeterminada, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_CuentaBancaria, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TAB_General As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Uso_Cliente As M_UltraGrid.m_UltraGrid
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents R_Condiciones As M_RichText.M_RichText
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_Tipo As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents L_Estado As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Codigo As M_TextEditor.m_TextEditor
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Giros As M_UltraGrid.m_UltraGrid
    Friend WithEvents UltraTabControl1 As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage2 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl4 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl5 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Uso_Proveedor As M_UltraGrid.m_UltraGrid
    Friend WithEvents CH_Predeterminada As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents C_CuentaBancaria As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
End Class
