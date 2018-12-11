Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Public Class frmPropuesta
    'Implements IDisposable
    Dim oDTC As DTCDataContext
    Dim oLinqPropuesta As Propuesta
    Dim oLinqInstalacion As Instalacion
    Dim DTAdvertencies As DataTable
    Dim oLinqPropuestaAdvertencias As Propuesta
    Private Const GradoNoTenerEnCuenta As Integer = 0
    Private Const ClaseAmbientalNoTenerEnCuenta As Integer = 0
    Private Const GradoNotificacionlNoTenerEnCuenta As Integer = 0
    Dim Indentat As Boolean = False
    'Dim oInstalacionAnterior As Boolean = False
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2
    Dim oClipBoardGridLineas As New ArrayList
    Public oTipoPropuesta As enumTipoPropuesta
    Dim oDTFotosProductos As New DataTable
    Dim _ArrayFoto As New ArrayList
    Dim _ArrayProducte As New ArrayList

    Enum enumTipoPropuesta
        Propuesta = 1
        InstalacionAnterior = 2
        TalIComoSeInstalo = 3
    End Enum


#Region "M_ToolForm"

    'Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
    '    Try
    '        If oLinqPropuesta.ID_Propuesta <> 0 Then
    '            If Mensaje.Mostrar_Mensaje("Desea eliminar esta propuesta?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
    '                oLinqPropuesta.Activo = False
    '                oDTC.SubmitChanges()
    '                Call Netejar_Pantalla()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)

    '    End Try
    'End Sub
    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Call Guardar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Imprimir() Handles ToolForm.m_ToolForm_Imprimir
        If Guardar() = False Then
            Exit Sub
        End If
        Dim _DTC As New DTCDataContext(BD.Conexion)
        Informes.ObrirInformePreparacio(_DTC, EnumInforme.Propuesta, "[ID_Propuesta] = " & oLinqPropuesta.ID_Propuesta, "Propuesta - " & oLinqPropuesta.Codigo)
        '"[ID_Personal] = " & oLinqPersonal.ID_Personal

        'Dim frm As New frmInformePreparacio
        'frm.Entrada(EnumInforme.Propuesta, "[ID_Propuesta] = " & oLinqPropuesta.ID_Propuesta, oLinqPropuesta.ID_Propuesta)
        'frm.FormObrir(Me, False)
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pInstalacion As Instalacion, ByRef pDTC As DTCDataContext, Optional ByVal pId As Integer = 0, Optional ByVal pInstalacionAnterior As Boolean = False)

        Try

            Me.AplicarDisseny()
            Me.GRD_Linea.GRID.DisplayLayout.MaxBandDepth = 15
            Me.GroupBoxPie.UseAppStyling = False

            Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True

            oLinqInstalacion = pInstalacion
            ' oInstalacionAnterior = pInstalacionAnterior

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Propuesta_Archivo", 1)
            Me.Preview_RTF.pBotoGuardarVisible = True
            'AddHandler Fichero.DespresDeAfegirRegistre, AddressOf FicheroBinarioAlAfegirRegistre

            oDTC = pDTC

            Util.Cargar_Combo(Me.C_Estado, "SELECT ID_Propuesta_Estado, Descripcion FROM Propuesta_Estado WHERE Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_EstadoCRM, "SELECT ID_Propuesta_EstadoCRM, Descripcion + ' (' + cast(PorcentajeCompleto as nvarchar(10)) + '%)' as Descripcion FROM Propuesta_EstadoCRM WHERE Activo=1 ORDER BY PorcentajeCompleto", False)
            Util.Cargar_Combo(Me.C_Tipo, "SELECT ID_Propuesta_Tipo, Descripcion FROM Propuesta_Tipo WHERE Activo=1 ORDER BY Codigo", True)
            Util.Cargar_Combo(Me.C_Grado, "SELECT ID_Producto_Grado, Descripcion FROM Producto_Grado WHERE Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_Grado_Notificacion, "SELECT ID_Grado_Notificacion, Descripcion FROM Grado_Notificacion WHERE Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_GradoSugerido, "SELECT ID_Producto_Grado, Descripcion FROM Producto_Grado WHERE Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_GradoTipoNegocio, "SELECT ID_Producto_Grado, Descripcion FROM Producto_Grado WHERE Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_Comercial, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and ID_Personal_Tipo=" & EnumPersonalTipo.Comercial & " ORDER BY Nombre", False, True, "No asignado")
            Util.Cargar_Combo(Me.C_FormaPago, "SELECT ID_FormaPago, Descripcion FROM FormaPago WHERE Activo=1 ORDER BY Descripcion", False, True, "No asignado")
            Util.Cargar_Combo(Me.C_Receptora, "SELECT ID_Receptora, Nombre FROM Receptora WHERE Activo=1 ORDER BY Nombre", False, True, "No asignado")
            Util.Cargar_Combo(Me.C_Emplazamiento, "SELECT ID_Instalacion_Emplazamiento, Descripcion FROM Instalacion_Emplazamiento Where ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " ORDER BY Descripcion", True)

            Util.Cargar_Combo(Me.C_Empresa, "SELECT ID_Empresa, NombreComercial FROM Empresa ORDER BY NombreComercial", False)

            Dim SQLText As String = ""
            If pId <> 0 Then
                SQLText = " and ID_Propuesta<>" & pId
            End If

            Util.Cargar_Combo(Me.C_Vinculacion, "SELECT ID_Propuesta, Codigo FROM Propuesta WHERE Codigo<>0 and Activo=1 and ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & SQLText & "  ORDER BY Descripcion", False)

            Me.DT_Alta.Value = Nothing

            Call Netejar_Pantalla()


            Me.GRD_Linea.M.clsToolBar.Boto_Afegir("VerProducto", "Ver ficha producto", True)
            'Me.GRD_Linea.M.clsToolBar.Boto_Afegir("VisualizarFotos", "Visualizar fotos", True)
            Me.GRD_Linea.M.clsToolBar.Boto_Afegir("ActualizarNivelesAnidamiento", "Actualizar niveles anidamiento", True)
            Me.GRD_Linea.M.clsToolBar.Boto_Afegir("DuplicarALRestoDeEmplazamientos", "Duplicar al resto de ubicaciones", True)
            Me.GRD_Planos.M.clsToolBar.Boto_Afegir("Plano", "Planos/Diagramas", True)
            Me.GRD_Planos.M.clsToolBar.Boto_Afegir("Duplicar", "Duplicar", True)
            Me.GRD_Diagrama.M.clsToolBar.Boto_Afegir("Diagrama", "Diseñar diagrama", True)
            Me.GRD_Diagrama.M.clsToolBar.Boto_Afegir("Duplicar", "Duplicar", True)

            If pId <> 0 Then
                Call Cargar_Form(pId)
                If oLinqPropuesta Is Nothing Then
                    GoTo ComSiNoHaguessinPassatUnIdPropuesta
                End If
            Else
ComSiNoHaguessinPassatUnIdPropuesta:
                'Si estem afegint una nova "propuesta" si ens han passat el paràmentre instalacion anterior a true voldrà dir que es una instalacion anterior si no, com que no es pot crear un tal i como se instaló, llavors serà una propuesta
                If pInstalacionAnterior = True Then
                    oTipoPropuesta = enumTipoPropuesta.InstalacionAnterior
                Else
                    oTipoPropuesta = enumTipoPropuesta.Propuesta
                End If
                If oLinqInstalacion.Cliente.ID_Personal.HasValue Then
                    Me.C_Comercial.Value = oLinqInstalacion.Cliente.ID_Personal
                End If

                If oLinqInstalacion.Cliente.ID_FormaPago.HasValue = True Then
                    Me.C_FormaPago.Value = oLinqInstalacion.Cliente.ID_FormaPago
                End If
                Me.T_Codigo.Text = oDTC.Contadores.FirstOrDefault.Propuesta
                oDTC.Contadores.FirstOrDefault.Propuesta = oDTC.Contadores.FirstOrDefault.Propuesta + 1
                Me.C_Grado.Value = 0 'no tener en cuenta
            End If
            Me.KeyPreview = False
            '            ' Me.GRD_Associacio.GRID.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation

            Me.Tab_Principal.Tabs("General").Selected = True

            Call HabilitarPestanyas()

            If oLinqPropuesta.ID_Propuesta_Estado <> EnumPropuestaEstado.Pendiente And oLinqPropuesta.ID_Propuesta_Estado <> 0 Then
                Me.Pan_Cabecera.Enabled = False
                Me.GRD_Linea.M.Botons.tGridAfegir.SharedProps.Enabled = False
                Me.GRD_Linea.M.Botons.tGridEditar.SharedProps.Enabled = False
                Me.GRD_Linea.M.Botons.tGridEliminar.SharedProps.Enabled = False
                Me.CH_ConectadoCRA.Enabled = False
                Me.C_Grado.ReadOnly = False
                Me.C_Grado_Notificacion.ReadOnly = False
                'Me.GRD_Planos.M.Botons.tGridEditar.SharedProps.Enabled = False
                'Me.GRD_Planos.M.Botons.tGridAfegir.SharedProps.Enabled = False
                Me.R_Observaciones.pEnable = False
            End If

            If oLinqPropuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Traspasado Then
                Fichero.Desactivar()
            End If

            If oTipoPropuesta = enumTipoPropuesta.TalIComoSeInstalo Then
                Me.Pan_Cabecera.Visible = False
                Me.GroupBoxPie.Visible = False
                'Me.Tab_Principal.Top = 43
                Me.Tab_Principal.Height = Me.Height - 90
                Me.Text = "Tal y cómo se instaló"
                Me.GRD_Linea.M.clsToolBar.Boto_Afegir("VistaIndentada", "Ocultar vista indentada", True)
                Me.GRD_Linea.M.clsToolBar.Boto_Afegir("NumerosSerieDuplicados", "Números de serie duplicados", True)
                Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = False
                Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Especificaciones")
                Me.C_Empresa.Visible = False
                Me.UltraLabel34.Visible = False
            Else
                Me.GRD_Linea.M.clsToolBar.Boto_Afegir("VistaIndentada", "Vista indentada", True)
            End If

            If oTipoPropuesta = enumTipoPropuesta.InstalacionAnterior Then  'si el pressupost es de tipus Instalacion Anterior...
                Me.GroupBoxPie.Visible = False
                Me.C_Tipo.Value = CInt(EnumPropuestaTipo.InstalacionAnterior)
                Me.C_Tipo.ReadOnly = True
                Me.C_Vinculacion.ReadOnly = True
            Else
                Me.C_Tipo.Items.Remove(2)   'El tipus "Instalacion Anterior" no ha d'apareixer a menys que sigui una instalacion anterior
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.T_Codigo.Focus()
            'If oLinqProducto.ID_Producto = 0 Then
            '    If BD.RetornaValorSQL("SELECT Count (*) From Associat WHERE Nom='" & Util.APOSTROF(Me.T_Nom.Text) & "'") > 0 Then
            '        Mensaje.Mostrar_Mensaje("Impossible afegir, nom de l'associat existent", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '        Exit Function
            '    End If
            'Else
            '    If BD.RetornaValorSQL("SELECT Count (*) From Associat WHERE Nom='" & Util.APOSTROF(Me.T_Nom.Text) & "' and id_associat<>" & oLinqAssociat.ID_Associat) > 0 Then
            '        Mensaje.Mostrar_Mensaje("Impossible modificar, nom de l'associat existent", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '        Exit Function
            '    End If
            'End If

            'If Me.C_Forma_Juridica.Value = Nothing Then
            '    Mensaje.Mostrar_Mensaje("Cal informar la forma jurídica", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Exit Function
            'End If

            'If Me.C_VOTS.Value = Nothing Then
            'Mensaje.Mostrar_Mensaje("Cal informar el nº de vots", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            'Exit Function
            'End If  ' Alex 29/10/2008 No volen els bots obligatoris


            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqPropuesta)

            If oLinqPropuesta.ID_Propuesta = 0 Then  ' Comprovacio per saber si es un insertar o un nou

                oDTC.Propuesta.InsertOnSubmit(oLinqPropuesta)


                oDTC.SubmitChanges()


                'Les 5 línies inferiors son per automàticament, al afegir, asigarem el usuari actual com a propietari del pressupost
                Dim _UsuariActual As New Propuesta_Seguridad
                _UsuariActual.ID_Usuario = Seguretat.oUser.ID_Usuario
                _UsuariActual.ID_Propuesta = oLinqPropuesta.ID_Propuesta
                oLinqPropuesta.Propuesta_Seguridad.Add(_UsuariActual)
                oDTC.SubmitChanges()
                'oDTC.Propuesta_Seguridad.InsertOnSubmit(_UsuariActual)


                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Call Fichero.Cargar_GRID(oLinqPropuesta.ID_Propuesta) 'Fem això pq la classe tingui el ID de pressupost

                Dim _ClsNotificacion As New clsNotificacion(oDTC)
                _ClsNotificacion.CrearNotificacion_AlCrearPresupuesto(oLinqPropuesta)
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If


            If oLinqPropuesta.SeInstalo = False And oLinqPropuesta.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior Then
                Me.EstableixCaptionForm("Propuesta: " & (oLinqPropuesta.Descripcion))
            End If


            'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqPropuesta
                Me.T_Codigo.Text = .Codigo
                Me.T_Descripcion.Text = .Descripcion
                Me.DT_Alta.Value = .Fecha
                Me.C_Grado.Value = .ID_Producto_Grado
                Me.C_Estado.Value = .ID_Propuesta_Estado
                Me.C_Tipo.Value = .ID_Propuesta_Tipo
                Me.T_Persona.Text = .Persona
                Me.CH_Segun_Normativa.Checked = .SegunNormativa
                Me.T_Version.Text = .Version
                Me.C_Emplazamiento.Value = .ID_Instalacion_Emplazamiento
                Me.C_Planta.Value = .ID_Instalacion_Emplazamiento_Planta
                Me.C_Zona.Value = .ID_Instalacion_Emplazamiento_Zona
                If .ID_Propuesta_Relacion <> 0 Then
                    Me.C_Vinculacion.Value = .ID_Propuesta_Relacion
                End If
                Me.CH_ConectadoCRA.Checked = .ConectadoCRA
                Me.C_Grado_Notificacion.Value = .ID_Grado_Notificacion

                If .ID_Receptora Is Nothing OrElse .ID_Receptora = 0 Then
                    Me.C_Receptora.Value = 0
                Else
                    Me.C_Receptora.Value = .ID_Receptora
                End If

                Me.T_TiempoInstalacion.Value = .TiempoInstalacion
                Me.T_Validez.Value = .Validez
                If .ID_Personal Is Nothing OrElse .ID_Personal = 0 Then
                    Me.C_Comercial.Value = 0
                Else
                    Me.C_Comercial.Value = .ID_Personal
                End If
                If .ID_FormaPago Is Nothing OrElse .ID_FormaPago = 0 Then
                    Me.C_FormaPago.Value = 0
                Else
                    Me.C_FormaPago.Value = .ID_FormaPago
                End If
                Me.R_Observaciones.pText = .Observaciones
                Me.T_Descuento.Value = DbnullToNothing(.Descuento)
                Me.R_DetallesExtendidos.pText = .DetalleExtendido

                Me.DT_FechaPrevisionCierre.Value = .FechaPrevisionCierre

                If .ID_Propuesta_EstadoCRM.HasValue = False Then
                    Me.C_EstadoCRM.Value = 0
                Else
                    Me.C_EstadoCRM.Value = .ID_Propuesta_EstadoCRM
                End If

                Me.T_HorasPrevistas.Value = .HorasPrevistas



                If .Hoja Is Nothing = False Then
                    Me.Excel1.M_LoadDocument(.Hoja)
                End If

                Me.C_Empresa.Value = .ID_Empresa
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pPropuesta As Propuesta)
        Try
            With pPropuesta
                .Activo = True

                .Codigo = Me.T_Codigo.Text
                .Descripcion = Me.T_Descripcion.Text
                .Fecha = Me.DT_Alta.Value
                .ID_Producto_Grado = Me.C_Grado.Value
                .ID_Propuesta_Estado = Me.C_Estado.Value
                If Me.C_Vinculacion.SelectedIndex <> -1 Then
                    .ID_Propuesta_Relacion = Me.C_Vinculacion.Value
                Else
                    .ID_Propuesta_Relacion = Nothing
                End If
                .ID_Propuesta_Tipo = Me.C_Tipo.Value
                .Persona = Me.T_Persona.Text
                .SegunNormativa = Me.CH_Segun_Normativa.Checked
                .Version = Me.T_Version.Text
                .Instalacion = oLinqInstalacion
                .ConectadoCRA = Me.CH_ConectadoCRA.Checked
                .ID_Grado_Notificacion = Me.C_Grado_Notificacion.Value
                .TiempoInstalacion = CInt(Me.T_TiempoInstalacion.Value)
                .Validez = DbnullToNothing(Me.T_Validez.Value)

                If Me.C_Comercial.Value = 0 Then
                    .Personal = Nothing
                Else
                    .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_Comercial.Value)).FirstOrDefault
                End If

                If Me.C_FormaPago.Value = 0 Then
                    .FormaPago = Nothing
                Else
                    .FormaPago = oDTC.FormaPago.Where(Function(F) F.ID_FormaPago = CInt(Me.C_FormaPago.Value)).FirstOrDefault
                End If

                If Me.C_Receptora.Value = 0 Then
                    .Receptora = Nothing
                Else
                    .Receptora = oDTC.Receptora.Where(Function(F) F.ID_Receptora = CInt(Me.C_Receptora.Value)).FirstOrDefault
                End If

                If Me.C_Emplazamiento.SelectedIndex <> -1 Then
                    .Instalacion_Emplazamiento = oDTC.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = CInt(Me.C_Emplazamiento.Value)).FirstOrDefault
                Else
                    .Instalacion_Emplazamiento = Nothing
                End If

                If Me.C_Planta.SelectedIndex <> -1 Then
                    .Instalacion_Emplazamiento_Planta = oDTC.Instalacion_Emplazamiento_Planta.Where(Function(F) F.ID_Instalacion_Emplazamiento_Planta = CInt(Me.C_Planta.Value)).FirstOrDefault
                Else
                    .Instalacion_Emplazamiento_Planta = Nothing
                End If

                If Me.C_Zona.SelectedIndex <> -1 Then
                    .Instalacion_Emplazamiento_Zona = oDTC.Instalacion_Emplazamiento_Zona.Where(Function(F) F.ID_Instalacion_Emplazamiento_Zona = CInt(Me.C_Zona.Value)).FirstOrDefault
                Else
                    .Instalacion_Emplazamiento_Zona = Nothing
                End If

                If Me.C_EstadoCRM.SelectedIndex <> -1 Then
                    .Propuesta_EstadoCRM = oDTC.Propuesta_EstadoCRM.Where(Function(F) F.ID_Propuesta_EstadoCRM = CInt(Me.C_EstadoCRM.Value)).FirstOrDefault
                Else
                    .Propuesta_EstadoCRM = Nothing
                End If

                .FechaPrevisionCierre = Me.DT_FechaPrevisionCierre.Value
                .Observaciones = Me.R_Observaciones.pText
                .Base = CDbl(Me.T_TotalBase.Value)
                .IVA = CDbl(Me.T_TotalIVA.Value)
                .Descuento = CDbl(Me.T_Descuento.Value)
                .Total = CDbl(Me.T_TotalPropuesta.Value)
                .DetalleExtendido = Me.R_DetallesExtendidos.pText

                .HorasPrevistas = DbnullToNothing(Me.T_HorasPrevistas.Value)

                .Hoja = Me.Excel1.M_SaveDocument.ToArray

                .Empresa = oDTC.Empresa.Where(Function(F) F.ID_Empresa = CInt(Me.C_Empresa.Value)).FirstOrDefault
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            ' Call Netejar_Pantalla()

            oLinqPropuesta = (From taula In oDTC.Propuesta Where taula.ID_Propuesta = pID Select taula).First

            If oLinqPropuesta.SeInstalo = True Then
                oTipoPropuesta = enumTipoPropuesta.TalIComoSeInstalo
            Else
                If oLinqPropuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then
                    oTipoPropuesta = enumTipoPropuesta.InstalacionAnterior
                Else
                    oTipoPropuesta = enumTipoPropuesta.Propuesta
                End If
            End If

            'Si no es tal i como se instaló llavors comprovarem si te permisos a nivell de registre
            If oTipoPropuesta = enumTipoPropuesta.Propuesta Or oTipoPropuesta = enumTipoPropuesta.InstalacionAnterior Then
                If ComprovarPermisosSeguretat() = False Then
                    Me.pNoTobrisFormulari = True
                    oLinqPropuesta = New Propuesta 'fem això pq la instancia te totes les dades carregades, així que millor buidarla si no tens permisos
                    Exit Sub
                End If
            End If

            Call SetToForm()

            If oLinqPropuesta.SeInstalo = True Then
                Me.AccessibleName = "SeInstalo"
                Indentat = True
            End If
            Call CargaGrid_Propuesta_Linea(pID)
            Fichero.Cargar_GRID(pID)

            Me.EstableixCaptionForm("Propuesta: " & (oLinqPropuesta.Descripcion))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqPropuesta = New Propuesta
        ' oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.T_Codigo.Value = Nothing
        Me.T_Codigo.Tag = Nothing
        Me.DT_Alta.Value = Now.Date

        Util.Cargar_Combo(Me.C_Comercial, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and ID_Personal_Tipo=" & EnumPersonalTipo.Comercial & " ORDER BY Nombre", False, True, "No asignado")

        Me.C_Estado.SelectedIndex = 0
        'Me.C_Tipo.SelectedIndex = -1
        Me.C_Grado.SelectedIndex = -1
        Me.C_Vinculacion.SelectedIndex = -1
        Me.C_Grado_Notificacion.Value = GradoNotificacionlNoTenerEnCuenta
        Me.C_Comercial.Value = 0
        Me.C_FormaPago.Value = 0

        'si hi han items, seleccionarem el primer
        If Me.C_Emplazamiento.Items.Count > 0 Then
            Me.C_Emplazamiento.SelectedIndex = 0
        End If
        'Me.C_Emplazamiento.SelectedIndex = -1
        Me.C_Planta.SelectedIndex = -1

        Me.C_Empresa.SelectedIndex = BD.RetornaValorSQL("Select ID_Empresa From Empresa Where Predeterminada=1")

        Me.T_Version.Text = "A"
        Me.T_Persona.Text = oLinqInstalacion.PersonaContacto

        Call CargaGrid_Propuesta_Linea(0)
        Fichero.Cargar_GRID(0)
        Me.EstableixCaptionForm("Propuesta")

        Me.Excel1.M_NewDocument()

        ErrorProvider1.Clear()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.T_Codigo)
                oClsControls.ControlBuit(.T_Descripcion)
                oClsControls.ControlBuit(.C_Estado)
                oClsControls.ControlBuit(.C_Grado)
                oClsControls.ControlBuit(.C_Tipo)
                oClsControls.ControlBuit(.DT_Alta)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function DbnullToNothing(ByVal pValor As Object) As Decimal?
        ' DbnullToNothing = pValor
        Try
            If pValor Is Nothing = False Then
                If IsDBNull(pValor) = True Then
                    Return Nothing
                Else
                    Return CDbl(pValor)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub CalcularTotals()
        Dim oDTC As New DTCDataContext(BD.Conexion)
        Dim oLinqPropuesta2 As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = oLinqPropuesta.ID_Propuesta).FirstOrDefault
        If oLinqPropuesta2 Is Nothing Then
            Exit Sub
        End If
        If oLinqPropuesta2.Propuesta_Linea.Count = 0 Then
            Me.T_TotalBase.Value = 0
            Me.T_TotalIVA.Value = 0
            Me.T_TotalPropuesta.Value = 0
            Me.T_TiempoInstalacion.Value = 0
            Me.T_Descuento_Importe.Value = 0
            Me.T_Descuento.Value = 0
        Else
            Me.T_TotalBase.Value = oLinqPropuesta2.Propuesta_Linea.Where(Function(X) X.ID_Propuesta_Opcion.HasValue = False).Sum(Function(F) F.TotalBase).Value
            If Me.T_Descuento.Value Is Nothing OrElse IsDBNull(Me.T_Descuento.Value) OrElse Me.T_Descuento.Value = 0 Then
                Me.T_Descuento.Value = 0
                Me.T_Descuento_Importe.Value = 0
            Else
                Me.T_Descuento_Importe.Value = Math.Round((Me.T_TotalBase.Value * Me.T_Descuento.Value) / 100, 2)
            End If

            Me.T_Base_Menos_Descuento.Value = Math.Round(Me.T_TotalBase.Value - Me.T_Descuento_Importe.Value, 2)

            Dim _TotalIva As Decimal = Util.Comprobar_NULL_Per_0_Decimal(oLinqPropuesta2.Propuesta_Linea.Where(Function(X) X.ID_Propuesta_Opcion.HasValue = False).Sum(Function(F) F.TotalIVA).Value)
            If Me.T_Descuento.Value Is Nothing OrElse IsDBNull(Me.T_Descuento.Value) OrElse Me.T_Descuento.Value = 0 Then
                Me.T_TotalIVA.Value = _TotalIva
            Else
                Me.T_TotalIVA.Value = _TotalIva - Math.Round((_TotalIva * Me.T_Descuento.Value) / 100, 2)
            End If


            Me.T_TotalPropuesta.Value = Me.T_Base_Menos_Descuento.Value + Me.T_TotalIVA.Value

            Dim _TotalCoste As Decimal = oLinqPropuesta2.Propuesta_Linea.Where(Function(X) X.ID_Propuesta_Opcion.HasValue = False).Sum(Function(F) F.Unidad * F.PrecioCoste)
            Me.T_Margen_Total.Value = Me.T_Base_Menos_Descuento.Value - _TotalCoste
            If _TotalCoste = 0 Then
                Me.T_Margen_Porcentaje.Value = 0
            Else
                If Me.T_Base_Menos_Descuento.Value = 0 Then
                    Me.T_Margen_Porcentaje.Value = 0
                Else
                    Me.T_Margen_Porcentaje.Value = (Me.T_Margen_Total.Value * 100) / Me.T_Base_Menos_Descuento.Value  '  (Me.T_Base_Menos_Descuento.Value / _TotalCoste) * 100
                End If

            End If

            Me.T_TiempoInstalacion.Value = Math.Round(oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta = oLinqPropuesta.ID_Propuesta And F.ID_Propuesta_Opcion.HasValue = False).Sum(Function(F) F.Unidad * F.Producto.TiempoInstalacion), 0)
        End If
    End Sub

    Private Function DetectarGradoPropuesta() As Integer
        Try
            Dim MaxGrado As Integer = 0
            If Me.CH_ConectadoCRA.Checked = True Then
                MaxGrado = 2
            End If

            'Dim oDTC As New DTCDataContext(BD.Conexion)

            Dim _Linea As Propuesta_Linea

            For Each _Linea In oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta = oLinqPropuesta.ID_Propuesta)
                If IsNothing(_Linea.Instalacion_Emplazamiento_Zona) = False Then
                    If _Linea.Instalacion_Emplazamiento_Zona.ID_Producto_Grado <> GradoNotificacionlNoTenerEnCuenta AndAlso _Linea.Instalacion_Emplazamiento_Zona.ID_Producto_Grado > MaxGrado Then
                        MaxGrado = _Linea.Instalacion_Emplazamiento_Zona.ID_Producto_Grado
                    End If
                End If
            Next

            If MaxGrado = 0 Then
                MaxGrado = GradoNotificacionlNoTenerEnCuenta
            End If

            Return MaxGrado
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Function DetectarGradoNotificacion() As Integer
        Try
            DetectarGradoNotificacion = 0
            If oLinqPropuesta.ID_Propuesta = 0 Then
                Exit Function
            End If


            Dim oDTC As New DTCDataContext(BD.Conexion)

            Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = oLinqPropuesta.ID_Propuesta).FirstOrDefault

            Dim Grado As Integer
            Grado = Me.C_Grado.Value

            Select Case Grado
                Case 3, 4
                    If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_ATS >= 5).Count() > 0 Then
                        Return 4
                    End If
                    If _Propuesta.Propuesta_Linea.Where(Function(F) (F.Producto.ID_Producto_ATS = 4) And (F.Producto.ID_Producto_ATS2 >= 3)).Count() > 0 Then
                        Return 3
                    End If
                    If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_ATS = 4).Count() > 0 Then
                        If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.Sirena = True And F.Producto.Baterias = True).Count() > 0 Then
                            Return 2
                        End If
                    End If
                    If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_ATS = 4).Count > 0 Then
                        If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.Sirena = True).Sum(Function(F) F.Unidad) > 1 Then
                            Return 1
                        End If
                    End If
                Case 2
                    If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_ATS >= 3).Count() > 0 Then
                        Return 4
                    End If
                    If _Propuesta.Propuesta_Linea.Where(Function(F) (F.Producto.ID_Producto_ATS = 2) And (F.Producto.ID_Producto_ATS2 >= 1)).Count() > 0 Then
                        Return 3
                    End If
                    If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_ATS = 2).Count() > 0 Then
                        If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.Sirena = True And F.Producto.Baterias = True).Count() > 0 Then
                            Return 2
                        End If
                    End If
                    If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_ATS = 2).Count() > 0 Then
                        If _Propuesta.Propuesta_Linea.Where(Function(F) F.Producto.Sirena = True).Sum(Function(F) F.Unidad) > 1 Then
                            Return 1
                        End If
                    End If
                Case Else
                    Return 5
            End Select


            Return DetectarGradoNotificacion
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub HabilitarPestanyas()

        If oLinqPropuesta.ID_Propuesta = 0 Then
            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Intrusion")
        Else
            If oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.Activo = True And F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion).Count > 0 Then
                Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Intrusion")
            Else
                Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Intrusion")
            End If
        End If

        If oLinqPropuesta.ID_Propuesta = 0 Then
            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Incendios")
        Else
            If oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.Activo = True And F.Producto.ID_Producto_Division = EnumProductoDivision.Incendios).Count > 0 Then
                Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Incendios")
            Else
                Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Incendios")
            End If
        End If

        If oLinqPropuesta.ID_Propuesta = 0 Then
            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "CCTV")
        Else
            If oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.Activo = True And F.Producto.ID_Producto_Division = EnumProductoDivision.CCTV).Count > 0 Then
                Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "CCTV")
            Else
                Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "CCTV")
            End If
        End If

        If oLinqPropuesta.ID_Propuesta = 0 Then
            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Accesos")
        Else
            If oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.Activo = True And F.Producto.ID_Producto_Division = EnumProductoDivision.Accesos).Count > 0 Then
                Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Accesos")
            Else
                Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Accesos")
            End If
        End If

        If oLinqPropuesta.ID_Propuesta = 0 Then
            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Megafonia")
        Else
            If oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.Activo = True And F.Producto.ID_Producto_Division = EnumProductoDivision.Megafonia).Count > 0 Then
                Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Megafonia")
            Else
                Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Megafonia")
            End If
        End If

        'If oLinqPropuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then
        '    Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Intrusion", "Incendios", "CCTV", "Megafonia", "Accesos")
        'End If

        If oLinqPropuesta.SeInstalo = True Then
            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Planos", "Diagrama", "Seguridad", "General", "CRM", "Opciones", "Financiacion")
        End If

        If oLinqPropuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then
            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Opciones", "Financiacion")
        End If


    End Sub

    Public Sub CalcularGrau()
        Try


            Dim Calcul As Integer

            Calcul = oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Daños.Where(Function(F) F.ID_Valoracion.HasValue = True).Sum(Function(F) F.Valoracion.Puntuacion).Value
            Calcul = Calcul + oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Peligros.Where(Function(F) F.ID_Valoracion.HasValue = True).Sum(Function(F) F.Valoracion.Puntuacion).Value
            Calcul = Calcul + oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Tipo.Where(Function(F) F.ID_Valoracion.HasValue = True).Sum(Function(F) F.Valoracion.Puntuacion).Value
            Calcul = Calcul + oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Valor.Where(Function(F) F.ID_Valoracion.HasValue = True).Sum(Function(F) F.Valoracion.Puntuacion).Value
            Calcul = Calcul + oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Volumen.Where(Function(F) F.ID_Valoracion.HasValue = True).Sum(Function(F) F.Valoracion.Puntuacion).Value

            'Dim ZonesUsades As ArrayList
            'Dim pepe = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.Activo = True).GroupBy(Function(F) F.ID_Instalacion_Emplazamiento)
            Dim _ZonesUsades = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.Activo = True).Select(Function(F) F.ID_Instalacion_Emplazamiento).Distinct
            Dim _Zones As Integer?

            For Each _Zones In _ZonesUsades
                If _Zones.HasValue = True Then
                    Calcul = Calcul + oLinqPropuesta.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = _Zones).FirstOrDefault.Instalacion_Emplazamiento_Construccion.Sum(Function(F) F.Instalacion_Emplazamiento_Construccion_Tipo.Puntuacion).Value
                    Calcul = Calcul + oLinqPropuesta.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = _Zones).FirstOrDefault.Instalacion_Emplazamiento_Ocupacion.Sum(Function(F) F.Instalacion_Emplazamiento_Ocupacion_Estado.Puntuacion).Value
                    Calcul = Calcul + oLinqPropuesta.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = _Zones).FirstOrDefault.Instalacion_Emplazamiento_Custodia.Sum(Function(F) F.Instalacion_Emplazamiento_Custodia_Estado.Puntuacion).Value
                    Calcul = Calcul + oLinqPropuesta.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = _Zones).FirstOrDefault.Instalacion_Emplazamiento_Localizacion.Sum(Function(F) F.Instalacion_Emplazamiento_Localizacion_Estado.Puntuacion).Value
                    Calcul = Calcul + oLinqPropuesta.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = _Zones).FirstOrDefault.Instalacion_Emplazamiento_SeguridadExistente.Sum(Function(F) F.Instalacion_Emplazamiento_SeguridadExistente_Respuesta.Puntuacion).Value
                    Calcul = Calcul + oLinqPropuesta.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = _Zones).FirstOrDefault.Instalacion_Emplazamiento_HistoriaRobo.Sum(Function(F) F.Instalacion_Emplazamiento_HistoriaRobo_Tipo.Puntuacion).Value
                    Calcul = Calcul + oLinqPropuesta.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = _Zones).FirstOrDefault.Instalacion_Emplazamiento_Entorno.Sum(Function(F) F.Instalacion_Emplazamiento_Entorno_Tipo.Puntuacion).Value
                End If
            Next

            If oLinqInstalacion.ID_Sector.HasValue = True Then
                Me.C_GradoTipoNegocio.Value = oLinqInstalacion.Sector.Producto_Grado.ID_Producto_Grado
            Else
                Me.C_GradoTipoNegocio.Value = 0
            End If

            Dim _Grado As Producto_Grado = oDTC.Producto_Grado.Where(Function(F) Calcul >= F.ValorMinimo And Calcul <= F.ValorMaximo).FirstOrDefault()
            If _Grado Is Nothing = False Then
                Me.C_GradoSugerido.Value = _Grado.ID_Producto_Grado
            Else
                Me.C_GradoSugerido.Value = 0
            End If

            'Si esta conectat a CRA el grau suggerit com a mínim serà 2
            If CH_ConectadoCRA.Checked = True And Me.C_GradoSugerido.Value < 2 Then
                Me.C_GradoSugerido.Value = 2
            End If


            'Si el grau del tipus de negoci és més alt que el sugerit, adoptarem el grau del tipo de negoci
            If C_GradoTipoNegocio.Value > C_GradoSugerido.Value Then
                C_GradoSugerido.Value = C_GradoTipoNegocio.Value
            End If

            Me.T_Puntuacion.Value = Calcul


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function ComprovarPermisosSeguretat() As Boolean
        Try
            ComprovarPermisosSeguretat = False

            'si no s'ha afegit ningú al llistat es accessible per tothom
            If oLinqPropuesta.Propuesta_Seguridad.Count = 0 Then
                Return True
            End If

            'Si te nivell de seguretat 1 te accés accessible
            If Seguretat.oUser.NivelSeguridad = 1 Then
                Return True
            End If

            'si esta dins de la llista te accés
            'If oLinqPropuesta.Propuesta_Seguridad.Contains(1).Count = 1 Then
            ' Return True
            ' End If

            'Si no està dins de la llista però algu de la llista esta a càrrec del usuari actual llavors tb tindrem accés
            ' If oLinqPropuesta.Propuesta_Seguridad.Where(Function(F) F.Usuario.Personal.Personal_PersonalACargo.Where(Function(F2) F2.id_p
            'Dim pepe = From Taula In oDTC.Personal_PersonalACargo Where Taula.c Select ID_Usuario = Taula.Personal.Usuario.FirstOrDefault.ID_Usuario, NombreUsuario = Taula.Personal.Usuario.FirstOrDefault.Nombre, ID_Personal = Taula.ID_Personal, NombrePersonal = Taula.Personal.Nombre, Taula.ID_PersonalACargo
            Dim pepe = From A In oLinqPropuesta.Propuesta_Seguridad Where (From Taula In oDTC.Personal_PersonalACargo Select Taula.ID_PersonalACargo).Contains(A.Usuario.ID_Personal)
            Dim Juan As IEnumerable(Of Personal_PersonalACargo) = From Taula In oDTC.Personal_PersonalACargo Where (From A In oLinqPropuesta.Propuesta_Seguridad Select A.Usuario.Personal.ID_Personal).Contains(Taula.ID_PersonalACargo)

            If Juan.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 Then
                Return True
            End If


            'Dim a5 = From A1 In oUser.Personal.Personal_PersonalACargo Select A1
            'Dim b5 = From B1 In oLinqPropuesta.Propuesta_Seguridad Select B1

            'Dim Query = From A In oUser.Personal.Personal_PersonalACargo Where (From C In oLinqPropuesta.Propuesta_Seguridad Select C.ID_Usuario).Contains(A.ID_Personal_PersonalACargo) Select A

            Mensaje.Mostrar_Mensaje("Imposible cargar los datos, no tiene permisos", M_Mensaje.Missatge_Modo.INFORMACIO)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub CalcularPreusLinea(ByRef pRow As UltraGridRow)
        If IsDBNull(pRow.Cells("Precio").Value) OrElse IsDBNull(pRow.Cells("Unidad").Value) OrElse IsDBNull(pRow.Cells("Descuento").Value) Then
            Exit Sub
        End If


        Dim _Preu As Decimal = pRow.Cells("Precio").Value
        Dim _Quantitat As Decimal = pRow.Cells("Unidad").Value
        Dim _Descompte As Decimal = pRow.Cells("Descuento").Value
        Dim _PrecioCoste As Decimal = pRow.Cells("PrecioCoste").Value
        Dim _IVA As Decimal = pRow.Cells("IVA").Value


        pRow.Cells("TotalBase").Value = (_Quantitat * _Preu - ((_Quantitat * _Preu) * _Descompte) / 100)
        pRow.Cells("TotalIVA").Value = (pRow.Cells("TotalBase").Value * _IVA) / 100
        pRow.Cells("TotalLinea").Value = pRow.Cells("TotalBase").Value + pRow.Cells("TotalIVA").Value
        pRow.Cells("TotalPrecioCoste").Value = _Quantitat * _PrecioCoste
        pRow.Cells("Margen").Value = pRow.Cells("TotalBase").Value - pRow.Cells("TotalPrecioCoste").Value
        'If Me.T_TotalCoste.Value Is Nothing = False Then
        '    Me.T_Margen.Value = Me.T_TotalBase.Value - Util.Comprobar_NULL_Per_0_Decimal(Me.T_TotalCoste.Value)
        'Else
        '    Me.T_Margen.Value = Me.T_TotalBase.Value
        'End If

        Call CalcularTotals()
    End Sub

