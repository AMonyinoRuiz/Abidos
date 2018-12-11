Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports System.Text
Imports System.ComponentModel
Imports System.Collections
Imports DevExpress.XtraTreeList

Public Class frmParte
    Implements IDisposable

    Dim oDTC As DTCDataContext
    Dim oLinqParte As Parte
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2
    Dim oTreeTipusManteniments As Projects
    Dim GridTrabajosARealizarIndentat As Boolean = True
    Enum EstatPantalla As Integer
        Nuevo = 1
        DespresDeCargar = 2
        DespresDeGuardar = 3
    End Enum

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqParte.ID_Parte <> 0 Then
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    ' Call Guardar()

                    'Si hi ha alguna reparació que tingui data vol dir que ja s'ha arreglat algo i no es podrà eliminar el part de treball
                    If oLinqParte.Parte_Reparacion.Where(Function(F) F.Fecha.HasValue = True).Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible eliminar éste parte de trabajo, hay lineas de reparación realizadas.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    If oLinqParte.Parte_Horas.Count > 0 Then
                        Mensaje.Mostrar_Mensaje("Imposible eliminar éste parte de trabajo, hay horas imputadas a este parte de trabajo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Exit Sub
                    End If

                    'Si hi ha alguna linea de parte de revisió que te alguna linea de parte de reparació que estem borrant actualment haurem d'eliminar el ID de les revisions
                    Dim Lineas = oDTC.Parte_Revision.Where(Function(F) F.Parte_Reparacion.ID_Parte = oLinqParte.ID_Parte)
                    If Lineas.Count <> 0 Then
                        If Lineas.FirstOrDefault.Parte.ID_Parte_Estado = EnumParteEstado.Finalizado Then
                            Mensaje.Mostrar_Mensaje("Imposible eliminar un parte de trabajo que proviene de un parte de revisión en estado finalizado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                            Exit Sub
                        End If
                        Dim _Linea As Parte_Revision
                        For Each _Linea In Lineas
                            _Linea.Parte_Reparacion = Nothing
                        Next
                    End If

                    'Comprovem que el parte tingui un magatzem i si te magatzem comprovem que no hi hagi material
                    Dim _MagatzemParte As Almacen
                    _MagatzemParte = oLinqParte.Almacen.FirstOrDefault
                    If _MagatzemParte Is Nothing = False Then
                        If oDTC.RetornaStock(0, _MagatzemParte.ID_Almacen).Count = 0 Then 'Si te magatzem, i el magatzem no te stock, eliminarem el magatzem
                            _MagatzemParte.Activo = False
                        Else
                            Mensaje.Mostrar_Mensaje("Imposible eliminar el parte, el almacén relacionado a este parte todavía tiene stock", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                            Exit Sub
                        End If
                    End If

                    oLinqParte.Activo = False
                    Me.TAB_Principal.Tabs("General").Selected = True 'posem això pq no peti al fer invisible o disabled les pestanyes
                    oDTC.SubmitChanges()
                    Call Netejar_Pantalla()
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
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

    Private Sub ToolForm_m_ToolForm_Imprimir() Handles ToolForm.m_ToolForm_Imprimir
        If Guardar() = False Then
            Exit Sub
        End If

        Informes.ObrirInformePreparacio(oDTC, EnumInforme.ParteTrabajo, "[ID_Parte] = " & oLinqParte.ID_Parte, "Parte - " & oLinqParte.ID_Parte)

        'Dim frm As New frmInformePreparacio
        'frm.Entrada(EnumInforme.ParteTrabajo, "[ID_Parte] = " & oLinqParte.ID_Parte)
        'frm.FormObrir(Me, False)
    End Sub

    Private Sub ToolForm_m_ToolForm_Seleccionar() Handles ToolForm.m_ToolForm_Seleccionar
        Try
            If Me.C_Estado.Value = EnumParteEstado.Pendiente Then
                Mensaje.Mostrar_Mensaje("No se puede finalizar un parte en estado pendiente", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If Me.C_Estado.Value = EnumParteEstado.Finalizado Then
                Mensaje.Mostrar_Mensaje("El parte ya está finalizado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            'Si hi ha alguna reparació sense fecha vol dir que encara s'ha de solucionar, per tant no deixarem finalitzar el parte
            If oLinqParte.Parte_Reparacion.Where(Function(F) F.Fecha.HasValue = False).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible finalizar el parte, todavía hay reparaciones pendientes de solucionar", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If oLinqParte.Parte_Revision.Where(Function(F) F.ID_Parte_Revision_Estado = EnumParteRevisionEstado.Incorrecto Or F.ID_Parte_Revision_Estado = EnumParteRevisionEstado.PendienteRevisar).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible finalizar el parte, todavía hay revisiones incorrectas o pendientes de revisar", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If oLinqParte.Parte_ToDo.Where(Function(F) F.Realizado = False).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible finalizar el parte, todavía hay To Do's pendientes de realizar", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If Guardar() = False Then
                Exit Sub
            End If

            oLinqParte.Parte_Estado = oDTC.Parte_Estado.Where(Function(F) F.ID_Parte_Estado = EnumParteEstado.Finalizado).FirstOrDefault
            Me.C_Estado.Value = CInt(EnumParteEstado.Finalizado)
            Me.DT_Fin.Value = Now.Date
            Mensaje.Mostrar_Mensaje("El parte se ha finalizado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
            Call CalcularEstadoParte()

            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub ToolForm_m_ToolForm_ToolClick_Botones_Extras(Sender As Object, e As UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
        Try
            Select Case e.Tool.Key
                Case "Recalcular"
                    Util.WaitFormObrir()

                    Dim Llistat As IEnumerable(Of Parte) = oDTC.Parte.Where(Function(F) F.Activo = True).OrderBy(Function(F) F.FechaInicio)
                    Dim _Parte As Parte
                    Dim ContadorRevision As Integer = 1
                    Dim ContadorInstalacion As Integer = 1
                    Dim ContadorReparacion As Integer = 1

                    For Each _Parte In Llistat
                        Select Case _Parte.ID_Parte_Tipo
                            Case EnumParteTipo.Instalacion
                                _Parte.Numeracion = "I-" & Util.OmplicarCadenaAmbElCaracterDonatFinsLogitudFixadaPelDavant(ContadorInstalacion, 5, "0")
                                ContadorInstalacion = ContadorInstalacion + 1
                            Case EnumParteTipo.Reparacion
                                _Parte.Numeracion = "Rep-" & Util.OmplicarCadenaAmbElCaracterDonatFinsLogitudFixadaPelDavant(ContadorReparacion, 5, "0")
                                ContadorReparacion = ContadorReparacion + 1
                            Case EnumParteTipo.Revision
                                If _Parte.ID_Producto_Division.HasValue = True AndAlso _Parte.Producto_Division.Descripcion = "Intrusión" Then
                                    _Parte.Numeracion = "Rev-" & Util.OmplicarCadenaAmbElCaracterDonatFinsLogitudFixadaPelDavant(ContadorRevision, 5, "0")
                                    ContadorRevision = ContadorRevision + 1
                                End If
                        End Select
                    Next
                    oDTC.SubmitChanges()

                    Util.WaitFormTancar()

                Case "IrAInstalacion"
                    If Me.TL_Instalacion.Tag Is Nothing Then
                        Exit Sub
                    End If

                    Dim _IDInstalacio As Integer = Me.TL_Instalacion.Tag
                    Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                    frm.Entrada(_IDInstalacio)
                    frm.FormObrir(Me, True)

                Case "CambiarAEstadoEnCurso"
                    If Mensaje.Mostrar_Mensaje("¿Desea cambiar el estado del parte?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                        Exit Sub
                    End If

                    If Guardar() = False Then
                        Exit Sub
                    End If

                    oLinqParte.Parte_Estado = oDTC.Parte_Estado.Where(Function(F) F.ID_Parte_Estado = EnumParteEstado.EnCurso).FirstOrDefault
                    Me.C_Estado.Value = CInt(EnumParteEstado.EnCurso)
                    Me.DT_Fin.Value = Now.Date
                    Mensaje.Mostrar_Mensaje("El parte se ha cambiado de estado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                    Call CalcularEstadoParte()


            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pIdParte As Integer = 0, Optional ByVal pTrabajosARealizar As String = "")

        Try

            If Me.C_Estado.Items.Count = 0 Then 'Si no s'han carregat els estats.. els carregarem (obertura a través de la pila)
                Me.AplicarDisseny()
                Util.Cargar_Combo(Me.C_Estado, "SELECT ID_Parte_Estado, Descripcion FROM Parte_Estado WHERE Activo=1 ORDER BY Codigo", True)
                Util.Cargar_Combo(Me.C_Tipo, "SELECT ID_Parte_Tipo, Descripcion FROM Parte_Tipo WHERE Activo=1 ORDER BY Codigo", True)
                Util.Cargar_Combo(Me.C_TipoFacturacion, "SELECT ID_Parte_TipoFacturacion, Descripcion FROM Parte_TipoFacturacion WHERE Activo=1 ORDER BY Codigo", True)
                Util.Cargar_Combo(Me.C_Division, "SELECT ID_Producto_Division, Descripcion FROM Producto_Division WHERE Activo=1 ORDER BY Codigo", True)
                Me.C_Division.Value = CInt(EnumProductoDivision.Interno) 'Predeterminem intrusión per defecte
                Util.Cargar_Combo(Me.C_Pais, "SELECT ID_Pais, Nombre FROM Pais ORDER BY Nombre", True)
                Util.Cargar_Combo(Me.C_Delegacion, "SELECT ID_Delegacion, Descripcion FROM Delegacion ORDER BY Descripcion", False)

                Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Parte_Archivo", 1)
                Me.Preview_RTF.pBotoGuardarVisible = True
                'Me.GRD_Horas.M.clsToolBar.Boto_Afegir("CaracteristicasPredeterminadas", "Cargar características predeterminadas", True)



                Me.GRD_Incidencia.M.clsToolBar.Boto_Afegir("GenerarParte", "Generar parte de trabajo")
                Me.GRD_Reparacion_TalComoSeInstalo.M.clsToolBar.Boto_Afegir("MarcarArticulo", "Marcar incidencia en el artículo")
                Me.GRD_Reparacion.M.clsToolBar.Boto_Afegir("FinalizarReparacion", "Finalizar reparación")
                Me.GRD_Revision.M.clsToolBar.Boto_Afegir("MarcarLineasCorrectas", "Marcar todas las líneas a correctas")
                Me.GRD_Revision.M.clsToolBar.Boto_Afegir("GenerarParte", "Generar parte")
                Me.GRD_Revision.M.clsToolBar.Boto_Afegir("VerParte", "Visualizar parte de reparación")
                Me.ToolForm.M.clsToolBar.Botons.tSeleccionar.SharedProps.Caption = "Finalizar parte"
                Me.ToolForm.M.clsToolBar.Botons.tSeleccionar.SharedProps.Visible = True
                Me.ToolForm.M.clsToolBar.Botons.tImprimir.SharedProps.Visible = True

                Me.GRD_MantenimientoPendiente.M.clsToolBar.Boto_Afegir("AsignarAlParte", "Asignar al parte")
                Me.GRD_MantenimientoAsignado.M.clsToolBar.Boto_Afegir("MarcarLineasCorrectas", "Marcar todas las líneas a correctas")
                Me.GRD_MantenimientoAsignado.M.clsToolBar.Boto_Afegir("GenerarParte", "Generar parte")
                Me.GRD_MantenimientoAsignado.M.clsToolBar.Boto_Afegir("VerParte", "Visualizar parte de reparación")


                Me.GRD_Horas.M.clsToolBar.Boto_Afegir("VerAlbaran", "Ver albarán")
                Me.GRD_Gastos.M.clsToolBar.Boto_Afegir("VerAlbaran", "Ver albarán")
                Me.GRD_Material.M.clsToolBar.Boto_Afegir("VerAlbaran", "Ver albarán")


                Me.GRD_TrabajosARealizar.M.clsToolBar.Boto_Afegir("Indentar", "Indentación")


                Me.ToolForm.M.Botons.tAccions.SharedProps.Visible = True
                Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("IrAInstalacion", "Ir a la instalación", True)
                Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("Recalcular", "Recalcular numeración", True)
                Me.ToolForm.M.clsToolBar.Afegir_BotoAccions("CambiarAEstadoEnCurso", "Cambiar a estado en curso", True)
                'Me.ToolForm.M.clsToolBar.Afegir_Boto("IrAInstalacion", "Ir a la instalación")
                'Me.ToolForm.M.clsToolBar.Afegir_Boto("Recalcular", "Recalcular numeración")

                Me.R_ExplicacionHorasTecnico.RichText.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.PrintLayout
                Me.R_ObservacionesReparacion.RichText.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.PrintLayout


                Me.KeyPreview = False

                Me.List_Emplazamientos.Items.Clear()


                Dim BotoLupeta As New UltraWinEditors.EditorButton
                BotoLupeta.Key = "Cancelar2"
                Dim oDisseny As New M_Disseny.ClsDisseny
                BotoLupeta.Appearance.Image = Me.TL_Instalacion.ButtonsRight("Ficha").Appearance.Image   'oDisseny.Leer_Imagen("text_cancelar.gif")
                BotoLupeta.Width = 16
                BotoLupeta.Appearance.BackColor = Color.White
                BotoLupeta.Appearance.BorderAlpha = Alpha.Transparent
                Me.C_Presupuesto.ButtonsRight.Add(BotoLupeta.Clone)
                Me.TL_Instalacion.ButtonsRight("Ficha").Enabled = True



                Call CargarCombo_Bonos(True)

                Dim BotoCancelar As UltraWinEditors.EditorButton
                BotoCancelar = New UltraWinEditors.EditorButton
                BotoCancelar.Key = "Cancelar2"
                ' Dim oDisseny As New M_Disseny.ClsDisseny
                BotoCancelar.Appearance.Image = oDisseny.Leer_Imagen("text_cancelar.gif")
                BotoCancelar.Width = 16
                BotoCancelar.Appearance.BackColor = Color.White
                BotoCancelar.Appearance.BorderAlpha = Alpha.Transparent
                Me.C_Bono.ButtonsRight.Add(BotoCancelar.Clone)


                'Me.TE_Instalacion.ButtonsRight.Add(BotoCancelar.Clone)
                ' Me.TE_Parte_Vinculado.ButtonsRight.Add(BotoCancelar.Clone)
                'Me.TE_Cliente.ButtonsRight.Add(BotoCancelar.Clone)

            End If

            Util.Tab_InVisible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "MaterialNoPresupuestado") 'Fem això pq ja no utilitzarem aquesta pestanya mai mes i per que modificanto des de la propia pantalla  no fa cas

            If pIdParte <> 0 Then
                Call Cargar_Form(pIdParte)
            Else
                Call Netejar_Pantalla()
                If pTrabajosARealizar Is Nothing = False AndAlso pTrabajosARealizar.Count > 0 Then 'això està fet per a que des de la pantalla activitat es pugui passar el text de la activitat
                    Me.R_TrabajosARealizar.pText = pTrabajosARealizar
                End If
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Bonos(ByVal pTots As Boolean)
        Dim DT As DataTable
        If pTots = True Then
            DT = BD.RetornaDataTable("SELECT ID_Bono,  Codigo, DescripcionProducto, ClienteNombre, Cantidad, HorasRestantes, cast(Codigo as nvarchar(20)) + ' - ' + DescripcionProducto as Visualizacion FROM C_Bono Order by Codigo Desc") 'and ID_Instalacion in (Select ID_Instalacion From Entrada_Instalacion Where ID_Entrada=" & oclsEntradaLinea.oLinqEntrada.ID_Entrada & " Group by ID_Instalacion)
        Else
            DT = BD.RetornaDataTable("SELECT ID_Bono,  Codigo, DescripcionProducto, ClienteNombre, Cantidad, HorasRestantes, cast(Codigo as nvarchar(20)) + ' - ' + DescripcionProducto as Visualizacion FROM C_Bono WHERE (Cerrado=0 and ID_Bono in (Select ID_Bono From Bono_Instalacion Where ID_Instalacion=" & Me.TL_Instalacion.Tag & " Group by ID_Bono))  or id_Bono=" & Util.Comprobar_NULL_Per_0(Me.C_Bono.Value) & "  Order by Codigo Desc")
        End If

        'Dim DT As DataTable = BD.RetornaDataTable("SELECT ID_Instalacion,  Cliente, Poblacion, FechaInstalacion, cast(ID_Instalacion as nvarchar(20)) + ' - ' + Cliente as Visualizacion FROM C_Instalacion WHERE Activo=1 and ID_Instalacion in (Select ID_Instalacion From Entrada_Instalacion Where ID_Entrada=" & oclsEntradaLinea.oLinqEntrada.ID_Entrada & " Group by ID_Instalacion) Order by ID_Instalacion")
        Me.C_Bono.DataSource = DT
        If DT Is Nothing Then
            Exit Sub
        End If

        With C_Bono

            .MaxDropDownItems = 16
            .DisplayMember = "Visualizacion"
            .ValueMember = "ID_Bono"
            .DisplayLayout.Bands(0).Columns("Visualizacion").Hidden = True
            .DisplayLayout.Bands(0).Columns("ID_Bono").Hidden = True
            .DisplayLayout.Bands(0).Columns("Codigo").Width = 50
            .DisplayLayout.Bands(0).Columns("Codigo").Header.Caption = "Código"
            .DisplayLayout.Bands(0).Columns("Codigo").CellAppearance.TextHAlign = HAlign.Right
            .DisplayLayout.Bands(0).Columns("DescripcionProducto").Width = 400
            .DisplayLayout.Bands(0).Columns("DescripcionProducto").Header.Caption = "Descripción"
            .DisplayLayout.Bands(0).Columns("Cantidad").Width = 70
            .DisplayLayout.Bands(0).Columns("Cantidad").Header.Caption = "Cantidad"
            .DisplayLayout.Bands(0).Columns("Cantidad").CellAppearance.TextHAlign = HAlign.Right
            .DisplayLayout.Bands(0).Columns("HorasRestantes").Width = 70
            .DisplayLayout.Bands(0).Columns("HorasRestantes").Header.Caption = "Cantidad"
            .DisplayLayout.Bands(0).Columns("HorasRestantes").CellAppearance.TextHAlign = HAlign.Right
            .DisplayLayout.Bands(0).Columns("ClienteNombre").Width = 200
            .DisplayLayout.Bands(0).Columns("ClienteNombre").Header.Caption = "Cliente"

            '.DisplayLayout.Bands(0).Columns("FechaInstalacion").Width = 100
            '.DisplayLayout.Bands(0).Columns("FechaInstalacion").Header.Caption = "Fecha de instalación"
        End With
        Me.C_Bono.DropDownStyle = UltraComboStyle.DropDownList

        Dim _pRow As UltraGridRow

        For Each _pRow In Me.C_Bono.Rows
            If _pRow.Cells("HorasRestantes").Value <= 0 Then
                _pRow.CellAppearance.BackColor = Color.LightCoral
            End If
        Next

        ' Me.C_Vinculacion.DisplayLayout.Bands(0).Override.AllowRowFiltering = DefaultableBoolean.True

    End Sub

    Private Function Guardar(Optional ByVal pMostrarMissatge As Boolean = True) As Boolean
        Guardar = False
        Try
            Me.TE_Codigo.Focus()
            'If oLinqProducto.ID_Producto = 0 Then
            '    If BD.RetornaValorSQL("SELECT Count (*) From Producto WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
            '        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
            '        Exit Function
            '    End If
            'Else
            '    If BD.RetornaValorSQL("SELECT Count (*) From Producto WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_Producto<>" & oLinqProducto.ID_Producto) > 0 Then
            '        Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
            '        Exit Function
            '    End If
            'End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqParte)

            Call CalcularEstadoParte()

            If oLinqParte.ID_Parte_Tipo = EnumParteTipo.Revision And oLinqParte.ID_Producto_Division = EnumProductoDivision.Intrusion And oLinqParte.ID_Instalacion_Contrato.HasValue = False Then
                Mensaje.Mostrar_Mensaje("Imposible guardar el parte, los partes de revisión con productos de intrusión es obligatorio la intruducción del contrato", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Function
            End If

            If oLinqParte.ID_Parte_Tipo = EnumParteTipo.Reparacion Then
                If IsNothing(oLinqParte.Instalacion) = True Then
                    Mensaje.Mostrar_Mensaje("Imposible guardar, un parte de trabajo de tipo 'Reparación' tiene que tener asignado una instalación.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Function
                End If
            End If

            If oLinqParte.ID_Parte = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                '   oDTC.Parte_Aux.InsertOnSubmit(oLinqParte.Parte_Aux)
                oDTC.Parte.InsertOnSubmit(oLinqParte)

                oDTC.SubmitChanges()
                Me.TE_Codigo.Text = oLinqParte.ID_Parte
                Me.EstableixCaptionForm("Parte: " & (oLinqParte.ID_Parte) & " - " & oLinqParte.Cliente.Nombre)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
                Call Fichero.Cargar_GRID(oLinqParte.ID_Parte) 'Fem això pq la classe tingui el ID de pressupost
                Call ActivarSegonsEstatPantalla(EstatPantalla.DespresDeGuardar)
                'Carguem el grid de revisió per a que es generin les línies de revisio
                If oLinqParte.ID_Parte_Tipo = EnumParteTipo.Revision Then
                    Call CargarGrid_Revision()
                End If
                Call CrearPreguntesQuestionari()
                Call clsParte.CrearMagatzem(oDTC, oLinqParte)

                oDTC.SubmitChanges()


                Dim _ClsNotificacion As New clsNotificacion(oDTC)
                _ClsNotificacion.CrearNotificacion_AlCrearParte(oLinqParte)

            Else
                If oLinqParte.Parte_Cuestionario_Respuestas.Count = 0 Then 'si no hi han preguntes al questionari les crearem
                    Call CrearPreguntesQuestionari()
                End If
                oDTC.SubmitChanges()

                If pMostrarMissatge = True Then
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
            With oLinqParte
                Me.TE_Codigo.Text = .ID_Parte
                If .Cliente Is Nothing = False Then
                    Me.TE_Cliente.Text = .Cliente.Nombre
                    Me.TE_Cliente.Tag = .ID_Cliente
                End If
                If .Instalacion Is Nothing = False Then
                    Me.TL_Instalacion.Text = .ID_Instalacion
                    Me.TL_Instalacion.Tag = .ID_Instalacion
                End If

                If .ID_Parte_Vinculado.HasValue = True Then
                    Me.TE_Parte_Vinculado.Text = .ID_Parte_Vinculado
                End If

                If .ID_Producto_Division.HasValue = True Then
                    Me.C_Division.Text = .ID_Producto_Division
                End If

                If .ID_Personal.HasValue = True Then
                    Me.C_ResponsableInforme.Value = .ID_Personal
                End If

                If .ID_PersonalExplicacionHorasTecnico.HasValue = True Then
                    Me.C_ResponsableExplicacion.Text = .ID_PersonalExplicacionHorasTecnico
                End If

                Me.T_CosteImputadoMO.Value = .CosteImputadoMO
                Me.T_CosteMaterial.Value = .CosteMaterial
                Me.T_CosteGastos.Value = .CosteGastos
                Me.T_CostePrevisto.Value = .CostePrevisto
                Me.T_CostePrevistoGastos.Value = .CostePrevistoGastos
                Me.T_CostePrevistoMaterial.Value = .CostePrevistoMaterial
                Me.T_CostePrevistoTotal.Value = .CostePrevisto + .CostePrevistoMaterial + .CostePrevistoGastos
                Me.T_CosteTotal.Value = .CosteMaterial + .CosteImputadoMO + .CosteGastos
                Me.T_Direccion.Text = .Direccion
                Me.T_QuienDetectoIncidencia.Text = .QuienDetectoIncidencia
                Me.R_Observaciones_Questionario.pText = .CuestionarioObservaciones

                Me.T_HorasPrevistas.Value = .HorasPrevistas
                Me.T_HorasRealizadas.Value = .HorasRealizadas
                Me.T_HorasTecnico.Value = .HorasTecnico
                Me.T_MargenGastos.Value = .MargenGastos
                Me.T_MargenMaterial.Value = .MargenMaterial
                Me.T_MargenMO.Value = .MargenMO
                Me.T_MargenTotal.Value = .MargenMO + .MargenMaterial + .MargenGastos
                Me.R_ObservacionesReparacion.pText = .Parte_Aux.ObservacionesTecnico
                Me.T_Persona_Contacto.Text = .PersonaContacto
                Me.T_PersonaQueLoFirmo.Text = .PersonaQueLoFirmo
                Me.T_Poblacion.Text = .Poblacion
                Me.T_Provincia.Text = .Provincia
                Me.T_Telefono.Text = .Telefono
                If .TrabajoARealizarRTF Is Nothing = False Then
                    Me.R_TrabajosARealizar.pText = .TrabajoARealizarRTF
                Else
                    Me.R_TrabajosARealizar.pText = .TrabajoARealizar
                End If

                Me.R_TrabajosRealizados.pText = .Parte_Aux.TrabajosRealizados
                Me.R_ExplicacionHorasTecnico.pText = .Parte_Aux.ExplicacionHorasTecnico
                Me.T_Factura.Text = .Factura

                Me.DT_Alta.Value = .FechaAlta
                Me.DT_Fin.Value = .FechaFin
                Me.DT_HoraInicio.Value = .HoraInicio
                Me.DT_Inicio.Value = .FechaInicio
                Me.DT_LimiteFinalizacion.Value = .FechaLimiteFinalizacion

                Me.T_Longitud.Value = .Longitud
                Me.T_Latitud.Value = .Latitud
                Me.T_CP.Value = .CP
                Me.C_Pais.Value = .ID_Pais

                If .ID_Delegacion.HasValue = False Then
                    Me.C_Delegacion.SelectedIndex = -1
                Else
                    Me.C_Delegacion.Value = .ID_Delegacion
                End If


                Me.C_Estado.Value = .ID_Parte_Estado

                Call C_Presupuesto_BeforeDropDown(Nothing, Nothing)

                Me.C_Presupuesto.Value = .ID_Propuesta
                Me.C_Tipo.Value = .ID_Parte_Tipo
                Me.C_TipoFacturacion.Value = .ID_Parte_TipoFacturacion

                Me.CH_Firmado.Checked = .ParteFirmado
                Me.CH_Punteo.Checked = .Punteo
                Me.CH_BloqueoImputacionHoras.Checked = .BloquearImputacionHoras
                Me.CH_BloquearImputacionMaterial.Checked = .BloquearImputacionMaterial

                Me.L_PersonaContacto.Text = .PersonaContacto
                Me.L_QuienDetectoIncidencia.Text = .QuienDetectoIncidencia
                Me.L_Telefono.Text = .Telefono
                Me.CH_Informe_Finalizado.Checked = .NoPermitirModificarInformeTecnico


                Call CarregarEmplazamientos(oLinqParte.ID_Instalacion)
                Call CarregarContratos(oLinqParte.ID_Instalacion)


                'El codi de abaix es per omplir el listview. 
                Dim _Item As Infragistics.Win.UltraWinListView.UltraListViewItem
                For Each _Item In Me.List_Emplazamientos.Items
                    'If _Item.CheckState = CheckState.Checked Then
                    If oLinqParte.Parte_Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = CInt(_Item.Key)).Count > 0 Then
                        _Item.CheckState = CheckState.Checked
                    End If
                    ' End If
                Next

                'Carreguem el contracte seleccionat
                If .ID_Instalacion_Contrato <> 0 Then
                    Dim pRow As UltraGridRow
                    For Each pRow In Me.C_Contrato.Rows
                        If pRow.Cells("ID_Instalacion_Contrato").Value = .ID_Instalacion_Contrato Then
                            Me.C_Contrato.SelectedRow = pRow
                            Exit For
                        End If
                    Next
                    'Me.C_Vinculacion.Rows(40).Selected = True
                    'Me.C_Vinculacion.Value = .Descripcion
                End If

                If .Hoja Is Nothing = False Then
                    Me.Excel1.M_LoadDocument(.Hoja)
                End If

                Me.C_Bono.Value = .ID_Bono
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pParte As Parte)
        Try
            With pParte
                .Activo = True


                If Me.TE_Cliente.Text.Length = 0 Then
                    .Cliente = Nothing
                Else
                    .Cliente = oDTC.Cliente.Where(Function(F) F.ID_Cliente = CInt(Me.TE_Cliente.Tag)).FirstOrDefault
                End If

                If Me.TL_Instalacion.Text.Length = 0 Then
                    .Instalacion = Nothing
                Else
                    .Instalacion = oDTC.Instalacion.Where(Function(F) F.ID_Instalacion = CInt(Me.TL_Instalacion.Tag)).FirstOrDefault
                End If

                If Me.TE_Parte_Vinculado.Text.Length = 0 Then
                    .Parte = Nothing
                Else
                    .Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = CInt(Me.TE_Parte_Vinculado.Text)).FirstOrDefault
                End If

                If Me.C_Division.SelectedIndex <> -1 Then
                    .Producto_Division = oDTC.Producto_Division.Where(Function(F) F.ID_Producto_Division = CInt(Me.C_Division.Value)).FirstOrDefault
                Else
                    .Producto_Division = Nothing
                End If

                If Me.C_ResponsableInforme.SelectedIndex <> -1 Then
                    .Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_ResponsableInforme.Value)).FirstOrDefault
                Else
                    .Personal = Nothing
                End If

                If Me.C_ResponsableExplicacion.SelectedIndex <> -1 Then
                    .Personal1 = oDTC.Personal.Where(Function(F) F.ID_Personal = CInt(Me.C_ResponsableExplicacion.Value)).FirstOrDefault
                Else
                    .Personal1 = Nothing
                End If

                If oLinqParte.ID_Parte = 0 Then
                    Dim _aux As New Parte_Aux

                    _aux.ObservacionesTecnico = Me.R_ObservacionesReparacion.pText
                    _aux.TrabajosRealizados = Me.R_TrabajosRealizados.pText
                    _aux.ExplicacionHorasTecnico = Me.R_ExplicacionHorasTecnico.pText
                    .Parte_Aux = _aux
                Else
                    .Parte_Aux.ObservacionesTecnico = Me.R_ObservacionesReparacion.pText
                    .Parte_Aux.TrabajosRealizados = Me.R_TrabajosRealizados.pText
                    .Parte_Aux.ExplicacionHorasTecnico = Me.R_ExplicacionHorasTecnico.pText
                End If


                .CosteImputadoMO = DbnullToNothing(Me.T_CosteImputadoMO.Value)
                .CosteMaterial = DbnullToNothing(Me.T_CosteMaterial.Value)
                .CosteGastos = DbnullToNothing(Me.T_CosteGastos.Value)
                .CostePrevisto = DbnullToNothing(Me.T_CostePrevisto.Value)
                .CostePrevistoMaterial = DbnullToNothing(Me.T_CostePrevistoMaterial.Value)
                .CostePrevistoGastos = DbnullToNothing(Me.T_CostePrevistoGastos.Value)
                .Direccion = Me.T_Direccion.Text
                .QuienDetectoIncidencia = Me.T_QuienDetectoIncidencia.Text
                .Factura = Me.T_Factura.Text
                .HorasPrevistas = DbnullToNothing(Me.T_HorasPrevistas.Value)
                .HorasRealizadas = DbnullToNothing(Me.T_HorasRealizadas.Value)
                .HorasTecnico = DbnullToNothing(Me.T_HorasTecnico.Value)
                .MargenMaterial = DbnullToNothing(Me.T_MargenMaterial.Value)
                .MargenMO = DbnullToNothing(Me.T_MargenMO.Value)
                .MargenGastos = DbnullToNothing(Me.T_MargenGastos.Value)

                .PersonaContacto = Me.T_Persona_Contacto.Text
                .PersonaQueLoFirmo = Me.T_PersonaQueLoFirmo.Text
                .Poblacion = Me.T_Poblacion.Text
                .Provincia = Me.T_Provincia.Text
                .Telefono = Me.T_Telefono.Text
                .TrabajoARealizar = Me.R_TrabajosARealizar.pTextEspecial
                .TrabajoARealizarRTF = Me.R_TrabajosARealizar.pText
                '  .Parte_Aux.TrabajosRealizados = Me.R_TrabajosRealizados.pText
                '  .Parte_Aux.ExplicacionHorasTecnico = Me.R_ExplicacionHorasTecnico.pText

                .FechaAlta = Me.DT_Alta.Value
                .FechaFin = Me.DT_Fin.Value
                .HoraInicio = Me.DT_HoraInicio.Value
                .FechaInicio = Me.DT_Inicio.Value
                .FechaLimiteFinalizacion = Me.DT_LimiteFinalizacion.Value

                .ID_Parte_Estado = Me.C_Estado.Value
                .ID_Propuesta = Me.C_Presupuesto.Value
                .ID_Parte_Tipo = Me.C_Tipo.Value
                .ID_Parte_TipoFacturacion = Me.C_TipoFacturacion.Value
                .ID_Instalacion_Contrato = Me.C_Contrato.Value

                .ParteFirmado = Me.CH_Firmado.Checked
                .Punteo = Me.CH_Punteo.Checked
                .BloquearImputacionHoras = Me.CH_BloqueoImputacionHoras.Checked
                .BloquearImputacionMaterial = Me.CH_BloquearImputacionMaterial.Checked
                .NoPermitirModificarInformeTecnico = Me.CH_Informe_Finalizado.Checked

                .CuestionarioObservaciones = Me.R_Observaciones_Questionario.pText
                .CuestionarioPuntuacion = oLinqParte.Parte_Cuestionario_Respuestas.Sum(Function(F) F.Respuesta)

                .Longitud = Util.DbnullToNothing(Me.T_Longitud.Value)
                .Latitud = Util.DbnullToNothing(Me.T_Latitud.Value)

                .CP = Me.T_CP.Value
                .Pais = oDTC.Pais.Where(Function(F) F.ID_Pais = CInt(Me.C_Pais.Value)).FirstOrDefault

                If Me.C_Delegacion.Value = 0 Then
                    .Delegacion = Nothing
                Else
                    .Delegacion = oDTC.Delegacion.Where(Function(F) F.ID_Delegacion = CInt(Me.C_Delegacion.Value)).FirstOrDefault
                End If

                'El codi de abaix es per omplir el listview. 
                Dim _Item As Infragistics.Win.UltraWinListView.UltraListViewItem
                For Each _Item In Me.List_Emplazamientos.Items
                    If _Item.CheckState = CheckState.Checked Then
                        If oLinqParte.ID_Parte = 0 Then
                            Dim _Emp As New Parte_Instalacion_Emplazamiento
                            _Emp.Instalacion_Emplazamiento = oDTC.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = CInt(_Item.Key)).FirstOrDefault
                            oLinqParte.Parte_Instalacion_Emplazamiento.Add(_Emp)
                        Else
                            If oLinqParte.Parte_Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = _Item.Key).Count = 0 Then
                                Dim _Emp As New Parte_Instalacion_Emplazamiento
                                _Emp.Instalacion_Emplazamiento = oDTC.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = CInt(_Item.Key)).FirstOrDefault
                                _Emp.Parte = oLinqParte
                                oLinqParte.Parte_Instalacion_Emplazamiento.Add(_Emp)
                            End If
                        End If
                    Else
                        'Si el registre ja s'havia guardat i trobem que el registre estava a la taula l'eliminarem pq ara ja no està seleccionat
                        If oLinqParte.ID_Parte <> 0 Then
                            Dim _Emp As New Parte_Instalacion_Emplazamiento
                            _Emp = oLinqParte.Parte_Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = CInt(_Item.Key)).FirstOrDefault
                            If IsNothing(_Emp) = False Then
                                oLinqParte.Parte_Instalacion_Emplazamiento.Remove(_Emp)
                                oDTC.Parte_Instalacion_Emplazamiento.DeleteOnSubmit(_Emp)
                            End If
                        End If
                    End If
                Next

                .Hoja = Me.Excel1.M_SaveDocument.ToArray


                If Me.C_Bono.SelectedRow Is Nothing Then
                    .Bono = Nothing
                Else
                    .Bono = oDTC.Bono.Where(Function(F) F.ID_Bono = CInt(Me.C_Bono.Value)).FirstOrDefault
                End If


            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer, Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        Try
            Call Netejar_Pantalla(pNoCanviarALaPestanyaGeneral)
            oLinqParte = (From taula In oDTC.Parte Where taula.ID_Parte = pID Select taula).First
            Util.Cargar_Combo(Me.C_ResponsableInforme, "SELECT ID_Personal, Nombre FROM Personal Where ID_Personal in (Select ID_Personal From Parte_Asignacion Where ID_Parte=" & oLinqParte.ID_Parte & ") ORDER BY Nombre", False)
            Call CargarCombo_Bonos(True)

            Call SetToForm()

            Call CalcularEstadoParte()
            'Call CargaGrid_Asignacion(pID)
            'Call CargaGrid_Horas(pID)
            'Call CargaGrid_Gastos(pID)
            'Call CargaGrid_ATenerEnCuenta(pID)
            'Call CargaGrid_Incidencia(pID)
            'Call CargaGrid_Material(pID)
            Fichero.Cargar_GRID(pID)
            'Fem això pq un producte guardat no es pot canviar la divisió


            Me.C_Tipo.ReadOnly = True
            Call ActivarSegonsEstatPantalla(EstatPantalla.DespresDeCargar)

            'Me.C_Familia.ReadOnly = True

            If oLinqParte.Cliente Is Nothing Then
                Me.EstableixCaptionForm("Parte: " & (oLinqParte.ID_Parte))
            Else
                Me.EstableixCaptionForm("Parte: " & (oLinqParte.ID_Parte) & " - " & oLinqParte.Cliente.Nombre)
            End If

            Call CalculsPantalla()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CalculsPantalla()
        Call CalcularHoresRealitzades()
        Call CalcularCosteHoresRealitzades()
        Call CalcularGastos()
    End Sub

    Private Sub Netejar_Pantalla(Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        oLinqParte = New Parte
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Me.TE_Codigo.Value = Nothing

        'Me.DT_Alta.Value = Nothing
        'Me.DT_Inicio.Value = Nothing
        Me.DT_Alta.Value = Now.Date

        Me.C_Estado.Value = 1
        Me.C_TipoFacturacion.Value = 1
        Me.C_Tipo.Value = 1
        GridTrabajosARealizarIndentat = False
        Call ActivarSegonsEstatPantalla(EstatPantalla.Nuevo)
        Me.C_Presupuesto.Clear()

        Me.C_Tipo.ReadOnly = False
        Me.C_Contrato.Value = Nothing

        ErrorProvider1.Clear()
        'Call CargaGrid_Asignacion(0)
        'Call CargaGrid_Horas(0)
        'Call CargaGrid_Gastos(0)
        'Call CargaGrid_ATenerEnCuenta(0)
        'Call CargaGrid_Incidencia(0)
        'Call CargaGrid_Material(0)
        'Call CargaGrid_MaterialPresupuestado(True)

        Me.GRD_AsignacionPersonal.GRID.DataSource = Nothing
        Me.GRD_ATenerEnCuenta.GRID.DataSource = Nothing
        Me.GRD_Gastos.GRID.DataSource = Nothing
        Me.GRD_Horas.GRID.DataSource = Nothing
        Me.GRD_Incidencia.GRID.DataSource = Nothing
        Me.GRD_Material.GRID.DataSource = Nothing
        Me.GRD_MaterialPresupuestado.GRID.DataSource = Nothing
        Me.GRD_Reparacion.GRID.DataSource = Nothing
        Me.GRD_Reparacion_TalComoSeInstalo.GRID.DataSource = Nothing
        Me.GRD_Revision.GRID.DataSource = Nothing
        Me.GRD_TrabajosARealizar.GRID.DataSource = Nothing

        Me.C_ResponsableInforme.Items.Clear()
        Me.C_ResponsableInforme.Value = Nothing

        Me.C_ResponsableExplicacion.Items.Clear()
        Me.C_ResponsableExplicacion.Value = Nothing

        Me.GRD_Step_Albaranes.GRID.DataSource = Nothing

        Me.C_Bono.Value = Nothing

        Fichero.Cargar_GRID(0)

        Me.EstableixCaptionForm("Parte")
        If pNoCanviarALaPestanyaGeneral = False Then
            Me.TAB_Principal.Tabs("General").Selected = True
        End If
        Me.L_Emplazamientos.Visible = False
        Me.List_Emplazamientos.Visible = False
        Me.List_Emplazamientos.Items.Clear()

        Me.L_PersonaContacto.Text = ""
        Me.L_QuienDetectoIncidencia.Text = ""
        Me.L_Telefono.Text = ""


        Me.ToolForm.ToolForm.Tools("CambiarAEstadoEnCurso").SharedProps.Enabled = False
        'Me.B_CambiarEstadoAEnCurso.Visible = False

        Me.C_Pais.Value = oDTC.Pais.Where(Function(F) F.Predeterminat = True).FirstOrDefault.ID_Pais
        Me.C_Delegacion.SelectedIndex = -1

        Me.Excel1.M_NewDocument()

    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                ' oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.C_TipoFacturacion)
                oClsControls.ControlBuit(.C_Tipo)
                oClsControls.ControlBuit(.C_Estado)
                oClsControls.ControlBuit(.TE_Cliente)
                oClsControls.ControlBuit(.TL_Instalacion)
                oClsControls.ControlBuit(.R_TrabajosARealizar)
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

    Private Sub Cridar_Llistat_Generic()
        Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Parte Where Activo=1 ORDER BY ID_Parte", Me.TE_Codigo, "ID_Parte", "ID_Parte")
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

    Private Sub AlTancarLlistatInstalacions(ByVal pID As String)
        Try
            If pID Is Nothing = True OrElse pID = 0 Then
                Exit Sub
            End If

            Me.C_Presupuesto.SelectedIndex = -1
            'If Mensaje.Mostrar_Mensaje("¿Desea importar las líneas de 'A tener en cuenta' de la instalación seleccionada?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
            '    Exit Sub
            'End If

            'Agafarem les línies de AtenerenCuenta de la instalació i las copiarem al parte
            For Each Instalacion_ATenerEnCuenta In oDTC.Instalacion_ATenerEnCuenta.Where(Function(F) F.ID_Instalacion = CInt(pID))
                Dim _ATenerEnCuenta As New Parte_ATenerEnCuenta
                If Me.C_Tipo.Value = EnumParteTipo.Instalacion And Instalacion_ATenerEnCuenta.Instalacion = True Then
                    _ATenerEnCuenta.Descripcion = Instalacion_ATenerEnCuenta.Descripcion
                    oLinqParte.Parte_ATenerEnCuenta.Add(_ATenerEnCuenta)
                End If

                If Me.C_Tipo.Value = EnumParteTipo.Reparacion And Instalacion_ATenerEnCuenta.Reparacion = True Then
                    _ATenerEnCuenta.Descripcion = Instalacion_ATenerEnCuenta.Descripcion
                    oLinqParte.Parte_ATenerEnCuenta.Add(_ATenerEnCuenta)
                End If

                If Me.C_Tipo.Value = EnumParteTipo.Revision And Instalacion_ATenerEnCuenta.Revision = True Then
                    _ATenerEnCuenta.Descripcion = Instalacion_ATenerEnCuenta.Descripcion
                    oLinqParte.Parte_ATenerEnCuenta.Add(_ATenerEnCuenta)
                End If

                If _ATenerEnCuenta.ID_Parte <> 0 Then

                End If
                oDTC.SubmitChanges()
            Next

            Dim _Instalacion As Instalacion = oDTC.Instalacion.Where(Function(F) F.ID_Instalacion = CInt(pID)).FirstOrDefault
            Me.TE_Cliente.Tag = _Instalacion.Cliente.ID_Cliente
            Me.TE_Cliente.Text = _Instalacion.Cliente.Nombre
            Me.T_Direccion.Text = _Instalacion.Direccion
            Me.T_Persona_Contacto.Text = _Instalacion.PersonaContacto
            Me.T_Poblacion.Text = _Instalacion.Poblacion
            Me.T_Provincia.Text = _Instalacion.Provincia
            Me.T_Telefono.Text = _Instalacion.Telefono
            Me.T_CP.Text = _Instalacion.CP
            Me.T_Latitud.Value = _Instalacion.Latitud
            Me.T_Longitud.Value = _Instalacion.Longitud
            Me.C_Pais.Value = _Instalacion.ID_Pais
            If _Instalacion.ID_Delegacion.HasValue = True Then
                Me.C_Delegacion.Value = _Instalacion.ID_Delegacion
            Else
                Me.C_Delegacion.SelectedIndex = -1
            End If


            Call CarregarEmplazamientos(pID)
            Call CarregarContratos(pID)

            'Marquem tots els items a checked. 
            Dim _Item As Infragistics.Win.UltraWinListView.UltraListViewItem
            For Each _Item In Me.List_Emplazamientos.Items
                _Item.CheckState = CheckState.Checked
            Next
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub ActivarTabsSegonsTipusDeParte()
        Try

            If Me.C_Tipo.SelectedIndex <> -1 Then
                Me.C_Division.Visible = False
                Me.List_Emplazamientos.Visible = False
                Me.L_Emplazamientos.Visible = False

                Util.Tab_InVisible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Reparacion", "Revision", "MantenimientosPlanificados")
                Select Case Me.C_Tipo.Value
                    Case CInt(EnumParteTipo.Instalacion)
                        ' Util.Tab_Visible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Instalacion")
                    Case CInt(EnumParteTipo.Reparacion)
                        Util.Tab_Visible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Reparacion")
                        Me.List_Emplazamientos.Visible = True
                    Case CInt(EnumParteTipo.Revision)
                        Util.Tab_Visible_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Revision", "MantenimientosPlanificados")
                        Me.C_Division.Visible = True
                        If oLinqParte.ID_Parte = 0 Then  'si seleccionem el parte de tipus revisió i encara no hem guardat el parte predeterminarem la divisió Intrusión
                            Me.C_Division.Value = CInt(EnumProductoDivision.Intrusion)
                        End If
                        Me.List_Emplazamientos.Visible = True
                        Me.L_Emplazamientos.Visible = True

                End Select
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CalcularHoresRealitzades()
        Try
            'Me.T_HorasRealizadas.Value = oLinqParte.Parte_Horas.Sum(Function(F) F.Horas + F.HorasExtras)
            Me.T_HorasRealizadas.Value = oLinqParte.RetornaHoresRealizadas
            Call CallActualitzarEnLaBDTotsElsTotals()
            'Ho guardem a la BD pq sempre estigui actualitzat

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CalcularCosteHoresRealitzades()
        Try
            'If oLinqParte.ID_Parte = 0 Then
            '    Me.T_CosteImputadoMO.Value = 0
            'Else
            '    Me.T_CosteImputadoMO.Value = oDTC.Parte.Where(Function(F) F.ID_Parte = oLinqParte.ID_Parte).FirstOrDefault.Parte_Horas.Sum(Function(F) F.ID_Parte_Horas <> 0 And F.Horas * F.Parte.Parte_Asignacion.Where(Function(A) A.ID_Personal = F.ID_Personal).FirstOrDefault.PrecioCoste + F.HorasExtras * F.Parte.Parte_Asignacion.Where(Function(A) A.ID_Personal = F.ID_Personal).FirstOrDefault.PrecioCosteHoraExtra)
            'End If

            oLinqParte.CostePrevisto = Util.Comprobar_NULL_Per_0_Decimal(Me.T_CostePrevisto.Value)

            'xapuça pq no peti quan es crea una linea d'hores i es canvia de pestanya. El grid elimina la linea però no elimina la instancia Parte_horas dins de olinqParte i llavors peta
            Dim _Horas As Parte_Horas
            For Each _Horas In oLinqParte.Parte_Horas
                If _Horas.ID_Personal = 0 Then
                    oLinqParte.Parte_Horas.Remove(_Horas)
                    Exit For
                End If
            Next
            oDTC.SubmitChanges()

            Me.T_CosteImputadoMO.Value = oLinqParte.RetornaCosteImputadoMO   'oLinqParte.Parte_Horas.Sum(Function(F) F.ID_Parte_Horas <> 0 And F.Horas * F.Parte.Parte_Asignacion.Where(Function(A) A.ID_Personal = F.ID_Personal).FirstOrDefault.PrecioCoste + F.HorasExtras * F.Parte.Parte_Asignacion.Where(Function(A) A.ID_Personal = F.ID_Personal).FirstOrDefault.PrecioCosteHoraExtra)
            Me.T_MargenMO.Value = oLinqParte.RetornaMargenMO ' IIf(Me.T_CostePrevisto.Value Is DBNull.Value, 0, Me.T_CostePrevisto.Value) - IIf(Me.T_CosteImputadoMO.Value Is DBNull.Value, 0, Me.T_CosteImputadoMO.Value)
            Call CalcularTotales()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CalcularGastos()
        Try
            oLinqParte.CostePrevistoGastos = Util.Comprobar_NULL_Per_0_Decimal(Me.T_CostePrevistoGastos.Value)
            Me.T_CosteGastos.Value = oLinqParte.RetornaGastos 'oLinqParte.Parte_Gastos.Sum(Function(F) F.Gasto)
            Me.T_MargenGastos.Value = oLinqParte.RetornaMargenGastos '  IIf(Me.T_CostePrevistoGastos.Value Is DBNull.Value, 0, Me.T_CostePrevistoGastos.Value) - IIf(Me.T_CosteGastos.Value Is DBNull.Value, 0, Me.T_CosteGastos.Value)
            Call CalcularTotales()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CalcularTotales()
        Try
            Me.T_CostePrevistoTotal.Value = oLinqParte.RetornaCostePrevistoTotal   'Util.Comprobar_NULL_Per_0(Me.T_CostePrevistoMaterial.Value) + Util.Comprobar_NULL_Per_0(Me.T_CostePrevistoGastos.Value) + Util.Comprobar_NULL_Per_0(Me.T_CostePrevisto.Value)
            Me.T_CosteTotal.Value = oLinqParte.RetornaCosteTotal 'Util.Comprobar_NULL_Per_0(Me.T_CosteImputadoMO.Value) + Util.Comprobar_NULL_Per_0(Me.T_CosteMaterial.Value) + Util.Comprobar_NULL_Per_0(Me.T_CosteGastos.Value)
            Me.T_MargenTotal.Value = oLinqParte.RetornaMargenTotal  'Util.Comprobar_NULL_Per_0(Me.T_MargenMO.Value) + Util.Comprobar_NULL_Per_0(Me.T_MargenMaterial.Value) + Util.Comprobar_NULL_Per_0(Me.T_MargenGastos.Value)
            Call CallActualitzarEnLaBDTotsElsTotals()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CalcularEstadoParte()
        Try
            If oLinqParte.ID_Parte_Estado = EnumParteEstado.Finalizado Then
                Util.DesActivar(Me, M_Util.Enum_Controles_Activacion.TODOS_MENOS_ALGUNOS, True, Me.T_Factura, Me.DT_Fin, Me.T_PersonaQueLoFirmo, Me.GRD_Ficheros, Me.GRD_Horas, Me.OP_Filtre)
                Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
                'Me.GRD_Incidencia.ToolGrid.Tools("GenerarParte").SharedProps.Enabled = False
                ' Me.GRD_Revision.ToolGrid.Tools("GenerarParte").SharedProps.Enabled = False
                ' Me.GRD_Reparacion_TalComoSeInstalo.ToolGrid.Tools("MarcarArticulo").SharedProps.Enabled = False
                'Me.GRD_Reparacion.ToolGrid.Tools("GenerarReparacion").SharedProps.Enabled = False
                Me.ToolForm.ToolForm.Tools("CambiarAEstadoEnCurso").SharedProps.Enabled = True
                'Me.B_CambiarEstadoAEnCurso.Visible = True
                Exit Sub
            Else
                Util.Activar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
                ' Me.GRD_Incidencia.ToolGrid.Tools("GenerarParte").SharedProps.Enabled = True
                ' Me.GRD_Revision.ToolGrid.Tools("GenerarParte").SharedProps.Enabled = True
                ' Me.GRD_Reparacion_TalComoSeInstalo.ToolGrid.Tools("MarcarArticulo").SharedProps.Enabled = True
                Me.ToolForm.ToolForm.Tools("CambiarAEstadoEnCurso").SharedProps.Enabled = False
                'Me.B_CambiarEstadoAEnCurso.Visible = False
                ' Me.GRD_Reparacion.ToolGrid.Tools("GenerarReparacion").SharedProps.Enabled = True
            End If

            If oLinqParte.ID_Parte <> 0 Then
                If oLinqParte.Parte_Horas.Count > 0 Or oLinqParte.Parte_Gastos.Count > 0 Or oLinqParte.Parte_Incidencia.Count > 0 Then
                    Me.C_Estado.Value = CInt(EnumParteEstado.EnCurso)
                    oLinqParte.Parte_Estado = oDTC.Parte_Estado.Where(Function(F) F.ID_Parte_Estado = CInt(EnumParteEstado.EnCurso)).FirstOrDefault
                Else
                    Me.C_Estado.Value = CInt(EnumParteEstado.Pendiente)
                    oLinqParte.Parte_Estado = oDTC.Parte_Estado.Where(Function(F) F.ID_Parte_Estado = CInt(EnumParteEstado.Pendiente)).FirstOrDefault
                End If
                oDTC.SubmitChanges()
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CallActualitzarEnLaBDTotsElsTotals()
        If oLinqParte.ID_Parte <> 0 Then
            oLinqParte.HorasRealizadas = oLinqParte.RetornaHoresRealizadas
            oLinqParte.CosteImputadoMO = oLinqParte.RetornaCosteImputadoMO
            oLinqParte.MargenMO = oLinqParte.RetornaMargenMO
            oLinqParte.CosteGastos = oLinqParte.RetornaGastos
            oLinqParte.MargenGastos = oLinqParte.RetornaMargenGastos
            oDTC.SubmitChanges()
        End If
    End Sub

    Private Sub ActivarSegonsEstatPantalla(ByRef Estat As EstatPantalla)

        Select Case Estat
            Case EstatPantalla.Nuevo
                Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General")
                Me.TL_Instalacion.ButtonsRight("Lupeta").Enabled = True
                Me.TE_Parte_Vinculado.ButtonsRight("Lupeta").Enabled = True
                Me.TE_Cliente.ButtonsRight("Lupeta").Enabled = True
                Me.C_Division.Enabled = True
                Util.Activar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)
                Me.List_Emplazamientos.Enabled = True
            Case EstatPantalla.DespresDeCargar
                Me.TL_Instalacion.ButtonsRight("Lupeta").Enabled = False
                Me.TE_Parte_Vinculado.ButtonsRight("Lupeta").Enabled = False
                Me.TE_Cliente.ButtonsRight("Lupeta").Enabled = False
                Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)
                Me.C_Division.Enabled = False
                Call ActivarTabsSegonsTipusDeParte()
                Me.List_Emplazamientos.Enabled = False
            Case EstatPantalla.DespresDeGuardar
                Me.TL_Instalacion.ButtonsRight("Lupeta").Enabled = False
                Me.TE_Parte_Vinculado.ButtonsRight("Lupeta").Enabled = False
                Me.TE_Cliente.ButtonsRight("Lupeta").Enabled = False
                Me.C_Division.Enabled = False
                Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)
                Me.C_Tipo.ReadOnly = True
                Me.List_Emplazamientos.Enabled = False
        End Select

    End Sub

    Private Sub CarregarEmplazamientos(ByVal pID As Integer)
        Try
            Dim _Emp As Instalacion_Emplazamiento
            Me.List_Emplazamientos.Items.Clear()

            For Each _Emp In oDTC.Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion = CInt(pID))
                Me.List_Emplazamientos.Items.Add(_Emp.ID_Instalacion_Emplazamiento, _Emp.Descripcion)
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CarregarContratos(ByVal pID As Integer)
        Try
            Dim DT As DataTable = BD.RetornaDataTable("SELECT  ID_Instalacion_Contrato, NumeroContrato, Instalacion_Contrato.Descripcion, Instalacion_Contrato_TipoContrato.Descripcion as TipoContrato, Producto_Division.Descripcion as Division, FechaInicio, FechaFin  FROM Instalacion_Contrato, Instalacion_Contrato_TipoContrato , Producto_Division WHERE Instalacion_Contrato_TipoContrato.ID_Instalacion_Contrato_TipoContrato=Instalacion_Contrato.ID_Instalacion_Contrato_TipoContrato and Producto_Division.ID_Producto_Division=Instalacion_Contrato.ID_Producto_Division and Activo=1 and  ID_Instalacion=" & pID & " ORDER BY FechaInicio Desc")
            With C_Contrato

                .DataSource = DT
                If DT Is Nothing Then
                    Exit Sub
                End If
                .MaxDropDownItems = 16
                .DisplayMember = "Descripcion"
                .ValueMember = "ID_Instalacion_Contrato"
                .DisplayLayout.Bands(0).Columns("ID_Instalacion_Contrato").Hidden = True
                .DisplayLayout.Bands(0).Columns("NumeroContrato").Width = 100
                .DisplayLayout.Bands(0).Columns("NumeroContrato").Header.Caption = "NºContrato"
                .DisplayLayout.Bands(0).Columns("Descripcion").Width = 250
                .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Descripción"
                .DisplayLayout.Bands(0).Columns("TipoContrato").Width = 120
                .DisplayLayout.Bands(0).Columns("TipoContrato").Header.Caption = "Tipo de Contrato"
                .DisplayLayout.Bands(0).Columns("Division").Width = 100
                .DisplayLayout.Bands(0).Columns("Division").Header.Caption = "División"
                .DisplayLayout.Bands(0).Columns("FechaInicio").Width = 70
                .DisplayLayout.Bands(0).Columns("FechaInicio").Header.Caption = "Fecha Inicio"
                .DisplayLayout.Bands(0).Columns("FechaFin").Width = 70
                .DisplayLayout.Bands(0).Columns("FechaFin").Header.Caption = "Fecha Fin"
                'Me.C_Vinculacion.DropDownStyle = UltraComboStyle.DropDownList
                .DisplayLayout.Bands(0).Override.AllowRowFiltering = DefaultableBoolean.True

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CrearPreguntesQuestionari()
        Dim _Pregunta As Parte_Cuestionario_Preguntas
        For Each _Pregunta In oDTC.Parte_Cuestionario_Preguntas.Where(Function(F) F.ID_Parte_Tipo = oLinqParte.ID_Parte_Tipo)
            Dim _NewResposta As New Parte_Cuestionario_Respuestas
            _NewResposta.Parte = oLinqParte
            _NewResposta.Parte_Cuestionario_Preguntas = _Pregunta
            _NewResposta.Respuesta = 0
            oLinqParte.Parte_Cuestionario_Respuestas.Add(_NewResposta)
        Next
        oDTC.SubmitChanges()
    End Sub

    Private Sub PosarEditableColumnes(ByVal pIDBanda As Integer, ByVal pNomColumna As String)
        Try
            Me.GRD_TrabajosARealizar.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellActivation = Activation.AllowEdit
            Me.GRD_TrabajosARealizar.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellClickAction = CellClickAction.EditAndSelectText
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CarregarDadesPestanyes(ByVal pKeyPestanya As String)
        Try
            Me.GRD_Horas.GRID.ActiveRow = Nothing
            Me.GRD_Horas.GRID.Update()

            Dim ID As Integer = oLinqParte.ID_Parte

            Select Case pKeyPestanya
                Case "General"
                    Call CalcularEstadoParte()
                    Call CalculsPantalla()
                Case "AsignacionPersonal"
                    Call CargaGrid_Asignacion(ID)
                    Call CargaGrid_Calendario(ID)
                Case "Horas"
                    Call CargaGrid_Horas(ID)
                Case "Gastos"
                    Call CargaGrid_Gastos(ID)
                    Call CalculsPantalla()
                Case "Incidencias"
                    Call CargaGrid_Incidencia(ID)
                Case "MaterialNoPresupuestado"
                    Call CargaGrid_Material(ID)
                Case "ATenerEnCuenta"
                    Call CargaGrid_ATenerEnCuenta(ID)
                Case "Reparacion"
                    If Me.TL_Instalacion.Text.Length <> 0 Then
                        Call CargaGrid_Reparacion_TalComoSeInstalo()
                        Call CargaGrid_Reparacion(ID)
                    End If
                Case "Revision"
                    Call CargarGrid_Revision()
                Case "MaterialPresupuestado"
                    Call CargaGrid_MaterialPresupuestado()
                Case "Calidad"
                    Call CargaGrid_Cuestionario(ID)
                Case "InformeTecnico"
                    If oLinqParte.Parte_Asignacion.Count > 0 Then
                        Util.Cargar_Combo(Me.C_ResponsableInforme, "SELECT ID_Personal, Nombre FROM Personal Where ID_Personal in (Select ID_Personal From Parte_Asignacion Where ID_Parte=" & oLinqParte.ID_Parte & ") ORDER BY Nombre", False)
                        If oLinqParte.ID_Personal.HasValue = True Then
                            Me.C_ResponsableInforme.Value = oLinqParte.ID_Personal
                        Else
                            Me.C_ResponsableInforme.Value = Nothing
                        End If
                    End If
                Case "ExplicacionHorasTecnico"
                    If oLinqParte.Parte_Asignacion.Count > 0 Then
                        Util.Cargar_Combo(Me.C_ResponsableExplicacion, "SELECT ID_Personal, Nombre FROM Personal Where ID_Personal in (Select ID_Personal From Parte_Asignacion Where ID_Parte=" & oLinqParte.ID_Parte & ") ORDER BY Nombre", False)
                        If oLinqParte.ID_PersonalExplicacionHorasTecnico.HasValue = True Then
                            Me.C_ResponsableExplicacion.Value = oLinqParte.ID_PersonalExplicacionHorasTecnico
                        Else
                            Me.C_ResponsableExplicacion.Value = Nothing
                        End If
                    End If
                Case "Step"
                    Dim DTS As New DataSet
                    BD.CargarDataSet(DTS, "Select * From C_Entrada Where ID_Entrada_Tipo=" & EnumEntradaTipo.AlbaranVenta & " and ID_Entrada in (Select ID_Entrada From Entrada_Parte Where ID_Parte= " & oLinqParte.ID_Parte & " Group By ID_Entrada) Order by FechaEntrada")
                    BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Tipo=" & EnumEntradaTipo.AlbaranVenta & " and ID_Entrada in (Select ID_Entrada From Entrada_Parte Where ID_Parte= " & oLinqParte.ID_Parte & " Group By ID_Entrada) Order by FechaEntrada", "aa", 0, "ID_Entrada", "ID_Entrada", True)
                    Me.GRD_Step_Albaranes.GRID.DisplayLayout.MaxBandDepth = 4 'tinc que fer aquesta merda pq si no no em surten els fills
                    Me.GRD_Step_Albaranes.M.clsUltraGrid.Cargar(DTS)


                    DTS = Nothing
                    BD.CargarDataSet(DTS, "Select * From C_Entrada Where ID_Entrada_Tipo=" & EnumEntradaTipo.FacturaVenta & " and ID_Entrada in (Select ID_Entrada From Entrada_Parte Where ID_Parte= " & oLinqParte.ID_Parte & " Group By ID_Entrada) Order by FechaEntrada")
                    BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Tipo=" & EnumEntradaTipo.FacturaVenta & " and ID_Entrada in (Select ID_Entrada From Entrada_Parte Where ID_Parte= " & oLinqParte.ID_Parte & " Group By ID_Entrada) Order by FechaEntrada", "aa", 0, "ID_Entrada", "ID_Entrada", True)
                    Me.GRD_Step_Facturas.GRID.DisplayLayout.MaxBandDepth = 4 'tinc que fer aquesta merda pq si no no em surten els fills
                    Me.GRD_Step_Facturas.M.clsUltraGrid.Cargar(DTS)
                Case "ToDo"
                    Call CargaGrid_ToDo(oLinqParte.ID_Parte)
                    Call CargaGrid_ToDo_Instalacion()

                Case "MantenimientosPlanificados"
                    Me.DT_FechaMaxima.Value = Now.Date.AddMonths(3)
                    Call CargarTree()
                    Call CargarGrid_MantenimientoAsignado()
                    Call CargarGrid_MantenimientoPendiente(0)

                Case "Especificaciones"
                    'Call CargaGrid_Especificaciones()
                    Dim _IDPropuesta As Integer = 0
                    If oLinqParte.ID_Propuesta.HasValue = True Then
                        _IDPropuesta = oLinqParte.ID_Propuesta
                    End If
                    Me.GRD_Especificaciones.M.clsUltraGrid.Cargar("Select * From C_Propuesta_Especificacion Where ID_Propuesta=" & _IDPropuesta, BD)
                Case "TrabajosARealizar"
                    Call CargaGrid_TrabajosARealizar(oLinqParte.ID_Parte)
            End Select



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AvanzarRetroceder(ByVal pAvanzar As Boolean)
        'Que pasa si no s'ha seleccinat cap article?

        Dim _IDATrobar As Integer
        Dim _Trobat As Boolean = False

        If oLinqParte.ID_Parte = 0 Then
            If Me.OP_Filtre.Value = "Cliente" Or Me.OP_Filtre.Value = "Instalacion" Then
                Exit Sub
            Else
                'Si actualment no tenim cap registre carregat farem veure com si estiguessim al últim.
                _IDATrobar = oDTC.Parte.Where(Function(F) F.Activo = True).Max(Function(F) F.ID_Parte)
                _Trobat = True

            End If
        Else
            _IDATrobar = oLinqParte.ID_Parte
        End If

        Dim _LlistatPartes As IList(Of Parte)
        Select Case Me.OP_Filtre.Value
            Case "Codigo"
                If pAvanzar = True Then
                    _LlistatPartes = oDTC.Parte.Where(Function(F) F.Activo = True).OrderBy(Function(F) F.ID_Parte).ToList
                Else
                    _LlistatPartes = oDTC.Parte.Where(Function(F) F.Activo = True).OrderByDescending(Function(F) F.ID_Parte).ToList
                End If
            Case "Cliente"
                If pAvanzar = True Then
                    _LlistatPartes = oDTC.Parte.Where(Function(F) F.Activo = True And F.ID_Cliente = oLinqParte.ID_Cliente).OrderBy(Function(F) F.ID_Parte).ToList
                Else
                    _LlistatPartes = oDTC.Parte.Where(Function(F) F.Activo = True And F.ID_Cliente = oLinqParte.ID_Cliente).OrderByDescending(Function(F) F.ID_Parte).ToList
                End If
            Case "Instalacion"
                If pAvanzar = True Then
                    _LlistatPartes = oDTC.Parte.Where(Function(F) F.Activo = True And F.ID_Instalacion = oLinqParte.ID_Instalacion).OrderBy(Function(F) F.ID_Parte).ToList
                Else
                    _LlistatPartes = oDTC.Parte.Where(Function(F) F.Activo = True And F.ID_Instalacion = oLinqParte.ID_Instalacion).OrderByDescending(Function(F) F.ID_Parte).ToList
                End If

        End Select


        Dim _Parte As Parte
        Dim _ParteSeguent As Parte
        For Each _Parte In _LlistatPartes
            If _Trobat = True Then
                _ParteSeguent = _Parte
                Exit For
            End If
            If _Parte.ID_Parte = _IDATrobar Then
                _Trobat = True
            End If
        Next
        If _ParteSeguent Is Nothing = False Then
            Dim _TabActual As String = Me.TAB_Principal.SelectedTab.Key
            Call Cargar_Form(_ParteSeguent.ID_Parte, True)
            Call CarregarDadesPestanyes(_TabActual)
        End If
    End Sub
