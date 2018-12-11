Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEntrada_Linea
    Dim oDTC As DTCDataContext
    Dim oclsEntradaLinea As clsEntradaLinea
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2
    Dim noTocarLinea As Entrada_Linea
    Dim noTocarEntrada As Entrada

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If Guardar() = True Then
            Me.FormTancar()
            'Call M_ToolForm1_m_ToolForm_Sortir()
        End If
    End Sub

    'Private Sub ToolForm_m_ToolForm_GuardarIMantener() Handles ToolForm.m_ToolForm_Buscar
    '    If Guardar() = True Then
    '        'Dim ID_Producte As Integer = oLinqPropuesta_Linea.ID_Producto
    '        'Dim Codi As String = oLinqPropuesta_Linea.Producto.Codigo
    '        'Dim Descripcio As String = oLinqPropuesta_Linea.Descripcion
    '        'Dim Uso As String = oLinqPropuesta_Linea.Uso
    '        'Dim Descuento As Decimal = oLinqPropuesta_Linea.Descuento


    '        'Call Netejar_Pantalla()
    '        oLinqPropuesta_Linea = New Propuesta_Linea
    '        Call ActivarPantalla(True)
    '        Me.TE_Producto_Codigo.ReadOnly = False
    '        Call CargarIdentificadorDeLinea()
    '        'Call CargarCombo_Vinculacions(oLinqPropuesta.ID_Propuesta)

    '        Me.T_NumSerie.Text = ""
    '        Me.T_NumZona.Text = ""
    '        Me.GRD_Acceso.GRID.DataSource = Nothing
    '        ' Me.T_Identificador.Text = ""

    '        ''Me.TE_Producto_Codigo.ReadOnly = False
    '        'Me.TE_Producto_Codigo.Tag = ID_Producte
    '        'Me.TE_Producto_Codigo.Text = Codi
    '        'Me.T_Producto_Descripcion.Text = Descripcio
    '        'Me.T_Uso.Text = Uso
    '        'Me.T_Descuento.Value = Descuento
    '        'Call CargaPreuProductoYTraspasable(ID_Producte)
    '    End If
    'End Sub

    'Private Sub ToolForm_m_ToolForm_GuardarIContinuar() Handles ToolForm.m_ToolForm_Imprimir
    '    If Guardar() = True Then

    '        Call Netejar_Pantalla()
    '        Call ActivarPantalla(False)
    '        Me.TE_Producto_Codigo.ReadOnly = False
    '        Call CargarIdentificadorDeLinea()
    '        Call CargarCombo_Vinculacions(oLinqPropuesta.ID_Propuesta)
    '    End If
    'End Sub

    'Private Sub ToolForm_m_ToolForm_Seleccionar() Handles ToolForm.m_ToolForm_Seleccionar
    '    Try

    '        If Me.T_Unidades.Value <> 1 Then
    '            Mensaje.Mostrar_Mensaje("Información: La actual línea tiene una cantidad diferente a 1. La duplicación creará los nuevos registros con cantidad 1", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '            Exit Sub
    '        End If

    '        Dim quantitat As String
    '        quantitat = Mensaje.Mostrar_Entrada_Datos("Introduzca la cantidad de veces que desea duplicar la línea de propuesta. (Ejemplo: si introduce un 2 se copiará la linea 2 veces, aparte, tendrá la linea actual)", "1", False)
    '        If IsNumeric(quantitat) = False Then
    '            Mensaje.Mostrar_Mensaje("Los datos introducidos no son correctos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '            Exit Sub
    '        End If

    '        If Guardar() = False Then
    '            Exit Sub
    '        End If


    '        Dim i As Integer = 0

    '        Util.WaitFormObrir()
    '        Dim IdentificadorLinea As Integer
    '        IdentificadorLinea = clsPropuestaLinea.RetornaUltimIdentificadorDeLinea(oDTC, oLinqPropuesta.ID_Propuesta)

    '        For i = 1 To CInt(quantitat)
    '            Dim _LineaNova As New Propuesta_Linea
    '            _LineaNova = clsPropuestaLinea.RetornaDuplicacioInstancia(oLinqPropuesta_Linea)
    '            _LineaNova.Unidad = 1
    '            _LineaNova.Identificador = IdentificadorLinea

    '            IdentificadorLinea = CrearFills(_LineaNova, IdentificadorLinea)

    '            oDTC.Propuesta_Linea.InsertOnSubmit(_LineaNova)

    '            IdentificadorLinea = IdentificadorLinea + 1
    '        Next
    '        oDTC.SubmitChanges()

    '        Util.WaitFormTancar()
    '        Mensaje.Mostrar_Mensaje("Registro duplicado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
    '        Call M_ToolForm1_m_ToolForm_Sortir()
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub
    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        ''No deixarem sortir si la pantalla te una quantitat de 0
        'If oclsEntradaLinea.oLinqLinea.Unidad = 0 And oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea <> 0 Then
        '    Mensaje.Mostrar_Mensaje("Imposible crear la línea con cantidad 0", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
        '    Exit Sub
        'End If

        'Si ets una línea de traspaso entre almacenes i la línea ja s'ha guardat i apretes el botó sortir eliminarem la línea pq si no no es crea la línea "germana" i no t'he sentit
        If oclsEntradaLinea.oLinqEntrada.ID_Entrada_Tipo = EnumEntradaTipo.TraspasoAlmacen AndAlso oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea <> 0 Then
            If Mensaje.Mostrar_Mensaje("Al salir se eliminarà la línea de traspaso, ¿està seguro que desea salir?", M_Mensaje.Missatge_Modo.PREGUNTA, , , True) = M_Mensaje.Botons.SI Then

                If oclsEntradaLinea.EliminarLinea() = False Then
                    Mensaje.Mostrar_Mensaje("No se ha podido eliminar la línea", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                    ' End If
                Else
                    oDTC.SubmitChanges()
                End If
            Else
                Exit Sub
            End If

        End If
        Me.FormTancar()

    End Sub

    Private Sub ToolForm_m_ToolForm_GuardarIMantener() Handles ToolForm.m_ToolForm_Buscar
        If Guardar() = True Then
            If oclsEntradaLinea.pRequiereNS = True Then
                Me.T_Unidades.Value = 0
            End If

            oclsEntradaLinea = New clsEntradaLinea(noTocarEntrada, Nothing, oDTC)
            Call CargarGrid_Partes()
            Call CargarGrid_TalYComoSeInstalo()
            Call CargaGrid_NS(0)
            'Call CargaGrid_PreciosAnteriores(0)
            'Call CargaGrid_PreciosAnteriores_propuesta(0)
            Call CargaGrid_CompuestoPor()



            'oLinqPropuesta_Linea = New Propuesta_Linea
            'Call ActivarPantalla(True)
            'Me.TE_Producto_Codigo.ReadOnly = False
            'Call CargarIdentificadorDeLinea()
            'Me.T_NumSerie.Text = ""
            'Me.T_NumZona.Text = ""
            'Me.GRD_Acceso.GRID.DataSource = Nothing
        End If
    End Sub

    Private Sub ToolForm_m_ToolForm_GuardarIContinuar() Handles ToolForm.m_ToolForm_Imprimir
        If Guardar() = True Then
            Call Netejar_Pantalla()
            Call HabilitarCampsImportats(True)
            Me.T_Unidades.Enabled = True
            Me.TE_Producto_Codigo.ReadOnly = False
            Call CarregarDadesPredeterminadesDelDocument()
            'Call ActivarPantalla(False)

            'Util.Tab_InVisible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "NumeroSerie")
            ' Call CargaGrid_NS(0)
            'Call CargarIdentificadorDeLinea()
            'Call CargarCombo_Vinculacions(oLinqPropuesta.ID_Propuesta)
            'Call VisualitzacioPantalla()
        End If
    End Sub

    Private Sub ToolForm_m_ToolForm_ToolClick_Botones_Extras(Sender As Object, e As UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        Select Case e.Tool.Key
            Case "Trazabilidad"

                If Me.TE_Producto_Codigo.Tag Is Nothing Then
                    Exit Sub
                End If

                Dim frm As New frmProducto_Trazabilidad
                frm.Entrada(Me.TE_Producto_Codigo.Tag)
                frm.FormObrir(Me, True)

        End Select
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pEntrada As Entrada, ByRef pDTC As DTCDataContext, Optional ByRef pLinea As Entrada_Linea = Nothing)
        Try

            Me.AplicarDisseny()

            oDTC = pDTC
            noTocarEntrada = pEntrada
            noTocarLinea = pLinea

            'oclsEntradaLinea = New clsEntradaLinea(pEntrada, pLinea, oDTC)

            oclsEntradaLinea = New clsEntradaLinea(noTocarEntrada, noTocarLinea, oDTC)
            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Entrada_Linea_Archivo", 1)

            Me.GRD_CompuestoPor.M.clsToolBar.Boto_Afegir("VerTrazabilidad", "Ver trazabilidad del producto", True)
            Me.GRD_CompuestoPor.M.clsToolBar.Boto_Afegir("VerProducto", "Ver ficha del producto", True)
            Me.GRD_CompuestoPor.M.clsToolBar.Boto_Afegir("VerAlmacen", "Ver almacén", True)
            Me.GRD_CompuestoPor.M.clsToolBar.Boto_Afegir("VisualizarFotos", "Visualizar fotos", True)

            Me.ToolForm.M.Botons.tAccions.SharedProps.Visible = True
            Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("Trazabilidad", "Trazabilidad del producto seleccionado", True)

            Me.GRD_MaterialEnAlmacenesDeLosPartesAsignados.M.clsToolBar.Boto_Afegir("VerProducto", "Ver ficha del producto", True)
            Me.GRD_MaterialEnAlmacenesDeLosPartesAsignados.M.clsToolBar.Boto_Afegir("VerTrazabilidad", "Ver trazabilidad del producto", True)

            ' oLinqEntrada = pEntrada
            ' oLinqEntrada_Linea = pLinea
            'oEntradaTipo = pEntrada.ID_Entrada_Tipo


            'Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True
            'Me.ToolForm.M.Botons.tImprimir.SharedProps.Caption = "Guardar y continuar"
            'Me.ToolForm.M.Botons.tBuscar.SharedProps.Visible = True
            'Me.ToolForm.M.Botons.tBuscar.SharedProps.Caption = "Guardar y mantener"
            'Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Visible = True
            'Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Caption = "Duplicar"

            'rrrr
            'If oclsEntradaLinea.oEntradaTipo <> EnumEntradaTipo.TraspasoAlmacen And oclsEntradaLinea.oEntradaTipo <> EnumEntradaTipo.Regularizacion Then
            '    Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True
            '    Me.ToolForm.M.Botons.tImprimir.SharedProps.Caption = "Guardar y continuar"
            '    Me.ToolForm.M.Botons.tBuscar.SharedProps.Visible = True
            '    Me.ToolForm.M.Botons.tBuscar.SharedProps.Caption = "Guardar y mantener"
            'End If


            Me.GRD_NS.M.clsToolBar.Boto_Afegir("SeleccionarNS", "Seleccionar números de serie", False)

            Call CargarCombo_Instalacions()
            Util.Cargar_Combo(Me.C_Almacen, "SELECT ID_Almacen, Descripcion FROM Almacen Where Activo=1 ORDER BY Descripcion", False)
            Util.Cargar_Combo(Me.C_Garantia, "SELECT ID_Producto_Garantia, Tiempo FROM Producto_Garantia Where Activo=1 ORDER BY Tiempo", False)

            Me.GRD_TalYComoSeInstalo.M.clsToolBar.Boto_Afegir("Asignar", "Asignar", True)
            Me.GRD_TalYComoSeInstalo.M.clsToolBar.Boto_Afegir("IrALaInstalacion", "Ir a la Instalación", True)

            Me.GRD_Parte_Gastos.M.clsToolBar.Boto_Afegir("Asignar", "Asignar", True)
            Me.GRD_Parte_Horas.M.clsToolBar.Boto_Afegir("Asignar", "Asignar", True)
            Me.GRD_Parte_Material.M.clsToolBar.Boto_Afegir("Asignar", "Asignar", True)

            Me.GRD_Parte_Gastos.M.clsToolBar.Boto_Afegir("IrAlParte", "Ir al parte", True)
            Me.GRD_Parte_Horas.M.clsToolBar.Boto_Afegir("IrAlParte", "Ir al parte", True)
            Me.GRD_Parte_Material.M.clsToolBar.Boto_Afegir("IrAlParte", "Ir al parte", True)

            Me.GRD_HorasAsignadasALosPartes.M.clsToolBar.Boto_Afegir("IrAlParte", "Ir al parte", True)

            Dim BotoCancelar As UltraWinEditors.EditorButton
            BotoCancelar = New UltraWinEditors.EditorButton
            BotoCancelar.Key = "Cancelar"
            Dim oDisseny As New M_Disseny.ClsDisseny
            BotoCancelar.Appearance.Image = oDisseny.Leer_Imagen("text_cancelar.gif")
            BotoCancelar.Width = 16
            BotoCancelar.Appearance.BackColor = Color.White
            BotoCancelar.Appearance.BorderAlpha = Alpha.Transparent


            Me.C_Abertura.ButtonsRight.Add(BotoCancelar.Clone)
            Me.C_ElementosAProteger.ButtonsRight.Add(BotoCancelar.Clone)
            Me.C_Planta.ButtonsRight.Add(BotoCancelar.Clone)

            Me.GRD_Ficheros.M.clsToolBar.Boto_Afegir("FotoPredeterminada", "Predeterminar foto", True)
            Me.GRD_Ficheros.M.clsToolBar.Boto_Afegir("QuitarFotoPredeterminada", "Quitar foto predeterminada", True)
            AddHandler Fichero.DespresDeCarregarDades, AddressOf DespresDeCarregarDadesGridArchivos
            AddHandler Fichero.DespresDeEliminarRegistre, AddressOf DespresDeEliminarElRegistreArchivos
            'Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "SeInstalo", "DatosAcceso", "PreciosAnteriores")
            'Call CargarCombo_Vinculacions(pId)


            If pLinea Is Nothing = False Then
                Call Cargar_Form(pLinea)
                ' Call ActivarPantalla(True)
            Else
                Call Netejar_Pantalla()
                'Si la linea es nova heredarem el magatzem del document pare
                Call CarregarDadesPredeterminadesDelDocument()

                'oLinqEntrada_Linea = New Entrada_Linea
                ' Call ActivarPantalla(False)
                '  Call CargarIdentificadorDeLinea()
            End If

            'Util.Tab_Desactivar_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "NumeroSerie")
            Util.Tab_InVisible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "Regularizacion", "CompuestoPor", "NumeroSerie", "ParteTrabajoVenta", "TalYComoSeInstalo")
            'Me.GRD_NS.ToolGrid.Tools("SeleccionarNS").SharedProps.Visible = False
            Me.Frm_Garantia.Visible = False
            Me.Frm_Ubicacion.Visible = False
            Select Case oclsEntradaLinea.oEntradaTipo
                Case EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.DevolucionVenta, EnumEntradaTipo.FacturaVentaRectificativa

                Case Else
                    Me.GRD_PreciosCompras.Height = Me.GRD_PreciosCompras.Height + (Me.GRD_PreciosCompras.Top - Me.Frm_Ubicacion.Top - 10)
                    Me.GRD_PreciosCompras.Top = Me.Frm_Ubicacion.Top
            End Select



            Me.Frm_Vinculaciones.Visible = False
            Me.CH_NoRestarStock.Visible = False

            Call VisualitzacioPantalla()

            Util.Tab_Seleccio_x_Key(Me.TAB_General, "Precios")

            Me.Text = clsEntradaLinea.RetornaDescripcioDocumentEnFuncioDelSeuTipus(oclsEntradaLinea.oEntradaTipo, pEntrada.Codigo)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    'Private Sub CargarCombo_Vinculacions(ByVal pID As Integer)

    '    Dim SQLText As String = ""
    '    If pID <> 0 Then
    '        SQLText = " and ID_Propuesta_Linea<>" & pID
    '    End If
    '    'Util.Cargar_Combo(Me.C_Vinculacion, "SELECT ID_Propuesta_Linea, cast(Identificador as nvarchar(20)) + ' - ' + Propuesta_Linea.Descripcion FROM Propuesta_Linea, Producto WHERE Producto.ID_Producto=Propuesta_Linea.ID_Producto and (Producto.Central=1 or Producto.Expansor=1 or Producto.Modulo_Rele=1) and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " ORDER BY Identificador", False)
    '    ' Util.Cargar_Combo(Me.C_Vinculacion, "SELECT ID_Propuesta_Linea, cast(Identificador as nvarchar(20)) + ' - ' + Propuesta_Linea.Descripcion FROM Propuesta_Linea, Producto WHERE Producto.ID_Producto=Propuesta_Linea.ID_Producto and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 ORDER BY Identificador", False)
    '    Dim DT As DataTable = BD.RetornaDataTable("SELECT  ID_Propuesta_Linea, Identificador, IdentificadorDelProducto,  Descripcion,Emplazamiento, Planta, Zona, NickZona  FROM C_Propuesta_Linea WHERE Activo=1 and  ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 and Conectable=1 ORDER BY IdentificadorDelProducto")
    '    Me.C_Vinculacion.DataSource = DT
    '    If DT Is Nothing Then
    '        Exit Sub
    '    End If
    '    Me.C_Vinculacion.MaxDropDownItems = 16
    '    Me.C_Vinculacion.DisplayMember = "Descripcion"
    '    Me.C_Vinculacion.ValueMember = "ID_Propuesta_Linea"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("ID_Propuesta_Linea").Hidden = True
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Header.Caption = "Identificador"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Header.Caption = "Linea"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Descripcion").Width = 400
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Emplazamiento").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Emplazamiento").Header.Caption = "Ubicación"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Planta").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Planta").Header.Caption = "Planta"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Zona").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Zona").Header.Caption = "Zona"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("NickZona").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("NickZona").Header.Caption = "Alias"
    '    'Me.C_Vinculacion.DropDownStyle = UltraComboStyle.DropDownList

    '    If oLinqPropuesta.SeInstalo = True Then
    '        'Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Hidden = False
    '        Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Hidden = True
    '    Else
    '        'Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Hidden = True
    '        Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Hidden = False
    '    End If

    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Override.AllowRowFiltering = DefaultableBoolean.True

    '    'Util.Cargar_Combo(Me.UltraCombo1, "SELECT ID_Propuesta_Linea, cast(Identificador as nvarchar(20)), Propuesta_Linea.Descripcion FROM Propuesta_Linea, Producto WHERE Producto.ID_Producto=Propuesta_Linea.ID_Producto and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 ORDER BY Identificador", False)
    'End Sub

    'Private Sub CargarIdentificadorDeLinea()
    '    If oLinqPropuesta.Propuesta_Linea.Count = 0 Then
    '        Me.T_Identificador.Value = 1
    '    Else
    '        Me.T_Identificador.Value = oLinqPropuesta.Propuesta_Linea.Max(Function(F) F.Identificador) + 1
    '    End If
    'End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.TE_Producto_Codigo.Focus()

            If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
                If Me.T_CantidadDeseada.Value > Me.T_StockReal.Value Then
                    Mensaje.Mostrar_Mensaje("Imposible traspasar más cantidad de la que hay en stock", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm()

            'Si el producte que volem guardar no és de números de série i te quantitat 0 ho impedirem.
            Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
            If Me.T_Unidades.Value = 0 And _Producto.RequiereNumeroSerie = False Then
                Mensaje.Mostrar_Mensaje("Imposible guardar, la cantidad no puede ser 0", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            Else
                'Si s'ha posat 0 i te número de serie i es una modificació tb ho impedirem
                If Me.T_Unidades.Value = 0 And _Producto.RequiereNumeroSerie = True And oclsEntradaLinea.EsUnaLineaNova = False Then
                    Mensaje.Mostrar_Mensaje("Imposible guardar, la cantidad no puede ser 0", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
            End If


            'Call CalcularNivellsLineaActual()

            If oclsEntradaLinea.EsUnaLineaNova Then
                If oclsEntradaLinea.AfegirLinea(True) = True Then
                    oDTC.SubmitChanges()
                    If oclsEntradaLinea.pRequiereNS = False Then 'Si requereix números de serie encara no duplicarem la línea ja que per introduir números de serie primer s'ha de guardar la línea per collons
                        If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
                            Dim _clsEntradaLinea As New clsEntradaLinea(oclsEntradaLinea.oLinqEntrada, , oDTC)
                            oclsEntradaLinea.TraspasoAlmacenesRetornaNovaLineaDeSortida(_clsEntradaLinea.oLinqLinea)
                            oDTC.SubmitChanges()
                        End If
                    End If

                    Call Fichero.Cargar_GRID(oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea) 'Fem això pq la classe tingui el ID de Entrada_Linea

                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                End If

            Else
                If oclsEntradaLinea.ModificaLinea Then
                    If oclsEntradaLinea.pRequiereNS = True Then 'Si requereix números de serie duplicarem la línea ja que per introduir números de serie primer s'ha de guardar la línea per collons
                        If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
                            Dim _clsEntradaLinea As New clsEntradaLinea(oclsEntradaLinea.oLinqEntrada, , oDTC)
                            oclsEntradaLinea.TraspasoAlmacenesRetornaNovaLineaDeSortida(_clsEntradaLinea.oLinqLinea)
                            oDTC.SubmitChanges()
                        End If
                    End If

                    oDTC.SubmitChanges()
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
                End If

            End If

            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oclsEntradaLinea.oLinqLinea
                Me.TE_Producto_Codigo.Tag = .ID_Producto
                Me.TE_Producto_Codigo.Value = .Producto.Codigo
                Me.T_Producto_Descripcion.Text = .Descripcion  '.Producto.Descripcion
                Me.T_Detalle.Value = .Uso

                Me.C_Garantia.Value = .ID_Producto_Garantia
                Me.DT_FinGarantia.Value = .FechaFinGarantia
                Me.T_Unidades.Value = .Unidad
                Me.T_Precio.Value = .Precio
                Me.T_Descuento1.Value = .Descuento1
                Me.T_Descuento2.Value = .Descuento2
                Me.T_IVA.Value = .IVA
                Me.T_Coste.Value = .Coste
                Me.C_Almacen.Value = .ID_Almacen
                Me.DT_PeriodoInicio.Value = .PeriodoInicio
                Me.DT_PeriodoFin.Value = .PeriodoFin
                Me.C_Instalacion.Value = .ID_Instalacion
                Me.C_Instalacion_Contrato.Value = .ID_Instalacion_Contrato
                Me.DT_Entrega.Value = .FechaEntrega
                Me.R_DescripcionAmpliada.pText = .DescripcionAmpliada
                Me.R_DescripcionAmpliada_Tecnica.pText = .DescripcionAmpliada_Tecnica
                Me.R_Observaciones.pText = .Observaciones

                Me.T_Amidamientos_Fase.Text = .AmidamientosFase
                Me.T_Amidamientos_ReferenciaMemoria.Text = .AmidamientosReferenciaEnObra
                Me.T_Referencia_Linea.Text = .ReferenciaLinea
                Me.T_Referencia_NombreObra.Text = .ReferenciaNombreObra
                Me.T_Referencia_Numero.Text = .ReferenciaNum
                Me.T_Referencia_Pedido.Text = .ReferenciaNumeroPedidoDeCompra
                Me.T_Referencia_Proyecto.Text = .ReferenciaProyecto

                Me.C_Emplazamiento.Value = .ID_Instalacion_Emplazamiento
                Me.C_Planta.Value = .ID_Instalacion_Emplazamiento_Planta
                Me.C_Zona.Value = .ID_Instalacion_Emplazamiento_Zona
                Me.C_ElementosAProteger.Value = .ID_Instalacion_ElementosAProteger
                Me.C_Abertura.Value = .ID_Instalacion_Emplazamiento_Abertura
                Me.CH_NoRestarStock.Checked = .NoRestarStock

                Me.CH_NoImprimir.Checked = .NoImprimirLinea

                Me.DT_FechaInicio.Value = .FechaInicio
                Me.DT_FechaFin.Value = .FechaFin

                Call CargarFoto(Nothing)

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm() 'ByRef pEntrada_Linea As Entrada_Linea
        Try
            With oclsEntradaLinea.oLinqLinea
                .Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.C_Almacen.Value)).FirstOrDefault

                If oclsEntradaLinea.EsUnaLineaNova Then 'Si la línea es nova guardarem la data de creació
                    .FechaEntrada = Now
                End If

                .Unidad = Me.T_Unidades.Value

                If .CantidadTraspasada.HasValue = False Then 'Si no te valor li posarem un 0 pq no petin coses...
                    .CantidadTraspasada = 0
                End If
                .Precio = Me.T_Precio.Value
                .IVA = Me.T_IVA.Value
                .Descuento1 = Util.Comprobar_NULL_Per_0_Decimal(Me.T_Descuento1.Value)
                .Descuento2 = Util.Comprobar_NULL_Per_0_Decimal(Me.T_Descuento2.Value)

                .Coste = Util.Comprobar_NULL_Per_0_Decimal(Me.T_Coste.Value)

                If IsNumeric(Me.TE_Producto_Codigo.Tag) = True Then
                    .Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault
                Else
                    .Producto = Nothing
                End If

                .Uso = Me.T_Detalle.Value
                .FechaEntrega = Me.DT_Entrega.Value

                If Me.C_Garantia.SelectedIndex <> -1 Then
                    .Producto_Garantia = oDTC.Producto_Garantia.Where(Function(F) F.ID_Producto_Garantia = CInt(Me.C_Garantia.Value)).FirstOrDefault
                Else
                    .Producto_Garantia = Nothing
                End If

                .Descripcion = Me.T_Producto_Descripcion.Text
                .FechaFinGarantia = Me.DT_FinGarantia.Value
                .PeriodoInicio = Me.DT_PeriodoInicio.Value
                .PeriodoFin = Me.DT_PeriodoFin.Value

                If Me.C_Instalacion.SelectedRow Is Nothing Then
                    .Instalacion = Nothing
                Else
                    .Instalacion = oDTC.Instalacion.Where(Function(F) F.ID_Instalacion = CInt(Me.C_Instalacion.Value)).FirstOrDefault
                End If

                If Me.C_Instalacion_Contrato.SelectedRow Is Nothing Then
                    .Instalacion_Contrato = Nothing
                Else
                    .Instalacion_Contrato = oDTC.Instalacion_Contrato.Where(Function(F) F.ID_Instalacion_Contrato = CInt(Me.C_Instalacion_Contrato.Value)).FirstOrDefault
                End If

                .AmidamientosFase = Me.T_Amidamientos_Fase.Text
                .AmidamientosReferenciaEnObra = Me.T_Amidamientos_ReferenciaMemoria.Text
                .ReferenciaLinea = Me.T_Referencia_Linea.Text
                .ReferenciaNombreObra = Me.T_Referencia_NombreObra.Text
                .ReferenciaNum = Me.T_Referencia_Numero.Text
                .ReferenciaNumeroPedidoDeCompra = Me.T_Referencia_Pedido.Text
                .ReferenciaProyecto = Me.T_Referencia_Proyecto.Text
                .DescripcionAmpliada = Me.R_DescripcionAmpliada.pText
                .DescripcionAmpliada_Tecnica = Me.R_DescripcionAmpliada_Tecnica.pText
                .Observaciones = Me.R_Observaciones.pText
                .NoRestarStock = Me.CH_NoRestarStock.Checked

                .NoImprimirLinea = Me.CH_NoImprimir.Checked

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

                If oclsEntradaLinea.oLinqEntrada.ID_Entrada_Tipo = EnumEntradaTipo.Regularizacion Then
                    .Unidad = Me.T_Diferencia.Value
                    .CantidadQueHabiaEnStock = CDbl(Me.T_StockReal.Value)
                End If

                If oclsEntradaLinea.oLinqEntrada.ID_Entrada_Tipo = EnumEntradaTipo.TraspasoAlmacen Then
                    .Unidad = Me.T_CantidadDeseada.Value * -1
                    '.CantidadQueHabiaEnStock = CDbl(Me.T_StockReal.Value)
                End If

                .FechaInicio = Me.DT_FechaInicio.Value
                .FechaFin = Me.DT_FechaFin.Value

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByRef pLinea As Entrada_Linea, Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        Try
            ' Call Netejar_Pantalla()
            'oLinqPropuesta_Linea = (From taula In oDTC.Propuesta_Linea Where taula.ID_Propuesta_Linea = pID Select taula).First

            Call SetToForm()

            Call CargarGrid_TalYComoSeInstalo()
            Fichero.Cargar_GRID(pLinea.ID_Entrada_Linea)
            Call CargaGrid_NS(pLinea.ID_Entrada_Linea)
            Call CalcularResum()
            Call CargaGrid_PreciosCompras(Me.TE_Producto_Codigo.Tag)
            ' Call CalcularStock()
            'Call CargaGrid_Accesos(pID)
            'Call CargaGrid_UsuariosSistema(pID)
            'Call CargaGrid_PreciosAnteriores(oLinqPropuesta_Linea.ID_Producto)

            'Call Carga_Grid_Caracteristicas_Personalizadas(pID)
            'Me.Text = clsEntradaLinea.RetornaDescripcioDocumentEnFuncioDelSeuTipus(oclsEntradaLinea.oEntradaTipo, pLinea.Entrada.Codigo)

            'Si estem editant una línea d'entrada no podrem canviar l'article

            Call VisualitzacioPantalla()


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarPreciosAnteriores()
        If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.PedidoVenta Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.AlbaranVenta Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.FacturaVenta And oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.PedidoCompra Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.AlbaranCompra Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.FacturaCompra Then
            Call CargaGrid_PreciosAnteriores(Me.TE_Producto_Codigo.Tag)
        End If
        If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.PedidoVenta Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.AlbaranVenta Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.FacturaVenta Then
            Call CargaGrid_PreciosAnteriores_propuesta(Me.TE_Producto_Codigo.Tag)
        End If
    End Sub

    Private Sub HabilitarCampsImportats(ByVal pHabilitar As Boolean)
        ' Me.T_Producto_Descripcion.Enabled = pHabilitar
        Me.TE_Producto_Codigo.Enabled = pHabilitar
        Me.TE_Producto_Codigo.ButtonsRight("Lupeta").Enabled = pHabilitar
        Me.C_Almacen.Enabled = True
    End Sub

    Private Sub Netejar_Pantalla(Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        oclsEntradaLinea = New clsEntradaLinea(noTocarEntrada, Nothing, oDTC)
        ' oDTC = New DTCDataContext(BD.Conexion)

        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        If pNoCanviarALaPestanyaGeneral = False Then
            Me.Tab_Principal.Tabs("General").Selected = True
        End If

        Select Case oclsEntradaLinea.oEntradaTipo
            Case EnumEntradaTipo.TraspasoAlmacen, EnumEntradaTipo.Regularizacion
                'Me.TAB_General.Tabs("Regularizacion").Selected = True
            Case Else
                Me.TAB_General.Tabs("Precios").Selected = True
        End Select

        'Me.TE_Producto_Codigo.ReadOnly = False
        'Me.TE_Producto_Codigo.Enabled = True
        'Me.TE_Producto_Codigo.ButtonsRight("Lupeta").Enabled = True
        'Me.T_Unidades.Enabled = True

        Me.L_Anotacion_ProvieneDeUnPedido.Visible = False
        Me.TE_Producto_Codigo.Value = Nothing
        Me.TE_Producto_Codigo.Tag = Nothing

        Me.TE_Producto_Codigo.Focus()

        Me.T_Unidades.Value = 1
        Me.C_Instalacion.Value = Nothing
        Me.C_Instalacion_Contrato.Value = Nothing

        Me.C_Abertura.SelectedIndex = -1
        Me.C_ElementosAProteger.SelectedIndex = -1

        'si hi han items, seleccionarem el primer
        If Me.C_Emplazamiento.Items.Count > 0 Then
            Me.C_Emplazamiento.SelectedIndex = 0
        End If
        'Me.C_Emplazamiento.SelectedIndex = -1
        Me.C_Planta.SelectedIndex = -1
        If Me.C_Almacen.Items.Count > 0 Then
            Me.C_Almacen.SelectedIndex = 0
        End If

        Me.DT_Entrega.Value = Now.Date

        Me.C_Zona.SelectedIndex = -1

        Me.C_Abertura.ReadOnly = True
        Me.C_ElementosAProteger.ReadOnly = True
        Me.C_Planta.ReadOnly = True
        Me.C_Zona.ReadOnly = True

        Me.T_IVA.Value = oDTC.Configuracion.FirstOrDefault.IVA
        Call CargarGrid_Partes()
        Call CargarGrid_TalYComoSeInstalo()
        Call CargaGrid_NS(0)
        Call CargaGrid_PreciosAnteriores(0)
        Call CargaGrid_PreciosAnteriores_propuesta(0)
        Call CargaGrid_CompuestoPor()
        Call CargaGrid_PreciosCompras(0)

        Fichero.Cargar_GRID(0)
        Me.Foto.Image = Nothing
        'Call CargaGrid_Accesos(0)
        'Call CargaGrid_UsuariosSistema(0)
        'Call CargaGrid_PreciosAnteriores(0)

        'AvisoCantidadSuperiorAUno = ""
        'AvisoVinculacionArticuloFamiliaNoTraspasar = ""
        Call VisualitzacioPantalla()

        ErrorProvider1.Clear()
        'Call CargaGrid_Productos()
        'Call CargaCombo_Proveidor(0)

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Producto_Codigo)
                oClsControls.ControlBuit(.T_Producto_Descripcion)
                oClsControls.ControlBuit(.T_Precio)
                oClsControls.ControlBuit(.T_Unidades)
                oClsControls.ControlBuit(.C_Almacen)
                oClsControls.ControlBuit(.DT_Entrega)
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
        Try

            If pID Is Nothing Then
            Else
                Dim _Producto As Producto
                _Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = pID).FirstOrDefault

                Me.TE_Producto_Codigo.Tag = _Producto.ID_Producto
                Me.T_Producto_Descripcion.Text = _Producto.Descripcion
                Me.R_DescripcionAmpliada.pText = _Producto.DescripcionAmpliada
                Me.R_DescripcionAmpliada_Tecnica.pText = _Producto.DescripcionAmpliada_Tecnica

                Dim _ProductoProveedor As Producto_Proveedor = oDTC.Producto_Proveedor.Where(Function(F) F.ID_Producto = _Producto.ID_Producto And F.ID_Proveedor = oclsEntradaLinea.oLinqEntrada.ID_Proveedor).FirstOrDefault

                If _ProductoProveedor Is Nothing = False Then
                    Me.T_CodigoProductoProveedor.Text = _ProductoProveedor.CodigoProductoProveedor
                End If
                'Les línies d'albarà on l'article requereix número de serie no es podrà introduir ni modificar la quantitat, s'haurà de fer des de el grid de números de serie
                Select Case oclsEntradaLinea.oEntradaTipo
                    Case EnumEntradaTipo.AlbaranCompra, EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.DevolucionCompra, EnumEntradaTipo.DevolucionVenta
                        If _Producto.RequiereNumeroSerie = True Then
                            Me.T_Unidades.Enabled = False
                            Me.T_Unidades.Value = 0
                        End If
                    Case EnumEntradaTipo.Regularizacion, EnumEntradaTipo.TraspasoAlmacen
                        If oDTC.RetornaStock(pID, oclsEntradaLinea.oLinqEntrada.ID_Almacen).Count > 0 Then
                            Me.T_StockReal.Value = oDTC.RetornaStock(pID, oclsEntradaLinea.oLinqEntrada.ID_Almacen).FirstOrDefault.StockReal
                        Else
                            Me.T_StockReal.Value = 0
                        End If
                        Me.T_CantidadDeseada.Value = Me.T_StockReal.Value
                        Me.T_Diferencia.Value = 0
                        If _Producto.RequiereNumeroSerie = True Then
                            Me.T_CantidadDeseada.Enabled = False
                            If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
                                Me.T_CantidadDeseada.Value = 0
                            End If
                        End If
                End Select

                If _Producto.RequiereNumeroSerie = True Then
                    Me.T_Unidades.InputMask = "{double:6.0}"
                Else
                    Me.T_Unidades.InputMask = "{double:6.2}"
                End If

                Select Case oclsEntradaLinea.oEntradaTipo
                    Case EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.PedidoVenta
                        Dim _PVP As Decimal
                        Dim _PVD As Decimal

                        Call frmPropuesta_Linea.RetornaPVPiPVD(pID, _PVP, _PVD)

                        Me.T_Precio.Value = _PVP
                        Me.T_Coste.Value = _PVD

                    Case EnumEntradaTipo.PedidoCompra, EnumEntradaTipo.AlbaranCompra
                        Dim _PVP As Decimal
                        Dim _PVD As Decimal

                        Call frmPropuesta_Linea.RetornaPVPiPVD(pID, _PVP, _PVD)

                        Me.T_Precio.Value = _PVD
                        Me.T_Coste.Value = _PVD
                End Select


                Call CargarFoto(_Producto)
                Me.C_Garantia.Value = _Producto.ID_Producto_Garantia
                Call VisualitzacioPantalla()
                Call CalcularStock()

                ' Me.TE_Producto_Codigo.Enabled = False
                Me.TE_Producto_Codigo.ButtonsRight("Lupeta").Enabled = False
                Me.TE_Producto_Codigo.ReadOnly = True

                Call CargarPreciosAnteriores()
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Instalacions()
        Dim DT As DataTable = BD.RetornaDataTable("SELECT ID_Instalacion,  OtrosDetalles, Poblacion, cast(ID_Instalacion as nvarchar(20)) + ' - ' + OtrosDetalles as Visualizacion FROM C_Instalacion WHERE Activo=1 and ID_Instalacion in (Select ID_Instalacion From Entrada_Instalacion Where ID_Entrada=" & oclsEntradaLinea.oLinqEntrada.ID_Entrada & " Group by ID_Instalacion) Order by ID_Instalacion")
        'Dim DT As DataTable = BD.RetornaDataTable("SELECT ID_Instalacion,  Cliente, Poblacion, FechaInstalacion, cast(ID_Instalacion as nvarchar(20)) + ' - ' + Cliente as Visualizacion FROM C_Instalacion WHERE Activo=1 and ID_Instalacion in (Select ID_Instalacion From Entrada_Instalacion Where ID_Entrada=" & oclsEntradaLinea.oLinqEntrada.ID_Entrada & " Group by ID_Instalacion) Order by ID_Instalacion")
        Me.C_Instalacion.DataSource = DT
        If DT Is Nothing Then
            Exit Sub
        End If

        With C_Instalacion

            .MaxDropDownItems = 16
            .DisplayMember = "Visualizacion"
            .ValueMember = "ID_Instalacion"
            .DisplayLayout.Bands(0).Columns("Visualizacion").Hidden = True
            .DisplayLayout.Bands(0).Columns("ID_Instalacion").Width = 100
            .DisplayLayout.Bands(0).Columns("ID_Instalacion").Header.Caption = "Código"
            .DisplayLayout.Bands(0).Columns("OtrosDetalles").Width = 500
            .DisplayLayout.Bands(0).Columns("OtrosDetalles").Header.Caption = "Descripción"
            .DisplayLayout.Bands(0).Columns("Poblacion").Width = 300
            .DisplayLayout.Bands(0).Columns("Poblacion").Header.Caption = "Población"
            '.DisplayLayout.Bands(0).Columns("FechaInstalacion").Width = 100
            '.DisplayLayout.Bands(0).Columns("FechaInstalacion").Header.Caption = "Fecha de instalación"
        End With
        Me.C_Instalacion.DropDownStyle = UltraComboStyle.DropDownList

        ' Me.C_Vinculacion.DisplayLayout.Bands(0).Override.AllowRowFiltering = DefaultableBoolean.True

    End Sub

    Private Sub CargarCombo_Contractes(ByVal pIDInstalacion As Integer)
        Me.C_Instalacion_Contrato.Value = Nothing
        Me.C_Instalacion_Contrato.DataSource = Nothing
        Me.C_Instalacion_Contrato.DropDownStyle = UltraComboStyle.DropDownList

        Dim DT As DataTable = BD.RetornaDataTable("SELECT ID_Instalacion_Contrato,  NumeroContrato, Descripcion FROM Instalacion_Contrato WHERE ID_Instalacion= " & pIDInstalacion & "  Order by ID_Instalacion")
        Me.C_Instalacion_Contrato.DataSource = DT
        If DT Is Nothing Then
            Exit Sub
        End If

        With C_Instalacion_Contrato
            .MaxDropDownItems = 16
            .DisplayMember = "NumeroContrato"
            .ValueMember = "ID_Instalacion_Contrato"
            .DisplayLayout.Bands(0).Columns("ID_Instalacion_Contrato").Hidden = True
            .DisplayLayout.Bands(0).Columns("NumeroContrato").Width = 100
            .DisplayLayout.Bands(0).Columns("NumeroContrato").Header.Caption = "Nº Contrato"
            .DisplayLayout.Bands(0).Columns("Descripcion").Width = 300
            .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripcion"
        End With


        '   Me.C_Vinculacion.DisplayLayout.Bands(0).Override.AllowRowFiltering = DefaultableBoolean.True

    End Sub

    Private Sub CargarFoto(ByRef pProducto As Producto)
        'If pProducto Is Nothing = False Then 'Si m'han passat el producto carregarem la foto si la te
        '    If pProducto.Archivo Is Nothing = False Then 'si te foto la carregarem
        '        If pProducto.Archivo.CampoBinario Is Nothing = False Then
        '            Me.Foto.Image = Util.BinaryToImage(pProducto.Archivo.CampoBinario.ToArray)
        '        End If
        '    Else
        '        Me.Foto.Image = Nothing
        '    End If

        'Else
        '    If oclsEntradaLinea.oLinqLinea.Producto.ID_Archivo_FotoPredeterminada.HasValue = True Then
        '        If oclsEntradaLinea.oLinqLinea.Producto.Archivo.CampoBinario Is Nothing = False Then
        '            Me.Foto.Image = Util.BinaryToImage(oclsEntradaLinea.oLinqLinea.Producto.Archivo.CampoBinario.ToArray)
        '        End If
        '    Else
        '        Me.Foto.Image = Nothing
        '    End If
        'End If


        Me.Foto.Image = Nothing
        If pProducto Is Nothing = False Then 'Si m'han passat el producto carregarem la foto si la te
            If pProducto.Archivo Is Nothing = False Then 'si te foto la carregarem
                If pProducto.Archivo.CampoBinario Is Nothing = False Then
                    Me.Foto.Image = Util.BinaryToImage(pProducto.Archivo.CampoBinario.ToArray)
                End If
            Else
                Me.Foto.Image = Nothing
            End If

        Else

            If oclsEntradaLinea.oLinqLinea.ID_Archivo_FotoPredeterminada.HasValue = True Then
                If oclsEntradaLinea.oLinqLinea.Archivo.CampoBinario Is Nothing = False Then
                    Me.Foto.Image = Util.BinaryToImage(oclsEntradaLinea.oLinqLinea.Archivo.CampoBinario.ToArray)
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
        End If
    End Sub

    Private Sub CalcularTotalBase()

        Dim _Preu As Decimal = 0
        Dim _Quantitat As Decimal = 0
        Dim _IVA As Decimal = 0
        Dim _Descompte1 As Decimal = 0
        Dim _Descompte2 As Decimal = 0

        If Me.T_Precio.Value Is Nothing = False AndAlso IsNumeric(Me.T_Precio.Value) = True Then
            _Preu = Me.T_Precio.Value
        End If

        If Me.T_Unidades.Value Is Nothing = False AndAlso IsNumeric(Me.T_Unidades.Value) = True Then
            _Quantitat = Me.T_Unidades.Value
        End If

        If Me.T_Descuento1.Value Is Nothing = False AndAlso IsNumeric(Me.T_Descuento1.Value) = True Then
            _Descompte1 = Me.T_Descuento1.Value
        End If

        If Me.T_Descuento2.Value Is Nothing = False AndAlso IsNumeric(Me.T_Descuento2.Value) = True Then
            _Descompte2 = Me.T_Descuento2.Value
        End If

        Me.T_TotalBase.Value = (_Quantitat * _Preu - ((_Quantitat * _Preu) * _Descompte1) / 100)
        Me.T_TotalBase.Value = (Me.T_TotalBase.Value - ((Me.T_TotalBase.Value) * _Descompte2) / 100)

        If Me.T_IVA.Value Is Nothing = False AndAlso IsNumeric(Me.T_IVA.Value) = True Then
            _IVA = Me.T_IVA.Value
        End If

        Me.T_TotalIVA.Value = (Me.T_TotalBase.Value * _IVA) / 100
        Me.T_Total.Value = Me.T_TotalBase.Value + Me.T_TotalIVA.Value

        Call CalcularResum()
    End Sub

    Private Sub CalcularResum()
        Try
            If IsNothing(Me.T_Coste.Value) = True OrElse IsDBNull(Me.T_Coste.Value) Then
                Me.T_Coste.Value = 0
            End If

            If IsNothing(Me.T_Unidades.Value) = True OrElse IsDBNull(Me.T_Unidades.Value) Then
                Me.T_Unidades.Value = 0
            End If

            If oclsEntradaLinea Is Nothing Then
                Exit Sub
            End If
            Me.T_CompuestoPor_NumProductos.Value = oclsEntradaLinea.oLinqLinea.Entrada_Linea_Hijo.Count
            Me.T_NumHoras.Value = oclsEntradaLinea.oLinqLinea.Parte_Horas.Sum(Function(F) F.Horas)
            Me.T_NumHorasExtras.Value = oclsEntradaLinea.oLinqLinea.Parte_Horas.Sum(Function(F) F.HorasExtras)
            Me.T_CompuestoPor_TotalCoste.Value = oDTC.RetornaCalculos_Entrada_Linea(oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea).FirstOrDefault.CompuestoPor_TotalCoste
            Me.T_ImporteGastos.Value = oclsEntradaLinea.oLinqLinea.Parte_Gastos.Sum(Function(F) F.Gasto)
            Me.T_Importe_Horas.Value = oclsEntradaLinea.oLinqLinea.RetornaImporteHoras
            Me.T_Importe_HorasExtras.Value = oclsEntradaLinea.oLinqLinea.RetornaImporteHorasExtras

            Dim _CosteLinea As Decimal = 0

            If Me.CH_NoRestarStock.Checked = False Then 'Si es posa no restar stock llavors no te cost la línea
                _CosteLinea = Me.T_Unidades.Value * Me.T_Coste.Value
            End If

            Me.T_TotalCoste.Value = Me.T_ImporteGastos.Value + Me.T_Importe_Horas.Value + Me.T_Importe_HorasExtras.Value + Me.T_CompuestoPor_TotalCoste.Value + _CosteLinea

            Me.T_Margen.Value = Me.T_TotalBase.Value - Me.T_TotalCoste.Value
            If Me.T_TotalBase.Value > 0 Then
                Me.T_Margen_Porcentaje.Value = Me.T_Margen.Value * 100 / Me.T_TotalBase.Value
            Else
                Me.T_Margen_Porcentaje.Value = 0
            End If


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub HabilitarEnFuncioEstatPantalla()
    '    'If Me.TE_Producto_Codigo.Tag Is Nothing = False Then 'si  hi ha producte seleccionat
    '    '    If oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault.RequiereNumeroSerie = True Then
    '    '        If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.PedidoVenta Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.PedidoCompra Then
    '    '            Util.Tab_InVisible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "NumeroSerie")
    '    '        Else
    '    '            Util.Tab_Visible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "NumeroSerie")
    '    '        End If
    '    '    Else
    '    '        Util.Tab_InVisible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "NumeroSerie")
    '    '    End If
    '    'End If

    '    ''Habilitar en funció del estat del document o del la linea
    '    'Select Case oclsEntradaLinea.oEntradaTipo
    '    '    Case EnumEntradaTipo.AlbaranCompra, EnumEntradaTipo.AlbaranVenta
    '    '        If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea <> 0 Then 'si es una modificació d'una línea d'albarà llavors...
    '    '            If oclsEntradaLinea.oLinqLinea.ID_Entrada_Factura.HasValue = True Then  'si la línea ja ha estat facturada llavors no podrem modificar re
    '    '                Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
    '    '                Me.ToolForm.M.clsToolBar.ToolBotons_Accions(m_ToolForm.clsToolForm.Enum_Seleccio.DesActivar, m_ToolForm.clsToolForm.Enum_Tipus_Seleccio.TODOS)
    '    '                Me.ToolForm.M.Botons.tSalir.SharedProps.Enabled = True
    '    '            End If
    '    '        End If
    '    '        'Case EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaVenta
    '    '        '    Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
    '    '        '    Me.ToolForm.M.clsToolBar.ToolBotons_Accions(m_ToolForm.clsToolForm.Enum_Seleccio.DesActivar, m_ToolForm.clsToolForm.Enum_Tipus_Seleccio.TODOS)
    '    '        '    Me.ToolForm.M.Botons.tSalir.SharedProps.Enabled = True
    '    'End Select
    'End Sub

    Private Sub DespresDeCarregarDadesGridArchivos()
        Try
            If Me.GRD_Ficheros.GRID.Rows.Count > 0 Then
                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Ficheros.GRID.Rows
                    If oclsEntradaLinea.oLinqLinea.ID_Archivo_FotoPredeterminada.HasValue = True AndAlso pRow.Cells("ID_Archivo").Value = oclsEntradaLinea.oLinqLinea.ID_Archivo_FotoPredeterminada Then
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
        If pIDArchivo = oclsEntradaLinea.oLinqLinea.ID_Archivo_FotoPredeterminada Then
            oclsEntradaLinea.oLinqLinea.ID_Archivo_FotoPredeterminada = Nothing
            oDTC.SubmitChanges()
        End If
    End Sub
    'Private Sub CargarCombo_Vinculacions(ByVal pID As Integer)

    '    Dim SQLText As String = ""
    '    If pID <> 0 Then
    '        SQLText = " and ID_Propuesta_Linea<>" & pID
    '    End If
    '    'Util.Cargar_Combo(Me.C_Vinculacion, "SELECT ID_Propuesta_Linea, cast(Identificador as nvarchar(20)) + ' - ' + Propuesta_Linea.Descripcion FROM Propuesta_Linea, Producto WHERE Producto.ID_Producto=Propuesta_Linea.ID_Producto and (Producto.Central=1 or Producto.Expansor=1 or Producto.Modulo_Rele=1) and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " ORDER BY Identificador", False)
    '    ' Util.Cargar_Combo(Me.C_Vinculacion, "SELECT ID_Propuesta_Linea, cast(Identificador as nvarchar(20)) + ' - ' + Propuesta_Linea.Descripcion FROM Propuesta_Linea, Producto WHERE Producto.ID_Producto=Propuesta_Linea.ID_Producto and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 ORDER BY Identificador", False)
    '    Dim DT As DataTable = BD.RetornaDataTable("SELECT  ID_Propuesta_Linea, Identificador, IdentificadorDelProducto,  Descripcion,Emplazamiento, Planta, Zona, NickZona  FROM C_Propuesta_Linea WHERE Activo=1 and  ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 and Conectable=1 ORDER BY IdentificadorDelProducto")
    '    Me.C_Vinculacion.DataSource = DT
    '    If DT Is Nothing Then
    '        Exit Sub
    '    End If
    '    Me.C_Vinculacion.MaxDropDownItems = 16
    '    Me.C_Vinculacion.DisplayMember = "Descripcion"
    '    Me.C_Vinculacion.ValueMember = "ID_Propuesta_Linea"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("ID_Propuesta_Linea").Hidden = True
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Header.Caption = "Identificador"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Header.Caption = "Linea"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Descripcion").Width = 400
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Emplazamiento").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Emplazamiento").Header.Caption = "Ubicación"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Planta").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Planta").Header.Caption = "Planta"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Zona").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Zona").Header.Caption = "Zona"
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("NickZona").Width = 100
    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("NickZona").Header.Caption = "Alias"
    '    'Me.C_Vinculacion.DropDownStyle = UltraComboStyle.DropDownList

    '    If oLinqPropuesta.SeInstalo = True Then
    '        'Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Hidden = False
    '        Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Hidden = True
    '    Else
    '        'Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("IdentificadorDelProducto").Hidden = True
    '        Me.C_Vinculacion.DisplayLayout.Bands(0).Columns("Identificador").Hidden = False
    '    End If

    '    Me.C_Vinculacion.DisplayLayout.Bands(0).Override.AllowRowFiltering = DefaultableBoolean.True

    '    'Util.Cargar_Combo(Me.UltraCombo1, "SELECT ID_Propuesta_Linea, cast(Identificador as nvarchar(20)), Propuesta_Linea.Descripcion FROM Propuesta_Linea, Producto WHERE Producto.ID_Producto=Propuesta_Linea.ID_Producto and ID_Propuesta=" & oLinqPropuesta.ID_Propuesta & SQLText & " and Unidad=1 ORDER BY Identificador", False)
    'End Sub

    'Private Sub ActivarPantalla(ByVal Activar As Boolean)
    '    If Activar = True Then
    '        Util.Activar(Me, M_Util.Enum_Controles_Activacion.TODOS_MENOS_ALGUNOS, True)  'Me.C_Planta, Me.C_Zona, Me.C_Abertura, Me.C_ElementosAProteger
    '    Else
    '        Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
    '    End If
    '    Me.TE_Producto_Codigo.ReadOnly = Activar

    '    'Fem això per no permetre vincular un articule central amb unaltre article
    '    If Me.TE_Producto_Codigo.Tag Is Nothing = False Then
    '        Dim ooDTC As New DTCDataContext(BD.Conexion)
    '        Dim ooLinqProducto As Producto = ooDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Producto_Codigo.Text).FirstOrDefault()
    '        If ooLinqProducto Is Nothing = False Then
    '            If ooLinqProducto.Central = True Then
    '                Me.C_Vinculacion.ReadOnly = True
    '            Else
    '                Me.C_Vinculacion.ReadOnly = False
    '            End If
    '        End If

    '        If ooLinqProducto.ID_Producto_Division = EnumProductoDivision.Informatica Then
    '            Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "UsuariosSistema")
    '        Else
    '            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "UsuariosSistema")
    '        End If
    '    End If

    'End Sub

    Private Sub CalcularStock()
        Dim _ID_Producto As Integer = 0
        Dim _ID_Almacen As Integer = 0

        If Me.TE_Producto_Codigo.Tag Is Nothing = False Then
            _ID_Producto = Me.TE_Producto_Codigo.Tag
        End If

        If Me.C_Almacen.SelectedIndex <> -1 Then
            _ID_Almacen = Me.C_Almacen.Value
        End If

        If _ID_Producto = 0 Then
            Me.T_StockActual.Value = 0
            Me.T_StockTeorico.Value = 0
        Else
            Dim oclsProducto As New clsProducto(_ID_Producto, oDTC)

            Me.T_StockActual.Value = oclsProducto.RetornaStockActualJaCalculat(_ID_Almacen)
            Me.T_StockTeorico.Value = oclsProducto.RetornaStockTeoricJaCalculat(_ID_Almacen)
        End If
    End Sub

    Private Sub VisualitzacioPantalla(Optional ByVal pSenseTocarLaPestanyaNS As Boolean = False)
        Try
            Me.L_Anotacion_ProvieneDeUnPedido.Visible = False
            Dim _SiShaSeleccionatElProducte As Boolean = False
            Dim _SiElProducteRequereixNS As Boolean = False
            Dim _SiEsUnaModificacioDeLaPantalla As Boolean = False

            If Me.TE_Producto_Codigo.Tag Is Nothing = False Then 'si  hi ha producte seleccionat
                _SiShaSeleccionatElProducte = True
                If oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Producto_Codigo.Tag)).FirstOrDefault.RequiereNumeroSerie = True Then
                    _SiElProducteRequereixNS = True
                End If
            End If

            If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea <> 0 Then
                _SiEsUnaModificacioDeLaPantalla = True
            End If

            If pSenseTocarLaPestanyaNS = False Then
                Util.Tab_InVisible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "NumeroSerie")
            End If

            '  Me.GRD_NS.ToolGrid.Tools("SeleccionarNS").SharedProps.Visible = False


            'If _SiElProducteRequereixNS = True Then
            '    Me.CH_NoRestarStock.Enabled = False
            '    Me.CH_NoRestarStock.Checked = False
            'Else
            '    Me.CH_NoRestarStock.Enabled = True
            'End If


            Select Case oclsEntradaLinea.oEntradaTipo
                Case EnumEntradaTipo.PedidoCompra
                    Util.Tab_InVisible_x_Key(Me.Tab_PreciosAnteriores, M_Util.Enum_Tab_Activacion.ALGUNOS, "Propuestas")
                    Call VisualitzarElsBotonsGuardarYContinuarYGuardarYMantener()
                    If _SiEsUnaModificacioDeLaPantalla = True AndAlso oclsEntradaLinea.oLinqLinea.CantidadTraspasada > 0 Then
                        Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
                    End If

                Case EnumEntradaTipo.AlbaranCompra, EnumEntradaTipo.DevolucionCompra
                    Util.Tab_InVisible_x_Key(Me.Tab_PreciosAnteriores, M_Util.Enum_Tab_Activacion.ALGUNOS, "Propuestas")
                    Call VisualitzarElsBotonsGuardarYContinuarYGuardarYMantener()
                    Call VisualitzarONoTotElTemaNS(_SiShaSeleccionatElProducte, _SiElProducteRequereixNS, _SiEsUnaModificacioDeLaPantalla)
                    Me.CH_NoRestarStock.Visible = True
                    If _SiEsUnaModificacioDeLaPantalla = True Then 'si es una modificació d'una línea d'albarà llavors...
                        If (oclsEntradaLinea.LineaRelacionadaAmbUnaComanda = True Or oclsEntradaLinea.oLinqLinea.Producto.RequiereNumeroSerie = True) Then
                            Call HabilitarCampsImportats(False)
                            Me.T_Unidades.Enabled = False
                        End If

                        If oclsEntradaLinea.oLinqLinea.ID_Entrada_Factura.HasValue = True Then  'si la línea ja ha estat facturada llavors no podrem modificar re
                            Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
                            Me.ToolForm.M.clsToolBar.ToolBotons_Accions(m_ToolForm.clsToolForm.Enum_Seleccio.DesActivar, m_ToolForm.clsToolForm.Enum_Tipus_Seleccio.TODOS)
                            Me.ToolForm.M.Botons.tSalir.SharedProps.Enabled = True
                            Me.ToolForm.M.Botons.tAccions.SharedProps.Enabled = True
                        End If

                        If oclsEntradaLinea.LineaRelacionadaAmbUnaComanda = True Then
                            Me.L_Anotacion_ProvieneDeUnPedido.Visible = True
                        End If
                    End If
                    If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.AlbaranCompra Then
                        Me.GRD_NS.ToolGrid.Tools("SeleccionarNS").SharedProps.Visible = False
                    End If

                Case EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaCompraRectificativa
                    Util.Tab_InVisible_x_Key(Me.Tab_PreciosAnteriores, M_Util.Enum_Tab_Activacion.ALGUNOS, "Propuestas")
                    Call VisualitzarElsBotonsGuardarYContinuarYGuardarYMantener()
                    Call VisualitzarONoTotElTemaNS(_SiShaSeleccionatElProducte, _SiElProducteRequereixNS, _SiEsUnaModificacioDeLaPantalla)
                    Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS_MENOS_ALGUNOS, True, Me.T_Producto_Descripcion)
                    Me.ToolForm.M.clsToolBar.ToolBotons_Accions(m_ToolForm.clsToolForm.Enum_Seleccio.DesActivar, m_ToolForm.clsToolForm.Enum_Tipus_Seleccio.TODOS)
                    Me.ToolForm.M.Botons.tSalir.SharedProps.Enabled = True
                    Me.ToolForm.M.Botons.tAccions.SharedProps.Enabled = True
                    If oclsEntradaLinea.oEntradaEstado <> EnumEntradaEstado.Cerrado Then
                        Me.ToolForm.M.Botons.tGuardar.SharedProps.Enabled = True
                    End If

                Case EnumEntradaTipo.PedidoVenta
                    Call VisualitzarElsBotonsGuardarYContinuarYGuardarYMantener()

                Case EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.DevolucionVenta
                    Call VisualitzarElsBotonsGuardarYContinuarYGuardarYMantener()
                    Util.Tab_Visible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "CompuestoPor", "ParteTrabajoVenta", "TalYComoSeInstalo")
                    Me.Frm_Garantia.Visible = True
                    Me.Frm_Ubicacion.Visible = True
                    Me.Frm_Vinculaciones.Visible = True
                    Me.CH_NoRestarStock.Visible = True
                    '  Me.GRD_NS.ToolGrid.Tools("SeleccionarNS").SharedProps.Visible = True
                    Call VisualitzarONoTotElTemaNS(_SiShaSeleccionatElProducte, _SiElProducteRequereixNS, _SiEsUnaModificacioDeLaPantalla)
                    Me.GRD_NS.M.Botons.tGridAfegir.SharedProps.Enabled = False
                    Me.GRD_NS.M.Botons.tGridEliminar.SharedProps.Enabled = False

                    If _SiEsUnaModificacioDeLaPantalla = True Then 'si es una modificació d'una línea d'albarà llavors...
                        If oclsEntradaLinea.oLinqLinea.ID_Entrada_Factura.HasValue = True Then  'si la línea ja ha estat facturada llavors no podrem modificar re
                            Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
                            Me.ToolForm.M.clsToolBar.ToolBotons_Accions(m_ToolForm.clsToolForm.Enum_Seleccio.DesActivar, m_ToolForm.clsToolForm.Enum_Tipus_Seleccio.TODOS)
                            Me.ToolForm.M.Botons.tSalir.SharedProps.Enabled = True
                            Me.ToolForm.M.Botons.tAccions.SharedProps.Enabled = True
                        End If
                    End If

                    If _SiEsUnaModificacioDeLaPantalla = True AndAlso (oclsEntradaLinea.LineaRelacionadaAmbUnaComanda = True Or oclsEntradaLinea.oLinqLinea.Producto.RequiereNumeroSerie = True) Then
                        Call HabilitarCampsImportats(False)
                        Me.T_Unidades.Enabled = False
                        'Me.T_Producto_Descripcion.ReadOnly = False
                        'Me.T_Producto_Descripcion.Enabled = True
                        Me.T_Detalle.ReadOnly = False
                    End If

                    If oclsEntradaLinea.LineaRelacionadaAmbUnaComanda = True Then
                        Me.L_Anotacion_ProvieneDeUnPedido.Visible = True
                    End If
                    If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.AlbaranVenta Then
                        Me.GRP_FechasInstalacion.Visible = True
                    End If


                Case EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaVentaRectificativa
                    Call VisualitzarElsBotonsGuardarYContinuarYGuardarYMantener()
                    Util.Tab_Visible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "CompuestoPor", "ParteTrabajoVenta", "TalYComoSeInstalo")
                    Me.Frm_Garantia.Visible = True
                    Me.Frm_Ubicacion.Visible = True
                    Me.Frm_Vinculaciones.Visible = True
                    Call VisualitzarONoTotElTemaNS(_SiShaSeleccionatElProducte, _SiElProducteRequereixNS, _SiEsUnaModificacioDeLaPantalla)
                    Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
                    Me.T_Producto_Descripcion.ReadOnly = False
                    Me.T_Detalle.ReadOnly = False
                    Me.ToolForm.M.clsToolBar.ToolBotons_Accions(m_ToolForm.clsToolForm.Enum_Seleccio.DesActivar, m_ToolForm.clsToolForm.Enum_Tipus_Seleccio.TODOS)
                    Me.ToolForm.M.Botons.tSalir.SharedProps.Enabled = True
                    Me.ToolForm.M.Botons.tAccions.SharedProps.Enabled = True
                    If oclsEntradaLinea.oEntradaEstado <> EnumEntradaEstado.Cerrado Then
                        Me.ToolForm.M.Botons.tGuardar.SharedProps.Enabled = True
                    End If

                    If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.FacturaVenta Then
                        Me.GRP_FechasInstalacion.Visible = True
                    End If

                    'Case EnumEntradaTipo.DevolucionVenta
                    '    Call VisualitzarElsBotonsGuardarYContinuarYGuardarYMantener()
                    '    Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General", "Observaciones", "Ficheros")
                    '    Util.Tab_InVisible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Precios")
                    '    Util.Tab_Visible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "Precios")
                    '    Me.UltraLabel55.Visible = False
                    '    Me.C_Instalacion.Visible = False
                    '    ' Me.GRD_NS.ToolGrid.Tools("SeleccionarNS").SharedProps.Visible = True
                    '    Call VisualitzarONoTotElTemaNS(_SiShaSeleccionatElProducte, _SiElProducteRequereixNS, _SiEsUnaModificacioDeLaPantalla)

                    'Case EnumEntradaTipo.DevolucionCompra
                    '    Call VisualitzarElsBotonsGuardarYContinuarYGuardarYMantener()
                    '    Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General", "Observaciones", "Ficheros")
                    '    Util.Tab_InVisible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.TODOS)
                    '    'Util.Tab_Visible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "Precios")
                    '    Me.UltraLabel55.Visible = False
                    '    Me.C_Instalacion.Visible = False
                    '    Me.GRD_NS.ToolGrid.Tools("SeleccionarNS").SharedProps.Visible = True
                    '    Call VisualitzarONoTotElTemaNS(_SiShaSeleccionatElProducte, _SiElProducteRequereixNS, _SiEsUnaModificacioDeLaPantalla)

                Case EnumEntradaTipo.Regularizacion
                    Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General", "Observaciones", "Ficheros")
                    Util.Tab_InVisible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Regularizacion")
                    Util.Tab_Visible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "Regularizacion")
                    Me.TAB_General.Tabs("Regularizacion").Text = "General"
                    Me.GRD_NS.M.Botons.tGridAfegir.SharedProps.Visible = False
                    ' Me.GRD_NS.ToolGrid.Tools("SeleccionarNS").SharedProps.Visible = True
                    Call VisualitzarONoTotElTemaNS(_SiShaSeleccionatElProducte, _SiElProducteRequereixNS, _SiEsUnaModificacioDeLaPantalla)

                Case EnumEntradaTipo.TraspasoAlmacen
                    Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General", "Observaciones", "Ficheros")
                    Util.Tab_InVisible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Regularizacion")
                    Util.Tab_Visible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "Regularizacion")
                    Me.TAB_General.Tabs("Regularizacion").Text = "General"
                    Me.L_CantidadDeseada.Text = "Cantidad a traspasar:"
                    Me.L_NotaRegularizacion.Visible = False
                    Me.L_Diferencia.Visible = False
                    Me.T_Diferencia.Visible = False
                    ' Me.GRD_NS.ToolGrid.Tools("SeleccionarNS").SharedProps.Visible = True
                    Call VisualitzarONoTotElTemaNS(_SiShaSeleccionatElProducte, _SiElProducteRequereixNS, _SiEsUnaModificacioDeLaPantalla)

            End Select

            If oclsEntradaLinea.oEntradaEstado = EnumEntradaEstado.Cerrado Then
                Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS_MENOS_ALGUNOS, True, Me.OP_Filtre)
                Me.OP_Filtre.Enabled = True
                Exit Sub
            Else
                Me.OP_Filtre.Enabled = True
            End If

            If oclsEntradaLinea.EsUnaLineaNova = False Then
                Call HabilitarCampsImportats(False)
            End If


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub VisualitzarElsBotonsGuardarYContinuarYGuardarYMantener()
        Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True
        Me.ToolForm.M.Botons.tImprimir.SharedProps.Caption = "Guardar y continuar"
        Me.ToolForm.M.Botons.tBuscar.SharedProps.Visible = True
        Me.ToolForm.M.Botons.tBuscar.SharedProps.Caption = "Guardar y mantener"
    End Sub

    Private Sub VisualitzarONoTotElTemaNS(ByVal pSiShaSeleccionatElProducte As Boolean, ByVal pSiElProducteRequereixNS As Boolean, ByVal pSiEsUnaModificacioDeLaPantalla As Boolean)
        If pSiShaSeleccionatElProducte = True And pSiElProducteRequereixNS = True Then 'si  hi ha producte seleccionat
            Util.Tab_Visible_x_Key(Me.TAB_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "NumeroSerie")
            If pSiEsUnaModificacioDeLaPantalla = True Then
                If oclsEntradaLinea.EsPotEditarNS Then
                    Me.GRD_NS.M.Botons.tGridAfegir.SharedProps.Enabled = True
                    Me.GRD_NS.M.Botons.tGridEliminar.SharedProps.Enabled = True
                Else
                    Me.GRD_NS.M.Botons.tGridAfegir.SharedProps.Enabled = False
                    Me.GRD_NS.M.Botons.tGridEliminar.SharedProps.Enabled = False
                End If
            Else
                'si no es una modificació i es un traspaso de almacén el botó afegir no te que ser-hi
                If oclsEntradaLinea.oLinqEntrada.ID_Entrada_Tipo = EnumEntradaTipo.TraspasoAlmacen Then
                    Me.GRD_NS.M.Botons.tGridAfegir.SharedProps.Enabled = False
                    Me.GRD_NS.M.Botons.tGridEliminar.SharedProps.Enabled = False
                End If
            End If
        End If
    End Sub

    Private Sub CarregarDadesPredeterminadesDelDocument()
        Me.C_Almacen.Value = oclsEntradaLinea.oLinqEntrada.ID_Almacen
        Me.T_Referencia_Pedido.Text = oclsEntradaLinea.oLinqEntrada.NumPedidoCliente
        Me.T_Referencia_Numero.Text = oclsEntradaLinea.oLinqEntrada.NumReferencia
        Me.T_Referencia_NombreObra.Text = oclsEntradaLinea.oLinqEntrada.NombreObra
        Me.T_Referencia_Proyecto.Text = oclsEntradaLinea.oLinqEntrada.Proyecto
    End Sub

    Private Sub CarregarDadesPestanyes(ByVal pKeyPestanya As String)
        Try

            Dim _ID As Integer = oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea

            Select Case pKeyPestanya
                Case "NS"
                    Call CargaGrid_NS(_ID)
                Case "PreciosAnteriores"
                    Call CargarPreciosAnteriores()
                Case "ContenidoDeLosPartes"
                    With Me.GRD_MaterialEnAlmacenesDeLosPartesAsignados
                        .GRID.DataSource = Nothing
                        .M.clsUltraGrid.Cargar("Select * From RetornaStock(0,0) Where ID_Almacen In (Select ID_Almacen From Almacen Where ID_Parte in (Select ID_Parte From Entrada_Parte Where ID_Entrada=" & oclsEntradaLinea.oLinqEntrada.ID_Entrada & ")) Order by ProductoDescripcion", BD)
                    End With

                    With Me.GRD_HorasAsignadasALosPartes
                        Dim DT As DataTable
                        DT = BD.RetornaDataTable("Select * From C_Parte_Horas Where ID_Parte in (Select ID_Parte From Entrada_Parte Where ID_Entrada =" & oclsEntradaLinea.oLinqEntrada.ID_Entrada & ") Order By Fecha", True)
                        .M.clsUltraGrid.Cargar(DT)
                        .M_NoEditable()

                        Dim _pRow As UltraGridRow
                        For Each _pRow In .GRID.Rows
                            If IsDBNull(_pRow.Cells("ID_Entrada_Linea").Value) = False Then
                                _pRow.CellAppearance.BackColor = Color.LightGreen
                            End If
                        Next

                        .GRID.Selected.Rows.Clear()
                        .GRID.ActiveRow = Nothing
                    End With
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Events varis"

    Private Sub TE_Producto_Codigo_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Producto_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim _VariacioSQL As String = ""
            Dim _VariacioSQL2 As String = ""
            Dim _VariacioSQL3 As String = ""

            If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
                _VariacioSQL = " Where Fecha_Baja is null and ID_Producto in (Select ID_Producto From TempStockRealPorProductoYPorAlmacen Where ID_Almacen=" & Me.C_Almacen.Value & " and StockReal>0)"
                _VariacioSQL2 = " , (Select StockReal From TempStockRealPorProductoYPorAlmacen Where ID_Producto= C_Producto.ID_Producto  and ID_Almacen=" & Me.C_Almacen.Value & ") as StockReal "

                '_VariacioSQL = " Where ID_Producto in (Select ID_Producto From retornastock(0," & Me.C_Almacen.Value & ") Where StockReal>0)"
                '  _VariacioSQL2 = " , (Select StockReal From retornastock(C_Producto.ID_Producto," & Me.C_Almacen.Value & ")) as StockReal "
            Else
                _VariacioSQL = " Where Fecha_Baja is null "
            End If

            If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.PedidoCompra Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.AlbaranCompra Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.FacturaCompra Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.DevolucionCompra Then
                _VariacioSQL3 = ", (Select top 1 CodigoProductoProveedor From Producto_Proveedor Where ID_Producto=C_Producto.ID_Producto and ID_Proveedor=" & oclsEntradaLinea.oLinqEntrada.ID_Proveedor & ") as CodigoProductoProveedor "
            End If

            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * " & _VariacioSQL2 & _VariacioSQL3 & " FROM C_Producto " & _VariacioSQL & " ORDER BY Descripcion", Me.TE_Producto_Codigo, "ID_Producto", "Codigo", Me.T_Producto_Descripcion, "Descripcion")
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
    End Sub

    Private Sub AlApretarElBotoAuxiliarDelLlistatGeneric(ByRef pInstanciaLlistatGeneric As M_LlistatGeneric.clsLlistatGeneric)
        'Hem tingut que tornar a copiar les mateixes línies que el  TE_Producto_Codigo_EditorButtonClick però treient lo de la fecha baja

        Dim _VariacioSQL As String = ""
        Dim _VariacioSQL2 As String = ""
        Dim _VariacioSQL3 As String = ""

        If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
            _VariacioSQL = " Where ID_Producto in (Select ID_Producto From TempStockRealPorProductoYPorAlmacen Where ID_Almacen=" & Me.C_Almacen.Value & " and StockReal>0)"
            _VariacioSQL2 = " , (Select StockReal From TempStockRealPorProductoYPorAlmacen Where ID_Producto= C_Producto.ID_Producto  and ID_Almacen=" & Me.C_Almacen.Value & ") as StockReal "

            '_VariacioSQL = " Where ID_Producto in (Select ID_Producto From retornastock(0," & Me.C_Almacen.Value & ") Where StockReal>0)"
            '  _VariacioSQL2 = " , (Select StockReal From retornastock(C_Producto.ID_Producto," & Me.C_Almacen.Value & ")) as StockReal "
        Else
            _VariacioSQL = " "
        End If

        If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.PedidoCompra Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.AlbaranCompra Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.FacturaCompra Or oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.DevolucionCompra Then
            _VariacioSQL3 = ", (Select top 1 CodigoProductoProveedor From Producto_Proveedor Where ID_Producto=C_Producto.ID_Producto and ID_Proveedor=" & oclsEntradaLinea.oLinqEntrada.ID_Proveedor & ") as CodigoProductoProveedor "
        End If


        'Això es només per traspasos de magatzems
        pInstanciaLlistatGeneric.pGrid.M.clsUltraGrid.Cargar("SELECT * " & _VariacioSQL2 & _VariacioSQL3 & " FROM C_Producto " & _VariacioSQL & " ORDER BY Descripcion", BD)
        pInstanciaLlistatGeneric.AplicarCanvisBotoAuxiliarAlNouGrid()
        pInstanciaLlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = False

    End Sub

    Private Sub TE_Producto_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Producto_Codigo.KeyDown
        If Me.TE_Producto_Codigo.Text Is Nothing = False Then
            If e.KeyCode = Keys.Enter Then
                Dim ooLinqProducto As Producto = oDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Producto_Codigo.Text).FirstOrDefault()
                If ooLinqProducto Is Nothing = False Then
                    Call AlTancarLlistatGeneric(ooLinqProducto.ID_Producto)
                    'Me.TE_Producto_Codigo.Tag = ooLinqProducto.ID_Producto
                    'Me.T_Producto_Descripcion.Text = ooLinqProducto.Descripcion
                    ''Call CargaPreuProductoYTraspasable(ooLinqProducto.ID_Producto)
                    ''Call ActivarPantalla(True)
                    ''If Me.T_Unidades.Visible = True Then
                    ''    Me.T_Unidades.Focus()
                    ''Else
                    ''    Me.T_Producto_Descripcion.Focus()
                    ''End If
                    'Call CargarFoto(ooLinqProducto)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Me.TE_Producto_Codigo.Value = Nothing
                End If
            End If
        End If
    End Sub

    Private Sub Tab_Principal_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles Tab_Principal.SelectedTabChanged
        Call CarregarDadesPestanyes(e.Tab.Key)
    End Sub

    Private Sub TAB_General_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_General.SelectedTabChanged
        Select Case e.Tab.Key
            Case "ParteTrabajoVenta"
                Call CargarGrid_Partes()
            Case "CompuestoPor"
                Call CargaGrid_CompuestoPor()
        End Select
    End Sub

    Private Sub C_Instalacion_RowSelected(sender As Object, e As RowSelectedEventArgs) Handles C_Instalacion.RowSelected
        If e.Row Is Nothing = False AndAlso IsNothing(e.Row.Cells("ID_Instalacion").Value) = False Then
            Call CargarCombo_Contractes(e.Row.Cells("ID_Instalacion").Value)
            Util.Cargar_Combo(Me.C_Emplazamiento, "SELECT ID_Instalacion_Emplazamiento, Descripcion FROM Instalacion_Emplazamiento Where ID_Instalacion=" & e.Row.Cells("ID_Instalacion").Value & " ORDER BY Descripcion", False)
        Else
            'Me.C_Instalacion_Contrato.SelectedRow = Nothing
            'Me.C_Emplazamiento.SelectedIndex = 0
        End If
        Me.C_Planta.ReadOnly = False
        Me.C_Zona.ReadOnly = True
        Me.C_ElementosAProteger.ReadOnly = True
        Me.C_Abertura.ReadOnly = True
        Me.C_Planta.Value = Nothing
        Me.C_Zona.Value = Nothing
        Me.C_ElementosAProteger.Value = Nothing
        Me.C_Abertura.Value = Nothing
    End Sub

    Private Sub T_Unidades_ValueChanged(sender As Object, e As System.EventArgs) Handles T_Unidades.ValueChanged
        Call CalcularTotalBase()
    End Sub

    Private Sub T_Preu_ValueChanged(sender As Object, e As System.EventArgs) Handles T_Precio.ValueChanged
        Call CalcularTotalBase()
    End Sub

    Private Sub T_Descuento1_ValueChanged(sender As Object, e As System.EventArgs) Handles T_Descuento1.ValueChanged
        Call CalcularTotalBase()
    End Sub

    Private Sub T_Descuento2_ValueChanged(sender As Object, e As System.EventArgs) Handles T_Descuento2.ValueChanged
        Call CalcularTotalBase()
    End Sub

    Private Sub T_IVA_ValueChanged(sender As Object, e As System.EventArgs) Handles T_IVA.ValueChanged
        Call CalcularTotalBase()
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

    Private Sub C_ElementosAProteger_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_ElementosAProteger.EditorButtonClick
        If e.Button.Key = "Cancelar" Then
            If C_ElementosAProteger.Value Is Nothing = False Then  'fem això pq si s'apreta el botó cancelar i no hi ha cap registre seleccionat llavors no faci re
                Me.C_ElementosAProteger.Value = Nothing
                Me.C_Abertura.ReadOnly = False
            End If
        End If
    End Sub

    Private Sub C_Abertura_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles C_Abertura.EditorButtonClick
        If e.Button.Key = "Cancelar" Then
            If C_Abertura.Value Is Nothing = False Then 'fem això pq si s'apreta el botó cancelar i no hi ha cap registre seleccionat llavors no faci re
                Me.C_Abertura.Value = Nothing
                Me.C_ElementosAProteger.ReadOnly = False
            End If
        End If
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

    Private Sub B_Calcular_Garantia_Click(sender As Object, e As EventArgs) Handles B_Calcular_Garantia.Click
        If Me.C_Garantia.Items.Count = 0 OrElse Me.DT_Entrega.Value Is Nothing Then
            Exit Sub
        End If
        Me.DT_FinGarantia.Value = CDate(Me.DT_Entrega.Value).AddYears(Me.C_Garantia.Value)
    End Sub

    Private Sub DT_Entrega_ValueChanged(sender As Object, e As EventArgs) Handles DT_Entrega.ValueChanged
        Call B_Calcular_Garantia_Click(Nothing, Nothing)
    End Sub

    Private Sub C_Garantia_ValueChanged(sender As Object, e As EventArgs) Handles C_Garantia.ValueChanged
        Call B_Calcular_Garantia_Click(Nothing, Nothing)
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
                oclsEntradaLinea.oLinqLinea.Archivo = _Archivo
                oDTC.SubmitChanges()
                Call DespresDeCarregarDadesGridArchivos()
            End If

            If e.Tool.Key = "QuitarFotoPredeterminada" Then
                'Dim _IDArchivo As Integer = Me.GRD_Ficheros.GRID.Selected.Rows(0).Cells("ID_Archivo").Value
                'Dim _Archivo As Archivo
                '_Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = _IDArchivo).FirstOrDefault
                'If _Archivo.ID_Archivo = oLinqPropuesta_Linea.ID_Archivo_FotoPredeterminada Then
                oclsEntradaLinea.oLinqLinea.Archivo = Nothing
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
        oclsEntradaLinea.oLinqLinea.Archivo = Nothing
        oDTC.SubmitChanges()
    End Sub

    Private Sub T_CantidadActual_ValueChanged(sender As Object, e As EventArgs) Handles T_CantidadDeseada.ValueChanged
        If T_CantidadDeseada.Value Is Nothing = False AndAlso IsNumeric(Me.T_CantidadDeseada.Value) Then
            Me.T_Diferencia.Value = Me.T_CantidadDeseada.Value - Me.T_StockReal.Value
        End If
    End Sub

    Private Sub C_Almacen_ValueChanged(sender As Object, e As EventArgs) Handles C_Almacen.ValueChanged
        Call CalcularStock()

    End Sub

    Private Sub C_Almacen_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Almacen.BeforeDropDown
        If oclsEntradaLinea.oLinqLinea.Entrada_Linea_NS.Count > 0 Then
            e.Cancel = True
            Exit Sub
        End If

        Dim _IDAlmacen As Integer = Me.C_Almacen.Value
        Dim _Boto As Infragistics.Win.UltraWinEditors.StateEditorButton = Me.C_Almacen.ButtonsRight("Todos")
        Select Case _Boto.CheckState
            Case CheckState.Checked 'Visualizar todos
                Util.Cargar_Combo(Me.C_Almacen, "SELECT ID_Almacen, Descripcion FROM Almacen Where Activo=1 ORDER BY Descripcion", False)
            Case CheckState.Indeterminate  'Visualizar almacenes de tipo parte de las instalaciones asignadas
                Util.Cargar_Combo(Me.C_Almacen, "Select ID_Almacen, Descripcion From Almacen Where Activo=1 and ID_Parte in (SELECT   dbo.Parte.ID_Parte FROM dbo.Parte INNER JOIN dbo.Almacen ON dbo.Parte.ID_Parte = dbo.Almacen.ID_Parte INNER JOIN dbo.Entrada_Instalacion ON dbo.Parte.ID_Instalacion = dbo.Entrada_Instalacion.ID_Instalacion WHERE dbo.Entrada_Instalacion.ID_Entrada = " & oclsEntradaLinea.oLinqEntrada.ID_Entrada & " ) Order by Descripcion", False, False)
            Case CheckState.Unchecked  'Visualizar solo los asignados
                If oclsEntradaLinea.oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
                    Util.Cargar_Combo(Me.C_Almacen, "SELECT Almacen.ID_Almacen, Almacen.Descripcion FROM  Almacen LEFT OUTER JOIN  Parte ON Almacen.ID_Parte = dbo.Parte.ID_Parte WHERE (isnull(Parte.ID_Parte_Estado,0) <> 3) Order by Descripcion", True, False)
                Else
                    Util.Cargar_Combo(Me.C_Almacen, "Select ID_Almacen, Descripcion From Almacen Where Activo=1 and ID_Almacen in (Select ID_Almacen From Almacen Where Predeterminado=1 or ID_Parte in (Select ID_Parte From Entrada_Parte Where ID_Entrada=" & oclsEntradaLinea.oLinqEntrada.ID_Entrada & ")) or ID_Almacen In (SELECT dbo.Almacen.ID_Almacen  FROM  dbo.Entrada_Parte INNER JOIN dbo.Parte ON dbo.Entrada_Parte.ID_Parte = dbo.Parte.ID_Parte INNER JOIN dbo.Parte_Asignacion ON dbo.Parte.ID_Parte = dbo.Parte_Asignacion.ID_Parte INNER JOIN   dbo.Almacen ON dbo.Parte_Asignacion.ID_Personal = dbo.Almacen.ID_Personal WHERE (dbo.Entrada_Parte.ID_Entrada = " & oclsEntradaLinea.oLinqEntrada.ID_Entrada & ") GROUP BY dbo.Entrada_Parte.ID_Entrada, dbo.Almacen.ID_Almacen) Order by Descripcion", False, False)
                End If
        End Select

        Me.C_Almacen.Value = _IDAlmacen
    End Sub

    Private Sub C_Almacen_BeforeEditorButtonCheckStateChanged(sender As Object, e As UltraWinEditors.BeforeCheckStateChangedEventArgs) Handles C_Almacen.BeforeEditorButtonCheckStateChanged
        Dim _Boto As Infragistics.Win.UltraWinEditors.StateEditorButton = Me.C_Almacen.ButtonsRight("Todos")
        _Boto.DisplayStyle = UltraWinEditors.StateButtonDisplayStyle.StateButtonTriState

        Select Case _Boto.CheckState
            Case CheckState.Checked
                _Boto.Text = "Visualizar los almacenes relacionados"
            Case CheckState.Indeterminate
                _Boto.Text = "Visualizar todos"
            Case CheckState.Unchecked
                _Boto.Text = "Visualizar los almacenes-partes"

        End Select

    End Sub

    Private Sub CH_NoImprimir_CheckedChanged(sender As Object, e As EventArgs) Handles CH_NoImprimir.CheckedChanged
        If Me.CH_NoImprimir.Checked = True Then
            Me.T_Precio.Value = 0
            Me.T_Descuento1.Value = 0
            Me.T_Descuento2.Value = 0
        End If
    End Sub

    Private Sub CH_NoRestarStock_CheckedChanged(sender As Object, e As EventArgs) Handles CH_NoRestarStock.CheckedChanged
        Me.T_StockActual.Visible = Not Me.CH_NoRestarStock.Checked
        Me.L_StockActual.Visible = Not Me.CH_NoRestarStock.Checked
        Call CalcularResum()
    End Sub

    Private Sub T_Coste_ValueChanged(sender As Object, e As EventArgs) Handles T_Coste.ValueChanged
        Call CalcularResum()
    End Sub

    Private Sub GRD_MaterialEnAlmacenesDeLosPartesAsignados_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_MaterialEnAlmacenesDeLosPartesAsignados.M_ToolGrid_ToolClickBotonsExtras2
        Select Case e.Tool.Key

            Case "VerProducto"
                With Me.GRD_MaterialEnAlmacenesDeLosPartesAsignados
                    If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Cells.Exists("ID_Producto") = False Then
                        Exit Sub
                    End If

                    Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
                    frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
                    frm.FormObrir(frmPrincipal, True)
                End With

            Case "VerTrazabilidad"
                With Me.GRD_MaterialEnAlmacenesDeLosPartesAsignados
                    If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Cells.Exists("ID_Producto") = False Then
                        Exit Sub
                    End If

                    Dim frm As New frmProducto_Trazabilidad
                    frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
                    frm.FormObrir(Me, True)
                End With

        End Select
    End Sub

    Private Sub GRD_HorasAsignadasALosPartes_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_HorasAsignadasALosPartes.M_ToolGrid_ToolClickBotonsExtras
        Select Case e.Tool.Key
            Case "IrAlParte"
                Dim IDParte As Integer

                If Me.GRD_HorasAsignadasALosPartes.GRID.ActiveRow Is Nothing = True Then
                    Exit Sub
                End If
                IDParte = Me.GRD_HorasAsignadasALosPartes.GRID.ActiveRow.Cells("ID_Parte").Value

                Dim frm As New frmParte
                frm.Entrada(IDParte)
                frm.FormObrir(Me, True)
        End Select


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

        If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea = 0 Then
            'Si actualment no tenim cap registre carregat farem veure com si estiguessim al últim.
            _IDATrobar = oclsEntradaLinea.oLinqEntrada.Entrada_Linea.Max(Function(F) F.ID_Entrada_Linea)
            _Trobat = True
        Else
            _IDATrobar = oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea
        End If

        Dim _LlistatLinies As IList(Of Entrada_Linea)
        Select Case Me.OP_Filtre.Value
            Case "Orden"
                If pAvanzar = True Then
                    _LlistatLinies = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada = oclsEntradaLinea.oLinqLinea.ID_Entrada).OrderBy(Function(F) F.ID_Entrada_Linea).ToList
                Else
                    _LlistatLinies = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada = oclsEntradaLinea.oLinqLinea.ID_Entrada).OrderByDescending(Function(F) F.ID_Entrada_Linea).ToList
                End If
            Case "Articulo"
                If pAvanzar = True Then
                    _LlistatLinies = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada = oclsEntradaLinea.oLinqLinea.ID_Entrada And F.ID_Producto = oclsEntradaLinea.oLinqLinea.ID_Producto).OrderBy(Function(F) F.ID_Entrada_Linea).ToList
                Else
                    _LlistatLinies = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada = oclsEntradaLinea.oLinqLinea.ID_Entrada And F.ID_Producto = oclsEntradaLinea.oLinqLinea.ID_Producto).OrderByDescending(Function(F) F.ID_Entrada_Linea).ToList
                End If

        End Select


        Dim _Linea As Entrada_Linea
        Dim _LineaSeguent As Entrada_Linea
        For Each _Linea In _LlistatLinies
            If _Trobat = True Then
                _LineaSeguent = _Linea
                Exit For
            End If
            If _Linea.ID_Entrada_Linea = _IDATrobar Then
                _Trobat = True
            End If
        Next
        If _LineaSeguent Is Nothing = False Then
            Dim _TabActual As String = Me.Tab_Principal.SelectedTab.Key
            Call Netejar_Pantalla(True)
            oclsEntradaLinea = New clsEntradaLinea(noTocarEntrada, _LineaSeguent, oDTC)
            Call HabilitarCampsImportats(True)
            Me.T_Unidades.Enabled = True
            Me.TE_Producto_Codigo.ReadOnly = False
            Call CarregarDadesPredeterminadesDelDocument()
            Call Cargar_Form(_LineaSeguent)
            Call CarregarDadesPestanyes(_TabActual)
        End If
        Util.WaitFormTancar()
    End Sub

