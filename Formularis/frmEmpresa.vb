Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEmpresa
    Dim oDTC As DTCDataContext
    Dim oLinqEmpresa As Empresa
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqEmpresa.ID_Empresa <> 0 Then
                If Mensaje.Mostrar_Mensaje("Desea dar de alta/baja el registro?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then


                    If DT_Baja.Value Is Nothing Then
                        If oLinqEmpresa.Predeterminada = True Then
                            Mensaje.Mostrar_Mensaje("Imposible dar de baja una empresa predeterminada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                            Exit Sub
                        End If
                        Me.DT_Baja.Value = Date.Now
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                        oLinqEmpresa.Activo = False
                    Else
                        Me.DT_Baja.Value = Nothing
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                        oLinqEmpresa.Activo = True
                    End If
                    Call Guardar()

                    oDTC.SubmitChanges()
                    Call Netejar_Pantalla()
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

    Private Sub ToolForm_m_ToolForm_ToolClick_Botones_Extras(Sender As Object, e As UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        Select Case e.Tool.Key
            Case "m_aplicar_fotos_mini"
                Dim _Producto As Producto
                For Each _Producto In oDTC.Producto
                    If _Producto.ID_Archivo_FotoPredeterminada.HasValue Then
                        Dim _ArchivoMini As New Archivo
                        _ArchivoMini.Activo = True
                        _ArchivoMini.CampoBinario = Util.ImageToBinary(M_Util.clsFunciones.CompressImage(M_Util.clsFunciones.ResizeImage(Util.BinaryToImage(_Producto.Archivo.CampoBinario.ToArray), New System.Drawing.Size(40, 40), True))).ToArray
                        _ArchivoMini.Descripcion = "Foto Mini ID=" & _Producto.ID_Producto
                        _ArchivoMini.Ruta_Fichero = _Producto.Archivo.Ruta_Fichero
                        _ArchivoMini.Tipo = _Producto.Archivo.Tipo
                        _ArchivoMini.Fecha = Now.Date

                        oDTC.Archivo.InsertOnSubmit(_ArchivoMini)
                        _Producto.Archivo_Mini = _ArchivoMini
                    End If
                Next
                oDTC.SubmitChanges()
        End Select
    End Sub
#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pId As Integer = 0)
        Try

            Me.AplicarDisseny()

            Dim _Anys As Integer
            For _Anys = 2010 To 2030
                Me.C_Año.Items.Add(_Anys)
            Next

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Empresa_Archivo", 1)

            ' Me.T_Codigo.ButtonsRight("Lupeta").Enabled = True
            Call Netejar_Pantalla()

            ' If pId <> 0 Then
            Call Cargar_Form(1)

            Me.C_Año.Value = Now.Year
            'End If
            Me.KeyPreview = False

            'Me.ToolForm.M.Botons.tBuscar.SharedProps.Visible = False
            'Me.ToolForm.M.Botons.tEliminar.SharedProps.Visible = False
            'Me.ToolForm.M.Botons.tNou.SharedProps.Visible = False

            Me.ToolForm.M.clsToolBar.Afegir_Boto("m_aplicar_fotos_mini", "Aplicar fotos mini", True)

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

            If Me.CH_Predeterminada.Checked = True Then
                If oLinqEmpresa.ID_Empresa = 0 Then
                    If oDTC.Empresa.Where(Function(f) f.Predeterminada = True).Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible añadir, ya hay una empresa predeterminada", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                        Exit Function
                    End If
                Else
                    If oDTC.Empresa.Where(Function(f) f.Predeterminada = True And f.ID_Empresa <> oLinqEmpresa.ID_Empresa).Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible modificar, ya hay una empresa predeterminada", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                        Exit Function
                    End If
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqEmpresa)

            If oLinqEmpresa.ID_Empresa = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Empresa.InsertOnSubmit(oLinqEmpresa)
                oDTC.SubmitChanges()
                Call Fichero.Cargar_GRID(oLinqEmpresa.ID_Empresa) 'Fem això pq la classe tingui el ID de pressupost
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                'Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeGuardar)
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
            With oLinqEmpresa
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

                Me.R_Observaciones.pText = .Observaciones
                Me.T_URLAcceso.Text = .URLAcceso
                Me.T_Usuario.Text = .Usuario
                Me.T_Contraseña.Text = .Contraseña
                If .Logo Is Nothing = False Then
                    Me.PictureEdit1.Image = Util.BinaryToImage(.Logo.ToArray)
                Else
                    Me.PictureEdit1.Image = Nothing
                End If

                Me.CH_Predeterminada.Checked = .Predeterminada

                Me.T_NumeracionFacturaVenta.Value = .NumeracionFacturaVenta
                Me.T_NumeracionFacturaCompra.Value = .NumeracionFacturaCompra
                Me.T_NumeracionFacturaVentaRectificativa.Value = .NumeracionFacturaVentaRectificativa
                Me.T_NumeracionFacturaCompraRectificativa.Value = .NumeracionFacturaCompraRectificativa

                If .ColorEmpresa.HasValue = True Then
                    Me.UltraColorPicker1.Value = Color.FromArgb(.ColorEmpresa)
                End If
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pEmpresa As Empresa)
        Try
            With pEmpresa
                .Activo = True

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
                .FechaAlta = Me.DT_Alta.Value
                .FechaBaja = Me.DT_Baja.Value
                .Observaciones = Me.R_Observaciones.pText
                .URLAcceso = Me.T_URLAcceso.Text
                .Usuario = Me.T_Usuario.Text
                .Contraseña = Me.T_Contraseña.Text
                If Me.PictureEdit1.Image Is Nothing Then
                    .Logo = Nothing
                Else
                    .Logo = Util.ImageToBinary(Me.PictureEdit1.Image).ToArray
                End If

                .Predeterminada = Me.CH_Predeterminada.Checked

                .NumeracionFacturaVenta = Me.T_NumeracionFacturaVenta.Value
                .NumeracionFacturaCompra = Me.T_NumeracionFacturaCompra.Value
                .NumeracionFacturaVentaRectificativa = Me.T_NumeracionFacturaVentaRectificativa.Value
                .NumeracionFacturaCompraRectificativa = Me.T_NumeracionFacturaCompraRectificativa.Value

                .ColorEmpresa = Me.UltraColorPicker1.Color.ToArgb
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()

            oLinqEmpresa = (From taula In oDTC.Empresa Where taula.ID_Empresa = pID Select taula).FirstOrDefault

            Call SetToForm()

            Call CargaGrid_Festivos(pID, Now.Date.Year)

            Call CargaGrid_Delegaciones(pID)
            Call CargaGrid_CuentasBancarias(pID)
            Fichero.Cargar_GRID(pID)

            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Me.EstableixCaptionForm("Empresa: " & (oLinqEmpresa.Nombre))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqEmpresa = New Empresa
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing

        Me.DT_Alta.Value = Now.Date
        Me.DT_Baja.Value = Nothing

        Call CargaGrid_Festivos(0, Now.Date.Year)
        Call CargaGrid_Delegaciones(0)
        Call CargaGrid_CuentasBancarias(0)

        Fichero.Cargar_GRID(0)

        ' Me.C_OrigenCliente.Value = 1
        ' Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Instalacion")

        Me.PictureEdit1.Image = Nothing

        Me.TAB_Principal.Tabs("General").Selected = True
        ErrorProvider1.Clear()

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.T_Nombre)
                oClsControls.ControlBuit(.T_NumeracionFacturaCompra)
                oClsControls.ControlBuit(.T_NumeracionFacturaCompraRectificativa)
                oClsControls.ControlBuit(.T_NumeracionFacturaVenta)
                oClsControls.ControlBuit(.T_NumeracionFacturaVentaRectificativa)
                'oClsControls.ControlBuit(.T_Persona_Contacto)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    'Private Sub Cridar_Llistat_Generic()
    '    Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
    '    LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Cliente Where Activo=1 ORDER BY Nombre", Me.TE_Codigo, "ID_Cliente", "ID_Cliente")
    '    AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat

    '    'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
    '    'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
    '    '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
    '    '        pRow.Appearance.BackColor = Color.LightCoral
    '    '    End If
    '    'Next
    'End Sub

    Private Sub AlTancarLlistat(ByVal pID As String)
        Try
            Call Cargar_Form(pID)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cridar_Llistat_Generic()

        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM Empresa  ORDER BY ID_Empresa", Me.TE_Codigo, "ID_Empresa", "ID_Empresa")
        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat

    End Sub

   
