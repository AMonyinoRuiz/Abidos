Imports Infragistics.Win.UltraWinGrid

Public Class frmAutomatismo
    Dim oDTC As DTCDataContext
    Dim oLinqAutomatismo As Automatismo


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

#End Region

#Region "Métodes"
    Public Sub Entrada(Optional ByVal pID As Integer = 0)

        Me.AplicarDisseny()

        oDTC = New DTCDataContext(BD.Conexion)

        Util.Cargar_Combo(Me.C_TipoActividad, "Select ID_ActividadCRM_Tipo, Descripcion From ActividadCRM_Tipo Where Activo=1 Order by Codigo", False)
        Util.Cargar_Combo(Me.C_Prioridad, "Select ID_Prioridad, Descripcion From Prioridad Order by ID_Prioridad", False)

        If pID <> 0 Then
            Call Cargar_Form(pID)
        Else
            Call Netejar_Pantalla()
        End If

        Me.KeyPreview = False
        Me.UltraTabControl1.Tabs("Acciones").Selected = True
    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.TE_Codigo.Focus()

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqAutomatismo)

            If oLinqAutomatismo.ID_Automatismo = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                'oDTC.ActividadCRM.InsertOnSubmit(oLinqActividad)


                'oDTC.SubmitChanges()
                'Me.TE_Codigo.Value = oLinqActividad.ID_ActividadCRM
                'Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                'Call Fichero.Cargar_GRID(oLinqActividad.ID_ActividadCRM) 'Fem això pq la classe tingui el ID de pressupost
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If



            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Netejar_Pantalla()
        oLinqAutomatismo = New Automatismo
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.TE_Codigo.Value = Nothing

        Me.C_Prioridad.Value = Nothing

        Me.UltraTabControl1.Tabs("Acciones").Selected = True

        Call CargaGrid_Accions(0)
        Call CargaGrid_Personal(0)

        '  Fichero.Cargar_GRID(0)

        Me.EstableixCaptionForm("Automatismo")

        ' Me.Excel1.M_NewDocument()

        ErrorProvider1.Clear()
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqAutomatismo = (From taula In oDTC.Automatismo Where taula.ID_Automatismo = pID Select taula).First

            Call SetToForm()

            Call CargaGrid_Accions(pID)
            Call CargaGrid_Personal(pID)

            'Fichero.Cargar_GRID(pID)

            Me.EstableixCaptionForm("Automatismo actividad: " & (oLinqAutomatismo.ID_Automatismo))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub SetToForm()
        Try

            With oLinqAutomatismo

                Me.TE_Codigo.Value = .ID_Automatismo
                Me.T_Descripcion.Text = .Descripcion
                Me.C_TipoActividad.Value = .ID_ActividadCRM_Tipo
                Me.C_Prioridad.Value = .ID_Prioridad
                Me.R_Explicacion.pText = .Explicacion

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pAutomatismo As Automatismo)
        Try

            With pAutomatismo

                .Descripcion = Me.T_Descripcion.Text
                .Explicacion = Me.R_Explicacion.pText

                .ActividadCRM_Tipo = oDTC.ActividadCRM_Tipo.Where(Function(F) F.ID_ActividadCRM_Tipo = CInt(Me.C_TipoActividad.Value)).FirstOrDefault
                .Prioridad = oDTC.Prioridad.Where(Function(F) F.ID_Prioridad = CInt(Me.C_Prioridad.Value)).FirstOrDefault

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.C_Prioridad)
                oClsControls.ControlBuit(.C_TipoActividad)
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

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric

        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Automatismo Where Activo=1  ORDER BY ID_Automatismo ", Me.TE_Codigo, "ID_Automatismo", "ID_Automatismo")

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
#End Region

#Region "Events"
    Private Sub TE_Codigo_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Call Cridar_Llistat_Generic()
        End If
    End Sub

#End Region

