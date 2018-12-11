Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmImputacionHoras
    Dim oDTC As DTCDataContext
    Dim Fichero As M_Archivos_Binarios.clsArchivosBinarios2


#Region "ToolForm"
    Private Sub M_ToolForm1_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

#End Region

#Region "Mètodes"
    Public Sub Entrada()
        If Seguretat.oUser.ID_Personal.HasValue = False Then
            Mensaje.Mostrar_Mensaje("Imposible entrar en la pantalla, el usuario actual no tiene asociado ningún persona 'Personal'", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Me.Visible = False
            Exit Sub
        End If

        Me.AplicarDisseny()
        Me.KeyPreview = False
        Me.ToolForm.M.Botons.tGuardar.SharedProps.Visible = False



        Dim _Anys As Integer
        For _Anys = 2010 To 2030
            Me.C_Año.Items.Add(_Anys)
        Next
        Me.C_Año.Value = Now.Year

        oDTC = New DTCDataContext(BD.Conexion)

        Fichero = New M_Archivos_Binarios.clsArchivosBinarios2(BD, Me.GRD_Ficheros, "Parte_Archivo", 1)
        Me.GRD_Horas.M.clsToolBar.Boto_Afegir("IrAlParte", "Ir al parte", True)
        Me.GRD_HorasIncorrectas.M.clsToolBar.Boto_Afegir("IrAlParte", "Ir al parte", True)

        Me.C_Semana.Value = Util.RetornaNumeroDeLaSetmana(Now.Date)
        Me.DT_Lunes.Value = Nothing
        Me.DT_Domingo.Value = Nothing
        Me.GRD_HorasIncorrectas.M.clsToolBar.Boto_Afegir("Arreglada", "Dar por arreglada la imputación de horas", True)
        Me.GRD_Material.M.clsToolBar.Boto_Afegir("AsignarMaterials", "Asignar materials", True)

        Call B_Buscar_Click(Nothing, Nothing)


        Me.Calendario_OperariosTableAdapter.Connection = BD.Conexion 'si no posem aquesta línea peta pq agafa la connexio meva de casa

        Me.Preview_RTF.pBotoGuardarVisible = True

        Me.Calendari.GoToToday()
        Me.Calendari.ActiveView.Enabled = False
        Me.Calendari.OptionsCustomization.AllowAppointmentDelete = DevExpress.XtraScheduler.UsedAppointmentType.None
        Me.Calendari.OptionsCustomization.AllowAppointmentDrag = DevExpress.XtraScheduler.UsedAppointmentType.None
        Me.Calendari.OptionsCustomization.AllowAppointmentCreate = DevExpress.XtraScheduler.UsedAppointmentType.None
        Me.Calendari.OptionsCustomization.AllowAppointmentEdit = DevExpress.XtraScheduler.UsedAppointmentType.None
        Me.Calendari.OptionsCustomization.AllowAppointmentResize = DevExpress.XtraScheduler.UsedAppointmentType.None
        Me.Calendari.OptionsCustomization.AllowDisplayAppointmentDependencyForm = DevExpress.XtraScheduler.AllowDisplayAppointmentDependencyForm.Never
        Me.Calendari.OptionsCustomization.AllowDisplayAppointmentForm = DevExpress.XtraScheduler.AllowDisplayAppointmentForm.Never



        'Call CargaGrid_Partes()
        'Call CargaGrid_PartesFicheros()
        'Call Fichero.Cargar_GRID(0)
        ' Call CargaGrid_Division()
        ' Call CargaGrid_Marca(0)

    End Sub

    Private Sub CarregarComboSetmanes(ByVal pAny As Integer)
        Try
            Me.C_Semana.Items.Clear()
            Dim _Data As Date
            _Data = "31/12/" & pAny
            Dim NumSetmanes As Integer = Util.RetornaNumeroDeLaSetmana(_Data)

            For _Semanas = 1 To NumSetmanes
                Me.C_Semana.Items.Add(_Semanas)
            Next

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CalculHoresPendents(ByVal pDataInici As Date, ByVal pDataFi As Date)
        Dim _TotalHoresTreballades As Decimal
        If oDTC.Parte_Horas.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi).Count > 0 Then
            _TotalHoresTreballades = oDTC.Parte_Horas.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi).Sum(Function(F) F.Horas)
        End If

        Me.T_HorasPendientes.Value = 40 - Calcular_HoresPendentsAssignar(oDTC, pDataInici, pDataFi, Seguretat.oUser.ID_Usuario) - _TotalHoresTreballades
        Me.T_HorasPendientes.Value = IIf(Me.T_HorasPendientes.Value < 0, 0, Me.T_HorasPendientes.Value)
    End Sub

    Private Function CompressImage(ByVal myImage As Image) As Image
        Dim jgpEncoder As Imaging.ImageCodecInfo = GetEncoder(Imaging.ImageFormat.Jpeg)
        Dim myEncoderQuality As System.Drawing.Imaging.Encoder = System.Drawing.Imaging.Encoder.Quality


        Dim myEncoderParameters As New Imaging.EncoderParameters(1)
        Dim myEncoderParameter As New Imaging.EncoderParameter(myEncoderQuality, 50&)
        myEncoderParameters.Param(0) = myEncoderParameter
        ' myEncoderParameters.Param(1) = myEncoderParameter

        Dim ImageStream As New System.IO.MemoryStream

        myImage.Save(ImageStream, jgpEncoder, myEncoderParameters)
        'myImage.Save("C:\Users\men\desktop\compressed.jpg", jgpEncoder, myEncoderParameters) 

        Return Image.FromStream(ImageStream)
    End Function

    Private Function GetEncoder(ByVal format As Imaging.ImageFormat) As Imaging.ImageCodecInfo
        Dim codecs As Imaging.ImageCodecInfo() = Imaging.ImageCodecInfo.GetImageDecoders()

        Dim codec As Imaging.ImageCodecInfo
        For Each codec In codecs
            If codec.FormatID = format.Guid Then
                Return codec
            End If
        Next codec
        Return Nothing
    End Function

    Public Shared Function ResizeImage(ByVal image As Image, ByVal size As Size, Optional ByVal preserveAspectRatio As Boolean = True) As Image
        Dim newWidth As Integer
        Dim newHeight As Integer
        If preserveAspectRatio Then
            Dim originalWidth As Integer = image.Width
            Dim originalHeight As Integer = image.Height
            Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
            Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
            Dim percent As Single = If(percentHeight < percentWidth,
                    percentHeight, percentWidth)
            newWidth = CInt(originalWidth * percent)
            newHeight = CInt(originalHeight * percent)
        Else
            newWidth = size.Width
            newHeight = size.Height
        End If
        Dim newImage As Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function

#End Region

#Region "Events"
    Private Sub frmManteniment_Familias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: esta línea de código carga datos en la tabla 'AbidosDataSet1.Calendario_Operarios' Puede moverla o quitarla según sea necesario.
        Me.Calendario_OperariosTableAdapter.Fill(Me.AbidosDataSet1.Calendario_Operarios)
        'Fem això per a que no surti la divisió carregada
        Me.GRD_Horas.GRID.Selected.Rows.Clear()
        Me.GRD_Horas.GRID.ActiveRow = Nothing
    End Sub

    Private Sub C_Año_ValueChanged(sender As Object, e As EventArgs) Handles C_Año.ValueChanged
        If C_Año.SelectedIndex <> -1 Then
            Call CarregarComboSetmanes(Me.C_Año.Value)
            Me.C_Semana.Value = 1
        End If
    End Sub

    Private Sub B_Buscar_Click(sender As Object, e As EventArgs) Handles B_Buscar.Click
        oDTC = New DTCDataContext(BD.Conexion)
        Dim _DataInici As Date
        Dim _DataFi As Date
        _DataInici = Util.RetornaDataPassantLaSetmanaIAny(Me.C_Semana.Value, Me.C_Año.Value)
        _DataFi = _DataInici.AddDays(6)
        Me.DT_Lunes.Value = _DataInici
        Me.DT_Domingo.Value = _DataFi
        Call CargaGrid_Horas(_DataInici, _DataFi)
        Call CargaGrid_HorasIncorrectes()
        Call CalculHoresPendents(_DataInici, _DataFi)
        Call CargaGrid_Partes()
        Call CargaGrid_PartesFicheros()
        Call CargaGrid_PartesActuales()
        Call CargaGrid_ToDo_Partes()
        Call CargaGrid_StockPersonal()
        Call CargaGrid_Trabajo_PartesActuales()
    End Sub


    Private Sub UltraTabControl1_SelectedTabChanged(sender As Object, e As UltraWinTabControl.SelectedTabChangedEventArgs) Handles Tab_Principal.SelectedTabChanged

        Me.GRD_Informe.GRID.Selected.Rows.Clear()
        Me.GRD_Informe.GRID.ActiveRow = Nothing

        Select Case e.Tab.Key
            Case "StockPersonal"
                Call CargaGrid_StockPersonal()
            Case "Calendario"
                Me.Calendari.ResourceNavigator.Visibility = DevExpress.XtraScheduler.ResourceNavigatorVisibility.Always
                Me.SchedulerStorage1.Resources.DataSource = BD.RetornaDataTable("Select ID_Personal, Nombre From Personal Where Activo=1 and ActivarCalendario=1 and ID_Personal =" & Seguretat.oUser.ID_Personal & " Order by Nombre")
                Me.SchedulerStorage1.Resources.Mappings.Caption = "Nombre"
                Me.SchedulerStorage1.Resources.Mappings.Id = "ID_Personal"


                Me.Calendario_OperariosTableAdapter.Fill(Me.AbidosDataSet1.Calendario_Operarios)
                Me.Calendari.DayView.TopRowTime = New TimeSpan(7, 0, 0) 'DateTime.Now.TimeOfDay  

        End Select

    End Sub

