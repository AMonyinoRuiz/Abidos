Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmInstalacion_Contrato
    Dim oDTC As DTCDataContext
    Dim oLinqContrato As Instalacion_Contrato
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

    Private Sub ToolForm_m_ToolForm_Imprimir() Handles ToolForm.m_ToolForm_Imprimir
        If Guardar() = False Then
            Exit Sub
        End If
        Informes.ObrirInformePreparacio(oDTC, EnumInforme.InstalacionContrato, "[ID_Instalacion_Contrato] = " & oLinqContrato.ID_Instalacion_Contrato, "Contrato - " & oLinqContrato.NumeroContrato)

        '        Dim frm As New frmInformePreparacio
        '       frm.Entrada(EnumInforme.Instalacion, )
        '      frm.FormObrir(Me, False)
    End Sub


#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pInstalacion As Instalacion, ByRef pDTC As DTCDataContext, Optional ByVal pId As Integer = 0)

        Try

            Me.AplicarDisseny()

            oLinqInstalacion = pInstalacion

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Instalacion_Contrato_Archivo", 1)

            Me.ToolForm.M.clsToolBar.Botons.tImprimir.SharedProps.Visible = True
            Me.ToolForm.M.clsToolBar.Afegir_Boto("RecuperarCondiciones", "Recuperar condiciones", True)

            oDTC = pDTC

            Util.Cargar_Combo(Me.C_Division, "SELECT ID_Producto_Division, Descripcion FROM Producto_Division Where Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_TipoContrato, "SELECT ID_Instalacion_Contrato_TipoContrato, Descripcion FROM Instalacion_Contrato_TipoContrato  ORDER BY Descripcion", False)

            Call CarregarComboProductes()

            If pId <> 0 Then
                Call Cargar_Form(pId)
            Else
                Call Netejar_Pantalla()
            End If

            Me.KeyPreview = False

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

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqContrato)

            If oLinqContrato.ID_Instalacion_Contrato = 0 Then  ' Comprovacio per saber si es un insertar o un nou

                'Retorna el últim contracte del tipus seleccionat,
                If oDTC.Instalacion_Contrato.Where(Function(F) F.ID_Instalacion_Contrato_TipoContrato = oLinqContrato.ID_Instalacion_Contrato_TipoContrato).Count > 0 Then
                    Dim _UltimContracte As Integer
                    _UltimContracte = oDTC.Instalacion_Contrato.Where(Function(F) F.ID_Instalacion_Contrato_TipoContrato = oLinqContrato.ID_Instalacion_Contrato_TipoContrato).Max(Function(F) F.NumeroContrato)
                    oLinqContrato.NumeroContrato = _UltimContracte + 1
                Else
                    oLinqContrato.NumeroContrato = oDTC.Instalacion_Contrato_TipoContrato.Where(Function(F) F.ID_Instalacion_Contrato_TipoContrato = oLinqContrato.ID_Instalacion_Contrato_TipoContrato).FirstOrDefault.Contador
                End If


                oLinqInstalacion.Instalacion_Contrato.Add(oLinqContrato)
                oDTC.SubmitChanges()

                Me.T_NumeroContrato.Value = oLinqContrato.NumeroContrato

                Call Fichero.Cargar_GRID(oLinqContrato.ID_Instalacion_Contrato) 'Fem això pq la classe tingui el ID del contracte
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
            With oLinqContrato

                Me.T_NumeroContrato.Value = .NumeroContrato
                Me.C_TipoContrato.Value = .ID_Instalacion_Contrato_TipoContrato
                Me.T_Descripcion.Text = .Descripcion
                Me.C_Division.Value = .ID_Producto_Division
                Me.DT_FechaInicio.Value = .FechaInicio
                Me.DT_FechaFin.Value = .FechaFin
                Me.T_Importe.Value = .Importe
                Me.T_NombreFirmante.Text = .NombreFirmante
                Me.T_DNIFirmante.Text = .DNIFirmante
                Me.R_OtrasCondiciones.pText = .OtrasCondiciones
                Me.R_Observaciones.pText = .Observaciones

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pContrato As Instalacion_Contrato)
        Try
            With pContrato
                .NumeroContrato = Me.T_NumeroContrato.Value
                .Instalacion_Contrato_TipoContrato = oDTC.Instalacion_Contrato_TipoContrato.Where(Function(F) F.ID_Instalacion_Contrato_TipoContrato = CInt(Me.C_TipoContrato.Value)).FirstOrDefault
                .Descripcion = Me.T_Descripcion.Text
                .Producto_Division = oDTC.Producto_Division.Where(Function(F) F.ID_Producto_Division = CInt(Me.C_Division.Value)).FirstOrDefault
                .FechaInicio = Me.DT_FechaInicio.Value
                .FechaFin = Me.DT_FechaFin.Value
                .Importe = Me.T_Importe.Value
                .NombreFirmante = Me.T_NombreFirmante.Text
                .DNIFirmante = Me.T_DNIFirmante.Text
                .OtrasCondiciones = Me.R_OtrasCondiciones.pText
                .Observaciones = Me.R_Observaciones.pText
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()
            oLinqContrato = (From taula In oDTC.Instalacion_Contrato Where taula.ID_Instalacion_Contrato = pID Select taula).First
            Call SetToForm()
            Fichero.Cargar_GRID(pID)
            Call CargaGrid_Productos(pID)
            Call CargaGrid_DocumentosVinculados(pID)



            'Call Carga_Grid_Caracteristicas_Personalizadas(pID)
            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqContrato = New Instalacion_Contrato
        ' oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Fichero.Cargar_GRID(0)

        'Me.Tab_Principal.Tabs("Detalle").Selected = True

        Me.T_NumeroContrato.Value = 0

        Me.C_TipoContrato.SelectedIndex = -1
        Me.C_Division.SelectedIndex = -1

        Call CargaGrid_Productos(0)
        Call CargaGrid_DocumentosVinculados(0)

        ErrorProvider1.Clear()
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.T_NumeroContrato)
                oClsControls.ControlBuit(.C_TipoContrato)
                oClsControls.ControlBuit(.C_Division)
                oClsControls.ControlBuit(.DT_FechaInicio)
                oClsControls.ControlBuit(.DT_FechaFin)
                oClsControls.ControlBuit(.T_Descripcion)
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

    Private Sub CalcularTotalImport()
        Try
            Me.T_Importe.Value = oLinqContrato.Instalacion_Contrato_Producto.Sum(Function(F) F.Precio)
            oLinqContrato.Importe = Me.T_Importe.Value
            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Events Varis"


