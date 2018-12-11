Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmActividadCRM_Mantenimiento
    Implements IDisposable
    Dim oDTC As DTCDataContext
    Dim oLinqActividad As ActividadCRM
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2
    Dim oclsActividad As clsActividad

    Enum SeleccionPersonal
        UsuarioActual = 1
        TodoElPersonal = 2
        SoloLosAsignadosALaActividad = 3
        SoloLosAsignadosALaActividadMenosUnoMismo = 4
    End Enum


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Call Guardar()
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Buscar() Handles ToolForm.m_ToolForm_Buscar
        Call Cridar_Llistat_Generic()
    End Sub

    Private Sub ToolForm_m_ToolForm_Nuevo() Handles ToolForm.m_ToolForm_Nuevo
        Call Netejar_Pantalla()
    End Sub

    Private Sub ToolForm_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqActividad.ID_ActividadCRM <> 0 Then
                If oLinqActividad.ID_Personal <> Seguretat.oUser.ID_Personal Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar, sólo el propietario de la actividad puede eliminarla", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
                    oLinqActividad.Activo = False
                    oDTC.SubmitChanges()
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                    Call Netejar_Pantalla()
                End If
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)

        End Try
    End Sub

    Private Sub ToolForm_m_ToolForm_ToolClick_Botones_Extras(Sender As Object, e As UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        Select Case e.Tool.Key
            Case "DarPorFinalizada"
                'If oLinqActividad.ID_ActividadCRM = 0 Then
                '    Exit Sub
                'End If

                'If oLinqActividad.ID_Personal = Seguretat.oUser.ID_Personal Then 'si ets el propietari de l'activitat finalitzarem tota la activitat
                '    oLinqActividad.Realizado = True
                '    oLinqActividad.PorcentajeRealizado = 100

                '    Me.CH_Realizado.Checked = True
                '    Me.T_PorcentajeRealizado.Value = 100

                '    oDTC.SubmitChanges()
                'Else
                '    Dim _Destinatari As ActividadCRM_Personal = oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
                '    If _Destinatari Is Nothing Then
                '        Exit Sub
                '    End If

                '    _Destinatari.Finalizado = True
                '    oDTC.SubmitChanges()

                '    Dim _NumDestinataris As Integer = oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.Finalizado = True).Count
                '    Dim _PercentatgeFinalitzat As Integer = Math.Round(_NumDestinataris / oLinqActividad.ActividadCRM_Personal.Count, 0)
                '    Me.T_PorcentajeRealizado.Value = _PercentatgeFinalitzat
                '    oLinqActividad.PorcentajeRealizado = _PercentatgeFinalitzat
                '    oDTC.SubmitChanges()
                'End If

                If oclsActividad.FinalizarActividad = True Then
                    Mensaje.Mostrar_Mensaje("Actividad finalizada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)
                    'Me.T_PorcentajeRealizado.Value = oLinqActividad.PorcentajeRealizado
                    'Me.CH_Realizado.Checked = oLinqActividad.Realizado
                    Call Cargar_Form(oLinqActividad.ID_ActividadCRM)
                End If
            Case "DarPorNoFinalizada"
                If oclsActividad.DesFinalizarActividad = True Then
                    Mensaje.Mostrar_Mensaje("Actividad activada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)
                    'Me.T_PorcentajeRealizado.Value = oLinqActividad.PorcentajeRealizado
                    'Me.CH_Realizado.Checked = oLinqActividad.Realizado
                    Call Cargar_Form(oLinqActividad.ID_ActividadCRM)
                End If

            Case "CrearParte"
                If oLinqActividad.ID_ActividadCRM <> 0 Then
                    Dim frm As New frmParte
                    frm.Entrada(0, Me.R_Descripcion.pText)
                    frm.FormObrir(Me, True)
                End If
        End Select
    End Sub

    'Private Sub ToolForm_m_ToolForm_Imprimir() Handles ToolForm.m_ToolForm_Imprimir
    '    If Guardar() = False Then
    '        Exit Sub
    '    End If
    '    Dim _DTC As New DTCDataContext(BD.Conexion)
    '    Informes.ObrirInformePreparacio(_DTC, EnumInforme.Propuesta, "[ID_Propuesta] = " & oLinqPropuesta.ID_Propuesta, "Propuesta - " & oLinqPropuesta.Codigo)
    '    '"[ID_Personal] = " & oLinqPersonal.ID_Personal

    '    'Dim frm As New frmInformePreparacio
    '    'frm.Entrada(EnumInforme.Propuesta, "[ID_Propuesta] = " & oLinqPropuesta.ID_Propuesta, oLinqPropuesta.ID_Propuesta)
    '    'frm.FormObrir(Me, False)
    'End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pID As Integer = 0, Optional ByVal pNombrePestañaSeleccionada As String = "")

        Try

            Me.AplicarDisseny()

            Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "ActividadCRM_Archivo", 1)

            AddHandler Preview_Excel.RibbonControl1.ItemClick, AddressOf AlApretarElBotoGuardarDelExcel
            Me.Preview_RTF.pBotoGuardarVisible = True

            Util.Cargar_Combo(Me.C_Tipo, "SELECT ID_ActividadCRM_Tipo, Descripcion FROM ActividadCRM_Tipo WHERE Activo=1 ORDER BY Descripcion", False)
            Util.Cargar_Combo(Me.C_Personal, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and FechaBajaEmpresa is null ORDER BY Nombre", False)
            Util.Cargar_Combo(Me.C_Prioridad, "SELECT ID_Prioridad, Descripcion FROM Prioridad  ORDER BY ID_Prioridad", False)

            Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = True
            Me.TL_Cliente.ButtonsRight("Ficha").Enabled = True
            Me.TL_Instalacion.ButtonsRight("Lupeta").Enabled = True
            Me.TL_Instalacion.ButtonsRight("Ficha").Enabled = True
            Me.TE_Propuesta_Codigo.ButtonsRight("Lupeta").Enabled = True

            Me.ToolForm.M.Botons.tAccions.SharedProps.Visible = True
            Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("DarPorFinalizada", "Dar por finalizada", True)
            Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("DarPorNoFinalizada", "Dar por no finalizada", True)
            Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("CrearParte", "Crear parte", True)

            Me.GRD_Destinatarios.M.clsToolBar.Boto_Afegir("Desfinalizar", "Desfinalizar la acción", False)
            Me.GRD_Chat.M.clsToolBar.Boto_Afegir("EnviarADemasDestinatarios", "Enviar a los demás destinatarios", True)
            Me.GRD_Chat.M.clsToolBar.Boto_Afegir("Responder", "Responder", True)

            If pID <> 0 Then
                Call Cargar_Form(pID)
            Else
                Call Netejar_Pantalla()
                ' Me.T_Codigo.Text = BD.RetornaMaxId("ActividadCRM") + 1
            End If

            Me.KeyPreview = False

            'If pNombrePestañaSeleccionada = "" Then
            '    Me.Tab_Principal.Tabs("General").Selected = True
            'Else
            '    Me.Tab_Principal.Tabs(pNombrePestañaSeleccionada).Selected = True
            'End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.TE_Codigo.Focus()

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqActividad)

            If oLinqActividad.ID_ActividadCRM = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.ActividadCRM.InsertOnSubmit(oLinqActividad)
                oDTC.SubmitChanges()

                oclsActividad = New clsActividad(oDTC, oLinqActividad, Seguretat.oUser.ID_Personal)
                oclsActividad.AfegirDestinatarilALaActivitat(oLinqActividad.ID_Personal, True, False)

                Me.TE_Codigo.Value = oLinqActividad.ID_ActividadCRM
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Call Fichero.Cargar_GRID(oLinqActividad.ID_ActividadCRM) 'Fem això pq la classe tingui el ID de pressupost
                Call CargaGrid_Destinatarios(oLinqActividad.ID_ActividadCRM)  'Fem això pq al apretar el botó afegir de la grid de personal, si la acció no estava guardada peta
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If

            Me.EstableixCaptionForm("Actividad: " & (oLinqActividad.Asunto))

            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqActividad
                Me.TE_Codigo.Text = .ID_ActividadCRM
                Me.T_Asunto.Text = .Asunto
                Me.DT_Alta.Value = .FechaAlta
                '  Me.DT_Aviso.Value = .FechaAviso
                Me.DT_Vencimiento.Value = .FechaVencimiento
                Me.CH_Realizado.Checked = .Finalizada
                Me.CH_ALaEsperaRespuesta.Checked = .ALaEsperaRespuesta
                Me.CH_Seguimiento.Checked = .SoloSeguimiento
                Me.T_PorcentajeRealizado.Value = .PorcentajeFinalizada

                If .ActividadCRM_Aux.DescripcionRTF Is Nothing = False Then
                    Me.R_Descripcion.RichText.RtfText = .ActividadCRM_Aux.DescripcionRTF
                Else
                    Me.R_Descripcion.pText = .Descripcion
                End If


                Me.R_Solucion.pText = .ActividadCRM_Aux.SolucionRTF
                Me.R_Observaciones.pText = .ActividadCRM_Aux.Observaciones
                ' Me.R_Descripcion.RichText.MhtText = .ActividadCRM_Aux.DescripcionRTF
                Me.C_Personal.Value = .ID_Personal
                ' Me.DT_HoraAviso.Value = .HoraAviso
                Me.C_Tipo.Value = .ID_ActividadCRM_Tipo
                Me.C_Prioridad.Value = .ID_Prioridad
                Me.DT_Aviso.Value = .AvisoFecha
                Me.DT_HoraAviso.Value = .AvisoHora

                If .ID_Cliente.HasValue = True Then
                    Call CargarDadesClient(.ID_Cliente)
                End If

                If .ID_Instalacion.HasValue = True Then
                    Call CargarDadesInstalacio(.ID_Instalacion, False)
                End If

                If .ID_Propuesta.HasValue = True Then
                    Call CargarDadesProposta(.ID_Propuesta, False)
                End If

                If .ActividadCRM_Aux.Hoja Is Nothing = False Then
                    Me.Excel1.M_LoadDocument(.ActividadCRM_Aux.Hoja)
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pActividadCRM As ActividadCRM)
        Try
            With pActividadCRM
                .Activo = True

                .Asunto = Me.T_Asunto.Text
                .FechaAlta = Me.DT_Alta.Value
                '.FechaAviso = Me.DT_Aviso.Value
                '.HoraAviso = Me.DT_HoraAviso.Value
                .FechaVencimiento = Me.DT_Vencimiento.Value
                .Finalizada = Me.CH_Realizado.Checked
                .PorcentajeFinalizada = Util.Comprobar_NULL_Per_0(Me.T_PorcentajeRealizado.Value)
                .ActividadCRM_Tipo = oDTC.ActividadCRM_Tipo.Where(Function(F) F.ID_ActividadCRM_Tipo = CInt(Me.C_Tipo.Value)).FirstOrDefault

                .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_Personal.Value)).FirstOrDefault
                .Prioridad = oDTC.Prioridad.Where(Function(F) F.ID_Prioridad = CInt(Me.C_Prioridad.Value)).FirstOrDefault
                .ALaEsperaRespuesta = Me.CH_ALaEsperaRespuesta.Checked
                .SoloSeguimiento = Me.CH_Seguimiento.Checked

                If Me.DT_Aviso.Value Is Nothing = False Then
                    If Me.DT_HoraAviso.Value Is Nothing Then
                        Me.DT_HoraAviso.Value = "00:00"
                    End If
                    Dim _FechaSenseHora As Date = Me.DT_Aviso.Value
                    Dim _FechaAmbHora As Date = Me.DT_HoraAviso.Value
                    Dim _fecha As New Date(_FechaSenseHora.Year, _FechaSenseHora.Month, _FechaSenseHora.Day, _FechaAmbHora.Hour, _FechaAmbHora.Minute, 0)
                    .AvisoFecha = _fecha
                    .AvisoHora = _fecha
                Else
                    .AvisoFecha = Nothing
                    .AvisoHora = Nothing

                End If

                'If pActividadCRM.ID_ActividadCRM = 0 Then
                '    Dim _aux As New ActividadCRM_Aux

                '    _aux.Observaciones = Me.R_Observaciones.pText
                '    _aux.DescripcionRTF = Me.R_Descripcion.pText
                '    _aux.Hoja = Me.Excel1.M_SaveDocument.ToArray
                '    .ActividadCRM_Aux = _aux
                'Else
                '    .ActividadCRM_Aux.Observaciones = Me.R_Observaciones.pText
                '    .ActividadCRM_Aux.DescripcionRTF = Me.R_Descripcion.pText
                '    .ActividadCRM_Aux.Hoja = Me.Excel1.M_SaveDocument.ToArray
                'End If

                Dim _aux As ActividadCRM_Aux

                If pActividadCRM.ID_ActividadCRM = 0 Then
                    _aux = New ActividadCRM_Aux
                    .ActividadCRM_Aux = _aux
                Else
                    _aux = .ActividadCRM_Aux
                End If

                _aux.Observaciones = Me.R_Observaciones.pText
                _aux.DescripcionRTF = Me.R_Descripcion.pText
                _aux.Hoja = Me.Excel1.M_SaveDocument.ToArray
                _aux.SolucionRTF = Me.R_Solucion.pText

                If Me.TL_Cliente.Tag Is Nothing Then
                    .Cliente = Nothing
                Else
                    .Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(Me.TL_Cliente.Tag)).FirstOrDefault
                End If

                If Me.TL_Instalacion.Tag Is Nothing Then
                    .Instalacion = Nothing
                Else
                    .Instalacion = oDTC.Instalacion.Where(Function(F) F.ID_Instalacion = CInt(Me.TL_Instalacion.Tag)).FirstOrDefault
                End If

                If Me.TE_Propuesta_Codigo.Tag Is Nothing Then
                    .Propuesta = Nothing
                Else
                    .Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = CInt(Me.TE_Propuesta_Codigo.Tag)).FirstOrDefault
                End If

                .Descripcion = Me.R_Descripcion.pTextEspecial
                .Solucion = Me.R_Solucion.pTextEspecial
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try

            Call Netejar_Pantalla()

            oLinqActividad = (From taula In oDTC.ActividadCRM Where taula.ID_ActividadCRM = pID Select taula).First

            oclsActividad = New clsActividad(oDTC, oLinqActividad, Seguretat.oUser.ID_Personal)

            If oclsActividad.TincAccesALaActivitat = False Then
                Mensaje.Mostrar_Mensaje("No tiene acceso a esta actividad", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Call Netejar_Pantalla()
                Exit Sub
            End If

            Me.ToolForm.ToolForm.Tools("DarPorFinalizada").SharedProps.Enabled = oclsActividad.HabilitarElBotonFinalizarUnaActividad
            Me.ToolForm.ToolForm.Tools("DarPorNoFinalizada").SharedProps.Enabled = oclsActividad.HabilitarElBotonDesFinalizarUnaActividad

            Call oclsActividad.DarPorLeidaLaActividad()

            Call SetToForm()

            Call CargaGrid_Destinatarios(pID)
            Call CargaGrid_Acciones(pID)

            Call CargaGrid_Chat(oLinqActividad.ID_ActividadCRM)
            oclsActividad.DonarPerLlegitTotElChat()


            Fichero.Cargar_GRID(pID)

            Call Permisos()

            Me.EstableixCaptionForm("Actividad: " & (oLinqActividad.Asunto))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()

        oLinqActividad = New ActividadCRM
        oDTC = New DTCDataContext(BD.Conexion)

        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.TE_Codigo.Value = Nothing
        Me.TE_Codigo.Tag = Nothing
        Me.DT_Alta.Value = Now.Date

        Me.C_Personal.Value = Seguretat.oUser.ID_Personal
        Me.C_Prioridad.Value = 1

        Me.Tab_Principal.Tabs("General").Selected = True
        Me.TAB_General.Tabs("Cliente").Selected = True

        Me.C_Personal.Value = Seguretat.oUser.ID_Personal



        Call CargaGrid_Acciones(0)
        Call CargarGrid_Cliente_Contacto(0)
        Call CargarGrid_Cliente_Seguimiento(0)
        Call CargarGrid_Cliente_ActividadesCRM(0)
        Call CargaGrid_Chat(0)
        Call CargaGrid_Destinatarios(0)

        Call BlanquejarInstalacion()

        Call BlanquejarPropuesta()

        'Me.C_Tipo.SelectedIndex = 0
        Me.C_Tipo.Value = Nothing

        Fichero.Cargar_GRID(0)

        Me.EstableixCaptionForm("Actividad")

        Me.Excel1.M_NewDocument()

        ErrorProvider1.Clear()

        Call Permisos()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                ' oClsControls.ControlBuit(.T_Codigo)
                oClsControls.ControlBuit(.T_Asunto)
                oClsControls.ControlBuit(.C_Tipo)
                oClsControls.ControlBuit(.DT_Alta)
                '  oClsControls.ControlBuit(.TL_Cliente, clsControles.EPropietat.pTag)
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

    Private Sub CargarDadesClient(ByVal pIDClient As Integer)
        Try
            Me.TL_Cliente.Tag = pIDClient

            Dim _Cliente As Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = pIDClient).FirstOrDefault

            With _Cliente
                Me.TL_Cliente.Text = .Codigo
                Me.T_Cliente_Codigo.Text = .Codigo

                Me.T_Cliente_PersonaContacto.Text = .PersonaContacto
                Me.T_Cliente_Direccion.Text = .Direccion
                Me.T_Cliente_Email.Text = .Email
                Me.T_Cliente_NIF.Text = .NIF
                Me.T_Cliente_Nombre.Text = .Nombre
                Me.T_Cliente_Nombre2.Text = .Nombre
                Me.T_Cliente_NombreComercial.Text = .NombreComercial
                Me.T_Cliente_Poblacion.Text = .Poblacion
                Me.T_Cliente_Provincia.Text = .Provincia
                Me.T_Cliente_Telefono.Text = .Telefono

                Call CargarGrid_Cliente_Contacto(.ID_Cliente)
                Call CargarGrid_Cliente_Seguimiento(.ID_Cliente)
                Call CargarGrid_Cliente_ActividadesCRM(.ID_Cliente)
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarDadesInstalacio(ByVal pIDInstalacion As Integer, Optional ByVal pCargantGrids As Boolean = False)
        Try
            If pIDInstalacion = 0 Then
                Exit Sub
            End If

            Me.TL_Instalacion.Tag = pIDInstalacion
            Me.TL_Instalacion.Text = pIDInstalacion

            Dim _Instalacion As Instalacion = oDTC.Instalacion.Where(Function(F) F.ID_Instalacion = pIDInstalacion).FirstOrDefault

            With _Instalacion
                If .ID_Delegacion.HasValue = True Then
                    Me.T_Instalacion_Delegacion.Text = .Delegacion.Descripcion
                End If
                Me.DT_Instalacion_Fecha.Value = .FechaInstalacion
                Me.T_Instalacion_Email.Text = .Email
                Me.T_Instalacion_Estado.Text = .Instalacion_Estado.Descripcion
                Me.T_Instalacion_PersonaContacto.Text = .PersonaContacto
                If .ID_Personal.HasValue = True Then
                    Me.T_Instalacion_Responsable.Text = .Personal.Nombre
                End If

                Me.T_Instalacion_Telefono.Text = .Telefono
                Me.T_Instalacion_Detalle.Text = .OtrosDetalles

                If pCargantGrids = True Then
                    Call CargarGrids_Instalacion(pIDInstalacion)
                End If
            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarGrids_Instalacion(ByVal pIDInstalacion As Integer)
        Call CargarGrid_Instalacion_Contacto(pIDInstalacion)
        Call CargarGrid_Instalacion_ActividadCRM(pIDInstalacion)
        Call CargarGrid_Instalacion_Propuesta(pIDInstalacion)
    End Sub

    Private Sub CargarDadesProposta(ByVal pIDProposta As Integer, Optional ByVal pCargantGrids As Boolean = False)
        Try
            If pIDProposta = 0 Then
                Exit Sub
            End If

            Me.TE_Propuesta_Codigo.Tag = pIDProposta


            Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = pIDProposta).FirstOrDefault

            With _Propuesta
                Me.TE_Propuesta_Codigo.Text = _Propuesta.Codigo
                If _Propuesta.ID_Personal.HasValue = True Then
                    Me.T_Propuesta_Comercial.Text = _Propuesta.Personal.Nombre
                End If
                Me.T_Propuesta_Descripcion.Text = _Propuesta.Descripcion
                Me.T_Propuesta_PersonaDirigido.Text = _Propuesta.Persona
                Me.T_Propuesta_Version.Text = _Propuesta.Version
                Me.T_Propuesta_Estado.Text = _Propuesta.Propuesta_Estado.Descripcion
                If .ID_Propuesta_EstadoCRM.HasValue = True Then
                    Me.T_Propuesta_EstadoCRM.Text = _Propuesta.Propuesta_EstadoCRM.Descripcion
                End If
                Me.DT_Propuesta_Fecha.Value = _Propuesta.Fecha
                Me.DT_Propuesta_FechaPrevisionCierre.Value = _Propuesta.FechaPrevisionCierre

                Me.T_Propuesta_TotalBase.Value = _Propuesta.RetornaTotalBase
                Me.T_Propuesta_Descuento.Value = _Propuesta.Descuento
                Me.T_Propuesta_Descuento_Importe.Value = (_Propuesta.RetornaTotalBase * _Propuesta.Descuento) / 100
                Me.T_Propuesta_Base_Menos_Descuento.Value = Me.T_Propuesta_TotalBase.Value - Me.T_Propuesta_Base_Menos_Descuento.Value
                Me.T_Propuesta_TotalIVA.Value = _Propuesta.IVA
                Me.T_Propuesta_TotalPropuesta.Value = _Propuesta.Total

                Me.R_Detalles_Extendidos.pText = _Propuesta.DetalleExtendido

                Dim _TotalCoste As Decimal = _Propuesta.Propuesta_Linea.Sum(Function(F) F.Unidad * F.PrecioCoste)
                Me.T_Propuesta_Margen_Total.Value = Me.T_Propuesta_Base_Menos_Descuento.Value - _TotalCoste
                If _TotalCoste = 0 Then
                    Me.T_Propuesta_Margen_Porcentaje.Value = 0
                Else
                    If Me.T_Propuesta_Base_Menos_Descuento.Value = 0 Then
                        Me.T_Propuesta_Margen_Porcentaje.Value = 0
                    Else
                        Me.T_Propuesta_Margen_Porcentaje.Value = (Me.T_Propuesta_Margen_Total.Value * 100) / Me.T_Propuesta_Base_Menos_Descuento.Value  '  (Me.T_Base_Menos_Descuento.Value / _TotalCoste) * 100
                    End If
                End If

                If pCargantGrids = True Then
                    Call CargarGrids_Propuesta(pIDProposta)
                End If

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarGrids_Propuesta(ByVal pIDPropuesta As Integer)
        Call CargarGrid_Propuesta_Lineas(pIDPropuesta)
    End Sub

    Private Sub AlTancarLlistatGenericCliente(ByVal pIDCliente As Integer)
        Call CargarDadesClient(pIDCliente)

        'netejem dades
        Call BlanquejarInstalacion()
        Call BlanquejarPropuesta()
    End Sub

    Private Sub AlTancarLlistatGenericInstalacion(ByVal pIDInstalacion As Integer)
        Call CargarDadesInstalacio(pIDInstalacion, True)
        Call BlanquejarPropuesta()
    End Sub

    Private Sub AlTancarLlistatGenericPropuesta(ByVal pIDPropuesta As Integer)
        Call CargarDadesProposta(pIDPropuesta, True)
    End Sub

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric

        'LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_ActividadCRM Where Activo=1  ORDER BY ID_ActividadCRM Desc", Me.TE_Codigo, "ID_ActividadCRM", "ID_ActividadCRM")
        LlistatGeneric.Mostrar_Llistat(clsActividad.RetornaDTActivitatsAVisualitzarSegonsPersonal(Seguretat.oUser.ID_Personal, True, False, True, True), Me.TE_Codigo, "ID_ActividadCRM", "ID_ActividadCRM")
        LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = True
        LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Caption = "Visualizar todas las actividades"


        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat
        AddHandler LlistatGeneric.AlApretarElBotoAuxiliar, AddressOf AlApretarElBotoAuxiliarDelLlistatGeneric

        'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
        '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
        '        pRow.Appearance.BackColor = Color.LightCoral
        '    End If
        'Next
    End Sub

    Private Sub AlApretarElBotoAuxiliarDelLlistatGenericClients(ByRef pInstanciaLlistatGeneric As M_LlistatGeneric.clsLlistatGeneric)
        'si el personal que esta mirant i te la marca de versoloclientesdondesetenga permisos pasarà això
        Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
        Dim _SQL As String = ""
        If _Personal.VerSoloClientesDondeSeTengaPermisos = True Then
            _SQL = " or ID_Cliente in (Select ID_Cliente From Cliente_Seguridad Where Cliente_Seguridad.ID_usuario=" & Seguretat.oUser.ID_Usuario & ") "
        End If

        'Això és el botó per visualitzar totes les activiats
        pInstanciaLlistatGeneric.pGrid.M.clsUltraGrid.Cargar("SELECT * FROM C_Cliente Where  Activo=1 and ID_Cliente_Tipo=" & EnumClienteTipo.FuturoCliente & _SQL & "  ORDER BY Nombre", BD)
        pInstanciaLlistatGeneric.AplicarCanvisBotoAuxiliarAlNouGrid()
        pInstanciaLlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = False

    End Sub

    Private Sub AlTancarLlistat(ByVal pID As String)
        Try
            Call Cargar_Form(pID)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub BlanquejarInstalacion()
        Me.TL_Instalacion.Tag = Nothing
        Util.Blanquejar(Me.TABPage_Instalacion, M_Util.Enum_Controles_Activacion.TODOS, True)
        Call CargarGrid_Instalacion_Contacto(0)
        Call CargarGrid_Instalacion_ActividadCRM(0)
        Call CargarGrid_Instalacion_Propuesta(0)
    End Sub

    Private Sub BlanquejarPropuesta()
        Me.TE_Propuesta_Codigo.Tag = Nothing
        Util.Blanquejar(Me.TABPage_Propuesta, M_Util.Enum_Controles_Activacion.TODOS, True)
        Call CargarGrid_Propuesta_Lineas(0)
    End Sub

    Private Sub CargarGrid_Cliente_Contacto(ByVal pIDCliente As Integer)
        Me.GRD_Cliente_Contactos.M.clsUltraGrid.Cargar(clsCliente.RetornaContactosDelCliente(pIDCliente))
    End Sub

    Private Sub CargarGrid_Cliente_Seguimiento(ByVal pIDCliente As Integer)
        Me.GRD_Cliente_Telemarketing.M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_Campaña_Cliente_Seguimiento Where Activo=1 and ID_Cliente=" & pIDCliente & " Order by ID_Campaña_Cliente Desc", True))
    End Sub

    Private Sub CargarGrid_Cliente_ActividadesCRM(ByVal pIDCliente As Integer)
        Me.GRD_Cliente_ActividadesCRM.M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_ActividadCRM Where Activo=1 and ID_Cliente=" & pIDCliente & " and ID_ActividadCRM<> " & oLinqActividad.ID_ActividadCRM & " Order by ID_ActividadCRM Desc", True))
    End Sub

    Private Sub CargarGrid_Instalacion_Contacto(ByVal pIDInstalacion As Integer)
        Me.GRD_Instalacion_Contacto.M.clsUltraGrid.Cargar(clsInstalacion.RetornaContactosDeLaInstalacion(pIDInstalacion))
    End Sub

    Private Sub CargarGrid_Instalacion_ActividadCRM(ByVal pIDInstalacion As Integer)
        Me.GRD_Instalacion_CRM.M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_ActividadCRM Where Activo=1 and ID_Instalacion=" & pIDInstalacion & " and ID_ActividadCRM<> " & oLinqActividad.ID_ActividadCRM & " Order by ID_ActividadCRM Desc", True))
    End Sub

    Private Sub CargarGrid_Instalacion_Propuesta(ByVal pIDInstalacion As Integer)
        Me.GRD_Instalacion_Propuesta.M.clsUltraGrid.Cargar(clsInstalacion.RetornaPropuestasDeLaInstalacion(pIDInstalacion))
    End Sub

    Private Sub CargarGrid_Propuesta_Lineas(ByVal pIDPropuesta As Integer)
        Me.GRD_Lineas.M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & pIDPropuesta, True))
    End Sub

    Private Sub Permisos()
        Me.TE_Codigo.ReadOnly = True
        Me.C_Tipo.ReadOnly = True
        Me.C_Prioridad.ReadOnly = True
        Me.DT_Vencimiento.ReadOnly = True
        Me.DT_Aviso.ReadOnly = True
        Me.DT_HoraAviso.ReadOnly = True
        Me.T_Asunto.ReadOnly = True
        Me.GRD_Destinatarios.M.clsToolBar.DesactivarBotonesEdicion()
        Me.GRD_Acciones.M.clsToolBar.DesactivarBotonesEdicion()
        Me.R_Descripcion.RichText.ReadOnly = True
        Me.CH_ALaEsperaRespuesta.Enabled = False
        Me.CH_Seguimiento.Enabled = False
        Me.GRD_Destinatarios.ToolGrid.Tools("Desfinalizar").SharedProps.Visible = False

        'Si l'activitat no esta finalitzada i ets el propietari O  si estàs fent una nova activitat es podran editar els textes
        If oLinqActividad.Finalizada = False AndAlso Me.oclsActividad Is Nothing = False AndAlso Me.oclsActividad.oEsPropietari = True OrElse oLinqActividad.ID_ActividadCRM = 0 Then
            Me.TE_Codigo.ReadOnly = False
            Me.C_Tipo.ReadOnly = False
            Me.C_Prioridad.ReadOnly = False
            Me.DT_Vencimiento.ReadOnly = False
            Me.DT_Aviso.ReadOnly = False
            Me.DT_HoraAviso.ReadOnly = False
            Me.T_Asunto.ReadOnly = False
            'Me.GRD_Destinatarios.M.
            Me.GRD_Destinatarios.M.clsToolBar.ActivarBotonesEdicion()
            Me.GRD_Acciones.M.clsToolBar.ActivarBotonesEdicion()
            Me.R_Descripcion.RichText.ReadOnly = False
            Me.CH_ALaEsperaRespuesta.Enabled = True
            Me.CH_Seguimiento.Enabled = True
            Me.GRD_Destinatarios.ToolGrid.Tools("Desfinalizar").SharedProps.Visible = True  'si ets el propietari podràs desfinalitzar
        Else
            If oLinqActividad.Finalizada = False Then  'una persona que no sigui propietaria i que sigui destinataria de l'activitat podrà crear noves activitats
                Me.GRD_Acciones.M.clsToolBar.ActivarBotonesEdicion()
            End If
        End If
    End Sub

