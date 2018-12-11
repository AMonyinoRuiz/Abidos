<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInstalacionCreacionPartes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInstalacionCreacionPartes))
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.GRD_Ficheros = New M_UltraGrid.m_UltraGrid()
        Me.R_TrabajosARealizar = New M_RichText.M_RichText()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.CH_GenerarParte = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.CH_GenerarPedidoVenta = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.DT_Documento = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.C_Empresa = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel34 = New Infragistics.Win.Misc.UltraLabel()
        Me.Panel_PedidoVenta = New Infragistics.Win.Misc.UltraGroupBox()
        Me.Panel_Parte = New Infragistics.Win.Misc.UltraGroupBox()
        CType(Me.CH_GenerarParte, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CH_GenerarPedidoVenta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Documento, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Empresa, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Panel_PedidoVenta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_PedidoVenta.SuspendLayout()
        CType(Me.Panel_Parte, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_Parte.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(1017, 24)
        Me.ToolForm.TabIndex = 29
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
        'GRD_Ficheros
        '
        Me.GRD_Ficheros.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Ficheros.Location = New System.Drawing.Point(13, 221)
        Me.GRD_Ficheros.Name = "GRD_Ficheros"
        Me.GRD_Ficheros.pAccessibleName = Nothing
        Me.GRD_Ficheros.pActivarBotonFiltro = False
        Me.GRD_Ficheros.pText = " "
        Me.GRD_Ficheros.Size = New System.Drawing.Size(969, 310)
        Me.GRD_Ficheros.TabIndex = 136
        '
        'R_TrabajosARealizar
        '
        Me.R_TrabajosARealizar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.R_TrabajosARealizar.Location = New System.Drawing.Point(13, 50)
        Me.R_TrabajosARealizar.Name = "R_TrabajosARealizar"
        Me.R_TrabajosARealizar.pBotoGuardarVisible = False
        Me.R_TrabajosARealizar.pEnable = True
        Me.R_TrabajosARealizar.pText = resources.GetString("R_TrabajosARealizar.pText")
        Me.R_TrabajosARealizar.pTextEspecial = ""
        Me.R_TrabajosARealizar.Size = New System.Drawing.Size(969, 157)
        Me.R_TrabajosARealizar.TabIndex = 0
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(17, 32)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(240, 16)
        Me.UltraLabel4.TabIndex = 207
        Me.UltraLabel4.Text = "Trabajos a realizar:"
        '
        'CH_GenerarParte
        '
        Me.CH_GenerarParte.Location = New System.Drawing.Point(12, 166)
        Me.CH_GenerarParte.Name = "CH_GenerarParte"
        Me.CH_GenerarParte.Size = New System.Drawing.Size(138, 19)
        Me.CH_GenerarParte.TabIndex = 208
        Me.CH_GenerarParte.Text = "Generar Parte"
        '
        'CH_GenerarPedidoVenta
        '
        Me.CH_GenerarPedidoVenta.Location = New System.Drawing.Point(12, 53)
        Me.CH_GenerarPedidoVenta.Name = "CH_GenerarPedidoVenta"
        Me.CH_GenerarPedidoVenta.Size = New System.Drawing.Size(164, 19)
        Me.CH_GenerarPedidoVenta.TabIndex = 209
        Me.CH_GenerarPedidoVenta.Text = "Generar pedido de venta"
        '
        'DT_Documento
        '
        Me.DT_Documento.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Documento.Location = New System.Drawing.Point(16, 41)
        Me.DT_Documento.Name = "DT_Documento"
        Me.DT_Documento.Size = New System.Drawing.Size(106, 21)
        Me.DT_Documento.TabIndex = 210
        Me.DT_Documento.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel7
        '
        Me.UltraLabel7.Location = New System.Drawing.Point(16, 25)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.Size = New System.Drawing.Size(118, 16)
        Me.UltraLabel7.TabIndex = 223
        Me.UltraLabel7.Text = "Fecha pedido venta:"
        '
        'C_Empresa
        '
        Me.C_Empresa.Location = New System.Drawing.Point(140, 41)
        Me.C_Empresa.Name = "C_Empresa"
        Me.C_Empresa.Size = New System.Drawing.Size(330, 21)
        Me.C_Empresa.TabIndex = 224
        Me.C_Empresa.Text = "C_Empresa"
        '
        'UltraLabel34
        '
        Me.UltraLabel34.Location = New System.Drawing.Point(140, 24)
        Me.UltraLabel34.Name = "UltraLabel34"
        Me.UltraLabel34.Size = New System.Drawing.Size(96, 16)
        Me.UltraLabel34.TabIndex = 225
        Me.UltraLabel34.Text = "Empresa:"
        '
        'Panel_PedidoVenta
        '
        Me.Panel_PedidoVenta.Controls.Add(Me.UltraLabel7)
        Me.Panel_PedidoVenta.Controls.Add(Me.UltraLabel34)
        Me.Panel_PedidoVenta.Controls.Add(Me.DT_Documento)
        Me.Panel_PedidoVenta.Controls.Add(Me.C_Empresa)
        Me.Panel_PedidoVenta.Location = New System.Drawing.Point(35, 78)
        Me.Panel_PedidoVenta.Name = "Panel_PedidoVenta"
        Me.Panel_PedidoVenta.Size = New System.Drawing.Size(545, 78)
        Me.Panel_PedidoVenta.TabIndex = 228
        Me.Panel_PedidoVenta.Text = "Datos del pedido de venta que se generará automáticamente"
        '
        'Panel_Parte
        '
        Me.Panel_Parte.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel_Parte.Controls.Add(Me.UltraLabel4)
        Me.Panel_Parte.Controls.Add(Me.GRD_Ficheros)
        Me.Panel_Parte.Controls.Add(Me.R_TrabajosARealizar)
        Me.Panel_Parte.Location = New System.Drawing.Point(35, 197)
        Me.Panel_Parte.Name = "Panel_Parte"
        Me.Panel_Parte.Size = New System.Drawing.Size(994, 541)
        Me.Panel_Parte.TabIndex = 229
        Me.Panel_Parte.Text = "Datos del parte que se generará automáticamente"
        '
        'frmInstalacionCreacionPartes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1039, 750)
        Me.Controls.Add(Me.Panel_Parte)
        Me.Controls.Add(Me.Panel_PedidoVenta)
        Me.Controls.Add(Me.CH_GenerarPedidoVenta)
        Me.Controls.Add(Me.CH_GenerarParte)
        Me.KeyPreview = True
        Me.Name = "frmInstalacionCreacionPartes"
        Me.Text = "Creación parte de trabajo"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.CH_GenerarParte, 0)
        Me.Controls.SetChildIndex(Me.CH_GenerarPedidoVenta, 0)
        Me.Controls.SetChildIndex(Me.Panel_PedidoVenta, 0)
        Me.Controls.SetChildIndex(Me.Panel_Parte, 0)
        CType(Me.CH_GenerarParte, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CH_GenerarPedidoVenta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Documento, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Empresa, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Panel_PedidoVenta, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_PedidoVenta.ResumeLayout(False)
        Me.Panel_PedidoVenta.PerformLayout()
        CType(Me.Panel_Parte, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_Parte.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents GRD_Ficheros As M_UltraGrid.m_UltraGrid
    Friend WithEvents R_TrabajosARealizar As M_RichText.M_RichText
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents CH_GenerarParte As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents CH_GenerarPedidoVenta As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents DT_Documento As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_Empresa As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel34 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Panel_PedidoVenta As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents Panel_Parte As Infragistics.Win.Misc.UltraGroupBox
End Class
