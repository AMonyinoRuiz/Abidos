Public Class clsAnotacioCalendariOperaris
    Dim oDTC As New DTCDataContext(BD.Conexion)

    Public Sub New(ByRef pDTC As DTCDataContext)
        oDTC = pDTC
    End Sub


    Public Sub CrearAnotacioBaixa(ByVal pIDPersonal As Integer, ByVal pFechaInici As Date, ByVal pFechaFi As Date, ByRef pPersonalBaja As Personal_Baja)
        Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = pIDPersonal).FirstOrDefault
        Dim _Anotacio As New Calendario_Operarios
        _Anotacio.Asunto = "Baja del trabajador: " & _Personal.Nombre
        _Anotacio.FechaInicio = New Date(pFechaInici.Year, pFechaInici.Month, pFechaInici.Day, 8, 0, 0)
        If pFechaFi.Year = 1 Then 'si no s'ha introduit fecha fi llavors l'anotació durarà un any a partir de la fecha d'inici
            _Anotacio.FechaFin = New Date(pFechaInici.Year + 1, pFechaInici.Month, pFechaInici.Day, 20, 0, 0)
        Else
            _Anotacio.FechaFin = New Date(pFechaFi.Year, pFechaFi.Month, pFechaFi.Day, 20, 0, 0)
        End If

        _Anotacio.Personal = _Personal
        _Anotacio.Personal_Baja = pPersonalBaja

        oDTC.Calendario_Operarios.InsertOnSubmit(_Anotacio)
        oDTC.SubmitChanges()

    End Sub

    Public Sub ModificarAnotacioBaixa(ByRef pPersonalBaja As Personal_Baja)
        Dim _IDPersonalBaja As Integer = pPersonalBaja.ID_Personal_Baja
        Dim _NewAnotacio As Calendario_Operarios = oDTC.Calendario_Operarios.Where(Function(F) F.ID_Personal_Baja = _IDPersonalBaja).FirstOrDefault
        _NewAnotacio.FechaInicio = New Date(pPersonalBaja.FechaInicio.Year, pPersonalBaja.FechaInicio.Month, pPersonalBaja.FechaInicio.Day, 8, 0, 0)
        If pPersonalBaja.FechaFin Is Nothing OrElse CDate(pPersonalBaja.FechaFin).Year = 1 Then 'si no s'ha introduit fecha fi llavors l'anotació durarà un any a partir de la fecha d'inici
            _NewAnotacio.FechaFin = New Date(pPersonalBaja.FechaInicio.Year + 1, pPersonalBaja.FechaInicio.Month, pPersonalBaja.FechaInicio.Day, 20, 0, 0)
        Else
            _NewAnotacio.FechaFin = New Date(CDate(pPersonalBaja.FechaFin).Year, CDate(pPersonalBaja.FechaFin).Month, CDate(pPersonalBaja.FechaFin).Day, 20, 0, 0)
        End If

        oDTC.SubmitChanges()
    End Sub

    Public Sub EliminarAnotacioBaixa(ByRef pPersonalBaja As Personal_Baja)
        Dim _IDPersonalBaja As Integer = pPersonalBaja.ID_Personal_Baja
        Dim _Anotacio As Calendario_Operarios = oDTC.Calendario_Operarios.Where(Function(F) F.ID_Personal_Baja = _IDPersonalBaja).FirstOrDefault
        oDTC.Calendario_Operarios.DeleteOnSubmit(_Anotacio)
        oDTC.SubmitChanges()
    End Sub


    Public Sub CrearAnotacioAusencia(ByVal pIDPersonal As Integer, ByVal pFechaInici As Date, ByRef pPersonalAusencia As Personal_Ausencia)
        Dim _Personal As Personal = oDTC.Personal.Where(Function(F) F.ID_Personal = pIDPersonal).FirstOrDefault
        Dim _Anotacio As New Calendario_Operarios
        _Anotacio.Asunto = "Ausencia del trabajador: " & _Personal.Nombre
        _Anotacio.FechaInicio = New Date(pFechaInici.Year, pFechaInici.Month, pFechaInici.Day, 8, 0, 0)
        _Anotacio.FechaFin = New Date(pFechaInici.Year, pFechaInici.Month, pFechaInici.Day, 20, 0, 0)
        _Anotacio.Personal = _Personal
        _Anotacio.Personal_Ausencia = pPersonalAusencia

        oDTC.Calendario_Operarios.InsertOnSubmit(_Anotacio)
        oDTC.SubmitChanges()

    End Sub

    Public Sub ModificarAnotacioAusencia(ByRef pPersonalAusencia As Personal_Ausencia)
        Dim _IDPersonalAusencia As Integer = pPersonalAusencia.ID_Personal_Ausencia
        Dim _NewAnotacio As Calendario_Operarios = oDTC.Calendario_Operarios.Where(Function(F) F.ID_Personal_Ausencia = _IDPersonalAusencia).FirstOrDefault
        _NewAnotacio.FechaInicio = New Date(pPersonalAusencia.Fecha.Year, pPersonalAusencia.Fecha.Month, pPersonalAusencia.Fecha.Day, 8, 0, 0)
        _NewAnotacio.FechaFin = New Date(CDate(pPersonalAusencia.Fecha).Year, CDate(pPersonalAusencia.Fecha).Month, CDate(pPersonalAusencia.Fecha).Day, 20, 0, 0)

        oDTC.SubmitChanges()
    End Sub

    Public Sub EliminarAnotacioAusencia(ByRef pPersonalAusencia As Personal_Ausencia)
        Dim _IDPersonalAusencia As Integer = pPersonalAusencia.ID_Personal_Ausencia
        Dim _Anotacio As Calendario_Operarios = oDTC.Calendario_Operarios.Where(Function(F) F.ID_Personal_Ausencia = _IDPersonalAusencia).FirstOrDefault
        If _Anotacio Is Nothing = False Then
            oDTC.Calendario_Operarios.DeleteOnSubmit(_Anotacio)
        End If
        oDTC.SubmitChanges()
    End Sub


    Public Sub CrearAnotacioDiaNoLaboral(ByVal pFecha As Date, ByRef pFechasNoLaborables As Empresa_FechasNoLaborables)
        Dim _LlistatPersones As IEnumerable(Of Personal) = oDTC.Personal
        Dim _Personal As Personal

        For Each _Personal In _LlistatPersones
            Dim _Anotacio As New Calendario_Operarios
            _Anotacio.Asunto = "Festivo"
            _Anotacio.FechaInicio = New Date(pFecha.Year, pFecha.Month, pFecha.Day, 8, 0, 0)
            _Anotacio.FechaFin = New Date(pFecha.Year, pFecha.Month, pFecha.Day, 20, 0, 0)
            _Anotacio.Personal = _Personal
            _Anotacio.Empresa_FechasNoLaborables = pFechasNoLaborables
            oDTC.Calendario_Operarios.InsertOnSubmit(_Anotacio)
        Next

        oDTC.SubmitChanges()
    End Sub


    Public Sub ModificarAnotacioDiaNoLaboral(ByRef pFechasNoLaborables As Empresa_FechasNoLaborables)
        Dim _IDEmpresa_FechasNoLaborables As Integer = pFechasNoLaborables.ID_Empresa_FechasNoLaborables
        Dim _LlistatAnotacions As IEnumerable(Of Calendario_Operarios) = oDTC.Calendario_Operarios.Where(Function(F) F.ID_Empresa_FechasNoLaborables = _IDEmpresa_FechasNoLaborables)
        Dim _Anotacio As Calendario_Operarios

        For Each _Anotacio In _LlistatAnotacions
            _Anotacio.FechaInicio = New Date(pFechasNoLaborables.Fecha.Year, pFechasNoLaborables.Fecha.Month, pFechasNoLaborables.Fecha.Day, 8, 0, 0)
            _Anotacio.FechaFin = New Date(CDate(pFechasNoLaborables.Fecha).Year, CDate(pFechasNoLaborables.Fecha).Month, CDate(pFechasNoLaborables.Fecha).Day, 20, 0, 0)
        Next

        oDTC.SubmitChanges()
    End Sub


    Public Sub EliminarAnotacioDiaNoLaboral(ByRef pFechasNoLaborables As Empresa_FechasNoLaborables)
        Dim _IDEmpresa_FechasNoLaborables As Integer = pFechasNoLaborables.ID_Empresa_FechasNoLaborables
        Dim _LlistatAnotacions As IEnumerable(Of Calendario_Operarios) = oDTC.Calendario_Operarios.Where(Function(F) F.ID_Empresa_FechasNoLaborables = _IDEmpresa_FechasNoLaborables)

        For Each _Anotacio In _LlistatAnotacions
            oDTC.Calendario_Operarios.DeleteOnSubmit(_Anotacio)
        Next
        oDTC.SubmitChanges()
    End Sub





End Class
