Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmParte_TrabajosARealizar
    Dim oDTC As DTCDataContext
    Dim oLinqTrabajosARealizar As Parte_TrabajosARealizar
    Dim oLinqParte As Parte

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If Guardar() = True Then
            Call M_ToolForm1_m_ToolForm_Sortir()
        End If
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_ToolClick_Botones_Extras(Sender As Object, e As UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        Select Case e.Tool.Key
            Case "GuardarYContinuar"
                If Guardar() = True Then
                    Call Netejar_Pantalla()
                End If
            Case "GuardarYMantener"
                If Guardar() = True Then

                    Dim _LinQAuxTrabajos As Parte_TrabajosARealizar
                    _LinQAuxTrabajos = oLinqTrabajosARealizar
                    oLinqTrabajosARealizar = New Parte_TrabajosARealizar
                    Call Guardar()
                    Dim _personal As Parte_TrabajosARealizar_Personal
                    For Each _personal In _LinQAuxTrabajos.Parte_TrabajosARealizar_Personal
                        Dim _NewPersonal As New Parte_TrabajosARealizar_Personal
                        _NewPersonal.FechaFinalizacion = Nothing
                        _NewPersonal.Finalizada = False
                        _NewPersonal.Personal = _personal.Personal
                        oLinqTrabajosARealizar.Parte_TrabajosARealizar_Personal.Add(_NewPersonal)
                        oDTC.Parte_TrabajosARealizar_Personal.InsertOnSubmit(_NewPersonal)
                    Next
                    oDTC.SubmitChanges()

                    Dim _Producto As Parte_TrabajosARealizar_Producto
                    For Each _Producto In _LinQAuxTrabajos.Parte_TrabajosARealizar_Producto
                        Dim _NewProducto As New Parte_TrabajosARealizar_Producto
                        _NewProducto.Producto = _Producto.Producto
                        _NewProducto.Cantidad = _Producto.Cantidad
                        oLinqTrabajosARealizar.Parte_TrabajosARealizar_Producto.Add(_NewProducto)
                        oDTC.Parte_TrabajosARealizar_Producto.InsertOnSubmit(_NewProducto)
                    Next
                    oDTC.SubmitChanges()
                End If
                Call CargaGrid_Personal(oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar)
                Call CargaGrid_Productos(oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar)
        End Select
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pParte As Parte, ByRef pDTC As DTCDataContext, Optional ByVal pId As Integer = 0)

        Try

            Me.AplicarDisseny()

            oLinqParte = pParte

            oDTC = pDTC

            '            Util.Cargar_Combo(Me.C_Receptora, "SELECT ID_Receptora, Nombre FROM Receptora Where Activo=1 ORDER BY Codigo", False)

            Dim BotoCancelar As UltraWinEditors.EditorButton
            BotoCancelar = New UltraWinEditors.EditorButton
            BotoCancelar.Key = "Cancelar"
            Dim oDisseny As New M_Disseny.ClsDisseny
            BotoCancelar.Appearance.Image = oDisseny.Leer_Imagen("text_cancelar.gif")
            BotoCancelar.Width = 16
            BotoCancelar.Appearance.BackColor = Color.White
            BotoCancelar.Appearance.BorderAlpha = Alpha.Transparent

            Me.TE_TrabajoARealizarPrimero.ButtonsRight.Add(BotoCancelar.Clone)
            Me.GRD_Personal.M.clsToolBar.Boto_Afegir("DarPorNoFinalizado", "Dar por NO finalizado", True)

            Me.ToolForm.M.clsToolBar.Afegir_Boto("GuardarYContinuar", "Guardar y continuar", True)
            Me.ToolForm.M.clsToolBar.Afegir_Boto("GuardarYMantener", "Guardar y mantener", True)

            Call Netejar_Pantalla()

            If pId <> 0 Then
                Call Cargar_Form(pId)
            End If

            Me.KeyPreview = False

            '            ' Me.GRD_Associacio.GRID.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqTrabajosARealizar)

            If oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar = 0 Then  ' Comprovacio per saber si es un insertar o un nou

                oLinqParte.Parte_TrabajosARealizar.Add(oLinqTrabajosARealizar)
                'oDTC.Instalacion_Receptora.InsertOnSubmit(oLinqReceptora)

                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
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
            With oLinqTrabajosARealizar
                Me.T_Codigo.Text = oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar
                Me.R_Descripcion_Detallada.pText = .Descripcion
                Me.T_DescripcionBreve.Text = .Titulo
                Me.T_DuracionAproximada.Value = DbnullToNothing(.DuracionAproximada)
                Me.T_NumDia.Value = DbnullToNothing(.NumDia)
                Me.T_Orden.Value = DbnullToNothing(.Orden)
                Me.T_Participantes.Value = DbnullToNothing(.Participantes)
                Me.CH_Finalizado.Checked = DbnullToNothing(.Realizada)
                Me.DT_Alta.Value = .FechaAlta
                Me.DT_FechaPrevision.Value = .FechaPrevision
                If .ID_Partes_TrabajosARealizar_Obligatorio.HasValue Then
                    Me.TE_TrabajoARealizarPrimero.Tag = .ID_Partes_TrabajosARealizar_Obligatorio
                    Me.TE_TrabajoARealizarPrimero.Text = .Parte_TrabajosARealizar.Titulo
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pTrabajoARealizar As Parte_TrabajosARealizar)
        Try
            With pTrabajoARealizar

                .Descripcion = Me.R_Descripcion_Detallada.pText
                .Titulo = Me.T_DescripcionBreve.Text
                .DuracionAproximada = DbnullToNothing(Me.T_DuracionAproximada.Value)
                .NumDia = CInt(Me.T_NumDia.Value)
                .Orden = CInt(Me.T_Orden.Value)
                .Participantes = CInt(Me.T_Participantes.Value)
                .Realizada = Me.CH_Finalizado.Checked
                .FechaAlta = Me.DT_Alta.Value
                .FechaPrevision = Me.DT_FechaPrevision.Value
                If Me.TE_TrabajoARealizarPrimero.Tag Is Nothing = False Then
                    .Parte_TrabajosARealizar = oDTC.Parte_TrabajosARealizar.Where(Function(F) F.ID_Parte_TrabajosARealizar = CInt(Me.TE_TrabajoARealizarPrimero.Tag)).FirstOrDefault
                Else
                    .Parte_TrabajosARealizar = Nothing
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqTrabajosARealizar = (From taula In oDTC.Parte_TrabajosARealizar Where taula.ID_Parte_TrabajosARealizar = pID Select taula).First
            Call SetToForm()
            Call CargaGrid_Personal(pID)
            Call CargaGrid_Productos(pID)



            'Call Carga_Grid_Caracteristicas_Personalizadas(pID)
            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqTrabajosARealizar = New Parte_TrabajosARealizar
        ' oDTC = New DTCDataContext(BD.Conexion)

        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
        Me.DT_Alta.Value = Now.Date
        Me.T_NumDia.Value = 0
        Me.TE_TrabajoARealizarPrimero.ButtonsRight("Lupeta").Enabled = True
        Me.TE_TrabajoARealizarPrimero.Tag = Nothing
        Me.T_Participantes.Value = 1
        Call CargaGrid_Personal(0)
        Call CargaGrid_Productos(0)

        ErrorProvider1.Clear()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.T_DescripcionBreve)
                oClsControls.ControlBuit(.T_NumDia)
                oClsControls.ControlBuit(.T_Participantes)
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

    Private Sub AlTancarLlistatGeneric(ByVal pID As String)
        If pID Is Nothing Then
        Else

        End If

    End Sub