#End Region

#Region "Events Varis"

    Private Sub TE_Codigo_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Call Cridar_Llistat_Generic()
        End If
        If e.Button.Key = "Cancelar" Then
            Call Netejar_Pantalla()
        End If
    End Sub

    Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown
        If Me.TE_Codigo.Text Is Nothing = False AndAlso IsNumeric(Me.TE_Codigo.Text) = True Then
            If e.KeyCode = Keys.Enter Then
                Dim ooLinqParte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = Me.TE_Codigo.Text).FirstOrDefault()
                If ooLinqParte Is Nothing = False Then
                    Call Cargar_Form(ooLinqParte.ID_Parte)
                Else
                    If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_NO_EXISTENT_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                        Dim Codi As String = Me.TE_Codigo.Text
                        Call Netejar_Pantalla()
                        Me.TE_Codigo.Text = Codi
                    Else
                        Call Netejar_Pantalla()
                    End If

                End If
            End If
        End If
    End Sub

    'Private Sub C_Familia_ValueChanged(sender As System.Object, e As System.EventArgs)
    '    If Me.C_Familia.Value Is Nothing = False Then
    '        Me.C_Subfamilia.ReadOnly = False
    '        Me.C_Subfamilia.Value = Nothing

    '        If IsNumeric(Me.C_Familia.Value) = True Then
    '            Util.Cargar_Combo(Me.C_Subfamilia, "SELECT ID_Producto_SubFamilia, Descripcion FROM Producto_SubFamilia WHERE Activo=1 and ID_Producto_Familia=" & Me.C_Familia.Value & " ORDER BY Descripcion", False)
    '        End If
    '    End If
    'End Sub

    Private Sub CH_Firmado_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Firmado.CheckedChanged
        If Me.CH_Firmado.Checked = True Then
            Me.T_PersonaQueLoFirmo.ReadOnly = False
        Else
            Me.T_PersonaQueLoFirmo.Text = Nothing
            Me.T_PersonaQueLoFirmo.ReadOnly = True
        End If
    End Sub

    Private Sub TE_Parte_Vinculado_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Parte_Vinculado.EditorButtonClick
        Try
            If e.Button.Key = "Lupeta" Then
                Dim Sql As String = ""
                If oLinqParte.ID_Parte <> 0 Then
                    Sql = " and ID_Parte<>" & oLinqParte.ID_Parte
                End If

                Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
                LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Parte Where Activo=1 " & Sql & " ORDER BY ID_Parte", Me.TE_Parte_Vinculado, "ID_Parte", "ID_Parte")
                ' AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat
            End If

            If e.Button.Key = "Cancelar2" Then
                Me.TE_Parte_Vinculado.Text = Nothing
                Me.TE_Parte_Vinculado.Tag = Nothing
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub TE_Cliente_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Cliente.EditorButtonClick
        Try
            If Me.TL_Instalacion.Tag Is Nothing = False Then 'Si hi ha una instalacio assignada no es podrà modificar el client
                Mensaje.Mostrar_Mensaje("No se puede modificar el cliente cuando hay una instalación asignada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If e.Button.Key = "Lupeta" Then
                Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
                LlistatGeneric.Mostrar_Llistat("SELECT * FROM Cliente Where ID_Cliente_Tipo=" & EnumClienteTipo.Cliente & " and Activo=1  ORDER BY Nombre", Me.TE_Cliente, "ID_Cliente", "Nombre")
                'AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatCliente
            End If

            If e.Button.Key = "Cancelar2" Then
                Me.TE_Cliente.Text = Nothing
                Me.TE_Cliente.Tag = Nothing
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub TE_Instalacion_EditorButtonClick(sender As Object, e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TL_Instalacion.EditorButtonClick
        Try

            Select Case e.Button.Key
                Case "Lupeta"
                    Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
                    LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Instalacion Where Activo=1 ORDER BY ID_Instalacion", Me.TL_Instalacion, "ID_Instalacion", "ID_Instalacion") ' and (Select Count(*) From Propuesta Where SeInstalo=1 and ID_Instalacion=C_Instalacion.ID_Instalacion and Activo=1)>0 
                    AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistatInstalacions
                Case "Ficha"
                    If Me.TL_Instalacion.Tag Is Nothing Then
                        Exit Sub
                    End If

                    Dim _IDInstalacio As Integer = Me.TL_Instalacion.Tag
                    Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                    frm.Entrada(_IDInstalacio)
                    frm.FormObrir(Me, True)

                Case "Cancelar2"
                    Me.TL_Instalacion.Text = Nothing
                    Me.TL_Instalacion.Tag = Nothing
                    Me.C_Presupuesto.Items.Clear()
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub C_Presupuesto_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Presupuesto.BeforeDropDown
        If Me.TL_Instalacion.TextLength = 0 Then
            Me.C_Presupuesto.Items.Clear()
        Else
            Dim IDSeleccionat As Integer = 0
            If Me.C_Presupuesto.SelectedIndex <> -1 Then
                IDSeleccionat = Me.C_Presupuesto.Value
            End If
            Util.Cargar_Combo(Me.C_Presupuesto, "Select ID_Propuesta, cast(Codigo as nvarchar(50)) + ' ' + Version + ' - ' + Descripcion From Propuesta Where Activo=1 and ID_Propuesta_Estado<>" & EnumPropuestaEstado.Pendiente & " and ID_Instalacion=" & Me.TL_Instalacion.Text, False)
            If IDSeleccionat <> 0 Then
                Me.C_Presupuesto.Value = IDSeleccionat
            End If
        End If
    End Sub

    Private Sub C_Tipo_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Tipo.ValueChanged
        Try
            Call ActivarTabsSegonsTipusDeParte()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub T_CostePrevisto_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles T_CostePrevisto.KeyUp
        Try
            Call CalcularCosteHoresRealitzades()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub T_CostePrevistoGastos_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles T_CostePrevistoGastos.KeyUp
        Try
            Call CalcularGastos()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub TAB_Principal_SelectedTabChanged(sender As System.Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles TAB_Principal.SelectedTabChanged
        Call CarregarDadesPestanyes(e.Tab.Key)
    End Sub

    Private Sub B_CambiarEstadoAEnCurso_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub frmParte_AlTancarForm(ByRef pCancel As Boolean) Handles Me.AlTancarForm
        If Me.Visible = True Then
            If oclsPilaFormularis.OcultarFormulari(Me) = True Then
                pCancel = True
            End If
        End If
    End Sub

    Private Sub L_Telefono_DoubleClick(sender As Object, e As EventArgs) Handles L_Telefono.DoubleClick
        Dim _Prefix As String = oDTC.Configuracion.FirstOrDefault.PrefijoCentralita
        Clipboard.Clear()
        Clipboard.SetText(_Prefix & sender.text)
    End Sub
    'aaaa
    'Private Sub C_Tipo_Documento_ValueChanged(sender As Object, e As EventArgs) Handles C_Tipo_Documento.ValueChanged
    '    If Me.C_Tipo_Documento.Items.Count = 0 OrElse Me.C_Tipo_Documento.Value Is Nothing Then
    '        Exit Sub
    '    End If

    '    Dim DTS As New DataSet
    '    BD.CargarDataSet(DTS, "Select * From C_Entrada Where ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Entrada in (Select ID_Entrada From Entrada_Parte Where ID_Parte= " & oLinqParte.ID_Parte & " Group By ID_Entrada) Order by FechaEntrada")
    '    BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Entrada in (Select ID_Entrada From Entrada_Parte Where ID_Parte= " & oLinqParte.ID_Parte & " Group By ID_Entrada) Order by FechaEntrada", "aa", 0, "ID_Entrada", "ID_Entrada", True)
    '    Me.GRD_Step_Albaranes.GRID.DisplayLayout.MaxBandDepth = 4 'tinc que fer aquesta merda pq si no no em surten els fills
    '    ' Me.GRD_Step.GRID.DataSource = DTS

    '    Me.GRD_Step_Albaranes.M.clsUltraGrid.Cargar(DTS)

    'End Sub

    Private Sub GRD_Step_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Step_Albaranes.M_GRID_DoubleClickRow2
        If e.Band.Index = 0 Then
            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
            frm.Entrada(e.Cells("ID_Entrada").Value, e.Cells("ID_Entrada_Tipo").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

    Private Sub C_Bono_BeforeDropDown(sender As Object, e As CancelEventArgs) Handles C_Bono.BeforeDropDown
        Dim _Aux As Integer = 0
        If IsNothing(Me.C_Bono.Value) = False Then
            _Aux = Me.C_Bono.Value
        End If

        If Me.TL_Instalacion.Tag Is Nothing Then
            Mensaje.Mostrar_Mensaje("No se puede asignar un bono si no se ha asignado antes una instalación", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            e.Cancel = True
            Exit Sub
        End If

        Call CargarCombo_Bonos(False)
        Me.C_Bono.Value = _Aux
    End Sub

    Private Sub C_Bono_EditorButtonClick(sender As Object, e As UltraWinEditors.EditorButtonEventArgs) Handles C_Bono.EditorButtonClick
        Select Case e.Button.Key
            Case "Cancelar2"
                Me.C_Bono.Value = Nothing

        End Select
    End Sub


    Private Sub B_Atras_Click(sender As Object, e As EventArgs) Handles B_Atras.Click
        Call AvanzarRetroceder(False)
    End Sub

    Private Sub B_Adelante_Click(sender As Object, e As EventArgs) Handles B_Adelante.Click
        Call AvanzarRetroceder(True)
    End Sub
#End Region

#Region "Caracteristicas Instalacion"

    'Private Sub Carga_Grid_Caracteristicas_Instalacion(ByVal pId As Integer)
    '    Try
    '        Dim Caracteristicas = From taula In oDTC.Producto_Producto_Caracteristica_Instalacion Where taula.ID_Producto = pId Select taula
    '        Me.GRD_Gastos.GRID.DataSource = Caracteristicas

    '        'Me.GRD_Caracteristicas_Personalizadas.GRID.DisplayLayout.Bands(0).Columns("ID_Conveni_Laboral").CellActivation = UltraWinGrid.Activation.NoEdit
    '        Me.GRD_Gastos.GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
    '        Me.GRD_Gastos.GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.EditAndSelectText

    '        'Me.GRD_Caracteristicas_Instalacion.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Caracteristica").Nullable = Nullable.EmptyString

    '        Call CargarComboCaracteristicaInstalacion()
    '        Call CargarComboVision()
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub CargarComboCaracteristicaInstalacion()
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oCaracteristicas As IQueryable(Of Producto_Caracteristica_Instalacion) = (From Taula In oDTC.Producto_Caracteristica_Instalacion Where Taula.Activo = True And Taula.ID_Producto_Familia = CInt(Me.C_Familia.Value) Order By Taula.Descripcion Select Taula)

    '        Valors.ValueListItems.Add("-1", "Selecciona una característica")
    '        For Each Producto_Caracteristica_Instalacion In oCaracteristicas
    '            Valors.ValueListItems.Add(Producto_Caracteristica_Instalacion.ID_Producto_Caracteristica_Instalacion, Producto_Caracteristica_Instalacion.Descripcion)
    '        Next

    '        Me.GRD_Gastos.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Caracteristica_Instalacion").Style = ColumnStyle.DropDownList
    '        Me.GRD_Gastos.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Caracteristica_Instalacion").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    'Private Sub CargarComboVision()
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oVision As IQueryable(Of Producto_Caracteristica_Vision) = (From Taula In oDTC.Producto_Caracteristica_Vision Where Taula.Activo = True Order By Taula.Descripcion Select Taula)


    '        For Each Producto_Caracteristica_Vision In oVision
    '            Valors.ValueListItems.Add(Producto_Caracteristica_Vision.ID_Producto_Caracteristica_Vision, Producto_Caracteristica_Vision.Descripcion)
    '        Next

    '        Me.GRD_Gastos.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Caracteristica_Vision").Style = ColumnStyle.DropDownList
    '        Me.GRD_Gastos.GRID.DisplayLayout.Bands(0).Columns("ID_Producto_Caracteristica_Vision").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    'Private Sub GRD_Caracteristicas_Instalacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Gastos.M_ToolGrid_ToolAfegir
    '    Try

    '        If oLinqProducto.ID_Producto = 0 Then
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '            Exit Sub
    '        End If

    '        Call CargarComboCaracteristicaInstalacion()

    '        Me.GRD_Gastos.GRID.DisplayLayout.Bands(0).AddNew() 'Afegim una Row al Grid
    '        'Me.GRD.GRID.Rows(Me.GRD.GRID.Rows.Count - 1).Cells("ID_Associat").Value = (From Taula In oDTC.Associats Where Taula.Activo = True Select Taula.ID_Associat).FirstOrDefault
    '        'Me.GRD_Caracteristicas_Personalizadas.GRID.Rows(Me.GRD_Caracteristicas_Personalizadas.GRID.Rows.Count - 1).Cells("ID_Producto_Caracteristica").ValueList.SelectedItemIndex = -1



    '        Me.GRD_Gastos.GRID.Rows(Me.GRD_Gastos.GRID.Rows.Count - 1).Cells("ID_Producto_Caracteristica_Instalacion").Value = "-1"
    '        Me.GRD_Gastos.GRID.Rows(Me.GRD_Gastos.GRID.Rows.Count - 1).Cells("Imprimible").Value = True
    '        'Establim que la variable Linia és el mateix que la última row del grid recient afegida
    '        Dim Linia As New Producto_Producto_Caracteristica_Instalacion
    '        Linia = Me.GRD_Gastos.GRID.Rows.GetItem(Me.GRD_Gastos.GRID.Rows.Count - 1).listObject

    '        'Afegim aquesta línia a la colecció de línies del actual albarà
    '        oLinqProducto.Producto_Producto_Caracteristica_Instalacion.Add(Linia)

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Caracteristicas_Instalacion_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Gastos.M_ToolGrid_ToolEliminar
    '    If Me.GRD_Gastos.GRID.Selected.Rows.Count = 1 Then
    '        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then
    '            Dim pRow As UltraGridRow
    '            If Me.GRD_Gastos.GRID.Selected.Cells.Count > 0 Then
    '                pRow = Me.GRD_Gastos.GRID.Selected.Cells(0).Row
    '            Else
    '                pRow = Me.GRD_Gastos.GRID.Selected.Rows(0)
    '            End If

    '            Dim Linea As Producto_Producto_Caracteristica_Instalacion = pRow.ListObject
    '            If Linea.ID_Producto_Producto_Caracteristica_Instalacion <> 0 Then
    '                oDTC.Producto_Producto_Caracteristica_Instalacion.DeleteOnSubmit(Linea)

    '            End If

    '            'pRow.Hidden = True
    '            oLinqProducto.Producto_Producto_Caracteristica_Instalacion.Remove(Linea)
    '            'Linea.Activo = False
    '            oDTC.SubmitChanges()
    '        End If
    '    End If
    'End Sub

    'Private Sub GRD_Caracteristicas_Instalacion_Personalizadas_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Gastos.M_ToolGrid_ToolClickBotonsExtras
    '    Try
    '        If e.Tool.Key = "CaracteristicasPredeterminadas" Then
    '            If oLinqProducto.ID_Producto = 0 Then
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '                Exit Sub
    '            End If

    '            Dim _Carac As Producto_Caracteristica_Instalacion

    '            For Each _Carac In oDTC.Producto_Caracteristica_Instalacion.Where(Function(F) F.Predeterminado = True And F.Activo = True And F.ID_Producto_Familia = CInt(Me.C_Familia.Value))
    '                If oLinqProducto.Producto_Producto_Caracteristica_Instalacion.Where(Function(F) F.ID_Producto_Caracteristica_Instalacion = _Carac.ID_Producto_Caracteristica_Instalacion).Count = 0 Then
    '                    Dim _newCarac As New Producto_Producto_Caracteristica_Instalacion
    '                    _newCarac.ID_Producto_Caracteristica_Instalacion = _Carac.ID_Producto_Caracteristica_Instalacion
    '                    _newCarac.Valor = " "
    '                    oLinqProducto.Producto_Producto_Caracteristica_Instalacion.Add(_newCarac)
    '                End If
    '            Next
    '            oDTC.SubmitChanges()

    '            Call Carga_Grid_Caracteristicas_Instalacion(oLinqProducto.ID_Producto)
    '        End If
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

#End Region

#Region "Grid Asignación"

    Private Sub CargaGrid_Asignacion(ByVal pId As Integer)
        Try
            Dim _Asignacion As IEnumerable(Of Parte_Asignacion) = From taula In oDTC.Parte_Asignacion Where taula.ID_Parte = pId Select taula

            With Me.GRD_AsignacionPersonal

                '.GRID.DataSource = _Asignacion
                .M.clsUltraGrid.CargarIEnumerable(_Asignacion)

                If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                    .M_Editable()
                Else
                    .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                End If

                'Call CargarCombo_Personal(.GRID)
                .GRID.DisplayLayout.Bands(0).Columns("Personal").Style = ColumnStyle.DropDownList
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_AsignacionPersonal_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_AsignacionPersonal.M_GRID_CellListSelect
        Try
            If e.Cell.Column.Key <> "Personal" Then 'si no modifiquem el combo de personal no hi hem de fer re aqui
                Exit Sub
            End If

            'Comprovem que el registre que hi havia abans no tenia hores imputades
            Dim _Personal As Personal = e.Cell.Value
            If oLinqParte.Parte_Horas.Where(Function(F) F.Personal Is _Personal).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible desasignar el personal mientras tenga horas imputadas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                e.Cell.CancelUpdate()
                Exit Sub
            End If

            'Comprovem que no s'hagi introduit aquest treballador abans
            _Personal = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
            If oLinqParte.Parte_Asignacion.Where(Function(F) F.Personal Is _Personal).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar la misma persona dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                e.Cell.CancelUpdate()
                Exit Sub
            End If

            ''El bloc de baix es per comprovar que aquest treballador està autoritzat pel client o simplement el client no te treballadors autoritzats. Si te treballadors aurotizats el client, i el treballador seleccionat no hi és llavors mostrarem un simple avís
            If oDTC.Cliente_PersonalAceptado.Where(Function(F) F.ID_Cliente = oLinqParte.ID_Cliente).Count <> 0 Then
                If _Personal.Cliente_PersonalAceptado.Where(Function(F) F.ID_Cliente = oLinqParte.ID_Cliente).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Aviso. Este trabajador no está en la lista de trabajadores autorizados por el cliente", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                End If
            End If

            'tot això es per aplicar el preu de cost al personal. Al seleccionar el personal per primera vegada automàticament es possarà el preu de cost
            e.Cell.Row.Cells("PrecioCoste").Value = _Personal.PrecioCoste
            e.Cell.Row.Cells("PrecioCosteHoraExtra").Value = _Personal.PrecioCosteHoraExtra

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Personal(ByVal pGrid As UltraWinGrid.UltraGrid, ByRef pCell As UltraWinGrid.UltraGridCell, ByVal pTOTS As Boolean)
        Try

            Dim oTaula As IQueryable(Of Personal)
            Dim Valors As New Infragistics.Win.ValueList
            Dim _Personal As Personal = pCell.Value

            If pTOTS = True Then
                oTaula = (From Taula In oDTC.Personal Where (Taula Is _Personal) Or Taula.Activo = True And Taula.FechaBajaEmpresa.HasValue = False Order By Taula.Nombre Select Taula)
            Else
                oTaula = (From Taula In oDTC.Personal Where Taula Is _Personal Order By Taula.Nombre Select Taula)
            End If

            Dim Var As Personal

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pCell.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_AsignacionPersonal_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_AsignacionPersonal.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_AsignacionPersonal

                'If Guardar(False) = False Then
                '    Exit Sub
                'End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte").Value = oLinqParte

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_AsignacionPersonal_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_AsignacionPersonal.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqParte.Parte_Asignacion.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _IDPersonal As Integer = e.Cells("ID_Personal").Value
            If oLinqParte.Parte_Horas.Where(Function(F) F.ID_Personal = _IDPersonal).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible desasignar el personal mientras tenga horas imputadas", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If oLinqParte.Parte_Gastos.Where(Function(F) F.ID_Personal = _IDPersonal).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible desasignar el personal mientras tenga gastos imputados", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            If oLinqParte.Parte_Incidencia.Where(Function(F) F.ID_Personal = _IDPersonal).Count > 0 Then
                Mensaje.Mostrar_Mensaje("Imposible desasignar el personal mientras tenga alguna incidencia asignada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_AsignacionPersonal_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_AsignacionPersonal.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()
            Call CalcularCosteHoresRealitzades()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_AsignacionPersonal_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_AsignacionPersonal.M_Grid_InitializeRow
        If e.ReInitialize = False Then
            Call CargarCombo_Personal(Sender, e.Row.Cells("Personal"), False)
        End If
    End Sub

    Private Sub GRD_AsignacionPersonal_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_AsignacionPersonal.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "Personal" Then
            Call CargarCombo_Personal(sender, e.Cell, True)
        End If
    End Sub
#End Region

#Region "Grid Horas"

    Private Sub CargaGrid_Horas(ByVal pId As Integer)
        Try
            Dim _Horas As IEnumerable(Of Parte_Horas) = From taula In oDTC.Parte_Horas Where taula.ID_Parte = pId Select taula

            With Me.GRD_Horas
                '.GRID.DataSource = _Horas
                .M.clsUltraGrid.CargarIEnumerable(_Horas)

                .M_Editable()

                Call CargarCombo_Personal3(.GRID)
                Call CargarCombo_EstadoHoras(.GRID)
                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqParte.ID_Instalacion)
                Call CargarCombo_Planta(.GRID, 0, oLinqParte.ID_Instalacion)
                Call CargarCombo_Zona(.GRID, 0, oLinqParte.ID_Instalacion)
                Call CargarCombo_ElementoAProteger(.GRID, 0, oLinqParte.ID_Instalacion)
                Call CargarCombo_Abertura(.GRID, 0, oLinqParte.ID_Instalacion)
                ' Call CargarCombo_Entrada(.GRID)
                Call CargarCombo_TipoActuacion(.GRID)

                .GRID.DisplayLayout.Bands(0).Columns("Entrada_Linea").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("Entrada_Linea").CellClickAction = CellClickAction.CellSelect
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_TipoActuacion(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Parte_Horas_TipoActuacion) = (From Taula In oDTC.Parte_Horas_TipoActuacion Order By Taula.Descripcion Select Taula)

            Dim Var As Parte_Horas_TipoActuacion
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Parte_Horas_TipoActuacion").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Parte_Horas_TipoActuacion").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Personal3(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal) = (From Taula In oDTC.Parte_Asignacion Where Taula.ID_Parte = oLinqParte.ID_Parte And Taula.Personal.Activo = True Order By Taula.Personal.Nombre Select Taula.Personal)

            Dim Var As Personal
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Personal").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Personal").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_EstadoHoras(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Parte_Horas_Estado) = (From Taula In oDTC.Parte_Horas_Estado Order By Taula.ID_Parte_Horas_Estado Select Taula)

            Dim Var As Parte_Horas_Estado
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Parte_Horas_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Parte_Horas_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Horas_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Horas.M_GRID_BeforeCellActivate
        Call GRD_Material_M_GRID_BeforeCellActivate(sender, e)
    End Sub

    Private Sub GRD_Horas_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Horas.M_GRID_CellListSelect
        Call GRD_Material_M_GRID_CellListSelect(sender, e)
    End Sub


    Private Sub GRD_Horas_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Horas.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Horas

                If oLinqParte.Parte_Asignacion.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible añadir, no hay ningún trabajador asignado al parte de trabajo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Fecha").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte").Value = oLinqParte

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Horas_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Horas.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqParte.Parte_Horas.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _Hora As Parte_Horas = e.ListObject
            If _Hora.ID_Entrada_Linea.HasValue = True Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar la línea, la línea esta vinculada con un albarán de venta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_Horas_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Horas.M_GRID_AfterRowUpdate
        Try

            'If Me.DT_Inicio.Value Is Nothing Then 'si no s'ha introduit la data d'inici, llavors, l'assignarem automàticament al introduir alguna hora.
            '    Me.DT_Inicio.Value = Now.Date
            '    oLinqParte.FechaInicio = Now.Date
            'End If

            Call CalcularHoresRealitzades()
            'Call CalcularCosteHoresRealitzades()
            'Call CalcularEstadoParte()

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub CargarCombo_Entrada(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Entrada_Linea) = (From Taula In oDTC.Entrada_Linea Where Taula.Parte_Horas.Count <> 0 Select Taula)

    '        Dim Var As Entrada_Linea
    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Entrada.Codigo)
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("Entrada_Linea").Style = ColumnStyle.DropDownList
    '        pGrid.DisplayLayout.Bands(0).Columns("Entrada_Linea").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub GRD_Generic_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Gastos.M_Grid_InitializeRow, GRD_Horas.M_Grid_InitializeRow, GRD_Material.M_Grid_InitializeRow
        If IsNothing(e.Row.Cells("ID_Entrada_Linea").Value) = False Then
            e.Row.Activation = Activation.Disabled
        End If
    End Sub

    Private Sub GRD_Generic_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Horas.M_ToolGrid_ToolClickBotonsExtras2, GRD_Gastos.M_ToolGrid_ToolClickBotonsExtras2, GRD_Material.M_ToolGrid_ToolClickBotonsExtras2
        If pGrid.GRID.ActiveRow Is Nothing Then
            Exit Sub
        End If

        If IsNothing(pGrid.GRID.ActiveRow.Cells("ID_Entrada_Linea").Value) Then
            Mensaje.Mostrar_Mensaje("No hay ningún albarán asignado", M_Mensaje.Missatge_Modo.INFORMACIO, "")
            Exit Sub
        End If

        Dim _IDEntradaLinea As Integer = pGrid.GRID.ActiveRow.Cells("ID_Entrada_linea").Value
        Dim _EntradaLinea As Entrada_Linea = oDTC.Entrada_Linea.Where(Function(F) F.ID_Entrada_Linea = _IDEntradaLinea).FirstOrDefault
        Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
        frm.Entrada(_EntradaLinea.ID_Entrada, EnumEntradaTipo.AlbaranVenta)
        frm.FormObrir(Me, True)

    End Sub

    'Private Sub GRD_Horas_M_GRID_BeforeRowUpdate(ByRef sender As Object, ByRef e As CancelableRowEventArgs) Handles GRD_Horas.M_GRID_BeforeRowUpdate
    '    If e.Row.Cells("ID_Personal").Value = -1 Then 'Fem això pq pot tornar un -1 i peta
    '        If e.Row.Cells("ID_Parte_Horas").Value = 0 Then
    '            'Establim que la variable Linia és el mateix que la última row del grid recient afegida
    '            Dim Linia As New Parte_Horas
    '            Linia = e.Row.ListObject
    '            oLinqParte.Parte_Horas.Remove(Linia)
    '        End If
    '        e.Cancel = True
    '        Exit Sub
    '    End If
    'End Sub

    'Private Sub GRD_Horas_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Horas.M_ToolGrid_ToolEliminar
    '    With GRD_Horas
    '        If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
    '            Exit Sub
    '        End If
    '        If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then

    '            Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

    '            If .GRID.Selected.Cells.Count > 0 Then
    '                pRow = .GRID.Selected.Cells(0).Row
    '            Else
    '                pRow = .GRID.Selected.Rows(0)
    '            End If

    '            Dim Linea As Parte_Horas = pRow.ListObject
    '            If Linea.ID_Parte_Horas <> 0 Then
    '                oDTC.Parte_Horas.DeleteOnSubmit(Linea)
    '            End If
    '            pRow.Delete(False)

    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)

    '        End If
    '        oDTC.SubmitChanges()
    '    End With
    'End Sub

    'Private Sub GRD_Horas_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Horas.M_GRID_CellListSelect
    '    Try
    '        'tot això es per aplicar el preu de cost al personal. Al seleccionar el personal per primera vegada automàticament es possarà el preu de cost
    '        If e.Cell.Column.Key = "ID_Personal" And e.Cell.Row.Cells("ID_Personal").Value = -1 Then
    '            Dim ValorCelda As Integer = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue

    '            If ValorCelda > 0 Then
    '                Dim _IDPersonal As Integer = ValorCelda
    '                Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = _IDPersonal).FirstOrDefault
    '                If _Personal Is Nothing = False Then
    '                    'Dim _Tarifa As Proveedor_Tarifa = _Proveedor.Proveedor_Tarifa.Where(Function(F) F.ID_Producto_Division = CInt(Me.C_Tipo.Value)).FirstOrDefault
    '                    'If _Tarifa Is Nothing = False Then
    '                    e.Cell.Row.Cells("Coste").Value = _Personal.PrecioCoste
    '                    'End If
    '                End If

    '            End If
    '        End If
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Assignacion_M_GRID_CellChange(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_AsignacionPersonal.M_GRID_CellChange
    '    Try
    '        If e.Cell.Column.Key = "Predeterminado" Then
    '            If e.Cell.Value = False Then
    '                Dim pRow As UltraGridRow
    '                For Each pRow In Me.GRD_Precios.GRID.Rows
    '                    If e.Cell.Row Is pRow = False Then
    '                        pRow.Cells(e.Cell.Column.Key).Value = False
    '                        pRow.Update()
    '                    End If
    '                Next
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

#End Region

#Region "Grid Gastos"

    Private Sub CargaGrid_Gastos(ByVal pId As Integer)
        Try
            Dim _Gastos As IEnumerable(Of Parte_Gastos) = From taula In oDTC.Parte_Gastos Where taula.ID_Parte = pId Select taula

            With Me.GRD_Gastos
                '.GRID.DataSource = _Gastos
                .M.clsUltraGrid.CargarIEnumerable(_Gastos)

                If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                    .M_Editable()
                Else
                    .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                End If

                Call CargarCombo_Personal3(.GRID)
                'Call CargarCombo_Entrada(.GRID)
                Call CargarCombo_TipoGastos(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Gastos_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Gastos.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Gastos
                If oLinqParte.Parte_Asignacion.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible añadir, no hay ningún trabajador asignado a éste parte de trabajo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Fecha").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte").Value = oLinqParte


            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Gastos_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Gastos.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _Gasto As Parte_Gastos = e.ListObject
            If _Gasto.ID_Entrada_Linea.HasValue = True Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar la línea, la línea esta vinculada con un albarán de venta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If


            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            Call CalcularGastos()
            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Gastos_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Gastos.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
        Call CalcularGastos()
    End Sub

    Private Sub CargarCombo_TipoGastos(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Parte_Gastos_Tipo) = (From Taula In oDTC.Parte_Gastos_Tipo Order By Taula.Descripcion Select Taula)

            Dim Var As Parte_Gastos_Tipo
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Parte_Gastos_Tipo").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Parte_Gastos_Tipo").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Grid ATenerEnCuenta"

    Private Sub CargaGrid_ATenerEnCuenta(ByVal pId As Integer)
        Try
            Dim _ATenerEnCuenta As IEnumerable(Of Parte_ATenerEnCuenta) = From taula In oDTC.Parte_ATenerEnCuenta Where taula.ID_Parte = pId Select taula

            With Me.GRD_ATenerEnCuenta
                '.GRID.DataSource = _ATenerEnCuenta
                .M.clsUltraGrid.CargarIEnumerable(_ATenerEnCuenta)

                If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                    .M_Editable()
                Else
                    .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                End If

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ATenerEnCuenta_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ATenerEnCuenta.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_ATenerEnCuenta

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte").Value = oLinqParte

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ATenerEnCuenta_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_ATenerEnCuenta.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqParte.Parte_ATenerEnCuenta.Remove(e.ListObject)
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

    Private Sub GRD_ATenerEnCuenta_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_ATenerEnCuenta.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Incidencias"

    Private Sub CargaGrid_Incidencia(ByVal pId As Integer)
        Try
            Dim _Incidencias As IEnumerable(Of Parte_Incidencia) = From taula In oDTC.Parte_Incidencia Where taula.ID_Parte = pId Select taula

            With Me.GRD_Incidencia
                '.GRID.DataSource = _Incidencias
                .M.clsUltraGrid.CargarIEnumerable(_Incidencias)

                If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                    .M_Editable()
                Else
                    .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                End If

                .GRID.DisplayLayout.Bands(0).Columns("ID_Parte_Vinculado").CellActivation = Activation.NoEdit
                Call CargarCombo_Personal3(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Incidencia_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Incidencia.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Incidencia
                If oLinqParte.Parte_Asignacion.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible añadir, no hay ningún trabajador asignado a éste parte de trabajo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Fecha").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte").Value = oLinqParte

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Incidencia_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Incidencia.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqParte.Parte_Incidencia.Remove(e.ListObject)
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

    Private Sub GRD_Incidencia_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Incidencia.M_ToolGrid_ToolClickBotonsExtras
        Try
            If Me.GRD_Incidencia.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If e.Tool.Key = "GenerarParte" Then
                Me.GRD_Incidencia.GRID.ActiveRow = Nothing

                Dim _NewParte As New Parte

                With oLinqParte

                    _NewParte.Activo = True
                    _NewParte.ID_Parte_Vinculado = .ID_Parte
                    _NewParte.ID_Cliente = .ID_Cliente
                    _NewParte.ID_Instalacion = .ID_Instalacion
                    _NewParte.ID_Propuesta = .ID_Propuesta

                    _NewParte.Direccion = .Direccion
                    _NewParte.QuienDetectoIncidencia = .QuienDetectoIncidencia
                    _NewParte.PersonaContacto = .PersonaContacto
                    _NewParte.Poblacion = .Poblacion
                    _NewParte.Provincia = .Provincia

                    _NewParte.Longitud = .Longitud
                    _NewParte.Latitud = .Latitud
                    _NewParte.CP = .CP
                    _NewParte.Pais = .Pais
                    _NewParte.Delegacion = .Delegacion

                    _NewParte.Telefono = .Telefono
                    _NewParte.TrabajoARealizar = Me.GRD_Incidencia.GRID.Selected.Rows(0).Cells("Descripcion").Value
                    _NewParte.TrabajoARealizarRTF = _NewParte.TrabajoARealizar


                    _NewParte.FechaAlta = Date.Now
                    _NewParte.ID_Parte_Estado = EnumParteEstado.Pendiente
                    _NewParte.ID_Parte_Tipo = EnumParteTipo.Reparacion

                    _NewParte.ID_Parte_TipoFacturacion = .ID_Parte_TipoFacturacion

                    Dim _aux As New Parte_Aux
                    _aux.ObservacionesTecnico = ""
                    _NewParte.Parte_Aux = _aux


                    oDTC.Parte.InsertOnSubmit(_NewParte)
                    oDTC.SubmitChanges()

                    Call CrearPreguntesQuestionari()
                    Call clsParte.CrearMagatzem(oDTC, oLinqParte)

                    oDTC.SubmitChanges()


                    Dim _ClsNotificacion As New clsNotificacion(oDTC)
                    _ClsNotificacion.CrearNotificacion_AlCrearParte(oLinqParte)

                    Me.GRD_Incidencia.GRID.Selected.Rows(0).Cells("ID_Parte_Vinculado").Value = _NewParte.ID_Parte
                    Me.GRD_Incidencia.GRID.Selected.Rows(0).Update()
                End With


            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Incidencia_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Incidencia.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Material No presupuestado"

    Private Sub CargaGrid_Material(ByVal pId As Integer)
        Try
            Dim _Material As IEnumerable(Of Parte_Material) = From taula In oDTC.Parte_Material Where taula.ID_Parte = pId Select taula

            With Me.GRD_Material
                '.GRID.DataSource = _Material
                .M.clsUltraGrid.CargarIEnumerable(_Material)

                If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                    .M_Editable()
                Else
                    .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                End If

                Call CargarCombo_Personal3(.GRID)
                Call CargarCombo_Estado(.GRID)
                Call CargarCombo_Proveedor(.GRID)
                ' Call CargarCombo_Producto(.GRID)
                Call CargarCombo_Emplazamiento(.GRID, 0, oLinqParte.ID_Instalacion)
                Call CargarCombo_Planta(.GRID, 0, oLinqParte.ID_Instalacion)
                Call CargarCombo_Zona(.GRID, 0, oLinqParte.ID_Instalacion)
                Call CargarCombo_ElementoAProteger(.GRID, 0, oLinqParte.ID_Instalacion)
                Call CargarCombo_Abertura(.GRID, 0, oLinqParte.ID_Instalacion)
                'Call CargarCombo_Entrada(.GRID)

                Call CarregarComboProductes()

                '.GRID.DisplayLayout.Bands(0).Columns("ID_Producto").AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
                '.GRID.DisplayLayout.Bands(0).Columns("ID_Producto").AutoCompleteMode = AutoCompleteMode.Suggest
                '.GRID.DisplayLayout.Bands(0).Columns("ID_Producto").CellActivation = Activation.AllowEdit
                '.GRID.DisplayLayout.Bands(0).Columns("ID_Producto").CellClickAction = CellClickAction.Edit

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Material_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Material.M_GRID_BeforeCellActivate
        Select Case e.Cell.Column.Key
            Case "Instalacion_ElementosAProteger"
                Call CargarCombo_ElementoAProteger(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Zona").Value)
            Case "Instalacion_Emplazamiento_Abertura"
                Call CargarCombo_Abertura(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Zona").Value)
            Case "Instalacion_Emplazamiento_Zona"
                Call CargarCombo_Zona(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento_Planta").Value)
            Case "Instalacion_Emplazamiento_Planta"
                Call CargarCombo_Planta(e.Cell, e.Cell.Row.Cells("ID_Instalacion_Emplazamiento").Value)
        End Select
    End Sub

    Private Sub GRD_Material_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Material.M_GRID_CellListSelect
        Try
            'Fem això pq al ser un ultracombo el cellListSelect peta
            If e.Cell.Column.Key = "Producto" Then
                Exit Sub
            End If

            If e.Cell.ValueListResolved Is Nothing = False AndAlso CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem.DataValue Is Nothing Then
                e.Cell.ValueList = Nothing
            End If

            Select Case e.Cell.Column.Key
                Case "Instalacion_Emplazamiento_Zona"
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Abertura").Value = Nothing
                    e.Cell.Row.Cells("Instalacion_ElementosAProteger").Value = Nothing

                Case "Instalacion_Emplazamiento_Planta"
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Zona").Value = Nothing
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Abertura").Value = Nothing
                    e.Cell.Row.Cells("Instalacion_ElementosAProteger").Value = Nothing
                Case "Instalacion_Emplazamiento"
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Planta").Value = Nothing
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Zona").Value = Nothing
                    e.Cell.Row.Cells("Instalacion_Emplazamiento_Abertura").Value = Nothing
                    e.Cell.Row.Cells("Instalacion_ElementosAProteger").Value = Nothing
                Case "Proveedor"
                    'tot això es per assignar el preu del article del proveidor seleccionat
                    Dim _Proveedor As Proveedor = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
                    Dim _Producto As Producto = e.Cell.Row.Cells("Producto").Value
                    If _Producto.Producto_Proveedor.Where(Function(F) F.Proveedor.Equals(_Proveedor)).FirstOrDefault Is Nothing = False Then
                        e.Cell.Row.Cells("Precio").Value = _Producto.Producto_Proveedor.Where(Function(F) F.Proveedor.Equals(_Proveedor)).FirstOrDefault.PVD
                    End If
                Case "Producto"
                    Dim _Producto As Producto = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
                    Call CargarCombo_Proveedor(Me.GRD_Material.GRID, _Producto.ID_Producto)
                    e.Cell.Row.Cells("Proveedor").Value = Nothing
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Estado(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Propuesta_Linea_Estado) = (From Taula In oDTC.Propuesta_Linea_Estado Where Taula.Activo = True And Taula.ID_Propuesta_Linea_Estado <> 5 Order By Taula.Codigo Select Taula)
            Dim Var As Propuesta_Linea_Estado

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Propuesta_Linea_Estado").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Propuesta_Linea_Estado").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Proveedor(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, Optional ByVal pIDProducto As Integer = 0)
        Try
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Proveedor)

            If pIDProducto = 0 Then
                oTaula = (From Taula In oDTC.Proveedor Where Taula.Activo = True Order By Taula.Nombre Select Taula)
            Else
                oTaula = (From Taula In oDTC.Proveedor Where Taula.Producto_Proveedor.Where(Function(F) F.ID_Producto = pIDProducto).Count > 0 Order By Taula.Nombre Select Taula)
            End If
            Dim Var As Proveedor

            Valors.ValueListItems.Add(Nothing, "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Proveedor").Style = ColumnStyle.DropDownList

            If pIDProducto = 0 Then
                pGrid.DisplayLayout.Bands(0).Columns("Proveedor").ValueList = Valors.Clone
            Else
                pGrid.ActiveRow.Cells("Proveedor").ValueList = Valors.Clone
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    'Private Sub CargarCombo_Producto(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Producto) = (From Taula In oDTC.Producto Where Taula.Producto_SubFamilia.ID_Producto_Subfamilia_Tipo = CInt(EnumProductoSubFamiliaTipo.Material) And Taula.Activo = True Order By Taula.Descripcion Select Taula)
    '        Dim Var As Producto

    '        For Each Var In oTaula
    '            Valors.ValueListItems.Add(Var, Var.Codigo & " - " & Var.Descripcion) '
    '        Next

    '        pGrid.DisplayLayout.Bands(0).Columns("Producto").Style = ColumnStyle.DropDown
    '        pGrid.DisplayLayout.Bands(0).Columns("Producto").ValueList = Valors.Clone

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub GRD_Material_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Material.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Material
                If oLinqParte.Parte_Asignacion.Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible añadir, no hay ningún trabajador asignado a éste parte de trabajo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Fecha").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte").Value = oLinqParte

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Material_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Material.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqParte.Parte_Material.Remove(e.ListObject)
                Exit Sub
            End If

            Dim _Material As Parte_Material = e.ListObject
            If _Material.ID_Entrada_Linea.HasValue = True Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar la línea, la línea esta vinculada con un albarán de venta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
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

    Private Sub GRD_Material_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Material.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
        'Call CalcularCosteHoresRealitzades()
    End Sub

    Private Sub CargarCombo_Emplazamiento(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento) = (From Taula In oDTC.Instalacion_Emplazamiento Where Taula.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
            Dim Var As New Instalacion_Emplazamiento

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next


            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarCombo_Planta(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer, Optional ByVal pIDEmplazamiento As Integer = 0)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta)

            If pIDEmplazamiento = 0 Then
                oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
            Else
                oTaula = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.ID_Instalacion_Emplazamiento = pIDEmplazamiento Order By Taula.Descripcion Select Taula)
            End If

            Dim Var As Instalacion_Emplazamiento_Planta

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Planta").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Planta(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDEmplazamiento As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta) = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.ID_Instalacion_Emplazamiento = pIDEmplazamiento Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Planta


            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            'pGrid.DisplayLayout.Bands(0).Columns("ID_Instalacion_Emplazamiento_Planta").Style = ColumnStyle.DropDownList

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Zona(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Zona) = (From Taula In oDTC.Instalacion_Emplazamiento_Zona Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Zona

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Zona").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Zona").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Zona(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDPlanta As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Zona) = (From Taula In oDTC.Instalacion_Emplazamiento_Zona Where Taula.ID_Instalacion_Emplazamiento_Planta = pIDPlanta Order By Taula.Descripcion Select Taula)
            Dim Var As Instalacion_Emplazamiento_Zona

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Abertura(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Abertura) = (From Taula In oDTC.Instalacion_Emplazamiento_Abertura Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Descripcion_Detallada Select Taula)
            Dim Var As Instalacion_Emplazamiento_Abertura

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion_Detallada)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Abertura").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_Emplazamiento_Abertura").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Abertura(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDZona As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Abertura) = (From Taula In oDTC.Instalacion_Emplazamiento_Abertura Where Taula.ID_Instalacion_Emplazamiento_Zona = pIDZona Order By Taula.Descripcion_Detallada Select Taula)
            Dim Var As Instalacion_Emplazamiento_Abertura

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion_Detallada)
            Next

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_ElementoAProteger(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal pBandaIndex As Integer, ByVal pIDInstalacion As Integer)
        Try
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_ElementosAProteger) = (From Taula In oDTC.Instalacion_ElementosAProteger Where Taula.Instalacion_Emplazamiento.ID_Instalacion = pIDInstalacion Order By Taula.Instalacion_ElementosAProteger_Tipo.Descripcion Select Taula)
            Dim Var As Instalacion_ElementosAProteger

            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Instalacion_ElementosAProteger_Tipo.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_ElementosAProteger").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(pBandaIndex).Columns("Instalacion_ElementosAProteger").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_ElementoAProteger(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDZona As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Instalacion_ElementosAProteger) = (From Taula In oDTC.Instalacion_ElementosAProteger Where Taula.ID_Instalacion_Emplazamiento_Zona = pIDZona Order By Taula.Instalacion_ElementosAProteger_Tipo.Descripcion Select Taula)
            Dim Var As Instalacion_ElementosAProteger

            'Valors.ValueListItems.Add(IIf(pItemBuitEsNull, DBNull.Value, Nothing), "")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Instalacion_ElementosAProteger_Tipo.Descripcion)
            Next

            pCelda.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
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

    Private Sub GRD_Productos_M_Grid_InitializeLayout(Sender As Object, e As InitializeLayoutEventArgs) Handles GRD_Material.M_Grid_InitializeLayout
        e.Layout.Bands(0).Columns("Producto").EditorComponent = Me.C_Producto
        e.Layout.Bands(0).Columns("Producto").AutoCompleteMode = AutoCompleteMode.SuggestAppend
        e.Layout.Bands(0).Columns("Producto").AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
    End Sub

