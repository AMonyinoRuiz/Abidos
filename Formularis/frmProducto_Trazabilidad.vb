Imports Infragistics.Win.UltraWinGrid

Public Class frmProducto_Trazabilidad
    Dim oDTC As DTCDataContext
    Enum EnumDadaQueNecessito
        Quantitat = 1
        QuantitatStockReal = 2
        QuantitatStockTeorico = 3
    End Enum

    Public Sub Entrada(Optional ByVal pIDProducto As Integer = 0, Optional ByVal pIDNS As Integer = 0)
        Me.AplicarDisseny()
        Call Netejar_Pantalla()

        Util.Cargar_Combo(Me.C_Division, "SELECT ID_Producto_Division, Descripcion FROM Producto_Division WHERE Activo=1 ORDER BY Descripcion", False)
        'Util.Cargar_Combo(Me.C_Familia, "SELECT ID_Producto_Familia, Descripcion FROM Producto_Familia WHERE Activo=1 ORDER BY Descripcion", False)
        'Util.Cargar_Combo(Me.C_Subfamilia, "SELECT ID_Producto_Subfamilia, Descripcion FROM Producto_SubFamilia WHERE Activo=1 ORDER BY Descripcion", False)
        'Util.Cargar_Combo(Me.C_Marca, "SELECT ID_Producto_Marca, Descripcion FROM Producto_Marca WHERE Activo=1 ORDER BY Descripcion", False)

        'Si ens han passat un id de producto obrirem la pantalla posant l'article i carregant les dades
        If pIDProducto <> 0 Then
            Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = pIDProducto).FirstOrDefault
            Me.TE_Codigo.Tag = _Producto.ID_Producto
            Me.TE_Codigo.Text = _Producto.Codigo
            Me.T_Descripcion.Text = _Producto.Descripcion
            Call B_Buscar_Producto_Click(Nothing, Nothing)
        End If

        If pIDNS <> 0 Then
            Dim _NS As NS = oDTC.NS.Where(Function(F) F.ID_NS = pIDNS).FirstOrDefault
            Me.T_NS.Text = _NS.Descripcion
            Call B_Buscar_NS_Click(Nothing, Nothing)
            Util.Tab_Seleccio_x_Key(Me.UltraTabControl1, "NS")
        End If

    End Sub

    Public Sub Netejar_Pantalla()
        oDTC = New DTCDataContext(BD.Conexion)
        Call CargaGrid_NS(0)
        Call CargaGrid_Producto(0)
        Call CargaGrid_Producto_Almacen(0)
        Call CargaGrid_ProductoNS(0)
        Me.GRD_Clasificacion.GRID.DataSource = Nothing
        Me.L_Disponible_NS.Text = ""
    End Sub