#End Region

#Region "Contrato"

    Private Sub CargaGrid_Productos(ByVal pId As Integer)
        Try

            With Me.GRD_Productos

                Dim _Productos As IEnumerable(Of Instalacion_Contrato_Producto) = From taula In oDTC.Instalacion_Contrato_Producto Where taula.ID_Instalacion_Contrato = pId Select taula Order By taula.ID_Instalacion_Contrato_Producto
                .M.clsUltraGrid.CargarIEnumerable(_Productos)
                '.GRID.DataSource = Contactos

                .GRID.DisplayLayout.Bands(0).Columns("Total").CellActivation = Activation.NoEdit
                .M_Editable()
                .M_TreureFocus()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Productos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Productos.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Productos

                If oLinqContrato.ID_Instalacion_Contrato = 0 Then
                    If Guardar() = False Then
                        Exit Sub
                    End If
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Instalacion_Contrato").Value = oLinqContrato

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Productos_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Productos.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqContrato.Instalacion_Contrato_Producto.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()
            Call CalcularTotalImport()


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Productos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Productos.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
        Call CalcularTotalImport()

    End Sub

    Private Sub CarregarComboProductes()
        Try
            ' Dim DT As DataTable = BD.RetornaDataTable("SELECT ID_Producto, Codigo, Descripcion From Producto Where Activo=1 Order by Descripcion")
            Dim _LlistatProductes As IEnumerable = From Taula In oDTC.Producto Where Taula.Activo = True Order By Taula.Descripcion Select Taula.ID_Producto, Taula.Codigo, Taula.Descripcion, Producto = Taula


            Me.C_Producto.DataSource = _LlistatProductes
            If _LlistatProductes Is Nothing Then
                Exit Sub
            End If

            With C_Producto
                .AutoCompleteMode = AutoCompleteMode.Suggest
                .AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
                .MaxDropDownItems = 16
                .DisplayMember = "Descripcion"
                .ValueMember = "Producto"
                .DisplayLayout.Bands(0).Columns("ID_Producto").Hidden = True
                .DisplayLayout.Bands(0).Columns("Codigo").Width = 100
                .DisplayLayout.Bands(0).Columns("Codigo").Header.Caption = "Código"
                .DisplayLayout.Bands(0).Columns("Descripcion").Width = 600
                .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
                .DisplayLayout.Bands(0).Columns("Producto").Hidden = True
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Productos_M_Grid_InitializeLayout(Sender As Object, e As InitializeLayoutEventArgs) Handles GRD_Productos.M_Grid_InitializeLayout
        e.Layout.Bands(0).Columns("Producto").EditorComponent = Me.C_Producto
        e.Layout.Bands(0).Columns("Producto").AutoCompleteMode = AutoCompleteMode.SuggestAppend
        e.Layout.Bands(0).Columns("Producto").AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
    End Sub
