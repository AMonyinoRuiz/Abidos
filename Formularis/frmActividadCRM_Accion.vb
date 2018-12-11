Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmActividadCRM_Accion
    Dim oDTC As DTCDataContext
    Dim oLinqAccion As ActividadCRM_Accion
    Dim oLinqActividad As ActividadCRM
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2
    Dim oclsActividad As clsActividad

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If Guardar() = True Then
            Call M_ToolForm1_m_ToolForm_Sortir()
        End If
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pActividad As ActividadCRM, ByRef pDTC As DTCDataContext, ByRef pClsActividad As clsActividad, Optional ByVal pId As Integer = 0)

        Try

            Me.AplicarDisseny()

            oLinqActividad = pActividad
            oclsActividad = pClsActividad

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Instalacion_Receptora_Archivo", 1)

            oDTC = pDTC

            Util.Cargar_Combo(Me.C_Tipo, "SELECT ID_ActividadCRM_Accion_Tipo, Descripcion FROM ActividadCRM_Accion_Tipo WHERE Activo=1 ORDER BY Descripcion", True)
            Util.Cargar_Combo(Me.C_Personal, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and FechaBajaEmpresa is null ORDER BY Nombre", False)
            Util.Cargar_Combo(Me.C_Prioridad, "SELECT ID_Prioridad, Descripcion FROM Prioridad  ORDER BY ID_Prioridad", False)

            If pId <> 0 Then
                Call Cargar_Form(pId)
            Else
                Call Netejar_Pantalla()
            End If

            Me.KeyPreview = False

            Me.TE_Codigo.ButtonsRight("Lupeta").Visible = False

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

            Call GetFromForm(oLinqAccion)

            If oLinqAccion.ID_ActividadCRM_Accion = 0 Then  ' Comprovacio per saber si es un insertar o un nou

                oLinqActividad.ActividadCRM_Accion.Add(oLinqAccion)

                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Me.TE_Codigo.Value = oLinqAccion.ID_ActividadCRM_Accion
                Call Fichero.Cargar_GRID(oLinqAccion.ID_ActividadCRM_Accion)
                Call CargaGrid_Destinatarios(oLinqAccion.ID_ActividadCRM_Accion)  'Fem això pq al apretar el botó afegir de la grid de personal, si la acció no estava guardada peta

                If oLinqActividad.ActividadCRM_Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal And F.Finalizado = True).Count = 1 Then
                    If oclsActividad.DesFinalizarActividad = True Then
                        Mensaje.Mostrar_Mensaje("Actividad desfinalizada", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                    End If
                End If
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If

            'Me.EstableixCaptionForm("Acción:" & )

            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqAccion
                Me.TE_Codigo.Text = .ID_ActividadCRM
                'Me.T_Asunto.Text = .Asunto
                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Aviso.Value = .AvisoFecha

                'If .ActividadCRM_Accion_Aux.DescripcionRTF Is Nothing = False Then
                '    Me.R_Descripcion.pText = .ActividadCRM_Accion_Aux.DescripcionRTF
                'Else
                '    Me.R_Descripcion.pText = .Descripcion
                'End If

                Me.R_Observaciones.pText = .ActividadCRM_Accion_Aux.Observaciones
                Me.R_Descripcion.pText = .ActividadCRM_Accion_Aux.DescripcionRTF
                Me.R_Solucion.pText = .ActividadCRM_Accion_Aux.SolucionRTF
                Me.C_Personal.Value = .ID_Personal
                Me.C_Prioridad.Value = .ID_Prioridad
                Me.DT_HoraAviso.Value = .AvisoHora
                Me.C_Tipo.Value = .ID_ActividadCRM_Accion_Tipo
                Me.CH_Finalizada.Checked = .Finalizada

                If .ActividadCRM_Accion_Aux.Hoja Is Nothing = False Then
                    Me.Excel1.M_LoadDocument(.ActividadCRM_Accion_Aux.Hoja)
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pAccion As ActividadCRM_Accion)
        Try
            With pAccion
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



                .FechaAlta = Me.DT_Alta.Value
                .AvisoFecha = Me.DT_Aviso.Value
                .AvisoHora = Me.DT_HoraAviso.Value
                .ActividadCRM_Accion_Tipo = oDTC.ActividadCRM_Accion_Tipo.Where(Function(F) F.ID_ActividadCRM_Accion_Tipo = CInt(Me.C_Tipo.Value)).FirstOrDefault
                .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_Personal.Value)).FirstOrDefault
                .Prioridad = oDTC.Prioridad.Where(Function(F) F.ID_Prioridad = CInt(Me.C_Prioridad.Value)).FirstOrDefault

                Dim _aux As ActividadCRM_Accion_Aux

                If pAccion.ID_ActividadCRM_Accion = 0 Then
                    _aux = New ActividadCRM_Accion_Aux
                    .ActividadCRM_Accion_Aux = _aux
                Else
                    _aux = .ActividadCRM_Accion_Aux
                End If

                _aux.Observaciones = Me.R_Observaciones.pText
                _aux.DescripcionRTF = Me.R_Descripcion.pText
                _aux.Hoja = Me.Excel1.M_SaveDocument.ToArray
                _aux.SolucionRTF = Me.R_Solucion.pText

                .Descripcion = Me.R_Descripcion.pTextEspecial
                .Solucion = Me.R_Solucion.pTextEspecial

                .Finalizada = Me.CH_Finalizada.Checked

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqAccion = (From taula In oDTC.ActividadCRM_Accion Where taula.ID_ActividadCRM_Accion = pID Select taula).First
            Call SetToForm()

            If oLinqAccion.ID_Personal <> Seguretat.oUser.ID_Personal Then 'només si ets el propietari podràs finalitzar la acció
                Me.CH_Finalizada.Enabled = False
            End If

            Call CargaGrid_Destinatarios(pID)

            Fichero.Cargar_GRID(pID)

            Call Permisos()

            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqAccion = New ActividadCRM_Accion
        ' oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.C_Personal.Value = Seguretat.oUser.ID_Personal
        Me.C_Prioridad.Value = 1

        Fichero.Cargar_GRID(0)
        'Me.Tab_Principal.Tabs("Detalle").Selected = True

        Me.DT_Alta.Value = Now.Date

        Call CargaGrid_Destinatarios(0)

        ErrorProvider1.Clear()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.C_Tipo)
                oClsControls.ControlBuit(.DT_Alta)
                oClsControls.ControlBuit(.R_Descripcion)
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

    Private Sub Permisos()
        Me.C_Tipo.ReadOnly = True
        Me.C_Prioridad.ReadOnly = True
        Me.DT_Aviso.ReadOnly = True
        Me.DT_HoraAviso.ReadOnly = True
        Me.CH_Finalizada.Enabled = False
        Me.R_Descripcion.RichText.ReadOnly = True
        Me.R_Solucion.RichText.ReadOnly = True
        Me.GRD_Destinatarios.M.clsToolBar.DesactivarBotonesEdicion()

        If oLinqActividad.Finalizada = True Or oLinqAccion.Finalizada = True Then 'si l'activitat o l'acció està finalitzada no es podrà editar re
            Exit Sub
        End If

        If oclsActividad.oEsPropietari = True Or oLinqAccion.ID_Personal = Seguretat.oUser.ID_Personal Then 'Si ets propietari de l'activitat o de l'acció podràs fer qualsevol cosa
            Me.C_Tipo.ReadOnly = False
            Me.C_Prioridad.ReadOnly = False
            Me.DT_Aviso.ReadOnly = False
            Me.DT_HoraAviso.ReadOnly = False
            Me.CH_Finalizada.Enabled = True
            Me.R_Descripcion.RichText.ReadOnly = False
            Me.R_Solucion.RichText.ReadOnly = False
            Me.GRD_Destinatarios.M.clsToolBar.ActivarBotonesEdicion()
        End If


        Me.R_Solucion.RichText.ReadOnly = False
    End Sub

