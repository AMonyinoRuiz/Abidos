Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEntrada_Linea_CompuestoPor
    Dim oDTC As DTCDataContext
    Dim oclsEntradaLineaPare As clsEntradaLinea
    Dim oclsEntradaLinea As clsEntradaLinea
    Dim RowsSeleccionadas As New ArrayList

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Try

            Util.WaitFormObrir()
            If Guardar() = True Then
                'Mensaje.Mostrar_Mensaje(" traspasada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                Call M_ToolForm1_m_ToolForm_Sortir()
            End If
            Util.WaitFormTancar()


        Catch ex As Exception
            Util.WaitFormTancar()
            Mensaje.Mostrar_Mensaje_Error(ex)

        End Try
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pclsEntradaLineaPare As clsEntradaLinea, ByRef pDTC As DTCDataContext, Optional ByVal pLinea As Entrada_Linea = Nothing)

        Try

            Me.AplicarDisseny()

            oclsEntradaLineaPare = pclsEntradaLineaPare

            oclsEntradaLinea = New clsEntradaLinea(pclsEntradaLineaPare.oLinqEntrada, pLinea, pDTC)

            oDTC = pDTC
            'oDTC = New DTCDataContext(BD.Conexion)


            ''Cargar en rowsseleccionadas els id propuesta ja assignats
            'Dim _Relacio As Entrada_Linea_Propuesta_Linea
            'For Each _Relacio In oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea
            '    RowsSeleccionadas.Add(_Relacio.ID_Propuesta_Linea)
            'Next

            Me.GRD_NS.M.clsToolBar.Boto_Afegir("SeleccionarTodo", "Seleccionar todos", True)

            If pLinea Is Nothing = False Then
                Call Cargar_Form()

            End If

            ' Me.GRD_Lineas.GRID.ActiveRow = Nothing
            ' Me.GRD_Lineas.GRID.Selected.Rows.Clear()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Private Sub Cargar_Form()
        Try
            Call SetToForm()
            Call CargaGrid_NS(Me.TE_Producto_Codigo.Tag)
            Call CargaGrid_NSSeleccionats()

            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.TE_Producto_Codigo.Focus()



            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            If Me.T_Unidades.Value Is Nothing = True OrElse IsNumeric(Me.T_Unidades.Value) = False OrElse Me.T_Unidades.Value = 0 Then
                Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
                If _Producto.RequiereNumeroSerie = False Then
                    Mensaje.Mostrar_Mensaje("Imposible guardar los datos, la cantidad no puede ser 0", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
            End If

            Call GetFromForm()

            If oclsEntradaLinea.EsUnaLineaNova Then
                If oclsEntradaLinea.AfegirLinea(True) = True Then
                    oDTC.SubmitChanges()
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                End If

            Else
                If oclsEntradaLinea.ModificaLinea Then
                    oDTC.SubmitChanges()
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
                End If

            End If

            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Producto_Codigo)
                oClsControls.ControlBuit(.C_Almacen)

                'oClsControls.ControlBuit(.T_Unidades)

                'oClsControls.ControlBuit(.T_Persona_Contacto)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oclsEntradaLinea.oLinqLinea
                Me.TE_Producto_Codigo.Tag = .ID_Producto
                Me.TE_Producto_Codigo.Value = .Producto.Codigo
                Me.T_Producto_Descripcion.Text = .Producto.Descripcion
                Me.T_Unidades.Value = .Unidad
                Me.C_Almacen.Value = .ID_Almacen

                Call CarregarProducto()
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm() 'ByRef pEntrada_Linea As Entrada_Linea
        Try

            With oclsEntradaLinea.oLinqLinea

                .Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.C_Almacen.Value)).FirstOrDefault

                If oclsEntradaLinea.EsUnaLineaNova Then 'Si la línea es nova guardarem la data de creació
                    .FechaEntrada = oclsEntradaLineaPare.oLinqLinea.FechaEntrada
                    .FechaEntrega = oclsEntradaLineaPare.oLinqLinea.FechaEntrega
                End If

                If T_Unidades.Value Is Nothing OrElse IsDBNull(T_Unidades.Value) Then
                    Me.T_Unidades.Value = 0
                End If
                .Unidad = Me.T_Unidades.Value
                .Descripcion = Me.T_Producto_Descripcion.Text

                If IsNumeric(Me.TE_Producto_Codigo.Tag) = True Then
                    .Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
                Else
                    .Producto = Nothing
                End If
                .ID_Entrada_Linea_Padre = oclsEntradaLineaPare.oLinqLinea.ID_Entrada_Linea


                ' .Entrada_Linea_Padre = oclsEntradaLineaPare.oLinqLinea

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Assignar()


        Dim _IDLineaPropuesta As Integer
        'Aquest bucle es per no traspasar les línies que no estan seleccionades

        oDTC.Entrada_Linea_Propuesta_Linea.DeleteAllOnSubmit(oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea)

        For Each _IDLineaPropuesta In RowsSeleccionadas
            'If oclsEntradaLinea.oLinqLinea.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(pRow.Cells("ID_Propuesta_Linea").Value)).Count = 0 Then
            Dim _Relacio As New Entrada_Linea_Propuesta_Linea
            _Relacio.Entrada_Linea = oclsEntradaLinea.oLinqLinea
            _Relacio.Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _IDLineaPropuesta).FirstOrDefault
            oDTC.Entrada_Linea_Propuesta_Linea.InsertOnSubmit(_Relacio)
            'End If
        Next


        ''For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso <> EnumProductoSubFamiliaTraspaso.No).OrderBy(Function(F) F.ID_Propuesta_Linea_Vinculado)
        'If _Linea.Unidad > 0 Then
        '    For i = 1 To _Linea.Unidad
        '        Dim DescartarLinea As Boolean = False
        '        Dim _NewLinea As New Propuesta_Linea
        '        With _NewLinea
        '            .Descripcion = _Linea.Descripcion
        '            .ID_Instalacion_ElementosAProteger = _Linea.ID_Instalacion_ElementosAProteger
        '            .ID_Instalacion_Emplazamiento = _Linea.ID_Instalacion_Emplazamiento
        '            .ID_Instalacion_Emplazamiento_Abertura = _Linea.ID_Instalacion_Emplazamiento_Abertura
        '            .ID_Instalacion_Emplazamiento_Planta = _Linea.ID_Instalacion_Emplazamiento_Planta
        '            .ID_Instalacion_Emplazamiento_Zona = _Linea.ID_Instalacion_Emplazamiento_Zona
        '            .ID_Producto = _Linea.ID_Producto
        '            .ID_Propuesta = pPropuestaSeInstalo.ID_Propuesta

        '            If _Linea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
        '                'El If de baix es per comprovar que la línea s'ha traspasat. Pq potser que el pare tingues la marca de família que no s'ha de traspasar i el fill llavors no s'ha de vincular
        '                If IsNothing(pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault) = True Then
        '                    ' DescartarLinea = True
        '                Else
        '                    .ID_Propuesta_Linea_Vinculado = pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault.ID_Propuesta_Linea
        '                End If
        '            End If

        '            .Identificador = _Linea.Identificador
        '            .ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea
        '            .ID_Propuesta_Antigua = _Linea.ID_Propuesta
        '            .Precio = 0
        '            .Descuento = 0
        '            .IVA = 0
        '            .Unidad = 1
        '            .Activo = True
        '            .ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Traspasada
        '            .Uso = _Linea.Uso


        '            .NumZona = _Linea.NumZona
        '            .BocaConexion = _Linea.BocaConexion
        '            '.CantidadPendienteRecibir = _Linea.CantidadPendienteRecibir
        '            .DescripcionAmpliada = _Linea.DescripcionAmpliada
        '            .DetalleInstalacion = _Linea.DetalleInstalacion
        '            '.FechaPrevista = _Linea.FechaPrevista
        '            .IdentificadorDelProducto = _Linea.IdentificadorDelProducto
        '            .NickZona = _Linea.NickZona
        '            .NumSerie = _Linea.NumSerie
        '            .Particion = _Linea.Particion
        '            .PrecioCoste = _Linea.PrecioCoste
        '            .RutaOrden = _Linea.RutaOrden
        '            .RutaParametros = _Linea.RutaParametros
        '            .ID_Propuesta_Linea_TipoZona = _Linea.ID_Propuesta_Linea_TipoZona
        '            .ATenerEnCuenta = _Linea.ATenerEnCuenta
        '            .Fase = _Linea.Fase
        '            .ReferenciaMemoria = _Linea.ReferenciaMemoria

        '            Dim _DatoAcceso As Propuesta_Linea_Acceso
        '            For Each _DatoAcceso In _Linea.Propuesta_Linea_Acceso
        '                Dim _NewDatoAcceso As New Propuesta_Linea_Acceso
        '                With _NewDatoAcceso
        '                    .Contraseña = _DatoAcceso.Contraseña
        '                    .Detalle = _DatoAcceso.Detalle
        '                    .Explicación = _DatoAcceso.Explicación
        '                    .Propuesta_Linea_TipoAcceso = _DatoAcceso.Propuesta_Linea_TipoAcceso
        '                    .Usuario = _DatoAcceso.Usuario
        '                    .ValorCRA = _DatoAcceso.ValorCRA
        '                End With
        '                .Propuesta_Linea_Acceso.Add(_NewDatoAcceso)
        '            Next
        '        End With
        '        pPropuestaSeInstalo.Propuesta_Linea.Add(_NewLinea)
        '        oDTC.SubmitChanges()
        '    Next
        'End If
        oDTC.SubmitChanges()
    End Sub

    Private Sub AlTancarLlistatGeneric(ByVal pID As String)
        If pID Is Nothing Then
        Else
            Call CarregarProducto()
        End If

    End Sub

