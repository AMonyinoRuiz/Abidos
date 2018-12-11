Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmProveedor
    Dim oDTC As DTCDataContext
    Dim oLinqProveedor As Proveedor
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqProveedor.ID_Proveedor <> 0 Then
                If Mensaje.Mostrar_Mensaje("Desea dar de alta/baja el registro?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    If DT_Baja.Value Is Nothing Then
                        Me.DT_Baja.Value = Date.Now
                        oLinqProveedor.Activo = False
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                    Else
                        Me.DT_Baja.Value = Nothing
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                    End If
                    Call Guardar()
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
        Dim Contador As Integer
        If oDTC.Proveedor.Count = 0 Then
            Contador = 0
        Else
            Contador = oDTC.Proveedor.Max(Function(F) F.Codigo) + 1
        End If

        Me.TE_Codigo.Value = Contador
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Buscar() Handles ToolForm.m_ToolForm_Buscar
        Call Cridar_Llistat_Generic()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pId As Integer = 0)

        Try

            Me.AplicarDisseny()

            Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Proveedor_Archivo", 1)

            Util.Cargar_Combo(Me.C_Tipo_Documento, "SELECT ID_Entrada_Tipo, Descripcion FROM Entrada_Tipo Where Tipo='C' ORDER BY ID_Entrada_Tipo", False)
            Util.Cargar_Combo(Me.C_FormaPago, "SELECT ID_FormaPago, Descripcion FROM FormaPago Where Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_Pais, "SELECT ID_Pais, Nombre FROM Pais ORDER BY Nombre", True)

            Me.GRD_CuentasBancarias.M.clsToolBar.Boto_Afegir("Predeterminar", "Predeterminar")

            'Carregarem 31 dias de la setmana al combo
            Dim i As Integer
            For i = 1 To 31
                Me.C_DiaPago.Items.Add(i, i)
            Next

            Call Netejar_Pantalla()

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
            If oLinqProveedor.ID_Proveedor = 0 Then
                If BD.RetornaValorSQL("SELECT Count (*) From Proveedor WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            Else
                If BD.RetornaValorSQL("SELECT Count (*) From Proveedor WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_Proveedor<>" & oLinqProveedor.ID_Proveedor) > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqProveedor)

            If oLinqProveedor.ID_Proveedor = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oLinqProveedor.Activo = True 'Fem això aqui pq no ho fem al GetFromForm ja que si no no es podria borrar mai (ja que el eliminar després fa un guardar)
                oDTC.Proveedor.InsertOnSubmit(oLinqProveedor)
                oDTC.SubmitChanges()
                Call InsertarTarifasAutomaticamente(oLinqProveedor.ID_Proveedor)
                Call CargaGrid_Tarifas(oLinqProveedor.ID_Proveedor)
                Call Fichero.Cargar_GRID(oLinqProveedor.ID_Proveedor) 'Fem això pq la classe tingui el ID de pressupost
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeGuardar)

                'Automàticament al insertar un registre afegirem la empresa predeterminada
                Dim _ProveedorEmpresa As New Proveedor_Empresa
                _ProveedorEmpresa.ID_Empresa = oEmpresa.ID_Empresa
                _ProveedorEmpresa.Predeterminada = True
                oLinqProveedor.Proveedor_Empresa.Add(_ProveedorEmpresa)
                oDTC.SubmitChanges()
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
            With oLinqProveedor
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
                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Baja.Value = .FechaBaja
                If DT_Baja.Value Is Nothing Then
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                Else
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                End If

                Me.T_Longitud.Value = .Longitud
                Me.T_Latitud.Value = .Latitud

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

                Me.C_Pais.Value = .ID_Pais

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pProveedor As Proveedor)
        Try
            With pProveedor
                '.Activo = True
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
                .FechaAlta = Me.DT_Alta.Value
                .FechaBaja = Me.DT_Baja.Value
                .Observaciones = Me.R_Observaciones.pText
                .URLAcceso = Me.T_URLAcceso.Text
                .Usuario = Me.T_Usuario.Text
                .Contraseña = Me.T_Contraseña.Text

                .Longitud = Util.DbnullToNothing(Me.T_Longitud.Value)
                .Latitud = Util.DbnullToNothing(Me.T_Latitud.Value)

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

                .CP = Me.T_CP.Value
                .Pais = oDTC.Pais.Where(Function(F) F.ID_Pais = CInt(Me.C_Pais.Value)).FirstOrDefault

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer, Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        Try
            Call Netejar_Pantalla(pNoCanviarALaPestanyaGeneral)
            oLinqProveedor = (From taula In oDTC.Proveedor Where taula.ID_Proveedor = pID Select taula).First

            If ComprovarPermisosSeguretat() = False Then
                Exit Sub
            End If

            Call SetToForm()
            Call InsertarTarifasAutomaticamente(pID)

            Fichero.Cargar_GRID(pID)

            Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeCargar)

            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Me.EstableixCaptionForm("Proveedor: " & (oLinqProveedor.Nombre))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla(Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        oLinqProveedor = New Proveedor
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing

        Me.DT_Alta.Value = Now.Date
        Me.DT_Baja.Value = Nothing

        'Call CargaGrid_Tarifas(0)
        'Call CargarGrid_ProductosPendientesReparar()
        'Call CargarGrid_ComprasInstalacion()
        'Call CargarGrid_ComprasParte()
        'Call CargaGrid_Contacto(0)
        'Call CargaGrid_Seguridad(0)
        'Call CargaGrid_CuentasBancarias(0)
        Fichero.Cargar_GRID(0)

        Me.C_DiaPago.Value = Nothing
        Me.C_FormaPago.Value = Nothing

        Me.C_Tipo_Documento.Value = Nothing
        Me.GRD_Step.GRID.DataSource = Nothing
        Me.C_Pais.Value = oDTC.Pais.Where(Function(F) F.Predeterminat = True).FirstOrDefault.ID_Pais

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
            Dim oClsControls As New M_GenericForm.clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.T_Nombre)
                ' oClsControls.ControlBuit(.T_Persona_Contacto)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Proveedor ORDER BY Nombre", Me.TE_Codigo, "ID_Proveedor", "ID_Proveedor")
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

    Private Sub InsertarTarifasAutomaticamente(ByVal pId As Integer)
        BD.EjecutarConsulta("Insert Into Proveedor_Tarifa (ID_Proveedor, ID_Producto_Division, Descuento)  Select " & pId & ", ID_Producto_Division, null From Producto_Division as TBL1 Where Activo=1 and (Select COUNT(*) From Proveedor_Tarifa as TBL2 Where ID_Proveedor= " & pId & " and TBL1.ID_Producto_Division=TBL2.ID_Producto_Division)=0")
        'BD.EjecutarConsulta("Insert Into Instalacion_Instalacion_Estudio_Riesgo_Valor (ID_Instalacion, ID_Instalacion_Estudio_Riesgo_Valor, ID_Valoracion, Explicacion)  Select " & pId & ", ID_Instalacion_Estudio_Riesgo_Valor, null, null From Instalacion_Estudio_Riesgo_Valor as TBL1 Where Activo=1 and (Select COUNT(*) From Instalacion_Instalacion_Estudio_Riesgo_Valor as TBL2 Where ID_Instalacion= " & pId & " and TBL1.ID_Instalacion_Estudio_Riesgo_Valor=TBL2.ID_Instalacion_Estudio_Riesgo_Valor)=0")
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
            If oLinqProveedor.Proveedor_Seguridad.Count = 0 Then
                Return True
            End If

            'Si te nivell de seguretat 1 te accés accessible
            If Seguretat.oUser.NivelSeguridad = 1 Then
                Return True
            End If

            'si esta dins de la llista te accés
            If oLinqProveedor.Proveedor_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count = 1 Then
                Return True
            End If

            Mensaje.Mostrar_Mensaje("Imposible cargar los datos, no tiene permisos", M_Mensaje.Missatge_Modo.INFORMACIO)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub CarregarDadesPestanyes(ByVal pKeyPestanya As String)
        Try

            Dim _ID As Integer = oLinqProveedor.ID_Proveedor

            Select Case pKeyPestanya
                Case "Contactos"
                    Call CargaGrid_Contacto(_ID)
                Case "DescuentoDivision"
                    Call CargaGrid_Tarifas(_ID)
                Case "ProductosPendientesReparar"
                    Call CargarGrid_ProductosPendientesReparar()
                Case "Compras"
                    Call CargarGrid_ComprasInstalacion()
                    Call CargarGrid_ComprasParte()
                Case "Productos_Proveedor"
                    Call CargarGrid_ProveedorArticulo()
                Case "Step"
                    Call CargaGrid_CuentasBancarias(_ID)

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

    Private Sub TAB_Principal_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
        Call CarregarDadesPestanyes(e.Tab.Key)
    End Sub

    Private Sub TE_Codigo_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Call Cridar_Llistat_Generic()
        End If
    End Sub

    Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown

        If e.KeyCode = Keys.Enter Then
            If Me.TE_Codigo.Value Is Nothing = False AndAlso IsDBNull(Me.TE_Codigo.Value) = False Then
                Dim ooLinQProveedor As Proveedor = oDTC.Proveedor.Where(Function(F) F.Codigo = CInt(Me.TE_Codigo.Value)).FirstOrDefault()
                If ooLinQProveedor Is Nothing = False Then
                    Call Cargar_Form(ooLinQProveedor.ID_Proveedor)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If
            Else
                If oDTC.Proveedor.Count > 0 Then
                    Me.TE_Codigo.Value = oDTC.Proveedor.Where(Function(F) F.Activo = True).Max(Function(F) F.Codigo) + 1
                Else
                    Me.TE_Codigo.Value = 1
                End If
            End If
        End If
    End Sub

    Private Sub C_Tipo_Documento_ValueChanged(sender As Object, e As EventArgs) Handles C_Tipo_Documento.ValueChanged
        If Me.C_Tipo_Documento.Items.Count = 0 OrElse Me.C_Tipo_Documento.Value Is Nothing Then
            Exit Sub
        End If

        Dim DTS As New DataSet
        BD.CargarDataSet(DTS, "Select * From C_Entrada Where ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Proveedor=" & oLinqProveedor.ID_Proveedor)
        BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Entrada in (Select ID_Entrada From Entrada Where ID_Proveedor=" & oLinqProveedor.ID_Proveedor & " Group By ID_Entrada) Order by FechaEntrada", "aa", 0, "ID_Entrada", "ID_Entrada", True)
        Me.GRD_Step.GRID.DisplayLayout.MaxBandDepth = 4 'tinc que fer aquesta merda pq si no no em surten els fills


        Me.GRD_Step.M.clsUltraGrid.Cargar(DTS)
    End Sub

    Private Sub GRD_Step_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Step.M_GRID_DoubleClickRow2
        If e.Band.Index = 0 Then
            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
            frm.Entrada(e.Cells("ID_Entrada").Value, e.Cells("ID_Entrada_Tipo").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

    Private Sub B_Adelante_Click(sender As Object, e As EventArgs) Handles B_Adelante.Click
        Call AvanzarRetroceder(True)
    End Sub

    Private Sub B_Atras_Click(sender As Object, e As EventArgs) Handles B_Atras.Click
        Call AvanzarRetroceder(True)
    End Sub

    Private Sub AvanzarRetroceder(ByVal pAvanzar As Boolean)
        'Que pasa si no s'ha seleccinat cap article?

        Dim _IDATrobar As Integer
        Dim _Trobat As Boolean = False

        If oLinqProveedor.ID_Proveedor = 0 Then
            'Si actualment no tenim cap registre carregat farem veure com si estiguessim al últim.
            _IDATrobar = oDTC.Proveedor.Where(Function(F) F.Activo = True).Max(Function(F) F.ID_Proveedor)
            _Trobat = True

        Else
            _IDATrobar = oLinqProveedor.ID_Proveedor
        End If

        Dim _LlistatProveedors As IList(Of Proveedor)
        Select Case Me.OP_Filtre.Value
            Case "Codigo"
                If pAvanzar = True Then
                    _LlistatProveedors = oDTC.Proveedor.Where(Function(F) F.Activo = True).OrderBy(Function(F) F.Codigo).ToList
                Else
                    _LlistatProveedors = oDTC.Proveedor.Where(Function(F) F.Activo = True).OrderByDescending(Function(F) F.Codigo).ToList
                End If
        End Select


        Dim _Proveedor As Proveedor
        Dim _ProveedortSeguent As Proveedor
        For Each _Proveedor In _LlistatProveedors
            If _Trobat = True Then
                _ProveedortSeguent = _Proveedor
                Exit For
            End If
            If _Proveedor.ID_Proveedor = _IDATrobar Then
                _Trobat = True
            End If
        Next
        If _ProveedortSeguent Is Nothing = False Then
            Dim _TabActual As String = Me.TAB_Principal.SelectedTab.Key
            Call Cargar_Form(_ProveedortSeguent.ID_Proveedor, True)
            Call CarregarDadesPestanyes(_TabActual)
        End If
    End Sub

#End Region

#Region "Grid Tarifas"

    Private Sub CargaGrid_Tarifas(ByVal pId As Integer)
        Try
            With Me.GRD_Tarifas

                Dim oProveedor_Tarifa As IEnumerable(Of Proveedor_Tarifa) = From taula In oDTC.Proveedor_Tarifa Where taula.ID_Proveedor = pId Select taula
                '.GRID.DataSource = oProveedor_Tarifa
                .M.clsUltraGrid.CargarIEnumerable(oProveedor_Tarifa)

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("Producto_Division").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Division()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Division()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Division) = (From Taula In oDTC.Producto_Division Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Division

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next


            Me.GRD_Tarifas.GRID.DisplayLayout.Bands(0).Columns("Producto_Division").Style = ColumnStyle.DropDownList
            Me.GRD_Tarifas.GRID.DisplayLayout.Bands(0).Columns("Producto_Division").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Entorno_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Tarifas.M_GRID_AfterRowUpdate

        If oLinqProveedor.ID_Proveedor <> 0 AndAlso e.Row.Cells("Descuento").Value <> e.Row.Cells("Descuento").OriginalValue Then
            If Mensaje.Mostrar_Mensaje("¿Desea modificar los descuentos de todos los productos de este proveedor para la división seleccionada?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
                Dim ID_Div As Integer = e.Row.Cells("ID_Producto_Division").Value
                Dim Descompte As Decimal = e.Row.Cells("Descuento").Value

                Dim _LlistaProducteProv = oDTC.Producto_Proveedor.Where(Function(F) F.Producto.ID_Producto_Division = ID_Div And F.ID_Proveedor = oLinqProveedor.ID_Proveedor)

                Dim _ProducteProv As Producto_Proveedor

                For Each _ProducteProv In _LlistaProducteProv
                    _ProducteProv.Descuento = Descompte
                Next
            End If
        End If
        oDTC.SubmitChanges()


    End Sub

#End Region

#Region "Grid ProductosPendientesReparar"

    Private Sub CargarGrid_ProductosPendientesReparar()
        Try
            Me.GRD_ProductosPendientesReparar.M.clsUltraGrid.Cargar("Select * From C_Parte_Reparacion Where Activo=1 and Finalizado=0 and ID_Proveedor=" & oLinqProveedor.ID_Proveedor & "  Order by FechaAlta", BD)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductosPendientesReparar_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_ProductosPendientesReparar.M_ToolGrid_ToolVisualitzarDobleClickRow
        If Me.GRD_ProductosPendientesReparar.GRID.Selected.Rows.Count = 1 Then
            Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
            frm.Entrada(Me.GRD_ProductosPendientesReparar.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

#End Region

#Region "Grid Compras Instalación"
    Private Sub CargarGrid_ComprasInstalacion()
        Try
            Me.GRD_ComprasInstalacion.M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where Activo=1 and ID_Proveedor=" & oLinqProveedor.ID_Proveedor & " and ID_Propuesta_Linea_Estado in (" & EnumPropuestaLineaEstado.Aprobado_PendienteRecibir & " , " & EnumPropuestaLineaEstado.RecepcionadoParcialmente & " , " & EnumPropuestaLineaEstado.PendienteDeAprobar & ") Order by ID_Instalacion", BD)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ComprasInstalacion_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_ComprasInstalacion.M_ToolGrid_ToolVisualitzarDobleClickRow
        If Me.GRD_ComprasInstalacion.GRID.Selected.Rows.Count = 1 Then
            Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
            frm.Entrada(Me.GRD_ComprasInstalacion.GRID.Selected.Rows(0).Cells("ID_Instalacion").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

#End Region

#Region "Grid Compras Parte"
    Private Sub CargarGrid_ComprasParte()
        Try
            Me.GRD_ComprasPartes.M.clsUltraGrid.Cargar("Select * From C_Parte_Material Where ParteActivo=1 And InstalacionActivo=1 and ID_Proveedor=" & oLinqProveedor.ID_Proveedor & " and ID_Propuesta_Linea_Estado in (" & EnumPropuestaLineaEstado.Aprobado_PendienteRecibir & " , " & EnumPropuestaLineaEstado.RecepcionadoParcialmente & " , " & EnumPropuestaLineaEstado.PendienteDeAprobar & ") Order by ID_Parte", BD)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ComprasPartes_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_ComprasPartes.M_ToolGrid_ToolVisualitzarDobleClickRow
        If Me.GRD_ComprasPartes.GRID.Selected.Rows.Count = 1 Then
            Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
            frm.Entrada(Me.GRD_ComprasPartes.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

#End Region

#Region "Grid Contacto"

    Private Sub CargaGrid_Contacto(ByVal pId As Integer)
        Try

            With Me.GRD_Contactos
                Dim _Contactes As IEnumerable(Of Proveedor_Contacto) = From taula In oDTC.Proveedor_Contacto Where taula.ID_Proveedor = pId Select taula

                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(_Contactes)
                .M_Editable()

                'Me.GRD_Contactos.GRID.RowUpdateCancelAction = RowUpdateCancelAction.CancelUpdate
                'Me.GRD_Contactos.GRID.UpdateMode = UpdateMode.OnRowChangeOrLostFocus
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contacto_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Contactos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Contactos

                If oLinqProveedor.ID_Proveedor = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Proveedor").Value = oLinqProveedor

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contactos_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Contactos.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
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

#End Region

#Region "Grid Empresas"

    Private Sub CargaGrid_Empresas(ByVal pId As Integer)
        Try
            With Me.GRD_Empresas
                Dim Contactes As IEnumerable(Of Proveedor_Empresa) = From taula In oDTC.Proveedor_Empresa Where taula.ID_Proveedor = pId Select taula

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

                If oLinqProveedor.ID_Proveedor = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Proveedor").Value = oLinqProveedor
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Idioma").Value = oDTC.Idioma.Where(Function(F) F.ID_Idioma = CInt(EnumIdioma.Castella)).FirstOrDefault

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Empresas_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Empresas.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqProveedor.Proveedor_Empresa.Remove(e.ListObject)
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

#Region "Grid productos en los que trabaja"
    Private Sub CargarGrid_ProveedorArticulo()
        Try
            'Me.GRD_Productos_Proveedor.M.clsUltraGrid.Cargar("Select * From C_Parte_Material Where ParteActivo=1 And InstalacionActivo=1 and ID_Proveedor=" & oLinqProveedor.ID_Proveedor & " and ID_Propuesta_Linea_Estado in (" & EnumPropuestaLineaEstado.Aprobado_PendienteRecibir & " , " & EnumPropuestaLineaEstado.RecepcionadoParcialmente & " , " & EnumPropuestaLineaEstado.PendienteDeAprobar & ") Order by ID_Parte", BD)
            Me.GRD_Productos_Proveedor.M.clsUltraGrid.Cargar("Select * From C_Producto, Producto_Proveedor Where C_Producto.ID_Producto=Producto_Proveedor.ID_Producto and ID_Proveedor=" & oLinqProveedor.ID_Proveedor & " Order by Descripcion", BD)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Productos_Proveedor_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Productos_Proveedor.M_ToolGrid_ToolVisualitzarDobleClickRow
        If Me.GRD_Productos_Proveedor.GRID.Selected.Rows.Count = 1 Then
            Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
            frm.Entrada(Me.GRD_Productos_Proveedor.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

#End Region

#Region "Grid Seguridad"

    Private Sub CargaGrid_Seguridad(ByVal pId As Integer)
        Try
            Dim _Seguretat As IEnumerable(Of Proveedor_Seguridad) = From taula In oDTC.Proveedor_Seguridad Where taula.ID_Proveedor = pId Select taula

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
            If oLinqProveedor.Proveedor_Seguridad.Where(Function(F) F.Usuario Is _Usuario).Count <> 0 Then
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

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Proveedor").Value = oLinqProveedor

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguridad_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Seguridad.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqProveedor.Proveedor_Seguridad.Remove(e.ListObject)
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

#Region "Grid Cuentas bancarias"

    Private Sub CargaGrid_CuentasBancarias(ByVal pId As Integer)
        Try
            With Me.GRD_CuentasBancarias
                Dim _Cuentas As IEnumerable(Of Proveedor_CuentaBancaria) = From taula In oDTC.Proveedor_CuentaBancaria Where taula.ID_Proveedor = pId Select taula

                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(_Cuentas)

                .M_Editable()
                .M_TreureFocus()

                .GRID.DisplayLayout.Bands(0).Columns("Predeterminada").CellActivation = Activation.Disabled

                Dim _pRow As UltraGridRow
                For Each _pRow In .GRID.Rows
                    Dim _CuentaBancaria As Proveedor_CuentaBancaria = _pRow.ListObject
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

                If oLinqProveedor.ID_Proveedor = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Proveedor").Value = oLinqProveedor
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CuentasBancarias_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_CuentasBancarias.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqProveedor.Proveedor_CuentaBancaria.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _CuentaBancaria As Proveedor_CuentaBancaria = e.ListObject
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




End Class