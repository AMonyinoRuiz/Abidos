Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmManteniment_Grados
    Dim oDTC As DTCDataContext

    Private Sub frmManteniment_Familias_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        Me.GRD_Grado.GRID.ActiveRow = Nothing
        Me.GRD_Grado.GRID.Selected.Rows.Clear()
    End Sub

    Public Sub Entrada()
        Me.AplicarDisseny()
        Me.KeyPreview = False
        Me.ToolForm.M.Botons.tGuardar.SharedProps.Visible = False
        oDTC = New DTCDataContext(BD.Conexion)

        Call CargaGrid_Grado()
        Call CargaGrid_Sector(0)
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#Region "Grado"

    Private Sub CargaGrid_Grado()
        Try
            Dim _Grado As IEnumerable(Of Producto_Grado) = From taula In oDTC.Producto_Grado Order By taula.Descripcion Select taula

            With Me.GRD_Grado
                '.GRID.DataSource = _Grado
                .M.clsUltraGrid.CargarIEnumerable(_Grado)

                .M_Editable()
                .M_TreureFocus()
            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Grado_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Grado.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_Division_M_GRID_AfterSelectChange(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles GRD_Grado.M_GRID_AfterSelectChange
        If Me.GRD_Grado.GRID.Selected.Rows.Count = 0 Then
            Call CargaGrid_Sector(0)
        End If
    End Sub

    Private Sub GRD_Division_M_GRID_ClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Grado.M_GRID_ClickRow
        If Me.GRD_Grado.GRID.Selected.Rows.Count = 1 Then
            If Me.GRD_Grado.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Grado.GRID.ActiveRow.Cells("ID_Producto_Grado").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                Exit Sub
            End If

            Dim _Grado As New Producto_Grado
            _Grado = Me.GRD_Grado.GRID.Rows.GetItem(Me.GRD_Grado.GRID.ActiveRow.Index).listObject
            Dim IDGrado As Integer = _Grado.ID_Producto_Grado

            Call CargaGrid_Sector(IDGrado)
        End If
    End Sub

#End Region

#Region "Sector"

    Private Sub CargaGrid_Sector(ByVal pIDGrado As Integer)
        Try
            With Me.GRD_Sector

                Dim _Sector As IEnumerable(Of Sector) = From taula In oDTC.Sector Where taula.ID_Producto_Grado = pIDGrado Order By taula.Descripcion Select taula

                '.GRID.DataSource = _Sector
                .M.clsUltraGrid.CargarIEnumerable(_Sector)

                .M_Editable()
                .M_TreureFocus()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Sector_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Sector.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Sector

                If Me.GRD_Grado.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Grado.GRID.ActiveRow.Cells("ID_Producto_Grado").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                Dim _Grado As New Producto_Grado
                _Grado = Me.GRD_Grado.GRID.Rows.GetItem(Me.GRD_Grado.GRID.ActiveRow.Index).listObject

                .M_AfegirFila() 'Afegim una Row al Grid

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Activo").Value = 1
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto_Grado").Value = _Grado

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Sector_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Sector.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

End Class