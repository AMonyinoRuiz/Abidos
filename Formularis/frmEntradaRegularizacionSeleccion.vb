Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmEntradaRegularizacionSeleccion
    Dim oDTC As DTCDataContext
    Dim oLinea As Entrada_Linea
    Dim RowsSeleccionadas As New ArrayList



#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If RowsSeleccionadas.Count = 0 Then
            Mensaje.Mostrar_Mensaje("Error. Tiene que haber almenos una línea seleccionada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If
        If Mensaje.Mostrar_Mensaje("¿Desea traspasar la propuesta al estado traspasado y traspasar las líneas seleccionadas?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
            oLinqPropuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Traspasado

            If oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).Count = 0 Then 'si no hi ha cap proposta amb seinstalo s'ha de crear
                Call CrearPropuestaSeInstalo(oLinqPropuesta.ID_Propuesta)
            End If

            Call CrearLineasPropuestaSeInstalo(oLinqPropuesta.ID_Propuesta)

            'assignem la propuesta que estem creant a la taula instalacion. Per tenir una relació única entre la instalació i la propuesta de tipus tal y como se instalo
            Dim PropuestaSeInstalo As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
            oLinqInstalacion.ID_Propuesta = PropuestaSeInstalo.ID_Propuesta

            oDTC.SubmitChanges()

            Mensaje.Mostrar_Mensaje("Propuesta traspasada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
        Else
            Exit Sub
        End If

        Call M_ToolForm1_m_ToolForm_Sortir()
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pDTC As DTCDataContext, ByRef pLinea As Entrada_Linea)

        Try

            Me.AplicarDisseny()

            oLinea = pLinea

            oDTC = pDTC
            'oDTC = New DTCDataContext(BD.Conexion)
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Seleccionar"

            Call CargaGrid_Lineas()

            ' Me.GRD_Lineas.GRID.ActiveRow = Nothing
            ' Me.GRD_Lineas.GRID.Selected.Rows.Clear()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Private Sub CreaLineasPropuestaSeInstalo(ByVal pLinea As Propuesta_Linea, ByRef pPropuestaSeInstalo As Propuesta)
        Dim _Linea As Propuesta_Linea
        _Linea = pLinea

        Dim pRow As UltraGridRow
        Dim _Trobat As Boolean = False
        'Aquest bucle es per no traspasar les línies que no estan seleccionades
        For Each pRow In RowsSeleccionadas
            If pRow.Cells("ID_Propuesta_Linea").Value = pLinea.ID_Propuesta_Linea Then
                _Trobat = True
                Exit For
            End If
        Next

        If _Trobat = False Then
            Exit Sub
        End If

        'For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso <> EnumProductoSubFamiliaTraspaso.No).OrderBy(Function(F) F.ID_Propuesta_Linea_Vinculado)
        If _Linea.Unidad > 0 Then
            For i = 1 To _Linea.Unidad
                Dim DescartarLinea As Boolean = False
                Dim _NewLinea As New Propuesta_Linea
                With _NewLinea
                    .Descripcion = _Linea.Descripcion
                    .ID_Instalacion_ElementosAProteger = _Linea.ID_Instalacion_ElementosAProteger
                    .ID_Instalacion_Emplazamiento = _Linea.ID_Instalacion_Emplazamiento
                    .ID_Instalacion_Emplazamiento_Abertura = _Linea.ID_Instalacion_Emplazamiento_Abertura
                    .ID_Instalacion_Emplazamiento_Planta = _Linea.ID_Instalacion_Emplazamiento_Planta
                    .ID_Instalacion_Emplazamiento_Zona = _Linea.ID_Instalacion_Emplazamiento_Zona
                    .ID_Producto = _Linea.ID_Producto
                    .ID_Propuesta = pPropuestaSeInstalo.ID_Propuesta

                    If _Linea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
                        'El If de baix es per comprovar que la línea s'ha traspasat. Pq potser que el pare tingues la marca de família que no s'ha de traspasar i el fill llavors no s'ha de vincular
                        If IsNothing(pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault) = True Then
                            ' DescartarLinea = True
                        Else
                            .ID_Propuesta_Linea_Vinculado = pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault.ID_Propuesta_Linea
                        End If
                    End If

                    .Identificador = _Linea.Identificador
                    .ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea
                    .ID_Propuesta_Antigua = _Linea.ID_Propuesta
                    .Precio = 0
                    .Descuento = 0
                    .IVA = 0
                    .Unidad = 1
                    .Activo = True
                    .ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Traspasada
                    .Uso = _Linea.Uso


                    .NumZona = _Linea.NumZona
                    .BocaConexion = _Linea.BocaConexion
                    '.CantidadPendienteRecibir = _Linea.CantidadPendienteRecibir
                    .DescripcionAmpliada = _Linea.DescripcionAmpliada
                    .DetalleInstalacion = _Linea.DetalleInstalacion
                    '.FechaPrevista = _Linea.FechaPrevista
                    .IdentificadorDelProducto = _Linea.IdentificadorDelProducto
                    .NickZona = _Linea.NickZona
                    .NumSerie = _Linea.NumSerie
                    .Particion = _Linea.Particion
                    .PrecioCoste = _Linea.PrecioCoste
                    .RutaOrden = _Linea.RutaOrden
                    .RutaParametros = _Linea.RutaParametros
                    .ID_Propuesta_Linea_TipoZona = _Linea.ID_Propuesta_Linea_TipoZona
                    .ATenerEnCuenta = _Linea.ATenerEnCuenta
                    .Fase = _Linea.Fase
                    .ReferenciaMemoria = _Linea.ReferenciaMemoria

                    Dim _DatoAcceso As Propuesta_Linea_Acceso
                    For Each _DatoAcceso In _Linea.Propuesta_Linea_Acceso
                        Dim _NewDatoAcceso As New Propuesta_Linea_Acceso
                        With _NewDatoAcceso
                            .Contraseña = _DatoAcceso.Contraseña
                            .Detalle = _DatoAcceso.Detalle
                            .Explicación = _DatoAcceso.Explicación
                            .Propuesta_Linea_TipoAcceso = _DatoAcceso.Propuesta_Linea_TipoAcceso
                            .Usuario = _DatoAcceso.Usuario
                            .ValorCRA = _DatoAcceso.ValorCRA
                        End With
                        .Propuesta_Linea_Acceso.Add(_NewDatoAcceso)
                    Next
                End With
                pPropuestaSeInstalo.Propuesta_Linea.Add(_NewLinea)
                oDTC.SubmitChanges()
            Next
        End If
        'Next
    End Sub

#End Region

#Region "Grid Lineas"
    Public Sub CargaGrid_Lineas()
        Dim DT As New DataTable


        ' BD.CargarDataSet(DT, "Select *, cast(0 as bit) as Seleccion From C_Numeros_Serie Where  ID_Producto=" & oLinea.ID_Producto & " and ID_Almacen=" & oLinea.ID_Almacen & " and ID_Entrada_Tipo=" & EnumEntradaTipo.Albaran & " and )
        Me.GRD_Lineas.M.clsUltraGrid.Cargar(DTS)
        Me.GRD_Lineas.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True

        Dim pRow As UltraGridRow
        For Each pRow In Me.GRD_Lineas.GRID.Rows
            If pRow.Cells("ID_Producto_Subfamilia_Traspaso").Value = 0 Then

                Dim _Linea As Propuesta_Linea
                _Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(pRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
                If _Linea.Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True And F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea).Count > 0 Then
                    pRow.Cells("Seleccion").Value = True
                    pRow.Cells("Seleccion").Activation = Activation.Disabled
                    RowsSeleccionadas.Add(pRow)
                Else
                    pRow.Cells("Seleccion").Value = False
                    pRow.CellAppearance.BackColor = Color.LightCoral
                End If

            Else
                pRow.Cells("Seleccion").Value = True
                pRow.CellAppearance.BackColor = Color.White
                RowsSeleccionadas.Add(pRow)
            End If

            pRow.Update()
        Next

    End Sub

    Private Sub GRD_Lineas_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Lineas.M_Grid_InitializeRow
        'If e.Row.Cells("ID_Producto_Subfamilia_Traspaso").Value = 0 Then
        '    e.Row.Cells("Seleccion").Value = False
        '    e.Row.CellAppearance.BackColor = Color.LightCoral
        'Else
        '    e.Row.Cells("Seleccion").Value = True
        '    e.Row.CellAppearance.BackColor = Color.White
        '    e.Row.Update()
        '    RowsSeleccionadas.Add(e.Row)
        'End If

        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_Lineas.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
        e.Row.Cells("Seleccion").Column.CellClickAction = CellClickAction.CellSelect
        e.Row.Cells("Seleccion").Column.CellActivation = Activation.AllowEdit

        'Dim _Linea As Propuesta_Linea
        '_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault
        'If _Linea.Propuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = True And F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea).Count > 0 Then
        '    e.Row.Cells("Seleccion").Value = True
        '    e.Row.Cells("Seleccion").Activation = Activation.Disabled
        'End If


    End Sub

    Private Sub GRD_Lineas_M_GRID_AfterCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Lineas.M_GRID_AfterCellActivate
        With GRD_Lineas.GRID
            If .ActiveCell.Column.Key = "Seleccion" Then
                Dim _Linea As Propuesta_Linea = oLinqPropuesta.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = CInt(.ActiveRow.Cells("ID_Propuesta_Linea").Value)).FirstOrDefault()
                If .ActiveCell.Value = True Then
                    .ActiveCell.Value = False
                    .ActiveCell.Row.Cells("ID_Producto_SubFamilia_Traspaso").Value = 0
                    _Linea.Producto_SubFamilia_Traspaso = oDTC.Producto_SubFamilia_Traspaso.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso = 0).FirstOrDefault
                    RowsSeleccionadas.Remove(.ActiveCell.Row)
                Else
                    .ActiveCell.Value = True
                    .ActiveCell.Row.Cells("ID_Producto_SubFamilia_Traspaso").Value = 1
                    _Linea.Producto_SubFamilia_Traspaso = oDTC.Producto_SubFamilia_Traspaso.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso = 1).FirstOrDefault
                    RowsSeleccionadas.Add(.ActiveCell.Row)
                End If
                oDTC.SubmitChanges()
            End If
            .ActiveCell.Row.Update()
            .ActiveCell = Nothing
        End With
    End Sub
#End Region


End Class