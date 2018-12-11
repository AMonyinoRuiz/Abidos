Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports System.Reflection

Public Class frmMenu
    Dim oDTC As DTCDataContext
    Dim oLinqMenu As Menu
    Dim Projects As Projects

#Region "M_ToolForm"
    Private Sub ToolForm_m_ToolForm_Salir() Handles ToolForm.m_ToolForm_Salir
        Me.FormTancar()
    End Sub



    Private Sub ToolForm_m_ToolForm_Guardar() Handles ToolForm.m_ToolForm_Guardar
        If Me.TreeMenu.Selection.Count <> 1 Then
            Mensaje.Mostrar_Mensaje("Imposible realizar la acción, seleccione primero una item", M_Mensaje.Missatge_Modo.INFORMACIO)
            Exit Sub
        End If

        Dim _NodeSeleccionat As DevExpress.XtraTreeList.Nodes.TreeListNode = Me.TreeMenu.Selection(0)

        If IsNothing(_NodeSeleccionat.ParentNode) Then 'per a que no modifiquin el root node que  és Abidos
            Mensaje.Mostrar_Mensaje("Imposible modificar este ítem", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        Dim _IDMenu As Integer = _NodeSeleccionat.GetValue(0)
        Dim _Menu As Menus = oDTC.Menus.Where(Function(F) F.ID_Menus = _IDMenu).FirstOrDefault

        _Menu.Descripcion = Me.T_Descripcion.Text
        _Menu.Menus_Tipo = oDTC.Menus_Tipo.Where(Function(F) F.ID_Menus_Tipo = CInt(Me.C_Tipo.Value)).FirstOrDefault
        _Menu.NivellSeguretat = Me.T_NivellSeguretat.Value
        _Menu.Ordre = Me.T_Orden.Value
        If Me.ImageComboBoxEdit1.SelectedIndex <> -1 Then
            _Menu.NomFoto = Me.ImageComboBoxEdit1.SelectedItem.Description
        End If

        _Menu.Formulario = Nothing
        _Menu.Listado = Nothing
        _Menu.ListadoADV = Nothing
        _Menu.GaugeAgrupacion = Nothing

        If IsNothing(Me.C_Tipo.Value) = False AndAlso IsNumeric(Me.C_Tipo.Value) Then
            Select Case DirectCast(Me.C_Tipo.Value, EnumMenuTipo)
                Case EnumMenuTipo.Carpeta
                Case EnumMenuTipo.Formulario
                    _Menu.Formulario = oDTC.Formulario.Where(Function(F) F.ID_Formulario = CInt(Me.C_Objeto.Value)).FirstOrDefault
                Case EnumMenuTipo.Listado
                    _Menu.ListadoADV = oDTC.ListadoADV.Where(Function(F) F.ID_ListadoADV = CInt(Me.C_Objeto.Value)).FirstOrDefault
                Case EnumMenuTipo.Informe
                    _Menu.Listado = oDTC.Listado.Where(Function(F) F.ID_Listado = CInt(Me.C_Objeto.Value)).FirstOrDefault
                Case EnumMenuTipo.Gauge
                    _Menu.GaugeAgrupacion = oDTC.GaugeAgrupacion.Where(Function(F) F.ID_GaugeAgrupacion = CInt(Me.C_Objeto.Value)).FirstOrDefault
                Case EnumMenuTipo.CuadroDeMando
                    _Menu.BI = oDTC.BI.Where(Function(F) F.ID_BI = CInt(Me.C_Objeto.Value)).FirstOrDefault
            End Select
        End If

        oDTC.SubmitChanges()

        Dim _Project As Project
        _Project = Me.TreeMenu.GetDataRecordByNode(_NodeSeleccionat)
        _Project.pDescripcio = Me.T_Descripcion.Text
        _Project.pIDTipusMenu = _Menu.ID_Menus_Tipo
        _Project.pOrdre = _Menu.Ordre
        Dim _IDFoto As Integer = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf(_Menu.NomFoto)
        _Project.pFotoIndex = _IDFoto

    End Sub

    Private Sub ToolForm_m_ToolForm_Eliminar() Handles ToolForm.m_ToolForm_Eliminar
        If Me.TreeMenu.Selection.Count <> 1 Then
            Mensaje.Mostrar_Mensaje("Imposible realizar la acción, seleccione primero un ítem", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        Dim _NodeSeleccionat As DevExpress.XtraTreeList.Nodes.TreeListNode = Me.TreeMenu.Selection(0)


        If IsNothing(_NodeSeleccionat.ParentNode) Then
            Mensaje.Mostrar_Mensaje("Imposible eliminar este ítem", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        If _NodeSeleccionat.HasChildren = True Then
            Mensaje.Mostrar_Mensaje("Imposible eliminar un ítem que contenga otros ítems", M_Mensaje.Missatge_Modo.INFORMACIO, , , True)
            Exit Sub
        End If

        Dim _IDMenu As Integer = _NodeSeleccionat.GetValue(0)
        Dim _Menu As Menus = oDTC.Menus.Where(Function(F) F.ID_Menus = _IDMenu).FirstOrDefault

        oDTC.Menus.DeleteOnSubmit(_Menu)
        oDTC.SubmitChanges()

        Dim _ProjectABorrar As Project
        _ProjectABorrar = Me.TreeMenu.GetDataRecordByNode(_NodeSeleccionat)

        Dim _ProjectPare As Projects
        _ProjectPare = _ProjectABorrar.Owner
        _ProjectPare.Remove(_ProjectABorrar)
    End Sub

    Private Sub ToolForm_m_ToolForm_Nuevo() Handles ToolForm.m_ToolForm_Nuevo
        If Me.TreeMenu.Selection.Count <> 1 Then
            Mensaje.Mostrar_Mensaje("Imposible realizar la acción, seleccione primero una carpeta", M_Mensaje.Missatge_Modo.INFORMACIO)
            Exit Sub
        End If

        Dim _NodeSeleccionat As DevExpress.XtraTreeList.Nodes.TreeListNode = Me.TreeMenu.Selection(0)

        Dim _IDMenu As Integer = _NodeSeleccionat.GetValue(0)
        Dim _Menu As Menus = oDTC.Menus.Where(Function(F) F.ID_Menus = _IDMenu).FirstOrDefault

        If _Menu.ID_Menus_Tipo <> EnumMenuTipo.Carpeta Then
            Mensaje.Mostrar_Mensaje("Imposible realizar la acción, el menú seleccionado tiene que ser una carpeta", M_Mensaje.Missatge_Modo.INFORMACIO)
            Exit Sub
        End If

        Dim _NewMenu As New Menus
        _NewMenu.Descripcion = "Nueva Carpeta"
        _NewMenu.Menus = oDTC.Menus.Where(Function(F) F.ID_Menus = _Menu.ID_Menus).FirstOrDefault
        _NewMenu.Menus_Tipo = oDTC.Menus_Tipo.Where(Function(F) F.ID_Menus_Tipo = _Menu.ID_Menus_Tipo).FirstOrDefault
        _NewMenu.NivellSeguretat = 10
        _NewMenu.Ordre = 0
        _NewMenu.NomFoto = "" 'rururu
        oDTC.Menus.InsertOnSubmit(_NewMenu)
        oDTC.SubmitChanges()

        Dim _Project As Project
        _Project = Me.TreeMenu.GetDataRecordByNode(_NodeSeleccionat)
        _Project.Projects.Add(New Project(_NewMenu.ID_Menus, _NewMenu.Descripcion, _NewMenu.ID_Menus_Tipo, True, _Menu.Ordre, 0)) 'rururu


        '_NodeSeleccionat.Nodes.Add(_NouNode)


        'Dim _NewProject As New Projects
        '_NewProject(0).Projects.Add(New Project(0, "Carpeta nueva", EnumMenuTipo.Carpeta, True))

        ' TreeMenu.AppendNode(New Object() {5, "my sub child three", "421"}, _NodeSeleccionat)

        'TreeMenu.RefreshNode(_NodeSeleccionat.ParentNode)
    End Sub
#End Region

#Region "Metodes"

    Public Sub Entrada(Optional ByVal pId As Integer = 0)
        Try

            Me.AplicarDisseny()


            Me.ToolForm.M.Botons.tBuscar.SharedProps.Visible = False
            Me.ToolForm.M.Botons.tNou.SharedProps.Caption = "Añadir menú"
            Me.ToolForm.M.Botons.tGuardar.SharedProps.Caption = "Modificar menú"
            Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Eliminar menú"

            '        Me.TE_Codigo.ButtonsRight("Lupeta").Enabled = True
            '        Util.Cargar_Combo(Me.C_Tipo, "Select ID_FormaPago_Tipo, Descripcion From FormaPago_Tipo", True, False)
            Call Netejar_Pantalla()
            '        If pId <> 0 Then
            '            Call Cargar_Form(pId)
            '        End If
            '        Me.KeyPreview = False

            Util.Cargar_Combo(Me.C_Tipo, "Select ID_Menus_Tipo, Descripcion From Menus_Tipo Order by ID_Menus_Tipo", True)

            'Call CargarTree()
            Dim _clsMenu As New clsMenuPrincipal(oDTC, Me.TreeMenu)
            _clsMenu.CargarTree()
            Projects = _clsMenu.pProjects

            Dim i As Integer
            For i = 0 To Me.M_Images1.SharedImageCollection1.ImageSource.Images.Count - 1
                Me.ImageComboBoxEdit1.Properties.Items.Add(New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.Item(i), i, i)) '"Identificador " & (i + 1).ToString(), i, i)
            Next
            Me.ImageComboBoxEdit1.Properties.SmallImages = Me.M_Images1.SharedImageCollection1
            Me.ImageComboBoxEdit1.Properties.Sorted = True



        Catch ex As Exception
            Mensaje.Mostrar_Mensaje_Error(ex)
        End Try

    End Sub

    'Private Sub CargarTree()

    '    Dim _Llistat As IQueryable(Of Menus) = oDTC.Menus.OrderBy(Function(F) F.Ordre)
    '    Dim _Menu As Menus
    '    Dim _Index As Integer = 0
    '    For Each _Menu In _Llistat.Where(Function(F) F.ID_Menus_Padre.HasValue = False).OrderBy(Function(F) F.Ordre)
    '        Projects.Add(New Project(_Menu.ID_Menus, _Menu.Descripcion, _Menu.ID_Menus_Tipo, True, _Menu.Ordre))
    '        Call CrearFills(0, _Index, _Menu)
    '        _Index = _Index + 1
    '    Next

    '    Me.TreeMenu.DataSource = Projects

    'End Sub

    'Private Sub CrearFills(ByVal pNivell As Integer, ByVal pIndex As Integer, ByRef pMenuPare As Menus)

    '    Dim _Menu As Menus
    '    Dim _MenuPare As Menus = pMenuPare
    '    Dim _Index As Integer = 0

    '    For Each _Menu In oDTC.Menus.Where(Function(F) F.ID_Menus_Padre = _MenuPare.ID_Menus).OrderBy(Function(F) F.Ordre)
    '        Select Case pNivell
    '            Case 0
    '                Projects(pIndex).Projects.Add(New Project(_Menu.ID_Menus, _Menu.Descripcion, _Menu.ID_Menus_Tipo, True, _Menu.Ordre))
    '            Case 1
    '                Projects(Projects.Count - 1).Projects(pIndex).Projects.Add(New Project(_Menu.ID_Menus, _Menu.Descripcion, _Menu.ID_Menus_Tipo, True, _Menu.Ordre))
    '            Case 2
    '                Projects(Projects.Count - 1).Projects(Projects(Projects.Count - 1).Projects.Count - 1).Projects(pIndex).Projects.Add(New Project(_Menu.ID_Menus, _Menu.Descripcion, _Menu.ID_Menus_Tipo, True, _Menu.Ordre))
    '            Case 3
    '                Projects(Projects.Count - 1).Projects(Projects(Projects.Count - 1).Projects.Count - 1).Projects(Projects(Projects(Projects.Count - 1).Projects.Count - 1).Projects.Count - 1).Projects(pIndex).Projects.Add(New Project(_Menu.ID_Menus, _Menu.Descripcion, _Menu.ID_Menus_Tipo, True, _Menu.Ordre))
    '        End Select
    '        Call CrearFills(pNivell + 1, _Index, _Menu)
    '        _Index = _Index + 1
    '    Next
    'End Sub

    'Private Function Guardar() As Boolean
    '    Guardar = False
    '    Try
    '        Me.TE_Codigo.Focus()

    '        If oLinqFormaPago.ID_FormaPago = 0 Then
    '            If BD.RetornaValorSQL("SELECT Count (*) From FormaPago WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "'") > 0 Then
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
    '                Exit Function
    '            End If
    '        Else
    '            If BD.RetornaValorSQL("SELECT Count (*) From FormaPago WHERE Codigo='" & Util.APOSTROF(Me.TE_Codigo.Text) & "' and ID_FormaPago<>" & oLinqFormaPago.ID_FormaPago) > 0 Then
    '                Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_CODI_EXISTENT, "")
    '                Exit Function
    '            End If
    '        End If

    '        If ComprovacioCampsRequeritsError() = True Then
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_TEXTE_REQUERIT, "")
    '            Exit Function
    '        End If

    '        Call GetFromForm(oLinqFormaPago)

    '        If oLinqFormaPago.ID_FormaPago = 0 Then  ' Comprovacio per saber si es un insertar o un nou
    '            oDTC.FormaPago.InsertOnSubmit(oLinqFormaPago)
    '            oDTC.SubmitChanges()
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_AFEGIR_REGISTRE)
    '        Else
    '            oDTC.SubmitChanges()
    '            Mensaje.Mostrar_Mensaje(M_Mensaje.Missatge_Tipus.MISSATGE_MODIFICAR_REGISTRE)
    '        End If


    '        Dim _FormaPago As FormaPago
    '        If oLinqFormaPago.Predeterminada = True Then  'Si aquest és el magatzem predetrerminat mirarem si ni ha algun altre que tb ho sigui, i si hi és el despredeterminarem
    '            _FormaPago = oDTC.FormaPago.Where(Function(F) F.ID_FormaPago <> oLinqFormaPago.ID_FormaPago And F.Predeterminada = True).FirstOrDefault
    '            If _FormaPago Is Nothing = False Then
    '                _FormaPago.Predeterminada = False
    '            End If
    '            oDTC.SubmitChanges()
    '        End If

    '        'Aqui comprovem si hi ha algun magatzem predeterminat, si no ni ha cap li assigarem a aquest el predeterminat ja que en te que haver un
    '        _FormaPago = oDTC.FormaPago.Where(Function(F) F.Predeterminada = True).FirstOrDefault
    '        If _FormaPago Is Nothing = True Then
    '            oLinqFormaPago.Predeterminada = True
    '        End If
    '        oDTC.SubmitChanges()



    '        'Call ComprovarSiAlgoHaCanviat(oLinqAssociat.ID_Associat)
    '        Guardar = True

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Function

    'Private Sub SetToForm()
    '    Try
    '        With oLinqFormaPago
    '            Me.TE_Codigo.Value = .Codigo
    '            Me.T_Descripcion.Text = .Descripcion
    '            Me.R_Condiciones.pText = .Condiciones
    '            Me.C_Tipo.Value = .ID_FormaPago_Tipo
    '            Me.CH_Predeterminada.Checked = .Predeterminada
    '        End With
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub GetFromForm(ByRef pEntidad As FormaPago)
    '    Try
    '        With pEntidad
    '            .Activo = True
    '            .Codigo = Me.TE_Codigo.Value
    '            .Descripcion = Me.T_Descripcion.Text
    '            .Condiciones = Me.R_Condiciones.pText
    '            .FormaPago_Tipo = oDTC.FormaPago_Tipo.Where(Function(F) F.ID_FormaPago_Tipo = CInt(Me.C_Tipo.Value)).FirstOrDefault
    '            .Predeterminada = Me.CH_Predeterminada.Checked
    '        End With

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

    'Private Sub Cargar_Form(ByVal pID As Integer)
    '    Try
    '        Call Netejar_Pantalla()
    '        oLinqFormaPago = (From taula In oDTC.FormaPago Where taula.ID_FormaPago = pID Select taula).FirstOrDefault
    '        Call SetToForm()

    '        Call CargaGrid_Giros(pID)
    '        Call CargaGrid_Uso_Clientes(pID)
    '        Call CargaGrid_Uso_Proveedores(pID)
    '        'Util.Tab_Activar(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS)

    '        Me.EstableixCaptionForm("Forma de pago: " & (oLinqFormaPago.Descripcion))

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub
    Private Sub Netejar_Pantalla()
        'oLinqFormaPago = New FormaPago
        oDTC = New DTCDataContext(BD.Conexion)
        Util.Blanquejar(Me, M_Util.Enum_Controles_Activacion.TODOS, True)

        ''Me.ToolForm.M.Botons.tEliminar.SharedProps.Caption = "Dar de baja"
        'Me.TE_Codigo.Value = Nothing
        'Me.TE_Codigo.Focus()

        'Call CargaGrid_Uso_Clientes(0)
        'Call CargaGrid_Uso_Proveedores(0)
        'Call CargaGrid_Giros(0)

        'Me.C_Tipo.Value = Nothing
        'Me.TAB_General.Tabs("Condiciones").Selected = True
        ''Me.C_Tipo.Value = CInt(EnumCampañaEstado.Abierta)
        '' Me.C_OrigenCliente.Value = 1
        '' Util.Tab_Desactivar_x_Key(Me.TAB_Principal, M_Util.Enum_Tab_Activacion.TODOS_MENOS_ALGUNOS, "Instalacion")


        ErrorProvider1.Clear()

    End Sub

    'Private Function ComprovacioCampsRequeritsError() As Boolean
    '    Try
    '        Dim oClsControls As New clsControles(ErrorProvider1)

    '        With Me
    '            ErrorProvider1.Clear()
    '            oClsControls.ControlBuit(.TE_Codigo)
    '            oClsControls.ControlBuit(.T_Descripcion)
    '            oClsControls.ControlBuit(.C_Tipo)
    '        End With

    '        ComprovacioCampsRequeritsError = oClsControls.pCampRequeritTrobat

    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Function

    'Private Sub Cridar_Llistat_Generic()
    '    Dim LlistatGeneric As New M_LlistatGeneric.clsLlistatGeneric
    '    LlistatGeneric.Mostrar_Llistat("SELECT * FROM FormaPago Where Activo=1 ORDER BY Codigo", Me.TE_Codigo, "ID_FormaPago", "ID_FormaPago")
    '    AddHandler LlistatGeneric.AlTancarLlistatGeneric, AddressOf AlTancarLlistat
    'End Sub

    'Private Sub AlTancarLlistat(ByVal pID As String)
    '    Try
    '        Call Cargar_Form(pID)
    '    Catch ex As Exception
    '        Mensaje.Mostrar_Mensaje_Error(ex)
    '    End Try
    'End Sub

