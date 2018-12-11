Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmInstalacionCreacionPartes
    Dim oDTC As DTCDataContext
    Dim oLinqInstalacion As Instalacion
    Dim oLinqPropuesta As Propuesta

#Region "M_ToolForm"

    Private Sub ToolForm_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        Me.GRD_Ficheros.GRID.Selected.Rows.Clear()
        Me.GRD_Ficheros.GRID.ActiveRow = Nothing

        If Me.CH_GenerarPedidoVenta.Checked = False AndAlso Me.CH_GenerarParte.Checked = False Then
            Exit Sub
        End If

        If Me.CH_GenerarParte.Checked = True AndAlso Me.R_TrabajosARealizar.pText.Length = 0 Then
            Mensaje.Mostrar_Mensaje("Imposible crear el parte, el campo 'Trabajos a realizar' es obligatorio", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        If Me.CH_GenerarPedidoVenta.Checked = True AndAlso (Me.DT_Documento.Value Is Nothing) Then
            Mensaje.Mostrar_Mensaje("Imposible crear el pedido de venta, el campo 'Fecha del pedido de venta' es obligatorio", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        If CH_GenerarParte.Checked = True Then
            Util.WaitFormObrir()
            Call GenerarParte()
            Util.WaitFormTancar()
        End If

        If CH_GenerarPedidoVenta.Checked = True Then
            Util.WaitFormObrir()
            Dim _NewEntrada As New Entrada
            Dim oclsEntrada As New clsEntrada(oDTC, _NewEntrada)
            oclsEntrada.CrearPedidodeVentaAPartirDeUnaPropuesta(oLinqPropuesta, Me.DT_Documento.Value, Me.C_Empresa.Value)

            Dim _ClsNotificacion As New clsNotificacion(oDTC)
            _ClsNotificacion.CrearNotificacion_AlCrearPedidoVenta(_NewEntrada)

            Util.WaitFormTancar()
        End If

        Call M_ToolForm1_m_ToolForm_Sortir()

    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Sortir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Metodes"

    Public Sub Entrada(ByRef pInstalacion As Instalacion, ByRef pPropuesta As Propuesta, ByRef pDTC As DTCDataContext)

        Try
            Me.AplicarDisseny()
            'Me.ToolForm.M.Botons.tGuardar.SharedProps.Visible = False

            oLinqInstalacion = pInstalacion
            oLinqPropuesta = pPropuesta
            oDTC = pDTC

            Me.R_TrabajosARealizar.pText = "Trabajo a realizar según presupuesto nº: " & oLinqPropuesta.Codigo & " " & oLinqPropuesta.Descripcion

            Util.Cargar_Combo(Me.C_Empresa, "Select ID_Empresa, NombreComercial From Empresa Where Activo=1 and ID_Empresa in (Select ID_Empresa From Cliente_Empresa Where ID_Cliente=" & oLinqInstalacion.ID_Cliente & ") Order by NombreComercial ", False)
            Me.C_Empresa.Value = oEmpresa.ID_Empresa 'Carreguem la predeterminada
            If Me.C_Empresa.SelectedIndex = -1 Then  'Si la predeterminada no està en el combo, carregarem la primera que trobem
                Me.C_Empresa.SelectedIndex = 0
            End If

            Me.DT_Documento.Value = Now.Date

            Call CargaGrid_Ficheros()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try


    End Sub

    Private Sub GenerarParte()
        Dim _NewParte As New Parte
        With _NewParte
            .Activo = True
            .ID_Parte_Vinculado = Nothing
            .Cliente = oLinqInstalacion.Cliente
            .Instalacion = oLinqInstalacion
            .Propuesta = oLinqPropuesta
            .Direccion = oLinqInstalacion.Direccion
            .PersonaContacto = oLinqInstalacion.PersonaContacto
            .Poblacion = oLinqInstalacion.Poblacion
            .Provincia = oLinqInstalacion.Provincia
            .Telefono = oLinqInstalacion.Telefono
            .Longitud = oLinqInstalacion.Longitud
            .Latitud = oLinqInstalacion.Latitud
            .CP = oLinqInstalacion.CP
            .Pais = oLinqInstalacion.Pais
            .Delegacion = oLinqInstalacion.Delegacion
            .TrabajoARealizar = Me.R_TrabajosARealizar.pTextEspecial
            .TrabajoARealizarRTF = Me.R_TrabajosARealizar.pText
            .HorasPrevistas = oLinqPropuesta.HorasPrevistas

            .FechaAlta = Date.Now
            .ID_Parte_Estado = EnumParteEstado.Pendiente
            .ID_Parte_Tipo = EnumParteTipo.Instalacion
            .ParteFirmado = False
            .ID_Parte_TipoFacturacion = 1  'Facturable

            Dim _aux As New Parte_Aux
            _aux.ObservacionesTecnico = ""
            .Parte_Aux = _aux

            Dim pRow As UltraWinGrid.UltraGridRow
            For Each pRow In Me.GRD_Ficheros.GRID.Rows
                If pRow.Cells("Seleccion").Value = True Then
                    Dim _Archivo As Archivo
                    _Archivo = oDTC.Archivo.Where(Function(F) F.ID_Archivo = CInt(pRow.Cells("ID_Archivo").Value)).FirstOrDefault

                    Dim _NewArchivo As New Archivo
                    Dim _NewParteArchivo As New Parte_Archivo
                    _NewArchivo.Activo = True
                    _NewArchivo.CampoBinario = _Archivo.CampoBinario
                    _NewArchivo.Color = _Archivo.Color
                    _NewArchivo.Descripcion = _Archivo.Descripcion
                    _NewArchivo.Fecha = _Archivo.Fecha
                    _NewArchivo.ID_Usuario = _Archivo.ID_Usuario
                    _NewArchivo.Ruta_Fichero = _Archivo.Ruta_Fichero
                    _NewArchivo.Tamaño = _Archivo.Tamaño
                    _NewArchivo.Tipo = _Archivo.Tipo

                    _NewParteArchivo.Archivo = _NewArchivo
                    _NewParteArchivo.Parte = _NewParte
                End If
            Next

            oDTC.Parte.InsertOnSubmit(_NewParte)
            oDTC.SubmitChanges()
            Call clsParte.CrearMagatzem(oDTC, _NewParte) 'ho posem davant del submitchanges per a tenir el ID del magatzem

            Dim _ClsNotificacion As New clsNotificacion(oDTC)
            _ClsNotificacion.CrearNotificacion_AlCrearParte(_NewParte)

            Call CrearPreguntesQuestionari(_NewParte)

            Call CopiarPlanosDelPressupostAlParte(_NewParte)

            oDTC.SubmitChanges()
        End With

        Mensaje.Mostrar_Mensaje("Parte creado correctamente", M_Mensaje.Missatge_Modo.INFORMACIO)
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


    Private Sub CrearPreguntesQuestionari(ByRef pNewParte As Parte)
        Dim _IDParteTipo As Integer = pNewParte.ID_Parte_Tipo
        Dim _Pregunta As Parte_Cuestionario_Preguntas
        For Each _Pregunta In oDTC.Parte_Cuestionario_Preguntas.Where(Function(F) F.ID_Parte_Tipo = _IDParteTipo)
            Dim _NewResposta As New Parte_Cuestionario_Respuestas
            _NewResposta.Parte = pNewParte
            _NewResposta.Parte_Cuestionario_Preguntas = _Pregunta
            _NewResposta.Respuesta = 0
            pNewParte.Parte_Cuestionario_Respuestas.Add(_NewResposta)
        Next
        oDTC.SubmitChanges()
    End Sub

    Private Sub CopiarPlanosDelPressupostAlParte(ByRef pNewParte As Parte)
        'Dim _IDParte As Integer = pNewParte.ID_Parte
        Dim _Plano As Propuesta_Plano
        For Each _Plano In oLinqPropuesta.Propuesta_Plano.Where(Function(F) IsNothing(F.PlanoBinario.Foto) = False AndAlso F.PlanoBinario.Foto.Length > 0)
            Dim _NewArchivo As New Archivo
            Dim _NewParteArchivo As New Parte_Archivo
            _NewArchivo.Activo = True
            _NewArchivo.CampoBinario = _Plano.PlanoBinario.Foto
            _NewArchivo.Color = System.Drawing.Color.LightGreen.ToArgb
            _NewArchivo.Descripcion = _Plano.Descripcion
            _NewArchivo.Fecha = _Plano.FechaCreacion
            _NewArchivo.ID_Usuario = Seguretat.oUser.ID_Usuario
            _NewArchivo.Ruta_Fichero = "Plano " & _Plano.ID_PlanoBinario & ".jpg"
            _NewArchivo.Tamaño = "144 Kb"
            _NewArchivo.Tipo = "Archivo JPEG"
            _NewParteArchivo.Archivo = _NewArchivo
            _NewParteArchivo.Parte = pNewParte
        Next
        oDTC.SubmitChanges()

    End Sub

    Private Sub CH_GenerarPedidoVenta_CheckedChanged(sender As Object, e As EventArgs) Handles CH_GenerarPedidoVenta.CheckedChanged
        If Me.CH_GenerarPedidoVenta.Checked = True Then
            Me.Panel_PedidoVenta.Enabled = True
        Else
            Me.Panel_PedidoVenta.Enabled = False
        End If

    End Sub

    Private Sub CH_GenerarParte_CheckedChanged(sender As Object, e As EventArgs) Handles CH_GenerarParte.CheckedChanged
        If Me.CH_GenerarParte.Checked = True Then
            Me.Panel_Parte.Enabled = True
        Else
            Me.Panel_Parte.Enabled = False
        End If
    End Sub
End Class