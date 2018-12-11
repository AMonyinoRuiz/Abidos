Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmImputacionHoras_Materiales
    Dim oDTC As New DTCDataContext
    Dim oLinqParte As Parte
    Dim oDTFotosProductos As DataTable
    Dim oArrayProductesSeleccionats As New ArrayList
    Dim oArrayNSSeleccionats As New ArrayList
    Dim oActualitzantQuantitats As Boolean = False

    Public Structure StructProductesSeleccionats
        Dim IDProducto As Integer
        Dim Cantidad As Integer
    End Structure

    Public Structure StructProductesNSSeleccionats
        Dim IDProducto As Integer
        Dim IDNS As Integer
    End Structure

#Region "ToolForm"
    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub
    Private Sub ToolForm_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Try
            If oArrayProductesSeleccionats.Count = 0 Then
                Me.FormTancar()
                Exit Sub
            End If

            Dim _Struct As StructProductesSeleccionats
            Dim i As Integer
            For i = 0 To oArrayProductesSeleccionats.Count - 1
                If oArrayProductesSeleccionats.Item(i).cantidad <> 0 Then

                    Dim _AlmacenPersonal As Almacen = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Almacen.FirstOrDefault
                    Dim _IDProducto As Integer = CInt(oArrayProductesSeleccionats.Item(i).IDProducto)
                    Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = _IDProducto).FirstOrDefault



                    Dim _LineaOrigen As Entrada_Linea
                    Dim _LineaDestino As Entrada_Linea


                    If _Producto.RequiereNumeroSerie = True Then
                        Dim _cont As Integer
                        For _cont = 0 To oArrayNSSeleccionats.Count - 1
                            Call CrearTraspaso(_AlmacenPersonal, oLinqParte.Almacen.FirstOrDefault, _Producto, 1, False, _LineaOrigen, _LineaDestino, oArrayNSSeleccionats(_cont).IDNS)
                            Call CrearLineaMaterial(_Producto, _LineaOrigen, _LineaDestino, 1, oArrayNSSeleccionats(_cont).IDNS)
                        Next
                    Else
                        Call CrearTraspaso(_AlmacenPersonal, oLinqParte.Almacen.FirstOrDefault, _Producto, CInt(oArrayProductesSeleccionats(i).Cantidad), False, _LineaOrigen, _LineaDestino)
                        Call CrearLineaMaterial(_Producto, _LineaOrigen, _LineaDestino, CInt(oArrayProductesSeleccionats(i).Cantidad))
                    End If

                End If
            Next

            oDTC.SubmitChanges()

            Me.FormTancar()
            ' Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = CInt(Me.GRD_PartesActuales.GRID.ActiveRow.Cells("ID_Parte").Value)).FirstOrDefault
 

            'If _Producto.RequiereNumeroSerie = True Then
            '    If e.Row.Cells("NS").Value Is Nothing Then
            '        Mensaje.Mostrar_Mensaje("Imposible guardar, el producto seleccionado requiere la selección de un número de serie", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '        Exit Sub
            '    End If
            'End If

            'Dim _IDNS As Integer = 0
            'If _Producto.RequiereNumeroSerie = True Then
            '    _IDNS = e.Row.Cells("ID_NS").Value
            'End If
         Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


        'If oArrayProductesSeleccionats.Count > 0 Then
        '    Dim _IDArticulo As Integer
        '    For Each _IDArticulo In oArrayProductesSeleccionats
        '        Dim _NewLinea As New OfertaColeccion_Linea
        '        _NewLinea.Articulo = oDTC.Articulo.Where(Function(F) F.ID_Articulo = _IDArticulo).FirstOrDefault
        '        _NewLinea.Espacios = 0
        '        If oLinqfertaColeccion.OfertaColeccion_Linea.Count = 0 Then
        '            _NewLinea.Posicion = 1
        '        Else
        '            _NewLinea.Posicion = oLinqfertaColeccion.OfertaColeccion_Linea.Max(Function(F) F.Posicion) + 1
        '        End If
        '        _NewLinea.PVP = _NewLinea.Articulo.PVP
        '        _NewLinea.Observaciones = _NewLinea.Articulo.Observaciones
        '        oLinqfertaColeccion.OfertaColeccion_Linea.Add(_NewLinea)
        '    Next
        '    oDTC.SubmitChanges()

        'End If

        'Me.FormTancar()

    End Sub

    Private Sub CrearLineaMaterial(ByRef pProducte As Producto, ByRef pLineaOrigen As Entrada_Linea, ByRef pLineaDesti As Entrada_Linea, ByVal pQuantitat As Integer, Optional ByVal pIDNS As Integer = 0)
        Dim _Material As New Parte_MaterialOperarios
        _Material.Cantidad = pQuantitat
        _Material.FechaAlta = Now.Date
        _Material.Parte = oLinqParte
        _Material.Producto = pProducte
        _Material.Entrada_Linea = pLineaOrigen
        _Material.Entrada_Linea1 = pLineaDesti
        If pIDNS <> 0 Then
            _Material.NS = oDTC.NS.Where(Function(F) F.ID_NS = pIDNS).FirstOrDefault
        End If

        Dim _ClsNotificacion As New clsNotificacion(oDTC)
        _ClsNotificacion.CrearNotificacion_AlAssignarMaterialUnTecnicoAUnAlmacenDesdeImputacionDeTecnicos(oLinqParte)

        oLinqParte.Parte_MaterialOperarios.Add(_Material)
    End Sub