#Region "Grid NS"

    Private Sub CargaGrid_NS(ByVal pId As Integer)
        Try
            'Dim _Listado As IEnumerable = From taula In oDTC.Propuesta_Linea Where taula.ID_Producto = pId And taula.Propuesta.Instalacion.Cliente.ID_Cliente = oLinqInstalacion.ID_Cliente And taula.Activo = True And taula.ID_Propuesta <> oLinqPropuesta.ID_Propuesta Order By taula.Propuesta.Fecha Descending Select taula.ID_Propuesta_Linea, taula.Propuesta.Instalacion.ID_Instalacion, taula.Propuesta.Codigo, taula.Propuesta.Fecha, taula.Producto.Descripcion, Emplazamiento = taula.Instalacion_Emplazamiento.Descripcion, Planta = taula.Instalacion_Emplazamiento_Planta.Descripcion, Zona = taula.Instalacion_Emplazamiento_Zona.Descripcion, ElementosAProteger = taula.Instalacion_ElementosAProteger.Instalacion_ElementosAProteger_Tipo.Descripcion, Abertura = taula.Instalacion_Emplazamiento_Abertura.Descripcion_Detallada, taula.Unidad, taula.Precio, taula.Descuento, TotalUnitario = taula.Precio - taula.Precio * taula.Descuento
            Dim _Listado As IEnumerable

            _Listado = From Taula In oDTC.Entrada_Linea_NS Where Taula.ID_NS = pId Order By Taula.ID_Entrada_Linea_NS Descending Select Taula.Entrada_Linea.Entrada.Codigo, Taula.Entrada_Linea.Entrada.Descripcion, Entrada_Tipo = Taula.Entrada_Linea.Entrada.Entrada_Tipo.Descripcion, Taula.Entrada_Linea.Entrada.FechaEntrada, Almacen = Taula.Entrada_Linea.Almacen.Descripcion, CompuestoPor = Taula.Entrada_Linea.ID_Entrada_Linea_Padre.HasValue, Taula.Entrada_Linea.Entrada.ID_Entrada, Proveedor = Taula.Entrada_Linea.Entrada.Proveedor.Nombre, Cliente = Taula.Entrada_Linea.Entrada.Cliente.Nombre


            With Me.GRD_NS
                '.GRID.DataSource = _Listado
                .M.clsUltraGrid.CargarIEnumerable(_Listado)

                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                .GRID.DisplayLayout.Bands(0).Columns("CompuestoPor").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub B_Buscar_NS_Click(sender As Object, e As EventArgs) Handles B_Buscar_NS.Click
        If Me.T_NS.Text.Length = 0 Then
            Exit Sub
        End If
        Me.L_Disponible_NS.Text = ""
        Dim _NS As [NS] = oDTC.NS.Where(Function(F) F.Descripcion = Me.T_NS.Text).FirstOrDefault
        If _NS Is Nothing = False Then
            Call CargaGrid_NS(_NS.ID_NS)
            Me.T_Producto_Codigo_NS.Text = _NS.Producto.Codigo
            Me.T_Producto_Descripcion_NS.Text = _NS.Producto.Descripcion
            If CType(_NS.ID_NS_Estado, EnumNSEstado) = EnumNSEstado.Disponible Then
                Me.L_Disponible_NS.Text = "Disponible"
                Me.L_Disponible_NS.Appearance.ForeColor = Color.Green
            Else
                Me.L_Disponible_NS.Text = "No disponible"
                Me.L_Disponible_NS.Appearance.ForeColor = Color.Red
            End If
        Else
            Mensaje.Mostrar_Mensaje("El número de serie introducido no existe", M_Mensaje.Missatge_Modo.INFORMACIO)
            Call Netejar_Pantalla()
        End If
    End Sub

    Private Sub GRD_NS_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles GRD_NS.M_GRID_DoubleClickRow2, GRD_Producto.M_GRID_DoubleClickRow2
        Dim _ID_Entrada As Integer = e.Cells("ID_Entrada").Value
        Dim _Entrada As Entrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = _ID_Entrada).FirstOrDefault
        Dim frm As New frmEntrada
        frm.Entrada(_ID_Entrada, _Entrada.ID_Entrada_Tipo)
        frm.FormObrir(Me, True)


    End Sub

    'Private Sub GRD_PreciosAnteriores_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_PreciosAnteriores.M_GRID_DoubleClickRow2
    '    Dim _IDTipusDocument As EnumEntradaTipo
    '    _IDTipusDocument = e.Cells("ID_Entrada_Tipo").Value
    '    Dim _IDDocument As Integer = e.Cells("ID_Entrada").Value
    '    Dim frm As New frmEntrada
    '    frm.Entrada(_IDDocument, _IDTipusDocument)
    '    frm.FormObrir(Me, True)
    'End Sub

