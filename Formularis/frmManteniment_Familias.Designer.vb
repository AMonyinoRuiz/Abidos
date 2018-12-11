<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManteniment_Familias
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
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab3 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab4 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Subfamilia = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Carac_Personalizadas = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Carac_Instalacion = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl4 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Mantenimientos = New M_UltraGrid.m_UltraGrid()
        Me.GRD_Division = New M_UltraGrid.m_UltraGrid()
        Me.GRD_Familia = New M_UltraGrid.m_UltraGrid()
        Me.GRD_Marca = New M_UltraGrid.m_UltraGrid()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TAB1 = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.UltraTabPageControl1.SuspendLayout()
        Me.UltraTabPageControl2.SuspendLayout()
        Me.UltraTabPageControl3.SuspendLayout()
        Me.UltraTabPageControl4.SuspendLayout()
        CType(Me.TAB1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(980, 24)
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Subfamilia)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(348, 289)
        '
        'GRD_Subfamilia
        '
        Me.GRD_Subfamilia.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Subfamilia.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Subfamilia.Name = "GRD_Subfamilia"
        Me.GRD_Subfamilia.pAccessibleName = Nothing
        Me.GRD_Subfamilia.pActivarBotonFiltro = False
        Me.GRD_Subfamilia.pText = " "
        Me.GRD_Subfamilia.Size = New System.Drawing.Size(348, 289)
        Me.GRD_Subfamilia.TabIndex = 2
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.GRD_Carac_Personalizadas)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(348, 289)
        '
        'GRD_Carac_Personalizadas
        '
        Me.GRD_Carac_Personalizadas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Carac_Personalizadas.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Carac_Personalizadas.Name = "GRD_Carac_Personalizadas"
        Me.GRD_Carac_Personalizadas.pAccessibleName = Nothing
        Me.GRD_Carac_Personalizadas.pActivarBotonFiltro = False
        Me.GRD_Carac_Personalizadas.pText = " "
        Me.GRD_Carac_Personalizadas.Size = New System.Drawing.Size(348, 289)
        Me.GRD_Carac_Personalizadas.TabIndex = 3
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Controls.Add(Me.GRD_Carac_Instalacion)
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(348, 289)
        '
        'GRD_Carac_Instalacion
        '
        Me.GRD_Carac_Instalacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Carac_Instalacion.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Carac_Instalacion.Name = "GRD_Carac_Instalacion"
        Me.GRD_Carac_Instalacion.pAccessibleName = Nothing
        Me.GRD_Carac_Instalacion.pActivarBotonFiltro = False
        Me.GRD_Carac_Instalacion.pText = " "
        Me.GRD_Carac_Instalacion.Size = New System.Drawing.Size(348, 289)
        Me.GRD_Carac_Instalacion.TabIndex = 3
        '
        'UltraTabPageControl4
        '
        Me.UltraTabPageControl4.Controls.Add(Me.GRD_Mantenimientos)
        Me.UltraTabPageControl4.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl4.Name = "UltraTabPageControl4"
        Me.UltraTabPageControl4.Size = New System.Drawing.Size(348, 289)
        '
        'GRD_Mantenimientos
        '
        Me.GRD_Mantenimientos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Mantenimientos.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Mantenimientos.Name = "GRD_Mantenimientos"
        Me.GRD_Mantenimientos.pAccessibleName = Nothing
        Me.GRD_Mantenimientos.pActivarBotonFiltro = False
        Me.GRD_Mantenimientos.pText = " "
        Me.GRD_Mantenimientos.Size = New System.Drawing.Size(348, 289)
        Me.GRD_Mantenimientos.TabIndex = 4
        '
        'GRD_Division
        '
        Me.GRD_Division.Location = New System.Drawing.Point(12, 43)
        Me.GRD_Division.Name = "GRD_Division"
        Me.GRD_Division.pAccessibleName = Nothing
        Me.GRD_Division.pActivarBotonFiltro = False
        Me.GRD_Division.pText = " "
        Me.GRD_Division.Size = New System.Drawing.Size(582, 261)
        Me.GRD_Division.TabIndex = 10
        '
        'GRD_Familia
        '
        Me.GRD_Familia.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GRD_Familia.Location = New System.Drawing.Point(12, 343)
        Me.GRD_Familia.Name = "GRD_Familia"
        Me.GRD_Familia.pAccessibleName = Nothing
        Me.GRD_Familia.pActivarBotonFiltro = False
        Me.GRD_Familia.pText = " "
        Me.GRD_Familia.Size = New System.Drawing.Size(582, 286)
        Me.GRD_Familia.TabIndex = 0
        '
        'GRD_Marca
        '
        Me.GRD_Marca.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Marca.Location = New System.Drawing.Point(640, 43)
        Me.GRD_Marca.Name = "GRD_Marca"
        Me.GRD_Marca.pAccessibleName = Nothing
        Me.GRD_Marca.pActivarBotonFiltro = False
        Me.GRD_Marca.pText = " "
        Me.GRD_Marca.Size = New System.Drawing.Size(352, 261)
        Me.GRD_Marca.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(608, 160)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(19, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = ">>"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(608, 456)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(19, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = ">>"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(287, 314)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(9, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "|"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(283, 327)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(17, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "\/"
        '
        'TAB1
        '
        Me.TAB1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TAB1.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.TAB1.Controls.Add(Me.UltraTabPageControl1)
        Me.TAB1.Controls.Add(Me.UltraTabPageControl2)
        Me.TAB1.Controls.Add(Me.UltraTabPageControl3)
        Me.TAB1.Controls.Add(Me.UltraTabPageControl4)
        Me.TAB1.Location = New System.Drawing.Point(640, 314)
        Me.TAB1.Name = "TAB1"
        Me.TAB1.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.TAB1.Size = New System.Drawing.Size(352, 315)
        Me.TAB1.TabIndex = 8
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Subfamilias"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Características personalizadas"
        UltraTab3.TabPage = Me.UltraTabPageControl3
        UltraTab3.Text = "Características instalación/revisión"
        UltraTab4.TabPage = Me.UltraTabPageControl4
        UltraTab4.Text = "Mantenimientos"
        Me.TAB1.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2, UltraTab3, UltraTab4})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(348, 289)
        '
        'frmManteniment_Familias
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1004, 641)
        Me.Controls.Add(Me.TAB1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GRD_Marca)
        Me.Controls.Add(Me.GRD_Familia)
        Me.Controls.Add(Me.GRD_Division)
        Me.KeyPreview = True
        Me.Name = "frmManteniment_Familias"
        Me.Text = "Mantenimiento de divisiones, familias, subfamilias y marcas"
        Me.Controls.SetChildIndex(Me.GRD_Division, 0)
        Me.Controls.SetChildIndex(Me.GRD_Familia, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.GRD_Marca, 0)
        Me.Controls.SetChildIndex(Me.Label1, 0)
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.Controls.SetChildIndex(Me.Label3, 0)
        Me.Controls.SetChildIndex(Me.Label4, 0)
        Me.Controls.SetChildIndex(Me.TAB1, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl2.ResumeLayout(False)
        Me.UltraTabPageControl3.ResumeLayout(False)
        Me.UltraTabPageControl4.ResumeLayout(False)
        CType(Me.TAB1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD_Division As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Familia As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Subfamilia As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Marca As M_UltraGrid.m_UltraGrid
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TAB1 As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Carac_Personalizadas As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Carac_Instalacion As M_UltraGrid.m_UltraGrid
    Friend WithEvents UltraTabPageControl4 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Mantenimientos As M_UltraGrid.m_UltraGrid
End Class