#End Region

#Region "Métodes"
    Public Sub Entrada(ByRef pDTC As DTCDataContext, ByRef pParte As Parte)
        Try
            Me.AplicarDisseny()
            oDTC = pDTC
            oLinqParte = pParte

            'Me.GRD_Seleccionats.M.clsToolBar.Boto_Afegir("VisualizarNS", "Visualizar números de serie", True)
            Call CargarGrid_Productos()
            Call CargarGrid_Productos_Seleccionats()


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearTraspaso(ByRef pMagatzemOrigen As Almacen, pMagatzemDesti As Almacen, ByRef pProducte As Producto, ByVal pQuantitat As Decimal, ByVal pRectificacio As Boolean, ByRef pLineaOrigen As Entrada_Linea, ByRef pLineaDestino As Entrada_Linea, Optional ByVal pID_NS As Integer = 0)
        Dim _MagatzemOrigen As Almacen
        Dim _MagatzemDesti As Almacen

        If pRectificacio = True Then 'Si el que estem fent és corregir un material assignat a un parte llavors el origen i destí s'inverteixen
            _MagatzemOrigen = pMagatzemDesti
            _MagatzemDesti = pMagatzemOrigen
        Else
            _MagatzemOrigen = pMagatzemOrigen
            _MagatzemDesti = pMagatzemDesti
        End If



        Dim _NewDocument As New Entrada
        _NewDocument.Almacen = _MagatzemOrigen
        _NewDocument.Almacen_Destino = pMagatzemDesti '_Parte.Almacen.FirstOrDefault
        _NewDocument.Descripcion = "Traspaso entre almacenes (Almacen personal - Parte)"
        _NewDocument.FechaAlta = Now.Date
        _NewDocument.FechaEntrada = Now.Date
        _NewDocument.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault
        _NewDocument.Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = EnumEntradaTipo.TraspasoAlmacen).FirstOrDefault
        _NewDocument.ID_Empresa = oEmpresa.ID_Empresa

        Dim _clsNewDocument As New clsEntrada(oDTC, _NewDocument)
        _NewDocument.Codigo = clsEntrada.RetornaCodiSeguent(EnumEntradaTipo.TraspasoAlmacen)
        oDTC.SubmitChanges()

        Dim _Linea As New Entrada_Linea
        With _Linea
            .Almacen = _MagatzemOrigen
            .Producto = pProducte
            .Descripcion = pProducte.Descripcion
            .FechaEntrada = Now.Date
            .FechaEntrega = Now.Date
            .Unidad = pQuantitat * -1
            .NoRestarStock = False
            .NoImprimirLinea = False
        End With

        _NewDocument.Entrada_Linea.Add(_Linea)

        Dim _ClsLinea As New clsEntradaLinea(_NewDocument, _Linea, oDTC)
        If _ClsLinea.AfegirLinea(True) = True Then
            If _ClsLinea.pRequiereNS = True Then
                _ClsLinea.LineaNSCrear(pID_NS)
            End If
            oDTC.SubmitChanges()


            pLineaOrigen = _ClsLinea.oLinqLinea
            'If pRow Is Nothing = False Then
            '    pRow.Cells("Entrada_Linea").Value = _ClsLinea.oLinqLinea
            'End If

            Dim _clsLinea2 As New clsEntradaLinea(_NewDocument, , oDTC)
            'If _ClsLinea.pRequiereNS = True Then 'Si requereix números de serie encara no duplicarem la línea ja que per introduir números de serie primer s'ha de guardar la línea per collons

            If pRectificacio = True Then 'Fem aquesta trampa pq si és una rectificació m'ho envia al revés del que vull
                _ClsLinea.oLinqEntrada.Almacen_Destino = _MagatzemDesti
            End If

            _ClsLinea.TraspasoAlmacenesRetornaNovaLineaDeSortida(_clsLinea2.oLinqLinea)
            oDTC.SubmitChanges()

            pLineaDestino = _clsLinea2.oLinqLinea

            'If pRow Is Nothing = False Then
            '    pRow.Cells("Entrada_Linea1").Value = _clsLinea2.oLinqLinea
            'End If
            'End If
        End If

        'oDTC.SubmitChanges()


    End Sub
