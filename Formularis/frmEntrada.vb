Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEntrada
    Implements IDisposable


    Dim oDTC As DTCDataContext
    Dim oLinqEntrada As Entrada
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2
    Dim oEntradaTipo As EnumEntradaTipo
    Public _BotoAuxiliar As String = ""

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqEntrada.ID_Almacen <> 0 Then
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then

                    Select Case oEntradaTipo
                        Case EnumEntradaTipo.PedidoCompra, EnumEntradaTipo.PedidoVenta
                            If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                                Mensaje.Mostrar_Mensaje("Imposible eliminar un pedido en estado cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                                Exit Sub
                            End If

                            If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Parcial Then
                                Mensaje.Mostrar_Mensaje("Imposible eliminar un pedido en estado parcial", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                                Exit Sub
                            End If

                            Dim _clsEntrada As New clsEntrada(oDTC, oLinqEntrada)
                            If _clsEntrada.Eliminar = True Then
                                Call Netejar_Pantalla()
                                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                            End If
                        Case EnumEntradaTipo.AlbaranCompra, EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.DevolucionVenta, EnumEntradaTipo.DevolucionCompra
                            If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                                Mensaje.Mostrar_Mensaje("Imposible eliminar un documeto en estado cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                                Exit Sub
                            End If

                            Dim _clsEntrada As New clsEntrada(oDTC, oLinqEntrada)
                            If _clsEntrada.Eliminar = True Then
                                Call Netejar_Pantalla()
                                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                            End If



                        Case EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaVentaRectificativa, EnumEntradaTipo.FacturaCompraRectificativa
                            Dim _clsEntrada As New clsEntrada(oDTC, oLinqEntrada)
                            If _clsEntrada.Eliminar = True Then
                                Call Netejar_Pantalla()
                                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                            End If

                        Case EnumEntradaTipo.Inicializacion
                            If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                                Mensaje.Mostrar_Mensaje("Imposible eliminar un inicialización en estado cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                                Exit Sub
                            End If
                            Dim _clsEntrada As New clsEntrada(oDTC, oLinqEntrada)
                            Me.GRD_Linea.GRID.DataSource = Nothing  'Fem això pq peta el active row no se pq, 
                            If _clsEntrada.Eliminar = True Then
                                Call Netejar_Pantalla()
                                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                            End If

                        Case EnumEntradaTipo.FacturaVenta
                            Mensaje.Mostrar_Mensaje("Imposible eliminar una factura de venta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                            Exit Sub

                        Case EnumEntradaTipo.Regularizacion, EnumEntradaTipo.TraspasoAlmacen
                            Mensaje.Mostrar_Mensaje("Imposible eliminar este tipo de documento", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                            Exit Sub

                    End Select

                    'Call Guardar()

                    ' oDTC.SubmitChanges()
                    ' Call Netejar_Pantalla()
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

    Private Sub ToolForm_m_ToolForm_ToolClick_Botones_Extras(Sender As Object, e As UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        Select Case e.Tool.Key
            Case "Albaranar"
                If Guardar() = False Then
                    Exit Sub
                End If

                If oLinqEntrada.Entrada_Linea.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible albaranar un pedido sin líneas", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                    Mensaje.Mostrar_Mensaje("Imposible albaranar un pedido cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada <> 0 Then
                    If clsEntrada.ComprovacioQuantitatLineaAmbNumeroSerie(oLinqEntrada.ID_Entrada) = False Then
                        Mensaje.Mostrar_Mensaje("Imposible traspasar, hay una o más líneas con una cantidad diferente a los números de serie introducidos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    Dim frm As New frmEntradaTraspasoPedidoAlbaranCompra
                    frm.Entrada(oLinqEntrada, oDTC)
                    AddHandler frm.AlTancarFormulariTraspas, AddressOf AlTancarFormTraspas
                    frm.FormObrir(Me, False)
                End If


            Case "Facturar"
                If Guardar() = False Then
                    Exit Sub
                End If

                If oLinqEntrada.Entrada_Linea.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible facturar un albarán sin líneas", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                    Mensaje.Mostrar_Mensaje("Imposible facturar un albaran cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada <> 0 Then
                    If clsEntrada.ComprovacioQuantitatLineaAmbNumeroSerie(oLinqEntrada.ID_Entrada) = False Then
                        Mensaje.Mostrar_Mensaje("Imposible facturar, hay una o más líneas con una cantidad diferente a los números de serie introducidos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    Dim _frm As New frmEntradaTraspasoAlbaranAFactura
                    _frm.Entrada(oLinqEntrada, oDTC)
                    _frm.FormObrir(Me, False)
                    AddHandler _frm.AlTancarFormulariTraspas, AddressOf AlTancarFormTraspas

                End If

            Case "FacturarVenta"
                If Guardar() = False Then
                    Exit Sub
                End If

                If oLinqEntrada.Entrada_Linea.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible facturar un albarán sin líneas", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                    Mensaje.Mostrar_Mensaje("Imposible facturar un albaran cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada <> 0 Then
                    If clsEntrada.ComprovacioQuantitatLineaAmbNumeroSerie(oLinqEntrada.ID_Entrada) = False Then
                        Mensaje.Mostrar_Mensaje("Imposible facturar, hay una o más líneas con una cantidad diferente a los números de serie introducidos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    Dim _frm As New frmEntradaTraspasoAlbaranAFactura
                    _frm.Entrada(oLinqEntrada, oDTC)
                    _frm.FormObrir(Me, False)
                    AddHandler _frm.AlTancarFormulariTraspas, AddressOf AlTancarFormTraspas

                End If

            Case "AplicarInicializacion"
                If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                    Mensaje.Mostrar_Mensaje("Imposible inicializar, el albarán ya ha sido inicializado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If oLinqEntrada.Entrada_Linea.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción, no hay nada a inicializar", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Desea aplicar la inicialización d'stocks en el almacén seleccionado?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Util.WaitFormObrir()
                Dim _ArticlesAInicialitzar As IEnumerable(Of Entrada_Linea) = oDTC.Entrada_Linea.Where(Function(F) F.StockActivo = True And F.ID_Almacen = CInt(Me.C_Almacen.Value))
                Dim _Linea As Entrada_Linea
                For Each _Linea In _ArticlesAInicialitzar
                    'Les comandes en estat pendent no s'han de posar stockactivo=False
                    If _Linea.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.PedidoCompra And _Linea.Entrada.ID_Entrada_Estado = EnumEntradaEstado.Pendiente Then
                    Else
                        _Linea.StockActivo = False
                        _Linea.ID_DocumentoInicializacionStocks = oLinqEntrada.ID_Entrada
                    End If

                Next

                For Each _Linea In oLinqEntrada.Entrada_Linea
                    _Linea.StockActivo = True
                    _Linea.ID_DocumentoInicializacionStocks = Nothing
                Next

                oLinqEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault

                oDTC.SubmitChanges()

                Call Cargar_Form(oLinqEntrada.ID_Entrada)
                Mensaje.Mostrar_Mensaje("Inicialización aplicada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                Util.WaitFormTancar()

            Case "AlbaranarVenta"
                If Guardar() = False Then
                    Exit Sub
                End If

                If oLinqEntrada.Entrada_Linea.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible albaranar un pedido sin líneas", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                    Mensaje.Mostrar_Mensaje("Imposible albaranar un pedido cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada <> 0 Then
                    If clsEntrada.ComprovacioQuantitatLineaAmbNumeroSerie(oLinqEntrada.ID_Entrada) = False Then
                        Mensaje.Mostrar_Mensaje("Imposible traspasar, hay una o más líneas con una cantidad diferente a los números de serie introducidos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    Dim oclsEntrada As New clsEntrada(oDTC, oLinqEntrada)

                    Dim frm As New frmEntradaTraspasoPedidoAlbaranVenta
                    frm.Entrada(oclsEntrada, oDTC)
                    AddHandler frm.AlTancarFormulariTraspas, AddressOf AlTancarFormTraspas
                    frm.FormObrir(Me, False)
                End If

            Case "CerrarFactura"
                If Guardar() = False Then
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción la factura ya esta cerrada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Desea cerrar el pedido?", M_Mensaje.Missatge_Modo.PREGUNTA, "Cerrar pedido") = M_Mensaje.Botons.SI Then
                    Me.C_Estado.Value = CInt(EnumEntradaEstado.Cerrado)
                    oLinqEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Cerrado)).FirstOrDefault
                    oDTC.SubmitChanges()
                    Mensaje.Mostrar_Mensaje("Factura cerrada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                End If
            Case "CerrarDocumentoRegularizacion"
                If Guardar() = False Then
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción la regularización ya esta cerrada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                Me.C_Estado.Value = CInt(EnumEntradaEstado.Cerrado)
                oLinqEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Cerrado)).FirstOrDefault
                oDTC.SubmitChanges()
                Call Cargar_Form(oLinqEntrada.ID_Entrada)
                Mensaje.Mostrar_Mensaje("Regularización cerrada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)

            Case "AnularPedido"
                If Guardar() = False Then
                    Exit Sub
                End If


                If oLinqEntrada.ID_Entrada_Estado <> EnumEntradaEstado.Pendiente Then
                    Mensaje.Mostrar_Mensaje("Imposible anular el pedido, sólo se pueden anular los pedidos en estado pendiente", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción el pedido ya esta cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Desea anular el pedido?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Me.C_Estado.Value = CInt(EnumEntradaEstado.Cerrado)
                Me.CH_DocumentoAnulado.Checked = True
                oLinqEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(EnumEntradaEstado.Cerrado)).FirstOrDefault
                oLinqEntrada.Anulado = True
                'Me.CH_DocumentoAnulado.Visible = True
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje("Pedido anulado", M_Mensaje.Missatge_Modo.INFORMACIO)

                'Si te un pressupost associat l'intentarem pasar a negatiu
                If oLinqEntrada.ID_Propuesta.HasValue = True Then
                    Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = oLinqEntrada.ID_Propuesta).FirstOrDefault
                    If DirectCast(_Propuesta.ID_Propuesta_Estado, EnumPropuestaEstado) = EnumPropuestaEstado.Aprobado Then
                        clsPropuesta.PasarLaPropuestaANegativaoPositiva(oDTC, _Propuesta.ID_Propuesta, True)
                    End If
                End If

                'If oLinqEntrada.ID_Propuesta.HasValue = True Then
                '    Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = oLinqEntrada.ID_Propuesta).FirstOrDefault
                '    If _Propuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Aprobado Then

                '    End If
                'End If

                Call Cargar_Form(oLinqEntrada.ID_Entrada)
        End Select
    End Sub

    Private Sub ToolForm_m_ToolForm_Imprimir() Handles ToolForm.m_ToolForm_Imprimir
        If Guardar() = False Then
            Exit Sub
        End If

        Me.CH_DocumentoImpreso.Checked = True
        oLinqEntrada.DocumentoImpreso = True
        oDTC.SubmitChanges()


        Dim _TipusInforme As EnumInforme
        '_TipusInforme = CType(CInt(oEntradaTipo) + 10, EnumInforme)
        Select Case oEntradaTipo
            Case EnumEntradaTipo.PedidoCompra
                _TipusInforme = EnumInforme.PedidoCompra
            Case EnumEntradaTipo.AlbaranCompra
                _TipusInforme = EnumInforme.AlbaranCompra
            Case EnumEntradaTipo.FacturaCompra
                _TipusInforme = EnumInforme.FacturaCompra
            Case EnumEntradaTipo.PedidoVenta
                _TipusInforme = EnumInforme.PedidoVenta
            Case EnumEntradaTipo.AlbaranVenta
                _TipusInforme = EnumInforme.AlbaranVenta
            Case EnumEntradaTipo.FacturaVenta
                _TipusInforme = EnumInforme.FacturaVenta
            Case EnumEntradaTipo.TraspasoAlmacen
                _TipusInforme = EnumInforme.TraspasoAlmacen
            Case EnumEntradaTipo.Regularizacion
                _TipusInforme = EnumInforme.Regularizacion
            Case EnumEntradaTipo.FacturaCompraRectificativa
                _TipusInforme = EnumInforme.FacturaCompraRectificativa
            Case EnumEntradaTipo.FacturaVentaRectificativa
                _TipusInforme = EnumInforme.FacturaVentaRectificativa
            Case EnumEntradaTipo.DevolucionVenta
                _TipusInforme = EnumInforme.DevolucionVenta
            Case EnumEntradaTipo.DevolucionCompra
                _TipusInforme = EnumInforme.DevolucionCompra
        End Select


        Informes.ObrirInformePreparacio(oDTC, _TipusInforme, "[ID_Entrada] = " & oLinqEntrada.ID_Entrada, RetornaTextTipusDocument(oLinqEntrada.Codigo))

        'Dim frm As New frmInformePreparacio
        'frm.Entrada(EnumInforme.ParteTrabajo, "[ID_Parte] = " & oLinqParte.ID_Parte)
        'frm.FormObrir(Me, False)
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByVal pId As Integer, Optional ByVal pEntradaTipo As EnumEntradaTipo = EnumEntradaTipo.NoEspecificado)
        Try
            If pEntradaTipo = EnumEntradaTipo.NoEspecificado Then
                oEntradaTipo = oDTC.Entrada.Where(Function(F) F.ID_Entrada = pId).FirstOrDefault.ID_Entrada_Tipo
            Else
                oEntradaTipo = pEntradaTipo
            End If

            If Me.C_Estado.Items.Count = 0 Then 'Fem això per saber si la pantalla ja ha estat carregada en modo caché
                Me.AplicarDisseny()

                Me.CH_DocumentoAnulado.Visible = False
                Me.ToolForm.M.clsToolBar.Afegir_Boto("Albaranar", "Albaranar", False)
                Me.ToolForm.M.clsToolBar.Afegir_Boto("Facturar", "Facturar", False)
                Me.ToolForm.M.clsToolBar.Afegir_Boto("AnularPedido", "Anular pedido", False)

                Me.GRD_Linea.M.clsToolBar.Boto_Afegir("VerProducto", "Ver ficha del producto", True)
                Me.GRD_Linea.M.clsToolBar.Boto_Afegir("VerAlmacen", "Ver almacén", True)
                Me.GRD_Linea.M.clsToolBar.Boto_Afegir("VerTrazabilidad", "Ver trazabilidad del producto", True)
                Me.GRD_Linea.M.clsToolBar.Boto_Afegir("VisualizarFotos", "Visualizar fotos", True)

                Me.GRD_Instalacion.M.clsToolBar.Boto_Afegir("IrAInstalacion", "Ir a Instalación", True)
                Me.GRD_Partes.M.clsToolBar.Boto_Afegir("IrAParte", "Ir al parte", True)
                Me.GRD_Propuesta.M.clsToolBar.Boto_Afegir("IrAPropuesta", "Ir a la propuesta", True)
                'Me.GRD_Linea.M.clsToolBar.Boto_Afegir("RecuperarDatos", "Recuperar datos", False)

                Me.ToolForm.M.Botons.tAccions.SharedProps.Visible = True

                Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("AplicarInicializacion", "Aplicar inicialización", False)
                Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("AlbaranarVenta", "Albaranar", False)
                Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("FacturarVenta", "Facturar", False)
                Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("CerrarDocumentoRegularizacion", "Cerrar documento", False)

                Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True
                Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Entrada_Archivo", 1)
                Util.Cargar_Combo(Me.C_TipoEntrada, "Select ID_Entrada_Tipo, Descripcion From Entrada_Tipo Order by Descripcion", True, False)
                Util.Cargar_Combo(Me.C_Estado, "Select ID_Entrada_Estado, Descripcion From Entrada_Estado Order by Descripcion", True, False)
                Util.Cargar_Combo(Me.C_Almacen, "Select ID_Almacen, Descripcion From Almacen Where Activo=1 Order by Descripcion", True, False)
                Util.Cargar_Combo(Me.C_Almacen_Destino, "Select ID_Almacen, Descripcion From Almacen Where Activo=1 Order by Descripcion", True, False)
                'Util.Cargar_Combo(Me.C_Almacen_Destino, "SELECT Almacen.ID_Almacen, Almacen.Descripcion FROM  Almacen LEFT OUTER JOIN  Parte ON Almacen.ID_Parte = dbo.Parte.ID_Parte WHERE (Parte.ID_Parte_Estado <> 3) Order by Descripcion", True, False)
                Util.Cargar_Combo(Me.C_Origen, "Select ID_Entrada_Origen, Descripcion From Entrada_Origen  Order by Descripcion", True, False)
                Util.Cargar_Combo(Me.C_CompañiaTransporte, "Select ID_CompañiaTransporte, Descripcion From CompañiaTransporte Order by Descripcion", False, False)
                Util.Cargar_Combo(Me.C_Comercial, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and ID_Personal_Tipo=" & EnumPersonalTipo.Comercial & " ORDER BY Nombre", False, True, "No asignado")
                Util.Cargar_Combo(Me.C_Campaña, "Select ID_Campaña, Descripcion From Campaña Order by Descripcion ")
                Util.Cargar_Combo(Me.C_FormaPago, "SELECT ID_FormaPago, Descripcion FROM FormaPago Where Activo=1 ORDER BY Codigo", False)
                Me.C_Facturable.Items.Add(1, "Si")
                Me.C_Facturable.Items.Add(0, "No")

                Util.Cargar_Combo(Me.C_Empresa, "Select ID_Empresa, NombreComercial From Empresa Where Activo=1  Order by NombreComercial ", False)


                'Carregarem 31 dias de la setmana al combo
                Dim i As Integer
                For i = 1 To 31
                    Me.C_DiaPago.Items.Add(i, i)
                Next
            End If

            Me.Preview_RTF.pBotoGuardarVisible = True

            Me.ToolForm.ToolForm.Tools("Albaranar").SharedProps.Visible = False
            Me.ToolForm.ToolForm.Tools("Facturar").SharedProps.Visible = False
            'Me.GRD_Linea.ToolGrid.Tools("RecuperarDatos").SharedProps.Visible = False
            Me.ToolForm.ToolForm.Tools("AplicarInicializacion").SharedProps.Visible = False
            Me.ToolForm.ToolForm.Tools("AlbaranarVenta").SharedProps.Visible = False
            Me.ToolForm.ToolForm.Tools("FacturarVenta").SharedProps.Visible = False
            Me.ToolForm.ToolForm.Tools("CerrarDocumentoRegularizacion").SharedProps.Visible = False
            Me.Pan_Facturable.Visible = False
            Me.Pan_Vencimientos.Visible = False
            Me.L_NumDocumentoProveedor.Visible = False
            Me.T_NumDocumentoProveedor.Visible = False
            Select Case pEntradaTipo
                Case EnumEntradaTipo.PedidoCompra
                    Me.AccessibleName = "PedidoCompra"
                    Me.ToolForm.ToolForm.Tools("Albaranar").SharedProps.Visible = True
                    Me.L_NumDocumentoProveedor.Visible = True
                    Me.T_NumDocumentoProveedor.Visible = True
                    Me.GRD_Linea.M.clsToolBar.Boto_Afegir("IrAlAlbaran", "Ir al albarán", True)
                Case EnumEntradaTipo.AlbaranCompra
                    Me.AccessibleName = "AlbaranCompra"
                    Me.ToolForm.ToolForm.Tools("Facturar").SharedProps.Visible = True
                    Me.L_NumDocumentoProveedor.Visible = True
                    Me.T_NumDocumentoProveedor.Visible = True
                    Me.GRD_Linea.M.clsToolBar.Boto_Afegir("IrAFactura", "Ir a la factura", True)
                Case EnumEntradaTipo.FacturaCompra
                    Me.AccessibleName = "FacturaCompra"
                    Me.L_NumDocumentoProveedor.Visible = True
                    Me.T_NumDocumentoProveedor.Visible = True
                    Me.Pan_Vencimientos.Visible = True
                Case EnumEntradaTipo.FacturaCompraRectificativa
                    Me.AccessibleName = "FacturaRectificativaCompra"
                    Me.L_NumDocumentoProveedor.Visible = True
                    Me.T_NumDocumentoProveedor.Visible = True
                    Me.Pan_Vencimientos.Visible = True
                Case EnumEntradaTipo.Regularizacion
                    Me.AccessibleName = "Regularizacion"
                    Me.ToolForm.ToolForm.Tools("CerrarDocumentoRegularizacion").SharedProps.Visible = True
                Case EnumEntradaTipo.DevolucionCompra
                    Me.AccessibleName = "Devolucion"
                    Me.L_NumDocumentoProveedor.Visible = True
                    Me.T_NumDocumentoProveedor.Visible = True
                    Me.GRD_Linea.M.clsToolBar.Boto_Afegir("IrAFactura", "Ir a la factura", True)
                    Me.ToolForm.ToolForm.Tools("Facturar").SharedProps.Visible = True
                Case EnumEntradaTipo.DevolucionVenta
                    Me.AccessibleName = "DevolucionVenta"
                    Me.GRD_Linea.M.clsToolBar.Boto_Afegir("IrAFactura", "Ir a la factura", True)
                    Me.ToolForm.ToolForm.Tools("Facturar").SharedProps.Visible = True
                Case EnumEntradaTipo.Inicializacion
                    Me.AccessibleName = "Inicializacion"
                    ' Me.GRD_Linea.ToolGrid.Tools("RecuperarDatos").SharedProps.Visible = True
                    Me.ToolForm.ToolForm.Tools("AplicarInicializacion").SharedProps.Visible = True
                Case EnumEntradaTipo.PedidoVenta
                    Me.AccessibleName = "PedidoVenta"
                    Me.ToolForm.ToolForm.Tools("AlbaranarVenta").SharedProps.Visible = True
                    Me.GRD_Linea.M.clsToolBar.Boto_Afegir("IrAlAlbaran", "Ir al albarán", True)
                    Me.ToolForm.ToolForm.Tools("AnularPedido").SharedProps.Visible = True
                Case EnumEntradaTipo.AlbaranVenta
                    Me.AccessibleName = "AlbaranVenta"
                    Me.ToolForm.ToolForm.Tools("FacturarVenta").SharedProps.Visible = True
                    Me.Pan_Facturable.Visible = True
                    Me.GRD_Linea.M.clsToolBar.Boto_Afegir("IrAFactura", "Ir a la factura", True)
                Case EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaVentaRectificativa
                    Me.AccessibleName = "FacturaVenta"
                    Me.Pan_Vencimientos.Visible = True
                    Me.ToolForm.M.clsToolBar.Afegir_Boto("CerrarFactura", "Cerrar factura", True)
                    Me.ToolForm.M.Botons.tNou.SharedProps.Visible = False
                Case EnumEntradaTipo.FacturaVentaRectificativa
                    Me.AccessibleName = "FacturaRectificativaVenta"
                    Me.Pan_Vencimientos.Visible = True
                    Me.ToolForm.M.clsToolBar.Afegir_Boto("CerrarFactura", "Cerrar factura", True)
                    Me.ToolForm.M.Botons.tNou.SharedProps.Visible = False

                Case EnumEntradaTipo.TraspasoAlmacen
                    Me.AccessibleName = "TraspasoAlmacen"
                    Me.GRD_Linea.M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.Visible, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "AsistenteTraspasos")
                    Me.GRD_Linea.M.clsToolBar.Boto_Afegir("AsistenteTraspasos", "Asistente", True)
            End Select


            Call CargarTextsDelsFormularis()

            '  Util.Tab_InVisible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Ficheros")
            ' Me.T_Codigo.ButtonsRight("Lupeta").Enabled = True


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

    Private Function Guardar(Optional ByVal pMostrarMissatgeDeGuardar As Boolean = True) As Boolean
        Guardar = False
        Try
            Me.TE_Codigo.Focus()

            'If oLinqCliente.ID_Cliente = 0 Then
            '    If BD.RetornaValorSQL("SELECT Count (*) From Cliente WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
            '        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
            '        Exit Function
            '    End If
            'Else
            '    If BD.RetornaValorSQL("SELECT Count (*) From Cliente WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_Cliente<>" & oLinqCliente.ID_Cliente) > 0 Then
            '        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
            '        Exit Function
            '    End If
            'End If

            If oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen And Me.C_Almacen.Value = Me.C_Almacen_Destino.Value Then
                Mensaje.Mostrar_Mensaje("Imposible guardar, el almacén origen no puede ser igual que el almacén destino", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqEntrada)

            If oLinqEntrada.ID_Entrada = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Entrada.InsertOnSubmit(oLinqEntrada)
                oDTC.SubmitChanges()
                Call Fichero.Cargar_GRID(oLinqEntrada.ID_Entrada) 'Fem això pq la classe tingui el ID de pressupost
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                'Call ActivarSegonsEstatPantalla(EnumEstatPantalla.DespresDeGuardar)
                Call HabilitarEnFuncioEstatPantalla()

                If oEntradaTipo = EnumEntradaTipo.PedidoVenta Then 'al crear un pedido de venta crearem una notificació
                    Dim _ClsNotificacion As New clsNotificacion(oDTC)
                    _ClsNotificacion.CrearNotificacion_AlCrearPedidoVenta(oLinqEntrada)
                End If
            Else
                oDTC.SubmitChanges()
                If pMostrarMissatgeDeGuardar = True Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
                End If
            End If

            'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqEntrada
                Me.C_Empresa.Value = .ID_Empresa  'hem de posar aquesta línea abans de carregar el codi de document
                Me.TE_Codigo.Value = .Codigo
                Me.T_Descripcion.Text = .Descripcion
                Me.C_TipoEntrada.Value = .ID_Entrada_Tipo
                Me.C_Estado.Value = .ID_Entrada_Estado
                Me.C_Almacen.Value = .ID_Almacen
                Me.DT_Alta.Value = .FechaAlta
                Me.R_Observaciones.pText = .Observaciones
                Me.DT_Documento.Value = .FechaEntrada
                If .Cliente Is Nothing = False Then
                    Me.TL_Cliente.Tag = .Cliente.ID_Cliente
                    Me.TL_Cliente.Text = .Cliente.Nombre
                    Me.T_Cliente_NIF.Text = .Cliente_Nif
                    Me.T_Cliente_Direccion.Text = .Cliente_Direccion
                    Me.T_Cliente_Poblacion.Text = .Cliente_Poblacion
                    Me.T_Cliente_Provincia.Text = .Cliente_Provincia
                    Me.T_Cliente_PersonaContacto.Text = .Cliente_PersonaContacto
                    Me.T_Cliente_Email.Text = .Cliente_Email
                    Me.T_Cliente_Telefono.Text = .Cliente_Telefono
                    Me.T_Cliente_CP.Text = .Cliente_CP
                    Me.DT_Cliente_Alta.Value = .Cliente_Alta
                    Me.R_Cliente_Observaciones.pText = .Cliente_Observaciones
                End If

                If .Proveedor Is Nothing = False Then
                    Me.TL_Proveedor.Tag = .Proveedor.ID_Proveedor
                    Me.TL_Proveedor.Text = .Proveedor.Nombre
                    Me.T_Proveedor_NIF.Text = .Proveedor_Nif
                    Me.T_Proveedor_Direccion.Text = .Proveedor_Direccion
                    Me.T_Proveedor_Poblacion.Text = .Proveedor_Poblacion
                    Me.T_Proveedor_Provincia.Text = .Proveedor_Provincia
                    Me.T_Proveedor_PersonaContacto.Text = .Proveedor_PersonaContacto
                    Me.T_Proveedor_Email.Text = .Proveedor_Email
                    Me.T_Proveedor_Telefono.Text = .Proveedor_Telefono
                    Me.T_Proveedor_CP.Text = .Proveedor_CP
                    Me.DT_Proveedor_Alta.Value = .Proveedor_Alta
                    Me.R_Proveedor_Observaciones.pText = .Proveedor_Observaciones
                End If

                Me.T_NumAsiento.Text = .NumAsiento
                Me.T_NumPedidoCliente.Text = .NumPedidoCliente
                Me.T_Num_Referencia.Text = .NumReferencia
                Me.T_Proyecto.Text = .Proyecto
                Me.T_NombreObra.Text = .NombreObra
                Me.T_NumeroSeguimiento.Text = .NumeroSeguimiento
                Me.T_NumDocumentoProveedor.Text = .NumeroDocumentoProveedor
                Me.CH_DocumentoImpreso.Checked = .DocumentoImpreso

                Me.T_Descuento.Value = Util.DbnullToNothing(.Descuento)


                If .ID_Entrada_Origen.HasValue = True Then
                    Me.C_Origen.Value = .ID_Entrada_Origen
                Else
                    Me.C_Origen.Value = Nothing
                End If

                If .ID_CompañiaTransporte.HasValue = True Then
                    Me.C_CompañiaTransporte.Value = .ID_CompañiaTransporte
                Else
                    Me.C_CompañiaTransporte.Value = Nothing
                End If

                If .ID_Personal.HasValue = True Then
                    Me.C_Comercial.Value = .ID_Personal
                Else
                    Me.C_Comercial.Value = Nothing
                End If

                If .ID_Campaña.HasValue = True Then
                    Me.C_Campaña.Value = .ID_Campaña
                Else
                    Me.C_Campaña.Value = Nothing
                End If

                If .ID_Almacen_Destino.HasValue = True Then
                    Me.C_Almacen_Destino.Value = .ID_Almacen_Destino
                Else
                    Me.C_Almacen_Destino.Value = Nothing
                End If

                Call CalcularRiesgoStep()

                Me.C_Facturable.Value = Util.Bool_To_Int(.Facturable)

                If oEntradaTipo = EnumEntradaTipo.FacturaVenta Or oEntradaTipo = EnumEntradaTipo.FacturaVentaRectificativa Then 'només amb les factures de venda no recalcuarem i posarem el que hi ha guardat.
                    Me.T_TotalBase.Value = oLinqEntrada.Base
                    Me.T_TotalIVA.Value = oLinqEntrada.IVA
                    Me.T_TotalPropuesta.Value = oLinqEntrada.Descuento
                    Me.T_Descuento_Importe.Value = 0
                    Me.T_Descuento.Value = oLinqEntrada.Descuento
                End If

                If .ID_FormaPago.HasValue Then
                    Me.C_FormaPago.Value = .ID_FormaPago
                End If

                If .DiaDePago.HasValue Then
                    Me.C_DiaPago.Value = .DiaDePago
                End If

                If .Hoja Is Nothing = False Then
                    Me.Excel1.M_LoadDocument(.Hoja)
                End If


            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pEntrada As Entrada)
        Try

            With pEntrada
                .Codigo = Me.TE_Codigo.Value
                .Descripcion = Me.T_Descripcion.Text
                .Observaciones = Me.R_Observaciones.pText
                .FechaAlta = Me.DT_Alta.Value
                .Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = CInt(Me.C_TipoEntrada.Value)).FirstOrDefault
                .Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = CInt(Me.C_Estado.Value)).FirstOrDefault
                .Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.C_Almacen.Value)).FirstOrDefault

                .FechaEntrada = Me.DT_Documento.Value

                If Me.TL_Cliente.Tag Is Nothing = False Then
                    .Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(Me.TL_Cliente.Tag)).FirstOrDefault
                    .Cliente_Nif = Me.T_Cliente_NIF.Text
                    .Cliente_Direccion = Me.T_Cliente_Direccion.Text
                    .Cliente_Poblacion = Me.T_Cliente_Poblacion.Text
                    .Cliente_Provincia = Me.T_Cliente_Provincia.Text
                    .Cliente_PersonaContacto = Me.T_Cliente_PersonaContacto.Text
                    .Cliente_Email = Me.T_Cliente_Email.Text
                    .Cliente_Telefono = Me.T_Cliente_Telefono.Text
                    .Cliente_CP = Me.T_Cliente_CP.Text
                    .Cliente_Alta = Me.DT_Cliente_Alta.Value
                    .Cliente_Observaciones = Me.R_Cliente_Observaciones.pText
                End If
                If Me.TL_Proveedor.Tag Is Nothing = False Then
                    .Proveedor = oDTC.Proveedor.Where(Function(F) F.ID_Proveedor = CInt(Me.TL_Proveedor.Tag)).FirstOrDefault
                    .Proveedor_Nif = Me.T_Proveedor_NIF.Text
                    .Proveedor_Direccion = Me.T_Proveedor_Direccion.Text
                    .Proveedor_Poblacion = Me.T_Proveedor_Poblacion.Text
                    .Proveedor_Provincia = Me.T_Proveedor_Provincia.Text
                    .Proveedor_PersonaContacto = Me.T_Proveedor_PersonaContacto.Text
                    .Proveedor_Email = Me.T_Proveedor_Email.Text
                    .Proveedor_Telefono = Me.T_Proveedor_Telefono.Text
                    .Proveedor_CP = Me.T_Proveedor_CP.Text
                    .Proveedor_Alta = Me.DT_Proveedor_Alta.Value
                    .Proveedor_Observaciones = Me.R_Proveedor_Observaciones.pText
                End If

                .NumAsiento = Me.T_NumAsiento.Text
                .NumPedidoCliente = Me.T_NumPedidoCliente.Text
                .NumReferencia = Me.T_Num_Referencia.Text
                .Proyecto = Me.T_Proyecto.Text
                .NombreObra = Me.T_NombreObra.Text
                .NumeroSeguimiento = Me.T_NumeroSeguimiento.Text
                .NumeroDocumentoProveedor = Me.T_NumDocumentoProveedor.Text

                If oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
                    .Almacen_Destino = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.C_Almacen_Destino.Value)).FirstOrDefault
                End If

                If Me.C_Origen.SelectedIndex <> -1 Then
                    .Entrada_Origen = oDTC.Entrada_Origen.Where(Function(F) F.ID_Entrada_Origen = CInt(Me.C_Origen.Value)).FirstOrDefault
                Else
                    .Entrada_Origen = Nothing
                End If

                If Me.C_CompañiaTransporte.SelectedIndex <> -1 Then
                    .CompañiaTransporte = oDTC.CompañiaTransporte.Where(Function(F) F.ID_CompañiaTransporte = CInt(Me.C_CompañiaTransporte.Value)).FirstOrDefault
                Else
                    .CompañiaTransporte = Nothing
                End If

                If Me.C_Comercial.SelectedIndex <> -1 Then
                    .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_Comercial.Value)).FirstOrDefault
                Else
                    .Personal = Nothing
                End If

                If Me.C_Campaña.SelectedIndex <> -1 Then
                    .Campaña = oDTC.Campaña.Where(Function(F) F.ID_Campaña = CInt(Me.C_Campaña.Value)).FirstOrDefault
                Else
                    .Campaña = Nothing
                End If

                .Base = CDbl(Me.T_TotalBase.Value)
                .IVA = CDbl(Me.T_TotalIVA.Value)
                .Descuento = CDbl(Me.T_Descuento.Value)
                .Total = CDbl(Me.T_TotalPropuesta.Value)
                .Facturable = Me.C_Facturable.Value

                If Me.C_FormaPago.Value = 0 Then
                    .FormaPago = Nothing
                Else
                    .FormaPago = oDTC.FormaPago.Where(Function(F) F.ID_FormaPago = CInt(Me.C_FormaPago.Value)).FirstOrDefault
                End If

                If Me.C_DiaPago.Value = 0 Then
                    .DiaDePago = Nothing
                Else
                    .DiaDePago = Me.C_DiaPago.Value
                End If

                .Hoja = Me.Excel1.M_SaveDocument.ToArray

                .ID_Empresa = Me.C_Empresa.Value
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer, Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        Try
            Call Netejar_Pantalla(pNoCanviarALaPestanyaGeneral)

            oLinqEntrada = (From taula In oDTC.Entrada Where taula.ID_Entrada = pID Select taula).FirstOrDefault


            If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Pendiente Then
                Me.C_Empresa.Enabled = True
                If DirectCast(oLinqEntrada.ID_Entrada_Tipo, EnumEntradaTipo) <> EnumEntradaTipo.Regularizacion And DirectCast(oLinqEntrada.ID_Entrada_Tipo, EnumEntradaTipo) <> EnumEntradaTipo.TraspasoAlmacen And DirectCast(oLinqEntrada.ID_Entrada_Tipo, EnumEntradaTipo) <> EnumEntradaTipo.Inicializacion Then
                    If oLinqEntrada.ID_Cliente.HasValue = True Then
                        Util.Cargar_Combo(Me.C_Empresa, "Select ID_Empresa, NombreComercial From Empresa Where ID_Empresa in (Select ID_Empresa From Cliente_Empresa Where ID_Cliente=" & oLinqEntrada.ID_Cliente & ")  Order by NombreComercial ", False)
                    Else
                        Util.Cargar_Combo(Me.C_Empresa, "Select ID_Empresa, NombreComercial From Empresa Where ID_Empresa in (Select ID_Empresa From Proveedor_Empresa Where ID_Proveedor=" & oLinqEntrada.ID_Proveedor & ")  Order by NombreComercial ", False)
                    End If
                    ' Me.C_Empresa.Value = oLinqEntrada.ID_Empresa
                Else
                    'Util.Cargar_Combo(Me.C_Empresa, "Select ID_Empresa, NombreComercial From Empresa Where ID_Empresa in (Select ID_Empresa From Cliente_Empresa Where ID_Cliente=" & oLinqEntrada.ID_Cliente & ")  Order by NombreComercial ", False)
                    ' Me.C_Empresa.Value = oLinqEntrada.ID_Empresa

                    Me.C_Empresa.Enabled = False
                End If
            End If

            Call SetToForm()

            Call CargaGrid_Seguimiento(pID)
            Fichero.Cargar_GRID(pID)
            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)


            Select Case oEntradaTipo
                Case EnumEntradaTipo.PedidoCompra


                Case EnumEntradaTipo.AlbaranCompra
                    'If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                    '    Me.GRD_Linea.M.Botons.tGridAfegir.SharedProps.Enabled = False
                    '    Me.GRD_Linea.M.Botons.tGridEliminar.SharedProps.Enabled = False
                    'End If
                Case EnumEntradaTipo.FacturaCompra
                    Call GridNSHabilitar(False)
                    Call CargaGrid_Vencimientos(pID)

                Case EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaVentaRectificativa
                    Call CargaGrid_Vencimientos(pID)

                Case EnumEntradaTipo.Inicializacion
                    If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Parcial Then
                        Me.C_Almacen.Enabled = False
                    End If
            End Select

            If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                Call GridNSHabilitar(False)
                Me.GRD_Linea.M_NoEditable()
            Else
                Call GridNSHabilitar(True)
            End If


            Call HabilitarEnFuncioEstatPantalla()
            Call CargarTextsDelsFormularis(oLinqEntrada.Codigo & " - " & oLinqEntrada.Descripcion)

            'Peti qui peti.. intentarem que la pestanya línies sigui la primera
            Util.Tab_Seleccio_x_Key(Me.TAB_Lineas, "Lineas")

            Call CalcularTotals() 'Si no fem això al carregar les dades, al prèmer el botó guardar, guardarà els totals amb 0's
            ' Me.C_Empresa.Enabled = False
            ' Me.EstableixCaptionForm("Entrada: " & (oLinqEntrada.Descripcion))



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla(Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        Try

            oLinqEntrada = New Entrada

            oDTC = New DTCDataContext(BD.Conexion)
            Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
            Call HabilitarLaPantalla()
            Call PestanyesVisibles()

            Me.DT_Alta.Value = Now.Date
            Me.DT_Documento.Value = Now.Date

            'Call CargaGrid_Lineas(0)
            'Exit Sub
            'Call CargaGrid_NS(0)
            Me.C_Estado.Enabled = True
            Fichero.Cargar_GRID(0)

            Util.Cargar_Combo(Me.C_Comercial, "SELECT ID_Personal, Nombre FROM Personal WHERE Activo=1 and ID_Personal_Tipo=" & EnumPersonalTipo.Comercial & " ORDER BY Nombre", False, True, "No asignado")
            Util.Cargar_Combo(Me.C_Empresa, "Select ID_Empresa, NombreComercial From Empresa Where Activo=1  Order by NombreComercial ", False)

            If pNoCanviarALaPestanyaGeneral = False Then
                Me.TAB_Principal.Tabs("General").Selected = True
            End If
            ErrorProvider1.Clear()
            Me.C_Almacen.Enabled = True
            Me.C_Almacen_Destino.Enabled = True
            If oDTC.Almacen.Where(Function(F) F.Predeterminado = True).Count > 0 Then 'Si hi ha algun magatzem predetermina automáticament l'assignarem
                Me.C_Almacen.Value = oDTC.Almacen.Where(Function(F) F.Predeterminado = True).FirstOrDefault.ID_Almacen
            End If


            Me.C_Estado.Value = CInt(EnumEntradaEstado.Pendiente)
            Me.C_TipoEntrada.Value = CInt(oEntradaTipo)

            Me.C_Origen.Value = Nothing
            Me.C_Campaña.Value = Nothing
            Me.C_Comercial.Value = Nothing
            Me.C_Campaña.Value = Nothing

            Me.C_CompañiaTransporte.Value = Nothing
            Me.C_Facturable.Value = 1
            Me.C_FormaPago.Value = Nothing

            Me.C_DiaPago.Value = Nothing
            Me.CH_DocumentoImpreso.Checked = False
            Me.C_Empresa.Value = oEmpresa.ID_Empresa

            Me.C_Empresa.Enabled = True

            Me.TE_Codigo.Value = clsEntrada.RetornaCodiSeguent(oEntradaTipo, oEmpresa.ID_Empresa)
            Call GridNSHabilitar(True)

            Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = True
            Me.TL_Proveedor.ButtonsRight("Lupeta").Enabled = True

            Me.CH_DocumentoAnulado.Visible = False

            'Call CargaGrid_Seguimiento(0)
            'Call CargaGrid_Instalacion(0)
            'Call CargaGrid_Propuesta(0)
            'Call CargaGrid_Partes(0)
            'Call CargaGrid_CambiosEnDocumento()
            'Call CargaGrid_Vencimientos(0)
            Me.GRD_DocumentosAnteriores.GRID.DataSource = Nothing
            Me.GRD_DocumentosVinculados.GRID.DataSource = Nothing

            Call CargarTextsDelsFormularis(Me.TE_Codigo.Value)
            Me.T_Descripcion.Text = clsEntrada.RetornaDescripcioDocumentEnFuncioDelSeuTipus(oEntradaTipo)
            Me.C_Facturable.Value = 1

            Me.Excel1.M_NewDocument()

            Call HabilitarEnFuncioEstatPantalla()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GridNSHabilitar(ByVal pHabilitar As Boolean)
        Me.GRD_Linea.M.Botons.tGridAfegir.SharedProps.Enabled = pHabilitar
        Me.GRD_Linea.M.Botons.tGridEliminar.SharedProps.Enabled = pHabilitar
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                ' oClsControls.ControlBuit(.T_Descripcion)
                oClsControls.ControlBuit(.DT_Alta)
                oClsControls.ControlBuit(.C_Almacen)
                oClsControls.ControlBuit(.C_TipoEntrada)
                Select Case oEntradaTipo
                    Case EnumEntradaTipo.PedidoVenta, EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.DevolucionVenta, EnumEntradaTipo.FacturaVentaRectificativa
                        oClsControls.ControlBuit(.TL_Cliente, clsControles.EPropietat.pTag)
                    Case EnumEntradaTipo.PedidoCompra, EnumEntradaTipo.AlbaranCompra, EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.DevolucionCompra, EnumEntradaTipo.FacturaCompraRectificativa
                        oClsControls.ControlBuit(.TL_Proveedor, clsControles.EPropietat.pTag)
                End Select

                If oEntradaTipo = EnumEntradaTipo.AlbaranCompra Or oEntradaTipo = EnumEntradaTipo.FacturaCompra Or oEntradaTipo = EnumEntradaTipo.DevolucionCompra Or oEntradaTipo = EnumEntradaTipo.FacturaCompraRectificativa Then
                    oClsControls.ControlBuit(.T_NumDocumentoProveedor)
                End If

                'oClsControls.ControlBuit(.T_Persona_Contacto)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()

        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        Select Case oEntradaTipo
            Case EnumEntradaTipo.TraspasoAlmacen
                'Aquesta select es simplement per a que no es visualitzin els documents que el magatzem destí sigui una magatzem de tipus parte
                LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Entrada Where ID_Entrada_Tipo=" & oEntradaTipo & " and IDTipoAlmacenDestino<>5  ORDER BY FechaEntrada", Me.TE_Codigo, "ID_Entrada", "Codigo")
                LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = True
                LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Caption = "Incluir los traspasos con destino a un almacén de tipo parte"
            Case Else
                LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Entrada Where ID_Entrada_Tipo=" & oEntradaTipo & "  ORDER BY FechaEntrada", Me.TE_Codigo, "ID_Entrada", "Codigo")
        End Select

        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat
        AddHandler LlistatGeneric.AlApretarElBotoAuxiliar, AddressOf AlApretarElBotoAuxiliarDelLlistatGeneric
        Dim j As Integer = 0
        Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        For Each pRow In LlistatGeneric.pGrid.GRID.Rows
            ' If pRow.Cells("ID_Empresa").Value Then

            If IsDBNull(pRow.Cells("ID_Empresa").Value) = False AndAlso oDTC.Empresa.Where(Function(F) F.ID_Empresa = CInt(pRow.Cells("ID_Empresa").Value)).FirstOrDefault.ColorEmpresa.HasValue = True Then

                pRow.Cells("Empresa").Appearance.BackColor = Color.FromArgb(oDTC.Empresa.Where(Function(F) F.ID_Empresa = CInt(pRow.Cells("ID_Empresa").Value)).FirstOrDefault.ColorEmpresa)

                pRow.Cells("Codigo").Appearance.BackColor = pRow.Cells("Empresa").Appearance.BackColor
            End If
            '   AddHandler LlistatGeneric.pGridDevExpress.grid

            '  End If
        Next
    End Sub

    Private Sub AlTancarLlistat(ByVal pID As String)
        Try
            Call Cargar_Form(pID)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub AlApretarElBotoAuxiliarDelLlistatGeneric(ByRef pInstanciaLlistatGeneric As M_LlistatGeneric.clsLlistatGeneric)
        'Això es només per traspasos de magatzems
        pInstanciaLlistatGeneric.pGrid.M.clsUltraGrid.Cargar("SELECT * FROM C_Entrada Where  ID_Entrada_Tipo=" & oEntradaTipo & " ORDER BY FechaEntrada", BD)
        pInstanciaLlistatGeneric.AplicarCanvisBotoAuxiliarAlNouGrid()
        pInstanciaLlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = False

    End Sub

    Private Sub CargarTextsDelsFormularis(Optional ByVal pText As String = Nothing)
        Me.EstableixCaptionForm(RetornaTextTipusDocument(pText))

        'Dim _TextComplementari As String = ""
        'If pText Is Nothing = False Then
        '    _TextComplementari = " " & pText
        'End If

        'Select Case oEntradaTipo
        '    Case EnumEntradaTipo.PedidoCompra
        '        Me.EstableixCaptionForm("Pedido de compra" & _TextComplementari)
        '    Case EnumEntradaTipo.AlbaranCompra
        '        Me.EstableixCaptionForm("Albarán de compra" & _TextComplementari)
        '    Case EnumEntradaTipo.FacturaCompra
        '        Me.EstableixCaptionForm("Factura de compra" & _TextComplementari)
        '    Case EnumEntradaTipo.Regularizacion
        '        Me.EstableixCaptionForm("Regularización" & _TextComplementari)
        '    Case EnumEntradaTipo.Devolucion
        '        Me.EstableixCaptionForm("Devolución" & _TextComplementari)
        '    Case EnumEntradaTipo.Inicializacion
        '        Me.EstableixCaptionForm("Albarán de inicialización" & _TextComplementari)
        '    Case EnumEntradaTipo.PedidoVenta
        '        Me.EstableixCaptionForm("Pedido de venta" & _TextComplementari)
        '    Case EnumEntradaTipo.AlbaranVenta
        '        Me.EstableixCaptionForm("Albarán de venta" & _TextComplementari)
        '    Case EnumEntradaTipo.FacturaVenta
        '        Me.EstableixCaptionForm("Factura de venta" & _TextComplementari)
        '    Case EnumEntradaTipo.TraspasoAlmacen
        '        Me.EstableixCaptionForm("Traspaso entre almacenes" & _TextComplementari)
        'End Select
    End Sub

    Public Function RetornaTextTipusDocument(Optional ByVal pText As String = Nothing) As String
        Dim _TextComplementari As String = ""
        If pText Is Nothing = False Then
            _TextComplementari = " " & pText
        End If

        Select Case oEntradaTipo
            Case EnumEntradaTipo.PedidoCompra
                Return "Pedido de compra" & _TextComplementari
            Case EnumEntradaTipo.AlbaranCompra
                Return "Albarán de compra" & _TextComplementari
            Case EnumEntradaTipo.FacturaCompra
                Return "Factura de compra" & _TextComplementari
            Case EnumEntradaTipo.Regularizacion
                Return "Regularización" & _TextComplementari
            Case EnumEntradaTipo.DevolucionCompra
                Return "Devolución de compra " & _TextComplementari
            Case EnumEntradaTipo.DevolucionVenta
                Return "Devolución de venta " & _TextComplementari
            Case EnumEntradaTipo.Inicializacion
                Return "Albarán de inicialización" & _TextComplementari
            Case EnumEntradaTipo.PedidoVenta
                Return "Pedido de venta" & _TextComplementari
            Case EnumEntradaTipo.AlbaranVenta
                Return "Albarán de venta" & _TextComplementari
            Case EnumEntradaTipo.FacturaVenta
                Return "Factura de venta" & _TextComplementari
            Case EnumEntradaTipo.TraspasoAlmacen
                Return "Traspaso entre almacenes" & _TextComplementari
            Case EnumEntradaTipo.FacturaVentaRectificativa
                Return "Factura rectificativa de venta " & _TextComplementari
            Case EnumEntradaTipo.FacturaCompraRectificativa
                Return "Factura rectificativa de compra " & _TextComplementari
        End Select
    End Function

    Private Sub AlTancarFrmEntrada_Linea()
        Call CargaGrid_Lineas(oLinqEntrada.ID_Entrada)
    End Sub

    Private Sub CalcularRiesgoStep()
        Me.T_PedidosPendientesServir.Value = 0
        Me.T_TotalPendienteCobro.Value = 0
        Me.T_AlbaranesPendientesFacturar.Value = 0
        Me.T_CreditoDisponible.Value = 0
        Me.T_FacturasPendientesCobro.Value = 0
        Me.T_CreditoDisponible.Value = 0

        If Me.TL_Cliente.Tag Is Nothing Then
            Exit Sub
        End If

        Me.T_RiesgoMaximo.Value = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(Me.TL_Cliente.Tag)).FirstOrDefault.RiesgoMaximo



        Me.T_PedidosPendientesServir.Value = oDTC.Entrada_Linea.Where(Function(F) F.Entrada.ID_Cliente = CInt(Me.TL_Cliente.Tag) And F.Entrada.ID_Entrada_Estado <> EnumEntradaEstado.Cerrado And F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.PedidoVenta).Sum(Function(T) T.TotalBase)
        Me.T_AlbaranesPendientesFacturar.Value = oDTC.Entrada_Linea.Where(Function(F) F.Entrada.ID_Cliente = CInt(Me.TL_Cliente.Tag) And F.Entrada.ID_Entrada_Estado <> EnumEntradaEstado.Cerrado And F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranVenta And F.ID_Entrada_Linea_Padre.HasValue = False).Sum(Function(T) T.TotalBase)
        Me.T_TotalPendienteCobro.Value = Util.Comprobar_NULL_Per_0_Decimal(Me.T_PedidosPendientesServir.Value) + Util.Comprobar_NULL_Per_0_Decimal(Me.T_AlbaranesPendientesFacturar.Value)
        Me.T_CreditoDisponible.Value = Util.Comprobar_NULL_Per_0_Decimal(Me.T_RiesgoMaximo.Value) - Util.Comprobar_NULL_Per_0_Decimal(Me.T_TotalPendienteCobro.Value)
    End Sub

    Private Sub CalcularTotals()
        'If oEntradaTipo = EnumEntradaTipo.FacturaVenta Then 'Si es una factura de venta no recalcularem els totals
        '    Exit Sub
        'End If

        If (oEntradaTipo <> EnumEntradaTipo.FacturaVenta And oEntradaTipo <> EnumEntradaTipo.FacturaCompra And oEntradaTipo <> EnumEntradaTipo.FacturaVentaRectificativa And oEntradaTipo <> EnumEntradaTipo.FacturaCompraRectificativa) And oLinqEntrada.Entrada_Linea.Count = 0 Then
            Me.T_TotalBase.Value = 0
            Me.T_TotalIVA.Value = 0
            Me.T_TotalPropuesta.Value = 0
            Me.T_Descuento_Importe.Value = 0
            Me.T_Descuento.Value = 0
        Else
            Me.T_TotalBase.Value = oLinqEntrada.RetornaImporteBase  'oLinqEntrada.Entrada_Linea.Sum(Function(F) F.TotalBase).Value

            If Me.T_Descuento.Value Is Nothing OrElse IsDBNull(Me.T_Descuento.Value) OrElse Me.T_Descuento.Value = 0 Then
                Me.T_Descuento.Value = 0
                Me.T_Descuento_Importe.Value = 0
            Else
                oLinqEntrada.Descuento = CDbl(Me.T_Descuento.Value)
                Me.T_Descuento_Importe.Value = oLinqEntrada.RetornaImporteDescuento     'Math.Round((Me.T_TotalBase.Value * Me.T_Descuento.Value) / 100, 2)
            End If

            Me.T_Base_Menos_Descuento.Value = Math.Round(Me.T_TotalBase.Value - Me.T_Descuento_Importe.Value, 2)

            Me.T_TotalIVA.Value = oLinqEntrada.RetornaImporteIVA         'oLinqEntrada.Entrada_Linea.Sum(Function(F) F.TotalIVA).Value
            Me.T_TotalPropuesta.Value = oLinqEntrada.RetornaImporteTotal   'Me.T_Base_Menos_Descuento.Value + Me.T_TotalIVA.Value

            Me.T_TotalCoste.Value = Util.Comprobar_NULL_Per_0_Decimal(BD.RetornaValorSQL("Select Sum(Linea_TotalCostes) From C_Entrada_Linea_ConCalculos Where ID_Entrada_Linea_Padre is null and ID_Entrada=" & oLinqEntrada.ID_Entrada))

            Me.T_Margen.Value = Me.T_Base_Menos_Descuento.Value - Me.T_TotalCoste.Value

            If Me.T_Base_Menos_Descuento.Value > 0 Then
                Me.T_Margen_Porcentaje.Value = Me.T_Margen.Value * 100 / Me.T_Base_Menos_Descuento.Value
            Else
                Me.T_Margen_Porcentaje.Value = 0
            End If

            'Dim _TotalCoste As Decimal = oLinqEntrada.Entrada_Linea.Sum(Function(F) F.Unidad * F.PrecioCoste)
            ' Me.T_Margen_Total.Value = Me.T_Base_Menos_Descuento.Value - _TotalCoste
            'If _TotalCoste = 0 Then
            '    Me.T_Margen_Porcentaje.Value = 0
            'Else
            '    If Me.T_Base_Menos_Descuento.Value = 0 Then
            '        Me.T_Margen_Porcentaje.Value = 0
            '    Else
            '        Me.T_Margen_Porcentaje.Value = (Me.T_Margen_Total.Value * 100) / Me.T_Base_Menos_Descuento.Value  '  (Me.T_Base_Menos_Descuento.Value / _TotalCoste) * 100
            '    End If

            'End If

        End If

        If oLinqEntrada.ID_Entrada <> 0 Then 'Sempre que recalculem guardarem les dades
            With oLinqEntrada
                .Base = CDbl(Me.T_TotalBase.Value)
                .IVA = CDbl(Me.T_TotalIVA.Value)
                .Descuento = CDbl(Me.T_Descuento.Value)
                .Total = CDbl(Me.T_TotalPropuesta.Value)
            End With
            oDTC.SubmitChanges()
            'Call Guardar(False)
        End If
    End Sub

    Private Function RetornaStock(ByVal pIDProducto As Integer, ByVal pIDAlmacen As Integer) As Decimal
        Try
            Dim _Entrada_Linea As IEnumerable(Of Entrada_Linea) = oDTC.Entrada_Linea.Where(Function(F) F.StockActivo = True And F.ID_Almacen = pIDAlmacen And F.ID_Producto = pIDProducto And (F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranCompra Or F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.Inicializacion))
            If _Entrada_Linea Is Nothing Then
                Return 0
            Else
                Return _Entrada_Linea.Sum(Function(F) F.Precio)
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Function RetornaStockPreuPonderat(ByVal pIDProducto As Integer, ByVal pIDAlmacen As Integer) As Decimal
        Try
            Dim _Entrada_Linea As IEnumerable(Of Entrada_Linea) = oDTC.Entrada_Linea.Where(Function(F) F.StockActivo = True And F.ID_Almacen = pIDAlmacen And F.ID_Producto = pIDProducto And (F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranCompra Or F.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.Inicializacion))
            If _Entrada_Linea Is Nothing Then
                Return 0
            Else
                Dim _PrecioTotal As Decimal = _Entrada_Linea.Sum(Function(F) F.TotalBase)
                Dim _CantidadTotal As Decimal = _Entrada_Linea.Sum(Function(F) F.Unidad)

                If _CantidadTotal = 0 Then
                    Return 0
                Else
                    Return _PrecioTotal / _CantidadTotal
                End If

            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Function RetornaNumerosSerie(ByVal pIDProducto As Integer, ByVal pIDAlmacen As Integer) As IEnumerable(Of Entrada_Linea_NS)
        Try
            Dim _Entrada_Linea_NS As IEnumerable(Of Entrada_Linea_NS) = oDTC.Entrada_Linea_NS.Where(Function(F) F.Entrada_Linea.StockActivo = True And F.Entrada_Linea.ID_Almacen = pIDAlmacen And F.Entrada_Linea.ID_Producto = pIDProducto And (F.Entrada_Linea.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.AlbaranCompra Or F.Entrada_Linea.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.Inicializacion))
            Return _Entrada_Linea_NS
            'If _Entrada_Linea Is Nothing Then
            '    Return 0
            'Else
            '    Return _Entrada_Linea.Sum(Function(F) F.Cantidad)
            'End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub PestanyesVisibles()
        Util.Tab_InVisible_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "Facturacion")
        Util.Tab_InVisible_x_Key(Me.TAB_Lineas, M_Util.Enum_Tab_Activacion.ALGUNOS, "MaterialEnAlmacenesParte")
        Me.C_Almacen_Destino.Visible = False
        Me.L_Almacen_Destino.Visible = False
        Select Case oEntradaTipo
            Case EnumEntradaTipo.PedidoVenta, EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.DevolucionVenta, EnumEntradaTipo.FacturaVentaRectificativa
                Util.Tab_InVisible_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "Proveedor")
                Util.Tab_Visible_x_Key(Me.TAB_Lineas, M_Util.Enum_Tab_Activacion.ALGUNOS, "MaterialEnAlmacenesParte")
                'If oEntradaTipo = EnumEntradaTipo.PedidoVenta Then 'el dia 7/9/2016 el Domingo em diu k els pedidos de venta tb tenen que veure aquesta pestanya
                '    Util.Tab_InVisible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Partes")
                'End If
                If oEntradaTipo = EnumEntradaTipo.DevolucionVenta Then
                    Util.Tab_InVisible_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "ReferenciaPedido")
                End If

                Dim _Item As New Infragistics.Win.ValueListItem("Cliente", "Por Cliente")
                If OP_Filtre.Items.Count = 1 Then
                    Me.OP_Filtre.Items.Add(_Item)
                End If


            Case EnumEntradaTipo.PedidoCompra, EnumEntradaTipo.AlbaranCompra, EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.DevolucionCompra, EnumEntradaTipo.FacturaCompraRectificativa
                Util.Tab_InVisible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Partes", "Instalaciones", "Propuestas")
                Util.Tab_InVisible_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "Cliente", "CRM")
                'If oEntradaTipo = EnumEntradaTipo.PedidoCompra Then
                '    Util.Tab_InVisible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "DocumentosVinculados")
                'End If
                If oEntradaTipo = EnumEntradaTipo.DevolucionCompra Then
                    Util.Tab_InVisible_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.ALGUNOS, "ReferenciaPedido")
                End If

                Dim _Item As New Infragistics.Win.ValueListItem("Proveedor", "Por Proveedor")
                If OP_Filtre.Items.Count = 1 Then
                    Me.OP_Filtre.Items.Add(_Item)
                End If

            Case EnumEntradaTipo.Regularizacion, EnumEntradaTipo.TraspasoAlmacen
                Util.Tab_InVisible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General", "Lineas", "Observaciones", "Ficheros")
                Util.Tab_InVisible_x_Key(Me.TAB_Lineas, M_Util.Enum_Tab_Activacion.ALGUNOS, "LíneasStock")
                'Util.Tab_InVisible_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.TODOS)
                Me.Tab_General.Visible = False
                Me.C_Origen.Visible = False
                Me.UltraLabel6.Visible = False
                Me.Pan_Lineas_Precios.Visible = False
                If oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
                    Me.C_Almacen_Destino.Visible = True
                    Me.L_Almacen_Destino.Visible = True
                End If


        End Select
    End Sub

    Private Sub HabilitarEnFuncioEstatPantalla()
        If oLinqEntrada.ID_Entrada = 0 Then 'Si encara no hem guardat...
            Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General")
            Util.Tab_Desactivar_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Cliente", "Proveedor")
            Util.Tab_Desactivar_x_Key(Me.Tab_Cliente, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General")
            Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = True
            Me.TL_Proveedor.ButtonsRight("Lupeta").Enabled = True
            Me.TL_Cliente.ButtonsRight("Ficha").Enabled = False
            Me.TL_Proveedor.ButtonsRight("Ficha").Enabled = False

            Me.C_Almacen.ReadOnly = False 'Això només s'usa per les regularitzacions
            Me.C_Almacen_Destino.ReadOnly = False 'Això només s'usa per les regularitzacions
        Else
            Util.Tab_Activar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)
            Util.Tab_Activar_x_Key(Me.Tab_General, M_Util.Enum_Tab_Activacion.TODOS)
            Util.Tab_Activar_x_Key(Me.Tab_Cliente, M_Util.Enum_Tab_Activacion.TODOS)
            Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = False
            Me.TL_Proveedor.ButtonsRight("Lupeta").Enabled = False
            Me.TL_Cliente.ButtonsRight("Ficha").Enabled = True
            Me.TL_Proveedor.ButtonsRight("Ficha").Enabled = True

            Select Case oEntradaTipo
                Case EnumEntradaTipo.Regularizacion
                    Me.C_Almacen.ReadOnly = True
                Case EnumEntradaTipo.TraspasoAlmacen
                    Me.C_Almacen.ReadOnly = True
                    Me.C_Almacen_Destino.ReadOnly = True
                Case EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaVentaRectificativa, EnumEntradaTipo.FacturaCompraRectificativa
                    Call DesHabilitarLaPantalla()
                    If oLinqEntrada.ID_Entrada_Estado <> EnumEntradaEstado.Cerrado Then
                        ' Call ActivarDesactivarDadesClient(True)
                        ' Call ActivarDesactivarDadesProveidor(True)
                        Util.Activar(Me.Tab_General, M_Util.Enum_Controles_Activacion.TODOS, True)
                    End If
                Case EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.AlbaranCompra, EnumEntradaTipo.DevolucionVenta, EnumEntradaTipo.DevolucionCompra
                    If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                        Call DesHabilitarLaPantalla()
                    End If
                    If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Pendiente Then
                        Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = True
                        Me.TL_Proveedor.ButtonsRight("Lupeta").Enabled = True
                    End If
                Case EnumEntradaTipo.PedidoCompra, EnumEntradaTipo.PedidoVenta
                    If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Pendiente Then
                        Me.TL_Cliente.ButtonsRight("Lupeta").Enabled = True
                        Me.TL_Proveedor.ButtonsRight("Lupeta").Enabled = True
                    End If

                    If oEntradaTipo = EnumEntradaTipo.PedidoVenta AndAlso oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
                        Me.CH_DocumentoAnulado.Visible = True
                    Else
                        Me.CH_DocumentoAnulado.Visible = True
                    End If

            End Select
        End If
    End Sub

    Private Sub DesHabilitarLaPantalla()
        Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS_MENOS_ALGUNOS, True, Me.GRD_Seguimiento)

        'Aquestes dos línies es per fer editable els venciments
        Util.Activar(Me, M_Util.Enum_Controles_Activacion.ALGUNOS, True, Me.GRD_Vencimientos, Me.OP_Filtre, Me.TE_Codigo)
        Me.GRD_Vencimientos.M_Editable()

        Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
        If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Enabled = False
            Me.ToolForm.M.Botons.tEliminar.SharedProps.Enabled = False
        Else
            Me.T_Descripcion.ReadOnly = False
            Me.DT_Documento.ReadOnly = False
        End If
    End Sub

    Private Sub HabilitarLaPantalla()
        Util.Activar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
        Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
        Me.ToolForm.M.Botons.tGuardar.SharedProps.Enabled = True
        Me.ToolForm.M.Botons.tEliminar.SharedProps.Enabled = True
    End Sub

    Function FVencim(ByVal dFechaIni As Date, ByVal nDias As Integer, ByVal nDiaPag As Integer, ByVal nDiaPag2 As Integer) As Date
        Dim nDia, nMes, nAnyo As Integer
        Dim sFecha As String
        Select Case nDias
            Case 30
                FVencim = dFechaIni.AddMonths(1)
            Case 60
                FVencim = dFechaIni.AddMonths(2)
            Case 90
                FVencim = dFechaIni.AddMonths(3)
            Case 120
                FVencim = dFechaIni.AddMonths(4)
            Case Else
                FVencim = dFechaIni.AddDays(nDias)
        End Select

        nDia = FVencim.Day

        nMes = Month(FVencim)
        nAnyo = Year(FVencim)
        ' Si hay dia de pago
        If nDiaPag > 0 Then
            If nDiaPag >= FVencim.Day Then
                nDia = nDiaPag
            Else
                If nDiaPag2 >= FVencim.Day Then
                    nDia = nDiaPag2
                Else
                    nDia = nDiaPag
                    nMes = nMes + 1
                    If nMes > 12 Then
                        nAnyo = nAnyo + 1
                        nMes = 1
                    End If
                End If
            End If
        End If
        sFecha = nDia & "/" & nMes & "/" & nAnyo
        FVencim = CDate(sFecha)
    End Function

    Private Sub ActivarDesactivarDadesClient(ByVal pActivar As Boolean)
        Me.T_Cliente_CP.ReadOnly = Not pActivar
        Me.T_Cliente_NIF.ReadOnly = Not pActivar
        Me.T_Cliente_Direccion.ReadOnly = Not pActivar
        Me.T_Cliente_Poblacion.ReadOnly = Not pActivar
        Me.T_Cliente_Provincia.ReadOnly = Not pActivar
        Me.T_Cliente_PersonaContacto.ReadOnly = Not pActivar
        Me.T_Cliente_Email.ReadOnly = Not pActivar
        Me.T_Cliente_Telefono.ReadOnly = Not pActivar
    End Sub

    Private Sub ActivarDesactivarDadesProveidor(ByVal pActivar As Boolean)
        Me.T_Proveedor_CP.ReadOnly = Not pActivar
        Me.T_Proveedor_NIF.ReadOnly = Not pActivar
        Me.T_Proveedor_Direccion.ReadOnly = Not pActivar
        Me.T_Proveedor_Poblacion.ReadOnly = Not pActivar
        Me.T_Proveedor_Provincia.ReadOnly = Not pActivar
        Me.T_Proveedor_PersonaContacto.ReadOnly = Not pActivar
        Me.T_Proveedor_Email.ReadOnly = Not pActivar
        Me.T_Proveedor_Telefono.ReadOnly = Not pActivar
    End Sub

    Private Sub AlTancarFormAsistenteTraspas(ByVal pTraspasCorrecte As Boolean)
        If pTraspasCorrecte = True Then
            Call CargaGrid_Lineas(oLinqEntrada.ID_Entrada)
        End If
    End Sub

    Private Sub PosarEditableColumnes(ByVal pIDBanda As Integer, ByVal pNomColumna As String)
        Try
            Me.GRD_Linea.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellActivation = Activation.AllowEdit
            Me.GRD_Linea.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellClickAction = CellClickAction.EditAndSelectText
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CalcularPreusLinea(ByRef pRow As UltraGridRow)
        If IsDBNull(pRow.Cells("Precio").Value) OrElse IsDBNull(pRow.Cells("Unidad").Value) OrElse IsDBNull(pRow.Cells("Descuento1").Value) OrElse IsDBNull(pRow.Cells("Descuento2").Value) Then
            Exit Sub
        End If


        Dim _Preu As Decimal = pRow.Cells("Precio").Value
        Dim _Quantitat As Decimal = pRow.Cells("Unidad").Value
        Dim _Descompte1 As Decimal = pRow.Cells("Descuento1").Value
        Dim _Descompte2 As Decimal = pRow.Cells("Descuento2").Value
        Dim _Coste As Decimal = pRow.Cells("Coste").Value
        Dim _IVA As Decimal = pRow.Cells("IVA").Value


        pRow.Cells("TotalBase").Value = (_Quantitat * _Preu - ((_Quantitat * _Preu) * _Descompte1) / 100)
        pRow.Cells("TotalBase").Value = (pRow.Cells("TotalBase").Value - (pRow.Cells("TotalBase").Value * _Descompte2) / 100)
        pRow.Cells("TotalIVA").Value = (pRow.Cells("TotalBase").Value * _IVA) / 100
        pRow.Cells("TotalLinea").Value = pRow.Cells("TotalBase").Value + pRow.Cells("TotalIVA").Value
        'pRow.Cells("TotalPrecioCoste").Value = _Quantitat * _PrecioCoste
        'pRow.Cells("Margen").Value = pRow.Cells("TotalBase").Value - pRow.Cells("TotalPrecioCoste").Value
        'If Me.T_TotalCoste.Value Is Nothing = False Then
        '    Me.T_Margen.Value = Me.T_TotalBase.Value - Util.Comprobar_NULL_Per_0_Decimal(Me.T_TotalCoste.Value)
        'Else
        '    Me.T_Margen.Value = Me.T_TotalBase.Value
        'End If

        ' Call CalcularTotals()
    End Sub

    Private Sub CarregarDadesPestanyes(ByVal pKeyPestanya As String)
        Try

            Dim _ID As Integer = oLinqEntrada.ID_Entrada

            Select Case pKeyPestanya
                Case "Lineas"
                    Call CargaGrid_Lineas(_ID)
                Case "Instalaciones"
                    Call CargaGrid_Instalacion(_ID)
                Case "Propuestas"
                    Call CargaGrid_Propuesta(_ID)
                Case "Partes"
                    Call CargaGrid_Partes(_ID)
                Case "Ficheros"

                Case "CambiosEnDocumento"
                    Call CargaGrid_CambiosEnDocumento()
                Case "DocumentosAnteriores"
                    Call CargaGrid_DocumentosAnteriores()
                Case "DocumentosVinculados"
                    Call CargaGrid_DocumentosVinculados()
                Case "Lineas"
                    'Util.Tab_Seleccio_x_Key(Me.TAB_Lineas, "Lineas")
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub



    Private Sub AvanzarRetroceder(ByVal pAvanzar As Boolean)
        'Que pasa si no s'ha seleccinat cap article?


        Dim _IDATrobar As Integer
        Dim _Trobat As Boolean = False

        If oLinqEntrada.ID_Entrada = 0 Then
            If Me.OP_Filtre.Value = "Cliente" Or Me.OP_Filtre.Value = "Proveedor" Then
                Exit Sub
            Else
                'Si actualment no tenim cap registre carregat farem veure com si estiguessim al últim.
                _IDATrobar = oDTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = oEntradaTipo).Max(Function(F) F.ID_Entrada)
                _Trobat = True
            End If
        Else
            _IDATrobar = oLinqEntrada.ID_Entrada
        End If

        Dim _LlistatDocuments As IList(Of Entrada)
        Select Case Me.OP_Filtre.Value
            Case "Codigo"
                If pAvanzar = True Then
                    _LlistatDocuments = oDTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = oEntradaTipo).OrderBy(Function(F) F.ID_Entrada).ToList
                Else
                    _LlistatDocuments = oDTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = oEntradaTipo).OrderByDescending(Function(F) F.ID_Entrada).ToList
                End If

            Case "Cliente"
                If pAvanzar = True Then
                    _LlistatDocuments = oDTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = oEntradaTipo And F.ID_Cliente = oLinqEntrada.ID_Cliente).OrderBy(Function(F) F.ID_Entrada).ToList
                Else
                    _LlistatDocuments = oDTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = oEntradaTipo And F.ID_Cliente = oLinqEntrada.ID_Cliente).OrderByDescending(Function(F) F.ID_Entrada).ToList
                End If

            Case "Proveedor"
                If pAvanzar = True Then
                    _LlistatDocuments = oDTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = oEntradaTipo And F.ID_Proveedor = oLinqEntrada.ID_Proveedor).OrderBy(Function(F) F.ID_Entrada).ToList
                Else
                    _LlistatDocuments = oDTC.Entrada.Where(Function(F) F.ID_Entrada_Tipo = oEntradaTipo And F.ID_Proveedor = oLinqEntrada.ID_Proveedor).OrderByDescending(Function(F) F.ID_Entrada).ToList
                End If
        End Select


        Dim _Document As Entrada
        Dim _DocumentSeguent As Entrada
        For Each _Document In _LlistatDocuments
            If _Trobat = True Then
                _DocumentSeguent = _Document
                Exit For
            End If
            If _Document.ID_Entrada = _IDATrobar Then
                _Trobat = True
            End If
        Next
        If _DocumentSeguent Is Nothing = False Then
            Dim _TabActual As String = Me.TAB_Principal.SelectedTab.Key
            Call Cargar_Form(_DocumentSeguent.ID_Entrada, True)
            Call CarregarDadesPestanyes(_TabActual)
        End If
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
                Dim _Entrada As Entrada = oDTC.Entrada.Where(Function(F) F.Codigo = CInt(Me.TE_Codigo.Value) And F.ID_Entrada_Tipo = CInt(C_TipoEntrada.Value)).FirstOrDefault()
                If _Entrada Is Nothing = False Then
                    Call Cargar_Form(_Entrada.ID_Entrada)
                Else
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
                    Call Netejar_Pantalla()
                End If
            Else
                Me.TE_Codigo.Value = clsEntrada.RetornaCodiSeguent(oEntradaTipo, oEmpresa.ID_Empresa)
                Exit Sub
            End If
        End If
    End Sub

    Private Sub TE_Cliente_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TL_Cliente.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM Cliente Where ID_Cliente_Tipo=" & EnumClienteTipo.Cliente & "  and Activo=1 and ID_Cliente in (Select ID_Cliente From Cliente_Empresa Where ID_Empresa=" & Me.C_Empresa.Value & ") ORDER BY Nombre", Me.TL_Cliente, "ID_Cliente", "Nombre")
            AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatCliente
        End If

        If e.Button.Key = "Ficha" And Me.TL_Cliente.Tag Is Nothing = False Then
            Dim frmClient As New frmCliente
            frmClient.Entrada(Me.TL_Cliente.Tag)
            frmClient.FormObrir(Me, True)
        End If
    End Sub

    Private Sub AlTancarLlistatCliente(ByVal pID As String)
        If IsNumeric(pID) Then
            Dim oCliente As Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(pID)).FirstOrDefault
            With oCliente
                Me.T_Cliente_Direccion.Text = .Direccion
                Me.T_Cliente_PersonaContacto.Text = .PersonaContacto
                Me.T_Cliente_Email.Text = .Email
                Me.T_Cliente_Poblacion.Text = .Poblacion
                Me.T_Cliente_Provincia.Text = .Provincia
                Me.T_Cliente_Telefono.Text = .Telefono
                Me.T_Cliente_NIF.Text = .NIF
                Me.TL_Cliente.Tag = .ID_Cliente
                Me.TL_Cliente.Text = .Nombre
                Me.DT_Cliente_Alta.Value = .FechaAlta
                Me.T_Cliente_CP.Value = .CP
                Me.R_Cliente_Observaciones.pText = .Observaciones
                Me.C_Comercial.Value = .ID_Personal
                If oCliente.ID_FormaPago.HasValue Then
                    Me.C_FormaPago.Value = .ID_FormaPago
                Else
                    Dim _FormaPago As FormaPago = oDTC.FormaPago.Where(Function(F) F.Predeterminada).FirstOrDefault
                    If _FormaPago Is Nothing = False Then
                        Me.C_FormaPago.Value = _FormaPago.ID_FormaPago
                    End If
                End If
                If oCliente.DiaDePago.HasValue Then
                    Me.C_DiaPago.Value = .DiaDePago
                End If
            End With
            Call CalcularRiesgoStep()
            Me.C_Empresa.Enabled = False
        End If
    End Sub

    Private Sub frmEntrada_AlTancarForm(ByRef pCancel As Boolean) Handles Me.AlTancarForm
        If Me.Visible = True Then
            If oclsPilaFormularis.OcultarFormulari(Me) = True Then
                pCancel = True
            End If
        End If
    End Sub

    Private Sub TE_Proveedor_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TL_Proveedor.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
            LlistatGeneric.Mostrar_Llistat("SELECT * FROM Proveedor Where Activo=1 and ID_Proveedor in (Select ID_Proveedor From Proveedor_Empresa Where ID_Empresa=" & Me.C_Empresa.Value & ") ORDER BY Nombre", Me.TL_Proveedor, "ID_Proveedor", "Nombre")
            AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatProveedor
        End If

        If e.Button.Key = "Ficha" And Me.TL_Proveedor.Tag Is Nothing = False Then
            Dim _frm As New frmProveedor
            _frm.Entrada(Me.TL_Proveedor.Tag)
            _frm.FormObrir(Me, True)
        End If
    End Sub

    Private Sub AlTancarLlistatProveedor(ByVal pID As String)
        If IsNumeric(pID) Then

            Dim oProveedor As Proveedor = oDTC.Proveedor.Where(Function(F) F.ID_Proveedor = CInt(pID)).FirstOrDefault
            With oProveedor
                Me.T_Proveedor_Direccion.Text = .Direccion
                Me.T_Proveedor_PersonaContacto.Text = .PersonaContacto
                Me.T_Proveedor_Email.Text = .Email
                Me.T_Proveedor_Poblacion.Text = .Poblacion
                Me.T_Proveedor_Provincia.Text = .Provincia
                Me.T_Proveedor_Telefono.Text = .Telefono
                Me.T_Proveedor_NIF.Text = .NIF
                Me.TL_Proveedor.Tag = .ID_Proveedor
                Me.TL_Proveedor.Text = .Nombre
                Me.DT_Proveedor_Alta.Value = .FechaAlta
                Me.T_Proveedor_CP.Value = .CP
                Me.R_Proveedor_Observaciones.pText = .Observaciones
                If .ID_FormaPago.HasValue Then
                    Me.C_FormaPago.Value = .ID_FormaPago
                Else
                    Dim _FormaPago As FormaPago = oDTC.FormaPago.Where(Function(F) F.Predeterminada).FirstOrDefault
                    If _FormaPago Is Nothing = False Then
                        Me.C_FormaPago.Value = _FormaPago.ID_FormaPago
                    End If
                End If
                If .DiaDePago.HasValue Then
                    Me.C_DiaPago.Value = .DiaDePago
                End If
            End With
        End If
    End Sub

    Public Sub AlTancarFormTraspas(ByVal pTraspasatCorrectament As Boolean)
        Try
            If pTraspasatCorrectament = True Then
                Call Cargar_Form(oLinqEntrada.ID_Entrada)
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub TAB_Principal_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
        Call CarregarDadesPestanyes(e.Tab.Key)
    End Sub

    Private Sub C_Campaña_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Campaña.BeforeDropDown
        If Me.TL_Cliente.Tag Is Nothing = False Then
            Util.Cargar_Combo(Me.C_Campaña, "Select ID_Campaña, Descripcion From Campaña Where ID_Campaña in (Select ID_Campaña From Campaña_Cliente Where ID_Cliente=" & Me.TL_Cliente.Tag & " Group by ID_Campaña) Order by Descripcion ")
        End If
    End Sub

    Private Sub T_Descuento_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles T_Descuento.KeyUp
        Call CalcularTotals()
    End Sub

    Private Sub C_Facturable_ValueChanged(sender As Object, e As EventArgs) Handles C_Facturable.ValueChanged
        If Me.C_Facturable.SelectedIndex <> -1 Then
            If Me.C_Facturable.Value = 1 Then
                Me.B_CerrarAlbaran.Visible = False
            Else
                If oLinqEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Factura.HasValue = True).Count Then
                    Mensaje.Mostrar_Mensaje("Imposible establecer al albaran como no facturable si tiene líneas facturadas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Me.C_Facturable.Value = 1
                    Exit Sub
                End If
                Me.B_CerrarAlbaran.Visible = True
            End If
        End If
    End Sub

    Private Sub B_CerrarAlbaran_Click(sender As Object, e As EventArgs) Handles B_CerrarAlbaran.Click
        If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
            Mensaje.Mostrar_Mensaje("El albarán ya está cerrado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If
        'Canviem l'estat a cerrado, i si no es pot guardar tornem a canviar l'estat
        Me.C_Estado.Value = CInt(EnumEntradaEstado.Cerrado)
        If Guardar() = False Then
            Me.C_Estado.Value = CInt(EnumEntradaEstado.Pendiente)
        End If


    End Sub

    Private Sub TAB_Lineas_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Lineas.SelectedTabChanged
        Try
            Select Case e.Tab.Key
                Case "LíneasStock"
                    With Me.GRD_LineasStock
                        .GRID.DataSource = Nothing

                        Dim DTS As New DataSet

                        Select Case oEntradaTipo
                            Case EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaCompraRectificativa, EnumEntradaTipo.FacturaVentaRectificativa 'Les factures agafen les dades del camp ID_Factura
                                BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where NoRestarStock=0 and ID_Entrada_Factura=" & oLinqEntrada.ID_Entrada)
                            Case EnumEntradaTipo.TraspasoAlmacen
                                'BD.CargarDataSet(DTS, "Select ID_Entrada_Linea , Descripcion as C From Entrada_Linea_NS, NS Where Entrada_Linea_NS.ID_NS=NS.ID_NS Order by Descripcion", "aaaa", 0, "ID_Entrada_Linea", "ID_Entrada_Linea", False)
                            Case Else
                                BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where NoRestarStock=0 and  ID_Entrada=" & oLinqEntrada.ID_Entrada)
                        End Select

                        '.M.clsUltraGrid.Cargar(DTS)
                        .GRID.DataSource = DTS
                        .GRID.DisplayLayout.MaxBandDepth = 2

                    End With

                Case "MaterialEnAlmacenesParte"
                    With Me.GRD_MaterialEnAlmacenesDeLosPartesAsignados
                        .GRID.DataSource = Nothing

                        .M.clsUltraGrid.Cargar("Select * From RetornaStock(0,0) Where ID_Almacen In (Select ID_Almacen From Almacen Where ID_Parte in (Select ID_Parte From Entrada_Parte Where ID_Entrada=" & oLinqEntrada.ID_Entrada & ")) Order by ProductoDescripcion", BD)

                    End With
            End Select





        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub B_FormaPago_Calcular_Click(sender As Object, e As EventArgs) Handles B_FormaPago_Calcular.Click
        If oLinqEntrada.ID_Entrada = 0 Then
            Exit Sub
        End If

        If Guardar() = False Then
            Exit Sub
        End If

        If oLinqEntrada.Entrada_Vencimiento.Count > 0 Then
            If oLinqEntrada.Entrada_Vencimiento.Where(Function(F) F.ID_Remesa.HasValue = True).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar los vencimientos actualmente asignados, uno o más vencimientos estan asignados a una remesa", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje("Ya hay datos introducidos, ¿seguro que quiere eliminarlos?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.NO Then
                Exit Sub
            Else
                oDTC.Entrada_Vencimiento.DeleteAllOnSubmit(oLinqEntrada.Entrada_Vencimiento) 'borrem tots els vencimientos
            End If
        End If

        If Me.C_FormaPago.Value Is Nothing Then
            Mensaje.Mostrar_Mensaje("Imposible calcular datos si no está especificada la forma de pago", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        ' Me.C_DiaPago.Value Is Nothing 
        Dim _FormaPago As FormaPago = oDTC.FormaPago.Where(Function(F) F.ID_FormaPago = CInt(Me.C_FormaPago.Value)).FirstOrDefault

        If oEntradaTipo = EnumEntradaTipo.FacturaVenta Or oEntradaTipo = EnumEntradaTipo.FacturaVentaRectificativa Then
            If _FormaPago.FormaPago_Tipo.GenerarDomiciliacion = True And oLinqEntrada.Cliente.Cliente_CuentaBancaria.Where(Function(F) F.Domiciliacion = True).Count = 0 Then
                Mensaje.Mostrar_Mensaje("Imposible crear vencimientos con una forma de pago domiciliada. El cliente no tiene ninguna cuenta bancaria con domiciliación", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If
        End If


        Dim _DiaPago As Integer
        If Me.C_DiaPago.SelectedIndex = -1 Then 'si no s'ha especificat dia de pago
            _DiaPago = 0
        Else
            _DiaPago = Me.C_DiaPago.Value
        End If


        If _FormaPago.FormaPago_Giro.Count = 0 Then 'Si no s'han espeficat giros vol dir que tot es farà en un pagament
            Dim _NewVencimiento As New Entrada_Vencimiento
            With _NewVencimiento
                .Importe = oLinqEntrada.Total
                .Observaciones = ""
                .Pagado = False
                .Empresa_CuentaBancaria = Nothing
                .Fecha = FVencim(Me.DT_Documento.Value, 0, _DiaPago, 0)
                .FormaPago = oLinqEntrada.FormaPago
                .Domiciliacion = False
                .Entrada_Vencimiento_Estado = Nothing
                .Cliente_CuentaBancaria = Nothing
                .Proveedor_CuentaBancaria = Nothing
                If _FormaPago.FormaPago_Tipo.GenerarDomiciliacion = True Then
                    .Empresa_CuentaBancaria = _FormaPago.Empresa_CuentaBancaria
                    .Domiciliacion = True
                    .Entrada_Vencimiento_Estado = oDTC.Entrada_Vencimiento_Estado.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.PendienteAsignarARemesa).FirstOrDefault
                    Select Case oEntradaTipo
                        Case EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaVentaRectificativa
                            .Cliente_CuentaBancaria = oLinqEntrada.Cliente.Cliente_CuentaBancaria.Where(Function(F) F.Domiciliacion = True And F.Predeterminada = True).FirstOrDefault
                        Case EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaCompraRectificativa
                            .Proveedor_CuentaBancaria = oLinqEntrada.Proveedor.Proveedor_CuentaBancaria.Where(Function(F) F.Domiciliacion = True And F.Predeterminada = True).FirstOrDefault
                    End Select

                End If
                oLinqEntrada.Entrada_Vencimiento.Add(_NewVencimiento)
            End With
        Else
            Dim _Giros As FormaPago_Giro
            For Each _Giros In _FormaPago.FormaPago_Giro
                Dim _NewVencimiento As New Entrada_Vencimiento
                With _NewVencimiento
                    Dim _Import As Decimal = Math.Round(CDbl((oLinqEntrada.Total / _FormaPago.FormaPago_Giro.Count)), 3)
                    .Importe = Math.Round(_Import, 2)
                    .Observaciones = ""
                    .Pagado = False
                    .Empresa_CuentaBancaria = Nothing
                    .Fecha = FVencim(Me.DT_Documento.Value, _Giros.DiasGiro, _DiaPago, 0)
                    .FormaPago = oLinqEntrada.FormaPago

                    .Domiciliacion = False
                    .Entrada_Vencimiento_Estado = Nothing
                    .Cliente_CuentaBancaria = Nothing
                    .Proveedor_CuentaBancaria = Nothing
                    If _FormaPago.FormaPago_Tipo.GenerarDomiciliacion = True Then
                        .Empresa_CuentaBancaria = _FormaPago.Empresa_CuentaBancaria
                        .Domiciliacion = True
                        .Entrada_Vencimiento_Estado = oDTC.Entrada_Vencimiento_Estado.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.PendienteAsignarARemesa).FirstOrDefault
                        Select Case oEntradaTipo
                            Case EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaVentaRectificativa
                                .Cliente_CuentaBancaria = oLinqEntrada.Cliente.Cliente_CuentaBancaria.Where(Function(F) F.Domiciliacion = True And F.Predeterminada = True).FirstOrDefault
                            Case EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaCompraRectificativa
                                .Proveedor_CuentaBancaria = oLinqEntrada.Proveedor.Proveedor_CuentaBancaria.Where(Function(F) F.Domiciliacion = True And F.Predeterminada = True).FirstOrDefault
                        End Select
                    End If
                    oLinqEntrada.Entrada_Vencimiento.Add(_NewVencimiento)
                End With
            Next
        End If
        oDTC.SubmitChanges()
        Call CargaGrid_Vencimientos(oLinqEntrada.ID_Entrada)
    End Sub

    Private Sub C_Almacen_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Almacen.BeforeDropDown, C_Almacen_Destino.BeforeDropDown
        Dim _Combo As Infragistics.Win.UltraWinEditors.UltraComboEditor = sender
        Dim _IDAlmacen As Integer = Me.C_Almacen.Value
        Dim _Boto As Infragistics.Win.UltraWinEditors.StateEditorButton = Me.C_Almacen.ButtonsRight("Todos")
        If _Boto.Checked = True Then
            Util.Cargar_Combo(_Combo, "SELECT ID_Almacen, Descripcion FROM Almacen Where Activo=1 ORDER BY Descripcion", False)
        Else
            If oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
                Util.Cargar_Combo(_Combo, "SELECT Almacen.ID_Almacen, Almacen.Descripcion FROM  Almacen LEFT OUTER JOIN  Parte ON Almacen.ID_Parte = dbo.Parte.ID_Parte WHERE (isnull(Parte.ID_Parte_Estado,0) <> 3) Order by Descripcion", True, False)
            Else
                Util.Cargar_Combo(_Combo, "Select ID_Almacen, Descripcion From Almacen Where Activo=1 and ID_Almacen in (Select ID_Almacen From Almacen Where ID_Almacen_Tipo<>" & EnumAlmacenTipo.Parte & " or ID_Parte in (Select ID_Parte From Entrada_Parte Where ID_Entrada=" & oLinqEntrada.ID_Entrada & "))  Order by Descripcion", False, False)
            End If

            ' 
        End If
        _Combo.Value = _IDAlmacen
    End Sub

    Private Sub OP_Instalacions_ValueChanged(sender As Object, e As EventArgs) Handles OP_Instalacions.ValueChanged
        'Fem això, per que si la línea esta seleccionada, el combo no refresca en funció del canvi que s'hagi fet en el option button
        Me.GRD_Instalacion.GRID.Selected.Rows.Clear()
        Me.GRD_Instalacion.GRID.ActiveRow = Nothing
    End Sub

    Private Sub OP_Partes_ValueChanged(sender As Object, e As EventArgs) Handles OP_Partes.ValueChanged
        'Fem això, per que si la línea esta seleccionada, el combo no refresca en funció del canvi que s'hagi fet en el option button
        Me.GRD_Partes.GRID.Selected.Rows.Clear()
        Me.GRD_Partes.GRID.ActiveRow = Nothing
    End Sub

    Private Sub C_Comercial_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Comercial.BeforeDropDown
        Dim _SQL As String = ""
        If oLinqEntrada.ID_Personal.HasValue Then
            _SQL = " or ID_Personal= " & oLinqEntrada.ID_Personal
        End If
        Util.Cargar_Combo(Me.C_Comercial, "Select ID_Personal, Nombre From Personal Where Activo=1 and FechaBajaEmpresa is null" & "  and ID_Personal_Tipo=" & EnumPersonalTipo.Comercial & _SQL & " Order by Nombre", False, False)
        Me.C_Comercial.Value = oLinqEntrada.ID_Personal
    End Sub

    Private Sub B_Adelante_Click(sender As Object, e As EventArgs) Handles B_Adelante.Click
        Call AvanzarRetroceder(True)
    End Sub

    Private Sub B_Atras_Click(sender As Object, e As EventArgs) Handles B_Atras.Click
        Call AvanzarRetroceder(False)
    End Sub

    Private Sub C_Empresa_ValueChanged(sender As Object, e As EventArgs) Handles C_Empresa.ValueChanged
        If Me.C_Empresa.Value Is Nothing = False AndAlso Me.C_Empresa.Text <> "C_Empresa" Then
            Me.TE_Codigo.Value = clsEntrada.RetornaCodiSeguent(oEntradaTipo, Me.C_Empresa.Value)
        End If
    End Sub
#End Region

#Region "Grid Lineas"
    Private Sub CargaGrid_Lineas(ByVal pId As Integer)
        Try

            With Me.GRD_Linea
                '.M.clsUltraGrid.CargarIEnumerable(_Lineas)
                Dim DT As New DataTable



                '.M.clsUltraGrid.Cargar(DTS)
                Dim DTS As New DataSet

                If pId = 0 Then
                    ' BD.CargarDataSet(DTS, "Select top 0 * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Factura=" & oLinqEntrada.ID_Entrada)
                    ' .M.clsUltraGrid.Cargar(DTS)
                    .GRID.DataSource = Nothing
                    Exit Sub
                End If

                Select Case oEntradaTipo
                    Case EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaCompraRectificativa, EnumEntradaTipo.FacturaVentaRectificativa 'Les factures agafen les dades del camp ID_Factura
                        'DT = BD.RetornaDataTable("Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Factura=" & oLinqEntrada.ID_Entrada)
                        BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Factura=" & oLinqEntrada.ID_Entrada)
                    Case Else
                        BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and  ID_Entrada=" & oLinqEntrada.ID_Entrada)
                        'DT = BD.RetornaDataTable("Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and  ID_Entrada=" & oLinqEntrada.ID_Entrada)
                End Select

                BD.CargarDataSet(DTS, "Select Entrada_Linea.ID_Entrada_Linea, NS.Descripcion, NS.Virtual From NS, Entrada_Linea_NS, Entrada_Linea Where NS.ID_NS=Entrada_Linea_NS.ID_NS and Entrada_Linea_NS.ID_Entrada_Linea=Entrada_Linea.ID_Entrada_Linea and ID_Entrada=" & oLinqEntrada.ID_Entrada, "NS", 0, "ID_Entrada_Linea", "ID_Entrada_Linea", False)
                BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada=" & oLinqEntrada.ID_Entrada, "CompuestoPor", 0, "ID_Entrada_Linea", "ID_Entrada_Linea_Padre", False)
                'Dim DT2 As DataTable = BD.RetornaDataTable("Select Entrada_Linea.ID_Entrada_Linea, NS.Descripcion, NS.Virtual From NS, Entrada_Linea_NS, Entrada_Linea Where NS.ID_NS=Entrada_Linea_NS.ID_NS and Entrada_Linea_NS.ID_Entrada_Linea=Entrada_Linea.ID_Entrada_Linea and ID_Entrada=" & oLinqEntrada.ID_Entrada)
                'MsgBox(DT2.Rows.Count)
                BD.CargarDataSet(DTS, "Select Entrada_Linea.ID_Entrada_Linea, NS.Descripcion, NS.Virtual From NS, Entrada_Linea_NS, Entrada_Linea Where NS.ID_NS=Entrada_Linea_NS.ID_NS and Entrada_Linea_NS.ID_Entrada_Linea=Entrada_Linea.ID_Entrada_Linea and ID_Entrada=" & oLinqEntrada.ID_Entrada, "NS2", 2, "ID_Entrada_Linea", "ID_Entrada_Linea", False)

                .M.clsUltraGrid.Cargar(DTS)
                '.GRID.DataSource = DT
                .GRID.DisplayLayout.MaxBandDepth = 4

                'Si és un pedido passarem per totes les línies per calcular la quantitat traspasada als albarans
                If .GRID.DisplayLayout.Bands(0).Columns.Exists("RequiereNumeroSerie") Then
                    Dim pRow As UltraGridRow
                    For Each pRow In .GRID.Rows
                        If pRow.Cells("RequiereNumeroSerie").Value = True Then
                            pRow.Cells("RequiereNumeroSerie").Appearance.BackColor = Color.FromArgb(223, 255, 192)
                            Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(pRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                            If _Linea.NoRestarStock = False And _Linea.Entrada_Linea_NS.Where(Function(F) F.NS.Virtual = True).Count > 0 Then
                                pRow.Cells("RequiereNumeroSerie").Appearance.BackColor = Color.Red
                            End If

                        Else
                            pRow.Cells("RequiereNumeroSerie").Appearance.BackColor = Color.FromArgb(192, 255, 255)
                        End If
                        If IsDBNull(pRow.Cells("NumFactura").Value) = False AndAlso IsNumeric(pRow.Cells("NumFactura").Value) Then
                            pRow.Cells("NumFactura").Appearance.BackColor = Color.LightGreen
                        End If
                    Next
                End If

                Call CalcularTotals()

                'Si el botó editar del grid esta deshabilitat llavors no es podrà editar el grid
                If Me.GRD_Linea.M.Botons.tGridEditar.SharedProps.Enabled = True Then
                    .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
                    For Each _Banda In Me.GRD_Linea.GRID.DisplayLayout.Bands
                        If _Banda.Index <> 0 Then 'Demoment només deixarem editable la banda 0
                            Exit For
                        End If

                        Call PosarEditableColumnes(_Banda.Index, "Unidad")
                        Call PosarEditableColumnes(_Banda.Index, "Precio")
                        Call PosarEditableColumnes(_Banda.Index, "Descuento1")
                        Call PosarEditableColumnes(_Banda.Index, "Descuento2")
                        Call PosarEditableColumnes(_Banda.Index, "Coste")
                        Call PosarEditableColumnes(_Banda.Index, "Descripcion")

                    Next
                End If

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    '   Private Sub GRD_Linea_M_GRID_ClickRow(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Linea.M_GRID_ClickRow
    'If Me.GRD_Linea.GRID.ActiveRow Is Nothing Then
    '    Exit Sub
    'End If
    'Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(Me.GRD_Linea.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
    'Dim _clsLinea As New clsEntradaLinea(oLinqEntrada, _Linea, oDTC)

    'If Me.GRD_Linea.GRID.ActiveRow Is Nothing = True Then
    '    Exit Sub
    'End If

    'If Me.GRD_Linea.GRID.Selected.Rows.Count <> 1 Then
    '    Exit Sub
    'End If

    'Dim pIDLinea As Integer = Me.GRD_Linea.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value
    'Select Case oEntradaTipo
    '    Case EnumEntradaTipo.Regularizacion
    '        Call CargaGrid_NS(pIDLinea)
    '    Case Else
    '        Call CargaGrid_NS(pIDLinea)
    'End Select
    ' End If
    '   End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Linea.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Linea
                If oLinqEntrada.ID_Entrada = 0 Then
                    If Guardar() = False Then
                        Exit Sub
                    End If
                End If


                Dim frm As New frmEntrada_Linea
                frm.Entrada(oLinqEntrada, oDTC, Nothing)
                AddHandler frm.FormClosing, AddressOf AlTancarFrmEntrada_Linea
                frm.FormObrir(Me)

                '.M_ExitEditMode()
                '.M_AfegirFila()

                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Almacen").Value = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.C_Almacen.Value)).FirstOrDefault
                ''.GRID.Rows(.GRID.Rows.Count - 1).Cells("CantidadTraspasada").Value = 0   no se pq pero peta al afegir aquesta línea
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaEntrada").Value = Now.Date
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Entrada").Value = oLinqEntrada
                '.GRID.Rows(.GRID.Rows.Count - 1).Cells("StockActivo").Value = True



            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolEditar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Linea.M_ToolGrid_ToolEditar
        Call GRD_Linea_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Linea.M_ToolGrid_ToolVisualitzarDobleClickRow
        'If oLinqEntrada.ID_Entrada = 0 Then
        '    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
        '    Exit Sub
        'End If

        If oEntradaTipo = EnumEntradaTipo.Regularizacion Or oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Then
            Exit Sub
        End If

        If Me.GRD_Linea.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        If Me.GRD_Linea.GRID.Selected.Rows(0).Band.Index <> 0 Then
            Exit Sub
        End If

        If Guardar() = False Then
            Exit Sub
        End If

        Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(Me.GRD_Linea.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault

        Dim frm As New frmEntrada_Linea
        frm.Entrada(oLinqEntrada, oDTC, _Linea)
        AddHandler frm.FormClosing, AddressOf AlTancarFrmEntrada_Linea
        frm.FormObrir(Me)
    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Linea.M_ToolGrid_ToolEliminarRow
        Try
            If Me.GRD_Linea.GRID.Selected.Rows.Count = 0 OrElse Me.GRD_Linea.GRID.Selected.Rows(0).Band.Index <> 0 Then
                Exit Sub
            End If

            If oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Or oEntradaTipo = EnumEntradaTipo.Regularizacion Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar una línea de este tipo de documento", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If




            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
                Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(Me.GRD_Linea.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                Dim _CodiArticle As String = _Linea.Producto.Codigo
                Dim _clsEntradaLinea As New clsEntradaLinea(oLinqEntrada, _Linea, oDTC)
                'If oEntradaTipo = EnumEntradaTipo.TraspasoAlmacen Or oEntradaTipo = EnumEntradaTipo.Regularizacion Then
                '    'Eliminarem totes les rows que tinguin el Codi d'article de la línea que ja hem esborrat
                '    'For Each _Linea In oLinqEntrada.Entrada_Linea.Where(Function(F) F.Producto.Codigo = _CodiArticle)
                '    '    _clsEntradaLinea = New clsEntradaLinea(oLinqEntrada, _Linea, oDTC)
                '    '    _clsEntradaLinea.EliminarLinea()
                '    'Next
                'Else
                If _clsEntradaLinea.EliminarLinea() = False Then
                    Mensaje.Mostrar_Mensaje("No se ha podido eliminar la línea", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                    ' End If

                End If


                oDTC.SubmitChanges()

                Mensaje.Mostrar_Mensaje("Línea eliminada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)

                Call CargaGrid_Lineas(oLinqEntrada.ID_Entrada)
            End If

            'If e.IsAddRow Then
            '    oLinqEntrada.Entrada_Linea.Remove(e.ListObject)
            '    Exit Sub
            'End If

            'Dim _Linea As Entrada_Linea = e.ListObject

            'Dim _clsEntrada As New clsEntrada(oDTC, oLinqEntrada)
            '_clsEntrada.EliminarLinea(_Linea, e)


            'If _Linea.CantidadTraspasada > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible eliminar una línea que ya se ha albaranado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '    Exit Sub
            'End If

            'If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
            '    If _Linea.ID_Entrada_Linea_Pedido.HasValue = True Then 'si la linea prové d'una comanda
            '        If _Linea.Entrada_Linea_Albaran.Entrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado Then
            '            If Mensaje.Mostrar_Mensaje("Esta línea de albarán proviene de un pedido cerrado, ¿desea abrir el pedido?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
            '                _Linea.Entrada_Linea_Albaran.Entrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
            '            End If
            '        End If
            '    End If

            '    e.Delete(False)
            '    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            'End If

            'oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    ' Private Sub GRD_Linea_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Linea.M_GRID_AfterRowUpdate
    'Select Case oEntradaTipo
    '    Case EnumEntradaTipo.Pedido
    '        Dim _LineaPedido As Entrada_Linea
    '        _LineaPedido = e.Row.ListObject
    '        '  If _LineaPedido.ID_Entrada_Linea = 0 Then 'si es una nova linea
    '        If _LineaPedido.Producto.RequiereNumeroSerie = True Then
    '            Call clsEntrada.GenerarNumerosSerieLinea(oDTC, _LineaPedido, _LineaPedido.Cantidad)
    '            ' Call CargaGrid_NS(0)
    '        End If
    '        '   End If
    '    Case EnumEntradaTipo.Regularizacion
    '        'Aqui el que fem es impedir que es puguin posar en una mateixa regularització dos productes iguals
    '        'Si es una nova row
    '        Dim _Linea As Entrada_Linea
    '        _Linea = e.Row.ListObject
    '        If _Linea.ID_Entrada_Linea = 0 Then
    '            'Posem +1 per excloure's a si mateixa
    '            If oLinqEntrada.Entrada_Linea.Where(Function(F) F.ID_Producto = _Linea.ID_Producto).Count > 1 Then
    '                Mensaje.Mostrar_Mensaje("Imposible el mismo producto dos veces a la misma regularización", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                e.Row.Delete(False)
    '            End If
    '        End If
    'End Select




    'oDTC.SubmitChanges()
    ' End Sub

    'Private Sub CarregarComboProductes()
    '    Try
    '        ' Dim DT As DataTable = BD.RetornaDataTable("SELECT ID_Producto, Codigo, Descripcion From Producto Where Activo=1 Order by Descripcion")
    '        Dim _LlistatProductes As IEnumerable = From Taula In oDTC.Producto Where Taula.Activo = True Order By Taula.Descripcion Select Taula.ID_Producto, Taula.Codigo, Taula.Descripcion, Producto = Taula

    '        Me.C_Producto.DataSource = _LlistatProductes
    '        If _LlistatProductes Is Nothing Then
    '            Exit Sub
    '        End If

    '        With C_Producto
    '            .AutoCompleteMode = AutoCompleteMode.Suggest
    '            .AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
    '            .MaxDropDownItems = 16
    '            .DisplayMember = "Descripcion"
    '            .ValueMember = "Producto"
    '            .DisplayLayout.Bands(0).Columns("ID_Producto").Hidden = True
    '            .DisplayLayout.Bands(0).Columns("Codigo").Width = 100
    '            .DisplayLayout.Bands(0).Columns("Codigo").Header.Caption = "Código"
    '            .DisplayLayout.Bands(0).Columns("Descripcion").Width = 600
    '            .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
    '            .DisplayLayout.Bands(0).Columns("Producto").Hidden = True
    '        End With
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    'Private Sub GRD_Productos_M_Grid_InitializeLayout(Sender As Object, e As InitializeLayoutEventArgs) Handles GRD_Linea.M_Grid_InitializeLayout
    '    e.Layout.Bands(0).Columns("Producto").EditorComponent = Me.C_Producto
    '    e.Layout.Bands(0).Columns("Producto").AutoCompleteMode = AutoCompleteMode.SuggestAppend
    '    e.Layout.Bands(0).Columns("Producto").AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
    'End Sub

    'Private Sub CarregarCombo_Almacen(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Almacen) = (From Taula In oDTC.Almacen Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
    '        Dim Var As Almacen

    '        'Valors.ValueListItems.Add("-1", "Seleccione un registro")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Descripcion)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("Almacen").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("Almacen").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    'Private Sub GRD_Linea_M_GRID_BeforeCellUpdate(sender As Object, e As BeforeCellUpdateEventArgs) Handles GRD_Linea.M_GRID_BeforeCellUpdate
    '    If oEntradaTipo = EnumEntradaTipo.Albaran Or oEntradaTipo = EnumEntradaTipo.Pedido Then
    '        'Si s'ha modificat la quantitat, si la quantiat introduida es numérica..
    '        If e.Cell.Column.Key = "Cantidad" Then
    '            If IsNumeric(e.NewValue) Then
    '                'Si la quantitat traspasada es superior a la nova quantitat que es vol introduir no ho deixarem fer
    '                If e.Cell.Row.Cells("CantidadTraspasada").Value > 0 AndAlso e.NewValue < e.Cell.Row.Cells("CantidadTraspasada").Value Then
    '                    Mensaje.Mostrar_Mensaje("Imposible introducir una cantidad más pequeña que la ya traspasada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                    e.Cancel = True
    '                    Exit Sub
    '                End If
    '                Dim _LineaPedido As Entrada_Linea
    '                _LineaPedido = e.Cell.Row.ListObject

    '                If _LineaPedido.Producto Is Nothing = False AndAlso _LineaPedido.Producto.RequiereNumeroSerie = True Then
    '                    Call clsEntrada.GenerarNumerosSerieLinea(oDTC, _LineaPedido, e.NewValue)
    '                End If
    '                'Call CargaGrid_NS(0)
    '            End If
    '        End If
    '    End If
    'End Sub

    Private Sub GRD_Lineas_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Linea.M_Grid_InitializeRow
        With Me.GRD_Linea
            If e.Row.Band.Index = 0 Then
                '  If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
                Dim _Linea As Entrada_Linea
                _Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(e.Row.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                If _Linea.Producto.Archivo Is Nothing = False AndAlso _Linea.Producto.Archivo.CampoBinario.Length > 0 Then
                    e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(_Linea.Producto.Archivo_Mini.CampoBinario.ToArray)
                End If
                .GRID.DisplayLayout.Override.DefaultRowHeight = 40
                ' Else
                .GRID.DisplayLayout.Override.DefaultRowHeight = 20
                '  End If
                If oEntradaTipo = EnumEntradaTipo.PedidoCompra Or oEntradaTipo = EnumEntradaTipo.PedidoVenta Then
                    If Util.Comprobar_NULL_Per_0_Decimal(e.Row.Cells("CantidadTraspasada").Value) <> 0 Then
                        If e.Row.Cells("Unidad").Value <> e.Row.Cells("CantidadTraspasada").Value Then
                            e.Row.CellAppearance.BackColor = Color.FromArgb(255, 192, 255)
                        Else
                            e.Row.CellAppearance.BackColor = Color.FromArgb(192, 255, 192)
                        End If
                    End If
                End If
            End If
        End With

        'Dim _Linea As Entrada_Linea
        '_Linea = e.Row.ListObject
        'If oEntradaTipo <> EnumEntradaTipo.Inicializacion Then
        '    If IsNothing(e.Row.Cells("CantidadTraspasada").Value) = False And e.Row.Cells("CantidadTraspasada").Value > 0 Then
        '        e.Row.Cells("Producto").Activation = Activation.Disabled
        '        'e.Row.Cells("Producto").Activation = Activation.Disabled
        '    Else
        '        ' e.Row.Cells("CantidadATraspasar").Activation = Activation.AllowEdit
        '        ' e.Row.Cells("Seleccion").Activation = Activation.AllowEdit
        '    End If


        '    If _Linea.ID_Entrada_Linea_Pedido.HasValue = True Then  'Si la línea prové d'una comanda vol dir que estem a un albarà o a una factura. I no es podrà editar.
        '        e.Row.Activation = Activation.NoEdit
        '    End If
        'End If

        ''Les regularitzacions, un cop introduida la línea no es podra canviar el producte
        'If oEntradaTipo = EnumEntradaTipo.Regularizacion Then
        '    If e.Row.IsAddRow = False Then
        '        e.Row.Cells("Producto").Activation = Activation.Disabled
        '    End If
        'End If

        ''Si hi ha un producte i aquest requereix de número de serie i  
        'If _Linea.Producto Is Nothing = False AndAlso _Linea.Producto.RequiereNumeroSerie = True And (oEntradaTipo = EnumEntradaTipo.AlbaranCompra Or oEntradaTipo = EnumEntradaTipo.Regularizacion) Then
        '    e.Row.Cells("Cantidad").Activation = Activation.Disabled
        '    Dim __Linea As Entrada_Linea = e.Row.ListObject
        '    If e.Row.Cells("Cantidad").Value <> _Linea.Entrada_Linea_NS.Where(Function(F) F.ID_Entrada_Linea_NS <> 0).Count Then
        '        Try

        '        Catch ex As Exception
        '            e.Row.Cells("Cantidad").Value = _Linea.Entrada_Linea_NS.Where(Function(F) F.ID_Entrada_Linea_NS <> 0).Count
        '        End Try
        '        ' e.Row.Update()
        '    End If
        'Else
        '    e.Row.Cells("Cantidad").Activation = Activation.AllowEdit
        'End If

    End Sub

    Private Sub GRD_Linea_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Linea.M_ToolGrid_ToolClickBotonsExtras
        Try
            Select Case e.Tool.Key
                Case "RecuperarDatos"
                    If oLinqEntrada.ID_Entrada = 0 Then
                        If Guardar() = False Then
                            Exit Sub
                        End If
                    End If

                    If oLinqEntrada.Entrada_Linea.Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Sólo se puede recuperar los datos una sola vez", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    If Guardar() = False Then
                        Exit Sub
                    End If

                    Util.WaitFormObrir()

                    Dim _Producto As Producto
                    Dim _Linea As Entrada_Linea
                    Dim _Almacen As Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.C_Almacen.Value)).FirstOrDefault
                    For Each _Producto In oDTC.Producto.Where(Function(F) F.Activo = True)
                        Dim _Stock As Decimal = RetornaStock(_Producto.ID_Producto, _Almacen.ID_Almacen)
                        If _Stock <> 0 Then
                            _Linea = New Entrada_Linea
                            _Linea.Almacen = _Almacen
                            _Linea.Unidad = _Stock
                            _Linea.CantidadTraspasada = 0
                            _Linea.FechaEntrada = Me.DT_Alta.Value
                            _Linea.Precio = RetornaStockPreuPonderat(_Producto.ID_Producto, _Almacen.ID_Almacen)
                            _Linea.Producto = _Producto
                            _Linea.StockActivo = False
                            _Linea.Entrada_Linea_NS.AddRange(RetornaNumerosSerie(_Producto.ID_Producto, _Almacen.ID_Almacen))
                            oLinqEntrada.Entrada_Linea.Add(_Linea)
                            oDTC.Entrada_Linea.InsertOnSubmit(_Linea)
                        End If
                    Next
                    oLinqEntrada.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Parcial).FirstOrDefault
                    Me.C_Estado.Value = CInt(EnumEntradaEstado.Parcial)
                    Me.C_Almacen.Enabled = False
                    oDTC.SubmitChanges()

                    Call CargaGrid_Lineas(oLinqEntrada.ID_Entrada)
                    Util.WaitFormTancar()

                Case "VerProducto"
                    With Me.GRD_Linea
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Cells.Exists("ID_Producto") = False Then
                            Exit Sub
                        End If

                        Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
                        frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
                        frm.FormObrir(frmPrincipal, True)
                    End With

                Case "VerAlmacen"
                    With Me.GRD_Linea
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Cells.Exists("ID_Almacen") = False Then
                            Exit Sub
                        End If

                        Dim frm As New frmAlmacen
                        frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Almacen").Value)
                        frm.FormObrir(frmPrincipal, True)
                    End With

                Case "VerTrazabilidad"
                    With Me.GRD_Linea
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Cells.Exists("ID_Producto") = False Then
                            Exit Sub
                        End If

                        Dim frm As New frmProducto_Trazabilidad
                        frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
                        frm.FormObrir(Me, True)
                    End With

                Case "VisualizarFotos"
                    With Me.GRD_Linea
                        'If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
                        '    .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "Visualizar fotos"
                        'Else
                        '    .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos"
                        'End If
                        Call CargaGrid_Lineas(oLinqEntrada.ID_Entrada)
                    End With

                Case "IrAFactura"
                    With Me.GRD_Linea
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Cells.Exists("ID_Entrada_Linea") = False Then
                            Exit Sub
                        End If

                        If IsDBNull(.GRID.Selected.Rows(0).Cells("NumFactura").Value) = False AndAlso IsNumeric(.GRID.Selected.Rows(0).Cells("NumFactura").Value) Then

                        Else
                            Mensaje.Mostrar_Mensaje("Esta línea de albarán no está asociada a ninguna factura", M_Mensaje.Missatge_Modo.INFORMACIO)
                            Exit Sub
                        End If

                        Dim _PantallaAObrir As EnumEntradaTipo
                        Select Case oEntradaTipo
                            Case EnumEntradaTipo.AlbaranVenta
                                _PantallaAObrir = EnumEntradaTipo.FacturaVenta
                            Case EnumEntradaTipo.AlbaranCompra
                                _PantallaAObrir = EnumEntradaTipo.FacturaCompra
                            Case EnumEntradaTipo.DevolucionVenta
                                _PantallaAObrir = EnumEntradaTipo.FacturaVentaRectificativa
                            Case EnumEntradaTipo.DevolucionCompra
                                _PantallaAObrir = EnumEntradaTipo.FacturaCompraRectificativa
                        End Select


                        Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
                        frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Entrada_Factura").Value, _PantallaAObrir)
                        frm.FormObrir(Me)

                    End With

                Case "IrAlAlbaran"
                    With Me.GRD_Linea
                        If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Cells.Exists("ID_Entrada_Linea") = False Then
                            Exit Sub
                        End If

                        Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(.GRID.Selected.Rows(0).Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                        If _Linea.Entrada_Linea_Pedido.Count = 0 Then
                            Mensaje.Mostrar_Mensaje("Esta línea de pedido no está asociada a ningun albarán", M_Mensaje.Missatge_Modo.INFORMACIO)
                            Exit Sub
                        End If

                        Dim _PantallaAObrir As EnumEntradaTipo
                        Select Case oEntradaTipo
                            Case EnumEntradaTipo.PedidoVenta
                                _PantallaAObrir = EnumEntradaTipo.AlbaranVenta
                            Case EnumEntradaTipo.PedidoCompra
                                _PantallaAObrir = EnumEntradaTipo.AlbaranCompra
                        End Select

                        Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
                        frm.Entrada(_Linea.Entrada_Linea_Pedido.FirstOrDefault.ID_Entrada, _PantallaAObrir)
                        frm.FormObrir(Me)

                    End With

                Case "AsistenteTraspasos"
                    With Me.GRD_Linea
                        If oLinqEntrada.ID_Entrada = 0 Then
                            Exit Sub
                        End If

                        If oLinqEntrada.Entrada_Linea.Count > 0 Then
                            Mensaje.Mostrar_Mensaje("Imposible ejecutar el asistente si ya hay lineas introducidas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                            Exit Sub
                        End If

                        Dim frm As New frmEntradaTraspasoEntreAlmacenesAsistente
                        AddHandler frm.AlTancarFormulariTraspas, AddressOf AlTancarFormAsistenteTraspas
                        frm.Entrada(oLinqEntrada, oDTC)
                        frm.FormObrir(Me, False)

                    End With

            End Select

        Catch ex As Exception
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm()
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub C_Producto_RowSelected(sender As Object, e As RowSelectedEventArgs)
        Try
            'If oEntradaTipo = EnumEntradaTipo.Regularizacion Then
            '    Dim _Producto As Producto = Me.C_Instalacion.Value
            '    Dim _IDProducto As Integer = _Producto.ID_Producto
            '    Me.GRD_Linea.GRID.ActiveRow.Cells("CantidadTraspasada").Value = Util.Comprobar_NULL_Per_0_Decimal(BD.RetornaValorSQL("Select Cantidad From C_Almacen_Stock_Agrupado Where ID_Producto=" & _IDProducto & " and ID_Almacen=" & Me.C_Almacen.Value))
            'End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Linea_M_GRID_AfterCellUpdate(sender As Object, e As CellEventArgs) Handles GRD_Linea.M_GRID_AfterCellUpdate

        Dim _Linea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(e.Cell.Row.Cells("ID_Entrada_Linea").Value)).FirstOrDefault


        Select Case e.Cell.Column.Key
            Case "Unidad"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Unidad = Nothing
                Else
                    _Linea.Unidad = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "Precio"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Precio = Nothing
                Else
                    _Linea.Precio = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "Descuento1"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Descuento1 = Nothing
                Else
                    _Linea.Descuento1 = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "Descuento2"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Descuento2 = Nothing
                Else
                    _Linea.Descuento2 = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "Coste"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Coste = Nothing
                Else
                    _Linea.Coste = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "IVA"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.IVA = Nothing
                Else
                    _Linea.IVA = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
            Case "Descripcion"
                If IsDBNull(e.Cell.Value) Then
                    _Linea.Descripcion = Nothing
                Else
                    _Linea.Descripcion = e.Cell.Value
                End If
                Call CalcularPreusLinea(e.Cell.Row)
        End Select

        oDTC.SubmitChanges()
        Call CalcularTotals()

    End Sub

    Private Sub GRD_Linea_M_GRID_MouseDownRow(ByRef sender As UltraGrid, ByRef e As MouseEventArgs) Handles GRD_Linea.M_GRID_MouseDownRow
        Try
            GRD_Linea.GRID.ContextMenuStrip = Nothing
            Dim _UIElement As Infragistics.Win.UIElement
            Dim _Row As UltraGridRow

            If sender.ActiveRow Is Nothing Then Exit Sub
            _UIElement = Me.GRD_Linea.GRID.DisplayLayout.UIElement.ElementFromPoint(New Point(e.X, e.Y))

            If _UIElement Is Nothing = False Then
                _Row = _UIElement.GetContext(GetType(UltraGridRow))
                If _Row Is Nothing = False Then
                    If e.Button = Windows.Forms.MouseButtons.Right Then
                        '_Row.Activate()
                        ' _Row.Selected = True

                        If oLinqEntrada.ID_Entrada_Estado = EnumEntradaEstado.Cerrado And oLinqEntrada.ID_Entrada_Estado = 0 Then
                            Exit Sub
                        End If

                        If oLinqEntrada.ID_Entrada_Tipo <> EnumEntradaTipo.PedidoVenta Then 'només hi haurà botó dret amb els pedidos de venta
                            Exit Sub
                        End If

                        If _Row.Band.Index = 0 Then
                            ContextMenuStrip1.Show(New Point(e.X + 300, e.Y + 160))
                        End If

                        Dim _Prow As UltraGridRow
                        Dim _IDProducto As Integer = _Row.Cells("ID_Producto").Value
                        Dim _DetectatArticleDiferent As Boolean = False
                        For Each _Prow In Me.GRD_Linea.GRID.Selected.Rows
                            If _Prow.Cells("ID_Producto").Value <> _IDProducto Then
                                _DetectatArticleDiferent = True
                            End If
                        Next
                        If _DetectatArticleDiferent = True Then
                            MenuCambiarArticulo.Enabled = False
                        Else
                            MenuCambiarArticulo.Enabled = True
                        End If

                        'Només es podrà cambiar d'article si estem a tal i como se instaló
                        'If oLinqPropuesta.SeInstalo = False Then
                        '    MenuCambiarArticulo.Enabled = False
                        'End If


                        ' ContextMenuStrip1.Show(Me.GRD_Linea, New System.Drawing.Point(e.X, e.Y))
                    End If
                End If
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub MenuCambiarArticulo_Click(sender As Object, e As EventArgs) Handles MenuCambiarArticulo.Click
        Try
            Dim frm As New frmAuxiliarSeleccioLlistatGeneric
            frm.Entrada(frmAuxiliarSeleccioLlistatGeneric.EnumTipusEntrada.SeleccionarProducto)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormCambiarArticle
            frm.FormObrir(Me, False)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CambiarAlmacénToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CambiarAlmacénToolStripMenuItem.Click
        Try
            Dim frm As New frmAuxiliarSeleccioLlistatGeneric
            frm.Entrada(frmAuxiliarSeleccioLlistatGeneric.EnumTipusEntrada.SeleccionarAlmacen)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormCambiarAlmacen
            frm.FormObrir(Me, False)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub AlTancarFormCambiarArticle(ByVal pID As Integer)
        Try
            Dim _pRow As UltraGridRow
            For Each _pRow In Me.GRD_Linea.GRID.Selected.Rows
                Dim _Linea As Entrada_Linea = oLinqEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(_pRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                If _Linea.CantidadTraspasada = 0 Then 'Les línies que s'hagin traspassat no es podran canviar d'article
                    _Linea.Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = pID).FirstOrDefault
                    _Linea.ID_Producto = _Linea.Producto.ID_Producto
                    _Linea.Descripcion = _Linea.Producto.Descripcion
                    _Linea.DescripcionAmpliada = _Linea.Producto.DescripcionAmpliada
                    _pRow.Cells("ID_Producto").Value = _Linea.Producto.ID_Producto
                    '_pRow.Cells("CodigoProducto").Value = _Linea.Producto.Codigo
                    _pRow.Cells("Descripcion").Value = _Linea.Producto.Descripcion
                    _pRow.Cells("DescripcionAmpliada").Value = _Linea.Producto.DescripcionAmpliada
                    _pRow.Update()
                    oDTC.SubmitChanges()
                End If
            Next


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarFormCambiarAlmacen(ByVal pID As Integer)
        Try

            Dim _pRow As UltraGridRow
            For Each _pRow In Me.GRD_Linea.GRID.Selected.Rows
                Dim _Linea As Entrada_Linea = oLinqEntrada.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(_pRow.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                If _Linea.CantidadTraspasada = 0 Then 'Les línies que s'hagin traspassat no es podran canviar d'article
                    _Linea.Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen = pID).FirstOrDefault
                    _pRow.Cells("Almacen_Descripcion").Value = _Linea.Almacen.Descripcion
                    _pRow.Update()
                    oDTC.SubmitChanges()
                End If
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Seguimiento"

    Private Sub CargaGrid_Seguimiento(ByVal pId As Integer)
        Try

            Dim _Seguimiento As IEnumerable(Of Entrada_Seguimiento) = From taula In oDTC.Entrada_Seguimiento Where taula.ID_Entrada = pId Select taula

            With Me.GRD_Seguimiento
                .M.clsUltraGrid.CargarIEnumerable(_Seguimiento)

                .M_Editable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguimiento_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Seguimiento.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Seguimiento

                If oLinqEntrada.ID_Entrada = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Fecha").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Entrada").Value = oLinqEntrada

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguimiento_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Seguimiento.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqEntrada.Entrada_Seguimiento.Remove(e.ListObject)
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

    Private Sub GRD_Seguimiento_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Seguimiento.M_GRID_AfterRowUpdate
        Try

            'If Me.DT_Inicio.Value Is Nothing Then 'si no s'ha introduit la data d'inici, llavors, l'assignarem automàticament al introduir alguna hora.
            '    Me.DT_Inicio.Value = Now.Date
            '    oLinqParte.FechaInicio = Now.Date
            'End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Instalacion"

    Private Sub CargaGrid_Instalacion(ByVal pId As Integer)
        Try
            Dim _EntradaInstalacion As IEnumerable(Of Entrada_Instalacion) = From taula In oDTC.Entrada_Instalacion Where taula.ID_Entrada = pId Select taula

            With Me.GRD_Instalacion
                .M.clsUltraGrid.CargarIEnumerable(_EntradaInstalacion)

                .M_Editable()

                'Call CargarCombo_Instalacion(.GRID, 0)
                Call CalcularNumInstalacions()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Instalacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Instalacion.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Instalacion

                If oLinqEntrada.ID_Entrada = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Entrada").Value = oLinqEntrada

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Instalacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Instalacion.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqEntrada.Entrada_Instalacion.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _Instalacion As Entrada_Instalacion = e.ListObject

            If oLinqEntrada.Entrada_Linea.Where(Function(F) F.ID_Instalacion.HasValue AndAlso F.ID_Instalacion = _Instalacion.ID_Instalacion).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar la instalacion. Esta instalación esta asignada a una línea del documento", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If oDTC.Entrada_Linea_Propuesta_Linea.Where(Function(F) F.Entrada_Linea.ID_Entrada = oLinqEntrada.ID_Entrada And F.Propuesta_Linea.Propuesta.ID_Instalacion = _Instalacion.ID_Instalacion).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar la instalacion. Hay líneas de esta instalación asignadas a una o más líneas del documento", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

            Call CalcularNumInstalacions()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Instalacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Instalacion.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

            Call CalcularNumInstalacions()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Instalacion_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Instalacion.M_Grid_InitializeRow
        Dim _ComboInstalacio As New Infragistics.Win.UltraWinGrid.UltraCombo

        Call CargarCombo_Instalacion(_ComboInstalacio)

        e.Row.Cells("Instalacion").EditorComponent = Nothing
        e.Row.Cells("Instalacion").EditorComponent = _ComboInstalacio
    End Sub

    Private Sub CalcularNumInstalacions()
        If Me.GRD_Instalacion.GRID.Rows.Count = 0 Then
            Me.TAB_Principal.Tabs("Instalaciones").Text = "Instalaciones"
        Else
            Me.TAB_Principal.Tabs("Instalaciones").Text = "Instalaciones (" & Me.GRD_Instalacion.GRID.Rows.Count & ")"
        End If
    End Sub

    Private Sub CargarCombo_Instalacion(ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo, Optional ByVal pCell As UltraGridCell = Nothing)
        Try
            '  DT = From Taula In oDTC.Parte Where Taula.Parte_Asignacion.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 And Taula.Activo = True And Taula.BloquearImputacionHoras = False And (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Cliente = Taula.Cliente.Nombre, Poblacion = Taula.Cliente.Poblacion, Taula.TrabajoARealizar, Parte = Taula
            Dim _LlistatInstalacions As IEnumerable

            If oLinqEntrada.ID_Cliente.HasValue = True Then
                If pCell Is Nothing Then
                    '_LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True And Taula.ID_Cliente = oLinqEntrada.ID_Cliente Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Responsable = Taula.Personal.Nombre, Instalacion = Taula
                    _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Direccion = Taula.Direccion, Responsable = Taula.Personal.Nombre, Instalacion = Taula
                Else
                    Select Case Me.OP_Instalacions.Value
                        Case 0
                            _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True And Taula.ID_Cliente = oLinqEntrada.ID_Cliente Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Direccion = Taula.Direccion, Responsable = Taula.Personal.Nombre, Instalacion = Taula
                        Case 1
                            _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True And Taula.ID_Cliente_Contratante = oLinqEntrada.ID_Cliente Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Direccion = Taula.Direccion, Responsable = Taula.Personal.Nombre, Instalacion = Taula
                        Case 2
                            _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Direccion = Taula.Direccion, Responsable = Taula.Personal.Nombre, Instalacion = Taula
                    End Select
                End If

            Else
                _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True Order By Taula.ID_Instalacion Descending Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Descripcion = Taula.OtrosDetalles, Poblacion = Taula.Poblacion, Direccion = Taula.Direccion, Responsable = Taula.Personal.Nombre, Instalacion = Taula
            End If

                pCombo.DataSource = _LlistatInstalacions
                If _LlistatInstalacions Is Nothing Then
                    Exit Sub
                End If

                With pCombo
                    '.DisplayLayout.Bands(0).Columns.Add("a")
                    '.Rows(0).Cells("a").Value = oDTC.Parte.FirstOrDefault

                    '.AutoCompleteMode = AutoCompleteMode.Suggest
                    '.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
                    .AutoCompleteMode = AutoCompleteMode.None
                    .AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
                    .AlwaysInEditMode = False
                    .DropDownStyle = UltraComboStyle.DropDownList

                    .MaxDropDownItems = 16
                    .DisplayMember = "Visualitzar"
                    .ValueMember = "Instalacion"
                    .DisplayLayout.Bands(0).Columns("ID_Instalacion").Width = 50
                    .DisplayLayout.Bands(0).Columns("ID_Instalacion").Header.Caption = "Código"
                    .DisplayLayout.Bands(0).Columns("ID_Instalacion").Header.Appearance.TextHAlign = HAlign.Right
                    .DisplayLayout.Bands(0).Columns("Descripcion").Width = 200
                    .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
                .DisplayLayout.Bands(0).Columns("Poblacion").Width = 200
                .DisplayLayout.Bands(0).Columns("Poblacion").Header.Caption = "Población"
                .DisplayLayout.Bands(0).Columns("Direccion").Width = 200
                .DisplayLayout.Bands(0).Columns("Direccion").Header.Caption = "Dirección"
                    .DisplayLayout.Bands(0).Columns("Responsable").Width = 200
                    .DisplayLayout.Bands(0).Columns("Responsable").Header.Caption = "Responsable"
                    .DisplayLayout.Bands(0).Columns("Instalacion").Hidden = True
                    .DisplayLayout.Bands(0).Columns("Visualitzar").Hidden = True
                End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Instalacion_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Instalacion.M_ToolGrid_ToolClickBotonsExtras2
        If e.Tool.Key = "IrAInstalacion" Then
            With Me.GRD_Instalacion
                If .GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If

                Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Instalacion").Value)
                frm.FormObrir(frmPrincipal, True)
            End With
        End If
    End Sub

    Private Sub GRD_InstaladoEn_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Instalacion.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "Instalacion"
                Call CargarCombo_Instalacion(e.Cell.EditorComponent, e.Cell)
        End Select
    End Sub

    'Private Sub CargarCombo_Propuesta(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer) ', ByVal pIDInstalacion As Integer
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Propuesta)

    '        oTaula = (From Taula In oDTC.Propuesta Where Taula.SeInstalo = False And Taula.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior Order By Taula.Descripcion Select Taula)


    '        Dim Var As Propuesta

    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Descripcion)
    '        Next

    '        pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Propuesta").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Propuesta").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarCombo_Propuesta(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDInstalacion As Integer)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Propuesta) = (From Taula In oDTC.Propuesta Where Taula.ID_Instalacion = pIDInstalacion And Taula.SeInstalo = False And Taula.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior Order By Taula.Descripcion Select Taula)
    '        Dim Var As Propuesta

    '        'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Descripcion)
    '        Next

    '        'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

    '        pCelda.ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

#End Region

#Region "Grid Propuesta"

    Private Sub CargaGrid_Propuesta(ByVal pId As Integer)
        Try
            Dim _Propuesta As IEnumerable(Of Entrada_Propuesta) = From taula In oDTC.Entrada_Propuesta Where taula.ID_Entrada = pId Select taula

            With Me.GRD_Propuesta
                .M.clsUltraGrid.CargarIEnumerable(_Propuesta)

                .M_Editable()

                Call CalcularNumPropuestas()
                ' Call CargarCombo_Instalacion(.GRID, 0)
                'Call CargarCombo_Propuesta(.GRID, 0)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Propuesta_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Propuesta.M_GRID_BeforeCellActivate
        'Select Case e.Cell.Column.Key
        '    Case "Propuesta"
        '        Call CargarCombo_Propuesta(e.Cell, e.Cell.Row.Cells("ID_Instalacion").Value)
        'End Select
    End Sub

    Private Sub GRD_Propuesta_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Propuesta.M_GRID_CellListSelect
        Try

            If e.Cell.ValueListResolved Is Nothing = False AndAlso CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue Is Nothing Then
                e.Cell.ValueList = Nothing
            End If

            Select Case e.Cell.Column.Key
                Case "Instalacion"
                    e.Cell.Row.Cells("Propuesta").Value = Nothing
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Propuesta_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Propuesta.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Propuesta

                If oLinqEntrada.ID_Entrada = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Entrada").Value = oLinqEntrada

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Propuesta_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Propuesta.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqEntrada.Entrada_Propuesta.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

            Call CalcularNumPropuestas()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Propuesta_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Propuesta.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

            Call CalcularNumPropuestas()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Propuesta_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Propuesta.M_Grid_InitializeRow
        'Dim _ComboInstalacio As New Infragistics.Win.UltraWinGrid.UltraCombo
        Dim _ComboPropuesta As New Infragistics.Win.UltraWinGrid.UltraCombo

        'Call CargarCombo_Instalacion(_ComboInstalacio)
        Call CargarCombo_Propuesta(_ComboPropuesta)

        'e.Row.Cells("Instalacion").EditorComponent = Nothing
        'e.Row.Cells("Instalacion").EditorComponent = _ComboInstalacio
        e.Row.Cells("Propuesta").EditorComponent = Nothing
        e.Row.Cells("Propuesta").EditorComponent = _ComboPropuesta
    End Sub

    Private Sub GRD_Propuesta_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Propuesta.M_ToolGrid_ToolClickBotonsExtras2
        If e.Tool.Key = "IrAPropuesta" Then
            With Me.GRD_Propuesta
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
        End If
    End Sub

    'Private Sub CargarCombo_Instalacion(ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo)
    '    Try
    '        '  DT = From Taula In oDTC.Parte Where Taula.Parte_Asignacion.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 And Taula.Activo = True And Taula.BloquearImputacionHoras = False And (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Cliente = Taula.Cliente.Nombre, Poblacion = Taula.Cliente.Poblacion, Taula.TrabajoARealizar, Parte = Taula
    '        Dim _LlistatInstalacions As IEnumerable

    '        If oLinqEntrada.ID_Cliente.HasValue = True Then
    '            _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True And Taula.ID_Cliente = oLinqEntrada.ID_Cliente Order By Taula.ID_Instalacion Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Taula.OtrosDetalles, Taula.FechaInstalacion, Cliente = Taula.Cliente.NombreComercial, Poblacion = Taula.Poblacion, Instalacion = Taula
    '        Else
    '            _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True Order By Taula.ID_Instalacion Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.OtrosDetalles, Taula.ID_Instalacion, Taula.OtrosDetalles, Taula.FechaInstalacion, Cliente = Taula.Cliente.NombreComercial, Poblacion = Taula.Poblacion, Instalacion = Taula
    '        End If

    '        pCombo.DataSource = _LlistatInstalacions
    '        If _LlistatInstalacions Is Nothing Then
    '            Exit Sub
    '        End If

    '        With pCombo
    '            '.DisplayLayout.Bands(0).Columns.Add("a")
    '            '.Rows(0).Cells("a").Value = oDTC.Parte.FirstOrDefault

    '            '.AutoCompleteMode = AutoCompleteMode.Suggest
    '            '.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
    '            .AutoCompleteMode = AutoCompleteMode.None
    '            .AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
    '            .AlwaysInEditMode = False
    '            .DropDownStyle = UltraComboStyle.DropDownList

    '            .MaxDropDownItems = 16
    '            .DisplayMember = "Visualitzar"
    '            .ValueMember = "Instalacion"
    '            .DisplayLayout.Bands(0).Columns("ID_Instalacion").Width = 50
    '            .DisplayLayout.Bands(0).Columns("ID_Instalacion").Header.Caption = "Código"
    '            .DisplayLayout.Bands(0).Columns("ID_Instalacion").Header.Appearance.TextHAlign = HAlign.Right
    '            .DisplayLayout.Bands(0).Columns("OtrosDetalles").Width = 200
    '            .DisplayLayout.Bands(0).Columns("OtrosDetalles").Header.Caption = "OtrosDetalles"
    '            .DisplayLayout.Bands(0).Columns("FechaInstalacion").Width = 75
    '            .DisplayLayout.Bands(0).Columns("FechaInstalacion").Header.Caption = "Fecha"
    '            .DisplayLayout.Bands(0).Columns("Cliente").Width = 300
    '            .DisplayLayout.Bands(0).Columns("Cliente").Header.Caption = "Cliente"
    '            .DisplayLayout.Bands(0).Columns("Poblacion").Width = 200
    '            .DisplayLayout.Bands(0).Columns("Poblacion").Header.Caption = "Población"
    '            .DisplayLayout.Bands(0).Columns("Instalacion").Hidden = True
    '            .DisplayLayout.Bands(0).Columns("Visualitzar").Hidden = True
    '        End With

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    Private Sub CargarCombo_Propuesta(ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo)
        Try

            Dim _LlistatInstalacionsSeleccionades As IEnumerable(Of Instalacion) = From Taula In oDTC.Entrada_Instalacion Where Taula.ID_Entrada = oLinqEntrada.ID_Entrada Select Taula.Instalacion

            '_LlistatInstalacions = From Taula In oDTC.Entrada_InstalacionPropuesta Order By Taula.ID_Instalacion Select Visualitzar = Taula.ID_Instalacion & " - " & Taula.Instalacion.OtrosDetalles, Taula.ID_Instalacion, Taula.Instalacion.OtrosDetalles, Taula.Instalacion.FechaInstalacion, Cliente = Taula.Instalacion.Cliente.NombreComercial, Poblacion = Taula.Instalacion.Poblacion
            '_LlistatPartes = From Taula In oDTC.Parte Where _LlistatInstalacionsSeleccionades.Contains(Taula.Instalacion) Order By Taula.ID_Parte Group By ID_Parte = Taula.ID_Parte, Visualitzar = Taula.ID_Parte & " - " & Taula.Parte.FechaAlta, TrabajoARealizar = Taula.Parte.TrabajoARealizar, FechaParte = Taula.Parte.FechaAlta Into Group Select ID_Parte, Visualitzar, TrabajoARealizar, FechaParte



            '  DT = From Taula In oDTC.Parte Where Taula.Parte_Asignacion.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 And Taula.Activo = True And Taula.BloquearImputacionHoras = False And (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Cliente = Taula.Cliente.Nombre, Poblacion = Taula.Cliente.Poblacion, Taula.TrabajoARealizar, Parte = Taula
            '   Dim _LlistatPropuesta As IEnumerable = From Taula In oDTC.Propuesta Where Taula.Activo = True And Taula.SeInstalo = False And Taula.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior And Taula.ID_Instalacion = pIDInstalacion Order By Taula.Codigo Select ID_Propuesta = Taula.ID_Propuesta & " - " & Taula.Descripcion, Taula.Codigo, Descripcion = Taula.Descripcion, Taula.Fecha, Propuesta = Taula
            Dim _LlistatPropuesta As IEnumerable = From Taula In oDTC.Propuesta Where _LlistatInstalacionsSeleccionades.Contains(Taula.Instalacion) And Taula.Activo = True And Taula.SeInstalo = False And Taula.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior Order By Taula.ID_Propuesta Descending Select ID_Propuesta = Taula.Codigo & " - " & Taula.Descripcion, Taula.Codigo, Descripcion = Taula.Descripcion, Taula.Fecha, Propuesta = Taula

            'If oLinqEntrada.ID_Cliente.HasValue = True Then
            '    _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True And Taula.ID_Cliente = oLinqEntrada.ID_Cliente Order By Taula.ID_Instalacion Select Taula.ID_Instalacion, Taula.FechaInstalacion, Cliente = Taula.Cliente.NombreComercial, Poblacion = Taula.Poblacion, Instalacion = Taula
            'Else
            '    _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True Order By Taula.ID_Instalacion Select Taula.ID_Instalacion, Taula.FechaInstalacion, Cliente = Taula.Cliente.NombreComercial, Poblacion = Taula.Poblacion, Instalacion = Taula
            'End If

            pCombo.DataSource = _LlistatPropuesta
            If _LlistatPropuesta Is Nothing Then
                Exit Sub
            End If

            With pCombo
                '.DisplayLayout.Bands(0).Columns.Add("a")
                '.Rows(0).Cells("a").Value = oDTC.Parte.FirstOrDefault

                '.AutoCompleteMode = AutoCompleteMode.Suggest
                '.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
                .AutoCompleteMode = AutoCompleteMode.None
                .AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
                .AlwaysInEditMode = False
                .DropDownStyle = UltraComboStyle.DropDownList

                .MaxDropDownItems = 16
                .DisplayMember = "ID_Propuesta"
                .ValueMember = "Propuesta"
                .DisplayLayout.Bands(0).Columns("Codigo").Width = 50
                .DisplayLayout.Bands(0).Columns("Codigo").Header.Caption = "Código"
                .DisplayLayout.Bands(0).Columns("Codigo").Header.Appearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("Fecha").Width = 75
                .DisplayLayout.Bands(0).Columns("Fecha").Header.Caption = "Fecha"
                .DisplayLayout.Bands(0).Columns("Descripcion").Width = 250
                .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
                .DisplayLayout.Bands(0).Columns("Propuesta").Hidden = True
                .DisplayLayout.Bands(0).Columns("ID_Propuesta").Hidden = True
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    'Private Sub CargarCombo_Propuesta(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer) ', ByVal pIDInstalacion As Integer
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Propuesta)

    '        oTaula = (From Taula In oDTC.Propuesta Where Taula.SeInstalo = False And Taula.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior Order By Taula.Descripcion Select Taula)


    '        Dim Var As Propuesta

    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Descripcion)
    '        Next

    '        pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Propuesta").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Propuesta").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarCombo_Propuesta(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDInstalacion As Integer)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Propuesta) = (From Taula In oDTC.Propuesta Where Taula.ID_Instalacion = pIDInstalacion And Taula.SeInstalo = False And Taula.ID_Propuesta_Tipo <> EnumPropuestaTipo.InstalacionAnterior Order By Taula.Descripcion Select Taula)
    '        Dim Var As Propuesta

    '        'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Descripcion)
    '        Next

    '        'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

    '        pCelda.ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub CalcularNumPropuestas()
        If Me.GRD_Propuesta.GRID.Rows.Count = 0 Then
            Me.TAB_Principal.Tabs("Propuestas").Text = "Propuestas"
        Else
            Me.TAB_Principal.Tabs("Propuestas").Text = "Propuestas (" & Me.GRD_Propuesta.GRID.Rows.Count & ")"
        End If
    End Sub