#End Region

#Region "Events Varis"

    Private Sub C_Año_ValueChanged(sender As Object, e As EventArgs) Handles C_Año.ValueChanged
        If Me.C_Año.SelectedIndex <> -1 And oLinqEmpresa Is Nothing = False Then
            Call CargaGrid_Festivos(oLinqEmpresa.ID_Empresa, Me.C_Año.Value)
        End If
    End Sub

    'Private Sub TE_Codigo_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
    '    If e.Button.Key = "Lupeta" Then
    '        Call Cridar_Llistat_Generic()
    '    End If
    'End Sub

    'Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown

    '    If e.KeyCode = Keys.Enter Then
    '        If Me.TE_Codigo.Value Is Nothing = False AndAlso IsDBNull(Me.TE_Codigo.Value) = False Then
    '            Dim ooLinqCliente As Cliente = oDTC.Cliente.Where(Function(F) F.Codigo = CInt(Me.TE_Codigo.Value)).FirstOrDefault()
    '            If ooLinqCliente Is Nothing = False Then
    '                Call Cargar_Form(ooLinqCliente.ID_Cliente)
    '            Else
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
    '                Call Netejar_Pantalla()
    '            End If
    '        Else
    '            Me.TE_Codigo.Value = oDTC.Cliente.Where(Function(F) F.Activo = True).Max(Function(F) F.Codigo) + 1
    '            Exit Sub
    '        End If
    '    End If
    'End Sub

    Private Sub TE_Codigo_EditorButtonClick(sender As Object, e As UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Call Cridar_Llistat_Generic()
        End If
        If e.Button.Key = "Cancelar" Then
            Call Netejar_Pantalla()
        End If
    End Sub
#End Region

#Region "Grid Festivos"

    Private Sub CargaGrid_Festivos(ByVal pId As Integer, ByVal pAño As Integer)
        Try
            With Me.GRD_Festivos
                Dim Festivos As IEnumerable(Of Empresa_FechasNoLaborables) = From taula In oDTC.Empresa_FechasNoLaborables Where taula.ID_Empresa = pId And taula.Fecha.Year = pAño Order By taula.Fecha Select taula

                .M.clsUltraGrid.CargarIEnumerable(Festivos)
                '.GRID.DataSource = Festivos

                .M_Editable()
                .M_TreureFocus()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Festivos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Festivos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Festivos

                If oLinqEmpresa.ID_Empresa = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Fecha").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Empresa").Value = oLinqEmpresa

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Festivos_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Festivos.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqEmpresa.Empresa_FechasNoLaborables.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim _clsNewAnotacio As New clsAnotacioCalendariOperaris(oDTC)
                _clsNewAnotacio.EliminarAnotacioDiaNoLaboral(e.ListObject)
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contacto_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Festivos.M_GRID_AfterRowUpdate
        Dim _clsNewAnotacio As New clsAnotacioCalendariOperaris(oDTC)
        If e.Row.Cells("ID_Empresa_FechasNoLaborables").Value = 0 Then
            _clsNewAnotacio.CrearAnotacioDiaNoLaboral(e.Row.Cells("Fecha").Value, e.Row.ListObject)
        Else
            _clsNewAnotacio.ModificarAnotacioDiaNoLaboral(e.Row.ListObject)
        End If
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Delegaciones"

    Private Sub CargaGrid_Delegaciones(ByVal pId As Integer)
        Try
            Dim _Delegaciones As IEnumerable(Of Delegacion) = From taula In oDTC.Delegacion Select taula

            With Me.GRD_Delegaciones
                .GRID.DisplayLayout.MaxBandDepth = 1
                .M.clsUltraGrid.CargarIEnumerable(_Delegaciones)
                .M_Editable()
                Call CargarCombo_Paises(.GRID)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Delegaciones_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Delegaciones.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Delegaciones

                'If oLinqEntrada.ID_Entrada = 0 Then
                '    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                '    Exit Sub
                'End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Pais").Value = oDTC.Pais.FirstOrDefault

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Delegaciones_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Delegaciones.M_ToolGrid_ToolEliminarRow
        Try

            'If e.IsAddRow Then
            'oLinqEntrada.Entrada_Vencimiento.Remove(e.ListObject)
            '   Exit Sub
            'End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            'If e.IsAddRow = False Then
            '    'oLinqEntrada.Entrada_Vencimiento.Remove(e.ListObject)
            '    '   Exit Sub
            'End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Delegaciones_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Delegaciones.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Paises(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Pais) = (From Taula In oDTC.Pais Order By Taula.Nombre Select Taula)
            Dim Var As Pais

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Pais").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Pais").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Cuentas bancarias"

    Private Sub CargaGrid_CuentasBancarias(ByVal pId As Integer)
        Try
            With Me.GRD_CuentasBancarias
                Dim _Cuentas As IEnumerable(Of Empresa_CuentaBancaria) = From taula In oDTC.Empresa_CuentaBancaria Where taula.ID_Empresa = pId Select taula

                '.GRID.DataSource = Contactes
                .GRID.DisplayLayout.MaxBandDepth = 1
                .M.clsUltraGrid.CargarIEnumerable(_Cuentas)

                .M_Editable()
                .M_TreureFocus()

                Dim _pRow As UltraGridRow
                For Each _pRow In .GRID.Rows
                    Dim _CuentaBancaria As Empresa_CuentaBancaria = _pRow.ListObject
                    If _CuentaBancaria.Entrada_Vencimiento.Count > 0 Then
                        _pRow.Cells("NombreBanco").Activation = Activation.Disabled
                        _pRow.Cells("NumeroCuenta").Activation = Activation.Disabled
                        _pRow.Cells("BIC").Activation = Activation.Disabled
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

                If oLinqEmpresa.ID_Empresa = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Empresa").Value = oLinqEmpresa
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CuentasBancarias_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_CuentasBancarias.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqEmpresa.Empresa_CuentaBancaria.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _CuentaBancaria As Empresa_CuentaBancaria = e.ListObject
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

#End Region


End Class