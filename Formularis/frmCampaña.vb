Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmCampaña

    Dim oDTC As DTCDataContext
    Dim oLinqCampaña As Campaña


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqCampaña.ID_Campaña <> 0 Then
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    ' Call Guardar()

                    oLinqCampaña.Activo = False
                    oDTC.SubmitChanges()
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
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

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pId As Integer = 0)
        Try

            Me.AplicarDisseny()

            Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
            Me.GRD_Clientes.M.clsToolBar.Boto_Afegir("seleccionar_clientes", "Seleccionar clientes", True)
            Util.Cargar_Combo(Me.C_Estado, "Select ID_Campaña_Estado, Descripcion From Campaña_Estado", True, False)
            Call Netejar_Pantalla()
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

            If oLinqCampaña.ID_Campaña = 0 Then
                If BD.RetornaValorSQL("SELECT Count (*) From Campaña WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            Else
                If BD.RetornaValorSQL("SELECT Count (*) From Campaña WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_Campaña<>" & oLinqCampaña.ID_Campaña) > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqCampaña)

            If oLinqCampaña.ID_Campaña = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Campaña.InsertOnSubmit(oLinqCampaña)
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Call CargaGrid_Clientes(oLinqCampaña.ID_Campaña) 'Fem això pq apareixin els clients després de guardar per primer cop la campaña
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
            With oLinqCampaña
                Me.TE_Codigo.Value = .Codigo
                Me.T_Descripcion.Text = .Descripcion
                Me.DT_Data.Value = .Data
                Me.R_Observaciones.pText = .Observaciones
                Me.C_Estado.Value = .ID_Campaña_Estado
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pEntidad As Campaña)
        Try
            With pEntidad
                .Activo = True
                .Codigo = Me.TE_Codigo.Value
                .Descripcion = Me.T_Descripcion.Text
                .Data = Me.DT_Data.Value
                .Observaciones = Me.R_Observaciones.pText
                .Campaña_Estado = oDTC.Campaña_Estado.Where(Function(F) F.ID_Campaña_Estado = CInt(Me.C_Estado.Value)).FirstOrDefault
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqCampaña = (From taula In oDTC.Campaña Where taula.ID_Campaña = pID Select taula).FirstOrDefault
            Call SetToForm()

            Call CargaGrid_Clientes(pID)
            Call CargaGrid_ClientesSeleccionados(pID)
            Call CargaGrid_Asignacion(pID)

            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Me.EstableixCaptionForm("Campaña: " & (oLinqCampaña.Descripcion))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqCampaña = New Campaña
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        'Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing
        Me.TE_Codigo.Focus()

        Call CargaGrid_Clientes(0)
        Call CargaGrid_ClientesSeleccionados(0)
        Call CargaGrid_Asignacion(0)
        Call CargarGrid_Rendimiento(0, Now.Date, Now.Date)

        Me.DT_Rendimiento_Inicio.Value = Now.Date
        Me.DT_Rendimiento_Fin.Value = Now.Date

        Me.TAB_General.Tabs("Seleccion").Selected = True
        Me.C_Estado.Value = CInt(EnumCampañaEstado.Abierta)
        ' Me.C_OrigenCliente.Value = 1
        ' Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Instalacion")
        Me.TE_Codigo.Value = oDTC.Campaña.Where(Function(F) F.Activo = True).Max(Function(F) F.Codigo) + 1

        ErrorProvider1.Clear()

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.T_Descripcion)
                oClsControls.ControlBuit(.DT_Data)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM Campaña Where Activo=1 ORDER BY Codigo", Me.TE_Codigo, "ID_Campaña", "ID_Campaña")
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

    Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Me.TE_Codigo.Value Is Nothing = False AndAlso IsDBNull(Me.TE_Codigo.Value) = False Then
                Dim ooLinqCampaña As Campaña = oDTC.Campaña.Where(Function(F) F.Codigo = CInt(Me.TE_Codigo.Value)).FirstOrDefault()
                If ooLinqCampaña Is Nothing = False Then
                    Call Cargar_Form(ooLinqCampaña.ID_Campaña)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If

            Else
                Me.TE_Codigo.Value = oDTC.Campaña.Where(Function(F) F.Activo = True).Max(Function(F) F.Codigo) + 1
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "GRD Clientes"

    Private Sub CargaGrid_Clientes(ByVal pId As Integer)
        Try
            With Me.GRD_Clientes

                '.GRID.DataSource = Hilos
                If pId <> 0 Then
                    .M.clsUltraGrid.Cargar("Select * From C_Campaña_Cliente Order by Nombre", BD)
                Else
                    Me.GRD_Clientes.GRID.DataSource = Nothing
                End If

                '.GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.EditAndSelectText

                ' .GRID.RowUpdateCancelAction = RowUpdateCancelAction.CancelUpdate
                '.GRID.UpdateMode = UpdateMode.OnRowChangeOrLostFocus

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Clientes_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Clientes.M_ToolGrid_ToolClickBotonsExtras
        Select Case e.Tool.Key
            Case "seleccionar_clientes"
                Util.WaitFormObrir()
                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Clientes.GRID.Selected.Rows
                    Dim _Seleccion As New Campaña_Cliente
                    _Seleccion.ID_Campaña = oLinqCampaña.ID_Campaña
                    _Seleccion.ID_Cliente = pRow.Cells("ID_Cliente").Value
                    _Seleccion.Campaña_Cliente_Seguimiento_Estado = oDTC.Campaña_Cliente_Seguimiento_Estado.Where(Function(F) F.ID_Campaña_Cliente_Seguimiento_Estado = CInt(EnumCampaña_Cliente_Seguimiento_Estado.Pendiente)).FirstOrDefault
                    If oLinqCampaña.Campaña_Cliente.Where(Function(F) F.ID_Cliente = _Seleccion.ID_Cliente).Count = 0 Then
                        oLinqCampaña.Campaña_Cliente.Add(_Seleccion)
                    End If
                    Dim _Cliente As Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(_Seleccion.ID_Cliente)).FirstOrDefault
                    _Cliente.FechaUltimaAsignacionCampaña = Now.Date
                Next
                oDTC.SubmitChanges()
                Call CargaGrid_ClientesSeleccionados(oLinqCampaña.ID_Campaña)
                Util.WaitFormTancar()
        End Select
    End Sub

