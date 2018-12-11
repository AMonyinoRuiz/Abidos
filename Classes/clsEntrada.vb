Public Class clsEntrada
    Dim oDTC As DTCDataContext
    Public oEntrada As Entrada

    Public Sub New(ByRef pDTC As DTCDataContext, ByRef pEntrada As Entrada)
        oDTC = pDTC
        oEntrada = pEntrada
    End Sub

    Public Sub New(ByRef pDTC As DTCDataContext)
        oDTC = pDTC
    End Sub

    Public Function TraspasarDeComandaAAlbaraCompra(ByRef pLiniesSeleccionades As ArrayList, ByRef pNSSeleccionats As ArrayList, ByVal pNumDocumentoProveedor As String, ByVal pIDAlbaraDesti As Integer) As Integer
        'Si retorna 0 és que no s'ha creat el nou albarà, si retorna diferent a 0 és el número ID Entrada creada
        Try
            TraspasarDeComandaAAlbaraCompra = 0
            Dim _NewEntrada As Entrada


            If pIDAlbaraDesti = 0 Then 'Si no em passen un albarà destí crearem un nou albarà
                _NewEntrada = DuplicarEntrada()
                With _NewEntrada
                    .Codigo = RetornaCodiSeguent(EnumEntradaTipo.AlbaranCompra)
                    .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Pendiente)).FirstOrDefault
                    .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = CInt(EnumEntradaTipo.AlbaranCompra)).FirstOrDefault
                    .FechaEntrada = Now.Date
                    .FechaAlta = Now.Date
                    .Observaciones = ""
                    .Descripcion = RetornaDescripcioDocumentEnFuncioDelSeuTipus(EnumEntradaTipo.AlbaranCompra, .Codigo)
                    .NumeroDocumentoProveedor = pNumDocumentoProveedor

                    '.Entrada_Archivo
                    '.Entrada_Instalacion
                    '.Entrada_Parte
                    '.Entrada_Propuesta
                End With
            Else 'Si me'l passen el buscarem i apart d'afegir-li les noves línies li cambiarem el número de documento de proveedor
                _NewEntrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = pIDAlbaraDesti).FirstOrDefault
                _NewEntrada.NumeroDocumentoProveedor = pNumDocumentoProveedor
            End If



            Dim _pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
            Dim _pRowNS As Infragistics.Win.UltraWinGrid.UltraGridRow

            For Each _pRow In pLiniesSeleccionades
                Dim _CantidadATraspasar As Integer = _pRow.Cells("CantidadATraspasar").Value
                Dim _LineaEntrada As Entrada_Linea = oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(_pRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                Dim _ClsLinea As New clsEntradaLinea(_NewEntrada, Nothing, oDTC)
                clsEntradaLinea.DuplicaLineaEntrada(_LineaEntrada, _ClsLinea.oLinqLinea)

                With _ClsLinea.oLinqLinea
                    '.Almacen = _LineaEntrada.Almacen
                    .Unidad = _CantidadATraspasar
                    .ID_Entrada_Linea_Pedido = _LineaEntrada.ID_Entrada_Linea
                    .FechaEntrada = Now.Date
                    'If IsDBNull(_LineaEntrada.FechaEntrega) Then
                    '    .FechaEntrega = Now.Date
                    'Else
                    '    .FechaEntrega = _LineaEntrada.FechaEntrega
                    'End If
                    '.Precio = _LineaEntrada.Precio
                    '.Descuento1 = _LineaEntrada.Descuento1
                    '.Descuento2 = _LineaEntrada.Descuento2
                    '.IVA = _LineaEntrada.IVA
                    '.Producto = _LineaEntrada.Producto
                    .StockActivo = True
                End With

                If _ClsLinea.AfegirLinea(False) = True Then
                    _LineaEntrada.CantidadTraspasada = Util.Comprobar_NULL_Per_0_Decimal(_LineaEntrada.CantidadTraspasada) + _CantidadATraspasar
                    For Each _pRowNS In pNSSeleccionats
                        If _pRowNS.Cells("ID_Entrada_Linea").Value = _LineaEntrada.ID_Entrada_Linea Then
                            _ClsLinea.LineaNSCrear(oDTC.NS.Where(Function(F) F.ID_NS = CInt(_pRowNS.Cells("ID_NS").Value)).FirstOrDefault.ID_NS)
                        End If
                    Next
                End If
            Next

            'Si no s'ha afegit cap línea no farem el traspàs
            If _NewEntrada.Entrada_Linea.Count = 0 Then
                Mensaje.Mostrar_Mensaje("Imposible crear el albarán, no hay ninguna línea seleccionada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            Else
                If EstaLaComandaTotalmentTraspasada() = True OrElse Mensaje.Mostrar_Mensaje("¿Desea cerrar el pedido?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
                    oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault
                Else
                    oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
                End If

            End If

            If pIDAlbaraDesti = 0 Then 'Si no em passen un albarà destí crearem un nou albarà
                oDTC.Entrada.InsertOnSubmit(_NewEntrada)
            End If
            oDTC.SubmitChanges()
            Return _NewEntrada.ID_Entrada
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function TraspasarDeComandaAAlbaraVenta(ByRef pLiniesSeleccionades As ArrayList, ByVal pNSSeleccionats(,) As Integer, ByVal pIDAlbaraDesti As Integer) As Integer
        'Si retorna 0 és que no s'ha creat el nou albarà, si retorna diferent a 0 és el número ID Entrada creada
        Try
            TraspasarDeComandaAAlbaraVenta = 0
            Dim _NewEntrada As Entrada

            If pIDAlbaraDesti = 0 Then 'Si no em passen un albarà destí crearem un nou albarà
                _NewEntrada = DuplicarEntrada()

                With _NewEntrada
                    .Codigo = RetornaCodiSeguent(EnumEntradaTipo.AlbaranVenta)
                    .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Pendiente)).FirstOrDefault
                    .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = CInt(EnumEntradaTipo.AlbaranVenta)).FirstOrDefault
                    .FechaEntrada = Now.Date
                    .FechaAlta = Now.Date
                    .Observaciones = ""
                    .Descripcion = RetornaDescripcioDocumentEnFuncioDelSeuTipus(EnumEntradaTipo.AlbaranVenta, .Codigo)
                End With
            Else
                _NewEntrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = pIDAlbaraDesti).FirstOrDefault
            End If


            Dim _pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

            For Each _pRow In pLiniesSeleccionades
                Dim _CantidadATraspasar As Decimal = _pRow.Cells("CantidadATraspasar").Value
                Dim _LineaEntrada As Entrada_Linea = oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(_pRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                Dim _ClsLinea As New clsEntradaLinea(_NewEntrada, Nothing, oDTC)
                clsEntradaLinea.DuplicaLineaEntrada(_LineaEntrada, _ClsLinea.oLinqLinea)
                With _ClsLinea.oLinqLinea
                    .Unidad = _CantidadATraspasar
                    .ID_Entrada_Linea_Pedido = _LineaEntrada.ID_Entrada_Linea

                    .FechaEntrada = Now.Date
                    'If IsDBNull(_LineaEntrada.FechaEntrega) Then
                    '    .FechaEntrega = Now.Date
                    'Else
                    '    .FechaEntrega = _LineaEntrada.FechaEntrega
                    'End If

                    '.Precio = _LineaEntrada.Precio
                    '.Descuento1 = _LineaEntrada.Descuento1
                    '.Descuento2 = _LineaEntrada.Descuento2
                    '.IVA = _LineaEntrada.IVA
                    '.Producto = _LineaEntrada.Producto
                    .StockActivo = True
                End With

                If _ClsLinea.AfegirLinea(False) = True Then
                    _LineaEntrada.CantidadTraspasada = Util.Comprobar_NULL_Per_0_Decimal(_LineaEntrada.CantidadTraspasada) + _CantidadATraspasar
                    If _ClsLinea.pRequiereNS = True And _pRow.Cells("NoRestarStock").Value = True Then
                        'Dim _NumNS As Integer
                        '_NumNS = Mensaje.Mostrar_Entrada_Datos("Introduzca el número de números de serie virtuales a generar", "1", False)

                        'Dim _ID_Linea As Integer
                        '_ID_Linea = Me.GRD_Linea.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value
                        'Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = _ID_Linea).FirstOrDefault
                        'Dim oclsEntradaLinea As New clsEntradaLinea(oclsEntrada.oEntrada, _Linea, oDTC)

                        For i = 1 To _CantidadATraspasar
                            _ClsLinea.LineaNSCrear(, , True)
                        Next
                        'oDTC.SubmitChanges()
                    Else
                        Dim i As Integer
                        For i = 0 To 10000
                            If pNSSeleccionats(i, 0) = _LineaEntrada.ID_Entrada_Linea Then
                                _ClsLinea.LineaNSCrear(oDTC.NS.Where(Function(F) F.ID_NS = pNSSeleccionats(i, 1)).FirstOrDefault.ID_NS)
                            End If
                        Next
                    End If

                End If




            Next

            'Si no s'ha afegit cap línea no farem el traspàs
            If _NewEntrada.Entrada_Linea.Count = 0 Then
                Mensaje.Mostrar_Mensaje("Imposible crear el albarán, no hay ninguna línea seleccionada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            Else


                If EstaLaComandaTotalmentTraspasada() = True OrElse Mensaje.Mostrar_Mensaje("¿Desea cerrar el pedido?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
                    oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault
                Else
                    oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
                End If

            End If

            If pIDAlbaraDesti = 0 Then 'Si no em passen un albarà destí crearem un nou albarà
                oDTC.Entrada.InsertOnSubmit(_NewEntrada)
            End If

            oDTC.SubmitChanges()
            Return _NewEntrada.ID_Entrada

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function FacturarAlbaraCompra(ByRef pLiniesSeleccionades As ArrayList, ByVal pNumDocumentoProveedor As String, Optional ByVal pIDFacturaDesti As Integer = 0, Optional ByVal pIDEmpresa As Integer = 0) As Integer
        'Si retorna 0 és que no s'ha creat la factura, si retorna diferent a 0 és el número ID Entrada corresponent a la factura
        Try
            FacturarAlbaraCompra = 0

            Dim _NewEntrada As Entrada

            If pIDFacturaDesti = 0 Then
                _NewEntrada = DuplicarEntrada()
                With _NewEntrada

                    .Codigo = RetornaCodiSeguent(TransformaOrigenDesti(oEntrada.ID_Entrada_Tipo), pIDEmpresa)
                    .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Pendiente)).FirstOrDefault
                    .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = CInt(TransformaOrigenDesti(oEntrada.ID_Entrada_Tipo))).FirstOrDefault
                    .FechaEntrada = Now.Date
                    .FechaAlta = Now.Date
                    .Descripcion = clsEntrada.RetornaDescripcioDocumentEnFuncioDelSeuTipus(TransformaOrigenDesti(oEntrada.ID_Entrada_Tipo), .Codigo)
                    .Observaciones = ""
                    .NumeroDocumentoProveedor = pNumDocumentoProveedor
                    Dim _IDEmpresa As Integer = pIDEmpresa
                    .Empresa = oDTC.Empresa.Where(Function(F) F.ID_Empresa = _IDEmpresa).FirstOrDefault
                End With
            Else
                _NewEntrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = pIDFacturaDesti).FirstOrDefault
                _NewEntrada.NumeroDocumentoProveedor = pNumDocumentoProveedor
            End If


            'Dim _Linea As Entrada_Linea
            'For Each _Linea In oEntrada.Entrada_Linea
            '    _Linea.Factura = _NewEntrada
            'Next

            Dim _pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
            For Each _pRow In pLiniesSeleccionades
                Dim _LineaEntrada As Entrada_Linea = oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(_pRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                _LineaEntrada.Factura = _NewEntrada
            Next

            If oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Factura.HasValue = False).Count > 0 Then
                oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
            Else
                oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault
            End If

            If pIDFacturaDesti = 0 Then 'Si no em passen un albarà destí crearem un nou albarà
                oDTC.Entrada.InsertOnSubmit(_NewEntrada)
            End If

            _NewEntrada.Base = _NewEntrada.RetornaImporteBase
            _NewEntrada.Total = _NewEntrada.RetornaImporteTotal

            oDTC.SubmitChanges()
            Return _NewEntrada.ID_Entrada
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Function TransformaOrigenDesti(ByVal pTipusDocument As EnumEntradaTipo) As EnumEntradaTipo
        Select Case pTipusDocument
            Case EnumEntradaTipo.AlbaranCompra
                Return EnumEntradaTipo.FacturaCompra
            Case EnumEntradaTipo.DevolucionCompra
                Return EnumEntradaTipo.FacturaCompraRectificativa
            Case EnumEntradaTipo.AlbaranVenta
                Return EnumEntradaTipo.FacturaVenta
            Case EnumEntradaTipo.DevolucionVenta
                Return EnumEntradaTipo.FacturaVentaRectificativa
        End Select
    End Function

    Public Function FacturarAlbaraCompraMultiple(ByRef pLiniesSeleccionades As ArrayList, ByVal pIDFormaPago As Integer, ByVal pDiaPago As Integer, ByVal pNumDocumentoProveedor As String, Optional ByVal pIDEmpresa As Integer = 0) As Integer
        'Si retorna 0 és que no s'ha creat la factura, si retorna diferent a 0 és el número ID Entrada corresponent a la factura
        Try
            FacturarAlbaraCompraMultiple = 0

            Dim _NewEntrada As Entrada

            _NewEntrada = DuplicarEntrada()
            With _NewEntrada

                .Codigo = RetornaCodiSeguent(EnumEntradaTipo.FacturaCompra, pIDEmpresa)

                .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Pendiente)).FirstOrDefault
                .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = CInt(EnumEntradaTipo.FacturaCompra)).FirstOrDefault
                .FechaEntrada = Now.Date
                .FechaAlta = Now.Date
                .Descripcion = clsEntrada.RetornaDescripcioDocumentEnFuncioDelSeuTipus(EnumEntradaTipo.FacturaCompra, .Codigo)
                .Observaciones = ""
                .FormaPago = oDTC.FormaPago.Where(Function(F) F.ID_FormaPago = CInt(pIDFormaPago)).FirstOrDefault
                .DiaDePago = pDiaPago
                .NumeroDocumentoProveedor = pNumDocumentoProveedor
                Dim _IDEmpresa As Integer = pIDEmpresa
                .Empresa = oDTC.Empresa.Where(Function(F) F.ID_Empresa = _IDEmpresa).FirstOrDefault
            End With

            Dim _IDEntradaLinea As Integer
            For Each _IDEntradaLinea In pLiniesSeleccionades
                Dim _Entrada As Entrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = _IDEntradaLinea).FirstOrDefault
                Dim _LineaEntrada As Entrada_Linea
                For Each _LineaEntrada In _Entrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Factura.HasValue = False)
                    _LineaEntrada.Factura = _NewEntrada

                    'Totes les línies de compuesto por tb les facturarem
                    Dim _LineaCompuestoPor As Entrada_Linea
                    For Each _LineaCompuestoPor In _LineaEntrada.Entrada_Linea_Hijo
                        _LineaCompuestoPor.Factura = _NewEntrada
                    Next
                Next

                'If oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Factura.HasValue = False).Count > 0 Then
                '    oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
                'Else
                _Entrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault
                'End If
            Next

            oDTC.Entrada.InsertOnSubmit(_NewEntrada)

            _NewEntrada.Base = _NewEntrada.RetornaImporteBase
            _NewEntrada.Total = _NewEntrada.RetornaImporteTotal

            oDTC.SubmitChanges()
            Return _NewEntrada.ID_Entrada



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function FacturarAlbaraVenta(ByRef pLiniesSeleccionades As ArrayList, Optional ByVal pIDFacturaDesti As Integer = 0, Optional ByVal pNumFactura As Integer = 0, Optional ByVal pIDEmpresa As Integer = 0) As Integer
        'Si retorna 0 és que no s'ha creat la factura, si retorna diferent a 0 és el número ID Entrada corresponent a la factura
        Try
            FacturarAlbaraVenta = 0

            Dim _NewEntrada As Entrada

            If pIDFacturaDesti = 0 Then
                _NewEntrada = DuplicarEntrada()
                With _NewEntrada
                    If pNumFactura = 0 Then
                        .Codigo = RetornaCodiSeguent(TransformaOrigenDesti(oEntrada.ID_Entrada_Tipo), pIDEmpresa)
                    Else
                        .Codigo = pNumFactura
                    End If

                    .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Pendiente)).FirstOrDefault
                    .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = CInt(TransformaOrigenDesti(oEntrada.ID_Entrada_Tipo))).FirstOrDefault
                    .FechaEntrada = Now.Date
                    .FechaAlta = Now.Date
                    .Descripcion = clsEntrada.RetornaDescripcioDocumentEnFuncioDelSeuTipus(TransformaOrigenDesti(oEntrada.ID_Entrada_Tipo), .Codigo)
                    .Observaciones = ""
                    Dim _IDEmpresa As Integer = pIDEmpresa
                    .Empresa = oDTC.Empresa.Where(Function(F) F.ID_Empresa = _IDEmpresa).FirstOrDefault
                End With
            Else
                _NewEntrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = pIDFacturaDesti).FirstOrDefault
            End If


            'Dim _Linea As Entrada_Linea
            'For Each _Linea In oEntrada.Entrada_Linea
            '    _Linea.Factura = _NewEntrada
            'Next

            Dim _pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
            For Each _pRow In pLiniesSeleccionades
                Dim _LineaEntrada As Entrada_Linea = oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(_pRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                _LineaEntrada.Factura = _NewEntrada

                'Totes les línies de compuesto por tb les facturarem
                Dim _LineaCompuestoPor As Entrada_Linea
                For Each _LineaCompuestoPor In _LineaEntrada.Entrada_Linea_Hijo
                    _LineaCompuestoPor.Factura = _NewEntrada
                Next
            Next

            If oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Factura.HasValue = False).Count > 0 Then
                oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
            Else
                oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault
            End If

            If pIDFacturaDesti = 0 Then 'Si no em passen un albarà destí crearem un nou albarà
                oDTC.Entrada.InsertOnSubmit(_NewEntrada)
            End If

            _NewEntrada.Base = _NewEntrada.RetornaImporteBase
            _NewEntrada.Total = _NewEntrada.RetornaImporteTotal

            oDTC.SubmitChanges()
            Return _NewEntrada.ID_Entrada





            '    FacturarAlbaraVenta = 0
            '    Dim _NewEntrada As Entrada = DuplicarEntrada()
            '    With _NewEntrada
            '        .Codigo = RetornaCodiSeguent(EnumEntradaTipo.FacturaVenta)
            '        .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Cerrado)).FirstOrDefault
            '        .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = CInt(EnumEntradaTipo.FacturaVenta)).FirstOrDefault
            '        .FechaEntrada = Now.Date
            '        .FechaAlta = Now.Date
            '        .Descripcion = clsEntrada.RetornaDescripcioDocumentEnFuncioDelSeuTipus(EnumEntradaTipo.FacturaVenta, .Codigo)
            '        .Observaciones = ""

            '    End With

            '    Dim _Linea As Entrada_Linea

            '    For Each _Linea In oEntrada.Entrada_Linea
            '        _Linea.Factura = _NewEntrada
            '    Next

            '    oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault

            '    oDTC.Entrada.InsertOnSubmit(_NewEntrada)
            '    oDTC.SubmitChanges()
            '    Return _NewEntrada.ID_Entrada
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function FacturarAlbaraVentaMultiple(ByRef pLiniesSeleccionades As ArrayList, ByVal pIDFormaPago As Integer, ByVal pDiaPago As Integer, Optional ByVal pNumFactura As Integer = 0, Optional ByVal pIDEmpresa As Integer = 0) As Integer
        'Si retorna 0 és que no s'ha creat la factura, si retorna diferent a 0 és el número ID Entrada corresponent a la factura
        Try
            FacturarAlbaraVentaMultiple = 0

            Dim _NewEntrada As Entrada

            _NewEntrada = DuplicarEntrada()
            With _NewEntrada
                If pNumFactura = 0 Then
                    .Codigo = RetornaCodiSeguent(EnumEntradaTipo.FacturaVenta, pIDEmpresa)
                Else
                    .Codigo = pNumFactura
                End If

                .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Pendiente)).FirstOrDefault
                .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = CInt(EnumEntradaTipo.FacturaVenta)).FirstOrDefault
                .FechaEntrada = Now.Date
                .FechaAlta = Now.Date
                .Descripcion = clsEntrada.RetornaDescripcioDocumentEnFuncioDelSeuTipus(EnumEntradaTipo.FacturaVenta, .Codigo)
                .Observaciones = ""
                .FormaPago = oDTC.FormaPago.Where(Function(F) F.ID_FormaPago = CInt(pIDFormaPago)).FirstOrDefault
                .DiaDePago = pDiaPago
                Dim _IDEmpresa As Integer = pIDEmpresa
                .Empresa = oDTC.Empresa.Where(Function(F) F.ID_Empresa = _IDEmpresa).FirstOrDefault
            End With

            Dim _IDEntradaLinea As Integer
            For Each _IDEntradaLinea In pLiniesSeleccionades
                Dim _Entrada As Entrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = _IDEntradaLinea).FirstOrDefault
                Dim _LineaEntrada As Entrada_Linea
                For Each _LineaEntrada In _Entrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Factura.HasValue = False)
                    _LineaEntrada.Factura = _NewEntrada

                    'Totes les línies de compuesto por tb les facturarem
                    Dim _LineaCompuestoPor As Entrada_Linea
                    For Each _LineaCompuestoPor In _LineaEntrada.Entrada_Linea_Hijo
                        _LineaCompuestoPor.Factura = _NewEntrada
                    Next
                Next

                'If oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Factura.HasValue = False).Count > 0 Then
                '    oEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
                'Else
                _Entrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault
                'End If
            Next

            oDTC.Entrada.InsertOnSubmit(_NewEntrada)

            _NewEntrada.Base = _NewEntrada.RetornaImporteBase
            _NewEntrada.Total = _NewEntrada.RetornaImporteTotal

            oDTC.SubmitChanges()
            Return _NewEntrada.ID_Entrada



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function


    Private Function DuplicarEntrada() As Entrada
        Dim _NewEntrada As New Entrada
        With _NewEntrada
            .Almacen = oEntrada.Almacen
            .Campaña = oEntrada.Campaña
            .Cliente = oEntrada.Cliente
            .Empresa = oEntrada.Empresa
            .Cliente_Direccion = oEntrada.Cliente_Direccion
            .Cliente_Email = oEntrada.Cliente_Email
            .Cliente_Nif = oEntrada.Cliente_Nif
            .Cliente_PersonaContacto = oEntrada.Cliente_PersonaContacto
            .Cliente_Poblacion = oEntrada.Cliente_Poblacion
            .Cliente_Provincia = oEntrada.Cliente_Provincia
            .Cliente_Telefono = oEntrada.Cliente_Telefono
            .Cliente_CP = oEntrada.Cliente_CP
            .Codigo = oEntrada.Codigo
            .CompañiaTransporte = oEntrada.CompañiaTransporte
            .Descripcion = oEntrada.Descripcion
            .Entrada_Estado = oEntrada.Entrada_Estado
            .Entrada_Origen = oEntrada.Entrada_Origen
            .Entrada_Tipo = oEntrada.Entrada_Tipo
            .FechaAlta = oEntrada.FechaAlta
            .FechaEntrada = oEntrada.FechaEntrada
            .NombreObra = oEntrada.NombreObra
            .NumAsiento = oEntrada.NumAsiento
            .NumeroSeguimiento = oEntrada.NumeroSeguimiento
            .NumPedidoCliente = oEntrada.NumPedidoCliente
            .NumReferencia = oEntrada.NumReferencia
            .Observaciones = oEntrada.Observaciones
            .Personal = oEntrada.Personal
            .Proveedor = oEntrada.Proveedor
            .Proveedor_Direccion = oEntrada.Proveedor_Direccion
            .Proveedor_Email = oEntrada.Proveedor_Email
            .Proveedor_Nif = oEntrada.Proveedor_Nif
            .Proveedor_PersonaContacto = oEntrada.Proveedor_PersonaContacto
            .Proveedor_Poblacion = oEntrada.Proveedor_Poblacion
            .Proveedor_Provincia = oEntrada.Proveedor_Provincia
            .Proveedor_Telefono = oEntrada.Proveedor_Telefono
            .Proveedor_CP = oEntrada.Proveedor_CP
            .Proyecto = oEntrada.Proyecto
            .FormaPago = oEntrada.FormaPago
            .DiaDePago = oEntrada.DiaDePago

            .Base = oEntrada.Base
            .IVA = oEntrada.IVA
            .Descuento = oEntrada.Descuento
            .Total = oEntrada.Total
            .Facturable = oEntrada.Facturable

            Dim _Instalacions As Entrada_Instalacion
            For Each _Instalacions In oEntrada.Entrada_Instalacion
                Dim _NewInstalacio As New Entrada_Instalacion
                _NewInstalacio.Instalacion = _Instalacions.Instalacion
                _NewEntrada.Entrada_Instalacion.Add(_NewInstalacio)
            Next

            Dim _Propuesta As Entrada_Propuesta
            For Each _Propuesta In oEntrada.Entrada_Propuesta
                Dim _NewPropuesta As New Entrada_Propuesta
                _NewPropuesta.Propuesta = _Propuesta.Propuesta
                _NewEntrada.Entrada_Propuesta.Add(_NewPropuesta)
            Next

            Dim _Parte As Entrada_Parte
            For Each _Parte In oEntrada.Entrada_Parte
                Dim _NewParte As New Entrada_Parte
                _NewParte.Parte = _Parte.Parte
                _NewEntrada.Entrada_Parte.Add(_NewParte)
            Next

            Dim _EntradaArchivo As Entrada_Archivo
            For Each _EntradaArchivo In oEntrada.Entrada_Archivo
                Dim _NewArchivo As New Entrada_Archivo
                _NewArchivo.Archivo = clsFichero.DuplicaFichero(_EntradaArchivo.Archivo)
                _NewEntrada.Entrada_Archivo.Add(_NewArchivo)
            Next


        End With
        Return _NewEntrada
    End Function

    Public Function Eliminar() As Boolean
        Try
            Eliminar = False

            'If EsUnDocumentAnteriorALaUltimaInicialitzacioStocks() = True Then
            '    If Mensaje.Mostrar_Mensaje("Este documento es anterior a la última inicialización de stocks. Si modifica este documento se eliminará la inicialización de stocks. ¿Desea continuar?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.NO Then
            '        Exit Function
            '    Else
            '        If EliminarUltimaInicialitzacioStocks() = False Then
            '            Mensaje.Mostrar_Mensaje("Imposible modificar datos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '            Exit Function
            '        End If
            '    End If
            'End If

            Select Case DirectCast(oEntrada.ID_Entrada_Tipo, EnumEntradaTipo)
                Case EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaVentaRectificativa, EnumEntradaTipo.FacturaCompraRectificativa
                    Dim _Linea As Entrada_Linea
                    For Each _Linea In oEntrada.Factura_Linea
                        Dim _clsLinea As New clsEntradaLinea(oEntrada, _Linea, oDTC)
                        _clsLinea.DesfacturarLinea()
                    Next
                Case Else
                    Dim _Linea As Entrada_Linea
                    For Each _Linea In oEntrada.Entrada_Linea
                        Dim _clsLinea As New clsEntradaLinea(oEntrada, _Linea, oDTC)
                        _clsLinea.EliminarLinea()
                    Next
            End Select

            'Eliminem totes les instalacions assignades al document
            oDTC.Entrada_Instalacion.DeleteAllOnSubmit(oEntrada.Entrada_Instalacion)

            'Eliminem tots els partes assignats al document
            oDTC.Entrada_Parte.DeleteAllOnSubmit(oEntrada.Entrada_Parte)

            'Eliminem totes les propostes assignades al document
            oDTC.Entrada_Propuesta.DeleteAllOnSubmit(oEntrada.Entrada_Propuesta)

            'Eliminem tots els seguiments assignats al document
            oDTC.Entrada_Seguimiento.DeleteAllOnSubmit(oEntrada.Entrada_Seguimiento)

            'Eliminem tots els venciments assignats al document
            oDTC.Entrada_Vencimiento.DeleteAllOnSubmit(oEntrada.Entrada_Vencimiento)

            Dim _Archivo As Entrada_Archivo
            For Each _Archivo In oEntrada.Entrada_Archivo
                oDTC.Archivo.DeleteOnSubmit(_Archivo.Archivo)
                oDTC.Entrada_Archivo.DeleteOnSubmit(_Archivo)
            Next



            'Select Case oEntrada.ID_Entrada_Tipo
            '    Case EnumEntradaTipo.PedidoCompra, EnumEntradaTipo.Inicializacion
            '        Dim _Linea As Entrada_Linea
            '        For Each _Linea In oEntrada.Entrada_Linea
            '            Dim _clsLinea As New clsEntradaLinea(oEntrada, _Linea, oDTC)
            '            _clsLinea.EliminarLinea()


            '            Dim _LineaNS As Entrada_Linea_NS
            '            For Each _LineaNS In _Linea.Entrada_Linea_NS
            '                _clsLinea.LineaNSEliminar(_LineaNS)
            '            Next

            '            'oDTC.Entrada_Linea_NS.DeleteAllOnSubmit(_Linea.Entrada_Linea_NS)
            '            'oDTC.NS.DeleteAllOnSubmit(From A In _Linea.Entrada_Linea_NS Where A.ID_Entrada_Linea <> 0 Select A.NS)
            '            oDTC.Entrada_Linea.DeleteOnSubmit(_Linea)
            '        Next

            '    Case EnumEntradaTipo.AlbaranCompra
            '        Dim _Linea As Entrada_Linea
            '        For Each _Linea In oEntrada.Entrada_Linea
            '            Dim _NS As Entrada_Linea_NS
            '            For Each _NS In _Linea.Entrada_Linea_NS
            '                ojo _NS.NS.NS_Estado = oDTC.NS_Estado.Where(Function(F) F.ID_NS_Estado = clsEntrada.RetornaEstatQueHauriaDeTenirElNSalCrearloSegonsPantalla(EnumEntradaTipo.Pedido)).FirstOrDefault
            '            Next

            '            oDTC.Entrada_Linea_NS.DeleteAllOnSubmit(_Linea.Entrada_Linea_NS)


            '            Treiem de la linea de comanda relacionada la quantitat traspasada
            '            If _Linea.Entrada_Linea_Albaran Is Nothing = False Then 'Si la linea d'albarà prové d'una comanda
            '                _Linea.Entrada_Linea_Albaran.CantidadTraspasada = _Linea.Entrada_Linea_Albaran.CantidadTraspasada - _Linea.Cantidad
            '            End If
            '            oDTC.Entrada_Linea.DeleteOnSubmit(_Linea)
            '        Next

            '        If oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea_Pedido.HasValue).Count > 0 Then 'si hi ha alguna linea que provingui d'una comanda
            '            Dim _Pedido As Entrada = oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea_Pedido.HasValue).FirstOrDefault.Entrada_Linea_Albaran.Entrada
            '            Call ComandaComprovarICanviarEstat(_Pedido, oDTC)
            '        End If

            '    Case EnumEntradaTipo.FacturaCompra

            '        If oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea_Pedido.HasValue).Count > 0 Then
            '            Dim _Albara As Entrada = oEntrada.Factura_Linea.Where(Function(F) F.ID_Entrada_Factura.HasValue).FirstOrDefault.Entrada
            '            If _Albara.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
            '                If Mensaje.Mostrar_Mensaje("Este albarán contiene líneas que provienen de un pedido cerrado. Desea abrir el pedido relacionado?", M_Mensaje.Missatge_Modo.PREGUNTA, , , True) = M_Mensaje.Botons.SI Then
            '                    _Albara.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Pendiente).FirstOrDefault
            '                End If
            '            End If
            '        End If

            '        Dim _Linea As Entrada_Linea
            '        For Each _Linea In _Albara.Entrada_Linea
            '            _Linea.Factura = Nothing
            '        Next


            'End Select

            'If oLinqEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea_Pedido.HasValue).Count > 0 Then
            '    Dim _Pedido As Entrada = oLinqEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea_Pedido.HasValue).FirstOrDefault.Entrada_Linea_Albaran.Entrada
            '    If _Pedido.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
            '        If Mensaje.Mostrar_Mensaje("Este albaran contiene líneas que provienen de un pedido cerrado. Desea abrir el pedido relacionado?", M_Mensaje.Missatge_Modo.PREGUNTA, , , True) = M_Mensaje.Botons.SI Then
            '            _Pedido.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
            '        End If
            '    End If
            'End If

            oDTC.Entrada.DeleteOnSubmit(oEntrada)
            oDTC.SubmitChanges()
            Return True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function EsUnDocumentAnteriorALaUltimaInicialitzacioStocks() As Boolean
        If oEntrada.Entrada_Linea.Where(Function(F) F.StockActivo = False).Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function EliminarUltimaInicialitzacioStocks() As Boolean
        Try
            EliminarUltimaInicialitzacioStocks = False
            'Agafem la primera línea del document
            Dim _Primeralinea As Entrada_Linea = oEntrada.Entrada_Linea.FirstOrDefault

            If _Primeralinea Is Nothing Then
                Exit Function
            End If

            'Busquem tots els documents d'inicialització posteriors a la fecha d'entrada de la primera línea del document que volem modificar
            Dim _InicialitzacionsAnteriors As IEnumerable(Of Entrada) = oDTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = EnumEntradaTipo.Inicializacion And F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado And F.FechaEntrada >= _Primeralinea.FechaEntrada)

            Dim _Entrada As Entrada
            Dim _Linea As Entrada_Linea

            For Each _Entrada In _InicialitzacionsAnteriors
                Dim _LineasAssignadesALaInicialitzacio As IQueryable(Of Entrada_Linea) = oDTC.Entrada_Linea.Where(Function(F) F.ID_DocumentoInicializacionStocks = _Entrada.ID_Entrada)
                For Each _Linea In _LineasAssignadesALaInicialitzacio
                    _Linea.StockActivo = True
                    _Linea.ID_DocumentoInicializacionStocks = Nothing
                Next
                oDTC.Entrada_Linea.DeleteAllOnSubmit(_Entrada.Entrada_Linea)

                oDTC.Entrada.DeleteOnSubmit(_Entrada)
            Next

            oDTC.SubmitChanges()
            EliminarUltimaInicialitzacioStocks = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Function ComprovacioQuantitatLineaAmbNumeroSerie(ByVal pIDEntrada As Integer) As Boolean
        Try
            ComprovacioQuantitatLineaAmbNumeroSerie = True
            Dim oDTC As New DTCDataContext(BD.Conexion)
            Dim _Linea As Entrada_Linea
            Dim _Entrada As Entrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = pIDEntrada).FirstOrDefault
            For Each _Linea In _Entrada.Entrada_Linea
                'Si s'ha assignat un número de serie a la línea però la línea te una quantitat diferent als números de serie assignats retornarem false
                If _Linea.Entrada_Linea_NS.Count > 1 AndAlso _Linea.Unidad <> _Linea.Entrada_Linea_NS.Count Then
                    Return False
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Function RetornaCodiSeguent(ByVal pEntradaTipo As EnumEntradaTipo, Optional ByVal pIDEmpresa As Integer = 0) As Integer
        Try
            Dim _DTC As New DTCDataContext(BD.Conexion)

            If _DTC.Empresa.Count > 1 AndAlso (pEntradaTipo = EnumEntradaTipo.FacturaCompra Or pEntradaTipo = EnumEntradaTipo.FacturaVenta Or pEntradaTipo = EnumEntradaTipo.FacturaCompraRectificativa Or pEntradaTipo = EnumEntradaTipo.FacturaVentaRectificativa) Then 'Si hi ha més d'una empresa recollirem el número  de factura de la taula empresa o en funció de la empresa
                If _DTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = CInt(pEntradaTipo) And F.ID_Empresa = pIDEmpresa).Count = 0 Then 'Si no hi ha cap entrada, la primera serà la 1
                    Select Case pEntradaTipo
                        Case EnumEntradaTipo.FacturaCompra
                            Return _DTC.Empresa.Where(Function(F) F.ID_Empresa = pIDEmpresa).FirstOrDefault.NumeracionFacturaCompra
                        Case EnumEntradaTipo.FacturaCompraRectificativa
                            Return _DTC.Empresa.Where(Function(F) F.ID_Empresa = pIDEmpresa).FirstOrDefault.NumeracionFacturaCompraRectificativa
                        Case EnumEntradaTipo.FacturaVenta
                            Return _DTC.Empresa.Where(Function(F) F.ID_Empresa = pIDEmpresa).FirstOrDefault.NumeracionFacturaVenta
                        Case EnumEntradaTipo.FacturaVentaRectificativa
                            Return _DTC.Empresa.Where(Function(F) F.ID_Empresa = pIDEmpresa).FirstOrDefault.NumeracionFacturaVentaRectificativa
                    End Select
                Else
                    Return _DTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = CInt(pEntradaTipo) And F.ID_Empresa = pIDEmpresa).Max(Function(F) F.Codigo) + 1
                End If
            Else
                If _DTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = CInt(pEntradaTipo)).Count = 0 Then 'Si no hi ha cap entrada, la primera serà la 1
                    Return _DTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = CInt(pEntradaTipo)).FirstOrDefault.Contador
                Else
                    Return _DTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = CInt(pEntradaTipo)).Max(Function(F) F.Codigo) + 1
                End If
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    'Public Function EliminarLinea(ByRef pLinea As Entrada_Linea) As Boolean
    '    Try
    '        EliminarLinea = False

    '        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.NO Then
    '            Exit Function
    '        End If

    '        Select Case pLinea.Entrada.ID_Entrada_Tipo
    '            Case EnumEntradaTipo.Pedido
    '                If pLinea.CantidadTraspasada > 0 Then
    '                    Mensaje.Mostrar_Mensaje("Imposible eliminar una línea que ya se ha albaranado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                    Exit Function
    '                Else
    '                    oDTC.Entrada_Linea.DeleteOnSubmit(pLinea)
    '                End If

    '            Case EnumEntradaTipo.Albaran
    '                If pLinea.ID_Entrada_Linea_Pedido.HasValue = True Then 'si la linea prové d'una comanda
    '                    'If pLinea.Entrada_Linea_Albaran.Entrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
    '                    ' If Mensaje.Mostrar_Mensaje("Esta línea de albarán proviene de un pedido cerrado, ¿desea abrir el pedido?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
    '                    '     Call ComandaComprovarICanviarEstat(pLinea.Entrada_Linea_Albaran.Entrada, oDTC)
    '                    ' End If
    '                    'Else
    '                    '    Call ComandaComprovarICanviarEstat(pLinea.Entrada_Linea_Albaran.Entrada, oDTC)
    '                    '  End If
    '                    Call ComandaComprovarICanviarEstat(pLinea.Entrada_Linea_Albaran.Entrada, oDTC)
    '                    'Al eliminar una linea d'albarà eliminarem la quantitat traspasada corresponent de la línea de comanda
    '                    pLinea.Entrada_Linea_Albaran.CantidadTraspasada = pLinea.Entrada_Linea_Albaran.CantidadTraspasada - pLinea.Cantidad
    '                    Dim _NS As Entrada_Linea_NS
    '                    For Each _NS In pLinea.Entrada_Linea_NS
    '                        _NS.NS.NS_Estado = oDTC.NS_Estado.Where(Function(F) F.ID_NS_Estado = clsEntrada.RetornaEstatQueHauriaDeTenirElNSalCrearloSegonsPantalla(EnumEntradaTipo.Pedido)).FirstOrDefault
    '                    Next
    '                    oDTC.Entrada_Linea_NS.DeleteAllOnSubmit(pLinea.Entrada_Linea_NS)
    '                Else
    '                    'Si la línea no ve d'una comanda i requereix números de serie esborrarem els números de serie i les entrada_linea_ns
    '                    If pLinea.Producto.RequiereNumeroSerie = True Then

    '                        oDTC.Entrada_Linea_NS.DeleteAllOnSubmit(pLinea.Entrada_Linea_NS)
    '                        oDTC.NS.DeleteAllOnSubmit(From A In pLinea.Entrada_Linea_NS Where A.ID_Entrada_Linea <> 0 Select A.NS)
    '                    End If
    '                End If
    '        End Select
    '        ' oDTC.SubmitChanges()
    '        'If pRowGrid Is Nothing = False Then 'Si m'han passat per parametre la row de la grid farem això
    '        '    pRowGrid.Delete(False)
    '        'End If

    '        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)

    '        oDTC.SubmitChanges()

    '        EliminarLinea = True


    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Function

    Public Shared Sub ComandaComprovarICanviarEstat(ByRef pPedido As Entrada, ByRef pDTC As DTCDataContext)
        Dim _CanviarEstat As Boolean = True

        If pPedido.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
            ' If Mensaje.Mostrar_Mensaje("Este albarán contiene líneas que provienen de un pedido cerrado. Desea abrir el pedido relacionado?", M_Mensaje.Missatge_Modo.PREGUNTA, , , True) = M_Mensaje.Botons.SI Then
            _CanviarEstat = True
            'Else
            '    _CanviarEstat = False
            'End If
        End If

        'Fem aquest if pq si ens han dit que no volen obrir una comanda, no canviarem l'estat
        If _CanviarEstat = True Then
            'si hi ha alguna quantitat traspasada vol dir que la comanda te que estar en estat parcial
            ' MsgBox(pPedido.Entrada_Linea.Sum(Function(F) F.CantidadTraspasada))
            If pPedido.Entrada_Linea.Sum(Function(F) F.CantidadTraspasada) = 0 Then
                pPedido.Entrada_Estado = pDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Pendiente).FirstOrDefault
            Else
                pPedido.Entrada_Estado = pDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
            End If
        End If

        'Dim _Pedido As Entrada = oEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea_Pedido.HasValue).FirstOrDefault.Entrada_Linea_Albaran.Entrada
        'Select Case _Pedido.ID_Entrada_Estado
        '    Case EnumEntradaEstado.Parcial
        '        'Si la comanda només es va traspasar al albarà que estem eliminant llavors pasarem el estat de la comanda a pendent
        '        If _Pedido.Entrada_Linea.Where(Function(F) F.Entrada_Linea_Pedido Is Nothing = False AndAlso F.Entrada_Linea_Pedido.Where(Function(Y) Y.ID_Entrada <> oEntrada.ID_Entrada).Count > 0).Count = 0 Then
        '            _Pedido.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Pendiente).FirstOrDefault
        '        End If
        '    Case EnumEntradaEstado.Cerrado
        '        If Mensaje.Mostrar_Mensaje("Este albarán contiene líneas que provienen de un pedido cerrado. Desea abrir el pedido relacionado?", M_Mensaje.Missatge_Modo.PREGUNTA, , , True) = M_Mensaje.Botons.SI Then
        '            'Si la comanda només es va traspasar al albarà que estem eliminant llavors pasarem el estat de la comanda a pendent
        '            If _Pedido.Entrada_Linea.Where(Function(F) F.Entrada_Linea_Pedido Is Nothing = False AndAlso F.Entrada_Linea_Pedido.Where(Function(Y) Y.ID_Entrada <> oEntrada.ID_Entrada).Count > 0).Count = 0 Then
        '                _Pedido.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Pendiente).FirstOrDefault
        '            Else
        '                _Pedido.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
        '            End If
        '        End If
        'End Select
    End Sub

    'Public Shared Sub GenerarNumerosSerieLinea(ByRef pDTC As DTCDataContext, ByRef pLinea As Entrada_Linea, ByVal pNouValor As Integer)
    '    Dim _QuantitatActual As Integer = pLinea.Entrada_Linea_NS.Count
    '    Dim _Diferencia As Integer
    '    _Diferencia = pNouValor - _QuantitatActual

    '    If _Diferencia = 0 Then 'Si no s'ha modificat la quantitat no farem re
    '        Exit Sub
    '    End If

    '    'Si la nova quantitat es superior a la que ja hi havia crearem nous numeros de serie
    '    If pNouValor > _QuantitatActual Then
    '        For i = 1 To _Diferencia
    '            Dim _ConNS As Integer
    '            _ConNS = pDTC.Contadores.FirstOrDefault.NumeroSerie
    '            Dim _NewLineaNS As New Entrada_Linea_NS
    '            Dim _NewNS As New NS
    '            _NewNS.Descripcion = _ConNS
    '            _NewNS.Almacen = pLinea.Almacen
    '            Dim _EstatEntrada As Integer = pLinea.Entrada.ID_Entrada_Tipo
    '            _NewNS.NS_Estado = pDTC.NS_Estado.Where(Function(F) F.ID_NS_Estado = CInt(RetornaEstatQueHauriaDeTenirElNSalCrearloSegonsPantalla(_EstatEntrada))).FirstOrDefault
    '            _NewLineaNS.NS = _NewNS
    '            pLinea.Entrada_Linea_NS.Add(_NewLineaNS)
    '            pDTC.Contadores.FirstOrDefault.NumeroSerie = _ConNS + 1
    '        Next

    '        'Si no es superior els eliminarem
    '    Else
    '        _Diferencia = _Diferencia * -1 'per posar-ho en positiu
    '        Dim _ContaEliminats As Integer = 0
    '        Dim _LineaNS As Entrada_Linea_NS
    '        For Each _LineaNS In pLinea.Entrada_Linea_NS
    '            If _ContaEliminats = _Diferencia Then
    '                Exit For
    '            End If
    '            If pDTC.Entrada_Linea_NS.Where(Function(F) F.Entrada_Linea.ID_Entrada_Linea_Pedido = _LineaNS.ID_Entrada_Linea And F.NS.Descripcion = _LineaNS.NS.Descripcion).Count = 0 Then
    '                pDTC.Entrada_Linea_NS.DeleteOnSubmit(_LineaNS)
    '                pDTC.NS.DeleteOnSubmit(_LineaNS.NS)
    '                _ContaEliminats = _ContaEliminats + 1
    '            End If
    '        Next
    '    End If
    '    'pDTC.SubmitChanges()
    'End Sub

    'Public Shared Function RetornaEstatQueHauriaDeTenirElNSalCrearloSegonsPantalla(ByVal pTipusDocument As EnumEntradaTipo) As EnumNSEstado
    '    Select Case pTipusDocument
    '        Case EnumEntradaTipo.Pedido
    '            Return EnumNSEstado.NoDisponible
    '        Case EnumEntradaTipo.Albaran
    '            Return EnumNSEstado.Disponible
    '        Case EnumEntradaTipo.Regularizacion
    '            Return EnumNSEstado.Disponible
    '    End Select

    'End Function

    Public Function RetornaLineasTraspasPedidoAlbaranVenta() As DataTable
        Dim _DT As New DataTable
        _DT.Columns.Add("ID_Entrada_Linea", GetType(Long))
        _DT.Columns.Add("CodigoProducto", GetType(String))
        _DT.Columns.Add("DescripcionProducto", GetType(String))
        _DT.Columns.Add("Cantidad", GetType(Decimal))
        _DT.Columns.Add("CantidadATraspasar", GetType(Decimal))
        _DT.Columns.Add("CantidadTraspasada", GetType(Decimal))
        _DT.Columns.Add("RequiereNS", GetType(Boolean))
        _DT.Columns.Add("Almacen", GetType(String))
        _DT.Columns.Add("NoRestarStock", GetType(Boolean))


        Dim _Linea As Entrada_Linea
        For Each _Linea In oEntrada.Entrada_Linea
            Dim _NewRow As DataRow = _DT.NewRow
            _NewRow("ID_Entrada_Linea") = _Linea.ID_Entrada_Linea
            _NewRow("CodigoProducto") = _Linea.Producto.Codigo
            _NewRow("DescripcionProducto") = _Linea.Producto.Descripcion
            _NewRow("Cantidad") = _Linea.Unidad
            _NewRow("CantidadTraspasada") = Util.Comprobar_NULL_Per_0_Decimal(_Linea.CantidadTraspasada)
            If _Linea.Producto.RequiereNumeroSerie Then 'Si son números de serie llavors posarem 0 ja que s'hauran de seleccionar un a un
                _NewRow("CantidadATraspasar") = 0
            Else
                _NewRow("CantidadATraspasar") = _Linea.Unidad - Util.Comprobar_NULL_Per_0_Decimal(_Linea.CantidadTraspasada)
            End If
            _NewRow("RequiereNS") = _Linea.Producto.RequiereNumeroSerie
            _NewRow("Almacen") = _Linea.Almacen.Descripcion
            _NewRow("NoRestarStock") = False

            If _Linea.Unidad <> Util.Comprobar_NULL_Per_0_Decimal(_Linea.CantidadTraspasada) Then 'Només afegirem la row si la linea no ha estat traspasada totalment
                _DT.Rows.Add(_NewRow)
            End If
        Next

        Return _DT
    End Function

    Public Function RetornaLineasTraspasAlbaranFacturaVenta() As DataTable
        Dim _DT As New DataTable
        _DT.Columns.Add("ID_Entrada_Linea", GetType(Long))
        _DT.Columns.Add("CodigoProducto", GetType(String))
        _DT.Columns.Add("DescripcionProducto", GetType(String))
        _DT.Columns.Add("Cantidad", GetType(Decimal))
        '_DT.Columns.Add("CantidadATraspasar", GetType(Decimal))
        '_DT.Columns.Add("CantidadTraspasada", GetType(Decimal))
        _DT.Columns.Add("RequiereNS", GetType(Boolean))
        _DT.Columns.Add("Almacen", GetType(String))
        _DT.Columns.Add("NoRestarStock", GetType(Boolean))


        Dim _Linea As Entrada_Linea
        For Each _Linea In oEntrada.Entrada_Linea
            Dim _NewRow As DataRow = _DT.NewRow
            _NewRow("ID_Entrada_Linea") = _Linea.ID_Entrada_Linea
            _NewRow("CodigoProducto") = _Linea.Producto.Codigo
            _NewRow("DescripcionProducto") = _Linea.Producto.Descripcion
            _NewRow("Cantidad") = _Linea.Unidad
            '_NewRow("CantidadTraspasada") = Util.Comprobar_NULL_Per_0_Decimal(_Linea.CantidadTraspasada)
            'If _Linea.Producto.RequiereNumeroSerie Then 'Si son números de serie llavors posarem 0 ja que s'hauran de seleccionar un a un
            ' _NewRow("CantidadATraspasar") = 0
            ' Else
            ' _NewRow("CantidadATraspasar") = _Linea.Unidad - Util.Comprobar_NULL_Per_0_Decimal(_Linea.CantidadTraspasada)
            ' End If
            _NewRow("RequiereNS") = _Linea.Producto.RequiereNumeroSerie
            _NewRow("Almacen") = _Linea.Almacen.Descripcion
            _NewRow("NoRestarStock") = False

            If _Linea.ID_Entrada_Factura.HasValue = False Then 'Només afegirem la row si la linea no ha estat traspasada totalment
                _DT.Rows.Add(_NewRow)
            End If
        Next

        Return _DT
    End Function

    Public Sub CrearPedidodeVentaAPartirDeUnaPropuesta(ByRef pPropuesta As Propuesta, Optional ByRef pFechaDocumento As Date = Nothing, Optional ByRef pIDEmpresa As Integer = 0)
        With oEntrada
            .Almacen = oDTC.Almacen.Where(Function(F) F.Predeterminado = True).FirstOrDefault
            .Campaña = Nothing
            .ID_Propuesta = pPropuesta.ID_Propuesta
            .Cliente = pPropuesta.Instalacion.Cliente
            .Cliente_Direccion = pPropuesta.Instalacion.Cliente.Direccion
            .Cliente_Email = pPropuesta.Instalacion.Cliente.Email
            .Cliente_Nif = pPropuesta.Instalacion.Cliente.NIF
            .Cliente_PersonaContacto = pPropuesta.Instalacion.Cliente.PersonaContacto
            .Cliente_Poblacion = pPropuesta.Instalacion.Cliente.Poblacion
            .Cliente_Provincia = pPropuesta.Instalacion.Cliente.Provincia
            .Cliente_Telefono = pPropuesta.Instalacion.Cliente.Telefono
            .Codigo = RetornaCodiSeguent(EnumEntradaTipo.PedidoVenta)
            .CompañiaTransporte = Nothing
            .Descripcion = pPropuesta.Codigo & " " & pPropuesta.Version & " - " & pPropuesta.Descripcion
            .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Pendiente).FirstOrDefault
            .Entrada_Origen = Nothing
            .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = EnumEntradaTipo.PedidoVenta).FirstOrDefault
            .FechaAlta = Now.Date
            If pFechaDocumento = Nothing Then
                .FechaEntrada = Now.Date
            Else
                .FechaEntrada = pFechaDocumento
            End If

            If pIDEmpresa = 0 Then
                .Empresa = pPropuesta.Empresa
            Else
                Dim _IDEmpresa As Integer = pIDEmpresa
                .Empresa = oDTC.Empresa.Where(Function(F) F.ID_Empresa = _IDEmpresa).FirstOrDefault
            End If

            .NombreObra = Nothing
            .NumAsiento = Nothing
            .NumeroSeguimiento = Nothing
            .NumPedidoCliente = Nothing
            .NumReferencia = Nothing
            .Observaciones = pPropuesta.Observaciones
            .Personal = pPropuesta.Personal
            .Facturable = True
            If pPropuesta.ID_FormaPago.HasValue = True Then 'si la proposta te forma de pago li posarem aquesta a la nova comanda
                .FormaPago = pPropuesta.FormaPago
            Else
                If pPropuesta.Instalacion.Cliente.ID_FormaPago.HasValue Then 'si la proposta no te forma de pago però el client si li posarem aquesta a la nova comanda
                    .FormaPago = pPropuesta.Instalacion.Cliente.FormaPago
                Else
                    .FormaPago = oDTC.FormaPago.Where(Function(F) F.Predeterminada = True And F.Activo = True).FirstOrDefault
                End If
            End If



            Dim _NewEntradaInstalacion As New Entrada_Instalacion
            _NewEntradaInstalacion.Instalacion = pPropuesta.Instalacion
            oEntrada.Entrada_Instalacion.Add(_NewEntradaInstalacion)

            Dim _NewEntradaPropuesta As New Entrada_Propuesta
            _NewEntradaPropuesta.Propuesta = pPropuesta
            oEntrada.Entrada_Propuesta.Add(_NewEntradaPropuesta)
        End With


        Dim _PropuestaArchivo As Propuesta_Archivo

        For Each _PropuestaArchivo In pPropuesta.Propuesta_Archivo
            Dim _NewEntradaArchivo As New Entrada_Archivo
            With _NewEntradaArchivo
                If IsNothing(_PropuestaArchivo.ID_Archivo) = False Then
                    Dim _NewArchivo As New Archivo
                    _NewArchivo.Activo = _PropuestaArchivo.Archivo.Activo
                    _NewArchivo.CampoBinario = _PropuestaArchivo.Archivo.CampoBinario
                    _NewArchivo.Color = _PropuestaArchivo.Archivo.Color
                    _NewArchivo.Descripcion = _PropuestaArchivo.Archivo.Descripcion
                    _NewArchivo.Fecha = _PropuestaArchivo.Archivo.Fecha
                    _NewArchivo.ID_Usuario = _PropuestaArchivo.Archivo.ID_Usuario
                    _NewArchivo.Ruta_Fichero = _PropuestaArchivo.Archivo.Ruta_Fichero
                    _NewArchivo.Tamaño = _PropuestaArchivo.Archivo.Tamaño
                    _NewArchivo.Tipo = _PropuestaArchivo.Archivo.Tipo
                    .Archivo = _NewArchivo
                    oEntrada.Entrada_Archivo.Add(_NewEntradaArchivo)
                    oDTC.Entrada_Archivo.InsertOnSubmit(_NewEntradaArchivo)
                    oDTC.Archivo.InsertOnSubmit(_NewArchivo)
                End If
            End With
        Next

        'Crear les línies d'entrada a partir de les línies de la comanda
        Dim _PropuestaLinea As Propuesta_Linea
        For Each _PropuestaLinea In pPropuesta.Propuesta_Linea
            Dim oclsLinea As New clsEntradaLinea(oEntrada, , oDTC)
            oclsLinea.CrearLineaAPartirDePropuestaLinea(_PropuestaLinea)
        Next

        oDTC.SubmitChanges()

        oEntrada.Total = oEntrada.RetornaImporteTotal
        oEntrada.Base = oEntrada.RetornaImporteBase
        oEntrada.IVA = oEntrada.RetornaImporteIVA
        oEntrada.Descuento = 0

        oDTC.SubmitChanges()

        'Return _NewEntrada

    End Sub

    Private Sub CrearFicherosPressupost(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _PropuestaArchivo As Propuesta_Archivo

            For Each _PropuestaArchivo In pPropuestaOriginal.Propuesta_Archivo
                Dim _NewPropuestaArchivo As New Propuesta_Archivo
                With _NewPropuestaArchivo
                    If IsNothing(_PropuestaArchivo.ID_Archivo) = False Then
                        Dim _NewArchivo As New Archivo
                        _NewArchivo.Activo = _PropuestaArchivo.Archivo.Activo
                        _NewArchivo.CampoBinario = _PropuestaArchivo.Archivo.CampoBinario
                        _NewArchivo.Color = _PropuestaArchivo.Archivo.Color
                        _NewArchivo.Descripcion = _PropuestaArchivo.Archivo.Descripcion
                        _NewArchivo.Fecha = _PropuestaArchivo.Archivo.Fecha
                        _NewArchivo.ID_Usuario = _PropuestaArchivo.Archivo.ID_Usuario
                        _NewArchivo.Ruta_Fichero = _PropuestaArchivo.Archivo.Ruta_Fichero
                        _NewArchivo.Tamaño = _PropuestaArchivo.Archivo.Tamaño
                        _NewArchivo.Tipo = _PropuestaArchivo.Archivo.Tipo
                        .Archivo = _NewArchivo
                        pPropuestaClon.Propuesta_Archivo.Add(_NewPropuestaArchivo)
                        oDTC.Propuesta_Archivo.InsertOnSubmit(_NewPropuestaArchivo)
                        oDTC.Archivo.InsertOnSubmit(_NewArchivo)
                    End If
                End With
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function EstaLaComandaTotalmentTraspasada() As Boolean
        EstaLaComandaTotalmentTraspasada = False

        Dim Quantitat As Decimal = oEntrada.Entrada_Linea.Sum(Function(F) F.Unidad)
        Dim QuantitatTraspasada As Decimal = oEntrada.Entrada_Linea.Sum(Function(F) F.CantidadTraspasada)
        If Quantitat = QuantitatTraspasada Then
            Return True
        Else
            Return False
        End If

    End Function

    'Public Sub CrearTraspasoAlmacenPerImputacionHoras()
    '    oEntrada.Codigo = RetornaCodiSeguent(EnumEntradaTipo.TraspasoAlmacen)

    '    'Crear linea
    '    Dim oclsEntradaLinea As New clsEntradaLinea(oEntrada)


    'End Sub

    Public Function CrearAlbaraDeVentaApartirDeContrato(ByVal pIDContrato As Integer) As Entrada
        Try
            Dim _NewAlbaranVenta As New Entrada
            Dim _Contrato As Instalacion_Contrato = oDTC.Instalacion_Contrato.Where(Function(F) F.ID_Instalacion_Contrato = pIDContrato).FirstOrDefault
            Dim _Cliente As Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = _Contrato.Instalacion.ID_Cliente).FirstOrDefault


            With _NewAlbaranVenta
                .Almacen = oDTC.Almacen.Where(Function(F) F.Predeterminado = True).FirstOrDefault
                .Base = 0
                .Cliente = _Cliente
                .Cliente_Alta = _Cliente.FechaAlta
                .Cliente_CP = _Cliente.CP
                .Cliente_Direccion = _Cliente.Direccion
                .Cliente_Email = _Cliente.Email
                .Cliente_Nif = _Cliente.NIF
                .Cliente_Observaciones = _Cliente.Observaciones
                .Cliente_PersonaContacto = _Cliente.PersonaContacto
                .Cliente_Poblacion = _Cliente.Poblacion
                .Cliente_Provincia = _Cliente.Provincia
                .Cliente_Telefono = _Cliente.Telefono
                .Codigo = RetornaCodiSeguent(EnumEntradaTipo.AlbaranVenta)
                .Descripcion = "Albarán relacionado con el contrato: " & _Contrato.NumeroContrato
                .Descuento = 0

                If _Cliente.ID_Personal.HasValue = True Then
                    .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = _Cliente.ID_Personal).FirstOrDefault
                End If

                If _Cliente.ID_FormaPago.HasValue Then
                    .ID_FormaPago = _Cliente.ID_FormaPago
                Else
                    Dim _FormaPago As FormaPago = oDTC.FormaPago.Where(Function(F) F.Predeterminada).FirstOrDefault
                    If _FormaPago Is Nothing = False Then
                        .ID_FormaPago = _FormaPago.ID_FormaPago
                    End If
                End If
                If _Cliente.DiaDePago.HasValue Then
                    .DiaDePago = _Cliente.DiaDePago
                End If

                .DocumentoImpreso = False
                .Facturable = True
                .FechaAlta = Now.Date
                .FechaEntrada = Now.Date
                .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Pendiente).FirstOrDefault
                .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranVenta).FirstOrDefault


                'Assignem al Albarà la mateixa instalació que la del contracte
                Dim _EntradaInstalacion As New Entrada_Instalacion
                _EntradaInstalacion.Instalacion = _Contrato.Instalacion
                .Entrada_Instalacion.Add(_EntradaInstalacion)


            End With

            Return _NewAlbaranVenta


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function


    Public Function CrearLineaAlbaraDeVentaApartirDeLineaContrato(ByVal pIDLineaContrato As Integer) As Entrada_Linea
        Try
            Dim _NewLineaAlbara As New Entrada_Linea
            Dim _LineaContrato As Instalacion_Contrato_Producto = oDTC.Instalacion_Contrato_Producto.Where(Function(F) F.ID_Instalacion_Contrato_Producto = pIDLineaContrato).FirstOrDefault

            With _NewLineaAlbara
                .Almacen = oDTC.Almacen.Where(Function(F) F.Predeterminado = True).FirstOrDefault
                .Descripcion = _LineaContrato.Producto.Descripcion
                .FechaEntrada = Now.Date
                .FechaEntrega = Now.Date
                .Instalacion = oDTC.Instalacion.Where(Function(F) F.ID_Instalacion = _LineaContrato.Instalacion_Contrato.ID_Instalacion).FirstOrDefault
                .Instalacion_Contrato = oDTC.Instalacion_Contrato.Where(Function(F) F.ID_Instalacion_Contrato = _LineaContrato.ID_Instalacion_Contrato).FirstOrDefault
                .IVA = oDTC.Configuracion.FirstOrDefault.IVA
                .Precio = _LineaContrato.Precio
                .Producto = _LineaContrato.Producto
                .Unidad = _LineaContrato.Cantidad
            End With

            Return _NewLineaAlbara

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Function RetornaDescripcioDocumentEnFuncioDelSeuTipus(ByVal pTipusDocument As EnumEntradaTipo, Optional ByVal pNumDocumento As Integer = 0) As String
        Select Case pTipusDocument
            Case EnumEntradaTipo.PedidoCompra
                Return "Pedido de compra" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.AlbaranCompra
                Return "Albarán de compra" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.FacturaCompra
                Return "Factura de compra" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.PedidoVenta
                Return "Pedido de venta" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.AlbaranVenta
                Return "Albarán de venta" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.FacturaVenta
                Return "Factura de venta" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.Regularizacion
                Return "Regularización" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.DevolucionCompra
                Return "Devolución de compra " & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.DevolucionVenta
                Return "Devolución de venta" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.TraspasoAlmacen
                Return "Traspaso entre almacenes" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.FacturaCompraRectificativa
                Return "Factura de compra rectificativa" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.FacturaVentaRectificativa
                Return "Factura de venta rectificativa" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
        End Select
    End Function

 

End Class
