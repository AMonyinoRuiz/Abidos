Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmManteniment_Familias
    Dim oDTC As DTCDataContext

    Private Sub frmManteniment_Familias_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        ' Me.GRD_Division.GRID.ActiveRow = Nothing
        ' Me.GRD_Division.GRID.Selected.Rows.Clear()
    End Sub

    Public Sub Entrada()
        Me.AplicarDisseny()
        Me.KeyPreview = False
        Me.ToolForm.M.Botons.tGuardar.SharedProps.Visible = False
        oDTC = New DTCDataContext(BD.Conexion)

        Call CargaGrid_Division()
        Call CargaGrid_Familia(0)
        Call CargaGrid_SubFamilia(0)
        Call CargaGrid_Marca(0)
        Call CargaGrid_Caracteristicas_Instalacion(0)
        Call CargaGrid_Caracteristicas_Personalizadas(0)
        Call CargaGrid_Mantenimientos(0)

        Me.GRD_Carac_Instalacion.M.clsToolBar.Boto_Afegir("Aplicar", "Aplicar a todos los artículos", True)
        Me.GRD_Mantenimientos.M.clsToolBar.Boto_Afegir("Aplicar", "Aplicar a todos los artículos", True)
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub frmManteniment_Familias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Fem això per a que no surti la divisió carregada
        Me.GRD_Division.GRID.Selected.Rows.Clear()
        Me.GRD_Division.GRID.ActiveRow = Nothing
    End Sub