#Region "Por Producto"

    Private Sub TE_Codigo_ProductoNS_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo_ProductoNS.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Call Cridar_Llistat_Generic_ProductoNS()
        End If
    End Sub

    Private Sub Cridar_Llistat_Generic_ProductoNS()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto Where RequiereNumeroSerie=1 ORDER BY Descripcion", Me.TE_Codigo_ProductoNS, "ID_Producto", "Codigo", T_Descripcion_ProductoNS, "Descripcion")
        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGeneric
    End Sub

    Private Sub TE_Codigo_ProductoNS_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo_ProductoNS.KeyDown
        If Me.TE_Codigo_ProductoNS.Text Is Nothing = False Then
            If e.KeyCode = Keys.Enter Then
                Dim ooLinqProducto As Producto = oDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Codigo_ProductoNS.Text).FirstOrDefault()
                If ooLinqProducto Is Nothing = False Then
                    Me.TE_Codigo_ProductoNS.Tag = ooLinqProducto.ID_Producto
                    Me.TE_Codigo_ProductoNS.Text = ooLinqProducto.Codigo
                    Me.T_Descripcion_ProductoNS.Text = ooLinqProducto.Descripcion
                    Call B_Buscar_NS_PorProducto_Click(Nothing, Nothing)
                Else
                    Call Netejar_Pantalla()
                End If
            End If
        End If
    End Sub

    Private Sub B_Buscar_NS_PorProducto_Click(sender As Object, e As EventArgs) Handles B_Buscar_NS_PorProducto.Click
        If Me.TE_Codigo_ProductoNS.Tag Is Nothing Then
            Exit Sub
        End If

        Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Codigo_ProductoNS.Tag)).FirstOrDefault
        If _Producto Is Nothing = False Then
            Call CargaGrid_ProductoNS(_Producto.ID_Producto)
        Else
            Mensaje.Mostrar_Mensaje("El producto no existe", M_Mensaje.Missatge_Modo.INFORMACIO)
            Call Netejar_Pantalla()
        End If
    End Sub

    Private Sub CargaGrid_ProductoNS(ByVal pIDProducto As Integer)
        Try
            Dim _Llistat As IEnumerable = From Taula In oDTC.NS Where Taula.ID_Producto = pIDProducto Order By Taula.Descripcion Select Taula.ID_NS_Estado, Taula.ID_NS, Taula.Descripcion, Almacen = Taula.Almacen.Descripcion

            With Me.GRD_ProductoNS
                .M.clsUltraGrid.CargarIEnumerable(_Llistat)

                Dim pRow As UltraGridRow
                For Each pRow In .GRID.Rows
                    If pRow.Cells("ID_NS_Estado").Value = CInt(EnumNSEstado.NoDisponible) Then
                        pRow.CellAppearance.BackColor = Color.LightCoral
                    End If
                Next
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductoNS_NS_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_ProductoNS.M_GRID_DoubleClickRow2
        If Me.GRD_ProductoNS.GRID.Selected.Rows.Count = 1 Then
            'Dim frm As New frmProducto_Trazabilidad
            'frm.Entrada(0, e.Cells("ID_NS").Value)
            'frm.FormObrir(Me, True)
            Me.T_NS.Value = Me.GRD_ProductoNS.GRID.Selected.Rows(0).Cells("Descripcion").Value
            Util.Tab_Seleccio_x_Key(Me.TAB_NS, "NS")
            Call B_Buscar_NS_Click(Nothing, Nothing)
        End If
    End Sub

#End Region

#End Region

