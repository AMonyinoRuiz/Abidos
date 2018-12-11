Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmInstalacion_TipoContrato
    Dim oDTC As DTCDataContext
    Dim oLinqTipoContrato As Instalacion_Contrato_TipoContrato


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqTipoContrato.ID_Instalacion_Contrato_TipoContrato <> 0 Then
                If oLinqTipoContrato.Instalacion_Contrato.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar, este tipo de contrato està asignado a uno o más contratos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    oDTC.Instalacion_Contrato_TipoContrato.DeleteOnSubmit(oLinqTipoContrato)
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

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqTipoContrato)

            If oLinqTipoContrato.ID_Instalacion_Contrato_TipoContrato = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Instalacion_Contrato_TipoContrato.InsertOnSubmit(oLinqTipoContrato)
                oDTC.SubmitChanges()
                Me.TE_Codigo.Text = oLinqTipoContrato.ID_Instalacion_Contrato_TipoContrato
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If

            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqTipoContrato
                Me.TE_Codigo.Value = .ID_Instalacion_Contrato_TipoContrato
                Me.T_Descripcion.Text = .Descripcion
                Me.R_Condiciones.pText = .Condiciones
                Me.T_Contador.Value = .Contador
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pEntidad As Instalacion_Contrato_TipoContrato)
        Try
            With pEntidad

                .Descripcion = Me.T_Descripcion.Text
                .Condiciones = Me.R_Condiciones.pText
                .Contador = Me.T_Contador.Value

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqTipoContrato = (From taula In oDTC.Instalacion_Contrato_TipoContrato Where taula.ID_Instalacion_Contrato_TipoContrato = pID Select taula).FirstOrDefault
            Call SetToForm()

            Me.EstableixCaptionForm("Tipo de contrato: " & (oLinqTipoContrato.Descripcion))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqTipoContrato = New Instalacion_Contrato_TipoContrato
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        'Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        Me.TE_Codigo.Value = Nothing
        Me.TE_Codigo.Focus()
        Me.T_Contador.Value = 0

        ErrorProvider1.Clear()

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                'oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.T_Descripcion)
                oClsControls.ControlBuit(.T_Contador)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM Instalacion_Contrato_TipoContrato ORDER BY Descripcion", Me.TE_Codigo, "ID_Instalacion_Contrato_TipoContrato", "ID_Instalacion_Contrato_TipoContrato")
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

    'Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        If Me.TE_Codigo.Value Is Nothing = False AndAlso IsDBNull(Me.TE_Codigo.Value) = False Then
    '            Dim ooLinqFormaPago As FormaPago = oDTC.FormaPago.Where(Function(F) F.Codigo = Me.TE_Codigo.Text).FirstOrDefault()
    '            If ooLinqFormaPago Is Nothing = False Then
    '                Call Cargar_Form(ooLinqFormaPago.ID_FormaPago)
    '            Else
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
    '                Call Netejar_Pantalla()
    '            End If

    '        Else
    '            ' Me.TE_Codigo.Value = oDTC.FormaPago.Where(Function(F) F.Activo = True).Max(Function(F) F.Codigo) + 1
    '            '   Exit Sub
    '        End If
    '    End If
    'End Sub

#End Region



End Class