#End Region

#Region "Grid Destinatarios"

    Private Sub CargaGrid_Destinatarios(ByVal pId As Integer)
        Try
            Dim _Asignacion As IEnumerable(Of ActividadCRM_Personal) = From taula In oDTC.ActividadCRM_Personal Where taula.ID_ActividadCRM = pId Select taula

            With Me.GRD_Destinatarios

                '.GRID.DataSource = _Asignacion
                .M.clsUltraGrid.CargarIEnumerable(_Asignacion)

                'If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                .M_Editable()
                'Else
                '.GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                'End If

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Destinatarios_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Destinatarios.M_GRID_CellListSelect
        Try
            If e.Cell.Column.Key <> "Personal" Then 'si no modifiquem el combo de personal no hi hem de fer re aqui
                Exit Sub
            End If

            ''Comprovem que el registre que hi havia abans no tenia hores imputades
            Dim _Personal As Personal = e.Cell.Value
            'If oLinqParte.Parte_Horas.Where(Function(F) F.Personal Is _Personal).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible desasignar el personal mientras tenga horas imputadas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    e.Cell.CancelUpdate()
            '    Exit Sub
            'End If

            'Comprovem que no s'hagi introduit aquest treballador abans
            _Personal = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
            If oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.Personal Is _Personal).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar la misma persona dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                e.Cell.CancelUpdate()
                Exit Sub
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub CargarCombo_Personal(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Personal) = (From Taula In oDTC.Personal Where Taula.Activo = True And Taula.FechaBajaEmpresa.HasValue = False Order By Taula.Nombre Select Taula)
    '        Dim Var As Personal

    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Nombre)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("Personal").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("Personal").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub CargarCombo_Personal(ByVal pGrid As UltraWinGrid.UltraGrid, ByRef pCell As UltraWinGrid.UltraGridCell, ByVal pSeleccionPersonal As SeleccionPersonal)
        Try

            Dim oTaula As IQueryable(Of Personal)
            Dim Valors As New Infragistics.Win.ValueList
            Dim _Personal As Personal = pCell.Value


            Select Case pSeleccionPersonal
                Case SeleccionPersonal.UsuarioActual
                    oTaula = (From Taula In oDTC.Personal Where Taula Is _Personal Order By Taula.Nombre Select Taula)
                Case SeleccionPersonal.TodoElPersonal
                    oTaula = (From Taula In oDTC.Personal Where (Taula Is _Personal) Or Taula.Activo = True And Taula.FechaBajaEmpresa.HasValue = False Order By Taula.Nombre Select Taula)
                Case SeleccionPersonal.SoloLosAsignadosALaActividad
                    Dim _PersonalAssignat As IEnumerable(Of Personal) = oDTC.ActividadCRM_Personal.Where(Function(F) F.ID_ActividadCRM = oLinqActividad.ID_ActividadCRM).Select(Function(F) F.Personal)
                    oTaula = (From Taula In oDTC.Personal Where _PersonalAssignat.Contains(Taula) Order By Taula.Nombre Select Taula)
                    ' Case SeleccionPersonal.SoloLosAsignadosALaActividadMenosUnoMismo
                    '     Dim _PersonalAssignat As IEnumerable(Of Personal) = oDTC.ActividadCRM_Personal.Where(Function(F) F.ID_ActividadCRM = oLinqActividad.ID_ActividadCRM And F.ID_Personal <> Seguretat.oUser.ID_Personal).Select(Function(F) F.Personal)
                    '     oTaula = (From Taula In oDTC.Personal Where _PersonalAssignat.Contains(Taula) Order By Taula.Nombre Select Taula)
            End Select

            Dim Var As Personal

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pCell.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Destinatarios_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Destinatarios.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "Personal" Then
            Call CargarCombo_Personal(sender, e.Cell, SeleccionPersonal.TodoElPersonal)
        End If
    End Sub

    Private Sub GRD_Destinatarios_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Destinatarios.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Destinatarios

                If oLinqActividad.ID_ActividadCRM = 0 AndAlso Guardar() = False Then
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("ActividadCRM").Value = oLinqActividad

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Destinatarios_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Destinatarios.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqActividad.ActividadCRM_Personal.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _IDPersonal As Integer = e.Cells("ID_Personal").Value
            'If oLinqParte.Parte_Horas.Where(Function(F) F.ID_Personal = _IDPersonal).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible desasignar el personal mientras tenga horas imputadas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Exit Sub
            'End If

            'If oLinqParte.Parte_Gastos.Where(Function(F) F.ID_Personal = _IDPersonal).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible desasignar el personal mientras tenga gastos imputados", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Exit Sub
            'End If

            'If oLinqParte.Parte_Incidencia.Where(Function(F) F.ID_Personal = _IDPersonal).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible desasignar el personal mientras tenga alguna incidencia asignada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Exit Sub
            'End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim _AsignacionPersonal As ActividadCRM_Personal = e.ListObject
                Dim _NewAvis As New clsAvisos(oDTC)
                _NewAvis.AlEliminarUnPersonalDeUnaActividad(oLinqActividad, _AsignacionPersonal.Personal)

                'oDTC.ActividadCRM_Personal.DeleteOnSubmit(e.ListObject)
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Destinatarios_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Destinatarios.M_GRID_AfterRowUpdate
        Try

            Dim _AsignacionPersonal As ActividadCRM_Personal = e.Row.ListObject
            If _AsignacionPersonal.ID_ActividadCRM_Personal = 0 Then
                Dim _NewAvis As New clsAvisos(oDTC)
                _NewAvis.AlAsignarUnPersonalAUnaActividad(oLinqActividad, _AsignacionPersonal.Personal)
                If _AsignacionPersonal.Personal.Personal_Emails.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("No se ha podido enviar un email de notificacion porque no tiene ningún mail asignado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Else
                    Dim _PersonalActual As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
                    Dim _Missatge As String = "El usuario:<b>" & _PersonalActual.Nombre & " </b><br>Te ha asignado a la actividad nº: <b>" & oLinqActividad.ID_ActividadCRM & "</b><br>Asunto:<b> " & oLinqActividad.Asunto
                    _Missatge = _Missatge & "<br>" & Me.R_Descripcion.RichText.HtmlText

                    Dim _Asunto As String = "Has sido asignado a una nueva actividad de Abidos"
                    clsActividad.EnviarCorreuAlaBustia(_AsignacionPersonal.Personal.Personal_Emails.FirstOrDefault.Email, "abidos@westpoint.es", _Asunto, _Missatge, Seguretat.oUser.ID_Personal, _AsignacionPersonal.ID_Personal, oLinqActividad.ID_ActividadCRM)
                End If

            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Destinatarios_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Destinatarios.M_Grid_InitializeRow
        If e.Row.Cells("ID_ActividadCRM_Personal").Value <> 0 Then
            e.Row.Activation = Activation.NoEdit
        Else
            e.Row.Activation = Activation.AllowEdit
        End If
        If e.ReInitialize = False Then
            Call CargarCombo_Personal(Sender, e.Row.Cells("Personal"), SeleccionPersonal.UsuarioActual)
        End If
    End Sub

    Private Sub GRD_Destinatarios_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Destinatarios.M_ToolGrid_ToolClickBotonsExtras2
        If e.Tool.Key = "Desfinalizar" Then
            If Me.GRD_Destinatarios.GRID.ActiveRow Is Nothing Then
                Exit Sub
            End If

            If Me.GRD_Destinatarios.GRID.ActiveRow.Cells("Finalizado").Value = True Then
                oclsActividad.DesFinalizarActividad(Me.GRD_Destinatarios.GRID.ActiveRow.Cells("ID_ActividadCRM_Personal").Value)
                Mensaje.Mostrar_Mensaje("Actividad desfinalizada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                Call Cargar_Form(oLinqActividad.ID_ActividadCRM)
            End If
        End If

    End Sub
#End Region

#Region "Grid Acciones"

    Private Sub CargaGrid_Acciones(ByVal pId As Integer)
        Try

            With Me.GRD_Acciones

                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_ActividadCRM_Acciones Where ID_ActividadCRM=" & oLinqActividad.ID_ActividadCRM, True))

                .M_NoEditable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Acciones_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Acciones.M_GRID_DoubleClickRow2
        Call ObrirPantallaAccionesEnModoEdicio()
    End Sub

    Private Sub GRD_Acciones_M_ToolGrid_ToolAfegir(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Acciones.M_ToolGrid_ToolAfegir
        Try
            If oLinqActividad.ID_ActividadCRM = 0 AndAlso Guardar() = False Then
                Exit Sub
            End If

            Dim frm As New frmActividadCRM_Accion
            frm.Entrada(oLinqActividad, oDTC, oclsActividad)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormulariAccion
            frm.FormObrir(Me, False)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Acciones_M_ToolGrid_ToolEliminar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Acciones.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_Acciones.GRID.ActiveRow Is Nothing = True Then
                Exit Sub
            End If

            Dim _Accion As ActividadCRM_Accion = oDTC.ActividadCRM_Accion.Where(Function(F) F.ID_ActividadCRM_Accion = CInt(Me.GRD_Acciones.GRID.ActiveRow.Cells("ID_ActividadCRM_Accion").Value)).FirstOrDefault
            If _Accion.ID_Personal <> Seguretat.oUser.ID_Personal Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar, sólo el propietario de la acción puede eliminarla", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
                ' oLinqActividad.ActividadCRM_Accion.Remove(_Accion)
                oDTC.ActividadCRM_Accion.DeleteOnSubmit(_Accion)
                oDTC.ActividadCRM_Accion_Aux.DeleteOnSubmit(_Accion.ActividadCRM_Accion_Aux)
                oDTC.ActividadCRM_Accion_Personal.DeleteAllOnSubmit(_Accion.ActividadCRM_Accion_Personal)
                oDTC.Aviso.DeleteAllOnSubmit(_Accion.Aviso)
                'oDTC.ActividadCRM_Accion.DeleteOnSubmit(_Accion)
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                Call AlTancarFormulariAccion()
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Acciones_M_ToolGrid_ToolVisualitzar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Acciones.M_ToolGrid_ToolVisualitzar
        Call ObrirPantallaAccionesEnModoEdicio()
    End Sub

    Private Sub AlTancarFormulariAccion()
        Call CargaGrid_Acciones(oLinqActividad.ID_ActividadCRM)
        Call CargaGrid_Destinatarios(oLinqActividad.ID_ActividadCRM)
    End Sub

    Private Sub ObrirPantallaAccionesEnModoEdicio()
        If Me.GRD_Acciones.GRID.ActiveRow Is Nothing = False Then
            Dim _IDAccion As Integer = Me.GRD_Acciones.GRID.ActiveRow.Cells("ID_ActividadCRM_Accion").Value
            If oclsActividad.TienePersmisoElUsuarioActualAEntrarALaAccion(_IDAccion) = False Then
                Mensaje.Mostrar_Mensaje("No tiene permisos para entrar en la pantalla", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            Dim frm As New frmActividadCRM_Accion
            frm.Entrada(oLinqActividad, oDTC, oclsActividad, _IDAccion)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormulariAccion
            frm.FormObrir(Me, False)
        End If
    End Sub

#End Region

#Region "Grid Chat"
    Private Sub CargaGrid_Chat(ByVal pId As Integer)
        Try
            Dim _Chat As IEnumerable(Of ActividadCRM_Chat) = From taula In oDTC.ActividadCRM_Chat Where taula.ID_ActividadCRM = pId Select taula

            With Me.GRD_Chat

                '.GRID.DataSource = _Asignacion
                .M.clsUltraGrid.CargarIEnumerable(_Chat)

                'If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                .M_Editable()
                '.GRID.DisplayLayout.Override.RowSizingAutoMaxLines = 3
                .GRID.DisplayLayout.Bands(0).Columns("Mensaje").CellMultiLine = DefaultableBoolean.True
                .GRID.DisplayLayout.Override.DefaultRowHeight = 35
                'Else
                '.GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                'End If

                '  Call CargarCombo_Personal_Origen(.GRID)
                '  Call CargarCombo_Personal_Destino(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub CargarCombo_Personal_Origen(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Personal) = (From Taula In oDTC.Personal Where Taula.Activo = True And Taula.FechaBajaEmpresa.HasValue = False Order By Taula.Nombre Select Taula)
    '        Dim Var As Personal

    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Nombre)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("Personal_Origen").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("Personal_Origen").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarCombo_PersonalChat(ByVal pGrid As UltraWinGrid.UltraGrid, ByRef pCell As UltraWinGrid.UltraGridCell, ByVal pTOTS As Boolean, Optional ByVal pCarregarUsuariActual As Boolean = False)
    '    Try

    '        Dim oTaula As IQueryable(Of Personal)
    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim _Personal As Personal = pCell.Value

    '        If pCarregarUsuariActual = True Then  'aquesta opció es pq quan apretem el botó afegir com que no carreguem cap combo no apareix re
    '            If pTOTS = True Then
    '                oTaula = (From Taula In oDTC.Personal Where (Taula Is _Personal) Or Taula.Activo = True And Taula.FechaBajaEmpresa.HasValue = False Order By Taula.Nombre Select Taula)
    '            Else
    '                oTaula = (From Taula In oDTC.Personal Where Taula Is _Personal Order By Taula.Nombre Select Taula)
    '            End If
    '        Else
    '            oTaula = (From Taula In oDTC.Personal Where Taula Is oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault)
    '        End If

    '        Dim Var As Personal

    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Nombre)
    '        Next

    '        pCell.ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarCombo_Personal_Destino(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
    '    Try
    '        Dim _PersonalAssignat As IEnumerable(Of Personal) = oDTC.ActividadCRM_Personal.Where(Function(F) F.ID_ActividadCRM = oLinqActividad.ID_ActividadCRM).Select(Function(F) F.Personal)

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Personal) = (From Taula In oDTC.Personal Where _PersonalAssignat.Contains(Taula) And Taula.Activo = True And Taula.FechaBajaEmpresa.HasValue = False Order By Taula.Nombre Select Taula)
    '        Dim Var As Personal

    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Nombre)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("Personal_Destino").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("Personal_Destino").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub GRD_Chat_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Chat.M_Grid_InitializeRow
        If e.Row.Cells("ID_ActividadCRM_Chat").Value <> 0 Then
            e.Row.Activation = Activation.NoEdit
        Else
            e.Row.Cells("Personal_Origen").Activation = Activation.NoEdit
            ' e.Row.Cells("Personal_Destino").Activation = Activation.ActivateOnly
            e.Row.Cells("FechaAlta").Activation = Activation.NoEdit
        End If
        If e.ReInitialize = False Then
            Call CargarCombo_Personal(Sender, e.Row.Cells("Personal_Origen"), SeleccionPersonal.UsuarioActual)
            Call CargarCombo_Personal(Sender, e.Row.Cells("Personal_Destino"), SeleccionPersonal.SoloLosAsignadosALaActividad)
        End If
    End Sub

    Private Sub GRD_Chat_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Chat.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Chat

                If oLinqActividad.ID_ActividadCRM = 0 AndAlso Guardar() = False Then
                    Exit Sub
                End If

                If oLinqActividad.ActividadCRM_Personal.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Para realizar esta acción primero debe introducir destinatarios de la actividad", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                .M_ExitEditMode()
                Call AfegirNouChat()

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Chat_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Chat.M_GRID_AfterRowUpdate
        Try

            'Dim _AsignacionPersonal As ActividadCRM_Personal = e.Row.ListObject
            'If _AsignacionPersonal.ID_ActividadCRM_Personal = 0 Then
            '    Dim _NewAvis As New clsAvisos(oDTC)
            '    _NewAvis.AlAsignarUnPersonalAUnaActividad(oLinqActividad, _AsignacionPersonal.Personal)
            'End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Chat_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Chat.M_GRID_BeforeCellActivate
        'If e.Cell.Column.Key = "Personal_Origen" Or e.Cell.Column.Key = "Personal_Destino" Then
        '    Call CargarCombo_Personal(sender, e.Cell, SeleccionPersonal.SoloLosAsignadosALaActividad)
        'End If
    End Sub

    Private Sub GRD_Chat_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Chat.M_ToolGrid_ToolClickBotonsExtras2
        Select Case e.Tool.Key
            Case "EnviarADemasDestinatarios"
                If Me.GRD_Chat.GRID.ActiveRow Is Nothing = False AndAlso Me.GRD_Chat.GRID.ActiveRow.Cells("ID_ActividadCRM_Chat").Value <> 0 Then
                    Call oclsActividad.EnviarChatADemasDestinatarios(Me.GRD_Chat.GRID.ActiveRow.ListObject)
                    Mensaje.Mostrar_Mensaje("Mensaje reenviado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)
                    Call CargaGrid_Chat(oLinqActividad.ID_ActividadCRM)
                End If

            Case "Responder"
                If Me.GRD_Chat.GRID.ActiveRow Is Nothing = False AndAlso Me.GRD_Chat.GRID.ActiveRow.Cells("ID_ActividadCRM_Chat").Value <> 0 Then
                    Call AfegirNouChat(True)
                End If
        End Select

    End Sub

    Private Sub AfegirNouChat(Optional ByVal pResponder As Boolean = False)
        Try
            With Me.GRD_Chat

                Dim _pRow As UltraGridRow
                _pRow = .GRID.ActiveRow

                .M_AfegirFila()
                Call CargarCombo_Personal(.GRID, .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal_Origen"), SeleccionPersonal.TodoElPersonal)


                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaAlta").Value = Now
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal_Origen").Value = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("ActividadCRM").Value = oLinqActividad

                If .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal_Destino").ValueList.ItemCount = 2 Then
                    Dim _Personal1 As Personal = .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal_Destino").ValueList.GetValue(0)
                    Dim _Personal2 As Personal = .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal_Destino").ValueList.GetValue(1)
                    If _Personal1.ID_Personal = Seguretat.oUser.ID_Personal Then
                        .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal_Destino").Value = _Personal2
                    Else
                        .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal_Destino").Value = _Personal1
                    End If

                    '.GRID.Focus()

                    ' System.Threading.Thread.Sleep(200)
                    '.GRID.Rows.FilterRow.Cells("Mensaje").Activate()
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("Mensaje").Activate()
                    .GRID.PerformAction(UltraGridAction.EnterEditMode)
                End If



                If pResponder = True Then
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal_Destino").Value = _pRow.Cells("Personal_Origen").Value
                End If


            End With
        Catch ex As Exception

        End Try
    End Sub



#End Region

#Region "Events Varis"
    Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Me.TE_Codigo.Value Is Nothing = False AndAlso IsDBNull(Me.TE_Codigo.Value) = False Then
                Dim ooLinqActividad As ActividadCRM = oDTC.ActividadCRM.Where(Function(F) F.ID_ActividadCRM = CInt(Me.TE_Codigo.Value) And F.Activo = True).FirstOrDefault()
                If ooLinqActividad Is Nothing = False Then
                    Call Cargar_Form(ooLinqActividad.ID_ActividadCRM)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If
            Else
                Me.TE_Codigo.Value = oDTC.ActividadCRM.Where(Function(F) F.Activo = True).Max(Function(F) F.ID_Instalacion) + 1
                Exit Sub
            End If
        End If
    End Sub

    Private Sub TE_Codigo_EditorButtonClick(sender As Object, e As UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Call Cridar_Llistat_Generic()
        End If
    End Sub

    Private Sub TL_Cliente_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TL_Cliente.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            'si el personal que esta mirant i te la marca de versoloclientesdondesetenga permisos pasarà això
            Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
            Dim _SQL As String = ""
            If _Personal.VerSoloClientesDondeSeTengaPermisos = True Then
                _SQL = " and ID_Cliente in (Select ID_Cliente From Cliente_Seguridad Where Cliente_Seguridad.ID_usuario=" & Seguretat.oUser.ID_Usuario & ") "
            End If

            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Cliente Where  Activo=1  and ID_Cliente_Tipo=1 " & _SQL & " ORDER BY Nombre", Me.TL_Cliente, "ID_Cliente", "Codigo", Me.T_Cliente_Nombre2, "Nombre")
            LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = True
            LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Caption = "Visualizar también los futuros clientes"
            AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGenericCliente
            AddHandler LlistatGeneric.AlApretarElBotoAuxiliar, AddressOf AlApretarElBotoAuxiliarDelLlistatGenericClients
        End If

        If e.Button.Key = "Ficha" And Me.TL_Cliente.Tag Is Nothing = False Then
            Dim frmClient As New frmCliente
            frmClient.Entrada(Me.TL_Cliente.Tag)
            frmClient.FormObrir(Me, True)
        End If
    End Sub

    Private Sub AlApretarElBotoAuxiliarDelLlistatGeneric(ByRef pInstanciaLlistatGeneric As M_LlistatGeneric.clsLlistatGeneric)
        pInstanciaLlistatGeneric.pGrid.M.clsUltraGrid.Cargar("SELECT * FROM C_Instalacion Where  activo=1 ORDER BY ID_Instalacion desc", BD)
        pInstanciaLlistatGeneric.AplicarCanvisBotoAuxiliarAlNouGrid()
        pInstanciaLlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = False
    End Sub

    Private Sub TL_Instalacion_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TL_Instalacion.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Instalacion Where ID_Cliente=" & Util.Comprobar_NULL_Per_0(Me.TL_Cliente.Tag) & " and Activo=1 ORDER BY ID_Instalacion", Me.TL_Instalacion, "ID_Instalacion", "ID_Instalacion")
            ' AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatCliente
        End If

        If e.Button.Key = "Ficha" And Me.TL_Cliente.Tag Is Nothing = False Then
            Dim frmInstalacion As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
            frmInstalacion.Entrada(Me.TL_Instalacion.Tag)
            frmInstalacion.FormObrir(Me, True)
        End If
    End Sub

    Private Sub TAB_Cliente_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Cliente.SelectedTabChanged
        Select Case e.Tab.Key
            Case "Contactos"

            Case "Telemarketing"

            Case "ActividadesCRMCliente"

        End Select

    End Sub

    Private Sub TE_Propuesta_Codigo_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Propuesta_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Propuesta Where ID_Instalacion=" & Util.Comprobar_NULL_Per_0(Me.TL_Instalacion.Tag) & " and Activo=1 ORDER BY Codigo", Me.TE_Propuesta_Codigo, "ID_Propuesta", "Codigo")
            AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGenericPropuesta
        End If
    End Sub

    Private Sub TAB_General_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_General.SelectedTabChanged
        Select Case e.Tab.Key
            Case "Instalacion"
                If Me.TL_Instalacion.Tag Is Nothing = False Then
                    Call CargarGrids_Instalacion(CInt(Me.TL_Instalacion.Tag))
                End If
            Case "Propuesta"
                If Me.TE_Propuesta_Codigo.Tag Is Nothing = False Then
                    Call CargarGrids_Propuesta(CInt(Me.TE_Propuesta_Codigo.Tag))
                End If
        End Select
    End Sub

    Private Sub frmActividadCRM_Mantenimiento_AlTancarForm(ByRef pCancel As Boolean) Handles Me.AlTancarForm
        'Si la activitat esta guardada
        If oLinqActividad.ID_ActividadCRM <> 0 Then
            'Si no s'ha guardat amb el client
            If oLinqActividad.ID_Cliente.HasValue = False Then
                'Si tampoc s'ha seleccionat a la pantalla llavors no deixarem sortir, en cas contrari guardarem el client seleccionat a dins de l'activitat
                If Me.TL_Cliente.Tag Is Nothing Then
                    Mensaje.Mostrar_Mensaje("Imposible cerrar la pantalla, es obligatorio introducir el cliente", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    pCancel = True
                Else
                    oLinqActividad.ID_Cliente = Me.TL_Cliente.Tag
                    oDTC.SubmitChanges()
                End If
            End If
            If oLinqActividad.ID_ActividadCRM_Tipo = 1 And Me.C_Tipo.ReadOnly = False Then 'Si és la genèrica no deixarem guardar
                Mensaje.Mostrar_Mensaje("Imposible cerrar la pantalla, la actividad no puede ser de tipo 'Genérica'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                pCancel = True
            Else
                oLinqActividad.ID_ActividadCRM_Tipo = Me.C_Tipo.Value
                oDTC.SubmitChanges()
            End If

        End If
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

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Para detectar llamadas redundantes

    ' IDisposable
    Protected Overrides Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If components IsNot Nothing Then components.Dispose()
                ' TODO: eliminar estado administrado (objetos administrados).

                If oDTC Is Nothing = False Then
                    oDTC.Dispose()
                End If
                If oDTC Is Nothing = False Then
                    Fichero.Dispose()
                End If

                Me.GRD_Acciones.GRID.DataSource = Nothing
                Me.GRD_Acciones.GRID.Dispose()
                Me.GRD_Acciones.Dispose()
                Me.R_Descripcion.Dispose()
                Me.R_Detalles_Extendidos.Dispose()
                Me.R_Observaciones.Dispose()
                Me.R_Solucion.Dispose()
            End If

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
            Me.GRD_Acciones.GRID = Nothing
            Me.GRD_Acciones = Nothing

            Me.R_Descripcion = Nothing
            Me.R_Detalles_Extendidos = Nothing
            Me.R_Observaciones = Nothing
            Me.R_Solucion = Nothing

            oDTC = Nothing
            Fichero = Nothing

        End If

        Me.disposedValue = True
        ' Me.Dispose(True)
        MyBase.Dispose(True)
    End Sub
#End Region

    Private Sub C_Tipo_ValueChanged(sender As Object, e As EventArgs) Handles C_Tipo.ValueChanged

    End Sub
End Class