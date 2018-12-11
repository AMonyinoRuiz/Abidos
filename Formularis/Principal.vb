Imports System.Deployment
Imports Infragistics.Win.UltraWinToolbars
Imports System.Threading
Imports DevExpress.XtraTreeList.Nodes


Public Class Principal
    Dim Temps_Blinking As Integer
    Public frmWEB As New frmWebBrowser

    ' Dim oProjects As Projects

#Region "Events formulari"
    Private Sub Principal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Seguretat = New M_Seguretat.clsSeguretat(Me)
        Seguretat.pFrameWork40 = False
        Seguretat.pIdioma = M_Mensaje.clsLog.EnumIdioma.Español
        Seguretat.pNomDelPrograma = "Abidos"
        Seguretat.pUtilitzarGridDevExpress = True

        ' Seguretat.pNomDeLaInstanciaSqlServer = "10.7.7.41"
        Select Case My.Computer.Name.ToUpper
            Case "HOME"

                Seguretat.pUsuariPerDefecte = "Carles"
                'Seguretat.pUsuariPerDefecte = "Francisco"
                Seguretat.pContrasenyaPerDefecte = "Carles123"
                Seguretat.pValidarUsuariPerDefecte = True
                Seguretat.pNomDeLaBaseDades = "AbidosDomingoReal"
                Seguretat.pNomDeLaBaseDades2 = "AbidosDomingo"

                'Seguretat.pNomDeLaBaseDades = "AbidosDomingo"
                '  Seguretat.pNomDeLaInstanciaSqlServer = "Home\SQLExpress"
                'Seguretat.pNomDeLaInstanciaSqlServer = "10.7.7.180"
                ' Seguretat.pNomDeLaInstanciaSqlServer = "25.72.164.176" 'ip rodri seguretat
                Seguretat.pNomDeLaInstanciaSqlServer = "Server2012R2"
                ' Seguretat.pUtilitzarFitxerKey = True
                'Case "SQLABIDOS"
                '    Seguretat.pNomDeLaInstanciaSqlServer = "SQLABIDOS\SQLEXPRESS"
                '    Seguretat.pUtilitzarFitxerKey = True
            Case Else
                Seguretat.pNomDeLaBaseDades = "Abidos"
                Seguretat.pNomDeLaBaseDades2 = "AbidosNova"
                Seguretat.pNomDeLaInstanciaSqlServer = "10.7.7.16"   '77116296X  jlojo
                If My.Computer.Name.ToUpper = "HOME" Then
                    Seguretat.pNomDeLaBaseDades = "Abidos"
                    Seguretat.pUtilitzarFitxerKey = False
                Else
                    Seguretat.pUtilitzarFitxerKey = True
                End If
        End Select

        If Seguretat.ObrirFormulariLogin(False) = False Then
            Seguretat = Nothing
            Me.Close()
            End
            Exit Sub
        End If

        Util = Seguretat.pUtil
        Mensaje = Seguretat.pMensaje
        BD = Seguretat.pBD

        Call clsActualizacionBD.EstableixVersioPrograma() 'Fem això per posar en aquesta clase la versió del programa a nivell de base de dades

        If Seguretat.pUtilitzarFitxerKey = True Then
            Call ActualizacionBD()
        End If

        LlistatsADV = New M_LlistatADV.clsLlistatADV(Me)
        LlistatsADV.pBD = BD
        LlistatsADV.pMensaje = Mensaje
        LlistatsADV.pUtil = Util
        LlistatsADV.pToolbarPrincipal = Me.ToolManagerPrincipal
        LlistatsADV.pSeguretat = Seguretat

        AddHandler LlistatsADV.AlFerDobleClickEnElLlistat, AddressOf AlFerDobleClickEnElLlistatADV

        Informes = New M_Informes.clsInformes(Me)
        Informes.pBD = BD
        Informes.pMensaje = Mensaje
        Informes.pUtil = Util

        AddHandler Informes.EventDobleClick, AddressOf EventDobleClickEnUnLlistat

        Call Util.SplashObrir("Iniciando " & Seguretat.pNomDelPrograma & "...")

        oclsPilaFormularis.NetejarPila()

        'Carreguem els formularis en caché
        Dim _Form As M_Seguretat.Formulario
        For Each _Form In Seguretat.RetornaFormularisCache
            Select Case _Form.NombreReal
                Case "frmInstalacion"
                    Dim a1 As New frmInstalacion
                    a1.Entrada(0, _Form.ParametroEntrada)
                    a1.FormObrir(Me)
                    a1.Visible = False
                    oclsPilaFormularis.AfegirFormulari(a1)

                Case "frmProducto"
                    Dim b1 As New frmProducto
                    b1.Entrada()
                    b1.FormObrir(Me)
                    b1.Visible = False
                    oclsPilaFormularis.AfegirFormulari(b1)

                Case "frmParte"
                    Dim c1 As New frmParte
                    c1.Entrada()
                    c1.FormObrir(Me)
                    c1.Visible = False
                    oclsPilaFormularis.AfegirFormulari(c1)

                Case "frmEntrada"
                    Dim d1 As New frmEntrada
                    d1.Entrada(0, EnumEntradaTipo.PedidoVenta)  'posem pedido de venta per posar algo, funciona posis el que posis
                    d1.FormObrir(Me)
                    d1.Visible = False
                    oclsPilaFormularis.AfegirFormulari(d1)
            End Select
        Next

        AddHandler M_Mensaje.clsLog.Actualitzacio_Log, AddressOf Mostrar_Missatges_En_Log
        AddHandler M_UltraGrid.m_UltraGrid.Actualitzacio_Log, AddressOf Mostrar_Missatges_En_Log
        frmPrincipal = Me

        'If My.User.Name.Contains("carles") = False Then
        '    Select Case Math.Round(Screen.PrimaryScreen.Bounds.Width / Screen.PrimaryScreen.Bounds.Height, 1)
        '        Case 1.6
        '            Me.BackgroundImage = My.Resources._1_6
        '        Case 1.3
        '            Me.BackgroundImage = My.Resources._1_33
        '        Case 1.8
        '            Me.BackgroundImage = My.Resources._1_77
        '        Case Else
        '            Me.BackgroundImage = My.Resources._1_6
        '    End Select
        'End If


        If IsNothing(Seguretat.oUser.CampoAux1) = False Then
            If Seguretat.oUser.CampoAux1 = 0 Then
                M_GenericForm.frmBase.pUsarReductorDeMemoria = False
            Else
                M_GenericForm.frmBase.pUsarReductorDeMemoria = True
            End If
        End If

        If Application.ApplicationDeployment.IsNetworkDeployed = True Then
            Call CarregarFormWeb(Util.compruebaConexion)
        End If

        'Dim pepe2 As New Form1
        'pepe2.Entrada()
        'pepe2.FormObrir(Me, True)
        'Call Informes.CarregarMenusLlistats(Seguretat.oUser.NivelSeguridad, Me.ToolManagerPrincipal)
        'Call LlistatsADV.CarregarMenusLlistats(Seguretat.oUser.NivelSeguridad)
        'Call LlistatsADV.CarregarMenusGauge(Seguretat.oUser.NivelSeguridad)
        'Call FerInvisiblesElsFormularisQueNoEstePermis()


        Dim oDTC As New DTCDataContext(BD.Conexion)
        MenuPrincipal = New clsMenuPrincipal(oDTC, Me.TreeMenu)
        MenuPrincipal.CargarTree()

        oEmpresa = oDTC.Empresa.Where(Function(F) F.Predeterminada = True).FirstOrDefault  'Guardem en aquesta variable pública la empresa predeterminada

        If oEmpresa Is Nothing Then
            Mensaje.Mostrar_Mensaje("Para el correcto funcionamiento de Abidos se requiere que haya una empresa predeterminada", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
        End If
        'oProjects = MenuPrincipal.pProjects


        'Call Seguretat.ActivarTemporitzadorControlInactivitat() 'cada cop que es canvii de pantalla s'inicialitzarà el control d'inactivitat


        Call CarregarFotosBotons()
        'Dim oDTC As New DTCDataContext(BD.Conexion)
        PublicProductoDivision = oDTC.Producto_Division.ToArray
        M_Informes.clsInformes.pSeguretat = Seguretat

        If Seguretat.oUser.NivelSeguridad = 1 AndAlso oDTC.Almacen.Where(Function(F) F.Predeterminado = True).Count = 0 Then
            Mensaje.Mostrar_Mensaje("No existe ningún almacén predeterminado, para utilizar la gestión es obligatorio tener uno", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
        End If

        Call Util.SplashTancar()

        Call Util.WaitFormObrir()

        Dim _BIUsuario As BI_Usuario
        Dim _Usuari As Usuario = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault

        For Each _BIUsuario In _Usuari.BI_Usuario.Where(Function(F) F.CargarAlIniciarPrograma = True)
            LlistatsADV.ObrirBI(oDTC, _BIUsuario.ID_BI)
        Next

        Call Util.WaitFormTancar()

        'oInforme.ObrirInformeManteniment(oDTC)
        'oInforme.ObrirInformePreparacio(oDTC, 2, "ID_Instalacion=39", "aaa")
        'oInforme.ObrirInformePlantillaManteniment()

        'Dim thread1 As Threading.Thread = New Threading.Thread(New ThreadStart(AddressOf DisplayThread1))
        'thread1.Start()

        Me.UltraPanel1.Visible = True

        Me.Timer2.Enabled = True
        Me.TimerComprovacioVersioBD.Enabled = True
        AddHandler Me.UltraStatusBarPrincipal.PanelDoubleClick, AddressOf AlFerDobleClickSobreUnPanelDeLaStatusBar

        If oDTC.Empresa.FirstOrDefault.Fax.Contains("46712303") = True Then
            '46712303
            Dim _frmFixe As New frmActividadCRM_Principal
            _frmFixe.Entrada()
            _frmFixe.FormObrir(Me, True)


            'la línea d'abaix es pq no es pugui tancar mai aquesta pestanya
            Me.UltraTabbedMdiManagerPrincipal.ActiveTab.Settings.TabCloseAction = Infragistics.Win.UltraWinTabbedMdi.MdiTabCloseAction.None
            'Me.UltraTabbedMdiManagerPrincipal.ActiveTab.Settings.TabAppearance.BackColor = Color.Red
        End If


 

    End Sub
    Private Sub Principal_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Mensaje Is Nothing Then 'Fem això pq si estas en la pantalla Login i apretes el botó sortir la variable Mensaje es nothing i peta
            Exit Sub
        End If

        If Mensaje.Mostrar_Mensaje("¿Desea salir de la aplicación?", M_Mensaje.Missatge_Modo.PREGUNTA, "Abidos Security") = M_Mensaje.Botons.NO Then
            e.Cancel = True
            Exit Sub
        Else
            'ho fem dos vegades pq la primera vegada no ho neteja tot pq la pila es buida després d'eliminar els forms. Així la segona vegada la pila ja esta buida i tots els forms en cache s'eliminen definitivament
            Call oclsPilaFormularis.NetejarPila()
            Call oclsPilaFormularis.NetejarPila()
            e.Cancel = False

        End If

        Seguretat.TancarSesioControlUsuaris()

        'frmWEB.TancadaForçada = True

    End Sub
    Private Sub Principal_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control = True And e.KeyCode = Keys.OemBackslash Then
            If M_UltraGrid.clsParametres.pNivellSeguretat = 1 Then
                M_UltraGrid.clsParametres.pModoProgramacio = Not M_UltraGrid.clsParametres.pModoProgramacio
                M_GenericForm.frmBase.pModeDisseny = Not M_GenericForm.frmBase.pModeDisseny
                Me.B_ModificarMenu.Visible = Not Me.B_ModificarMenu.Visible
            End If
        End If

        If e.Control = True And e.KeyCode = Keys.OemMinus Then
            Dim i As Integer = M_Disseny.ClsDisseny.pStyle
            i = i + 1
            If i = 6 Then
                i = 0
            End If
            M_Disseny.ClsDisseny.pStyle = i
        End If

        If e.Control = True And e.KeyCode = Keys.Oem5 And e.Shift = True Then
            Me.AppStylistRuntime1.ShowRuntimeApplicationStylingEditor(Me, "Hola")
        End If

        Call Seguretat.ActivarTemporitzadorControlInactivitat() 'cada cop que apretin una tecla el control d'inactivitat s'incialitzarà
    End Sub
    Private Sub Principal_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        'Me.BackgroundImageLayout = ImageLayout.None
        'Me.BackgroundImageLayout = ImageLayout.Stretch
        'Me.Refresh()
    End Sub

#End Region

#Region "Botons a sobre del Tree"
    Private Sub B_Notificacion_Click(sender As Object, e As EventArgs) Handles B_Notificacion.Click
        Me.TreeMenu.Focus()
        Call MenuPrincipal.ObrirFormulari("frmNotificaciones")
    End Sub

    Private Sub B_Cerrar_Click(sender As Object, e As EventArgs)
        Me.TreeMenu.Focus()
        Me.Close()
    End Sub

    Private Sub B_ExpandirContraer_Click(sender As Object, e As EventArgs) Handles B_ExpandirContraer.Click
        Me.TreeMenu.Focus()
        If Me.B_ExpandirContraer.Tag Is Nothing Then
            Me.TreeMenu.ExpandAll()
            Me.B_ExpandirContraer.Tag = "1"
        Else
            Me.TreeMenu.CollapseAll()
            Me.TreeMenu.Nodes(0).Expanded = True
            Me.B_ExpandirContraer.Tag = Nothing
        End If
    End Sub

    Private Sub B_RefrescarMenu_Click(sender As Object, e As EventArgs) Handles B_RefrescarMenu.Click
        Me.TreeMenu.Focus()
        ' Util.WaitFormObrir()
        'Call CargarMenus()
        Call MenuPrincipal.CargarTree()
        '  Util.WaitFormTancar()
    End Sub

    Private Sub B_ModificarMenu_Click(sender As Object, e As EventArgs) Handles B_ModificarMenu.Click
        Me.TreeMenu.Focus()
        Dim frm As New frmMenu
        frm.Entrada()
        frm.FormObrir(Me)
        AddHandler frm.AlTancarForm, AddressOf AlTancarFormConfiguracioMenus
    End Sub

    Private Sub B_Login_Click(sender As Object, e As EventArgs)
        Me.TreeMenu.Focus()
        MenuPrincipal.ObrirFormulari("frmLogin")
        Call MenuPrincipal.CargarTree()
    End Sub
#End Region

#Region "Botons que no s'usen"
    Private Sub UltraButton2_Click(sender As System.Object, e As System.EventArgs) Handles UltraButton2.Click
        'Dim pepe As New RibbonReportDesigner.MainForm
        Dim _Report As New DevExpress.XtraReports.UI.XtraReport



        Dim oDTC As New DTCDataContext(BD.Conexion)
        Dim _Versio As Informe_Apartado_Version = oDTC.Informe_Plantilla_Apartado_Version.Where(Function(F) F.ID_Informe_Plantilla_Apartado_Version = 6).Select(Function(F) F.Informe_Apartado_Version).FirstOrDefault
        Dim dStream As System.IO.MemoryStream = New System.IO.MemoryStream(_Versio.Fichero.ToArray, 0, _Versio.Fichero.Length)
        dStream.Position = 0
        'DevExpress.XtraReports.UI.XtraReport.FromStream(dStream, True)
        _Report.DataSource = oDTC.Propuesta
        _Report.LoadLayout(dStream)

        Dim pt As New DevExpress.XtraReports.UI.ReportPrintTool(_Report)
        pt.ShowPreview()

        'Dim designForm As New DevExpress.XtraReports.UserDesigner.XRDesignForm
        'designForm.OpenReport(_Report)
        'designForm.Show()


    End Sub

    Private Sub UltraButton3_Click(sender As Object, e As EventArgs) Handles UltraButton3.Click
        'Dim oDTC As New DTCDataContext(BD.Conexion)
        'Dim _Clients As IQueryable(Of Cliente) = oDTC.Cliente.Where(Function(F) F.ID_Cliente_Tipo = 2 And F.ID_Cliente < 3000)
        'Dim _Importats As IQueryable(Of clientesImportados) = oDTC.clientesImportados

        'Dim _NovaCampaña As New Campaña
        '_NovaCampaña = oDTC.Campaña.FirstOrDefault


        'Dim _Client As Cliente
        'Dim i As Integer
        'For Each _Client In _Clients
        '    Dim _NomEmpresa As String
        '    _NomEmpresa = _Client.Nombre.Replace(",", "")
        '    Dim _Importat = _Importats.Where(Function(F) F.Nombre.Replace(",", "") = _NomEmpresa).FirstOrDefault
        '    If _Importat Is Nothing = False Then

        '        If IsNothing(_Importat.Contacto) = False AndAlso IsDBNull(_Importat.Contacto) = False Then
        '            Dim _Contacte As New Cliente_Contacto
        '            _Contacte.Nombre = _Importat.Contacto
        '            If _Importat.Telefono.HasValue = True Then
        '                _Contacte.Telefono = _Importat.Telefono
        '            End If

        '            If _Importat.Telefono2.HasValue = True Then
        '                _Contacte.Telefono2 = _Importat.Telefono2
        '            End If
        '            _Contacte.Email = _Importat.Email
        '            _Client.Cliente_Contacto.Add(_Contacte)
        '        End If

        '        Dim _NouClientCampaña As New Campaña_Cliente
        '        _NouClientCampaña.Cliente = _Client
        '        _NouClientCampaña.ID_Campaña_Cliente_Seguimiento_Estado = 1
        '        _NovaCampaña.Campaña_Cliente.Add(_NouClientCampaña)

        '        Dim _Seguiment As New Campaña_Cliente_Seguimiento
        '        _Seguiment.ID_Campaña_Cliente_Seguimiento_Estado = 1
        '        _Seguiment.FechaIntroduccion = Now.Date
        '        _Seguiment.ID_Usuario = 1
        '        _Seguiment.Descripcion = _Importat.MotivoEstado
        '        _NouClientCampaña.Campaña_Cliente_Seguimiento.Add(_Seguiment)
        '    End If
        'Next

        'oDTC.SubmitChanges()
        'MsgBox("Dades Importades correctament")

    End Sub

    Private Sub UltraButton4_Click(sender As Object, e As EventArgs) Handles UltraButton4.Click
        'Dim oDTC As New DTCDataContext(BD.Conexion)
        'Dim _Clients As IQueryable(Of Cliente) = oDTC.Cliente.Where(Function(F) F.ID_Cliente_Tipo = 2 And (F.ID_Cliente >= 3000 And F.ID_Cliente < 6000))
        'Dim _Importats As IQueryable(Of clientesImportados) = oDTC.clientesImportados

        'Dim _NovaCampaña As New Campaña
        '_NovaCampaña = oDTC.Campaña.FirstOrDefault


        'Dim _Client As Cliente
        'Dim i As Integer

        'For Each _Client In _Clients
        '    Dim _NomEmpresa As String
        '    _NomEmpresa = _Client.Nombre.Replace(",", "")
        '    Dim _Importat = _Importats.Where(Function(F) F.Nombre.Replace(",", "") = _NomEmpresa).FirstOrDefault
        '    If _Importat Is Nothing = False Then

        '        If IsNothing(_Importat.Contacto) = False AndAlso IsDBNull(_Importat.Contacto) = False Then
        '            Dim _Contacte As New Cliente_Contacto
        '            _Contacte.Nombre = _Importat.Contacto
        '            If _Importat.Telefono.HasValue = True Then
        '                _Contacte.Telefono = _Importat.Telefono
        '            End If

        '            If _Importat.Telefono2.HasValue = True Then
        '                _Contacte.Telefono2 = _Importat.Telefono2
        '            End If
        '            _Contacte.Email = _Importat.Email
        '            _Client.Cliente_Contacto.Add(_Contacte)
        '        End If

        '        Dim _NouClientCampaña As New Campaña_Cliente
        '        _NouClientCampaña.Cliente = _Client
        '        _NouClientCampaña.ID_Campaña_Cliente_Seguimiento_Estado = 1
        '        _NovaCampaña.Campaña_Cliente.Add(_NouClientCampaña)

        '        Dim _Seguiment As New Campaña_Cliente_Seguimiento
        '        _Seguiment.ID_Campaña_Cliente_Seguimiento_Estado = 1
        '        _Seguiment.FechaIntroduccion = Now.Date
        '        _Seguiment.ID_Usuario = 1
        '        _Seguiment.Descripcion = _Importat.MotivoEstado
        '        _NouClientCampaña.Campaña_Cliente_Seguimiento.Add(_Seguiment)
        '    End If
        'Next

        'oDTC.SubmitChanges()
        'MsgBox("Dades Importades correctament")
    End Sub

    Private Sub UltraButton5_Click(sender As Object, e As EventArgs) Handles UltraButton5.Click
        'Dim oDTC As New DTCDataContext(BD.Conexion)
        'Dim _Clients As IQueryable(Of Cliente) = oDTC.Cliente.Where(Function(F) F.ID_Cliente_Tipo = 2 And (F.ID_Cliente >= 6000 And F.ID_Cliente < 9000))
        'Dim _Importats As IQueryable(Of clientesImportados) = oDTC.clientesImportados

        'Dim _NovaCampaña As New Campaña
        '_NovaCampaña = oDTC.Campaña.FirstOrDefault



        'Dim _Client As Cliente
        'Dim i As Integer
        'For Each _Client In _Clients
        '    Dim _NomEmpresa As String
        '    _NomEmpresa = _Client.Nombre.Replace(",", "")
        '    Dim _Importat = _Importats.Where(Function(F) F.Nombre.Replace(",", "") = _NomEmpresa).FirstOrDefault
        '    If _Importat Is Nothing = False Then

        '        If IsNothing(_Importat.Contacto) = False AndAlso IsDBNull(_Importat.Contacto) = False Then
        '            Dim _Contacte As New Cliente_Contacto
        '            _Contacte.Nombre = _Importat.Contacto
        '            If _Importat.Telefono.HasValue = True Then
        '                _Contacte.Telefono = _Importat.Telefono
        '            End If

        '            If _Importat.Telefono2.HasValue = True Then
        '                _Contacte.Telefono2 = _Importat.Telefono2
        '            End If
        '            _Contacte.Email = _Importat.Email
        '            _Client.Cliente_Contacto.Add(_Contacte)
        '        End If

        '        Dim _NouClientCampaña As New Campaña_Cliente
        '        _NouClientCampaña.Cliente = _Client
        '        _NouClientCampaña.ID_Campaña_Cliente_Seguimiento_Estado = 1
        '        _NovaCampaña.Campaña_Cliente.Add(_NouClientCampaña)

        '        Dim _Seguiment As New Campaña_Cliente_Seguimiento
        '        _Seguiment.ID_Campaña_Cliente_Seguimiento_Estado = 1
        '        _Seguiment.FechaIntroduccion = Now.Date
        '        _Seguiment.ID_Usuario = 1
        '        _Seguiment.Descripcion = _Importat.MotivoEstado
        '        _NouClientCampaña.Campaña_Cliente_Seguimiento.Add(_Seguiment)
        '    End If
        'Next

        'oDTC.SubmitChanges()
        'MsgBox("Dades Importades correctament")
    End Sub

    Private Sub UltraButton6_Click(sender As Object, e As EventArgs) Handles UltraButton6.Click
        'Dim _frmFixe As New frmActividadCRM_Principal
        '_frmFixe.Entrada()
        '_frmFixe.FormObrir(Me, True)


        'Dim oDTC As New DTCDataContext(BD.Conexion)
        'Dim _Clients As IQueryable(Of Cliente) = oDTC.Cliente.Where(Function(F) F.ID_Cliente_Tipo = 2 And (F.ID_Cliente >= 9000 And F.ID_Cliente < 14000))
        'Dim _Importats As IQueryable(Of clientesImportados) = oDTC.clientesImportados

        'Dim _NovaCampaña As New Campaña
        '_NovaCampaña = oDTC.Campaña.FirstOrDefault



        'Dim _Client As Cliente
        'Dim i As Integer
        'For Each _Client In _Clients
        '    Dim _NomEmpresa As String
        '    _NomEmpresa = _Client.Nombre.Replace(",", "")
        '    Dim _Importat = _Importats.Where(Function(F) F.Nombre.Replace(",", "") = _NomEmpresa).FirstOrDefault
        '    If _Importat Is Nothing = False Then

        '        If IsNothing(_Importat.Contacto) = False AndAlso IsDBNull(_Importat.Contacto) = False Then
        '            Dim _Contacte As New Cliente_Contacto
        '            _Contacte.Nombre = _Importat.Contacto
        '            If _Importat.Telefono.HasValue = True Then
        '                _Contacte.Telefono = _Importat.Telefono
        '            End If

        '            If _Importat.Telefono2.HasValue = True Then
        '                _Contacte.Telefono2 = _Importat.Telefono2
        '            End If
        '            _Contacte.Email = _Importat.Email
        '            _Client.Cliente_Contacto.Add(_Contacte)
        '        End If

        '        Dim _NouClientCampaña As New Campaña_Cliente
        '        _NouClientCampaña.Cliente = _Client
        '        _NouClientCampaña.ID_Campaña_Cliente_Seguimiento_Estado = 1
        '        _NovaCampaña.Campaña_Cliente.Add(_NouClientCampaña)

        '        Dim _Seguiment As New Campaña_Cliente_Seguimiento
        '        _Seguiment.ID_Campaña_Cliente_Seguimiento_Estado = 1
        '        _Seguiment.FechaIntroduccion = Now.Date
        '        _Seguiment.ID_Usuario = 1
        '        _Seguiment.Descripcion = _Importat.MotivoEstado
        '        _NouClientCampaña.Campaña_Cliente_Seguimiento.Add(_Seguiment)
        '    End If
        'Next

        'oDTC.SubmitChanges()
        'MsgBox("Dades Importades correctament")
    End Sub
#End Region

#Region "Timers"
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'Me.UltraStatusBarPrincipal.Panels(0).Text = ""
        'Me.Timer1.Stop()


        Temps_Blinking = Temps_Blinking + 1
        Me.UltraStatusBarPrincipal.Panels(0).Appearance.ForeColor = Color.Black
        Dim Color_Original As Color = Me.UltraStatusBarPrincipal.Panels(1).Appearance.BackColor
        If Me.UltraStatusBarPrincipal.Panels(0).Appearance.BackColor = Color_Original Then
            If Me.UltraStatusBarPrincipal.Panels(0).Tag Is Nothing Then
                Me.UltraStatusBarPrincipal.Panels(0).Appearance.BackColor = Color.LightGreen
            Else
                Me.UltraStatusBarPrincipal.Panels(0).Appearance.BackColor = Color.LightCoral
            End If


            ' Me.UltraStatusBarPrincipal.Panels(0).Appearance.BackColor2 = Color.LightBlue
            ' Me.UltraStatusBarPrincipal.Panels(0).Appearance.BackGradientStyle = GradientStyle.Vertical

        Else
            Me.UltraStatusBarPrincipal.Panels(0).Appearance.BackColor = Color_Original
        End If
        If Temps_Blinking = 8 Then
            Me.UltraStatusBarPrincipal.Panels(0).Appearance.BackColor = Color_Original
            Me.UltraStatusBarPrincipal.Panels(0).Text = ""
            Me.Timer1.Stop()
        End If
    End Sub
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim x As Process = Process.GetCurrentProcess
        Me.UltraStatusBarPrincipal.Panels("Memory").Text = (x.WorkingSet / 1024).ToString("N0") & " K"
        If (x.WorkingSet / 1024) > 1000000 Then
            Me.UltraStatusBarPrincipal.Panels("Memory").Appearance.ForeColor = Color.Black
            Me.UltraStatusBarPrincipal.Panels("Memory").Appearance.BackColor = Color.Red
        Else
            Me.UltraStatusBarPrincipal.Panels("Memory").Appearance.ForeColor = Me.UltraStatusBarPrincipal.Panels("Notificaciones").Appearance.ForeColor
            Me.UltraStatusBarPrincipal.Panels("Memory").Appearance.BackColor = Me.UltraStatusBarPrincipal.Panels("Notificaciones").Appearance.BackColor
        End If


        If Seguretat.pConnexioPerduda = False Then 'Si hi ha connexió anirem comprovant les notificacions
            Try
                Me.B_Notificacion.Visible = False
                Dim _Num As Integer = BD.RetornaValorSQL("Select Count(*) From Notificacion Where ID_Usuario_Destino=" & Seguretat.oUser.ID_Usuario & " and Leido=0")


                Dim _Texte As String = ""
                If _Num <> 0 Then
                    _Texte = "Tienes (" & _Num & ") notificaciones nuevas"
                    Me.B_Notificacion.Visible = True
                End If

                Me.UltraStatusBarPrincipal.Panels("Notificaciones").Text = _Texte
            Catch ex As Exception
                'Fem això per si perd la connexio
            End Try
        End If
    End Sub
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        If Me.UltraStatusBarPrincipal.Panels("Notificaciones").Text.Length > 0 Then
            If Me.UltraStatusBarPrincipal.Panels("Notificaciones").Appearance.ForeColor = Me.UltraStatusBarPrincipal.Panels(1).Appearance.ForeColor Then
                Me.UltraStatusBarPrincipal.Panels("Notificaciones").Appearance.ForeColor = Color.Red
                Me.B_Notificacion.ImageIndex = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf("mail_red.png")
            Else
                Me.UltraStatusBarPrincipal.Panels("Notificaciones").Appearance.ForeColor = Me.UltraStatusBarPrincipal.Panels(1).Appearance.ForeColor
                Me.B_Notificacion.ImageIndex = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf("mail_green.png")
                'Me.B_Notificacion.Visible = Nothing
            End If
        Else
            Me.UltraStatusBarPrincipal.Panels("Notificaciones").Appearance.ForeColor = Me.UltraStatusBarPrincipal.Panels(1).Appearance.ForeColor
            Me.B_Notificacion.ImageIndex = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf("mail_green.png")
            'Me.B_Notificacion.Visible = Nothing
        End If
    End Sub
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles TimerComprovacioVersioBD.Tick
        If Seguretat.pConnexioPerduda = False Then 'Si hi ha connexió anirem comprovant les notificacions
            Try

                If clsActualizacionBD.pInformatAlUsuariVersioDiferent = False Then
                    If clsActualizacionBD.RetornaUltimaVersioBD <> clsActualizacionBD.pVersioActualPrograma Then
                        clsActualizacionBD.pInformatAlUsuariVersioDiferent = True
                        Mensaje.Mostrar_Mensaje("Se ha detectado una nueva versión del software ABIDOS, por favor salga del programa para que se actualize.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                        Me.TreeMenu.Appearance.Empty.BackColor = Color.LightCoral
                        Me.TreeMenu.Appearance.Row.BackColor = Color.LightCoral
                        Me.Text = "Se ha detectado una nueva versión del software ABIDOS, por favor salga del programa para que se actualize."

                    End If
                End If
            Catch ex As Exception

                'Fem això per si perd la connexio
            End Try
        End If
    End Sub
#End Region

#Region "Varis"
    Public Sub CarregarFormWeb(ByVal pURL As String)
        If pURL.Length > 3 Then 'Si te més de 3 caracters
            If frmWEB.FormCarregat = False Then
                frmWEB.Entrada(pURL)
                frmWEB.MdiParent = Me
                frmWEB.Show()

                'Fem això pq el formulari de la web no es tanqui mai
                Me.UltraTabbedMdiManagerPrincipal.ActiveTab.Settings.AllowClose = False
                Me.UltraTabbedMdiManagerPrincipal.ActiveTab.Settings.CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Never
                Me.UltraTabbedMdiManagerPrincipal.ActiveTab.Settings.TabCloseAction = Infragistics.Win.UltraWinTabbedMdi.MdiTabCloseAction.None
            Else
                frmWEB.Entrada(pURL)
                frmWEB.Activate()
            End If
        End If
    End Sub

    Private Sub Mostrar_Missatges_En_Log(ByVal pTexte As String, ByVal pFuncio As String, ByVal pTipoMensaje As M_Mensaje.Missatge_Modo, ByVal pStackTrace As String)
        Dim Texte As String = ""
        Try
            With UltraStatusBarPrincipal.Panels(0)
                Texte = pTexte
                If pTipoMensaje = M_Mensaje.Missatge_Modo.ERRORS Then
                    If pFuncio = "" Then
                        Texte = pTexte
                    Else
                        Texte = "[" & pFuncio & "] " & pTexte
                    End If
                    If pStackTrace Is Nothing OrElse pStackTrace.Length = 0 Then 'Si no hi ha stacktrace vol dir que no ha estat un error automàtic i que ha estat un error de tipus (texto requerido donat per mi per exemple)
                        .Tag = Nothing
                    Else
                        .Tag = "Error"
                    End If

                Else
                    '.Appearance.ForeColor = Color.Green
                    .Tag = Nothing
                End If

                .Text = Texte

                Me.Timer1.Stop()
                Me.Timer1.Interval = 500
                Temps_Blinking = 0
                Me.Timer1.Start()

                Call Seguretat.ActivarTemporitzadorControlInactivitat()
            End With
        Catch ex As Exception
            MsgBox(Reflection.MethodBase.GetCurrentMethod.Name.ToString & "  " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Sub TancaPrograma()
        End
    End Sub

    Private Sub UltraTabbedMdiManagerPrincipal_TabClosing(sender As Object, e As Infragistics.Win.UltraWinTabbedMdi.CancelableMdiTabEventArgs) Handles UltraTabbedMdiManagerPrincipal.TabClosing
        If e.Tab.IsFormEnabled = False Then
            e.Cancel = True
        End If
    End Sub

    Private Sub AlFerDobleClickSobreUnPanelDeLaStatusBar(Sender As Object, ByVal e As Infragistics.Win.UltraWinStatusBar.PanelClickEventArgs)
        If e.Panel.Key = "Notificaciones" Then
            Call MenuPrincipal.ObrirFormulari("frmNotificaciones")
            'Dim _frm As New frmNotificaciones
            '_frm.Entrada()
            '_frm.FormObrir(Me, True)
        End If
    End Sub

    Private Sub CarregarFotosBotons()
        Me.B_Notificacion.Image = Nothing
        Me.B_Notificacion.ImageList = Me.M_Images1.SharedImageCollection1
        Me.B_Notificacion.ImageIndex = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf("mail_red.png")

        Me.B_ModificarMenu.Image = Nothing
        Me.B_ModificarMenu.ImageList = Me.M_Images1.SharedImageCollection1
        Me.B_ModificarMenu.ImageIndex = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf("google_webmaster_tools.png")

        Me.B_ExpandirContraer.Image = Nothing
        Me.B_ExpandirContraer.ImageList = Me.M_Images1.SharedImageCollection1
        Me.B_ExpandirContraer.ImageIndex = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf("folders_explorer.png")

        Me.B_RefrescarMenu.Image = Nothing
        Me.B_RefrescarMenu.ImageList = Me.M_Images1.SharedImageCollection1
        Me.B_RefrescarMenu.ImageIndex = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf("arrow_refresh.png")

        'Me.B_Login.Image = Nothing
        'Me.B_Login.ImageList = Me.M_Images1.SharedImageCollection1
        'Me.B_Login.ImageIndex = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf("group_key.png")

        'Me.B_Cerrar.Image = Nothing
        'Me.B_Cerrar.ImageList = Me.M_Images1.SharedImageCollection1
        'Me.B_Cerrar.ImageIndex = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf("cancel.png")
    End Sub
#End Region

    Private Sub EventDobleClickEnUnLlistat(ByVal pNomTaula As String, ByVal pID As Integer)
        Dim oDTC As New DTCDataContext(BD.Conexion)

        Select Case pNomTaula
            Case "Cliente"
                Dim frm As New frmCliente
                frm.Entrada(pID)
                frm.FormObrir(frmPrincipal, True)
            Case "Proveedor"
                Dim frm As New frmProveedor
                frm.Entrada(pID)
                frm.FormObrir(frmPrincipal, True)
            Case "Personal"
                Dim frm As New frmPersonal
                frm.Entrada(pID)
                frm.FormObrir(frmPrincipal, True)
            Case "Instalacion"
                Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                frm.Entrada(pID)
                frm.FormObrir(frmPrincipal, True)
            Case "Parte"
                Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                frm.Entrada(pID)
                frm.FormObrir(frmPrincipal, True)
            Case "Propuesta"

                Dim _Propuesta As Propuesta = oDTC.Propuesta.Where(Function(F) F.ID_Propuesta = pID).FirstOrDefault
                If _Propuesta Is Nothing = False Then

                    Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
                    frm.Entrada(_Propuesta.ID_Instalacion)
                    frm.FormObrir(frmPrincipal, True)

                    Dim frm2 As New frmPropuesta
                    frm2.Entrada(frm.oLinqInstalacion, frm.oDTC, _Propuesta.ID_Propuesta)
                    AddHandler frm2.FormClosing, AddressOf frm.AlTancarfrmPropuesta
                    frm2.FormObrir(frm)

                End If

            Case "Producto"
                Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
                frm.Entrada(pID)
                frm.FormObrir(frmPrincipal, True)
            Case "Producto División"

            Case "Usuarios"

        End Select

    End Sub

    Private Sub AlFerDobleClickEnElLlistatADV(ByVal pIDLlistatADV As Integer, ByVal pIDSeleccionatEnGrid As Integer)
        Dim oDTC As New DTCDataContext(BD.Conexion)
        Dim _llistatADV As ListadoADV
        _llistatADV = oDTC.ListadoADV.Where(Function(F) F.ID_ListadoADV = pIDLlistatADV).FirstOrDefault

        Dim juan As Type
        juan = Type.GetType("Abidos." & _llistatADV.Formulario.NombreReal)

        Dim _frm2 As Object
        _frm2 = Activator.CreateInstance(juan)
        If _llistatADV.CodigoApertura Is Nothing = False AndAlso _llistatADV.CodigoApertura.Length > 0 Then
            _frm2.Entrada(pIDSeleccionatEnGrid, pIDSeleccionatEnGrid)
        Else
            _frm2.Entrada(pIDSeleccionatEnGrid)
        End If

        _frm2.FormObrir(Me)
    End Sub

    Private Sub AlTancarFormConfiguracioMenus()
        MenuPrincipal.CargarTree()
    End Sub

#Region "Codi que no s'usa"
    'Private Sub ToolManagerPrincipal_BeforeToolDropdown(sender As Object, e As BeforeToolDropdownEventArgs) Handles ToolManagerPrincipal.BeforeToolDropdown
    '    If e.Tool.Key = "ListadosADV" Then
    '        Call LlistatsADV.DestruirMenusLlistats()
    '        Call LlistatsADV.CarregarMenusLlistats(Seguretat.oUser.NivelSeguridad)
    '    End If
    'End Sub

    'Public Sub FerInvisiblesElsFormularisQueNoEstePermis()
    '    Try
    '        Dim _Boto As ToolBase
    '        Dim oDTC As New DTCDataContext(BD.Conexion)

    '        For Each _Boto In Me.ToolManagerPrincipal.Tools
    '            If oUser.Usuario_Grupo.Formulario_Usuario_Grupo.Where(Function(F) F.Formulario.NombreReal = CType(_Boto.Tag, String) And F.Visualizar = False).Count > 0 Then
    '                _Boto.SharedProps.Visible = False
    '            Else
    '                _Boto.SharedProps.Visible = True
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub TEmporitzadorComprovarConnexio_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TemporitzadorComprovarConnexio.Tick
    '    If BD.ProvarConexioActiva() = False Then
    '        Call MissatgeConexioPerduda()
    '    End If
    'End Sub

    'Private Shared Sub MissatgeConexioPerduda()
    '    ' If BD.Conectado = False Then
    '    frmPrincipal.TemporitzadorComprovarConnexio.Enabled = False
    '    Dim Missatge As String = "Atención, se ha perdido la conexión con el servidor. Desea intentar reconectar? (Si selecciona NO se cerrará el programa)"
    '    If MsgBox(Missatge, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
    '        Util.WaitFormObrir()
    '        If BD.Conectar(BD.pBDNomInstancia, BD.pBDNomBaseDades, BD.pBDUsuari, BD.pBDContrasenya) Then
    '            Util.WaitFormTancar()
    '            frmPrincipal.TemporitzadorComprovarConnexio.Enabled = True
    '            frmPrincipal.Enabled = True
    '        Else
    '            Util.WaitFormTancar()
    '            Mensaje.Mostrar_Mensaje("La conexión con la base de datos no funciona", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
    '            Call MissatgeConexioPerduda()
    '        End If
    '    Else
    '        End
    '    End If
    '    'End If
    'End Sub

    'Private Sub TemporitzadorControlInactivitat_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles TemporitzadorControlInactivitat.Tick
    '    'Call clsInici.MostrarPantallaLogin(True)
    '    Me.TemporitzadorControlInactivitat.Enabled = False
    '    Call CridarFormulariLogin(True)
    '    Me.TemporitzadorControlInactivitat.Enabled = True
    'End Sub

    'Private Sub ActivarTemporitzadorControlInactivitat()
    '    'si no s'ha posat temps d'inactivitat voldrà dir que no és vol apareixer la pantalla login quan hi hagi X temps
    '    If oUser.TiempoInactividadPantallaLogin <> 0 Then
    '        Me.TemporitzadorControlInactivitat.Stop()
    '        Me.TemporitzadorControlInactivitat.Interval = oUser.TiempoInactividadPantallaLogin * 60000
    '        Me.TemporitzadorControlInactivitat.Start()
    '        Me.TemporitzadorControlInactivitat.Enabled = True
    '    End If
    'End Sub

    'Private Sub CridarFormulariLogin(Optional ByVal pCarregaPerInactivitat As Boolean = False)
    '    Try
    '        Me.TemporitzadorComprovarConnexio.Enabled = False

    '        If pCarregaPerInactivitat = False Then
    '            If Me.MdiChildren.Count > 0 Then
    '                If Mensaje.Mostrar_Mensaje("¿Desea entrar en el programa con otro usuario? En caso afirmativo se procederá a cerrar todos los formularios abiertos", M_Mensaje.Missatge_Modo.PREGUNTA) = M_Mensaje.Botons.NO Then
    '                    Exit Sub
    '                Else
    '                    Dim _frm2 As M_GenericForm.frmBase
    '                    For Each _frm2 In Me.MdiChildren
    '                        _frm2.Close()
    '                    Next
    '                End If
    '            End If
    '        End If

    '        'Posem els formularis invisibles
    '        Dim _Pila As New ArrayList
    '        Dim _frm As M_GenericForm.frmBase

    '        For Each _frm In Me.MdiChildren
    '            If _frm.Visible = True Then
    '                _frm.Visible = False
    '                _Pila.Add(_frm)

    '            End If
    '        Next

    '        Dim frm As New frmLogin
    '        frm.Entrada(Me, pCarregaPerInactivitat)
    '        frm.ShowDialog()

    '        'Posem els formularis visibles
    '        For Each _frm In _Pila
    '            _frm.Visible = True
    '        Next

    '        If pCarregaPerInactivitat = False Then
    '            Call DestruirMenusLlistats()
    '            Call CarregarMenusLlistats()
    '            Call FerInvisiblesElsFormularisQueNoEstePermis()
    '        End If

    '        Me.TemporitzadorComprovarConnexio.Enabled = True
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub


    'Private Sub ToolManagerPrincipal_ToolKeyDown(sender As Object, e As Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs) Handles ToolManagerPrincipal.ToolKeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        Dim TXT As Infragistics.Win.UltraWinToolbars.TextBoxTool
    '        TXT = e.Tool
    '        Select Case Mid(TXT.Text.ToUpper, 1, 1).ToLower
    '            Case "i"

    '                Dim Texte As String = Mid(TXT.Text, 2, Len(TXT.Text) - 1)
    '                If IsNumeric(Texte) Then
    '                    If BD.RetornaValorSQL("Select Count(*) From Instalacion Where ID_Instalacion=" & Texte) > 0 Then
    '                        Dim frm As New frmInstalacion
    '                        frm.Entrada(Texte)
    '                        frm.FormObrir(Me, True)
    '                    End If
    '                End If
    '                TXT.Text = ""
    '            Case "p"Dim frm As new frmProducto
    '                Dim Texte As String = Mid(TXT.Text, 2, Len(TXT.Text) - 1)
    '                If IsNumeric(Texte) Then
    '                    If BD.RetornaValorSQL("Select Count(*) From Parte Where ID_Parte=" & Texte) > 0 Then
    '                        Dim frm As 


    '                        frm.Entrada(Texte)
    '                        frm.FormObrir(Me, True)
    '                    End If
    '                End If
    '                TXT.Text = ""
    '            Case "="
    '                Dim Texte As String = Mid(TXT.Text, 2, Len(TXT.Text) - 1)
    '                If BD.RetornaValorSQL("Select Count(*) From Producto Where codigo='" & Texte & "'") > 0 Then
    '                    Dim IDArticulo As Integer = BD.RetornaValorSQL("Select ID_Producto From Producto Where codigo='" & Texte & "'")
    '                    Dim frm As New frmProducto
    '                    frm.Entrada(IDArticulo)
    '                    frm.FormObrir(Me, True)
    '                End If
    '                TXT.Text = ""
    '        End Select
    '    End If
    'End Sub


    'Private Sub DisplayThread1()
    '    Dim pepe As New frmInstalacion
    '    pepe.Entrada()
    '    pepe.FormObrir(Me)
    'End Sub


    'Private Sub ToolManagerPrincipal_ToolClick(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles ToolManagerPrincipal.ToolClick
    '    Temps = Date.Now.Ticks
    '    Select Case e.Tool.Key.ToLower
    '        Case "m_producto"
    '            Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
    '            frm.Entrada()
    '            frm.FormObrir(Me)
    '        Case "m_manteniment"
    '            Dim frm As New frmManteniment
    '            frm.Entrada()
    '            frm.FormObrir(Me)
    '        Case "m_instalacion"
    '            Dim frm As New frmInstalacion '= oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion))
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_cliente"
    '            Dim frm As New frmCliente
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_futuro_cliente"
    '            Dim frm As New frmCliente
    '            frm.Entrada(0, frmCliente.TipoEntrada.FuturoCliente)
    '            frm.FormObrir(Me)

    '        Case "m_proveedor"
    '            Dim frm As New frmProveedor
    '            frm.Entrada()
    '            frm.FormObrir(Me)
    '        Case "m_cableado"
    '            Dim frm As New frmCables
    '            frm.Entrada()
    '            frm.FormObrir(Me)
    '        Case "m_personal"
    '            Dim frm As New frmPersonal
    '            frm.Entrada()
    '            frm.FormObrir(Me)
    '        Case "m_parte"
    '            Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
    '            frm.Entrada()
    '            frm.FormObrir(Me)
    '        Case "m_receptora"
    '            Dim frm As New frmReceptora
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_manteniment_divisions"
    '            Dim frm As New frmManteniment_Familias
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_mantenimiento_informe"
    '            Dim oDTC As New DTCDataContext(BD.Conexion)
    '            Informes.ObrirInformeManteniment(oDTC)
    '            'Dim frm As New frmManteniment_Informe
    '            'frm.Entrada()
    '            'frm.FormObrir(Me)

    '        Case "m_mantenimiento_listado"
    '            Dim oDTC As New DTCDataContext(BD.Conexion)
    '            Informes.ObrirLlistatManteniment(oDTC)
    '            'Dim frm As New frmManteniment_Informe
    '            'frm.Entrada()
    '            'frm.FormObrir(Me)

    '        Case "m_informe_plantilla"
    '            Informes.ObrirInformePlantillaManteniment()
    '            'Dim frm As New frmMantenimient_Informe_Plantilla
    '            'frm.Entrada()
    '            'frm.FormObrir(Me)

    '        Case "m_mantenimiento_grados"
    '            Dim frm As New frmManteniment_Grados
    '            frm.Entrada()
    '            frm.FormObrir(Me)
    '        Case "m_revision"
    '            Dim frm As New frmListado_ParteRevisiones
    '            frm.Entrada()
    '            frm.FormObrir(Me)
    '        Case "m_usuario"
    '            Seguretat.ObrirFormulariUsuaris()
    '            'Dim frm As New frmManteniment_Usuario
    '            'frm.Entrada()
    '            'frm.FormObrir(Me)
    '        Case "m_grupo_usuario"
    '            Seguretat.ObrirFormulariGrupUsuaris()
    '            'Dim frm As New frmManteniment_Usuario_Grupo
    '            'frm.Entrada()
    '            'frm.FormObrir(Me)
    '        Case "m_formulario"
    '            Seguretat.ObrirFormulariMantenimentFormularis()
    '            'Dim frm As New frmManteniment_Formulario
    '            'frm.Entrada()
    '            'frm.FormObrir(Me)
    '        Case "m_cambiocontraseña"
    '            Seguretat.ObrirFormulariCanviContrasenya()
    '            'Dim frm As New frmManteniment_Formulario
    '            'frm.Entrada()
    '            'frm.FormObrir(Me)
    '        Case "m_login"
    '            'Call CridarFormulariLogin()
    '            Seguretat.ObrirFormulariLogin(True)

    '            'If pCarregaPerInactivitat = False Then
    '            Call Informes.DestruirMenusLlistats(Me.ToolManagerPrincipal)
    '            Call Informes.CarregarMenusLlistats(Seguretat.oUser.NivelSeguridad, Me.ToolManagerPrincipal)
    '            Call LlistatsADV.DestruirMenusLlistats()
    '            Call LlistatsADV.CarregarMenusLlistats(Seguretat.oUser.NivelSeguridad)



    '            'Call FerInvisiblesElsFormularisQueNoEstePermis()
    '            'End If

    '        Case "m_listados"
    '            'Dim frm As New frmManteniment_Listado
    '            'frm.Entrada()
    '            'frm.FormObrir(Me)

    '            'Case "m_script"
    '            '    Dim frm As New frmManteniment_Script
    '            '    frm.Entrada()
    '            '    frm.FormObrir(Me)

    '            'Case "grid"
    '            '    Dim frm As New FrmNewGrid
    '            '    frm.Entrada()
    '            '    frm.FormObrir(Me)

    '        Case "m_logtransacciones"
    '            Dim frm As New frmLogTransacciones
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_imputacionhoras"
    '            Dim frm As New frmImputacionHoras
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_empresa"
    '            Dim frm As New frmEmpresa
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_resumenhoras"
    '            Dim frm As New frmResumenHoras
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_campañas"
    '            Dim frm As New frmCampaña
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_telemarketing"
    '            Dim frm As New frmCampaña_Telemarketing
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_almacen"
    '            Dim frm As New frmAlmacen
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_pedido_compra"
    '            Dim frm As New frmEntrada
    '            frm.Entrada(0, EnumEntradaTipo.PedidoCompra)
    '            frm.FormObrir(Me)

    '        Case "m_albaran_compra"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.AlbaranCompra)
    '            frm.FormObrir(Me)

    '        Case "m_factura_compra"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.FacturaCompra)
    '            frm.FormObrir(Me)

    '        Case "m_devolucion_compra"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.DevolucionCompra)
    '            frm.FormObrir(Me)

    '        Case "m_devolucion_venta"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.DevolucionVenta)
    '            frm.FormObrir(Me)

    '        Case "m_albaraninicializacion"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.Inicializacion)
    '            frm.FormObrir(Me)

    '        Case "m_regularizacion"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.Regularizacion)
    '            frm.FormObrir(Me)

    '        Case "m_pedido_venta"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.PedidoVenta)
    '            frm.FormObrir(Me)

    '        Case "m_albaran_venta"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.AlbaranVenta)
    '            frm.FormObrir(Me)

    '        Case "m_factura_venta"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.FacturaVenta)
    '            frm.FormObrir(Me)

    '        Case "m_traspaso_almacen"
    '            Dim frm As frmEntrada = oclsPilaFormularis.ObrirFormulari(GetType(frmEntrada))
    '            frm.Entrada(0, EnumEntradaTipo.TraspasoAlmacen)
    '            frm.FormObrir(Me)

    '        Case "m_trazabilidad"
    '            Dim frm As New frmProducto_Trazabilidad
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_formapago"
    '            Dim frm As New frmFormaDePago
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_mantenimiento_listadosadv"
    '            LlistatsADV.ObrirPantallaManteniment()

    '        Case "m_notificaciones_usuarios"
    '            Dim frm As New frmNotificacionesUsuarios
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_notificacion"
    '            Dim frm As New frmNotificaciones
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_calendario_tecnico"
    '            Dim frm As New frmCalendarioTecnico
    '            frm.Entrada()
    '            frm.FormObrir(Me)

    '        Case "m_manteniment_gauge"
    '            LlistatsADV.ObrirPantallaMantenimentGauge()

    '        Case "m_manteniment_agrupacions_gauge"
    '            LlistatsADV.ObrirPantallaMantenimentGaugeAgrupaciones()

    '        Case Else
    '            Dim _Key As String = e.Tool.Key
    '            If _Key.StartsWith("@") Then
    '                _Key = Mid(_Key, 2, _Key.Length)
    '            Else
    '                If _Key.StartsWith("#") Then
    '                    _Key = Mid(_Key, 2, _Key.Length)
    '                End If
    '            End If
    '            If IsNumeric(_Key) Then
    '                Dim oDTC As New DTCDataContext(BD.Conexion)
    '                If e.Tool.SharedProps.Tag Is Nothing = False AndAlso e.Tool.SharedProps.Tag = "ADV" Then
    '                    LlistatsADV.ObrirLlistat(oDTC, CInt(_Key))
    '                Else
    '                    If e.Tool.SharedProps.Tag Is Nothing = False AndAlso e.Tool.SharedProps.Tag = "Gauge" Then
    '                        LlistatsADV.ObrirGauge(oDTC, CInt(_Key))
    '                    Else
    '                        Informes.ObrirLlistat(oDTC, CInt(_Key))
    '                    End If
    '                End If
    '                'frm.Entrada(e.Tool.Key)
    '                'frm.MdiParent = frmPrincipal
    '                'frm.Show()
    '            End If
    '    End Select

    '    If M_UltraGrid.clsParametres.pModoProgramacio = True Then
    '        MsgBox(TimeSpan.FromTicks(Date.Now.Ticks - Temps).TotalSeconds)
    '    End If


    '    Call Seguretat.ActivarTemporitzadorControlInactivitat()
    'End Sub

#End Region

    Private Sub UltraTabbedMdiManagerPrincipal_TabActivated(sender As Object, e As Infragistics.Win.UltraWinTabbedMdi.MdiTabEventArgs) Handles UltraTabbedMdiManagerPrincipal.TabActivated
        ' Principal.UltraTabbedMdiManagerPrincipal.TabsFromKey("frmInstalacion")(0).Settings.TabAppearance.Image = My.Resources.text_aceptar

        'Dim _NomFormulari As String = e.Tab.Form.Name

        ''posar el Icono al formulari
        'Dim oDTC As New DTCDataContext(M_Global.clsVariables.pBD.Conexion)
        'Dim _Formulario As Formulario = oDTC.Formulario.Where(Function(F) F.NombreReal = _NomFormulari).FirstOrDefault
        'If IsNothing(_Formulario) Then
        '    ' e.Tab.Settings.TabAppearance.Image = Me.M_Images1.SharedImageCollection1.ImageSource.Images("layout_content.png")
        'Else
        '    Dim _Menu As Menus = oDTC.Menus.Where(Function(F) F.Formulario.NombreReal = _NomFormulari).FirstOrDefault
        '    'e.Tab.Settings.TabAppearance.Image = Me.M_Images1.SharedImageCollection1.ImageSource.Images(_Menu.NomFoto)
        'End If


    End Sub

    Private Sub ActualizacionBD()
        Try
            ' Dim Util As New M_Util.clsFunciones

            Dim _clsActualitzacioBD As New clsActualizacionBD()
            If _clsActualitzacioBD.EstablirConnexioPrincipal = False Then
                Exit Sub
            End If


            If _clsActualitzacioBD.ComprovarSiVersioBDIgualAVersioPrograma = True Then
                Exit Sub
            End If
            Util.SplashObrir("Actualizando la base de datos...")

            If _clsActualitzacioBD.ActualitzacioBD = True Then
                If BD.pBDNomBaseDades <> "AbidosMestre" And BD.pBDNomBaseDades <> "AbidosMestreVersioAnterior" Then 'si no són les bases de dades mestres llavors aplicarem la inicialització dels identities a 50000
                    Try
                        BD.EjecutarConsulta("prInicialitzarIdentitiesA5000")
                    Catch ex As Exception

                    End Try
                End If
                'Mensaje.Mostrar_Mensaje("Actualización de la base de datos realizada correctamente", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Else
                Mensaje.Mostrar_Mensaje("Error en actualitzación de la base de datos. Actualización: " & _clsActualitzacioBD.RetornaUltimaVersioBD + 1 & ", por favor contacte con el soporte técnico de ABIDOS. El programa puede volverse inestable.", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            End If
            Util.SplashTancar()


        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

End Class
