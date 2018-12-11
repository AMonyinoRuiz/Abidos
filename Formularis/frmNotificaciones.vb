Imports Infragistics.Win.UltraWinGrid

Public Class frmNotificaciones
    Dim oDTC As DTCDataContext

#Region "Toolform"
    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada()
        Me.AplicarDisseny()
        oDTC = New DTCDataContext(BD.Conexion)


    End Sub

#End Region

#Region "Events"
    Private Sub TAB_Principal_SelectedTabChanged(sender As Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
        Select Case e.Tab.Key
            Case "Recibidas"
                Call CargarNotificacionesRecibidas(False)
            Case "Enviadas"
                Call CargarNotificacionesEnviadasAutomaticamente()
                Call CargarNotificacionesEnviadasManualmente()
        End Select
    End Sub

    Private Sub AlTancarFormNotificacion()
        Call CargarNotificacionesEnviadasManualmente()
    End Sub

    Private Sub TABStrip_Recibidas_SelectedTabChanged(sender As Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles TABStrip_Recibidas.SelectedTabChanged
        Select Case e.Tab.Key
            Case "Pendientes"
                Call CargarNotificacionesRecibidas(False)
            Case "Finalizadas"
                Call CargarNotificacionesRecibidas(True)
        End Select
    End Sub


#End Region

#Region "Grid Recibidas"

    Private Sub CargarNotificacionesRecibidas(ByVal pRealizado As Boolean)
        Try
            With Me.GRD_Notificaciones_Recibidas
                .M.clsUltraGrid.Cargar("Select * From C_Notificacion Where Realizado=" & Util.Bool_To_Int(pRealizado) & " and ID_Usuario_Destino=" & Seguretat.oUser.ID_Usuario & " Order by FechaAlta Desc", BD)

                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Notificaciones_Recibidas.GRID.Rows
                    If pRow.Cells("Leido").Value = False Then
                        pRow.CellAppearance.BackColor = Color.LightYellow
                    End If
                    If pRow.Cells("Realizado").Value = True Then
                        pRow.CellAppearance.BackColor = Color.LightGreen
                    End If
                Next

                Dim _NotificacionsNoLlegides As IEnumerable(Of Notificacion) = oDTC.Notificacion.Where(Function(F) F.ID_Usuario_Destino = Seguretat.oUser.ID_Usuario And F.Leido = False)
                Dim _Notificacion As Notificacion
                For Each _Notificacion In _NotificacionsNoLlegides
                    _Notificacion.Leido = True
                Next
                oDTC.SubmitChanges()

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Notificaciones_Recibidas_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Notificaciones_Recibidas.M_GRID_DoubleClickRow2
        e.Cells("Realizado").Value = Not e.Cells("Realizado").Value

        Dim _IDNotificacion As Integer = e.Cells("ID_Notificacion").Value
        Dim _Notificacion As Notificacion = oDTC.Notificacion.Where(Function(F) F.ID_Notificacion = _IDNotificacion).FirstOrDefault
        _Notificacion.Realizado = e.Cells("Realizado").Value
        oDTC.SubmitChanges()
        e.Delete(False)
    End Sub

#End Region

#Region "Grid Enviadas Automaticamente"

    Private Sub CargarNotificacionesEnviadasAutomaticamente()
        Try
            With Me.GRD_Notificaciones_Enviadas_Automaticamente
                .M.clsUltraGrid.Cargar("Select * From C_Notificacion Where Automatica=1 and ID_Usuario_Origen=" & Seguretat.oUser.ID_Usuario & " Order by FechaAlta Desc", BD)

                Dim pRow As UltraGridRow
                For Each pRow In .GRID.Rows
                    If pRow.Cells("Leido").Value = False Then
                        pRow.CellAppearance.BackColor = Color.LightYellow
                    End If
                    If pRow.Cells("Realizado").Value = True Then
                        pRow.CellAppearance.BackColor = Color.LightGreen
                    End If
                Next
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Enviadas Manualmente"

    Private Sub CargarNotificacionesEnviadasManualmente()
        Try
            With Me.GRD_Notificaciones_Enviadas_Manualmente
                .M.clsUltraGrid.Cargar("Select * From C_Notificacion Where Automatica=0 and ID_Usuario_Origen=" & Seguretat.oUser.ID_Usuario & " Order by FechaAlta Desc", BD)

                Dim pRow As UltraGridRow
                For Each pRow In .GRID.Rows
                    If pRow.Cells("Leido").Value = False Then
                        pRow.CellAppearance.BackColor = Color.LightYellow
                    End If
                    If pRow.Cells("Realizado").Value = True Then
                        pRow.CellAppearance.BackColor = Color.LightGreen
                    End If
                Next
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Notificaciones_Enviadas_Manualmente_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Notificaciones_Enviadas_Manualmente.M_ToolGrid_ToolAfegir
        Dim _frm As New frmNotificacion_Mantenimiento
        _frm.Entrada(oDTC)
        _frm.FormObrir(Me, False)
        AddHandler _frm.AlTancarForm, AddressOf AlTancarFormNotificacion
    End Sub

    Private Sub GRD_Notificaciones_Enviadas_Manualmente_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Notificaciones_Enviadas_Manualmente.M_ToolGrid_ToolEliminarRow
        If Me.GRD_Notificaciones_Enviadas_Manualmente.GRID.Selected.Rows.Count = 1 Then
            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
                Dim _IDNotificacion As Integer = e.Cells("ID_Notificacion").Value
                Dim _Notificacion As Notificacion = oDTC.Notificacion.Where(Function(F) F.ID_Notificacion = _IDNotificacion).FirstOrDefault
                oDTC.Notificacion.DeleteOnSubmit(_Notificacion)
                oDTC.SubmitChanges()
                Call CargarNotificacionesEnviadasManualmente()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If
        End If
    End Sub

    Private Sub GRD_Notificaciones_Enviadas_Manualmente_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Notificaciones_Enviadas_Manualmente.M_ToolGrid_ToolVisualitzarDobleClickRow
        If Me.GRD_Notificaciones_Enviadas_Manualmente.GRID.Selected.Rows.Count = 1 Then
            Dim _frm As New frmNotificacion_Mantenimiento
            _frm.Entrada(oDTC, Me.GRD_Notificaciones_Enviadas_Manualmente.GRID.Selected.Rows(0).Cells("ID_Notificacion").Value)
            _frm.FormObrir(Me, False)
            AddHandler _frm.AlTancarForm, AddressOf AlTancarFormNotificacion
        End If
    End Sub

#End Region

 End Class