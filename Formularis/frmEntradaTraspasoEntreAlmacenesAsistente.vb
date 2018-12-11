Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEntradaTraspasoEntreAlmacenesAsistente
    Dim oDTC As DTCDataContext
    Dim oLinqEntrada As Entrada
    Dim oRowsSeleccionadas As New ArrayList
    Dim oNSSeleccionadas As New ArrayList
    Public Event AlTancarFormulariTraspas(ByVal pTraspasCorrecte As Boolean)


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar

        If oRowsSeleccionadas.Count = 0 Then
            Mensaje.Mostrar_Mensaje("Imposible crear un traspaso de almacén. Tiene que haber al menos una línea seleccionada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        'Aquest bucle és per impedir que es puguin traspasar líneas amb quantitats 0
        Dim _pRow As UltraGridRow
        Dim _Trobat As Boolean = False
        For Each _pRow In oRowsSeleccionadas
            If _pRow.Cells("CantidadATraspasar").Value = 0 Then
                _Trobat = True
            End If
        Next
        If _Trobat = True Then
            Mensaje.Mostrar_Mensaje("Imposible traspasar, hay alguna línea seleccionada con cantidad a traspasar a 0", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If


        For Each _pRow In oRowsSeleccionadas
            Dim _Linea As New Entrada_Linea
            With _Linea
                .Almacen = oLinqEntrada.Almacen
                .Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(_pRow.Cells("ID_Producto").Value)).FirstOrDefault
                .Descripcion = .Producto.Descripcion
                .FechaEntrada = Now.Date
                .FechaEntrega = Now.Date
                .Unidad = _pRow.Cells("CantidadATraspasar").Value * -1
                .NoRestarStock = False
                .NoImprimirLinea = False
            End With

            ' oLinqEntrada.Entrada_Linea.Add(_Linea)

            Dim _ClsLinea As New clsEntradaLinea(oLinqEntrada, _Linea, oDTC)
            If _ClsLinea.AfegirLinea(False) = True Then
                If _ClsLinea.pRequiereNS = True Then
                    For Each _pRowNS In oNSSeleccionadas
                        If _pRowNS.Cells("ID_Producto").Value = _Linea.ID_Producto Then
                            Dim _IDNS As Integer = _pRowNS.Cells("ID_NS").Value
                            _ClsLinea.LineaNSCrear(oDTC.NS.Where(Function(F) F.ID_NS = _IDNS).FirstOrDefault.ID_NS)
                        End If
                    Next
                End If
            End If




            Dim _clsLinea2 As New clsEntradaLinea(oLinqEntrada, , oDTC)

            _ClsLinea.TraspasoAlmacenesRetornaNovaLineaDeSortida(_clsLinea2.oLinqLinea)
            oDTC.SubmitChanges()




            '_ClsLinea.TraspasoAlmacenesRetornaNovaLineaDeSortida()

            'Dim _SegonaLinea As New Entrada_Linea
            'With _SegonaLinea
            '    .Almacen = oLinqEntrada.Almacen_Destino
            '    .Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(_pRow.Cells("ID_Producto").Value)).FirstOrDefault
            '    .Descripcion = .Producto.Descripcion
            '    .FechaEntrada = Now.Date
            '    .FechaEntrega = Now.Date
            '    .Unidad = _pRow.Cells("CantidadATraspasar").Value
            '    .NoRestarStock = False
            '    .NoImprimirLinea = False
            'End With

            ''    oLinqEntrada.Entrada_Linea.Add(_SegonaLinea)

            'Dim _ClsLinea2 As New clsEntradaLinea(oLinqEntrada, _SegonaLinea, oDTC)
            'If _ClsLinea2.AfegirLinea(False) = True Then
            '    If _ClsLinea2.pRequiereNS = True Then
            '        For Each _pRowNS In oNSSeleccionadas
            '            If _pRowNS.Cells("ID_Producto").Value = _SegonaLinea.ID_Producto Then
            '                Dim _IDNS As Integer = _pRowNS.Cells("ID_NS").Value
            '                _ClsLinea2.LineaNSCrear(oDTC.NS.Where(Function(F) F.ID_NS = _IDNS).FirstOrDefault.ID_NS)
            '            End If
            '        Next
            '    End If
            'End If
        Next


        oDTC.SubmitChanges()


        RaiseEvent AlTancarFormulariTraspas(True)
        Mensaje.Mostrar_Mensaje("Traspaso realizado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
        Me.FormTancar()


        'Dim _clsEntrada As New clsEntrada(oDTC, oLinqEntrada)
        'Dim _IDNouAlbara As Integer = _clsEntrada.TraspasarDeComandaAAlbaraCompra(oRowsSeleccionadas, oNSSeleccionadas, Me.T_NumDocumentoProveedor.Text, IIf(Me.TE_Codigo.Tag Is Nothing, 0, Me.TE_Codigo.Tag))
        'If _IDNouAlbara <> 0 Then


        '    Dim frm As New frmEntrada
        '    frm.Entrada(_IDNouAlbara, EnumEntradaTipo.AlbaranCompra)
        '    frm.FormObrir(Principal, False)  'posem el principal, pq com es destrueix el formulari traspas queda orfa

        '    Exit Sub
        'End If


    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        RaiseEvent AlTancarFormulariTraspas(False)
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pEntrada As Entrada, ByRef pDTC As DTCDataContext)

        Try

            Me.AplicarDisseny()

            oLinqEntrada = pEntrada

            oDTC = pDTC

            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Traspasar"

            Call CargaGrid_Lineas(oLinqEntrada.ID_Entrada)

            ' Me.GRD_Lineas.GRID.ActiveRow = Nothing
            ' Me.GRD_Lineas.GRID.Selected.Rows.Clear()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    'Private Sub RecursivitatLineas(ByVal pLinea As Propuesta_Linea, ByRef pPropuestaSeInstalo As Propuesta)
    '    Dim _Linea2 As Propuesta_Linea
    '    For Each _Linea2 In pLinea.Propuesta_Linea1
    '        Call CreaLineasPropuestaSeInstalo(_Linea2, pPropuestaSeInstalo)
    '        Call RecursivitatLineas(_Linea2, pPropuestaSeInstalo)
    '    Next
    'End Sub


    'Private Sub CreaLineasPropuestaSeInstalo(ByVal pLinea As Propuesta_Linea, ByRef pPropuestaSeInstalo As Propuesta)
    '    Dim _Linea As Propuesta_Linea
    '    _Linea = pLinea

    '    Dim pRow As UltraGridRow
    '    Dim _Trobat As Boolean = False
    '    'Aquest bucle es per no traspasar les línies que no estan seleccionades
    '    For Each pRow In RowsSeleccionadas
    '        If pRow.Cells("ID_Propuesta_Linea").Value = pLinea.ID_Propuesta_Linea Then
    '            _Trobat = True
    '            Exit For
    '        End If
    '    Next

    '    If _Trobat = False Then
    '        Exit Sub
    '    End If

    '    'For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso <> EnumProductoSubFamiliaTraspaso.No).OrderBy(Function(F) F.ID_Propuesta_Linea_Vinculado)
    '    If _Linea.Unidad > 0 Then
    '        For i = 1 To _Linea.Unidad
    '            Dim DescartarLinea As Boolean = False
    '            Dim _NewLinea As New Propuesta_Linea
    '            With _NewLinea
    '                .Descripcion = _Linea.Descripcion
    '                .ID_Instalacion_ElementosAProteger = _Linea.ID_Instalacion_ElementosAProteger
    '                .ID_Instalacion_Emplazamiento = _Linea.ID_Instalacion_Emplazamiento
    '                .ID_Instalacion_Emplazamiento_Abertura = _Linea.ID_Instalacion_Emplazamiento_Abertura
    '                .ID_Instalacion_Emplazamiento_Planta = _Linea.ID_Instalacion_Emplazamiento_Planta
    '                .ID_Instalacion_Emplazamiento_Zona = _Linea.ID_Instalacion_Emplazamiento_Zona
    '                .ID_Producto = _Linea.ID_Producto
    '                .ID_Propuesta = pPropuestaSeInstalo.ID_Propuesta

    '                If _Linea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
    '                    'El If de baix es per comprovar que la línea s'ha traspasat. Pq potser que el pare tingues la marca de família que no s'ha de traspasar i el fill llavors no s'ha de vincular
    '                    If IsNothing(pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault) = True Then
    '                        ' DescartarLinea = True
    '                    Else
    '                        .ID_Propuesta_Linea_Vinculado = pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault.ID_Propuesta_Linea
    '                    End If
    '                End If

    '                .Identificador = _Linea.Identificador
    '                .ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea
    '                .ID_Propuesta_Antigua = _Linea.ID_Propuesta
    '                .Precio = 0
    '                .Descuento = 0
    '                .IVA = 0
    '                .Unidad = 1
    '                .Activo = True
    '                .ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Traspasada
    '                .Uso = _Linea.Uso


    '                .NumZona = _Linea.NumZona
    '                .BocaConexion = _Linea.BocaConexion
    '                '.CantidadPendienteRecibir = _Linea.CantidadPendienteRecibir
    '                .DescripcionAmpliada = _Linea.DescripcionAmpliada
    '                .DetalleInstalacion = _Linea.DetalleInstalacion
    '                '.FechaPrevista = _Linea.FechaPrevista
    '                .IdentificadorDelProducto = _Linea.IdentificadorDelProducto
    '                .NickZona = _Linea.NickZona
    '                .NumSerie = _Linea.NumSerie
    '                .Particion = _Linea.Particion
    '                .PrecioCoste = _Linea.PrecioCoste
    '                .RutaOrden = _Linea.RutaOrden
    '                .RutaParametros = _Linea.RutaParametros
    '                .ID_Propuesta_Linea_TipoZona = _Linea.ID_Propuesta_Linea_TipoZona
    '                .ATenerEnCuenta = _Linea.ATenerEnCuenta
    '                .Fase = _Linea.Fase
    '                .ReferenciaMemoria = _Linea.ReferenciaMemoria

    '                Dim _DatoAcceso As Propuesta_Linea_Acceso
    '                For Each _DatoAcceso In _Linea.Propuesta_Linea_Acceso
    '                    Dim _NewDatoAcceso As New Propuesta_Linea_Acceso
    '                    With _NewDatoAcceso
    '                        .Contraseña = _DatoAcceso.Contraseña
    '                        .Detalle = _DatoAcceso.Detalle
    '                        .Explicación = _DatoAcceso.Explicación
    '                        .Propuesta_Linea_TipoAcceso = _DatoAcceso.Propuesta_Linea_TipoAcceso
    '                        .Usuario = _DatoAcceso.Usuario
    '                        .ValorCRA = _DatoAcceso.ValorCRA
    '                    End With
    '                    .Propuesta_Linea_Acceso.Add(_NewDatoAcceso)
    '                Next
    '            End With
    '            pPropuestaSeInstalo.Propuesta_Linea.Add(_NewLinea)
    '            oDTC.SubmitChanges()
    '        Next
    '    End If
    '    'Next
    'End Sub

    'Private Sub CrearLineasPropuestaSeInstalo(ByVal pIDPropuestaOriginal As Integer)
    '    Try
    '        Dim PropuestaOriginal As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = pIDPropuestaOriginal).FirstOrDefault
    '        Dim PropuestaSeInstalo As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault

    '        Dim _Linea As Propuesta_Linea
    '        'Dim _LineaVinculada As Propuesta_Linea

    '        For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = False)
    '            Call CreaLineasPropuestaSeInstalo(_Linea, PropuestaSeInstalo)
    '            Call RecursivitatLineas(_Linea, PropuestaSeInstalo)
    '            'For Each _LineaVinculada In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea_Vinculado)
    '            'Next
    '        Next

    '        ''For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso <> EnumProductoSubFamiliaTraspaso.No).OrderBy(Function(F) F.ID_Propuesta_Linea_Vinculado)
    '        'If _Linea.Unidad > 0 Then
    '        '    For i = 1 To _Linea.Unidad
    '        '        Dim DescartarLinea As Boolean = False
    '        '        Dim _NewLinea As New Propuesta_Linea
    '        '        With _NewLinea
    '        '            .Descripcion = _Linea.Descripcion
    '        '            .ID_Instalacion_ElementosAProteger = _Linea.ID_Instalacion_ElementosAProteger
    '        '            .ID_Instalacion_Emplazamiento = _Linea.ID_Instalacion_Emplazamiento
    '        '            .ID_Instalacion_Emplazamiento_Abertura = _Linea.ID_Instalacion_Emplazamiento_Abertura
    '        '            .ID_Instalacion_Emplazamiento_Planta = _Linea.ID_Instalacion_Emplazamiento_Planta
    '        '            .ID_Instalacion_Emplazamiento_Zona = _Linea.ID_Instalacion_Emplazamiento_Zona
    '        '            .ID_Producto = _Linea.ID_Producto
    '        '            .ID_Propuesta = PropuestaSeInstalo.ID_Propuesta

    '        '            If _Linea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
    '        '                'El If de baix es per comprovar que la línea s'ha traspasat. Pq potser que el pare tingues la marca de família que no s'ha de traspasar i el fill llavors no s'ha de vincular
    '        '                If IsNothing(PropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault) = True Then
    '        '                    DescartarLinea = True
    '        '                Else
    '        '                    .ID_Propuesta_Linea_Vinculado = PropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault.ID_Propuesta_Linea
    '        '                End If
    '        '            End If

    '        '            .Identificador = _Linea.Identificador
    '        '            .ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea
    '        '            .ID_Propuesta_Antigua = _Linea.ID_Propuesta
    '        '            .Precio = 0
    '        '            .Descuento = 0
    '        '            .IVA = 0
    '        '            .Unidad = 1
    '        '            .Activo = True
    '        '            .ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Traspasada
    '        '            .Uso = _Linea.Uso


    '        '            .NumZona = _Linea.NumZona
    '        '            .BocaConexion = _Linea.BocaConexion
    '        '            '.CantidadPendienteRecibir = _Linea.CantidadPendienteRecibir
    '        '            .DescripcionAmpliada = _Linea.DescripcionAmpliada
    '        '            .DetalleInstalacion = _Linea.DetalleInstalacion
    '        '            '.FechaPrevista = _Linea.FechaPrevista
    '        '            .IdentificadorDelProducto = _Linea.IdentificadorDelProducto
    '        '            .NickZona = _Linea.NickZona
    '        '            .NumSerie = _Linea.NumSerie
    '        '            .Particion = _Linea.Particion
    '        '            .PrecioCoste = _Linea.PrecioCoste
    '        '            .RutaOrden = _Linea.RutaOrden
    '        '            .RutaParametros = _Linea.RutaParametros
    '        '            .ID_Propuesta_Linea_TipoZona = _Linea.ID_Propuesta_Linea_TipoZona
    '        '        End With
    '        '        PropuestaSeInstalo.Propuesta_Linea.Add(_NewLinea)
    '        '        oDTC.SubmitChanges()
    '        '    Next
    '        'End If
    '        ''Next

    '        oDTC.SubmitChanges()

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CrearPlanosPropuestaSeInstalo(ByVal pIDPropuestaOriginal As Integer)
    '    Try
    '        Dim PropuestaOriginal As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = pIDPropuestaOriginal).FirstOrDefault
    '        Dim PropuestaSeInstalo As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault

    '        Dim _Plano As Propuesta_Plano


    '        For Each _Plano In PropuestaOriginal.Propuesta_Plano
    '            Dim _NewPlano As New Propuesta_Plano
    '            With _NewPlano
    '                .Descripcion = _Plano.Descripcion
    '                .FechaCreacion = _Plano.FechaCreacion
    '                .Validado = _Plano.Validado

    '                .ID_Propuesta = PropuestaSeInstalo.ID_Propuesta
    '                .ID_Propuesta_Antigua = _Plano.ID_Propuesta
    '                .Propuesta_Version_Antigua = _Plano.Propuesta.Version

    '                .ID_Instalacion_Emplazamiento = _Plano.ID_Instalacion_Emplazamiento
    '                .ID_Instalacion_Emplazamiento_Planta = _Plano.ID_Instalacion_Emplazamiento_Planta
    '                .ID_Instalacion_Emplazamiento_Zona = _Plano.ID_Instalacion_Emplazamiento_Zona

    '                If IsNothing(_Plano.ID_PlanoBinario) = False Then
    '                    Dim _PlanoBinario As New PlanoBinario
    '                    _PlanoBinario.Fichero = _Plano.PlanoBinario.Fichero
    '                    .PlanoBinario = _PlanoBinario
    '                    oDTC.PlanoBinario.InsertOnSubmit(_PlanoBinario)
    '                End If
    '            End With



    '            PropuestaSeInstalo.Propuesta_Plano.Add(_NewPlano)
    '            oDTC.SubmitChanges()
    '        Next

    '        oDTC.SubmitChanges()

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CrearFicherosPropuestaSeInstalo()
    '    Dim PropuestaSeInstalo As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault

    '    Dim pRow As UltraWinGrid.UltraGridRow
    '    For Each pRow In Me.GRD_Ficheros.GRID.Rows
    '        If pRow.Cells("Seleccion").Value = True Then
    '            Dim _Archivo As Archivo
    '            _Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = CInt(pRow.Cells("ID_Archivo").Value)).FirstOrDefault

    '            Dim _NewArchivo As New Archivo
    '            Dim _NewPropuestaArchivo As New Propuesta_Archivo
    '            _NewArchivo.Activo = True
    '            _NewArchivo.CampoBinario = _Archivo.CampoBinario
    '            _NewArchivo.Color = _Archivo.Color
    '            _NewArchivo.Descripcion = _Archivo.Descripcion
    '            _NewArchivo.Fecha = _Archivo.Fecha
    '            _NewArchivo.ID_Usuario = _Archivo.ID_Usuario
    '            _NewArchivo.Ruta_Fichero = _Archivo.Ruta_Fichero
    '            _NewArchivo.Tamaño = _Archivo.Tamaño
    '            _NewArchivo.Tipo = _Archivo.Tipo

    '            _NewPropuestaArchivo.Archivo = _NewArchivo
    '            _NewPropuestaArchivo.Propuesta = PropuestaSeInstalo
    '            PropuestaSeInstalo.Propuesta_Archivo.Add(_NewPropuestaArchivo)
    '            oDTC.Archivo.InsertOnSubmit(_NewArchivo)
    '            oDTC.Propuesta_Archivo.InsertOnSubmit(_NewPropuestaArchivo)
    '        End If
    '    Next
    '    oDTC.SubmitChanges()
    'End Sub

