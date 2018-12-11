Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win
Imports DevExpress.XtraScheduler

Public Class frmCampaña_Telemarketing
    Dim oDTC As DTCDataContext
    Dim oLinqCliente As Cliente
    ' Dim oVisualizarAnteriores As Boolean = False

    Public Sub Entrada()
        Try

            Me.AplicarDisseny()

            Dim _SQLFiltreUsuari As String
            If Seguretat.oUser.NivelSeguridad = 1 Then
                _SQLFiltreUsuari = ""
            Else
                _SQLFiltreUsuari = " Where ID_Campaña_Estado=" & EnumCampañaEstado.Abierta & " and  ID_Campaña in (Select ID_Campaña From Campaña_Usuario Where ID_Usuario=" & Seguretat.oUser.ID_Usuario & ")"
            End If
            Util.Cargar_Combo(Me.C_Campaña, "Select ID_Campaña, Descripcion From Campaña " & _SQLFiltreUsuari)

            oDTC = New DTCDataContext(BD.Conexion)
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Visible = False
            Me.L_Telefono_Empresa.Text = ""
            Me.L_Telefono_Contacto.Text = ""
            Me.L_Nombre_Contacto.Text = ""

            Me.GRD_Exportacion.M.clsToolBar.Boto_Afegir("MarcarExportado", "Marcar como exportado", True)

            ' Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
            'Call Netejar_Pantalla()

            Me.KeyPreview = False

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CarregarDades()
        Try
            Dim _ID_Campaña As Integer = Me.C_Campaña.Value
            Util.Cargar_Combo(Me.C_Estado_Seguimiento, "Select ID_Campaña_Cliente_Seguimiento_Estado, Descripcion  + ' (' + cast((Select Count(*) From Campaña_Cliente Where ID_Campaña= " & _ID_Campaña & " and ID_Campaña_Cliente_Seguimiento_Estado=Campaña_Cliente_Seguimiento_Estado.ID_Campaña_Cliente_Seguimiento_Estado) as nvarchar(50)) + ')' as Contador From Campaña_Cliente_Seguimiento_Estado", True)
            Dim _NumSeguimiento As Integer = oDTC.Campaña_Cliente.Where(Function(F) F.ID_Campaña = _ID_Campaña).Count
            Me.C_Estado_Seguimiento.Items.Add(999, "Todos (" & _NumSeguimiento & ")")
            Me.R_Observaciones.pText = oDTC.Campaña.Where(Function(F) F.ID_Campaña = _ID_Campaña).FirstOrDefault.Observaciones

            If Me.GRD_Clientes_A_Llamar.GRID.Rows.Count > 0 Then
                Me.GRD_Clientes_A_Llamar.GRID.Rows(0).Selected = True
                Me.GRD_Clientes_A_Llamar.GRID.Rows(0).Activated = True
                Call GRD_Clientes_A_Llamar_M_GRID_ClickRow(Nothing, Nothing)
            End If

            'Trobar l'ultima client modificat
            'Dim _Campaña As Campaña = oDTC.Campaña.Where(Function(F) F.ID_Campaña = CInt(Me.C_Campaña.Value)).FirstOrDefault
            Dim _Seguimiento As Campaña_Cliente_Seguimiento = oDTC.Campaña_Cliente_Seguimiento.Where(Function(F) F.Campaña_Cliente.ID_Campaña = _ID_Campaña).OrderByDescending(Function(F) F.FechaIntroduccion).FirstOrDefault
            If _Seguimiento Is Nothing Then
                Me.L_Usuario_Actual.Text = ""
            Else
                Me.L_Usuario_Actual.Text = "Última empresa contactada:  " & _Seguimiento.Campaña_Cliente.Cliente.Nombre & " - (" & _Seguimiento.Campaña_Cliente_Seguimiento_Estado.Descripcion & ")"
            End If

            Me.TAB_General.Tabs("EmpresasALlamar").Selected = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#Region "Events varis"

    Private Sub C_Campaña_ValueChanged(sender As Object, e As EventArgs) Handles C_Campaña.ValueChanged
        If Me.C_Campaña.SelectedIndex <> -1 Then
            Call CarregarDades()
        End If
    End Sub

    Private Sub C_Estado_Seguimiento_ValueChanged(sender As Object, e As EventArgs) Handles C_Estado_Seguimiento.ValueChanged
        If Me.C_Estado_Seguimiento.SelectedIndex <> -1 Then
            Call CargarLlistatClientsSeleccionats(Me.C_Campaña.Value, Me.C_Estado_Seguimiento.Value)
        End If
    End Sub

    Private Sub B_Refrescar_Click(sender As Object, e As EventArgs) Handles B_Refrescar.Click
        If Me.C_Campaña.SelectedIndex <> -1 Then
            Call CarregarDades()
        End If
    End Sub

    Private Sub TAB_General_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_General.SelectedTabChanged
        Select Case e.Tab.Key
            Case "Calendario"
                If Me.C_Campaña.SelectedIndex <> -1 Then
                    Me.SchedulerControl1.DayView.TimeScale = TimeSpan.FromMinutes(15)
                    Me.SchedulerControl1.GoToToday()
                    Me.SchedulerControl1.DayView.TopRowTime = DateTime.Now.TimeOfDay
                    Me.C_Campaña_SeguimientoTableAdapter.Connection = BD.Conexion
                    Me.C_Campaña_SeguimientoTableAdapter.Fill(Me.AbidosDataSet1.C_Campaña_Seguimiento, Me.C_Campaña.Value)
                    'Me.SchedulerControl1.DayView.ShowWorkTimeOnly = True
                End If
            Case "Exportacion"
                Call CargarGrid_Exportacion(Me.C_Campaña.Value)
        End Select


    End Sub

    Private Sub TAB_Inferior_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Inferior.SelectedTabChanged
        Me.GRD_Contactos.GRID.ActiveRow = Nothing
        Me.GRD_Seguimiento.GRID.ActiveRow = Nothing
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim _Seguimiento As Campaña_Cliente_Seguimiento
        If BD.RetornaValorSQL("Select Count(*) From C_Campaña_Avisos WHERE ID_Usuario=" & Seguretat.oUser.ID_Usuario) = 0 Then
            Exit Sub
        End If

        Dim _IDSeguimiento As Integer = BD.RetornaValorSQL("Select top 1 ID_Campaña_Cliente_Seguimiento From C_Campaña_Avisos WHERE ID_Usuario=" & Seguretat.oUser.ID_Usuario)
        _Seguimiento = oDTC.Campaña_Cliente_Seguimiento.Where(Function(F) F.ID_Campaña_Cliente_Seguimiento = _IDSeguimiento).FirstOrDefault
        Dim _Text As String
        _Text = "Campaña: " & _Seguimiento.Campaña_Cliente.Campaña.Descripcion & vbCrLf
        _Text = _Text & "Empresa: " & _Seguimiento.Campaña_Cliente.Cliente.Nombre & vbCrLf
        _Text = _Text & "Estado: " & _Seguimiento.Campaña_Cliente_Seguimiento_Estado.Descripcion & vbCrLf
        _Text = _Text & "Anotación: " & _Seguimiento.Descripcion

        Mensaje.Mostrar_Mensaje(_Text, M_Mensaje.Missatge_Modo.INFORMACIO, "", "", True)
    End Sub