#End Region

#Region "Grid Partes"

    Private Sub CargaGrid_Partes(ByVal pId As Integer)
        Try
            Dim _EntradaParte As IEnumerable(Of Entrada_Parte) = From taula In oDTC.Entrada_Parte Where taula.ID_Entrada = pId Select taula

            With Me.GRD_Partes
                .M.clsUltraGrid.CargarIEnumerable(_EntradaParte)

                .M_Editable()

                ' Call CargarCombo_Instalacion(.GRID, 0)
                'Call CargarCombo_Parte(.GRID, 0)

                Call CalcularNumPartes()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub GRD_Partes_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Partes.M_GRID_BeforeCellActivate
    '    Select Case e.Cell.Column.Key
    '        Case "Parte"
    '            'Call CargarCombo_Parte(e.Cell, e.Cell.Row.Cells("ID_Instalacion").Value)
    '    End Select
    'End Sub

    'Private Sub GRD_Partes_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Partes.M_GRID_CellListSelect
    '    Try

    '        If e.Cell.ValueListResolved Is Nothing = False AndAlso CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue Is Nothing Then
    '            e.Cell.ValueList = Nothing
    '        End If

    '        Select Case e.Cell.Column.Key
    '            Case "Instalacion"
    '                e.Cell.Row.Cells("Parte").Value = Nothing
    '        End Select

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub GRD_Partes_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Partes.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Partes

                If oLinqEntrada.ID_Entrada = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Entrada").Value = oLinqEntrada

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Partes_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Partes.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqEntrada.Entrada_Parte.Remove(e.ListObject)
                Exit Sub
            End If

            ' Dim _Parte As Entrada_Parte = e.ListObject

            'If oDTC.Parte_Horas.Where(Function(F) F.Entrada_Linea.Entrada.ID_Entrada = oLinqEntrada.ID_Entrada And F.ID_Parte = _Parte.ID_Parte).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible eliminar el parte. Hay horas asignadas en una o más lineas del documento", M_Mensaje.Missatge_Modo.INFORMACIO, "")
            '    Exit Sub
            'End If

            'If oDTC.Parte_Horas.Where(Function(F) F.Entrada_Linea.Entrada.ID_Entrada = oLinqEntrada.ID_Entrada And F.ID_Parte = _Parte.ID_Parte).Count > 0 Then
            '    Mensaje.Mostrar_Mensaje("Imposible eliminar el parte. Hay horas asignadas en una o más lineas del documento", M_Mensaje.Missatge_Modo.INFORMACIO, "")
            '    Exit Sub
            'End If


            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

            Call CalcularNumPartes()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Partes_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Partes.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

            Call CalcularNumPartes()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Partes_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Partes.M_Grid_InitializeRow
        Dim _ComboInstalacio As New Infragistics.Win.UltraWinGrid.UltraCombo
        Dim _ComboParte As New Infragistics.Win.UltraWinGrid.UltraCombo

        ' Call CargarCombo_Instalacion(_ComboInstalacio)
        Call CargarCombo_Parte(_ComboParte)

        'e.Row.Cells("Instalacion").EditorComponent = Nothing
        'e.Row.Cells("Instalacion").EditorComponent = _ComboInstalacio
        e.Row.Cells("Parte").EditorComponent = Nothing
        e.Row.Cells("Parte").EditorComponent = _ComboParte
    End Sub

    'Private Sub CargarCombo_Parte(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer) ', ByVal pIDInstalacion As Integer
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Parte)

    '        oTaula = (From Taula In oDTC.Parte Where Taula.Activo = True Order By Taula.ID_Parte Select Taula)


    '        Dim Var As Parte

    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.ID_Parte)
    '        Next

    '        pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Parte").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Parte").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarCombo_Parte(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDInstalacion As Integer)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Parte) = (From Taula In oDTC.Parte Where Taula.ID_Instalacion = pIDInstalacion Order By Taula.ID_Parte Select Taula)
    '        Dim Var As Parte

    '        'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.ID_Parte)
    '        Next

    '        'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

    '        pCelda.ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub GRD_Partes_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Partes.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "Parte"
                Call CargarCombo_Parte(e.Cell.EditorComponent, e.Cell)
        End Select
    End Sub

    Private Sub CargarCombo_Parte(ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo, Optional ByVal pCell As UltraGridCell = Nothing)
        Try
            Dim _LlistatInstalacionsSeleccionades As IEnumerable(Of Instalacion) = From Taula In oDTC.Entrada_Instalacion Where Taula.ID_Entrada = oLinqEntrada.ID_Entrada Select Taula.Instalacion



            '  DT = From Taula In oDTC.Parte Where Taula.Parte_Asignacion.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 And Taula.Activo = True And Taula.BloquearImputacionHoras = False And (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Cliente = Taula.Cliente.Nombre, Poblacion = Taula.Cliente.Poblacion, Taula.TrabajoARealizar, Parte = Taula
            Dim _LlistatParte As IEnumerable


            'If oLinqEntrada.ID_Cliente.HasValue = True Then
            '    _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True And Taula.ID_Cliente = oLinqEntrada.ID_Cliente Order By Taula.ID_Instalacion Select Taula.ID_Instalacion, Taula.FechaInstalacion, Cliente = Taula.Cliente.NombreComercial, Poblacion = Taula.Poblacion, Instalacion = Taula
            'Else
            '    _LlistatInstalacions = From Taula In oDTC.Instalacion Where Taula.Activo = True Order By Taula.ID_Instalacion Select Taula.ID_Instalacion, Taula.FechaInstalacion, Cliente = Taula.Cliente.NombreComercial, Poblacion = Taula.Poblacion, Instalacion = Taula
            'End If


            If oLinqEntrada.ID_Cliente.HasValue = True Then
                If pCell Is Nothing Then
                    _LlistatParte = From Taula In oDTC.Parte Where (_LlistatInstalacionsSeleccionades.Contains(Taula.Instalacion) Or Taula.ID_Cliente = oLinqEntrada.ID_Cliente) And Taula.Activo = True Order By Taula.ID_Parte Descending Select ID_Parte = Taula.ID_Parte & " - " & Taula.TrabajoARealizar, Taula.TrabajoARealizar, Taula.FechaAlta, Tipo = Taula.Parte_Tipo.Descripcion, Poblacion = Taula.Poblacion, Parte = Taula
                Else
                    Select Case Me.OP_Partes.Value
                        Case 0
                            _LlistatParte = From Taula In oDTC.Parte Where (_LlistatInstalacionsSeleccionades.Contains(Taula.Instalacion)) And Taula.Activo = True And (Taula.ID_Parte_Estado = EnumParteEstado.EnCurso Or Taula.ID_Parte_Estado = EnumParteEstado.Pendiente) Order By Taula.ID_Parte Descending Select ID_Parte = Taula.ID_Parte & " - " & Taula.TrabajoARealizar, Taula.TrabajoARealizar, Taula.FechaAlta, Tipo = Taula.Parte_Tipo.Descripcion, Poblacion = Taula.Poblacion, Parte = Taula
                        Case 1
                            _LlistatParte = From Taula In oDTC.Parte Where (_LlistatInstalacionsSeleccionades.Contains(Taula.Instalacion)) And Taula.Activo = True Order By Taula.ID_Parte Descending Select ID_Parte = Taula.ID_Parte & " - " & Taula.TrabajoARealizar, Taula.TrabajoARealizar, Taula.FechaAlta, Tipo = Taula.Parte_Tipo.Descripcion, Poblacion = Taula.Poblacion, Parte = Taula
                        Case 2
                            _LlistatParte = From Taula In oDTC.Parte Where (Taula.ID_Cliente = oLinqEntrada.ID_Cliente) And Taula.Activo = True Order By Taula.ID_Parte Descending Select ID_Parte = Taula.ID_Parte & " - " & Taula.TrabajoARealizar, Taula.TrabajoARealizar, Taula.FechaAlta, Tipo = Taula.Parte_Tipo.Descripcion, Poblacion = Taula.Poblacion, Parte = Taula
                    End Select
                End If
            End If

            pCombo.DataSource = _LlistatParte
            If _LlistatParte Is Nothing Then
                Exit Sub
            End If

            With pCombo
                '.DisplayLayout.Bands(0).Columns.Add("a")
                '.Rows(0).Cells("a").Value = oDTC.Parte.FirstOrDefault

                '.AutoCompleteMode = AutoCompleteMode.Suggest
                '.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
                .AutoCompleteMode = AutoCompleteMode.None
                .AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
                .AlwaysInEditMode = False
                .DropDownStyle = UltraComboStyle.DropDownList

                .MaxDropDownItems = 16
                .DisplayMember = "ID_Parte"
                .ValueMember = "Parte"
                .DisplayLayout.Bands(0).Columns("ID_Parte").Width = 50
                .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Caption = "Código"
                .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Appearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Width = 200
                .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Header.Caption = "Descripcion"
                .DisplayLayout.Bands(0).Columns("FechaAlta").Width = 75
                .DisplayLayout.Bands(0).Columns("FechaAlta").Header.Caption = "Fecha"
                .DisplayLayout.Bands(0).Columns("Tipo").Width = 150
                .DisplayLayout.Bands(0).Columns("Tipo").Header.Caption = "Tipo"
                .DisplayLayout.Bands(0).Columns("Poblacion").Width = 200
                .DisplayLayout.Bands(0).Columns("Poblacion").Header.Caption = "Población"
                .DisplayLayout.Bands(0).Columns("Parte").Hidden = True
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Partes_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Partes.M_ToolGrid_ToolClickBotonsExtras2
        If e.Tool.Key = "IrAParte" Then
            With Me.GRD_Partes
                If .GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If
                '  Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                Dim frm As New frmParte
                frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
                frm.FormObrir(frmPrincipal, True)
            End With
        End If
    End Sub

    Private Sub CalcularNumPartes()
        If GRD_Partes.GRID.Rows.Count = 0 Then
            Me.TAB_Principal.Tabs("Partes").Text = "Partes"
        Else
            Me.TAB_Principal.Tabs("Partes").Text = "Partes (" & GRD_Partes.GRID.Rows.Count & ")"
        End If
    End Sub
