'Imports Microsoft.Office.Interop
Imports System.Deployment


'Public Class clsInici

'    Public Shared Function Main(ByVal pUsuariNom As String, ByVal pUsuariContrasenya As String, ByRef pfrmPrincipal As Principal) As Boolean
'        Try
'            Main = True

'            Dim oDTC As New DTCDataContext(BD.Conexion)
'            Dim _Usuario As Usuario = oDTC.Usuario.Where(Function(F) F.Nombre = pUsuariNom And F.Contraseña = pUsuariContrasenya).FirstOrDefault

'            If _Usuario Is Nothing Then
'                Return False
'                Exit Function
'            End If

'            oUser = _Usuario

'            Dim Sesio As New Log_Sesiones
'            Sesio.FechaEntrada = Date.Now
'            Sesio.FechaSalida = Nothing
'            Sesio.Usuario = _Usuario
'            Sesio.Ordenador = SystemInformation.ComputerName
'            Sesio.NombreUsuario = SystemInformation.UserName
'            Sesio.ConexionPorTerminal = SystemInformation.TerminalServerSession

'            oDTC.Log_Sesiones.InsertOnSubmit(Sesio)
'            oDTC.SubmitChanges()

'            PublicProductoDivision = oDTC.Producto_Division.ToArray


'            m_ToolForm.clsToolForm.pIdioma = m_ToolForm.clsToolForm.Enum_Idioma.Castella

'            M_Archivos_Binarios.clsArchivosBinarios2.oNivelSeguridadMaximo = ConstNivellSeguretatMaxim
'            M_Archivos_Binarios.clsArchivosBinarios2.oNivelSeguridadUsuarioActual = oUser.NivelSeguridad

'            M_Global.clsVariables.pBD = BD
'            M_Global.clsVariables.pModoProgramacio = True
'            M_Global.clsVariables.pUsuari_ID = 10

'            M_GenericForm.clsParametres.Cargar_Dades_Formulari(_Usuario.ID_Usuario)
'            M_GenericForm.clsParametres.pNivellSeguretat = _Usuario.NivelSeguridad
'            M_GenericForm.frmBase.pModeDisseny = False

'            Util = New M_Util.clsFunciones(BD)
'            M_Util.clsFunciones.FrameWork40 = False


'            Mensaje = New M_Mensaje.clsMensaje
'            M_Mensaje.clsLog.pBDConexio = BD
'            M_Mensaje.clsLog.pModoProgramacio = True
'            M_Mensaje.clsLog.pMostrarPorPantalla = False
'            M_Mensaje.clsLog.pUsuari = 10



'            Dim Log As M_Mensaje.clsLog
'            Log = New M_Mensaje.clsLog
'            M_Mensaje.clsLog.pIdioma = M_Mensaje.clsLog.EnumIdioma.Español

'            M_UltraGrid.clsParametres.pBD = BD
'            M_UltraGrid.clsParametres.pModoProgramacio = False
'            M_UltraGrid.clsParametres.pUsuari_ID = 10
'            M_UltraGrid.clsParametres.pMostrar_Errors_Grid_Sense_Capçaleres = True
'            M_UltraGrid.clsParametres.Cargar_Taula_Grid()
'            M_UltraGrid.clsParametres.Cargar_Taula_GridColumnas()
'            M_UltraGrid.clsParametres.Cargar_Taula_GridToolGrid()
'            M_UltraGrid.clsParametres.pNivellSeguretat = 1
'            M_UltraGrid.clsParametres.pNivellSeguretat = _Usuario.NivelSeguridad
'            M_UltraGrid.clsParametres.pIdioma = M_UltraGrid.clsParametres.Enum_Idioma.Castella

'            M_LlistatGeneric.clsLlistatGeneric.pBD = BD
'            M_LlistatGeneric.clsLlistatGeneric.oUtilitzarGridDevExpress = True

'            M_Disseny.ClsDisseny.pStyle = M_Disseny.ClsDisseny.Enum_Style.Office2007Black


'            Dim myVersion As Version
'            If Application.ApplicationDeployment.IsNetworkDeployed = True Then
'                myVersion = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion
'                pfrmPrincipal.Text = "Abidos Security (" & String.Format("V{0}.{1}.{2}.{3}", myVersion.Major, myVersion.Minor, myVersion.Build, myVersion.Revision) & ")" & " Usuario: " & oUser.NombreCompleto
'            Else
'                pfrmPrincipal.Text = "Usuario: " & oUser.NombreCompleto
'            End If

'        Catch ex As Exception
'            MsgBox(Reflection.MethodBase.GetCurrentMethod.Name.ToString & "  " & ex.Message, MsgBoxStyle.Critical)
'        End Try
'    End Function



'    'Private Shared Sub ConnexioPerduda(ByVal sender As Object, ByVal e As System.Data.StateChangeEventArgs)
'    '    If e.CurrentState = ConnectionState.Broken Or e.CurrentState = ConnectionState.Closed Then
'    '        Call MissatgeConexioPerduda()
'    '    End If
'    'End Sub


'End Class
