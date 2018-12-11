

Partial Public Class Entrada_Linea_NS

    Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
        Exit Sub
        Select Case action
            Case Data.Linq.ChangeAction.Insert
                Me.Entrada_Linea.Unidad = Me.Entrada_Linea.Entrada_Linea_NS.Count
                Me.Entrada_Linea.RecalculaCantidadNS()
            Case Data.Linq.ChangeAction.Delete
                'Me.Entrada_Linea.RecalculaCantidadNS()


                '  Me.Entrada_Linea.Cantidad = Me.Entrada_Linea.Entrada_Linea_NS.Count - 1
                ' Dim _Linea As Entrada_Linea = Me.Entrada_Linea
                ' _Linea.Cantidad = _Linea.Entrada_Linea_NS.Count - 1
                ' Me.Entrada_Linea.Recalcula()
            Case Data.Linq.ChangeAction.None


                'Dim a As Integer
                'a = Me.Entrada_Linea.Entrada_Linea_NS.Count
        End Select

    End Sub

    Private Sub OnID_Entrada_Linea_NSChanged()
        Dim a As Integer = Me.ID_NS
    End Sub

End Class

Partial Public Class Entrada_Linea

    Private Sub OnLoaded()

    End Sub

    Public Function RecalculaCantidadNS() As Integer
        Me.Unidad = Me.Entrada_Linea_NS.Count
        Return Me.Unidad
    End Function

    Public Function RetornaImporteHoras() As Decimal
        If Me.Parte_Horas.Count > 0 Then
            Return Me.Parte_Horas.Sum(Function(F) F.Horas * F.Personal.PrecioCoste)
        End If
    End Function

    Public Function RetornaImporteHorasExtras() As Decimal
        If Me.Parte_Horas.Count > 0 Then
            Return Me.Parte_Horas.Sum(Function(F) F.HorasExtras * Util.Comprobar_NULL_Per_0_Decimal(F.Personal.PrecioCosteHoraExtra))
        End If
    End Function

End Class

Partial Public Class Parte
    Public Function RetornaHoresRealizadas() As Decimal
        'Me.HorasRealizadas = Me.Parte_Horas.Sum(Function(F) F.Horas + F.HorasExtras)
        Return Util.Comprobar_NULL_Per_0_Decimal(Me.Parte_Horas.Sum(Function(F) F.Horas + F.HorasExtras))
    End Function

    Public Function RetornaHoresPendentsAssignarAUnAlbara() As Decimal
        'Me.HorasRealizadas = Me.Parte_Horas.Sum(Function(F) F.Horas + F.HorasExtras)
        Dim _Hores As Decimal = Util.Comprobar_NULL_Per_0_Decimal(Me.Parte_Horas.Where(Function(F) F.ID_Entrada_Linea.HasValue = False).Sum(Function(F) Util.Comprobar_NULL_Per_0_Decimal(F.Horas) + Util.Comprobar_NULL_Per_0_Decimal(F.HorasExtras)))
        Return _Hores
    End Function


    Public Function RetornaCosteImputadoMO() As Decimal
        'Me.HorasRealizadas = Me.Parte_Horas.Sum(Function(F) F.Horas + F.HorasExtras)
        Return Util.Comprobar_NULL_Per_0_Decimal(Me.Parte_Horas.Sum(Function(F) F.ID_Parte_Horas <> 0 And F.Horas * F.Parte.Parte_Asignacion.Where(Function(A) A.ID_Personal = F.ID_Personal).FirstOrDefault.PrecioCoste + F.HorasExtras * F.Parte.Parte_Asignacion.Where(Function(A) A.ID_Personal = F.ID_Personal).FirstOrDefault.PrecioCosteHoraExtra))
    End Function

    Public Function RetornaMargenMO() As Decimal
        Return Util.Comprobar_NULL_Per_0_Decimal(IIf(Me.CostePrevisto Is Nothing, 0, Me.CostePrevisto) - IIf(Me.CosteImputadoMO Is Nothing, 0, Me.CosteImputadoMO))
    End Function

    Public Function RetornaGastos() As Decimal
        Return Util.Comprobar_NULL_Per_0_Decimal(Me.Parte_Gastos.Sum(Function(F) F.Gasto))
    End Function

    Public Function RetornaMargenGastos() As Decimal
        Return Util.Comprobar_NULL_Per_0_Decimal(IIf(Me.CostePrevistoGastos Is Nothing, 0, Me.CostePrevistoGastos) - IIf(Me.CosteGastos Is Nothing, 0, Me.CosteGastos))
    End Function

    Public Function RetornaCostePrevistoTotal() As Decimal
        Return Util.Comprobar_NULL_Per_0_Decimal(Me.CostePrevistoMaterial) + Util.Comprobar_NULL_Per_0_Decimal(Me.CostePrevistoGastos) + Util.Comprobar_NULL_Per_0_Decimal(Me.CostePrevisto)
    End Function

    Public Function RetornaCosteTotal() As Decimal
        Return Util.Comprobar_NULL_Per_0_Decimal(Me.CosteImputadoMO) + Util.Comprobar_NULL_Per_0_Decimal(Me.CosteMaterial) + Util.Comprobar_NULL_Per_0_Decimal(Me.CosteGastos)
    End Function

    Public Function RetornaMargenTotal() As Decimal
        Return Util.Comprobar_NULL_Per_0_Decimal(Me.MargenMO) + Util.Comprobar_NULL_Per_0_Decimal(Me.MargenMaterial) + Util.Comprobar_NULL_Per_0_Decimal(Me.MargenGastos)
    End Function
End Class

