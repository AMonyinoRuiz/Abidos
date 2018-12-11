Public Class clsEntradaLinea
    Public oLinqEntrada As Entrada
    Public oLinqLinea As Entrada_Linea
    Public oEntradaTipo As EnumEntradaTipo
    Public oEntradaEstado As EnumEntradaEstado

    Public ReadOnly Property pRequiereNS As Boolean
        Get
            Return oLinqLinea.Producto.RequiereNumeroSerie
        End Get
    End Property

    Dim oDTC As DTCDataContext

    Public Sub New(ByRef pEntrada As Entrada, Optional ByRef pLinea As Entrada_Linea = Nothing, Optional ByVal pDTC As DTCDataContext = Nothing)
        oLinqEntrada = pEntrada
        If pLinea Is Nothing Then
            oLinqLinea = New Entrada_Linea
        Else
            oLinqLinea = pLinea
        End If

        oEntradaTipo = pEntrada.ID_Entrada_Tipo
        oEntradaEstado = pEntrada.ID_Entrada_Estado
        oDTC = pDTC
    End Sub

    Public Function EliminarLinea() As Boolean
        EliminarLinea = False

        If oEntradaEstado = EnumEntradaEstado.Cerrado Then
            Mensaje.Mostrar_Mensaje("Imposible eliminar la línea si el documento esta cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Function
        End If

        If oLinqLinea.Entrada_Linea_Hijo.Count > 0 Then
            Mensaje.Mostrar_Mensaje("Imposible eliminar la línea ya que tiene artículos asignados en el apartado 'Compuesto por'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Function
        End If

        Select Case oEntradaTipo
            Case EnumEntradaTipo.PedidoCompra, EnumEntradaTipo.PedidoVenta, EnumEntradaTipo.Regularizacion, EnumEntradaTipo.TraspasoAlmacen
                If oLinqLinea.CantidadTraspasada > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar una línea que ya se ha albaranado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                Else
                    Dim _LineaNS As Entrada_Linea_NS
                    For Each _LineaNS In oLinqLinea.Entrada_Linea_NS
                        Call LineaNSEliminar(_LineaNS)
                    Next

                    oDTC.Entrada_Linea.DeleteOnSubmit(oLinqLinea)
                End If

            Case EnumEntradaTipo.AlbaranCompra, EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.DevolucionCompra, EnumEntradaTipo.DevolucionVenta

                If oLinqLinea.ID_Entrada_Linea_Pedido.HasValue = True Then 'si la linea prové d'una comanda
                    'Al eliminar una linea d'albarà eliminarem la quantitat traspasada corresponent de la línea de comanda
                    oLinqLinea.Entrada_Linea_Albaran.CantidadTraspasada = oLinqLinea.Entrada_Linea_Albaran.CantidadTraspasada - oLinqLinea.Unidad
                    Call clsEntrada.ComandaComprovarICanviarEstat(oLinqLinea.Entrada_Linea_Albaran.Entrada, oDTC)
                End If


                'ojo Dim _NS As Entrada_Linea_NS
                'For Each _NS In oLinqLinea.Entrada_Linea_NS
                '    _NS.NS.NS_Estado = oDTC.NS_Estado.Where(Function(F) F.ID_NS_Estado = RetornaEstatQueHauriaDeTenirElNSalCrearloSegonsPantalla(EnumEntradaTipo.PedidoCompra)).FirstOrDefault
                'Next

                'oDTC.Entrada_Linea_NS.DeleteAllOnSubmit(oLinqLinea.Entrada_Linea_NS)

                Dim _LineaNS As Entrada_Linea_NS
                For Each _LineaNS In oLinqLinea.Entrada_Linea_NS
                    Call LineaNSEliminar(_LineaNS)
                Next


                oDTC.Entrada_Linea.DeleteOnSubmit(oLinqLinea)


                ' Else

                'Si la línea no ve d'una comanda i requereix números de serie esborrarem els números de serie i les entrada_linea_ns
                'If oLinqLinea.Producto.RequiereNumeroSerie = True Then

                '    ' oDTC.Entrada_Linea_NS.DeleteAllOnSubmit(oLinqLinea.Entrada_Linea_NS)
                '    ' oDTC.NS.DeleteAllOnSubmit(From A In oLinqLinea.Entrada_Linea_NS Where A.ID_Entrada_Linea <> 0 Select A.NS)
                '    oDTC.Entrada_Linea.DeleteOnSubmit(oLinqLinea)

                'End If
                'End If
        End Select

        'Eliminem totes les relacions fetes en la taula propuesta_linea
        oDTC.Entrada_Linea_Propuesta_Linea.DeleteAllOnSubmit(oLinqLinea.Entrada_Linea_Propuesta_Linea)

        'Elminem totes les línies del document relacionades a través del compuesto por
        oDTC.Entrada_Linea.DeleteAllOnSubmit(oLinqLinea.Entrada_Linea_Hijo)

        'Eliminem la relació entre les hores dels partes
        Dim _LlistatParteHoras As IList(Of Parte_Horas) = oLinqLinea.Parte_Horas.ToList
        For Each _ParteHoras In _LlistatParteHoras
            _ParteHoras.Entrada_Linea = Nothing
            _ParteHoras.ID_Entrada_Linea = Nothing
        Next

        'Eliminem la relació entre les hores dels partes
        Dim _LlistatParteMaterial As IList(Of Parte_Material) = oLinqLinea.Parte_Material.ToList
        For Each _ParteMaterial In _LlistatParteMaterial
            _ParteMaterial.Entrada_Linea = Nothing
            _ParteMaterial.ID_Entrada_Linea = Nothing
        Next

        'Eliminem la relació entre les hores dels partes
        Dim _LlistatParteGastos As IList(Of Parte_Gastos) = oLinqLinea.Parte_Gastos.ToList
        For Each _ParteGastos In _LlistatParteGastos
            _ParteGastos.Entrada_Linea = Nothing
            _ParteGastos.ID_Entrada_Linea = Nothing
        Next

        Dim _Archivo As Entrada_Linea_Archivo
        For Each _Archivo In oLinqLinea.Entrada_Linea_Archivo
            oDTC.Archivo.DeleteOnSubmit(_Archivo.Archivo)
            oDTC.Entrada_Linea_Archivo.DeleteOnSubmit(_Archivo)
        Next

        EliminarLinea = True

    End Function

    Public Function DesfacturarLinea() As Boolean
        DesfacturarLinea = False
        oLinqLinea.Entrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Pendiente).FirstOrDefault
        ' oLinqLinea.Entrada_Linea_Albaran = Nothing
        oLinqLinea.ID_Entrada_Factura = Nothing

        DesfacturarLinea = True

    End Function


    Public Function AfegirLinea(ByVal pCreantNSAutomaticament As Boolean) As Boolean
        AfegirLinea = False
        If pCreantNSAutomaticament = True And pRequiereNS = True Then  'Si es un article amb números de serie i s'ha dit que es crein automàticament, els crearem
            If oEntradaTipo <> EnumEntradaTipo.PedidoVenta And oEntradaTipo <> EnumEntradaTipo.Regularizacion And oEntradaTipo <> EnumEntradaTipo.TraspasoAlmacen Then ' A les comandes de venta no generarem mai números de serie
                Call ModificaQuantitatNSDeUnaLinea(oLinqLinea.Unidad)
            End If
        End If
        oLinqEntrada.Entrada_Linea.Add(oLinqLinea)
        AfegirLinea = True
    End Function

    Public Function ModificaLinea() As Boolean
        ModificaLinea = False

        If pRequiereNS = True And oEntradaTipo <> EnumEntradaTipo.Regularizacion And oEntradaTipo <> EnumEntradaTipo.TraspasoAlmacen Then
            Call ModificaQuantitatNSDeUnaLinea(oLinqLinea.Unidad)
        End If

        ModificaLinea = True
    End Function

    Public Function EsUnaLineaNova() As Boolean
        If oLinqLinea.ID_Entrada_Linea = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub ModificaQuantitatNSDeUnaLinea(ByVal pNouValor As Integer)
        Dim _QuantitatActual As Integer
        If oLinqEntrada.ID_Entrada_Tipo = CInt(EnumEntradaTipo.PedidoVenta) Then  'oLinqEntrada.ID_Entrada_Tipo = CInt(EnumEntradaTipo.PedidoCompra) Or
            _QuantitatActual = oLinqLinea.Unidad
        Else
            _QuantitatActual = oLinqLinea.Entrada_Linea_NS.Count
        End If

        Dim _Diferencia As Integer
        _Diferencia = pNouValor - _QuantitatActual

        If _Diferencia = 0 Then 'Si no s'ha modificat la quantitat no farem re
            Exit Sub
        End If

        'Si la nova quantitat es superior a la que ja hi havia crearem nous numeros de serie
        If pNouValor > _QuantitatActual Then

            For i = 1 To _Diferencia
                Call LineaNSCrear()
            Next

        Else  'Si no es superior els eliminarem

            _Diferencia = _Diferencia * -1 'per posar-ho en positiu

            'Fem un bucle i guardem en un array list les línies que eliminarem, despres fem unaltre bucle per eliminar-les realment
            Dim _LlistaAEliminar As New ArrayList
            Dim _LineaNS As Entrada_Linea_NS

            For Each _LineaNS In oLinqLinea.Entrada_Linea_NS
                If _LlistaAEliminar.Count = _Diferencia Then
                    Exit For
                End If

                _LlistaAEliminar.Add(_LineaNS)

            Next

            For Each _LineaNS In _LlistaAEliminar
                Call LineaNSEliminar(_LineaNS)
            Next

            'Dim _ContaEliminats As Integer = 0
            'Dim _LineaNS As Entrada_Linea_NS
            'For Each _LineaNS In oLinqLinea.Entrada_Linea_NS
            '    If _ContaEliminats = _Diferencia Then
            '        Exit For
            '    End If
            '    If oDTC.Entrada_Linea_NS.Where(Function(F) F.Entrada_Linea.ID_Entrada_Linea_Pedido = _LineaNS.ID_Entrada_Linea And F.NS.Descripcion = _LineaNS.NS.Descripcion).Count = 0 Then
            '        oDTC.Entrada_Linea_NS.DeleteOnSubmit(_LineaNS)
            '        oDTC.NS.DeleteOnSubmit(_LineaNS.NS)
            '        _ContaEliminats = _ContaEliminats + 1
            '    End If
            'Next

        End If
        'pDTC.SubmitChanges()
    End Sub

    'Public Shared Function RetornaEstatQueHauriaDeTenirElNSalCrearloSegonsPantalla(ByVal pTipusDocument As EnumEntradaTipo) As EnumNSEstado
    '    Select Case pTipusDocument
    '        Case EnumEntradaTipo.PedidoCompra
    '            Return EnumNSEstado.NoDisponible
    '        Case EnumEntradaTipo.AlbaranCompra
    '            Return EnumNSEstado.Disponible
    '        Case EnumEntradaTipo.Regularizacion
    '            Return EnumNSEstado.Disponible
    '    End Select

    'End Function

    Public Function LineaNSCrear(Optional ByVal pIDNS As Integer = 0, Optional ByVal pDescripcioNS As String = Nothing, Optional ByVal pNSVirtual As Boolean = False) As Boolean
        LineaNSCrear = False

        Dim _NewLineaNS As New Entrada_Linea_NS
        Dim _NewNS As NS
        If pIDNS = 0 Then 'Si no ens han passat un ID de NS voldrà dir que hem de crear el número de serie amb la descripcio que em passen
            _NewNS = NSCrear(pDescripcioNS, pNSVirtual)
            If _NewNS Is Nothing Then 'Si el número de sèrie existeix retornarem un False
                Return False
            End If
        Else
            _NewNS = oDTC.NS.Where(Function(F) F.ID_NS = pIDNS).FirstOrDefault

        End If
        _NewLineaNS.NS = _NewNS
        oLinqLinea.Entrada_Linea_NS.Add(_NewLineaNS)
        Call EstablirEstatNS(_NewLineaNS, True)

        LineaNSCrear = True
    End Function

    Public Function LineaNSEliminar(ByRef pLinea_NS As Entrada_Linea_NS) As Boolean
        LineaNSEliminar = False

        'Si la Linea_NS que anem a esborrar te relacionat un NS í aquest només esta relacionada amb aquesta Linea_NS també eliminarem el NS
        oDTC.Entrada_Linea_NS.DeleteOnSubmit(pLinea_NS)

        If pLinea_NS.NS.Entrada_Linea_NS.Count = 1 Then
            oDTC.NS.DeleteOnSubmit(pLinea_NS.NS)
        End If

        Call EstablirEstatNS(pLinea_NS, False)
        '  oLinqLinea.Entrada_Linea_NS.Remove(pLinea_NS)

        LineaNSEliminar = True
    End Function

    Public Function LineaNSModificar(ByRef pLinea_NS As Entrada_Linea_NS, ByVal pNovaDescripcio As String) As Boolean
        LineaNSModificar = False

        'Si el número de serie que anem a introduir ja existeix retornarem un false
        Dim _IDNS As Integer = pLinea_NS.NS.ID_NS
        If oDTC.NS.Where(Function(F) F.Descripcion = pNovaDescripcio And F.ID_NS <> _IDNS).Count > 0 Then
            Return False
        End If

        pLinea_NS.NS.Descripcion = pNovaDescripcio

        LineaNSModificar = True
    End Function

    Private Function NSCrear(Optional ByVal pText As String = Nothing, Optional ByVal pNSVirtual As Boolean = False) As NS
        NSCrear = Nothing

        If pText Is Nothing = False AndAlso oDTC.NS.Where(Function(F) F.Descripcion = pText).Count > 0 Then
            Return Nothing
        End If

        Dim _NewNS As New NS
        'Si ens pasen per parametre el text del número de serie el posarem si no el recollirem de la taula configuracio
        If pText = Nothing Then
            _NewNS.Descripcion = NumeroNSRecupera(pNSVirtual)
        Else
            _NewNS.Descripcion = pText
        End If

        _NewNS.Almacen = oLinqLinea.Almacen
        _NewNS.Producto = oLinqLinea.Producto


        _NewNS.Virtual = pNSVirtual


        'ojo _NewNS.NS_Estado = oDTC.NS_Estado.Where(Function(F) F.ID_NS_Estado = CInt(RetornaEstatQueHauriaDeTenirElNSalCrearloSegonsPantalla(oEntradaTipo))).FirstOrDefault
        ' pLineaNS.NS = _NewNS
        ' oLinqLinea.Entrada_Linea_NS.Add(_NewLineaNS)



        Return _NewNS
    End Function

    Public Function LineaRelacionadaAmbUnaComanda() As Boolean
        If oLinqLinea.ID_Entrada_Linea_Pedido.HasValue Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub EstablirEstatNS(ByRef pLineaNS As Entrada_Linea_NS, ByVal pInsersio As Boolean)
        Dim _Disponible As NS_Estado = oDTC.NS_Estado.Where(Function(F) F.ID_NS_Estado = EnumNSEstado.Disponible).FirstOrDefault
        Dim _NoDisponible As NS_Estado = oDTC.NS_Estado.Where(Function(F) F.ID_NS_Estado = EnumNSEstado.NoDisponible).FirstOrDefault

        Select Case oEntradaTipo
            Case EnumEntradaTipo.PedidoCompra
                pLineaNS.NS.NS_Estado = _NoDisponible
            Case EnumEntradaTipo.AlbaranCompra
                If pInsersio = True Then
                    pLineaNS.NS.NS_Estado = _Disponible
                Else
                    pLineaNS.NS.NS_Estado = _NoDisponible
                End If
            Case EnumEntradaTipo.FacturaCompra
                pLineaNS.NS.NS_Estado = _Disponible

            Case EnumEntradaTipo.AlbaranVenta
                If pInsersio = True Then
                    pLineaNS.NS.NS_Estado = _NoDisponible
                Else
                    pLineaNS.NS.NS_Estado = _Disponible
                End If
            Case EnumEntradaTipo.Regularizacion
                If pInsersio = True Then
                    pLineaNS.NS.NS_Estado = _NoDisponible
                Else
                    pLineaNS.NS.NS_Estado = _Disponible
                End If
            Case EnumEntradaTipo.TraspasoAlmacen
                'No farem re, no influeix amb l'estat
            Case EnumEntradaTipo.DevolucionCompra
                pLineaNS.NS.NS_Estado = _NoDisponible
            Case EnumEntradaTipo.DevolucionVenta
                pLineaNS.NS.NS_Estado = _Disponible
                pLineaNS.NS.Almacen = pLineaNS.Entrada_Linea.Almacen
            Case EnumEntradaTipo.PedidoVenta 'Això es només pels números de serie virtuals
                pLineaNS.NS.NS_Estado = _NoDisponible

        End Select
    End Sub

    'Public Function NSCrear9(Optional ByVal pText As String = Nothing) As Boolean
    '    NSCrear9 = False

    '    Dim _NewLineaNS As New Entrada_Linea_NS
    '    Dim _NewNS As New NS
    '    'Si ens pasen per parametre el text del número de serie el posarem si no el recollirem de la taula configuracio
    '    If pText = Nothing Then
    '        _NewNS.Descripcion = NumeroNSRecupera()
    '    Else
    '        _NewNS.Descripcion = pText
    '    End If

    '    _NewNS.Almacen = oLinqLinea.Almacen

    '    _NewNS.NS_Estado = oDTC.NS_Estado.Where(Function(F) F.ID_NS_Estado = CInt(RetornaEstatQueHauriaDeTenirElNSalCrearloSegonsPantalla(oEntradaTipo))).FirstOrDefault
    '    _NewLineaNS.NS = _NewNS
    '    oLinqLinea.Entrada_Linea_NS.Add(_NewLineaNS)

    '    Call NumeroNSAugmenta()

    '    NSCrear9 = True
    'End Function

    Public Function RetornaNSDeLaLineaIQueryable() As IQueryable(Of NS)
        Return From NS In oDTC.NS Join Entrada_Linea_NS In oDTC.Entrada_Linea_NS On New With {NS.ID_NS} Equals New With {Entrada_Linea_NS.ID_NS} Join Entrada_Linea In oDTC.Entrada_Linea On New With {Entrada_Linea_NS.ID_Entrada_Linea} Equals New With {Entrada_Linea.ID_Entrada_Linea} Where CLng(Entrada_Linea_NS.ID_Entrada_Linea) = oLinqLinea.ID_Entrada_Linea Select NS
    End Function

    Public Function RetornaNSDeLaLinea() As DataTable
        Return BD.RetornaDataTable("Select * From Entrada_Linea_NS, NS Where Entrada_Linea_NS.ID_NS=NS.ID_NS and ID_Entrada_Linea=" & oLinqLinea.ID_Entrada_Linea & " Order by Descripcion", True)
    End Function

    Public Function RetornaNSDisponiblesDunArticle(ByVal pIDArticle As Integer) As DataTable
        Return BD.RetornaDataTable("Select * From  NS Where  id_NS_Estado=" & EnumNSEstado.Disponible & " and ID_Producto=" & pIDArticle & " Order by Descripcion")
    End Function

    Private Function NumeroNSRecupera(Optional ByVal pVirtual As Boolean = False) As String
        Try
            Dim I As Integer
            For I = oDTC.Contadores.FirstOrDefault.NumeroSerie To I + 1000000  'Agafem el últim número de serie que hi ha a la taula de contadors i anem provant fins a 1000 vegades si el número ja existeix
                If oDTC.NS.Where(Function(F) F.Descripcion = I.ToString).Count = 0 Then
                    Exit For
                End If
            Next
            Call NumeroNSAugmenta(I)
            If pVirtual = True Then
                Return "Abidos-" & I
            Else
                Return I
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje("No ha inicializado el contador de números de serie en la tabla contadores", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
        End Try
    End Function

    Private Sub NumeroNSAugmenta(ByVal pNumeroIntroduit As Integer)
        oDTC.Contadores.FirstOrDefault.NumeroSerie = pNumeroIntroduit + 1
    End Sub

    Public Function EsPotEditarNS() As Boolean

        If oEntradaEstado = EnumEntradaEstado.Cerrado Then
            Return False
        End If

        Select Case oEntradaTipo
            Case EnumEntradaTipo.PedidoCompra
                Return False
            Case EnumEntradaTipo.AlbaranCompra
                If oLinqLinea.Producto.RequiereNumeroSerie = True Then 'Si es requereix número de serie en principi s'hauria de poder tocar els números de serie
                    If LineaRelacionadaAmbUnaComanda() = True Then  'Si la línea prové d'una comanda no deixarem tocar re
                        Return False
                    Else
                        Return True
                    End If

                Else
                    Return False
                End If
            Case EnumEntradaTipo.FacturaCompra
                Return False
            Case EnumEntradaTipo.TraspasoAlmacen
                Return False

        End Select
    End Function

    Public Shared Sub DuplicaLineaEntrada(ByRef pLineaOriginal As Entrada_Linea, ByRef pLineaCopiada As Entrada_Linea)
        With pLineaCopiada
            .ID_Almacen = pLineaOriginal.ID_Almacen
            .ID_Entrada = pLineaOriginal.ID_Entrada
            .ID_Instalacion = pLineaOriginal.ID_Instalacion
            .ID_Instalacion_Contrato = pLineaOriginal.ID_Instalacion_Contrato
            .ID_Instalacion_ElementosAProteger = pLineaOriginal.ID_Instalacion_ElementosAProteger
            .ID_Instalacion_Emplazamiento = pLineaOriginal.ID_Instalacion_Emplazamiento
            .ID_Instalacion_Emplazamiento_Abertura = pLineaOriginal.ID_Instalacion_Emplazamiento_Abertura
            .ID_Instalacion_Emplazamiento_Planta = pLineaOriginal.ID_Instalacion_Emplazamiento_Planta
            .ID_Instalacion_Emplazamiento_Zona = pLineaOriginal.ID_Instalacion_Emplazamiento_Zona
            .ID_Producto = pLineaOriginal.ID_Producto
            .ID_Producto_Garantia = pLineaOriginal.ID_Producto_Garantia
            .Almacen = pLineaOriginal.Almacen
            .AmidamientosFase = pLineaOriginal.AmidamientosFase
            .AmidamientosReferenciaEnObra = pLineaOriginal.AmidamientosReferenciaEnObra
            .CantidadTraspasada = pLineaOriginal.CantidadTraspasada 'ojo
            .Descripcion = pLineaOriginal.Descripcion
            .DescripcionAmpliada = pLineaOriginal.DescripcionAmpliada
            .Descuento1 = pLineaOriginal.Descuento1
            .Descuento2 = pLineaOriginal.Descuento2
            .Entrada = pLineaCopiada.Entrada  'ojo
            .Entrada_Linea_Propuesta_Linea = pLineaOriginal.Entrada_Linea_Propuesta_Linea  'ojo
            .FechaEntrada = pLineaOriginal.FechaEntrada
            .FechaEntrega = pLineaOriginal.FechaEntrega
            .FechaFinGarantia = pLineaOriginal.FechaFinGarantia
            .NoRestarStock = pLineaOriginal.NoRestarStock

            .Instalacion = pLineaOriginal.Instalacion
            .Instalacion_Contrato = pLineaOriginal.Instalacion_Contrato
            .Instalacion_ElementosAProteger = pLineaOriginal.Instalacion_ElementosAProteger
            .Instalacion_Emplazamiento = pLineaOriginal.Instalacion_Emplazamiento
            .Instalacion_Emplazamiento_Abertura = pLineaOriginal.Instalacion_Emplazamiento_Abertura
            .Instalacion_Emplazamiento_Planta = pLineaOriginal.Instalacion_Emplazamiento_Planta
            .Instalacion_Emplazamiento_Zona = pLineaOriginal.Instalacion_Emplazamiento_Zona
            .IVA = pLineaOriginal.IVA
            .Observaciones = pLineaOriginal.Observaciones
            .PeriodoFin = pLineaOriginal.PeriodoFin
            .PeriodoInicio = pLineaOriginal.PeriodoInicio
            .Precio = pLineaOriginal.Precio
            .Producto = pLineaOriginal.Producto
            .Producto_Garantia = pLineaOriginal.Producto_Garantia
            .ReferenciaLinea = pLineaOriginal.ReferenciaLinea
            .ReferenciaNombreObra = pLineaOriginal.ReferenciaNombreObra
            .ReferenciaNum = pLineaOriginal.ReferenciaNum
            .ReferenciaNumeroPedidoDeCompra = pLineaOriginal.ReferenciaNumeroPedidoDeCompra
            .ReferenciaProyecto = pLineaOriginal.ReferenciaProyecto
            .Unidad = pLineaOriginal.Unidad
            .Uso = pLineaOriginal.Uso
            .NoImprimirLinea = pLineaOriginal.NoImprimirLinea
        End With
    End Sub

    Public Sub CrearLineaAPartirDePropuestaLinea(ByRef pLineaPropuesta As Propuesta_Linea)
        With oLinqLinea
            ' ojooooooooooo   .Archivo=pLineaPropues
            .Almacen = oLinqEntrada.Almacen
            .AmidamientosFase = pLineaPropuesta.Fase
            .AmidamientosReferenciaEnObra = pLineaPropuesta.ReferenciaMemoria
            .CantidadTraspasada = 0
            .Descripcion = pLineaPropuesta.Descripcion
            .DescripcionAmpliada = pLineaPropuesta.DescripcionAmpliada
            .Descuento1 = pLineaPropuesta.Descuento
            .Descuento2 = 0
            .FechaEntrada = oLinqEntrada.FechaEntrada
            .FechaEntrega = oLinqEntrada.FechaEntrada
            '.FechaFinGarantia
            .Instalacion = pLineaPropuesta.Propuesta.Instalacion
            .ID_Propuesta_Linea = pLineaPropuesta.ID_Propuesta_Linea
            '.Instalacion_Contrato
            .Instalacion_ElementosAProteger = pLineaPropuesta.Instalacion_ElementosAProteger
            .Instalacion_Emplazamiento = pLineaPropuesta.Instalacion_Emplazamiento
            .Instalacion_Emplazamiento_Abertura = pLineaPropuesta.Instalacion_Emplazamiento_Abertura
            .Instalacion_Emplazamiento_Planta = pLineaPropuesta.Instalacion_Emplazamiento_Planta
            .Instalacion_Emplazamiento_Zona = pLineaPropuesta.Instalacion_Emplazamiento_Zona
            .IVA = pLineaPropuesta.IVA
            '.Observaciones=""
            '.PeriodoFin
            '.PeriodoInicio
            .Precio = pLineaPropuesta.Precio
            .Producto = pLineaPropuesta.Producto
            '.Producto_Garantia
            ' .ReferenciaLinea
            '.ReferenciaNombreObra
            '.ReferenciaNum
            '.ReferenciaNumeroPedidoDeCompra
            '.ReferenciaProyecto

            .StockActivo = False
            .Unidad = pLineaPropuesta.Unidad
            .Uso = pLineaPropuesta.Uso
            pLineaPropuesta.Entrada_Linea = oLinqLinea

            Dim _PropuestaLineaArchivo As Propuesta_Linea_Archivo

            For Each _PropuestaLineaArchivo In pLineaPropuesta.Propuesta_Linea_Archivo
                Dim _NewEntradaLineaArchivo As New Entrada_Linea_Archivo
                With _NewEntradaLineaArchivo
                    If IsNothing(_PropuestaLineaArchivo.ID_Archivo) = False Then
                        Dim _NewArchivo As Archivo = clsFichero.DuplicaFichero(_PropuestaLineaArchivo.Archivo)
                        .Archivo = _NewArchivo
                        oLinqLinea.Entrada_Linea_Archivo.Add(_NewEntradaLineaArchivo)
                        oDTC.Entrada_Linea_Archivo.InsertOnSubmit(_NewEntradaLineaArchivo)
                        oDTC.Archivo.InsertOnSubmit(_NewArchivo)

                        'Si la proposta te un ficher predeterminat i es aquest que estem creant l'assignarem com a predeterminat a la linea 
                        If pLineaPropuesta.Archivo Is Nothing = False AndAlso pLineaPropuesta.ID_Archivo_FotoPredeterminada = _PropuestaLineaArchivo.ID_Archivo Then
                            oLinqLinea.Archivo = _NewArchivo
                        End If
                    End If
                End With
            Next

            oLinqEntrada.Entrada_Linea.Add(oLinqLinea)
        End With
    End Sub

    Public Sub TraspasoAlmacenesRetornaNovaLineaDeSortida(ByRef pEntradaLineaSortida As Entrada_Linea)
        With pEntradaLineaSortida
            .Unidad = oLinqLinea.Unidad * -1  ' Ho tornem a posar a -1 pq ja ve amb negatiu 
            .Almacen = oLinqEntrada.Almacen_Destino
            .FechaEntrada = oLinqLinea.FechaEntrada
            .FechaEntrega = oLinqLinea.FechaEntrega
            .Producto = oLinqLinea.Producto
            .Descripcion = oLinqLinea.Descripcion
            .Uso = oLinqLinea.Uso
            .Observaciones = oLinqLinea.Observaciones

            Dim _EntradaLineaNS As Entrada_Linea_NS
            For Each _EntradaLineaNS In oLinqLinea.Entrada_Linea_NS
                Dim _New As New Entrada_Linea_NS
                _New.NS = _EntradaLineaNS.NS
                'La línea d'abaix és pq al fer un traspas de magatzem el NS canvia d'ubicació al magatzem de destí
                _New.NS.Almacen = oLinqLinea.Entrada.Almacen_Destino
                pEntradaLineaSortida.Entrada_Linea_NS.Add(_New)

            Next

        End With
        oLinqEntrada.Entrada_Linea.Add(pEntradaLineaSortida)
    End Sub

    Public Shared Function RetornaDescripcioDocumentEnFuncioDelSeuTipus(ByVal pTipusDocument As EnumEntradaTipo, Optional ByVal pNumDocumento As Integer = 0) As String
        Select Case pTipusDocument
            Case EnumEntradaTipo.PedidoCompra
                Return "Línea pedido de compra" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.AlbaranCompra
                Return "Línea albarán de compra" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.FacturaCompra
                Return "Línea factura de compra" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.PedidoVenta
                Return "Línea pedido de venta" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.AlbaranVenta
                Return "Línea albarán de venta" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.FacturaVenta
                Return "Línea factura de venta" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.Regularizacion
                Return "Línea regularización" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.DevolucionCompra
                Return "Línea devolución de compra" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.DevolucionVenta
                Return "Línea devolución de venta" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
            Case EnumEntradaTipo.TraspasoAlmacen
                Return "Línea traspaso entre almacenes" & IIf(pNumDocumento = 0, "", ": " & pNumDocumento)
        End Select

    End Function

End Class
