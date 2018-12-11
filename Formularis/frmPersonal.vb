Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmPersonal
    Dim oDTC As DTCDataContext
    Dim oLinqPersonal As Personal
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqPersonal.ID_Personal <> 0 Then
                'If DT_Baja.Value Is Nothing Then
                '    Mensaje.Mostrar_Mensaje("Para poder dar de baja una persona se requiere introducir la fecha de la baja laboral", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                '    Exit Sub
                'End If

                If Mensaje.Mostrar_Mensaje("Desea dar de alta/baja el registro?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    If Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja" Then
                        Me.T_MotivoBaja.Text = Mensaje.Mostrar_Entrada_Datos("Introduzca el motivo de la baja laboral:", , True)
                        Me.DT_Baja.Value = Date.Now
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                        Me.L_Baja.Visible = True
                        Me.T_MotivoBaja.Visible = True
                        Me.L_FechaBaja.Visible = True
                        Me.DT_Baja.Visible = True
                    Else
                        Me.DT_Baja.Value = Nothing
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                        Me.L_Baja.Visible = False
                        Me.T_MotivoBaja.Visible = False
                        Me.L_FechaBaja.Visible = False
                        Me.DT_Baja.Visible = False
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
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Buscar() Handles ToolForm.m_ToolForm_Buscar
        Call Cridar_Llistat_Generic()
    End Sub

    Private Sub ToolForm_m_ToolForm_Imprimir() Handles ToolForm.m_ToolForm_Imprimir
        If Guardar() = False Then
            Exit Sub
        End If

        Informes.ObrirInformePreparacio(oDTC, EnumInforme.Personal, "[ID_Personal] = " & oLinqPersonal.ID_Personal, "Personal - " & oLinqPersonal.ID_Personal)
        'Dim frm As New frmInformePreparacio
        'frm.Entrada(EnumInforme.Personal, "[ID_Personal] = " & oLinqPersonal.ID_Personal)
        'frm.FormObrir(Me, False)
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pId As Integer = 0)
        Try

            Me.AplicarDisseny()



            Util.Cargar_Combo(Me.C_Tipo, "Select ID_Personal_Tipo, Descripcion From Personal_Tipo Where Activo=1 Order by Codigo")
            Util.Cargar_Combo(Me.C_Sexo, "Select ID_Sexo, Descripcion From Sexo Order by Descripcion", False)
            Util.Cargar_Combo(Me.C_Nacionalidad, "Select ID_Nacionalidad, Descripcion From Nacionalidad Order by Descripcion", False)
            Util.Cargar_Combo(Me.C_EstadoCivil, "Select ID_EstadoCivil, Descripcion From EstadoCivil Order by Descripcion", False)

            Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Personal_Archivo", 1)

            Call Netejar_Pantalla()

            Me.GRD_HorasRealizadas.M.clsToolBar.Boto_Afegir("HorasMalIntroducidas", "Marcar horas mal introducidas", True)
            Me.GRD_Ficheros.M.clsToolBar.Boto_Afegir("FotoPredeterminada", "Foto Predeterminada", True)
            Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True

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

            If oLinqPersonal.ID_Personal = 0 Then
                If BD.RetornaValorSQL("SELECT Count (*) From Personal WHERE DNI='" & Util.APOSTROF(Me.T_DNI.Text) & "'") > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible añadir, existe un trabajador con este DNI dado de alta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
            Else
                If BD.RetornaValorSQL("SELECT Count (*) From Personal WHERE DNI='" & Util.APOSTROF(Me.T_DNI.Text) & "' and ID_Personal<>" & oLinqPersonal.ID_Personal) > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible modificar, existe un trabajador con este DNI dado de alta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqPersonal)

            If oLinqPersonal.ID_Personal = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Personal.InsertOnSubmit(oLinqPersonal)
                oDTC.SubmitChanges()

                Me.TE_Codigo.Text = oLinqPersonal.ID_Personal
                Call Fichero.Cargar_GRID(oLinqPersonal.ID_Personal) 'Fem això pq la classe tingui el ID de pressupost
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeGuardar)

                'Automàticament al insertar un registre afegirem la empresa predeterminada
                Dim _PersonalEmpresa As New Personal_Empresa
                _PersonalEmpresa.ID_Empresa = oEmpresa.ID_Empresa
                _PersonalEmpresa.Predeterminada = True
                oLinqPersonal.Personal_Empresa.Add(_PersonalEmpresa)
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
            With oLinqPersonal
                Me.TE_Codigo.Value = .ID_Personal
                Me.T_Nombre.Text = .Nombre
                Me.T_DNI.Text = .DNI
                Me.T_PrecioCoste.Text = .PrecioCoste
                Me.T_PrecioCoste_HoraExtra.Value = .PrecioCosteHoraExtra
                Me.T_HorasMinimas.Value = .HorasMinimas
                Me.C_Tipo.Value = .ID_Personal_Tipo
                Me.R_Observaciones.pText = .Observaciones
                Me.T_NumTip.Text = .NumTip
                Me.DT_FechaTip.Value = .FechaTip
                Me.DT_Alta.Value = .FechaAltaEmpresa
                Me.DT_Baja.Value = .FechaBajaEmpresa
                Me.DT_FechaAltaSS.Value = .FechaAltaSS
                Me.DT_FechaBajaSS.Value = .FechaBajaSS
                Me.T_NumSS.Text = .NumSS
                Me.T_Cargo.Text = .Cargo
                Me.T_MotivoBaja.Text = .MotivoBaja
                If DT_Baja.Value Is Nothing Then
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                    Me.L_Baja.Visible = False
                    Me.T_MotivoBaja.Visible = False
                Else
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                    Me.L_Baja.Visible = True
                    Me.T_MotivoBaja.Visible = True
                End If

                Me.T_Direccion.Text = .Direccion
                Me.T_Poblacion.Text = .Poblacion
                Me.T_Provincia.Text = .Provincia
                Me.T_CodigoPostal.Text = .CP
                Me.T_Pais.Text = .Pais
                Me.T_TelefonoParticular.Text = .TelefonoParticular
                Me.T_MovilParticular.Text = .MovilParticular
                Me.T_EmailParticular.Text = .CorreoElectronicoParticular
                Me.T_NumSS.Text = .NumSS
                Me.C_Sexo.Value = .ID_Sexo
                Me.C_Nacionalidad.Value = .ID_Nacionalidad
                Me.DT_Nacimiento.Value = .FechaNacimiento
                If IsNothing(.ID_EstadoCivil) Then
                    Me.C_EstadoCivil.SelectedIndex = -1
                Else
                    Me.C_EstadoCivil.Value = .ID_EstadoCivil
                End If

                Me.T_Telefono.Text = .Telefono
                Me.T_Extension.Text = .Extension
                Me.T_Movil.Text = .Movil
                Me.T_Email.Text = .CorreoElectronico
                Me.CH_PersonalExterno.Checked = .PersonalExterno
                Me.CH_OcultarEnListados.Checked = .OcultarEnListados
                Me.CH_ActivarCalendario.Checked = .ActivarCalendario
                Me.CH_VerSoloClientesDondeSeTengaPermisos.Checked = .VerSoloClientesDondeSeTengaPermisos

                Call CargarFoto()

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pPersonal As Personal)
        Try
            With pPersonal
                .Activo = True

                .Nombre = Me.T_Nombre.Text
                .DNI = Me.T_DNI.Text
                .PrecioCoste = Me.T_PrecioCoste.Value
                .PrecioCosteHoraExtra = Me.T_PrecioCoste_HoraExtra.Value
                If IsDBNull(Me.T_HorasMinimas.Value) Then
                    .HorasMinimas = 0
                Else
                    .HorasMinimas = Me.T_HorasMinimas.Value
                End If

                If Me.C_Tipo.Value = 0 Then
                    .Personal_Tipo = Nothing
                Else
                    .Personal_Tipo = oDTC.Personal_Tipo.Where(Function(F) F.ID_Personal_Tipo = CInt(Me.C_Tipo.Value)).FirstOrDefault
                End If

                .Observaciones = Me.R_Observaciones.pText
                .NumTip = Me.T_NumTip.Text
                .FechaTip = Me.DT_FechaTip.Value
                .FechaAltaEmpresa = Me.DT_Alta.Value
                .FechaBajaEmpresa = Me.DT_Baja.Value
                .FechaAltaSS = Me.DT_FechaAltaSS.Value
                .FechaBajaSS = Me.DT_FechaBajaSS.Value
                .Cargo = Me.T_Cargo.Text
                .MotivoBaja = Me.T_MotivoBaja.Text
                .NumSS = Me.T_NumSS.Text

                .Direccion = Me.T_Direccion.Text
                .Poblacion = Me.T_Poblacion.Text
                .Provincia = Me.T_Provincia.Text
                .CP = Me.T_CodigoPostal.Text
                .Pais = Me.T_Pais.Text
                .TelefonoParticular = Me.T_TelefonoParticular.Text
                .MovilParticular = Me.T_MovilParticular.Text
                .CorreoElectronicoParticular = Me.T_EmailParticular.Text

                If Me.C_Sexo.Value = 0 Then
                    .Sexo = Nothing
                Else
                    .Sexo = oDTC.Sexo.Where(Function(F) F.ID_Sexo = CInt(Me.C_Sexo.Value)).FirstOrDefault
                End If

                If Me.C_Nacionalidad.Value = 0 Then
                    .Nacionalidad = Nothing
                Else
                    .Nacionalidad = oDTC.Nacionalidad.Where(Function(F) F.ID_Nacionalidad = CInt(Me.C_Nacionalidad.Value)).FirstOrDefault
                End If

                .FechaNacimiento = Me.DT_Nacimiento.Value

                If Me.C_EstadoCivil.Value = 0 Then
                    .EstadoCivil = Nothing
                Else
                    .EstadoCivil = oDTC.EstadoCivil.Where(Function(F) F.ID_EstadoCivil = CInt(Me.C_EstadoCivil.Value)).FirstOrDefault
                End If

                .Telefono = Me.T_Telefono.Text
                .Extension = Me.T_Extension.Text
                .Movil = Me.T_Movil.Text
                .CorreoElectronico = Me.T_Email.Text
                .PersonalExterno = Me.CH_PersonalExterno.Checked
                .OcultarEnListados = Me.CH_OcultarEnListados.Checked
                .ActivarCalendario = Me.CH_ActivarCalendario.Checked
                .VerSoloClientesDondeSeTengaPermisos = Me.CH_VerSoloClientesDondeSeTengaPermisos.Checked
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqPersonal = (From taula In oDTC.Personal Where taula.ID_Personal = pID Select taula).First

            If ComprovarPermisosSeguretat() = False Then
                Exit Sub
            End If

            Call SetToForm()

            Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeCargar)

            Me.EstableixCaptionForm("Personal: " & (oLinqPersonal.Nombre))

            Call CargarGrid_AsignacionPersonal(oLinqPersonal.ID_Personal)
            Call CargarGrid_HorasRealizadas(oLinqPersonal.ID_Personal)
            Call CargaGrid_Seguridad(oLinqPersonal.ID_Personal)
            Call CargaGrid_AutomatismosActividad(oLinqPersonal.ID_Personal)

            If oLinqPersonal.Almacen.Count <> 0 Then
                Call CargarGrid_Stocks(oLinqPersonal.Almacen.FirstOrDefault.ID_Almacen)
            End If

            If oLinqPersonal.FechaBajaEmpresa.HasValue = True Then
                Me.L_FechaBaja.Visible = True
                Me.DT_Baja.Visible = True
                Me.L_Baja.Visible = True
                Me.T_MotivoBaja.Visible = True
            Else
                Me.L_FechaBaja.Visible = False
                Me.DT_Baja.Visible = False
                Me.L_Baja.Visible = False
                Me.T_MotivoBaja.Visible = False

            End If

            Fichero.Cargar_GRID(pID)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqPersonal = New Personal
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
        Me.DT_Alta.Value = Now.Date
        'Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing
        Call CargarGrid_AsignacionPersonal(0)
        Call CargarGrid_HorasRealizadas(0)
        Call CargaGrid_Seguridad(0)
        Call CargarGrid_Stocks(0)
        Call CargaGrid_Delegaciones(0)
        Call CargaGrid_AutomatismosActividad(0)

        Fichero.Cargar_GRID(0)
        Me.UltraPictureBox1.Image = Nothing
        Me.C_Sexo.SelectedIndex = -1
        Me.C_Nacionalidad.SelectedIndex = -1
        Me.C_EstadoCivil.SelectedIndex = -1

        ErrorProvider1.Clear()
        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Call ActivarSegonsEstatPantalla(EnumEstatPantalla.Nuevo)
        Me.TAB_Principal.Tabs("General").Selected = True

        Me.L_FechaBaja.Visible = False
        Me.DT_Baja.Visible = False
        Me.L_Baja.Visible = False
        Me.T_MotivoBaja.Visible = False

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.T_Nombre)
                oClsControls.ControlBuit(.T_DNI)
                oClsControls.ControlBuit(.T_PrecioCoste)
                oClsControls.ControlBuit(.T_PrecioCoste_HoraExtra)
                oClsControls.ControlBuit(.T_NumSS)
                oClsControls.ControlBuit(.C_Nacionalidad)
                oClsControls.ControlBuit(.C_Sexo)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Personal Where Activo=1 ORDER BY Nombre", Me.TE_Codigo, "ID_Personal", "ID_Personal")
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
            If oLinqPersonal.Personal_Seguridad.Count = 0 Then
                Return True
            End If

            'Si te nivell de seguretat 1 te accés accessible
            If Seguretat.oUser.NivelSeguridad = 1 Then
                Return True
            End If

            'si esta dins de la llista te accés
            If oLinqPersonal.Personal_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count = 1 Then
                Return True
            End If

            Mensaje.Mostrar_Mensaje("Imposible cargar los datos, no tiene permisos", M_Mensaje.Missatge_Modo.INFORMACIO)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub CargarFoto()
        If oLinqPersonal.ID_Archivo.HasValue = True Then
            Me.UltraPictureBox1.Image = Util.BinaryToImage(oLinqPersonal.Archivo.CampoBinario.ToArray)
            ' Me.UltraPictureBox1.Image = Image.FromFile(Fichero.ExtreuIRetornaRutaArxiu(oLinqProducto.ID_Archivo_FotoPredeterminada))
        Else
            Me.UltraPictureBox1.Image = Nothing
        End If

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
                Dim ooLinqPersonal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.TE_Codigo.Value)).FirstOrDefault()
                If ooLinqPersonal Is Nothing = False Then
                    Call Cargar_Form(ooLinqPersonal.ID_Personal)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If
            Else
                If oDTC.Personal.Count > 0 Then
                    Me.TE_Codigo.Value = oDTC.Personal.Where(Function(F) F.Activo = True).Max(Function(F) F.ID_Personal) + 1
                Else
                    Me.TE_Codigo.Value = 1
                End If
            End If
        End If
    End Sub

    Private Sub TAB_Principal_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
        Select Case e.Tab.Key
            Case "General"
                Call CargarFoto()
            Case "Inmigracion"
                Call CargaGrid_Inmigracion(oLinqPersonal.ID_Personal)
            Case "Retribuciones"
                Call CargaGrid_Retribuciones(oLinqPersonal.ID_Personal)
            Case "Bajas"
                Call CargaGrid_Bajas(oLinqPersonal.ID_Personal)
            Case "Idioma"
                Call CargaGrid_Idiomas(oLinqPersonal.ID_Personal)
            Case "Formacion"
                Call CargaGrid_Formacion(oLinqPersonal.ID_Personal)
            Case "ExperienciaLaboral"
                Call CargaGrid_ExperienciaLaboral(oLinqPersonal.ID_Personal)
            Case "DatosFamiliares"
                Call CargaGrid_DatosFamiliares(oLinqPersonal.ID_Personal)
            Case "CuentasBancarias"
                Call CargaGrid_CuentasBancarias(oLinqPersonal.ID_Personal)
            Case "Incidencias"
                Call CargaGrid_Incidencias(oLinqPersonal.ID_Personal)
            Case "Ausencias"
                Call CargaGrid_Ausencias(oLinqPersonal.ID_Personal)
            Case "PersonalACargo"
                Call CargaGrid_PersonalACargo(oLinqPersonal.ID_Personal)
            Case "Delegaciones"
                Call CargaGrid_Delegaciones(oLinqPersonal.ID_Personal)
            Case "Emails"
                Call CargaGrid_Emails(oLinqPersonal.ID_Personal)
            Case "Empresas"
                Call CargaGrid_Empresas(oLinqPersonal.ID_Personal)
        End Select
    End Sub

    Private Sub GRD_Ficheros_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Ficheros.M_ToolGrid_ToolClickBotonsExtras
        Try
            If e.Tool.Key = "FotoPredeterminada" AndAlso Me.GRD_Ficheros.GRID.Selected.Rows.Count = 1 Then
                Dim _IDArchivo As Integer = Me.GRD_Ficheros.GRID.Selected.Rows(0).Cells("ID_Archivo").Value
                Dim _Archivo As Archivo
                _Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = _IDArchivo).FirstOrDefault
                If _Archivo.Tipo.ToString.ToLower.Contains("image") = True Or _Archivo.Tipo.ToString.ToLower.Contains("jpg") = True Or _Archivo.Tipo.ToString.ToLower.Contains("jpeg") = True Or _Archivo.Tipo.ToString.ToLower.Contains("png") = True Or _Archivo.Tipo.ToString.ToLower.Contains("tiff") = True Or _Archivo.Tipo.ToString.ToLower.Contains("gif") = True Or _Archivo.Tipo.ToString.ToLower.Contains("bmp") = True Then
                Else
                    Mensaje.Mostrar_Mensaje("Sólo se pueden predeterminar ficheros de tipo imagen", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
                oLinqPersonal.Archivo = _Archivo
                oDTC.SubmitChanges()
                Call DespresDeCarregarDadesGridArchivos()
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub DespresDeCarregarDadesGridArchivos()
        Try
            If Me.GRD_Ficheros.GRID.Rows.Count > 0 And oLinqPersonal.ID_Archivo.HasValue = True Then
                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Ficheros.GRID.Rows
                    If pRow.Cells("ID_Archivo").Value = oLinqPersonal.ID_Archivo Then
                        pRow.CellAppearance.BackColor = Color.LightGreen
                    Else
                        pRow.CellAppearance.BackColor = Color.White
                    End If
                Next
                Me.GRD_Ficheros.GRID.ActiveRow = Nothing
                Me.GRD_Ficheros.GRID.Selected.Rows.Clear()
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Assignacion Partes"

    Private Sub CargarGrid_AsignacionPersonal(ByVal pID As Integer)
        Try

            Me.GRD_AsignacionPartes.M.clsUltraGrid.Cargar("Select * From C_Personal_Asignacion Where ID_Personal=" & oLinqPersonal.ID_Personal & " Order by FechaAlta", BD)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_AsignacionPartes_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_AsignacionPartes.M_ToolGrid_ToolVisualitzarDobleClickRow
        Try
            If Me.GRD_AsignacionPartes.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If
            If Me.GRD_AsignacionPartes.GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                frm.Entrada(Me.GRD_AsignacionPartes.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
                frm.FormObrir(Me, True)
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "HoresRealitzades"

    Private Sub CargarGrid_HorasRealizadas(ByVal pID As Integer)
        Try

            With GRD_HorasRealizadas
                '.GRID.DataSource = Nothing

                .M.clsUltraGrid.Cargar("Select * From C_Personal_HorasRealizadas Where ID_Personal=" & pID & " Order by Fecha Desc", BD)

                'If .GRID.DisplayLayout.Bands(0).Summaries.Count = 0 Then
                '    .GRID.DisplayLayout.Bands(0).Summaries.Add("Horas", Infragistics.Win.UltraWinGrid.SummaryType.Sum, .GRID.DisplayLayout.Bands(0).Columns("Horas"), Infragistics.Win.UltraWinGrid.SummaryPosition.UseSummaryPositionColumn)
                '    .GRID.DisplayLayout.Bands(0).Summaries("Horas").Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                '    .GRID.DisplayLayout.Bands(0).Summaries("Horas").DisplayFormat = "{0:#,##0.00}"
                '    .GRID.DisplayLayout.Bands(0).Summaries("Horas").Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True

                '    .GRID.DisplayLayout.Bands(0).Summaries.Add("HorasExtras", Infragistics.Win.UltraWinGrid.SummaryType.Sum, .GRID.DisplayLayout.Bands(0).Columns("HorasExtras"), Infragistics.Win.UltraWinGrid.SummaryPosition.UseSummaryPositionColumn)
                '    .GRID.DisplayLayout.Bands(0).Summaries("HorasExtras").Appearance.TextHAlign = Infragistics.Win.HAlign.Right
                '    .GRID.DisplayLayout.Bands(0).Summaries("HorasExtras").DisplayFormat = "{0:#,##0.00}"
                '    .GRID.DisplayLayout.Bands(0).Summaries("HorasExtras").Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True
                'End If
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_HorasRealizadas_M_ToolGrid_ToolActualizar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_HorasRealizadas.M_ToolGrid_ToolActualizar
        Call CargarGrid_HorasRealizadas(oLinqPersonal.ID_Personal)
    End Sub

    Private Sub GRD_HorasRealizadas_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_HorasRealizadas.M_ToolGrid_ToolClickBotonsExtras
        Try
            If e.Tool.Key = "HorasMalIntroducidas" Then
                If Me.GRD_HorasRealizadas.GRID.Selected.Rows.Count > 0 Then
                    Dim pRow As UltraGridRow = Me.GRD_HorasRealizadas.GRID.Selected.Rows(0)
                    Dim _Horas As Parte_Horas
                    _Horas = oLinqPersonal.Parte_Horas.Where(Function(F) F.ID_Parte_Horas = pRow.Cells("ID_Parte_Horas").Value).FirstOrDefault()
                    _Horas.ObservacionesIncorrecto = Mensaje.Mostrar_Entrada_Datos("Introduzca las observaciones del parte incorrecto:", "", True, , True)
                    _Horas.ID_Parte_Horas_Estado = EnumParteHorasEstado.Incorrecto
                    pRow.Cells("ObservacionesIncorrecto").Value = _Horas.ObservacionesIncorrecto
                    oDTC.SubmitChanges()
                    Mensaje.Mostrar_Mensaje("Datos guardados correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                End If
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_HorasRealizadas_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_HorasRealizadas.M_ToolGrid_ToolVisualitzarDobleClickRow
        Try
            If Me.GRD_HorasRealizadas.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If
            If Me.GRD_HorasRealizadas.GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                frm.Entrada(Me.GRD_HorasRealizadas.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
                frm.FormObrir(Me, True)
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Seguridad"

    Private Sub CargaGrid_Seguridad(ByVal pId As Integer)
        Try
            Dim _Seguretat As IEnumerable(Of Personal_Seguridad) = From taula In oDTC.Personal_Seguridad Where taula.ID_Personal = pId Select taula

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
            If oLinqPersonal.Personal_Seguridad.Where(Function(F) F.Usuario Is _Usuario).Count <> 0 Then
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

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguridad_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Seguridad.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Seguridad.Remove(e.ListObject)
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

#Region "Grid Inmigración"

    Private Sub CargaGrid_Inmigracion(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Inmigracion) = From taula In oDTC.Personal_Inmigracion Where taula.ID_Personal = pId Select taula

            With Me.GRD_Inmigracion
                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

                Call CargarCombo_TipoDocumento(.GRID)
                Call CargarCombo_Estado(.GRID)
                Call CargarCombo_EmitidoPor(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Inmigracion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Inmigracion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Inmigracion

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Inmigracion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Inmigracion.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Inmigracion.Remove(e.ListObject)
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

    Private Sub GRD_Inmigracion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Inmigracion.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_TipoDocumento(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal_Inmigracion_TipoDocumento) = (From Taula In oDTC.Personal_Inmigracion_TipoDocumento Order By Taula.Descripcion Select Taula)

            Dim Var As Personal_Inmigracion_TipoDocumento
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal_Inmigracion_TipoDocumento").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal_Inmigracion_TipoDocumento").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Estado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal_Inmigracion_Estado) = (From Taula In oDTC.Personal_Inmigracion_Estado Order By Taula.Descripcion Select Taula)

            Dim Var As Personal_Inmigracion_Estado
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal_Inmigracion_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal_Inmigracion_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_EmitidoPor(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal_Inmigracion_EmitidoPor) = (From Taula In oDTC.Personal_Inmigracion_EmitidoPor Order By Taula.Descripcion Select Taula)

            Dim Var As Personal_Inmigracion_EmitidoPor
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal_Inmigracion_EmitidoPor").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal_Inmigracion_EmitidoPor").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Retribuciones"

    Private Sub CargaGrid_Retribuciones(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Retribucion) = From taula In oDTC.Personal_Retribucion Where taula.ID_Personal = pId Select taula

            With Me.GRD_Retribuciones
                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Retribuciones_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Retribuciones.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Retribuciones

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Retribuciones_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Retribuciones.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Retribucion.Remove(e.ListObject)
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

    Private Sub GRD_Retribuciones_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Retribuciones.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Bajas"

    Private Sub CargaGrid_Bajas(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Baja) = From taula In oDTC.Personal_Baja Where taula.ID_Personal = pId Select taula

            With Me.GRD_Bajas

                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("TotalDiasBaja").CellActivation = Activation.Disabled

                Call CargarCombo_Tipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Bajas_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Bajas.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Bajas

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaInicio").Value = Now.Date
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Bajas_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Bajas.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Baja.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim _clsNewAnotacio As New clsAnotacioCalendariOperaris(oDTC)
                _clsNewAnotacio.EliminarAnotacioBaixa(e.ListObject)
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Bajas_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Bajas.M_GRID_AfterRowUpdate
        Dim _clsNewAnotacio As New clsAnotacioCalendariOperaris(oDTC)
        If e.Row.Cells("ID_Personal_Baja").Value = 0 Then
            _clsNewAnotacio.CrearAnotacioBaixa(e.Row.Cells("ID_Personal").Value, e.Row.Cells("FechaInicio").Value, e.Row.Cells("FechaFin").Value, e.Row.ListObject)
        Else
            _clsNewAnotacio.ModificarAnotacioBaixa(e.Row.ListObject)
        End If
        oDTC.SubmitChanges()
    End Sub




    Private Sub CargarCombo_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal_Baja_Tipo) = (From Taula In oDTC.Personal_Baja_Tipo Order By Taula.Descripcion Select Taula)

            Dim Var As Personal_Baja_Tipo
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal_Baja_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal_Baja_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Idiomas"

    Private Sub CargaGrid_Idiomas(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Idioma) = From taula In oDTC.Personal_Idioma Where taula.ID_Personal = pId Select taula

            With Me.GRD_Idioma
                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

                Call CargarCombo_Idioma(.GRID)
                Call CargarCombo_NivelAcademico(.GRID)
                Call CargarCombo_NivelEscrito(.GRID)
                Call CargarCombo_NivelHablado(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Idioma_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Idioma.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Idioma

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Idioma_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Idioma.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Idioma.Remove(e.ListObject)
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

    Private Sub GRD_Idioma_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Idioma.M_GRID_AfterRowUpdate
        If e.Row.Cells("Nivel").Value < 1 Or e.Row.Cells("Nivel").Value > 10 Then
            Mensaje.Mostrar_Mensaje("Imposible asignar un nivel  inferior a 1 o superior a 10", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            e.Row.Cells("Nivel").Value = e.Row.Cells("Nivel").OriginalValue
            e.Row.CancelUpdate()
            Exit Sub
        End If
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_Idioma(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Idioma) = (From Taula In oDTC.Idioma Order By Taula.Descripcion Select Taula)

            Dim Var As Idioma
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Idioma").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Idioma").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_NivelAcademico(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal_Idioma_NivelAcademico) = (From Taula In oDTC.Personal_Idioma_NivelAcademico Order By Taula.Descripcion Select Taula)

            Dim Var As Personal_Idioma_NivelAcademico
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal_Idioma_NivelAcademico").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal_Idioma_NivelAcademico").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_NivelEscrito(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal_Idioma_NivelEscrito) = (From Taula In oDTC.Personal_Idioma_NivelEscrito Order By Taula.Descripcion Select Taula)

            Dim Var As Personal_Idioma_NivelEscrito
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal_Idioma_NivelEscrito").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal_Idioma_NivelEscrito").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_NivelHablado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal_Idioma_NivelHablado) = (From Taula In oDTC.Personal_Idioma_NivelHablado Order By Taula.Descripcion Select Taula)

            Dim Var As Personal_Idioma_NivelHablado
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal_Idioma_NivelHablado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal_Idioma_NivelHablado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Formación"

    Private Sub CargaGrid_Formacion(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Formacion) = From taula In oDTC.Personal_Formacion Where taula.ID_Personal = pId Select taula

            With Me.GRD_Formacion
                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Formacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Formacion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Formacion

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Formacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Formacion.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Formacion.Remove(e.ListObject)
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

    Private Sub GRD_Formacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Formacion.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Experiencia Laboral"

    Private Sub CargaGrid_ExperienciaLaboral(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_ExperienciaLaboral) = From taula In oDTC.Personal_ExperienciaLaboral Where taula.ID_Personal = pId Select taula

            With Me.GRD_ExperienciaLaboral
                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ExperienciaLaboral_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ExperienciaLaboral.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_ExperienciaLaboral

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ExperienciaLaboral_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_ExperienciaLaboral.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_ExperienciaLaboral.Remove(e.ListObject)
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

    Private Sub GRD_ExperienciaLaboral_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_ExperienciaLaboral.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Datos Familiares"

    Private Sub CargaGrid_DatosFamiliares(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Familiar) = From taula In oDTC.Personal_Familiar Where taula.ID_Personal = pId Select taula

            With Me.GRD_DatosFamiliares
                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

                Call CargarCombo_RelacionFamiliar(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_DatosFamiliares_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_DatosFamiliares.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_DatosFamiliares

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_DatosFamiliares_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_DatosFamiliares.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Familiar.Remove(e.ListObject)
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

    Private Sub GRD_DatosFamiliares_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_DatosFamiliares.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_RelacionFamiliar(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal_Familiar_Relacion) = (From Taula In oDTC.Personal_Familiar_Relacion Order By Taula.Descripcion Select Taula)

            Dim Var As Personal_Familiar_Relacion
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal_Familiar_Relacion").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal_Familiar_Relacion").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Cuentas Bancarias"

    Private Sub CargaGrid_CuentasBancarias(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_CuentasBancarias) = From taula In oDTC.Personal_CuentasBancarias Where taula.ID_Personal = pId Select taula

            With Me.GRD_CuentasBancarias
                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CuentasBancarias_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_CuentasBancarias.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_CuentasBancarias

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CuentasBancarias_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_CuentasBancarias.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_CuentasBancarias.Remove(e.ListObject)
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