#Region "Producto"

    Private Sub CargaGrid_Producto(ByVal pId As Integer)
        Try


            'Dim _Listado As IEnumerable = From taula In oDTC.Propuesta_Linea Where taula.ID_Producto = pId And taula.Propuesta.Instalacion.Cliente.ID_Cliente = oLinqInstalacion.ID_Cliente And taula.Activo = True And taula.ID_Propuesta <> oLinqPropuesta.ID_Propuesta Order By taula.Propuesta.Fecha Descending Select taula.ID_Propuesta_Linea, taula.Propuesta.Instalacion.ID_Instalacion, taula.Propuesta.Codigo, taula.Propuesta.Fecha, taula.Producto.Descripcion, Emplazamiento = taula.Instalacion_Emplazamiento.Descripcion, Planta = taula.Instalacion_Emplazamiento_Planta.Descripcion, Zona = taula.Instalacion_Emplazamiento_Zona.Descripcion, ElementosAProteger = taula.Instalacion_ElementosAProteger.Instalacion_ElementosAProteger_Tipo.Descripcion, Abertura = taula.Instalacion_Emplazamiento_Abertura.Descripcion_Detallada, taula.Unidad, taula.Precio, taula.Descuento, TotalUnitario = taula.Precio - taula.Precio * taula.Descuento
            Dim _Listado As IEnumerable

            _Listado = From Taula In oDTC.Entrada_Linea Where Taula.ID_Producto = pId And Taula.NoRestarStock = False Order By Taula.ID_Entrada_Linea Descending Select Taula.Entrada.Codigo, Taula.Entrada.Descripcion, Entrada_Tipo = Taula.Entrada.Entrada_Tipo.Descripcion, Taula.Entrada.FechaEntrada, Almacen = Taula.Almacen.Descripcion, CantidadDocumento = RetornaQuantitat(Taula, EnumDadaQueNecessito.Quantitat), CantidadStockReal = RetornaQuantitat(Taula, EnumDadaQueNecessito.QuantitatStockReal), CantidadStockTeorico = RetornaQuantitat(Taula, EnumDadaQueNecessito.QuantitatStockTeorico), CompuestoPor = Taula.ID_Entrada_Linea_Padre.HasValue = True, Taula.ID_Entrada, Proveedor = Taula.Entrada.Proveedor.Nombre, Cliente = Taula.Entrada.Cliente.Nombre, TotalAcumuladoReal = 0, TotalAcumuladoTeorico = 0



            With Me.GRD_Producto
                '.GRID.DataSource = _Listado
                .M.clsUltraGrid.CargarIEnumerable(_Listado)
                .GRID.DisplayLayout.Bands(0).Columns("CompuestoPor").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect
            End With
            If pId <> 0 Then
                Me.T_StockRealCentral.Value = oDTC.RetornaStock(pId, 0).Sum(Function(F) F.StockReal)
                Me.T_StockTeoricoCentral.Value = oDTC.RetornaStock(pId, 0).Sum(Function(F) F.StockTeorico)
            End If

            Me.GRD_Producto.GRID.DisplayLayout.Bands(0).Columns("TotalAcumuladoReal").Formula = "[CantidadStockReal]+ if( iserror( [TotalAcumuladoReal(+1)] ), 0, [TotalAcumuladoReal(+1)])"
            Me.GRD_Producto.GRID.DisplayLayout.Bands(0).Columns("TotalAcumuladoTeorico").Formula = "[CantidadStockTeorico]+ if( iserror( [TotalAcumuladoTeorico(+1)] ), 0, [TotalAcumuladoTeorico(+1)])"
            Me.GRD_Producto.GRID.CalcManager = New Infragistics.Win.UltraWinCalcManager.UltraCalcManager


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function RetornaQuantitat(ByRef pLineaDocument As Entrada_Linea, ByVal pDadaQueNecessito As EnumDadaQueNecessito) As Decimal
        Select Case pDadaQueNecessito
            Case EnumDadaQueNecessito.Quantitat
                Return (IIf(RetornaUnTrueSiHasDeMultiplicarhoPerMenys1(pLineaDocument.Entrada.ID_Entrada_Tipo), pLineaDocument.Unidad * -1, pLineaDocument.Unidad))

            Case EnumDadaQueNecessito.QuantitatStockReal
                If RetornaUnTrueSiEtsStockReal(pLineaDocument.Entrada.ID_Entrada_Tipo) = False Then
                    Return 0
                Else
                    Return (IIf(RetornaUnTrueSiHasDeMultiplicarhoPerMenys1(pLineaDocument.Entrada.ID_Entrada_Tipo), pLineaDocument.Unidad * -1, pLineaDocument.Unidad))
                End If

            Case EnumDadaQueNecessito.QuantitatStockTeorico
                If RetornaUnTrueSiEtsStockReal(pLineaDocument.Entrada.ID_Entrada_Tipo) = True Then
                    Return 0
                Else
                    If pLineaDocument.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.FacturaCompra Or pLineaDocument.Entrada.ID_Entrada_Tipo = EnumEntradaTipo.FacturaVenta Then
                        Return 0
                    Else
                        Return (IIf(RetornaUnTrueSiHasDeMultiplicarhoPerMenys1(pLineaDocument.Entrada.ID_Entrada_Tipo), pLineaDocument.Unidad * -1, pLineaDocument.Unidad))
                    End If
                End If
        End Select

    End Function

    Private Function RetornaUnTrueSiEtsStockReal(ByRef pIDDocumentoTipo As EnumEntradaTipo) As Boolean
        Select Case pIDDocumentoTipo
            Case EnumEntradaTipo.AlbaranCompra
                Return True
            Case EnumEntradaTipo.AlbaranVenta
                Return True
            Case EnumEntradaTipo.DevolucionCompra
                Return True
            Case EnumEntradaTipo.DevolucionVenta
                Return True
            Case EnumEntradaTipo.Regularizacion
                Return True
            Case EnumEntradaTipo.TraspasoAlmacen
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Function RetornaUnTrueSiHasDeMultiplicarhoPerMenys1(ByRef pIDDocumentoTipo As EnumEntradaTipo) As Boolean
        Select Case pIDDocumentoTipo
            Case EnumEntradaTipo.AlbaranVenta
                Return True
            Case EnumEntradaTipo.DevolucionCompra
                Return True
            Case EnumEntradaTipo.PedidoVenta
                Return True
            Case Else
                Return False
        End Select
    End Function

    Private Sub TE_Codigo_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Call Cridar_Llistat_Generic()
        End If
    End Sub

    Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown
        If Me.TE_Codigo.Text Is Nothing = False Then
            If e.KeyCode = Keys.Enter Then
                Dim ooLinqProducto As Producto = oDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Codigo.Text).FirstOrDefault()
                If ooLinqProducto Is Nothing = False Then
                    Me.TE_Codigo.Tag = ooLinqProducto.ID_Producto
                    Me.TE_Codigo.Text = ooLinqProducto.Codigo
                    Me.T_Descripcion.Text = ooLinqProducto.Descripcion
                    Call B_Buscar_Producto_Click(Nothing, Nothing)
                Else
                    Call Netejar_Pantalla()
                End If
            End If
        End If
    End Sub

    Private Sub B_Buscar_Producto_Click(sender As Object, e As EventArgs) Handles B_Buscar_Producto.Click
        If Me.TE_Codigo.Tag Is Nothing Then
            Exit Sub
        End If

        Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(Me.TE_Codigo.Tag)).FirstOrDefault
        If _Producto Is Nothing = False Then
            Call CargaGrid_Producto(_Producto.ID_Producto)
            Call CargaGrid_Producto_Almacen(_Producto.ID_Producto)
        Else
            Mensaje.Mostrar_Mensaje("El producto no existe", M_Mensaje.Missatge_Modo.INFORMACIO)
            Call Netejar_Pantalla()
        End If
    End Sub

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto ORDER BY Descripcion", Me.TE_Codigo, "ID_Producto", "Codigo", T_Descripcion, "Descripcion")
        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatGeneric
    End Sub