#End Region

#Region "Grid Lineas"

    Public Sub CargaGrid_Lineas(ByVal pIDEntrada As Integer)
        ' Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        'If _Propuesta Is Nothing = False Then
        Dim DTS As New DataSet
        BD.CargarDataSet(DTS, "Select *, cast(0 as bit) as Seleccion, 0 as CantidadATraspasar, (Select Count(*) From NS Where NS.ID_Producto=RetornaStock.ID_Producto and ID_Almacen=" & oLinqEntrada.ID_Almacen & " and ID_NS_Estado=" & EnumNSEstado.Disponible & " and Virtual=0) as NumNS  From RetornaStock(0," & oLinqEntrada.ID_Almacen & ") Where StockReal>0 Order by ProductoDescripcion")
        BD.CargarDataSet(DTS, "Select *, cast(0 as bit) as Seleccion From NS Where ID_Almacen=" & oLinqEntrada.ID_Almacen & " and ID_NS_Estado=" & EnumNSEstado.Disponible & " and Virtual=0 Order by Descripcion", "AA", 0, "ID_Producto", "ID_Producto", False)

        Me.GRD_Lineas.M.clsUltraGrid.Cargar(DTS)
        Me.GRD_Lineas.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True

        Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellClickAction = CellClickAction.CellSelect
        Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellActivation = Activation.AllowEdit

        Me.GRD_Lineas.GRID.DisplayLayout.Bands(0).Columns("CantidadATraspasar").CellClickAction = CellClickAction.EditAndSelectText

        'Dim pRow As UltraGridRow
        'For Each pRow In Me.GRD_Lineas.GRID.Rows
        '    pRow.Cells("Seleccion").Value = True
        '    'pRow.Cells("Seleccion").Activation = Activation.Disabled
        '    oRowsSeleccionadas.Add(pRow)
        '    pRow.Update()
        'Next

    End Sub

    Private Sub GRD_Lineas_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Lineas.M_Grid_InitializeRow
        If e.Row.Cells("Seleccion").Value = True Then
            e.Row.CellAppearance.BackColor = Color.LightGreen
        Else
            e.Row.CellAppearance.BackColor = Color.LightCoral
        End If

        Call ControlSeleccio(Me.GRD_Lineas.GRID.ActiveCell, e.Row)

        If e.Row.Band.Index = 0 Then
            If e.Row.Cells("NumNS").Value > 0 Then
                e.Row.Cells("CantidadATraspasar").Activation = Activation.Disabled
                e.Row.Cells("Seleccion").Activation = Activation.Disabled
                '  e.Row.Cells("CantidadATraspasar").Activation = Activation.AllowEdit
            Else
                e.Row.Cells("CantidadATraspasar").Activation = Activation.AllowEdit
                e.Row.Cells("Seleccion").Activation = Activation.AllowEdit

                '  e.Row.Cells("CantidadATraspasar").Activation = Activation.NoEdit
                'e.Row.Cells("CantidadATraspasar").ClickAction = CellClickAction.CellSelect
            End If

        End If


        'Dim _Linea As Propuesta_Linea
        '_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
        'If _Linea.Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True And F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea).Count > 0 Then
        '    e.Row.Cells("Seleccion").Value = True
        '    e.Row.Cells("Seleccion").Activation = Activation.Disabled
        'End If


    End Sub



    Private Sub GRD_Horas_M_GRID_BeforeCellUpdate(sender As Object, e As BeforeCellUpdateEventArgs) Handles GRD_Lineas.M_GRID_BeforeCellUpdate
        If e.Cell.Column.Key = "CantidadATraspasar" Then
            If e.Cell.IsActiveCell = False Then 'Aquest if es pq això no s'apliqui la primera vegada k es carrega el grid
                Exit Sub
            End If
            If IsNumeric(e.NewValue) Then
                Dim QuantitatTraspasada As Decimal
                'If IsDBNull(e.Cell.Row.Cells("CantidadTraspasada").Value) = True Then
                '    QuantitatTraspasada = 0
                'Else
                '    QuantitatTraspasada = e.Cell.Row.Cells("CantidadTraspasada").Value
                'End If
                If e.NewValue > e.Cell.Row.Cells("StockReal").Value Then
                    Mensaje.Mostrar_Mensaje("Imposible traspasar más cantidad de la que hay en el origen.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    e.Cancel = True
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub GRD_Lineas_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Lineas.M_GRID_AfterCellActivate
        With GRD_Lineas.GRID
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

    Private Sub ControlSeleccio(ByRef pActiveCell As UltraGridCell, ByRef pActiveRow As UltraGridRow)
        If pActiveRow.Cells("Seleccion").Value = True Then
            Select Case pActiveRow.Band.Index
                Case 0
                    If oRowsSeleccionadas.Contains(pActiveRow) = False Then
                        oRowsSeleccionadas.Add(pActiveRow)
                    End If
                Case 1
                    If oNSSeleccionadas.Contains(pActiveRow) = False Then
                        pActiveRow.ParentRow.Cells("CantidadATraspasar").Value = pActiveRow.ParentRow.Cells("CantidadATraspasar").Value + 1
                        pActiveRow.ParentRow.Update()
                        oNSSeleccionadas.Add(pActiveRow)
                    End If
            End Select
        Else
            Select Case pActiveRow.Band.Index
                Case 0
                    If oRowsSeleccionadas.Contains(pActiveRow) Then
                        oRowsSeleccionadas.Remove(pActiveRow)
                    End If
                Case 1
                    If oNSSeleccionadas.Contains(pActiveRow) Then
                        pActiveRow.ParentRow.Cells("CantidadATraspasar").Value = pActiveRow.ParentRow.Cells("CantidadATraspasar").Value - 1
                        pActiveRow.ParentRow.Update()
                        oNSSeleccionadas.Remove(pActiveRow)
                    End If
            End Select
        End If

        'Aquest if es pq si acabem clickar sobre una celda de Seleccion de la banda 1 comprovi si la quantitat a traspassar es 0, si es 0 desmarcarem el check de seleccion de la banda 0
        If pActiveRow.Band.Index = 1 Then
            If pActiveRow.ParentRow.Cells("CantidadATraspasar").Value = 0 Then
                If oRowsSeleccionadas.Contains(pActiveRow.ParentRow) Then
                    oRowsSeleccionadas.Remove(pActiveRow.ParentRow)
                    pActiveRow.ParentRow.Cells("Seleccion").Value = False
                    pActiveRow.ParentRow.Update()
                End If
            Else
                If oRowsSeleccionadas.Contains(pActiveRow.ParentRow) = False Then
                    oRowsSeleccionadas.Add(pActiveRow.ParentRow)
                    pActiveRow.ParentRow.Cells("Seleccion").Value = True
                    pActiveRow.ParentRow.Update()
                End If
            End If
        End If
        ' oDTC.SubmitChanges()

    End Sub

#End Region


End Class