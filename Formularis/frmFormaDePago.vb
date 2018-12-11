Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmFormaDePago
    Dim oDTC As DTCDataContext
    Dim oLinqFormaPago As FormaPago


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqFormaPago.ID_FormaPago <> 0 Then
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    ' Call Guardar()

                    oLinqFormaPago.Activo = False
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
            oDTC = New DTCDataContext(BD.Conexion) 'Fem això aqui ja que el cargar combo de tipo requereix que hi hagi un odtc creat
            Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
            Util.Cargar_Combo(Me.C_Tipo, "Select ID_FormaPago_Tipo, Descripcion From FormaPago_Tipo", True, False)
            Util.Cargar_Combo(Me.C_CuentaBancaria, "Select ID_Empresa_CuentaBancaria, NombreBanco From Empresa_CuentaBancaria", True, False)
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

            If oLinqFormaPago.ID_FormaPago = 0 Then
                If BD.RetornaValorSQL("SELECT Count (*) From FormaPago WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            Else
                If BD.RetornaValorSQL("SELECT Count (*) From FormaPago WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_FormaPago<>" & oLinqFormaPago.ID_FormaPago) > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqFormaPago)

            If oLinqFormaPago.ID_FormaPago = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.FormaPago.InsertOnSubmit(oLinqFormaPago)
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If


            Dim _FormaPago As FormaPago
            If oLinqFormaPago.Predeterminada = True Then  'Si aquest és el magatzem predetrerminat mirarem si ni ha algun altre que tb ho sigui, i si hi és el despredeterminarem
                _FormaPago = oDTC.FormaPago.Where(Function(F) F.ID_FormaPago <> oLinqFormaPago.ID_FormaPago And F.Predeterminada = True).FirstOrDefault
                If _FormaPago Is Nothing = False Then
                    _FormaPago.Predeterminada = False
                End If
                oDTC.SubmitChanges()
            End If

            'Aqui comprovem si hi ha algun magatzem predeterminat, si no ni ha cap li assigarem a aquest el predeterminat ja que en te que haver un
            _FormaPago = oDTC.FormaPago.Where(Function(F) F.Predeterminada = True).FirstOrDefault
            If _FormaPago Is Nothing = True Then
                oLinqFormaPago.Predeterminada = True
            End If
            oDTC.SubmitChanges()



            'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqFormaPago
                Me.TE_Codigo.Value = .Codigo
                Me.T_Descripcion.Text = .Descripcion
                Me.R_Condiciones.pText = .Condiciones
                Me.C_Tipo.Value = .ID_FormaPago_Tipo
                Me.CH_Predeterminada.Checked = .Predeterminada
                If .ID_Empresa_CuentaBancaria.HasValue = True Then
                    Me.C_CuentaBancaria.Value = .ID_Empresa_CuentaBancaria
                Else
                    Me.C_CuentaBancaria.SelectedIndex = -1
                End If

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pEntidad As FormaPago)
        Try
            With pEntidad
                .Activo = True
                .Codigo = Me.TE_Codigo.Value
                .Descripcion = Me.T_Descripcion.Text
                .Condiciones = Me.R_Condiciones.pText
                .FormaPago_Tipo = oDTC.FormaPago_Tipo.Where(Function(F) F.ID_FormaPago_Tipo = CInt(Me.C_Tipo.Value)).FirstOrDefault
                .Predeterminada = Me.CH_Predeterminada.Checked
                If Me.C_CuentaBancaria.Value = 0 Then
                    .Empresa_CuentaBancaria = Nothing
                Else
                    .Empresa_CuentaBancaria = oDTC.Empresa_CuentaBancaria.Where(Function(F) F.ID_Empresa_CuentaBancaria = CInt(Me.C_CuentaBancaria.Value)).FirstOrDefault
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqFormaPago = (From taula In oDTC.FormaPago Where taula.ID_FormaPago = pID Select taula).FirstOrDefault
            Call SetToForm()

            Call CargaGrid_Giros(pID)
            Call CargaGrid_Uso_Clientes(pID)
            Call CargaGrid_Uso_Proveedores(pID)
            'Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Me.EstableixCaptionForm("Forma de pago: " & (oLinqFormaPago.Descripcion))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqFormaPago = New FormaPago
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        'Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing
        Me.TE_Codigo.Focus()

        Call CargaGrid_Uso_Clientes(0)
        Call CargaGrid_Uso_Proveedores(0)
        Call CargaGrid_Giros(0)

        Me.C_Tipo.Value = Nothing
        Me.C_CuentaBancaria.Value = Nothing
        Me.TAB_General.Tabs("Condiciones").Selected = True
        'Me.C_Tipo.Value = CInt(EnumCampañaEstado.Abierta)
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
                oClsControls.ControlBuit(.C_Tipo)

                If Me.C_CuentaBancaria.ReadOnly = False Then 'si està activat serà obligatoria la seva introducció
                    oClsControls.ControlBuit(.C_CuentaBancaria)
                End If
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM FormaPago Where Activo=1 ORDER BY Codigo", Me.TE_Codigo, "ID_FormaPago", "ID_FormaPago")
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
                Dim ooLinqFormaPago As FormaPago = oDTC.FormaPago.Where(Function(F) F.Codigo = Me.TE_Codigo.Text).FirstOrDefault()
                If ooLinqFormaPago Is Nothing = False Then
                    Call Cargar_Form(ooLinqFormaPago.ID_FormaPago)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If

            Else
                ' Me.TE_Codigo.Value = oDTC.FormaPago.Where(Function(F) F.Activo = True).Max(Function(F) F.Codigo) + 1
                '   Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "Grid giros"

    Private Sub CargaGrid_Giros(ByVal pId As Integer)
        Try
            Dim _Giros As IEnumerable(Of FormaPago_Giro) = From taula In oDTC.FormaPago_Giro Where taula.ID_FormaPago = pId Select taula

            With Me.GRD_Giros

                '.GRID.DataSource = _Asignacion
                .M.clsUltraGrid.CargarIEnumerable(_Giros)

                'If oLinqCampaña.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                '    .M_Editable()
                'Else
                .M_Editable()
                'End If


            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Giros_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Giros.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Giros

                If oLinqFormaPago.ID_FormaPago = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL, "")
                    Exit Sub
                End If
                'If Guardar(False) = False Then
                '    Exit Sub
                'End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FormaPago").Value = oLinqFormaPago

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Giros_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Giros.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqFormaPago.FormaPago_Giro.Remove(e.ListObject)
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

    Private Sub GRD_Giros_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Giros.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "GRD Clientes"

    Private Sub CargaGrid_Uso_Clientes(ByVal pId As Integer)
        Try
            With Me.GRD_Uso_Cliente

                '.GRID.DataSource = Hilos
                If pId <> 0 Then
                    .M.clsUltraGrid.Cargar("Select * From C_Cliente Where ID_FormaPago=" & pId & " and Activo=1 Order by Nombre", BD)
                Else
                    .GRID.DataSource = Nothing
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

    Private Sub GRD_Uso_Clientes_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Uso_Cliente.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_Uso_Cliente
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As New frmCliente
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Cliente").Value))
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

