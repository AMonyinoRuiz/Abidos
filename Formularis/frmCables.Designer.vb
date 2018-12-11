<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCables
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
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Hilos = New M_UltraGrid.m_UltraGrid()
        Me.TE_Codigo = New M_MaskEditor.m_MaskEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Size = New System.Drawing.Size(558, 24)
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
        'GRD_Hilos
        '
        Me.GRD_Hilos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Hilos.Location = New System.Drawing.Point(12, 85)
        Me.GRD_Hilos.Name = "GRD_Hilos"
        Me.GRD_Hilos.pAccessibleName = Nothing
        Me.GRD_Hilos.pActivarBotonFiltro = False
        Me.GRD_Hilos.pText = " "
        Me.GRD_Hilos.Size = New System.Drawing.Size(558, 471)
        Me.GRD_Hilos.TabIndex = 134
        '
        'TE_Codigo
        '
        Me.TE_Codigo.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.TE_Codigo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.TE_Codigo.InputMask = "nnnnnnnnnn"
        Me.TE_Codigo.Location = New System.Drawing.Point(12, 59)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.Size = New System.Drawing.Size(120, 20)
        Me.TE_Codigo.TabIndex = 0
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(150, 58)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(420, 21)
        Me.T_Descripcion.TabIndex = 1
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(120, 15)
        Me.UltraLabel1.TabIndex = 137
        Me.UltraLabel1.Text = "*Código:"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(150, 42)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 138
        Me.UltraLabel5.Text = "*Descripción"
        '
        'frmCables
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(582, 568)
        Me.Controls.Add(Me.TE_Codigo)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel5)
        Me.Controls.Add(Me.GRD_Hilos)
        Me.KeyPreview = True
        Me.Name = "frmCables"
        Me.Text = "Tipos de cable"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.GRD_Hilos, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.TE_Codigo, 0)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Hilos As M_UltraGrid.m_UltraGrid
    Friend WithEvents TE_Codigo As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
End Class