#End Region

#Region "GRD Clientes seleccionados"

    Private Sub CargaGrid_ClientesSeleccionados(ByVal pId As Integer)
        Try
            With Me.GRD_Seleccion

                '.GRID.DataSource = Hilos
                If pId <> 0 Then
                    .M.clsUltraGrid.Cargar("Select * From C_Campaña_Cliente Where ID_Cliente in (Select ID_Cliente From Campaña_Cliente Where ID_Campaña=" & pId & ") Order by Nombre", BD)
                Else
                    Me.GRD_Seleccion.GRID.DataSource = Nothing
                End If

                '.GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.EditAndSelectText

                ' .GRID.RowUpdateCancelAction = RowUpdateCancelAction.CancelUpdate
                '.GRID.UpdateMode = UpdateMode.OnRowChangeOrLostFocus
                ' me voy a comer... petons.
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seleccion_M_ToolGrid_ToolEliminar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Seleccion.M_ToolGrid_ToolEliminar
        Dim pRow As UltraGridRow

        For Each pRow In Me.GRD_Seleccion.GRID.Selected.Rows
            Dim _Seleccion As Campaña_Cliente
            _Seleccion = oLinqCampaña.Campaña_Cliente.Where(Function(F) F.ID_Cliente = CInt(pRow.Cells("ID_Cliente").Value)).FirstOrDefault
            oLinqCampaña.Campaña_Cliente.Remove(_Seleccion)
            oDTC.Campaña_Cliente.DeleteOnSubmit(_Seleccion)
        Next
        oDTC.SubmitChanges()
        Call CargaGrid_ClientesSeleccionados(oLinqCampaña.ID_Campaña)
    End Sub

#End Region