#Region "Producto almacén"

    Private Sub CargaGrid_Producto_Almacen(ByVal pId As Integer)
        Try
            'Dim _Listado As IEnumerable = From taula In oDTC.Propuesta_Linea Where taula.ID_Producto = pId And taula.Propuesta.Instalacion.Cliente.ID_Cliente = oLinqInstalacion.ID_Cliente And taula.Activo = True And taula.ID_Propuesta <> oLinqPropuesta.ID_Propuesta Order By taula.Propuesta.Fecha Descending Select taula.ID_Propuesta_Linea, taula.Propuesta.Instalacion.ID_Instalacion, taula.Propuesta.Codigo, taula.Propuesta.Fecha, taula.Producto.Descripcion, Emplazamiento = taula.Instalacion_Emplazamiento.Descripcion, Planta = taula.Instalacion_Emplazamiento_Planta.Descripcion, Zona = taula.Instalacion_Emplazamiento_Zona.Descripcion, ElementosAProteger = taula.Instalacion_ElementosAProteger.Instalacion_ElementosAProteger_Tipo.Descripcion, Abertura = taula.Instalacion_Emplazamiento_Abertura.Descripcion_Detallada, taula.Unidad, taula.Precio, taula.Descuento, TotalUnitario = taula.Precio - taula.Precio * taula.Descuento
            Dim _Listado As IEnumerable

            _Listado = From Taula In oDTC.RetornaStock(pId, 0) Where Taula.ID_Producto = pId Order By Taula.ProductoDescripcion Descending Select Taula


            With Me.GRD_Producto_Almacen
                '.GRID.DataSource = _Listado
                .M.clsUltraGrid.CargarIEnumerable(_Listado)
                .M_NoEditable()
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Producto_Almacen_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles GRD_Producto_Almacen.M_GRID_DoubleClickRow2
        Dim _ID_Almacen As Integer = e.Cells("ID_Almacen").Value
        Dim frm As New frmAlmacen
        frm.Entrada(_ID_Almacen)
        frm.FormObrir(Me, True)
    End Sub

