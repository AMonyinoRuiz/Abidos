Imports System.Text
Imports System.ComponentModel
Imports System.Collections
Imports DevExpress.XtraTreeList

Public Enum EnumTipusMenu
    Carpeta = 1
    Formulario = 2
    Listado = 3
    Informe = 4
    Gauge = 5
End Enum
Public Class Project
    Private IDMenu As Integer
    Private Descripcio As String
    Private Ordre As Integer
    'Private startDate_Renamed As Date
    'Private endDate_Renamed As Date
    Private IDTipusMenu As EnumTipusMenu
    Private FotoIndex As Integer
    Private owner_Renamed As Projects
    Private projects_Renamed As Projects
    Private isTask_Renamed As Boolean
    Public Sub New()
        Me.owner_Renamed = Nothing
        Me.IDMenu = 0
        Me.Ordre = 0
        Me.Descripcio = ""
        Me.FotoIndex = 0
        Me.isTask_Renamed = False
        Me.IDTipusMenu = EnumTipusMenu.Carpeta
        Me.projects_Renamed = New Projects()
    End Sub
    Public Sub New(ByVal pIDMenu As Integer, ByVal pDescripcio As String, ByVal pTipusMenu As EnumTipusMenu, ByVal isTask As Boolean, ByVal pOrdre As Integer, ByVal pFotoIndex As Integer)
        Me.IDMenu = pIDMenu
        Me.Descripcio = pDescripcio
        Me.isTask_Renamed = isTask
        Me.IDTipusMenu = pTipusMenu
        Me.Ordre = pOrdre
        Me.FotoIndex = pFotoIndex
        Me.projects_Renamed = New Projects()
    End Sub
    Public Sub New(ByVal projects As Projects, ByVal name As String, ByVal description As String, ByVal pIDTipusMenu As EnumTipusMenu, ByVal isTask As Boolean, ByVal pOrdre As Integer, ByVal pFotoIndex As Integer)
        Me.New(name, description, pIDTipusMenu, isTask, pOrdre, pFotoIndex)
        Me.projects_Renamed = projects
    End Sub
    <Browsable(False)> _
    Public Property Owner() As Projects
        Get
            Return owner_Renamed
        End Get
        Set(ByVal value As Projects)
            owner_Renamed = value
        End Set
    End Property
    Public Property IsTask() As Boolean
        Get
            Return isTask_Renamed
        End Get
        Set(ByVal value As Boolean)
            If isTask_Renamed = value Then
                Return
            End If
            isTask_Renamed = value
            OnChanged()
        End Set
    End Property
    Public Property pIDMenu() As Integer
        Get
            Return IDMenu
        End Get
        Set(ByVal value As Integer)
            If pIDMenu = value Then
                Return
            End If
            IDMenu = value
            OnChanged()
        End Set
    End Property
    <Browsable(False)> _
    Public Property pDescripcio As String
        Get
            Return Descripcio
        End Get
        Set(ByVal value As String)
            If pDescripcio = value Then
                Return
            End If
            Descripcio = value
            OnChanged()
        End Set
    End Property
    Public Property pIDTipusMenu() As EnumTipusMenu
        Get
            Return IDTipusMenu
        End Get
        Set(ByVal value As EnumTipusMenu)
            If pIDTipusMenu = value Then
                Return
            End If
            IDTipusMenu = value
            OnChanged()
        End Set
    End Property
    Public Property pOrdre() As Integer
        Get
            Return Ordre
        End Get
        Set(ByVal value As Integer)
            If pOrdre = value Then
                Return
            End If
            Ordre = value
            OnChanged()
        End Set
    End Property

    Public Property pFotoIndex() As Integer
        Get
            Return FotoIndex
        End Get
        Set(ByVal value As Integer)
            If pFotoIndex = value Then
                Return
            End If
            FotoIndex = value
            OnChanged()
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Projects() As Projects
        Get
            Return projects_Renamed
        End Get
    End Property
    Private Sub OnChanged()
        If owner_Renamed Is Nothing Then
            Return
        End If
        Dim index As Integer = owner_Renamed.IndexOf(Me)
        owner_Renamed.ResetItem(index)
    End Sub
End Class
'<treeList1>
Public Class Projects
    Inherits BindingList(Of Project)
    Implements TreeList.IVirtualTreeListData

    Private Sub IVirtualTreeListData_VirtualTreeGetChildNodes(ByVal info As VirtualTreeGetChildNodesInfo) Implements TreeList.IVirtualTreeListData.VirtualTreeGetChildNodes
        Dim obj As Project = TryCast(info.Node, Project)
        info.Children = obj.Projects
    End Sub
    Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As Project)
        item.Owner = Me
        MyBase.InsertItem(index, item)
    End Sub
    Private Sub IVirtualTreeListData_VirtualTreeGetCellValue(ByVal info As VirtualTreeGetCellValueInfo) Implements TreeList.IVirtualTreeListData.VirtualTreeGetCellValue
        Dim obj As Project = TryCast(info.Node, Project)
        Select Case info.Column.Caption
            Case "IDMenu"
                info.CellData = obj.pIDMenu
            Case "Descripcio"
                info.CellData = obj.pDescripcio
            Case "IdTipusMenu"
                info.CellData = obj.pIDTipusMenu
            Case "Ordre"
                info.CellData = obj.pOrdre
            Case "FotoIndex"
                info.CellData = obj.pFotoIndex
        End Select
    End Sub
    Private Sub IVirtualTreeListData_VirtualTreeSetCellValue(ByVal info As VirtualTreeSetCellValueInfo) Implements TreeList.IVirtualTreeListData.VirtualTreeSetCellValue
        Dim obj As Project = TryCast(info.Node, Project)
        Select Case info.Column.Caption
            Case "IDMenu"
                obj.pIDMenu = CInt(info.NewCellData)
            Case "Descripcio"
                obj.pDescripcio = CStr(info.NewCellData)
            Case "IDTipusMenu"
                obj.pIDTipusMenu = CType(info.NewCellData, EnumTipusMenu)
            Case "Ordre"
                obj.pOrdre = CInt(info.NewCellData)
            Case "FotoIndex"
                obj.pFotoIndex = CInt(info.NewCellData)
        End Select
    End Sub
End Class
'</treeList1>

