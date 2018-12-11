Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmInstalacionTraspaso
    Dim oDTC As DTCDataContext
    Dim oLinqPropuesta_Linea As Propuesta_Linea
    Dim oLinqPropuesta As Propuesta
    Dim oLinqInstalacion As Instalacion
    Dim RowsSeleccionadas As New ArrayList

    Public Structure StructOriginalClon
        Dim IDOriginal As Integer
        Dim IDClon As Integer
    End Structure

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar

        Me.GRD_Ficheros.GRID.Selected.Rows.Clear()
        Me.GRD_Ficheros.GRID.ActiveRow = Nothing

        If RowsSeleccionadas.Count = 0 Then
            Mensaje.Mostrar_Mensaje("Error. Tiene que haber al menos una línea seleccionada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If
        If Mensaje.Mostrar_Mensaje("¿Desea traspasar la propuesta al estado traspasado y traspasar las líneas seleccionadas?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
            Util.WaitFormObrir()
            oLinqPropuesta.ID_Propuesta_Estado = EnumPropuestaEstado.Traspasado

            'Aquestes línies són per posar l'estat Propuesta_CRM si te una equivalencia amb  l'estat de la propuesta
            Dim _EstatCRM As Propuesta_EstadoCRM = oDTC.Propuesta_EstadoCRM.Where(Function(F) F.ID_Propuesta_Estado = CInt(EnumPropuestaEstado.Traspasado)).FirstOrDefault()
            If _EstatCRM Is Nothing = False Then 'Només si hem trobat un EstatCRM equivalment al ESTAT de la proposta cambiada llavors cambiarem l'estat CRM
                oLinqPropuesta.Propuesta_EstadoCRM = _EstatCRM
            End If


            If oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).Count = 0 Then 'si no hi ha cap proposta amb seinstalo s'ha de crear
                Call CrearPropuestaSeInstalo(oLinqPropuesta.ID_Propuesta)
            End If

            Call CrearLineasPropuestaSeInstalo(oLinqPropuesta.ID_Propuesta)
            Call CrearPlanosPropuestaSeInstalo(oLinqPropuesta.ID_Propuesta)
            Call CrearFicherosPropuestaSeInstalo()

            'assignem la propuesta que estem creant a la taula instalacion. Per tenir una relació única entre la instalació i la propuesta de tipus tal y como se instalo
            Dim PropuestaSeInstalo As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
            oLinqInstalacion.ID_Propuesta = PropuestaSeInstalo.ID_Propuesta

            oDTC.SubmitChanges()

            Util.WaitFormTancar()
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

    Public Sub Entrada(ByRef pInstalacion As Instalacion, ByRef pPropuesta As Propuesta, ByRef pDTC As DTCDataContext, Optional ByVal pId As Integer = 0)

        Try

            Me.AplicarDisseny()

            oLinqInstalacion = pInstalacion
            oLinqPropuesta = pPropuesta

            oDTC = pDTC
            'oDTC = New DTCDataContext(BD.Conexion)
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Traspasar"

            Call CargaGrid_Lineas(oLinqPropuesta.ID_Propuesta)
            Call CargaGrid_Ficheros()

            ' Me.GRD_Lineas.GRID.ActiveRow = Nothing
            ' Me.GRD_Lineas.GRID.Selected.Rows.Clear()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Private Sub CrearPropuestaSeInstalo(ByVal pIDPropuestaOriginal As Integer)
        Try
            Dim PropuestaOriginal As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = pIDPropuestaOriginal).FirstOrDefault
            Dim _Propuesta As New Propuesta

            With _Propuesta
                .Activo = True
                .Base = 0
                .Codigo = 0
                .ConectadoCRA = False
                .Descripcion = 0
                .Descuento = 0
                .Fecha = Now.Date
                .ID_Grado_Notificacion = PropuestaOriginal.ID_Grado_Notificacion
                .ID_Instalacion = PropuestaOriginal.ID_Instalacion
                .ID_Producto_Grado = PropuestaOriginal.ID_Producto_Grado
                .ID_Propuesta_Estado = EnumPropuestaEstado.Pendiente
                .ID_Propuesta_Tipo = 1
                .SegunNormativa = PropuestaOriginal.SegunNormativa
                .SeInstalo = True
                .Version = 0
                .Empresa = PropuestaOriginal.Empresa
            End With
            oLinqInstalacion.Propuesta.Add(_Propuesta)
            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub RecursivitatLineas(ByVal pLinea As Propuesta_Linea, ByRef pPropuestaSeInstalo As Propuesta, ByRef pRelacionsOriginalAmbClon As ArrayList)
        Dim _Linea2 As Propuesta_Linea
        For Each _Linea2 In pLinea.Propuesta_Linea1
            Call CreaLineasPropuestaSeInstalo(_Linea2, pPropuestaSeInstalo, pRelacionsOriginalAmbClon)
            Call RecursivitatLineas(_Linea2, pPropuestaSeInstalo, pRelacionsOriginalAmbClon)
        Next
    End Sub

    Private Sub CreaLineasPropuestaSeInstalo(ByVal pLinea As Propuesta_Linea, ByRef pPropuestaSeInstalo As Propuesta, ByRef pRelacionsOriginalAmbClon As ArrayList)
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

                    If _Linea.ID_Propuesta_Linea_Vinculado_Energetico Is Nothing = False Then
                        'El If de baix es per comprovar que la línea s'ha traspasat. Pq potser que el pare tingues la marca de família que no s'ha de traspasar i el fill llavors no s'ha de vincular
                        If IsNothing(pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado_Energetico) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault) = True Then
                            ' DescartarLinea = True
                        Else
                            .ID_Propuesta_Linea_Vinculado_Energetico = pPropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado_Energetico) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault.ID_Propuesta_Linea
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
                    .ID_Archivo_FotoPredeterminada = _Linea.ID_Archivo_FotoPredeterminada


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


                    .VLAN = _Linea.VLAN
                    .IP = _Linea.IP
                    .MascaraSubred = _Linea.MascaraSubred
                    .PuertaEnlace = _Linea.PuertaEnlace
                    .DNSPrimaria = _Linea.DNSPrimaria
                    .DNSSecundaria = _Linea.DNSSecundaria
                    .IPPublica = _Linea.IPPublica
                    .ServidorWINS = _Linea.ServidorWINS
                    .Dominio = _Linea.Dominio
                    .NombreEquipo = _Linea.NombreEquipo
                    .NetBios = _Linea.NetBios
                    .SistemaOperativo = _Linea.SistemaOperativo
                    .AlmacenamientoEnDisco = _Linea.AlmacenamientoEnDisco
                    .MemoriaRam = _Linea.MemoriaRam
                    .Procesador = _Linea.Procesador
                    .MacAdress = _Linea.MacAdress

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


                    ''vinculació de la línea de tal i como se instaló amb la línea de comanda (si es que realment l'usuari ha volgut generar la comanda automàticament)
                    'If _Linea.Entrada_Linea Is Nothing = False Then
                    '    Dim _new As New Entrada_Linea_Propuesta_Linea
                    '    _new.Entrada_Linea = _Linea.Entrada_Linea
                    '    _NewLinea.Entrada_Linea_Propuesta_Linea.Add(_new)
                    'End If

                End With
                pPropuestaSeInstalo.Propuesta_Linea.Add(_NewLinea)
                oDTC.SubmitChanges()

                'Fem una taula per guardar el ID linea del orginal i el ID del clonat. Així podrem fer les vinculacions de les línies posteriorment
                Dim pepe As New StructOriginalClon
                pepe.IDClon = _NewLinea.ID_Propuesta_Linea
                pepe.IDOriginal = pLinea.ID_Propuesta_Linea
                pRelacionsOriginalAmbClon.Add(pepe)
            Next
        End If
        'Next
    End Sub

    Private Sub CrearLineasPropuestaSeInstalo(ByVal pIDPropuestaOriginal As Integer)
        Try
            Dim PropuestaOriginal As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = pIDPropuestaOriginal).FirstOrDefault
            Dim PropuestaSeInstalo As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault

            Dim _Linea As Propuesta_Linea
            Dim _RelacionsOriginalAmbClon As New ArrayList

            'Dim _LineaVinculada As Propuesta_Linea

            For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = False)
                Call CreaLineasPropuestaSeInstalo(_Linea, PropuestaSeInstalo, _RelacionsOriginalAmbClon)
                Call RecursivitatLineas(_Linea, PropuestaSeInstalo, _RelacionsOriginalAmbClon)
                'For Each _LineaVinculada In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado = _Linea.ID_Propuesta_Linea_Vinculado)
                'Next
            Next

            ''For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso <> EnumProductoSubFamiliaTraspaso.No).OrderBy(Function(F) F.ID_Propuesta_Linea_Vinculado)
            'If _Linea.Unidad > 0 Then
            '    For i = 1 To _Linea.Unidad
            '        Dim DescartarLinea As Boolean = False
            '        Dim _NewLinea As New Propuesta_Linea
            '        With _NewLinea
            '            .Descripcion = _Linea.Descripcion
            '            .ID_Instalacion_ElementosAProteger = _Linea.ID_Instalacion_ElementosAProteger
            '            .ID_Instalacion_Emplazamiento = _Linea.ID_Instalacion_Emplazamiento
            '            .ID_Instalacion_Emplazamiento_Abertura = _Linea.ID_Instalacion_Emplazamiento_Abertura
            '            .ID_Instalacion_Emplazamiento_Planta = _Linea.ID_Instalacion_Emplazamiento_Planta
            '            .ID_Instalacion_Emplazamiento_Zona = _Linea.ID_Instalacion_Emplazamiento_Zona
            '            .ID_Producto = _Linea.ID_Producto
            '            .ID_Propuesta = PropuestaSeInstalo.ID_Propuesta

            '            If _Linea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
            '                'El If de baix es per comprovar que la línea s'ha traspasat. Pq potser que el pare tingues la marca de família que no s'ha de traspasar i el fill llavors no s'ha de vincular
            '                If IsNothing(PropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault) = True Then
            '                    DescartarLinea = True
            '                Else
            '                    .ID_Propuesta_Linea_Vinculado = PropuestaSeInstalo.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault.ID_Propuesta_Linea
            '                End If
            '            End If

            '            .Identificador = _Linea.Identificador
            '            .ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea
            '            .ID_Propuesta_Antigua = _Linea.ID_Propuesta
            '            .Precio = 0
            '            .Descuento = 0
            '            .IVA = 0
            '            .Unidad = 1
            '            .Activo = True
            '            .ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Traspasada
            '            .Uso = _Linea.Uso


            '            .NumZona = _Linea.NumZona
            '            .BocaConexion = _Linea.BocaConexion
            '            '.CantidadPendienteRecibir = _Linea.CantidadPendienteRecibir
            '            .DescripcionAmpliada = _Linea.DescripcionAmpliada
            '            .DetalleInstalacion = _Linea.DetalleInstalacion
            '            '.FechaPrevista = _Linea.FechaPrevista
            '            .IdentificadorDelProducto = _Linea.IdentificadorDelProducto
            '            .NickZona = _Linea.NickZona
            '            .NumSerie = _Linea.NumSerie
            '            .Particion = _Linea.Particion
            '            .PrecioCoste = _Linea.PrecioCoste
            '            .RutaOrden = _Linea.RutaOrden
            '            .RutaParametros = _Linea.RutaParametros
            '            .ID_Propuesta_Linea_TipoZona = _Linea.ID_Propuesta_Linea_TipoZona
            '        End With
            '        PropuestaSeInstalo.Propuesta_Linea.Add(_NewLinea)
            '        oDTC.SubmitChanges()
            '    Next
            'End If
            ''Next

            oDTC.SubmitChanges()

            'Bucle per posar correctament la vinculació energètica
            For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado_Energetico.HasValue = False)
                Call RecursivitatLineasEnergeticas(_Linea, _RelacionsOriginalAmbClon)
            Next

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub RecursivitatLineasEnergeticas(ByRef pLinea As Propuesta_Linea, ByRef pRelacionsOriginalAmbClon As ArrayList)
        Dim _Linea2 As Propuesta_Linea
        For Each _Linea2 In pLinea.Propuesta_Linea_VinculadoEnergeticoHijo
            Call AssignacioEnergetica(_Linea2, pRelacionsOriginalAmbClon)
            Call RecursivitatLineasEnergeticas(_Linea2, pRelacionsOriginalAmbClon)
        Next
    End Sub

    Private Sub AssignacioEnergetica(ByRef pLinea As Propuesta_Linea, ByRef pRelacionsOriginalAmbClon As ArrayList)
        If pLinea.ID_Propuesta_Linea_Vinculado_Energetico Is Nothing = False Then
            Dim _IDClonFill As Integer
            Dim _IDClonPare As Integer
            Dim _Struct As StructOriginalClon
            For Each _Struct In pRelacionsOriginalAmbClon
                If _Struct.IDOriginal = pLinea.ID_Propuesta_Linea_Vinculado_Energetico Then
                    '.Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Struct.IDClon).FirstOrDefault
                    '                    Dim _LineaClonada As Propuesta_Linea=oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea=)

                    'pLinea.ID_Propuesta_Linea_Vinculado_Energetico = _Struct.IDClon
                    _IDClonPare = _Struct.IDClon 'Aquest ID és la ID_Propuesta_Linea_vinculacio_energetica del pressupost clonat

                End If

                If _Struct.IDOriginal = pLinea.ID_Propuesta_Linea Then
                    _IDClonFill = _Struct.IDClon
                End If
            Next
            Dim _LineaClonada As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _IDClonFill).FirstOrDefault
            _LineaClonada.ID_Propuesta_Linea_Vinculado_Energetico = _IDClonPare
        End If
    End Sub

    Private Sub CrearPlanosPropuestaSeInstalo(ByVal pIDPropuestaOriginal As Integer)
        Try
            Dim PropuestaOriginal As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.ID_Propuesta = pIDPropuestaOriginal).FirstOrDefault
            Dim PropuestaSeInstalo As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault

            Dim _Plano As Propuesta_Plano


            For Each _Plano In PropuestaOriginal.Propuesta_Plano
                Dim _NewPlano As New Propuesta_Plano
                With _NewPlano
                    .Descripcion = _Plano.Descripcion
                    .FechaCreacion = _Plano.FechaCreacion
                    .Validado = _Plano.Validado

                    .ID_Propuesta = PropuestaSeInstalo.ID_Propuesta
                    .ID_Propuesta_Antigua = _Plano.ID_Propuesta
                    .Propuesta_Version_Antigua = _Plano.Propuesta.Version

                    .ID_Instalacion_Emplazamiento = _Plano.ID_Instalacion_Emplazamiento
                    .ID_Instalacion_Emplazamiento_Planta = _Plano.ID_Instalacion_Emplazamiento_Planta
                    .ID_Instalacion_Emplazamiento_Zona = _Plano.ID_Instalacion_Emplazamiento_Zona

                    If IsNothing(_Plano.ID_PlanoBinario) = False Then
                        Dim _PlanoBinario As New PlanoBinario
                        _PlanoBinario.Fichero = _Plano.PlanoBinario.Fichero
                        _PlanoBinario.Foto = _Plano.PlanoBinario.Foto
                        .PlanoBinario = _PlanoBinario
                        oDTC.PlanoBinario.InsertOnSubmit(_PlanoBinario)
                    End If
                End With



                PropuestaSeInstalo.Propuesta_Plano.Add(_NewPlano)
                oDTC.SubmitChanges()
            Next

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CrearFicherosPropuestaSeInstalo()
        Dim PropuestaSeInstalo As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault

        Dim pRow As UltraWinGrid.UltraGridRow
        For Each pRow In Me.GRD_Ficheros.GRID.Rows
            If pRow.Cells("Seleccion").Value = True Then
                Dim _Archivo As Archivo
                _Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = CInt(pRow.Cells("ID_Archivo").Value)).FirstOrDefault

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
                _NewPropuestaArchivo.Propuesta = PropuestaSeInstalo
                PropuestaSeInstalo.Propuesta_Archivo.Add(_NewPropuestaArchivo)
                oDTC.Archivo.InsertOnSubmit(_NewArchivo)
                oDTC.Propuesta_Archivo.InsertOnSubmit(_NewPropuestaArchivo)
            End If
        Next
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Lineas"
    Public Sub CargaGrid_Lineas(ByVal pIDPropuesta As Integer)
        ' Dim _Propuesta As Propuesta = oLinqInstalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        'If _Propuesta Is Nothing = False Then
        Dim DTS As New DataSet
        BD.CargarDataSet(DTS, "Select *, cast(0 as bit) as Seleccion From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta=" & pIDPropuesta)
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

