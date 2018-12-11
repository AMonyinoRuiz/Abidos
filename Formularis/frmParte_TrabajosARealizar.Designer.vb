<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmParte_TrabajosARealizar
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmParte_TrabajosARealizar))
        Dim UltraTab1 As Infragistics.Win.UltraWinTabControl.UltraTab = New Infragistics.Win.UltraWinTabControl.UltraTab()
        Me.UltraTabPageControl1 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.DT_FechaPrevision = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.GRD_Personal = New M_UltraGrid.m_UltraGrid()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.R_Descripcion_Detallada = New M_RichText.M_RichText()
        Me.T_Orden = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Participantes = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_DuracionAproximada = New M_MaskEditor.m_MaskEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.CH_Finalizado = New Infragistics.Win.UltraWinEditors.UltraCheckEditor()
        Me.DT_Alta = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel28 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_NumDia = New M_MaskEditor.m_MaskEditor()
        Me.L_klkl = New Infragistics.Win.Misc.UltraLabel()
        Me.T_DescripcionBreve = New M_TextEditor.m_TextEditor()
        Me.UltraLabel16 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Codigo = New M_TextEditor.m_TextEditor()
        Me.TE_TrabajoARealizarPrimero = New M_TextEditor.m_TextEditor()
        Me.L_BLABLLAB = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.Tab_Principal = New Infragistics.Win.UltraWinTabControl.UltraTabControl()
        Me.UltraTabSharedControlsPage2 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.GRD_Productos = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl1.SuspendLayout()
        CType(Me.DT_FechaPrevision, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CH_Finalizado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_DescripcionBreve, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Codigo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TE_TrabajoARealizarPrimero, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Tab_Principal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Tab_Principal.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(1139, 24)
        Me.ToolForm.TabIndex = 29
        '
        'UltraTabPageControl1
        '
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Productos)
        Me.UltraTabPageControl1.Controls.Add(Me.DT_FechaPrevision)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel6)
        Me.UltraTabPageControl1.Controls.Add(Me.GRD_Personal)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel5)
        Me.UltraTabPageControl1.Controls.Add(Me.R_Descripcion_Detallada)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Orden)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel3)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Participantes)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel2)
        Me.UltraTabPageControl1.Controls.Add(Me.T_DuracionAproximada)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel1)
        Me.UltraTabPageControl1.Controls.Add(Me.CH_Finalizado)
        Me.UltraTabPageControl1.Controls.Add(Me.DT_Alta)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel28)
        Me.UltraTabPageControl1.Controls.Add(Me.T_NumDia)
        Me.UltraTabPageControl1.Controls.Add(Me.L_klkl)
        Me.UltraTabPageControl1.Controls.Add(Me.T_DescripcionBreve)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel16)
        Me.UltraTabPageControl1.Controls.Add(Me.T_Codigo)
        Me.UltraTabPageControl1.Controls.Add(Me.TE_TrabajoARealizarPrimero)
        Me.UltraTabPageControl1.Controls.Add(Me.L_BLABLLAB)
        Me.UltraTabPageControl1.Controls.Add(Me.UltraLabel4)
        Me.UltraTabPageControl1.Location = New System.Drawing.Point(1, 23)
        Me.UltraTabPageControl1.Name = "UltraTabPageControl1"
        Me.UltraTabPageControl1.Size = New System.Drawing.Size(1135, 442)
        '
        'DT_FechaPrevision
        '
        Me.DT_FechaPrevision.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_FechaPrevision.Location = New System.Drawing.Point(206, 31)
        Me.DT_FechaPrevision.Name = "DT_FechaPrevision"
        Me.DT_FechaPrevision.Size = New System.Drawing.Size(90, 21)
        Me.DT_FechaPrevision.TabIndex = 157
        Me.DT_FechaPrevision.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(207, 15)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(89, 16)
        Me.UltraLabel6.TabIndex = 158
        Me.UltraLabel6.Text = "Fecha previsión:"
        '
        'GRD_Personal
        '
        Me.GRD_Personal.Location = New System.Drawing.Point(12, 113)
        Me.GRD_Personal.Name = "GRD_Personal"
        Me.GRD_Personal.pAccessibleName = Nothing
        Me.GRD_Personal.pActivarBotonFiltro = False
        Me.GRD_Personal.pText = " "
        Me.GRD_Personal.Size = New System.Drawing.Size(954, 171)
        Me.GRD_Personal.TabIndex = 18
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(12, 295)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(142, 16)
        Me.UltraLabel5.TabIndex = 156
        Me.UltraLabel5.Text = "Descripción detallada:"
        '
        'R_Descripcion_Detallada
        '
        Me.R_Descripcion_Detallada.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.R_Descripcion_Detallada.Location = New System.Drawing.Point(12, 314)
        Me.R_Descripcion_Detallada.Name = "R_Descripcion_Detallada"
        Me.R_Descripcion_Detallada.pEnable = True
        Me.R_Descripcion_Detallada.pText = resources.GetString("R_Descripcion_Detallada.pText")
        Me.R_Descripcion_Detallada.pTextEspecial = "R_Descripcion_Detallada"
        Me.R_Descripcion_Detallada.Size = New System.Drawing.Size(1106, 115)
        Me.R_Descripcion_Detallada.TabIndex = 155
        '
        'T_Orden
        '
        Me.T_Orden.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_Orden.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Integer]
        Me.T_Orden.InputMask = "nnnn"
        Me.T_Orden.Location = New System.Drawing.Point(365, 81)
        Me.T_Orden.Name = "T_Orden"
        Me.T_Orden.NonAutoSizeHeight = 20
        Me.T_Orden.Size = New System.Drawing.Size(58, 20)
        Me.T_Orden.TabIndex = 153
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(365, 65)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(58, 16)
        Me.UltraLabel3.TabIndex = 154
        Me.UltraLabel3.Text = "Orden:"
        '
        'T_Participantes
        '
        Me.T_Participantes.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_Participantes.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Integer]
        Me.T_Participantes.InputMask = "nnnn"
        Me.T_Participantes.Location = New System.Drawing.Point(263, 81)
        Me.T_Participantes.Name = "T_Participantes"
        Me.T_Participantes.NonAutoSizeHeight = 20
        Me.T_Participantes.Size = New System.Drawing.Size(84, 20)
        Me.T_Participantes.TabIndex = 151
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(263, 65)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(84, 16)
        Me.UltraLabel2.TabIndex = 152
        Me.UltraLabel2.Text = "*Participantes:"
        '
        'T_DuracionAproximada
        '
        Me.T_DuracionAproximada.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_DuracionAproximada.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Double]
        Me.T_DuracionAproximada.InputMask = "{double:4.2}"
        Me.T_DuracionAproximada.Location = New System.Drawing.Point(115, 81)
        Me.T_DuracionAproximada.Name = "T_DuracionAproximada"
        Me.T_DuracionAproximada.NonAutoSizeHeight = 20
        Me.T_DuracionAproximada.Size = New System.Drawing.Size(129, 20)
        Me.T_DuracionAproximada.TabIndex = 149
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(115, 65)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(129, 16)
        Me.UltraLabel1.TabIndex = 150
        Me.UltraLabel1.Text = "Duración aprox. (horas):"
        '
        'CH_Finalizado
        '
        Me.CH_Finalizado.Location = New System.Drawing.Point(881, 34)
        Me.CH_Finalizado.Name = "CH_Finalizado"
        Me.CH_Finalizado.Size = New System.Drawing.Size(85, 17)
        Me.CH_Finalizado.TabIndex = 9
        Me.CH_Finalizado.Text = "Finalizado"
        '
        'DT_Alta
        '
        Me.DT_Alta.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Alta.Location = New System.Drawing.Point(101, 30)
        Me.DT_Alta.Name = "DT_Alta"
        Me.DT_Alta.ReadOnly = True
        Me.DT_Alta.Size = New System.Drawing.Size(90, 21)
        Me.DT_Alta.TabIndex = 2
        Me.DT_Alta.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel28
        '
        Me.UltraLabel28.Location = New System.Drawing.Point(102, 14)
        Me.UltraLabel28.Name = "UltraLabel28"
        Me.UltraLabel28.Size = New System.Drawing.Size(89, 16)
        Me.UltraLabel28.TabIndex = 142
        Me.UltraLabel28.Text = "*Fecha alta:"
        '
        'T_NumDia
        '
        Me.T_NumDia.DataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw
        Me.T_NumDia.EditAs = Infragistics.Win.UltraWinMaskedEdit.EditAsType.[Integer]
        Me.T_NumDia.InputMask = "nnnn"
        Me.T_NumDia.Location = New System.Drawing.Point(12, 81)
        Me.T_NumDia.Name = "T_NumDia"
        Me.T_NumDia.NonAutoSizeHeight = 20
        Me.T_NumDia.Size = New System.Drawing.Size(85, 20)
        Me.T_NumDia.TabIndex = 3
        '
        'L_klkl
        '
        Me.L_klkl.Location = New System.Drawing.Point(12, 65)
        Me.L_klkl.Name = "L_klkl"
        Me.L_klkl.Size = New System.Drawing.Size(97, 16)
        Me.L_klkl.TabIndex = 17
        Me.L_klkl.Text = "*Número de día:"
        '
        'T_DescripcionBreve
        '
        Me.T_DescripcionBreve.Location = New System.Drawing.Point(309, 30)
        Me.T_DescripcionBreve.Name = "T_DescripcionBreve"
        Me.T_DescripcionBreve.Size = New System.Drawing.Size(557, 21)
        Me.T_DescripcionBreve.TabIndex = 6
        Me.T_DescripcionBreve.Text = "T_DescripcionBreve"
        '
        'UltraLabel16
        '
        Me.UltraLabel16.Location = New System.Drawing.Point(309, 14)
        Me.UltraLabel16.Name = "UltraLabel16"
        Me.UltraLabel16.Size = New System.Drawing.Size(121, 16)
        Me.UltraLabel16.TabIndex = 41
        Me.UltraLabel16.Text = "Descripción breve:"
        '
        'T_Codigo
        '
        Me.T_Codigo.Location = New System.Drawing.Point(12, 30)
        Me.T_Codigo.Name = "T_Codigo"
        Me.T_Codigo.ReadOnly = True
        Me.T_Codigo.Size = New System.Drawing.Size(74, 21)
        Me.T_Codigo.TabIndex = 5
        Me.T_Codigo.Text = "T_Codigo"
        '
        'TE_TrabajoARealizarPrimero
        '
        Me.TE_TrabajoARealizarPrimero.Location = New System.Drawing.Point(441, 80)
        Me.TE_TrabajoARealizarPrimero.Name = "TE_TrabajoARealizarPrimero"
        Me.TE_TrabajoARealizarPrimero.ReadOnly = True
        Me.TE_TrabajoARealizarPrimero.Size = New System.Drawing.Size(525, 21)
        Me.TE_TrabajoARealizarPrimero.TabIndex = 0
        Me.TE_TrabajoARealizarPrimero.Text = "TE_TrabajoARealizarPrimero"
        '
        'L_BLABLLAB
        '
        Me.L_BLABLLAB.Location = New System.Drawing.Point(12, 15)
        Me.L_BLABLLAB.Name = "L_BLABLLAB"
        Me.L_BLABLLAB.Size = New System.Drawing.Size(74, 16)
        Me.L_BLABLLAB.TabIndex = 21
        Me.L_BLABLLAB.Text = "Código:"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(441, 65)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(177, 16)
        Me.UltraLabel4.TabIndex = 16
        Me.UltraLabel4.Text = "Trabajo a realizar primero:"
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
        'UltraTabSharedControlsPage1
        '
        Me.UltraTabSharedControlsPage1.Location = New System.Drawing.Point(1, 20)
        Me.UltraTabSharedControlsPage1.Name = "UltraTabSharedControlsPage1"
        Me.UltraTabSharedControlsPage1.Size = New System.Drawing.Size(694, 303)
        '
        'Tab_Principal
        '
        Me.Tab_Principal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tab_Principal.Controls.Add(Me.UltraTabSharedControlsPage2)
        Me.Tab_Principal.Controls.Add(Me.UltraTabPageControl1)
        Me.Tab_Principal.Location = New System.Drawing.Point(12, 43)
        Me.Tab_Principal.Name = "Tab_Principal"
        Me.Tab_Principal.SharedControlsPage = Me.UltraTabSharedControlsPage2
        Me.Tab_Principal.Size = New System.Drawing.Size(1139, 468)
        Me.Tab_Principal.TabIndex = 32
        UltraTab1.Key = "TrabajosARealizar"
        UltraTab1.TabPage = Me.UltraTabPageControl1
        UltraTab1.Text = "Trabajos a realizar"
        Me.Tab_Principal.Tabs.AddRange(New Infragistics.Win.UltraWinTabControl.UltraTab() {UltraTab1})
        '
        'UltraTabSharedControlsPage2
        '
        Me.UltraTabSharedControlsPage2.Location = New System.Drawing.Point(-10000, -10000)
        Me.UltraTabSharedControlsPage2.Name = "UltraTabSharedControlsPage2"
        Me.UltraTabSharedControlsPage2.Size = New System.Drawing.Size(1135, 442)
        '
        'GRD_Productos
        '
        Me.GRD_Productos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Productos.Location = New System.Drawing.Point(991, 30)
        Me.GRD_Productos.Name = "GRD_Productos"
        Me.GRD_Productos.pAccessibleName = Nothing
        Me.GRD_Productos.pActivarBotonFiltro = False
        Me.GRD_Productos.pText = " "
        Me.GRD_Productos.Size = New System.Drawing.Size(134, 254)
        Me.GRD_Productos.TabIndex = 159
        '
        'frmParte_TrabajosARealizar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1163, 523)
        Me.Controls.Add(Me.Tab_Principal)
        Me.KeyPreview = True
        Me.Name = "frmParte_TrabajosARealizar"
        Me.Text = "Trabajos a realizar"
        Me.Controls.SetChildIndex(Me.Tab_Principal, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.UltraTabPageControl1.ResumeLayout(False)
        Me.UltraTabPageControl1.PerformLayout()
        CType(Me.DT_FechaPrevision, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CH_Finalizado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_DescripcionBreve, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Codigo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TE_TrabajoARealizarPrimero, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Tab_Principal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Tab_Principal.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents L_klkl As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents L_BLABLLAB As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_NumDia As M_MaskEditor.m_MaskEditor
    Friend WithEvents Tab_Principal As Infragistics.Win.UltraWinTabControl.UltraTabControl
    Friend WithEvents UltraTabSharedControlsPage2 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraTabPageControl1 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents UltraLabel16 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents CH_Finalizado As Infragistics.Win.UltraWinEditors.UltraCheckEditor
    Friend WithEvents DT_Alta As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel28 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents GRD_Personal As M_UltraGrid.m_UltraGrid
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents R_Descripcion_Detallada As M_RichText.M_RichText
    Friend WithEvents T_Orden As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Participantes As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_DuracionAproximada As M_MaskEditor.m_MaskEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents DT_FechaPrevision As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents GRD_Productos As M_UltraGrid.m_UltraGrid
    Friend WithEvents TE_TrabajoARealizarPrimero As M_TextEditor.m_TextEditor
    Friend WithEvents T_Codigo As M_TextEditor.m_TextEditor
    Friend WithEvents T_DescripcionBreve As M_TextEditor.m_TextEditor
End Class
