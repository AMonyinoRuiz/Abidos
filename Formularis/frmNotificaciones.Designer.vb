<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNotificaciones
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
        Dim UltraTab3 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab4 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab6 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab5 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl4 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Notificaciones_Enviadas_Manualmente = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Notificaciones_Enviadas_Automaticamente = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.TABStrip_Recibidas = New Infragistics.Win.UltraWinTabControl.UltraTabStripControl()
        Me.UltraTabSharedControlsPage2 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.GRD_Notificaciones_Recibidas = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage3 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.TAB_Principal = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.UltraTabPageControl4.SuspendLayout()
        Me.UltraTabPageControl3.SuspendLayout()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.TABStrip_Recibidas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TABStrip_Recibidas.SuspendLayout()
        Me.UltraTabSharedControlsPage2.SuspendLayout()
        Me.UltraTabPageControl2.SuspendLayout()
        CType(Me.UltraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabControl1.SuspendLayout()
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_Principal.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.Sortir
        Me.ToolForm.Size = New System.Drawing.Size(966, 24)
        '
        'UltraTabPageControl4
        '
        Me.UltraTabPageControl4.Controls.Add(Me.GRD_Notificaciones_Enviadas_Manualmente)
        Me.UltraTabPageControl4.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl4.Name = "UltraTabPageControl4"
        Me.UltraTabPageControl4.Size = New System.Drawing.Size(958, 522)
        '
        'GRD_Notificaciones_Enviadas_Manualmente
        '
        Me.GRD_Notificaciones_Enviadas_Manualmente.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Notificaciones_Enviadas_Manualmente.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Notificaciones_Enviadas_Manualmente.Name = "GRD_Notificaciones_Enviadas_Manualmente"
        Me.GRD_Notificaciones_Enviadas_Manualmente.pAccessibleName = Nothing
        Me.GRD_Notificaciones_Enviadas_Manualmente.pActivarBotonFiltro = False
        Me.GRD_Notificaciones_Enviadas_Manualmente.pText = " "
        Me.GRD_Notificaciones_Enviadas_Manualmente.Size = New System.Drawing.Size(958, 522)
        Me.GRD_Notificaciones_Enviadas_Manualmente.TabIndex = 134
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Controls.Add(Me.GRD_Notificaciones_Enviadas_Automaticamente)
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(958, 522)
        '
        'GRD_Notificaciones_Enviadas_Automaticamente
        '
        Me.GRD_Notificaciones_Enviadas_Automaticamente.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Notificaciones_Enviadas_Automaticamente.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Notificaciones_Enviadas_Automaticamente.Name = "GRD_Notificaciones_Enviadas_Automaticamente"
        Me.GRD_Notificaciones_Enviadas_Automaticamente.pAccessibleName = Nothing
        Me.GRD_Notificaciones_Enviadas_Automaticamente.pActivarBotonFiltro = False
        Me.GRD_Notificaciones_Enviadas_Automaticamente.pText = " "
        Me.GRD_Notificaciones_Enviadas_Automaticamente.Size = New System.Drawing.Size(958, 522)
        Me.GRD_Notificaciones_Enviadas_Automaticamente.TabIndex = 133
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.TABStrip_Recibidas)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(962, 560)
        '
        'TABStrip_Recibidas
        '
        Me.TABStrip_Recibidas.Controls.Add(Me.UltraTabSharedControlsPage2)
        Me.TABStrip_Recibidas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TABStrip_Recibidas.Location = New System.Drawing.Point(0, 0)
        Me.TABStrip_Recibidas.Name = "TABStrip_Recibidas"
        Me.TABStrip_Recibidas.SharedControls.AddRange(New System.Windows.Forms.Control() {Me.GRD_Notificaciones_Recibidas})
        Me.TABStrip_Recibidas.SharedControlsPage = Me.UltraTabSharedControlsPage2
        Me.TABStrip_Recibidas.Size = New System.Drawing.Size(962, 560)
        Me.TABStrip_Recibidas.TabIndex = 133
        UltraTab3.Key = "Pendientes"
        UltraTab3.Text = "Pendientes"
        UltraTab4.Key = "Finalizadas"
        UltraTab4.Text = "Finalizadas"
        Me.TABStrip_Recibidas.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab3, UltraTab4})
        '
        'UltraTabSharedControlsPage2
        '
        Me.UltraTabSharedControlsPage2.Controls.Add(Me.GRD_Notificaciones_Recibidas)
        Me.UltraTabSharedControlsPage2.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabSharedControlsPage2.Name = "UltraTabSharedControlsPage2"
        Me.UltraTabSharedControlsPage2.Size = New System.Drawing.Size(958, 534)
        '
        'GRD_Notificaciones_Recibidas
        '
        Me.GRD_Notificaciones_Recibidas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Notificaciones_Recibidas.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Notificaciones_Recibidas.Name = "GRD_Notificaciones_Recibidas"
        Me.GRD_Notificaciones_Recibidas.pAccessibleName = Nothing
        Me.GRD_Notificaciones_Recibidas.pActivarBotonFiltro = False
        Me.GRD_Notificaciones_Recibidas.pText = " "
        Me.GRD_Notificaciones_Recibidas.Size = New System.Drawing.Size(958, 534)
        Me.GRD_Notificaciones_Recibidas.TabIndex = 132
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.UltraTabControl1)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(962, 548)
        '
        'UltraTabControl1
        '
        Me.UltraTabControl1.Controls.Add(Me.UltraTabSharedControlsPage3)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabPageControl3)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabPageControl4)
        Me.UltraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UltraTabControl1.Location = New System.Drawing.Point(0, 0)
        Me.UltraTabControl1.Name = "UltraTabControl1"
        Me.UltraTabControl1.SharedControlsPage = Me.UltraTabSharedControlsPage3
        Me.UltraTabControl1.Size = New System.Drawing.Size(962, 548)
        Me.UltraTabControl1.TabIndex = 134
        UltraTab6.Key = "Manualmente"
        UltraTab6.TabPage = Me.UltraTabPageControl4
        UltraTab6.Text = "Manualmente"
        UltraTab5.Key = "Automaticamente"
        UltraTab5.TabPage = Me.UltraTabPageControl3
        UltraTab5.Text = "Aumaticamente"
        Me.UltraTabControl1.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab6, UltraTab5})
        '
        'UltraTabSharedControlsPage3
        '
        Me.UltraTabSharedControlsPage3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage3.Name = "UltraTabSharedControlsPage3"
        Me.UltraTabSharedControlsPage3.Size = New System.Drawing.Size(958, 522)
        '
        'TAB_Principal
        '
        Me.TAB_Principal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TAB_Principal.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl1)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl2)
        Me.TAB_Principal.Location = New System.Drawing.Point(12, 43)
        Me.TAB_Principal.Name = "TAB_Principal"
        Me.TAB_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.TAB_Principal.Size = New System.Drawing.Size(966, 586)
        Me.TAB_Principal.TabIndex = 133
        UltraTab1.Key = "Recibidas"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Recibidas"
        UltraTab2.Key = "Enviadas"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Enviadas"
        Me.TAB_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(962, 560)
        '
        'frmNotificaciones
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(990, 641)
        Me.Controls.Add(Me.TAB_Principal)
        Me.KeyPreview = True
        Me.Name = "frmNotificaciones"
        Me.Text = "Notificaciones"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.TAB_Principal, 0)
        Me.UltraTabPageControl4.ResumeLayout(False)
        Me.UltraTabPageControl3.ResumeLayout(False)
        Me.UltraTabPageControl1.ResumeLayout(False)
        CType(Me.TABStrip_Recibidas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TABStrip_Recibidas.ResumeLayout(False)
        Me.UltraTabSharedControlsPage2.ResumeLayout(False)
        Me.UltraTabPageControl2.ResumeLayout(False)
        CType(Me.UltraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabControl1.ResumeLayout(False)
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_Principal.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GRD_Notificaciones_Recibidas As M_UltraGrid.m_UltraGrid
    Friend WithEvents TAB_Principal As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Notificaciones_Enviadas_Automaticamente As M_UltraGrid.m_UltraGrid
    Friend WithEvents TABStrip_Recibidas As Infragistics.Win.UltraWinTabControl.UltraTabStripControl
    Friend WithEvents UltraTabSharedControlsPage2 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabControl1 As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage3 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl4 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Notificaciones_Enviadas_Manualmente As M_UltraGrid.m_UltraGrid
End Class