#Region "Division"

    Private Sub CargaGrid_Division()
        Try
            With Me.GRD_Division

                Dim _Division As IEnumerable(Of Producto_Division) = From taula In oDTC.Producto_Division Order By taula.Descripcion Select taula

                '.GRID.DataSource = _Division
                .M.clsUltraGrid.CargarIEnumerable(_Division)

                .M_Editable()
                .M_TreureFocus()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Division_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Division.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Division

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Codigo").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Activo").Value = 1

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Division_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Division.M_ToolGrid_ToolEliminarRow
        Try

            'If e.IsAddRow Then
            '    oLinqParte.Parte_Gastos.Remove(e.ListObject)
            '    Exit Sub
            'End If

            Dim _Division As Producto_Division = e.ListObject
            If _Division.Campaña_Cliente_Division.Count > 0 OrElse _Division.Campaña_Cliente_Division_Respuesta.Count > 0 OrElse _Division.Instalacion_Contrato.Count > 0 OrElse _Division.Instalacion_Producto_Division.Count > 0 OrElse _Division.Producto.Count > 0 OrElse _Division.Producto_Familia.Count > 0 OrElse _Division.Producto_Marca.Count > 0 OrElse _Division.Propuesta_Plano.Count > 0 OrElse _Division.Proveedor_Tarifa.Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar esta división, la división está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_Division_M_GRID_AfterSelectChange(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles GRD_Division.M_GRID_AfterSelectChange
        If Me.GRD_Division.GRID.Selected.Rows.Count = 0 Then
            Call CargaGrid_Marca(0)
            Call CargaGrid_Familia(0)
            Call CargaGrid_SubFamilia(0)
        End If
    End Sub

    Private Sub GRD_Division_M_GRID_ClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Division.M_GRID_ClickRow
        If Me.GRD_Division.GRID.Selected.Rows.Count = 1 Then
            If Me.GRD_Division.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Division.GRID.ActiveRow.Cells("ID_Producto_Division").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                Exit Sub
            End If

            Dim _Division As New Producto_Division
            _Division = Me.GRD_Division.GRID.Rows.GetItem(Me.GRD_Division.GRID.ActiveRow.Index).listObject
            Dim IDDivision As Integer = _Division.ID_Producto_Division

            Call CargaGrid_Familia(IDDivision)
            Call CargaGrid_Marca(IDDivision)
        End If
    End Sub

#End Region

#Region "Familia"

    Private Sub CargaGrid_Familia(ByVal pIDDivision As Integer)
        Try
            Dim _Familia As IEnumerable(Of Producto_Familia) = From taula In oDTC.Producto_Familia Where taula.ID_Producto_Division = pIDDivision Order By taula.Descripcion Select taula

            With Me.GRD_Familia
                '.GRID.DataSource = _Familia
                .M.clsUltraGrid.CargarIEnumerable(_Familia)

                Call Cargar_Producto_Familia_Simbolo()

                .M_Editable()
                .M_TreureFocus()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Familia_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Familia.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Familia

                If Me.GRD_Division.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Division.GRID.ActiveRow.Cells("ID_Producto_Division").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                'Les dos línies d'abaix són pq si s'està editant una línea de subfamilia i s'apreta el botó d'afegir familia peta.
                Me.GRD_Subfamilia.GRID.Selected.Rows.Clear()
                Me.GRD_Subfamilia.GRID.ActiveRow = Nothing

                Dim _Division As New Producto_Division
                _Division = Me.GRD_Division.GRID.Rows.GetItem(Me.GRD_Division.GRID.ActiveRow.Index).listObject

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Codigo").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Activo").Value = 1
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto_Division").Value = _Division

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Familia_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Familia.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                Exit Sub
            End If

            Dim _Familia As Producto_Familia = e.ListObject
            If _Familia.Producto.Count > 0 OrElse _Familia.Producto_Caracteristica.Count > 0 OrElse _Familia.Producto_Caracteristica_Instalacion.Count > 0 OrElse _Familia.Producto_Mantenimiento.Count > 0 OrElse _Familia.Producto_SubFamilia.Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar la familia, la familia está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_Familia_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Familia.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_Familia_M_GRID_ClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Familia.M_GRID_ClickRow
        If Me.GRD_Familia.GRID.Selected.Rows.Count = 1 Then
            If Me.GRD_Familia.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Familia.GRID.ActiveRow.Cells("ID_Producto_Familia").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                Exit Sub
            End If

            Dim _Familia As New Producto_Familia
            _Familia = Me.GRD_Familia.GRID.Rows.GetItem(Me.GRD_Familia.GRID.ActiveRow.Index).listObject
            Dim IDFamilia As Integer = _Familia.ID_Producto_Familia

            Call CargaGrid_SubFamilia(IDFamilia)
            Call CargaGrid_Caracteristicas_Personalizadas(IDFamilia)
            Call CargaGrid_Caracteristicas_Instalacion(IDFamilia)
            Call CargaGrid_Mantenimientos(IDFamilia)
        End If
    End Sub

    Private Sub GRD_Familia_M_GRID_AfterSelectChange(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles GRD_Familia.M_GRID_AfterSelectChange
        If Me.GRD_Familia.GRID.Selected.Rows.Count = 0 Then
            Call CargaGrid_SubFamilia(0)
            Call CargaGrid_Caracteristicas_Personalizadas(0)
            Call CargaGrid_Caracteristicas_Instalacion(0)
            Call CargaGrid_Mantenimientos(0)
        End If
    End Sub

    Private Sub Cargar_Producto_Familia_Simbolo()
        Try

            Dim oDTC2 As New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            'Dim Result = From taula In oDTC2.Producto_Familia_Simbolo Where taula.Activo = True Select taula Order By taula.Codigo Ascending
            Dim Result As IQueryable(Of Producto_Familia_Simbolo) = (From Taula In oDTC.Producto_Familia_Simbolo Where Taula.Activo = True Select Taula Order By Taula.Codigo Ascending)
            Dim Var As Producto_Familia_Simbolo

            For Each Var In Result
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            With Me.GRD_Familia
                .GRID.DisplayLayout.Bands(0).Columns("Producto_Familia_Simbolo").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
                .GRID.DisplayLayout.Bands(0).Columns("Producto_Familia_Simbolo").ValueList = Valors.Clone
                .GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

#End Region

#Region "SubFamilia"

    Private Sub CargaGrid_SubFamilia(ByVal pIDFamilia As Integer)
        Try

            Dim _SubFamilia As IEnumerable(Of Producto_SubFamilia) = From taula In oDTC.Producto_SubFamilia Where taula.ID_Producto_Familia = pIDFamilia Order By taula.Descripcion Select taula

            With Me.GRD_Subfamilia
                '.GRID.DataSource = SubFamilia
                .M.clsUltraGrid.CargarIEnumerable(_SubFamilia)

                Call Cargar_Producto_SubFamilia_Traspaso()
                Call Cargar_Producto_SubFamilia_Tipo()

                .M_Editable()
                .M_TreureFocus()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_SubFamilia_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Subfamilia.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Subfamilia

                If Me.GRD_Familia.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Familia.GRID.ActiveRow.Cells("ID_Producto_Familia").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                Dim _Familia As New Producto_Familia
                _Familia = Me.GRD_Familia.GRID.Rows.GetItem(Me.GRD_Familia.GRID.ActiveRow.Index).listObject


                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Codigo").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Activo").Value = 1
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto_Familia").Value = _Familia

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Subfamilia_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Subfamilia.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                Exit Sub
            End If


            Dim _Subfamilia As Producto_SubFamilia = e.ListObject
            If _Subfamilia.Producto.Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar la subfamilia, la subfamilia está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_SubFamilia_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Subfamilia.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub Cargar_Producto_SubFamilia_Traspaso()
        Try
            Dim oDTC2 As New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_SubFamilia_Traspaso) = (From Taula In oDTC.Producto_SubFamilia_Traspaso Where Taula.Activo = True Order By Taula.Codigo Select Taula)
            Dim Var As Producto_SubFamilia_Traspaso

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            With Me.GRD_Subfamilia
                .GRID.DisplayLayout.Bands(0).Columns("Producto_SubFamilia_Traspaso").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
                .GRID.DisplayLayout.Bands(0).Columns("Producto_SubFamilia_Traspaso").ValueList = Valors.Clone
                .GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub Cargar_Producto_SubFamilia_Tipo()
        Try
            Dim oDTC2 As New DTCDataContext(BD.Conexion)
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Subfamilia_Tipo) = (From Taula In oDTC.Producto_Subfamilia_Tipo Where Taula.Activo = True Order By Taula.Codigo Select Taula)
            Dim Var As Producto_Subfamilia_Tipo

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            With Me.GRD_Subfamilia
                .GRID.DisplayLayout.Bands(0).Columns("Producto_SubFamilia_Tipo").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
                .GRID.DisplayLayout.Bands(0).Columns("Producto_SubFamilia_Tipo").ValueList = Valors.Clone
                .GRID.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