#End Region

    'Private Sub Calcular_HoresPendentsAssignar(ByVal pDataInici As Date, ByVal pDataFi As Date)
    '    Dim LlistatFestiusEmpresa As IEnumerable(Of Empresa_FechasNoLaborables) = oDTC.Empresa_FechasNoLaborables.Where(Function(F) F.ID_Empresa = 1 And F.Fecha >= pDataInici And F.Fecha <= pDataFi).OrderBy(Function(F) F.Fecha)
    '    Dim LlistatAusenciesTreballador As IEnumerable(Of Personal_Ausencia) = oDTC.Personal_Ausencia.Where(Function(F) F.ID_Personal = oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi).OrderBy(Function(F) F.Fecha)
    '    Dim _LlistatHoresTreballades As IEnumerable(Of Parte_Horas) = oDTC.Parte_Horas.Where(Function(F) F.Parte.Activo = True And F.ID_Personal = oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi)

    '    Dim _TotalHoresTreballades As Decimal
    '    If oDTC.Parte_Horas.Where(Function(F) F.ID_Personal = oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi).Count > 0 Then
    '        _TotalHoresTreballades = oDTC.Parte_Horas.Where(Function(F) F.ID_Personal = oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi).Sum(Function(F) F.Horas)
    '    End If


    '    Dim _Calcul As Integer
    '    'Primer multipliquem per 8 els dies festius d'empresa que hi ha en aquesta setmana
    '    Dim _horesARestarPerDiesQueNoTreballava As Integer
    '    _horesARestarPerDiesQueNoTreballava = 8 * LlistatFestiusEmpresa.Count

    '    'Després mirarem si aquella setmana ha estat algun dia ausent. 
    '    If LlistatAusenciesTreballador.Count > 0 Then
    '        'Si ho ha estat i encanvi no hi havia cap dia festiu d'empresa multiplicarem * 8 el número de dies que ha estat ausent
    '        If LlistatFestiusEmpresa.Count = 0 Then
    '            _horesARestarPerDiesQueNoTreballava = 8 * LlistatAusenciesTreballador.Count
    '        Else
    '            'Si encanvi, Coincideix que hi havia algun dia festiu d'empresa en aquesta setmana, haurem de vigilar que no coincideixi amb un dels ausents
    '            Dim _Ausencia As Personal_Ausencia
    '            Dim DiesQueHaFaltatIQueNoCoincideixAmbFestiuEmpresa As Integer = 0
    '            For Each _Ausencia In LlistatAusenciesTreballador
    '                If LlistatFestiusEmpresa.Where(Function(F) F.Fecha = _Ausencia.Fecha).Count = 0 Then
    '                    DiesQueHaFaltatIQueNoCoincideixAmbFestiuEmpresa = DiesQueHaFaltatIQueNoCoincideixAmbFestiuEmpresa + 1
    '                End If
    '            Next
    '            _horesARestarPerDiesQueNoTreballava = _horesARestarPerDiesQueNoTreballava + DiesQueHaFaltatIQueNoCoincideixAmbFestiuEmpresa * 8
    '        End If
    '    End If

    '    'Després mirarem si aquella setmana ha estat algun dia de baixa. 
    '    Dim DTBaixes As DataTable = Mod_Util.DiesDeBaixa(oDTC, pDataInici, pDataFi)
    '    Dim DiesDeBaixa As Integer

    '    If DTBaixes Is Nothing = False AndAlso DTBaixes.Rows.Count > 0 Then
    '        'Si ho ha estat i encanvi no hi havia cap dia festiu d'empresa multiplicarem * 8 el número de dies que ha estat ausent
    '        Dim i As Integer
    '        For i = 0 To DTBaixes.Rows.Count
    '            If LlistatFestiusEmpresa.Where(Function(F) F.Fecha = DTBaixes(0).Item("Data")).Count = 0 Then
    '                DiesDeBaixa = DiesDeBaixa + 1
    '            End If
    '        Next
    '    End If

    '    _Calcul = 40 - _horesARestarPerDiesQueNoTreballava - _TotalHoresTreballades - DiesDeBaixa * 8
    '    Me.T_HorasPendientes.Text = IIf(_Calcul < 0, 0, _Calcul)

    'End Sub

