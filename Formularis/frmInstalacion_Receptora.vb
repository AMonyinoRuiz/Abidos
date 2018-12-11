Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmInstalacion_Receptora
    Dim oDTC As DTCDataContext
    Dim oLinqReceptora As Instalacion_Receptora
    Dim oLinqInstalacion As Instalacion
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If Guardar() = True Then
            Call M_ToolForm1_m_ToolForm_Sortir()
        End If
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pInstalacion As Instalacion, ByRef pDTC As DTCDataContext, Optional ByVal pId As Integer = 0)

        Try

            Me.AplicarDisseny()

            oLinqInstalacion = pInstalacion

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Instalacion_Receptora_Archivo", 1)

            oDTC = pDTC

            Util.Cargar_Combo(Me.C_ControlDeTest, "SELECT ID_ControlDeTest, Descripcion FROM ControlDeTest Where Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_FormatoTransmision, "SELECT ID_FormatoTransmision, Descripcion FROM FormatoTransmision Where Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_Receptora, "SELECT ID_Receptora, Nombre FROM Receptora Where Activo=1 ORDER BY Codigo", False)

            Call Netejar_Pantalla()

            If pId <> 0 Then
                Call Cargar_Form(pId)
            End If

            Me.KeyPreview = False

            '            ' Me.GRD_Associacio.GRID.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            'If oLinqProducto.ID_Producto = 0 Then
            '    If BD.RetornaValorSQL("SELECT Count (*) From Associat WHERE Nom='" & Util.APOSTROF(Me.T_Nom.Text) & "'") > 0 Then
            '        Mensaje.Mostrar_Mensaje("Impossible afegir, nom de l'associat existent", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '        Exit Function
            '    End If
            'Else
            '    If BD.RetornaValorSQL("SELECT Count (*) From Associat WHERE Nom='" & Util.APOSTROF(Me.T_Nom.Text) & "' and id_associat<>" & oLinqAssociat.ID_Associat) > 0 Then
            '        Mensaje.Mostrar_Mensaje("Impossible modificar, nom de l'associat existent", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '        Exit Function
            '    End If
            'End If

            'If Me.C_Forma_Juridica.Value = Nothing Then
            '    Mensaje.Mostrar_Mensaje("Cal informar la forma jurídica", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Exit Function
            'End If

            'If Me.C_VOTS.Value = Nothing Then
            'Mensaje.Mostrar_Mensaje("Cal informar el nº de vots", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            'Exit Function
            'End If  ' Alex 29/10/2008 No volen els bots obligatoris


            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqReceptora)

            If oLinqReceptora.ID_Instalacion_Receptora = 0 Then  ' Comprovacio per saber si es un insertar o un nou

                oLinqInstalacion.Instalacion_Receptora.Add(oLinqReceptora)
                'oDTC.Instalacion_Receptora.InsertOnSubmit(oLinqReceptora)

                oDTC.SubmitChanges()
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
            With oLinqReceptora

                Me.T_CodigoAbonado.Text = .CodigoAbonado
                Me.T_Contraseña.Text = .Contraseña
                Me.T_IP.Text = .IP
                Me.T_PalabraClave.Text = .PalabraClave
                Me.T_Preu.Value = .Precio
                Me.T_TelefonoConexion1.Text = .TelefonoConexion1
                Me.T_TelefonoConexion2.Text = .TelefonoConexion2
                Me.T_TelefonoInstalacion.Text = .TelefonoInstalacion
                Me.T_Periodo.Value = .Periodo
                Me.T_Usuario.Text = .Usuario
                Me.TE_PropuestaLinea.Text = .Propuesta_Linea.Descripcion
                Me.TE_PropuestaLinea.Tag = .ID_Propuesta_Linea
                Me.C_Receptora.Value = .ID_Receptora
                Me.C_ControlDeTest.Value = .ID_ControlDeTest
                Me.C_FormatoTransmision.Value = .ID_FormatoTransmision
                Me.CH_CustodiaLlaves.Checked = .CustodiaLlaves
                Me.CH_TransmisionImagen.Checked = .TransmisionImagen
                Me.T_Otros.Text = .Otros
                Me.DT_FechaConexion.Value = .FechaConexion
                Me.R_Observaciones.pText = .Observaciones
                Me.CH_Comunicado_Policia.Checked = .ComunicadoPolicia
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pReceptora As Instalacion_Receptora)
        Try
            With pReceptora
                .CodigoAbonado = Me.T_CodigoAbonado.Text
                .Contraseña = Me.T_Contraseña.Text
                .IP = Me.T_IP.Text
                .PalabraClave = Me.T_PalabraClave.Text
                .Precio = Util.Comprobar_NULL_Per_0_Decimal(Me.T_Preu.Value)
                .TelefonoConexion1 = Me.T_TelefonoConexion1.Text
                .TelefonoConexion2 = Me.T_TelefonoConexion2.Text
                .TelefonoInstalacion = Me.T_TelefonoInstalacion.Text
                .Periodo = Me.T_Periodo.Value
                .Usuario = Me.T_Usuario.Text
                .Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(Me.TE_PropuestaLinea.Tag)).FirstOrDefault
                .ControlDeTest = oDTC.ControlDeTest.Where(Function(F) F.ID_ControlDeTest = CInt(Me.C_ControlDeTest.Value)).FirstOrDefault
                .FormatoTransmision = oDTC.FormatoTransmision.Where(Function(F) F.ID_FormatoTransmision = CInt(Me.C_FormatoTransmision.Value)).FirstOrDefault
                .Receptora = oDTC.Receptora.Where(Function(F) F.ID_Receptora = CInt(Me.C_Receptora.Value)).FirstOrDefault
                .CustodiaLlaves = Me.CH_CustodiaLlaves.CheckedValue
                .TransmisionImagen = Me.CH_TransmisionImagen.CheckedValue
                .Otros = Me.T_Otros.Text
                .FechaConexion = Me.DT_FechaConexion.Value
                .Observaciones = Me.R_Observaciones.pText
                .ComunicadoPolicia = Me.CH_Comunicado_Policia.Checked
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqReceptora = (From taula In oDTC.Instalacion_Receptora Where taula.ID_Instalacion_Receptora = pID Select taula).First
            Call SetToForm()
            Fichero.Cargar_GRID(pID)
            Call CargaGrid_Contacto(pID)
            Call CargaGrid_OrdenLlamada(pID)


            'Call Carga_Grid_Caracteristicas_Personalizadas(pID)
            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqReceptora = New Instalacion_Receptora
        ' oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Fichero.Cargar_GRID(0)
        'Me.Tab_Principal.Tabs("Detalle").Selected = True

        Me.T_Periodo.Value = 0

        Me.C_ControlDeTest.SelectedIndex = -1
        Me.C_FormatoTransmision.SelectedIndex = -1

        Call CargaGrid_Contacto(0)
        Call CargaGrid_OrdenLlamada(0)

        Me.TE_PropuestaLinea.ButtonsRight("Lupeta").Enabled = True
        ErrorProvider1.Clear()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_PropuestaLinea)
                oClsControls.ControlBuit(.C_ControlDeTest)
                oClsControls.ControlBuit(.C_FormatoTransmision)
                oClsControls.ControlBuit(.T_TelefonoInstalacion)
                oClsControls.ControlBuit(.T_CodigoAbonado)
                oClsControls.ControlBuit(.T_Periodo)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function DbnullToNothing(ByVal pValor As Object) As Decimal?
        ' DbnullToNothing = pValor
        Try
            If pValor Is Nothing = False Then
                If IsDBNull(pValor) = True Then
                    Return Nothing
                Else
                    Return CDbl(pValor)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub AlTancarLlistatGeneric(ByVal pID As String)
        If pID Is Nothing Then
        Else

        End If

    End Sub