#End Region

#Region "Marca"

    Private Sub CargaGrid_Marca(ByVal pIDDivision As Integer)
        Try
            Dim _Marca As IEnumerable(Of Producto_Marca) = From taula In oDTC.Producto_Marca Where taula.ID_Producto_Division = pIDDivision Order By taula.Descripcion Select taula

            With Me.GRD_Marca
                '.GRID.DataSource = _Marca
                .M.clsUltraGrid.CargarIEnumerable(_Marca)

                .M_Editable()
                .M_TreureFocus()

                AddHandler Me.GRD_Marca.GRID.CellDataError, AddressOf GRD_Marca_CellDataError

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Marca_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Marca.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Marca

                If Me.GRD_Division.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Division.GRID.ActiveRow.Cells("ID_Producto_Division").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                Dim _Division As New Producto_Division
                _Division = Me.GRD_Division.GRID.Rows.GetItem(Me.GRD_Division.GRID.ActiveRow.Index).listObject

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Codigo").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Activo").Value = 1
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto_Division").Value = _Division

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Marca_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Marca.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Marca_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Marca.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_Marca_CellDataError(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellDataErrorEventArgs)

        ' CellDataError gets fired when the user attempts to exit the edit mode
        ' after entering an invalid value in the cell. There are several properties
        ' on the passed in event args that you can set to control the UltraGrid's
        ' behaviour.

        ' Typically ForceExit is false. The UltraGrid forces exits on cells under
        ' circumstances like when it's being disposed of. If ForceExit is true, then 
        ' the UltraGrid will ignore StayInEditMode property and exit the cell 
        ' restoring the original value ignoring the value you set to StayInEditMode
        ' property.
        If Not e.ForceExit Then
            ' Default for StayInEditMode is true. However you can set it to false to
            ' cause the grid to exit the edit mode and restore the original value. We
            ' will just leave it true for this example.
            e.StayInEditMode = True

            ' Set the RaiseErrorEvent to false to prevent the grid from raising 
            ' the error event and displaying any message.
            e.RaiseErrorEvent = False

            ' Instead display our own message.
            If Me.GRD_Marca.GRID.ActiveCell.Column.DataType Is GetType(DateTime) Then
                MessageBox.Show(Me, "Please enter a valid date.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf Me.GRD_Marca.GRID.ActiveCell.Column.DataType Is GetType(Decimal) Then
                MessageBox.Show(Me, "Please enter a valid numer.", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            ' Set the RaiseErrorEvent to false to prevent the grid from raising 
            ' the error event.
            e.RaiseErrorEvent = False
        End If

    End Sub

#End Region

#Region "Características personalizadas"

    Private Sub CargaGrid_Caracteristicas_Personalizadas(ByVal pIDFamilia As Integer)
        Try
            Dim _CP As IEnumerable(Of Producto_Caracteristica) = From taula In oDTC.Producto_Caracteristica Where taula.ID_Producto_Familia = pIDFamilia And taula.Activo = True Order By taula.Descripcion Select taula

            With Me.GRD_Carac_Personalizadas
                '.GRID.DataSource = _CP
                .M.clsUltraGrid.CargarIEnumerable(_CP)

                .M_Editable()
                .M_TreureFocus()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Carac_Personalizadas_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Carac_Personalizadas.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Carac_Personalizadas

                If Me.GRD_Familia.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Familia.GRID.ActiveRow.Cells("ID_Producto_Familia").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                Dim _Familia As New Producto_Familia
                _Familia = Me.GRD_Familia.GRID.Rows.GetItem(Me.GRD_Familia.GRID.ActiveRow.Index).listObject

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Codigo").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Activo").Value = 1
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto_Familia").Value = _Familia

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Carac_Personalizadas_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Carac_Personalizadas.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Carac_Personalizadas_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Carac_Personalizadas.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Características Instalacion"

    Private Sub CargaGrid_Caracteristicas_Instalacion(ByVal pIDFamilia As Integer)
        Try

            With Me.GRD_Carac_Instalacion

                Dim _CI As IEnumerable(Of Producto_Caracteristica_Instalacion) = From taula In oDTC.Producto_Caracteristica_Instalacion Where taula.ID_Producto_Familia = pIDFamilia And taula.Activo = True Order By taula.Descripcion Select taula

                '.GRID.DataSource = _CI
                .M.clsUltraGrid.CargarIEnumerable(_CI)

                .M_Editable()
                .M_TreureFocus()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Carac_Instalacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Carac_Instalacion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Carac_Instalacion

                If Me.GRD_Familia.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Familia.GRID.ActiveRow.Cells("ID_Producto_Familia").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                Dim _Familia As New Producto_Familia
                _Familia = Me.GRD_Familia.GRID.Rows.GetItem(Me.GRD_Familia.GRID.ActiveRow.Index).listObject

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Codigo").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Activo").Value = 1
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto_Familia").Value = _Familia

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Carac_Instalacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Carac_Instalacion.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Carac_Instalacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Carac_Instalacion.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_Carac_Instalacion_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Carac_Instalacion.M_ToolGrid_ToolClickBotonsExtras
        Try
            With Me.GRD_Carac_Instalacion
                If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible aplicar, no hay ninguna característica seleccionada", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)
                    Exit Sub
                End If

                Dim _Familia As New Producto_Familia
                _Familia = Me.GRD_Familia.GRID.Rows.GetItem(Me.GRD_Familia.GRID.ActiveRow.Index).listObject
                Dim _IDFamilia As Integer = _Familia.ID_Producto_Familia

                Dim frm As New frmManteniment_Familias_Automatismo
                frm.Entrada(_IDFamilia, .GRID.ActiveRow.Cells("ID_Producto_Caracteristica_Instalacion").Value)
                frm.FormObrir(Me, True)
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Mantenimientos"

    Private Sub CargaGrid_Mantenimientos(ByVal pIDFamilia As Integer)
        Try

            With Me.GRD_Mantenimientos

                Dim _CI As IEnumerable(Of Producto_Mantenimiento) = From taula In oDTC.Producto_Mantenimiento Where taula.ID_Producto_Familia = pIDFamilia And taula.Activo = True Order By taula.Descripcion Select taula

                '.GRID.DataSource = _CI
                .M.clsUltraGrid.CargarIEnumerable(_CI)

                .M_Editable()
                .M_TreureFocus()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Mantenimientos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Mantenimientos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Mantenimientos

                If Me.GRD_Familia.GRID.ActiveRow Is Nothing = True OrElse Me.GRD_Familia.GRID.ActiveRow.Cells("ID_Producto_Familia").Value = 0 Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
                    Exit Sub
                End If

                Dim _Familia As New Producto_Familia
                _Familia = Me.GRD_Familia.GRID.Rows.GetItem(Me.GRD_Familia.GRID.ActiveRow.Index).listObject

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Codigo").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Activo").Value = 1
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto_Familia").Value = _Familia

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Mantenimientos_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Mantenimientos.M_ToolGrid_ToolEliminarRow
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

    Private Sub GRD_Mantenimientos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Mantenimientos.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    'Private Sub GRD_Mantenimientos_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Mantenimientos.M_ToolGrid_ToolClickBotonsExtras
    '    Try
    '        With Me.GRD_Mantenimientos
    '            If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
    '                Mensaje.Mostrar_Mensaje("Imposible aplicar, no hay ninguna característica seleccionada", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)
    '                Exit Sub
    '            End If

    '            Dim _Familia As New Producto_Familia
    '            _Familia = Me.GRD_Familia.GRID.Rows.GetItem(Me.GRD_Familia.GRID.ActiveRow.Index).listObject
    '            Dim _IDFamilia As Integer = _Familia.ID_Producto_Familia

    '            Dim frm As New frmManteniment_Familias_Automatismo
    '            frm.Entrada(_IDFamilia, .GRID.ActiveRow.Cells("ID_Producto_Caracteristica_Instalacion").Value)
    '            frm.FormObrir(Me, True)
    '        End With
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub
#End Region



End Class