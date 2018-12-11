Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports System.IO
Public Class frmInstalacion
    Implements IDisposable

    Public oDTC As DTCDataContext
    Public oLinqInstalacion As Instalacion
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2
    Dim oTmpFrm As frmPropuesta
    Public oTipoEntrada As TipoEntrada
    Dim oDTFotosProductos As New DataTable
    Dim _ArrayFoto As New ArrayList
    Dim _ArrayProducte As New ArrayList
    Dim ofrmPlano As frmPlano

    Enum TipoEntrada
        Instalacion = 1
        FuturaInstalacion = 2
    End Enum

    Public Structure StructOriginalClon
        Dim IDOriginal As Integer
        Dim IDClon As Integer
    End Structure

#Region "M_ToolForm"

    Private Sub ToolForm_m_ToolForm_Seleccionar() Handles ToolForm.m_ToolForm_Seleccionar
        If Mensaje.Mostrar_Mensaje("¿Desea dar de baja/alta la instalación?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
            Exit Sub
        End If

        If oLinqInstalacion.ID_Instalacion_Estado = EnumInstalacionEstado.Negativa Then
            oLinqInstalacion.Instalacion_Estado = oDTC.Instalacion_Estado.Where(Function(F) F.ID_Instalacion_Estado = CInt(EnumInstalacionEstado.Pendiente)).FirstOrDefault 'ho pasem a pendiente expressament pq la instrucció posterior ja ho arreglarà
            Call DeterminarEstatInstalacio()
        Else
            oLinqInstalacion.Instalacion_Estado = oDTC.Instalacion_Estado.Where(Function(F) F.ID_Instalacion_Estado = CInt(EnumInstalacionEstado.Negativa)).FirstOrDefault
            Call DeterminarEstatInstalacio()
        End If
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqInstalacion.ID_Instalacion <> 0 Then
                If oLinqInstalacion.Parte.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar, esta instalación tiene partes asignados", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If oLinqInstalacion.Propuesta.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar, esta instalación tiene propuestas asignadas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    ' Call Guardar()


                    oLinqInstalacion.Activo = False
                    oDTC.SubmitChanges()
                    Call Netejar_Pantalla()
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
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
        Informes.ObrirInformePreparacio(oDTC, EnumInforme.Instalacion, "[ID_Instalacion] = " & oLinqInstalacion.ID_Instalacion, "Instalación - " & oLinqInstalacion.ID_Instalacion)

    End Sub

    Private Sub M_ToolForm1_m_ToolForm_ToolClick_Botones_Extras(ByVal Sender As Object, ByVal e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        Try
            If oLinqInstalacion.ID_Instalacion = 0 Then
                Exit Sub
            End If

            If Guardar() = False Then
                Exit Sub
            End If

            Select Case e.Tool.Key
                Case "ConvertirAInstalacion"
                    If Mensaje.Mostrar_Mensaje("¿Estás seguro de querer convertir la futura instalación a una instalación?", M_Mensaje.Missatge_Modo.PREGUNTA, , , True) = M_Mensaje.Botons.NO Then
                        Exit Sub
                    End If

                    oLinqInstalacion.Instalacion_Tipo = oDTC.Instalacion_Tipo.Where(Function(F) F.ID_Instalacion_Tipo = CInt(EnumInstalacionTipo.Instalacion)).FirstOrDefault
                    oLinqInstalacion.Cliente.Cliente_Tipo = oDTC.Cliente_Tipo.Where(Function(F) F.ID_Cliente_Tipo = CInt(EnumClienteTipo.Cliente)).FirstOrDefault
                    oLinqInstalacion.Cliente_Contratante.Cliente_Tipo = oDTC.Cliente_Tipo.Where(Function(F) F.ID_Cliente_Tipo = CInt(EnumClienteTipo.Cliente)).FirstOrDefault

                    'Al pasar un futur client a client posarem al usuari que ho fa el permis que toca (només si no hi era)
                    If oLinqInstalacion.Cliente.Cliente_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count = 0 Then
                        Dim _Cliente As Cliente = oLinqInstalacion.Cliente
                        Dim _NewSeguretat As New Cliente_Seguridad
                        _NewSeguretat.Usuario = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault
                        _NewSeguretat.Cliente = _Cliente
                        _Cliente.Cliente_Seguridad.Add(_NewSeguretat)
                    End If

                    'Al pasar un futur client a client posarem al usuari que ho fa el permis que toca (només si no hi era)
                    If oLinqInstalacion.Cliente_Contratante.Cliente_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count = 0 Then
                        Dim _Cliente As Cliente = oLinqInstalacion.Cliente_Contratante
                        Dim _NewSeguretat As New Cliente_Seguridad
                        _NewSeguretat.Usuario = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault
                        _NewSeguretat.Cliente = _Cliente
                        _Cliente.Cliente_Seguridad.Add(_NewSeguretat)
                    End If

                    oDTC.SubmitChanges()
                    oTipoEntrada = TipoEntrada.Instalacion
                    Call Cargar_Form(oLinqInstalacion.ID_Instalacion)
                    Util.Tab_Visible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "ToDo", "RecepcionMateriales", "RevisionProductos", "SeInstalo", "Contrato", "Receptora", "Accesos", "Partes", "Step")
                    Me.ToolForm.ToolForm.Tools("ConvertirAInstalacion").SharedProps.Visible = False

            End Select


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)

        End Try
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pId As Integer = 0, Optional ByVal pTipoEntrada As TipoEntrada = TipoEntrada.Instalacion)
        Try
            oTipoEntrada = pTipoEntrada
            If Me.C_Estado.Items.Count = 0 Then
                Me.AplicarDisseny()


                Util.Cargar_Combo(Me.C_Estado, "SELECT ID_Instalacion_Estado, Descripcion FROM Instalacion_Estado WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Responsable, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and ID_Personal_Tipo in (" & EnumPersonalTipo.Comercial & " , " & EnumPersonalTipo.ResponsableCuenta & ") ORDER BY Nombre", False, True, "No asignado")
                Util.Cargar_Combo(Me.C_TipoNegocio, "SELECT ID_Sector, Descripcion FROM sector WHERE Activo=1 ORDER BY Descripcion", False, True, "No asignado")
                Util.Cargar_Combo(Me.C_Tipo_Documento, "SELECT ID_Entrada_Tipo, Descripcion FROM Entrada_Tipo Where Tipo='V' or Tipo='C' ORDER BY ID_Entrada_Tipo", False)
                Util.Cargar_Combo(Me.C_Pais, "SELECT ID_Pais, Nombre FROM Pais ORDER BY Nombre", True)
                Util.Cargar_Combo(Me.C_Delegacion, "SELECT ID_Delegacion, Descripcion FROM Delegacion ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Propuesta_Estado, "SELECT ID_Propuesta_Estado, Descripcion FROM Propuesta_Estado ORDER BY Descripcion", False, True, "Todos")

                Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Instalacion_Archivo", 1)
                AddHandler Fichero.DespresDeCarregarDades, AddressOf DespresDeCarregarDades
                'Me.C_Familia.ReadOnly = True
                'Me.C_Subfamilia.ReadOnly = True
                'Me.C_Marca.ReadOnly = True

                Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("InstalacionAnterior", "Instalación anterior", True)
                Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("Aprobar", "Aprobar", True)
                Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("Traspasar", "Traspasar", True)
                Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("VerParte", "Ver parte de instalación", True)
                Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("NegativoPositivo", "Negativo / Positivo", True)
                'Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("DuplicarPropuesta", "Duplicar", True)
                'Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("DuplicarPropuestaEnOtraInstalacion", "Duplicar en otra instalación", True)
                'Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("GenerarPedidoVenta", "Generar pedido", True)
                'Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("GenerarParte", "Generar parte", True)

                AddHandler Preview_Excel.RibbonControl1.ItemClick, AddressOf AlApretarElBotoGuardarDelExcel
                Me.Preview_RTF.pBotoGuardarVisible = True

                Dim _ToolAccions As New Infragistics.Win.UltraWinToolbars.PopupMenuTool("Accions")
                Dim _ToolAccionsFill1 As New Infragistics.Win.UltraWinToolbars.ButtonTool("DuplicarPropuesta")
                Dim _ToolAccionsFill2 As New Infragistics.Win.UltraWinToolbars.ButtonTool("DuplicarPropuestaEnOtraInstalacion")
                Dim _ToolAccionsFill3 As New Infragistics.Win.UltraWinToolbars.ButtonTool("GenerarPedidoVenta")
                Dim _ToolAccionsFill4 As New Infragistics.Win.UltraWinToolbars.ButtonTool("GenerarParte")
                Dim _ToolAccionsFill5 As New Infragistics.Win.UltraWinToolbars.ButtonTool("MoverPresupuesto")
                Dim _ToolAccionsFill6 As New Infragistics.Win.UltraWinToolbars.ButtonTool("FusionarPresupuestos")

                Me.GRD_Propuesta.ToolGrid.Tools.Add(_ToolAccions)
                Me.GRD_Propuesta.ToolGrid.Tools.Add(_ToolAccionsFill1)
                Me.GRD_Propuesta.ToolGrid.Tools.Add(_ToolAccionsFill2)
                Me.GRD_Propuesta.ToolGrid.Tools.Add(_ToolAccionsFill3)
                Me.GRD_Propuesta.ToolGrid.Tools.Add(_ToolAccionsFill4)
                Me.GRD_Propuesta.ToolGrid.Tools.Add(_ToolAccionsFill5)
                Me.GRD_Propuesta.ToolGrid.Tools.Add(_ToolAccionsFill6)
                Me.GRD_Propuesta.ToolGrid.Toolbars(0).Tools.AddTool("Accions")

                _ToolAccions.SharedProps.Caption = "Acciones"
                _ToolAccionsFill1.SharedProps.Caption = "Duplicar"
                _ToolAccionsFill2.SharedProps.Caption = "Duplicar en otra instalación"
                _ToolAccionsFill3.SharedProps.Caption = "Generar pedido"
                _ToolAccionsFill4.SharedProps.Caption = "Generar parte"
                _ToolAccionsFill5.SharedProps.Caption = "Mover presupuesto"
                _ToolAccionsFill6.SharedProps.Caption = "Fusionar presupuestos"

                _ToolAccions.Tools.AddTool("DuplicarPropuesta")
                _ToolAccions.Tools.AddTool("DuplicarPropuestaEnOtraInstalacion")
                _ToolAccions.Tools.AddTool("GenerarPedidoVenta")
                _ToolAccions.Tools.AddTool("GenerarParte")
                _ToolAccions.Tools.AddTool("MoverPresupuesto")
                _ToolAccions.Tools.AddTool("FusionarPresupuestos")
                _ToolAccions.SharedProps.DisplayStyle = UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways
                _ToolAccionsFill1.SharedProps.DisplayStyle = UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways
                _ToolAccionsFill2.SharedProps.DisplayStyle = UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways
                _ToolAccionsFill3.SharedProps.DisplayStyle = UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways
                _ToolAccionsFill4.SharedProps.DisplayStyle = UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways
                _ToolAccionsFill5.SharedProps.DisplayStyle = UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways
                _ToolAccionsFill6.SharedProps.DisplayStyle = UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways

                _ToolAccions.SharedProps.AppearancesSmall.Appearance.ForeColor = Color.White


                Me.GRD_SeInstalo.M.clsToolBar.Boto_Afegir("ModificarInstalacion", "Modificar instalación", True)
                'Me.GRD_SeInstalo.ToolGrid.Tools("ModificarInstalacion").SharedProps.AppearancesSmall.Appearance.FontData.Bold = DefaultableBoolean.True
                'Me.GRD_SeInstalo.ToolGrid.Tools("ModificarInstalacion").SharedProps.AppearancesSmall.Appearance.FontData.Underline = DefaultableBoolean.True
                Me.GRD_SeInstalo.M.clsToolBar.Boto_Afegir("VistaIndentada", "Ocultar vista indentada", True)

                Me.GRD_SeInstalo.M.clsToolBar.Boto_Afegir("VisualizarPartes", "Visualizar partes", True)
                Me.GRD_SeInstalo.M.clsToolBar.Boto_Afegir("GenerarParte", "Generar parte de reparación", True)
                'Me.GRD_SeInstalo.M.clsToolBar.Boto_Afegir("VisualizarFotos", "Visualizar fotos", True)
                Me.GRD_SeInstalo.M.clsToolBar.Boto_Afegir("VerProducto", "Ver ficha producto", True)
                Me.GRD_SeInstalo.M.clsToolBar.Boto_Afegir("IrAlbaran", "Ver albarán", True)
                Me.GRD_Cableado.M.clsToolBar.Boto_Afegir("AsignarCableado", "Asignar cableado", True)
                Me.GRD_Planos.M.clsToolBar.Boto_Afegir("Plano", "Planos/Diagramas", True)
                Me.GRD_Planos.M.clsToolBar.Boto_Afegir("Duplicar", "Duplicar", True)
                Me.GRD_ProductosEliminados.M.clsToolBar.Boto_Afegir("RecuperarProducto", "Recuperar Producto", True)
                Me.GRD_Productos_PendientesAprobacion.M.clsToolBar.Boto_Afegir("AprobarProducto", "Aprobar Producto", True)
                Me.GRD_Productos_PendientesAprobacion.M.clsToolBar.Boto_Afegir("AprobarTodosProducto", "Aprobar todos los Producto", True)
                Me.GRD_RecepcionMateriales.M.clsToolBar.Boto_Afegir("RecepcionarTodo", "Recepcionar todo")
                Me.GRD_Planta.M.clsToolBar.Boto_Afegir("Multiplicar", "Multiplicar", True)
                Me.GRD_Zona.M.clsToolBar.Boto_Afegir("Multiplicar", "Multiplicar", True)
                Me.GRD_Abertura.M.clsToolBar.Boto_Afegir("Multiplicar", "Multiplicar", True)
                Me.GRD_Cables.M.clsToolBar.Boto_Afegir("Multiplicar", "Multiplicar", True)
                Me.GRD_CajasIntermedias.M.clsToolBar.Boto_Afegir("Multiplicar", "Multiplicar", True)

                Me.GRD_ToDo.M.clsToolBar.Boto_Afegir("IrAlParte", "Ir al parte", True)

                Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Visible = True
                Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True


                ''            Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("ajuda", "Ajuda", True)
                ''            Me.ToolForm.M.Botons.tAccions.SharedProps.Visible = True
                ''            Me.T_CP.MaxLength = 5
                ''            'Util.DesActivar(Me.GRP_Personal, M_Util.Enum_Controles_Activacion.TODOS, False)

                Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = True
                Me.TL_Cliente.ButtonsRight("Ficha").Enabled = True
                Me.TL_Cliente_Contratante.ButtonsRight("Lupeta").Enabled = True
                Me.TL_Cliente_Contratante.ButtonsRight("Ficha").Enabled = True
                Me.TE_BusquedaInterconexiones.ButtonsRight("Lupeta").Enabled = True
                Me.TE_BusquedaCajaIntermedia.ButtonsRight("Lupeta").Enabled = True

                Dim BotoCancelar As UltraWinEditors.EditorButton
                BotoCancelar = New UltraWinEditors.EditorButton
                BotoCancelar.Key = "Cancelar"
                Dim oDisseny As New M_Disseny.ClsDisseny
                BotoCancelar.Appearance.Image = oDisseny.Leer_Imagen("text_cancelar.gif")
                BotoCancelar.Width = 16
                BotoCancelar.Appearance.BackColor = Color.White
                BotoCancelar.Appearance.BorderAlpha = Alpha.Transparent
                Me.TE_BusquedaInterconexiones.ButtonsRight.Add(BotoCancelar.Clone)
                Me.TE_BusquedaCajaIntermedia.ButtonsRight.Add(BotoCancelar.Clone)
                Me.KeyPreview = False



                Select Case pTipoEntrada
                    Case TipoEntrada.Instalacion
                        'If Me.ToolForm.ToolForm.Tools.Exists("ConvertirAInstalacion") = True Then

                        'End If
                    Case TipoEntrada.FuturaInstalacion
                        Util.Tab_InVisible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "ToDo", "RecepcionMateriales", "RevisionProductos", "SeInstalo", "Contrato", "Receptora", "Accesos", "Partes", "Step")
                        Me.GRD_Propuesta.M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.Invisible, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "Aprobar", "Traspasar", "VerParte", "GenerarPedidoVenta", "GenerarParte")
                        'If Me.ToolForm.ToolForm.Tools.Exists("ConvertirAInstalacion") = False Then
                        Me.ToolForm.M.clsToolBar.Afegir_Boto("ConvertirAInstalacion", "Convertir a instalación", True)
                        Me.ToolForm.ToolForm.Tools("ConvertirAInstalacion").SharedProps.DisplayStyle = UltraWinToolbars.ToolDisplayStyle.ImageAndText
                        Dim _Imatges As New M_Global.M_Images
                        Me.ToolForm.ToolForm.Tools("ConvertirAInstalacion").SharedProps.AppearancesSmall.Appearance.Image = _Imatges.SharedImageCollection1.ImageSource.Images("centroid.png")
                        'End If
                        'Util.Tab_InVisible_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "FormaPago")
                End Select
            End If


            If pId <> 0 Then
                Call Cargar_Form(pId)
            Else
                Call Netejar_Pantalla()
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Function Guardar(Optional ByVal pSenseMissatge As Boolean = False) As Boolean
        Guardar = False
        Try
            Me.TE_Codigo.Focus()
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

            Call GetFromForm(oLinqInstalacion)

            If oLinqInstalacion.ID_Instalacion = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Instalacion.InsertOnSubmit(oLinqInstalacion)

                oDTC.SubmitChanges()

                Me.TE_Codigo.Text = oLinqInstalacion.ID_Instalacion
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                'Util.Tab_Activar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, )
                Call Fichero.Cargar_GRID(oLinqInstalacion.ID_Instalacion) 'Fem això pq la classe tingui el ID de pressupost
                Call ActivarTabsEnFuncioDelEstatDeLaInstalacio()

                'Guardar el usuari actual en els permisos de seguretat 
                Dim _NewSeguretat As New Instalacion_Seguridad
                _NewSeguretat.Usuario = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault
                oLinqInstalacion.Instalacion_Seguridad.Add(_NewSeguretat)
                oDTC.SubmitChanges()
                Call CargaGrid_Seguridad(oLinqInstalacion.ID_Instalacion)

            Else
            oDTC.SubmitChanges()
            If pSenseMissatge = False Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If
            End If

            'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqInstalacion
                Me.TE_Codigo.Text = .ID_Instalacion
                Me.TL_Cliente.Tag = .ID_Cliente
                Me.TL_Cliente.Text = .Cliente.Nombre
                Me.TL_Cliente_Contratante.Tag = .ID_Cliente_Contratante
                Me.TL_Cliente_Contratante.Text = .Cliente_Contratante.Nombre
                Me.C_Estado.Value = .ID_Instalacion_Estado
                Me.T_Direccion.Text = .Direccion
                Me.T_Poblacion.Text = .Poblacion
                Me.T_Provincia.Text = .Provincia
                Me.T_Telefono.Text = .Telefono
                Me.T_Persona_Contacto.Text = .PersonaContacto
                Me.T_Email.Text = .Email
                Me.T_Otros_Detalles.Text = .OtrosDetalles
                Me.DT_FechaInstalacion.Value = .FechaInstalacion
                Me.R_DescripcionDetallada.pText = .DescripcionDetallada
                Me.R_Observaciones.pText = .Observaciones
                Me.R_SeInstalo_OtrosDetalles.pText = .SeInstalo_OtrosDetalles
                Me.T_Longitud.Value = .Longitud
                Me.T_Latitud.Value = .Latitud
                Me.T_CP.Value = .CP
                Me.C_Pais.Value = .ID_Pais

                If .ID_Delegacion.HasValue = False Then
                    Me.C_Delegacion.SelectedIndex = -1
                Else
                    Me.C_Delegacion.Value = .ID_Delegacion
                End If

                If IsNothing(.ID_Personal) Then
                    Me.C_Responsable.Value = 0
                Else
                    Me.C_Responsable.Value = .ID_Personal
                End If

                If IsNothing(.ID_Sector) Then
                    Me.C_TipoNegocio.Value = 0
                Else
                    Me.C_TipoNegocio.Value = .ID_Sector
                End If

                If .Hoja Is Nothing = False Then
                    Me.Excel1.M_LoadDocument(.Hoja)
                End If

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pInstalacion As Instalacion)
        Try
            With pInstalacion
                .Activo = True

                .Instalacion_Tipo = oDTC.Instalacion_Tipo.Where(Function(F) F.ID_Instalacion_Tipo = CInt(oTipoEntrada)).FirstOrDefault
                .Cliente = (From Taula In oDTC.Cliente Where Taula.ID_Cliente = CInt(Me.TL_Cliente.Tag) Select Taula).First
                .Cliente_Contratante = (From Taula In oDTC.Cliente Where Taula.ID_Cliente = CInt(Me.TL_Cliente_Contratante.Tag) Select Taula).First
                .Instalacion_Estado = (From Taula In oDTC.Instalacion_Estado Where Taula.ID_Instalacion_Estado = CInt(Me.C_Estado.Value) Select Taula).First
                .Direccion = Me.T_Direccion.Text
                .Poblacion = Me.T_Poblacion.Text
                .Provincia = Me.T_Provincia.Text
                .Telefono = Me.T_Telefono.Text
                .PersonaContacto = Me.T_Persona_Contacto.Text
                .Email = Me.T_Email.Text
                .OtrosDetalles = Me.T_Otros_Detalles.Text
                .FechaInstalacion = Me.DT_FechaInstalacion.Value

                .DescripcionDetallada = Me.R_DescripcionDetallada.pText
                .Observaciones = Me.R_Observaciones.pText
                .SeInstalo_OtrosDetalles = Me.R_SeInstalo_OtrosDetalles.pText

                .Longitud = Util.DbnullToNothing(Me.T_Longitud.Value)
                .Latitud = Util.DbnullToNothing(Me.T_Latitud.Value)

                .CP = Me.T_CP.Value
                .Pais = oDTC.Pais.Where(Function(F) F.ID_Pais = CInt(Me.C_Pais.Value)).FirstOrDefault

                If Me.C_Delegacion.Value = 0 Then
                    .Delegacion = Nothing
                Else
                    .Delegacion = oDTC.Delegacion.Where(Function(F) F.ID_Delegacion = CInt(Me.C_Delegacion.Value)).FirstOrDefault
                End If

                If Me.C_Responsable.Value = 0 Then
                    .Personal = Nothing
                Else
                    .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_Responsable.Value)).FirstOrDefault
                End If

                If Me.C_TipoNegocio.Value = 0 Then
                    .Sector = Nothing
                Else
                    .Sector = oDTC.Sector.Where(Function(F) F.ID_Sector = CInt(Me.C_TipoNegocio.Value)).FirstOrDefault
                End If


                .Hoja = Me.Excel1.M_SaveDocument.ToArray

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer, Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        Try
            Call Netejar_Pantalla(pNoCanviarALaPestanyaGeneral)

            oLinqInstalacion = (From taula In oDTC.Instalacion Where taula.ID_Instalacion = pID Select taula).First

            If ComprovarPermisosSeguretat() = False Then
                oLinqInstalacion = Nothing
                Exit Sub
            End If

            Call SetToForm()

            Call CargaGrid_Propuesta(pID)
            Fichero.Cargar_GRID(pID)

            Call ActivarTabsEnFuncioDelEstatDeLaInstalacio()


            If oTipoEntrada = TipoEntrada.Instalacion Then
                Me.EstableixCaptionForm("Instalación: " & (oLinqInstalacion.ID_Instalacion) & " - " & oLinqInstalacion.Cliente.Nombre)
            Else
                Me.EstableixCaptionForm("Futura instalación: " & (oLinqInstalacion.ID_Instalacion) & " - " & oLinqInstalacion.Cliente.Nombre)
            End If



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub Netejar_Pantalla(Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        oLinqInstalacion = New Instalacion
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)



        Me.TE_Codigo.Value = Nothing

        Me.DT_FechaInstalacion.Value = Now.Date
        Util.Cargar_Combo(Me.C_Responsable, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and ID_Personal_Tipo in (" & EnumPersonalTipo.Comercial & " , " & EnumPersonalTipo.ResponsableCuenta & ") ORDER BY Nombre", False, True, "No asignado")

        Me.C_Responsable.Value = 0
        Me.C_TipoNegocio.Value = 0
        Me.C_Estado.Value = 1

        If oTipoEntrada = TipoEntrada.Instalacion Then
            Me.EstableixCaptionForm("Instalación")
        Else
            Me.EstableixCaptionForm("Futura instalación")
        End If



        Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Caption = "Dar de baja"

        Me.C_Tipo_Documento.Value = Nothing
        Me.GRD_Step.GRID.DataSource = Nothing

        Me.C_Pais.Value = oDTC.Pais.Where(Function(F) F.Predeterminat = True).FirstOrDefault.ID_Pais
        Me.C_Delegacion.SelectedIndex = -1
        If pNoCanviarALaPestanyaGeneral = False Then
            Me.TAB_Principal.Tabs("General").Selected = True
        End If
        Me.Tab_Diseño.Tabs("Emplazamiento").Selected = True
        Me.TAB_ComoSeInstalo.Tabs("ProductosInstalados").Selected = True
        Call CargaGrid_SeInstalo()

        'Me.SpreadsheetControl1.Document.CreateNewDocument()
        Me.Excel1.M_NewDocument()

        Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General")

        ErrorProvider1.Clear()

        Fichero.Cargar_GRID(0)
        Call NetejarGrids_Emplazamiento()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TL_Cliente)
                oClsControls.ControlBuit(.TL_Cliente_Contratante)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function DbnullToNothingDecimal(ByVal pValor As Object) As Decimal?
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

    Public Function DbnullToNothingInteger(ByVal pValor As Object) As Integer?
        ' DbnullToNothing = pValor
        Try
            If pValor Is Nothing = False Then
                If IsDBNull(pValor) = True Then
                    Return Nothing
                Else
                    Return CInt(pValor)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim _Filtre As String = ""
        If Seguretat.oUser.NivelSeguridad <> 1 Then
            _Filtre = " and ((Select Count(*) From Instalacion_Seguridad Where Instalacion_Seguridad.ID_Instalacion=C_Instalacion.ID_Instalacion)=0 or ID_Instalacion in (Select ID_Instalacion From Instalacion_Seguridad Where Instalacion_Seguridad.ID_usuario=" & Seguretat.oUser.ID_Usuario & "))"
        End If

        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Instalacion Where ID_Instalacion_Tipo=" & oTipoEntrada & " and Activo=1 and ID_Instalacion_Estado <> " & EnumInstalacionEstado.Negativa & _Filtre & "  ORDER BY ID_Instalacion", Me.TE_Codigo, "ID_Instalacion", "ID_Instalacion")
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

    Private Sub InsertarRiesgosAutomaticament(ByVal pId As Integer)
        BD.EjecutarConsulta("Insert Into Instalacion_Instalacion_Estudio_Riesgo_Tipo (ID_Instalacion, ID_Instalacion_Estudio_Riesgo_Tipo, ID_Valoracion, Explicacion)  Select " & pId & ", ID_Instalacion_Estudio_Riesgo_Tipo, null, null From Instalacion_Estudio_Riesgo_Tipo as TBL1 Where Activo=1 and (Select COUNT(*) From Instalacion_Instalacion_Estudio_Riesgo_Tipo as TBL2 Where ID_Instalacion= " & pId & " and TBL1.ID_Instalacion_Estudio_Riesgo_Tipo=TBL2.ID_Instalacion_Estudio_Riesgo_Tipo)=0")
        BD.EjecutarConsulta("Insert Into Instalacion_Instalacion_Estudio_Riesgo_Valor (ID_Instalacion, ID_Instalacion_Estudio_Riesgo_Valor, ID_Valoracion, Explicacion)  Select " & pId & ", ID_Instalacion_Estudio_Riesgo_Valor, null, null From Instalacion_Estudio_Riesgo_Valor as TBL1 Where Activo=1 and (Select COUNT(*) From Instalacion_Instalacion_Estudio_Riesgo_Valor as TBL2 Where ID_Instalacion= " & pId & " and TBL1.ID_Instalacion_Estudio_Riesgo_Valor=TBL2.ID_Instalacion_Estudio_Riesgo_Valor)=0")
        BD.EjecutarConsulta("Insert Into Instalacion_Instalacion_Estudio_Riesgo_Daños (ID_Instalacion, ID_Instalacion_Estudio_Riesgo_Daños, ID_Valoracion, Explicacion)  Select " & pId & ", ID_Instalacion_Estudio_Riesgo_Daños, null, null From Instalacion_Estudio_Riesgo_Daños as TBL1 Where Activo=1 and (Select COUNT(*) From Instalacion_Instalacion_Estudio_Riesgo_Daños as TBL2 Where ID_Instalacion= " & pId & " and TBL1.ID_Instalacion_Estudio_Riesgo_Daños=TBL2.ID_Instalacion_Estudio_Riesgo_Daños)=0")
        BD.EjecutarConsulta("Insert Into Instalacion_Instalacion_Estudio_Riesgo_Peligros (ID_Instalacion, ID_Instalacion_Estudio_Riesgo_Peligros, ID_Valoracion, Explicacion)  Select " & pId & ", ID_Instalacion_Estudio_Riesgo_Peligros, null, null From Instalacion_Estudio_Riesgo_Peligros as TBL1 Where Activo=1 and (Select COUNT(*) From Instalacion_Instalacion_Estudio_Riesgo_Peligros as TBL2 Where ID_Instalacion= " & pId & " and TBL1.ID_Instalacion_Estudio_Riesgo_Peligros=TBL2.ID_Instalacion_Estudio_Riesgo_Peligros)=0")
        BD.EjecutarConsulta("Insert Into Instalacion_Instalacion_Estudio_Riesgo_Volumen (ID_Instalacion, ID_Instalacion_Estudio_Riesgo_Volumen, ID_Valoracion, Explicacion)  Select " & pId & ", ID_Instalacion_Estudio_Riesgo_Volumen, null, null From Instalacion_Estudio_Riesgo_Volumen as TBL1 Where Activo=1 and (Select COUNT(*) From Instalacion_Instalacion_Estudio_Riesgo_Volumen as TBL2 Where ID_Instalacion= " & pId & " and TBL1.ID_Instalacion_Estudio_Riesgo_Volumen=TBL2.ID_Instalacion_Estudio_Riesgo_Volumen)=0")
        BD.EjecutarConsulta("Insert Into Instalacion_Instalacion_Estudio_Riesgo_Historia (ID_Instalacion, ID_Instalacion_Estudio_Riesgo_Historia, ID_Instalacion_Estudio_Riesgo_Historia_Valoracion, Explicacion)  Select " & pId & ", ID_Instalacion_Estudio_Riesgo_Historia, null, null From Instalacion_Estudio_Riesgo_Historia as TBL1 Where Activo=1 and (Select COUNT(*) From Instalacion_Instalacion_Estudio_Riesgo_Historia as TBL2 Where ID_Instalacion= " & pId & " and TBL1.ID_Instalacion_Estudio_Riesgo_Historia=TBL2.ID_Instalacion_Estudio_Riesgo_Historia)=0")
    End Sub

    Private Sub CargarComboValoracion(ByRef GRD As M_UltraGrid.m_UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oValoracion As IQueryable(Of Valoracion) = (From Taula In oDTC.Valoracion Where Taula.Activo = True Order By Taula.ID_Valoracion Select Taula)
            Dim Var As Valoracion

            For Each Var In oValoracion
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD.GRID.DisplayLayout.Bands(0).Columns("Valoracion").Style = ColumnStyle.DropDownList
            GRD.GRID.DisplayLayout.Bands(0).Columns("Valoracion").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Function RetornaEmplazamiento() As Instalacion_Emplazamiento
        If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = False Then
            RetornaEmplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
        Else
            Return Nothing
        End If
    End Function

    Private Sub NetejarGrids_Emplazamiento()
        Me.GRD_Planta.GRID.DataSource = Nothing
        Me.GRD_Zona.GRID.DataSource = Nothing
        Me.GRD_Abertura.GRID.DataSource = Nothing
        Me.GRD_Construccion.GRID.DataSource = Nothing
        Me.GRD_Ocupacion.GRID.DataSource = Nothing
        Me.GRD_Custodia.GRID.DataSource = Nothing
        Me.GRD_Localizacion.GRID.DataSource = Nothing
        Me.GRD_SeguridadExistente.GRID.DataSource = Nothing
        Me.GRD_HistoriaRobo.GRID.DataSource = Nothing
        Me.GRD_Legislacion.GRID.DataSource = Nothing
        Me.GRD_Entorno.GRID.DataSource = Nothing
        Me.GRD_InfluenciaInt.GRID.DataSource = Nothing
        Me.GRD_InfluenciaExt.GRID.DataSource = Nothing
        Me.GRD_ElementosAProteger.GRID.DataSource = Nothing
        Me.R_Diseño_OtrasEspecificaciones.pText = Nothing
    End Sub

    Private Sub CarregarGrids_Emplazamiento(ByVal pID As Integer)
        If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = False And pID <> 0 Then
            Dim LiniaEmplazamiento As New Instalacion_Emplazamiento
            LiniaEmplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
            Me.R_Diseño_OtrasEspecificaciones.pText = LiniaEmplazamiento.Diseño_OtrasEspecificaciones
        Else
            Me.R_Diseño_OtrasEspecificaciones.pText = Nothing
        End If

        Call CargaGrid_Planta(pID)
        Call CargaGrid_Zona(pID)
        Call CargaGrid_Abertura(pID)
        Call CargaGrid_Construccion(pID)
        Call CargaGrid_Ocupacion(pID)
        Call CargaGrid_Custodia(pID)
        Call CargaGrid_Localizacion(pID)
        Call CargaGrid_SeguridadExistente(pID)
        Call CargaGrid_HistoriaRobo(pID)
        Call CargaGrid_Legislacion(pID)
        Call CargaGrid_Entorno(pID)
        Call CargaGrid_InfluenciaInt(pID)
        Call CargaGrid_InfluenciaExt(pID)
        Call CargaGrid_ElementosAProteger(pID)
    End Sub

    Private Function HiHaComoSeInstalo() As Boolean
        Try
            HiHaComoSeInstalo = False
            If oLinqInstalacion.ID_Instalacion = 0 Then
                Exit Function
            End If

            Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
            If _Propuesta Is Nothing Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub ActivarTabsEnFuncioDelEstatDeLaInstalacio()
        Try
            Util.Tab_Desactivar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)
            Me.TAB_Principal.Tabs("Partes").Enabled = False
            'If HiHaComoSeInstalo() = True Then
            If oLinqInstalacion.Instalacion_Emplazamiento.Count > 0 Then
                Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)
            Else
                If oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta_Estado = EnumPropuestaEstado.Aprobado).Count > 0 Then
                    Util.Tab_Activar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Receptora", "SeInstalo", "Accesos")
                Else
                    Util.Tab_Activar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Receptora", "RecepcionMateriales", "SeInstalo", "Accesos")
                End If

            End If

            Call DeterminarEstatInstalacio()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub DeterminarEstatInstalacio()
        Try
            If oLinqInstalacion.ID_Instalacion_Estado = EnumInstalacionEstado.Negativa Then
                Me.C_Estado.Value = CInt(EnumInstalacionEstado.Negativa)
                oLinqInstalacion.Instalacion_Estado = oDTC.Instalacion_Estado.Where(Function(F) F.ID_Instalacion_Estado = CInt(EnumInstalacionEstado.Negativa)).FirstOrDefault
                oDTC.SubmitChanges()
                Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Caption = "Dar de alta"
                Exit Sub
            Else
                Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Caption = "Dar de baja"
            End If

            If HiHaComoSeInstalo() = True Then
                Me.C_Estado.Value = CInt(EnumInstalacionEstado.Instalado)
                oLinqInstalacion.Instalacion_Estado = oDTC.Instalacion_Estado.Where(Function(F) F.ID_Instalacion_Estado = CInt(EnumInstalacionEstado.Instalado)).FirstOrDefault
                oDTC.SubmitChanges()
                Exit Sub
            End If

            If oLinqInstalacion.Propuesta.Where(Function(F) F.Activo = True And F.ID_Propuesta_Estado = EnumPropuestaEstado.Aprobado).Count > 0 Then
                Me.C_Estado.Value = CInt(EnumInstalacionEstado.Instalacion)
                oLinqInstalacion.Instalacion_Estado = oDTC.Instalacion_Estado.Where(Function(F) F.ID_Instalacion_Estado = CInt(EnumInstalacionEstado.Instalacion)).FirstOrDefault
                oDTC.SubmitChanges()
                Exit Sub
            End If

            If oLinqInstalacion.Propuesta.Where(Function(F) F.Activo = True).Count > 0 Then
                Me.C_Estado.Value = CInt(EnumInstalacionEstado.Presupuestado)
                oLinqInstalacion.Instalacion_Estado = oDTC.Instalacion_Estado.Where(Function(F) F.ID_Instalacion_Estado = CInt(EnumInstalacionEstado.Presupuestado)).FirstOrDefault
                oDTC.SubmitChanges()
                Exit Sub
            End If

            If oLinqInstalacion.Instalacion_Emplazamiento.Count > 0 Then
                Me.C_Estado.Value = CInt(EnumInstalacionEstado.Diseño)
                oLinqInstalacion.Instalacion_Estado = oDTC.Instalacion_Estado.Where(Function(F) F.ID_Instalacion_Estado = CInt(EnumInstalacionEstado.Diseño)).FirstOrDefault
                oDTC.SubmitChanges()
                Exit Sub
            End If

            Me.C_Estado.Value = CInt(EnumInstalacionEstado.Pendiente)
            oLinqInstalacion.Instalacion_Estado = oDTC.Instalacion_Estado.Where(Function(F) F.ID_Instalacion_Estado = CInt(EnumInstalacionEstado.Pendiente)).FirstOrDefault
            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub DespresDeCarregarDades()
        'If Me.GRD_Ficheros.GRID.Rows.Count > 0 Then
        Util.TabPintarHeaderTab(Me.TAB_Principal.Tabs("Ficheros"), Me.TAB.Tabs("Instalacion").Appearance.BackColor)
        'Else
        'Util.TabPintarHeaderTab(Me.TAB_Principal.Tabs("Ficheros"), Me.TAB_Ficheros.Tabs("Instalacion").Appearance.BackColor)
        'End If
    End Sub

    Private Function ComprovarPermisosSeguretat() As Boolean
        Try
            ComprovarPermisosSeguretat = False

            'si no s'ha afegit ningú al llistat es accessible per tothom
            If oLinqInstalacion.Instalacion_Seguridad.Count = 0 Then
                Return True
            End If

            'Si te nivell de seguretat 1 te accés accessible
            If Seguretat.oUser.NivelSeguridad = 1 Then
                Return True
            End If

            'si esta dins de la llista te accés
            If oLinqInstalacion.Instalacion_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count = 1 Then
                Return True
            End If

            Mensaje.Mostrar_Mensaje("Imposible cargar los datos, no tiene permisos", M_Mensaje.Missatge_Modo.INFORMACIO)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

