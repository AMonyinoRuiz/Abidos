Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmReceptora
    Dim oDTC As DTCDataContext
    Dim oLinqReceptora As Receptora
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqReceptora.ID_Receptora <> 0 Then
                If Mensaje.Mostrar_Mensaje("Desea dar de alta/baja el registro?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then

                    ' Call Guardar()

                    ' oLinqCliente.Activo = False
                    If DT_Baja.Value Is Nothing Then
                        Me.DT_Baja.Value = Date.Now
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                    Else
                        Me.DT_Baja.Value = Nothing
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                    End If
                    Call Guardar()

                    'oDTC.SubmitChanges()
                    'Call Netejar_Pantalla()
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

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Cliente_Archivo", 1)

            Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
            Call Netejar_Pantalla()

            ''            'Call Cargar_Combo_Associacions() pels grids

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

            If oLinqReceptora.ID_Receptora = 0 Then
                If BD.RetornaValorSQL("SELECT Count (*) From Receptora WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            Else
                If BD.RetornaValorSQL("SELECT Count (*) From Receptora WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_Receptora<>" & oLinqReceptora.ID_Receptora) > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqReceptora)

            If oLinqReceptora.ID_Receptora = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Receptora.InsertOnSubmit(oLinqReceptora)
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeGuardar)
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
            With oLinqReceptora
                Me.TE_Codigo.Value = .Codigo
                Me.T_AccesoWeb.Text = .AccesoWeb
                Me.T_Contraseña.Text = .Contraseña
                Me.T_DescripcionCRA1.Text = .TelefonoCra1_Descripcion
                Me.T_DescripcionCRA2.Text = .TelefonoCra2_Descripcion
                Me.T_DescripcionIP1.Text = .IP1_Descripcion
                Me.T_DescripcionIP2.Text = .IP2_Descripcion
                Me.T_Direccion.Text = .Direccion
                Me.T_Poblacion.Text = .Poblacion
                Me.T_Provincia.Text = .Provincia

                Me.T_Email.Text = .Email
                Me.T_IP1.Text = .IP1
                Me.T_IP2.Text = .IP2
                Me.T_Nombre.Text = .Nombre
                Me.T_Persona_Contacto.Text = .PersonaContacto
                Me.T_Telefono.Text = .TelefonoContacto
                Me.T_TelefonoCRA1.Text = .TelefonoCra1
                Me.T_TelefonoCRA2.Text = .TelefonoCra2
                Me.T_Usuario.Text = .Usuario

                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Baja.Value = .FechaBaja

                If DT_Baja.Value Is Nothing Then
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                Else
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                End If

                Me.R_Observaciones.pText = .Observaciones
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pReceptora As Receptora)
        Try
            With pReceptora


                .Codigo = Me.TE_Codigo.Value
                .AccesoWeb = Me.T_AccesoWeb.Text
                .Contraseña = Me.T_Contraseña.Text
                .TelefonoCra1_Descripcion = Me.T_DescripcionCRA1.Text
                .TelefonoCra2_Descripcion = Me.T_DescripcionCRA2.Text
                .IP1_Descripcion = Me.T_DescripcionIP1.Text
                .IP2_Descripcion = Me.T_DescripcionIP2.Text
                .Direccion = Me.T_Direccion.Text
                .Poblacion = Me.T_Poblacion.Text
                .Provincia = Me.T_Provincia.Text

                .Email = Me.T_Email.Text
                .IP1 = Me.T_IP1.Text
                .IP2 = Me.T_IP2.Text
                .Nombre = Me.T_Nombre.Text
                .PersonaContacto = Me.T_Persona_Contacto.Text
                .TelefonoContacto = Me.T_Telefono.Text
                .TelefonoCra1 = Me.T_TelefonoCRA1.Text
                .TelefonoCra2 = Me.T_TelefonoCRA2.Text
                .Usuario = Me.T_Usuario.Text
                .FechaAlta = Me.DT_Alta.Value
                .FechaBaja = Me.DT_Baja.Value

                If .FechaBaja.HasValue = True Then
                    .Activo = False
                Else
                    .Activo = True
                End If

                .Observaciones = Me.R_Observaciones.pText

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqReceptora = (From taula In oDTC.Receptora Where taula.ID_Receptora = pID Select taula).First
            Call SetToForm()

            Fichero.Cargar_GRID(pID)

            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Me.EstableixCaptionForm("Receptora: " & (oLinqReceptora.Nombre))
            Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeCargar)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqReceptora = New Receptora
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing

        Me.DT_Alta.Value = Now.Date
        Me.DT_Baja.Value = Nothing

        Fichero.Cargar_GRID(0)

        Call ActivarSegonsEstatPantalla(EnumEstatPantalla.Nuevo)
        Me.TAB_Principal.Tabs("General").Selected = True
        ErrorProvider1.Clear()

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.T_Nombre)
                'oClsControls.ControlBuit(.T_Persona_Contacto)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM Receptora ORDER BY Nombre", Me.TE_Codigo, "ID_Receptora", "Codigo") ' Where Activo=1
        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat

        'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
        '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
        '        pRow.Appearance.BackColor = Color.LightCoral
        '    End If
        'Next
    End Sub

    Private Sub AlTancarLlistat(ByVal pID As String)
        Try
            Call Cargar_Form(pID)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub ActivarSegonsEstatPantalla(ByVal Estat As EnumEstatPantalla)
        Select Case Estat
            Case EnumEstatPantalla.Nuevo
                Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General")
            Case EnumEstatPantalla.DespresDeCargar
                Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)
            Case EnumEstatPantalla.DespresDeGuardar
                Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)
        End Select
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
                Dim ooLinqReceptora As Receptora = oDTC.Receptora.Where(Function(F) F.Codigo = CInt(Me.TE_Codigo.Value)).FirstOrDefault()
                If ooLinqReceptora Is Nothing = False Then
                    Call Cargar_Form(ooLinqReceptora.ID_Receptora)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If
            End If
        End If
    End Sub

#End Region



End Class