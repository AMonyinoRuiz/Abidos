Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmInstalacionFusionarPropuestas
    Dim oDTC As DTCDataContext
    Dim oListaPropuestas As List(Of Propuesta)
    '  Dim oPropuestaMare As Propuesta
    Dim oLinqInstalacion As Instalacion

    Public Structure StructOriginalClon
        Dim IDOriginal As Integer
        Dim IDClon As Integer
    End Structure


#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar

        If Me.T_Descripcion.TextLength = 0 Or Me.T_Persona.TextLength = 0 Then
            Mensaje.Mostrar_Mensaje("La descripción de la propuesta y la persona a la que va dirigida son campos obligatorios", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        If Mensaje.Mostrar_Mensaje("¿Desea fusionar las propuestas seleccionadas?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
            Util.WaitFormObrir()

            Call DuplicarPressupost(oListaPropuestas(0).ID_Propuesta, oLinqInstalacion)

            Util.WaitFormTancar()
            Mensaje.Mostrar_Mensaje("Propuestas fusionadas correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)

        End If
        Call M_ToolForm1_m_ToolForm_Sortir()
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pInstalacion As Instalacion, ByRef pListaPropuestas As List(Of Propuesta), ByRef pDTC As DTCDataContext, Optional ByVal pId As Integer = 0)

        Try

            Me.AplicarDisseny()

            oLinqInstalacion = pInstalacion
            oListaPropuestas = pListaPropuestas
            ' oLinqPropuesta = pPropuesta

            oDTC = pDTC
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Fusionar"

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Function DuplicarPressupost(ByVal pIDPressupost As Integer, ByRef pInstalacion As Instalacion) As Boolean
        Try
            DuplicarPressupost = False

            Dim Original As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = pIDPressupost).FirstOrDefault
            Dim Clon As New Propuesta
            With Original
                Clon.Activo = True
                ' Clon.Base = .Base
                Clon.Codigo = oDTC.Contadores.FirstOrDefault.Propuesta
                oDTC.Contadores.FirstOrDefault.Propuesta = oDTC.Contadores.FirstOrDefault.Propuesta + 1
                Clon.ConectadoCRA = .ConectadoCRA
                Clon.Descripcion = Me.T_Descripcion.Text
                Clon.Descuento = .Descuento
                Clon.Fecha = Now.Date
                Clon.FormaPago = .FormaPago

                If oLinqInstalacion.ID_Instalacion = pInstalacion.ID_Instalacion Then   'si s'esta duplicant la proposta a unaltre instalació llavors no posarem els planta y zona pero buscarem la predeterminada
                    Clon.Instalacion_Emplazamiento = .Instalacion_Emplazamiento
                    Clon.Instalacion_Emplazamiento_Planta = .Instalacion_Emplazamiento_Planta
                    Clon.Instalacion_Emplazamiento_Zona = .Instalacion_Emplazamiento_Zona
                Else
                    Clon.Instalacion_Emplazamiento = pInstalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                End If

                Clon.Grado_Notificacion = .Grado_Notificacion
                Clon.Instalacion = .Instalacion
                Clon.IVA = .IVA
                Clon.NivelMaximoLineas = .NivelMaximoLineas
                Clon.Observaciones = .Observaciones
                Clon.Persona = Me.T_Persona.Text
                Clon.Personal = .Personal
                Clon.Producto_Grado = .Producto_Grado
                Clon.Propuesta = .Propuesta
                Clon.Propuesta_Estado = oDTC.Propuesta_Estado.Where(Function(F) F.ID_Propuesta_Estado = CInt(EnumPropuestaEstado.Pendiente)).FirstOrDefault
                Clon.Propuesta_Tipo = .Propuesta_Tipo
                Clon.Receptora = .Receptora
                Clon.SegunNormativa = .SegunNormativa
                Clon.SeInstalo = False
                Clon.TiempoInstalacion = .TiempoInstalacion
                Clon.Total = .Total
                Clon.Validez = .Validez
                'If pInstalacion.ID_Instalacion <> oLinqInstalacion.ID_Instalacion Then  'Si copiem el pressupost en unaltre instalació sempre serà la versió A
                Clon.Version = "A"
                'Else
                '    Clon.Version = Chr(Asc(Original.Version) + 1)
                'End If
                Clon.Empresa = .Empresa
                Clon.DetalleExtendido = .DetalleExtendido
                Clon.HorasPrevistas = .HorasPrevistas

                pInstalacion.Propuesta.Add(Clon)
            End With



            If Me.CH_TraspasarPlanos.Checked = True Then
                Call CrearPlanosPressupost(Original, Clon)
            End If

            Call CrearDiagramasPressupost(Original, Clon)

            Dim _Propuesta As Propuesta
            For Each _Propuesta In oListaPropuestas
                Call DuplicarLineasPressupost(_Propuesta, Clon)
            Next

            If Me.CH_TraspasarFicheros.Checked = True Then
                Call CrearFicherosPressupost(Original, Clon)
            End If

            Call CrearEspecificaciones(Original, Clon)

            Call CrearSeguridad(Original, Clon)



            oDTC.SubmitChanges()

            'Calcular totals pressupost
            If Clon.Propuesta_Linea.Count = 0 Then
                Clon.Base = 0
                Clon.IVA = 0
                Clon.TiempoInstalacion = 0
                Clon.Descuento = 0
            Else
                Clon.Base = Clon.RetornaTotalBase
                Clon.Total = Clon.RetornaTotalPropuesta
                Clon.TiempoInstalacion = Math.Round(oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta = Clon.ID_Propuesta And F.ID_Propuesta_Opcion.HasValue = False).Sum(Function(F) F.Unidad * F.Producto.TiempoInstalacion), 0)
            End If

            oDTC.SubmitChanges()

            Dim _Ordre As Integer = 1
            Dim _Linea As Propuesta_Linea
            For Each _Linea In Clon.Propuesta_Linea
                _Linea.Identificador = _Ordre
                _Ordre = _Ordre + 1
            Next

            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub CrearPlanosPressupost(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _Plano As Propuesta_Plano


            For Each _Plano In pPropuestaOriginal.Propuesta_Plano
                Dim _NewPlano As New Propuesta_Plano
                With _NewPlano
                    .Descripcion = _Plano.Descripcion
                    .FechaCreacion = _Plano.FechaCreacion
                    .Validado = _Plano.Validado

                    '.ID_Propuesta = PropuestaSeInstalo.ID_Propuesta
                    '.ID_Propuesta_Antigua = _Plano.ID_Propuesta
                    '.Propuesta_Version_Antigua = _Plano.Propuesta.Version

                    If pPropuestaOriginal.ID_Instalacion = pPropuestaClon.ID_Instalacion Then   'si s'esta duplicant la proposta a unaltre instalació llavors no posarem els planta y zona pero buscarem la predeterminada
                        .ID_Instalacion_Emplazamiento = _Plano.ID_Instalacion_Emplazamiento
                        .ID_Instalacion_Emplazamiento_Planta = _Plano.ID_Instalacion_Emplazamiento_Planta
                        .ID_Instalacion_Emplazamiento_Zona = _Plano.ID_Instalacion_Emplazamiento_Zona
                    Else
                        .Instalacion_Emplazamiento = pPropuestaClon.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                    End If

                    If IsNothing(_Plano.ID_PlanoBinario) = False Then
                        Dim _PlanoBinario As New PlanoBinario
                        _PlanoBinario.Fichero = _Plano.PlanoBinario.Fichero
                        _PlanoBinario.Foto = _Plano.PlanoBinario.Foto
                        .PlanoBinario = _PlanoBinario

                        'No està fet pq es força complexe.
                        'Dim _ElementoIntroducido As Propuesta_Plano_ElementosIntroducidos
                        'For Each _ElementoIntroducido In _Plano.Propuesta_Plano_ElementosIntroducidos
                        '    Dim _NewElemento As Propuesta_Plano_ElementosIntroducidos
                        '    _NewElemento.

                        'Next

                        oDTC.PlanoBinario.InsertOnSubmit(_PlanoBinario)
                    End If


                End With



                pPropuestaClon.Propuesta_Plano.Add(_NewPlano)
                'oDTC.SubmitChanges()
            Next

            '  oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CrearDiagramasPressupost(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _Plano As Propuesta_Diagrama


            For Each _Plano In pPropuestaOriginal.Propuesta_Diagrama
                Dim _NewDiagrama As New Propuesta_Diagrama
                With _NewDiagrama
                    .Descripcion = _Plano.Descripcion
                    .FechaCreacion = _Plano.FechaCreacion
                    .Validado = _Plano.Validado

                    '.ID_Propuesta = PropuestaSeInstalo.ID_Propuesta
                    '.ID_Propuesta_Antigua = _Plano.ID_Propuesta
                    '.Propuesta_Version_Antigua = _Plano.Propuesta.Version

                    If pPropuestaOriginal.ID_Instalacion = pPropuestaClon.ID_Instalacion Then   'si s'esta duplicant la proposta a unaltre instalació llavors no posarem els planta y zona pero buscarem la predeterminada
                        .ID_Instalacion_Emplazamiento = _Plano.ID_Instalacion_Emplazamiento
                        .ID_Instalacion_Emplazamiento_Planta = _Plano.ID_Instalacion_Emplazamiento_Planta
                        .ID_Instalacion_Emplazamiento_Zona = _Plano.ID_Instalacion_Emplazamiento_Zona
                    Else
                        .Instalacion_Emplazamiento = pPropuestaClon.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                    End If

                    If IsNothing(_Plano.ID_DiagramaBinario) = False Then
                        Dim _PlanoBinario As New DiagramaBinario
                        _PlanoBinario.Fichero = _Plano.DiagramaBinario.Fichero
                        _PlanoBinario.Foto = _Plano.DiagramaBinario.Foto
                        .DiagramaBinario = _PlanoBinario

                        'No està fet pq es força complexe.
                        'Dim _ElementoIntroducido As Propuesta_Plano_ElementosIntroducidos
                        'For Each _ElementoIntroducido In _Plano.Propuesta_Plano_ElementosIntroducidos
                        '    Dim _NewElemento As Propuesta_Plano_ElementosIntroducidos
                        '    _NewElemento.

                        'Next

                        oDTC.DiagramaBinario.InsertOnSubmit(_PlanoBinario)
                    End If


                End With



                pPropuestaClon.Propuesta_Diagrama.Add(_NewDiagrama)
                'oDTC.SubmitChanges()
            Next

            '  oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub


    Private Sub CrearEspecificaciones(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _Resposta As Propuesta_PropuestaEspecificacion

            For Each _Resposta In pPropuestaOriginal.Propuesta_PropuestaEspecificacion
                Dim _NewResposta As New Propuesta_PropuestaEspecificacion
                With _NewResposta
                    .FechaRespuesta = _Resposta.FechaRespuesta
                    .PropuestaEspecificacion = _Resposta.PropuestaEspecificacion
                    .PropuestaEspecificacion_Respuesta = _Resposta.PropuestaEspecificacion_Respuesta
                    .Observaciones = _Resposta.Observaciones
                    .Usuario = _Resposta.Usuario
                End With

                pPropuestaClon.Propuesta_PropuestaEspecificacion.Add(_NewResposta)
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CrearSeguridad(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _Resposta As Propuesta_Seguridad

            For Each _Resposta In pPropuestaOriginal.Propuesta_Seguridad
                Dim _NewResposta As New Propuesta_Seguridad
                With _NewResposta
                    .Usuario = _Resposta.Usuario
                End With

                pPropuestaClon.Propuesta_Seguridad.Add(_NewResposta)
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub DuplicarLineasPressupost(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim RelacionsOriginalAmbClon As New ArrayList

            Dim _Linea As Propuesta_Linea

            For Each _Linea In pPropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado.HasValue = False).OrderBy(Function(F) F.Identificador)
                Call CreaLineasPressupost(_Linea, pPropuestaClon, RelacionsOriginalAmbClon)
                Call RecursivitatLineas(_Linea, pPropuestaClon, RelacionsOriginalAmbClon)
            Next
            oDTC.SubmitChanges()
            'Bucle per posar correctament la vinculació energètica
            For Each _Linea In pPropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea_Vinculado_Energetico.HasValue = False)
                Call RecursivitatLineasEnergeticas(_Linea, RelacionsOriginalAmbClon)
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

    Private Sub RecursivitatLineas(ByVal pLinea As Propuesta_Linea, ByRef pPropuestaSeInstalo As Propuesta, ByRef pRelacionsOriginalAmbClon As ArrayList)
        Dim _Linea2 As Propuesta_Linea
        For Each _Linea2 In pLinea.Propuesta_Linea1.OrderBy(Function(F) F.Identificador)
            Call CreaLineasPressupost(_Linea2, pPropuestaSeInstalo, pRelacionsOriginalAmbClon)
            Call RecursivitatLineas(_Linea2, pPropuestaSeInstalo, pRelacionsOriginalAmbClon)
        Next
    End Sub

    Private Sub CreaLineasPressupost(ByVal pLinea As Propuesta_Linea, ByRef pPropuestaClon As Propuesta, ByRef pRelacionsOriginalAmbClon As ArrayList)
        Try
            'For Each _Linea In PropuestaOriginal.Propuesta_Linea.Where(Function(F) F.ID_Producto_SubFamilia_Traspaso <> EnumProductoSubFamiliaTraspaso.No).OrderBy(Function(F) F.ID_Propuesta_Linea_Vinculado)
            If pLinea.Unidad > 0 Then
                Dim DescartarLinea As Boolean = False
                Dim _NewLinea As Propuesta_Linea = clsPropuestaLinea.RetornaDuplicacioInstancia(pLinea, False)
                With _NewLinea

                    '.Propuesta = pPropuestaClon.Propuesta
                    .ID_Propuesta = pPropuestaClon.ID_Propuesta

                    If oLinqInstalacion.ID_Instalacion = pPropuestaClon.ID_Instalacion Then   'si s'esta duplicant la proposta a unaltre instalació llavors no posarem els planta y zona pero buscarem la predeterminada
                        .ID_Instalacion_Emplazamiento = pLinea.ID_Instalacion_Emplazamiento
                        .ID_Instalacion_Emplazamiento_Planta = pLinea.ID_Instalacion_Emplazamiento_Planta
                        .ID_Instalacion_Emplazamiento_Zona = pLinea.ID_Instalacion_Emplazamiento_Zona
                    Else
                        .Instalacion_Emplazamiento = pPropuestaClon.Instalacion.Instalacion_Emplazamiento.Where(Function(F) F.Predeterminada = True).FirstOrDefault
                    End If

                    If pLinea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
                        Dim _Struct As StructOriginalClon
                        For Each _Struct In pRelacionsOriginalAmbClon
                            If _Struct.IDOriginal = pLinea.ID_Propuesta_Linea_Vinculado Then
                                '.Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = _Struct.IDClon).FirstOrDefault
                                .ID_Propuesta_Linea_Vinculado = _Struct.IDClon
                            End If
                        Next
                    End If

                    .ID_Propuesta_Linea_Vinculado_Energetico = Nothing 'Posem això pq al duplicar copia el ID però com que estem creant línies noves no ens interesa el ID de la línea original. Per això després farem un bucle per introduir els ID's correctament

                    'If _Linea.ID_Propuesta_Linea_Vinculado Is Nothing = False Then
                    '    'El If de baix es per comprovar que la línea s'ha traspasat. Pq potser que el pare tingues la marca de família que no s'ha de traspasar i el fill llavors no s'ha de vincular
                    '    If IsNothing(pPropuestaClon.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Linea_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault) = True Then
                    '        ' DescartarLinea = True
                    '    Else
                    'If _Linea.Activo = False Then
                    '    .ID_Propuesta_Linea_Vinculado = pPropuestaClon.Propuesta_Linea.Where(Function(F) (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Linea_Antigua = _Linea.ID_Propuesta_Linea_Vinculado) And (F.ID_Propuesta_Antigua.HasValue = True AndAlso F.ID_Propuesta_Antigua = _Linea.ID_Propuesta)).FirstOrDefault.ID_Propuesta_Linea
                    'End If
                    '   End If
                    'End If

                    .Activo = True
                    .ID_Propuesta_Linea_Estado = EnumPropuestaLineaEstado.Aprobado_PendienteRecibir
                End With

                pPropuestaClon.Propuesta_Linea.Add(_NewLinea)
                oDTC.SubmitChanges()

                'Fem una taula per guardar el ID linea del orginal i el ID del clonat. Així podrem fer les vinculacions de les línies posteriorment
                Dim pepe As New StructOriginalClon
                pepe.IDClon = _NewLinea.ID_Propuesta_Linea
                pepe.IDOriginal = pLinea.ID_Propuesta_Linea
                pRelacionsOriginalAmbClon.Add(pepe)

            End If
            'Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CrearFicherosPressupost(ByVal pPropuestaOriginal As Propuesta, ByRef pPropuestaClon As Propuesta)
        Try

            Dim _PropuestaArchivo As Propuesta_Archivo

            For Each _PropuestaArchivo In pPropuestaOriginal.Propuesta_Archivo
                Dim _NewPropuestaArchivo As New Propuesta_Archivo
                With _NewPropuestaArchivo
                    If IsNothing(_PropuestaArchivo.ID_Archivo) = False Then
                        Dim _NewArchivo As New Archivo
                        _NewArchivo.Activo = _PropuestaArchivo.Archivo.Activo
                        _NewArchivo.CampoBinario = _PropuestaArchivo.Archivo.CampoBinario
                        _NewArchivo.Color = _PropuestaArchivo.Archivo.Color
                        _NewArchivo.Descripcion = _PropuestaArchivo.Archivo.Descripcion
                        _NewArchivo.Fecha = _PropuestaArchivo.Archivo.Fecha
                        _NewArchivo.ID_Usuario = _PropuestaArchivo.Archivo.ID_Usuario
                        _NewArchivo.Ruta_Fichero = _PropuestaArchivo.Archivo.Ruta_Fichero
                        _NewArchivo.Tamaño = _PropuestaArchivo.Archivo.Tamaño
                        _NewArchivo.Tipo = _PropuestaArchivo.Archivo.Tipo
                        .Archivo = _NewArchivo
                        ' .Propuesta = pPropuestaClon
                        pPropuestaClon.Propuesta_Archivo.Add(_NewPropuestaArchivo)
                        oDTC.Propuesta_Archivo.InsertOnSubmit(_NewPropuestaArchivo)
                        oDTC.Archivo.InsertOnSubmit(_NewArchivo)
                    End If
                End With

            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

End Class