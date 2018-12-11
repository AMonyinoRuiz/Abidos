<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImputacionHoras_Materiales
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
        Me.GRD_Seleccionats = New M_UltraGrid.m_UltraGrid()
        Me.GRD_NS = New M_UltraGrid.m_UltraGrid()
        Me.GRD_Articles = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Foto = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.ID_Producto = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Marca = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Familia = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Subfamilia = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Codigo = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.Descripcion = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RequiereNumeroSerie = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.StockReal = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(Me.GRD_Articles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ToolForm
        '
        Me.ToolForm.pMode_Toolbar = m_ToolForm.clsToolForm.Enum_ToolMode.GuardarSortir
        '
        'GRD_Seleccionats
        '
        Me.GRD_Seleccionats.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Seleccionats.Location = New System.Drawing.Point(12, 415)
        Me.GRD_Seleccionats.Name = "GRD_Seleccionats"
        Me.GRD_Seleccionats.pAccessibleName = Nothing
        Me.GRD_Seleccionats.pActivarBotonFiltro = False
        Me.GRD_Seleccionats.pText = " "
        Me.GRD_Seleccionats.Size = New System.Drawing.Size(743, 181)
        Me.GRD_Seleccionats.TabIndex = 2
        '
        'GRD_NS
        '
        Me.GRD_NS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_NS.Location = New System.Drawing.Point(761, 415)
        Me.GRD_NS.Name = "GRD_NS"
        Me.GRD_NS.pAccessibleName = Nothing
        Me.GRD_NS.pActivarBotonFiltro = False
        Me.GRD_NS.pText = " "
        Me.GRD_NS.Size = New System.Drawing.Size(230, 181)
        Me.GRD_NS.TabIndex = 3
        '
        'GRD_Articles
        '
        Me.GRD_Articles.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GRD_Articles.Location = New System.Drawing.Point(12, 54)
        Me.GRD_Articles.MainView = Me.GridView1
        Me.GRD_Articles.Name = "GRD_Articles"
        Me.GRD_Articles.Size = New System.Drawing.Size(979, 355)
        Me.GRD_Articles.TabIndex = 4
        Me.GRD_Articles.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.Foto, Me.ID_Producto, Me.Marca, Me.Familia, Me.Subfamilia, Me.Codigo, Me.Descripcion, Me.StockReal, Me.RequiereNumeroSerie})
        Me.GridView1.GridControl = Me.GRD_Articles
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsFind.AlwaysVisible = True
        Me.GridView1.OptionsFind.SearchInPreview = True
        '
        'Foto
        '
        Me.Foto.Caption = "Foto"
        Me.Foto.FieldName = "CampoBinario"
        Me.Foto.Name = "Foto"
        Me.Foto.Visible = True
        Me.Foto.VisibleIndex = 0
        Me.Foto.Width = 62
        '
        'ID_Producto
        '
        Me.ID_Producto.Caption = "ID_Producto"
        Me.ID_Producto.FieldName = "ID_Producto"
        Me.ID_Producto.Name = "ID_Producto"
        '
        'Marca
        '
        Me.Marca.Caption = "Marca"
        Me.Marca.FieldName = "Marca"
        Me.Marca.Name = "Marca"
        Me.Marca.OptionsColumn.AllowEdit = False
        Me.Marca.Visible = True
        Me.Marca.VisibleIndex = 1
        Me.Marca.Width = 112
        '
        'Familia
        '
        Me.Familia.Caption = "Familia"
        Me.Familia.FieldName = "Familia"
        Me.Familia.Name = "Familia"
        Me.Familia.OptionsColumn.AllowEdit = False
        Me.Familia.Visible = True
        Me.Familia.VisibleIndex = 2
        Me.Familia.Width = 105
        '
        'Subfamilia
        '
        Me.Subfamilia.Caption = "Subfamilia"
        Me.Subfamilia.FieldName = "Subfamilia"
        Me.Subfamilia.Name = "Subfamilia"
        Me.Subfamilia.OptionsColumn.AllowEdit = False
        Me.Subfamilia.Visible = True
        Me.Subfamilia.VisibleIndex = 3
        Me.Subfamilia.Width = 93
        '
        'Codigo
        '
        Me.Codigo.Caption = "Código"
        Me.Codigo.FieldName = "Codigo"
        Me.Codigo.Name = "Codigo"
        Me.Codigo.OptionsColumn.AllowEdit = False
        Me.Codigo.Visible = True
        Me.Codigo.VisibleIndex = 4
        Me.Codigo.Width = 155
        '
        'Descripcion
        '
        Me.Descripcion.Caption = "Descripción"
        Me.Descripcion.FieldName = "Descripcion"
        Me.Descripcion.Name = "Descripcion"
        Me.Descripcion.OptionsColumn.AllowEdit = False
        Me.Descripcion.Visible = True
        Me.Descripcion.VisibleIndex = 5
        Me.Descripcion.Width = 1049
        '
        'RequiereNumeroSerie
        '
        Me.RequiereNumeroSerie.Caption = "N.S."
        Me.RequiereNumeroSerie.FieldName = "RequiereNumeroSerie"
        Me.RequiereNumeroSerie.Name = "RequiereNumeroSerie"
        Me.RequiereNumeroSerie.OptionsColumn.AllowEdit = False
        Me.RequiereNumeroSerie.Visible = True
        Me.RequiereNumeroSerie.VisibleIndex = 7
        '
        'StockReal
        '
        Me.StockReal.AppearanceCell.Options.UseTextOptions = True
        Me.StockReal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        Me.StockReal.Caption = "StockReal"
        Me.StockReal.FieldName = "StockReal"
        Me.StockReal.Name = "StockReal"
        Me.StockReal.OptionsColumn.AllowEdit = False
        Me.StockReal.Visible = True
        Me.StockReal.VisibleIndex = 6
        '
        'frmImputacionHoras_Materiales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1003, 607)
        Me.Controls.Add(Me.GRD_Articles)
        Me.Controls.Add(Me.GRD_NS)
        Me.Controls.Add(Me.GRD_Seleccionats)
        Me.KeyPreview = True
        Me.Name = "frmImputacionHoras_Materiales"
        Me.Text = "Asignación de materiales"
        Me.Controls.SetChildIndex(Me.ToolForm, 0)
        Me.Controls.SetChildIndex(Me.GRD_Seleccionats, 0)
        Me.Controls.SetChildIndex(Me.GRD_NS, 0)
        Me.Controls.SetChildIndex(Me.GRD_Articles, 0)
        CType(Me.GRD_Articles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GRD_Seleccionats As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_NS As M_UltraGrid.m_UltraGrid
    Friend WithEvents GRD_Articles As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents ID_Producto As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Codigo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Descripcion As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Foto As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Marca As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Familia As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents Subfamilia As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents RequiereNumeroSerie As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents StockReal As DevExpress.XtraGrid.Columns.GridColumn
End Class
