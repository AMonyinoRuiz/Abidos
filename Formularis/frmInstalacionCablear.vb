Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmInstalacionCablear
    Dim oDTC As DTCDataContext
    Dim oLinqCableadoMontaje As Instalacion_CableadoMontaje
    Dim oLinqInstalacion As Instalacion
    Dim oLinqCable As Instalacion_Cableado
    Dim RowDesti As UltraGridRow
    Dim RowsOrigen As New ArrayList

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Call GuardarGlobal(False)

    End Sub

    Private Sub ToolForm_m_ToolForm_GuardarIContinuar() Handles ToolForm.m_ToolForm_Imprimir
        Call GuardarGlobal(True)
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pInstalacion As Instalacion, ByRef pCable As Instalacion_Cableado, ByRef pDTC As DTCDataContext, Optional ByVal pId As Integer = 0)
        Try

            Me.AplicarDisseny()

            oLinqInstalacion = pInstalacion


            oLinqCable = pCable

            Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True
            Me.ToolForm.M.Botons.tImprimir.SharedProps.Caption = "Guardar y continuar"

            oDTC = pDTC

            Call CargarCombo_Cables(pInstalacion.ID_Instalacion)
            Util.Cargar_Combo(Me.C_CajaIntermedia_Origen, "SELECT ID_Instalacion_CajaIntermedia, Identificador FROM Instalacion_CajaIntermedia Where ID_Instalacion=" & pInstalacion.ID_Instalacion & " ORDER BY Identificador", False)
            Util.Cargar_Combo(Me.C_CajaIntermedia_Destino, "SELECT ID_Instalacion_CajaIntermedia, Identificador FROM Instalacion_CajaIntermedia Where ID_Instalacion=" & pInstalacion.ID_Instalacion & " ORDER BY Identificador", False)
            Util.Cargar_Combo(Me.C_FuenteAlimentacion, "SELECT ID_Instalacion_FuenteAlimentacion, Identificador FROM Instalacion_FuenteAlimentacion Where ID_Instalacion=" & pInstalacion.ID_Instalacion & " ORDER BY Identificador", False)

            Call Netejar_Pantalla()

            Me.C_Cable.Value = oLinqCable.ID_Instalacion_Cableado

            Me.GRD_Origen.M.clsToolBar.Boto_Afegir("VistaIndentada", "Ocultar vista indentada", True)
            Me.GRD_Destino.M.clsToolBar.Boto_Afegir("VistaIndentada", "Ocultar vista indentada", True)


            If pId <> 0 Then
                Call Cargar_Form(pId)

            Else
                Call CargaGrid_Origen(0)
                Call CargaGrid_Destino(0)

                Call CargaGrid_Hilos()
                '    Call ActivarPantalla(True)
                'Else
                '    Call ActivarPantalla(False)
                '    Call CargarIdentificadorDeLinea()
            End If

            Me.KeyPreview = False
            Me.TAB_Principal.Tabs("Especificaciones").Selected = True



            ' BD.EjecutarConsulta("Insert Into Instalacion_CableadoMontaje_Hilo (ID_Instalacion_CableadoMontaje, ID_Cableado_Hilo)  Select " & oLinqCableadoMontaje.ID_Instalacion_CableadoMontaje & ", ID_Cableado_Hilo From Cableado_Hilo as TBL1 Where  (Select COUNT(*) From Instalacion_CableadoMontaje_Hilo as TBL2 Where ID_Instalacion_CableadoMontaje= " & oLinqCableadoMontaje.ID_Instalacion_CableadoMontaje & " and TBL1.ID_Cableado_Hilo=TBL2.ID_Cableado_Hilo)=0")

            '            ' Me.GRD_Associacio.GRID.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Private Sub CargarCombo_Cables(ByVal pID As Integer)
        Util.Cargar_Combo(Me.C_Cable, "SELECT ID_Instalacion_Cableado, cast(Identificador as nvarchar(20)) + ' - ' + Cableado.Descripcion  FROM Instalacion_Cableado, Cableado Where Cableado.ID_Cableado=Instalacion_Cableado.ID_Cableado and ID_Instalacion=" & pID & " ORDER BY Identificador", False)
    End Sub

    Private Function Guardar(Optional ByVal pNumLineaOrigen As Integer = 0) As Boolean
        Guardar = False
        Try
            If Me.CH_CajaIntermediaOrigen.Checked = False And Me.CH_FuenteAlimentacion.Checked = False And RowsOrigen.Count = 0 Then
                Mensaje.Mostrar_Mensaje("No hay ningún origen seleccionado para el cable", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If

            If Me.CH_CajaIntermediaDestino.Checked = False And IsNothing(RowDesti) = True Then
                Mensaje.Mostrar_Mensaje("No hay ningún destino seleccionado para el cable", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If

            If Me.CH_CajaIntermediaOrigen.Checked = True AndAlso Me.C_CajaIntermedia_Origen.SelectedIndex = -1 Then
                Mensaje.Mostrar_Mensaje("Debe seleccionar un origen para el cable", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If

            If Me.CH_CajaIntermediaDestino.Checked = True AndAlso Me.C_CajaIntermedia_Destino.SelectedIndex = -1 Then
                Mensaje.Mostrar_Mensaje("Debe seleccionar un destino para el cable", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If

            If Me.CH_FuenteAlimentacion.Checked = True AndAlso Me.C_FuenteAlimentacion.SelectedIndex = -1 Then
                Mensaje.Mostrar_Mensaje("Debe seleccionar un origen para el cable", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If


            Me.GRD_Destino.GRID.ActiveRow = Nothing
            Me.GRD_Origen.GRID.ActiveRow = Nothing
            Me.GRD_Hilos.GRID.ActiveRow = Nothing

            Dim pRow As UltraGridRow
            Dim _Trobat As Boolean = False
            'Aquest bucle es per comprovar que hi ha alguna fil seleccionat
            For Each pRow In Me.GRD_Hilos.GRID.Rows
                If pRow.Cells("Seleccion").Value = True Then
                    _Trobat = True
                    Exit For
                End If
            Next

            If _Trobat = False Then
                Mensaje.Mostrar_Mensaje("No hay hilos seleccionados para el cable", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If

            Call GetFromForm(oLinqCableadoMontaje, pNumLineaOrigen)

            If oLinqCableadoMontaje.ID_Instalacion_CableadoMontaje = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                For Each pRow In Me.GRD_Hilos.GRID.Rows
                    If pRow.Cells("Seleccion").Value = True Then
                        Dim Hilo As New Instalacion_CableadoMontaje_Hilo
                        Hilo.ID_Cableado_Hilo = pRow.Cells("ID_Cableado_Hilo").Value
                        'Hilo = pRow.Cells("Cableado_Hilo").Value
                        If Me.CH_UtilizarTodosLosPares.Checked = True Then
                            Hilo.Uso = Me.T_Uso.Text
                        Else
                            Hilo.Uso = pRow.Cells("Uso").Value
                        End If

                        'Hilo.ID_Instalacion_CableadoMontaje = oLinqCableadoMontaje.ID_Instalacion_CableadoMontaje
                        'Hilo.ID_Instalacion_CableadoMontaje = oLinqCableadoMontaje.ID_Instalacion_CableadoMontaje
                        oLinqCableadoMontaje.Instalacion_CableadoMontaje_Hilo.Add(Hilo)
                    End If
                Next

                '               oDTC.Instalacion_CableadoMontaje.InsertOnSubmit(oLinqCableadoMontaje)

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

    Private Sub GuardarGlobal(ByVal pContinuar As Boolean)

        If Me.CH_CajaIntermediaOrigen.Checked = False And Me.CH_FuenteAlimentacion.Checked = False Then
            Dim i As Integer

            If RowsOrigen.Count = 0 Then
                Mensaje.Mostrar_Mensaje("Debe seleccionar un origen para el cable", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                Exit Sub
            End If

            For i = 0 To RowsOrigen.Count - 1
                oLinqCableadoMontaje = New Instalacion_CableadoMontaje
                If Guardar(i) = False Then
                    Exit Sub
                End If
            Next i

        Else
            oLinqCableadoMontaje = New Instalacion_CableadoMontaje
            If Guardar() = False Then
                Exit Sub
            End If
        End If

        If pContinuar = True Then
            Call Netejar_Pantalla()
        Else
            Call M_ToolForm1_m_ToolForm_Sortir()
        End If
    End Sub

    Private Sub SetToForm()
        Try
            With oLinqCableadoMontaje

                If .ID_Instalacion_CajaIntermedia_Origen.HasValue = True Then
                    Me.CH_CajaIntermediaOrigen.Checked = True
                    Me.C_CajaIntermedia_Origen.Value = .ID_Instalacion_CajaIntermedia_Origen
                Else
                    Me.CH_CajaIntermediaOrigen.Checked = False
                    Me.C_CajaIntermedia_Origen.SelectedIndex = -1
                End If

                If .ID_Instalacion_CajaIntermedia_Destino.HasValue = True Then
                    Me.CH_CajaIntermediaDestino.Checked = True
                    Me.C_CajaIntermedia_Destino.Value = .ID_Propuesta_Linea_Destino
                Else
                    Me.CH_CajaIntermediaDestino.Checked = False
                    Me.C_CajaIntermedia_Destino.SelectedIndex = -1
                End If

                Me.CH_UtilizarTodosLosPares.Checked = .UtilizarTodosLosPares
                Me.T_Localizacion.Text = .Localizacion
                Me.T_Uso.Text = .Uso
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pCableado As Instalacion_CableadoMontaje, Optional ByVal pNumLineaOrigen As Integer = 0)
        Try
            With pCableado


                .ID_Instalacion_Cableado = oLinqCable.ID_Instalacion_Cableado
                .Instalacion_Cableado = oLinqCable

                If Me.CH_CajaIntermediaDestino.Checked = True Then
                    .ID_Instalacion_CajaIntermedia_Destino = Me.C_CajaIntermedia_Destino.Value
                Else
                    .ID_Propuesta_Linea_Destino = RowDesti.Cells("ID_Propuesta_Linea").Value
                End If

                If Me.CH_CajaIntermediaOrigen.Checked = True Then
                    .ID_Instalacion_CajaIntermedia_Origen = Me.C_CajaIntermedia_Origen.Value
                Else
                    If Me.CH_FuenteAlimentacion.Checked = False Then
                        .ID_Propuesta_Linea_Origen = RowsOrigen(pNumLineaOrigen).Cells("ID_Propuesta_Linea").Value
                    End If
                End If

                    If Me.CH_FuenteAlimentacion.Checked = True Then
                        .ID_Instalacion_FuenteAlimentacion = Me.C_FuenteAlimentacion.Value
                    Else
                        .ID_Propuesta_Linea_Origen = RowsOrigen(pNumLineaOrigen).Cells("ID_Propuesta_Linea").Value
                    End If

                    .UtilizarTodosLosPares = Me.CH_UtilizarTodosLosPares.Checked
                    .Localizacion = Me.T_Localizacion.Text
                    .Uso = Me.T_Uso.Text

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqCableadoMontaje = oDTC.Instalacion_CableadoMontaje.Where(Function(F) F.ID_Instalacion_CableadoMontaje = pID).FirstOrDefault

            '  oLinqCableadoMontaje = (From taula In oDTC.Instalacion_CableadoMontaje Where taula.ID_Instalacion_CableadoMontaje = pID Select taula).First

            Call SetToForm()

            If oLinqCableadoMontaje.ID_Propuesta_Linea_Origen.HasValue = True Then
                Call CargaGrid_Origen(oLinqCableadoMontaje.ID_Propuesta_Linea_Origen)
            Else
                Call CargaGrid_Origen(0)
            End If
            If oLinqCableadoMontaje.ID_Propuesta_Linea_Destino.HasValue = True Then
                Call CargaGrid_Destino(oLinqCableadoMontaje.ID_Propuesta_Linea_Destino)
            Else
                Call CargaGrid_Destino(0)
            End If
            Call CargaGrid_Hilos()


            'Call CargaGrid_Accesos(pID)


            'Call Carga_Grid_Caracteristicas_Personalizadas(pID)
            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqCableadoMontaje = New Instalacion_CableadoMontaje
        ' oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.TAB_Principal.Tabs("Origen").Selected = True
        Me.C_CajaIntermedia_Origen.SelectedIndex = -1
        Me.C_CajaIntermedia_Destino.SelectedIndex = -1
        Me.C_FuenteAlimentacion.SelectedIndex = -1

        'Me.CH_CajaIntermediaDestino.Checked = False
        'Me.CH_CajaIntermediaOrigen.Checked = False
        'Me.CH_UtilizarTodosLosPares.Checked = False

        If RowDesti Is Nothing = False Then
            RowDesti.Cells("Seleccion").Value = 0
            RowDesti.Update()
        End If

        Call DesmarcarLiniesOrigen()

        Me.CH_UtilizarTodosLosPares.Checked = True
        Me.CH_UtilizarTodosLosPares.Checked = False


        'Me.T_IVA.Value = oDTC.Configuracion.FirstOrDefault.IVA

        'Call CargaGrid_Accesos(0)



        ErrorProvider1.Clear()
    End Sub

    Private Sub DesmarcarLiniesOrigen()
        For i = 0 To RowsOrigen.Count - 1
            RowsOrigen(i).cells("Seleccion").value = 0
            RowsOrigen(i).Update()
        Next

        RowDesti = Nothing
        RowsOrigen.Clear()
    End Sub

    'Private Function ComprovacioCampsRequeritsError() As Boolean
    '    Try
    '        Dim oClsControls As New clsControles(ErrorProvider1)

    '        With Me
    '            ErrorProvider1.Clear()
    '            oClsControls.ControlBuit(.TE_Producto_Codigo)
    '            oClsControls.ControlBuit(.T_Producto_Descripcion)
    '            oClsControls.ControlBuit(.T_IVA)
    '            oClsControls.ControlBuit(.T_Preu)
    '            oClsControls.ControlBuit(.T_Unidades)
    '        End With

    '        ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Function

    'Public Function DbnullToNothing(ByVal pValor As Object) As Decimal?
    '    ' DbnullToNothing = pValor
    '    Try
    '        If pValor Is Nothing = False Then
    '            If IsDBNull(pValor) = True Then
    '                Return Nothing
    '            Else
    '                Return CDbl(pValor)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

#End Region

#Region "Events Varis"
    Private Sub CH_UtilizarTodosLosPares_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_UtilizarTodosLosPares.CheckedChanged
        If CH_UtilizarTodosLosPares.Checked = True Then
            Me.GRD_Hilos.Enabled = False
            Dim pRow As UltraGridRow
            For Each pRow In Me.GRD_Hilos.GRID.Rows
                pRow.Cells("Seleccion").Value = True
                If Me.T_Uso.TextLength > 0 Then
                    pRow.Cells("Uso").Value = Me.T_Uso.Text
                End If
                pRow.Update()
            Next
        Else
            Me.GRD_Hilos.Enabled = True
            For Each pRow In Me.GRD_Hilos.GRID.Rows
                pRow.Cells("Seleccion").Value = False
                pRow.Cells("Uso").Value = ""
                pRow.Update()
            Next
        End If
    End Sub

    Private Sub CH_CajaIntermediaOrigen_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_CajaIntermediaOrigen.CheckedChanged
        If CH_CajaIntermediaOrigen.Checked = True Then
            Me.CH_FuenteAlimentacion.Checked = False
            Me.C_FuenteAlimentacion.ReadOnly = True
            Me.C_FuenteAlimentacion.SelectedIndex = -1

            Me.C_CajaIntermedia_Origen.ReadOnly = False
            Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Origen")
            Call DesmarcarLiniesOrigen()
        Else
            Me.C_CajaIntermedia_Origen.ReadOnly = True
            Util.Tab_Activar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Origen")
        End If
        Me.C_CajaIntermedia_Origen.SelectedIndex = -1
    End Sub

    Private Sub CH_CajaIntermediaDestino_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_CajaIntermediaDestino.CheckedChanged
        If CH_CajaIntermediaDestino.Checked = True Then
            Me.C_CajaIntermedia_Destino.ReadOnly = False
            Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Destino")
        Else
            Me.C_CajaIntermedia_Destino.ReadOnly = True
            Util.Tab_Activar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Destino")
        End If
        Me.C_CajaIntermedia_Destino.SelectedIndex = -1
    End Sub

    Private Sub CH_FuenteAlimentacion_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_FuenteAlimentacion.CheckedChanged
        If Me.CH_FuenteAlimentacion.Checked = True Then
            Me.CH_CajaIntermediaOrigen.Checked = False
            Me.C_CajaIntermedia_Origen.SelectedIndex = -1
            Me.C_CajaIntermedia_Origen.ReadOnly = True

            Me.C_FuenteAlimentacion.ReadOnly = False
            Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Origen")
        Else
            Me.C_FuenteAlimentacion.ReadOnly = True
            Util.Tab_Activar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Origen")

        End If
        Me.C_FuenteAlimentacion.SelectedIndex = -1
    End Sub

    'Private Sub TE_Producto_Codigo_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Producto_Codigo.EditorButtonClick
    '    If e.Button.Key = "Lupeta" Then
    '        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
    '        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto ORDER BY Descripcion", Me.TE_Producto_Codigo, "ID_Producto", "Codigo", Me.T_Producto_Descripcion, "Descripcion")

    '        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGeneric

    '    End If
    'End Sub

    'Private Sub TE_Producto_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Producto_Codigo.KeyDown
    '    If Me.TE_Producto_Codigo.Text Is Nothing = False Then
    '        If e.KeyCode = Keys.Enter Then
    '            Dim ooLinqProducto As Producto = oDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Producto_Codigo.Text).FirstOrDefault()
    '            If ooLinqProducto Is Nothing = False Then
    '                Me.TE_Producto_Codigo.Tag = ooLinqProducto.ID_Producto
    '                Me.T_Producto_Descripcion.Text = ooLinqProducto.Descripcion
    '                Call CargaPreuProducto(ooLinqProducto.ID_Producto)
    '                Call ActivarPantalla(True)
    '            Else
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
    '                Me.TE_Producto_Codigo.Value = Nothing
    '            End If
    '        End If
    '    End If
    'End Sub

    'Private Sub C_Emplazamiento_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Emplazamiento.ValueChanged
    '    If Me.C_Emplazamiento.Value Is Nothing = False Then
    '        Me.C_Planta.ReadOnly = False
    '        Me.C_Zona.ReadOnly = True
    '        Me.C_ElementosAProteger.ReadOnly = True
    '        Me.C_Abertura.ReadOnly = True
    '        Me.C_Planta.Value = Nothing
    '        Me.C_Zona.Value = Nothing
    '        Me.C_ElementosAProteger.Value = Nothing
    '        Me.C_Abertura.Value = Nothing

    '        If IsNumeric(Me.C_Emplazamiento.Value) = True Then
    '            Util.Cargar_Combo(Me.C_Planta, "SELECT ID_Instalacion_Emplazamiento_Planta, Descripcion FROM Instalacion_Emplazamiento_Planta WHERE ID_Instalacion_Emplazamiento=" & Me.C_Emplazamiento.Value & " ORDER BY Descripcion", False)
    '        End If
    '    End If
    'End Sub

    'Private Sub C_Planta_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Planta.ValueChanged
    '    If Me.C_Planta.Value Is Nothing = False Then
    '        Me.C_Zona.ReadOnly = False
    '        Me.C_ElementosAProteger.ReadOnly = True
    '        Me.C_Abertura.ReadOnly = True
    '        Me.C_Zona.Value = Nothing
    '        Me.C_ElementosAProteger.Value = Nothing
    '        Me.C_Abertura.Value = Nothing

    '        If IsNumeric(Me.C_Planta.Value) = True Then
    '            Util.Cargar_Combo(Me.C_Zona, "SELECT ID_Instalacion_Emplazamiento_Zona, Descripcion FROM Instalacion_Emplazamiento_Zona WHERE ID_Instalacion_Emplazamiento_Planta=" & Me.C_Planta.Value & " ORDER BY Descripcion", False)
    '        End If
    '    End If
    'End Sub

    'Private Sub C_Zona_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Zona.ValueChanged
    '    If Me.C_Zona.Value Is Nothing = False Then
    '        Me.C_ElementosAProteger.ReadOnly = False
    '        Me.C_Abertura.ReadOnly = False
    '        Me.C_ElementosAProteger.Value = Nothing
    '        Me.C_Abertura.Value = Nothing

    '        If IsNumeric(Me.C_Zona.Value) = True Then
    '            Util.Cargar_Combo(Me.C_ElementosAProteger, "SELECT ID_Instalacion_ElementosAProteger, Descripcion FROM Instalacion_ElementosAProteger, Instalacion_ElementosAProteger_Tipo WHERE Instalacion_ElementosAProteger.ID_Instalacion_ElementosAProteger_Tipo=Instalacion_ElementosAProteger_Tipo.ID_Instalacion_ElementosAProteger_Tipo and  ID_Instalacion_Emplazamiento_Zona=" & Me.C_Zona.Value & " ORDER BY Descripcion", False)
    '            Util.Cargar_Combo(Me.C_Abertura, "SELECT ID_Instalacion_Emplazamiento_Abertura, Descripcion FROM Instalacion_Emplazamiento_Abertura, Instalacion_Emplazamiento_Abertura_Elemento WHERE Instalacion_Emplazamiento_Abertura.ID_Instalacion_Emplazamiento_Abertura_Elemento=Instalacion_Emplazamiento_Abertura_Elemento.ID_Instalacion_Emplazamiento_Abertura_Elemento and  ID_Instalacion_Emplazamiento_Zona=" & Me.C_Zona.Value & " ORDER BY Descripcion", False)
    '        End If
    '    End If
    'End Sub
#End Region

    'Private Sub C_Abertura_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Abertura.EditorButtonClick
    '    If e.Button.Key = "Cancelar" Then
    '        If C_Abertura.Value Is Nothing = False Then 'fem això pq si s'apreta el botó cancelar i no hi ha cap registre seleccionat llavors no faci re
    '            Me.C_Abertura.Value = Nothing
    '            Me.C_ElementosAProteger.ReadOnly = False
    '        End If

    '    End If
    'End Sub

    'Private Sub C_Abertura_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Abertura.ValueChanged
    '    Me.C_ElementosAProteger.Value = Nothing
    '    Me.C_ElementosAProteger.ReadOnly = True
    'End Sub

    'Private Sub C_ElementosAProteger_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_ElementosAProteger.EditorButtonClick
    '    If e.Button.Key = "Cancelar" Then
    '        If C_ElementosAProteger.Value Is Nothing = False Then  'fem això pq si s'apreta el botó cancelar i no hi ha cap registre seleccionat llavors no faci re
    '            Me.C_ElementosAProteger.Value = Nothing
    '            Me.C_Abertura.ReadOnly = False
    '        End If
    '    End If
    'End Sub

    'Private Sub C_ElementosAProteger_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_ElementosAProteger.ValueChanged
    '    Me.C_Abertura.Value = Nothing
    '    Me.C_Abertura.ReadOnly = True
    'End Sub

    'Private Sub C_Vinculacion_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Vinculacion.EditorButtonClick
    '    Me.C_Vinculacion.Value = Nothing
    'End Sub

    'Private Sub C_Emplazamiento_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Emplazamiento.EditorButtonClick
    '    Me.C_Emplazamiento.Value = Nothing
    '    Me.C_Planta.Value = Nothing
    '    Me.C_Zona.Value = Nothing
    '    Me.C_Abertura.Value = Nothing
    '    Me.C_ElementosAProteger.Value = Nothing
    '    Me.C_Planta.ReadOnly = True
    '    Me.C_Zona.ReadOnly = True
    '    Me.C_Abertura.ReadOnly = True
    '    Me.C_ElementosAProteger.ReadOnly = True
    'End Sub

    '#Region "Accesos"

    '    Private Sub CargaGrid_Accesos(ByVal pId As Integer)
    '        Try
    '            Dim Accesos = From taula In oDTC.Propuesta_Linea_Acceso Where taula.ID_Propuesta_Linea = pId Select taula

    '            With Me.GRD_Acceso
    '                .GRID.DataSource = Accesos

    '                '.GRID.DisplayLayout.Bands(0).Columns("ID_").CellActivation = UltraWinGrid.Activation.NoEdit
    '                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
    '                .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.EditAndSelectText

    '                Call CargarCombo_TipoAcceso(.GRID)

    '                'Dim pCol As Infragistics.Win.UltraWinGrid.UltraGridColumn
    '                'For Each pCol In .GRID.DisplayLayout.Bands(0).Columns
    '                '    pCol.PerformAutoResize()
    '                'Next
    '            End With


    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '    Private Sub CargarCombo_TipoAcceso(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
    '        Try

    '            Dim Valors As New Infragistics.Win.ValueList
    '            Dim oTaula As IQueryable(Of Propuesta_Linea_TipoAcceso) = (From Taula In oDTC.Propuesta_Linea_TipoAcceso Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
    '            Dim Var As Propuesta_Linea_TipoAcceso

    '            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
    '            For Each Var In oTaula
    '                Valors.ValueListItems.Add(Var.ID_Propuesta_Linea_TipoAcceso, Var.Descripcion)
    '            Next


    '            pGrid.DisplayLayout.Bands(0).Columns("ID_Propuesta_Linea_TipoAcceso").Style = ColumnStyle.DropDownList
    '            pGrid.DisplayLayout.Bands(0).Columns("ID_Propuesta_Linea_TipoAcceso").ValueList = Valors.Clone

    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try

    '    End Sub

    '    Private Sub GRD_Acceso_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Acceso.M_ToolGrid_ToolAfegir
    '        Try

    '            With Me.GRD_Acceso
    '                If oLinqPropuesta_Linea.ID_Propuesta_Linea = 0 Then
    '                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '                    Exit Sub
    '                End If

    '                .GRID.DisplayLayout.Bands(0).AddNew() 'Afegim una Row al Grid

    '                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento").Value = IDEmplazamiento
    '                .GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Propuesta_Linea_TipoAcceso").Value = 0


    '                'Establim que la variable Linia és el mateix que la última row del grid recient afegida
    '                Dim Linia As New Propuesta_Linea_Acceso
    '                Linia = .GRID.Rows.GetItem(.GRID.Rows.Count - 1).listObject

    '                'Afegim aquesta línia a la colecció de línies del actual albarà
    '                oLinqPropuesta_Linea.Propuesta_Linea_Acceso.Add(Linia)


    '            End With
    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try
    '    End Sub

    '    Private Sub GRD_Acceso_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Acceso.M_ToolGrid_ToolEliminar
    '        Try
    '            With Me.GRD_Acceso
    '                If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
    '                    Exit Sub
    '                End If
    '                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then

    '                    Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

    '                    If .GRID.Selected.Cells.Count > 0 Then
    '                        pRow = .GRID.Selected.Cells(0).Row
    '                    Else
    '                        pRow = .GRID.Selected.Rows(0)
    '                    End If

    '                    Dim Linea As Propuesta_Linea_Acceso = pRow.ListObject

    '                    'If Linea.Instalacion_ElementosAProteger.Count > 0 Or Linea.Instalacion_Emplazamiento_Abertura.Count > 0 Or Linea.Instalacion_Emplazamiento_Construccion.Count > 0 Or Linea.Instalacion_Emplazamiento_Custodia.Count > 0 Or Linea.Instalacion_Emplazamiento_Entorno.Count > 0 Or Linea.Instalacion_Emplazamiento_HistoriaRobo.Count > 0 Or Linea.Instalacion_Emplazamiento_InfluenciaExt.Count > 0 Or Linea.Instalacion_Emplazamiento_InfluenciaInt.Count > 0 Or Linea.Instalacion_Emplazamiento_Localizacion.Count > 0 Or Linea.Instalacion_Emplazamiento_Ocupacion.Count > 0 Or Linea.Instalacion_Emplazamiento_Planta.Count > 0 Or Linea.Instalacion_Emplazamiento_Zona.Count > 0 Then
    '                    '    Mensaje.Mostrar_Mensaje("Imposible eliminar el registro, hay registros activos", M_Mensaje.Missatge_Modo.INFORMACIO)
    '                    '    Exit Sub
    '                    'End If

    '                    If Linea.ID_Propuesta_Linea_Acceso <> 0 Then
    '                        oDTC.Propuesta_Linea_Acceso.DeleteOnSubmit(Linea)
    '                    End If

    '                    pRow.Delete(False)

    '                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)

    '                End If
    '                oDTC.SubmitChanges()
    '            End With
    '        Catch ex As Exception
    '            Mensaje.Mostrar_Mensaje_Error(ex)
    '        End Try

    '    End Sub

    '    Private Sub GRD_Acesso_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Acceso.M_GRID_AfterRowUpdate
    '        oDTC.SubmitChanges()
    '    End Sub

    '#End Region


#Region "Grid Origen"
    Public Sub CargaGrid_Origen(ByVal pIDLinea As Integer)
        Try
            Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
            If _Propuesta Is Nothing = False Then
                With Me.GRD_Origen
                    '                 If .ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Ocultar vista indentada" Then
                    Me.AccessibleName = Nothing
                    Dim DTS As New DataSet

                    BD.CargarDataSet(DTS, "Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado  From C_Propuesta_Linea Where Activo=1 and ID_Propuesta_Linea_Vinculado is null and ID_Propuesta=" & _Propuesta.ID_Propuesta)
                    Dim i As Integer = 0
                    Dim Fin As Integer

                    If _Propuesta.NivelMaximoLineas.HasValue = True Then
                        Fin = _Propuesta.NivelMaximoLineas - 1
                    Else
                        Fin = 5
                    End If


                    For i = 0 To Fin
                        BD.CargarDataSet(DTS, "Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado  From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & _Propuesta.ID_Propuesta, "Propuesta_Linea" & i + 2, i, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                    Next

                    .M.clsUltraGrid.Cargar(DTS, 1)

                    .GRID.DisplayLayout.Bands(0).ColumnFilters.ClearAllFilters()
                    .GRID.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows
                    .GRID.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons
                    '               Else
                    Me.AccessibleName = "NoIndentatSeInstalo"
                    .M.clsUltraGrid.Cargar("Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado  From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & _Propuesta.ID_Propuesta, BD)
                    Me.AccessibleName = Nothing
                    .GRID.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow
                    .GRID.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
                    .GRID.DisplayLayout.Override.RowSelectorWidth = 16
                    ' End If

                End With
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

        'Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        'If _Propuesta Is Nothing = False Then
        '    Dim DTS As New DataSet
        '    BD.CargarDataSet(DTS, "Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado  From C_Propuesta_Linea Where Activo=1 and ID_Propuesta_Linea_Vinculado is null and ID_Propuesta=" & _Propuesta.ID_Propuesta)
        '    Dim i As Integer = 0
        '    Dim Fin As Integer

        '    If _Propuesta.NivelMaximoLineas.HasValue = True Then
        '        Fin = _Propuesta.NivelMaximoLineas - 1
        '    Else
        '        Fin = 5
        '    End If

        '    For i = 0 To Fin
        '        BD.CargarDataSet(DTS, "Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado  From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & _Propuesta.ID_Propuesta, "Propuesta_Linea" & i + 2, i, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
        '    Next
        '    Me.GRD_Origen.M.clsUltraGrid.Cargar(DTS)

        'End If
    End Sub

    Private Sub GRD_Origen_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Origen.M_Grid_InitializeRow

        If e.Row.Cells("Utilizado").Value = True Then
            Me.GRD_Origen.M.clsUltraGrid.FilaFondoColor(e.Row, Color.LightGray)
        End If

        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_Origen.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
        e.Row.Cells("Seleccion").Column.CellClickAction = CellClickAction.CellSelect

    End Sub

    Private Sub GRD_Origen_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Origen.M_GRID_AfterCellActivate
        With GRD_Origen.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                If .ActiveCell.Value = True Then
                    .ActiveCell.Value = False
                    RowsOrigen.Remove(.ActiveCell.Row)
                Else
                    .ActiveCell.Value = True
                    RowsOrigen.Add(.ActiveCell.Row)
                End If
            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
        End With
    End Sub

    Private Sub GRD_Origen_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Origen.M_ToolGrid_ToolClickBotonsExtras
        If oLinqInstalacion.ID_Instalacion = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        Select Case e.Tool.Key
            Case "VistaIndentada"
                'If Me.GRD_Origen.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Vista indentada" Then
                '    Me.GRD_Origen.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Ocultar vista indentada"
                'Else
                '    Me.GRD_Origen.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Vista indentada"
                'End If

                If oLinqCableadoMontaje.ID_Propuesta_Linea_Origen.HasValue = True Then
                    Call CargaGrid_Origen(oLinqCableadoMontaje.ID_Propuesta_Linea_Origen)
                Else
                    Call CargaGrid_Origen(0)
                End If


        End Select
    End Sub
#End Region

#Region "Grid Destino"
    Public Sub CargaGrid_Destino(ByVal pIDLinea As Integer)
        Try
            Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
            If _Propuesta Is Nothing = False Then
                With Me.GRD_Destino
                    '   If .ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Ocultar vista indentada" Then
                    Me.AccessibleName = Nothing
                    Dim DTS As New DataSet

                    BD.CargarDataSet(DTS, "Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado  From C_Propuesta_Linea Where Activo=1 and ID_Propuesta_Linea_Vinculado is null and ID_Propuesta=" & _Propuesta.ID_Propuesta)
                    Dim i As Integer = 0
                    Dim Fin As Integer

                    If _Propuesta.NivelMaximoLineas.HasValue = True Then
                        Fin = _Propuesta.NivelMaximoLineas - 1
                    Else
                        Fin = 5
                    End If


                    For i = 0 To Fin
                        BD.CargarDataSet(DTS, "Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado  From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & _Propuesta.ID_Propuesta, "Propuesta_Linea" & i + 2, i, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                    Next

                    .M.clsUltraGrid.Cargar(DTS, 1)

                    .GRID.DisplayLayout.Bands(0).ColumnFilters.ClearAllFilters()
                    .GRID.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows
                    .GRID.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons
                    '      Else
                    Me.AccessibleName = "NoIndentatSeInstalo"
                    .M.clsUltraGrid.Cargar("Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado  From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & _Propuesta.ID_Propuesta, BD)
                    Me.AccessibleName = Nothing
                    .GRID.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow
                    .GRID.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
                    .GRID.DisplayLayout.Override.RowSelectorWidth = 16
                    '        End If

                End With
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try













        'Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        'If _Propuesta Is Nothing = False Then
        '    Dim DTS As New DataSet
        '    BD.CargarDataSet(DTS, "Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is null and ID_Propuesta=" & _Propuesta.ID_Propuesta)
        '    Dim i As Integer = 0
        '    Dim Fin As Integer

        '    If _Propuesta.NivelMaximoLineas.HasValue = True Then
        '        Fin = _Propuesta.NivelMaximoLineas - 1
        '    Else
        '        Fin = 5
        '    End If

        '    For i = 0 To Fin
        '        BD.CargarDataSet(DTS, "Select *, cast(case ID_Propuesta_Linea When " & pIDLinea & " then 1 Else 0 end as bit) as Seleccion, cast(case(select Count(*) From [Instalacion_CableadoMontaje] Where ID_Propuesta_Linea_Origen=ID_Propuesta_Linea or ID_Propuesta_linea_Destino=ID_Propuesta_Linea) when 0 then 0 else 1 end as bit) as Utilizado From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & _Propuesta.ID_Propuesta, "Propuesta_Linea" & i + 2, i, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
        '    Next
        '    Me.GRD_Destino.M.clsUltraGrid.Cargar(DTS)
        'End If
    End Sub

    Private Sub GRD_Destino_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Destino.M_GRID_AfterCellActivate
        With GRD_Destino.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                If .ActiveCell.Value = True Then
                    .ActiveCell.Value = False
                    RowDesti.Update()
                    RowDesti = Nothing
                Else
                    .ActiveCell.Value = True
                    If RowDesti Is Nothing = False Then
                        RowDesti.Cells("Seleccion").Value = False
                        RowDesti.Update()
                    End If
                    RowDesti = .ActiveCell.Row
                End If

            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
        End With
    End Sub

    Private Sub GRD_Destino_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Destino.M_Grid_InitializeRow
        If e.Row.Cells("Utilizado").Value = True Then
            Me.GRD_Destino.M.clsUltraGrid.FilaFondoColor(e.Row, Color.LightGray)
        End If

        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_Destino.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
        e.Row.Cells("Seleccion").Column.CellClickAction = CellClickAction.CellSelect
    End Sub

    Private Sub GRD_Destino_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Destino.M_ToolGrid_ToolClickBotonsExtras
        If oLinqInstalacion.ID_Instalacion = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        Select Case e.Tool.Key
            Case "VistaIndentada"
                'If Me.GRD_Destino.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Vista indentada" Then
                '    '  Me.GRD_Destino.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Ocultar vista indentada"
                'Else
                '    '   Me.GRD_Destino.ToolGrid.Tools("VistaIndentada").SharedProps.Caption = "Vista indentada"
                'End If

                If oLinqCableadoMontaje.ID_Propuesta_Linea_Origen.HasValue = True Then
                    Call CargaGrid_Destino(oLinqCableadoMontaje.ID_Propuesta_Linea_Destino)
                Else
                    Call CargaGrid_Destino(0)
                End If


        End Select
    End Sub
#End Region

#Region "Grid Hilos"

    Private Sub CargaGrid_Hilos()
        Try
            Call CrearHilos()

            With Me.GRD_Hilos

                Dim _Hilos As IEnumerable(Of Instalacion_CableadoMontaje_Hilo) = oLinqCableadoMontaje.Instalacion_CableadoMontaje_Hilo

                .M.clsUltraGrid.CargarIEnumerable(_Hilos)
                '.GRID.DataSource = _Hilos

                .GRID.DisplayLayout.Bands(0).Columns("ID_Cableado_Hilo").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Utilizado").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
                .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.EditAndSelectText

                Call CargarCombo_Color(.GRID)


                oDTC.SubmitChanges()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CrearHilos()
        Try

            Dim _Hilo As Cableado_Hilo

            For Each _Hilo In oDTC.Cableado_Hilo.Where(Function(F) F.ID_Cableado = oLinqCable.ID_Cableado)
                Dim _HiloMontado As New Instalacion_CableadoMontaje_Hilo
                _HiloMontado.ID_Cableado_Hilo = _Hilo.ID_Cableado_Hilo
                _HiloMontado.Seleccion = False
                Dim CuentaUsos As Integer = oDTC.Instalacion_CableadoMontaje_Hilo.Where(Function(F) F.ID_Cableado_Hilo = _Hilo.ID_Cableado_Hilo And F.Instalacion_CableadoMontaje.ID_Instalacion_Cableado = oLinqCable.ID_Instalacion_Cableado).Count
                If CuentaUsos = 0 Then
                    _HiloMontado.Utilizado = False
                Else
                    _HiloMontado.Utilizado = True
                End If
                oLinqCableadoMontaje.Instalacion_CableadoMontaje_Hilo.Add(_HiloMontado)
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Color(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Cableado_Hilo) = (From Taula In oDTC.Cableado_Hilo Order By Taula.ID_Cableado_Hilo Select Taula)
            Dim Var As Cableado_Hilo

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Cableado_Hilo, Var.Color)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("ID_Cableado_Hilo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("ID_Cableado_Hilo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Hilos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Hilos.M_GRID_AfterRowUpdate
        '     oDTC.SubmitChanges()
    End Sub

#End Region

End Class