#End Region

#Region "Grid Cuestionario"

    Private Sub CargaGrid_Cuestionario(ByVal pId As Integer)
        Try
            Dim _Respuestas As IEnumerable(Of Parte_Cuestionario_Respuestas) = From taula In oDTC.Parte_Cuestionario_Respuestas Where taula.ID_Parte = pId Order By taula.Parte_Cuestionario_Preguntas.Orden Select taula

            With Me.GRD_Questionario
                '.GRID.DataSource = _Incidencias
                .M.clsUltraGrid.CargarIEnumerable(_Respuestas)

                If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                    .M_Editable()
                Else
                    .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                End If

                .GRID.DisplayLayout.Bands(0).Columns("Parte_Cuestionario_Preguntas").CellActivation = Activation.NoEdit
                Call CargarCombo_Preguntas(.GRID)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Cuestionario_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Questionario.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub CargarCombo_Preguntas(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Parte_Cuestionario_Preguntas) = (From Taula In oDTC.Parte_Cuestionario_Preguntas)

            Dim Var As Parte_Cuestionario_Preguntas
            For Each Var In oTaula
                If IsNothing(Var.Detalle) = False AndAlso Var.Detalle.Length > 0 Then
                    Valors.ValueListItems.Add(Var, Var.Pregunta & " (" & Var.Detalle & ")")
                Else
                    Valors.ValueListItems.Add(Var, Var.Pregunta)
                End If
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Parte_Cuestionario_Preguntas").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Parte_Cuestionario_Preguntas").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Material Presupuestado"

    Private Sub CargaGrid_MaterialPresupuestado(Optional ByVal pBlanquejar As Boolean = False)
        Try
            Me.GRD_MaterialPresupuestado.GRID.DataSource = Nothing
            Me.GRD_Stock.GRID.DataSource = Nothing

            With Me.GRD_MaterialPresupuestado
                'If Indentat = False Then
                If pBlanquejar = True OrElse oLinqParte.ID_Propuesta Is Nothing = True Then
                    .M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where ID_Propuesta = 0 and Activo=1", BD)
                Else
                    .M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where ID_Propuesta = " & oLinqParte.ID_Propuesta & " and Activo=1", BD)
                End If

                '.GRID.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow
                '.GRID.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True
                '.GRID.DisplayLayout.Override.RowSelectorWidth = 16
                'Else
                '    Dim DTS As New DataSet
                '    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is null and ID_Propuesta=" & pId)
                '    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea2", 0, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea3", 1, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea4", 2, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea5", 3, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea6", 4, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea7", 5, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where  Activo=1 and ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea8", 6, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    ' BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea9", 7, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    ' BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea10", 8, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    'BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea11", 9, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)
                '    ' BD.CargarDataSet(DTS, "Select * From C_Propuesta_Linea Where ID_Propuesta_Linea_Vinculado is not null and ID_Propuesta=" & pId, "Propuesta_Linea12", 10, "ID_Propuesta_Linea", "ID_Propuesta_Linea_Vinculado", False)

                '    .M.clsUltraGrid.Cargar(DTS)
                '    .GRID.DisplayLayout.Bands(0).ColumnFilters.ClearAllFilters()
                '    .GRID.DisplayLayout.Override.RowFilterAction = Infragistics.Win.UltraWinGrid.RowFilterAction.HideFilteredOutRows
                '    .GRID.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False
                '    .GRID.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons
                'End If

                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing

                Call CargaGrid_Stock(oLinqParte.Almacen.FirstOrDefault.ID_Almacen)
            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_MaterialPresupuestado_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_MaterialPresupuestado.M_Grid_InitializeRow
        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_MaterialPresupuestado.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
    End Sub

