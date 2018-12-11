Public Class frmListado_ParteRevisiones
    Dim oDTC As DTCDataContext
    Public Sub Entrada()
        Me.AplicarDisseny()

        Me.ToolForm.M.Botons.tSeleccionar.SharedProps.Visible = False
        Util.Cargar_Combo(Me.C_Division, "Select ID_Producto_Division, Descripcion From Producto_Division Where Activo=1 Order by ID_Producto_Division")

        Dim _Anys As Integer
        For _Anys = 2010 To 2030
            Me.C_Años.Items.Add(_Anys)
        Next

        Me.C_Años.Value = Now.Year
        Me.C_Division.SelectedIndex = 0

        oDTC = New DTCDataContext(BD.Conexion)




        'Me.GRD.M.clsUltraGrid.Cargar("Select * From C_Listado_Parte Where Activo=1 Order by FechaAlta Desc", BD)



    End Sub

    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub GRD_M_GRID_DoubleClickRow(ByRef sender As Infragistics.Win.UltraWinGrid.UltraGrid, ByRef e As System.EventArgs) Handles GRD.M_GRID_DoubleClickRow
        If Me.GRD.GRID.Selected.Rows.Count <> 1 Then
            Exit Sub
        End If

        Dim ID As Integer
        ID = Me.GRD.GRID.Selected.Rows(0).Cells("ID_Instalacion").Value

        Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
        frm.Entrada(ID)
        frm.FormObrir(Me, True)
    End Sub

    Private Sub CarregarDades(ByVal pIDDivision As Integer)
        Try
            oDTC = New DTCDataContext(BD.Conexion)
            Dim _Instalaciones = oDTC.Instalacion.Where(Function(F) F.ID_Instalacion_Estado = EnumInstalacionEstado.Instalado And F.Activo = True And F.Instalacion_Producto_Division.Where(Function(F2) F2.ID_Producto_Division = pIDDivision And F2.FechaInicio.HasValue = True And F2.MesesRevision.HasValue = True AndAlso F2.MesesRevision.Value > 0).Count > 0)

            Dim _Ins As Instalacion
            Dim _Array(0 To 1) As Integer
            Dim _DT As New DataTable
            _DT.Columns.Add("ID_Instalacion", "System.Integer".GetType)
            _DT.Columns.Add("Cliente", "System.String".GetType)
            _DT.Columns.Add("Instalacion_Poblacion", "System.String".GetType)
            _DT.Columns.Add("Instalacion_Direccion", "System.String".GetType)
            _DT.Columns.Add("Instalacion_Telefono", "System.String".GetType)
            _DT.Columns.Add("Q1", "System.String".GetType)
            _DT.Columns.Add("Q2", "System.String".GetType)
            _DT.Columns.Add("Q3", "System.String".GetType)
            _DT.Columns.Add("Q4", "System.String".GetType)
            _DT.Columns.Add("DiasDesDeLaUltimaRevision", "System.Integer".GetType)
            _DT.Columns.Add("Importe", "System.Decimal".GetType)



            For Each _Ins In _Instalaciones
                Dim _InsDiv As Instalacion_Producto_Division
                Dim _DTRow As DataRow = _DT.NewRow
                _InsDiv = _Ins.Instalacion_Producto_Division.Where(Function(F) F.ID_Producto_Division = pIDDivision).LastOrDefault

                Dim _FechaInicio As Date
                Dim _Periodicidad As Integer
                _FechaInicio = _InsDiv.FechaInicio
                _Periodicidad = _InsDiv.MesesRevision

                _DTRow("ID_Instalacion") = _Ins.ID_Instalacion
                _DTRow("Cliente") = _Ins.Cliente.Nombre
                _DTRow("Instalacion_Poblacion") = _Ins.Poblacion
                _DTRow("Instalacion_Direccion") = _Ins.Direccion
                _DTRow("Instalacion_Telefono") = _Ins.Telefono
                _DTRow("Importe") = _InsDiv.Importe

                Dim _Any As Integer = Me.C_Años.Value

                _DTRow("Q1") = RevisioTrimestre(_Ins, _InsDiv, Me.C_Division.Value, "1/1/" & _Any, "31/3/" & _Any)
                _DTRow("Q2") = RevisioTrimestre(_Ins, _InsDiv, Me.C_Division.Value, "1/4/" & _Any, "30/6/" & _Any)
                _DTRow("Q3") = RevisioTrimestre(_Ins, _InsDiv, Me.C_Division.Value, "1/7/" & _Any, "30/9/" & _Any)
                _DTRow("Q4") = RevisioTrimestre(_Ins, _InsDiv, Me.C_Division.Value, "1/10/" & _Any, "31/12/" & _Any)



                Dim Valor As Parte = _Ins.Parte.Where(Function(F) F.Activo = True And F.ID_Parte_Tipo = EnumParteTipo.Revision And F.ID_Producto_Division = pIDDivision And F.FechaInicio.HasValue).OrderByDescending(Function(F) F.FechaInicio).FirstOrDefault
                If Valor Is Nothing = False Then
                    Dim _FechaUltimaRevisio As Date
                    _FechaUltimaRevisio = Valor.FechaInicio
                    If _InsDiv.FechaFin Is Nothing Then
                        _DTRow("DiasDesDeLaUltimaRevision") = DateDiff(DateInterval.Day, _FechaUltimaRevisio, Date.Now)
                    Else
                        If _InsDiv.FechaFin > Date.Now Then
                            _DTRow("DiasDesDeLaUltimaRevision") = DateDiff(DateInterval.Day, _FechaUltimaRevisio, Date.Now)
                        Else
                            _DTRow("DiasDesDeLaUltimaRevision") = Nothing
                        End If
                    End If
                Else
                    _DTRow("DiasDesDeLaUltimaRevision") = Nothing
                End If



                '                Dim i As Integer
                '                Dim _FechaAux As Date = _FechaInicio
                '                Dim _DiesAvis As Integer
                '                For i = 1 To 100000
                ' _FechaAux = _FechaAux.AddMonths(_Periodicidad)
                ' Dim _Partes = _Ins.Parte.Where(Function(F) F.Activo = True And F.ID_Parte_Tipo = EnumParteTipo.Revision AndAlso F.ID_Producto_Division = pIDDivision And F.FechaInicio.HasValue = True AndAlso F.FechaInicio > _FechaAux)
                ' If IsNothing(_Partes) OrElse _Partes.Count = 0 Then
                ' _DiesAvis = DateDiff(DateInterval.Day, Now.Date, _FechaAux)
                ' Exit For
                ' End If
                'Next

                '_Array.Add(_Ins)

                _DT.Rows.Add(_DTRow)
            Next

            'If _DT Is Nothing Then
            ' _DT = BD.RetornaDataTable("Select * From C_Instalacion")
            ' _DT.Columns.Add("DiferenciaDias", "System.Integer".GetType)
            ' End If
            'Me.GRD.GRID.DataSource = _DT
            Me.GRD.M.clsUltraGrid.Cargar(_DT)


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Function RevisioTrimestre(ByRef pIns As Instalacion, ByRef pInsDiv As Instalacion_Producto_Division, ByVal pIDDivision As Integer, ByVal pFechaInici As Date, ByVal pFechaFin As Date) As String
        Dim _Parte
        _Parte = pIns.Parte.Where(Function(F) F.Activo = True And F.ID_Parte_Tipo = EnumParteTipo.Revision And F.ID_Producto_Division = pIDDivision And F.FechaInicio.HasValue AndAlso F.FechaInicio.Value >= pFechaInici AndAlso F.FechaInicio <= pFechaFin).OrderByDescending(Function(F) F.FechaInicio).FirstOrDefault

        If pInsDiv.FechaFin Is Nothing = False AndAlso pInsDiv.FechaFin < "1/4/" & Me.C_Años.Value Then
            Return "Fin contrato"
        Else
            If _Parte Is Nothing = False Then
                Return _Parte.Parte_Estado.Descripcion
            Else
                Return ""
            End If
        End If
    End Function

    Private Sub B_Buscar_Click(sender As Object, e As EventArgs) Handles B_Buscar.Click
        If C_Division.SelectedIndex >= 0 Then
            Call CarregarDades(Me.C_Division.Value)
        End If
    End Sub

    Private Sub frmListado_ParteRevisiones_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class