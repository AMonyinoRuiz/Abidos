Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmAlmacen
    Dim oDTC As DTCDataContext
    Dim oLinqAlmacen As Almacen
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqAlmacen.ID_Almacen <> 0 Then
                If Mensaje.Mostrar_Mensaje("Desea dar de alta/baja el registro?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then

                    ' oLinqCliente.Activo = False
                    If DT_Baja.Value Is Nothing Then
                        If oDTC.RetornaStock(0, oLinqAlmacen.ID_Almacen).Count > 0 Then
                            Mensaje.Mostrar_Mensaje("Imposible dar de baja un almacén con productos en stock", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                            Exit Sub
                        End If
                        Me.DT_Baja.Value = Date.Now
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

    Public Sub Entrada(Optional ByVal pId As Integer = 0)
        Try
            Me.AplicarDisseny()


            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Almacen_Archivo", 1)



            Me.GRD_Stock.M.clsToolBar.Boto_Afegir("VerTrazabilidad", "Ver trazabilidad del producto", True)
            Me.GRD_Stock.M.clsToolBar.Boto_Afegir("VerProducto", "Ver ficha del producto", True)
            Me.GRD_Stock.M.clsToolBar.Boto_Afegir("VisualizarFotos", "Visualizar fotos", True)

            Me.TE_Cliente.ButtonsRight("Lupeta").Enabled = True
            Me.TE_Proveedor.ButtonsRight("Lupeta").Enabled = True
            Me.TE_Parte.ButtonsRight("Lupeta").Enabled = True

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

            'If oLinqCliente.ID_Cliente = 0 Then
            '    If BD.RetornaValorSQL("SELECT Count (*) From Cliente WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
            '        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
            '        Exit Function
            '    End If
            'Else
            '    If BD.RetornaValorSQL("SELECT Count (*) From Cliente WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_Cliente<>" & oLinqCliente.ID_Cliente) > 0 Then
            '        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
            '        Exit Function
            '    End If
            'End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            If Me.C_Personal.SelectedIndex <> -1 Then
                If oLinqAlmacen.ID_Almacen = 0 And oDTC.Almacen.Where(Function(F) F.ID_Personal = CInt(Me.C_Personal.Value)).Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible introducir un almacén con el personal asignado. El personal sólo puede tener un almacén asignado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                Else
                    If oLinqAlmacen.ID_Almacen <> 0 And oDTC.Almacen.Where(Function(F) F.ID_Personal = CInt(Me.C_Personal.Value) And F.ID_Almacen <> oLinqAlmacen.ID_Almacen).Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible introducir un almacén con el personal asignado. El personal sólo puede tener un almacén asignado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Function
                    End If
                End If
            End If

            Call GetFromForm(oLinqAlmacen)

            If oLinqAlmacen.ID_Almacen = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Almacen.InsertOnSubmit(oLinqAlmacen)
                oDTC.SubmitChanges()
                Me.TE_Codigo.Text = oLinqAlmacen.ID_Almacen
                Call Fichero.Cargar_GRID(oLinqAlmacen.ID_Almacen) 'Fem això pq la classe tingui el ID de pressupost
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                'Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeGuardar)
                Me.OP_TipusMagatzem.Enabled = False 'no permetrem modificar el option un cop guardat
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If

            Dim _Almacen As Almacen
            If oLinqAlmacen.Predeterminado = True Then  'Si aquest és el magatzem predetrerminat mirarem si ni ha algun altre que tb ho sigui, i si hi és el despredeterminarem
                _Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen <> oLinqAlmacen.ID_Almacen And F.Predeterminado = True).FirstOrDefault
                If _Almacen Is Nothing = False Then
                    _Almacen.Predeterminado = False
                End If
                oDTC.SubmitChanges()
            End If

            'Aqui comprovem si hi ha algun magatzem predeterminat, si no ni ha cap li assigarem a aquest el predeterminat ja que en te que haver un
            _Almacen = oDTC.Almacen.Where(Function(F) F.Predeterminado = True).FirstOrDefault
            If _Almacen Is Nothing = True Then
                oLinqAlmacen.Predeterminado = True
            End If
            oDTC.SubmitChanges()

            'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqAlmacen
                Me.TE_Codigo.Value = .ID_Almacen
                Me.T_Descripcion.Text = .Descripcion
                Me.T_Persona_Contacto.Text = .PersonaContacto
                Me.T_Email.Text = .Email
                Me.T_Telefono.Text = .Telefono
                Me.T_Fax.Text = .Fax
                Me.T_Direccion.Text = .Direccion
                Me.T_Poblacion.Text = .Poblacion
                Me.T_Provincia.Text = .Provincia
                Me.T_CP.Text = .CP
                Me.OP_TipusMagatzem.Value = .ID_Almacen_Tipo
                If .ID_Personal.HasValue Then
                    Me.C_Personal.Value = .ID_Personal
                Else
                    Me.C_Personal.Value = 0
                End If
                If .ID_Cliente.HasValue Then
                    Me.TE_Cliente.Tag = .ID_Cliente
                    Me.TE_Cliente.Text = oLinqAlmacen.Cliente.Nombre
                Else
                    Me.TE_Cliente.Tag = Nothing
                    Me.TE_Cliente.Text = ""
                End If

                If .ID_Proveedor.HasValue Then
                    Me.TE_Proveedor.Tag = .ID_Proveedor
                    Me.TE_Proveedor.Text = oLinqAlmacen.Proveedor.Nombre
                Else
                    Me.TE_Proveedor.Tag = Nothing
                    Me.TE_Proveedor.Text = ""
                End If

                If .ID_Parte.HasValue Then
                    Me.TE_Parte.Tag = .ID_Parte
                    Me.TE_Parte.Text = oLinqAlmacen.Parte.ID_Parte
                Else
                    Me.TE_Parte.Tag = Nothing
                    Me.TE_Parte.Text = ""
                End If

                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Baja.Value = .FechaBaja
                If DT_Baja.Value Is Nothing Then
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                Else
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                End If



                Me.R_Observaciones.pText = .Observaciones
                Me.CH_Predeterminado.Checked = .Predeterminado

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pAlmacen As Almacen)
        Try
            With pAlmacen
                .Activo = True

                '.Cliente = (From Taula In oDTC.Cliente Where Taula.ID_Cliente = CInt(Me.T_Nombre.Tag) Select Taula).First
                '.Codigo = Me.TE_Codigo.Value
                .Descripcion = Me.T_Descripcion.Text
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
                .Predeterminado = Me.CH_Predeterminado.Checked
                .Almacen_Tipo = oDTC.Almacen_Tipo.Where(Function(F) F.ID_Almacen_Tipo = CInt(Me.OP_TipusMagatzem.Value)).FirstOrDefault  'Te que posar-se abans de la carga del client i del proveidor

                If Me.C_Personal.Value = 0 Then
                    .Personal = Nothing
                Else
                    .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_Personal.Value)).FirstOrDefault
                End If

                If Me.TE_Cliente.Tag Is Nothing Then
                    .Cliente = Nothing
                Else
                    .Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(Me.TE_Cliente.Tag)).FirstOrDefault
                End If

                If Me.TE_Proveedor.Tag Is Nothing Then
                    .Proveedor = Nothing
                Else
                    .Proveedor = oDTC.Proveedor.Where(Function(F) F.ID_Proveedor = CInt(Me.TE_Proveedor.Tag)).FirstOrDefault
                End If

                If Me.TE_Parte.Tag Is Nothing Then
                    .Parte = Nothing
                Else
                    .Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = CInt(Me.TE_Parte.Tag)).FirstOrDefault
                End If



            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()

            oLinqAlmacen = (From taula In oDTC.Almacen Where taula.ID_Almacen = pID Select taula).First

            Call SetToForm()

            Fichero.Cargar_GRID(pID)
            Call CargarGrid_Stocks(pID)

            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Me.EstableixCaptionForm("Almacén: " & (oLinqAlmacen.Descripcion))
            Me.OP_TipusMagatzem.Enabled = False

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqAlmacen = New Almacen
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing

        Util.Cargar_Combo(Me.C_Personal, "Select ID_Personal, Nombre From Personal where activo=1", False, False)

        Me.DT_Alta.Value = Now.Date
        Me.DT_Baja.Value = Nothing
        Me.C_Personal.SelectedIndex = -1
        Fichero.Cargar_GRID(0)
        Call CargarGrid_Stocks(0)

        Me.OP_TipusMagatzem.Value = 1

        ' Me.C_OrigenCliente.Value = 1
        ' Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Instalacion")

        Me.TAB_Principal.Tabs("General").Selected = True
        ErrorProvider1.Clear()
        Me.OP_TipusMagatzem.Enabled = True
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                'oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.T_Descripcion)
                oClsControls.ControlBuit(.DT_Alta)
                'oClsControls.ControlBuit(.T_Persona_Contacto)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Almacen Where AlmacenActivo=1 and ID_Almacen_Tipo<>" & EnumAlmacenTipo.Parte & " ORDER BY ID_Almacen", Me.TE_Codigo, "ID_Almacen", "ID_Almacen")
        LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = True
        LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Caption = "Incluir los almacenes tipo 'Parte'"


        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat
        AddHandler LlistatGeneric.AlApretarElBotoAuxiliar, AddressOf AlApretarElBotoAuxiliarDelLlistatGeneric


        'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
        '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
        '        pRow.Appearance.BackColor = Color.LightCoral
        '    End If
        'Next
    End Sub

    Private Sub AlApretarElBotoAuxiliarDelLlistatGeneric(ByRef pInstanciaLlistatGeneric As M_LlistatGeneric.clsLlistatGeneric)
        'Això es només per traspasos de magatzems
        pInstanciaLlistatGeneric.pGrid.M.clsUltraGrid.Cargar("SELECT * FROM C_Almacen Where AlmacenActivo=1 ORDER BY ID_Almacen", BD)
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

