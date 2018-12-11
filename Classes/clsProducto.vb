Public Class clsProducto
    Dim oDTC As DTCDataContext
    Public oProducto As Producto

    Public Sub New(ByVal pIDProducto As Integer, ByRef pDTC As DTCDataContext)
        oProducto = pDTC.Producto.Where(Function(F) F.ID_Producto = pIDProducto).FirstOrDefault
        oDTC = pDTC
    End Sub

    Public Function RetornaStockActual(ByVal pIDAlmacen As Integer) As Decimal
        Dim _Stock As RetornaStockResult
        _Stock = oDTC.RetornaStock(oProducto.ID_Producto, pIDAlmacen).FirstOrDefault
        If _Stock Is Nothing Then
            Return 0
        Else
            Return _Stock.StockReal
        End If
    End Function

    Public Function RetornaStockActualJaCalculat(ByVal pIDAlmacen As Integer) As Decimal
        If oDTC.TempStockRealPorProductoYPorAlmacen.Where(Function(F) F.ID_Producto = oProducto.ID_Producto And F.ID_Almacen = pIDAlmacen).Count > 0 Then
            Return oDTC.TempStockRealPorProductoYPorAlmacen.Where(Function(F) F.ID_Producto = oProducto.ID_Producto And F.ID_Almacen = pIDAlmacen).FirstOrDefault.StockReal
        End If
    End Function

    Public Function RetornaStockTeorico(ByVal pIDAlmacen As Integer) As Decimal
        Dim _Stock As RetornaStockResult
        _Stock = oDTC.RetornaStock(oProducto.ID_Producto, pIDAlmacen).FirstOrDefault
        If _Stock Is Nothing Then
            Return 0
        Else
            Return _Stock.StockTeorico
        End If
    End Function

    Public Function RetornaStockTeoricJaCalculat(ByVal pIDAlmacen As Integer) As Decimal
        If oDTC.TempStockRealPorProductoYPorAlmacen.Where(Function(F) F.ID_Producto = oProducto.ID_Producto And F.ID_Almacen = pIDAlmacen).Count > 0 Then
            Return oDTC.TempStockRealPorProductoYPorAlmacen.Where(Function(F) F.ID_Producto = oProducto.ID_Producto And F.ID_Almacen = pIDAlmacen).FirstOrDefault.StockTeorico
        End If
    End Function




    'Public Function RetornaStock() As DataTable
    '    Dim _DT As New DataTable
    '    _DT.Columns.Add("ID_Producto", GetType(Integer))
    '    _DT.Columns.Add("Codigo", GetType(String))
    '    _DT.Columns.Add("Descripcion", GetType(String))
    '    _DT.Columns.Add("ID_Almacen", GetType(Integer))
    '    _DT.Columns.Add("Almacen", GetType(String))
    '    _DT.Columns.Add("StockReal", GetType(Decimal))
    '    _DT.Columns.Add("StockPrevision", GetType(Decimal))

    '    Dim _Almacen As Almacen
    '    For Each _Almacen In oDTC.Almacen.Where(Function(F) F.Activo = True)
    '        Dim _DTRow As DataRow
    '        _DTRow = _DT.NewRow
    '        _DTRow("ID_Producto") = oProducto.ID_Producto
    '        _DTRow("Codigo") = oProducto.Codigo
    '        _DTRow("Descripcion") = oProducto.Descripcion
    '        _DTRow("ID_Almacen") = _Almacen.ID_Almacen
    '        _DTRow("Almacen") = _Almacen.Descripcion
    '        If oProducto.RequiereNumeroSerie = True Then
    '            _DTRow("StockReal") = oProducto.NS.Where(Function(F) F.ID_Almacen = _Almacen.ID_Almacen And F.ID_NS_Estado = EnumNSEstado.Disponible).Count ' oDTC.NS.Where(Function(F) F.ID_Almacen=_Almacen.ID_Almacen and  F.ID_Producto = oProducto.ID_Producto And F.ID_NS_Estado = EnumNSEstado.Disponible).Count
    '            _DTRow("StockPrevision") = 9999
    '        Else
    '            Dim _CantidadAlbaranesCompra As Decimal
    '            Dim _CantidadAlbaranesVenta As Decimal
    '            Dim _CantidadPedidosCompra As Decimal
    '            Dim _CantidadPedidosVenta As Decimal
    '            _CantidadAlbaranesCompra = oProducto.Entrada_Linea.Where(Function(F) F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranCompra).Sum(Function(F) F.Unidad)
    '            _CantidadAlbaranesVenta = oProducto.Entrada_Linea.Where(Function(F) F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranVenta).Sum(Function(F) F.Unidad)
    '            _CantidadPedidosCompra = oProducto.Entrada_Linea.Where(Function(F) F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.PedidoCompra).Sum(Function(F) F.Unidad - F.CantidadTraspasada)
    '            _CantidadPedidosVenta = oProducto.Entrada_Linea.Where(Function(F) F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.PedidoVenta).Sum(Function(F) F.Unidad - F.CantidadTraspasada)

    '            _DTRow("StockReal") = _CantidadAlbaranesCompra - _CantidadAlbaranesVenta
    '            _DTRow("StockPrevision") = _DTRow("StockReal") + _CantidadPedidosCompra - _CantidadPedidosVenta
    '        End If
    '        _DT.Rows.Add(_DTRow)
    '    Next

    '    Return _DT

    'End Function
End Class
