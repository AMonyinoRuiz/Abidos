Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmPropuesta_Linea
    Dim oDTC As DTCDataContext
    Dim oLinqPropuesta_Linea As Propuesta_Linea
    Dim oLinqPropuesta As Propuesta
    Dim oLinqInstalacion As Instalacion
    Dim AvisoCantidadSuperiorAUno As String = ""
    Dim AvisoVinculacionArticuloFamiliaNoTraspasar As String = ""
    Dim oEntradaAccesos As Boolean
    Public oTipoPropuesta As TipoPropuesta
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2

    Enum TipoPropuesta
        Propuesta = 1
        InstalacionAnterior = 2
        SeInstalo = 3
    End Enum

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Call Guardar()
    End Sub

    'Guardar y mantener
    Private Sub ToolForm_m_ToolForm_GuardarIMantener() Handles ToolForm.m_ToolForm_Buscar
        If Guardar() = True Then
            'Dim ID_Producte As Integer = oLinqPropuesta_Linea.ID_Producto
            'Dim Codi As String = oLinqPropuesta_Linea.Producto.Codigo
            'Dim Descripcio As String = oLinqPropuesta_Linea.Descripcion
            'Dim Uso As String = oLinqPropuesta_Linea.Uso
            'Dim Descuento As Decimal = oLinqPropuesta_Linea.Descuento


            'Call Netejar_Pantalla()
            oLinqPropuesta_Linea = New Propuesta_Linea
            Call ActivarPantalla(True)
            Me.TE_Producto_Codigo.ReadOnly = False
            Call CargarIdentificadorDeLinea()
            'Call CargarCombo_Vinculacions(oLinqPropuesta.ID_Propuesta)

            Me.T_NumSerie.Text = ""
            Me.T_NumZona.Text = ""
            Me.GRD_Acceso.GRID.DataSource = Nothing
            ' Me.T_Identificador.Text = ""

            ''Me.TE_Producto_Codigo.ReadOnly = False
            'Me.TE_Producto_Codigo.Tag = ID_Producte
            'Me.TE_Producto_Codigo.Text = Codi
            'Me.T_Producto_Descripcion.Text = Descripcio
            'Me.T_Uso.Text = Uso
            'Me.T_Descuento.Value = Descuento
            'Call CargaPreuProductoYTraspasable(ID_Producte)
        End If
    End Sub

    'Guardar y continuar
    Private Sub ToolForm_m_ToolForm_GuardarIContinuar() Handles ToolForm.m_ToolForm_Imprimir
        If Guardar() = True Then

            Call Netejar_Pantalla()
            Call ActivarPantalla(False)
            Me.TE_Producto_Codigo.ReadOnly = False
            Call CargarIdentificadorDeLinea()
            Call CargarCombo_Vinculacions(oLinqPropuesta.ID_Propuesta)
            Call CargarCombo_VinculacionsEnergetiques(oLinqPropuesta.ID_Propuesta)
        End If
    End Sub

    'Duplicar
    Private Sub ToolForm_m_ToolForm_Seleccionar() Handles ToolForm.m_ToolForm_Seleccionar
        Try

            If Me.T_Unidades.Value <> 1 Then
                Mensaje.Mostrar_Mensaje("Información: La actual línea tiene una cantidad diferente a 1. La duplicación creará los nuevos registros con cantidad 1", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            Dim quantitat As String
            quantitat = Mensaje.Mostrar_Entrada_Datos("Introduzca la cantidad de veces que desea duplicar la línea de propuesta. (Ejemplo: si introduce un 2 se copiará la linea 2 veces, aparte, tendrá la linea actual)", "1", False)
            If IsNumeric(quantitat) = False Then
                Mensaje.Mostrar_Mensaje("Los datos introducidos no son correctos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If Guardar() = False Then
                Exit Sub
            End If


            Dim i As Integer = 0

            Util.WaitFormObrir()
            Dim IdentificadorLinea As Integer
            IdentificadorLinea = clsPropuestaLinea.RetornaUltimIdentificadorDeLinea(oDTC, oLinqPropuesta.ID_Propuesta)

            For i = 1 To CInt(quantitat)
                Dim _LineaNova As New Propuesta_Linea
                _LineaNova = clsPropuestaLinea.RetornaDuplicacioInstancia(oLinqPropuesta_Linea)
                _LineaNova.Unidad = 1
                _LineaNova.Identificador = IdentificadorLinea

                IdentificadorLinea = CrearFills(_LineaNova, IdentificadorLinea)

                oDTC.Propuesta_Linea.InsertOnSubmit(_LineaNova)

                IdentificadorLinea = IdentificadorLinea + 10
            Next
            oDTC.SubmitChanges()

            Util.WaitFormTancar()
            Mensaje.Mostrar_Mensaje("Registro duplicado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
            Call M_ToolForm1_m_ToolForm_Sortir()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Nuevo() Handles ToolForm.m_ToolForm_Nuevo
        If Guardar() = True Then
            Call M_ToolForm1_m_ToolForm_Sortir()
        End If
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pInstalacion As Instalacion, ByRef pPropuesta As Propuesta, ByRef pDTC As DTCDataContext, Optional ByVal pId As Integer = 0, Optional ByVal pEntradaAccesos As Boolean = False)
        Try

            Me.AplicarDisseny()

            oLinqInstalacion = pInstalacion
            oLinqPropuesta = pPropuesta
            oEntradaAccesos = pEntradaAccesos

            Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True
            Me.ToolForm.M.Botons.tImprimir.SharedProps.Caption = "Guardar y continuar"
            Me.ToolForm.M.Botons.tBuscar.SharedProps.Visible = True
            Me.ToolForm.M.Botons.tBuscar.SharedProps.Caption = "Guardar y mantener"
            Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Visible = True
            Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Caption = "Duplicar"
            Me.ToolForm.M.Botons.tNou.SharedProps.Visible = True
            Me.ToolForm.M.Botons.tNou.SharedProps.Caption = "Guardar y salir"
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Guardar"



            oDTC = pDTC

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Propuesta_Linea_Archivo", 1)
            AddHandler Fichero.DespresDeCarregarDades, AddressOf DespresDeCarregarDadesGridArchivos
            AddHandler Fichero.DespresDeEliminarRegistre, AddressOf DespresDeEliminarElRegistreArchivos

            'Util.Cargar_Combo(Me.C_Abertura, "SELECT ID_Instalacion_Emplazamiento_Abertura, Descripcion FROM Instalacion_Emplazamiento_Abertura  ORDER BY Codigo", False)
            ' Util.Cargar_Combo(Me.C_ElementosAProteger, "SELECT ID_Instalacion_ElementosAProteger, Descripcion FROM Instalacion_ElementosAProteger  ORDER BY Codigo", True)

            Util.Cargar_Combo(Me.C_Emplazamiento, "SELECT ID_Instalacion_Emplazamiento, Descripcion FROM Instalacion_Emplazamiento Where ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " ORDER BY Descripcion", True)
            Util.Cargar_Combo(Me.C_Traspasable, "SELECT ID_Producto_SubFamilia_Traspaso, Descripcion FROM Producto_SubFamilia_Traspaso Where Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_TipoZona, "SELECT ID_Propuesta_Linea_TipoZona, Descripcion FROM Propuesta_Linea_TipoZona Where Activo=1 ORDER BY Codigo", False)
            Util.Cargar_Combo(Me.C_InstaladoEn, "SELECT ID_Instalacion_InstaladoEn, Identificador + '-' + Descripcion FROM Instalacion_InstaladoEn Where ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " ORDER BY Descripcion", False)
            Util.Cargar_Combo(Me.C_Sistema_Operativo, "SELECT ID_SistemaOperativo, Descripcion FROM SistemaOperativo ORDER BY Descripcion", False)
            Util.Cargar_Combo(Me.C_Opcion, "SELECT ID_Propuesta_Opcion, Nombre FROM Propuesta_Opcion ORDER BY ID_Propuesta_Opcion", False)

            Dim BotoCancelar As UltraWinEditors.EditorButton
            BotoCancelar = New UltraWinEditors.EditorButton
            BotoCancelar.Key = "Cancelar"
            Dim oDisseny As New M_Disseny.ClsDisseny
            BotoCancelar.Appearance.Image = oDisseny.Leer_Imagen("text_cancelar.gif")
            BotoCancelar.Width = 16
            BotoCancelar.Appearance.BackColor = Color.White
            BotoCancelar.Appearance.BorderAlpha = Alpha.Transparent

            Me.C_Vinculacion.ButtonsRight.Add(BotoCancelar.Clone)
            Me.C_Vinculacion_Energetica.ButtonsRight.Add(BotoCancelar.Clone)
            Me.C_Abertura.ButtonsRight.Add(BotoCancelar.Clone)
            Me.C_ElementosAProteger.ButtonsRight.Add(BotoCancelar.Clone)
            Me.C_Planta.ButtonsRight.Add(BotoCancelar.Clone)
            Me.C_InstaladoEn.ButtonsRight.Add(BotoCancelar.Clone)
            Me.C_Proveedor.ButtonsRight.Add(BotoCancelar.Clone)
            Me.C_Opcion.ButtonsRight.Add(BotoCancelar.Clone)
            Me.GRD_Ficheros.M.clsToolBar.Boto_Afegir("FotoPredeterminada", "Predeterminar foto", True)
            Me.GRD_Ficheros.M.clsToolBar.Boto_Afegir("QuitarFotoPredeterminada", "Quitar foto predeterminada", True)

            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "SeInstalo", "DatosAcceso", "PreciosAnteriores")

            Call CargarCombo_Vinculacions(pId)
            Call CargarCombo_VinculacionsEnergetiques(pId)

            If pId <> 0 Then
                Call Cargar_Form(pId)
                Call ActivarPantalla(True)
            Else
                Call Netejar_Pantalla()
                Call ActivarPantalla(False)
                Call CargarIdentificadorDeLinea()
                'Me.C_Emplazamiento.ReadOnly = False
                'Me.C_Planta.ReadOnly = False
                'Me.C_Zona.ReadOnly = False
                'Me.C_Abertura.ReadOnly = False
                'Me.C_ElementosAProteger.ReadOnly = False

            End If



            If oLinqPropuesta.SeInstalo = True Then
                oTipoPropuesta = TipoPropuesta.SeInstalo
            Else
                If oLinqPropuesta.ID_Propuesta_Tipo = EnumPropuestaTipo.InstalacionAnterior Then
                    oTipoPropuesta = TipoPropuesta.InstalacionAnterior
                Else
                    oTipoPropuesta = TipoPropuesta.Propuesta
                End If
            End If

            Me.KeyPreview = False

            Select Case oTipoPropuesta
                Case TipoPropuesta.Propuesta
                    Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "PreciosAnteriores")
                    Util.Tab_InVisible_x_Key(Me.TAB_Secundari, M_Util.Enum_Tab_Activacion.ALGUNOS, "MantenimientosPlanificados")
                Case TipoPropuesta.SeInstalo
                    Me.Text = "Línea de 'Cómo se instaló'"
                    Util.Tab_InVisible_x_Key(Me.TAB_Secundari, M_Util.Enum_Tab_Activacion.ALGUNOS, "Precios")
                    Me.L_Traspasable.Visible = False
                    Me.C_Traspasable.Visible = False
                    Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "SeInstalo", "DatosAcceso")
                Case TipoPropuesta.InstalacionAnterior
                    Util.Tab_InVisible_x_Key(Me.TAB_Secundari, M_Util.Enum_Tab_Activacion.ALGUNOS, "Precios", "MantenimientosPlanificados")
                    Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "SeInstalo", "DatosAcceso")

            End Select

            If oLinqPropuesta.ID_Propuesta_Estado <> EnumPropuestaEstado.Pendiente Then
                ' Me.UltraTabPageControl1.Enabled = False
                Me.UltraTabPageControl4.Enabled = False
                Me.UltraTabPageControl2.Enabled = False
                Me.UltraTabPageControl6.Enabled = False 'Articles fills
                Me.ToolForm.M.Botons.tBuscar.SharedProps.Visible = False
                Me.ToolForm.M.Botons.tGuardar.SharedProps.Visible = False
                Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = False
                Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Visible = False 'Duplicar
                Me.R_DescripcionAmpliada.pEnable = False
                Me.B_Cancelar.Enabled = False
            End If

            Me.Tab_Principal.Tabs("Detalle").Selected = True

            If pEntradaAccesos = True Then
                Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = False
                Me.ToolForm.M.Botons.tBuscar.SharedProps.Visible = False
                Util.Tab_Desactivar_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "DatosAcceso")
                Me.Tab_Principal.Tabs("DatosAcceso").Selected = True
            End If

            '            ' Me.GRD_Associacio.GRID.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Private Sub CargarCombo_Vinculacions(ByVal pID As Integer)
        Try
            Dim SQLText As String = ""
            If pID <> 0 Then
                SQLText = " and ID_Propuesta_Linea<>" & pID
            End If
            'Util.Cargar_Combo(Me.C_Vinculacion, "SELECT ID_Propuesta_Linea, cast(Identificador as nvarchar(20)) + ' - ' + Propuesta_Linea.Descripcion FROM Propuesta_Linea, Producto WHERE Producto.ID_Producto=Propuesta_Linea.ID_Producto and (Producto.Central=1 or Producto.Expansor=1 or Producto.Modulo_Rele=1) and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " ORDER BY Identificador", False)
            ' Util.Cargar_Combo(Me.C_Vinculacion, "SELECT ID_Propuesta_Linea, cast(Identificador as nvarchar(20)) + ' - ' + Propuesta_Linea.Descripcion FROM Propuesta_Linea, Producto WHERE Producto.ID_Producto=Propuesta_Linea.ID_Producto and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 ORDER BY Identificador", False)
            Dim DT As DataTable = BD.RetornaDataTable("SELECT  ID_Propuesta_Linea, Identificador, IdentificadorDelProducto,  Descripcion,Emplazamiento, Planta, Zona, NickZona, (SELECT Sum(Unidad) AS Expr1 FROM dbo.Propuesta_Linea AS B WHERE (ID_Propuesta_Linea_Vinculado = dbo.C_Propuesta_Linea.ID_Propuesta_Linea)) AS NumVinculados  FROM C_Propuesta_Linea WHERE Activo=1 and  ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 and Conectable=1 ORDER BY IdentificadorDelProducto")
            Me.C_Vinculacion.DataSource = DT
            If DT Is Nothing Then
                Exit Sub
            End If
            Me.C_Vinculacion.MaxDropDownItems = 16
            Me.C_Vinculacion.DisplayMember = "Descripcion"
            Me.C_Vinculacion.ValueMember = "ID_Propuesta_Linea"
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("ID_Propuesta_Linea").Hidden = True
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Width = 100
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Header.Caption = "Identificador"
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Width = 60
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Header.Caption = "Linea"
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").CellAppearance.TextHAlign = HAlign.Right
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Descripcion").Width = 400
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Emplazamiento").Width = 100
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Emplazamiento").Header.Caption = "Ubicación"
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Planta").Width = 100
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Planta").Header.Caption = "Planta"
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Zona").Width = 100
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Zona").Header.Caption = "Zona"
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("NickZona").Width = 100
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("NickZona").Header.Caption = "Alias"
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("NumVinculados").Width = 100
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("NumVinculados").Header.Caption = "Nº Vinculados"
            Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("NumVinculados").CellAppearance.TextHAlign = HAlign.Right

            'Me.C_Vinculacion.DropDownStyle = UltraComboStyle.DropDownList

            If oLinqPropuesta.SeInstalo = True Then
                'Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Hidden = False
                Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Hidden = True
            Else
                'Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Hidden = True
                Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Hidden = False
            End If

            Me.C_Vinculacion.DisplayLayout.Bands(0).Override.AllowRowFiltering = DefaultableBoolean.True

            'Util.Cargar_Combo(Me.UltraCombo1, "SELECT ID_Propuesta_Linea, cast(Identificador as nvarchar(20)), Propuesta_Linea.Descripcion FROM Propuesta_Linea, Producto WHERE Producto.ID_Producto=Propuesta_Linea.ID_Producto and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 ORDER BY Identificador", False)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_VinculacionsEnergetiques(ByVal pID As Integer)
        Try
            With Me.C_Vinculacion_Energetica

                Dim SQLText As String = ""
                If pID <> 0 Then
                    SQLText = " and ID_Propuesta_Linea<>" & pID
                End If

                Dim DT As DataTable = BD.RetornaDataTable("SELECT  ID_Propuesta_Linea, Identificador, IdentificadorDelProducto,  Descripcion,Emplazamiento, Planta, Zona, NickZona, NumVinculados, TotalSalidaRestante  FROM C_Propuesta_Linea_Vinculacion_Energetica WHERE Activo=1 and  ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 and Conectable=1 ORDER BY IdentificadorDelProducto")
                .DataSource = DT
                If DT Is Nothing Then
                    Exit Sub
                End If

                .MaxDropDownItems = 16
                .DisplayMember = "Descripcion"
                .ValueMember = "ID_Propuesta_Linea"
                .DisplayLayout.Bands(0).Columns("ID_Propuesta_Linea").Hidden = True
                .DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Width = 100
                .DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Header.Caption = "Identificador"
                .DisplayLayout.Bands(0).Columns("Identificador").Width = 60
                .DisplayLayout.Bands(0).Columns("Identificador").Header.Caption = "Linea"
                .DisplayLayout.Bands(0).Columns("Identificador").CellAppearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("Descripcion").Width = 400
                .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
                .DisplayLayout.Bands(0).Columns("Emplazamiento").Width = 100
                .DisplayLayout.Bands(0).Columns("Emplazamiento").Header.Caption = "Ubicación"
                .DisplayLayout.Bands(0).Columns("Planta").Width = 100
                .DisplayLayout.Bands(0).Columns("Planta").Header.Caption = "Planta"
                .DisplayLayout.Bands(0).Columns("Zona").Width = 100
                .DisplayLayout.Bands(0).Columns("Zona").Header.Caption = "Zona"
                .DisplayLayout.Bands(0).Columns("NickZona").Width = 100
                .DisplayLayout.Bands(0).Columns("NickZona").Header.Caption = "Alias"
                .DisplayLayout.Bands(0).Columns("NumVinculados").Width = 100
                .DisplayLayout.Bands(0).Columns("NumVinculados").Header.Caption = "Nº Vinculados (mA)"
                .DisplayLayout.Bands(0).Columns("NumVinculados").CellAppearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("TotalSalidaRestante").Width = 100
                .DisplayLayout.Bands(0).Columns("TotalSalidaRestante").Header.Caption = "Potencia restante"
                .DisplayLayout.Bands(0).Columns("TotalSalidaRestante").CellAppearance.TextHAlign = HAlign.Right

                If oLinqPropuesta.SeInstalo = True Then
                    .DisplayLayout.Bands(0).Columns("Identificador").Hidden = True
                Else
                    .DisplayLayout.Bands(0).Columns("Identificador").Hidden = False
                End If

                .DisplayLayout.Bands(0).Override.AllowRowFiltering = DefaultableBoolean.True

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarIdentificadorDeLinea()
        If oLinqPropuesta.Propuesta_Linea.Count = 0 Then
            Me.T_Identificador.Value = 1
        Else
            Me.T_Identificador.Value = oLinqPropuesta.Propuesta_Linea.Max(Function(F) F.Identificador) + 10
        End If
    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.GRD_Productos.GRID.ActiveRow = Nothing 'treiem el focus per a que guardi si hi ha una línea editantse
            Me.TE_Producto_Codigo.Focus()

            If IsNothing(Me.T_Unidades.Value) = False AndAlso Me.T_Unidades.Value = 0 Then
                Mensaje.Mostrar_Mensaje("Imposible asignar a una línea de propuesta la cantidad 0", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If

            If Me.C_Vinculacion.SelectedRow Is Nothing = False And oLinqPropuesta_Linea.ID_Propuesta_Linea <> 0 Then
                Dim _Propuesta_linea As Propuesta_Linea
                _Propuesta_linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(Me.C_Vinculacion.SelectedRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
                If _Propuesta_linea.ID_Propuesta_Linea_Vinculado = oLinqPropuesta_Linea.ID_Propuesta_Linea Then
                    Mensaje.Mostrar_Mensaje("Imposible guardar los datos, la vinculación con esta línea provocaría una recursividad", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
            End If

            If Me.C_Vinculacion_Energetica.SelectedRow Is Nothing = False And oLinqPropuesta_Linea.ID_Propuesta_Linea <> 0 Then
                Dim _Propuesta_linea As Propuesta_Linea
                _Propuesta_linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(Me.C_Vinculacion_Energetica.SelectedRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
                If _Propuesta_linea.ID_Propuesta_Linea_Vinculado_Energetico = oLinqPropuesta_Linea.ID_Propuesta_Linea Then
                    Mensaje.Mostrar_Mensaje("Imposible guardar los datos, la vinculación energética con esta línea provocaría una recursividad", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqPropuesta_Linea)

            Call CalcularNivellsLineaActual()

            If oLinqPropuesta_Linea.ID_Propuesta_Linea = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oLinqPropuesta_Linea.ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.PendienteDeAprobar

                oDTC.Propuesta_Linea.InsertOnSubmit(oLinqPropuesta_Linea)

                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)

                Call clsPropuesta.AsignarEspecificaciones(oDTC, oLinqPropuesta, oLinqPropuesta_Linea)
            Else
                If oLinqPropuesta_Linea.Unidad > 1 AndAlso oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True AndAlso F.ID_Propuesta_Linea_Vinculado = oLinqPropuesta_Linea.ID_Propuesta_Linea).Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible asignar una cantidad superior a 1. Las líneas de propuesta que tienen articulos vinculados tienen que tener siempre una cantidad de uno.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
                If BD.RetornaValorSQL("Select isnull(Max(a.Conta),0) From (select COUNT(*) as Conta, ID_Propuesta_Plano From Propuesta_Plano_ElementosIntroducidos Where id_propuesta_linea=" & oLinqPropuesta_Linea.ID_Propuesta_Linea & " Group By ID_Propuesta_Plano) as a") > oLinqPropuesta_Linea.Unidad Then
                    Mensaje.Mostrar_Mensaje("Imposible modificar la cantidad de la línea. La cantidad que se quiere guardar es inferior a las asignadas en un o varios planos/diagramas", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Exit Function
                End If

                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If
            Call CrearFills(oLinqPropuesta_Linea, Me.T_Identificador.Value)
            oLinqPropuesta.Base = oLinqPropuesta.RetornaTotalBase
            oLinqPropuesta.Total = oLinqPropuesta.RetornaTotalPropuesta

            oDTC.SubmitChanges()
            'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqPropuesta_Linea
                'Posem primer de tot el seleccionar el combo del proveidor pq no maxaqui les dades de coste que hi ha guardades a la BD
                Me.C_Proveedor.Value = .ID_Proveedor

                Me.T_Identificador.Text = .Identificador
                Me.T_Descuento.Value = .Descuento
                Me.T_IVA.Value = .IVA
                Me.T_Preu.Value = .Precio

                Me.TE_Producto_Codigo.Tag = .ID_Producto
                Me.TE_Producto_Codigo.Value = .Producto.Codigo
                Me.T_Coste.Value = .PrecioCoste
                Me.T_Producto_Descripcion.Value = .Descripcion
                Me.T_Unidades.Value = .Unidad
                Me.T_Uso.Text = .Uso
                Me.C_TipoZona.Value = .ID_Propuesta_Linea_TipoZona
                Me.C_Emplazamiento.Value = .ID_Instalacion_Emplazamiento
                Me.C_Planta.Value = .ID_Instalacion_Emplazamiento_Planta
                Me.C_Zona.Value = .ID_Instalacion_Emplazamiento_Zona
                Me.C_ElementosAProteger.Value = .ID_Instalacion_ElementosAProteger
                Me.C_Abertura.Value = .ID_Instalacion_Emplazamiento_Abertura
                Me.T_IdentificadorProducto.Value = .IdentificadorDelProducto
                Me.T_DetalleComoSeInstalo.pText = .DetalleInstalacion
                Me.T_NumZona.Text = .NumZona
                Me.C_Traspasable.Value = .ID_Producto_SubFamilia_Traspaso
                Me.T_BocaConexion.Text = .BocaConexion
                Me.T_NickZona.Text = .NickZona
                Me.R_DescripcionAmpliada.pText = .DescripcionAmpliada
                Me.R_DescripcionAmpliada_Tecnica.pText = .DescripcionAmpliada_Tecnica
                Me.T_NumSerie.Text = .NumSerie
                Me.T_Particion.Text = .Particion
                Me.T_Ruta_Orden.Value = .RutaOrden
                Me.T_Ruta_Parametros.Text = .RutaParametros
                Me.T_ReferenciaMemoria.Text = .ReferenciaMemoria
                Me.T_ATenerEnCuenta.Text = .ATenerEnCuenta
                Me.T_Fase.Text = .Fase
                Me.C_InstaladoEn.Value = .ID_Instalacion_InstaladoEn
                Me.T_VLAN.Value = .VLAN
                Me.T_IP.Value = .IP
                Me.T_MascaraSubRed.Value = .MascaraSubred
                Me.T_PuertaEnlace.Value = .PuertaEnlace
                Me.T_DNSPrimaria.Value = .DNSPrimaria
                Me.T_DNSSecundaria.Value = .DNSSecundaria
                Me.T_IPPublica.Value = .IPPublica
                Me.T_ServidorWINS.Text = .ServidorWINS
                Me.T_Dominio.Text = .Dominio
                Me.T_NombreEquipo.Text = .NombreEquipo
                Me.T_NetBios.Text = .NetBios
                Me.T_Almacenamiento.Value = .AlmacenamientoEnDisco
                Me.T_MemoriaRam.Value = .MemoriaRam
                Me.T_Procesador.Text = .Procesador
                Me.C_Sistema_Operativo.Value = .ID_SistemaOperativo
                Me.T_MacAdress.Text = .MacAdress
                Me.DT_FechaFabricacion.Value = .FechaFabricacion
                Me.DT_FechaFinalVidaUtil.Value = .FechaFinalVidaUtil
                'If .ID_Propuesta_Opcion.HasValue = True Then
                Me.C_Opcion.Value = .ID_Propuesta_Opcion
                Me.T_PrecioOpcion.Value = .ImporteOpcion
                'End If
                Me.T_PlazoEntrega.Value = .PlazoEntrega



                If .ID_Propuesta_Linea_Vinculado <> 0 Then
                    Dim pRow As UltraGridRow
                    For Each pRow In Me.C_Vinculacion.Rows
                        If pRow.Cells("ID_Propuesta_Linea").Value = .ID_Propuesta_Linea_Vinculado Then
                            Me.C_Vinculacion.SelectedRow = pRow
                            Exit For
                        End If
                    Next
                    'Me.C_Vinculacion.Rows(40).Selected = True
                    'Me.C_Vinculacion.Value = .Descripcion
                End If

                If .ID_Propuesta_Linea_Vinculado_Energetico <> 0 Then
                    Dim pRow As UltraGridRow
                    For Each pRow In Me.C_Vinculacion_Energetica.Rows
                        If pRow.Cells("ID_Propuesta_Linea").Value = .ID_Propuesta_Linea_Vinculado_Energetico Then
                            Me.C_Vinculacion_Energetica.SelectedRow = pRow
                            Exit For
                        End If
                    Next
                End If

                Call CargaCombo_Proveidor(.ID_Producto)
                Call CargarFoto()
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pPropuesta_Linea As Propuesta_Linea)
        Try
            With pPropuesta_Linea
                .Activo = True
                .Propuesta = oLinqPropuesta
                .Identificador = Me.T_Identificador.Value
                .Descuento = Util.Comprobar_NULL_Per_0_Decimal(Me.T_Descuento.Value)
                .IVA = Me.T_IVA.Value
                .Precio = Me.T_Preu.Value
                If Me.T_Coste.Value Is Nothing OrElse Me.T_Coste.Value.ToString.Length = 0 Then
                    .PrecioCoste = 0
                Else
                    .PrecioCoste = CDbl(Me.T_Coste.Value)
                End If

                .BocaConexion = Me.T_BocaConexion.Text
                .NickZona = Me.T_NickZona.Text
                .DescripcionAmpliada = Me.R_DescripcionAmpliada.pText
                .DescripcionAmpliada_Tecnica = Me.R_DescripcionAmpliada_Tecnica.pText

                .RutaOrden = DbnullToNothing(Me.T_Ruta_Orden.Value)
                .VLAN = DbnullToNothing(Me.T_VLAN.Value)
                .RutaParametros = Me.T_Ruta_Parametros.Text

                If IsNumeric(Me.TE_Producto_Codigo.Tag) = True Then
                    .Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
                Else
                    .Producto = Nothing
                End If
                .Descripcion = Me.T_Producto_Descripcion.Text

                .Unidad = Me.T_Unidades.Value
                .Uso = Me.T_Uso.Text

                .IdentificadorDelProducto = Me.T_IdentificadorProducto.Value
                .DetalleInstalacion = Me.T_DetalleComoSeInstalo.pText
                .NumZona = Me.T_NumZona.Text
                .NumSerie = Me.T_NumSerie.Text
                .Particion = Me.T_Particion.Text
                .ReferenciaMemoria = Me.T_ReferenciaMemoria.Text
                .ATenerEnCuenta = Me.T_ATenerEnCuenta.Text
                .Fase = Me.T_Fase.Text
                .IP = Util.Comprobar_NULL(Me.T_IP.Value)
                .MascaraSubred = Util.Comprobar_NULL(Me.T_MascaraSubRed.Value)
                .PuertaEnlace = Util.Comprobar_NULL(Me.T_PuertaEnlace.Value)
                .DNSPrimaria = Util.Comprobar_NULL(Me.T_DNSPrimaria.Value)
                .DNSSecundaria = Util.Comprobar_NULL(Me.T_DNSSecundaria.Value)
                .IPPublica = Util.Comprobar_NULL(Me.T_IPPublica.Value)
                .ServidorWINS = Me.T_ServidorWINS.Text
                .Dominio = Me.T_Dominio.Text
                .NombreEquipo = Me.T_NombreEquipo.Text
                .NetBios = Me.T_NetBios.Text
                .AlmacenamientoEnDisco = DbnullToNothing(Me.T_Almacenamiento.Value)
                .MemoriaRam = DbnullToNothing(Me.T_MemoriaRam.Value)
                .Procesador = Me.T_Procesador.Text
                .MacAdress = Me.T_MacAdress.Text

                .FechaFabricacion = Me.DT_FechaFabricacion.Value
                .FechaFinalVidaUtil = Me.DT_FechaFinalVidaUtil.Value

                If Me.C_Abertura.SelectedIndex <> -1 Then
                    .Instalacion_Emplazamiento_Abertura = oDTC.Instalacion_Emplazamiento_Abertura.Where(Function(F) F.ID_Instalacion_Emplazamiento_Abertura = CInt(Me.C_Abertura.Value)).FirstOrDefault
                Else
                    .Instalacion_Emplazamiento_Abertura = Nothing
                End If

                If Me.C_ElementosAProteger.SelectedIndex <> -1 Then
                    .Instalacion_ElementosAProteger = oDTC.Instalacion_ElementosAProteger.Where(Function(F) F.ID_Instalacion_ElementosAProteger = CInt(Me.C_ElementosAProteger.Value)).FirstOrDefault
                Else
                    .Instalacion_ElementosAProteger = Nothing
                End If


                If Me.C_Emplazamiento.SelectedIndex <> -1 Then
                    .Instalacion_Emplazamiento = oDTC.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = CInt(Me.C_Emplazamiento.Value)).FirstOrDefault
                Else
                    .Instalacion_Emplazamiento = Nothing
                End If

                If Me.C_Planta.SelectedIndex <> -1 Then
                    .Instalacion_Emplazamiento_Planta = oDTC.Instalacion_Emplazamiento_Planta.Where(Function(F) F.ID_Instalacion_Emplazamiento_Planta = CInt(Me.C_Planta.Value)).FirstOrDefault
                Else
                    .Instalacion_Emplazamiento_Planta = Nothing
                End If

                If Me.C_Zona.SelectedIndex <> -1 Then
                    .Instalacion_Emplazamiento_Zona = oDTC.Instalacion_Emplazamiento_Zona.Where(Function(F) F.ID_Instalacion_Emplazamiento_Zona = CInt(Me.C_Zona.Value)).FirstOrDefault
                Else
                    .Instalacion_Emplazamiento_Zona = Nothing
                End If

                .ID_Instalacion_Emplazamiento_Zona = Me.C_Zona.Value

                If Me.C_Vinculacion.SelectedRow Is Nothing Then
                    .Propuesta_Linea = Nothing
                Else
                    .Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(Me.C_Vinculacion.SelectedRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
                End If

                If Me.C_Vinculacion_Energetica.SelectedRow Is Nothing Then
                    .Propuesta_Linea_VinculadoEnergeticoPadre = Nothing
                Else
                    .Propuesta_Linea_VinculadoEnergeticoPadre = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(Me.C_Vinculacion_Energetica.SelectedRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
                End If

                If Me.C_Traspasable.SelectedIndex <> -1 Then
                    .Producto_SubFamilia_Traspaso = oDTC.Producto_SubFamilia_Traspaso.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso = CInt(Me.C_Traspasable.Value)).FirstOrDefault
                Else
                    .Producto_SubFamilia_Traspaso = Nothing
                End If

                If Me.C_TipoZona.SelectedIndex <> -1 Then
                    .Propuesta_Linea_TipoZona = oDTC.Propuesta_Linea_TipoZona.Where(Function(F) F.ID_Propuesta_Linea_TipoZona = CInt(Me.C_TipoZona.Value)).FirstOrDefault
                Else
                    .Propuesta_Linea_TipoZona = Nothing
                End If
                Me.C_TipoZona.Value = .ID_Propuesta_Linea_TipoZona

                If Me.C_InstaladoEn.SelectedIndex <> -1 Then
                    .Instalacion_InstaladoEn = oDTC.Instalacion_InstaladoEn.Where(Function(F) F.ID_Instalacion_InstaladoEn = CInt(Me.C_InstaladoEn.Value)).FirstOrDefault
                Else
                    .Instalacion_InstaladoEn = Nothing
                End If

                If Me.C_Proveedor.SelectedRow Is Nothing Then
                    .Proveedor = Nothing
                Else
                    .Proveedor = oDTC.Proveedor.Where(Function(F) F.ID_Proveedor = CInt(Me.C_Proveedor.SelectedRow.Cells("ID_Proveedor").Value)).FirstOrDefault
                End If

                If Me.C_Sistema_Operativo.SelectedIndex <> -1 Then
                    .SistemaOperativo = oDTC.SistemaOperativo.Where(Function(F) F.ID_SistemaOperativo = CInt(Me.C_Sistema_Operativo.Value)).FirstOrDefault
                Else
                    .SistemaOperativo = Nothing
                End If

                If Me.C_Opcion.SelectedIndex <> -1 Then
                    .Propuesta_Opcion = oDTC.Propuesta_Opcion.Where(Function(F) F.ID_Propuesta_Opcion = CInt(Me.C_Opcion.Value)).FirstOrDefault
                Else
                    .Propuesta_Opcion = Nothing
                End If

                .ImporteOpcion = DbnullToNothing(Me.T_PrecioOpcion.Value)
                .PlazoEntrega = DbnullToNothing(Me.T_PlazoEntrega.Value)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer, Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        Try
            Call Netejar_Pantalla(pNoCanviarALaPestanyaGeneral)
            oLinqPropuesta_Linea = (From taula In oDTC.Propuesta_Linea Where taula.ID_Propuesta_Linea = pID Select taula).First
            Call SetToForm()

            Call CargaGrid_MantenimientosPlanificados(oLinqPropuesta_Linea.ID_Propuesta_Linea)
            Fichero.Cargar_GRID(pID)

            Call CargaGrid_PreciosAnteriores(Me.TE_Producto_Codigo.Tag) 'ho fem aqui pq així es pinti de color verd el tab si te preus anteriors
            Call CargaGrid_PreciosCompras(Me.TE_Producto_Codigo.Tag)
            Call CargaGrid_Informatica(pID)
            Call CargaGrid_Software(pID)

            'Call Carga_Grid_Caracteristicas_Personalizadas(pID)
            ' Me.EstableixCaptionForm("Associat: " & (oLinqAssociat.Nom))

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla(Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        oLinqPropuesta_Linea = New Propuesta_Linea
        ' oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
        'Exit Sub
        'Me.Tab_Principal.Tabs("Detalle").Selected = True

        If pNoCanviarALaPestanyaGeneral = False Then
            Me.Tab_Principal.Tabs("Detalle").Selected = True
        End If

        Me.TE_Producto_Codigo.Value = Nothing
        Me.TE_Producto_Codigo.Tag = Nothing

        Me.TE_Producto_Codigo.Focus()

        Me.T_Unidades.Value = 1


        Me.C_Abertura.SelectedIndex = -1
        Me.C_ElementosAProteger.SelectedIndex = -1

        'si hi han items, seleccionarem el primer
        If Me.C_Emplazamiento.Items.Count > 0 Then
            Me.C_Emplazamiento.SelectedIndex = 0
        End If
        'Me.C_Emplazamiento.SelectedIndex = -1
        Me.C_Planta.SelectedIndex = -1
        Me.C_TipoZona.SelectedIndex = -1
        Me.C_Vinculacion.ActiveRow = Nothing
        Me.C_Vinculacion_Energetica.ActiveRow = Nothing
        Me.C_Sistema_Operativo.SelectedIndex = -1

        Me.C_Zona.SelectedIndex = -1
        Me.C_Opcion.SelectedIndex = -1
        Me.T_Ruta_Orden.Value = Nothing  'Si no el blanquejar ens posa un 0, i no volem 0!!

        Me.C_Abertura.ReadOnly = True
        Me.C_ElementosAProteger.ReadOnly = True
        Me.C_Planta.ReadOnly = True
        Me.C_Zona.ReadOnly = True

        Me.T_Coste.Appearance.ForeColor = Color.Black

        Me.T_IVA.Value = oDTC.Configuracion.FirstOrDefault.IVA

        'Recuperem dades d'emplazamiento, planta i zona de la propuesta original
        If oLinqPropuesta.ID_Instalacion_Emplazamiento <> 0 Then
            Me.C_Emplazamiento.Value = oLinqPropuesta.ID_Instalacion_Emplazamiento
        End If
        If oLinqPropuesta.ID_Instalacion_Emplazamiento_Planta <> 0 Then
            Me.C_Planta.Value = oLinqPropuesta.ID_Instalacion_Emplazamiento_Planta
        End If
        If oLinqPropuesta.ID_Instalacion_Emplazamiento_Zona <> 0 Then
            Me.C_Zona.Value = oLinqPropuesta.ID_Instalacion_Emplazamiento_Zona
        End If

        'Call CargaGrid_Accesos(0)
        'Call CargaGrid_UsuariosSistema(0)
        Call CargaGrid_MantenimientosPlanificados(0)
        Call CargaGrid_Informatica(0)
        Call CargaGrid_Software(0)
        Call CargaGrid_PreciosCompras(0)
        'Call CargaGrid_PreciosAnteriores(0)


        AvisoCantidadSuperiorAUno = ""
        AvisoVinculacionArticuloFamiliaNoTraspasar = ""

        ErrorProvider1.Clear()
        'Call CargaGrid_Productos()
        Call CargaCombo_Proveidor(0)
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Producto_Codigo)
                oClsControls.ControlBuit(.T_Producto_Descripcion)
                oClsControls.ControlBuit(.T_IVA)
                oClsControls.ControlBuit(.T_Preu)
                oClsControls.ControlBuit(.T_Unidades)
                oClsControls.ControlBuit(.C_Emplazamiento)
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

    Private Sub ActivarPantalla(ByVal Activar As Boolean)
        If Activar = True Then
            Util.Activar(Me, M_Util.Enum_Controles_Activacion.TODOS_MENOS_ALGUNOS, True, Me.C_Planta, Me.C_Zona, Me.C_Abertura, Me.C_ElementosAProteger)  'Me.C_Planta, Me.C_Zona, Me.C_Abertura, Me.C_ElementosAProteger
        Else
            Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
        End If
        Me.TE_Producto_Codigo.ReadOnly = Activar

        'Fem això per no permetre vincular un articule central amb unaltre article
        If Me.TE_Producto_Codigo.Tag Is Nothing = False Then
            Dim ooDTC As New DTCDataContext(BD.Conexion)
            Dim ooLinqProducto As Producto = ooDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Producto_Codigo.Text).FirstOrDefault()
            If ooLinqProducto Is Nothing = False Then
                If ooLinqProducto.Central = True Then
                    Me.C_Vinculacion.ReadOnly = True
                    Me.C_Vinculacion_Energetica.ReadOnly = True
                Else
                    Me.C_Vinculacion.ReadOnly = False
                    Me.C_Vinculacion_Energetica.ReadOnly = False
                End If
            End If

            If ooLinqProducto.ID_Producto_Division = EnumProductoDivision.Informatica Then
                Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "UsuariosSistema")
            Else
                Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "UsuariosSistema")
            End If
        End If

        Me.C_Emplazamiento.ReadOnly = False
        Me.C_Planta.ReadOnly = True
        Me.C_Zona.ReadOnly = True
        Me.C_Abertura.ReadOnly = True
        Me.C_ElementosAProteger.ReadOnly = True
        If IsNothing(Me.C_Emplazamiento.Value) = False AndAlso IsNumeric(Me.C_Emplazamiento.Value) Then
            Me.C_Planta.ReadOnly = False
            If IsNothing(Me.C_Planta.Value) = False AndAlso IsNumeric(Me.C_Planta.Value) Then
                Me.C_Zona.ReadOnly = False
                If IsNothing(Me.C_Zona.Value) = False AndAlso IsNumeric(Me.C_Zona.Value) Then
                    Me.C_Abertura.ReadOnly = False
                    Me.C_ElementosAProteger.ReadOnly = False
                End If
            End If
        End If
    End Sub

    Private Sub CargaPreuProductoYTraspasable(ByVal pIDProducto As Integer)
        Try
            Dim ooDTC As New DTCDataContext(BD.Conexion)
            Dim ooLinqProducto As Producto = ooDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Producto_Codigo.Text).FirstOrDefault()

            Me.C_Traspasable.Value = ooLinqProducto.Producto_SubFamilia.ID_Producto_SubFamilia_Traspaso
            Me.R_DescripcionAmpliada.pText = ooLinqProducto.DescripcionAmpliada
            Me.R_DescripcionAmpliada_Tecnica.pText = ooLinqProducto.DescripcionAmpliada_Tecnica

            Dim _PVP As Decimal
            Dim _PVD As Decimal

            Call RetornaPVPiPVD(pIDProducto, _PVP, _PVD)

            Me.T_Preu.Value = _PVP
            Me.T_Coste.Value = _PVD
            Me.T_PlazoEntrega.Value = ooLinqProducto.PlazoEntrega

            'Tot lo de abaix es per carregar el combo de proveidors amb els proveidors del article seleccionat i si ni ha un de predeterminat el carregarem també
            Call CargaCombo_Proveidor(pIDProducto)
            Dim _Prod_Prov = ooLinqProducto.Producto_Proveedor.Where(Function(F) F.Predeterminado = True).FirstOrDefault
            If _Prod_Prov Is Nothing = False Then
                Me.C_Proveedor.Value = _Prod_Prov.ID_Proveedor
            End If

            Call CargarFoto()

            Call CargaGrid_PreciosAnteriores(pIDProducto)
            Call CargaGrid_PreciosCompras(pIDProducto)

            'Al seleccionar un article, si aquest article te en la descripcio (p) dels fitxers adjunts llavors els copiarem al pressupost
            Dim _Producto_Archivo As Producto_Archivo
            For Each _Producto_Archivo In ooLinqProducto.Producto_Archivo
                If _Producto_Archivo.Archivo.Descripcion.Contains("(p)") = True Then
                    'si ja s'ha copiat no el tornarem a copiar
                    If oLinqPropuesta.Propuesta_Archivo.Where(Function(F) F.Archivo.Descripcion = _Producto_Archivo.Archivo.Descripcion).Count = 0 Then
                        Dim _Archivo As Archivo
                        _Archivo = _Producto_Archivo.Archivo

                        Dim _NewArchivo As New Archivo
                        Dim _NewPropuestaArchivo As New Propuesta_Archivo
                        _NewArchivo.Activo = True
                        _NewArchivo.CampoBinario = _Archivo.CampoBinario
                        _NewArchivo.Color = _Archivo.Color
                        _NewArchivo.Descripcion = _Archivo.Descripcion
                        _NewArchivo.Fecha = _Archivo.Fecha
                        _NewArchivo.ID_Usuario = _Archivo.ID_Usuario
                        _NewArchivo.Ruta_Fichero = _Archivo.Ruta_Fichero
                        _NewArchivo.Tamaño = _Archivo.Tamaño
                        _NewArchivo.Tipo = _Archivo.Tipo

                        _NewPropuestaArchivo.Archivo = _NewArchivo
                        _NewPropuestaArchivo.Propuesta = oLinqPropuesta
                        oLinqPropuesta.Propuesta_Archivo.Add(_NewPropuestaArchivo)
                        oDTC.Archivo.InsertOnSubmit(_NewArchivo)
                        oDTC.Propuesta_Archivo.InsertOnSubmit(_NewPropuestaArchivo)
                    End If
                End If
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargaCombo_Proveidor(ByVal pID As Integer)

        Dim DT As DataTable = BD.RetornaDataTable("SELECT  Proveedor.ID_Proveedor, Nombre, PVD, PVP From Proveedor, Producto_Proveedor Where Proveedor.ID_Proveedor=Producto_Proveedor.ID_Proveedor and ID_Producto=" & pID & " Order by PVD Desc")
        Me.C_Proveedor.DataSource = DT
        If DT Is Nothing Then
            Exit Sub
        End If

        With C_Proveedor
            .MaxDropDownItems = 8
            .DisplayMember = "Nombre"
            .ValueMember = "ID_Proveedor"
            .DisplayLayout.Bands(0).Columns("ID_Proveedor").Hidden = True
            .DisplayLayout.Bands(0).Columns("Nombre").Width = 250
            .DisplayLayout.Bands(0).Columns("Nombre").Header.Caption = "Descripción"
            .DisplayLayout.Bands(0).Columns("PVD").Format = "###,###,##0.00 €"
            .DisplayLayout.Bands(0).Columns("PVD").CellAppearance.TextHAlign = HAlign.Right
            .DisplayLayout.Bands(0).Columns("PVD").Width = 50
            .DisplayLayout.Bands(0).Columns("PVD").Header.Caption = "PVD"
            .DisplayLayout.Bands(0).Columns("PVP").Format = "###,###,##0.00 €"
            .DisplayLayout.Bands(0).Columns("PVP").CellAppearance.TextHAlign = HAlign.Right
            .DisplayLayout.Bands(0).Columns("PVP").Width = 50
            .DisplayLayout.Bands(0).Columns("PVP").Header.Caption = "PVP"
        End With

        'Me.C_Vinculacion.DisplayLayout.Bands(0).Override.AllowRowFiltering = DefaultableBoolean.True
    End Sub

    Public Shared Sub RetornaPVPiPVD(ByVal pIDProducto As Integer, ByRef pPVP As Decimal, ByRef pPVD As Decimal)
        Try
            Dim ooDTC As New DTCDataContext(BD.Conexion)
            Dim ooLinqProducto As Producto = ooDTC.Producto.Where(Function(F) F.ID_Producto = pIDProducto).FirstOrDefault()

            If ooLinqProducto Is Nothing = False Then
                If ooLinqProducto.PVP_Proveedor_Predeterminado = False Then
                    pPVP = Util.Comprobar_NULL_Per_0_Decimal(ooLinqProducto.PVP)
                    pPVD = Util.Comprobar_NULL_Per_0_Decimal(ooLinqProducto.PVD)
                Else
                    If ooLinqProducto.Producto_Proveedor.Where(Function(F) F.ID_Producto = pIDProducto And F.Predeterminado = True).Count = 1 Then
                        pPVP = Util.Comprobar_NULL_Per_0_Decimal(ooLinqProducto.Producto_Proveedor.Where(Function(F) F.ID_Producto = pIDProducto And F.Predeterminado = True).FirstOrDefault.PVP)
                        pPVD = Util.Comprobar_NULL_Per_0_Decimal(ooLinqProducto.Producto_Proveedor.Where(Function(F) F.ID_Producto = pIDProducto And F.Predeterminado = True).FirstOrDefault.PVD)
                    Else
                        pPVP = 0
                        pPVD = 0
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub AlTancarLlistatGeneric(ByVal pID As String)
        If pID Is Nothing Then
        Else
            Call ActivarPantalla(True)
            Call CargaPreuProductoYTraspasable(Me.TE_Producto_Codigo.Tag)

        End If

    End Sub

    Private Sub CalcularTotalBase()
        Dim _Preu As Decimal = 0
        Dim _Quantitat As Decimal = 0
        Dim _Descompte As Decimal = 0

        If Me.T_Preu.Value Is Nothing = False AndAlso IsNumeric(Me.T_Preu.Value) = True Then
            _Preu = Me.T_Preu.Value
        End If

        If Me.T_Unidades.Value Is Nothing = False AndAlso IsNumeric(Me.T_Unidades.Value) = True Then
            _Quantitat = Me.T_Unidades.Value
        End If

        If Me.T_Descuento.Value Is Nothing = False AndAlso IsNumeric(Me.T_Descuento.Value) = True Then
            _Descompte = Me.T_Descuento.Value
        End If

        Me.T_TotalBase.Value = (_Quantitat * _Preu - ((_Quantitat * _Preu) * _Descompte) / 100)
        If Me.T_TotalCoste.Value Is Nothing = False Then
            Me.T_Margen.Value = Me.T_TotalBase.Value - Util.Comprobar_NULL_Per_0_Decimal(Me.T_TotalCoste.Value)
        Else
            Me.T_Margen.Value = Me.T_TotalBase.Value
        End If

        Me.T_Preu.Appearance.ForeColor = Color.Black
        If Me.TE_Producto_Codigo.Tag Is Nothing = False Then
            If Util.Comprobar_NULL_Per_0_Decimal(Me.T_Coste.Value) > Util.Comprobar_NULL_Per_0_Decimal(Me.T_Preu.Value) Then
                Me.T_Preu.Appearance.ForeColor = Color.Coral
            End If
        End If

    End Sub

    Private Sub CalcularTotalCoste()
        Dim _Preu As Decimal = 0
        Dim _Quantitat As Decimal = 0

        If Me.T_Coste.Value Is Nothing = False AndAlso IsNumeric(Me.T_Coste.Value) = True Then
            _Preu = Me.T_Coste.Value
        End If

        If Me.T_Unidades.Value Is Nothing = False AndAlso IsNumeric(Me.T_Unidades.Value) = True Then
            _Quantitat = Me.T_Unidades.Value
        End If

        Me.T_TotalCoste.Value = _Quantitat * _Preu

        If Me.T_TotalBase.Value Is Nothing = False Then
            Me.T_Margen.Value = Me.T_TotalBase.Value - Me.T_TotalCoste.Value
        Else
            Me.T_Margen.Value = Me.T_TotalCoste.Value * -1
        End If

        Me.T_Coste.Appearance.ForeColor = Color.Black
        If Me.TE_Producto_Codigo.Tag Is Nothing = False Then
            Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag) And F.Entrada.ID_Entrada_Tipo = CInt(EnumEntradaTipo.AlbaranCompra)).OrderBy(Function(F) F.FechaEntrada).FirstOrDefault
            If _Linea Is Nothing = False Then
                If Me.T_Coste.Value < _Linea.Precio Then
                    Me.T_Coste.Appearance.ForeColor = Color.Coral
                End If
            End If
        End If

    End Sub

    Private Function RetornaNivellMaxim() As Integer
        Dim _NivellMaxim As Integer = 1
        'Dim _NivellTemporal As Integer = 1
        Dim _Linea As Propuesta_Linea

        For Each _Linea In oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = False)
            Dim _NivellActual As Integer = 0
            Call RecursivitatLineas(_Linea, _NivellActual, _NivellMaxim)
            If _NivellActual > _NivellMaxim Then
                _NivellMaxim = _NivellActual
            End If
        Next
        Return _NivellMaxim
    End Function

    Private Sub RecursivitatLineas(ByVal pLinea As Propuesta_Linea, ByRef pNumNivellActual As Integer, ByRef pNumMaxNivell As Integer)
        'Dim _NivellTemporal As Integer = 1

        pNumNivellActual = pNumNivellActual + 1
        Dim _Linea2 As Propuesta_Linea
        For Each _Linea2 In pLinea.Propuesta_Linea1
            Call RecursivitatLineas(_Linea2, pNumNivellActual, pNumMaxNivell)
        Next
        If pNumNivellActual > pNumMaxNivell Then
            pNumMaxNivell = pNumNivellActual
        End If
        pNumNivellActual = pNumNivellActual - 1
    End Sub

    Private Sub RecursivitatLineasAvall(ByVal pLinea As Propuesta_Linea, ByRef pNumNivellActual As Integer, ByRef pNumMaxNivell As Integer)
        'Dim _NivellTemporal As Integer = 1


        'Dim _Linea2 As Propuesta_Linea
        If pLinea.Propuesta_Linea Is Nothing = False Then
            pNumNivellActual = pNumNivellActual + 1
            Call RecursivitatLineasAvall(pLinea.Propuesta_Linea, pNumNivellActual, pNumMaxNivell)
        End If
        If pNumNivellActual > pNumMaxNivell Then
            pNumMaxNivell = pNumNivellActual
        End If
        pNumNivellActual = pNumNivellActual - 1
    End Sub

    Private Sub CalcularNivellsLineaActual()
        Dim TotalNivells As Integer
        Dim _NivellMaxim As Integer = 0
        Dim _NivellActual As Integer = 0


        Call RecursivitatLineas(oLinqPropuesta_Linea, _NivellActual, _NivellMaxim)
        TotalNivells = _NivellMaxim

        _NivellMaxim = 0
        _NivellActual = 0

        Call RecursivitatLineasAvall(oLinqPropuesta_Linea, _NivellActual, _NivellMaxim)

        TotalNivells = TotalNivells + _NivellMaxim
        If oLinqPropuesta.NivelMaximoLineas.HasValue = False OrElse TotalNivells > oLinqPropuesta.NivelMaximoLineas Then
            oLinqPropuesta.NivelMaximoLineas = TotalNivells
        End If
    End Sub

    Private Function CrearFills(ByRef pLinea As Propuesta_Linea, ByVal pIdentificadorLinea As Integer) As Integer
        Try
            CrearFills = pIdentificadorLinea
            'Funció que crea fills introduits a la pestanya productes
            If Me.GRD_Productos.GRID.Rows.Count = 0 Then
                Exit Function
            End If

            Dim _Row As UltraGridRow
            For Each _Row In Me.GRD_Productos.GRID.Rows
                Dim _ID As Integer
                If IsNumeric(_Row.Cells("ID_Producto").Value) = True AndAlso oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(_Row.Cells("ID_Producto").Value)).Count > 0 Then
                    pIdentificadorLinea = pIdentificadorLinea + 10
                    _ID = _Row.Cells("ID_Producto").Value

                    Dim _PVP As Decimal = 0
                    Dim _PVD As Decimal = 0
                    Dim _Descuento As Decimal = 0
                    Dim _IVA As Decimal = 0

                    If oLinqPropuesta.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior AndAlso oLinqPropuesta.SeInstalo <> True Then
                        _PVP = _Row.Cells("PVP").Value
                        _PVD = _Row.Cells("PVD").Value
                        _Descuento = _Row.Cells("Descuento").Value
                        _IVA = _Row.Cells("IVA").Value
                    End If

                    clsPropuestaLinea.AssignaFillALaLinea(pLinea, _ID, oDTC, pIdentificadorLinea, _Row.Cells("Cantidad").Value, _PVP, _PVD, _Descuento, _IVA)

                End If
            Next

            Return pIdentificadorLinea

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub CarregarAvisos()

        If oLinqPropuesta Is Nothing OrElse oLinqPropuesta.SeInstalo = True Then
            Exit Sub
        End If

        Me.L_Aviso.Text = ""
        AvisoCantidadSuperiorAUno = ""

        If IsNothing(Me.T_Unidades.Value) = False AndAlso IsDBNull(Me.T_Unidades.Value) = False Then
            If Me.T_Unidades.Value > 1 Then
                AvisoCantidadSuperiorAUno = "Atención! Las líneas de la propuesta con cantidades superiores a 1 no podran ser vinculadas des de otras líneas de propuesta"
            End If
        End If

        AvisoVinculacionArticuloFamiliaNoTraspasar = ""

        If Me.C_Vinculacion.SelectedRow Is Nothing = False Then
            Dim _Propuesta_Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(Me.C_Vinculacion.Value)).FirstOrDefault
            If _Propuesta_Linea.ID_Producto_SubFamilia_Traspaso = EnumProductoSubFamiliaTraspaso.No Then
                AvisoVinculacionArticuloFamiliaNoTraspasar = "Atención! Está vinculando la línea de propuesta a una línea de propuesta que no se va a traspasar a 'Tal y como se instaló'"
            End If
        End If

        If Me.C_Vinculacion_Energetica.SelectedRow Is Nothing = False Then
            Dim _Propuesta_Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(Me.C_Vinculacion_Energetica.Value)).FirstOrDefault
            If _Propuesta_Linea.ID_Producto_SubFamilia_Traspaso = EnumProductoSubFamiliaTraspaso.No Then
                AvisoVinculacionArticuloFamiliaNoTraspasar = "Atención! Está vinculando energéticamente la línea de propuesta a una línea de propuesta que no se va a traspasar a 'Tal y como se instaló'"
            End If
        End If

        If AvisoCantidadSuperiorAUno.Length > 0 Then
            Me.L_Aviso.Text = AvisoCantidadSuperiorAUno
        End If

        If AvisoVinculacionArticuloFamiliaNoTraspasar.Length > 0 Then
            Me.L_Aviso.Text = Me.L_Aviso.Text & vbCrLf & AvisoVinculacionArticuloFamiliaNoTraspasar
        End If

        Me.L_Aviso.UseAppStyling = False
        Me.L_Aviso.Appearance.BackColor = Color.Transparent
        Me.L_Aviso.Appearance.ForeColor = Color.Red
    End Sub

    Private Sub CargarFoto()
        Me.Foto.Image = Nothing
        If oLinqPropuesta_Linea.ID_Archivo_FotoPredeterminada.HasValue = True Then
            If oLinqPropuesta_Linea.Archivo.CampoBinario Is Nothing = False Then
                Me.Foto.Image = Util.BinaryToImage(oLinqPropuesta_Linea.Archivo.CampoBinario.ToArray)
            End If
            ' Me.UltraPictureBox1.Image = Image.FromFile(Fichero.ExtreuIRetornaRutaArxiu(oLinqProducto.ID_Archivo_FotoPredeterminada))
        Else
            If Me.TE_Producto_Codigo.Tag Is Nothing = False Then
                Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
                If _Producto.ID_Archivo_FotoPredeterminada.HasValue = True Then
                    If _Producto.Archivo.CampoBinario Is Nothing = False Then
                        Me.Foto.Image = Util.BinaryToImage(_Producto.Archivo.CampoBinario.ToArray)
                    End If
                End If

            End If
        End If
    End Sub

    Private Sub DespresDeCarregarDadesGridArchivos()
        Try
            If Me.GRD_Ficheros.GRID.Rows.Count > 0 Then
                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Ficheros.GRID.Rows
                    If oLinqPropuesta_Linea.ID_Archivo_FotoPredeterminada.HasValue = True AndAlso pRow.Cells("ID_Archivo").Value = oLinqPropuesta_Linea.ID_Archivo_FotoPredeterminada Then
                        pRow.CellAppearance.BackColor = Color.LightGreen
                    Else
                        pRow.CellAppearance.BackColor = Color.White
                    End If
                Next
                Me.GRD_Ficheros.GRID.ActiveRow = Nothing
                Me.GRD_Ficheros.GRID.Selected.Rows.Clear()


            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub DespresDeEliminarElRegistreArchivos(ByVal pIDArchivo As Integer)
        If pIDArchivo = oLinqPropuesta_Linea.ID_Archivo_FotoPredeterminada Then
            oLinqPropuesta_Linea.ID_Archivo_FotoPredeterminada = Nothing
            oDTC.SubmitChanges()
        End If
    End Sub

    Private Sub CarregarDadesPestanyes(ByVal pKeyPestanya As String)
        Try

            Dim _ID As Integer = oLinqPropuesta_Linea.ID_Propuesta_Linea

            Select Case pKeyPestanya
                Case "Detalle"
                    Call CargarFoto()
                Case "DatosAcceso"
                    Call CargaGrid_Accesos(oLinqPropuesta_Linea.ID_Propuesta_Linea)
                Case "UsuariosSistema"
                    Call CargaGrid_UsuariosSistema(oLinqPropuesta_Linea.ID_Propuesta_Linea)
                Case "PreciosAnteriores"
                    Call CargaGrid_PreciosAnteriores(Me.TE_Producto_Codigo.Tag)
                Case "AsignacionProductos"
                    Call CargaGrid_Productos()
            End Select


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Events Varis"

    Private Sub TE_Producto_Codigo_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Producto_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto Where Fecha_Baja is  null ORDER BY Descripcion", Me.TE_Producto_Codigo, "ID_Producto", "Codigo", Me.T_Producto_Descripcion, "Descripcion")
            AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGeneric
            AddHandler LlistatGeneric.AlApretarElBotoAuxiliar, AddressOf AlApretarElBotoAuxiliarDelLlistatGeneric

            Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
            For Each pRow In LlistatGeneric.pGrid.GRID.Rows
                If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
                    pRow.Appearance.BackColor = Color.LightCoral
                End If
            Next

            Dim _RulCondition As New DevExpress.XtraEditors.FormatConditionRuleValue
            _RulCondition.Condition = DevExpress.XtraEditors.FormatCondition.NotEqual
            _RulCondition.Appearance.BackColor = Color.LightCoral
            _RulCondition.Value1 = Nothing

            Dim _FormatRule As New DevExpress.XtraGrid.GridFormatRule()
            Dim _View As DevExpress.XtraGrid.Views.Grid.GridView
            _View = LlistatGeneric.pGridDevExpress.Views(0)
            _FormatRule.Column = _View.Columns("Fecha_Baja")
            _FormatRule.ApplyToRow = True
            _FormatRule.Rule = _RulCondition
            _View.FormatRules.Add(_FormatRule)


            LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = True
            LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Caption = "Incluir productos en estado 'baja'"


        End If
        If e.Button.Key = "Cancelar" Then
            Call B_Cancelar_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub AlApretarElBotoAuxiliarDelLlistatGeneric(ByRef pInstanciaLlistatGeneric As M_LlistatGeneric.clsLlistatGeneric)
        'Això es només per traspasos de magatzems
        pInstanciaLlistatGeneric.pGrid.M.clsUltraGrid.Cargar("SELECT * FROM C_Producto ORDER BY Descripcion", BD)
        pInstanciaLlistatGeneric.AplicarCanvisBotoAuxiliarAlNouGrid()
        pInstanciaLlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = False

    End Sub

    Private Sub TE_Producto_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Producto_Codigo.KeyDown
        If Me.TE_Producto_Codigo.Text Is Nothing = False Then
            If e.KeyCode = Keys.Enter Then
                Dim ooLinqProducto As Producto = oDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Producto_Codigo.Text).FirstOrDefault()
                If ooLinqProducto Is Nothing = False Then
                    Me.TE_Producto_Codigo.Tag = ooLinqProducto.ID_Producto
                    Me.T_Producto_Descripcion.Text = ooLinqProducto.Descripcion
                    Me.C_Traspasable.Value = ooLinqProducto.Producto_SubFamilia.ID_Producto_SubFamilia_Traspaso
                    Call CargaPreuProductoYTraspasable(ooLinqProducto.ID_Producto)
                    Call ActivarPantalla(True)
                    If Me.T_Unidades.Visible = True Then
                        Me.T_Unidades.Focus()
                    Else
                        Me.T_Producto_Descripcion.Focus()
                    End If
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Me.TE_Producto_Codigo.Value = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub C_Emplazamiento_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Emplazamiento.ValueChanged
        If Me.C_Emplazamiento.Value Is Nothing = False Then
            Me.C_Planta.ReadOnly = False
            Me.C_Zona.ReadOnly = True
            Me.C_ElementosAProteger.ReadOnly = True
            Me.C_Abertura.ReadOnly = True
            Me.C_Planta.Value = Nothing
            Me.C_Zona.Value = Nothing
            Me.C_ElementosAProteger.Value = Nothing
            Me.C_Abertura.Value = Nothing

            If IsNumeric(Me.C_Emplazamiento.Value) = True Then
                Util.Cargar_Combo(Me.C_Planta, "SELECT ID_Instalacion_Emplazamiento_Planta, Descripcion FROM Instalacion_Emplazamiento_Planta WHERE ID_Instalacion_Emplazamiento=" & Me.C_Emplazamiento.Value & " ORDER BY Descripcion", False)
            End If
        End If
    End Sub

    Private Sub C_Planta_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Planta.ValueChanged
        If Me.C_Planta.Value Is Nothing = False Then
            Me.C_Zona.ReadOnly = False
            Me.C_ElementosAProteger.ReadOnly = True
            Me.C_Abertura.ReadOnly = True
            Me.C_Zona.Value = Nothing
            Me.C_ElementosAProteger.Value = Nothing
            Me.C_Abertura.Value = Nothing

            If IsNumeric(Me.C_Planta.Value) = True Then
                Util.Cargar_Combo(Me.C_Zona, "SELECT ID_Instalacion_Emplazamiento_Zona, Descripcion FROM Instalacion_Emplazamiento_Zona WHERE ID_Instalacion_Emplazamiento_Planta=" & Me.C_Planta.Value & " ORDER BY Descripcion", False)
            End If
        End If
    End Sub

    Private Sub C_Zona_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Zona.ValueChanged
        If Me.C_Zona.Value Is Nothing = False Then
            Me.C_ElementosAProteger.ReadOnly = False
            Me.C_Abertura.ReadOnly = False
            Me.C_ElementosAProteger.Value = Nothing
            Me.C_Abertura.Value = Nothing

            If IsNumeric(Me.C_Zona.Value) = True Then
                Util.Cargar_Combo(Me.C_ElementosAProteger, "SELECT ID_Instalacion_ElementosAProteger, Descripcion FROM Instalacion_ElementosAProteger, Instalacion_ElementosAProteger_Tipo WHERE Instalacion_ElementosAProteger.ID_Instalacion_ElementosAProteger_Tipo=Instalacion_ElementosAProteger_Tipo.ID_Instalacion_ElementosAProteger_Tipo and  ID_Instalacion_Emplazamiento_Zona=" & Me.C_Zona.Value & " ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Abertura, "SELECT ID_Instalacion_Emplazamiento_Abertura, Descripcion_Detallada FROM Instalacion_Emplazamiento_Abertura, Instalacion_Emplazamiento_Abertura_Elemento WHERE Instalacion_Emplazamiento_Abertura.ID_Instalacion_Emplazamiento_Abertura_Elemento=Instalacion_Emplazamiento_Abertura_Elemento.ID_Instalacion_Emplazamiento_Abertura_Elemento and  ID_Instalacion_Emplazamiento_Zona=" & Me.C_Zona.Value & " ORDER BY Descripcion", False)
            End If
        End If
    End Sub

    Private Sub T_Unidades_ValueChanged(sender As Object, e As System.EventArgs) Handles T_Unidades.ValueChanged
        Call CarregarAvisos()
        Call CalcularTotalBase()
        Call CalcularTotalCoste()
    End Sub

    Private Sub T_Preu_ValueChanged(sender As Object, e As System.EventArgs) Handles T_Preu.ValueChanged
        Call CalcularTotalBase()
    End Sub

    Private Sub T_Descuento_ValueChanged(sender As Object, e As System.EventArgs) Handles T_Descuento.ValueChanged
        Call CalcularTotalBase()
    End Sub

    Private Sub T_Coste_ValueChanged(sender As Object, e As EventArgs) Handles T_Coste.ValueChanged
        Call CalcularTotalCoste()
    End Sub

    Private Sub C_Vinculacion_ValueChanged(sender As Object, e As System.EventArgs)
        Call CarregarAvisos()
    End Sub

    Private Sub B_Cancelar_Click(sender As System.Object, e As System.EventArgs) Handles B_Cancelar.Click
        Me.TE_Producto_Codigo.Text = Nothing
        Me.TE_Producto_Codigo.Tag = Nothing
        Me.T_Producto_Descripcion.Text = Nothing
        Me.Foto.Image = Nothing
        Call Netejar_Pantalla()
        Call ActivarPantalla(False)

    End Sub

    Private Sub Tab_Principal_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles Tab_Principal.SelectedTabChanged
        Call CarregarDadesPestanyes(e.Tab.Key)
    End Sub

    Private Sub C_Abertura_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Abertura.EditorButtonClick
        If e.Button.Key = "Cancelar" Then
            If C_Abertura.Value Is Nothing = False Then 'fem això pq si s'apreta el botó cancelar i no hi ha cap registre seleccionat llavors no faci re
                Me.C_Abertura.Value = Nothing
                Me.C_ElementosAProteger.ReadOnly = False
            End If
        End If
    End Sub

    Private Sub C_Abertura_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Abertura.ValueChanged
        Me.C_ElementosAProteger.Value = Nothing
        Me.C_ElementosAProteger.ReadOnly = True
    End Sub

    Private Sub C_ElementosAProteger_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_ElementosAProteger.EditorButtonClick
        If e.Button.Key = "Cancelar" Then
            If C_ElementosAProteger.Value Is Nothing = False Then  'fem això pq si s'apreta el botó cancelar i no hi ha cap registre seleccionat llavors no faci re
                Me.C_ElementosAProteger.Value = Nothing
                Me.C_Abertura.ReadOnly = False
            End If
        End If
    End Sub

    Private Sub C_ElementosAProteger_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_ElementosAProteger.ValueChanged
        Me.C_Abertura.Value = Nothing
        Me.C_Abertura.ReadOnly = True
    End Sub

    Private Sub C_Vinculacion_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Vinculacion.EditorButtonClick
        Me.C_Vinculacion.Value = Nothing
    End Sub

    Private Sub C_Vinculacion_Energetica_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Vinculacion_Energetica.EditorButtonClick
        Me.C_Vinculacion_Energetica.Value = Nothing
    End Sub

    Private Sub C_Planta_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Planta.EditorButtonClick
        Me.C_Planta.Value = Nothing
        Me.C_Zona.Value = Nothing
        Me.C_Abertura.Value = Nothing
        Me.C_ElementosAProteger.Value = Nothing
        Me.C_Zona.ReadOnly = True
        Me.C_Abertura.ReadOnly = True
        Me.C_ElementosAProteger.ReadOnly = True
    End Sub

    Private Sub C_InstaladoEn_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_InstaladoEn.BeforeDropDown
        ' Util.Cargar_Combo(Me.C_InstaladoEn, "SELECT ID_Instalacion_InstaladoEn, Identificador + '-' + Descripcion FROM Instalacion_InstaladoEn Where ID_Instalacion=" & oLinqInstalacion.ID_Instalacion & " ORDER BY Descripcion", False)
    End Sub

    Private Sub C_InstaladoEn_EditorButtonClick(sender As Object, e As UltraWinEditors.EditorButtonEventArgs) Handles C_InstaladoEn.EditorButtonClick
        If e.Button.Key = "Cancelar" Then
            If C_InstaladoEn.Value Is Nothing = False Then 'fem això pq si s'apreta el botó cancelar i no hi ha cap registre seleccionat llavors no faci re
                Me.C_InstaladoEn.Value = Nothing
            End If
        End If
    End Sub

    Private Sub C_Proveedor_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Proveedor.EditorButtonClick
        If e.Button.Key = "Cancelar" Then
            If C_Proveedor.Value Is Nothing = False Then 'fem això pq si s'apreta el botó cancelar i no hi ha cap registre seleccionat llavors no faci re
                Me.C_Proveedor.Value = Nothing
            End If
        End If
    End Sub

    Private Sub C_Proveedor_ValueChanged(sender As Object, e As EventArgs) Handles C_Proveedor.ValueChanged
        Try
            If Me.C_Proveedor.SelectedRow Is Nothing = False Then
                Me.T_Coste.Value = Me.C_Proveedor.SelectedRow.Cells("PVD").Value
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Ficheros_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Ficheros.M_ToolGrid_ToolClickBotonsExtras
        Try
            If e.Tool.Key = "FotoPredeterminada" AndAlso Me.GRD_Ficheros.GRID.Selected.Rows.Count = 1 Then
                Dim _IDArchivo As Integer = Me.GRD_Ficheros.GRID.Selected.Rows(0).Cells("ID_Archivo").Value
                Dim _Archivo As Archivo
                _Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = _IDArchivo).FirstOrDefault
                If _Archivo.Tipo.ToString.ToLower.Contains("image") = True Or _Archivo.Tipo.ToString.ToLower.Contains("jpg") = True Or _Archivo.Tipo.ToString.ToLower.Contains("jpeg") = True Or _Archivo.Tipo.ToString.ToLower.Contains("png") = True Or _Archivo.Tipo.ToString.ToLower.Contains("tiff") = True Or _Archivo.Tipo.ToString.ToLower.Contains("gif") = True Or _Archivo.Tipo.ToString.ToLower.Contains("bmp") = True Then
                Else
                    Mensaje.Mostrar_Mensaje("Sólo se pueden predeterminar ficheros de tipo imagen", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
                oLinqPropuesta_Linea.Archivo = _Archivo
                oDTC.SubmitChanges()
                Call DespresDeCarregarDadesGridArchivos()
            End If

            If e.Tool.Key = "QuitarFotoPredeterminada" Then
                'Dim _IDArchivo As Integer = Me.GRD_Ficheros.GRID.Selected.Rows(0).Cells("ID_Archivo").Value
                'Dim _Archivo As Archivo
                '_Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = _IDArchivo).FirstOrDefault
                'If _Archivo.ID_Archivo = oLinqPropuesta_Linea.ID_Archivo_FotoPredeterminada Then
                oLinqPropuesta_Linea.Archivo = Nothing
                oDTC.SubmitChanges()
                'End If
                Call DespresDeCarregarDadesGridArchivos()
            End If

            '
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Ficheros_M_ToolGrid_ToolEliminar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Ficheros.M_ToolGrid_ToolEliminar
        oLinqPropuesta_Linea.Archivo = Nothing
        oDTC.SubmitChanges()
    End Sub

    Private Sub C_Vinculacion_RowSelected(sender As Object, e As RowSelectedEventArgs) Handles C_Vinculacion.RowSelected
        Try

            If Me.C_Planta.Text <> "" Then  'si no hi ha planta seleccionada, omplirem les dades d'ubicació generals amb el producte vinculat
                Exit Sub
            End If

            Dim _IDLinea As Integer = e.Row.Cells("ID_Propuesta_Linea").Value
            Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _IDLinea).FirstOrDefault

            If _Linea.ID_Instalacion_Emplazamiento.HasValue = True Then
                Me.C_Emplazamiento.Value = _Linea.ID_Instalacion_Emplazamiento
            End If

            If _Linea.ID_Instalacion_Emplazamiento_Planta.HasValue = True Then
                Me.C_Planta.Value = _Linea.ID_Instalacion_Emplazamiento_Planta
            End If

            If _Linea.ID_Instalacion_Emplazamiento_Zona.HasValue = True Then
                Me.C_Zona.Value = _Linea.ID_Instalacion_Emplazamiento_Zona
            End If

            If _Linea.ID_Instalacion_Emplazamiento_Abertura.HasValue = True Then
                Me.C_Abertura.Value = _Linea.ID_Instalacion_Emplazamiento_Abertura
            End If

            If _Linea.ID_Instalacion_ElementosAProteger.HasValue = True Then
                Me.C_ElementosAProteger.Value = _Linea.ID_Instalacion_ElementosAProteger
            End If


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub C_Vinculacion_ValueChanged1(sender As Object, e As EventArgs) Handles C_Vinculacion.ValueChanged

    End Sub

    Private Sub C_Opcion_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Opcion.EditorButtonClick
        If e.Button.Key = "Cancelar" Then
            Me.C_Opcion.Value = Nothing
        End If
    End Sub

    Private Sub B_Adelante_Click(sender As Object, e As EventArgs) Handles B_Adelante.Click
        Call AvanzarRetroceder(True)
    End Sub

    Private Sub B_Atras_Click(sender As Object, e As EventArgs) Handles B_Atras.Click
        Call AvanzarRetroceder(False)
    End Sub

    Private Sub AvanzarRetroceder(ByVal pAvanzar As Boolean)
        'Que pasa si no s'ha seleccinat cap article?
        Util.WaitFormObrir()
        Dim _IDATrobar As Integer
        Dim _Trobat As Boolean = False

        If oLinqPropuesta_Linea.ID_Propuesta_Linea = 0 Then
            'Si actualment no tenim cap registre carregat farem veure com si estiguessim al últim.
            _IDATrobar = oLinqPropuesta.Propuesta_Linea.Max(Function(F) F.ID_Propuesta_Linea)
            _Trobat = True
        Else
            _IDATrobar = oLinqPropuesta_Linea.ID_Propuesta_Linea
        End If

        Dim _LlistatLinies As IList(Of Propuesta_Linea)
        Select Case Me.OP_Filtre.Value
            Case "Orden"
                If pAvanzar = True Then
                    _LlistatLinies = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta = oLinqPropuesta.ID_Propuesta).OrderBy(Function(F) F.ID_Propuesta_Linea).ToList
                Else
                    _LlistatLinies = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta = oLinqPropuesta.ID_Propuesta).OrderByDescending(Function(F) F.ID_Propuesta_Linea).ToList
                End If
            Case "Articulo"
                If pAvanzar = True Then
                    _LlistatLinies = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta = oLinqPropuesta.ID_Propuesta And F.ID_Producto = oLinqPropuesta_Linea.ID_Producto).OrderBy(Function(F) F.ID_Propuesta_Linea).ToList
                Else
                    _LlistatLinies = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta = oLinqPropuesta.ID_Propuesta And F.ID_Producto = oLinqPropuesta_Linea.ID_Producto).OrderByDescending(Function(F) F.ID_Propuesta_Linea).ToList
                End If
        End Select


        Dim _Linea As Propuesta_Linea
        Dim _LineaSeguent As Propuesta_Linea
        For Each _Linea In _LlistatLinies
            If _Trobat = True Then
                _LineaSeguent = _Linea
                Exit For
            End If
            If _Linea.ID_Propuesta_Linea = _IDATrobar Then
                _Trobat = True
            End If
        Next
        If _LineaSeguent Is Nothing = False Then
            Dim _TabActual As String = Me.Tab_Principal.SelectedTab.Key
            Call Netejar_Pantalla(True)
            oLinqPropuesta_Linea = New Propuesta_Linea
            ' Call HabilitarCampsImportats(True)
            Me.T_Unidades.Enabled = True
            Me.TE_Producto_Codigo.ReadOnly = False
            '  Call CarregarDadesPredeterminadesDelDocument()
            Call Cargar_Form(_LineaSeguent.ID_Propuesta_Linea, True)
            Call CarregarDadesPestanyes(_TabActual)
            Call ActivarPantalla(True)
        End If
        Util.WaitFormTancar()
    End Sub