#End Region

#Region "Reparaciones"
    Private Sub CargaGrid_Reparacion_TalComoSeInstalo()
        Try
            Dim PropuestaTalComoSeInstalo As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Instalacion = CInt(Me.TL_Instalacion.Text) And F.SeInstalo = True And F.Activo = True).FirstOrDefault
            If PropuestaTalComoSeInstalo Is Nothing = True Then
                Exit Sub
            End If

            With Me.GRD_Reparacion_TalComoSeInstalo
                .M.clsUltraGrid.Cargar("Select * From C_Propuesta_Linea Where  ID_Producto_Subfamilia_Tipo=" & EnumProductoSubFamiliaTipo.Material & " and Activo=1 and ID_Propuesta=" & PropuestaTalComoSeInstalo.ID_Propuesta & " and ID_Propuesta_Linea not in (Select ID_Propuesta_Linea From C_Parte_Reparacion Where ID_Parte=" & oLinqParte.ID_Parte & ")", BD)
                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Reparacion_TalComoSeInstalo_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Reparacion_TalComoSeInstalo.M_Grid_InitializeRow
        If IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Me.GRD_Reparacion_TalComoSeInstalo.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
    End Sub

    Private Sub GRD_Reparacion_TalComoSeInstalo_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Reparacion_TalComoSeInstalo.M_ToolGrid_ToolClickBotonsExtras
        Try
            If e.Tool.Key = "MarcarArticulo" Then
                If Me.GRD_Reparacion_TalComoSeInstalo.GRID.Selected.Rows.Count <> 1 Then
                    Exit Sub
                End If
                Dim Respuesta As String
                Respuesta = Mensaje.Mostrar_Entrada_Datos("Introduzca el motivo de la incidencia:", "", True)
                If Respuesta = "" Then
                    Exit Sub
                End If

                Dim _Reparacion As New Parte_Reparacion
                _Reparacion.MotivoAsignacion = Respuesta
                _Reparacion.Parte = oLinqParte
                _Reparacion.ID_Propuesta_Linea = Me.GRD_Reparacion_TalComoSeInstalo.GRID.Selected.Rows(0).Cells("ID_Propuesta_Linea").Value
                _Reparacion.ID_Parte_Reparacion_Tipo = EnumParteReparacionTipo.ReparacionInterna
                oLinqParte.Parte_Reparacion.Add(_Reparacion)
                oDTC.SubmitChanges()
                Call CargaGrid_Reparacion_TalComoSeInstalo()
                Call CargaGrid_Reparacion(oLinqParte.ID_Parte)
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargaGrid_Reparacion(ByVal pId As Integer)
        Try
            With Me.GRD_Reparacion

                .M.clsUltraGrid.Cargar("Select * From C_Parte_Reparacion Where ID_Parte=" & pId, BD)
                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing

                Dim pRow As UltraGridRow
                For Each pRow In .GRID.Rows
                    If pRow.Cells("Finalizado").Value = True Then
                        pRow.CellAppearance.BackColor = Color.LightGreen
                    End If
                Next
            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub AlTancarFormReparacion()
        Try
            Call CargaGrid_Reparacion(oLinqParte.ID_Parte)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Reparacion_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Reparacion.M_GRID_DoubleClickRow
        Call GRD_Reparacion_M_ToolGrid_ToolEditar(Nothing, Nothing)
    End Sub

    Private Sub GRD_Reparacion_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Reparacion.M_ToolGrid_ToolClickBotonsExtras
        Try
            If e.Tool.Key = "FinalizarReparacion" Then
                If Me.GRD_Reparacion.GRID.Selected.Rows.Count = 1 Then
                    Dim _IDReparacion As Integer = Me.GRD_Reparacion.GRID.Selected.Rows(0).Cells("ID_Parte_Reparacion").Value
                    Dim _Reparacion As Parte_Reparacion = oLinqParte.Parte_Reparacion.Where(Function(F) F.ID_Parte_Reparacion = _IDReparacion).FirstOrDefault
                    If _Reparacion.Finalizado = False Then
                        If Mensaje.Mostrar_Mensaje("¿Ha sido fallo del producto?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                            _Reparacion.FalloDelProducto = True
                        Else
                            _Reparacion.FalloDelProducto = False
                        End If
                        _Reparacion.Finalizado = True
                        oDTC.SubmitChanges()
                        Call CargaGrid_Reparacion(oLinqParte.ID_Parte)
                    End If
                End If
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Reparacion_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Reparacion.M_ToolGrid_ToolEditar
        Try
            If Me.GRD_Reparacion.GRID.Selected.Rows.Count <> 1 Then
                Exit Sub
            End If

            If oLinqParte.ID_Parte_Estado = EnumParteEstado.Finalizado Then
                Exit Sub
            End If

            If oLinqParte.Parte_Asignacion.Count = 0 Then
                Mensaje.Mostrar_Mensaje("Imposible asignar una reparación, no hay ningún trabajador asignado a éste parte de trabajo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Exit Sub
            End If

            Dim IDPropuestaLinea As Integer = Me.GRD_Reparacion.GRID.Selected.Rows(0).Cells("ID_Propuesta_Linea").Value
            Dim frm As New frmParte_Reparacion
            frm.Entrada(oLinqParte, oDTC, IDPropuestaLinea, Me.GRD_Reparacion.GRID.Selected.Rows(0).Cells("ID_Parte_Reparacion").Value)
            frm.FormObrir(Me, False)
            AddHandler frm.AlTancarForm, AddressOf AlTancarFormReparacion

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Reparacion_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Reparacion.M_ToolGrid_ToolEliminar
        With GRD_Reparacion
            If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then

                Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                If .GRID.Selected.Cells.Count > 0 Then
                    pRow = .GRID.Selected.Cells(0).Row
                Else
                    pRow = .GRID.Selected.Rows(0)
                End If

                If IsDBNull(pRow.Cells("Fecha").Value) = False Then 'Si s'ha escrit algo a la descripció vol dir que ja s'ha efectuat la reparació
                    Mensaje.Mostrar_Mensaje("Imposible eliminar, la reparación ya se ha realizado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                Dim _Reparacion As Parte_Reparacion = oDTC.Parte_Reparacion.Where(Function(F) F.ID_Parte_Reparacion = CInt(pRow.Cells("ID_Parte_Reparacion").Value)).FirstOrDefault

                'Si la reparació prové d'una revisió llavors no es podrà eliminar
                If _Reparacion.Parte_Revision.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar la reparación. La reparación proviene de un parte de revisión. Para eliminar la línea tendrà que eliminar todo el parte", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                oDTC.Parte_Reparacion.DeleteOnSubmit(_Reparacion)
                pRow.Delete(False)

                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)

            End If
            oDTC.SubmitChanges()
            Call CargaGrid_Reparacion_TalComoSeInstalo()
        End With
    End Sub
#End Region

#Region "Revisiones"

    Private Sub CargarGrid_Revision()
        Dim PropuestaTalComoSeInstalo As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Instalacion = CInt(Me.TL_Instalacion.Text) And F.SeInstalo = True And F.Activo = True).FirstOrDefault
        If PropuestaTalComoSeInstalo Is Nothing = True Then
            Exit Sub
        End If

        If oLinqParte.Parte_Revision.Count = 0 Then 'si encara no s'han importat les línies les importarem
            Dim Lineas = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta = PropuestaTalComoSeInstalo.ID_Propuesta And F.Activo = True And F.Producto.ID_Producto_Division = oLinqParte.ID_Producto_Division And F.Producto.Producto_Producto_Caracteristica_Instalacion.Where(Function(A) A.ID_Producto_Caracteristica_Vision = 3 Or A.ID_Producto_Caracteristica_Vision = 4).Count > 0)
            For Each Propuesta_Linea In Lineas
                Dim _Carac As Producto_Producto_Caracteristica_Instalacion
                For Each _Carac In Propuesta_Linea.Producto.Producto_Producto_Caracteristica_Instalacion
                    If Propuesta_Linea.ID_Instalacion_Emplazamiento.HasValue = True Then
                        If RetornaSiElIDTeElEmplazamientoDelParte(Propuesta_Linea.ID_Instalacion_Emplazamiento) = True Then
                            If _Carac.ID_Producto_Caracteristica_Vision = 3 Or _Carac.ID_Producto_Caracteristica_Vision = 4 Or _Carac.ID_Producto_Caracteristica_Vision = 6 Then
                                Dim _ParteRevision As New Parte_Revision
                                _ParteRevision.ID_Parte = oLinqParte.ID_Parte
                                _ParteRevision.ID_Producto_Producto_Caracteristica_Instalacion = _Carac.ID_Producto_Producto_Caracteristica_Instalacion
                                If _Carac.Verificable = True Then
                                    _ParteRevision.ID_Parte_Revision_Estado = EnumParteRevisionEstado.PendienteRevisar
                                Else
                                    _ParteRevision.ID_Parte_Revision_Estado = EnumParteRevisionEstado.NoSeRequiereRevision
                                End If
                                _ParteRevision.ID_Propuesta_Linea = Propuesta_Linea.ID_Propuesta_Linea
                                '_ParteRevision.Detalle = Propuesta_Linea.DetalleInstalacion
                                oLinqParte.Parte_Revision.Add(_ParteRevision)
                            End If
                        End If
                    End If
                Next
            Next
            oDTC.SubmitChanges()

        End If

        With Me.GRD_Revision
            Dim DTS As New DataSet

            BD.CargarDataSet(DTS, "Select *, (Select count(*) From C_Parte_Revision Where ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and ID_Parte_Revision_Estado=" & EnumParteRevisionEstado.Solucionado & " and ID_Parte=" & oLinqParte.ID_Parte & " ) as NumSolucionadas, (Select count(*) From C_Parte_Revision Where ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and ID_Parte_Revision_Estado=" & EnumParteRevisionEstado.PendienteRevisar & " and ID_Parte=" & oLinqParte.ID_Parte & " ) as NumPendientesRevisar, (Select count(*) From C_Parte_Revision Where ID_Parte=" & oLinqParte.ID_Parte & " and ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and ID_Parte_Revision_Estado=" & EnumParteRevisionEstado.Incorrecto & ") as NumIncorrectas From C_Propuesta_Linea Where ID_Propuesta_Linea In (Select ID_Propuesta_Linea From Parte_Revision Where ID_Parte=" & oLinqParte.ID_Parte & ") and ID_Propuesta=" & PropuestaTalComoSeInstalo.ID_Propuesta & " and ID_Producto in (Select ID_Producto From Producto_Producto_Caracteristica_Instalacion WHERE ID_Producto_Caracteristica_Vision in (3,4,6) Group By ID_Producto)")
            BD.CargarDataSet(DTS, "Select * From C_Parte_Revision Where ID_Parte=" & oLinqParte.ID_Parte & " and ID_Propuesta=" & PropuestaTalComoSeInstalo.ID_Propuesta & " Order by ID_Parte_Revision_Estado Desc ", "juan", 0, "ID_Propuesta_Linea", "ID_Propuesta_Linea", False)
            .M.clsUltraGrid.Cargar(DTS)
            .GRID.Selected.Rows.Clear()
            .GRID.ActiveRow = Nothing
            .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
            .GRID.DisplayLayout.Bands(1).Columns("ID_Parte_Revision_Estado").CellClickAction = CellClickAction.EditAndSelectText
            .GRID.DisplayLayout.Bands(1).Columns("ID_Parte_Revision_Estado").CellActivation = Activation.AllowEdit
            .GRID.DisplayLayout.Bands(1).Columns("Detalle").CellClickAction = CellClickAction.EditAndSelectText
            Call CargarCombo_EstadoRevision(.GRID)

            Dim pRow As UltraGridRow
            For Each pRow In .GRID.Rows

                If pRow.Band.Index = 0 Then
                    Dim NumSolucionadas As Integer = pRow.Cells("NumSolucionadas").Value
                    Dim NumPendientesRevisar As Integer = pRow.Cells("NumPendientesRevisar").Value
                    Dim NumIncorrectas As Integer = pRow.Cells("NumIncorrectas").Value
                    Call PintarFilaBanda(NumSolucionadas, NumPendientesRevisar, NumIncorrectas, pRow)
                Else
                    Call PintarFilaSubBanda(pRow)
                End If

                Dim pRow2 As UltraGridRow
                For Each pRow2 In pRow.ChildBands(0).Rows
                    Call PintarFilaSubBanda(pRow2)
                Next
            Next
        End With

        'si hi ha una solá línea que tingui ID_Parte_Reparación vol dir que ja s'ha exportat una vegada. LLavors no es podrà tornar a crear unaltre parte
        If oLinqParte.Parte_Revision.Where(Function(F) F.ID_Parte_Reparacion.HasValue = True).Count > 0 Then
            Me.GRD_Revision.M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.DesActivar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "GenerarParte")
            Me.GRD_Revision.M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.DesActivar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "MarcarLineasCorrectas")
            Me.GRD_Revision.M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.Activar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "VerParte")
            Me.GRD_Revision.GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False
            Me.GRD_Revision.GRID.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect
        Else
            Me.GRD_Revision.M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.Activar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "GenerarParte")
            Me.GRD_Revision.M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.Activar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "MarcarLineasCorrectas")
            Me.GRD_Revision.M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.DesActivar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "VerParte")
        End If
    End Sub

    Private Function RetornaSiElIDTeElEmplazamientoDelParte(ByVal pIDLinea As Integer) As Boolean
        If oLinqParte.Parte_Instalacion_Emplazamiento.Where(Function(F) F.ID_Instalacion_Emplazamiento = pIDLinea).Count = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub PintarFilaBanda(ByVal NumSolucionadas As Integer, ByVal NumPendientesRevisar As Integer, ByVal NumIncorrectas As Integer, ByRef pRow As UltraGridRow)
        If NumIncorrectas > 0 Then
            pRow.CellAppearance.BackColor = Color.LightCoral
        Else
            If NumSolucionadas > 0 Then
                pRow.CellAppearance.BackColor = Color.Yellow
            Else
                If NumPendientesRevisar = 0 Then
                    pRow.CellAppearance.BackColor = Color.LightGreen
                Else
                    pRow.CellAppearance.BackColor = Color.White
                End If
            End If
        End If
    End Sub

    Private Sub PintarFilaSubBanda(ByRef pRow As UltraGridRow, Optional ByVal pParaMantenimientosPlanificados As Boolean = False)
        If pRow.Band.Index = 1 Or pParaMantenimientosPlanificados = True Then
            Select Case pRow.Cells("ID_Parte_Revision_Estado").Value
                Case EnumParteRevisionEstado.NoSeRequiereRevision, EnumParteRevisionEstado.Correcto
                    pRow.CellAppearance.BackColor = Color.LightGreen
                Case EnumParteRevisionEstado.PendienteRevisar
                    pRow.CellAppearance.BackColor = Color.White
                Case EnumParteRevisionEstado.Incorrecto
                    pRow.CellAppearance.BackColor = Color.LightCoral
                Case EnumParteRevisionEstado.Solucionado
                    pRow.CellAppearance.BackColor = Color.Yellow
            End Select
            If pRow.Cells("ID_Parte_Revision_Estado").Value = EnumParteRevisionEstado.NoSeRequiereRevision Or pRow.Cells("ID_Parte_Revision_Estado").Value = EnumParteRevisionEstado.Solucionado Then
                pRow.Cells("ID_Parte_Revision_Estado").Activation = Activation.Disabled
                Exit Sub
            End If
        End If
    End Sub

    Private Sub CargarCombo_EstadoRevision(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, Optional ByVal pNoSeleccionables As Boolean = True, Optional ByVal pParaLosMantenimientosPlanificados As Boolean = False)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Parte_Revision_Estado)
            If pNoSeleccionables = True Then
                oTaula = (From Taula In oDTC.Parte_Revision_Estado Where Taula.Activo = True Order By Taula.Codigo Select Taula)
            Else
                oTaula = (From Taula In oDTC.Parte_Revision_Estado Where Taula.Activo = True And Taula.ID_Parte_Revision_Estado <> CInt(EnumParteRevisionEstado.Solucionado) And Taula.ID_Parte_Revision_Estado <> CInt(EnumParteRevisionEstado.NoSeRequiereRevision) Order By Taula.Codigo Select Taula)
            End If

            Dim Var As Parte_Revision_Estado

            For Each Var In oTaula

                Valors.ValueListItems.Add(Var.ID_Parte_Revision_Estado, Var.Descripcion)
            Next

            Dim _Banda As Integer
            If pParaLosMantenimientosPlanificados = True Then
                _Banda = 0
            Else
                _Banda = 1
            End If

            pGrid.DisplayLayout.Bands(_Banda).Columns("ID_Parte_Revision_Estado").Style = ColumnStyle.DropDownList
            If pNoSeleccionables = True Then
                pGrid.DisplayLayout.Bands(_Banda).Columns("ID_Parte_Revision_Estado").ValueList = Valors.Clone
            Else
                pGrid.ActiveRow.Cells("ID_Parte_Revision_Estado").ValueList = Valors.Clone
            End If


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Revision_M_GRID_AfterRowUpdate(sender As Object, e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Revision.M_GRID_AfterRowUpdate

        Dim DT As DataTable
        DT = BD.RetornaDataTable("Select (Select count(*) From C_Parte_Revision Where ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and ID_Parte_Revision_Estado=" & EnumParteRevisionEstado.Solucionado & " and ID_Parte=" & oLinqParte.ID_Parte & " ) as NumSolucionadas, (Select count(*) From C_Parte_Revision Where ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and ID_Parte_Revision_Estado=" & EnumParteRevisionEstado.PendienteRevisar & " and ID_Parte=" & oLinqParte.ID_Parte & " ) as NumPendientesRevisar, (Select count(*) From C_Parte_Revision Where ID_Parte=" & oLinqParte.ID_Parte & " and ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and ID_Parte_Revision_Estado=" & EnumParteRevisionEstado.Incorrecto & ") as NumIncorrectas From C_Propuesta_Linea Where ID_Propuesta_Linea=" & e.Row.Cells("ID_Propuesta_Linea").Value)
        Dim NumSolucionadas As Integer = DT(0).Item("NumSolucionadas")
        Dim NumPendientesRevisar As Integer = DT(0).Item("NumPendientesRevisar")
        Dim NumIncorrectas As Integer = DT(0).Item("NumIncorrectas")
        Call PintarFilaBanda(NumSolucionadas, NumPendientesRevisar, NumIncorrectas, e.Row.ParentRow)
        ' oDTC.SubmitChanges()

    End Sub

    Private Sub GRD_Revision_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Revision.M_Grid_InitializeRow, GRD_MantenimientoAsignado.M_Grid_InitializeRow, GRD_MantenimientoPendiente.M_Grid_InitializeRow
        If e.Row.Band.Index = 0 AndAlso IsNumeric(e.Row.Cells("ID_Producto_Division").Value) = True AndAlso e.Row.Cells("ID_Producto_Division").Value <> 0 Then
            Sender.parent.parent.parent.M.clsUltraGrid.CeldaFondoColor(e.Row.Cells("Division"), RetornaColorSegonsDivisio(e.Row.Cells("ID_Producto_Division").Value))
        End If
    End Sub

    Private Sub GRD_Revision_M_GRID_BeforeRowUpdate(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles GRD_Revision.M_GRID_BeforeRowUpdate
        Try
            If e.Row.Band.Index <> 1 Then
                Exit Sub
            End If
            Dim IDLinea As Integer = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)
            Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = IDLinea).FirstOrDefault

            Dim IDParteRevision As Integer = CInt(e.Row.Cells("ID_Parte_Revision").Value)
            Dim _ParteRevision As Parte_Revision = oDTC.Parte_Revision.Where(Function(F) F.ID_Parte_Revision = IDParteRevision).FirstOrDefault

            'If IsDBNull(e.Row.Cells("FechaPrevista").Value) Then
            '    _Linea.FechaPrevista = Nothing
            'Else
            '    _Linea.FechaPrevista = CDate(e.Row.Cells("FechaPrevista").Value)
            'End If


            _ParteRevision.Detalle = IIf(IsDBNull(e.Row.Cells("Detalle").Value), "", e.Row.Cells("Detalle").Value)
            _ParteRevision.ID_Parte_Revision_Estado = e.Row.Cells("ID_Parte_Revision_Estado").Value
            Call PintarFilaSubBanda(e.Row)

            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Revision_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Revision.M_ToolGrid_ToolClickBotonsExtras
        Try
            If e.Tool.Key = "VerParte" Then
                Dim Parte As Parte = oDTC.Parte.Where(Function(F) F.Activo = True And F.ID_Parte_Vinculado = oLinqParte.ID_Parte).FirstOrDefault

                If Parte Is Nothing = False Then
                    Dim frm As New frmParte
                    frm.Entrada(Parte.ID_Parte)
                    frm.FormObrir(Me, True)
                End If

            End If


            If e.Tool.Key = "MarcarLineasCorrectas" Then
                If Mensaje.Mostrar_Mensaje("¿Está seguro que desea dar por correctas todas las líneas pendientes de revisar?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Dim _Lineas As Parte_Revision
                For Each _Lineas In oLinqParte.Parte_Revision
                    If _Lineas.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.PendienteRevisar) Then
                        _Lineas.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.Correcto)
                    End If
                Next
                oDTC.SubmitChanges()

                Call CargarGrid_Revision()
            End If

            If e.Tool.Key = "GenerarParte" Then

                Me.GRD_Revision.GRID.ActiveRow = Nothing

                If oLinqParte.Parte_Revision.Where(Function(F) F.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.Incorrecto)).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede generar un parte de reparación si no hay ninguna línea incorrecta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If oLinqParte.Parte_Revision.Where(Function(F) F.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.PendienteRevisar)).Count > 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede generar un parte de reparación todavía hay líneas pendientes de revisar", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("Está seguro que desea generar un parte de reparación?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Dim _NewParte As New Parte

                With oLinqParte

                    _NewParte.Activo = True
                    _NewParte.ID_Parte_Vinculado = .ID_Parte
                    _NewParte.ID_Cliente = .ID_Cliente
                    _NewParte.ID_Instalacion = .ID_Instalacion
                    _NewParte.ID_Propuesta = .ID_Propuesta

                    _NewParte.Direccion = .Direccion
                    _NewParte.QuienDetectoIncidencia = .QuienDetectoIncidencia
                    _NewParte.PersonaContacto = .PersonaContacto
                    _NewParte.Poblacion = .Poblacion
                    _NewParte.Provincia = .Provincia
                    _NewParte.Longitud = .Longitud
                    _NewParte.Latitud = .Latitud
                    _NewParte.CP = .CP
                    _NewParte.Pais = .Pais
                    _NewParte.Delegacion = .Delegacion
                    _NewParte.Telefono = .Telefono
                    _NewParte.TrabajoARealizar = "Reparación del parte de revisión número:" & oLinqParte.ID_Parte 'Me.GRD_Incidencia.GRID.Selected.Rows(0).Cells("Descripcion").Value
                    _NewParte.TrabajoARealizarRTF = _NewParte.TrabajoARealizar

                    _NewParte.FechaAlta = Date.Now
                    _NewParte.ID_Parte_Estado = EnumParteEstado.Pendiente
                    _NewParte.ID_Parte_Tipo = EnumParteTipo.Reparacion

                    _NewParte.ID_Parte_TipoFacturacion = .ID_Parte_TipoFacturacion

                    Dim _aux As New Parte_Aux
                    _aux.ObservacionesTecnico = ""
                    _NewParte.Parte_Aux = _aux


                    '  Dim _LineaRevision
                    Dim _LineaRevision2 As Parte_Revision

                    For Each _Linea In oLinqParte.Parte_Revision.Where(Function(F) F.ID_Parte_Revision_Estado = EnumParteRevisionEstado.Incorrecto).GroupBy(Function(F) F.ID_Propuesta_Linea)
                        Dim _NewLineaReparacion As New Parte_Reparacion
                        Dim IDPropuestaLinea As Integer = _Linea.FirstOrDefault.ID_Propuesta_Linea
                        For Each _LineaRevision2 In oLinqParte.Parte_Revision.Where(Function(F) F.ID_Propuesta_Linea = IDPropuestaLinea And F.ID_Parte_Revision_Estado = EnumParteRevisionEstado.Incorrecto).OrderBy(Function(F) F.ID_Propuesta_Linea)
                            _NewLineaReparacion.MotivoAsignacion = _NewLineaReparacion.MotivoAsignacion & "Característica: " & _LineaRevision2.Producto_Producto_Caracteristica_Instalacion.Producto_Caracteristica_Vision.Descripcion & " - Valor esperado: " & _LineaRevision2.Producto_Producto_Caracteristica_Instalacion.Valor & " - Incidencia: " & _LineaRevision2.Detalle & vbCrLf
                            _LineaRevision2.Parte_Reparacion = _NewLineaReparacion
                        Next
                        _NewLineaReparacion.ID_Propuesta_Linea = _Linea.FirstOrDefault.ID_Propuesta_Linea
                        _NewLineaReparacion.ID_Parte_Reparacion_Tipo = EnumParteReparacionTipo.ReparacionInterna
                        _NewParte.Parte_Reparacion.Add(_NewLineaReparacion)
                    Next

                    oDTC.Parte.InsertOnSubmit(_NewParte)

                    oDTC.SubmitChanges()
                    Call clsParte.CrearMagatzem(oDTC, _NewParte) 'ho posem davant del submitchanges per a tenir el ID del magatzem
                    Dim _ClsNotificacion As New clsNotificacion(oDTC)
                    _ClsNotificacion.CrearNotificacion_AlCrearParte(_NewParte)
                    oDTC.SubmitChanges()

                    Call CrearPreguntesQuestionari()
                    oDTC.SubmitChanges()

                    Call CargarGrid_Revision()
                    Mensaje.Mostrar_Mensaje("Parte de reparación creado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                    'Me.GRD_Revision.GRID.Selected.Rows(0).Cells("ID_Parte_Vinculado").Value = _NewParte.ID_Parte
                    'Me.GRD_Revision.GRID.Selected.Rows(0).Update()
                End With
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Revision_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_Revision.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "ID_Parte_Revision_Estado" Then
            Call CargarCombo_EstadoRevision(Me.GRD_Revision.GRID, False)
        End If

    End Sub


#End Region


#Region "Grid ToDo"

    Private Sub CargaGrid_ToDo(ByVal pId As Integer)
        Try
            Dim _ToDo As IEnumerable(Of Parte_ToDo) = From taula In oDTC.Parte_ToDo Where taula.ID_Parte = pId Select taula

            With Me.GRD_ToDo
                '.GRID.DataSource = _Gastos
                .M.clsUltraGrid.CargarIEnumerable(_ToDo)

                If oLinqParte.ID_Parte_Estado <> EnumParteEstado.Finalizado Then
                    .M_Editable()
                    Me.GRD_ToDo.GRID.DisplayLayout.Bands(0).Columns("Usuario").CellActivation = Activation.Disabled
                Else
                    .GRID.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                    .GRID.DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.CellSelect
                End If

                'Call CargarCombo_Usuario(.GRID)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Usuario(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef pCell As UltraWinGrid.UltraGridCell, ByVal pSoloUsuarioSeleccionado As Boolean)
        Try
            Dim _Usuario As Usuario = pCell.Value
            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Usuario)
            If pSoloUsuarioSeleccionado = False Then
                oTaula = (From Taula In oDTC.Usuario Where (Taula.Activo = True And Taula.Personal.FechaBajaEmpresa.HasValue = False) Or (Taula Is _Usuario) Order By Taula.Nombre Select Taula)
            Else
                oTaula = (From Taula In oDTC.Usuario Where Taula Is _Usuario Order By Taula.Nombre Select Taula)
            End If

            Dim Var As Usuario

            For Each Var In oTaula
                If Var.ID_Personal.HasValue = True Then
                    Valors.ValueListItems.Add(Var, Var.Personal.Nombre)
                End If
            Next

            pCell.ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Seguridad_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_ToDo.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "Usuario" Then
            Call CargarCombo_Usuario(sender, e.Cell, False)
        End If
    End Sub

    Private Sub GRD_Seguridad_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_ToDo.M_Grid_InitializeRow
        If e.ReInitialize = False Then
            Call CargarCombo_Usuario(Sender, e.Row.Cells("Usuario"), True)
        End If
    End Sub

    Private Sub GRD_ToDo_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ToDo.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_ToDo
                'If oLinqParte.Parte_ToDo.Count = 0 Then
                '    Mensaje.Mostrar_Mensaje("Imposible añadir, no hay ningún trabajador asignado a éste parte de trabajo", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                '    Exit Sub
                'End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaAlta").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte").Value = oLinqParte
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Realizado").Value = False
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Usuario").Value = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ToDo_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_ToDo.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                oLinqParte.Parte_ToDo.Remove(e.ListObject)
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

    Private Sub GRD_ToDo_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_ToDo.M_GRID_AfterRowUpdate
        'Fem això per saber si estem fent una insersió o una modificació
        Dim _EsUnaInsersio As Boolean = False
        If IsDBNull(e.Row.Cells("ID_Parte_ToDo").Value) = True OrElse e.Row.Cells("ID_Parte_ToDo").Value = 0 Then
            _EsUnaInsersio = True
        End If

        oDTC.SubmitChanges()

        'Sim fem una inserció crearem una notificació
        If _EsUnaInsersio = True Then
            Dim _IDParte As Integer = e.Row.Cells("ID_Parte").Value
            Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = _IDParte).FirstOrDefault
            Dim _ClsNotificacion As New clsNotificacion(oDTC)
            _ClsNotificacion.CrearNotificacion_AlAñadirUnToDoDesdeLaPantallaParte(_Parte)
        End If
    End Sub

