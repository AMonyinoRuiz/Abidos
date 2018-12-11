Public Module Declaracio
    Public BD As M_Conexion.clsConexion
    Public BD2 As M_Conexion.clsConexion
    Public Util As M_Util.clsFunciones
    Public Mensaje As M_Mensaje.clsMensaje
    Public Seguretat As M_Seguretat.clsSeguretat
    Public frmPrincipal As Principal
    Public Informes As M_Informes.clsInformes
    Public LlistatsADV As M_LlistatADV.clsLlistatADV

    'Public oUser As Usuario
    Public oclsPilaFormularis As New clsPilaFormularis


    Public PublicProductoDivision() As Producto_Division

    Public Temps As Long

    Public Const ConstNivellSeguretatMaxim As Integer = 100
    Public MenuPrincipal As clsMenuPrincipal
    Public oEmpresa As Empresa

    Enum EnumEstatPantalla As Integer
        Nuevo = 1
        DespresDeCargar = 2
        DespresDeGuardar = 3
    End Enum

    Enum EnumProductoDivision As Integer
        Intrusion = 1
        CCTV = 2
        Incendios = 3
        Accesos = 4
        Interno = 5
        Megafonia = 6
        Material = 7
        Informatica = 8
    End Enum

    Enum EnumPropuestaEstado As Integer
        Pendiente = 1
        Aprobado = 2
        Traspasado = 3
        Negativo = 4
    End Enum

    Enum EnumProductoSubFamiliaTraspaso As Integer
        No = 0
        Si = 1
        Preguntar = 2
    End Enum

    Enum EnumProductoSubFamiliaTipo As Integer
        Material = 1
        ManoDeObra = 2
        Varios = 3
    End Enum

    Enum EnumProductoCaracteristicaVision
        Presupuesto = 1
        Instalacion = 2
        Revision = 3
        Instalacion_Revision = 4
        NoTenerEnCuenta = 5
        Todos = 6
    End Enum

    Enum EnumPropuestaLineaEstado As Integer
        PendienteDeAprobar = 1
        Aprobado_PendienteRecibir = 2
        RecepcionadoParcialmente = 3
        Recepcionadototalmente = 4
        Traspasada = 5
    End Enum

    Enum EnumPropuestaTipo As Integer
        NuevaInstalacion = 1
        AmpliacionInstalacion = 2
        InstalacionAnterior = 3
    End Enum

    Enum EnumParteTipo As Integer
        Reparacion = 1
        Revision = 2
        Instalacion = 3
    End Enum

    Enum EnumParteEstado As Integer
        Pendiente = 1
        EnCurso = 2
        Finalizado = 3
    End Enum

    Enum EnumParteRevisionEstado As Integer
        NoSeRequiereRevision = 1
        PendienteRevisar = 2
        Correcto = 3
        Incorrecto = 4
        Solucionado = 5
    End Enum

    Enum EnumParteReparacionTipo As Integer
        ReparacionInterna = 1
        ReparacionExterna = 2
        SubstitucionArticulo = 3
    End Enum

    Enum EnumPersonalTipo As Integer
        Comercial = 1
        Tecnico = 2
        ResponsableCuenta = 3
    End Enum

    Enum EnumInstalacionEstado As Integer
        Pendiente = 1
        Diseño = 2
        Presupuestado = 3
        Instalacion = 4
        Instalado = 5
        Negativa = 6
    End Enum

    Enum EnumInforme As Integer
        Propuesta = 1
        Instalacion = 2
        ParteTrabajo = 3
        Articulo = 4
        Personal = 5
        PedidoCompra = 11
        AlbaranCompra = 12
        FacturaCompra = 13
        DevolucionCompra = 14
        Regularizacion = 15
        DevolucionVenta = 16
        PedidoVenta = 17
        AlbaranVenta = 18
        FacturaVenta = 19
        TraspasoAlmacen = 20
        InstalacionContrato = 21
        FacturaCompraRectificativa = 22
        FacturaVentaRectificativa = 23
        Bonos = 24
    End Enum

    Enum EnumParteTipoFacturacion As Integer
        Facturable = 1
        Bono = 2
        Garantia = 3
    End Enum

    Enum EnumParteHorasEstado As Integer
        Correcto = 1
        Incorrecto = 2
        Reparado = 3
    End Enum

    Enum EnumEntradaTipo As Integer
        NoEspecificado = 0
        PedidoCompra = 1
        AlbaranCompra = 2
        FacturaCompra = 3
        DevolucionCompra = 4
        Regularizacion = 5
        Inicializacion = 6
        PedidoVenta = 7
        AlbaranVenta = 8
        FacturaVenta = 9
        TraspasoAlmacen = 10
        DevolucionVenta = 11
        FacturaVentaRectificativa = 12
        FacturaCompraRectificativa = 13
    End Enum

    Enum EnumEntradaEstado As Integer
        Pendiente = 1
        Parcial = 2
        Cerrado = 3
    End Enum

    Enum EnumClienteTipo As Integer
        Cliente = 1
        FuturoCliente = 2
    End Enum

    Enum EnumCampaña_Cliente_Seguimiento_Estado As Integer
        Pendiente = 1
    End Enum

    Enum EnumCampañaEstado As Integer
        Abierta = 1
        Cerrada = 2
    End Enum

    Enum EnumIdioma As Integer
        Castella = 1
        Catala = 2
    End Enum

    Enum EnumNSEstado As Integer
        Disponible = 1
        NoDisponible = 2
    End Enum

    Enum EnumAlmacenTipo As Integer
        Interno = 1
        Personal = 2
        Cliente = 3
        Proveedor = 4
        Parte = 5
    End Enum

    Enum EnumMenuTipo As Integer
        Carpeta = 1
        Formulario = 2
        Listado = 3
        Informe = 4
        Gauge = 5
        CuadroDeMando = 6
    End Enum

    Enum EnumInstalacionTipo As Integer
        Instalacion = 1
        FuturaInstalacion = 2
    End Enum

    Enum EnumVencimientoEstado As Integer
        PendienteAsignarARemesa = 1
        PendienteGenerarArchivo = 2
        ReciboExportado = 3
        ReciboEnviadoBanco = 4
        Cobrado = 5
        Devuelto = 6
    End Enum
End Module