#Region "Grid Usuarios"

    Private Sub CargaGrid_Asignacion(ByVal pId As Integer)
        Try
            Dim _Asignacion As IEnumerable(Of Campaña_Usuario) = From taula In oDTC.Campaña_Usuario Where taula.ID_Campaña = pId Select taula

            With Me.GRD_Usuario

                '.GRID.DataSource = _Asignacion
                .M.clsUltraGrid.CargarIEnumerable(_Asignacion)

                'If oLinqCampaña.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                '    .M_Editable()
                'Else
                .M_Editable()
                'End If

                'Call CargarCombo_Usuario(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_AsignacionPersonal_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Usuario.M_GRID_CellListSelect
        Try
            If e.Cell.Column.Key <> "Usuario" Then 'si no modifiquem el combo de personal no hi hem de fer re aqui
                Exit Sub
            End If

            Dim _Usuario As Usuario
            'Comprovem que no s'hagi introduit aquest treballador abans
            _Usuario = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
            If oLinqCampaña.Campaña_Usuario.Where(Function(F) F.Usuario Is _Usuario).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar la misma persona dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
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

    Private Sub GRD_Seguridad_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Usuario.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "Usuario" Then
            Call CargarCombo_Usuario(sender, e.Cell, False)
        End If
    End Sub

    Private Sub GRD_Seguridad_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Usuario.M_Grid_InitializeRow
        If e.ReInitialize = False Then
            Call CargarCombo_Usuario(Sender, e.Row.Cells("Usuario"), True)
        End If
    End Sub

    Private Sub GRD_Usuario_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Usuario.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Usuario

                'If Guardar(False) = False Then
                '    Exit Sub
                'End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Campaña").Value = oLinqCampaña

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_AsignacionPersonal_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Usuario.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCampaña.Campaña_Usuario.Remove(e.ListObject)
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

    Private Sub GRD_AsignacionPersonal_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Usuario.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Rendimiento"
    Private Sub CargarGrid_Rendimiento(ByVal pIDCampaña As Integer, ByVal pDataInici As DateTime, ByVal pDataFi As DateTime)
        Try

            If pIDCampaña = 0 Then
                Exit Sub
            End If




            'Creem el DATATAble amb tantes columnes com estats hi hagi de seguiment + El nom d'usuari + el total
            Dim DT As New DataTable
            DT.Columns.Add("Usuario", GetType(String))


            Dim _Estat As Campaña_Cliente_Seguimiento_Estado

            For Each _Estat In oDTC.Campaña_Cliente_Seguimiento_Estado
                DT.Columns.Add(_Estat.Descripcion, GetType(Integer))
            Next

            DT.Columns.Add("Total", GetType(Integer))


            '        Dim _Usuaris = From t In oDTC.Campaña_Cliente_Seguimiento Group t By t.ID_Usuario Into r() Select N
            ' Dim _Usuaris = From t In oDTC.Campaña_Cliente_Seguimiento Group t By t.ID_Usuario Into g = Group Select g.
            Dim _Usuaris = From t In oDTC.Campaña_Cliente_Seguimiento Where t.Campaña_Cliente.Campaña.ID_Campaña = pIDCampaña And (t.FechaIntroduccion.Date >= pDataInici And t.FechaIntroduccion.Date <= pDataFi) Group By pepe = t.Usuario Into Group Select pepe

            Dim _Usuario As Usuario

            For Each _Usuario In _Usuaris
                Dim _NewRow As DataRow = DT.NewRow
                Dim _Totales As Integer = 0
                _NewRow("Usuario") = _Usuario.Nombre
                For Each _Estat In oDTC.Campaña_Cliente_Seguimiento_Estado
                    _NewRow(_Estat.Descripcion) = _Usuario.Campaña_Cliente_Seguimiento.Where(Function(F) F.ID_Campaña_Cliente_Seguimiento_Estado = _Estat.ID_Campaña_Cliente_Seguimiento_Estado And (F.FechaIntroduccion.Date >= pDataInici And F.FechaIntroduccion.Date <= pDataFi) And F.Campaña_Cliente.Campaña.ID_Campaña = pIDCampaña).Count
                    If _Estat.ID_Campaña_Cliente_Seguimiento_Estado <> EnumCampaña_Cliente_Seguimiento_Estado.Pendiente Then
                        _Totales = _Totales + _NewRow(_Estat.Descripcion)
                    End If
                Next
                _NewRow("Total") = _Totales
                DT.Rows.Add(_NewRow)
            Next

            Me.GRD_Rendimiento.M.clsUltraGrid.Cargar(DT)

            'Bucle per carregar les dades segons usuari i estat
            'Dim _UsuarisQueHanEntratDades As IQueryable(Of Usuario) = 



            'Dim pepe = From Taula In oDTC.Campaña_Cliente_Seguimiento Group By Taula.Usuario Select 
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

    Private Sub B_Rendimiento_Buscar_Click(sender As Object, e As EventArgs) Handles B_Rendimiento_Buscar.Click
        Call CargarGrid_Rendimiento(oLinqCampaña.ID_Campaña, Me.DT_Rendimiento_Inicio.Value, Me.DT_Rendimiento_Fin.Value)
    End Sub

    Private Sub TAB_General_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_General.SelectedTabChanged
        Select Case e.Tab.Key

        End Select
    End Sub


End Class