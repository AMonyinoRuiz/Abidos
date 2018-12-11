<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEntrada_Linea_CompuestoPor
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
        Dim StateEditorButton1 As Infragistics.Win.UltraWinEditors.StateEditorButton = New Infragistics.Win.UltraWinEditors.StateEditorButton("Todos")
        Me.GRD_Lineas = New M_UltraGrid.m_UltraGrid()
        Me.UltraTabPageControl3 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl10 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabPageControl11 = New Infragistics.Win.UltraWinTabControl.UltraTabPageControl()
        Me.UltraTabSharedControlsPage1 = New Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage()
        Me.C_Almacen = New Infragistics.Win.UltraWinEditors.UltraComboEditor()
        Me.UltraLabel29 = New Infragistics.Win.Misc.UltraLabel()
        Me.B_CarregarNS = New Infragistics.Win.Misc.UltraButton()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.C_Almacen, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        Me.ToolForm.Size = New System.Drawing.Size(986, 24)
        Me.ToolForm.TabIndex = 29
        '
        'GRD_Lineas
        '
        Me.GRD_Lineas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Lineas.Location = New System.Drawing.Point(12, 94)
        Me.GRD_Lineas.Name = "GRD_Lineas"
        Me.GRD_Lineas.pAccessibleName = Nothing
        Me.GRD_Lineas.pActivarBotonFiltro = False
        Me.GRD_Lineas.pText = " "
        Me.GRD_Lineas.Size = New System.Drawing.Size(986, 417)
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
        'C_Almacen
        '
        StateEditorButton1.DisplayStyle = Infragistics.Win.UltraWinEditors.StateButtonDisplayStyle.StateButton
        StateEditorButton1.Key = "Todos"
        StateEditorButton1.Text = "Todos"
        Me.C_Almacen.ButtonsRight.Add(StateEditorButton1)
        Me.C_Almacen.Location = New System.Drawing.Point(12, 59)
        Me.C_Almacen.Name = "C_Almacen"
        Me.C_Almacen.Size = New System.Drawing.Size(398, 21)
        Me.C_Almacen.TabIndex = 2
        Me.C_Almacen.Text = "C_Almacen"
        '
        'UltraLabel29
        '
        Me.UltraLabel29.Location = New System.Drawing.Point(12, 43)
        Me.UltraLabel29.Name = "UltraLabel29"
        Me.UltraLabel29.Size = New System.Drawing.Size(96, 16)
        Me.UltraLabel29.TabIndex = 141
        Me.UltraLabel29.Text = "Almacén:"
        '
        'B_CarregarNS
        '
        Me.B_CarregarNS.Location = New System.Drawing.Point(416, 59)
        Me.B_CarregarNS.Name = "B_CarregarNS"
        Me.B_CarregarNS.Size = New System.Drawing.Size(79, 21)
        Me.B_CarregarNS.TabIndex = 4
        Me.B_CarregarNS.Text = "Buscar"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'frmEntrada_Linea_CompuestoPor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1013, 523)
        Me.Controls.Add(Me.B_CarregarNS)
        Me.Controls.Add(Me.C_Almacen)
        Me.Controls.Add(Me.UltraLabel29)
        Me.Controls.Add(Me.GRD_Lineas)
        Me.KeyPreview = True
        Me.Name = "frmEntrada_Linea_CompuestoPor"
        Me.Text = "Compuesto por..."
        Me.Controls.SetChildIndex(Me.GRD_Lineas, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel29, 0)
        Me.Controls.SetChildIndex(Me.C_Almacen, 0)
        Me.Controls.SetChildIndex(Me.B_CarregarNS, 0)
        CType(Me.C_Almacen, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents UltraTabPageControl3 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl10 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabPageControl11 As Infragistics.Win.UltraWinTabControl.UltraTabPageControl
    Friend WithEvents UltraTabSharedControlsPage1 As Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage
    Friend WithEvents GRD_Lineas As M_UltraGrid.m_UltraGrid
    Friend WithEvents C_Almacen As Infragistics.Win.UltraWinEditors.UltraComboEditor
    Friend WithEvents UltraLabel29 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents B_CarregarNS As Infragistics.Win.Misc.UltraButton
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
End Class