#End Region

#Region "Grid NS"

    Private Sub CargaGrid_NS(ByVal pId As Integer)
        Try

            With Me.GRD_NS
                .M.clsUltraGrid.Cargar(oclsEntradaLinea.RetornaNSDeLaLinea)
                'Els albarans de venta sempre tindran els botons afegir i eliminar desasctivats

                If pId <> 0 Then 'Si no hi ha cap linea d'entrada seleccionada no farem les següents coses
                    Call VisualitzacioPantalla(True)
                End If
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_NS_M_GRID_DoubleClickRow(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_NS.M_GRID_DoubleClickRow
        Call GRD_NS_M_ToolGrid_ToolEditar(Nothing, Nothing)
    End Sub

    Private Sub GRD_NS_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_NS.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_NS
                If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea = 0 Then
                    If Guardar() = False Then
                        Mensaje.Mostrar_Mensaje("Los datos de la linea no son correctos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If
                End If

                Dim _TextNS As String = "a"
                Do Until _TextNS Is Nothing
                    _TextNS = Mensaje.Mostrar_Entrada_Datos("Introduce la descripción del número de serie", "", False)

                    If _TextNS.Length = 0 Then
                        Exit Sub
                    End If

                    If oclsEntradaLinea.LineaNSCrear(, _TextNS) = True Then
                        oDTC.SubmitChanges()
                        Call CargaGrid_NS(oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea)
                        Me.T_Unidades.Value = oclsEntradaLinea.oLinqLinea.RecalculaCantidadNS
                    Else
                        Mensaje.Mostrar_Mensaje("Imposible añadir, el número de serie introducido ya existe", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    End If
                Loop


                'If oLinqEntrada_Linea.Producto.RequiereNumeroSerie = False Then
                '    Mensaje.Mostrar_Mensaje("Este producto no usa números de serie", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                '    Exit Sub
                'End If


                '.M_ExitEditMode()
                '.M_AfegirFila()

                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Almacen").Value = oLinqEntrada_Linea.Almacen
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("NS_Estado").Value = oDTC.NS_Estado.Where(Function(F) F.ID_NS_Estado = CInt(clsEntrada.RetornaEstatQueHauriaDeTenirElNSalCrearloSegonsPantalla(oEntradaTipo))).FirstOrDefault

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_NS_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_NS.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "SeleccionarNS" Then
            'si sóm un albarà de venta... i volem seleccionar NS's i no hi ha stock llavors ho impedirem. Si fos una devolución no faria falta mirar-ho ja que precisament lo normal es que no hi hagi stock
            If oclsEntradaLinea.oLinqEntrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranVenta AndAlso Me.T_StockActual.Value = 0 Then
                Mensaje.Mostrar_Mensaje("No hay stock de éste artículo para poder seleccionar los números de serie", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea = 0 Then
                If Guardar() = False Then
                    Mensaje.Mostrar_Mensaje("Los datos de la linea no son correctos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
            End If

            Dim frm As New frmEntrada_SeleccionNS
            frm.Entrada(oclsEntradaLinea, oDTC, Me.C_Almacen.Value)
            If frm.HiHaNumerosDeSerieDisponibles = False Then
                Mensaje.Mostrar_Mensaje("No hay números de serie disponibles", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                frm.FormTancar()
            Else
                frm.FormObrir(Me, True)
            End If
            AddHandler frm.AlTancarFormulariSeleccioNS, AddressOf AlTancarFormulariSeleccioNS

        End If
    End Sub

    Private Sub AlTancarFormulariSeleccioNS(ByVal pRecarregarDades As Boolean, ByRef pNSSeleccionats As ArrayList)
        If pRecarregarDades = True Then
            Call CargaGrid_NS(oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea)
            Me.T_Unidades.Value = oclsEntradaLinea.oLinqLinea.Entrada_Linea_NS.Count

            Select Case oclsEntradaLinea.oEntradaTipo
                Case EnumEntradaTipo.Regularizacion
                    'Això serveix per la regularització: Serà la quantitat d'stock real que hi ha menys la quantiat de números de serie seleccionats
                    Me.T_CantidadDeseada.Value = Me.T_StockReal.Value - Me.T_Unidades.Value
                Case EnumEntradaTipo.TraspasoAlmacen
                    'En el traspas de magatzem els números de serie seleccionats sera la quantitat que traspasem
                    Me.T_CantidadDeseada.Value = Me.T_Unidades.Value
            End Select
            oclsEntradaLinea.oLinqLinea.Unidad = Me.T_Unidades.Value
            oDTC.SubmitChanges()
        End If
    End Sub

    Private Sub GRD_NS_M_ToolGrid_ToolEditar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_NS.M_ToolGrid_ToolEditar
        Try
            If Me.GRD_NS.GRID.ActiveRow Is Nothing Then
                Exit Sub
            End If

            Dim IDLineaNS As Integer = Me.GRD_NS.GRID.ActiveRow.Cells("ID_Entrada_Linea_NS").Value
            Dim _LineaNS As Entrada_Linea_NS
            _LineaNS = oclsEntradaLinea.oLinqLinea.Entrada_Linea_NS.Where(Function(F) F.ID_Entrada_Linea_NS = IDLineaNS).FirstOrDefault

            Dim _TextNS As String
            _TextNS = Mensaje.Mostrar_Entrada_Datos("Introduce la descripción del número de serie", _LineaNS.NS.Descripcion, False)

            If _TextNS.Trim.Length = 0 Then
                Exit Sub
            End If

            If oclsEntradaLinea.LineaNSModificar(_LineaNS, _TextNS) = True Then
                oDTC.SubmitChanges()
                Call CargaGrid_NS(oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea)
            Else
                Mensaje.Mostrar_Mensaje("Imposible modificar el número de serie, este número de serie ya existe", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            End If


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_NS_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_NS.M_ToolGrid_ToolEliminarRow
        Try

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim IDLineaNS As Integer = e.Cells("ID_Entrada_Linea_NS").Value
                Dim _LineaNS As Entrada_Linea_NS
                _LineaNS = oclsEntradaLinea.oLinqLinea.Entrada_Linea_NS.Where(Function(F) F.ID_Entrada_Linea_NS = IDLineaNS).FirstOrDefault
                oclsEntradaLinea.LineaNSEliminar(_LineaNS)
                oDTC.SubmitChanges()
                Call CargaGrid_NS(oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea)
                Me.T_Unidades.Value = oclsEntradaLinea.oLinqLinea.RecalculaCantidadNS
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    '    Private Sub GRD_NS_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_NS.M_Grid_InitializeRow
    '        'Dim _NS As NS = e.Row.ListObject
    '        ''Imposible eliminar, el número de serie ya ha sido traspasado a un albarán
    '        '' If oDTC.Entrada_Linea_NumeroSerie.Where(Function(F) F.Entrada_Linea.ID_Entrada_Linea_Pedido = _NS.ID_Entrada_Linea And F.Descripcion = _NS.Descripcion).Count > 0 Then
    '        'If _NS.ID_NS_Estado = EnumNSEstado.NoDisponible Then
    '        '    e.Row.Cells("Descripcion").Activation = Activation.Disabled
    '        'End If
    '    End Sub

    '    Private Sub GRD_NS_M_GRID_BeforeCellUpdate(sender As Object, e As BeforeCellUpdateEventArgs) Handles GRD_NS.M_GRID_BeforeCellUpdate
    '        If e.Cell.Column.Key = "Descripcion" Then
    '            If e.NewValue.ToString.Length <> 0 Then

    '                Dim _Repetit As Boolean = False
    '                If e.Cell.Row.IsAddRow Then
    '                    'If _Linea.Entrada_Linea_NumeroSerie.Where(Function(F) F.Descripcion = e.NewValue).Count > 0 Then
    '                    '    _Repetit = True
    '                    'End If
    '                    If oDTC.NS.Where(Function(F) F.Descripcion = CStr(e.NewValue)).Count > 0 Then
    '                        'If oDTC.Entrada_Linea_NumeroSerie.Where(Function(F) F.Descripcion = e.NewValue And F.Entrada_Linea.ID_Producto = _Linea.ID_Producto).Count > 0 Then
    '                        _Repetit = True
    '                    End If
    '                Else
    '                    'Busca a veure si hi ha alguna descripcio igual que la recient introduida, entre els numeros de series de les línies de tots els albarans, només del producte seleccionat i descartant-se a si mateix
    '                    'If oDTC.Entrada_Linea_NumeroSerie.Where(Function(F) F.Descripcion = CStr(e.NewValue) And F.Entrada_Linea.Entrada.ID_Entrada_Tipo = CInt(EnumEntradaTipo.Albaran) And F.Entrada_Linea.ID_Producto = _Linea.ID_Producto And F.ID_Entrada_Linea_NumeroSerie <> CInt(e.Cell.Row.Cells("ID_Entrada_Linea_NumeroSerie").Value)).Count > 0 Then
    '                    If oDTC.NS.Where(Function(F) F.Descripcion = CStr(e.NewValue) And F.ID_NS <> CInt(e.Cell.Row.Cells("ID_NS").Value)).Count > 0 Then
    '                        _Repetit = True
    '                    End If

    '                    'If _Linea.Entrada_Linea_NumeroSerie.Where(Function(F) F.Descripcion = e.NewValue And F.ID_Entrada_Linea_NumeroSerie <> e.Cell.Row.Cells("ID_Entrada_Linea_NumeroSerie").Value).Count > 0 Then
    '                    '    _Repetit = True
    '                    'End If
    '                End If

    '                If _Repetit = True Then
    '                    Mensaje.Mostrar_Mensaje("Imposible introducir un mismo número de serie dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                    e.Cancel = True
    '                    Exit Sub
    '                Else
    '                    ''Modifiquem tots els números de serie relacionats
    '                    'Dim _llistatNSRelacionats As IQueryable(Of Entrada_Linea_NumeroSerie) = oDTC.Entrada_Linea_NumeroSerie.Where(Function(F) F.Entrada_Linea.ID_Producto = _Linea.ID_Producto And F.Descripcion = CStr(e.Cell.Value))
    '                    'Dim _NS As Entrada_Linea_NumeroSerie
    '                    'For Each _NS In _llistatNSRelacionats
    '                    '    _NS.Descripcion = e.NewValue
    '                    'Next
    '                    'oDTC.SubmitChanges()
    '                End If
    '            End If
    '        End If

    '    End Sub

    '    Private Sub GRD_NS_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_NS.M_GRID_AfterRowUpdate
    '        'Dim _Linea As Entrada_Linea = Me.GRD_Linea.GRID.ActiveRow.ListObject

    '        ''Amb aquestes dos línies aconseguim que s'actualitzi la quantitat
    '        'Me.GRD_Linea.GRID.ActiveRow.Cells("CantidadTraspasada").Value = Util.Comprobar_NULL_Per_0_Decimal(Me.GRD_Linea.GRID.ActiveRow.Cells("CantidadTraspasada").Value) + 1
    '        'Me.GRD_Linea.GRID.ActiveRow.Update()
    '        'Me.GRD_Linea.GRID.ActiveRow.Cells("CantidadTraspasada").Value = Me.GRD_Linea.GRID.ActiveRow.Cells("CantidadTraspasada").Value - 1
    '        'Me.GRD_Linea.GRID.ActiveRow.Update()

    '        Select Case oEntradaTipo
    '            Case EnumEntradaTipo.Albaran

    '                Dim _NS As NS
    '                _NS = e.Row.ListObject
    '                If _NS.ID_NS = 0 Then 'Si estem afegint una nova linea de Numero de serie també afegirem la linea Entrada_Linea_NS
    '                    Dim _Linea_NS As New Entrada_Linea_NS
    '                    _Linea_NS.NS = _NS
    '                    oLinqEntrada_Linea.Entrada_Linea_NS.Add(_Linea_NS)
    '                End If
    '        End Select


    '        oDTC.SubmitChanges()
    '        'Call CalcularCosteHoresRealitzades()
    '    End Sub

    '    Private Sub GRD_NS_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_NS.M_ToolGrid_ToolClickBotonsExtras
    '        If e.Tool.Key = "EliminarNS" Then
    '            If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
    '                Mensaje.Mostrar_Mensaje("Imposible modificar datos, la regularización esta cerrada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                Exit Sub
    '            End If
    '            'Dim _frmSeleccio As New frmEntradaRegularizacionSeleccion
    '            '_frmSeleccio.Entrada(oDTC, _Linea)
    '            '_frmSeleccio.FormObrir(Me, True)
    '        End If
    '    End Sub

#End Region

#Region "Grid TalYComoSeInstalo"

    Private Sub CargarGrid_TalYComoSeInstalo()
        Try

            Dim DT As DataTable = BD.RetornaDataTable("Select * From C_Propuesta_Linea Where Activo=1 and ID_Propuesta_Linea in (Select ID_Propuesta_Linea From Entrada_Linea_Propuesta_Linea Where ID_Entrada_Linea=" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea & ") Order By Descripcion", True)

            With Me.GRD_TalYComoSeInstalo
                .M.clsUltraGrid.Cargar(DT)
                .M_NoEditable()
            End With
            'Dim pRow As UltraGridRow
            'For Each pRow In Me.GRD_TalYComoSeInstalo.GRID.Rows

            '    'pRow.Cells("Seleccion").Value = True
            '    'pRow.CellAppearance.BackColor = Color.White
            '    'RowsSeleccionadas.Add(pRow)
            '    pRow.Update()
            'Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarFormTalYComoSeInstalo()
        Call CargarGrid_TalYComoSeInstalo()
        Call CalcularResum()
    End Sub

    Private Sub GRD_TalYComoSeInstalo_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_TalYComoSeInstalo.M_ToolGrid_ToolClickBotonsExtras
        Try

            Select Case e.Tool.Key
                Case "Asignar"
                    If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea = 0 Then
                        If Guardar() = False Then
                            Mensaje.Mostrar_Mensaje("Los datos de la linea no son correctos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                            Exit Sub
                        End If
                    End If
                    Dim frm As New frmEntrada_Linea_TalYComoSeInstalo
                    frm.Entrada(oclsEntradaLinea, oDTC)
                    frm.FormObrir(Me, False)
                    AddHandler frm.AlTancarForm, AddressOf AlTancarFormTalYComoSeInstalo
                Case "IrALaInstalacion"
                    Dim _IDInstalacion As Integer
                    If Me.GRD_TalYComoSeInstalo.GRID.ActiveRow Is Nothing = True Then
                        Exit Sub
                    End If
                    _IDInstalacion = Me.GRD_TalYComoSeInstalo.GRID.ActiveRow.Cells("ID_Instalacion").Value

                    Dim frm As New frmInstalacion
                    frm.Entrada(_IDInstalacion)
                    frm.FormObrir(Me, True)
            End Select
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Parte Horas"

    Private Sub CargarGrid_Partes()
        Try
            Dim DT As DataTable
            DT = BD.RetornaDataTable("Select * From C_Parte_Horas Where ID_Entrada_Linea=" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea & " Order By Fecha", True)

            With Me.GRD_Parte_Horas
                .M.clsUltraGrid.Cargar(DT)
                .M_NoEditable()

                If .GRID.Rows.Count > 0 Then
                    Util.TabPintarHeaderTab(Me.Tab_PartesTrabajo.Tabs("Horas"), System.Drawing.Color.Green)
                Else
                    Util.TabPintarHeaderTab(Me.Tab_PartesTrabajo.Tabs("Horas"), System.Drawing.Color.Gray)
                End If
            End With



            DT = BD.RetornaDataTable("Select * From C_Parte_Material Where ID_Entrada_Linea=" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea & " Order By Fecha", True)

            With Me.GRD_Parte_Material
                .M.clsUltraGrid.Cargar(DT)
                .M_NoEditable()
                If .GRID.Rows.Count > 0 Then
                    Util.TabPintarHeaderTab(Me.Tab_PartesTrabajo.Tabs("Material"), System.Drawing.Color.Green)
                Else
                    Util.TabPintarHeaderTab(Me.Tab_PartesTrabajo.Tabs("Material"), System.Drawing.Color.Gray)
                End If

            End With


            DT = BD.RetornaDataTable("Select * From C_Parte_Gastos Where ID_Entrada_Linea=" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea & " Order By Fecha", True)

            With Me.GRD_Parte_Gastos
                .M.clsUltraGrid.Cargar(DT)
                .M_NoEditable()
                If .GRID.Rows.Count > 0 Then
                    Util.TabPintarHeaderTab(Me.Tab_PartesTrabajo.Tabs("Gastos"), System.Drawing.Color.Green)
                Else
                    Util.TabPintarHeaderTab(Me.Tab_PartesTrabajo.Tabs("Gastos"), System.Drawing.Color.Gray)
                End If

            End With




            'Dim pRow As UltraGridRow
            'For Each pRow In Me.GRD_TalYComoSeInstalo.GRID.Rows

            '    'pRow.Cells("Seleccion").Value = True
            '    'pRow.CellAppearance.BackColor = Color.White
            '    'RowsSeleccionadas.Add(pRow)
            '    pRow.Update()
            'Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Parte_Gastos_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Parte_Gastos.M_ToolGrid_ToolClickBotonsExtras, GRD_Parte_Horas.M_ToolGrid_ToolClickBotonsExtras, GRD_Parte_Material.M_ToolGrid_ToolClickBotonsExtras
        Select Case e.Tool.Key
            Case "Asignar"
                If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea = 0 Then
                    If Guardar() = False Then
                        Mensaje.Mostrar_Mensaje("Los datos de la linea no son correctos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If
                End If

                Dim frm As New frmEntrada_Linea_Parte
                frm.Entrada(oclsEntradaLinea, oDTC, Me.Tab_PartesTrabajo.ActiveTab.Key)
                AddHandler frm.AlTancarForm, AddressOf AlTancarFormPartes
                frm.FormObrir(Me, False)

            Case "IrAlParte"
                Dim IDParte As Integer
                Select Case Me.Tab_PartesTrabajo.ActiveTab.Key
                    Case "Material"
                        If Me.GRD_Parte_Material.GRID.ActiveRow Is Nothing = True Then
                            Exit Sub
                        End If
                        IDParte = Me.GRD_Parte_Material.GRID.ActiveRow.Cells("ID_Parte").Value
                    Case "Horas"
                        If Me.GRD_Parte_Horas.GRID.ActiveRow Is Nothing = True Then
                            Exit Sub
                        End If
                        IDParte = Me.GRD_Parte_Horas.GRID.ActiveRow.Cells("ID_Parte").Value
                    Case "Gastos"
                        If Me.GRD_Parte_Gastos.GRID.ActiveRow Is Nothing = True Then
                            Exit Sub
                        End If
                        IDParte = Me.GRD_Parte_Gastos.GRID.ActiveRow.Cells("ID_Parte").Value
                End Select
                Dim frm As New frmParte
                frm.Entrada(IDParte)
                frm.FormObrir(Me, True)
        End Select


    End Sub

    Private Sub GRD_Parte_Gastos_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Parte_Gastos.M_ToolGrid_ToolVisualitzarDobleClickRow, GRD_Parte_Horas.M_ToolGrid_ToolVisualitzarDobleClickRow, GRD_Parte_Material.M_ToolGrid_ToolVisualitzarDobleClickRow
        'Call GRD_Parte_Gastos_M_ToolGrid_ToolClickBotonsExtras(Nothing, Nothing)
    End Sub

    Private Sub AlTancarFormPartes()
        Call CargarGrid_Partes()
        Call CalcularResum()
    End Sub

#End Region

#Region "Grid Precios Anteriores"

    Private Sub CargaGrid_PreciosAnteriores(ByVal pId As Integer)
        Try
            'Dim _Listado As IEnumerable = From taula In oDTC.Propuesta_Linea Where taula.ID_Producto = pId And taula.Propuesta.Instalacion.Cliente.ID_Cliente = oLinqInstalacion.ID_Cliente And taula.Activo = True And taula.ID_Propuesta <> oLinqPropuesta.ID_Propuesta Order By taula.Propuesta.Fecha Descending Select taula.ID_Propuesta_Linea, taula.Propuesta.Instalacion.ID_Instalacion, taula.Propuesta.Codigo, taula.Propuesta.Fecha, taula.Producto.Descripcion, Emplazamiento = taula.Instalacion_Emplazamiento.Descripcion, Planta = taula.Instalacion_Emplazamiento_Planta.Descripcion, Zona = taula.Instalacion_Emplazamiento_Zona.Descripcion, ElementosAProteger = taula.Instalacion_ElementosAProteger.Instalacion_ElementosAProteger_Tipo.Descripcion, Abertura = taula.Instalacion_Emplazamiento_Abertura.Descripcion_Detallada, taula.Unidad, taula.Precio, taula.Descuento, TotalUnitario = taula.Precio - taula.Precio * taula.Descuento
            Dim _Listado As IEnumerable
            If oclsEntradaLinea.oLinqEntrada.ID_Cliente.HasValue = True Then
                _Listado = From Taula In oDTC.Entrada_Linea Where Taula.ID_Producto = pId And Taula.Entrada.ID_Cliente = oclsEntradaLinea.oLinqEntrada.ID_Cliente And (Taula.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.PedidoVenta Or Taula.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranVenta) And Taula.ID_Entrada_Linea <> oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea Order By Taula.Entrada.FechaEntrada Descending Select Taula.ID_Entrada_Linea, Taula.ID_Entrada, Taula.Entrada.Codigo, Taula.Entrada.Entrada_Tipo.Descripcion, Taula.Entrada.ID_Entrada_Tipo, Taula.Entrada.FechaEntrada, Taula.Unidad, Taula.Precio, Taula.Descuento1, Taula.Descuento2, Taula.TotalBase
            Else
                _Listado = From Taula In oDTC.Entrada_Linea Where Taula.ID_Producto = pId And Taula.Entrada.ID_Proveedor = oclsEntradaLinea.oLinqEntrada.ID_Proveedor And (Taula.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.PedidoCompra Or Taula.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranCompra) And Taula.ID_Entrada_Linea <> oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea Order By Taula.Entrada.FechaEntrada Descending Select Taula.ID_Entrada_Linea, Taula.ID_Entrada, Taula.Entrada.Codigo, Taula.Entrada.Entrada_Tipo.Descripcion, Taula.Entrada.ID_Entrada_Tipo, Taula.Entrada.FechaEntrada, Taula.Unidad, Taula.Precio, Taula.Descuento1, Taula.Descuento2, Taula.TotalBase
            End If

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

                'Comentem el text d'abaix pq la comprovació la fem amb la carga de preciosanteriores_propuesta
                'If .GRID.Rows.Count > 0 Then
                '    Util.TabPintarHeaderTab(Me.Tab_Principal.Tabs("PreciosAnteriores"), Color.Green)
                'Else
                '    Util.TabPintarHeaderTab(Me.Tab_Principal.Tabs("PreciosAnteriores"), Me.Tab_Principal.Tabs(0).Appearance.BackColor)
                'End If
            End With



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PreciosAnteriores_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_PreciosAnteriores.M_GRID_DoubleClickRow2
        Dim _IDTipusDocument As EnumEntradaTipo
        _IDTipusDocument = e.Cells("ID_Entrada_Tipo").Value
        Dim _IDDocument As Integer = e.Cells("ID_Entrada").Value
        Dim frm As New frmEntrada
        frm.Entrada(_IDDocument, _IDTipusDocument)
        frm.FormObrir(Me, True)
    End Sub

#End Region

#Region "Grid Precios Anteriores propuestas"

    Private Sub CargaGrid_PreciosAnteriores_propuesta(ByVal pId As Integer)
        Try

            Dim _Listado As IEnumerable = From taula In oDTC.Propuesta_Linea Where taula.ID_Producto = pId And taula.Propuesta.Instalacion.Cliente.ID_Cliente = oclsEntradaLinea.oLinqEntrada.ID_Cliente And taula.Activo = True Order By taula.Propuesta.Fecha Descending Select taula.ID_Propuesta, taula.ID_Propuesta_Linea, taula.Propuesta.Instalacion.ID_Instalacion, taula.Propuesta.Codigo, taula.Propuesta.Fecha, taula.Producto.Descripcion, Emplazamiento = taula.Instalacion_Emplazamiento.Descripcion, Planta = taula.Instalacion_Emplazamiento_Planta.Descripcion, Zona = taula.Instalacion_Emplazamiento_Zona.Descripcion, ElementosAProteger = taula.Instalacion_ElementosAProteger.Instalacion_ElementosAProteger_Tipo.Descripcion, Abertura = taula.Instalacion_Emplazamiento_Abertura.Descripcion_Detallada, taula.Unidad, taula.Precio, taula.Descuento, TotalUnitario = taula.Precio - ((taula.Precio * taula.Descuento) / 100)

            With Me.GRD_PreciosAnteriores_Propuesta
                '.GRID.DataSource = _Listado
                .M.clsUltraGrid.CargarIEnumerable(_Listado)

                '.GRID.DisplayLayout.Bands(0).Columns("ID_").CellActivation = UltraWinGrid.Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect

                'Dim pCol As Infragistics.Win.UltraWinGrid.UltraGridColumn
                'For Each pCol In .GRID.DisplayLayout.Bands(0).Columns
                '    pCol.PerformAutoResize()
                'Next

                If .GRID.Rows.Count > 0 Or Me.GRD_PreciosAnteriores.GRID.Rows.Count > 0 Then
                    Util.TabPintarHeaderTab(Me.Tab_Principal.Tabs("PreciosAnteriores"), Color.Green)
                Else
                    Util.TabPintarHeaderTab(Me.Tab_Principal.Tabs("PreciosAnteriores"), Me.Tab_Principal.Tabs(0).Appearance.BackColor)
                End If

            End With



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PreciosAnteriores_propuestas_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_PreciosAnteriores_Propuesta.M_GRID_DoubleClickRow2
        With Me.GRD_PreciosAnteriores_Propuesta
            If .GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = CInt(.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value)).FirstOrDefault
            If _Propuesta Is Nothing = False Then
                Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                frm.Entrada(_Propuesta.ID_Instalacion)
                frm.FormObrir(frmPrincipal, True)

                Dim frm2 As New frmPropuesta
                frm2.Entrada(frm.oLinqInstalacion, frm.oDTC, _Propuesta.ID_Propuesta)
                AddHandler frm2.FormClosing, AddressOf frm.AlTancarfrmPropuesta
                frm2.FormObrir(frm)
            End If
        End With
        'Dim _IDTipusDocument As EnumEntradaTipo
        '_IDTipusDocument = e.Cells("ID_Entrada_Tipo").Value
        'Dim _IDDocument As Integer = e.Cells("ID_Entrada").Value
        'Dim frm As New frmEntrada
        'frm.Entrada(_IDDocument, _IDTipusDocument)
        'frm.FormObrir(Me, True)
    End Sub

#End Region

#Region "Grid Compuesto Por"

    Private Sub CargaGrid_CompuestoPor()
        Try

            With Me.GRD_CompuestoPor
                Dim DTS As New DataSet

                BD.CargarDataSet(DTS, "Select *, Coste*Unidad as TotalCosteProductos From C_Entrada_Linea Where   ID_Entrada_Linea_Padre=" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea)
                BD.CargarDataSet(DTS, "Select Entrada_Linea.ID_Entrada_Linea, NS.Descripcion, NS.Virtual From NS, Entrada_Linea_NS, Entrada_Linea Where NS.ID_NS=Entrada_Linea_NS.ID_NS and Entrada_Linea_NS.ID_Entrada_Linea=Entrada_Linea.ID_Entrada_Linea and ID_Entrada_Linea_Padre=" & oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea, "aaaaa", 0, "ID_Entrada_Linea", "ID_Entrada_Linea", True)

                .M.clsUltraGrid.Cargar(DTS)

                .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
                Call PosarEditableColumnes(0, "Coste")

                Call CalcularResum()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub PosarEditableColumnes(ByVal pIDBanda As Integer, ByVal pNomColumna As String)
        Try
            Me.GRD_CompuestoPor.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellActivation = Activation.AllowEdit
            Me.GRD_CompuestoPor.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellClickAction = CellClickAction.EditAndSelectText
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CompuestoPor_M_GRID_AfterCellUpdate(sender As Object, e As CellEventArgs) Handles GRD_CompuestoPor.M_GRID_AfterCellUpdate

        Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(e.Cell.Row.Cells("ID_Entrada_Linea").Value)).FirstOrDefault


        Select Case e.Cell.Column.Key
            Case "Coste"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Coste = Nothing
                Else
                    _Linea.Coste = e.Cell.Value
                End If
                e.Cell.Row.Cells("TotalCosteProductos").Value = _Linea.Coste * _Linea.Unidad
                oDTC.SubmitChanges()
                Call CalcularResum()
        End Select


    End Sub

    Private Sub GRD_CompuestoPor_M_ToolGrid_ToolAfegir(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_CompuestoPor.M_ToolGrid_ToolAfegir
        Try
            If oclsEntradaLinea.oLinqLinea.ID_Entrada_Linea = 0 Then
                If Guardar() = False Then
                    Mensaje.Mostrar_Mensaje("Los datos de la linea no son correctos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
            End If

            Dim frm As New frmEntrada_Linea_CompuestoPor
            frm.Entrada(oclsEntradaLinea, oDTC)
            frm.FormObrir(Me, False)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormCompuestoPor

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_CompuestoPor_M_ToolGrid_ToolEditar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_CompuestoPor.M_ToolGrid_ToolEditar

        Exit Sub






        If Me.GRD_CompuestoPor.GRID.ActiveRow Is Nothing Then
            Exit Sub
        End If

        Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(Me.GRD_CompuestoPor.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
        Dim frm As New frmEntrada_Linea_CompuestoPor
        frm.Entrada(oclsEntradaLinea, oDTC, _Linea)
        frm.FormObrir(Me, False)
        AddHandler frm.AlTancarForm, AddressOf AlTancarFormCompuestoPor


    End Sub

    Private Sub GRD_CompuestoPor_M_ToolGrid_ToolEliminar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_CompuestoPor.M_ToolGrid_ToolEliminar
        If Me.GRD_CompuestoPor.GRID.ActiveRow Is Nothing Then
            Exit Sub
        End If

        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then

            Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(Me.GRD_CompuestoPor.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
            Dim _ClsEntradaLinea As New clsEntradaLinea(oclsEntradaLinea.oLinqEntrada, _Linea, oDTC)
            _ClsEntradaLinea.EliminarLinea()
            'oDTC.Entrada_Linea.DeleteOnSubmit(_Linea)
            oDTC.SubmitChanges()
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            Call CargaGrid_CompuestoPor()
        End If
    End Sub

    Private Sub GRD_CompuestoPor_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_CompuestoPor.M_ToolGrid_ToolVisualitzarDobleClickRow
        Call GRD_CompuestoPor_M_ToolGrid_ToolEditar(Nothing, Nothing)
    End Sub

    Private Sub AlTancarFormCompuestoPor()
        Call CargaGrid_CompuestoPor()
    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_CompuestoPor.M_ToolGrid_ToolClickBotonsExtras
        Try
            Select Case e.Tool.Key
                Case "VerProducto"
                    With Me.GRD_CompuestoPor
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Band.Index <> 0 Then
                            Exit Sub
                        End If

                        Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
                        frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
                        frm.FormObrir(frmPrincipal, True)
                    End With

                Case "VerAlmacen"
                    With Me.GRD_CompuestoPor
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Band.Index <> 0 Then
                            Exit Sub
                        End If

                        Dim frm As New frmAlmacen
                        frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Almacen").Value)
                        frm.FormObrir(frmPrincipal, True)
                    End With

                Case "VerTrazabilidad"
                    With Me.GRD_CompuestoPor
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Band.Index <> 0 Then
                            Exit Sub
                        End If

                        Dim frm As New frmProducto_Trazabilidad
                        frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
                        frm.FormObrir(Me, True)
                    End With

                Case "VisualizarFotos"
                    With Me.GRD_CompuestoPor
                        'If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
                        '    .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "Visualizar fotos"
                        'Else
                        '    .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos"
                        'End If
                        Call CargaGrid_CompuestoPor()
                    End With
            End Select

        Catch ex As Exception
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm()
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Lineas_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_CompuestoPor.M_Grid_InitializeRow
        With Me.GRD_CompuestoPor
            If e.Row.Band.Index = 0 Then
                '  If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
                Dim _Linea As Entrada_Linea
                _Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(e.Row.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                If _Linea.Producto.Archivo Is Nothing = False AndAlso _Linea.Producto.Archivo.CampoBinario.Length > 0 Then
                    e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(_Linea.Producto.Archivo.CampoBinario.ToArray)
                End If
                .GRID.DisplayLayout.Override.DefaultRowHeight = 40
                ' Else
                .GRID.DisplayLayout.Override.DefaultRowHeight = 20
            End If
            ' End If
        End With
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