<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInstalacion_TipoContrato
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInstalacion_TipoContrato))
        Me.R_Condiciones = New M_RichText.M_RichText()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Codigo = New M_TextEditor.m_TextEditor()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.T_Contador = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel9 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TE_Codigo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Size = New System.Drawing.Size(986, 24)
        '
        'R_Condiciones
        '
        Me.R_Condiciones.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.R_Condiciones.Location = New System.Drawing.Point(12, 95)
        Me.R_Condiciones.Name = "R_Condiciones"
        Me.R_Condiciones.pEnable = True
        Me.R_Condiciones.pText = resources.GetString("R_Condiciones.pText")
        Me.R_Condiciones.pTextEspecial = ""
        Me.R_Condiciones.Size = New System.Drawing.Size(982, 526)
        Me.R_Condiciones.TabIndex = 0
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(148, 58)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(502, 21)
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
        'TE_Codigo
        '
        Me.TE_Codigo.Location = New System.Drawing.Point(12, 58)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.ReadOnly = True
        Me.TE_Codigo.Size = New System.Drawing.Size(125, 21)
        Me.TE_Codigo.TabIndex = 0
        Me.TE_Codigo.Text = "TE_Codigo"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'T_Contador
        '
        Me.T_Contador.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_Contador.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Integer]
        Me.T_Contador.Location = New System.Drawing.Point(668, 58)
        Me.T_Contador.Name = "T_Contador"
        Me.T_Contador.Size = New System.Drawing.Size(108, 20)
        Me.T_Contador.TabIndex = 157
        Me.T_Contador.Text = "T_Importe"
        '
        'UltraLabel9
        '
        Me.UltraLabel9.Location = New System.Drawing.Point(669, 44)
        Me.UltraLabel9.Name = "UltraLabel9"
        Me.UltraLabel9.Size = New System.Drawing.Size(107, 16)
        Me.UltraLabel9.TabIndex = 158
        Me.UltraLabel9.Text = "Contador:"
        '
        'frmInstalacion_TipoContrato
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 641)
        Me.Controls.Add(Me.T_Contador)
        Me.Controls.Add(Me.UltraLabel9)
        Me.Controls.Add(Me.R_Condiciones)
        Me.Controls.Add(Me.TE_Codigo)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel5)
        Me.KeyPreview = True
        Me.Name = "frmInstalacion_TipoContrato"
        Me.Text = "Tipo de contrato"
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.TE_Codigo, 0)
        Me.Controls.SetChildIndex(Me.R_Condiciones, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel9, 0)
        Me.Controls.SetChildIndex(Me.T_Contador, 0)
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TE_Codigo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents R_Condiciones As M_RichText.M_RichText
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Codigo As M_TextEditor.m_TextEditor
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents T_Contador As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel9 As Infragistics.Win.Misc.UltraLabel
End Class
