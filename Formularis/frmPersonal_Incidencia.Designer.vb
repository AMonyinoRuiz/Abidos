<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPersonal_Incidencia
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPersonal_Incidencia))
        Me.T_Lugar = New M_TextEditor.m_TextEditor()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.DT_Alta = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel10 = New Infragistics.Win.Misc.UltraLabel()
        Me.DT_Incidencia = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_Testigos = New M_TextEditor.m_TextEditor()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.T_MaterialAfectado = New M_TextEditor.m_TextEditor()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.TE_Cliente = New M_TextEditor.m_TextEditor()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.R_Descripcion = New M_RichText.M_RichText()
        Me.R_Sancion = New M_RichText.M_RichText()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        CType(Me.T_Lugar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_Incidencia, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_Testigos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.T_MaterialAfectado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TE_Cliente, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.SeleccionarSortir
        Me.ToolForm.Size = New System.Drawing.Size(987, 24)
        '
        'T_Lugar
        '
        Me.T_Lugar.Location = New System.Drawing.Point(12, 107)
        Me.T_Lugar.Name = "T_Lugar"
        Me.T_Lugar.Size = New System.Drawing.Size(987, 21)
        Me.T_Lugar.TabIndex = 3
        Me.T_Lugar.Text = "T_Lugar"
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 225)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(163, 16)
        Me.UltraLabel1.TabIndex = 19
        Me.UltraLabel1.Text = "*Descripción de la incidencia:"
        '
        'UltraLabel4
        '
        Me.UltraLabel4.Location = New System.Drawing.Point(12, 92)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel4.TabIndex = 20
        Me.UltraLabel4.Text = "*Lugar:"
        '
        'DT_Alta
        '
        Me.DT_Alta.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Alta.Location = New System.Drawing.Point(123, 59)
        Me.DT_Alta.MaskInput = "{date}"
        Me.DT_Alta.Name = "DT_Alta"
        Me.DT_Alta.ReadOnly = True
        Me.DT_Alta.Size = New System.Drawing.Size(91, 21)
        Me.DT_Alta.TabIndex = 1
        Me.DT_Alta.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel10
        '
        Me.UltraLabel10.Location = New System.Drawing.Point(122, 43)
        Me.UltraLabel10.Name = "UltraLabel10"
        Me.UltraLabel10.Size = New System.Drawing.Size(81, 16)
        Me.UltraLabel10.TabIndex = 173
        Me.UltraLabel10.Text = "*Fecha alta:"
        '
        'DT_Incidencia
        '
        Me.DT_Incidencia.DateTime = New Date(2007, 1, 23, 0, 0, 0, 0)
        Me.DT_Incidencia.Location = New System.Drawing.Point(12, 59)
        Me.DT_Incidencia.Name = "DT_Incidencia"
        Me.DT_Incidencia.Size = New System.Drawing.Size(93, 21)
        Me.DT_Incidencia.TabIndex = 0
        Me.DT_Incidencia.Value = New Date(2007, 1, 23, 0, 0, 0, 0)
        '
        'UltraLabel7
        '
        Me.UltraLabel7.Location = New System.Drawing.Point(11, 43)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.Size = New System.Drawing.Size(100, 16)
        Me.UltraLabel7.TabIndex = 172
        Me.UltraLabel7.Text = "*Fecha incidencia:"
        '
        'T_Testigos
        '
        Me.T_Testigos.Location = New System.Drawing.Point(12, 149)
        Me.T_Testigos.Name = "T_Testigos"
        Me.T_Testigos.Size = New System.Drawing.Size(987, 21)
        Me.T_Testigos.TabIndex = 4
        Me.T_Testigos.Text = "T_Testigos"
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Location = New System.Drawing.Point(12, 134)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel2.TabIndex = 175
        Me.UltraLabel2.Text = "Testigos:"
        '
        'T_MaterialAfectado
        '
        Me.T_MaterialAfectado.Location = New System.Drawing.Point(12, 193)
        Me.T_MaterialAfectado.Name = "T_MaterialAfectado"
        Me.T_MaterialAfectado.Size = New System.Drawing.Size(987, 21)
        Me.T_MaterialAfectado.TabIndex = 5
        Me.T_MaterialAfectado.Text = "T_MaterialAfectado"
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Location = New System.Drawing.Point(12, 178)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel3.TabIndex = 177
        Me.UltraLabel3.Text = "Material afectado:"
        '
        'TE_Cliente
        '
        Me.TE_Cliente.Location = New System.Drawing.Point(230, 59)
        Me.TE_Cliente.Name = "TE_Cliente"
        Me.TE_Cliente.ReadOnly = True
        Me.TE_Cliente.Size = New System.Drawing.Size(769, 21)
        Me.TE_Cliente.TabIndex = 2
        Me.TE_Cliente.Text = "TE_Cliente"
        '
        'UltraLabel5
        '
        Me.UltraLabel5.Location = New System.Drawing.Point(229, 43)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(257, 16)
        Me.UltraLabel5.TabIndex = 179
        Me.UltraLabel5.Text = "Cliente final:"
        '
        'R_Descripcion
        '
        Me.R_Descripcion.Location = New System.Drawing.Point(11, 242)
        Me.R_Descripcion.Name = "R_Descripcion"
        Me.R_Descripcion.pEnable = True
        Me.R_Descripcion.pText = resources.GetString("R_Descripcion.pText")
        Me.R_Descripcion.pTextEspecial = ""
        Me.R_Descripcion.Size = New System.Drawing.Size(988, 169)
        Me.R_Descripcion.TabIndex = 6
        '
        'R_Sancion
        '
        Me.R_Sancion.Location = New System.Drawing.Point(12, 433)
        Me.R_Sancion.Name = "R_Sancion"
        Me.R_Sancion.pEnable = True
        Me.R_Sancion.pText = resources.GetString("R_Sancion.pText")
        Me.R_Sancion.pTextEspecial = ""
        Me.R_Sancion.Size = New System.Drawing.Size(987, 169)
        Me.R_Sancion.TabIndex = 7
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(13, 416)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(55, 16)
        Me.UltraLabel6.TabIndex = 181
        Me.UltraLabel6.Text = "Sanción:"
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'frmPersonal_Incidencia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1011, 641)
        Me.Controls.Add(Me.R_Sancion)
        Me.Controls.Add(Me.UltraLabel6)
        Me.Controls.Add(Me.R_Descripcion)
        Me.Controls.Add(Me.TE_Cliente)
        Me.Controls.Add(Me.UltraLabel5)
        Me.Controls.Add(Me.T_MaterialAfectado)
        Me.Controls.Add(Me.UltraLabel3)
        Me.Controls.Add(Me.T_Testigos)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.DT_Alta)
        Me.Controls.Add(Me.UltraLabel10)
        Me.Controls.Add(Me.DT_Incidencia)
        Me.Controls.Add(Me.UltraLabel7)
        Me.Controls.Add(Me.T_Lugar)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel4)
        Me.KeyPreview = True
        Me.Name = "frmPersonal_Incidencia"
        Me.Text = "Incidencias de personal"
        Me.Controls.SetChildIndex(Me.UltraLabel4, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel1, 0)
        Me.Controls.SetChildIndex(Me.T_Lugar, 0)
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel7, 0)
        Me.Controls.SetChildIndex(Me.DT_Incidencia, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel10, 0)
        Me.Controls.SetChildIndex(Me.DT_Alta, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel2, 0)
        Me.Controls.SetChildIndex(Me.T_Testigos, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel3, 0)
        Me.Controls.SetChildIndex(Me.T_MaterialAfectado, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel5, 0)
        Me.Controls.SetChildIndex(Me.TE_Cliente, 0)
        Me.Controls.SetChildIndex(Me.R_Descripcion, 0)
        Me.Controls.SetChildIndex(Me.UltraLabel6, 0)
        Me.Controls.SetChildIndex(Me.R_Sancion, 0)
        CType(Me.T_Lugar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Alta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_Incidencia, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_Testigos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.T_MaterialAfectado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TE_Cliente, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents T_Lugar As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents DT_Alta As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel10 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents DT_Incidencia As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_Testigos As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents T_MaterialAfectado As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents TE_Cliente As M_TextEditor.m_TextEditor
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents R_Descripcion As M_RichText.M_RichText
    Friend WithEvents R_Sancion As M_RichText.M_RichText
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents ErrorProvider1 As System.Windows.Forms.ErrorProvider
End Class
