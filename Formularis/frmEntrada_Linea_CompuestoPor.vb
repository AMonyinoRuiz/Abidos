Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEntrada_Linea_CompuestoPor
    Dim oDTC As DTCDataContext
    Dim oclsEntradaLineaPare As clsEntradaLinea
    Dim oclsEntradaLinea As clsEntradaLinea
    Dim oRowsSeleccionadas As New ArrayList
    Dim oNSSeleccionadas As New ArrayList
    Public Event AlTancarFormulariTraspas(ByVal pTraspasCorrecte As Boolean)

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar

        If oRowsSeleccionadas.Count = 0 Then
            Mensaje.Mostrar_Mensaje("No hay nada a guardar, no se ha seleccionado ningún producto.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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
            Mensaje.Mostrar_Mensaje("Imposible asignar, hay alguna línea seleccionada con cantidad a traspasar a 0", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If


        For Each _pRow In oRowsSeleccionadas
            Dim _Linea As New Entrada_Linea
            With _Linea
                .Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.C_Almacen.Value)).FirstOrDefault ' oclsEntradaLinea.oLinqEntrada.Almacen
                .Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(_pRow.Cells("ID_Producto").Value)).FirstOrDefault
                .Descripcion = .Producto.Descripcion
                .FechaEntrada = oclsEntradaLineaPare.oLinqLinea.FechaEntrada
                .FechaEntrega = oclsEntradaLineaPare.oLinqLinea.FechaEntrega
                .Unidad = _pRow.Cells("CantidadATraspasar").Value
                .NoRestarStock = False
                .NoImprimirLinea = False
                '.ID_Entrada_Linea_Padre = oclsEntradaLineaPare.oLinqLinea.ID_Entrada_Linea
                .Entrada_Linea_Padre = oclsEntradaLineaPare.oLinqLinea

                Dim _PVP As Decimal
                Dim _PVD As Decimal

                Call frmPropuesta_Linea.RetornaPVPiPVD(_Linea.ID_Producto, _PVP, _PVD)

                .Coste = _PVD
            End With

            ' oLinqEntrada.Entrada_Linea.Add(_Linea)

            Dim _ClsLinea As New clsEntradaLinea(oclsEntradaLinea.oLinqEntrada, _Linea, oDTC)
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
        Mensaje.Mostrar_Mensaje("Compuesto por generado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
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

    Public Sub Entrada(ByRef pclsEntradaLineaPare As clsEntradaLinea, ByRef pDTC As DTCDataContext, Optional ByVal pLinea As Entrada_Linea = Nothing)

        Try

            Me.AplicarDisseny()

            oclsEntradaLineaPare = pclsEntradaLineaPare

            oclsEntradaLinea = New clsEntradaLinea(pclsEntradaLineaPare.oLinqEntrada, pLinea, pDTC)

            oDTC = pDTC

            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Asignar"
            Me.GRD_Lineas.M.clsToolBar.Boto_Afegir("SeleccionarTodo", "Seleccionar todos")
            Me.GRD_Lineas.M.clsToolBar.Boto_Afegir("SeleccionarTodoMenosNS", "Seleccionar todos menos números de serie")


            Call CarregarCombo(False)

            ' Me.GRD_Lineas.M.clsToolBar.Boto_Afegir("As")



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
        BD.CargarDataSet(DTS, "Select *, cast(0 as bit) as Seleccion, cast(0 as decimal(10,2)) as CantidadATraspasar, (Select Count(*) From NS Where NS.ID_Producto=RetornaStock.ID_Producto and ID_Almacen=" & Me.C_Almacen.Value & " and ID_NS_Estado=" & EnumNSEstado.Disponible & " and Virtual=0) as NumNS  From RetornaStock(0," & Me.C_Almacen.Value & ") Where StockReal>0 Order by ProductoDescripcion")
        BD.CargarDataSet(DTS, "Select *, cast(0 as bit) as Seleccion From NS Where ID_Almacen=" & Me.C_Almacen.Value & " and ID_NS_Estado=" & EnumNSEstado.Disponible & " and Virtual=0 Order by Descripcion", "AA", 0, "ID_Producto", "ID_Producto", False)

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
            If e.Row.Cells("CantidadATraspasar").Value > 0 Then
                e.Row.Cells("Seleccion").Value = True
            Else
                e.Row.Cells("Seleccion").Value = False
            End If

        End If


        'Dim _Linea As Propuesta_Linea
        '_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
        'If _Linea.Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True And F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea).Count > 0 Then
        '    e.Row.Cells("Seleccion").Value = True
        '    e.Row.Cells("Seleccion").Activation = Activation.Disabled
        'End If

        If oRowsSeleccionadas.Count > 0 Then
            Me.C_Almacen.Enabled = False
            Me.B_CarregarNS.Enabled = False
        Else
            Me.C_Almacen.Enabled = True
            Me.B_CarregarNS.Enabled = True
        End If
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

    Private Sub GRD_Lineas_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Lineas.M_ToolGrid_ToolClickBotonsExtras2
        Select Case e.Tool.Key
            Case "SeleccionarTodo", "SeleccionarTodoMenosNS"
                Dim _pRow As UltraGridRow
                For Each _pRow In Me.GRD_Lineas.GRID.Rows
                    If _pRow.Cells("NumNS").Value = 0 Then
                        _pRow.Cells("CantidadATraspasar").Value = _pRow.Cells("StockReal").Value
                        _pRow.Update()
                    Else
                        If e.Tool.Key = "SeleccionarTodo" Then
                            Dim _Fills As UltraGridRow
                            For Each _Fills In _pRow.ChildBands(0).Rows
                                _Fills.Cells("Seleccion").Value = True
                                _Fills.Update()
                            Next
                        End If
                    End If
                Next

        End Select
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

    Private Sub C_Almacen_BeforeEditorButtonCheckStateChanged(sender As Object, e As UltraWinEditors.BeforeCheckStateChangedEventArgs) Handles C_Almacen.BeforeEditorButtonCheckStateChanged
        Dim _Boto As Infragistics.Win.UltraWinEditors.StateEditorButton = Me.C_Almacen.ButtonsRight("Todos")

        If e.NewCheckState = CheckState.Checked Then
            _Boto.Text = "Visualizar los almacenes relacionados"
        Else
            _Boto.Text = "Visualizar todos"
        End If

        If oclsEntradaLinea.oLinqLinea.Entrada_Linea_NS.Count > 0 Then
            e.Cancel = True
            Exit Sub
        End If

        Call CarregarCombo(e.NewCheckState)
    End Sub

    Private Sub CarregarCombo(ByVal pTots As Boolean)
        Dim _IDAlmacen As Integer = Me.C_Almacen.Value
        ' Dim _Boto As Infragistics.Win.UltraWinEditors.StateEditorButton = Me.C_Almacen.ButtonsRight("Todos")
        If pTots = True Then
            Util.Cargar_Combo(Me.C_Almacen, "SELECT ID_Almacen, Descripcion FROM Almacen Where Activo=1 ORDER BY Descripcion", False)
        Else
            Util.Cargar_Combo(Me.C_Almacen, "Select ID_Almacen, Descripcion From Almacen Where Activo=1 and ID_Almacen in (Select ID_Almacen From Almacen Where Predeterminado=1 or ID_Parte in (Select ID_Parte From Entrada_Parte Where ID_Entrada=" & oclsEntradaLinea.oLinqEntrada.ID_Entrada & ")) or ID_Almacen In (SELECT dbo.Almacen.ID_Almacen  FROM  dbo.Entrada_Parte INNER JOIN dbo.Parte ON dbo.Entrada_Parte.ID_Parte = dbo.Parte.ID_Parte INNER JOIN dbo.Parte_Asignacion ON dbo.Parte.ID_Parte = dbo.Parte_Asignacion.ID_Parte INNER JOIN   dbo.Almacen ON dbo.Parte_Asignacion.ID_Personal = dbo.Almacen.ID_Personal WHERE (dbo.Entrada_Parte.ID_Entrada = " & oclsEntradaLinea.oLinqEntrada.ID_Entrada & ") GROUP BY dbo.Entrada_Parte.ID_Entrada, dbo.Almacen.ID_Almacen) Order by Descripcion", False, False)
        End If

        Me.C_Almacen.Value = _IDAlmacen
    End Sub

    '#Region "M_ToolForm"

    '    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
    '        Try

    '            Util.WaitFormObrir()
    '            If Guardar() = True Then
    '                'Mensaje.Mostrar_Mensaje(" traspasada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
    '                Call M_ToolForm1_m_ToolForm_Sortir()
    '            End If
    '            Util.WaitFormTancar()


    '        Catch ex As Exception
    '            Util.WaitFormTancar()
    '            Mensaje.Mostrar_Mensaje_Error(ex)

    '        End Try
    '    End Sub

    '    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
    '        Me.FormTancar()
    '    End Sub

    '#End Region

    '#Region "Metodes"

    'Public Sub Entrada(ByRef pclsEntradaLineaPare As clsEntradaLinea, ByRef pDTC As DTCDataContext, Optional ByVal pLinea As Entrada_Linea = Nothing)

    '    '        Try

    '    '            Me.AplicarDisseny()

    '    '            oclsEntradaLineaPare = pclsEntradaLineaPare

    '    '            oclsEntradaLinea = New clsEntradaLinea(pclsEntradaLineaPare.oLinqEntrada, pLinea, pDTC)

    '    '            oDTC = pDTC
    '    '            'oDTC = New DTCDataContext(BD.Conexion)


    '    '            ''Cargar en rowsseleccionadas els id propuesta ja assignats
    '    '            'Dim _Relacio As Entrada_Linea_Propuesta_Linea
    '    '            'For Each _Relacio In oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea
    '    '            '    RowsSeleccionadas.Add(_Relacio.ID_Propuesta_Linea)
    '    '            'Next

    '    '            Me.GRD_NS.M.clsToolBar.Boto_Afegir("SeleccionarTodo", "Seleccionar todos", True)

    '    '            If pLinea Is Nothing = False Then
    '    '                Call Cargar_Form()

    '    '            End If

    '    '            ' Me.GRD_Lineas.GRID.ActiveRow = Nothing
    '    '            ' Me.GRD_Lineas.GRID.Selected.Rows.Clear()
    '    '        Catch ex As Exception
    '    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '    '        End Try


    'End Sub

    '    Private Sub Cargar_Form()
    '        Try
    '            Call SetToForm()
    '            Call CargaGrid_NS(Me.TE_Producto_Codigo.Tag)
    '            Call CargaGrid_NSSeleccionats()

    '            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))


    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '    Private Function Guardar() As Boolean
    '        Guardar = False
    '        Try
    '            Me.TE_Producto_Codigo.Focus()



    '            If ComprovacioCampsRequeritsError() = True Then
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
    '                Exit Function
    '            End If

    '            If Me.T_Unidades.Value Is Nothing = True OrElse IsNumeric(Me.T_Unidades.Value) = False OrElse Me.T_Unidades.Value = 0 Then
    '                Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
    '                If _Producto.RequiereNumeroSerie = False Then
    '                    Mensaje.Mostrar_Mensaje("Imposible guardar los datos, la cantidad no puede ser 0", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                    Exit Function
    '                End If
    '            End If

    '            Call GetFromForm()

    '            If oclsEntradaLinea.EsUnaLineaNova Then
    '                If oclsEntradaLinea.AfegirLinea(True) = True Then
    '                    oDTC.SubmitChanges()
    '                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
    '                End If

    '            Else
    '                If oclsEntradaLinea.ModificaLinea Then
    '                    oDTC.SubmitChanges()
    '                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
    '                End If

    '            End If

    '            Guardar = True

    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Function

    '    Private Function ComprovacioCampsRequeritsError() As Boolean
    '        Try
    '            Dim oClsControls As New clsControles(ErrorProvider1)

    '            With Me
    '                ErrorProvider1.Clear()
    '                oClsControls.ControlBuit(.TE_Producto_Codigo)
    '                oClsControls.ControlBuit(.C_Almacen)

    '                'oClsControls.ControlBuit(.T_Unidades)

    '                'oClsControls.ControlBuit(.T_Persona_Contacto)
    '            End With

    '            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Function

    '    Private Sub SetToForm()
    '        Try
    '            With oclsEntradaLinea.oLinqLinea
    '                Me.TE_Producto_Codigo.Tag = .ID_Producto
    '                Me.TE_Producto_Codigo.Value = .Producto.Codigo
    '                Me.T_Producto_Descripcion.Text = .Producto.Descripcion
    '                Me.T_Unidades.Value = .Unidad
    '                Me.C_Almacen.Value = .ID_Almacen

    '                Call CarregarProducto()
    '            End With
    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '    Private Sub GetFromForm() 'ByRef pEntrada_Linea As Entrada_Linea
    '        Try

    '            With oclsEntradaLinea.oLinqLinea

    '                .Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.C_Almacen.Value)).FirstOrDefault

    '                If oclsEntradaLinea.EsUnaLineaNova Then 'Si la línea es nova guardarem la data de creació
    '                    .FechaEntrada = oclsEntradaLineaPare.oLinqLinea.FechaEntrada
    '                    .FechaEntrega = oclsEntradaLineaPare.oLinqLinea.FechaEntrega
    '                End If

    '                If T_Unidades.Value Is Nothing OrElse IsDBNull(T_Unidades.Value) Then
    '                    Me.T_Unidades.Value = 0
    '                End If
    '                .Unidad = Me.T_Unidades.Value
    '                .Descripcion = Me.T_Producto_Descripcion.Text

    '                If IsNumeric(Me.TE_Producto_Codigo.Tag) = True Then
    '                    .Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
    '                Else
    '                    .Producto = Nothing
    '                End If
    '                .ID_Entrada_Linea_Padre = oclsEntradaLineaPare.oLinqLinea.ID_Entrada_Linea


    '                ' .Entrada_Linea_Padre = oclsEntradaLineaPare.oLinqLinea

    '            End With

    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '    Private Sub Assignar()


    '        Dim _IDLineaPropuesta As Integer
    '        'Aquest bucle es per no traspasar les línies que no estan seleccionades

    '        oDTC.Entrada_Linea_Propuesta_Linea.DeleteAllOnSubmit(oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea)

    '        For Each _IDLineaPropuesta In RowsSeleccionadas
    '            'If oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(pRow.Cells("ID_Propuesta_Linea").Value)).Count = 0 Then
    '            Dim _Relacio As New Entrada_Linea_Propuesta_Linea
    '            _Relacio.Entrada_Linea = oclsEntradaLinea.oLinqLinea
    '            _Relacio.Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _IDLineaPropuesta).FirstOrDefault
    '            oDTC.Entrada_Linea_Propuesta_Linea.InsertOnSubmit(_Relacio)
    '            'End If
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
    '        '            .ID_Propuesta = pPropuestaSeInstalo.ID_Propuesta

    '        '            If _Linea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
    '        '                'El If de baix es per comprovar que la línea s'ha traspasat. Pq potser que el pare tingues la marca de família que no s'ha de traspasar i el fill llavors no s'ha de vincular
    '        '                If IsNothing(pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault) = True Then
    '        '                    ' DescartarLinea = True
    '        '                Else
    '        '                    .ID_Propuesta_Linea_Vinculado = pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault.ID_Propuesta_Linea
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
    '        '            .ATenerEnCuenta = _Linea.ATenerEnCuenta
    '        '            .Fase = _Linea.Fase
    '        '            .ReferenciaMemoria = _Linea.ReferenciaMemoria

    '        '            Dim _DatoAcceso As Propuesta_Linea_Acceso
    '        '            For Each _DatoAcceso In _Linea.Propuesta_Linea_Acceso
    '        '                Dim _NewDatoAcceso As New Propuesta_Linea_Acceso
    '        '                With _NewDatoAcceso
    '        '                    .Contraseña = _DatoAcceso.Contraseña
    '        '                    .Detalle = _DatoAcceso.Detalle
    '        '                    .Explicación = _DatoAcceso.Explicación
    '        '                    .Propuesta_Linea_TipoAcceso = _DatoAcceso.Propuesta_Linea_TipoAcceso
    '        '                    .Usuario = _DatoAcceso.Usuario
    '        '                    .ValorCRA = _DatoAcceso.ValorCRA
    '        '                End With
    '        '                .Propuesta_Linea_Acceso.Add(_NewDatoAcceso)
    '        '            Next
    '        '        End With
    '        '        pPropuestaSeInstalo.Propuesta_Linea.Add(_NewLinea)
    '        '        oDTC.SubmitChanges()
    '        '    Next
    '        'End If
    '        oDTC.SubmitChanges()
    '    End Sub

    '    Private Sub AlTancarLlistatGeneric(ByVal pID As String)
    '        If pID Is Nothing Then
    '        Else
    '            Call CarregarProducto()
    '        End If

    '    End Sub

    '#End Region

    '#Region "Events varis"

    '    Private Sub TE_Producto_Codigo_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs)
    '        If e.Button.Key = "Lupeta" Then
    '            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
    '            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto ORDER BY Descripcion", Me.TE_Producto_Codigo, "ID_Producto", "Codigo", Me.T_Producto_Descripcion, "Descripcion")
    '            AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGeneric


    '            'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
    '            'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
    '            '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
    '            '        pRow.Appearance.BackColor = Color.LightCoral
    '            '    End If
    '            'Next

    '        End If
    '    End Sub

    '    Private Sub TE_Producto_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs)
    '        If Me.TE_Producto_Codigo.Text Is Nothing = False Then
    '            If e.KeyCode = Keys.Enter Then
    '                Call CarregarProducto()
    '            End If
    '        End If
    '    End Sub

    '    Private Sub CarregarProducto()
    '        Dim ooLinqProducto As Producto = oDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Producto_Codigo.Text).FirstOrDefault()
    '        If ooLinqProducto Is Nothing = False Then
    '            Me.TE_Producto_Codigo.Tag = ooLinqProducto.ID_Producto
    '            Me.T_Producto_Descripcion.Text = ooLinqProducto.Descripcion
    '            If ooLinqProducto.RequiereNumeroSerie = True Then
    '                Me.T_Unidades.Enabled = False
    '                Me.B_Asignar.Enabled = True
    '                Me.B_Desasignar.Enabled = True
    '                Me.B_CarregarNS.Enabled = True
    '                Me.L_Producto_ConNS.Visible = True
    '            Else
    '                Me.T_Unidades.Enabled = True
    '                Me.B_Asignar.Enabled = False
    '                Me.B_Desasignar.Enabled = False
    '                Me.B_CarregarNS.Enabled = False
    '                Me.L_Producto_ConNS.Visible = False
    '            End If
    '            ' Me.TE_Producto_Codigo.Enabled = False
    '            Me.TE_Producto_Codigo.ReadOnly = True
    '            Me.TE_Producto_Codigo.ButtonsRight("Lupeta").Enabled = False

    '            Util.Cargar_Combo(Me.C_Almacen, "Select ID_Almacen, almacenDescripcion + ' (' + Cast(StockReal as nvarchar(10)) + ' )'  From RetornaStock(" & Me.TE_Producto_Codigo.Tag & ",0) Where StockReal>0 ORDER BY almacenDescripcion", False)
    '            If Me.C_Almacen.Items.Count = 0 Then 'Si no hi ha cap magatzem que tingui stock llavors els carregarem tots
    '                Call CarregarTotsElsMagatzems()
    '            End If
    '        Else
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
    '            Me.TE_Producto_Codigo.Value = Nothing
    '        End If
    '    End Sub

    '    Private Sub B_CarregarNS_Click(sender As Object, e As EventArgs) Handles B_CarregarNS.Click
    '        Try
    '            'If Me.C_Almacen.SelectedIndex > -1 Then
    '            Call CargaGrid_NS(Me.TE_Producto_Codigo.Tag)
    '            Call CargaGrid_NSSeleccionats()
    '            'Else
    '            '    Mensaje.Mostrar_Mensaje("Imposible cargar los datos, es obligatorio seleccionar el almacén", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
    '            'End If

    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '#End Region

    '#Region "Grid NS"

    '    Private Sub CargaGrid_NS(ByVal pId As Integer)
    '        Try

    '            With Me.GRD_NS

    '                .M.clsUltraGrid.Cargar(oclsEntradaLinea.RetornaNSDisponiblesDunArticle(pId))

    '            End With

    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '#End Region

    '#Region "Grid NS Seleccionats"

    '    Private Sub CargaGrid_NSSeleccionats()
    '        Try

    '            With Me.GRD_NS_Seleccionats

    '                .M.clsUltraGrid.Cargar(oclsEntradaLinea.RetornaNSDeLaLinea)

    '            End With

    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '#End Region

    '    Private Sub B_Asignar_Click(sender As Object, e As EventArgs)
    '        If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea = 0 Then
    '            If Guardar() = False Then
    '                Exit Sub
    '            End If
    '        End If

    '        If Me.GRD_NS.GRID.ActiveRow Is Nothing Then
    '            Exit Sub
    '        End If

    '        oclsEntradaLinea.LineaNSCrear(Me.GRD_NS.GRID.ActiveRow.Cells("ID_NS").Value)
    '        oDTC.SubmitChanges()

    '        Call CargaGrid_NS(Me.TE_Producto_Codigo.Tag)
    '        Call CargaGrid_NSSeleccionats()
    '        Me.T_Unidades.Value = Me.GRD_NS_Seleccionats.GRID.Rows.Count
    '    End Sub

    '    Private Sub B_Desasignar_Click(sender As Object, e As EventArgs)
    '        If Me.GRD_NS_Seleccionats.GRID.ActiveRow Is Nothing Then
    '            Exit Sub
    '        End If

    '        Dim _LineaNS As Entrada_Linea_NS = oDTC.Entrada_Linea_NS.Where(Function(F) F.ID_Entrada_Linea_NS = CInt(Me.GRD_NS_Seleccionats.GRID.ActiveRow.Cells("ID_Entrada_Linea_NS").Value)).FirstOrDefault

    '        oclsEntradaLinea.LineaNSEliminar(_LineaNS)
    '        oDTC.SubmitChanges()

    '        Call CargaGrid_NS(Me.TE_Producto_Codigo.Tag)
    '        Call CargaGrid_NSSeleccionats()
    '        Me.T_Unidades.Value = Me.GRD_NS_Seleccionats.GRID.Rows.Count
    '    End Sub

    '    Private Sub C_Almacen_EditorButtonClick(sender As Object, e As UltraWinEditors.EditorButtonEventArgs) Handles C_Almacen.EditorButtonClick
    '        If e.Button.Key = "Todos" Then
    '            Call CarregarTotsElsMagatzems()
    '        End If
    '    End Sub

    '    Private Sub CarregarTotsElsMagatzems()
    '        Util.Cargar_Combo(Me.C_Almacen, "SELECT ID_Almacen, Descripcion FROM Almacen Where Activo=1 ORDER BY Descripcion", False)
    '    End Sub

    '    Private Sub TE_Producto_Codigo_ValueChanged(sender As Object, e As EventArgs)

    '    End Sub


    Private Sub B_CarregarNS_Click(sender As Object, e As EventArgs) Handles B_CarregarNS.Click
        If Me.C_Almacen.SelectedIndex <> -1 Then
            Call CargaGrid_Lineas(oclsEntradaLinea.oLinqEntrada.ID_Entrada)
        End If


    End Sub


End Class