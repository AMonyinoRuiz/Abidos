Public Class clsPropuestaLinea
    'Dim oLinea2 As Propuesta_Linea
    'Dim oDTC As DTCDataContext

    'Public Sub New(ByVal pIDLinea As Integer, ByRef pDTC As DTCDataContext)
    '    oDTC = pDTC
    '    oLinea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = pIDLinea).FirstOrDefault
    'End Sub

    Public Shared Function RetornaDuplicacioInstancia(ByRef pLinea As Propuesta_Linea, Optional ByVal pAssignantInstancia As Boolean = True) As Propuesta_Linea
        Try
            Dim _NewLinea As New Propuesta_Linea
            With _NewLinea

                .Descripcion = pLinea.Descripcion
                .ID_Instalacion_ElementosAProteger = pLinea.ID_Instalacion_ElementosAProteger
                .ID_Instalacion_Emplazamiento = pLinea.ID_Instalacion_Emplazamiento
                .ID_Instalacion_Emplazamiento_Abertura = pLinea.ID_Instalacion_Emplazamiento_Abertura
                .ID_Instalacion_Emplazamiento_Planta = pLinea.ID_Instalacion_Emplazamiento_Planta
                .ID_Instalacion_Emplazamiento_Zona = pLinea.ID_Instalacion_Emplazamiento_Zona
                .ID_Instalacion_InstaladoEn = pLinea.ID_Instalacion_InstaladoEn
                .ID_Producto = pLinea.ID_Producto
                .ID_Propuesta = pLinea.ID_Propuesta
                .ID_Propuesta_Linea_Vinculado = pLinea.ID_Propuesta_Linea_Vinculado
                .ID_Propuesta_Linea_Vinculado_Energetico = pLinea.ID_Propuesta_Linea_Vinculado_Energetico
                .ID_Proveedor = pLinea.ID_Proveedor
                .Identificador = pLinea.Identificador
                .ID_Propuesta_Linea_Antigua = pLinea.ID_Propuesta_Linea
                .ID_Propuesta_Antigua = pLinea.ID_Propuesta
                .Precio = pLinea.Precio
                .Descuento = pLinea.Descuento
                .IVA = pLinea.IVA
                .Unidad = pLinea.Unidad
                .Activo = pLinea.Activo
                .ID_Propuesta_Linea_Estado = pLinea.ID_Propuesta_Linea_Estado
                .Uso = pLinea.Uso
                .NumZona = pLinea.NumZona
                .BocaConexion = pLinea.BocaConexion
                .CantidadPendienteRecibir = pLinea.CantidadPendienteRecibir
                .DescripcionAmpliada = pLinea.DescripcionAmpliada
                .DescripcionAmpliada_Tecnica = pLinea.DescripcionAmpliada_Tecnica
                .DetalleInstalacion = pLinea.DetalleInstalacion
                .FechaPrevista = pLinea.FechaPrevista
                .IdentificadorDelProducto = pLinea.IdentificadorDelProducto
                .NickZona = pLinea.NickZona
                .NumSerie = pLinea.NumSerie
                .Particion = pLinea.Particion
                .PrecioCoste = pLinea.PrecioCoste
                .RutaOrden = pLinea.RutaOrden
                .RutaParametros = pLinea.RutaParametros
                .ID_Propuesta_Linea_TipoZona = pLinea.ID_Propuesta_Linea_TipoZona
                .ATenerEnCuenta = pLinea.ATenerEnCuenta
                .Fase = pLinea.Fase
                .ReferenciaMemoria = pLinea.ReferenciaMemoria
                .VLAN = pLinea.VLAN
                .IP = pLinea.IP
                .MascaraSubred = pLinea.MascaraSubred
                .PuertaEnlace = pLinea.PuertaEnlace
                .DNSPrimaria = pLinea.DNSPrimaria
                .DNSSecundaria = pLinea.DNSSecundaria
                .IPPublica = pLinea.IPPublica
                .ServidorWINS = pLinea.ServidorWINS
                .Dominio = pLinea.Dominio
                .NombreEquipo = pLinea.NombreEquipo
                .NetBios = pLinea.NetBios
                .ID_SistemaOperativo = pLinea.ID_SistemaOperativo
                .AlmacenamientoEnDisco = pLinea.AlmacenamientoEnDisco
                .MemoriaRam = pLinea.MemoriaRam
                .Procesador = pLinea.Procesador
                .MacAdress = pLinea.MacAdress
                .ID_Producto_SubFamilia_Traspaso = pLinea.ID_Producto_SubFamilia_Traspaso

                .FechaFabricacion = pLinea.FechaFabricacion
                .FechaFinalVidaUtil = pLinea.FechaFinalVidaUtil
                .ImporteOpcion = pLinea.ImporteOpcion
                .PlazoEntrega = pLinea.PlazoEntrega

                If pAssignantInstancia = True Then
                    .Propuesta = pLinea.Propuesta
                    .Propuesta_Linea = pLinea.Propuesta_Linea
                    .Propuesta_Linea_VinculadoEnergeticoPadre = pLinea.Propuesta_Linea_VinculadoEnergeticoPadre
                End If


                Dim _DatoAcceso As Propuesta_Linea_Acceso
                For Each _DatoAcceso In pLinea.Propuesta_Linea_Acceso
                    Dim _NewDatoAcceso As New Propuesta_Linea_Acceso
                    With _NewDatoAcceso
                        .Contraseña = _DatoAcceso.Contraseña
                        .Detalle = _DatoAcceso.Detalle
                        .Explicación = _DatoAcceso.Explicación
                        .Propuesta_Linea_TipoAcceso = _DatoAcceso.Propuesta_Linea_TipoAcceso
                        .Usuario = _DatoAcceso.Usuario
                        .ValorCRA = _DatoAcceso.ValorCRA
                    End With
                    .Propuesta_Linea_Acceso.Add(_NewDatoAcceso)
                Next
            End With


            Return _NewLinea
        Catch ex As Exception
            pLinea = Nothing
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Function RetornaUltimIdentificadorDeLinea(ByRef pDTC As DTCDataContext, ByVal pIDPropuesta As Integer) As Integer
        Try
            If pDTC.Propuesta_Linea.Count = 0 Then
                Return 1
            Else
                Return pDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta = pIDPropuesta).Max(Function(F) F.Identificador) + 10
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Shared Sub AssignaFillALaLinea(ByRef pLinea As Propuesta_Linea, ByVal pIDArticle As Integer, ByRef pDTC As DTCDataContext, ByVal pIdentificadorLinea As Integer, ByVal pQuantitat As Decimal, ByVal pPVP As Decimal, ByVal pPVD As Decimal, ByVal pDescuento As Decimal, ByVal pIVA As Decimal)
        Try
            If pLinea.Propuesta.SeInstalo = True OrElse pLinea.Propuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then
                Dim i As Integer
                For i = 1 To pQuantitat
                    Call CreacioFill(pLinea, pIDArticle, pDTC, pIdentificadorLinea, 1, pPVP, pPVD, pDescuento, pIVA)
                Next
            Else
                Call CreacioFill(pLinea, pIDArticle, pDTC, pIdentificadorLinea, pQuantitat, pPVP, pPVD, pDescuento, pIVA)
            End If




            '_LineaNova.ID_Propuesta_Linea_Vinculado = pLinea.ID_Propuesta_Linea
            'oDTC.Propuesta_Linea.InsertOnSubmit(_LineaNova)
            'oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Shared Sub CreacioFill(ByRef pLinea As Propuesta_Linea, ByVal pIDArticle As Integer, ByRef pDTC As DTCDataContext, ByVal pIdentificadorLinea As Integer, ByVal pQuantitat As Decimal, ByVal pPVP As Decimal, ByVal pPVD As Decimal, ByVal pDescuento As Decimal, ByVal pIVA As Decimal)
        Try
            Dim _LineaNova As New Propuesta_Linea
            _LineaNova = clsPropuestaLinea.RetornaDuplicacioInstancia(pLinea)
            Dim _Producto As Producto = pDTC.Producto.Where(Function(F) F.ID_Producto = pIDArticle).FirstOrDefault

            _LineaNova.Descripcion = _Producto.Descripcion
            _LineaNova.Producto = _Producto
            _LineaNova.Identificador = pIdentificadorLinea 'clsPropuestaLinea.RetornaUltimIdentificadorDeLinea(pDTC)
            _LineaNova.DescripcionAmpliada = _Producto.DescripcionAmpliada
            _LineaNova.Producto_SubFamilia_Traspaso = _Producto.Producto_SubFamilia.Producto_SubFamilia_Traspaso

            _LineaNova.Precio = pPVP
            _LineaNova.PrecioCoste = pPVD
            _LineaNova.Descuento = pDescuento
            _LineaNova.IVA = pIVA
            _LineaNova.Unidad = pQuantitat
            _LineaNova.Propuesta_Linea = pLinea
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub


End Class