Partial Public Class Propuesta

    Public Function RetornaTotalBase() As Decimal
        'Me.HorasRealizadas = Me.Parte_Horas.Sum(Function(F) F.Horas + F.HorasExtras)
        Return Util.Comprobar_NULL_Per_0_Decimal(Me.Propuesta_Linea.Sum(Function(F) F.TotalBase).Value)
    End Function

    Public Function RetornaTotalPropuesta() As Decimal
        Dim _ImporteBase As Decimal = Me.RetornaTotalBase
        Dim _PorcentageDescuento As Decimal = Me.Descuento
        Dim _ImporteDescuento As Decimal
        Dim _BaseMenosDescuento As Decimal
        ' Dim _PorcentageIVA As Decimal = Me.IVA
        Dim _ImporteIVA As Decimal
        Dim _TotalPropuesta As Decimal

        If Me.Descuento.HasValue = False OrElse Me.Descuento.Value = 0 Then
            _ImporteDescuento = 0
        Else
            _ImporteDescuento = Math.Round((_ImporteBase * _PorcentageDescuento) / 100, 2)
        End If

        _BaseMenosDescuento = Math.Round(_ImporteBase - _ImporteDescuento, 2)

        _ImporteIVA = Me.Propuesta_Linea.Sum(Function(F) F.TotalIVA).Value

        _TotalPropuesta = _BaseMenosDescuento + _ImporteIVA

        Return _TotalPropuesta
    End Function

    'Public Function RetornaMargenTotal() As Decimal
    '    Return Util.Comprobar_NULL_Per_0_Decimal(Me.MargenMO) + Util.Comprobar_NULL_Per_0_Decimal(Me.MargenMaterial) + Util.Comprobar_NULL_Per_0_Decimal(Me.MargenGastos)
    'End Function
End Class

Partial Public Class Entrada
    Public Function RetornaImporteBase() As Decimal
        If Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaCompra Or Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaVenta Or Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaCompraRectificativa Or Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaVentaRectificativa Then
            Return Me.Factura_Linea.Sum(Function(F) F.TotalBase)
        Else
            Return Me.Entrada_Linea.Sum(Function(F) F.TotalBase)
        End If
    End Function

    Public Function RetornaImporteDescuento() As Decimal
        If Me.Descuento.HasValue = False OrElse Me.Descuento = 0 Then
            Return 0
        End If

        Dim _Base As Decimal
        _Base = RetornaImporteBase()
        Return Math.Round((_Base * CDbl(Me.Descuento)) / 100, 2)
    End Function

    Public Function RetornaImporteIVA() As Decimal
        Dim _TotalIVA As Decimal
        If Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaCompra Or Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaVenta Or Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaCompraRectificativa Or Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaVentaRectificativa Then
            _TotalIVA = Math.Round(CDbl(Me.Factura_Linea.Sum(Function(F) F.TotalIVA)), 2)
            If Me.Descuento.HasValue = True AndAlso Me.Descuento.Value <> 0 Then
                _TotalIVA = _TotalIVA - Math.Round((_TotalIVA * CDbl(Me.Descuento)) / 100, 2)
            End If
        Else
            _TotalIVA = Math.Round(CDbl(Me.Entrada_Linea.Sum(Function(F) F.TotalIVA)), 2)
            If Me.Descuento.HasValue = True AndAlso Me.Descuento.Value <> 0 Then
                _TotalIVA = _TotalIVA - Math.Round((_TotalIVA * CDbl(Me.Descuento)) / 100, 2)
            End If
        End If
        Return _TotalIVA
    End Function

    Public Function RetornaImporteTotal()
        Dim _Base As Decimal = RetornaImporteBase()
        Dim _IVA As Decimal
        Dim _BaseReal As Decimal
        _BaseReal = _Base - CDbl(RetornaImporteDescuento())
        _IVA = RetornaImporteIVA()
        Return _BaseReal + _IVA
    End Function
End Class



'Partial Public Class RetornaStockResult
'    Public Function RetornaImportePVP() As Decimal
'        Dim _PVP As Decimal = BD.RetornaValorSQL("Select isnull(PVP,0) From Producto Where ID_Producto=" & Me.ID_Producto)
'        Return Util.Comprobar_NULL_Per_0(Me.StockReal * _PVP)

'        'If Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaCompra Or Me.ID_Entrada_Tipo = EnumEntradaTipo.FacturaVenta Then
'        '    Return Me.Factura_Linea.Sum(Function(F) F.TotalBase)
'        'Else
'        '    Return Me.Entrada_Linea.Sum(Function(F) F.TotalBase)
'        'End If
'    End Function
'End Class


'Partial Public Class Parte_Horas
'    Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
'        Select Case action
'            Case Data.Linq.ChangeAction.Insert
'                'Me.Entrada_Linea.Unidad = Me.Entrada_Linea.Entrada_Linea_NS.Count
'                'Me.Parte.RecalculaHorasRealizadas()
'            Case Data.Linq.ChangeAction.Delete
'                'Me.Entrada_Linea.RecalculaCantidadNS()


'                '  Me.Entrada_Linea.Cantidad = Me.Entrada_Linea.Entrada_Linea_NS.Count - 1
'                ' Dim _Linea As Entrada_Linea = Me.Entrada_Linea
'                ' _Linea.Cantidad = _Linea.Entrada_Linea_NS.Count - 1
'                ' Me.Entrada_Linea.Recalcula()
'            Case Data.Linq.ChangeAction.Update
'                'Me.Parte.RecalculaHorasRealizadas()


'                'Dim a As Integer
'                'a = Me.Entrada_Linea.Entrada_Linea_NS.Count
'        End Select
'    End Sub

'    'Private Sub OnID_Entrada_Linea_NSChanged()
'    '    Dim a As Integer = Me.ID_NS
'    'End Sub
'End Class