#Region "Grid Ficheros"

    Public Sub CargaGrid_Ficheros()
        Try


            Dim DT As DataTable
            DT = BD.RetornaDataTable("SELECT cast(0 as bit) as Seleccion, Archivo.ID_Archivo, Archivo.Descripcion, Archivo.Tipo, Archivo.Fecha as Fecha, Archivo.Tamaño FROM Archivo INNER JOIN Propuesta_Archivo ON Archivo.ID_Archivo = Propuesta_Archivo.ID_Archivo WHERE Archivo.Activo = 1 and ID_Propuesta_Archivo=" & oLinqPropuesta.ID_Propuesta & " ORDER BY Archivo.Descripcion")
            Me.GRD_Ficheros.M.clsUltraGrid.Cargar(DT)
            If DT Is Nothing = False Then
                Me.GRD_Ficheros.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
                Me.GRD_Ficheros.GRID.DisplayLayout.Bands(0).Columns("Fecha").CellClickAction = CellClickAction.CellSelect
                Me.GRD_Ficheros.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellClickAction = CellClickAction.Edit
                Me.GRD_Ficheros.GRID.DisplayLayout.Bands(0).Columns("Seleccion").CellActivation = Activation.AllowEdit
            End If
            ' Dim Lineas = oLinqPropuesta_Linea.par
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region


End Class