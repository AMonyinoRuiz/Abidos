Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Public Class frmProducto
    Dim oDTC As DTCDataContext
    Dim oLinqProducto As Producto
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2

#Region "M_ToolForm"

    Private Sub M_ToolForm1_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        Try
            If oLinqProducto.ID_Producto <> 0 Then
                'Comprovarem si s'ha usat mai, si no s'ha usat preguntarem si el volen eliminar definitivament.
                If oLinqProducto.Entrada_Linea.Count = 0 AndAlso oLinqProducto.Propuesta_Linea.Count = 0 AndAlso oLinqProducto.Instalacion_Contrato_Producto.Count = 0 AndAlso oLinqProducto.Parte_Material.Count = 0 AndAlso oLinqProducto.Parte_MaterialOperarios.Count = 0 AndAlso oLinqProducto.Parte_Reparacion.Count = 0 AndAlso oLinqProducto.Producto_Proveedor.Count = 0 Then
                    If Mensaje.Mostrar_Mensaje("¿Este producto todavía no se ha usado, desea eliminarlo completamente?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                        oDTC.Producto_Producto_Caracteristica.DeleteAllOnSubmit(oLinqProducto.Producto_Producto_Caracteristica)
                        oDTC.Producto_Producto_Caracteristica_Instalacion.DeleteAllOnSubmit(oLinqProducto.Producto_Producto_Caracteristica_Instalacion)
                        oDTC.Producto_Alternativo.DeleteAllOnSubmit(oLinqProducto.Producto_Alternativo)
                        oDTC.Producto_Archivo.DeleteAllOnSubmit(oLinqProducto.Producto_Archivo)
                        oDTC.Producto_DescripcionIdioma.DeleteAllOnSubmit(oLinqProducto.Producto_DescripcionIdioma)
                        oDTC.Producto_Producto_Mantenimiento.DeleteAllOnSubmit(oLinqProducto.Producto_Producto_Mantenimiento)
                        oDTC.Producto.DeleteOnSubmit(oLinqProducto)
                        oDTC.SubmitChanges()
                        Call Netejar_Pantalla()
                        Exit Sub
                    End If
                End If

                If Mensaje.Mostrar_Mensaje("Desea dar de alta/baja este producto?", M_Mensaje.Missatge_Modo.PREGUNTA, "") = M_Mensaje.Botons.SI Then
                    If DT_Baja.Value Is Nothing Then
                        Me.DT_Baja.Value = Date.Now
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                    Else
                        Me.DT_Baja.Value = Nothing
                        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                    End If
                    Call Guardar()
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

    Private Sub M_ToolForm1_m_ToolForm_Duplicar() Handles ToolForm.m_ToolForm_Imprimir
        Try
            If oLinqProducto.ID_Producto = 0 Then
                Exit Sub
            End If
            If Mensaje.Mostrar_Mensaje("Está seguro de querer duplicar el producto?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
                Exit Sub
            End If

            If Guardar() = False Then
                Exit Sub
            End If

            Dim Resposta As String = Mensaje.Mostrar_Entrada_Datos("Introduzca el nuevo código de producto:", Me.TE_Codigo.Text & "-Copia", False, False, True)

            If BD.RetornaValorSQL("SELECT Count (*) From Producto WHERE Codigo='" & Resposta & "'") > 0 Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                Exit Sub
            End If

            Dim _Clonat As New Producto
            Call GetFromForm(_Clonat)
            Dim _Carac As Producto_Producto_Caracteristica
            Dim _CaracInstalacion As Producto_Producto_Caracteristica_Instalacion
            Dim _Proveedores As Producto_Proveedor
            Dim _ProductoArchivo As Producto_Archivo
            Dim _Archivo As Archivo

            For Each _Carac In oLinqProducto.Producto_Producto_Caracteristica
                Dim Clon As New Producto_Producto_Caracteristica
                Clon.ID_Producto = _Carac.ID_Producto
                Clon.ID_Producto_Caracteristica = _Carac.ID_Producto_Caracteristica
                Clon.Imprimible = _Carac.Imprimible
                Clon.Valor = _Carac.Valor
                _Clonat.Producto_Producto_Caracteristica.Add(Clon)
            Next

            For Each _CaracInstalacion In oLinqProducto.Producto_Producto_Caracteristica_Instalacion
                Dim Clon As New Producto_Producto_Caracteristica_Instalacion
                Clon.ID_Producto = _CaracInstalacion.ID_Producto
                Clon.ID_Producto_Caracteristica_Instalacion = _CaracInstalacion.ID_Producto_Caracteristica_Instalacion
                Clon.Imprimible = _CaracInstalacion.Imprimible
                Clon.Valor = _CaracInstalacion.Valor
                Clon.ID_Producto_Caracteristica_Vision = _CaracInstalacion.ID_Producto_Caracteristica_Vision
                Clon.Verificable = _CaracInstalacion.Verificable
                _Clonat.Producto_Producto_Caracteristica_Instalacion.Add(Clon)
            Next

            For Each _Proveedores In oLinqProducto.Producto_Proveedor
                Dim Clon As New Producto_Proveedor
                Clon.ID_Producto = _Proveedores.ID_Producto
                Clon.ID_Proveedor = _Proveedores.ID_Proveedor
                Clon.Descuento = _Proveedores.Descuento
                Clon.Predeterminado = _Proveedores.Predeterminado
                Clon.PVD = _Proveedores.PVD
                Clon.PVP = _Proveedores.PVP
                _Clonat.Producto_Proveedor.Add(Clon)
            Next

            For Each _ProductoArchivo In oLinqProducto.Producto_Archivo
                Dim ClonArchivo As New Archivo
                Dim Clon As New Producto_Archivo
                ClonArchivo.Activo = _ProductoArchivo.Archivo.Activo
                ClonArchivo.CampoBinario = _ProductoArchivo.Archivo.CampoBinario
                ClonArchivo.Color = _ProductoArchivo.Archivo.Color
                ClonArchivo.Descripcion = _ProductoArchivo.Archivo.Descripcion
                ClonArchivo.Fecha = _ProductoArchivo.Archivo.Fecha
                ClonArchivo.ID_Usuario = _ProductoArchivo.Archivo.ID_Usuario
                ClonArchivo.Ruta_Fichero = _ProductoArchivo.Archivo.Ruta_Fichero
                ClonArchivo.Tamaño = _ProductoArchivo.Archivo.Tamaño
                ClonArchivo.Tipo = _ProductoArchivo.Archivo.Tipo
                oDTC.Archivo.InsertOnSubmit(ClonArchivo)

                Clon.Archivo = ClonArchivo
                _Clonat.Producto_Archivo.Add(Clon)

                'Si te una foto predeterminada també la predeterminarem al producte clonat
                If oLinqProducto.ID_Archivo_FotoPredeterminada.HasValue = True AndAlso _ProductoArchivo.ID_Archivo = oLinqProducto.ID_Archivo_FotoPredeterminada Then
                    _Clonat.Archivo = ClonArchivo
                End If
            Next



            _Clonat.Observaciones = ""
            _Clonat.Fecha_Alta = Date.Now
            _Clonat.Fecha_Baja = Nothing
            _Clonat.Activo = True
            _Clonat.Codigo = Resposta

            oDTC.Producto.InsertOnSubmit(_Clonat)
            oDTC.SubmitChanges()
            Dim NouID As Integer
            NouID = _Clonat.ID_Producto
            Call Netejar_Pantalla()
            Call Cargar_Form(NouID)
            Mensaje.Mostrar_Mensaje("Registro duplicado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub ToolForm_m_ToolForm_Buscar() Handles ToolForm.m_ToolForm_Buscar
        Call Cridar_Llistat_Generic()
    End Sub

    'Private Sub M_ToolForm1_m_ToolForm_ToolClick_Botones_Extras(ByVal Sender As Object, ByVal e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles ToolForm.m_ToolForm_ToolClick_Botones_Extras
    '    Try
    '        If oLinqAssociat.ID_Associat <> 0 Then
    '            Select Case e.Tool.Key
    '                Case "baixa"
    '                    If Mensaje.Mostrar_Mensaje("Estàs segur de donar de baixa l'associat?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
    '                        Dim frm As New frmAssociat_Baixa
    '                        frm.Entrada()
    '                        Dim oDisseny As New M_Disseny.ClsDisseny
    '                        oDisseny.Abrir_Formulario(Me, frm, False)
    '                        AddHandler frm.AlTancar, AddressOf AlTancarfrmBaixa
    '                    End If
    '                Case "alta"
    '                    If Mensaje.Mostrar_Mensaje("Estàs segur de tornar a donar d'alta l'associat?", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.SI Then
    '                        Me.DT_Baixa.Value = Nothing
    '                        Me.C_Estat.SelectedIndex = Util.Buscar_en_Combo(C_Estat, 1) ' 1 es estat d'alta
    '                        Call M_ToolForm1_m_ToolForm_Guardar()
    '                        Mensaje.Mostrar_Mensaje("Associat donat d'alta correctament", M_Mensaje.Missatge_Modo.INFORMACIO)
    '                        Me.ToolForm.ToolForm.Tools("alta").SharedProps.Visible = False
    '                        Me.ToolForm.ToolForm.Tools("baixa").SharedProps.Visible = True
    '                    End If

    '            End Select
    '        End If

    '        If e.Tool.Key = "ajuda" Then
    '            If BD.RetornaValorSQL("Select count(*) From FitxerAjuda Where NomFormulari='" & Me.Name & "'") = 0 Then
    '                Mensaje.Mostrar_Mensaje("No existeix ajuda per aquest formulari", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                Exit Sub
    '            End If

    '            Dim DT As DataTable = BD.RetornaDataTable("Select * From FitxerAjuda Where NomFormulari='" & Me.Name & "'")

    '            If My.Computer.FileSystem.FileExists(DT.Rows(0).Item("RutaFitxer") & "\" & DT.Rows(0).Item("NomFitxer")) = True Then
    '                System.Diagnostics.Process.Start(DT.Rows(0).Item("RutaFitxer") & "\" & DT.Rows(0).Item("NomFitxer"))
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)

    '    End Try
    'End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pIdProducto As Integer = 0)

        Try
            If Me.C_Division.Items.Count = 0 Then 'si encara no s'han carregat els combos els carregarem

                '            'oDisseny.Configuracio_Formulari(Me)
                Me.AplicarDisseny()

                Util.Cargar_Combo(Me.C_Division, "SELECT ID_Producto_Division, Descripcion FROM Producto_Division WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Familia, "SELECT ID_Producto_Familia, Descripcion FROM Producto_Familia WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Subfamilia, "SELECT ID_Producto_Subfamilia, Descripcion FROM Producto_SubFamilia WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Marca, "SELECT ID_Producto_Marca, Descripcion FROM Producto_Marca WHERE Activo=1 ORDER BY Descripcion", False)

                Util.Cargar_Combo(Me.C_Garantia, "SELECT ID_Producto_Garantia, Tiempo FROM Producto_Garantia WHERE Activo=1 ORDER BY Tiempo", False)
                Util.Cargar_Combo(Me.C_Grado, "SELECT ID_Producto_Grado, Descripcion FROM Producto_Grado WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Sistema_Trasmision, "SELECT ID_Producto_SistemaTransmision, Descripcion FROM Producto_SistemaTransmision WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_ATS, "SELECT ID_Producto_ATS, Descripcion FROM Producto_ATS WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Sistema_Trasmision2, "SELECT ID_Producto_SistemaTransmision, Descripcion FROM Producto_SistemaTransmision WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_ATS2, "SELECT ID_Producto_ATS, Descripcion FROM Producto_ATS WHERE Activo=1 ORDER BY Descripcion", False)

                Util.Cargar_Combo(Me.C_Tipo_Fuente_Alimentacion, "SELECT ID_Producto_Tipo_Fuente_Alimentacion, Descripcion FROM Producto_Tipo_Fuente_Alimentacion WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Tipo_Sirena, "SELECT ID_Producto_TipoSirena, Descripcion FROM Producto_TipoSirena WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Frecuencia, "SELECT ID_Producto_FrecuenciaInalambrica, Descripcion FROM Producto_FrecuenciaInalambrica WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Clase_Ambiental, "SELECT ID_Producto_ClaseAmbiental, Descripcion FROM Producto_ClaseAmbiental WHERE Activo=1 ORDER BY Descripcion", False)

                Util.Cargar_Combo(Me.C_CCTV_ClasePOE, "SELECT ID_Producto_ClasePOE, Descripcion FROM Producto_ClasePOE WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_CCTV_EstandardNEMA, "SELECT ID_Producto_EstandardNema, Descripcion FROM Producto_EstandardNema WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_CCTV_Luminosidad, "SELECT ID_Producto_Luminosidad, Descripcion FROM Producto_Luminosidad WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_CCTV_TipoRosca, "SELECT ID_Producto_TipoRosca, Descripcion FROM Producto_TipoRosca WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_CCTV_Voltage, "SELECT ID_Producto_Voltaje, Descripcion FROM Producto_Voltaje WHERE Activo=1 ORDER BY Descripcion", False)


                Util.Cargar_Combo(Me.C_INC_FrecuenciasInalambricas, "SELECT ID_Producto_Incendio_FrecuenciaInalambrica, Descripcion FROM Producto_Incendio_FrecuenciaInalambrica WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_INC_TipoDetector, "SELECT ID_Producto_TipoDetector, Descripcion FROM Producto_TipoDetector WHERE Activo=1 ORDER BY Descripcion", False)

                Util.Cargar_Combo(Me.C_Acceso_Cerradura, "SELECT ID_Producto_TipoCerradura, Descripcion FROM Producto_TipoCerradura WHERE Activo=1 ORDER BY Descripcion", False)
                Util.Cargar_Combo(Me.C_Acceso_Lector, "SELECT ID_Producto_TipoLector, Descripcion FROM Producto_TipoLector WHERE Activo=1 ORDER BY Descripcion", False)

                Util.Cargar_Combo(Me.C_Certificado_ClaseA, "Select 0, 'No Seleccionado'")
                Util.Cargar_Combo(Me.C_Certificado_Grado, "Select 0, 'No Seleccionado'")
                Util.Cargar_Combo(Me.C_FichaProducto, "Select 0, 'No Seleccionado'")

                Util.Cargar_Combo(Me.C_Tipo_Documento, "SELECT ID_Entrada_Tipo, Descripcion FROM Entrada_Tipo ORDER BY ID_Entrada_Tipo", False) ' Where Tipo='C'

                Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Producto_Archivo", 1)
                AddHandler Fichero.DespresDeCarregarDades, AddressOf DespresDeCarregarDadesGridArchivos
                AddHandler Fichero.DespresDeEliminarRegistre, AddressOf DespresDeEliminarElRegistreArchivos

                Me.DT_Alta.Value = Nothing
                Me.DT_Baja.Value = Nothing

                Me.ToolForm.M.Botons.tImprimir.SharedProps.Visible = True
                Me.ToolForm.M.Botons.tImprimir.SharedProps.Caption = "Duplicar"
                Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"

                Me.GRD_Caracteristicas_Personalizadas.M.clsToolBar.Boto_Afegir("CaracteristicasPredeterminadas", "Cargar características predeterminadas", True)
                Me.GRD_MantenimientosPlanificados.M.clsToolBar.Boto_Afegir("Mantenimientos", "Cargar mantenimientos planificados predeterminados", True)
                Me.GRD_Caracteristicas_Instalacion.M.clsToolBar.Boto_Afegir("CaracteristicasPredeterminadas", "Cargar características predeterminadas", True)
                Me.GRD_Ficheros.M.clsToolBar.Boto_Afegir("FotoPredeterminada", "Foto Predeterminada", True)
                Me.GRD_Ficheros.M.clsToolBar.Boto_Afegir("QuitarFotoPredeterminada", "Quitar foto predeterminada", True)

                Me.KeyPreview = False
            End If

            If pIdProducto <> 0 Then
                Call Cargar_Form(pIdProducto)
            Else
                Call Netejar_Pantalla()
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Function Guardar() As Boolean
        Guardar = False
        Try
            Me.TE_Codigo.Focus()
            If oLinqProducto.ID_Producto = 0 Then
                If BD.RetornaValorSQL("SELECT Count (*) From Producto WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            Else
                If BD.RetornaValorSQL("SELECT Count (*) From Producto WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_Producto<>" & oLinqProducto.ID_Producto) > 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
                    Exit Function
                End If
            End If

            If ComprovacioCampsRequeritsError() = True Then
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
                Exit Function
            End If

            Call GetFromForm(oLinqProducto)

            If oLinqProducto.ID_Producto = 0 Then  ' Comprovacio per saber si es un insertar o un nou
                oDTC.Producto.InsertOnSubmit(oLinqProducto)

                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)

                'Fem això pq un producte guardat no es pot canviar la divisió
                Me.C_Division.ReadOnly = True
                Me.C_Familia.ReadOnly = True
                Call Fichero.Cargar_GRID(oLinqProducto.ID_Producto)
                Call CopiarCaracteristicasPersonalitzades()
                Call CopiarCaracteristicasInstalacion()
                Call Carga_Grid_Caracteristicas_Instalacion(oLinqProducto.ID_Producto)
                Call Carga_Grid_Mantenimientos_Planificados(oLinqProducto.ID_Producto)
                Call Carga_Grid_Caracteristicas_Personalizadas(oLinqProducto.ID_Producto)
                Util.Tab_Activar(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.TODOS)

            Else

                Dim _NewODTC As New DTCDataContext(BD.Conexion)
                'Si l'article a la BD esta com a no obsoleto y ara el posem com a obsoleto enviarem una notificació
                If _NewODTC.Producto.Where(Function(F) F.ID_Producto = oLinqProducto.ID_Producto).FirstOrDefault.Obsoleto = False And oLinqProducto.Obsoleto = True Then
                    Dim _ClsNotificacion As New clsNotificacion(oDTC)
                    _ClsNotificacion.CrearNotificacion_AlMarcaUnProductoComoObsoleto(oLinqProducto)
                End If

                oDTC.SubmitChanges()
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
            End If

            'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
            Call CanviarTitolPantalla()
            Guardar = True

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Private Sub SetToForm()
        Try
            With oLinqProducto
                Me.T_PotenciaEntrada.Value = .PotenciaEntrada
                Me.T_PotenciaSalida.Value = .PotenciaSalida


                Me.TE_Codigo.Text = .Codigo
                Me.T_Descripcion.Text = .Descripcion
                Me.T_Referencia_Fabricante.Text = .Referencia_Fabricante
                Me.C_Division.Value = .ID_Producto_Division
                Me.C_Familia.Value = .ID_Producto_Familia
                Me.C_Subfamilia.Value = .ID_Producto_SubFamilia
                Me.C_Marca.Value = .ID_Producto_Marca
                Me.C_Grado.Value = .ID_Producto_Grado
                Me.C_Clase_Ambiental.Value = .ID_Producto_Clase_Ambiental
                Me.C_Garantia.Value = .ID_Producto_Garantia
                Me.C_Tipo_Fuente_Alimentacion.Value = .ID_Producto_Tipo_Fuente_Alimentacion
                Me.C_Tipo_Sirena.Value = .ID_Producto_TipoSirena
                Me.C_Sistema_Trasmision.Value = .ID_Producto_SistemaTransmision
                Me.C_ATS.Value = .ID_Producto_ATS
                Me.C_Sistema_Trasmision2.Value = .ID_Producto_SistemaTransmision2
                Me.C_ATS2.Value = .ID_Producto_ATS2
                Me.C_Frecuencia.Value = .ID_Producto_FrecuenciaInalambrica
                Me.DT_Alta.Value = .Fecha_Alta
                Me.DT_Baja.Value = .Fecha_Baja
                Me.CH_Baterias.Checked = .Baterias
                Me.CH_Bidireccional.Checked = .Bidirecciona
                Me.CH_Central.Checked = .Central
                Me.CH_Elemento_arme_desarme.Checked = .Elemento_arme_desarme
                Me.CH_Elemento_Deteccion.Checked = .Elemento_Deteccion
                Me.CH_Elemento_Verificacion.Checked = .Elemento_Verificación
                Me.CH_Expansor.Checked = .Expansor
                Me.CH_Fuente_Alimentacion.Checked = .Fuente_Alimentacion
                Me.CH_Inalambrico.Checked = .Inalambrico
                Me.CH_Modulo_Rele.Checked = .Modulo_Rele
                Me.CH_Pulsador.Checked = .Pulsador
                Me.CH_Sirena.Checked = .Sirena
                Me.CH_Sistema_Trasmision.Checked = .Sistema_Transmision
                Me.CH_Sistema_Trasmision2.Checked = .Sistema_Transmision2
                Me.CH_Supervisado.Checked = .Supervisado
                Me.CH_ControlPenetracion.Checked = .ControlPenetracion
                Me.CH_ElementoConectadoABus.Checked = .ConectadoBus
                Me.CH_MarcaEspecificada.Checked = .MarcaEspecificada
                Me.T_Num_Zonas.Value = .Central_Num_Zonas
                Me.T_Num_Zonas_Placa.Value = .Central_Num_Zonas_Placa
                Me.T_Num_Zonas_Inalambricas.Value = .Central_Num_Zonas_Inalambricas
                Me.T_Num_Zonas_Inalambricas_Placa.Value = .Central_Num_Zonas_Inalambricas_Placa
                Me.T_Expansor_Elementos.Value = .Expansor_Num_Elementos
                Me.T_Modulo_Rele_Elementos.Value = .Modulo_Rele_Num_Elementos
                Me.T_Num_Aberturas.Value = .Numero_Aberturas
                Me.T_Num_Elementos_Bus.Value = .Central_Num_Elementos_Max_Bus
                Me.T_Num_Zonas_Utilitzadas.Value = .Numero_Zonas_Utilizadas
                Me.T_TiempoInstalacion.Value = .TiempoInstalacion

                Me.T_PVP.Value = .PVP
                Me.T_PVD.Value = .PVD
                Me.T_PlazoEntrega.Value = .PlazoEntrega
                Me.CH_PVP_Proveedor_Predeterminado.Checked = .PVP_Proveedor_Predeterminado
                Call CH_PVP_Proveedor_Predeterminado_CheckedChanged(Nothing, Nothing)  'fem això pq els objectes es possin a enableds o dissableds quan toqui

                Me.R_Observaciones.pText = .Observaciones
                Me.R_DescripcionAmpliada.pText = .DescripcionAmpliada
                Me.R_DescripcionAmpliada_Tecnica.pText = .DescripcionAmpliada_Tecnica

                Me.T_VidaUtil.Value = .VidaUtil
                Me.T_StockMinimo.Value = .StockMinimo
                Me.T_StockMaximo.Value = .StockMaximo

                'CCTV
                Me.T_CCTV_NumCanales.Value = .CCTV_NumeroCanales
                Me.T_CCTV_NumCanalesMaximos.Value = .CCTV_NumeroCanalesMaximos
                Me.T_CCTV_NumCanalesUsados.Value = .CCTV_NumCanalesUsa
                Me.T_CCTV_NumDiscoDuros.Value = .CCTV_NumeroDiscoDurosSoportados
                Me.T_CCTV_NumMaximoMonitores.Value = .CCTV_NumeroMonitores

                Me.C_CCTV_ClasePOE.Value = .ID_Producto_ClasePOE
                Me.C_CCTV_EstandardNEMA.Value = .ID_Producto_EstandardNema
                Me.C_CCTV_Luminosidad.Value = .ID_Producto_Luminosidad
                Me.C_CCTV_TipoRosca.Value = .ID_Producto_TipoRosca
                Me.C_CCTV_Voltage.Value = .ID_Producto_Voltaje

                Me.CH_CCTV_Carcasa.Checked = .CCTV_Carcasa
                Me.CH_CCTV_ConexionBNC.Checked = .CCTV_ConexionBNC
                Me.CH_CCTV_ConexionUTP.Checked = .CCTV_ConexionUTP
                Me.CH_CCTV_DiscoDuro.Checked = .CCTV_DiscoDuro
                Me.CH_CCTV_ElementoCaptacion.Checked = .CCTV_ElementoCaptacion
                Me.CH_CCTV_ElementoDistribucion.Checked = .CCTV_ElementoDistribucion
                Me.CH_CCTV_ElementoGrabacion.Checked = .CCTV_ElementoGrabacion
                Me.CH_CCTV_FuenteAlimentacion.Checked = .CCTV_FuenteAlimentacion
                Me.CH_CCTV_Monitor.Checked = .CCTV_Monitor
                Me.CH_CCTV_NoIncluyeDispositivoAlmacenamiento.Checked = .CCTV_NoIncluyeDispositivoAlmacenamiento
                Me.CH_CCTV_Optica.Checked = .CCTV_Optica
                Me.CH_CCTV_POE.Checked = .CCTV_POE
                Me.CH_CCTV_PTZ.Checked = .CCTV_PTZ
                Me.CH_CCTV_RequiereAlimentacion.Checked = .CCTV_RequiereAlimentacion
                Me.CH_CCTV_Retenedor.Checked = .CCTV_Retenedor
                Me.CH_CCTV_ServidorVideo.Checked = .CCTV_ServidorVideoBNC
                Me.CH_CCTV_Teclado.Checked = .CCTV_Teclado

                'Incendios
                Me.T_INC_CentralesVinculadas.Value = .Inc_NumeroCentralesVinculadas
                Me.T_INC_ElementosPorLazo.Value = .Inc_ElementosPorLazo
                Me.T_INC_NumLazos.Value = .Inc_NumeroLazos

                Me.C_INC_FrecuenciasInalambricas.Value = .ID_Producto_FrecuenciaInalambrica
                Me.C_INC_TipoDetector.Value = .ID_Producto_TipoDetector


                Me.CH_INC_Aislador.Checked = .Inc_Aislador
                Me.CH_INC_Analogico.Checked = .Inc_Analogico
                Me.CH_INC_Base.Checked = .Inc_Base
                Me.CH_INC_Baterias.Checked = .Inc_Baterias
                Me.CH_INC_Cable.Checked = .Inc_Cable
                Me.CH_INC_Central.Checked = .Inc_Central
                Me.CH_INC_Convencional.Checked = .Inc_Convencional
                Me.CH_INC_Detector.Checked = .Inc_Detector
                Me.CH_INC_ElementoComunicacion.Checked = .Inc_ElementoComunicacion
                Me.CH_INC_Inalambrico.Checked = .Inc_Inalambrico
                Me.CH_INC_Luminoso.Checked = .Inc_Luminoso
                Me.CH_INC_Pulsador.Checked = .Inc_Pulsador
                Me.CH_INC_Relees.Checked = .Inc_Relees
                Me.CH_INC_RequiereBase.Checked = .Inc_Base
                Me.CH_INC_Sirena.Checked = .Inc_Sirena
                Me.CH_INC_Retenedor.Checked = .Inc_Retenedor

                Me.CH_Obsoleto.Checked = .Obsoleto

                Me.CH_Comercial.Checked = .Comercial
                Me.CH_Produccion.Checked = .Produccion

                'Accesos
                Me.T_Accesos_NumElementosCubre.Value = .Acceso_NumeroElementosCubre
                Me.C_Acceso_Cerradura.Value = .ID_Producto_TipoCerradura
                Me.C_Acceso_Lector.Value = .ID_Producto_TipoLector
                Me.CH_Acceso_Cerradura.Checked = .Acceso_Cerradura
                Me.CH_Acceso_Lector.Checked = .Acceso_Lector

                Me.T_Peso.Value = .Peso



                If DT_Baja.Value Is Nothing Then
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
                Else
                    Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de alta"
                End If

                Util.Cargar_Combo(Me.C_Certificado_ClaseA, "Select Archivo.ID_archivo, Descripcion From Archivo, Producto_Archivo Where Archivo.ID_Archivo=producto_archivo.ID_Archivo and Tipo like '%Adobe Acrobat%' and Activo=1 and ID_Producto_Archivo=" & oLinqProducto.ID_Producto, True, True, "No seleccionado")
                Util.Cargar_Combo(Me.C_Certificado_Grado, "Select Archivo.ID_archivo, Descripcion From Archivo, Producto_Archivo Where Archivo.ID_Archivo=producto_archivo.ID_Archivo and Tipo like '%Adobe Acrobat%' and Activo=1 and ID_Producto_Archivo=" & oLinqProducto.ID_Producto, True, True, "No seleccionado")
                Util.Cargar_Combo(Me.C_FichaProducto, "Select Archivo.ID_archivo, Descripcion From Archivo, Producto_Archivo Where Archivo.ID_Archivo=producto_archivo.ID_Archivo and Tipo like '%Adobe Acrobat%' and Activo=1 and ID_Producto_Archivo=" & oLinqProducto.ID_Producto, True, True, "No seleccionado")

                If .ID_Archivo_CertificadoClaseA.HasValue = True Then
                    Me.C_Certificado_ClaseA.Value = .ID_Archivo_CertificadoClaseA
                End If
                If .ID_Archivo_CertificadoGrado.HasValue = True Then
                    Me.C_Certificado_Grado.Value = .ID_Archivo_CertificadoGrado
                End If
                If .ID_Archivo_FichaTecnica.HasValue = True Then
                    Me.C_FichaProducto.Value = .ID_Archivo_FichaTecnica
                End If

                Me.T_Valoracion.Value = .Valoracion
                If .LinkAbidos Is Nothing = True Then
                    Me.UltraFormattedLinkLabel1.Value = "http://www.abidos.es"
                Else
                    Me.UltraFormattedLinkLabel1.Value = .LinkAbidos
                End If

                Me.CH_RequiereNS.Checked = .RequiereNumeroSerie

                If .ID_Producto_TipoCalculoPrecio.HasValue = True Then
                    Me.C_TipoCalculoPrecio.Value = .ID_Producto_TipoCalculoPrecio
                Else
                    Me.C_TipoCalculoPrecio.Value = 0
                End If

                Me.CH_Bono.Checked = .EsBono
                Me.T_Bono_Cantidad.Value = DbnullToNothing(.Bono_Cantidad)

                Call CargarFoto()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GetFromForm(ByRef pProducto As Producto)
        Try
            With pProducto
                .Activo = True
                .Codigo = Me.TE_Codigo.Text
                .Descripcion = Me.T_Descripcion.Text
                .Referencia_Fabricante = Me.T_Referencia_Fabricante.Text
                .ID_Producto_Division = Me.C_Division.Value
                .ID_Producto_Familia = Me.C_Familia.Value

                .PotenciaEntrada = DbnullToNothing(Me.T_PotenciaEntrada.Value)
                .PotenciaSalida = DbnullToNothing(Me.T_PotenciaSalida.Value)

                .ID_Producto_SubFamilia = Me.C_Subfamilia.Value
                If Me.C_Marca.Value = 0 Then
                    .Producto_Marca = Nothing
                Else
                    .Producto_Marca = oDTC.Producto_Marca.Where(Function(F) F.ID_Producto_Marca = CInt(Me.C_Marca.Value)).FirstOrDefault
                End If
                .ID_Producto_Garantia = Me.C_Garantia.Value
                .ID_Producto_Grado = Me.C_Grado.Value
                .ID_Producto_Clase_Ambiental = Me.C_Clase_Ambiental.Value
                .ID_Producto_Tipo_Fuente_Alimentacion = Me.C_Tipo_Fuente_Alimentacion.Value
                .ID_Producto_TipoSirena = Me.C_Tipo_Sirena.Value
                .ID_Producto_SistemaTransmision = Me.C_Sistema_Trasmision.Value
                .ID_Producto_ATS = Me.C_ATS.Value
                .ID_Producto_SistemaTransmision2 = Me.C_Sistema_Trasmision2.Value
                .ID_Producto_ATS2 = Me.C_ATS2.Value
                .ID_Producto_FrecuenciaInalambrica = Me.C_Frecuencia.Value

                If oLinqProducto.ID_Producto = 0 Then 'si estem fent una alta guardarem la data d'alta
                    .Fecha_Alta = Now.Date
                End If

                .Fecha_Baja = Me.DT_Baja.Value
                .Baterias = Me.CH_Baterias.Checked
                .Bidirecciona = Me.CH_Bidireccional.Checked
                .Central = Me.CH_Central.Checked
                .Elemento_arme_desarme = Me.CH_Elemento_arme_desarme.Checked
                .Elemento_Deteccion = Me.CH_Elemento_Deteccion.Checked
                .Elemento_Verificación = Me.CH_Elemento_Verificacion.Checked
                .Expansor = Me.CH_Expansor.Checked
                .Fuente_Alimentacion = Me.CH_Fuente_Alimentacion.Checked
                .Inalambrico = Me.CH_Inalambrico.Checked
                .Modulo_Rele = Me.CH_Modulo_Rele.Checked
                .Pulsador = Me.CH_Pulsador.Checked
                .Sirena = Me.CH_Sirena.Checked
                .Sistema_Transmision = Me.CH_Sistema_Trasmision.Checked
                .Sistema_Transmision2 = Me.CH_Sistema_Trasmision2.Checked
                .Supervisado = Me.CH_Supervisado.Checked
                .Central_Num_Zonas = DbnullToNothing(Me.T_Num_Zonas.Value)
                .Central_Num_Zonas_Placa = DbnullToNothing(Me.T_Num_Zonas_Placa.Value)
                .Central_Num_Zonas_Inalambricas = DbnullToNothing(Me.T_Num_Zonas_Inalambricas.Value)
                .Central_Num_Zonas_Inalambricas_Placa = DbnullToNothing(Me.T_Num_Zonas_Inalambricas_Placa.Value)
                .Expansor_Num_Elementos = DbnullToNothing(Me.T_Expansor_Elementos.Value)
                .Modulo_Rele_Num_Elementos = DbnullToNothing(Me.T_Modulo_Rele_Elementos.Value)
                .Numero_Aberturas = DbnullToNothing(Me.T_Num_Aberturas.Value)
                .Numero_Zonas_Utilizadas = DbnullToNothing(Me.T_Num_Zonas_Utilitzadas.Value)
                .Central_Num_Elementos_Max_Bus = DbnullToNothing(Me.T_Num_Elementos_Bus.Value)
                .ControlPenetracion = Me.CH_ControlPenetracion.Checked
                .ConectadoBus = Me.CH_ElementoConectadoABus.Checked
                .MarcaEspecificada = Me.CH_MarcaEspecificada.Checked
                .TiempoInstalacion = Me.T_TiempoInstalacion.Value


                .PVP = DbnullToNothing(Me.T_PVP.Value)
                .PVD = DbnullToNothing(Me.T_PVD.Value)
                .PlazoEntrega = DbnullToNothing(Me.T_PlazoEntrega.Value)
                .PVP_Proveedor_Predeterminado = Me.CH_PVP_Proveedor_Predeterminado.Checked

                .VidaUtil = DbnullToNothing(Me.T_VidaUtil.Value)
                .StockMinimo = DbnullToNothing(Me.T_StockMinimo.Value)
                .StockMaximo = DbnullToNothing(Me.T_StockMaximo.Value)

                .Observaciones = Me.R_Observaciones.pText
                .DescripcionAmpliada = Me.R_DescripcionAmpliada.pText
                .DescripcionAmpliada_Tecnica = Me.R_DescripcionAmpliada_Tecnica.pText

                'CCTV
                .CCTV_NumeroCanales = DbnullToNothing(Me.T_CCTV_NumCanales.Value)
                .CCTV_NumeroCanalesMaximos = DbnullToNothing(Me.T_CCTV_NumCanalesMaximos.Value)
                .CCTV_NumCanalesUsa = DbnullToNothing(Me.T_CCTV_NumCanalesUsados.Value)
                .CCTV_NumeroDiscoDurosSoportados = DbnullToNothing(Me.T_CCTV_NumDiscoDuros.Value)
                .CCTV_NumeroMonitores = DbnullToNothing(Me.T_CCTV_NumMaximoMonitores.Value)

                .ID_Producto_ClasePOE = Me.C_CCTV_ClasePOE.Value
                .ID_Producto_EstandardNema = Me.C_CCTV_EstandardNEMA.Value
                .ID_Producto_Luminosidad = Me.C_CCTV_Luminosidad.Value
                .ID_Producto_TipoRosca = Me.C_CCTV_TipoRosca.Value
                .ID_Producto_Voltaje = Me.C_CCTV_Voltage.Value

                .CCTV_Carcasa = Me.CH_CCTV_Carcasa.Checked
                .CCTV_ConexionBNC = Me.CH_CCTV_ConexionBNC.Checked
                .CCTV_ConexionUTP = Me.CH_CCTV_ConexionUTP.Checked
                .CCTV_DiscoDuro = Me.CH_CCTV_DiscoDuro.Checked
                .CCTV_ElementoCaptacion = Me.CH_CCTV_ElementoCaptacion.Checked
                .CCTV_ElementoDistribucion = Me.CH_CCTV_ElementoDistribucion.Checked
                .CCTV_ElementoGrabacion = Me.CH_CCTV_ElementoGrabacion.Checked
                .CCTV_FuenteAlimentacion = Me.CH_CCTV_FuenteAlimentacion.Checked
                .CCTV_Monitor = Me.CH_CCTV_Monitor.Checked
                .CCTV_NoIncluyeDispositivoAlmacenamiento = Me.CH_CCTV_NoIncluyeDispositivoAlmacenamiento.Checked
                .CCTV_Optica = Me.CH_CCTV_Optica.Checked
                .CCTV_POE = Me.CH_CCTV_POE.Checked
                .CCTV_PTZ = Me.CH_CCTV_PTZ.Checked
                .CCTV_RequiereAlimentacion = Me.CH_CCTV_RequiereAlimentacion.Checked
                .CCTV_Retenedor = Me.CH_CCTV_Retenedor.Checked
                .CCTV_ServidorVideoBNC = Me.CH_CCTV_ServidorVideo.Checked
                .CCTV_Teclado = Me.CH_CCTV_Teclado.Checked

                'Incendios
                .Inc_NumeroCentralesVinculadas = DbnullToNothing(Me.T_INC_CentralesVinculadas.Value)
                .Inc_ElementosPorLazo = DbnullToNothing(Me.T_INC_ElementosPorLazo.Value)
                .Inc_NumeroLazos = DbnullToNothing(Me.T_INC_NumLazos.Value)

                .ID_Producto_FrecuenciaInalambrica = Me.C_INC_FrecuenciasInalambricas.Value
                .ID_Producto_TipoDetector = Me.C_INC_TipoDetector.Value

                .Inc_Aislador = Me.CH_INC_Aislador.Checked
                .Inc_Analogico = Me.CH_INC_Analogico.Checked
                .Inc_Base = Me.CH_INC_Base.Checked
                .Inc_Baterias = Me.CH_INC_Baterias.Checked
                .Inc_Cable = Me.CH_INC_Cable.Checked
                .Inc_Central = Me.CH_INC_Central.Checked
                .Inc_Convencional = Me.CH_INC_Convencional.Checked
                .Inc_Detector = Me.CH_INC_Detector.Checked
                .Inc_ElementoComunicacion = Me.CH_INC_ElementoComunicacion.Checked
                .Inc_Inalambrico = Me.CH_INC_Inalambrico.Checked
                .Inc_Luminoso = Me.CH_INC_Luminoso.Checked
                .Inc_Pulsador = Me.CH_INC_Pulsador.Checked
                .Inc_Relees = Me.CH_INC_Relees.Checked
                .Inc_Base = Me.CH_INC_RequiereBase.Checked
                .Inc_Sirena = Me.CH_INC_Sirena.Checked
                .Inc_Retenedor = Me.CH_INC_Retenedor.Checked

                .Obsoleto = Me.CH_Obsoleto.Checked

                .Comercial = Me.CH_Comercial.Checked
                .Produccion = Me.CH_Produccion.Checked

                .Peso = DbnullToNothing(Me.T_Peso.Value)

                'Accesos
                .Acceso_NumeroElementosCubre = DbnullToNothing(Me.T_Accesos_NumElementosCubre.Value)
                .ID_Producto_TipoCerradura = Me.C_Acceso_Cerradura.Value
                .ID_Producto_TipoLector = Me.C_Acceso_Lector.Value
                .Acceso_Cerradura = Me.CH_Acceso_Cerradura.Checked
                .Acceso_Lector = Me.CH_Acceso_Lector.Checked

                If Me.C_Certificado_ClaseA.Value = 0 Then
                    .ID_Archivo_CertificadoClaseA = Nothing
                Else
                    .ID_Archivo_CertificadoClaseA = Me.C_Certificado_ClaseA.Value
                End If

                If Me.C_Certificado_Grado.Value = 0 Then
                    .ID_Archivo_CertificadoGrado = Nothing
                Else
                    .ID_Archivo_CertificadoGrado = Me.C_Certificado_Grado.Value
                End If

                If Me.C_FichaProducto.Value = 0 Then
                    .ID_Archivo_FichaTecnica = Nothing
                Else
                    .ID_Archivo_FichaTecnica = Me.C_FichaProducto.Value
                End If
                .RequiereNumeroSerie = Me.CH_RequiereNS.Checked

                .ID_Producto_TipoCalculoPrecio = CInt(Me.C_TipoCalculoPrecio.Value)

                .EsBono = Me.CH_Bono.Checked
                .Bono_Cantidad = DbnullToNothing(Me.T_Bono_Cantidad.Value)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Cargar_Form(ByVal pID As Integer, Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        Try
            Call Netejar_Pantalla(pNoCanviarALaPestanyaGeneral)
            oLinqProducto = (From taula In oDTC.Producto Where taula.ID_Producto = pID Select taula).First
            Call SetToForm()

            'Call Carga_Grid_Caracteristicas_Personalizadas(pID)
            'Call Carga_Grid_Caracteristicas_Instalacion(pID)
            'Call Carga_Grid_Precios(pID)
            'Call CargaGrid_Uso(pID)
            'Call CargaGrid_UsoPresupuestado(pID)
            'Call CargaGrid_Partes(pID)
            'Call CargaGrid_ProductoAlternativo(pID)
            'Call CargaGrid_ProductoRequerido(pID)
            Call Carga_Grid_Idiomas(pID)
            Fichero.Cargar_GRID(pID)



            'Fem això pq un producte guardat no es pot canviar la divisió
            Me.C_Division.ReadOnly = True
            Me.C_Familia.ReadOnly = True
            Call CanviarTitolPantalla()

            Util.Tab_Activar(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.TODOS)
            Me.B_Informe.Enabled = True 'habilitem el botó d'informe només quan s'hagin carregat les dades
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub Netejar_Pantalla(Optional ByVal pNoCanviarALaPestanyaGeneral As Boolean = False)
        oLinqProducto = New Producto
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        Fichero.Cargar_GRID(0)

        Me.T_Num_Aberturas.Value = Nothing
        Me.T_Num_Zonas.Value = Nothing
        Me.T_Num_Zonas_Placa.Value = Nothing
        Me.T_Num_Zonas_Inalambricas.Value = Nothing
        Me.T_Num_Zonas_Inalambricas_Placa.Value = Nothing
        Me.T_Num_Elementos_Bus.Value = Nothing
        Me.T_Expansor_Elementos.Value = Nothing
        Me.T_Modulo_Rele_Elementos.Value = Nothing
        Me.T_Num_Zonas_Utilitzadas.Value = Nothing

        Me.TE_Codigo.Value = Nothing

        Me.DT_Alta.Value = Nothing
        Me.DT_Baja.Value = Nothing

        'Me.T_Num_Aberturas.Value = 1   'Demanat pel domingo per defecte

        Me.C_Familia.SelectedIndex = -1
        Me.C_Subfamilia.SelectedIndex = -1
        Me.C_Marca.SelectedIndex = -1
        Me.C_Garantia.SelectedIndex = 0
        Me.C_Grado.SelectedIndex = 0
        Me.C_Sistema_Trasmision.SelectedIndex = 0
        Me.C_Sistema_Trasmision2.SelectedIndex = 0
        Me.C_ATS.SelectedIndex = 0
        Me.C_ATS2.SelectedIndex = 0
        Me.C_Tipo_Fuente_Alimentacion.SelectedIndex = 0
        Me.C_Tipo_Sirena.SelectedIndex = 0
        Me.C_Frecuencia.SelectedIndex = 0
        Me.C_Clase_Ambiental.SelectedIndex = 0

        Me.C_CCTV_ClasePOE.SelectedIndex = 0
        Me.C_CCTV_EstandardNEMA.SelectedIndex = 0
        Me.C_CCTV_Luminosidad.SelectedIndex = 0
        Me.C_CCTV_TipoRosca.SelectedIndex = 0
        Me.C_CCTV_Voltage.SelectedIndex = 0

        Me.C_INC_FrecuenciasInalambricas.SelectedIndex = 0
        Me.C_INC_TipoDetector.SelectedIndex = 0

        Me.C_Acceso_Cerradura.SelectedIndex = 0
        Me.C_Acceso_Lector.SelectedIndex = 0

        Me.C_Division.ReadOnly = False
        Me.C_Familia.ReadOnly = True
        Me.C_Subfamilia.ReadOnly = True
        Me.C_Marca.ReadOnly = True

        Me.CH_Produccion.Checked = True

        Me.C_Division.Value = Nothing

        Me.EstableixCaptionForm("Producto")
        Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"

        ErrorProvider1.Clear()

        If pNoCanviarALaPestanyaGeneral = False Then
            Me.Tab_Principal.Tabs("General").Selected = True
        End If
        Util.Tab_Desactivar_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "General")

        Me.UltraPictureBox1.Image = Nothing

        Me.B_Informe.Enabled = False 'deshabilitem el botó d'informe quan no hi hagi un producte carregat

        Me.GRD_Caracteristicas_Instalacion.GRID.DataSource = Nothing
        Me.GRD_Caracteristicas_Personalizadas.GRID.DataSource = Nothing
        Me.GRD_Partes.GRID.DataSource = Nothing
        Me.GRD_Precios.GRID.DataSource = Nothing
        Me.GRD_ProductoAlternativo.GRID.DataSource = Nothing
        Me.GRD_ProductoRequerido.GRID.DataSource = Nothing
        Me.GRD_Uso.GRID.DataSource = Nothing
        Me.GRD_Uso_Presupuestado.GRID.DataSource = Nothing
        Me.GRD_Idioma.GRID.DataSource = Nothing

        Me.C_Tipo_Documento.Value = Nothing
        Me.GRD_Step.GRID.DataSource = Nothing

        Me.C_TipoCalculoPrecio.SelectedIndex = 0
    End Sub

    Private Function ComprovacioCampsRequeritsError() As Boolean
        Try
            Dim oClsControls As New clsControles(ErrorProvider1)

            With Me
                ErrorProvider1.Clear()
                oClsControls.ControlBuit(.TE_Codigo)
                oClsControls.ControlBuit(.T_Descripcion)
                oClsControls.ControlBuit(.C_Division)
                oClsControls.ControlBuit(.C_Familia)
                oClsControls.ControlBuit(.C_Subfamilia)
                oClsControls.ControlBuit(.C_Marca)
                oClsControls.ControlBuit(.C_Grado)
                oClsControls.ControlBuit(.C_Clase_Ambiental)

                'oClsControls.ControlBuit(.T_NIF)
                'oClsControls.ControlBuit(.DT_Data_Alta)
                'oClsControls.ControlBuit(.T_Poblacio)
                'oClsControls.ControlBuit(.C_VOTS)
                'oClsControls.ControlBuit(.C_Forma_Juridica)
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
        LlistatGeneric.Mostrar_Llistat("SELECT * FROM C_Producto ORDER BY Descripcion", Me.TE_Codigo, "ID_Producto", "ID_Producto")
        AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat

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

        'LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = True
        'LlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Caption = "Visualizar fotos"
        'AddHandler LlistatGeneric.m_UltraGrid_AfterRefresh, AddressOf Grid_AfterRefresh
        'AddHandler LlistatGeneric.AlApretarElBotoAuxiliar, AddressOf Grid_AlApretarElBotoAuxiliar
    End Sub

    'Private Sub Grid_AfterRefresh(ByRef pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)

    'End Sub

    'Private Sub Grid_AlApretarElBotoAuxiliar(ByRef pInstanciaLlistatGeneric As M_LlistatGeneric.clsLlistatGeneric)
    '    'Dim _Producto As Producto
    '    'For Each _Producto In oDTC.Producto
    '    '    If _Producto.ID_Archivo_FotoPredeterminada.HasValue Then
    '    '        Dim _ArchivoMini As New Archivo
    '    '        _ArchivoMini.Activo = True
    '    '        _ArchivoMini.CampoBinario = Util.ImageToBinary(M_Util.clsFunciones.CompressImage(M_Util.clsFunciones.ResizeImage(Util.BinaryToImage(_Producto.Archivo.CampoBinario.ToArray), New System.Drawing.Size(40, 40), True))).ToArray
    '    '        _ArchivoMini.Descripcion = "Foto Mini ID=" & _Producto.ID_Producto
    '    '        _ArchivoMini.Ruta_Fichero = _Producto.Archivo.Ruta_Fichero
    '    '        _ArchivoMini.Tipo = _Producto.Archivo.Tipo
    '    '        ' _ArchivoMini.Ruta_Fichero
    '    '        _ArchivoMini.Fecha = Now.Date

    '    '        oDTC.Archivo.InsertOnSubmit(_ArchivoMini)
    '    '        _Producto.Archivo_Mini = _ArchivoMini
    '    '    End If
    '    'Next
    '    'oDTC.SubmitChanges()
    '    pInstanciaLlistatGeneric.pGrid.M.clsUltraGrid.Cargar("SELECT * FROM C_Producto ORDER BY Descripcion", BD)
    '    pInstanciaLlistatGeneric.AplicarCanvisBotoAuxiliarAlNouGrid()
    '    pInstanciaLlistatGeneric.Formulari.BotoAuxiliar.SharedProps.Visible = False

    '    With pInstanciaLlistatGeneric.pGrid.GRID
    '        Dim _pRow As UltraGridRow
    '        Dim _Contador As Integer = 0
    '        For Each _pRow In .Rows
    '            ' Dim _Producto As Producto
    '            ' _Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(_pRow.Cells("id_producto").Value)).FirstOrDefault
    '            ' If _Producto.Archivo_Mini Is Nothing = False AndAlso _Producto.Archivo_Mini.CampoBinario.Length > 0 Then
    '            '     _pRow.Cells("Foto").Appearance.Image = Util.BinaryToImage(_Producto.Archivo_Mini.CampoBinario.ToArray)
    '            If _pRow.Cells("Foto").Value.ToString.Length > 0 Then
    '                Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(DirectCast(_pRow.Cells("Foto").Value, Byte()))

    '                _pRow.Cells("Foto").Appearance.Image = M_Util.clsFunciones.ResizeImage(Image.FromStream(ms), New System.Drawing.Size(40, 40), True)
    '                _pRow.Cells("Foto").Value = Nothing
    '                ms.Dispose()
    '                ms = Nothing
    '            End If
    '            '  End If
    '            '_Contador = _Contador + 1
    '            'If _Contador = 500 Then
    '            '    Exit For
    '            'End If
    '        Next
    '    End With
    'End Sub

    Private Sub AlTancarLlistat(ByVal pID As String)
        Try
            Call Cargar_Form(pID)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarFoto()
        If oLinqProducto.ID_Archivo_FotoPredeterminada.HasValue = True Then
            If oLinqProducto.Archivo.CampoBinario Is Nothing = False Then
                Me.UltraPictureBox1.Image = Util.BinaryToImage(oLinqProducto.Archivo.CampoBinario.ToArray)

            End If
            ' Me.UltraPictureBox1.Image = Image.FromFile(Fichero.ExtreuIRetornaRutaArxiu(oLinqProducto.ID_Archivo_FotoPredeterminada))
        Else
            Me.UltraPictureBox1.Image = Nothing
        End If

        'Me.UltraPictureBox1.Image = Util.BinaryToImage(oDTC.PlanoBinario.Where(Function(F) F.ID_PlanoBinario = 21).FirstOrDefault.Foto.ToArray)
        'Dim juan As Image
        'juan = Me.UltraPictureBox1.Image
        'Me.UltraPictureBox1.
        'juan.Save("c:\pgm\juan.jpg", Imaging.ImageFormat.Jpeg)
        'juan.Save("c:\pgm\juan2.jpg", Imaging.ImageFormat.Bmp)
    End Sub

    Private Sub DespresDeCarregarDadesGridArchivos()
        Try
            If Me.GRD_Ficheros.GRID.Rows.Count > 0 Then
                Dim pRow As UltraGridRow
                For Each pRow In Me.GRD_Ficheros.GRID.Rows
                    If oLinqProducto.ID_Archivo_FotoPredeterminada.HasValue = True AndAlso pRow.Cells("ID_Archivo").Value = oLinqProducto.ID_Archivo_FotoPredeterminada Then
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
        If pIDArchivo = oLinqProducto.ID_Archivo_FotoPredeterminada Then
            oLinqProducto.ID_Archivo_FotoPredeterminada = Nothing
            oDTC.SubmitChanges()
        End If
    End Sub

    Private Sub CopiarCaracteristicasPersonalitzades()
        Try
            Dim _Carac As Producto_Caracteristica

            For Each _Carac In oDTC.Producto_Caracteristica.Where(Function(F) F.Predeterminado = True And F.Activo = True And F.ID_Producto_Familia = CInt(Me.C_Familia.Value))
                If oLinqProducto.Producto_Producto_Caracteristica.Where(Function(F) F.ID_Producto_Caracteristica = _Carac.ID_Producto_Caracteristica).Count = 0 Then
                    Dim _newCarac As New Producto_Producto_Caracteristica
                    _newCarac.ID_Producto_Caracteristica = _Carac.ID_Producto_Caracteristica
                    _newCarac.Valor = " "
                    oLinqProducto.Producto_Producto_Caracteristica.Add(_newCarac)
                End If
            Next
            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CopiarCaracteristicasInstalacion()
        Try
            Dim _Carac As Producto_Caracteristica_Instalacion

            For Each _Carac In oDTC.Producto_Caracteristica_Instalacion.Where(Function(F) F.Predeterminado = True And F.Activo = True And F.ID_Producto_Familia = CInt(Me.C_Familia.Value))
                If oLinqProducto.Producto_Producto_Caracteristica_Instalacion.Where(Function(F) F.ID_Producto_Caracteristica_Instalacion = _Carac.ID_Producto_Caracteristica_Instalacion).Count = 0 Then
                    Dim _newCarac As New Producto_Producto_Caracteristica_Instalacion
                    _newCarac.ID_Producto_Caracteristica_Instalacion = _Carac.ID_Producto_Caracteristica_Instalacion
                    _newCarac.Valor = " "
                    oLinqProducto.Producto_Producto_Caracteristica_Instalacion.Add(_newCarac)
                End If
            Next
            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CopiarMantenimientosPlanificados()
        Try
            Dim _Carac As Producto_Mantenimiento

            For Each _Carac In oDTC.Producto_Mantenimiento.Where(Function(F) F.Predeterminado = True And F.Activo = True And F.ID_Producto_Familia = CInt(Me.C_Familia.Value))
                If oLinqProducto.Producto_Producto_Mantenimiento.Where(Function(F) F.ID_Producto_Mantenimiento = _Carac.ID_Producto_Mantenimiento).Count = 0 Then
                    Dim _newCarac As New Producto_Producto_Mantenimiento
                    _newCarac.Producto_Mantenimiento = _Carac
                    _newCarac.Producto_Caracteristica_Vision = oDTC.Producto_Caracteristica_Vision.Where(Function(F) F.ID_Producto_Caracteristica_Vision = CInt(EnumProductoCaracteristicaVision.Revision)).FirstOrDefault
                    If _Carac.Valor Is Nothing Then
                        _newCarac.Valor = " "
                    Else
                        _newCarac.Valor = _Carac.Valor
                    End If

                    _newCarac.Tiempo = Util.Comprobar_NULL_Per_0(_Carac.Tiempo)
                    oLinqProducto.Producto_Producto_Mantenimiento.Add(_newCarac)
                End If
            Next
            oDTC.SubmitChanges()
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CanviarTitolPantalla()
        Me.EstableixCaptionForm("Producto: " & (oLinqProducto.Codigo) & " - " & oLinqProducto.Producto_Marca.Descripcion)
    End Sub

    Private Sub AvanzarRetroceder(ByVal pAvanzar As Boolean)
        'Que pasa si no s'ha seleccinat cap article?

        Dim _IDATrobar As Integer
        Dim _Trobat As Boolean = False

        If oLinqProducto.ID_Producto = 0 Then
            If Me.OP_Filtre.Value = "Cliente" Then
                Exit Sub
            Else
                'Si actualment no tenim cap registre carregat farem veure com si estiguessim al últim.
                _IDATrobar = oDTC.Producto.Where(Function(F) F.Activo = True).Max(Function(F) F.ID_Producto)
                _Trobat = True

            End If
        Else
            _IDATrobar = oLinqProducto.ID_Producto
        End If

        Dim _LlistatProductos As IList(Of Producto)
        Select Case Me.OP_Filtre.Value
            Case "Codigo"
                If pAvanzar = True Then
                    _LlistatProductos = oDTC.Producto.Where(Function(F) F.Activo = True).OrderBy(Function(F) F.ID_Producto).ToList
                Else
                    _LlistatProductos = oDTC.Producto.Where(Function(F) F.Activo = True).OrderByDescending(Function(F) F.ID_Producto).ToList
                End If
            Case "Marca"
                If pAvanzar = True Then
                    _LlistatProductos = oDTC.Producto.Where(Function(F) F.Activo = True And F.ID_Producto_Marca = oLinqProducto.ID_Producto_Marca).OrderBy(Function(F) F.ID_Producto).ToList
                Else
                    _LlistatProductos = oDTC.Producto.Where(Function(F) F.Activo = True And F.ID_Producto_Marca = oLinqProducto.ID_Producto_Marca).OrderByDescending(Function(F) F.ID_Producto).ToList
                End If
            Case "Familia"
                If pAvanzar = True Then
                    _LlistatProductos = oDTC.Producto.Where(Function(F) F.Activo = True And F.ID_Producto_Familia = oLinqProducto.ID_Producto_Familia).OrderBy(Function(F) F.ID_Producto).ToList
                Else
                    _LlistatProductos = oDTC.Producto.Where(Function(F) F.Activo = True And F.ID_Producto_Familia = oLinqProducto.ID_Producto_Familia).OrderByDescending(Function(F) F.ID_Producto).ToList
                End If
        End Select


        Dim _Producto As Producto
        Dim _ProductoSeguent As Producto
        For Each _Producto In _LlistatProductos
            If _Trobat = True Then
                _ProductoSeguent = _Producto
                Exit For
            End If
            If _Producto.ID_Producto = _IDATrobar Then
                _Trobat = True
            End If
        Next
        If _ProductoSeguent Is Nothing = False Then
            Dim _TabActual As String = Me.Tab_Principal.SelectedTab.Key
            Call Cargar_Form(_ProductoSeguent.ID_Producto, True)
            Call CarregarDadesPestanyes(_TabActual)
        End If
    End Sub

    Private Sub CarregarDadesPestanyes(ByVal pKeyPestanya As String)
        Try
            Dim ID As Integer
            If oLinqProducto Is Nothing = False Then
                ID = oLinqProducto.ID_Producto
            End If

            Select Case Me.Tab_Principal.SelectedTab.Key
                Case "General"
                    Call CargarFoto()
                Case "CaracteristicasPersonalizadas"
                    Call Carga_Grid_Caracteristicas_Personalizadas(ID)
                Case "CaracteristicasInstalacion"
                    Call Carga_Grid_Caracteristicas_Instalacion(ID)
                Case "Precios"
                    Call Carga_Grid_Precios(ID)
                    Call CargaGrid_PreciosDeCompra(ID)
                Case "ProductoRequerido"
                    Call CargaGrid_ProductoRequerido(ID)
                Case "ProductoAlternativo"
                    Call CargaGrid_ProductoAlternativo(ID)
                Case "Uso"
                    Call CargaGrid_Uso(ID)
                Case "Uso_Presupuestado"
                    Call CargaGrid_UsoPresupuestado(ID)
                Case "Partes"
                    Call CargaGrid_Partes(ID)
                Case "Stock"
                    Call CargaGrid_Stock(ID)
                    Call CargaGrid_StockNS(ID)
                Case "MantenimientosPlanificados"
                    Call Carga_Grid_Mantenimientos_Planificados(ID)
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Caracteristicas Personalizadas"

    Private Sub Carga_Grid_Caracteristicas_Personalizadas(ByVal pId As Integer)
        Try

            With Me.GRD_Caracteristicas_Personalizadas

                Dim _Caracteristicas As IEnumerable(Of Producto_Producto_Caracteristica) = From taula In oDTC.Producto_Producto_Caracteristica Where taula.ID_Producto = pId Select taula
                '.GRID.DataSource = _Caracteristicas
                .M.clsUltraGrid.CargarIEnumerable(_Caracteristicas)

                .M_Editable()

                Call CargarComboCaracteristica()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboCaracteristica()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oCaracteristicas As IQueryable(Of Producto_Caracteristica) = (From Taula In oDTC.Producto_Caracteristica Where Taula.Activo = True And Taula.ID_Producto_Familia = CInt(Me.C_Familia.Value) Order By Taula.Descripcion Select Taula)
            Dim var As Producto_Caracteristica

            ' Valors.ValueListItems.Add("-1", "Selecciona una característica")
            For Each var In oCaracteristicas
                Valors.ValueListItems.Add(var, var.Descripcion)
            Next

            Me.GRD_Caracteristicas_Personalizadas.GRID.DisplayLayout.Bands(0).Columns("Producto_Caracteristica").Style = ColumnStyle.DropDownList
            Me.GRD_Caracteristicas_Personalizadas.GRID.DisplayLayout.Bands(0).Columns("Producto_Caracteristica").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Caracteristicas_Personalizadas_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Caracteristicas_Personalizadas.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Caracteristicas_Personalizadas

                If oLinqProducto.ID_Producto = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto").Value = oLinqProducto
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Imprimible").Value = True

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Caracteristicas_Personalizadas_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Caracteristicas_Personalizadas.M_ToolGrid_ToolClickBotonsExtras
        Try
            If e.Tool.Key = "CaracteristicasPredeterminadas" Then
                If oLinqProducto.ID_Producto = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Call CopiarCaracteristicasPersonalitzades()

                Call Carga_Grid_Caracteristicas_Personalizadas(oLinqProducto.ID_Producto)
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub GRD_M_GRID_BeforeCellListDropDown(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles GRD.M_GRID_BeforeCellListDropDown
    '    If e.Cell.Row.Cells("ID_Associat_Participacio_Reunio").Value <> 0 Then
    '        e.Cancel = True
    '    End If
    'End Sub

    'Private Sub GRD_Peticionaris_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD.M_ToolGrid_ToolAfegir
    '    Try

    '        If oLinqReunio.ID_Participacio_Reunio = 0 Then
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
    '            Exit Sub
    '        End If

    '        Me.GRD.GRID.DisplayLayout.Bands(0).AddNew() 'Afegim una Row al Grid
    '        Me.GRD.GRID.Rows(Me.GRD.GRID.Rows.Count - 1).Cells("ID_Associat").Value = (From Taula In oDTC.Associats Where Taula.Activo = True Select Taula.ID_Associat).FirstOrDefault

    '        'Establim que la variable Linia és el mateix que la última row del grid recient afegida
    '        Dim Linia As New Associat_Participacio_Reunio
    '        Linia = Me.GRD.GRID.Rows.GetItem(Me.GRD.GRID.Rows.Count - 1).listObject

    '        'Afegim aquesta línia a la colecció de línies del actual albarà
    '        oLinqReunio.Associat_Participacio_Reunio.Add(Linia)

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    Private Sub GRD_Caracteristicas_Personalizadas_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Caracteristicas_Personalizadas.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
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

    'Private Sub BeforeRowUpdate(ByVal Sender As Object, ByVal e As CancelableRowEventArgs)
    '    Dim valors As Infragistics.Win.ValueList = e.Row.Cells("ID_Associat").ValueListResolved
    '    Dim IDAssociat As Integer = e.Row.Cells("ID_Associat").Value
    '    Dim Trobats As Integer = (From taula In oLinqReunio.Associat_Participacio_Reunio Where taula.ID_Associat = IDAssociat And taula.ID_Participacio_Reunio = oLinqReunio.ID_Participacio_Reunio Select taula).Count
    '    If Trobats > 1 Then
    '        Mensaje.Mostrar_Mensaje("No es pot seleccionar dos vegades el mateix associat", M_Mensaje.Missatge_Modo.INFORMACIO, "", , True)
    '        If e.Row.Cells("ID_Associat").OriginalValue <> 0 Then
    '            e.Row.Cells("ID_Associat").Value = e.Row.Cells("ID_Associat").OriginalValue
    '        Else
    '            Dim Linea As Associat_Participacio_Reunio = oLinqReunio.Associat_Participacio_Reunio(e.Row.ListIndex)
    '            oLinqReunio.Associat_Participacio_Reunio.Remove(Linea)
    '        End If
    '        e.Row.CancelUpdate()
    '    End If
    'End Sub

#End Region

#Region "Caracteristicas Instalacion"

    Private Sub Carga_Grid_Caracteristicas_Instalacion(ByVal pId As Integer)
        Try
            With Me.GRD_Caracteristicas_Instalacion
                Dim _Caracteristicas As IEnumerable(Of Producto_Producto_Caracteristica_Instalacion) = From taula In oDTC.Producto_Producto_Caracteristica_Instalacion Where taula.ID_Producto = pId Select taula
                '.GRID.DataSource = _Caracteristicas
                .M.clsUltraGrid.CargarIEnumerable(_Caracteristicas)

                .M_Editable()

                Call CargarComboCaracteristicaInstalacion()
                Call CargarComboVision()
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboCaracteristicaInstalacion()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oCaracteristicas As IQueryable(Of Producto_Caracteristica_Instalacion) = (From Taula In oDTC.Producto_Caracteristica_Instalacion Where Taula.Activo = True And Taula.ID_Producto_Familia = CInt(Me.C_Familia.Value) Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Caracteristica_Instalacion

            'Valors.ValueListItems.Add("-1", "Selecciona una característica")
            For Each Var In oCaracteristicas
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            Me.GRD_Caracteristicas_Instalacion.GRID.DisplayLayout.Bands(0).Columns("Producto_Caracteristica_Instalacion").Style = ColumnStyle.DropDownList
            Me.GRD_Caracteristicas_Instalacion.GRID.DisplayLayout.Bands(0).Columns("Producto_Caracteristica_Instalacion").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarComboVision()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oVision As IQueryable(Of Producto_Caracteristica_Vision) = (From Taula In oDTC.Producto_Caracteristica_Vision Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Caracteristica_Vision

            For Each Var In oVision
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            Me.GRD_Caracteristicas_Instalacion.GRID.DisplayLayout.Bands(0).Columns("Producto_Caracteristica_Vision").Style = ColumnStyle.DropDownList
            Me.GRD_Caracteristicas_Instalacion.GRID.DisplayLayout.Bands(0).Columns("Producto_Caracteristica_Vision").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_Caracteristicas_Instalacion_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Caracteristicas_Instalacion.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Caracteristicas_Instalacion
                If oLinqProducto.ID_Producto = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Call CargarComboCaracteristicaInstalacion()

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto").Value = oLinqProducto
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Imprimible").Value = True

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Caracteristicas_Instalacion_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Caracteristicas_Instalacion.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim Linea As Producto_Producto_Caracteristica_Instalacion = e.ListObject
                If Linea.Parte_Revision.Count > 0 Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar. Ésta característica está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Caracteristicas_Instalacion_Personalizadas_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Caracteristicas_Instalacion.M_ToolGrid_ToolClickBotonsExtras
        Try
            If e.Tool.Key = "CaracteristicasPredeterminadas" Then
                If oLinqProducto.ID_Producto = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Call CopiarCaracteristicasInstalacion()

                Call Carga_Grid_Caracteristicas_Instalacion(oLinqProducto.ID_Producto)
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Mantenimientos planificados"

    Private Sub Carga_Grid_Mantenimientos_Planificados(ByVal pId As Integer)
        Try
            With Me.GRD_MantenimientosPlanificados
                Dim _MantenimientosPlanificados As IEnumerable(Of Producto_Producto_Mantenimiento) = From taula In oDTC.Producto_Producto_Mantenimiento Where taula.ID_Producto = pId Select taula Order By taula.Tiempo Descending
                '.GRID.DataSource = _Caracteristicas
                .M.clsUltraGrid.CargarIEnumerable(_MantenimientosPlanificados)

                .M_Editable()

                Call CargarComboMantenimientos()
                Call CargarComboVisionMantenimientosPlanificados()
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarComboMantenimientos()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oCaracteristicas As IQueryable(Of Producto_Mantenimiento) = (From Taula In oDTC.Producto_Mantenimiento Where Taula.Activo = True And Taula.ID_Producto_Familia = CInt(Me.C_Familia.Value) Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Mantenimiento

            'Valors.ValueListItems.Add("-1", "Selecciona una característica")
            For Each Var In oCaracteristicas
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            Me.GRD_MantenimientosPlanificados.GRID.DisplayLayout.Bands(0).Columns("Producto_Mantenimiento").Style = ColumnStyle.DropDownList
            Me.GRD_MantenimientosPlanificados.GRID.DisplayLayout.Bands(0).Columns("Producto_Mantenimiento").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub CargarComboVisionMantenimientosPlanificados()
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oVision As IQueryable(Of Producto_Caracteristica_Vision) = (From Taula In oDTC.Producto_Caracteristica_Vision Where Taula.Activo = True Order By Taula.Descripcion Select Taula)
            Dim Var As Producto_Caracteristica_Vision

            For Each Var In oVision
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            Me.GRD_MantenimientosPlanificados.GRID.DisplayLayout.Bands(0).Columns("Producto_Caracteristica_Vision").Style = ColumnStyle.DropDownList
            Me.GRD_MantenimientosPlanificados.GRID.DisplayLayout.Bands(0).Columns("Producto_Caracteristica_Vision").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_MantenimientosPlanificados_M_GRID_AfterRowUpdate(sender As Object, e As RowEventArgs) Handles GRD_MantenimientosPlanificados.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
    End Sub

    Private Sub GRD_MantenimientosPlanificados_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_MantenimientosPlanificados.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_MantenimientosPlanificados
                If oLinqProducto.ID_Producto = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                'Call CargarComboCaracteristicaInstalacion()

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto_Caracteristica_Vision").Value = oDTC.Producto_Caracteristica_Vision.Where(Function(F) F.ID_Producto_Caracteristica_Vision = CInt(EnumProductoCaracteristicaVision.Revision)).FirstOrDefault
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto").Value = oLinqProducto
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Imprimible").Value = True

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_MantenimientosPlanificados_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_MantenimientosPlanificados.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                Dim Linea As Producto_Producto_Mantenimiento = e.ListObject
                'If Linea.Parte_Revision.Count > 0 Then
                '    Mensaje.Mostrar_Mensaje("Imposible eliminar. Ésta característica está en uso", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                '    Exit Sub
                'End If
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_MantenimientosPlanificados_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_MantenimientosPlanificados.M_ToolGrid_ToolClickBotonsExtras
        Try
            If e.Tool.Key = "Mantenimientos" Then
                If oLinqProducto.ID_Producto = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                Call CopiarMantenimientosPlanificados()

                Call Carga_Grid_Mantenimientos_Planificados(oLinqProducto.ID_Producto)
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Precios"

    Private Sub Carga_Grid_Precios(ByVal pId As Integer)
        Try
            With Me.GRD_Precios

                Dim _Producto_Proveedor As IEnumerable(Of Producto_Proveedor) = From taula In oDTC.Producto_Proveedor Where taula.ID_Producto = pId Select taula
                '.GRID.DataSource = _Producto_Proveedor
                .M.clsUltraGrid.CargarIEnumerable(_Producto_Proveedor)

                ' .GRID.DisplayLayout.Bands(0).Columns("PVD").CellActivation = Activation.Disabled

                .M_Editable()

                Call CargarCombo_Proveedor(.GRID)
                Call C_TipoCalculoPrecio_ValueChanged(Nothing, Nothing)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Proveedor(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Proveedor) = (From Taula In oDTC.Proveedor Where Taula.Activo = True Order By Taula.Nombre Select Taula)
            Dim Var As Proveedor

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Nombre)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Proveedor").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Proveedor").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Precios_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Precios.M_GRID_CellListSelect
        Try
            'tot això es per aplicar descomptes assignats a la fitxa proveedor per familia. Al seleccionar el proveedor per primera vegada automàticament es possarà el descompte
            If e.Cell.Column.Key = "Proveedor" And e.Cell.Row.Cells("ID_Producto_Proveedor").Value = 0 Then
                Dim _Proveedor As Proveedor = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
                If _Proveedor.ID_Proveedor > 0 Then
                    If _Proveedor Is Nothing = False Then
                        Dim _Tarifa As Proveedor_Tarifa = _Proveedor.Proveedor_Tarifa.Where(Function(F) F.ID_Producto_Division = CInt(Me.C_Division.Value)).FirstOrDefault
                        If _Tarifa Is Nothing = False Then
                            If _Tarifa.Descuento Is Nothing Then
                                e.Cell.Row.Cells("Descuento").Value = Decimal.Add(0, 0) 'xorrada pq el 0 sigui decimal
                            Else
                                e.Cell.Row.Cells("Descuento").Value = _Tarifa.Descuento
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Precios_M_GRID_CellChange(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Precios.M_GRID_CellChange
        Try
            If e.Cell.Column.Key = "Predeterminado" Then
                If e.Cell.Value = False Then
                    Dim pRow As UltraGridRow
                    For Each pRow In Me.GRD_Precios.GRID.Rows
                        If e.Cell.Row Is pRow = False Then
                            pRow.Cells(e.Cell.Column.Key).Value = False
                            pRow.Update()
                        End If
                    Next
                End If
            End If



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Precios_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Precios.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Precios
                If oLinqProducto.ID_Producto = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto").Value = oLinqProducto
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Descuento").Value = "0"

                If .GRID.Rows.Count = 1 Then 'si hi ha una row (una te que haber segur pq l'estem afegint) llavors predeterminarem el proveedor
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("Predeterminado").Value = True
                End If

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Precios_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Precios.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                'oLinqParte.Parte_Gastos.Remove(e.ListObject)
                Exit Sub
            End If

            If sender.Rows.Count > 1 Then 'Si hi ha més d'una Row i la que estem esborrant es la predeterminada, ho impedirem
                If sender.Selected.Rows(0).Cells("Predeterminado").Value = True Then
                    Mensaje.Mostrar_Mensaje("Imposible eliminar el proveedor predeterminado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If
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

    'Private Sub GRD_Abertura_M_GRID_BeforeRowUpdate(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CancelableRowEventArgs) Handles GRD_Abertura.M_GRID_BeforeRowUpdate

    '    Dim LiniaEmplazamiento As New Instalacion_Emplazamiento
    '    If Me.GRD_Emplazamiento.GRID.ActiveRow Is Nothing = True Then ' si no s'ha seleccionat cap emplazamiento no es podrà afegir una row
    '        Exit Sub
    '    Else
    '        LiniaEmplazamiento = Me.GRD_Emplazamiento.GRID.Rows.GetItem(Me.GRD_Emplazamiento.GRID.ActiveRow.Index).listObject
    '    End If


    '    Dim Linia As New Instalacion_Emplazamiento_Abertura
    '    Linia = Me.GRD_Abertura.GRID.Rows.GetItem(e.Row.ListIndex).listObject
    '    If IsDBNull(Linia.Numerico) = False AndAlso Linia.Numerico > 0 Then
    '    Else
    '        If Linia.ID_Instalacion_Emplazamiento = 0 Then
    '            LiniaEmplazamiento.Instalacion_Emplazamiento_Abertura.Remove(Linia)
    '            e.Cancel = True
    '        Else
    '            e.Row.Cells("Numerico").Value = e.Row.Cells("Numerico").OriginalValue
    '            'e.Cancel = False
    '        End If
    '        Mensaje.Mostrar_Mensaje("El número de aberturas mínimo és de 1", M_Mensaje.Missatge_Modo.INFORMACIO, "")
    '    End If
    'End Sub

    Private Sub GRD_Precios_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Precios.M_GRID_AfterRowUpdate
        Dim _Producto_Precio As Producto_Proveedor = e.Row.ListObject

        Dim _ContadorQuantesVegadesUsatAquestProveidor As Integer = oLinqProducto.Producto_Proveedor.Where(Function(F) F.ID_Producto = _Producto_Precio.ID_Producto And F.ID_Proveedor = _Producto_Precio.ID_Proveedor And F.ID_Producto_Proveedor <> _Producto_Precio.ID_Producto_Proveedor).Count
        'If _Producto_Precio.ID_Producto_Proveedor = 0 Then 'si s'esta insertant
        If _ContadorQuantesVegadesUsatAquestProveidor = 1 Then
            Mensaje.Mostrar_Mensaje("Imposible seleccionar dos veces el mismo proveedor", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            e.Row.CancelUpdate()

            If _Producto_Precio.ID_Producto_Proveedor = 0 Then
                e.Row.Delete(False)
            Else
                e.Row.Cells("Proveedor").Value = e.Row.Cells("Proveedor").OriginalValue
            End If
        End If
        'Else
        'If _ContadorQuantesVegadesUsatAquestProveidor = 0 Then
        '    Mensaje.Mostrar_Mensaje("Imposible seleccionar dos veces el mismo proveedor", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
        '    e.Row.CancelUpdate()
        '    e.Row.Delete(True)
        'End If
        'End If

        oDTC.SubmitChanges()

        Dim _ProductoPrecio As Producto_Proveedor = oLinqProducto.Producto_Proveedor.Where(Function(F) F.Predeterminado = True).FirstOrDefault
        If _ProductoPrecio Is Nothing = False Then
            Me.CH_PVP_Proveedor_Predeterminado.Checked = True
            Me.T_PVP.Value = _ProductoPrecio.PVP
            Me.T_PVD.Value = _ProductoPrecio.PVD
            Me.T_PlazoEntrega.Value = _ProductoPrecio.PlazoEntrega
        Else
            'Me.CH_PVP_Proveedor_Predeterminado.Checked = False
        End If
    End Sub

    Private Sub GRD_Precios_M_Grid_InitializeRow(Sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles GRD_Precios.M_Grid_InitializeRow
        'If e.Row.Cells("Descuento").Value = 0 Then
        '    e.Row.Cells("PVD").Activation = Activation.AllowEdit
        'Else
        '    e.Row.Cells("PVD").Activation = Activation.Disabled
        'End If

        If Me.C_TipoCalculoPrecio.Value = 0 Then
            If IsDBNull(e.Row.Cells("PVP").Value) = True OrElse e.Row.Cells("PVP").Value = 0 Then
                e.Row.Cells("PVP").Value = 0
                e.Row.Cells("PVD").Value = CDec(0) 'si no poso el cdec peta...
            Else
                If IsDBNull(e.Row.Cells("Descuento").Value) = True OrElse e.Row.Cells("Descuento").Value = 0 Then
                    e.Row.Cells("PVD").Value = e.Row.Cells("PVP").Value
                Else
                    e.Row.Cells("PVD").Value = Math.Round(e.Row.Cells("PVP").Value - (e.Row.Cells("PVP").Value * e.Row.Cells("Descuento").Value) / 100, 2)
                End If
            End If
        Else
            If IsDBNull(e.Row.Cells("PVD").Value) = True OrElse e.Row.Cells("PVD").Value = 0 Then
                e.Row.Cells("PVP").Value = 0
                e.Row.Cells("PVD").Value = CDec(0) 'si no poso el cdec peta...
            Else
                If IsDBNull(e.Row.Cells("Descuento").Value) = True OrElse e.Row.Cells("Descuento").Value = 0 Then
                    e.Row.Cells("PVP").Value = e.Row.Cells("PVD").Value
                Else
                    e.Row.Cells("PVP").Value = Math.Round(e.Row.Cells("PVD").Value + (e.Row.Cells("PVD").Value * e.Row.Cells("Descuento").Value) / 100, 2)
                End If
            End If
        End If
    End Sub

    Private Sub CargaGrid_PreciosDeCompra(ByVal pId As Integer)
        Try

            Dim _Listado As IEnumerable

            _Listado = From Taula In oDTC.Entrada_Linea Where Taula.ID_Producto = pId And Taula.NoRestarStock = False And Taula.Entrada.ID_Entrada_Tipo = CInt(EnumEntradaTipo.AlbaranCompra) Order By Taula.FechaEntrada Descending Select Taula.ID_Entrada, Taula.Entrada.Codigo, Taula.Entrada.FechaEntrada, Taula.Entrada.Proveedor.Nombre, Taula.Unidad, Taula.Precio, Taula.Descuento1, Taula.Descuento2, Taula.IVA, Taula.TotalIVA, Taula.TotalBase, Taula.TotalLinea

            With Me.GRD_PreciosCompra

                .M.clsUltraGrid.CargarIEnumerable(_Listado)
                .M_NoEditable()

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PreciosCompra_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_PreciosCompra.M_GRID_DoubleClickRow2
        Dim _ID_Entrada As Integer = e.Cells("ID_Entrada").Value
        Dim _Entrada As Entrada = oDTC.Entrada.Where(Function(F) F.ID_Entrada = _ID_Entrada).FirstOrDefault
        Dim frm As New frmEntrada
        frm.Entrada(_ID_Entrada, _Entrada.ID_Entrada_Tipo)
        frm.FormObrir(Me, True)
    End Sub

#End Region

#Region "Grid Uso"

    Public Sub CargaGrid_Uso(ByVal pID As Integer)
        Try
            With Me.GRD_Uso
                .M.clsUltraGrid.Cargar("Select * From C_Instalacion Where ID_Instalacion in (Select ID_Instalacion From C_Propuesta_Linea Where Activo=1 And SeInstalo=1 and ID_Producto=" & pID & ")", BD)
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Uso_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Uso.M_GRID_DoubleClickRow
        Try
            Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
            frm.Entrada(Me.GRD_Uso.GRID.Selected.Rows(0).Cells("ID_Instalacion").Value)
            frm.FormObrir(Me, True)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Partes"

    Public Sub CargaGrid_Partes(ByVal pID As Integer)
        Try
            With Me.GRD_Partes
                .M.clsUltraGrid.Cargar("Select * From C_Parte_Reparacion Where Activo=1 and ID_Producto=" & pID, BD)
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Partes_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Partes.M_GRID_DoubleClickRow
        Try
            Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
            frm.Entrada(Me.GRD_Partes.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
            frm.FormObrir(Me, True)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "Grid Uso Presupuestado"

    Public Sub CargaGrid_UsoPresupuestado(ByVal pID As Integer)
        Try
            With Me.GRD_Uso_Presupuestado
                .M.clsUltraGrid.Cargar("Select * From C_Propuesta Where SeInstalo=0 and ID_Propuesta in (Select ID_Propuesta From C_Propuesta_Linea Where Activo=1 And SeInstalo=0 and ID_Producto=" & pID & ")", BD)
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Uso_Presupuestado_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD_Uso_Presupuestado.M_GRID_DoubleClickRow
        Try
            Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
            frm.Entrada(Me.GRD_Uso_Presupuestado.GRID.Selected.Rows(0).Cells("ID_Instalacion").Value)
            frm.FormObrir(Me, True)

            Dim frm2 As New frmPropuesta
            frm2.Entrada(frm.oLinqInstalacion, frm.oDTC, Me.GRD_Uso_Presupuestado.GRID.Selected.Rows(0).Cells("ID_Propuesta").Value)
            AddHandler frm2.FormClosing, AddressOf frm.AlTancarfrmPropuesta
            frm2.FormObrir(frm)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region

#Region "GRID Producto Alternativo"
    Private Sub CargaGrid_ProductoAlternativo(ByVal pId As Integer)
        Try
            With Me.GRD_ProductoAlternativo
                ' .M.clsUltraGrid.Cargar("Select * From Producto_Alternativo as A, C_Producto Where ID_Producto in (Select ID_Producto From Producto_Alternativo Where ID_Producto_Necesario=" & pId & ") order by Descripcion", BD)
                .M.clsUltraGrid.Cargar("Select * From Producto_Alternativo as A, C_Producto as B Where A.ID_Producto_Necesario = B.ID_Producto and A.ID_Producto=" & pId & " order by Descripcion", BD)
                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductoAlternativo_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ProductoAlternativo.M_ToolGrid_ToolAfegir
        Try
            If Guardar() = False Then
                Exit Sub
            End If

            Dim frm As New frmProducto_Alternativo
            frm.Entrada(oLinqProducto, oDTC)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmProductoAlternativo
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_ProductoAlternativo_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ProductoAlternativo.M_ToolGrid_ToolEditar
        Call GRD_ProductoAlternativo_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub GRD_ProductoAlternativo_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ProductoAlternativo.M_ToolGrid_ToolEliminar
        Try
            If Me.GRD_ProductoAlternativo.GRID.Selected.Cells.Count = 0 And Me.GRD_ProductoAlternativo.GRID.Selected.Rows.Count = 0 Then
                Exit Sub
            End If
            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then

                Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                If Me.GRD_ProductoAlternativo.GRID.Selected.Cells.Count > 0 Then
                    pRow = Me.GRD_ProductoAlternativo.GRID.Selected.Cells(0).Row
                Else
                    pRow = Me.GRD_ProductoAlternativo.GRID.Selected.Rows(0)
                End If

                Dim ID As Integer = pRow.Cells("ID_Producto_Alternativo").Value
                Dim _Linea As Producto_Alternativo = oLinqProducto.Producto_Alternativo.Where(Function(F) F.ID_Producto_Alternativo = ID).FirstOrDefault()

                oLinqProducto.Producto_Alternativo.Remove(_Linea)
                oDTC.Producto_Alternativo.DeleteOnSubmit(_Linea)


                oDTC.SubmitChanges()

                Call CargaGrid_ProductoAlternativo(oLinqProducto.ID_Producto)

                '_DTC.SubmitChanges()
                'pRow.Hidden = True
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductoAlternativo_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_ProductoAlternativo.M_ToolGrid_ToolVisualitzarDobleClickRow
        If oLinqProducto.ID_Producto = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        If Me.GRD_ProductoAlternativo.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        If Guardar() = False Then
            Exit Sub
        End If

        Dim frm As New frmProducto_Alternativo
        frm.Entrada(oLinqProducto, oDTC, Me.GRD_ProductoAlternativo.GRID.Selected.Rows(0).Cells("ID_Producto_Alternativo").Value)
        AddHandler frm.FormClosing, AddressOf AlTancarfrmProductoAlternativo
        frm.FormObrir(Me)
    End Sub

    Private Sub AlTancarfrmProductoAlternativo()
        Call CargaGrid_ProductoAlternativo(oLinqProducto.ID_Producto)
    End Sub
#End Region

#Region "GRID Producto Requerido"
    Private Sub CargaGrid_ProductoRequerido(ByVal pId As Integer)
        Try

            With Me.GRD_ProductoRequerido
                ' .M.clsUltraGrid.Cargar("Select * From Producto_Alternativo as A, C_Producto Where ID_Producto in (Select ID_Producto From Producto_Alternativo Where ID_Producto_Necesario=" & pId & ") order by Descripcion", BD)
                .M.clsUltraGrid.Cargar("Select * From Producto_Requerido as A, C_Producto as B Where A.ID_Producto_Necesario = B.ID_Producto and A.ID_Producto=" & pId & " order by Descripcion", BD)
                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductoRequerido_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ProductoRequerido.M_ToolGrid_ToolAfegir
        Try
            If Guardar() = False Then
                Exit Sub
            End If

            Dim frm As New frmProducto_Requerido
            frm.Entrada(oLinqProducto, oDTC)
            AddHandler frm.FormClosing, AddressOf AlTancarfrmProductoRequerido
            frm.FormObrir(Me)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    Private Sub GRD_ProductoRequerido_M_ToolGrid_ToolEditar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ProductoRequerido.M_ToolGrid_ToolEditar
        Call GRD_ProductoRequerido_M_ToolGrid_ToolVisualitzarDobleClickRow()
    End Sub

    Private Sub GRD_ProductoRequerido_M_ToolGrid_ToolEliminar(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ProductoRequerido.M_ToolGrid_ToolEliminar
        Try
            With GRD_ProductoRequerido
                If .GRID.Selected.Cells.Count = 0 And .GRID.Selected.Rows.Count = 0 Then
                    Exit Sub
                End If
                If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA) = M_Mensaje.Botons.SI Then

                    Dim pRow As Infragistics.Win.UltraWinGrid.UltraGridRow

                    If .GRID.Selected.Cells.Count > 0 Then
                        pRow = .GRID.Selected.Cells(0).Row
                    Else
                        pRow = .GRID.Selected.Rows(0)
                    End If

                    Dim ID As Integer = pRow.Cells("ID_Producto_Requerido").Value
                    Dim _Linea As Producto_Requerido = oLinqProducto.Producto_Requerido.Where(Function(F) F.ID_Producto_Requerido = ID).FirstOrDefault()

                    oLinqProducto.Producto_Requerido.Remove(_Linea)
                    oDTC.Producto_Requerido.DeleteOnSubmit(_Linea)


                    oDTC.SubmitChanges()

                    Call CargaGrid_ProductoRequerido(oLinqProducto.ID_Producto)

                End If

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ProductoRequerido_M_ToolGrid_ToolVisualitzarDobleClickRow() Handles GRD_ProductoRequerido.M_ToolGrid_ToolVisualitzarDobleClickRow
        If oLinqProducto.ID_Producto = 0 Then
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
            Exit Sub
        End If

        If Me.GRD_ProductoRequerido.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        If Guardar() = False Then
            Exit Sub
        End If

        Dim frm As New frmProducto_Requerido
        frm.Entrada(oLinqProducto, oDTC, Me.GRD_ProductoRequerido.GRID.Selected.Rows(0).Cells("ID_Producto_Requerido").Value)
        AddHandler frm.FormClosing, AddressOf AlTancarfrmProductoRequerido
        frm.FormObrir(Me)
    End Sub

    Private Sub AlTancarfrmProductoRequerido()
        Call CargaGrid_ProductoRequerido(oLinqProducto.ID_Producto)
    End Sub
#End Region

#Region "Stock"

#Region "Grid Stock"

    Private Sub CargaGrid_Stock(ByVal pIDProducto As Integer)
        Try

            Me.GRD_Stock.M.clsUltraGrid.Cargar("Select * From RetornaStock(" & pIDProducto & ",0)  Order by ProductoDescripcion", BD)

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

#Region "Grid Stock NS"

    Private Sub CargaGrid_StockNS(ByVal pIDProducto As Integer)
        Try

            Dim _Llistat As IEnumerable = From Taula In oDTC.NS Where Taula.ID_Producto = pIDProducto Order By Taula.Descripcion Select Taula.ID_NS_Estado, Taula.ID_NS, Taula.Descripcion, Almacen = Taula.Almacen.Descripcion

            Me.GRD_Stock_NS.M.clsUltraGrid.CargarIEnumerable(_Llistat)

            Dim pRow As UltraGridRow
            For Each pRow In Me.GRD_Stock_NS.GRID.Rows
                If pRow.Cells("ID_NS_Estado").Value = CInt(EnumNSEstado.NoDisponible) Then
                    pRow.CellAppearance.BackColor = Color.LightCoral
                End If
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Stock_NS_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Stock_NS.M_GRID_DoubleClickRow2
        If Me.GRD_Stock_NS.GRID.Selected.Rows.Count = 1 Then
            Dim frm As New frmProducto_Trazabilidad
            frm.Entrada(0, e.Cells("ID_NS").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

#End Region

#End Region

#Region "Events Varis"

    Private Sub TE_Codigo_EditorButtonClick(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinEditors.EditorButtonEventArgs) Handles TE_Codigo.EditorButtonClick
        If e.Button.Key = "Lupeta" Then
            Call Cridar_Llistat_Generic()
        End If
    End Sub

    Private Sub TE_Codigo_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles TE_Codigo.KeyDown
        If Me.TE_Codigo.Text Is Nothing = False AndAlso Me.TE_Codigo.Text.Length > 0 Then
            If e.KeyCode = Keys.Enter Then
                Dim ooLinqProducto As Producto = oDTC.Producto.Where(Function(F) F.Codigo = Me.TE_Codigo.Text).FirstOrDefault()
                If ooLinqProducto Is Nothing = False Then
                    Call Cargar_Form(ooLinqProducto.ID_Producto)
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

    Private Sub CH_Central_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Central.CheckedChanged
        If Me.CH_Central.Checked = True Then
            Me.T_Num_Zonas.ReadOnly = False
            Me.T_Num_Zonas_Inalambricas.ReadOnly = False
            Me.T_Num_Zonas_Placa.ReadOnly = False
            Me.T_Num_Zonas_Inalambricas_Placa.ReadOnly = False
            Me.T_Num_Elementos_Bus.ReadOnly = False
        Else
            Me.T_Num_Zonas.Value = Nothing
            Me.T_Num_Zonas.ReadOnly = True

            Me.T_Num_Zonas_Inalambricas.Value = Nothing
            Me.T_Num_Zonas_Inalambricas.ReadOnly = True

            Me.T_Num_Zonas_Placa.Value = Nothing
            Me.T_Num_Zonas_Placa.ReadOnly = True

            Me.T_Num_Zonas_Inalambricas_Placa.Value = Nothing
            Me.T_Num_Zonas_Inalambricas_Placa.ReadOnly = True

            Me.T_Num_Elementos_Bus.Value = Nothing
            Me.T_Num_Elementos_Bus.ReadOnly = True
        End If

    End Sub

    Private Sub CH_Sistema_Trasmision_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Sistema_Trasmision.CheckedChanged
        If Me.CH_Sistema_Trasmision.Checked = True Then
            Me.C_Sistema_Trasmision.ReadOnly = False
            Me.C_ATS.ReadOnly = False
        Else
            Me.C_Sistema_Trasmision.Value = Nothing
            Me.C_Sistema_Trasmision.ReadOnly = True

            Me.C_ATS.Value = Nothing
            Me.C_ATS.ReadOnly = True
        End If
    End Sub

    Private Sub CH_Fuente_Alimentacion_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Fuente_Alimentacion.CheckedChanged
        If Me.CH_Fuente_Alimentacion.Checked = True Then
            Me.C_Tipo_Fuente_Alimentacion.ReadOnly = False
        Else
            Me.C_Tipo_Fuente_Alimentacion.Value = Nothing
            Me.C_Tipo_Fuente_Alimentacion.ReadOnly = True
        End If
    End Sub

    Private Sub CH_Sirena_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Sirena.CheckedChanged
        If Me.CH_Sirena.Checked = True Then
            Me.C_Tipo_Sirena.ReadOnly = False
        Else
            Me.C_Tipo_Sirena.Value = Nothing
            Me.C_Tipo_Sirena.ReadOnly = True
        End If
    End Sub

    Private Sub CH_Inalambrico_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Inalambrico.CheckedChanged
        If Me.CH_Inalambrico.Checked = True Then
            Me.C_Frecuencia.ReadOnly = False
        Else
            Me.C_Frecuencia.Value = Nothing
            Me.C_Frecuencia.ReadOnly = True
        End If
    End Sub

    Private Sub CH_Expansor_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Expansor.CheckedChanged
        If Me.CH_Expansor.Checked = True Then
            Me.T_Expansor_Elementos.ReadOnly = False
        Else
            Me.T_Expansor_Elementos.Value = Nothing
            Me.T_Expansor_Elementos.ReadOnly = True
        End If
    End Sub

    Private Sub CH_Modulo_Rele_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Modulo_Rele.CheckedChanged
        If Me.CH_Modulo_Rele.Checked = True Then
            Me.T_Modulo_Rele_Elementos.ReadOnly = False
        Else
            Me.T_Modulo_Rele_Elementos.Value = Nothing
            Me.T_Modulo_Rele_Elementos.ReadOnly = True
        End If
    End Sub

    Private Sub CH_Elemento_Deteccion_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Elemento_Deteccion.CheckedChanged
        If Me.CH_Elemento_Deteccion.Checked = True Then
            Me.T_Num_Aberturas.ReadOnly = False
            Me.T_Num_Aberturas.Value = 0
            Me.T_Num_Zonas_Utilitzadas.Value = 1
        Else
            Me.T_Num_Aberturas.Value = Nothing
            Me.T_Num_Aberturas.ReadOnly = True
        End If
    End Sub

    Private Sub C_Division_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Division.ValueChanged
        If Me.C_Division.Value Is Nothing = False Then
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

            If Me.Tab_Principal.Tabs.Count <> 0 Then 'fem això pq si no peta al entrar al formulari
                Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Intrusion", "CCTV", "Incendios", "Accesos")
            End If
            Select Case Me.C_Division.Value
                Case "1"
                    Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Intrusion")
                    'Me.UltraTabControl1.Tabs("Intrusion").Selected = True
                Case "2"
                    Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "CCTV")
                    'Me.UltraTabControl1.Tabs("CCTV").Selected = True
                Case "3"
                    Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Incendios")
                    ' Me.UltraTabControl1.Tabs("Incendios").Selected = True
                Case "4"
                    Util.Tab_Visible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Accesos")
                    ' Me.UltraTabControl1.Tabs("Accesos").Selected = True
            End Select
        Else
            'Fem això pq el blanquejar ens fa un selectedindex=0 i després al posar a nothing la division no ho tornava a posar invisible per l'if on som
            Util.Tab_InVisible_x_Key(Me.Tab_Principal, M_Util.Enum_Tab_Activacion.ALGUNOS, "Intrusion", "CCTV", "Incendios", "Accesos")
        End If
    End Sub

    Private Sub C_Familia_ValueChanged(sender As System.Object, e As System.EventArgs) Handles C_Familia.ValueChanged
        If Me.C_Familia.Value Is Nothing = False Then
            Me.C_Subfamilia.ReadOnly = False
            Me.C_Subfamilia.Value = Nothing

            If IsNumeric(Me.C_Familia.Value) = True Then
                Util.Cargar_Combo(Me.C_Subfamilia, "SELECT ID_Producto_SubFamilia, Descripcion FROM Producto_SubFamilia WHERE Activo=1 and ID_Producto_Familia=" & Me.C_Familia.Value & " ORDER BY Descripcion", False)
            End If
        End If
    End Sub

    Private Sub CH_Sistema_Trasmision2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Sistema_Trasmision2.CheckedChanged
        If Me.CH_Sistema_Trasmision2.Checked = True Then
            Me.C_Sistema_Trasmision2.ReadOnly = False
            Me.C_ATS2.ReadOnly = False
        Else
            Me.C_Sistema_Trasmision2.Value = Nothing
            Me.C_Sistema_Trasmision2.ReadOnly = True

            Me.C_ATS2.Value = Nothing
            Me.C_ATS2.ReadOnly = True
        End If
    End Sub

    Private Sub CH_CCTV_POE_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_CCTV_POE.CheckedChanged
        If Me.CH_CCTV_POE.Checked = True Then
            Me.C_CCTV_ClasePOE.ReadOnly = False
        Else
            Me.C_CCTV_ClasePOE.Value = 0
            Me.C_CCTV_ClasePOE.ReadOnly = True
        End If
    End Sub

    Private Sub CH_Acceso_Cerradura_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Acceso_Cerradura.CheckedChanged
        If Me.CH_Acceso_Cerradura.Checked = True Then
            Me.C_Acceso_Cerradura.ReadOnly = False
        Else
            Me.C_Acceso_Cerradura.Value = 0
            Me.C_Acceso_Cerradura.ReadOnly = True
        End If
    End Sub

    Private Sub CH_Acceso_Lector_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_Acceso_Lector.CheckedChanged
        If Me.CH_Acceso_Lector.Checked = True Then
            Me.C_Acceso_Lector.ReadOnly = False
        Else
            Me.C_Acceso_Lector.Value = 0
            Me.C_Acceso_Lector.ReadOnly = True
        End If
    End Sub

    Private Sub CH_INC_Detector_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_INC_Detector.CheckedChanged
        If Me.CH_INC_Detector.Checked = True Then
            Me.C_INC_TipoDetector.ReadOnly = False
        Else
            Me.C_INC_TipoDetector.Value = 0
            Me.C_INC_TipoDetector.ReadOnly = True
        End If
    End Sub

    Private Sub CH_INC_Inalambrico_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_INC_Inalambrico.CheckedChanged
        If Me.CH_INC_Inalambrico.Checked = True Then
            Me.C_INC_FrecuenciasInalambricas.ReadOnly = False
        Else
            Me.C_INC_FrecuenciasInalambricas.Value = 0
            Me.C_INC_FrecuenciasInalambricas.ReadOnly = True
        End If
    End Sub

    Private Sub CH_INC_Central_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_INC_Central.CheckedChanged
        If Me.CH_INC_Central.Checked = True Then
            Me.T_INC_NumLazos.ReadOnly = False
            Me.T_INC_ElementosPorLazo.ReadOnly = False
            Me.T_INC_CentralesVinculadas.ReadOnly = False
        Else
            Me.T_INC_NumLazos.Value = Nothing
            Me.T_INC_NumLazos.ReadOnly = True

            Me.T_INC_ElementosPorLazo.Value = Nothing
            Me.T_INC_ElementosPorLazo.ReadOnly = True

            Me.T_INC_CentralesVinculadas.Value = Nothing
            Me.T_INC_CentralesVinculadas.ReadOnly = True
        End If
    End Sub

    Private Sub UltraTabControl1_SelectedTabChanged(sender As System.Object, e As Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs) Handles Tab_Principal.SelectedTabChanged
        Call CarregarDadesPestanyes(e.Tab.Key)
    End Sub

    Private Sub C_Certificado_Grado_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Certificado_Grado.BeforeDropDown
        Util.Cargar_Combo(Me.C_Certificado_Grado, "Select Archivo.ID_archivo, Descripcion From Archivo, Producto_Archivo Where Archivo.ID_Archivo=producto_archivo.ID_Archivo and Tipo like '%Adobe Acrobat%' and Activo=1 and ID_Producto_Archivo=" & oLinqProducto.ID_Producto, True, True, "No seleccionado")
    End Sub

    Private Sub C_Certificado_ClaseA_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_Certificado_ClaseA.BeforeDropDown
        Util.Cargar_Combo(Me.C_Certificado_ClaseA, "Select Archivo.ID_archivo, Descripcion From Archivo, Producto_Archivo Where Archivo.ID_Archivo=producto_archivo.ID_Archivo and Tipo like '%Adobe Acrobat%' and Activo=1 and ID_Producto_Archivo=" & oLinqProducto.ID_Producto, True, True, "No seleccionado")
    End Sub

    Private Sub C_FichaTecnica_BeforeDropDown(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles C_FichaProducto.BeforeDropDown
        Util.Cargar_Combo(Me.C_FichaProducto, "Select Archivo.ID_archivo, Descripcion From Archivo, Producto_Archivo Where Archivo.ID_Archivo=producto_archivo.ID_Archivo and Tipo like '%Adobe Acrobat%' and Activo=1 and ID_Producto_Archivo=" & oLinqProducto.ID_Producto, True, True, "No seleccionado")
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
                oLinqProducto.Archivo = _Archivo
                If oLinqProducto.ID_Archivo_FotoPredeterminadaMini.HasValue = True Then
                    oDTC.Archivo.DeleteOnSubmit(oLinqProducto.Archivo_Mini)
                End If
                oDTC.SubmitChanges()

                Dim _ArchivoMini As New Archivo
                _ArchivoMini.Activo = True
                _ArchivoMini.CampoBinario = Util.ImageToBinary(M_Util.clsFunciones.CompressImage(M_Util.clsFunciones.ResizeImage(Util.BinaryToImage(_Archivo.CampoBinario.ToArray), New System.Drawing.Size(40, 40), True))).ToArray
                _ArchivoMini.Descripcion = "Foto Mini ID=" & oLinqProducto.ID_Producto
                _ArchivoMini.Ruta_Fichero = _Archivo.Ruta_Fichero
                _ArchivoMini.Tipo = _Archivo.Tipo
                ' _ArchivoMini.Ruta_Fichero
                _ArchivoMini.Fecha = Now.Date

                oDTC.Archivo.InsertOnSubmit(_ArchivoMini)
                oDTC.SubmitChanges()
                oLinqProducto.Archivo_Mini = _ArchivoMini

                oDTC.SubmitChanges()
                Call DespresDeCarregarDadesGridArchivos()
            End If

            If e.Tool.Key = "QuitarFotoPredeterminada" Then
                'Dim _IDArchivo As Integer = Me.GRD_Ficheros.GRID.Selected.Rows(0).Cells("ID_Archivo").Value
                'Dim _Archivo As Archivo
                '_Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = _IDArchivo).FirstOrDefault
                'If _Archivo.ID_Archivo = oLinqProducto.ID_Archivo_FotoPredeterminada Then
                oLinqProducto.Archivo = Nothing
                oDTC.Archivo.DeleteOnSubmit(oLinqProducto.Archivo_Mini)
                oLinqProducto.Archivo_Mini = Nothing
                oDTC.SubmitChanges()
                'End If
                Call DespresDeCarregarDadesGridArchivos()
            End If
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Ficheros_M_ToolGrid_ToolEliminar(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Ficheros.M_ToolGrid_ToolEliminar
        oLinqProducto.Archivo = Nothing
        oDTC.SubmitChanges()
    End Sub

    Private Sub B_Informe_Click(sender As System.Object, e As System.EventArgs) Handles B_Informe.Click
        'Dim frmDesigner As New RibbonReportDesigner.MainForm
        'Dim _Report As New DevExpress.XtraReports.UI.XtraReport

        Dim _Archivo As Archivo
        If oLinqProducto.ID_Archivo_Informe = 0 Or oLinqProducto.ID_Archivo_Informe.HasValue = False Then
            _Archivo = New Archivo
            _Archivo.Activo = True
            _Archivo.CampoBinario = Nothing
            _Archivo.Descripcion = "Informe ficha producto id: " & oLinqProducto.ID_Producto
            _Archivo.Fecha = Now.Date
            oDTC.Archivo.InsertOnSubmit(_Archivo)
            oLinqProducto.ArchivoInforme = _Archivo
            oDTC.SubmitChanges()
        Else
            _Archivo = oLinqProducto.ArchivoInforme
        End If

        Informes.ObrirFormReportDisseny(M_Informes.EnumTipusAperturaFormulariDisseny.PassantElFitxer, oDTC, _Archivo.ID_Archivo)

        'If IsNothing(_Archivo.CampoBinario) = False Then
        '    Dim dStream As System.IO.MemoryStream = New System.IO.MemoryStream(_Archivo.CampoBinario.ToArray, 0, _Archivo.CampoBinario.Length)
        '    dStream.Position = 0

        '    _Report.LoadLayout(dStream)
        '    dStream.Close()
        'Else
        '    _Report.PaperKind = Drawing.Printing.PaperKind.A4
        '    _Report.Margins.Right = 39  '99
        '    _Report.Margins.Bottom = 39  '99
        '    _Report.Margins.Left = 60  '152
        '    _Report.Margins.Top = 52   '132
        '    _Report.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter
        'End If
        ''_Report.Name = _Linea.ID_Informe_Apartado_Version
        '_Report.Tag = oLinqProducto.ID_Archivo_Informe
        '_Report.DisplayName = oLinqProducto.Descripcion
        '_Report.DataAdapter = Nothing


        'frmDesigner.Entrada(oDTC, EnumInformeDiseñadorTipoEntrada.FichaProducto, _Report)
        'frmDesigner.Show()
    End Sub

    Private Sub UltraFormattedLinkLabel1_LinkClicked(sender As System.Object, e As Infragistics.Win.FormattedLinkLabel.LinkClickedEventArgs) Handles UltraFormattedLinkLabel1.LinkClicked
        frmPrincipal.CarregarFormWeb(UltraFormattedLinkLabel1.Value)

    End Sub

    Private Sub CH_PVP_Proveedor_Predeterminado_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CH_PVP_Proveedor_Predeterminado.CheckedChanged
        If CH_PVP_Proveedor_Predeterminado.Checked = True Then
            Me.T_PVP.ReadOnly = True
            Me.T_PVD.ReadOnly = True
            Me.T_PlazoEntrega.ReadOnly = True
            Me.GRD_Precios.Enabled = True
        Else
            Me.T_PVP.ReadOnly = False
            Me.T_PVD.ReadOnly = False
            Me.T_PlazoEntrega.ReadOnly = False
            Me.GRD_Precios.Enabled = False
            Dim _pRow As UltraGridRow
            For Each _pRow In Me.GRD_Precios.GRID.Rows
                If _pRow.Cells("Predeterminado").Value = True Then
                    _pRow.Cells("Predeterminado").Value = False
                    _pRow.Update()
                End If
            Next
        End If
    End Sub

    Private Sub frmProducto_AlTancarForm(ByRef pCancel As Boolean) Handles Me.AlTancarForm
        If Me.Visible = True Then
            If oclsPilaFormularis.OcultarFormulari(Me) = True Then
                pCancel = True
            End If
        End If
    End Sub

    Private Sub C_Tipo_Documento_ValueChanged(sender As Object, e As EventArgs) Handles C_Tipo_Documento.ValueChanged
        If Me.C_Tipo_Documento.Items.Count = 0 OrElse Me.C_Tipo_Documento.Value Is Nothing Then
            Exit Sub
        End If

        Dim DTS As New DataSet
        BD.CargarDataSet(DTS, "Select * From C_Entrada Where ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Entrada in (Select ID_Entrada From C_Entrada_Linea Where ID_Producto=" & oLinqProducto.ID_Producto & " Group by ID_Entrada) Order by Codigo")
        BD.CargarDataSet(DTS, "Select * From C_Entrada_Linea Where ID_Entrada_Linea_Padre is null and ID_Entrada_Tipo=" & Me.C_Tipo_Documento.Value & " and ID_Producto= " & oLinqProducto.ID_Producto & "  Order by FechaEntrada", "aa", 0, "ID_Entrada", "ID_Entrada", True)
        Me.GRD_Step.GRID.DisplayLayout.MaxBandDepth = 4 'tinc que fer aquesta merda pq si no no em surten els fills

        Me.GRD_Step.M.clsUltraGrid.Cargar(DTS)
    End Sub

    Private Sub GRD_Step_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Step.M_GRID_DoubleClickRow2
        If e.Band.Index = 0 Then
            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
            frm.Entrada(e.Cells("ID_Entrada").Value, e.Cells("ID_Entrada_Tipo").Value)
            frm.FormObrir(Me, True)
        End If
    End Sub

    Private Sub C_TipoCalculoPrecio_ValueChanged(sender As Object, e As EventArgs) Handles C_TipoCalculoPrecio.ValueChanged
        If Me.C_TipoCalculoPrecio.SelectedIndex = -1 Or Me.GRD_Precios.GRID.DataSource Is Nothing Then
            Exit Sub
        End If
        If Me.C_TipoCalculoPrecio.Value = 0 Then
            Call PosarEditableColumnes(Me.GRD_Precios, 0, "PVD", False)
            Call PosarEditableColumnes(Me.GRD_Precios, 0, "PVP", True)
            Me.GRD_Precios.GRID.DisplayLayout.Bands(0).Columns("Descuento").Header.Caption = "Descuento"
        Else
            Call PosarEditableColumnes(Me.GRD_Precios, 0, "PVD", True)
            Call PosarEditableColumnes(Me.GRD_Precios, 0, "PVP", False)
            Me.GRD_Precios.GRID.DisplayLayout.Bands(0).Columns("Descuento").Header.Caption = "Incremento"
        End If

        Dim _pRow As UltraGridRow
        For Each _pRow In Me.GRD_Precios.GRID.Rows
            If Me.C_TipoCalculoPrecio.Value = 0 Then
                _pRow.Cells("PVD").Value = CDec(0)
            Else
                _pRow.Cells("PVP").Value = 0
            End If

            _pRow.Update()
        Next
    End Sub

    Private Sub PosarEditableColumnes(ByRef pGrid As M_UltraGrid.m_UltraGrid, ByVal pIDBanda As Integer, ByVal pNomColumna As String, ByVal pEnabled As Boolean)
        Try
            If pEnabled = True Then
                pGrid.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellActivation = Activation.AllowEdit
            Else
                pGrid.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellActivation = Activation.Disabled
            End If

            pGrid.GRID.DisplayLayout.Bands(pIDBanda).Columns(pNomColumna).CellClickAction = CellClickAction.EditAndSelectText
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CH_Bono_CheckedChanged(sender As Object, e As EventArgs) Handles CH_Bono.CheckedChanged
        If Me.CH_Bono.Checked = True Then
            Me.T_Bono_Cantidad.ReadOnly = False
        Else
            Me.T_Bono_Cantidad.ReadOnly = True
            Me.T_Bono_Cantidad.Value = 0
        End If
    End Sub

    Private Sub B_Atras_Click(sender As Object, e As EventArgs) Handles B_Atras.Click
        Call AvanzarRetroceder(False)
    End Sub

    Private Sub B_Adelante_Click(sender As Object, e As EventArgs) Handles B_Adelante.Click
        Call AvanzarRetroceder(True)
    End Sub

#End Region

#Region "Grid Idiomas"

    Private Sub Carga_Grid_Idiomas(ByVal pId As Integer)
        Try
            With Me.GRD_Idioma

                Dim _Idioma As IEnumerable(Of Producto_DescripcionIdioma) = From taula In oDTC.Producto_DescripcionIdioma Where taula.ID_Producto = pId Select taula
                .M.clsUltraGrid.CargarIEnumerable(_Idioma)

                .M_Editable()

                Call CargarCombo_Idioma(.GRID)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Idioma(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Idioma) = (From Taula In oDTC.Idioma Order By Taula.Descripcion Select Taula)
            Dim Var As Idioma

            'Valors.ValueListItems.Add("-1", "Seleccione un registro")
            For Each Var In oTaula
                Valors.ValueListItems.Add(Var, Var.Descripcion)
            Next

            pGrid.DisplayLayout.Bands(0).Columns("Idioma").Style = ColumnStyle.DropDownList
            pGrid.DisplayLayout.Bands(0).Columns("Idioma").ValueList = Valors.Clone

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Idioma_M_GRID_CellListSelect(ByRef sender As Object, ByRef e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles GRD_Idioma.M_GRID_CellListSelect
        Try
            If e.Cell.Column.Key <> "Idioma" Then 'si no modifiquem el combo de personal no hi hem de fer re aqui
                Exit Sub
            End If

            'Comprovem que el registre que hi havia abans no tenia hores imputades
            Dim _Idioma As Idioma = e.Cell.Value

            'Comprovem que no s'hagi introduit aquest treballador abans
            _Idioma = CType(CType(e.Cell.ValueListResolved, Infragistics.Win.ValueList).SelectedItem, Infragistics.Win.ValueListItem).DataValue
            If oLinqProducto.Producto_DescripcionIdioma.Where(Function(F) F.Idioma Is _Idioma).Count <> 0 Then
                Mensaje.Mostrar_Mensaje("Imposible añadir, no se puede asignar el mismo idioma dos veces", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                e.Cell.CancelUpdate()
                Exit Sub
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Idioma_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Idioma.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_Idioma
                If oLinqProducto.ID_Producto = 0 Then
                    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_PRIMER_GUARDA_ENTITAT_PRINCIPAL)
                    Exit Sub
                End If

                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Producto").Value = oLinqProducto

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Idioma_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Idioma.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                e.Delete(False)
                oLinqProducto.Producto_DescripcionIdioma.Remove(e.ListObject)
                Exit Sub
            End If

            'If sender.Rows.Count > 1 Then 'Si hi ha més d'una Row i la que estem esborrant es la predeterminada, ho impedirem
            '    If sender.Selected.Rows(0).Cells("Predeterminado").Value = True Then
            '        Mensaje.Mostrar_Mensaje("Imposible eliminar el proveedor predeterminado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            '        Exit Sub
            '    End If
            'End If

            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.SI Then
                e.Delete(False)
                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            End If

            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Idioma_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Idioma.M_GRID_AfterRowUpdate
        oDTC.SubmitChanges()
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

End Class