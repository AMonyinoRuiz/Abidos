<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAutomatismo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAutomatismo))
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab2 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Dim UltraTab3 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Acciones = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl2 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.R_Explicacion = New M_RichText.M_RichText()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.GRD_Personal = New M_UltraGrid.m_UltraGrid()
        Me.TE_Codigo = New M_MaskEditor.m_MaskEditor()
        Me.T_Descripcion = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.C_TipoActividad = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.C_Prioridad = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.UltraTabPageControl1.SuspendLayout()
        Me.UltraTabPageControl2.SuspendLayout()
        Me.UltraTabPageControl3.SuspendLayout()
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraTabControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraTabControl1.SuspendLayout()
        CType(Me.C_TipoActividad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.C_Prioridad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.Location = New System.Drawing.Point(12, 12)
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(987, 24)
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Acciones)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(983, 507)
        '
        'GRD_Acciones
        '
        Me.GRD_Acciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Acciones.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Acciones.Name = "GRD_Acciones"
        Me.GRD_Acciones.pAccessibleName = Nothing
        Me.GRD_Acciones.pActivarBotonFiltro = False
        Me.GRD_Acciones.pText = " "
        Me.GRD_Acciones.Size = New System.Drawing.Size(983, 507)
        Me.GRD_Acciones.TabIndex = 131
        '
        'UltraTabPageControl2
        '
        Me.UltraTabPageControl2.Controls.Add(Me.R_Explicacion)
        Me.UltraTabPageControl2.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl2.Name = "UltraTabPageControl2"
        Me.UltraTabPageControl2.Size = New System.Drawing.Size(983, 507)
        '
        'R_Explicacion
        '
        Me.R_Explicacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.R_Explicacion.Location = New System.Drawing.Point(0, 0)
        Me.R_Explicacion.Name = "R_Explicacion"
        Me.R_Explicacion.pEnable = True
        Me.R_Explicacion.pText = resources.GetString("R_Explicacion.pText")
        Me.R_Explicacion.pTextEspecial = "R_Explicacion"
        Me.R_Explicacion.Size = New System.Drawing.Size(983, 507)
        Me.R_Explicacion.TabIndex = 0
        '
        'UltraTabPageControl3
        '
        Me.UltraTabPageControl3.Controls.Add(Me.GRD_Personal)
        Me.UltraTabPageControl3.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabPageControl3.Name = "UltraTabPageControl3"
        Me.UltraTabPageControl3.Size = New System.Drawing.Size(983, 507)
        '
        'GRD_Personal
        '
        Me.GRD_Personal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GRD_Personal.Location = New System.Drawing.Point(0, 0)
        Me.GRD_Personal.Name = "GRD_Personal"
        Me.GRD_Personal.pAccessibleName = Nothing
        Me.GRD_Personal.pActivarBotonFiltro = False
        Me.GRD_Personal.pText = " "
        Me.GRD_Personal.Size = New System.Drawing.Size(983, 507)
        Me.GRD_Personal.TabIndex = 132
        '
        'TE_Codigo
        '
        Me.TE_Codigo.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.TE_Codigo.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Long]
        Me.TE_Codigo.InputMask = "nnnnnnnnnn"
        Me.TE_Codigo.Location = New System.Drawing.Point(12, 63)
        Me.TE_Codigo.Name = "TE_Codigo"
        Me.TE_Codigo.NonAutoSizeHeight = 20
        Me.TE_Codigo.ReadOnly = True
        Me.TE_Codigo.Size = New System.Drawing.Size(120, 20)
        Me.TE_Codigo.TabIndex = 132
        '
        'T_Descripcion
        '
        Me.T_Descripcion.Location = New System.Drawing.Point(150, 63)
        Me.T_Descripcion.Name = "T_Descripcion"
        Me.T_Descripcion.ReadOnly = True
        Me.T_Descripcion.Size = New System.Drawing.Size(424, 21)
        Me.T_Descripcion.TabIndex = 133
        Me.T_Descripcion.Text = "T_Descripcion"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(15, 48)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel1.TabIndex = 134
        Me.UltraLabel1.Text = "*Código:"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(150, 48)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 135
        Me.UltraLabel5.Text = "*Descripción"
        '
        'UltraTabControl1
        '
        Me.UltraTabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabSharedControlsPage1)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabPageControl1)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabPageControl2)
        Me.UltraTabControl1.Controls.Add(Me.UltraTabPageControl3)
        Me.UltraTabControl1.Location = New System.Drawing.Point(12, 96)
        Me.UltraTabControl1.Name = "UltraTabControl1"
        Me.UltraTabControl1.SharedControlsPage = Me.UltraTabSharedControlsPage1
        Me.UltraTabControl1.Size = New System.Drawing.Size(987, 533)
        Me.UltraTabControl1.TabIndex = 136
        UltraTab1.Key = "Acciones"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Acciones"
        UltraTab2.Key = "Explicacion"
        UltraTab2.TabPage = Me.UltraTabPageControl2
        UltraTab2.Text = "Explicación"
        UltraTab3.Key = "PersonalAsignado"
        UltraTab3.TabPage = Me.UltraTabPageControl3
        UltraTab3.Text = "Personal asignado"
        Me.UltraTabControl1.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1, UltraTab2, UltraTab3})
        '
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(983, 507)
        '
        'C_TipoActividad
        '
        Me.C_TipoActividad.Location = New System.Drawing.Point(589, 62)
        Me.C_TipoActividad.Name = "C_TipoActividad"
        Me.C_TipoActividad.Size = New System.Drawing.Size(196, 21)
        Me.C_TipoActividad.TabIndex = 237
        Me.C_TipoActividad.Text = "C_TipoActividad"
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(590, 48)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(176, 16)
        Me.UltraLabel6.TabIndex = 236
        Me.UltraLabel6.Text = "*Tipo de actividad:"
        '
        'C_Prioridad
        '
        Me.C_Prioridad.Location = New System.Drawing.Point(800, 62)
        Me.C_Prioridad.Name = "C_Prioridad"
        Me.C_Prioridad.Size = New System.Drawing.Size(196, 21)
        Me.C_Prioridad.TabIndex = 239
        Me.C_Prioridad.Text = "C_Prioridad"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(801, 48)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(176, 16)
        Me.UltraLabel2.TabIndex = 238
        Me.UltraLabel2.Text = "*Prioridad:"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'frmAutomatismo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 641)
        Me.Controls.Add(Me.C_Prioridad)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.C_TipoActividad)
        Me.Controls.Add(Me.UltraLabel6)
        Me.Controls.Add(Me.UltraTabControl1)
        Me.Controls.Add(Me.TE_Codigo)
        Me.Controls.Add(Me.T_Descripcion)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel5)
        Me.KeyPreview = True
        Me.Name = "frmAutomatismo"
        Me.Text = "Automatismo de actividades"
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.TE_Codigo, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraTabControl1, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel6, 0)
        Me.Controls.SetChildIndex(Me.C_TipoActividad, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel2, 0)
        Me.Controls.SetChildIndex(Me.C_Prioridad, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl2.ResumeLayout(False)
        Me.UltraTabPageControl3.ResumeLayout(False)
        CType(Me.T_Descripcion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraTabControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraTabControl1.ResumeLayout(False)
        CType(Me.C_TipoActividad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.C_Prioridad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GRD_Acciones As M_UltraGrid.m_UltraGrid
    Friend WithEvents TE_Codigo As M_MaskEditor.m_MaskEditor
    Friend WithEvents T_Descripcion As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTabControl1 As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl2 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents R_Explicacion As M_RichText.M_RichText
    Friend WithEvents C_TipoActividad As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents C_Prioridad As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents GRD_Personal As M_UltraGrid.m_UltraGrid
End Class
