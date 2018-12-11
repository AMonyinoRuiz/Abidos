Imports Excel = Microsoft.Office.Interop.Excel


Public Class frmManteniment
    Implements IDisposable

    Dim DT As New DataTable
    Dim oDA As SqlClient.SqlDataAdapter
    Dim oDTC As DTCDataContext

    Public Sub Entrada(ByVal pNoServeixDeRe As Integer, ByVal pIDMestre As Integer)
        Me.AplicarDisseny()
        Util.Cargar_Combo(Me.C_Taula, "SELECT Tabla, Descripcion From Maestro order by Descripcion", False, False)
        Me.KeyPreview = False
        Me.AccessibleName = Me.C_Taula.Value

        Dim _IDMestre As Integer = pIDMestre
        oDTC = New DTCDataContext(BD.Conexion)
        Dim _Mestre As Maestro = oDTC.Maestro.Where(Function(F) F.ID_Maestro = _IDMestre).FirstOrDefault
        Me.C_Taula.SelectedIndex = Util.Buscar_en_Combo(Me.C_Taula, _Mestre.Tabla)

        'Me.ToolForm.M.clsToolBar.Afegir_Boto("Eliminar Propuesta")
    End Sub

    Private Sub C_Taula_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles C_Taula.SelectionChanged
        Try
            Dim oCmd As New SqlClient.SqlCommand
            Dim oCmdBuilder As New SqlClient.SqlCommandBuilder


            DT = New DataTable
            With oCmd
                .CommandType = CommandType.Text
                .CommandText = "SELECT * From " & Me.C_Taula.Value
                .Connection = BD.Conexion
                .CommandTimeout = 1500
                .Transaction = BD.Transaccion
            End With
            oDA = New SqlClient.SqlDataAdapter(oCmd)
            oDA.Fill(DT)
            oCmdBuilder = New SqlClient.SqlCommandBuilder(oDA)
            oDA.UpdateCommand = oCmdBuilder.GetUpdateCommand
            Me.AccessibleName = Me.C_Taula.Value
            Me.GRD.M.clsUltraGrid.Cargar(DT)
            Me.GRD.GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
            Me.GRD.GRID.DisplayLayout.Bands(0).Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText

            Select Case Me.C_Taula.Value
                Case "Producto_Familia"
                    Call Cargar_Producto_Division()
                    Call Cargar_Producto_Familia_Simbolo()
                Case "Producto_Subfamilia"
                    Call Cargar_Producto_Familia()
                    Call Cargar_Producto_SubFamilia_Traspaso()
                    Call Cargar_Producto_SubFamilia_Tipo()
                Case "Producto_Caracteristica"
                    Call Cargar_Producto_Familia()
                Case "Producto_Caracteristica_Instalacion"
                    Call Cargar_Producto_Familia()
                Case "Informe_Apartado"
                    Call Cargar_Informe()
                Case "Campaña_Cliente_Division_Respuesta"
                    Call Cargar_Producto_Division()
                Case "Parte_Cuestionario_Preguntas"
                    Call Cargar_Parte_Tipo()
                Case "Propuesta_EstadoCRM"
                    Call Cargar_Propuesta_Estado()
                Case "Delegacion"
                    Call Cargar_Delegacion()
                Case "Entrada_Tipo"
                    Me.GRD.M.clsToolBar.Botons.tGridAfegir.SharedProps.Enabled = False
                    Me.GRD.M.clsToolBar.Botons.tGridEliminar.SharedProps.Enabled = False
                    Me.GRD.GRID.DisplayLayout.Bands(0).Columns("Descripcion").CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled
                    Me.GRD.GRID.DisplayLayout.Bands(0).Columns("Tipo").CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled
            End Select


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Producto_Division()
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim Result = From taula In oDTC.Producto_Division Where taula.Activo = True Select taula Order By taula.Descripcion Ascending

            For Each Producto_Division In Result
                Valors.ValueListItems.Add(Producto_Division.ID_Producto_Division, Producto_Division.Descripcion)
            Next

            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Division").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Division").ValueList = Valors.Clone
            ' Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cargar_Producto_Familia_Simbolo()
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim Result = From taula In oDTC.Producto_Familia_Simbolo Where taula.Activo = True Select taula Order By taula.Codigo Ascending

            For Each Producto_Familia_Simbolo In Result
                Valors.ValueListItems.Add(Producto_Familia_Simbolo.ID_Producto_Familia_Simbolo, Producto_Familia_Simbolo.Descripcion)
            Next

            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Familia_Simbolo").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Familia_Simbolo").ValueList = Valors.Clone
            Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cargar_Producto_Familia()
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim Result = From taula In oDTC.Producto_Familia Where taula.Activo = True Select taula Order By taula.Descripcion Ascending

            For Each Producto_Familia In Result
                Valors.ValueListItems.Add(Producto_Familia.ID_Producto_Familia, Producto_Familia.Producto_Division.Descripcion & "-" & Producto_Familia.Descripcion)
            Next

            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Familia").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Familia").ValueList = Valors.Clone
            Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cargar_Producto_SubFamilia_Traspaso()
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim Result = From taula In oDTC.Producto_SubFamilia_Traspaso Where taula.Activo = True Select taula Order By taula.Codigo Ascending

            For Each Producto_SubFamilia_Traspaso In Result
                Valors.ValueListItems.Add(Producto_SubFamilia_Traspaso.ID_Producto_SubFamilia_Traspaso, Producto_SubFamilia_Traspaso.Descripcion)
            Next

            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_SubFamilia_Traspaso").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_SubFamilia_Traspaso").ValueList = Valors.Clone
            Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cargar_Producto_SubFamilia_Tipo()
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim Result = From taula In oDTC.Producto_Subfamilia_Tipo Where taula.Activo = True Select taula Order By taula.Codigo Ascending

            For Each Producto_SubFamilia_Tipo In Result
                Valors.ValueListItems.Add(Producto_SubFamilia_Tipo.ID_Producto_Subfamilia_Tipo, Producto_SubFamilia_Tipo.Descripcion)
            Next

            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_SubFamilia_Tipo").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_SubFamilia_Tipo").ValueList = Valors.Clone
            Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cargar_Informe()
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim Result = From taula In oDTC.Informe Select taula Order By taula.Descripcion Ascending

            For Each Informe In Result
                Valors.ValueListItems.Add(Informe.ID_Informe, Informe.Descripcion)
            Next

            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Informe").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Informe").ValueList = Valors.Clone
            Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cargar_Propuesta_Estado()
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim Result = From taula In oDTC.Propuesta_Estado Select taula Order By taula.Codigo Ascending

            For Each Propuesta_Estado In Result
                Valors.ValueListItems.Add(Propuesta_Estado.ID_Propuesta_Estado, Propuesta_Estado.Descripcion)
            Next

            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Propuesta_Estado").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Propuesta_Estado").ValueList = Valors.Clone
            Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cargar_Delegacion()
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim Result = From taula In oDTC.Pais Select taula Order By taula.Nombre Ascending
            Dim _Pais As Pais


            For Each _Pais In Result
                Valors.ValueListItems.Add(_Pais.ID_Pais, _Pais.Nombre)
            Next

            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Pais").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Pais").ValueList = Valors.Clone
            Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cargar_Parte_Tipo()
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim Result = From taula In oDTC.Parte_Tipo Where taula.Activo = True Select taula Order By taula.Codigo Ascending

            For Each Parte_Tipo In Result
                Valors.ValueListItems.Add(Parte_Tipo.ID_Parte_Tipo, Parte_Tipo.Descripcion)
            Next

            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Parte_Tipo").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
            Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Parte_Tipo").ValueList = Valors.Clone
            Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

            oDTC.Dispose()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_M_GRID_AfterRowUpdate(sender As Object, e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD.M_GRID_AfterRowUpdate
        Call M_ToolForm1_m_ToolForm_Guardar()
    End Sub

    Private Sub GRD_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD.M_Grid_InitializeRow
        If e.Row.Band.Columns.Exists("RO") = True Then
            If IsDBNull(e.Row.Cells("RO").Value) = False AndAlso e.Row.Cells("RO").Value = True Then
                e.Row.Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled
            End If
        End If
    End Sub

    'Private Sub Cargar_Producto_Subfamilia()
    '    Try
    '        oDTC = New DTCDataContext(BD.Conexion)
    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim Result = From taula In oDTC.Producto_SubFamilia Where taula.Activo = True Select taula Order By taula.Descripcion Ascending

    '        For Each Producto_SubFamilia In Result
    '            Valors.ValueListItems.Add(Producto_SubFamilia.ID_Producto_SubFamilia, Producto_SubFamilia.Descripcion)
    '        Next

    '        Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_SubFamilia").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
    '        Me.GRD.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_SubFamilia").ValueList = Valors.Clone
    '        Me.GRD.GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    Private Sub GRD_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD.M_ToolGrid_ToolAfegir
        Me.GRD.GRID.DisplayLayout.Bands(0).AddNew()

        If Me.GRD.GRID.DisplayLayout.Bands(0).Columns.Exists("RO") = True Then
            Me.GRD.GRID.Rows(Me.GRD.GRID.Rows.Count - 1).Cells("RO").Value = False
        End If

        If Me.GRD.GRID.DisplayLayout.Bands(0).Columns.Exists("ACtivo") = True Then
            Me.GRD.GRID.Rows(Me.GRD.GRID.Rows.Count - 1).Cells("Activo").Value = True
        End If
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Try
            Me.GRD.GRID.UpdateData()
            oDA.Update(DT)
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje("Error al eliminar el registro, el registro eliminado tiene relación con otros registros de una o más tablas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Call C_Taula_SelectionChanged(Nothing, Nothing)
            ' Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub B_EliminarPropuesta_Click(sender As System.Object, e As System.EventArgs) Handles B_EliminarPropuesta.Click
        Dim Resposta As String = Mensaje.Mostrar_Entrada_Datos("Introduce el ID de propuesta a eliminar", , False, False)
        If Resposta Is Nothing OrElse Resposta.Length = 0 Then
            Exit Sub
        End If

        Dim IDPropuesta As Integer
        If IsNumeric(Resposta) = False Then
            Mensaje.Mostrar_Mensaje("El ID de propuesta siempre es numérico", M_Mensaje.Missatge_Modo.INFORMACIO)
            Exit Sub
        End If

        IDPropuesta = Resposta
        Dim oDTC As New DTCDataContext(BD.Conexion)
        If oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = IDPropuesta).Count = 0 Then
            Mensaje.Mostrar_Mensaje("El ID de propuesta no existe", M_Mensaje.Missatge_Modo.INFORMACIO)
            Exit Sub
        End If


        Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = IDPropuesta).FirstOrDefault
        Dim _Linea As Propuesta_Linea
        Dim _Linea_Acceso As Propuesta_Linea_Acceso
        Dim _Parte_Revision As Parte_Revision


        For Each _Linea In _Propuesta.Propuesta_Linea
            For Each _Linea_Acceso In _Linea.Propuesta_Linea_Acceso
                oDTC.Propuesta_Linea_Acceso.DeleteOnSubmit(_Linea_Acceso)
            Next
            For Each _Parte_Revision In _Linea.Parte_Revision
                oDTC.Parte_Revision.DeleteOnSubmit(_Parte_Revision)
            Next
            oDTC.Propuesta_Linea.DeleteOnSubmit(_Linea)
        Next

        Dim _Plano As Propuesta_Plano
        Dim _Plano_Elementos As Propuesta_Plano_ElementosIntroducidos
        'Dim _PlanoBinario As PlanoBinario
        For Each _Plano In _Propuesta.Propuesta_Plano
            For Each _Plano_Elementos In _Plano.Propuesta_Plano_ElementosIntroducidos
                oDTC.Propuesta_Plano_ElementosIntroducidos.DeleteOnSubmit(_Plano_Elementos)
            Next
            oDTC.PlanoBinario.DeleteOnSubmit(_Plano.PlanoBinario)
            oDTC.Propuesta_Plano.DeleteOnSubmit(_Plano)
        Next


        '_Propuesta_Archivo.
        'For Each _Propuesta_Archivo In 

        'Next


        _Propuesta.Instalacion.ID_Propuesta = Nothing
        oDTC.Propuesta.DeleteOnSubmit(_Propuesta)
        oDTC.SubmitChanges()
        Mensaje.Mostrar_Mensaje("Propuesta eliminada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
    End Sub


    Private Sub B_Recalcular_Estados_Parte_Click(sender As Object, e As EventArgs) Handles B_Recalcular_Estados_Parte.Click
        Try
            Dim _DTC As New DTCDataContext(BD.Conexion)
            Dim _LlistatPartes As IEnumerable(Of Parte) = _DTC.Parte.Where(Function(F) F.ID_Parte_Estado = EnumParteEstado.EnCurso Or F.ID_Parte_Estado = EnumParteEstado.Pendiente)
            Dim _Parte As Parte
            For Each _Parte In _LlistatPartes
                If _Parte.Parte_Horas.Count > 0 Or _Parte.Parte_Gastos.Count > 0 Or _Parte.Parte_Incidencia.Count > 0 Then
                    _Parte.Parte_Estado = _DTC.Parte_Estado.Where(Function(F) F.ID_Parte_Estado = CInt(EnumParteEstado.EnCurso)).FirstOrDefault
                Else
                    _Parte.Parte_Estado = _DTC.Parte_Estado.Where(Function(F) F.ID_Parte_Estado = CInt(EnumParteEstado.Pendiente)).FirstOrDefault
                End If
            Next

            _DTC.SubmitChanges()

            Mensaje.Mostrar_Mensaje("Datos actualizados correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
                DT.Dispose()

            End If
            DT = Nothing
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private Sub GRD_M_ToolGrid_ToolEliminarRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles GRD.M_ToolGrid_ToolEliminarRow
        'If e.Band.Columns.Exists("RO") = True AndAlso e.Cells("RO").Value = True Then
        '    Mensaje.Mostrar_Mensaje("Imposible eliminar, este registro es del sistema", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
        '    Exit Sub
        'End If
    End Sub

    Private Sub GRD_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD.M_ToolGrid_ToolEliminar
        If Me.GRD.GRID.ActiveRow Is Nothing Then
            Exit Sub
        End If

        If Me.GRD.GRID.DisplayLayout.Bands(0).Columns.Exists("RO") = True AndAlso Me.GRD.GRID.ActiveRow.Cells("RO").Value = True Then
            Mensaje.Mostrar_Mensaje("Imposible eliminar, este registro es del sistema", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        Me.GRD.GRID.ActiveRow.Delete(False)
        Call M_ToolForm1_m_ToolForm_Guardar()
    End Sub

    Private Sub GRD_Load(sender As Object, e As EventArgs) Handles GRD.Load

    End Sub
End Class