#Region "Grid Horas"

    Private Sub CargaGrid_Horas(ByVal pDataInici As Date, ByVal pDataFi As Date)
        Try
            'Dim pepe As New System.IO.StreamWriter("C:\PGM\Log.log")

            'oDTC.Log = pepe
            Dim _Horas As IEnumerable(Of Parte_Horas) = From taula In oDTC.Parte_Horas Where taula.Personal.ID_Personal = Seguretat.oUser.ID_Personal And taula.Fecha >= pDataInici And taula.Fecha <= pDataFi Order By taula.Fecha Select taula

            With Me.GRD_Horas
                '.GRID.DataSource = _Horas
                .GRID.DisplayLayout.MaxBandDepth = 1

                .M.clsUltraGrid.CargarIEnumerable(_Horas)


                .M_Editable()

                Call CargarCombo_Personal(.GRID)
                Call CargarCombo_TipoActuacion(.GRID)
                '.GRID.DisplayLayout.Bands(0).Columns("Parte").CellClickAction = CellClickAction.CellSelect
                '.GRID.DisplayLayout.Bands(0).Columns("Parte").Editor.CanEditType = False
                .GRID.DisplayLayout.Bands(0).Columns("Parte").Style = ColumnStyle.DropDownList
                'Call CargarCombo_Parte(False)
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub CargarCombo_Personal(ByVal pGrid As Infragistics.Win.UltraWinGrid.UltraGrid)
        Try

            Dim Valors As New Infragistics.Win.ValueList
            Dim oTaula As IQueryable(Of Personal) = (From Taula In oDTC.Personal Where Taula.Activo = True Order By Taula.Nombre Select Taula)

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

    'Private Sub CargarCombo_Parte(ByVal pNomesPendents As Boolean)
    '    Try
    '        'Dim DT As DataTable = BD.RetornaDataTable("SELECT ID_Parte, FechaInicio,  Descripcion as Tipo, TrabajoARealizar From Parte, Parte_Tipo Where Parte.ID_Parte_Tipo=Parte_Tipo.ID_Parte_Tipo and Parte.Activo=1 Order by ID_Parte")
    '        Dim DT As IEnumerable
    '        If pNomesPendents = True Then
    '            DT = From Taula In oDTC.Parte Where (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) And Taula.Activo = True Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Taula.TrabajoARealizar, Parte = Taula
    '        Else
    '            DT = From Taula In oDTC.Parte Where Taula.Activo = True Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Taula.TrabajoARealizar, Parte = Taula
    '        End If


    '        Me.C_Parte.DataSource = DT
    '        If DT Is Nothing Then
    '            Exit Sub
    '        End If

    '        With C_Parte
    '            '.DisplayLayout.Bands(0).Columns.Add("a")
    '            '.Rows(0).Cells("a").Value = oDTC.Parte.FirstOrDefault

    '            '.AutoCompleteMode = AutoCompleteMode.Suggest
    '            '.AutoSuggestFilterMode = AutoSuggestFilterMode.Contains
    '            .AutoCompleteMode = AutoCompleteMode.None
    '            .AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
    '            .MaxDropDownItems = 16
    '            .DisplayMember = "ID_Parte"
    '            .ValueMember = "Parte"
    '            .DisplayLayout.Bands(0).Columns("ID_Parte").Width = 50
    '            .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Caption = "Parte"
    '            .DisplayLayout.Bands(0).Columns("ID_Parte").CellAppearance.TextHAlign = HAlign.Right
    '            .DisplayLayout.Bands(0).Columns("FechaInicio").Width = 80
    '            .DisplayLayout.Bands(0).Columns("FechaInicio").Header.Caption = "Fecha"
    '            .DisplayLayout.Bands(0).Columns("FechaInicio").CellAppearance.TextHAlign = HAlign.Center
    '            .DisplayLayout.Bands(0).Columns("Tipo").Width = 80
    '            .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Appearance.TextHAlign = HAlign.Right
    '            .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Width = 400
    '            .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Header.Caption = "Descripción"
    '            .DisplayLayout.Bands(0).Columns("Parte").Hidden = True


    '        End With
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    Private Sub CargarCombo_Parte2(ByVal pNomesPendents As Boolean, ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo)
        Try
            'Dim DT As DataTable = BD.RetornaDataTable("SELECT ID_Parte, FechaInicio,  Descripcion as Tipo, TrabajoARealizar From Parte, Parte_Tipo Where Parte.ID_Parte_Tipo=Parte_Tipo.ID_Parte_Tipo and Parte.Activo=1 Order by ID_Parte")
            Dim DT As IEnumerable
            If pNomesPendents = True Then
                DT = From Taula In oDTC.Parte Where Taula.Parte_Asignacion.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 And Taula.Activo = True And Taula.BloquearImputacionHoras = False And (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Cliente = Taula.Cliente.Nombre, Poblacion = Taula.Instalacion.Poblacion, Taula.TrabajoARealizar, Parte = Taula
            Else
                DT = From Taula In oDTC.Parte Where Taula.Activo = True Select Taula.ID_Parte, Taula.FechaInicio, Tipo = Taula.Parte_Tipo.Descripcion, Cliente = Taula.Cliente.Nombre, Poblacion = Taula.Poblacion, Taula.TrabajoARealizar, Parte = Taula
            End If
            pCombo.DisplayLayout.MaxBandDepth = 1
            pCombo.DataSource = DT
            If DT Is Nothing Then
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
                .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Caption = "Parte"
                .DisplayLayout.Bands(0).Columns("ID_Parte").CellAppearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("ID_Parte").Header.Appearance.TextHAlign = HAlign.Right
                .DisplayLayout.Bands(0).Columns("FechaInicio").Width = 80
                .DisplayLayout.Bands(0).Columns("FechaInicio").Header.Caption = "Fecha"
                .DisplayLayout.Bands(0).Columns("FechaInicio").CellAppearance.TextHAlign = HAlign.Center
                .DisplayLayout.Bands(0).Columns("Tipo").Width = 80
                .DisplayLayout.Bands(0).Columns("Cliente").Width = 200
                .DisplayLayout.Bands(0).Columns("Cliente").Header.Caption = "Cliente"
                .DisplayLayout.Bands(0).Columns("Poblacion").Width = 200
                .DisplayLayout.Bands(0).Columns("Poblacion").Header.Caption = "Población"

                .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Width = 400
                .DisplayLayout.Bands(0).Columns("TrabajoARealizar").Header.Caption = "Descripción"
                .DisplayLayout.Bands(0).Columns("Parte").Hidden = True
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    'Private Sub GRD_Horas_M_GRID_BeforeCellListDropDown(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_Horas.M_GRID_BeforeCellListDropDown
    '    Call CargarCombo_Parte(True)
    'End Sub

    Private Sub GRD_Horas_M_GRID_BeforeCellUpdate(sender As Object, e As BeforeCellUpdateEventArgs) Handles GRD_Horas.M_GRID_BeforeCellUpdate
        '    If e.NewValue > 8 Then
        '        Mensaje.Mostrar_Mensaje("Imposible asignar más de 8 horas", M_Mensaje.Missatge_Modo.INFORMACIO, "")
        '        e.Cancel = True
        '    End If
        'End If
        If e.Cell.Column.Key = "Fecha" Then
            If IsDate(e.NewValue) Then
                If Util.RetornaNumeroDeLaSetmana(e.NewValue) <> Me.C_Semana.Value Then
                    Mensaje.Mostrar_Mensaje("Imposible cambiar la fecha. La fecha tiene que ser de la semana: " & Me.C_Semana.Value, M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    e.Cancel = True
                    Exit Sub
                End If
            End If
        End If

    End Sub

    'Private Sub GRD_Horas_M_Grid_InitializeLayout(Sender As Object, e As InitializeLayoutEventArgs) Handles GRD_Horas.M_Grid_InitializeLayout
    '    '   e.Layout.Bands(0).Columns("Parte").EditorComponent = Me.C_Parte
    'End Sub

    Private Sub GRD_Horas_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Horas.M_Grid_InitializeRow
        Dim _Horas As Parte_Horas = e.Row.ListObject
        Dim _Combo As New Infragistics.Win.UltraWinGrid.UltraCombo
        If e.Row.IsAddRow = False Then
            If _Horas.Parte.ID_Parte_Estado = EnumParteEstado.Finalizado Or _Horas.Parte.BloquearImputacionHoras = True Then
                e.Row.Activation = Activation.Disabled
            End If
            If _Horas.Parte.ID_Parte_Estado = EnumParteEstado.Finalizado Or _Horas.Parte.BloquearImputacionHoras = True Then
                Call CargarCombo_Parte2(False, _Combo)
            Else
                Call CargarCombo_Parte2(True, _Combo)
            End If
        Else
            Call CargarCombo_Parte2(True, _Combo)
        End If

        e.Row.Cells("Parte").EditorComponent = Nothing
        e.Row.Cells("Parte").EditorComponent = _Combo
        'e.Row.Band.Columns("Parte").EditorComponent = _Combo
    End Sub

    'Private Sub CargarCombo_Planta(ByRef pCelda As Infragistics.Win.UltraWinGrid.UltraGridCell, ByVal pIDEmplazamiento As Integer, Optional ByVal pItemBuitEsNull As Boolean = False)
    '    Try

    '        Dim Valors As New Infragistics.Win.ValueList
    '        Dim oTaula As IQueryable(Of Instalacion_Emplazamiento_Planta) = (From Taula In oDTC.Instalacion_Emplazamiento_Planta Where Taula.ID_Instalacion_Emplazamiento = pIDEmplazamiento Order By Taula.Descripcion Select Taula)
    '        Dim Var As Instalacion_Emplazamiento_Planta


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

    Private Sub GRD_Horas_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Horas.M_ToolGrid_ToolAfegir
        Try
            With Me.GRD_Horas

                'Si no hi ha dades a DT_Lunes vol dir que encara no hem carregat les dades
                If Me.DT_Lunes.Value Is Nothing Then
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                'Fem aquest if pq si apretes el botó afegir mentres estàs visualitzant unes dades de fa temps, quan apretis el botó afegir et carregui el dilluns de la setmana que estas visualitzant
                If Util.RetornaNumeroDeLaSetmana(Now.Date) = Me.C_Semana.Value Then
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("Fecha").Value = Now.Date
                Else
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("Fecha").Value = Me.DT_Lunes.Value
                End If

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte_Horas_Estado").Value = oDTC.Parte_Horas_Estado.Where(Function(F) F.ID_Parte_Horas_Estado = EnumParteHorasEstado.Correcto).FirstOrDefault
                If Seguretat.oUser.ID_Personal.HasValue = True Then 'si l'usuari actual està assignat a un "Personal"
                    .GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
                    '.GRID.Rows(.GRID.Rows.Count - 1).Cells("Personal").Value = oUser.Personal
                End If

            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Horas_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Horas.M_ToolGrid_ToolEliminarRow
        Try

            If e.IsAddRow Then
                'oLinqParte.Parte_Horas.Remove(e.ListObject)
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

            'Call CalcularHoresRealitzades()
            'Call CalcularCosteHoresRealitzades()
            'Call CalcularEstadoParte()

            oDTC.SubmitChanges()
            Dim _ID_Parte As Integer = e.Row.Cells("ID_Parte").Value
            Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = _ID_Parte).FirstOrDefault
            _Parte.HorasRealizadas = _Parte.RetornaHoresRealizadas
            _Parte.CosteImputadoMO = _Parte.RetornaCosteImputadoMO
            oDTC.SubmitChanges()


            If _Parte.Parte_Horas.Count > 0 Or _Parte.Parte_Gastos.Count > 0 Or _Parte.Parte_Incidencia.Count > 0 Then
                _Parte.Parte_Estado = oDTC.Parte_Estado.Where(Function(F) F.ID_Parte_Estado = CInt(EnumParteEstado.EnCurso)).FirstOrDefault
                If _Parte.FechaInicio.HasValue = False Then
                    _Parte.FechaInicio = Now.Date
                End If
            Else
                _Parte.Parte_Estado = oDTC.Parte_Estado.Where(Function(F) F.ID_Parte_Estado = CInt(EnumParteEstado.Pendiente)).FirstOrDefault
            End If
            oDTC.SubmitChanges()

            Call CalculHoresPendents(Me.DT_Lunes.Value, Me.DT_Domingo.Value)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Horas_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_Horas.M_ToolGrid_ToolClickBotonsExtras
        If e.Tool.Key = "IrAlParte" Then
            If Me.GRD_Horas.GRID.ActiveRow Is Nothing = False Then
                Dim frm As New frmParte
                frm.Entrada(Me.GRD_Horas.GRID.ActiveRow.Cells("ID_Parte").Value)
                frm.FormObrir(Me, False)
            End If
        End If
    End Sub

#End Region

#Region "Grid HorasIncorrectes"

    Private Sub CargaGrid_HorasIncorrectes()
        Try
            Dim _Horas As IEnumerable(Of Parte_Horas) = From taula In oDTC.Parte_Horas Where taula.Personal.ID_Personal = Seguretat.oUser.ID_Personal And taula.ID_Parte_Horas_Estado = EnumParteHorasEstado.Incorrecto Order By taula.Fecha Select taula

            With Me.GRD_HorasIncorrectas
                '.GRID.DataSource = _Horas
                .M.clsUltraGrid.CargarIEnumerable(_Horas)
                .GRID.DisplayLayout.Bands(0).Columns("Fecha").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("ID_Parte").CellActivation = Activation.NoEdit
                .GRID.DisplayLayout.Bands(0).Columns("ObservacionesIncorrecto").CellActivation = Activation.NoEdit
                .M_Editable()

                If .GRID.Rows.Count = 0 Then
                    Me.Tab_Principal.Tabs("Incidencias").Text = "Incidencias"
                    Util.TabPintarHeaderTab(Me.Tab_Principal.Tabs("Incidencias"), Me.Tab_Principal.Tabs(0).Appearance.BackColor)
                Else
                    Me.Tab_Principal.Tabs("Incidencias").Text = "Incidencias (" & .GRID.Rows.Count & ")"
                    Util.TabPintarHeaderTab(Me.Tab_Principal.Tabs("Incidencias"), Color.LightGreen)
                End If
            End With


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_HorasIncorrectas_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_HorasIncorrectas.M_GRID_AfterRowUpdate
        Try

            oDTC.SubmitChanges()
            Call CalculHoresPendents(Me.DT_Lunes.Value, Me.DT_Domingo.Value)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_HorasIncorrectas_M_ToolGrid_ToolClickBotonsExtras(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs) Handles GRD_HorasIncorrectas.M_ToolGrid_ToolClickBotonsExtras
        If Me.GRD_HorasIncorrectas.GRID.Selected.Rows.Count = 1 Then
            If e.Tool.Key = "Arreglada" Then
                If Mensaje.Mostrar_Mensaje("Desea dar por arreglada la imputacion de horas", M_Mensaje.Missatge_Modo.PREGUNTA, , , True) = M_Mensaje.Botons.SI Then
                    Dim _Horas As Parte_Horas
                    _Horas = Me.GRD_HorasIncorrectas.GRID.Selected.Rows(0).ListObject
                    _Horas.Parte_Horas_Estado = oDTC.Parte_Horas_Estado.Where(Function(F) F.ID_Parte_Horas_Estado = EnumParteHorasEstado.Reparado).FirstOrDefault
                    oDTC.SubmitChanges()
                    Call CargaGrid_HorasIncorrectes()
                End If
            End If
            If e.Tool.Key = "IrAlParte" Then
                Dim frm As New frmParte
                frm.Entrada(Me.GRD_HorasIncorrectas.GRID.Selected.Rows(0).Cells("ID_Parte").Value)
                frm.FormObrir(Me, False)
            End If
        End If
    End Sub

#End Region

#Region "Grid Partes para informes técnicos"

    Private Sub CargaGrid_Partes()
        Try
            'Dim _Partes As IEnumerable(Of Parte_Horas) = From taula In oDTC.Parte_Horas Where taula.Personal.ID_Personal = Seguretat.oUser.ID_Personal And taula.Fecha >= pDataInici And taula.Fecha <= pDataFi Order By taula.Fecha Select taula
            Dim DT As DataTable = BD.RetornaDataTable("Select C_Parte.*, cast(isnull(len(ObservacionesTecnico),0) as bit) as ObservacionesTecnicoIntroducidas, cast(isnull(len(ExplicacionHorasTecnico),0) as bit) as ExplicacionHorasTecnicoIntroducidas  From C_Parte, Parte_Aux Where C_Parte.ID_Parte=Parte_Aux.ID_Parte and NoPermitirModificarInformeTecnico=0 and ID_Personal = " & Seguretat.oUser.ID_Personal & " And ID_Parte_Estado <>" & EnumParteEstado.Finalizado & " Order By FechaInicio")

            With Me.GRD_Informe
                '.GRID.DataSource = _Horas
                .M.clsUltraGrid.Cargar(DT)

                .M_NoEditable()
                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    'Private Sub GRD_Informe_M_GRID_Click(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Informe.M_GRID_Click
    '    If Me.GRD_Informe.GRID.Selected.Rows.Count = 0 Then
    '        Me.R_Informe.pText = ""
    '        Me.R_ExplicacionHoras.pText = ""
    '    End If
    'End Sub

    Private Sub GRD_Informe_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Informe.M_GRID_DoubleClickRow2
        Dim _IDParte As Integer = CInt(e.Cells("ID_Parte").Value)
        Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = _IDParte).FirstOrDefault
        Dim frm As New frmImputacionHoras_Informes
        frm.Entrada(_Parte, oDTC)
        frm.FormObrir(Me, True)
        AddHandler frm.AlTancarForm, AddressOf AlTancarFormInformes

    End Sub

    Private Sub AlTancarFormInformes()
        Call CargaGrid_Partes()
    End Sub

    'Private Sub B_Guardar_Click(sender As Object, e As EventArgs) Handles B_Guardar.Click
    '    If Me.GRD_Informe.GRID.Selected.Rows.Count <> 1 Then
    '        Exit Sub
    '    End If
    '    Dim _DTC As New DTCDataContext(BD.Conexion)

    '    Dim _Parte As Parte = _DTC.Parte.Where(Function(F) F.ID_Parte = CInt(Me.GRD_Informe.GRID.Selected.Rows(0).Cells("ID_Parte").Value)).FirstOrDefault
    '    _Parte.Parte_Aux.ObservacionesTecnico = Me.R_Informe.pText
    '    _Parte.Parte_Aux.ExplicacionHorasTecnico = Me.R_ExplicacionHoras.pText
    '    _DTC.SubmitChanges()
    '    Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE, "")


    '    _DTC.Dispose()
    '    _Parte = Nothing
    '    _DTC = Nothing

    '    ' Dim _ClsNotificacion As New clsNotificacion(oDTC)
    '    '_ClsNotificacion.CrearNotificacion_AlAñadirOModificarUnInformeTecnicoDesdeImputacionDeTecnicos(_Parte)
    'End Sub