#End Region

#Region "Grid ToDo Instalacion"

    Private Sub CargaGrid_ToDo_Instalacion()
        Try
            Dim _ToDo As IEnumerable(Of Instalacion_ToDo) = From taula In oDTC.Instalacion_ToDo Where taula.ID_Instalacion = oLinqParte.ID_Instalacion Select taula

            With Me.GRD_ToDo_Instalacion
                '.GRID.DataSource = _Gastos
                .M.clsUltraGrid.CargarIEnumerable(_ToDo)

                .M_Editable()
                .GRID.DisplayLayout.Bands(0).Columns("ID_Parte").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("ID_Parte").CellClickAction = CellClickAction.CellSelect
                .GRID.DisplayLayout.Bands(0).Columns("FechaLimite").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("FechaLimite").CellClickAction = CellClickAction.CellSelect
                .GRID.DisplayLayout.Bands(0).Columns("FechaAlta").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("FechaAlta").CellClickAction = CellClickAction.CellSelect
                .GRID.DisplayLayout.Bands(0).Columns("Descripcion").CellClickAction = CellClickAction.CellSelect
                .GRID.DisplayLayout.Bands(0).Columns("Descripcion").CellClickAction = CellClickAction.CellSelect
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ToDo_Instalacion_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_ToDo_Instalacion.M_GRID_AfterRowUpdate
        Dim _ToDo As Instalacion_ToDo
        _ToDo = e.Row.ListObject
        _ToDo.Parte = oLinqParte
        oDTC.SubmitChanges()
    End Sub

