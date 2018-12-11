Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmParte_Reparacion
    Dim oDTC As DTCDataContext
    Dim oLinqParte As Parte
    Dim oLinqParteReparacion As Parte_Reparacion
    Dim olinqPropuestaLinea As Propuesta_Linea


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

    Public Sub Entrada(ByRef pParte As Parte, ByRef pDTC As DTCDataContext, ByVal pIDPropuestaLinea As Integer, ByVal pIdParteReparacion As Integer)

        Try

            Me.AplicarDisseny()

            oDTC = pDTC

            oLinqParte = pParte
            olinqPropuestaLinea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = pIDPropuestaLinea).FirstOrDefault

            Util.Cargar_Combo(Me.C_Personal, "Select Personal.ID_Personal, Nombre From Personal, Parte_Asignacion Where Personal.ID_Personal=Parte_Asignacion.ID_Personal and ID_Parte=" & oLinqParte.ID_Parte & " and Activo=1 Order by Nombre", False)

            Me.TE_Producto.ButtonsRight("Lupeta").Enabled = True
            Me.TE_Proveedor.ButtonsRight("Lupeta").Enabled = True

            ' Me.TE_Producto_Codigo.ButtonsRight("Lupeta").Enabled = True

            Call Netejar_Pantalla()

            'If pId <> 0 Then
            Call Cargar_Form(pIdParteReparacion)
            'Else

            'End If
            Me.KeyPreview = False


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            'Si s'esta substituint un article, i els dos articles són d'intrusion
            If Me.OP_Tipo_Reparacion.Value = EnumParteReparacionTipo.SubstitucionArticulo Then
                Dim _Producto_Substituido As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto.Tag)).FirstOrDefault
                If oLinqParteReparacion.Propuesta_Linea.Producto.ID_Producto_Division = EnumProductoDivision.Intrusion And _Producto_Substituido.ID_Producto_Division = EnumProductoDivision.Intrusion Then
                    If oLinqParteReparacion.Propuesta_Linea.Producto.ID_Producto_Grado > _Producto_Substituido.ID_Producto_Grado Then
                        If Mensaje.Mostrar_Mensaje("¿El producto substituido és de menor grado que el original, desea continuar?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                            Exit Function
                        End If
                    End If
                End If
            End If

            Call GetFromForm(oLinqParteReparacion)

            If oLinqParteReparacion.ID_Propuesta_Linea = 0 Then  ' Comprovacio per saber si es un insertar o un nou

                oDTC.Parte_Reparacion.InsertOnSubmit(oLinqParteReparacion)

                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)

            End If

            'Si hi ha una o més líneas de revisió relacionada la pasarem al estat solucionada
            Dim _LineaRevision As Parte_Revision
            For Each _LineaRevision In oLinqParteReparacion.Parte_Revision
                _LineaRevision.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.Solucionado)

            Next

            If oLinqParteReparacion.Propuesta_Linea_Mantenimiento.Count = 1 Then
                oLinqParteReparacion.Propuesta_Linea_Mantenimiento.FirstOrDefault.Parte_Revision_Estado = oDTC.Parte_Revision_Estado.Where(Function(F) F.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.Solucionado)).FirstOrDefault
            End If

            oDTC.SubmitChanges()

            If Me.OP_Tipo_Reparacion.Value = EnumParteReparacionTipo.SubstitucionArticulo Then
                olinqPropuestaLinea.Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto.Tag)).FirstOrDefault
                olinqPropuestaLinea.Descripcion = Me.TE_Producto.Text
                oDTC.SubmitChanges()
            End If

            'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqParteReparacion
                If .ID_Personal <> 0 Then
                    Me.C_Personal.Value = .ID_Personal
                End If
                Me.T_Detalle.Text = .Descripcion

                Me.OP_Tipo_Reparacion.Value = .ID_Parte_Reparacion_Tipo 'important posar aquesta línea aqui, ja que blanqueja objectes 


                If .ID_Producto <> 0 Then
                    Me.TE_Producto_Anterior.Text = .Propuesta_Linea.Descripcion
                    Me.TE_Producto_Anterior.Tag = .Propuesta_Linea.ID_Producto
                End If

                If .ID_Proveedor <> 0 Then
                    Me.TE_Proveedor.Text = .Proveedor.Nombre
                    Me.TE_Proveedor.Tag = .ID_Proveedor
                End If



                Me.DT_FechaPrevisionRecepcion.Value = .Fecha_Reparacion
                Me.DT_Fecha.Value = .Fecha

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pParteReparacion As Parte_Reparacion)
        Try
            With pParteReparacion

                .Parte = oLinqParte
                .Propuesta_Linea = olinqPropuestaLinea

                .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_Personal.Value)).FirstOrDefault
                .Descripcion = Me.T_Detalle.Text

                If IsNothing(Me.TE_Producto.Tag) = True Then
                    .Producto = Nothing
                Else
                    .Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto.Tag)).FirstOrDefault
                End If

                If IsNothing(Me.TE_Proveedor.Tag) = True Then
                    .Proveedor = Nothing
                Else
                    .Proveedor = oDTC.Proveedor.Where(Function(F) F.ID_Proveedor = CInt(Me.TE_Proveedor.Tag)).FirstOrDefault
                End If

                .Fecha = Me.DT_Fecha.Value
                .Fecha_Reparacion = Me.DT_FechaPrevisionRecepcion.Value
                .ID_Parte_Reparacion_Tipo = Me.OP_Tipo_Reparacion.Value

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try

            oLinqParteReparacion = (From taula In oDTC.Parte_Reparacion Where taula.ID_Parte_Reparacion = pID Select taula).First
            Call SetToForm()

            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqParteReparacion = New Parte_Reparacion

        Me.OP_Tipo_Reparacion.Value = EnumParteReparacionTipo.ReparacionInterna

        Util.Blanquejar_AmbDataDiaAvui(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.C_Personal.SelectedIndex = -1

        Me.C_Personal.Focus()

        ErrorProvider1.Clear()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()

                Select Case OP_Tipo_Reparacion.Value
                    Case CInt(EnumParteReparacionTipo.ReparacionInterna)
                    Case CInt(EnumParteReparacionTipo.ReparacionExterna)
                        oClsControls.ControlBuit(.TE_Proveedor)
                    Case CInt(EnumParteReparacionTipo.SubstitucionArticulo)
                        oClsControls.ControlBuit(.TE_Producto)
                End Select

                oClsControls.ControlBuit(.C_Personal)
                oClsControls.ControlBuit(.DT_Fecha)
                oClsControls.ControlBuit(.T_Detalle)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

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

    Private Sub TE_Producto_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Producto.EditorButtonClick
        Try
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto Where Activo=1 ORDER BY Descripcion", Me.TE_Producto, "ID_Producto", "Descripcion")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub TE_Proveedor_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Proveedor.EditorButtonClick
        Try
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Proveedor Where Activo=1 ORDER BY Nombre", Me.TE_Proveedor, "ID_Proveedor", "Nombre")

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
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
    '                Me.C_Traspasable.Value = ooLinqProducto.Producto_SubFamilia.ID_Producto_SubFamilia_Traspaso
    '                Call CargaPreuProductoYTraspasable(ooLinqProducto.ID_Producto)
    '                Call ActivarPantalla(True)
    '                If Me.T_Unidades.Visible = True Then
    '                    Me.T_Unidades.Focus()
    '                Else
    '                    Me.T_Producto_Descripcion.Focus()
    '                End If
    '            Else
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
    '                Me.TE_Producto_Codigo.Value = Nothing
    '            End If
    '        End If
    '    End If
    'End Sub

#End Region

    Private Sub OP_Tipo_Reparacion_ValueChanged(sender As System.Object, e As System.EventArgs) Handles OP_Tipo_Reparacion.ValueChanged
        Me.Panel_SubstitucionArticulo.Visible = False
        Me.Panel_ReparacionExterna.Visible = False
        Me.TE_Producto.Tag = Nothing
        Me.TE_Producto.Text = ""
        Me.TE_Proveedor.Tag = Nothing
        Me.TE_Proveedor.Text = ""
        Me.DT_FechaPrevisionRecepcion.Value = Nothing
        Select Case OP_Tipo_Reparacion.Value
            Case CInt(EnumParteReparacionTipo.ReparacionInterna)

            Case CInt(EnumParteReparacionTipo.ReparacionExterna)
                Me.Panel_ReparacionExterna.Visible = True
            Case CInt(EnumParteReparacionTipo.SubstitucionArticulo)
                Me.Panel_SubstitucionArticulo.Visible = True
        End Select
    End Sub


    Private Sub frmParte_Reparacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class