#End Region

#Region "Grid Incidencias"

    Private Sub CargaGrid_Incidencias(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Incidencia) = From taula In oDTC.Personal_Incidencia Where taula.ID_Personal = pId Select taula

            With Me.GRD_Incidencia

                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)
                '.M_Editable()
                Call CargarCombo_Cliente(Me.GRD_Incidencia.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub GRD_Incidencia_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Incidencia.M_ToolGrid_ToolAfegir
    '    Try

    '        With Me.GRD_Incidencia

    '            .M_AfegirFila()

    '            .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

    '        End With
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Incidencia_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Incidencia.M_ToolGrid_ToolEliminarRow
    '    Try

    '        If e.IsAddRow Then
    '            oLinqPersonal.Personal_Incidencia.Remove(e.ListObject)
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

    'Private Sub GRD_Incidencia_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Incidencia.M_GRID_AfterRowUpdate
    '    oDTC.SubmitChanges()
    'End Sub

    Private Sub CargarCombo_Cliente(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Cliente) = (From Taula In oDTC.Cliente Order By Taula.Nombre Select Taula)

            Dim Var As Cliente
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Cliente").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Cliente").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Incidencia_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Incidencia.M_ToolGrid_ToolAfegir
        Try
            If Guardar() = False Then
                Exit Sub
            End If

            Dim frm As New frmPersonal_Incidencia
            frm.Entrada(oLinqPersonal, oDTC)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmIncidencia
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Incidencia_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Incidencia.M_ToolGrid_ToolEditar
        Call GRD_Incidencia_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub GRD_Incidencia_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Incidencia.M_ToolGrid_ToolEliminar
        Try
            With Me.GRD_Incidencia


                If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then

                    Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                    If .GRID.Selected.Cells.Count > 0 Then
                        pRow = .GRID.Selected.Cells(0).Row
                    Else
                        pRow = .GRID.Selected.Rows(0)
                    End If

                    Dim ID As Integer = pRow.Cells("ID_Personal_Incidencia").Value
                    Dim _Linea As Personal_Incidencia = oLinqPersonal.Personal_Incidencia.Where(Function(F) F.ID_Personal_Incidencia = ID).FirstOrDefault()

                    oLinqPersonal.Personal_Incidencia.Remove(_Linea)
                    oDTC.Personal_Incidencia.DeleteOnSubmit(_Linea)


                    oDTC.SubmitChanges()

                    Call CargaGrid_Incidencias(oLinqPersonal.ID_Personal)

                End If
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Incidencia_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Incidencia.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_Incidencia
            If oLinqPersonal.ID_Personal = 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If

            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If Guardar() = False Then
                Exit Sub
            End If

            Dim frm As New frmPersonal_Incidencia
            frm.Entrada(oLinqPersonal, oDTC, .GRID.Selected.Rows(0).Cells("ID_Personal_Incidencia").Value)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmIncidencia
            frm.FormObrir(Me)
        End With
    End Sub

    Private Sub AlTancarfrmIncidencia()
        Call CargaGrid_Incidencias(oLinqPersonal.ID_Personal)
    End Sub