#Region "Duplicar Pressupost"
    Function DuplicarPressupost(ByVal pIDPressupost As Integer, ByRef pInstalacion As Instalacion) As Boolean
        Try
            DuplicarPressupost = False

            Dim Original As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = pIDPressupost).FirstOrDefault
            Dim Clon As New Propuesta
            With Original
                Clon.Activo = True
                Clon.Base = .Base
                Clon.Codigo = oDTC.Contadores.FirstOrDefault.Propuesta
                oDTC.Contadores.FirstOrDefault.Propuesta = oDTC.Contadores.FirstOrDefault.Propuesta + 1
                Clon.ConectadoCRA = .ConectadoCRA
                Clon.Descripcion = .Descripcion
                Clon.Descuento = .Descuento
                Clon.Fecha = Now.Date
                Clon.FormaPago = .FormaPago

                If oLinqInstalacion.ID_Instalacion = pInstalacion.ID_Instalacion Then   'si s'esta duplicant la proposta a unaltre instalació llavors no posarem els planta y zona pero buscarem la predeterminada
                    Clon.Instalacion_Emplazamiento = .Instalacion_Emplazamiento
                    Clon.Instalacion_Emplazamiento_Planta = .Instalacion_Emplazamiento_Planta
                    Clon.Instalacion_Emplazamiento_Zona = .Instalacion_Emplazamiento_Zona
                Else
                    Clon.Instalacion_Emplazamiento = pInstalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                End If


                Clon.Grado_Notificacion = .Grado_Notificacion
                Clon.Instalacion = .Instalacion
                Clon.IVA = .IVA
                Clon.NivelMaximoLineas = .NivelMaximoLineas
                Clon.Observaciones = .Observaciones
                Clon.Persona = pInstalacion.PersonaContacto
                Clon.Personal = .Personal
                Clon.Producto_Grado = .Producto_Grado
                Clon.Propuesta = .Propuesta
                Clon.Propuesta_Estado = oDTC.Propuesta_Estado.Where(Function(F) F.ID_Propuesta_Estado = CInt(EnumPropuestaEstado.Pendiente)).FirstOrDefault
                Clon.Propuesta_Tipo = .Propuesta_Tipo
                Clon.Receptora = .Receptora
                Clon.SegunNormativa = .SegunNormativa
                Clon.SeInstalo = False
                Clon.TiempoInstalacion = .TiempoInstalacion
                Clon.Total = .Total
                Clon.Validez = .Validez
                Clon.Empresa = .Empresa
                If pInstalacion.ID_Instalacion <> oLinqInstalacion.ID_Instalacion Then  'Si copiem el pressupost en unaltre instalació sempre serà la versió A
                    Clon.Version = "A"
                Else
                    Clon.Version = Chr(Asc(Original.Version) + 1)
                End If

                Clon.DetalleExtendido = .DetalleExtendido
                Clon.HorasPrevistas = .HorasPrevistas

                pInstalacion.Propuesta.Add(Clon)
            End With

            Call CrearPlanosPressupost(Original, Clon)

            Call CrearDiagramasPressupost(Original, Clon)

            Call DuplicarLineasPressupost(Original, Clon)

            Call CrearFicherosPressupost(Original, Clon)

            Call CrearEspecificaciones(Original, Clon)

            Call CrearSeguridad(Original, Clon)

            oDTC.SubmitChanges()


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub CrearPlanosPressupost(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _Plano As Propuesta_Plano


            For Each _Plano In pPropuestaOriginal.Propuesta_Plano
                Dim _NewPlano As New Propuesta_Plano
                With _NewPlano
                    .Descripcion = _Plano.Descripcion
                    .FechaCreacion = _Plano.FechaCreacion
                    .Validado = _Plano.Validado

                    '.ID_Propuesta = PropuestaSeInstalo.ID_Propuesta
                    '.ID_Propuesta_Antigua = _Plano.ID_Propuesta
                    '.Propuesta_Version_Antigua = _Plano.Propuesta.Version

                    If pPropuestaOriginal.ID_Instalacion = pPropuestaClon.ID_Instalacion Then   'si s'esta duplicant la proposta a unaltre instalació llavors no posarem els planta y zona pero buscarem la predeterminada
                        .ID_Instalacion_Emplazamiento = _Plano.ID_Instalacion_Emplazamiento
                        .ID_Instalacion_Emplazamiento_Planta = _Plano.ID_Instalacion_Emplazamiento_Planta
                        .ID_Instalacion_Emplazamiento_Zona = _Plano.ID_Instalacion_Emplazamiento_Zona
                    Else
                        .Instalacion_Emplazamiento = pPropuestaClon.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                    End If

                    .ID_Instalacion_Emplazamiento = _Plano.ID_Instalacion_Emplazamiento
                    .ID_Instalacion_Emplazamiento_Planta = _Plano.ID_Instalacion_Emplazamiento_Planta
                    .ID_Instalacion_Emplazamiento_Zona = _Plano.ID_Instalacion_Emplazamiento_Zona

                    If IsNothing(_Plano.ID_PlanoBinario) = False Then
                        Dim _PlanoBinario As New PlanoBinario
                        _PlanoBinario.Fichero = _Plano.PlanoBinario.Fichero
                        _PlanoBinario.Foto = _Plano.PlanoBinario.Foto
                        .PlanoBinario = _PlanoBinario

                        'No està fet pq es força complexe.
                        'Dim _ElementoIntroducido As Propuesta_Plano_ElementosIntroducidos
                        'For Each _ElementoIntroducido In _Plano.Propuesta_Plano_ElementosIntroducidos
                        '    Dim _NewElemento As Propuesta_Plano_ElementosIntroducidos
                        '    _NewElemento.

                        'Next

                        oDTC.PlanoBinario.InsertOnSubmit(_PlanoBinario)
                    End If


                End With



                pPropuestaClon.Propuesta_Plano.Add(_NewPlano)
                'oDTC.SubmitChanges()
            Next

            '  oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CrearDiagramasPressupost(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _Plano As Propuesta_Diagrama


            For Each _Plano In pPropuestaOriginal.Propuesta_Diagrama
                Dim _NewDiagrama As New Propuesta_Diagrama
                With _NewDiagrama
                    .Descripcion = _Plano.Descripcion
                    .FechaCreacion = _Plano.FechaCreacion
                    .Validado = _Plano.Validado

                    '.ID_Propuesta = PropuestaSeInstalo.ID_Propuesta
                    '.ID_Propuesta_Antigua = _Plano.ID_Propuesta
                    '.Propuesta_Version_Antigua = _Plano.Propuesta.Version

                    If pPropuestaOriginal.ID_Instalacion = pPropuestaClon.ID_Instalacion Then   'si s'esta duplicant la proposta a unaltre instalació llavors no posarem els planta y zona pero buscarem la predeterminada
                        .ID_Instalacion_Emplazamiento = _Plano.ID_Instalacion_Emplazamiento
                        .ID_Instalacion_Emplazamiento_Planta = _Plano.ID_Instalacion_Emplazamiento_Planta
                        .ID_Instalacion_Emplazamiento_Zona = _Plano.ID_Instalacion_Emplazamiento_Zona
                    Else
                        .Instalacion_Emplazamiento = pPropuestaClon.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                    End If

                    If IsNothing(_Plano.ID_DiagramaBinario) = False Then
                        Dim _PlanoBinario As New DiagramaBinario
                        _PlanoBinario.Fichero = _Plano.DiagramaBinario.Fichero
                        _PlanoBinario.Foto = _Plano.DiagramaBinario.Foto
                        .DiagramaBinario = _PlanoBinario

                        'No està fet pq es força complexe.
                        'Dim _ElementoIntroducido As Propuesta_Plano_ElementosIntroducidos
                        'For Each _ElementoIntroducido In _Plano.Propuesta_Plano_ElementosIntroducidos
                        '    Dim _NewElemento As Propuesta_Plano_ElementosIntroducidos
                        '    _NewElemento.

                        'Next

                        oDTC.DiagramaBinario.InsertOnSubmit(_PlanoBinario)
                    End If


                End With



                pPropuestaClon.Propuesta_Diagrama.Add(_NewDiagrama)
                'oDTC.SubmitChanges()
            Next

            '  oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub


    Private Sub CrearEspecificaciones(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _Resposta As Propuesta_PropuestaEspecificacion

            For Each _Resposta In pPropuestaOriginal.Propuesta_PropuestaEspecificacion
                Dim _NewResposta As New Propuesta_PropuestaEspecificacion
                With _NewResposta
                    .FechaRespuesta = _Resposta.FechaRespuesta
                    .PropuestaEspecificacion = _Resposta.PropuestaEspecificacion
                    .PropuestaEspecificacion_Respuesta = _Resposta.PropuestaEspecificacion_Respuesta
                    .Observaciones = _Resposta.Observaciones
                    .Usuario = _Resposta.Usuario
                End With

                pPropuestaClon.Propuesta_PropuestaEspecificacion.Add(_NewResposta)
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CrearSeguridad(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _Resposta As Propuesta_Seguridad

            For Each _Resposta In pPropuestaOriginal.Propuesta_Seguridad
                Dim _NewResposta As New Propuesta_Seguridad
                With _NewResposta
                    .Usuario = _Resposta.Usuario
                End With

                pPropuestaClon.Propuesta_Seguridad.Add(_NewResposta)
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub DuplicarLineasPressupost(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim RelacionsOriginalAmbClon As New ArrayList

            Dim _Linea As Propuesta_Linea

            For Each _Linea In pPropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = False)
                Call CreaLineasPressupost(_Linea, pPropuestaClon, RelacionsOriginalAmbClon)
                Call RecursivitatLineas(_Linea, pPropuestaClon, RelacionsOriginalAmbClon)
            Next
            oDTC.SubmitChanges()
            'Bucle per posar correctament la vinculació energètica
            For Each _Linea In pPropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado_Energetico.HasValue = False)
                Call RecursivitatLineasEnergeticas(_Linea, RelacionsOriginalAmbClon)
            Next

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub RecursivitatLineasEnergeticas(ByRef pLinea As Propuesta_Linea, ByRef pRelacionsOriginalAmbClon As ArrayList)
        Dim _Linea2 As Propuesta_Linea
        For Each _Linea2 In pLinea.Propuesta_Linea_VinculadoEnergeticoHijo
            Call AssignacioEnergetica(_Linea2, pRelacionsOriginalAmbClon)
            Call RecursivitatLineasEnergeticas(_Linea2, pRelacionsOriginalAmbClon)
        Next
    End Sub

    Private Sub AssignacioEnergetica(ByRef pLinea As Propuesta_Linea, ByRef pRelacionsOriginalAmbClon As ArrayList)
        If pLinea.ID_Propuesta_Linea_Vinculado_Energetico Is Nothing = False Then
            Dim _IDClonFill As Integer
            Dim _IDClonPare As Integer
            Dim _Struct As StructOriginalClon
            For Each _Struct In pRelacionsOriginalAmbClon
                If _Struct.IDOriginal = pLinea.ID_Propuesta_Linea_Vinculado_Energetico Then
                    '.Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Struct.IDClon).FirstOrDefault
                    '                    Dim _LineaClonada As Propuesta_Linea=oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea=)

                    'pLinea.ID_Propuesta_Linea_Vinculado_Energetico = _Struct.IDClon
                    _IDClonPare = _Struct.IDClon 'Aquest ID és la ID_Propuesta_Linea_vinculacio_energetica del pressupost clonat

                End If

                If _Struct.IDOriginal = pLinea.ID_Propuesta_Linea Then
                    _IDClonFill = _Struct.IDClon
                End If
            Next
            Dim _LineaClonada As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _IDClonFill).FirstOrDefault
            _LineaClonada.ID_Propuesta_Linea_Vinculado_Energetico = _IDClonPare
        End If
    End Sub

    Private Sub RecursivitatLineas(ByVal pLinea As Propuesta_Linea, ByRef pPropuestaSeInstalo As Propuesta, ByRef pRelacionsOriginalAmbClon As ArrayList)
        Dim _Linea2 As Propuesta_Linea
        For Each _Linea2 In pLinea.Propuesta_Linea1
            Call CreaLineasPressupost(_Linea2, pPropuestaSeInstalo, pRelacionsOriginalAmbClon)
            Call RecursivitatLineas(_Linea2, pPropuestaSeInstalo, pRelacionsOriginalAmbClon)
        Next
    End Sub

    Private Sub CreaLineasPressupost(ByVal pLinea As Propuesta_Linea, ByRef pPropuestaClon As Propuesta, ByRef pRelacionsOriginalAmbClon As ArrayList)
        Try
            'For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso <> EnumProductoSubFamiliaTraspaso.No).OrderBy(Function(F) F.ID_Propuesta_Linea_Vinculado)
            If pLinea.Unidad > 0 Then
                Dim DescartarLinea As Boolean = False
                Dim _NewLinea As Propuesta_Linea = clsPropuestaLinea.RetornaDuplicacioInstancia(pLinea, False)
                With _NewLinea

                    '.Propuesta = pPropuestaClon.Propuesta
                    .ID_Propuesta = pPropuestaClon.ID_Propuesta

                    If oLinqInstalacion.ID_Instalacion = pPropuestaClon.ID_Instalacion Then   'si s'esta duplicant la proposta a unaltre instalació llavors no posarem els planta y zona pero buscarem la predeterminada
                        .ID_Instalacion_Emplazamiento = pLinea.ID_Instalacion_Emplazamiento
                        .ID_Instalacion_Emplazamiento_Planta = pLinea.ID_Instalacion_Emplazamiento_Planta
                        .ID_Instalacion_Emplazamiento_Zona = pLinea.ID_Instalacion_Emplazamiento_Zona
                    Else
                        .Instalacion_Emplazamiento = pPropuestaClon.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                        .Instalacion_Emplazamiento_Planta = Nothing
                        .Instalacion_Emplazamiento_Zona = Nothing
                        .Instalacion_ElementosAProteger = Nothing
                        .Instalacion_Emplazamiento_Abertura = Nothing
                    End If


                    If pLinea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
                        Dim _Struct As StructOriginalClon
                        For Each _Struct In pRelacionsOriginalAmbClon
                            If _Struct.IDOriginal = pLinea.ID_Propuesta_Linea_Vinculado Then
                                '.Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Struct.IDClon).FirstOrDefault
                                .ID_Propuesta_Linea_Vinculado = _Struct.IDClon
                            End If
                        Next
                    End If

                    .ID_Propuesta_Linea_Vinculado_Energetico = Nothing 'Posem això pq al duplicar copia el ID però com que estem creant línies noves no ens interesa el ID de la línea original. Per això després farem un bucle per introduir els ID's correctament

                    'If _Linea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
                    '    'El If de baix es per comprovar que la línea s'ha traspasat. Pq potser que el pare tingues la marca de família que no s'ha de traspasar i el fill llavors no s'ha de vincular
                    '    If IsNothing(pPropuestaClon.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault) = True Then
                    '        ' DescartarLinea = True
                    '    Else
                    'If _Linea.Activo = False Then
                    '    .ID_Propuesta_Linea_Vinculado = pPropuestaClon.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault.ID_Propuesta_Linea
                    'End If
                    '   End If
                    'End If

                    .Activo = True
                    .ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Aprobado_PendienteRecibir
                End With

                pPropuestaClon.Propuesta_Linea.Add(_NewLinea)
                oDTC.SubmitChanges()

                'Fem una taula per guardar el ID linea del orginal i el ID del clonat. Així podrem fer les vinculacions de les línies posteriorment
                Dim pepe As New StructOriginalClon
                pepe.IDClon = _NewLinea.ID_Propuesta_Linea
                pepe.IDOriginal = pLinea.ID_Propuesta_Linea
                pRelacionsOriginalAmbClon.Add(pepe)

            End If
            'Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CrearFicherosPressupost(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _PropuestaArchivo As Propuesta_Archivo

            For Each _PropuestaArchivo In pPropuestaOriginal.Propuesta_Archivo
                Dim _NewPropuestaArchivo As New Propuesta_Archivo
                With _NewPropuestaArchivo
                    If IsNothing(_PropuestaArchivo.ID_Archivo) = False Then
                        Dim _NewArchivo As New Archivo
                        _NewArchivo.Activo = _PropuestaArchivo.Archivo.Activo
                        _NewArchivo.CampoBinario = _PropuestaArchivo.Archivo.CampoBinario
                        _NewArchivo.Color = _PropuestaArchivo.Archivo.Color
                        _NewArchivo.Descripcion = _PropuestaArchivo.Archivo.Descripcion
                        _NewArchivo.Fecha = _PropuestaArchivo.Archivo.Fecha
                        _NewArchivo.ID_Usuario = _PropuestaArchivo.Archivo.ID_Usuario
                        _NewArchivo.Ruta_Fichero = _PropuestaArchivo.Archivo.Ruta_Fichero
                        _NewArchivo.Tamaño = _PropuestaArchivo.Archivo.Tamaño
                        _NewArchivo.Tipo = _PropuestaArchivo.Archivo.Tipo
                        .Archivo = _NewArchivo
                        ' .Propuesta = pPropuestaClon
                        pPropuestaClon.Propuesta_Archivo.Add(_NewPropuestaArchivo)
                        oDTC.Propuesta_Archivo.InsertOnSubmit(_NewPropuestaArchivo)
                        oDTC.Archivo.InsertOnSubmit(_NewArchivo)
                    End If
                End With

            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub CrearPresupuestoRutas(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
    '    Try

    '        Dim _PropuestaRuta As Propuesta_rut

    '        For Each _PropuestaArchivo In pPropuestaOriginal.Propuesta_Archivo
    '            Dim _NewPropuestaArchivo As New Propuesta_Archivo
    '            With _NewPropuestaArchivo
    '                If IsNothing(_PropuestaArchivo.ID_Archivo) = False Then
    '                    Dim _NewArchivo As New Archivo
    '                    _NewArchivo.Activo = _PropuestaArchivo.Archivo.Activo
    '                    _NewArchivo.CampoBinario = _PropuestaArchivo.Archivo.CampoBinario
    '                    _NewArchivo.Color = _PropuestaArchivo.Archivo.Color
    '                    _NewArchivo.Descripcion = _PropuestaArchivo.Archivo.Descripcion
    '                    _NewArchivo.Fecha = _PropuestaArchivo.Archivo.Fecha
    '                    _NewArchivo.ID_Usuario = _PropuestaArchivo.Archivo.ID_Usuario
    '                    _NewArchivo.Ruta_Fichero = _PropuestaArchivo.Archivo.Ruta_Fichero
    '                    _NewArchivo.Tamaño = _PropuestaArchivo.Archivo.Tamaño
    '                    _NewArchivo.Tipo = _PropuestaArchivo.Archivo.Tipo
    '                    .Archivo = _NewArchivo
    '                    pPropuestaClon.Propuesta_Archivo.Add(_NewPropuestaArchivo)
    '                    oDTC.Propuesta_Archivo.InsertOnSubmit(_NewPropuestaArchivo)
    '                    oDTC.Archivo.InsertOnSubmit(_NewArchivo)
    '                End If
    '            End With
    '        Next

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub
#End Region

    Private Sub CrearPreguntesQuestionari(ByRef pNewParte As Parte)
        Dim _IDParte As Integer = pNewParte.ID_Parte
        Dim _Pregunta As Parte_Cuestionario_Preguntas
        For Each _Pregunta In oDTC.Parte_Cuestionario_Preguntas.Where(Function(F) F.ID_Parte_Tipo = _IDParte)
            Dim _NewResposta As New Parte_Cuestionario_Respuestas
            _NewResposta.Parte = pNewParte
            _NewResposta.Parte_Cuestionario_Preguntas = _Pregunta
            _NewResposta.Respuesta = 0
            pNewParte.Parte_Cuestionario_Respuestas.Add(_NewResposta)
        Next
        oDTC.SubmitChanges()
    End Sub

    Private Sub AvanzarRetroceder(ByVal pAvanzar As Boolean)
        'Que pasa si no s'ha seleccinat cap article?

        Dim _IDATrobar As Integer
        Dim _Trobat As Boolean = False

        If oLinqInstalacion.ID_Instalacion = 0 Then
            If Me.OP_Filtre.Value = "Cliente" Then
                Exit Sub
            Else
                'Si actualment no tenim cap registre carregat farem veure com si estiguessim al últim.
                _IDATrobar = oDTC.Instalacion.Where(Function(F) F.Activo = True).Max(Function(F) F.ID_Instalacion)
                _Trobat = True

            End If
        Else
            _IDATrobar = oLinqInstalacion.ID_Instalacion
        End If

        Dim _LlistatInstalaciones As IList(Of Instalacion)
        Select Case Me.OP_Filtre.Value
            Case "Codigo"
                If pAvanzar = True Then
                    _LlistatInstalaciones = oDTC.Instalacion.Where(Function(F) F.Activo = True).OrderBy(Function(F) F.ID_Instalacion).ToList
                Else
                    _LlistatInstalaciones = oDTC.Instalacion.Where(Function(F) F.Activo = True).OrderByDescending(Function(F) F.ID_Instalacion).ToList
                End If
            Case "Cliente"
                If pAvanzar = True Then
                    _LlistatInstalaciones = oDTC.Instalacion.Where(Function(F) F.Activo = True And F.ID_Cliente = oLinqInstalacion.ID_Instalacion).OrderBy(Function(F) F.ID_Instalacion).ToList
                Else
                    _LlistatInstalaciones = oDTC.Instalacion.Where(Function(F) F.Activo = True And F.ID_Cliente = oLinqInstalacion.ID_Instalacion).OrderByDescending(Function(F) F.ID_Instalacion).ToList
                End If
        End Select


        Dim _Instalacion As Instalacion
        Dim _InstalacionSeguent As Instalacion
        For Each _Instalacion In _LlistatInstalaciones
            If _Trobat = True Then
                _InstalacionSeguent = _Instalacion
                Exit For
            End If
            If _Instalacion.ID_Instalacion = _IDATrobar Then
                _Trobat = True
            End If
        Next
        If _InstalacionSeguent Is Nothing = False Then
            Dim _TabActual As String = Me.TAB_Principal.SelectedTab.Key
            Call Cargar_Form(_InstalacionSeguent.ID_Instalacion, True)
            Call CarregarDadesPestanyes(_TabActual)
        End If
    End Sub

    Private Sub CarregarDadesPestanyes(ByVal pKeyPestanya As String)
        Try
            Dim ID As Integer
            If oLinqInstalacion Is Nothing = False Then
                ID = oLinqInstalacion.ID_Instalacion
            End If
            Select Case pKeyPestanya
                Case "Principal"
                    Call DeterminarEstatInstalacio()
                Case "Diseño"
                    'Call CargarGrids_Diseño(ID)
                    'Case "Emplazamiento"
                    'Me.Tab_Diseño.Tabs("Emplazamiento").Selected = True
                    Call CargaGrid_Emplazamiento(ID)
                    'Si hi ha algun emplazamiento directament el carreguem
                    If Me.GRD_Emplazamiento.GRID.Selected.Rows.Count = 1 Then
                        Dim pIDEmplazamiento As Integer = Me.GRD_Emplazamiento.GRID.Selected.Rows(0).Cells("ID_Instalacion_Emplazamiento").Value
                        Call CarregarGrids_Emplazamiento(pIDEmplazamiento)
                    End If
                Case "Propuesta"
                    ' Call CargaGrid_Propuesta(ID)
                    C_Propuesta_Estado.Value = 1
                Case "SeInstalo"
                    'Me.TAB_ComoSeInstalo.Tabs("ProductosInstalados").Selected = True
                    Call CargaGrid_SeInstalo()
                Case "Accesos"
                    Call CargarGrid_Accesos()
                Case "ATenerEnCuenta"
                    Call CargaGrid_ATenerEnCuenta(ID)
                Case "RecepcionMateriales"
                    Call CargaGridRecepcionMateriales()
                Case "Partes"
                    Call CargarGrid_Partes()
                Case "RevisionProductos"
                    Call CargaGrid_RevisionProductos()
                Case "Receptora"
                    Call CargaGrid_Receptora(ID)
                Case "Seguridad"
                    Call CargaGrid_Seguridad(ID)
                Case "Contrato"
                    Call CargaGrid_Contratos(ID)
                Case "Contactos"
                    Call CargaGrid_Contactos()
                Case "ToDo"
                    Call CargaGrid_ToDo(ID)
                Case "ActividadCRM"
                    Call CargaGrid_ActividadCRM(ID)
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
        If e.Button.Key = "Cancelar" Then
            Call Netejar_Pantalla()
        End If
    End Sub

    Private Sub TE_Cliente_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TL_Cliente.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            'si el personal que esta mirant i te la marca de versoloclientesdondesetenga permisos pasarà això
            Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
            Dim _SQL As String = ""

            Dim _FiltreTipoClient As String = ""
            Select Case oTipoEntrada
                Case TipoEntrada.Instalacion
                    _FiltreTipoClient = " ID_Cliente_Tipo=" & EnumClienteTipo.Cliente & " And "
                    If _Personal.VerSoloClientesDondeSeTengaPermisos = True Then
                        _SQL = " and ID_Cliente in (Select ID_Cliente From Cliente_Seguridad Where Cliente_Seguridad.ID_usuario=" & Seguretat.oUser.ID_Usuario & ") "
                    End If

                Case TipoEntrada.FuturaInstalacion
                    'En la futura instalación te que veure tots els futurs i clients i, si te el putu check de "VerSoloClientesDondeSeTengaPermisos" només ha de veure els seus
                    If _Personal.VerSoloClientesDondeSeTengaPermisos = True Then
                        _FiltreTipoClient = ""
                        _SQL = " and ID_Cliente_Tipo=" & EnumClienteTipo.FuturoCliente & " or (ID_Cliente_Tipo=" & EnumClienteTipo.Cliente & " and  ID_Cliente in (Select ID_Cliente From Cliente_Seguridad Where Cliente_Seguridad.ID_usuario=" & Seguretat.oUser.ID_Usuario & "))"
                    Else
                        _FiltreTipoClient = " "
                    End If
            End Select

            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM Cliente Where " & _FiltreTipoClient & "  Activo=1 " & _SQL & " ORDER BY Nombre", Me.TL_Cliente, "ID_Cliente", "Nombre")
            AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatCliente
        End If

        If e.Button.Key = "Ficha" And Me.TL_Cliente.Tag Is Nothing = False Then
            Dim frmClient As New frmCliente
            frmClient.Entrada(Me.TL_Cliente.Tag)
            frmClient.FormObrir(Me, True)
        End If
    End Sub

    Private Sub TE_Cliente_Contratante_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TL_Cliente_Contratante.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            'si el personal que esta mirant i te la marca de versoloclientesdondesetenga permisos pasarà això
            Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
            Dim _SQL As String = ""

            Dim _FiltreTipoClient As String = ""
            Select Case oTipoEntrada
                Case TipoEntrada.Instalacion
                    _FiltreTipoClient = " ID_Cliente_Tipo=" & EnumClienteTipo.Cliente & " And "
                    If _Personal.VerSoloClientesDondeSeTengaPermisos = True Then
                        _SQL = " and ID_Cliente in (Select ID_Cliente From Cliente_Seguridad Where Cliente_Seguridad.ID_usuario=" & Seguretat.oUser.ID_Usuario & ") "
                    End If

                Case TipoEntrada.FuturaInstalacion

                    If _Personal.VerSoloClientesDondeSeTengaPermisos = True Then
                        _FiltreTipoClient = ""
                        _SQL = " and ID_Cliente_Tipo=" & EnumClienteTipo.FuturoCliente & " or (ID_Cliente_Tipo=" & EnumClienteTipo.Cliente & " and  ID_Cliente in (Select ID_Cliente From Cliente_Seguridad Where Cliente_Seguridad.ID_usuario=" & Seguretat.oUser.ID_Usuario & "))"
                    Else
                        _FiltreTipoClient = "  "
                    End If

            End Select

            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM Cliente Where " & _FiltreTipoClient & " Activo=1 " & _SQL & " ORDER BY Nombre", Me.TL_Cliente_Contratante, "ID_Cliente", "Nombre")
            'AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatClienteContratante
        End If

        If e.Button.Key = "Ficha" And Me.TL_Cliente_Contratante.Tag Is Nothing = False Then
            Dim frmClient As New frmCliente
            frmClient.Entrada(Me.TL_Cliente_Contratante.Tag)
            frmClient.FormObrir(Me, True)
        End If
    End Sub

    Private Sub AlTancarLlistatCliente(ByVal pID As String)
        If IsNumeric(pID) Then
            Dim oCliente As Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(pID)).FirstOrDefault
            Me.T_Direccion.Text = oCliente.Direccion
            Me.T_Persona_Contacto.Text = oCliente.PersonaContacto
            Me.T_Email.Text = oCliente.Email
            Me.T_Poblacion.Text = oCliente.Poblacion
            Me.T_Provincia.Text = oCliente.Provincia
            Me.T_Telefono.Text = oCliente.Telefono
            Me.TL_Cliente_Contratante.Tag = oCliente.ID_Cliente
            Me.TL_Cliente_Contratante.Text = oCliente.Nombre
            Me.T_CP.Text = oCliente.CP
            Me.T_Latitud.Value = oCliente.Latitud
            Me.T_Longitud.Value = oCliente.Longitud
            Me.C_Pais.Value = oCliente.ID_Pais
            If oCliente.ID_Delegacion.HasValue = True Then
                Me.C_Delegacion.Value = oCliente.ID_Delegacion
            Else
                Me.C_Delegacion.SelectedIndex = -1
            End If

        End If
    End Sub

    Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Me.TE_Codigo.Value Is Nothing = False AndAlso IsDBNull(Me.TE_Codigo.Value) = False Then
                Dim ooLinqInstalacion As Instalacion = oDTC.Instalacion.Where(Function(F) F.ID_Instalacion = CInt(Me.TE_Codigo.Value) And F.Activo = True And F.ID_Instalacion_Tipo = CInt(oTipoEntrada)).FirstOrDefault()
                If ooLinqInstalacion Is Nothing = False Then
                    'Els permisos ja s'apliquen en el cargarform
                    ' If ooLinqInstalacion.Instalacion_Seguridad.Count = 0 OrElse ooLinqInstalacion.Instalacion_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count > 0 Then
                    Call Cargar_Form(ooLinqInstalacion.ID_Instalacion)
                    'Else
                    '    Mensaje.Mostrar_Mensaje("No tienes suficientes permisos para entrar en esta instalación", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    ' End If
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If
            Else
                If oDTC.Instalacion.Count > 0 Then
                    Me.TE_Codigo.Value = oDTC.Instalacion.Where(Function(F) F.Activo = True).Max(Function(F) F.ID_Instalacion) + 1
                Else
                    Me.TE_Codigo.Value = 1
                End If
            End If
        End If
    End Sub

    Private Sub TAB_Principal_SelectedTabChanged(sender As System.Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
        Call CarregarDadesPestanyes(e.Tab.Key)
    End Sub

    Private Sub Tab_Diseño_SelectedTabChanged(sender As System.Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles Tab_Diseño.SelectedTabChanged
        If Me.TAB_Principal.SelectedTab Is Nothing = False AndAlso Me.TAB_Principal.SelectedTab.Key = "Diseño" Then
            Me.GRD_RevisionProducto.GRID.ActiveCell = Nothing
            Select Case e.Tab.Key
                Case "Estudio"
                    Call CargarGrids_Diseño(oLinqInstalacion.ID_Instalacion)
                Case "Emplazamiento"
                    Me.GRD_Emplazamiento.GRID.DataSource = Nothing
                    Call CargaGrid_Emplazamiento(oLinqInstalacion.ID_Instalacion)
                    Me.GRD_Emplazamiento.GRID.ActiveRow = Nothing
                    Me.GRD_Emplazamiento.GRID.Selected.Rows.Clear()
            End Select
        End If

    End Sub

    Private Sub TAB_ComoSeInstalo_Diseño_SelectedTabChanged(sender As System.Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_ComoSeInstalo.SelectedTabChanged
        If Me.TAB_Principal.SelectedTab.Key = "SeInstalo" Then
            Select Case e.Tab.Key
                Case "Interconexiones"
                    Call CargaGrid_Cableado()
                Case "Cables"
                    Call CargaGrid_Cables()
                Case "FuenteAlimentacion"
                    Call CargaGrid_FuenteAlimentacion()
                Case "CajasIntermedias"
                    Call CargaGrid_CajasIntermedias()
                Case "ProductosInstalados"
                    Call CargaGrid_SeInstalo()
                Case "ModificacionesInstalacion"
                    Call CargaGrid_ProductosEliminados()
                    Call CargaGrid_ProductosPendientesAprobar()
                Case "Planos"
                    Call CargaGrid_Planos()
                Case "InstaladoEn"
                    Call CargaGrid_InstaladoEn()
                Case "Ruta"
                    Call CargaGrid_Ruta()
            End Select
        End If
    End Sub

    Private Sub CargarGrids_Diseño(ByVal pID As Integer)
        If pID = 0 Then
            Exit Sub
        End If
        Call InsertarRiesgosAutomaticament(pID)
        Call Carga_Grid_Riesgo_Tipo(pID)
        Call Carga_Grid_Riesgo_Valor(pID)
        Call Carga_Grid_Riesgo_Volumen(pID)
        Call Carga_Grid_Riesgo_Daños(pID)
        Call Carga_Grid_Riesgo_Historia(pID)
        Call Carga_Grid_Riesgo_Peligros(pID)
    End Sub

    Private Sub B_Diseño_Guardar_Click(sender As System.Object, e As System.EventArgs) Handles B_Diseño_Guardar.Click
        If Me.GRD_Emplazamiento.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        Dim LiniaEmplazamiento As New Instalacion_Emplazamiento
        LiniaEmplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
        LiniaEmplazamiento.Diseño_OtrasEspecificaciones = Me.R_Diseño_OtrasEspecificaciones.pText
        oDTC.SubmitChanges()
        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
    End Sub

    Private Sub frmInstalacion_AlTancarForm(ByRef pCancel As Boolean) Handles Me.AlTancarForm
        If Me.Visible = True Then
            If oclsPilaFormularis.OcultarFormulari(Me) = True Then
                pCancel = True
            End If
        End If
    End Sub

    Private Sub UltraTabControl1_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB.SelectedTabChanged
        If e.Tab.Key = "FicherosPropuestas" Or e.Tab.Key = "FicherosPartes" Then
            Call CargarGrid_FicherosPropuestasPartes()
        End If
    End Sub

    Private Sub C_Tipo_Documento_ValueChanged(sender As Object, e As EventArgs) Handles C_Tipo_Documento.ValueChanged

        If Me.C_Tipo_Documento.Items.Count = 0 OrElse Me.C_Tipo_Documento.Value Is Nothing OrElse oLinqInstalacion.ID_Instalacion = 0 Then
            Exit Sub
        End If

        Dim DTS As New DataSet
        BD.CargarDataSet(DTS, "Select * From C_Entrada Where ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Entrada in (Select ID_Entrada From Entrada_Instalacion Where ID_Instalacion= " & oLinqInstalacion.ID_Instalacion & " Group By ID_Entrada) Order by FechaEntrada")
        BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Entrada in (Select ID_Entrada From Entrada_Instalacion Where ID_Instalacion= " & oLinqInstalacion.ID_Instalacion & " Group By ID_Entrada) Order by FechaEntrada", "aa", 0, "ID_Entrada", "ID_Entrada", True)
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

    Private Sub C_Propuesta_Estado_ValueChanged(sender As Object, e As EventArgs) Handles C_Propuesta_Estado.ValueChanged
        If oLinqInstalacion Is Nothing = False Then
            Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)
        End If
    End Sub

    Private Sub C_Responsable_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Responsable.BeforeDropDown
        Dim _SQL As String = ""
        If oLinqInstalacion.ID_Personal.HasValue Then
            _SQL = " or ID_Personal= " & oLinqInstalacion.ID_Personal
        End If
        'Util.Cargar_Combo(Me.C_Responsable, "Select ID_Personal, Nombre From Personal Where Activo=1 and FechaBajaEmpresa is null" & _SQL & " Order by Nombre", False, False)
        Util.Cargar_Combo(Me.C_Responsable, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and FechaBajaEmpresa is null and ID_Personal_Tipo in (" & EnumPersonalTipo.Comercial & " , " & EnumPersonalTipo.ResponsableCuenta & ")" & _SQL & " ORDER BY Nombre", False, True, "No asignado")
        Me.C_Responsable.Value = oLinqInstalacion.ID_Personal
    End Sub

    Private Sub B_Atras_Click(sender As Object, e As EventArgs) Handles B_Atras.Click
        Call AvanzarRetroceder(False)
    End Sub

    Private Sub B_Adelante_Click(sender As Object, e As EventArgs) Handles B_Adelante.Click
        Call AvanzarRetroceder(True)
    End Sub
#End Region

#Region "Diseño Riesgo"

#Region "Grid Riesgo Tipo"

    Private Sub Carga_Grid_Riesgo_Tipo(ByVal pId As Integer)
        Try

            With Me.GRD_Riesgo_Tipo

                Dim _Instalacion_Instalacion_Estudio_Riesgo_Tipo As IEnumerable(Of Instalacion_Instalacion_Estudio_Riesgo_Tipo) = From taula In oDTC.Instalacion_Instalacion_Estudio_Riesgo_Tipo Where taula.ID_Instalacion = pId Select taula

                .M.clsUltraGrid.CargarIEnumerable(_Instalacion_Instalacion_Estudio_Riesgo_Tipo)
                '.GRID.DataSource = Instalacion_Instalacion_Estudio_Riesgo_Tipo

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Tipo").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarComboValoracion(Me.GRD_Riesgo_Tipo)
                Call CargarComboRiesgo_Tipo()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboRiesgo_Tipo()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oRiesgoTipo As IQueryable(Of Instalacion_Estudio_Riesgo_Tipo) = (From Taula In oDTC.Instalacion_Estudio_Riesgo_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Estudio_Riesgo_Tipo

            For Each Var In oRiesgoTipo
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Riesgo_Tipo.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Tipo").Style = ColumnStyle.DropDownList
            GRD_Riesgo_Tipo.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_RiesgoTipo_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Riesgo_Tipo.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Riesgo Valor"

    Private Sub Carga_Grid_Riesgo_Valor(ByVal pId As Integer)
        Try

            With Me.GRD_Riesgo_Valor
                Dim _RiesgoValor As IEnumerable(Of Instalacion_Instalacion_Estudio_Riesgo_Valor) = From taula In oDTC.Instalacion_Instalacion_Estudio_Riesgo_Valor Where taula.ID_Instalacion = pId Select taula

                '.GRID.DataSource = Instalacion_Instalacion_Estudio_Riesgo_Valor
                .M.clsUltraGrid.CargarIEnumerable(_RiesgoValor)

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Valor").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarComboValoracion(Me.GRD_Riesgo_Valor)
                Call CargarComboRiesgo_Valor()

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboRiesgo_Valor()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oRiesgoValor As IQueryable(Of Instalacion_Estudio_Riesgo_Valor) = (From Taula In oDTC.Instalacion_Estudio_Riesgo_Valor Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Estudio_Riesgo_Valor

            For Each Var In oRiesgoValor
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Riesgo_Valor.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Valor").Style = ColumnStyle.DropDownList
            GRD_Riesgo_Valor.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Valor").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_RiesgoValor_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Riesgo_Valor.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    'Private Sub GRD_Riesgo_Valor_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Valor.M_ToolGrid_ToolAfegir
    '    Try

    '        If oLinqInstalacion.ID_Instalacion = 0 Then
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '            Exit Sub
    '        End If


    '        Me.GRD_Riesgo_Valor.GRID.DisplayLayout.Bands(0).AddNew() 'Afegim una Row al Grid
    '        'Me.GRD.GRID.Rows(Me.GRD.GRID.Rows.Count - 1).Cells("ID_Associat").Value = (From Taula In oDTC.Associats Where Taula.Activo = True Select Taula.ID_Associat).FirstOrDefault
    '        'Me.GRD_Caracteristicas_Personalizadas.GRID.Rows(Me.GRD_Caracteristicas_Personalizadas.GRID.Rows.Count - 1).Cells("ID_Producto_Caracteristica").ValueList.SelectedItemIndex = -1



    '        Me.GRD_Riesgo_Valor.GRID.Rows(Me.GRD_Riesgo_Valor.GRID.Rows.Count - 1).Cells("ID_Valoracion").Value = "-1"
    '        'Establim que la variable Linia és el mateix que la última row del grid recient afegida
    '        Dim Linia As New Instalacion_Instalacion_Estudio_Riesgo_Valor
    '        Linia = Me.GRD_Riesgo_Valor.GRID.Rows.GetItem(Me.GRD_Riesgo_Valor.GRID.Rows.Count - 1).listObject

    '        'Afegim aquesta línia a la colecció de línies del actual albarà
    '        oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Valor.Add(Linia)

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Riesgo_Valor_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Valor.M_ToolGrid_ToolEliminar
    '    If Me.GRD_Riesgo_Valor.GRID.Selected.Rows.Count = 1 Then
    '        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
    '            Dim pRow As UltraGridRow
    '            If Me.GRD_Riesgo_Valor.GRID.Selected.Cells.Count > 0 Then
    '                pRow = Me.GRD_Riesgo_Valor.GRID.Selected.Cells(0).Row
    '            Else
    '                pRow = Me.GRD_Riesgo_Valor.GRID.Selected.Rows(0)
    '            End If

    '            Dim Linea As Instalacion_Instalacion_Estudio_Riesgo_Valor = pRow.ListObject
    '            If Linea.ID_Instalacion_Instalacion_Estudio_Riesgo_Valor <> 0 Then
    '                oDTC.Instalacion_Instalacion_Estudio_Riesgo_Valor.DeleteOnSubmit(Linea)
    '            End If

    '            'pRow.Hidden = True
    '            oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Valor.Remove(Linea)
    '            'Linea.Activo = False
    '            oDTC.SubmitChanges()
    '        End If
    '    End If
    'End Sub

#End Region

#Region "Grid Riesgo Volumen"

    Private Sub Carga_Grid_Riesgo_Volumen(ByVal pId As Integer)
        Try

            With Me.GRD_Riesgo_Volumen

                Dim _Riesgo_Volumen As IEnumerable(Of Instalacion_Instalacion_Estudio_Riesgo_Volumen) = From taula In oDTC.Instalacion_Instalacion_Estudio_Riesgo_Volumen Where taula.ID_Instalacion = pId Select taula

                .M.clsUltraGrid.CargarIEnumerable(_Riesgo_Volumen)
                '.GRID.DataSource = Instalacion_Instalacion_Estudio_Riesgo_Volumen

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Volumen").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarComboValoracion(Me.GRD_Riesgo_Volumen)
                Call CargarComboRiesgo_Volumen()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboRiesgo_Volumen()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oRiesgoVolumen As IQueryable(Of Instalacion_Estudio_Riesgo_Volumen) = (From Taula In oDTC.Instalacion_Estudio_Riesgo_Volumen Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Estudio_Riesgo_Volumen

            For Each Var In oRiesgoVolumen
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Riesgo_Volumen.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Volumen").Style = ColumnStyle.DropDownList
            GRD_Riesgo_Volumen.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Volumen").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_RiesgoVolumen_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Riesgo_Volumen.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    'Private Sub GRD_Riesgo_Volumen_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Volumen.M_ToolGrid_ToolAfegir
    '    Try

    '        If oLinqInstalacion.ID_Instalacion = 0 Then
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '            Exit Sub
    '        End If


    '        Me.GRD_Riesgo_Volumen.GRID.DisplayLayout.Bands(0).AddNew() 'Afegim una Row al Grid
    '        'Me.GRD.GRID.Rows(Me.GRD.GRID.Rows.Count - 1).Cells("ID_Associat").Value = (From Taula In oDTC.Associats Where Taula.Activo = True Select Taula.ID_Associat).FirstOrDefault
    '        'Me.GRD_Caracteristicas_Personalizadas.GRID.Rows(Me.GRD_Caracteristicas_Personalizadas.GRID.Rows.Count - 1).Cells("ID_Producto_Caracteristica").ValueList.SelectedItemIndex = -1



    '        Me.GRD_Riesgo_Volumen.GRID.Rows(Me.GRD_Riesgo_Volumen.GRID.Rows.Count - 1).Cells("ID_Valoracion").Value = "-1"
    '        'Establim que la variable Linia és el mateix que la última row del grid recient afegida
    '        Dim Linia As New Instalacion_Instalacion_Estudio_Riesgo_Volumen
    '        Linia = Me.GRD_Riesgo_Volumen.GRID.Rows.GetItem(Me.GRD_Riesgo_Volumen.GRID.Rows.Count - 1).listObject

    '        'Afegim aquesta línia a la colecció de línies del actual albarà
    '        oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Volumen.Add(Linia)

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Riesgo_Volumen_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Volumen.M_ToolGrid_ToolEliminar
    '    If Me.GRD_Riesgo_Volumen.GRID.Selected.Rows.Count = 1 Then
    '        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
    '            Dim pRow As UltraGridRow
    '            If Me.GRD_Riesgo_Volumen.GRID.Selected.Cells.Count > 0 Then
    '                pRow = Me.GRD_Riesgo_Volumen.GRID.Selected.Cells(0).Row
    '            Else
    '                pRow = Me.GRD_Riesgo_Volumen.GRID.Selected.Rows(0)
    '            End If

    '            Dim Linea As Instalacion_Instalacion_Estudio_Riesgo_Volumen = pRow.ListObject
    '            If Linea.ID_Instalacion_Instalacion_Estudio_Riesgo_Volumen <> 0 Then
    '                oDTC.Instalacion_Instalacion_Estudio_Riesgo_Volumen.DeleteOnSubmit(Linea)
    '            End If

    '            'pRow.Hidden = True
    '            oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Volumen.Remove(Linea)
    '            'Linea.Activo = False
    '            oDTC.SubmitChanges()
    '        End If
    '    End If
    'End Sub

#End Region

#Region "Grid Riesgo Peligros"

    Private Sub Carga_Grid_Riesgo_Peligros(ByVal pId As Integer)
        Try

            With Me.GRD_Riesgo_Peligros

                Dim _Riesgo_Peligros As IEnumerable(Of Instalacion_Instalacion_Estudio_Riesgo_Peligros) = From taula In oDTC.Instalacion_Instalacion_Estudio_Riesgo_Peligros Where taula.ID_Instalacion = pId Select taula
                '.GRID.DataSource = Instalacion_Instalacion_Estudio_Riesgo_Peligros
                .M.clsUltraGrid.CargarIEnumerable(_Riesgo_Peligros)

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Peligros").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarComboValoracion(Me.GRD_Riesgo_Peligros)
                Call CargarComboRiesgo_Peligros()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboRiesgo_Peligros()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oRiesgoPeligros As IQueryable(Of Instalacion_Estudio_Riesgo_Peligros) = (From Taula In oDTC.Instalacion_Estudio_Riesgo_Peligros Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Estudio_Riesgo_Peligros

            For Each Var In oRiesgoPeligros
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Riesgo_Peligros.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Peligros").Style = ColumnStyle.DropDownList
            GRD_Riesgo_Peligros.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Peligros").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Riesgo_Peligros_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Riesgo_Peligros.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    'Private Sub GRD_Riesgo_Peligros_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Peligros.M_ToolGrid_ToolAfegir
    '    Try

    '        If oLinqInstalacion.ID_Instalacion = 0 Then
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '            Exit Sub
    '        End If


    '        Me.GRD_Riesgo_Peligros.GRID.DisplayLayout.Bands(0).AddNew() 'Afegim una Row al Grid
    '        'Me.GRD.GRID.Rows(Me.GRD.GRID.Rows.Count - 1).Cells("ID_Associat").Value = (From Taula In oDTC.Associats Where Taula.Activo = True Select Taula.ID_Associat).FirstOrDefault
    '        'Me.GRD_Caracteristicas_Personalizadas.GRID.Rows(Me.GRD_Caracteristicas_Personalizadas.GRID.Rows.Count - 1).Cells("ID_Producto_Caracteristica").ValueList.SelectedItemIndex = -1



    '        Me.GRD_Riesgo_Peligros.GRID.Rows(Me.GRD_Riesgo_Peligros.GRID.Rows.Count - 1).Cells("ID_Valoracion").Value = "-1"
    '        'Establim que la variable Linia és el mateix que la última row del grid recient afegida
    '        Dim Linia As New Instalacion_Instalacion_Estudio_Riesgo_Peligros
    '        Linia = Me.GRD_Riesgo_Peligros.GRID.Rows.GetItem(Me.GRD_Riesgo_Peligros.GRID.Rows.Count - 1).listObject

    '        'Afegim aquesta línia a la colecció de línies del actual albarà
    '        oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Peligros.Add(Linia)

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Riesgo_Peligros_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Peligros.M_ToolGrid_ToolEliminar
    '    If Me.GRD_Riesgo_Peligros.GRID.Selected.Rows.Count = 1 Then
    '        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
    '            Dim pRow As UltraGridRow
    '            If Me.GRD_Riesgo_Peligros.GRID.Selected.Cells.Count > 0 Then
    '                pRow = Me.GRD_Riesgo_Peligros.GRID.Selected.Cells(0).Row
    '            Else
    '                pRow = Me.GRD_Riesgo_Peligros.GRID.Selected.Rows(0)
    '            End If

    '            Dim Linea As Instalacion_Instalacion_Estudio_Riesgo_Peligros = pRow.ListObject
    '            If Linea.ID_Instalacion_Instalacion_Estudio_Riesgo_Peligros <> 0 Then
    '                oDTC.Instalacion_Instalacion_Estudio_Riesgo_Peligros.DeleteOnSubmit(Linea)
    '            End If

    '            'pRow.Hidden = True
    '            oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Peligros.Remove(Linea)
    '            'Linea.Activo = False
    '            oDTC.SubmitChanges()
    '        End If
    '    End If
    'End Sub

#End Region

#Region "Grid Riesgo Daños"

    Private Sub Carga_Grid_Riesgo_Daños(ByVal pId As Integer)
        Try

            With Me.GRD_Riesgo_Daños

                Dim _Riesgo_Daños As IEnumerable(Of Instalacion_Instalacion_Estudio_Riesgo_Daños) = From taula In oDTC.Instalacion_Instalacion_Estudio_Riesgo_Daños Where taula.ID_Instalacion = pId Select taula

                .M.clsUltraGrid.CargarIEnumerable(_Riesgo_Daños)
                '.GRID.DataSource = _Riesgo_Daños

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Daños").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarComboValoracion(Me.GRD_Riesgo_Daños)
                Call CargarComboRiesgo_Daños()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboRiesgo_Daños()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oRiesgoDaños As IQueryable(Of Instalacion_Estudio_Riesgo_Daños) = (From Taula In oDTC.Instalacion_Estudio_Riesgo_Daños Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Estudio_Riesgo_Daños

            For Each Var In oRiesgoDaños
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Riesgo_Daños.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Daños").Style = ColumnStyle.DropDownList
            GRD_Riesgo_Daños.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Daños").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Riesgo_Daños_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Riesgo_Daños.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    'Private Sub GRD_Riesgo_Daños_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Daños.M_ToolGrid_ToolAfegir
    '    Try

    '        If oLinqInstalacion.ID_Instalacion = 0 Then
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '            Exit Sub
    '        End If


    '        Me.GRD_Riesgo_Daños.GRID.DisplayLayout.Bands(0).AddNew() 'Afegim una Row al Grid
    '        'Me.GRD.GRID.Rows(Me.GRD.GRID.Rows.Count - 1).Cells("ID_Associat").Value = (From Taula In oDTC.Associats Where Taula.Activo = True Select Taula.ID_Associat).FirstOrDefault
    '        'Me.GRD_Caracteristicas_Personalizadas.GRID.Rows(Me.GRD_Caracteristicas_Personalizadas.GRID.Rows.Count - 1).Cells("ID_Producto_Caracteristica").ValueList.SelectedItemIndex = -1



    '        Me.GRD_Riesgo_Daños.GRID.Rows(Me.GRD_Riesgo_Daños.GRID.Rows.Count - 1).Cells("ID_Valoracion").Value = "-1"
    '        'Establim que la variable Linia és el mateix que la última row del grid recient afegida
    '        Dim Linia As New Instalacion_Instalacion_Estudio_Riesgo_Daños
    '        Linia = Me.GRD_Riesgo_Daños.GRID.Rows.GetItem(Me.GRD_Riesgo_Daños.GRID.Rows.Count - 1).listObject

    '        'Afegim aquesta línia a la colecció de línies del actual albarà
    '        oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Daños.Add(Linia)

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Riesgo_Daños_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Daños.M_ToolGrid_ToolEliminar
    '    If Me.GRD_Riesgo_Daños.GRID.Selected.Rows.Count = 1 Then
    '        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
    '            Dim pRow As UltraGridRow
    '            If Me.GRD_Riesgo_Daños.GRID.Selected.Cells.Count > 0 Then
    '                pRow = Me.GRD_Riesgo_Daños.GRID.Selected.Cells(0).Row
    '            Else
    '                pRow = Me.GRD_Riesgo_Daños.GRID.Selected.Rows(0)
    '            End If

    '            Dim Linea As Instalacion_Instalacion_Estudio_Riesgo_Daños = pRow.ListObject
    '            If Linea.ID_Instalacion_Instalacion_Estudio_Riesgo_Daños <> 0 Then
    '                oDTC.Instalacion_Instalacion_Estudio_Riesgo_Daños.DeleteOnSubmit(Linea)
    '            End If

    '            'pRow.Hidden = True
    '            oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Daños.Remove(Linea)
    '            'Linea.Activo = False
    '            oDTC.SubmitChanges()
    '        End If
    '    End If
    'End Sub

#End Region

#Region "Grid Riesgo Historia"

    Private Sub Carga_Grid_Riesgo_Historia(ByVal pId As Integer)
        Try

            With Me.GRD_Riesgo_Historia

                Dim _Riesgo_Historia = From taula In oDTC.Instalacion_Instalacion_Estudio_Riesgo_Historia Where taula.ID_Instalacion = pId Select taula

                .M.clsUltraGrid.CargarIEnumerable(_Riesgo_Historia)
                '.GRID.DataSource = _Riesgo_Historia

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Historia").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarComboRiesgo_Historia_Valoracion()
                Call CargarComboRiesgo_Historia()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboRiesgo_Historia()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oRiesgoHistoria As IQueryable(Of Instalacion_Estudio_Riesgo_Historia) = (From Taula In oDTC.Instalacion_Estudio_Riesgo_Historia Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Estudio_Riesgo_Historia

            For Each Var In oRiesgoHistoria
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Riesgo_Historia.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Historia").Style = ColumnStyle.DropDownList
            GRD_Riesgo_Historia.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Historia").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarComboRiesgo_Historia_Valoracion()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oRiesgoHistoria_Valoracion As IQueryable(Of Instalacion_Estudio_Riesgo_Historia_Valoracion) = (From Taula In oDTC.Instalacion_Estudio_Riesgo_Historia_Valoracion Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Estudio_Riesgo_Historia_Valoracion

            For Each Var In oRiesgoHistoria_Valoracion
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Riesgo_Historia.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Historia_Valoracion").Style = ColumnStyle.DropDownList
            GRD_Riesgo_Historia.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Estudio_Riesgo_Historia_Valoracion").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Riesgo_Historia_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Riesgo_Historia.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    'Private Sub GRD_Riesgo_Historia_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Historia.M_ToolGrid_ToolAfegir
    '    Try

    '        If oLinqInstalacion.ID_Instalacion = 0 Then
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '            Exit Sub
    '        End If


    '        Me.GRD_Riesgo_Historia.GRID.DisplayLayout.Bands(0).AddNew() 'Afegim una Row al Grid
    '        'Me.GRD.GRID.Rows(Me.GRD.GRID.Rows.Count - 1).Cells("ID_Associat").Value = (From Taula In oDTC.Associats Where Taula.Activo = True Select Taula.ID_Associat).FirstOrDefault
    '        'Me.GRD_Caracteristicas_Personalizadas.GRID.Rows(Me.GRD_Caracteristicas_Personalizadas.GRID.Rows.Count - 1).Cells("ID_Producto_Caracteristica").ValueList.SelectedItemIndex = -1



    '        'Me.GRD_Riesgo_Historia.GRID.Rows(Me.GRD_Riesgo_Historia.GRID.Rows.Count - 1).Cells("ID_Instalacion_Estudio_Riesgo_Historia_Valoracion").Value = "-1"
    '        'Establim que la variable Linia és el mateix que la última row del grid recient afegida
    '        Dim Linia As New Instalacion_Instalacion_Estudio_Riesgo_Historia
    '        Linia = Me.GRD_Riesgo_Historia.GRID.Rows.GetItem(Me.GRD_Riesgo_Historia.GRID.Rows.Count - 1).listObject

    '        'Afegim aquesta línia a la colecció de línies del actual albarà
    '        oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Historia.Add(Linia)

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Riesgo_Historia_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Riesgo_Historia.M_ToolGrid_ToolEliminar
    '    If Me.GRD_Riesgo_Historia.GRID.Selected.Rows.Count = 1 Then
    '        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
    '            Dim pRow As UltraGridRow
    '            If Me.GRD_Riesgo_Historia.GRID.Selected.Cells.Count > 0 Then
    '                pRow = Me.GRD_Riesgo_Historia.GRID.Selected.Cells(0).Row
    '            Else
    '                pRow = Me.GRD_Riesgo_Historia.GRID.Selected.Rows(0)
    '            End If

    '            Dim Linea As Instalacion_Instalacion_Estudio_Riesgo_Historia = pRow.ListObject
    '            If Linea.ID_Instalacion_Instalacion_Estudio_Riesgo_Historia <> 0 Then
    '                oDTC.Instalacion_Instalacion_Estudio_Riesgo_Historia.DeleteOnSubmit(Linea)
    '            End If

    '            'pRow.Hidden = True
    '            oLinqInstalacion.Instalacion_Instalacion_Estudio_Riesgo_Historia.Remove(Linea)
    '            'Linea.Activo = False
    '            oDTC.SubmitChanges()
    '        End If
    '    End If
    'End Sub

#End Region

#End Region

#Region "Diseño Emplazamiento"

#Region "Grid Emplazamiento"

    Private Sub CargaGrid_Emplazamiento(ByVal pId As Integer)
        Try

            With Me.GRD_Emplazamiento

                ' Dim oDTC2 As New DTCDataContext(BD.Conexion)
                'oDTC2.ObjectTrackingEnabled = False
                'oDTC2.DeferredLoadingEnabled = True
                ' Me.GRD_Emplazamiento.GRID.DisplayLayout.LoadStyle = LoadStyle.LoadOnDemand

                Dim _InstalacionEmplazamiento As IEnumerable(Of Instalacion_Emplazamiento) = From taula In oDTC.Instalacion_Emplazamiento Where taula.ID_Instalacion = pId Select taula

                .M.clsUltraGrid.CargarIEnumerable(_InstalacionEmplazamiento)
                'Me.GRD_Emplazamiento.GRID.DataSource = InstalacionEmplazamiento

                .M_Editable()

                Me.GRD_Emplazamiento.GRID.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Emplazamiento_M_GRID_AfterRowActivate(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Emplazamiento.M_GRID_AfterRowActivate
        'If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing Then
        '    MsgBox("a")
        'End If
        'If Me.GRD_Emplazamiento.GRID.Selected.Rows.Count = 0 Then
        '    Me.GRD_Emplazamiento.Tag = "1" 'xapuça que fa que si l'event ha saltat per l'afterselectchange no faci en el afterrow unaltre cop la neteja de grids
        '    Call CarregarGrids_Emplazamiento(0)
        '    Me.GRD_Emplazamiento.Tag = Nothing
        'End If
        Dim pIDEmplazamiento As Integer = Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value
        Call CarregarGrids_Emplazamiento(pIDEmplazamiento)
        Me.GRD_Emplazamiento.GRID.ActiveRow.Selected = True

    End Sub

    Private Sub GRD_Emplazamiento_M_GRID_AfterSelectChange(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles GRD_Emplazamiento.M_GRID_AfterSelectChange
        '   MsgBox("a")
    End Sub

    Private Sub GRD_Emplazamiento_M_GRID_Click(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Emplazamiento.M_GRID_Click
        If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing Then
            Me.GRD_Emplazamiento.Tag = "1" 'xapuça que fa que si l'event ha saltat per l'afterselectchange no faci en el afterrow unaltre cop la neteja de grids
            Call CarregarGrids_Emplazamiento(0)
            Me.GRD_Emplazamiento.Tag = Nothing
        End If
    End Sub
    Private Sub GRD_Emplazamiento_M_GRID_ClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Emplazamiento.M_GRID_ClickRow
        'If Me.GRD_Emplazamiento.GRID.Selected.Rows.Count = 1 Then
        '    Dim pIDEmplazamiento As Integer = Me.GRD_Emplazamiento.GRID.Selected.Rows(0).Cells("ID_Instalacion_Emplazamiento").Value
        '    Call CarregarGrids_Emplazamiento(pIDEmplazamiento)
        'End If
    End Sub

    Private Sub GRD_Emplazamiento_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Emplazamiento.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Emplazamiento

                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()

                .M_AfegirFila()

                If .GRID.Rows.Count = 1 Then 'Si es el primer emplazamiento que creem automaticament li direm que és la predeterminada
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("Predeterminada").Value = True
                End If

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion").Value = oLinqInstalacion

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Emplazamiento_M_ToolGrid_ToolEliminar(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Emplazamiento.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then

                Dim Linea As Instalacion_Emplazamiento = e.ListObject

                If Linea.Predeterminada = True Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar una ubicación predeterminada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Linea.Parte_Instalacion_Emplazamiento.Count > 0 Or Linea.Instalacion_ElementosAProteger.Count > 0 Or Linea.Instalacion_Emplazamiento_Abertura.Count > 0 Or Linea.Instalacion_Emplazamiento_Construccion.Count > 0 Or Linea.Instalacion_Emplazamiento_Custodia.Count > 0 Or Linea.Instalacion_Emplazamiento_Entorno.Count > 0 Or Linea.Instalacion_Emplazamiento_HistoriaRobo.Count > 0 Or Linea.Instalacion_Emplazamiento_InfluenciaExt.Count > 0 Or Linea.Instalacion_Emplazamiento_InfluenciaInt.Count > 0 Or Linea.Instalacion_Emplazamiento_Localizacion.Count > 0 Or Linea.Instalacion_Emplazamiento_Ocupacion.Count > 0 Or Linea.Instalacion_Emplazamiento_Planta.Count > 0 Or Linea.Instalacion_Emplazamiento_Zona.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar el registro, hay registros activos", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Exit Sub
                End If

                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Emplazamiento_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Emplazamiento.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
        Call ActivarTabsEnFuncioDelEstatDeLaInstalacio()
        If Me.GRD_Emplazamiento.Tag Is Nothing Then 'xapuça per culpa d'events. si es nothing netejarem
            Call NetejarGrids_Emplazamiento()
        End If
    End Sub

#End Region

#Region "Grid Planta"

    Private Sub CargaGrid_Planta(ByVal pId As Integer)
        Try
            With Me.GRD_Planta

                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing Then
                    '.GRID.DataSource = oDTC.Instalacion_Emplazamiento_Planta.Where(Function(F) F.ID_Instalacion_Emplazamiento = pId)
                    Dim _Planta2 As IEnumerable(Of Instalacion_Emplazamiento_Planta) = From taula In oDTC.Instalacion_Emplazamiento_Planta Where taula.ID_Instalacion_Emplazamiento = pId Select taula
                    .M.clsUltraGrid.CargarIEnumerable(_Planta2)
                    Exit Sub
                End If

                Dim LiniaEmplazamiento As Instalacion_Emplazamiento
                LiniaEmplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject

                Dim _Planta As IEnumerable(Of Instalacion_Emplazamiento_Planta) = From taula In oDTC.Instalacion_Emplazamiento_Planta Where taula.ID_Instalacion_Emplazamiento = pId Select taula

                .M.clsUltraGrid.CargarIEnumerable(_Planta)
                '.GRID.DataSource = _Planta

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Identificador").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Identificador").CellAppearance.ForeColor = Color.Black

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Planta_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Planta.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Planta

                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                'Dim IDEmplazamiento As Integer = LiniaEmplazamiento.ID_Instalacion_Emplazamiento

                .M_ExitEditMode()
                .M_AfegirFila()

                Me.GRD_Planta.GRID.Rows(Me.GRD_Planta.GRID.Rows.Count - 1).Cells("Identificador").Value = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Planta
                oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Planta = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Planta + 1

                Me.GRD_Planta.GRID.Rows(Me.GRD_Planta.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Planta_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Planta.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim _Planta As Instalacion_Emplazamiento_Planta = e.ListObject
                If _Planta.Instalacion_Emplazamiento_Zona.Count > 0 OrElse _Planta.Instalacion_Emplazamiento_Abertura.Count > 0 OrElse _Planta.Instalacion_Contacto.Count > 0 OrElse _Planta.Instalacion_ElementosAProteger.Count > 0 OrElse _Planta.Instalacion_InstaladoEn.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar, esta zona está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If


                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Planta_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Planta.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
        Dim _Emplazamiento As Instalacion_Emplazamiento = RetornaEmplazamiento()
        If _Emplazamiento Is Nothing = False Then
            Call CargarCombo_Planta(Me.GRD_Zona.GRID, 0, oLinqInstalacion.ID_Instalacion, _Emplazamiento.ID_Instalacion_Emplazamiento)
            Call CargarCombo_Planta(Me.GRD_Abertura.GRID, 0, oLinqInstalacion.ID_Instalacion, _Emplazamiento.ID_Instalacion_Emplazamiento)
            Call CargarCombo_Planta(Me.GRD_ElementosAProteger.GRID, 0, oLinqInstalacion.ID_Instalacion, _Emplazamiento.ID_Instalacion_Emplazamiento)
        End If
    End Sub

    Private Sub GRD_Planta_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Planta.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "Multiplicar" Then
            If Me.GRD_Planta.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            Dim frm As New frmAuxiliarDesdeHasta
            frm.Entrada(Me.GRD_Planta.GRID.Selected.Rows(0).Cells("Descripcion").Value)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFrmDesdeHastaPlantas

            frm.FormObrir(Me, False)

        End If
    End Sub

    Private Sub AlTancarFrmDesdeHastaPlantas(ByVal pDescripcio As String, ByVal pDesde As Integer, ByVal pHasta As Integer)
        Dim _Emplazamiento As Instalacion_Emplazamiento
        If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
            Exit Sub
        Else
            _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
        End If


        Dim _Indentificador As Integer = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Planta

        Dim _OldPlanta As Instalacion_Emplazamiento_Planta = Me.GRD_Planta.GRID.Selected.Rows(0).ListObject
        Dim i As Integer
        For i = pDesde To pHasta
            Dim _NewPlanta As New Instalacion_Emplazamiento_Planta
            With _NewPlanta
                .Descripcion = pDescripcio & "-" & i
                .Identificador = _Indentificador
                _Indentificador = _Indentificador + 1
                _Emplazamiento.Instalacion_Emplazamiento_Planta.Add(_NewPlanta)
                oDTC.Instalacion_Emplazamiento_Planta.InsertOnSubmit(_NewPlanta)
            End With
        Next

        oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Planta = _Indentificador
        oDTC.SubmitChanges()
        Call CargaGrid_Planta(_Emplazamiento.ID_Instalacion_Emplazamiento)
        Mensaje.Mostrar_Mensaje("Datos introducidos correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
    End Sub

    'Private Sub CargarComboEmplazamiento4(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pID As Integer)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Instalacion_Emplazamiento) = (From Taula In oDTC.Instalacion_Emplazamiento Where Taula.ID_Instalacion = pID Order By Taula.Descripcion Select Taula)
    '        Dim Var As Instalacion_Emplazamiento

    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Descripcion)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

#End Region

#Region "Grid Zonas"

    Private Sub CargaGrid_Zona(ByVal pId As Integer)
        Try

            With Me.GRD_Zona

                Dim _Zonas As IEnumerable(Of Instalacion_Emplazamiento_Zona) = From taula In oDTC.Instalacion_Emplazamiento_Zona Where taula.ID_Instalacion_Emplazamiento = pId Select taula

                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing Then
                    ' .GRID.DataSource = oDTC.Instalacion_Emplazamiento_Zona.Where(Function(F) F.ID_Instalacion_Emplazamiento = pId)
                    .M.clsUltraGrid.CargarIEnumerable(_Zonas)
                    Exit Sub
                End If

                .M.clsUltraGrid.CargarIEnumerable(_Zonas)
                ' .GRID.DataSource = _Zonas

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Identificador").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)

                ' Call CargarCombo_Planta4(pId, .GRID)
                Call CargarCombo_Planta(.GRID, 0, oLinqInstalacion.ID_Instalacion, pId)

                Call CargarCombo_ClaseAmbiental(.GRID)
                Call CargarCombo_Grado(.GRID)

                Call CargarCombo_EstandardNema(.GRID)
                Call CargarCombo_Luminosidad(.GRID)
                Call CargarCombo_TipoDetector(.GRID)


                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Intrusion"), RetornaColorSegonsDivisio(EnumProductoDivision.Intrusion))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Producto_ClaseAmbiental"), RetornaColorSegonsDivisio(EnumProductoDivision.Intrusion))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Producto_Grado"), RetornaColorSegonsDivisio(EnumProductoDivision.Intrusion))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Numerico"), RetornaColorSegonsDivisio(EnumProductoDivision.Intrusion))

                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("CCTV"), RetornaColorSegonsDivisio(EnumProductoDivision.CCTV))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Producto_EstandardNema"), RetornaColorSegonsDivisio(EnumProductoDivision.CCTV))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Producto_Luminosidad"), RetornaColorSegonsDivisio(EnumProductoDivision.CCTV))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("NumElementos"), RetornaColorSegonsDivisio(EnumProductoDivision.CCTV))

                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Incendios"), RetornaColorSegonsDivisio(EnumProductoDivision.Incendios))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Producto_TipoDetector"), RetornaColorSegonsDivisio(EnumProductoDivision.Incendios))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("NumDetectores"), RetornaColorSegonsDivisio(EnumProductoDivision.Incendios))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("NumPulsadores"), RetornaColorSegonsDivisio(EnumProductoDivision.Incendios))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("NumSirenas"), RetornaColorSegonsDivisio(EnumProductoDivision.Incendios))

                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Megafonia"), RetornaColorSegonsDivisio(EnumProductoDivision.Megafonia))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("NumAltavoces"), RetornaColorSegonsDivisio(EnumProductoDivision.Megafonia))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Producto_EstandardNema1"), RetornaColorSegonsDivisio(EnumProductoDivision.Megafonia))

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub CargarCombo_Planta4(ByVal pID As Integer, ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta) = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.ID_Instalacion_Emplazamiento = pID Order By Taula.Descripcion Select Taula)
    '        Dim Var As Instalacion_Emplazamiento_Planta

    '        'Valors.ValueListItems.Add("-1", "Seleccione un registros")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Descripcion)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Planta").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    'Private Sub CargarCombo_Planta(ByVal pID As Integer, ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta) = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.ID_Instalacion_Emplazamiento = pID Order By Taula.Descripcion Select Taula)
    '        Dim Var As Instalacion_Emplazamiento_Planta

    '        Valors.ValueListItems.Add("-1", "Seleccione un registros")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Planta, Var.Descripcion)
    '        Next


    '        pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    Private Sub CargarCombo_ClaseAmbiental(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_ClaseAmbiental) = (From Taula In oDTC.Producto_ClaseAmbiental Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_ClaseAmbiental

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_ClaseAmbiental").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_ClaseAmbiental").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Grado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Grado) = (From Taula In oDTC.Producto_Grado Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Grado

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_Grado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_Grado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_EstandardNema(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_EstandardNema) = (From Taula In oDTC.Producto_EstandardNema Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_EstandardNema

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_EstandardNema").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_EstandardNema").ValueList = Valors.Clone

            pGrid.DisplayLayout.Bands(0).Columns("Producto_EstandardNema1").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_EstandardNema1").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Luminosidad(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Luminosidad) = (From Taula In oDTC.Producto_Luminosidad Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Luminosidad

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_Luminosidad").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_Luminosidad").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TipoDetector(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_TipoDetector) = (From Taula In oDTC.Producto_TipoDetector Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_TipoDetector

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_TipoDetector").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_TipoDetector").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Zona_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Zona.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Zona
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                'Dim IDEmplazamiento As Integer
                Dim _LiniaEmplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _LiniaEmplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                    'IDEmplazamiento = LiniaEmplazamiento.ID_Instalacion_Emplazamiento
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Identificador").Value = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Zona
                oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Zona = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Zona + 1

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _LiniaEmplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Planta").Value = "-1"
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Numerico").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("NumElementos").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("NumDetectores").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("NumPulsadores").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("NumSirenas").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("NumAltavoces").Value = 0
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Producto_ClaseAmbiental").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Producto_Grado").Value = "-1"

                ''Establim que la variable Linia és el mateix que la última row del grid recient afegida
                'Dim Linia As New Instalacion_Emplazamiento_Zona
                'Linia = .GRID.Rows.GetItem(Me.GRD_Zona.GRID.Rows.Count - 1).listObject

                ''Afegim aquesta línia a la colecció de línies del actual albarà
                ''oLinqInstalacion.Instalacion_Emplazamiento(1).Instalacion_Emplazamiento_Planta.Add(Linia)
                'LiniaEmplazamiento.Instalacion_Emplazamiento_Zona.Add(Linia)
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Zona_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Zona.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "Multiplicar" Then
            If Me.GRD_Zona.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            Dim frm As New frmAuxiliarDesdeHasta
            frm.Entrada(Me.GRD_Zona.GRID.Selected.Rows(0).Cells("Descripcion").Value)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFrmDesdeHastaZonas

            frm.FormObrir(Me, False)

        End If
    End Sub

    Private Sub GRD_Zona_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Zona.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim Linea As Instalacion_Emplazamiento_Zona = e.ListObject
                If SeUsaLaZona(Linea) = True Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar el registro, el registro está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                    Exit Sub
                End If

                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function SeUsaLaZona(ByVal Zona As Instalacion_Emplazamiento_Zona) As Boolean
        Try

            SeUsaLaZona = False
            If oDTC.Propuesta_Linea.Count(Function(F) F.ID_Instalacion_Emplazamiento_Zona = Zona.ID_Instalacion_Emplazamiento_Zona) > 0 Then
                SeUsaLaZona = True
            End If

            If oDTC.Instalacion_ElementosAProteger.Count(Function(F) F.ID_Instalacion_Emplazamiento_Zona = Zona.ID_Instalacion_Emplazamiento_Zona) > 0 Then
                SeUsaLaZona = True
            End If

            If oDTC.Instalacion_Emplazamiento_Abertura.Count(Function(F) F.ID_Instalacion_Emplazamiento_Zona = Zona.ID_Instalacion_Emplazamiento_Zona) > 0 Then
                SeUsaLaZona = True
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    'Private Sub GRD_Zona_AfterRowInsert(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Zona.M_GRID_AfterRowInsert
    '    oDTC.SubmitChanges()
    'End Sub

    Private Sub GRD_Zona_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Zona.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub AlTancarFrmDesdeHastaZonas(ByVal pDescripcio As String, ByVal pDesde As Integer, ByVal pHasta As Integer)
        Dim _Emplazamiento As Instalacion_Emplazamiento
        If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
            Exit Sub
        Else
            _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
        End If


        Dim _Indentificador As Integer = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Zona

        Dim _OldZona As Instalacion_Emplazamiento_Zona = Me.GRD_Zona.GRID.Selected.Rows(0).ListObject
        Dim i As Integer
        For i = pDesde To pHasta
            Dim _NewZona As New Instalacion_Emplazamiento_Zona
            With _NewZona
                .CCTV = _OldZona.CCTV
                .Descripcion = pDescripcio & "-" & i
                .Descripcion_Detallada = _OldZona.Descripcion_Detallada
                .Identificador = _Indentificador
                _Indentificador = _Indentificador + 1
                .Incendios = _OldZona.Incendios

                '.Instalacion_ElementosAProteger = _OldZona.Instalacion_ElementosAProteger
                '.Instalacion_Emplazamiento = _OldZona.Instalacion_Emplazamiento
                .ID_Instalacion_Emplazamiento = _OldZona.ID_Instalacion_Emplazamiento
                '.Instalacion_Emplazamiento_Abertura = _OldZona.Instalacion_Emplazamiento_Abertura
                '.Instalacion_Emplazamiento_Planta = _OldZona.Instalacion_Emplazamiento_Planta
                .ID_Instalacion_Emplazamiento_Planta = _OldZona.ID_Instalacion_Emplazamiento_Planta
                .Intrusion = _OldZona.Intrusion
                .Megafonia = _OldZona.Megafonia
                .NumAltavoces = _OldZona.NumAltavoces
                .NumDetectores = _OldZona.NumDetectores
                .NumElementos = _OldZona.NumElementos
                .Numerico = _OldZona.Numerico
                .NumPulsadores = _OldZona.NumPulsadores
                .NumSirenas = _OldZona.NumSirenas
                '.Producto_ClaseAmbiental = _OldZona.Producto_ClaseAmbiental
                .ID_Producto_ClaseAmbiental = _OldZona.ID_Producto_ClaseAmbiental
                '.Producto_EstandardNema = _OldZona.Producto_EstandardNema
                .ID_Producto_EstandardNema = _OldZona.ID_Producto_EstandardNema
                '.Producto_EstandardNema1 = _OldZona.Producto_EstandardNema1
                .ID_Producto_EstandardNema_Megafonia = _OldZona.ID_Producto_EstandardNema_Megafonia
                '.Producto_Grado = _OldZona.Producto_Grado
                .ID_Producto_Grado = _OldZona.ID_Producto_Grado
                '.Producto_Luminosidad = _OldZona.Producto_Luminosidad
                .ID_Producto_Luminosidad = _OldZona.ID_Producto_Luminosidad
                '.Producto_TipoDetector = _OldZona.Producto_TipoDetector
                .ID_Producto_TipoDetector = _OldZona.ID_Producto_TipoDetector
                _Emplazamiento.Instalacion_Emplazamiento_Zona.Add(_NewZona)
                oDTC.Instalacion_Emplazamiento_Zona.InsertOnSubmit(_NewZona)
            End With
        Next

        oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Zona = _Indentificador
        oDTC.SubmitChanges()
        Call CargaGrid_Zona(_Emplazamiento.ID_Instalacion_Emplazamiento)
        Mensaje.Mostrar_Mensaje("Datos introducidos correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
    End Sub

#End Region

#Region "Grid Construccion"

    Private Sub CargaGrid_Construccion(ByVal pId As Integer)
        Try
            Dim _Construccion As IEnumerable(Of Instalacion_Emplazamiento_Construccion) = From taula In oDTC.Instalacion_Emplazamiento_Construccion Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_Construccion
                .M.clsUltraGrid.CargarIEnumerable(_Construccion)
                '.GRID.DataSource = _Construccion

                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Identificador").CellActivation = UltraWinGrid.Activation.NoEdit

                .M_Editable()

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Construccion_Elemento(.GRID)
                Call CargarCombo_Construccion_Tipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Construccion_Elemento(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Construccion_Elemento) = (From Taula In oDTC.Instalacion_Emplazamiento_Construccion_Elemento Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Construccion_Elemento

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Construccion_Elemento").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Construccion_Elemento").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarCombo_Construccion_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Construccion_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_Construccion_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Construccion_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Construccion_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Construccion_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Construccion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Construccion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Construccion
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                ' Dim IDEmplazamiento As Integer
                Dim _Emplazamiento As Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                    'IDEmplazamiento = LiniaEmplazamiento.ID_Instalacion_Emplazamiento
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Identificador").Value = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Construccion
                oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Construccion = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Construccion + 1

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Construccion_Tipo").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Construccion_Elemento").Value = "-1"

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Construccion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Construccion.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Construccion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Construccion.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Ocupacion"

    Private Sub CargaGrid_Ocupacion(ByVal pId As Integer)
        Try
            Dim _Ocupacion As IEnumerable(Of Instalacion_Emplazamiento_Ocupacion) = From taula In oDTC.Instalacion_Emplazamiento_Ocupacion Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_Ocupacion
                .M.clsUltraGrid.CargarIEnumerable(_Ocupacion)
                '.GRID.DataSource = _Ocupacion

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Ocupacion_Estado(.GRID)
                Call CargarCombo_Ocupacion_Tipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Ocupacion_Estado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Ocupacion_Estado) = (From Taula In oDTC.Instalacion_Emplazamiento_Ocupacion_Estado Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Ocupacion_Estado

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Ocupacion_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Ocupacion_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarCombo_Ocupacion_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Ocupacion_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_Ocupacion_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Ocupacion_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Ocupacion_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Ocupacion_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Ocupacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Ocupacion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Ocupacion
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                ' .GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Ocupacion_Tipo").Value = "-1"
                ' .GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Ocupacion_Estado").Value = "-1"
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Ocupacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Ocupacion.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Ocupacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Ocupacion.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Custodia"

    Private Sub CargaGrid_Custodia(ByVal pId As Integer)
        Try
            Dim _Custodia As IEnumerable(Of Instalacion_Emplazamiento_Custodia) = From taula In oDTC.Instalacion_Emplazamiento_Custodia Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_Custodia
                .M.clsUltraGrid.CargarIEnumerable(_Custodia)
                '.GRID.DataSource = _Custodia

                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit
                .M_Editable()

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Custodia_Estado(.GRID)
                Call CargarCombo_Custodia_Tipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Custodia_Estado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Custodia_Estado) = (From Taula In oDTC.Instalacion_Emplazamiento_Custodia_Estado Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Custodia_Estado

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Custodia_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Custodia_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarCombo_Custodia_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Custodia_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_Custodia_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Custodia_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Custodia_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Custodia_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Custodia_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Custodia.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Custodia
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Custodia_Tipo").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Custodia_Estado").Value = "-1"
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Custodia_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Custodia.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Custodia_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Custodia.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Localizacion"

    Private Sub CargaGrid_Localizacion(ByVal pId As Integer)
        Try
            Dim _Localizacion As IEnumerable(Of Instalacion_Emplazamiento_Localizacion) = From taula In oDTC.Instalacion_Emplazamiento_Localizacion Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_Localizacion
                .M.clsUltraGrid.CargarIEnumerable(_Localizacion)
                '.GRID.DataSource = _Localizacion

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Localizacion_Estado(.GRID)
                Call CargarCombo_Localizacion_Tipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Localizacion_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Localizacion_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_Localizacion_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Localizacion_Tipo

            ' Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Localizacion_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Localizacion_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarCombo_Localizacion_Estado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Localizacion_Estado) = (From Taula In oDTC.Instalacion_Emplazamiento_Localizacion_Estado Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Localizacion_Estado

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Localizacion_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Localizacion_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Localizacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Localizacion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Localizacion
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Localizacion_Tipo").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Localizacion_Estado").Value = "-1"
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Localizacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Localizacion.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Localizacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Localizacion.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid SeguridadExistente"

    Private Sub CargaGrid_SeguridadExistente(ByVal pId As Integer)
        Try
            Dim _SeguridadExistente As IEnumerable(Of Instalacion_Emplazamiento_SeguridadExistente) = From taula In oDTC.Instalacion_Emplazamiento_SeguridadExistente Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_SeguridadExistente
                .M.clsUltraGrid.CargarIEnumerable(_SeguridadExistente)
                '.GRID.DataSource = _SeguridadExistente

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_SeguridadExistente_Respuesta(.GRID)
                Call CargarCombo_SeguridadExistente_Tipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_SeguridadExistente_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_SeguridadExistente_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_SeguridadExistente_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_SeguridadExistente_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_SeguridadExistente_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_SeguridadExistente_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarCombo_SeguridadExistente_Respuesta(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_SeguridadExistente_Respuesta) = (From Taula In oDTC.Instalacion_Emplazamiento_SeguridadExistente_Respuesta Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_SeguridadExistente_Respuesta

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_SeguridadExistente_Respuesta").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_SeguridadExistente_Respuesta").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_SeguridadExistente_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_SeguridadExistente.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_SeguridadExistente
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject

                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_SeguridadExistente_Tipo").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_SeguridadExistente_Respuesta").Value = "-1"

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_SeguridadExistente_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_SeguridadExistente.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_SeguridadExistente_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_SeguridadExistente.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Historia de Robos"

    Private Sub CargaGrid_HistoriaRobo(ByVal pId As Integer)
        Try

            With Me.GRD_HistoriaRobo
                Dim _HistoriaRobo As IEnumerable(Of Instalacion_Emplazamiento_HistoriaRobo) = From taula In oDTC.Instalacion_Emplazamiento_HistoriaRobo Where taula.ID_Instalacion_Emplazamiento = pId Select taula
                .M.clsUltraGrid.CargarIEnumerable(_HistoriaRobo)
                '.GRID.DataSource = _HistoriaRobo

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_HistoriaRobo_Tipo(.GRID)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_HistoriaRobo_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_HistoriaRobo_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_HistoriaRobo_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_HistoriaRobo_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_HistoriaRobo_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_HistoriaRobo_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_HistoriaRobo_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_HistoriaRobo.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_HistoriaRobo
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento_HistoriaRobo_Tipo").Value = "-1"

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_HistoriaRobo_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_HistoriaRobo.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_HistoriaRobo_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_HistoriaRobo.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Legislacion"

    Private Sub CargaGrid_Legislacion(ByVal pId As Integer)
        Try
            Dim _Legislacion As IEnumerable(Of Instalacion_Emplazamiento_Legislacion) = From taula In oDTC.Instalacion_Emplazamiento_Legislacion Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_Legislacion
                .M.clsUltraGrid.CargarIEnumerable(_Legislacion)
                '.GRID.DataSource = _Legislacion

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Legislacion_Tipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Legislacion_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Legislacion_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_Legislacion_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Legislacion_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Legislacion_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Legislacion_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Legislacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Legislacion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Legislacion
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Legislacion_Tipo").Value = "-1"

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Legislacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Legislacion.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Legislacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Legislacion.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Entorno"

    Private Sub CargaGrid_Entorno(ByVal pId As Integer)
        Try
            Dim _Entorno As IEnumerable(Of Instalacion_Emplazamiento_Entorno) = From taula In oDTC.Instalacion_Emplazamiento_Entorno Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_Entorno
                .M.clsUltraGrid.CargarIEnumerable(_Entorno)
                '.GRID.DataSource = _Entorno

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Entorno_Tipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Entorno_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Entorno_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_Entorno_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Entorno_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Entorno_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Entorno_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Entorno_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Entorno.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Entorno
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Entorno_Tipo").Value = "-1"

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Entorno_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Entorno.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Entorno_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Entorno.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Influencias Internas"

    Private Sub CargaGrid_InfluenciaInt(ByVal pId As Integer)
        Try
            Dim _InfluenciaInt As IEnumerable(Of Instalacion_Emplazamiento_InfluenciaInt) = From taula In oDTC.Instalacion_Emplazamiento_InfluenciaInt Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_InfluenciaInt
                .M.clsUltraGrid.CargarIEnumerable(_InfluenciaInt)
                '.GRID.DataSource = _InfluenciaInt

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_InfluenciaInt_Tipo(.GRID)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_InfluenciaInt_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_InfluenciaInt_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_InfluenciaInt_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_InfluenciaInt_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_InfluenciaInt_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_InfluenciaInt_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_InfluenciaInt_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_InfluenciaInt.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_InfluenciaInt
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_InfluenciaInt_Tipo").Value = "-1"
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_InfluenciaInt_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_InfluenciaInt.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_InfluenciaInt_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_InfluenciaInt.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Influencias Externas"

    Private Sub CargaGrid_InfluenciaExt(ByVal pId As Integer)
        Try

            Dim _InfluenciaExt As IEnumerable(Of Instalacion_Emplazamiento_InfluenciaExt) = From taula In oDTC.Instalacion_Emplazamiento_InfluenciaExt Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_InfluenciaExt
                .M.clsUltraGrid.CargarIEnumerable(_InfluenciaExt)
                '.GRID.DataSource = _InfluenciaExt

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_InfluenciaExt_Tipo(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_InfluenciaExt_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_InfluenciaExt_Tipo) = (From Taula In oDTC.Instalacion_Emplazamiento_InfluenciaExt_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_InfluenciaExt_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_InfluenciaExt_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_InfluenciaExt_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_InfluenciaExt_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_InfluenciaExt.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_InfluenciaExt
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If


                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_InfluenciaExt_Tipo").Value = "-1"
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_InfluenciaExt_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_InfluenciaExt.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_InfluenciaExt_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_InfluenciaExt.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Aberturas"

    Private Sub CargaGrid_Abertura(ByVal pId As Integer)
        Try
            Dim _Abertura As IEnumerable(Of Instalacion_Emplazamiento_Abertura) = From taula In oDTC.Instalacion_Emplazamiento_Abertura Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_Abertura
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing Then
                    '.GRID.DataSource = oDTC.Instalacion_Emplazamiento_Abertura.Where(Function(F) F.ID_Instalacion_Emplazamiento = pId)
                    .M.clsUltraGrid.CargarIEnumerable(_Abertura)
                    Exit Sub
                End If

                .M.clsUltraGrid.CargarIEnumerable(_Abertura)
                '.GRID.DataSource = _Abertura

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Identificador").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Planta(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Zona(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Elemento(.GRID)
                Call CargarCombo_Construccion_Tipo(.GRID)
                Call CargarCombo_TipoLector(.GRID)
                Call CargarCombo_TipoCerradura(.GRID)
                Call CargarCombo_IncendioTipoElemento(.GRID)

                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Intrusion"), RetornaColorSegonsDivisio(EnumProductoDivision.Intrusion))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Abertura_Elemento"), RetornaColorSegonsDivisio(EnumProductoDivision.Intrusion))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Construccion_Tipo"), RetornaColorSegonsDivisio(EnumProductoDivision.Intrusion))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Numerico"), RetornaColorSegonsDivisio(EnumProductoDivision.Intrusion))

                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Accesos"), RetornaColorSegonsDivisio(EnumProductoDivision.Accesos))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Producto_TipoLector"), RetornaColorSegonsDivisio(EnumProductoDivision.Accesos))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Producto_TipoCerradura"), RetornaColorSegonsDivisio(EnumProductoDivision.Accesos))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("NumElementosAccessos"), RetornaColorSegonsDivisio(EnumProductoDivision.Accesos))

                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Incendios"), RetornaColorSegonsDivisio(EnumProductoDivision.Incendios))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("Producto_IncendioTipoElemento"), RetornaColorSegonsDivisio(EnumProductoDivision.Incendios))
                .M.clsUltraGrid.ColumnaFondoColor(.GRID.DisplayLayout.Bands(0).Columns("NumElementosIncendio"), RetornaColorSegonsDivisio(EnumProductoDivision.Incendios))

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Abertura_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Abertura.M_GRID_BeforeCellActivate
        'Select Case e.Cell.Column.Key
        '    Case "Instalacion_Emplazamiento_Zona"
        '        'If e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value Is Nothing OrElse IsDBNull(e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value) Then
        '        '    ' Mensaje.Mostrar_Mensaje(141, M_Mensaje.Missatge_Modo.INFORMACIO)
        '        '    e.Cancel = True
        '        'Else
        '        Call CargarCombo_Zona(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value)
        '        'End If
        'End Select
        Call GRD_InstaladoEn_M_GRID_BeforeCellActivate(sender, e)
    End Sub

    Private Sub GRD_Abertura_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Abertura.M_GRID_CellListSelect
        Call GRD_InstaladoEn_M_GRID_CellListSelect(sender, e)
        'If e.Cell.Column.Key = "Instalacion_Emplazamiento_Planta" Then
        '    Call CargarCombo_Zona(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value)
        '    e.Cell.Row.Cells("Instalacion_Emplazamiento_Zona").Value = Nothing
        'End If

    End Sub

    Private Sub CargarCombo_Elemento(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Abertura_Elemento) = (From Taula In oDTC.Instalacion_Emplazamiento_Abertura_Elemento Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Abertura_Elemento

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Abertura_Elemento").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento_Abertura_Elemento").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TipoLector(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_TipoLector) = (From Taula In oDTC.Producto_TipoLector Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_TipoLector

            ' Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_TipoLector").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_TipoLector").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TipoCerradura(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_TipoCerradura) = (From Taula In oDTC.Producto_TipoCerradura Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_TipoCerradura

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_TipoCerradura").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_TipoCerradura").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_IncendioTipoElemento(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_IncendioTipoElemento) = (From Taula In oDTC.Producto_IncendioTipoElemento Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_IncendioTipoElemento

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_IncendioTipoElemento").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_IncendioTipoElemento").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Abertura_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Abertura.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Abertura
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Identificador").Value = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Planta
                oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Planta = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Planta + 1

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Numerico").Value = "1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Planta").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Zona").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Abertura_Elemento").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Construccion_Tipo").Value = "-1"
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Abertura_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Abertura.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim Linea As Instalacion_Emplazamiento_Abertura = e.ListObject
                If SeUsaLaAbertura(Linea) = True Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar el registro, el registro está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                    Exit Sub
                End If
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Abertura_M_GRID_BeforeRowUpdate(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles GRD_Abertura.M_GRID_BeforeRowUpdate

        Dim _Emplazamiento As New Instalacion_Emplazamiento
        If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
            Exit Sub
        Else
            _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
        End If

        'If e.Row.ListIndex = -1 Then  'fem això pq peta si es perd el focus de la row quan la row es nova i no s'han afegit tots els camps requerits
        '    Exit Sub
        'End If

        Dim Linia As New Instalacion_Emplazamiento_Abertura
        Linia = e.Row.ListObject

        If Linia Is Nothing Then
            Exit Sub
        End If
        If IsDBNull(Linia.Numerico) = False AndAlso Linia.Numerico > 0 Then
        Else
            If Linia.ID_Instalacion_Emplazamiento_Abertura = 0 Then
                _Emplazamiento.Instalacion_Emplazamiento_Abertura.Remove(Linia)
                e.Cancel = True
                Exit Sub
            Else
                e.Row.Cells("Numerico").Value = e.Row.Cells("Numerico").OriginalValue
            End If
            Mensaje.Mostrar_Mensaje("El número de aberturas mínimo és de 1", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
        End If

        Dim _IDZona As Integer = e.Row.Cells("ID_Instalacion_Emplazamiento_Zona").Value
        Dim _Descripcion As String = e.Row.Cells("Descripcion_Detallada").Value

        If _Emplazamiento.Instalacion_Emplazamiento_Abertura.Where(Function(F) F.ID_Instalacion_Emplazamiento_Abertura <> Linia.ID_Instalacion_Emplazamiento_Abertura And F.ID_Instalacion_Emplazamiento_Zona = _IDZona And F.Descripcion_Detallada = _Descripcion).Count > 0 Then
            Mensaje.Mostrar_Mensaje("No pueden haber dos aberturas en la misma zona y con el mismo detalle", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            If Linia.ID_Instalacion_Emplazamiento_Abertura = 0 Then
                _Emplazamiento.Instalacion_Emplazamiento_Abertura.Remove(Linia)
                e.Cancel = True
            Else
                e.Row.Cells("Descripcion_Detallada").Value = e.Row.Cells("Descripcion_Detallada").OriginalValue
            End If
        End If

    End Sub

    Private Sub GRD_Abertura_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Abertura.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Function SeUsaLaAbertura(ByVal Abertura As Instalacion_Emplazamiento_Abertura) As Boolean
        Try

            SeUsaLaAbertura = False
            If oDTC.Propuesta_Linea.Count(Function(F) F.ID_Instalacion_Emplazamiento_Abertura = Abertura.ID_Instalacion_Emplazamiento_Abertura) > 0 Then
                SeUsaLaAbertura = True
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub GRD_Abertura_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Abertura.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "Multiplicar" Then
            If Me.GRD_Abertura.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            Dim frm As New frmAuxiliarDesdeHasta
            frm.Entrada(Me.GRD_Abertura.GRID.Selected.Rows(0).Cells("Descripcion_Detallada").Value)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFrmDesdeHastaAberturas

            frm.FormObrir(Me, False)

        End If
    End Sub

    Private Sub AlTancarFrmDesdeHastaAberturas(ByVal pDescripcio As String, ByVal pDesde As Integer, ByVal pHasta As Integer)
        Dim _Emplazamiento As Instalacion_Emplazamiento
        If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
            Exit Sub
        Else
            _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
        End If


        Dim _Indentificador As Integer = oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Abertura

        Dim _OldAbertura As Instalacion_Emplazamiento_Abertura = Me.GRD_Abertura.GRID.Selected.Rows(0).ListObject
        Dim i As Integer
        For i = pDesde To pHasta
            Dim _NewAbertura As New Instalacion_Emplazamiento_Abertura
            With _NewAbertura
                .Accesos = _OldAbertura.Accesos
                .Descripcion_Detallada = pDescripcio & "-" & i
                .Identificador = _Indentificador
                _Indentificador = _Indentificador + 1
                .Incendios = _OldAbertura.Incendios
                '.Instalacion_Emplazamiento = _OldAbertura.Instalacion_Emplazamiento
                .ID_Instalacion_Emplazamiento = _OldAbertura.ID_Instalacion_Emplazamiento
                '.Instalacion_Emplazamiento_Abertura_Elemento = _OldAbertura.Instalacion_Emplazamiento_Abertura_Elemento
                .ID_Instalacion_Emplazamiento_Abertura_Elemento = _OldAbertura.ID_Instalacion_Emplazamiento_Abertura_Elemento

                '.Instalacion_Emplazamiento_Construccion_Tipo = _OldAbertura.Instalacion_Emplazamiento_Construccion_Tipo
                .ID_Instalacion_Emplazamiento_Construccion_Tipo = _OldAbertura.ID_Instalacion_Emplazamiento_Construccion_Tipo
                '.Instalacion_Emplazamiento_Planta = _OldAbertura.Instalacion_Emplazamiento_Planta
                .ID_Instalacion_Emplazamiento_Planta = _OldAbertura.ID_Instalacion_Emplazamiento_Planta
                '.Instalacion_Emplazamiento_Zona = _OldAbertura.Instalacion_Emplazamiento_Zona
                .ID_Instalacion_Emplazamiento_Zona = _OldAbertura.ID_Instalacion_Emplazamiento_Zona
                .Intrusion = _OldAbertura.Intrusion
                .NumElementosAccessos = _OldAbertura.NumElementosAccessos
                .NumElementosIncendio = _OldAbertura.NumElementosIncendio
                .Numerico = _OldAbertura.Numerico
                '.Producto_IncendioTipoElemento = _OldAbertura.Producto_IncendioTipoElemento
                .ID_Producto_IncendioTipoElemento = _OldAbertura.ID_Producto_IncendioTipoElemento
                '.Producto_TipoCerradura = _OldAbertura.Producto_TipoCerradura
                .ID_Producto_TipoCerradura = _OldAbertura.ID_Producto_TipoCerradura
                '.Producto_TipoLector = _OldAbertura.Producto_TipoLector
                .ID_Producto_TipoLector = _OldAbertura.ID_Producto_TipoLector
                _Emplazamiento.Instalacion_Emplazamiento_Abertura.Add(_NewAbertura)
                oDTC.Instalacion_Emplazamiento_Abertura.InsertOnSubmit(_NewAbertura)
            End With
        Next

        oDTC.Contadores.FirstOrDefault.Instalacion_Emplazamiento_Abertura = _Indentificador
        oDTC.SubmitChanges()
        Call CargaGrid_Abertura(_Emplazamiento.ID_Instalacion_Emplazamiento)
        Mensaje.Mostrar_Mensaje("Datos introducidos correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
    End Sub

#End Region

#Region "Grid Elementos a Proteger"

    Private Sub CargaGrid_ElementosAProteger(ByVal pId As Integer)
        Try
            Dim _ElementosAProteger As IEnumerable(Of Instalacion_ElementosAProteger) = From taula In oDTC.Instalacion_ElementosAProteger Where taula.ID_Instalacion_Emplazamiento = pId Select taula

            With Me.GRD_ElementosAProteger
                '.GRID.DataSource = _ElementosAProteger
                .M.clsUltraGrid.CargarIEnumerable(_ElementosAProteger)

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("Instalacion_Emplazamiento").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Planta(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Zona(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_ElementosAProteger_Tipo(.GRID)
                Call CargarComboValoracion(Me.GRD_ElementosAProteger)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_ElementosAProteger_Tipo(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_ElementosAProteger_Tipo) = (From Taula In oDTC.Instalacion_ElementosAProteger_Tipo Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_ElementosAProteger_Tipo

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_ElementosAProteger_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_ElementosAProteger_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ElementosAProteger_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ElementosAProteger.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_ElementosAProteger
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Emplazamiento As New Instalacion_Emplazamiento
                If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Emplazamiento.GRID.ActiveRow.Cells("ID_Instalacion_Emplazamiento").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                Else
                    _Emplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Emplazamiento").Value = _Emplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Planta").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento_Zona").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_ElementosAProteger_Tipo").Value = "-1"
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Valoracion").Value = "-1"
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ElementosAProteger_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_ElementosAProteger.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim Linea As Instalacion_ElementosAProteger = e.ListObject
                If SeUsaElElementoAProteger(Linea) = True Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar el registro, el registro está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                    Exit Sub
                End If
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ElementosAProteger_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_ElementosAProteger.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_ElementosAProteger_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_ElementosAProteger.M_GRID_BeforeCellActivate
        Call GRD_InstaladoEn_M_GRID_BeforeCellActivate(sender, e)
        'Select Case e.Cell.Column.Key
        '    Case "Instalacion_Emplazamiento_Zona"
        '        'If e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value Is Nothing OrElse IsDBNull(e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value) Then
        '        '    ' Mensaje.Mostrar_Mensaje(141, M_Mensaje.Missatge_Modo.INFORMACIO)
        '        '    e.Cancel = True
        '        'Else
        '        Call CargarCombo_Zona2(0, Me.GRD_ElementosAProteger.GRID, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value)
        '        'End If
        'End Select
    End Sub

    Private Sub GRD_ElementosAProteger_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_ElementosAProteger.M_GRID_CellListSelect
        'If e.Cell.Column.Key = "Instalacion_Emplazamiento_Planta" Then
        '    Call CargarCombo_Zona2(0, Me.GRD_ElementosAProteger.GRID, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value)
        '    e.Cell.Row.Cells("Instalacion_Emplazamiento_Zona").Value = Nothing
        'End If
        Call GRD_InstaladoEn_M_GRID_CellListSelect(sender, e)
    End Sub

    Private Function SeUsaElElementoAProteger(ByVal ElementoAProteger As Instalacion_ElementosAProteger) As Boolean
        Try

            SeUsaElElementoAProteger = False
            If oDTC.Propuesta_Linea.Count(Function(F) F.ID_Instalacion_ElementosAProteger = ElementoAProteger.ID_Instalacion_ElementosAProteger) > 0 Then
                SeUsaElElementoAProteger = True
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

#End Region

#End Region

#Region "Propuesta"

    Private Sub CargaGrid_Propuesta(ByVal pId As Integer)
        Try
            'Dim Propuesta = From taula In oDTC.Propuesta Where taula.ID_Instalacion = pId Select taula

            With Me.GRD_Propuesta
                Dim _FiltrePerEstats As String = ""
                If pId <> 0 And C_Propuesta_Estado.Text <> "Todos" Then
                    _FiltrePerEstats = " and ID_Propuesta_Estado=" & Me.C_Propuesta_Estado.Value
                End If


                .M.clsUltraGrid.Cargar("Select * From C_Propuesta Where Activo=1 " & _FiltrePerEstats & " and SeInstalo=0 and ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " Order by Codigo", BD)
                '.GRID.DataSource = Propuesta

                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Propuesta.GRID.Rows
                    If pRow.Cells("ID_Propuesta_Estado").Value = EnumPropuestaEstado.Aprobado Then
                        pRow.Appearance.BackColor = Color.LightGreen
                    End If
                    If pRow.Cells("ID_Propuesta_Estado").Value = EnumPropuestaEstado.Traspasado Then
                        pRow.Appearance.BackColor = Color.LightYellow
                    End If
                Next

                If oTipoEntrada = TipoEntrada.FuturaInstalacion Then
                    Me.GRD_Propuesta.M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.Invisible, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "InstalacionAnterior", "Aprobar", "Traspasar", "VerParte", "GenerarPedidoVenta", "GenerarParte")
                End If
            End With

            Me.GRD_Propuesta.GRID.ActiveRow = Nothing

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Propuesta_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Propuesta.M_ToolGrid_ToolAfegir
        Try
            If oLinqInstalacion.ID_Instalacion = 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If

            If oLinqInstalacion.Instalacion_Emplazamiento.Count = 0 Then
                If Mensaje.Mostrar_Mensaje("No se puede crear una propuesta hasta que no se hayan creado las ubicaciones. Desea Crearla?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
                    Dim _Resposta As String = Mensaje.Mostrar_Entrada_Datos("Introduzca el nombre de la ubicación", "", False, False)
                    If _Resposta Is Nothing Or _Resposta = "" Then
                        Exit Sub
                    Else
                        Dim _Emplazamiento As New Instalacion_Emplazamiento
                        _Emplazamiento.Descripcion = _Resposta
                        oLinqInstalacion.Instalacion_Emplazamiento.Add(_Emplazamiento)
                        oDTC.SubmitChanges()
                    End If
                Else
                    Exit Sub
                End If

            End If

            If Guardar(True) = False Then
                Exit Sub
            End If

            Dim frm As New frmPropuesta
            frm.Entrada(oLinqInstalacion, oDTC)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmPropuesta
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Propuesta_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Propuesta.M_ToolGrid_ToolClickBotonsExtras

        Select Case e.Tool.Key
            Case "VerParte"
                If Me.GRD_Propuesta.GRID.Selected.Cells.Count = 0 And Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If
                If IsDBNull(Me.GRD_Propuesta.GRID.ActiveRow.Cells("ID_Parte").Value) = True Then
                    Mensaje.Mostrar_Mensaje("No hay ningún parte de instalación asignado a esta propuesta", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Exit Sub
                End If

                Dim IDParte As Integer = Me.GRD_Propuesta.GRID.ActiveRow.Cells("ID_Parte").Value

                Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                frm.Entrada(IDParte)
                frm.FormObrir(Me, True)


            Case "Aprobar"
                If Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If

                Dim IDPropuesta As Integer = Me.GRD_Propuesta.GRID.ActiveRow.Cells("ID_Propuesta").Value
                Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = IDPropuesta).FirstOrDefault
                If _Propuesta.ID_Propuesta_Estado <> EnumPropuestaEstado.Pendiente Then
                    Mensaje.Mostrar_Mensaje("Sólo se puede dar por aprobada una propuesta en estado pendiente", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Exit Sub
                End If

                If _Propuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then
                    Mensaje.Mostrar_Mensaje("No se puede dar por aprobada una propuesta de tipo 'Instalación anterior'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If _Propuesta.Propuesta_Linea.Where(Function(F) F.Activo = True).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede dar por aprobada una propuesta vacía", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If _Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Instalacion_Emplazamiento.HasValue = False).Count > 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede dar por aprobada una propuesta que tenga líneas sin un emplazamiento asignado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Desea dar por aprobada la propuesta?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
                    Dim Fecha As DateTime
                    Fecha = _Propuesta.Fecha
                    If _Propuesta.Validez.HasValue = True AndAlso Now.Date > Fecha.AddDays(_Propuesta.Validez) Then
                        If Mensaje.Mostrar_Mensaje("Se ha superado el período de validez de la propuesta, está seguro de querer aprobar la propuesta igualmente?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.NO Then
                            Exit Sub
                        End If
                    End If

                    _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Aprobado
                    clsPropuesta.CambiarEstadoPropuestaCRM(oDTC, _Propuesta, EnumPropuestaEstado.Aprobado)


                    Dim _Linea As Propuesta_Linea
                    For Each _Linea In _Propuesta.Propuesta_Linea
                        _Linea.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Aprobado_PendienteRecibir
                    Next
                    oDTC.SubmitChanges()
                    Mensaje.Mostrar_Mensaje("Propuesta aprobada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)

                    Dim frm As New frmInstalacionCreacionPartes
                    frm.Entrada(oLinqInstalacion, _Propuesta, oDTC)
                    frm.FormObrir(Me, False)
                    AddHandler frm.AlTancarForm, AddressOf AlTancarfrmPropuesta

                    'If Mensaje.Mostrar_Mensaje("¿Desea crear el pedido de venta correspondiente?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    '    Util.WaitFormObrir()
                    '    Dim _NewEntrada As New Entrada
                    '    Dim oclsEntrada As New clsEntrada(oDTC, _NewEntrada)
                    '    oclsEntrada.CrearPedidodeVentaAPartirDeUnaPropuesta(_Propuesta)

                    '    Dim _ClsNotificacion As New clsNotificacion(oDTC)
                    '    _ClsNotificacion.CrearNotificacion_AlCrearPedidoVenta(_NewEntrada)

                    '    Util.WaitFormTancar()
                    'End If

                    'If Mensaje.Mostrar_Mensaje("¿Desea crear el parte de trabajo correspondiente?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
                    '    Dim frm As New frmInstalacionCreacionPartes
                    '    frm.Entrada(oLinqInstalacion, _Propuesta, oDTC)
                    '    frm.FormObrir(Me, False)
                    '    AddHandler frm.AlTancarForm, AddressOf AlTancarfrmPropuesta
                    'End If


                End If
                Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)
                Call ActivarTabsEnFuncioDelEstatDeLaInstalacio()

            Case "Traspasar"
                If Me.GRD_Propuesta.GRID.Selected.Cells.Count = 0 And Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If

                Dim IDPropuesta As Integer = Me.GRD_Propuesta.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value
                Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = IDPropuesta).FirstOrDefault
                If _Propuesta.ID_Propuesta_Estado <> EnumPropuestaEstado.Aprobado Then
                    If _Propuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior And _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Pendiente Then 'Les instalacions anteriors si que es poden traspasar sense estar aprobades
                    Else
                        If _Propuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then
                            Mensaje.Mostrar_Mensaje("Imposible traspasar dos veces la misma instalación anterior", M_Mensaje.Missatge_Modo.INFORMACIO)
                        Else
                            Mensaje.Mostrar_Mensaje("Sólo se puede traspasar una propuesta en estado aprobado", M_Mensaje.Missatge_Modo.INFORMACIO)
                        End If
                        Exit Sub
                    End If
                End If

                If _Propuesta.Propuesta_Linea.Where(Function(F) F.Activo = True).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede dar por aprobada una propuesta vacía", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                Dim frm As New frmInstalacionTraspaso
                frm.Entrada(oLinqInstalacion, _Propuesta, oDTC)
                frm.FormObrir(Me, True)
                AddHandler frm.AlTancarForm, AddressOf AlTancarFormInstalacionTraspaso


            Case "InstalacionAnterior"
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                If oLinqInstalacion.Instalacion_Emplazamiento.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede crear una propuesta hasta que no se hayan creado las ubicaciones", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Exit Sub
                End If

                If Guardar(True) = False Then
                    Exit Sub
                End If

                Dim frm As New frmPropuesta
                frm.Entrada(oLinqInstalacion, oDTC, , True)
                AddHandler frm.FormClosing, AddressOf AlTancarfrmPropuesta
                frm.FormObrir(Me)

            Case "NegativoPositivo"
                If Me.GRD_Propuesta.GRID.Selected.Cells.Count = 0 And Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If

                Dim _IDPropuesta As Integer = Me.GRD_Propuesta.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value

                Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = _IDPropuesta).FirstOrDefault
                If _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Traspasado Then
                    Mensaje.Mostrar_Mensaje("Imposible dar por negativa una propuesta traspasada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Aprobado Then
                    If _Propuesta.Parte.Where(Function(F) F.Activo = True).Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible dar por negativa una propuesta aprobada con un parte relacionado. Elimine el parte relacionado para poder realizar la acción.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If
                    If _Propuesta.Entrada_Propuesta.Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible dar por negativa una propuesta aprobada con un pedido de venta relacionado. Elimine el pedido de venta relacionado para poder realizar la acción.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If
                End If

                If _Propuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then 'Les instalacions anteriors si que es poden traspasar sense estar aprobades
                    Mensaje.Mostrar_Mensaje("Este tipo de propuesta no se puede dar por negativa", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                'Select Case _Propuesta.ID_Propuesta_Estado
                '    Case EnumPropuestaEstado.Pendiente
                Call clsPropuesta.PasarLaPropuestaANegativaoPositiva(oDTC, _IDPropuesta)
                '    Case EnumPropuestaEstado.Negativo
                '        Call clsPropuesta.PasarLaPropuestaANegativaoPositiva(clsPropuesta.EnumTipoTraspaso.aPositiva, oDTC, _IDPropuesta)
                'End Select

                Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)

                'If _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Pendiente Then
                '    If Mensaje.Mostrar_Mensaje("¿Desea dar por negativa la propuesta?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                '        _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Negativo
                '        Call CambiarEstadoPropuestaCRM(_Propuesta, EnumPropuestaEstado.Negativo)
                '        oDTC.SubmitChanges()
                '        Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)
                '        Exit Sub
                '    End If
                'End If

                'If _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Negativo Then
                '    If Mensaje.Mostrar_Mensaje("¿Desea quitar el estado 'negativa' a la propuesta?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                '        _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Pendiente
                '        Call CambiarEstadoPropuestaCRM(_Propuesta, EnumPropuestaEstado.Pendiente)
                '        oDTC.SubmitChanges()
                '        Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)
                '        Exit Sub
                '    End If
                'End If

            Case "DuplicarPropuesta"
                If Me.GRD_Propuesta.GRID.Selected.Cells.Count = 0 And Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Está seguro que desea duplicar la propuesta seleccionada?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Dim _IDPropuesta As Integer = Me.GRD_Propuesta.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value

                If PucDuplicarPropuesta(_IDPropuesta) = False Then
                    Mensaje.Mostrar_Mensaje("Imposible duplicar si no se tiene permisos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                Util.WaitFormObrir()
                Call DuplicarPressupost(_IDPropuesta, oLinqInstalacion)

                Mensaje.Mostrar_Mensaje("Propuesta duplicada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)
                Call Cargar_Form(oLinqInstalacion.ID_Instalacion)
                Me.TAB_Principal.Tabs("Propuesta").Selected = True
                Util.WaitFormTancar()

            Case "DuplicarPropuestaEnOtraInstalacion"
                If Me.GRD_Propuesta.GRID.Selected.Cells.Count = 0 And Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If

                Dim _IDPropuesta As Integer = Me.GRD_Propuesta.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value
                If PucDuplicarPropuesta(_IDPropuesta) = False Then
                    Mensaje.Mostrar_Mensaje("Imposible duplicar si no se tiene permisos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                Dim frm As New frmAuxiliarSeleccioLlistatGeneric
                frm.Entrada(frmAuxiliarSeleccioLlistatGeneric.EnumTipusEntrada.SeleccionInstalacionAlDuplicarPropuestaAOtraInstalacion, oLinqInstalacion.ID_Cliente)
                AddHandler frm.AlTancarForm, AddressOf AlTancarFormAlDuplicarPropuestaAOtraInstalacion
                frm.FormObrir(Me, False)

            Case "GenerarPedidoVenta"
                If Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If

                Dim IDPropuesta As Integer = Me.GRD_Propuesta.GRID.ActiveRow.Cells("ID_Propuesta").Value
                Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = IDPropuesta).FirstOrDefault
                If _Propuesta.ID_Propuesta_Estado <> EnumPropuestaEstado.Aprobado And _Propuesta.ID_Propuesta_Estado <> EnumPropuestaEstado.Traspasado Then
                    Mensaje.Mostrar_Mensaje("Sólo se puede generar un pedido sobre los presupuestos aprobados o traspasados", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Exit Sub
                End If

                If _Propuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then
                    Mensaje.Mostrar_Mensaje("No se puede generar un pedido sobre una propuesta de tipo 'Instalación anterior'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If _Propuesta.Propuesta_Linea.Where(Function(F) F.Activo = True).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede generar un pedido sobre una propuesta vacía", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If oDTC.Entrada.Where(Function(F) F.ID_Propuesta = _Propuesta.ID_Propuesta).Count <> 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible generar otro pedido sobre esta propuesta.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Desea crear el pedido de venta correspondiente?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    'Util.WaitFormObrir()
                    'Dim _NewEntrada As New Entrada
                    'Dim oclsEntrada As New clsEntrada(oDTC, _NewEntrada)
                    'oclsEntrada.CrearPedidodeVentaAPartirDeUnaPropuesta(_Propuesta)
                    'Util.WaitFormTancar()
                    'Mensaje.Mostrar_Mensaje("Pedido de venta generado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                    'Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)

                    Dim frm As New frmInstalacionCreacionPartes
                    frm.Entrada(oLinqInstalacion, _Propuesta, oDTC)
                    frm.FormObrir(Me, False)

                    AddHandler frm.AlTancarForm, AddressOf AlTancarfrmPropuesta
                End If

            Case "GenerarParte"
                If Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If

                Dim IDPropuesta As Integer = Me.GRD_Propuesta.GRID.ActiveRow.Cells("ID_Propuesta").Value
                Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = IDPropuesta).FirstOrDefault
                If _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Pendiente Then
                    Mensaje.Mostrar_Mensaje("No se puede generar un parte si la propuesta está en estado pendiente", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Exit Sub
                End If

                If _Propuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then
                    Mensaje.Mostrar_Mensaje("No se puede generar un parte en una propuesta de tipo 'Instalación anterior'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If _Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Instalacion_Emplazamiento.HasValue = False).Count > 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede dar por aprobada una propuesta que tenga líneas sin un emplazamiento asignado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If _Propuesta.Parte.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible crear un parte, ésta propuesta ya tiene asignada un parte", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If _Propuesta.Propuesta_Linea.Where(Function(F) F.Activo = True).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede dar por aprobada una propuesta vacía", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                Dim frm As New frmInstalacionCreacionPartes
                frm.Entrada(oLinqInstalacion, _Propuesta, oDTC)
                frm.FormObrir(Me, False)

                AddHandler frm.AlTancarForm, AddressOf AlTancarfrmPropuesta

            Case "MoverPresupuesto"
                If Me.GRD_Propuesta.GRID.Selected.Cells.Count = 0 And Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Está seguro que desea mover la propuesta seleccionada?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Dim _IDPropuesta As Integer = Me.GRD_Propuesta.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value

                If PucDuplicarPropuesta(_IDPropuesta) = False Then
                    Mensaje.Mostrar_Mensaje("Imposible mover si no se tiene permisos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                Dim frm As New frmAuxiliarSeleccioLlistatGeneric
                frm.Entrada(frmAuxiliarSeleccioLlistatGeneric.EnumTipusEntrada.SeleccionInstalacionAlMoverPropuestaAOtraInstalacion, oLinqInstalacion.ID_Cliente)
                AddHandler frm.AlTancarForm, AddressOf AlTancarFormAlMoverPropuestaAOtraInstalacion
                frm.FormObrir(Me, False)

            Case "FusionarPresupuestos"
                If Me.GRD_Propuesta.GRID.Selected.Rows.Count < 2 Then
                    Mensaje.Mostrar_Mensaje("Imposible fusionar presupuestos si no seleccionan 2 o más propuestas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                Dim _LlistatPropostes As New List(Of Propuesta)

                Dim _pRow As UltraGridRow
                For Each _pRow In Me.GRD_Propuesta.GRID.Selected.Rows
                    Dim _Propuesta As Propuesta
                    _Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = CInt(_pRow.Cells("ID_Propuesta").Value)).FirstOrDefault
                    _LlistatPropostes.Add(_Propuesta)
                Next

                Dim frm As New frmInstalacionFusionarPropuestas
                frm.Entrada(oLinqInstalacion, _LlistatPropostes, oDTC)
                frm.FormObrir(Me, True)

                AddHandler frm.AlTancarForm, AddressOf AlTancarFormFusionarPressupostos

        End Select


    End Sub

    Private Sub AlTancarFormFusionarPressupostos()
        Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)
    End Sub

    Private Function PucDuplicarPropuesta(ByVal pIDPropuesta As Integer) As Boolean
        Dim _IDPropuesta As Integer = pIDPropuesta

        Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = _IDPropuesta).FirstOrDefault
        If _Propuesta.Propuesta_Seguridad.Count = 0 Then
            Return True
        End If

        If Seguretat.oUser.NivelSeguridad = 1 Then
            Return True
        End If

        If _Propuesta.Propuesta_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count > 0 Then
            Return True
        End If

        Return False
    End Function

    Private Sub AlTancarFormAlDuplicarPropuestaAOtraInstalacion(ByVal pIDInstalacion As Integer)
        Try

            ' Dim _Resposta As String = Mensaje.Mostrar_Entrada_Datos("Introduzca el número de instalación:", , False, False, False)

            'If _Resposta = "" Or IsNumeric(_Resposta) = False Then
            '    Exit Sub
            'End If

            'If oDTC.Instalacion.Where(Function(F) F.Activo = True And F.ID_Instalacion = CInt(_Resposta)).Count = 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible copiar la propuesta a la instalación. El código de instalación introducido no existe", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Exit Sub
            'End If

            Dim _Instalacion As Instalacion = oDTC.Instalacion.Where(Function(F) F.Activo = True And F.ID_Instalacion = CInt(pIDInstalacion)).FirstOrDefault

            Dim IDPropuesta As Integer = Me.GRD_Propuesta.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value
            'Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = IDPropuesta).FirstOrDefault

            Util.WaitFormObrir()
            Call DuplicarPressupost(IDPropuesta, _Instalacion)
            Util.WaitFormTancar()

            Mensaje.Mostrar_Mensaje("Propuesta duplicada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarFormAlMoverPropuestaAOtraInstalacion(ByVal pIDInstalacion As Integer)
        Try

            Dim _Instalacion As Instalacion = oDTC.Instalacion.Where(Function(F) F.Activo = True And F.ID_Instalacion = CInt(pIDInstalacion)).FirstOrDefault

            Dim _IDPropuesta As Integer = Me.GRD_Propuesta.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value

            Util.WaitFormObrir()

            Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = _IDPropuesta).FirstOrDefault

            _Propuesta.Instalacion = _Instalacion

            'Tot lo de baix es per blanquejar les zones i plantes i per intentar posar la ubicación predeterminada de la instalació destí
            _Propuesta.Instalacion_Emplazamiento = _Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
            _Propuesta.Instalacion_Emplazamiento_Planta = Nothing
            _Propuesta.Instalacion_Emplazamiento_Zona = Nothing

            Dim _Linea As Propuesta_Linea
            For Each _Linea In _Propuesta.Propuesta_Linea
                _Linea.Instalacion_Emplazamiento = _Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                _Linea.Instalacion_Emplazamiento_Planta = Nothing
                _Linea.Instalacion_Emplazamiento_Zona = Nothing
                _Linea.Instalacion_Emplazamiento_Abertura = Nothing
                _Linea.Instalacion_ElementosAProteger = Nothing
            Next

            Dim _Diagrama As Propuesta_Diagrama
            For Each _Diagrama In _Propuesta.Propuesta_Diagrama
                _Diagrama.Instalacion_Emplazamiento = _Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                _Diagrama.Instalacion_Emplazamiento_Planta = Nothing
                _Diagrama.Instalacion_Emplazamiento_Zona = Nothing
            Next

            Dim _Plano As Propuesta_Plano
            For Each _Plano In _Propuesta.Propuesta_Plano
                _Plano.Instalacion_Emplazamiento = _Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                _Plano.Instalacion_Emplazamiento_Planta = Nothing
                _Plano.Instalacion_Emplazamiento_Zona = Nothing
            Next

            oDTC.SubmitChanges()

            Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)
            Util.WaitFormTancar()

            Mensaje.Mostrar_Mensaje("Propuesta movida correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub CambiarEstadoPropuestaCRM(ByRef pPropuesta As Propuesta, ByVal pIDDestinoEstadoPropuesta As EnumPropuestaEstado)

    '    Dim _EstatCRM As Propuesta_EstadoCRM = oDTC.Propuesta_EstadoCRM.Where(Function(F) F.ID_Propuesta_Estado = pIDDestinoEstadoPropuesta).FirstOrDefault()
    '    If _EstatCRM Is Nothing = False Then 'Només si hem trobat un EstatCRM equivalment al ESTAT de la proposta cambiada llavors cambiarem l'estat CRM
    '        pPropuesta.Propuesta_EstadoCRM = _EstatCRM
    '    End If

    'End Sub

    Private Sub GRD_Propuesta_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Propuesta.M_ToolGrid_ToolEditar
        Call GRD_Propuesta_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub GRD_Propuesta_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Propuesta.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_Propuesta.GRID.Selected.Cells.Count = 0 And Me.GRD_Propuesta.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then

                Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                If Me.GRD_Propuesta.GRID.Selected.Cells.Count > 0 Then
                    pRow = Me.GRD_Propuesta.GRID.Selected.Cells(0).Row
                Else
                    pRow = Me.GRD_Propuesta.GRID.Selected.Rows(0)
                End If

                Dim _ID As Integer = pRow.Cells("ID_Propuesta").Value
                Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = _ID).FirstOrDefault

                If _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Aprobado Or _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Traspasado Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar una propuesta que esté en estado aprobada o traspasada a tal y como se instaló", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                BD.EjecutarConsulta("Update Propuesta Set Activo=0 Where ID_Propuesta=" & _ID)
                Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)

                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                '_DTC.SubmitChanges()
                'pRow.Hidden = True
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Propuesta_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Propuesta.M_ToolGrid_ToolVisualitzarDobleClickRow
        If oLinqInstalacion.ID_Instalacion = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        If Me.GRD_Propuesta.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        If Guardar(True) = False Then
            Exit Sub
        End If

        Dim frm As New frmPropuesta
        frm.Entrada(oLinqInstalacion, oDTC, Me.GRD_Propuesta.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value)
        AddHandler frm.FormClosing, AddressOf AlTancarfrmPropuesta
        frm.FormObrir(Me)
    End Sub

    Public Sub AlTancarfrmPropuesta()
        Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)

    End Sub

    Public Sub AlTancarfrmPropuestaTalYComoSeInstalo()
        Call CargaGrid_SeInstalo()
    End Sub

    Private Sub AlTancarFormInstalacionTraspaso()
        Call CargaGrid_Propuesta(oLinqInstalacion.ID_Instalacion)
        Call ActivarTabsEnFuncioDelEstatDeLaInstalacio()
    End Sub

#End Region

#Region "A tener en cuenta"

    Private Sub CargaGrid_ATenerEnCuenta(ByVal pId As Integer)
        Try

            With Me.GRD_ATenerEnCuenta

                Dim ATenerEnCuenta As IEnumerable(Of Instalacion_ATenerEnCuenta) = From taula In oDTC.Instalacion_ATenerEnCuenta Where taula.ID_Instalacion = pId Select taula

                '.GRID.DataSource = ATenerEnCuenta
                .M.clsUltraGrid.CargarIEnumerable(ATenerEnCuenta)

                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ATenerEnCuenta_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ATenerEnCuenta.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_ATenerEnCuenta

                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion1").Value = oLinqInstalacion

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ATenerEnCuenta_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_ATenerEnCuenta.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_ATenerEnCuenta_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_ATenerEnCuenta.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Recepción de materiales"

    Private Sub CargaGridRecepcionMateriales()
        Try
            Dim DT As New DataTable

            DT.Columns.Add("ID_Propuesta_Linea", "System.Integer".GetType)
            DT.Columns.Add("ID_Propuesta_Linea_Estado", "System.Integer".GetType)
            DT.Columns.Add("ID_Producto", "System.Integer".GetType)
            DT.Columns.Add("CodigoProducto", "System.String".GetType)
            DT.Columns.Add("DescripcionProducto", "System.String".GetType)
            DT.Columns.Add("Unidad", "System.Decimal".GetType)
            DT.Columns.Add("FechaPrevista", "System.Date".GetType)
            DT.Columns.Add("ID_Proveedor", "System.Integer".GetType)
            DT.Columns.Add("PrecioCoste", "System.Decimal".GetType)
            DT.Columns.Add("CantidadPendienteRecibir", "System.Integer".GetType)
            DT.Columns.Add("CodigoPropuesta", "System.Integer".GetType)

            Dim LineasTrobadas = oDTC.Propuesta_Linea.Where(Function(F) F.Propuesta.ID_Instalacion = oLinqInstalacion.ID_Instalacion And F.Activo = True And (F.Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Aprobado Or F.Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Traspasado) And (F.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Aprobado_PendienteRecibir OrElse F.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.RecepcionadoParcialmente OrElse F.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Recepcionadototalmente)).OrderBy(Function(F) F.ID_Propuesta_Linea_Estado)

            Dim _Linea As Propuesta_Linea
            For Each _Linea In LineasTrobadas
                With _Linea
                    Dim DTRow = DT.NewRow
                    DTRow.Item("ID_Propuesta_Linea") = _Linea.ID_Propuesta_Linea
                    DTRow.Item("ID_Propuesta_Linea_Estado") = _Linea.ID_Propuesta_Linea_Estado
                    DTRow.Item("ID_Producto") = _Linea.ID_Producto
                    DTRow.Item("CodigoProducto") = _Linea.Producto.Codigo
                    DTRow.Item("DescripcionProducto") = _Linea.Producto.Descripcion
                    DTRow.Item("Unidad") = Decimal.Round(_Linea.Unidad, 0)
                    DTRow.Item("FechaPrevista") = _Linea.FechaPrevista
                    DTRow.Item("ID_Proveedor") = _Linea.ID_Proveedor
                    DTRow.Item("PrecioCoste") = _Linea.PrecioCoste
                    DTRow.Item("CantidadPendienteRecibir") = _Linea.CantidadPendienteRecibir
                    DTRow.Item("CodigoPropuesta") = _Linea.Propuesta.Codigo
                    DT.Rows.Add(DTRow)
                End With
            Next

            GRD_RecepcionMateriales.M.clsUltraGrid.Cargar(DT)

            Me.GRD_RecepcionMateriales.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
            Me.GRD_RecepcionMateriales.GRID.DisplayLayout.Override.CellClickAction = CellClickAction.EditAndSelectText

            Me.GRD_RecepcionMateriales.GRID.DisplayLayout.Bands(0).Columns("CodigoProducto").CellActivation = Activation.NoEdit
            Me.GRD_RecepcionMateriales.GRID.DisplayLayout.Bands(0).Columns("DescripcionProducto").CellActivation = Activation.NoEdit
            '  Me.GRD_RecepcionMateriales.GRID.DisplayLayout.Bands(0).Columns("ID_Propuesta_Linea_Estado").CellActivation = Activation.NoEdit
            Me.GRD_RecepcionMateriales.GRID.DisplayLayout.Bands(0).Columns("Unidad").CellActivation = Activation.NoEdit
            Me.GRD_RecepcionMateriales.GRID.DisplayLayout.Bands(0).Columns("FechaPrevista").Style = ColumnStyle.Date



            Call CargarCombo_Proveedores(Me.GRD_RecepcionMateriales.GRID, 0)
            Call CargarCombo_PropuestaLineaEstado(Me.GRD_RecepcionMateriales.GRID)

            Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

            For Each pRow In Me.GRD_RecepcionMateriales.GRID.Rows
                If pRow.Cells("ID_Propuesta_Linea_Estado").Value <> EnumPropuestaLineaEstado.RecepcionadoParcialmente Then
                    pRow.Cells("CantidadPendienteRecibir").Activation = Activation.Disabled
                Else
                    pRow.Cells("CantidadPendienteRecibir").Activation = Activation.AllowEdit
                End If
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Proveedores(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, Optional ByVal pID As Integer = 0)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Proveedor)
            If pID = 0 Then
                oTaula = (From Taula In oDTC.Proveedor Where Taula.Activo = True Order By Taula.Nombre Select Taula)
            Else
                oTaula = (From Taula In oDTC.Proveedor Where Taula.Activo = True And Taula.Producto_Proveedor.Where(Function(F) F.ID_Proveedor = Taula.ID_Proveedor And F.ID_Producto = pID).Count > 0 Order By Taula.Nombre Select Taula)
            End If

            ' Valors.ValueListItems.Add(0, "Seleccione un registro")
            For Each Proveedor In oTaula
                Valors.ValueListItems.Add(Proveedor.ID_Proveedor, Proveedor.Nombre)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("ID_Proveedor").Style = ColumnStyle.DropDownList
            If pID = 0 Then
                pGrid.DisplayLayout.Bands(0).Columns("ID_Proveedor").ValueList = Valors.Clone
            Else
                pGrid.ActiveRow.Cells("ID_Proveedor").ValueList = Valors.Clone
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_PropuestaLineaEstado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Propuesta_Linea_Estado) = (From Taula In oDTC.Propuesta_Linea_Estado Where Taula.Activo = True Order By Taula.Codigo Select Taula)

            'Valors.ValueListItems.Add("", "Seleccione un registro")
            For Each Propuesta_Linea_Estado In oTaula
                If Propuesta_Linea_Estado.ID_Propuesta_Linea_Estado = 2 Or Propuesta_Linea_Estado.ID_Propuesta_Linea_Estado = 3 Or Propuesta_Linea_Estado.ID_Propuesta_Linea_Estado = 4 Then
                    Valors.ValueListItems.Add(Propuesta_Linea_Estado.ID_Propuesta_Linea_Estado, Propuesta_Linea_Estado.Descripcion)
                End If
            Next

            pGrid.DisplayLayout.Bands(0).Columns("ID_Propuesta_Linea_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("ID_Propuesta_Linea_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_RecepcionMateriales_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_RecepcionMateriales.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "ID_Proveedor"
                Call CargarCombo_Proveedores(Me.GRD_RecepcionMateriales.GRID, e.Cell.Row.Cells("ID_Producto").Value)
        End Select
    End Sub

    Private Sub GRD_RecepcionMateriales_M_GRID_BeforeRowUpdate(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles GRD_RecepcionMateriales.M_GRID_BeforeRowUpdate

        Dim IDLinea As Integer = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)

        Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = IDLinea).FirstOrDefault

        If IsDBNull(e.Row.Cells("FechaPrevista").Value) Then
            _Linea.FechaPrevista = Nothing
        Else
            _Linea.FechaPrevista = CDate(e.Row.Cells("FechaPrevista").Value)
        End If

        _Linea.ID_Proveedor = DbnullToNothingInteger(e.Row.Cells("ID_Proveedor").Value)
        _Linea.PrecioCoste = DbnullToNothingDecimal(e.Row.Cells("PrecioCoste").Value)
        _Linea.ID_Propuesta_Linea_Estado = DbnullToNothingInteger(e.Row.Cells("ID_Propuesta_Linea_Estado").Value)
        If e.Row.Cells("CantidadPendienteRecibir").Value.ToString.Trim = "" Then
            _Linea.CantidadPendienteRecibir = Nothing
        Else
            _Linea.CantidadPendienteRecibir = DbnullToNothingInteger(e.Row.Cells("CantidadPendienteRecibir").Value)
        End If


        oDTC.SubmitChanges()


    End Sub

    Private Sub GRD_RecepcionMateriales_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_RecepcionMateriales.M_GRID_CellListSelect
        Try

            If e.Cell.Column.Key = "ID_Proveedor" Then
                Dim ID_Proveedor As Integer = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
                If ID_Proveedor > 0 Then

                    Dim ID_Producto As Integer = e.Cell.Row.Cells("ID_Producto").Value
                    Dim Tarifa As Producto_Proveedor = oDTC.Producto_Proveedor.Where(Function(F) F.ID_Proveedor = ID_Proveedor And F.ID_Producto = ID_Producto).FirstOrDefault
                    If Tarifa Is Nothing = False Then
                        If Tarifa.PVD.HasValue = True Then
                            e.Cell.Row.Cells("PrecioCoste").Value = Decimal.Round(Tarifa.PVD.Value, 2)
                        Else
                            e.Cell.Row.Cells("PrecioCoste").Value = 0
                        End If
                    Else
                        e.Cell.Row.Cells("PrecioCoste").Value = 0
                    End If
                End If
            End If

            If e.Cell.Column.Key = "ID_Propuesta_Linea_Estado" Then
                Dim Estat As Integer = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
                Select Case Estat
                    Case EnumPropuestaLineaEstado.RecepcionadoParcialmente
                        e.Cell.Row.Cells("CantidadPendienteRecibir").Activation = Activation.AllowEdit
                    Case Else
                        e.Cell.Row.Cells("CantidadPendienteRecibir").Activation = Activation.Disabled
                        e.Cell.Row.Cells("CantidadPendienteRecibir").Value = Nothing
                End Select
            End If


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_RecepcionMateriales_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_RecepcionMateriales.M_ToolGrid_ToolClickBotonsExtras
        Try
            If Me.GRD_RecepcionMateriales.GRID.Rows.Count = 0 Then
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje("¿Está seguro de querer dar por recibidas todas las línieas?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                Exit Sub
            End If

            Dim LineasTrobadas = oDTC.Propuesta_Linea.Where(Function(F) F.Propuesta.ID_Instalacion = oLinqInstalacion.ID_Instalacion And F.Activo = True And (F.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Aprobado_PendienteRecibir OrElse F.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.RecepcionadoParcialmente OrElse F.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Recepcionadototalmente)).OrderBy(Function(F) F.ID_Propuesta_Linea_Estado)

            Dim _Linea As Propuesta_Linea
            For Each _Linea In LineasTrobadas
                With _Linea
                    _Linea.Propuesta_Linea_Estado = oDTC.Propuesta_Linea_Estado.Where(Function(F) F.ID_Propuesta_Linea_Estado = CInt(EnumPropuestaLineaEstado.Recepcionadototalmente)).FirstOrDefault
                    _Linea.CantidadPendienteRecibir = Nothing
                End With
            Next
            oDTC.SubmitChanges()

            Call CargaGridRecepcionMateriales()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Tal y como se instalo"

#Region "Cables"

    Private Sub CargaGrid_Cables()
        Try

            With Me.GRD_Cables

                Dim _Cables As IEnumerable(Of Instalacion_Cableado) = From taula In oDTC.Instalacion_Cableado Where taula.ID_Instalacion = oLinqInstalacion.ID_Instalacion Order By taula.Identificador Select taula

                .M.clsUltraGrid.CargarIEnumerable(_Cables)
                '.GRID.DataSource = _Cables

                .GRID.DisplayLayout.Bands(0).Columns("DistanciaMaxima").CellActivation = UltraWinGrid.Activation.NoEdit

                .M_Editable()

                Call CargarCombo_Categoria(.GRID)
                Call CargarCombo_TipoCable(.GRID)
                Call CargarCombo_InstaladoEn(.GRID, oLinqInstalacion.ID_Instalacion, "Instalacion_InstaladoEn_Inicio")
                Call CargarCombo_InstaladoEn(.GRID, oLinqInstalacion.ID_Instalacion, "Instalacion_InstaladoEn_Fin")

                '.GRID.RowUpdateCancelAction = RowUpdateCancelAction.CancelUpdate
                '.GRID.UpdateMode = UpdateMode.OnRowChangeOrLostFocus
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TipoCable(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Cableado) = (From Taula In oDTC.Cableado Where Taula.Activo = True And Taula.Cableado_Hilo.Count > 0 Order By Taula.Descripcion Select Taula)
            Dim Var As Cableado

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Cableado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Cableado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarCombo_Categoria(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Cableado_CategoriaCertificada) = (From Taula In oDTC.Instalacion_Cableado_CategoriaCertificada Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Cableado_CategoriaCertificada

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Cableado_CategoriaCertificada").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Cableado_CategoriaCertificada").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Cables_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Cables.M_GRID_BeforeCellActivate
        Try

            If e.Cell.Column.Key = "Cableado" Then

                Dim _Instalacion_Cableado As Instalacion_Cableado = Me.GRD_Cables.GRID.ActiveRow.ListObject

                If oDTC.Instalacion_CableadoMontaje.Where(Function(F) F.ID_Instalacion_Cableado.Equals(_Instalacion_Cableado)).Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible cambiar el tipo de cable mientras esté asignado", M_Mensaje.Missatge_Modo.INFORMACIO)
                    e.Cancel = True
                End If

                Call CargarCombo_TipoCable(Me.GRD_Cables.GRID)

            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Cables_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Cables.M_GRID_CellListSelect
        Try

            If e.Cell.Column.Key = "Instalacion_Cableado_CategoriaCertificada" Then
                Dim _CatCert As Instalacion_Cableado_CategoriaCertificada = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
                e.Cell.Row.Cells("DistanciaMaxima").Value = oDTC.Instalacion_Cableado_CategoriaCertificada.Where(Function(F) F.Equals(_CatCert)).FirstOrDefault.DistanciaMaxima
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Cables_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Cables.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Cables

                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Cableado_CategoriaCertificada").Value = oDTC.Instalacion_Cableado_CategoriaCertificada.FirstOrDefault.ID_Instalacion_Cableado_CategoriaCertificada
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion").Value = oLinqInstalacion

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Cables_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Cables.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            Dim Linea As Instalacion_Cableado = e.ListObject
            If Linea.Instalacion_CableadoMontaje.Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar el cable, este cable se ha usado en el apartado de 'Interconexiones'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_Cables_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Cables.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    'Private Sub GRD_Cables_M_Grid_BeforeRowCancelUpdate(sender As Object, e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles GRD_Cables.M_Grid_BeforeRowCancelUpdate
    '    Dim Linia As New Instalacion_Cableado
    '    Linia = Me.GRD_Cables.GRID.Rows.GetItem(Me.GRD_Cables.GRID.Rows.Count - 1).listObject
    '    If Linia.ID_Instalacion_Cableado = 0 Then
    '        oLinqInstalacion.Instalacion_Cableado.Remove(Linia)
    '    End If

    'End Sub

    Private Sub GRD_Cables_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Cables.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "Multiplicar" Then
            If Me.GRD_Cables.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            Dim frm As New frmAuxiliarDesdeHasta
            frm.Entrada(Me.GRD_Cables.GRID.Selected.Rows(0).Cells("Identificador").Value)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFrmDesdeHastaCables

            frm.FormObrir(Me, False)

        End If
    End Sub

    Private Sub AlTancarFrmDesdeHastaCables(ByVal pDescripcio As String, ByVal pDesde As Integer, ByVal pHasta As Integer)
        Dim _OldCable As Instalacion_Cableado = Me.GRD_Cables.GRID.Selected.Rows(0).ListObject

        Dim i As Integer
        For i = pDesde To pHasta
            Dim _NewCable As New Instalacion_Cableado
            With _NewCable
                .ID_Cableado = _OldCable.ID_Cableado
                '.Cableado = oDTC.Cableado.Where(Function(F) F.ID_Cableado = _OldCable.ID_Cableado).FirstOrDefault   '_OldCable.Cableado
                .Certificado = False
                .Detalle = ""
                '.Distancia
                .DistanciaMaxima = _OldCable.DistanciaMaxima
                .Identificador = pDescripcio & "-" & i
                '.Instalacion = _OldCable.Instalacion
                .ID_Instalacion_Cableado_CategoriaCertificada = _OldCable.ID_Instalacion_Cableado_CategoriaCertificada

                '.Instalacion_Cableado_CategoriaCertificada = oDTC.Instalacion_Cableado_CategoriaCertificada.Where(Function(F) F.ID_Instalacion_Cableado_CategoriaCertificada = _OldCable.ID_Instalacion_Cableado_CategoriaCertificada).FirstOrDefault   '_OldCable.Instalacion_Cableado_CategoriaCertificada
                ' .Instalacion_CableadoMontaje = _OldCable.Instalacion_CableadoMontaje
                '_Emplazamiento.Instalacion_Emplazamiento_Abertura.Add(_NewCable)
                .ID_Instalacion = _OldCable.ID_Instalacion
                '.Instalacion = oDTC.Instalacion.Where(Function(F) F.ID_Instalacion = _OldCable.ID_Instalacion).FirstOrDefault
                ' oLinqInstalacion.Instalacion_Cableado.Add(_NewCable)
                oDTC.Instalacion_Cableado.InsertOnSubmit(_NewCable)
                '_NewCable = Nothing
            End With
        Next

        oDTC.SubmitChanges()
        Call CargaGrid_Cables()
        Mensaje.Mostrar_Mensaje("Datos introducidos correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
    End Sub

    Private Sub CargarCombo_InstaladoEn(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pIDInstalacion As Integer, ByVal pColumna As String)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_InstaladoEn) = (From Taula In oDTC.Instalacion_InstaladoEn Where Taula.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_InstaladoEn

            'Valors.ValueListItems.Add("", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns(pColumna).Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns(pColumna).ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

#End Region

#Region "Cajas Intermedias"

    Private Sub CargaGrid_CajasIntermedias()
        Try
            Dim _CajasIntermedias As IEnumerable(Of Instalacion_CajaIntermedia) = From taula In oDTC.Instalacion_CajaIntermedia Where taula.ID_Instalacion = oLinqInstalacion.ID_Instalacion Select taula

            With Me.GRD_CajasIntermedias

                .M.clsUltraGrid.CargarIEnumerable(_CajasIntermedias)
                '.GRID.DataSource = _CajasIntermedias
                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CajasIntermedias_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_CajasIntermedias.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_CajasIntermedias
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion").Value = oLinqInstalacion

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CajasIntermedias_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_CajasIntermedias.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_CajasIntermedias_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_CajasIntermedias.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_CajasIntermedias_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_CajasIntermedias.M_ToolGrid_ToolClickBotonsExtras
        Try

            If e.Tool.Key = "Multiplicar" Then
                If Me.GRD_CajasIntermedias.GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If
                Dim frm As New frmAuxiliarDesdeHasta
                frm.Entrada(Me.GRD_CajasIntermedias.GRID.Selected.Rows(0).Cells("Identificador").Value)
                AddHandler frm.AlTancarForm, AddressOf AlTancarFrmDesdeHastaCajasIntermedias

                frm.FormObrir(Me, False)
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarFrmDesdeHastaCajasIntermedias(ByVal pDescripcio As String, ByVal pDesde As Integer, ByVal pHasta As Integer)
        Dim _OldCable As Instalacion_CajaIntermedia = Me.GRD_CajasIntermedias.GRID.Selected.Rows(0).ListObject
        Dim i As Integer
        For i = pDesde To pHasta
            Dim _NewCable As New Instalacion_CajaIntermedia
            With _NewCable
                .Detalle = ""
                .Identificador = pDescripcio & "-" & i
                '.Instalacion = _OldCable.Instalacion
                .ID_Instalacion = _OldCable.ID_Instalacion
                '_Emplazamiento.Instalacion_Emplazamiento_Abertura.Add(_NewCable)
                oDTC.Instalacion_CajaIntermedia.InsertOnSubmit(_NewCable)
            End With
        Next

        oDTC.SubmitChanges()
        Call CargaGrid_CajasIntermedias()
        Mensaje.Mostrar_Mensaje("Datos introducidos correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
    End Sub

#End Region

#Region "Fuentes de alimentación"

    Private Sub CargaGrid_FuenteAlimentacion()
        Try

            With Me.GRD_FuenteAlimentacion
                Dim _FuenteAlimentacion As IEnumerable(Of Instalacion_FuenteAlimentacion) = From taula In oDTC.Instalacion_FuenteAlimentacion Where taula.ID_Instalacion = oLinqInstalacion.ID_Instalacion Select taula
                '.GRID.DataSource = FuenteAlimentacion
                .M.clsUltraGrid.CargarIEnumerable(_FuenteAlimentacion)

                .M_Editable()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_FuenteAlimentacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_FuenteAlimentacion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_FuenteAlimentacion
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion").Value = oLinqInstalacion

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_FuenteAlimentacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_FuenteAlimentacion.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_CajasFuenteAlimentacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_FuenteAlimentacion.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "SeInstalo"

    Public Sub CargaGrid_SeInstalo()
        Me.GRD_SeInstalo.GRID.BeginUpdate()
        Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault


        If _Propuesta Is Nothing = False Then

            oDTFotosProductos = Nothing
            oDTFotosProductos = BD.RetornaDataTable("SELECT dbo.Instalacion.ID_Instalacion, dbo.Propuesta.ID_Propuesta, dbo.Producto.ID_Producto, dbo.Archivo.CampoBinario FROM  dbo.Producto INNER JOIN dbo.Archivo ON dbo.Producto.ID_Archivo_FotoPredeterminadaMini = dbo.Archivo.ID_Archivo INNER JOIN dbo.Instalacion INNER JOIN dbo.Propuesta ON dbo.Instalacion.ID_Instalacion = dbo.Propuesta.ID_Instalacion INNER JOIN dbo.Propuesta_Linea ON dbo.Propuesta.ID_Propuesta = dbo.Propuesta_Linea.ID_Propuesta ON dbo.Producto.ID_Producto = dbo.Propuesta_Linea.ID_Producto Where Propuesta.ID_Propuesta=" & _Propuesta.ID_Propuesta)

            _ArrayProducte = New ArrayList
            _ArrayFoto = New ArrayList


            With Me.GRD_SeInstalo
                If .ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Ocultar vista indentada" Then
                    Me.AccessibleName = Nothing
                    Dim DTS As New DataSet

                    BD.CargarDataSet(DTS, "Select *, Case NumPartes When 0 then '' else 'X' end as Partes From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Estado<>1  and ID_Propuesta_Linea_Vinculado is null and ID_Propuesta=" & _Propuesta.ID_Propuesta & " Order by Division, CodigoProducto")  '
                    Dim i As Integer = 0
                    Dim Fin As Integer

                    If _Propuesta.NivelMaximoLineas.HasValue = True Then
                        Fin = _Propuesta.NivelMaximoLineas - 1
                    Else
                        Fin = 5
                    End If


                    For i = 0 To Fin
                        BD.CargarDataSet(DTS, "Select *, Case NumPartes When 0 then '' else 'X' end as Partes From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Estado<>1  and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & _Propuesta.ID_Propuesta & " Order by Division, CodigoProducto", "Propuesta_Linea" & i + 2, i, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                    Next

                    .M.clsUltraGrid.Cargar(DTS, 1)

                    .GRID.DisplayLayout.Bands(0).ColumnFilters.ClearAllFilters()
                    .GRID.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows
                    .GRID.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons


                Else

                    Me.AccessibleName = "NoIndentatSeInstalo"
                    .M.clsUltraGrid.Cargar("Select *, Case NumPartes When 0 then '' else 'X' end as Partes From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & _Propuesta.ID_Propuesta & " Order by Division, CodigoProducto", BD, 2)
                    Me.AccessibleName = Nothing
                    .GRID.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow
                    .GRID.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
                    .GRID.DisplayLayout.Override.RowSelectorWidth = 16

                End If


            End With

            'Dim pRow As UltraGridRow
            'AddHandler Util.PerCadaRow, AddressOf PerCadaFila
            'Call Util.RecorrerGrid(Me.GRD_SeInstalo.GRID)

            _ArrayFoto.Clear()
            _ArrayProducte.Clear()
            _ArrayFoto = Nothing
            _ArrayProducte = Nothing

            If oDTFotosProductos Is Nothing = False Then
                oDTFotosProductos.Clear()
                oDTFotosProductos.Dispose()
                oDTFotosProductos = Nothing
            End If




        Else
            Me.GRD_SeInstalo.GRID.DataSource = Nothing
        End If
        Me.GRD_SeInstalo.GRID.EndUpdate()
    End Sub

    Private Sub GRD_SeInstalo_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_SeInstalo.M_Grid_InitializeRow

        Try
            With Me.GRD_SeInstalo
                'If e.Row.Index > 1000 Or e.Row.Band.Index <> 1 Then
                '    Exit Sub
                'End If

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



                ' If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
                'Dim _Linea As Propuesta_Linea
                '_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
                'If _Linea.Producto.Archivo Is Nothing = False AndAlso _Linea.Producto.Archivo.CampoBinario.Length > 0 Then
                ' e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(_Linea.Producto.Archivo.CampoBinario.ToArray)

                'Dim DTRow As DataRow() = oDTFotosProductos.Select("ID_Producto=" & e.Row.Cells("ID_Producto").Value)
                'If DTRow Is Nothing = False AndAlso DTRow.Count > 0 Then
                '    'e.Row.Cells("Foto").Appearance.Image =
                '    Dim pepe As System.Drawing.Image
                '    pepe = Util.BinaryToImage(DTRow(0).Item("CampoBinario"))


                '    e.Row.Cells("Foto").Appearance.Image = pepe



                '    'End If


                'End If
                'DTRow = Nothing

                ' .GRID.DisplayLayout.Override.DefaultRowHeight = 40
                ' Else
                '.GRID.DisplayLayout.Override.DefaultRowHeight = 20
                '  End If

                If e.Row.Cells("CodigoDocumentoRelacionado").Value.ToString.Length > 0 Then
                    e.Row.Cells("CodigoDocumentoRelacionado").Appearance.BackColor = Color.LightGreen
                End If
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_SeInstalo_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_SeInstalo.M_ToolGrid_ToolClickBotonsExtras
        If oLinqInstalacion.ID_Instalacion = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        Select Case e.Tool.Key
            Case "ModificarInstalacion"
                Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
                If _Propuesta Is Nothing = False Then

                    If Guardar(True) = False Then
                        Exit Sub
                    End If
                    Util.WaitFormObrir()

                    Dim frm As New frmPropuesta
                    frm.Entrada(oLinqInstalacion, oDTC, _Propuesta.ID_Propuesta)
                    AddHandler frm.FormClosing, AddressOf AlTancarfrmPropuestaTalYComoSeInstalo
                    frm.FormObrir(Me)
                    Util.WaitFormTancar()
                End If
                ' Call GRD_SeInstalo_M_ToolGrid_ToolVisualitzarDobleClickRow()

            Case "VistaIndentada"
                If Me.GRD_SeInstalo.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Vista indentada" Then
                    Me.GRD_SeInstalo.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Ocultar vista indentada"
                Else
                    Me.GRD_SeInstalo.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Vista indentada"
                End If

                Call CargaGrid_SeInstalo()

            Case "VisualizarPartes"
                If Me.GRD_SeInstalo.GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If

                Dim ID_Propuesta_Linea As Integer = Me.GRD_SeInstalo.GRID.Selected.Rows(0).Cells("ID_Propuesta_Linea").Value
                Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = ID_Propuesta_Linea).FirstOrDefault

                Dim frmInstalacionPartes As New frmInstalacionPartes
                frmInstalacionPartes.Entrada(_Linea, oDTC)
                frmInstalacionPartes.FormObrir(Me, True)

            Case "GenerarParte"
                Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault

                Dim _NewParte As New Parte
                With _NewParte
                    .Activo = True
                    .ID_Parte_Vinculado = Nothing
                    .Cliente = oLinqInstalacion.Cliente
                    .Instalacion = oLinqInstalacion
                    .Propuesta = _Propuesta
                    .Direccion = oLinqInstalacion.Direccion
                    .PersonaContacto = oLinqInstalacion.PersonaContacto
                    .Poblacion = oLinqInstalacion.Poblacion
                    .Provincia = oLinqInstalacion.Provincia
                    .Longitud = oLinqInstalacion.Longitud
                    .Latitud = oLinqInstalacion.Latitud
                    .CP = oLinqInstalacion.CP
                    .Pais = oLinqInstalacion.Pais
                    .Delegacion = oLinqInstalacion.Delegacion

                    .Telefono = oLinqInstalacion.Telefono
                    .TrabajoARealizar = "" 'Trabajo a realizar según presupuesto nº: " & _Propuesta.Codigo 'Me.GRD_Incidencia.GRID.Selected.Rows(0).Cells("Descripcion").Value

                    .FechaAlta = Date.Now
                    .ID_Parte_TipoFacturacion = EnumParteTipoFacturacion.Facturable
                    .ID_Parte_Estado = EnumParteEstado.Pendiente
                    .ID_Parte_Tipo = EnumParteTipo.Reparacion
                    .ParteFirmado = False

                    Dim _aux As New Parte_Aux
                    _aux.ObservacionesTecnico = ""
                    _NewParte.Parte_Aux = _aux

                    oDTC.Parte.InsertOnSubmit(_NewParte)
                    oDTC.SubmitChanges()

                    Call CrearPreguntesQuestionari(_NewParte)
                    Call clsParte.CrearMagatzem(oDTC, _NewParte) 'ho posem davant del submitchanges per a tenir el ID del magatzem
                    Dim _ClsNotificacion As New clsNotificacion(oDTC)
                    _ClsNotificacion.CrearNotificacion_AlCrearParte(_NewParte)
                    oDTC.SubmitChanges()


                End With

                Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                frm.Entrada(_NewParte.ID_Parte)
                frm.FormObrir(Me, True)

                Mensaje.Mostrar_Mensaje("Parte creado automáticamente", M_Mensaje.Missatge_Modo.INFORMACIO)

            Case "VisualizarFotos"
                If Me.GRD_SeInstalo.ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
                    Me.GRD_SeInstalo.ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "Visualizar fotos"
                Else
                    Me.GRD_SeInstalo.ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos"
                End If
                Call CargaGrid_SeInstalo()

            Case "VerProducto"
                If Me.GRD_SeInstalo.GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If
                Dim _IDProducto As Integer = Me.GRD_SeInstalo.GRID.Selected.Rows(0).Cells("ID_Producto").Value
                Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
                frm.Entrada(_IDProducto)
                frm.FormObrir(Me, True)

            Case "IrAlbaran"
                If Me.GRD_SeInstalo.GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If
                If Me.GRD_SeInstalo.GRID.Selected.Rows(0).Cells("CodigoDocumentoRelacionado").Value.ToString.Length = 0 Then
                    Exit Sub
                End If
                Dim _IDEntrada As Integer = Me.GRD_SeInstalo.GRID.Selected.Rows(0).Cells("ID_Entrada").Value
                Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
                frm.Entrada(_IDEntrada, EnumEntradaTipo.AlbaranVenta)
                frm.FormObrir(Me, True)

        End Select

    End Sub

    Private Sub GRD_SeInstalo_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_SeInstalo.M_ToolGrid_ToolVisualitzarDobleClickRow
        'Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        'If _Propuesta Is Nothing = False Then

        '    If Guardar(True) = False Then
        '        Exit Sub
        '    End If
        '    Util.WaitFormObrir()

        '    Dim frm As New frmPropuesta
        '    frm.Entrada(oLinqInstalacion, oDTC, _Propuesta.ID_Propuesta)
        '    AddHandler frm.FormClosing, AddressOf AlTancarfrmPropuesta
        '    'oDisseny.Abrir_Formulario(Me, frm, False)
        '    frm.FormObrir(Me)
        '    Util.WaitFormTancar()
        'End If
    End Sub

#End Region

#Region "Productos eliminados"

    Public Sub CargaGrid_ProductosEliminados()
        Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        If _Propuesta Is Nothing = False Then
            Me.GRD_ProductosEliminados.M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where Activo=0 and ID_Propuesta=" & _Propuesta.ID_Propuesta, BD)
        Else
            Me.GRD_ProductosEliminados.M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where Activo=0 and ID_Propuesta=0", BD)
        End If
    End Sub

    Private Sub GRD_ProductosEliminados_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_ProductosEliminados.M_Grid_InitializeRow
        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_ProductosEliminados.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
    End Sub

    Private Sub GRD_ProductosEliminados_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ProductosEliminados.M_ToolGrid_ToolClickBotonsExtras
        Try


            If oLinqInstalacion.ID_Instalacion = 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If

            If e.Tool.Key = "RecuperarProducto" Then
                With Me.GRD_ProductosEliminados
                    If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
                        Exit Sub
                    End If

                    If Mensaje.Mostrar_Mensaje("¿Desea recuperar esta línea de propuesta eliminada?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then

                        Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                        If .GRID.Selected.Cells.Count > 0 Then
                            pRow = .GRID.Selected.Cells(0).Row
                        Else
                            pRow = .GRID.Selected.Rows(0)
                        End If

                        Dim ID As Integer = pRow.Cells("ID_Propuesta_Linea").Value
                        Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = ID).FirstOrDefault()
                        _Linea.Activo = True
                        _Linea.MotivoEliminacion = ""
                        oDTC.SubmitChanges()
                        Mensaje.Mostrar_Mensaje("Recuperación realizada correctamente, recuerde de asignarle el cableado correspondiente", M_Mensaje.Missatge_Modo.INFORMACIO)
                        Call CargaGrid_ProductosEliminados()
                    End If




                End With

            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Productos pendientes de aprobar"

    Public Sub CargaGrid_ProductosPendientesAprobar()
        Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        If _Propuesta Is Nothing = False Then
            Me.GRD_Productos_PendientesAprobacion.M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where Activo=1 and ID_Propuesta_Linea_Estado=" & EnumPropuestaLineaEstado.PendienteDeAprobar & " and ID_Propuesta=" & _Propuesta.ID_Propuesta, BD)
        Else
            Me.GRD_Productos_PendientesAprobacion.M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where Activo=1 and ID_Propuesta=0", BD)
        End If
    End Sub

    Private Sub GRD_ProductosPendientesAprobar_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Productos_PendientesAprobacion.M_Grid_InitializeRow
        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_Productos_PendientesAprobacion.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
    End Sub

    Private Sub GRD_ProductosPendientesAprobar_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Productos_PendientesAprobacion.M_ToolGrid_ToolClickBotonsExtras
        Try
            If oLinqInstalacion.ID_Instalacion = 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If

            If e.Tool.Key = "AprobarProducto" Then
                With Me.GRD_Productos_PendientesAprobacion
                    If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
                        Exit Sub
                    End If

                    If Mensaje.Mostrar_Mensaje("¿Desea aprobar esta línea de propuesta?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then

                        Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                        If .GRID.Selected.Cells.Count > 0 Then
                            pRow = .GRID.Selected.Cells(0).Row
                        Else
                            pRow = .GRID.Selected.Rows(0)
                        End If

                        Dim ID As Integer = pRow.Cells("ID_Propuesta_Linea").Value
                        Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = ID).FirstOrDefault()
                        _Linea.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Aprobado_PendienteRecibir
                        oDTC.SubmitChanges()

                        Call CargaGrid_ProductosPendientesAprobar()
                    End If
                End With
            End If

            If e.Tool.Key = "AprobarTodosProducto" Then
                With Me.GRD_Productos_PendientesAprobacion
                    'If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
                    '    Exit Sub
                    'End If

                    If Mensaje.Mostrar_Mensaje("¿Desea aprobar todas líneas de propuesta?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
                        Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                        For Each pRow In .GRID.Rows
                            Dim ID As Integer = pRow.Cells("ID_Propuesta_Linea").Value
                            Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = ID).FirstOrDefault()
                            _Linea.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Aprobado_PendienteRecibir

                        Next
                        oDTC.SubmitChanges()
                        'If .GRID.Selected.Cells.Count > 0 Then
                        '    pRow = .GRID.Selected.Cells(0).Row
                        'Else
                        '    pRow = .GRID.Selected.Rows(0)
                        'End If

                        Call CargaGrid_ProductosPendientesAprobar()
                    End If
                End With
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Cableado"

    Public Sub CargaGrid_Cableado(Optional ByVal pIDPropuestaLinea As Integer = 0, Optional ByVal pIDCajaIntermedia As Integer = 0)
        Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        If _Propuesta Is Nothing = False Then
            Dim DTS As New DataSet
            Dim _SqlFilterXArticle As String = ""
            If pIDPropuestaLinea <> 0 Then
                _SqlFilterXArticle = " and ID_Instalacion_Cableado in (Select ID_Instalacion_Cableado From C_Cableado_Hilos Where ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " and (ID_Propuesta_Linea_Origen=" & pIDPropuestaLinea & " or ID_Propuesta_Linea_Destino=" & pIDPropuestaLinea & ")) "
            End If
            If pIDCajaIntermedia <> 0 Then
                _SqlFilterXArticle = " and ID_Instalacion_Cableado in (Select ID_Instalacion_Cableado From C_Cableado_Hilos Where ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " and (ID_Instalacion_CajaIntermedia_Origen=" & pIDCajaIntermedia & " or ID_Instalacion_CajaIntermedia_Destino=" & pIDCajaIntermedia & ")) "
            End If

            BD.CargarDataSet(DTS, "Select * From Instalacion_Cableado, Cableado Where Instalacion_Cableado.ID_Cableado=Cableado.ID_Cableado and ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & _SqlFilterXArticle & " Order by Identificador")
            BD.CargarDataSet(DTS, "Select * From C_Cableado_Hilos Where ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " Order by Identificador", "Nivell1", 0, "ID_Instalacion_Cableado", "ID_Instalacion_Cableado")
            BD.CargarDataSet(DTS, "Select * From C_Cableado_Hilos2 Order by ID_Cableado", "Nivell2", 1, "ID_Instalacion_CableadoMontaje", "ID_Instalacion_CableadoMontaje")
            Me.GRD_Cableado.M.clsUltraGrid.Cargar(DTS)
        End If
    End Sub

    Private Sub GRD_Cableado_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Cableado.M_GRID_DoubleClickRow
        If Me.GRD_Cableado.GRID.ActiveRow.Band.Index = 2 Then
            Dim _Hilo As Instalacion_CableadoMontaje_Hilo = oDTC.Instalacion_CableadoMontaje_Hilo.Where(Function(F) F.ID_Instalacion_CableadoMontaje_Hilo = CInt(Me.GRD_Cableado.GRID.ActiveRow.Cells("ID_Instalacion_CableadoMontaje_Hilo").Value)).FirstOrDefault
            Dim Resultado = Mensaje.Mostrar_Entrada_Datos("Introduzca el uso del hilo", IIf(IsDBNull(_Hilo.Uso), "", _Hilo.Uso), False)
            If Resultado <> "" Then
                _Hilo.Uso = Resultado
            End If
            oDTC.SubmitChanges()
            Me.GRD_Cableado.GRID.ActiveRow.Cells("Uso").Value = Resultado
        End If

    End Sub

    Private Sub GRD_Cableado_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Cableado.M_ToolGrid_ToolClickBotonsExtras
        Try

            If oLinqInstalacion.ID_Instalacion = 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If

            If Me.GRD_Cableado.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
            If _Propuesta Is Nothing = False Then
                Dim _Cable As Instalacion_Cableado = oLinqInstalacion.Instalacion_Cableado.Where(Function(F) F.ID_Instalacion_Cableado = CInt(Me.GRD_Cableado.GRID.Selected.Rows(0).Cells("ID_Instalacion_Cableado").Value)).FirstOrDefault

                If Guardar(True) = False Then
                    Exit Sub
                End If

                Dim frm As New frmInstalacionCablear
                frm.Entrada(oLinqInstalacion, _Cable, oDTC)
                AddHandler frm.FormClosing, AddressOf AlTancarFrmCableado
                frm.FormObrir(Me)
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub GRD_Cableado_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Cableado.M_ToolGrid_ToolEditar
    '    Try
    '        If oLinqInstalacion.ID_Instalacion = 0 Then
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '            Exit Sub
    '        End If

    '        If Me.GRD_Cableado.GRID.Selected.Rows.Count <> 1 AndAlso Me.GRD_Cableado.GRID.Selected.Rows(0).Band.Index = 1 Then
    '            Exit Sub
    '        End If

    '        Dim IDCable As Integer = Me.GRD_Cableado.GRID.Selected.Rows(0).Cells("ID_Instalacion_CableadoMontaje").Value
    '        Dim _CableMontado As Instalacion_CableadoMontaje = oDTC.Instalacion_CableadoMontaje.Where(Function(F) F.ID_Instalacion_CableadoMontaje = IDCable).FirstOrDefault


    '        If Guardar(True) = False Then
    '            Exit Sub
    '        End If

    '        Dim frm As New frmInstalacionCablear
    '        frm.Entrada(oLinqInstalacion, _CableMontado.Instalacion_Cableado, oDTC, _CableMontado.ID_Instalacion_CableadoMontaje)
    '        AddHandler frm.FormClosing, AddressOf AlTancarFrmCableado
    '        frm.FormObrir(Me)


    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub GRD_Cableado_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Cableado.M_ToolGrid_ToolEliminar
        Try
            If oLinqInstalacion.ID_Instalacion = 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If

            If Me.GRD_Cableado.GRID.Selected.Rows.Count <> 1 OrElse Me.GRD_Cableado.GRID.Selected.Rows(0).Band.Index <> 1 Then
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
                Dim IDCable As Integer = Me.GRD_Cableado.GRID.Selected.Rows(0).Cells("ID_Instalacion_CableadoMontaje").Value
                Dim _CableMontado As Instalacion_CableadoMontaje = oDTC.Instalacion_CableadoMontaje.Where(Function(F) F.ID_Instalacion_CableadoMontaje = IDCable).FirstOrDefault

                oDTC.Instalacion_CableadoMontaje_Hilo.DeleteAllOnSubmit(oDTC.Instalacion_CableadoMontaje_Hilo.Where(Function(F) F.ID_Instalacion_CableadoMontaje = IDCable))
                oDTC.Instalacion_CableadoMontaje.DeleteOnSubmit(_CableMontado)
                oDTC.SubmitChanges()

                Me.GRD_Cableado.GRID.Selected.Rows(0).Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarFrmCableado()
        Try
            Call CargaGrid_Cableado()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub TE_BusquedaInterconexiones_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_BusquedaInterconexiones.EditorButtonClick
        Try
            If e.Button.Key = "Lupeta" Then
                Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
                Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
                If _Propuesta Is Nothing = False Then
                    LlistatGeneric.Mostrar_Llistat("Select *, Case NumPartes When 0 then '' else 'X' end as Partes From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & _Propuesta.ID_Propuesta, Me.TE_BusquedaInterconexiones, "ID_Propuesta_Linea", "Descripcion")
                    AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGenericBusquedaInterConexiones
                End If
            End If

            If e.Button.Key = "Cancelar" Then
                Call CargaGrid_Cableado()
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarLlistatGenericBusquedaInterConexiones(ByVal pID As Integer)
        If pID <> 0 Then
            Call CargaGrid_Cableado(pID)
        End If
    End Sub

    Private Sub TE_BusquedaCajaIntermedia_EditorButtonClick(sender As System.Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_BusquedaCajaIntermedia.EditorButtonClick
        Try
            If e.Button.Key = "Lupeta" Then
                Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
                LlistatGeneric.Mostrar_Llistat("Select *, cast(Identificador as nvarchar(50)) + ' - ' + Detalle As Descripcion From Instalacion_CajaIntermedia Where ID_Instalacion=" & oLinqInstalacion.ID_Instalacion, Me.TE_BusquedaCajaIntermedia, "ID_Instalacion_CajaIntermedia", "Descripcion")
                AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGenericBusquedaCajaIntermedia
            End If

            If e.Button.Key = "Cancelar" Then
                Call CargaGrid_Cableado()
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarLlistatGenericBusquedaCajaIntermedia(ByVal pID As Integer)
        If pID <> 0 Then
            Call CargaGrid_Cableado(0, pID)
        End If
    End Sub
#End Region

#Region "Planos"

    Private Sub CargaGrid_Planos()
        Try

            Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
            If _Propuesta Is Nothing Then
                Exit Sub
            End If

            Dim _Planos As IEnumerable(Of Propuesta_Plano) = From taula In oDTC.Propuesta_Plano Where taula.ID_Propuesta = _Propuesta.ID_Propuesta Select taula

            With Me.GRD_Planos
                .M.clsUltraGrid.CargarIEnumerable(_Planos)
                '.GRID.DataSource = _Planos

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("FechaCreacion").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("ID_Propuesta_Antigua").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Propuesta_Version_Antigua").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Planta(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Zona(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_ProductoDivision(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Planos_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Planos.M_GRID_BeforeCellActivate
        Call GRD_InstaladoEn_M_GRID_BeforeCellActivate(sender, e)
    End Sub

    Private Sub GRD_Planos_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Planos.M_GRID_CellListSelect
        Call GRD_InstaladoEn_M_GRID_CellListSelect(sender, e)
    End Sub

    Private Sub GRD_Planos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Planos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Planos
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
                If _Propuesta Is Nothing Then
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaCreacion").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta").Value = _Propuesta

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
                'Dim pepe As New frmPlano(Me.MdiParent)
                ofrmPlano = New frmPlano(Me.MdiParent)
                ofrmPlano.Entrada(oDTC, Me.GRD_Planos.GRID.ActiveRow.ListObject)
                ofrmPlano.FormObrir(Me, False)
                AddHandler ofrmPlano.AlTancarForm, AddressOf AlTancarFormPlanos
                ' pepe.MdiParent = Principal
                ' pepe.Show()

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

                _NewPlano.Propuesta = _Plano.Propuesta
                oDTC.SubmitChanges()
                Call CargaGrid_Planos()

        End Select
    End Sub

    Private Sub AlTancarFormPlanos(ByRef pID As String)
        ofrmPlano.Dispose()
        ofrmPlano = Nothing
        Call CargaGrid_Planos()
    End Sub

    Private Sub GRD_Planos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Planos.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_Planta(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer, Optional ByVal pIDEmplazamiento As Integer = 0)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta)

            If pIDEmplazamiento = 0 Then
                oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
            Else
                oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.ID_Instalacion_Emplazamiento = pIDEmplazamiento Order By Taula.Descripcion Select Taula)
            End If

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

    Private Sub CargarCombo_Emplazamiento(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento) = (From Taula In oDTC.Instalacion_Emplazamiento Where Taula.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
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

    Private Sub CargarCombo_ProductoDivision(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Division) = (From Taula In oDTC.Producto_Division Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Division

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_Division").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_Division").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

#End Region

#Region "InstaladoEn"

    Private Sub CargaGrid_InstaladoEn()
        Try

            With Me.GRD_InstaladoEn

                Dim _InstaladoEn As IEnumerable(Of Instalacion_InstaladoEn) = From taula In oDTC.Instalacion_InstaladoEn Where taula.ID_Instalacion = oLinqInstalacion.ID_Instalacion Select taula

                .M.clsUltraGrid.CargarIEnumerable(_InstaladoEn)
                '.GRID.DataSource = _InstaladoEn

                .M_Editable()

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Planta(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Zona(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_FuenteAlimentacion(.GRID, oLinqInstalacion.ID_Instalacion)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_InstaladoEn_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_InstaladoEn.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "Instalacion_Emplazamiento_Zona"
                Call CargarCombo_Zona(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value)
            Case "Instalacion_Emplazamiento_Planta"
                Call CargarCombo_Planta(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento").Value)
        End Select
    End Sub

    Private Sub GRD_InstaladoEn_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_InstaladoEn.M_GRID_CellListSelect
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

    Private Sub CargarCombo_FuenteAlimentacion(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pID As Integer)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_FuenteAlimentacion) = (From Taula In oDTC.Instalacion_FuenteAlimentacion Where Taula.ID_Instalacion = pID Order By Taula.Detalle Select Taula)
            Dim Var As Instalacion_FuenteAlimentacion

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Detalle)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_FuenteAlimentacion").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_FuenteAlimentacion").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_InstaladoEn_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_InstaladoEn.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_InstaladoEn

                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion").Value = oLinqInstalacion

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_InstaladoEn_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_InstaladoEn.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim _InstaladoEn As Instalacion_InstaladoEn = e.ListObject
                If _InstaladoEn.Instalacion_Cableado_Fin.Count > 0 Or _InstaladoEn.Instalacion_Cableado_Inicio.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar, la línea se está usando en las interconnexiones", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Exit Sub
                End If

                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_InstaladoEn_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_InstaladoEn.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    'Private Sub CargarCombo_Planta3(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pIDInstalacion As Integer)
    '    Try
    '        Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta)
    '        oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim Var As Instalacion_Emplazamiento_Planta

    '        'Valors.ValueListItems.Add("-1", "Seleccione un registro")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Planta, Var.Descripcion)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

    '        pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarCombo_Planta3(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDEmplazamiento As Integer)
    '    Try
    '        Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta)
    '        oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.ID_Instalacion_Emplazamiento = pIDEmplazamiento Order By Taula.Descripcion Select Taula)

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim Var As Instalacion_Emplazamiento_Planta

    '        Valors.ValueListItems.Add(Nothing, "")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Planta, Var.Descripcion)
    '        Next

    '        'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

    '        pCelda.ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarCombo_Zona3(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pIDInstalacion As Integer)
    '    Try
    '        Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Zona)
    '        oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Zona Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim Var As Instalacion_Emplazamiento_Zona

    '        'Valors.ValueListItems.Add("-1", "Seleccione un registro")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Zona, Var.Descripcion)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Zona").Style = ColumnStyle.DropDownList

    '        pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Zona").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarCombo_Zona3(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDPlanta As Integer)
    '    Try
    '        Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Zona)
    '        oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Zona Where Taula.ID_Instalacion_Emplazamiento_Planta = pIDPlanta Order By Taula.Descripcion Select Taula)

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim Var As Instalacion_Emplazamiento_Zona

    '        Valors.ValueListItems.Add(Nothing, "")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var.ID_Instalacion_Emplazamiento_Zona, Var.Descripcion)
    '        Next

    '        'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

    '        pCelda.ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarCombo_Emplazamiento3(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pID As Integer)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Instalacion_Emplazamiento) = (From Taula In oDTC.Instalacion_Emplazamiento Where Taula.ID_Instalacion = pID Order By Taula.Descripcion Select Taula)

    '        Valors.ValueListItems.Add(Nothing, "")
    '        For Each Instalacion_Emplazamiento In oTaula
    '            Valors.ValueListItems.Add(Instalacion_Emplazamiento.ID_Instalacion_Emplazamiento, Instalacion_Emplazamiento.Descripcion)
    '        Next


    '        pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

#End Region


#End Region

#Region "Accesos"

    Private Sub CargarGrid_Accesos()
        Try
            Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
            If _Propuesta Is Nothing Then
                Me.GRD_Accesos.GRID.DataSource = Nothing
                Exit Sub
            End If

            Dim DTS As New DataSet
            BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & _Propuesta.ID_Propuesta & " and  (Select Count(*) From Propuesta_Linea_Acceso Where Propuesta_Linea_Acceso.ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea)>0")
            BD.CargarDataSet(DTS, "Select * From C_Instalacion_Accesos Where ID_Propuesta=" & _Propuesta.ID_Propuesta & " Order by TipoAcceso", "Nivell1", 0, "ID_Propuesta_Linea", "ID_Propuesta_Linea")

            Me.GRD_Accesos.M.clsUltraGrid.Cargar(DTS)


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Accesos_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Accesos.M_GRID_DoubleClickRow
        Try
            If Me.GRD_Accesos.GRID.Selected.Rows.Count = 1 AndAlso Me.GRD_Accesos.GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim _PropuestaLinea As Propuesta_Linea
                _PropuestaLinea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(Me.GRD_Accesos.GRID.Selected.Rows(0).Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
                Dim frm As New frmPropuesta_Linea
                frm.Entrada(_PropuestaLinea.Propuesta.Instalacion, _PropuestaLinea.Propuesta, oDTC, _PropuestaLinea.ID_Propuesta_Linea, True)
                AddHandler frm.AlTancarForm, AddressOf AlTancarFormPropuestaLinea
                frm.FormObrir(Me, True)
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Accesos_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Accesos.M_Grid_InitializeRow
        If e.Row.Band.Index = 0 AndAlso IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_Accesos.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
    End Sub

    Private Sub AlTancarFormPropuestaLinea()
        Call CargarGrid_Accesos()
    End Sub

#End Region

#Region "Partes"
    Private Sub CargarGrid_Partes()
        Try

            Me.GRD_Partes.M.clsUltraGrid.Cargar("Select * From C_Parte Where Activo=1 and ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " Order by FechaAlta", BD)

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

    'Private Sub GRD_Accesos_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Accesos.M_Grid_InitializeRow
    '    If e.Row.Band.Index = 0 AndAlso IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
    '        Me.GRD_Accesos.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
    '    End If
    'End Sub
#End Region

#Region "Revisión Productos"

    Private Sub CargaGrid_RevisionProductos()
        Try

            With Me.GRD_RevisionProducto

                BD.EjecutarConsulta("Insert Into Instalacion_Producto_Division (ID_Instalacion, ID_Producto_Division, MesesRevision, FechaInicio)  Select " & oLinqInstalacion.ID_Instalacion & ", ID_Producto_Division, MesesRevision, null From Producto_Division as TBL1 Where Activo=1 and (Select COUNT(*) From Instalacion_Producto_Division as TBL2 Where ID_Instalacion= " & oLinqInstalacion.ID_Instalacion & " and TBL1.ID_Producto_Division=TBL2.ID_Producto_Division)=0")

                Dim _Revision As IEnumerable(Of Instalacion_Producto_Division) = From taula In oDTC.Instalacion_Producto_Division Where taula.ID_Instalacion = oLinqInstalacion.ID_Instalacion Order By taula.Producto_Division.Codigo Select taula

                .M.clsUltraGrid.CargarIEnumerable(_Revision)
                '.GRID.DataSource = _Revision

                .M_Editable()

                .GRID.DisplayLayout.Bands(0).Columns("Producto_Division").CellActivation = UltraWinGrid.Activation.NoEdit

                Call CargarCombo_Division(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Division(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Division) = (From Taula In oDTC.Producto_Division Where Taula.Activo = True Order By Taula.Codigo Select Taula)
            Dim Var As Producto_Division

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_Division").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_Division").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_RevisionProducto_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_RevisionProducto.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Receptora"
    Private Sub CargaGrid_Receptora(ByVal pId As Integer)
        Try
            With Me.GRD_Receptora
                Dim DTS As New DataSet
                BD.CargarDataSet(DTS, "Select * From C_Instalacion_Receptora Where ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " Order By ReceptoraNombre")
                BD.CargarDataSet(DTS, "Select IRC.* From Instalacion_Receptora_Contacto as IRC, Instalacion_Receptora as IR Where IR.ID_Instalacion_Receptora=IRC.ID_Instalacion_Receptora and ID_Instalacion  =" & oLinqInstalacion.ID_Instalacion & " Order by Orden", "Contactes", 0, "ID_Instalacion_Receptora", "ID_Instalacion_Receptora", False)
                BD.CargarDataSet(DTS, "Select IRC.* From Instalacion_Receptora_OrdenLlamada as IRC, Instalacion_Receptora as IR Where IR.ID_Instalacion_Receptora=IRC.ID_Instalacion_Receptora and ID_Instalacion  =" & oLinqInstalacion.ID_Instalacion & " Order by Orden", "Contactes2", 0, "ID_Instalacion_Receptora", "ID_Instalacion_Receptora", False)
                .M.clsUltraGrid.Cargar(DTS)
            End With

            Me.GRD_Propuesta.GRID.ActiveRow = Nothing

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Receptora_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Receptora.M_ToolGrid_ToolAfegir
        Try
            If oLinqInstalacion.ID_Instalacion = 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If

            If Guardar(True) = False Then
                Exit Sub
            End If

            Dim frm As New frmInstalacion_Receptora
            frm.Entrada(oLinqInstalacion, oDTC)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmReceptora
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Receptora_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Receptora.M_ToolGrid_ToolEditar
        Call GRD_Receptora_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub GRD_Receptora_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Receptora.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_Receptora.GRID.Selected.Cells.Count = 0 And Me.GRD_Receptora.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then

                Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                If Me.GRD_Receptora.GRID.Selected.Cells.Count > 0 Then
                    pRow = Me.GRD_Receptora.GRID.Selected.Cells(0).Row
                Else
                    pRow = Me.GRD_Receptora.GRID.Selected.Rows(0)
                End If

                Dim _ID As Integer = pRow.Cells("ID_Instalacion_Receptora").Value
                BD.EjecutarConsulta("Delete From Instalacion_Receptora_Contacto Where ID_Instalacion_Receptora=" & _ID)
                BD.EjecutarConsulta("Delete From Instalacion_Receptora Where ID_Instalacion_Receptora=" & _ID)
                Call CargaGrid_Receptora(oLinqInstalacion.ID_Instalacion)

                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                '_DTC.SubmitChanges()
                pRow.Hidden = True
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Receptora_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Receptora.M_ToolGrid_ToolVisualitzarDobleClickRow
        If oLinqInstalacion.ID_Instalacion = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        If Me.GRD_Propuesta.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        If Guardar(True) = False Then
            Exit Sub
        End If

        Dim frm As New frmInstalacion_Receptora
        frm.Entrada(oLinqInstalacion, oDTC, Me.GRD_Receptora.GRID.Selected.Rows(0).Cells("ID_Instalacion_Receptora").Value)
        AddHandler frm.FormClosing, AddressOf AlTancarfrmReceptora
        frm.FormObrir(Me)
    End Sub

    Private Sub AlTancarfrmReceptora()
        Call CargaGrid_Receptora(oLinqInstalacion.ID_Instalacion)
    End Sub
#End Region

#Region "GRID Ruta"

    Private Sub CargaGrid_Ruta()
        Try
            'If oInstalacionAnterior = True Then
            '    Me.AccessibleName = "PropuestaAnterior"
            'End If

            Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
            If _Propuesta Is Nothing Then
                Exit Sub
            End If

            With Me.GRD_Ruta

                .M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where RutaOrden is not null and Activo=1 and ID_Propuesta=" & _Propuesta.ID_Propuesta & " Order by RutaOrden", BD)

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

#Region "Ficheros Propuestas Partes"

    Private Sub CargarGrid_FicherosPropuestasPartes()
        Try
            With Me.GRD_Ficheros2
                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_Instalacion_Ficheros_Varios WHERE TipoDocumento='Propuesta' and ID_Instalacion=" & oLinqInstalacion.ID_Instalacion))
            End With

            Me.GRD_Ficheros2.GRID.ActiveRow = Nothing

            With Me.GRD_Ficheros3
                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_Instalacion_Ficheros_Varios WHERE TipoDocumento='Parte' and ID_Instalacion=" & oLinqInstalacion.ID_Instalacion))
            End With

            Me.GRD_Ficheros3.GRID.ActiveRow = Nothing

            'RemoveHandler Preview_RTF.BotoGuardar.ItemClick, AddressOf AlApretarElBotoGuardarDelWord
            'AddHandler Preview_RTF.BotoGuardar.ItemClick, AddressOf AlApretarElBotoGuardarDelWord

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Ficheros2_M_GRID_DoubleClickRow(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Ficheros2.M_GRID_DoubleClickRow, GRD_Ficheros3.M_GRID_DoubleClickRow
        Try
            Dim pepe As New M_Archivos_Binarios.clsArchivosBinarios2(BD)
            pepe.Ejecutar_Aplicacion(Util.Retorna_Ruta_Temporal_Sistema, CType(sender, UltraGrid).ActiveRow.Cells("ID_Archivo").Value)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Ficheros_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Ficheros.M_GRID_ClickRow2, GRD_Ficheros2.M_GRID_ClickRow2, GRD_Ficheros3.M_GRID_ClickRow2
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

#Region "Grid Seguridad"

    Private Sub CargaGrid_Seguridad(ByVal pId As Integer)
        Try
            Dim _Seguretat As IEnumerable(Of Instalacion_Seguridad) = From taula In oDTC.Instalacion_Seguridad Where taula.ID_Instalacion = pId Select taula

            With Me.GRD_Seguridad

                .M.clsUltraGrid.CargarIEnumerable(_Seguretat)
                '.GRID.DataSource = _Seguretat

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
            If oLinqInstalacion.Instalacion_Seguridad.Where(Function(F) F.Usuario Is _Usuario).Count <> 0 Then
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

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion").Value = oLinqInstalacion

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguridad_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Seguridad.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqInstalacion.Instalacion_Seguridad.Remove(e.ListObject)
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

#Region "Grid Contratos"

    Private Sub CargaGrid_Contratos(ByVal pId As Integer)
        Try
            Dim _Seguretat As IEnumerable(Of Instalacion_Contrato) = From taula In oDTC.Instalacion_Contrato Where taula.ID_Instalacion = pId Select taula

            With Me.GRD_Contrato

                .M.clsUltraGrid.CargarIEnumerable(_Seguretat)
                '.GRID.DataSource = _Seguretat

                .M_NoEditable()

                Call CargarCombo_TipoContrato(.GRID)
                Call CargarCombo_Division(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TipoContrato(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Contrato_TipoContrato) = (From Taula In oDTC.Instalacion_Contrato_TipoContrato Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Contrato_TipoContrato

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Contrato_TipoContrato").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Instalacion_Contrato_TipoContrato").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contrato_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Contrato.M_GRID_DoubleClickRow2
        Dim frm As New frmInstalacion_Contrato
        frm.Entrada(oLinqInstalacion, oDTC, e.Cells("ID_Instalacion_Contrato").Value)
        AddHandler frm.AlTancarForm, AddressOf AlTancarFormulariContrato
        frm.FormObrir(Me, False)
    End Sub

    Private Sub GRD_Contrato_M_ToolGrid_ToolAfegir(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Contrato.M_ToolGrid_ToolAfegir
        Try
            Dim frm As New frmInstalacion_Contrato
            frm.Entrada(oLinqInstalacion, oDTC)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormulariContrato
            frm.FormObrir(Me, False)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contrato_M_ToolGrid_ToolEliminar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Contrato.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_Contrato.GRID.ActiveRow Is Nothing = True Then
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
                Dim _Contrato As Instalacion_Contrato = Me.GRD_Contrato.GRID.ActiveRow.ListObject
                If _Contrato.Entrada_Linea.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar, este contrato esta asociado en algún albaran o factura", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                oDTC.Instalacion_Contrato_Producto.DeleteAllOnSubmit(_Contrato.Instalacion_Contrato_Producto)
                oDTC.Instalacion_Contrato.DeleteOnSubmit(_Contrato)
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                Call AlTancarFormulariContrato()
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contrato_M_ToolGrid_ToolVisualitzar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Contrato.M_ToolGrid_ToolVisualitzar
        If Me.GRD_Contrato.GRID.ActiveRow Is Nothing = False Then
            Dim frm As New frmInstalacion_Contrato
            frm.Entrada(oLinqInstalacion, oDTC, Me.GRD_Contrato.GRID.ActiveRow.Cells("ID_Instalacion_Contrato").Value)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormulariContrato
            frm.FormObrir(Me, False)
        End If
    End Sub

    Private Sub AlTancarFormulariContrato()
        Call CargaGrid_Contratos(oLinqInstalacion.ID_Instalacion)
    End Sub

    'Private Sub GRD_Contrato_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Contrato.M_ToolGrid_ToolAfegir
    '    Try
    '        With Me.GRD_Contrato

    '            'If Guardar(False) = False Then
    '            '    Exit Sub
    '            'End If

    '            .M_ExitEditMode()
    '            .M_AfegirFila()

    '            .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaInicio").Value = Now.Date
    '            .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaFin").Value = Now.Date
    '            .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion").Value = oLinqInstalacion

    '        End With
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Contrato_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Contrato.M_ToolGrid_ToolEliminarRow
    '    Try

    '        If e.IsAddRow Then
    '            oLinqInstalacion.Instalacion_Contrato.Remove(e.ListObject)
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

    'Private Sub GRD_Contrato_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Contrato.M_GRID_AfterRowUpdate
    '    Try
    '        If oDTC.Instalacion_Contrato.Where(Function(F) F.NumeroContrato = CStr(e.Row.Cells("NumeroContrato").Value) And F.ID_Instalacion_Contrato <> CInt(e.Row.Cells("ID_Instalacion_Contrato").Value)).Count > 0 Then
    '            If e.Row.Cells("ID_Instalacion_Contrato").Value = 0 Then
    '                e.Row.Delete(False)
    '            Else
    '                e.Row.Cells("NumeroContrato").Value = e.Row.Cells("NumeroContrato").OriginalValue
    '                e.Row.Update()
    '            End If
    '            Mensaje.Mostrar_Mensaje("Imposible aplicar los cambios, éste número de contrato ya existe", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '        End If

    '        oDTC.SubmitChanges()

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

#End Region

#Region "Contactos"

    Private Sub CargaGrid_Contactos()
        Try

            Dim _Dades As IEnumerable(Of Instalacion_Contacto) = From taula In oDTC.Instalacion_Contacto Where taula.ID_Instalacion = oLinqInstalacion.ID_Instalacion Select taula

            With Me.GRD_Contactos

                .M.clsUltraGrid.CargarIEnumerable(_Dades)
                '.GRID.DataSource = _Dades

                .M_Editable()

                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Planta(.GRID, 0, oLinqInstalacion.ID_Instalacion)
                Call CargarCombo_Zona(.GRID, 0, oLinqInstalacion.ID_Instalacion)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contactos_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Contactos.M_GRID_BeforeCellActivate
        Call GRD_InstaladoEn_M_GRID_BeforeCellActivate(sender, e)
    End Sub

    Private Sub GRD_Contactos_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Contactos.M_GRID_CellListSelect
        Call GRD_InstaladoEn_M_GRID_CellListSelect(sender, e)
    End Sub

    Private Sub GRD_Conctactos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Contactos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Contactos
                If oLinqInstalacion.ID_Instalacion = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion").Value = oLinqInstalacion

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

    Private Sub GRD_Contactos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Contactos.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub



#End Region

#Region "Grid ToDo"

    Private Sub CargaGrid_ToDo(ByVal pId As Integer)
        Try
            Dim _ToDo As IEnumerable(Of Instalacion_ToDo) = From taula In oDTC.Instalacion_ToDo Where taula.ID_Instalacion = pId Select taula

            With Me.GRD_ToDo
                '.GRID.DataSource = _Gastos
                .M.clsUltraGrid.CargarIEnumerable(_ToDo)

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("ID_Parte").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("ID_Parte").CellClickAction = CellClickAction.CellSelect
                '.GRID.DisplayLayout.Bands(0).Columns("Realizado").CellActivation = Activation.NoEdit
                '.GRID.DisplayLayout.Bands(0).Columns("Realizado").CellClickAction = CellClickAction.CellSelect
                .GRID.DisplayLayout.Bands(0).Columns("FechaAlta").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("FechaAlta").CellClickAction = CellClickAction.CellSelect
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ToDo_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ToDo.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_ToDo

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaAlta").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion").Value = oLinqInstalacion


            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ToDo_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_ToDo.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqInstalacion.Instalacion_ToDo.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _ToDo As Instalacion_ToDo = e.ListObject
            If _ToDo.ID_Parte.HasValue Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar la línea, la línea esta vinculada a un parte", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_ToDo_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_ToDo.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_ToDo_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_ToDo.M_ToolGrid_ToolClickBotonsExtras2
        If e.Tool.Key = "IrAlParte" Then
            If pGrid.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If pGrid.GRID.Selected.Rows(0).Cells("ID_Parte").Value Is Nothing Then
                Exit Sub
            End If

            Dim _frm As New frmParte
            _frm.Entrada(pGrid.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
            _frm.FormObrir(Me, True)

        End If
    End Sub

    Private Sub CargarCombo_Parte(ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo)
        Try
            '  DT = From Taula In oDTC.Parte Where Taula.Parte_Asignacion.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 And Taula.Activo = True And Taula.BloquearImputacionHoras = False And (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Cliente = Taula.Cliente.Nombre, Poblacion = Taula.Cliente.Poblacion, Taula.TrabajoARealizar, Parte = Taula
            Dim _LlistatParte As IEnumerable = From Taula In oDTC.Parte Where Taula.Activo = True And Taula.ID_Instalacion = oLinqInstalacion.ID_Instalacion Order By Taula.ID_Parte Descending Select ID_Parte = Taula.ID_Parte & " - " & Taula.TrabajoARealizar, Taula.TrabajoARealizar, Taula.FechaAlta, Tipo = Taula.Parte_Tipo.Descripcion, Poblacion = Taula.Poblacion, Parte = Taula


            'If oLinqEntrada.ID_Cliente.HasValue = True Then
            '    _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True And Taula.ID_Cliente = oLinqEntrada.ID_Cliente Order By Taula.ID_Instalacion Select Taula.ID_Instalacion, Taula.FechaInstalacion, Cliente = Taula.Cliente.NombreComercial, Poblacion = Taula.Poblacion, Instalacion = Taula
            'Else
            '    _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True Order By Taula.ID_Instalacion Select Taula.ID_Instalacion, Taula.FechaInstalacion, Cliente = Taula.Cliente.NombreComercial, Poblacion = Taula.Poblacion, Instalacion = Taula
            'End If

            pCombo.DataSource = _LlistatParte
            If _LlistatParte Is Nothing Then
                Exit Sub
            End If

            With pCombo
                '.DisplayLayout.Bands(0).Columns.Add("a")
                '.Rows(0).Cells("a").Value = oDTC.Parte.FirstOrDefault

                '.AutoCompleteMode = AutoCompleteMode.Suggest
                '.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
                .AutoCompleteMode = AutoCompleteMode.None
                .AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
                .AlwaysInEditMode = False
                .DropDownStyle = UltraComboStyle.DropDownList

                .MaxDropDownItems = 16
                .DisplayMember = "ID_Parte"
                .ValueMember = "Parte"
                .DisplayLayout.Bands(0).Columns("ID_Parte").Width = 50
                .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Caption = "Código"
                .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Appearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Width = 200
                .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Header.Caption = "Descripcion"
                .DisplayLayout.Bands(0).Columns("FechaAlta").Width = 75
                .DisplayLayout.Bands(0).Columns("FechaAlta").Header.Caption = "Fecha"
                .DisplayLayout.Bands(0).Columns("Tipo").Width = 150
                .DisplayLayout.Bands(0).Columns("Tipo").Header.Caption = "Tipo"
                .DisplayLayout.Bands(0).Columns("Poblacion").Width = 200
                .DisplayLayout.Bands(0).Columns("Poblacion").Header.Caption = "Población"
                .DisplayLayout.Bands(0).Columns("Parte").Hidden = True
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_ToDo_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_ToDo.M_Grid_InitializeRow
        Dim _ComboParte As New Infragistics.Win.UltraWinGrid.UltraCombo

        Call CargarCombo_Parte(_ComboParte)

        e.Row.Cells("Parte").EditorComponent = Nothing
        e.Row.Cells("Parte").EditorComponent = _ComboParte
    End Sub
#End Region

#Region "Grid ActividadCRM"
    Private Sub CargaGrid_ActividadCRM(ByVal pID As Integer)
        Try
            Me.GRD_ActividadCRM.M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_ActividadCRM Where Activo=1 and ID_Instalacion=" & pID & " Order by ID_ActividadCRM Desc", True))

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


    'Public Shared Function GetLengthLimit(ent As Model.RIVFeedsEntities, table As String, field As String) As Integer
    '    Dim maxLength As Integer = 0 ' default value = we can't determine the length
    '    ' Connect to the meta data...
    '    Dim mw As MetadataWorkspace = ent.MetadataWorkspace
    '    ' Force a read of the model (just in case)...
    '    ' http:'thedatafarm.com/blog/data-access/quick-trick-for-forcing-metadataworkspace-itemcollections-to-load/
    '    Dim q = ent.Clients
    '    Dim n As String = q.ToTraceString()
    '    ' Get the items (tables, views, etc)...
    '    Dim items = mw.GetItems(Of EntityType)(DataSpace.SSpace)
    '    For Each i In items
    '        If i.Name.Equals(table, StringComparison.CurrentCultureIgnoreCase) Then
    '            ' wrapped in try/catch for other data types that don't have a MaxLength...
    '            Try
    '                Dim val As String = i.Properties(field).TypeUsage.Facets("MaxLength").Value.ToString()
    '            Integer.TryParse(val, out maxLength)

    '            Catch
    '                maxLength = 0

    '            End Try
    '            break()

    '        End If

    '    Next

    '    Return maxLength
    'End Function

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

            End If

            ' TODO: liberar recursos no administrados (objetos no administrados) e invalidar Finalize() below.
            ' TODO: Establecer campos grandes como Null.
            'RemoveHandler Fichero.DespresDeCarregarDades, AddressOf DespresDeCarregarDades
            oDTC = Nothing
            Fichero = Nothing

        End If
        Me.disposedValue = True
        MyBase.Dispose(True)
    End Sub
#End Region





End Class