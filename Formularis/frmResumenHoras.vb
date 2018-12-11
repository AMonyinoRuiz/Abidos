Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid

Public Class frmResumenHoras
    Dim oDTC As DTCDataContext


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

        '   Dim _LlistatPersonal As IEnumerable(Of Personal) = oDTC.Personal.Where(Function(F) F.Activo = True And F.Personal_PersonalACargo.Where(Function(F2) F2.ID_Personal = F.ID_Personal).Count = 1)
        Util.Cargar_Combo(C_Personal, "Select ID_Personal, Nombre From Personal Where ID_Personal in (Select ID_PersonalACargo From Personal_PersonalACargo as A Where A.ID_Personal=" & Seguretat.oUser.ID_Personal & ") and Activo=1 and FechaBajaEmpresa is null Order By Nombre", True, False)

    End Sub

    Private Sub M_ToolForm1_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub

    Private Sub frmManteniment_Familias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Fem això per a que no surti la divisió carregada
        Me.GRD_Resumen.GRID.Selected.Rows.Clear()
        Me.GRD_Resumen.GRID.ActiveRow = Nothing
    End Sub

#Region "Resumen Horas realizadas"

    Private Sub CargaGrid_ResumenHorasRealizadas(ByVal pIdPersonal As Integer, ByVal pAny As Integer)
        Try
            Dim oDTC As New DTCDataContext(BD.Conexion)

            Dim DT As New DataTable
            DT.Columns.Add("Mes", GetType(String))
            DT.Columns.Add("NumMes", GetType(Integer))
            Dim _Dia As Integer
            Dim _Mes As Integer
            Dim _TotalHores As Decimal
            Dim _TotalHoresMes As Decimal
            'Dim _HorasLaborables As Decimal

            Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = pIdPersonal).FirstOrDefault



            For _Dia = 1 To 31
                DT.Columns.Add(_Dia, GetType(Decimal))
            Next
            DT.Columns.Add("HorasTrabajadas", GetType(Decimal))
            DT.Columns.Add("HorasLaborables", GetType(Decimal))
            DT.Columns.Add("Diferencia", GetType(Decimal))


            Dim _DiesAusenciesTreballador As IEnumerable(Of Personal_Ausencia) = oDTC.Personal_Ausencia.Where(Function(F) F.ID_Personal = _Personal.ID_Personal And F.Fecha.Year = pAny)
            Dim _DiesFestiusEmpresa As IEnumerable(Of Empresa_FechasNoLaborables) = oDTC.Empresa_FechasNoLaborables.Where(Function(F) F.ID_Empresa = 1 And F.Fecha.Year = pAny)
            Dim _HoresTreballadorAny As IEnumerable(Of Parte_Horas) = oDTC.Parte_Horas.Where(Function(F) F.Parte.Activo = True And F.ID_Personal = pIdPersonal And F.Fecha.Year = pAny)
            Dim DTRowMes As DataRow
            Dim _DiesNoCapDeSetmana As Integer

            'Omplir 

            For _Mes = 1 To 12
                DTRowMes = DT.NewRow
                DTRowMes.Item("NumMes") = _Mes
                DTRowMes("Mes") = Util.Retorna_NombreMes(_Mes)
                _DiesNoCapDeSetmana = 0
                _TotalHoresMes = 0
                For _Dia = 1 To 31
                    If _Dia > Util.RetornaUltimDiaDelMes(pAny, _Mes).Day Then 'Aquest IF es per vigilar que la data existeixi, per exemple no existeix el 31/2/...
                        _TotalHores = 0
                    Else
                        Dim _Data As Date
                        _Data = _Dia & "/" & _Mes & "/" & pAny

                        _TotalHores = _HoresTreballadorAny.Where(Function(F) F.Fecha = _Data).Sum(Function(F) F.Horas)
                        If Util.EsCapDeSetmana(_Data) = False Then
                            _DiesNoCapDeSetmana = _DiesNoCapDeSetmana + 1
                        End If
                    End If
                    DTRowMes(_Dia.ToString) = _TotalHores
                    _TotalHoresMes = _TotalHoresMes + _TotalHores
                Next
                DTRowMes("HorasTrabajadas") = _TotalHoresMes
                DTRowMes("HorasLaborables") = _DiesNoCapDeSetmana * 8 - Mod_Util.Calcular_HoresPendentsAssignar(oDTC, "1/" & _Mes & "/" & pAny, Util.RetornaUltimDiaDelMes(pAny, _Mes), Me.C_Personal.Value)
                DTRowMes("Diferencia") = DTRowMes("HorasLaborables") - DTRowMes("HorasTrabajadas")
                'DTRowMes("
                DT.Rows.Add(DTRowMes)
            Next

            'Dim IDPersonal As Integer = Me.TL_Personal.Tag
            'Dim DTResultat As New DataTable
            'DTResultat.Columns.Add("Dia", "System.int32".GetType)
            'DTResultat.Columns.Add("Data", "System.date".GetType)
            'DTResultat.Columns.Add("Obra1", "System.String".GetType)




            With Me.GRD_Resumen

                '.GRID.DataSource = DT
                .M.clsUltraGrid.Cargar(DT)

                'Formatejar les columnes
                Dim pCol As UltraGridColumn
                For Each pCol In .GRID.DisplayLayout.Bands(0).Columns
                    If IsNumeric(pCol.Key) = True Then
                        pCol.Width = 35
                        pCol.CellAppearance.TextHAlign = HAlign.Right
                        pCol.Format = "###,###,##0.0"
                    End If
                Next

                Dim pRow As UltraGridRow
                Dim pCell As UltraGridCell
                Dim _DTBaixes As DataTable


                For Each pRow In .GRID.Rows
                    _Mes = pRow.Cells("NumMes").Value
                    _DTBaixes = Mod_Util.DiesDeBaixa(oDTC, "1/" & _Mes & "/" & pAny, Util.RetornaUltimDiaDelMes(pAny, _Mes), Me.C_Personal.Value)
                    For Each pCell In pRow.Cells
                        If IsNumeric(pCell.Column.Key) Then
                            Dim _Data As Date


                            If pCell.Column.Key <= Util.RetornaUltimDiaDelMes(pAny, _Mes).Day Then 'Aquest IF es per vigilar que la data existeixi, per exemple no existeix el 31/2/...
                                _Data = pCell.Column.Key & "/" & _Mes & "/" & pAny
                                If pCell.Value = 0 Then
                                    pCell.Appearance.ForeColor = Color.Transparent
                                End If

                                'Pintem els dies "vacances" treballador
                                If _DiesAusenciesTreballador.Count > 0 Then
                                    If _DiesAusenciesTreballador.Where(Function(F) F.Fecha = _Data).Count = 1 Then
                                        pCell.Appearance.BackColor = Color.FromArgb(233, 255, 192)
                                    End If
                                End If

                                'Baixes treballador
                                If IsNothing(_DTBaixes) = False AndAlso _DTBaixes.Select("Fecha='" & _Data & "'").Count = 1 Then
                                    pCell.Appearance.BackColor = Color.FromArgb(255, 192, 255)
                                End If

                                'Pintem els caps de setmana
                                If Util.EsCapDeSetmana(_Data) = True Then
                                    pCell.Appearance.BackColor = Color.FromArgb(192, 255, 255)
                                End If

                                'Pintem els dies festius de l'empresa
                                If _DiesFestiusEmpresa.Count > 0 Then
                                    If _DiesFestiusEmpresa.Where(Function(F) F.Fecha = _Data).Count = 1 Then
                                        pCell.Appearance.BackColor = Color.FromArgb(255, 224, 192)
                                    End If
                                End If

                            Else
                                'Pitem els dies del mes que no existeixin 31/2/...
                                pCell.Appearance.BackColor = Color.White
                                pCell.Appearance.ForeColor = Color.White
                            End If
                        End If
                    Next

                Next
                .GRID.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.Red
                .GRID.ActiveRow = Nothing
                .GRID.Selected.Rows.Clear()
            End With

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

#End Region


    Private Sub B_Buscar_Click(sender As Object, e As EventArgs) Handles B_Buscar.Click
        If C_Personal.Items.Count = 0 Then
            Mensaje.Mostrar_Mensaje("Imposible cargar los datos, seleccione primero a un trabajador", M_Mensaje.Missatge_Modo.INFORMACIO)
            Exit Sub
        End If
        Util.WaitFormObrir()
        Call CargaGrid_ResumenHorasRealizadas(Me.C_Personal.Value, Me.C_Año.Value)
        Util.WaitFormTancar()
    End Sub
End Class