#End Region

#Region "Grid Documentos Anteriores del mismo tipo"

    Private Sub CargaGrid_DocumentosAnteriores()
        Try

            Me.GRD_DocumentosAnteriores.GRID.DataSource = Nothing
            If oLinqEntrada.ID_Cliente.HasValue = False And oLinqEntrada.ID_Proveedor.HasValue = False Then
                Exit Sub
            End If

            Dim _Filtre As String

            If oLinqEntrada.ID_Cliente.HasValue = True Then
                _Filtre = " ID_Cliente=" & oLinqEntrada.ID_Cliente
            Else
                _Filtre = " ID_Proveedor=" & oLinqEntrada.ID_Proveedor
            End If



            With Me.GRD_DocumentosAnteriores
                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_Entrada Where " & _Filtre & " and ID_Entrada_Tipo=" & oLinqEntrada.ID_Entrada_Tipo & "  and ID_Entrada<> " & oLinqEntrada.ID_Entrada & " Order by FechaEntrada Desc"))   'and FechaEntrada<='" & oLinqEntrada.FechaEntrada & "'

                .M_NoEditable()

                ' Call CargarCombo_Instalacion(.GRID, 0)
                'Call CargarCombo_Parte(.GRID, 0)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_DocumentosAnteriores_M_GRID_DoubleClickRow(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_DocumentosAnteriores.M_GRID_DoubleClickRow


        Dim pRow As UltraGridRow = Me.GRD_DocumentosAnteriores.GRID.ActiveRow
        Dim _IDEntrada As Integer = pRow.Cells("ID_Entrada").Value
        Dim _IDTipoDocumento As Integer = pRow.Cells("ID_Entrada_Tipo").Value

        Dim frm As New frmEntrada
        frm.Entrada(_IDEntrada, _IDTipoDocumento)
        frm.FormObrir(Me, True)
    End Sub

