Module Mod_Util
    Public Function RetornaColorSegonsDivisio(ByVal pDivisio As EnumProductoDivision) As Color
        Try
            'Dim oDTC As New DTCDataContext(BD.Conexion)
            'Dim _Divisio As Producto_Division = oDTC.Producto_Division.Where(Function(F) F.ID_Producto_Division = pDivisio).FirstOrDefault
            'Return Color.FromArgb(_Divisio.ColorR, _Divisio.ColorG, _Divisio.ColorB)


            Dim _Divisio As Producto_Division = PublicProductoDivision.Where(Function(F) F.ID_Producto_Division = pDivisio).FirstOrDefault()

            Return Color.FromArgb(_Divisio.ColorR, _Divisio.ColorG, _Divisio.ColorB)
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Sub ExecutarReport(ByRef pReport As DevExpress.XtraReports.UI.XtraReport, ByVal pRutaReport As String, ByVal pSelect As String, ByVal pTaula As String, ByVal pFiltre As String)
        If pRutaReport <> "" Then 'Si entrem al disenyador no fa falta ruta
            pReport.LoadLayout(pRutaReport)
        End If
        Dim SelectCommand As New System.Data.SqlClient.SqlCommand(pSelect)
        Dim da As New System.Data.SqlClient.SqlDataAdapter(SelectCommand)
        Dim ds As DataSet = New DataSet

        SelectCommand.Connection = BD.Conexion
        da.Fill(ds, pTaula)
        Dim oDTC As New DTCDataContext(BD.Conexion)
        pReport.DataSource = oDTC.Instalacion
        '        pReport.DataSource = ds
        '       pReport.DataMember = pTaula
        pReport.DataAdapter = Nothing

        If pRutaReport <> "" Then 'Si entrem al disenyador no fa falta ruta
            pReport.FilterString = pFiltre
            pReport.CreateDocument()
        End If
    End Sub

    Public Function DiesDeBaixa(ByVal pDTC As DTCDataContext, ByVal pDataInici As Date, ByVal pDataFi As Date, ByVal pIDPersonal As Integer) As DataTable
        Try
            Dim _BajasTrabajador As IEnumerable(Of Personal_Baja) = pDTC.Personal_Baja.Where(Function(F) F.ID_Personal = pIDPersonal)
            Dim DT As New DataTable
            Dim DTRow As DataRow
            DT.Columns.Add("Fecha", GetType(Date))
            Dim _Data As Date

            Dim _Baja As Personal_Baja
            For Each _Baja In _BajasTrabajador

                If _Baja.FechaFin.HasValue = True Then
                    _Data = _Baja.FechaInicio
                    Do Until _Data = _Baja.FechaFin
                        If _Data >= pDataInici And _Data <= pDataFi Then
                            DTRow = DT.NewRow
                            DTRow.Item("Fecha") = _Data
                            DT.Rows.Add(DTRow)
                        End If
                        _Data = _Data.AddDays(1)

                    Loop
                End If
            Next
            Return DT
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function Calcular_HoresPendentsAssignar(ByVal pDTC As DTCDataContext, ByVal pDataInici As Date, ByVal pDataFi As Date, ByVal pIDPersonal As Integer) As Integer
        Dim LlistatFestiusEmpresa As IEnumerable(Of Empresa_FechasNoLaborables) = pDTC.Empresa_FechasNoLaborables.Where(Function(F) F.ID_Empresa = 1 And F.Fecha >= pDataInici And F.Fecha <= pDataFi).OrderBy(Function(F) F.Fecha)
        Dim LlistatAusenciesTreballador As IEnumerable(Of Personal_Ausencia) = pDTC.Personal_Ausencia.Where(Function(F) F.ID_Personal = Seguretat.oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi).OrderBy(Function(F) F.Fecha)
        '  Dim _LlistatHoresTreballades As IEnumerable(Of Parte_Horas) = pDTC.Parte_Horas.Where(Function(F) F.Parte.Activo = True And F.ID_Personal = oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi)

        'Dim _TotalHoresTreballades As Decimal
        'If pDTC.Parte_Horas.Where(Function(F) F.ID_Personal = oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi).Count > 0 Then
        '    _TotalHoresTreballades = pDTC.Parte_Horas.Where(Function(F) F.ID_Personal = oUser.ID_Personal And F.Fecha >= pDataInici And F.Fecha <= pDataFi).Sum(Function(F) F.Horas)
        'End If


        Dim _Calcul As Integer
        'Primer multipliquem per 8 els dies festius d'empresa que hi ha en aquesta setmana
        Dim _horesARestarPerDiesQueNoTreballava As Integer
        _horesARestarPerDiesQueNoTreballava = 8 * LlistatFestiusEmpresa.Count

        'Després mirarem si aquella setmana ha estat algun dia ausent. 
        If LlistatAusenciesTreballador.Count > 0 Then
            'Si ho ha estat i encanvi no hi havia cap dia festiu d'empresa multiplicarem * 8 el número de dies que ha estat ausent
            If LlistatFestiusEmpresa.Count = 0 Then
                _horesARestarPerDiesQueNoTreballava = 8 * LlistatAusenciesTreballador.Count
            Else
                'Si encanvi, Coincideix que hi havia algun dia festiu d'empresa en aquesta setmana, haurem de vigilar que no coincideixi amb un dels ausents
                Dim _Ausencia As Personal_Ausencia
                Dim DiesQueHaFaltatIQueNoCoincideixAmbFestiuEmpresa As Integer = 0
                For Each _Ausencia In LlistatAusenciesTreballador
                    If LlistatFestiusEmpresa.Where(Function(F) F.Fecha = _Ausencia.Fecha).Count = 0 Then
                        DiesQueHaFaltatIQueNoCoincideixAmbFestiuEmpresa = DiesQueHaFaltatIQueNoCoincideixAmbFestiuEmpresa + 1
                    End If
                Next
                _horesARestarPerDiesQueNoTreballava = _horesARestarPerDiesQueNoTreballava + DiesQueHaFaltatIQueNoCoincideixAmbFestiuEmpresa * 8
            End If
        End If

        'Després mirarem si aquella setmana ha estat algun dia de baixa. 
        Dim DTBaixes As DataTable = Mod_Util.DiesDeBaixa(pDTC, pDataInici, pDataFi, pIDPersonal)
        Dim DiesDeBaixa As Integer

        If DTBaixes Is Nothing = False AndAlso DTBaixes.Rows.Count > 0 Then
            'Si ho ha estat i encanvi no hi havia cap dia festiu d'empresa multiplicarem * 8 el número de dies que ha estat ausent
            Dim i As Integer
            DiesDeBaixa = 0
            For i = 0 To DTBaixes.Rows.Count - 1
                If LlistatFestiusEmpresa.Where(Function(F) F.Fecha = DTBaixes(i).Item("Fecha")).Count = 0 Then
                    DiesDeBaixa = DiesDeBaixa + 1
                End If
            Next
        End If

        _Calcul = _horesARestarPerDiesQueNoTreballava + DiesDeBaixa * 8 '_TotalHoresTreballades
        Return IIf(_Calcul < 0, 0, _Calcul)
    End Function
End Module