#End Region

#Region "Grid Ausencias"

    Private Sub CargaGrid_Ausencias(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Ausencia) = From taula In oDTC.Personal_Ausencia Where taula.ID_Personal = pId Select taula

            With Me.GRD_Ausencias

                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

                Call CargarCombo_AusenciaTipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Ausencias_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Ausencias.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Ausencias

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Fecha").Value = Now.Date
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Ausencias_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Ausencias.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Ausencia.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim _clsNewAnotacio As New clsAnotacioCalendariOperaris(oDTC)
                _clsNewAnotacio.EliminarAnotacioAusencia(e.ListObject)
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Ausencias_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Ausencias.M_GRID_AfterRowUpdate
        Dim _clsNewAnotacio As New clsAnotacioCalendariOperaris(oDTC)
        If e.Row.Cells("ID_Personal_Ausencia").Value = 0 Then
            _clsNewAnotacio.CrearAnotacioAusencia(e.Row.Cells("ID_Personal").Value, e.Row.Cells("Fecha").Value, e.Row.ListObject)
        Else
            _clsNewAnotacio.ModificarAnotacioAusencia(e.Row.ListObject)
        End If
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_AusenciaTipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal_Ausencia_Tipo) = (From Taula In oDTC.Personal_Ausencia_Tipo Order By Taula.Descripcion Select Taula)

            Dim Var As Personal_Ausencia_Tipo
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal_Ausencia_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal_Ausencia_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid personal a cargo"

    Private Sub CargaGrid_PersonalACargo(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_PersonalACargo) = From taula In oDTC.Personal_PersonalACargo Where taula.ID_Personal = pId Select taula

            With Me.GRD_PersonalACargo

                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

                'Call CargarCombo_Personal(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PersonalACargo_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_PersonalACargo.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_PersonalACargo

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PersonalACargo_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_PersonalACargo.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_PersonalACargo.Remove(e.ListObject)
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

    Private Sub GRD_PersonalACargo_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_PersonalACargo.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

 
    Private Sub CargarCombo_Personal(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef pCell As UltraWinGrid.UltraGridCell, ByVal pSoloPersonalSeleccionado As Boolean)
        Try
            Dim _Personal As Personal = pCell.Value
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal)
            If pSoloPersonalSeleccionado = False Then
                oTaula = (From Taula In oDTC.Personal Where (Taula.Activo = True And Taula.FechaBajaEmpresa.HasValue = False) Or (Taula Is _Personal) Order By Taula.Nombre Select Taula)
            Else
                oTaula = (From Taula In oDTC.Personal Where Taula Is _Personal Order By Taula.Nombre Select Taula)
            End If

            Dim Var As Personal

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pCell.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PersonalACargo_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_PersonalACargo.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "PersonalACargo" Then
            Call CargarCombo_Personal(sender, e.Cell, False)
        End If
    End Sub

    Private Sub GRD_PersonalACargo_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_PersonalACargo.M_Grid_InitializeRow
        If e.ReInitialize = False Then
            Call CargarCombo_Personal(Sender, e.Row.Cells("PersonalACargo"), True)
        End If
    End Sub