#End Region

#Region "Grid Accesos"

    Private Sub CargaGrid_Accesos(ByVal pId As Integer)
        Try
            Dim _Accesos As IEnumerable(Of Propuesta_Linea_Acceso) = From taula In oDTC.Propuesta_Linea_Acceso Where taula.ID_Propuesta_Linea = pId Select taula

            With Me.GRD_Acceso

                '.GRID.DataSource = _Accesos
                .M.clsUltraGrid.CargarIEnumerable(_Accesos)

                .M_Editable()

                Call CargarCombo_TipoAcceso(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TipoAcceso(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Propuesta_Linea_TipoAcceso) = (From Taula In oDTC.Propuesta_Linea_TipoAcceso Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Propuesta_Linea_TipoAcceso

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Propuesta_Linea_TipoAcceso").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Propuesta_Linea_TipoAcceso").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Acceso_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Acceso.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Acceso

                If oLinqPropuesta_Linea.ID_Propuesta_Linea = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento").Value = IDEmplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Propuesta_Linea_TipoAcceso").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta_Linea").Value = oLinqPropuesta_Linea

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Acceso_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Acceso.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                ' oLinqPropuesta_Linea.Propuesta_Linea_Acceso.Remove(e.ListObject)
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

    Private Sub GRD_Acesso_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Acceso.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Precios Anteriores"

    Private Sub CargaGrid_PreciosAnteriores(ByVal pId As Integer)
        Try
            Dim _Listado As IEnumerable = From taula In oDTC.Propuesta_Linea Where taula.ID_Producto = pId And taula.Propuesta.SeInstalo = False And taula.Propuesta.Instalacion.Cliente.ID_Cliente = oLinqInstalacion.ID_Cliente And taula.Activo = True And taula.Propuesta.Activo = True And taula.ID_Propuesta <> oLinqPropuesta.ID_Propuesta And (taula.Propuesta.Propuesta_Seguridad.Count = 0 Or taula.Propuesta.Propuesta_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count > 0) Order By taula.Propuesta.Fecha Descending Select taula.ID_Propuesta_Linea, taula.Propuesta.Instalacion.ID_Instalacion, taula.Propuesta.Codigo, taula.Propuesta.Fecha, taula.Descripcion, Emplazamiento = taula.Instalacion_Emplazamiento.Descripcion, Planta = taula.Instalacion_Emplazamiento_Planta.Descripcion, Zona = taula.Instalacion_Emplazamiento_Zona.Descripcion, ElementosAProteger = taula.Instalacion_ElementosAProteger.Instalacion_ElementosAProteger_Tipo.Descripcion, Abertura = taula.Instalacion_Emplazamiento_Abertura.Descripcion_Detallada, taula.Unidad, taula.Precio, taula.Descuento, TotalUnitario = taula.Precio - ((taula.Precio * taula.Descuento) / 100)

            With Me.GRD_PreciosAnteriores
                '.GRID.DataSource = _Listado
                .M.clsUltraGrid.CargarIEnumerable(_Listado)

                '.GRID.DisplayLayout.Bands(0).Columns("ID_").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect

                'Dim pCol As Infragistics.Win.UltraWinGrid.UltraGridColumn
                'For Each pCol In .GRID.DisplayLayout.Bands(0).Columns
                '    pCol.PerformAutoResize()
                'Next

                If .GRID.Rows.Count > 0 Then
                    Util.TabPintarHeaderTab(Me.Tab_Principal.Tabs("PreciosAnteriores"), Color.Green)
                Else
                    Util.TabPintarHeaderTab(Me.Tab_Principal.Tabs("PreciosAnteriores"), Me.Tab_Principal.Tabs(0).Appearance.BackColor)
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Productos"

    Private Sub CargaGrid_Productos()
        Try

            Dim DT As New DataTable
            DT.Columns.Add("ID_Producto", "System.Integer".GetType)
            DT.Columns.Add("Cantidad", "System.Decimal".GetType)
            If oLinqPropuesta.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior AndAlso oLinqPropuesta.SeInstalo <> True Then
                DT.Columns.Add("PVP", "System.Decimal".GetType)
                DT.Columns.Add("PVD", "System.Decimal".GetType)
                DT.Columns.Add("Descuento", "System.Decimal".GetType)
                DT.Columns.Add("IVA", "System.Decimal".GetType)
                ' DT.Columns.Add("ID_Producto", "System.Integer".GetType)
            End If


            With Me.GRD_Productos
                .M.clsUltraGrid.Cargar(DT)
                '.GRID.DataSource=DT
                '.GRID.DisplayLayout.Bands(0).Columns("ID_").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
                .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.EditAndSelectText

            End With
            Call CarregarComboProductes()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Productos_M_GRID_AfterRowUpdate(sender As Object, e As RowEventArgs) Handles GRD_Productos.M_GRID_AfterRowUpdate
        If e.Row.Cells("ID_Producto").Value Is Nothing OrElse IsDBNull(e.Row.Cells("ID_Producto").Value) = True Then
            e.Row.Delete(False)
            Exit Sub
        End If
        If IsNumeric(e.Row.Cells("ID_Producto").Value) = False Then
            e.Row.Delete(False)
        End If

        'Fem aquest if pq si el value i el text es el mateix vol dir que no han seleccionat cap item 
        If e.Row.Cells("ID_Producto").Value = e.Row.Cells("ID_Producto").Text Then
            e.Row.Delete(False)
        End If

    End Sub

    Private Sub GRD_Productos_M_ToolGrid_ToolAfegir(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Productos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Productos
                .GRID.DisplayLayout.Bands(0).AddNew() 'Afegim una Row al Grid

                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento").Value = IDEmplazamiento
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Cantidad").Value = 1
                If oLinqPropuesta.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior AndAlso oLinqPropuesta.SeInstalo <> True Then
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("Descuento").Value = 0
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("PvP").Value = 0
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("PvD").Value = 0
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("IVA").Value = oDTC.Configuracion.FirstOrDefault.IVA
                End If
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Productos_M_ToolGrid_ToolEliminar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Productos.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_Productos.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            Me.GRD_Productos.GRID.Selected.Rows(0).Delete(False)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Productos_M_Grid_InitializeLayout(Sender As Object, e As InitializeLayoutEventArgs) Handles GRD_Productos.M_Grid_InitializeLayout
        e.Layout.Bands(0).Columns("ID_Producto").EditorComponent = Me.C_Producto
        e.Layout.Bands(0).Columns("ID_Producto").AutoCompleteMode = AutoCompleteMode.SuggestAppend
        e.Layout.Bands(0).Columns("ID_Producto").AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
    End Sub

    Private Sub CarregarComboProductes()
        Try
            Dim DT As DataTable = BD.RetornaDataTable("SELECT ID_Producto, Codigo, Descripcion From Producto Where Activo=1 Order by Descripcion")
            Me.C_Producto.DataSource = DT
            If DT Is Nothing Then
                Exit Sub
            End If

            With C_Producto
                .AutoCompleteMode = AutoCompleteMode.Suggest
                .AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
                .MaxDropDownItems = 16
                .DisplayMember = "Descripcion"
                .ValueMember = "ID_Producto"
                .DisplayLayout.Bands(0).Columns("ID_Producto").Hidden = True
                .DisplayLayout.Bands(0).Columns("Codigo").Width = 100
                .DisplayLayout.Bands(0).Columns("Codigo").Header.Caption = "Código"
                .DisplayLayout.Bands(0).Columns("Descripcion").Width = Me.GRD_Productos.GRID.DisplayLayout.Bands(0).Columns("ID_Producto").Width - 100
                .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub C_Producto_RowSelected(sender As Object, e As RowSelectedEventArgs) Handles C_Producto.RowSelected
        Try
            If oLinqPropuesta.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior AndAlso oLinqPropuesta.SeInstalo <> True Then
                Dim IDProducto As Integer = Me.C_Producto.Value
                Dim _PVP As Decimal
                Dim _PVD As Decimal
                Call RetornaPVPiPVD(IDProducto, _PVP, _PVD)
                Me.GRD_Productos.GRID.ActiveRow.Cells("PVP").Value = _PVP
                Me.GRD_Productos.GRID.ActiveRow.Cells("PVD").Value = _PVD
                Me.GRD_Productos.GRID.ActiveRow.Cells("Descuento").Value = 0
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Usuarios del Sistema"

    Private Sub CargaGrid_UsuariosSistema(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Propuesta_Linea_UsuarioSistema) = From taula In oDTC.Propuesta_Linea_UsuarioSistema Where taula.ID_Propuesta_Linea = pId Select taula

            With Me.GRD_UsuarioSistema

                .M.clsUltraGrid.CargarIEnumerable(_Dades)
                '.GRID.DataSource = _Dades

                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_UsuarioSistema_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_UsuarioSistema.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_UsuarioSistema

                If oLinqPropuesta_Linea.ID_Propuesta_Linea = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta_Linea").Value = oLinqPropuesta_Linea

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_UsuarioSistema_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_UsuarioSistema.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                ' oLinqPropuesta_Linea.Propuesta_Linea_Acceso.Remove(e.ListObject)
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

    Private Sub GRD_UsuarioSistema_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_UsuarioSistema.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Planificación mantenimiento"
    Private Sub B_Calcular_Mantenimientos_Click(sender As Object, e As EventArgs) Handles B_Calcular_Mantenimientos.Click

        If oLinqPropuesta_Linea.ID_Propuesta_Linea = 0 Then
            If Guardar() = False Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If
        End If


        If Me.DT_FechaFabricacion.Value Is Nothing Then
            Mensaje.Mostrar_Mensaje("Primero se debe introducir la fecha de fabricación", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If




        If oLinqPropuesta_Linea.Propuesta_Linea_Mantenimiento.Count > 0 Then
            If Mensaje.Mostrar_Mensaje("Ya hay registros introducidos, ¿desea eliminarlos?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                Exit Sub
            Else
                oDTC.Propuesta_Linea_Mantenimiento.DeleteAllOnSubmit(oLinqPropuesta_Linea.Propuesta_Linea_Mantenimiento)
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

        End If

        Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault

        If _Producto.VidaUtil.HasValue = True Then
            Me.DT_FechaFinalVidaUtil.Value = CDate(Me.DT_FechaFabricacion.Value).AddYears(_Producto.VidaUtil)
        End If

        Dim _Mantenimiento As Producto_Producto_Mantenimiento

        For Each _Mantenimiento In _Producto.Producto_Producto_Mantenimiento
            Dim _Plazo As New Propuesta_Linea_Mantenimiento
            _Plazo.Fecha = CDate(Me.DT_FechaFabricacion.Value).AddDays(_Mantenimiento.Tiempo)
            _Plazo.Realizado = False
            _Plazo.Producto_Producto_Mantenimiento = _Mantenimiento
            _Plazo.Parte_Revision_Estado = oDTC.Parte_Revision_Estado.Where(Function(F) F.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.PendienteRevisar)).FirstOrDefault
            oLinqPropuesta_Linea.Propuesta_Linea_Mantenimiento.Add(_Plazo)
        Next

        oLinqPropuesta_Linea.FechaFabricacion = Me.DT_FechaFabricacion.Value
        oLinqPropuesta_Linea.FechaFinalVidaUtil = Me.DT_FechaFinalVidaUtil.Value

        oDTC.SubmitChanges()

        Call CargaGrid_MantenimientosPlanificados(oLinqPropuesta_Linea.ID_Propuesta_Linea)
    End Sub

    Private Sub CargaGrid_MantenimientosPlanificados(ByVal pId As Integer)
        Try
            Dim _Dades As IEnumerable(Of Propuesta_Linea_Mantenimiento) = From taula In oDTC.Propuesta_Linea_Mantenimiento Where taula.ID_Propuesta_Linea = pId Select taula

            With Me.GRD_MantenimientosPlanificados

                .M.clsUltraGrid.CargarIEnumerable(_Dades)
                '.GRID.DataSource = _Dades
                Call CargarCombo_Mantenimiento(Me.GRD_MantenimientosPlanificados.GRID, Me.TE_Producto_Codigo.Tag)

                .GRID.DisplayLayout.Bands(0).Columns("Fecha").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Fecha").CellClickAction = CellClickAction.CellSelect

                .GRID.DisplayLayout.Bands(0).Columns("Producto_Producto_Mantenimiento").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Producto_Producto_Mantenimiento").CellClickAction = CellClickAction.CellSelect

                .GRID.DisplayLayout.Bands(0).Columns("Detalle").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Detalle").CellClickAction = CellClickAction.CellSelect

                .GRID.DisplayLayout.Bands(0).Columns("Parte_Revision_Estado").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Parte_Revision_Estado").CellClickAction = CellClickAction.CellSelect

                .M_Editable()

                Call CargarCombo_MantenimientoEstado(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Mantenimiento(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pIDProducto As Integer)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Producto_Producto_Mantenimiento) = (From Taula In oDTC.Producto_Producto_Mantenimiento Where Taula.ID_Producto = pIDProducto Order By Taula.Tiempo Select Taula)
            Dim Var As Producto_Producto_Mantenimiento

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Producto_Mantenimiento.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Producto_Producto_Mantenimiento").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Producto_Producto_Mantenimiento").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_MantenimientosPlanificados_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_MantenimientosPlanificados.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_MantenimientoEstado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Parte_Revision_Estado) = (From Taula In oDTC.Parte_Revision_Estado Order By Taula.Descripcion Select Taula)

            Dim Var As Parte_Revision_Estado
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Parte_Revision_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Parte_Revision_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Grid Informatica"

    Private Sub CargaGrid_Informatica(ByVal pId As Integer)
        Try
            Dim _Informatica As IEnumerable(Of Propuesta_Linea_Informatica) = From taula In oDTC.Propuesta_Linea_Informatica Where taula.ID_Propuesta_Linea = pId Select taula

            With Me.GRD_Informatica

                '.GRID.DataSource = _Accesos
                .M.clsUltraGrid.CargarIEnumerable(_Informatica)

                .M_Editable()

                Call CargarCombo_TipoDato(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TipoDato(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of TipoDato) = (From Taula In oDTC.TipoDato Order By Taula.Descripcion Select Taula)
            Dim Var As TipoDato

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("TipoDato").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("TipoDato").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Informatica_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Informatica.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Informatica

                If oLinqPropuesta_Linea.ID_Propuesta_Linea = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento").Value = IDEmplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Propuesta_Linea_TipoAcceso").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta_Linea").Value = oLinqPropuesta_Linea

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Informatica_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Informatica.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                ' oLinqPropuesta_Linea.Propuesta_Linea_Acceso.Remove(e.ListObject)
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

    Private Sub GRD_Informatica_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Informatica.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Software"

    Private Sub CargaGrid_Software(ByVal pId As Integer)
        Try
            Dim _Software As IEnumerable(Of Propuesta_Linea_Software) = From taula In oDTC.Propuesta_Linea_Software Where taula.ID_Propuesta_Linea = pId Select taula

            With Me.GRD_Software

                '.GRID.DataSource = _Accesos
                .M.clsUltraGrid.CargarIEnumerable(_Software)

                .M_Editable()

                Call CargarCombo_Software(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Software(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Software) = (From Taula In oDTC.Software Order By Taula.Descripcion Select Taula)
            Dim Var As Software

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Software").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Software").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Software_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Software.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Software

                If oLinqPropuesta_Linea.ID_Propuesta_Linea = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Instalacion_Emplazamiento").Value = IDEmplazamiento
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("ID_Propuesta_Linea_TipoAcceso").Value = 0
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Propuesta_Linea").Value = oLinqPropuesta_Linea

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Software_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Software.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                ' oLinqPropuesta_Linea.Propuesta_Linea_Acceso.Remove(e.ListObject)
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

    Private Sub GRD_Software_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Software.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Precios Compras"

    Private Sub CargaGrid_PreciosCompras(ByVal pId As Integer)
        Try

            ' Dim _Listado As IEnumerable = From taula In oDTC.Propuesta_Linea Where taula.ID_Producto = pId And taula.Propuesta.SeInstalo = False And taula.Propuesta.Instalacion.Cliente.ID_Cliente = oLinqInstalacion.ID_Cliente And taula.Activo = True And taula.Propuesta.Activo = True And taula.ID_Propuesta <> oLinqPropuesta.ID_Propuesta And (taula.Propuesta.Propuesta_Seguridad.Count = 0 Or taula.Propuesta.Propuesta_Seguridad.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).Count > 0) Order By taula.Propuesta.Fecha Descending Select taula.ID_Propuesta_Linea, taula.Propuesta.Instalacion.ID_Instalacion, taula.Propuesta.Codigo, taula.Propuesta.Fecha, taula.Descripcion, Emplazamiento = taula.Instalacion_Emplazamiento.Descripcion, Planta = taula.Instalacion_Emplazamiento_Planta.Descripcion, Zona = taula.Instalacion_Emplazamiento_Zona.Descripcion, ElementosAProteger = taula.Instalacion_ElementosAProteger.Instalacion_ElementosAProteger_Tipo.Descripcion, Abertura = taula.Instalacion_Emplazamiento_Abertura.Descripcion_Detallada, taula.Unidad, taula.Precio, taula.Descuento, TotalUnitario = taula.Precio - ((taula.Precio * taula.Descuento) / 100)
            ' _Listado = From Taula In oDTC.Entrada_Linea Where Taula.ID_Producto = pId And Taula.NoRestarStock = False And Taula.Entrada.ID_Entrada_Tipo = CInt(EnumEntradaTipo.AlbaranCompra) Order By Taula.FechaEntrada Descending Select Taula.ID_Entrada, Taula.Entrada.Codigo, Taula.Entrada.FechaEntrada, Taula.Entrada.Proveedor.Nombre, Taula.Unidad, Taula.Precio, Taula.Descuento1, Taula.Descuento2, Taula.IVA, Taula.TotalIVA, Taula.TotalBase, Taula.TotalLinea


            With Me.GRD_PreciosCompras
                ' .M.clsUltraGrid.CargarIEnumerable(_Listado)
                Dim _DT As DataTable = BD.RetornaDataTable("Select * From c_Entrada_Linea Where NoRestarStock=0 and id_producto=" & pId & " and id_entrada_tipo=" & CInt(EnumEntradaTipo.AlbaranCompra) & " Order by FechaEntrada")
                '.M.clsUltraGrid.Cargar("Select * From c_Entrada_Linea Where NoRestarStock=0 and id_producto=" & pId & " and id_entrada_tipo=" & CInt(EnumEntradaTipo.AlbaranCompra) & " Order by FechaEntrada", BD)
                .M.clsUltraGrid.Cargar(_DT)

                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region


End Class