#Region "Grid Accions"
    Private Sub CargaGrid_Accions(ByVal pId As Integer)
        Try
            With Me.GRD_Acciones
                Dim DT As New DataTable
                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_Automatismo_Accion Where ID_Automatismo=" & oLinqAutomatismo.ID_Automatismo & " Order By ID_Automatismo ", True))

                .GRID.ActiveRow = Nothing
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Acciones_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Acciones.M_ToolGrid_ToolAfegir
        Try
            'If oLinqAutomatismo.ID_Automatismo = 0 Then
            '    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            '    Exit Sub
            'End If

            If oLinqAutomatismo.ID_Automatismo = 0 AndAlso Guardar() = False Then
                Exit Sub
            End If

            Dim frm As New frmAutomatismo_Accion
            frm.Entrada(oLinqAutomatismo, oDTC)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmAutomatismo_Accion
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Acciones_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Acciones.M_ToolGrid_ToolEditar
        Call GRD_Acciones_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub GRD_Acciones_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Acciones.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_Acciones.GRID.Selected.Cells.Count = 0 And Me.GRD_Acciones.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then

                Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                If Me.GRD_Acciones.GRID.Selected.Cells.Count > 0 Then
                    pRow = Me.GRD_Acciones.GRID.Selected.Cells(0).Row
                Else
                    pRow = Me.GRD_Acciones.GRID.Selected.Rows(0)
                End If

                Dim _Accion As Automatismo_Accion = oDTC.Automatismo_Accion.Where(Function(F) F.ID_Automatismo_Accion = CInt(pRow.Cells("ID_Automatismo_Accion").Value)).FirstOrDefault

                oDTC.Automatismo_Accion_Personal.DeleteAllOnSubmit(_Accion.Automatismo_Accion_Personal)
                oDTC.Automatismo_Accion.DeleteOnSubmit(_Accion)

                oDTC.SubmitChanges()

                ' Call CargaGrid_Accions(oLinqAutomatismo.ID_Automatismo)

                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)

                pRow.Hidden = True
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Acciones_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Acciones.M_ToolGrid_ToolVisualitzarDobleClickRow
        'If oLinqAutomatismo.ID_Automatismo = 0 Then
        '    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
        '    Exit Sub
        'End If

        If Me.GRD_Acciones.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        'If Guardar(True) = False Then
        '    Exit Sub
        'End If

        Dim frm As New frmAutomatismo_Accion
        frm.Entrada(oLinqAutomatismo, oDTC, Me.GRD_Acciones.GRID.Selected.Rows(0).Cells("ID_Automatismo_Accion").Value)
        AddHandler frm.FormClosing, AddressOf AlTancarfrmAutomatismo_Accion
        frm.FormObrir(Me)
    End Sub

    Private Sub AlTancarfrmAutomatismo_Accion()
        Call CargaGrid_Accions(oLinqAutomatismo.ID_Automatismo)
    End Sub
#End Region

#Region "Grid Personal"

    Private Sub CargaGrid_Personal(ByVal pId As Integer)
        Try
            Dim _Asignacion As IEnumerable(Of Automatismo_Personal) = From taula In oDTC.Automatismo_Personal Where taula.ID_Automatismo = pId Select taula

            With Me.GRD_Personal

                .M.clsUltraGrid.CargarIEnumerable(_Asignacion)

                'If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                .M_Editable()
                'Else
                '.GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                'End If

                Call CargarCombo_Personal(.GRID)

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
            If oLinqAutomatismo.Automatismo_Personal.Where(Function(F) F.Personal Is _Personal).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar la misma persona dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                e.Cell.CancelUpdate()
                Exit Sub
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Personal(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal) = (From Taula In oDTC.Personal Where Taula.Activo = True And Taula.FechaBajaEmpresa.HasValue = False Order By Taula.Nombre Select Taula)

            Dim Var As Personal

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Personal_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Personal.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Personal


                If oLinqAutomatismo.ID_Automatismo = 0 AndAlso Guardar() = False Then
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Automatismo").Value = oLinqAutomatismo

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Personal_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Personal.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqAutomatismo.Automatismo_Personal.Remove(e.ListObject)
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

    Private Sub GRD_Personal_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Personal.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

End Class