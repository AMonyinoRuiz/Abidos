Imports System
Imports DevExpress.XtraBars
Imports DevExpress.LookAndFeel
Imports System.ComponentModel
Imports DevExpress.Skins


Namespace ReportDesigner
    <ToolboxItem(False), DesignTimeVisible(False)> _
    Public Class BarLookAndFeelItem
        Inherits BarButtonItem

        Public Sub New(ByVal manager As BarManager, ByVal privateItem As Boolean)
            fIsPrivateItem = privateItem
            manager = manager
            ButtonStyle = BarButtonStyle.Check
        End Sub 'New

        Public Overridable Sub UpdateState(ByVal lookAndFeel As UserLookAndFeel)
        End Sub 'UpdateState

        Public Overridable Sub ApplyChanges(ByVal lookAndFeel As UserLookAndFeel)
        End Sub 'ApplyChanges
    End Class 'BarLookAndFeelItem
    _
    Public Class BarLookAndFeelStyleItem
        Inherits BarLookAndFeelItem
        Private activeStyle As ActiveLookAndFeelStyle
        Private style As LookAndFeelStyle

        Public Sub New(ByVal manager As BarManager, ByVal privateItem As Boolean, ByVal activeStyle As ActiveLookAndFeelStyle, ByVal style As LookAndFeelStyle)
            MyBase.New(manager, privateItem)
            Me.style = style
            Me.activeStyle = activeStyle
        End Sub 'New

        Public Overrides Sub UpdateState(ByVal lookAndFeel As UserLookAndFeel)
            Down = lookAndFeel.ActiveStyle = activeStyle
        End Sub 'UpdateState

        Public Overrides Sub ApplyChanges(ByVal lookAndFeel As UserLookAndFeel)
            lookAndFeel.UseWindowsXPTheme = False
            lookAndFeel.Style = style
        End Sub 'ApplyChanges
    End Class 'BarLookAndFeelStyleItem
    _
    Public Class BarLookAndFeelSkinNameItem
        Inherits BarLookAndFeelItem
        Private skinName As String

        Public Sub New(ByVal manager As BarManager, ByVal privateItem As Boolean, ByVal skinName As String)
            MyBase.New(manager, privateItem)
            Me.skinName = skinName
        End Sub 'New

        Public Overrides Sub UpdateState(ByVal lookAndFeel As UserLookAndFeel)
            Down = lookAndFeel.ActiveStyle = ActiveLookAndFeelStyle.Skin And lookAndFeel.SkinName = skinName
        End Sub 'UpdateState

        Public Overrides Sub ApplyChanges(ByVal lookAndFeel As UserLookAndFeel)
            lookAndFeel.UseWindowsXPTheme = False
            lookAndFeel.Style = LookAndFeelStyle.Skin
            lookAndFeel.SkinName = skinName
        End Sub 'ApplyChanges
    End Class 'BarLookAndFeelSkinNameItem
    _
    Public Class BarLookAndFeelUseWindowsXPThemeItem
        Inherits BarLookAndFeelItem

        Public Sub New(ByVal manager As BarManager, ByVal privateItem As Boolean)
            MyBase.New(manager, privateItem)
        End Sub 'New

        Public Overrides Sub UpdateState(ByVal lookAndFeel As UserLookAndFeel)
            Down = lookAndFeel.ActiveStyle = ActiveLookAndFeelStyle.WindowsXP
            Enabled = DevExpress.Utils.WXPaint.Painter.ThemesEnabled
        End Sub 'UpdateState

        Public Overrides Sub ApplyChanges(ByVal lookAndFeel As UserLookAndFeel)
            lookAndFeel.UseWindowsXPTheme = True
        End Sub 'ApplyChanges
    End Class 'BarLookAndFeelUseWindowsXPThemeItem
    _
    Public Class BarLookAndFeelListItem
        Inherits BarLinkContainerItem
        Private lookAndFeel As UserLookAndFeel
        Private skinSubMenuItem As BarSubItem

        Public Sub New(ByVal lookAndFeel As UserLookAndFeel)
            Me.lookAndFeel = lookAndFeel

            skinSubMenuItem = New BarSubItem()
            skinSubMenuItem.Caption = "Skin"
        End Sub 'New
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)> _
        Public Overrides Property LinksPersistInfo() As LinksInfo
            Get
                Return Nothing
            End Get
            Set(ByVal Value As LinksInfo)
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)> _
        Public Overrides ReadOnly Property ItemLinks() As BarItemLinkCollection
            Get
                Return MyBase.ItemLinks
            End Get
        End Property

        Protected Overrides Sub OnManagerChanged()
            MyBase.OnManagerChanged()
            If Manager Is Nothing Then
                Return
            End If
            BeginUpdate()
            ClearLinks()
            Try
                skinSubMenuItem.ClearLinks()
                AddBarLookAndFeelItem(Me, New BarLookAndFeelUseWindowsXPThemeItem(Manager, True), "Use WindowsXP Theme")

                AddBarLookAndFeelItem(Me, New BarLookAndFeelStyleItem(Manager, True, ActiveLookAndFeelStyle.Flat, LookAndFeelStyle.Flat), "Flat Style")
                AddBarLookAndFeelItem(Me, New BarLookAndFeelStyleItem(Manager, True, ActiveLookAndFeelStyle.Office2003, LookAndFeelStyle.Office2003), "Office2003 Style")
                AddBarLookAndFeelItem(Me, New BarLookAndFeelStyleItem(Manager, True, ActiveLookAndFeelStyle.Style3D, LookAndFeelStyle.Style3D), "Style3D")
                AddBarLookAndFeelItem(Me, New BarLookAndFeelStyleItem(Manager, True, ActiveLookAndFeelStyle.UltraFlat, LookAndFeelStyle.UltraFlat), "UltraFlat Style")
                AddItem(skinSubMenuItem)

                Dim container As SkinContainer
                For Each container In SkinManager.Default.Skins
                    AddBarLookAndFeelItem(skinSubMenuItem, New BarLookAndFeelSkinNameItem(Manager, True, container.SkinName), container.SkinName)
                Next container
            Finally
                CancelUpdate()
            End Try
        End Sub 'OnManagerChanged

        Protected Overrides Sub OnGetItemData()
            UpdateLookAndFeelItemsState(Me)
            MyBase.OnGetItemData()
        End Sub 'OnGetItemData

        Sub UpdateLookAndFeelItemsState(ByVal holder As BarLinksHolder)
            Dim link As BarItemLink
            For Each link In holder.ItemLinks
                If TypeOf link.Item Is BarLookAndFeelItem Then
                    CType(link.Item, BarLookAndFeelItem).UpdateState(lookAndFeel)
                End If
                If TypeOf link.Item Is BarLinksHolder Then
                    UpdateLookAndFeelItemsState(link.Item) '
                End If
            Next link
        End Sub 'UpdateLookAndFeelItemsState

        Sub AddBarLookAndFeelItem(ByVal holder As BarLinksHolder, ByVal item As BarLookAndFeelItem, ByVal caption As String)
            item.Caption = caption
            AddHandler item.ItemClick, AddressOf OnItemClick
            item.UpdateState(lookAndFeel)
            holder.AddItem(item)
        End Sub 'AddBarLookAndFeelItem

        Sub OnItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            Dim item As BarLookAndFeelItem = e.Item '
            If Not (item Is Nothing) Then
                item.ApplyChanges(lookAndFeel)
            End If
        End Sub 'OnItemClick
    End Class 'BarLookAndFeelListItem
End Namespace 'XtraPrintingDemos