#End Region

#Region "Events Varis"

    Private Sub TreeMenu_AfterDragNode(sender As Object, e As DevExpress.XtraTreeList.NodeEventArgs) Handles TreeMenu.AfterDragNode
        Dim _Project As Project
        _Project = Me.TreeMenu.GetDataRecordByNode(e.Node)
        Dim _PareProject As Project
        _PareProject = Me.TreeMenu.GetDataRecordByNode(e.Node.ParentNode)

        Dim _Menu As Menus
        _Menu = oDTC.Menus.Where(Function(F) F.ID_Menus = _Project.pIDMenu).FirstOrDefault
        _Menu.Menus = oDTC.Menus.Where(Function(F) F.ID_Menus = _PareProject.pIDMenu).FirstOrDefault
        oDTC.SubmitChanges()

    End Sub

    Private Sub TreeMenu_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles TreeMenu.FocusedNodeChanged
        'Dim _Project As Project
        '_Project = Me.TreeMenu.GetDataRecordByNode(e.Node)

        Dim _IDMenu As Integer = e.Node.GetValue(0)
        If _IDMenu = 0 Then
            Me.TreeMenu.MoveNext()
            Exit Sub
        End If
        Dim _Menu As Menus = oDTC.Menus.Where(Function(F) F.ID_Menus = _IDMenu).FirstOrDefault
        Me.C_Tipo.Value = _Menu.ID_Menus_Tipo
        If _Menu.ID_Menus_Tipo <> EnumMenuTipo.Carpeta Then

        End If

        If IsNothing(Me.C_Tipo.Value) = False AndAlso IsNumeric(Me.C_Tipo.Value) Then
            Select Case DirectCast(Me.C_Tipo.Value, EnumMenuTipo)
                Case EnumMenuTipo.Carpeta
                    Me.C_Objeto.Items.Clear()
                Case EnumMenuTipo.Formulario
                    Me.C_Objeto.Value = _Menu.ID_Formulario
                Case EnumMenuTipo.Listado
                    Me.C_Objeto.Value = _Menu.ID_ListadoADV
                Case EnumMenuTipo.Informe
                    Me.C_Objeto.Value = _Menu.ID_Listado
                Case EnumMenuTipo.Gauge
                    Me.C_Objeto.Value = _Menu.ID_GaugeAgrupacion
                Case EnumMenuTipo.CuadroDeMando
                    Me.C_Objeto.Value = _Menu.ID_BI
            End Select
        End If
        Me.T_Descripcion.Text = _Menu.Descripcion
        Me.T_Observaciones.Text = _Menu.Observaciones
        Me.T_NivellSeguretat.Value = _Menu.NivellSeguretat
        Me.T_Orden.Value = _Menu.Ordre
        Me.TE_Codigo.Value = _Menu.ID_Menus
        Dim _IDFoto As Integer = Me.M_Images1.SharedImageCollection1.ImageSource.Images.Keys.IndexOf(_Menu.NomFoto)

        Me.ImageComboBoxEdit1.EditValue = _IDFoto 'rururu


    End Sub

    Private Sub C_Tipo_ValueChanged(sender As Object, e As EventArgs) Handles C_Tipo.ValueChanged
        If IsNothing(Me.C_Tipo.Value) = False AndAlso IsNumeric(Me.C_Tipo.Value) Then
            Select Case DirectCast(Me.C_Tipo.Value, EnumMenuTipo)
                Case EnumMenuTipo.Carpeta
                    Me.C_Objeto.Items.Clear()
                Case EnumMenuTipo.Formulario
                    Util.Cargar_Combo(Me.C_Objeto, "Select ID_Formulario, Descripcion From Formulario Where AperturaExterna=1 Order by Descripcion")
                Case EnumMenuTipo.Listado
                    Util.Cargar_Combo(Me.C_Objeto, "Select ID_ListadoADV, Descripcion From ListadoADV Order by Descripcion")
                Case EnumMenuTipo.Informe
                    Util.Cargar_Combo(Me.C_Objeto, "Select ID_Listado, Descripcion From Listado Where Activo=1 Order by Descripcion")
                Case EnumMenuTipo.Gauge
                    Util.Cargar_Combo(Me.C_Objeto, "Select ID_GaugeAgrupacion, Descripcion From GaugeAgrupacion Order by Descripcion")
                Case EnumMenuTipo.CuadroDeMando
                    Util.Cargar_Combo(Me.C_Objeto, "Select ID_BI, Descripcion From BI Where Activo=1 Order by Descripcion")
            End Select
        End If
    End Sub


#End Region




End Class