#End Region

#Region "Grid Calendario"

    Private Sub CargaGrid_Calendario(ByVal pId As Integer)
        Try
            Dim _Calendari As IEnumerable = From taula In oDTC.Calendario_Operarios Where taula.ID_Parte = pId Order By taula.FechaInicio Descending Select taula.Asunto, taula.Descripcion, taula.FechaInicio, taula.FechaFin, taula.Personal.Nombre

            With Me.GRD_Calendario
                '.GRID.DataSource = _Incidencias
                .M.clsUltraGrid.CargarIEnumerable(_Calendari)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Calendario_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Calendario.M_GRID_ClickRow2
        Dim frm As New frmCalendarioTecnico
        frm.Entrada(e.Cells("FechaInicio").Value)
        frm.FormObrir(Me, True)
    End Sub

#End Region

#Region "Grid Stock"

    Private Sub CargaGrid_Stock(ByVal pIDAlmacen As Integer)
        Try

            Me.GRD_Stock.M.clsUltraGrid.Cargar("Select * From RetornaStock(0, " & pIDAlmacen & ")  Order by ProductoDescripcion", BD)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Stock_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_Stock.M_ToolGrid_ToolVisualitzarDobleClickRow
        'If Me.GRD_Stock.GRID.Selected.Rows.Count = 1 Then
        '    Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
        '    frm.Entrada(Me.GRD_Stock.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
        '    frm.FormObrir(Me, True)
        'End If
    End Sub

    Private Sub GRD_Stock_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Stock.M_GRID_DoubleClickRow2
        If Me.GRD_Stock.GRID.Selected.Rows.Count = 1 Then
            Dim frm As New frmProducto_Trazabilidad
            frm.Entrada(e.Cells("ID_Producto").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

#End Region

#Region "TrabajosARealizar"
    Private Sub CargaGrid_TrabajosARealizar(ByVal pId As Integer)
        Try
            With Me.GRD_TrabajosARealizar
                Dim DTS As New DataSet

                If GridTrabajosARealizarIndentat = True Then
                    Dim _Fin As Integer = 5
                    BD.CargarDataSet(DTS, "Select * From Parte_TrabajosARealizar Where ID_Partes_TrabajosARealizar_Obligatorio is null and ID_Parte=" & pId & " Order By NumDia, Orden")
                    For i = 0 To _Fin
                        BD.CargarDataSet(DTS, "Select * From Parte_TrabajosARealizar Where ID_Partes_TrabajosARealizar_Obligatorio is not null and ID_Parte=" & pId & " Order By NumDia, Orden", "pepe" & i + 2, i, "ID_Parte_TrabajosARealizar", "ID_Partes_TrabajosARealizar_Obligatorio", False)
                    Next
                Else
                    BD.CargarDataSet(DTS, "Select * From Parte_TrabajosARealizar Where ID_Parte=" & pId & " Order By NumDia, Orden")
                End If

                .M.clsUltraGrid.Cargar(DTS)
                .GRID.ActiveRow = Nothing
                .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
                For Each _Banda In .GRID.DisplayLayout.Bands


                    Call PosarEditableColumnes(_Banda.Index, "FechaPrevision")
                    Call PosarEditableColumnes(_Banda.Index, "Orden")
                    Call PosarEditableColumnes(_Banda.Index, "NumDia")
                    Call PosarEditableColumnes(_Banda.Index, "DuracionAproximada")
                    Call PosarEditableColumnes(_Banda.Index, "Participantes")

                    .GRID.DisplayLayout.Bands(_Banda.Index).Columns("FechaAlta").CellActivation = Activation.NoEdit
                    '.GRID.DisplayLayout.Bands(_Banda.Index).Columns("FechaAlta").CellClickAction = CellClickAction.
                Next

                Call CargarGrid_TrabajosARealizar_Materiales()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_TrabajosARealizar_M_GRID_AfterCellUpdate(sender As Object, e As UltraWinGrid.CellEventArgs) Handles GRD_TrabajosARealizar.M_GRID_AfterCellUpdate

        Dim _Trabajos As Parte_TrabajosARealizar = oDTC.Parte_TrabajosARealizar.Where(Function(F) F.ID_Parte_TrabajosARealizar = CInt(e.Cell.Row.Cells("ID_Parte_TrabajosARealizar").Value)).FirstOrDefault

        With _Trabajos
            Select Case e.Cell.Column.Key
                Case "FechaPrevision"
                    If IsDBNull(e.Cell.Value) Then
                        .FechaPrevision = Nothing
                    Else
                        .FechaPrevision = e.Cell.Value
                    End If
                Case "Orden"
                    If IsDBNull(e.Cell.Value) Then
                        .Orden = Nothing
                    Else
                        .Orden = e.Cell.Value
                    End If
                Case "NumDia"
                    If IsDBNull(e.Cell.Value) Then
                        .NumDia = Nothing
                    Else
                        .NumDia = e.Cell.Value
                    End If
                Case "DuracionAproximada"
                    If IsDBNull(e.Cell.Value) Then
                        .DuracionAproximada = Nothing
                    Else
                        .DuracionAproximada = e.Cell.Value
                    End If
                Case "Participantes"
                    If IsDBNull(e.Cell.Value) Then
                        .Participantes = Nothing
                    Else
                        .Participantes = e.Cell.Value
                    End If
            End Select
        End With
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_TrabajoARealizar_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_TrabajosARealizar.M_ToolGrid_ToolAfegir
        Try
            If oLinqParte.ID_Parte = 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                Exit Sub
            End If

            If Guardar(True) = False Then
                Exit Sub
            End If

            Dim frm As New frmParte_TrabajosARealizar
            frm.Entrada(oLinqParte, oDTC)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmTrabajosARealizar
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_TrabajosARealizar_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_TrabajosARealizar.M_ToolGrid_ToolClickBotonsExtras2
        GridTrabajosARealizarIndentat = Not GridTrabajosARealizarIndentat
        Call CargaGrid_TrabajosARealizar(oLinqParte.ID_Parte)
    End Sub

    Private Sub GRD_TrabajoARealizar_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_TrabajosARealizar.M_ToolGrid_ToolEditar
        Call GRD_TareasARealizar_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub GRD_TareasARealizar_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_TrabajosARealizar.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_TrabajosARealizar.GRID.Selected.Cells.Count = 0 And Me.GRD_TrabajosARealizar.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then

                Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                If Me.GRD_TrabajosARealizar.GRID.Selected.Cells.Count > 0 Then
                    pRow = Me.GRD_TrabajosARealizar.GRID.Selected.Cells(0).Row
                Else
                    pRow = Me.GRD_TrabajosARealizar.GRID.Selected.Rows(0)
                End If

                Dim _ID As Integer = pRow.Cells("ID_Parte_TrabajosARealizar").Value
                BD.EjecutarConsulta("Delete From Parte_TrabajosARealizar_Personal Where ID_Parte_TrabajosARealizar=" & _ID)
                BD.EjecutarConsulta("Delete From Parte_TrabajosARealizar Where ID_Parte_TrabajosARealizar=" & _ID)
                Call CargaGrid_TrabajosARealizar(oLinqParte.ID_Parte)

                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
                '_DTC.SubmitChanges()
                pRow.Hidden = True
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_TareasARealizar_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_TrabajosARealizar.M_ToolGrid_ToolVisualitzarDobleClickRow
        If oLinqParte.ID_Parte = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        If Me.GRD_TrabajosARealizar.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        If Guardar(True) = False Then
            Exit Sub
        End If

        Dim frm As New frmParte_TrabajosARealizar
        frm.Entrada(oLinqParte, oDTC, Me.GRD_TrabajosARealizar.GRID.Selected.Rows(0).Cells("ID_Parte_TrabajosARealizar").Value)
        AddHandler frm.FormClosing, AddressOf AlTancarfrmTrabajosARealizar
        frm.FormObrir(Me)
    End Sub

    Private Sub AlTancarfrmTrabajosARealizar()
        Call CargaGrid_TrabajosARealizar(oLinqParte.ID_Parte)
    End Sub

    Private Sub CargarGrid_TrabajosARealizar_Materiales()
        Try
            Me.GRD_TrabajosARealizar_Materiales.M.clsUltraGrid.Cargar("Select ID_Producto, ID_Parte, Sum(Cantidad), Referencia_Fabricante, Descripcion From C_Parte_TrabajosARealizar_Productos Where ID_Parte=" & oLinqParte.ID_Parte & " Group By Referencia_Fabricante, Descripcion, ID_Producto, ID_Parte Order by Descripcion", BD)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

    Private Sub C_Presupuesto_EditorButtonClick(sender As Object, e As UltraWinEditors.EditorButtonEventArgs) Handles C_Presupuesto.EditorButtonClick
        If Me.C_Presupuesto.Items.Count > 0 AndAlso IsNumeric(Me.C_Presupuesto.Value) Then

            Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = CInt(Me.C_Presupuesto.Value)).FirstOrDefault
            If _Propuesta Is Nothing = False Then

                Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                frm.Entrada(_Propuesta.ID_Instalacion)
                frm.FormObrir(frmPrincipal, True)

                Dim frm2 As New frmPropuesta
                frm2.Entrada(frm.oLinqInstalacion, frm.oDTC, _Propuesta.ID_Propuesta)
                AddHandler frm2.FormClosing, AddressOf frm.AlTancarfrmPropuesta
                frm2.FormObrir(frm)

            End If
        End If
    End Sub

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
                oLinqParte = Nothing
                Fichero = Nothing
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

