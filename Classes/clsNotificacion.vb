Public Class clsNotificacion
    Dim oDTC As DTCDataContext
    Dim oLinqNotificacion As Notificacion

    Enum TipoNotificacion
        AlCrearUnPedidoVenta = 1
        AlCrearUnParte = 2
        AlAsignarUnMaterialAUnAlmacenDesdeImputacionHoras = 3
        AlAñadirOModificarUnInformeTecnicoDesdeImputacionHoras = 4
        AlCrearUnCliente = 5
        AlMarcarUnProductoComoObsoleto = 6
        AlCrearUnPresupuesto = 7
        AlModificarOAñadirUnaLineaDeTalYComoSeInstalo = 8
        AlAñadirUnToDoDesdeLaPantallaImputacionHoras = 9
        AlAñadirUnToDoDesdeLaPantallaParte = 10
    End Enum

    Public Sub New(ByRef pDTC As DTCDataContext)
        oDTC = pDTC
        ' oLinqNotificacion = New Notificacion
    End Sub

    Private Sub CrearNotificacion(ByVal pRealizado As Boolean, ByVal pLeido As Boolean, ByVal pAutomatica As Boolean, ByVal pDescripcion As String, ByVal pFechaAlta As Date?, ByVal pFechaLimite As Date, ByRef pFormulario As Formulario, ByVal pIdentificadorParaAbrirFormulario As Integer, ByVal pTipoNotificacion As TipoNotificacion, ByVal pIDUsuarioOrigen As Integer)
        Try
            Dim _Rel As Notificacion_Automatica_Usuario

            Dim _GentQueTeAquestaNotificacioActivada As IEnumerable(Of Notificacion_Automatica_Usuario) = oDTC.Notificacion_Automatica_Usuario.Where(Function(F) F.ID_Notificacion_Automatica_Tipo = pTipoNotificacion And F.ID_Usuario <> Seguretat.oUser.ID_Usuario)

            For Each _Rel In _GentQueTeAquestaNotificacioActivada
                Dim _NovaNotificacio As New Notificacion
                With _NovaNotificacio
                    .Realizado = pRealizado
                    .Leido = pLeido
                    .Automatica = pAutomatica
                    .Descripcion = pDescripcion
                    .FechaAlta = pFechaAlta
                    If pAutomatica = False Then
                        .FechaLimite = pFechaLimite
                    End If
                    .Formulario = pFormulario
                    .IdentificadorParaAbrirFormulario = pIdentificadorParaAbrirFormulario
                    .Notificacion_Automatica_Tipo = oDTC.Notificacion_Automatica_Tipo.Where(Function(F) F.ID_Notificacion_Automatica_Tipo = pTipoNotificacion).FirstOrDefault
                    .Usuario_Origen = oDTC.Usuario.Where(Function(F) F.ID_Usuario = pIDUsuarioOrigen).FirstOrDefault
                    .Usuario_Destino = _Rel.Usuario
                    oDTC.Notificacion.InsertOnSubmit(_NovaNotificacio)
                End With
            Next
            oLinqNotificacion = New Notificacion
            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Public Sub CrearNotificacion_AlCrearCliente(ByRef pCliente As Cliente)
        Try

            Dim _Descripcio As String = "El usuario: " & Seguretat.oUser.Nombre & " ha creado el cliente con código: " & pCliente.Codigo & " y nombre: " & pCliente.Nombre
            Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = "frmCliente").FirstOrDefault
            Call CrearNotificacion(False, False, True, _Descripcio, Now, Nothing, _Formulario, pCliente.ID_Cliente, TipoNotificacion.AlCrearUnCliente, Seguretat.oUser.ID_Usuario)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearNotificacion_AlCrearParte(ByRef pParte As Parte)
        Try

            Dim _Descripcio As String = "El usuario: " & Seguretat.oUser.Nombre & " ha creado el parte con código: " & pParte.ID_Parte ' & " y nombre: " & pCliente.Nombre
            Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = "frmParte").FirstOrDefault
            Call CrearNotificacion(False, False, True, _Descripcio, Now, Nothing, _Formulario, pParte.ID_Parte, TipoNotificacion.AlCrearUnParte, Seguretat.oUser.ID_Usuario)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearNotificacion_AlCrearPresupuesto(ByRef pPropuesta As Propuesta)
        Try

            Dim _Descripcio As String = "El usuario: " & Seguretat.oUser.Nombre & " ha creado el presupuesto con código: " & pPropuesta.Codigo & " para el cliente: " & pPropuesta.Instalacion.Cliente.Nombre
            Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = "frmPropuesta").FirstOrDefault
            Call CrearNotificacion(False, False, True, _Descripcio, Now, Nothing, _Formulario, pPropuesta.ID_Propuesta, TipoNotificacion.AlCrearUnPresupuesto, Seguretat.oUser.ID_Usuario)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearNotificacion_AlMarcaUnProductoComoObsoleto(ByRef pProducto As Producto)
        Try

            Dim _Descripcio As String = "El usuario: " & Seguretat.oUser.Nombre & " ha indicado como obsoleto el producto con código: " & pProducto.Codigo & " con descripción: " & pProducto.Descripcion
            Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = "frmProducto").FirstOrDefault
            Call CrearNotificacion(False, False, True, _Descripcio, Now, Nothing, _Formulario, pProducto.ID_Producto, TipoNotificacion.AlMarcarUnProductoComoObsoleto, Seguretat.oUser.ID_Usuario)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearNotificacion_AlCrearPedidoVenta(ByRef pPedidoVenta As Entrada)
        Try
            Dim _Descripcio As String = "El usuario: " & Seguretat.oUser.Nombre & " ha creado el pedido de venta con código: " & pPedidoVenta.Codigo & " para el cliente: " & pPedidoVenta.Cliente.Nombre
            Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = "frmEntrada").FirstOrDefault
            Call CrearNotificacion(False, False, True, _Descripcio, Now, Nothing, _Formulario, pPedidoVenta.ID_Entrada, TipoNotificacion.AlCrearUnPedidoVenta, Seguretat.oUser.ID_Usuario)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearNotificacion_AlAssignarMaterialUnTecnicoAUnAlmacenDesdeImputacionDeTecnicos(ByRef pParte As Parte)
        Try

            Dim _Descripcio As String = "El usuario: " & Seguretat.oUser.Nombre & " ha asignado material de su almacén al parte: " & pParte.ID_Parte
            Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = "frmParte").FirstOrDefault
            Call CrearNotificacion(False, False, True, _Descripcio, Now, Nothing, _Formulario, pParte.ID_Parte, TipoNotificacion.AlAsignarUnMaterialAUnAlmacenDesdeImputacionHoras, Seguretat.oUser.ID_Usuario)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearNotificacion_AlAñadirOModificarUnInformeTecnicoDesdeImputacionDeTecnicos(ByRef pParte As Parte)
        Try

            Dim _Descripcio As String = "El usuario: " & Seguretat.oUser.Nombre & " ha modificado un el informe técnico del parte: " & pParte.ID_Parte
            Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = "frmParte").FirstOrDefault
            Call CrearNotificacion(False, False, True, _Descripcio, Now, Nothing, _Formulario, pParte.ID_Parte, TipoNotificacion.AlAñadirOModificarUnInformeTecnicoDesdeImputacionHoras, Seguretat.oUser.ID_Usuario)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearNotificacion_AlAñadirUnToDoDesdeLaPantallaImputacionDeTecnicos(ByRef pParte As Parte)
        Try

            Dim _Descripcio As String = "El usuario: " & Seguretat.oUser.Nombre & " ha añadido un To Do en el parte: " & pParte.ID_Parte
            Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = "frmParte").FirstOrDefault
            Call CrearNotificacion(False, False, True, _Descripcio, Now, Nothing, _Formulario, pParte.ID_Parte, TipoNotificacion.AlAñadirUnToDoDesdeLaPantallaImputacionHoras, Seguretat.oUser.ID_Usuario)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearNotificacion_AlAñadirUnToDoDesdeLaPantallaParte(ByRef pParte As Parte)
        Try

            Dim _Descripcio As String = "El usuario: " & Seguretat.oUser.Nombre & " ha añadido un To Do en el parte: " & pParte.ID_Parte
            Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = "frmParte").FirstOrDefault
            Call CrearNotificacion(False, False, True, _Descripcio, Now, Nothing, _Formulario, pParte.ID_Parte, TipoNotificacion.AlAñadirUnToDoDesdeLaPantallaParte, Seguretat.oUser.ID_Usuario)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

End Class