#End Region

#Region "Events varis"

    Private Sub TE_Producto_Codigo_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Producto_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto ORDER BY Descripcion", Me.TE_Producto_Codigo, "ID_Producto", "Codigo", Me.T_Producto_Descripcion, "Descripcion")
            AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGeneric


            'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
            'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
            '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
            '        pRow.Appearance.BackColor = Color.LightCoral
            '    End If
            'Next

        End If
    End Sub

    Private Sub TE_Producto_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Producto_Codigo.KeyDown
        If Me.TE_Producto_Codigo.Text Is Nothing = False Then
            If e.KeyCode = Keys.Enter Then
                Call CarregarProducto()
            End If
        End If
    End Sub

    Private Sub CarregarProducto()
        Dim ooLinqProducto As Producto = oDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Producto_Codigo.Text).FirstOrDefault()
        If ooLinqProducto Is Nothing = False Then
            Me.TE_Producto_Codigo.Tag = ooLinqProducto.ID_Producto
            Me.T_Producto_Descripcion.Text = ooLinqProducto.Descripcion
            If ooLinqProducto.RequiereNumeroSerie = True Then
                Me.T_Unidades.Enabled = False
                Me.B_Asignar.Enabled = True
                Me.B_Desasignar.Enabled = True
                Me.B_CarregarNS.Enabled = True
                Me.L_Producto_ConNS.Visible = True
            Else
                Me.T_Unidades.Enabled = True
                Me.B_Asignar.Enabled = False
                Me.B_Desasignar.Enabled = False
                Me.B_CarregarNS.Enabled = False
                Me.L_Producto_ConNS.Visible = False
            End If
            ' Me.TE_Producto_Codigo.Enabled = False
            Me.TE_Producto_Codigo.ReadOnly = True
            Me.TE_Producto_Codigo.ButtonsRight("Lupeta").Enabled = False

            Util.Cargar_Combo(Me.C_Almacen, "Select ID_Almacen, almacenDescripcion + ' (' + Cast(StockReal as nvarchar(10)) + ' )'  From RetornaStock(" & Me.TE_Producto_Codigo.Tag & ",0) Where StockReal>0 ORDER BY almacenDescripcion", False)
            If Me.C_Almacen.Items.Count = 0 Then 'Si no hi ha cap magatzem que tingui stock llavors els carregarem tots
                Call CarregarTotsElsMagatzems()
            End If
        Else
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
            Me.TE_Producto_Codigo.Value = Nothing
        End If
    End Sub

    Private Sub B_CarregarNS_Click(sender As Object, e As EventArgs) Handles B_CarregarNS.Click
        Try
            'If Me.C_Almacen.SelectedIndex > -1 Then
            Call CargaGrid_NS(Me.TE_Producto_Codigo.Tag)
            Call CargaGrid_NSSeleccionats()
            'Else
            '    Mensaje.Mostrar_Mensaje("Imposible cargar los datos, es obligatorio seleccionar el almacén", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
            'End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid NS"

    Private Sub CargaGrid_NS(ByVal pId As Integer)
        Try

            With Me.GRD_NS

                .M.clsUltraGrid.Cargar(oclsEntradaLinea.RetornaNSDisponiblesDunArticle(pId))

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid NS Seleccionats"

    Private Sub CargaGrid_NSSeleccionats()
        Try

            With Me.GRD_NS_Seleccionats

                .M.clsUltraGrid.Cargar(oclsEntradaLinea.RetornaNSDeLaLinea)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

    Private Sub B_Asignar_Click(sender As Object, e As EventArgs) Handles B_Asignar.Click
        If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea = 0 Then
            If Guardar() = False Then
                Exit Sub
            End If
        End If

        If Me.GRD_NS.GRID.ActiveRow Is Nothing Then
            Exit Sub
        End If

        oclsEntradaLinea.LineaNSCrear(Me.GRD_NS.GRID.ActiveRow.Cells("ID_NS").Value)
        oDTC.SubmitChanges()

        Call CargaGrid_NS(Me.TE_Producto_Codigo.Tag)
        Call CargaGrid_NSSeleccionats()
        Me.T_Unidades.Value = Me.GRD_NS_Seleccionats.GRID.Rows.Count
    End Sub

    Private Sub B_Desasignar_Click(sender As Object, e As EventArgs) Handles B_Desasignar.Click
        If Me.GRD_NS_Seleccionats.GRID.ActiveRow Is Nothing Then
            Exit Sub
        End If

        Dim _LineaNS As Entrada_Linea_NS = oDTC.Entrada_Linea_NS.Where(Function(F) F.ID_Entrada_Linea_NS = CInt(Me.GRD_NS_Seleccionats.GRID.ActiveRow.Cells("ID_Entrada_Linea_NS").Value)).FirstOrDefault

        oclsEntradaLinea.LineaNSEliminar(_LineaNS)
        oDTC.SubmitChanges()

        Call CargaGrid_NS(Me.TE_Producto_Codigo.Tag)
        Call CargaGrid_NSSeleccionats()
        Me.T_Unidades.Value = Me.GRD_NS_Seleccionats.GRID.Rows.Count
    End Sub

    Private Sub C_Almacen_EditorButtonClick(sender As Object, e As UltraWinEditors.EditorButtonEventArgs) Handles C_Almacen.EditorButtonClick
        If e.Button.Key = "Todos" Then
            Call CarregarTotsElsMagatzems()
        End If
    End Sub

    Private Sub CarregarTotsElsMagatzems()
        Util.Cargar_Combo(Me.C_Almacen, "SELECT ID_Almacen, Descripcion FROM Almacen Where Activo=1 ORDER BY Descripcion", False)
    End Sub

    Private Sub TE_Producto_Codigo_ValueChanged(sender As Object, e As EventArgs) Handles TE_Producto_Codigo.ValueChanged

    End Sub
End Class