#End Region

#Region "Grid Partes Ficheros"

    Private Sub CargaGrid_PartesFicheros()
        Try
            'Dim _Partes As IEnumerable(Of Parte_Horas) = From taula In oDTC.Parte_Horas Where taula.Personal.ID_Personal = Seguretat.oUser.ID_Personal And taula.Fecha >= pDataInici And taula.Fecha <= pDataFi Order By taula.Fecha Select taula
            Dim DT As DataTable = BD.RetornaDataTable("Select * From C_Parte Where ID_Parte in (Select ID_Parte From Parte_Asignacion Where ID_Personal=" & Seguretat.oUser.ID_Personal & ") And ID_Parte_Estado <>" & EnumParteEstado.Finalizado & " Order By FechaInicio")


            With Me.GRD_Partes_Ficheros
                '.GRID.DataSource = _Horas
                .M.clsUltraGrid.Cargar(DT)

                .M_NoEditable()
                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
                ' Me.R_Informe.pText = ""
                'Me.R_ExplicacionHoras.pText = ""
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ParteFicheros_M_GRID_Click(ByRef sender As UltraGrid, ByRef e As EventArgs) Handles GRD_Partes_Ficheros.M_GRID_Click
        If Me.GRD_Partes_Ficheros.GRID.Selected.Rows.Count = 0 Then
            Fichero.Cargar_GRID(0)
        End If
    End Sub

    Private Sub GRD_ParteFicheros_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Partes_Ficheros.M_GRID_ClickRow2
        Dim _IDParte As Integer = CInt(e.Cells("ID_Parte").Value)
        Fichero.Cargar_GRID(_IDParte)
    End Sub

#End Region

#Region "Grid Partes Actuales"

    Private Sub CargaGrid_PartesActuales()
        Try

            ' Taula.Parte_Asignacion.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 And Taula.Activo = True And Taula.BloquearImputacionHoras = False And (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) 
            'Dim _Partes As IEnumerable(Of Parte_Horas) = From taula In oDTC.Parte_Horas Where taula.Personal.ID_Personal = Seguretat.oUser.ID_Personal And taula.Fecha >= pDataInici And taula.Fecha <= pDataFi Order By taula.Fecha Select taula
            Dim DT As DataTable = BD.RetornaDataTable("Select * From C_Parte Where  ID_Parte in (Select ID_Parte From Parte_Asignacion Where id_personal = " & Seguretat.oUser.ID_Personal & ") And ID_Parte_Estado <>" & EnumParteEstado.Finalizado & " and BloquearImputacionMaterial = 0  Order By FechaInicio")

            With Me.GRD_PartesActuales
                '.GRID.DataSource = _Horas
                .M.clsUltraGrid.Cargar(DT)

                .M_NoEditable()
                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
                ' Me.R_Informe.pText = ""
                ' Me.R_ExplicacionHoras.pText = ""
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_PartesActuales_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_PartesActuales.M_GRID_ClickRow2
        'Dim _IDParte As Integer = CInt(e.Cells("ID_Parte").Value)
        'Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = _IDParte).FirstOrDefault
        'Me.R_Informe.pText = _Parte.ObservacionesTecnico
        Call CargaGrid_Material(e.Cells("ID_Parte").Value)
        Call CargaGrid_StockEnElParte(e.Cells("ID_Parte").Value)
    End Sub

#End Region

