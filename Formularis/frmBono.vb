Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmBono
    Dim oDTC As DTCDataContext
    Dim oLinqBono As Bono
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try

            If oLinqBono.ID_Bono <> 0 Then
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
                    If oLinqBono.Parte.Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible eliminar el bono, el bono está asignado a uno o más partes", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    oDTC.Bono_Instalacion.DeleteAllOnSubmit(oLinqBono.Bono_Instalacion)
                    oDTC.Bono.DeleteOnSubmit(oLinqBono)
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
        Informes.ObrirInformePreparacio(oDTC, EnumInforme.Bonos, "[ID_Bono] = " & oLinqBono.ID_Bono, "Bono - " & oLinqBono.Codigo)

    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pId As Integer = 0)
        Try
            Me.AplicarDisseny()


            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Bono_Archivo", 1)



            Me.GRD_HorasAsignadas.M.clsToolBar.Boto_Afegir("IrAParte", "Ver parte", True)
            Me.GRD_Instalacion.M.clsToolBar.Boto_Afegir("IrAInstalacion", "Ver instalación", True)

            Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True

            Me.TL_Cliente.ButtonsRight("Ficha").Enabled = True
            Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = True

            Me.TL_FacturaRelacionada.ButtonsRight("Ficha").Enabled = True
            Me.TL_FacturaRelacionada.ButtonsRight("Lupeta").Enabled = True

            Call Netejar_Pantalla()


            If pId <> 0 Then
                Call Cargar_Form(pId)
            End If

            Me.KeyPreview = False

            'Me.ToolForm.M.Botons.tEliminar.SharedProps.Visible = False

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

            Call GetFromForm(oLinqBono)

            If oLinqBono.ID_Bono = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Bono.InsertOnSubmit(oLinqBono)
                oDTC.SubmitChanges()
                Me.TE_Codigo.Text = oLinqBono.Codigo
                Call Fichero.Cargar_GRID(oLinqBono.ID_Bono) 'Fem això pq la classe tingui el ID de pressupost
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)

                Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = False
                Me.TL_Producto.ButtonsRight("Lupeta").Enabled = False

                'Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeGuardar)
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If

            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqBono
                Me.TE_Codigo.Value = .Codigo
                Me.TL_Producto.Text = .DescripcionProducto
                Me.TL_Producto.Tag = .ID_Producto
                Me.T_Cantidad.Value = .Cantidad
                Me.T_HorasConsumidas.Text = .HorasConsumidas
                Me.T_HorasDiferencia.Text = .Cantidad - .HorasConsumidas
                Me.TL_Cliente.Tag = .ID_Cliente
                Me.TL_Cliente.Text = .Cliente.Nombre
                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Caducidad.Value = .FechaCaducidad
                Me.CH_Cerrado.Checked = .Cerrado
                Me.R_Observaciones.pText = .Observaciones
                Me.R_CondicionesComerciales.pText = .CondicionesComerciales

                If .Entrada Is Nothing = False Then
                    Me.TL_FacturaRelacionada.Text = .Entrada.Descripcion
                    Me.TL_FacturaRelacionada.Tag = .ID_Entrada
                End If
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pBono As Bono)
        Try
            With pBono
                .Codigo = Me.TE_Codigo.Value
                .DescripcionProducto = Me.TL_Producto.Text
                .Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TL_Producto.Tag)).FirstOrDefault
                .Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(Me.TL_Cliente.Tag)).FirstOrDefault
                .FechaAlta = Me.DT_Alta.Value
                .FechaCaducidad = Me.DT_Caducidad.Value

                .Cantidad = Me.T_Cantidad.Value
                .HorasConsumidas = Me.T_HorasConsumidas.Text
                .FechaAlta = Me.DT_Alta.Value
                .FechaCaducidad = Me.DT_Caducidad.Value
                .Cerrado = Me.CH_Cerrado.Checked

                .Observaciones = Me.R_Observaciones.pText
                .CondicionesComerciales = Me.R_CondicionesComerciales.pText

                If Me.TL_FacturaRelacionada.Text.Length = 0 Then
                    .Entrada = Nothing
                Else
                    .Entrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = CInt(Me.TL_FacturaRelacionada.Tag)).FirstOrDefault
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()

            oLinqBono = (From taula In oDTC.Bono Where taula.ID_Bono = pID Select taula).First

            Call SetToForm()

            Call CargaGrid_Instalacion(pID)
            Call CargarGrid_HorasAsignadas(pID)

            Fichero.Cargar_GRID(pID)
            'Call CargarGrid_Stocks(pID)

            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Me.EstableixCaptionForm("Bono: " & (oLinqBono.DescripcionProducto))

            Me.T_HorasConsumidas.Value = BD.RetornaValorSQL("Select HorasConsumidas From C_Bono Where ID_Bono=" & oLinqBono.ID_Bono)
            Me.T_HorasDiferencia.Value = BD.RetornaValorSQL("Select HorasRestantes From C_Bono Where ID_Bono=" & oLinqBono.ID_Bono)

            Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = False
            Me.TL_Producto.ButtonsRight("Lupeta").Enabled = False

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqBono = New Bono
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.TE_Codigo.Value = Nothing

        Me.DT_Alta.Value = Now.Date
        Me.DT_Caducidad.Value = Nothing

        Fichero.Cargar_GRID(0)
        Call CargaGrid_Instalacion(0)
        Call CargarGrid_HorasAsignadas(0)

        ' Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Instalacion")
        'Me.TAB_Principal.Tabs("General").Selected = True

        If oDTC.Bono.Count > 0 Then
            Me.TE_Codigo.Value = oDTC.Bono.Max(Function(F) F.Codigo) + 1
        Else
            Me.TE_Codigo.Value = oDTC.Contadores.FirstOrDefault.Bono
        End If

        Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = True
        Me.TL_Producto.ButtonsRight("Lupeta").Enabled = True

        ErrorProvider1.Clear()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.TL_Producto)
                oClsControls.ControlBuit(.TL_Producto, clsControles.EPropietat.pTag)
                oClsControls.ControlBuit(.TL_Cliente, clsControles.EPropietat.pTag)
                oClsControls.ControlBuit(.T_Cantidad)
                oClsControls.ControlBuit(.DT_Alta)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()

        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Bono ORDER BY FechaAlta Desc", Me.TE_Codigo, "ID_Bono", "ID_Bono")

        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat

    End Sub

    Private Sub AlTancarLlistat(ByVal pID As String)
        Try
            Call Cargar_Form(pID)
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

    'Private Sub TAB_Principal_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
    '    Select Case e.Tab.Key
    '        Case "Stocks"
    '            Call CargarGrid_Stocks(oLinqAlmacen.ID_Almacen)
    '    End Select
    'End Sub

    Private Sub TE_Cliente_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TL_Cliente.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Cliente Where ID_Cliente_Tipo=" & EnumClienteTipo.Cliente & " and Activo=1 ORDER BY Nombre", Me.TL_Cliente, "ID_Cliente", "Nombre")
        End If
        If e.Button.Key = "Ficha" And Me.TL_Cliente.Tag Is Nothing = False Then
            Dim frmClient As New frmCliente
            frmClient.Entrada(Me.TL_Cliente.Tag)
            frmClient.FormObrir(Me, True)
        End If
    End Sub

    Private Sub TE_Producto_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TL_Producto.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto Where Activo=1 and Esbono=1 ORDER BY Descripcion", Me.TL_Producto, "ID_Producto", "Descripcion")
            AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarFormulariProducto
        End If

        If e.Button.Key = "Ficha" And Me.TL_Producto.Tag Is Nothing = False Then
            Dim _frmProducto As New frmProducto
            _frmProducto.Entrada(Me.TL_Producto.Tag)
            _frmProducto.FormObrir(Me, True)
        End If
    End Sub

    Private Sub AlTancarFormulariProducto(pIDProducte As String)
        If pIDProducte <> 0 Then
            Dim _Producte As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = pIDProducte).FirstOrDefault
            Me.T_Cantidad.Value = _Producte.Bono_Cantidad
            Me.R_CondicionesComerciales.pText = _Producte.DescripcionAmpliada
        End If
    End Sub

    Private Sub OP_Instalacions_ValueChanged(sender As Object, e As EventArgs) Handles OP_Instalacions.ValueChanged
        'Fem això pq si no perd el foco la celda de la grid al apretar el segon cop, si has canviat de option button, no s'entera la càrrega del combo
        Me.GRD_Instalacion.GRID.ActiveRow = Nothing
        Me.GRD_Instalacion.GRID.Selected.Rows.Clear()
    End Sub

    Private Sub TL_FacturaRelacionada_EditorButtonClick(sender As Object, e As UltraWinEditors.EditorButtonEventArgs) Handles TL_FacturaRelacionada.EditorButtonClick
        Select Case e.Button.Key
            Case "Lupeta"
                If TL_Cliente.Tag Is Nothing = True Then
                    Mensaje.Mostrar_Mensaje("Imposible seleccionar una factura si préviamente no ha seleccionado el cliente", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                End If

                Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
                LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Entrada Where ID_Entrada_Estado=" & EnumEntradaEstado.Pendiente & " and ID_Entrada_Tipo=" & EnumEntradaTipo.FacturaVenta & " and ID_Cliente=" & Me.TL_Cliente.Tag & "  ORDER BY FechaEntrada", Me.TL_FacturaRelacionada, "ID_Entrada", "Codigo", Me.TL_FacturaRelacionada, "Descripcion")

            Case "Ficha"
                If Me.TL_FacturaRelacionada.Tag Is Nothing = False Then
                    Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
                    frm.Entrada(CInt(Me.TL_FacturaRelacionada.Tag), EnumEntradaTipo.AlbaranVenta)
                    frm.FormObrir(frmPrincipal, True)
                End If
        End Select

    End Sub

#End Region

#Region "Grid Instalacion"

    Private Sub CargaGrid_Instalacion(ByVal pId As Integer)
        Try
            Dim _BonoInstalacion As IEnumerable(Of Bono_Instalacion) = From taula In oDTC.Bono_Instalacion Where taula.ID_Bono = pId Select taula

            With Me.GRD_Instalacion
                .M.clsUltraGrid.CargarIEnumerable(_BonoInstalacion)

                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Instalacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Instalacion.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Instalacion

                If oLinqBono.ID_Bono = 0 Then
                    If Guardar() = False Then
                        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                        Exit Sub
                    End If
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Bono").Value = oLinqBono

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Instalacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Instalacion.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqBono.Bono_Instalacion.Remove(e.ListObject)
                Exit Sub
            End If

            'Dim _Instalacion As Entrada_Instalacion = e.ListObject

            'If oLinqEntrada.Entrada_Linea.Where(Function(F) F.ID_Instalacion.HasValue AndAlso F.ID_Instalacion = _Instalacion.ID_Instalacion).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible eliminar la instalacion. Esta instalación esta asignada a una línea del documento", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Exit Sub
            'End If

            'If oDTC.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.Entrada_Linea.ID_Entrada = oLinqEntrada.ID_Entrada And F.Propuesta_Linea.Propuesta.ID_Instalacion = _Instalacion.ID_Instalacion).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible eliminar la instalacion. Hay líneas de esta instalación asignadas a una o más líneas del documento", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_Instalacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Instalacion.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Instalacion_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Instalacion.M_Grid_InitializeRow
        Dim _ComboInstalacio As New Infragistics.Win.UltraWinGrid.UltraCombo

        Call CargarCombo_Instalacion(_ComboInstalacio)

        e.Row.Cells("Instalacion").EditorComponent = Nothing
        e.Row.Cells("Instalacion").EditorComponent = _ComboInstalacio
    End Sub

    Private Sub CargarCombo_Instalacion(ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo, Optional ByVal pCell As UltraGridCell = Nothing)
        Try
            '  DT = From Taula In oDTC.Parte Where Taula.Parte_Asignacion.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 And Taula.Activo = True And Taula.BloquearImputacionHoras = False And (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Cliente = Taula.Cliente.Nombre, Poblacion = Taula.Cliente.Poblacion, Taula.TrabajoARealizar, Parte = Taula
            Dim _LlistatInstalacions As IEnumerable

            If pCell Is Nothing Then
                '_LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True And Taula.ID_Cliente = oLinqEntrada.ID_Cliente Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Responsable = Taula.Personal.Nombre, Instalacion = Taula
                _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Direccion = Taula.Direccion, Responsable = Taula.Personal.Nombre, Instalacion = Taula, Cliente = Taula.Cliente.Nombre
            Else
                Dim _InstalacioActual As Instalacion = pCell.Value


                Select Case Me.OP_Instalacions.Value
                    Case 0
                        _LlistatInstalacions = From Taula In oDTC.Instalacion Where (Taula.Activo = True And Taula.ID_Cliente = oLinqBono.ID_Cliente) Or Taula Is _InstalacioActual Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Direccion = Taula.Direccion, Responsable = Taula.Personal.Nombre, Instalacion = Taula, Cliente = Taula.Cliente.Nombre
                    Case 1
                        _LlistatInstalacions = From Taula In oDTC.Instalacion Where (Taula.Activo = True And Taula.ID_Cliente_Contratante = oLinqBono.ID_Cliente) Or Taula Is _InstalacioActual Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Direccion = Taula.Direccion, Responsable = Taula.Personal.Nombre, Instalacion = Taula, Cliente = Taula.Cliente.Nombre
                    Case 2
                        _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True Or Taula Is _InstalacioActual Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Direccion = Taula.Direccion, Responsable = Taula.Personal.Nombre, Instalacion = Taula, Cliente = Taula.Cliente.Nombre
                End Select
            End If

            pCombo.DataSource = _LlistatInstalacions
            If _LlistatInstalacions Is Nothing Then
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
                .DisplayMember = "Visualitzar"
                .ValueMember = "Instalacion"
                .DisplayLayout.Bands(0).Columns("ID_Instalacion").Width = 50
                .DisplayLayout.Bands(0).Columns("ID_Instalacion").Header.Caption = "Código"
                .DisplayLayout.Bands(0).Columns("ID_Instalacion").Header.Appearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("Descripcion").Width = 200
                .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
                .DisplayLayout.Bands(0).Columns("Poblacion").Width = 200
                .DisplayLayout.Bands(0).Columns("Poblacion").Header.Caption = "Población"
                .DisplayLayout.Bands(0).Columns("Direccion").Width = 200
                .DisplayLayout.Bands(0).Columns("Direccion").Header.Caption = "Dirección"
                .DisplayLayout.Bands(0).Columns("Responsable").Width = 200
                .DisplayLayout.Bands(0).Columns("Responsable").Header.Caption = "Responsable"
                .DisplayLayout.Bands(0).Columns("Instalacion").Hidden = True
                .DisplayLayout.Bands(0).Columns("Visualitzar").Hidden = True
                .DisplayLayout.Bands(0).Columns("Cliente").Width = 200
                .DisplayLayout.Bands(0).Columns("Cliente").Header.Caption = "Cliente"
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Instalacion_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Instalacion.M_ToolGrid_ToolClickBotonsExtras2
        If e.Tool.Key = "IrAInstalacion" Then
            With Me.GRD_Instalacion
                If .GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If

                Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Instalacion").Value)
                frm.FormObrir(frmPrincipal, True)
            End With
        End If
    End Sub

    Private Sub GRD_InstaladoEn_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Instalacion.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "Instalacion"
                Call CargarCombo_Instalacion(e.Cell.EditorComponent, e.Cell)
        End Select
    End Sub

#End Region

#Region "Grid Parte"

    Private Sub CargarGrid_HorasAsignadas(ByVal pID As Integer)
        Try

            With Me.GRD_HorasAsignadas

                .M.clsUltraGrid.Cargar("Select * From C_Parte_Horas Where ID_Parte in (Select ID_Parte From Parte Where ID_Bono=" & pID & ") Order by Fecha desc", BD)

                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing

                Dim pRow As UltraGridRow
                For Each pRow In .GRID.Rows
                    If pRow.Cells("ErrorDelTecnico").Value = True Or pRow.Cells("ErrorDeOtroTecnico").Value = True Then
                        pRow.CellAppearance.BackColor = Color.LightCoral
                    End If
                Next

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
    Private Sub GRD_HorasAsignadas_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_HorasAsignadas.M_ToolGrid_ToolClickBotonsExtras2
        If e.Tool.Key = "IrAParte" Then
            With Me.GRD_HorasAsignadas
                If .GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If
                '  Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                Dim frm As New frmParte
                frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
                frm.FormObrir(frmPrincipal, True)
            End With
        End If
    End Sub

#End Region




End Class