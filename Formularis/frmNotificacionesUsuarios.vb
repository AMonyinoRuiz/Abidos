Imports Infragistics.Win.UltraWinGrid

Public Class frmNotificacionesUsuarios
    Dim oDTC As DTCDataContext

    Public Sub Entrada()

        Me.AplicarDisseny()

        oDTC = New DTCDataContext(BD.Conexion)

        Util.Cargar_Combo(Me.C_Usuario, "Select ID_Usuario, NombreCompleto From Usuario Where Activo=1", False)

    End Sub

    Private Sub CargaGrid_GRD_TipoNotificacion(ByVal pId As Integer)
        Try
            Dim _Horas As IEnumerable(Of Notificacion_Automatica_Usuario) = From taula In oDTC.Notificacion_Automatica_Usuario Where taula.ID_Usuario = pId Select taula

            With Me.GRD_TipoNotificacion
                '.GRID.DataSource = _Horas
                .M.clsUltraGrid.CargarIEnumerable(_Horas)

                .M_Editable()


                Call CargarCombo_TipoNotificacion(.GRID)

                .GRID.DisplayLayout.Bands(0).Columns("Usuario").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Usuario").CellClickAction = CellClickAction.CellSelect
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TipoNotificacion(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Notificacion_Automatica_Tipo) = (From Taula In oDTC.Notificacion_Automatica_Tipo Order By Taula.Descripcion Select Taula)

            Dim Var As Notificacion_Automatica_Tipo
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Notificacion_Automatica_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Notificacion_Automatica_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_TipoNotificacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_TipoNotificacion.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_TipoNotificacion

                If Me.C_Usuario.SelectedIndex = -1 Then
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Usuario").Value = oDTC.Usuario.Where(Function(F) F.ID_Usuario = CInt(Me.C_Usuario.Value)).FirstOrDefault

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_TipoNotificacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_TipoNotificacion.M_ToolGrid_ToolEliminarRow
        Try

            'If e.IsAddRow Then
            '    oLinqParte.Parte_Horas.Remove(e.ListObject)
            '    Exit Sub
            'End If

            ' Dim _Hora As Parte_Horas = e.ListObject

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_TipoNotificacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_TipoNotificacion.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub C_Usuario_ValueChanged(sender As Object, e As EventArgs) Handles C_Usuario.ValueChanged
        If Me.C_Usuario.SelectedIndex <> -1 Then
            Call CargaGrid_GRD_TipoNotificacion(Me.C_Usuario.Value)
        End If
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

End Class