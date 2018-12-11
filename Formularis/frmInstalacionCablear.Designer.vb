<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInstalacionCablear
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
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab3 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.C_FuenteAlimentacion = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.CH_FuenteAlimentacion = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.CH_CajaIntermediaOrigen = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.CH_CajaIntermediaDestino = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.C_CajaIntermedia_Destino = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.C_CajaIntermedia_Origen = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.T_Localizacion = New M_TextEditor.m_TextEditor()
        Me.T_Uso = New M_TextEditor.m_TextEditor()
        Me.C_Cable = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.CH_UtilizarTodosLosPares = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.GRD_Hilos = New M_UltraGrid.m_UltraGrid()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Origen = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl5 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Destino = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.TAB_Principal = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl2.SuspendLayout()
        CType(Me.C_FuenteAlimentacion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CH_FuenteAlimentacion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CH_CajaIntermediaOrigen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CH_CajaIntermediaDestino, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_CajaIntermedia_Destino, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_CajaIntermedia_Origen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Localizacion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Uso, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Cable, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CH_UtilizarTodosLosPares, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl1.SuspendLayout()
        Me.UltraTabPageControl5.SuspendLayout()
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_Principal.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(987, 24)
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.C_FuenteAlimentacion)
        Me.UltraTabPageControl2.Controls.Add(Me.CH_FuenteAlimentacion)
        Me.UltraTabPageControl2.Controls.Add(Me.CH_CajaIntermediaOrigen)
        Me.UltraTabPageControl2.Controls.Add(Me.CH_CajaIntermediaDestino)
        Me.UltraTabPageControl2.Controls.Add(Me.C_CajaIntermedia_Destino)
        Me.UltraTabPageControl2.Controls.Add(Me.C_CajaIntermedia_Origen)
        Me.UltraTabPageControl2.Controls.Add(Me.T_Localizacion)
        Me.UltraTabPageControl2.Controls.Add(Me.T_Uso)
        Me.UltraTabPageControl2.Controls.Add(Me.C_Cable)
        Me.UltraTabPageControl2.Controls.Add(Me.CH_UtilizarTodosLosPares)
        Me.UltraTabPageControl2.Controls.Add(Me.UltraLabel1)
        Me.UltraTabPageControl2.Controls.Add(Me.GRD_Hilos)
        Me.UltraTabPageControl2.Controls.Add(Me.UltraLabel3)
        Me.UltraTabPageControl2.Controls.Add(Me.UltraLabel2)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(983, 561)
        '
        'C_FuenteAlimentacion
        '
        Me.C_FuenteAlimentacion.Location = New System.Drawing.Point(175, 344)
        Me.C_FuenteAlimentacion.Name = "C_FuenteAlimentacion"
        Me.C_FuenteAlimentacion.ReadOnly = True
        Me.C_FuenteAlimentacion.Size = New System.Drawing.Size(189, 21)
        Me.C_FuenteAlimentacion.TabIndex = 3
        Me.C_FuenteAlimentacion.Text = "C_FuenteAlimentacion"
        '
        'CH_FuenteAlimentacion
        '
        Me.CH_FuenteAlimentacion.Location = New System.Drawing.Point(11, 341)
        Me.CH_FuenteAlimentacion.Name = "CH_FuenteAlimentacion"
        Me.CH_FuenteAlimentacion.Size = New System.Drawing.Size(167, 24)
        Me.CH_FuenteAlimentacion.TabIndex = 149
        Me.CH_FuenteAlimentacion.Text = "Origen de la alimentación:"
        '
        'CH_CajaIntermediaOrigen
        '
        Me.CH_CajaIntermediaOrigen.Location = New System.Drawing.Point(11, 376)
        Me.CH_CajaIntermediaOrigen.Name = "CH_CajaIntermediaOrigen"
        Me.CH_CajaIntermediaOrigen.Size = New System.Drawing.Size(136, 24)
        Me.CH_CajaIntermediaOrigen.TabIndex = 135
        Me.CH_CajaIntermediaOrigen.Text = "Caja intermedia origen"
        '
        'CH_CajaIntermediaDestino
        '
        Me.CH_CajaIntermediaDestino.Location = New System.Drawing.Point(11, 407)
        Me.CH_CajaIntermediaDestino.Name = "CH_CajaIntermediaDestino"
        Me.CH_CajaIntermediaDestino.Size = New System.Drawing.Size(144, 24)
        Me.CH_CajaIntermediaDestino.TabIndex = 136
        Me.CH_CajaIntermediaDestino.Text = "Caja intermedia destino"
        '
        'C_CajaIntermedia_Destino
        '
        Me.C_CajaIntermedia_Destino.Location = New System.Drawing.Point(175, 410)
        Me.C_CajaIntermedia_Destino.Name = "C_CajaIntermedia_Destino"
        Me.C_CajaIntermedia_Destino.ReadOnly = True
        Me.C_CajaIntermedia_Destino.Size = New System.Drawing.Size(189, 21)
        Me.C_CajaIntermedia_Destino.TabIndex = 5
        Me.C_CajaIntermedia_Destino.Text = "C_CajaIntermedia_Destino"
        '
        'C_CajaIntermedia_Origen
        '
        Me.C_CajaIntermedia_Origen.Location = New System.Drawing.Point(175, 377)
        Me.C_CajaIntermedia_Origen.Name = "C_CajaIntermedia_Origen"
        Me.C_CajaIntermedia_Origen.ReadOnly = True
        Me.C_CajaIntermedia_Origen.Size = New System.Drawing.Size(189, 21)
        Me.C_CajaIntermedia_Origen.TabIndex = 4
        Me.C_CajaIntermedia_Origen.Text = "C_CajaIntermedia_Origen"
        '
        'T_Localizacion
        '
        Me.T_Localizacion.Location = New System.Drawing.Point(83, 91)
        Me.T_Localizacion.Multiline = True
        Me.T_Localizacion.Name = "T_Localizacion"
        Me.T_Localizacion.Size = New System.Drawing.Size(455, 231)
        Me.T_Localizacion.TabIndex = 2
        Me.T_Localizacion.Text = "T_Localizacion"
        '
        'T_Uso
        '
        Me.T_Uso.Location = New System.Drawing.Point(83, 54)
        Me.T_Uso.Name = "T_Uso"
        Me.T_Uso.Size = New System.Drawing.Size(455, 21)
        Me.T_Uso.TabIndex = 1
        Me.T_Uso.Text = "T_Uso"
        '
        'C_Cable
        '
        Me.C_Cable.Location = New System.Drawing.Point(83, 16)
        Me.C_Cable.Name = "C_Cable"
        Me.C_Cable.ReadOnly = True
        Me.C_Cable.Size = New System.Drawing.Size(455, 21)
        Me.C_Cable.TabIndex = 0
        Me.C_Cable.Text = "C_Cable"
        '
        'CH_UtilizarTodosLosPares
        '
        Me.CH_UtilizarTodosLosPares.Location = New System.Drawing.Point(560, 15)
        Me.CH_UtilizarTodosLosPares.Name = "CH_UtilizarTodosLosPares"
        Me.CH_UtilizarTodosLosPares.Size = New System.Drawing.Size(136, 24)
        Me.CH_UtilizarTodosLosPares.TabIndex = 142
        Me.CH_UtilizarTodosLosPares.Text = "Utilizar todos los hilos"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 58)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(74, 22)
        Me.UltraLabel1.TabIndex = 145
        Me.UltraLabel1.Text = "Uso:"
        '
        'GRD_Hilos
        '
        Me.GRD_Hilos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Hilos.Location = New System.Drawing.Point(560, 36)
        Me.GRD_Hilos.Name = "GRD_Hilos"
        Me.GRD_Hilos.pAccessibleName = Nothing
        Me.GRD_Hilos.pActivarBotonFiltro = False
        Me.GRD_Hilos.pText = " "
        Me.GRD_Hilos.Size = New System.Drawing.Size(406, 286)
        Me.GRD_Hilos.TabIndex = 6
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(12, 90)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(74, 22)
        Me.UltraLabel3.TabIndex = 147
        Me.UltraLabel3.Text = "Instalación:"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(12, 19)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(74, 22)
        Me.UltraLabel2.TabIndex = 143
        Me.UltraLabel2.Text = "Nº de cable:"
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Origen)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(983, 561)
        '
        'GRD_Origen
        '
        Me.GRD_Origen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Origen.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Origen.Name = "GRD_Origen"
        Me.GRD_Origen.pAccessibleName = Nothing
        Me.GRD_Origen.pActivarBotonFiltro = False
        Me.GRD_Origen.pText = " "
        Me.GRD_Origen.Size = New System.Drawing.Size(983, 561)
        Me.GRD_Origen.TabIndex = 133
        '
        'UltraTabPageControl5
        '
        Me.UltraTabPageControl5.Controls.Add(Me.GRD_Destino)
        Me.UltraTabPageControl5.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl5.Name = "UltraTabPageControl5"
        Me.UltraTabPageControl5.Size = New System.Drawing.Size(983, 561)
        '
        'GRD_Destino
        '
        Me.GRD_Destino.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Destino.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Destino.Name = "GRD_Destino"
        Me.GRD_Destino.pAccessibleName = Nothing
        Me.GRD_Destino.pActivarBotonFiltro = False
        Me.GRD_Destino.pText = " "
        Me.GRD_Destino.Size = New System.Drawing.Size(983, 561)
        Me.GRD_Destino.TabIndex = 132
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(959, 328)
        '
        'TAB_Principal
        '
        Me.TAB_Principal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TAB_Principal.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl5)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl1)
        Me.TAB_Principal.Controls.Add(Me.UltraTabPageControl2)
        Me.TAB_Principal.Location = New System.Drawing.Point(12, 43)
        Me.TAB_Principal.Name = "TAB_Principal"
        Me.TAB_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.TAB_Principal.Size = New System.Drawing.Size(987, 587)
        Me.TAB_Principal.TabIndex = 107
        UltraTab1.Key = "Especificaciones"
        UltraTab1.TabPage = Me.UltraTabPageControl2
        UltraTab1.Text = "Especificaciones"
        UltraTab2.Key = "Origen"
        UltraTab2.TabPage = Me.UltraTabPageControl1
        UltraTab2.Text = "Origen"
        UltraTab3.Key = "Destino"
        UltraTab3.TabPage = Me.UltraTabPageControl5
        UltraTab3.Text = "Destino"
        Me.TAB_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2, UltraTab3})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(983, 561)
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
        'frmInstalacionCablear
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 641)
        Me.Controls.Add(Me.TAB_Principal)
        Me.KeyPreview = True
        Me.Name = "frmInstalacionCablear"
        Me.Text = "Asignación del cableado"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.TAB_Principal, 0)
        Me.UltraTabPageControl2.ResumeLayout(False)
        Me.UltraTabPageControl2.PerformLayout()
        CType(Me.C_FuenteAlimentacion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CH_FuenteAlimentacion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CH_CajaIntermediaOrigen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CH_CajaIntermediaDestino, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_CajaIntermedia_Destino, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_CajaIntermedia_Origen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Localizacion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Uso, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Cable, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CH_UtilizarTodosLosPares, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl5.ResumeLayout(False)
        CType(Me.TAB_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_Principal.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TAB_Principal As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl5 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Destino As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Origen As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_CajaIntermedia_Destino As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents C_Cable As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents C_CajaIntermedia_Origen As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents CH_CajaIntermediaDestino As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents CH_CajaIntermediaOrigen As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents GRD_Hilos As M_UltraGrid.m_UltraGrid
    Friend WithEvents CH_UtilizarTodosLosPares As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents T_Uso As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Localizacion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_FuenteAlimentacion As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents CH_FuenteAlimentacion As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
End Class
