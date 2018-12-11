<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManteniment_Grados
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
        Me.GRD_Grado = New M_UltraGrid.m_UltraGrid()
        Me.GRD_Sector = New M_UltraGrid.m_UltraGrid()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(976, 24)
        '
        'GRD_Grado
        '
        Me.GRD_Grado.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Grado.Location = New System.Drawing.Point(12, 43)
        Me.GRD_Grado.Name = "GRD_Grado"
        Me.GRD_Grado.pAccessibleName = Nothing
        Me.GRD_Grado.pActivarBotonFiltro = False
        Me.GRD_Grado.pText = " "
        Me.GRD_Grado.Size = New System.Drawing.Size(976, 212)
        Me.GRD_Grado.TabIndex = 0
        '
        'GRD_Sector
        '
        Me.GRD_Sector.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Sector.Location = New System.Drawing.Point(12, 304)
        Me.GRD_Sector.Name = "GRD_Sector"
        Me.GRD_Sector.pAccessibleName = Nothing
        Me.GRD_Sector.pActivarBotonFiltro = False
        Me.GRD_Sector.pText = " "
        Me.GRD_Sector.Size = New System.Drawing.Size(976, 325)
        Me.GRD_Sector.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(491, 266)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(9, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "|"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(487, 279)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(17, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "\/"
        '
        'frmManteniment_Grados
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1000, 641)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GRD_Sector)
        Me.Controls.Add(Me.GRD_Grado)
        Me.KeyPreview = True
        Me.Name = "frmManteniment_Grados"
        Me.Text = "Mantenimiento de grados y tipos de negocio"
        Me.Controls.SetChildIndex(Me.GRD_Grado, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.GRD_Sector, 0)
        Me.Controls.SetChildIndex(Me.Label3, 0)
        Me.Controls.SetChildIndex(Me.Label4, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD_Grado As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Sector As M_UltraGrid.m_UltraGrid
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