#End Region

#Region "Grid Documentos vinculados"

    Private Sub CargaGrid_DocumentosVinculados()
        Try

            Dim _Filtre As String

            Select Case DirectCast(oLinqEntrada.ID_Entrada_Tipo, EnumEntradaTipo)
                Case EnumEntradaTipo.PedidoCompra, EnumEntradaTipo.PedidoVenta
                    _Filtre = "(Select ID_Entrada From Entrada_Linea Where ID_Entrada_Linea_Pedido in (Select ID_Entrada_Linea From Entrada_Linea Where ID_Entrada=" & oLinqEntrada.ID_Entrada & ") Group By ID_Entrada)"
                Case EnumEntradaTipo.AlbaranVenta, EnumEntradaTipo.AlbaranCompra
                    _Filtre = "(Select B.ID_Entrada From (Select (Select ID_Entrada From Entrada_Linea as A Where Entrada_Linea.ID_Entrada_Linea_Pedido=A.ID_Entrada_Linea) as ID_Entrada From Entrada_Linea Where ID_Entrada_Linea_Pedido is not null and ID_Entrada=" & oLinqEntrada.ID_Entrada & ") as B Group By B.ID_Entrada)"
                Case EnumEntradaTipo.FacturaVenta, EnumEntradaTipo.FacturaCompra, EnumEntradaTipo.FacturaVentaRectificativa, EnumEntradaTipo.FacturaCompraRectificativa
                    _Filtre = "(Select ID_Entrada From Entrada_Linea Where ID_Entrada_Factura=" & oLinqEntrada.ID_Entrada & " Group By ID_Entrada)"
                Case Else
                    Exit Sub
            End Select

            With Me.GRD_DocumentosVinculados

                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From C_Entrada Where C_Entrada.ID_Entrada In  " & _Filtre & " Order by FechaEntrada Desc"))

                .M_NoEditable()

                ' Call CargarCombo_Instalacion(.GRID, 0)
                'Call CargarCombo_Parte(.GRID, 0)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_DocumentosVinculados_M_GRID_DoubleClickRow(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_DocumentosVinculados.M_GRID_DoubleClickRow

        Dim pRow As UltraGridRow = Me.GRD_DocumentosVinculados.GRID.ActiveRow
        Dim _IDEntrada As Integer = pRow.Cells("ID_Entrada").Value
        Dim _IDTipoDocumento As Integer = pRow.Cells("ID_Entrada_Tipo").Value

        Dim frm As New frmEntrada
        frm.Entrada(_IDEntrada, _IDTipoDocumento)
        frm.FormObrir(Me, True)

    End Sub