#End Region

#Region "Grid Productos"
    Private Sub CargarGrid_Productos()
        Try

            'Dim _ArticlesSeleccionats As String = ""


            'If oArrayProductesSeleccionats.Count > 0 Then

            '    Dim _IDArticle As Integer
            '    For Each _IDArticle In oArrayProductesSeleccionats
            '        _ArticlesSeleccionats = _ArticlesSeleccionats & _IDArticle & ", "
            '    Next

            '    _ArticlesSeleccionats = "and ID_Articulo Not in (" & Mid(_ArticlesSeleccionats, 1, _ArticlesSeleccionats.Length - 2) & ")"
            'End If

            Dim _Almacen As Almacen = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Almacen.FirstOrDefault


            Dim _Result As RetornaStockResult
            Dim Total As Integer = oDTC.RetornaStock(0, _Almacen.ID_Almacen).Count
            Dim Llistat(0 To Total) As Integer

            Dim _ArticlesSeleccionats As String = ""

            Dim i As Integer = 0

            For Each _Result In oDTC.RetornaStock(0, _Almacen.ID_Almacen)
                If _Result.StockReal > 0 Then
                    _ArticlesSeleccionats = _ArticlesSeleccionats & _Result.ID_Producto & ", "
                End If
            Next
            _ArticlesSeleccionats = "Where ID_Producto  in (" & Mid(_ArticlesSeleccionats, 1, _ArticlesSeleccionats.Length - 2) & ")"

            ' Me.GRD_Articles.M.clsUltraGrid.Cargar("Select * From C_Articulo Order by Descripcion", BD)
            Me.GRD_Articles.DataSource = BD.RetornaDataTable("Select  ID_Producto, (Select CampoBinario From Archivo Where ID_Archivo= (Select ID_Archivo_FotoPredeterminada From Producto Where ID_Producto=C_Producto.ID_Producto) ) as CampoBinario  , Marca, Familia, Subfamilia, Codigo, Descripcion, RequiereNumeroSerie, (Select StockReal From TempStockRealPorProductoYPorAlmacen Where ID_Producto= C_Producto.ID_Producto  and ID_Almacen=" & _Almacen.ID_Almacen & ") as StockReal  From C_Producto " & _ArticlesSeleccionats & "  Order by Descripcion")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GridViewActividades_DoubleClick(sender As Object, e As EventArgs) Handles GridView1.DoubleClick
        Dim view As GridView = CType(sender, GridView)

        Dim pt As Point = view.GridControl.PointToClient(Control.MousePosition)

        DoRowDoubleClickActividades(view, pt)
    End Sub

    Private Sub DoRowDoubleClickActividades(ByVal view As GridView, ByVal pt As Point)

        Dim info As GridHitInfo = view.CalcHitInfo(pt)

        If info.InRow OrElse info.InRowCell Then

            If info.Column Is Nothing Then
                Exit Sub
            End If

            If GridView1.GetRowCellValue(info.RowHandle, info.Column) Is Nothing Then
                Exit Sub
            End If


            Dim _ID_Producto As Integer = Me.GridView1.GetRowCellValue(info.RowHandle, "ID_Producto")


            Dim _Struct As StructProductesSeleccionats

            'For Each _Struct In oArrayProductesSeleccionats
            '    If _Struct.IDProducto = _ID_Producto Then
            '        Mensaje.Mostrar_Mensaje("Imposible seleccionar dos veces el mismo artículo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '        Exit Sub
            '    End If
            'Next
            If BuscaProducto(_ID_Producto) <> 9999 Then
                Mensaje.Mostrar_Mensaje("Imposible seleccionar dos veces el mismo artículo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If


            Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = _ID_Producto).FirstOrDefault

            _Struct = New StructProductesSeleccionats
            _Struct.IDProducto = _ID_Producto
            If _Producto.RequiereNumeroSerie = True Then
                _Struct.Cantidad = 0
            Else
                _Struct.Cantidad = 1
            End If

            oArrayProductesSeleccionats.Add(_Struct)
            Call CargarGrid_Productos_Seleccionats()
            Call CargarGrid_NS(True)


            'Dim _frmActividadCRM As New frmActividadCRM_Mantenimiento
            '_frmActividadCRM.Entrada(_IDActividad)
            '_frmActividadCRM.FormObrir(frmPrincipal, True)

            ''Dim view As GridView = CType(sender, GridView)
            ''Dim pt As Point = view.GridControl.PointToClient(Control.MousePosition)
            ''Dim info As GridHitInfo = view.CalcHitInfo(pt)

            'If info.InRow Then
            '    If info.Column Is Nothing Then
            '        Exit Sub
            '    End If
            '    If GridView1.GetRowCellValue(info.RowHandle, "Leido") = 0 Then
            '        GridView1.SetRowCellValue(info.RowHandle, "Leido", 1)
            '    Else
            '        ' GridView1.SetRowCellValue(info.RowHandle, "Leido", 0)
            '    End If

            '    ' MsgBox(GridView1.GetRowCellValue(info.RowHandle, "Leido"))

            '    'Exit Sub
            'End If


            ''Dim info As GridHitInfo = view.CalcHitInfo(pt)

            ''GridView1.SetRowCellValue(info)

            'GridView1.LayoutChanged()
            'GridView1.MakeRowVisible(GridView1.FocusedRowHandle)
        End If

    End Sub

    Private Function BuscaProducto(ByVal pIDProducto As Integer) As Integer
        Dim i As Integer = 9999
        BuscaProducto = i
        For i = 0 To oArrayProductesSeleccionats.Count - 1
            Dim _Struct2 As StructProductesSeleccionats = oArrayProductesSeleccionats(i)
            If _Struct2.IDProducto = pIDProducto Then
                Return i
            End If
        Next


    End Function

    'Private Sub GRD_Articles_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.UltraGridRow)
    '    oArrayProductesSeleccionats.Add(e.Cells("ID_Articulo").Value)
    '    e.Hidden = True
    '    Call CargarGrid_Articles_Seleccionats()
    'End Sub

    'Private Sub GRD_Articles_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs)
    '    'With Me.GRD_Articles
    '    '    If oDTFotosProductos Is Nothing = False Then
    '    '        Dim DTRow As DataRow() = oDTFotosProductos.Select("ID_Articulo=" & e.Row.Cells("ID_Articulo").Value)
    '    '        If DTRow Is Nothing = False AndAlso DTRow.Length <> 0 Then
    '    '            If IsDBNull(DTRow(0).Item("FotoArticulo")) = False Then
    '    '                e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(DTRow(0).Item("FotoArticulo"))
    '    '            End If
    '    '        End If
    '    '    End If
    '    'End With
    '    'With Me.GRD_Articles
    '    '    '   If oDTFotosProductos Is Nothing = False Then
    '    '    'Dim DTRow As DataRow() = oDTFotosProductos.Select("ID_Articulo=" & e.Row.Cells("ID_Articulo").Value)
    '    '    Exit Sub
    '    '    Dim ID_Archivo As Integer = Util.Comprobar_NULL_Per_0(BD.RetornaValorSQL("Select ID_Archivo_FotoPrincipal From Articulo Where ID_Articulo=" & e.Row.Cells("ID_Articulo").Value))
    '    '    If ID_Archivo > 0 Then
    '    '        e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(BD.RetornaValorSQL("Select CampoBinario From Archivo Where ID_Archivo=" & ID_Archivo))
    '    '    End If

    '    'End With
    'End Sub