#Region "Grid Material"

    Private Sub CargaGrid_Material(ByVal pID_Parte As Integer)
        Try
            'Dim _Horas As IEnumerable(Of Parte_MaterialOperarios) = From taula In oDTC.Parte_MaterialOperarios Where taula.ID_Parte = pID_Parte Order By taula.FechaAlta Select taula
            Dim _DT As DataTable = BD.RetornaDataTable("Select * From C_Parte_MaterialOperarios Where ID_Parte=" & pID_Parte & " Order by FechaAlta")

            With Me.GRD_Material

                .M.clsUltraGrid.Cargar(_DT)

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Material_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_Material.M_ToolGrid_ToolEliminarRow
        Try


            If e Is Nothing = True Then
                Exit Sub
            End If

            If e.IsAddRow = True Then
                Exit Sub
            End If

            If Me.GRD_PartesActuales.GRID.ActiveRow Is Nothing Then
                Exit Sub
            End If

            Dim _IDParteMaterial As Integer = e.Cells("ID_Parte_MaterialOperarios").Value
            Dim _Parte_Material As Parte_MaterialOperarios = oDTC.Parte_MaterialOperarios.Where(Function(F) F.ID_Parte_MaterialOperarios = _IDParteMaterial).FirstOrDefault

            Dim _IDProducto As Integer = e.Cells("ID_Producto").Value
            Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = CInt(Me.GRD_PartesActuales.GRID.ActiveRow.Cells("ID_Parte").Value)).FirstOrDefault
            Dim _AlmacenPersonal As Almacen = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Almacen.FirstOrDefault
            Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(_IDProducto)).FirstOrDefault

            If _Producto.RequiereNumeroSerie = True Then
                Mensaje.Mostrar_Mensaje("Imposible eliminar líneas con números de serie", M_Mensaje.Missatge_Modo.INFORMACIO, "")
                Exit Sub
            End If

            'Borrem la línea
            If Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE_PREGUNTA, "") = M_Mensaje.Botons.NO Then
                Exit Sub
            End If

            Call CrearTraspaso(_AlmacenPersonal, _Parte.Almacen.FirstOrDefault, _Producto, e.Cells("Cantidad").Value, True)
            oDTC.Parte_MaterialOperarios.DeleteOnSubmit(_Parte_Material)

            e.Delete((False))
            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_ELIMINAR_REGISTRE)
            oDTC.SubmitChanges()

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub CrearTraspaso(ByRef pMagatzemOrigen As Almacen, pMagatzemDesti As Almacen, ByRef pProducte As Producto, ByVal pQuantitat As Decimal, ByVal pRectificacio As Boolean, Optional ByVal pID_NS As Integer = 0, Optional ByRef pRow As UltraGridRow = Nothing)
        Dim _MagatzemOrigen As Almacen
        Dim _MagatzemDesti As Almacen

        If pRectificacio = True Then 'Si el que estem fent és corregir un material assignat a un parte llavors el origen i destí s'inverteixen
            _MagatzemOrigen = pMagatzemDesti
            _MagatzemDesti = pMagatzemOrigen
        Else
            _MagatzemOrigen = pMagatzemOrigen
            _MagatzemDesti = pMagatzemDesti
        End If



        Dim _NewDocument As New Entrada
        _NewDocument.Almacen = _MagatzemOrigen
        _NewDocument.Almacen_Destino = pMagatzemDesti '_Parte.Almacen.FirstOrDefault
        _NewDocument.Descripcion = "Traspaso entre almacenes (Almacen personal - Parte)"
        _NewDocument.FechaAlta = Now.Date
        _NewDocument.FechaEntrada = Now.Date
        _NewDocument.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault
        _NewDocument.Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = EnumEntradaTipo.TraspasoAlmacen).FirstOrDefault
        _NewDocument.Empresa = oEmpresa
        Dim _clsNewDocument As New clsEntrada(oDTC, _NewDocument)
        _NewDocument.Codigo = clsEntrada.RetornaCodiSeguent(EnumEntradaTipo.TraspasoAlmacen)
        oDTC.SubmitChanges()

        Dim _Linea As New Entrada_Linea
        With _Linea
            .Almacen = _MagatzemOrigen
            .Producto = pProducte
            .Descripcion = pProducte.Descripcion
            .FechaEntrada = Now.Date
            .FechaEntrega = Now.Date
            .Unidad = pQuantitat * -1
            .NoRestarStock = False
            .NoImprimirLinea = False
        End With

        _NewDocument.Entrada_Linea.Add(_Linea)

        Dim _ClsLinea As New clsEntradaLinea(_NewDocument, _Linea, oDTC)
        If _ClsLinea.AfegirLinea(True) = True Then
            If _ClsLinea.pRequiereNS = True Then
                _ClsLinea.LineaNSCrear(pID_NS)
            End If
            oDTC.SubmitChanges()

            If pRow Is Nothing = False Then
                pRow.Cells("Entrada_Linea").Value = _ClsLinea.oLinqLinea
            End If

            Dim _clsLinea2 As New clsEntradaLinea(_NewDocument, , oDTC)
            'If _ClsLinea.pRequiereNS = True Then 'Si requereix números de serie encara no duplicarem la línea ja que per introduir números de serie primer s'ha de guardar la línea per collons

            If pRectificacio = True Then 'Fem aquesta trampa pq si és una rectificació m'ho envia al revés del que vull
                _ClsLinea.oLinqEntrada.Almacen_Destino = _MagatzemDesti
            End If

            _ClsLinea.TraspasoAlmacenesRetornaNovaLineaDeSortida(_clsLinea2.oLinqLinea)
            oDTC.SubmitChanges()

            If pRow Is Nothing = False Then
                pRow.Cells("Entrada_Linea1").Value = _clsLinea2.oLinqLinea
            End If
            'End If
        End If

        'oDTC.SubmitChanges()


    End Sub


    Private Sub CargaGrid_StockEnElParte(ByVal pIDParte As Integer)
        Try

            Dim _IDMagatzem As Integer = 0
            If pIDParte <> 0 Then
                Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = pIDParte).FirstOrDefault
                If _Parte Is Nothing = False AndAlso _Parte.Almacen.FirstOrDefault Is Nothing = False Then
                    _IDMagatzem = _Parte.Almacen.FirstOrDefault.ID_Almacen
                End If
            End If



            Dim _Stock As IEnumerable(Of RetornaStockResult) = From taula In oDTC.RetornaStock(0, _IDMagatzem)

            With Me.GRD_MagatzemParte
                ' Me.T_ImporteStock.Value = _Stock.Sum(Function(F) F.ImportePVPStockReal)
                .M.clsUltraGrid.CargarIEnumerable(_Stock)
                '.M.clsUltraGrid.Cargar(DTS)
                ' .M_NoEditable()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_Material_M_ToolGrid_ToolClickBotonsExtras2(ByRef sender As UltraWinToolbars.UltraToolbarsManager, ByRef e As UltraWinToolbars.ToolClickEventArgs, ByRef pGrid As M_UltraGrid.m_UltraGrid) Handles GRD_Material.M_ToolGrid_ToolClickBotonsExtras2
        If Me.GRD_PartesActuales.GRID.ActiveRow Is Nothing = True Then
            Exit Sub
        End If
        Dim _IDParte As Integer = CInt(Me.GRD_PartesActuales.GRID.ActiveRow.Cells("ID_Parte").Value)
        Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = _IDParte).FirstOrDefault
        Dim frm As New frmImputacionHoras_Materiales
        frm.Entrada(oDTC, _Parte)
        frm.FormObrir(Me, True)
        AddHandler frm.AlTancarForm, AddressOf AlTancarFormMateriales

    End Sub


    Private Sub AlTancarFormMateriales()
        Call CargaGrid_Material(Me.GRD_PartesActuales.GRID.ActiveRow.Cells("ID_Parte").Value)
    End Sub

    'Private Sub GRD_Material_AfterRowUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.RowEventArgs) Handles GRD_Material.M_GRID_AfterRowUpdate
    '    Try
    '        If e.Row.Cells("Cantidad").Value = 0 Then
    '            e.Row.Delete(False)
    '            Exit Sub
    '        End If

    '        If e.Row.Cells("ID_Parte_MaterialOperarios").Value <> 0 Then 'Fem això pq l'event afterowupdate salta avegades sense voler
    '            Exit Sub
    '        End If

    '        Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = CInt(Me.GRD_PartesActuales.GRID.ActiveRow.Cells("ID_Parte").Value)).FirstOrDefault
    '        Dim _AlmacenPersonal As Almacen = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Almacen.FirstOrDefault
    '        Dim _Producto As Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = CInt(e.Row.Cells("ID_Producto").Value)).FirstOrDefault

    '        Dim _Stock As RetornaStockResult = oDTC.RetornaStock(_Producto.ID_Producto, _AlmacenPersonal.ID_Almacen).FirstOrDefault
    '        If _Stock.StockReal < CDbl(e.Row.Cells("Cantidad").Value) Then
    '            Mensaje.Mostrar_Mensaje("Imposible guardar, el stock introducido supera el stock del almacén", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '            e.Row.Delete(False)
    '            Exit Sub
    '        End If

    '        If _Producto.RequiereNumeroSerie = True Then
    '            If e.Row.Cells("NS").Value Is Nothing Then
    '                Mensaje.Mostrar_Mensaje("Imposible guardar, el producto seleccionado requiere la selección de un número de serie", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                Exit Sub
    '            End If
    '        End If

    '        Dim _IDNS As Integer = 0
    '        If _Producto.RequiereNumeroSerie = True Then
    '            _IDNS = e.Row.Cells("ID_NS").Value
    '        End If
    '        Call CrearTraspaso(_AlmacenPersonal, _Parte.Almacen.FirstOrDefault, _Producto, e.Row.Cells("Cantidad").Value, False, _IDNS, e.Row)

    '        Dim _ClsNotificacion As New clsNotificacion(oDTC)
    '        _ClsNotificacion.CrearNotificacion_AlAssignarMaterialUnTecnicoAUnAlmacenDesdeImputacionDeTecnicos(_Parte)

    '        'Dim _NewDocument As New Entrada
    '        '_NewDocument.Almacen = _AlmacenPersonal
    '        '_NewDocument.Almacen_Destino = _Parte.Almacen.FirstOrDefault
    '        '_NewDocument.Descripcion = "Traspaso entre almacenes (Almacen personal - Parte)"
    '        '_NewDocument.FechaAlta = Now.Date
    '        '_NewDocument.FechaEntrada = Now.Date
    '        '_NewDocument.Entrada_Estado = oDTC.Entrada_Estado.Where(Function(F) F.ID_Entrada_Estado = EnumEntradaEstado.Cerrado).FirstOrDefault
    '        '_NewDocument.Entrada_Tipo = oDTC.Entrada_Tipo.Where(Function(F) F.ID_Entrada_Tipo = EnumEntradaTipo.TraspasoAlmacen).FirstOrDefault

    '        'Dim _clsNewDocument As New clsEntrada(oDTC, _NewDocument)
    '        '_NewDocument.Codigo = clsEntrada.RetornaCodiSeguent(EnumEntradaTipo.TraspasoAlmacen)
    '        'oDTC.SubmitChanges()

    '        'Dim _Linea As New Entrada_Linea
    '        'With _Linea
    '        '    .Almacen = _AlmacenPersonal
    '        '    .Producto = _Producto
    '        '    .Descripcion = _Producto.Descripcion
    '        '    .FechaEntrada = Now.Date
    '        '    .FechaEntrega = Now.Date
    '        '    .Unidad = e.Row.Cells("Cantidad").Value * -1
    '        '    .NoRestarStock = False
    '        '    .NoImprimirLinea = False
    '        'End With

    '        '_NewDocument.Entrada_Linea.Add(_Linea)

    '        'Dim _ClsLinea As New clsEntradaLinea(_NewDocument, _Linea, oDTC)
    '        'If _ClsLinea.AfegirLinea(True) = True Then
    '        '    If _ClsLinea.pRequiereNS = True Then
    '        '        _ClsLinea.LineaNSCrear(e.Row.Cells("ID_NS").Value)
    '        '    End If
    '        '    oDTC.SubmitChanges()
    '        '    e.Row.Cells("Entrada_Linea").Value = _ClsLinea.oLinqLinea
    '        '    Dim _clsLinea2 As New clsEntradaLinea(_NewDocument, , oDTC)
    '        '    'If _ClsLinea.pRequiereNS = True Then 'Si requereix números de serie encara no duplicarem la línea ja que per introduir números de serie primer s'ha de guardar la línea per collons
    '        '    _ClsLinea.TraspasoAlmacenesRetornaNovaLineaDeSortida(_clsLinea2.oLinqLinea)
    '        '    oDTC.SubmitChanges()
    '        '    e.Row.Cells("Entrada_Linea1").Value = _clsLinea2.oLinqLinea
    '        '    'End If
    '        'End If

    '        'oDTC.SubmitChanges()

    '        'Dim _ClsNotificacion As New clsNotificacion(oDTC)
    '        '_ClsNotificacion.CrearNotificacion_AlAssignarMaterialUnTecnicoAUnAlmacenDesdeImputacionDeTecnicos(_Parte)


    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub


    'Private Sub CargarCombo_Producto(ByVal pNomesAmbStock As Boolean, ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo, Optional ByVal pIDProducto As Integer = 0)
    '    Try
    '        ' Dim oDTC As New DTCDataContext(BD.Conexion)
    '        ' oDTC.ObjectTrackingEnabled = False
    '        'oDTC.DeferredLoadingEnabled = True
    '        'Creem un array d'integers per emmagatzemar els ID's d'articles que te l'usuari actual en stock
    '        Dim _Almacen As Almacen = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Almacen.FirstOrDefault


    '        Dim _Result As RetornaStockResult
    '        Dim Total As Integer = oDTC.RetornaStock(0, _Almacen.ID_Almacen).Count
    '        Dim Llistat(0 To Total) As Integer

    '        Dim i As Integer = 0
    '        For Each _Result In oDTC.RetornaStock(0, _Almacen.ID_Almacen)
    '            i = i + 1
    '            Llistat(i) = _Result.ID_Producto
    '        Next

    '        Dim DT As IEnumerable
    '        'If pNomesAmbStock = True Then
    '        DT = From Taula In oDTC.Producto Where Llistat.Contains(Taula.ID_Producto) And Taula.Activo = True Or Taula.ID_Producto = pIDProducto Order By Taula.Descripcion Select Producto = Taula, Descripcion = Taula.Descripcion, ID_Producto = Taula.ID_Producto

    '        'Else
    '        'DT = From Taula In oDTC.Producto Where Taula.Activo = True Order By Taula.Descripcion Select Producto = Taula, Descripcion = Taula.Descripcion, ID_Producto = Taula.ID_Producto
    '        'End If
    '        'pCombo.DisplayLayout.ViewStyle = ViewStyle.SingleBand
    '        pCombo.DisplayLayout.MaxBandDepth = 1
    '        pCombo.DataSource = DT

    '        If DT Is Nothing Then
    '            Exit Sub
    '        End If

    '        With pCombo
    '            ' .AutoCompleteMode = AutoCompleteMode.None
    '            '.AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
    '            .AlwaysInEditMode = False
    '            .DropDownStyle = UltraComboStyle.DropDownList

    '            .MaxDropDownItems = 16
    '            .DisplayMember = "Descripcion"
    '            .ValueMember = "Producto"
    '            .DisplayLayout.Bands(0).Columns("Descripcion").Width = 430
    '            .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Producto"
    '            .DisplayLayout.Bands(0).Columns("Producto").Hidden = True
    '            .DisplayLayout.Bands(0).Columns("ID_Producto").Hidden = True
    '        End With

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    'Private Sub CargarCombo_NS(ByVal pNomesAmbStock As Boolean, ByRef pCombo As Infragistics.Win.UltraWinGrid.UltraCombo, ByRef pProducto As Producto, ByRef pRow As UltraGridRow)
    '    Try
    '        ' Dim oDTC As New DTCDataContext(BD.Conexion)
    '        ' oDTC.ObjectTrackingEnabled = False
    '        'oDTC.DeferredLoadingEnabled = True
    '        'Creem un array d'integers per emmagatzemar els ID's d'articles que te l'usuari actual en stock

    '        If pProducto Is Nothing Then
    '            Exit Sub
    '        End If
    '        Dim _IDProducto As Integer = pProducto.ID_Producto


    '        Dim _Almacen As Almacen = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault.Almacen.FirstOrDefault


    '        Dim _Result As NS
    '        Dim Total As Integer = oDTC.NS.Where(Function(F) F.ID_Almacen = _Almacen.ID_Almacen And F.ID_Producto = _IDProducto).Count 'oDTC.RetornaStock(pProducto.ID_Producto, _Almacen.ID_Almacen).FirstOrDefault.StockReal
    '        Dim Llistat(0 To Total) As Integer

    '        Dim i As Integer = 0
    '        For Each _Result In oDTC.NS.Where(Function(F) F.ID_Almacen = _Almacen.ID_Almacen And F.ID_Producto = _IDProducto)
    '            i = i + 1
    '            Llistat(i) = _Result.ID_NS
    '        Next

    '        Dim DT As IEnumerable
    '        If pNomesAmbStock = True Then
    '            DT = From Taula In oDTC.NS Where Llistat.Contains(Taula.ID_NS) And Taula.ID_NS_Estado = EnumNSEstado.Disponible Order By Taula.Descripcion Select NS = Taula, Descripcion = Taula.Descripcion
    '        Else
    '            Dim _IDNS As Integer = pRow.Cells("ID_NS").Value
    '            DT = From Taula In oDTC.NS Where Taula.ID_NS = _IDNS Order By Taula.Descripcion Select NS = Taula, Descripcion = Taula.Descripcion
    '        End If

    '        'pCombo.DisplayLayout.ViewStyle = ViewStyle.SingleBand
    '        pCombo.DisplayLayout.MaxBandDepth = 1
    '        pCombo.DataSource = DT

    '        If DT Is Nothing Then
    '            Exit Sub
    '        End If

    '        With pCombo
    '            ' .AutoCompleteMode = AutoCompleteMode.None
    '            '.AutoSuggestFilterMode = AutoSuggestFilterMode.StartsWith
    '            .AlwaysInEditMode = False
    '            .DropDownStyle = UltraComboStyle.DropDownList

    '            .MaxDropDownItems = 16
    '            .DisplayMember = "Descripcion"
    '            .ValueMember = "NS"
    '            .DisplayLayout.Bands(0).Columns("Descripcion").Width = 320
    '            .DisplayLayout.Bands(0).Columns("Descripcion").Header.Caption = "Número de serie"
    '            .DisplayLayout.Bands(0).Columns("NS").Hidden = True
    '        End With

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try

    'End Sub

    'Private Sub GRD_Material_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_Material.M_Grid_InitializeRow
    '    If e.ReInitialize = True Then
    '        Exit Sub
    '    End If
    '    Dim _Producto As Producto = e.Row.Cells("Producto").Value
    '    Dim _Combo As New Infragistics.Win.UltraWinGrid.UltraCombo
    '    AddHandler _Combo.InitializeRow, AddressOf ComboProducto_InitializeRow
    '    Dim _ComboNS As New Infragistics.Win.UltraWinGrid.UltraCombo

    '    If _Producto Is Nothing = False Then
    '        If _Producto.RequiereNumeroSerie = True Then
    '            e.Row.Cells("Cantidad").Activated = False
    '            e.Row.Cells("Cantidad").Activation = Activation.Disabled
    '            e.Row.Cells("Cantidad").Value = 1
    '            e.Row.Cells("NS").Activation = Activation.AllowEdit

    '        Else
    '            e.Row.Cells("Cantidad").Activated = True
    '            e.Row.Cells("Cantidad").Activation = Activation.AllowEdit
    '            e.Row.Cells("NS").Activation = Activation.Disabled
    '        End If
    '    End If

    '    If e.Row.IsAddRow = False Then
    '        Call CargarCombo_Producto(False, _Combo, e.Row.Cells("ID_Producto").Value)
    '        Call CargarCombo_NS(False, _ComboNS, _Producto, e.Row)
    '        e.Row.Activation = Activation.NoEdit

    '    Else
    '        Call CargarCombo_Producto(True, _Combo)
    '        If e.Row.Cells("NS").Value Is Nothing = True Then
    '            Call CargarCombo_NS(True, _ComboNS, _Producto, Nothing)
    '        End If
    '    End If

    '    e.Row.Cells("Producto").EditorComponent = Nothing
    '    e.Row.Cells("Producto").EditorComponent = _Combo

    '    e.Row.Cells("NS").EditorComponent = Nothing
    '    e.Row.Cells("NS").EditorComponent = _ComboNS

    '    'e.Row.Band.Columns("Parte").EditorComponent = _Combo
    'End Sub

    'Private Sub ComboProducto_InitializeRow(sender As Object, e As InitializeRowEventArgs)
    '    With Me.GRD_Material

    '        If e.Row.Band.Index = 0 AndAlso Me.GRD_Material.GRID.ActiveRow Is Nothing = False AndAlso Me.GRD_Material.GRID.ActiveRow.IsAddRow = True Then
    '            Dim _IDProducto As Integer = e.Row.Cells("ID_Producto").Value

    '            Dim _Producto As Producto
    '            _Producto = oDTC.Producto.Where(Function(F) F.ID_Producto = _IDProducto).FirstOrDefault

    '            If _Producto.Archivo Is Nothing = False AndAlso _Producto.Archivo.CampoBinario.Length > 0 Then
    '                e.Row.Cells("Descripcion").Appearance.Image = ResizeImage(Util.BinaryToImage(_Producto.Archivo.CampoBinario.ToArray), New Size(20, 10), True)
    '            End If

    '            '.GRID.DisplayLayout.Override.DefaultRowHeight = 40
    '            'Else
    '            .GRID.DisplayLayout.Override.DefaultRowHeight = 20
    '            ' End If
    '        End If
    '    End With
    'End Sub

    'Private Sub CargaGrid_Material(ByVal pID_Parte As Integer)
    '    Try
    '        Dim _Horas As IEnumerable(Of Parte_MaterialOperarios) = From taula In oDTC.Parte_MaterialOperarios Where taula.ID_Parte = pID_Parte Order By taula.FechaAlta Select taula

    '        With Me.GRD_Material
    '            '.GRID.DataSource = _Horas

    '            .M.clsUltraGrid.CargarIEnumerable(_Horas)

    '            .M_Editable()

    '            'Call CargarCombo_Personal(.GRID)
    '            'Call CargarCombo_TipoActuacion(.GRID)
    '            '.GRID.DisplayLayout.Bands(0).Columns("Parte").CellClickAction = CellClickAction.CellSelect
    '            '.GRID.DisplayLayout.Bands(0).Columns("Parte").Editor.CanEditType = False
    '            '.GRID.DisplayLayout.Bands(0).Columns("Parte").Style = ColumnStyle.DropDownList
    '            'Call CargarCombo_Parte(False)
    '        End With

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GRD_Material_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_Material.M_ToolGrid_ToolAfegir
    '    Try
    '        With Me.GRD_Material
    '            If Me.GRD_PartesActuales.GRID.ActiveRow Is Nothing Then
    '                Exit Sub
    '            End If


    '            If Seguretat.oUser.ID_Personal.HasValue = False Then
    '                Mensaje.Mostrar_Mensaje("Imposible crear una línea de material, el usuario actual no tiene asignado un objeto personal", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                Exit Sub
    '            End If
    '            Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
    '            If _Personal.Almacen Is Nothing = True Then
    '                Mensaje.Mostrar_Mensaje("Imposible crear una línea de material, el usuario actual no tiene asignado un almacén", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                Exit Sub
    '            End If
    '            Dim _Almacen As Almacen = _Personal.Almacen.FirstOrDefault

    '            Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = CInt(Me.GRD_PartesActuales.GRID.ActiveRow.Cells("ID_Parte").Value)).FirstOrDefault
    '            If _Parte.Almacen Is Nothing = True Then
    '                Mensaje.Mostrar_Mensaje("Imposible crear una línea de material, el parte seleccionado no tiene un almacén asignado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '                Exit Sub
    '            End If


    '            .M_ExitEditMode()
    '            .M_AfegirFila()

    '            .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte").Value = _Parte
    '            .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaAlta").Value = Now.Date

    '            'Fem una trampa pq guardi sense petar
    '            .GRID.Rows(.GRID.Rows.Count - 1).Cells("Entrada_Linea").Value = oDTC.Entrada_Linea.FirstOrDefault
    '            .GRID.Rows(.GRID.Rows.Count - 1).Cells("Entrada_Linea1").Value = oDTC.Entrada_Linea.FirstOrDefault

    '            'Call CargarCombo_Producto(Me.GRD_Material.GRID, _Almacen)


    '        End With
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub
#End Region

