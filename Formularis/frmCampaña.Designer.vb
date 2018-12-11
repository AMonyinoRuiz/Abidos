<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCampaña
    Inherits M_GenericForm.frmBase 'System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCampaña))
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab4 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab3 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Clientes = New M_UltraGrid.m_UltraGrid()
        Me.GRD_Seleccion = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl4 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.B_Rendimiento_Buscar = New Infragistics.Win.Misc.UltraButton()
        Me.DT_Rendimiento_Fin = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.DT_Rendimiento_Inicio = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.GRD_Rendimiento = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Usuario = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.R_Observaciones = New M_RichText.M_RichText()
        Me.TE_Codigo = New M_MaskEditor.m_MaskEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.DT_Data = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel13 = New Infragistics.Win.Misc.UltraLabel()
        Me.TAB_General = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.C_Estado = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.L_Estado = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl1.SuspendLayout()
        Me.UltraTabPageControl4.SuspendLayout()
        CType(Me.DT_Rendimiento_Fin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Rendimiento_Inicio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabPageControl2.SuspendLayout()
        Me.UltraTabPageControl3.SuspendLayout()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Data, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TAB_General, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TAB_General.SuspendLayout()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Estado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Size = New System.Drawing.Size(984, 24)
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Clientes)
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Seleccion)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(966, 387)
        '
        'GRD_Clientes
        '
        Me.GRD_Clientes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Clientes.Location = New System.Drawing.Point(3, 3)
        Me.GRD_Clientes.Name = "GRD_Clientes"
        Me.GRD_Clientes.pAccessibleName = Nothing
        Me.GRD_Clientes.pActivarBotonFiltro = False
        Me.GRD_Clientes.pText = " "
        Me.GRD_Clientes.Size = New System.Drawing.Size(960, 303)
        Me.GRD_Clientes.TabIndex = 144
        '
        'GRD_Seleccion
        '
        Me.GRD_Seleccion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Seleccion.Location = New System.Drawing.Point(3, 312)
        Me.GRD_Seleccion.Name = "GRD_Seleccion"
        Me.GRD_Seleccion.pAccessibleName = Nothing
        Me.GRD_Seleccion.pActivarBotonFiltro = False
        Me.GRD_Seleccion.pText = " "
        Me.GRD_Seleccion.Size = New System.Drawing.Size(960, 72)
        Me.GRD_Seleccion.TabIndex = 139
        '
        'UltraTabPageControl4
        '
        Me.UltraTabPageControl4.Controls.Add(Me.B_Rendimiento_Buscar)
        Me.UltraTabPageControl4.Controls.Add(Me.DT_Rendimiento_Fin)
        Me.UltraTabPageControl4.Controls.Add(Me.UltraLabel3)
        Me.UltraTabPageControl4.Controls.Add(Me.DT_Rendimiento_Inicio)
        Me.UltraTabPageControl4.Controls.Add(Me.UltraLabel2)
        Me.UltraTabPageControl4.Controls.Add(Me.GRD_Rendimiento)
        Me.UltraTabPageControl4.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl4.Name = "UltraTabPageControl4"
        Me.UltraTabPageControl4.Size = New System.Drawing.Size(966, 387)
        '
        'B_Rendimiento_Buscar
        '
        Me.B_Rendimiento_Buscar.Location = New System.Drawing.Point(244, 21)
        Me.B_Rendimiento_Buscar.Name = "B_Rendimiento_Buscar"
        Me.B_Rendimiento_Buscar.Size = New System.Drawing.Size(91, 21)
        Me.B_Rendimiento_Buscar.TabIndex = 2
        Me.B_Rendimiento_Buscar.Text = "Buscar"
        '
        'DT_Rendimiento_Fin
        '
        Me.DT_Rendimiento_Fin.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Rendimiento_Fin.Location = New System.Drawing.Point(121, 21)
        Me.DT_Rendimiento_Fin.Name = "DT_Rendimiento_Fin"
        Me.DT_Rendimiento_Fin.Size = New System.Drawing.Size(102, 21)
        Me.DT_Rendimiento_Fin.TabIndex = 1
        Me.DT_Rendimiento_Fin.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(121, 5)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(102, 16)
        Me.UltraLabel3.TabIndex = 155
        Me.UltraLabel3.Text = "Fecha fin:"
        '
        'DT_Rendimiento_Inicio
        '
        Me.DT_Rendimiento_Inicio.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Rendimiento_Inicio.Location = New System.Drawing.Point(3, 21)
        Me.DT_Rendimiento_Inicio.Name = "DT_Rendimiento_Inicio"
        Me.DT_Rendimiento_Inicio.Size = New System.Drawing.Size(102, 21)
        Me.DT_Rendimiento_Inicio.TabIndex = 0
        Me.DT_Rendimiento_Inicio.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(3, 5)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(102, 16)
        Me.UltraLabel2.TabIndex = 153
        Me.UltraLabel2.Text = "Fecha inicio:"
        '
        'GRD_Rendimiento
        '
        Me.GRD_Rendimiento.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Rendimiento.Location = New System.Drawing.Point(3, 57)
        Me.GRD_Rendimiento.Name = "GRD_Rendimiento"
        Me.GRD_Rendimiento.pAccessibleName = Nothing
        Me.GRD_Rendimiento.pActivarBotonFiltro = False
        Me.GRD_Rendimiento.pText = " "
        Me.GRD_Rendimiento.Size = New System.Drawing.Size(1123, 335)
        Me.GRD_Rendimiento.TabIndex = 145
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.GRD_Usuario)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(966, 387)
        '
        'GRD_Usuario
        '
        Me.GRD_Usuario.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Usuario.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Usuario.Name = "GRD_Usuario"
        Me.GRD_Usuario.pAccessibleName = Nothing
        Me.GRD_Usuario.pActivarBotonFiltro = False
        Me.GRD_Usuario.pText = " "
        Me.GRD_Usuario.Size = New System.Drawing.Size(966, 387)
        Me.GRD_Usuario.TabIndex = 145
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Controls.Add(Me.R_Observaciones)
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(966, 387)
        '
        'R_Observaciones
        '
        Me.R_Observaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.R_Observaciones.Location = New System.Drawing.Point(0, 0)
        Me.R_Observaciones.Name = "R_Observaciones"
        Me.R_Observaciones.pEnable = True
        Me.R_Observaciones.pText = resources.GetString("R_Observaciones.pText")
        Me.R_Observaciones.pTextEspecial = ""
        Me.R_Observaciones.Size = New System.Drawing.Size(966, 387)
        Me.R_Observaciones.TabIndex = 3
        '
        'TE_Codigo
        '
        Me.TE_Codigo.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.TE_Codigo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.TE_Codigo.InputMask = "nnnnnnnnnn"
        Me.TE_Codigo.Location = New System.Drawing.Point(12, 60)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.NonAutoSizeHeight = 20
        Me.TE_Codigo.Size = New System.Drawing.Size(120, 20)
        Me.TE_Codigo.TabIndex = 0
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(149, 59)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.Size = New System.Drawing.Size(590, 21)
        Me.T_Descripcion.TabIndex = 1
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel1.TabIndex = 142
        Me.UltraLabel1.Text = "*Código:"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(149, 43)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 143
        Me.UltraLabel5.Text = "*Descripción"
        '
        'DT_Data
        '
        Me.DT_Data.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Data.Location = New System.Drawing.Point(745, 59)
        Me.DT_Data.Name = "DT_Data"
        Me.DT_Data.Size = New System.Drawing.Size(102, 21)
        Me.DT_Data.TabIndex = 2
        Me.DT_Data.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel13
        '
        Me.UltraLabel13.Location = New System.Drawing.Point(745, 42)
        Me.UltraLabel13.Name = "UltraLabel13"
        Me.UltraLabel13.Size = New System.Drawing.Size(116, 16)
        Me.UltraLabel13.TabIndex = 151
        Me.UltraLabel13.Text = "Fecha de campaña:"
        '
        'TAB_General
        '
        Me.TAB_General.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TAB_General.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl1)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl2)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl3)
        Me.TAB_General.Controls.Add(Me.UltraTabPageControl4)
        Me.TAB_General.Location = New System.Drawing.Point(12, 99)
        Me.TAB_General.Name = "TAB_General"
        Me.TAB_General.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.TAB_General.Size = New System.Drawing.Size(970, 413)
        Me.TAB_General.TabIndex = 152
        UltraTab1.Key = "Seleccion"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Selección"
        UltraTab4.Key = "Rendimiento"
        UltraTab4.TabPage = Me.UltraTabPageControl4
        UltraTab4.Text = "Rendimiento"
        UltraTab2.Key = "Usuarios"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Usuarios"
        UltraTab3.Key = "Observaciones"
        UltraTab3.TabPage = Me.UltraTabPageControl3
        UltraTab3.Text = "Observaciones"
        Me.TAB_General.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab4, UltraTab2, UltraTab3})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(966, 387)
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'C_Estado
        '
        Me.C_Estado.Location = New System.Drawing.Point(863, 59)
        Me.C_Estado.Name = "C_Estado"
        Me.C_Estado.Size = New System.Drawing.Size(133, 21)
        Me.C_Estado.TabIndex = 3
        Me.C_Estado.Text = "C_Estado"
        '
        'L_Estado
        '
        Me.L_Estado.Location = New System.Drawing.Point(863, 43)
        Me.L_Estado.Name = "L_Estado"
        Me.L_Estado.Size = New System.Drawing.Size(112, 16)
        Me.L_Estado.TabIndex = 219
        Me.L_Estado.Text = "Estado:"
        '
        'frmCampaña
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 555)
        Me.Controls.Add(Me.C_Estado)
        Me.Controls.Add(Me.L_Estado)
        Me.Controls.Add(Me.TAB_General)
        Me.Controls.Add(Me.DT_Data)
        Me.Controls.Add(Me.UltraLabel13)
        Me.Controls.Add(Me.TE_Codigo)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel5)
        Me.KeyPreview = True
        Me.Name = "frmCampaña"
        Me.Text = "Campaña"
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.TE_Codigo, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel13, 0)
        Me.Controls.SetChildIndex(Me.DT_Data, 0)
        Me.Controls.SetChildIndex(Me.TAB_General, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.L_Estado, 0)
        Me.Controls.SetChildIndex(Me.C_Estado, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl4.ResumeLayout(False)
        Me.UltraTabPageControl4.PerformLayout()
        CType(Me.DT_Rendimiento_Fin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Rendimiento_Inicio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabPageControl2.ResumeLayout(False)
        Me.UltraTabPageControl3.ResumeLayout(False)
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Data, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TAB_General, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TAB_General.ResumeLayout(False)
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Estado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TE_Codigo As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents GRD_Seleccion As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Clientes As M_UltraGrid.m_UltraGrid
    Friend WithEvents DT_Data As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel13 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TAB_General As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Usuario As M_UltraGrid.m_UltraGrid
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents R_Observaciones As M_RichText.M_RichText
    Friend WithEvents C_Estado As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents L_Estado As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTabPageControl4 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Rendimiento As M_UltraGrid.m_UltraGrid
    Friend WithEvents B_Rendimiento_Buscar As Infragistics.Win.Misc.UltraButton
    Friend WithEvents DT_Rendimiento_Fin As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents DT_Rendimiento_Inicio As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
End Class