#End Region

#Region "Events Varis"

#End Region

#Region "Grid Destinatarios"

    Private Sub CargaGrid_Destinatarios(ByVal pId As Integer)
        Try
            Dim _Asignacion As IEnumerable(Of ActividadCRM_Accion_Personal) = From taula In oDTC.ActividadCRM_Accion_Personal Where taula.ID_ActividadCRM_Accion = pId Select taula

            With Me.GRD_Destinatarios

                '.GRID.DataSource = _Asignacion
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
            If oLinqAccion.ActividadCRM_Accion_Personal.Where(Function(F) F.Personal Is _Personal).Count <> 0 Then
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

            ' Dim _PersonalAssignat As IEnumerable(Of Personal) = oDTC.ActividadCRM_Personal.Where(Function(F) F.ID_ActividadCRM = oLinqActividad.ID_ActividadCRM).Select(Function(F) F.Personal)

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal) = (From Taula In oDTC.Personal Where Taula.Activo = True And Taula.FechaBajaEmpresa.HasValue = False Order By Taula.Nombre Select Taula)   '_PersonalAssignat.Contains(Taula)
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

    Private Sub GRD_Destinatarios_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Destinatarios.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Destinatarios

                If oLinqAccion.ID_ActividadCRM_Accion = 0 AndAlso Guardar() = False Then
                    Exit Sub
                End If

                If oLinqActividad.ActividadCRM_Personal.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible asignar personal, primero se debe asignar los destinatarios a la actividad", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("ActividadCRM_Accion").Value = oLinqAccion

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
                Dim _AsignacionPersonal As ActividadCRM_Accion_Personal = e.ListObject
                Dim _NewAvis As New clsAvisos(oDTC)
                _NewAvis.AlEliminarUnPersonalDeUnaAccion(oLinqAccion, _AsignacionPersonal.ActividadCRM_Accion.ActividadCRM.Personal)

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

            Dim _AsignacionPersonal As ActividadCRM_Accion_Personal = e.Row.ListObject
            If _AsignacionPersonal.ID_ActividadCRM_Accion_Personal = 0 Then
                Dim _NewAvis As New clsAvisos(oDTC)
                _NewAvis.AlAsignarUnPersonalAUnaAccion(oLinqAccion, _AsignacionPersonal.Personal)
            End If

            Dim _PersonalActual As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
            Dim _Missatge As String = "El usuario:<b> " & _PersonalActual.Nombre & "</b> te ha asignado a una acción de la actividad nº <b>" & oLinqActividad.ID_ActividadCRM & "</b> con asunto:<b> " & oLinqActividad.Asunto
            Dim _Asunto As String = "Has sido asignado a una nueva acción de una actividad de Abidos"
            clsActividad.EnviarCorreuAlaBustia(_AsignacionPersonal.Personal.Personal_Emails.FirstOrDefault.Email, "abidos@westpoint.es", _Asunto, _Missatge, Seguretat.oUser.ID_Personal, _AsignacionPersonal.ID_Personal, oLinqActividad.ID_ActividadCRM, oLinqAccion.ID_ActividadCRM_Accion)



            oDTC.SubmitChanges()
            Call clsActividad.DarPorNoLeidoALosDestinatariosDeUnaAccionDeUnaActividad(oDTC, oLinqAccion)
            Call clsActividad.AñadirDestinatarioALaActividadSiHaceFalta(oDTC, oLinqAccion, _AsignacionPersonal.ID_Personal)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Destinatarios_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Destinatarios.M_Grid_InitializeRow
        If e.Row.Cells("ID_ActividadCRM_Accion_Personal").Value <> 0 Then
            e.Row.Activation = Activation.NoEdit
        Else
            e.Row.Activation = Activation.AllowEdit
        End If
    End Sub

#End Region

End Class