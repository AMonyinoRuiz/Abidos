<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFacturacionAlbaranes
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
        Me.TE_Codigo = New M_TextEditor.m_TextEditor()
        Me.T_Nombre = New M_TextEditor.m_TextEditor()
        Me.L_Codigo = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.B_CargarAlbaranes = New Infragistics.Win.Misc.UltraButton()
        Me.GRD_Albaranes = New M_UltraGrid.m_UltraGrid()
        Me.T_Total = New M_MaskEditor.m_MaskEditor()
        Me.T_TotalBase = New M_MaskEditor.m_MaskEditor()
        Me.T_TotalIVA = New M_MaskEditor.m_MaskEditor()
        Me.T_UltimaFactura = New M_TextEditor.m_TextEditor()
        Me.L_UltimaFactura = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NumeroFactura = New M_TextEditor.m_TextEditor()
        Me.C_DiaPago = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.C_FormaPago = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl6 = New DevExpress.XtraEditors.LabelControl()
        Me.C_Empresa = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel55 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.TE_Codigo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Nombre, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_UltimaFactura, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_NumeroFactura, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_DiaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_FormaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Empresa, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Size = New System.Drawing.Size(980, 24)
        '
        'TE_Codigo
        '
        Me.TE_Codigo.Location = New System.Drawing.Point(261, 63)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.ReadOnly = True
        Me.TE_Codigo.Size = New System.Drawing.Size(173, 21)
        Me.TE_Codigo.TabIndex = 141
        Me.TE_Codigo.Text = "TE_Codigo"
        '
        'T_Nombre
        '
        Me.T_Nombre.Location = New System.Drawing.Point(446, 63)
        Me.T_Nombre.Name = "T_Nombre"
        Me.T_Nombre.ReadOnly = True
        Me.T_Nombre.Size = New System.Drawing.Size(312, 21)
        Me.T_Nombre.TabIndex = 142
        Me.T_Nombre.Text = "T_Nombre"
        '
        'L_Codigo
        '
        Me.L_Codigo.Location = New System.Drawing.Point(261, 47)
        Me.L_Codigo.Name = "L_Codigo"
        Me.L_Codigo.Size = New System.Drawing.Size(116, 16)
        Me.L_Codigo.TabIndex = 143
        Me.L_Codigo.Text = "*Código cliente:"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(446, 47)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel4.TabIndex = 144
        Me.UltraLabel4.Text = "Nombre"
        '
        'B_CargarAlbaranes
        '
        Me.B_CargarAlbaranes.Location = New System.Drawing.Point(765, 62)
        Me.B_CargarAlbaranes.Name = "B_CargarAlbaranes"
        Me.B_CargarAlbaranes.Size = New System.Drawing.Size(79, 21)
        Me.B_CargarAlbaranes.TabIndex = 145
        Me.B_CargarAlbaranes.Text = "Buscar"
        '
        'GRD_Albaranes
        '
        Me.GRD_Albaranes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Albaranes.Location = New System.Drawing.Point(12, 99)
        Me.GRD_Albaranes.Name = "GRD_Albaranes"
        Me.GRD_Albaranes.pAccessibleName = Nothing
        Me.GRD_Albaranes.pActivarBotonFiltro = False
        Me.GRD_Albaranes.pText = " "
        Me.GRD_Albaranes.Size = New System.Drawing.Size(980, 440)
        Me.GRD_Albaranes.TabIndex = 146
        '
        'T_Total
        '
        Me.T_Total.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.T_Total.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_Total.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Currency
        Me.T_Total.InputMask = "{currency:-9.2}"
        Me.T_Total.Location = New System.Drawing.Point(271, 575)
        Me.T_Total.Name = "T_Total"
        Me.T_Total.NonAutoSizeHeight = 20
        Me.T_Total.ReadOnly = True
        Me.T_Total.Size = New System.Drawing.Size(114, 20)
        Me.T_Total.TabIndex = 149
        Me.T_Total.Text = "M_MaskEditor2"
        '
        'T_TotalBase
        '
        Me.T_TotalBase.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.T_TotalBase.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_TotalBase.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Currency
        Me.T_TotalBase.InputMask = "{currency:-9.2}"
        Me.T_TotalBase.Location = New System.Drawing.Point(14, 575)
        Me.T_TotalBase.Name = "T_TotalBase"
        Me.T_TotalBase.NonAutoSizeHeight = 20
        Me.T_TotalBase.ReadOnly = True
        Me.T_TotalBase.Size = New System.Drawing.Size(114, 20)
        Me.T_TotalBase.TabIndex = 147
        Me.T_TotalBase.Text = "T_Preu"
        '
        'T_TotalIVA
        '
        Me.T_TotalIVA.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.T_TotalIVA.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_TotalIVA.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.Currency
        Me.T_TotalIVA.InputMask = "{currency:-9.2}"
        Me.T_TotalIVA.Location = New System.Drawing.Point(142, 575)
        Me.T_TotalIVA.Name = "T_TotalIVA"
        Me.T_TotalIVA.NonAutoSizeHeight = 20
        Me.T_TotalIVA.ReadOnly = True
        Me.T_TotalIVA.Size = New System.Drawing.Size(114, 20)
        Me.T_TotalIVA.TabIndex = 148
        Me.T_TotalIVA.Text = "M_MaskEditor1"
        '
        'T_UltimaFactura
        '
        Me.T_UltimaFactura.Location = New System.Drawing.Point(865, 63)
        Me.T_UltimaFactura.Name = "T_UltimaFactura"
        Me.T_UltimaFactura.ReadOnly = True
        Me.T_UltimaFactura.Size = New System.Drawing.Size(116, 21)
        Me.T_UltimaFactura.TabIndex = 153
        Me.T_UltimaFactura.Text = "T_UltimaFactura"
        '
        'L_UltimaFactura
        '
        Me.L_UltimaFactura.Location = New System.Drawing.Point(865, 47)
        Me.L_UltimaFactura.Name = "L_UltimaFactura"
        Me.L_UltimaFactura.Size = New System.Drawing.Size(116, 16)
        Me.L_UltimaFactura.TabIndex = 154
        Me.L_UltimaFactura.Text = "Última factura:"
        '
        'T_NumeroFactura
        '
        Me.T_NumeroFactura.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.T_NumeroFactura.Location = New System.Drawing.Point(455, 574)
        Me.T_NumeroFactura.Name = "T_NumeroFactura"
        Me.T_NumeroFactura.Size = New System.Drawing.Size(133, 21)
        Me.T_NumeroFactura.TabIndex = 155
        Me.T_NumeroFactura.Text = "T_NumeroFactura"
        '
        'C_DiaPago
        '
        Me.C_DiaPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.C_DiaPago.Location = New System.Drawing.Point(904, 574)
        Me.C_DiaPago.Name = "C_DiaPago"
        Me.C_DiaPago.Size = New System.Drawing.Size(88, 21)
        Me.C_DiaPago.TabIndex = 173
        Me.C_DiaPago.Text = "C_DiaPago"
        '
        'C_FormaPago
        '
        Me.C_FormaPago.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.C_FormaPago.Location = New System.Drawing.Point(605, 574)
        Me.C_FormaPago.Name = "C_FormaPago"
        Me.C_FormaPago.Size = New System.Drawing.Size(282, 21)
        Me.C_FormaPago.TabIndex = 172
        Me.C_FormaPago.Text = "C_FormaPago"
        '
        'LabelControl1
        '
        Me.LabelControl1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl1.Location = New System.Drawing.Point(14, 559)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(108, 13)
        Me.LabelControl1.TabIndex = 176
        Me.LabelControl1.Text = "Base de los albaranes:"
        '
        'LabelControl2
        '
        Me.LabelControl2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl2.Location = New System.Drawing.Point(142, 559)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(102, 13)
        Me.LabelControl2.TabIndex = 177
        Me.LabelControl2.Text = "IVA de los albaranes:"
        '
        'LabelControl3
        '
        Me.LabelControl3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl3.Location = New System.Drawing.Point(271, 559)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(78, 13)
        Me.LabelControl3.TabIndex = 178
        Me.LabelControl3.Text = "Total albaranes:"
        '
        'LabelControl4
        '
        Me.LabelControl4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl4.Location = New System.Drawing.Point(455, 559)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(54, 13)
        Me.LabelControl4.TabIndex = 179
        Me.LabelControl4.Text = "Nº factura:"
        '
        'LabelControl5
        '
        Me.LabelControl5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl5.Location = New System.Drawing.Point(605, 559)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(76, 13)
        Me.LabelControl5.TabIndex = 180
        Me.LabelControl5.Text = "Forma de pago:"
        '
        'LabelControl6
        '
        Me.LabelControl6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelControl6.Location = New System.Drawing.Point(904, 559)
        Me.LabelControl6.Name = "LabelControl6"
        Me.LabelControl6.Size = New System.Drawing.Size(61, 13)
        Me.LabelControl6.TabIndex = 181
        Me.LabelControl6.Text = "Día de pago:"
        '
        'C_Empresa
        '
        Me.C_Empresa.Location = New System.Drawing.Point(14, 63)
        Me.C_Empresa.Name = "C_Empresa"
        Me.C_Empresa.Size = New System.Drawing.Size(223, 21)
        Me.C_Empresa.TabIndex = 228
        Me.C_Empresa.Text = "C_Empresa"
        '
        'UltraLabel55
        '
        Me.UltraLabel55.Location = New System.Drawing.Point(14, 48)
        Me.UltraLabel55.Name = "UltraLabel55"
        Me.UltraLabel55.Size = New System.Drawing.Size(96, 16)
        Me.UltraLabel55.TabIndex = 229
        Me.UltraLabel55.Text = "Empresa:"
        '
        'frmFacturacionAlbaranes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1004, 607)
        Me.Controls.Add(Me.C_Empresa)
        Me.Controls.Add(Me.UltraLabel55)
        Me.Controls.Add(Me.LabelControl6)
        Me.Controls.Add(Me.LabelControl5)
        Me.Controls.Add(Me.LabelControl4)
        Me.Controls.Add(Me.LabelControl3)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.C_DiaPago)
        Me.Controls.Add(Me.C_FormaPago)
        Me.Controls.Add(Me.T_NumeroFactura)
        Me.Controls.Add(Me.T_UltimaFactura)
        Me.Controls.Add(Me.L_UltimaFactura)
        Me.Controls.Add(Me.T_Total)
        Me.Controls.Add(Me.T_TotalBase)
        Me.Controls.Add(Me.T_TotalIVA)
        Me.Controls.Add(Me.GRD_Albaranes)
        Me.Controls.Add(Me.B_CargarAlbaranes)
        Me.Controls.Add(Me.TE_Codigo)
        Me.Controls.Add(Me.T_Nombre)
        Me.Controls.Add(Me.L_Codigo)
        Me.Controls.Add(Me.UltraLabel4)
        Me.KeyPreview = True
        Me.Name = "frmFacturacionAlbaranes"
        Me.Text = "Facturación de múltiples albaranes"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel4, 0)
        Me.Controls.SetChildIndex(Me.L_Codigo, 0)
        Me.Controls.SetChildIndex(Me.T_Nombre, 0)
        Me.Controls.SetChildIndex(Me.TE_Codigo, 0)
        Me.Controls.SetChildIndex(Me.B_CargarAlbaranes, 0)
        Me.Controls.SetChildIndex(Me.GRD_Albaranes, 0)
        Me.Controls.SetChildIndex(Me.T_TotalIVA, 0)
        Me.Controls.SetChildIndex(Me.T_TotalBase, 0)
        Me.Controls.SetChildIndex(Me.T_Total, 0)
        Me.Controls.SetChildIndex(Me.L_UltimaFactura, 0)
        Me.Controls.SetChildIndex(Me.T_UltimaFactura, 0)
        Me.Controls.SetChildIndex(Me.T_NumeroFactura, 0)
        Me.Controls.SetChildIndex(Me.C_FormaPago, 0)
        Me.Controls.SetChildIndex(Me.C_DiaPago, 0)
        Me.Controls.SetChildIndex(Me.LabelControl1, 0)
        Me.Controls.SetChildIndex(Me.LabelControl2, 0)
        Me.Controls.SetChildIndex(Me.LabelControl3, 0)
        Me.Controls.SetChildIndex(Me.LabelControl4, 0)
        Me.Controls.SetChildIndex(Me.LabelControl5, 0)
        Me.Controls.SetChildIndex(Me.LabelControl6, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel55, 0)
        Me.Controls.SetChildIndex(Me.C_Empresa, 0)
        CType(Me.TE_Codigo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Nombre, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_UltimaFactura, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_NumeroFactura, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_DiaPago, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_FormaPago, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Empresa, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TE_Codigo As M_TextEditor.m_TextEditor
    Friend WithEvents T_Nombre As M_TextEditor.m_TextEditor
    Friend WithEvents L_Codigo As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents B_CargarAlbaranes As Infragistics.Win.Misc.UltraButton
    Friend WithEvents GRD_Albaranes As M_UltraGrid.m_UltraGrid
    Friend WithEvents T_Total As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_TotalBase As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_TotalIVA As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_UltimaFactura As M_TextEditor.m_TextEditor
    Friend WithEvents L_UltimaFactura As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NumeroFactura As M_TextEditor.m_TextEditor
    Friend WithEvents C_DiaPago As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents C_FormaPago As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl6 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents C_Empresa As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel55 As Infragistics.Win.Misc.UltraLabel
End Class