#End Region

#Region "Clientes a llamar"

    Private Sub CargarLlistatClientsSeleccionats(ByVal pIdCampaña As Integer, ByVal pIDEstado As Integer)
        Try
            With Me.GRD_Clientes_A_Llamar

                Dim _SQLCampañes As String
                'La campaña 999 es Todos
                If pIDEstado = 999 Then
                    _SQLCampañes = " Where ID_Campaña=" & pIdCampaña
                Else
                    _SQLCampañes = " Where ID_Campaña=" & pIdCampaña & " and ID_Campaña_Cliente_Seguimiento_Estado=" & pIDEstado & ""
                End If

                '.GRID.DataSource = Hilos
                If pIdCampaña <> 0 And pIDEstado <> 0 Then
                    .M.clsUltraGrid.Cargar("Select * From C_Campaña_Cliente_Seguimiento " & _SQLCampañes & " Order by Nombre", BD)
                Else
                    Me.GRD_Clientes_A_Llamar.GRID.DataSource = Nothing
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

    Private Sub GRD_Clientes_A_Llamar_M_GRID_DoubleClickRow(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Clientes_A_Llamar.M_GRID_DoubleClickRow
        If Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows.Count = 1 Then
            Dim frm As New frmCliente
            frm.Entrada(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Cliente").Value, frmCliente.TipoEntrada.FuturoCliente)
            frm.FormObrir(Me, True)
        End If
    End Sub

    Private Sub GRD_Clientes_A_Llamar_M_GRID_ClickRow(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Clientes_A_Llamar.M_GRID_ClickRow
        If Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows.Count > 0 Then
            Call CargaGrid_Contacto(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Cliente").Value)
            Call CargaGrid_Seguimiento(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Campaña_Cliente").Value)
            Call CargaGrid_Seguimiento_Anterior(CInt(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Cliente").Value))
            Call CargaGrid_Division(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Campaña_Cliente").Value)
            Me.TAB_Inferior.Tabs("Contactos").Selected = True
            Me.L_Telefono_Empresa.Text = Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("Telefono").Value
            Me.L_Nombre_Contacto.Text = ""
            Me.L_Telefono_Contacto.Text = ""
        End If
    End Sub

    Private Sub GRD_Clientes_A_Llamar_M_GRID_AfterSelectChange(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles GRD_Clientes_A_Llamar.M_GRID_AfterSelectChange
        If Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows.Count = 0 Then
            Call CargaGrid_Contacto(0)
            Call CargaGrid_Seguimiento(0)
            Call CargaGrid_Seguimiento_Anterior(0)
        End If
    End Sub

#End Region

#Region "Grid Contacto"

    Private Sub CargaGrid_Contacto(ByVal pId As Integer)
        Try
            With Me.GRD_Contactos
                Dim Contactes As IEnumerable(Of Cliente_Contacto) = From taula In oDTC.Cliente_Contacto Where taula.ID_Cliente = pId Select taula

                oLinqCliente = oDTC.Cliente.Where(Function(f) f.ID_Cliente = pId).FirstOrDefault
                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(Contactes)

                .M_Editable()
                .M_TreureFocus()

                Call CargarComboIdioma()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contactos_M_GRID_ClickRow(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Contactos.M_GRID_ClickRow
        If Me.GRD_Contactos.GRID.Selected.Rows.Count > 0 Then
            Me.L_Nombre_Contacto.Text = Me.GRD_Contactos.GRID.Selected.Rows(0).Cells("Nombre").Value
            Me.L_Telefono_Contacto.Text = Me.GRD_Contactos.GRID.Selected.Rows(0).Cells("Telefono").Value
        End If
    End Sub

    Private Sub GRD_Contacto_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Contactos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Contactos

                If oLinqCliente Is Nothing = True OrElse oLinqCliente.ID_Cliente = 0 Then
                    'Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cliente").Value = oLinqCliente
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Idioma").Value = oDTC.Idioma.Where(Function(F) F.ID_Idioma = CInt(EnumIdioma.Castella)).FirstOrDefault

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contacto_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Contactos.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCliente.Cliente_Contacto.Remove(e.ListObject)
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

    Private Sub GRD_Contacto_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Contactos.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarComboIdioma()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oIdioma As IQueryable(Of Idioma) = (From Taula In oDTC.Idioma Order By Taula.Descripcion Select Taula)
            Dim Var As Idioma

            For Each Var In oIdioma
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Contactos.GRID.DisplayLayout.Bands(0).Columns("Idioma").Style = ColumnStyle.DropDownList
            GRD_Contactos.GRID.DisplayLayout.Bands(0).Columns("Idioma").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub
#End Region

#Region "Grid Seguimiento"

    Private Sub CargaGrid_Seguimiento(ByVal pId As Integer)
        Try
            Dim Contactes As IEnumerable(Of Campaña_Cliente_Seguimiento)
            With Me.GRD_Seguimiento

                Contactes = From taula In oDTC.Campaña_Cliente_Seguimiento Where taula.ID_Campaña_Cliente = pId Select taula



                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(Contactes)

                .GRID.DisplayLayout.Bands(0).Columns("Usuario").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("FechaIntroduccion").CellActivation = UltraWinGrid.Activation.NoEdit

                .M_Editable()
                .M_TreureFocus()



                Call CargarCombo_Estado(.GRID)
                'Call CargarCombo_Usuario(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguimiento_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Seguimiento.M_Grid_InitializeRow, GRD_Seguimiento_Anterior.M_Grid_InitializeRow
        If e.Row.IsAddRow = False Then
            Dim _Seguimiento As Campaña_Cliente_Seguimiento = e.Row.ListObject
            'Si s'ha introduit el color el posarem, si no el deixarem en blanc
            If _Seguimiento.Campaña_Cliente_Seguimiento_Estado.ColorB.HasValue Then
                e.Row.Appearance.BackColor = System.Drawing.Color.FromArgb(_Seguimiento.Campaña_Cliente_Seguimiento_Estado.ColorR, _Seguimiento.Campaña_Cliente_Seguimiento_Estado.ColorG, _Seguimiento.Campaña_Cliente_Seguimiento_Estado.ColorB)
            Else
                e.Row.Appearance.BackColor = Color.White
            End If
        End If

        If e.ReInitialize = False Then
            Call CargarCombo_Usuario(Sender, e.Row.Cells("Usuario"), True)
        End If
    End Sub

    Private Sub GRD_Seguimiento_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Seguimiento.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Seguimiento

                If Me.GRD_Clientes_A_Llamar.GRID.ActiveRow Is Nothing = True Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                Dim _ClienteALlamar As New Campaña_Cliente
                _ClienteALlamar = oDTC.Campaña_Cliente.Where(Function(F) F.ID_Campaña_Cliente = CInt(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Campaña_Cliente").Value)).FirstOrDefault

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.DisplayLayout.Bands(0).Columns("Usuario").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("FechaIntroduccion").CellActivation = UltraWinGrid.Activation.NoEdit

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Campaña_Cliente").Value = _ClienteALlamar
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaIntroduccion").Value = Date.Now
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Usuario").Value = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguimiento_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Seguimiento.M_ToolGrid_ToolEliminarRow
        Try

            If Me.GRD_Clientes_A_Llamar.GRID.ActiveRow Is Nothing = True Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                Exit Sub
            End If

            Dim _ClienteALlamar As New Campaña_Cliente
            _ClienteALlamar = oDTC.Campaña_Cliente.Where(Function(F) F.ID_Campaña_Cliente = CInt(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Campaña_Cliente").Value)).FirstOrDefault


            If e.IsAddRow Then
                _ClienteALlamar.Campaña_Cliente_Seguimiento.Remove(e.ListObject)
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

    Private Sub GRD_Seguimiento_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Seguimiento.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()

        If Me.GRD_Clientes_A_Llamar.GRID.ActiveRow Is Nothing = True Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
            Exit Sub
        End If

        Dim _ClienteALlamar As New Campaña_Cliente
        _ClienteALlamar = oDTC.Campaña_Cliente.Where(Function(F) F.ID_Campaña_Cliente = CInt(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Campaña_Cliente").Value)).FirstOrDefault

        If _ClienteALlamar.Campaña_Cliente_Seguimiento.Count = 0 Then
            _ClienteALlamar.Campaña_Cliente_Seguimiento_Estado = oDTC.Campaña_Cliente_Seguimiento_Estado.Where(Function(F) F.ID_Campaña_Cliente_Seguimiento_Estado = CInt(EnumCampaña_Cliente_Seguimiento_Estado.Pendiente)).FirstOrDefault
        Else
            _ClienteALlamar.Campaña_Cliente_Seguimiento_Estado = _ClienteALlamar.Campaña_Cliente_Seguimiento.LastOrDefault.Campaña_Cliente_Seguimiento_Estado
        End If

        oDTC.SubmitChanges()

 


    End Sub

    Private Sub CargarCombo_Estado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Campaña_Cliente_Seguimiento_Estado) = (From Taula In oDTC.Campaña_Cliente_Seguimiento_Estado Order By Taula.Descripcion Select Taula)
            Dim Var As Campaña_Cliente_Seguimiento_Estado

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Campaña_Cliente_Seguimiento_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Campaña_Cliente_Seguimiento_Estado").ValueList = Valors.Clone

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

    Private Sub GRD_Seguridad_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Seguimiento.M_GRID_BeforeCellActivate, GRD_Seguimiento_Anterior.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "Usuario" Then
            Call CargarCombo_Usuario(sender, e.Cell, False)
        End If
    End Sub



#End Region

#Region "Seguimiento anterior"

    Private Sub CargaGrid_Seguimiento_Anterior(ByVal pId As Integer)
        Try
            Dim Contactes As IEnumerable(Of Campaña_Cliente_Seguimiento)
            With Me.GRD_Seguimiento_Anterior
                Contactes = From taula In oDTC.Campaña_Cliente_Seguimiento Where taula.Campaña_Cliente.ID_Cliente = pId Select taula

                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(Contactes)

                .M_Editable()
                .M_TreureFocus()

                Call CargarCombo_Estado(.GRID)
                ' Call CargarCombo_Usuario(.GRID)

                If .GRID.Rows.Count > 0 Then
                    Util.TabPintarHeaderTab(Me.TAB_Inferior.Tabs("Anterior"), Color.Red)
                Else
                    Util.TabPintarHeaderTab(Me.TAB_Inferior.Tabs("Anterior"), Me.TAB_Inferior.Tabs("Actual").Appearance.BackColor)
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Division"

    Private Sub CargaGrid_Division(ByVal pId As Integer)
        Try
            With Me.GRD_Division
                Dim _Divisiones As IEnumerable(Of Campaña_Cliente_Division) = From taula In oDTC.Campaña_Cliente_Division Where taula.ID_Campaña_Cliente = pId Select taula

                '.GRID.DataSource = Contactes
                .M.clsUltraGrid.CargarIEnumerable(_Divisiones)

                .M_Editable()
                .M_TreureFocus()

                Call CargarComboDivision()
                Call CargarCombo_Respuesta(.GRID, 0)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Division_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Division.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "Campaña_Cliente_Division_Respuesta" Then
            Call CargarCombo_Respuesta(e.Cell, e.Cell.Row.Cells("ID_Producto_Division").Value)

        End If
    End Sub

    Private Sub GRD_Division_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As CellEventArgs) Handles GRD_Division.M_GRID_CellListSelect
        Try

            If CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue Is Nothing Then
                e.Cell.ValueList = Nothing
            End If

            Select Case e.Cell.Column.Key
                Case "Producto_Division"
                    e.Cell.Row.Cells("Campaña_Cliente_Division_Respuesta").Value = Nothing

            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Division_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Division.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Division

                Dim _Campaña_Client As Campaña_Cliente
                If Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                Else
                    _Campaña_Client = oDTC.Campaña_Cliente.Where(Function(F) F.ID_Campaña_Cliente = CInt(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Campaña_Cliente").Value)).FirstOrDefault
                End If


                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Campaña_Cliente").Value = _Campaña_Client

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboDivision()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oDivisiones As IQueryable(Of Producto_Division) = (From Taula In oDTC.Producto_Division Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Division

            For Each Var In oDivisiones
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            GRD_Division.GRID.DisplayLayout.Bands(0).Columns("Producto_Division").Style = ColumnStyle.DropDownList
            GRD_Division.GRID.DisplayLayout.Bands(0).Columns("Producto_Division").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarCombo_Respuesta(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer) ', ByVal pIDDivision As Integer
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Campaña_Cliente_Division_Respuesta)

            ' Where Taula.ID_Producto_Division = pIDDivision
            oTaula = (From Taula In oDTC.Campaña_Cliente_Division_Respuesta Order By Taula.Descripcion Select Taula)


            Dim Var As Campaña_Cliente_Division_Respuesta

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Campaña_Cliente_Division_Respuesta").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Campaña_Cliente_Division_Respuesta").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Respuesta(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDDivision As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Campaña_Cliente_Division_Respuesta) = (From Taula In oDTC.Campaña_Cliente_Division_Respuesta Where Taula.ID_Producto_Division = pIDDivision Order By Taula.Descripcion Select Taula)
            Dim Var As Campaña_Cliente_Division_Respuesta


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

    Private Sub GRD_Division_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Division.M_ToolGrid_ToolEliminarRow
        Try
            If Me.GRD_Clientes_A_Llamar.GRID.ActiveRow Is Nothing = True Then ' si no s'ha seleccionat cap client a trucar no es podrà afegir una row
                Exit Sub
            End If

            Dim _ClienteALlamar As New Campaña_Cliente
            _ClienteALlamar = oDTC.Campaña_Cliente.Where(Function(F) F.ID_Campaña_Cliente = CInt(Me.GRD_Clientes_A_Llamar.GRID.Selected.Rows(0).Cells("ID_Campaña_Cliente").Value)).FirstOrDefault

            If e.IsAddRow Then
                _ClienteALlamar.Campaña_Cliente_Division.Remove(e.ListObject)
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

    Private Sub GRD_Division_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Division.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Exportacion"
    Private Sub CargarGrid_Exportacion(ByVal pID As Integer)
        Try
            With Me.GRD_Exportacion

                .M.clsUltraGrid.Cargar("Select * From C_Campaña_Exportacion Where ID_Campaña=" & pID & " Order by Empresa_Nombre", BD)

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Exportacion_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Exportacion.M_ToolGrid_ToolClickBotonsExtras
        Select Case e.Tool.Key
            Case "MarcarExportado"
                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Exportacion.GRID.Selected.Rows
                    Dim _Seleccion As Campaña_Cliente = oDTC.Campaña_Cliente.Where(Function(F) F.ID_Campaña_Cliente = CInt(pRow.Cells("ID_Campaña_Cliente").Value)).FirstOrDefault
                    _Seleccion.Exportado = True
                    oDTC.SubmitChanges()
                Next

                Call CargarGrid_Exportacion(Me.C_Campaña.Value)
        End Select
    End Sub
#End Region

#Region "Scheduler"
    Private Sub SchedulerControl1_PopupMenuShowing(sender As Object, e As DevExpress.XtraScheduler.PopupMenuShowingEventArgs) Handles SchedulerControl1.PopupMenuShowing
        'Fem això pq no es vegi el menú que surt al fer botó dret
        e.Menu.HidePopup()
        e.Menu.Visible = False
        e.Menu.Items.Clear()
    End Sub
    Private Sub SchedulerControl1_AppointmentViewInfoCustomizing(sender As Object, e As DevExpress.XtraScheduler.AppointmentViewInfoCustomizingEventArgs) Handles SchedulerControl1.AppointmentViewInfoCustomizing
        If e.ViewInfo.Appointment.StatusId.ToString.Length <> 0 Then
            If e.ViewInfo.Appointment.CustomFields("Realizado") = False Then
                Dim _Vermell As Integer = Util.Comprobar_NULL_Per_0(e.ViewInfo.Appointment.CustomFields("ColorR"))
                Dim _Verd As Integer = Util.Comprobar_NULL_Per_0(e.ViewInfo.Appointment.CustomFields("ColorG"))
                Dim _Blau As Integer = Util.Comprobar_NULL_Per_0(e.ViewInfo.Appointment.CustomFields("ColorB"))

                e.ViewInfo.Appearance.BackColor = System.Drawing.Color.FromArgb(_Vermell, _Verd, _Blau)
            End If
        End If
    End Sub
    Private Sub SchedulerControl1_EditAppointmentFormShowing(sender As Object, e As DevExpress.XtraScheduler.AppointmentFormEventArgs) Handles SchedulerControl1.EditAppointmentFormShowing
        'Fem això per a que no es pugui crear un nou appointment
        If e.Appointment Is Nothing = False Then
            If e.Appointment.Id Is Nothing = False Then
                Dim _Seguiment As Campaña_Cliente_Seguimiento = oDTC.Campaña_Cliente_Seguimiento.Where(Function(F) F.ID_Campaña_Cliente_Seguimiento = CInt(e.Appointment.Id)).FirstOrDefault
                Me.C_Estado_Seguimiento.Value = 999 'Todos  '_Seguiment.ID_Campaña_Cliente_Seguimiento_Estado
                Me.TAB_General.Tabs("EmpresasALlamar").Selected = True
                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Clientes_A_Llamar.GRID.Rows
                    If pRow.Cells("ID_Campaña_Cliente").Value = _Seguiment.ID_Campaña_Cliente Then
                        pRow.Activate()
                        pRow.Selected = True
                        Call GRD_Clientes_A_Llamar_M_GRID_ClickRow(Nothing, Nothing)
                        Exit For
                    End If
                Next
            End If
        End If
        e.Handled = True
    End Sub

#End Region

    Private Sub L_Telefono_Empresa_DoubleClick(sender As Object, e As EventArgs) Handles L_Telefono_Empresa.DoubleClick, L_Telefono_Contacto.DoubleClick
        Dim _Prefix As String = oDTC.Configuracion.FirstOrDefault.PrefijoCentralita
        Clipboard.Clear()
        Clipboard.SetText(_Prefix & sender.text)

    End Sub

    Private Sub SchedulerControl1_Click(sender As Object, e As EventArgs) Handles SchedulerControl1.Click

    End Sub

    Private Sub SchedulerStorage1_AppointmentsChanged(sender As Object, e As DevExpress.XtraScheduler.PersistentObjectsEventArgs) Handles SchedulerStorage1.AppointmentsChanged
        Dim _Appointment As DevExpress.XtraScheduler.Appointment
        For Each _Appointment In e.Objects
            Call AlFerAlgoAmbUnAppointment(_Appointment)
        Next
    End Sub

    Private Sub AlFerAlgoAmbUnAppointment(ByRef pAppointment As Appointment)

        Dim _ID As Integer = CInt(pAppointment.Id)
        Dim _Seguiment As Campaña_Cliente_Seguimiento = oDTC.Campaña_Cliente_Seguimiento.Where(Function(F) F.ID_Campaña_Cliente_Seguimiento = _ID).FirstOrDefault
        _Seguiment.FechaAviso = pAppointment.Start
        _Seguiment.HoraAviso = pAppointment.Start.Date & " " & pAppointment.Start.Hour & ":" & pAppointment.Start.Minute
        oDTC.SubmitChanges()

    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub
End Class