#End Region

#End Region

#Region "Clasificación"

    Private Sub CargaGrid_Clasificacion(ByVal pIDDivision As Integer, ByVal pIDFamilia As Integer, ByVal pIDSubfamilia As Integer, ByVal pIDMarca As Integer)
        Try
            'Dim _Listado As IEnumerable = From taula In oDTC.Propuesta_Linea Where taula.ID_Producto = pId And taula.Propuesta.Instalacion.Cliente.ID_Cliente = oLinqInstalacion.ID_Cliente And taula.Activo = True And taula.ID_Propuesta <> oLinqPropuesta.ID_Propuesta Order By taula.Propuesta.Fecha Descending Select taula.ID_Propuesta_Linea, taula.Propuesta.Instalacion.ID_Instalacion, taula.Propuesta.Codigo, taula.Propuesta.Fecha, taula.Producto.Descripcion, Emplazamiento = taula.Instalacion_Emplazamiento.Descripcion, Planta = taula.Instalacion_Emplazamiento_Planta.Descripcion, Zona = taula.Instalacion_Emplazamiento_Zona.Descripcion, ElementosAProteger = taula.Instalacion_ElementosAProteger.Instalacion_ElementosAProteger_Tipo.Descripcion, Abertura = taula.Instalacion_Emplazamiento_Abertura.Descripcion_Detallada, taula.Unidad, taula.Precio, taula.Descuento, TotalUnitario = taula.Precio - taula.Precio * taula.Descuento
            Dim _Listado As IEnumerable
            _Listado = From Taula In oDTC.Entrada_Linea Where (Taula.Producto.ID_Producto_Division = pIDDivision Or pIDDivision = 0) And (Taula.Producto.ID_Producto_Familia = pIDFamilia Or pIDFamilia = 0) And (Taula.Producto.ID_Producto_SubFamilia = pIDSubfamilia Or pIDSubfamilia = 0) And (Taula.Producto.ID_Producto_Marca = pIDMarca Or pIDMarca = 0) Order By Taula.ID_Entrada_Linea Descending Select Taula.Entrada.Codigo, Taula.Entrada.Descripcion, Entrada_Tipo = Taula.Entrada.Entrada_Tipo.Descripcion, Taula.Entrada.FechaEntrada, Almacen = Taula.Almacen.Descripcion, Cantidad = Taula.Unidad, CompuestoPor = Taula.ID_Entrada_Linea_Padre.HasValue, Taula.ID_Entrada, Proveedor = Taula.Entrada.Proveedor.Nombre, Cliente = Taula.Entrada.Cliente.Nombre, Division = Taula.Producto.Producto_Division.Descripcion, Familia = Taula.Producto.Producto_Familia.Descripcion, SubFamilia = Taula.Producto.Producto_SubFamilia.Descripcion, Marca = Taula.Producto.Producto_Marca.Descripcion, CodigoProducto = Taula.Producto.Codigo, DescripcionProducto = Taula.Producto.Descripcion


            With Me.GRD_Clasificacion
                '.GRID.DataSource = _Listado
                .M.clsUltraGrid.CargarIEnumerable(_Listado)
                .GRID.DisplayLayout.Bands(0).Columns("CompuestoPor").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
                .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                '.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub B_Buscar_Clasificacion_Click(sender As Object, e As EventArgs) Handles B_Buscar_Clasificacion.Click
        'Dim _IDDivision As Integer
        'Dim _IDFamilia As Integer
        'Dim _IDSubfamilia As Integer
        'Dim _Marca As Integer

        Call CargaGrid_Clasificacion(Util.Comprobar_NULL_Per_0(Me.C_Division.Value), Util.Comprobar_NULL_Per_0(Me.C_Familia.Value), Util.Comprobar_NULL_Per_0(Me.C_Subfamilia.Value), Util.Comprobar_NULL_Per_0(Me.C_Marca.Value))
    End Sub

    Private Sub GRD_Clasificacion_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.UltraGridRow) Handles GRD_Clasificacion.M_GRID_DoubleClickRow2
        Dim _ID_Entrada As Integer = e.Cells("ID_Entrada").Value
        Dim _Entrada As Entrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = _ID_Entrada).FirstOrDefault
        Dim frm As New frmEntrada
        frm.Entrada(_ID_Entrada, _Entrada.ID_Entrada_Tipo)
        frm.FormObrir(Me, True)
    End Sub
