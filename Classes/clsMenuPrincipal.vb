Imports DevExpress.XtraTreeList

Public Class clsMenuPrincipal
    Dim oProjects As New Projects()
    Dim oDTC As DTCDataContext
    Dim WithEvents oTree As TreeList
    Dim oSharedImages As New M_Global.M_Images
    Dim oLlistatMenus As IQueryable(Of Menus)

    Public ReadOnly Property pProjects As Projects
        Get
            Return oProjects
        End Get
    End Property

    Public Sub New(ByRef pDTC As DTCDataContext, ByRef pTree As DevExpress.XtraTreeList.TreeList)
        oDTC = pDTC
        oTree = pTree
    End Sub

    Public Sub CargarTree()
        Util.WaitFormObrir()

        oDTC = New DTCDataContext(BD.Conexion)

        oTree.StateImageList = oSharedImages.SharedImageCollection1
        oTree.DataSource = Nothing
        oProjects.Clear()


        oLlistatMenus = oDTC.Menus.OrderBy(Function(F) F.Ordre)
        Dim _Menu As Menus
        For Each _Menu In oLlistatMenus.Where(Function(F) F.ID_Menus_Padre.HasValue = False).OrderBy(Function(F) F.Ordre)
            oProjects.Add(New Project(_Menu.ID_Menus, _Menu.Descripcion, _Menu.ID_Menus_Tipo, True, _Menu.Ordre, 0))
            Call CrearFills(oProjects(0), _Menu)
        Next


        oTree.DataSource = oProjects

        'oTree.StateImageList = oSharedImages

        oTree.ForceInitialize()
        oTree.CollapseAll()
        oTree.FocusedNode = oTree.Nodes.FirstNode
        oTree.Nodes(0).Expanded = True
        oTree.BestFitColumns()
        Util.WaitFormTancar()
    End Sub

    Private Sub CrearFills(ByRef pProjectePare As Project, ByRef pMenuPare As Menus)
        Dim _Menu As Menus
        Dim _MenuPare As Menus = pMenuPare

        For Each _Menu In oLlistatMenus.Where(Function(F) F.ID_Menus_Padre = _MenuPare.ID_Menus)
            Dim _ProjecteNou As Project = CrearEstructuraProjects(pProjectePare, _Menu)
            Call CrearFills(_ProjecteNou, _Menu)
            If _ProjecteNou Is Nothing = False AndAlso _ProjecteNou.Projects.Count = 0 AndAlso _ProjecteNou.pIDTipusMenu = EnumTipusMenu.Carpeta Then
                pProjectePare.Projects.Remove(_ProjecteNou)
            End If
        Next
    End Sub

    Private Function CrearEstructuraProjects(ByRef pProjectPare As Project, pMenuACrear As Menus) As Project
        If PermisPerCrearMenu(pMenuACrear) = True Then
            Dim _IDFoto As Integer = oSharedImages.SharedImageCollection1.ImageSource.Images.Keys.IndexOf(pMenuACrear.NomFoto)
            pProjectPare.Projects.Add(New Project(pMenuACrear.ID_Menus, pMenuACrear.Descripcion, pMenuACrear.ID_Menus_Tipo, True, pMenuACrear.Ordre, _IDFoto))
            Return pProjectPare.Projects(pProjectPare.Projects.Count - 1)
        End If
    End Function

    Public Function PermisPerCrearMenu(pMenu As Menus) As Boolean
        PermisPerCrearMenu = False
        Select Case DirectCast(pMenu.ID_Menus_Tipo, EnumMenuTipo)
            Case EnumMenuTipo.Carpeta
                Return True
            Case EnumMenuTipo.Informe
                If Seguretat.oUser.NivelSeguridad <= pMenu.Listado.NivelSeguridad Then
                    Return True
                End If
            Case EnumMenuTipo.Listado
                If Seguretat.oUser.NivelSeguridad <= pMenu.ListadoADV.NivelSeguridad Then
                    Return True
                End If
            Case EnumMenuTipo.Gauge
                If Seguretat.oUser.NivelSeguridad <= pMenu.GaugeAgrupacion.NivelSeguridad Then
                    Return True
                End If
            Case EnumMenuTipo.CuadroDeMando
                Dim _Menu As Menus = pMenu
                Dim _Usuari As Usuario = oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault
                If _Usuari.BI_Usuario.Where(Function(F) F.BI.Activo = True And F.ID_BI = _Menu.ID_BI).Count = 1 Then
                    '    If Seguretat.oUser.Usuario_Grupo.Formulario_Usuario_Grupo.Where(Function(F) F.ID_Formulario = _Menu.ID_Formulario).FirstOrDefault.Visualizar = True Then
                    Return True
                    '    End If
                End If
            Case EnumMenuTipo.Formulario
                Dim _Menu As Menus = pMenu

                If oDTC.Usuario.Where(Function(F) F.ID_Usuario = Seguretat.oUser.ID_Usuario).FirstOrDefault.Usuario_Grupo.Formulario_Usuario_Grupo.Where(Function(F) F.ID_Formulario = _Menu.ID_Formulario).Count = 1 Then
                    'If Seguretat.oUser.Usuario_Grupo.Formulario_Usuario_Grupo.Where(Function(F) F.ID_Formulario = _Menu.ID_Formulario).Count = 1 Then
                    If Seguretat.oUser.Usuario_Grupo.Formulario_Usuario_Grupo.Where(Function(F) F.ID_Formulario = _Menu.ID_Formulario).FirstOrDefault.Visualizar = True Then
                        Return True
                    End If
                End If
                'si no es cumpleix els dos if's retornara un false amb missatge de que el formualari no se li poden assignar permisos
                'Mensaje.Mostrar_Mensaje("Error al aplicar la política de permisos en el formulario:" & _Menu.Formulario.Descripcion, M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
                Return False
        End Select

    End Function

    Private Sub treeList1_GetStateImage(ByVal sender As Object, ByVal e As DevExpress.XtraTreeList.GetStateImageEventArgs) Handles oTree.GetStateImage
        Dim project As Project = CType(oTree.GetDataRecordByNode(e.Node), Project)
        If project Is Nothing Then
            Return
        End If

        If project.pFotoIndex <> 0 Then
            e.NodeImageIndex = project.pFotoIndex
            e.Node.StateImageIndex = project.pFotoIndex
        End If
    End Sub

    Private Sub TreeMenu_DoubleClick(sender As Object, e As EventArgs) Handles oTree.DoubleClick
        Dim viewInfo As DevExpress.XtraTreeList.TreeListHitInfo = Me.oTree.CalcHitInfo((CType(e, MouseEventArgs)).Location)
        If viewInfo.Node IsNot Nothing Then
            Dim _Menu As Menus = oDTC.Menus.Where(Function(F) F.ID_Menus = CInt(viewInfo.Node.GetValue(0))).FirstOrDefault
            Call ObrirMenu(_Menu)
        End If

        viewInfo = Nothing
    End Sub

    Private Sub ObrirMenu(ByVal pMenu As Menus)
        Try
            'MsgBox(My.Application.Info.WorkingSet)
            Dim oDTC As New DTCDataContext(BD.Conexion)

            Select Case DirectCast(pMenu.ID_Menus_Tipo, EnumMenuTipo)
                Case EnumMenuTipo.Carpeta

                Case EnumMenuTipo.Formulario
                    Call ObrirFormulari(pMenu.Formulario.NombreReal, Util.Comprobar_NULL_Per_0(pMenu.Formulario.ParametroEntrada))

                Case EnumMenuTipo.Listado
                    Util.WaitFormObrir()
                    LlistatsADV.ObrirLlistat(oDTC, pMenu.ID_ListadoADV)
                    Util.WaitFormTancar()
                Case EnumMenuTipo.Informe
                    Util.WaitFormObrir()
                    Informes.ObrirLlistat(oDTC, pMenu.ID_Listado)
                    Util.WaitFormTancar()
                Case EnumMenuTipo.Gauge
                    Util.WaitFormObrir()
                    LlistatsADV.ObrirGauge(oDTC, pMenu.ID_GaugeAgrupacion)
                    Util.WaitFormTancar()
                Case EnumMenuTipo.CuadroDeMando
                    Util.WaitFormObrir()
                    LlistatsADV.ObrirBI(oDTC, pMenu.ID_BI)
                    Util.WaitFormTancar()
            End Select

        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try
    End Sub

    Public Sub ObrirFormulari(ByVal pNomFormulariReal As String, Optional ByVal pParametroEntrada As Integer = 0)
        Dim oDTC As New DTCDataContext(BD.Conexion)
        Dim _BDForm As Formulario
        If pParametroEntrada = 0 Then
            _BDForm = oDTC.Formulario.Where(Function(F) F.NombreReal = pNomFormulariReal And F.ParametroEntrada.HasValue = False).FirstOrDefault
        Else
            _BDForm = oDTC.Formulario.Where(Function(F) F.NombreReal = pNomFormulariReal And F.ParametroEntrada = pParametroEntrada).FirstOrDefault
        End If

        Select Case pNomFormulariReal
            Case "frmProducto"
                Dim frm As frmProducto = oclsPilaFormularis.ObrirFormulari(GetType(frmProducto))
                frm.Entrada()
                frm.FormObrir(frmPrincipal)

            Case "frmInstalacion"
                'If pParametroEntrada = 2 Then 'si és una futura instalación no la obrirem en caché
                '    Dim frm As New frmInstalacion
                '    frm.Entrada(0, pParametroEntrada)
                '    frm.FormObrir(frmPrincipal)
                'Else
                'Dim pepe As Long = Now.Ticks



                Dim frm As frmInstalacion = oclsPilaFormularis.ObrirFormulari(GetType(frmInstalacion), pParametroEntrada)
                frm.Entrada(0, pParametroEntrada)
                frm.FormObrir(frmPrincipal)

                ' MsgBox(TimeSpan.FromTicks(Date.Now.Ticks - pepe).TotalSeconds)
                '  End If

            Case "frmParte"
                Dim frm As frmParte = oclsPilaFormularis.ObrirFormulari(GetType(frmParte))
                frm.Entrada()
                frm.FormObrir(frmPrincipal)

            Case "frmManteniment_Informe"
                'Dim oDTC As New DTCDataContext(BD.Conexion)
                Informes.ObrirInformeManteniment(oDTC)

            Case "frmManteniment_Informe_Plantilla"
                Informes.ObrirInformePlantillaManteniment()

            Case "frmManteniment_Listado"
                'Dim oDTC As New DTCDataContext(BD.Conexion)
                Informes.ObrirLlistatManteniment(oDTC)

            Case "frmManteniment_Usuario"
                Seguretat.ObrirFormulariUsuaris()

            Case "frmManteniment_Usuario_Grupo"
                Seguretat.ObrirFormulariGrupUsuaris()

            Case "frmManteniment_Formulario"
                Seguretat.ObrirFormulariMantenimentFormularis()

            Case "CambioDeContraseña"
                Seguretat.ObrirFormulariCanviContrasenya()

            Case "frmLogin"
                If Seguretat.ObrirFormulariLogin(True) = False Then
                    End
                    Exit Sub
                End If
                Call CargarTree()

            Case "frmCreadorListados"
                LlistatsADV.ObrirPantallaManteniment()


            Case "frmMantenimentGauge"
                LlistatsADV.ObrirPantallaMantenimentGauge()

            Case "frmMantenimentGaugeAgrupacion"
                LlistatsADV.ObrirPantallaMantenimentGaugeAgrupaciones()

            Case "frmBIManteniment"
                LlistatsADV.ObrirPantallaMantenimentBI()

            Case "frmCrearEmpresa"
                M_Instalacion.clsInstalacion.ObrirFormulariCrearBD(frmPrincipal, BD)

            Case Else

                Dim _Tipus As Type
                _Tipus = Type.GetType("Abidos." & pNomFormulariReal)
                Dim _frm As Object
                _frm = Activator.CreateInstance(_Tipus)
                If _BDForm.ParametroEntrada.HasValue = True Then
                    _frm.Entrada(0, _BDForm.ParametroEntrada)
                Else
                    _frm.Entrada()
                End If

                _frm.FormObrir(frmPrincipal, False)

        End Select



    End Sub
End Class