#End Region

#Region "Grid cambios en documentos"
    Private Sub CargaGrid_CambiosEnDocumento()
        Try

            With Me.GRD_CambiosEnDocumento

                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select * From LogTransacciones Where Tabla='Entrada' and PK= " & oLinqEntrada.ID_Entrada & " Order by FechaTrn Desc"))

                .M_NoEditable()

                ' Call CargarCombo_Instalacion(.GRID, 0)
                'Call CargarCombo_Parte(.GRID, 0)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Vencimientos"

    Private Sub CargaGrid_Vencimientos(ByVal pId As Integer)
        Try
            Dim _Vencimientos As IEnumerable(Of Entrada_Vencimiento) = From taula In oDTC.Entrada_Vencimiento Where taula.ID_Entrada = pId Select taula

            With Me.GRD_Vencimientos
                .M.clsUltraGrid.CargarIEnumerable(_Vencimientos)
                .M_Editable()
                Call CargarCombo_Empresa_CuentasBancarias(.GRID)
                Call CargarCombo_Cliente_CuentasBancarias(.GRID)
                Call CargarCombo_Proveedor_CuentasBancarias(.GRID)
                Call CargarCombo_FormaPago(.GRID)
                Call CargarCombo_VencimientoEstado(.GRID)

                .GRID.DisplayLayout.Bands(0).Columns("Entrada_Vencimiento_Estado").CellActivation = Activation.Disabled
                .GRID.DisplayLayout.Bands(0).Columns("Domiciliacion").CellActivation = Activation.Disabled
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Vencimientos_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Vencimientos.M_GRID_CellListSelect
        Try

            If e.Cell.ValueListResolved Is Nothing = False AndAlso CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue Is Nothing Then
                e.Cell.ValueList = Nothing
            End If

            Select Case e.Cell.Column.Key
                Case "FormaPago"

                    If oEntradaTipo <> EnumEntradaTipo.FacturaVenta And oEntradaTipo <> EnumEntradaTipo.FacturaVentaRectificativa Then
                        Exit Sub
                    End If

                    Dim _FormaPagament As FormaPago = CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue

                    If _FormaPagament.FormaPago_Tipo.GenerarDomiciliacion = True And oLinqEntrada.Cliente.Cliente_CuentaBancaria.Where(Function(F) F.Domiciliacion = True).Count = 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible asignar una forma de pago con domiciliación, el cliente no tiene ninguna cuenta bancaria de este tipo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        e.Cell.CancelUpdate()
                        Exit Sub
                    End If


                    If _FormaPagament.FormaPago_Tipo.GenerarDomiciliacion = True Then
                        e.Cell.Row.Cells("Domiciliacion").Value = True
                        e.Cell.Row.Cells("Entrada_Vencimiento_Estado").Value = oDTC.Entrada_Vencimiento_Estado.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.PendienteAsignarARemesa).FirstOrDefault
                    Else
                        e.Cell.Row.Cells("Domiciliacion").Value = False
                        e.Cell.Row.Cells("Entrada_Vencimiento_Estado").Value = Nothing
                    End If

            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Vencimientos_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Vencimientos.M_Grid_InitializeRow
        With Me.GRD_Linea
            If e.Row.Band.Index = 0 Then
                Dim _Venciment As Entrada_Vencimiento = e.Row.ListObject
                If _Venciment.Pagado Then
                    e.Row.Activation = Activation.Disabled
                    Exit Sub
                End If

                If _Venciment.ID_Remesa.HasValue = True Then 'si està assignat a una remesa
                    e.Row.Cells("Fecha").Activation = Activation.Disabled
                    e.Row.Cells("Importe").Activation = Activation.Disabled
                    e.Row.Cells("Pagado").Activation = Activation.Disabled
                    e.Row.Cells("Empresa_CuentaBancaria").Activation = Activation.Disabled
                    e.Row.Cells("Fecha").Activation = Activation.Disabled
                    e.Row.Cells("Cliente_CuentaBancaria").Activation = Activation.Disabled
                    e.Row.Cells("FormaPago").Activation = Activation.Disabled
                Else
                    e.Row.Cells("Fecha").Activation = Activation.AllowEdit
                    e.Row.Cells("Importe").Activation = Activation.AllowEdit
                    If _Venciment.Domiciliacion = True Then
                        e.Row.Cells("Pagado").Activation = Activation.Disabled
                    Else
                        e.Row.Cells("Pagado").Activation = Activation.AllowEdit
                    End If

                    e.Row.Cells("Empresa_CuentaBancaria").Activation = Activation.AllowEdit
                    e.Row.Cells("Fecha").Activation = Activation.AllowEdit
                    e.Row.Cells("Cliente_CuentaBancaria").Activation = Activation.AllowEdit
                    e.Row.Cells("FormaPago").Activation = Activation.AllowEdit
                End If


                'Dim _Linea As Entrada_Linea
                '_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = CInt(e.Row.Cells("ID_Entrada_Linea").Value)).FirstOrDefault
                'If _Linea.Producto.Archivo Is Nothing = False AndAlso _Linea.Producto.Archivo.CampoBinario.Length > 0 Then
                '    e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(_Linea.Producto.Archivo.CampoBinario.ToArray)
                'End If
                '.GRID.DisplayLayout.Override.DefaultRowHeight = 40
                '' Else
                '.GRID.DisplayLayout.Override.DefaultRowHeight = 20
                ''  End If
                'If oEntradaTipo = EnumEntradaTipo.PedidoCompra Or oEntradaTipo = EnumEntradaTipo.PedidoVenta Then
                '    If Util.Comprobar_NULL_Per_0_Decimal(e.Row.Cells("CantidadTraspasada").Value) <> 0 Then
                '        If e.Row.Cells("Unidad").Value <> e.Row.Cells("CantidadTraspasada").Value Then
                '            e.Row.CellAppearance.BackColor = Color.FromArgb(255, 192, 255)
                '        Else
                '            e.Row.CellAppearance.BackColor = Color.FromArgb(192, 255, 192)
                '        End If
                '    End If
                'End If
            End If
        End With
    End Sub

    Private Sub GRD_Vencimientos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Vencimientos.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Vencimientos

                If oLinqEntrada.ID_Entrada = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Entrada").Value = oLinqEntrada

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Vencimientos_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Vencimientos.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqEntrada.Entrada_Vencimiento.Remove(e.ListObject)
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

    Private Sub GRD_Vencimientos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Vencimientos.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Empresa_CuentasBancarias(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Empresa_CuentaBancaria) = (From Taula In oDTC.Empresa_CuentaBancaria Where Taula.Domiciliacion = True Order By Taula.NombreBanco Select Taula)
            Dim Var As Empresa_CuentaBancaria

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.NombreBanco & " - " & Var.NumeroCuenta)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Empresa_CuentaBancaria").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Empresa_CuentaBancaria").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Cliente_CuentasBancarias(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Cliente_CuentaBancaria) = (From Taula In oDTC.Cliente_CuentaBancaria Where Taula.Domiciliacion = True And Taula.Cliente Is oLinqEntrada.Cliente Order By Taula.NombreBanco Select Taula)
            Dim Var As Cliente_CuentaBancaria

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.NombreBanco & " - " & Var.NumeroCuenta)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Cliente_CuentaBancaria").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Cliente_CuentaBancaria").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Proveedor_CuentasBancarias(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Proveedor_CuentaBancaria) = (From Taula In oDTC.Proveedor_CuentaBancaria Where Taula.Domiciliacion = True And Taula.Proveedor Is oLinqEntrada.Proveedor Order By Taula.NombreBanco Select Taula)
            Dim Var As Proveedor_CuentaBancaria

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.NombreBanco & " - " & Var.NumeroCuenta)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Proveedor_CuentaBancaria").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Proveedor_CuentaBancaria").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_FormaPago(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of FormaPago) = (From Taula In oDTC.FormaPago Where Taula.Activo = True Order By Taula.Codigo Select Taula)
            Dim Var As FormaPago

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("FormaPago").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("FormaPago").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_VencimientoEstado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Entrada_Vencimiento_Estado) = (From Taula In oDTC.Entrada_Vencimiento_Estado Order By Taula.ID_Entrada_Vencimiento_Estado Select Taula)
            Dim Var As Entrada_Vencimiento_Estado

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Entrada_Vencimiento_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Entrada_Vencimiento_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Ficheros"

    Private Sub GRD_Ficheros_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Ficheros.M_GRID_ClickRow2
        Try
            Util.WaitFormObrir()
            Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
            Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "Previsualizacion")
            'Me.Preview_UltraTab.Tabs("Previsualizacion").Selected = True

            Dim _IDArchivo As Integer = e.Cells("ID_Archivo").Value
            Dim _Archivo As Archivo
            _Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = _IDArchivo).FirstOrDefault

            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".pdf") Then
                Dim _stream As New System.IO.MemoryStream(_Archivo.CampoBinario.ToArray)
                Preview_PdfViewer.LoadDocument(_stream)
                'Me.Preview_UltraTab.Tabs("PDF").Selected = True
                Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
                Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "PDF")

            End If


            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".doc") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".docx") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".txt") Then
                Dim _stream As New System.IO.MemoryStream(_Archivo.CampoBinario.ToArray)
                If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".txt") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".csv") Then
                    Preview_RTF.RichText.LoadDocument(_stream, DevExpress.XtraRichEdit.DocumentFormat.PlainText)
                End If

                If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".doc") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".docx") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xml") Then
                    Preview_RTF.RichText.LoadDocument(_stream, DevExpress.XtraRichEdit.DocumentFormat.OpenXml)
                End If
                ' Me.Preview_UltraTab.Tabs("Word").Selected = True
                Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
                Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "Word")
                Me.Preview_RTF.Tag = _Archivo.ID_Archivo

                ' Me.Preview_RTF.BotoGuardar.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
            End If


            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xls") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xlsx") Then
                Dim _stream As New System.IO.MemoryStream(_Archivo.CampoBinario.ToArray)

                If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xls") Then
                    Preview_Excel.M_LoadDocumentXLS(_Archivo.CampoBinario)
                End If
                If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xlsx") Then
                    Preview_Excel.M_LoadDocumentXLSX(_Archivo.CampoBinario)
                End If

                Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
                Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "Excel")
                'Me.Preview_UltraTab.Tabs("Excel").Selected = True
                Me.Preview_Excel.Tag = _Archivo.ID_Archivo

                Me.Preview_Excel.RibbonControl1.Items("BotoGuardar").Visibility = DevExpress.XtraBars.BarItemVisibility.Always


            End If

            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".tiff") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".png") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".jpg") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".jpeg") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".gif") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".bmp") Then
                Dim _stream As New System.IO.MemoryStream(_Archivo.CampoBinario.ToArray)

                Me.Preview_Picture.Properties.PictureStoreMode = DevExpress.XtraEditors.Controls.PictureStoreMode.ByteArray
                Me.Preview_Picture.EditValue = _Archivo.CampoBinario.ToArray

                Util.Tab_InVisible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.TODOS)
                Util.Tab_Visible_x_Key(Me.Preview_UltraTab, M_Util.Enum_Tab_Activacion.ALGUNOS, "Foto")

                'Me.Preview_UltraTab.Tabs("Foto").Selected = True
            End If
            Util.WaitFormTancar()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlApretarElBotoGuardarDelExcel(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs)
        If e.Item.Name = "BotoGuardar" Then
            If Me.Preview_Excel.Tag Is Nothing Then
                Exit Sub
            End If
            Dim _Archivo As Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = CInt(Me.Preview_Excel.Tag)).FirstOrDefault

            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xls") Then
                _Archivo.CampoBinario = Me.Preview_Excel.M_SaveDocumentXLS.ToArray
            End If
            If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xlsx") Then
                _Archivo.CampoBinario = Me.Preview_Excel.M_SaveDocumentXLSX.ToArray
            End If

            oDTC.SubmitChanges()
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
        End If

    End Sub

    Public Sub AlApretarElBotoGuardarDelWord() Handles Preview_RTF.EventAlApretarElBotoGuardar

        If Me.Preview_RTF.Tag Is Nothing Then
            Exit Sub
        End If
        Dim _Archivo As Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = CInt(Me.Preview_RTF.Tag)).FirstOrDefault



        Dim _Resultat As New System.IO.MemoryStream
        If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".txt") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".csv") Then
            Me.Preview_RTF.RichText.Document.SaveDocument(_Resultat, DevExpress.XtraRichEdit.DocumentFormat.PlainText)
            _Archivo.CampoBinario = _Resultat.ToArray
        End If

        If _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".doc") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".docx") Or _Archivo.Ruta_Fichero.ToString.ToLower.Contains(".xml") Then
            Me.Preview_RTF.RichText.Document.SaveDocument(_Resultat, DevExpress.XtraRichEdit.DocumentFormat.OpenXml)
            _Archivo.CampoBinario = _Resultat.ToArray
        End If

        oDTC.SubmitChanges()
        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)

    End Sub
#End Region

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()

                If oDTC Is Nothing = False Then
                    oDTC.Dispose()
                End If

                If Fichero Is Nothing = False Then
                    Fichero.Dispose()
                End If

                oDTC = Nothing
                oLinqEntrada = Nothing
                Fichero = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

End Class