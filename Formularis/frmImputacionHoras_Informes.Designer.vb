<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImputacionHoras_Informes
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
        Dim UltraTab11 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab12 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmImputacionHoras_Informes))
        Me.UltraTabControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage3 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.R_Informe = New M_RichText.M_RichText()
        Me.UltraTabPageControl12 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.R_ExplicacionHoras = New M_RichText.M_RichText()
        CType(Me.UltraTabControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabControl2.SuspendLayout()
        Me.UltraTabPageControl11.SuspendLayout()
        Me.UltraTabPageControl12.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.SeleccionarSortir
        Me.ToolForm.Size = New System.Drawing.Size(980, 24)
        '
        'UltraTabControl2
        '
        Me.UltraTabControl2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UltraTabControl2.Controls.Add(Me.UltraTabSharedControlsPage3)
        Me.UltraTabControl2.Controls.Add(Me.UltraTabPageControl11)
        Me.UltraTabControl2.Controls.Add(Me.UltraTabPageControl12)
        Me.UltraTabControl2.Location = New System.Drawing.Point(12, 54)
        Me.UltraTabControl2.Name = "UltraTabControl2"
        Me.UltraTabControl2.SharedControlsPage = Me.UltraTabSharedControlsPage3
        Me.UltraTabControl2.Size = New System.Drawing.Size(980, 606)
        Me.UltraTabControl2.TabIndex = 163
        UltraTab11.TabPage = Me.UltraTabPageControl11
        UltraTab11.Text = "Informe"
        UltraTab12.TabPage = Me.UltraTabPageControl12
        UltraTab12.Text = "Explicación horas"
        Me.UltraTabControl2.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab11, UltraTab12})
        '
        'UltraTabSharedControlsPage3
        '
        Me.UltraTabSharedControlsPage3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage3.Name = "UltraTabSharedControlsPage3"
        Me.UltraTabSharedControlsPage3.Size = New System.Drawing.Size(976, 580)
        '
        'UltraTabPageControl11
        '
        Me.UltraTabPageControl11.Controls.Add(Me.R_Informe)
        Me.UltraTabPageControl11.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl11.Name = "UltraTabPageControl11"
        Me.UltraTabPageControl11.Size = New System.Drawing.Size(976, 580)
        '
        'R_Informe
        '
        Me.R_Informe.Dock = System.Windows.Forms.DockStyle.Fill
        Me.R_Informe.Location = New System.Drawing.Point(0, 0)
        Me.R_Informe.Name = "R_Informe"
        Me.R_Informe.pEnable = True
        Me.R_Informe.pText = resources.GetString("R_Informe.pText")
        Me.R_Informe.pTextEspecial = ""
        Me.R_Informe.Size = New System.Drawing.Size(976, 580)
        Me.R_Informe.TabIndex = 160
        '
        'UltraTabPageControl12
        '
        Me.UltraTabPageControl12.Controls.Add(Me.R_ExplicacionHoras)
        Me.UltraTabPageControl12.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl12.Name = "UltraTabPageControl12"
        Me.UltraTabPageControl12.Size = New System.Drawing.Size(976, 580)
        '
        'R_ExplicacionHoras
        '
        Me.R_ExplicacionHoras.Dock = System.Windows.Forms.DockStyle.Fill
        Me.R_ExplicacionHoras.Location = New System.Drawing.Point(0, 0)
        Me.R_ExplicacionHoras.Name = "R_ExplicacionHoras"
        Me.R_ExplicacionHoras.pEnable = True
        Me.R_ExplicacionHoras.pText = resources.GetString("R_ExplicacionHoras.pText")
        Me.R_ExplicacionHoras.pTextEspecial = ""
        Me.R_ExplicacionHoras.Size = New System.Drawing.Size(976, 580)
        Me.R_ExplicacionHoras.TabIndex = 161
        '
        'frmImputacionHoras_Informes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1004, 672)
        Me.Controls.Add(Me.UltraTabControl2)
        Me.KeyPreview = True
        Me.Name = "frmImputacionHoras_Informes"
        Me.Text = "Informes"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraTabControl2, 0)
        CType(Me.UltraTabControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabControl2.ResumeLayout(False)
        Me.UltraTabPageControl11.ResumeLayout(False)
        Me.UltraTabPageControl12.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraTabControl2 As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage3 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents R_Informe As M_RichText.M_RichText
    Friend WithEvents UltraTabPageControl12 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents R_ExplicacionHoras As M_RichText.M_RichText
End Class
