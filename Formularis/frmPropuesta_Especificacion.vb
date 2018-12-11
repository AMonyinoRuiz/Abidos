Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmPropuesta_Especificacion
    Dim oDTC As DTCDataContext

    Public Sub Entrada()
        Me.AplicarDisseny()
        Me.KeyPreview = False

        oDTC = New DTCDataContext(BD.Conexion)

        Call CargaGrid_Preguntes()
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#Region "Preguntes"

    Private Sub CargaGrid_Preguntes()
        Try
            With Me.GRD_Preguntas

                Dim _Preguntas As IEnumerable(Of PropuestaEspecificacion) = From taula In oDTC.PropuestaEspecificacion Order By taula.Descripcion Select taula

                '.GRID.DataSource = _Division
                .M.clsUltraGrid.CargarIEnumerable(_Preguntas)

                Call CargarCombo_Division(.GRID)


                '.GRID.DisplayLayout.Bands(0).Columns("Producto_Familia").Style = ColumnStyle.DropDownList



                .M_Editable()
                .M_TreureFocus()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Pregunta_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Preguntas.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Preguntas

                .M_ExitEditMode()
                .M_AfegirFila()

                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Codigo").Value = 0
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Activo").Value = 1

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Division(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Division) = (From Taula In oDTC.Producto_Division Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As New Producto_Division

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_Division").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_Division").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Preguntas_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Preguntas.M_Grid_InitializeRow
        Call CargarCombo_Familia(Me.GRD_Preguntas.GRID, e.Row.Cells("Producto_Familia"), e.Row.Cells("ID_Producto_Division").Value)
    End Sub

    Private Sub CargarCombo_Familia(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pCell As UltraGridCell, ByVal pIDDivision As Integer)
        Try
            If pCell Is Nothing OrElse pIDDivision = 0 Then
                Exit Sub
            End If

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Familia) = (From Taula In oDTC.Producto_Familia Where Taula.ID_Producto_Division = pIDDivision And Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Familia

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pCell.Style = ColumnStyle.DropDownList
            pCell.ValueList = Valors.Clone

            'pGrid.DisplayLayout.Bands(0).Columns("Producto_Familia").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Preguntes_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Preguntas.M_ToolGrid_ToolEliminarRow
        Try

            'If e.IsAddRow Then
            '    oLinqParte.Parte_Gastos.Remove(e.ListObject)
            '    Exit Sub
            'End If

            Dim _Pregunta As PropuestaEspecificacion = e.ListObject
            If _Pregunta.PropuestaEspecificacion_Respuesta.Count > 0 OrElse _Pregunta.Propuesta_PropuestaEspecificacion.Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar esta pregunta, la pregunta está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_Preguntes_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Preguntas.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_Preguntas_M_GRID_AfterSelectChange(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles GRD_Preguntas.M_GRID_AfterSelectChange
        If Me.GRD_Preguntas.GRID.Selected.Rows.Count = 0 Then
            Call CargaGrid_Respostes(0)
        End If
    End Sub

    Private Sub GRD_Preguntas_M_GRID_ClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Preguntas.M_GRID_ClickRow
        If Me.GRD_Preguntas.GRID.Selected.Rows.Count = 1 Then
            If Me.GRD_Preguntas.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Preguntas.GRID.ActiveRow.Cells("ID_PropuestaEspecificacion").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                Exit Sub
            End If

            Dim _Pregunta As New PropuestaEspecificacion
            _Pregunta = Me.GRD_Preguntas.GRID.Rows.GetItem(Me.GRD_Preguntas.GRID.ActiveRow.Index).listObject
            Dim _IDPregunta As Integer = _Pregunta.ID_PropuestaEspecificacion

            Call CargaGrid_Respostes(_IDPregunta)
        End If
    End Sub

#End Region

#Region "Respostes"

    Private Sub CargaGrid_Respostes(ByVal pIDPregunta As Integer)
        Try
            Dim _Resposta As IEnumerable(Of PropuestaEspecificacion_Respuesta) = From taula In oDTC.PropuestaEspecificacion_Respuesta Where taula.ID_PropuestaEspecificacion = pIDPregunta Order By taula.Descripcion Select taula

            With Me.GRD_Respuestas
                '.GRID.DataSource = _Marca
                .M.clsUltraGrid.CargarIEnumerable(_Resposta)

                .M_Editable()
                .M_TreureFocus()


            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Respuesta_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Respuestas.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Respuestas

                If Me.GRD_Preguntas.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Preguntas.GRID.ActiveRow.Cells("ID_PropuestaEspecificacion").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                Dim _Pregunta As New PropuestaEspecificacion
                _Pregunta = Me.GRD_Preguntas.GRID.Rows.GetItem(Me.GRD_Preguntas.GRID.ActiveRow.Index).listObject

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("PropuestaEspecificacion").Value = _Pregunta

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Respuesta_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Respuestas.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
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

    Private Sub GRD_Respuesta_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Respuestas.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub



#End Region


End Class