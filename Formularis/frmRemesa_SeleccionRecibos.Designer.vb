<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRemesa_SeleccionRecibos
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
        Me.L_NS = New Infragistics.Win.Misc.UltraLabel()
        Me.GRD_Lineas = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.DT_Remesa = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel13 = New Infragistics.Win.Misc.UltraLabel()
        Me.B_Buscar = New Infragistics.Win.Misc.UltraButton()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.DT_Remesa, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(987, 24)
        Me.ToolForm.TabIndex = 29
        '
        'L_NS
        '
        Me.L_NS.Location = New System.Drawing.Point(224, 32)
        Me.L_NS.Name = "L_NS"
        Me.L_NS.Size = New System.Drawing.Size(312, 17)
        Me.L_NS.TabIndex = 137
        '
        'GRD_Lineas
        '
        Me.GRD_Lineas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Lineas.Location = New System.Drawing.Point(12, 105)
        Me.GRD_Lineas.Name = "GRD_Lineas"
        Me.GRD_Lineas.pAccessibleName = Nothing
        Me.GRD_Lineas.pActivarBotonFiltro = False
        Me.GRD_Lineas.pText = " "
        Me.GRD_Lineas.Size = New System.Drawing.Size(987, 409)
        Me.GRD_Lineas.TabIndex = 136
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
        'DT_Remesa
        '
        Me.DT_Remesa.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Remesa.Location = New System.Drawing.Point(12, 69)
        Me.DT_Remesa.Name = "DT_Remesa"
        Me.DT_Remesa.Size = New System.Drawing.Size(90, 21)
        Me.DT_Remesa.TabIndex = 150
        Me.DT_Remesa.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel13
        '
        Me.UltraLabel13.Location = New System.Drawing.Point(12, 55)
        Me.UltraLabel13.Name = "UltraLabel13"
        Me.UltraLabel13.Size = New System.Drawing.Size(90, 16)
        Me.UltraLabel13.TabIndex = 151
        Me.UltraLabel13.Text = "*Fecha remesa:"
        '
        'B_Buscar
        '
        Me.B_Buscar.Location = New System.Drawing.Point(118, 69)
        Me.B_Buscar.Name = "B_Buscar"
        Me.B_Buscar.Size = New System.Drawing.Size(80, 21)
        Me.B_Buscar.TabIndex = 152
        Me.B_Buscar.Text = "Buscar"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 518)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(322, 13)
        Me.Label1.TabIndex = 153
        Me.Label1.Text = "*Para poder seleccionar los recibos debe presionar el botón buscar"
        '
        'frmRemesa_SeleccionRecibos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 540)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.B_Buscar)
        Me.Controls.Add(Me.DT_Remesa)
        Me.Controls.Add(Me.UltraLabel13)
        Me.Controls.Add(Me.L_NS)
        Me.Controls.Add(Me.GRD_Lineas)
        Me.KeyPreview = True
        Me.Name = "frmRemesa_SeleccionRecibos"
        Me.Text = "Selección de vencimientos"
        Me.Controls.SetChildIndex(Me.GRD_Lineas, 0)
        Me.Controls.SetChildIndex(Me.L_NS, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel13, 0)
        Me.Controls.SetChildIndex(Me.DT_Remesa, 0)
        Me.Controls.SetChildIndex(Me.B_Buscar, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        CType(Me.DT_Remesa, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents GRD_Lineas As M_UltraGrid.m_UltraGrid
    Friend WithEvents L_NS As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents DT_Remesa As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel13 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents B_Buscar As Infragistics.Win.Misc.UltraButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