#End Region

#Region "Grid Stock"

    Private Sub CargarGrid_Stocks(ByVal pIDAlmacen As Integer)
        Try

            If pIDAlmacen = 0 Then
                Me.GRD_Stock.M.clsUltraGrid.Cargar(BD.RetornaEsquemaDataTable("Select * From RetornaStock(0," & pIDAlmacen & ") Order by ProductoDescripcion"))
            Else
                Me.GRD_Stock.M.clsUltraGrid.Cargar("Select * From RetornaStock(0," & pIDAlmacen & ") Order by ProductoDescripcion", BD)
            End If


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Stock_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Stock.M_GRID_DoubleClickRow2
        If Me.GRD_Stock.GRID.Selected.Rows.Count = 1 Then
            Dim frm As New frmProducto_Trazabilidad
            frm.Entrada(e.Cells("ID_Producto").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

#End Region

#Region "Grid Delegaciones"

    Private Sub CargaGrid_Delegaciones(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Delegacion) = From taula In oDTC.Personal_Delegacion Where taula.ID_Personal = pId Select taula

            With Me.GRD_Delegaciones

                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()

                Call CargarCombo_Delegaciones(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Delegacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Delegaciones.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Delegaciones

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Delegaciones_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Delegaciones.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Delegacion.Remove(e.ListObject)
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

    Private Sub GRD_Delegaciones_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Delegaciones.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_Delegaciones(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Delegacion) = (From Taula In oDTC.Delegacion Order By Taula.Descripcion Select Taula)

            Dim Var As Delegacion
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Delegacion").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Delegacion").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Delegaciones_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Delegaciones.M_GRID_CellListSelect
        Try
            If e.Cell.Column.Key <> "Delegacion" Then 'si no modifiquem el combo de personal no hi hem de fer re aqui
                Exit Sub
            End If

            'Comprovem que no s'hagi introduit aquest treballador abans
            Dim _Delegacion As Delegacion
            _Delegacion = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
            If oLinqPersonal.Personal_Delegacion.Where(Function(F) F.Delegacion Is _Delegacion).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar la misma delegación dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                e.Cell.CancelUpdate()
                Exit Sub
            End If


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Grid gestión de automatismos"

    Private Sub CargaGrid_AutomatismosActividad(ByVal pId As Integer)
        Try
            Dim _Asignacion As IEnumerable(Of Automatismo_Personal) = From taula In oDTC.Automatismo_Personal Where taula.ID_Personal = pId Select taula

            With Me.GRD_AvisosActividades

                '.GRID.DataSource = _Asignacion
                .M.clsUltraGrid.CargarIEnumerable(_Asignacion)

                'If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                .M_Editable()
                'Else
                '.GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                'End If

                Call CargarCombo_TiposAviso(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_AvisosActividades_M_GRID_AfterRowActivate(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_AvisosActividades.M_GRID_AfterRowActivate
        Call CargaGrid_Automatismo_Accion(Me.GRD_AvisosActividades.GRID.ActiveRow.Cells("ID_Automatismo").Value)
        Me.GRD_AvisosActividades.GRID.ActiveRow.Selected = True
    End Sub

    Private Sub GRD_Avisos_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_AvisosActividades.M_GRID_CellListSelect
        Try
            If e.Cell.Column.Key <> "Automatismo" Then 'si no modifiquem el combo de personal no hi hem de fer re aqui
                Exit Sub
            End If

            ''Comprovem que el registre que hi havia abans no tenia hores imputades
            Dim _Tipo As Automatismo = e.Cell.Value
            'If oLinqParte.Parte_Horas.Where(Function(F) F.Personal Is _Personal).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible desasignar el personal mientras tenga horas imputadas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    e.Cell.CancelUpdate()
            '    Exit Sub
            'End If

            'Comprovem que no s'hagi introduit aquest treballador abans
            _Tipo = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
            If oLinqPersonal.Automatismo_Personal.Where(Function(F) F.Automatismo Is _Tipo).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar el mismo automatismo dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                e.Cell.CancelUpdate()
                Exit Sub
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TiposAviso(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Automatismo) = (From Taula In oDTC.Automatismo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Automatismo

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Automatismo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Automatismo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Avisos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_AvisosActividades.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_AvisosActividades

                'If Guardar(False) = False Then
                '    Exit Sub
                'End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Avisos_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_AvisosActividades.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Automatismo_Personal.Remove(e.ListObject)
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

    Private Sub GRD_Avisos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_AvisosActividades.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid gestión de acciones de los automatismos"

    Private Sub CargaGrid_Automatismo_Accion(ByVal pId As Integer)
        Try
            '  Dim _Accion As IEnumerable(Of Automatismo_Accion_Personal) = From taula In oDTC.Automatismo_Accion_Personal Where taula.ID_Personal = oLinqPersonal.ID_Personal And taula.Automatismo_Accion.ID_Automatismo = pId Select taula

            With Me.GRD_AvisosAcciones
                ' .M.clsUltraGrid.CargarIEnumerable(_Accion)

                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select cast((Select Count(*) From Automatismo_Accion_Personal Where ID_Automatismo_Accion=C_Automatismo_Accion.ID_Automatismo_Accion and ID_Personal=" & oLinqPersonal.ID_Personal & ") as bit) as Seleccion, * From C_Automatismo_Accion Where ID_Automatismo=" & pId & " Order by Descripcion", True))


                '.GRID.DisplayLayout.Bands(0).Columns("Personal").CellActivation = UltraWinGrid.Activation.NoEdit
                .M_NoEditable()

                .GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellClickAction = CellClickAction.CellSelect
                .GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellActivation = Activation.AllowEdit
                '   Call CargarCombo_Acciones(.GRID, pId)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub CargarCombo_Acciones(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pIDAutomatismo As Integer)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Automatismo_Accion) = (From Taula In oDTC.Automatismo_Accion Where Taula.ID_Automatismo = pIDAutomatismo Order By Taula.Descripcion Select Taula)
    '        Dim Var As Automatismo_Accion

    '        'Valors.ValueListItems.Add("-1", "Seleccione un registro")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Descripcion)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("Automatismo_Accion").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("Automatismo_Accion").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    Private Sub GRD_AvisosAcciones_M_GRID_AfterCellActivate(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_AvisosAcciones.M_GRID_AfterCellActivate
        With GRD_AvisosAcciones.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                .ActiveCell.Value = Not .ActiveCell.Value
                .ActiveCell.Row.Update()

                If .ActiveCell.Value = True Then
                    Dim _Accion As Automatismo_Accion
                    _Accion = oDTC.Automatismo_Accion.Where(Function(F) F.ID_Automatismo_Accion = CInt(.ActiveRow.Cells("ID_Automatismo_Accion").Value)).FirstOrDefault

                    Dim _Asignacio As New Automatismo_Accion_Personal
                    _Asignacio.Automatismo_Accion = _Accion
                    _Asignacio.Personal = oLinqPersonal
                    oLinqPersonal.Automatismo_Accion_Personal.Add(_Asignacio)
                    oDTC.SubmitChanges()
                Else
                    Dim _Asignacio As Automatismo_Accion_Personal = oDTC.Automatismo_Accion_Personal.Where(Function(F) F.ID_Automatismo_Accion = CInt(.ActiveRow.Cells("ID_Automatismo_Accion").Value) And F.ID_Personal = oLinqPersonal.ID_Personal).FirstOrDefault
                    ' oLinqPersonal.Automatismo_Accion_Personal.Remove(_Asignacio)
                    oDTC.Automatismo_Accion_Personal.DeleteOnSubmit(_Asignacio)
                    oDTC.SubmitChanges()
                End If

                .ActiveRow = Nothing
                .Selected.Rows.Clear()
                .ActiveCell = Nothing



            End If
        End With
    End Sub

    'Private Sub GRD_AvisosAcciones_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_AvisosAcciones.M_ToolGrid_ToolAfegir
    '    Try

    '        With Me.GRD_AvisosAcciones
    '            If oLinqPersonal.ID_Personal = 0 Then
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '                Exit Sub
    '            End If

    '            Dim _Automatismo As New Automatismo_Personal
    '            If Me.GRD_AvisosActividades.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_AvisosActividades.GRID.ActiveRow.Cells("ID_Automatismo").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
    '                Exit Sub
    '            Else
    '                _Automatismo = Me.GRD_AvisosActividades.GRID.Rows.GetItem(Me.GRD_AvisosActividades.GRID.ActiveRow.Index).listObject
    '            End If

    '            If _Automatismo.Automatismo.Automatismo_Accion.Count = 0 Then
    '                Mensaje.Mostrar_Mensaje("Imposible asignar acciones, este automatismo no tiene acciones disponibles", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                Exit Sub
    '            End If

    '            .M_ExitEditMode()
    '            .M_AfegirFila()

    '            .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

    '        End With
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_AvisosAcciones_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_AvisosAcciones.M_ToolGrid_ToolEliminarRow
    '    Try

    '        If e.IsAddRow Then
    '            e.Delete(False)
    '            'oLinqParte.Parte_Gastos.Remove(e.ListObject)
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

    'Private Sub GRD_AvisosAcciones_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_AvisosAcciones.M_GRID_AfterRowUpdate
    '    oDTC.SubmitChanges()
    'End Sub


#End Region

#Region "Grid Emails"

    Private Sub CargaGrid_Emails(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Personal_Emails) = From taula In oDTC.Personal_Emails Where taula.ID_Personal = pId Select taula

            With Me.GRD_Emails
                '.GRID.DataSource = _Dades
                .M.clsUltraGrid.CargarIEnumerable(_Dades)

                .M_Editable()


            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Email_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Emails.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Emails

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Email_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Emails.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Emails.Remove(e.ListObject)
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

    Private Sub GRD_Emails_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Emails.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Empresas"

    Private Sub CargaGrid_Empresas(ByVal pId As Integer)
        Try
            With Me.GRD_Empresas
                Dim Contactes As IEnumerable(Of Personal_Empresa) = From taula In oDTC.Personal_Empresa Where taula.ID_Personal = pId Select taula

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

                If oLinqPersonal.ID_Personal = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oLinqPersonal

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Empresas_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Empresas.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqPersonal.Personal_Empresa.Remove(e.ListObject)
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

End Class