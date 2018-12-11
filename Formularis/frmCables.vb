Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmCables
    Dim oDTC As DTCDataContext
    Dim oLinqCableado As Cableado


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqCableado.ID_Cableado <> 0 Then
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    ' Call Guardar()

                    oLinqCableado.Activo = False
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

            If oLinqCableado.ID_Cableado = 0 Then
                If BD.RetornaValorSQL("SELECT Count (*) From Cableado WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            Else
                If BD.RetornaValorSQL("SELECT Count (*) From Cableado WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_Cableado<>" & oLinqCableado.ID_Cableado) > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqCableado)

            If oLinqCableado.ID_Cableado = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Cableado.InsertOnSubmit(oLinqCableado)
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
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
            With oLinqCableado
                Me.TE_Codigo.Value = .Codigo
                Me.T_Descripcion.Text = .Descripcion
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pEntidad As Cableado)
        Try
            With pEntidad
                .Activo = True
                .Codigo = Me.TE_Codigo.Value
                .Descripcion = Me.T_Descripcion.Text
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqCableado = (From taula In oDTC.Cableado Where taula.ID_Cableado = pID Select taula).First
            Call SetToForm()

            Call CargaGrid_Hilos(pID)

            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Me.EstableixCaptionForm("Cable: " & (oLinqCableado.Descripcion))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqCableado = New Cableado
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        'Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing
        Me.TE_Codigo.Focus()


        Call CargaGrid_Hilos(0)

        ' Me.C_OrigenCliente.Value = 1
        ' Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Instalacion")


        ErrorProvider1.Clear()

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.T_Descripcion)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM Cableado Where Activo=1 ORDER BY Descripcion", Me.TE_Codigo, "ID_Cableado", "ID_Cableado")
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
        If Me.TE_Codigo.Value Is Nothing = False AndAlso IsDBNull(Me.TE_Codigo.Value) = False Then
            If e.KeyCode = Keys.Enter Then
                Dim ooLinQCableado As Cableado = oDTC.Cableado.Where(Function(F) F.Codigo = CInt(Me.TE_Codigo.Value)).FirstOrDefault()
                If ooLinQCableado Is Nothing = False Then
                    Call Cargar_Form(ooLinQCableado.ID_Cableado)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If
            End If
        End If
    End Sub

#End Region

#Region "Cableado"
    Private Sub CargaGrid_Hilos(ByVal pId As Integer)
        Try
            Dim Hilos As IEnumerable(Of Cableado_Hilo) = From taula In oDTC.Cableado_Hilo Where taula.ID_Cableado = pId Select taula
            With Me.GRD_Hilos

                '.GRID.DataSource = Hilos
                .M.clsUltraGrid.CargarIEnumerable(Hilos)

                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
                .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.EditAndSelectText

                .GRID.RowUpdateCancelAction = RowUpdateCancelAction.CancelUpdate
                .GRID.UpdateMode = UpdateMode.OnRowChangeOrLostFocus
                .GRID.DisplayLayout.Bands(0).Columns("Color").Nullable = Nullable.Nothing
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Hilos_M_GRID_BeforeRowUpdate(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles GRD_Hilos.M_GRID_BeforeRowUpdate
        Try

            'Si la línea existia i han esborrat el contingut el tornarem a posar
            Dim Linia As New Cableado_Hilo
            Linia = e.Row.ListObject
            If Linia Is Nothing = False AndAlso Linia.Color Is Nothing OrElse Linia.Color.ToString.Length = 0 Then

                e.Row.Cells("Color").Value = e.Row.Cells("Color").OriginalValue

            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Hilos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Hilos.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Hilos

                If oLinqCableado.ID_Cableado = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cableado").Value = oLinqCableado

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Gastos_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Hilos.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqCableado.Cableado_Hilo.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _CableadoHilo As Cableado_Hilo = e.ListObject
            If _CableadoHilo.Instalacion_CableadoMontaje_Hilo.Count > 0 Then
                Mensaje.Mostrar_Mensaje("No se puede eliminar el hilo porque esta en uso", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_Hilos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Hilos.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub
#End Region


End Class