#End Region

    Private Sub C_Division_ValueChanged(sender As Object, e As EventArgs) Handles C_Division.ValueChanged
        Me.C_Familia.ReadOnly = False
        Me.C_Marca.ReadOnly = False
        Me.C_Familia.Value = Nothing
        Me.C_Marca.Value = Nothing
        Me.C_Subfamilia.Value = Nothing
        Me.C_Subfamilia.ReadOnly = True

        If IsNumeric(Me.C_Division.Value) = True Then
            Util.Cargar_Combo(Me.C_Familia, "SELECT ID_Producto_Familia, Descripcion FROM Producto_Familia WHERE Activo=1 and ID_Producto_Division=" & Me.C_Division.Value & " ORDER BY Descripcion", False)
            Util.Cargar_Combo(Me.C_Marca, "SELECT ID_Producto_Marca, Descripcion FROM Producto_Marca WHERE Activo=1 and ID_Producto_Division=" & Me.C_Division.Value & " ORDER BY Descripcion", False)
        Else

        End If

    End Sub

    Private Sub C_Familia_ValueChanged(sender As Object, e As EventArgs) Handles C_Familia.ValueChanged
        If Me.C_Familia.Value Is Nothing = False Then
            Me.C_Subfamilia.ReadOnly = False
            Me.C_Subfamilia.Value = Nothing

            If IsNumeric(Me.C_Familia.Value) = True Then
                Util.Cargar_Combo(Me.C_Subfamilia, "SELECT ID_Producto_SubFamilia, Descripcion FROM Producto_SubFamilia WHERE Activo=1 and ID_Producto_Familia=" & Me.C_Familia.Value & " ORDER BY Descripcion", False)
            End If
        End If
    End Sub

    Private Sub T_NS_KeyDown(sender As Object, e As KeyEventArgs) Handles T_NS.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call B_Buscar_NS_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub AlTancarLlistatGeneric()
        Call B_Buscar_Producto_Click(Nothing, Nothing)
    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub GRD_Producto_Load(sender As Object, e As EventArgs) Handles GRD_Producto.Load

    End Sub
End Class