#Region "MantenimientosPlanificados"

#Region "Tree"
    Private Sub CargarTree()
        Dim _TalIComoSeInstalo As Propuesta = oLinqParte.Instalacion.Propuesta.Where(Function(F) F.SeInstalo = True).FirstOrDefault
        If _TalIComoSeInstalo Is Nothing = True Then
            Exit Sub
        End If


        'Dim _Linea As Propuesta_Linea

        'For Each _Linea In _TalIComoSeInstalo.Propuesta_Linea.Where(Function(F) F.Propuesta_Linea_Mantenimiento.Where(Function(F2) F2.Realizado = False).Count > 0)
        '    If _Linea.Propuesta_Linea_Mantenimiento.Where(Function(F) F.)

        'Next
        If oTreeTipusManteniments Is Nothing = False Then
            oTreeTipusManteniments.Clear()
        End If
        Me.TreeMenu.DataSource = Nothing

        oTreeTipusManteniments = New Projects

        Dim DT As DataTable = BD.RetornaDataTable("Select ID_Producto_Familia, Familia, ID_Producto_Producto_Mantenimiento, TipoMantenimiento, Count(*) as Contador From C_Parte_Mantenimiento_Planificado Where Realizado=0 and ID_Parte is null and Fecha<='" & Me.DT_FechaMaxima.Value & "' and ID_Instalacion=" & oLinqParte.ID_Instalacion & " Group by ID_Producto_Familia, Familia, ID_Producto_Producto_Mantenimiento, TipoMantenimiento Order by Familia, TipoMantenimiento")

        If DT Is Nothing Then
            Exit Sub
        End If

        Dim _IDFamilia As Integer = 0
        Dim _DTRow As DataRow
        For Each _DTRow In DT.Rows
            If _IDFamilia <> _DTRow.Item("ID_Producto_Familia") Then
                _IDFamilia = _DTRow.Item("ID_Producto_Familia")
                oTreeTipusManteniments.Add(New Project(_DTRow.Item("ID_Producto_Familia"), _DTRow.Item("Familia")))
            End If
            oTreeTipusManteniments(oTreeTipusManteniments.Count - 1).Projects.Add(New Project(_DTRow.Item("ID_Producto_Producto_Mantenimiento"), _DTRow.Item("TipoMantenimiento") & "(" & _DTRow.Item("Contador") & ")"))
        Next

        'Dim oDTC As New DTCDataContext(BD.Conexion)
        'Dim _Avisos As IQueryable(Of Propuesta_Linea_Mantenimiento) = oDTC.Propuesta_Linea_Mantenimiento.Where(Function(F) F.Propuesta_Linea.Propuesta.ID_Instalacion = oLinqParte.ID_Instalacion And F.Realizado = False).OrderBy(Function(F2) F2.Producto_Producto_Mantenimiento.Producto_Mantenimiento.Descripcion).GroupBy (Function(F) )



        'Dim pepe As IQueryable(Of String)
        'pepe = _Avisos.GroupBy(Function(F) F.Producto_Producto_Mantenimiento.Producto_Mantenimiento.Descripcion And F.Fecha).Select(Function(Y) Y.Key)




        'Dim _Llistat As IQueryable(Of PlantillaCDG_Nivell1) = oDTC.PlantillaCDG_Nivell1.Where(Function(F) F.ID_PlantillaCDG = oLinqPlantillaCDG.ID_PlantillaCDG).OrderBy(Function(F) F.Descripcio)
        'Dim _Menu As PlantillaCDG_Nivell1
        'For Each _Menu In _Llistat

        '    Projects.Add(New Project(_Menu.ID_PlantillaCDG_Nivell1, _Menu.Descripcio, 0, True))

        '    Dim _ProjectNivell1 As Project = Projects(Projects.Count - 1)
        '    Dim _MenuNivell2 As PlantillaCDG_Nivell2
        '    For Each _MenuNivell2 In _Menu.PlantillaCDG_Nivell2
        '        _ProjectNivell1.Projects.Add(New Project(_MenuNivell2.ID_PlantillaCDG_Nivell2, _MenuNivell2.Descripcio, 0, True))

        '        Dim _ProjectNivell2 As Project = _ProjectNivell1.Projects(_ProjectNivell1.Projects.Count - 1)
        '        Dim _MenuNivell3 As PlantillaCDG_Nivell3
        '        For Each _MenuNivell3 In _MenuNivell2.PlantillaCDG_Nivell3
        '            _ProjectNivell2.Projects.Add(New Project(_MenuNivell3.ID_PlantillaCDG_Nivell3, _MenuNivell3.Descripcio, 0, True))
        '        Next

        '    Next

        'Next

        Me.TreeMenu.DataSource = oTreeTipusManteniments
        Me.TreeMenu.ExpandAll()

    End Sub

    Private Sub TreeMenu_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles TreeMenu.FocusedNodeChanged
        'Dim _Project As Project
        '_Project = Me.TreeMenu.GetDataRecordByNode(e.Node)
        If e.Node Is Nothing Then
            Exit Sub
        End If

        Dim _IDTipoMantenimiento As Integer
        If e.Node.Level = 0 Then
            _IDTipoMantenimiento = 0
        Else
            _IDTipoMantenimiento = e.Node.GetValue(0)
        End If


        'If _IDTipoMantenimiento = 0 Then
        '    Me.TreeMenu.MoveNext()
        '    Exit Sub
        'End If

        Call CargarGrid_MantenimientoPendiente(_IDTipoMantenimiento)
    End Sub

    Private Sub B_Aplicar_Click(sender As Object, e As EventArgs) Handles B_Aplicar.Click
        Call CargarTree()
    End Sub