#End Region

#Region "Grid Documentos vinculados"

    Private Sub CargaGrid_DocumentosVinculados(ByVal pIDContrato As Integer)
        Try

            Dim _Filtre As String

            'Select Case DirectCast(oLinqEntrada.ID_Entrada_Tipo, EnumEntradaTipo)
            '    Case EnumEntradaTipo.PedidoCompra, EnumEntradaTipo.PedidoVenta
            '        _Filtre = "(Select ID_Entrada From Entrada_Linea Where ID_Entrada_Linea_Pedido in (Select ID_Entrada_Linea From Entrada_Linea Where ID_Entrada=" & oLinqEntrada.ID_Entrada & ") Group By ID_Entrada)"
            '    Case EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.AlbaranCompra
            '        _Filtre = "(Select B.ID_Entrada From (Select (Select ID_Entrada From Entrada_Linea as A Where Entrada_Linea.ID_Entrada_Linea_Pedido=A.ID_Entrada_Linea) as ID_Entrada From Entrada_Linea Where ID_Entrada_Linea_Pedido is not null and ID_Entrada=" & oLinqEntrada.ID_Entrada & ") as B Group By B.ID_Entrada)"
            '    Case EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaCompra
            _Filtre = "(Select ID_Entrada From Entrada_Linea Where ID_Instalacion_Contrato=" & pIDContrato & " Group By ID_Entrada)"
            '    Case Else
            '        Exit Sub
            'End Select


            With Me.GRD_Facturacion

                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_Entrada Where ID_Entrada_Tipo=" & EnumEntradaTipo.FacturaVenta & " and C_Entrada.ID_Entrada In  " & _Filtre & " Order by FechaEntrada Desc", True))

                .M_NoEditable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Facturacions_M_GRID_DoubleClickRow(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Facturacion.M_GRID_DoubleClickRow


        Dim pRow As UltraGridRow = Me.GRD_Facturacion.GRID.ActiveRow
        Dim _IDEntrada As Integer = pRow.Cells("ID_Entrada").Value
        Dim _IDTipoDocumento As Integer = pRow.Cells("ID_Entrada_Tipo").Value

        Dim frm As New frmEntrada
        frm.Entrada(_IDEntrada, _IDTipoDocumento)
        frm.FormObrir(Me, True)
    End Sub

#End Region

    Private Sub ToolForm_m_ToolForm_ToolClick_Botones_Extras(Sender As Object, e As UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        If e.Tool.Key = "RecuperarCondiciones" Then
            If Me.R_OtrasCondiciones.pTextEspecial.Length > 0 Then
                If Mensaje.Mostrar_Mensaje("Ya hay datos introducidos en el campo 'Otras condiciones'. ¿Desea sobreescribirlas? ", M_Mensaje.Missatge_Modo.PREGUNTA, , , True) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If
            End If
            Me.R_OtrasCondiciones.pText = oDTC.Instalacion_Contrato_TipoContrato.Where(Function(F) F.ID_Instalacion_Contrato_TipoContrato = CInt(Me.C_TipoContrato.Value)).FirstOrDefault.Condiciones
        End If
    End Sub
End Class