#End Region

#Region "Events Varis"
    Private Sub TE_PropuestaLinea_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_TrabajoARealizarPrimero.EditorButtonClick
      Select e.Button.Key
            Case "Lupeta"
                Dim _SQLFiltre As String = ""
                If oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar <> 0 Then
                    _SQLFiltre = " ID_Parte_TrabajosARealizar <> " & oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar & " and "
                End If

                Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
                LlistatGeneric.Mostrar_Llistat("Select * From C_Parte_TrabajosARealizar Where " & _SQLFiltre & " ID_Parte=" & oLinqParte.ID_Parte, Me.TE_TrabajoARealizarPrimero, "ID_Parte_TrabajosARealizar", "Titulo")

            Case "Cancelar"
                Me.TE_TrabajoARealizarPrimero.Text = ""
                Me.TE_TrabajoARealizarPrimero.Tag = Nothing

        End Select



    End Sub

#End Region

#Region "Grid Personal"

    Private Sub CargaGrid_Personal(ByVal pId As Integer)
        Try
            Dim _Asignacion As IEnumerable(Of Parte_TrabajosARealizar_Personal) = From taula In oDTC.Parte_TrabajosARealizar_Personal Where taula.ID_Parte_TrabajosARealizar = pId Select taula

            With Me.GRD_Personal

                '.GRID.DataSource = _Asignacion
                .M.clsUltraGrid.CargarIEnumerable(_Asignacion)

                'If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                .M_Editable()
                'Else
                '.GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                'End If
                .GRID.DisplayLayout.Bands(0).Columns("FechaFinalizacion").CellActivation = Activation.Disabled
                .GRID.DisplayLayout.Bands(0).Columns("Finalizada").CellActivation = Activation.Disabled
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Personal_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Personal.M_GRID_CellListSelect
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
            If oLinqTrabajosARealizar.Parte_TrabajosARealizar_Personal.Where(Function(F) F.Personal Is _Personal).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar la misma persona dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                e.Cell.CancelUpdate()
                Exit Sub
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Personal(ByVal pGrid As UltraWinGrid.UltraGrid, ByRef pCell As UltraWinGrid.UltraGridCell)
        Try

            Dim oTaula As IQueryable(Of Personal)
            Dim Valors As New Infragistics.Win.ValueList
            Dim _Personal As Personal = pCell.Value

            Dim _PersonalAssignat As IEnumerable(Of Personal) = oDTC.Parte_Asignacion.Where(Function(F) F.ID_Parte = oLinqParte.ID_Parte).Select(Function(F) F.Personal)
            oTaula = (From Taula In oDTC.Personal Where _PersonalAssignat.Contains(Taula) Order By Taula.Nombre Select Taula)

            Dim Var As Personal

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pCell.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub GRD_Destinatarios_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Destinatarios.M_GRID_BeforeCellActivate
    '    If e.Cell.Column.Key = "Personal" Then
    '        Call CargarCombo_Personal(sender, e.Cell, SeleccionPersonal.TodoElPersonal)
    '    End If
    'End Sub

    Private Sub GRD_Personal_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Personal.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Personal

                If oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar = 0 AndAlso Guardar() = False Then
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte_TrabajosARealizar").Value = oLinqTrabajosARealizar

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Personal_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Personal.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqTrabajosARealizar.Parte_TrabajosARealizar_Personal.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim _AsignacionPersonal As ActividadCRM_Personal = e.ListObject
                'oDTC.ActividadCRM_Personal.DeleteOnSubmit(e.ListObject)
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Personal_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Personal.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Personal_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Personal.M_Grid_InitializeRow
        If e.ReInitialize = False Then
            Call CargarCombo_Personal(Sender, e.Row.Cells("Personal"))
        End If
    End Sub
    Private Sub GRD_Personal_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Personal.M_ToolGrid_ToolClickBotonsExtras2
        Try

            If Me.GRD_Personal.GRID.ActiveRow Is Nothing Then
                Exit Sub
            End If

            Dim _Personal As Parte_TrabajosARealizar_Personal
            _Personal = Me.GRD_Personal.GRID.ActiveRow.ListObject

            If _Personal.Finalizada Then
                _Personal.Finalizada = False
                _Personal.FechaFinalizacion = Nothing
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Productos"
    Private Sub CargaGrid_Productos(ByVal pId As Integer)
        Try
            With Me.GRD_Productos

                .M.clsUltraGrid.Cargar("Select * From C_Parte_TrabajosARealizar_Productos Where ID_Parte_TrabajosARealizar=" & oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar & "  Order by  ID_Parte_TrabajosARealizar_Producto", BD)
                .GRID.ActiveRow = Nothing
                .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Productos_Producto_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Productos.M_ToolGrid_ToolAfegir
        Try
            If oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar = 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If

            If Guardar() = False Then
                Exit Sub
            End If

            Dim frm As New frmParte_TrabajosARealizar_Producto
            frm.Entrada(oLinqTrabajosARealizar, oDTC)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmTrabajosARealizar_Producto
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Productos_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Productos.M_ToolGrid_ToolEditar
        Call GRD_Productos_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub GRD_Productos_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Productos.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_Productos.GRID.Selected.Cells.Count = 0 And Me.GRD_Productos.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then

                Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                If Me.GRD_Productos.GRID.Selected.Cells.Count > 0 Then
                    pRow = Me.GRD_Productos.GRID.Selected.Cells(0).Row
                Else
                    pRow = Me.GRD_Productos.GRID.Selected.Rows(0)
                End If

                Dim _ID As Integer = pRow.Cells("ID_Parte_TrabajosARealizar_Producto").Value
                Dim _Producto As Parte_TrabajosARealizar_Producto = oDTC.Parte_TrabajosARealizar_Producto.Where(Function(F) F.ID_Parte_TrabajosARealizar_Producto = _ID).FirstOrDefault
                oDTC.Parte_TrabajosARealizar_Producto.DeleteOnSubmit(_Producto)
                oDTC.SubmitChanges()

                Call CargaGrid_Productos(oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar)

                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                '_DTC.SubmitChanges()
                ' pRow.Hidden = True
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Productos_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Productos.M_ToolGrid_ToolVisualitzarDobleClickRow
        If oLinqParte.ID_Parte = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        If Me.GRD_Productos.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        If Me.GRD_Productos.GRID.ActiveRow Is Nothing Then
            Exit Sub
        End If

        If Guardar() = False Then
            Exit Sub
        End If

        Dim frm As New frmParte_TrabajosARealizar_Producto
        frm.Entrada(oLinqTrabajosARealizar, oDTC, Me.GRD_Productos.GRID.ActiveRow.Cells("ID_Parte_TrabajosARealizar_Producto").Value)
        AddHandler frm.FormClosing, AddressOf AlTancarfrmTrabajosARealizar_Producto
        frm.FormObrir(Me)
    End Sub

    Private Sub AlTancarfrmTrabajosARealizar_Producto()
        Call CargaGrid_Productos(oLinqTrabajosARealizar.ID_Parte_TrabajosARealizar)
    End Sub
#End Region


End Class