Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmRemesa
    Dim oDTC As DTCDataContext
    Dim oLinqRemesa As Remesa
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqRemesa.ID_Remesa <> 0 Then
                If oLinqRemesa.Entrada_Vencimiento.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar una remesa que tenga vencimientos asociados", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                    Exit Sub
                Else
                    If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                        oDTC.Remesa.DeleteOnSubmit(oLinqRemesa)
                        oDTC.SubmitChanges()
                        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                        Call Netejar_Pantalla()
                    End If
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
            Case "GenerarFichero"
                If oLinqRemesa.ID_Remesa = 0 Then
                    Exit Sub
                End If
                If oLinqRemesa.Entrada_Vencimiento.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción, no hay recibos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
                If oLinqRemesa.Entrada_Vencimiento.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.PendienteGenerarArchivo).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción, no hay recibos en estado 'pendiente de generar archivo'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Desea exportar los recibos?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Dim _Vencimiento As Entrada_Vencimiento
                For Each _Vencimiento In oLinqRemesa.Entrada_Vencimiento
                    _Vencimiento.Entrada_Vencimiento_Estado = oDTC.Entrada_Vencimiento_Estado.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.ReciboExportado).FirstOrDefault
                Next
                oDTC.SubmitChanges()

                Dim _Ruta As String = ""
                Dim _SaveDialog As New SaveFileDialog
                _SaveDialog.RestoreDirectory = True
                _SaveDialog.FileName = "Remesa_" & oLinqRemesa.ID_Remesa & ".xml"
                If _SaveDialog.ShowDialog = DialogResult.OK Then
                    _Ruta = _SaveDialog.FileName()
                Else
                    Exit Sub
                End If

                Dim _Exportacio As New clsRemesaXML(oDTC, oLinqRemesa, _Ruta)
                _Exportacio.GenerarFitxer()

                Call CargarGrid_Venciments(oLinqRemesa.ID_Remesa)
                Mensaje.Mostrar_Mensaje("Proceso realizado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)

            Case "CancelarFichero"

                If oLinqRemesa.ID_Remesa = 0 Then
                    Exit Sub
                End If

                If oLinqRemesa.Entrada_Vencimiento.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción, no hay recibos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If oLinqRemesa.Entrada_Vencimiento.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.ReciboExportado).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción, no hay recibos en estado 'Exportado'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Desea cancelar el proceso?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Dim _Vencimiento As Entrada_Vencimiento
                For Each _Vencimiento In oLinqRemesa.Entrada_Vencimiento
                    _Vencimiento.Entrada_Vencimiento_Estado = oDTC.Entrada_Vencimiento_Estado.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.PendienteGenerarArchivo).FirstOrDefault
                Next
                oDTC.SubmitChanges()

                Call CargarGrid_Venciments(oLinqRemesa.ID_Remesa)
                Mensaje.Mostrar_Mensaje("Proceso realizado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)


            Case "EnvioFicheroAlBanco"
                If oLinqRemesa.ID_Remesa = 0 Then
                    Exit Sub
                End If
                If oLinqRemesa.Entrada_Vencimiento.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción, no hay recibos", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
                If oLinqRemesa.Entrada_Vencimiento.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.ReciboExportado).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible realizar la acción, no hay recibos exportados", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("¿Desea confirmar que los recibos se han enviado al banco?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If


                Dim _Vencimiento As Entrada_Vencimiento
                For Each _Vencimiento In oLinqRemesa.Entrada_Vencimiento
                    _Vencimiento.Entrada_Vencimiento_Estado = oDTC.Entrada_Vencimiento_Estado.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.ReciboEnviadoBanco).FirstOrDefault
                Next
                oDTC.SubmitChanges()

                Call CargarGrid_Venciments(oLinqRemesa.ID_Remesa)
                Mensaje.Mostrar_Mensaje("Proceso realizado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)
        End Select
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pId As Integer = 0)
        Try
            Me.AplicarDisseny()
            Me.ToolForm.M.Botons.tAccions.SharedProps.Visible = True

            Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Remesa_Archivo", 1)
            Util.Cargar_Combo(Me.C_CuentaBancaria, "Select ID_Empresa_CuentaBancaria, NombreBanco + '-' + NumeroCuenta From Empresa_CuentaBancaria  Where Domiciliacion=1 Order by NombreBanco", False, False)

            Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("GenerarFichero", "Generar fichero XML para el banco", True)
            Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("CancelarFichero", "Cancelar fichero XML para el banco", True)
            Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("EnvioFicheroAlBanco", "Fichero enviado al banco", True)
            Me.GRD_Venciments.M.clsToolBar.Boto_Afegir("IrAFactura", "Ir a la factura", True)

            Call Netejar_Pantalla()

            If pId <> 0 Then
                Call Cargar_Form(pId)
            End If



            Me.KeyPreview = False

            If ValidarDadesEmpresa() = False Then
                Me.pNoTobrisFormulari = True
            End If

            'Me.ToolForm.M.Botons.tEliminar.SharedProps.Visible = False

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Function ValidarDadesEmpresa() As Boolean
        ValidarDadesEmpresa = True
        Dim _Empresa As Empresa = oDTC.Empresa.FirstOrDefault
        If _Empresa.Empresa_CuentaBancaria.Where(Function(F) F.Domiciliacion = True).Count = 0 Then
            Mensaje.Mostrar_Mensaje("Imposible realizar remesas, en la definición de la empresa no hay asignada ninguna cuenta bancaria con domiciliación", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Return False
        End If

        If _Empresa.NIF.Length = 0 Then
            Mensaje.Mostrar_Mensaje("Imposible realizar remesas, en la definición de la empresa no está informado el NIF", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Return False
        End If
        Dim _CuentaBancaria As Empresa_CuentaBancaria
        For Each _CuentaBancaria In _Empresa.Empresa_CuentaBancaria.Where(Function(F) F.Domiciliacion = True)
            If Util.IBANValidacion(_CuentaBancaria.NumeroCuenta) = False Then
                Mensaje.Mostrar_Mensaje("Imposible realizar remesas, la cuenta bancaria: " & _CuentaBancaria.NumeroCuenta & " no es una cuenta IBAN válida", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Return False
            End If

            If _CuentaBancaria.BIC.Length = 0 Then
                Mensaje.Mostrar_Mensaje("Imposible realizar remesas, la cuenta bancaria: " & _CuentaBancaria.BIC & " no tiene informado el BIC", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Return False
            End If
        Next

    End Function

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.TE_Codigo.Focus()

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If


            Call GetFromForm(oLinqRemesa)

            If oLinqRemesa.ID_Remesa = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Remesa.InsertOnSubmit(oLinqRemesa)
                oDTC.SubmitChanges()
                Me.TE_Codigo.Text = oLinqRemesa.ID_Remesa
                Call Fichero.Cargar_GRID(oLinqRemesa.ID_Remesa) 'Fem això pq la classe tingui el ID de pressupost
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
            Else
                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If

            Me.C_CuentaBancaria.ReadOnly = True  'Sempre que es guardi farem ro la compte bancaria 
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqRemesa
                Me.TE_Codigo.Value = .ID_Remesa
                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Remesa.Value = .FechaRemesa
                Me.C_CuentaBancaria.Value = .ID_Empresa_CuentaBancaria
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pRemesa As Remesa)
        Try
            With pRemesa

                .FechaAlta = Me.DT_Alta.Value
                .FechaRemesa = Me.DT_Remesa.Value
                .Empresa_CuentaBancaria = oDTC.Empresa_CuentaBancaria.Where(Function(F) F.ID_Empresa_CuentaBancaria = CInt(Me.C_CuentaBancaria.Value)).FirstOrDefault

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer)
        Try
            Call Netejar_Pantalla()

            oLinqRemesa = (From taula In oDTC.Remesa Where taula.ID_Remesa = pID Select taula).First

            Call SetToForm()

            Fichero.Cargar_GRID(pID)

            Call CargarGrid_Venciments(pID)
            ' Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Me.EstableixCaptionForm("Remesa: " & (oLinqRemesa.ID_Remesa))
            Me.C_CuentaBancaria.ReadOnly = True 'sempre esta readonly a menys que sigui un nuevo

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla()
        oLinqRemesa = New Remesa
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.TE_Codigo.Value = Nothing

        Me.DT_Alta.Value = Now.Date
        Me.DT_Remesa.Value = Nothing
        Me.C_CuentaBancaria.SelectedIndex = -1
        Fichero.Cargar_GRID(0)
        Call CargarGrid_Venciments(0)

        ' Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Instalacion")

        Me.TAB_Principal.Tabs("General").Selected = True
        ErrorProvider1.Clear()

        Me.C_CuentaBancaria.ReadOnly = False
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                'oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.C_CuentaBancaria)
                oClsControls.ControlBuit(.DT_Remesa)
                oClsControls.ControlBuit(.DT_Alta)
                'oClsControls.ControlBuit(.T_Persona_Contacto)
            End With

            ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Remesa ORDER BY [Fecha remesa] Desc", Me.TE_Codigo, "ID_Remesa", "ID_Remesa")

        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat

        'Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow
        'For Each pRow In LlistatGeneric.pGrid.GRID.Rows
        '    If IsDBNull(pRow.Cells("Fecha_Baja").Value) = False Then
        '        pRow.Appearance.BackColor = Color.LightCoral
        '    End If
        'Next
    End Sub

    Private Sub AlTancarLlistat(ByVal pID As String)
        Try
            Call Cargar_Form(pID)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

#End Region

#Region "Grid Rebuts"

    Private Sub CargarGrid_Venciments(ByVal pID As Integer)
        Try
            With Me.GRD_Venciments
                .M.clsUltraGrid.Cargar("Select * From C_Remesa_Vencimientos Where ID_Remesa=" & oLinqRemesa.ID_Remesa & " Order by Cliente_Nombre,NumFactura", BD)
                .M_Editable()
                Call CargarCombo_EstatRebut(.GRID, 0, pID)

                Dim _pCol As UltraGridColumn
                For Each _pCol In Me.GRD_Venciments.GRID.DisplayLayout.Bands(0).Columns
                    If _pCol.Key <> "ID_Entrada_Vencimiento_Estado" Then
                        _pCol.CellActivation = Activation.NoEdit
                        _pCol.CellClickAction = CellClickAction.RowSelect
                    End If
                Next
                .M_TreureFocus()
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Venciments_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Venciments.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "ID_Entrada_Vencimiento_Estado"
                Dim _IDEstat As EnumVencimientoEstado = e.Cell.Value 'no hem de carregar el combo dels 3 estats si no estás en un dels 3 estats.
                If _IDEstat = EnumVencimientoEstado.Cobrado Or _IDEstat = EnumVencimientoEstado.Devuelto Or _IDEstat = EnumVencimientoEstado.ReciboEnviadoBanco Then
                    Call CargarCombo_EstatRebut(e.Cell, 0, True)  'fem això aqui pq només volem que surtin 3 estats quan es click dels 6 que hi ha
                End If
        End Select

    End Sub

    Private Sub GRD_Venciments_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Venciments.M_ToolGrid_ToolEliminarRow
        Try
            Dim _IDVenciment As Integer = e.Cells("ID_Entrada_Vencimiento").Value
            Dim _Venciment As Entrada_Vencimiento = oDTC.Entrada_Vencimiento.Where(Function(F) F.ID_Entrada_Vencimiento = _IDVenciment).FirstOrDefault

            If _Venciment.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.PendienteAsignarARemesa OrElse _Venciment.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.PendienteGenerarArchivo Then 'si  està enviat al banc no el deixarem borrar
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
                    oLinqRemesa.Entrada_Vencimiento.Remove(_Venciment)
                    _Venciment.Entrada_Vencimiento_Estado = oDTC.Entrada_Vencimiento_Estado.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.PendienteAsignarARemesa).FirstOrDefault
                    oDTC.SubmitChanges()
                    Call CargarGrid_Venciments(oLinqRemesa.ID_Remesa)
                End If
            Else
                Mensaje.Mostrar_Mensaje("Imposible desasignar de la remesa un recibo enviado al banco", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
                Exit Sub
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Venciments_M_ToolGrid_ToolAfegir(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Venciments.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Venciments
                If oLinqRemesa.ID_Remesa = 0 Then
                    If Guardar() = False Then
                        Exit Sub
                    End If
                End If

                If oLinqRemesa.Entrada_Vencimiento.Where(Function(F) F.ID_Entrada_Vencimiento_Estado > 2).Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible añadir más recibos si ya se ha exportado la remesa", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                Dim frm As New frmRemesa_SeleccionRecibos
                frm.Entrada(oLinqRemesa, oDTC)
                AddHandler frm.AlTancarFormulariSeleccio, AddressOf AlTancarForm
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

    Private Sub GRD_Venciments_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Venciments.M_Grid_InitializeRow
        Dim _IDEstat As EnumVencimientoEstado = e.Row.Cells("ID_Entrada_Vencimiento_Estado").Value
        If _IDEstat = EnumVencimientoEstado.Cobrado Or _IDEstat = EnumVencimientoEstado.Devuelto Or _IDEstat = EnumVencimientoEstado.ReciboEnviadoBanco Then
            e.Row.Cells("ID_Entrada_Vencimiento_Estado").Activation = Activation.AllowEdit
        Else
            e.Row.Cells("ID_Entrada_Vencimiento_Estado").Activation = Activation.NoEdit
        End If
        If _IDEstat = EnumVencimientoEstado.Devuelto Then
            e.Row.CellAppearance.BackColor = Color.LightCoral
        Else
            e.Row.CellAppearance.BackColor = Color.White
        End If
    End Sub

    Private Sub GRD_Venciments_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Venciments.M_GRID_CellListSelect
        Try

            If IsDBNull(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue) Then
                e.Cell.Value = DBNull.Value
            End If

            Select Case e.Cell.Column.Key
                Case "ID_Entrada_Vencimiento_Estado"
                    Dim _Valor As Integer = CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue
                    Dim _IDVenciment As Integer = e.Cell.Row.Cells("ID_Entrada_Vencimiento").Value
                    Dim _Venciment As Entrada_Vencimiento = oDTC.Entrada_Vencimiento.Where(Function(F) F.ID_Entrada_Vencimiento = _IDVenciment).FirstOrDefault
                    _Venciment.Entrada_Vencimiento_Estado = oDTC.Entrada_Vencimiento_Estado.Where(Function(F) F.ID_Entrada_Vencimiento_Estado = _Valor).FirstOrDefault
                    Select Case DirectCast(CInt(_Venciment.ID_Entrada_Vencimiento_Estado), EnumVencimientoEstado)
                        Case EnumVencimientoEstado.Cobrado, EnumVencimientoEstado.Devuelto
                            _Venciment.Pagado = True

                        Case EnumVencimientoEstado.ReciboEnviadoBanco
                            _Venciment.Pagado = False
                    End Select
                    e.Cell.Row.Cells("Pagado").Value = _Venciment.Pagado
                    oDTC.SubmitChanges()
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarForm(pCorrecte As Boolean, ByRef pSeleccio As ArrayList)
        If pCorrecte = True Then
            Call CargarGrid_Venciments(oLinqRemesa.ID_Remesa)
        End If
    End Sub

    Private Sub CargarCombo_EstatRebut(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDRemesa As Integer)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Entrada_Vencimiento_Estado) = (From Taula In oDTC.Entrada_Vencimiento_Estado Order By Taula.ID_Entrada_Vencimiento_Estado Select Taula)
            Dim Var As Entrada_Vencimiento_Estado

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Entrada_Vencimiento_Estado, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Entrada_Vencimiento_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("ID_Entrada_Vencimiento_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_EstatRebut(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDEmplazamiento As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Entrada_Vencimiento_Estado) = (From Taula In oDTC.Entrada_Vencimiento_Estado Where Taula.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.ReciboEnviadoBanco Or Taula.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.Cobrado Or Taula.ID_Entrada_Vencimiento_Estado = EnumVencimientoEstado.Devuelto Order By Taula.Descripcion Select Taula)
            Dim Var As Entrada_Vencimiento_Estado


            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var.ID_Entrada_Vencimiento_Estado, Var.Descripcion)
            Next

            'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Venciments_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Venciments.M_ToolGrid_ToolClickBotonsExtras2
        Try
            Select Case e.Tool.Key
                Case "IrAFactura"
                    With Me.GRD_Venciments
                        If .GRID.Selected.Rows.Count <> 1 Then
                            Exit Sub
                        End If

                        If .GRID.Selected.Rows(0).Band.Index = 0 Then
                            Dim _Documento As Integer = .GRID.ActiveRow.Cells("ID_Entrada").Value
                            Dim _TipoDocumento As Integer = oDTC.Entrada.Where(Function(F) F.ID_Entrada = _Documento).FirstOrDefault.ID_Entrada_Tipo

                            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
                            frm.Entrada(CInt(.GRID.Selected.Rows(0).Cells("ID_Entrada").Value), _TipoDocumento)
                            frm.FormObrir(frmPrincipal, True)
                        End If
                    End With
            End Select
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub GRD_Stock_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow)
    '    If Me.GRD_Stock.GRID.Selected.Rows.Count = 1 Then
    '        Dim frm As New frmProducto_Trazabilidad
    '        frm.Entrada(e.Cells("ID_Producto").Value)
    '        frm.FormObrir(Me, True)
    '    End If
    'End Sub

    'Private Sub GRD_Stock_M_ToolGrid_ToolVisualitzarDobleClickRow()
    '    ' If Me.GRD_Stock.GRID.Selected.Rows.Count = 1 Then
    '    '    Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
    '    '    frm.Entrada(Me.GRD_Stock.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
    '    '    frm.FormObrir(Me, True)
    '    ' End If
    'End Sub

    'Private Sub GRD_Stock_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs)
    '    Try
    '        Select Case e.Tool.Key
    '            Case "VerProducto"
    '                With Me.GRD_Stock
    '                    If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Band.Index <> 0 Then
    '                        Exit Sub
    '                    End If

    '                    Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
    '                    frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
    '                    frm.FormObrir(frmPrincipal, True)
    '                End With

    '            Case "VerTrazabilidad"
    '                With Me.GRD_Stock
    '                    If .GRID.Selected.Rows.Count <> 1 OrElse .GRID.Selected.Rows(0).Band.Index <> 0 Then
    '                        Exit Sub
    '                    End If

    '                    Dim frm As New frmProducto_Trazabilidad
    '                    frm.Entrada(.GRID.Selected.Rows(0).Cells("ID_Producto").Value)
    '                    frm.FormObrir(Me, True)
    '                End With

    '            Case "VisualizarFotos"
    '                With Me.GRD_Stock
    '                    'If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
    '                    '    .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "Visualizar fotos"
    '                    'Else
    '                    '    .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos"
    '                    'End If
    '                    Call CargarGrid_Stocks(oLinqAlmacen.ID_Almacen)
    '                End With
    '        End Select

    '    Catch ex As Exception
    '        DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm()
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Stock_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs)
    '    With Me.GRD_Stock
    '        If e.Row.Band.Index = 0 Then
    '            '  If .ToolGrid.Tools("VisualizarFotos").SharedProps.Caption = "No visualizar fotos" Then
    '            Dim _Producto As Producto
    '            _Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(e.Row.Cells("ID_Producto").Value)).FirstOrDefault
    '            If _Producto.Archivo Is Nothing = False AndAlso _Producto.Archivo.CampoBinario.Length > 0 Then
    '                e.Row.Cells("Foto").Appearance.Image = Util.BinaryToImage(_Producto.Archivo.CampoBinario.ToArray)
    '            End If
    '            .GRID.DisplayLayout.Override.DefaultRowHeight = 40
    '            'Else
    '            .GRID.DisplayLayout.Override.DefaultRowHeight = 20
    '            ' End If
    '        End If
    '    End With
    'End Sub
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
    '            Dim ooLinqAlmacen As Almacen = oDTC.Almacen.Where(Function(F) F.ID_Almacen = CInt(Me.TE_Codigo.Value)).FirstOrDefault()
    '            If ooLinqAlmacen Is Nothing = False Then
    '                Call Cargar_Form(ooLinqAlmacen.ID_Almacen)
    '            Else
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT, "")
    '                Call Netejar_Pantalla()
    '            End If
    '        Else
    '            'Me.TE_Codigo.Value = oDTC.Cliente.Where(Function(F) F.Activo = True).Max(Function(F) F.Codigo) + 1
    '            'Exit Sub
    '        End If
    '    End If
    'End Sub

    Private Sub TAB_Principal_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
        'Select Case e.Tab.Key
        '    Case "Stocks"
        '        Call CargarGrid_Stocks(oLinqAlmacen.ID_Almacen)
        'End Select
    End Sub

#End Region

    Private Sub frmRemesa_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
End Class