#End Region

#Region "Grid articles seleccionats"
    Private Sub CargarGrid_Productos_Seleccionats()
        Try

            Dim _ProductosSeleccionats As String = ""


            If oArrayProductesSeleccionats.Count > 0 Then

                Dim _Struct As StructProductesSeleccionats
                For Each _Struct In oArrayProductesSeleccionats
                    _ProductosSeleccionats = _ProductosSeleccionats & _Struct.IDProducto & ", "
                Next

                _ProductosSeleccionats = "Where ID_Producto in (" & Mid(_ProductosSeleccionats, 1, _ProductosSeleccionats.Length - 2) & ")"

                Dim _DT As DataTable = BD.RetornaDataTable("Select * From C_Producto " & _ProductosSeleccionats & "Order by Descripcion")
                _DT.Columns.Add("Cantidad", GetType(Integer))

                'Dim _DTRow As DataRow
                'For Each _DTRow In _DT.Rows
                '    Dim _ID As Integer = 0
                '    Dim _Ordre As Integer = 0
                '    For Each _ID In oArrayProductesSeleccionats
                '        If _ID = _DTRow.Item("ID_Articulo") Then
                '            _DTRow.Item("Ordre") = _Ordre
                '        End If
                '        _Ordre = _Ordre + 1
                '    Next
                'Next

                Me.GRD_Seleccionats.M.clsUltraGrid.Cargar(_DT)
            Else
                Me.GRD_Seleccionats.M.clsUltraGrid.Cargar("Select * From C_Producto Where ID_Producto=0 Order by Descripcion", BD)
            End If


            Me.GRD_Seleccionats.M_Editable()

            Dim _Col As Infragistics.Win.UltraWinGrid.UltraGridColumn
            For Each _Col In Me.GRD_Seleccionats.GRID.DisplayLayout.Bands(0).Columns
                If _Col.Key <> "Cantidad" Then
                    _Col.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
                End If
            Next
            oActualitzantQuantitats = True
            Dim _Row As Infragistics.Win.UltraWinGrid.UltraGridRow
            For Each _Row In Me.GRD_Seleccionats.GRID.DisplayLayout.Rows

                Dim _Struct As StructProductesSeleccionats
                For Each _Struct In oArrayProductesSeleccionats
                    If _Struct.IDProducto = _Row.Cells("ID_Producto").Value Then
                        _Row.Cells("Cantidad").Value = _Struct.Cantidad
                        _Row.Update()
                    End If
                Next
            Next
            oActualitzantQuantitats = False
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seleccionats_M_GRID_BeforeCellUpdate(sender As Object, e As Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs) Handles GRD_Seleccionats.M_GRID_BeforeCellUpdate
        If e.Cell.Column.Key = "Cantidad" Then
            If oActualitzantQuantitats = True Then
                Exit Sub
            End If


            Dim _IDProducto As Integer = e.Cell.Row.Cells("ID_Producto").Value
            Dim _IDAlmacen As Integer = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Almacen.FirstOrDefault.ID_Almacen
            Dim _StockActual As Integer = oDTC.RetornaStock(_IDProducto, _IDAlmacen).FirstOrDefault.StockReal
            Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = _IDProducto).FirstOrDefault

            If e.NewValue = 0 And _Producto.RequiereNumeroSerie = False Then
                Mensaje.Mostrar_Mensaje("Imposible asignar 0 como cantidad", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                e.Cancel = True
                Exit Sub
            End If

            If e.NewValue > _StockActual Then
                Mensaje.Mostrar_Mensaje("Imposible asignar más cantidad de la se posee", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                e.Cancel = True
                Exit Sub
            End If

            Dim _Struct As StructProductesSeleccionats
            Dim i As Integer
            For i = 0 To oArrayProductesSeleccionats.Count - 1
                If oArrayProductesSeleccionats.Item(i).IDProducto = _IDProducto Then
                    _Struct = oArrayProductesSeleccionats(i)
                    _Struct.Cantidad = e.NewValue
                    'oArrayProductesSeleccionats(i).cantidad = e.NewValue
                    oArrayProductesSeleccionats(i) = _Struct
                    Exit For
                End If
            Next
            'For Each _Struct In oArrayProductesSeleccionats
            '    If _Struct.IDProducto = e.Cell.Row.Cells("ID_Producto").Value Then
            '        _Struct.Cantidad = e.NewValue
            '        'oArrayProductesSeleccionats.Remove(_Struct)
            '        'oArrayProductesSeleccionats.Add(_Struct)
            '        'oArrayProductesSeleccionats(0) = _Struct
            '        ' oArrayProductesSeleccionats.SetRange(0, _Struct)
            '        Exit For
            '    End If
            'Next
        End If
    End Sub

    Private Sub GRD_Seleccionats_M_GRID_Click(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Seleccionats.M_GRID_Click
        If Me.GRD_Seleccionats.GRID.ActiveRow Is Nothing Then
            Call CargarGrid_NS(True)
        End If
    End Sub

    Private Sub GRD_Seleccionats_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles GRD_Seleccionats.M_GRID_ClickRow2
        Call CargarGrid_NS(False)
    End Sub

    Private Sub GRD_NS_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_NS.M_GRID_AfterCellActivate
        With GRD_NS.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                .ActiveCell.Value = Not .ActiveCell.Value
                .ActiveCell.Row.Update()
                .ActiveRow = Nothing
                .Selected.Rows.Clear()
                .ActiveCell = Nothing
            End If

            'If .ActiveRow.ParentRow Is Nothing = False Then
            '    .ActiveRow.ParentRow.Update()
            'End If


        End With
    End Sub
    Private Sub GRD_Seleccionats_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles GRD_Seleccionats.M_GRID_DoubleClickRow2

        'Call CargarGrid_Productos()
        e.Hidden = True

        Dim _Struct As StructProductesSeleccionats
        Dim i As Integer
        Dim _Trobat As Integer
        For i = 0 To oArrayProductesSeleccionats.Count - 1
            If oArrayProductesSeleccionats.Item(i).IDProducto = e.Cells("ID_Producto").Value Then
                _Trobat = i
                Exit For
            End If
        Next

        Call EliminaNS(e.Cells("ID_Producto").Value)

        oArrayProductesSeleccionats.RemoveAt(_Trobat)

        Call CargarGrid_NS(True)

        'Call CargarGrid_Articles_Seleccionats()
    End Sub

    Private Sub GRD_Seleccionats_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Seleccionats.M_Grid_InitializeRow
        If e.Row.Cells("RequiereNumeroSerie").Value = True Then
            e.Row.Activation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
            e.Row.CellAppearance.BackColor = Color.LightYellow
        End If




        'With Me.GRD_Seleccionats
        '    If oDTFotosProductos Is Nothing = False Then
        '        Dim DTRow As DataRow() = oDTFotosProductos.Select("ID_Articulo=" & e.Row.Cells("ID_Articulo").Value)
        '        If DTRow Is Nothing = False AndAlso DTRow.Length <> 0 Then
        '            If IsDBNull(DTRow(0).Item("FotoArticulo")) = False Then
        '                e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(DTRow(0).Item("FotoArticulo"))
        '            End If
        '        End If
        '    End If
        'End With

        'With Me.GRD_Seleccionats
        '    '   If oDTFotosProductos Is Nothing = False Then
        '    'Dim DTRow As DataRow() = oDTFotosProductos.Select("ID_Articulo=" & e.Row.Cells("ID_Articulo").Value)

        '    Dim ID_Archivo As Integer = Util.Comprobar_NULL_Per_0(BD.RetornaValorSQL("Select ID_Archivo_FotoPrincipal From Articulo Where ID_Articulo=" & e.Row.Cells("ID_Articulo").Value))
        '    If ID_Archivo > 0 Then
        '        e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(BD.RetornaValorSQL("Select CampoBinario From Archivo Where ID_Archivo=" & ID_Archivo))
        '    End If

        'End With
    End Sub



#End Region

#Region "Grid NS"
    Private Sub CargarGrid_NS(Optional ByVal pBlanquejar As Boolean = False)
        Try
            If pBlanquejar = True Then
                Me.GRD_NS.GRID.DataSource = Nothing
                Exit Sub
            End If
            If Me.GRD_Seleccionats.GRID.ActiveRow Is Nothing Then
                Exit Sub
            End If
            'If Me.GRD_Seleccionats.GRID.ActiveRow.Cells("RequiereNumeroSerie").Value = False Then
            '    Mensaje.Mostrar_Mensaje("El artículo seleccionado no requiere números de serie", M_Mensaje.Missatge_Modo.INFORMACIO, "")
            '    Exit Sub
            'End If

            Dim _IDProducto As Integer
            _IDProducto = Me.GRD_Seleccionats.GRID.ActiveRow.Cells("ID_Producto").Value

            Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = _IDProducto).FirstOrDefault
            If _Producto.RequiereNumeroSerie = False Then
                Me.GRD_NS.GRID.DataSource = Nothing
                Exit Sub
            End If

            Dim _IDAlmacen As Integer = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Almacen.FirstOrDefault.ID_Almacen

            Dim _Result As NS
            Dim Total As Integer = oDTC.NS.Where(Function(F) F.ID_Almacen = _IDAlmacen And F.ID_Producto = _IDProducto).Count
            Dim Llistat(0 To Total) As Integer

            Dim i As Integer = 0
            For Each _Result In oDTC.NS.Where(Function(F) F.ID_Almacen = _IDAlmacen And F.ID_Producto = _IDProducto)
                i = i + 1
                Llistat(i) = _Result.ID_NS
            Next

            Dim _LlistaNS As IEnumerable(Of Integer)
            _LlistaNS = From Taula In oDTC.NS Where Llistat.Contains(Taula.ID_NS) And Taula.ID_NS_Estado = EnumNSEstado.Disponible Order By Taula.Descripcion Select Taula.ID_NS

            Dim _ID As Integer
            Dim _Select As String = ""
            For Each _ID In _LlistaNS
                _Select = _Select & _ID & ", "
            Next
            _Select = Mid(_Select, 1, _Select.Length - 2)

            Dim DT As New DataTable

            DT = BD.RetornaDataTable("Select *,cast(0 as bit) as Seleccion From NS Where ID_NS in (" & _Select & ")")

            'BD.RetornaDataTable("Select *, cast(0 as bit) as Seleccion From C_Entrada_Seleccion_NS Where (ID_NS_Estado=" & _STRDisponible & " or ID_NS in (Select ID_NS From Entrada_Linea_NS, Entrada_Linea Where Entrada_Linea_NS.ID_Entrada_Linea=Entrada_Linea.ID_Entrada_Linea and Entrada_Linea.ID_Entrada_Linea=" & pIDEntradaLinea & ")) and ID_Producto=" & pIDProducto & _strMagatzem & _STRCliente & _STRProveedor)

            oActualitzantQuantitats = True
            Me.GRD_NS.M.clsUltraGrid.Cargar(DT)

            Me.GRD_NS.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True

            Me.GRD_NS.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellClickAction = CellClickAction.CellSelect
            Me.GRD_NS.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellActivation = Activation.AllowEdit



            Dim _Row As Infragistics.Win.UltraWinGrid.UltraGridRow
            For Each _Row In Me.GRD_NS.GRID.DisplayLayout.Rows
                Dim _Struct2 As StructProductesNSSeleccionats
                For Each _Struct2 In oArrayNSSeleccionats
                    If _Struct2.IDNS = _Row.Cells("ID_NS").Value Then
                        _Row.Cells("Seleccion").Value = True
                        _Row.Update()
                    End If
                Next
            Next
            oActualitzantQuantitats = False
            'If pNomesAmbStock = True Then
            '    
            'Else
            '    Dim _IDNS As Integer = pRow.Cells("ID_NS").Value
            '    DT = From Taula In oDTC.NS Where Taula.ID_NS = _IDNS Order By Taula.Descripcion Select NS = Taula, Descripcion = Taula.Descripcion
            'End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_NS_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_NS.M_Grid_InitializeRow

        If oActualitzantQuantitats = False Then
            Call ControlSeleccio(Me.GRD_NS.GRID.ActiveCell, e.Row)
        End If

    End Sub

    Private Sub ControlSeleccio(ByRef pActiveCell As UltraGridCell, ByRef pActiveRow As UltraGridRow)

        Dim _IDNS As Integer = pActiveRow.Cells("ID_NS").Value
        If pActiveRow.Cells("Seleccion").Value = True Then
            Dim _seleccionat As Integer = BuscaNS(_IDNS)
            If _seleccionat = 9999 Then
                Dim _NSBD As NS = oDTC.NS.Where(Function(F) F.ID_NS = _IDNS).FirstOrDefault
                Dim _NS As New StructProductesNSSeleccionats
                _NS.IDNS = _IDNS
                _NS.IDProducto = _NSBD.Producto.ID_Producto
                oArrayNSSeleccionats.Add(_NS)
                Me.GRD_Seleccionats.GRID.ActiveRow.Cells("Cantidad").Value = ContaNS(_NS.IDProducto)
                Me.GRD_Seleccionats.GRID.ActiveRow.Update()
            End If
        Else
            Dim _seleccionat As Integer = BuscaNS(_IDNS)
            If _seleccionat <> 9999 Then
                Dim _NS As StructProductesNSSeleccionats = oArrayNSSeleccionats(_seleccionat)
                oArrayNSSeleccionats.RemoveAt(_seleccionat)
                Me.GRD_Seleccionats.GRID.ActiveRow.Cells("Cantidad").Value = ContaNS(_NS.IDProducto)
                Me.GRD_Seleccionats.GRID.ActiveRow.Update()
            End If
        End If

    End Sub

    Private Function BuscaNS(ByVal pIDNS As Integer) As Integer
        Dim i As Integer = 9999
        BuscaNS = i
        For i = 0 To oArrayNSSeleccionats.Count - 1
            Dim _Struct2 As StructProductesNSSeleccionats = oArrayNSSeleccionats(i)
            If _Struct2.IDNS = pIDNS Then
                Return i
            End If
        Next

    End Function


    Private Function BuscaNSxProducte(ByVal pIDProducte As Integer) As Integer
        Dim i As Integer = 9999
        BuscaNSxProducte = i
        For i = 0 To oArrayNSSeleccionats.Count - 1
            Dim _Struct2 As StructProductesNSSeleccionats = oArrayNSSeleccionats(i)
            If _Struct2.IDProducto = pIDProducte Then
                Return i
            End If
        Next

    End Function

    Private Function ContaNS(ByVal pIDProducto As Integer) As Integer
        Dim i As Integer = 9999
        ContaNS = 0
        For i = 0 To oArrayNSSeleccionats.Count - 1
            Dim _Struct2 As StructProductesNSSeleccionats = oArrayNSSeleccionats(i)
            If _Struct2.IDProducto = pIDProducto Then
                ContaNS = ContaNS + 1
            End If
        Next
        Return ContaNS
    End Function

    Private Sub EliminaNS(ByVal pIDProducto As Integer)
        Dim _IDAEliminar As Integer = BuscaNSxProducte(pIDProducto)
        Do While _IDAEliminar <> 9999
            oArrayNSSeleccionats.RemoveAt(_IDAEliminar)
            _IDAEliminar = BuscaNSxProducte(pIDProducto)
        Loop


    End Sub
#End Region

End Class