#End Region

#Region "Events Varis"

    Private Sub Tab_Principal_SelectedTabChanged(sender As System.Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles Tab_Principal.SelectedTabChanged
        Select Case e.Tab.Key
            Case "Intrusion"
                Call CargarAdvertencias_Intrusion()
                Call CalcularGrau()
            Case "CCTV"
                Call CargarAdvertencias_CCTV()
            Case "Planos"
                Call CargaGrid_Planos(oLinqPropuesta.ID_Propuesta)
            Case "Diagrama"
                Call CargaGrid_Diagramas(oLinqPropuesta.ID_Propuesta)
            Case "Ruta"
                Call CargaGrid_Ruta(oLinqPropuesta.ID_Propuesta)
            Case "Seguridad"
                Call CargaGrid_Seguridad(oLinqPropuesta.ID_Propuesta)
            Case "VinculacionEnergetica"
                Call CargarGrid_VinculacionEnergetica(oLinqPropuesta.ID_Propuesta)
            Case "Especificaciones"
                Call CargaGrid_Respostes(oLinqPropuesta.ID_Propuesta)
            Case "Opciones"
                Call CargaGrid_Opciones(oLinqPropuesta.ID_Propuesta)
            Case "Financiacion"
                Call CargaGrid_Financiacion(oLinqPropuesta.ID_Propuesta)
        End Select
    End Sub

    Private Sub C_Tipo_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Tipo.ValueChanged
        If Me.C_Tipo.SelectedIndex = -1 OrElse Me.C_Tipo.SelectedItem.DataValue = 1 Then
            Me.C_Vinculacion.ReadOnly = True
            Me.C_Vinculacion.SelectedIndex = -1
        Else
            Me.C_Vinculacion.ReadOnly = False
        End If
    End Sub

    Private Sub GRD_Advertencias_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Advertencias_Intrusion.M_GRID_DoubleClickRow
        Try

            If Me.GRD_Advertencias_Intrusion.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            Dim IDLinea As Integer = Me.GRD_Advertencias_Intrusion.GRID.Selected.Rows(0).Cells("Linea").Value

            If IDLinea = 0 Then
                Exit Sub

            End If
            Dim frm As New frmPropuesta_Linea
            frm.Entrada(oLinqInstalacion, oLinqPropuesta, oDTC, IDLinea)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmPropuesta_Linea
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Advertencias_CCTV_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Advertencias_CCTV.M_GRID_DoubleClickRow
        Try

            If Me.GRD_Advertencias_CCTV.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            Dim IDLinea As Integer = Me.GRD_Advertencias_CCTV.GRID.Selected.Rows(0).Cells("Linea").Value

            If IDLinea = 0 Then
                Exit Sub

            End If
            Dim frm As New frmPropuesta_Linea
            frm.Entrada(oLinqInstalacion, oLinqPropuesta, oDTC, IDLinea)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmPropuesta_Linea
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CH_ConectadoCRA_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_ConectadoCRA.CheckedChanged
        Me.C_Grado.Value = DetectarGradoPropuesta()
        Me.C_Grado_Notificacion.Value = DetectarGradoNotificacion()
        Call CalcularGrau()
        If oLinqPropuesta.ID_Propuesta <> 0 Then
            oLinqPropuesta.Producto_Grado = oDTC.Producto_Grado.Where(Function(F) F.ID_Producto_Grado = CInt(Me.C_Grado.Value)).FirstOrDefault
            'oLinqPropuesta.ID_Producto_Grado = Me.C_Grado.Value
            oLinqPropuesta.Grado_Notificacion = oDTC.Grado_Notificacion.Where(Function(F) F.ID_Grado_Notificacion = CInt(Me.C_Grado_Notificacion.Value)).FirstOrDefault
            'oLinqPropuesta.ID_Grado_Notificacion = Me.C_Grado_Notificacion.Value
            oDTC.SubmitChanges()
        End If
        If Me.CH_ConectadoCRA.Checked = True Then
            Me.C_Receptora.ReadOnly = False
        Else
            Me.C_Receptora.ReadOnly = True
            Me.C_Receptora.Value = 0
        End If
    End Sub

    Private Sub T_Descuento_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles T_Descuento.KeyUp
        Call CalcularTotals()
    End Sub

    Private Sub FicheroBinarioAlAfegirRegistre(ByVal pID As Integer)
        'Try
        '    If pID = 0 Then
        '        Exit Sub
        '    End If

        '    Dim _DTC As New DTCDataContext(BD.Conexion)
        '    Dim _Archivo As Archivo = _DTC.Archivo.Where(Function(F) F.ID_Archivo = pID).FirstOrDefault
        '    Dim _Prop_Archivo As Propuesta_Archivo = _DTC.Propuesta_Archivo.Where(Function(f) f.ID_Archivo = pID).FirstOrDefault
        '    ' oDTC.Refresh(Data.Linq.RefreshMode.OverwriteCurrentValues, _Archivo)

        '    Dim _NewArchivo As New Archivo
        '    _NewArchivo.ID_Archivo = _Archivo.ID_Archivo
        '    _NewArchivo.ID_Usuario = _Archivo.ID_Usuario
        '    _NewArchivo.Activo = _Archivo.Activo
        '    _NewArchivo.CampoBinario = _Archivo.CampoBinario
        '    _NewArchivo.Color = _Archivo.Color
        '    _NewArchivo.Descripcion = _Archivo.Descripcion
        '    _NewArchivo.Fecha = _Archivo.Fecha
        '    _NewArchivo.Ruta_Fichero = _Archivo.Ruta_Fichero
        '    _NewArchivo.Tamaño = _Archivo.Tamaño
        '    _NewArchivo.Tipo = _Archivo.Tipo
        '    oDTC.Archivo.Attach(_NewArchivo)

        '    Dim _New_Prop_Archivo As New Propuesta_Archivo
        '    _New_Prop_Archivo.ID_Archivo = _Archivo.ID_Archivo
        '    _New_Prop_Archivo.Propuesta = oLinqPropuesta

        '    oDTC.Propuesta_Archivo.Attach(_New_Prop_Archivo)
        '    oDTC.Refresh(Data.Linq.RefreshMode.OverwriteCurrentValues, Propuesta_Archivo)
        '    oDTC.SubmitChanges()
        'Catch ex As Exception
        '    Mensaje.Mostrar_Mensaje_Error(ex)
        'End Try
    End Sub

    Private Sub C_Emplazamiento_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Emplazamiento.ValueChanged
        If Me.C_Emplazamiento.Value Is Nothing = False Then
            Me.C_Planta.ReadOnly = False
            Me.C_Zona.ReadOnly = True
            Me.C_Planta.Value = Nothing
            Me.C_Zona.Value = Nothing
            If IsNumeric(Me.C_Emplazamiento.Value) = True Then
                Util.Cargar_Combo(Me.C_Planta, "SELECT ID_Instalacion_Emplazamiento_Planta, Descripcion FROM Instalacion_Emplazamiento_Planta WHERE ID_Instalacion_Emplazamiento=" & Me.C_Emplazamiento.Value & " ORDER BY Descripcion", False)
            End If
        End If
    End Sub

    Private Sub C_Planta_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Planta.ValueChanged
        If Me.C_Planta.Value Is Nothing = False Then
            Me.C_Zona.ReadOnly = False
            Me.C_Zona.Value = Nothing
            If IsNumeric(Me.C_Planta.Value) = True Then
                Util.Cargar_Combo(Me.C_Zona, "SELECT ID_Instalacion_Emplazamiento_Zona, Descripcion FROM Instalacion_Emplazamiento_Zona WHERE ID_Instalacion_Emplazamiento_Planta=" & Me.C_Planta.Value & " ORDER BY Descripcion", False)
            End If
        End If
    End Sub

    Private Sub C_Comercial_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Comercial.BeforeDropDown
        Dim _SQL As String = ""
        If oLinqPropuesta.ID_Personal.HasValue Then
            _SQL = " or ID_Personal= " & oLinqPropuesta.ID_Personal
        End If

        Util.Cargar_Combo(Me.C_Comercial, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and FechaBajaEmpresa is null and ID_Personal_Tipo=" & EnumPersonalTipo.Comercial & _SQL & " ORDER BY Nombre", False, True, "No asignado")
        Me.C_Comercial.Value = oLinqPropuesta.ID_Personal
    End Sub

    Private Sub C_Empresa_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Empresa.BeforeDropDown
        Dim _SQL As String = ""
        If oLinqPropuesta.ID_Empresa.HasValue Then
            _SQL = " or ID_Empresa= " & oLinqPropuesta.ID_Empresa
        End If

        Util.Cargar_Combo(Me.C_Empresa, "SELECT ID_Empresa, NombreComercial FROM Empresa WHERE Activo=1 and FechaBaja is null " & _SQL & " ORDER BY NombreComercial", False)
        Me.C_Empresa.Value = oLinqPropuesta.ID_Empresa
    End Sub

#End Region

#Region "GRID Lineas"

    Private Sub CargaGrid_Propuesta_Linea(ByVal pId As Integer)
        Try

            If oTipoPropuesta = enumTipoPropuesta.InstalacionAnterior Then
                Me.AccessibleName = "PropuestaAnterior"
            End If

            Dim SQLOrdenacio As String = ""

            Select Case oTipoPropuesta
                Case enumTipoPropuesta.InstalacionAnterior, enumTipoPropuesta.TalIComoSeInstalo
                    SQLOrdenacio = " Order by Division, CodigoProducto"
                Case enumTipoPropuesta.Propuesta
                    SQLOrdenacio = " Order by Identificador "

            End Select


            With Me.GRD_Linea
                oDTFotosProductos = Nothing
                oDTFotosProductos = BD.RetornaDataTable("SELECT dbo.Instalacion.ID_Instalacion, dbo.Propuesta.ID_Propuesta, dbo.Producto.ID_Producto, dbo.Archivo.CampoBinario FROM  dbo.Producto INNER JOIN dbo.Archivo ON dbo.Producto.ID_Archivo_FotoPredeterminadaMini = dbo.Archivo.ID_Archivo INNER JOIN dbo.Instalacion INNER JOIN dbo.Propuesta ON dbo.Instalacion.ID_Instalacion = dbo.Propuesta.ID_Instalacion INNER JOIN dbo.Propuesta_Linea ON dbo.Propuesta.ID_Propuesta = dbo.Propuesta_Linea.ID_Propuesta ON dbo.Producto.ID_Producto = dbo.Propuesta_Linea.ID_Producto Where Propuesta.ID_Propuesta=" & oLinqPropuesta.ID_Propuesta)

                _ArrayProducte = New ArrayList
                _ArrayFoto = New ArrayList

                If Indentat = False Then
                    .M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & pId & SQLOrdenacio, BD, 3)
                    '.GRID.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow
                    '.GRID.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
                    '.GRID.DisplayLayout.Override.RowSelectorWidth = 16
                Else
                    Dim DTS As New DataSet
                    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is null and ID_Propuesta=" & pId & SQLOrdenacio)
                    Dim i As Integer = 0
                    Dim Fin As Integer

                    If oLinqPropuesta.NivelMaximoLineas.HasValue = True Then
                        Fin = oLinqPropuesta.NivelMaximoLineas - 1
                    Else
                        Fin = 5
                    End If

                    For i = 0 To Fin
                        BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId & SQLOrdenacio, "Propuesta_Linea" & i + 2, i, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                    Next
                    .M.clsUltraGrid.Cargar(DTS, 4)
                End If

                'Si el botó editar del grid esta deshabilitat llavors no es podrà editar el grid
                If Me.GRD_Linea.M.Botons.tGridEditar.SharedProps.Enabled = True Then
                    .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
                    For Each _Banda In Me.GRD_Linea.GRID.DisplayLayout.Bands

                        'per pressupost
                        Call PosarEditableColumnes(_Banda.Index, "Unidad")
                        Call PosarEditableColumnes(_Banda.Index, "Precio")
                        Call PosarEditableColumnes(_Banda.Index, "Descuento")
                        Call PosarEditableColumnes(_Banda.Index, "IVA")
                        Call PosarEditableColumnes(_Banda.Index, "PrecioCoste")

                        'Per tal i como se instaló
                        Call PosarEditableColumnes(_Banda.Index, "Uso")
                        Call PosarEditableColumnes(_Banda.Index, "IdentificadorDelProducto")
                        Call PosarEditableColumnes(_Banda.Index, "NumZona")
                        Call PosarEditableColumnes(_Banda.Index, "NickZona")
                        Call PosarEditableColumnes(_Banda.Index, "BocaConexion")
                        Call PosarEditableColumnes(_Banda.Index, "Particion")
                        'Call PosarEditableColumnes(_Banda.Index, "InstaladoEn")
                        Call PosarEditableColumnes(_Banda.Index, "RutaOrden")
                        Call PosarEditableColumnes(_Banda.Index, "NumSerie")
                        Call PosarEditableColumnes(_Banda.Index, "VLAN")
                        Call PosarEditableColumnes(_Banda.Index, "IP")
                        Call PosarEditableColumnes(_Banda.Index, "MascaraSubred")
                        Call PosarEditableColumnes(_Banda.Index, "PuertaEnlace")
                        Call PosarEditableColumnes(_Banda.Index, "DNSPrimaria")
                        Call PosarEditableColumnes(_Banda.Index, "DNSSecundaria")
                        Call PosarEditableColumnes(_Banda.Index, "IPPublica")
                        Call PosarEditableColumnes(_Banda.Index, "Dominio")
                        Call PosarEditableColumnes(_Banda.Index, "NombreEquipo")
                        Call PosarEditableColumnes(_Banda.Index, "PlazoEntrega")
                        Call CargarCombo_Emplazamiento3(.GRID, _Banda.Index, oLinqInstalacion.ID_Instalacion, True)
                        Call CargarCombo_Planta3(.GRID, _Banda.Index, oLinqInstalacion.ID_Instalacion)
                        Call CargarCombo_Zona3(.GRID, _Banda.Index, oLinqInstalacion.ID_Instalacion)
                        Call CargarCombo_Abertura3(.GRID, _Banda.Index, oLinqInstalacion.ID_Instalacion)
                        Call CargarCombo_ElementoAProteger3(.GRID, _Banda.Index, oLinqInstalacion.ID_Instalacion)
                        Call CargarCombo_InstaladoEn3(.GRID, _Banda.Index, oLinqInstalacion.ID_Instalacion)
                    Next
                End If

                'If .GRID.DisplayLayout.Bands(0).Summaries.Count = 0 Then
                '    .GRID.DisplayLayout.Bands(0).Summaries.Add("Total", Infragistics.Win.UltraWinGrid.SummaryType.Sum, .GRID.DisplayLayout.Bands(0).Columns("Total"), Infragistics.Win.UltraWinGrid.SummaryPosition.UseSummaryPositionColumn)
                '    .GRID.DisplayLayout.Bands(0).Summaries("Total").Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                '    .GRID.DisplayLayout.Bands(0).Summaries("Total").DisplayFormat = "{0:#,##0.00} €"
                '    .GRID.DisplayLayout.Bands(0).Summaries("Total").Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True
                'End If

                Call CalcularTotals()

                Me.C_Grado.Value = DetectarGradoPropuesta()
                Me.C_Grado_Notificacion.Value = DetectarGradoNotificacion()

                If pId <> 0 Then
                    oLinqPropuesta.Producto_Grado = oDTC.Producto_Grado.Where(Function(F) F.ID_Producto_Grado = CInt(Me.C_Grado.Value)).FirstOrDefault
                    oLinqPropuesta.Grado_Notificacion = oDTC.Grado_Notificacion.Where(Function(F) F.ID_Grado_Notificacion = CInt(Me.C_Grado_Notificacion.Value)).FirstOrDefault
                    oDTC.SubmitChanges()
                    Call HabilitarPestanyas()
                End If

                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing

                _ArrayFoto.Clear()
                _ArrayProducte.Clear()
                _ArrayFoto = Nothing
                _ArrayProducte = Nothing

                If oDTFotosProductos Is Nothing = False Then
                    oDTFotosProductos.Clear()
                    oDTFotosProductos.Dispose()
                    oDTFotosProductos = Nothing
                End If

                If oTipoPropuesta <> enumTipoPropuesta.TalIComoSeInstalo Then
                    Dim _Row As Infragistics.Win.UltraWinGrid.UltraGridRow
                    For Each _Row In .GRID.Rows
                        If Util.Comprobar_NULL_Per_0_Decimal(_Row.Cells("PrecioCoste").Value) <= 0 Then
                            _Row.CellAppearance.BackColor = Color.FromArgb(255, 192, 192)
                        End If

                        If Util.Comprobar_NULL_Per_0_Decimal(_Row.Cells("Precio").Value) <= 0 Then
                            _Row.CellAppearance.BackColor = Color.FromArgb(255, 224, 192)
                        End If

                    Next
                End If
            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Abertura3(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Abertura)
            oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Abertura Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion_Detallada Select Taula)

            Dim Valors As New Infragistics.Win.ValueList
            Dim Var As Instalacion_Emplazamiento_Abertura

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Abertura, Var.Descripcion_Detallada)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_Emplazamiento_Abertura").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_Emplazamiento_Abertura").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Abertura3(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDZona As Integer)
        Try
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Abertura)
            oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Abertura Where Taula.ID_Instalacion_Emplazamiento_Zona = pIDZona Order By Taula.Descripcion_Detallada Select Taula)

            Dim Valors As New Infragistics.Win.ValueList
            Dim Var As Instalacion_Emplazamiento_Abertura

            Valors.ValueListItems.Add(DBNull.Value, "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Abertura, Var.Descripcion_Detallada)
            Next

            'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_ElementoAProteger3(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try
            Dim oTaula As IQueryable(Of Instalacion_ElementosAProteger)
            oTaula = (From Taula In oDTC.Instalacion_ElementosAProteger Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Instalacion_ElementosAProteger_Tipo.Descripcion Select Taula)

            Dim Valors As New Infragistics.Win.ValueList
            Dim Var As Instalacion_ElementosAProteger

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_ElementosAProteger, Var.Instalacion_ElementosAProteger_Tipo.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_ElementosAProteger").Style = ColumnStyle.DropDownList

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_ElementosAProteger").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_ElementoAProteger3(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDZona As Integer)
        Try
            Dim oTaula As IQueryable(Of Instalacion_ElementosAProteger)
            oTaula = (From Taula In oDTC.Instalacion_ElementosAProteger Where Taula.ID_Instalacion_Emplazamiento_Zona = pIDZona Order By Taula.Instalacion_ElementosAProteger_Tipo.Descripcion Select Taula)

            Dim Valors As New Infragistics.Win.ValueList
            Dim Var As Instalacion_ElementosAProteger

            Valors.ValueListItems.Add(DBNull.Value, "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_ElementosAProteger, Var.Instalacion_ElementosAProteger_Tipo.Descripcion)
            Next

            'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_InstaladoEn3(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try
            Dim oTaula As IQueryable(Of Instalacion_InstaladoEn)
            oTaula = (From Taula In oDTC.Instalacion_InstaladoEn Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)

            Dim Valors As New Infragistics.Win.ValueList
            Dim Var As Instalacion_InstaladoEn

            Valors.ValueListItems.Add(DBNull.Value, "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_InstaladoEn, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_InstaladoEn").Style = ColumnStyle.DropDownList

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_InstaladoEn").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub CargarCombo_InstaladoEn3(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pID_Instalacion As Integer)
    '    Try
    '        Dim oTaula As IQueryable(Of Instalacion_InstaladoEn)
    '        oTaula = (From Taula In oDTC.Instalacion_InstaladoEn Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pID_Instalacion Order By Taula.Descripcion Select Taula)

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim Var As Instalacion_InstaladoEn

    '        Valors.ValueListItems.Add(DBNull.Value, "")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var.ID_Instalacion_InstaladoEn, Var.Descripcion)
    '        Next

    '        'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

    '        pCelda.ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub GRD_Lineas_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Linea.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "ID_Instalacion_Emplazamiento_Zona"
                Dim _IDPlanta As Integer
                If IsDBNull(e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value) Then
                    _IDPlanta = 0
                Else
                    _IDPlanta = e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value
                End If
                Call CargarCombo_Zona3(e.Cell, _IDPlanta, True)

            Case "ID_Instalacion_Emplazamiento_Planta"
                Dim _IDEmplazamiento As Integer
                If IsDBNull(e.Cell.Row.Cells("ID_Instalacion_Emplazamiento").Value) Then
                    _IDEmplazamiento = 0
                Else
                    _IDEmplazamiento = e.Cell.Row.Cells("ID_Instalacion_Emplazamiento").Value
                End If
                Call CargarCombo_Planta3(e.Cell, _IDEmplazamiento, True)

            Case "ID_Instalacion_ElementosAProteger"
                Dim _IDZona As Integer
                If IsDBNull(e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Zona").Value) Then
                    _IDZona = 0
                Else
                    _IDZona = e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Zona").Value
                End If
                Call CargarCombo_ElementoAProteger3(e.Cell, _IDZona)

            Case "ID_Instalacion_Emplazamiento_Abertura"
                Dim _IDZona As Integer
                If IsDBNull(e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Zona").Value) Then
                    _IDZona = 0
                Else
                    _IDZona = e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Zona").Value
                End If
                Call CargarCombo_Abertura3(e.Cell, _IDZona)

                'Case "ID_Instalacion_InstaladoEn"
                '    Dim _IDInstaladoEn As Integer
                '    If IsDBNull(e.Cell.Row.Cells("ID_Instalacion_InstaladoEn").Value) Then
                '        _IDInstaladoEn = 0
                '    Else
                '        _IDInstaladoEn = e.Cell.Row.Cells("ID_Instalacion_InstaladoEn").Value
                '    End If
                '    Call CargarCombo_InstaladoEn3(e.Cell, oLinqInstalacion.ID_Instalacion)
        End Select
    End Sub

    Private Sub GRD_Lineas_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Linea.M_GRID_CellListSelect
        Try

            If IsDBNull(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue) Then
                e.Cell.Value = DBNull.Value
            End If

            Select Case e.Cell.Column.Key
                Case "ID_Instalacion_Emplazamiento"
                    e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value = DBNull.Value
                    e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Zona").Value = DBNull.Value
                    e.Cell.Row.Cells("ID_Instalacion_ElementosAProteger").Value = DBNull.Value
                    e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Abertura").Value = DBNull.Value
                Case "ID_Instalacion_Emplazamiento_Planta"
                    e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Zona").Value = DBNull.Value
                    e.Cell.Row.Cells("ID_Instalacion_ElementosAProteger").Value = DBNull.Value
                    e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Abertura").Value = DBNull.Value
                Case "ID_Instalacion_Emplazamiento_Zona"
                    e.Cell.Row.Cells("ID_Instalacion_ElementosAProteger").Value = DBNull.Value
                    e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Abertura").Value = DBNull.Value
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Linea_M_GRID_AfterCellUpdate(sender As Object, e As CellEventArgs) Handles GRD_Linea.M_GRID_AfterCellUpdate

        Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Cell.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault


        Select Case e.Cell.Column.Key
            Case "Uso"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Uso = Nothing
                Else
                    _Linea.Uso = e.Cell.Value
                End If
            Case "VLAN"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.VLAN = Nothing
                Else
                    _Linea.VLAN = e.Cell.Value
                End If
            Case "IdentificadorDelProducto"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.IdentificadorDelProducto = Nothing
                Else
                    _Linea.IdentificadorDelProducto = e.Cell.Value
                End If
            Case "NumZona"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.NumZona = Nothing
                Else
                    _Linea.NumZona = e.Cell.Value
                End If

            Case "NickZona"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.NickZona = Nothing
                Else
                    _Linea.NickZona = e.Cell.Value
                End If
            Case "BocaConexion"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.BocaConexion = Nothing
                Else
                    _Linea.BocaConexion = e.Cell.Value
                End If
            Case "Particion"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Particion = Nothing
                Else
                    _Linea.Particion = e.Cell.Value
                End If
            Case "RutaOrden"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.RutaOrden = Nothing
                Else
                    _Linea.RutaOrden = e.Cell.Value
                End If
            Case "NumSerie"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.NumSerie = Nothing
                Else
                    _Linea.NumSerie = e.Cell.Value
                End If
            Case "ID_Instalacion_Emplazamiento"
                If IsDBNull(e.Cell.Value) = True Then
                    _Linea.Instalacion_Emplazamiento = Nothing
                Else
                    _Linea.Instalacion_Emplazamiento = oDTC.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = CInt(e.Cell.Value)).FirstOrDefault
                End If
            Case "ID_Instalacion_Emplazamiento_Planta"
                If IsDBNull(e.Cell.Value) = True Then
                    _Linea.Instalacion_Emplazamiento_Planta = Nothing
                Else
                    _Linea.Instalacion_Emplazamiento_Planta = oDTC.Instalacion_Emplazamiento_Planta.Where(Function(F) F.ID_Instalacion_Emplazamiento_Planta = CInt(e.Cell.Value)).FirstOrDefault()
                End If

            Case "ID_Instalacion_Emplazamiento_Zona"
                If IsDBNull(e.Cell.Value) = True Then
                    _Linea.Instalacion_Emplazamiento_Zona = Nothing
                Else
                    _Linea.Instalacion_Emplazamiento_Zona = oDTC.Instalacion_Emplazamiento_Zona.Where(Function(F) F.ID_Instalacion_Emplazamiento_Zona = CInt(e.Cell.Value)).FirstOrDefault()
                End If

            Case "ID_Instalacion_Emplazamiento_Abertura"
                If IsDBNull(e.Cell.Value) = True Then
                    _Linea.Instalacion_Emplazamiento_Abertura = Nothing
                Else
                    _Linea.Instalacion_Emplazamiento_Abertura = oDTC.Instalacion_Emplazamiento_Abertura.Where(Function(F) F.ID_Instalacion_Emplazamiento_Abertura = CInt(e.Cell.Value)).FirstOrDefault()
                End If
            Case "ID_Instalacion_ElementosAProteger"
                If IsDBNull(e.Cell.Value) = True Then
                    _Linea.Instalacion_ElementosAProteger = Nothing
                Else
                    _Linea.Instalacion_ElementosAProteger = oDTC.Instalacion_ElementosAProteger.Where(Function(F) F.ID_Instalacion_ElementosAProteger = CInt(e.Cell.Value)).FirstOrDefault()
                End If
            Case "ID_Instalacion_InstaladoEn"
                If IsDBNull(e.Cell.Value) = True Then
                    _Linea.Instalacion_InstaladoEn = Nothing
                Else
                    _Linea.Instalacion_InstaladoEn = oDTC.Instalacion_InstaladoEn.Where(Function(F) F.ID_Instalacion_InstaladoEn = CInt(e.Cell.Value)).FirstOrDefault()
                End If
            Case "IP"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.IP = Nothing
                Else
                    _Linea.IP = e.Cell.Value
                End If
            Case "MascaraSubred"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.MascaraSubred = Nothing
                Else
                    _Linea.MascaraSubred = e.Cell.Value
                End If
            Case "PuertaEnlace"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.PuertaEnlace = Nothing
                Else
                    _Linea.PuertaEnlace = e.Cell.Value
                End If
            Case "DNSPrimaria"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.DNSPrimaria = Nothing
                Else
                    _Linea.DNSPrimaria = e.Cell.Value
                End If
            Case "DNSSecundaria"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.DNSSecundaria = Nothing
                Else
                    _Linea.DNSSecundaria = e.Cell.Value
                End If
            Case "IPPublica"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.IPPublica = Nothing
                Else
                    _Linea.IPPublica = e.Cell.Value
                End If
            Case "Dominio"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Dominio = Nothing
                Else
                    _Linea.Dominio = e.Cell.Value
                End If
            Case "NombreEquipo"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.NombreEquipo = Nothing
                Else
                    _Linea.NombreEquipo = e.Cell.Value
                End If

            Case "Unidad"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Unidad = Nothing
                Else
                    _Linea.Unidad = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "Precio"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Precio = Nothing
                Else
                    _Linea.Precio = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "Descuento"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Descuento = Nothing
                Else
                    _Linea.Descuento = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "PrecioCoste"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.PrecioCoste = Nothing
                Else
                    _Linea.PrecioCoste = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "IVA"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.IVA = Nothing
                Else
                    _Linea.IVA = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "PlazoEntrega"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.PlazoEntrega = Nothing
                Else
                    _Linea.PlazoEntrega = e.Cell.Value
                End If
        End Select

        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_Linea_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Linea.M_Grid_InitializeRow
        With Me.GRD_Linea
            If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
                .M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
            End If


            If oDTFotosProductos Is Nothing = False Then
                Dim DTRow As DataRow() = oDTFotosProductos.Select("ID_Producto=" & e.Row.Cells("ID_Producto").Value)
                If DTRow Is Nothing = False AndAlso DTRow.Count > 0 Then

                    If _ArrayProducte.Contains(e.Row.Cells("ID_Producto").Value) = False Then
                        e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(DTRow(0).Item("CampoBinario"))
                        _ArrayFoto.Add(e.Row.Cells("Foto").Appearance)
                        _ArrayProducte.Add(e.Row.Cells("ID_Producto").Value)
                    Else
                        e.Row.Cells("Foto").Appearance = _ArrayFoto(_ArrayProducte.IndexOf(e.Row.Cells("ID_Producto").Value))
                    End If

                End If
            End If

            '  If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
            'Dim _Linea As Propuesta_Linea
            '_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
            'If _Linea.Producto.Archivo Is Nothing = False AndAlso _Linea.Producto.Archivo.CampoBinario.Length > 0 Then
            '    e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(_Linea.Producto.Archivo.CampoBinario.ToArray)
            'End If
            '.GRID.DisplayLayout.Override.DefaultRowHeight = 40
            '   Else
            .GRID.DisplayLayout.Override.DefaultRowHeight = 20
            '   End If
        End With
    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Linea.M_ToolGrid_ToolAfegir
        Try
            'If oLinqPropuesta.ID_Propuesta = 0 Then
            '    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            '    Exit Sub
            'End If

            If Guardar() = False Then
                Exit Sub
            End If

            Dim frm As New frmPropuesta_Linea
            frm.Entrada(oLinqInstalacion, oLinqPropuesta, oDTC)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmPropuesta_Linea
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Linea.M_ToolGrid_ToolClickBotonsExtras
        Select Case e.Tool.Key
            Case "VistaIndentada"
                Indentat = Not Indentat
                If Indentat = True Then
                    '  Me.GRD_Linea.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Ocultar vista indentada"
                    If oLinqPropuesta.SeInstalo = True Then
                        Me.AccessibleName = "SeInstalo"
                    Else
                        Me.AccessibleName = "Indentat"
                    End If
                Else
                    '   Me.GRD_Linea.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Vista indentada"
                    If oLinqPropuesta.SeInstalo = True Then
                        Me.AccessibleName = "NoIndentatSeInstalo"
                    Else
                        Me.AccessibleName = Nothing
                    End If
                End If

                Call CargaGrid_Propuesta_Linea(oLinqPropuesta.ID_Propuesta)

            Case "VerProducto"
                If Me.GRD_Linea.GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If
                Dim _IDProducto As Integer = Me.GRD_Linea.GRID.Selected.Rows(0).Cells("ID_Producto").Value
                Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
                frm.Entrada(_IDProducto)
                frm.FormObrir(Me, True)

            Case "VisualizarFotos"
                'If Me.GRD_Linea.ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
                '    Me.GRD_Linea.ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "Visualizar fotos"
                'Else
                '    Me.GRD_Linea.ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos"
                'End If
                Call CargaGrid_Propuesta_Linea(oLinqPropuesta.ID_Propuesta)

            Case "ActualizarNivelesAnidamiento"
                oLinqPropuesta.NivelMaximoLineas = Mensaje.Mostrar_Entrada_Datos("Introduce el número de niveles de anidamiento máximo", 1, False)
                oDTC.SubmitChanges()

            Case "NumerosSerieDuplicados"
                'Fem la merda del tag per... quan s'apreti un cop el botó de visualitzar els números de serie duplicats es carreguin i si es torna a clickar desapareguin
                If e.Tool.Tag Is Nothing Then
                    Me.GRD_Linea.M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where Activo=1 and ID_Propuesta= " & oLinqPropuesta.ID_Propuesta & " and NumSerie in (Select NumSerie From C_Propuesta_Linea Where Activo=1 and ID_Propuesta= " & oLinqPropuesta.ID_Propuesta & "  and NumSerie<>'' Group By NumSerie Having Count(*)>1)", BD, 3)
                    e.Tool.Tag = "1"
                    e.Tool.SharedProps.Caption = "Ocultar números de serie duplicados"
                Else
                    e.Tool.SharedProps.Caption = "Números de serie duplicados"
                    Call CargaGrid_Propuesta_Linea(oLinqPropuesta.ID_Propuesta)
                    e.Tool.Tag = Nothing

                End If

            Case "DuplicarALRestoDeEmplazamientos"
                Dim _LineaOriginal As Propuesta_Linea
                If Me.GRD_Linea.GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If
                Dim _IDPropuesta_Linea As Integer = Me.GRD_Linea.GRID.Selected.Rows(0).Cells("ID_Propuesta_Linea").Value
                _LineaOriginal = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _IDPropuesta_Linea).FirstOrDefault

                Dim _Emplazamiento As Instalacion_Emplazamiento

                If _LineaOriginal.ID_Instalacion_Emplazamiento.HasValue = False Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción. Sólo las líneas de propuesta que tengan emplazamiento pueden realizar esta acción", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If oLinqInstalacion.Instalacion_Emplazamiento.Count <= 1 Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción. No hay más emplazamientos para poder duplicar", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                For Each _Emplazamiento In oLinqInstalacion.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento <> _LineaOriginal.ID_Instalacion_Emplazamiento)
                    Dim _NewLinea As Propuesta_Linea = clsPropuestaLinea.RetornaDuplicacioInstancia(_LineaOriginal, True)
                    _NewLinea.Activo = True
                    '_NewLinea.Archivo
                    _NewLinea.Instalacion_Emplazamiento = _Emplazamiento
                    _NewLinea.Instalacion_Emplazamiento_Abertura = Nothing
                    _NewLinea.Instalacion_Emplazamiento_Planta = Nothing
                    _NewLinea.Instalacion_Emplazamiento_Zona = Nothing
                    _NewLinea.Instalacion_ElementosAProteger = Nothing
                    _NewLinea.Identificador = clsPropuestaLinea.RetornaUltimIdentificadorDeLinea(oDTC, oLinqPropuesta.ID_Propuesta)
                Next

                oDTC.SubmitChanges()

                Call CargaGrid_Propuesta_Linea(oLinqPropuesta.ID_Propuesta)

        End Select
    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Linea.M_ToolGrid_ToolEditar
        Call GRD_Linea_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub PosarEditableColumnes(ByVal pIDBanda As Integer, ByVal pNomColumna As String)
        Try
            Me.GRD_Linea.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellActivation = Activation.AllowEdit
            Me.GRD_Linea.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellClickAction = CellClickAction.EditAndSelectText
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Linea.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_Linea.GRID.Selected.Cells.Count = 0 And Me.GRD_Linea.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
                Dim _Motivo As String = ""
                If oLinqPropuesta.SeInstalo = True Then ' And pLinea.ID_Propuesta_Linea_Antigua.HasValue = True Then
                    _Motivo = Mensaje.Mostrar_Entrada_Datos("Introduzca el motivo por el qual está eliminando la línea:", "", True, , True)
                End If
                'Comprovem que no hi hagi cap línea amb fills
                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Linea.GRID.Selected.Rows
                    Dim _Linea As Propuesta_Linea = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(pRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault()

                    If oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True And F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea).Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible eliminar la línea ya que tiene una o más líneas relacionadas.", M_Mensaje.Missatge_Modo.INFORMACIO)
                        Exit Sub
                    End If

                    If oDTC.Propuesta_Plano_ElementosIntroducidos.Where(Function(F) F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea).Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible eliminar la línea ya que está asignada a un plano/diagrama", M_Mensaje.Missatge_Modo.INFORMACIO)
                        Exit Sub
                    End If

                    If oDTC.Instalacion_CableadoMontaje.Where(Function(F) F.ID_Propuesta_Linea_Origen = _Linea.ID_Propuesta_Linea Or F.ID_Propuesta_Linea_Destino = _Linea.ID_Propuesta_Linea).Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible eliminar la línea, este artículo está asignado a un cable", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    If _Linea.Entrada_Linea_Propuesta_Linea.Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible eliminar la línea se ha relacionado con un pedido o albarán de venta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    Call EliminarLinea(_Linea, _Motivo)
                Next

            End If

            Call CargaGrid_Propuesta_Linea(oLinqPropuesta.ID_Propuesta)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub EliminarLinea(ByVal pLinea As Propuesta_Linea, Optional ByVal pMotivoEliminacion As String = "")
        'Eliminarem totes les relacions entre linies de pressupost que hi hagi amb la línea eliminada

        Dim Linea As Propuesta_Linea
        For Each Linea In oLinqPropuesta.Propuesta_Linea
            If Linea.ID_Propuesta_Linea_Vinculado = pLinea.ID_Propuesta_Linea Then
                Linea.ID_Propuesta_Linea_Vinculado = Nothing
            End If
        Next


        If oLinqPropuesta.SeInstalo = True Then ' And pLinea.ID_Propuesta_Linea_Antigua.HasValue = True Then
            pLinea.Activo = False
            pLinea.MotivoEliminacion = pMotivoEliminacion
        Else
            'Dim _Acceso As Propuesta_Linea_Acceso
            'For Each _Acceso In pLinea.Propuesta_Linea_Acceso
            '    pLinea.Propuesta_Linea_Acceso.Remove(_Acceso)
            '    oDTC.Propuesta_Linea_Acceso.DeleteOnSubmit(_Acceso)
            'Next

            oDTC.Propuesta_Linea_Acceso.DeleteAllOnSubmit(pLinea.Propuesta_Linea_Acceso)

            Dim _LineaArchivo As Propuesta_Linea_Archivo
            For Each _LineaArchivo In pLinea.Propuesta_Linea_Archivo
                oDTC.Archivo.DeleteOnSubmit(_LineaArchivo.Archivo)
            Next
            oDTC.Propuesta_Linea_Archivo.DeleteAllOnSubmit(pLinea.Propuesta_Linea_Archivo)

            oLinqPropuesta.Propuesta_Linea.Remove(pLinea)
            oDTC.Propuesta_Linea.DeleteOnSubmit(pLinea)
        End If

        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Linea.M_ToolGrid_ToolVisualitzarDobleClickRow
        If oLinqPropuesta.ID_Propuesta = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        If Me.GRD_Linea.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        If Guardar() = False Then
            Exit Sub
        End If

        Dim frm As New frmPropuesta_Linea
        frm.Entrada(oLinqInstalacion, oLinqPropuesta, oDTC, Me.GRD_Linea.GRID.Selected.Rows(0).Cells("ID_Propuesta_Linea").Value)
        AddHandler frm.FormClosing, AddressOf AlTancarfrmPropuesta_Linea
        frm.FormObrir(Me)
    End Sub

    Private Sub AlTancarfrmPropuesta_Linea()
        Call CargaGrid_Propuesta_Linea(oLinqPropuesta.ID_Propuesta)
        Call Fichero.Cargar_GRID(oLinqPropuesta.ID_Propuesta)
    End Sub

    Private Sub GRD_Linea_M_GRID_MouseDownRow(ByRef sender As UltraGrid, ByRef e As MouseEventArgs) Handles GRD_Linea.M_GRID_MouseDownRow
        Try
            GRD_Linea.GRID.ContextMenuStrip = Nothing
            Dim _UIElement As Infragistics.Win.UIElement
            Dim _Row As UltraGridRow

            If sender.ActiveRow Is Nothing Then Exit Sub
            _UIElement = Me.GRD_Linea.GRID.DisplayLayout.UIElement.ElementFromPoint(New Point(e.X, e.Y))

            If _UIElement Is Nothing = False Then
                _Row = _UIElement.GetContext(GetType(UltraGridRow))
                If _Row Is Nothing = False Then
                    If e.Button = Windows.Forms.MouseButtons.Right Then
                        '_Row.Activate()
                        ' _Row.Selected = True

                        If oLinqPropuesta.ID_Propuesta_Estado <> EnumPropuestaEstado.Pendiente And oLinqPropuesta.ID_Propuesta_Estado <> 0 Then
                            Exit Sub
                        End If
                        If Indentat = True Then
                            If oLinqPropuesta.SeInstalo = False Then
                                MenuCortar.Visible = False
                                MenuPegar.Visible = False
                                MenuPegarEnRaiz.Visible = False
                            End If
                            If IsNothing(oClipBoardGridLineas) = True OrElse oClipBoardGridLineas.Count = 0 Then
                                MenuPegar.Enabled = False
                                MenuPegarEnRaiz.Enabled = False
                            Else
                                MenuPegar.Enabled = True
                                MenuPegarEnRaiz.Enabled = True
                            End If

                            ContextMenuStrip1.Show(New Point(e.X + 28, e.Y + 132))

                            'GRD_Linea.GRID.ContextMenuStrip = ContextMenuStrip1
                        End If

                        Dim _Prow As UltraGridRow
                        Dim _IDProducto As Integer = _Row.Cells("ID_Producto").Value
                        Dim _DetectatArticleDiferent As Boolean = False
                        For Each _Prow In Me.GRD_Linea.GRID.Selected.Rows
                            If _Prow.Cells("ID_Producto").Value <> _IDProducto Then
                                _DetectatArticleDiferent = True
                            End If
                        Next
                        If _DetectatArticleDiferent = True Then
                            MenuCambiarArticulo.Enabled = False
                        Else
                            MenuCambiarArticulo.Enabled = True
                        End If

                        'Només es podrà cambiar d'article si estem a tal i como se instaló
                        'If oLinqPropuesta.SeInstalo = False Then
                        '    MenuCambiarArticulo.Enabled = False
                        'End If


                        ' ContextMenuStrip1.Show(Me.GRD_Linea, New System.Drawing.Point(e.X, e.Y))
                    End If
                End If
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub MenuCortar_Click(sender As Object, e As EventArgs) Handles MenuCortar.Click
        oClipBoardGridLineas = New ArrayList
        Dim pRow As UltraGridRow

        For Each pRow In Me.GRD_Linea.GRID.Selected.Rows
            oClipBoardGridLineas.Add(pRow.Cells("ID_Propuesta_Linea").Value)
        Next

    End Sub

    Private Sub MenuPegar_Click(sender As Object, e As EventArgs) Handles MenuPegar.Click
        Dim _LineaPare As Propuesta_Linea
        If Me.GRD_Linea.GRID.Selected.Rows.Count = 0 Then
            _LineaPare = Nothing
        Else
            _LineaPare = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(Me.GRD_Linea.GRID.Selected.Rows(0).Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
        End If
        Dim i As Integer
        For Each i In oClipBoardGridLineas
            Dim _Linea As Propuesta_Linea
            _Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = i).FirstOrDefault
            _Linea.Propuesta_Linea = _LineaPare
        Next
        oDTC.SubmitChanges()
        oClipBoardGridLineas = Nothing
        Call CargaGrid_Propuesta_Linea(oLinqPropuesta.ID_Propuesta)

    End Sub

    Private Sub MenuCambiarArticulo_Click(sender As Object, e As EventArgs) Handles MenuCambiarArticulo.Click
        Try
            Dim frm As New frmAuxiliarSeleccioLlistatGeneric
            frm.Entrada(frmAuxiliarSeleccioLlistatGeneric.EnumTipusEntrada.SeleccionarProducto)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormCambiarArticle
            frm.FormObrir(Me, False)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub AlTancarFormCambiarArticle(ByVal pID As Integer)
        Try
            Dim _pRow As UltraGridRow
            For Each _pRow In Me.GRD_Linea.GRID.Selected.Rows
                Dim _Linea As Propuesta_Linea = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(_pRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
                _Linea.Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = pID).FirstOrDefault
                _Linea.ID_Producto = _Linea.Producto.ID_Producto
                _Linea.Descripcion = _Linea.Producto.Descripcion
                _Linea.DescripcionAmpliada = _Linea.Producto.DescripcionAmpliada
                _pRow.Cells("ID_Producto").Value = _Linea.Producto.ID_Producto
                _pRow.Cells("CodigoProducto").Value = _Linea.Producto.Codigo
                _pRow.Cells("Descripcion").Value = _Linea.Producto.Descripcion
                _pRow.Cells("DescripcionAmpliada").Value = _Linea.Producto.DescripcionAmpliada
                _pRow.Update()
                oDTC.SubmitChanges()
            Next


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        'If Me.GRD_Linea.GRID.Selected.Rows.Count = 0 Then
        '    e.Cancel = True
        'End If
    End Sub

    Private Sub MenuPegarEnRaiz_Click(sender As Object, e As EventArgs) Handles MenuPegarEnRaiz.Click
        Me.GRD_Linea.GRID.ActiveRow = Nothing
        Me.GRD_Linea.GRID.Selected.Rows.Clear()
        Call MenuPegar_Click(sender, e)
    End Sub

    Private Sub GRD_Linea_M_GRID_KeyDown(ByRef sender As Object, ByRef e As KeyEventArgs) Handles GRD_Linea.M_GRID_KeyDown
        Dim OldIndex As UltraGridCell

        Select Case e.KeyCode
            Case Keys.Enter
                With Me.GRD_Linea.GRID
                    If .ActiveCell Is Nothing = True Then
                        Exit Sub
                    End If

                    If .ActiveCell.IsInEditMode = True Then

                        OldIndex = .ActiveCell
                        .PerformAction(UltraGridAction.ExitEditMode, False, False)
                        .PerformAction(UltraGridAction.BelowRow, False, False)
                        .ActiveCell = .ActiveRow.Cells(OldIndex.Column)
                        e.Handled = True
                        .PerformAction(UltraGridAction.EnterEditMode, False, False)
                    End If
                End With
        End Select

    End Sub

    Private Sub CargarCombo_Planta3(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta) = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Planta

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Planta, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_Emplazamiento_Planta").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Planta3(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDEmplazamiento As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta) = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.ID_Instalacion_Emplazamiento = pIDEmplazamiento Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Planta


            Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Planta, Var.Descripcion)
            Next

            'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Zona3(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Zona) = (From Taula In oDTC.Instalacion_Emplazamiento_Zona Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Zona

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Zona, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_Emplazamiento_Zona").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_Emplazamiento_Zona").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Zona3(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDPlanta As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Zona) = (From Taula In oDTC.Instalacion_Emplazamiento_Zona Where Taula.ID_Instalacion_Emplazamiento_Planta = pIDPlanta Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Zona

            Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Zona, Var.Descripcion)
            Next

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Emplazamiento3(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pID As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento) = (From Taula In oDTC.Instalacion_Emplazamiento Where Taula.ID_Instalacion = pID Order By Taula.Descripcion Select Taula)
            Dim Var As New Instalacion_Emplazamiento

            Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_Emplazamiento").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Instalacion_Emplazamiento").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