#Region "GRD Proveedores"

    Private Sub CargaGrid_Uso_Proveedores(ByVal pId As Integer)
        Try
            With Me.GRD_Uso_Proveedor

                '.GRID.DataSource = Hilos
                If pId <> 0 Then
                    .M.clsUltraGrid.Cargar("Select * From C_Proveedor Where ID_FormaPago=" & pId & " and Activo=1 Order by Nombre", BD)
                Else
                    .GRID.DataSource = Nothing
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

    Private Sub GRD_Uso_Proveedor_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Uso_Proveedor.M_ToolGrid_ToolVisualitzarDobleClickRow
        With Me.GRD_Uso_Proveedor
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If .GRID.Selected.Rows(0).Band.Index = 0 Then
                Dim frm As New frmProveedor
                frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Proveedor").Value))
                frm.FormObrir(frmPrincipal, True)
            End If
        End With
    End Sub
#End Region

    Private Sub C_Tipo_ValueChanged(sender As Object, e As EventArgs) Handles C_Tipo.ValueChanged
        If Me.C_Tipo.SelectedIndex <> -1 Then
            Dim _Tipo As FormaPago_Tipo = oDTC.FormaPago_Tipo.Where(Function(F) F.ID_FormaPago_Tipo = CInt(Me.C_Tipo.Value)).FirstOrDefault
            If _Tipo.GenerarDomiciliacion = True Then
                Me.C_CuentaBancaria.ReadOnly = False
            Else
                Me.C_CuentaBancaria.ReadOnly = True
                Me.C_CuentaBancaria.Value = Nothing
            End If
        Else
            Me.C_CuentaBancaria.ReadOnly = True
            Me.C_CuentaBancaria.Value = Nothing
        End If
    End Sub
End Class
