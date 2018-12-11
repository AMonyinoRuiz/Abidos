Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmCliente
    Dim oDTC As DTCDataContext
    Dim oLinqCliente As Cliente
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2
    Dim oTipoEntrada As TipoEntrada
    Enum TipoEntrada
        Cliente = 1
        FuturoCliente = 2
    End Enum


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqCliente.ID_Cliente <> 0 Then
                If Mensaje.Mostrar_Mensaje("Desea dar de alta/baja el registro?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then

                    ' oLinqCliente.Activo = False
                    If DT_Baja.Value Is Nothing Then
                        Me.DT_Baja.Value = Date.Now
                        oLinqCliente.Activo = False
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                    Else
                        Me.DT_Baja.Value = Nothing
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                    End If
                    Call Guardar()

                    'oDTC.SubmitChanges()
                    'Call Netejar_Pantalla()
                End If
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)

        End Try
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Call Guardar()
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Nuevo() Handles ToolForm.m_ToolForm_Nuevo
        Call Netejar_Pantalla()
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Buscar() Handles ToolForm.m_ToolForm_Buscar
        Call Cridar_Llistat_Generic()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pId As Integer = 0, Optional ByVal pTipoEntrada As TipoEntrada = TipoEntrada.Cliente)
        Try

            Me.AplicarDisseny()

            Util.Cargar_Combo(Me.C_OrigenCliente, "SELECT ID_Cliente_Origen, Descripcion FROM Cliente_Origen WHERE Activo=1 ORDER BY Descripcion", False)

            Util.Cargar_Combo(Me.C_Sector, "SELECT ID_Sector, Descripcion FROM Sector  ORDER BY Descripcion", False)
            Util.Cargar_Combo(Me.C_Tipo_Cliente, "SELECT ID_Cliente_Tipo, Descripcion FROM Cliente_Tipo  ORDER BY Descripcion", True)
            Util.Cargar_Combo(Me.C_Tipo_Documento, "SELECT ID_Entrada_Tipo, Descripcion FROM Entrada_Tipo Where Tipo='V' ORDER BY ID_Entrada_Tipo", False)
            Util.Cargar_Combo(Me.C_FormaPago, "SELECT ID_FormaPago, Descripcion FROM FormaPago Where Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_Pais, "SELECT ID_Pais, Nombre FROM Pais ORDER BY Nombre", True)
            Util.Cargar_Combo(Me.C_Delegacion, "SELECT ID_Delegacion, Descripcion FROM Delegacion ORDER BY Descripcion", False)

            Me.GRD_CuentasBancarias.M.clsToolBar.Boto_Afegir("Predeterminar", "Predeterminar")

            'Carregarem 31 dias de la setmana al combo
            Dim i As Integer
            For i = 1 To 31
                Me.C_DiaPago.Items.Add(i, i)
            Next

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Cliente_Archivo", 1)

            oTipoEntrada = pTipoEntrada

            Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
            Call Netejar_Pantalla()

            Select pTipoEntrada
                Case TipoEntrada.Cliente

                Case TipoEntrada.FuturoCliente
                    Me.EstableixCaptionForm("Futuro cliente")
                    Util.Tab_InVisible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "ListadoInstalaciones", "ListadoPropuestas", "PersonalAceptado", "Partes", "Seguridad", "Step")
                    ' Util.Tab_InVisible_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "FormaPago")
            End Select

            If pId <> 0 Then
                Call Cargar_Form(pId)
            End If

            Me.KeyPreview = False

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.TE_Codigo.Focus()

            If oLinqCliente.ID_Cliente = 0 Then
                If BD.RetornaValorSQL("SELECT Count (*) From Cliente WHERE ID_Cliente_Tipo=" & oTipoEntrada & " and Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            Else
                If BD.RetornaValorSQL("SELECT Count (*) From Cliente WHERE ID_Cliente_Tipo=" & oTipoEntrada & " and Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_Cliente<>" & oLinqCliente.ID_Cliente) > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            End If

            If oDTC.Personal.Where(Function(F) F.ID_Personal_Tipo = CInt(EnumPersonalTipo.Comercial)).Count > 0 And Me.C_Comercial.SelectedIndex = -1 Then
                Mensaje.Mostrar_Mensaje("Imposible guardar el cliente, es obligatorio seleccionar un comercial debido a que tiene uno o más personas asignadas como comercial", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqCliente)

            If oLinqCliente.ID_Cliente = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oLinqCliente.Activo = True 'Fem això aqui pq no ho fem al GetFromForm ja que si no no es podria borrar mai (ja que el eliminar després fa un guardar)
                oDTC.Cliente.InsertOnSubmit(oLinqCliente)
                oDTC.SubmitChanges()
                Call Fichero.Cargar_GRID(oLinqCliente.ID_Cliente) 'Fem això pq la classe tingui el ID de client
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeGuardar)
                If oLinqCliente.ID_Cliente_Tipo = EnumClienteTipo.Cliente Then
                    Dim _ClsNotificacion As New clsNotificacion(oDTC)
                    _ClsNotificacion.CrearNotificacion_AlCrearCliente(oLinqCliente)
                End If

                'Guardar el usuari actual en els permisos de seguretat quan es un Client (pels futurs clients No ho farem)
                If oTipoEntrada = TipoEntrada.Cliente Then
                    Dim _NewSeguretat As New Cliente_Seguridad
                    _NewSeguretat.Usuario = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault
                    oLinqCliente.Cliente_Seguridad.Add(_NewSeguretat)
                    oDTC.SubmitChanges()
                    Call CargaGrid_Seguridad(oLinqCliente.ID_Cliente)
                End If


                'Automàticament al insertar un registre afegirem la empresa predeterminada
                Dim _ClientEmpresa As New Cliente_Empresa
                _ClientEmpresa.ID_Empresa = oEmpresa.ID_Empresa
                _ClientEmpresa.Predeterminada = True
                oLinqCliente.Cliente_Empresa.Add(_ClientEmpresa)
                oDTC.SubmitChanges()

                Call EstableixCaptionForm("Cliente: " & oLinqCliente.Nombre)
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If

            'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqCliente
                Me.TE_Codigo.Value = .Codigo
                Me.T_Nombre.Text = .Nombre
                Me.T_NombreComercial.Text = .NombreComercial
                Me.T_NIF.Text = .NIF
                Me.T_Persona_Contacto.Text = .PersonaContacto
                Me.T_Email.Text = .Email
                Me.T_Telefono.Text = .Telefono
                Me.T_Fax.Text = .Fax
                Me.T_Direccion.Text = .Direccion
                Me.T_Poblacion.Text = .Poblacion
                Me.T_Provincia.Text = .Provincia
                Me.T_CP.Text = .CP
                Me.C_OrigenCliente.Value = .ID_Cliente_Origen
                Me.C_Tipo_Cliente.Value = .ID_Cliente_Tipo

                If IsNothing(.ID_Personal) Then
                    Me.C_Comercial.Value = 0
                Else
                    Me.C_Comercial.Value = .ID_Personal
                End If

                If IsNothing(.ID_Cliente_Sector) Then
                    Me.C_Sector.Value = 0
                Else
                    Me.C_Sector.Value = .ID_Cliente_Sector
                End If

                Me.C_Pais.Value = .ID_Pais

                If .ID_Delegacion.HasValue = False Then
                    Me.C_Delegacion.SelectedIndex = -1
                Else
                    Me.C_Delegacion.Value = .ID_Delegacion
                End If
 
                Me.T_CapitalSocial.Value = .CapitalSocial
                Me.T_Facturacion.Value = .Facturacion
                Me.T_NumTrabajadores.Value = .NumTrabajadores
                Me.T_RiesgoMaximo.Value = .RiesgoMaximo

                Me.T_Longitud.Value = .Longitud
                Me.T_Latitud.Value = .Latitud

                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Baja.Value = .FechaBaja
                If DT_Baja.Value Is Nothing Then
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                Else
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                End If

                Me.R_Observaciones.pText = .Observaciones
                Me.T_URLAcceso.Text = .URLAcceso
                Me.T_Usuario.Text = .Usuario
                Me.T_Contraseña.Text = .Contraseña

                If .ID_FormaPago.HasValue Then
                    Me.C_FormaPago.Value = .ID_FormaPago
                End If

                If .DiaDePago.HasValue Then
                    Me.C_DiaPago.Value = .DiaDePago
                End If

                Me.DT_UltimaCampaña.Value = .FechaUltimaAsignacionCampaña
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pCliente As Cliente)
        Try
            With pCliente
                '.Cliente = (From Taula In oDTC.Cliente Where Taula.ID_Cliente = CInt(Me.T_Nombre.Tag) Select Taula).First
                .Codigo = Me.TE_Codigo.Value
                .Nombre = Me.T_Nombre.Text
                .NombreComercial = Me.T_NombreComercial.Text
                .NIF = Me.T_NIF.Text
                .PersonaContacto = Me.T_Persona_Contacto.Text
                .Email = Me.T_Email.Text
                .Telefono = Me.T_Telefono.Text
                .Fax = Me.T_Fax.Text
                .Direccion = Me.T_Direccion.Text
                .Poblacion = Me.T_Poblacion.Text
                .Provincia = Me.T_Provincia.Text
                .CP = Me.T_CP.Text
                .ID_Cliente_Origen = Me.C_OrigenCliente.Value
                .FechaAlta = Me.DT_Alta.Value
                .FechaBaja = Me.DT_Baja.Value
                .Observaciones = Me.R_Observaciones.pText

                .Longitud = Util.DbnullToNothing(Me.T_Longitud.Value)
                .Latitud = Util.DbnullToNothing(Me.T_Latitud.Value)

                If Me.C_Comercial.Value = 0 Then
                    .Personal = Nothing
                Else
                    .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_Comercial.Value)).FirstOrDefault
                End If
                .URLAcceso = Me.T_URLAcceso.Text
                .Usuario = Me.T_Usuario.Text
                .Contraseña = Me.T_Contraseña.Text

                If Me.C_Sector.Value = 0 Then
                    .Cliente_Sector = Nothing
                Else
                    .Cliente_Sector = oDTC.Cliente_Sector.Where(Function(F) F.ID_Cliente_Sector = CInt(Me.C_Sector.Value)).FirstOrDefault
                End If
                .CapitalSocial = Util.Comprobar_NULL_Per_0_Decimal(Me.T_CapitalSocial.Value)
                .Facturacion = Util.Comprobar_NULL_Per_0_Decimal(Me.T_Facturacion.Value)
                .NumTrabajadores = Util.Comprobar_NULL_Per_0(Me.T_NumTrabajadores.Value)
                .RiesgoMaximo = Util.Comprobar_NULL_Per_0(Me.T_RiesgoMaximo.Value)
                .Cliente_Tipo = oDTC.Cliente_Tipo.Where(Function(F) F.ID_Cliente_Tipo = CInt(Me.C_Tipo_Cliente.Value)).FirstOrDefault

                If Me.C_FormaPago.Value = 0 Then
                    .FormaPago = Nothing
                Else
                    .FormaPago = oDTC.FormaPago.Where(Function(F) F.ID_FormaPago = CInt(Me.C_FormaPago.Value)).FirstOrDefault
                End If

                If Me.C_DiaPago.Value = 0 Then
                    .DiaDePago = Nothing
                Else
                    .DiaDePago = Me.C_DiaPago.Value
                End If

                .Pais = oDTC.Pais.Where(Function(F) F.ID_Pais = CInt(Me.C_Pais.Value)).FirstOrDefault

                If Me.C_Delegacion.Value = 0 Then
                    .Delegacion = Nothing
                Else
                    .Delegacion = oDTC.Delegacion.Where(Function(F) F.ID_Delegacion = CInt(Me.C_Delegacion.Value)).FirstOrDefault
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer, Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        Try
            Call Netejar_Pantalla(pNoCanviarALaPestanyaGeneral)

            oLinqCliente = (From taula In oDTC.Cliente Where taula.ID_Cliente = pID Select taula).First

            If ComprovarPermisosSeguretat() = False Then
                Exit Sub
            End If

            Call SetToForm()

            'Call CargaGrid_Instalaciones(pID)
            'Call CargaGrid_Contacto(pID)
            'Call CargarGrid_Partes()
            'Call CargaGrid_PersonalAceptado(pID)
            'Call CargaGrid_Propuestas(pID)
            'Call CargaGrid_Seguridad(pID)
            'Call CargaGrid_Marketing(pID)
            'Call CargaGrid_ProductosPresupuestados(pID)
            'Call CargaGrid_CuentasBancarias(pID)
            'Call CargaGrid_Direcciones(pID)
            'Call CargaGrid_ProductosInteres(pID)

            Call CargaGrid_Direcciones(pID)

            Fichero.Cargar_GRID(pID)

            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            If oTipoEntrada = TipoEntrada.Cliente Then
                Me.EstableixCaptionForm("Cliente: " & (oLinqCliente.Nombre))
            Else
                Me.EstableixCaptionForm("Futuro cliente: " & (oLinqCliente.Nombre))
            End If

            Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeCargar)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla(Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        oLinqCliente = New Cliente
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing

        Util.Cargar_Combo(Me.C_Comercial, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and ID_Personal_Tipo=" & EnumPersonalTipo.Comercial & " ORDER BY Nombre", False, True, "No asignado")

        Me.DT_Alta.Value = Now.Date
        Me.DT_Baja.Value = Nothing
        Me.C_Comercial.Value = 0
        Me.C_Sector.Value = 0
        Me.C_Tipo_Cliente.Value = CInt(oTipoEntrada)
        Me.C_DiaPago.Value = Nothing
        Me.C_FormaPago.Value = Nothing

        ' Me.C_Tipo_Documento.Value = Nothing

        Me.GRD_Step.GRID.DataSource = Nothing

        Me.C_Pais.Value = oDTC.Pais.Where(Function(F) F.Predeterminat = True).FirstOrDefault.ID_Pais
        Me.C_Delegacion.SelectedIndex = -1


        'Call CargaGrid_Instalaciones(0)
        'Call CargarGrid_Partes()
        'Call CargaGrid_PersonalAceptado(0)
        'Call CargaGrid_Seguridad(0)
        'Call CargaGrid_Marketing(0)
        'Call CargaGrid_Contacto(0)
        'Call CargaGrid_ProductosPresupuestados(0)
        'Call CargaGrid_CuentasBancarias(0)
        Call CargaGrid_Direcciones(0)

        'Call CargaGrid_ProductosInteres(0)
        Fichero.Cargar_GRID(0)

        ' Me.C_OrigenCliente.Value = 1
        ' Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Instalacion")
        Call ActivarSegonsEstatPantalla(EnumEstatPantalla.Nuevo)
        If pNoCanviarALaPestanyaGeneral = False Then
            Me.TAB_Principal.Tabs("General").Selected = True
        End If
        ErrorProvider1.Clear()

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.T_Nombre)
                'oClsControls.ControlBuit(.T_Persona_Contacto)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric

        'si el personal que esta mirant i te la marca de versoloclientesdondesetenga permisos pasarà això
        Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
        Dim _SQL As String = ""
        If _Personal.VerSoloClientesDondeSeTengaPermisos = True And oTipoEntrada = EnumClienteTipo.Cliente Then
            _SQL = " and ID_Cliente in (Select ID_Cliente From Cliente_Seguridad Where Cliente_Seguridad.ID_usuario=" & Seguretat.oUser.ID_Usuario & ") "
        End If

        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Cliente Where ID_Cliente_Tipo=" & oTipoEntrada & _SQL & " ORDER BY Nombre", Me.TE_Codigo, "ID_Cliente", "ID_Cliente")

        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat

        'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
        '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
        '        pRow.Appearance.BackColor = Color.LightCoral
        '    End If
        'Next
    End Sub

    Private Sub AlTancarLlistat(ByVal pID As String)
        Try
            Call Cargar_Form(pID)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub ActivarSegonsEstatPantalla(ByVal Estat As EnumEstatPantalla)
        Select Case Estat
            Case EnumEstatPantalla.Nuevo
                Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General")
            Case EnumEstatPantalla.DespresDeCargar
                Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)
            Case EnumEstatPantalla.DespresDeGuardar
                Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)
        End Select
    End Sub

    Private Function ComprovarPermisosSeguretat() As Boolean
        Try
            ComprovarPermisosSeguretat = False

            'si no s'ha afegit ningú al llistat es accessible per tothom
            If oLinqCliente.Cliente_Seguridad.Count = 0 Then
                Return True
            End If

            'Si te nivell de seguretat 1 te accés accessible
            If Seguretat.oUser.NivelSeguridad = 1 Then
                Return True
            End If

            'si esta dins de la llista te accés
            If oLinqCliente.Cliente_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count = 1 Then
                Return True
            End If

            Mensaje.Mostrar_Mensaje("Imposible cargar los datos, no tiene permisos", M_Mensaje.Missatge_Modo.INFORMACIO)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub CalcularRiesgoStep()
        Me.T_PedidosPendientesServir.Value = 0
        Me.T_TotalPendienteCobro.Value = 0
        Me.T_AlbaranesPendientesFacturar.Value = 0
        Me.T_CreditoDisponible.Value = 0
        Me.T_FacturasPendientesCobro.Value = 0
        Me.T_CreditoDisponible.Value = 0

        If oLinqCliente Is Nothing OrElse oLinqCliente.ID_Cliente = 0 Then
            Exit Sub
        End If

        Me.T_PedidosPendientesServir.Value = oDTC.Entrada_Linea.Where(Function(F) F.Entrada.ID_Cliente = oLinqCliente.ID_Cliente And F.Entrada.ID_Entrada_Estado <> EnumEntradaEstado.Cerrado And F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.PedidoVenta).Sum(Function(T) T.TotalBase)
        Me.T_AlbaranesPendientesFacturar.Value = oDTC.Entrada_Linea.Where(Function(F) F.Entrada.ID_Cliente = oLinqCliente.ID_Cliente And F.Entrada.ID_Entrada_Estado <> EnumEntradaEstado.Cerrado And F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranVenta And F.ID_Entrada_Linea_Padre.HasValue = False).Sum(Function(T) T.TotalBase)
        Me.T_TotalPendienteCobro.Value = Util.Comprobar_NULL_Per_0_Decimal(Me.T_PedidosPendientesServir.Value) + Util.Comprobar_NULL_Per_0_Decimal(Me.T_AlbaranesPendientesFacturar.Value)
        Me.T_CreditoDisponible.Value = Util.Comprobar_NULL_Per_0_Decimal(Me.T_RiesgoMaximo.Value) - Util.Comprobar_NULL_Per_0_Decimal(Me.T_TotalPendienteCobro.Value)
    End Sub

    Private Sub CarregarDadesPestanyes(ByVal pKeyPestanya As String)
        Try

            Dim _ID As Integer = oLinqCliente.ID_Cliente

            Select Case pKeyPestanya
                Case "Contactos"
                    Call CargaGrid_Contacto(_ID)
                Case "ListadoInstalaciones"
                    Call CargaGrid_Instalaciones(_ID)
                Case "ListadoPropuestas"
                    Call CargaGrid_Propuestas(_ID)
                Case "ProductosPresupuestados"
                    Call CargaGrid_ProductosPresupuestados(_ID)
                Case "ActividadCRM"
                    Call CargaGrid_ActividadCRM(oLinqCliente.ID_Cliente)
                    Call CargaGrid_ProductosInteres(_ID)
                Case "Telemarketing"
                    Call CargaGrid_Marketing(_ID)
                Case "PersonalAceptado"
                    Call CargaGrid_PersonalAceptado(_ID)
                Case "Partes"
                    Call CargarGrid_Partes()
                Case "Step"
                    Call CargaGrid_CuentasBancarias(_ID)
                    Call CalcularRiesgoStep()
                    Util.Tab_Seleccio_x_Key(Me.TAB_Step, "Step")
                Case "Observaciones"
                Case "Ficheros"
                Case "Seguridad"
                    Call CargaGrid_Seguridad(_ID)
                Case "Empresas"
                    Call CargaGrid_Empresas(_ID)
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Events Varis"

    Private Sub TE_Codigo_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Call Cridar_Llistat_Generic()
        End If
    End Sub

    Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown

        If e.KeyCode = Keys.Enter Then
            If Me.TE_Codigo.Value Is Nothing = False AndAlso IsDBNull(Me.TE_Codigo.Value) = False Then
                Dim ooLinqCliente As Cliente = oDTC.Cliente.Where(Function(F) F.Codigo = CInt(Me.TE_Codigo.Value)).FirstOrDefault()
                If ooLinqCliente Is Nothing = False Then
                    Call Cargar_Form(ooLinqCliente.ID_Cliente)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If
            Else
                If oDTC.Cliente.Count > 0 Then
                    Me.TE_Codigo.Value = oDTC.Cliente.Where(Function(F) F.Activo = True).Max(Function(F) F.Codigo) + 1
                Else
                    Me.TE_Codigo.Value = 1
                End If
            End If
        End If
    End Sub

    Private Sub C_Tipo_Documento_ValueChanged(sender As Object, e As EventArgs) Handles C_Tipo_Documento.ValueChanged
        'If Me.C_Tipo_Documento.Value Is Nothing = False Then


        If Me.C_Tipo_Documento.Items.Count = 0 OrElse Me.C_Tipo_Documento.Value Is Nothing Then
            Exit Sub
        End If

        Dim DTS As New DataSet
        BD.CargarDataSet(DTS, "Select * From C_Entrada Where ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Cliente=" & oLinqCliente.ID_Cliente & " Order by FechaEntrada")
        BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Entrada in (Select ID_Entrada From Entrada Where ID_Cliente= " & oLinqCliente.ID_Cliente & " Group By ID_Entrada) Order by FechaEntrada", "aa", 0, "ID_Entrada", "ID_Entrada", True)
        Me.GRD_Step.GRID.DisplayLayout.MaxBandDepth = 4 'tinc que fer aquesta merda pq si no no em surten els fills

        Me.GRD_Step.M.clsUltraGrid.Cargar(DTS)
        ' End If
    End Sub

    Private Sub GRD_Step_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Step.M_GRID_DoubleClickRow2
        If e.Band.Index = 0 Then
            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
            frm.Entrada(e.Cells("ID_Entrada").Value, e.Cells("ID_Entrada_Tipo").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

    Private Sub TAB_Principal_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
        Call CarregarDadesPestanyes(e.Tab.Key)

    End Sub

    Private Sub T_RiesgoMaximo_ValueChanged(sender As Object, e As EventArgs) Handles T_RiesgoMaximo.ValueChanged
        Call CalcularRiesgoStep()
    End Sub

    Private Sub TAB_Step_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Step.SelectedTabChanged
        Select Case e.Tab.Key
            Case "Situacion"
                Call CargarGrid_AlbarandoHoras()
                Call CargarGrid_NOAlbarandoHoras()
                Call CargarGrid_AlbarandoGastos()
                Call CargarGrid_NOAlbarandoGastos()
                Call CargarGrid_AlbarandoMaterial()
                Call CargarGrid_NOAlbarandoMaterial()
                Call CargarGrid_AlbarandoTalYComoSeInstalo()
                Call CargarGrid_NOAlbarandoTalYComoSeInstalo()
        End Select
    End Sub

    Private Sub C_Comercial_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Comercial.BeforeDropDown
        Dim _SQL As String = ""
        If oLinqCliente.ID_Personal.HasValue Then
            _SQL = " or ID_Personal= " & oLinqCliente.ID_Personal
        End If
        Util.Cargar_Combo(Me.C_Comercial, "Select ID_Personal, Nombre From Personal Where Activo=1 and FechaBajaEmpresa is null" & "  and ID_Personal_Tipo=" & EnumPersonalTipo.Comercial & _SQL & " Order by Nombre", False, False)
        Me.C_Comercial.Value = oLinqCliente.ID_Personal
    End Sub

    Private Sub B_Adelante_Click(sender As Object, e As EventArgs) Handles B_Adelante.Click
        Call AvanzarRetroceder(True)
    End Sub

    Private Sub B_Atras_Click(sender As Object, e As EventArgs) Handles B_Atras.Click
        Call AvanzarRetroceder(False)
    End Sub

    Private Sub AvanzarRetroceder(ByVal pAvanzar As Boolean)
        'Que pasa si no s'ha seleccinat cap article?

        Dim _IDATrobar As Integer
        Dim _Trobat As Boolean = False

        If oLinqCliente.ID_Cliente = 0 Then
            'Si actualment no tenim cap registre carregat farem veure com si estiguessim al últim.
            _IDATrobar = oDTC.Cliente.Where(Function(F) F.ID_Cliente_Tipo = oTipoEntrada And F.Activo = True).Max(Function(F) F.ID_Cliente)
            _Trobat = True

        Else
            _IDATrobar = oLinqCliente.ID_Cliente
        End If

        Dim _LlistatClients As IList(Of Cliente)
        Select Case Me.OP_Filtre.Value
            Case "Codigo"
                If pAvanzar = True Then
                    _LlistatClients = oDTC.Cliente.Where(Function(F) F.ID_Cliente_Tipo = oTipoEntrada And F.Activo = True).OrderBy(Function(F) F.Codigo).ToList
                Else
                    _LlistatClients = oDTC.Cliente.Where(Function(F) F.ID_Cliente_Tipo = oTipoEntrada And F.Activo = True).OrderByDescending(Function(F) F.Codigo).ToList
                End If
        End Select


        Dim _Client As Cliente
        Dim _ClientSeguent As Cliente
        For Each _Client In _LlistatClients
            If _Trobat = True Then
                _ClientSeguent = _Client
                Exit For
            End If
            If _Client.ID_Cliente = _IDATrobar Then
                _Trobat = True
            End If
        Next
        If _ClientSeguent Is Nothing = False Then
            Dim _TabActual As String = Me.TAB_Principal.SelectedTab.Key
            Call Cargar_Form(_ClientSeguent.ID_Cliente, True)
            Call CarregarDadesPestanyes(_TabActual)
        End If
    End Sub

#End Region

#Region "Grid Instalaciones"

    Private Sub CargaGrid_Instalaciones(ByVal pID As Integer)
        Try
            Me.GRD_Instalaciones.M.clsUltraGrid.Cargar("Select * From C_Instalacion Where Activo=1 and ID_Cliente=" & pID & " and ((Select Count(*) From Instalacion_Seguridad Where Instalacion_Seguridad.ID_Instalacion=C_Instalacion.ID_Instalacion)=0 or ID_Instalacion in (Select ID_Instalacion From Instalacion_Seguridad Where Instalacion_Seguridad.ID_usuario=" & Seguretat.oUser.ID_Usuario & "))", BD)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Instalaciones_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Instalaciones.M_ToolGrid_ToolVisualitzarDobleClickRow
        If Me.GRD_Instalaciones.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If
        If Me.GRD_Instalaciones.GRID.Selected.Rows(0).Band.Index = 0 Then
            Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
            frm.Entrada(Me.GRD_Instalaciones.GRID.Selected.Rows(0).Cells("ID_Instalacion").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

#End Region

#Region "Partes"

    Private Sub CargarGrid_Partes()
        Try

            Me.GRD_Partes.M.clsUltraGrid.Cargar("Select * From C_Parte Where Activo=1 and ID_Cliente=" & oLinqCliente.ID_Cliente & " Order by FechaAlta", BD)


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Partes_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Partes.M_ToolGrid_ToolVisualitzarDobleClickRow
        If Me.GRD_Partes.GRID.Selected.Rows.Count = 1 Then
            Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
            frm.Entrada(Me.GRD_Partes.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

#End Region

#Region "Grid Contacto"

    Private Sub CargaGrid_Contacto(ByVal pId As Integer)
        Try
            With Me.GRD_Contactos
                Dim Contactes As IEnumerable(Of Cliente_Contacto) = From taula In oDTC.Cliente_Contacto Where taula.ID_Cliente = pId Select taula

                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(Contactes)

                .M_Editable()
                .M_TreureFocus()

                Call CargarComboIdioma()
                Call CargarComboIdiomaEscrito()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contacto_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Contactos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Contactos

                If oLinqCliente.ID_Cliente = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cliente").Value = oLinqCliente
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Idioma").Value = oDTC.Idioma.Where(Function(F) F.ID_Idioma = CInt(EnumIdioma.Castella)).FirstOrDefault
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Idioma1").Value = oDTC.Idioma.Where(Function(F) F.ID_Idioma = CInt(EnumIdioma.Castella)).FirstOrDefault
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contacto_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Contactos.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCliente.Cliente_Contacto.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contacto_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Contactos.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarComboIdioma()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oIdioma As IQueryable(Of Idioma) = (From Taula In oDTC.Idioma Order By Taula.Descripcion Select Taula)
            Dim Var As Idioma

            For Each Var In oIdioma
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Contactos.GRID.DisplayLayout.Bands(0).Columns("Idioma").Style = ColumnStyle.DropDownList
            GRD_Contactos.GRID.DisplayLayout.Bands(0).Columns("Idioma").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarComboIdiomaEscrito()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oIdioma As IQueryable(Of Idioma) = (From Taula In oDTC.Idioma Order By Taula.Descripcion Select Taula)
            Dim Var As Idioma

            For Each Var In oIdioma
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Contactos.GRID.DisplayLayout.Bands(0).Columns("Idioma1").Style = ColumnStyle.DropDownList
            GRD_Contactos.GRID.DisplayLayout.Bands(0).Columns("Idioma1").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub
#End Region

#Region "Grid Empresas"

    Private Sub CargaGrid_Empresas(ByVal pId As Integer)
        Try
            With Me.GRD_Empresas
                Dim Contactes As IEnumerable(Of Cliente_Empresa) = From taula In oDTC.Cliente_Empresa Where taula.ID_Cliente = pId Select taula

                .M.clsUltraGrid.CargarIEnumerable(Contactes)

                .M_Editable()
                .M_TreureFocus()

                Call CargarComboEmpresa()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Empresas_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Empresas.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Empresas

                If oLinqCliente.ID_Cliente = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cliente").Value = oLinqCliente
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Idioma").Value = oDTC.Idioma.Where(Function(F) F.ID_Idioma = CInt(EnumIdioma.Castella)).FirstOrDefault

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Empresas_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Empresas.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCliente.Cliente_Empresa.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Empresas_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Empresas.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarComboEmpresa()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oEmpresa As IQueryable(Of Empresa) = (From Taula In oDTC.Empresa Order By Taula.NombreComercial Select Taula)
            Dim Var As Empresa

            For Each Var In oEmpresa
                Valors.ValueListItems.Add(Var, Var.NombreComercial)
            Next

            GRD_Empresas.GRID.DisplayLayout.Bands(0).Columns("Empresa").Style = ColumnStyle.DropDownList
            GRD_Empresas.GRID.DisplayLayout.Bands(0).Columns("Empresa").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub


    Private Sub GRD_Empresas_M_GRID_CellChange(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Empresas.M_GRID_CellChange
        Try
            If e.Cell.Column.Key = "Predeterminada" Then
                If e.Cell.Value = False Then
                    Dim pRow As UltraGridRow
                    For Each pRow In Me.GRD_Empresas.GRID.Rows
                        If e.Cell.Row Is pRow = False Then
                            pRow.Cells(e.Cell.Column.Key).Value = False
                            pRow.Update()
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Grid Personal aceptado"

    Private Sub CargaGrid_PersonalAceptado(ByVal pId As Integer)
        Try
            With Me.GRD_PersonalAceptado
                Dim _Asignacion As IEnumerable(Of Cliente_PersonalAceptado) = From taula In oDTC.Cliente_PersonalAceptado Where taula.ID_Cliente = pId Select taula

                '.GRID.DataSource = _Asignacion
                .M.clsUltraGrid.CargarIEnumerable(_Asignacion)

                .M_Editable()
                .M_TreureFocus()

                Call CargarCombo_Personal(.GRID)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Personal(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal) = (From Taula In oDTC.Personal Where Taula.Activo = True Order By Taula.Nombre Select Taula)
            Dim Var As Personal

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PersonalAceptado_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_PersonalAceptado.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_PersonalAceptado
                If oLinqCliente.ID_Cliente = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cliente").Value = oLinqCliente

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PersonalAceptado_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_PersonalAceptado.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCliente.Cliente_PersonalAceptado.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PersonalAceptado_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_PersonalAceptado.M_GRID_CellListSelect
        Try
            If e.Cell.Column.Key <> "Personal" Then 'si no modifiquem el combo de personal no hi hem de fer re aqui
                Exit Sub
            End If

            'Comprovem que no s'hagi introduit aquest treballador abans
            Dim _Personal As Personal
            _Personal = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
            If oLinqCliente.Cliente_PersonalAceptado.Where(Function(F) F.Personal Is _Personal).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar la misma persona dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                e.Cell.CancelUpdate()
                Exit Sub
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PersonalAceptado_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_PersonalAceptado.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Propuestas"

    Private Sub CargaGrid_Propuestas(ByVal pID As Integer)
        Try

            Dim DTS As New DataSet
            BD.CargarDataSet(DTS, "Select * From C_Propuesta Where Activo=1 and SeInstalo=0 and ID_Cliente=" & oLinqCliente.ID_Cliente)
            'BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where Activo=1 and ID_Cliente=" & oLinqCliente.ID_Cliente,)



            Me.GRD_Propuestas.M.clsUltraGrid.Cargar(DTS)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Propuestas_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Propuestas.M_ToolGrid_ToolVisualitzarDobleClickRow

        If Me.GRD_Propuestas.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        If Me.GRD_Propuestas.GRID.Selected.Rows(0).Band.Index = 0 Then
            Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = CInt(Me.GRD_Propuestas.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value)).FirstOrDefault
            If _Propuesta Is Nothing = False Then
                Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                frm.Entrada(_Propuesta.ID_Instalacion)
                frm.FormObrir(frmPrincipal, True)

                Dim frm2 As New frmPropuesta
                frm2.Entrada(frm.oLinqInstalacion, frm.oDTC, _Propuesta.ID_Propuesta)
                AddHandler frm2.FormClosing, AddressOf frm.AlTancarfrmPropuesta
                frm2.FormObrir(frm)
            End If
        End If
    End Sub

#End Region

#Region "Grid productos presupuestados"

    Private Sub CargaGrid_ProductosPresupuestados(ByVal pID As Integer)
        Try

            Dim DTS As New DataSet
            BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where Activo=1 and SeInstalo=0 and ID_Cliente=" & oLinqCliente.ID_Cliente)
            'BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where Activo=1 and ID_Cliente=" & oLinqCliente.ID_Cliente,)

            Me.GRD_ProductosPresupuestados.M.clsUltraGrid.Cargar(DTS)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductosPresupuestados_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_ProductosPresupuestados.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_ProductosPresupuestados
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = CInt(.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value)).FirstOrDefault
                If _Propuesta Is Nothing = False Then
                    Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                    frm.Entrada(_Propuesta.ID_Instalacion)
                    frm.FormObrir(frmPrincipal, True)

                    Dim frm2 As New frmPropuesta
                    frm2.Entrada(frm.oLinqInstalacion, frm.oDTC, _Propuesta.ID_Propuesta)
                    AddHandler frm2.FormClosing, AddressOf frm.AlTancarfrmPropuesta
                    frm2.FormObrir(frm)
                End If
            End If
        End With
    End Sub

#End Region

#Region "Grid Seguridad"

    Private Sub CargaGrid_Seguridad(ByVal pId As Integer)
        Try
            Dim _Seguretat As IEnumerable(Of Cliente_Seguridad) = From taula In oDTC.Cliente_Seguridad Where taula.ID_Cliente = pId Select taula

            With Me.GRD_Seguridad


                '.GRID.DataSource = _Seguretat
                .M.clsUltraGrid.CargarIEnumerable(_Seguretat)

                .M_Editable()

                ' Call CargarCombo_Usuario(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguridad_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Seguridad.M_GRID_CellListSelect
        Try
            If e.Cell.Column.Key <> "Usuario" Then 'si no modifiquem el combo de personal no hi hem de fer re aqui
                Exit Sub
            End If

            Dim _Usuario As Usuario

            'Comprovem que no s'hagi introduit aquest usuari abans
            _Usuario = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
            If oLinqCliente.Cliente_Seguridad.Where(Function(F) F.Usuario Is _Usuario).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar el mismo usuario dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                e.Cell.CancelUpdate()
                Exit Sub
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Usuario(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef pCell As UltraWinGrid.UltraGridCell, ByVal pSoloUsuarioSeleccionado As Boolean)
        Try
            Dim _Usuario As Usuario = pCell.Value
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Usuario)
            If pSoloUsuarioSeleccionado = False Then
                oTaula = (From Taula In oDTC.Usuario Where (Taula.Activo = True And Taula.Personal.FechaBajaEmpresa.HasValue = False) Or (Taula Is _Usuario) Order By Taula.Nombre Select Taula)
            Else
                oTaula = (From Taula In oDTC.Usuario Where Taula Is _Usuario Order By Taula.Nombre Select Taula)
            End If

            Dim Var As Usuario

            For Each Var In oTaula
                If Var.ID_Personal.HasValue = True Then
                    Valors.ValueListItems.Add(Var, Var.Personal.Nombre)
                End If
            Next

         pCell.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguridad_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Seguridad.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "Usuario" Then
            Call CargarCombo_Usuario(sender, e.Cell, False)
        End If
    End Sub

    Private Sub GRD_Seguridad_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Seguridad.M_Grid_InitializeRow
        If e.ReInitialize = False Then
            Call CargarCombo_Usuario(Sender, e.Row.Cells("Usuario"), True)
        End If
    End Sub

    Private Sub GRD_Seguridad_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Seguridad.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Seguridad

                'If Guardar(False) = False Then
                '    Exit Sub
                'End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cliente").Value = oLinqCliente

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguridad_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Seguridad.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCliente.Cliente_Seguridad.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguridad_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Seguridad.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Marketing"
    Private Sub CargaGrid_Marketing(ByVal pID As Integer)
        Try
            Me.GRD_Telemarketing.M.clsUltraGrid.Cargar("Select * From C_Campaña_Cliente_Seguimiento Where Activo=1 and ID_Cliente=" & pID & " Order by ID_Campaña_Cliente Desc", BD)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid ActividadCRM"
    Private Sub CargaGrid_ActividadCRM(ByVal pID As Integer)
        Try
            Me.GRD_ActividadCRM.M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_ActividadCRM Where Activo=1 and ID_Cliente=" & pID & " Order by ID_ActividadCRM Desc", True))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ActividadCRM_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_ActividadCRM.M_GRID_DoubleClickRow2
        Try

            Dim frm As New frmActividadCRM_Mantenimiento
            frm.Entrada(e.Cells("ID_ActividadCRM").Value)
            frm.FormObrir(Me, True)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Direcciones"

    Private Sub CargaGrid_Direcciones(ByVal pId As Integer)
        Try
            With Me.GRD_Direcciones
                Dim _Direcciones As IEnumerable(Of Cliente_Direccion) = From taula In oDTC.Cliente_Direccion Where taula.ID_Cliente = pId Select taula

                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(_Direcciones)

                .M_Editable()
                .M_TreureFocus()

                Call CargarComboDireccionTipo()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Direcciones_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Direcciones.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Direcciones

                If oLinqCliente.ID_Cliente = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cliente").Value = oLinqCliente
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Idioma").Value = oDTC.Idioma.Where(Function(F) F.ID_Idioma = CInt(EnumIdioma.Castella)).FirstOrDefault
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Direcciones_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Direcciones.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCliente.Cliente_Direccion.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Direcciones_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Direcciones.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarComboDireccionTipo()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oDirecciones As IQueryable(Of Cliente_DireccionTipo) = (From Taula In oDTC.Cliente_DireccionTipo Order By Taula.Descripcion Select Taula)
            Dim Var As Cliente_DireccionTipo

            For Each Var In oDirecciones
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Direcciones.GRID.DisplayLayout.Bands(0).Columns("Cliente_DireccionTipo").Style = ColumnStyle.DropDownList
            GRD_Direcciones.GRID.DisplayLayout.Bands(0).Columns("Cliente_DireccionTipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

#End Region

#Region "Grid productos interés"

    Private Sub CargaGrid_ProductosInteres(ByVal pId As Integer)
        Try
            With Me.GRD_ProductosInteres
                Dim _Direcciones As IEnumerable(Of Cliente_ProductosInteres) = From taula In oDTC.Cliente_ProductosInteres Where taula.ID_Cliente = pId Select taula

                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(_Direcciones)

                .M_Editable()
                .M_TreureFocus()

                Call CargarComboDivision()
                ' Call CargarComboFamilia()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductosInteres_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ProductosInteres.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_ProductosInteres

                If oLinqCliente.ID_Cliente = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cliente").Value = oLinqCliente
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaAlta").Value = Now.Date
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Idioma").Value = oDTC.Idioma.Where(Function(F) F.ID_Idioma = CInt(EnumIdioma.Castella)).FirstOrDefault
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductosInteres_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_ProductosInteres.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCliente.Cliente_ProductosInteres.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductosInteres_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_ProductosInteres.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarComboDivision()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oProducto_Division As IQueryable(Of Producto_Division) = (From Taula In oDTC.Producto_Division Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Division

            For Each Var In oProducto_Division
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_ProductosInteres.GRID.DisplayLayout.Bands(0).Columns("Producto_Division").Style = ColumnStyle.DropDownList
            GRD_ProductosInteres.GRID.DisplayLayout.Bands(0).Columns("Producto_Division").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarCombo_Familia(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pCell As UltraGridCell, ByVal pIDDivision As Integer)
        Try
            If pCell Is Nothing OrElse pIDDivision = 0 Then
                Exit Sub
            End If

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Familia) = (From Taula In oDTC.Producto_Familia Where Taula.ID_Producto_Division = pIDDivision And Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Familia

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pCell.Style = ColumnStyle.DropDownList
            pCell.ValueList = Valors.Clone

            'pGrid.DisplayLayout.Bands(0).Columns("Producto_Familia").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_ProductosInteres_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_ProductosInteres.M_Grid_InitializeRow
        Call CargarCombo_Familia(Me.GRD_ProductosInteres.GRID, e.Row.Cells("Producto_Familia"), e.Row.Cells("ID_Producto_Division").Value)
    End Sub

    'Private Sub CargarComboFamilia()
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oProducto_Familia As IQueryable(Of Producto_Familia) = (From Taula In oDTC.Producto_Familia Order By Taula.Descripcion Select Taula)
    '        Dim Var As Producto_Familia

    '        For Each Var In oProducto_Familia
    '            Valors.ValueListItems.Add(Var, Var.Descripcion)
    '        Next

    '        GRD_ProductosInteres.GRID.DisplayLayout.Bands(0).Columns("Producto_Familia").Style = ColumnStyle.DropDownList
    '        GRD_ProductosInteres.GRID.DisplayLayout.Bands(0).Columns("Producto_Familia").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

#End Region

#Region "Grid Situacion - Albaranado - Horas"
    Private Sub CargarGrid_AlbarandoHoras()
        Try

            Dim DT As New DataTable
            DT = BD.RetornaDataTable("Select * From C_Cliente_Lineas_Relacionadas_Con_Partes Where Albaranado=1 and ID_Cliente=" & oLinqCliente.ID_Cliente)
            Me.GRD_Albaranado_Horas.M.clsUltraGrid.Cargar(DT)
            Call CargarRegistrosEnTab(Me.GRD_Albaranado_Horas.GRID.Rows.Count, Me.Tab_Step_Situacion_Albaranado, "Horas")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Albaranado_Horas_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Albaranado_Horas.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_Albaranado_Horas
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Entrada").Value), EnumEntradaTipo.AlbaranVenta)
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

#Region "Grid Situacion - NOAlbaranado - Horas"
    Private Sub CargarGrid_NOAlbarandoHoras()
        Try

            Dim DT As New DataTable
            DT = BD.RetornaDataTable("Select * From C_Cliente_Lineas_Relacionadas_Con_Partes Where Albaranado=0 and ID_Cliente=" & oLinqCliente.ID_Cliente)
            Me.GRD_NoAlbaranado_Horas.M.clsUltraGrid.Cargar(DT)
            Call CargarRegistrosEnTab(Me.GRD_NoAlbaranado_Horas.GRID.Rows.Count, Me.Tab_Step_Situacion_NoAlbaranado, "Horas")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_NOAlbaranado_Horas_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_NoAlbaranado_Horas.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_NoAlbaranado_Horas
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Parte").Value))
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

#Region "Grid Situacion - Albaranado - Gastos"
    Private Sub CargarGrid_AlbarandoGastos()
        Try

            Dim DT As New DataTable
            DT = BD.RetornaDataTable("Select * From C_Cliente_Lineas_Relacionadas_Con_Gastos Where Albaranado=1 and ID_Cliente=" & oLinqCliente.ID_Cliente)
            Me.GRD_Albaranado_Gastos.M.clsUltraGrid.Cargar(DT)
            Call CargarRegistrosEnTab(Me.GRD_Albaranado_Gastos.GRID.Rows.Count, Me.Tab_Step_Situacion_Albaranado, "Gastos")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Albaranado_Gastos_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Albaranado_Gastos.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_Albaranado_Gastos
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Entrada").Value), EnumEntradaTipo.AlbaranVenta)
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

#Region "Grid Situacion - NOAlbaranado - Gastos"
    Private Sub CargarGrid_NOAlbarandoGastos()
        Try

            Dim DT As New DataTable
            DT = BD.RetornaDataTable("Select * From C_Cliente_Lineas_Relacionadas_Con_Gastos Where Albaranado=0 and ID_Cliente=" & oLinqCliente.ID_Cliente)
            Me.GRD_NoAlbaranado_Gastos.M.clsUltraGrid.Cargar(DT)
            Call CargarRegistrosEnTab(Me.GRD_NoAlbaranado_Gastos.GRID.Rows.Count, Me.Tab_Step_Situacion_NoAlbaranado, "Gastos")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_NOAlbaranado_Gastos_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_NoAlbaranado_Gastos.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_NoAlbaranado_Gastos
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Parte").Value))
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

#Region "Grid Situacion - Albaranado - Material"
    Private Sub CargarGrid_AlbarandoMaterial()
        Try

            Dim DT As New DataTable
            DT = BD.RetornaDataTable("Select * From C_Cliente_Lineas_Relacionadas_Con_Material Where Albaranado=1 and ID_Cliente=" & oLinqCliente.ID_Cliente)
            Me.GRD_Albaranado_Material.M.clsUltraGrid.Cargar(DT)
            Call CargarRegistrosEnTab(Me.GRD_Albaranado_Material.GRID.Rows.Count, Me.Tab_Step_Situacion_Albaranado, "Material")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Albaranado_Material_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Albaranado_Material.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_Albaranado_Material
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Entrada").Value), EnumEntradaTipo.AlbaranVenta)
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

#Region "Grid Situacion - NOAlbaranado - Material"
    Private Sub CargarGrid_NOAlbarandoMaterial()
        Try

            Dim DT As New DataTable
            DT = BD.RetornaDataTable("Select * From C_Cliente_Lineas_Relacionadas_Con_Material Where Albaranado=0 and ID_Cliente=" & oLinqCliente.ID_Cliente)
            Me.GRD_NoAlbaranado_Material.M.clsUltraGrid.Cargar(DT)
            Call CargarRegistrosEnTab(Me.GRD_NoAlbaranado_Material.GRID.Rows.Count, Me.Tab_Step_Situacion_NoAlbaranado, "Material")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_NOAlbaranado_Material_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_NoAlbaranado_Material.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_NoAlbaranado_Material
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Parte").Value))
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

#Region "Grid Situacion - Albaranado - Tal y Como se instalo"
    Private Sub CargarGrid_AlbarandoTalYComoSeInstalo()
        Try

            Dim DT As New DataTable
            DT = BD.RetornaDataTable("Select * From C_Cliente_Lineas_Relacionadas_Con_TalYComoSeInstalo Where ID_Entrada_Tipo=" & EnumEntradaTipo.AlbaranVenta & " and ID_Cliente=" & oLinqCliente.ID_Cliente)
            Me.GRD_Albaranado_TalYComoSeInstalo.M.clsUltraGrid.Cargar(DT)
            Call CargarRegistrosEnTab(Me.GRD_Albaranado_TalYComoSeInstalo.GRID.Rows.Count, Me.Tab_Step_Situacion_Albaranado, "TalYComoSeInstalo")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Albaranado_TalYComoSeInstalo_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Albaranado_TalYComoSeInstalo.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_Albaranado_TalYComoSeInstalo
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Entrada").Value), EnumEntradaTipo.AlbaranVenta)
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

#Region "Grid Situacion - NOAlbaranado - Tal y Como se instalo"
    Private Sub CargarGrid_NOAlbarandoTalYComoSeInstalo()
        Try

            Dim DT As New DataTable
            DT = BD.RetornaDataTable("Select * From C_Cliente_Lineas_Relacionadas_Con_TalYComoSeInstalo Where isnull(ID_Entrada_Tipo,0)<>" & EnumEntradaTipo.AlbaranVenta & " and isnull(CantidadTraspasada,0)=0 and ID_Cliente=" & oLinqCliente.ID_Cliente)
            Me.GRD_NoAlbaranado_TalYComoSeInstalo.M.clsUltraGrid.Cargar(DT)
            Call CargarRegistrosEnTab(Me.GRD_NoAlbaranado_TalYComoSeInstalo.GRID.Rows.Count, Me.Tab_Step_Situacion_NoAlbaranado, "TalYComoSeInstalo")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_NOAlbaranado_TalYComoSeInstalo_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_NoAlbaranado_TalYComoSeInstalo.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_NoAlbaranado_TalYComoSeInstalo
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Instalacion").Value))
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

#Region "Grid Cuentas bancarias"

    Private Sub CargaGrid_CuentasBancarias(ByVal pId As Integer)
        Try
            With Me.GRD_CuentasBancarias
                Dim _Cuentas As IEnumerable(Of Cliente_CuentaBancaria) = From taula In oDTC.Cliente_CuentaBancaria Where taula.ID_Cliente = pId Select taula

                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(_Cuentas)

                .M_Editable()
                .M_TreureFocus()

                .GRID.DisplayLayout.Bands(0).Columns("Predeterminada").CellActivation = Activation.Disabled


                Dim _pRow As UltraGridRow
                For Each _pRow In .GRID.Rows
                    Dim _CuentaBancaria As Cliente_CuentaBancaria = _pRow.ListObject
                    If _CuentaBancaria.Entrada_Vencimiento.Count > 0 Then
                        _pRow.Cells("NombreBanco").Activation = Activation.Disabled
                        _pRow.Cells("NumeroCuenta").Activation = Activation.Disabled
                    End If
                Next


            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CuentasBancarias_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_CuentasBancarias.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_CuentasBancarias

                If oLinqCliente.ID_Cliente = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cliente").Value = oLinqCliente
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CuentasBancarias_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_CuentasBancarias.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCliente.Cliente_CuentaBancaria.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _CuentaBancaria As Cliente_CuentaBancaria = e.ListObject
            If _CuentaBancaria.Entrada_Vencimiento.Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar, esta cuenta bancaria ya ha sido usada en uno o más vencimientos de facturas", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CuentasBancarias_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_CuentasBancarias.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_CuentasBancarias_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_CuentasBancarias.M_ToolGrid_ToolClickBotonsExtras2
        With Me.GRD_CuentasBancarias
            If e.Tool.Key = "Predeterminar" AndAlso .GRID.Selected.Rows.Count = 1 Then
                If .GRID.ActiveRow.Cells("Predeterminada").Value = False Then 'si no es predeterminat vigilarem que no ni hagi unaltre
                    Dim _pRow As UltraGridRow
                    For Each _pRow In .GRID.Rows
                        If Not _pRow Is .GRID.ActiveRow Then
                            If _pRow.Cells("Predeterminada").Value = True Then
                                _pRow.Cells("Predeterminada").Value = False
                                _pRow.Update()
                            End If
                        End If
                    Next
                End If


                .GRID.ActiveRow.Cells("Predeterminada").Value = Not .GRID.ActiveRow.Cells("Predeterminada").Value
                .GRID.ActiveRow.Update()
            End If
        End With
    End Sub
#End Region


    Private Sub CargarRegistrosEnTab(ByVal pNumRegistres As Integer, ByRef pTab As UltraWinTabControl.UltraTabControl, ByRef pNomPestanya As String)
        Dim _TextActual As String = pTab.Tabs(pNomPestanya).Text
        If _TextActual.Contains("  ") Then 'Si ja li hem assignat uns números els treurem per després, si s'escau, tornar a afegir-los actualtizats
            _TextActual = Mid(_TextActual, 1, InStr(_TextActual, "  ") - 1)
        End If

        If pNumRegistres > 0 Then
            _TextActual = _TextActual & "   (" & pNumRegistres & ")"
        End If

        pTab.Tabs(pNomPestanya).Text = _TextActual
    End Sub

    Private Function e() As Object
        Throw New NotImplementedException
    End Function



 

  

End Class