#End Region

#Region "GRID Lineas Vinculación energética"
    Private Sub CargarGrid_VinculacionEnergetica(ByVal pIDPropuesta As Integer)

        Dim DTS As New DataSet 'La select de sota es diferent de la de més a sota pq en la primera mirem si tots els pares tenen fills. Si no tenen fills no mostrarem les línies
        BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea_Vinculacion_Energetica Where (Select Count(*) From Propuesta_Linea Where Activo=1 and Propuesta_Linea.ID_Propuesta_Linea_Vinculado_Energetico=C_Propuesta_Linea_Vinculacion_Energetica.ID_Propuesta_Linea)>0 and Activo=1 and ID_Propuesta_Linea_Vinculado_Energetico is null and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta)
        Dim i As Integer = 0
        Dim Fin As Integer

        If oLinqPropuesta.NivelMaximoLineas.HasValue = True Then
            Fin = oLinqPropuesta.NivelMaximoLineas - 1
        Else
            Fin = 8
        End If

        For i = 0 To Fin
            BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea_Vinculacion_Energetica Where  Activo=1 and ID_Propuesta_Linea_Vinculado_Energetico is not null and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta, "Propuesta_Linea" & i + 2, i, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado_Energetico", False)
        Next

        Me.GRD_VinculacionEnergetica.M.clsUltraGrid.Cargar(DTS, 4)

    End Sub