#End Region

    Private Sub CargarGrid_MantenimientoPendiente(ByVal pIDProducto_Producto_Mantenimiento As Integer)
        Me.GRD_MantenimientoPendiente.M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select Planif.ID_Producto_Producto_Mantenimiento, Planif.ID_Propuesta_Linea_Mantenimiento, Planif.Fecha, C_Propuesta_Linea.* From C_Parte_Mantenimiento_Planificado as Planif, C_Propuesta_Linea Where Planif.ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and Planif.Realizado=0 and Planif.ID_Parte is null  and Planif.Fecha<='" & Me.DT_FechaMaxima.Value & "' and Planif.ID_Instalacion=" & oLinqParte.ID_Instalacion & " and Planif.ID_Producto_Producto_Mantenimiento=" & pIDProducto_Producto_Mantenimiento, True))
    End Sub
    Private Sub GRD_MantenimientoPendiente_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_MantenimientoPendiente.M_ToolGrid_ToolClickBotonsExtras2
        If Me.GRD_MantenimientoPendiente.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        Select Case e.Tool.Key
            Case "AsignarAlParte"
                ' BD.EjecutarConsulta("Update Propuesta_Linea_Mantenimiento Set ID_Parte=" & oLinqParte.ID_Parte & " Where ID_Propuesta_Linea_Mantenimiento=" & Me.GRD_MantenimientoPendiente.GRID.Selected.Rows(0).Cells("ID_Propuesta_Linea_Mantenimiento").Value)
                Dim _oPropuestaMantenimiento As Propuesta_Linea_Mantenimiento = oDTC.Propuesta_Linea_Mantenimiento.Where(Function(F) F.ID_Propuesta_Linea_Mantenimiento = CInt(Me.GRD_MantenimientoPendiente.GRID.Selected.Rows(0).Cells("ID_Propuesta_Linea_Mantenimiento").Value)).FirstOrDefault

                oLinqParte.Propuesta_Linea_Mantenimiento.Add(_oPropuestaMantenimiento)
                oDTC.SubmitChanges()
                Call CargarGrid_MantenimientoPendiente(Me.GRD_MantenimientoPendiente.GRID.Selected.Rows(0).Cells("ID_Producto_Producto_Mantenimiento").Value)
                Call CargarGrid_MantenimientoAsignado()
                Call CargarTree()
        End Select
    End Sub

    Private Sub CargarGrid_MantenimientoAsignado()
        Try
            With Me.GRD_MantenimientoAsignado
                .M.clsUltraGrid.Cargar(BD.RetornaDataTable("Select Planif.ID_Producto_Producto_Mantenimiento, Planif.ID_Propuesta_Linea_Mantenimiento, Planif.Fecha, Planif.ID_Parte_Revision_Estado, Planif.Detalle, C_Propuesta_Linea.* From C_Parte_Mantenimiento_Planificado as Planif, C_Propuesta_Linea Where Planif.ID_Propuesta_Linea=C_Propuesta_Linea.ID_Propuesta_Linea and Planif.ID_Parte is not null  and Planif.ID_Instalacion=" & oLinqParte.ID_Instalacion, True))
                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
                .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True
                .GRID.DisplayLayout.Bands(0).Columns("ID_Parte_Revision_Estado").CellClickAction = CellClickAction.EditAndSelectText
                .GRID.DisplayLayout.Bands(0).Columns("ID_Parte_Revision_Estado").CellActivation = Activation.AllowEdit
                .GRID.DisplayLayout.Bands(0).Columns("Detalle").CellClickAction = CellClickAction.EditAndSelectText
                Call CargarCombo_EstadoRevision(.GRID, True, True)

                Dim pRow As UltraGridRow
                For Each pRow In .GRID.Rows
                    Call PintarFilaSubBanda(pRow)
                Next

                'si hi ha una solá línea que tingui ID_Parte_Reparación vol dir que ja s'ha exportat una vegada. LLavors no es podrà tornar a crear unaltre parte
                If oLinqParte.Propuesta_Linea_Mantenimiento.Where(Function(F) F.ID_Parte_Reparacion.HasValue = True).Count > 0 Then
                    .M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.DesActivar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "GenerarParte")
                    .M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.DesActivar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "MarcarLineasCorrectas")
                    .M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.Activar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "VerParte")
                    .GRID.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False
                    .GRID.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect
                Else
                    .M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.Activar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "GenerarParte")
                    .M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.Activar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "MarcarLineasCorrectas")
                    .M.clsToolBar.Boto_Accions(M_UltraGrid.clsToolBar.Enum_Seleccio.DesActivar, M_UltraGrid.clsToolBar.Enum_Tipus_Seleccio.ALGUNOS, "VerParte")
                End If
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub
    Private Sub GRD_MantenimientoAsignado_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_MantenimientoAsignado.M_ToolGrid_ToolClickBotonsExtras2
        Select Case e.Tool.Key
            Case "MarcarLineasCorrectas"
                If Mensaje.Mostrar_Mensaje("¿Está seguro que desea dar por correctas todas las líneas pendientes de revisar?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Dim _Lineas As Propuesta_Linea_Mantenimiento
                For Each _Lineas In oLinqParte.Propuesta_Linea_Mantenimiento
                    If _Lineas.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.PendienteRevisar) Then
                        _Lineas.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.Correcto)
                    End If
                Next
                oDTC.SubmitChanges()

                Call CargarGrid_MantenimientoAsignado()


            Case "VerParte"
                Dim Parte As Parte = oDTC.Parte.Where(Function(F) F.Activo = True And F.ID_Parte_Vinculado = oLinqParte.ID_Parte).FirstOrDefault
                If oLinqParte.Propuesta_Linea_Mantenimiento.Where(Function(F) F.ID_Parte_Reparacion.HasValue = True).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("Todavía no se ha generado ningún parte de reparación", M_Mensaje.Missatge_Modo.INFORMACIO, , , False)
                    Exit Sub
                End If

                Dim _IDParte As Integer = oLinqParte.Propuesta_Linea_Mantenimiento.Where(Function(F) F.ID_Parte_Reparacion.HasValue = True).FirstOrDefault.Parte_Reparacion.Parte.ID_Parte

                If Parte Is Nothing = False Then
                    Dim frm As New frmParte
                    frm.Entrada(_IDParte)
                    frm.FormObrir(Me, True)
                End If

            Case "GenerarParte"
                Me.GRD_Revision.GRID.ActiveRow = Nothing

                If oLinqParte.Propuesta_Linea_Mantenimiento.Where(Function(F) F.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.Incorrecto)).Count = 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede generar un parte de reparación si no hay ninguna línea incorrecta", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If oLinqParte.Propuesta_Linea_Mantenimiento.Where(Function(F) F.ID_Parte_Revision_Estado = CInt(EnumParteRevisionEstado.PendienteRevisar)).Count > 0 Then
                    Mensaje.Mostrar_Mensaje("No se puede generar un parte de reparación todavía hay líneas pendientes de revisar", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                If Mensaje.Mostrar_Mensaje("Está seguro que desea generar un parte de reparación?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                    Exit Sub
                End If

                Dim _NewParte As New Parte

                With oLinqParte

                    _NewParte.Activo = True
                    _NewParte.ID_Parte_Vinculado = .ID_Parte
                    _NewParte.ID_Cliente = .ID_Cliente
                    _NewParte.ID_Instalacion = .ID_Instalacion
                    _NewParte.ID_Propuesta = .ID_Propuesta

                    _NewParte.Direccion = .Direccion
                    _NewParte.QuienDetectoIncidencia = .QuienDetectoIncidencia
                    _NewParte.PersonaContacto = .PersonaContacto
                    _NewParte.Poblacion = .Poblacion
                    _NewParte.Provincia = .Provincia
                    _NewParte.Longitud = .Longitud
                    _NewParte.Latitud = .Latitud
                    _NewParte.CP = .CP
                    _NewParte.Pais = .Pais
                    _NewParte.Delegacion = .Delegacion
                    _NewParte.Telefono = .Telefono
                    _NewParte.TrabajoARealizar = "Reparación del parte de revisión número:" & oLinqParte.ID_Parte 'Me.GRD_Incidencia.GRID.Selected.Rows(0).Cells("Descripcion").Value
                    _NewParte.TrabajoARealizarRTF = _NewParte.TrabajoARealizar


                    Dim _aux As New Parte_Aux
                    _aux.ObservacionesTecnico = ""
                    _NewParte.Parte_Aux = _aux

                    _NewParte.FechaAlta = Date.Now
                    _NewParte.ID_Parte_Estado = EnumParteEstado.Pendiente
                    _NewParte.ID_Parte_Tipo = EnumParteTipo.Reparacion

                    _NewParte.ID_Parte_TipoFacturacion = .ID_Parte_TipoFacturacion


                    '  Dim _LineaRevision
                    Dim _LineaRevision2 As Propuesta_Linea_Mantenimiento

                    For Each _Linea In oLinqParte.Propuesta_Linea_Mantenimiento.Where(Function(F) F.ID_Parte_Revision_Estado = EnumParteRevisionEstado.Incorrecto).GroupBy(Function(F) F.ID_Propuesta_Linea)
                        Dim _NewLineaReparacion As New Parte_Reparacion
                        Dim IDPropuestaLinea As Integer = _Linea.FirstOrDefault.ID_Propuesta_Linea
                        For Each _LineaRevision2 In oLinqParte.Propuesta_Linea_Mantenimiento.Where(Function(F) F.ID_Propuesta_Linea = IDPropuestaLinea And F.ID_Parte_Revision_Estado = EnumParteRevisionEstado.Incorrecto).OrderBy(Function(F) F.ID_Propuesta_Linea)
                            _NewLineaReparacion.MotivoAsignacion = _NewLineaReparacion.MotivoAsignacion & "TipoMantenimiento: " & _LineaRevision2.Producto_Producto_Mantenimiento.Producto_Mantenimiento.Descripcion & " - Valor esperado: " & _LineaRevision2.Producto_Producto_Mantenimiento.Valor & " - Incidencia: " & _LineaRevision2.Detalle & vbCrLf
                            _LineaRevision2.Parte_Reparacion = _NewLineaReparacion
                        Next
                        _NewLineaReparacion.ID_Propuesta_Linea = _Linea.FirstOrDefault.ID_Propuesta_Linea
                        _NewLineaReparacion.ID_Parte_Reparacion_Tipo = EnumParteReparacionTipo.ReparacionInterna
                        _NewParte.Parte_Reparacion.Add(_NewLineaReparacion)
                    Next

                    oDTC.Parte.InsertOnSubmit(_NewParte)

                    oDTC.SubmitChanges()
                    Call CrearPreguntesQuestionari()
                    Call clsParte.CrearMagatzem(oDTC, _NewParte) 'ho posem davant del submitchanges per a tenir el ID del magatzem
                    Dim _ClsNotificacion As New clsNotificacion(oDTC)
                    _ClsNotificacion.CrearNotificacion_AlCrearParte(_NewParte)
                    oDTC.SubmitChanges()
                    Call CargarGrid_MantenimientoAsignado()
                    Mensaje.Mostrar_Mensaje("Parte de reparación creado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
                    'Me.GRD_Revision.GRID.Selected.Rows(0).Cells("ID_Parte_Vinculado").Value = _NewParte.ID_Parte
                    'Me.GRD_Revision.GRID.Selected.Rows(0).Update()
                End With
        End Select

    End Sub

    Private Sub GRD_MantenimientoAsignado_M_GRID_BeforeCellActivate(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD_MantenimientoAsignado.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "ID_Parte_Revision_Estado" Then
            Call CargarCombo_EstadoRevision(Me.GRD_MantenimientoAsignado.GRID, False, True)
        End If
    End Sub

    Private Sub GRD_MantenimientoAsignado_M_GRID_BeforeRowUpdate(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles GRD_MantenimientoAsignado.M_GRID_BeforeRowUpdate
        Try
            Dim IDLinea As Integer = CInt(e.Row.Cells("ID_Propuesta_Linea").Value)
            Dim _Linea As Propuesta_Linea = oDTC.Propuesta_Linea.Where(Function(F) F.ID_Propuesta_Linea = IDLinea).FirstOrDefault

            Dim _ID As Integer = CInt(e.Row.Cells("ID_Propuesta_Linea_Mantenimiento").Value)
            Dim _Mante As Propuesta_Linea_Mantenimiento = oDTC.Propuesta_Linea_Mantenimiento.Where(Function(F) F.ID_Propuesta_Linea_Mantenimiento = _ID).FirstOrDefault

            'If IsDBNull(e.Row.Cells("FechaPrevista").Value) Then
            '    _Linea.FechaPrevista = Nothing
            'Else
            '    _Linea.FechaPrevista = CDate(e.Row.Cells("FechaPrevista").Value)
            'End If


            _Mante.Detalle = IIf(IsDBNull(e.Row.Cells("Detalle").Value), "", e.Row.Cells("Detalle").Value)
            _Mante.ID_Parte_Revision_Estado = e.Row.Cells("ID_Parte_Revision_Estado").Value
            Call PintarFilaSubBanda(e.Row, True)

            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Tree"

    Private Class Project
        Private IDMenu As Integer
        Private Descripcio As String
        Private owner_Renamed As Projects
        Private projects_Renamed As Projects
        Public Sub New()
            Me.owner_Renamed = Nothing
            Me.IDMenu = 0
            Me.Descripcio = ""
            Me.projects_Renamed = New Projects()
        End Sub
        Public Sub New(ByVal pIDMenu As Integer, ByVal pDescripcio As String)
            Me.IDMenu = pIDMenu
            Me.Descripcio = pDescripcio
            Me.projects_Renamed = New Projects()
        End Sub
        Public Sub New(ByVal projects As Projects, ByVal name As String, ByVal description As String, ByVal pIDTipusMenu As EnumTipusMenu, ByVal isTask As Boolean, ByVal pOrdre As Integer, ByVal pFotoIndex As Integer)
            Me.New(name, description)
            Me.projects_Renamed = projects
        End Sub
        <Browsable(False)> _
        Public Property Owner() As Projects
            Get
                Return owner_Renamed
            End Get
            Set(ByVal value As Projects)
                owner_Renamed = value
            End Set
        End Property
        Public Property pIDMenu() As Integer
            Get
                Return IDMenu
            End Get
            Set(ByVal value As Integer)
                If pIDMenu = value Then
                    Return
                End If
                IDMenu = value
                OnChanged()
            End Set
        End Property
        <Browsable(False)> _
        Public Property pDescripcio As String
            Get
                Return Descripcio
            End Get
            Set(ByVal value As String)
                If pDescripcio = value Then
                    Return
                End If
                Descripcio = value
                OnChanged()
            End Set
        End Property
        <Browsable(False)> _
        Public ReadOnly Property Projects() As Projects
            Get
                Return projects_Renamed
            End Get
        End Property
        Private Sub OnChanged()
            If owner_Renamed Is Nothing Then
                Return
            End If
            Dim index As Integer = owner_Renamed.IndexOf(Me)
            owner_Renamed.ResetItem(index)
        End Sub
    End Class
    '<treeList1>
    Private Class Projects
        Inherits BindingList(Of Project)
        Implements TreeList.IVirtualTreeListData

        Private Sub IVirtualTreeListData_VirtualTreeGetChildNodes(ByVal info As VirtualTreeGetChildNodesInfo) Implements TreeList.IVirtualTreeListData.VirtualTreeGetChildNodes
            Dim obj As Project = TryCast(info.Node, Project)
            info.Children = obj.Projects
        End Sub
        Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As Project)
            item.Owner = Me
            MyBase.InsertItem(index, item)
        End Sub
        Private Sub IVirtualTreeListData_VirtualTreeGetCellValue(ByVal info As VirtualTreeGetCellValueInfo) Implements TreeList.IVirtualTreeListData.VirtualTreeGetCellValue
            Dim obj As Project = TryCast(info.Node, Project)
            Select Case info.Column.Caption
                Case "IDMenu"
                    info.CellData = obj.pIDMenu
                Case "Descripcio"
                    info.CellData = obj.pDescripcio
            End Select
        End Sub
        Private Sub IVirtualTreeListData_VirtualTreeSetCellValue(ByVal info As VirtualTreeSetCellValueInfo) Implements TreeList.IVirtualTreeListData.VirtualTreeSetCellValue
            Dim obj As Project = TryCast(info.Node, Project)
            Select Case info.Column.Caption
                Case "IDMenu"
                    obj.pIDMenu = CInt(info.NewCellData)
                Case "Descripcio"
                    obj.pDescripcio = CStr(info.NewCellData)
            End Select
        End Sub
    End Class
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

End Class