#End Region

#Region "Grid Stock"

    Private Sub CargarGrid_Stocks(ByVal pIDAlmacen As Integer)
        Try

            If pIDAlmacen = 0 Then
                Me.GRD_Stock.M.clsUltraGrid.Cargar(BD.RetornaEsquemaDataTable("Select *, '' as Foto From RetornaStock(0," & pIDAlmacen & ") Order by ProductoDescripcion"))
            Else
                Me.GRD_Stock.M.clsUltraGrid.Cargar("Select *, '' as Foto From RetornaStock(0," & pIDAlmacen & ") Order by ProductoDescripcion", BD)
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

    Private Sub GRD_Stock_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Stock.M_ToolGrid_ToolVisualitzarDobleClickRow
        ' If Me.GRD_Stock.GRID.Selected.Rows.Count = 1 Then
        '    Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
        '    frm.Entrada(Me.GRD_Stock.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
        '    frm.FormObrir(Me, True)
        ' End If
    End Sub

    Private Sub GRD_Stock_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Stock.M_ToolGrid_ToolClickBotonsExtras
        Try
            Select Case e.Tool.Key
                Case "VerProducto"
                    With Me.GRD_Stock
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Band.Index <> 0 Then
                            Exit Sub
                        End If

                        Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
                        frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
                        frm.FormObrir(frmPrincipal, True)
                    End With

                Case "VerTrazabilidad"
                    With Me.GRD_Stock
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Band.Index <> 0 Then
                            Exit Sub
                        End If

                        Dim frm As New frmProducto_Trazabilidad
                        frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
                        frm.FormObrir(Me, True)
                    End With

                Case "VisualizarFotos"
                    With Me.GRD_Stock
                        If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
                            .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "Visualizar fotos"
                        Else
                            .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos"
                        End If
                        Call CargarGrid_Stocks(oLinqAlmacen.ID_Almacen)
                    End With
            End Select

        Catch ex As Exception
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm()
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Stock_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Stock.M_Grid_InitializeRow
        With Me.GRD_Stock
            If e.Row.Band.Index = 0 Then
                If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
                    Dim _Producto As Producto
                    _Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(e.Row.Cells("ID_Producto").Value)).FirstOrDefault
                    If _Producto.Archivo Is Nothing = False AndAlso _Producto.Archivo.CampoBinario.Length > 0 Then
                        e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(_Producto.Archivo.CampoBinario.ToArray)
                    End If
                    .GRID.DisplayLayout.Override.DefaultRowHeight = 40
                Else
                    .GRID.DisplayLayout.Override.DefaultRowHeight = 20
                End If
            End If
        End With
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
                Dim ooLinqAlmacen As Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.TE_Codigo.Value)).FirstOrDefault()
                If ooLinqAlmacen Is Nothing = False Then
                    Call Cargar_Form(ooLinqAlmacen.ID_Almacen)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If
            Else
                'Me.TE_Codigo.Value = oDTC.Cliente.Where(Function(F) F.Activo = True).Max(Function(F) F.Codigo) + 1
                'Exit Sub
            End If
        End If
    End Sub

    Private Sub TAB_Principal_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
        Select Case e.Tab.Key
            Case "Stocks"
                Call CargarGrid_Stocks(oLinqAlmacen.ID_Almacen)
        End Select
    End Sub

    Private Sub OP_TipusMagatzem_ValueChanged(sender As Object, e As EventArgs) Handles OP_TipusMagatzem.ValueChanged
        Me.Pan_Cliente.Visible = False
        Me.Pan_Proveedor.Visible = False
        Me.Pan_Personal.Visible = False
        Me.Pan_Parte.Visible = False
        Me.TE_Cliente.Tag = Nothing
        Me.TE_Proveedor.Tag = Nothing
        Me.TE_Parte.Tag = Nothing
        Me.TE_Cliente.Text = Nothing
        Me.TE_Proveedor.Text = Nothing
        Me.TE_Parte.Text = Nothing
        Me.C_Personal.Value = Nothing
        Select Case Me.OP_TipusMagatzem.Value
            Case 1
            Case 2
                Me.Pan_Personal.Visible = True
            Case 3
                Me.Pan_Cliente.Visible = True
            Case 4
                Me.Pan_Proveedor.Visible = True
            Case 5
                Me.Pan_Parte.Visible = True
        End Select
    End Sub

    Private Sub TE_Cliente_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Cliente.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM Cliente Where ID_Cliente_Tipo=" & EnumClienteTipo.Cliente & " and Activo=1 ORDER BY Nombre", Me.TE_Cliente, "ID_Cliente", "Nombre")
        End If
        'If e.Button.Key = "Ficha" And Me.TE_Cliente.Tag Is Nothing = False Then
        '    Dim frmClient As New frmCliente
        '    frmClient.Entrada(Me.TL_Cliente.Tag)
        '    frmClient.FormObrir(Me, True)
        'End If
    End Sub

    Private Sub TE_Proveedor_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Proveedor.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM Proveedor Where Activo=1 ORDER BY Nombre", Me.TE_Proveedor, "ID_Proveedor", "Nombre")
        End If

        'If e.Button.Key = "Ficha" And Me.TL_Proveedor.Tag Is Nothing = False Then
        '    Dim _frm As New frmProveedor
        '    _frm.Entrada(Me.TL_Proveedor.Tag)
        '    _frm.FormObrir(Me, True)
        'End If
    End Sub

    Private Sub TE_Parte_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Parte.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Parte Where Activo=1 ORDER BY ID_Parte Desc", Me.TE_Parte, "ID_Parte", "ID_Parte")
        End If

        'If e.Button.Key = "Ficha" And Me.TL_Proveedor.Tag Is Nothing = False Then
        '    Dim _frm As New frmProveedor
        '    _frm.Entrada(Me.TL_Proveedor.Tag)
        '    _frm.FormObrir(Me, True)
        'End If
    End Sub

    Private Sub C_Personal_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Personal.BeforeDropDown
        Dim _SQL As String = ""
        If oLinqAlmacen.ID_Personal.HasValue Then
            _SQL = " or ID_Personal= " & oLinqAlmacen.ID_Personal
        End If
        Util.Cargar_Combo(Me.C_Personal, "Select ID_Personal, Nombre From Personal Where Activo=1 and FechaBajaEmpresa is null" & _SQL & " Order by Nombre", False, False)
        Me.C_Personal.Value = oLinqAlmacen.ID_Personal
    End Sub
#End Region


End Class