#End Region

#Region "GRID Ruta"

    Private Sub CargaGrid_Ruta(ByVal pId As Integer)
        Try
            'If oInstalacionAnterior = True Then
            '    Me.AccessibleName = "PropuestaAnterior"
            'End If

            With Me.GRD_Ruta

                .M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where RutaOrden is not null and Activo=1 and ID_Propuesta=" & pId & " Order by RutaOrden", BD)

                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Ruta_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Ruta.M_Grid_InitializeRow
        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_Ruta.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
    End Sub

#End Region

#Region "Advertencias Intrusion"

    Private Sub CargarAdvertencias_Intrusion()
        Dim oDTC As New DTCDataContext(BD.Conexion)
        oLinqPropuestaAdvertencias = (From taula In oDTC.Propuesta Where taula.ID_Propuesta = oLinqPropuesta.ID_Propuesta Select taula).FirstOrDefault

        DTAdvertencies = New DataTable
        DTAdvertencies.Columns.Add("Norma", "System.int32".GetType)
        DTAdvertencies.Columns.Add("Mensaje", "System.string".GetType)
        DTAdvertencies.Columns.Add("Linea", "System.int32".GetType)
        DTAdvertencies.Columns.Add("Rojo", "System.boolean".GetType)

        Call MenosElementosDeteccionQueEspecificadosEnZona()
        Call ElementosInalambricosConectadosDeDiferenteFrecuencia()
        Call ElementosConGradoInferiorAlEspecificadoEnLaPropuesta()
        Call PropuestaSinElementoDeArmeYDesarme()
        Call PropuestaConExpansoresPeroCon0FuentesAlimentacion()
        Call ElementosConClaseAmbientalInferiorALaDeLaZona()
        Call ZonasConAperturasSinNingunElementoDeteccion()
        Call ProductosVinculadosDeDiferentesMarcas()
        Call ElementoInalambricoVinculadoAExpansorAlambrico()
        Call ElementoalambricoVinculadoAExpansorAlambrico()
        Call CentralSinBateria()
        Call MenosElementosDeteccionQueEspecificadosEnAberturas()
        Call AberturaSinElementoDeControlPenetracion()
        Call CentralesNoBidireccionales()
        Call ExcedidoLimiteDeExpansores()
        Call ExcedidoLimiteDeRele()
        Call ExcedidoElementosCableadosALaCentral()
        Call ExcedidoElementosInalambricosALaCentral()
        Call CapacidadElectricaExcedida()

        Me.GRD_Advertencias_Intrusion.M.clsUltraGrid.Cargar(DTAdvertencies)

        Dim pRow As UltraGridRow
        For Each pRow In Me.GRD_Advertencias_Intrusion.GRID.Rows
            If pRow.Cells("Rojo").Value = True Then
                pRow.Cells("Norma").Appearance.BackColor = Color.LightCoral
            End If
        Next
    End Sub

    Private Sub MenosElementosDeteccionQueEspecificadosEnZona()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)
            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 1).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Emplazamiento As Instalacion_Emplazamiento
            Dim _Zona As Instalacion_Emplazamiento_Zona
            For Each _Emplazamiento In oLinqPropuestaAdvertencias.Instalacion.Instalacion_Emplazamiento
                For Each _Zona In _Emplazamiento.Instalacion_Emplazamiento_Zona
                    If _Zona.Intrusion = True And _Zona.Numerico > 0 Then
                        Dim NumZonasCubiertas As Integer = (oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso (IsNothing(F.ID_Instalacion_Emplazamiento_Zona) = False AndAlso F.ID_Instalacion_Emplazamiento_Zona = _Zona.ID_Instalacion_Emplazamiento_Zona) And F.Producto.Elemento_Deteccion = True AndAlso (IsNothing(F.Producto.Numero_Zonas_Utilizadas) = False)).Sum(Function(F) F.Unidad * F.Producto.Numero_Zonas_Utilizadas).Value)
                        If NumZonasCubiertas < _Zona.Numerico Then
                            Missatge = "El número de detectores assignados a la zona (" & _Zona.Descripcion & ") es inferior a los requeridos por la zona. Hay (" & NumZonasCubiertas & ") y se requieren (" & _Zona.Numerico & ")"
                            Call CargaDataTableAdvertencies(1, Missatge, 0)
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ElementosInalambricosConectadosDeDiferenteFrecuencia()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 2).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                If _Linea.ID_Propuesta_Linea_Vinculado <> 0 And _Linea.Producto.Inalambrico = True And _Linea.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion = True Then
                    Dim _LineaVinculada As Propuesta_Linea
                    _LineaVinculada = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado).FirstOrDefault
                    If _LineaVinculada.Producto.Inalambrico = True Then
                        If _Linea.Producto.ID_Producto_FrecuenciaInalambrica <> _LineaVinculada.Producto.ID_Producto_FrecuenciaInalambrica Then
                            Missatge = "La línea de propuesta (" & _Linea.Identificador & ") vinculada a la línea de propuesta (" & _LineaVinculada.Identificador & ") son dispositivos inalámbricos con diferente frecuencia inalámbrica."
                            Call CargaDataTableAdvertencies(2, Missatge, _Linea.ID_Propuesta_Linea)
                        End If
                    End If

                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ElementosConGradoInferiorAlEspecificadoEnLaPropuesta()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 3).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            If oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = oLinqPropuesta.ID_Propuesta).FirstOrDefault.ID_Producto_Grado = GradoNoTenerEnCuenta Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso _Linea.Producto.ID_Producto_Grado <> GradoNoTenerEnCuenta And _Linea.Producto.ID_Producto_Grado < oLinqPropuestaAdvertencias.ID_Producto_Grado Then
                    Missatge = "La línea de propuesta (" & _Linea.Identificador & ") que contiene el producto: (" & _Linea.Producto.Descripcion & ") tiene un grado inferior al de la propuesta."
                    Call CargaDataTableAdvertencies(3, Missatge, _Linea.ID_Propuesta_Linea, True)
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub PropuestaSinElementoDeArmeYDesarme()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 4).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            If oLinqPropuesta.ID_Producto_Grado = 2 Or oLinqPropuesta.ID_Producto_Grado = 3 Or oLinqPropuesta.ID_Producto_Grado = 4 Then
                Dim Missatge As String = ""
                If oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso F.Producto.Elemento_arme_desarme = True).Count = 0 Then
                    Missatge = "La propuesta no tiene ningún elemento de arme y desarme."
                    Call CargaDataTableAdvertencies(4, Missatge, 0, True)
                End If
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub PropuestaConExpansoresPeroCon0FuentesAlimentacion()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 5).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""
            If oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso F.Producto.Fuente_Alimentacion = True).Count = 0 And oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.Expansor = True).Count > 0 Then
                Missatge = "Hay expansores en la propuesta pero no hay ninguna fuente de alimentación"
                Call CargaDataTableAdvertencies(5, Missatge, 0)
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ElementosConClaseAmbientalInferiorALaDeLaZona()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 6).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            If oLinqPropuesta.ID_Producto_Grado = 2 Or oLinqPropuesta.ID_Producto_Grado = 3 Or oLinqPropuesta.ID_Producto_Grado = 4 Then
                Dim Missatge As String = ""

                Dim _Linea As Propuesta_Linea
                For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                    'Si la línea te assignat una zona // Si la zona te una clase ambiental de no tener encuenta o el article de la línea te una clase ambiental de no tenir en compte// si la clase ambiental del article es inferior al especificat en la zona llavors error
                    If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso IsNothing(_Linea.ID_Instalacion_Emplazamiento_Zona) = False AndAlso _Linea.Instalacion_Emplazamiento_Zona.Intrusion = True Then
                        If _Linea.Producto.ID_Producto_Clase_Ambiental <> ClaseAmbientalNoTenerEnCuenta And _Linea.Instalacion_Emplazamiento_Zona.ID_Producto_ClaseAmbiental <> 5 Then
                            If _Linea.Producto.ID_Producto_Clase_Ambiental < _Linea.Instalacion_Emplazamiento_Zona.ID_Producto_ClaseAmbiental = True Then
                                Missatge = "La línea de propuesta (" & _Linea.Identificador & ") tiene una clase ambiental inferior a la de la zona."
                                Call CargaDataTableAdvertencies(6, Missatge, _Linea.ID_Propuesta_Linea, True)
                            End If
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ZonasConAperturasSinNingunElementoDeteccion()
        Dim oDTC As New DTCDataContext(BD.Conexion)
        If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 7).FirstOrDefault.Activo = False Then
            Exit Sub
        End If

        Dim Missatge As String = ""
        If oLinqPropuestaAdvertencias Is Nothing = True Then
            Exit Sub
        End If

        If oLinqPropuestaAdvertencias.ID_Producto_Grado = 3 Or oLinqPropuestaAdvertencias.ID_Producto_Grado = 4 Then
            Dim _Emplazamiento As Instalacion_Emplazamiento
            Dim _Zona As Instalacion_Emplazamiento_Zona
            For Each _Emplazamiento In oLinqPropuestaAdvertencias.Instalacion.Instalacion_Emplazamiento
                For Each _Zona In _Emplazamiento.Instalacion_Emplazamiento_Zona
                    If _Zona.Intrusion = True AndAlso _Zona.Instalacion_Emplazamiento_Abertura.Sum(Function(F) F.Numerico) > 0 Then
                        Dim NumElementosDeteccionEnZona As Integer = (oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso (IsNothing(F.ID_Instalacion_Emplazamiento_Zona) = False AndAlso F.ID_Instalacion_Emplazamiento_Zona = _Zona.ID_Instalacion_Emplazamiento_Zona) And F.Producto.Elemento_Deteccion = True AndAlso (IsDBNull(F.Producto.Numero_Zonas_Utilizadas) = False)).Sum(Function(F) F.Unidad * F.Producto.Numero_Zonas_Utilizadas).Value)

                        If NumElementosDeteccionEnZona = 0 Then
                            Missatge = "El número de detectores assignados a la zona (" & _Zona.Descripcion & ") es inferior a los requeridos por el grado de la instalacion."
                            Call CargaDataTableAdvertencies(7, Missatge, 0, True)
                        End If
                    End If
                Next
            Next
        End If
    End Sub

    Private Sub ProductosVinculadosDeDiferentesMarcas()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 8).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso _Linea.ID_Propuesta_Linea_Vinculado <> 0 And _Linea.Producto.MarcaEspecificada = True Then
                    Dim _LineaVinculada As Propuesta_Linea
                    _LineaVinculada = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado).FirstOrDefault
                    If _LineaVinculada.Producto.ID_Producto_Marca <> _Linea.Producto.ID_Producto_Marca Then
                        Missatge = "La línea de propuesta (" & _Linea.Identificador & ") vinculada a la línea de propuesta (" & _LineaVinculada.Identificador & ") no son compatibles entre ellas."
                        Call CargaDataTableAdvertencies(8, Missatge, _Linea.ID_Propuesta_Linea)
                    End If

                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ElementoInalambricoVinculadoAExpansorAlambrico()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 9).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso _Linea.ID_Propuesta_Linea_Vinculado <> 0 And _Linea.Producto.Inalambrico = True Then
                    Dim _LineaVinculada As Propuesta_Linea
                    _LineaVinculada = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado).FirstOrDefault
                    If _LineaVinculada.Producto.Inalambrico = False And _LineaVinculada.Producto.Expansor = True Then
                        Missatge = "La línea de propuesta (" & _Linea.Identificador & ") vinculada a la línea de propuesta (" & _LineaVinculada.Identificador & ") es un dispositivo inalámbrico conectado a un expansor cableado."
                        Call CargaDataTableAdvertencies(9, Missatge, _Linea.ID_Propuesta_Linea)
                    End If
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ElementoalambricoVinculadoAExpansorAlambrico()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 9).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso _Linea.ID_Propuesta_Linea_Vinculado <> 0 And _Linea.Producto.Inalambrico = False Then
                    Dim _LineaVinculada As Propuesta_Linea
                    _LineaVinculada = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado).FirstOrDefault
                    If _LineaVinculada.Producto.Inalambrico = True And _LineaVinculada.Producto.Expansor = True Then
                        Missatge = "La línea de propuesta (" & _Linea.Identificador & ") vinculada a la línea de propuesta (" & _LineaVinculada.Identificador & ") es un dispositivo cableado conectado a un expansor inalámbrico."
                        Call CargaDataTableAdvertencies(10, Missatge, _Linea.ID_Propuesta_Linea)
                    End If
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CentralSinBateria()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 11).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            If oLinqPropuesta.ID_Producto_Grado = 2 Or oLinqPropuesta.ID_Producto_Grado = 3 Or oLinqPropuesta.ID_Producto_Grado = 4 Then
                Dim Missatge As String = ""

                Dim _Linea As Propuesta_Linea
                For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                    If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso _Linea.Producto.Central = True And _Linea.Producto.Baterias = False Then
                        If oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea And F.Producto.Baterias = True).Count = 0 Then
                            Missatge = "La línea de propuesta (" & _Linea.Identificador & ") no tiene ninguna batería."
                            Call CargaDataTableAdvertencies(11, Missatge, _Linea.ID_Propuesta_Linea, True)
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub MenosElementosDeteccionQueEspecificadosEnAberturas()
        Dim oDTC As New DTCDataContext(BD.Conexion)
        If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 12).FirstOrDefault.Activo = False Then
            Exit Sub
        End If

        If oLinqPropuesta.ID_Producto_Grado = 2 Or oLinqPropuesta.ID_Producto_Grado = 3 Or oLinqPropuesta.ID_Producto_Grado = 4 Then
            Dim Missatge As String = ""

            Dim _Emplazamiento As Instalacion_Emplazamiento
            Dim _Zona As Instalacion_Emplazamiento_Zona
            Dim _Abertura As Instalacion_Emplazamiento_Abertura
            For Each _Emplazamiento In oLinqPropuestaAdvertencias.Instalacion.Instalacion_Emplazamiento
                For Each _Zona In _Emplazamiento.Instalacion_Emplazamiento_Zona
                    For Each _Abertura In _Zona.Instalacion_Emplazamiento_Abertura
                        If _Abertura.Numerico > 0 And _Abertura.Intrusion = True Then
                            Dim NumAberturasCubiertas As Integer = (oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso (IsNothing(F.ID_Instalacion_Emplazamiento_Abertura) = False AndAlso F.ID_Instalacion_Emplazamiento_Abertura = _Abertura.ID_Instalacion_Emplazamiento_Abertura) And F.Producto.Elemento_Deteccion = True AndAlso (IsNothing(F.Producto.Numero_Aberturas) = False)).Sum(Function(F) F.Unidad * F.Producto.Numero_Aberturas).Value)
                            If NumAberturasCubiertas < _Abertura.Numerico Then
                                Missatge = "El número de detectores assignados (" & NumAberturasCubiertas & ") a la abertura (" & _Abertura.Identificador & " - " & _Abertura.Instalacion_Emplazamiento_Abertura_Elemento.Descripcion & ") de la zona (" & _Abertura.Instalacion_Emplazamiento_Zona.Descripcion & ") de la planta (" & _Abertura.Instalacion_Emplazamiento_Zona.Instalacion_Emplazamiento_Planta.Descripcion & ") de la ubicación (" & _Abertura.Instalacion_Emplazamiento_Zona.Instalacion_Emplazamiento_Planta.Instalacion_Emplazamiento.Descripcion & ") es inferior a los requeridos por la abertura (" & _Abertura.Numerico & ")."
                                Call CargaDataTableAdvertencies(12, Missatge, 0, True)
                            End If
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Private Sub AberturaSinElementoDeControlPenetracion()
        Dim oDTC As New DTCDataContext(BD.Conexion)
        If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 13).FirstOrDefault.Activo = False Then
            Exit Sub
        End If

        If oLinqPropuesta.ID_Producto_Grado = 3 Or oLinqPropuesta.ID_Producto_Grado = 4 Then
            Dim Missatge As String = ""

            Dim _Emplazamiento As Instalacion_Emplazamiento
            Dim _Zona As Instalacion_Emplazamiento_Zona
            Dim _Abertura As Instalacion_Emplazamiento_Abertura
            For Each _Emplazamiento In oLinqPropuestaAdvertencias.Instalacion.Instalacion_Emplazamiento
                For Each _Zona In _Emplazamiento.Instalacion_Emplazamiento_Zona
                    For Each _Abertura In _Zona.Instalacion_Emplazamiento_Abertura
                        If (oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso IsNothing(F.ID_Instalacion_Emplazamiento_Abertura) = False AndAlso F.ID_Instalacion_Emplazamiento_Abertura = _Abertura.ID_Instalacion_Emplazamiento_Abertura) And F.Producto.ControlPenetracion = True AndAlso (IsNothing(F.Producto.Numero_Zonas_Utilizadas) = False And F.Producto.Numero_Zonas_Utilizadas > 0))).Count = 0 Then
                            Missatge = "La abertura (" & _Abertura.Identificador & " - " & _Abertura.Instalacion_Emplazamiento_Abertura_Elemento.Descripcion & ") de la zona (" & _Abertura.Instalacion_Emplazamiento_Zona.Descripcion & ") de la planta (" & _Abertura.Instalacion_Emplazamiento_Zona.Instalacion_Emplazamiento_Planta.Descripcion & ") de la ubicación (" & _Abertura.Instalacion_Emplazamiento_Zona.Instalacion_Emplazamiento_Planta.Instalacion_Emplazamiento.Descripcion & ") no tiene ningún elemento de control de penetración)."
                            Call CargaDataTableAdvertencies(13, Missatge, 0, True)

                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Private Sub CentralesNoBidireccionales()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 14).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            If oLinqPropuesta.ID_Producto_Grado = 2 Or oLinqPropuesta.ID_Producto_Grado = 3 Or oLinqPropuesta.ID_Producto_Grado = 4 Then
                Dim Missatge As String = ""
                Dim _Linea As Propuesta_Linea
                For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                    If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso _Linea.Producto.Central = True And _Linea.Producto.Bidirecciona = False Then
                        Missatge = "La línea de propuesta (" & _Linea.Identificador & ") és una central no bidireccional."
                        Call CargaDataTableAdvertencies(14, Missatge, _Linea.ID_Propuesta_Linea, True)
                    End If
                Next
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ExcedidoLimiteDeExpansores()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 15).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso F.Producto.Expansor = True)
                ' Dim _LineaVinculada As Propuesta_Linea
                Dim SumaZonasUtilizadas As Integer
                'oDTC.Propuesta_Linea.Where(Function(F) (IsNothing(F.ID_Propuesta_Linea) = False AndAlso F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado) AndAlso IsDBNull(F.Producto.Numero_Zonas_Utilizadas) = vbNull).Sum(Function(F) F.Unidad * F.Producto.Numero_Zonas_Utilizadas).Value


                'Dim NumAberturasCubiertas As Integer = (oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (IsNothing(F.ID_Instalacion_Emplazamiento_Abertura) = False AndAlso F.ID_Instalacion_Emplazamiento_Abertura = _Abertura.ID_Instalacion_Emplazamiento_Abertura) And F.Producto.Elemento_Deteccion = True AndAlso (IsNothing(F.Producto.Numero_Aberturas) = False)).Sum(Function(F) F.Unidad * F.Producto.Numero_Aberturas).Value)
                SumaZonasUtilizadas = oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso IsNothing(F.ID_Propuesta_Linea_Vinculado) = False AndAlso F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea)).Sum(Function(F) F.Unidad * F.Producto.Numero_Zonas_Utilizadas).Value
                If SumaZonasUtilizadas > (_Linea.Producto.Expansor_Num_Elementos * _Linea.Unidad) Then
                    Missatge = "El expansor de la línea de propuesta (" & _Linea.Identificador & ") tiene (" & SumaZonasUtilizadas & ") elementos vinculados y sólo soporta (" & _Linea.Producto.Expansor_Num_Elementos * _Linea.Unidad & ")."
                    Call CargaDataTableAdvertencies(15, Missatge, _Linea.ID_Propuesta_Linea)
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ExcedidoLimiteDeRele()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 16).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso F.Producto.Modulo_Rele = True)
                ' Dim _LineaVinculada As Propuesta_Linea
                Dim SumaZonasUtilizadas As Integer
                'oDTC.Propuesta_Linea.Where(Function(F) (IsNothing(F.ID_Propuesta_Linea) = False AndAlso F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado) AndAlso IsDBNull(F.Producto.Numero_Zonas_Utilizadas) = vbNull).Sum(Function(F) F.Unidad * F.Producto.Numero_Zonas_Utilizadas).Value


                'Dim NumAberturasCubiertas As Integer = (oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (IsNothing(F.ID_Instalacion_Emplazamiento_Abertura) = False AndAlso F.ID_Instalacion_Emplazamiento_Abertura = _Abertura.ID_Instalacion_Emplazamiento_Abertura) And F.Producto.Elemento_Deteccion = True AndAlso (IsNothing(F.Producto.Numero_Aberturas) = False)).Sum(Function(F) F.Unidad * F.Producto.Numero_Aberturas).Value)
                SumaZonasUtilizadas = oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso (IsNothing(F.ID_Propuesta_Linea_Vinculado) = False AndAlso F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea)).Sum(Function(F) F.Unidad * F.Producto.Numero_Zonas_Utilizadas).Value
                If SumaZonasUtilizadas > (_Linea.Producto.Modulo_Rele_Num_Elementos * _Linea.Unidad) Then
                    Missatge = "El módulo relé de la línea de propuesta (" & _Linea.Identificador & ") tiene (" & SumaZonasUtilizadas & ") elementos vinculados y sólo soporta (" & _Linea.Producto.Modulo_Rele_Num_Elementos * _Linea.Unidad & ")."
                    Call CargaDataTableAdvertencies(16, Missatge, _Linea.ID_Propuesta_Linea)
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ExcedidoElementosCableadosALaCentral()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 17).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso F.Producto.Central = True And F.Producto.Inalambrico = False)
                Dim SumaZonasUtilizadas As Integer
                SumaZonasUtilizadas = oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso (IsNothing(F.ID_Propuesta_Linea_Vinculado) = False AndAlso F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea) And F.Producto.Inalambrico = False And F.Producto.Elemento_Deteccion = True).Sum(Function(F) F.Unidad * F.Producto.Numero_Zonas_Utilizadas).Value
                If SumaZonasUtilizadas > (_Linea.Producto.Central_Num_Zonas_Placa * _Linea.Unidad) Then
                    Missatge = "La central de la línea de la propuesta (" & _Linea.Identificador & ") tiene (" & SumaZonasUtilizadas & ") elementos vinculados cableados y sólo soporta (" & _Linea.Producto.Central_Num_Zonas_Placa * _Linea.Unidad & ")."
                    Call CargaDataTableAdvertencies(17, Missatge, _Linea.ID_Propuesta_Linea)
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ExcedidoElementosInalambricosALaCentral()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 18).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso F.Producto.Central = True And F.Producto.Inalambrico = True)
                Dim SumaZonasUtilizadas As Integer
                SumaZonasUtilizadas = oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso (IsNothing(F.ID_Propuesta_Linea_Vinculado) = False AndAlso F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea) And F.Producto.Inalambrico = True And F.Producto.Elemento_Deteccion = True).Sum(Function(F) F.Unidad * F.Producto.Numero_Zonas_Utilizadas).Value
                If SumaZonasUtilizadas > (_Linea.Producto.Central_Num_Zonas_Inalambricas_Placa * _Linea.Unidad) Then
                    Missatge = "La central de la línea de la propuesta (" & _Linea.Identificador & ") tiene (" & SumaZonasUtilizadas & ") elementos vinculados inalámbricos y sólo soporta (" & _Linea.Producto.Central_Num_Zonas_Inalambricas_Placa * _Linea.Unidad & ")."
                    Call CargaDataTableAdvertencies(18, Missatge, _Linea.ID_Propuesta_Linea)
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CapacidadElectricaExcedida()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 28).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            'If oLinqPropuesta.ID_Producto_Grado = 2 Or oLinqPropuesta.ID_Producto_Grado = 3 Or oLinqPropuesta.ID_Producto_Grado = 4 Then
            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                Dim PotenciatPare As Decimal = IIf(_Linea.Producto.PotenciaSalida.HasValue, _Linea.Producto.PotenciaSalida * _Linea.Unidad, 0)
                Dim PotenciaFills As Decimal

                Dim _Llista = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea And F.Activo = True And F.Producto.PotenciaEntrada.HasValue)
                If _Llista.Count <> 0 Then
                    PotenciaFills = _Llista.Sum(Function(F) F.Producto.PotenciaEntrada * F.Unidad)
                Else
                    PotenciaFills = 0
                End If


                If PotenciatPare < PotenciaFills Then
                    Missatge = "La línea de propuesta (" & _Linea.Identificador & ") tiene menos capacidad electrica que la suma de los productos conectados a ella."
                    Call CargaDataTableAdvertencies(28, Missatge, _Linea.ID_Propuesta_Linea, False)
                End If
            Next
            ' End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub


    Private Sub CargaDataTableAdvertencies(ByVal pNorma As Integer, ByVal pMensaje As String, ByVal pIDLinea As Integer, Optional ByVal pRojo As Boolean = False)
        Try
            Dim DTRow As DataRow
            DTRow = DTAdvertencies.NewRow
            DTRow("Norma") = pNorma
            DTRow("Mensaje") = pMensaje
            DTRow("Linea") = pIDLinea
            DTRow("Rojo") = pRojo
            DTAdvertencies.Rows.Add(DTRow)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub


#End Region

#Region "Advertencias CCTV"

    Private Sub CargarAdvertencias_CCTV()
        Dim oDTC As New DTCDataContext(BD.Conexion)
        oLinqPropuestaAdvertencias = (From taula In oDTC.Propuesta Where taula.ID_Propuesta = oLinqPropuesta.ID_Propuesta Select taula).First

        DTAdvertencies = New DataTable
        DTAdvertencies.Columns.Add("Norma", "System.int32".GetType)
        DTAdvertencies.Columns.Add("Mensaje", "System.string".GetType)
        DTAdvertencies.Columns.Add("Linea", "System.int32".GetType)
        DTAdvertencies.Columns.Add("Rojo", "System.boolean".GetType)

        Call N20_ElementoDeCaptacionSinOpticaYSinNingunElementoVinculadoConOptica()
        Call N21_ElementoDeCaptacionSinOpticaConElementoVinculadoConOpticaPeroDiferenteTipoRosca()
        Call N22_ElementoDeCaptacionSinFuenteAlimentacionOConOpticaVinculada()
        Call N8_ProductosVinculadosDeDiferentesMarcas()
        Call N23_ElementoDeCaptacionConConexionUTPVinculadoAOtroElementoSinConexionUTP()
        Call N24_ElementoDeCaptacionConConexionUTPVinculadoAOtroElementoSinConexionBNC()
        Call N25_ElementoPTZVinculadoAElmentoNoPTZ()
        Call N26_ElementoDeGrabacionSinDiscoDuroVinculado()
        Call N27_ElementoDeCaptacionConLuminosidadInferiorALaDeLaZona()

        Me.GRD_Advertencias_CCTV.M.clsUltraGrid.Cargar(DTAdvertencies)


        Dim pRow As UltraGridRow
        For Each pRow In Me.GRD_Advertencias_CCTV.GRID.Rows
            If pRow.Cells("Rojo").Value = True Then
                pRow.Cells("Norma").Appearance.BackColor = Color.LightCoral
            End If
        Next
    End Sub

    Private Sub N20_ElementoDeCaptacionSinOpticaYSinNingunElementoVinculadoConOptica()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 20).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.CCTV And F.Producto.CCTV_ElementoCaptacion = True And F.Producto.CCTV_Optica = False)
                If oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Vinculado.HasValue = True AndAlso F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea) And F.Producto.CCTV_Optica = True).Count = 0 Then
                    Missatge = "El elemento de captación (" & _Linea.Identificador & ") necesita una óptica para poder funcionar."
                    Call CargaDataTableAdvertencies(20, Missatge, _Linea.ID_Propuesta_Linea)
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub N21_ElementoDeCaptacionSinOpticaConElementoVinculadoConOpticaPeroDiferenteTipoRosca()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 21).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.CCTV And F.Producto.CCTV_ElementoCaptacion = True And F.Producto.CCTV_Optica = False)
                Dim _LineaVinculada As Propuesta_Linea = oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Vinculado.HasValue = True AndAlso F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea) And F.Producto.CCTV_Optica = True).FirstOrDefault()
                If _LineaVinculada Is Nothing = False AndAlso _LineaVinculada.Producto.ID_Producto_TipoRosca <> _Linea.Producto.ID_Producto_TipoRosca Then
                    Missatge = "La línea de propuesta (" & _LineaVinculada.Identificador & ") es una optica con diferente tipo de rosca que la línea (" & _Linea.Identificador & ")."
                    Call CargaDataTableAdvertencies(21, Missatge, _LineaVinculada.ID_Propuesta_Linea)
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub N22_ElementoDeCaptacionSinFuenteAlimentacionOConOpticaVinculada()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 22).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.CCTV And F.Producto.CCTV_ElementoCaptacion = True And F.Producto.CCTV_RequiereAlimentacion = True)
                If oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (F.Producto.ID_Producto_Division = EnumProductoDivision.CCTV And F.Producto.CCTV_FuenteAlimentacion = True And F.Producto.ID_Producto_Voltaje = _Linea.Producto.ID_Producto_Voltaje)).Count = 0 Then
                    Missatge = "El elemento de captacion (" & _Linea.Identificador & ") no está correctamente alimentado."
                    Call CargaDataTableAdvertencies(22, Missatge, _Linea.ID_Propuesta_Linea)
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub N23_ElementoDeCaptacionConConexionUTPVinculadoAOtroElementoSinConexionUTP()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 23).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.CCTV AndAlso _Linea.ID_Propuesta_Linea_Vinculado <> 0 And _Linea.Producto.CCTV_ConexionUTP = True Then
                    Dim _LineaVinculada As Propuesta_Linea
                    _LineaVinculada = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado).FirstOrDefault
                    If _LineaVinculada.Producto.CCTV_ConexionUTP = False And (_LineaVinculada.Producto.CCTV_ServidorVideoBNC = True Or _LineaVinculada.Producto.CCTV_ElementoGrabacion = True) Then
                        Missatge = "La línea de propuesta (" & _Linea.Identificador & ") vinculada a la línea de propuesta (" & _LineaVinculada.Identificador & ") tienen distinto formato de conexión."
                        Call CargaDataTableAdvertencies(23, Missatge, _Linea.ID_Propuesta_Linea)
                    End If
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub N24_ElementoDeCaptacionConConexionUTPVinculadoAOtroElementoSinConexionBNC()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 24).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.CCTV AndAlso _Linea.ID_Propuesta_Linea_Vinculado <> 0 And _Linea.Producto.CCTV_ConexionBNC = True Then
                    Dim _LineaVinculada As Propuesta_Linea
                    _LineaVinculada = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado).FirstOrDefault
                    If _LineaVinculada.Producto.CCTV_ConexionBNC = False And (_LineaVinculada.Producto.CCTV_ServidorVideoBNC = True Or _LineaVinculada.Producto.CCTV_ElementoGrabacion = True) Then
                        Missatge = "La línea de propuesta (" & _Linea.Identificador & ") vinculada a la línea de propuesta (" & _LineaVinculada.Identificador & ") tienen distinto formato de conexión."
                        Call CargaDataTableAdvertencies(24, Missatge, _Linea.ID_Propuesta_Linea)
                    End If
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub N25_ElementoPTZVinculadoAElmentoNoPTZ()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 25).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.CCTV AndAlso _Linea.ID_Propuesta_Linea_Vinculado <> 0 And _Linea.Producto.CCTV_PTZ = True Then
                    Dim _LineaVinculada As Propuesta_Linea
                    _LineaVinculada = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado).FirstOrDefault
                    If _LineaVinculada.Producto.CCTV_PTZ = False Then
                        Missatge = "La línea de propuesta (" & _Linea.Identificador & ") vinculada a la línea de propuesta (" & _LineaVinculada.Identificador & ") no tiene conexión PTZ."
                        Call CargaDataTableAdvertencies(25, Missatge, _Linea.ID_Propuesta_Linea)
                    End If
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub N26_ElementoDeGrabacionSinDiscoDuroVinculado()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 26).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (F.Producto.ID_Producto_Division = EnumProductoDivision.CCTV And F.Producto.CCTV_ElementoGrabacion = True And F.Producto.CCTV_NoIncluyeDispositivoAlmacenamiento = True))
                If oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Vinculado.HasValue = True AndAlso F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea) And F.Producto.CCTV_DiscoDuro = True).Count = 0 Then
                    Missatge = "La línea de propuesta (" & _Linea.Identificador & ") no tiene asignado ningún disco duro"
                    Call CargaDataTableAdvertencies(26, Missatge, _Linea.ID_Propuesta_Linea)
                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub N8_ProductosVinculadosDeDiferentesMarcas()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 8).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""

            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea
                If _Linea.Producto.ID_Producto_Division = EnumProductoDivision.CCTV AndAlso _Linea.ID_Propuesta_Linea_Vinculado <> 0 And _Linea.Producto.MarcaEspecificada = True Then
                    Dim _LineaVinculada As Propuesta_Linea
                    _LineaVinculada = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Linea.ID_Propuesta_Linea_Vinculado).FirstOrDefault
                    If _LineaVinculada.Producto.ID_Producto_Marca <> _Linea.Producto.ID_Producto_Marca Then
                        Missatge = "La línea de propuesta (" & _Linea.Identificador & ") vinculada a la línea de propuesta (" & _LineaVinculada.Identificador & ") no son compatibles entre ellas."
                        Call CargaDataTableAdvertencies_CCTV(8, Missatge, _Linea.ID_Propuesta_Linea)
                    End If

                End If
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub N27_ElementoDeCaptacionConLuminosidadInferiorALaDeLaZona()
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 27).FirstOrDefault.Activo = False Then
                Exit Sub
            End If

            Dim Missatge As String = ""


            Dim _Linea As Propuesta_Linea
            For Each _Linea In oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) (F.ID_Instalacion_Emplazamiento_Zona.HasValue = True AndAlso F.Instalacion_Emplazamiento_Zona.CCTV = True) And F.Producto.ID_Producto_Division = EnumProductoDivision.CCTV And F.Producto.CCTV_ElementoCaptacion = True AndAlso (F.Producto.ID_Producto_Luminosidad <> 0 And F.Instalacion_Emplazamiento_Zona.ID_Producto_Luminosidad <> 0 And F.Producto.ID_Producto_Luminosidad < F.Instalacion_Emplazamiento_Zona.ID_Producto_Luminosidad))
                Missatge = "La línea de propuesta (" & _Linea.Identificador & ") tiene una luminosidad inferior a la zona asignada"
                Call CargaDataTableAdvertencies(27, Missatge, _Linea.ID_Propuesta_Linea, False)
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub


    'Private Sub MenosElementosDeteccionQueEspecificadosEnZona()
    '    Try
    '        Dim oDTC As New DTCDataContext(BD.Conexion)
    '        If oDTC.Restriccion.Where(Function(F) F.ID_Restriccion = 1).FirstOrDefault.Activo = False Then
    '            Exit Sub
    '        End If

    '        Dim Missatge As String = ""

    '        Dim _Emplazamiento As Instalacion_Emplazamiento
    '        Dim _Zona As Instalacion_Emplazamiento_Zona
    '        For Each _Emplazamiento In oLinqPropuestaAdvertencias.Instalacion.Instalacion_Emplazamiento
    '            For Each _Zona In _Emplazamiento.Instalacion_Emplazamiento_Zona
    '                If _Zona.Intrusion = True And _Zona.Numerico > 0 Then
    '                    Dim NumZonasCubiertas As Integer = (oLinqPropuestaAdvertencias.Propuesta_Linea.Where(Function(F) F.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion AndAlso (IsNothing(F.ID_Instalacion_Emplazamiento_Zona) = False AndAlso F.ID_Instalacion_Emplazamiento_Zona = _Zona.ID_Instalacion_Emplazamiento_Zona) And F.Producto.Elemento_Deteccion = True AndAlso (IsNothing(F.Producto.Numero_Zonas_Utilizadas) = False)).Sum(Function(F) F.Unidad * F.Producto.Numero_Zonas_Utilizadas).Value)
    '                    If NumZonasCubiertas < _Zona.Numerico Then
    '                        Missatge = "El número de detectores assignados a la zona (" & _Zona.Descripcion & ") es inferior a los requeridos por la zona. Hay (" & NumZonasCubiertas & ") y se requieren (" & _Zona.Numerico & ")"
    '                        Call CargaDataTableAdvertencies(1, Missatge, 0)
    '                    End If
    '                End If
    '            Next
    '        Next

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub CargaDataTableAdvertencies_CCTV(ByVal pNorma As Integer, ByVal pMensaje As String, ByVal pIDLinea As Integer, Optional ByVal pRojo As Boolean = False)
        Try
            Dim DTRow As DataRow
            DTRow = DTAdvertencies.NewRow
            DTRow("Norma") = pNorma
            DTRow("Mensaje") = pMensaje
            DTRow("Linea") = pIDLinea
            DTRow("Rojo") = pRojo
            DTAdvertencies.Rows.Add(DTRow)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Planos"

    Private Sub CargaGrid_Planos(ByVal pId As Integer)
        Try

            With Me.GRD_Planos

                Dim _Planos As IEnumerable(Of Propuesta_Plano) = From taula In oDTC.Propuesta_Plano Where taula.ID_Propuesta = pId Select taula

                '.GRID.DataSource = _Planos
                .M.clsUltraGrid.CargarIEnumerable(_Planos)
                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("FechaCreacion").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Planta(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Zona(.GRID, 0, oLinqInstalacion.ID_Instalacion)
            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Planos_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Planos.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "Instalacion_Emplazamiento_Zona"
                Call CargarCombo_Zona(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value)
            Case "Instalacion_Emplazamiento_Planta"
                Call CargarCombo_Planta(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento").Value)
        End Select
    End Sub

    Private Sub GRD_Planos_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Planos.M_GRID_CellListSelect
        Try
            If CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue Is Nothing Then
                e.Cell.ValueList = Nothing
            End If

            Select Case e.Cell.Column.Key
                Case "Instalacion_Emplazamiento_Planta"
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Zona").Value = Nothing
                Case "Instalacion_Emplazamiento"
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Planta").Value = Nothing
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Zona").Value = Nothing
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Planos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Planos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Planos
                If oLinqPropuesta.ID_Propuesta = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaCreacion").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta").Value = oLinqPropuesta

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Planos_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Planos.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then

                Dim _Plano As Propuesta_Plano = e.ListObject
                Dim _Elementos As Propuesta_Plano_ElementosIntroducidos
                For Each _Elementos In _Plano.Propuesta_Plano_ElementosIntroducidos
                    oDTC.Propuesta_Plano_ElementosIntroducidos.DeleteOnSubmit(_Elementos)
                Next
                _Plano.Propuesta_Plano_ElementosIntroducidos.Clear()

                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Planos_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Planos.M_ToolGrid_ToolClickBotonsExtras
        If Me.GRD_Planos.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If
        Select Case e.Tool.Key
            Case "Plano"
                Dim pepe As New frmPlano(Principal)
                pepe.Entrada(oDTC, Me.GRD_Planos.GRID.ActiveRow.ListObject)
                pepe.FormObrir(Me, False)
                AddHandler pepe.AlTancarForm, AddressOf AlTancarFormPlanos
                pepe.MdiParent = Principal
                pepe.Show()

            Case "Duplicar"
                Dim _Plano As Propuesta_Plano = Me.GRD_Planos.GRID.ActiveRow.ListObject

                Dim _NewPlano As New Propuesta_Plano
                With _NewPlano
                    .Descripcion = _Plano.Descripcion
                    .FechaCreacion = _Plano.FechaCreacion
                    .Validado = _Plano.Validado

                    .ID_Instalacion_Emplazamiento = _Plano.ID_Instalacion_Emplazamiento
                    .ID_Instalacion_Emplazamiento_Planta = _Plano.ID_Instalacion_Emplazamiento_Planta
                    .ID_Instalacion_Emplazamiento_Zona = _Plano.ID_Instalacion_Emplazamiento_Zona

                    If IsNothing(_Plano.ID_PlanoBinario) = False Then
                        Dim _PlanoBinario As New PlanoBinario
                        _PlanoBinario.Fichero = _Plano.PlanoBinario.Fichero
                        _PlanoBinario.Foto = _Plano.PlanoBinario.Foto
                        .PlanoBinario = _PlanoBinario
                        oDTC.PlanoBinario.InsertOnSubmit(_PlanoBinario)
                    End If
                End With
                _NewPlano.Propuesta = oLinqPropuesta
                oDTC.SubmitChanges()
                Call CargaGrid_Planos(oLinqPropuesta.ID_Propuesta)

        End Select

    End Sub

    Private Sub GRD_Planos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Planos.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub AlTancarFormPlanos(ByRef pID As String)
        Call CargaGrid_Planos(oLinqPropuesta.ID_Propuesta)
    End Sub

    Private Sub CargarCombo_Planta(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta) = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Planta

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Planta").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Planta(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDEmplazamiento As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta) = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.ID_Instalacion_Emplazamiento = pIDEmplazamiento Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Planta


            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Zona(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Zona) = (From Taula In oDTC.Instalacion_Emplazamiento_Zona Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Zona

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Zona").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Zona").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Zona(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDPlanta As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Zona) = (From Taula In oDTC.Instalacion_Emplazamiento_Zona Where Taula.ID_Instalacion_Emplazamiento_Planta = pIDPlanta Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Zona

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Emplazamiento(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pID As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento) = (From Taula In oDTC.Instalacion_Emplazamiento Where Taula.ID_Instalacion = pID Order By Taula.Descripcion Select Taula)
            Dim Var As New Instalacion_Emplazamiento

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next


            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

#End Region

#Region "Grid Diagramas"

    Private Sub CargaGrid_Diagramas(ByVal pId As Integer)
        Try

            With Me.GRD_Diagrama

                Dim _Diagramas As IEnumerable(Of Propuesta_Diagrama) = From taula In oDTC.Propuesta_Diagrama Where taula.ID_Propuesta = pId Select taula

                .M.clsUltraGrid.CargarIEnumerable(_Diagramas)
                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("FechaCreacion").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Planta(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Zona(.GRID, 0, oLinqInstalacion.ID_Instalacion)
            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Diagrama_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Diagrama.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "Instalacion_Emplazamiento_Zona"
                Call CargarCombo_Zona(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value)
            Case "Instalacion_Emplazamiento_Planta"
                Call CargarCombo_Planta(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento").Value)
        End Select
    End Sub

    Private Sub GRD_Diagrama_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Diagrama.M_GRID_CellListSelect
        Try
            If CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue Is Nothing Then
                e.Cell.ValueList = Nothing
            End If

            Select Case e.Cell.Column.Key
                Case "Instalacion_Emplazamiento_Planta"
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Zona").Value = Nothing
                Case "Instalacion_Emplazamiento"
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Planta").Value = Nothing
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Zona").Value = Nothing
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Diagrama_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Diagrama.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Diagrama
                If oLinqPropuesta.ID_Propuesta = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaCreacion").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta").Value = oLinqPropuesta

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Diagrama_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Diagrama.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Diagrama_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Diagrama.M_ToolGrid_ToolClickBotonsExtras
        If Me.GRD_Diagrama.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If
        Select Case e.Tool.Key
            Case "Diagrama"
                Dim pepe As New frmDiagrama
                pepe.Entrada(oDTC, Me.GRD_Diagrama.GRID.ActiveRow.ListObject)
                pepe.FormObrir(Me, False)
                AddHandler pepe.AlTancarForm, AddressOf AlTancarFormDiagramas
                'pepe.MdiParent = Principal
                ' pepe.Show()

            Case "Duplicar"
                Dim _Diagrama As Propuesta_Diagrama = Me.GRD_Diagrama.GRID.ActiveRow.ListObject

                Dim _NewDiagrama As New Propuesta_Diagrama
                With _NewDiagrama
                    .Descripcion = _Diagrama.Descripcion
                    .FechaCreacion = _Diagrama.FechaCreacion
                    .Validado = _Diagrama.Validado

                    .ID_Instalacion_Emplazamiento = _Diagrama.ID_Instalacion_Emplazamiento
                    .ID_Instalacion_Emplazamiento_Planta = _Diagrama.ID_Instalacion_Emplazamiento_Planta
                    .ID_Instalacion_Emplazamiento_Zona = _Diagrama.ID_Instalacion_Emplazamiento_Zona

                    If IsNothing(_Diagrama.ID_DiagramaBinario) = False Then
                        Dim _DiagramaBinario As New DiagramaBinario
                        _DiagramaBinario.Fichero = _Diagrama.DiagramaBinario.Fichero
                        _DiagramaBinario.Foto = _Diagrama.DiagramaBinario.Foto
                        .DiagramaBinario = _DiagramaBinario
                        oDTC.DiagramaBinario.InsertOnSubmit(_DiagramaBinario)
                    End If
                End With
                _NewDiagrama.Propuesta = oLinqPropuesta
                oDTC.SubmitChanges()
                Call CargaGrid_Diagramas(oLinqPropuesta.ID_Propuesta)

        End Select

    End Sub

    Private Sub GRD_Diagrama_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Diagrama.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub AlTancarFormDiagramas(ByRef pID As String)
        Call CargaGrid_Diagramas(oLinqPropuesta.ID_Propuesta)
    End Sub


#End Region

#Region "Grid Seguridad"

    Private Sub CargaGrid_Seguridad(ByVal pId As Integer)
        Try
            Dim _Seguretat As IEnumerable(Of Propuesta_Seguridad) = From taula In oDTC.Propuesta_Seguridad Where taula.ID_Propuesta = pId Select taula

            With Me.GRD_Seguridad

                '.GRID.DataSource = _Seguretat
                .M.clsUltraGrid.CargarIEnumerable(_Seguretat)

                .M_Editable()

                'Call CargarCombo_Usuario(.GRID)

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
            If oLinqPropuesta.Propuesta_Seguridad.Where(Function(F) F.Usuario Is _Usuario).Count <> 0 Then
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

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta").Value = oLinqPropuesta

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguridad_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Seguridad.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPropuesta.Propuesta_Seguridad.Remove(e.ListObject)
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

#Region "Especificaciones"

    Private Sub CargaGrid_Respostes(ByVal pIDPropuesta As Integer)
        Try
            Dim _Resposta As IEnumerable(Of Propuesta_PropuestaEspecificacion) = From taula In oDTC.Propuesta_PropuestaEspecificacion Where taula.ID_Propuesta = pIDPropuesta Order By taula.ID_Propuesta_PropuestaEspecificacion Select taula

            With Me.GRD_Especificaciones
                '.GRID.DataSource = _Marca
                .M.clsUltraGrid.CargarIEnumerable(_Resposta)

                .M_Editable()
                .M_TreureFocus()

                .GRID.DisplayLayout.Bands(0).Columns("PropuestaEspecificacion").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("FechaRespuesta").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Usuario").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Preguntas(.GRID)
                'Call CargarCombo_Usuario(.GRID)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Especificaciones_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Especificaciones.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Especificaciones



                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta").Value = oLinqPropuesta

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub GRD_Respuesta_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Respuestas.M_ToolGrid_ToolEliminarRow
    '    Try

    '        If e.IsAddRow Then
    '            e.Delete(False)
    '            Exit Sub
    '        End If

    '        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
    '            e.Delete(False)
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
    '        End If

    '        oDTC.SubmitChanges()

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub


    Private Sub GRD_Especificaciones_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Especificaciones.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_Especificaciones_M_GRID_BeforeRowUpdate(ByRef sender As Object, ByRef e As CancelableRowEventArgs) Handles GRD_Especificaciones.M_GRID_BeforeRowUpdate
        e.Row.Cells("FechaRespuesta").Value = Now.Date
        e.Row.Cells("Usuario").Value = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault
    End Sub

    Private Sub CargarCombo_Preguntas(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of PropuestaEspecificacion) = (From Taula In oDTC.PropuestaEspecificacion Order By Taula.Descripcion Select Taula)
            Dim Var As New PropuestaEspecificacion

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("PropuestaEspecificacion").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("PropuestaEspecificacion").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub


    Private Sub GRD_Especificaciones_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Especificaciones.M_Grid_InitializeRow
        Call CargarCombo_Respuestas(Me.GRD_Especificaciones.GRID, e.Row.Cells("PropuestaEspecificacion_Respuesta"), e.Row.Cells("ID_PropuestaEspecificacion").Value)
        If e.ReInitialize = False Then
            Call CargarCombo_Usuario(Sender, e.Row.Cells("Usuario"), True)
        End If
    End Sub

    Private Sub GRD_Especificaciones_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Especificaciones.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "Usuario" Then
            Call CargarCombo_Usuario(sender, e.Cell, False)
        End If
    End Sub

    Private Sub CargarCombo_Respuestas(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pCell As UltraGridCell, ByVal pIDPregunta As Integer)
        Try
            If pCell Is Nothing OrElse pIDPregunta = 0 Then
                Exit Sub
            End If

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of PropuestaEspecificacion_Respuesta) = (From Taula In oDTC.PropuestaEspecificacion_Respuesta Where Taula.ID_PropuestaEspecificacion = pIDPregunta Order By Taula.Descripcion Select Taula)
            Dim Var As PropuestaEspecificacion_Respuesta

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
#End Region

#Region "Opciones"

    Private Sub CargaGrid_Opciones(ByVal pIDPropuesta As Integer)
        Try
            Dim _Opciones As IEnumerable(Of Propuesta_Opcion) = From taula In oDTC.Propuesta_Opcion Where taula.ID_Propuesta = pIDPropuesta Order By taula.ID_Propuesta_Opcion Select taula

            With Me.GRD_Opciones
                '.GRID.DataSource = _Marca
                .GRID.DisplayLayout.ViewStyle = ViewStyle.SingleBand
                .GRID.DisplayLayout.MaxBandDepth = 1
                .M.clsUltraGrid.CargarIEnumerable(_Opciones)

                .M_Editable()
                .M_TreureFocus()

                '.GRID.DisplayLayout.Bands(0).Columns("PropuestaEspecificacion").CellActivation = UltraWinGrid.Activation.NoEdit
                '.GRID.DisplayLayout.Bands(0).Columns("FechaRespuesta").CellActivation = UltraWinGrid.Activation.NoEdit
                '.GRID.DisplayLayout.Bands(0).Columns("Usuario").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_OpcionAcciones(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Opciones_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Opciones.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Opciones

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta").Value = oLinqPropuesta

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Opciones_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Opciones.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                Exit Sub
            End If

            Dim _Opcion As Propuesta_Opcion = e.ListObject
            If _Opcion.Propuesta_Linea.Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar, esta opción esta usada en una línea de propuesta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_Opciones_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Opciones.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_OpcionAcciones(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Propuesta_Opcion_Accion) = (From Taula In oDTC.Propuesta_Opcion_Accion Order By Taula.Descripcion Select Taula)
            Dim Var As New Propuesta_Opcion_Accion

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Propuesta_Opcion_Accion").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Propuesta_Opcion_Accion").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Financiación"

    Private Sub CargaGrid_Financiacion(ByVal pIDPropuesta As Integer)
        Try
            Dim _Opciones As IEnumerable(Of Propuesta_Financiacion) = From taula In oDTC.Propuesta_Financiacion Where taula.ID_Propuesta = pIDPropuesta Order By taula.ID_Propuesta_Financiacion Select taula

            With Me.GRD_Financiacion
                '.GRID.DataSource = _Marca
                .GRID.DisplayLayout.ViewStyle = ViewStyle.SingleBand
                .GRID.DisplayLayout.MaxBandDepth = 1
                .M.clsUltraGrid.CargarIEnumerable(_Opciones)

                .M_Editable()
                .M_TreureFocus()

                Call CargarCombo_FinanciacionMeses(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Financiacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Financiacion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Financiacion

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta").Value = oLinqPropuesta

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Financiacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Financiacion.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                Exit Sub
            End If

            'Dim _Opcion As Propuesta_Financiacion = e.ListObject
            'If _Opcion.Propuesta_Linea.Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible eliminar, esta opción esta usada en una línea de propuesta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Exit Sub
            'End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Financiacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Financiacion.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_FinanciacionMeses(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of FinanciacionMeses) = (From Taula In oDTC.FinanciacionMeses Order By Taula.Meses Select Taula)
            Dim Var As New FinanciacionMeses

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("FinanciacionMeses").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("FinanciacionMeses").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Ficheros"

    Private Sub GRD_Ficheros_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Ficheros.M_GRID_ClickRow2
        Try
            Util.WaitFormObrir()
            Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
            Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "Previsualizacion")
            'Me.Preview_UltraTab.Tabs("Previsualizacion").Selected = True

            Dim _IDArchivo As Integer = e.Cells("ID_Archivo").Value
            Dim _Archivo As Archivo
            _Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = _IDArchivo).FirstOrDefault

            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".pdf") Then
                Dim _stream As New System.IO.MemoryStream(_Archivo.CampoBinario.ToArray)
                Preview_PdfViewer.LoadDocument(_stream)
                'Me.Preview_UltraTab.Tabs("PDF").Selected = True
                Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
                Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "PDF")

            End If


            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".doc") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".docx") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".txt") Then
                Dim _stream As New System.IO.MemoryStream(_Archivo.CampoBinario.ToArray)
                If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".txt") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".csv") Then
                    Preview_RTF.RichText.LoadDocument(_stream, DevExpress.XtraRichEdit.DocumentFormat.PlainText)
                End If

                If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".doc") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".docx") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xml") Then
                    Preview_RTF.RichText.LoadDocument(_stream, DevExpress.XtraRichEdit.DocumentFormat.OpenXml)
                End If
                ' Me.Preview_UltraTab.Tabs("Word").Selected = True
                Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
                Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "Word")
                Me.Preview_RTF.Tag = _Archivo.ID_Archivo

                ' Me.Preview_RTF.BotoGuardar.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            End If


            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xls") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xlsx") Then
                Dim _stream As New System.IO.MemoryStream(_Archivo.CampoBinario.ToArray)

                If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xls") Then
                    Preview_Excel.M_LoadDocumentXLS(_Archivo.CampoBinario)
                End If
                If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xlsx") Then
                    Preview_Excel.M_LoadDocumentXLSX(_Archivo.CampoBinario)
                End If

                Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
                Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "Excel")
                'Me.Preview_UltraTab.Tabs("Excel").Selected = True
                Me.Preview_Excel.Tag = _Archivo.ID_Archivo

                Me.Preview_Excel.RibbonControl1.Items("BotoGuardar").Visibility = DevExpress.XtraBars.BarItemVisibility.Always


            End If

            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".tiff") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".png") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".jpg") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".jpeg") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".gif") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".bmp") Then
                Dim _stream As New System.IO.MemoryStream(_Archivo.CampoBinario.ToArray)

                Me.Preview_Picture.Properties.PictureStoreMode = DevExpress.XtraEditors.Controls.PictureStoreMode.ByteArray
                Me.Preview_Picture.EditValue = _Archivo.CampoBinario.ToArray

                Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
                Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "Foto")

                'Me.Preview_UltraTab.Tabs("Foto").Selected = True
            End If
            Util.WaitFormTancar()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlApretarElBotoGuardarDelExcel(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        If e.Item.Name = "BotoGuardar" Then
            If Me.Preview_Excel.Tag Is Nothing Then
                Exit Sub
            End If
            Dim _Archivo As Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = CInt(Me.Preview_Excel.Tag)).FirstOrDefault

            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xls") Then
                _Archivo.CampoBinario = Me.Preview_Excel.M_SaveDocumentXLS.ToArray
            End If
            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xlsx") Then
                _Archivo.CampoBinario = Me.Preview_Excel.M_SaveDocumentXLSX.ToArray
            End If

            oDTC.SubmitChanges()
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
        End If

    End Sub

    Public Sub AlApretarElBotoGuardarDelWord() Handles Preview_RTF.EventAlApretarElBotoGuardar

        If Me.Preview_RTF.Tag Is Nothing Then
            Exit Sub
        End If
        Dim _Archivo As Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = CInt(Me.Preview_RTF.Tag)).FirstOrDefault



        Dim _Resultat As New System.IO.MemoryStream
        If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".txt") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".csv") Then
            Me.Preview_RTF.RichText.Document.SaveDocument(_Resultat, DevExpress.XtraRichEdit.DocumentFormat.PlainText)
            _Archivo.CampoBinario = _Resultat.ToArray
        End If

        If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".doc") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".docx") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xml") Then
            Me.Preview_RTF.RichText.Document.SaveDocument(_Resultat, DevExpress.XtraRichEdit.DocumentFormat.OpenXml)
            _Archivo.CampoBinario = _Resultat.ToArray
        End If

        oDTC.SubmitChanges()
        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)

    End Sub
#End Region

    '#Region "IDisposable Support"
    '    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    '    ' IDisposable
    '    Protected Overrides Sub Dispose(disposing As Boolean)
    '        If Not Me.disposedValue Then
    '            If disposing Then
    '                If components IsNot Nothing Then components.Dispose()
    '                ' TODO: eliminar estado administrado (objetos administrados).

    '                'If oDTC Is Nothing = False Then
    '                '    'oDTC.Dispose()
    '                'End If
    '                If oDTC Is Nothing = False Then
    '                    Fichero.Dispose()
    '                End If

    '            End If

    '            ' Me.R_DetallesExtendidos.RichText.Dispose()
    '            ' Me.R_DetallesExtendidos.Dispose()
    '            ' Me.R_Observaciones.RichText.Dispose()
    '            ' Me.R_Observaciones.Dispose()

    '            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
    '            ' TODO: Establecer campos grandes como Null.
    '            'RemoveHandler Fichero.DespresDeCarregarDades, AddressOf DespresDeCarregarDades
    '            oDTC = Nothing
    '            Fichero = Nothing

    '        End If
    '        Me.disposedValue = True
    '        MyBase.Dispose(True)
    '    End Sub
    '#End Region


End Class