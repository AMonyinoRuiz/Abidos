<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListado_ParteRevisiones
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
        Me.GRD = New M_UltraGrid.m_UltraGrid()
        Me.C_Division = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.C_Años = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.B_Buscar = New Infragistics.Win.Misc.UltraButton()
        CType(Me.C_Division, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Años, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.SeleccionarSortir
        Me.ToolForm.Size = New System.Drawing.Size(775, 24)
        '
        'GRD
        '
        Me.GRD.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD.Location = New System.Drawing.Point(12, 98)
        Me.GRD.Name = "GRD"
        Me.GRD.pAccessibleName = Nothing
        Me.GRD.pActivarBotonFiltro = False
        Me.GRD.pText = " "
        Me.GRD.Size = New System.Drawing.Size(775, 440)
        Me.GRD.TabIndex = 3
        '
        'C_Division
        '
        Me.C_Division.Location = New System.Drawing.Point(12, 60)
        Me.C_Division.Name = "C_Division"
        Me.C_Division.Size = New System.Drawing.Size(238, 21)
        Me.C_Division.TabIndex = 0
        Me.C_Division.Text = "C_Division"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(173, 15)
        Me.UltraLabel1.TabIndex = 3
        Me.UltraLabel1.Text = "Divisiones de los productos:"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(267, 43)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(91, 15)
        Me.UltraLabel2.TabIndex = 5
        Me.UltraLabel2.Text = "Años:"
        '
        'C_Años
        '
        Me.C_Años.Location = New System.Drawing.Point(267, 60)
        Me.C_Años.Name = "C_Años"
        Me.C_Años.Size = New System.Drawing.Size(91, 21)
        Me.C_Años.TabIndex = 1
        Me.C_Años.Text = "C_Años"
        '
        'B_Buscar
        '
        Me.B_Buscar.Location = New System.Drawing.Point(373, 59)
        Me.B_Buscar.Name = "B_Buscar"
        Me.B_Buscar.Size = New System.Drawing.Size(88, 22)
        Me.B_Buscar.TabIndex = 2
        Me.B_Buscar.Text = "Buscar"
        '
        'frmListado_ParteRevisiones
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(799, 550)
        Me.Controls.Add(Me.C_Años)
        Me.Controls.Add(Me.C_Division)
        Me.Controls.Add(Me.B_Buscar)
        Me.Controls.Add(Me.GRD)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.UltraLabel1)
        Me.KeyPreview = True
        Me.Name = "frmListado_ParteRevisiones"
        Me.Text = "Listado de revisión de las instalaciones"
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel2, 0)
        Me.Controls.SetChildIndex(Me.GRD, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.B_Buscar, 0)
        Me.Controls.SetChildIndex(Me.C_Division, 0)
        Me.Controls.SetChildIndex(Me.C_Años, 0)
        CType(Me.C_Division, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Años, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_Division As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_Años As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents B_Buscar As Infragistics.Win.Misc.UltraButton
End Class
