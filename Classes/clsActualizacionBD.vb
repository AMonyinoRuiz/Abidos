Public Class clsActualizacionBD
    Dim oBDPrincipal As M_Conexion.clsConexion
    Dim oConnectat As Boolean = False
    Dim _ArrayVersions As IEnumerable(Of String)
    Shared oVersioActualBD As Integer
    Shared oVersioActualPrograma As Integer
    Shared oInformatAlUsuariVersioDiferent As Boolean = False

    Public Shared ReadOnly Property pVersioActualBD() As Integer
        Get
            Return oVersioActualBD
        End Get
    End Property

    Public Shared ReadOnly Property pVersioActualPrograma() As Integer
        Get
            Return oVersioActualPrograma
        End Get
    End Property

    'Quan detectem i avisem al usuari que la versió de la BD es diferent que la versió del programa posarem a true aquesta propietat
    Public Shared Property pInformatAlUsuariVersioDiferent() As Boolean
        Get
            Return oInformatAlUsuariVersioDiferent
        End Get
        Set(ByVal Value As Boolean)
            oInformatAlUsuariVersioDiferent = Value
        End Set
    End Property

    Public Sub New()
        'Fem això pq automàticament la propietat pVersioActualBD tingui la versió actual de la base de dades
        oVersioActualBD = RetornaUltimaVersioBD()
    End Sub

    Public Function ComprovarSiVersioBDIgualAVersioPrograma() As Boolean
        Try

            Dim thisExe As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            _ArrayVersions = thisExe.GetManifestResourceNames.Where(Function(F) F.StartsWith("Abidos.Version")).OrderBy(Function(F) F)

            If _ArrayVersions.LastOrDefault.Contains(RetornaUltimaVersioBD) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
            Throw ex 'Fem això pq volem que envii un error, ja que retorna un true o false no ens aporta re
        End Try
    End Function

    Public Function EstablirConnexioPrincipal() As Boolean
        Try
            oConnectat = False
            EstablirConnexioPrincipal = False

            M_Seguretat.clsKey.CargarKey(GetSetting("Abidos", "Datos Key", "Ruta"))
            oBDPrincipal = New M_Conexion.clsConexion
            'M_Seguretat.clsKey.pServidor

            oBDPrincipal.Conectar(BD.pBDNomInstancia, BD.pBDNomBaseDades, M_Seguretat.clsKey.pUsuario, M_Seguretat.clsKey.pContraseña)

            EstablirConnexioPrincipal = True
            oConnectat = True
        Catch ex As Exception
            Mensaje.Mostrar_Mensaje("Imposible establecer conexion con el servidor principal: " & ex.Message, M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
        End Try
    End Function

    Public Function ActualitzacioBD() As Boolean
        Try
            ActualitzacioBD = False

            Dim Inici As Integer = RetornaUltimaVersioBD() + 1
            For i = Inici To RetornaElNumeroDeVersioDelFitxer(_ArrayVersions.LastOrDefault)
                Dim _ActualitzacioBD As String
                _ActualitzacioBD = Leer_FicherSQL(i)
                If _ActualitzacioBD Is Nothing OrElse _ActualitzacioBD.Length = 0 Then
                    Exit For
                End If

                If oBDPrincipal.EjecutarNonQuery(_ActualitzacioBD) = False Then
                    Return False
                End If
                oBDPrincipal.EjecutarConsulta("Update VersionBD set Version=" & i)
                oBDPrincipal.EjecutarConsulta("Insert Into VersionBD_Historial Values(" & i & ", Getdate())")
                'Fem això pq automàticament la propietat pVersioActualBD tingui la versió actual de la base de dades
                oVersioActualBD = RetornaUltimaVersioBD()

            Next

            ActualitzacioBD = True
        Catch ex As Exception
            MsgBox(ex.Message)
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

    Public Function Leer_FicherSQL(ByRef pNomFitxer As String) As String
        Try
            Dim thisExe As System.Reflection.Assembly
            thisExe = System.Reflection.Assembly.GetExecutingAssembly()
            Dim file As System.IO.Stream = thisExe.GetManifestResourceStream("Abidos.Version" & pNomFitxer.ToLower & ".sql")

            If file Is Nothing Then 'si no ha trobat el fitxer sortirem
                Return Nothing
            End If

            Dim _FitxerTexte = New System.IO.StreamReader(file, System.Text.Encoding.Default)

            Leer_FicherSQL = _FitxerTexte.ReadToEnd


            file.Close()
            file.Dispose()
            file = Nothing

            _FitxerTexte.Close()
            _FitxerTexte.Dispose()
            _FitxerTexte = Nothing

        Catch oerr As Exception
            Throw New Exception(oerr.Message & " " & Reflection.MethodBase.GetCurrentMethod.Name.ToString)
        End Try
    End Function

    Private Shared Function RetornaElNumeroDeVersioDelFitxer(ByVal pText As String) As Integer
        Return CInt(pText.Replace("Abidos.Version", "").Replace(".sql", ""))
    End Function

    Public Shared Function RetornaUltimaVersioBD() As Integer
        Return BD.RetornaValorSQL("Select Version From VersionBD")
    End Function

    Public Shared Function EstableixVersioPrograma()
        Try

            Dim thisExe As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()

            Dim _ArrayVersions2 As IEnumerable(Of String) = thisExe.GetManifestResourceNames.Where(Function(F) F.StartsWith("Abidos.Version")).OrderBy(Function(F) F)

            oVersioActualPrograma = RetornaElNumeroDeVersioDelFitxer(_ArrayVersions2.LastOrDefault)

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Function

End Class