#End Region

#Region "Events Varis"

    Private Sub TE_PropuestaLinea_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_PropuestaLinea.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("Select C_Propuesta_Linea.* From C_Propuesta_Linea, Producto Where SeInstalo=1 and C_Propuesta_Linea.ID_Producto=Producto.ID_Producto and ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " and (Producto.Sistema_Transmision=1 or Producto.Sistema_Transmision2=1)", Me.TE_PropuestaLinea, "ID_Propuesta_Linea", "Descripcion")
        End If
    End Sub

#End Region

#Region "Contacto"

    Private Sub CargaGrid_Contacto(ByVal pId As Integer)
        Try

            With Me.GRD_Contacto

                Dim _Contactos As IEnumerable(Of Instalacion_Receptora_Contacto) = From taula In oDTC.Instalacion_Receptora_Contacto Where taula.ID_Instalacion_Receptora = pId Select taula Order By taula.Orden
                .M.clsUltraGrid.CargarIEnumerable(_Contactos)
                '.GRID.DataSource = Contactos

                .M_Editable()
                .M_TreureFocus()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contacto_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Contacto.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Contacto

                If oLinqReceptora.ID_Instalacion_Receptora = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Receptora").Value = oLinqReceptora

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Contacto_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Contacto.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqReceptora.Instalacion_Receptora_Contacto.Remove(e.ListObject)
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

    Private Sub GRD_Contacto_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Contacto.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid OrdenLlamada"

    Private Sub CargaGrid_OrdenLlamada(ByVal pId As Integer)
        Try
            With Me.GRD_OrdenLlamada

                Dim _Contactos As IEnumerable(Of Instalacion_Receptora_OrdenLlamada) = From taula In oDTC.Instalacion_Receptora_OrdenLlamada Where taula.ID_Instalacion_Receptora = pId Select taula Order By taula.Orden
                .M.clsUltraGrid.CargarIEnumerable(_Contactos)
                '.GRID.DataSource = _Contactos

                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_OrdenLlamada_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_OrdenLlamada.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_OrdenLlamada

                If oLinqReceptora.ID_Instalacion_Receptora = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If


                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Receptora").Value = oLinqReceptora

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_OrdenLlamada_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_OrdenLlamada.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqReceptora.Instalacion_Receptora_OrdenLlamada.Remove(e.ListObject)
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

    Private Sub GRD_OrdenLlamada_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_OrdenLlamada.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

End Class