#Region "Grids To Do's"
    Private Sub CargaGrid_ToDo_Partes()
        Try
            'Dim _Partes As IEnumerable(Of Parte_Horas) = From taula In oDTC.Parte_Horas Where taula.Personal.ID_Personal = Seguretat.oUser.ID_Personal And taula.Fecha >= pDataInici And taula.Fecha <= pDataFi Order By taula.Fecha Select taula
            Dim DT As DataTable = BD.RetornaDataTable("Select * From C_Parte Where  ID_Personal = " & Seguretat.oUser.ID_Personal & " And ID_Parte_Estado <>" & EnumParteEstado.Finalizado & " Order By FechaInicio")

            With Me.GRD_ToDo_Partes
                '.GRID.DataSource = _Horas
                .M.clsUltraGrid.Cargar(DT)

                .M_NoEditable()
                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
                ' Me.R_Informe.pText = ""
                '  Me.R_ExplicacionHoras.pText = ""
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ToDo_Partes_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_ToDo_Partes.M_GRID_ClickRow2
        'Dim _IDParte As Integer = CInt(e.Cells("ID_Parte").Value)
        'Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = _IDParte).FirstOrDefault
        'Me.R_Informe.pText = _Parte.ObservacionesTecnico
        Call CargaGrid_ToDo(e.Cells("ID_Parte").Value)
    End Sub

    Private Sub CargaGrid_ToDo(ByVal pId As Integer)
        Try
            Dim _ToDo As IEnumerable(Of Parte_ToDo) = From taula In oDTC.Parte_ToDo Where taula.ID_Parte = pId Select taula

            With Me.GRD_ToDo
                .M.clsUltraGrid.CargarIEnumerable(_ToDo)
                .M_Editable()

                ' Call CargarCombo_Usuario(.GRID)
                Me.GRD_ToDo.GRID.DisplayLayout.Bands(0).Columns("Usuario").CellActivation = Activation.Disabled

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

    Private Sub GRD_ToDo_M_GRID_BeforeCellActivate(ByRef sender As UltraGrid, ByRef e As CancelableCellEventArgs) Handles GRD_ToDo.M_GRID_BeforeCellActivate
        If e.Cell.Column.Key = "Usuario" Then
            Call CargarCombo_Usuario(sender, e.Cell, False)
        End If
    End Sub

    Private Sub GRD_ToDo_M_Grid_InitializeRow(Sender As Object, e As InitializeRowEventArgs) Handles GRD_ToDo.M_Grid_InitializeRow
        If e.ReInitialize = False Then
            Call CargarCombo_Usuario(Sender, e.Row.Cells("Usuario"), True)
        End If
    End Sub

    Private Sub GRD_ToDo_M_ToolGrid_ToolAfegir(ByRef sender As Infragistics.Win.UltraWinToolbars.UltraToolbarsManager, ByRef e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles GRD_ToDo.M_ToolGrid_ToolAfegir
        Try

            With Me.GRD_ToDo
                Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = CInt(Me.GRD_ToDo_Partes.GRID.ActiveRow.Cells("ID_Parte").Value)).FirstOrDefault
                If _Parte.Almacen Is Nothing = True Then
                    Mensaje.Mostrar_Mensaje("Imposible crear una línea de material, el parte seleccionado no tiene un almacén asignado", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                    Exit Sub
                End If

                .M_ExitEditMode()
                .M_AfegirFila()

                .GRID.Rows(.GRID.Rows.Count - 1).Cells("FechaAlta").Value = Now.Date
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Parte").Value = _Parte
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Realizado").Value = False
                .GRID.Rows(.GRID.Rows.Count - 1).Cells("Usuario").Value = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault
                Call CargarCombo_Usuario(Me.GRD_ToDo.GRID, .GRID.Rows(.GRID.Rows.Count - 1).Cells("Usuario"), True)
            End With
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Private Sub GRD_ToDo_M_ToolGrid_ToolEliminarRow(ByRef sender As UltraGrid, ByRef e As UltraGridRow) Handles GRD_ToDo.M_ToolGrid_ToolEliminarRow
        Try


            If e.IsAddRow Then
                Dim _Parte As Parte = oDTC.Parte.Where(Function(F) F.ID_Parte = CInt(Me.GRD_ToDo_Partes.GRID.ActiveRow.Cells("ID_Parte").Value)).FirstOrDefault
                _Parte.Parte_ToDo.Remove(e.ListObject)
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
            _ClsNotificacion.CrearNotificacion_AlAñadirUnToDoDesdeLaPantallaImputacionDeTecnicos(_Parte)
        End If
    End Sub


#End Region

#Region "Grid Stock Personal"
    Private Sub CargaGrid_StockPersonal()
        Try
            If Seguretat.oUser.ID_Personal.HasValue = False Then 'Si l'usuari actual no te associat cap personal
                Exit Sub
            End If

            Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault

            If _Personal.Almacen.Count = 0 Then 'Si el personal del usuari actual no te assignat cpa almacen
                Exit Sub
            End If

            Dim _Stock As IEnumerable(Of RetornaStockResult) = From taula In oDTC.RetornaStock(0, _Personal.Almacen.FirstOrDefault.ID_Almacen)

            'Dim DT As New DataTable
            'DT.Columns.Add("ID_Producto", GetType(Integer))
            'DT.Columns.Add("ID_Almacen", GetType(Integer))
            'DT.Columns.Add("StockReal", GetType(Decimal))
            'DT.Columns.Add("StockTeorico", GetType(Integer))
            'DT.Columns.Add("ProductoCodigo", GetType(String))
            'DT.Columns.Add("ProductoDescripcion", GetType(String))
            'DT.Columns.Add("AlmacenDescripcion", GetType(String))
            'DT.Columns.Add("ImporteStockReal", GetType(Decimal))
            'DT.Columns.Add("ImporteStockTeorico", GetType(Decimal))
            'DT.Columns.Add("ImportePVPStockReal", GetType(Decimal))

            'Dim DTBanda2 As New DataTable
            'DTBanda2.Columns.Add("ID_Producto", GetType(Integer))
            'DTBanda2.Columns.Add("Codigo", GetType(Integer))
            'DTBanda2.Columns.Add("Descripcion", GetType(String))
            'DTBanda2.Columns.Add("Fecha", GetType(Date))
            'DTBanda2.Columns.Add("Cantidad", GetType(Date))
            'DTBanda2.Columns.Add("AlmacenDestino", GetType(String))



            'Dim _LineaStock As RetornaStockResult
            'For Each _LineaStock In _Stock
            '    Dim DtRow As DataRow = DT.NewRow
            '    With DtRow
            '        .Item("ID_Producto") = _LineaStock.ID_Producto
            '        .Item("ID_Almacen") = _LineaStock.ID_Almacen
            '        .Item("StockReal") = _LineaStock.StockReal
            '        .Item("StockTeorico") = _LineaStock.StockTeorico
            '        .Item("ProductoCodigo") = _LineaStock.ProductoCodigo
            '        .Item("ProductoDescripcion") = _LineaStock.ProductoDescripcion
            '        .Item("AlmacenDescripcion") = _LineaStock.AlmacenDescripcion
            '        .Item("ImporteStockReal") = _LineaStock.ImporteStockReal
            '        .Item("ImporteStockTeorico") = _LineaStock.ImporteStockTeorico
            '        .Item("ImportePVPStockReal") = _LineaStock.ImportePVPStockReal
            '    End With
            '    DT.Rows.Add(DtRow)
            'Next

            'DTBanda2 = BD.RetornaDataTable("Select * From C_Imputacion_MaterialTecnicos Where ID_Almacen=" & _Personal.Almacen.FirstOrDefault.ID_Almacen)

            'Dim DTS As New DataSet
            'BD.CargarDataSet(DTS, DT, "a")
            'BD.CargarDataSet(DTS, DTBanda2, "BB", 0, "ID_Producto", "ID_Producto")




            With Me.GRD_StockPersonal
                Me.T_ImporteStock.Value = _Stock.Sum(Function(F) F.ImportePVPStockReal)
                .M.clsUltraGrid.CargarIEnumerable(_Stock)
                '.M.clsUltraGrid.Cargar(DTS)
                .M_NoEditable()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
#End Region

#Region "Calendari"
    Private Sub Calendari_PopupMenuShowing(sender As Object, e As DevExpress.XtraScheduler.PopupMenuShowingEventArgs) Handles Calendari.PopupMenuShowing
        'Fem això pq no es vegi el menú que surt al fer botó dret
        e.Menu.HidePopup()
        e.Menu.Visible = False
        e.Menu.Items.Clear()
    End Sub

#End Region

#Region "Grid Trabajos Partes Actuales"
    Private Sub CargaGrid_Trabajo_PartesActuales()
        Try

            ' Taula.Parte_Asignacion.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).Count > 0 And Taula.Activo = True And Taula.BloquearImputacionHoras = False And (Taula.ID_Parte_Estado = EnumParteEstado.Pendiente Or Taula.ID_Parte_Estado = EnumParteEstado.EnCurso) 
            'Dim _Partes As IEnumerable(Of Parte_Horas) = From taula In oDTC.Parte_Horas Where taula.Personal.ID_Personal = Seguretat.oUser.ID_Personal And taula.Fecha >= pDataInici And taula.Fecha <= pDataFi Order By taula.Fecha Select taula
            Dim DT As DataTable = BD.RetornaDataTable("Select * From C_Parte Where  ID_Parte in (Select ID_Parte From Parte_Asignacion Where id_personal = " & Seguretat.oUser.ID_Personal & ") And ID_Parte_Estado <>" & EnumParteEstado.Finalizado & " Order By FechaInicio")

            With Me.GRD_Trabajos_PartesActuales
                '.GRID.DataSource = _Horas
                .M.clsUltraGrid.Cargar(DT)

                .M_NoEditable()

                .GRID.Selected.Rows.Clear()
                .GRID.ActiveRow = Nothing
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
    Private Sub GRD_Trabajos_PartesActuales_M_GRID_ClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Trabajos_PartesActuales.M_GRID_ClickRow2
        Call CargarGrid_Trabajos_PartesActuales(e.Cells("ID_Parte").Value)
    End Sub

#End Region

#Region "Grid TrabajosARealizar"
    Private Sub CargarGrid_Trabajos_PartesActuales(ByVal pIDParte As Integer)
        Try

            With Me.GRD_Trabajos
                Dim DTS As New DataSet
                BD.CargarDataSet(DTS, "Select * From C_Parte_TrabajosARealizar Where ID_Parte=" & pIDParte & " Order By NumDia, Orden")
                .M.clsUltraGrid.Cargar(DTS)
                .GRID.ActiveRow = Nothing

                Dim pRow As UltraGridRow

                For Each pRow In .GRID.Rows
                    Dim _TrabajoARealizar As Parte_TrabajosARealizar
                    Dim _ID As Integer = CInt(pRow.Cells("ID_Parte_TrabajosARealizar").Value)
                    _TrabajoARealizar = oDTC.Parte_TrabajosARealizar.Where(Function(F) F.ID_Parte_TrabajosARealizar = _ID).FirstOrDefault
                    Dim _Personal As Parte_TrabajosARealizar_Personal
                    _Personal = _TrabajoARealizar.Parte_TrabajosARealizar_Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
                    If _Personal Is Nothing = False Then
                        If _Personal.Finalizada = True Then
                            pRow.Appearance.BackColor = Color.LightGreen
                        End If
                    End If
                Next

            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub
    Private Sub GRD_Trabajos_M_GRID_DoubleClickRow2(ByRef sender As M_UltraGrid.m_UltraGrid, ByRef e As UltraGridRow) Handles GRD_Trabajos.M_GRID_DoubleClickRow2
        Try
            Dim _TrabajoARealizar As Parte_TrabajosARealizar
            Dim ID As Integer = CInt(e.Cells("ID_Parte_TrabajosARealizar").Value)
            _TrabajoARealizar = oDTC.Parte_TrabajosARealizar.Where(Function(F) F.ID_Parte_TrabajosARealizar = ID).FirstOrDefault
            Dim _Personal As Parte_TrabajosARealizar_Personal
            _Personal = _TrabajoARealizar.Parte_TrabajosARealizar_Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
            If _Personal Is Nothing Then
                Dim _NewPersonal As New Parte_TrabajosARealizar_Personal
                _NewPersonal.Parte_TrabajosARealizar = _TrabajoARealizar
                _NewPersonal.Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal).FirstOrDefault
                _NewPersonal.FechaFinalizacion = Now
                _NewPersonal.Finalizada = True
                oDTC.Parte_TrabajosARealizar_Personal.InsertOnSubmit(_NewPersonal)
            Else
                If _Personal.Finalizada = False Then
                    _Personal.Finalizada = True
                    _Personal.FechaFinalizacion = Now
                End If
            End If
            oDTC.SubmitChanges()
            Me.GRD_Trabajos_PartesActuales.GRID.ActiveRow = Nothing

            Call CargarGrid_Trabajos_PartesActuales(e.Cells("ID_Parte").Value)

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




    Private Sub GRD_PartesActuales_Load(sender As Object, e As EventArgs) Handles GRD_PartesActuales.Load

    End Sub
End Class