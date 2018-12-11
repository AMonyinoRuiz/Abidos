Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmAutomatismo_Accion
    Dim oDTC As DTCDataContext
    Dim oLinqAccion As Automatismo_Accion
    Dim oLinqAutomatismo As Automatismo
    'Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2

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

    Public Sub Entrada(ByRef pAutomatismo As Automatismo, ByRef pDTC As DTCDataContext, Optional ByVal pId As Integer = 0)

        Try

            Me.AplicarDisseny()

            oLinqAutomatismo = pAutomatismo

            'Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Instalacion_Receptora_Archivo", 1)

            oDTC = pDTC

            Util.Cargar_Combo(Me.C_TipoAccion, "Select ID_ActividadCRM_Accion_Tipo, Descripcion From ActividadCRM_Accion_Tipo Where Activo=1 Order by Codigo", False)
            Util.Cargar_Combo(Me.C_Prioridad, "Select ID_Prioridad, Descripcion From Prioridad Order by ID_Prioridad", False)

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

            Call GetFromForm(oLinqAccion)

            If oLinqAccion.ID_Automatismo_Accion = 0 Then  ' Comprovacio per saber si es un insertar o un nou

                oLinqAutomatismo.Automatismo_Accion.Add(oLinqAccion)

                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Call CargaGrid_Personal(oLinqAccion.ID_Automatismo_Accion)  'Fem això pq al apretar el botó afegir de la grid de personal, si la acció no estava guardada peta
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
            With oLinqAccion

                Me.T_Descripcion.Text = .Descripcion
                Me.C_TipoAccion.Value = .ID_ActividadCRM_Accion_Tipo
                Me.C_Prioridad.Value = .ID_Prioridad
                Me.R_Explicacion.pText = .Explicacion

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pAccion As Automatismo_Accion)
        Try
            With pAccion
                .Descripcion = Me.T_Descripcion.Text
                .Explicacion = Me.R_Explicacion.pText

                .ActividadCRM_Accion_Tipo = oDTC.ActividadCRM_Accion_Tipo.Where(Function(F) F.ID_ActividadCRM_Accion_Tipo = CInt(Me.C_TipoAccion.Value)).FirstOrDefault
                .Prioridad = oDTC.Prioridad.Where(Function(F) F.ID_Prioridad = CInt(Me.C_Prioridad.Value)).FirstOrDefault

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqAccion = (From taula In oDTC.Automatismo_Accion Where taula.ID_Automatismo_Accion = pID Select taula).First
            Call SetToForm()
            'Fichero.Cargar_GRID(pID)
            Call CargaGrid_Personal(pID)

            'Call Carga_Grid_Caracteristicas_Personalizadas(pID)
            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqAccion = New Automatismo_Accion
        ' oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        '  Fichero.Cargar_GRID(0)
        'Me.Tab_Principal.Tabs("Detalle").Selected = True

        Me.C_Prioridad.SelectedIndex = 0
        Me.C_TipoAccion.SelectedIndex = 0

        ' Me.TE_PropuestaLinea.ButtonsRight("Lupeta").Enabled = True
        ErrorProvider1.Clear()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.T_Descripcion)
                oClsControls.ControlBuit(.C_TipoAccion)
                oClsControls.ControlBuit(.C_Prioridad)
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

#End Region

#Region "Grid Personal"

    Private Sub CargaGrid_Personal(ByVal pId As Integer)
        Try
            Dim _Asignacion As IEnumerable(Of Automatismo_Accion_Personal) = From taula In oDTC.Automatismo_Accion_Personal Where taula.ID_Automatismo_Accion = pId Select taula

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
            If oLinqAccion.Automatismo_Accion_Personal.Where(Function(F) F.Personal Is _Personal).Count <> 0 Then
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
            'Dim _LlistatDePersonalAssignat As ArrayList = oLinqAutomatismo.Automatismo_Personal.ToArray
            Dim _PersonalAssignat As IEnumerable(Of Personal) = oDTC.Automatismo_Personal.Where(Function(F) F.ID_Automatismo = oLinqAutomatismo.ID_Automatismo).Select(Function(F) F.Personal)

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal) = (From Taula In oDTC.Personal Where _PersonalAssignat.Contains(Taula) Order By Taula.Nombre Select Taula)
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


                If oLinqAccion.ID_Automatismo_Accion = 0 AndAlso Guardar() = False Then
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Automatismo_Accion").Value = oLinqAccion

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Personal_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Personal.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqAccion.Automatismo_Accion_Personal.Remove(e.ListObject)
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

    '#Region "Contacto"

    '    Private Sub CargaGrid_Contacto(ByVal pId As Integer)
    '        Try

    '            With Me.GRD_Contacto

    '                Dim _Contactos As IEnumerable(Of Instalacion_Receptora_Contacto) = From taula In oDTC.Instalacion_Receptora_Contacto Where taula.ID_Instalacion_Receptora = pId Select taula Order By taula.Orden
    '                .M.clsUltraGrid.CargarIEnumerable(_Contactos)
    '                '.GRID.DataSource = Contactos

    '                .M_Editable()
    '                .M_TreureFocus()

    '            End With

    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '    Private Sub GRD_Contacto_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs)
    '        Try
    '            With Me.GRD_Contacto

    '                If oLinqReceptora.ID_Instalacion_Receptora = 0 Then
    '                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '                    Exit Sub
    '                End If

    '                .M_ExitEditMode()
    '                .M_AfegirFila()

    '                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Receptora").Value = oLinqReceptora

    '            End With
    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '    Private Sub GRD_Contacto_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow)
    '        Try

    '            If e.IsAddRow Then
    '                oLinqReceptora.Instalacion_Receptora_Contacto.Remove(e.ListObject)
    '                Exit Sub
    '            End If

    '            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
    '                e.Delete(False)
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
    '            End If

    '            oDTC.SubmitChanges()

    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '    Private Sub GRD_Contacto_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs)
    '        oDTC.SubmitChanges()
    '    End Sub

    '#End Region


End Class